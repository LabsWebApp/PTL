CancellationTokenSource parentCts1 = new CancellationTokenSource();
CancellationTokenSource parentCts2 = new CancellationTokenSource();
CancellationTokenSource parentCts3 = new CancellationTokenSource();
CancellationTokenSource linkedCts4 =
    CancellationTokenSource.CreateLinkedTokenSource(parentCts1.Token, parentCts2.Token);
CancellationTokenSource linkedCts5 =
    CancellationTokenSource.CreateLinkedTokenSource(linkedCts4.Token, parentCts3.Token);

CancellationToken parentToken1 = parentCts1.Token;
CancellationToken parentToken2 = parentCts2.Token;
CancellationToken parentToken3 = parentCts3.Token;
CancellationToken linkedToken4 = linkedCts4.Token;
CancellationToken linkedToken5 = linkedCts5.Token;

var t1 = Task.Run(() => Work("1", parentToken1), parentToken1);
var t2 = Task.Run(() => Work("2", parentToken2), parentToken2);
var t3 = Task.Run(() => Work("3", parentToken3), parentToken3);
var t4 = Task.Run(() => Work("4", linkedToken4), linkedToken4);
var t5 = Task.Run(() => Work("5", linkedToken5), linkedToken5);

//Регистрация обработки отмены
parentToken1.Register(() => Canceled(1));
parentToken2.Register(() => Canceled(2));
parentToken3.Register(() => Canceled(3));
linkedToken4.Register(() => Canceled(4));
linkedToken5.Register(() => Canceled(5));

//parentCts1.CancelAfter(1500);
//parentCts2.CancelAfter(1500);
//parentCts3.CancelAfter(1500);
linkedCts4.CancelAfter(1500);
//linkedCts5.CancelAfter(1500);

ReadKey();

void Work(string taskId, CancellationToken cancellationToken)
{
    WriteLine($"Задача №{taskId} начала свою работу в потоке {Thread.CurrentThread.ManagedThreadId}");
    Thread.Sleep(1000);

    int sum = 0;
    for (int i = 0; i < 150; i++)
    {
        cancellationToken.ThrowIfCancellationRequested();
        sum += i;
        Thread.Sleep(1);
    }

    WriteLine($"\tЗадача №{taskId} в потоке {Thread.CurrentThread.ManagedThreadId} насчитала {sum}");
}

void Canceled(int taskId) =>
    WriteLine($"---Задача №{taskId} в потоке {Thread.CurrentThread.ManagedThreadId} была отменена");