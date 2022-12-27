// Дочерние задачи не выйдет:
// (Run тоже!)
Task parent = Task.Run(() =>
{
    Task.Factory.StartNew(() =>
    {
        Thread.Sleep(1000);
        WriteLine("Child 1 completed.");
    }, TaskCreationOptions.AttachedToParent);

    Task.Factory.StartNew(() =>
    {
        Thread.Sleep(2000);
        WriteLine("Child 2 completed.");
    }, TaskCreationOptions.AttachedToParent);

    Thread.Sleep(100);
});

//parent.Start();
parent.Wait();
WriteLine("parent completed.");

ReadKey();