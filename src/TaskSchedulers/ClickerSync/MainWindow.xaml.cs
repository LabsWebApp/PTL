using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ClickerSync;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private bool _flag;
    public MainWindow()
    {
        InitializeComponent();
        LblThreadId.Content += $"{Thread.CurrentThread.ManagedThreadId}";
    }

    private void BtnStart_Click(object sender, RoutedEventArgs e)
    {
        if (_flag) return;

        HandlerSetup();

        new Task(() =>
        {
            while (_flag)
            {
                ShowThreadPoolInfo();
                Thread.Sleep(150);
            }
        }, TaskCreationOptions.LongRunning).Start();

        Worker();
    }

    private void HandlerSetup()
    {
        TxtContinuations.Text = string.Empty;
        TxtThreadPool.Text = string.Empty;
        _flag = true;
    }

    private void ShowThreadPoolInfo()
    {
        ThreadPool.GetAvailableThreads(out int threads, out int completionPorts);
        ThreadPool.GetMaxThreads(out int maxThreads, out int maxCompletionPorts);

        string result = $"Worker Threads - [{threads}:{maxThreads}]{Environment.NewLine}";

        Dispatcher.Invoke(() => TxtThreadPool.Text += result);
    }

    private void Worker()
    {
        TaskScheduler scheduler = new SynchronizationContextTaskScheduler();
        // TaskScheduler.Default;
        // new SynchronizationContextTaskScheduler(); 
        // смысла нет, так имеется:
        // TaskScheduler.FromCurrentSynchronizationContext();

        Task<int>[] tasks = new Task<int>[19];

        new Task(() =>
        {
            for (int i = 0; i < tasks.Length; i++)
            {
                int m = i;
                tasks[i] = new Task<int>(() =>
                {
                    var id = Thread.CurrentThread.ManagedThreadId;
                    Dispatcher.Invoke(() =>
                    {
                        TxtContinuations.Text += $"Старт {m} в потоке[{id}]";
                        TxtContinuations.Text += Environment.NewLine;
                    });

                    //TxtContinuations.Text += $"Старт {m} в потоке[{id}]";
                    //TxtContinuations.Text += Environment.NewLine;

                    Thread.Sleep(1000);
                    return m;
                }, TaskCreationOptions.RunContinuationsAsynchronously);
                tasks[i].Start(scheduler);

                tasks[i].ContinueWith(t =>
                {
                    TxtContinuations.Text += $"Результат - {t.Result} в потоке[{Thread.CurrentThread.ManagedThreadId}]";
                    TxtContinuations.Text += Environment.NewLine;
                }, CancellationToken.None, TaskContinuationOptions.RunContinuationsAsynchronously, scheduler);
            }

            foreach (var task in tasks) task.Wait();
            Thread.Sleep(1100);
            _flag = false;
        }, TaskCreationOptions.LongRunning).Start();
    }
}