using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace NU_Solver
{
    [Serializable]
    public class CourseDesignConfig
    {
        public string CourseKey { get; set; }
        public string DisplayName { get; set; }
        public string DegreeConferred { get; set; }
        public string DisciplineSubject { get; set; }
        public string EntranceQualification { get; set; }
        public string DurationOfCourse { get; set; }
        public int SemestersCount { get; set; }
        public int TotalCredits { get; set; }
        public int TotalPapers { get; set; }
        public string PageSize { get; set; } // "A4" or "Legal"
        public string RefPrefix { get; set; }
        public string CertificateDegreeTitle { get; set; }
        public string CertificateSerialPrefix { get; set; }

        // Stored Procedure / Function Selection
        public string StoredProcedureName { get; set; }

        // RDLC Report File Customization
        public string RdlcFilePath { get; set; }

        // Crystal Reports (.rpt) Template Customization
        public string RptFilePath { get; set; }
        public string ReportEngineType { get; set; } // "Default", "CrystalReports", "RDLC"

        // Database Connection & Credentials
        public string DbServer { get; set; }
        public string DbName { get; set; }
        public string DbUser { get; set; }
        public string DbPassword { get; set; }

        // Typography & Font Customization (Crystal Reports style)
        public string FontStudentName { get; set; }
        public int FontSizeStudentName { get; set; }
        public string FontStyleStudentName { get; set; } // "Italic", "Bold", "Regular", "Bold Italic"
        public string FontDegreeTitle { get; set; }
        public int FontSizeDegreeTitle { get; set; }
        public string FontCertificateBody { get; set; }
        public int FontSizeCertificateBody { get; set; }
        public string FontTranscriptTable { get; set; }
        public int FontSizeTranscriptTable { get; set; }

        public CourseDesignConfig()
        {
            CourseKey = "MED";
            DisplayName = "M.Ed - Master of Education (2 Semesters, 44 Credits)";
            DegreeConferred = "MASTER OF EDUCATION";
            DisciplineSubject = "MASTER OF EDUCATION";
            EntranceQualification = "BACHELOR OF EDUCATION";
            DurationOfCourse = "01 (One) Year";
            SemestersCount = 2;
            TotalCredits = 44;
            TotalPapers = 12;
            PageSize = "A4";
            RefPrefix = "NU/Con/Certi/Trans/{EXM_CODE}/{EXM_YEAR}/";
            CertificateDegreeTitle = "Master of Education";
            CertificateSerialPrefix = "NUPC 20- ";

            // Stored Procedure, RDLC and Crystal Reports defaults
            StoredProcedureName = "dbo.BBAMCPCTRANS_lot";
            RdlcFilePath = "";
            RptFilePath = "";
            ReportEngineType = "Default";

            // Database defaults
            DbServer = "192.168.0.230";
            DbName = "m_ed";
            DbUser = "sa";
            DbPassword = "1234";

            // Typography defaults
            FontStudentName = "Brush Script MT";
            FontSizeStudentName = 24;
            FontStyleStudentName = "Italic";
            FontDegreeTitle = "Old English Text MT";
            FontSizeDegreeTitle = 32;
            FontCertificateBody = "Times New Roman";
            FontSizeCertificateBody = 15;
            FontTranscriptTable = "Segoe UI";
            FontSizeTranscriptTable = 10;
        }

        public string GetConnectionString()
        {
            if (string.IsNullOrEmpty(DbServer))
            {
                return "Data Source=192.168.0.230;Initial Catalog=" + (string.IsNullOrEmpty(DbName) ? "m_ed" : DbName) + ";User Id=sa;Password=1234;";
            }

            try
            {
                SqlConnectionStringBuilder b = new SqlConnectionStringBuilder();
                b.DataSource = DbServer.Trim();
                b.InitialCatalog = string.IsNullOrEmpty(DbName) ? "m_ed" : DbName.Trim();
                if (!string.IsNullOrEmpty(DbUser))
                {
                    b.UserID = DbUser.Trim();
                    b.Password = DbPassword ?? "";
                }
                else
                {
                    b.IntegratedSecurity = true;
                }
                return b.ConnectionString;
            }
            catch
            {
                return string.Format("Data Source={0};Initial Catalog={1};User Id={2};Password={3};",
                    DbServer ?? "192.168.0.230",
                    string.IsNullOrEmpty(DbName) ? "m_ed" : DbName,
                    DbUser ?? "sa",
                    DbPassword ?? "1234");
            }
        }

        public override string ToString()
        {
            return DisplayName ?? CourseKey;
        }
    }

    [Serializable]
    public class CourseDesignCatalog
    {
        public List<CourseDesignConfig> Designs { get; set; }

        public CourseDesignCatalog()
        {
            Designs = new List<CourseDesignConfig>();
        }
    }

    public static class CourseDesignRepository
    {
        private static string GetConfigFilePath()
        {
            string dir = TranscriptGenerator.GetSafeReportsDirectory();
            return Path.Combine(dir, "CourseDesigns.xml");
        }

        public static List<CourseDesignConfig> LoadDesigns()
        {
            string path = GetConfigFilePath();
            try
            {
                if (File.Exists(path))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(CourseDesignCatalog));
                    using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                    {
                        CourseDesignCatalog catalog = (CourseDesignCatalog)serializer.Deserialize(fs);
                        if (catalog != null && catalog.Designs != null && catalog.Designs.Count > 0)
                        {
                            // Ensure backward compatibility defaults for any new properties
                            foreach (var d in catalog.Designs)
                            {
                                if (string.IsNullOrEmpty(d.StoredProcedureName)) d.StoredProcedureName = "dbo.BBAMCPCTRANS_lot";
                                if (d.RdlcFilePath == null) d.RdlcFilePath = "";

                                if (string.IsNullOrEmpty(d.DbServer)) d.DbServer = "192.168.0.230";
                                if (string.IsNullOrEmpty(d.DbName)) d.DbName = "m_ed";
                                if (string.IsNullOrEmpty(d.DbUser)) d.DbUser = "sa";
                                if (string.IsNullOrEmpty(d.DbPassword)) d.DbPassword = "1234";

                                if (string.IsNullOrEmpty(d.FontStudentName)) d.FontStudentName = "Brush Script MT";
                                if (d.FontSizeStudentName <= 0) d.FontSizeStudentName = 24;
                                if (string.IsNullOrEmpty(d.FontStyleStudentName)) d.FontStyleStudentName = "Italic";
                                if (string.IsNullOrEmpty(d.FontDegreeTitle)) d.FontDegreeTitle = "Old English Text MT";
                                if (d.FontSizeDegreeTitle <= 0) d.FontSizeDegreeTitle = 32;
                                if (string.IsNullOrEmpty(d.FontCertificateBody)) d.FontCertificateBody = "Times New Roman";
                                if (d.FontSizeCertificateBody <= 0) d.FontSizeCertificateBody = 15;
                                if (string.IsNullOrEmpty(d.FontTranscriptTable)) d.FontTranscriptTable = "Segoe UI";
                                if (d.FontSizeTranscriptTable <= 0) d.FontSizeTranscriptTable = 10;
                                if (string.IsNullOrEmpty(d.ReportEngineType)) d.ReportEngineType = "Default";
                                if (d.RptFilePath == null) d.RptFilePath = "";
                            }
                            return catalog.Designs;
                        }
                    }
                }
            }
            catch { }

            // Default predefined designs
            List<CourseDesignConfig> defaults = GetDefaultDesigns();
            SaveDesigns(defaults);
            return defaults;
        }

        public static void SaveDesigns(List<CourseDesignConfig> designs)
        {
            try
            {
                string path = GetConfigFilePath();
                XmlSerializer serializer = new XmlSerializer(typeof(CourseDesignCatalog));
                CourseDesignCatalog catalog = new CourseDesignCatalog { Designs = designs };
                using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    serializer.Serialize(fs, catalog);
                }
            }
            catch { }
        }

        public static List<CourseDesignConfig> GetDefaultDesigns()
        {
            return new List<CourseDesignConfig>
            {
                new CourseDesignConfig
                {
                    CourseKey = "MED",
                    DisplayName = "M.Ed - Master of Education (2 Semesters, 44 Credits)",
                    DegreeConferred = "MASTER OF EDUCATION",
                    DisciplineSubject = "MASTER OF EDUCATION",
                    EntranceQualification = "BACHELOR OF EDUCATION",
                    DurationOfCourse = "01 (One) Year",
                    SemestersCount = 2,
                    TotalCredits = 44,
                    TotalPapers = 12,
                    PageSize = "A4",
                    RefPrefix = "NU/Con/Certi/Trans/{EXM_CODE}/{EXM_YEAR}/",
                    CertificateDegreeTitle = "Master of Education",
                    CertificateSerialPrefix = "NUPC 20- ",
                    DbServer = "192.168.0.230",
                    DbName = "m_ed",
                    DbUser = "sa",
                    DbPassword = "1234",
                    FontStudentName = "Brush Script MT",
                    FontSizeStudentName = 24,
                    FontStyleStudentName = "Italic",
                    FontDegreeTitle = "Old English Text MT",
                    FontSizeDegreeTitle = 32,
                    FontCertificateBody = "Times New Roman",
                    FontSizeCertificateBody = 15,
                    FontTranscriptTable = "Segoe UI",
                    FontSizeTranscriptTable = 10
                },
                new CourseDesignConfig
                {
                    CourseKey = "BED_HONS",
                    DisplayName = "B.Ed (Hons) - Bachelor of Education (8 Semesters, Legal)",
                    DegreeConferred = "BACHELOR OF EDUCATION",
                    DisciplineSubject = "Bachelor of Education",
                    EntranceQualification = "DEGREE (PASS) / GRADUATION",
                    DurationOfCourse = "04 (Four) Years",
                    SemestersCount = 8,
                    TotalCredits = 152,
                    TotalPapers = 40,
                    PageSize = "Legal",
                    RefPrefix = "NU/Con/Certi/Trans/{EXM_CODE}/{EXM_YEAR}/",
                    CertificateDegreeTitle = "Bachelor of Education",
                    CertificateSerialPrefix = "NUPC 20- ",
                    DbServer = "192.168.0.230",
                    DbName = "m_ed",
                    DbUser = "sa",
                    DbPassword = "1234",
                    FontStudentName = "Brush Script MT",
                    FontSizeStudentName = 24,
                    FontStyleStudentName = "Italic",
                    FontDegreeTitle = "Old English Text MT",
                    FontSizeDegreeTitle = 32,
                    FontCertificateBody = "Times New Roman",
                    FontSizeCertificateBody = 15,
                    FontTranscriptTable = "Segoe UI",
                    FontSizeTranscriptTable = 10
                },
                new CourseDesignConfig
                {
                    CourseKey = "BBA",
                    DisplayName = "BBA - Bachelor of Business Administration (8 Semesters, Legal)",
                    DegreeConferred = "BACHELOR OF BUSINESS ADMINISTRATION",
                    DisciplineSubject = "BUSINESS ADMINISTRATION",
                    EntranceQualification = "HIGHER SECONDARY CERTIFICATE",
                    DurationOfCourse = "04 (Four) Years",
                    SemestersCount = 8,
                    TotalCredits = 126,
                    TotalPapers = 40,
                    PageSize = "Legal",
                    RefPrefix = "NU/Con/Certi/Trans/{EXM_CODE}/{EXM_YEAR}/",
                    CertificateDegreeTitle = "Bachelor of Business Administration",
                    CertificateSerialPrefix = "NUPC 20- ",
                    DbServer = "192.168.0.230",
                    DbName = "m_ed",
                    DbUser = "sa",
                    DbPassword = "1234",
                    FontStudentName = "Brush Script MT",
                    FontSizeStudentName = 24,
                    FontStyleStudentName = "Italic",
                    FontDegreeTitle = "Old English Text MT",
                    FontSizeDegreeTitle = 32,
                    FontCertificateBody = "Times New Roman",
                    FontSizeCertificateBody = 15,
                    FontTranscriptTable = "Segoe UI",
                    FontSizeTranscriptTable = 10
                },
                new CourseDesignConfig
                {
                    CourseKey = "MBA",
                    DisplayName = "MBA - Master of Business Administration (4 Semesters)",
                    DegreeConferred = "MASTER OF BUSINESS ADMINISTRATION",
                    DisciplineSubject = "BUSINESS ADMINISTRATION",
                    EntranceQualification = "BACHELOR DEGREE",
                    DurationOfCourse = "02 (Two) Years",
                    SemestersCount = 4,
                    TotalCredits = 60,
                    TotalPapers = 20,
                    PageSize = "A4",
                    RefPrefix = "NU/Con/Certi/Trans/{EXM_CODE}/{EXM_YEAR}/",
                    CertificateDegreeTitle = "Master of Business Administration",
                    CertificateSerialPrefix = "NUPC 20- ",
                    DbServer = "192.168.0.230",
                    DbName = "m_ed",
                    DbUser = "sa",
                    DbPassword = "1234",
                    FontStudentName = "Brush Script MT",
                    FontSizeStudentName = 24,
                    FontStyleStudentName = "Italic",
                    FontDegreeTitle = "Old English Text MT",
                    FontSizeDegreeTitle = 32,
                    FontCertificateBody = "Times New Roman",
                    FontSizeCertificateBody = 15,
                    FontTranscriptTable = "Segoe UI",
                    FontSizeTranscriptTable = 10
                },
                new CourseDesignConfig
                {
                    CourseKey = "BED",
                    DisplayName = "B.Ed - Bachelor of Education (2 Semesters)",
                    DegreeConferred = "BACHELOR OF EDUCATION",
                    DisciplineSubject = "EDUCATION",
                    EntranceQualification = "DEGREE (PASS) / GRADUATION",
                    DurationOfCourse = "01 (One) Year",
                    SemestersCount = 2,
                    TotalCredits = 40,
                    TotalPapers = 10,
                    PageSize = "A4",
                    RefPrefix = "NU/Con/Certi/Trans/{EXM_CODE}/{EXM_YEAR}/",
                    CertificateDegreeTitle = "Bachelor of Education",
                    CertificateSerialPrefix = "NUPC 20- ",
                    DbServer = "192.168.0.230",
                    DbName = "m_ed",
                    DbUser = "sa",
                    DbPassword = "1234",
                    FontStudentName = "Brush Script MT",
                    FontSizeStudentName = 24,
                    FontStyleStudentName = "Italic",
                    FontDegreeTitle = "Old English Text MT",
                    FontSizeDegreeTitle = 32,
                    FontCertificateBody = "Times New Roman",
                    FontSizeCertificateBody = 15,
                    FontTranscriptTable = "Segoe UI",
                    FontSizeTranscriptTable = 10
                },
                new CourseDesignConfig
                {
                    CourseKey = "CSE",
                    DisplayName = "B.Sc (Hons) in CSE - Computer Science & Engineering (8 Semesters, Legal)",
                    DegreeConferred = "BACHELOR OF SCIENCE (HONOURS) IN CSE",
                    DisciplineSubject = "COMPUTER SCIENCE & ENGINEERING",
                    EntranceQualification = "HIGHER SECONDARY CERTIFICATE",
                    DurationOfCourse = "04 (Four) Years",
                    SemestersCount = 8,
                    TotalCredits = 144,
                    TotalPapers = 42,
                    PageSize = "Legal",
                    RefPrefix = "NU/Con/Certi/Trans/{EXM_CODE}/{EXM_YEAR}/",
                    CertificateDegreeTitle = "Bachelor of Science (Honours) in Computer Science & Engineering",
                    CertificateSerialPrefix = "NUPC 20- ",
                    DbServer = "192.168.0.230",
                    DbName = "m_ed",
                    DbUser = "sa",
                    DbPassword = "1234",
                    FontStudentName = "Brush Script MT",
                    FontSizeStudentName = 24,
                    FontStyleStudentName = "Italic",
                    FontDegreeTitle = "Old English Text MT",
                    FontSizeDegreeTitle = 32,
                    FontCertificateBody = "Times New Roman",
                    FontSizeCertificateBody = 15,
                    FontTranscriptTable = "Segoe UI",
                    FontSizeTranscriptTable = 10
                }
            };
        }
    }

    public class CourseDesignDialog : Form
    {
        private List<CourseDesignConfig> designs;
        private ListBox lstDesigns;
        private Button btnAdd;
        private Button btnDelete;
        private Button btnSave;
        private Button btnCancel;

        // Tab 1: Academic & Layout
        private TextBox txtCourseKey;
        private TextBox txtDisplayName;
        private TextBox txtDegreeConferred;
        private TextBox txtCertificateDegree;
        private TextBox txtDiscipline;
        private TextBox txtEntranceQual;
        private TextBox txtDuration;
        private NumericUpDown numSemesters;
        private NumericUpDown numCredits;
        private NumericUpDown numPapers;
        private ComboBox cmbPageSize;
        private TextBox txtRefPrefix;
        private TextBox txtCertPrefix;

        // Tab 2: Database Connection
        private TextBox txtDbServer;
        private TextBox txtDbName;
        private TextBox txtDbUser;
        private TextBox txtDbPassword;
        private CheckBox chkShowPassword;
        private ComboBox cmbStoredProc;
        private TextBox txtConnPreview;
        private Button btnTestConnection;
        private Label lblConnStatus;

        // Tab 4: Report Templates (.rpt, .rdlc, Built-in)
        private RadioButton rbDefaultEngine;
        private RadioButton rbCustomRpt;
        private RadioButton rbCustomRdlc;
        private TextBox txtRptFilePath;
        private Button btnBrowseRpt;
        private Button btnOpenRptDesigner;
        private Button btnExportXsd;
        private Button btnTestLoadRpt;
        private TextBox txtRdlcFilePath;
        private Button btnBrowseRdlc;
        private Button btnClearReportEngine;

        // Tab 3: Typography & Fonts (Crystal Reports style)
        private ComboBox cmbFontStudentName;
        private NumericUpDown numFontSizeStudentName;
        private ComboBox cmbFontStyleStudentName;
        private ComboBox cmbFontDegreeTitle;
        private NumericUpDown numFontSizeDegreeTitle;
        private ComboBox cmbFontCertBody;
        private NumericUpDown numFontSizeCertBody;
        private ComboBox cmbFontTransTable;
        private NumericUpDown numFontSizeTransTable;
        private Panel pnlLivePreview;
        private Label lblPreviewStudentName;
        private Label lblPreviewDegreeTitle;
        private Label lblPreviewCertBody;

        public CourseDesignConfig SelectedDesign { get; private set; }

        public CourseDesignDialog() : this(null) { }

        public CourseDesignDialog(CourseDesignConfig current)
        {
            InitializeComponent();

            designs = CourseDesignRepository.LoadDesigns();
            PopulateList();

            if (current != null)
            {
                for (int i = 0; i < lstDesigns.Items.Count; i++)
                {
                    CourseDesignConfig d = lstDesigns.Items[i] as CourseDesignConfig;
                    if (d != null && string.Equals(d.CourseKey, current.CourseKey, StringComparison.OrdinalIgnoreCase))
                    {
                        lstDesigns.SelectedIndex = i;
                        break;
                    }
                }
            }
            if (lstDesigns.SelectedIndex < 0 && lstDesigns.Items.Count > 0)
            {
                lstDesigns.SelectedIndex = 0;
            }
        }

        private void InitializeComponent()
        {
            this.Text = "Admin Course Design Customization & Database Settings";
            this.Size = new Size(1050, 700);
            this.MinimumSize = new Size(960, 620);
            this.StartPosition = FormStartPosition.CenterParent;
            this.Font = UITheme.FontMain;
            this.BackColor = Color.FromArgb(248, 250, 252);

            // ==========================================
            // TOP HEADER BANNER (Single Clean Header)
            // ==========================================
            Panel pnlTopHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 62,
                BackColor = VectorGraphics.ColorBlueHeader
            };
            pnlTopHeader.Paint += (s, e) =>
            {
                VectorGraphics.PaintHeaderBanner(
                    e.Graphics,
                    new Rectangle(0, 0, pnlTopHeader.Width, pnlTopHeader.Height),
                    "⚙ COURSE DESIGN, DATABASE CREDENTIALS & TYPOGRAPHY",
                    "Configure academic layout, SQL database server & password, and Crystal Reports-style font templates.");
            };

            // ==========================================
            // LEFT PROFILES SIDEBAR
            // ==========================================
            Panel pnlLeft = new Panel
            {
                Dock = DockStyle.Left,
                Width = 270,
                Padding = new Padding(12),
                BackColor = Color.White
            };

            Label lblList = new Label
            {
                Text = "Available Course Profiles:",
                Font = UITheme.FontBold,
                ForeColor = VectorGraphics.ColorBlueDark,
                Dock = DockStyle.Top,
                Height = 26,
                UseMnemonic = false
            };

            FlowLayoutPanel pnlListBtns = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 44,
                WrapContents = false,
                Padding = new Padding(0, 4, 0, 0)
            };

            btnAdd = new Button
            {
                Text = "➕ Add New",
                Size = new Size(118, 34),
                Margin = new Padding(0, 2, 8, 0),
                Font = new Font("Segoe UI", 9f, FontStyle.Bold)
            };
            VectorGraphics.ApplyModernButton(btnAdd, VectorGraphics.ColorGreenMain, VectorGraphics.ColorGreenDark, VectorGraphics.ColorGreenHover, Color.White);
            btnAdd.Click += BtnAdd_Click;

            btnDelete = new Button
            {
                Text = "🗑 Delete",
                Size = new Size(118, 34),
                Margin = new Padding(0, 2, 0, 0),
                Font = new Font("Segoe UI", 9f)
            };
            VectorGraphics.ApplyModernButton(btnDelete, Color.FromArgb(241, 245, 249), Color.FromArgb(226, 232, 240), Color.FromArgb(254, 226, 226), VectorGraphics.ColorMagentaDark);
            btnDelete.Click += BtnDelete_Click;

            pnlListBtns.Controls.Add(btnAdd);
            pnlListBtns.Controls.Add(btnDelete);

            lstDesigns = new ListBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9.5f),
                BorderStyle = BorderStyle.FixedSingle
            };
            lstDesigns.SelectedIndexChanged += LstDesigns_SelectedIndexChanged;

            pnlLeft.Controls.Add(lstDesigns);
            pnlLeft.Controls.Add(pnlListBtns);
            pnlLeft.Controls.Add(lblList);

            // ==========================================
            // CENTER / RIGHT TABBED CONFIGURATOR
            // ==========================================
            TabControl tabConfig = new TabControl
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Padding = new Point(12, 6)
            };

            TabPage tabAcademic = new TabPage("Academic & Layout");
            tabAcademic.BackColor = Color.White;
            tabAcademic.Padding = new Padding(16);

            TabPage tabDatabase = new TabPage("Database Credentials");
            tabDatabase.BackColor = Color.White;
            tabDatabase.Padding = new Padding(16);

            TabPage tabTypography = new TabPage("Crystal Reports Typography");
            tabTypography.BackColor = Color.White;
            tabTypography.Padding = new Padding(16);

            // ------------------------------------------
            // TAB 1: ACADEMIC & LAYOUT
            // ------------------------------------------
            Panel pnlAcademicScroll = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true
            };

            int y = 10;
            int lblW = 180;
            int txtW = 380;

            Action<string, Control> addAcademicRow = (label, ctrl) =>
            {
                Label l = new Label
                {
                    Text = label,
                    Font = UITheme.FontMain,
                    ForeColor = VectorGraphics.ColorBlueDark,
                    Location = new Point(14, y + 3),
                    Width = lblW
                };
                ctrl.Location = new Point(14 + lblW + 8, y);
                if (ctrl.Width == 0 || ctrl is TextBox) ctrl.Width = txtW;
                pnlAcademicScroll.Controls.Add(l);
                pnlAcademicScroll.Controls.Add(ctrl);
                y += 34;
            };

            txtCourseKey = new TextBox();
            addAcademicRow("Course Key / Code:", txtCourseKey);

            txtDisplayName = new TextBox();
            addAcademicRow("Display Name:", txtDisplayName);

            txtDegreeConferred = new TextBox();
            addAcademicRow("Degree Conferred:", txtDegreeConferred);

            txtCertificateDegree = new TextBox();
            addAcademicRow("Certificate Degree Title:", txtCertificateDegree);

            txtDiscipline = new TextBox();
            addAcademicRow("Discipline / Subject:", txtDiscipline);

            txtEntranceQual = new TextBox();
            addAcademicRow("Entrance Qualification:", txtEntranceQual);

            txtDuration = new TextBox();
            addAcademicRow("Duration of Course:", txtDuration);

            numSemesters = new NumericUpDown { Minimum = 1, Maximum = 12, Value = 2, Width = 80 };
            addAcademicRow("Semesters Count:", numSemesters);

            numCredits = new NumericUpDown { Minimum = 1, Maximum = 300, Value = 44, Width = 80 };
            addAcademicRow("Total Credits:", numCredits);

            numPapers = new NumericUpDown { Minimum = 1, Maximum = 100, Value = 12, Width = 80 };
            addAcademicRow("Total Papers:", numPapers);

            cmbPageSize = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 110 };
            cmbPageSize.Items.AddRange(new object[] { "A4", "Legal" });
            cmbPageSize.SelectedIndex = 0;
            addAcademicRow("Transcript Page Size:", cmbPageSize);

            txtRefPrefix = new TextBox();
            addAcademicRow("Reference Prefix:", txtRefPrefix);

            txtCertPrefix = new TextBox();
            addAcademicRow("Certificate Serial Prefix:", txtCertPrefix);

            tabAcademic.Controls.Add(pnlAcademicScroll);

            // ------------------------------------------
            // TAB 2: DATABASE CREDENTIALS & PASSWORD
            // ------------------------------------------
            Panel pnlDbContainer = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true
            };

            GroupBox grpDb = new GroupBox
            {
                Text = "SQL Server Source & Authentication",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = VectorGraphics.ColorBlueHeader,
                Location = new Point(16, 12),
                Size = new Size(620, 240)
            };

            int dbY = 28;
            Action<string, Control> addDbRow = (label, ctrl) =>
            {
                Label l = new Label
                {
                    Text = label,
                    Font = UITheme.FontMain,
                    ForeColor = VectorGraphics.ColorBlueDark,
                    Location = new Point(20, dbY + 3),
                    Width = 160
                };
                ctrl.Location = new Point(190, dbY);
                if (ctrl.Width == 0 || ctrl is TextBox) ctrl.Width = 380;
                grpDb.Controls.Add(l);
                grpDb.Controls.Add(ctrl);
                dbY += 34;
            };

            txtDbServer = new TextBox { Text = "192.168.0.230" };
            addDbRow("Data Source (Server / IP):", txtDbServer);
            txtDbServer.TextChanged += UpdateConnPreview;

            txtDbName = new TextBox { Text = "m_ed" };
            addDbRow("Initial Catalog (Database):", txtDbName);
            txtDbName.TextChanged += UpdateConnPreview;

            txtDbUser = new TextBox { Text = "sa" };
            addDbRow("SQL User ID / Login:", txtDbUser);
            txtDbUser.TextChanged += UpdateConnPreview;

            txtDbPassword = new TextBox { Text = "1234", UseSystemPasswordChar = true, Width = 280 };
            grpDb.Controls.Add(new Label
            {
                Text = "SQL Password:",
                Font = UITheme.FontMain,
                ForeColor = VectorGraphics.ColorBlueDark,
                Location = new Point(20, dbY + 3),
                Width = 160
            });
            txtDbPassword.Location = new Point(190, dbY);
            txtDbPassword.TextChanged += UpdateConnPreview;
            grpDb.Controls.Add(txtDbPassword);

            chkShowPassword = new CheckBox
            {
                Text = "👁 Show",
                Font = new Font("Segoe UI", 8.5f),
                Location = new Point(480, dbY + 2),
                Width = 90,
                Cursor = Cursors.Hand
            };
            chkShowPassword.CheckedChanged += (s, e) => txtDbPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
            grpDb.Controls.Add(chkShowPassword);
            dbY += 34;

            // Stored Procedure / Function selector
            cmbStoredProc = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDown,
                Font = new Font("Segoe UI", 9f),
                Width = 380
            };
            cmbStoredProc.Items.AddRange(new object[] {
                "dbo.BBAMCPCTRANS_lot",
                "dbo.BBAMCPCTRANS_single",
                "dbo.BEDHONS_lot",
                "dbo.CSE_lot",
                "dbo.MED_lot"
            });
            cmbStoredProc.Text = "dbo.BBAMCPCTRANS_lot";
            addDbRow("Stored Procedure / Proc:", cmbStoredProc);

            btnTestConnection = new Button
            {
                Text = "⚡ Test SQL Connection",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Location = new Point(190, dbY),
                Size = new Size(200, 34)
            };
            VectorGraphics.ApplyModernButton(btnTestConnection, VectorGraphics.ColorBlueAccent, VectorGraphics.ColorBlueDark, Color.FromArgb(96, 165, 250), Color.White);
            btnTestConnection.Click += BtnTestConnection_Click;
            grpDb.Controls.Add(btnTestConnection);

            lblConnStatus = new Label
            {
                Text = "",
                Font = new Font("Segoe UI", 9f),
                Location = new Point(400, dbY + 7),
                AutoSize = true
            };
            grpDb.Controls.Add(lblConnStatus);

            grpDb.Height = dbY + 44;
            pnlDbContainer.Controls.Add(grpDb);

            // Preview Box
            GroupBox grpConnPreview = new GroupBox
            {
                Text = "Generated Connection String",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = Color.FromArgb(71, 85, 105),
                Location = new Point(16, grpDb.Bottom + 10),
                Size = new Size(620, 85)
            };

            txtConnPreview = new TextBox
            {
                Multiline = true,
                ReadOnly = true,
                Font = new Font("Consolas", 8.5f),
                ForeColor = Color.FromArgb(30, 41, 59),
                BackColor = Color.FromArgb(248, 250, 252),
                Location = new Point(14, 22),
                Size = new Size(592, 50),
                BorderStyle = BorderStyle.FixedSingle
            };
            grpConnPreview.Controls.Add(txtConnPreview);
            pnlDbContainer.Controls.Add(grpConnPreview);

            // Help info card
            Panel pnlDbInfo = new Panel
            {
                Location = new Point(16, grpConnPreview.Bottom + 10),
                Size = new Size(620, 80),
                BackColor = Color.FromArgb(239, 246, 255),
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(12)
            };
            Label lblDbInfoText = new Label
            {
                Text = "Note: Course Database Routing:\n" +
                       "Each course profile can connect to its designated SQL Server instance and database catalog (e.g. 'm_ed', 'bba_lot', 'nu_exam'). When you select a profile on the dashboard, its database credentials will be automatically applied.",
                Font = new Font("Segoe UI", 8.5f),
                ForeColor = Color.FromArgb(30, 58, 138),
                Dock = DockStyle.Fill
            };
            pnlDbInfo.Controls.Add(lblDbInfoText);
            pnlDbContainer.Controls.Add(pnlDbInfo);

            tabDatabase.Controls.Add(pnlDbContainer);

            // ------------------------------------------
            // TAB 3: TYPOGRAPHY & FONTS (Crystal Reports Style)
            // ------------------------------------------
            Panel pnlTypeContainer = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true
            };

            string[] cursiveFonts = new string[]
            {
                "Brush Script MT",
                "Lucida Handwriting",
                "Segoe Script",
                "Monotype Corsiva",
                "Edwardian Script ITC",
                "Freestyle Script",
                "Times New Roman",
                "Georgia",
                "Arial",
                "Garamond"
            };

            string[] gothicFonts = new string[]
            {
                "Old English Text MT",
                "Cloister Black",
                "Cinzel Decorative",
                "Engravers Old English BT",
                "Georgia",
                "Times New Roman",
                "Garamond"
            };

            string[] standardFonts = new string[]
            {
                "Times New Roman",
                "Georgia",
                "Garamond",
                "Cambria",
                "Calibri",
                "Segoe UI",
                "Arial"
            };

            GroupBox grpCertType = new GroupBox
            {
                Text = "Certificate Typography (Crystal Reports Customizer)",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = VectorGraphics.ColorMagentaDark,
                Location = new Point(16, 12),
                Size = new Size(620, 190)
            };

            // Student Name Font
            Label lblFontName = new Label { Text = "Student Name Font:", Font = UITheme.FontMain, Location = new Point(16, 26), AutoSize = true };
            cmbFontStudentName = new ComboBox { DropDownStyle = ComboBoxStyle.DropDown, Location = new Point(170, 24), Width = 190 };
            cmbFontStudentName.Items.AddRange(cursiveFonts);
            cmbFontStudentName.SelectedIndexChanged += (s, e) => UpdateLivePreview();

            Label lblFontNameSz = new Label { Text = "Size:", Font = UITheme.FontMain, Location = new Point(370, 26), AutoSize = true };
            numFontSizeStudentName = new NumericUpDown { Minimum = 14, Maximum = 48, Value = 24, Width = 60, Location = new Point(410, 24) };
            numFontSizeStudentName.ValueChanged += (s, e) => UpdateLivePreview();

            Label lblFontNameSt = new Label { Text = "Style:", Font = UITheme.FontMain, Location = new Point(480, 26), AutoSize = true };
            cmbFontStyleStudentName = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(524, 24), Width = 80 };
            cmbFontStyleStudentName.Items.AddRange(new object[] { "Italic", "Bold", "Regular", "Bold Italic" });
            cmbFontStyleStudentName.SelectedIndex = 0;
            cmbFontStyleStudentName.SelectedIndexChanged += (s, e) => UpdateLivePreview();

            grpCertType.Controls.Add(lblFontName);
            grpCertType.Controls.Add(cmbFontStudentName);
            grpCertType.Controls.Add(lblFontNameSz);
            grpCertType.Controls.Add(numFontSizeStudentName);
            grpCertType.Controls.Add(lblFontNameSt);
            grpCertType.Controls.Add(cmbFontStyleStudentName);

            // Degree Title Font
            Label lblFontDeg = new Label { Text = "Degree Heading Font:", Font = UITheme.FontMain, Location = new Point(16, 68), AutoSize = true };
            cmbFontDegreeTitle = new ComboBox { DropDownStyle = ComboBoxStyle.DropDown, Location = new Point(170, 66), Width = 190 };
            cmbFontDegreeTitle.Items.AddRange(gothicFonts);
            cmbFontDegreeTitle.SelectedIndexChanged += (s, e) => UpdateLivePreview();

            Label lblFontDegSz = new Label { Text = "Size:", Font = UITheme.FontMain, Location = new Point(370, 68), AutoSize = true };
            numFontSizeDegreeTitle = new NumericUpDown { Minimum = 18, Maximum = 60, Value = 32, Width = 60, Location = new Point(410, 66) };
            numFontSizeDegreeTitle.ValueChanged += (s, e) => UpdateLivePreview();

            grpCertType.Controls.Add(lblFontDeg);
            grpCertType.Controls.Add(cmbFontDegreeTitle);
            grpCertType.Controls.Add(lblFontDegSz);
            grpCertType.Controls.Add(numFontSizeDegreeTitle);

            // Certificate Body Font
            Label lblFontBody = new Label { Text = "Certificate Body Font:", Font = UITheme.FontMain, Location = new Point(16, 110), AutoSize = true };
            cmbFontCertBody = new ComboBox { DropDownStyle = ComboBoxStyle.DropDown, Location = new Point(170, 108), Width = 190 };
            cmbFontCertBody.Items.AddRange(standardFonts);
            cmbFontCertBody.SelectedIndexChanged += (s, e) => UpdateLivePreview();

            Label lblFontBodySz = new Label { Text = "Size:", Font = UITheme.FontMain, Location = new Point(370, 110), AutoSize = true };
            numFontSizeCertBody = new NumericUpDown { Minimum = 10, Maximum = 26, Value = 15, Width = 60, Location = new Point(410, 108) };
            numFontSizeCertBody.ValueChanged += (s, e) => UpdateLivePreview();

            grpCertType.Controls.Add(lblFontBody);
            grpCertType.Controls.Add(cmbFontCertBody);
            grpCertType.Controls.Add(lblFontBodySz);
            grpCertType.Controls.Add(numFontSizeCertBody);

            // Transcript Table Font
            Label lblFontTrans = new Label { Text = "Transcript Table Font:", Font = UITheme.FontMain, Location = new Point(16, 152), AutoSize = true };
            cmbFontTransTable = new ComboBox { DropDownStyle = ComboBoxStyle.DropDown, Location = new Point(170, 150), Width = 190 };
            cmbFontTransTable.Items.AddRange(standardFonts);

            Label lblFontTransSz = new Label { Text = "Size:", Font = UITheme.FontMain, Location = new Point(370, 152), AutoSize = true };
            numFontSizeTransTable = new NumericUpDown { Minimum = 8, Maximum = 16, Value = 10, Width = 60, Location = new Point(410, 150) };

            grpCertType.Controls.Add(lblFontTrans);
            grpCertType.Controls.Add(cmbFontTransTable);
            grpCertType.Controls.Add(lblFontTransSz);
            grpCertType.Controls.Add(numFontSizeTransTable);

            pnlTypeContainer.Controls.Add(grpCertType);

            // Live Typography Preview Panel
            GroupBox grpPreview = new GroupBox
            {
                Text = "👁️ Live Typography Preview (What It Looks Like On Print)",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = VectorGraphics.ColorGreenDark,
                Location = new Point(16, 212),
                Size = new Size(620, 230)
            };

            pnlLivePreview = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(254, 252, 232), // Warm certificate parchment tone
                Padding = new Padding(16)
            };

            lblPreviewDegreeTitle = new Label
            {
                Text = "Master of Education",
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 52,
                ForeColor = Color.Black
            };

            lblPreviewStudentName = new Label
            {
                Text = "Zakia Sultana Shamima",
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 48,
                ForeColor = Color.Black
            };

            lblPreviewCertBody = new Label
            {
                Text = "This is to certify that .....................................................................\nobtained the Degree under National University in the Examination of 2024.",
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                ForeColor = Color.FromArgb(30, 41, 59)
            };

            pnlLivePreview.Controls.Add(lblPreviewCertBody);
            pnlLivePreview.Controls.Add(lblPreviewStudentName);
            pnlLivePreview.Controls.Add(lblPreviewDegreeTitle);

            grpPreview.Controls.Add(pnlLivePreview);
            pnlTypeContainer.Controls.Add(grpPreview);

            tabTypography.Controls.Add(pnlTypeContainer);

            // ------------------------------------------
            // ------------------------------------------
            // TAB 4: REPORT TEMPLATES & CRYSTAL REPORTS (.RPT)
            // ------------------------------------------
            TabPage tabReports = new TabPage("Report Templates & .rpt");
            tabReports.BackColor = Color.White;
            tabReports.Padding = new Padding(16);

            Panel pnlReportContainer = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true
            };

            // Engine Selection Radios
            GroupBox grpEngineChoice = new GroupBox
            {
                Text = "Report Engine Mode",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = VectorGraphics.ColorGreenDark,
                Location = new Point(16, 10),
                Size = new Size(620, 115)
            };

            rbDefaultEngine = new RadioButton
            {
                Text = "Built-in High-Fidelity Engine (Single-Page Fit, QR Code, HTML/CSS Print)",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = VectorGraphics.ColorPrimary,
                Location = new Point(16, 24),
                Size = new Size(580, 24),
                Checked = true
            };

            rbCustomRpt = new RadioButton
            {
                Text = "SAP Crystal Reports (.rpt) Template Engine",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = VectorGraphics.ColorGreenDark,
                Location = new Point(16, 52),
                Size = new Size(580, 24)
            };

            rbCustomRdlc = new RadioButton
            {
                Text = "Microsoft Report Viewer (.rdlc) Template Engine",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = VectorGraphics.ColorSecondaryDark,
                Location = new Point(16, 80),
                Size = new Size(580, 24)
            };

            grpEngineChoice.Controls.Add(rbDefaultEngine);
            grpEngineChoice.Controls.Add(rbCustomRpt);
            grpEngineChoice.Controls.Add(rbCustomRdlc);
            pnlReportContainer.Controls.Add(grpEngineChoice);

            // GroupBox: SAP Crystal Reports (.rpt) Customizer
            GroupBox grpRpt = new GroupBox
            {
                Text = "Crystal Reports (.rpt) Template & Data Binding",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = VectorGraphics.ColorGreenDark,
                Location = new Point(16, 135),
                Size = new Size(620, 160)
            };

            Label lblRptPath = new Label
            {
                Text = "Crystal Report (.rpt) File Path:",
                Font = UITheme.FontMain,
                ForeColor = VectorGraphics.ColorTextPrimary,
                Location = new Point(16, 24),
                Width = 220
            };

            txtRptFilePath = new TextBox
            {
                Location = new Point(16, 46),
                Width = 460,
                ReadOnly = true,
                Font = new Font("Consolas", 8.5f),
                Text = "(No .rpt selected)"
            };

            btnBrowseRpt = new Button
            {
                Text = "Browse .rpt...",
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                Location = new Point(484, 44),
                Size = new Size(120, 28)
            };
            VectorGraphics.ApplyModernButton(btnBrowseRpt, VectorGraphics.ColorGreenMain, VectorGraphics.ColorGreenDark, VectorGraphics.ColorGreenHover, Color.White, 4);
            btnBrowseRpt.Click += (s, e) =>
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Title = "Select SAP Crystal Report (.rpt) Template";
                    ofd.Filter = "Crystal Reports (*.rpt)|*.rpt|All Files (*.*)|*.*";
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtRptFilePath.Text = ofd.FileName;
                        rbCustomRpt.Checked = true;
                    }
                }
            };

            btnOpenRptDesigner = new Button
            {
                Text = "Open in Designer",
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                Location = new Point(16, 84),
                Size = new Size(150, 32),
                Cursor = Cursors.Hand
            };
            VectorGraphics.ApplyModernButton(btnOpenRptDesigner, Color.FromArgb(79, 70, 229), Color.FromArgb(67, 56, 202), Color.FromArgb(99, 102, 241), Color.White, 4);
            btnOpenRptDesigner.Click += (s, e) =>
            {
                CrystalReportHelper.OpenInDesigner(txtRptFilePath.Text);
            };

            btnExportXsd = new Button
            {
                Text = "Export Schema (XSD)",
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                Location = new Point(174, 84),
                Size = new Size(175, 32),
                Cursor = Cursors.Hand
            };
            VectorGraphics.ApplyModernButton(btnExportXsd, Color.FromArgb(14, 116, 144), Color.FromArgb(15, 95, 118), Color.FromArgb(2, 132, 199), Color.White, 4);
            btnExportXsd.Click += (s, e) =>
            {
                try
                {
                    string courseKey = txtCourseKey.Text.Trim();
                    string targetFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CrystalReports");
                    DataTable sampleDt = CrystalReportHelper.CreateBaselineTranscriptTable();
                    string xsdPath = CrystalReportHelper.ExportSchemaForDesign(sampleDt, targetFolder, courseKey);
                    MessageBox.Show("Crystal Reports Schema (.xsd) and Sample XML exported successfully!\n\nSchema Path:\n" + xsdPath + "\n\nYou can use this in SAP Crystal Reports Designer (Database Expert -> ADO.NET XML) to design your report fields (including candidate info, grades, and noncredit_1..5).", "Schema Exported", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error exporting Crystal Reports schema:\n" + ex.Message, "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            btnTestLoadRpt = new Button
            {
                Text = "Test Load Data",
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                Location = new Point(357, 84),
                Size = new Size(140, 32),
                Cursor = Cursors.Hand
            };
            VectorGraphics.ApplyModernButton(btnTestLoadRpt, VectorGraphics.ColorSecondary, VectorGraphics.ColorSecondaryDark, VectorGraphics.ColorSecondaryHover, Color.White, 4);
            btnTestLoadRpt.Click += (s, e) =>
            {
                string msg;
                DataTable sampleDt = CrystalReportHelper.CreateBaselineTranscriptTable();
                bool ok = CrystalReportHelper.TestLoadRptData(txtRptFilePath.Text, sampleDt, out msg);
                MessageBox.Show(msg, ok ? "Crystal Reports Test: Verified" : "Crystal Reports Test: Notice", MessageBoxButtons.OK, ok ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
            };

            grpRpt.Controls.Add(lblRptPath);
            grpRpt.Controls.Add(txtRptFilePath);
            grpRpt.Controls.Add(btnBrowseRpt);
            grpRpt.Controls.Add(btnOpenRptDesigner);
            grpRpt.Controls.Add(btnExportXsd);
            grpRpt.Controls.Add(btnTestLoadRpt);
            pnlReportContainer.Controls.Add(grpRpt);

            // GroupBox: RDLC Report File Customization
            GroupBox grpRdlc = new GroupBox
            {
                Text = "Course Report Definition (.rdlc)",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = VectorGraphics.ColorBlueHeader,
                Location = new Point(16, 305),
                Size = new Size(620, 110)
            };

            Label lblRdlcPath = new Label
            {
                Text = "Active RDLC File Path:",
                Font = UITheme.FontMain,
                ForeColor = VectorGraphics.ColorTextPrimary,
                Location = new Point(16, 24),
                Width = 200
            };

            txtRdlcFilePath = new TextBox
            {
                Location = new Point(16, 46),
                Width = 460,
                ReadOnly = true,
                Font = new Font("Consolas", 8.5f),
                Text = "(No .rdlc selected)"
            };

            btnBrowseRdlc = new Button
            {
                Text = "📂 Browse .rdlc...",
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                Location = new Point(484, 44),
                Size = new Size(120, 28)
            };
            VectorGraphics.ApplyModernButton(btnBrowseRdlc, VectorGraphics.ColorPrimary, VectorGraphics.ColorPrimaryDark, VectorGraphics.ColorPrimaryHover, Color.White, 4);
            btnBrowseRdlc.Click += (s, e) =>
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Title = "Select Custom Course RDLC Report File";
                    ofd.Filter = "RDLC Report Files (*.rdlc)|*.rdlc|Report Definitions (*.rdl;*.rdlc)|*.rdl;*.rdlc|All Files (*.*)|*.*";
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtRdlcFilePath.Text = ofd.FileName;
                        rbCustomRdlc.Checked = true;
                    }
                }
            };

            btnClearReportEngine = new Button
            {
                Text = "↺ Reset to Built-in Engine",
                Font = new Font("Segoe UI", 8.5f),
                Location = new Point(16, 76),
                Size = new Size(180, 26)
            };
            VectorGraphics.ApplyOutlinedButton(btnClearReportEngine, VectorGraphics.ColorSecondary, VectorGraphics.ColorSecondary, 4);
            btnClearReportEngine.Click += (s, e) =>
            {
                rbDefaultEngine.Checked = true;
                txtRptFilePath.Text = "(No .rpt selected)";
                txtRdlcFilePath.Text = "(No .rdlc selected)";
            };

            grpRdlc.Controls.Add(lblRdlcPath);
            grpRdlc.Controls.Add(txtRdlcFilePath);
            grpRdlc.Controls.Add(btnBrowseRdlc);
            grpRdlc.Controls.Add(btnClearReportEngine);
            pnlReportContainer.Controls.Add(grpRdlc);

            // Information Box
            Panel pnlReportInfo = new Panel
            {
                Location = new Point(16, 425),
                Size = new Size(620, 110),
                BackColor = Color.FromArgb(239, 246, 255),
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(12)
            };
            Label lblReportInfo = new Label
            {
                Text = "Report Engine & Template Features:\n" +
                       "• Built-in Engine: Single-page official transcripts and certificates with live QR verification codes, automatic A4 / Legal fitting, and dedicated non-credit courses section.\n" +
                       "• Crystal Reports (.rpt): Load external .rpt templates designed in SAP Crystal Reports Designer. Use 'Export Schema (XSD)' to bind candidate fields, semester results, and noncredit_1..5.\n" +
                       "• RDLC: Compatible with Microsoft Report Viewer definitions.",
                Font = new Font("Segoe UI", 8.5f),
                ForeColor = Color.FromArgb(30, 58, 138),
                Dock = DockStyle.Fill
            };
            pnlReportInfo.Controls.Add(lblReportInfo);
            pnlReportContainer.Controls.Add(pnlReportInfo);

            tabReports.Controls.Add(pnlReportContainer);

            // Add tabs
            tabConfig.TabPages.Add(tabAcademic);
            tabConfig.TabPages.Add(tabDatabase);
            tabConfig.TabPages.Add(tabTypography);
            tabConfig.TabPages.Add(tabReports);

            // ==========================================
            // BOTTOM ACTION PANEL
            // ==========================================
            Panel pnlBottom = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 54,
                BackColor = Color.FromArgb(241, 245, 249),
                Padding = new Padding(16, 8, 16, 8)
            };

            FlowLayoutPanel pnlBottomBtns = new FlowLayoutPanel
            {
                Dock = DockStyle.Right,
                AutoSize = true,
                WrapContents = false,
                Padding = new Padding(0, 0, 0, 0)
            };

            btnSave = new Button
            {
                Text = "Save & Apply Profile",
                Size = new Size(200, 38),
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Margin = new Padding(0, 0, 10, 0),
                UseMnemonic = false
            };
            VectorGraphics.ApplyModernButton(btnSave, VectorGraphics.ColorGreenMain, VectorGraphics.ColorGreenDark, VectorGraphics.ColorGreenHover, Color.White);
            btnSave.Click += BtnSave_Click;

            btnCancel = new Button
            {
                Text = "Close",
                Size = new Size(110, 38),
                Font = new Font("Segoe UI", 9.5f),
                Margin = new Padding(0)
            };
            VectorGraphics.ApplyModernButton(btnCancel, Color.FromArgb(241, 245, 249), Color.FromArgb(226, 232, 240), Color.FromArgb(203, 213, 225), VectorGraphics.ColorBlueDark);
            btnCancel.Click += (s, e) => this.Close();

            pnlBottomBtns.Controls.Add(btnSave);
            pnlBottomBtns.Controls.Add(btnCancel);
            pnlBottom.Controls.Add(pnlBottomBtns);

            // Assembly
            this.Controls.Add(tabConfig);
            this.Controls.Add(pnlLeft);
            this.Controls.Add(pnlTopHeader);
            this.Controls.Add(pnlBottom);
        }

        private void PopulateList()
        {
            lstDesigns.Items.Clear();
            foreach (var d in designs)
            {
                lstDesigns.Items.Add(d);
            }
        }

        private void LstDesigns_SelectedIndexChanged(object sender, EventArgs e)
        {
            CourseDesignConfig d = lstDesigns.SelectedItem as CourseDesignConfig;
            if (d == null) return;

            // Tab 1
            txtCourseKey.Text = d.CourseKey ?? "";
            txtDisplayName.Text = d.DisplayName ?? "";
            txtDegreeConferred.Text = d.DegreeConferred ?? "";
            txtCertificateDegree.Text = d.CertificateDegreeTitle ?? "";
            txtDiscipline.Text = d.DisciplineSubject ?? "";
            txtEntranceQual.Text = d.EntranceQualification ?? "";
            txtDuration.Text = d.DurationOfCourse ?? "";
            numSemesters.Value = Math.Max(1, Math.Min(12, d.SemestersCount));
            numCredits.Value = Math.Max(1, Math.Min(300, d.TotalCredits));
            numPapers.Value = Math.Max(1, Math.Min(100, d.TotalPapers));
            cmbPageSize.SelectedItem = string.Equals(d.PageSize, "Legal", StringComparison.OrdinalIgnoreCase) ? "Legal" : "A4";
            txtRefPrefix.Text = d.RefPrefix ?? "";
            txtCertPrefix.Text = d.CertificateSerialPrefix ?? "NUPC 20- ";

            // Tab 2: Database
            txtDbServer.Text = d.DbServer ?? "192.168.0.230";
            txtDbName.Text = d.DbName ?? "m_ed";
            txtDbUser.Text = d.DbUser ?? "sa";
            txtDbPassword.Text = d.DbPassword ?? "1234";
            cmbStoredProc.Text = !string.IsNullOrEmpty(d.StoredProcedureName) ? d.StoredProcedureName : "dbo.BBAMCPCTRANS_lot";
            UpdateConnPreview(null, null);
            lblConnStatus.Text = "";

            // Tab 4: Report Templates (.rpt / .rdlc / default)
            txtRptFilePath.Text = !string.IsNullOrEmpty(d.RptFilePath) ? d.RptFilePath : "(No .rpt selected)";
            txtRdlcFilePath.Text = !string.IsNullOrEmpty(d.RdlcFilePath) ? d.RdlcFilePath : "(No .rdlc selected)";

            if (string.Equals(d.ReportEngineType, "CrystalReports", StringComparison.OrdinalIgnoreCase) || (!string.IsNullOrEmpty(d.RptFilePath) && !d.RptFilePath.StartsWith("(")))
            {
                rbCustomRpt.Checked = true;
            }
            else if (string.Equals(d.ReportEngineType, "RDLC", StringComparison.OrdinalIgnoreCase) || (!string.IsNullOrEmpty(d.RdlcFilePath) && !d.RdlcFilePath.StartsWith("(")))
            {
                rbCustomRdlc.Checked = true;
            }
            else
            {
                rbDefaultEngine.Checked = true;
            }

            // Tab 3: Typography
            SetComboValue(cmbFontStudentName, d.FontStudentName ?? "Brush Script MT");
            numFontSizeStudentName.Value = Math.Max(14, Math.Min(48, d.FontSizeStudentName > 0 ? d.FontSizeStudentName : 24));
            SetComboValue(cmbFontStyleStudentName, d.FontStyleStudentName ?? "Italic");

            SetComboValue(cmbFontDegreeTitle, d.FontDegreeTitle ?? "Old English Text MT");
            numFontSizeDegreeTitle.Value = Math.Max(18, Math.Min(60, d.FontSizeDegreeTitle > 0 ? d.FontSizeDegreeTitle : 32));

            SetComboValue(cmbFontCertBody, d.FontCertificateBody ?? "Times New Roman");
            numFontSizeCertBody.Value = Math.Max(10, Math.Min(26, d.FontSizeCertificateBody > 0 ? d.FontSizeCertificateBody : 15));

            SetComboValue(cmbFontTransTable, d.FontTranscriptTable ?? "Segoe UI");
            numFontSizeTransTable.Value = Math.Max(8, Math.Min(16, d.FontSizeTranscriptTable > 0 ? d.FontSizeTranscriptTable : 10));

            UpdateLivePreview();
        }

        private void SetComboValue(ComboBox cb, string val)
        {
            if (cb == null) return;
            for (int i = 0; i < cb.Items.Count; i++)
            {
                if (string.Equals(cb.Items[i].ToString(), val, StringComparison.OrdinalIgnoreCase))
                {
                    cb.SelectedIndex = i;
                    return;
                }
            }
            cb.Text = val;
        }

        private void UpdateConnPreview(object sender, EventArgs e)
        {
            string srv = txtDbServer.Text.Trim();
            string db = txtDbName.Text.Trim();
            string usr = txtDbUser.Text.Trim();
            string pwd = txtDbPassword.Text;

            string conn = string.IsNullOrEmpty(usr)
                ? string.Format("Data Source={0};Initial Catalog={1};Integrated Security=True;", srv, db)
                : string.Format("Data Source={0};Initial Catalog={1};User Id={2};Password={3};", srv, db, usr, pwd);

            txtConnPreview.Text = conn;
        }

        private void BtnTestConnection_Click(object sender, EventArgs e)
        {
            lblConnStatus.Text = "⏳ Testing...";
            lblConnStatus.ForeColor = VectorGraphics.ColorBlueAccent;
            Application.DoEvents();

            string conn = txtConnPreview.Text;
            string err;
            bool ok = Database.TestConnection(conn, out err);

            if (ok)
            {
                lblConnStatus.Text = "✔ Connection Successful!";
                lblConnStatus.ForeColor = VectorGraphics.ColorGreenDark;
                MessageBox.Show("Successfully connected to SQL database '" + txtDbName.Text + "' on server '" + txtDbServer.Text + "'.", "Connection Verified", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                lblConnStatus.Text = "✖ Connection Failed";
                lblConnStatus.ForeColor = Color.Crimson;
                MessageBox.Show("Database connection failed.\n\nTarget Server: " + txtDbServer.Text + "\nDatabase: " + txtDbName.Text + "\n\nError:\n" + err, "Connection Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateLivePreview()
        {
            try
            {
                // Update degree title preview font
                string degFont = cmbFontDegreeTitle.Text;
                float degSize = (float)numFontSizeDegreeTitle.Value;
                try
                {
                    lblPreviewDegreeTitle.Font = new Font(degFont, Math.Min(degSize, 26f), FontStyle.Regular);
                }
                catch
                {
                    lblPreviewDegreeTitle.Font = new Font("Georgia", 22f, FontStyle.Bold);
                }

                // Update student name preview font
                string stuFont = cmbFontStudentName.Text;
                float stuSize = (float)numFontSizeStudentName.Value;
                FontStyle st = FontStyle.Italic;
                string stStr = cmbFontStyleStudentName.SelectedItem != null ? cmbFontStyleStudentName.SelectedItem.ToString() : "Italic";
                if (stStr == "Bold") st = FontStyle.Bold;
                else if (stStr == "Bold Italic") st = FontStyle.Bold | FontStyle.Italic;
                else if (stStr == "Regular") st = FontStyle.Regular;

                try
                {
                    lblPreviewStudentName.Font = new Font(stuFont, Math.Min(stuSize, 24f), st);
                }
                catch
                {
                    lblPreviewStudentName.Font = new Font("Brush Script MT", 22f, FontStyle.Italic);
                }

                // Update body font
                string bodyFont = cmbFontCertBody.Text;
                float bodySize = (float)numFontSizeCertBody.Value;
                try
                {
                    lblPreviewCertBody.Font = new Font(bodyFont, Math.Min(bodySize, 12f), FontStyle.Regular);
                }
                catch
                {
                    lblPreviewCertBody.Font = new Font("Times New Roman", 11f, FontStyle.Regular);
                }
            }
            catch { }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            CourseDesignConfig newD = new CourseDesignConfig
            {
                CourseKey = "NEW_" + (designs.Count + 1),
                DisplayName = "New Course Profile (" + (designs.Count + 1) + ")",
                DegreeConferred = "BACHELOR OF ...",
                CertificateDegreeTitle = "Bachelor of ...",
                DisciplineSubject = "GENERAL",
                EntranceQualification = "HIGHER SECONDARY CERTIFICATE",
                DurationOfCourse = "04 (Four) Years",
                SemestersCount = 8,
                TotalCredits = 120,
                TotalPapers = 36,
                PageSize = "Legal",
                RefPrefix = "NU/Con/Certi/Trans/{EXM_CODE}/{EXM_YEAR}/",
                CertificateSerialPrefix = "NUPC 20- ",
                StoredProcedureName = "dbo.BBAMCPCTRANS_lot",
                RdlcFilePath = "",
                RptFilePath = "",
                ReportEngineType = "Default",
                DbServer = "192.168.0.230",
                DbName = "m_ed",
                DbUser = "sa",
                DbPassword = "1234",
                FontStudentName = "Brush Script MT",
                FontSizeStudentName = 24,
                FontStyleStudentName = "Italic",
                FontDegreeTitle = "Old English Text MT",
                FontSizeDegreeTitle = 32,
                FontCertificateBody = "Times New Roman",
                FontSizeCertificateBody = 15,
                FontTranscriptTable = "Segoe UI",
                FontSizeTranscriptTable = 10
            };
            designs.Add(newD);
            PopulateList();
            lstDesigns.SelectedItem = newD;
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            CourseDesignConfig d = lstDesigns.SelectedItem as CourseDesignConfig;
            if (d == null) return;

            if (designs.Count <= 1)
            {
                MessageBox.Show("Cannot delete the only remaining profile.", "Delete Protected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete profile '" + d.DisplayName + "'?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                designs.Remove(d);
                PopulateList();
                if (lstDesigns.Items.Count > 0) lstDesigns.SelectedIndex = 0;
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            CourseDesignConfig d = lstDesigns.SelectedItem as CourseDesignConfig;
            if (d != null)
            {
                // Tab 1
                d.CourseKey = txtCourseKey.Text.Trim();
                d.DisplayName = txtDisplayName.Text.Trim();
                d.DegreeConferred = txtDegreeConferred.Text.Trim();
                d.CertificateDegreeTitle = txtCertificateDegree.Text.Trim();
                d.DisciplineSubject = txtDiscipline.Text.Trim();
                d.EntranceQualification = txtEntranceQual.Text.Trim();
                d.DurationOfCourse = txtDuration.Text.Trim();
                d.SemestersCount = (int)numSemesters.Value;
                d.TotalCredits = (int)numCredits.Value;
                d.TotalPapers = (int)numPapers.Value;
                d.PageSize = cmbPageSize.SelectedItem != null ? cmbPageSize.SelectedItem.ToString() : "A4";
                d.RefPrefix = txtRefPrefix.Text.Trim();
                d.CertificateSerialPrefix = txtCertPrefix.Text.Trim();

                // Tab 2: Database & Stored Procedure
                d.DbServer = txtDbServer.Text.Trim();
                d.DbName = txtDbName.Text.Trim();
                d.DbUser = txtDbUser.Text.Trim();
                d.DbPassword = txtDbPassword.Text;
                d.StoredProcedureName = !string.IsNullOrEmpty(cmbStoredProc.Text) ? cmbStoredProc.Text.Trim() : "dbo.BBAMCPCTRANS_lot";

                // Tab 4: Report Template Engine
                d.RptFilePath = rbCustomRpt.Checked && !string.IsNullOrEmpty(txtRptFilePath.Text) && !txtRptFilePath.Text.StartsWith("(")
                    ? txtRptFilePath.Text.Trim()
                    : "";
                d.RdlcFilePath = rbCustomRdlc.Checked && !string.IsNullOrEmpty(txtRdlcFilePath.Text) && !txtRdlcFilePath.Text.StartsWith("(")
                    ? txtRdlcFilePath.Text.Trim()
                    : "";
                d.ReportEngineType = rbCustomRpt.Checked ? "CrystalReports" :
                                     rbCustomRdlc.Checked ? "RDLC" : "Default";

                // Tab 3: Typography
                d.FontStudentName = cmbFontStudentName.Text.Trim();
                d.FontSizeStudentName = (int)numFontSizeStudentName.Value;
                d.FontStyleStudentName = cmbFontStyleStudentName.SelectedItem != null ? cmbFontStyleStudentName.SelectedItem.ToString() : "Italic";
                d.FontDegreeTitle = cmbFontDegreeTitle.Text.Trim();
                d.FontSizeDegreeTitle = (int)numFontSizeDegreeTitle.Value;
                d.FontCertificateBody = cmbFontCertBody.Text.Trim();
                d.FontSizeCertificateBody = (int)numFontSizeCertBody.Value;
                d.FontTranscriptTable = cmbFontTransTable.Text.Trim();
                d.FontSizeTranscriptTable = (int)numFontSizeTransTable.Value;

                SelectedDesign = d;
            }

            CourseDesignRepository.SaveDesigns(designs);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
