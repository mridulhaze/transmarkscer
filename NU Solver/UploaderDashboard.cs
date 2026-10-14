using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace NU_Solver
{
    public partial class UploaderDashboard : Form
    {
        public string username = "";
        Form loginForm;

        public UploaderDashboard()
        {
            InitializeComponent();
            UITheme.ApplyTheme(this, "Uploader Admin Dashboard");
            WireEvents();
        }

        public UploaderDashboard(string username, Form loginForm)
        {
            this.username = username;
            this.loginForm = loginForm;
            InitializeComponent();
            UITheme.ApplyTheme(this, "Uploader Admin Dashboard - Operator: " + username);
            WireEvents();
            LoadDashboardData();
        }

        private void WireEvents()
        {
            dgvSolverReport.SelectionChanged += dgvSolverReport_SelectionChanged;
            dgvSolverReport.CellClick += dgvSolverReport_CellClick;
        }

        public void LoadDashboardData()
        {
            try
            {
                SqlConnection con = Database.GetConnectionObj();
                if (con == null) return;

                // Populate Solvers Dropdown for Assignment
                SqlCommand cmdSolversList = new SqlCommand("SELECT solver_name FROM dbo.solvers ORDER BY solver_name", con);
                SqlDataReader rdrSolvers = cmdSolversList.ExecuteReader();
                cmbAssignSolver.Items.Clear();
                while (rdrSolvers.Read())
                {
                    string sName = rdrSolvers["solver_name"].ToString().Trim();
                    if (!string.IsNullOrEmpty(sName) && sName.ToUpper() != "NONE" && sName.ToUpper() != "ALL" && sName.ToUpper() != "UNASSIGNED")
                    {
                        cmbAssignSolver.Items.Add(sName);
                    }
                }
                rdrSolvers.Dispose();

                // 1. Overall Statistics Summary
                int totalFiles = 0;
                int solvedFiles = 0;
                int unsolvedFiles = 0;
                int unassignedFiles = 0;

                SqlCommand cmdSummary = new SqlCommand(@"
                    SELECT 
                        COUNT(file_name) AS TotalFiles,
                        SUM(CASE WHEN solve_status = 'SOLVED' THEN 1 ELSE 0 END) AS SolvedFiles,
                        SUM(CASE WHEN ISNULL(solve_status, '') <> 'SOLVED' THEN 1 ELSE 0 END) AS UnsolvedFiles,
                        SUM(CASE WHEN solver_name IS NULL OR LTRIM(RTRIM(solver_name)) = '' OR solver_name = 'NONE' OR solver_name = '__NONE' OR solver_name = 'Unassigned' THEN 1 ELSE 0 END) AS UnassignedFiles
                    FROM dbo.H_file_list", con);

                SqlDataReader reader = cmdSummary.ExecuteReader();
                if (reader.Read())
                {
                    totalFiles = reader["TotalFiles"] != DBNull.Value ? Convert.ToInt32(reader["TotalFiles"]) : 0;
                    solvedFiles = reader["SolvedFiles"] != DBNull.Value ? Convert.ToInt32(reader["SolvedFiles"]) : 0;
                    unsolvedFiles = reader["UnsolvedFiles"] != DBNull.Value ? Convert.ToInt32(reader["UnsolvedFiles"]) : 0;
                    unassignedFiles = reader["UnassignedFiles"] != DBNull.Value ? Convert.ToInt32(reader["UnassignedFiles"]) : 0;

                    lblTotalFilesVal.Text = totalFiles.ToString();
                    lblSolvedFilesVal.Text = solvedFiles.ToString();
                    lblUnsolvedFilesVal.Text = unsolvedFiles.ToString();
                }
                reader.Dispose();

                // 2. Active Solvers Count
                SqlCommand cmdSolversCount = new SqlCommand("SELECT COUNT(*) FROM dbo.solvers WHERE status = 'solver'", con);
                object solverObj = cmdSolversCount.ExecuteScalar();
                lblActiveSolversVal.Text = solverObj != null ? solverObj.ToString() : "0";

                // 3. Construct Solvers & Files Summary Table
                DataTable dtReport = new DataTable();
                dtReport.Columns.Add("Username", typeof(string));
                dtReport.Columns.Add("Full Name", typeof(string));
                dtReport.Columns.Add("Role", typeof(string));
                dtReport.Columns.Add("Assigned", typeof(int));
                dtReport.Columns.Add("Solved", typeof(int));
                dtReport.Columns.Add("Pending", typeof(int));

                // Add special summary rows at top
                dtReport.Rows.Add("ALL", "ALL UPLOADED FILES", "All Files Overview", totalFiles, solvedFiles, unsolvedFiles);
                dtReport.Rows.Add("UNASSIGNED", "UNASSIGNED / AVAILABLE FILES", "Needs Solver Assignment", unassignedFiles, 0, unassignedFiles);

                // Fetch individual solver stats from DB
                SqlCommand cmdSolverReport = new SqlCommand(@"
                    SELECT 
                        s.solver_name AS [Username],
                        ISNULL(s.full_name, s.solver_name) AS [Full Name],
                        s.status AS [Role],
                        COUNT(f.file_name) AS [Assigned],
                        SUM(CASE WHEN f.solve_status = 'SOLVED' THEN 1 ELSE 0 END) AS [Solved],
                        SUM(CASE WHEN f.solve_status = 'SOLVED' THEN 0 ELSE 1 END) AS [Pending]
                    FROM dbo.solvers s
                    LEFT JOIN dbo.H_file_list f ON s.solver_name = f.solver_name
                    WHERE s.solver_name IS NOT NULL AND LTRIM(RTRIM(s.solver_name)) <> '' AND s.solver_name <> 'NONE'
                    GROUP BY s.solver_name, s.full_name, s.status
                    ORDER BY [Solved] DESC, [Assigned] DESC", con);

                SqlDataReader rdrStats = cmdSolverReport.ExecuteReader();
                while (rdrStats.Read())
                {
                    string uName = rdrStats["Username"].ToString();
                    string fName = rdrStats["Full Name"].ToString();
                    string rRole = rdrStats["Role"].ToString();
                    int assigned = Convert.ToInt32(rdrStats["Assigned"]);
                    int solved = Convert.ToInt32(rdrStats["Solved"]);
                    int pending = Convert.ToInt32(rdrStats["Pending"]);

                    dtReport.Rows.Add(uName, fName, rRole, assigned, solved, pending);
                }
                rdrStats.Dispose();

                dgvSolverReport.DataSource = dtReport;
                UITheme.StyleDataGridView(dgvSolverReport);
                if (dgvSolverReport.Columns["Full Name"] != null)
                {
                    dgvSolverReport.Columns["Full Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }

                con.Close();
                con.Dispose();

                // Auto-select the first row ("ALL") by default to display all uploaded files immediately
                if (dgvSolverReport.Rows.Count > 0)
                {
                    dgvSolverReport.Rows[0].Selected = true;
                    dgvSolverReport_SelectionChanged(null, null);
                }
                else
                {
                    LoadFilesForSelectedSolver("ALL", "ALL UPLOADED FILES");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading dashboard data:\n" + ex.Message);
            }
        }

        private void dgvSolverReport_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvSolverReport.CurrentRow == null) return;
            try
            {
                string selUser = dgvSolverReport.CurrentRow.Cells["Username"].Value != null ? dgvSolverReport.CurrentRow.Cells["Username"].Value.ToString() : "";
                string selFullName = dgvSolverReport.CurrentRow.Cells["Full Name"].Value != null ? dgvSolverReport.CurrentRow.Cells["Full Name"].Value.ToString() : selUser;

                LoadFilesForSelectedSolver(selUser, selFullName);
            }
            catch { }
        }

        private void dgvSolverReport_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                dgvSolverReport_SelectionChanged(sender, e);
            }
        }

        public void LoadFilesForSelectedSolver(string targetUser, string fullName)
        {
            try
            {
                SqlConnection con = Database.GetConnectionObj();
                if (con == null) return;

                pnlRightGroup.Text = string.Format("📁 Uploaded Files: {0} ({1})", fullName, targetUser);

                SqlCommand cmdFiles;
                if (targetUser == "ALL")
                {
                    cmdFiles = new SqlCommand(@"
                        SELECT 
                            f.file_name AS [File Name],
                            ISNULL(f.pap_code, '') AS [Paper Code],
                            ISNULL(f.exm_code, '201') AS [Exam Code],
                            ISNULL(f.exm_year, '2026') AS [Exam Year],
                            ISNULL(f.solver_name, 'UNASSIGNED') AS [Assigned Solver],
                            ISNULL(f.solve_status, 'PENDING') AS [Status],
                            ISNULL(CONVERT(VARCHAR, f.start_date, 120), 'Unassigned') AS [Started At],
                            (SELECT COUNT(*) FROM dbo.H_solving s WHERE s.file_name = f.file_name) AS [Total Rows],
                            (SELECT COUNT(*) FROM dbo.H_solving s WHERE s.file_name = f.file_name AND ISNULL(s.err_dscr, '') <> '') AS [Error Rows]
                        FROM dbo.H_file_list f
                        ORDER BY f.insert_date DESC", con);
                }
                else if (targetUser == "UNASSIGNED" || targetUser == "NONE" || string.IsNullOrEmpty(targetUser))
                {
                    cmdFiles = new SqlCommand(@"
                        SELECT 
                            f.file_name AS [File Name],
                            ISNULL(f.pap_code, '') AS [Paper Code],
                            ISNULL(f.exm_code, '201') AS [Exam Code],
                            ISNULL(f.exm_year, '2026') AS [Exam Year],
                            ISNULL(f.solver_name, 'UNASSIGNED') AS [Assigned Solver],
                            ISNULL(f.solve_status, 'PENDING') AS [Status],
                            ISNULL(CONVERT(VARCHAR, f.start_date, 120), 'Unassigned') AS [Started At],
                            (SELECT COUNT(*) FROM dbo.H_solving s WHERE s.file_name = f.file_name) AS [Total Rows],
                            (SELECT COUNT(*) FROM dbo.H_solving s WHERE s.file_name = f.file_name AND ISNULL(s.err_dscr, '') <> '') AS [Error Rows]
                        FROM dbo.H_file_list f
                        WHERE f.solver_name IS NULL 
                           OR LTRIM(RTRIM(f.solver_name)) = '' 
                           OR f.solver_name = 'NONE' 
                           OR f.solver_name = '__NONE' 
                           OR f.solver_name = 'Unassigned'
                        ORDER BY f.insert_date DESC", con);
                }
                else
                {
                    cmdFiles = new SqlCommand(@"
                        SELECT 
                            f.file_name AS [File Name],
                            ISNULL(f.pap_code, '') AS [Paper Code],
                            ISNULL(f.exm_code, '201') AS [Exam Code],
                            ISNULL(f.exm_year, '2026') AS [Exam Year],
                            ISNULL(f.solver_name, 'UNASSIGNED') AS [Assigned Solver],
                            ISNULL(f.solve_status, 'PENDING') AS [Status],
                            ISNULL(CONVERT(VARCHAR, f.start_date, 120), 'Not Started') AS [Started At],
                            (SELECT COUNT(*) FROM dbo.H_solving s WHERE s.file_name = f.file_name) AS [Total Rows],
                            (SELECT COUNT(*) FROM dbo.H_solving s WHERE s.file_name = f.file_name AND ISNULL(s.err_dscr, '') <> '') AS [Error Rows]
                        FROM dbo.H_file_list f
                        WHERE f.solver_name = @targetUser
                        ORDER BY f.insert_date DESC", con);
                    cmdFiles.Parameters.AddWithValue("@targetUser", targetUser);
                }

                SqlDataAdapter daFiles = new SqlDataAdapter(cmdFiles);
                DataTable dtFiles = new DataTable();
                daFiles.Fill(dtFiles);
                dgvUnsolvedFiles.DataSource = dtFiles;
                UITheme.StyleDataGridView(dgvUnsolvedFiles);

                if (dgvUnsolvedFiles.Columns["File Name"] != null)
                {
                    dgvUnsolvedFiles.Columns["File Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }

                con.Close();
                con.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading solver files:\n" + ex.Message);
            }
        }

        private void btnAssignSolver_Click(object sender, EventArgs e)
        {
            if (dgvUnsolvedFiles.CurrentRow == null)
            {
                MessageBox.Show("Please select a file from the list to assign.", "No File Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedFile = dgvUnsolvedFiles.CurrentRow.Cells["File Name"].Value != null ? dgvUnsolvedFiles.CurrentRow.Cells["File Name"].Value.ToString() : "";
            string selectedSolver = cmbAssignSolver.SelectedItem != null ? cmbAssignSolver.SelectedItem.ToString() : "";

            if (string.IsNullOrEmpty(selectedFile)) return;
            if (string.IsNullOrEmpty(selectedSolver))
            {
                MessageBox.Show("Please select a target solver from the dropdown list to assign.", "No Solver Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                SqlConnection con = Database.GetConnectionObj();
                if (con == null) return;

                SqlCommand cmd = new SqlCommand(
                    "UPDATE dbo.H_file_list SET solver_name = @solver, solve_status = 'solving', start_date = CURRENT_TIMESTAMP WHERE file_name = @file", con);
                cmd.Parameters.AddWithValue("@solver", selectedSolver);
                cmd.Parameters.AddWithValue("@file", selectedFile);
                cmd.ExecuteNonQuery();

                SqlCommand cmdSolving = new SqlCommand(
                    "UPDATE dbo.H_solving SET solver = @solver WHERE file_name = @file", con);
                cmdSolving.Parameters.AddWithValue("@solver", selectedSolver);
                cmdSolving.Parameters.AddWithValue("@file", selectedFile);
                cmdSolving.ExecuteNonQuery();

                con.Close();
                con.Dispose();

                MessageBox.Show(string.Format("File '{0}' assigned to solver '{1}' successfully!", selectedFile, selectedSolver), "Assignment Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDashboardData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error assigning file:\n" + ex.Message);
            }
        }

        private void btnUnassignFile_Click(object sender, EventArgs e)
        {
            if (dgvUnsolvedFiles.CurrentRow == null)
            {
                MessageBox.Show("Please select a file to unassign.", "No File Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedFile = dgvUnsolvedFiles.CurrentRow.Cells["File Name"].Value != null ? dgvUnsolvedFiles.CurrentRow.Cells["File Name"].Value.ToString() : "";
            if (string.IsNullOrEmpty(selectedFile)) return;

            try
            {
                SqlConnection con = Database.GetConnectionObj();
                if (con == null) return;

                SqlCommand cmd = new SqlCommand(
                    "UPDATE dbo.H_file_list SET solver_name = NULL, solve_status = 'PENDING', start_date = NULL WHERE file_name = @file", con);
                cmd.Parameters.AddWithValue("@file", selectedFile);
                cmd.ExecuteNonQuery();

                SqlCommand cmdSolving = new SqlCommand(
                    "UPDATE dbo.H_solving SET solver = NULL WHERE file_name = @file", con);
                cmdSolving.Parameters.AddWithValue("@file", selectedFile);
                cmdSolving.ExecuteNonQuery();

                con.Close();
                con.Dispose();

                MessageBox.Show(string.Format("File '{0}' unassigned successfully!", selectedFile), "Unassigned", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDashboardData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error unassigning file:\n" + ex.Message);
            }
        }

        private void btnDeleteFile_Click(object sender, EventArgs e)
        {
            if (dgvUnsolvedFiles.CurrentRow == null)
            {
                MessageBox.Show("Please select a file to delete.", "No File Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedFile = dgvUnsolvedFiles.CurrentRow.Cells["File Name"].Value != null ? dgvUnsolvedFiles.CurrentRow.Cells["File Name"].Value.ToString() : "";
            if (string.IsNullOrEmpty(selectedFile)) return;

            if (MessageBox.Show(string.Format("Are you sure you want to permanently delete file '{0}' and all its rows from database?", selectedFile),
                "Confirm Delete File", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    SqlConnection con = Database.GetConnectionObj();
                    if (con == null) return;

                    SqlCommand cmdSolving = new SqlCommand("DELETE FROM dbo.H_solving WHERE file_name = @file", con);
                    cmdSolving.Parameters.AddWithValue("@file", selectedFile);
                    cmdSolving.ExecuteNonQuery();

                    SqlCommand cmdFileList = new SqlCommand("DELETE FROM dbo.H_file_list WHERE file_name = @file", con);
                    cmdFileList.Parameters.AddWithValue("@file", selectedFile);
                    cmdFileList.ExecuteNonQuery();

                    con.Close();
                    con.Dispose();

                    MessageBox.Show(string.Format("File '{0}' deleted successfully from database!", selectedFile), "File Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDashboardData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting file:\n" + ex.Message);
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadDashboardData();
        }

        private void btnOpenUpload_Click(object sender, EventArgs e)
        {
            upload uploadFrm = new upload(this);
            uploadFrm.FormClosed += (s, args) => { LoadDashboardData(); };
            uploadFrm.Show();
        }

        private void btnConvertHex_Click(object sender, EventArgs e)
        {
            try
            {
                int rows = Database.RunBinToHexConversion();
                MessageBox.Show("Conversion complete! Updated hexcode for " + rows + " rows.", "Hex Conversion", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hex Conversion failed:\n" + ex.Message, "Conversion Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDownloadExcel_Click(object sender, EventArgs e)
        {
            try
            {
                string selectedFile = null;
                if (dgvUnsolvedFiles.CurrentRow != null && dgvUnsolvedFiles.CurrentRow.Cells["File Name"] != null)
                {
                    object cellVal = dgvUnsolvedFiles.CurrentRow.Cells["File Name"].Value;
                    if (cellVal != null && !string.IsNullOrEmpty(cellVal.ToString()))
                    {
                        selectedFile = cellVal.ToString().Trim();
                    }
                }

                string filterFileName = null;
                string defaultFileName = "";

                using (DownloadChoiceForm choiceDlg = new DownloadChoiceForm(selectedFile))
                {
                    if (choiceDlg.ShowDialog(this) != DialogResult.OK)
                    {
                        return;
                    }

                    if (choiceDlg.IsSingleFile)
                    {
                        filterFileName = choiceDlg.SelectedFileName;
                        if (string.IsNullOrEmpty(filterFileName))
                        {
                            MessageBox.Show("Please select a file to download.", "No File Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        defaultFileName = System.IO.Path.GetFileNameWithoutExtension(filterFileName) + "_H_solving";
                    }
                    else
                    {
                        filterFileName = null;
                        defaultFileName = "Total_H_solving_" + DateTime.Now.ToString("yyyyMMdd_HHmm");
                    }
                }

                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Title = "Save H_solving Data";
                    sfd.FileName = defaultFileName + ".xls";
                    sfd.Filter = "Excel 97-2003 Spreadsheet (*.xls)|*.xls|CSV (Comma delimited) (*.csv)|*.csv";
                    sfd.DefaultExt = "xls";

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        int count = Database.ExportHSolvingToExcel(sfd.FileName, filterFileName);
                        Cursor.Current = Cursors.Default;

                        string scopeText = !string.IsNullOrEmpty(filterFileName) ? "file '" + filterFileName + "'" : "Total H_solving (" + count + " rows)";
                        DialogResult openPrompt = MessageBox.Show(
                            string.Format("Successfully exported {0} rows for {1} to:\n{2}\n\nWould you like to open the file now?", count, scopeText, sfd.FileName),
                            "Export Complete",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Information);

                        if (openPrompt == DialogResult.Yes)
                        {
                            try
                            {
                                System.Diagnostics.Process.Start(sfd.FileName);
                            }
                            catch (Exception exOpen)
                            {
                                MessageBox.Show("Could not open file automatically:\n" + exOpen.Message, "Notice", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Export failed:\n" + ex.Message, "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            using (AddUserForm dlg = new AddUserForm())
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    LoadDashboardData();
                }
            }
        }

        private void btnCreateTables_Click(object sender, EventArgs e)
        {
            try
            {
                using (AdminAuthDialog auth = new AdminAuthDialog("Enter password for user 'admin' to create database tables:"))
                {
                    if (auth.ShowDialog(this) != DialogResult.OK)
                    {
                        return;
                    }
                }

                DialogResult dr = MessageBox.Show(
                    "This action will verify and create all required database tables for NU Solver (if they do not already exist).\n\nExisting tables and data will NOT be modified or lost.\n\nProceed to create missing tables?",
                    "Create Database Tables",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (dr != DialogResult.Yes) return;

                Cursor.Current = Cursors.WaitCursor;
                Database.TableCreationResult res = Database.CreateDatabaseTablesIfNotExists();
                Cursor.Current = Cursors.Default;

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.AppendLine("Database Table Verification & Creation Report:");
                sb.AppendLine("--------------------------------------------------");

                if (res.CreatedTables.Count > 0)
                {
                    sb.AppendLine(string.Format("✅ Newly Created Tables ({0}):", res.CreatedTables.Count));
                    foreach (var t in res.CreatedTables)
                    {
                        sb.AppendLine("   • " + t);
                    }
                    sb.AppendLine();
                }

                if (res.ExistingTables.Count > 0)
                {
                    sb.AppendLine(string.Format("ℹ Already Existing Tables ({0}):", res.ExistingTables.Count));
                    foreach (var t in res.ExistingTables)
                    {
                        sb.AppendLine("   • " + t);
                    }
                    sb.AppendLine();
                }

                if (res.Errors.Count > 0)
                {
                    sb.AppendLine(string.Format("⚠️ Warnings / Errors ({0}):", res.Errors.Count));
                    foreach (var err in res.Errors)
                    {
                        sb.AppendLine("   • " + err);
                    }
                }
                else
                {
                    sb.AppendLine("All required tables, keys, and indexes are ready!");
                }

                MessageBox.Show(sb.ToString(), "Create Tables Result", MessageBoxButtons.OK, res.Success ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

                LoadDashboardData();
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Error creating tables:\n" + ex.Message, "Table Creation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCreateMtemp_Click(object sender, EventArgs e)
        {
            try
            {
                using (AdminAuthDialog auth = new AdminAuthDialog("Enter password for user 'admin' to access MTEMP Operations:"))
                {
                    if (auth.ShowDialog(this) != DialogResult.OK)
                    {
                        return;
                    }
                }

                using (MtempOptForm form = new MtempOptForm())
                {
                    form.ShowDialog(this);
                }
                LoadDashboardData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening MTEMP Operations window:\n" + ex.Message, "MTEMP OPT Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnViewTranscript_Click(object sender, EventArgs e)
        {
            try
            {
                UserPermissionManager.EnsureTranscriptUserTableAndSeed(Database.GetConnectionString());

                if (!UserSession.IsAuthenticated)
                {
                    using (OperatorLoginForm login = new OperatorLoginForm(Database.GetConnectionString()))
                    {
                        if (login.ShowDialog(this) != DialogResult.OK)
                        {
                            return;
                        }
                    }
                }

                if (!UserSession.CanView)
                {
                    MessageBox.Show("Your account does not have permission to view Transcripts and Certificates.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (UserCredentialViewDialog credDialog = new UserCredentialViewDialog(UserSession.CurrentUser, Database.GetConnectionString()))
                {
                    if (credDialog.ShowDialog(this) != DialogResult.OK)
                    {
                        return;
                    }
                }

                using (TranscriptForm form = new TranscriptForm())
                {
                    form.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening Transcript Viewer:\n" + ex.Message, "Transcript Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void OpenAdminDashboard()
        {
            try
            {
                using (AdminDashboardForm form = new AdminDashboardForm())
                {
                    form.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening Admin Dashboard:\n" + ex.Message, "Admin Panel Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool isLoggingOut = false;

        private void btnLogout_Click(object sender, EventArgs e)
        {
            try
            {
                isLoggingOut = true;
                if (loginForm != null && !loginForm.IsDisposed)
                {
                    loginForm.Show();
                }
                else
                {
                    login loginFrm = new login();
                    loginFrm.Show();
                }
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during logout: " + ex.Message);
            }
        }

        private void UploaderDashboard_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!isLoggingOut)
            {
                Application.Exit();
            }
        }
    }

    public class DownloadChoiceForm : Form
    {
        private Panel pnlHeader;
        private Label lblTitle;
        private Label lblSubtitle;

        private Panel pnlCard;
        private RadioButton rbSingleFile;
        private Label lblSelectFile;
        private ComboBox cmbFiles;
        private RadioButton rbTotalFile;

        private Button btnDownload;
        private Button btnCancel;

        public bool IsSingleFile
        {
            get { return rbSingleFile.Checked; }
        }

        public string SelectedFileName
        {
            get { return cmbFiles.SelectedItem != null ? cmbFiles.SelectedItem.ToString().Trim() : ""; }
        }

        public DownloadChoiceForm(string preSelectedFile = null)
        {
            InitializeComponent();
            UITheme.ApplyTheme(this, "Download H_solving");
            LoadFileList(preSelectedFile);
        }

        private void InitializeComponent()
        {
            this.pnlHeader = new Panel();
            this.lblTitle = new Label();
            this.lblSubtitle = new Label();
            this.pnlCard = new Panel();
            this.rbSingleFile = new RadioButton();
            this.lblSelectFile = new Label();
            this.cmbFiles = new ComboBox();
            this.rbTotalFile = new RadioButton();
            this.btnDownload = new Button();
            this.btnCancel = new Button();

            this.pnlHeader.SuspendLayout();
            this.pnlCard.SuspendLayout();
            this.SuspendLayout();

            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = Color.FromArgb(5, 150, 105);
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Controls.Add(this.lblSubtitle);
            this.pnlHeader.Dock = DockStyle.Top;
            this.pnlHeader.Location = new Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new Size(460, 60);
            this.pnlHeader.TabIndex = 0;

            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.lblTitle.ForeColor = Color.White;
            this.lblTitle.Location = new Point(16, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new Size(200, 21);
            this.lblTitle.Text = "📥 Download H_solving";

            // 
            // lblSubtitle
            // 
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.Font = new Font("Segoe UI", 9F);
            this.lblSubtitle.ForeColor = Color.FromArgb(209, 250, 229);
            this.lblSubtitle.Location = new Point(18, 33);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new Size(340, 15);
            this.lblSubtitle.Text = "Choose Single File or Total H_solving dataset to download";

            // 
            // pnlCard
            // 
            this.pnlCard.BackColor = Color.White;
            this.pnlCard.BorderStyle = BorderStyle.FixedSingle;
            this.pnlCard.Controls.Add(this.rbSingleFile);
            this.pnlCard.Controls.Add(this.lblSelectFile);
            this.pnlCard.Controls.Add(this.cmbFiles);
            this.pnlCard.Controls.Add(this.rbTotalFile);
            this.pnlCard.Location = new Point(16, 75);
            this.pnlCard.Name = "pnlCard";
            this.pnlCard.Size = new Size(428, 175);
            this.pnlCard.TabIndex = 1;

            // 
            // rbSingleFile
            // 
            this.rbSingleFile.AutoSize = true;
            this.rbSingleFile.Checked = true;
            this.rbSingleFile.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            this.rbSingleFile.ForeColor = Color.FromArgb(30, 41, 59);
            this.rbSingleFile.Location = new Point(20, 18);
            this.rbSingleFile.Name = "rbSingleFile";
            this.rbSingleFile.Size = new Size(160, 23);
            this.rbSingleFile.TabIndex = 0;
            this.rbSingleFile.TabStop = true;
            this.rbSingleFile.Text = "📄 Single File Only";
            this.rbSingleFile.UseVisualStyleBackColor = true;
            this.rbSingleFile.CheckedChanged += new EventHandler(this.rbOption_CheckedChanged);

            // 
            // lblSelectFile
            // 
            this.lblSelectFile.AutoSize = true;
            this.lblSelectFile.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblSelectFile.ForeColor = Color.FromArgb(71, 85, 105);
            this.lblSelectFile.Location = new Point(42, 48);
            this.lblSelectFile.Name = "lblSelectFile";
            this.lblSelectFile.Size = new Size(68, 15);
            this.lblSelectFile.Text = "Choose File:";

            // 
            // cmbFiles
            // 
            this.cmbFiles.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbFiles.Font = new Font("Segoe UI", 9.5F);
            this.cmbFiles.FormattingEnabled = true;
            this.cmbFiles.Location = new Point(45, 68);
            this.cmbFiles.Name = "cmbFiles";
            this.cmbFiles.Size = new Size(350, 24);
            this.cmbFiles.TabIndex = 1;

            // 
            // rbTotalFile
            // 
            this.rbTotalFile.AutoSize = true;
            this.rbTotalFile.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            this.rbTotalFile.ForeColor = Color.FromArgb(30, 41, 59);
            this.rbTotalFile.Location = new Point(20, 115);
            this.rbTotalFile.Name = "rbTotalFile";
            this.rbTotalFile.Size = new Size(310, 23);
            this.rbTotalFile.TabIndex = 2;
            this.rbTotalFile.Text = "📚 Total H_solving File (All Uploaded Files)";
            this.rbTotalFile.UseVisualStyleBackColor = true;
            this.rbTotalFile.CheckedChanged += new EventHandler(this.rbOption_CheckedChanged);

            // 
            // btnDownload
            // 
            this.btnDownload.BackColor = Color.FromArgb(5, 150, 105);
            this.btnDownload.FlatStyle = FlatStyle.Flat;
            this.btnDownload.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            this.btnDownload.ForeColor = Color.White;
            this.btnDownload.Location = new Point(184, 265);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new Size(150, 36);
            this.btnDownload.TabIndex = 2;
            this.btnDownload.Text = "📥 Download Excel";
            this.btnDownload.UseVisualStyleBackColor = false;
            this.btnDownload.Click += new EventHandler(this.btnDownload_Click);

            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            this.btnCancel.Location = new Point(344, 265);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(100, 36);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);

            // 
            // DownloadChoiceForm
            // 
            this.AcceptButton = this.btnDownload;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new Size(460, 318);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnDownload);
            this.Controls.Add(this.pnlCard);
            this.Controls.Add(this.pnlHeader);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DownloadChoiceForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Download H_solving";
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlCard.ResumeLayout(false);
            this.pnlCard.PerformLayout();
            this.ResumeLayout(false);
        }

        private void rbOption_CheckedChanged(object sender, EventArgs e)
        {
            cmbFiles.Enabled = rbSingleFile.Checked;
            lblSelectFile.Enabled = rbSingleFile.Checked;
        }

        private void LoadFileList(string preSelectedFile)
        {
            try
            {
                cmbFiles.Items.Clear();
                using (SqlConnection con = Database.GetConnectionObj())
                {
                    if (con != null)
                    {
                        string query = "SELECT file_name FROM dbo.H_file_list ORDER BY file_name";
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string fn = reader["file_name"].ToString().Trim();
                                if (!string.IsNullOrEmpty(fn) && !cmbFiles.Items.Contains(fn))
                                {
                                    cmbFiles.Items.Add(fn);
                                }
                            }
                        }

                        if (cmbFiles.Items.Count == 0)
                        {
                            string querySolving = "SELECT DISTINCT file_name FROM dbo.H_solving WHERE file_name IS NOT NULL ORDER BY file_name";
                            using (SqlCommand cmd = new SqlCommand(querySolving, con))
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    string fn = reader["file_name"].ToString().Trim();
                                    if (!string.IsNullOrEmpty(fn) && !cmbFiles.Items.Contains(fn))
                                    {
                                        cmbFiles.Items.Add(fn);
                                    }
                                }
                            }
                        }
                    }
                }

                if (!string.IsNullOrEmpty(preSelectedFile) && cmbFiles.Items.Contains(preSelectedFile))
                {
                    cmbFiles.SelectedItem = preSelectedFile;
                    rbSingleFile.Checked = true;
                }
                else if (cmbFiles.Items.Count > 0)
                {
                    cmbFiles.SelectedIndex = 0;
                    rbSingleFile.Checked = true;
                }
                else
                {
                    rbTotalFile.Checked = true;
                    rbSingleFile.Enabled = false;
                    cmbFiles.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading file list: " + ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (rbSingleFile.Checked && string.IsNullOrEmpty(SelectedFileName))
            {
                MessageBox.Show("Please select a file to download.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}

