using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace ColorGame
{
    public partial class MainWindow : Window
    {
        Game game;
        DispatcherTimer timer;
        TimeSpan refreshRate = TimeSpan.FromTicks(30);
        int prevScore = 0;
        int startSize = 3;
        double viewRatio = 0.66;
        public MainWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = refreshRate;
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (game.LevelUp())
                flash(System.Windows.Media.Colors.CornflowerBlue);
            else if (game.GetScore() > prevScore)
                flash(System.Windows.Media.Colors.Green);
            else if (game.GetScore() < prevScore)
                flash(System.Windows.Media.Colors.Red);
            
            prevScore = game.GetScore();
            game.Draw(sourceGrid);
            timeLabel.Content = "Time Elapsed: " + (DateTime.Now - game.startTime).ToString(@"mm\:ss");
            scoreLabel.Content = "Score: " + game.GetScoreString();
            stageLabel.Content = "Stage " + (game.size - 2);
        }

        private void flash(System.Windows.Media.Color color)
        {
            Dispatcher.Invoke(() =>
            {
                mainBorder.Background = hexToBrush("#333");
                System.Windows.Media.Animation.ColorAnimation animation = new System.Windows.Media.Animation.ColorAnimation();
                animation.From = color;
                animation.To = hexToBrush("#333").Color;
                animation.Duration = new Duration(TimeSpan.FromSeconds(1));
                mainBorder.Background.BeginAnimation(System.Windows.Media.SolidColorBrush.ColorProperty, animation);
            });
        }

        private System.Windows.Media.SolidColorBrush hexToBrush(string hex)
        {
            return (System.Windows.Media.SolidColorBrush)new System.Windows.Media.BrushConverter().ConvertFromString(hex);
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            game = new Game(startSize, (int)(this.Width * viewRatio), refreshRate);
            timer.Start();
        }

        private void none_Click(object sender, RoutedEventArgs e)
        {
            game.DrawSolution(targetGrid);
            timer.Stop();
        }

        private void Window_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(e.Source == this)
                this.WindowState = this.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();

            // dispose
            timer.Stop();
            timer = null;
            game = null;

            // probably make the game dispose as well if you care

        }

        private void restartBtn_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            game = new Game(startSize, (int)(this.Width * viewRatio), refreshRate);
            game.HideSolution(targetGrid);
            timer.Start();
        }
    }
}
