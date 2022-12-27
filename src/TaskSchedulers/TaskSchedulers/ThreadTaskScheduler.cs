namespace ThreadTaskSchedulers;

public class ThreadTaskScheduler : TaskScheduler
{
    protected override IEnumerable<Task>? GetScheduledTasks() => Enumerable.Empty<Task>();

    protected override void QueueTask(Task task) =>
        new Thread(() => TryExecuteTask(task)).Start();

    protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued) =>
        TryExecuteTask(task);
}

public class ThreadPoolTaskScheduler : TaskScheduler
{
    protected override IEnumerable<Task>? GetScheduledTasks() => Enumerable.Empty<Task>();

    protected override void QueueTask(Task task) =>
        ThreadPool.QueueUserWorkItem(_ => TryExecuteTask(task), null);

    protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued) =>
        TryExecuteTask(task);
}




















//protected override IEnumerable<Task>? GetScheduledTasks() => Enumerable.Empty<Task>();

//protected override void QueueTask(Task task) =>
//    new Thread(() => TryExecuteTask(task)) { IsBackground = true }.Start();

//protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued) =>
//    TryExecuteTask(task);