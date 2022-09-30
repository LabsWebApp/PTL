void Method()
{
    Thread.Sleep(2000);
    WriteLine($"Задача #{Task.CurrentId} выполнила метод в потоке {Thread.CurrentThread.ManagedThreadId}");
    WriteLine(new string('-', 80));
}

void Continuation(Task task)
{
    WriteLine($"Id задачи продолжения - {Task.CurrentId}.");
    WriteLine($"Продолжение выполнилось в потоке {Thread.CurrentThread.ManagedThreadId}");
    WriteLine();
}

Task task = Task.Run(Method);
// Если не указывать настроек для продолжения,
// то по умолчанию TaskContinuationOptions.None :
task.ContinueWith((t) => Continuation(t));

// Указание настроек выполнения продолжения :
//task.ContinueWith(Continuation, TaskContinuationOptions.ExecuteSynchronously);
//task.ContinueWith(Continuation, TaskContinuationOptions.RunContinuationsAsynchronously);

// Другие разновидности настроек продолжения:
//task.ContinueWith((t) => Continuation(t), TaskContinuationOptions.AttachedToParent);
//task.ContinueWith((t) => Continuation(t), TaskContinuationOptions.DenyChildAttach);
//task.ContinueWith((t) => Continuation(t), TaskContinuationOptions.HideScheduler);
//task.ContinueWith((t) => Continuation(t), TaskContinuationOptions.LazyCancellation);
//task.ContinueWith((t) => Continuation(t), TaskContinuationOptions.LongRunning);
//task.ContinueWith((t) => Continuation(t), TaskContinuationOptions.None);
//task.ContinueWith((t) => Continuation(t), TaskContinuationOptions.NotOnCanceled);
//task.ContinueWith((t) => Continuation(t), TaskContinuationOptions.NotOnRanToCompletion);
//task.ContinueWith((t) => Continuation(t), TaskContinuationOptions.OnlyOnCanceled);
//task.ContinueWith((t) => Continuation(t), TaskContinuationOptions.OnlyOnFaulted);
//task.ContinueWith((t) => Continuation(t), TaskContinuationOptions.OnlyOnRanToCompletion);
//task.ContinueWith((t) => Continuation(t), TaskContinuationOptions.PreferFairness);


ReadKey();