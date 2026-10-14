using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace NU_Solver
{
    public class AddUserForm : Form
    {
        private Panel pnlHeader;
        private Label lblTitle;
        private Label lblSubtitle;
        
        private Panel pnlCard;
        private Label lblUsername;
        private TextBox tbUsername;
        
        private Label lblPasskey;
        private TextBox tbPasskey;
        
        private Label lblStatus;
        private ComboBox cmbStatus;
        
        private Label lblFullName;
        private TextBox tbFullName;
        
        private Button btnSave;
        private Button btnCancel;

        public AddUserForm()
        {
            InitializeComponent();
            UITheme.ApplyTheme(this, "Add New User Account");
        }

        private void InitializeComponent()
        {
            this.pnlHeader = new Panel();
            this.lblTitle = new Label();
            this.lblSubtitle = new Label();
            this.pnlCard = new Panel();
            this.lblUsername = new Label();
            this.tbUsername = new TextBox();
            this.lblPasskey = new Label();
            this.tbPasskey = new TextBox();
            this.lblStatus = new Label();
            this.cmbStatus = new ComboBox();
            this.lblFullName = new Label();
            this.tbFullName = new TextBox();
            this.btnSave = new Button();
            this.btnCancel = new Button();

            this.pnlHeader.SuspendLayout();
            this.pnlCard.SuspendLayout();
            this.SuspendLayout();

            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = UITheme.GreenHeader;
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Controls.Add(this.lblSubtitle);
            this.pnlHeader.Dock = DockStyle.Top;
            this.pnlHeader.Height = 60;

            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.lblTitle.ForeColor = Color.White;
            this.lblTitle.Location = new Point(15, 10);
            this.lblTitle.Text = "👤 Add New User Account";

            // 
            // lblSubtitle
            // 
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.Font = new Font("Segoe UI", 8.5F);
            this.lblSubtitle.ForeColor = Color.FromArgb(220, 252, 231);
            this.lblSubtitle.Location = new Point(17, 34);
            this.lblSubtitle.Text = "Create a solver, uploader, or admin login for system access";

            // 
            // pnlCard
            // 
            this.pnlCard.BackColor = Color.White;
            this.pnlCard.BorderStyle = BorderStyle.FixedSingle;
            this.pnlCard.Controls.Add(this.lblUsername);
            this.pnlCard.Controls.Add(this.tbUsername);
            this.pnlCard.Controls.Add(this.lblPasskey);
            this.pnlCard.Controls.Add(this.tbPasskey);
            this.pnlCard.Controls.Add(this.lblStatus);
            this.pnlCard.Controls.Add(this.cmbStatus);
            this.pnlCard.Controls.Add(this.lblFullName);
            this.pnlCard.Controls.Add(this.tbFullName);
            this.pnlCard.Controls.Add(this.btnSave);
            this.pnlCard.Controls.Add(this.btnCancel);
            this.pnlCard.Location = new Point(15, 72);
            this.pnlCard.Size = new Size(420, 280);

            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblUsername.Location = new Point(15, 15);
            this.lblUsername.Text = "Username (solver_name) *:";

            // 
            // tbUsername
            // 
            this.tbUsername.Font = new Font("Segoe UI", 9.5F);
            this.tbUsername.Location = new Point(18, 35);
            this.tbUsername.MaxLength = 20;
            this.tbUsername.Size = new Size(382, 24);

            // 
            // lblPasskey
            // 
            this.lblPasskey.AutoSize = true;
            this.lblPasskey.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblPasskey.Location = new Point(15, 70);
            this.lblPasskey.Text = "Passkey / Password *:";

            // 
            // tbPasskey
            // 
            this.tbPasskey.Font = new Font("Segoe UI", 9.5F);
            this.tbPasskey.Location = new Point(18, 90);
            this.tbPasskey.MaxLength = 50;
            this.tbPasskey.Size = new Size(382, 24);

            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblStatus.Location = new Point(15, 125);
            this.lblStatus.Text = "Status / Role *:";

            // 
            // cmbStatus
            // 
            this.cmbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbStatus.Font = new Font("Segoe UI", 9.5F);
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Items.AddRange(new object[] { "solver", "uploader", "admin" });
            this.cmbStatus.Location = new Point(18, 145);
            this.cmbStatus.Size = new Size(382, 24);
            this.cmbStatus.SelectedIndex = 0;

            // 
            // lblFullName
            // 
            this.lblFullName.AutoSize = true;
            this.lblFullName.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblFullName.Location = new Point(15, 180);
            this.lblFullName.Text = "Full Name (Optional):";

            // 
            // tbFullName
            // 
            this.tbFullName.Font = new Font("Segoe UI", 9.5F);
            this.tbFullName.Location = new Point(18, 200);
            this.tbFullName.MaxLength = 60;
            this.tbFullName.Size = new Size(382, 24);

            // 
            // btnSave
            // 
            this.btnSave.BackColor = UITheme.GreenPrimary;
            this.btnSave.FlatStyle = FlatStyle.Flat;
            this.btnSave.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            this.btnSave.ForeColor = Color.White;
            this.btnSave.Location = new Point(18, 235);
            this.btnSave.Size = new Size(185, 34);
            this.btnSave.Text = "💾 Save User";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new EventHandler(this.btnSave_Click);

            // 
            // btnCancel
            // 
            this.btnCancel.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            this.btnCancel.Location = new Point(215, 235);
            this.btnCancel.Size = new Size(185, 34);
            this.btnCancel.Text = "❌ Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);

            // 
            // Form properties
            // 
            this.AcceptButton = this.btnSave;
            this.ClientSize = new Size(450, 365);
            this.Controls.Add(this.pnlCard);
            this.Controls.Add(this.pnlHeader);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Add New User Account";
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlCard.ResumeLayout(false);
            this.pnlCard.PerformLayout();
            this.ResumeLayout(false);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string username = tbUsername.Text.Trim();
            string passkey = tbPasskey.Text.Trim();
            string status = cmbStatus.SelectedItem != null ? cmbStatus.SelectedItem.ToString() : "solver";
            string fullName = tbFullName.Text.Trim();

            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Please enter a username.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbUsername.Focus();
                return;
            }

            if (username.Length < 2)
            {
                MessageBox.Show("Username must be at least 2 characters long.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbUsername.Focus();
                return;
            }

            string upperName = username.ToUpper();
            if (upperName == "ALL" || upperName == "UNASSIGNED" || upperName == "NONE" || upperName == "__NONE")
            {
                MessageBox.Show("Username '" + username + "' is reserved and cannot be used.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbUsername.Focus();
                return;
            }

            if (string.IsNullOrEmpty(passkey))
            {
                MessageBox.Show("Please enter a passkey / password.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbPasskey.Focus();
                return;
            }

            try
            {
                using (SqlConnection con = Database.GetConnectionObj())
                {
                    if (con == null) return;

                    // Check duplicate username
                    SqlCommand cmdCheck = new SqlCommand("SELECT COUNT(*) FROM dbo.solvers WHERE UPPER(solver_name) = UPPER(@uname)", con);
                    cmdCheck.Parameters.AddWithValue("@uname", username);
                    int count = Convert.ToInt32(cmdCheck.ExecuteScalar());

                    if (count > 0)
                    {
                        MessageBox.Show("A user with username '" + username + "' already exists! Please choose another username.", "Duplicate User", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        tbUsername.SelectAll();
                        tbUsername.Focus();
                        return;
                    }

                    // Insert new user
                    SqlCommand cmdInsert = new SqlCommand(
                        "INSERT INTO dbo.solvers (solver_name, passkey, status, full_name) VALUES (@uname, @pass, @status, @fname)", con);
                    cmdInsert.Parameters.AddWithValue("@uname", username);
                    cmdInsert.Parameters.AddWithValue("@pass", passkey);
                    cmdInsert.Parameters.AddWithValue("@status", status);
                    if (string.IsNullOrEmpty(fullName))
                    {
                        cmdInsert.Parameters.AddWithValue("@fname", DBNull.Value);
                    }
                    else
                    {
                        cmdInsert.Parameters.AddWithValue("@fname", fullName);
                    }

                    cmdInsert.ExecuteNonQuery();
                    con.Close();
                }

                MessageBox.Show("✅ User '" + username + "' created successfully as '" + status + "'!", "User Created", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error creating user:\n" + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
