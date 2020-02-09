using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Windows.Controls.Primitives;
using Microsoft.Win32;
using System.Globalization;

namespace WpfApp3
{
    public partial class MainWindow : Window
    {
        private bool mediaPlayerIsPlaying = false;
        private bool userIsDraggingSlider = false;
        private bool isVisibleInterface = true;
        private bool isFullScreen = false;

        public MainWindow()
        {
            InitializeComponent();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            if ((mePlayer.Source != null) && (mePlayer.NaturalDuration.HasTimeSpan) && (!userIsDraggingSlider))
            {
                sliProgress.Minimum = 0;
                sliProgress.Maximum = mePlayer.NaturalDuration.TimeSpan.TotalSeconds;
                sliProgress.Value = mePlayer.Position.TotalSeconds;
            }
        }

        private void Open_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Media files (*.mp3;*.mp4;*.mpg;*.mpeg)|*.mp3;*.mp4;*.mpg;*.mpeg|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
                mePlayer.Source = new Uri(openFileDialog.FileName);
        }

        private void Play_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (mePlayer != null) && (mePlayer.Source != null);
        }

        private void Play_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            mePlayer.Play();
            mediaPlayerIsPlaying = true;
        }

        private void Pause_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = mediaPlayerIsPlaying;
        }

        private void Pause_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            mePlayer.Pause();
        }

        private void Stop_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = mediaPlayerIsPlaying;
        }

        private void Stop_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            mePlayer.Stop();
            mediaPlayerIsPlaying = false;
        }

        private void sliProgress_DragStarted(object sender, DragStartedEventArgs e)
        {
            userIsDraggingSlider = true;
        }

        private void sliProgress_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            userIsDraggingSlider = false;
            mePlayer.Position = TimeSpan.FromSeconds(sliProgress.Value);
        }

        private void sliProgress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            lblProgressStatus.Text = TimeSpan.FromSeconds(sliProgress.Value).ToString(@"hh\:mm\:ss");
        }

        private void Grid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            mePlayer.Volume += (e.Delta > 0) ? 0.1 : -0.1;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FullScreen();
        }
        private void FullScreen()
        {
            if (isFullScreen == false)
            {
                mePlayer.Stretch = Stretch.Fill;
                GridLength gridLength = new GridLength(35, GridUnitType.Star);
                RowDisplay.RowDefinitions[0].Height = gridLength;
                WindowState = WindowState.Maximized;
                isFullScreen = true;
            }
            else
            {
                mePlayer.Stretch = Stretch.None;
                GridLength gridLength = new GridLength(20, GridUnitType.Star);
                RowDisplay.RowDefinitions[0].Height = gridLength;
                WindowState = WindowState.Normal;
                isFullScreen = false;
            }
        }
        private void HideInterface()
        {
            if (isVisibleInterface == true)
            {
                GridLength gridLength = new GridLength(0, GridUnitType.Star);
                RowDisplay.RowDefinitions[1].Height = gridLength;
                StatusBarName.Visibility = Visibility.Collapsed;
                WindowName.WindowStyle = WindowStyle.None;
                isVisibleInterface = false;
            }
            else
            {
                GridLength gridLength = new GridLength(2, GridUnitType.Star);
                RowDisplay.RowDefinitions[1].Height = gridLength;
                StatusBarName.Visibility = Visibility.Visible;
                WindowName.WindowStyle = WindowStyle.SingleBorderWindow;
                isVisibleInterface = true;

            }
        }
        private void KeyDownPause(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.X)
            {
                if (mediaPlayerIsPlaying == true)
                {
                    mePlayer.Pause();
                    mediaPlayerIsPlaying = false;
                }
                else
                {
                    mePlayer.Play();
                    mediaPlayerIsPlaying = true;
                }
            }
            else if (e.Key == Key.Z)
            {
                mePlayer.Position -= TimeSpan.FromSeconds(5);
            }
            else if (e.Key == Key.C)
            {
                mePlayer.Position += TimeSpan.FromSeconds(5);
            }
            else if (e.Key == Key.W)
            {
                HideInterface();
            }
            else if (e.Key == Key.F)
            {
                FullScreen();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            HideInterface();
        }
    }
}
