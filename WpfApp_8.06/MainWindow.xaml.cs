using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp_8._06
{
    public partial class MainWindow : Window
    {
        private Thread numberThread;
        private Thread letterThread;
        private Thread symbolThread;
        private bool isRunning;  

        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            isRunning = true;

            numberThread = new Thread(GenerateNumbers)
            {
                Priority = GetPriority(NumberPriorityComboBox.Text)
            };

            letterThread = new Thread(GenerateLetters)
            {
                Priority = GetPriority(LetterPriorityComboBox.Text)
            };

            symbolThread = new Thread(GenerateSymbols)
            {
                Priority = GetPriority(SymbolPriorityComboBox.Text)
            };

            numberThread.Start();
            letterThread.Start();
            symbolThread.Start();

            StartButton.IsEnabled = false;
            StopButton.IsEnabled = true;
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            isRunning = false;

            numberThread?.Join();
            letterThread?.Join();
            symbolThread?.Join();

            StartButton.IsEnabled = true;
            StopButton.IsEnabled = false;
        }

        private void GenerateNumbers()
        {
            Random random = new Random();
            while (isRunning)
            {
                int number = random.Next(0, 100);
                Dispatcher.Invoke(() => OutputTextBox.AppendText($"{number} "));
                Thread.Sleep(1000);
            }
        }

        private void GenerateLetters()
        {
            Random random = new Random();
            while (isRunning)
            {
                char letter = (char)random.Next('A', 'Z' + 1);
                Dispatcher.Invoke(() => OutputTextBox.AppendText($"{letter} "));
                Thread.Sleep(1000);
            }
        }

        private void GenerateSymbols()
        {
            char[] symbols = { '!', '@', '#', '$', '%', '^', '&', '*', '(', ')' };
            Random random = new Random();
            while (isRunning)
            {
                char symbol = symbols[random.Next(symbols.Length)];
                Dispatcher.Invoke(() => OutputTextBox.AppendText($"{symbol} "));
                Thread.Sleep(1000);
            }
        }

        private ThreadPriority GetPriority(string priority)
        {
            return priority switch
            {
                "Lowest" => ThreadPriority.Lowest,
                "BelowNormal" => ThreadPriority.BelowNormal,
                "Normal" => ThreadPriority.Normal,
                "AboveNormal" => ThreadPriority.AboveNormal,
                "Highest" => ThreadPriority.Highest,
                _ => ThreadPriority.Normal
            };
        }
    }
}