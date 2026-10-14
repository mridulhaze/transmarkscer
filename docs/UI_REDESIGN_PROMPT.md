ROLE AND GOAL
You are a senior WinForms UI engineer and Material Design specialist. Redesign the user interface of the National University Academic Transcript and Provisional Certificate system so that it looks modern, calm and professional, follows the Material 3 colour system, and is simpler to use. This is a UI-only task: the generated transcripts and certificates, the SQL, and the business rules must not change.

READ FIRST: project_updated.md (architecture, files, current UI notes), then NU Solver/UITheme.cs, NU Solver/VectorGraphics.cs, NU Solver/TranscriptForm.cs and NU Solver/TranscriptForm.Designer.cs if present.

CONTEXT
- C# WinForms, .NET Framework 4.0 Client Profile, built with MSBuild 4.0. Main screen: TranscriptForm (split view: left sidebar with cards, right preview that hosts the generated HTML in an embedded WebBrowser control).
- Other forms: Login_Form, AdminDashboardForm (3 tabs), CourseDesign, PrintAudit, AddUserForm, DbSettingsForm, UploaderDashboard and the OMR solver forms.
- Current palette (emerald, magenta, royal blue, navy, slate) is in UITheme.cs and VectorGraphics.cs.

HARD CONSTRAINTS (do not break these)
1. DOCUMENT OUTPUT IS FROZEN. Do not change TranscriptGenerator output, the HTML or CSS of the transcript or certificate, @media print rules, page-fit logic, QR codes, signatures or serial numbers. Prove it: before any change, generate sample documents from fake data and save the HTML; after your changes, the generated HTML must be byte-identical. Include the hash comparison in each phase report.
2. UI LAYER ONLY. Do not change SQL, stored procedures, database logic, RBAC rules or method signatures. Keep every existing event handler behaviour. If you must move code, keep it behind the same public members.
3. FRAMEWORK. Stay on .NET Framework 4.0 Client Profile and WinForms. Do not migrate to WPF or WinUI. Do not add NuGet UI packages unless you verify they target .NET 4.0 and it still builds with MSBuild 4.0. Prefer your own owner-drawn controls built on GDI+ (the project already has VectorGraphics.cs).
4. RBAC STAYS IDENTICAL. Controls that are hidden or disabled by CanView, CanPrint, CanEdit and CanAdmin must stay hidden or disabled under the same conditions. Add a test checklist that proves it for all four permission combinations.
5. PRIVACY. Never use real student data in screenshots, tests or commits. Use generated fake names and numbers. Never log or display passwords or connection strings.
6. OFFLINE. No network calls, no web fonts, no new runtime downloads.

DESIGN SYSTEM (Material 3, light theme first)
Create one single source of truth in UITheme.cs with named colour tokens, fonts, spacing and radii. No hard-coded colours or fonts anywhere else in the UI code. Colour tokens:
- primary #00694B, onPrimary #FFFFFF, primaryContainer #8DF7C9, onPrimaryContainer #002117
- secondary #4C6359, secondaryContainer #CEE9DB, onSecondaryContainer #092016
- tertiary #3D6373, tertiaryContainer #C1E8FB, onTertiaryContainer #001F29 (use for the certificate action instead of magenta)
- error #BA1A1A, errorContainer #FFDAD6, onErrorContainer #410002
- surface #F4FBF6, surfaceContainerLowest #FFFFFF, surfaceContainerLow #EEF5F0, surfaceContainer #E8F0EA, surfaceContainerHigh #E2EAE5
- onSurface #171D1A, onSurfaceVariant #3F4944, outline #6F7A74, outlineVariant #BEC9C2
- appBar (header) #003827 with white title text and #B8D4C6 subtitle text. Keep the NU crest on the left.
- preview canvas background: surfaceContainerHigh. The document paper stays white, with a soft shadow.
Check all text and icon contrast: at least 4.5 to 1 for text and 3 to 1 for icons and borders. If a token fails, adjust it slightly and record the change.

TYPOGRAPHY AND SPACING
- Font: Segoe UI (available on Windows 7 and later). Scale: title 20 semibold, section title 14 semibold, body 13, label 12 medium, helper text 12. Minimum text size is 12. Never below.
- Spacing grid: 4, 8, 12, 16, 24. Control height 40 for buttons and 48 for text fields. Card padding 16.
- Shapes: cards radius 12, fields radius 8, buttons fully rounded (pill).
- State layers: hover 8 percent, focus 12 percent, pressed 12 percent overlay. Disabled at 38 percent opacity. Transitions about 120 ms.

COMPONENTS TO BUILD (new files under NU Solver/Controls/, added to the .csproj)
- MaterialButton: filled, tonal, outlined and text variants; optional leading icon; hover, pressed, focus ring and disabled states; anti-aliased; works at 100, 125 and 150 percent DPI.
- MaterialTextField: outlined, floating label, helper text, error state, optional trailing icon. Wrap a real TextBox so input and tab order keep working. Supports a password mode with a show and hide toggle.
- MaterialDropdown (owner-drawn ComboBox, DropDownList style), MaterialSwitch and MaterialCheckBox, MaterialCard (surfaceContainerLow, 1 px outlineVariant, soft shadow), MaterialChip (status), MaterialProgressBar (linear, determinate and indeterminate), MaterialSnackbar or inline banner, and an expander for advanced options.
- IconPainter: vector icons drawn with GDI+ paths (search, print, open in browser, report, settings, history, shield or admin, check, error, chevron). Remove emoji from all captions.
- Rules: DoubleBuffered, ResizeRedraw, SuspendLayout around bulk changes, no flicker when resizing the splitter, keyboard and screen-reader friendly (AccessibleName and AccessibleRole, tooltips, access keys).

