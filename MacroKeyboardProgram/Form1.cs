using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;

namespace MacroKeyboardProgram
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        float percentage;
        private void Form1_Load(object sender, EventArgs e)
        {
            btnCompile.Tag = "Start";
            if (Properties.Settings.Default.Backgrounds != "")
            {
                pictureBox1.Image = Image.FromFile(Properties.Settings.Default.Backgrounds);
            }
            else
            {
                MessageBox.Show("Please choose a background image.");
                OpenFileDialog ofile = new OpenFileDialog();
                ofile.ShowDialog();
                Properties.Settings.Default.Backgrounds = ofile.FileName;
                Properties.Settings.Default.Save();
                pictureBox1.Image = Image.FromFile(Properties.Settings.Default.Backgrounds);
            }
            //percentage
            //pictureBox1.Size = Image.FromFile(Properties.Settings.Default.Backgrounds).Size;
            //Sets Compile button to the lock state of the form
            LockCheck();
            if (Properties.Settings.Default.fileDirectory != "")
            {
                LoadLastSave();
            }
            else
            {
                frmNewMacro newMacro = new frmNewMacro();
                newMacro.ShowDialog();
                if (newMacro.response == "import")
                {
                    ImportMacroFolder();
                }
                else if (newMacro.response == "new")
                {
                    CreateNewMacroFolder();
                }
            }
            ResizeForm();
        }

        private void ResizeForm()
        {
            if (Properties.Settings.Default.SizeX != 0 || Properties.Settings.Default.SizeY != 0)
            {
                this.Size = new Size(Properties.Settings.Default.SizeX, Properties.Settings.Default.SizeY);
            }
        }

        private void LoadLastSave()
        {
            ClearButtons();

            List<string> lines = new List<string>();
            int count = 1;
            foreach (string str in File.ReadAllLines(Properties.Settings.Default.fileDirectory + @"\Save1.txt"))
            {

                string[] strSplit = str.Split(';');
                Button b = new Button();
                b.Name = strSplit[1];
                b.Text = strSplit[0];
                b.Size = new Size(int.Parse(strSplit[3]), int.Parse(strSplit[2]));
                b.Location = new Point(int.Parse(strSplit[4]) /*+ pictureBox1.Location.X*/, int.Parse(strSplit[5])/* + pictureBox1.Location.Y - 6*/);

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
                b.ContextMenuStrip = ctxtEdit;
                b.Tag = "macro";

                this.Controls.Add(b);
                b.BringToFront();
                b.MouseDown += button_dragMouseDown;
                b.MouseMove += button_dragMouseMove;

                b.Click += button_click;


                //MessageBox.Show(b.Name.ToString());
                count++;
            }
            LockCheck();


        }

        //When creating a new save file for
        private void NewExport()
        {
            if (Properties.Settings.Default.fileDirectory == "")
            {
                FolderBrowserDialog fDiag = new FolderBrowserDialog();
                fDiag.ShowNewFolderButton = true;
                fDiag.ShowDialog();
                Properties.Settings.Default.fileDirectory = fDiag.SelectedPath;
                Properties.Settings.Default.Save();
            }

            FolderCheck();

        }

        private static void FolderCheck()
        {
            if (!Directory.Exists(Properties.Settings.Default.fileDirectory + @"\HotKeys"))
            {
                Directory.CreateDirectory(Properties.Settings.Default.fileDirectory + @"\HotKeys");
            }
            if (!Directory.Exists(Properties.Settings.Default.fileDirectory + @"\Images"))
            {
                Directory.CreateDirectory(Properties.Settings.Default.fileDirectory + @"\Images");
            }
            if (!Directory.Exists(Properties.Settings.Default.fileDirectory + @"\Macros"))
            {
                Directory.CreateDirectory(Properties.Settings.Default.fileDirectory + @"\Macros");
            }
            if (!Directory.Exists(Properties.Settings.Default.fileDirectory + @"\Hasu"))
            {
                Directory.CreateDirectory(Properties.Settings.Default.fileDirectory + @"\Hasu");
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveCurrentFile();
        }

        private void SaveCurrentFile()
        {
            NewExport();
            List<string> lines = new List<string>();
            foreach (Button button in this.Controls.OfType<Button>())
            {

                if (button.Tag != null)
                {
                    if (button.Tag.ToString() == "macro")
                    {
                        if (button.BackgroundImage != null)
                        {
                            button.BackgroundImage.Save(Properties.Settings.Default.fileDirectory + @"\Images\" + button.Name);
                        }
                        lines.Add(button.Text + ";" + button.Name + ";" + button.Size.Height + ";" + button.Size.Width + ";" + (button.Location.X/* - pictureBox1.Location.X*/) + ";" + (button.Location.Y) + ";" + Properties.Settings.Default.fileDirectory + @"\Images\" + button.Name + ";");
                    }

                }
            }
            foreach (string str in lines)
            {
                //MessageBox.Show(str);
            }
            File.WriteAllLines(Properties.Settings.Default.fileDirectory + @"\Save1.txt", lines);
        }

        private void ClearButtons()
        {
            foreach (Button button in this.Controls.OfType<Button>())
            {
                if (button.Tag != null)
                {
                    if (button.Tag.ToString() == "macro")
                    {
                        Controls.Remove(button);
                    }
                }
            }
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImportMacroFolder();
        }

        private void ImportMacroFolder()
        {
            ClearButtons();

            FolderBrowserDialog fDiag = new FolderBrowserDialog();
            fDiag.RootFolder = Environment.SpecialFolder.Desktop;
            if (Properties.Settings.Default.fileDirectory != "")
            {
                fDiag.SelectedPath = Path.GetDirectoryName(Properties.Settings.Default.fileDirectory);
            }
            
            fDiag.ShowDialog();

            List<string> lines = new List<string>();
            int count = 1;
            foreach (string str in File.ReadAllLines(fDiag.SelectedPath + @"\Save1.txt"))
            {

                string[] strSplit = str.Split(';');
                Button b = new Button();
                b.Name = strSplit[1];
                b.Text = strSplit[0];
                b.Size = new Size(int.Parse(strSplit[3]), int.Parse(strSplit[2]));
                b.Location = new Point(int.Parse(strSplit[4]) + pictureBox1.Location.X, int.Parse(strSplit[5]) + pictureBox1.Location.Y - 6);
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
                b.ContextMenuStrip = ctxtEdit;
                b.Tag = "macro";

                this.Controls.Add(b);
                b.BringToFront();
                b.MouseDown += button_dragMouseDown;
                b.MouseMove += button_dragMouseMove;

                b.Click += button_click;


                //MessageBox.Show(b.Name.ToString());
                count++;
            }
            LockCheck();

            Properties.Settings.Default.fileDirectory = fDiag.SelectedPath;
            Properties.Settings.Default.Save();
        }

        private void button_click(object sender, EventArgs e)
        {
            if (chkLock.Checked == true)
            {
                Console.WriteLine("Start: " + Properties.Settings.Default.fileDirectory + @"\Macros\" + ((Button)sender).Name + ".ahk");
                if (File.Exists(Properties.Settings.Default.fileDirectory + @"\Macros\" + ((Button)sender).Name + ".ahk"))
                {
                    Process.Start(Properties.Settings.Default.fileDirectory + @"\Macros\" + ((Button)sender).Name + ".ahk");
                }
                //MessageBox.Show(((Button)sender).Name);
            }
        }

        //private void New_Click(object sender, EventArgs e)
        //{
        //    NewExport();
        //    Button b = new Button();
        //    b.Size = new Size(int.Parse(numWidth.Value.ToString()), int.Parse(numHeight.Value.ToString()));
        //    b.MouseDown += button_dragMouseDown;
        //    b.MouseMove += button_dragMouseMove;
        //    b.Click += button_click;

        //    this.Controls.Add(b);
        //    b.ContextMenuStrip = contextMenuStrip1;
        //    b.BackgroundImageLayout = ImageLayout.Stretch;

        //    b.FlatStyle = FlatStyle.Flat;
        //    b.Tag = "macro";
        //    int count = 1;

        //        foreach (Button button in this.Controls.OfType<Button>())
        //        {
        //            //if (button.Tag.ToString() != "UI")
        //            //{
        //                count++;
        //            //}
        //        }

        //        b.Name = "button" + count;


        //    b.BringToFront();
        //}

        private Point MouseDownLocation;

        Button CurrentButton;


        private void button_dragMouseDown(object sender, MouseEventArgs e)
        {
            if (chkLock.Checked == false)
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    MouseDownLocation = e.Location;
                    Console.WriteLine(MouseDownLocation);
                }

                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                }
            }
                    CurrentButton = ((Button)sender);

        }
            
        private void button_dragMouseMove(object sender, MouseEventArgs e)
        {
            if (chkLock.Checked == false)
            {
                Button b = sender as Button;
                Console.WriteLine(b.Name);
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    b.Left = e.X + b.Left - MouseDownLocation.X;
                    b.Top = e.Y + b.Top - MouseDownLocation.Y;

                    //((Button)sender).Left = e.X + ((Button)sender).Left - MouseDownLocation.X;
                    //((Button)sender).Top = e.Y + ((Button)sender).Top - MouseDownLocation.Y;
                }
            }
        }

        public void NotifyMe(string text, string name, string location, Size size, bool isText)
        {
            if (isText == true)
            {
                CurrentButton.Text = text;
            }
            else
            {
                CurrentButton.Text = "";
            }
            CurrentButton.Size = size;
            //MessageBox.Show(name + " " + CurrentButton.Name);
            CurrentButton.Name = name;

            if (File.Exists(location + @"\Images\" + name))
            {
                using (FileStream fs = new FileStream(location + @"\Images\" + name, FileMode.Open))
                {
                    CurrentButton.BackgroundImage = Image.FromStream(fs);
                    fs.Close();
                }
            }
            else
            {
                CurrentButton.BackgroundImage = null;
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditButton();

            //CurrentButton.Name = name;
        }

        private void EditButton()
        {
            if (!Directory.Exists(Properties.Settings.Default.fileDirectory + @"\HotKeys\"))
            {
                Directory.CreateDirectory(Properties.Settings.Default.fileDirectory + @"\HotKeys\");
            }

            if (!Directory.Exists(Properties.Settings.Default.fileDirectory + @"\Macros\"))
            {
                Directory.CreateDirectory(Properties.Settings.Default.fileDirectory + @"\Macros\");
            }

            if (!Directory.Exists(Properties.Settings.Default.fileDirectory + @"\Images\"))
            {
                Directory.CreateDirectory(Properties.Settings.Default.fileDirectory + @"\Images\");
            }

            if (!Directory.Exists(Properties.Settings.Default.fileDirectory + @"\Hasu\"))
            {
                Directory.CreateDirectory(Properties.Settings.Default.fileDirectory + @"\Hasu\");
            }
            string location, strname;
            using (frmEdit editing = new frmEdit(CurrentButton.Name, CurrentButton.Text, Properties.Settings.Default.fileDirectory, CurrentButton.Size))
            {
                location = editing.location;
                strname = editing.name;
                editing.ShowDialog(this);

            }
            //MessageBox.Show(strname);
            //Console.WriteLine("Editing: " + location + strname);
            //frmEdit edit = new frmEdit(CurrentButton.Name, Properties.Settings.Default.fileDirectory);
            //edit.Show();
            //MessageBox.Show("tets");

            if (File.Exists(location + @"\Images\" + strname))
            {
                using (FileStream fs = new FileStream(location + @"\Images\" + strname, FileMode.Open))
                {
                    CurrentButton.BackgroundImage = Image.FromStream(fs);
                    fs.Close();
                }
            }
            else
            {
                //CurrentButton.BackgroundImage = null;
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Controls.Remove(CurrentButton);
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewExport();
            Button b = new Button();
            b.Size = new Size(int.Parse(35.ToString()), int.Parse(35.ToString()));
            b.Location = this.PointToClient(Cursor.Position);
            b.MouseDown += button_dragMouseDown;
            b.MouseMove += button_dragMouseMove;
            b.Click += button_click;

            this.Controls.Add(b);
            b.ContextMenuStrip = ctxtEdit;
            b.BackgroundImageLayout = ImageLayout.Stretch;

            b.FlatStyle = FlatStyle.Flat;
            b.Tag = "macro";
            int count = 1;

            foreach (Button button in this.Controls.OfType<Button>())
            {
                //if (button.Tag.ToString() != "UI")
                //{
                count++;
                //}
            }
            Console.WriteLine((Properties.Settings.Default.fileDirectory + @"\Images\") + " " + (Properties.Settings.Default.fileDirectory + @"\Images\").Length);
            //b.Name = "button" + File.ReadAllLines(Properties.Settings.Default.fileDirectory + @"\Save1.txt").Length;
            b.Name = "button" + Directory.GetFiles(Properties.Settings.Default.fileDirectory + @"\Images\").Length;

            //b.Name = "button" + count;
            //}
            //else
            //{
            //    Console.WriteLine("THIS SHOULD NOT EXIST");
            //    b.Name = txtName.Text;
            //}

            b.BringToFront();

            
        }

        List<Process> procTest = new List<Process>();


        private void button1_Click(object sender, EventArgs e)
        {
            string[] dirHotkeys = Directory.GetFiles(Properties.Settings.Default.fileDirectory + @"\HotKeys\");
            procTest.Clear();
            string testAHK = "";
            if (Properties.Settings.Default.isHasu == true)
            {
                testAHK += @"#if (getKeyState(""F24"", ""P""))" + Environment.NewLine + "F24::" + Environment.NewLine + "return" + Environment.NewLine;
            }
            foreach (string str in dirHotkeys)
            {
                using (StreamReader fs = new StreamReader(str))
                {
                    testAHK += fs.ReadToEnd();
                }
                //testAHK += 
            }
            MessageBox.Show(testAHK);

            using (StreamWriter fs = new StreamWriter(Properties.Settings.Default.fileDirectory  + @"\test" + ".ahk"))
            {
                fs.Write(testAHK);
                fs.Close();
            }

            Process p = new Process();
            p.StartInfo.FileName = Properties.Settings.Default.fileDirectory + @"\test" + ".ahk";
            p.Start();

            procTest.Add(p);

            //foreach (string str in dirHotkeys)
            //{
            //    Console.WriteLine(str);
            //    Process p = new Process();
            //    p.StartInfo.FileName = str;
            //    p.Start();


            //    procTest.Add(p);
            //}
            ////Create process
            //System.Diagnostics.Process pProcess = new System.Diagnostics.Process();

            ////strCommand is path and file name of command to run
            //pProcess.StartInfo.FileName = "tasklist";

            ////strCommandParameters are parameters to pass to program
            ////pProcess.StartInfo.Arguments = strCommandParameters;

            //pProcess.StartInfo.UseShellExecute = false;

            ////Set output of program to be written to process output stream
            //pProcess.StartInfo.RedirectStandardOutput = true;

            ////Optional
            ////pProcess.StartInfo.WorkingDirectory = strWorkingDirectory;

            ////Start the process
            //pProcess.Start();

            ////Get program output
            //string strOutput = pProcess.StandardOutput.ReadToEnd();

            ////Wait for process to finish
            //pProcess.WaitForExit();
            //using (StreamReader st = pProcess.StandardOutput)
            //{
            //    while (!st.EndOfStream)
            //    {
            //        string curLine = st.ReadLine();
            //        if (curLine.Contains(".ahk"))
            //        {
            //            MessageBox.Show(curLine);
            //        }
            //    }
            //}
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (Process p in procTest)
            {
                p.Kill();
            }
            procTest.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            foreach (Process p in procTest)
            {
                Console.WriteLine(p.Id);
            }
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSettings settings = new frmSettings();
            settings.ShowDialog();
            pictureBox1.Image = Image.FromFile(Properties.Settings.Default.Backgrounds);
        }


        private void btnCompile_Click(object sender, EventArgs e)
        {
            Compile();
        }

        private void chkLock_CheckedChanged(object sender, EventArgs e)
        {
            LockCheck();
        }

        private void LockCheck()
        {
            if (chkLock.Checked == true)
            {
                btnCompile.Enabled = true;
            }
            else if (chkLock.Checked == false)
            {
                btnCompile.Enabled = false;
            }
        }

        private void compileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Compile();
        }

        private void Compile()
        {
            Console.WriteLine(btnCompile.Tag + "!");
            if (btnCompile.Tag.ToString() == "Start")
            {
                btnCompile.Tag = "Stop";
                btnCompile.Text = "Stop";
                picCompile.Image = Properties.Resources.Stop;

                tmrCompiled.Enabled = true;

                string[] dirHotkeys = Directory.GetFiles(Properties.Settings.Default.fileDirectory + @"\HotKeys\");
                string[] dirHasu = Directory.GetFiles(Properties.Settings.Default.fileDirectory + @"\Hasu\");
                procTest.Clear();
                string testAHK = "";
                //if (Properties.Settings.Default.isHasu == true)
                //{
                //}
                foreach (string str in dirHotkeys)
                {
                    using (StreamReader fs = new StreamReader(str))
                    {
                        testAHK += fs.ReadToEnd() + Environment.NewLine;
                    }
                    //testAHK += 
                }
                testAHK += @"#if (getKeyState(""F24"", ""P""))" + Environment.NewLine + "F24::" + Environment.NewLine + "return" + Environment.NewLine;
                foreach (string str in dirHasu)
                {
                    using (StreamReader fs = new StreamReader(str))
                    {
                        testAHK += fs.ReadToEnd() + Environment.NewLine;
                    }
                    //testAHK += 
                }
                //MessageBox.Show(testAHK);

                using (StreamWriter fs = new StreamWriter(Properties.Settings.Default.fileDirectory + Path.GetFileName(Path.GetDirectoryName(Properties.Settings.Default.fileDirectory)) + ".ahk"))
                {
                    fs.Write(testAHK);
                    fs.Close();
                }

                Process p = new Process();
                p.StartInfo.FileName = Properties.Settings.Default.fileDirectory + Path.GetFileName(Path.GetDirectoryName(Properties.Settings.Default.fileDirectory)) + ".ahk";
                p.Start();

                procTest.Add(p);

            }
            else if (btnCompile.Tag.ToString() == "Stop")
            {
                KillProc();
            }
        }

        private void KillProc()
        {
            foreach (Process p in procTest)
            {
                p.Kill();
            }
            procTest.Clear();
            btnCompile.Tag = "Start";
            btnCompile.Text = "Compile";
            btnCompile.Text = "Compile";
            picCompile.Image = Properties.Resources.Start;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            KillProc();
        }

        private void openMiniToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMini mini = new frmMini();
            mini.ShowDialog();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                SaveCurrentFile();
            }
        }

        private void newMacroFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateNewMacroFolder();
        }

        private void CreateNewMacroFolder()
        {
            ClearButtons();
            Properties.Settings.Default.fileDirectory = "";
            Properties.Settings.Default.Save();
            SaveCurrentFile();
        }

        private void reloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Button button in this.Controls.OfType<Button>())
            {

                if (File.Exists(Properties.Settings.Default.fileDirectory + @"\Images\" + button.Name))
                {
                    using (FileStream fs = new FileStream(Properties.Settings.Default.fileDirectory + @"\Images\" + button.Name, FileMode.Open))
                    {
                        button.BackgroundImage = Image.FromStream(fs);
                        fs.Close();
                    }
                }
                else
                {
                    //CurrentButton.BackgroundImage = null;
                }
            }
        }

        private void tmrCompiled_Tick(object sender, EventArgs e)
        {
            try
            {
                foreach (Process p in procTest)
                {
                    if (p.HasExited == true)
                    {
                        procTest.Clear();
                        btnCompile.Tag = "Start";
                        btnCompile.Text = "Compile";
                        btnCompile.Text = "Compile";
                        picCompile.Image = Properties.Resources.Start;

                        tmrCompiled.Enabled = false;
                    }
                
                }
            }
            catch
            {
                procTest.Clear();
                btnCompile.Tag = "Start";
                btnCompile.Text = "Compile";
                btnCompile.Text = "Compile";
                picCompile.Image = Properties.Resources.Start;

                tmrCompiled.Enabled = false;
            }
        }

        private void pictureBox1_SizeChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.SizeX = Size.Width;
            Properties.Settings.Default.SizeY = Size.Height;
            Properties.Settings.Default.Save();
        }
    }
}
