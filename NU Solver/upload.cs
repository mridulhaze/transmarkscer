using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace NU_Solver
{
    public class UploadState
    {
        public string Message { get; set; }
        public string Log { get; set; }
        public string Detail { get; set; }
        public int Percent { get; set; }
    }

    public class UploadTaskArgs
    {
        public string TargetFolder { get; set; }
        public string FormatMode { get; set; }
        public string ExmCode { get; set; }
        public string ExmYear { get; set; }
    }

    public partial class upload : Form
    {
        string folder = @"D:\working\H_Scanned";
        BackgroundWorker bgwUploader;
        Form callerForm;

        private bool isPaused = false;
        private bool isStopped = false;

        public upload()
        {
            InitializeComponent();
            UITheme.ApplyTheme(this, "Upload OMR Scan Files");

            bgwUploader = new BackgroundWorker();
            bgwUploader.WorkerReportsProgress = true;
            bgwUploader.WorkerSupportsCancellation = true;
            bgwUploader.DoWork += BgwUploader_DoWork;
            bgwUploader.ProgressChanged += BgwUploader_ProgressChanged;
            bgwUploader.RunWorkerCompleted += BgwUploader_RunWorkerCompleted;
        }

        public upload(Form caller) : this()
        {
            this.callerForm = caller;
        }

        private void upload_Click(object sender, EventArgs e)
        {
            if (bgwUploader.IsBusy)
            {
                MessageBox.Show("Upload is currently in progress on background thread.", "Upload Running", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string exmCode = tbExmCode.Text.Trim();
            string exmYear = tbExmYear.Text.Trim();

            if (string.IsNullOrEmpty(exmCode))
            {
                MessageBox.Show("Please enter the Exam Code before uploading (e.g., 201).", "Exam Code Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbExmCode.Focus();
                return;
            }

            if (string.IsNullOrEmpty(exmYear))
            {
                MessageBox.Show("Please enter the Exam Year before uploading (e.g., 2026).", "Exam Year Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbExmYear.Focus();
                return;
            }

            string targetFolder = tbFolderPath.Text.Trim();
            if (string.IsNullOrEmpty(targetFolder) || !Directory.Exists(targetFolder))
            {
                MessageBox.Show("Please select a valid folder containing scanned files before uploading.", "Invalid Folder", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            isPaused = false;
            isStopped = false;

            button1.Enabled = false;
            btnBrowse.Enabled = false;
            tbFolderPath.Enabled = false;
            tbExmCode.Enabled = false;
            tbExmYear.Enabled = false;
            gbFormat.Enabled = false;

            btnPause.Enabled = true;
            btnResume.Enabled = false;
            btnStop.Enabled = true;

            label2.Text = "Uploading files on background thread...";
            progressBar1.Value = 0;
            lblProgressPct.Text = "0% (Starting upload...)";

            string formatMode = "AUTO";
            if (rbEB6.Checked) formatMode = "EB6";
            else if (rbEB5.Checked) formatMode = "EB5";

            bgwUploader.RunWorkerAsync(new UploadTaskArgs
            {
                TargetFolder = targetFolder,
                FormatMode = formatMode,
                ExmCode = exmCode,
                ExmYear = exmYear
            });
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            if (bgwUploader.IsBusy && !isPaused)
            {
                isPaused = true;
                btnPause.Enabled = false;
                btnResume.Enabled = true;
                label2.Text = "⏸ Upload Paused by User";
                lblProgressPct.Text = lblProgressPct.Text + " [PAUSED]";
                richTextBox1.AppendText("--- UPLOAD PAUSED (Will resume from exact row) ---\n");
                richTextBox1.ScrollToCaret();
            }
        }

        private void btnResume_Click(object sender, EventArgs e)
        {
            if (bgwUploader.IsBusy && isPaused)
            {
                isPaused = false;
                btnPause.Enabled = true;
                btnResume.Enabled = false;
                label2.Text = "▶ Resuming Upload...";
                richTextBox1.AppendText("--- UPLOAD RESUMED ---\n");
                richTextBox1.ScrollToCaret();
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (bgwUploader.IsBusy)
            {
                if (MessageBox.Show("Are you sure you want to stop the upload process?", "Confirm Stop", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    isStopped = true;
                    isPaused = false;
                    bgwUploader.CancelAsync();
                }
            }
            else
            {
                if (callerForm != null) callerForm.Show();
                this.Close();
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                fbd.Description = "Select folder containing OMR scan text files:";
                if (!string.IsNullOrEmpty(tbFolderPath.Text) && Directory.Exists(tbFolderPath.Text))
                {
                    fbd.SelectedPath = tbFolderPath.Text;
                }
                else
                {
                    fbd.SelectedPath = @"D:\working\H_Scanned";
                }

                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    tbFolderPath.Text = fbd.SelectedPath;
                }
            }
        }

        private void BgwUploader_DoWork(object sender, DoWorkEventArgs e)
        {
            UploadTaskArgs taskArgs = e.Argument as UploadTaskArgs;
            string targetFolder = taskArgs != null ? taskArgs.TargetFolder : folder;
            string formatMode = taskArgs != null ? taskArgs.FormatMode : "AUTO";
            string exmCode = taskArgs != null ? taskArgs.ExmCode : "201";
            string exmYear = taskArgs != null ? taskArgs.ExmYear : DateTime.Now.Year.ToString();

            // Ensure database schema columns exist and have sufficient capacity
            try
            {
                using (SqlConnection conSchema = Database.GetConnectionObj())
                {
                    if (conSchema != null)
                    {
                        SqlCommand cmdSchema = new SqlCommand(@"
                            IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'H_file_list' AND COLUMN_NAME = 'exm_code')
                                ALTER TABLE dbo.H_file_list ADD exm_code varchar(10) NULL;

                            IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'H_file_list' AND COLUMN_NAME = 'exm_year')
                                ALTER TABLE dbo.H_file_list ADD exm_year varchar(4) NULL;

                            IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'H_solving' AND COLUMN_NAME = 'exm_code')
                                ALTER TABLE dbo.H_solving ADD exm_code varchar(10) NULL;

                            IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'H_solving' AND COLUMN_NAME = 'exm_year')
                                ALTER TABLE dbo.H_solving ADD exm_year varchar(10) NULL;

                            -- Ensure column lengths are wide enough so bulk copy never fails
                            IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'H_solving' AND COLUMN_NAME = 'exam_code' AND (DATA_TYPE = 'char' OR CHARACTER_MAXIMUM_LENGTH < 10))
                                ALTER TABLE dbo.H_solving ALTER COLUMN exam_code varchar(10) NULL;

                            IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'H_solving' AND COLUMN_NAME = 'err_dscr' AND CHARACTER_MAXIMUM_LENGTH < 255)
                                ALTER TABLE dbo.H_solving ALTER COLUMN err_dscr varchar(255) NULL;

                            IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'H_solving' AND COLUMN_NAME = 'exm_year' AND CHARACTER_MAXIMUM_LENGTH < 10)
                                ALTER TABLE dbo.H_solving ALTER COLUMN exm_year varchar(10) NULL;
                        ", conSchema);
                        cmdSchema.ExecuteNonQuery();
                    }
                }
            }
            catch { }

            DirectoryInfo dr = new DirectoryInfo(targetFolder);
            if (!dr.Exists)
            {
                dr.Create();
            }

            // Filter out system, hidden, and non-scan files
            FileInfo[] rawFiles = dr.GetFiles();
            List<FileInfo> files = new List<FileInfo>();
            foreach (FileInfo fi in rawFiles)
            {
                if ((fi.Attributes & FileAttributes.Hidden) != 0 || (fi.Attributes & FileAttributes.System) != 0)
                    continue;
                string ext = fi.Extension.ToLower();
                if (ext == ".png" || ext == ".jpg" || ext == ".exe" || ext == ".dll" || ext == ".zip" || ext == ".rar" || ext == ".pdf")
                    continue;
                if (fi.Name.Equals("Thumbs.db", StringComparison.OrdinalIgnoreCase) || fi.Name.Equals("desktop.ini", StringComparison.OrdinalIgnoreCase))
                    continue;
                files.Add(fi);
            }

            int totalFiles = files.Count;
            if (totalFiles == 0)
            {
                bgwUploader.ReportProgress(100, new UploadState
                {
                    Message = "No valid scan files found in folder.",
                    Log = "⚠️ No valid scan files found in: " + targetFolder,
                    Percent = 100,
                    Detail = "0 scan files found."
                });
                e.Result = new UploadResultSummary { TotalFiles = 0, SuccessFiles = 0, ErrorFiles = 0, TotalRows = 0 };
                return;
            }

            int totalRowsInsertedAllFiles = 0;
            int successfulFilesCount = 0;
            int failedFilesCount = 0;
            List<string> failedFileNames = new List<string>();

            for (int fIndex = 0; fIndex < totalFiles; fIndex++)
            {
                if (bgwUploader.CancellationPending || isStopped)
                {
                    e.Cancel = true;
                    return;
                }

                while (isPaused && !isStopped && !bgwUploader.CancellationPending)
                {
                    System.Threading.Thread.Sleep(200);
                }

                if (bgwUploader.CancellationPending || isStopped)
                {
                    e.Cancel = true;
                    return;
                }

                FileInfo file = files[fIndex];
                string filename = file.Name;
                string fullpath = file.FullName;

                string sub_code = "";
                if (filename.Length >= 6)
                    sub_code = filename.Substring(0, 6);

                Database.sub_code = sub_code;

                int fileStartPercent = (int)(((double)fIndex / totalFiles) * 100);
                bgwUploader.ReportProgress(fileStartPercent, new UploadState
                {
                    Message = string.Format("Reading & parsing file {0}/{1}: {2}...", fIndex + 1, totalFiles, filename),
                    Log = string.Format("⏳ [{0}/{1}] Reading {2}...", fIndex + 1, totalFiles, filename),
                    Percent = fileStartPercent,
                    Detail = string.Format("{0}% ({1}/{2} files | Exam: {3}/{4})", fileStartPercent, fIndex + 1, totalFiles, exmCode, exmYear)
                });

                try
                {
                    // 1. Clean up previous rows if re-uploading and ensure H_file_list entry exists
                    using (SqlConnection conPrep = Database.GetConnectionObj())
                    {
                        if (conPrep != null)
                        {
                            try
                            {
                                using (SqlCommand cmdDel = new SqlCommand("DELETE FROM dbo.H_solving WHERE file_name = @fn", conPrep))
                                {
                                    cmdDel.Parameters.AddWithValue("@fn", filename);
                                    cmdDel.ExecuteNonQuery();
                                }
                            }
                            catch { }

                            string upsertFileList = @"
                                IF EXISTS (SELECT 1 FROM dbo.H_file_list WHERE file_name = @fn)
                                BEGIN
                                    UPDATE dbo.H_file_list 
                                    SET pap_code = @pc, exm_code = @ec, exm_year = @ey, insert_date = CURRENT_TIMESTAMP, solve_status = 'PENDING'
                                    WHERE file_name = @fn;
                                END
                                ELSE
                                BEGIN
                                    INSERT INTO dbo.H_file_list (file_name, pap_code, exm_code, exm_year, insert_date, solve_status)
                                    VALUES (@fn, @pc, @ec, @ey, CURRENT_TIMESTAMP, 'PENDING');
                                END";

                            using (SqlCommand cmdUpsert = new SqlCommand(upsertFileList, conPrep))
                            {
                                cmdUpsert.Parameters.AddWithValue("@fn", filename.Length > 30 ? filename.Substring(0, 30) : filename);
                                cmdUpsert.Parameters.AddWithValue("@pc", sub_code.Length > 6 ? sub_code.Substring(0, 6) : sub_code);
                                cmdUpsert.Parameters.AddWithValue("@ec", exmCode.Length > 6 ? exmCode.Substring(0, 6) : exmCode);
                                cmdUpsert.Parameters.AddWithValue("@ey", exmYear.Length > 4 ? exmYear.Substring(0, 4) : exmYear);
                                cmdUpsert.ExecuteNonQuery();
                            }
                        }
                    }

                    string[] allLines = File.ReadAllLines(fullpath);
                    int totalLines = allLines.Length;

                    // 2. Format detection
                    bool fileIsEB6 = false;
                    if (formatMode == "EB6")
                    {
                        fileIsEB6 = true;
                    }
                    else if (formatMode == "EB5")
                    {
                        fileIsEB6 = false;
                    }
                    else // AUTO mode
                    {
                        int eb6Votes = 0;
                        int eb5Votes = 0;
                        int sampleCount = Math.Min(totalLines, 100);

                        for (int si = 0; si < sampleCount; si++)
                        {
                            string sLine = allLines[si];
                            if (string.IsNullOrEmpty(sLine) || sLine.Length < 140) continue;

                            string bdexH = sLine.Substring(50, 4);
                            string h107 = sLine.Substring(107, 4);
                            string h108 = sLine.Substring(108, 4);

                            if (bdexH == h108 && bdexH != h107) eb6Votes += 2;
                            else if (bdexH == h107 && bdexH != h108) eb5Votes += 2;

                            if (sLine.Length > 100)
                            {
                                if (sLine[99] != ' ' && sLine[100] == ' ') eb6Votes++;
                                else if (sLine[99] == ' ') eb5Votes++;
                            }
                        }

                        fileIsEB6 = (eb6Votes >= eb5Votes && eb6Votes > 0);
                    }

                    // 3. Build in-memory DataTable for high-speed SqlBulkCopy
                    DataTable dtSolving = new DataTable();
                    dtSolving.Columns.Add("file_name", typeof(string));
                    dtSolving.Columns.Add("scanned_row", typeof(string));
                    dtSolving.Columns.Add("serial", typeof(int));
                    dtSolving.Columns.Add("pap_code", typeof(string));
                    dtSolving.Columns.Add("bdexcode", typeof(string));
                    dtSolving.Columns.Add("bhexcode", typeof(string));
                    dtSolving.Columns.Add("marks", typeof(string));
                    dtSolving.Columns.Add("hmarks", typeof(string));
                    dtSolving.Columns.Add("eb_no", typeof(string));
                    dtSolving.Columns.Add("sl_no", typeof(string));
                    dtSolving.Columns.Add("updates", typeof(string));
                    dtSolving.Columns.Add("err_dscr", typeof(string));
                    dtSolving.Columns.Add("exam_code", typeof(string));
                    dtSolving.Columns.Add("exm_code", typeof(string));
                    dtSolving.Columns.Add("exm_year", typeof(string));

                    string safeExamCode = exmCode.Length > 4 ? exmCode.Substring(0, 4) : exmCode;
                    string safeExmCode = exmCode.Length > 10 ? exmCode.Substring(0, 10) : exmCode;
                    string safeExmYear = exmYear.Length > 4 ? exmYear.Substring(0, 4) : exmYear;
                    string safeFileName = filename.Length > 30 ? filename.Substring(0, 30) : filename;

                    for (int sl = 0; sl < totalLines; sl++)
                    {
                        if (bgwUploader.CancellationPending || isStopped)
                        {
                            e.Cancel = true;
                            return;
                        }

                        while (isPaused && !isStopped && !bgwUploader.CancellationPending)
                        {
                            System.Threading.Thread.Sleep(200);
                        }

                        string line = allLines[sl];
                        if (string.IsNullOrEmpty(line) || line.Length < 90) continue;

                        bool isEB6 = fileIsEB6;
                        if (formatMode == "EB6")
                        {
                            isEB6 = true;
                        }
                        else if (formatMode == "EB5")
                        {
                            isEB6 = false;
                        }
                        else
                        {
                            if (line.Length >= 140)
                            {
                                string bdexHeader = line.Substring(50, 4);
                                string hex107Header = line.Substring(107, 4);
                                string hex108Header = line.Substring(108, 4);

                                if (bdexHeader == hex108Header && bdexHeader != hex107Header)
                                    isEB6 = true;
                                else if (bdexHeader == hex107Header && bdexHeader != hex108Header)
                                    isEB6 = false;
                                else if (line.Length > 100)
                                {
                                    if (line[99] != ' ' && line[100] == ' ')
                                        isEB6 = true;
                                    else if (line[99] == ' ')
                                        isEB6 = false;
                                    else
                                        isEB6 = fileIsEB6;
                                }
                                else
                                {
                                    isEB6 = fileIsEB6;
                                }
                            }
                            else if (line.Length > 100)
                            {
                                if (line[99] != ' ' && line[100] == ' ')
                                    isEB6 = true;
                                else if (line[99] == ' ')
                                    isEB6 = false;
                                else
                                    isEB6 = fileIsEB6;
                            }
                            else
                            {
                                isEB6 = fileIsEB6;
                            }
                        }

                        string bdexcode = line.Length >= 82 ? line.Substring(50, Math.Min(32, line.Length - 50)).Replace(' ', '0') : "";
                        string pap_code = "";
                        string eb_no = "";
                        string sl_no = "";
                        string marks = "";
                        string hmarks = "";
                        string bhexcode = "";

                        if (isEB6)
                        {
                            pap_code = line.Length >= 88 ? line.Substring(82, Math.Min(6, line.Length - 82)).Replace(' ', '*') : "";
                            eb_no    = line.Length >= 94 ? line.Substring(88, Math.Min(6, line.Length - 88)).Replace(' ', '*') : "";
                            sl_no    = line.Length >= 97 ? line.Substring(94, Math.Min(3, line.Length - 94)).Replace(' ', '*') : "";
                            marks    = line.Length >= 100 ? line.Substring(97, Math.Min(3, line.Length - 97)).Replace(' ', '*') : "";
                            hmarks   = line.Length >= 108 ? line.Substring(105, Math.Min(3, line.Length - 105)).Replace('*', '*') : "000";
                        }
                        else
                        {
                            pap_code = line.Length >= 88 ? line.Substring(82, Math.Min(6, line.Length - 82)).Replace(' ', '*') : "";
                            eb_no    = line.Length >= 93 ? line.Substring(88, Math.Min(5, line.Length - 88)).Replace(' ', '*') : "";
                            sl_no    = line.Length >= 96 ? line.Substring(93, Math.Min(3, line.Length - 93)).Replace(' ', '*') : "";
                            marks    = line.Length >= 99 ? line.Substring(96, Math.Min(3, line.Length - 96)).Replace(' ', '*') : "";
                            hmarks   = line.Length >= 107 ? line.Substring(104, Math.Min(3, line.Length - 104)).Replace('*', '*') : "000";
                        }

                        if (line.Length >= 139)
                        {
                            string c107 = line.Substring(107, Math.Min(32, line.Length - 107)).Replace(' ', '0');
                            string c108 = line.Length >= 140 ? line.Substring(108, Math.Min(32, line.Length - 108)).Replace(' ', '0') : c107;

                            if (bdexcode.Length >= 4 && c107.StartsWith(bdexcode.Substring(0, 4)))
                            {
                                bhexcode = c107;
                            }
                            else if (bdexcode.Length >= 4 && c108.StartsWith(bdexcode.Substring(0, 4)))
                            {
                                bhexcode = c108;
                            }
                            else
                            {
                                bhexcode = isEB6 ? c108 : c107;
                            }
                        }
                        else
                        {
                            bhexcode = bdexcode;
                        }

                        string err_dscr =
                            Database.checkSubjectCode(pap_code) +
                            Database.checkExaminer(eb_no) +
                            Database.checkSerial(sl_no) +
                            Database.checkMarks(marks) +
                            Database.checkLitho("", "", bdexcode, bhexcode);

                        // Clamp strings safely to table boundaries
                        string safeRow = line.Length > 160 ? line.Substring(0, 160) : line;
                        string safePap = pap_code.Length > 6 ? pap_code.Substring(0, 6) : pap_code;
                        string safeBdex = bdexcode.Length > 32 ? bdexcode.Substring(0, 32) : bdexcode;
                        string safeBhex = bhexcode.Length > 32 ? bhexcode.Substring(0, 32) : bhexcode;
                        string safeMarks = marks.Length > 3 ? marks.Substring(0, 3) : marks;
                        string safeHmarks = hmarks.Length > 3 ? hmarks.Substring(0, 3) : hmarks;
                        string safeEb = eb_no.Length > 6 ? eb_no.Substring(0, 6) : eb_no;
                        string safeSl = sl_no.Length > 3 ? sl_no.Substring(0, 3) : sl_no;
                        string safeErr = err_dscr.Length > 100 ? err_dscr.Substring(0, 100) : err_dscr;

                        dtSolving.Rows.Add(
                            safeFileName,
                            safeRow,
                            sl + 1,
                            safePap,
                            safeBdex,
                            safeBhex,
                            safeMarks,
                            safeHmarks,
                            safeEb,
                            safeSl,
                            "",
                            safeErr,
                            safeExamCode,
                            safeExmCode,
                            safeExmYear
                        );

                        // Throttle progress updates during in-memory parsing (update status bar every 500 rows without adding to richTextBox)
                        if ((sl + 1) % 500 == 0 || sl + 1 == totalLines)
                        {
                            int rowPct = (int)(((fIndex + ((double)(sl + 1) / totalLines)) / totalFiles) * 100);
                            bgwUploader.ReportProgress(rowPct, new UploadState
                            {
                                Message = string.Format("Parsing [{0}] {1} ({2}/{3} rows)...", isEB6 ? "EB6" : "EB5", filename, sl + 1, totalLines),
                                Log = null, // Do not spam RichTextBox with every row
                                Percent = rowPct,
                                Detail = string.Format("{0}% ({1}/{2} files | Row {3}/{4})", rowPct, fIndex + 1, totalFiles, sl + 1, totalLines)
                            });
                        }
                    }

                    // 4. Ultra-fast bulk insert via SqlBulkCopy
                    if (dtSolving.Rows.Count > 0)
                    {
                        using (SqlConnection conBulk = Database.GetConnectionObj())
                        {
                            if (conBulk != null)
                            {
                                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conBulk))
                                {
                                    bulkCopy.DestinationTableName = "dbo.H_solving";
                                    bulkCopy.BulkCopyTimeout = 180;
                                    bulkCopy.BatchSize = 5000;

                                    bulkCopy.ColumnMappings.Add("file_name", "file_name");
                                    bulkCopy.ColumnMappings.Add("scanned_row", "scanned_row");
                                    bulkCopy.ColumnMappings.Add("serial", "serial");
                                    bulkCopy.ColumnMappings.Add("pap_code", "pap_code");
                                    bulkCopy.ColumnMappings.Add("bdexcode", "bdexcode");
                                    bulkCopy.ColumnMappings.Add("bhexcode", "bhexcode");
                                    bulkCopy.ColumnMappings.Add("marks", "marks");
                                    bulkCopy.ColumnMappings.Add("hmarks", "hmarks");
                                    bulkCopy.ColumnMappings.Add("eb_no", "eb_no");
                                    bulkCopy.ColumnMappings.Add("sl_no", "sl_no");
                                    bulkCopy.ColumnMappings.Add("updates", "updates");
                                    bulkCopy.ColumnMappings.Add("err_dscr", "err_dscr");
                                    bulkCopy.ColumnMappings.Add("exam_code", "exam_code");
                                    bulkCopy.ColumnMappings.Add("exm_code", "exm_code");
                                    bulkCopy.ColumnMappings.Add("exm_year", "exm_year");

                                    bulkCopy.WriteToServer(dtSolving);
                                }

                                // Update row count in H_file_list
                                using (SqlCommand cmdUpdRows = new SqlCommand("UPDATE dbo.H_file_list SET rows = @rc WHERE file_name = @fn", conBulk))
                                {
                                    cmdUpdRows.Parameters.AddWithValue("@rc", dtSolving.Rows.Count);
                                    cmdUpdRows.Parameters.AddWithValue("@fn", safeFileName);
                                    cmdUpdRows.ExecuteNonQuery();
                                }
                            }
                        }
                    }

                    totalRowsInsertedAllFiles += dtSolving.Rows.Count;
                    successfulFilesCount++;

                    int overallPercent = (int)(((double)(fIndex + 1) / totalFiles) * 100);
                    string detectedTag = fileIsEB6 ? "EB6" : "EB5";
                    bgwUploader.ReportProgress(overallPercent, new UploadState
                    {
                        Message = string.Format("✅ Uploaded [{0}] file {1}/{2}: {3}", detectedTag, fIndex + 1, totalFiles, filename),
                        Log = string.Format("✅ [{0}] {1} - {2} rows uploaded successfully.", detectedTag, filename, dtSolving.Rows.Count),
                        Percent = overallPercent,
                        Detail = string.Format("{0}% ({1}/{2} files complete | Total rows: {3})", overallPercent, fIndex + 1, totalFiles, totalRowsInsertedAllFiles)
                    });
                }
                catch (Exception ex)
                {
                    failedFilesCount++;
                    failedFileNames.Add(filename);
                    bgwUploader.ReportProgress(fileStartPercent, new UploadState
                    {
                        Message = "❌ Error uploading " + filename,
                        Log = "❌ Error uploading " + filename + ": " + ex.Message,
                        Percent = fileStartPercent,
                        Detail = "Error on " + filename
                    });
                }
            }

            e.Result = new UploadResultSummary
            {
                TotalFiles = totalFiles,
                SuccessFiles = successfulFilesCount,
                ErrorFiles = failedFilesCount,
                TotalRows = totalRowsInsertedAllFiles,
                FailedFileNames = failedFileNames
            };
        }

        private void BgwUploader_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            UploadState state = e.UserState as UploadState;
            if (state != null)
            {
                this.progressBar1.Value = Math.Max(0, Math.Min(100, state.Percent));
                if (!string.IsNullOrEmpty(state.Message))
                    this.label2.Text = state.Message;
                if (!string.IsNullOrEmpty(state.Detail))
                    this.lblProgressPct.Text = state.Detail;
                if (!string.IsNullOrEmpty(state.Log))
                {
                    this.richTextBox1.AppendText(state.Log + "\n");
                    // Keep RichTextBox from blowing up memory or stalling layout
                    if (this.richTextBox1.Lines.Length > 500)
                    {
                        int firstChar = this.richTextBox1.GetFirstCharIndexFromLine(100);
                        if (firstChar > 0)
                        {
                            this.richTextBox1.Select(0, firstChar);
                            this.richTextBox1.SelectedText = "";
                        }
                    }
                    this.richTextBox1.ScrollToCaret();
                }
            }
        }

        private void BgwUploader_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                this.button1.Enabled = true;
                this.btnBrowse.Enabled = true;
                this.tbFolderPath.Enabled = true;
                this.tbExmCode.Enabled = true;
                this.tbExmYear.Enabled = true;
                this.gbFormat.Enabled = true;

                this.btnPause.Enabled = false;
                this.btnResume.Enabled = false;
                this.btnStop.Enabled = true;

                if (e.Error != null)
                {
                    this.label2.Text = "❌ Upload Failed with Error";
                    this.lblProgressPct.Text = "Error during upload: " + e.Error.Message;
                    this.richTextBox1.AppendText("❌ ERROR: " + e.Error.Message + "\n");
                    this.richTextBox1.ScrollToCaret();
                    MessageBox.Show(this, "An error occurred during upload:\n\n" + e.Error.Message, "Upload Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (e.Cancelled || isStopped)
                {
                    this.label2.Text = "⏸ Upload Stopped by User";
                    this.lblProgressPct.Text = "Upload process was stopped.";
                    MessageBox.Show(this, "Upload process stopped.", "Upload Stopped", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (callerForm != null) callerForm.Show();
                    this.Close();
                    return;
                }

                UploadResultSummary summary = e.Result as UploadResultSummary;
                int totalFiles = summary != null ? summary.TotalFiles : 0;
                int successFiles = summary != null ? summary.SuccessFiles : 0;
                int errorFiles = summary != null ? summary.ErrorFiles : 0;
                int totalRows = summary != null ? summary.TotalRows : 0;

                this.progressBar1.Value = 100;
                this.label2.Text = errorFiles == 0 ? "✅ Upload Completed!" : string.Format("⚠️ Upload Completed with {0} Errors", errorFiles);
                this.lblProgressPct.Text = errorFiles == 0 
                    ? string.Format("100% (All {0} files uploaded successfully | {1} rows)", successFiles, totalRows)
                    : string.Format("Completed: {0}/{1} files succeeded, {2} failed.", successFiles, totalFiles, errorFiles);

                this.richTextBox1.AppendText("=====================================\n");
                if (errorFiles == 0)
                {
                    this.richTextBox1.AppendText("🎉 Upload completed successfully! All files processed.\n");
                }
                else
                {
                    this.richTextBox1.AppendText(string.Format("⚠️ Upload completed with warnings: {0}/{1} files uploaded, {2} failed.\n", successFiles, totalFiles, errorFiles));
                }
                this.richTextBox1.ScrollToCaret();

                if (errorFiles == 0 && successFiles > 0)
                {
                    DialogResult dialogResult = MessageBox.Show(
                        this,
                        string.Format("All {0} scanner files uploaded successfully ({1:N0} total rows)!\n\n" +
                        "• Click 'Yes' to proceed to Solving Workspace (Login).\n" +
                        "• Click 'No' to return to Uploader Dashboard.", successFiles, totalRows),
                        "Upload Complete",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Information);

                    if (dialogResult == DialogResult.Yes)
                    {
                        login upload_frm = new login();
                        upload_frm.Show();
                        this.Hide();
                    }
                    else
                    {
                        if (callerForm != null) callerForm.Show();
                        this.Close();
                    }
                }
                else if (successFiles > 0 && errorFiles > 0)
                {
                    MessageBox.Show(
                        this,
                        string.Format("Partial Upload Completed:\n\n" +
                        "• Successfully uploaded: {0} files ({1:N0} rows)\n" +
                        "• Failed files: {2}\n\n" +
                        "Please check the log panel below for details on failed files.", successFiles, totalRows, errorFiles),
                        "Upload Completed with Errors",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show(
                        this,
                        string.Format("No files were uploaded successfully ({0} failed).\n\n" +
                        "Please check the log panel below for specific error messages.", errorFiles),
                        "Upload Failed",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error completing upload: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void exit(object sender, FormClosedEventArgs e)
        {
            if (callerForm != null)
            {
                callerForm.Show();
            }
        }

        private void upload_Load(object sender, EventArgs e)
        {

        }
    }

    public class UploadResultSummary
    {
        public int TotalFiles { get; set; }
        public int SuccessFiles { get; set; }
        public int ErrorFiles { get; set; }
        public int TotalRows { get; set; }
        public List<string> FailedFileNames { get; set; }

        public UploadResultSummary()
        {
            FailedFileNames = new List<string>();
        }
    }
}
