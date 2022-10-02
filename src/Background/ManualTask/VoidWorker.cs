namespace ManualTask;

internal class VoidWorker
{
    private readonly Action<object?> _action;
    private const int Tick = 100;

    public VoidWorker(Action<object?> action) =>
        _action = action ?? throw new ArgumentNullException(nameof(action));

    public bool Success { get; private set; }
    public bool Completed { get; private set; }
    public bool IsRunning { get; private set; }
    public Exception? Exception { get; private set; }

    public void Start(object? state = default) =>
        ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadExecution!), state);

    public void Wait()
    {
        if (!IsRunning) throw new Exception("Action is not running.");

        while (Completed == false) Thread.Sleep(Tick);

        if (Exception != null) throw Exception;
    }

    private void ThreadExecution(object? state)
    {
        try
        {
            IsRunning = true;
            _action.Invoke(state);
            Success = true;
        }
        catch (Exception ex)
        {
            Exception = ex;
            Success = false;
        }
        finally
        {
            Completed = true;
            IsRunning = false;
        }
    }
}