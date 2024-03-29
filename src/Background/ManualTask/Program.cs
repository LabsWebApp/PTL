﻿using ManualTask;

void CharWriter(object arg)
{
    if (arg is not char) return;
    for (var i = 0; i < 120; i++)
    {
        Write(arg);
        Thread.Sleep(30);
    }
}

WriteLine("Для запуска нажмите любую клавишу");
ReadKey();

var threadPoolWorker = new VoidWorker(new Action<object?>(CharWriter!));
threadPoolWorker.Start('*');

for (var i = 0; i< 40; i++)
{
    Write('-');
    Thread.Sleep(50);
}

threadPoolWorker.Wait();

WriteLine("Метод Main закончил свою работу.");

ReadKey();