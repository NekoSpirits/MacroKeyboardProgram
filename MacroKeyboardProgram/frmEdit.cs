using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Windows.Forms;

namespace MacroKeyboardProgram
{
    public partial class frmEdit : Form
    {
        public string location;
        public string name;
        public string text;
        public Size size;
        public frmEdit(string _name, string _text, string _location, Size _size)
        {
            InitializeComponent();

            location = _location;
            name = _name;
            text = _text;
            size = _size;
        }

        private void btnLoadImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog oFile = new OpenFileDialog();
            oFile.ShowDialog();
            using (FileStream fs = new FileStream(oFile.FileName, FileMode.Open))
            {
                pictureBox1.Image = Image.FromStream(fs);
                fs.Close();
            }


            //name = txtName.Text;

            //button.BackgroundImage.Save(@"D:\Programs\Macro\Images\" + button.Name);
            if (File.Exists(location + @"\Images\" + txtName.Text))
            {
                File.Delete((location + @"\Images\" + txtName.Text));
            }



            //using (FileStream fs = new FileStream(@"D:\Programs\Macro\Images\" + txtName.Text, FileMode.Create))
            //{
            //    Image.FromStream(fs);
            //    fs.Close();
            //}
            //tempImage = pictureBox1.Image;
            //pictureBox1.Image = null;
            if (pictureBox1.Image != null)
            {
                Bitmap bitmap = new Bitmap(pictureBox1.Image);
                bitmap.Save(location + @"\Images\" + txtName.Text);
            }
            NotificationSend();
            //pictureBox1.Image = Image.FromFile(oFile.FileName);
        }

        private void frmEdit_Load(object sender, EventArgs e)
        {
            //Console.WriteLine("'" + text + "'");
            if (text != "")
            {
                txtText.Enabled = true;
                chkText.Checked = true;
            }
            else
            {
                txtText.Enabled = false;
                chkText.Checked = false;
            }
            numHeight.Value = size.Height;
            numWidth.Value = size.Width;

            if (File.Exists(location + @"\Hasu\" + name + ".ahk"))
            {
                using (StreamReader fs = new StreamReader(location + @"\Hasu\" + name + ".ahk"))
                {
                    rtxtHasu.Text = fs.ReadToEnd();
                    fs.Close();
                }
            }

            if (File.Exists(location + @"\HotKeys\" + name + ".ahk"))
            {
                using (StreamReader fs = new StreamReader(location + @"\HotKeys\" + name + ".ahk"))
                {
                    rtxtHotkey.Text = fs.ReadToEnd();
                    fs.Close();
                }
            }

            if (File.Exists(location + @"\Macros\" + name + ".ahk"))
            {
                using (StreamReader fs = new StreamReader(location + @"\Macros\" + name + ".ahk"))
                {
                    rtxtOnClick.Text = fs.ReadToEnd();
                    fs.Close();
                }
            }

            //richTextBox1.Text = File.ReadAllText(location + @"\Macro\" + name + ".ahk");
            txtName.Text = name;
            txtText.Text = text;
            try
            {
                using (FileStream fs = new FileStream(location + @"\Images\" + name, FileMode.Open))
                {
                    pictureBox1.Image = Image.FromStream(fs);
                    fs.Close();
                }
                //pictureBox1.Image = Image.FromFile(image);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        
        public void ReloadAHK()
        {
            string[] lstAHK = Directory.GetFiles(Properties.Settings.Default.fileDirectory + @"\HotKeys\" + ".ahk");

            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            name = txtName.Text;
            //MessageBox.Show(name);
            //if (rtxtHotkey.Text == "")
            //{
            //    if (File.Exists(location + @"\HotKeys\" + name + ".ahk"))
            //    {
            //        File.Delete(location + @"\HotKeys\" + name + ".ahk");
            //    }
            //}
            //else
            //{
                using (StreamWriter fs = new StreamWriter(location + @"\HotKeys\" + name + ".ahk"))
                {
                    fs.Write(rtxtHotkey.Text);
                    fs.Close();
                }
            //}

            //if (rtxtOnClick.Text == "")
            //{
            //    if(File.Exists(location + @"\Macros\" + name + ".ahk"))
            //    {
            //        File.Delete(location + @"\Macros\" + name + ".ahk");
            //    }
            //}
            //else
            //{
            //MessageBox.Show(location + @"\Macros\" + name + ".ahk");
                using (StreamWriter fs = new StreamWriter(location + @"\Macros\" + name + ".ahk"))
                {
                    fs.Write(rtxtOnClick.Text);
                    fs.Close();
                }
            //}

            //if (rtxtHasu.Text == "")
            //{
            //    if(File.Exists(location + @"\Macros\" + name + ".ahk"))
            //    {
            //        File.Delete(location + @"\Macros\" + name + ".ahk");
            //    }
            //}
            //else
            //{
                using (StreamWriter fs = new StreamWriter(location + @"\Hasu\" + name + ".ahk"))
                {
                    fs.Write(rtxtHasu.Text);
                    fs.Close();
                }
            //}

            


            //if (pictureBox1 != null)
            //{
            //    if (File.Exists(location + @"\Images\" + name))
            //    {
            //        File.Delete(location + @"\Images\" + name);
            //    }
            //}
            //Form1 parent = (Form1)Owner;
            NotificationSend();
            //Console.WriteLine(location);
            //MessageBox.Show(rtxtOnClick.Text);
            //parent.NotifyMe(textBox1.Text, name, location, new Size(int.Parse(numWidth.Value.ToString()), int.Parse(numHeight.Value.ToString())));
            //Console.WriteLine("HERE" + name);
            this.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (chkText.Checked == false)
            {
                pictureBox1.Enabled = false;
                txtText.Enabled = false;
            }
            else
            {
                pictureBox1.Enabled = true;
                txtText.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
            NotificationSend();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //name = txtName.Text;
            NotificationSend();
            
        }

        private void NotificationSend()
        {
            Form1 parent = (Form1)Owner;
            bool isText = chkText.Checked;
            //MessageBox.Show(txtName.Text);
            parent.NotifyMe(txtText.Text, txtName.Text, location, new Size(int.Parse(numWidth.Value.ToString()), int.Parse(numHeight.Value.ToString())), isText);

        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            //name = txtName.Text;
            NotificationSend();
        }

        private void txtText_TextChanged(object sender, EventArgs e)
        {
            NotificationSend();
        }

        private void btnClipboard_Click(object sender, EventArgs e)
        {

            pictureBox1.Image = Clipboard.GetImage();



            //name = txtName.Text;

            //button.BackgroundImage.Save(@"D:\Programs\Macro\Images\" + button.Name);
            if (File.Exists(location + @"\Images\" + txtName.Text))
            {
                File.Delete((location + @"\Images\" + txtName.Text));
            }



            //using (FileStream fs = new FileStream(@"D:\Programs\Macro\Images\" + txtName.Text, FileMode.Create))
            //{
            //    Image.FromStream(fs);
            //    fs.Close();
            //}
            //tempImage = pictureBox1.Image;
            //pictureBox1.Image = null;
            if (pictureBox1.Image != null)
            {
                Bitmap bitmap = new Bitmap(pictureBox1.Image);
                bitmap.Save(location + @"\Images\" + txtName.Text);
            }
            NotificationSend();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
