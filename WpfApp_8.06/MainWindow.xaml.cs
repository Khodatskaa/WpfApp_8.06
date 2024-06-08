using System;
using System.IO;
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

            try
            {
                await Task.Run(() => CopyFile(sourceFilePath, destinationPath));
                MessageBox.Show("File copied successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void CopyFile(string sourceFilePath, string destinationPath)
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
    }
}
