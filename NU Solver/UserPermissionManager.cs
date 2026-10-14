using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace NU_Solver
{
    /// <summary>
    /// User account model for Role-Based Access Control (RBAC).
    /// Supports permissions: View Only, Print, Edit, Admin.
    /// </summary>
    [Serializable]
    public class UserAccount
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; } // Admin, Operator, Viewer, Officer
        public string Department { get; set; }
        
        // Granular Permissions
        public bool CanView { get; set; }          // View only / preview
        public bool CanPrint { get; set; }         // Generate & print transcripts/certificates
        public bool CanEdit { get; set; }          // Edit templates, typography, DB connection
        public bool CanManageUsers { get; set; }   // Admin user & permission administration
        public bool CanBatchProcess { get; set; }  // Start, pause, resume batch lot jobs

        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastLoginDate { get; set; }

        public UserAccount()
        {
            IsActive = true;
            CanView = true;
            CreatedDate = DateTime.Now;
            LastLoginDate = DateTime.Now;
        }

        public UserAccount(string username, string password, string fullName, string role, 
            bool canView, bool canPrint, bool canEdit, bool canManageUsers, string dept = "Exam Controller Branch")
        {
            Username = username;
            Password = password;
            FullName = fullName;
            Role = role;
            Department = dept;
            CanView = canView;
            CanPrint = canPrint;
            CanEdit = canEdit;
            CanManageUsers = canManageUsers;
            CanBatchProcess = canPrint || canManageUsers;
            IsActive = true;
            CreatedDate = DateTime.Now;
            LastLoginDate = DateTime.Now;
        }

        public string GetPermissionsSummary()
        {
            List<string> p = new List<string>();
            if (CanView) p.Add("View");
            if (CanPrint) p.Add("Print");
            if (CanEdit) p.Add("Edit");
            if (CanManageUsers) p.Add("Admin");
            return string.Join(", ", p.ToArray());
        }
    }

    /// <summary>
    /// Static session manager keeping track of currently authenticated user.
    /// </summary>
    public static class UserSession
    {
        private static UserAccount _currentUser;

        public static bool IsAuthenticated { get; set; }

        public static UserAccount CurrentUser
        {
            get { return _currentUser; }
            set
            {
                _currentUser = value;
                IsAuthenticated = (_currentUser != null);
            }
        }

        static UserSession()
        {
            _currentUser = null;
            IsAuthenticated = false;
        }

        public static void EnsureDefaultAdminIfUnset()
        {
            if (_currentUser == null)
            {
                _currentUser = new UserAccount(
                    "admin", "admin123", "Chief Exam Controller", "Admin",
                    true, true, true, true, "National University Examination Division");
                IsAuthenticated = true;
            }
        }

        public static bool IsAdmin
        {
            get { return CurrentUser != null && (CurrentUser.CanManageUsers || string.Equals(CurrentUser.Role, "admin", StringComparison.OrdinalIgnoreCase)); }
        }

        public static bool CanPrint
        {
            get { return CurrentUser != null && CurrentUser.CanPrint; }
        }

        public static bool CanEdit
        {
            get { return CurrentUser != null && CurrentUser.CanEdit; }
        }

        public static bool CanView
        {
            get { return CurrentUser != null && CurrentUser.CanView; }
        }

        public static void SwitchUser(UserAccount user)
        {
            if (user != null)
            {
                user.LastLoginDate = DateTime.Now;
                _currentUser = user;
                IsAuthenticated = true;
                UserPermissionManager.SaveToFile();
            }
        }

        public static void Logout()
        {
            _currentUser = null;
            IsAuthenticated = false;
        }
    }

    /// <summary>
    /// Persistent storage manager for user credentials and RBAC permissions.
    /// Stored in 'Reports/UserPermissions.xml' with SQL Server synchronization in 'transcript_user'.
    /// </summary>
    public static class UserPermissionManager
    {
        private static List<UserAccount> _users = new List<UserAccount>();
        private static readonly string FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports", "UserPermissions.xml");

        static UserPermissionManager()
        {
            LoadFromFile();
            if (_users.Count == 0)
            {
                InitializeDefaultAccounts();
            }
        }

        public static List<UserAccount> GetAllUsers()
        {
            return new List<UserAccount>(_users);
        }

        public static void InitializeDefaultAccounts()
        {
            _users.Clear();
            // 1. Super Admin: full access
            _users.Add(new UserAccount("admin", "admin123", "Chief Exam Controller", "Admin", 
                true, true, true, true, "Examination Confidential Branch"));

            // 2. Lead Operator: View + Print + Batch
            _users.Add(new UserAccount("operator1", "op123", "Md. Enamul Karim (Section Officer)", "Operator", 
                true, true, false, false, "Certificate & Transcript Section"));

            // 3. Template & Course Editor: View + Edit
            _users.Add(new UserAccount("editor1", "edit123", "Senior Computer Programmer", "Editor", 
                true, false, true, false, "ICT & Database Cell"));

            // 4. View Only Auditor: Can only view/preview
            _users.Add(new UserAccount("viewer1", "view123", "Quality Assurance Inspector", "Viewer", 
                true, false, false, false, "Audit & Verification Cell"));

            // 5. Verification Officer: View + Print
            _users.Add(new UserAccount("officer_nu", "nu2026", "Certificate Verification Desk", "Officer", 
                true, true, false, false, "One Stop Service Center"));

            SaveToFile();
        }

        public static bool EnsureTranscriptUserTableAndSeed(string connStr)
        {
            if (string.IsNullOrEmpty(connStr)) return false;
            try
            {
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    con.Open();
                    string createTableSql = @"
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'transcript_user')
BEGIN
    CREATE TABLE dbo.transcript_user (
        id INT IDENTITY(1,1) PRIMARY KEY,
        username VARCHAR(50) NOT NULL UNIQUE,
        password VARCHAR(100) NOT NULL,
        full_name NVARCHAR(150) NULL,
        role VARCHAR(50) NULL,
        department NVARCHAR(150) NULL,
        can_view BIT NOT NULL DEFAULT 1,
        can_print BIT NOT NULL DEFAULT 1,
        can_edit BIT NOT NULL DEFAULT 0,
        can_manage_users BIT NOT NULL DEFAULT 0,
        can_batch_process BIT NOT NULL DEFAULT 0,
        is_active BIT NOT NULL DEFAULT 1,
        created_date DATETIME NOT NULL DEFAULT GETDATE(),
        last_login_date DATETIME NULL
    );
END";
                    using (SqlCommand cmd = new SqlCommand(createTableSql, con))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    if (_users.Count == 0) InitializeDefaultAccounts();

                    foreach (UserAccount u in _users)
                    {
                        string chkSql = "SELECT COUNT(*) FROM dbo.transcript_user WHERE username = @u";
                        using (SqlCommand chkCmd = new SqlCommand(chkSql, con))
                        {
                            chkCmd.Parameters.AddWithValue("@u", u.Username);
                            int cnt = Convert.ToInt32(chkCmd.ExecuteScalar());
                            if (cnt == 0)
                            {
                                string insSql = @"
INSERT INTO dbo.transcript_user 
(username, password, full_name, role, department, can_view, can_print, can_edit, can_manage_users, can_batch_process, is_active, created_date)
VALUES 
(@username, @password, @full_name, @role, @department, @can_view, @can_print, @can_edit, @can_manage_users, @can_batch_process, @is_active, @created_date)";

                                using (SqlCommand insCmd = new SqlCommand(insSql, con))
                                {
                                    insCmd.Parameters.AddWithValue("@username", u.Username);
                                    insCmd.Parameters.AddWithValue("@password", u.Password);
                                    insCmd.Parameters.AddWithValue("@full_name", (object)u.FullName ?? DBNull.Value);
                                    insCmd.Parameters.AddWithValue("@role", (object)u.Role ?? DBNull.Value);
                                    insCmd.Parameters.AddWithValue("@department", (object)u.Department ?? DBNull.Value);
                                    insCmd.Parameters.AddWithValue("@can_view", u.CanView);
                                    insCmd.Parameters.AddWithValue("@can_print", u.CanPrint);
                                    insCmd.Parameters.AddWithValue("@can_edit", u.CanEdit);
                                    insCmd.Parameters.AddWithValue("@can_manage_users", u.CanManageUsers);
                                    insCmd.Parameters.AddWithValue("@can_batch_process", u.CanBatchProcess);
                                    insCmd.Parameters.AddWithValue("@is_active", u.IsActive);
                                    insCmd.Parameters.AddWithValue("@created_date", u.CreatedDate);
                                    insCmd.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public static void AddUser(UserAccount account)
        {
            if (account == null) return;
            _users.RemoveAll(u => u.Username.Equals(account.Username, StringComparison.OrdinalIgnoreCase));
            _users.Add(account);
            SaveToFile();
        }

        public static void UpdateUser(UserAccount account)
        {
            if (account == null) return;
            for (int i = 0; i < _users.Count; i++)
            {
                if (_users[i].Username.Equals(account.Username, StringComparison.OrdinalIgnoreCase))
                {
                    _users[i] = account;
                    SaveToFile();
                    return;
                }
            }
            _users.Add(account);
            SaveToFile();
        }

        public static void DeleteUser(string username)
        {
            if (string.IsNullOrEmpty(username)) return;
            // Prevent deleting the root admin
            if (username.ToLower() == "admin") return;
            _users.RemoveAll(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
            SaveToFile();
        }

        public static UserAccount Authenticate(string username, string password, string connStr = null)
        {
            if (string.IsNullOrEmpty(username)) return null;

            // 1. Attempt database authentication against dbo.transcript_user if connection string provided
            if (!string.IsNullOrEmpty(connStr))
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(connStr))
                    {
                        con.Open();
                        string sql = @"
SELECT username, password, full_name, role, department, can_view, can_print, can_edit, can_manage_users, can_batch_process, is_active 
FROM dbo.transcript_user 
WHERE username = @u AND password = @p AND is_active = 1";

                        using (SqlCommand cmd = new SqlCommand(sql, con))
                        {
                            cmd.Parameters.AddWithValue("@u", username.Trim());
                            cmd.Parameters.AddWithValue("@p", password);

                            using (SqlDataReader rdr = cmd.ExecuteReader())
                            {
                                if (rdr.Read())
                                {
                                    UserAccount dbUser = new UserAccount
                                    {
                                        Username = rdr["username"].ToString().Trim(),
                                        Password = rdr["password"].ToString(),
                                        FullName = rdr["full_name"] != DBNull.Value ? rdr["full_name"].ToString().Trim() : username,
                                        Role = rdr["role"] != DBNull.Value ? rdr["role"].ToString().Trim() : "Operator",
                                        Department = rdr["department"] != DBNull.Value ? rdr["department"].ToString().Trim() : "Examination Branch",
                                        CanView = rdr["can_view"] != DBNull.Value && Convert.ToBoolean(rdr["can_view"]),
                                        CanPrint = rdr["can_print"] != DBNull.Value && Convert.ToBoolean(rdr["can_print"]),
                                        CanEdit = rdr["can_edit"] != DBNull.Value && Convert.ToBoolean(rdr["can_edit"]),
                                        CanManageUsers = rdr["can_manage_users"] != DBNull.Value && Convert.ToBoolean(rdr["can_manage_users"]),
                                        CanBatchProcess = rdr["can_batch_process"] != DBNull.Value && Convert.ToBoolean(rdr["can_batch_process"]),
                                        IsActive = true,
                                        LastLoginDate = DateTime.Now
                                    };

                                    rdr.Close();
                                    try
                                    {
                                        using (SqlCommand upd = new SqlCommand("UPDATE dbo.transcript_user SET last_login_date = GETDATE() WHERE username = @u", con))
                                        {
                                            upd.Parameters.AddWithValue("@u", username.Trim());
                                            upd.ExecuteNonQuery();
                                        }
                                    }
                                    catch { }

                                    AddUser(dbUser);
                                    return dbUser;
                                }
                            }
                        }
                    }
                }
                catch { }
            }

            // 2. Fallback to local accounts
            foreach (UserAccount u in _users)
            {
                if (u.Username.Equals(username, StringComparison.OrdinalIgnoreCase) && 
                    u.Password == password && u.IsActive)
                {
                    u.LastLoginDate = DateTime.Now;
                    SaveToFile();
                    return u;
                }
            }
            return null;
        }

        public static void SaveToFile()
        {
            try
            {
                string dir = Path.GetDirectoryName(FilePath);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                XmlSerializer serializer = new XmlSerializer(typeof(List<UserAccount>));
                using (StreamWriter writer = new StreamWriter(FilePath, false, Encoding.UTF8))
                {
                    serializer.Serialize(writer, _users);
                }
            }
            catch
            {
                // File lock or permission fail-safe
            }
        }

        public static void LoadFromFile()
        {
            try
            {
                if (!File.Exists(FilePath)) return;
                XmlSerializer serializer = new XmlSerializer(typeof(List<UserAccount>));
                using (StreamReader reader = new StreamReader(FilePath, Encoding.UTF8))
                {
                    _users = (List<UserAccount>)serializer.Deserialize(reader);
                }
            }
            catch
            {
                _users = new List<UserAccount>();
            }
        }
    }

    /// <summary>
    /// User Credential and Authority Profile View Dialog.
    /// Displays authenticated operator details and granular permissions from dbo.transcript_user.
    /// </summary>
    public class UserCredentialViewDialog : Form
    {
        private UserAccount _account;
        private string _connStr;
        private Label lblValUser;
        private Label lblValRole;
        private Label lblValName;
        private Label lblValDept;
        private Label lblValSource;
        private Label lblValLastLogin;

        private Label lblPermView;
        private Label lblPermPrint;
        private Label lblPermEdit;
        private Label lblPermAdmin;
        private Label lblPermBatch;

        public UserCredentialViewDialog(UserAccount account, string connStr = null)
        {
            _account = account ?? UserSession.CurrentUser;
            _connStr = connStr;
            InitializeComponent();
            RefreshAccountDisplay();
        }

        private void InitializeComponent()
        {
            this.Text = "Operator Credentials & Authority Profile - National University";
            this.Size = new Size(580, 560);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(248, 250, 252);
            this.Font = new Font("Segoe UI", 9.5f);

            // Header Banner
            Panel pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = VectorGraphics.ColorGreenDark
            };
            pnlHeader.Paint += (s, e) =>
            {
                VectorGraphics.PaintHeaderBanner(
                    e.Graphics,
                    pnlHeader.ClientRectangle,
                    "NATIONAL UNIVERSITY, BANGLADESH",
                    "Operator Credentials & Authority Profile");
            };

            // Main scrollable content
            Panel pnlBody = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(16, 12, 16, 12),
                AutoScroll = true
            };

            // Card 1: User Account & Authority Details
            GroupBox grpUser = new GroupBox
            {
                Text = "Authenticated Identity Details",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = VectorGraphics.ColorGreenDark,
                Location = new Point(14, 10),
                Size = new Size(530, 195),
                BackColor = Color.White
            };

            int ly = 24;
            int lblW = 140;
            int valW = 360;

            Label lblU = new Label { Text = "Operator Username:", Font = UITheme.FontMain, Location = new Point(16, ly), AutoSize = true, UseMnemonic = false };
            lblValUser = new Label { Font = new Font("Segoe UI", 9.5f, FontStyle.Bold), ForeColor = VectorGraphics.ColorTextPrimary, Location = new Point(lblW + 20, ly), Size = new Size(valW, 20), UseMnemonic = false };
            grpUser.Controls.Add(lblU);
            grpUser.Controls.Add(lblValUser);

            ly += 26;
            Label lblR = new Label { Text = "Assigned Role:", Font = UITheme.FontMain, Location = new Point(16, ly), AutoSize = true, UseMnemonic = false };
            lblValRole = new Label { Font = new Font("Segoe UI", 9f, FontStyle.Bold), ForeColor = Color.White, BackColor = VectorGraphics.ColorSecondaryDark, Location = new Point(lblW + 20, ly - 2), Size = new Size(130, 22), TextAlign = ContentAlignment.MiddleCenter, UseMnemonic = false };
            grpUser.Controls.Add(lblR);
            grpUser.Controls.Add(lblValRole);

            ly += 28;
            Label lblN = new Label { Text = "Full Name / Title:", Font = UITheme.FontMain, Location = new Point(16, ly), AutoSize = true, UseMnemonic = false };
            lblValName = new Label { Font = UITheme.FontMain, ForeColor = VectorGraphics.ColorTextPrimary, Location = new Point(lblW + 20, ly), Size = new Size(valW, 20), UseMnemonic = false };
            grpUser.Controls.Add(lblN);
            grpUser.Controls.Add(lblValName);

            ly += 26;
            Label lblD = new Label { Text = "Department / Cell:", Font = UITheme.FontMain, Location = new Point(16, ly), AutoSize = true, UseMnemonic = false };
            lblValDept = new Label { Font = UITheme.FontMain, ForeColor = VectorGraphics.ColorTextPrimary, Location = new Point(lblW + 20, ly), Size = new Size(valW, 20), UseMnemonic = false };
            grpUser.Controls.Add(lblD);
            grpUser.Controls.Add(lblValDept);

            ly += 26;
            Label lblS = new Label { Text = "Security Authority:", Font = UITheme.FontMain, Location = new Point(16, ly), AutoSize = true, UseMnemonic = false };
            lblValSource = new Label { Font = new Font("Segoe UI", 8.5f), ForeColor = Color.FromArgb(71, 85, 105), Location = new Point(lblW + 20, ly), Size = new Size(valW, 20), UseMnemonic = false };
            grpUser.Controls.Add(lblS);
            grpUser.Controls.Add(lblValSource);

            ly += 26;
            Label lblL = new Label { Text = "Last Session Login:", Font = UITheme.FontMain, Location = new Point(16, ly), AutoSize = true, UseMnemonic = false };
            lblValLastLogin = new Label { Font = new Font("Segoe UI", 8.5f), ForeColor = Color.FromArgb(71, 85, 105), Location = new Point(lblW + 20, ly), Size = new Size(valW, 20), UseMnemonic = false };
            grpUser.Controls.Add(lblL);
            grpUser.Controls.Add(lblValLastLogin);

            pnlBody.Controls.Add(grpUser);

            // Card 2: Granular RBAC Permissions
            GroupBox grpPerms = new GroupBox
            {
                Text = "Granted Permissions & Authority Matrix",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = VectorGraphics.ColorGreenDark,
                Location = new Point(14, 215),
                Size = new Size(530, 195),
                BackColor = Color.White
            };

            int py = 24;
            lblPermView = CreatePermRow(grpPerms, "View & Preview Transcripts / Certificates:", py);
            py += 30;
            lblPermPrint = CreatePermRow(grpPerms, "Generate & Print Official Documents:", py);
            py += 30;
            lblPermEdit = CreatePermRow(grpPerms, "Course Design & Template Editor:", py);
            py += 30;
            lblPermAdmin = CreatePermRow(grpPerms, "Administrator & User Management:", py);
            py += 30;
            lblPermBatch = CreatePermRow(grpPerms, "Batch Lot Processing (Pause / Resume):", py);

            pnlBody.Controls.Add(grpPerms);

            // Bottom Action Panel
            Panel pnlBottom = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 60,
                BackColor = Color.FromArgb(241, 245, 249),
                Padding = new Padding(16, 10, 16, 10)
            };

            Button btnProceed = new Button
            {
                Text = "✓ Continue to Workspace",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Size = new Size(220, 38),
                Location = new Point(16, 11),
                Cursor = Cursors.Hand,
                UseMnemonic = false
            };
            VectorGraphics.ApplyModernButton(
                btnProceed,
                VectorGraphics.ColorGreenMain,
                VectorGraphics.ColorGreenDark,
                VectorGraphics.ColorGreenHover,
                Color.White,
                6);
            btnProceed.Click += (s, e) =>
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            };

            Button btnSwitch = new Button
            {
                Text = "Switch User",
                Font = new Font("Segoe UI", 9f),
                Size = new Size(130, 38),
                Location = new Point(245, 11),
                Cursor = Cursors.Hand,
                UseMnemonic = false
            };
            VectorGraphics.ApplyOutlinedButton(btnSwitch, VectorGraphics.ColorSecondaryDark, VectorGraphics.ColorSecondaryHover, 6);
            btnSwitch.Click += (s, e) =>
            {
                using (OperatorLoginForm login = new OperatorLoginForm(_connStr))
                {
                    if (login.ShowDialog(this) == DialogResult.OK)
                    {
                        _account = UserSession.CurrentUser;
                        RefreshAccountDisplay();
                    }
                }
            };

            Button btnClose = new Button
            {
                Text = "Close",
                Font = new Font("Segoe UI", 9f),
                Size = new Size(100, 38),
                Location = new Point(445, 11),
                Cursor = Cursors.Hand
            };
            VectorGraphics.ApplyOutlinedButton(btnClose, Color.FromArgb(148, 163, 184), Color.FromArgb(100, 116, 139), 6);
            btnClose.Click += (s, e) =>
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            };

            pnlBottom.Controls.Add(btnProceed);
            pnlBottom.Controls.Add(btnSwitch);
            pnlBottom.Controls.Add(btnClose);

            this.Controls.Add(pnlBody);
            this.Controls.Add(pnlBottom);
            this.Controls.Add(pnlHeader);
            this.AcceptButton = btnProceed;
        }

        private Label CreatePermRow(GroupBox parent, string labelText, int topY)
        {
            Label lbl = new Label
            {
                Text = labelText,
                Font = UITheme.FontMain,
                Location = new Point(16, topY),
                Size = new Size(340, 22),
                UseMnemonic = false
            };
            Label lblBadge = new Label
            {
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                Location = new Point(365, topY - 2),
                Size = new Size(140, 22),
                TextAlign = ContentAlignment.MiddleCenter,
                UseMnemonic = false
            };
            parent.Controls.Add(lbl);
            parent.Controls.Add(lblBadge);
            return lblBadge;
        }

        private void SetBadge(Label lblBadge, bool granted)
        {
            if (granted)
            {
                lblBadge.Text = "✓ Granted";
                lblBadge.ForeColor = Color.White;
                lblBadge.BackColor = Color.FromArgb(16, 185, 129); // Modern Emerald Green
            }
            else
            {
                lblBadge.Text = "✗ Not Granted";
                lblBadge.ForeColor = Color.White;
                lblBadge.BackColor = Color.FromArgb(239, 68, 68); // Soft Red
            }
        }

        private void RefreshAccountDisplay()
        {
            if (_account == null) _account = UserSession.CurrentUser;
            if (_account == null)
            {
                _account = new UserAccount("anonymous", "", "Guest User", "Viewer", true, false, false, false);
            }

            lblValUser.Text = _account.Username;
            lblValRole.Text = (_account.Role ?? "Operator").ToUpper();
            lblValName.Text = _account.FullName ?? _account.Username;
            lblValDept.Text = _account.Department ?? "Examination Control Division";
            lblValSource.Text = "dbo.transcript_user on Microsoft SQL Server";
            lblValLastLogin.Text = _account.LastLoginDate != DateTime.MinValue ? _account.LastLoginDate.ToString("yyyy-MM-dd HH:mm:ss") : DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            SetBadge(lblPermView, _account.CanView);
            SetBadge(lblPermPrint, _account.CanPrint);
            SetBadge(lblPermEdit, _account.CanEdit);
            SetBadge(lblPermAdmin, _account.CanManageUsers);
            SetBadge(lblPermBatch, _account.CanBatchProcess);
        }
    }
}
