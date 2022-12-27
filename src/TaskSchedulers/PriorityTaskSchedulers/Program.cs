using PriorityTaskSchedulers;


WriteLine($"Поток метода Main - {Thread.CurrentThread.ManagedThreadId}.");

PriorityTaskScheduler scheduler = new PriorityTaskScheduler();

Task[] tasks = new Task[30];

for (int i = 0; i < 30; i++)
{
    tasks[i] = new Task(() =>
    {
        WriteLine($"Запустилась задача {Task.CurrentId} в потоке {Thread.CurrentThread.ManagedThreadId}");
        Thread.Sleep(5000);
        WriteLine($"{new string(' ', 10)}Выполнена задача {Task.CurrentId} в потоке {Thread.CurrentThread.ManagedThreadId}");
    });
    tasks[i].Start(scheduler);
}

Task lowPriorityTask = new Task(() =>
{
    WriteLine($"НИЗКО-ПРИОРИТЕТНАЯ {Task.CurrentId} задача запустилась в потоке {Thread.CurrentThread.ManagedThreadId}");
    Thread.Sleep(1000);
    WriteLine($"{new string(' ', 20)}НИЗКО-ПРИОРИТЕТНАЯ {Task.CurrentId} задача выполнилась в потоке {Thread.CurrentThread.ManagedThreadId}");
});

lowPriorityTask.Start(scheduler);
scheduler.Deprioritize(lowPriorityTask);

WriteLine("Высоко-приоритетные задачи начались позже, но выполнятся первее:");

for (int i = 0; i < 15; i++)
{
    Task task = new Task(() =>
    {
        WriteLine($"ПРИОРИТЕТНАЯ задача {Task.CurrentId} запустилась потоке - {Thread.CurrentThread.ManagedThreadId}");
        Thread.Sleep(5000);
        WriteLine($"{new string(' ', 20)}ПРИОРИТЕТНАЯ задача {Task.CurrentId} выполнилась в потоке - {Thread.CurrentThread.ManagedThreadId}");
    });

    task.Start(scheduler);
    scheduler.Prioritize(task);
}

Task.WaitAll(tasks);
Thread.Sleep(2000);

WriteLine("The End");

ReadKey();