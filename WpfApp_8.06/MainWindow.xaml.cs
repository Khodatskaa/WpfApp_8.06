using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        private async void SearchWordInDirectory_Click(object sender, RoutedEventArgs e)
        {
            string directoryPath = DirectoryPathTextBox.Text;
            string word = WordTextBox.Text;

            if (string.IsNullOrWhiteSpace(directoryPath) || string.IsNullOrWhiteSpace(word))
            {
                MessageBox.Show("Please enter both the directory path and the word.");
                return;
            }

            if (!Directory.Exists(directoryPath))
            {
                MessageBox.Show("Directory does not exist.");
                return;
            }

            ResultsListBox.Items.Clear();
            await SearchWordInDirectoryAsync(directoryPath, word);
        }

        private async Task SearchWordInDirectoryAsync(string directoryPath, string word)
        {
            await Task.Run(() =>
            {
                IEnumerable<string> files = Directory.EnumerateFiles(directoryPath, "*", SearchOption.AllDirectories);

                foreach (string file in files)
                {
                    int count = SearchWordInFile(file, word);
                    DisplayResult(file, count);
                }
            });
        }

        private int SearchWordInFile(string filePath, string word)
        {
            string content = File.ReadAllText(filePath);
            Regex regex = new Regex("\\b" + word + "\\b", RegexOptions.IgnoreCase);
            MatchCollection matches = regex.Matches(content);
            return matches.Count;
        }

        private void DisplayResult(string filePath, int count)
        {
            string fileName = Path.GetFileName(filePath);
            string directoryName = Path.GetDirectoryName(filePath);
            string result = $"File name: {fileName}\nFile path: {directoryName}\nNumber of word occurrences: {count}\n";
            ResultsListBox.Items.Add(result);
        }
    }
}