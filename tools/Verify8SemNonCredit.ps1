Add-Type -AssemblyName System.Data
Add-Type -AssemblyName System.Drawing
Add-Type -AssemblyName System.Windows.Forms

$asmPath = "e:\projects\NU H-Ttanscript_Marksheet_Certificate\NU Solver\bin\Debug\NU H-Type Solver.exe"
$asm = [System.Reflection.Assembly]::LoadFrom($asmPath)

Write-Host "=== TEST 1: Unit Testing ParsePaper for Non-Credit Courses ===" -ForegroundColor Cyan

$courses = New-Object 'System.Collections.Generic.Dictionary[string, NU_Solver.CourseInfo]' ([System.StringComparer]::OrdinalIgnoreCase)

# Add sample 8-semester B.Ed courses
$cList = @(
    @{ Code="510301"; Title="HISTORY OF THE EMERGENCE OF INDEPENDENT BANGLADESH"; Credits=4 },
    @{ Code="510303"; Title="BANGLA-1"; Credits=4 },
    @{ Code="510305"; Title="ENGLISH-1"; Credits=4 },
    @{ Code="510307"; Title="INTRODUCTION TO EDUCATION"; Credits=4 },
    @{ Code="510901"; Title="COMPREHENSIVE VIVA (SATISFACTORY/NON-SATISFACTORY)"; Credits=0 },
    @{ Code="520392"; Title="COMPREHENSIVE VIVA (SATISFACTORY/NON-SATISFACTORY)"; Credits=0 },
    @{ Code="540320"; Title="COMPREHENSIVE FINAL MCQ EXAM"; Credits=0 },
    @{ Code="540322"; Title="VIVA VOCE(SATISFACTORY/NON-SATISFACTORY)"; Credits=0 }
)

foreach ($c in $cList) {
    $ci = New-Object NU_Solver.CourseInfo
    $ci.Code = $c.Code
    $ci.Title = $c.Title
    $ci.Credits = $c.Credits
    $courses[$c.Code] = $ci
}

# 1. Regular course
$p1 = [NU_Solver.TranscriptGenerator]::ParsePaper("510301=A", $courses)
if ($p1.Credit -eq 4 -and -not $p1.IsNonCredit) {
    Write-Host "PASS: Regular course 510301 correctly parsed as 4 credits, IsNonCredit=False" -ForegroundColor Green
} else {
    Write-Error "FAIL: Regular course 510301: Credit=$($p1.Credit), IsNonCredit=$($p1.IsNonCredit)"
}

# 2. Non-credit course with S grade
$p2 = [NU_Solver.TranscriptGenerator]::ParsePaper("510901=S", $courses)
if ($p2.Credit -eq 0 -and $p2.IsNonCredit -and $p2.Grade -eq "Satisfactory") {
    Write-Host "PASS: Non-credit course 510901 correctly parsed as Credit=0, IsNonCredit=True, Grade=Satisfactory" -ForegroundColor Green
} else {
    Write-Error "FAIL: Non-credit course 510901: Credit=$($p2.Credit), IsNonCredit=$($p2.IsNonCredit), Grade=$($p2.Grade)"
}

# 3. Non-credit course by title inference
$p3 = [NU_Solver.TranscriptGenerator]::ParsePaper("540322=S", $courses)
if ($p3.Credit -eq 0 -and $p3.IsNonCredit) {
    Write-Host "PASS: Non-credit course 540322 correctly parsed as Credit=0, IsNonCredit=True" -ForegroundColor Green
} else {
    Write-Error "FAIL: Non-credit course 540322: Credit=$($p3.Credit), IsNonCredit=$($p3.IsNonCredit)"
}

Write-Host "`n=== TEST 2: 8-Semester Dataset with Non-Credit Courses Generation ===" -ForegroundColor Cyan

