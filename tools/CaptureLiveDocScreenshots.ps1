Add-Type -AssemblyName System.Data
Add-Type -AssemblyName System.Drawing
Add-Type -AssemblyName System.Windows.Forms

$projectRoot = "E:\projects\NU H-Ttanscript_Marksheet_Certificate"
$assemblyPath = Join-Path $projectRoot "NU Solver\bin\Debug\NU H-Type Solver.exe"
[System.Reflection.Assembly]::LoadFrom($assemblyPath) | Out-Null

. (Join-Path $projectRoot "tools\GenerateBaselineFixture.ps1")

$outDir = Join-Path $projectRoot "docs\screenshots\modern"
$artifactDir = "C:\Users\RAKIB\.gemini\antigravity\brain\cc16e424-484c-450c-b9bf-6d34fe8e1a56"

# Generate fake documents
$dt = Get-FakeStudentTable
$courses = Get-FakeCourses
$opts = Get-DefaultOptions

$tHtmlPath = ""
[NU_Solver.TranscriptGenerator]::GenerateTranscriptHtml($dt, $courses, $opts, [ref]$tHtmlPath) | Out-Null

$cHtmlPath = ""
[NU_Solver.TranscriptGenerator]::GenerateCertificateHtml($dt, $opts, [ref]$cHtmlPath) | Out-Null

# 1. Transcript Live Preview Screenshot
Write-Host "Capturing Live Transcript Preview..."
$tForm = New-Object NU_Solver.TranscriptForm("2024", "")
$tForm.Size = New-Object System.Drawing.Size(1360, 880)
$tForm.StartPosition = [System.Windows.Forms.FormStartPosition]::Manual
$tForm.Location = New-Object System.Drawing.Point(50, 50)
$tForm.Show()

# Find webBrowser control via reflection
$wbField = [NU_Solver.TranscriptForm].GetField("webBrowser", [System.Reflection.BindingFlags]::NonPublic -bor [System.Reflection.BindingFlags]::Instance)
$wb = $wbField.GetValue($tForm)

$badgeField = [NU_Solver.TranscriptForm].GetField("lblDocBadge", [System.Reflection.BindingFlags]::NonPublic -bor [System.Reflection.BindingFlags]::Instance)
$badge = $badgeField.GetValue($tForm)
$badge.Text = "ACADEMIC TRANSCRIPT"
$badge.BackColor = [System.Drawing.Color]::FromArgb(224, 242, 254)
$badge.ForeColor = [System.Drawing.Color]::FromArgb(3, 105, 161)

$summaryField = [NU_Solver.TranscriptForm].GetField("lblPreviewSummary", [System.Reflection.BindingFlags]::NonPublic -bor [System.Reflection.BindingFlags]::Instance)
$summary = $summaryField.GetValue($tForm)
$summary.Text = "1 examinee | M.Ed (2024)"

$btnPrintField = [NU_Solver.TranscriptForm].GetField("btnPrint", [System.Reflection.BindingFlags]::NonPublic -bor [System.Reflection.BindingFlags]::Instance)
$btnPrint = $btnPrintField.GetValue($tForm)
$btnPrint.Enabled = $true

$btnBrowserField = [NU_Solver.TranscriptForm].GetField("btnOpenBrowser", [System.Reflection.BindingFlags]::NonPublic -bor [System.Reflection.BindingFlags]::Instance)
$btnBrowser = $btnBrowserField.GetValue($tForm)
$btnBrowser.Enabled = $true

$wb.Navigate($tHtmlPath)

for ($i = 0; $i -lt 25; $i++) {
    [System.Windows.Forms.Application]::DoEvents()
    Start-Sleep -Milliseconds 100
}

$bmpTranscriptLive = New-Object System.Drawing.Bitmap($tForm.Width, $tForm.Height)
$tForm.DrawToBitmap($bmpTranscriptLive, (New-Object System.Drawing.Rectangle(0, 0, $tForm.Width, $tForm.Height)))

$livePath1 = Join-Path $outDir "transcript_form_modern_live.png"
$livePath2 = Join-Path $artifactDir "transcript_form_modern_live.png"
$bmpTranscriptLive.Save($livePath1, [System.Drawing.Imaging.ImageFormat]::Png)
$bmpTranscriptLive.Save($livePath2, [System.Drawing.Imaging.ImageFormat]::Png)
$bmpTranscriptLive.Dispose()
Write-Host "Saved Live Transcript Preview screenshot to $livePath1"

# 2. Certificate Live Preview Screenshot
Write-Host "Capturing Live Certificate Preview..."
$badge.Text = "PROVISIONAL CERTIFICATE"
$badge.BackColor = [System.Drawing.Color]::FromArgb(243, 232, 255)
$badge.ForeColor = [System.Drawing.Color]::FromArgb(126, 34, 206)
$summary.Text = "1 examinee | Provisional Certificate (2024)"

$wb.Navigate($cHtmlPath)

for ($i = 0; $i -lt 25; $i++) {
    [System.Windows.Forms.Application]::DoEvents()
    Start-Sleep -Milliseconds 100
}

$bmpCertLive = New-Object System.Drawing.Bitmap($tForm.Width, $tForm.Height)
$tForm.DrawToBitmap($bmpCertLive, (New-Object System.Drawing.Rectangle(0, 0, $tForm.Width, $tForm.Height)))

$certLivePath1 = Join-Path $outDir "transcript_form_modern_cert_live.png"
$certLivePath2 = Join-Path $artifactDir "transcript_form_modern_cert_live.png"
$bmpCertLive.Save($certLivePath1, [System.Drawing.Imaging.ImageFormat]::Png)
$bmpCertLive.Save($certLivePath2, [System.Drawing.Imaging.ImageFormat]::Png)
$bmpCertLive.Dispose()
Write-Host "Saved Live Certificate Preview screenshot to $certLivePath1"

$tForm.Close()
$tForm.Dispose()
