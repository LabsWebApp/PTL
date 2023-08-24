namespace factorial;

/// <summary>
/// Методы, расширяющие тип int.
/// Добавлены методы, возвращающие различными алгоритмами факториал числа.
/// </summary>
public static class FactorialExtensions
{
    /// <summary>
    /// Возвращает факториал числа самым простым алгоритмом, по сути это определение факториала.
    /// </summary>
    /// <param name="n">Любое число, представляющие факториал.</param>
    /// <returns>1, если <paramref name="n" /> меньше 0, или факториал <paramref name="n" />.</returns>
    public static BigInteger Factorial(this int n)
    {
        var result = BigInteger.One;
        for (BigInteger i = 2; i <= n; i++) result *= i;
        return result;
    }

    /// <summary>
    /// Возвращает факториал числа.
    /// Число представляется в виде параллельной последовательности чисел от 2 до <paramref name="n" />,
    /// затем возвращается агрегированное произведение всех элементов.
    /// </summary>
    /// <param name="n">Любое число, представляющие факториал.</param>
    /// <returns>1, если <paramref name="n" /> меньше 0, или факториал <paramref name="n" />.</returns>
    public static BigInteger AsParallelSimpleFactorial(this int n) => n < 2 
        ? BigInteger.One  
        : Enumerable
            .Range(2, n) 
            .AsParallel()   //получаем параллельную последовательность чисел от 2 до n
            .Aggregate(BigInteger.One, (b, i) => b * i);

    /// <summary>
    /// Возвращает факториал числа.
    /// Алгоритм разделяет задачу на <paramref name="works" /> потоков, к-ые перемножают числа с шагом <paramref name="works" />
    /// ({1, 1 + <paramref name="works" />, 1 + <paramref name="works" /> + <paramref name="works" />, ...} {2, ...}),
    /// затем смотрят в коллекцию промежуточных результатов, если она пуста, то добавляют свой результат в коллекцию,
    /// в противном случае забирают первый попавшийся результат и перемножают его на свой, далее повторяет проверку
    /// коллекции и повторяют предыдущие, пока коллекция не будет пуста.
    /// В итоге, когда все потоки закончат свою работу из коллекция генерируется окончательный результат.
    /// </summary>
    /// <param name="n">Любое число, представляющие факториал.</param>
    /// <param name="works">Число потоков.</param>
    /// <returns>1, если <paramref name="n" /> меньше 0, или факториал <paramref name="n" />.</returns>
    public static BigInteger ThreadFactorial(this int n, int works = 0)
    {
        if (n <= 1) return BigInteger.One;
        if (works == 0) works = Environment.ProcessorCount;
        if (works <= 1 || n <= works) return n.Factorial();

        var threads = new Thread[works];

        var results = new ConcurrentBag<BigInteger>();

        for (var i = 0; i < threads.Length; i++)
        {
            var k = i;
            threads[i] = new Thread(() =>
            {
                var res = BigInteger.One;
                for (var j = k + 1; j <= n; j += works) res *= j;
                while (results.TryTake(out var take)) res *= take;
                results.Add(res);
            });
            threads[i].Start();
        }

        foreach (var thread in threads) thread.Join();

        // ВОЗМОЖНО ЛИ ТАКОЕ, results.Count > 1? И ПОЧЕМУ?????????????????????????
        return results
            .AsParallel()
            .Aggregate((i, j) => i * j);
    }

    /// <summary>
    /// Возвращает факториал числа.
    /// Алгоритм разделяет задачу на <paramref name="works" /> к-ые перемножают числа с шагом <paramref name="works" />
    /// ({1, 1 + <paramref name="works" />, 1 + <paramref name="works" /> + <paramref name="works" />, ...} {2, ...}),
    /// затем смотрят в коллекцию промежуточных результатов, если она пуста, то добавляют свой результат в коллекцию,
    /// в противном случае забирают первый попавшийся результат и перемножают его на свой, далее повторяют проверку
    /// коллекции и повторяют предыдущие, пока коллекция не будет пуста.
    /// В итоге, когда все действия закончат свою работу из коллекция генерируется окончательный результат.
    /// </summary>
    /// <param name="n">Любое число, представляющие факториал.</param>
    /// <param name="scheduler">Планировщик задач, используемый для настройки поведения распараллеливания.</param>
    /// <param name="works">Число действий.</param>
    /// <param name="tree">Флаг указывает на то, каким способом будет осуществляться умножение.
    /// true - c помощью деревьев выражений, false - обычным способом ("х * у").</param>
    /// <returns>1, если <paramref name="n" /> меньше 0, или факториал <paramref name="n" />.</returns>
    public static BigInteger ParallelForFactorial(
        this int n, TaskScheduler? scheduler = null, int works = 0, bool tree = false)
    {
        works = works <= 0 ? Environment.ProcessorCount : works;
        if (n <= works) return n.Factorial();

        // для деревьев это разные функции!!!
        Func<BigInteger, int, BigInteger> multiplyBigIntInt = tree ? Multiply : (i, j) => i * j;
        Func<BigInteger, BigInteger, BigInteger> multiplyBigIntBigInt = tree ? Multiply : (i, j) => i * j;

        scheduler ??= TaskScheduler.Default;
        ParallelOptions options = new() { TaskScheduler = scheduler };

        ConcurrentBag<BigInteger> results = new();

        Parallel.For(1, works + 1, options, start =>
        {
            var res = BigInteger.One;
            for (var i = start; i <= n; i += works)
                res = multiplyBigIntInt(res, i);

            while (results.TryTake(out var take)) res = multiplyBigIntBigInt(res, take);

            results.Add(res);
        });

        return results
            .AsParallel()
            .Aggregate((i, j) => multiplyBigIntBigInt(i, j));
    }

