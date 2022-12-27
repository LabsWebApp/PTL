//максимально минимальный планировщик 
using ThreadTaskSchedulers;
//long globals = 0;

//Parallel.For(0, (long)int.MaxValue * 3,
//    new ParallelOptions { TaskScheduler = new ThreadTaskScheduler() },
//    _ => Interlocked.Increment(ref globals));
//WriteLine($"Max = {(long)int.MaxValue * 3}\nglobals = {globals}");
//ReadKey();

Random r = new Random();
void ShowThreadPoolInfo(object? _)
{
    ThreadPool.GetAvailableThreads(out int threads, out int completionPorts);
    ThreadPool.GetMaxThreads(out int maxThreads, out int maxCompletionPorts);
    WriteLine($"{new string(' ', 20)}Worker Threads - [{threads}:{maxThreads}]");
    //WriteLine($"{new string(' ', 20)}Completion Ports - [{completionPorts}:{maxCompletionPorts}]");
}

WriteLine($"Поток метода Main - {Thread.CurrentThread.ManagedThreadId}.");
//ShowThreadPoolInfo(null);
Timer timer = new Timer(ShowThreadPoolInfo!, null, 500, 1000);


TaskScheduler scheduler;
//scheduler = TaskScheduler.Default;
scheduler = new ThreadTaskScheduler();

WriteLine($"TaskScheduler - {scheduler.GetType()}");
Task[] tasks = new Task[30];
for (int i = 0; i < 30; i++)
{
    tasks[i] = new Task(() =>
    {
        Thread.Sleep(r.Next(900, 3000));
        WriteLine($"Выполнена задача #{Task.CurrentId} в потоке {Thread.CurrentThread.ManagedThreadId} из пула потоков - {Thread.CurrentThread.IsThreadPoolThread}");
    });

    tasks[i].Start(scheduler);
}

Task.WaitAll(tasks);
Thread.Sleep(1001);
timer.Dispose();
ShowThreadPoolInfo(null);
WriteLine("The End");

ReadKey();