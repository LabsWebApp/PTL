using System;
using System.Threading;
using System.Threading.Tasks;

namespace Progress;

class Operation
{
    internal Task<int> SumNumbersAsync(int[] numbers,
        CancellationToken cancellationToken = default,
        IProgress<int>? progress = null) => Task.Run(() =>
            Sum(numbers, cancellationToken, progress), cancellationToken);

    private int Sum(int[] numbers, 
        CancellationToken cancellationToken, 
        IProgress<int>? progress)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var sum = 0;
        for (int i = 0, j = 1; i < numbers.Length; i++, j++)
        {
            Thread.Sleep(1000);
            sum += numbers[i];

            cancellationToken.ThrowIfCancellationRequested();
            progress?.Report(j * 100 / numbers.Length);
        }
        return sum;
    }
}