    /// <summary>
    /// Возвращает факториал числа.
    /// Алгоритм разделяет задачу на <paramref name="works" /> параллельную коллекцию действий (Action), элементы к-ых перемножают числа с шагом <paramref name="works" />
    /// ({1, 1 + <paramref name="works" />, 1 + <paramref name="works" /> + <paramref name="works" />, ...} {2, ...}),
    /// затем смотрят в коллекцию промежуточных результатов, если она пуста, то добавляют свой результат в коллекцию,
    /// в противном случае забирают первый попавшийся результат и перемножают его на свой, далее повторяют проверку
    /// коллекции и повторяют предыдущие, пока коллекция не будет пуста.
    /// В итоге, когда все действия закончат свою работу из коллекция генерируется окончательный результат.
    /// </summary>
    /// <param name="n">Любое число, представляющие факториал.</param>
    /// <param name="works">Число элементов.</param>
    /// <returns>1, если <paramref name="n" /> меньше 0, или факториал <paramref name="n" />.</returns>
    public static BigInteger AsParallelFactorial(this int n, int works = 0)
    {
        works = works <= 0 ? Environment.ProcessorCount : works;
        if (n <= works) return n.Factorial();

        ConcurrentBag<BigInteger> results = new();

        Enumerable.Range(1, works)
            .AsParallel()
            .ForAll(start =>                    // коллекция действий
            {
                var res = BigInteger.One;
                for (var i = start; i <= n; i += works) res *= i;

                while (results.TryTake(out var take)) res *= take;

                results.Add(res);
            });

        return results
            .AsParallel()
            .Aggregate((i, j) => i * j);
    }

    /// <summary>
    /// Возвращает факториал числа.
    /// Алгоритм разделяет задачу на <paramref name="works" /> асинхронных задач (Task), к-ые перемножают числа с шагом <paramref name="works" />
    /// ({1, 1 + <paramref name="works" />, 1 + <paramref name="works" /> + <paramref name="works" />, ...} {2, ...}),
    /// затем смотрят в коллекцию промежуточных результатов, если она пуста, то добавляют свой результат в коллекцию,
    /// в противном случае забирают первый попавшийся результат и перемножают его на свой, далее повторяют проверку
    /// коллекции и повторяют предыдущие, пока коллекция не будет пуста.
    /// В итоге, когда все задачи закончат свою работу из коллекция генерируется окончательный результат.
    /// </summary>
    /// <param name="n">Любое число, представляющие факториал.</param>
    /// <param name="scheduler">Планировщик задач, используемый для настройки поведения асинхронизации.</param>
    /// <param name="works">Число задач.</param>
    /// <returns>1, если <paramref name="n" /> меньше 0, или факториал <paramref name="n" />.</returns>
    public static BigInteger TasksFactorial(
        this int n, TaskScheduler? scheduler = null, int works = 0)
    {
        scheduler ??= TaskScheduler.Default;
        works = works <= 0 ? Environment.ProcessorCount : works;
        if (n <= works) return n.Factorial();

        var options =
            scheduler == TaskScheduler.Default && works < 1024
                ? TaskCreationOptions.LongRunning
                : TaskCreationOptions.None;

        ConcurrentBag<BigInteger> results = new();

        var tasks = new Task[works];
        for (var i = 0; i < works; i++)
        {
            var res = BigInteger.One;
            var k = i + 1;
            tasks[i] = new Task(() =>
            {
                for (var j = k; j <= n; j += works) res *= j;

                while (results.TryTake(out var take)) res *= take;

                results.Add(res);
            }, options);
            tasks[i].Start(scheduler);
        }

        Task.WaitAll(tasks);

        return results
            .AsParallel()
            .Aggregate((i, j) => i * j);
    }
}