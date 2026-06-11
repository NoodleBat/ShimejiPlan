namespace ShimejiPlan124325
{
    public partial class FocusSettingsFormcs : Form
    {
        public FocusSettings Settings { get; private set; }
        public FocusSettingsFormcs(FocusSettings current)
        {
            InitializeComponent();

            Settings = new FocusSettings
            {
                AnimationFolder = current.AnimationFolder,
                FrameIntervalMs = current.FrameIntervalMs,
                WorkDurationMin = current.WorkDurationMin,
                BreakDurationMin = current.BreakDurationMin,
                BackgroundImagePath = current.BackgroundImagePath,
                BackgroundColor = current.BackgroundColor,
                MusicFilePath = current.MusicFilePath,
                MusicVolume = current.MusicVolume,
                EnableMusic = current.EnableMusic
            };
            txtAnimationFolder.Text = Settings.AnimationFolder;// Привязки
            nudFrameInterval.Value = Settings.FrameIntervalMs;
            nudWorkMinutes.Value = Settings.WorkDurationMin;
            nudBreakMinutes.Value = Settings.BreakDurationMin;
            txtBackgroundImage.Text = Settings.BackgroundImagePath;
            rbColor.Checked = string.IsNullOrEmpty(Settings.BackgroundImagePath);
            rbImage.Checked = !rbColor.Checked;
            txtMusicFile.Text = Settings.MusicFilePath;
            cbEnableMusic.Checked = Settings.EnableMusic;
            tbVolume.Value = Settings.MusicVolume;
            lblVolume.Text = $"{tbVolume.Value}%";
        }

        private void FocusSettingsFormcs_Load(object sender, EventArgs e)
        {

        }
        // Анимация
        private void btnBrowseAnimation_Click(object sender, EventArgs e)
        {
            using (var dlg = new FolderBrowserDialog())
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                    txtAnimationFolder.Text = dlg.SelectedPath;
            }
        }
        // Фон
        private void btnBrowseBackground_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog { Filter = "Изображения|*.jpg;*.jpeg;*.png;*.bmp" })
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                    txtBackgroundImage.Text = dlg.FileName;
            }
        }
        private void btnPickColor_Click(object sender, EventArgs e)
        {
            using (var dlg = new ColorDialog { Color = Settings.BackgroundColor })
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                    Settings.BackgroundColor = dlg.Color;
            }
        }
        // музыка
        private void btnBrowseMusic_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog { Filter = "Аудио|*.mp3;*.wav;*.ogg" })
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                    txtMusicFile.Text = dlg.FileName;
            }
        }
        private void tbVolume_Scroll(object sender, EventArgs e)
        {
            lblVolume.Text = $"{tbVolume.Value}%";
        }
        // базовые кнопки
        private void btnOK_Click(object sender, EventArgs e)
        {
            Settings.AnimationFolder = txtAnimationFolder.Text;// сохранение
            Settings.FrameIntervalMs = (int)nudFrameInterval.Value;
            Settings.WorkDurationMin = (int)nudWorkMinutes.Value;
            Settings.BreakDurationMin = (int)nudBreakMinutes.Value;
            Settings.BackgroundImagePath = rbImage.Checked ? txtBackgroundImage.Text : null;
            // BackgroundColor уже задан через диалог
            Settings.MusicFilePath = txtMusicFile.Text;
            Settings.EnableMusic = cbEnableMusic.Checked;
            Settings.MusicVolume = tbVolume.Value;

            // Валидация
            if (!string.IsNullOrEmpty(Settings.AnimationFolder) && !Directory.Exists(Settings.AnimationFolder))
            {
                MessageBox.Show("Указанная папка с анимацией не существует.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
