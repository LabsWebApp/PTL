using System.Diagnostics;
using System.Numerics;
using factorial;
using factorial.QuickThreadPools;

// Иллюстрация того, что c# не поддерживает оптимизацию хвостовой рекурсии (Tail Call):

BigInteger Factorial(BigInteger i) => i <= BigInteger.One
    ? BigInteger.One
    : i * Factorial(i - BigInteger.One);


BigInteger FactorialTail(BigInteger i)
{
    BigInteger FactorialAcc(BigInteger acc, BigInteger n) => n <= BigInteger.One
        ? acc
        : FactorialAcc(n * acc, n - BigInteger.One);

    return FactorialAcc(BigInteger.One, i);
}

//BigInteger test = 4600;
//WriteLine(Factorial(test));
//WriteLine(FactorialTail(test));

//ReadKey();

Stopwatch stopwatch = new();

while (true)
{
    using QuickThreadPoolTaskScheduler scheduler = new();

    WriteLine("Выберете метод вычисления факториала:");
    int methods = 0;
    WriteLine($"{++methods} - ThreadFactorial");
    WriteLine($"{++methods} - ParallelForFactorial");
    WriteLine($"{++methods} - ParallelForFactorial with QuickThreadPoolTaskScheduler");
    WriteLine($"{++methods} - AsParallelFactorial");
    WriteLine($"{++methods} - TasksFactorial");
    WriteLine($"{++methods} - TasksFactorial with QuickThreadPoolTaskScheduler");
    WriteLine($"{++methods} - AsParallelSimpleFactorial");
    WriteLine("любая клавиша - Factorial ('Esc' завершить работу)");
    
    var key = ReadKey();
    if (key.Key == ConsoleKey.Escape) break;
    if (!int.TryParse(new[] { key.KeyChar }, out var mode) || mode < 1) mode = ++methods;
    Write("\b");

    var processes = Environment.ProcessorCount;
    if (mode < methods)
    {
        WriteLine($"Разбить решения на 1024 подзадачи нажмите: 'Y', по умолчанию ({processes}) любая клавиша.");
        if (ReadKey().Key == ConsoleKey.Y) processes = 1024;
        Write("\b");
    }

    var info =
        $"{new string('-', 4)} Выбранный метод: " +
        $"{mode switch
        {
            1 => "ThreadFactorial",
            2 => "ParallelForFactorial",
            3 => "ParallelForFactorial with QuickThreadPoolTaskScheduler",
            4 => "AsParallelFactorial",
            5 => "TasksFactorial",
            6 => "TasksFactorial with QuickThreadPoolTaskScheduler",
            7 => "AsParallelSimpleFactorial",
            _ => "Factorial"
        }}";
    if (mode < 7) info += $"; processes = {processes}";

    WriteLine($"{info}\nВведите N: ");
    var nString = ReadLine();
    if (!int.TryParse(nString, out var n))
    {
        WriteLine("Error!");
        continue;
    }

    CursorVisible = false;
    SetCursorPosition(nString.TrimEnd().Length, GetCursorPosition().Top - 1);

    stopwatch.Restart();
    var result = mode switch
    {
        1 => n.ThreadFactorial(processes),
        2 or 3 => n.ParallelForFactorial(scheduler, processes),
        4 => n.AsParallelFactorial(processes: processes),
        5 or 6 => n.TasksFactorial(scheduler, processes),
        7 => n.AsParallelSimpleFactorial(),
        _ => n.Factorial()
    };
    stopwatch.Stop();
    Beep();

    Write($"! = {(n <= 100000 ? result.ToString() : result.GetByteCount() + " bytes")}\n");
    WriteLine(info + $" ... выполнил расчёт за {stopwatch.Elapsed}");

    GC.Collect();
    WriteLine("*****");
    CursorVisible = true;
}