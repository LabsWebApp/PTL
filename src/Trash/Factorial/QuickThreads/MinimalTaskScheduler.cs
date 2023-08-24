namespace factorial.QuickThreads;

/// <summary>
/// Облегчённый планировщик задач
/// </summary>
public class MinimalTaskScheduler : TaskScheduler
{
    public override int MaximumConcurrencyLevel => _pool.MaxConcurrencyLevel;

    private readonly IInstanceThreadPool _pool;

    /// <summary>
    /// Создаёт новый экземпляр класса
    /// </summary>
    /// <param name="pool">Пул потоков, который будет использовать планировщик задач.</param>
    public MinimalTaskScheduler(IInstanceThreadPool pool) => _pool = pool;

    protected override IEnumerable<Task> GetScheduledTasks() => Enumerable.Empty<Task>();

    protected override void QueueTask(Task task) => _pool.QueueWorkItem(() => TryExecuteTask(task));

    protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued) => TryExecuteTask(task);
}