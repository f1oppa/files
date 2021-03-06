using System.Diagnostics;

namespace File_Manager
{
    public partial class Form1 : Form
    {
        string path = "";
        public Form1()
        {
            InitializeComponent();

            ContextMenuStrip mainStrip = new ContextMenuStrip();
            ToolStripMenuItem refreshItem = new ToolStripMenuItem();
            ToolStripMenuItem openItem = new ToolStripMenuItem();
            ToolStripMenuItem aboutItem = new ToolStripMenuItem();
            ToolStripMenuItem gotoItem = new ToolStripMenuItem();

            refreshItem.Click += new EventHandler(refreshItem_Click);
            refreshItem.Text = "Refresh (F5)";
            mainStrip.Items.Add(refreshItem);

            gotoItem.Click += new EventHandler(gotoItem_Click);
            gotoItem.Text = "Go to";
            mainStrip.Items.Add(gotoItem);

            aboutItem.Click += new EventHandler(aboutItem_Click);
            aboutItem.Text = "About";
            mainStrip.Items.Add(aboutItem);

            this.ContextMenuStrip = mainStrip;
        }

        public void Form1_Load(object sender, EventArgs e)
        {
            this.AutoScroll = true;
            this.Icon = new Icon("icon.ico"); ;
            this.KeyPreview = true;

            listdrives();
        }

        public void listcontents(String filePath)
        {
            this.Controls.Clear();

            this.Text = "File Manager - " + filePath;

            Button backbtn = new Button();
            backbtn.Text = "Back";
            backbtn.Size = new Size(50, 30);
            backbtn.Location = new Point(10, 5);
            backbtn.Click += (s, e) => { goback(path); };
            this.Controls.Add(backbtn);

            Label title = new Label();

            title.Text = new DirectoryInfo(filePath).Name;

            title.Location = new Point(63, 11);
            title.Size = new Size(300, 30);
            title.Font = new Font("Arial", 12);
            this.Controls.Add(title);

            int i = 1;

            foreach (string folder in Directory.GetDirectories(filePath))
            {
                if(!new DirectoryInfo(folder).Attributes.HasFlag(FileAttributes.Hidden))
                {
                    Button folderName = new Button();
                    folderName.Size = new Size(500, 38);
                    folderName.TextAlign = ContentAlignment.MiddleLeft;
                    folderName.Text = folder.Replace(filePath, "").Replace("\\", "");
                    folderName.Location = new Point(10, i * 40);
                    folderName.Click += (s, e) => { path = folder; this.Text = "File Manager - " + path; listcontents(path); };
                    this.Controls.Add(folderName);
                    i++;
                }
            }

            foreach (string file in Directory.GetFiles(filePath, "*"))
            {
                if (!new FileInfo(filePath).Attributes.HasFlag(FileAttributes.Hidden))
                {
                    Button fileName = new Button();
                    fileName.Size = new Size(500, 38);
                    fileName.Text = file.Replace(filePath, "").Replace("\\", "");
                    fileName.TextAlign = ContentAlignment.MiddleLeft;
                    fileName.Click += (s, e) => {
                        Process cmd = new Process();
                        cmd.StartInfo.FileName = "cmd.exe";
                        cmd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        cmd.StartInfo.CreateNoWindow = true;
                        cmd.StartInfo.Arguments = "/c \"" + file + "\"";
                        cmd.Start();
                    };
                    fileName.Location = new Point(10, i * 40);
                    this.Controls.Add(fileName);
                    i++;
                }
            }

            if(i == 1)
            {
                Label infolabel = new Label();
                infolabel.Text = "This folder is empty.";
                infolabel.Size = new Size(500, 38);
                infolabel.Location = new Point(10, i * 50);
                infolabel.Font = new Font("Arial", 10);
                this.Controls.Add(infolabel);
            }
        }

