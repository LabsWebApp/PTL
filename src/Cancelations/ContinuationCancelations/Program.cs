CancellationTokenSource cts = new CancellationTokenSource();

Task<int> task = Task.Run(() =>
{
    Thread.Sleep(1000);
    WriteLine("Задача №1 выполнилась");
    return 10;
}, cts.Token);

Task c1 = task.ContinueWith(t =>
{
    WriteLine("Задача №2 (продолжение) выполнилась");

    if (!t.IsFaulted && !t.IsCanceled) 
        WriteLine($"Результат = {t.Result}");
});

cts.Cancel();

Thread.Sleep(3000);

WriteLine($"Статус задачи №1 - {task.Status}");
WriteLine($"Статус задачи №2 - {c1.Status}");

ReadKey();