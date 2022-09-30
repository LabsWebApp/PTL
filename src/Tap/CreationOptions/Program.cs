// TaskCreationOptions

void DoSomething()
{
    WriteLine($"Task Id метода DoSomething : {Task.CurrentId}\nThread Id метода DoSomething : {Thread.CurrentThread.ManagedThreadId}");
    WriteLine(new string('-', 80));

    for (int i = 0; i < 5; i++)
    {
        WriteLine("            Задача выполняется.");
        Thread.Sleep(100);
    }

    WriteLine($"Задача завершена в потоке : {Thread.CurrentThread.ManagedThreadId}.");
}

WriteLine($"Task Id метода Main : {Task.CurrentId ?? -1}");
WriteLine($"Thread Id метода Main : {Thread.CurrentThread.ManagedThreadId}");
WriteLine(new string('-', 80));

Task task = new Task(
    new Action(DoSomething), 
    TaskCreationOptions.PreferFairness | TaskCreationOptions.LongRunning);
//TaskCreationOptions.None 
//TaskCreationOptions.PreferFairness 
//TaskCreationOptions.LongRunning 
//TaskCreationOptions.AttachedToParent 
//TaskCreationOptions.DenyChildAttach 
//TaskCreationOptions.HideScheduler 
//TaskCreationOptions.RunContinuationsAsynchronously 

task.Start(); // Запуск задачи
Thread.Sleep(50); // Даем поработать методу DoSomething

for (int i = 0; i < 5; i++)
{
    WriteLine($"            Метод Main выполняется.");
    Thread.Sleep(100);
}

ReadKey();