namespace ShimejiPlan124325
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            tabControl1 = new TabControl();
            tabPageMascots = new TabPage();
            panel1 = new Panel();
            Deletebtn = new Button();
            Stopbtn = new Button();
            Runbtn = new Button();
            Addbtn = new Button();
            dgvMascots = new DataGridView();
            tabPage2 = new TabPage();
            DeleteNotbtn = new Button();
            EditNotbtn = new Button();
            AddNotbtn = new Button();
            dgvNotifications = new DataGridView();
            timerCheck = new System.Windows.Forms.Timer(components);
            tabControl1.SuspendLayout();
            tabPageMascots.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvMascots).BeginInit();
            tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvNotifications).BeginInit();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPageMascots);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Location = new Point(12, 12);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(765, 426);
            tabControl1.TabIndex = 0;
            // 
            // tabPageMascots
            // 
            tabPageMascots.Controls.Add(panel1);
            tabPageMascots.Controls.Add(dgvMascots);
            tabPageMascots.Location = new Point(4, 29);
            tabPageMascots.Name = "tabPageMascots";
            tabPageMascots.Padding = new Padding(3);
            tabPageMascots.Size = new Size(757, 393);
            tabPageMascots.TabIndex = 0;
            tabPageMascots.Text = "tabPage1";
            tabPageMascots.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            panel1.Controls.Add(Deletebtn);
            panel1.Controls.Add(Stopbtn);
            panel1.Controls.Add(Runbtn);
            panel1.Controls.Add(Addbtn);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(3, 265);
            panel1.Name = "panel1";
            panel1.Size = new Size(751, 125);
            panel1.TabIndex = 1;
            // 
            // Deletebtn
            // 
            Deletebtn.Location = new Point(529, 0);
            Deletebtn.Name = "Deletebtn";
            Deletebtn.Size = new Size(187, 119);
            Deletebtn.TabIndex = 3;
            Deletebtn.Text = "Удалить";
            Deletebtn.UseVisualStyleBackColor = true;
            Deletebtn.Click += Deletebtn_Click;
            // 
            // Stopbtn
            // 
            Stopbtn.Location = new Point(322, 0);
            Stopbtn.Name = "Stopbtn";
            Stopbtn.Size = new Size(187, 119);
            Stopbtn.TabIndex = 2;
            Stopbtn.Text = "Стоп";
            Stopbtn.UseVisualStyleBackColor = true;
            Stopbtn.Click += Stopbtn_Click;
            // 
            // Runbtn
            // 
            Runbtn.Location = new Point(155, 0);
            Runbtn.Name = "Runbtn";
            Runbtn.Size = new Size(161, 119);
            Runbtn.TabIndex = 1;
            Runbtn.Text = "Запустить";
            Runbtn.UseVisualStyleBackColor = true;
            Runbtn.Click += Runbtn_Click;
            // 
            // Addbtn
            // 
            Addbtn.Location = new Point(3, 3);
            Addbtn.Name = "Addbtn";
            Addbtn.Size = new Size(146, 119);
            Addbtn.TabIndex = 0;
            Addbtn.Text = "Добавить";
            Addbtn.UseVisualStyleBackColor = true;
            Addbtn.Click += Addbtn_Click;
            // 
            // dgvMascots
            // 
            dgvMascots.AllowUserToAddRows = false;
            dgvMascots.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvMascots.Dock = DockStyle.Top;
            dgvMascots.Location = new Point(3, 3);
            dgvMascots.MultiSelect = false;
            dgvMascots.Name = "dgvMascots";
            dgvMascots.ReadOnly = true;
            dgvMascots.RowHeadersWidth = 51;
            dgvMascots.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMascots.Size = new Size(751, 181);
            dgvMascots.TabIndex = 0;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(DeleteNotbtn);
            tabPage2.Controls.Add(EditNotbtn);
            tabPage2.Controls.Add(AddNotbtn);
            tabPage2.Controls.Add(dgvNotifications);
            tabPage2.Location = new Point(4, 29);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(757, 393);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "tabPage2";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // DeleteNotbtn
            // 
            DeleteNotbtn.Location = new Point(380, 286);
            DeleteNotbtn.Name = "DeleteNotbtn";
            DeleteNotbtn.Size = new Size(94, 29);
            DeleteNotbtn.TabIndex = 3;
            DeleteNotbtn.Text = "Удалить ";
            DeleteNotbtn.UseVisualStyleBackColor = true;
            DeleteNotbtn.Click += DeleteNotbtn_Click;
            // 
            // EditNotbtn
            // 
            EditNotbtn.Location = new Point(225, 282);
            EditNotbtn.Name = "EditNotbtn";
            EditNotbtn.Size = new Size(94, 29);
            EditNotbtn.TabIndex = 2;
            EditNotbtn.Text = "Изменить";
            EditNotbtn.UseVisualStyleBackColor = true;
            EditNotbtn.Click += EditNotbtn_Click;
            // 
            // AddNotbtn
            // 
            AddNotbtn.Location = new Point(83, 282);
            AddNotbtn.Name = "AddNotbtn";
            AddNotbtn.Size = new Size(94, 29);
            AddNotbtn.TabIndex = 1;
            AddNotbtn.Text = "Добавить";
            AddNotbtn.UseVisualStyleBackColor = true;
            AddNotbtn.Click += AddNotbtn_Click;
            // 
            // dgvNotifications
            // 
            dgvNotifications.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvNotifications.Location = new Point(0, 0);
            dgvNotifications.Name = "dgvNotifications";
            dgvNotifications.RowHeadersWidth = 51;
            dgvNotifications.Size = new Size(757, 188);
            dgvNotifications.TabIndex = 0;
            // 
            // timerCheck
            // 
            timerCheck.Interval = 30000;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(tabControl1);
            Name = "MainForm";
            Text = "MainForm";
            tabControl1.ResumeLayout(false);
            tabPageMascots.ResumeLayout(false);
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvMascots).EndInit();
            tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvNotifications).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControl1;
        private TabPage tabPageMascots;
        private TabPage tabPage2;
        private Panel panel1;
        private DataGridView dgvMascots;
        private Button Stopbtn;
        private Button Runbtn;
        private Button Addbtn;
        private Button Deletebtn;
        private DataGridView dgvNotifications;
        private Button AddNotbtn;
        private Button EditNotbtn;
        private Button DeleteNotbtn;
        private System.Windows.Forms.Timer timerCheck;
    }
}
