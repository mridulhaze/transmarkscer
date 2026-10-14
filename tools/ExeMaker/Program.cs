using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Windows.Forms;

namespace ExeMaker
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }

    public class MainForm : Form
    {
        // UI Controls
        private Panel pnlHeader;
        private Label lblHeaderTitle;
        private Label lblHeaderSub;
        private Panel pnlCard;
        private PictureBox pbIconPreview;
        private Label lblIconInfo;
        private Button btnAddIcon;
        private Button btnMakeExe;
        private ProgressBar progressBar;
        private Label lblStatus;
        private TextBox tbLog;
        private Button btnOpenFolder;
        private Button btnRunExe;

        // Colors
        private readonly Color colGreen = Color.FromArgb(5, 150, 105);      // #059669
        private readonly Color colGreenDark = Color.FromArgb(4, 120, 87);    // #047857
        private readonly Color colGreenHover = Color.FromArgb(16, 185, 129); // #10B981
        private readonly Color colBg = Color.FromArgb(245, 247, 250);
        private readonly Color colSurface = Color.White;
        private readonly Color colBorder = Color.FromArgb(226, 232, 240);

        // State
        private string selectedImagePath = "";
        private string projectRoot = "";
        private string csprojPath = "";
        private BackgroundWorker buildWorker;

        public MainForm()
        {
            InitializeComponent();
            LocateProjectPaths();
            LoadDefaultIcon();
        }

        private void InitializeComponent()
        {
            this.Text = "NU H-Type Solver - Standalone EXE & Icon Maker";
            this.Size = new Size(580, 640);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = colBg;
            this.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);

            // Header Panel
            pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = colGreenDark,
                Padding = new Padding(20, 12, 20, 12)
            };

            lblHeaderTitle = new Label
            {
                Text = "⚡ Standalone EXE & Icon Packager",
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(18, 12)
            };

            lblHeaderSub = new Label
            {
                Text = "Easily select a custom icon (JPG / PNG) and compile a portable EXE for any computer.",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Regular),
                ForeColor = Color.FromArgb(209, 250, 229),
                AutoSize = true,
                Location = new Point(20, 40)
            };

            pnlHeader.Controls.Add(lblHeaderTitle);
            pnlHeader.Controls.Add(lblHeaderSub);

            // Main Card
            pnlCard = new Panel
            {
                Location = new Point(20, 85),
                Size = new Size(524, 500),
                BackColor = colSurface
            };
            pnlCard.Paint += (s, e) =>
            {
                ControlPaint.DrawBorder(e.Graphics, pnlCard.ClientRectangle, colBorder, ButtonBorderStyle.Solid);
            };

            // Icon Preview Box
            Label lblIconHeading = new Label
            {
                Text = "APPLICATION ICON PREVIEW",
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = Color.FromArgb(100, 116, 139),
                Location = new Point(20, 16),
                AutoSize = true
            };

            pbIconPreview = new PictureBox
            {
                Location = new Point(20, 38),
                Size = new Size(110, 110),
                SizeMode = PictureBoxSizeMode.Zoom,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(248, 250, 252)
            };

            lblIconInfo = new Label
            {
                Location = new Point(145, 45),
                Size = new Size(360, 60),
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(51, 65, 85),
                Text = "Default icon: static/nu.png\nReady to embed."
            };

            // Button 1: Add Icon
            btnAddIcon = new Button
            {
                Text = "🖼️ Add Icon (Select JPG / PNG)",
                Location = new Point(145, 105),
                Size = new Size(240, 42),
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                BackColor = Color.White,
                ForeColor = colGreenDark,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnAddIcon.FlatAppearance.BorderColor = colGreen;
            btnAddIcon.FlatAppearance.BorderSize = 2;
            btnAddIcon.Click += BtnAddIcon_Click;

            // Separator line
            Panel pnlSep = new Panel
            {
                Location = new Point(20, 165),
                Size = new Size(484, 1),
                BackColor = colBorder
            };

            // Button 2: Make EXE
            btnMakeExe = new Button
            {
                Text = "⚡ Make EXE",
                Location = new Point(20, 180),
                Size = new Size(484, 50),
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                BackColor = colGreen,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnMakeExe.FlatAppearance.BorderSize = 0;
            btnMakeExe.Click += BtnMakeExe_Click;

            // Status Label
            lblStatus = new Label
            {
                Text = "Status: Ready to build",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(71, 85, 105),
                Location = new Point(20, 240),
                AutoSize = true
            };

            // Progress Bar
            progressBar = new ProgressBar
            {
                Location = new Point(20, 262),
                Size = new Size(484, 16),
                Style = ProgressBarStyle.Blocks,
                Value = 0
            };

            // Log Console
            Label lblLogHeading = new Label
            {
                Text = "COMPILATION LOG",
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = Color.FromArgb(100, 116, 139),
                Location = new Point(20, 290),
                AutoSize = true
            };

            tbLog = new TextBox
            {
                Location = new Point(20, 310),
                Size = new Size(484, 130),
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                BackColor = Color.FromArgb(15, 23, 42),
                ForeColor = Color.FromArgb(226, 232, 240),
                Font = new Font("Consolas", 8.5F)
            };

            // Action Buttons after build
            btnOpenFolder = new Button
            {
                Text = "📂 Open Output Folder",
                Location = new Point(20, 450),
                Size = new Size(236, 36),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(30, 41, 59),
                FlatStyle = FlatStyle.Flat,
                Enabled = false,
                Cursor = Cursors.Hand
            };
            btnOpenFolder.FlatAppearance.BorderColor = colBorder;
            btnOpenFolder.Click += (s, e) =>
            {
                string distDir = Path.Combine(projectRoot, "dist");
                if (Directory.Exists(distDir)) Process.Start("explorer.exe", distDir);
            };

            btnRunExe = new Button
            {
                Text = "▶️ Launch Standalone EXE",
                Location = new Point(268, 450),
                Size = new Size(236, 36),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                BackColor = Color.FromArgb(240, 253, 244),
                ForeColor = colGreenDark,
                FlatStyle = FlatStyle.Flat,
                Enabled = false,
                Cursor = Cursors.Hand
            };
            btnRunExe.FlatAppearance.BorderColor = colGreen;
            btnRunExe.Click += (s, e) =>
            {
                string exePath = Path.Combine(projectRoot, "dist", "NU H-Type Solver.exe");
                if (File.Exists(exePath)) Process.Start(exePath);
            };

            pnlCard.Controls.Add(lblIconHeading);
            pnlCard.Controls.Add(pbIconPreview);
            pnlCard.Controls.Add(lblIconInfo);
            pnlCard.Controls.Add(btnAddIcon);
            pnlCard.Controls.Add(pnlSep);
            pnlCard.Controls.Add(btnMakeExe);
            pnlCard.Controls.Add(lblStatus);
            pnlCard.Controls.Add(progressBar);
            pnlCard.Controls.Add(lblLogHeading);
            pnlCard.Controls.Add(tbLog);
            pnlCard.Controls.Add(btnOpenFolder);
            pnlCard.Controls.Add(btnRunExe);

            this.Controls.Add(pnlCard);
            this.Controls.Add(pnlHeader);

            // BackgroundWorker
            buildWorker = new BackgroundWorker
            {
                WorkerReportsProgress = true
            };
            buildWorker.DoWork += BuildWorker_DoWork;
            buildWorker.ProgressChanged += BuildWorker_ProgressChanged;
            buildWorker.RunWorkerCompleted += BuildWorker_RunWorkerCompleted;
        }

        private void LocateProjectPaths()
        {
            string current = AppDomain.CurrentDomain.BaseDirectory;
            while (!string.IsNullOrEmpty(current))
            {
                string candidate = Path.Combine(current, "NU Solver", "NU H-Type Solver.csproj");
                if (File.Exists(candidate))
                {
                    projectRoot = current;
                    csprojPath = candidate;
                    return;
                }
                current = Path.GetDirectoryName(current);
            }

            // Fallback
            projectRoot = @"E:\projects\NU H-Type Solver_code_5_for_database_solving";
            csprojPath = Path.Combine(projectRoot, "NU Solver", "NU H-Type Solver.csproj");
        }

        private void LoadDefaultIcon()
        {
            string defaultImg = Path.Combine(projectRoot, "static", "nu.png");
            if (File.Exists(defaultImg))
            {
                selectedImagePath = defaultImg;
                try
                {
                    using (var fs = new FileStream(defaultImg, FileMode.Open, FileAccess.Read))
                    {
                        pbIconPreview.Image = Image.FromStream(fs);
                    }
                    lblIconInfo.Text = string.Format("Default Icon:\n{0}\nImage: {1}x{2}", 
                        Path.GetFileName(defaultImg), pbIconPreview.Image.Width, pbIconPreview.Image.Height);
                }
                catch { }
            }
        }

        private void BtnAddIcon_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Select Application Icon (JPG or PNG)";
                ofd.Filter = "Image Files (*.png;*.jpg;*.jpeg;*.ico;*.bmp)|*.png;*.jpg;*.jpeg;*.ico;*.bmp|All Files (*.*)|*.*";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    selectedImagePath = ofd.FileName;
                    try
                    {
                        using (var fs = new FileStream(selectedImagePath, FileMode.Open, FileAccess.Read))
                        {
                            if (pbIconPreview.Image != null) pbIconPreview.Image.Dispose();
                            pbIconPreview.Image = Image.FromStream(fs);
                        }
                        lblIconInfo.Text = string.Format("Selected Icon:\n{0}\nResolution: {1}x{2}", 
                            Path.GetFileName(selectedImagePath), pbIconPreview.Image.Width, pbIconPreview.Image.Height);
                        AppendLog("Loaded custom icon: " + selectedImagePath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Could not load image: " + ex.Message, "Image Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnMakeExe_Click(object sender, EventArgs e)
        {
            if (buildWorker.IsBusy) return;

            btnMakeExe.Enabled = false;
            btnAddIcon.Enabled = false;
            btnOpenFolder.Enabled = false;
            btnRunExe.Enabled = false;
            progressBar.Style = ProgressBarStyle.Marquee;
            lblStatus.Text = "Status: Building Standalone EXE...";
            lblStatus.ForeColor = colGreenDark;
            tbLog.Clear();
            AppendLog("Starting build process...");

            buildWorker.RunWorkerAsync();
        }

        private void BuildWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                // Step 1: Prepare Icon
                buildWorker.ReportProgress(10, "Generating multi-resolution icon file...");
                string tempIcoPath = Path.Combine(Path.GetTempPath(), "nu_solver_app_icon.ico");

                if (!string.IsNullOrEmpty(selectedImagePath) && File.Exists(selectedImagePath))
                {
                    if (Path.GetExtension(selectedImagePath).Equals(".ico", StringComparison.OrdinalIgnoreCase))
                    {
                        File.Copy(selectedImagePath, tempIcoPath, true);
                    }
                    else
                    {
                        IcoConverter.ConvertToIco(selectedImagePath, tempIcoPath);
                    }
                }
                else
                {
                    string fallbackPng = Path.Combine(projectRoot, "static", "nu.png");
                    if (File.Exists(fallbackPng))
                    {
                        IcoConverter.ConvertToIco(fallbackPng, tempIcoPath);
                    }
                }

                buildWorker.ReportProgress(25, "Locating MSBuild compiler...");
                string msbuildPath = FindMsBuild();
                if (string.IsNullOrEmpty(msbuildPath))
                {
                    throw new Exception("Could not locate MSBuild.exe on this system. Make sure .NET Framework 4.0+ is installed.");
                }

                buildWorker.ReportProgress(40, "Compiling NU Solver in Release mode with embedded icon...");
                string args = string.Format("\"{0}\" /p:Configuration=Release /p:ApplicationIcon=\"{1}\" /v:m", csprojPath, tempIcoPath);

                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = msbuildPath,
                    Arguments = args,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WorkingDirectory = Path.GetDirectoryName(csprojPath)
                };

                using (Process proc = Process.Start(psi))
                {
                    string stdout = proc.StandardOutput.ReadToEnd();
                    string stderr = proc.StandardError.ReadToEnd();
                    proc.WaitForExit();

                    buildWorker.ReportProgress(70, stdout);
                    if (!string.IsNullOrEmpty(stderr))
                    {
                        buildWorker.ReportProgress(75, "STDERR: " + stderr);
                    }

                    if (proc.ExitCode != 0)
                    {
                        throw new Exception("MSBuild failed with exit code " + proc.ExitCode);
                    }
                }

                buildWorker.ReportProgress(85, "Packaging standalone files into dist/...");
                string outputExe = Path.Combine(Path.GetDirectoryName(csprojPath), "bin", "Release", "NU H-Type Solver.exe");
                if (!File.Exists(outputExe))
                {
                    throw new Exception("Output executable not found at: " + outputExe);
                }

                string distDir = Path.Combine(projectRoot, "dist");
                string distSingleDir = Path.Combine(distDir, "NU_H-Type_Solver_SingleEXE");
                Directory.CreateDirectory(distDir);
                Directory.CreateDirectory(distSingleDir);

                // Copy to dist/
                string targetExe1 = Path.Combine(distDir, "NU H-Type Solver.exe");
                string targetExe2 = Path.Combine(distSingleDir, "NU H-Type Solver.exe");
                File.Copy(outputExe, targetExe1, true);
                File.Copy(outputExe, targetExe2, true);

                // Update connection.config
                string connStr = "Data Source=192.168.0.230;User id=sa;Password=1234;Initial Catalog=Solving_5;";
                File.WriteAllText(Path.Combine(distSingleDir, "connection.config"), connStr);

                // Re-create Zip
                buildWorker.ReportProgress(95, "Creating standalone ZIP archive...");
                string zipPath = Path.Combine(distDir, "NU_H-Type_Solver_Single_EXE.zip");
                if (File.Exists(zipPath)) File.Delete(zipPath);

                using (var archive = ZipFile.Open(zipPath, ZipArchiveMode.Create))
                {
                    archive.CreateEntryFromFile(targetExe1, "NU H-Type Solver.exe");
                }

                buildWorker.ReportProgress(100, "BUILD COMPLETE! Standalone EXE is ready.");
                e.Result = targetExe1;
            }
            catch (Exception ex)
            {
                e.Result = ex;
            }
        }

        private void BuildWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            string msg = e.UserState as string;
            if (!string.IsNullOrEmpty(msg))
            {
                AppendLog(msg);
            }
        }

        private void BuildWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBar.Style = ProgressBarStyle.Blocks;
            progressBar.Value = 100;
            btnMakeExe.Enabled = true;
            btnAddIcon.Enabled = true;

            if (e.Result is Exception)
            {
                Exception ex = (Exception)e.Result;
                lblStatus.Text = "Status: Build Failed!";
                lblStatus.ForeColor = Color.Red;
                AppendLog("ERROR: " + ex.Message);
                MessageBox.Show("Build failed:\n" + ex.Message, "Build Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                lblStatus.Text = "Status: ✅ Standalone EXE Created Successfully!";
                lblStatus.ForeColor = colGreenDark;
                btnOpenFolder.Enabled = true;
                btnRunExe.Enabled = true;
                string exePath = (string)e.Result;
                AppendLog("SUCCESS: " + exePath);
                MessageBox.Show("Standalone EXE with your custom icon was created successfully!\n\nLocation:\n" + exePath, 
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private string FindMsBuild()
        {
            string[] searchPaths = new string[]
            {
                @"C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe",
                @"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe",
                @"C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe",
                @"C:\Program Files\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe",
                @"C:\Program Files\Microsoft Visual Studio\2022\Enterprise\MSBuild\Current\Bin\MSBuild.exe",
                @"C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe",
                @"C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin\MSBuild.exe"
            };

            foreach (string p in searchPaths)
            {
                if (File.Exists(p)) return p;
            }
            return null;
        }

        private void AppendLog(string text)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(AppendLog), text);
                return;
            }
            tbLog.AppendText("[" + DateTime.Now.ToString("HH:mm:ss") + "] " + text + Environment.NewLine);
        }
    }

    public static class IcoConverter
    {
        public static void ConvertToIco(string inputImagePath, string outputIcoPath)
        {
            using (Bitmap original = new Bitmap(inputImagePath))
            {
                int[] sizes = new int[] { 16, 32, 48, 64, 128, 256 };
                byte[][] pngBuffers = new byte[sizes.Length][];

                for (int i = 0; i < sizes.Length; i++)
                {
                    int s = sizes[i];
                    using (Bitmap resized = new Bitmap(s, s, PixelFormat.Format32bppArgb))
                    {
                        using (Graphics g = Graphics.FromImage(resized))
                        {
                            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            g.SmoothingMode = SmoothingMode.HighQuality;
                            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                            g.Clear(Color.Transparent);
                            g.DrawImage(original, 0, 0, s, s);
                        }
                        using (MemoryStream ms = new MemoryStream())
                        {
                            resized.Save(ms, ImageFormat.Png);
                            pngBuffers[i] = ms.ToArray();
                        }
                    }
                }

                using (FileStream fs = new FileStream(outputIcoPath, FileMode.Create))
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    // ICONDIR
                    bw.Write((short)0); // Reserved
                    bw.Write((short)1); // Type: Icon
                    bw.Write((short)sizes.Length); // Image count

                    int offset = 6 + (16 * sizes.Length);
                    for (int i = 0; i < sizes.Length; i++)
                    {
                        int s = sizes[i];
                        bw.Write((byte)(s == 256 ? 0 : s)); // Width
                        bw.Write((byte)(s == 256 ? 0 : s)); // Height
                        bw.Write((byte)0);                  // Color count
                        bw.Write((byte)0);                  // Reserved
                        bw.Write((short)1);                 // Color planes
                        bw.Write((short)32);                // Bits per pixel
                        bw.Write((int)pngBuffers[i].Length);// Size of image data
                        bw.Write((int)offset);              // Offset of image data
                        offset += pngBuffers[i].Length;
                    }

                    for (int i = 0; i < sizes.Length; i++)
                    {
                        bw.Write(pngBuffers[i]);
                    }
                }
            }
        }
    }
}
