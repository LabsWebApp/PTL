void Method(object _)
{
    //throw new Exception("Исключение во вторичном потоке");

    try
    {
        throw new Exception("Исключение во вторичном потоке");
    }
    catch (Exception e)
    {
        WriteLine(e.Message);
    }
}

//try - catch ТУТ НЕ СРАБОТАЕТ!!!

//new Thread(Method!).Start();
//ThreadPool.QueueUserWorkItem(Method!);

try
{
    //new Thread(Method!).Start();
    ThreadPool.QueueUserWorkItem(Method!);
}
catch (Exception e)
{
    WriteLine(e.Message);
}

while (true)
{
    Write("*");
    Thread.Sleep(300);
}