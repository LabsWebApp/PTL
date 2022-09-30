Random random = new();

double Calculate(int x)
{
    double res = 0.0;
    for (int i = 0; i < 10; i++)
    {
        res += i * random.Next(1, x) / (x * 2) * x;
    }
    WriteLine($"Промежуточный результат - {res:####}");
    return res;
}

TaskFactory taskFactory = new TaskFactory();

Task<double> t1 = taskFactory.StartNew(() => Calculate(1));
Task<double> t2 = taskFactory.StartNew(() => Calculate(2));
Task<double> t3 = taskFactory.StartNew(() => Calculate(3));
Task<double> t4 = taskFactory.StartNew(() => Calculate(4));
Task<double> t5 = taskFactory.StartNew(() => Calculate(5));

taskFactory.ContinueWhenAll(new Task[] { t1, t2, t3, t4, t5 },
    completedTasks =>
    {
        double sum = 0;

        foreach (var task in completedTasks)
        {
            var item = (Task<double>)task;
            sum += item.Result;
        }

        WriteLine($"Результат - {sum:####}");
    });
ReadKey();