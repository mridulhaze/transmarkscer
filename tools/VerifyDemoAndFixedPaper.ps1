[System.Reflection.Assembly]::LoadFrom((Resolve-Path "NU Solver\bin\Debug\NU H-Type Solver.exe")) | Out-Null
Add-Type -AssemblyName System.Windows.Forms
Add-Type -AssemblyName System.Drawing

$connStr = "Data Source=192.168.0.230;Initial Catalog=m_ed;Integrated Security=True;"
$dt = [NU_Solver.TranscriptGenerator]::FetchTranscriptData($connStr, "2022", "22603000005", $false)
$courses = [NU_Solver.TranscriptGenerator]::LoadCourses($connStr)

$artifactDir = "C:\Users\RAKIB\.gemini\antigravity\brain\cc16e424-484c-450c-b9bf-6d34fe8e1a56"

# Helper to render HTML in WebBrowser and save bitmap
function Save-PreviewImage($htmlPath, $outPng, $w, $h) {
    $wb = New-Object System.Windows.Forms.WebBrowser
    $wb.ScrollBarsEnabled = $false
    $wb.ScriptErrorsSuppressed = $true
    $wb.Width = $w
    $wb.Height = $h
    $wb.Navigate((Resolve-Path $htmlPath).Path)

    while ($wb.ReadyState -ne [System.Windows.Forms.WebBrowserReadyState]::Complete) {
        [System.Windows.Forms.Application]::DoEvents()
        Start-Sleep -Milliseconds 50
    }
    Start-Sleep -Milliseconds 500

    $bmp = New-Object System.Drawing.Bitmap($w, $h)
    $wb.DrawToBitmap($bmp, (New-Object System.Drawing.Rectangle(0, 0, $w, $h)))
    $bmp.Save($outPng, [System.Drawing.Imaging.ImageFormat]::Png)
    $bmp.Dispose()
    $wb.Dispose()
    Write-Host "Captured $outPng"
}

# 1. Transcript DEMO (with SAMPLE watermark)
$optDemo = New-Object NU_Solver.TranscriptPrintOptions
$optDemo.PageSize = "A4"
$optDemo.IsDemo = $true
$savedDemoPath = ""
$htmlDemo = [NU_Solver.TranscriptGenerator]::GenerateTranscriptHtml($dt, $courses, $optDemo, [ref]$savedDemoPath)
Save-PreviewImage $savedDemoPath (Join-Path $artifactDir "transcript_preview_fixed_demo_sample.png") 1200 1500

# 2. Transcript OFFICIAL (without watermark)
$optOfficial = New-Object NU_Solver.TranscriptPrintOptions
$optOfficial.PageSize = "A4"
$optOfficial.IsDemo = $false
$savedOffPath = ""
$htmlOff = [NU_Solver.TranscriptGenerator]::GenerateTranscriptHtml($dt, $courses, $optOfficial, [ref]$savedOffPath)
Save-PreviewImage $savedOffPath (Join-Path $artifactDir "transcript_preview_fixed_official_clean.png") 1200 1500

# 3. Certificate DEMO (with SAMPLE watermark)
$optCertDemo = New-Object NU_Solver.TranscriptPrintOptions
$optCertDemo.IsDemo = $true
$savedCertDemoPath = ""
$htmlCertDemo = [NU_Solver.TranscriptGenerator]::GenerateCertificateHtml($dt, $optCertDemo, [ref]$savedCertDemoPath)
Save-PreviewImage $savedCertDemoPath (Join-Path $artifactDir "certificate_preview_fixed_demo_sample.png") 1400 950

# 4. Certificate OFFICIAL (without watermark)
$optCertOff = New-Object NU_Solver.TranscriptPrintOptions
$optCertOff.IsDemo = $false
$savedCertOffPath = ""
$htmlCertOff = [NU_Solver.TranscriptGenerator]::GenerateCertificateHtml($dt, $optCertOff, [ref]$savedCertOffPath)
Save-PreviewImage $savedCertOffPath (Join-Path $artifactDir "certificate_preview_fixed_official_clean.png") 1400 950

Write-Host "All verification renders completed successfully."
