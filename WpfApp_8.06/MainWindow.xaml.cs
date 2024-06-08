using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp_8._06
{
    public partial class MainWindow : Window
    {
        private CancellationTokenSource cancellationTokenSource;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BrowseSourceFile_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                SourceFilePathTextBox.Text = openFileDialog.FileName;
            }
        }

        private void BrowseDestination_Click(object sender, RoutedEventArgs e)
        {
            var folderDialog = new Microsoft.Win32.SaveFileDialog();
            if (folderDialog.ShowDialog() == true)
            {
                DestinationPathTextBox.Text = folderDialog.FileName;
            }
        }

        private async void StartCopy_Click(object sender, RoutedEventArgs e)
        {
            string sourceFilePath = SourceFilePathTextBox.Text;
            string destinationPath = DestinationPathTextBox.Text;

            if (!File.Exists(sourceFilePath))
            {
                MessageBox.Show("Source file does not exist!");
                return;
            }

            cancellationTokenSource = new CancellationTokenSource();

            try
            {
                await Task.Run(() => CopyFile(sourceFilePath, destinationPath, cancellationTokenSource.Token));
                MessageBox.Show("File copied successfully!");
            }
            catch (OperationCanceledException)
            {
                MessageBox.Show("Copying stopped.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void StopCopy_Click(object sender, RoutedEventArgs e)
        {
            cancellationTokenSource?.Cancel();
        }

        private void CancelCopy_Click(object sender, RoutedEventArgs e)
        {
            cancellationTokenSource?.Cancel();
            ResetUI();
        }

        private void CopyFile(string sourceFilePath, string destinationPath, CancellationToken cancellationToken)
        {
            using (FileStream sourceStream = File.OpenRead(sourceFilePath))
            using (FileStream destinationStream = File.Create(destinationPath))
            {
                long totalLength = sourceStream.Length;
                byte[] buffer = new byte[4096];
                long bytesCopied = 0;
                int bytesRead;

                while ((bytesRead = sourceStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    destinationStream.Write(buffer, 0, bytesRead);
                    bytesCopied += bytesRead;

                    double progress = (double)bytesCopied / totalLength;
                    UpdateProgressBar(progress);
                }
            }
        }

        private void UpdateProgressBar(double value)
        {
            Dispatcher.Invoke(() =>
            {
                ProgressBar.Value = value * 100;
            });
        }

        private void ResetUI()
        {
            Dispatcher.Invoke(() =>
            {
                SourceFilePathTextBox.Text = string.Empty;
                DestinationPathTextBox.Text = string.Empty;
                ProgressBar.Value = 0;
            });
        }
    }
}
