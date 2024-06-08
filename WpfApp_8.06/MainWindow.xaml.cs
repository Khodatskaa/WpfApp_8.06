using Microsoft.Win32;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp_8._06
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BrowseSourceDirectory_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.ValidateNames = false;
            openFileDialog.CheckFileExists = false;
            openFileDialog.CheckPathExists = true;
            openFileDialog.FileName = "Select Folder";

            if (openFileDialog.ShowDialog() == true)
            {
                string path = Path.GetDirectoryName(openFileDialog.FileName);
                SourceDirectoryPathTextBox.Text = path;
            }
        }

        private void BrowseDestination_Click(object sender, RoutedEventArgs e)
        {
            var folderDialog = new SaveFileDialog();
            folderDialog.ValidateNames = false;
            folderDialog.CheckFileExists = false;
            folderDialog.CheckPathExists = true;
            folderDialog.FileName = "Select Folder";

            if (folderDialog.ShowDialog() == true)
            {
                string path = Path.GetDirectoryName(folderDialog.FileName);
                DestinationPathTextBox.Text = path;
            }
        }

        private async void StartCopy_Click(object sender, RoutedEventArgs e)
        {
            string sourceDirectoryPath = SourceDirectoryPathTextBox.Text;
            string destinationPath = DestinationPathTextBox.Text;
            int numThreads = int.Parse(((ComboBoxItem)ThreadsComboBox.SelectedItem).Content.ToString());

            if (!Directory.Exists(sourceDirectoryPath))
            {
                MessageBox.Show("Source directory does not exist!");
                return;
            }

            try
            {
                await Task.Run(() => CopyDirectory(sourceDirectoryPath, destinationPath, numThreads));
                MessageBox.Show("Directory copied successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void CopyDirectory(string sourceDirectoryPath, string destinationPath, int numThreads)
        {
            var sourceDirectory = new DirectoryInfo(sourceDirectoryPath);
            var destinationDirectory = new DirectoryInfo(destinationPath);

            if (!destinationDirectory.Exists)
            {
                destinationDirectory.Create();
            }

            var files = sourceDirectory.GetFiles();
            Parallel.ForEach(files, new ParallelOptions { MaxDegreeOfParallelism = numThreads }, file =>
            {
                string filePath = Path.Combine(destinationPath, file.Name);
                file.CopyTo(filePath, true);
                UpdateProgressBar(1.0 / files.Length);
            });

            var directories = sourceDirectory.GetDirectories();
            foreach (var directory in directories)
            {
                string directoryPath = Path.Combine(destinationPath, directory.Name);
                CopyDirectory(directory.FullName, directoryPath, numThreads);
            }
        }

        private void UpdateProgressBar(double value)
        {
            Dispatcher.Invoke(() =>
            {
                ProgressBar.Value += value * 100;
            });
        }
    }
}
