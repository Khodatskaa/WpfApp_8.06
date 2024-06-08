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

        private async void CalculateFactorial_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(NumberTextBox.Text, out int number))
            {
                if (number < 0)
                {
                    MessageBox.Show("Please enter a non-negative integer.");
                    return;
                }

                CalculateFactorialButton.IsEnabled = false;

                try
                {
                    BigInteger factorial = await Task.Run(() => CalculateFactorial(number));
                    ResultTextBlock.Text = $"Factorial of {number}: {factorial}";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
                finally
                {
                    CalculateFactorialButton.IsEnabled = true;
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid integer.");
            }
        }

        private BigInteger CalculateFactorial(int number)
        {
            if (number == 0)
            {
                return 1;
            }
            else
            {
                return number * CalculateFactorial(number - 1);
            }
        }
    }
}
