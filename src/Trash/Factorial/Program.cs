// Ignore Spelling: Acc

using BenchmarkDotNet.Running;
using factorial;
using factorial.Benchmark;
using factorial.QuickThreads;

// Почему нет расширения, 
// Иллюстрация того, что c# не поддерживает оптимизацию хвостовой рекурсии (Tail Call):

//BigInteger Factorial(BigInteger i) => i <= BigInteger.One
//    ? BigInteger.One
//    : i * Factorial(i - BigInteger.One);


//BigInteger FactorialTail(BigInteger i)
//{
//    BigInteger FactorialAcc(BigInteger acc, BigInteger n) => n <= BigInteger.One
//        ? acc
//        : FactorialAcc(n * acc, n - BigInteger.One);

//    return FactorialAcc(BigInteger.One, i);
//}

//BigInteger test = 4600;
//WriteLine(FactorialTail(test));
//WriteLine(Factorial(test));

//ReadKey();

Stopwatch stopwatch = new();

IInstanceThreadPool? pool = null;

while (true)
{
    WriteLine("Выберете метод вычисления факториала:");

    WriteLine("1 - ThreadFactorial");
    WriteLine("2 - ParallelForFactorial");
    WriteLine("3 - ParallelForFactorial with QuickThreadPoolTaskScheduler");
    WriteLine("4 - AsParallelFactorial");
    WriteLine("5 - TasksFactorial");
    WriteLine("6 - TasksFactorial with QuickThreadPoolTaskScheduler");
    WriteLine("7 - AsParallelSimpleFactorial");
    WriteLine("8 - ParallelForFactorial with QuickThreadPoolTaskScheduler and Expression Tree (???БОМБА???)");
    WriteLine("любая клавиша - Factorial ('Esc' завершить работу)");
    
    var key = ReadKey();
    if (key.Key == ConsoleKey.Escape) break;
    if (!int.TryParse(new[] { key.KeyChar }, out var mode) || mode < 1) mode = 0;
    Write("\b");

    var works = Environment.ProcessorCount;
    if (mode != 0)
    {
        WriteLine($"Разбить решения на 1024 подзадачи нажмите: 'Y', по умолчанию ({works}) любая клавиша.");
        if (ReadKey().Key == ConsoleKey.Y) works = 1024;
        Write("\b");
    }

    var info =
        $"{new string('-', 4)} Выбранный метод: " +
        $"{mode switch
        {
            1 => "ThreadFactorial",
            2 => "ParallelForFactorial",
            3 => "ParallelForFactorial with QuickThreadPoolTaskScheduler",
            4 => "AsParallelFactorial",
            5 => "TasksFactorial",
            6 => "TasksFactorial with QuickThreadPoolTaskScheduler",
            7 => "AsParallelSimpleFactorial",
            8 => "ParallelForFactorial with QuickThreadPoolTaskScheduler and Expression Tree",
            _ => "Factorial"
        }}";
    if (mode != 0) info += $"; works = {works}";

    WriteLine($"{info}\nВведите N: ");
    var nString = ReadLine();
    if (!int.TryParse(nString?.Replace("_", string.Empty), out var n))
    {
        WriteLine("Error!");
        continue;
    }

    CursorVisible = false;
    SetCursorPosition(nString.TrimEnd().Length, GetCursorPosition().Top - 1);

    MinimalTaskScheduler? scheduler = null;
    if ((mode is 3 or 6 or 8) && (pool is null || pool.MaxConcurrencyLevel != works))
    {
        pool = new GptChatCorrectedThreadPool(works, ThreadPriority.Lowest);
        scheduler = new MinimalTaskScheduler(pool);
    }

    stopwatch.Restart();
    var result = mode switch
    {
        1 => n.ThreadFactorial(works),
        2 or 3 => n.ParallelForFactorial(scheduler, works),
        4 => n.AsParallelFactorial(works: works),
        5 or 6 => n.TasksFactorial(scheduler, works),
        7 => n.AsParallelSimpleFactorial(),
        8 => n.ParallelForFactorial(scheduler, works, true),
        _ => n.Factorial()
    };
    stopwatch.Stop();

    Beep();

    Write($"! = {(n <= 100 ? result.ToString() : result.GetByteCount() + " bytes")}\n");
    WriteLine(info + $"\n\t... выполнил расчёт за {stopwatch.Elapsed}");

    GC.Collect();
    WriteLine("*****");
    CursorVisible = true;
}

pool?.Dispose();

Clear();


WriteLine("Test");

//BenchmarkRunner.Run<MultiplyBenchmark>();
BenchmarkRunner.Run<FactorialBenchmark>();

ReadKey();