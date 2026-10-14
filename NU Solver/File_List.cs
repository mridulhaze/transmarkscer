using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace NU_Solver
{
    public partial class H_file_list : Form
    {
        string username = "";
        Form caller;

        public H_file_list()
        {
            InitializeComponent();
            UITheme.ApplyTheme(this, "Available Files - OMR Workspace");
            LoadSolverHeroImage();
            loadFreeFiles();
            button1.Visible = isUserUploader();
        }

        public H_file_list(string username, Form caller)
        {
            this.username = username;
            this.caller = caller;
            InitializeComponent();
            UITheme.ApplyTheme(this, "Available OMR Files - Solver: " + username);
            LoadSolverHeroImage();
            loadFreeFiles();
            button1.Visible = isUserUploader();
            if (lblSolverNameVal != null && !string.IsNullOrEmpty(username))
            {
                lblSolverNameVal.Text = username;
            }
        }

        private void LoadSolverHeroImage()
        {
            try
            {
                Image img = UITheme.GetEmbeddedOrFileImage("2.jpg");
                if (img != null && pbSolverBanner != null)
                {
                    pbSolverBanner.Image = img;
                    pbSolverBanner.SizeMode = PictureBoxSizeMode.Zoom;
                }
            }
            catch { }
        }

        public bool isUserUploader()
        {
            try
            {
                if (string.IsNullOrEmpty(username)) return false;
                SqlConnection con = Database.GetConnectionObj();
                if (con == null) return false;
                SqlCommand cmd = new SqlCommand("SELECT status FROM dbo.solvers WHERE solver_name = @uname", con);
                cmd.Parameters.AddWithValue("@uname", username);
                object res = cmd.ExecuteScalar();
                con.Close();
                con.Dispose();
                if (res != null && res.ToString().Trim().ToLower() == "uploader")
                {
                    return true;
                }
            }
            catch { }
            return false;
        }

        private bool isLoggingOut = false;

        private void H_file_list_closed(object sender, FormClosedEventArgs e)
        {
            if (!isLoggingOut)
            {
                Application.Exit();
            }
        }

        private void H_file_list_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isLoggingOut && e.CloseReason == CloseReason.UserClosing)
            {
                if (MessageBox.Show("Are you sure you want to Exit?", "Exit Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                    e.Cancel = true;            
            }
        }

        public void loadFreeFiles()
        {
            try
            {
                if (dgvFiles == null) return;

                SqlConnection con = Database.GetConnectionObj();
                if (con == null) throw new Exception("Can't create and open a connection");

                string query = string.Format(@"
                    SELECT 
                        f.file_name AS [File Name],
                        ISNULL(f.pap_code, '') AS [Paper Code],
                        ISNULL(f.exm_code, '201') AS [Exam Code],
                        ISNULL(f.exm_year, '2026') AS [Exam Year],
                        (SELECT COUNT(*) FROM dbo.H_solving s WHERE s.file_name = f.file_name) AS [Total Rows],
                        (SELECT COUNT(*) FROM dbo.H_solving s WHERE s.file_name = f.file_name AND ISNULL(s.err_dscr, '') <> '') AS [Remaining Errors],
                        CASE 
                            WHEN f.solve_status = 'SOLVED' THEN 'SOLVED (' + ISNULL(f.solver_name, '') + ')'
                            WHEN f.solve_status = 'solving' AND f.solver_name <> '{0}' THEN 'SOLVING (' + ISNULL(f.solver_name, '') + ')'
                            WHEN f.solve_status = 'solving' AND f.solver_name = '{0}' THEN 'MY WORK IN PROGRESS'
                            ELSE 'UNASSIGNED (AVAILABLE)'
                        END AS [Status / Solver]
                    FROM dbo.H_file_list f
                    ORDER BY f.insert_date DESC", username);

                SqlDataAdapter da = new SqlDataAdapter(new SqlCommand(query, con));
                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvFiles.DataSource = dt;
                UITheme.StyleDataGridView(dgvFiles);
                ConfigureGridColumns();

                // Calculate summary card stats
                int totalAvailable = 0;
                int myWorkInProgress = 0;
                int totalSolved = 0;
                int totalPendingErrors = 0;

                foreach (DataRow row in dt.Rows)
                {
                    string status = row["Status / Solver"] != null ? row["Status / Solver"].ToString() : "";
                    int errs = row["Remaining Errors"] != DBNull.Value ? Convert.ToInt32(row["Remaining Errors"]) : 0;

                    if (status.StartsWith("UNASSIGNED")) totalAvailable++;
                    else if (status.Contains("MY WORK IN PROGRESS")) myWorkInProgress++;
                    else if (status.StartsWith("SOLVED")) totalSolved++;

                    if (!status.StartsWith("SOLVED")) totalPendingErrors += errs;
                }

                if (lblMetricAvailableVal != null) lblMetricAvailableVal.Text = totalAvailable.ToString();
                if (lblMetricMyWorkVal != null) lblMetricMyWorkVal.Text = myWorkInProgress.ToString();
                if (lblMetricSolvedVal != null) lblMetricSolvedVal.Text = totalSolved.ToString();
                if (lblMetricErrorsVal != null) lblMetricErrorsVal.Text = totalPendingErrors.ToString();

                // Sync legacy listbox for backwards compatibility
                if (H_file_list_box != null)
                {
                    H_file_list_box.Items.Clear();
                    foreach (DataRow r in dt.Rows)
                    {
                        H_file_list_box.Items.Add(r["File Name"].ToString());
                    }
                }

                con.Close();
                con.Dispose();
            }
            catch (Exception ee)
            {
                MessageBox.Show("Database error loading files.\n" + ee.Message);
            }
        }

        private void ConfigureGridColumns()
        {
            if (dgvFiles == null || dgvFiles.Columns.Count == 0) return;

            dgvFiles.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            if (dgvFiles.Columns["File Name"] != null)
            {
                dgvFiles.Columns["File Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvFiles.Columns["File Name"].FillWeight = 26;
                dgvFiles.Columns["File Name"].MinimumWidth = 160;
                dgvFiles.Columns["File Name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            }

            if (dgvFiles.Columns["Paper Code"] != null)
            {
                dgvFiles.Columns["Paper Code"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvFiles.Columns["Paper Code"].FillWeight = 11;
                dgvFiles.Columns["Paper Code"].MinimumWidth = 90;
                dgvFiles.Columns["Paper Code"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            if (dgvFiles.Columns["Exam Code"] != null)
            {
                dgvFiles.Columns["Exam Code"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvFiles.Columns["Exam Code"].FillWeight = 9;
                dgvFiles.Columns["Exam Code"].MinimumWidth = 80;
                dgvFiles.Columns["Exam Code"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            if (dgvFiles.Columns["Exam Year"] != null)
            {
                dgvFiles.Columns["Exam Year"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvFiles.Columns["Exam Year"].FillWeight = 9;
                dgvFiles.Columns["Exam Year"].MinimumWidth = 80;
                dgvFiles.Columns["Exam Year"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            if (dgvFiles.Columns["Total Rows"] != null)
            {
                dgvFiles.Columns["Total Rows"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvFiles.Columns["Total Rows"].FillWeight = 11;
                dgvFiles.Columns["Total Rows"].MinimumWidth = 90;
                dgvFiles.Columns["Total Rows"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            if (dgvFiles.Columns["Remaining Errors"] != null)
            {
                dgvFiles.Columns["Remaining Errors"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvFiles.Columns["Remaining Errors"].FillWeight = 14;
                dgvFiles.Columns["Remaining Errors"].MinimumWidth = 110;
                dgvFiles.Columns["Remaining Errors"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvFiles.Columns["Remaining Errors"].DefaultCellStyle.Font = new Font("Consolas", 10.5f, FontStyle.Bold);
                dgvFiles.Columns["Remaining Errors"].DefaultCellStyle.ForeColor = UITheme.ErrorRed;
            }

            if (dgvFiles.Columns["Status / Solver"] != null)
            {
                dgvFiles.Columns["Status / Solver"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvFiles.Columns["Status / Solver"].FillWeight = 20;
                dgvFiles.Columns["Status / Solver"].MinimumWidth = 180;
                dgvFiles.Columns["Status / Solver"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            }
        }

        public bool getAccess(string filename)
        {
            try
            {
                SqlConnection con = Database.GetConnectionObj();
                if (con == null) throw new Exception("Can't create and open a connection");                
                SqlCommand cmd = new SqlCommand("", con);

                cmd.CommandText = 
                    "UPDATE dbo.H_file_list " +
                    "SET solver_name = @uname, solve_status = 'solving', start_date = CURRENT_TIMESTAMP " +
                    "WHERE file_name = @fname AND (ISNULL(solver_name, '__NONE') IN ('__NONE', '', 'NONE', @uname))";
                cmd.Parameters.AddWithValue("@uname", username);
                cmd.Parameters.AddWithValue("@fname", filename);

                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected == 0)
                {
                    cmd.CommandText = "SELECT COUNT(*) FROM dbo.H_file_list WHERE file_name = @fname AND (solver_name = @uname OR @uname = 'a')";
                    int cnt = Convert.ToInt32(cmd.ExecuteScalar());
                    con.Close();
                    con.Dispose();
                    return cnt > 0;
                }

                con.Close();
                con.Dispose();
                return true;
            }
            catch (Exception ee)
            {
                MessageBox.Show("Database error.\n" + ee.StackTrace.ToString());
            }
            return false;
        }

        private void file_double_clicked(object sender, EventArgs e)
        {
            int index = this.H_file_list_box.SelectedIndex;
            if (index < 0 || index >= this.H_file_list_box.Items.Count) return;

            string filename = this.H_file_list_box.Items[index].ToString();
            OpenSelectedFile(filename);
        }

        private void dgvFiles_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dgvFiles.CurrentRow == null) return;

            string filename = dgvFiles.CurrentRow.Cells["File Name"].Value != null ? dgvFiles.CurrentRow.Cells["File Name"].Value.ToString() : "";
            string status = dgvFiles.CurrentRow.Cells["Status / Solver"].Value != null ? dgvFiles.CurrentRow.Cells["Status / Solver"].Value.ToString() : "";

            if (string.IsNullOrEmpty(filename)) return;

            if (status.StartsWith("SOLVING (") && !status.Contains(username))
            {
                if (MessageBox.Show(string.Format("File '{0}' is currently being solved by another user:\n{1}\n\nDo you still want to open this file?", filename, status),
                    "File Currently Being Solved", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                {
                    return;
                }
            }

            OpenSelectedFile(filename);
        }

        private void btnOpenSolve_Click(object sender, EventArgs e)
        {
            if (dgvFiles.CurrentRow == null)
            {
                MessageBox.Show("Please select a file from the list first.", "No File Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string filename = dgvFiles.CurrentRow.Cells["File Name"].Value != null ? dgvFiles.CurrentRow.Cells["File Name"].Value.ToString() : "";
            string status = dgvFiles.CurrentRow.Cells["Status / Solver"].Value != null ? dgvFiles.CurrentRow.Cells["Status / Solver"].Value.ToString() : "";

            if (string.IsNullOrEmpty(filename)) return;

            if (status.StartsWith("SOLVING (") && !status.Contains(username))
            {
                if (MessageBox.Show(string.Format("File '{0}' is currently being solved by another user:\n{1}\n\nDo you still want to open this file?", filename, status),
                    "File Currently Being Solved", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                {
                    return;
                }
            }

            OpenSelectedFile(filename);
        }

        private void OpenSelectedFile(string filename)
        {
            if (getAccess(filename))
            {
                H_solve slw = new H_solve(filename, username, getSub(filename), this, false);
                this.Hide();
                slw.Show();
            }
            else
            {
                MessageBox.Show("Unable to gain access to file: " + filename, "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public string getSub(string f_name)
        {
            try
            {
                return f_name.Substring(0, 6);
            }
            catch
            {
                return "";
            }
        }

        private void H_file_list_box_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            loadFreeFiles();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!isUserUploader())
            {
                MessageBox.Show("Access Denied: Only users with 'uploader' status can perform Hex conversion.", "Permission Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

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

        private void btnLogout_Click(object sender, EventArgs e)
        {
            try
            {
                isLoggingOut = true;
                if (caller != null && !caller.IsDisposed)
                {
                    caller.Show();
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
    }
}
