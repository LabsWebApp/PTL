// Задачи с параметрами

namespace Params;

internal record struct Box(int A, int B);

internal class Program
{
    private static int Calc(object arg)
    {
        Box box = (Box)arg;
        return box.A + box.B;
    }

    private static int Calc(int a, int b) => a + b;

    private static void ShowSelfParameters(
        int a, 
        bool b,
        char c, 
        string d,
        double e,
        object f,
        Box box,
        Program program, 
        dynamic something)
    {
        WriteLine(new string('-', 80));

        WriteLine(a);
        WriteLine(b);
        WriteLine(c);
        WriteLine(d);
        WriteLine(e);
        WriteLine(f);
        WriteLine(box);
        WriteLine(program.GetType().Name);
        WriteLine(something);

        WriteLine(new string('-', 80));
    }

    private static void Main()
    {
        int a = 3, b = 2;

        Box box = new(a,b)
        {
            A = a,
            B = b
        };

        Task<int> task = new Task<int>(Calc!, box);
        task.Start();

        WriteLine($"Сумма чисел : {task.Result}");
        WriteLine(new string('-', 80));

        // Неудобно создавать типы для передачи параметров, правда?
        // - Замыкание (Closure)
        Task<int> lambda = new(() => Calc(a, 5));
        lambda.Start();

        WriteLine($"Сумма чисел : {lambda.Result}");
        WriteLine(new string('-', 80));

        // Еще удобнее :
        Task<int> taskRun = Task.Run(() =>
        {
            int a1 = 5;
            int b1 = 5;
            return Calc(a1, b1) + Calc(a, b);
        });

        WriteLine($"Сумма чисел : {taskRun.Result}");

        // Метод с большим количеством параметров :
        Task.Run(() => ShowSelfParameters(
            1,
            false,
            'c',
            "hello",
            3.14,
            new object(),
            box, 
            new Program(), 
            taskRun));
        ReadKey();
    }
}