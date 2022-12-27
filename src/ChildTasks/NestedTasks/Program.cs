// Вложенные задачи
Task parent = new Task(() =>
{
    Task.Run(() =>
    {
        Thread.Sleep(1000);
        WriteLine("Nested 1 completed.");
    });

    Task.Run(() =>
    {
        Thread.Sleep(2000);
        WriteLine("Nested 2 completed.");
    });

    Thread.Sleep(100);
});

parent.Start();
parent.Wait();
WriteLine("parent completed.");

ReadKey();