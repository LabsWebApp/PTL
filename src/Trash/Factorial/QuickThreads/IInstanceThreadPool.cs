namespace factorial.QuickThreads;

/// <summary>
/// Интерфейс, связывающий пулы потоков, к-ые используются при построении MinimalTaskScheduler
/// </summary>
public interface IInstanceThreadPool : IDisposable
{
    int MaxConcurrencyLevel { get; }
    ThreadPriority Priority { get; }
    void QueueWorkItem(Action? work);
    void QueueWorkItem(Action<object?> work, object? param);
}
