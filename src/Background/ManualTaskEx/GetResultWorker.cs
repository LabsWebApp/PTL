namespace ManualTaskEx;

internal class GetResultWorker<TResult>
{
    private const int Tick = 100;
    private readonly Func<object?, TResult> _func;
#pragma warning disable CS8601
    private TResult _result = default;
#pragma warning restore CS8601

    public GetResultWorker(Func<object?, TResult> func) =>
        _func = func ?? throw new ArgumentNullException(nameof(func));

    public bool Success { get; private set; }
    public bool Completed { get; private set; }
    public Exception? Exception { get; private set; }

    public TResult Result
    {
        get
        {
            while (!Completed) Thread.Sleep(Tick);
            return Success && Exception == null ? _result : throw Exception!;
        }
    }

    public void Start(object? state) =>
        ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadExecution), state);

    private void ThreadExecution(object? state)
    {
        try
        {
            _result = _func.Invoke(state);
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