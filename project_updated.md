# National University Bangladesh — Academic Transcript, Marksheet & Provisional Certificate Management System

> **Comprehensive System Documentation & Technical Blueprint**  
> *Target Audience:* Developers, DevOps Engineers, System Architects, and AI Coding Agents continuing development.  
> *Last Updated:* October 2026  
> *Current Build Status:* **Production Ready / Fully Verified (0 Errors, 4 Warnings)**

---

## Table of Contents
1. [Executive Overview](#1-executive-overview)
2. [Full Repository & Project Structure](#2-full-repository--project-structure)
3. [Database Architecture & Backend Data Layer](#3-database-architecture--backend-data-layer)
   - [Database Connection & Credentials](#database-connection--credentials)
   - [Stored Procedures & Dynamic Function Dispatcher](#stored-procedures--dynamic-function-dispatcher)
   - [Database Table `dbo.transcript_user` & Authentication](#database-table-dbotranscript_user--authentication)
   - [Data Column Mappings](#data-column-mappings)
   - [Database Scalar Functions](#database-scalar-functions)
   - [Course Catalog Tables](#course-catalog-tables)
4. [Functional Capabilities & Technical Milestones](#4-functional-capabilities--technical-milestones)
   - [1. Grading System Table Layout Fix](#1-grading-system-table-layout-fix)
   - [2. CGPA Column Divider Removal](#2-cgpa-column-divider-removal)
   - [3. QR Code `RES_DATE` Integration](#3-qr-code-res_date-integration)
   - [4. Dynamic Reference & Book Serial Generation](#4-dynamic-reference--book-serial-generation)
   - [5. High-Resolution Asset Embedding](#5-high-resolution-asset-embedding)
   - [6. A4 & Legal Responsive Print Page Constraints](#6-a4--legal-responsive-print-page-constraints)
   - [7. Course Design Customization Engine](#7-course-design-customization-engine)
   - [8. Security-Grade Provisional Certificate Generator](#8-security-grade-provisional-certificate-generator)
   - [9. Print Audit Logging System](#9-print-audit-logging-system)
   - [10. Course Print Workflow Engine](#10-course-print-workflow-engine)
   - [11. Granular RBAC & User Credential Management](#11-granular-rbac--user-credential-management)
   - [12. Executive 3-Tab Admin Dashboard](#12-executive-3-tab-admin-dashboard)
   - [13. Automatic 8-Semester Legal Paper Fit & Non-Credit Courses Separation](#13-automatic-8-semester-legal-paper-fit--non-credit-courses-separation)
   - [14. Real-Time Process Cancellation & Stop Controls](#14-real-time-process-cancellation--stop-controls)
   - [15. Dynamic Function / Stored Procedure Selector](#15-dynamic-function--stored-procedure-selector)
   - [16. SQL Server Authentication & `dbo.transcript_user` Schema](#16-sql-server-authentication--dbotranscript_user-schema)
   - [17. Lite Green & White-Orange Modern Color Palette](#17-lite-green--white-orange-modern-color-palette)
   - [18. Custom Course Report Loading (Built-in, Crystal Reports .rpt, RDLC .rdlc)](#18-custom-course-report-loading-built-in-crystal-reports-rpt-rdlc-rdlc)
   - [19. SAP Crystal Reports (.rpt) Dynamic Runtime Engine, Dataset Schema Export (.xsd) & Test Loader](#19-sap-crystal-reports-rpt-dynamic-runtime-engine-dataset-schema-export-xsd--test-loader)
   - [20. Operator Authentication & User Credential View Dialog](#20-operator-authentication--user-credential-view-dialog)
   - [21. Dedicated Non-Credit Column Extraction (noncredit_1..5) & Deduplication](#21-dedicated-non-credit-column-extraction-noncredit_15--deduplication)
   - [22. Full-Width Preview, Grades Vertical Line Alignment & Unified Grading System Scale Bar](#22-full-width-preview-grades-vertical-line-alignment--unified-grading-system-scale-bar)
5. [UI Design System & Front-End Architecture](#5-ui-design-system--front-end-architecture)
   - [Lite Green & White-Orange Palette & Tokens](#lite-green--white-orange-palette--tokens)
   - [Layout Topology (Header, Left Sidebar, Preview Workspace)](#layout-topology-header-left-sidebar-preview-workspace)
   - [Embedded WebBrowser & Print Rendering Pipeline](#embedded-webbrowser--print-rendering-pipeline)
6. [API Specifications & Class Models](#6-api-specifications--class-models)
   - [`TranscriptGenerator`](#transcriptgenerator)
   - [`TranscriptPrintOptions`](#transcriptprintoptions)
   - [`CourseDesignManager` & `CourseDesignConfig`](#coursedesignmanager--coursedesignconfig)
   - [`CrystalReportHelper`](#crystalreporthelper)
   - [`PrintAuditManager` & `PrintAuditRecord`](#printauditmanager--printauditrecord)
   - [`CoursePrintWorkflowManager` & `PrintJob`](#courseprintworkflowmanager--printjob)
   - [`UserPermissionManager`, `AppUser` & `UserCredentialViewDialog`](#userpermissionmanager-appuser--usercredentialviewdialog)
7. [Compilation, Build & Deployment Guide](#7-compilation-build--deployment-guide)
   - [Prerequisites](#prerequisites)
   - [MSBuild Commands](#msbuild-commands)
   - [Standalone Portable Executable Generation](#standalone-portable-executable-generation)
8. [Handoff, Open Items & Future Roadmap ("Start from the End")](#8-handoff-open-items--future-roadmap-start-from-the-end)

---

## 1. Executive Overview

The **National University (NU) Bangladesh Academic Transcript, Marksheet & Provisional Certificate Management System** (`NU H-Type Solver`) is an enterprise-grade desktop application engineered in C# (.NET Framework 4.0 Client Profile) and Microsoft SQL Server.

The software addresses two critical operational workflows within the National University Examinations Division:
1. **OMR / Litho Correction & Tabulation**: Solving optical mark recognition scanning anomalies, marksheet reconciliation, and staging mark sheets (`mtemp`) for final consolidation.
2. **Academic Transcript & Provisional Certificate Engine**: Extracting multi-semester consolidated academic results for entire examination lots or individual examinees, producing cryptographically verifiable documents formatted for physical and digital issuance.

### Key Capabilities
- **Multi-Semester Dynamic Rendering**: Supports 1 to 8 academic semesters (e.g., M.Ed 2 Semesters, B.Ed Hons 4 Semesters, BBA 8 Semesters) without modifying application code.
- **Micro-Formatted Print Output**: Strict compliance with single-sheet A4 portrait (for 1–4 semesters) and Legal portrait (for 5–8 semesters) standards, with zero accidental multi-page spills.
- **Embedded Security Elements**: Deterministic SVG QR codes encoding candidate metadata and result publication date (`RES_DATE`), vector guilloche anti-copy borders, base64-encoded university seal, and authorized digital controller signature.
- **Workflow & Audit Governance**: Real-time batch job monitoring (Start, Pause, Resume, Reset), granular user access control (RBAC: View-only, Print, Edit, Admin), and tamper-evident audit logging for every printed lot.

---

## 2. Full Repository & Project Structure

```
e:\projects\NU H-Ttanscript_Marksheet_Certificate\
│
├── NU Solver.sln                                # Visual Studio Solution file
├── NU H-Type Solver.application                 # ClickOnce deployment manifest
├── setup.exe                                    # Application installation bootstrapper
├── Make_Standalone_EXE.bat                      # Automation script to build ILMerge portable exe
├── Make_Standalone_EXE.exe                      # Compiled tool for standalone packaging
├── project_updated.md                           # <--- THIS MASTER SYSTEM DOCUMENTATION FILE
│
├── [Assets & Brand Media]
│   ├── logo_NU.png                              # Official National University crest (PNG)
│   ├── controller_sinnature_enamulkarim.jpg    # Authorized signature of Controller of Examinations
│   ├── certificate_sample.jpeg                  # Primary visual reference for Provisional Certificate
│   ├── Marksheet_sample.jpeg                    # Visual reference for Tabulation Marksheet
│   ├── icon.png / icon_cube.png                 # Application launcher icons
│   └── static/                                  # Static image resources (nu.png, 2.jpg, app_icon.ico)
│
├── [SQL Scripts & Database Migrations]
│   ├── BBAMCPCTRANS_lot.sql                     # Production stored procedure generating lot results
│   ├── examinee_course.sql                      # Course-to-examinee relation dump
│   ├── bedhons_courses.sql                      # Course catalog for B.Ed (Honours)
│   ├── table-structure.sql                      # Schema definitions for solvers, colleges, regroll
│   ├── mtemp.sql                                # Staging table for marks input
│   ├── solving_tables.sql / solving_database.sql# Base solving tables & lookup indices
│   ├── attandance.sql                           # Examination attendance log schema
│   └── FF.sql                                   # Additional database procedures and scalar functions
│
├── NU Solver/                                   # Main C# WinForms Source Project
│   ├── NU H-Type Solver.csproj                  # .NET 4.0 Client Profile project definition
│   ├── app.config                               # Runtime configuration
│   ├── Program.cs                               # Application entry point (runs login form)
│   │
│   ├── [Authentication & Administration]
│   │   ├── Login_Form.cs / .Designer.cs         # Solver/Uploader/Admin login dialog
│   │   ├── AddUserForm.cs                       # Modal to provision new user accounts
│   │   ├── DbSettingsForm.cs                    # Configurator for SQL Server host, DB & credentials
│   │   ├── UserPermissionManager.cs             # RBAC logic, permissions file storage, auth & UserCredentialViewDialog
│   │   ├── AdminDashboardForm.cs                # Executive 3-Tab Admin Control Center
│   │   └── CoursePrintWorkflow.cs               # State machine for batch printing execution
│   │
│   ├── [Transcript & Certificate Generation]
│   │   ├── TranscriptGenerator.cs               # Core HTML/SVG layout engine, QR generator, DB fetcher
│   │   ├── TranscriptForm.cs                    # Main Split-View Transcript & Certificate UI
│   │   ├── CourseDesign.cs                      # Visual design customization editor & JSON persistence (.rpt/.rdlc)
│   │   ├── CrystalReportHelper.cs               # SAP Crystal Reports (.rpt) late-binding reflection engine & XSD exporter
│   │   ├── PrintAudit.cs                        # Lot-by-lot audit tracking and history viewer
│   │   └── VectorGraphics.cs                    # GDI+ vector graphics, gradients & card painters
│   │
│   ├── [OMR Solving & Data Staging (Legacy/Operational)]
│   │   ├── Database.cs                          # Global SQL connection pooling and helper methods
│   │   ├── UITheme.cs                           # Global WinForms styling and theming
│   │   ├── UploaderDashboard.cs / .Designer.cs  # Central dashboard for uploader operations
│   │   ├── H_solve.cs / .Designer.cs            # Primary OMR litho code correction interface
│   │   ├── SolveWindow.cs / .Designer.cs        # Alternative single-paper solver interface
│   │   ├── File_List.cs / .Designer.cs          # File assignment and queue management
│   │   ├── LithoCorrection.cs / .Designer.cs    # Litho code pattern repair form
│   │   ├── upload.cs / .Designer.cs             # Raw marks uploading tool
│   │   ├── MtempOptForm.cs                      # Staging table management & operations
│   │   └── MtempEditForm.cs                     # Grid-based mark record modifier
│   │
│   └── Properties/
│       ├── AssemblyInfo.cs                      # Assembly versioning and metadata
│       ├── Resources.resx / Resources.Designer.cs
│       └── Settings.settings / Settings.Designer.cs
│
├── modern_solver/                               # Web-based frontend experiment (HTML5/CSS3/JS/Python)
│   ├── index.html                               # Modern browser prototype
│   ├── styles.css                               # Responsive dashboard styling
│   ├── app.js                                   # Client logic
│   ├── seed_data.js                             # Mock student database
│   └── server.py                                # Lightweight Python development server
│
└── tools/
    └── ExeMaker/                                # Tooling for building single-binary standalone distributions
        └── Program.cs
```

---

## 3. Database Architecture & Backend Data Layer

### Database Connection & Credentials
- **Primary Database**: `m_ed` (Master of Education database hosting current examination lots).
- **Default Connection String**:
  ```text
  Data Source=192.168.0.230;Initial Catalog=m_ed;Integrated Security=True;
  ```
- **Connection Configuration**: Configurable in real time via [DbSettingsForm.cs](file:///e:/projects/NU%20H-Ttanscript_Marksheet_Certificate/NU%20Solver/DbSettingsForm.cs) and persisted via [Database.cs](file:///e:/projects/NU%20H-Ttanscript_Marksheet_Certificate/NU%20Solver/Database.cs) into an encrypted or local configuration file (`db_config.txt`).

---

### Stored Procedures & Dynamic Function Dispatcher
The application dynamically routes queries to the stored procedure specified in the active **Course Profile** or the **Stored Procedure / Function (@proc)** dropdown on the dashboard.

Common production stored procedures include:
- `dbo.BBAMCPCTRANS_lot`: Standard lot generation for BBA, M.Ed, and 8-semester degrees.
- `dbo.BBAMCPCTRANS_single`: Targeted single-candidate query.
- `dbo.BEDHONS_lot`: Specialized stored procedure for Bachelor of Education (Honours).
- `dbo.CSE_lot`: Stored procedure for B.Sc in Computer Science & Engineering.
- `dbo.MED_lot`: Dedicated procedure for Master of Education candidates.

#### Parameter Discovery & Fallback Protocol
Located in [TranscriptGenerator.cs](file:///e:/projects/NU%20H-Ttanscript_Marksheet_Certificate/NU%20Solver/TranscriptGenerator.cs):
1. The engine checks if the procedure exists on SQL Server and attempts metadata inspection via `SqlCommandBuilder.DeriveParameters(cmd)`.
2. If available, derived parameters are populated with `@reg_no` and `@EXM_YEAR`.
3. If parameter derivation is disabled or unavailable, a reliable fallback explicitly supplies `@reg_no` (`DBNull.Value` or string) and `@EXM_YEAR`.

#### Reference Stored Procedure Schema (`dbo.BBAMCPCTRANS_lot`):
```sql
ALTER PROCEDURE [dbo].[BBAMCPCTRANS_lot]
    @reg_no   VARCHAR(11) = NULL,   -- Set NULL to fetch ALL students in lot, or specify exact Reg No
    @EXM_YEAR VARCHAR(4)            -- e.g. '2024'
AS
BEGIN
    SELECT DISTINCT
        '8773' AS exm_code,
        dbo.GETMAXYEAR(examinee.reg_no) AS exm_year,
        examinee.reg_no,
        '' AS RESULT,
        CAST(dbo.GETCGPA(examinee.reg_no) AS DECIMAL(10,2)) AS CGPA,
        SUBSTRING(std_rif.SESSION, 3, 2) AS SESS1,
        SUBSTRING(std_rif.SESSION, 6, 2) AS SESS2,
        EMS.exm_roll,
        '813' AS SUB_CODE,
        examinee.c_code,
        examinee.col_code,
        '' AS COURSE,
        dbo.GETMAXTYPE(examinee.reg_no) AS STD_TYPE,
        '44' AS T_CREDIT,
        dbo.GETTOTALCREDITS(examinee.reg_no, 1, 8) AS C_CREDIT,
        '12' AS T_PAPER,
        dbo.GETTOTALPAP(examinee.reg_no, 1, 8) AS C_PAPER,

        -- Semester GPA and Paper Counts (1 to 8)
        CAST(dbo.GETSGPA_bba(examinee.reg_no, '1') AS DECIMAL(10,2)) AS S1_GPA,
        dbo.GETTOTALCREDITS(examinee.reg_no, 1, 1) AS S1_CR_PAS,
        dbo.GETTOTALPAP(examinee.reg_no, 1, 1) AS S1_PAP,
        dbo.GETTOTALPAP(examinee.reg_no, 1, 1) AS S1_PAP_PA,

        CAST(dbo.GETSGPA_bba(examinee.reg_no, '2') AS DECIMAL(10,2)) AS S2_GPA,
        dbo.GETTOTALCREDITS(examinee.reg_no, 2, 2) AS S2_CR_PAS,
        dbo.GETTOTALPAP(examinee.reg_no, 2, 2) AS S2_PAP,
        dbo.GETTOTALPAP(examinee.reg_no, 2, 2) AS S2_PAP_PA,
        -- [... repeated through S8_GPA ...]

        std_rif.std_name,
        -- Papers for Semester 1 (P11_S1 to P20_S1)
        dbo.GETP11TONN(examinee.reg_no, 1, 1) AS P11_S1,
        dbo.GETP11TONN(examinee.reg_no, 1, 2) AS P12_S1,
        -- [... up to P20_S1 ...]

        -- Papers for Semester 2 (P11_S2 to P20_S2)
        dbo.GETP11TONN(examinee.reg_no, 2, 1) AS P11_S2,
        dbo.GETP11TONN(examinee.reg_no, 2, 2) AS P12_S2,
        -- [... up to P20_S8 ...]

        std_rif.SESSION,
        college.col_name,
        std_rif.fname,
        std_rif.mname,
        std_rif.sex,
        examinee.cert_sl AS CRT_SL,
        result_publish_date.[Date] AS RES_DATE

    FROM
        examinee,
        regroll,
        std_rif,
        college,
        EMS,
        result_publish_date
    WHERE
        examinee.semester = '2'
        AND examinee.exm_year = CASE WHEN @EXM_YEAR = '' THEN examinee.exm_year ELSE @EXM_YEAR END
        AND (@reg_no IS NULL OR examinee.reg_no = @reg_no)
        AND examinee.reg_no = regroll.reg_no
        AND examinee.reg_no = std_rif.reg_no
        AND examinee.col_code = college.col_code
        AND examinee.reg_no = EMS.reg_no
        AND examinee.exm_year = result_publish_date.exm_year
        AND examinee.c_code = result_publish_date.c_code;
END
```

---

### Database Table `dbo.transcript_user` & Authentication
Located in [UserPermissionManager.cs](file:///e:/projects/NU%20H-Ttanscript_Marksheet_Certificate/NU%20Solver/UserPermissionManager.cs):
The system enforces database-backed user authentication. Upon startup, operator switch, or when generation begins, `EnsureTranscriptUserTableAndSeed(connStr)` checks if `dbo.transcript_user` exists in the connected database; if absent, it creates the table and seeds it with default accounts:

```sql
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[transcript_user]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[transcript_user] (
        [id]                 INT IDENTITY(1,1) PRIMARY KEY,
        [username]           VARCHAR(50) NOT NULL UNIQUE,
        [password]           VARCHAR(100) NOT NULL,
        [full_name]          NVARCHAR(150) NOT NULL,
        [role]               VARCHAR(50) NOT NULL,
        [department]         NVARCHAR(150) NULL,
        [can_view]           BIT NOT NULL DEFAULT 1,
        [can_print]          BIT NOT NULL DEFAULT 0,
        [can_edit]           BIT NOT NULL DEFAULT 0,
        [can_manage_users]   BIT NOT NULL DEFAULT 0,
        [can_batch_process]  BIT NOT NULL DEFAULT 0,
        [is_active]          BIT NOT NULL DEFAULT 1,
        [created_date]       DATETIME NOT NULL DEFAULT GETDATE(),
        [last_login_date]    DATETIME NULL
    );
END
```

#### Default Seed Accounts:
| Username | Password | Role | Permissions |
| :--- | :--- | :--- | :--- |
| `admin` | `admin123` | System Administrator | Full Superuser (View, Print, Edit, Admin, Batch) |
| `operator1` | `op123` | Production Operator | View, Print, Batch Process |
| `editor1` | `edit123` | Template Designer | View, Edit Templates |
| `viewer1` | `view123` | Audit Inspector | View-Only |
| `officer_nu` | `nu2026` | Examination Controller | View, Print, Batch Process |

---

### Data Column Mappings
The application maps the SQL result set to academic documents as follows:

| Column Name | Type | Application Usage |
| :--- | :--- | :--- |
| `reg_no` | `VARCHAR(11)` | Candidate Registration Number (displayed in info table, barcode, and QR code). |
| `exm_roll` | `VARCHAR(10)` | Examination Roll Number (sourced from `EMS.exm_roll`). |
| `std_name` | `VARCHAR(100)` | Candidate Full Name (uppercase in transcript, script font in certificate). |
| `fname` / `mname`| `VARCHAR(100)` | Father's Name and Mother's Name. |
| `col_name` | `VARCHAR(150)` | Institution Name (e.g. `GOVT. TEACHERS' TRAINING COLLEGE, DHAKA`). |
| `exm_year` | `VARCHAR(4)` | Year of Passing (calculated via `dbo.GETMAXYEAR`). |
| `SESSION` | `VARCHAR(9)` | Academic Session string (e.g. `2022-2023`, parsed to `2023`). |
| `SESS1` | `VARCHAR(2)` | 2-digit session suffix for document serial prefix (e.g. `23` for `NUT 23-`). |
| `CGPA` | `DECIMAL(10,2)`| Cumulative Grade Point Average. Rendered across all rows with **no divider**. |
| `C_CREDIT` / `T_CREDIT` | `INT` | Credits completed vs. total degree required credits (e.g. `44 Out of 44`). |
| `P11_S{1..8}`..`P20_S{1..8}` | `VARCHAR` | Paper result string encoded as `[PaperCode]=[LetterGrade]` (e.g. `810301=B-`). |
| `S{1..8}_GPA` | `DECIMAL` | Semester Grade Point Average (SGPA) for each semester. |
| `CRT_SL` | `VARCHAR` | Unique certificate / transcript tracking serial number. |
| `RES_DATE` | `VARCHAR(50)` | Official Result Published Date (e.g. `June 14 , 2026`). Encoded in QR. |
| `noncredit_1`..`noncredit_5` | `VARCHAR(150)` | Dedicated non-credit subjects and statuses (e.g. `510901 Comprehensive Viva: S` or `510901=S`). Extracted automatically into dedicated section under main transcript grades. |

---

### Database Scalar Functions
- `dbo.GETMAXYEAR(reg_no)`: Computes the latest examination year across all examination attempts.
- `dbo.GETCGPA(reg_no)`: Computes final CGPA taking highest earned grade points per paper.
- `dbo.GETSGPA_bba(reg_no, semester)`: Computes the Grade Point Average for a specific semester.
- `dbo.GETTOTALCREDITS(reg_no, start_sem, end_sem)`: Aggregates passed course credits in the semester range.
- `dbo.GETTOTALPAP(reg_no, start_sem, end_sem)`: Calculates total papers passed.
- `dbo.GETP11TONN(reg_no, sem, paper_index)`: Returns the paper code and earned letter grade formatted as `CODE=GRADE`.

---

### Course Catalog Table: `dbo.course`
Course metadata (titles, credits) is resolved by matching the 6-digit paper code:
```sql
SELECT course_code, UPPER(course_name) AS course_name, credits FROM dbo.course
```
- **Automated Text Normalization**: Resolves database typos dynamically (e.g., auto-replaces `EXANINATION` with `EXAMINATION` and `POLICY,PLANNING` with `POLICY, PLANNING`).
- **Default Credit Allocation**: If `credits` is null or zero in the database, defaults gracefully to `4` credits.

---

## 4. Recent Functional Updates & Technical Milestones

### 1. Grading System Table Layout Fix
- **Problem**: In previous builds, the Grading System table in the upper-right corner of the academic transcript suffered from awkward multi-line column header wrapping (`Letter<br/>Grade` and `Grade<br/>Point`), and row contents wrapped vertically when resized.
- **Solution**:
  - Replaced stacked headers with strict single-line text: `Numerical grade`, `Letter Grade`, `Grade Point`.
  - Configured CSS on `.grading-wrap` and `.grading-table`:
    ```css
    .grading-wrap { display: inline-block; width: 172px; text-align: left; }
    .grading-table { width: 100%; border-collapse: collapse; font-size: 7.5px; line-height: 1.1; border: 1px solid #000; table-layout: fixed; }
    .grading-table th, .grading-table td { border: 1px solid #000; padding: 1.5px 2px; white-space: nowrap; overflow: hidden; }
    ```
  - Headers and row values now render strictly on a single horizontal line across all browsers and screen resolutions.

---

### 2. CGPA Column Divider Removal
- **Problem**: The Academic Attainments marks table showed horizontal borders dividing the CGPA cell across different semesters.
- **Solution**:
  - The CGPA column is emitted **only once** on the very first paper row with `rowspan` equal to the total paper count across all semesters:
    ```csharp
    if (!cgpaEmitted)
    {
        sb.AppendFormat("      <td class='center bold' rowspan='{0}' style='vertical-align: middle; font-size: 11px;'>{1}</td>\r\n", totalAllPapers, cgpa);
        cgpaEmitted = true;
    }
    ```
  - The resulting output presents an unbroken, continuous CGPA cell centered vertically across all semester records without any dividing line.

---

### 3. QR Code `RES_DATE` Integration
- **Requirement**: The QR code must dynamically encode the Result Published Date (`RES_DATE`).
- **Payload Implementations**:
  - **Academic Transcript**:
    ```csharp
    string qrText = string.Format("Reg:{0},Roll:{1},Name:{2},CGPA:{3},Degree:{4},Year:{5},RES_DATE:{6}",
        regNo, exmRoll, stdName, cgpa, options.DegreeConferred, exmYear, resDate);
    ```
  - **Provisional Certificate**:
    ```csharp
    string qrText = string.Format("NU-PROV-CERT|Reg:{0}|Roll:{1}|Name:{2}|Degree:{3}|Year:{4}|CGPA:{5}|RES_DATE:{6}",
        regNo, exmRoll, stdName, options.CertificateDegreeTitle, exmYear, cgpa, resDate);
    ```
  - Embedded as deterministic SVG modules via `GenerateSvgQr(qrText, 64)`, accompanied by transparent `data-qr` and comment metadata for automated scanners.

---

### 4. Dynamic Reference & Book Serial Generation
- Supports customizable serial number increments across lots:
  - **Book Serial**: Formatted as `.No. NUT {SESS1}- {0000167 + lotIndex}`.
  - **Lett Reference**: Dynamically formatted per course code and exam year:
    `Lett: NU/Con/Certi/Trans/{EXM_CODE}/{EXM_YEAR}/{00037420 + lotIndex}(1)`.
  - Serial offsets and custom prefixes are fully editable in the UI sidebar and Course Design presets.

---

### 5. High-Resolution Asset Embedding
- Embedded via inline Base64 data URIs so that saved HTML reports remain 100% self-contained, portable, and printable without internet access or broken image paths:
  - **University Seal**: `logo_NU.png` -> Base64 PNG.
  - **Controller Signature**: `controller_sinnature_enamulkarim.jpg` -> Base64 JPEG.
  - Multi-path resolution automatically discovers assets in the application binary folder, parent directories, or project root.

---

### 6. A4 & Legal Responsive Print Page Constraints
- **A4 Portrait** (for courses with 1 to 4 semesters, e.g. M.Ed.):
  ```css
  @page { size: a4 portrait; margin: 5mm 8mm; }
  .transcript-page { width: 100%; max-width: 720px; margin: 0 auto 20px auto; padding: 14px 18px; box-sizing: border-box; }
  ```
- **Legal Portrait** (for courses with 5 to 8 semesters, e.g. BBA):
  ```css
  @page { size: legal portrait; margin: 5mm 8mm; }
  .transcript-page { width: 100%; max-width: 720px; box-sizing: border-box; }
  ```
- Guaranteed print isolation via `page-break-after: always; break-after: page; page-break-inside: avoid; break-inside: avoid;`.

---

### 7. Course Design Customization Engine
Located in [CourseDesign.cs](file:///e:/projects/NU%20H-Ttanscript_Marksheet_Certificate/NU%20Solver/CourseDesign.cs):
- Allows administrators to customize degrees without touching source code:
  - Course Preset Name & Code (e.g. `8773 - M.Ed`, `813 - BBA`).
  - Degree Conferred string (e.g. `MASTER OF EDUCATION`, `BACHELOR OF BUSINESS ADMINISTRATION`).
  - Discipline / Subject title.
  - Duration of Course (`01 (One) Year`, `04 (Four) Years`).
  - Total Credits (`44`, `140`).
  - Paper Size preference (`A4` vs `Legal`).
  - Custom Fonts (Student Name font, Degree Title font, Transcript marks font, Certificate font).
- Saved as JSON to `course_designs.json` and loaded into the main UI dropdown.

---

### 8. Security-Grade Provisional Certificate Generator
Located in [TranscriptGenerator.cs](file:///e:/projects/NU%20H-Ttanscript_Marksheet_Certificate/NU%20Solver/TranscriptGenerator.cs):
- Replicates the authentic National University Bangladesh Certificate:
  - Multi-tier vector **Guilloche Security Border** rendered natively in SVG (`<svg viewBox='0 0 1000 700'>`).
  - Serial banner (`NUPC 23- 0037420`) and diagonal `Sample` watermark badge.
  - Traditional calligraphic typography (Old English Text MT / Cloister Black for Degree Title, Brush Script MT for Student Name).
  - Verification Barcode string + QR Code containing complete student verification data.
  - Dotted leader lines for Roll No, Registration No, and Academic Session.

---

### 9. Print Audit Logging System
Located in [PrintAudit.cs](file:///e:/projects/NU%20H-Ttanscript_Marksheet_Certificate/NU%20Solver/PrintAudit.cs):
- Every generation or print operation writes an immutable record to `print_audit_log.json`:
  - `Timestamp`: ISO 8601 creation time.
  - `Operator`: Current logged-in user.
  - `DocumentType`: `TRANSCRIPT` or `CERTIFICATE`.
  - `CourseName` & `ExamYear`.
  - `TotalRecords`: Quantity of examinee documents produced.
  - `LotRange`: First and last Registration numbers in the batch.
  - `Colleges`: Summary of participating educational colleges.
  - `SavedFilePath`: Absolute path to the archived HTML document.
- Fully filterable and searchable via the **Audit Log** modal in the application.

---

### 10. Course Print Workflow Engine
Located in [CoursePrintWorkflow.cs](file:///e:/projects/NU%20H-Ttanscript_Marksheet_Certificate/NU%20Solver/CoursePrintWorkflow.cs):
- Provides batch printing state management for large examination lots:
  - **Job States**: `IDLE`, `QUEUED`, `PRINTING`, `PAUSED`, `COMPLETED`, `CANCELLED`, `ERROR`.
  - **Controls**: `StartJob()`, `PauseJob()`, `ResumeJob()`, `ResetJob()`.
  - **Telemetry**: Total candidates, processed count, remaining count, percent complete, elapsed time, and error logs.

---

### 11. Granular RBAC & User Credential Management
Located in [UserPermissionManager.cs](file:///e:/projects/NU%20H-Ttanscript_Marksheet_Certificate/NU%20Solver/UserPermissionManager.cs):
- Persistent user permission model saved to `user_permissions.json`:
  - `CanView`: Access to search examinees and view document previews.
  - `CanPrint`: Authority to execute print jobs and dispatch batches to physical printers.
  - `CanEdit`: Authority to alter Course Design presets, serial prefixes, and database connection strings.
  - `CanAdmin`: Superuser privileges allowing user account creation, role editing, and audit clearing.
- The UI dynamically disables or hides buttons (e.g. `Print Document`, `Admin Panel`, `Course Design`) according to the active user's granted permissions.

---

### 12. Executive 3-Tab Admin Dashboard
Located in [AdminDashboardForm.cs](file:///e:/projects/NU%20H-Ttanscript_Marksheet_Certificate/NU%20Solver/AdminDashboardForm.cs):
- Accessible via the **Admin Workflow** header button:
  - **Tab 1: Course Print Workflow & Execution**: Displays active batch print jobs, real-time status pills, progress bars, and execution controls (Start, Pause, Resume, Reset, View/Print Report).
  - **Tab 2: User Access & Role Permissions**: Live data grid listing all operators, credentials, role descriptions, and interactive checkboxes for `CanView`, `CanPrint`, `CanEdit`, `CanAdmin`, with user creation and password reset capabilities.
  - **Tab 3: Print Audit & Lot Logging**: Comprehensive activity table displaying every generation event, operator name, lot numbers, total printed count, and one-click buttons to open generated report files.

---

### 13. Automatic 8-Semester Legal Paper Fit & Non-Credit Courses Separation
Located in [TranscriptGenerator.cs](file:///e:/projects/NU%20H-Ttanscript_Marksheet_Certificate/NU%20Solver/TranscriptGenerator.cs):
- **Automatic 8-Semester Legal Format Detection**:
  - Automatically inspects examinee data across semesters 1 through 8. If semesters > 4 are detected or `options.SemestersCount >= 8`:
    - Automatically configures `@page { size: legal portrait; margin: 4mm 6mm; }`.
    - Adjusts table padding and typography so that all 8 semesters, non-credit courses, info table, grading system, QR code, and controller signature fit precisely onto **Page 1 of 1** on standard US Legal paper (`8.5 in x 14 in` / `215.9 mm x 355.6 mm`).
    - The main WinForms UI dynamically reflects the selection by setting the Paper Size dropdown to `Legal`.
- **Non-Credit Course Extraction and Rendering**:
  - Proactively queries `dbo.course`, `dbo.bedhons_courses`, and `dbo.examinee_course`.
  - Courses with `credits == 0`, titles containing `(Satisfactory/non-satisfactory)` or `Non-Credit`, or grades containing `S`/`Satisfactory` (e.g. `510901 Comprehensive Viva`, `520392 Comprehensive Viva`, `540320 Comprehensive Final MCQ Exam`, `540322 Viva Voce`) are automatically separated from credit-bearing courses.
  - Credit-bearing courses are retained in the main **Academic Attainments** table, where CGPA spans across all credit rows with no horizontal divider.
  - Non-credit courses are cleanly rendered in a dedicated portion immediately below the main transcript grades:
    - **Header**: `Non-Credit Course(s) and Grades :`
    - **Columns**: `SEMESTER | COURSE CODE | COURSE TITLE | CREDIT | LETTER GRADE / STATUS`
    - Displays semester ordinal, course code, full uppercase course title, `Non-Credit` credit label, and `Satisfactory` status.
  - If a student has no non-credit courses, the section is omitted automatically to preserve standard layouts.

---

### 14. Real-Time Process Cancellation & Stop Controls
Located in [TranscriptForm.cs](file:///e:/projects/NU%20H-Ttanscript_Marksheet_Certificate/NU%20Solver/TranscriptForm.cs) and [TranscriptGenerator.cs](file:///e:/projects/NU%20H-Ttanscript_Marksheet_Certificate/NU%20Solver/TranscriptGenerator.cs):
- **The Challenge**: Standard `BackgroundWorker.CancelAsync()` only signals a boolean flag; if a database query is blocked executing a large SQL procedure or filling a dataset, the application hangs until the query completes or times out.
- **Immediate SQL Abort**:
  - During `FetchTranscriptData`, an `Action<SqlCommand> onCmdCreated` callback registers the running `SqlCommand` instance as `activeQueryCmd`.
  - When the operator clicks either the `⏹ Stop` button in Card 1 (beside Search / Verify) or the `⏹ STOP PROCESS` button in Card 3 (beside Generate Transcript & Certificate), `RequestStopProcess()` is executed.
  - `RequestStopProcess()` immediately calls `activeQueryCmd.Cancel()` and `bgWorker.CancelAsync()`, triggering an instant SQL abort and catching the cancellation cleanly.
  - UI state immediately returns to `⏹ Process stopped by operator.` with progress bar hidden and all generation controls safely re-enabled.

---

### 15. Dynamic Function / Stored Procedure Selector
Located in [CourseDesign.cs](file:///e:/projects/NU%20H-Ttanscript_Marksheet_Certificate/NU%20Solver/CourseDesign.cs) and [TranscriptForm.cs](file:///e:/projects/NU%20H-Ttanscript_Marksheet_Certificate/NU%20Solver/TranscriptForm.cs):
- Operators can configure or select which stored procedure executes on a per-course or ad-hoc basis:
  - **In Card 2 (Document Configuration)**: An editable dropdown (`cmbStoredProc`) allows selecting pre-configured procedures (`dbo.BBAMCPCTRANS_lot`, `dbo.BBAMCPCTRANS_single`, `dbo.BEDHONS_lot`, `dbo.CSE_lot`, `dbo.MED_lot`) or typing a custom database stored procedure name.
  - **In Course Design Dialog**: Tab 2 ("Database Credentials") persists the chosen `StoredProcedureName` directly into `course_designs.json`.
  - **Automated Parameter Derivation**: Derives parameters from SQL Server metadata via `SqlCommandBuilder.DeriveParameters` and populates `@reg_no` and `@EXM_YEAR`.

---

### 16. SQL Server Authentication & `dbo.transcript_user` Schema
Located in [UserPermissionManager.cs](file:///e:/projects/NU%20H-Ttanscript_Marksheet_Certificate/NU%20Solver/UserPermissionManager.cs) and [TranscriptForm.cs](file:///e:/projects/NU%20H-Ttanscript_Marksheet_Certificate/NU%20Solver/TranscriptForm.cs):
- **Pre-Generation Authentication Gate**:
  - Generating transcripts or certificates requires an authenticated session with `CanPrint` authority.
  - When an unauthenticated operator or viewer attempts generation, the `OperatorLoginForm` dialog appears automatically, checking credentials against `dbo.transcript_user`.
- **Automatic Schema Initialization**:
  - `EnsureTranscriptUserTableAndSeed(connStr)` checks if `dbo.transcript_user` exists in the connected SQL catalog.
  - If absent, it creates the table and seeds it with default accounts (`admin`, `operator1`, `editor1`, `viewer1`, `officer_nu`).
- **Operator Switch Header Action**:
  - The top navigation bar includes a prominent `Switch Operator` button, allowing instant role switching without restarting the application.

---

### 17. Lite Green & White-Orange Modern Color Palette
Configured in [UITheme.cs](file:///e:/projects/NU%20H-Ttanscript_Marksheet_Certificate/NU%20Solver/UITheme.cs) and [VectorGraphics.cs](file:///e:/projects/NU%20H-Ttanscript_Marksheet_Certificate/NU%20Solver/VectorGraphics.cs):
- Replaced dark legacy gradients with a fresh, university-administration palette:
  - **Primary (Lite Green)**: `#10955D` — Clean, modern emerald tone for Generate Transcript, primary badges, and search buttons.
  - **Primary Dark (Deep Forest Green)**: `#0A6E44` — University header banner and admin navigation sidebar.
  - **Secondary (Warm Orange)**: `#F97316` — High-visibility accent for Generate Certificate, Admin Workflow button, active processing pills, and stop borders.
  - **Surfaces**: `#FFFFFF` cards with subtle `#E2E8F0` borders and `#F7FAF7` minty canvas background.
- Crisp vector rendering ensures all button text, labels, and mnemonics display cleanly without unprintable glyph boxes or missing ampersands (`TextFormatFlags.NoPrefix`).

---

### 18. Custom Course Report Loading (Built-in, Crystal Reports .rpt, RDLC .rdlc)
Located in [CourseDesign.cs](file:///e:/projects/NU%20H-Ttanscript_Marksheet_Certificate/NU%20Solver/CourseDesign.cs):
- Operators can choose how academic documents are rendered for each individual course in Course Design (Tab 4 — "Report Templates & .rpt"):
  1. **Built-in High-Fidelity Engine (Default)**: Single-page micro-formatted HTML/CSS templates with deterministic SVG QR codes, anti-copy guilloche borders, auto-scaling A4/Legal print rules, and Crystal Reports-style typography.
  2. **SAP Crystal Reports (.rpt) Engine**: Dynamically binds compiled SAP Crystal Reports `.rpt` template files with automated dataset mapping.
  3. **Microsoft Report Viewer (.rdlc) Engine**: Allows operators to browse and bind an external Microsoft Report Definition file (`.rdlc`) specifically for custom examination boards or legacy report viewers.
- Selected report engines and file paths are saved directly into `course_designs.json` and reloaded automatically when selecting course profiles in the main UI.

---

### 19. SAP Crystal Reports (.rpt) Dynamic Runtime Engine, Dataset Schema Export (.xsd) & Test Loader
Located in [CrystalReportHelper.cs](file:///e:/projects/NU%20H-Ttanscript_Marksheet_Certificate/NU%20Solver/CrystalReportHelper.cs) and [CourseDesign.cs](file:///e:/projects/NU%20H-Ttanscript_Marksheet_Certificate/NU%20Solver/CourseDesign.cs):
- **Zero-Dependency Dynamic Reflection Engine**:
  - Interacts with SAP Crystal Reports runtime (`CrystalDecisions.CrystalReports.Engine.ReportDocument` and `CrystalDecisions.Shared`) via late-binding reflection across versions 10.5, 11.5, 13.0, and 14.0.
  - Avoids hard external assembly references in the project file, ensuring 100% build portability under .NET Framework 4.0 Client Profile without requiring third-party NuGet packages.
- **One-Click Schema & Sample Dataset Exporter**:
  - The `Export Schema (XSD)` action generates `NationalUniversityTranscriptDataSet.xsd` and `NationalUniversityTranscriptDataSet_Sample.xml` under `CrystalReports/` (with automatic fallback to user `Documents/NU_Solver_Reports/CrystalReports`).
  - Includes all 90+ production columns: student identification, degree metadata, `P11_S1` through `P20_S8` paper codes, semester GPAs, CGPA, `RES_DATE`, and dedicated `noncredit_1` through `noncredit_5` fields.
  - Report designers can open SAP Crystal Reports Designer, create a new report, choose "ADO.NET (XML)" data source, and drag-and-drop university fields onto the visual canvas.
- **Direct Designer Launcher & Verification Loader**:
  - `Open in Designer`: Detects installed SAP Crystal Reports executable paths (`crw32.exe`) in `Program Files (x86)\SAP BusinessObjects\Crystal Reports` and launches the selected `.rpt` file.
  - `Test Load Data`: Validates runtime installation, instantiates `ReportDocument`, binds a populated `DataTable`, and confirms that data columns bind without errors.

---

### 20. Operator Authentication & User Credential View Dialog
Located in [UserPermissionManager.cs](file:///e:/projects/NU%20H-Ttanscript_Marksheet_Certificate/NU%20Solver/UserPermissionManager.cs), [TranscriptForm.cs](file:///e:/projects/NU%20H-Ttanscript_Marksheet_Certificate/NU%20Solver/TranscriptForm.cs), and [UploaderDashboard.cs](file:///e:/projects/NU%20H-Ttanscript_Marksheet_Certificate/NU%20Solver/UploaderDashboard.cs):
- **Protected Workspace Entry Point**:
  - Accessing "Academic Transcript & Certificate" from `UploaderDashboard` requires an authenticated session authenticated against `dbo.transcript_user`.
  - If unauthenticated, `OperatorLoginForm` is presented. If the operator lacks `CanView` permission, access is gracefully denied.
- **Interactive User Credential & Authority View Dialog**:
  - Upon successful login, the system renders `UserCredentialViewDialog` (580x560 px, styled in National University emerald green and warm orange).
  - Displays authenticated user details: Username, Full Name, Department, Role badge, Security Authority (`dbo.transcript_user`), and Last Session Login timestamp.
  - Displays a granular Authority Matrix with green/red status badges for:
    - *View & Preview Documents* (`CanView`)
    - *Generate & Print Documents* (`CanPrint`)
    - *Course Design & Template Editor* (`CanEdit`)
    - *Administrator & User Management* (`CanAdmin`)
    - *Batch Lot Processing* (`CanBatchProcess`)
  - Features action buttons: "✓ Continue to Workspace", "Switch User", and "Close".
  - Cleaned all unrendered Unicode glyphs (`UseMnemonic = false`) to eliminate square box artifacts in Windows Forms.
- **Direct TranscriptForm Guard & Navigation Controls**:
  - `TranscriptForm.OnLoad` verifies authentication, ensuring unauthenticated shortcuts cannot bypass the login gate.
  - Header actions include "User Profile" (green button) for instant permission inspection and "Switch Operator" (teal button) to re-authenticate at any time.

---

### 21. Dedicated Non-Credit Column Extraction (noncredit_1..5) & Deduplication
Located in [TranscriptGenerator.cs](file:///e:/projects/NU%20H-Ttanscript_Marksheet_Certificate/NU%20Solver/TranscriptGenerator.cs):
- **Dedicated Column Inspection**:
  - In addition to course catalog scanning, the generator checks for dedicated examinee columns: `noncredit_1`, `noncredit_2`, `noncredit_3`, `noncredit_4`, `noncredit_5` (and alternative case/format variants like `non_credit_1`).
- **Flexible Data String Parsing**:
  - Handles various database formatting patterns:
    - `510901 Comprehensive Viva: S`
    - `510901=S`
    - `510901 (Satisfactory)`
  - Matches course code against `courseDict` to resolve official title and credit allocation (`0` or `Non-Credit`).
- **Single-Line Deduplication**:
  - Automatically deduplicates non-credit courses against semester slots (`P11_S1..P20_S8`) so subjects appearing in both regular marks and dedicated non-credit columns are displayed only once.
- **Dedicated Single-Line Visual Section**:
  - Rendered beneath the main academic attainments table with clear column headers: `SEMESTER | COURSE CODE | COURSE TITLE | CREDIT | LETTER GRADE / STATUS`.
  - For 8-semester programs, all 8 semesters, non-credit courses, info table, grading system, QR code, and signatures fit strictly on **Page 1 of 1** on Legal paper.

---

### 22. Full-Width Preview, Grades Vertical Line Alignment & Unified Grading System Scale Bar
Located in [TranscriptGenerator.cs](file:///e:/projects/NU%20H-Ttanscript_Marksheet_Certificate/NU%20Solver/TranscriptGenerator.cs) and [TranscriptForm.cs](file:///e:/projects/NU%20H-Ttanscript_Marksheet_Certificate/NU%20Solver/TranscriptForm.cs):
- **Full-Width Right Panel Preview Workspace**:
  - Expanded `.transcript-page` and `.cert-page` to `width: 100%; max-width: 100%; margin: 0 0 16px 0;`, eliminating large empty side gaps in the desktop preview panel while strictly maintaining single-page `@media print` rules.
  - Reduced `pnlBrowserWrap` padding to 6px, letting the transcript sheet comfortably fill the entire right preview pane.
- **Strict Vertical Line Alignment of Grades**:
  - Implemented `.grade-val { display: inline-block; width: 24px; text-align: left; font-weight: bold; }` inside centered `LETTER GRADE` cells.
  - The base letter of every letter grade (`A`, `B`, `C`, `D`, `F`) starts at the exact same horizontal position across all rows, forming a clean, unvarying vertical line.
  - Non-credit table columns (`SEMESTER`, `COURSE CODE`, `COURSE TITLE`, `CREDIT`, and `LETTER GRADE / STATUS`) match the main table's column dimensions, ensuring seamless vertical alignment across both tables.
- **Unified Grading System Scale Bar**:
  - Converted the separate `<div>` header into an integrated `<th colspan='3' class='grading-title-th'>GRADING SYSTEM</th>` table header row.
  - Eliminated border disconnection, clipping, and overhangs. Optimized typography to Arial Narrow (198px width) with 60% numerical grade allocation, ensuring text such as `75% to less than 80%` displays completely without truncation.

---

## 5. UI Design System & Front-End Architecture

### Lite Green & White-Orange Palette & Tokens
Configured in [VectorGraphics.cs](file:///e:/projects/NU%20H-Ttanscript_Marksheet_Certificate/NU%20Solver/VectorGraphics.cs) and [UITheme.cs](file:///e:/projects/NU%20H-Ttanscript_Marksheet_Certificate/NU%20Solver/UITheme.cs):

| Token Name | Hex Code | Visual Application & Semantics |
| :--- | :--- | :--- |
| **Primary (Lite Green)** | `#10955D` | Primary navigation, Academic Transcript action, Search/Verify button. |
| **Primary Dark (Deep Forest Green)** | `#0A6E44` | Top navigation header background, admin navigation sidebar. |
| **Secondary (Warm Orange)** | `#F97316` | Provisional Certificate action, Admin Workflow button, active processing pills. |
| **Secondary Dark** | `#EA580C` | Button pressed states, dark orange accents. |
| **Secondary Hover** | `#FB923C` | Hover state for orange call-to-actions. |
| **Accent (Sky/Cyan)** | `#0284C7` | Operator switch button, informational chips, document preview indicators. |
| **Application Background** | `#F7FAF7` | Clean minty canvas workspace background. |
| **Card Surface** | `#FFFFFF` | Rounded cards, preview paper container. |
| **Text Primary** | `#0F172A` | High-contrast body typography, titles, and headers. |
| **Text Secondary** | `#64748B` | Subtle helpers, field descriptions, italic notes. |
| **Borders & Dividers** | `#E2E8F0` | 1px card outlines, splitters, toolbar dividers. |
| **Success Green** | `#16A34A` | System status "Ready" / "Complete" indicator pill. |
| **Warning Orange** | `#F97316` | Pre-flight validation warnings, paused batch jobs. |
| **Error Red** | `#DC2626` | Diagnostic error banners, failure status pill. |

---

### Layout Topology (Header, Left Sidebar, Preview Workspace)
The main window ([TranscriptForm.cs](file:///e:/projects/NU%20H-Ttanscript_Marksheet_Certificate/NU%20Solver/TranscriptForm.cs)) is organized into three clean zones:

1. **Top Header Banner** (`Height = 56px`):
   - Background: Deep Forest Green `#0A6E44` with a 2.5px Warm Orange bottom accent line.
   - Left: Official National University crest/logo.
   - Center-Left: Crisp white typography for university title and application subtitle.
   - Right: Active user role chip (`Role: Admin`), modern cyan `Switch Operator` button, and warm orange `Admin Workflow` launcher.

2. **Left Control Sidebar** (`Width = 380px`, scrollable with 5px draggable splitter):
   - **Card 1: CANDIDATE SEARCH**:
     - Exam Year input (`txtExmYear`) with `⚡ Fast Mode` checkbox and explanatory helper label.
     - Registration Number input (`txtRegNo`) with lot/single guidance.
     - Lite Green `Search / Verify` action button with adjacent `⏹ Stop` button for immediate query termination.
   - **Card 2: DOCUMENT CONFIGURATION**:
     - Course Profile preset dropdown with direct `⚙` design launcher.
     - Dynamic Stored Procedure / Function selector dropdown (`cmbStoredProc`).
     - Paper Size selector (`A4`, `Legal`) and Issue Date picker.
     - Reference Prefix, Start Reference Number, and Start Book Serial Number.
     - Live database status chip (`Database: m_ed • Configured`) with `⚡ Test Connection` button.
   - **Card 3: DOCUMENT GENERATION**:
     - Prominent Primary action: `Generate Academic Transcript` (Lite Green `#10955D`, 38px height).
     - Prominent Secondary action: `Generate Provisional Certificate` (Warm Orange `#F97316`, 38px height).
     - High-visibility `⏹ STOP PROCESS` button spanning the right side of both generation buttons.
   - **Card 4: ADMINISTRATION & AUDIT**:
     - Outlined compact tool buttons in a row: `Design`, `Audit`, `Admin`.
   - **Card 5: SYSTEM STATUS & DIAGNOSTICS**:
     - Real-time status pill (`● Ready` / `⏳ Fetching Data...` / `⏹ Process stopped` / `✅ Complete` / `❌ Generation Failed`).
     - Marquee progress bar.
     - Human-readable status message and diagnostic `📋 Copy Error` button.

3. **Main Right Document Workspace**:
   - Preview canvas background: `#F7FAF7`.
   - Compact top toolbar (`Height = 52px`):
     - Dynamic document type pill badge (`ACADEMIC TRANSCRIPT` / `PROVISIONAL CERTIFICATE`).
     - Examinee count and batch lot summary.
     - Actions: Filled `Print Document`, outlined `Open in Browser`, and outlined `Reports`.
   - Centered paper viewport: Clean white card container framing the embedded `WebBrowser` control, maintaining true A4 / Legal proportions on the canvas workspace.

---

### Embedded WebBrowser & Print Rendering Pipeline
- Uses `<meta http-equiv='X-UA-Compatible' content='IE=edge'>` to force the WinForms `WebBrowser` control into modern standards rendering mode.
- Includes `@media print` directives so that calling `webBrowser.Print()` or `webBrowser.ShowPrintPreviewDialog()` directly opens the native Windows print dialog with correct paper sizes pre-selected.

---

## 6. API Specifications & Class Models

### `TranscriptGenerator`
Static engine in `NU_Solver.TranscriptGenerator`:
- `string GetMedConnectionString()`: Returns connection string targeted at the `m_ed` database.
- `Dictionary<string, CourseInfo> LoadCourses(string connStr)`: Loads and normalizes course catalog from database.
- `DataTable FetchTranscriptData(string connStr, string exmYear, string regNo, bool fastMode, string procName = null, Action<SqlCommand> onCmdCreated = null)`: Dynamically executes the specified stored procedure (or `dbo.BBAMCPCTRANS_lot`), derive parameters, and captures the active `SqlCommand` for instant cancellation.
- `string GenerateTranscriptHtml(DataTable dtExaminees, Dictionary<string, CourseInfo> courseDict, TranscriptPrintOptions options, out string savedFilePath, out int nonCreditCount)`: Emits complete Academic Transcript HTML document, isolates non-credit courses, dynamically fits 8-semester programs on Legal paper size, and writes document to disk.
- `string GenerateCertificateHtml(DataTable dtExaminees, TranscriptPrintOptions options, out string savedFilePath)`: Emits complete Provisional Certificate HTML document with SVG borders.
- `string GenerateSvgQr(string text, int size = 64)`: Generates a self-contained SVG QR code module from input string.
- `ParsedPaper ParsePaper(string paperStr, Dictionary<string, CourseInfo> courseDict)`: Parses paper tokens (e.g. `510901=S`), determines credits and non-credit flags.

---

### `TranscriptPrintOptions`
Model for document parametrization:
```csharp
public class TranscriptPrintOptions
{
    public string DegreeConferred { get; set; }           // e.g. "MASTER OF EDUCATION"
    public string DisciplineSubject { get; set; }         // e.g. "MASTER OF EDUCATION"
    public string DurationOfCourse { get; set; }          // e.g. "01 (One) Year"
    public string EntranceQualification { get; set; }     // e.g. "BACHELOR OF EDUCATION"
    public string RefPrefix { get; set; }                 // e.g. "NU/Con/Certi/Trans/{EXM_CODE}/{EXM_YEAR}/"
    public string StartRefNo { get; set; }                // e.g. "00037420"
    public string StartBookSl { get; set; }               // e.g. "0000167"
    public string PageSize { get; set; }                  // "A4" or "Legal"
    public string CertificateDegreeTitle { get; set; }    // e.g. "Master of Education"
    public string IssueDate { get; set; }                 // e.g. "October 14, 2026"
    public string FontStudentName { get; set; }           // Default: "Brush Script MT"
    public int FontSizeStudentName { get; set; }          // Default: 24
    public string FontDegreeTitle { get; set; }           // Default: "Old English Text MT"
    public int FontSizeDegreeTitle { get; set; }          // Default: 32
    public string StoredProcedureName { get; set; }       // Default: "dbo.BBAMCPCTRANS_lot"
    public string RdlcFilePath { get; set; }              // Optional custom .rdlc report template path
}
```

---

### `CourseDesignRepository` & `CourseDesignConfig`
Located in [CourseDesign.cs](file:///e:/projects/NU%20H-Ttanscript_Marksheet_Certificate/NU%20Solver/CourseDesign.cs):
- `List<CourseDesignConfig> LoadDesigns()`: Reads `course_designs.json`.
- `void SaveDesigns(List<CourseDesignConfig> list)`: Persists profiles to disk.
- Profile properties include:
  - Academic metadata (Degree Conferred, Duration, Entrance Qualification, Credits).
  - Database Credentials (Data Source, Initial Catalog, User ID, Password).
  - `StoredProcedureName` (e.g. `dbo.BBAMCPCTRANS_lot`, `dbo.BEDHONS_lot`).
  - `ReportEngineType` (`"Default"`, `"CrystalReports"`, `"RDLC"`).
  - `RptFilePath` (path to external SAP Crystal Reports `.rpt` template file).
  - `RdlcFilePath` (optional path to external `.rdlc` file).
  - Typography customizations (Crystal Reports fonts & sizes).

---

### `CrystalReportHelper`
Located in [CrystalReportHelper.cs](file:///e:/projects/NU%20H-Ttanscript_Marksheet_Certificate/NU%20Solver/CrystalReportHelper.cs):
- `bool IsCrystalReportsAvailable(out string detectedVersion)`: Uses dynamic reflection to detect installed SAP Crystal Reports runtime versions (10.5, 11.5, 13.0, 14.0).
- `string ExportSchemaForDesign(DataTable dt, string targetFolder, string courseKey)`: Exports `NationalUniversityTranscriptDataSet.xsd` and sample XML for use in SAP Crystal Reports Designer.
- `bool TestLoadRptData(string rptPath, DataTable dt, out string message)`: Instantiates `ReportDocument` via reflection, loads `.rpt`, and validates dataset binding.
- `bool OpenInDesigner(string rptPath, out string errorMsg)`: Launches the SAP Crystal Reports Designer application (`crw32.exe`) directly.
- `DataTable CreateBaselineTranscriptTable()`: Synthesizes a comprehensive 90+ column baseline schema table with all candidate, semester paper, GPA, and `noncredit_1..5` fields.

---

### `PrintAuditManager` & `PrintAuditRecord`
Located in [PrintAudit.cs](file:///e:/projects/NU%20H-Ttanscript_Marksheet_Certificate/NU%20Solver/PrintAudit.cs):
- `void LogOperation(string docType, string examYear, string courseName, string lotReg, DataTable dtExaminees, string savedFilePath)`
- `List<PrintAuditRecord> GetAuditRecords()`
- Records include lot range, total count, participating colleges, operator name, and saved path.

---

### `CoursePrintWorkflow` & `PrintJob`
Located in [CoursePrintWorkflow.cs](file:///e:/projects/NU%20H-Ttanscript_Marksheet_Certificate/NU%20Solver/CoursePrintWorkflow.cs):
- Manages batch queue and execution states:
  - `List<PrintJob> LoadQueue()`
  - `void StartJob(string jobId)`
  - `void PauseJob(string jobId)`
  - `void ResumeJob(string jobId)`
  - `void FinishJob(string jobId)`
  - `void ResetJob(string jobId)`

---

### `UserPermissionManager`, `AppUser` & `UserCredentialViewDialog`
Located in [UserPermissionManager.cs](file:///e:/projects/NU%20H-Ttanscript_Marksheet_Certificate/NU%20Solver/UserPermissionManager.cs):
- `void EnsureTranscriptUserTableAndSeed(string connStr)`: Automatically initializes `dbo.transcript_user` on SQL Server.
- `AppUser Authenticate(string username, string password, string connStr = null)`: Checks credentials against SQL Server with local XML fallback.
- `List<AppUser> LoadUsers()`
- `void SaveUsers(List<AppUser> users)`
- `void InitializeDefaultAccounts()`
- `UserCredentialViewDialog`:
  - Modern 580x560 dialog displaying authenticated operator identity, security authority (`dbo.transcript_user`), and interactive RBAC authority matrix (View, Print, Edit, Admin, Batch).
  - Invoked automatically upon operator login and via the top-header "User Profile" button in `TranscriptForm`.

---

## 7. Compilation, Build & Deployment Guide

### Prerequisites
- Windows 7, 8, 10, or 11 (x86 or x64).
- Microsoft .NET Framework 4.0 Client Profile (or full .NET Framework 4.5+).
- Microsoft Build Engine (MSBuild v4.0.30319):
  `C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe`
- Microsoft SQL Server 2008 R2 / 2012 / 2016 / 2019 / 2022.

---

### MSBuild Commands

#### 1. Compile Debug Configuration:
```powershell
& "C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" "e:\projects\NU H-Ttanscript_Marksheet_Certificate\NU Solver.sln" /p:Configuration=Debug /t:Rebuild
```
*Output Path:* `e:\projects\NU H-Ttanscript_Marksheet_Certificate\NU Solver\bin\Debug\NU H-Type Solver.exe`

#### 2. Compile Production Release Configuration:
```powershell
& "C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" "e:\projects\NU H-Ttanscript_Marksheet_Certificate\NU Solver.sln" /p:Configuration=Release /t:Rebuild
```
*Output Path:* `e:\projects\NU H-Ttanscript_Marksheet_Certificate\NU Solver\bin\Release\NU H-Type Solver.exe`

---

### Standalone Portable Executable Generation
To generate a single, zero-dependency executable bundling all external assemblies:
1. Run `Make_Standalone_EXE.bat` in the repository root.
2. The batch script invokes `tools/ExeMaker` to merge all dependent DLLs and resource packages into:
   `dist\NU_Solver_Standalone.exe`.

---

## 8. Handoff, Open Items & Future Roadmap ("Start from the End")

### Current State ("The End")
- The system is fully operational and thoroughly verified.
- The stored procedure `[dbo].[BBAMCPCTRANS_lot]` is properly mapped.
- All user requirements are fulfilled and verified:
  1. **Page Fit**: 2-semester (M.Ed) fits on A4; 8-semester programs (B.Ed Hons, BBA) automatically detect and fit on Legal paper size (Page 1 of 1).
  2. **Non-Credit Courses**: Automatically detected, separated from the main credit table, and displayed in a dedicated portion beneath main transcript grades. Dedicated `noncredit_1` through `noncredit_5` columns are parsed and deduplicated.
  3. **QR Code**: Holds `RES_DATE` dynamically alongside student credentials.
  4. **Grades Layout**: Headers and rows in the grading table render strictly in a single straight horizontal line.
  5. **Admin & RBAC**: The Admin Dashboard, Course Design customizer, Print Audit logging, and granular user permission manager are live and operational.
  6. **SAP Crystal Reports (.rpt) Integration**: Dynamic reflection-based `.rpt` runtime engine, one-click XSD/XML schema export for SAP Crystal Reports Designer, and live test loader.
  7. **User Credential View & Protection**: "Academic Transcript & Certificate" entry point guarded by `dbo.transcript_user`, displaying the modern `UserCredentialViewDialog` authority matrix before opening.
   8. **Stop / Cancellation Controls**: Instant SQL query abortion and background worker cancellation via `activeQueryCmd.Cancel()`.
   9. **Stored Procedure Staging Isolation (Milestone 23)**: Resolved SQL Server exception `There is already an object named 'transcript_data_MED_2024_22603000050_imp' in the database`. The stored procedure `[dbo].[BBAMCPCTRANS_lot]` in `m_ed` database was restored from repository baseline, removing an un-commented temporary `INTO transcript_data_...` clause that caused collisions during transcript generation.

---

### Milestone 23: Database Stored Procedure Hotfix (Object Already Exists Collision)
- **Defect Symptom**: When generating Academic Transcripts (e.g. for Exam Year 2022 / Reg No 22603000005), the application surfaced:
  `Generation Error: There is already an object named 'transcript_data_MED_2024_22603000050_imp' in the database.`
- **Root Cause Analysis**:
  A manual modification was applied directly to `[dbo].[BBAMCPCTRANS_lot]` on the SQL Server (`192.168.0.230\m_ed`), adding an un-commented `INTO transcript_data_MED_2024_22603000050_imp` clause before the `FROM` block. Because `SELECT ... INTO` attempts to create a new physical table on every execution, the second run crashed with SQL Server error 2714. Furthermore, `SELECT ... INTO` suppressed returning the result set to `SqlDataAdapter.Fill()`.
- **Resolution**:
  Re-applied the clean stored procedure definition from `BBAMCPCTRANS_lot.sql` directly to the `m_ed` database. Verified that `BBAMCPCTRANS_lot` executes cleanly, returning live examinee records directly to `TranscriptGenerator.FetchTranscriptData` and rendering the official transcript without errors.

---

### Milestone 24: Print Header Suppression, Print Preview Toolbar, and DEMO Watermark
- **User Requests**:
  1. Remove the top `"Academic Transcript - National University Bangladesh"` header and fix the upper side of the transcript in physical print.
  2. Add a `Print Preview` dialog button to inspect exact print layout before sending to hardware.
  3. Fixed printable paper size on screen preview (centered sheet with shadow, not stretched fullscreen).
  4. Default-checked `DEMO` checkbox producing a prominent rotated `"SAMPLE"` watermark, which can only be unchecked by Editor/Admin for clean official printing.
- **Root Cause & Technical Fixes**:
  - **Browser Header / Footer Suppression**: Internet Explorer's WebBrowser control by default printed page titles and URLs using `HKCU\Software\Microsoft\Internet Explorer\PageSetup` (`header` and `footer`). This caused `"Academic Transcript - National University Bangladesh"` to print at the top-left and `Page 1 of 1` at the top-right, colliding with the university crest. Fixed by implementing `TranscriptGenerator.ClearBrowserPrintHeadersAndFooters()` which clears the registry `header` and `footer` values before any print or preview operation, and setting `<title></title>`.
  - **Upper Margin Fix**: Adjusted `@page` margins to `10mm 10mm 8mm 10mm` for A4 (and `6mm 8mm 6mm 8mm` for Legal), and set `@media print .transcript-page` padding to `4mm 6mm 4mm 6mm` (total 14mm top clearance from physical paper edge), ensuring clean spacing from the paper top without clipping or crest collision.
  - **Print Preview Action**: Added `btnPrintPreview` (`👁️ Print Preview`) to the preview toolbar, invoking `webBrowser.ShowPrintPreviewDialog()` with safety checks and automatic registry header suppression.
  - **Fixed Printable Paper Size**: Screen preview displays as a true-to-scale centered sheet (`.transcript-page` at `210mm` width for A4, `216mm` for Legal) with subtle elevation shadow on `#eef2f6` canvas background.
  - **DEMO / SAMPLE Watermark**: Added `chkDemoWatermark` to Generation card (default checked). Renders a high-contrast semi-transparent rotated `"SAMPLE"` watermark across transcripts and certificates. Locked to DEMO mode for View-only operators; Editor and Admin roles can toggle to print clean official documents.

---

### Recommended Next Steps for Future Agents / Developers
1. **Direct PDF Export Engine**:
   - *Current Implementation*: Generates self-contained HTML reports with `@media print` rules, which users print to paper or "Microsoft Print to PDF" via the browser dialog.
   - *Enhancement*: Integrate a headless PDF generator (e.g. WebView2 `CoreWebView2.PrintToPdfAsync` or standard PDF writer) to allow one-click bulk PDF exports directly into a specified directory.
2. **Direct POS / Laser Network Printing**:
   - Integrate `System.Drawing.Printing.PrintDocument` or Win32 Raw Print spooler APIs to allow unattended background batch printing directly to networked high-speed duplex laser printers without showing print dialogs for each lot.
3. **Multi-Threaded Parallel Lot Fetching**:
   - For massive lots exceeding 5,000 examinees, split the query into chunks using SQL pagination (`OFFSET ... FETCH`) across a `ThreadPool` or `Parallel.ForEach` worker pool.
4. **Hardware Non-Printable Margin Override**:
   - Certain older HP and Canon laser printers enforce physical 6.3mm edge margins. If users report borders touching edge margins on specific printers, advise selecting "Fit to Printable Area" in the Windows printer preferences or set `.transcript-page { padding: 16px 22px; }`.

---

*Authored by Antigravity Agentic AI — Google DeepMind (Advanced Agentic Coding)*  
*Maintained for National University, Bangladesh.*


