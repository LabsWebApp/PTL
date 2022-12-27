using System.Collections.Concurrent;

ConcurrentQueue<string> queue = new ConcurrentQueue<string>();

Task.Factory.StartNew(() =>
{
    Task.Factory.StartNew(() => queue.Enqueue("1"));
    Task.Factory.StartNew(() => queue.Enqueue("2"));
    Task.Factory.StartNew(() => queue.Enqueue("3"));
    Task.Factory.StartNew(() => queue.Enqueue("4"));
    Task.Factory.StartNew(() => queue.Enqueue("5"));

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
});

ReadKey();
foreach (var item in queue) WriteLine(item);
ReadKey();
