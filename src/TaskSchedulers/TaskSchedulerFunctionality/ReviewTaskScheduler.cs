namespace TaskSchedulerFunctionality;

internal class ReviewTaskScheduler : TaskScheduler
{
    private readonly LinkedList<Task> _tasksList = new();

    protected override IEnumerable<Task> GetScheduledTasks()
    {
        lock (_tasksList)
        {
            return _tasksList;
        }
    }


    /// <summary>
    /// Метод вызывается методом Start класса Task
    /// </summary>
    /// <param name="task"></param>
    protected override void QueueTask(Task task)
    {
        WriteLine($"    [QueueTask] Задача #{task.Id} поставлена в очередь..");
        lock (_tasksList)
        {
            Thread.Sleep(1);
            _tasksList.AddLast(task);
        }

        ThreadPool.QueueUserWorkItem(ExecuteTasks!, null);
        //ExecuteTasks(null);
    }

    private void ExecuteTasks(object? _)
    {
        while (true)
        {
            //Thread.Sleep(2000); // Убрать комментарий для проверки TryExecuteTaskInline
            Task? task = null;

            lock (_tasksList)
            {
                if (_tasksList.Count == 0) break;

                task = _tasksList.First?.Value;
                if (_tasksList.Count > 0) _tasksList.RemoveFirst();
            }

            if (task == null) break;

            TryExecuteTask(task);
        }
    }

    /// <summary>
    /// Метод вызывается методами ожидания, к примеру Wait, WaitAll, Result, RunSynchronously...
    /// для чего?
    /// </summary>
    /// <param name="task"></param>
    /// <param name="taskWasPreviouslyQueued"></param>
    /// <returns></returns>
    protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
    {
        WriteLine($"        [TryExecuteTaskInline] Попытка выполнить задачу #{task.Id} синхронно..");
        lock (_tasksList)
        {
            _tasksList.Remove(task);
        }

        return TryExecuteTask(task);
    }

    /// <summary>
    /// Метод вызывается при отмене выполнения задачи
    /// </summary>
    /// <param name="task"></param>
    /// <returns></returns>
    protected override bool TryDequeue(Task task)
    {
        WriteLine($"            [TryDequeue] Попытка удалить задачу {task.Id} из очереди..");
        bool result = false;

        lock (_tasksList)
        {
            result = _tasksList.Remove(task);
        }

        if (result)
        {
            WriteLine($"                [TryDequeue] Задача {task.Id} была удалена из очереди на выполнение..");
        }

        return result;
    }
}