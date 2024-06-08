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

        private async void AnalyzeText_Click(object sender, RoutedEventArgs e)
        {
            string text = TextInputTextBox.Text;

            VowelsTextBlock.Text = "Counting vowels...";
            ConsonantsTextBlock.Text = "Counting consonants...";
            SymbolsTextBlock.Text = "Counting symbols...";

            try
            {
                int vowelsCount = await Task.Run(() => CountVowels(text));
                VowelsTextBlock.Text = $"Vowels: {vowelsCount}";

                int consonantsCount = await Task.Run(() => CountConsonants(text));
                ConsonantsTextBlock.Text = $"Consonants: {consonantsCount}";

                int symbolsCount = await Task.Run(() => CountSymbols(text));
                SymbolsTextBlock.Text = $"Symbols: {symbolsCount}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private int CountVowels(string text)
        {
            int count = 0;
            foreach (char c in text)
            {
                if ("AEIOUaeiou".Contains(c))
                {
                    count++;
                }
            }
            return count;
        }

        private int CountConsonants(string text)
        {
            int count = 0;
            foreach (char c in text)
            {
                if (char.IsLetter(c) && !"AEIOUaeiou".Contains(c))
                {
                    count++;
                }
            }
            return count;
        }

        private int CountSymbols(string text)
        {
            int count = 0;
            foreach (char c in text)
            {
                if (!char.IsLetterOrDigit(c) && !char.IsWhiteSpace(c))
                {
                    count++;
                }
            }
            return count;
        }
    }
}