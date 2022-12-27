using System.ComponentModel.DataAnnotations;

namespace LimitedConcurrencyTaskSchedulers;

public class LimitedConcurrencyTaskScheduler : TaskScheduler
{
    [ThreadStatic]
    private static bool _currentThreadIsProcessingItems;

    private readonly LinkedList<Task> _tasksList = new ();
    private readonly int _concurrencyLevel;

    private int _runningTasks = 0;

    public LimitedConcurrencyTaskScheduler(int concurrencyLevel) =>
        _concurrencyLevel = concurrencyLevel < 1 ? 1 : concurrencyLevel;

    /// <summary>
    /// виртуальный getter, по умолчанию: int.MaxValue
    /// </summary>
    public sealed override int MaximumConcurrencyLevel => _concurrencyLevel;

    protected override IEnumerable<Task>? GetScheduledTasks()
    {
        lock (_tasksList)
        {
            return _tasksList;
        }
    }

    protected override void QueueTask(Task task)
    {
        lock (_tasksList)
        {
            _tasksList.AddLast(task);

            if (_runningTasks < _concurrencyLevel)
            {
                ++_runningTasks;
                NotifyThreadPoolOfPendingWork();
            }
        }
    }

    private void NotifyThreadPoolOfPendingWork() =>
        ThreadPool.QueueUserWorkItem(_ =>
        {
            _currentThreadIsProcessingItems = true;
            try
            {
                while (true)
                {
                    Task task;
                    lock (_tasksList)
                    {
                        if (_tasksList.Count == 0)
                        {
                            --_runningTasks;
                            break;
                        }
                        task = _tasksList.First!.Value;
                        _tasksList.RemoveFirst();
                    }
                    TryExecuteTask(task);
                }
            }
            finally { _currentThreadIsProcessingItems = false; }
        }, null);

    protected sealed override bool TryDequeue(Task task)
    {
        lock (_tasksList)
        {
            return _tasksList.Remove(task);
        }
    }

    protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
    {
        // Не поддерживается синхронный вызов при бездействии
        // для сохранения контроля над кол-ом одновременных задач
        if (_currentThreadIsProcessingItems == false) return false;

        if (taskWasPreviouslyQueued == true) TryDequeue(task);

        return TryExecuteTask(task);
    }
}