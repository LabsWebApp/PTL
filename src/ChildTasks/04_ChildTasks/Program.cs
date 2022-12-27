static int Addition(int length)
{
    int sum = 0;
    Thread.Sleep(1000);
    for (var i = 0; i < length; i++) sum++;
    return sum;
}

Task<string> parent = new Task<string>(() =>
{
    var t1 = new Task<int>(() => Addition(5000), TaskCreationOptions.AttachedToParent);
    var t2 = new Task<int>(() => Addition(10000), TaskCreationOptions.AttachedToParent);
    var t3 = new Task<int>(() => Addition(50000), TaskCreationOptions.AttachedToParent);

    t1.Start();
    t2.Start();
    t3.Start();

    t1.ContinueWith(t => WriteLine($"Сложение[1] = {t.Result}"), TaskContinuationOptions.AttachedToParent);
    t2.ContinueWith(t => WriteLine($"Сложение[2] = {t.Result}"), TaskContinuationOptions.AttachedToParent);
    t3.ContinueWith(t => WriteLine($"Сложение[3] = {t.Result}"), TaskContinuationOptions.AttachedToParent);

    return "ВЫПОЛНЕНА";
});

parent.Start();

WriteLine($"Результат задачи - {parent.Result}");

ReadKey();