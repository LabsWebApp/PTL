using System.Collections.Concurrent;
using System.Numerics;

namespace factorial;

public static class FactorialExtensions
{
    public static BigInteger Factorial(this int n)
    {
        var result = BigInteger.One;
        for (BigInteger i = 2; i <= n; i++) result *= i;
        return result;
    }

    public static BigInteger AsParallelSimpleFactorial(this int n) => n < 2 
        ? BigInteger.One  
        : Enumerable
            .Range(1, n)
            .AsParallel()
            .Aggregate(BigInteger.One, (b, i) => b * i);

    public static BigInteger ThreadFactorial(this int n, int proc = 0)
    {
        if (n <= 1) return BigInteger.One;
        if (proc == 0) proc = Environment.ProcessorCount;
        if (proc <= 1 || n <= proc) return n.Factorial();

        Thread[] threads = new Thread[proc];

        var results = new ConcurrentBag<BigInteger>();

        for (int i = 0; i < threads.Length; i++)
        {
            var k = i;
            threads[i] = new Thread(() =>
            {
                var res = BigInteger.One;
                for (int j = k + 1; j <= n; j += proc) res *= j;
                while (results.TryTake(out var take)) res *= take;
                results.Add(res);
            });
            threads[i].Start();
        }

        foreach (var thread in threads) thread.Join();

        return results
            .AsParallel()
            .Aggregate((i, j) => i * j);
    }

    public static BigInteger ParallelForFactorial(
        this int x, TaskScheduler? scheduler = null, int processes = 0)
    {
        processes = processes <= 0 ? Environment.ProcessorCount : processes;
        if (x <= processes) return x.Factorial();

        scheduler ??= TaskScheduler.Default;
        ParallelOptions options = new ParallelOptions { TaskScheduler = scheduler };

        ConcurrentBag<BigInteger> results = new();

        Parallel.For(1, processes + 1, options, start =>
        {
            var res = BigInteger.One;
            for (var i = start; i <= x; i += processes)
                res *= i;

            while (results.TryTake(out var take)) res *= take;

            results.Add(res); ;
        });

        return results
            .AsParallel()
            .Aggregate((i, j) => i * j);
    }

    public static BigInteger AsParallelFactorial(this int x, int processes = 0)
    {
        processes = processes <= 0 ? Environment.ProcessorCount : processes;
        if (x <= processes) return x.Factorial();

        ConcurrentBag<BigInteger> results = new();

        Enumerable.Range(1, processes)
            .AsParallel()
            .ForAll(start =>
            {
                var res = BigInteger.One;
                for (var i = start; i <= x; i += processes) res *= i;

                while (results.TryTake(out var take)) res *= take;

                results.Add(res); ;
            });

        return results
            .AsParallel()
            .Aggregate((i, j) => i * j);
    }

    public static BigInteger TasksFactorial(
        this int x, TaskScheduler? scheduler = null, int processes = 0)
    {
        scheduler ??= TaskScheduler.Default;
        processes = processes <= 0 ? Environment.ProcessorCount : processes;
        if (x <= processes) return x.Factorial();

        var options =
            scheduler == TaskScheduler.Default && processes < 1024
                ? TaskCreationOptions.LongRunning
                : TaskCreationOptions.None;

        ConcurrentBag<BigInteger> results = new();

        Task[] tasks = new Task[processes];
        for (var i = 0; i < processes; i++)
        {
            var res = BigInteger.One;
            var k = i + 1;
            tasks[i] = new Task(() =>
            {
                for (var j = k; j <= x; j += processes) res *= j;

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