using System.Diagnostics;
using ManualTaskEx;

int SumNumber(object arg)
{
    var number = (int)arg;
    var sum = 0;
    var sw = Stopwatch.StartNew();
    for (var i = 0; i < number; i++)
    {
        sum += i;
        Thread.Sleep(10);
    }
    sw.Stop();
    WriteLine($"\nDuration: {sw.ElapsedMilliseconds}");
    return sum;
}

var threadPoolWorker = new GetResultWorker<int>(SumNumber!);
threadPoolWorker.Start(1000);

while (threadPoolWorker.Completed == false)
{
    Write("*");
    Thread.Sleep(35);
}

WriteLine();
WriteLine($"Результат асинхронной операции = {threadPoolWorker.Result}");

ReadKey();