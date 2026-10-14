Add-Type -AssemblyName System.Windows.Forms
Add-Type -AssemblyName System.Drawing

$projectRoot = "E:\projects\NU H-Ttanscript_Marksheet_Certificate"
$assemblyPath = Join-Path $projectRoot "NU Solver\bin\Debug\NU H-Type Solver.exe"

[System.Reflection.Assembly]::LoadFrom($assemblyPath) | Out-Null

$outDir = Join-Path $projectRoot "docs\screenshots\modern"
if (-not (Test-Path $outDir)) {
    New-Item -ItemType Directory -Path $outDir -Force | Out-Null
}

$artifactDir = "C:\Users\RAKIB\.gemini\antigravity\brain\cc16e424-484c-450c-b9bf-6d34fe8e1a56"

# 1. Capture TranscriptForm
Write-Host "Capturing TranscriptForm..."
$tForm = New-Object NU_Solver.TranscriptForm("2024", "")
$tForm.StartPosition = [System.Windows.Forms.FormStartPosition]::Manual
$tForm.Location = New-Object System.Drawing.Point(-32000, -32000)
$tForm.Show()

# Process events to allow layout and paint
[System.Windows.Forms.Application]::DoEvents()
Start-Sleep -Milliseconds 400
[System.Windows.Forms.Application]::DoEvents()

$bmpTranscript = New-Object System.Drawing.Bitmap($tForm.Width, $tForm.Height)
$tForm.DrawToBitmap($bmpTranscript, (New-Object System.Drawing.Rectangle(0, 0, $tForm.Width, $tForm.Height)))

$transcriptPath1 = Join-Path $outDir "transcript_form_modern.png"
$transcriptPath2 = Join-Path $artifactDir "transcript_form_modern.png"
$bmpTranscript.Save($transcriptPath1, [System.Drawing.Imaging.ImageFormat]::Png)
$bmpTranscript.Save($transcriptPath2, [System.Drawing.Imaging.ImageFormat]::Png)
$tForm.Close()
$tForm.Dispose()
$bmpTranscript.Dispose()
Write-Host "Saved TranscriptForm screenshot to $transcriptPath1"

# 2. Capture AdminDashboardForm (Workflow Tab)
Write-Host "Capturing AdminDashboardForm..."
$adminForm = New-Object NU_Solver.AdminDashboardForm
$adminForm.StartPosition = [System.Windows.Forms.FormStartPosition]::Manual
$adminForm.Location = New-Object System.Drawing.Point(-32000, -32000)
$adminForm.Show()

[System.Windows.Forms.Application]::DoEvents()
Start-Sleep -Milliseconds 400
[System.Windows.Forms.Application]::DoEvents()

$bmpAdmin = New-Object System.Drawing.Bitmap($adminForm.Width, $adminForm.Height)
$adminForm.DrawToBitmap($bmpAdmin, (New-Object System.Drawing.Rectangle(0, 0, $adminForm.Width, $adminForm.Height)))

$adminPath1 = Join-Path $outDir "admin_dashboard_modern.png"
$adminPath2 = Join-Path $artifactDir "admin_dashboard_modern.png"
$bmpAdmin.Save($adminPath1, [System.Drawing.Imaging.ImageFormat]::Png)
$bmpAdmin.Save($adminPath2, [System.Drawing.Imaging.ImageFormat]::Png)
$adminForm.Close()
$adminForm.Dispose()
$bmpAdmin.Dispose()
Write-Host "Saved AdminDashboardForm screenshot to $adminPath1"
