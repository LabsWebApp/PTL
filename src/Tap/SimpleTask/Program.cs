void ThreadOutput()
{
    for (int i = 0; i < 40; i++)
    {
        Write('*');
        Thread.Sleep(75);
    }
}

void MainOutput()
{
    for (int i = 0; i < 40; i++)
    { 
        Write('!');
        Thread.Sleep(75);
    }
}

Action threadOutput = new Action(ThreadOutput);

Task task = new Task(threadOutput);
task.Start();
MainOutput();

WriteLine("\n--------------");

TaskFactory taskFactory = new TaskFactory();
//TaskFactory taskFactory = Task.Factory;

taskFactory.StartNew(threadOutput);
MainOutput();

WriteLine("\n--------------");

Task.Run(threadOutput);
MainOutput();

WriteLine("\n--------------");

task = new Task(threadOutput);
task.RunSynchronously();
MainOutput();

ReadKey();