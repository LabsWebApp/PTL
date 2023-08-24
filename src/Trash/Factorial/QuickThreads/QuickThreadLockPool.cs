namespace factorial.QuickThreads;

public class QuickThreadLockPool : IInstanceThreadPool
{
    /// <summary>
    /// Количество потоков в пуле.
    /// </summary>
    public int MaxConcurrencyLevel { get; init; }

    /// <summary>
    /// Приоритет всех потоков, входящих в пул. 
    /// </summary>
    public ThreadPriority Priority { get; init; }

    /// <summary>
    /// Очередь задач.
    /// </summary>
    private Action? _currentWork;
    private readonly object _lock = new();

    /// <summary>
    /// Пул.
    /// </summary>
    private readonly Thread[] _threads;

    /// <summary>
    /// Создаёт новый экземпляр класса
    /// </summary>
    /// <param name="maxConcurrencyLevel">Задаёт количество потоков в пуле - если меньше или равно 0, то число будет равно количеству процессоров.</param>
    /// <param name="priority">Задаёт приоритеты для всех потоков.</param>
    public QuickThreadLockPool(int maxConcurrencyLevel = 0,
        ThreadPriority priority = ThreadPriority.Normal)
    {
        MaxConcurrencyLevel =
            maxConcurrencyLevel <= 0 ? Environment.ProcessorCount : maxConcurrencyLevel;
        Priority = priority;
        _threads = new Thread[MaxConcurrencyLevel];

        for (var i = 0; i < _threads.Length; i++)
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

    /// <summary>
    /// Ставит задачу в очередь.
    /// </summary>
    /// <param name="work">Задача без параметра.</param>
    public void QueueWorkItem(Action? work)
    {
        lock (_lock)
        {
            if (_currentWork == null)
            {
                _currentWork = work;
                Monitor.Pulse(_lock); // Уведомляем ожидающий поток о наличии задачи
            }
        }
    }

    /// <summary>
    /// Ставит задачу в очередь.
    /// </summary>
    /// <param name="work">Задача с параметром.</param>
    /// <param name="param">Параметр задачи.</param>
    void IInstanceThreadPool.QueueWorkItem(Action<object?>? work, object? param) =>
        QueueWorkItem(() => work?.Invoke(param));

    /// <summary>
    /// Процесс выполнения задач из очереди.
    /// </summary>
    private void Working()
    {
        while (true)
        {
            Action? work;
            lock (_lock)
            {
                while (_currentWork == null) // Ожидаем наличия задачи
                    Monitor.Wait(_lock);
                work = _currentWork;
                _currentWork = null; // Задача будет выполнена, можно обнулить
            }
            work?.Invoke(); // Выполняем задачу
        }
    }


    #region Реализация IDisposable
    private volatile bool _isNotDisposed = true;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_isNotDisposed) return;
        _isNotDisposed = false;

        lock (_lock) Monitor.PulseAll(_lock); // Уведомляем все потоки о завершении
        
        foreach (var thread in _threads) thread.Join();
    }
    #endregion
}

