namespace ShimejiPlan124325
{
    partial class NotificationEditForm
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
            label1 = new Label();
            dtpTime = new DateTimePicker();
            label2 = new Label();
            txtMessage = new TextBox();
            label3 = new Label();
            label4 = new Label();
            cmbMascot = new ComboBox();
            cmbRepeat = new ComboBox();
            Okbtn = new Button();
            Cancelbtn = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(50, 20);
            label1.TabIndex = 0;
            label1.Text = "label1";
            // 
            // dtpTime
            // 
            dtpTime.CustomFormat = "dd.MM.yyyy HH:mm";
            dtpTime.Enabled = false;
            dtpTime.Location = new Point(171, 12);
            dtpTime.Name = "dtpTime";
            dtpTime.ShowUpDown = true;
            dtpTime.Size = new Size(250, 27);
            dtpTime.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 66);
            label2.Name = "label2";
            label2.Size = new Size(50, 20);
            label2.TabIndex = 2;
            label2.Text = "label2";
            // 
            // txtMessage
            // 
            txtMessage.Location = new Point(107, 63);
            txtMessage.Multiline = true;
            txtMessage.Name = "txtMessage";
            txtMessage.Size = new Size(125, 34);
            txtMessage.TabIndex = 3;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 131);
            label3.Name = "label3";
            label3.Size = new Size(50, 20);
            label3.TabIndex = 4;
            label3.Text = "label3";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 207);
            label4.Name = "label4";
            label4.Size = new Size(50, 20);
            label4.TabIndex = 5;
            label4.Text = "label4";
            // 
            // cmbMascot
            // 
            cmbMascot.FormattingEnabled = true;
            cmbMascot.Location = new Point(128, 133);
            cmbMascot.Name = "cmbMascot";
            cmbMascot.Size = new Size(151, 28);
            cmbMascot.TabIndex = 6;
            // 
            // cmbRepeat
            // 
            cmbRepeat.FormattingEnabled = true;
            cmbRepeat.Items.AddRange(new object[] { "Один раз", "Ежедневно", "Еженедельно" });
            cmbRepeat.Location = new Point(128, 199);
            cmbRepeat.Name = "cmbRepeat";
            cmbRepeat.Size = new Size(151, 28);
            cmbRepeat.TabIndex = 7;
            // 
            // Okbtn
            // 
            Okbtn.Location = new Point(93, 312);
            Okbtn.Name = "Okbtn";
            Okbtn.Size = new Size(94, 29);
            Okbtn.TabIndex = 8;
            Okbtn.Text = "Ок";
            Okbtn.UseVisualStyleBackColor = true;
            Okbtn.Click += Okbtn_Click;
            // 
            // Cancelbtn
            // 
            Cancelbtn.Location = new Point(327, 312);
            Cancelbtn.Name = "Cancelbtn";
            Cancelbtn.Size = new Size(94, 29);
            Cancelbtn.TabIndex = 9;
            Cancelbtn.Text = "Отмена";
            Cancelbtn.UseVisualStyleBackColor = true;
            // 
            // NotificationEditForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(Cancelbtn);
            Controls.Add(Okbtn);
            Controls.Add(cmbRepeat);
            Controls.Add(cmbMascot);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(txtMessage);
            Controls.Add(label2);
            Controls.Add(dtpTime);
            Controls.Add(label1);
            Name = "NotificationEditForm";
            Text = "NotificationEditForm";
            Load += NotificationEditForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private DateTimePicker dtpTime;
        private Label label2;
        private TextBox txtMessage;
        private Label label3;
        private Label label4;
        private ComboBox cmbMascot;
        private ComboBox cmbRepeat;
        private Button Okbtn;
        private Button Cancelbtn;
    }
}