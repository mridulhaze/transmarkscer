Add-Type -AssemblyName System.Data
Add-Type -AssemblyName System.Drawing
Add-Type -AssemblyName System.Windows.Forms

$asm = [System.Reflection.Assembly]::LoadFrom("e:\projects\NU H-Ttanscript_Marksheet_Certificate\NU Solver\bin\Debug\NU H-Type Solver.exe")

function Get-FakeStudentTable {
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

    for ($sem = 1; $sem -le 8; $sem++) {
        for ($p = 1; $p -le 10; $p++) {
            [void]$dt.Columns.Add("P$($p + 10)_S$sem", [string])
        }
        [void]$dt.Columns.Add("S$($sem)_GPA", [string])
    }

    $row = $dt.NewRow()
    $row["std_name"] = "TASNEEM SULTANA"
    $row["fname"] = "RAFIQUL ISLAM"
    $row["mname"] = "SHAHANA BEGUM"
    $row["col_name"] = "GOVT. TEACHERS' TRAINING COLLEGE, DHAKA"
    $row["exm_roll"] = "2489001"
    $row["reg_no"] = "23603999001"
    $row["SESSION"] = "2023"
    $row["exm_year"] = "2024"
    $row["exm_code"] = "8773"
    $row["SESS1"] = "23"
    $row["C_CREDIT"] = "44"
    $row["T_CREDIT"] = "44"
    $row["CGPA"] = "3.78"
    $row["RES_DATE"] = "October 14, 2026"
    $row["CRT_SL"] = "00037420"

    $row["P11_S1"] = "810301=A"
    $row["P12_S1"] = "810303=A-"
    $row["P13_S1"] = "810305=A+"
    $row["P14_S1"] = "810319=A"
    $row["P15_S1"] = "810327=A-"
    $row["S1_GPA"] = "3.75"

    $row["P11_S2"] = "810307=A"
    $row["P12_S2"] = "810309=A"
    $row["P13_S2"] = "810311=A+"
    $row["P14_S2"] = "810313=A"
    $row["P15_S2"] = "810314=A+"
    $row["P16_S2"] = "810321=A-"
    $row["P17_S2"] = "810329=A"
    $row["S2_GPA"] = "3.80"

    $dt.Rows.Add($row) | Out-Null
    return ,$dt
}

function Get-FakeCourses {
    $courses = New-Object 'System.Collections.Generic.Dictionary[string, NU_Solver.CourseInfo]' ([System.StringComparer]::OrdinalIgnoreCase)
    $cList = @(
        @{ Code="810301"; Title="CURRICULUM STUDIES"; Credits=4 },
        @{ Code="810303"; Title="RESEARCH IN EDUCATION"; Credits=4 },
        @{ Code="810305"; Title="ICT AND EDUCATIONAL DEVELOPMENT"; Credits=4 },
        @{ Code="810319"; Title="SOCIAL SCIENCE CURRICULUM AND INSTRUCTION MATERIALS"; Credits=4 },
        @{ Code="810327"; Title="CHILD AND ADOLESCENCE PSYCHOLOGY"; Credits=4 },
        @{ Code="810307"; Title="POLICY, PLANNING AND LEADERSHIP IN EDUCATION"; Credits=4 },
        @{ Code="810309"; Title="ADMINISTRATION OF EDUCATION IN BANGLADESH"; Credits=4 },
        @{ Code="810311"; Title="EDUCATION IN GLOBAL CONTEXT"; Credits=4 },
        @{ Code="810313"; Title="COMPREHENSIVE EXAMINATION : WRITTEN"; Credits=3 },
        @{ Code="810314"; Title="COMPREHENSIVE EXAMINATION : ORAL"; Credits=1 },
        @{ Code="810321"; Title="SOCIAL SCIENCE EDUCATION AND HUMAN DEVELOPMENT"; Credits=4 },
        @{ Code="810329"; Title="EDUCATIONAL COUNSELING AND GUIDANCE"; Credits=4 }
    )
    foreach ($c in $cList) {
        $ci = New-Object NU_Solver.CourseInfo
        $ci.Code = $c.Code
        $ci.Title = $c.Title
        $ci.Credits = $c.Credits
        $courses[$c.Code] = $ci
    }
    return $courses
}

function Get-DefaultOptions {
    $opts = New-Object NU_Solver.TranscriptPrintOptions
    $opts.DegreeConferred = "MASTER OF EDUCATION"
    $opts.DisciplineSubject = "MASTER OF EDUCATION"
    $opts.DurationOfCourse = "01 (One) Year"
    $opts.EntranceQualification = "BACHELOR OF EDUCATION"
    $opts.RefPrefix = "NU/Con/Certi/Trans/{EXM_CODE}/{EXM_YEAR}/"
    $opts.StartRefNo = "00037420"
    $opts.StartBookSl = "0000167"
    $opts.PageSize = "A4"
    $opts.CertificateDegreeTitle = "Master of Education"
    $opts.IssueDate = "October 14, 2026"
    return $opts
}

# Generate fixtures
$dt = Get-FakeStudentTable
$courses = Get-FakeCourses
$opts = Get-DefaultOptions

$transPath = ""
$transRes = [NU_Solver.TranscriptGenerator]::GenerateTranscriptHtml($dt, $courses, $opts, [ref]$transPath)
$certPath = ""
$certRes = [NU_Solver.TranscriptGenerator]::GenerateCertificateHtml($dt, $opts, [ref]$certPath)

$fixedTrans = "e:\projects\NU H-Ttanscript_Marksheet_Certificate\docs\fixtures\baseline_transcript_sample.html"
$fixedCert = "e:\projects\NU H-Ttanscript_Marksheet_Certificate\docs\fixtures\baseline_certificate_sample.html"

[System.IO.File]::Copy($transPath, $fixedTrans, $true)
[System.IO.File]::Copy($certPath, $fixedCert, $true)

$hashTrans = (Get-FileHash -Path $fixedTrans -Algorithm SHA256).Hash
$hashCert = (Get-FileHash -Path $fixedCert -Algorithm SHA256).Hash

Write-Host "Re-generated Transcript Hash: $hashTrans"
Write-Host "Re-generated Certificate Hash: $hashCert"

$baselineObj = Get-Content -Path "e:\projects\NU H-Ttanscript_Marksheet_Certificate\docs\BASELINE_HASHES.json" | ConvertFrom-Json

if ($hashTrans -eq $baselineObj.TranscriptSHA256 -and $hashCert -eq $baselineObj.CertificateSHA256) {
    Write-Host "VERIFIED: Baseline hashes match byte-for-byte perfectly!" -ForegroundColor Green
} else {
    Write-Host "ERROR: Hashes differ!" -ForegroundColor Red
}
