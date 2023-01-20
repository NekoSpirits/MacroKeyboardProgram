using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MacroKeyboardProgram
{
    public partial class frmMini : Form
    {
        public frmMini()
        {
            InitializeComponent();
        }

        private void frmMini_Load(object sender, EventArgs e)
        {
            this.Size = new Size(Image.FromFile(Properties.Settings.Default.Backgrounds).Size.Width + 17, Image.FromFile(Properties.Settings.Default.Backgrounds).Size.Height + 40);
            this.BackgroundImage = Image.FromFile(Properties.Settings.Default.Backgrounds);

            List<string> lines = new List<string>();
            int count = 1;
            foreach (string str in File.ReadAllLines(Properties.Settings.Default.fileDirectory + @"\Save1.txt"))
            {

                string[] strSplit = str.Split(';');
                Button b = new Button();
                b.Name = strSplit[1];
                b.Text = strSplit[0];
                b.Size = new Size(int.Parse(strSplit[3]), int.Parse(strSplit[2]));
                b.Location = new Point(int.Parse(strSplit[4]), int.Parse(strSplit[5]) - 6);
                //Console.WriteLine(int.Parse(strSplit[2]) + " " + int.Parse(strSplit[3]));
                //Console.WriteLine(int.Parse(strSplit[4]) + " " + int.Parse(strSplit[5]));
                //Console.WriteLine("ERROR:" + strSplit[6] + "!");
                if (File.Exists(strSplit[6]))
                {
                    using (FileStream fs = new FileStream(strSplit[6], FileMode.Open))
                    {
                        b.BackgroundImage = Image.FromStream(fs);
                        fs.Close();
                    }
                    //b.BackgroundImage = Image.FromFile(strSplit[6]);
                }
                b.BackgroundImageLayout = ImageLayout.Stretch;
                b.FlatStyle = FlatStyle.Flat;
                //b.ContextMenuStrip = ctxtEdit;
                b.Tag = "macro";

                this.Controls.Add(b);
                b.BringToFront();
                //b.MouseDown += button_dragMouseDown;
                //b.MouseMove += button_dragMouseMove;

                b.Click += button_click;


                //MessageBox.Show(b.Name.ToString());
                count++;
            }
        }
        private void button_click(object sender, EventArgs e)
        {
            //if (chkLock.Checked == true)
            //{
                Console.WriteLine("Start: " + Properties.Settings.Default.fileDirectory + @"\Macros\" + ((Button)sender).Name + ".ahk");
                if (File.Exists(Properties.Settings.Default.fileDirectory + @"\Macros\" + ((Button)sender).Name + ".ahk"))
                {
                    Process.Start(Properties.Settings.Default.fileDirectory + @"\Macros\" + ((Button)sender).Name + ".ahk");
                }
                //MessageBox.Show(((Button)sender).Name);
            //}
        }

        //private void frmMini_ResizeEnd(object sender, EventArgs e)
        //{
        //    imageA = Image.FromFile(Properties.Settings.Default.Backgrounds);
            

        //    double imageASize = imageA.Size.Height * imageA.Size.Width;
        //    double imageBSize = Size.Height * Size.Width;
        //    Console.WriteLine(imageASize + " " + imageBSize);

        //    string ratio = string.Format("Image B is {0}% of the size of image A",
        //      ((imageBSize / imageASize) * 100).ToString("#0"));
        //    Console.WriteLine(ratio);
        //    Console.WriteLine(imageBSize / imageASize);
        //    double percentRatio = imageBSize / imageASize;
        //    foreach (Button b in Controls)
        //    {

        //        //Console.WriteLine("X: " + int.Parse((b.Size.Width * (imageBSize / imageASize)).ToString()) + "Y: " + int.Parse((b.Size.Height * (imageBSize / imageASize)).ToString()));
        //        //b.Size = new Size(int.Parse((b.Size.Width * (imageBSize / imageASize)).ToString()), int.Parse((b.Size.Height * (imageBSize / imageASize)).ToString()));
        //        int x = int.Parse(Math.Round(b.Size.Width * percentRatio).ToString());
        //        int y = int.Parse(Math.Round(b.Size.Height * percentRatio).ToString());
        //        int xloc = int.Parse(Math.Round(b.Location.X * percentRatio).ToString());
        //        int yloc = int.Parse(Math.Round(b.Location.Y * percentRatio).ToString());
        //        b.Size = new Size(x, y);
        //        b.Location = new Point(xloc, yloc)
        //    }
        //}
        //Image imageA;
        //Image imageB;
        //private void frmMini_ResizeBegin(object sender, EventArgs e)
        //{
        //    imageA = this.BackgroundImage;
        //}
    }
}
