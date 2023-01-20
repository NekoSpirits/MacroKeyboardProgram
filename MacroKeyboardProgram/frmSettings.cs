using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MacroKeyboardProgram
{
    public partial class frmSettings : Form
    {
        public frmSettings()
        {
            InitializeComponent();
        }

        private void chkHasu_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void frmSettings_Load(object sender, EventArgs e)
        {
            chkHasu.Checked = Properties.Settings.Default.isHasu;
            pictureBox1.Image = Image.FromFile(Properties.Settings.Default.Backgrounds);
        }

        private void chkHasu_Click(object sender, EventArgs e)
        {
            if (chkHasu.Checked == true)
            {
                DialogResult butt = MessageBox.Show("Only use this setting if you are using a Hasu USB converter and second keyboard, otherwise your hotkeys will not work.", "Do you know what you are doing?", MessageBoxButtons.OKCancel);
                switch (butt)
                {
                    case DialogResult.Cancel:
                        chkHasu.Checked = false;
                        break;

                    case DialogResult.OK:
                        Properties.Settings.Default.isHasu = true;
                        Properties.Settings.Default.Save();
                        break;
                }
            }
            else
            {
                Properties.Settings.Default.isHasu = false;
                Properties.Settings.Default.Save();
            }
        }

        private void btnLoadImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog oFile = new OpenFileDialog();
            oFile.ShowDialog();
            Properties.Settings.Default.Backgrounds = oFile.FileName;
            Properties.Settings.Default.Save();
            pictureBox1.Image = Image.FromFile(Properties.Settings.Default.Backgrounds);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Backgrounds = "";
            Properties.Settings.Default.Save();
            pictureBox1.Image = null;
        }
    }
}
