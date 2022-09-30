using ManualTaskEx;

int SumNumber(object arg)
{
    var number = (int)arg;
    var sum = 0;
    for (var i = 0; i < number; i++)
    {
        sum += i;
        Thread.Sleep(1);
    }
    return sum;
}

var getResultWorker = new GetResultWorker<int>(SumNumber!);
getResultWorker.Start(1000);

try
{
    void WriteChar(char @char, int frequency = 40)
    {
        Write(@char);
        Thread.Sleep(frequency);
        Write("\b");
    }

    CursorVisible = false;
    ForegroundColor = ConsoleColor.Red;
    Write("Подождите! Идут сложные вычисления - ");
    while (!getResultWorker.Completed) 
    {
        WriteChar('\\');
        if (getResultWorker.Completed) break;

        WriteChar('|');
        if (getResultWorker.Completed) break;

        WriteChar('/'); ;
        if (getResultWorker.Completed) break;

        WriteChar('—');
    } 
}
finally
{
    SetCursorPosition(0, CursorTop);
    Write(new string(' ', BufferWidth));
    SetCursorPosition(0, CursorTop);
    ResetColor();
    CursorVisible = true;
}

WriteLine($"Результат асинхронной операции = {getResultWorker.Result}");

ReadLine();