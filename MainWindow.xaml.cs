using System;
using System.Windows;

namespace 对比视频播放器
{
    public partial class MainWindow : Window
    {
        private bool _isSyncing = false;

        public MainWindow()
        {
            InitializeComponent();
            
            LeftPlayer.PositionChanged += Player_PositionChanged;
            RightPlayer.PositionChanged += Player_PositionChanged;
        }

        private void Player_PositionChanged(object? sender, EventArgs e)
        {
            if (_isSyncing) return;
            
            var sourcePlayer = sender as VideoPlayerControl;
            if (sourcePlayer == null) return;
            
            _isSyncing = true;
            try
            {
                if (sourcePlayer == LeftPlayer)
                {
                    RightPlayer.Position = LeftPlayer.Position;
                }
                else
                {
                    LeftPlayer.Position = RightPlayer.Position;
                }
            }
            finally
            {
                _isSyncing = false;
            }
        }

        private void SyncPlayButton_Click(object sender, RoutedEventArgs e)
        {
            LeftPlayer.Play();
            RightPlayer.Play();
        }

        private void SyncPauseButton_Click(object sender, RoutedEventArgs e)
        {
            LeftPlayer.Pause();
            RightPlayer.Pause();
        }

        private void SyncStopButton_Click(object sender, RoutedEventArgs e)
        {
            LeftPlayer.Stop();
            RightPlayer.Stop();
        }

        private void SyncResetButton_Click(object sender, RoutedEventArgs e)
        {
            LeftPlayer.Stop();
            RightPlayer.Stop();
            LeftPlayer.Position = 0;
            RightPlayer.Position = 0;
        }

        private void SyncProgressButton_Click(object sender, RoutedEventArgs e)
        {
            double maxDuration = Math.Max(LeftPlayer.Duration, RightPlayer.Duration);
            if (maxDuration > 0)
            {
                double avgPosition = (LeftPlayer.Position + RightPlayer.Position) / 2;
                LeftPlayer.Position = avgPosition;
                RightPlayer.Position = avgPosition;
            }
        }

        private void LeftPlayButton_Click(object sender, RoutedEventArgs e)
        {
            LeftPlayer.TogglePlayPause();
        }

        private void RightPlayButton_Click(object sender, RoutedEventArgs e)
        {
            RightPlayer.TogglePlayPause();
        }

        private void LeftBackward5Button_Click(object sender, RoutedEventArgs e)
        {
            LeftPlayer.SeekBackward(5);
        }

        private void LeftBackward30Button_Click(object sender, RoutedEventArgs e)
        {
            LeftPlayer.SeekBackward(30);
        }

        private void LeftForward5Button_Click(object sender, RoutedEventArgs e)
        {
            LeftPlayer.SeekForward(5);
        }

        private void LeftForward30Button_Click(object sender, RoutedEventArgs e)
        {
            LeftPlayer.SeekForward(30);
        }

        private void RightBackward5Button_Click(object sender, RoutedEventArgs e)
        {
            RightPlayer.SeekBackward(5);
        }

        private void RightBackward30Button_Click(object sender, RoutedEventArgs e)
        {
            RightPlayer.SeekBackward(30);
        }

        private void RightForward5Button_Click(object sender, RoutedEventArgs e)
        {
            RightPlayer.SeekForward(5);
        }

        private void RightForward30Button_Click(object sender, RoutedEventArgs e)
        {
            RightPlayer.SeekForward(30);
        }

        private void LeftSpeedCombo_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
        }

        private void RightSpeedCombo_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
        }

        private void PlaybackRateCombo_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
            }
            else
            {
                WindowState = WindowState.Maximized;
            }
        }

        private void TopmostButton_Click(object sender, RoutedEventArgs e)
        {
            Topmost = !Topmost;
        }

        private void TitleBar_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
        }
    }
}