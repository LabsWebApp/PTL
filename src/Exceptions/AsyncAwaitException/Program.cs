using System.Threading.Tasks;

namespace AsyncAwaitException;

internal class Program
{
    static async Task Main()
    {
        WriteLine("Метод Main начал свою работу");

        try
        {
            await OperationAsync();
        }
        catch (Exception e)
        {
            BackgroundColor = ConsoleColor.Red;
            ForegroundColor = ConsoleColor.White;
            WriteLine($"Исключение - {e.GetType()}");
            WriteLine($"Сообщение - {e.Message}");
            ResetColor();
        }

        #region Обработка множественных исключений

        //с помощью свойств Exception
        #region Способ 1

        //Task task = OperationsAsync();
        //try
        //{
        //    task.Wait();
        //}
        //catch (Exception e)
        //{
        //    AggregateException? aggregateException = task.Exception;

        //    foreach (var exception in aggregateException?.InnerExceptions ?? Enumerable.Empty<Exception>())
        //    {
        //        WriteLine($"Сообщение исключения - {exception.Message}");
        //    }
        //}
        #endregion

        //с помощью ContinueWith
        #region Способ 2

        //Task tasks = OperationsAsync();
        //try
        //{
        //    await tasks.ContinueWith(_ => { }, TaskContinuationOptions.ExecuteSynchronously);
        //    tasks.Wait();
        //}
        //catch (AggregateException e)
        //{
        //    foreach (var exception in e.InnerExceptions)
        //    {
        //        WriteLine($"Сообщение исключения - {exception.Message}");
        //    }
        //}
        #endregion

        //с помощью внутри ContinueWith
        #region Способ 3

        //await OperationsAsync().ContinueWith(t =>
        //{
        //    try
        //    {
        //        t.Wait();
        //    }
        //    catch (AggregateException e)
        //    {
        //        foreach (var exception in e.InnerExceptions)
        //        {
        //            WriteLine($"Сообщение исключения - {exception.Message}");
        //        }
        //    }
        //});
        #endregion

        #endregion

        WriteLine("Метод Main закончил свою работу");
        ReadKey();
    }

    private static async Task OperationAsync()
    {
        WriteLine("Метод OperationAsync начал свою работу");
        await Task.Run(() =>
            throw new Exception("Метод OperationAsync выбросил исключение"));
        WriteLine("Метод OperationAsync закончил свою работу");
    }

    private static Task OperationsAsync()
    {
        Action<int> operation = threadNumber =>
        {
            Thread.Sleep(threadNumber * 300);
            throw new Exception($"Задача {threadNumber} в методе OperationAsync выбросила исключение");
        };
        
        Task t1 = Task.Run(() => operation.Invoke(1));
        Task t2 = Task.Run(() => operation.Invoke(2));
        Task t3 = Task.Run(() => operation.Invoke(3));

        return Task.WhenAll(t1, t2, t3);
    }
}