TRANSCRIPTFORM REDESIGN (the main screen)
Simplify with progressive disclosure. Keep every feature, but show less at once.
1. Header bar (56 px): crest, title National University, Bangladesh, subtitle Academic Transcript and Provisional Certificate Management System. On the right: signed-in user chip with role, and an Admin button (icon plus text, visible only when permitted).
2. Left sidebar (width about 380, collapsible with a menu button), three cards:
   a. Candidates: Exam year field and Registration No field with the helper text Leave blank to include the whole lot. Fast mode becomes a switch. Read the code to find out exactly what Fast mode does and write a one-line helper text from that. Do not guess.
   b. Course and layout: Course profile dropdown plus a settings icon button. A collapsed Advanced expander holds Paper size, Issue date, Reference prefix, Start Ref and Start Book Sl. The expander remembers its state.
   c. Generate: two large buttons. Generate Academic Transcript (filled primary) and Generate Provisional Certificate (tonal tertiary).
3. Secondary actions (Course Design, Audit Log, Admin Panel) move out of the big button stack into a compact row of outlined or text buttons with icons, or an overflow menu. Same permissions as today.
4. Database: remove the connection string box from the main screen. Show a small chip such as Database: m_ed, connected or not connected, plus a Test connection text button. Host, user and password are edited only in DbSettingsForm, with the password masked. Add a pre-flight guard: before generating, if the connected catalog does not match the database expected by the selected course profile, show a clear warning with an option to continue or cancel. Use only information the code already has.
5. Status and errors: replace the red Failed card with an inline banner or snackbar. Show a short human message plus a Details expander containing the raw error and a Copy button. Show an indeterminate progress bar under the header while work is running, and a determinate bar for batch jobs. Do NOT try to fix the SQL error here; log it as a separate defect (see below).
6. Right preview panel: toolbar with a status chip (document type, examinee count, lot name), a filled Print button, an outlined Open in browser button, and a text Reports button, all the same height. Add zoom out, zoom in and fit width if the WebBrowser control allows it safely. Add an empty state (icon plus the text Choose an exam year and generate to preview) when nothing is loaded. The preview canvas uses the grey surface token with the white page and a soft shadow.
7. Layout must work at 1366 by 768 and at 1920 by 1080, at 100, 125 and 150 percent DPI. Add an app.manifest with DPI awareness and use AutoScaleMode.Dpi. No clipped text or controls at any of these settings.

OTHER FORMS (do after TranscriptForm is approved)
Apply the same theme and components to Login_Form, AdminDashboardForm (Material tabs, tidy data grids with row height 40, zebra rows using surfaceContainerLow, header row in surfaceContainer, status pills, progress bars), CourseDesign, PrintAudit, AddUserForm, DbSettingsForm and the uploader and solver forms. Provide a Theme.Apply(Form) helper that restyles standard controls automatically so legacy forms improve without rewriting every designer file. Optional stretch: a dark theme toggle (the document paper stays white).

KNOWN SEPARATE DEFECT (log it, do not fix it in this task)
Generating shows the error: An insufficient number of arguments were supplied for the procedure or function dbo.GETCGPA. Record it in docs/DEFECTS.md with the exact message and the hypothesis that dbo.GETCGPA now expects an extra year argument while [dbo].[BBAMCPCTRANS_lot] calls it with only the registration number, and that the selected profile may point to a different database than the label shows. The UI must display this error gracefully.

WORK PLAN (stop and write a phase report after each phase; wait for my GO)
PHASE 0, baseline: build the solution with MSBuild 4.0 (Debug), record warnings. Create a fake-data mode or fixture so screens can run without real data. Save baseline screenshots (fake data only) at 100, 125 and 150 percent. Save baseline hashes of generated transcript and certificate HTML for the fake sample. No UI changes yet.
PHASE 1, theme foundation: UITheme tokens, fonts, spacing, IconPainter, and all Material controls, with a small demo form that shows every control and state. Build must pass.
PHASE 2, TranscriptForm redesign as specified. Build, run, screenshot, compare document hashes (must be identical), run the RBAC checklist for all four permission combinations.
PHASE 3, other forms and Theme.Apply, then the accessibility and keyboard pass.
PHASE 4, polish and verification: DPI matrix, contrast check, tab order, high-contrast mode sanity check, performance (open and resize without flicker), release build.

VERIFICATION (show real evidence, never claim without running)
- MSBuild Debug and Release builds succeed. Compare the warning count with the baseline.
- Document hash comparison: identical before and after.
- RBAC matrix: a table of View, Print, Edit and Admin cases versus visible and enabled controls.
- Screenshots before and after at three DPI settings and two resolutions (fake data only).
- Keyboard-only walkthrough: sign in, set year, generate, print, open admin.
- Colour contrast table for every text and background pair.

RULES
- Small, reversible commits with clear messages. Inspect the diff before each commit.
- Never weaken or delete existing behaviour to make the UI simpler. If something cannot be simplified safely, keep it and say why.
- Label results honestly: VERIFIED, NOT VERIFIED, BLOCKED or ASSUMED. Record assumptions in docs/UI_ASSUMPTIONS.md.
- After Phase 4, update project_updated.md (UI Design System section) to match the new palette, controls and layout.

DELIVERABLES
The themed application, the new Controls folder, UITheme documentation, before and after screenshots, hash comparison report, RBAC matrix, DEFECTS.md entry, updated project_updated.md, and a final summary of what changed and what was deliberately left alone.

START NOW with Phase 0 only. Do not change any UI code until the baseline is saved. Then write the Phase 0 report and wait for GO PHASE 1.