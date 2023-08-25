namespace factorial.QuickThreads;

/// <summary>
/// Пользовательский пул потоков.
/// </summary>
public class QuickThreadPool : IInstanceThreadPool
{
    /// <summary>
    /// Количество потоков в пуле.
    /// </summary>
    public int MaxConcurrencyLevel { get; }

    /// <summary>
    /// Приоритет всех потоков, входящих в пул. 
    /// </summary>
    public ThreadPriority Priority { get; }

    /// <summary>
    /// Очередь задач.
    /// </summary>
    private readonly BlockingCollection<Action?> _works = new(); // тормоз тут???

    /// <summary>
    /// Пул.
    /// </summary>
    private readonly Thread[] _threads;

    /// <summary>
    /// Создаёт новый экземпляр класса
    /// </summary>
    /// <param name="maxConcurrencyLevel">Задаёт количество потоков в пуле - если меньше или равно 0, то число будет равно количеству процессоров.</param>
    /// <param name="priority">Задаёт приоритеты для всех потоков.</param>
    public QuickThreadPool(int maxConcurrencyLevel = 0, 
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
    public void QueueWorkItem(Action? work) => _works.Add(work);

    /// <summary>
    /// Ставит задачу в очередь.
    /// </summary>
    /// <param name="work">Задача с параметром.</param>
    /// <param name="param">Параметр задачи.</param>
    void IInstanceThreadPool.QueueWorkItem(Action<object?> work, object? param) =>
        QueueWorkItem(() => work.Invoke(param));

    /// <summary>
    /// Процесс выполнения задач из очереди.
    /// </summary>
    private void Working()
    {
        foreach (var work in _works.GetConsumingEnumerable()) work?.Invoke();
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

        _works.CompleteAdding(); // блокировка очереди
        foreach (var thread in _threads) thread.Join();
    }
    #endregion
}