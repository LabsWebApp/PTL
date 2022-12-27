// планировщик с ограничением ||-зма

using LimitedConcurrencyTaskSchedulers;

void ShowThreadPoolInfo(object? _)
{
    ThreadPool.GetAvailableThreads(out int threads, out int completionPorts);
    ThreadPool.GetMaxThreads(out int maxThreads, out int maxCompletionPorts);
    WriteLine($"{new string(' ', 20)}Worker Threads - [{threads}:{maxThreads}]");
    //WriteLine($"{new string(' ', 20)}Completion Ports - [{completionPorts}:{maxCompletionPorts}]");
}

WriteLine($"Поток метода Main - {Thread.CurrentThread.ManagedThreadId}.");
//Timer timer = new Timer(ShowThreadPoolInfo!, null, 500, 3000);

var limit = 1; //Environment.ProcessorCount;
TaskScheduler scheduler = new LimitedConcurrencyTaskScheduler(limit);

WriteLine($"TaskScheduler - {scheduler.GetType()}");
Task[] tasks = new Task[30];
for (int i = 0; i < 30; i++)
{
    tasks[i] = new Task(() =>
    {
        ShowThreadPoolInfo(null);
        Thread.Sleep(3000);
        WriteLine($"Выполнена задача #{Task.CurrentId} в потоке {Thread.CurrentThread.ManagedThreadId} из пула потоков - {Thread.CurrentThread.IsThreadPoolThread}");
        ShowThreadPoolInfo(null);
    });

    tasks[i].Start(scheduler);
}

Task.WaitAll(tasks);
Thread.Sleep(3001);
//timer.Dispose();
WriteLine("The End");

ReadKey();