CancellationTokenSource cts = new CancellationTokenSource();

//Task<int> task = Task.Run(() => 666);
//Task<int> task = Task.Run(() => 666, cts.Token);
Task<int> task = Task.Run(() =>
{
    throw new Exception("Проверка продолжений");
#pragma warning disable CS0162
    return 666;
#pragma warning restore CS0162
});

cts.Cancel();

task.ContinueWith(t => WriteLine($"Результат задачи = {t.Result}"),
    TaskContinuationOptions.OnlyOnRanToCompletion);

task.ContinueWith(_ => WriteLine("Задача была отменена"),
    TaskContinuationOptions.OnlyOnCanceled);

task.ContinueWith(t =>
    {
        WriteLine("Обработка ошибок:");
        WriteLine($"\tСообщение ошибки - {t.Exception!.InnerException!.Message}");
    },
    TaskContinuationOptions.OnlyOnFaulted);

ReadKey();