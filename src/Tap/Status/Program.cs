// Task.Status

void Method() => Thread.Sleep(2000);

Task task = new Task(new Action(Method));

Console.WriteLine($"{task.Status}");

task.Start();

Console.WriteLine($"{task.Status}");

Thread.Sleep(1000);

Console.WriteLine($"{task.Status}");

Thread.Sleep(2000);

Console.WriteLine($"{task.Status}");

Console.ReadKey();