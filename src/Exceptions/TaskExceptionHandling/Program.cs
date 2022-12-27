int DoSomething()
{
    //DoSomething(); //StackOverflowException
    WriteLine($"Работа {nameof(DoSomething)} до ошибки...");
    throw new Exception($"Метод {nameof(DoSomething)} совершил ошибку");
    return -1;
}

Task<int> task = Task.Run(DoSomething);

try
{
    //task.Wait();
    //WriteLine(task.Result);
    WriteLine("Всё OK - ошибок нет!");
}
catch (AggregateException e)
{
    WriteLine($"Исключение - {e.GetType()} [{e.Message}]");
    foreach (var inner in e.InnerExceptions)
    {
        WriteLine(new string('-', 80));
        WriteLine($"Вложенное исключение - {inner.GetType()} [{inner.Message}]");
    }
}

while (true)
{
    Write("*");
    Thread.Sleep(300);
}