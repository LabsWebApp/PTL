using TaskSchedulerFunctionality;
#pragma warning disable CS8321

void QueueTaskTesting(IList<Task> tasks, TaskScheduler scheduler)
{
    for (int i = 0; i < tasks.Count; i++)
    {
        tasks[i] = new Task(() =>
        {
            Thread.Sleep(2000);
            WriteLine($"Задача {Task.CurrentId} выполнилась в потоке {Thread.CurrentThread.ManagedThreadId}\n");
        });
        Thread.Sleep(100);
        tasks[i].Start(scheduler);
    }
}

void TryExecuteTaskInlineTesting(IList<Task> tasks, TaskScheduler scheduler)
{
    for (int i = 0; i < tasks.Count; i++)
    {
        tasks[i] = new Task<int>(() =>
        {
            Thread.Sleep(2000);
            WriteLine($"Задача {Task.CurrentId} выполнилась в потоке {Thread.CurrentThread.ManagedThreadId}\n");
            return 1;
        });
    }

    foreach (var task in tasks)
    {
        task.Start(scheduler);
        task.Wait();
        //var result = ((Task<int>)task).Result;
    }
}

void TryDequeueTesting(IList<Task> tasks, TaskScheduler scheduler)
{
    #region Скоординированная отмена

    CancellationTokenSource cts = new CancellationTokenSource();
    CancellationToken token = cts.Token;

    cts.CancelAfter(500);

    #endregion

    for (int i = 0; i < tasks.Count; i++)
    {
        tasks[i] = new Task(() =>
        {
            Thread.Sleep(2000);
            if (token.IsCancellationRequested) return;
            WriteLine($"Задача {Task.CurrentId} выполнилась в потоке {Thread.CurrentThread.ManagedThreadId}\n");
        }, token);
        //Thread.Sleep(1);
        tasks[i].Start(scheduler);
    }
}

WriteLine($"Id потока метода Main - {Thread.CurrentThread.ManagedThreadId}");

Task[] tasks = new Task[10];
ReviewTaskScheduler reviewTaskScheduler = new ReviewTaskScheduler();

//QueueTaskTesting(tasks, reviewTaskScheduler); // + убрать пул потоков
//TryExecuteTaskInlineTesting(tasks, reviewTaskScheduler); // добавить ожидание + wait -> Result
TryDequeueTesting(tasks, reviewTaskScheduler);

try
{
    Task.WaitAll(tasks);
}
catch
{
    BackgroundColor = ConsoleColor.Red;
    ForegroundColor = ConsoleColor.White;
    WriteLine("Несколько задач были отменены!");
    ResetColor();
}
finally
{
    WriteLine("Все задачи закончили свое выполнение");
}

ReadKey();