using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Progress
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IProgress<int> _progress;
        private CancellationTokenSource _cts = new CancellationTokenSource();

        private readonly string _nLine = Environment.NewLine;

        public MainWindow()
        {
            InitializeComponent();
            _progress = new Progress<int>(ProgressBarUpdate);
        }

        private void ProgressBarUpdate(int value) => progressBar.Value = value;

        private async void btnStart_Click(object sender, RoutedEventArgs e)
        {
            progressBar.Value = 0;
            btnStart.IsEnabled = false;
            btnCancel.IsEnabled = true;

            Operation operation = new Operation();

            try
            {
                var nums = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
                var sum = await operation.SumNumbersAsync(nums, _cts.Token, _progress);
                txtOutput.Text += $"Операция завершена.{_nLine}{GetNumbers(nums)} = {sum}";
                txtOutput.Text += $"{_nLine}------------{_nLine}";
            }
            catch (OperationCanceledException)
            {
                _cts = new CancellationTokenSource();
                txtOutput.Text += "Операция отменена.";
                txtOutput.Text += $"{_nLine}------------{_nLine}";
            }
            catch (Exception exception)
            {
                txtOutput.Text += $"Название ошибки - {exception.GetType()}{_nLine}";
                txtOutput.Text += $"Сообщение ошибки - {exception.Message}{_nLine}";
                txtOutput.Text += $"{_nLine}------------{_nLine}";
            }
            finally
            {
                btnStart.IsEnabled = true;
                btnCancel.IsEnabled = false;
            }
        }

        private string GetNumbers(IEnumerable<int> numbers)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var num in numbers) sb.Append(num).Append('+');
            return sb.ToString().TrimEnd('+');
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            _cts.Cancel();
            btnCancel.IsEnabled = false;
        }
    }
}
