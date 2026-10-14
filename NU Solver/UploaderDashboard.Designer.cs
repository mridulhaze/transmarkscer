namespace NU_Solver
{
    partial class UploaderDashboard
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.pnlMetrics = new System.Windows.Forms.Panel();
            this.pnlCard1 = new System.Windows.Forms.Panel();
            this.lblTotalFilesVal = new System.Windows.Forms.Label();
            this.lblTotalFilesTitle = new System.Windows.Forms.Label();
            this.pnlCard2 = new System.Windows.Forms.Panel();
            this.lblSolvedFilesVal = new System.Windows.Forms.Label();
            this.lblSolvedFilesTitle = new System.Windows.Forms.Label();
            this.pnlCard3 = new System.Windows.Forms.Panel();
            this.lblUnsolvedFilesVal = new System.Windows.Forms.Label();
            this.lblUnsolvedFilesTitle = new System.Windows.Forms.Label();
            this.pnlCard4 = new System.Windows.Forms.Panel();
            this.lblActiveSolversVal = new System.Windows.Forms.Label();
            this.lblActiveSolversTitle = new System.Windows.Forms.Label();
            this.pnlMainLayout = new System.Windows.Forms.TableLayoutPanel();
            this.pnlLeftGroup = new System.Windows.Forms.GroupBox();
            this.dgvSolverReport = new System.Windows.Forms.DataGridView();
            this.pnlRightGroup = new System.Windows.Forms.GroupBox();
            this.dgvUnsolvedFiles = new System.Windows.Forms.DataGridView();
            this.pnlFileActions = new System.Windows.Forms.Panel();
            this.lblAssignTo = new System.Windows.Forms.Label();
            this.cmbAssignSolver = new System.Windows.Forms.ComboBox();
            this.btnAssignSolver = new System.Windows.Forms.Button();
            this.btnUnassignFile = new System.Windows.Forms.Button();
            this.btnDeleteFile = new System.Windows.Forms.Button();
            this.pnlBottomBar = new System.Windows.Forms.Panel();
            this.btnOpenUpload = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnConvertHex = new System.Windows.Forms.Button();
            this.btnDownloadExcel = new System.Windows.Forms.Button();
            this.btnAddUser = new System.Windows.Forms.Button();
            this.btnCreateTables = new System.Windows.Forms.Button();
            this.btnCreateMtemp = new System.Windows.Forms.Button();
            this.btnViewTranscript = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.pnlMetrics.SuspendLayout();
            this.pnlCard1.SuspendLayout();
            this.pnlCard2.SuspendLayout();
            this.pnlCard3.SuspendLayout();
            this.pnlCard4.SuspendLayout();
            this.pnlMainLayout.SuspendLayout();
            this.pnlLeftGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSolverReport)).BeginInit();
            this.pnlRightGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUnsolvedFiles)).BeginInit();
            this.pnlFileActions.SuspendLayout();
            this.pnlBottomBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMetrics
            // 
            this.pnlMetrics.Controls.Add(this.pnlCard1);
            this.pnlMetrics.Controls.Add(this.pnlCard2);
            this.pnlMetrics.Controls.Add(this.pnlCard3);
            this.pnlMetrics.Controls.Add(this.pnlCard4);
            this.pnlMetrics.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlMetrics.Location = new System.Drawing.Point(12, 12);
            this.pnlMetrics.Name = "pnlMetrics";
            this.pnlMetrics.Size = new System.Drawing.Size(960, 90);
            this.pnlMetrics.TabIndex = 0;
            // 
            // pnlCard1
            // 
            this.pnlCard1.BackColor = System.Drawing.Color.White;
            this.pnlCard1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlCard1.Controls.Add(this.lblTotalFilesVal);
            this.pnlCard1.Controls.Add(this.lblTotalFilesTitle);
            this.pnlCard1.Location = new System.Drawing.Point(0, 8);
            this.pnlCard1.Name = "pnlCard1";
            this.pnlCard1.Size = new System.Drawing.Size(225, 74);
            this.pnlCard1.TabIndex = 0;
            // 
            // lblTotalFilesVal
            // 
            this.lblTotalFilesVal.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTotalFilesVal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(150)))), ((int)(((byte)(105)))));
            this.lblTotalFilesVal.Location = new System.Drawing.Point(10, 32);
            this.lblTotalFilesVal.Name = "lblTotalFilesVal";
            this.lblTotalFilesVal.Size = new System.Drawing.Size(200, 32);
            this.lblTotalFilesVal.TabIndex = 1;
            this.lblTotalFilesVal.Text = "0";
            // 
            // lblTotalFilesTitle
            // 
            this.lblTotalFilesTitle.AutoSize = true;
            this.lblTotalFilesTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblTotalFilesTitle.ForeColor = System.Drawing.Color.Gray;
            this.lblTotalFilesTitle.Location = new System.Drawing.Point(10, 10);
            this.lblTotalFilesTitle.Name = "lblTotalFilesTitle";
            this.lblTotalFilesTitle.Size = new System.Drawing.Size(120, 15);
            this.lblTotalFilesTitle.TabIndex = 0;
            this.lblTotalFilesTitle.Text = "TOTAL FILES INGESTED";
            // 
            // pnlCard2
            // 
            this.pnlCard2.BackColor = System.Drawing.Color.White;
            this.pnlCard2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlCard2.Controls.Add(this.lblSolvedFilesVal);
            this.pnlCard2.Controls.Add(this.lblSolvedFilesTitle);
            this.pnlCard2.Location = new System.Drawing.Point(240, 8);
            this.pnlCard2.Name = "pnlCard2";
            this.pnlCard2.Size = new System.Drawing.Size(225, 74);
            this.pnlCard2.TabIndex = 1;
            // 
            // lblSolvedFilesVal
            // 
            this.lblSolvedFilesVal.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblSolvedFilesVal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(150)))), ((int)(((byte)(105)))));
            this.lblSolvedFilesVal.Location = new System.Drawing.Point(10, 32);
            this.lblSolvedFilesVal.Name = "lblSolvedFilesVal";
            this.lblSolvedFilesVal.Size = new System.Drawing.Size(200, 32);
            this.lblSolvedFilesVal.TabIndex = 1;
            this.lblSolvedFilesVal.Text = "0";
            // 
            // lblSolvedFilesTitle
            // 
            this.lblSolvedFilesTitle.AutoSize = true;
            this.lblSolvedFilesTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblSolvedFilesTitle.ForeColor = System.Drawing.Color.Gray;
            this.lblSolvedFilesTitle.Location = new System.Drawing.Point(10, 10);
            this.lblSolvedFilesTitle.Name = "lblSolvedFilesTitle";
            this.lblSolvedFilesTitle.Size = new System.Drawing.Size(81, 15);
            this.lblSolvedFilesTitle.TabIndex = 0;
            this.lblSolvedFilesTitle.Text = "SOLVED FILES";
            // 
            // pnlCard3
            // 
            this.pnlCard3.BackColor = System.Drawing.Color.White;
            this.pnlCard3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlCard3.Controls.Add(this.lblUnsolvedFilesVal);
            this.pnlCard3.Controls.Add(this.lblUnsolvedFilesTitle);
            this.pnlCard3.Location = new System.Drawing.Point(480, 8);
            this.pnlCard3.Name = "pnlCard3";
            this.pnlCard3.Size = new System.Drawing.Size(225, 74);
            this.pnlCard3.TabIndex = 2;
            // 
            // lblUnsolvedFilesVal
            // 
            this.lblUnsolvedFilesVal.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblUnsolvedFilesVal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.lblUnsolvedFilesVal.Location = new System.Drawing.Point(10, 32);
            this.lblUnsolvedFilesVal.Name = "lblUnsolvedFilesVal";
            this.lblUnsolvedFilesVal.Size = new System.Drawing.Size(200, 32);
            this.lblUnsolvedFilesVal.TabIndex = 1;
            this.lblUnsolvedFilesVal.Text = "0";
            // 
            // lblUnsolvedFilesTitle
            // 
            this.lblUnsolvedFilesTitle.AutoSize = true;
            this.lblUnsolvedFilesTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblUnsolvedFilesTitle.ForeColor = System.Drawing.Color.Gray;
            this.lblUnsolvedFilesTitle.Location = new System.Drawing.Point(10, 10);
            this.lblUnsolvedFilesTitle.Name = "lblUnsolvedFilesTitle";
            this.lblUnsolvedFilesTitle.Size = new System.Drawing.Size(127, 15);
            this.lblUnsolvedFilesTitle.TabIndex = 0;
            this.lblUnsolvedFilesTitle.Text = "UNSOLVED / PENDING";
            // 
            // pnlCard4
            // 
            this.pnlCard4.BackColor = System.Drawing.Color.White;
            this.pnlCard4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlCard4.Controls.Add(this.lblActiveSolversVal);
            this.pnlCard4.Controls.Add(this.lblActiveSolversTitle);
            this.pnlCard4.Location = new System.Drawing.Point(720, 8);
            this.pnlCard4.Name = "pnlCard4";
            this.pnlCard4.Size = new System.Drawing.Size(225, 74);
            this.pnlCard4.TabIndex = 3;
            // 
            // lblActiveSolversVal
            // 
            this.lblActiveSolversVal.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblActiveSolversVal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(150)))), ((int)(((byte)(105)))));
            this.lblActiveSolversVal.Location = new System.Drawing.Point(10, 32);
            this.lblActiveSolversVal.Name = "lblActiveSolversVal";
            this.lblActiveSolversVal.Size = new System.Drawing.Size(200, 32);
            this.lblActiveSolversVal.TabIndex = 1;
            this.lblActiveSolversVal.Text = "0";
            // 
            // lblActiveSolversTitle
            // 
            this.lblActiveSolversTitle.AutoSize = true;
            this.lblActiveSolversTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblActiveSolversTitle.ForeColor = System.Drawing.Color.Gray;
            this.lblActiveSolversTitle.Location = new System.Drawing.Point(10, 10);
            this.lblActiveSolversTitle.Name = "lblActiveSolversTitle";
            this.lblActiveSolversTitle.Size = new System.Drawing.Size(99, 15);
            this.lblActiveSolversTitle.TabIndex = 0;
            this.lblActiveSolversTitle.Text = "ACTIVE SOLVERS";
            // 
            // pnlMainLayout
            // 
            this.pnlMainLayout.ColumnCount = 2;
            this.pnlMainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 42F));
            this.pnlMainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 58F));
            this.pnlMainLayout.Controls.Add(this.pnlLeftGroup, 0, 0);
            this.pnlMainLayout.Controls.Add(this.pnlRightGroup, 1, 0);
            this.pnlMainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMainLayout.Location = new System.Drawing.Point(12, 102);
            this.pnlMainLayout.Name = "pnlMainLayout";
            this.pnlMainLayout.RowCount = 1;
            this.pnlMainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlMainLayout.Size = new System.Drawing.Size(960, 420);
            this.pnlMainLayout.TabIndex = 1;
            // 
            // pnlLeftGroup
            // 
            this.pnlLeftGroup.Controls.Add(this.dgvSolverReport);
            this.pnlLeftGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLeftGroup.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.pnlLeftGroup.Location = new System.Drawing.Point(3, 3);
            this.pnlLeftGroup.Name = "pnlLeftGroup";
            this.pnlLeftGroup.Size = new System.Drawing.Size(400, 414);
            this.pnlLeftGroup.TabIndex = 0;
            this.pnlLeftGroup.TabStop = false;
            this.pnlLeftGroup.Text = "📊 Solvers Summary";
            // 
            // dgvSolverReport
            // 
            this.dgvSolverReport.AllowUserToAddRows = false;
            this.dgvSolverReport.AllowUserToDeleteRows = false;
            this.dgvSolverReport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSolverReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSolverReport.Location = new System.Drawing.Point(3, 21);
            this.dgvSolverReport.Name = "dgvSolverReport";
            this.dgvSolverReport.ReadOnly = true;
            this.dgvSolverReport.Size = new System.Drawing.Size(394, 390);
            this.dgvSolverReport.TabIndex = 0;
            // 
            // pnlRightGroup
            // 
            this.pnlRightGroup.Controls.Add(this.dgvUnsolvedFiles);
            this.pnlRightGroup.Controls.Add(this.pnlFileActions);
            this.pnlRightGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRightGroup.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.pnlRightGroup.Location = new System.Drawing.Point(406, 3);
            this.pnlRightGroup.Name = "pnlRightGroup";
            this.pnlRightGroup.Size = new System.Drawing.Size(551, 414);
            this.pnlRightGroup.TabIndex = 1;
            this.pnlRightGroup.TabStop = false;
            this.pnlRightGroup.Text = "📁 Uploaded Files & Assignment Management";
            // 
            // dgvUnsolvedFiles
            // 
            this.dgvUnsolvedFiles.AllowUserToAddRows = false;
            this.dgvUnsolvedFiles.AllowUserToDeleteRows = false;
            this.dgvUnsolvedFiles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUnsolvedFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvUnsolvedFiles.Location = new System.Drawing.Point(3, 21);
            this.dgvUnsolvedFiles.Name = "dgvUnsolvedFiles";
            this.dgvUnsolvedFiles.ReadOnly = true;
            this.dgvUnsolvedFiles.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUnsolvedFiles.Size = new System.Drawing.Size(545, 345);
            this.dgvUnsolvedFiles.TabIndex = 0;
            // 
            // pnlFileActions
            // 
            this.pnlFileActions.Controls.Add(this.btnDeleteFile);
            this.pnlFileActions.Controls.Add(this.btnUnassignFile);
            this.pnlFileActions.Controls.Add(this.btnAssignSolver);
            this.pnlFileActions.Controls.Add(this.cmbAssignSolver);
            this.pnlFileActions.Controls.Add(this.lblAssignTo);
            this.pnlFileActions.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFileActions.Location = new System.Drawing.Point(3, 366);
            this.pnlFileActions.Name = "pnlFileActions";
            this.pnlFileActions.Size = new System.Drawing.Size(545, 45);
            this.pnlFileActions.TabIndex = 1;
            // 
            // lblAssignTo
            // 
            this.lblAssignTo.AutoSize = true;
            this.lblAssignTo.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblAssignTo.Location = new System.Drawing.Point(4, 15);
            this.lblAssignTo.Name = "lblAssignTo";
            this.lblAssignTo.Size = new System.Drawing.Size(46, 15);
            this.lblAssignTo.TabIndex = 0;
            this.lblAssignTo.Text = "Assign:";
            // 
            // cmbAssignSolver
            // 
            this.cmbAssignSolver.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAssignSolver.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.cmbAssignSolver.FormattingEnabled = true;
            this.cmbAssignSolver.Location = new System.Drawing.Point(52, 11);
            this.cmbAssignSolver.Name = "cmbAssignSolver";
            this.cmbAssignSolver.Size = new System.Drawing.Size(140, 23);
            this.cmbAssignSolver.TabIndex = 1;
            // 
            // btnAssignSolver
            // 
            this.btnAssignSolver.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.btnAssignSolver.Location = new System.Drawing.Point(196, 9);
            this.btnAssignSolver.Name = "btnAssignSolver";
            this.btnAssignSolver.Size = new System.Drawing.Size(100, 28);
            this.btnAssignSolver.TabIndex = 2;
            this.btnAssignSolver.Text = "👤 Assign";
            this.btnAssignSolver.UseVisualStyleBackColor = true;
            this.btnAssignSolver.Click += new System.EventHandler(this.btnAssignSolver_Click);
            // 
            // btnUnassignFile
            // 
            this.btnUnassignFile.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.btnUnassignFile.Location = new System.Drawing.Point(300, 9);
            this.btnUnassignFile.Name = "btnUnassignFile";
            this.btnUnassignFile.Size = new System.Drawing.Size(100, 28);
            this.btnUnassignFile.TabIndex = 3;
            this.btnUnassignFile.Text = "🔓 Unassign";
            this.btnUnassignFile.UseVisualStyleBackColor = true;
            this.btnUnassignFile.Click += new System.EventHandler(this.btnUnassignFile_Click);
            // 
            // btnDeleteFile
            // 
            this.btnDeleteFile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.btnDeleteFile.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.btnDeleteFile.ForeColor = System.Drawing.Color.White;
            this.btnDeleteFile.Location = new System.Drawing.Point(404, 9);
            this.btnDeleteFile.Name = "btnDeleteFile";
            this.btnDeleteFile.Size = new System.Drawing.Size(95, 28);
            this.btnDeleteFile.TabIndex = 4;
            this.btnDeleteFile.Text = "🗑 Delete";
            this.btnDeleteFile.UseVisualStyleBackColor = true;
            this.btnDeleteFile.Click += new System.EventHandler(this.btnDeleteFile_Click);
            // 
            // pnlBottomBar
            // 
            this.pnlBottomBar.Controls.Add(this.btnOpenUpload);
            this.pnlBottomBar.Controls.Add(this.btnRefresh);
            this.pnlBottomBar.Controls.Add(this.btnConvertHex);
            this.pnlBottomBar.Controls.Add(this.btnDownloadExcel);
            this.pnlBottomBar.Controls.Add(this.btnAddUser);
            this.pnlBottomBar.Controls.Add(this.btnCreateTables);
            this.pnlBottomBar.Controls.Add(this.btnCreateMtemp);
            this.pnlBottomBar.Controls.Add(this.btnViewTranscript);
            this.pnlBottomBar.Controls.Add(this.btnLogout);
            this.pnlBottomBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottomBar.Location = new System.Drawing.Point(12, 522);
            this.pnlBottomBar.Name = "pnlBottomBar";
            this.pnlBottomBar.Size = new System.Drawing.Size(1156, 50);
            this.pnlBottomBar.TabIndex = 2;
            // 
            // btnOpenUpload
            // 
            this.btnOpenUpload.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnOpenUpload.Location = new System.Drawing.Point(3, 6);
            this.btnOpenUpload.Name = "btnOpenUpload";
            this.btnOpenUpload.Size = new System.Drawing.Size(120, 38);
            this.btnOpenUpload.TabIndex = 0;
            this.btnOpenUpload.Text = "📤 Import Files";
            this.btnOpenUpload.UseVisualStyleBackColor = true;
            this.btnOpenUpload.Click += new System.EventHandler(this.btnOpenUpload_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.Location = new System.Drawing.Point(127, 6);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(95, 38);
            this.btnRefresh.TabIndex = 1;
            this.btnRefresh.Text = "🔄 Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnConvertHex
            // 
            this.btnConvertHex.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnConvertHex.Location = new System.Drawing.Point(226, 6);
            this.btnConvertHex.Name = "btnConvertHex";
            this.btnConvertHex.Size = new System.Drawing.Size(115, 38);
            this.btnConvertHex.TabIndex = 2;
            this.btnConvertHex.Text = "⚡ Bin To Hex";
            this.btnConvertHex.UseVisualStyleBackColor = true;
            this.btnConvertHex.Click += new System.EventHandler(this.btnConvertHex_Click);
            // 
            // btnDownloadExcel
            // 
            this.btnDownloadExcel.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnDownloadExcel.Location = new System.Drawing.Point(345, 6);
            this.btnDownloadExcel.Name = "btnDownloadExcel";
            this.btnDownloadExcel.Size = new System.Drawing.Size(135, 38);
            this.btnDownloadExcel.TabIndex = 3;
            this.btnDownloadExcel.Text = "📥 Download Excel";
            this.btnDownloadExcel.UseVisualStyleBackColor = true;
            this.btnDownloadExcel.Click += new System.EventHandler(this.btnDownloadExcel_Click);
            // 
            // btnAddUser
            // 
            this.btnAddUser.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnAddUser.Location = new System.Drawing.Point(484, 6);
            this.btnAddUser.Name = "btnAddUser";
            this.btnAddUser.Size = new System.Drawing.Size(95, 38);
            this.btnAddUser.TabIndex = 4;
            this.btnAddUser.Text = "👤 Add User";
            this.btnAddUser.UseVisualStyleBackColor = true;
            this.btnAddUser.Click += new System.EventHandler(this.btnAddUser_Click);
            // 
            // btnCreateTables
            // 
            this.btnCreateTables.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnCreateTables.Location = new System.Drawing.Point(583, 6);
            this.btnCreateTables.Name = "btnCreateTables";
            this.btnCreateTables.Size = new System.Drawing.Size(125, 38);
            this.btnCreateTables.TabIndex = 5;
            this.btnCreateTables.Text = "🛠 Create Tables";
            this.btnCreateTables.UseVisualStyleBackColor = true;
            this.btnCreateTables.Click += new System.EventHandler(this.btnCreateTables_Click);
            // 
            // btnCreateMtemp
            // 
            this.btnCreateMtemp.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnCreateMtemp.Location = new System.Drawing.Point(712, 6);
            this.btnCreateMtemp.Name = "btnCreateMtemp";
            this.btnCreateMtemp.Size = new System.Drawing.Size(125, 38);
            this.btnCreateMtemp.TabIndex = 6;
            this.btnCreateMtemp.Text = "⚙ Mtemp OPT";
            this.btnCreateMtemp.UseVisualStyleBackColor = true;
            this.btnCreateMtemp.Click += new System.EventHandler(this.btnCreateMtemp_Click);
            // 
            // btnViewTranscript
            // 
            this.btnViewTranscript.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnViewTranscript.Location = new System.Drawing.Point(825, 6);
            this.btnViewTranscript.Name = "btnViewTranscript";
            this.btnViewTranscript.Size = new System.Drawing.Size(195, 38);
            this.btnViewTranscript.TabIndex = 7;
            this.btnViewTranscript.Text = "📜 Transcript & Certificate";
            this.btnViewTranscript.UseVisualStyleBackColor = true;
            this.btnViewTranscript.Click += new System.EventHandler(this.btnViewTranscript_Click);
            // 
            // btnLogout
            // 
            this.btnLogout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnLogout.Location = new System.Drawing.Point(1030, 6);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(120, 38);
            this.btnLogout.TabIndex = 8;
            this.btnLogout.Text = "🚪 Logout";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // UploaderDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1180, 584);
            this.Controls.Add(this.pnlMainLayout);
            this.Controls.Add(this.pnlBottomBar);
            this.Controls.Add(this.pnlMetrics);
            this.Name = "UploaderDashboard";
            this.Padding = new System.Windows.Forms.Padding(12);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Uploader Admin Dashboard";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.UploaderDashboard_FormClosed);
            this.pnlMetrics.ResumeLayout(false);
            this.pnlCard1.ResumeLayout(false);
            this.pnlCard1.PerformLayout();
            this.pnlCard2.ResumeLayout(false);
            this.pnlCard2.PerformLayout();
            this.pnlCard3.ResumeLayout(false);
            this.pnlCard3.PerformLayout();
            this.pnlCard4.ResumeLayout(false);
            this.pnlCard4.PerformLayout();
            this.pnlMainLayout.ResumeLayout(false);
            this.pnlLeftGroup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSolverReport)).EndInit();
            this.pnlRightGroup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUnsolvedFiles)).EndInit();
            this.pnlFileActions.ResumeLayout(false);
            this.pnlFileActions.PerformLayout();
            this.pnlBottomBar.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlMetrics;
        private System.Windows.Forms.Panel pnlCard1;
        private System.Windows.Forms.Label lblTotalFilesVal;
        private System.Windows.Forms.Label lblTotalFilesTitle;
        private System.Windows.Forms.Panel pnlCard2;
        private System.Windows.Forms.Label lblSolvedFilesVal;
        private System.Windows.Forms.Label lblSolvedFilesTitle;
        private System.Windows.Forms.Panel pnlCard3;
        private System.Windows.Forms.Label lblUnsolvedFilesVal;
        private System.Windows.Forms.Label lblUnsolvedFilesTitle;
        private System.Windows.Forms.Panel pnlCard4;
        private System.Windows.Forms.Label lblActiveSolversVal;
        private System.Windows.Forms.Label lblActiveSolversTitle;
        private System.Windows.Forms.TableLayoutPanel pnlMainLayout;
        private System.Windows.Forms.GroupBox pnlLeftGroup;
        private System.Windows.Forms.DataGridView dgvSolverReport;
        private System.Windows.Forms.GroupBox pnlRightGroup;
        private System.Windows.Forms.DataGridView dgvUnsolvedFiles;
        private System.Windows.Forms.Panel pnlBottomBar;
        private System.Windows.Forms.Button btnOpenUpload;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnConvertHex;
        private System.Windows.Forms.Button btnDownloadExcel;
        private System.Windows.Forms.Button btnAddUser;
        private System.Windows.Forms.Button btnCreateTables;
        private System.Windows.Forms.Button btnCreateMtemp;
        private System.Windows.Forms.Button btnViewTranscript;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Panel pnlFileActions;
        private System.Windows.Forms.Label lblAssignTo;
        private System.Windows.Forms.ComboBox cmbAssignSolver;
        private System.Windows.Forms.Button btnAssignSolver;
        private System.Windows.Forms.Button btnUnassignFile;
        private System.Windows.Forms.Button btnDeleteFile;
    }
}
