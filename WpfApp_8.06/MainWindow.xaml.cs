using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp_8._06
{
    public partial class MainWindow : Window
    {
        private List<Horse> horses;

        public MainWindow()
        {
            InitializeComponent();
            InitializeHorses();
        }

        private void InitializeHorses()
        {
            horses = new List<Horse>();

            for (int i = 1; i <= 5; i++)
            {
                Horse horse = new Horse($"Horse {i}");
                ProgressBar progressBar = new ProgressBar();
                progressBar.Minimum = 0;
                progressBar.Maximum = 100;
                progressBar.Value = 0;
                progressBar.Height = 20;
                progressBar.Margin = new Thickness(5);
                ProgressBarContainer.Children.Add(progressBar);
                horse.ProgressBar = progressBar;
                horses.Add(horse);
            }
        }

        private async void StartRace_Click(object sender, RoutedEventArgs e)
        {
            ResultsDataGrid.Items.Clear();

            List<Task> raceTasks = new List<Task>();

            foreach (Horse horse in horses)
            {
                raceTasks.Add(horse.RunAsync());
            }

            await Task.WhenAll(raceTasks);

            horses.Sort((h1, h2) => h1.Position.CompareTo(h2.Position));

            for (int i = 0; i < horses.Count; i++)
            {
                ResultsDataGrid.Items.Add(new { HorseName = horses[i].Name, Position = i + 1 });
            }
        }
    }

    public class Horse
    {
        private static readonly Random random = new Random();
        private const int RaceLength = 100;

        public string Name { get; }
        public ProgressBar ProgressBar { get; set; }
        public int Position { get; set; }

        public Horse(string name)
        {
            Name = name;
        }

        public async Task RunAsync()
        {
            await Task.Run(() =>
            {
                for (int i = 0; i < RaceLength; i++)
                {
                    if (random.Next(1, 101) <= 50) 
                    {
                        Position++;
                        UpdateProgressBar();
                    }

                    Thread.Sleep(random.Next(50, 200)); 
                }
            });
        }

        private void UpdateProgressBar()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                ProgressBar.Value = Position;
            });
        }
    }
}
