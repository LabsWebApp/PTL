// Wait(), WaitAll(), WaitAny()
// Могут приводить к блокировкам (deadlocks).

void DoSomething(object sleepTime)
{
    WriteLine($"\tЗадача #{Task.CurrentId} началась в потоке {Thread.CurrentThread.ManagedThreadId}");
    Thread.Sleep((int)sleepTime);
    WriteLine($"\t\tЗадача #{Task.CurrentId} завершилась в потоке {Thread.CurrentThread.ManagedThreadId}");
}

Task[] tasks = 
{
    new(DoSomething!, 1000),
    new(DoSomething!, 800),
    new(DoSomething!, 2000),
    new(DoSomething!, 1000),
    new(DoSomething!, 3500),
};

WriteLine("Метод Main выполняется..");
foreach (Task task in tasks)
    task.Start();
//даём стартануть
Thread.Sleep(500);
WriteLine("Метод Main ожидает..");
foreach (Task task in tasks) task.Wait();
//Task.WaitAll(tasks);
//Task.WaitAny(tasks);

WriteLine("Метод Main продолжает свою работу");

for (int i = 0; i < 5; i++)
{
    WriteLine($"Main({i})");
}

Read();