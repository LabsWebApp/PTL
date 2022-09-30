// Задачи в продолжении

void OperationAsync(object arg)
{
    WriteLine($"Task #{Task.CurrentId} started in a thread {Thread.CurrentThread.ManagedThreadId}. ");
    WriteLine($"Argument value - {arg}");
    WriteLine($"Task #{Task.CurrentId} completed in the thread {Thread.CurrentThread.ManagedThreadId}.");
}

void Continuation(Task task)
{
    Write($"\nПродолжение #{Task.CurrentId} сработало в потоке {Thread.CurrentThread.ManagedThreadId}. ");
    WriteLine($"Параметр задачи - {task.AsyncState}");
    WriteLine("Сразу после выполнения задачи.");
}

Task task = new Task(new Action<object?>(OperationAsync!), "Hello world");
Task continuation = task.ContinueWith(Continuation);

WriteLine($"Статус продолжения - {continuation.Status}");

task.Start();

ReadKey();