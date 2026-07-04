using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace 对比视频播放器
{
    public partial class VideoPlayerControl : UserControl
    {
        private bool _isDragging = false;
        private DispatcherTimer _timer;
        private bool _isPlaying = false;
        private double _brightness = 0;
        private double _volume = 100;
        private DispatcherTimer _feedbackTimer;

        public string? FilePath { get; private set; }

        public bool IsPlaying => _isPlaying;

        public double Position
        {
            get => MediaPlayer.Position.TotalSeconds;
            set
            {
                if (!_isDragging && MediaPlayer.NaturalDuration.HasTimeSpan)
                {
                    MediaPlayer.Position = TimeSpan.FromSeconds(value);
                }
            }
        }

        public double Duration => MediaPlayer.NaturalDuration.HasTimeSpan ? MediaPlayer.NaturalDuration.TimeSpan.TotalSeconds : 0;

        public event EventHandler? PlayStateChanged;
        public event EventHandler? PositionChanged;

        public VideoPlayerControl()
        {
            InitializeComponent();
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(100);
            _timer.Tick += Timer_Tick;
            
            _feedbackTimer = new DispatcherTimer();
            _feedbackTimer.Interval = TimeSpan.FromSeconds(1);
            _feedbackTimer.Tick += FeedbackTimer_Tick;
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (!_isDragging && MediaPlayer.NaturalDuration.HasTimeSpan)
            {
                var progress = (MediaPlayer.Position.TotalSeconds / MediaPlayer.NaturalDuration.TimeSpan.TotalSeconds) * 100;
                ProgressSlider.Value = progress;
                PositionChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private void FeedbackTimer_Tick(object? sender, EventArgs e)
        {
            FeedbackOverlay.Visibility = Visibility.Hidden;
            _feedbackTimer.Stop();
        }

        private void ShowFeedback(string text)
        {
            FeedbackText.Text = text;
            FeedbackOverlay.Visibility = Visibility.Visible;
            _feedbackTimer.Stop();
            _feedbackTimer.Start();
        }

        private void MediaPlayer_MediaOpened(object sender, RoutedEventArgs e)
        {
            EmptyState.Visibility = Visibility.Collapsed;
            BottomControls.Opacity = 1;
        }

        private void MediaPlayer_MediaEnded(object sender, RoutedEventArgs e)
        {
            _isPlaying = false;
            _timer.Stop();
            PlayStateChanged?.Invoke(this, EventArgs.Empty);
        }

        private void MediaPlayer_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            EmptyState.Visibility = Visibility.Visible;
            BottomControls.Opacity = 0;
        }

        public void OpenFile(string filePath)
        {
            FilePath = filePath;
            MediaPlayer.Source = new Uri(filePath, UriKind.Absolute);
            EmptyState.Visibility = Visibility.Collapsed;
            BottomControls.Opacity = 1;
        }

        public void Play()
        {
            MediaPlayer.Play();
            _isPlaying = true;
            _timer.Start();
            PlayStateChanged?.Invoke(this, EventArgs.Empty);
        }

        public void Pause()
        {
            MediaPlayer.Pause();
            _isPlaying = false;
            _timer.Stop();
            PlayStateChanged?.Invoke(this, EventArgs.Empty);
        }

        public void Stop()
        {
            MediaPlayer.Stop();
            _isPlaying = false;
            ProgressSlider.Value = 0;
            _timer.Stop();
            PlayStateChanged?.Invoke(this, EventArgs.Empty);
        }

        public void TogglePlayPause()
        {
            if (_isPlaying)
            {
                Pause();
            }
            else
            {
                Play();
            }
        }

        public void SeekForward(double seconds)
        {
            if (MediaPlayer.NaturalDuration.HasTimeSpan)
            {
                var newPosition = MediaPlayer.Position.TotalSeconds + seconds;
                var maxPosition = MediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                MediaPlayer.Position = TimeSpan.FromSeconds(Math.Min(newPosition, maxPosition));
            }
        }

        public void SeekBackward(double seconds)
        {
            var newPosition = Math.Max(0, MediaPlayer.Position.TotalSeconds - seconds);
            MediaPlayer.Position = TimeSpan.FromSeconds(newPosition);
        }

        private void ProgressSlider_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            _isDragging = true;
        }

        private void ProgressSlider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            _isDragging = false;
            if (MediaPlayer.NaturalDuration.HasTimeSpan)
            {
                var position = TimeSpan.FromSeconds((ProgressSlider.Value / 100) * MediaPlayer.NaturalDuration.TimeSpan.TotalSeconds);
                MediaPlayer.Position = position;
            }
        }

        private void ProgressSlider_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!_isDragging && MediaPlayer.NaturalDuration.HasTimeSpan)
            {
                var position = TimeSpan.FromSeconds((ProgressSlider.Value / 100) * MediaPlayer.NaturalDuration.TimeSpan.TotalSeconds);
                MediaPlayer.Position = position;
            }
        }

        private void UserControl_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (EmptyState.Visibility == Visibility.Visible)
            {
                var openFileDialog = new Microsoft.Win32.OpenFileDialog();
                openFileDialog.Filter = "视频文件 (*.mp4;*.avi;*.mkv;*.mov;*.wmv;*.flv)|*.mp4;*.avi;*.mkv;*.mov;*.wmv;*.flv|所有文件 (*.*)|*.*";
                if (openFileDialog.ShowDialog() == true)
                {
                    OpenFile(openFileDialog.FileName);
                }
            }
        }

        private void UserControl_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            if (EmptyState.Visibility == Visibility.Visible)
                return;

            e.Handled = true;
            var mousePos = e.GetPosition(this);
            var halfWidth = ActualWidth / 2;

            if (mousePos.X < halfWidth)
            {
                _brightness += e.Delta > 0 ? 5 : -5;
                _brightness = Math.Clamp(_brightness, -50, 50);
                
                if (_brightness >= 0)
                {
                    BrightnessOverlay.Opacity = _brightness / 100;
                    DarknessOverlay.Opacity = 0;
                }
                else
                {
                    DarknessOverlay.Opacity = -_brightness / 100;
                    BrightnessOverlay.Opacity = 0;
                }
                
                ShowFeedback($"亮度: {100 + _brightness}%");
            }
            else
            {
                _volume += e.Delta > 0 ? 5 : -5;
                _volume = Math.Clamp(_volume, 0, 100);
                MediaPlayer.Volume = _volume / 100;
                ShowFeedback($"音量: {_volume}%");
            }
        }

        private void UserControl_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (EmptyState.Visibility == Visibility.Visible)
                return;
            
            if (e.GetPosition(this).Y > ActualHeight - 50)
            {
                BottomControls.Opacity = 1;
            }
            else
            {
                BottomControls.Opacity = 0;
            }
        }
    }
}