using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace NU_Solver
{
    public class TranscriptForm : Form
    {
        // Layout: Top Banner, Left Sidebar (Docked Left), Splitter, Right Preview (Docked Fill)
        private Panel pnlHeaderBanner;
        private Panel pnlSidebar;
        private Splitter sideSplitter;
        private Panel pnlMainPreview;

        // Left Panel Scrollable Area
        private Panel pnlLeftScroll;

        // Card 1: Candidate Search & Filter
        private Panel pnlCardParams;
        private Label lblCardParamsTitle;
        private Label lblExmYear;
        private TextBox txtExmYear;
        private CheckBox chkUseFastTable;
        private Label lblFastModeHelp;
        private Label lblRegNo;
        private TextBox txtRegNo;
        private Label lblHelpReg;
        private Button btnSearchCandidate;
        private Button btnStopSearch;

        // Card 2: Course Design & Document Configuration
        private Panel pnlCardCourse;
        private Label lblCardCourseTitle;
        private Label lblCoursePreset;
        private ComboBox cmbCoursePreset;
        private Button btnQuickCourseEdit;
        private Label lblStoredProc;
        private ComboBox cmbStoredProc;
        private Label lblPageSize;
        private ComboBox cmbPageSize;
        private Label lblIssueDate;
        private TextBox txtIssueDate;
        private Label lblRefPrefix;
        private TextBox txtRefPrefix;
        private Label lblStartRefNo;
        private TextBox txtStartRefNo;
        private Label lblStartBookSl;
        private TextBox txtStartBookSl;
        private Label lblDbChip;
        private Label lblConnStr;
        private TextBox txtConnStr;
        private Button btnTestDbQuick;

        // Card 3: Document Generation
        private Panel pnlCardActions;
        private Label lblCardActionsTitle;
        private Button btnGenerateTranscript;
        private Button btnGenerateCertificate;
        private Button btnStopGen;
        private CheckBox chkDemoWatermark;
        private Label lblDemoHelp;

        // Card 4: Administration & Audit Tools
        private Panel pnlCardAdminTools;
        private Label lblCardAdminTitle;
        private Button btnCourseSettings;
        private Button btnAuditLog;
        private Button btnAdminPanel;

        // Card 5: System Status & Diagnostic Indicator
        private Panel pnlCardStatus;
        private ProgressBar progressBar;
        private Label lblStatusPill;
        private Label lblStatusDetails;
        private Button btnCopyError;

        // Right Preview Toolbar & Browser
        private Panel pnlPreviewToolbar;
        private Label lblDocBadge;
        private Label lblPreviewSummary;
        private Button btnPrint;
        private Button btnPrintPreview;
        private Button btnOpenBrowser;
        private Button btnOpenFolder;
        private Button btnHeaderAdmin;
        private Button btnHeaderLogin;
        private Label lblUserRoleBadge;

        private Panel pnlBrowserWrap;
        private WebBrowser webBrowser;

        // State & Workers
        private BackgroundWorker bgWorker;
        private SqlCommand activeQueryCmd = null;
        private string currentGeneratedHtmlPath = null;
        private string currentDocType = "TRANSCRIPT";
        private int lastGeneratedCount = 0;

        private List<CourseDesignConfig> loadedDesigns;
        private CourseDesignConfig currentConfig;

        public TranscriptForm()
        {
            InitializeComponent();
            LoadCourseProfiles();
            ApplyUserPermissions();
        }

        public TranscriptForm(string defaultYear, string defaultRegNo = "")
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(defaultYear)) txtExmYear.Text = defaultYear;
            if (!string.IsNullOrEmpty(defaultRegNo)) txtRegNo.Text = defaultRegNo;
            LoadCourseProfiles();
            ApplyUserPermissions();
        }

        private void InitializeComponent()
        {
            this.Text = "National University - Academic Transcript & Certificate System";
            this.Size = new Size(1360, 880);
            this.MinimumSize = new Size(1050, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);
            this.BackColor = VectorGraphics.ColorAppBg;
            this.ForeColor = VectorGraphics.ColorTextPrimary;

            // ==========================================
            // 1. TOP HEADER BANNER (Material Deep Indigo + Crest)
            // ==========================================
            pnlHeaderBanner = new Panel
            {
                Dock = DockStyle.Top,
                Height = 56,
                BackColor = VectorGraphics.ColorPrimaryDark
            };
            pnlHeaderBanner.Paint += (s, e) =>
            {
                VectorGraphics.PaintHeaderBanner(
                    e.Graphics, 
                    pnlHeaderBanner.ClientRectangle, 
                    "NATIONAL UNIVERSITY, BANGLADESH", 
                    "Academic Transcript & Provisional Certificate Management System");
            };

            // Right header actions flow
            FlowLayoutPanel pnlHeaderRight = new FlowLayoutPanel
            {
                Dock = DockStyle.Right,
                AutoSize = true,
                FlowDirection = FlowDirection.RightToLeft,
                WrapContents = false,
                Padding = new Padding(0, 10, 14, 0),
                BackColor = Color.Transparent
            };

            btnHeaderAdmin = new Button
            {
                Text = "Admin Workflow",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = Color.White,
                Size = new Size(140, 34),
                Margin = new Padding(8, 0, 0, 0),
                Cursor = Cursors.Hand,
                UseMnemonic = false
            };
            VectorGraphics.ApplyModernButton(
                btnHeaderAdmin,
                VectorGraphics.ColorSecondary,
                VectorGraphics.ColorSecondaryDark,
                VectorGraphics.ColorSecondaryHover,
                Color.White,
                6);

            btnHeaderAdmin.Click += (s, e) =>
            {
                using (AdminDashboardForm dlg = new AdminDashboardForm())
                {
                    dlg.ShowDialog(this);
                    ApplyUserPermissions();
                }
            };

            btnHeaderLogin = new Button
            {
                Text = "Switch Operator",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = Color.White,
                Size = new Size(130, 34),
                Margin = new Padding(8, 0, 0, 0),
                Cursor = Cursors.Hand,
                UseMnemonic = false
            };
            VectorGraphics.ApplyModernButton(
                btnHeaderLogin,
                Color.FromArgb(14, 116, 144),
                Color.FromArgb(15, 95, 118),
                Color.FromArgb(2, 132, 199),
                Color.White,
                6);
            btnHeaderLogin.Click += (s, e) =>
            {
                using (OperatorLoginForm dlg = new OperatorLoginForm(txtConnStr.Text))
                {
                    if (dlg.ShowDialog(this) == DialogResult.OK)
                    {
                        using (UserCredentialViewDialog credDlg = new UserCredentialViewDialog(UserSession.CurrentUser, txtConnStr.Text))
                        {
                            credDlg.ShowDialog(this);
                        }
                        ApplyUserPermissions();
                    }
                }
            };

            Button btnHeaderProfile = new Button
            {
                Text = "User Profile",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = Color.White,
                Size = new Size(115, 34),
                Margin = new Padding(8, 0, 0, 0),
                Cursor = Cursors.Hand,
                UseMnemonic = false
            };
            VectorGraphics.ApplyModernButton(
                btnHeaderProfile,
                VectorGraphics.ColorGreenMain,
                VectorGraphics.ColorGreenDark,
                VectorGraphics.ColorGreenHover,
                Color.White,
                6);
            btnHeaderProfile.Click += (s, e) =>
            {
                using (UserCredentialViewDialog credDlg = new UserCredentialViewDialog(UserSession.CurrentUser, txtConnStr.Text))
                {
                    credDlg.ShowDialog(this);
                    ApplyUserPermissions();
                }
            };

            lblUserRoleBadge = new Label
            {
                Text = "Role: " + (UserSession.CurrentUser != null ? UserSession.CurrentUser.Role : "Admin"),
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(40, 255, 255, 255),
                Padding = new Padding(10, 8, 10, 8),
                Margin = new Padding(4, 1, 4, 0),
                AutoSize = true,
                UseMnemonic = false
            };

            pnlHeaderRight.Controls.Add(btnHeaderAdmin);
            pnlHeaderRight.Controls.Add(btnHeaderLogin);
            pnlHeaderRight.Controls.Add(btnHeaderProfile);
            pnlHeaderRight.Controls.Add(lblUserRoleBadge);
            pnlHeaderBanner.Controls.Add(pnlHeaderRight);

            // ==========================================
            // 2. LEFT SIDEBAR (Dock = Left, Width = 380)
            // ==========================================
            pnlSidebar = new Panel
            {
                Dock = DockStyle.Left,
                Width = 380,
                BackColor = VectorGraphics.ColorAppBg,
                Padding = new Padding(0)
            };

            sideSplitter = new Splitter
            {
                Dock = DockStyle.Left,
                Width = 5,
                BackColor = VectorGraphics.ColorBorderDivider,
                MinSize = 360,
                MinExtra = 450
            };

            pnlLeftScroll = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(12, 10, 12, 30),
                BackColor = VectorGraphics.ColorAppBg
            };

            int cardW = 356;
            int cardPad = 14;
            int innerW = cardW - (cardPad * 2);

            // ------------------------------------------
            // CARD 1: Candidate Search & Filter
            // ------------------------------------------
            pnlCardParams = new Panel
            {
                Width = cardW,
                Height = 216,
                Location = new Point(12, 10),
                BackColor = Color.White
            };
            pnlCardParams.Paint += (s, e) => VectorGraphics.PaintCard(e.Graphics, new Rectangle(0, 0, pnlCardParams.Width, pnlCardParams.Height), 8);

            lblCardParamsTitle = new Label
            {
                Text = "CANDIDATE SEARCH",
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                ForeColor = VectorGraphics.ColorPrimary,
                Location = new Point(cardPad, 12),
                AutoSize = true
            };

            lblExmYear = new Label
            {
                Text = "Exam Year (@EXM_YEAR):",
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                ForeColor = VectorGraphics.ColorTextPrimary,
                Location = new Point(cardPad, 34),
                AutoSize = true
            };

            txtExmYear = new TextBox
            {
                Text = "2024",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = VectorGraphics.ColorTextPrimary,
                Location = new Point(cardPad, 52),
                Width = 100,
                BorderStyle = BorderStyle.FixedSingle
            };

            chkUseFastTable = new CheckBox
            {
                Text = "⚡ Fast Mode",
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                ForeColor = VectorGraphics.ColorSecondaryDark,
                Checked = true,
                Location = new Point(cardPad + 115, 52),
                AutoSize = true,
                Cursor = Cursors.Hand
            };

            lblFastModeHelp = new Label
            {
                Text = "Uses precomputed snapshot table for instant batch generation.",
                Font = new Font("Segoe UI", 7.5f, FontStyle.Italic),
                ForeColor = VectorGraphics.ColorTextSecondary,
                Location = new Point(cardPad + 115, 74),
                Width = innerW - 115,
                Height = 34
            };

            lblRegNo = new Label
            {
                Text = "Registration No (@reg_no):",
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                ForeColor = VectorGraphics.ColorTextPrimary,
                Location = new Point(cardPad, 110),
                AutoSize = true
            };

            txtRegNo = new TextBox
            {
                Text = "",
                Font = new Font("Segoe UI", 9.5f),
                ForeColor = VectorGraphics.ColorTextPrimary,
                Location = new Point(cardPad, 128),
                Width = innerW,
                BorderStyle = BorderStyle.FixedSingle
            };

            lblHelpReg = new Label
            {
                Text = "• Blank = entire lot. Or specify Registration No for a single examinee.",
                Font = new Font("Segoe UI", 7.5f, FontStyle.Italic),
                ForeColor = VectorGraphics.ColorTextSecondary,
                Location = new Point(cardPad, 154),
                Width = innerW,
                AutoSize = true
            };

            btnSearchCandidate = new Button
            {
                Text = "🔍 Search / Verify",
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                Location = new Point(cardPad, 174),
                Size = new Size(innerW - 80, 30)
            };
            VectorGraphics.ApplyOutlinedButton(btnSearchCandidate, VectorGraphics.ColorPrimary, VectorGraphics.ColorPrimary, 6);
            btnSearchCandidate.Click += (s, e) =>
            {
                string yr = txtExmYear.Text.Trim();
                if (string.IsNullOrEmpty(yr) || yr.Length != 4)
                {
                    MessageBox.Show("Please enter a valid 4-digit Exam Year first.", "Search Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtExmYear.Focus();
                    return;
                }

                string regNum = txtRegNo.Text.Trim();
                if (string.IsNullOrEmpty(regNum))
                {
                    lblStatusPill.Text = "● Ready";
                    lblStatusPill.ForeColor = VectorGraphics.ColorPrimary;
                    lblStatusDetails.Text = string.Format("Exam Year {0} active. LOT mode enabled (all examinees). Ready to generate.", yr);
                }
                else
                {
                    lblStatusPill.Text = "🔍 Single Candidate";
                    lblStatusPill.ForeColor = VectorGraphics.ColorSecondary;
                    lblStatusDetails.Text = string.Format("Targeting candidate Reg No: {0}, Year: {1}. Click Generate to produce document.", regNum, yr);
                }
            };

            btnStopSearch = new Button
            {
                Text = "⏹ Stop",
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                Location = new Point(cardPad + innerW - 74, 174),
                Size = new Size(74, 30),
                Cursor = Cursors.Hand
            };
            VectorGraphics.ApplyOutlinedButton(btnStopSearch, VectorGraphics.ColorWarning, VectorGraphics.ColorWarning, 6);
            btnStopSearch.Click += (s, e) => RequestStopProcess();

            pnlCardParams.Controls.Add(lblCardParamsTitle);
            pnlCardParams.Controls.Add(lblExmYear);
            pnlCardParams.Controls.Add(txtExmYear);
            pnlCardParams.Controls.Add(chkUseFastTable);
            pnlCardParams.Controls.Add(lblFastModeHelp);
            pnlCardParams.Controls.Add(lblRegNo);
            pnlCardParams.Controls.Add(txtRegNo);
            pnlCardParams.Controls.Add(lblHelpReg);
            pnlCardParams.Controls.Add(btnSearchCandidate);
            pnlCardParams.Controls.Add(btnStopSearch);

            // ------------------------------------------
            // CARD 2: Document Configuration
            // ------------------------------------------
            pnlCardCourse = new Panel
            {
                Width = cardW,
                Height = 310,
                Location = new Point(12, 238),
                BackColor = Color.White
            };
            pnlCardCourse.Paint += (s, e) => VectorGraphics.PaintCard(e.Graphics, new Rectangle(0, 0, pnlCardCourse.Width, pnlCardCourse.Height), 8);

            lblCardCourseTitle = new Label
            {
                Text = "DOCUMENT CONFIGURATION",
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                ForeColor = VectorGraphics.ColorSecondary,
                Location = new Point(cardPad, 12),
                AutoSize = true
            };

            lblCoursePreset = new Label
            {
                Text = "Course Profile Preset:",
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                ForeColor = VectorGraphics.ColorTextPrimary,
                Location = new Point(cardPad, 32),
                AutoSize = true
            };

            cmbCoursePreset = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9f),
                Location = new Point(cardPad, 50),
                Width = innerW - 38
            };
            cmbCoursePreset.SelectedIndexChanged += CmbCoursePreset_SelectedIndexChanged;

            btnQuickCourseEdit = new Button
            {
                Text = "⚙",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Location = new Point(cardPad + innerW - 34, 49),
                Size = new Size(34, 26),
                Cursor = Cursors.Hand,
                FlatStyle = FlatStyle.Flat
            };
            btnQuickCourseEdit.FlatAppearance.BorderColor = VectorGraphics.ColorBorderDivider;
            btnQuickCourseEdit.Click += btnCourseSettings_Click;

            lblStoredProc = new Label
            {
                Text = "Stored Procedure / Function (@proc):",
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                ForeColor = VectorGraphics.ColorTextPrimary,
                Location = new Point(cardPad, 82),
                AutoSize = true
            };

            cmbStoredProc = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDown,
                Font = new Font("Segoe UI", 9f),
                Location = new Point(cardPad, 99),
                Width = innerW
            };
            cmbStoredProc.Items.AddRange(new object[] {
                "dbo.BBAMCPCTRANS_lot",
                "dbo.BBAMCPCTRANS_single",
                "dbo.BEDHONS_lot",
                "dbo.CSE_lot",
                "dbo.MED_lot"
            });
            cmbStoredProc.Text = "dbo.BBAMCPCTRANS_lot";

            lblPageSize = new Label
            {
                Text = "Paper Size:",
                Font = new Font("Segoe UI", 8.5f),
                Location = new Point(cardPad, 128),
                AutoSize = true
            };

            cmbPageSize = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                Location = new Point(cardPad, 145),
                Width = 100
            };
            cmbPageSize.Items.AddRange(new object[] { "A4", "Legal" });
            cmbPageSize.SelectedIndex = 0;

            lblIssueDate = new Label
            {
                Text = "Issue Date:",
                Font = new Font("Segoe UI", 8.5f),
                Location = new Point(cardPad + 112, 128),
                AutoSize = true
            };

            txtIssueDate = new TextBox
            {
                Text = DateTime.Now.ToString("MMMM dd, yyyy"),
                Font = new Font("Segoe UI", 8.5f),
                Location = new Point(cardPad + 112, 145),
                Width = innerW - 112,
                BorderStyle = BorderStyle.FixedSingle
            };

            lblRefPrefix = new Label
            {
                Text = "Reference Prefix:",
                Font = new Font("Segoe UI", 8.5f),
                Location = new Point(cardPad, 172),
                AutoSize = true
            };

            txtRefPrefix = new TextBox
            {
                Text = "NU/Con/Certi/Trans/{EXM_CODE}/{EXM_YEAR}/",
                Font = new Font("Segoe UI", 8f),
                Location = new Point(cardPad, 189),
                Width = innerW,
                BorderStyle = BorderStyle.FixedSingle
            };

            lblStartRefNo = new Label
            {
                Text = "Start Ref #:",
                Font = new Font("Segoe UI", 8.5f),
                Location = new Point(cardPad, 214),
                AutoSize = true
            };

            txtStartRefNo = new TextBox
            {
                Text = "00037420",
                Font = new Font("Segoe UI", 8.5f),
                Location = new Point(cardPad, 231),
                Width = 100,
                BorderStyle = BorderStyle.FixedSingle
            };

            lblStartBookSl = new Label
            {
                Text = "Start Book Sl #:",
                Font = new Font("Segoe UI", 8.5f),
                Location = new Point(cardPad + 112, 214),
                AutoSize = true
            };

            txtStartBookSl = new TextBox
            {
                Text = "0000167",
                Font = new Font("Segoe UI", 8.5f),
                Location = new Point(cardPad + 112, 231),
                Width = innerW - 112,
                BorderStyle = BorderStyle.FixedSingle
            };

            lblDbChip = new Label
            {
                Text = "Database: m_ed • Configured",
                Font = new Font("Segoe UI", 8f, FontStyle.Bold),
                BackColor = Color.FromArgb(238, 242, 255),
                ForeColor = VectorGraphics.ColorPrimary,
                Padding = new Padding(6, 4, 6, 4),
                Location = new Point(cardPad, 268),
                AutoSize = true
            };

            btnTestDbQuick = new Button
            {
                Text = "⚡ Test Connection",
                Font = new Font("Segoe UI", 8f, FontStyle.Bold),
                Location = new Point(cardPad + innerW - 125, 266),
                Size = new Size(125, 26)
            };
            VectorGraphics.ApplyOutlinedButton(btnTestDbQuick, VectorGraphics.ColorSecondary, VectorGraphics.ColorSecondary, 6);
            btnTestDbQuick.Click += (s, e) =>
            {
                string err;
                bool ok = Database.TestConnection(txtConnStr.Text, out err);
                if (ok)
                {
                    lblDbChip.Text = "Database: m_ed • Connected";
                    lblDbChip.BackColor = Color.FromArgb(220, 252, 231);
                    lblDbChip.ForeColor = VectorGraphics.ColorSuccess;
                    MessageBox.Show("✔ Database connection verified successfully!\n\nTarget:\n" + txtConnStr.Text, "Database Connected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    lblDbChip.Text = "Database: Connection Error";
                    lblDbChip.BackColor = Color.FromArgb(254, 226, 226);
                    lblDbChip.ForeColor = VectorGraphics.ColorError;
                    MessageBox.Show("✖ Database connection failed:\n\n" + err + "\n\nConnection String:\n" + txtConnStr.Text, "Connection Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            // Hidden background store for connection string
            lblConnStr = new Label { Visible = false, Text = "Database (m_ed):" };
            txtConnStr = new TextBox { Visible = false, Text = TranscriptGenerator.GetMedConnectionString() };

            pnlCardCourse.Controls.Add(lblCardCourseTitle);
            pnlCardCourse.Controls.Add(lblCoursePreset);
            pnlCardCourse.Controls.Add(cmbCoursePreset);
            pnlCardCourse.Controls.Add(btnQuickCourseEdit);
            pnlCardCourse.Controls.Add(lblStoredProc);
            pnlCardCourse.Controls.Add(cmbStoredProc);
            pnlCardCourse.Controls.Add(lblPageSize);
            pnlCardCourse.Controls.Add(cmbPageSize);
            pnlCardCourse.Controls.Add(lblIssueDate);
            pnlCardCourse.Controls.Add(txtIssueDate);
            pnlCardCourse.Controls.Add(lblRefPrefix);
            pnlCardCourse.Controls.Add(txtRefPrefix);
            pnlCardCourse.Controls.Add(lblStartRefNo);
            pnlCardCourse.Controls.Add(txtStartRefNo);
            pnlCardCourse.Controls.Add(lblStartBookSl);
            pnlCardCourse.Controls.Add(txtStartBookSl);
            pnlCardCourse.Controls.Add(lblDbChip);
            pnlCardCourse.Controls.Add(btnTestDbQuick);
            pnlCardCourse.Controls.Add(lblConnStr);
            pnlCardCourse.Controls.Add(txtConnStr);

            // ------------------------------------------
            // CARD 3: Document Generation
            // ------------------------------------------
            pnlCardActions = new Panel
            {
                Width = cardW,
                Height = 184,
                Location = new Point(12, 560),
                BackColor = Color.White
            };
            pnlCardActions.Paint += (s, e) => VectorGraphics.PaintCard(e.Graphics, new Rectangle(0, 0, pnlCardActions.Width, pnlCardActions.Height), 8);

            lblCardActionsTitle = new Label
            {
                Text = "DOCUMENT GENERATION",
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                ForeColor = VectorGraphics.ColorPrimary,
                Location = new Point(cardPad, 12),
                AutoSize = true
            };

            btnGenerateTranscript = new Button
            {
                Text = "📜 Generate Academic Transcript",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Location = new Point(cardPad, 34),
                Size = new Size(innerW - 82, 40)
            };
            VectorGraphics.ApplyModernButton(
                btnGenerateTranscript, 
                VectorGraphics.ColorPrimary, 
                VectorGraphics.ColorPrimaryDark, 
                VectorGraphics.ColorPrimaryHover, 
                Color.White, 
                8);
            btnGenerateTranscript.Click += (s, e) => StartGeneration("TRANSCRIPT");

            btnGenerateCertificate = new Button
            {
                Text = "🎓 Generate Provisional Certificate",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Location = new Point(cardPad, 80),
                Size = new Size(innerW - 82, 40)
            };
            VectorGraphics.ApplyModernButton(
                btnGenerateCertificate, 
                VectorGraphics.ColorSecondary, 
                VectorGraphics.ColorSecondaryDark, 
                VectorGraphics.ColorSecondaryHover, 
                Color.White, 
                8);
            btnGenerateCertificate.Click += (s, e) => StartGeneration("CERTIFICATE");

            btnStopGen = new Button
            {
                Text = "⏹ STOP\nPROCESS",
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                Location = new Point(cardPad + innerW - 76, 34),
                Size = new Size(76, 86),
                Cursor = Cursors.Hand
            };
            VectorGraphics.ApplyModernButton(
                btnStopGen, 
                Color.FromArgb(254, 242, 242), 
                VectorGraphics.ColorError, 
                Color.FromArgb(254, 226, 226), 
                VectorGraphics.ColorError, 
                8);
            btnStopGen.Click += (s, e) => RequestStopProcess();

            chkDemoWatermark = new CheckBox
            {
                Text = "🛡️ DEMO (SAMPLE Watermark)",
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(194, 65, 12),
                Checked = true, // Default checked as requested
                Location = new Point(cardPad, 128),
                Width = innerW,
                Height = 24,
                Cursor = Cursors.Hand
            };
            chkDemoWatermark.CheckedChanged += (s, e) =>
            {
                if (chkDemoWatermark.Checked)
                {
                    chkDemoWatermark.ForeColor = Color.FromArgb(194, 65, 12);
                    lblDemoHelp.Text = "• DEMO mode: document includes prominent 'SAMPLE' watermark.";
                    lblDemoHelp.ForeColor = Color.FromArgb(194, 65, 12);
                }
                else
                {
                    chkDemoWatermark.ForeColor = VectorGraphics.ColorSuccess;
                    lblDemoHelp.Text = "• Official mode: printing clean production document (no watermark).";
                    lblDemoHelp.ForeColor = VectorGraphics.ColorSuccess;
                }
            };

            lblDemoHelp = new Label
            {
                Text = "• Default enabled. Uncheck (Editor/Admin only) for official print.",
                Font = new Font("Segoe UI", 7.2f, FontStyle.Italic),
                ForeColor = Color.FromArgb(194, 65, 12),
                Location = new Point(cardPad + 20, 154),
                Width = innerW - 20,
                Height = 22
            };

            pnlCardActions.Controls.Add(lblCardActionsTitle);
            pnlCardActions.Controls.Add(btnGenerateTranscript);
            pnlCardActions.Controls.Add(btnGenerateCertificate);
            pnlCardActions.Controls.Add(btnStopGen);
            pnlCardActions.Controls.Add(chkDemoWatermark);
            pnlCardActions.Controls.Add(lblDemoHelp);

            // ------------------------------------------
            // CARD 4: Administration & Tools
            // ------------------------------------------
            pnlCardAdminTools = new Panel
            {
                Width = cardW,
                Height = 84,
                Location = new Point(12, 756),
                BackColor = Color.White
            };
            pnlCardAdminTools.Paint += (s, e) => VectorGraphics.PaintCard(e.Graphics, new Rectangle(0, 0, pnlCardAdminTools.Width, pnlCardAdminTools.Height), 8);

            lblCardAdminTitle = new Label
            {
                Text = "ADMINISTRATION & AUDIT",
                UseMnemonic = false,
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                ForeColor = VectorGraphics.ColorTextSecondary,
                Location = new Point(cardPad, 12),
                AutoSize = true
            };

            int colW = (innerW - 8) / 3;

            btnCourseSettings = new Button
            {
                Text = "⚙ Design",
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                Location = new Point(cardPad, 34),
                Size = new Size(colW, 34)
            };
            VectorGraphics.ApplyOutlinedButton(btnCourseSettings, VectorGraphics.ColorPrimary, VectorGraphics.ColorPrimary, 6);
            btnCourseSettings.Click += btnCourseSettings_Click;

            btnAuditLog = new Button
            {
                Text = "📊 Audit",
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                Location = new Point(cardPad + colW + 4, 34),
                Size = new Size(colW, 34)
            };
            VectorGraphics.ApplyOutlinedButton(btnAuditLog, VectorGraphics.ColorSecondary, VectorGraphics.ColorSecondary, 6);
            btnAuditLog.Click += btnAuditLog_Click;

            btnAdminPanel = new Button
            {
                Text = "👑 Admin",
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                Location = new Point(cardPad + (colW * 2) + 8, 34),
                Size = new Size(innerW - (colW * 2) - 8, 34)
            };
            VectorGraphics.ApplyOutlinedButton(btnAdminPanel, VectorGraphics.ColorPrimaryDark, VectorGraphics.ColorPrimaryDark, 6);
            btnAdminPanel.Click += (s, e) =>
            {
                using (AdminDashboardForm dlg = new AdminDashboardForm())
                {
                    dlg.ShowDialog(this);
                    ApplyUserPermissions();
                }
            };

            pnlCardAdminTools.Controls.Add(lblCardAdminTitle);
            pnlCardAdminTools.Controls.Add(btnCourseSettings);
            pnlCardAdminTools.Controls.Add(btnAuditLog);
            pnlCardAdminTools.Controls.Add(btnAdminPanel);

            // ------------------------------------------
            // CARD 5: System Status & Diagnostic Indicator
            // ------------------------------------------
            pnlCardStatus = new Panel
            {
                Width = cardW,
                Height = 94,
                Location = new Point(12, 852),
                BackColor = Color.White
            };
            pnlCardStatus.Paint += (s, e) => VectorGraphics.PaintCard(e.Graphics, new Rectangle(0, 0, pnlCardStatus.Width, pnlCardStatus.Height), 8);

            lblStatusPill = new Label
            {
                Text = "● Ready",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = VectorGraphics.ColorSuccess,
                Location = new Point(cardPad, 10),
                AutoSize = true
            };

            btnCopyError = new Button
            {
                Text = "📋 Copy Error",
                Font = new Font("Segoe UI", 7.5f, FontStyle.Bold),
                Location = new Point(cardPad + innerW - 90, 8),
                Size = new Size(90, 22),
                Visible = false
            };
            VectorGraphics.ApplyOutlinedButton(btnCopyError, VectorGraphics.ColorError, VectorGraphics.ColorError, 4);
            btnCopyError.Click += (s, e) =>
            {
                try
                {
                    Clipboard.SetText(lblStatusDetails.Text);
                    MessageBox.Show("Diagnostic details copied to clipboard.", "Copied", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch { }
            };

            progressBar = new ProgressBar
            {
                Location = new Point(cardPad, 30),
                Size = new Size(innerW, 12),
                Style = ProgressBarStyle.Marquee,
                Visible = false
            };

            lblStatusDetails = new Label
            {
                Text = "Ready to generate transcripts or certificates.",
                Font = new Font("Segoe UI", 8.5f),
                ForeColor = VectorGraphics.ColorTextSecondary,
                Location = new Point(cardPad, 46),
                Width = innerW,
                Height = 36
            };

            pnlCardStatus.Controls.Add(lblStatusPill);
            pnlCardStatus.Controls.Add(btnCopyError);
            pnlCardStatus.Controls.Add(progressBar);
            pnlCardStatus.Controls.Add(lblStatusDetails);

            // Assemble left sidebar
            pnlLeftScroll.Controls.Add(pnlCardParams);
            pnlLeftScroll.Controls.Add(pnlCardCourse);
            pnlLeftScroll.Controls.Add(pnlCardActions);
            pnlLeftScroll.Controls.Add(pnlCardAdminTools);
            pnlLeftScroll.Controls.Add(pnlCardStatus);
            pnlSidebar.Controls.Add(pnlLeftScroll);

            // ==========================================
            // 3. RIGHT PREVIEW PANEL (Dock = Fill)
            // ==========================================
            pnlMainPreview = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = VectorGraphics.ColorAppBg
            };

            // Top Toolbar inside Preview Panel
            pnlPreviewToolbar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 52,
                BackColor = Color.White,
                Padding = new Padding(14, 8, 14, 8)
            };
            pnlPreviewToolbar.Paint += (s, e) =>
            {
                using (Pen p = new Pen(VectorGraphics.ColorBorderDivider, 1))
                {
                    e.Graphics.DrawLine(p, 0, pnlPreviewToolbar.Height - 1, pnlPreviewToolbar.Width, pnlPreviewToolbar.Height - 1);
                }
            };

            FlowLayoutPanel pnlToolbarLeft = new FlowLayoutPanel
            {
                Dock = DockStyle.Left,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Padding = new Padding(14, 12, 0, 0),
                BackColor = Color.Transparent
            };

            lblDocBadge = new Label
            {
                Text = "DOCUMENT PREVIEW",
                Font = new Font("Segoe UI", 8f, FontStyle.Bold),
                BackColor = Color.FromArgb(224, 242, 254),
                ForeColor = Color.FromArgb(3, 105, 161),
                Padding = new Padding(8, 4, 8, 4),
                Margin = new Padding(0, 0, 10, 0),
                AutoSize = true
            };

            lblPreviewSummary = new Label
            {
                Text = "No document generated yet. Use the controls on the left to generate.",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = VectorGraphics.ColorTextPrimary,
                Margin = new Padding(0, 4, 0, 0),
                AutoSize = true,
                AutoEllipsis = true,
                MaximumSize = new Size(300, 24)
            };

            pnlToolbarLeft.Controls.Add(lblDocBadge);
            pnlToolbarLeft.Controls.Add(lblPreviewSummary);

            FlowLayoutPanel pnlToolbarRight = new FlowLayoutPanel
            {
                Dock = DockStyle.Right,
                AutoSize = true,
                FlowDirection = FlowDirection.RightToLeft,
                WrapContents = false,
                Padding = new Padding(0, 7, 8, 0),
                BackColor = Color.Transparent
            };

            btnPrint = new Button
            {
                Text = "🖨️ Print",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                Size = new Size(125, 36),
                Margin = new Padding(4, 0, 4, 0),
                Enabled = false
            };
            VectorGraphics.ApplyModernButton(
                btnPrint, 
                VectorGraphics.ColorSecondary, 
                VectorGraphics.ColorSecondaryDark, 
                VectorGraphics.ColorSecondaryHover, 
                Color.White, 
                6);
            btnPrint.Click += btnPrint_Click;

            btnPrintPreview = new Button
            {
                Text = "👁️ Print Preview",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                Size = new Size(135, 36),
                Margin = new Padding(4, 0, 4, 0),
                Enabled = false
            };
            VectorGraphics.ApplyOutlinedButton(
                btnPrintPreview, 
                VectorGraphics.ColorPrimary, 
                VectorGraphics.ColorPrimaryDark, 
                6);
            btnPrintPreview.Click += btnPrintPreview_Click;

            btnOpenBrowser = new Button
            {
                Text = "🌐 Browser",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                Size = new Size(115, 36),
                Margin = new Padding(4, 0, 4, 0),
                Enabled = false
            };
            VectorGraphics.ApplyOutlinedButton(
                btnOpenBrowser, 
                VectorGraphics.ColorPrimary, 
                VectorGraphics.ColorPrimary, 
                6);
            btnOpenBrowser.Click += btnOpenBrowser_Click;

            btnOpenFolder = new Button
            {
                Text = "📁 Reports",
                Font = new Font("Segoe UI", 9f),
                Size = new Size(95, 36),
                Margin = new Padding(4, 0, 4, 0)
            };
            VectorGraphics.ApplyOutlinedButton(
                btnOpenFolder, 
                VectorGraphics.ColorTextSecondary, 
                VectorGraphics.ColorTextPrimary, 
                6);
            btnOpenFolder.Click += btnOpenFolder_Click;

            pnlToolbarRight.Controls.Add(btnOpenFolder);
            pnlToolbarRight.Controls.Add(btnOpenBrowser);
            pnlToolbarRight.Controls.Add(btnPrintPreview);
            pnlToolbarRight.Controls.Add(btnPrint);

            pnlPreviewToolbar.Controls.Add(pnlToolbarRight);
            pnlPreviewToolbar.Controls.Add(pnlToolbarLeft);

            pnlBrowserWrap = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = VectorGraphics.ColorAppBg,
                Padding = new Padding(6)
            };

            Panel pnlBrowserCard = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(1)
            };
            pnlBrowserCard.Paint += (s, e) =>
            {
                using (Pen p = new Pen(VectorGraphics.ColorBorderDivider, 1f))
                {
                    e.Graphics.DrawRectangle(p, 0, 0, pnlBrowserCard.Width - 1, pnlBrowserCard.Height - 1);
                }
            };

            webBrowser = new WebBrowser
            {
                Dock = DockStyle.Fill,
                ScriptErrorsSuppressed = true
            };

            pnlBrowserCard.Controls.Add(webBrowser);
            pnlBrowserWrap.Controls.Add(pnlBrowserCard);

            pnlMainPreview.Controls.Add(pnlBrowserWrap);
            pnlMainPreview.Controls.Add(pnlPreviewToolbar);

            // ==========================================
            // ASSEMBLE CONTROLS IN PROPER DOCK ORDER
            // ==========================================
            this.Controls.Add(pnlMainPreview);
            this.Controls.Add(sideSplitter);
            this.Controls.Add(pnlSidebar);
            this.Controls.Add(pnlHeaderBanner);

            // BackgroundWorker
            bgWorker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };
            bgWorker.DoWork += BgWorker_DoWork;
            bgWorker.RunWorkerCompleted += BgWorker_RunWorkerCompleted;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            try
            {
                UserPermissionManager.EnsureTranscriptUserTableAndSeed(txtConnStr.Text);
            }
            catch { }

            if (!UserSession.IsAuthenticated)
            {
                using (OperatorLoginForm dlg = new OperatorLoginForm(txtConnStr.Text))
                {
                    if (dlg.ShowDialog(this) != DialogResult.OK)
                    {
                        this.BeginInvoke(new Action(this.Close));
                        return;
                    }
                }
                using (UserCredentialViewDialog credDlg = new UserCredentialViewDialog(UserSession.CurrentUser, txtConnStr.Text))
                {
                    if (credDlg.ShowDialog(this) != DialogResult.OK)
                    {
                        this.BeginInvoke(new Action(this.Close));
                        return;
                    }
                }
            }
            ApplyUserPermissions();
        }

        private void LoadCourseProfiles()
        {
            loadedDesigns = CourseDesignRepository.LoadDesigns();
            cmbCoursePreset.Items.Clear();
            foreach (var d in loadedDesigns)
            {
                cmbCoursePreset.Items.Add(d);
            }
            if (cmbCoursePreset.Items.Count > 0)
            {
                cmbCoursePreset.SelectedIndex = 0;
            }
        }

        private void CmbCoursePreset_SelectedIndexChanged(object sender, EventArgs e)
        {
            CourseDesignConfig d = cmbCoursePreset.SelectedItem as CourseDesignConfig;
            if (d == null) return;
            currentConfig = d;

            cmbPageSize.SelectedItem = string.Equals(d.PageSize, "Legal", StringComparison.OrdinalIgnoreCase) ? "Legal" : "A4";
            if (!string.IsNullOrEmpty(d.RefPrefix)) txtRefPrefix.Text = d.RefPrefix;
            txtConnStr.Text = d.GetConnectionString();
            lblConnStr.Text = string.Format("Database ({0}):", !string.IsNullOrEmpty(d.DbName) ? d.DbName : "m_ed");

            if (cmbStoredProc != null)
            {
                cmbStoredProc.Text = !string.IsNullOrEmpty(d.StoredProcedureName) ? d.StoredProcedureName : "dbo.BBAMCPCTRANS_lot";
            }

            if (lblDbChip != null)
            {
                lblDbChip.Text = string.Format("Database: {0} • Configured", !string.IsNullOrEmpty(d.DbName) ? d.DbName : "m_ed");
            }
        }

        private struct WorkerArgs
        {
            public string Mode; // "TRANSCRIPT" or "CERTIFICATE"
            public string ConnStr;
            public string ExmYear;
            public string RegNo;
            public bool FastMode;
            public string ProcName;
            public TranscriptPrintOptions PrintOptions;
        }

        private struct WorkerResult
        {
            public bool Success;
            public string ErrorMessage;
            public string HtmlFilePath;
            public string Mode;
            public int ExamineeCount;
            public bool Is8Semester;
            public int NonCreditCourseCount;
        }

        private void StartGeneration(string mode)
        {
            string year = txtExmYear.Text.Trim();
            if (string.IsNullOrEmpty(year) || year.Length != 4)
            {
                MessageBox.Show("Please enter a valid 4-digit Exam Year (e.g. 2024).", "Invalid Parameter", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtExmYear.Focus();
                return;
            }

            string reg = txtRegNo.Text.Trim();
            string conn = txtConnStr.Text.Trim();

            // 0. Ensure transcript_user table and database credentials verification
            try
            {
                UserPermissionManager.EnsureTranscriptUserTableAndSeed(conn);
            }
            catch { }

            // Operator authentication check
            if (!UserSession.CanPrint)
            {
                using (OperatorLoginForm loginDlg = new OperatorLoginForm(conn))
                {
                    if (loginDlg.ShowDialog(this) != DialogResult.OK)
                    {
                        lblStatusPill.Text = "🔒 Login Required";
                        lblStatusPill.ForeColor = VectorGraphics.ColorWarning;
                        lblStatusDetails.Text = "Document generation requires authorized operator login credentials.";
                        return;
                    }
                }
                ApplyUserPermissions();
                if (!UserSession.CanPrint)
                {
                    MessageBox.Show("Logged in user does not have Print authorization to generate official transcripts or certificates.", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            // Populate options from selected course profile and UI overrides
            CourseDesignConfig cfg = cmbCoursePreset.SelectedItem as CourseDesignConfig;

            // Pre-flight check: database catalog verification
            if (cfg != null && !string.IsNullOrEmpty(cfg.DbName))
            {
                if (!string.IsNullOrEmpty(conn) &&
                    conn.IndexOf("Initial Catalog=" + cfg.DbName, StringComparison.OrdinalIgnoreCase) < 0 &&
                    conn.IndexOf("Database=" + cfg.DbName, StringComparison.OrdinalIgnoreCase) < 0)
                {
                    DialogResult dlgRes = MessageBox.Show(
                        string.Format("Notice: The selected course profile '{0}' expects database catalog '{1}', but the active connection string points to another catalog.\n\nDo you wish to proceed?", 
                            cfg.DisplayName, cfg.DbName),
                        "Database Catalog Notice",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);

                    if (dlgRes != DialogResult.Yes)
                    {
                        return;
                    }
                }
            }

            string proc = cmbStoredProc != null && !string.IsNullOrEmpty(cmbStoredProc.Text) ? cmbStoredProc.Text.Trim() : "dbo.BBAMCPCTRANS_lot";

            TranscriptPrintOptions options = TranscriptPrintOptions.FromConfig(cfg);
            options.PageSize = cmbPageSize.SelectedItem != null ? cmbPageSize.SelectedItem.ToString() : "A4";
            options.RefPrefix = txtRefPrefix.Text.Trim();
            options.StartRefNo = txtStartRefNo.Text.Trim();
            options.StartBookSl = txtStartBookSl.Text.Trim();
            options.IssueDate = txtIssueDate.Text.Trim();
            options.StoredProcedureName = proc;
            options.IsDemo = (chkDemoWatermark != null && chkDemoWatermark.Checked);

            SetWorkingState(true);
            currentDocType = mode;
            lblStatusPill.Text = "⏳ Fetching Data...";
            lblStatusPill.ForeColor = VectorGraphics.ColorPrimary;
            lblStatusDetails.Text = string.IsNullOrEmpty(reg)
                ? string.Format("Querying LOT {0} using proc '{1}' for Year {2}...", mode.ToLower(), proc, year)
                : string.Format("Querying Single {0} for Reg No '{1}', Year {2} using '{3}'...", mode.ToLower(), reg, year, proc);

            if (btnCopyError != null) btnCopyError.Visible = false;

            WorkerArgs args = new WorkerArgs
            {
                Mode = mode,
                ConnStr = conn,
                ExmYear = year,
                RegNo = reg,
                FastMode = chkUseFastTable.Checked,
                ProcName = proc,
                PrintOptions = options
            };

            bgWorker.RunWorkerAsync(args);
        }

        private void BgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            WorkerArgs args = (WorkerArgs)e.Argument;
            WorkerResult res = new WorkerResult();
            res.Mode = args.Mode;

            try
            {
                // 1. Load course catalog
                Dictionary<string, CourseInfo> courses = TranscriptGenerator.LoadCourses(args.ConnStr);

                // 2. Fetch examinee data
                DataTable dt = TranscriptGenerator.FetchTranscriptData(
                    args.ConnStr, 
                    args.ExmYear, 
                    args.RegNo, 
                    args.FastMode, 
                    args.ProcName, 
                    cmd => { activeQueryCmd = cmd; });

                if (dt == null || dt.Rows.Count == 0)
                {
                    res.Success = false;
                    res.ErrorMessage = string.IsNullOrEmpty(args.RegNo)
                        ? string.Format("No examinees found for Exam Year '{0}'. Verify that data exists in database or procedure '{1}'.", args.ExmYear, args.ProcName)
                        : string.Format("No record found for Reg No '{0}' and Exam Year '{1}'.", args.RegNo, args.ExmYear);
                    e.Result = res;
                    return;
                }

                // 3. Generate HTML based on Mode
                string htmlPath;
                int nonCreditCount = 0;
                bool is8Sem = false;
                if (string.Equals(args.Mode, "CERTIFICATE", StringComparison.OrdinalIgnoreCase))
                {
                    TranscriptGenerator.GenerateCertificateHtml(dt, args.PrintOptions, out htmlPath);
                }
                else
                {
                    TranscriptGenerator.GenerateTranscriptHtml(dt, courses, args.PrintOptions, out htmlPath, out nonCreditCount);
                    is8Sem = string.Equals(args.PrintOptions.PageSize, "Legal", StringComparison.OrdinalIgnoreCase);
                }

                res.Success = true;
                res.HtmlFilePath = htmlPath;
                res.ExamineeCount = dt.Rows.Count;
                res.Is8Semester = is8Sem;
                res.NonCreditCourseCount = nonCreditCount;
            }
            catch (Exception ex)
            {
                if (bgWorker.CancellationPending || (ex is SqlException && ((SqlException)ex).Number == 0))
                {
                    res.Success = false;
                    res.ErrorMessage = "Operation was stopped by the operator.";
                }
                else
                {
                    res.Success = false;
                    res.ErrorMessage = ex.Message;
                }
            }

            e.Result = res;
        }

        private void BgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            activeQueryCmd = null;
            SetWorkingState(false);

            if (e.Result is WorkerResult)
            {
                WorkerResult res = (WorkerResult)e.Result;
                if (res.Success)
                {
                    currentGeneratedHtmlPath = res.HtmlFilePath;
                    lastGeneratedCount = res.ExamineeCount;

                    bool isCert = string.Equals(res.Mode, "CERTIFICATE", StringComparison.OrdinalIgnoreCase);
                    string docTitle = isCert ? "Provisional Certificate" : "Academic Transcript";

                    if (!isCert && res.Is8Semester)
                    {
                        cmbPageSize.SelectedItem = "Legal";
                    }

                    lblStatusPill.Text = "✅ Complete";
                    lblStatusPill.ForeColor = VectorGraphics.ColorSuccess;

                    string details = string.Format("Generated {0} {1}(s).", res.ExamineeCount, docTitle);
                    if (!isCert)
                    {
                        if (res.Is8Semester) details += " [8-Semester Legal • Single Page Fit]";
                        if (res.NonCreditCourseCount > 0) details += string.Format(" • {0} non-credit course(s) separated under main grades", res.NonCreditCourseCount);
                    }
                    details += " Ready to preview and print.";
                    lblStatusDetails.Text = details;

                    if (btnCopyError != null) btnCopyError.Visible = false;

                    lblDocBadge.Text = isCert ? "PROVISIONAL CERTIFICATE" : "ACADEMIC TRANSCRIPT";
                    lblDocBadge.BackColor = isCert ? Color.FromArgb(255, 247, 237) : Color.FromArgb(236, 253, 245);
                    lblDocBadge.ForeColor = isCert ? VectorGraphics.ColorSecondaryDark : VectorGraphics.ColorPrimaryDark;

                    lblPreviewSummary.Text = string.Format("{0} examinee{1} | {2}", 
                        res.ExamineeCount, res.ExamineeCount > 1 ? "s" : "", Path.GetFileName(res.HtmlFilePath));

                    btnPrint.Enabled = UserSession.CanPrint;
                    btnPrint.Visible = UserSession.CanPrint;
                    btnPrintPreview.Enabled = true;
                    btnPrintPreview.Visible = true;
                    btnOpenBrowser.Enabled = true;

                    // Display in WebBrowser
                    try
                    {
                        webBrowser.Navigate(res.HtmlFilePath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error rendering document preview:\n" + ex.Message, "Preview Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    bool wasStopped = res.ErrorMessage != null && res.ErrorMessage.IndexOf("stopped", StringComparison.OrdinalIgnoreCase) >= 0;
                    lblStatusPill.Text = wasStopped ? "⏹ Stopped" : "❌ Generation Failed";
                    lblStatusPill.ForeColor = wasStopped ? VectorGraphics.ColorWarning : VectorGraphics.ColorError;
                    lblStatusDetails.Text = res.ErrorMessage;
                    if (btnCopyError != null) btnCopyError.Visible = !wasStopped;
                    if (!wasStopped)
                    {
                        MessageBox.Show(res.ErrorMessage, "Generation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void SetWorkingState(bool isWorking)
        {
            btnGenerateTranscript.Enabled = !isWorking && UserSession.CanPrint;
            btnGenerateCertificate.Enabled = !isWorking && UserSession.CanPrint;
            btnSearchCandidate.Enabled = !isWorking;
            if (btnStopSearch != null) btnStopSearch.Enabled = isWorking;
            if (btnStopGen != null) btnStopGen.Enabled = isWorking;
            if (cmbStoredProc != null) cmbStoredProc.Enabled = !isWorking;
            btnCourseSettings.Enabled = !isWorking;
            btnAuditLog.Enabled = !isWorking;
            txtExmYear.Enabled = !isWorking;
            txtRegNo.Enabled = !isWorking;
            cmbCoursePreset.Enabled = !isWorking;
            chkUseFastTable.Enabled = !isWorking;
            if (btnPrintPreview != null) btnPrintPreview.Enabled = !isWorking && !string.IsNullOrEmpty(currentGeneratedHtmlPath);

            progressBar.Visible = isWorking;
        }

        private void RequestStopProcess()
        {
            try
            {
                if (activeQueryCmd != null)
                {
                    try { activeQueryCmd.Cancel(); } catch { }
                }
                if (bgWorker != null && bgWorker.IsBusy)
                {
                    bgWorker.CancelAsync();
                }
                lblStatusPill.Text = "⏹ Stopped";
                lblStatusPill.ForeColor = VectorGraphics.ColorWarning;
                lblStatusDetails.Text = "Current operation was stopped by the operator.";
                SetWorkingState(false);
            }
            catch (Exception ex)
            {
                lblStatusDetails.Text = "Stop notice: " + ex.Message;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(currentGeneratedHtmlPath) || !File.Exists(currentGeneratedHtmlPath))
            {
                MessageBox.Show("Please generate a transcript or certificate first.", "No Document Loaded", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                TranscriptGenerator.ClearBrowserPrintHeadersAndFooters();
                webBrowser.ShowPrintDialog();
            }
            catch
            {
                try
                {
                    Process.Start(new ProcessStartInfo(currentGeneratedHtmlPath) { UseShellExecute = true });
                }
                catch { }
            }
        }

        private void btnPrintPreview_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(currentGeneratedHtmlPath) || !File.Exists(currentGeneratedHtmlPath))
            {
                MessageBox.Show("Please generate a transcript or certificate first.", "No Document Loaded", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                TranscriptGenerator.ClearBrowserPrintHeadersAndFooters();
                webBrowser.ShowPrintPreviewDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not open print preview:\n" + ex.Message, "Print Preview Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnOpenBrowser_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(currentGeneratedHtmlPath) && File.Exists(currentGeneratedHtmlPath))
            {
                try
                {
                    Process.Start(new ProcessStartInfo(currentGeneratedHtmlPath) { UseShellExecute = true });
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not open external browser:\n" + ex.Message, "Browser Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnOpenFolder_Click(object sender, EventArgs e)
        {
            try
            {
                string reportsDir = TranscriptGenerator.GetSafeReportsDirectory();
                Process.Start(new ProcessStartInfo("explorer.exe", reportsDir));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening reports folder:\n" + ex.Message);
            }
        }

        private void btnCourseSettings_Click(object sender, EventArgs e)
        {
            CourseDesignConfig sel = cmbCoursePreset.SelectedItem as CourseDesignConfig;
            using (CourseDesignDialog dlg = new CourseDesignDialog(sel))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    LoadCourseProfiles();
                    if (dlg.SelectedDesign != null)
                    {
                        for (int i = 0; i < cmbCoursePreset.Items.Count; i++)
                        {
                            CourseDesignConfig d = cmbCoursePreset.Items[i] as CourseDesignConfig;
                            if (d != null && string.Equals(d.CourseKey, dlg.SelectedDesign.CourseKey, StringComparison.OrdinalIgnoreCase))
                            {
                                cmbCoursePreset.SelectedIndex = i;
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void btnAuditLog_Click(object sender, EventArgs e)
        {
            try
            {
                string auditHtmlPath = PrintAuditManager.GenerateAuditReportHtml();
                currentGeneratedHtmlPath = auditHtmlPath;
                lblDocBadge.Text = "AUDIT LOG";
                lblDocBadge.BackColor = Color.FromArgb(204, 251, 241);
                lblDocBadge.ForeColor = Color.FromArgb(17, 94, 89);
                lblPreviewSummary.Text = "Print & Lot Audit Report (All Operations)";
                btnPrint.Enabled = UserSession.CanPrint;
                btnOpenBrowser.Enabled = true;
                webBrowser.Navigate(auditHtmlPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating audit report:\n" + ex.Message, "Audit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Enforce Role-Based Access Control (RBAC) across the form controls.
        /// </summary>
        public void ApplyUserPermissions()
        {
            try
            {
                // CanPrint controls
                bool canPrint = UserSession.CanPrint;
                btnPrint.Visible = canPrint;
                if (btnPrintPreview != null) btnPrintPreview.Visible = true;
                btnGenerateTranscript.Enabled = canPrint || UserSession.CanView;
                btnGenerateCertificate.Enabled = canPrint || UserSession.CanView;

                // CanEdit controls (Course design presets & DB credentials)
                bool canEdit = UserSession.CanEdit;
                btnCourseSettings.Enabled = canEdit;
                btnQuickCourseEdit.Enabled = canEdit;
                txtConnStr.ReadOnly = !canEdit;

                // DEMO / Official Watermark control (Only Editor or Admin can uncheck)
                bool canEditOrAdmin = canEdit || UserSession.IsAdmin;
                if (chkDemoWatermark != null)
                {
                    if (!canEditOrAdmin)
                    {
                        // View-only operators are locked to DEMO mode with SAMPLE watermark
                        chkDemoWatermark.Checked = true;
                        chkDemoWatermark.Enabled = false;
                        if (lblDemoHelp != null)
                        {
                            lblDemoHelp.Text = "• View-only operator: locked to DEMO mode with SAMPLE watermark.";
                            lblDemoHelp.ForeColor = Color.FromArgb(194, 65, 12);
                        }
                    }
                    else
                    {
                        chkDemoWatermark.Enabled = true;
                        if (lblDemoHelp != null)
                        {
                            lblDemoHelp.Text = chkDemoWatermark.Checked 
                                ? "• DEMO mode: document includes prominent 'SAMPLE' watermark." 
                                : "• Official mode: printing clean production document (no watermark).";
                            lblDemoHelp.ForeColor = chkDemoWatermark.Checked ? Color.FromArgb(194, 65, 12) : VectorGraphics.ColorSuccess;
                        }
                    }
                }

                // Admin privileges
                bool isAdmin = UserSession.IsAdmin;
                btnAdminPanel.Visible = true; // allow launching to view or login as another user
                btnHeaderAdmin.Visible = true;

                // Header badge update
                if (lblUserRoleBadge != null)
                {
                    lblUserRoleBadge.Text = "👤 " + (UserSession.CurrentUser != null ? UserSession.CurrentUser.Role : "Admin");
                }

                // Title bar update with active user and permissions
                string userTitle = UserSession.CurrentUser != null 
                    ? string.Format("{0} ({1} - {2})", UserSession.CurrentUser.FullName, UserSession.CurrentUser.Role, UserSession.CurrentUser.GetPermissionsSummary())
                    : "Administrator";
                this.Text = "National University - Academic Transcript & Certificate System [User: " + userTitle + "]";
            }
            catch { }
        }
    }

    /// <summary>
    /// Operator Login Dialog for authenticating against dbo.transcript_user on the active database.
    /// </summary>
    public class OperatorLoginForm : Form
    {
        private string _connStr;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Label lblError;
        private Button btnLogin;
        private Button btnCancel;

        public OperatorLoginForm(string connStr)
        {
            _connStr = connStr;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Operator Login - National University";
            this.Size = new Size(460, 330);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;
            this.Font = new Font("Segoe UI", 9.5f);

            Panel pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 52,
                BackColor = VectorGraphics.ColorPrimaryDark
            };
            Label lblTitle = new Label
            {
                Text = "Operator Authentication",
                Font = new Font("Segoe UI", 10.5f, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(16, 14),
                AutoSize = true
            };
            pnlHeader.Controls.Add(lblTitle);

            Label lblSub = new Label
            {
                Text = "Authenticate against dbo.transcript_user to proceed with generation.",
                Font = new Font("Segoe UI", 8.5f),
                ForeColor = VectorGraphics.ColorTextSecondary,
                Location = new Point(20, 64),
                Size = new Size(400, 20)
            };

            Label lblUser = new Label
            {
                Text = "Username:",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = VectorGraphics.ColorTextPrimary,
                Location = new Point(20, 92),
                AutoSize = true
            };
            txtUsername = new TextBox
            {
                Text = "operator1",
                Font = new Font("Segoe UI", 9.5f),
                Location = new Point(20, 112),
                Width = 400
            };

            Label lblPass = new Label
            {
                Text = "Password:",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = VectorGraphics.ColorTextPrimary,
                Location = new Point(20, 146),
                AutoSize = true
            };
            txtPassword = new TextBox
            {
                Text = "op123",
                Font = new Font("Segoe UI", 9.5f),
                UseSystemPasswordChar = true,
                Location = new Point(20, 166),
                Width = 400
            };

            lblError = new Label
            {
                Text = "Default: admin/admin123, operator1/op123, officer_nu/nu2026",
                Font = new Font("Segoe UI", 8f, FontStyle.Italic),
                ForeColor = VectorGraphics.ColorTextSecondary,
                Location = new Point(20, 196),
                Size = new Size(400, 20)
            };

            btnLogin = new Button
            {
                Text = "Sign In & Authorize",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Location = new Point(20, 224),
                Size = new Size(240, 36),
                UseMnemonic = false
            };
            VectorGraphics.ApplyModernButton(
                btnLogin, 
                VectorGraphics.ColorPrimary, 
                VectorGraphics.ColorPrimaryDark, 
                VectorGraphics.ColorPrimaryHover, 
                Color.White, 
                6);
            btnLogin.Click += BtnLogin_Click;

            btnCancel = new Button
            {
                Text = "Cancel",
                Font = new Font("Segoe UI", 9.5f),
                Location = new Point(270, 224),
                Size = new Size(150, 36)
            };
            VectorGraphics.ApplyOutlinedButton(
                btnCancel, 
                VectorGraphics.ColorTextSecondary, 
                VectorGraphics.ColorTextSecondary, 
                6);
            btnCancel.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };

            this.Controls.Add(pnlHeader);
            this.Controls.Add(lblSub);
            this.Controls.Add(lblUser);
            this.Controls.Add(txtUsername);
            this.Controls.Add(lblPass);
            this.Controls.Add(txtPassword);
            this.Controls.Add(lblError);
            this.Controls.Add(btnLogin);
            this.Controls.Add(btnCancel);
            this.AcceptButton = btnLogin;
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string u = txtUsername.Text.Trim();
            string p = txtPassword.Text;

            if (string.IsNullOrEmpty(u) || string.IsNullOrEmpty(p))
            {
                lblError.Text = "Please enter both username and password.";
                lblError.ForeColor = VectorGraphics.ColorError;
                return;
            }

            lblError.Text = "Authenticating against dbo.transcript_user...";
            lblError.ForeColor = VectorGraphics.ColorPrimary;
            Application.DoEvents();

            UserAccount acc = UserPermissionManager.Authenticate(u, p, _connStr);
            if (acc == null)
            {
                lblError.Text = "Authentication failed. Check credentials in dbo.transcript_user.";
                lblError.ForeColor = VectorGraphics.ColorError;
                return;
            }

            if (!acc.IsActive)
            {
                lblError.Text = "Account '" + u + "' is deactivated.";
                lblError.ForeColor = VectorGraphics.ColorError;
                return;
            }

            UserSession.SwitchUser(acc);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
