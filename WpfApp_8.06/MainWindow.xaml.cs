using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfApp_8._06
{
    public partial class MainWindow : Window
    {
        private CancellationTokenSource cancellationTokenSource;
        private List<ProgressBar> progressBars;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void CreateBars_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(NumberOfBarsTextBox.Text, out int numberOfBars))
            {
                ProgressBarContainer.Children.Clear();
                progressBars = new List<ProgressBar>();

                for (int i = 0; i < numberOfBars; i++)
                {
                    ProgressBar progressBar = new ProgressBar();
                    progressBar.Minimum = 0;
                    progressBar.Maximum = 100;
                    progressBar.Value = 0;
                    progressBar.Height = 20;
                    progressBar.Margin = new Thickness(5);
                    ProgressBarContainer.Children.Add(progressBar);
                    progressBars.Add(progressBar);
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid number.");
            }
        }

        private async void StartDancing_Click(object sender, RoutedEventArgs e)
        {
            cancellationTokenSource = new CancellationTokenSource();
            await DanceProgressBarsAsync(cancellationTokenSource.Token);
        }

        private void StopDancing_Click(object sender, RoutedEventArgs e)
        {
            cancellationTokenSource?.Cancel();
        }

        private async Task DanceProgressBarsAsync(CancellationToken cancellationToken)
        {
            Random random = new Random();

            while (!cancellationToken.IsCancellationRequested)
            {
                foreach (ProgressBar progressBar in progressBars)
                {
                    if (cancellationToken.IsCancellationRequested)
                        break;

                    int value = random.Next(0, 101);
                    progressBar.Value = value;

                    progressBar.Foreground = new SolidColorBrush(Color.FromRgb((byte)random.Next(256), (byte)random.Next(256), (byte)random.Next(256)));

                    await Task.Delay(500);
                }
            }
        }
    }
}