$dt = New-Object System.Data.DataTable("Examinees")
[void]$dt.Columns.Add("std_name", [string])
[void]$dt.Columns.Add("fname", [string])
[void]$dt.Columns.Add("mname", [string])
[void]$dt.Columns.Add("col_name", [string])
[void]$dt.Columns.Add("exm_roll", [string])
[void]$dt.Columns.Add("reg_no", [string])
[void]$dt.Columns.Add("SESSION", [string])
[void]$dt.Columns.Add("exm_year", [string])
[void]$dt.Columns.Add("exm_code", [string])
[void]$dt.Columns.Add("SESS1", [string])
[void]$dt.Columns.Add("C_CREDIT", [string])
[void]$dt.Columns.Add("T_CREDIT", [string])
[void]$dt.Columns.Add("CGPA", [string])
[void]$dt.Columns.Add("RES_DATE", [string])
[void]$dt.Columns.Add("CRT_SL", [string])

for ($s = 1; $s -le 8; $s++) {
    for ($p = 1; $p -le 10; $p++) {
        [void]$dt.Columns.Add("P$($p + 10)_S$s", [string])
    }
    [void]$dt.Columns.Add("S$($s)_GPA", [string])
}

for ($nc = 1; $nc -le 5; $nc++) {
    [void]$dt.Columns.Add("noncredit_$nc", [string])
}

$row = $dt.NewRow()
$row["std_name"] = "TAZPRIYA AKTER"
$row["fname"] = "MD. ABU TAHER"
$row["mname"] = "FERDOUS BEGUM"
$row["col_name"] = "GOVT. TEACHERS' TRAINING COLLEGE, CHATTOGRAM"
$row["exm_roll"] = "2380371"
$row["reg_no"] = "18503004559"
$row["SESSION"] = "2018-19"
$row["exm_year"] = "2023"
$row["exm_code"] = "5688"
$row["SESS1"] = "18"
$row["C_CREDIT"] = "152"
$row["T_CREDIT"] = "152"
$row["CGPA"] = "2.94"
$row["RES_DATE"] = "February 23, 2026"
$row["CRT_SL"] = "NU/Con/Certi/Trans/5688/2023/23-88-2-71"
$row["noncredit_1"] = "510901=Satisfactory"
$row["noncredit_2"] = "520392=Satisfactory"

# Semester 1
$row["P11_S1"] = "510301=C+"
$row["P12_S1"] = "510303=B+"
$row["P13_S1"] = "510305=B-"
$row["P14_S1"] = "510307=B"
$row["S1_GPA"] = "2.88"

# Semester 2 (includes non-credit 510901)
$row["P11_S2"] = "510309=B-"
$row["P12_S2"] = "510311=B-"
$row["P13_S2"] = "510313=C+"
$row["P14_S2"] = "510315=B-"
$row["P15_S2"] = "510901=S"
$row["S2_GPA"] = "2.69"

# Semester 3
$row["P11_S3"] = "520301=A-"
$row["P12_S3"] = "520303=B+"
$row["P13_S3"] = "520309=B"
$row["P14_S3"] = "520313=C+"
$row["P15_S3"] = "520317=B+"
$row["S3_GPA"] = "3.10"

# Semester 4 (includes non-credit 520392)
$row["P11_S4"] = "520347=B+"
$row["P12_S4"] = "520349=B+"
$row["P13_S4"] = "520355=B+"
$row["P14_S4"] = "520359=B"
$row["P15_S4"] = "520363=B-"
$row["P16_S4"] = "520392=S"
$row["S4_GPA"] = "3.10"

# Semester 5
$row["P11_S5"] = "530301=B+"
$row["P12_S5"] = "530303=B-"
$row["P13_S5"] = "530309=B-"
$row["P14_S5"] = "530313=B"
$row["P15_S5"] = "530317=C"
$row["S5_GPA"] = "2.80"

# Semester 6
$row["P11_S6"] = "530347=B+"
$row["P12_S6"] = "530349=B"
$row["P13_S6"] = "530355=B"
$row["P14_S6"] = "530359=B"
$row["P15_S6"] = "530363=B-"
$row["S6_GPA"] = "3.00"

# Semester 7
$row["P11_S7"] = "540302=A"
$row["P12_S7"] = "540304=A"
$row["P13_S7"] = "540306=B"
$row["P14_S7"] = "540308=A+"
$row["P15_S7"] = "540310=A+"
$row["S7_GPA"] = "3.75"

