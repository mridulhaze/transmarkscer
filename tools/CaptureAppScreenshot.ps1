[System.Reflection.Assembly]::LoadFrom((Resolve-Path "NU Solver\bin\Debug\NU H-Type Solver.exe")) | Out-Null
Add-Type -AssemblyName System.Windows.Forms
Add-Type -AssemblyName System.Drawing

[System.Windows.Forms.Application]::EnableVisualStyles()

# Ensure authenticated session as Admin
[NU_Solver.UserSession]::EnsureDefaultAdminIfUnset()

$form = New-Object NU_Solver.TranscriptForm
$form.StartPosition = [System.Windows.Forms.FormStartPosition]::Manual
$form.Location = New-Object System.Drawing.Point(-32000, -32000)
$form.Show()

# Load the demo transcript into the form's webBrowser
$demoHtml = "C:\Users\RAKIB\Documents\NU_Solver_Reports\Academic_Transcript_22603000005_20261017_152155.html"
$wbField = $form.GetType().GetField("webBrowser", [System.Reflection.BindingFlags]::NonPublic -bor [System.Reflection.BindingFlags]::Instance)
if ($wbField) {
    $wb = $wbField.GetValue($form)
    $wb.Navigate((Resolve-Path $demoHtml).Path)
}

# Update toolbar buttons
$curField = $form.GetType().GetField("currentGeneratedHtmlPath", [System.Reflection.BindingFlags]::NonPublic -bor [System.Reflection.BindingFlags]::Instance)
if ($curField) {
    $curField.SetValue($form, $demoHtml)
}
$btnPrintField = $form.GetType().GetField("btnPrint", [System.Reflection.BindingFlags]::NonPublic -bor [System.Reflection.BindingFlags]::Instance)
if ($btnPrintField) {
    $btn = $btnPrintField.GetValue($form)
    $btn.Enabled = $true
}
$btnPrevField = $form.GetType().GetField("btnPrintPreview", [System.Reflection.BindingFlags]::NonPublic -bor [System.Reflection.BindingFlags]::Instance)
if ($btnPrevField) {
    $btnPrev = $btnPrevField.GetValue($form)
    $btnPrev.Enabled = $true
}

$start = [DateTime]::Now
while (([DateTime]::Now - $start).TotalSeconds -lt 2.5) {
    [System.Windows.Forms.Application]::DoEvents()
    Start-Sleep -Milliseconds 50
}

$bmp = New-Object System.Drawing.Bitmap($form.Width, $form.Height)
$form.DrawToBitmap($bmp, (New-Object System.Drawing.Rectangle(0, 0, $form.Width, $form.Height)))

$artifactDir = "C:\Users\RAKIB\.gemini\antigravity\brain\cc16e424-484c-450c-b9bf-6d34fe8e1a56"
$outPng = Join-Path $artifactDir "transcript_form_live_print_preview_demo.png"
$bmp.Save($outPng, [System.Drawing.Imaging.ImageFormat]::Png)
$bmp.Dispose()
$form.Close()
$form.Dispose()

Write-Host "Captured app screenshot to $outPng"
