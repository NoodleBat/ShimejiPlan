using NAudio.Wave;
using ShimejiPlan124325.Models;
using ShimejiPlan124325.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ShimejiPlan124325.Properties;


namespace ShimejiPlan124325
{
    public partial class FocusForm : Form
    {
        // panelControls - это панель для анимации, както промахнулась пока переименовывала править на другое назване лень
        private FocusSettings _settings;
        private FocusTimer _focusTimer;
        private System.Windows.Forms.Timer _animTimer;
        private List<Image> _frames;
        private int _currentFrame;

        // Музыка
        private WaveOutEvent? _waveOut;
        private AudioFileReader? _audioFile;
        public FocusForm(FocusSettings settings)
        {
            InitializeComponent();
            _settings = settings;
            _frames = new List<Image>();

            LoadFrames();
            ApplyBackground();

            _animTimer = new System.Windows.Forms.Timer(); // явная инициализация
            SetupAnimationTimer();

            _focusTimer = new FocusTimer(_settings);
            _focusTimer.Tick += OnTimerTick;
            _focusTimer.StateChanged += OnStateChanged;

            UpdateStateDisplay(FocusState.Idle);
            lblTimer.Text = "00:00";

            panelControls.Paint += PanelCanvas_Paint;// Подписка на Paint панели

            this.KeyPreview = true; // Кнопка закрытия (Escape)
            this.KeyDown += (s, e) => { if (e.KeyCode == Keys.Escape) this.Close(); };
        }

        private void LoadFrames()
        {
            foreach (var img in _frames) img.Dispose();
            _frames.Clear();

            // Пытаемся загрузить пользовательскую анимацию
            if (!string.IsNullOrEmpty(_settings.AnimationFolder) && Directory.Exists(_settings.AnimationFolder))
            {
                var files = Directory.GetFiles(_settings.AnimationFolder, "*.png")
                                     .OrderBy(f => f, new NaturalStringComparer())
                                     .ToList();
                foreach (var file in files)
                {
                    try { _frames.Add(Image.FromFile(file)); }
                    catch { }
                }
            }

            // Если пользовательские кадры не найдены — используем встроенные
            if (_frames.Count == 0)
            {
                // Добавляем кадры из ресурсов (можно сколько угодно)
                _frames.Add(Resources.DefaultFocusAnimation1);
                if (Resources.DefaultFocusAnimation2 != null)
                    _frames.Add(Resources.DefaultFocusAnimation2);
                // Можно потом расширить
            }

            _currentFrame = 0;
        }
        private void ApplyBackground() // отрисовка картинок и фона
        {
            // Пытаемся использовать пользовательский фон
            if (!string.IsNullOrEmpty(_settings.BackgroundImagePath) && File.Exists(_settings.BackgroundImagePath))
            {
                panelControls.BackgroundImage = Image.FromFile(_settings.BackgroundImagePath);
                panelControls.BackgroundImageLayout = ImageLayout.Stretch;
            }
            // Если нет — встроенное фоновое изображение
            else if (Properties.Resources.DefaultBackGround != null)
            {
                panelControls.BackgroundImage = Properties.Resources.DefaultBackGround;
                panelControls.BackgroundImageLayout = ImageLayout.Stretch;
            }
            // Совсем ничего нет — заливаем цветом из настроек
            else
            {
                panelControls.BackgroundImage = null;
                panelControls.BackColor = _settings.BackgroundColor;
            }
        }
        private void SetupAnimationTimer()
        {
            _animTimer.Interval = _settings.FrameIntervalMs;
            _animTimer.Tick += (s, e) =>
            {
                if (_frames.Count > 0)
                {
                    _currentFrame = (_currentFrame + 1) % _frames.Count;
                    panelControls.Invalidate();
                }
            };
        }
        private void PanelCanvas_Paint(object? sender, PaintEventArgs e)         // анимации отрисовка
        {
            if (_frames.Count > 0 && _currentFrame < _frames.Count)
            {
                Image img = _frames[_currentFrame];
                int x = (panelControls.Width - img.Width) / 2;
                int y = (panelControls.Height - img.Height) / 2;
                e.Graphics.DrawImage(img, x, y, img.Width, img.Height);
            }
        }
        private void OnTimerTick(TimeSpan remaining, FocusState state)
        {
            if (this.InvokeRequired)
                this.Invoke((Action)(() => lblTimer.Text = $"{remaining.Minutes:D2}:{remaining.Seconds:D2}"));
            else
                lblTimer.Text = $"{remaining.Minutes:D2}:{remaining.Seconds:D2}";
        }

        private void OnStateChanged(FocusState state)
        {
            if (this.InvokeRequired)
                this.Invoke((Action)(() => UpdateStateDisplay(state)));
            else
                UpdateStateDisplay(state);
        }

        private void UpdateStateDisplay(FocusState state)
        {
            switch (state)
            {
                case FocusState.Idle:
                    lblState.Text = "Ожидание";
                    lblState.ForeColor = Color.Gray;
                    break;
                case FocusState.Working:
                    lblState.Text = "Работа";
                    lblState.ForeColor = Color.OrangeRed;
                    break;
                case FocusState.OnBreak:
                    lblState.Text = "Перерыв";
                    lblState.ForeColor = Color.ForestGreen;
                    break;
            }
        }

        private void panelControls_Paint(object sender, PaintEventArgs e)
        {

        }

        // музыка
        internal void PlayMusic()
        {
            if (!_settings.EnableMusic || string.IsNullOrEmpty(_settings.MusicFilePath)) return;
            StopMusic();
            _audioFile = new AudioFileReader(_settings.MusicFilePath) { Volume = _settings.MusicVolume / 100f };
            _waveOut = new WaveOutEvent();
            _waveOut.Init(_audioFile);
            _waveOut.PlaybackStopped += (s, e) => { if (_focusTimer.State != FocusState.Idle) PlayMusic(); }; // зацикливание
            _waveOut.Play();
        }

        internal void StopMusic()
        {
            _waveOut?.Stop();
            _waveOut?.Dispose();
            _audioFile?.Dispose();
            _waveOut = null;
            _audioFile = null;
        }

        internal void PauseMusic() => _waveOut?.Pause();
        internal void ResumeMusic() => _waveOut?.Play();

        // кнопки
        private void btnCloseFocus_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void btnStart_Click(object sender, EventArgs e)//старт
        {
            if (_focusTimer.State == FocusState.Idle)
            {
                _focusTimer.StartWork();
                _animTimer.Start();
                PlayMusic();
            }
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            _focusTimer.Pause();
            _animTimer.Stop();
            _waveOut?.Pause();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            _focusTimer.Reset();
            _animTimer.Stop();
            _currentFrame = 0;
            panelControls.Invalidate();
            StopMusic();
            UpdateStateDisplay(FocusState.Idle);
            lblTimer.Text = "00:00";
        }

        private void Settingsbtn_Click(object sender, EventArgs e)
        {
            using (var settingsForm = new FocusSettingsFormcs(_settings))
            {
                if (settingsForm.ShowDialog() == DialogResult.OK)
                {
                    _settings = settingsForm.Settings;
                    FocusSettingsStorage.Save(_settings);

                    LoadFrames();
                    ApplyBackground();
                    _animTimer.Interval = _settings.FrameIntervalMs;
                    if (_waveOut != null)
                    {
                        StopMusic();
                        PlayMusic();
                    }

                    panelControls.Invalidate();
                }
            }
        }
    }
}
