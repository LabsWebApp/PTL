CancellationTokenSource cts = new CancellationTokenSource();

Task<int> task = Task.Run(() =>
{
    Thread.Sleep(1000);
    WriteLine("Задача №1 выполнилась");
    return 10;
});

Task<int> c1 = task.ContinueWith(t =>
{
    WriteLine("Задача №2 (продолжение) выполнилась");

    return t.Result * 2;
}, cts.Token);

Task c2 = c1.ContinueWith(t =>
{
    WriteLine("Задача №3 (продолжение продолжения) выполнилась");
    try
    {
        WriteLine($"Результат выполнения - {t.Result}");
    }
    catch (AggregateException e)
    {
        WriteLine($"\tОшибка - {e.InnerException!.GetType()}");
        WriteLine($"\tСообщение об ошибке - {e.InnerException!.Message}");
    }
});

cts.Cancel();

Thread.Sleep(3000);

WriteLine($"\nСтатус задачи №1 - {task.Status}");
WriteLine($"Статус задачи №2 - {c1.Status}");
WriteLine($"Статус задачи №3 - {c2.Status}");

ReadKey();