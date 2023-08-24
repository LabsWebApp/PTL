using BenchmarkDotNet.Attributes;

namespace factorial.Benchmark;

[MemoryDiagnoser]
public class MultiplyBenchmark
{
    private readonly Random _random = new();
    private BigInteger[]? _bigIntegerValues;
    private int[]? _intValues;

    [Params(1000, 10000, 100000)]
    public int N;

    [GlobalSetup]
    public void GlobalSetup()
    {
        _bigIntegerValues = new BigInteger[N];
        _intValues = new int[N];

        for (var i = 0; i < N; i++)
        {
            _bigIntegerValues[i] = new BigInteger(_random.Next(1, 1000));
            _intValues[i] = _random.Next(1, 1000);
        }
    }

    [Benchmark(Description = "tree")]
    public void MultiplyUsingFuncTest()
    {
        for (var i = 0; i < N; i++)
        {
#pragma warning disable IDE0059
            var result = Multiply(_bigIntegerValues![i], _intValues![i]);
#pragma warning restore IDE0059
        }
    }

    [Benchmark(Description = "x * y")]
    public void MultiplyUsingStandardTest()
    {
        for (var i = 0; i < N; i++)
        {
#pragma warning disable IDE0059
            var result = _bigIntegerValues![i] * _intValues![i];
#pragma warning restore IDE0059
        }
    }

}