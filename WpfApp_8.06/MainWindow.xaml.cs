using System;
using System.IO;
using System.Text.RegularExpressions;
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

        private async void SearchWord_Click(object sender, RoutedEventArgs e)
        {
            string filePath = FilePathTextBox.Text;
            string word = WordTextBox.Text;

            if (string.IsNullOrWhiteSpace(filePath) || string.IsNullOrWhiteSpace(word))
            {
                MessageBox.Show("Please enter both the file path and the word.");
                return;
            }

            if (!File.Exists(filePath))
            {
                MessageBox.Show("File does not exist.");
                return;
            }

            int count = await Task.Run(() => SearchWordInFile(filePath, word));
            ResultTextBlock.Text = $"The word \"{word}\" was found {count} times in the file.";
        }

        private int SearchWordInFile(string filePath, string word)
        {
            string content = File.ReadAllText(filePath);
            Regex regex = new Regex("\\b" + word + "\\b", RegexOptions.IgnoreCase);
            MatchCollection matches = regex.Matches(content);
            return matches.Count;
        }
    }
}