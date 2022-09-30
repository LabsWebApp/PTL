// Deadlock
/*
 * представьте следующую последовательность событий:
 *  - Поток 1 получает блокировку A.
 *  - Поток 2 получает блокировку B.
 *  - Поток 1 пытается получить блокировку B, но она уже удерживается потоком 2,
 *  поэтому поток 1 блокируется до тех пор, пока блокировка B не будет снята.
 *  - Поток 2 пытается получить блокировку A, но она удерживается потоком 1,
 *  поэтому поток 2 блокируется до тех пор, пока блокировка A не будет освобождена.
 * В этот момент оба потока заблокированы и никогда не проснутся.
 */
/*
object lockA = new();
object lockB = new();
Поток 1
{
    lock (lockA)
    {
        lock (lockB)
        {
            ... 
        }
    }
}
Поток 2
{
    lock (lockB)
    {
        lock (lockA)
        {
             ... 
        }
    }
}*/

object firstLock = new object();
object secondLock = new object();

void ThreadJob()
{
    WriteLine("\t\t\t\tLocking firstLock");
    lock (firstLock)
    {
        WriteLine("\t\t\t\tLocked firstLock");
        // Ждём, пока мы не будем достаточно уверены, что первый поток
        // захватил второй замок
        Thread.Sleep(1000);
        WriteLine("\t\t\t\tLocking secondLock");
        lock (secondLock)
        {
            WriteLine("\t\t\t\tLocked secondLock");
        }
        WriteLine("\t\t\t\tReleased secondLock");
    }
    WriteLine("\t\t\t\tReleased firstLock");
}

new Thread(new ThreadStart(ThreadJob)).Start();
// Ждём, пока мы не будем достаточно уверены, что первый поток
// захватил первый замок
Thread.Sleep(500);
WriteLine("Locking secondLock");
lock (secondLock)
{
    WriteLine("Locked secondLock");
    WriteLine("Locking firstLock");
    lock (firstLock)
    {
        WriteLine("Locked firstLock");
    }
    WriteLine("Released firstLock");
}
WriteLine("Released secondLock");
Read();