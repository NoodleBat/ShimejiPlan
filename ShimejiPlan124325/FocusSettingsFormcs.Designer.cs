namespace ShimejiPlan124325
{
    partial class FocusSettingsFormcs
    {
        private System.ComponentModel.IContainer components = null;

        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            txtAnimationFolder = new TextBox();
            btnBrowseAnimation = new Button();
            label1 = new Label();
            nudFrameInterval = new NumericUpDown();
            groupBox1 = new GroupBox();
            label2 = new Label();
            groupBox2 = new GroupBox();
            btnBrowseBackground = new Button();
            txtBackgroundImage = new TextBox();
            btnPickColor = new Button();
            rbImage = new RadioButton();
            rbColor = new RadioButton();
            groupBox3 = new GroupBox();
            nudBreakMinutes = new NumericUpDown();
            label4 = new Label();
            nudWorkMinutes = new NumericUpDown();
            label3 = new Label();
            btnOK = new Button();
            btnCancel = new Button();
            groupBox4 = new GroupBox();
            lblVolume = new Label();
            tbVolume = new TrackBar();
            label5 = new Label();
            btnBrowseMusic = new Button();
            txtMusicFile = new TextBox();
            cbEnableMusic = new CheckBox();
            ((System.ComponentModel.ISupportInitialize)nudFrameInterval).BeginInit();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudBreakMinutes).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudWorkMinutes).BeginInit();
            groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)tbVolume).BeginInit();
            SuspendLayout();
            // 
            // txtAnimationFolder
            // 
            txtAnimationFolder.Location = new Point(208, 68);
            txtAnimationFolder.Name = "txtAnimationFolder";
            txtAnimationFolder.ReadOnly = true;
            txtAnimationFolder.Size = new Size(169, 26);
            txtAnimationFolder.TabIndex = 0;
            // 
            // btnBrowseAnimation
            // 
            btnBrowseAnimation.Location = new Point(208, 111);
            btnBrowseAnimation.Name = "btnBrowseAnimation";
            btnBrowseAnimation.Size = new Size(169, 29);
            btnBrowseAnimation.TabIndex = 1;
            btnBrowseAnimation.Text = "Обзор...";
            btnBrowseAnimation.UseVisualStyleBackColor = true;
            btnBrowseAnimation.Click += btnBrowseAnimation_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(16, 22);
            label1.Name = "label1";
            label1.Size = new Size(186, 18);
            label1.TabIndex = 2;
            label1.Text = "Скорость кадров (мс):";
            label1.Click += label1_Click;
            // 
            // nudFrameInterval
            // 
            nudFrameInterval.Location = new Point(208, 20);
            nudFrameInterval.Name = "nudFrameInterval";
            nudFrameInterval.Size = new Size(169, 26);
            nudFrameInterval.TabIndex = 3;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(btnBrowseAnimation);
            groupBox1.Controls.Add(nudFrameInterval);
            groupBox1.Controls.Add(txtAnimationFolder);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(427, 155);
            groupBox1.TabIndex = 4;
            groupBox1.TabStop = false;
            groupBox1.Text = "Анимация";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(122, 76);
            label2.Name = "label2";
            label2.Size = new Size(55, 18);
            label2.TabIndex = 4;
            label2.Text = "Папка";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(btnBrowseBackground);
            groupBox2.Controls.Add(txtBackgroundImage);
            groupBox2.Controls.Add(btnPickColor);
            groupBox2.Controls.Add(rbImage);
            groupBox2.Controls.Add(rbColor);
            groupBox2.Location = new Point(12, 172);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(427, 125);
            groupBox2.TabIndex = 5;
            groupBox2.TabStop = false;
            groupBox2.Text = "Фон";
            // 
            // btnBrowseBackground
            // 
            btnBrowseBackground.Location = new Point(205, 96);
            btnBrowseBackground.Name = "btnBrowseBackground";
            btnBrowseBackground.Size = new Size(169, 29);
            btnBrowseBackground.TabIndex = 4;
            btnBrowseBackground.Text = "Обзор...";
            btnBrowseBackground.UseVisualStyleBackColor = true;
            // 
            // txtBackgroundImage
            // 
            txtBackgroundImage.Location = new Point(208, 56);
            txtBackgroundImage.Name = "txtBackgroundImage";
            txtBackgroundImage.ReadOnly = true;
            txtBackgroundImage.Size = new Size(166, 26);
            txtBackgroundImage.TabIndex = 3;
            // 
            // btnPickColor
            // 
            btnPickColor.Location = new Point(205, 17);
            btnPickColor.Name = "btnPickColor";
            btnPickColor.Size = new Size(169, 29);
            btnPickColor.TabIndex = 2;
            btnPickColor.Text = "Выбрать цвет";
            btnPickColor.UseVisualStyleBackColor = true;
            // 
            // rbImage
            // 
            rbImage.AutoSize = true;
            rbImage.Location = new Point(24, 56);
            rbImage.Name = "rbImage";
            rbImage.Size = new Size(136, 22);
            rbImage.TabIndex = 1;
            rbImage.TabStop = true;
            rbImage.Text = "Изображение";
            rbImage.UseVisualStyleBackColor = true;
            // 
            // rbColor
            // 
            rbColor.AutoSize = true;
            rbColor.Location = new Point(24, 20);
            rbColor.Name = "rbColor";
            rbColor.Size = new Size(150, 22);
            rbColor.TabIndex = 0;
            rbColor.TabStop = true;
            rbColor.Text = "Сплошной цвет";
            rbColor.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(nudBreakMinutes);
            groupBox3.Controls.Add(label4);
            groupBox3.Controls.Add(nudWorkMinutes);
            groupBox3.Controls.Add(label3);
            groupBox3.Location = new Point(15, 303);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(424, 125);
            groupBox3.TabIndex = 6;
            groupBox3.TabStop = false;
            groupBox3.Text = "Таймер";
            // 
            // nudBreakMinutes
            // 
            nudBreakMinutes.Location = new Point(203, 58);
            nudBreakMinutes.Name = "nudBreakMinutes";
            nudBreakMinutes.Size = new Size(171, 26);
            nudBreakMinutes.TabIndex = 3;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(51, 60);
            label4.Name = "label4";
            label4.Size = new Size(145, 18);
            label4.TabIndex = 2;
            label4.Text = "Перерыв (минут):";
            // 
            // nudWorkMinutes
            // 
            nudWorkMinutes.Location = new Point(202, 20);
            nudWorkMinutes.Name = "nudWorkMinutes";
            nudWorkMinutes.Size = new Size(169, 26);
            nudWorkMinutes.TabIndex = 1;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(65, 22);
            label3.Name = "label3";
            label3.Size = new Size(131, 18);
            label3.TabIndex = 0;
            label3.Text = "Работа (минут):";
            // 
            // btnOK
            // 
            btnOK.DialogResult = DialogResult.OK;
            btnOK.Location = new Point(502, 284);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(121, 65);
            btnOK.TabIndex = 8;
            btnOK.Text = "Ок";
            btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new Point(675, 284);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(121, 65);
            btnCancel.TabIndex = 9;
            btnCancel.Text = "Отмена";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(lblVolume);
            groupBox4.Controls.Add(tbVolume);
            groupBox4.Controls.Add(label5);
            groupBox4.Controls.Add(btnBrowseMusic);
            groupBox4.Controls.Add(txtMusicFile);
            groupBox4.Controls.Add(cbEnableMusic);
            groupBox4.Location = new Point(458, 12);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(428, 174);
            groupBox4.TabIndex = 10;
            groupBox4.TabStop = false;
            groupBox4.Text = "Музыка";
            // 
            // lblVolume
            // 
            lblVolume.AutoSize = true;
            lblVolume.Location = new Point(145, 122);
            lblVolume.Name = "lblVolume";
            lblVolume.Size = new Size(37, 18);
            lblVolume.TabIndex = 8;
            lblVolume.Text = "50%";
            // 
            // tbVolume
            // 
            tbVolume.Location = new Point(190, 112);
            tbVolume.Name = "tbVolume";
            tbVolume.Size = new Size(169, 56);
            tbVolume.TabIndex = 7;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(44, 122);
            label5.Name = "label5";
            label5.Size = new Size(95, 18);
            label5.TabIndex = 6;
            label5.Text = "Громкость:";
            // 
            // btnBrowseMusic
            // 
            btnBrowseMusic.Location = new Point(190, 66);
            btnBrowseMusic.Name = "btnBrowseMusic";
            btnBrowseMusic.Size = new Size(169, 29);
            btnBrowseMusic.TabIndex = 5;
            btnBrowseMusic.Text = "Обзор...";
            btnBrowseMusic.UseVisualStyleBackColor = true;
            // 
            // txtMusicFile
            // 
            txtMusicFile.Location = new Point(190, 25);
            txtMusicFile.Name = "txtMusicFile";
            txtMusicFile.ReadOnly = true;
            txtMusicFile.Size = new Size(169, 26);
            txtMusicFile.TabIndex = 1;
            // 
            // cbEnableMusic
            // 
            cbEnableMusic.AutoSize = true;
            cbEnableMusic.Location = new Point(22, 27);
            cbEnableMusic.Name = "cbEnableMusic";
            cbEnableMusic.Size = new Size(162, 22);
            cbEnableMusic.TabIndex = 0;
            cbEnableMusic.Text = "Включить музыку";
            cbEnableMusic.UseVisualStyleBackColor = true;
            // 
            // FocusSettingsFormcs
            // 
            AutoScaleDimensions = new SizeF(9F, 18F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(896, 460);
            Controls.Add(groupBox4);
            Controls.Add(btnCancel);
            Controls.Add(btnOK);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Font = new Font("Century Gothic", 9F, FontStyle.Bold);
            ForeColor = Color.FromArgb(64, 0, 0);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "FocusSettingsFormcs";
            StartPosition = FormStartPosition.WindowsDefaultBounds;
            Text = "Настройки Фокусировки";
            WindowState = FormWindowState.Minimized;
            Load += FocusSettingsFormcs_Load;
            ((System.ComponentModel.ISupportInitialize)nudFrameInterval).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudBreakMinutes).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudWorkMinutes).EndInit();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)tbVolume).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TextBox txtAnimationFolder;
        private Button btnBrowseAnimation;
        private Label label1;
        private NumericUpDown nudFrameInterval;
        private GroupBox groupBox1;
        private Label label2;
        private GroupBox groupBox2;
        private RadioButton rbImage;
        private RadioButton rbColor;
        private Button btnPickColor;
        private Button btnBrowseBackground;
        private TextBox txtBackgroundImage;
        private GroupBox groupBox3;
        private Label label4;
        private NumericUpDown nudWorkMinutes;
        private Label label3;
        private NumericUpDown nudBreakMinutes;
        private Button btnOK;
        private Button btnCancel;
        private GroupBox groupBox4;
        private CheckBox cbEnableMusic;
        private Label lblVolume;
        private TrackBar tbVolume;
        private Label label5;
        private Button btnBrowseMusic;
        private TextBox txtMusicFile;
    }
}
