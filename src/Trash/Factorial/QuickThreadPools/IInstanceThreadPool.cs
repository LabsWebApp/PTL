namespace factorial.QuickThreadPools;

public interface IInstanceThreadPool : IDisposable
{
    int MaxConcurrencyLevel { get; }
    ThreadPriority Priority { get; }
    void QueueWorkItem(Action work);
    void QueueWorkItem(Action<object?> work, object? param);
}
