//AsTask

static ValueTask<double> GetIndexing(int salary)
{
    Thread.Sleep(500);

    return salary switch
    {
        <= 0 or > 5000 => new ValueTask<double>(0),
        5000 => new ValueTask<double>(0.1),
        _ => new ValueTask<double>(Task.Run(() =>
        {
            var index = 0.0;
            for (var i = 0; i < 5; i++)
            {
                Thread.Sleep(500);
                index += 0.1;
            }
            return index;
        }))
    };
}

const int  salary = 5000;
ValueTask<double> valueTask = GetIndexing(salary);

while (!valueTask.IsCompleted)
{
    Write('*');
    Thread.Sleep(50);
}

Task<double> task = valueTask.AsTask();

task.ContinueWith(t => 
    WriteLine($"\nИндексация зарплаты {salary} = {t.Result}%"));

ReadKey();