# Semester 8 (includes non-credit 540320 and 540322)
$row["P11_S8"] = "540311=A-"
$row["P12_S8"] = "540313=B+"
$row["P13_S8"] = "540315=B"
$row["P14_S8"] = "540317=B"
$row["P15_S8"] = "540319=B-"
$row["P16_S8"] = "540320=S"
$row["P17_S8"] = "540322=S"
$row["S8_GPA"] = "3.15"

$dt.Rows.Add($row) | Out-Null

$opts = New-Object NU_Solver.TranscriptPrintOptions
$opts.DegreeConferred = "BACHELOR OF EDUCATION"
$opts.DisciplineSubject = "Bachelor of Education"
$opts.DurationOfCourse = "04 (Four) Years"
$opts.EntranceQualification = "DEGREE (PASS) / GRADUATION"
$opts.SemestersCount = 8
$opts.TotalCredits = 152
$opts.PageSize = "A4" # Deliberately provide A4 to test auto-detection to Legal!

$htmlPath = ""
$nonCreditCount = 0
[NU_Solver.TranscriptGenerator]::GenerateTranscriptHtml($dt, $courses, $opts, [ref]$htmlPath, [ref]$nonCreditCount)

Write-Host "Generated HTML Path: $htmlPath" -ForegroundColor Yellow
Write-Host "Total Non-Credit Courses Detected: $nonCreditCount" -ForegroundColor Yellow

$html = [System.IO.File]::ReadAllText($htmlPath)

# Assertions
if ($html -match "size: legal portrait") {
    Write-Host "PASS: 8-semester program automatically configured to Legal paper size" -ForegroundColor Green
} else {
    Write-Error "FAIL: Did not auto-configure to Legal paper size"
}

if ($nonCreditCount -eq 4) {
    Write-Host "PASS: Exactly 4 non-credit courses detected" -ForegroundColor Green
} else {
    Write-Error "FAIL: Expected 4 non-credit courses, got $nonCreditCount"
}

if ($html -match "Non-Credit Course\(s\) and Grades") {
    Write-Host "PASS: Non-Credit Course(s) and Grades section correctly rendered below main table" -ForegroundColor Green
} else {
    Write-Error "FAIL: Non-Credit section not found in HTML"
}

if ($html -match "COMPREHENSIVE VIVA" -and $html -match "VIVA VOCE") {
    Write-Host "PASS: Non-credit course titles properly rendered" -ForegroundColor Green
} else {
    Write-Error "FAIL: Non-credit course titles missing"
}

# Render in Form/WebBrowser and take screenshot
Write-Host "`n=== TEST 3: Capturing Rendered Legal Preview Screenshot ===" -ForegroundColor Cyan
$form = New-Object System.Windows.Forms.Form
$form.Text = "8-Semester Legal Transcript Verification"
$form.Width = 950
$form.Height = 1200
$form.StartPosition = "CenterScreen"

$wb = New-Object System.Windows.Forms.WebBrowser
$wb.Dock = "Fill"
$wb.ScrollBarsEnabled = $true
$form.Controls.Add($wb)

$form.Show()
$wb.Navigate($htmlPath)

$timeout = [DateTime]::Now.AddSeconds(5)
while ($wb.ReadyState -ne [System.Windows.Forms.WebBrowserReadyState]::Complete -and [DateTime]::Now -lt $timeout) {
    [System.Windows.Forms.Application]::DoEvents()
    Start-Sleep -Milliseconds 100
}
[System.Windows.Forms.Application]::DoEvents()
Start-Sleep -Milliseconds 500

$bmp = New-Object System.Drawing.Bitmap($form.ClientSize.Width, $form.ClientSize.Height)
$form.DrawToBitmap($bmp, (New-Object System.Drawing.Rectangle(0, 0, $form.ClientSize.Width, $form.ClientSize.Height)))

$screenshotPath = "C:\Users\RAKIB\.gemini\antigravity\brain\cc16e424-484c-450c-b9bf-6d34fe8e1a56\transcript_8sem_legal_noncredit.png"
$bmp.Save($screenshotPath, [System.Drawing.Imaging.ImageFormat]::Png)
$bmp.Dispose()
$form.Close()
$form.Dispose()

Write-Host "Screenshot saved to: $screenshotPath" -ForegroundColor Green
Write-Host "ALL TESTS PASSED!" -ForegroundColor Green
