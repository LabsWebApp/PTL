using System;
using System.Threading;
using System.Threading.Tasks;

WriteLine("основной поток запущен");

CancellationTokenSource cts = new CancellationTokenSource();

// Передача cts.Token в конструктор Task.Run нужна для:
// - если была отмена до запуска, то задача не запустится
// - если произошла OperationCanceledException c переданным токеном, то статус задачи будет
//              IsCanceled (Faulted)
Task task = Task.Run(() => Work(cts.Token), cts.Token);

Thread.Sleep(2000);

try
{
    cts.Cancel();
    task.Wait();
}
catch (AggregateException)
{
    if (task.IsCanceled) WriteLine("Задача отменена");
    if (task.IsFaulted) WriteLine("Задача провалена");
}
finally
{
    cts.Dispose();
    task.Dispose();
}

ReadKey();

void Work(CancellationToken token)
{
    token.ThrowIfCancellationRequested();
    WriteLine("Work запущен");

    for (int count = 0; count < 10; count++)
    {
        if (token.IsCancellationRequested)
        {
            WriteLine("Получен запрос на отмену");
            token.ThrowIfCancellationRequested();
            //throw new OperationCanceledException();
        }
        Thread.Sleep(500);
        WriteLine($"count = {count}");
    }

    WriteLine("Work завершён");
}