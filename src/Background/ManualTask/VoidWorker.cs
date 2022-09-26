namespace ManualTask;

internal class VoidWorker
{
    private readonly Action<object?> _action;
    private const int Tick = 100;

    public VoidWorker(Action<object?> action) =>
        _action = action ?? throw new ArgumentNullException(nameof(action));

    public bool Success { get; private set; }
    public bool Completed { get; private set; }
    public Exception? Exception { get; private set; }

    public void Wait()
    {
        while (Completed == false) Thread.Sleep(Tick);

        if (Exception != null) throw Exception;
    }

    public void Start(object state = default) =>
        ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadExecution!), state);

    private void ThreadExecution(object? state)
    {
        try
        {
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
        }
    }
}