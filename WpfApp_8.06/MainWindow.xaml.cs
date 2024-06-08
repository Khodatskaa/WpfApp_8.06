using System;
using System.Numerics;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp_8._06
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void CountFibonacci_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(LimitTextBox.Text, out int limit) && limit >= 0)
            {
                BigInteger result = await Task.Run(() => CountFibonacci(limit));
                ResultTextBlock.Text = $"Number of Fibonacci numbers up to {limit}: {result}";
            }
            else
            {
                MessageBox.Show("Please enter a valid non-negative integer.");
            }
        }

        private BigInteger CountFibonacci(int limit)
        {
            if (limit == 0)
                return 0;

            BigInteger a = 0, b = 1, c;
            BigInteger count = 2; 

            while ((c = a + b) <= limit)
            {
                count++;
                a = b;
                b = c;
            }

            return count;
        }
    }
}