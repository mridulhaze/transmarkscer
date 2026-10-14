using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace NU_Solver
{
    public class MtempOptForm : Form
    {
        // Top Quick Bar
        private Panel pnlTopBar;
        private Button btnOpenEditMtemp;
        private Label lblHeaderInfo;

        // Operation 1 Controls
        private GroupBox gbOp1;
        private Button btnCreateMtemp;
        private Label lblDescOp1;
        private Label lblConnOp1;
        private TextBox txtConnOp1;
        private Button btnSaveOp1;
        private Button btnTestOp1;

        // Operation 2 Controls
        private GroupBox gbOp2;
        private Button btnUpdateIncourseFF;
        private Label lblDescOp2;
        private Label lblConnOp2;
        private TextBox txtConnOp2;
        private Button btnSaveOp2;
        private Button btnTestOp2;

        // Operation 3 Controls
        private GroupBox gbOp3;
        private Button btnUpdateType3;
        private Label lblDescOp3;
        private Label lblConnOp3;
        private TextBox txtConnOp3;
        private Button btnSaveOp3;
        private Button btnTestOp3;

        // Operation 4 Controls
        private GroupBox gbOp4;
        private Button btnUpdateMarksHsolving;
        private Label lblDescOp4;
        private Label lblConnOp4;
        private TextBox txtConnOp4;
        private Button btnSaveOp4;
        private Button btnTestOp4;

        // Operation 5 Controls: MCQ Update
        private GroupBox gbOp5;
        private Button btnUpdateMcq;
        private Label lblDescOp5;
        private Label lblConnOp5;
        private TextBox txtConnOp5;
        private Button btnSaveOp5;
        private Button btnTestOp5;

        // Operation 6 Controls: Mark & Grade Calculation
        private GroupBox gbOp6;
        private Button btnCalcMarksGrades;
        private Label lblDescOp6;
        private Label lblConnOp6;
        private TextBox txtConnOp6;
        private Button btnSaveOp6;
        private Button btnTestOp6;

        // Operation 7 Controls: Absent Wanting & Printable Report
        private GroupBox gbOp7;
        private Button btnAbsentWanting;
        private Label lblDescOp7;
        private Label lblConnOp7;
        private TextBox txtConnOp7;
        private Button btnSaveOp7;
        private Button btnTestOp7;
        private Button btnOpenPrintableReport;
        private Button btnExportWantingCsv;

        // Bottom Controls
        private Panel pnlBottom;
        private GroupBox gbLog;
        private TextBox txtLog;
        private Button btnClearLog;
        private Button btnClose;

        // Scrollable container for cards
        private Panel pnlCardsContainer;

        // Last generated wanting report & data
        private string lastHtmlReportPath = null;
        private DataTable lastDtWanting = null;

        public MtempOptForm()
        {
            InitializeComponent();
            LoadConfiguredConnections();
            UITheme.ApplyTheme(this, "MTEMP Operations (Mtemp OPT)");

            // Apply distinct thematic colors to action buttons
            UITheme.StyleButton(btnOpenEditMtemp, UITheme.StyleIndigo);
            UITheme.StyleButton(btnCreateMtemp, UITheme.StyleTeal);
            UITheme.StyleButton(btnUpdateIncourseFF, UITheme.StyleAmber);
            UITheme.StyleButton(btnUpdateType3, UITheme.StyleSky);
            UITheme.StyleButton(btnUpdateMarksHsolving, UITheme.StyleEmerald);
            UITheme.StyleButton(btnUpdateMcq, UITheme.StyleSky);
            UITheme.StyleButton(btnCalcMarksGrades, UITheme.StyleEmerald);
            UITheme.StyleButton(btnAbsentWanting, UITheme.StyleRose);
            UITheme.StyleButton(btnOpenPrintableReport, UITheme.StyleTeal);
            UITheme.StyleButton(btnExportWantingCsv, UITheme.StyleEmerald);

            UITheme.StyleButton(btnSaveOp1, UITheme.StyleEmerald);
            UITheme.StyleButton(btnTestOp1, UITheme.StyleSlate);
            UITheme.StyleButton(btnSaveOp2, UITheme.StyleEmerald);
            UITheme.StyleButton(btnTestOp2, UITheme.StyleSlate);
            UITheme.StyleButton(btnSaveOp3, UITheme.StyleEmerald);
            UITheme.StyleButton(btnTestOp3, UITheme.StyleSlate);
            UITheme.StyleButton(btnSaveOp4, UITheme.StyleEmerald);
            UITheme.StyleButton(btnTestOp4, UITheme.StyleSlate);
            UITheme.StyleButton(btnSaveOp5, UITheme.StyleEmerald);
            UITheme.StyleButton(btnTestOp5, UITheme.StyleSlate);
            UITheme.StyleButton(btnSaveOp6, UITheme.StyleEmerald);
            UITheme.StyleButton(btnTestOp6, UITheme.StyleSlate);
            UITheme.StyleButton(btnSaveOp7, UITheme.StyleEmerald);
            UITheme.StyleButton(btnTestOp7, UITheme.StyleSlate);

            UITheme.StyleButton(btnClearLog, UITheme.StyleSlate);
            UITheme.StyleButton(btnClose, UITheme.StyleRose);

            Log("Ready. Loaded database links for all MTEMP operations.");
        }

        private void InitializeComponent()
        {
            this.Text = "MTEMP Operations (Mtemp OPT)";
            this.Size = new Size(960, 860);
            this.MinimumSize = new Size(880, 700);
            this.StartPosition = FormStartPosition.CenterParent;
            this.ShowInTaskbar = false;
            this.DoubleBuffered = true;

            // Top Quick Bar for Edit mtemp
            pnlTopBar = new Panel();
            pnlTopBar.Dock = DockStyle.Top;
            pnlTopBar.Height = 48;
            pnlTopBar.BackColor = Color.FromArgb(241, 245, 249);
            pnlTopBar.Padding = new Padding(14, 8, 14, 8);

            lblHeaderInfo = new Label();
            lblHeaderInfo.Text = "Manage, update, calculate, verify, and edit MTEMP records across databases:";
            lblHeaderInfo.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);
            lblHeaderInfo.ForeColor = Color.FromArgb(51, 65, 85);
            lblHeaderInfo.Location = new Point(14, 14);
            lblHeaderInfo.AutoSize = true;

            btnOpenEditMtemp = new Button();
            btnOpenEditMtemp.Text = "✏️ Edit mtemp (Interactive Grid Editor)";
            btnOpenEditMtemp.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            btnOpenEditMtemp.Size = new Size(270, 32);
            btnOpenEditMtemp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnOpenEditMtemp.Location = new Point(pnlTopBar.Width - 285, 8);
            btnOpenEditMtemp.Click += btnOpenEditMtemp_Click;

            pnlTopBar.Controls.Add(lblHeaderInfo);
            pnlTopBar.Controls.Add(btnOpenEditMtemp);

            // Bottom Panel for Log and Actions
            pnlBottom = new Panel();
            pnlBottom.Dock = DockStyle.Bottom;
            pnlBottom.Height = 200;
            pnlBottom.Padding = new Padding(12, 4, 12, 10);

            gbLog = new GroupBox();
            gbLog.Text = "Execution Log & Operation Details";
            gbLog.Dock = DockStyle.Fill;
            gbLog.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            gbLog.Padding = new Padding(8);

            txtLog = new TextBox();
            txtLog.Multiline = true;
            txtLog.ReadOnly = true;
            txtLog.ScrollBars = ScrollBars.Vertical;
            txtLog.Dock = DockStyle.Fill;
            txtLog.Font = new Font("Consolas", 9.5f, FontStyle.Regular);
            txtLog.BackColor = Color.FromArgb(248, 250, 252);
            txtLog.ForeColor = Color.FromArgb(15, 23, 42);

            Panel pnlLogActions = new Panel();
            pnlLogActions.Dock = DockStyle.Bottom;
            pnlLogActions.Height = 34;
            pnlLogActions.Padding = new Padding(0, 4, 0, 0);

            btnClearLog = new Button();
            btnClearLog.Text = "🧹 Clear Log";
            btnClearLog.Size = new Size(110, 28);
            btnClearLog.Location = new Point(0, 4);
            btnClearLog.Click += (s, e) => txtLog.Clear();

            btnClose = new Button();
            btnClose.Text = "❌ Close";
            btnClose.Size = new Size(100, 28);
            btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClose.Location = new Point(gbLog.Width - 120, 4);
            btnClose.Click += (s, e) => this.Close();

            pnlLogActions.Controls.Add(btnClearLog);
            pnlLogActions.Controls.Add(btnClose);
            gbLog.Controls.Add(txtLog);
            gbLog.Controls.Add(pnlLogActions);
            pnlBottom.Controls.Add(gbLog);

            // Container for operation cards
            pnlCardsContainer = new Panel();
            pnlCardsContainer.Dock = DockStyle.Fill;
            pnlCardsContainer.AutoScroll = true;
            pnlCardsContainer.Padding = new Padding(14, 10, 14, 6);

            // Build All Operation Cards
            BuildOp1Card();
            BuildOp2Card();
            BuildOp3Card();
            BuildOp4Card();
            BuildOp5Card();
            BuildOp6Card();
            BuildOp7Card();

            this.Controls.Add(pnlCardsContainer);
            this.Controls.Add(pnlTopBar);
            this.Controls.Add(pnlBottom);
        }

        private void BuildOp1Card()
        {
            gbOp1 = new GroupBox();
            gbOp1.Text = "Operation 1: Create mtemp from FF & Attendance";
            gbOp1.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
            gbOp1.Location = new Point(14, 10);
            gbOp1.Size = new Size(900, 110);
            gbOp1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            btnCreateMtemp = new Button();
            btnCreateMtemp.Text = "📋 Create mtemp from FF";
            btnCreateMtemp.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            btnCreateMtemp.Location = new Point(15, 24);
            btnCreateMtemp.Size = new Size(240, 36);
            btnCreateMtemp.Click += btnCreateMtemp_Click;

            lblDescOp1 = new Label();
            lblDescOp1.Text = "Drops & recreates 'mtemp' (40 cols) and populates from 'attandance', 'FF', and 'bedhons_courses'.";
            lblDescOp1.Font = new Font("Segoe UI", 9f, FontStyle.Regular);
            lblDescOp1.ForeColor = Color.FromArgb(71, 85, 105);
            lblDescOp1.Location = new Point(265, 32);
            lblDescOp1.AutoSize = true;

            lblConnOp1 = new Label();
            lblConnOp1.Text = "DB Link:";
            lblConnOp1.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            lblConnOp1.Location = new Point(15, 72);
            lblConnOp1.AutoSize = true;

            txtConnOp1 = new TextBox();
            txtConnOp1.Font = new Font("Segoe UI", 9f, FontStyle.Regular);
            txtConnOp1.Location = new Point(78, 69);
            txtConnOp1.Size = new Size(625, 25);
            txtConnOp1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            btnSaveOp1 = new Button();
            btnSaveOp1.Text = "💾 Save";
            btnSaveOp1.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            btnSaveOp1.Location = new Point(715, 67);
            btnSaveOp1.Size = new Size(80, 29);
            btnSaveOp1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSaveOp1.Click += (s, e) => SaveConnection("CreateMtempConn", txtConnOp1.Text);

            btnTestOp1 = new Button();
            btnTestOp1.Text = "🔌 Test";
            btnTestOp1.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            btnTestOp1.Location = new Point(803, 67);
            btnTestOp1.Size = new Size(80, 29);
            btnTestOp1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnTestOp1.Click += (s, e) => TestConn(txtConnOp1.Text, "Operation 1");

            gbOp1.Controls.Add(btnCreateMtemp);
            gbOp1.Controls.Add(lblDescOp1);
            gbOp1.Controls.Add(lblConnOp1);
            gbOp1.Controls.Add(txtConnOp1);
            gbOp1.Controls.Add(btnSaveOp1);
            gbOp1.Controls.Add(btnTestOp1);
            pnlCardsContainer.Controls.Add(gbOp1);
        }

        private void BuildOp2Card()
        {
            gbOp2 = new GroupBox();
            gbOp2.Text = "Operation 2: Update Incourse from FF";
            gbOp2.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
            gbOp2.Location = new Point(14, 128);
            gbOp2.Size = new Size(900, 110);
            gbOp2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            btnUpdateIncourseFF = new Button();
            btnUpdateIncourseFF.Text = "📝 Update Incourse from FF";
            btnUpdateIncourseFF.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            btnUpdateIncourseFF.Location = new Point(15, 24);
            btnUpdateIncourseFF.Size = new Size(240, 36);
            btnUpdateIncourseFF.Click += btnUpdateIncourseFF_Click;

            lblDescOp2 = new Label();
            lblDescOp2.Text = "Splits inc_mark & total_sub from FF for regular students (STD_TYPE <> '3') into staging & updates mtemp.";
            lblDescOp2.Font = new Font("Segoe UI", 9f, FontStyle.Regular);
            lblDescOp2.ForeColor = Color.FromArgb(71, 85, 105);
            lblDescOp2.Location = new Point(265, 32);
            lblDescOp2.AutoSize = true;

            lblConnOp2 = new Label();
            lblConnOp2.Text = "DB Link:";
            lblConnOp2.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            lblConnOp2.Location = new Point(15, 72);
            lblConnOp2.AutoSize = true;

            txtConnOp2 = new TextBox();
            txtConnOp2.Font = new Font("Segoe UI", 9f, FontStyle.Regular);
            txtConnOp2.Location = new Point(78, 69);
            txtConnOp2.Size = new Size(625, 25);
            txtConnOp2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            btnSaveOp2 = new Button();
            btnSaveOp2.Text = "💾 Save";
            btnSaveOp2.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            btnSaveOp2.Location = new Point(715, 67);
            btnSaveOp2.Size = new Size(80, 29);
            btnSaveOp2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSaveOp2.Click += (s, e) => SaveConnection("IncourseFFConn", txtConnOp2.Text);

            btnTestOp2 = new Button();
            btnTestOp2.Text = "🔌 Test";
            btnTestOp2.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            btnTestOp2.Location = new Point(803, 67);
            btnTestOp2.Size = new Size(80, 29);
            btnTestOp2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnTestOp2.Click += (s, e) => TestConn(txtConnOp2.Text, "Operation 2");

            gbOp2.Controls.Add(btnUpdateIncourseFF);
            gbOp2.Controls.Add(lblDescOp2);
            gbOp2.Controls.Add(lblConnOp2);
            gbOp2.Controls.Add(txtConnOp2);
            gbOp2.Controls.Add(btnSaveOp2);
            gbOp2.Controls.Add(btnTestOp2);
            pnlCardsContainer.Controls.Add(gbOp2);
        }

        private void BuildOp3Card()
        {
            gbOp3 = new GroupBox();
            gbOp3.Text = "Operation 3: Update Type 3 Incourse from examinee_course";
            gbOp3.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
            gbOp3.Location = new Point(14, 246);
            gbOp3.Size = new Size(900, 110);
            gbOp3.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            btnUpdateType3 = new Button();
            btnUpdateType3.Text = "🎓 Update Type 3 Incourse";
            btnUpdateType3.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            btnUpdateType3.Location = new Point(15, 24);
            btnUpdateType3.Size = new Size(240, 36);
            btnUpdateType3.Click += btnUpdateType3_Click;

            lblDescOp3 = new Label();
            lblDescOp3.Text = "Transfers incourse marks from 'examinee_course' for STD_TYPE = '3' matching reg_no and course_code.";
            lblDescOp3.Font = new Font("Segoe UI", 9f, FontStyle.Regular);
            lblDescOp3.ForeColor = Color.FromArgb(71, 85, 105);
            lblDescOp3.Location = new Point(265, 32);
            lblDescOp3.AutoSize = true;

            lblConnOp3 = new Label();
            lblConnOp3.Text = "DB Link:";
            lblConnOp3.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            lblConnOp3.Location = new Point(15, 72);
            lblConnOp3.AutoSize = true;

            txtConnOp3 = new TextBox();
            txtConnOp3.Font = new Font("Segoe UI", 9f, FontStyle.Regular);
            txtConnOp3.Location = new Point(78, 69);
            txtConnOp3.Size = new Size(625, 25);
            txtConnOp3.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            btnSaveOp3 = new Button();
            btnSaveOp3.Text = "💾 Save";
            btnSaveOp3.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            btnSaveOp3.Location = new Point(715, 67);
            btnSaveOp3.Size = new Size(80, 29);
            btnSaveOp3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSaveOp3.Click += (s, e) => SaveConnection("ExamineeCourseConn", txtConnOp3.Text);

            btnTestOp3 = new Button();
            btnTestOp3.Text = "🔌 Test";
            btnTestOp3.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            btnTestOp3.Location = new Point(803, 67);
            btnTestOp3.Size = new Size(80, 29);
            btnTestOp3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnTestOp3.Click += (s, e) => TestConn(txtConnOp3.Text, "Operation 3 (examinee_course)");

            gbOp3.Controls.Add(btnUpdateType3);
            gbOp3.Controls.Add(lblDescOp3);
            gbOp3.Controls.Add(lblConnOp3);
            gbOp3.Controls.Add(txtConnOp3);
            gbOp3.Controls.Add(btnSaveOp3);
            gbOp3.Controls.Add(btnTestOp3);
            pnlCardsContainer.Controls.Add(gbOp3);
        }

        private void BuildOp4Card()
        {
            gbOp4 = new GroupBox();
            gbOp4.Text = "Operation 4: Update Marks from H_solving";
            gbOp4.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
            gbOp4.Location = new Point(14, 364);
            gbOp4.Size = new Size(900, 110);
            gbOp4.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            btnUpdateMarksHsolving = new Button();
            btnUpdateMarksHsolving.Text = "🎯 Update Marks from H_solving";
            btnUpdateMarksHsolving.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            btnUpdateMarksHsolving.Location = new Point(15, 24);
            btnUpdateMarksHsolving.Size = new Size(240, 36);
            btnUpdateMarksHsolving.Click += btnUpdateMarksHsolving_Click;

            lblDescOp4 = new Label();
            lblDescOp4.Text = "Updates marks1, eb_no, sl_no, hexcodea, hexcodeb, Ehex_no, qr2, qr3 in 'mtemp' from 'H_solving'.";
            lblDescOp4.Font = new Font("Segoe UI", 9f, FontStyle.Regular);
            lblDescOp4.ForeColor = Color.FromArgb(71, 85, 105);
            lblDescOp4.Location = new Point(265, 32);
            lblDescOp4.AutoSize = true;

            lblConnOp4 = new Label();
            lblConnOp4.Text = "DB Link:";
            lblConnOp4.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            lblConnOp4.Location = new Point(15, 72);
            lblConnOp4.AutoSize = true;

            txtConnOp4 = new TextBox();
            txtConnOp4.Font = new Font("Segoe UI", 9f, FontStyle.Regular);
            txtConnOp4.Location = new Point(78, 69);
            txtConnOp4.Size = new Size(625, 25);
            txtConnOp4.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            btnSaveOp4 = new Button();
            btnSaveOp4.Text = "💾 Save";
            btnSaveOp4.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            btnSaveOp4.Location = new Point(715, 67);
            btnSaveOp4.Size = new Size(80, 29);
            btnSaveOp4.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSaveOp4.Click += (s, e) => SaveConnection("HsolvingConn", txtConnOp4.Text);

            btnTestOp4 = new Button();
            btnTestOp4.Text = "🔌 Test";
            btnTestOp4.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            btnTestOp4.Location = new Point(803, 67);
            btnTestOp4.Size = new Size(80, 29);
            btnTestOp4.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnTestOp4.Click += (s, e) => TestConn(txtConnOp4.Text, "Operation 4 (H_solving)");

            gbOp4.Controls.Add(btnUpdateMarksHsolving);
            gbOp4.Controls.Add(lblDescOp4);
            gbOp4.Controls.Add(lblConnOp4);
            gbOp4.Controls.Add(txtConnOp4);
            gbOp4.Controls.Add(btnSaveOp4);
            gbOp4.Controls.Add(btnTestOp4);
            pnlCardsContainer.Controls.Add(gbOp4);
        }

        private void BuildOp5Card()
        {
            gbOp5 = new GroupBox();
            gbOp5.Text = "Operation 5: Update MCQ Marks from mcq Table";
            gbOp5.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
            gbOp5.Location = new Point(14, 482);
            gbOp5.Size = new Size(900, 110);
            gbOp5.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            btnUpdateMcq = new Button();
            btnUpdateMcq.Text = "✍️ Update MCQ Marks";
            btnUpdateMcq.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            btnUpdateMcq.Location = new Point(15, 24);
            btnUpdateMcq.Size = new Size(240, 36);
            btnUpdateMcq.Click += btnUpdateMcq_Click;

            lblDescOp5 = new Label();
            lblDescOp5.Text = "Updates 'mtemp.mcq_marks' from 'mcq' table matching reg_no and pap_code=course_code.";
            lblDescOp5.Font = new Font("Segoe UI", 9f, FontStyle.Regular);
            lblDescOp5.ForeColor = Color.FromArgb(71, 85, 105);
            lblDescOp5.Location = new Point(265, 32);
            lblDescOp5.AutoSize = true;

            lblConnOp5 = new Label();
            lblConnOp5.Text = "DB Link:";
            lblConnOp5.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            lblConnOp5.Location = new Point(15, 72);
            lblConnOp5.AutoSize = true;

            txtConnOp5 = new TextBox();
            txtConnOp5.Font = new Font("Segoe UI", 9f, FontStyle.Regular);
            txtConnOp5.Location = new Point(78, 69);
            txtConnOp5.Size = new Size(625, 25);
            txtConnOp5.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            btnSaveOp5 = new Button();
            btnSaveOp5.Text = "💾 Save";
            btnSaveOp5.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            btnSaveOp5.Location = new Point(715, 67);
            btnSaveOp5.Size = new Size(80, 29);
            btnSaveOp5.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSaveOp5.Click += (s, e) => SaveConnection("McqConn", txtConnOp5.Text);

            btnTestOp5 = new Button();
            btnTestOp5.Text = "🔌 Test";
            btnTestOp5.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            btnTestOp5.Location = new Point(803, 67);
            btnTestOp5.Size = new Size(80, 29);
            btnTestOp5.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnTestOp5.Click += (s, e) => TestConn(txtConnOp5.Text, "Operation 5 (mcq)");

            gbOp5.Controls.Add(btnUpdateMcq);
            gbOp5.Controls.Add(lblDescOp5);
            gbOp5.Controls.Add(lblConnOp5);
            gbOp5.Controls.Add(txtConnOp5);
            gbOp5.Controls.Add(btnSaveOp5);
            gbOp5.Controls.Add(btnTestOp5);
            pnlCardsContainer.Controls.Add(gbOp5);
        }

        private void BuildOp6Card()
        {
            gbOp6 = new GroupBox();
            gbOp6.Text = "Operation 6: Mark & Grade Calculation";
            gbOp6.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
            gbOp6.Location = new Point(14, 600);
            gbOp6.Size = new Size(900, 110);
            gbOp6.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            btnCalcMarksGrades = new Button();
            btnCalcMarksGrades.Text = "🧮 Calculate Marks & Grades";
            btnCalcMarksGrades.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            btnCalcMarksGrades.Location = new Point(15, 24);
            btnCalcMarksGrades.Size = new Size(240, 36);
            btnCalcMarksGrades.Click += btnCalcMarksGrades_Click;

            lblDescOp6 = new Label();
            lblDescOp6.Text = "Calculates final_marks, total_marks, scales non-100 full marks, and assigns Letter Grades & Grade Points.";
            lblDescOp6.Font = new Font("Segoe UI", 9f, FontStyle.Regular);
            lblDescOp6.ForeColor = Color.FromArgb(71, 85, 105);
            lblDescOp6.Location = new Point(265, 32);
            lblDescOp6.AutoSize = true;

            lblConnOp6 = new Label();
            lblConnOp6.Text = "DB Link:";
            lblConnOp6.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            lblConnOp6.Location = new Point(15, 72);
            lblConnOp6.AutoSize = true;

            txtConnOp6 = new TextBox();
            txtConnOp6.Font = new Font("Segoe UI", 9f, FontStyle.Regular);
            txtConnOp6.Location = new Point(78, 69);
            txtConnOp6.Size = new Size(625, 25);
            txtConnOp6.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            btnSaveOp6 = new Button();
            btnSaveOp6.Text = "💾 Save";
            btnSaveOp6.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            btnSaveOp6.Location = new Point(715, 67);
            btnSaveOp6.Size = new Size(80, 29);
            btnSaveOp6.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSaveOp6.Click += (s, e) => SaveConnection("CalcConn", txtConnOp6.Text);

            btnTestOp6 = new Button();
            btnTestOp6.Text = "🔌 Test";
            btnTestOp6.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            btnTestOp6.Location = new Point(803, 67);
            btnTestOp6.Size = new Size(80, 29);
            btnTestOp6.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnTestOp6.Click += (s, e) => TestConn(txtConnOp6.Text, "Operation 6 (Calculation DB)");

            gbOp6.Controls.Add(btnCalcMarksGrades);
            gbOp6.Controls.Add(lblDescOp6);
            gbOp6.Controls.Add(lblConnOp6);
            gbOp6.Controls.Add(txtConnOp6);
            gbOp6.Controls.Add(btnSaveOp6);
            gbOp6.Controls.Add(btnTestOp6);
            pnlCardsContainer.Controls.Add(gbOp6);
        }

        private void BuildOp7Card()
        {
            gbOp7 = new GroupBox();
            gbOp7.Text = "Operation 7: Absent Wanting & Printable Verification";
            gbOp7.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
            gbOp7.Location = new Point(14, 718);
            gbOp7.Size = new Size(900, 115);
            gbOp7.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            btnAbsentWanting = new Button();
            btnAbsentWanting.Text = "⚠️ Process Absent Wanting";
            btnAbsentWanting.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            btnAbsentWanting.Location = new Point(15, 24);
            btnAbsentWanting.Size = new Size(240, 36);
            btnAbsentWanting.Click += btnAbsentWanting_Click;

            lblDescOp7 = new Label();
            lblDescOp7.Text = "Marks >4 absents as 'abs', extracts wanting courses into 'ABSENT_wanting', and creates printable verification file.";
            lblDescOp7.Font = new Font("Segoe UI", 9f, FontStyle.Regular);
            lblDescOp7.ForeColor = Color.FromArgb(71, 85, 105);
            lblDescOp7.Location = new Point(265, 32);
            lblDescOp7.AutoSize = true;

            btnOpenPrintableReport = new Button();
            btnOpenPrintableReport.Text = "🖨️ View/Print Verification File";
            btnOpenPrintableReport.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            btnOpenPrintableReport.Location = new Point(625, 24);
            btnOpenPrintableReport.Size = new Size(180, 36);
            btnOpenPrintableReport.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnOpenPrintableReport.Click += btnOpenPrintableReport_Click;

            btnExportWantingCsv = new Button();
            btnExportWantingCsv.Text = "📑 Excel/CSV";
            btnExportWantingCsv.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            btnExportWantingCsv.Location = new Point(810, 24);
            btnExportWantingCsv.Size = new Size(73, 36);
            btnExportWantingCsv.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnExportWantingCsv.Click += btnExportWantingCsv_Click;

            lblConnOp7 = new Label();
            lblConnOp7.Text = "DB Link:";
            lblConnOp7.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            lblConnOp7.Location = new Point(15, 74);
            lblConnOp7.AutoSize = true;

            txtConnOp7 = new TextBox();
            txtConnOp7.Font = new Font("Segoe UI", 9f, FontStyle.Regular);
            txtConnOp7.Location = new Point(78, 71);
            txtConnOp7.Size = new Size(625, 25);
            txtConnOp7.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            btnSaveOp7 = new Button();
            btnSaveOp7.Text = "💾 Save";
            btnSaveOp7.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            btnSaveOp7.Location = new Point(715, 69);
            btnSaveOp7.Size = new Size(80, 29);
            btnSaveOp7.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSaveOp7.Click += (s, e) => SaveConnection("AbsentConn", txtConnOp7.Text);

            btnTestOp7 = new Button();
            btnTestOp7.Text = "🔌 Test";
            btnTestOp7.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            btnTestOp7.Location = new Point(803, 69);
            btnTestOp7.Size = new Size(80, 29);
            btnTestOp7.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnTestOp7.Click += (s, e) => TestConn(txtConnOp7.Text, "Operation 7 (Absent DB)");

            gbOp7.Controls.Add(btnAbsentWanting);
            gbOp7.Controls.Add(lblDescOp7);
            gbOp7.Controls.Add(btnOpenPrintableReport);
            gbOp7.Controls.Add(btnExportWantingCsv);
            gbOp7.Controls.Add(lblConnOp7);
            gbOp7.Controls.Add(txtConnOp7);
            gbOp7.Controls.Add(btnSaveOp7);
            gbOp7.Controls.Add(btnTestOp7);
            pnlCardsContainer.Controls.Add(gbOp7);
        }

        private void LoadConfiguredConnections()
        {
            string defaultConn = Database.GetConnectionString();
            txtConnOp1.Text = Database.GetMtempOpConnection("CreateMtempConn", defaultConn);
            txtConnOp2.Text = Database.GetMtempOpConnection("IncourseFFConn", defaultConn);
            txtConnOp3.Text = Database.GetMtempOpConnection("ExamineeCourseConn", defaultConn);
            txtConnOp4.Text = Database.GetMtempOpConnection("HsolvingConn", defaultConn);
            txtConnOp5.Text = Database.GetMtempOpConnection("McqConn", defaultConn);
            txtConnOp6.Text = Database.GetMtempOpConnection("CalcConn", defaultConn);
            txtConnOp7.Text = Database.GetMtempOpConnection("AbsentConn", defaultConn);
        }

        private void SaveConnection(string key, string value)
        {
            string trimmed = (value ?? "").Trim();
            if (string.IsNullOrEmpty(trimmed))
            {
                MessageBox.Show("Connection string cannot be empty.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (Database.SaveMtempOpConnection(key, trimmed))
            {
                Log(string.Format("Saved connection for key '{0}'.", key));
                MessageBox.Show("Connection string saved successfully!", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Failed to save connection string configuration.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TestConn(string connStr, string opTitle)
        {
            string trimmed = (connStr ?? "").Trim();
            if (string.IsNullOrEmpty(trimmed))
            {
                MessageBox.Show("Please enter a connection string to test.", "Connection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Log(string.Format("Testing connection for {0}...", opTitle));
            string errorMsg;
            if (Database.TestConnection(trimmed, out errorMsg))
            {
                Log(string.Format("✅ Connection test PASSED for {0}.", opTitle));
                MessageBox.Show(string.Format("✅ Connection Successful!\r\nDatabase for {0} is reachable.", opTitle), "Connection Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                Log(string.Format("❌ Connection test FAILED for {0}: {1}", opTitle, errorMsg));
                MessageBox.Show(string.Format("❌ Connection Failed!\r\n\r\nError details:\r\n{0}", errorMsg), "Connection Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetWorking(bool working)
        {
            Cursor.Current = working ? Cursors.WaitCursor : Cursors.Default;
            btnOpenEditMtemp.Enabled = !working;
            btnCreateMtemp.Enabled = !working;
            btnUpdateIncourseFF.Enabled = !working;
            btnUpdateType3.Enabled = !working;
            btnUpdateMarksHsolving.Enabled = !working;
            btnUpdateMcq.Enabled = !working;
            btnCalcMarksGrades.Enabled = !working;
            btnAbsentWanting.Enabled = !working;
        }

        private void Log(string message)
        {
            string line = string.Format("[{0:HH:mm:ss}] {1}", DateTime.Now, message);
            if (txtLog.InvokeRequired)
            {
                txtLog.Invoke(new Action(() => {
                    txtLog.AppendText(line + Environment.NewLine);
                }));
            }
            else
            {
                txtLog.AppendText(line + Environment.NewLine);
            }
        }

        // ==================== EDIT MTEMP ACTION ====================
        private void btnOpenEditMtemp_Click(object sender, EventArgs e)
        {
            try
            {
                string targetConn = txtConnOp1.Text.Trim();
                using (MtempEditForm editForm = new MtempEditForm(targetConn))
                {
                    editForm.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error launching mtemp editor:\n" + ex.Message, "Editor Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ==================== OPERATION 1: Create mtemp from FF ====================
        private void btnCreateMtemp_Click(object sender, EventArgs e)
        {
            string conn = txtConnOp1.Text.Trim();
            if (string.IsNullOrEmpty(conn))
            {
                MessageBox.Show("Please provide a database link for Operation 1.", "Missing Connection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Database.SaveMtempOpConnection("CreateMtempConn", conn);

            DialogResult dr = MessageBox.Show(
                "This action will DROP and recreate the 'mtemp' table with the exact 40-column schema from 'mtemp.sql' and populate it from 'attandance', 'FF', and 'bedhons_courses'.\r\n\r\n" +
                "Existing data in 'mtemp' will be overwritten.\r\n\r\n" +
                "Are you sure you want to proceed?",
                "Operation 1: Create mtemp from FF",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (dr != DialogResult.Yes) return;

            SetWorking(true);
            Log("==================================================");
            Log("Starting Operation 1: Create mtemp from FF...");

            try
            {
                Database.MtempCreationResult res = Database.CreateMtempFromAttendance(conn);

                foreach (string d in res.Details)
                {
                    Log("  • " + d);
                }
                Log("Result: " + res.Message);

                MessageBox.Show(res.Message, "Operation 1: Create mtemp", MessageBoxButtons.OK, res.Success ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                Log("❌ Error during Operation 1: " + ex.Message);
                MessageBox.Show("Error executing Operation 1:\r\n" + ex.Message, "Operation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetWorking(false);
            }
        }

        // ==================== OPERATION 2: Update Incourse from FF ====================
        private void btnUpdateIncourseFF_Click(object sender, EventArgs e)
        {
            string conn = txtConnOp2.Text.Trim();
            if (string.IsNullOrEmpty(conn))
            {
                MessageBox.Show("Please provide a database link for Operation 2.", "Missing Connection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Database.SaveMtempOpConnection("IncourseFFConn", conn);

            DialogResult dr = MessageBox.Show(
                "This action will extract 5 course marks per examinee from 'FF' (STD_TYPE <> '3'), populate staging table 'BEDHONS_5685_ff_split_data_24_NEW', and update 'mtemp.incourse_marks'.\r\n\r\n" +
                "Proceed with Operation 2?",
                "Operation 2: Update Incourse from FF",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (dr != DialogResult.Yes) return;

            SetWorking(true);
            Log("==================================================");
            Log("Starting Operation 2: Update Incourse from FF...");

            try
            {
                Database.MtempCreationResult res = Database.UpdateIncourseFromFF(conn);

                foreach (string d in res.Details)
                {
                    Log("  • " + d);
                }
                Log("Result: " + res.Message);

                MessageBox.Show(res.Message, "Operation 2: Update Incourse from FF", MessageBoxButtons.OK, res.Success ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                Log("❌ Error during Operation 2: " + ex.Message);
                MessageBox.Show("Error executing Operation 2:\r\n" + ex.Message, "Operation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetWorking(false);
            }
        }

        // ==================== OPERATION 3: Update Type 3 from examinee_course ====================
        private void btnUpdateType3_Click(object sender, EventArgs e)
        {
            string mtempConn = txtConnOp1.Text.Trim();
            string examConn = txtConnOp3.Text.Trim();

            if (string.IsNullOrEmpty(mtempConn))
            {
                MessageBox.Show("Please ensure Operation 1 has a valid target mtemp database link.", "Missing Target Connection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrEmpty(examConn))
            {
                MessageBox.Show("Please provide a database link for Operation 3 (examinee_course database).", "Missing Connection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Database.SaveMtempOpConnection("ExamineeCourseConn", examConn);

            DialogResult dr = MessageBox.Show(
                "This action will transfer incourse marks from 'examinee_course' for STD_TYPE = '3' (irregular/retake students) into 'mtemp.incourse_marks' matching reg_no and course_code.\r\n\r\n" +
                "Target mtemp DB: " + txtConnOp1.Text.Trim() + "\r\n" +
                "Source examinee DB: " + examConn + "\r\n\r\n" +
                "Proceed with Operation 3?",
                "Operation 3: Update Type 3 Incourse Marks",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (dr != DialogResult.Yes) return;

            SetWorking(true);
            Log("==================================================");
            Log("Starting Operation 3: Update Type 3 Incourse from examinee_course...");

            try
            {
                Database.MtempCreationResult res = Database.UpdateType3IncourseFromExamineeCourse(mtempConn, examConn);

                foreach (string d in res.Details)
                {
                    Log("  • " + d);
                }
                Log("Result: " + res.Message);

                MessageBox.Show(res.Message, "Operation 3: Update Type 3 Incourse", MessageBoxButtons.OK, res.Success ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                Log("❌ Error during Operation 3: " + ex.Message);
                MessageBox.Show("Error executing Operation 3:\r\n" + ex.Message, "Operation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetWorking(false);
            }
        }

        // ==================== OPERATION 4: Update Marks from H_solving ====================
        private void btnUpdateMarksHsolving_Click(object sender, EventArgs e)
        {
            string mtempConn = txtConnOp1.Text.Trim();
            string hsolvConn = txtConnOp4.Text.Trim();

            if (string.IsNullOrEmpty(mtempConn))
            {
                MessageBox.Show("Please ensure Operation 1 has a valid target mtemp database link.", "Missing Target Connection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrEmpty(hsolvConn))
            {
                MessageBox.Show("Please provide a database link for Operation 4 (H_solving database).", "Missing Connection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Database.SaveMtempOpConnection("HsolvingConn", hsolvConn);

            DialogResult dr = MessageBox.Show(
                "This action will update marks1, eb_no, sl_no, hexcodea, hexcodeb, Ehex_no, qr2, qr3 in 'mtemp' from 'H_solving' matched on qr1, form_sl, or reg_no + pap_code.\r\n\r\n" +
                "Target mtemp DB: " + txtConnOp1.Text.Trim() + "\r\n" +
                "Source H_solving DB: " + hsolvConn + "\r\n\r\n" +
                "Proceed with Operation 4?",
                "Operation 4: Update Marks from H_solving",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (dr != DialogResult.Yes) return;

            SetWorking(true);
            Log("==================================================");
            Log("Starting Operation 4: Update Marks from H_solving...");

            try
            {
                Database.MtempCreationResult res = Database.UpdateMarksFromHsolving(mtempConn, hsolvConn);

                foreach (string d in res.Details)
                {
                    Log("  • " + d);
                }
                Log("Result: " + res.Message);

                MessageBox.Show(res.Message, "Operation 4: Update Marks from H_solving", MessageBoxButtons.OK, res.Success ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                Log("❌ Error during Operation 4: " + ex.Message);
                MessageBox.Show("Error executing Operation 4:\r\n" + ex.Message, "Operation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetWorking(false);
            }
        }

        // ==================== OPERATION 5: Update MCQ Marks ====================
        private void btnUpdateMcq_Click(object sender, EventArgs e)
        {
            string mtempConn = txtConnOp1.Text.Trim();
            string mcqConn = txtConnOp5.Text.Trim();

            if (string.IsNullOrEmpty(mtempConn))
            {
                MessageBox.Show("Please ensure Operation 1 has a valid target mtemp database link.", "Missing Target Connection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrEmpty(mcqConn))
            {
                MessageBox.Show("Please provide a database link for Operation 5 (mcq database).", "Missing Connection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Database.SaveMtempOpConnection("McqConn", mcqConn);

            DialogResult dr = MessageBox.Show(
                "This action will update 'mtemp.mcq_marks' from 'mcq.mark' matching reg_no and pap_code = course_code.\r\n\r\n" +
                "Target mtemp DB: " + mtempConn + "\r\n" +
                "Source mcq DB: " + mcqConn + "\r\n\r\n" +
                "Proceed with Operation 5?",
                "Operation 5: Update MCQ Marks",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (dr != DialogResult.Yes) return;

            SetWorking(true);
            Log("==================================================");
            Log("Starting Operation 5: Update MCQ Marks from mcq...");

            try
            {
                Database.MtempCreationResult res = Database.UpdateMcqMarks(mtempConn, mcqConn);

                foreach (string d in res.Details)
                {
                    Log("  • " + d);
                }
                Log("Result: " + res.Message);

                MessageBox.Show(res.Message, "Operation 5: MCQ Update", MessageBoxButtons.OK, res.Success ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                Log("❌ Error during Operation 5: " + ex.Message);
                MessageBox.Show("Error executing Operation 5:\r\n" + ex.Message, "Operation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetWorking(false);
            }
        }

        // ==================== OPERATION 6: Calculate Marks & Grades ====================
        private void btnCalcMarksGrades_Click(object sender, EventArgs e)
        {
            string mtempConn = txtConnOp6.Text.Trim();
            if (string.IsNullOrEmpty(mtempConn))
            {
                mtempConn = txtConnOp1.Text.Trim();
            }

            if (string.IsNullOrEmpty(mtempConn))
            {
                MessageBox.Show("Please provide a database link for Operation 6.", "Missing Connection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Database.SaveMtempOpConnection("CalcConn", mtempConn);

            DialogResult dr = MessageBox.Show(
                "This action will perform full calculation on 'mtemp':\r\n\r\n" +
                "1. Compute final_marks from marks1/marks2/absent codes\r\n" +
                "2. Calculate mcq_inc_total (mcq + incourse)\r\n" +
                "3. Compute total_marks\r\n" +
                "4. Scale non-100 full mark courses to 100% equivalent\r\n" +
                "5. Assign Letter Grades (A+ to F) and Grade Points (4.00 to 0.00)\r\n\r\n" +
                "Target DB: " + mtempConn + "\r\n\r\n" +
                "Proceed with calculation?",
                "Operation 6: Mark & Grade Calculation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (dr != DialogResult.Yes) return;

            SetWorking(true);
            Log("==================================================");
            Log("Starting Operation 6: Mark & Grade Calculation...");

            try
            {
                Database.MtempCreationResult res = Database.CalculateMtempMarksAndGrades(mtempConn);

                foreach (string d in res.Details)
                {
                    Log("  • " + d);
                }
                Log("Result: " + res.Message);

                MessageBox.Show(res.Message, "Operation 6: Calculation Result", MessageBoxButtons.OK, res.Success ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                Log("❌ Error during Operation 6: " + ex.Message);
                MessageBox.Show("Error executing Operation 6:\r\n" + ex.Message, "Operation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetWorking(false);
            }
        }

        // ==================== OPERATION 7: Absent Wanting ====================
        private void btnAbsentWanting_Click(object sender, EventArgs e)
        {
            string mtempConn = txtConnOp7.Text.Trim();
            if (string.IsNullOrEmpty(mtempConn))
            {
                mtempConn = txtConnOp1.Text.Trim();
            }

            if (string.IsNullOrEmpty(mtempConn))
            {
                MessageBox.Show("Please provide a database link for Operation 7.", "Missing Connection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Database.SaveMtempOpConnection("AbsentConn", mtempConn);

            DialogResult dr = MessageBox.Show(
                "This action will:\r\n" +
                "1. Find students missing in >4 courses and update their status to 'abs' (marks1 = -999)\r\n" +
                "2. Extract single/partial absent wanting records into 'dbo.ABSENT_wanting'\r\n" +
                "3. Generate a formatted printable HTML verification list with signature blocks\r\n\r\n" +
                "Proceed with Operation 7?",
                "Operation 7: Absent Wanting & Verification",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (dr != DialogResult.Yes) return;

            SetWorking(true);
            Log("==================================================");
            Log("Starting Operation 7: Absent Wanting & Verification Report...");

            try
            {
                DataTable dtWanting;
                string htmlPath;
                Database.MtempCreationResult res = Database.ProcessAbsentWantingAndExport(mtempConn, out dtWanting, out htmlPath);

                lastDtWanting = dtWanting;
                lastHtmlReportPath = htmlPath;

                foreach (string d in res.Details)
                {
                    Log("  • " + d);
                }
                Log("Result: " + res.Message);

                DialogResult openReportDr = MessageBox.Show(
                    res.Message + "\r\n\r\nWould you like to open the printable verification list right now?",
                    "Absent Wanting Ready",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information);

                if (openReportDr == DialogResult.Yes && !string.IsNullOrEmpty(htmlPath))
                {
                    Process.Start(new ProcessStartInfo(htmlPath) { UseShellExecute = true });
                }
            }
            catch (Exception ex)
            {
                Log("❌ Error during Operation 7: " + ex.Message);
                MessageBox.Show("Error executing Operation 7:\r\n" + ex.Message, "Operation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetWorking(false);
            }
        }

        private void btnOpenPrintableReport_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(lastHtmlReportPath) || !System.IO.File.Exists(lastHtmlReportPath))
            {
                string baseDir = string.IsNullOrEmpty(Application.StartupPath) ? AppDomain.CurrentDomain.BaseDirectory : Application.StartupPath;
                string[] checkDirs = new string[] {
                    System.IO.Path.Combine(baseDir, "Reports"),
                    System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "NU_Solver_Reports"),
                    System.IO.Path.GetTempPath()
                };

                foreach (string dir in checkDirs)
                {
                    if (System.IO.Directory.Exists(dir))
                    {
                        string[] files = System.IO.Directory.GetFiles(dir, "Absent_Wanting_*.html");
                        if (files.Length > 0)
                        {
                            Array.Sort(files);
                            lastHtmlReportPath = files[files.Length - 1]; // most recent
                            break;
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(lastHtmlReportPath) && System.IO.File.Exists(lastHtmlReportPath))
            {
                Process.Start(new ProcessStartInfo(lastHtmlReportPath) { UseShellExecute = true });
            }
            else
            {
                MessageBox.Show("No absent wanting report has been generated yet. Please run 'Process Absent Wanting' first.", "Report Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnExportWantingCsv_Click(object sender, EventArgs e)
        {
            if (lastDtWanting == null || lastDtWanting.Rows.Count == 0)
            {
                // Try query from DB directly
                try
                {
                    using (SqlConnection con = new SqlConnection(txtConnOp7.Text.Trim()))
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand("SELECT * FROM dbo.ABSENT_wanting ORDER BY [no]", con))
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            lastDtWanting = new DataTable();
                            da.Fill(lastDtWanting);
                        }
                    }
                }
                catch { }
            }

            if (lastDtWanting == null || lastDtWanting.Rows.Count == 0)
            {
                MessageBox.Show("No absent wanting records found to export. Please run 'Process Absent Wanting' first.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "CSV File (*.csv)|*.csv|All Files (*.*)|*.*";
                sfd.FileName = string.Format("Absent_Wanting_{0:yyyyMMdd}.csv", DateTime.Now);
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    if (Database.ExportDataTableToCsv(lastDtWanting, sfd.FileName))
                    {
                        MessageBox.Show("Exported absent wanting records successfully!", "Export Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Failed to export absent wanting records.", "Export Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
