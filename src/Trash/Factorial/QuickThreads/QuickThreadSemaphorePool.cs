namespace factorial.QuickThreads;

public class QuickThreadSemaphorePool : IInstanceThreadPool
{
    public int MaxConcurrencyLevel { get; }
    public ThreadPriority Priority { get; }

    private readonly Thread[] _threads;
    private readonly ConcurrentQueue<Action> _tasks = new();
    private readonly SemaphoreSlim _semaphore;
    private volatile bool _shutdown;

    public QuickThreadSemaphorePool(int maxConcurrencyLevel = 0, ThreadPriority priority = ThreadPriority.Normal)
    {
        MaxConcurrencyLevel = maxConcurrencyLevel <= 0 ? Environment.ProcessorCount : maxConcurrencyLevel;
        Priority = priority;
        _semaphore = new SemaphoreSlim(0);

        _threads = new Thread[MaxConcurrencyLevel];
        for (var i = 0; i < _threads.Length; i++)
        {
            _threads[i] = new Thread(Worker)
            {
                IsBackground = true,
                Priority = priority
            };
            _threads[i].Start();
        }
    }

    public void QueueWorkItem(Action? work)
    {
        if (_shutdown)
            throw new InvalidOperationException("ThreadPool is shutting down");

        if (work != null)
        {
            _tasks.Enqueue(work);
            _semaphore.Release();
        }
    }

    public void QueueWorkItem(Action<object?> work, object? param) => QueueWorkItem(() => work.Invoke(param));

    public void Dispose()
    {
        Shutdown();
        GC.SuppressFinalize(this);
    }

    private void Worker()
    {
        while (!_shutdown)
        {
            _semaphore.Wait();
            if (_tasks.TryDequeue(out var work))
            {
                work();
            }
        }
    }

    public void Shutdown()
    {
        _shutdown = true;
        _semaphore.Release(_threads.Length);
        foreach (var thread in _threads)
        {
            thread.Join();
        }
    }
}