        public void listdrives()
        {
            this.Controls.Clear();
            path = "";

            Label title = new Label();
            title.Size = new Size(500, 40);
            title.Location = new Point(10, 5);
            title.Font = new Font("Arial", 15);
            title.TextAlign = ContentAlignment.MiddleLeft;
            title.Text = "File Manager";
            this.Controls.Add(title);

            int i = 1;

            foreach (var drive in DriveInfo.GetDrives())
            {
                Button driveBtn = new Button();
                string label = "";

                driveBtn.TextAlign = ContentAlignment.MiddleLeft;

                try
                {
                    if (drive.VolumeLabel == "")
                    {
                        label = "Local Disk";
                    }
                    else
                    {
                        label = drive.VolumeLabel;
                    }
                }
                catch
                {
                    label = "Unknown filesystem";
                    driveBtn.Enabled = false;
                }

                driveBtn.Text = label + " (" + drive.Name.Replace("\\", "") + ")";
                driveBtn.Size = new Size(500, 45);
                driveBtn.Location = new Point(10, i * 50);
                driveBtn.Click += (s, e) => { path += drive.Name; this.Text = "File Manager - " + path; listcontents(drive.Name); };
                this.Controls.Add(driveBtn);
                i++;
            }

            Label loclabel = new Label();
            loclabel.TextAlign = ContentAlignment.MiddleLeft;
            loclabel.Text = "Locations";
            loclabel.Size = new Size(500, 45);
            loclabel.Location = new Point(10, i * 50);
            loclabel.Font = new Font("Arial", 13);
            this.Controls.Add(loclabel);

            i++;

            Button desktop = new Button();
            desktop.Size = new Size(120, 45);
            desktop.Text = "Desktop";
            desktop.Click += (s, e) => {
                path = "C:\\Users\\" + Environment.UserName + "\\Desktop";
                this.Text = "File Manager - Desktop";
                listcontents("C:\\Users\\" + Environment.UserName + "\\Desktop");
            };
            desktop.Location = new Point(10, i * 50);
            this.Controls.Add(desktop);

            Button docs = new Button();
            docs.Size = new Size(120, 45);
            docs.Text = "Documents";
            docs.Click += (s, e) => {
                path = "C:\\Users\\" + Environment.UserName + "\\Documents";
                this.Text = "File Manager - Documents";
                listcontents("C:\\Users\\" + Environment.UserName + "\\Documents");
            };
            docs.Location = new Point(135, i * 50);
            this.Controls.Add(docs);

            Button downloads = new Button();
            downloads.Size = new Size(120, 45);
            downloads.Text = "Downloads";
            downloads.Click += (s, e) => {
                path = "C:\\Users\\" + Environment.UserName + "\\Downloads";
                this.Text = "File Manager - Downloads";
                listcontents("C:\\Users\\" + Environment.UserName + "\\Downloads"); 
            };
            downloads.Location = new Point(260, i * 50);
            this.Controls.Add(downloads);

            Button plus = new Button();
            plus.Size = new Size(120, 45);
            plus.Text = "Custom...";
            plus.Click += (o, e) => {
                goTo();
            };
            plus.Location = new Point(385, i * 50);
            this.Controls.Add(plus);

            i++;
        }

        public void goback(string currentPath)
        {
            if (currentPath == "" || Directory.GetParent(currentPath) == null)
            {
                this.Text = "File Manager";
                listdrives();
            }
            else
            {
                path = Directory.GetParent(currentPath).FullName;
                listcontents(path);
            }
        }

        public void refresh()
        {
            if (Directory.Exists(path))
            {
                listcontents(path);
            }
            else
            {
                listdrives();
            }
        }

        public void refreshItem_Click(object sender, EventArgs e)
        {
            refresh();
        }

        public void aboutItem_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                UseShellExecute = true,
                FileName = "https://github.com/f1oppa/files"
            }).Dispose();
        }
        public void gotoItem_Click(object sender, EventArgs e)
        {
            goTo();
        }

        public void goTo()
        {
            string newPath = Microsoft.VisualBasic.Interaction.InputBox("Enter the location you want to go to.", "Go to", "");
            if(newPath != "")
            {
                if (Directory.Exists(newPath))
                {
                    listcontents(newPath);
                    path = newPath;
                }
                else
                {
                    MessageBox.Show("Not a valid path", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                goback(path);
                return true;
            }
            if (keyData == Keys.F5)
            {
                refresh();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}