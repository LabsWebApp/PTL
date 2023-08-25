#pragma warning disable VSSpell001
namespace factorial.QuickThreads;

public class GptChatCorrectedThreadPool : IInstanceThreadPool
{
    public int MaxConcurrencyLevel { get; }
    public ThreadPriority Priority { get; }

    private readonly object _lock = new();
    private readonly ConcurrentBag<Action?> _works = new();
    private readonly Thread[] _threads;
    private volatile bool _isDisposed;

    public GptChatCorrectedThreadPool(int maxConcurrencyLevel = 0, ThreadPriority priority = ThreadPriority.Normal)
    {
        MaxConcurrencyLevel = maxConcurrencyLevel <= 0 ? Environment.ProcessorCount : maxConcurrencyLevel;
        Priority = priority;

        _threads = new Thread[MaxConcurrencyLevel];
        for (var i = 0; i < MaxConcurrencyLevel; i++)
        {
            var thread = new Thread(Working)
            {
                IsBackground = true,
                Priority = priority
            };
            _threads[i] = thread;
            thread.Start();
        }
    }

    public void QueueWorkItem(Action? work)
    {
        lock (_lock)
        {
            _works.Add(work);
            Monitor.Pulse(_lock); // Уведомляем ожидающий поток о наличии задачи
        }
    }

    void IInstanceThreadPool.QueueWorkItem(Action<object?>? work, object? param) =>
        QueueWorkItem(() => work?.Invoke(param));

    private void Working()
    {
        while (!_isDisposed)
        {
            Action? work = null;
            lock (_lock)
            {
                while (!_isDisposed && _works.IsEmpty)
                    Monitor.Wait(_lock);

                if (!_works.IsEmpty)
                    _works.TryTake(out work);
            }

            work?.Invoke();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_isDisposed)
            return;

        _isDisposed = true;

        lock (_lock) Monitor.PulseAll(_lock); // Уведомляем все потоки о завершении

        foreach (var thread in _threads) thread.Join();
    }
}
