using System.Threading.Tasks;

Task parent = new Task(() =>
{
    new Task(() =>
    {
        Thread.Sleep(500);
        WriteLine("Дочерняя задача № 1 завершила свою работу.");
    }, TaskCreationOptions.AttachedToParent).Start();

    new Task(() =>
    {
        Thread.Sleep(600);
        WriteLine("Дочерняя задача № 2 завершила свою работу.");
    }, TaskCreationOptions.AttachedToParent).Start();

    new Task(() =>
    {
        Thread.Sleep(700);
        throw new Exception("Ошибка в дочерней задаче № 3.");
    }, TaskCreationOptions.AttachedToParent).Start(); // превратить во вложенные

    new Task(() =>
    {
        Thread.Sleep(800);
        WriteLine("Дочерняя задача № 4 завершила свою работу.");
    }, TaskCreationOptions.AttachedToParent).Start();

    new Task(() =>
    {
        new Task(() =>
            throw new Exception("Ошибка в дочерней задаче № 5.1 второго уровня вложенности"),
            TaskCreationOptions.AttachedToParent).Start();

        Thread.Sleep(900);
        throw new Exception("Ошибка в дочерней задаче № 5.");
    }, TaskCreationOptions.AttachedToParent).Start();

    new Task(() =>
    {
        Thread.Sleep(1000);
        throw new Exception("Ошибка в дочерней задаче № 6.");
    }, TaskCreationOptions.AttachedToParent).Start();

    new Task(() =>
    {
        Thread.Sleep(1100);
        throw new Exception("Ошибка в дочерней задаче № 7.");
    }, TaskCreationOptions.AttachedToParent).Start();

    new Task(() =>
    {
        Thread.Sleep(1200);
        WriteLine("Дочерняя задача № 8 завершила свою работу.");
    }, TaskCreationOptions.AttachedToParent).Start();
});

parent.Start();

try
{
    parent.Wait();
}
catch (AggregateException e)
{
   // WriteLine($"Исключение - {e.GetType()} [{e.Message}]");
    WriteLine(new string('-', 80));
    ////foreach (var inner in e.InnerExceptions)
    ////{
    ////    if (inner is AggregateException aggregateException)
    ////    {
    ////        foreach (var child in aggregateException.InnerExceptions)
    ////        {
    ////            WriteLine($"Сообщение из исключения дочерней задачи - {child.Message}");
    ////        }
    ////    }
    ////    else
    ////    {
    ////        WriteLine($"Сообщение из исключения родительской задачи - {inner.Message}");
    ////    }
    ////}

    HandleTaskException(e);
}

WriteLine(new string('-', 80));
WriteLine($"Состояние родительской задачи - {parent.Status}");

ReadKey();

void HandleTaskException(AggregateException ae)
{
    foreach (var item in ae.InnerExceptions)
    {
        if (item is AggregateException aggregateException)
        {
            HandleTaskException(aggregateException);
        }
        else
        {
            WriteLine($"Сообщение из исключения - {item.Message}");
        }
    }
}