namespace factorial.QuickThreadPools;

public class QuickThreadPoolTaskScheduler : TaskScheduler, IDisposable
{
    public override int MaximumConcurrencyLevel => _pool.MaxConcurrencyLevel;

    private readonly IInstanceThreadPool _pool;

    public QuickThreadPoolTaskScheduler(
        int maxConcurrencyLevel = 0, 
        ThreadPriority priority = ThreadPriority.Normal) =>
        _pool = new QuickThreadPool(maxConcurrencyLevel, priority);

    protected override IEnumerable<Task> GetScheduledTasks() => Enumerable.Empty<Task>();

    protected override void QueueTask(Task task) => _pool.QueueWorkItem(() => TryExecuteTask(task));

    protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued) =>
        TryExecuteTask(task);

    public void Dispose() => _pool.Dispose();
}