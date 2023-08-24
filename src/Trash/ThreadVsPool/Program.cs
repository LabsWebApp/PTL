var isPool = () => WriteLine("    " + Thread.CurrentThread.IsThreadPoolThread);

WriteLine("Parallel.Invoke");
Parallel.Invoke(isPool, isPool, isPool, isPool, isPool);

WriteLine("Parallel.For");
Parallel.For(0, 5, i => isPool());

WriteLine("AsParallel");
Enumerable.Range(0, 5).AsParallel().ForAll(_ => isPool.Invoke());

WriteLine("TaskScheduler.Default");
Task.Factory.StartNew(isPool).Wait();

WriteLine("TaskScheduler.Default TaskLongRunning");
Task.Factory.StartNew(isPool, TaskCreationOptions.LongRunning).Wait();

WriteLine("Thread");
new Thread(() => isPool()).Start();

ReadKey();