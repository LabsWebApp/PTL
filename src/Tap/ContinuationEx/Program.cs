// Задачи в продолжении

int Calc(int a, int b)
{
    WriteLine($"Task Id #{Task.CurrentId}. Thread Id #{Thread.CurrentThread.ManagedThreadId}.");
    Thread.Sleep(1000);
    return a + b;
}

void Continuation(Task<int> t)
{
    WriteLine($"\nПродолжение task Id #{Task.CurrentId}. Thread Id #{Thread.CurrentThread.ManagedThreadId}.");
    WriteLine($"Результат асинхронной операции - {t.Result}");
}

int a = 2, b = 3;
Task<int> task = Task.Run<int>(() => Calc(a, b));

task.ContinueWith(Continuation);

// Другой вариант продолжения:
//task.ContinueWith((t) =>
//{
//    WriteLine($"Continuation task Id #{Task.CurrentId}. Thread Id #{Thread.CurrentThread.ManagedThreadId}.");
//    WriteLine($"Operation result - {t.Result}");
//});

ReadKey();