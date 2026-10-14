using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace NU_Solver
{
    /// <summary>
    /// Unified Modern Admin Dashboard:
    /// - Course Printing Workflow Board (Kanban Cards with Start, Pause, Resume, Report - Image 1)
    /// - User Credentials & RBAC Permission Management (View Only, Print, Edit, Admin - Image 2)
    /// - Live summary KPI cards, Course Design & DB settings link, and Audit reports
    /// </summary>
    public class AdminDashboardForm : Form
    {
        // UI Layout containers
        private Panel pnlSidebar;
        private Panel pnlTopHeader;
        private Panel pnlContent;
        private FlowLayoutPanel pnlKpiMetrics;

        // Navigation tab buttons
        private Button btnNavWorkflow;
        private Button btnNavUsers;
        private Button btnNavCourseDesign;
        private Button btnNavAuditReport;
        private Button btnNavRefresh;

        // Content Views
        private Panel pnlWorkflowView;
        private Panel pnlUserAdminView;

        // Workflow Kanban columns
        private FlowLayoutPanel pnlColTodo;
        private FlowLayoutPanel pnlColProcessing;
        private FlowLayoutPanel pnlColPaused;
        private FlowLayoutPanel pnlColDone;

        // Workflow summary counters
        private Label lblKpiUpcoming;
        private Label lblKpiProcessing;
        private Label lblKpiTotalPrinted;
        private Label lblKpiResolved;

        // User Admin Controls
        private DataGridView dgvUsers;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private TextBox txtFullName;
        private ComboBox cboRole;
        private CheckBox chkViewOnly;
        private CheckBox chkCanPrint;
        private CheckBox chkCanEdit;
        private CheckBox chkCanAdmin;
        private Button btnAddUser;
        private Button btnUpdateUser;
        private Button btnDeleteUser;
        private Button btnNewUser;
        private Label lblUserStatus;

        // Theme colors (Lite Green and White-Orange theme)
        private readonly Color ColSidebarBg = Color.FromArgb(10, 110, 68);        // #0A6E44 Deep Green
        private readonly Color ColSidebarActive = Color.FromArgb(16, 149, 93);   // #10955D Lite Green
        private readonly Color ColHeaderBg = Color.FromArgb(255, 255, 255);     // Pure White #FFFFFF
        private readonly Color ColBoardBg = Color.FromArgb(247, 250, 247);      // Minty off-white #F7FAF7
        
        // Kanban Column Header Colors
        private readonly Color ColTodoHeader = Color.FromArgb(16, 149, 93);      // #10955D Lite Green
        private readonly Color ColProcessHeader = Color.FromArgb(249, 115, 22);  // #F97316 Warm Orange
        private readonly Color ColPausedHeader = Color.FromArgb(217, 119, 6);    // Amber #D97706
        private readonly Color ColDoneHeader = Color.FromArgb(10, 110, 68);      // #0A6E44 Deep Green

        public AdminDashboardForm()
        {
            InitializeComponentLayout();
            CoursePrintWorkflow.OnQueueChanged += RefreshWorkflowBoard;
            LoadWorkflowData();
            LoadUserData();
        }

        private void InitializeComponentLayout()
        {
            this.Text = "National University - Executive Admin Workflow & RBAC Panel";
            this.Size = new Size(1300, 840);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = ColBoardBg;
            this.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);
            this.Icon = this.Icon;

            // 1. Sidebar Panel (Matches Image 2 Left Sidebar)
            pnlSidebar = new Panel();
            pnlSidebar.Dock = DockStyle.Left;
            pnlSidebar.Width = 240;
            pnlSidebar.BackColor = ColSidebarBg;
            pnlSidebar.Padding = new Padding(12);

            // Admin User Profile Card in Sidebar
            Panel pnlUserProfile = new Panel();
            pnlUserProfile.Dock = DockStyle.Top;
            pnlUserProfile.Height = 150;
            pnlUserProfile.BackColor = Color.FromArgb(16, 56, 49);
            pnlUserProfile.Padding = new Padding(10);
            pnlUserProfile.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                // Draw circular avatar with green/gold border
                Rectangle avatarRect = new Rectangle(15, 15, 50, 50);
                using (SolidBrush b = new SolidBrush(Color.FromArgb(5, 150, 105)))
                    e.Graphics.FillEllipse(b, avatarRect);
                using (Pen p = new Pen(Color.FromArgb(250, 204, 21), 2.5f))
                    e.Graphics.DrawEllipse(p, avatarRect);
                using (Font f = new Font("Segoe UI", 16, FontStyle.Bold))
                using (SolidBrush b = new SolidBrush(Color.White))
                {
                    StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                    e.Graphics.DrawString("NU", f, b, avatarRect, sf);
                }
            };

            Label lblAdminName = new Label();
            lblAdminName.Text = UserSession.CurrentUser != null ? UserSession.CurrentUser.FullName : "System Administrator";
            lblAdminName.Font = new Font("Segoe UI", 10.5f, FontStyle.Bold);
            lblAdminName.ForeColor = Color.White;
            lblAdminName.Location = new Point(75, 18);
            lblAdminName.Size = new Size(140, 24);

            Label lblAdminRole = new Label();
            lblAdminRole.Text = "Role: " + (UserSession.CurrentUser != null ? UserSession.CurrentUser.Role : "Admin");
            lblAdminRole.Font = new Font("Segoe UI", 8.5f, FontStyle.Regular);
            lblAdminRole.ForeColor = Color.FromArgb(167, 243, 208);
            lblAdminRole.Location = new Point(75, 42);
            lblAdminRole.Size = new Size(140, 18);

            Label lblBranchTitle = new Label();
            lblBranchTitle.Text = "Exam Controller Division\nNational University, BD";
            lblBranchTitle.Font = new Font("Segoe UI", 8f, FontStyle.Italic);
            lblBranchTitle.ForeColor = Color.FromArgb(148, 163, 184);
            lblBranchTitle.Location = new Point(15, 80);
            lblBranchTitle.Size = new Size(200, 36);

            pnlUserProfile.Controls.AddRange(new Control[] { lblAdminName, lblAdminRole, lblBranchTitle });

            // Sidebar Nav Buttons
            Panel pnlNavItems = new Panel();
            pnlNavItems.Dock = DockStyle.Fill;
            pnlNavItems.BackColor = Color.Transparent;

            btnNavWorkflow = CreateNavButton("Printing Workflow Board", 0);
            btnNavWorkflow.Click += (s, e) => SwitchToView(pnlWorkflowView, btnNavWorkflow);

            btnNavUsers = CreateNavButton("User & Permissions (RBAC)", 1);
            btnNavUsers.Click += (s, e) => SwitchToView(pnlUserAdminView, btnNavUsers);

            btnNavCourseDesign = CreateNavButton("Course Templates & DB", 2);
            btnNavCourseDesign.Click += (s, e) =>
            {
                if (!UserSession.CanEdit)
                {
                    MessageBox.Show("Access Denied: Your account role does not have 'Edit' permission.", "RBAC Security", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                CourseDesignDialog dlg = new CourseDesignDialog();
                dlg.ShowDialog(this);
            };

            btnNavAuditReport = CreateNavButton("Print Audit Reports", 3);
            btnNavAuditReport.Click += (s, e) =>
            {
                try
                {
                    string auditHtmlPath = PrintAuditManager.GenerateAuditReportHtml();
                    ShowHtmlReportPreview(File.ReadAllText(auditHtmlPath), "National University - Overall Print Audit Report");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error opening audit report: " + ex.Message);
                }
            };

            btnNavRefresh = CreateNavButton("Refresh All Feeds", 4);
            btnNavRefresh.Click += (s, e) =>
            {
                CoursePrintWorkflow.LoadQueue();
                RefreshWorkflowBoard();
                LoadUserData();
                MessageBox.Show("All dashboard states & queues refreshed.", "Dashboard Sync", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            pnlNavItems.Controls.AddRange(new Control[] { btnNavRefresh, btnNavAuditReport, btnNavCourseDesign, btnNavUsers, btnNavWorkflow });

            pnlSidebar.Controls.Add(pnlNavItems);
            pnlSidebar.Controls.Add(pnlUserProfile);

            // 2. Main Content Wrapper
            Panel pnlMainWrapper = new Panel();
            pnlMainWrapper.Dock = DockStyle.Fill;
            pnlMainWrapper.BackColor = ColBoardBg;

            // 2.1 Top Header
            pnlTopHeader = new Panel();
            pnlTopHeader.Dock = DockStyle.Top;
            pnlTopHeader.Height = 70;
            pnlTopHeader.BackColor = Color.White;
            pnlTopHeader.Padding = new Padding(25, 12, 25, 12);
            pnlTopHeader.Paint += (s, e) =>
            {
                using (Pen p = new Pen(Color.FromArgb(226, 232, 240), 1))
                    e.Graphics.DrawLine(p, 0, pnlTopHeader.Height - 1, pnlTopHeader.Width, pnlTopHeader.Height - 1);
            };

            Label lblTitle = new Label();
            lblTitle.Text = "Dashboard User Admin Panel & Course Printing Workflow";
            lblTitle.Font = new Font("Segoe UI", 14f, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(15, 23, 42);
            lblTitle.AutoSize = true;
            lblTitle.UseMnemonic = false;
            lblTitle.Location = new Point(20, 12);

            Label lblSubTitle = new Label();
            lblSubTitle.Text = "Live course queue dispatching, pause/resume lot controls, and multi-user permission matrix";
            lblSubTitle.Font = new Font("Segoe UI", 9f, FontStyle.Regular);
            lblSubTitle.ForeColor = Color.FromArgb(100, 116, 139);
            lblSubTitle.AutoSize = true;
            lblSubTitle.Location = new Point(22, 40);

            pnlTopHeader.Controls.AddRange(new Control[] { lblTitle, lblSubTitle });

            // 2.2 KPI Metric Cards (Matching Image 2 top metrics + Image 1 totals)
            pnlKpiMetrics = new FlowLayoutPanel();
            pnlKpiMetrics.Dock = DockStyle.Top;
            pnlKpiMetrics.Height = 110;
            pnlKpiMetrics.BackColor = ColBoardBg;
            pnlKpiMetrics.Padding = new Padding(20, 12, 20, 8);
            pnlKpiMetrics.WrapContents = false;

            lblKpiUpcoming = new Label();
            lblKpiProcessing = new Label();
            lblKpiTotalPrinted = new Label();
            lblKpiResolved = new Label();

            pnlKpiMetrics.Controls.Add(CreateKpiCard("TOTAL QUEUED", "692 Lots", Color.FromArgb(30, 58, 138), lblKpiUpcoming));
            pnlKpiMetrics.Controls.Add(CreateKpiCard("IN PROCESSING", "2 Active", Color.FromArgb(217, 119, 6), lblKpiProcessing));
            pnlKpiMetrics.Controls.Add(CreateKpiCard("TOTAL PRINTED", "8,357 Docs", Color.FromArgb(5, 150, 105), lblKpiTotalPrinted));
            pnlKpiMetrics.Controls.Add(CreateKpiCard("COMPLETION RATE", "79.8%", Color.FromArgb(192, 38, 211), lblKpiResolved));

            // 2.3 Multi-View Content Container
            pnlContent = new Panel();
            pnlContent.Dock = DockStyle.Fill;
            pnlContent.BackColor = ColBoardBg;
            pnlContent.Padding = new Padding(20, 5, 20, 20);

            // Construct Workflow Kanban View (Image 1)
            BuildWorkflowView();

            // Construct User Admin & Permissions View (Image 2)
            BuildUserAdminView();

            pnlContent.Controls.Add(pnlWorkflowView);
            pnlContent.Controls.Add(pnlUserAdminView);

            // Default to Workflow View
            pnlUserAdminView.Visible = false;
            pnlWorkflowView.Visible = true;
            btnNavWorkflow.BackColor = ColSidebarActive;

            pnlMainWrapper.Controls.Add(pnlContent);
            pnlMainWrapper.Controls.Add(pnlKpiMetrics);
            pnlMainWrapper.Controls.Add(pnlTopHeader);

            this.Controls.Add(pnlMainWrapper);
            this.Controls.Add(pnlSidebar);
        }

        private Button CreateNavButton(string text, int index)
        {
            Button btn = new Button();
            btn.Text = "  " + text;
            btn.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);
            btn.ForeColor = Color.White;
            btn.BackColor = Color.Transparent;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.TextAlign = ContentAlignment.MiddleLeft;
            btn.Dock = DockStyle.Top;
            btn.Height = 44;
            btn.Cursor = Cursors.Hand;
            btn.UseMnemonic = false;
            btn.Margin = new Padding(0, 4, 0, 4);
            return btn;
        }

        private void SwitchToView(Panel targetView, Button activeBtn)
        {
            pnlWorkflowView.Visible = (targetView == pnlWorkflowView);
            pnlUserAdminView.Visible = (targetView == pnlUserAdminView);

            btnNavWorkflow.BackColor = (btnNavWorkflow == activeBtn) ? ColSidebarActive : Color.Transparent;
            btnNavUsers.BackColor = (btnNavUsers == activeBtn) ? ColSidebarActive : Color.Transparent;

            if (targetView == pnlWorkflowView) RefreshWorkflowBoard();
            if (targetView == pnlUserAdminView) LoadUserData();
        }

        private Panel CreateKpiCard(string title, string initialValue, Color accentColor, Label valLabel)
        {
            Panel card = new Panel();
            card.Size = new Size(230, 85);
            card.Margin = new Padding(0, 0, 16, 0);
            card.BackColor = Color.White;
            card.Padding = new Padding(16, 12, 16, 12);
            card.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                // Border
                using (Pen p = new Pen(Color.FromArgb(226, 232, 240), 1))
                    e.Graphics.DrawRectangle(p, 0, 0, card.Width - 1, card.Height - 1);
                // Top accent strip
                using (SolidBrush b = new SolidBrush(accentColor))
                    e.Graphics.FillRectangle(b, 0, 0, card.Width, 4);
            };

            Label lblT = new Label();
            lblT.Text = title;
            lblT.Font = new Font("Segoe UI", 8.5f, FontStyle.Bold);
            lblT.ForeColor = Color.FromArgb(100, 116, 139);
            lblT.Location = new Point(14, 12);
            lblT.AutoSize = true;

            valLabel.Text = initialValue;
            valLabel.Font = new Font("Segoe UI", 16f, FontStyle.Bold);
            valLabel.ForeColor = accentColor;
            valLabel.Location = new Point(12, 34);
            valLabel.AutoSize = true;

            card.Controls.AddRange(new Control[] { lblT, valLabel });
            return card;
        }

        #region Workflow Kanban Board (Image 1 Implementation)
        private void BuildWorkflowView()
        {
            pnlWorkflowView = new Panel();
            pnlWorkflowView.Dock = DockStyle.Fill;
            pnlWorkflowView.BackColor = ColBoardBg;

            TableLayoutPanel tlpColumns = new TableLayoutPanel();
            tlpColumns.Dock = DockStyle.Fill;
            tlpColumns.ColumnCount = 4;
            tlpColumns.RowCount = 1;
            tlpColumns.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
            tlpColumns.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
            tlpColumns.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
            tlpColumns.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));

            // Column 1: TO DO (Queued)
            Panel col1 = BuildKanbanColumn("TO DO (Queued)", ColTodoHeader, out pnlColTodo);
            // Column 2: PROCESSING (In Progress)
            Panel col2 = BuildKanbanColumn("PROCESSING (Active)", ColProcessHeader, out pnlColProcessing);
            // Column 3: PAUSED (To Check / Halt)
            Panel col3 = BuildKanbanColumn("PAUSED (On Hold)", ColPausedHeader, out pnlColPaused);
            // Column 4: DONE (Completed)
            Panel col4 = BuildKanbanColumn("DONE (Completed)", ColDoneHeader, out pnlColDone);

            tlpColumns.Controls.Add(col1, 0, 0);
            tlpColumns.Controls.Add(col2, 1, 0);
            tlpColumns.Controls.Add(col3, 2, 0);
            tlpColumns.Controls.Add(col4, 3, 0);

            // Action toolbar above Kanban board
            Panel pnlBoardToolbar = new Panel();
            pnlBoardToolbar.Dock = DockStyle.Top;
            pnlBoardToolbar.Height = 42;
            pnlBoardToolbar.Padding = new Padding(0, 0, 0, 8);

            Button btnAddNewJob = new Button();
            btnAddNewJob.Text = "+ Queue New Course Lot";
            btnAddNewJob.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            btnAddNewJob.BackColor = Color.FromArgb(5, 150, 105);
            btnAddNewJob.ForeColor = Color.White;
            btnAddNewJob.FlatStyle = FlatStyle.Flat;
            btnAddNewJob.FlatAppearance.BorderSize = 0;
            btnAddNewJob.Size = new Size(190, 32);
            btnAddNewJob.Location = new Point(0, 2);
            btnAddNewJob.Cursor = Cursors.Hand;
            btnAddNewJob.UseMnemonic = false;
            btnAddNewJob.Click += (s, e) => PromptCreateNewJob();

            Button btnStepBatch = new Button();
            btnStepBatch.Text = "» Simulate Batch Print Step (+25 docs)";
            btnStepBatch.Font = new Font("Segoe UI", 9f, FontStyle.Regular);
            btnStepBatch.BackColor = Color.FromArgb(241, 245, 249);
            btnStepBatch.ForeColor = Color.FromArgb(51, 65, 85);
            btnStepBatch.FlatStyle = FlatStyle.Flat;
            btnStepBatch.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            btnStepBatch.Size = new Size(260, 32);
            btnStepBatch.Location = new Point(200, 2);
            btnStepBatch.UseMnemonic = false;
            btnStepBatch.Cursor = Cursors.Hand;
            btnStepBatch.Click += (s, e) =>
            {
                var processingJobs = CoursePrintWorkflow.GetAllJobs().FindAll(j => j.Status == PrintJobStatus.Processing);
                if (processingJobs.Count == 0)
                {
                    MessageBox.Show("No active processing jobs. Click [Start ▶] on a queued card first.", "Batch Stepper", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                foreach (var j in processingJobs)
                {
                    CoursePrintWorkflow.StepJobProgress(j.JobId, 25);
                }
            };

            pnlBoardToolbar.Controls.AddRange(new Control[] { btnAddNewJob, btnStepBatch });

            pnlWorkflowView.Controls.Add(tlpColumns);
            pnlWorkflowView.Controls.Add(pnlBoardToolbar);
        }

        private Panel BuildKanbanColumn(string headerTitle, Color headerColor, out FlowLayoutPanel cardContainer)
        {
            Panel pnl = new Panel();
            pnl.Dock = DockStyle.Fill;
            pnl.Margin = new Padding(6);
            pnl.BackColor = Color.FromArgb(243, 244, 246);
            pnl.Padding = new Padding(2);

            // Column Header
            Panel pnlH = new Panel();
            pnlH.Dock = DockStyle.Top;
            pnlH.Height = 40;
            pnlH.BackColor = headerColor;
            pnlH.Padding = new Padding(12, 8, 12, 8);

            Label lblH = new Label();
            lblH.Text = headerTitle;
            lblH.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
            lblH.ForeColor = Color.White;
            lblH.Dock = DockStyle.Fill;
            lblH.TextAlign = ContentAlignment.MiddleLeft;
            pnlH.Controls.Add(lblH);

            cardContainer = new FlowLayoutPanel();
            cardContainer.Dock = DockStyle.Fill;
            cardContainer.AutoScroll = true;
            cardContainer.FlowDirection = FlowDirection.TopDown;
            cardContainer.WrapContents = false;
            cardContainer.BackColor = Color.FromArgb(243, 244, 246);
            cardContainer.Padding = new Padding(8);

            pnl.Controls.Add(cardContainer);
            pnl.Controls.Add(pnlH);
            return pnl;
        }

        private void LoadWorkflowData()
        {
            RefreshWorkflowBoard();
        }

        private void RefreshWorkflowBoard()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(RefreshWorkflowBoard));
                return;
            }

            pnlColTodo.Controls.Clear();
            pnlColProcessing.Controls.Clear();
            pnlColPaused.Controls.Clear();
            pnlColDone.Controls.Clear();

            List<CoursePrintJob> jobs = CoursePrintWorkflow.GetAllJobs();

            int countTodo = 0;
            int countProcessing = 0;
            int countPaused = 0;
            int countDone = 0;
            int totalDocsPrinted = 0;

            foreach (CoursePrintJob job in jobs)
            {
                totalDocsPrinted += job.ProcessedCount;
                Control card = CreateCourseJobCard(job);

                switch (job.Status)
                {
                    case PrintJobStatus.Queued:
                        pnlColTodo.Controls.Add(card);
                        countTodo++;
                        break;
                    case PrintJobStatus.Processing:
                        pnlColProcessing.Controls.Add(card);
                        countProcessing++;
                        break;
                    case PrintJobStatus.Paused:
                        pnlColPaused.Controls.Add(card);
                        countPaused++;
                        break;
                    case PrintJobStatus.Completed:
                        pnlColDone.Controls.Add(card);
                        countDone++;
                        break;
                }
            }

            // Update KPI metric labels
            lblKpiUpcoming.Text = countTodo + " Lots";
            lblKpiProcessing.Text = countProcessing + " Active";
            lblKpiTotalPrinted.Text = totalDocsPrinted.ToString("N0") + " Docs";
            int totalLots = jobs.Count;
            double completionRate = totalLots > 0 ? ((double)countDone / totalLots * 100.0) : 0;
            lblKpiResolved.Text = completionRate.ToString("0.0") + "%";
        }

        private Control CreateCourseJobCard(CoursePrintJob job)
        {
            Panel card = new Panel();
            card.Size = new Size(245, 175);
            card.BackColor = Color.White;
            card.Margin = new Padding(0, 0, 0, 10);
            card.Padding = new Padding(12);

            // Card Border and shadow
            card.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (Pen p = new Pen(Color.FromArgb(226, 232, 240), 1))
                    e.Graphics.DrawRectangle(p, 0, 0, card.Width - 1, card.Height - 1);
            };

            // Badge & DocType
            Label lblBadge = new Label();
            lblBadge.Text = job.DocType.ToUpper() + " • " + job.ExamYear;
            lblBadge.Font = new Font("Segoe UI", 7.5f, FontStyle.Bold);
            lblBadge.ForeColor = Color.FromArgb(79, 70, 229);
            lblBadge.Location = new Point(10, 8);
            lblBadge.Size = new Size(150, 16);

            Label lblJobId = new Label();
            lblJobId.Text = job.JobId;
            lblJobId.Font = new Font("Segoe UI", 7.5f, FontStyle.Regular);
            lblJobId.ForeColor = Color.FromArgb(148, 163, 184);
            lblJobId.Location = new Point(160, 8);
            lblJobId.Size = new Size(75, 16);
            lblJobId.TextAlign = ContentAlignment.TopRight;

            // Course Title
            Label lblTitle = new Label();
            lblTitle.Text = job.CourseTitle;
            lblTitle.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(15, 23, 42);
            lblTitle.Location = new Point(10, 26);
            lblTitle.Size = new Size(225, 34);

            // Progress text: e.g. "240 / 426 printed (56%)"
            Label lblProgress = new Label();
            lblProgress.Text = string.Format("{0} / {1} printed ({2}%)", job.ProcessedCount, job.TotalExaminees, job.ProgressPercent);
            lblProgress.Font = new Font("Segoe UI", 8f, FontStyle.Regular);
            lblProgress.ForeColor = Color.FromArgb(71, 85, 105);
            lblProgress.Location = new Point(10, 62);
            lblProgress.Size = new Size(225, 16);

            // Progress Bar Panel
            Panel pnlProgressBg = new Panel();
            pnlProgressBg.Location = new Point(10, 80);
            pnlProgressBg.Size = new Size(225, 8);
            pnlProgressBg.BackColor = Color.FromArgb(226, 232, 240);
            pnlProgressBg.Paint += (s, e) =>
            {
                int fillWidth = (int)(pnlProgressBg.Width * (job.ProgressPercent / 100.0));
                if (fillWidth > 0)
                {
                    Color fillCol = job.Status == PrintJobStatus.Completed ? Color.FromArgb(5, 150, 105) :
                                    job.Status == PrintJobStatus.Paused ? Color.FromArgb(220, 38, 38) :
                                    Color.FromArgb(16, 185, 129);
                    using (SolidBrush b = new SolidBrush(fillCol))
                        e.Graphics.FillRectangle(b, 0, 0, fillWidth, pnlProgressBg.Height);
                }
            };

            // Lot and Assignee note
            Label lblMeta = new Label();
            lblMeta.Text = string.Format("Lot: {0} | User: {1}", job.LotNumber, job.AssignedUser);
            lblMeta.Font = new Font("Segoe UI", 7.5f, FontStyle.Italic);
            lblMeta.ForeColor = Color.FromArgb(100, 116, 139);
            lblMeta.Location = new Point(10, 93);
            lblMeta.Size = new Size(225, 16);

            // Action Buttons Panel (Start, Pause, Resume, Get Report)
            FlowLayoutPanel pnlActions = new FlowLayoutPanel();
            pnlActions.Location = new Point(10, 114);
            pnlActions.Size = new Size(225, 52);
            pnlActions.FlowDirection = FlowDirection.LeftToRight;
            pnlActions.WrapContents = true;
            pnlActions.Padding = new Padding(0);

            // Start Button (Visible on Queued)
            if (job.Status == PrintJobStatus.Queued)
            {
                Button btnStart = CreateActionButton("▶ Start", Color.FromArgb(5, 150, 105), 65);
                btnStart.Click += (s, e) =>
                {
                    if (!UserSession.CanPrint)
                    {
                        MessageBox.Show("RBAC: You do not have permission to start printing jobs.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    CoursePrintWorkflow.StartJob(job.JobId);
                };
                pnlActions.Controls.Add(btnStart);
            }

            // Pause Button (Visible on Processing)
            if (job.Status == PrintJobStatus.Processing)
            {
                Button btnPause = CreateActionButton("⏸ Pause", Color.FromArgb(220, 38, 38), 65);
                btnPause.Click += (s, e) => CoursePrintWorkflow.PauseJob(job.JobId);
                pnlActions.Controls.Add(btnPause);

                Button btnDone = CreateActionButton("✔ Finish", Color.FromArgb(5, 150, 105), 65);
                btnDone.Click += (s, e) => CoursePrintWorkflow.MarkJobCompleted(job.JobId);
                pnlActions.Controls.Add(btnDone);
            }

            // Resume Button (Visible on Paused)
            if (job.Status == PrintJobStatus.Paused)
            {
                Button btnResume = CreateActionButton("⏯ Resume", Color.FromArgb(217, 119, 6), 72);
                btnResume.Click += (s, e) =>
                {
                    if (!UserSession.CanPrint)
                    {
                        MessageBox.Show("RBAC: You do not have permission to resume printing jobs.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    CoursePrintWorkflow.ResumeJob(job.JobId);
                };
                pnlActions.Controls.Add(btnResume);
            }

            // Report Button (Always available to inspect lot)
            Button btnReport = CreateActionButton("📜 Report", Color.FromArgb(30, 58, 138), 75);
            btnReport.Click += (s, e) =>
            {
                string reportHtml = CoursePrintWorkflow.GenerateJobReportHtml(job);
                ShowHtmlReportPreview(reportHtml, "Printing Lot Audit: " + job.JobId);
            };
            pnlActions.Controls.Add(btnReport);

            card.Controls.AddRange(new Control[] { lblBadge, lblJobId, lblTitle, lblProgress, pnlProgressBg, lblMeta, pnlActions });
            return card;
        }

        private Button CreateActionButton(string text, Color col, int width)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.Font = new Font("Segoe UI", 7.5f, FontStyle.Bold);
            btn.BackColor = col;
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Size = new Size(width, 26);
            btn.Margin = new Padding(0, 0, 4, 4);
            btn.Cursor = Cursors.Hand;
            return btn;
        }

        private void PromptCreateNewJob()
        {
            if (!UserSession.CanPrint && !UserSession.IsAdmin)
            {
                MessageBox.Show("RBAC: Only Print operators or Admins can queue new course print jobs.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (Form dlg = new Form())
            {
                dlg.Text = "Queue New Course Printing Lot";
                dlg.Size = new Size(460, 400);
                dlg.StartPosition = FormStartPosition.CenterParent;
                dlg.FormBorderStyle = FormBorderStyle.FixedDialog;
                dlg.MaximizeBox = false;
                dlg.MinimizeBox = false;

                Label lblC = new Label { Text = "Course Selection:", Location = new Point(20, 20), AutoSize = true };
                ComboBox cboCourse = new ComboBox { Location = new Point(20, 42), Size = new Size(400, 25), DropDownStyle = ComboBoxStyle.DropDownList };
                foreach (var c in CourseDesignRepository.LoadDesigns())
                {
                    cboCourse.Items.Add(c.CourseKey + " - " + c.DisplayName);
                }
                if (cboCourse.Items.Count > 0) cboCourse.SelectedIndex = 0;

                Label lblDoc = new Label { Text = "Document Type:", Location = new Point(20, 80), AutoSize = true };
                ComboBox cboDoc = new ComboBox { Location = new Point(20, 102), Size = new Size(400, 25), DropDownStyle = ComboBoxStyle.DropDownList };
                cboDoc.Items.AddRange(new object[] { "Transcript", "Certificate", "Both (Transcript + Certificate)" });
                cboDoc.SelectedIndex = 0;

                Label lblYr = new Label { Text = "Exam Year:", Location = new Point(20, 140), AutoSize = true };
                TextBox txtYr = new TextBox { Text = DateTime.Now.Year.ToString(), Location = new Point(20, 162), Size = new Size(180, 25) };

                Label lblLot = new Label { Text = "Lot Sequence Name:", Location = new Point(220, 140), AutoSize = true };
                TextBox txtLot = new TextBox { Text = "LOT-" + DateTime.Now.ToString("yyMM-01"), Location = new Point(220, 162), Size = new Size(200, 25) };

                Label lblTotal = new Label { Text = "Total Examinees (Count):", Location = new Point(20, 200), AutoSize = true };
                NumericUpDown nudCount = new NumericUpDown { Location = new Point(20, 222), Size = new Size(180, 25), Minimum = 1, Maximum = 100000, Value = 150 };

                Label lblNotes = new Label { Text = "Job Notes / Stored Procedure:", Location = new Point(20, 255), AutoSize = true };
                TextBox txtNotes = new TextBox { Text = "SP: [dbo].[BBAMCPCTRANS_lot]", Location = new Point(20, 275), Size = new Size(400, 25) };

                Button btnOk = new Button { Text = "Queue Lot", DialogResult = DialogResult.OK, Location = new Point(220, 315), Size = new Size(100, 32), BackColor = Color.FromArgb(5, 150, 105), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
                Button btnCancel = new Button { Text = "Cancel", DialogResult = DialogResult.Cancel, Location = new Point(330, 315), Size = new Size(90, 32) };

                dlg.Controls.AddRange(new Control[] { lblC, cboCourse, lblDoc, cboDoc, lblYr, txtYr, lblLot, txtLot, lblTotal, nudCount, lblNotes, txtNotes, btnOk, btnCancel });

                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    string selCourse = cboCourse.SelectedItem.ToString();
                    string key = selCourse.Split('-')[0].Trim();
                    string title = selCourse.Substring(selCourse.IndexOf('-') + 1).Trim();

                    CoursePrintJob newJob = new CoursePrintJob(
                        "JOB-" + new Random().Next(1000, 9999),
                        key, title, cboDoc.SelectedItem.ToString(),
                        txtYr.Text.Trim(), txtLot.Text.Trim(),
                        (int)nudCount.Value, 0, PrintJobStatus.Queued,
                        UserSession.CurrentUser != null ? UserSession.CurrentUser.Username : "admin",
                        txtNotes.Text.Trim()
                    );

                    CoursePrintWorkflow.AddJob(newJob);
                }
            }
        }

        private void ShowHtmlReportPreview(string html, string title)
        {
            Form frm = new Form();
            frm.Text = title;
            frm.Size = new Size(900, 700);
            frm.StartPosition = FormStartPosition.CenterParent;

            WebBrowser wb = new WebBrowser();
            wb.Dock = DockStyle.Fill;
            wb.DocumentText = html;
            frm.Controls.Add(wb);

            frm.ShowDialog(this);
        }
        #endregion

        #region User Credentials & RBAC Permission Management (Image 2 Implementation)
        private void BuildUserAdminView()
        {
            pnlUserAdminView = new Panel();
            pnlUserAdminView.Dock = DockStyle.Fill;
            pnlUserAdminView.BackColor = ColBoardBg;

            // Split into Top Grid & Bottom User Edit Form
            SplitContainer split = new SplitContainer();
            split.Dock = DockStyle.Fill;
            split.Orientation = Orientation.Horizontal;
            split.SplitterDistance = 320;
            split.Panel1.Padding = new Padding(0, 0, 0, 10);
            split.Panel2.Padding = new Padding(0, 10, 0, 0);

            // DataGridView for User Management
            dgvUsers = new DataGridView();
            dgvUsers.Dock = DockStyle.Fill;
            dgvUsers.BackgroundColor = Color.White;
            dgvUsers.BorderStyle = BorderStyle.None;
            dgvUsers.RowHeadersVisible = false;
            dgvUsers.AllowUserToAddRows = false;
            dgvUsers.AllowUserToDeleteRows = false;
            dgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsers.MultiSelect = false;
            dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUsers.CellClick += DgvUsers_CellClick;

            // Define Columns
            dgvUsers.Columns.Add("Username", "User ID");
            dgvUsers.Columns.Add("FullName", "Full Name / Designation");
            dgvUsers.Columns.Add("Role", "Assigned Role");
            dgvUsers.Columns.Add("Department", "Branch / Section");

            DataGridViewCheckBoxColumn colView = new DataGridViewCheckBoxColumn { HeaderText = "View Only", Name = "CanView", FillWeight = 60 };
            DataGridViewCheckBoxColumn colPrint = new DataGridViewCheckBoxColumn { HeaderText = "Print", Name = "CanPrint", FillWeight = 60 };
            DataGridViewCheckBoxColumn colEdit = new DataGridViewCheckBoxColumn { HeaderText = "Edit", Name = "CanEdit", FillWeight = 60 };
            DataGridViewCheckBoxColumn colAdmin = new DataGridViewCheckBoxColumn { HeaderText = "Admin", Name = "CanAdmin", FillWeight = 60 };

            dgvUsers.Columns.Add(colView);
            dgvUsers.Columns.Add(colPrint);
            dgvUsers.Columns.Add(colEdit);
            dgvUsers.Columns.Add(colAdmin);

            split.Panel1.Controls.Add(dgvUsers);

            // User Editor Panel
            Panel pnlEditUser = new Panel();
            pnlEditUser.Dock = DockStyle.Fill;
            pnlEditUser.BackColor = Color.White;
            pnlEditUser.Padding = new Padding(20);
            pnlEditUser.AutoScroll = true;
            pnlEditUser.Paint += (s, e) =>
            {
                using (Pen p = new Pen(Color.FromArgb(226, 232, 240), 1))
                    e.Graphics.DrawRectangle(p, 0, 0, pnlEditUser.Width - 1, pnlEditUser.Height - 1);
            };

            Label lblFormTitle = new Label { Text = "User Credentials & Role-Based Permissions (RBAC)", Font = new Font("Segoe UI", 11f, FontStyle.Bold), ForeColor = Color.FromArgb(15, 23, 42), Location = new Point(20, 12), AutoSize = true };

            // Field inputs
            Label lblU = new Label { Text = "Username / Solver ID:", Location = new Point(20, 42), AutoSize = true };
            txtUsername = new TextBox { Location = new Point(20, 62), Size = new Size(160, 25) };

            Label lblP = new Label { Text = "Passkey / Password:", Location = new Point(200, 42), AutoSize = true };
            txtPassword = new TextBox { Location = new Point(200, 62), Size = new Size(160, 25), UseSystemPasswordChar = true };

            Label lblN = new Label { Text = "Official Full Name:", Location = new Point(380, 42), AutoSize = true };
            txtFullName = new TextBox { Location = new Point(380, 62), Size = new Size(240, 25) };

            Label lblR = new Label { Text = "System Role:", Location = new Point(640, 42), AutoSize = true };
            cboRole = new ComboBox { Location = new Point(640, 62), Size = new Size(140, 25), DropDownStyle = ComboBoxStyle.DropDownList };
            cboRole.Items.AddRange(new object[] { "Admin", "Operator", "Editor", "Viewer", "Officer" });
            cboRole.SelectedIndex = 1;
            cboRole.SelectedIndexChanged += CboRole_SelectedIndexChanged;

            // Permissions Checkboxes
            GroupBox grpPerms = new GroupBox();
            grpPerms.Text = "Access Rights & Security Scope";
            grpPerms.Location = new Point(20, 95);
            grpPerms.Size = new Size(760, 65);

            chkViewOnly = new CheckBox { Text = "👁 View Only (Preview, Verify)", Location = new Point(15, 24), AutoSize = true };
            chkCanPrint = new CheckBox { Text = "🖨 Can Print (Generate & Spool Docs)", Location = new Point(210, 24), AutoSize = true };
            chkCanEdit = new CheckBox { Text = "✏ Can Edit (Templates, DB, Fonts)", Location = new Point(440, 24), AutoSize = true };
            chkCanAdmin = new CheckBox { Text = "👑 Full Admin (User RBAC)", Location = new Point(640, 24), AutoSize = true };

            grpPerms.Controls.AddRange(new Control[] { chkViewOnly, chkCanPrint, chkCanEdit, chkCanAdmin });

            // Action Buttons
            btnNewUser = new Button { Text = "➕ Clear / New", Location = new Point(20, 172), Size = new Size(110, 32) };
            btnNewUser.Click += (s, e) => ClearUserFields();

            btnAddUser = new Button { Text = "💾 Add User", Location = new Point(140, 172), Size = new Size(110, 32), BackColor = Color.FromArgb(5, 150, 105), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnAddUser.Click += BtnAddUser_Click;

            btnUpdateUser = new Button { Text = "🔄 Update User", Location = new Point(260, 172), Size = new Size(120, 32), BackColor = Color.FromArgb(30, 58, 138), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnUpdateUser.Click += BtnUpdateUser_Click;

            btnDeleteUser = new Button { Text = "❌ Delete User", Location = new Point(390, 172), Size = new Size(110, 32), BackColor = Color.FromArgb(220, 38, 38), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnDeleteUser.Click += BtnDeleteUser_Click;

            lblUserStatus = new Label { Text = "Ready. Select a user above to inspect or edit permissions.", ForeColor = Color.FromArgb(100, 116, 139), Location = new Point(520, 179), AutoSize = true };

            pnlEditUser.Controls.AddRange(new Control[] { 
                lblFormTitle, lblU, txtUsername, lblP, txtPassword, lblN, txtFullName, lblR, cboRole,
                grpPerms, btnNewUser, btnAddUser, btnUpdateUser, btnDeleteUser, lblUserStatus 
            });

            split.Panel2.Controls.Add(pnlEditUser);
            pnlUserAdminView.Controls.Add(split);
        }

        private void CboRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            string role = cboRole.SelectedItem != null ? cboRole.SelectedItem.ToString() : "";
            if (role == "Admin")
            {
                chkViewOnly.Checked = true;
                chkCanPrint.Checked = true;
                chkCanEdit.Checked = true;
                chkCanAdmin.Checked = true;
            }
            else if (role == "Operator" || role == "Officer")
            {
                chkViewOnly.Checked = true;
                chkCanPrint.Checked = true;
                chkCanEdit.Checked = false;
                chkCanAdmin.Checked = false;
            }
            else if (role == "Editor")
            {
                chkViewOnly.Checked = true;
                chkCanPrint.Checked = false;
                chkCanEdit.Checked = true;
                chkCanAdmin.Checked = false;
            }
            else if (role == "Viewer")
            {
                chkViewOnly.Checked = true;
                chkCanPrint.Checked = false;
                chkCanEdit.Checked = false;
                chkCanAdmin.Checked = false;
            }
        }

        private void LoadUserData()
        {
            dgvUsers.Rows.Clear();
            List<UserAccount> users = UserPermissionManager.GetAllUsers();
            foreach (UserAccount u in users)
            {
                dgvUsers.Rows.Add(u.Username, u.FullName, u.Role, u.Department, u.CanView, u.CanPrint, u.CanEdit, u.CanManageUsers);
            }
        }

        private void DgvUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvUsers.Rows.Count)
            {
                DataGridViewRow row = dgvUsers.Rows[e.RowIndex];
                string uname = row.Cells["Username"].Value.ToString();
                UserAccount user = UserPermissionManager.GetAllUsers().Find(u => u.Username.Equals(uname, StringComparison.OrdinalIgnoreCase));
                if (user != null)
                {
                    txtUsername.Text = user.Username;
                    txtPassword.Text = user.Password;
                    txtFullName.Text = user.FullName;
                    cboRole.SelectedItem = user.Role;
                    chkViewOnly.Checked = user.CanView;
                    chkCanPrint.Checked = user.CanPrint;
                    chkCanEdit.Checked = user.CanEdit;
                    chkCanAdmin.Checked = user.CanManageUsers;
                    lblUserStatus.Text = "Loaded credentials for " + user.Username;
                }
            }
        }

        private void ClearUserFields()
        {
            txtUsername.Text = "";
            txtPassword.Text = "";
            txtFullName.Text = "";
            cboRole.SelectedIndex = 1; // Operator
            chkViewOnly.Checked = true;
            chkCanPrint.Checked = true;
            chkCanEdit.Checked = false;
            chkCanAdmin.Checked = false;
            lblUserStatus.Text = "Ready for new user input.";
        }

        private void BtnAddUser_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUsername.Text.Trim()) || string.IsNullOrEmpty(txtPassword.Text.Trim()))
            {
                MessageBox.Show("Username and Password are required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            UserAccount u = new UserAccount(
                txtUsername.Text.Trim(),
                txtPassword.Text.Trim(),
                txtFullName.Text.Trim(),
                cboRole.SelectedItem != null ? cboRole.SelectedItem.ToString() : "Operator",
                chkViewOnly.Checked,
                chkCanPrint.Checked,
                chkCanEdit.Checked,
                chkCanAdmin.Checked,
                "National University Examination Cell"
            );

            UserPermissionManager.AddUser(u);
            LoadUserData();
            lblUserStatus.Text = "User '" + u.Username + "' created successfully.";
        }

        private void BtnUpdateUser_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUsername.Text.Trim())) return;

            UserAccount u = new UserAccount(
                txtUsername.Text.Trim(),
                txtPassword.Text.Trim(),
                txtFullName.Text.Trim(),
                cboRole.SelectedItem != null ? cboRole.SelectedItem.ToString() : "Operator",
                chkViewOnly.Checked,
                chkCanPrint.Checked,
                chkCanEdit.Checked,
                chkCanAdmin.Checked,
                "National University Examination Cell"
            );

            UserPermissionManager.UpdateUser(u);
            LoadUserData();
            lblUserStatus.Text = "User '" + u.Username + "' updated successfully.";
        }

        private void BtnDeleteUser_Click(object sender, EventArgs e)
        {
            string uname = txtUsername.Text.Trim();
            if (string.IsNullOrEmpty(uname)) return;
            if (uname.ToLower() == "admin")
            {
                MessageBox.Show("Cannot delete the primary root Administrator account.", "Security Constraint", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete user '" + uname + "'?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                UserPermissionManager.DeleteUser(uname);
                LoadUserData();
                ClearUserFields();
                lblUserStatus.Text = "User deleted.";
            }
        }
        #endregion
    }
}
