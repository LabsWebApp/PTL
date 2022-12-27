using System.Collections.Concurrent;

namespace factorial.QuickThreadPools;

public class QuickThreadPool : IInstanceThreadPool
{
    public int MaxConcurrencyLevel { get; init; }
    public ThreadPriority Priority { get; init; }

    private readonly AutoResetEvent _workingEvent = new(false);
    private readonly ConcurrentBag<Action> _works = new();

    private readonly Thread[] _threads;

    public QuickThreadPool(int maxConcurrencyLevel = 0, 
        ThreadPriority priority = ThreadPriority.Normal)
    {
        MaxConcurrencyLevel = 
            maxConcurrencyLevel <= 0 ? Environment.ProcessorCount : maxConcurrencyLevel;
        Priority = priority;
        _threads = new Thread[MaxConcurrencyLevel];

        for (var i = 0; i < _threads.Length; i++)
        {
            var thread = new Thread(ThreadRunning)
            {
                IsBackground = true,
                Priority = priority
            };
            _threads[i] = thread;
            thread.Start();
        }
    }

    public void QueueWorkItem(Action work)
    {
        _works.Add(work);
        _workingEvent.Set();
    }

    void IInstanceThreadPool.QueueWorkItem(Action<object?> work, object? param) =>
        QueueWorkItem(() => work.Invoke(param));

    private void ThreadRunning()
    {
        while (_isNotDisposed)
        {
            _workingEvent.WaitOne();
            if (_works.TryTake(out var work)) work.Invoke();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_isNotDisposed) return;
        _isNotDisposed = false;
        do
        {
            Parallel.For(0, _threads.Length, _ => QueueWorkItem(() =>
            {
                Thread.Sleep(0);
            }));
        } while (_threads.Any(t => t.IsAlive));

        _workingEvent.Dispose();
    }

    private volatile bool _isNotDisposed = true;
}