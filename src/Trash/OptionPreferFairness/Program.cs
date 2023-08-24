using System.Collections.Concurrent;

var queue = new ConcurrentQueue<string>();

Task.Factory.StartNew(() =>
{
    // Попробовать засунуть всё в циклы

    Task.Factory.StartNew(() => queue.Enqueue("PreferFairness - 1"), 
        TaskCreationOptions.PreferFairness);
    Task.Factory.StartNew(() => queue.Enqueue("PreferFairness - 2"),
        TaskCreationOptions.PreferFairness);
    Task.Factory.StartNew(() => queue.Enqueue("PreferFairness - 3"),
        TaskCreationOptions.PreferFairness);
    Task.Factory.StartNew(() => queue.Enqueue("PreferFairness - 4"),
        TaskCreationOptions.PreferFairness);
    Task.Factory.StartNew(() => queue.Enqueue("PreferFairness - 5"),
        TaskCreationOptions.PreferFairness);
    Task.Factory.StartNew(() => queue.Enqueue("PreferFairness - 6"), 
        TaskCreationOptions.PreferFairness);
    Task.Factory.StartNew(() => queue.Enqueue("PreferFairness - 7"),
        TaskCreationOptions.PreferFairness);
    Task.Factory.StartNew(() => queue.Enqueue("PreferFairness - 8"),
        TaskCreationOptions.PreferFairness);
    Task.Factory.StartNew(() => queue.Enqueue("PreferFairness - 9"),
        TaskCreationOptions.PreferFairness);
    Task.Factory.StartNew(() => queue.Enqueue("PreferFairness - 10"),
        TaskCreationOptions.PreferFairness);

    Task.Factory.StartNew(() => queue.Enqueue("1"));
    Task.Factory.StartNew(() => queue.Enqueue("2"));
    Task.Factory.StartNew(() => queue.Enqueue("3"));
    Task.Factory.StartNew(() => queue.Enqueue("4"));
    Task.Factory.StartNew(() => queue.Enqueue("5"));
    Task.Factory.StartNew(() => queue.Enqueue("6"));
    Task.Factory.StartNew(() => queue.Enqueue("7"));
    Task.Factory.StartNew(() => queue.Enqueue("8"));
    Task.Factory.StartNew(() => queue.Enqueue("9"));
    Task.Factory.StartNew(() => queue.Enqueue("10"));
});

ReadKey();
foreach (var item in queue) WriteLine(item);
ReadKey();
