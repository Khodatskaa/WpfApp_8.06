using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp_8._06
{
    public partial class MainWindow : Window
    {
        private CancellationTokenSource cancellationTokenSource;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BrowseSourceDirectory_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog();
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
            var folderDialog = new Microsoft.Win32.SaveFileDialog();
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

            // Create a new cancellation token source
            cancellationTokenSource = new CancellationTokenSource();

            try
            {
                await Task.Run(() => CopyDirectory(sourceDirectoryPath, destinationPath, numThreads, cancellationTokenSource.Token));
                MessageBox.Show("Directory copied successfully!");
            }
            catch (OperationCanceledException)
            {
                MessageBox.Show("Copying paused or stopped.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void PauseCopy_Click(object sender, RoutedEventArgs e)
        {
            // Pause the copying process
            cancellationTokenSource?.Cancel();
        }

        private void CancelCopy_Click(object sender, RoutedEventArgs e)
        {
            // Cancel the copying process and reset UI
            cancellationTokenSource?.Cancel();
            ResetUI();
        }

        private void CopyDirectory(string sourceDirectoryPath, string destinationPath, int numThreads, CancellationToken cancellationToken)
        {
            var sourceDirectory = new DirectoryInfo(sourceDirectoryPath);
            var destinationDirectory = new DirectoryInfo(destinationPath);

            if (!destinationDirectory.Exists)
            {
                destinationDirectory.Create();
            }

            var files = sourceDirectory.GetFiles();
            Parallel.ForEach(files, new ParallelOptions { MaxDegreeOfParallelism = numThreads, CancellationToken = cancellationToken }, file =>
            {
                string filePath = Path.Combine(destinationPath, file.Name);
                file.CopyTo(filePath, true);
                UpdateProgressBar(1.0 / files.Length);
            });

            var directories = sourceDirectory.GetDirectories();
            foreach (var directory in directories)
            {
                string directoryPath = Path.Combine(destinationPath, directory.Name);
                CopyDirectory(directory.FullName, directoryPath, numThreads, cancellationToken);
            }
        }

        private void UpdateProgressBar(double value)
        {
            Dispatcher.Invoke(() =>
            {
                ProgressBar.Value += value * 100;
            });
        }

        private void ResetUI()
        {
            Dispatcher.Invoke(() =>
            {
                SourceDirectoryPathTextBox.Text = string.Empty;
                DestinationPathTextBox.Text = string.Empty;
                ProgressBar.Value = 0;
            });
        }
    }
}
