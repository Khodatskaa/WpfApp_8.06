using System;
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

        private async void CalculatePower_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(NumberTextBox.Text, out double number) &&
                double.TryParse(DegreeTextBox.Text, out double degree))
            {
                CalculatePowerButton.IsEnabled = false;

                try
                {
                    double result = await Task.Run(() => CalculatePower(number, degree));
                    ResultTextBlock.Text = $"Result: {result}";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
                finally
                {
                    CalculatePowerButton.IsEnabled = true;
                }
            }
            else
            {
                MessageBox.Show("Please enter valid numbers.");
            }
        }

        private double CalculatePower(double number, double degree)
        {
            return Math.Pow(number, degree);
        }
    }
}
