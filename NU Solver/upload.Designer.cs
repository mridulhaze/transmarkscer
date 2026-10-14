namespace NU_Solver
{
    partial class upload
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.btnPause = new System.Windows.Forms.Button();
            this.btnResume = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.lblProgressPct = new System.Windows.Forms.Label();
            this.gbFormat = new System.Windows.Forms.GroupBox();
            this.rbAuto = new System.Windows.Forms.RadioButton();
            this.rbEB6 = new System.Windows.Forms.RadioButton();
            this.rbEB5 = new System.Windows.Forms.RadioButton();
            this.lblExmCode = new System.Windows.Forms.Label();
            this.tbExmCode = new System.Windows.Forms.TextBox();
            this.lblExmYear = new System.Windows.Forms.Label();
            this.tbExmYear = new System.Windows.Forms.TextBox();
            this.lblFolder = new System.Windows.Forms.Label();
            this.tbFolderPath = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.lblCredit = new System.Windows.Forms.Label();
            this.gbFormat.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.label1.Location = new System.Drawing.Point(15, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(350, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Specify Exam Code & Year, select folder, and start background upload.";
            // 
            // gbFormat
            // 
            this.gbFormat.Controls.Add(this.rbAuto);
            this.gbFormat.Controls.Add(this.rbEB6);
            this.gbFormat.Controls.Add(this.rbEB5);
            this.gbFormat.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.gbFormat.Location = new System.Drawing.Point(15, 32);
            this.gbFormat.Name = "gbFormat";
            this.gbFormat.Size = new System.Drawing.Size(370, 52);
            this.gbFormat.TabIndex = 1;
            this.gbFormat.TabStop = false;
            this.gbFormat.Text = "Format Detection Mode";
            // 
            // rbAuto
            // 
            this.rbAuto.AutoSize = true;
            this.rbAuto.Checked = true;
            this.rbAuto.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.rbAuto.Location = new System.Drawing.Point(15, 22);
            this.rbAuto.Name = "rbAuto";
            this.rbAuto.Size = new System.Drawing.Size(89, 19);
            this.rbAuto.TabIndex = 0;
            this.rbAuto.TabStop = true;
            this.rbAuto.Text = "Auto Detect";
            this.rbAuto.UseVisualStyleBackColor = true;
            // 
            // rbEB5
            // 
            this.rbEB5.AutoSize = true;
            this.rbEB5.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.rbEB5.Location = new System.Drawing.Point(125, 22);
            this.rbEB5.Name = "rbEB5";
            this.rbEB5.Size = new System.Drawing.Size(95, 19);
            this.rbEB5.TabIndex = 1;
            this.rbEB5.Text = "EB5 (5-digit)";
            this.rbEB5.UseVisualStyleBackColor = true;
            // 
            // rbEB6
            // 
            this.rbEB6.AutoSize = true;
            this.rbEB6.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.rbEB6.Location = new System.Drawing.Point(245, 22);
            this.rbEB6.Name = "rbEB6";
            this.rbEB6.Size = new System.Drawing.Size(95, 19);
            this.rbEB6.TabIndex = 2;
            this.rbEB6.Text = "EB6 (6-digit)";
            this.rbEB6.UseVisualStyleBackColor = true;
            // 
            // lblExmCode
            // 
            this.lblExmCode.AutoSize = true;
            this.lblExmCode.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblExmCode.Location = new System.Drawing.Point(15, 92);
            this.lblExmCode.Name = "lblExmCode";
            this.lblExmCode.Size = new System.Drawing.Size(68, 15);
            this.lblExmCode.TabIndex = 2;
            this.lblExmCode.Text = "Exam Code:";
            // 
            // tbExmCode
            // 
            this.tbExmCode.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tbExmCode.Location = new System.Drawing.Point(15, 110);
            this.tbExmCode.Name = "tbExmCode";
            this.tbExmCode.Size = new System.Drawing.Size(170, 23);
            this.tbExmCode.TabIndex = 3;
            this.tbExmCode.Text = "201";
            // 
            // lblExmYear
            // 
            this.lblExmYear.AutoSize = true;
            this.lblExmYear.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblExmYear.Location = new System.Drawing.Point(200, 92);
            this.lblExmYear.Name = "lblExmYear";
            this.lblExmYear.Size = new System.Drawing.Size(66, 15);
            this.lblExmYear.TabIndex = 4;
            this.lblExmYear.Text = "Exam Year:";
            // 
            // tbExmYear
            // 
            this.tbExmYear.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tbExmYear.Location = new System.Drawing.Point(200, 110);
            this.tbExmYear.Name = "tbExmYear";
            this.tbExmYear.Size = new System.Drawing.Size(185, 23);
            this.tbExmYear.TabIndex = 5;
            this.tbExmYear.Text = "2026";
            // 
            // lblFolder
            // 
            this.lblFolder.AutoSize = true;
            this.lblFolder.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblFolder.Location = new System.Drawing.Point(15, 142);
            this.lblFolder.Name = "lblFolder";
            this.lblFolder.Size = new System.Drawing.Size(107, 15);
            this.lblFolder.TabIndex = 6;
            this.lblFolder.Text = "Scan Files Folder:";
            // 
            // tbFolderPath
            // 
            this.tbFolderPath.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tbFolderPath.Location = new System.Drawing.Point(15, 160);
            this.tbFolderPath.Name = "tbFolderPath";
            this.tbFolderPath.Size = new System.Drawing.Size(275, 23);
            this.tbFolderPath.TabIndex = 7;
            this.tbFolderPath.Text = "D:\\working\\H_Scanned";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.btnBrowse.Location = new System.Drawing.Point(295, 159);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(90, 25);
            this.btnBrowse.TabIndex = 8;
            this.btnBrowse.Text = "Browse...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.button1.Location = new System.Drawing.Point(15, 195);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(370, 36);
            this.button1.TabIndex = 9;
            this.button1.Text = "🚀 Start Upload";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.upload_Click);
            // 
            // btnPause
            // 
            this.btnPause.Enabled = false;
            this.btnPause.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.btnPause.Location = new System.Drawing.Point(15, 238);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(118, 32);
            this.btnPause.TabIndex = 10;
            this.btnPause.Text = "⏸ Pause";
            this.btnPause.UseVisualStyleBackColor = true;
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            // 
            // btnResume
            // 
            this.btnResume.Enabled = false;
            this.btnResume.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.btnResume.Location = new System.Drawing.Point(141, 238);
            this.btnResume.Name = "btnResume";
            this.btnResume.Size = new System.Drawing.Size(118, 32);
            this.btnResume.TabIndex = 11;
            this.btnResume.Text = "▶ Resume";
            this.btnResume.UseVisualStyleBackColor = true;
            this.btnResume.Click += new System.EventHandler(this.btnResume_Click);
            // 
            // btnStop
            // 
            this.btnStop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.btnStop.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.btnStop.ForeColor = System.Drawing.Color.White;
            this.btnStop.Location = new System.Drawing.Point(267, 238);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(118, 32);
            this.btnStop.TabIndex = 12;
            this.btnStop.Text = "⏹ Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(15, 278);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(110, 15);
            this.label2.TabIndex = 13;
            this.label2.Text = "Ready to upload";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(15, 298);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(370, 22);
            this.progressBar1.TabIndex = 14;
            // 
            // lblProgressPct
            // 
            this.lblProgressPct.AutoSize = true;
            this.lblProgressPct.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblProgressPct.ForeColor = System.Drawing.Color.Gray;
            this.lblProgressPct.Location = new System.Drawing.Point(15, 324);
            this.lblProgressPct.Name = "lblProgressPct";
            this.lblProgressPct.Size = new System.Drawing.Size(125, 15);
            this.lblProgressPct.TabIndex = 15;
            this.lblProgressPct.Text = "0% (0 / 0 rows uploaded)";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Font = new System.Drawing.Font("Consolas", 8.5F);
            this.richTextBox1.Location = new System.Drawing.Point(15, 344);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(370, 195);
            this.richTextBox1.TabIndex = 16;
            this.richTextBox1.Text = "";
            // 
            // lblCredit
            // 
            this.lblCredit.AutoSize = true;
            this.lblCredit.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblCredit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(4)))), ((int)(((byte)(120)))), ((int)(((byte)(87)))));
            this.lblCredit.Location = new System.Drawing.Point(15, 546);
            this.lblCredit.Name = "lblCredit";
            this.lblCredit.Size = new System.Drawing.Size(315, 15);
            this.lblCredit.TabIndex = 17;
            this.lblCredit.Text = "©Mridul Roy";
            // 
            // upload
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 575);
            this.Controls.Add(this.lblCredit);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnResume);
            this.Controls.Add(this.btnPause);
            this.Controls.Add(this.tbExmYear);
            this.Controls.Add(this.lblExmYear);
            this.Controls.Add(this.tbExmCode);
            this.Controls.Add(this.lblExmCode);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.tbFolderPath);
            this.Controls.Add(this.lblFolder);
            this.Controls.Add(this.gbFormat);
            this.Controls.Add(this.lblProgressPct);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "upload";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Upload Scan Files";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.exit);
            this.Load += new System.EventHandler(this.upload_Load);
            this.gbFormat.ResumeLayout(false);
            this.gbFormat.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.Button btnResume;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label lblProgressPct;
        private System.Windows.Forms.GroupBox gbFormat;
        private System.Windows.Forms.RadioButton rbAuto;
        private System.Windows.Forms.RadioButton rbEB6;
        private System.Windows.Forms.RadioButton rbEB5;
        private System.Windows.Forms.Label lblFolder;
        private System.Windows.Forms.TextBox tbFolderPath;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Label lblExmCode;
        private System.Windows.Forms.TextBox tbExmCode;
        private System.Windows.Forms.Label lblExmYear;
        private System.Windows.Forms.TextBox tbExmYear;
        private System.Windows.Forms.Label lblCredit;
    }
}