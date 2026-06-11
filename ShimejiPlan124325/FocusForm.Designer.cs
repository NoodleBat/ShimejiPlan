namespace ShimejiPlan124325
{
    partial class FocusForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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
            panelControls = new Panel();
            btnCloseFocus = new Button();
            panelCntrl = new Panel();
            Settingsbtn = new Button();
            lblTimer = new Label();
            lblState = new Label();
            btnReset = new Button();
            btnPause = new Button();
            btnStart = new Button();
            panelControls.SuspendLayout();
            panelCntrl.SuspendLayout();
            SuspendLayout();
            // 
            // panelControls
            // 
            panelControls.Controls.Add(panelCntrl);
            panelControls.Dock = DockStyle.Fill;
            panelControls.Location = new Point(0, 0);
            panelControls.Name = "panelControls";
            panelControls.Size = new Size(475, 594);
            panelControls.TabIndex = 0;
            panelControls.Paint += panelControls_Paint;
            // 
            // btnCloseFocus
            // 
            btnCloseFocus.BackColor = Color.Transparent;
            btnCloseFocus.Cursor = Cursors.Hand;
            btnCloseFocus.FlatStyle = FlatStyle.Popup;
            btnCloseFocus.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCloseFocus.ForeColor = Color.Firebrick;
            btnCloseFocus.Location = new Point(325, 7);
            btnCloseFocus.Name = "btnCloseFocus";
            btnCloseFocus.Size = new Size(36, 31);
            btnCloseFocus.TabIndex = 1;
            btnCloseFocus.Text = "✕";
            btnCloseFocus.UseVisualStyleBackColor = false;
            btnCloseFocus.Click += btnCloseFocus_Click;
            // 
            // panelCntrl
            // 
            panelCntrl.AccessibleRole = AccessibleRole.None;
            panelCntrl.Controls.Add(btnCloseFocus);
            panelCntrl.Controls.Add(Settingsbtn);
            panelCntrl.Controls.Add(lblTimer);
            panelCntrl.Controls.Add(lblState);
            panelCntrl.Controls.Add(btnReset);
            panelCntrl.Controls.Add(btnPause);
            panelCntrl.Controls.Add(btnStart);
            panelCntrl.Dock = DockStyle.Bottom;
            panelCntrl.Location = new Point(0, 550);
            panelCntrl.Name = "panelCntrl";
            panelCntrl.Size = new Size(475, 44);
            panelCntrl.TabIndex = 0;
            // 
            // Settingsbtn
            // 
            Settingsbtn.Location = new Point(225, 9);
            Settingsbtn.Name = "Settingsbtn";
            Settingsbtn.Size = new Size(94, 29);
            Settingsbtn.TabIndex = 5;
            Settingsbtn.Text = "Настройки";
            Settingsbtn.UseVisualStyleBackColor = true;
            Settingsbtn.Click += Settingsbtn_Click;
            // 
            // lblTimer
            // 
            lblTimer.AutoSize = true;
            lblTimer.Location = new Point(413, 12);
            lblTimer.Name = "lblTimer";
            lblTimer.Size = new Size(50, 20);
            lblTimer.TabIndex = 4;
            lblTimer.Text = "label1";
            // 
            // lblState
            // 
            lblState.AutoSize = true;
            lblState.Location = new Point(357, 12);
            lblState.Name = "lblState";
            lblState.Size = new Size(50, 20);
            lblState.TabIndex = 3;
            lblState.Text = "label1";
            // 
            // btnReset
            // 
            btnReset.Font = new Font("Sylfaen", 7.8F, FontStyle.Bold, GraphicsUnit.Point, 204);
            btnReset.Location = new Point(151, 9);
            btnReset.Name = "btnReset";
            btnReset.Size = new Size(68, 29);
            btnReset.TabIndex = 2;
            btnReset.Text = "Сброс";
            btnReset.UseVisualStyleBackColor = true;
            btnReset.Click += btnReset_Click;
            // 
            // btnPause
            // 
            btnPause.Font = new Font("Sylfaen", 7.8F, FontStyle.Bold, GraphicsUnit.Point, 204);
            btnPause.Location = new Point(77, 9);
            btnPause.Name = "btnPause";
            btnPause.Size = new Size(68, 29);
            btnPause.TabIndex = 1;
            btnPause.Text = "Пауза";
            btnPause.UseVisualStyleBackColor = true;
            btnPause.Click += btnPause_Click;
            // 
            // btnStart
            // 
            btnStart.Font = new Font("Sylfaen", 7.8F, FontStyle.Bold, GraphicsUnit.Point, 204);
            btnStart.Location = new Point(3, 9);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(68, 29);
            btnStart.TabIndex = 0;
            btnStart.Text = "Старт";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click;
            // 
            // FocusForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(475, 594);
            Controls.Add(panelControls);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FocusForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FocusForm";
            TopMost = true;
            panelControls.ResumeLayout(false);
            panelCntrl.ResumeLayout(false);
            panelCntrl.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelControls;
        private Panel panelCntrl;
        private Button btnPause;
        private Button btnStart;
        private Label lblTimer;
        private Label lblState;
        private Button btnReset;
        private Button btnCloseFocus;
        private Button Settingsbtn;
    }
}
