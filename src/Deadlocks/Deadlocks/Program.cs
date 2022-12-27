namespace Deadlocks;

internal class Program
{
    private static object syncLock1 = new object();
    private static object syncLock2 = new object();

    static async Task Main()
    {
        //Классический deadlock

        //Task t1 = Task.Run(() =>
        //{
        //    lock (syncLock1)
        //    {
        //        Thread.Sleep(1000);
        //        lock (syncLock2)
        //        {
        //            Thread.Sleep(1000);
        //            WriteLine("Задача №1 выполнена");
        //        }
        //    }
        //});

        //Task t2 = Task.Run(() =>
        //{
        //    lock (syncLock2)
        //    {
        //        Thread.Sleep(1000);
        //        lock (syncLock1)
        //        {
        //            Thread.Sleep(1000);
        //            WriteLine("Задача №2 выполнена");
        //        }
        //    }
        //});

        Task t1 = Task.Run(Solution);
        Task t2 = Task.Run(Solution);

        await Task.WhenAll(t1, t2);
        ReadKey();
    }

    private static void Solution()
    {
        lock (syncLock1)
        {
            Thread.Sleep(1000);
            lock (syncLock2)
            {
                Thread.Sleep(1000);
                WriteLine($"Задача №{Task.CurrentId} выполнена");
            }
        }
    }
}