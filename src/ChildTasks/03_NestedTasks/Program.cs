// Вложенные задачи
static int Addition(int length)
{
    int sum = 0;
    Thread.Sleep(3000);
    for (var i = 0; i < length; i++) sum++;
    return sum;
}

Task<string> parent = Task.Run(() =>
{
    var t1 = new Task<int>(() => Addition(5000));
    var t2 = new Task<int>(() => Addition(10000));
    var t3 = new Task<int>(() => Addition(50000));

    t1.Start();
    t2.Start();
    t3.Start();

    t1.ContinueWith(t => WriteLine($"Сложение[1] = {t.Result}"));
    t2.ContinueWith(t => WriteLine($"Сложение[2] = {t.Result}"));
    t3.ContinueWith(t => WriteLine($"Сложение[3] = {t.Result}"));

    return "ВЫПОЛНЕНА";
    //return (t1.Result + t2.Result + t3.Result).ToString();
});

WriteLine($"Результат задачи - {parent.Result}");

ReadKey();