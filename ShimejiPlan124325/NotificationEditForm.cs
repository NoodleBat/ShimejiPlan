using ShimejiPlan124325.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShimejiPlan124325
{
    public partial class NotificationEditForm : Form
    {
        public ScheduledNotification Notification { get; private set; }
        public NotificationEditForm()
        {
            InitializeComponent();
        }

        private void NotificationEditForm_Load(object sender, EventArgs e)
        {

        }
        public NotificationEditForm(List<string> mascotNames)
        {
            InitializeComponent();
            // Заполняем список маскотов: «Любой активный» + имена из хранилища
            cmbMascot.Items.Add("Любой активный");
            cmbMascot.Items.AddRange(mascotNames.ToArray());
            cmbMascot.SelectedIndex = 0;

            cmbRepeat.Items.AddRange(new[] { "Один раз", "Ежедневно", "Еженедельно" });
            cmbRepeat.SelectedIndex = 0;

            dtpTime.Value = DateTime.Now.AddMinutes(1); // по умолчанию через минуту
        }
        public NotificationEditForm(ScheduledNotification existing, List<string> mascotNames)
            : this(mascotNames)
        {
            dtpTime.Value = existing.Time;
            txtMessage.Text = existing.Message;
            cmbMascot.SelectedItem = existing.TargetMascotName;
            cmbRepeat.SelectedIndex = (int)existing.Repeat;
            Notification = existing;
        }

        private void Okbtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMessage.Text))
            {
                MessageBox.Show("Введите текст уведомления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (Notification == null)
                Notification = new ScheduledNotification();

            Notification.Time = dtpTime.Value;
            Notification.Message = txtMessage.Text.Trim();
            Notification.TargetMascotName = cmbMascot.SelectedItem?.ToString() ?? "Любой активный";
            Notification.Repeat = (RepeatMode)cmbRepeat.SelectedIndex;
            Notification.Executed = false;

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
