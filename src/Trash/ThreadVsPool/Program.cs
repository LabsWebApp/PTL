Action IsPool = () => WriteLine("    " + Thread.CurrentThread.IsThreadPoolThread);

WriteLine("Parallel.Invoke");
Parallel.Invoke(IsPool, IsPool, IsPool, IsPool, IsPool);

WriteLine("Parallel.For");
Parallel.For(0, 5, i => IsPool());

WriteLine("AsParallel");
Enumerable.Range(0, 5).AsParallel().ForAll(_ => IsPool.Invoke());

WriteLine("TaskScheduler.Default");
Task.Factory.StartNew(IsPool).Wait();

WriteLine("TaskScheduler.Default TaskLongRunning");
Task.Factory.StartNew(IsPool, TaskCreationOptions.LongRunning).Wait();

WriteLine("Thread");
new Thread(() => IsPool()).Start();

ReadKey();