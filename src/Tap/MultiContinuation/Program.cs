int GetValue() => 10;

int Increment(Task<int> t)
{
    WriteLine($"Продолжение task Id #{Task.CurrentId}. Thread Id #{Thread.CurrentThread.ManagedThreadId}.");
    int result = t.Result + 1;
    return result;
}

void ShowRes(Task<int> t)
{
    WriteLine($"Продолжение task Id #{Task.CurrentId}. Thread Id #{Thread.CurrentThread.ManagedThreadId}.");
    WriteLine($"Результат - {t.Result}");
}

Task<int> task = Task.Run<int>(new Func<int>(GetValue));
//Task<int> c1 = task.ContinueWith<int>(Increment);
//Task<int> c2 = c1.ContinueWith<int>(Increment);
//Task<int> c3 = c2.ContinueWith<int>(Increment);
//Task<int> c4 = c3.ContinueWith<int>(Increment);
//Task<int> c5 = c4.ContinueWith<int>(Increment);
//c5.ContinueWith(ShowRes);

task.ContinueWith(Increment)
    .ContinueWith(Increment)
    .ContinueWith(Increment)
    .ContinueWith(Increment)
    .ContinueWith(Increment)
    .ContinueWith(ShowRes);

WriteLine("Метод Main завершил свою работу..");
ReadKey();
