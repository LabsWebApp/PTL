﻿using BenchmarkDotNet.Attributes;
using factorial.QuickThreads;

namespace factorial.Benchmark;

[MemoryDiagnoser]
public class FactorialBenchmark
{
    private const int Works = 1024;

    private readonly MinimalTaskScheduler _scheduler =
        new(new GptChatCorrectedThreadPool(Works, ThreadPriority.Normal));

    [Params( 10_000, 25000, 100_000/*, 500_000*/)]
    public int N;

    [Benchmark(Description = "Default")]
    public BigInteger ParallelForFactorial_DefaultScheduler() => N.ParallelForFactorial(works: Works);

    [Benchmark(Description = "Quick")]
    public BigInteger ParallelForFactorial_CustomScheduler() => N.ParallelForFactorial(_scheduler, Works);
}