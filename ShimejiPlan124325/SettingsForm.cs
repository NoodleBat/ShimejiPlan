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

namespace ShimejiPlan124325
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
            cbAutostart.Checked = AutostartService.IsAutostartEnabled();
        }

        private void SettingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            AutostartService.SetAutostart(cbAutostart.Checked);
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {

        }
    }
}
