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

function Capture-FormToImages($form, $baseName) {
    $form.StartPosition = [System.Windows.Forms.FormStartPosition]::Manual
    $form.Location = New-Object System.Drawing.Point(-32000, -32000)
    $form.Show()

    [System.Windows.Forms.Application]::DoEvents()
    Start-Sleep -Milliseconds 400
    [System.Windows.Forms.Application]::DoEvents()

    $bmp = New-Object System.Drawing.Bitmap($form.Width, $form.Height)
    $form.DrawToBitmap($bmp, (New-Object System.Drawing.Rectangle(0, 0, $form.Width, $form.Height)))

    $p1 = Join-Path $outDir "$baseName.png"
    $p2 = Join-Path $artifactDir "$baseName.png"
    $bmp.Save($p1, [System.Drawing.Imaging.ImageFormat]::Png)
    $bmp.Save($p2, [System.Drawing.Imaging.ImageFormat]::Png)
    $bmp.Dispose()
    Write-Host "Captured $baseName to $p1" -ForegroundColor Green
}

# Ensure User Session
[NU_Solver.UserSession]::EnsureDefaultAdminIfUnset()

# 1. Capture TranscriptForm
Write-Host "Capturing TranscriptForm..." -ForegroundColor Cyan
$tForm = New-Object NU_Solver.TranscriptForm("2024", "")
Capture-FormToImages $tForm "transcript_form_green_orange"
$tForm.Close()
$tForm.Dispose()

# 2. Capture AdminDashboardForm
Write-Host "Capturing AdminDashboardForm..." -ForegroundColor Cyan
$adminForm = New-Object NU_Solver.AdminDashboardForm
Capture-FormToImages $adminForm "admin_dashboard_green_orange"
$adminForm.Close()
$adminForm.Dispose()

# 3. Capture OperatorLoginForm
Write-Host "Capturing OperatorLoginForm..." -ForegroundColor Cyan
$loginForm = New-Object NU_Solver.OperatorLoginForm("Server=127.0.0.1;Database=NU_TRANSCRIPT;User Id=sa;Password=test;")
Capture-FormToImages $loginForm "operator_login_dialog"
$loginForm.Close()
$loginForm.Dispose()

# 4. Capture UserCredentialViewDialog
Write-Host "Capturing UserCredentialViewDialog..." -ForegroundColor Cyan
$opAcc = [NU_Solver.UserPermissionManager]::Authenticate("operator1", "op123")
if ($opAcc -eq $null) {
    $opAcc = New-Object NU_Solver.UserAccount("operator1", "op123", "Md. Enamul Karim (Section Officer)", "Operator", $true, $true, $false, $false, "Certificate & Transcript Section")
}
$credDialog = New-Object NU_Solver.UserCredentialViewDialog($opAcc, "Data Source=192.168.0.230;Initial Catalog=m_ed;User Id=sa;Password=1234;")
Capture-FormToImages $credDialog "user_credential_view_dialog"
$credDialog.Close()
$credDialog.Dispose()

# 5. Capture CourseDesignDialog - Tab 1 (Academic)
Write-Host "Capturing CourseDesignDialog Tab 1..." -ForegroundColor Cyan
$cdForm = New-Object NU_Solver.CourseDesignDialog

$tabCtrl = $null
foreach ($ctrl in $cdForm.Controls) {
    if ($ctrl -is [System.Windows.Forms.TabControl]) {
        $tabCtrl = $ctrl
        break
    }
}

Capture-FormToImages $cdForm "course_design_tab1_academic"

# Switch to Tab 2 (Stored Procedure)
if ($tabCtrl -ne $null) {
    $tabCtrl.SelectedIndex = 1
    [System.Windows.Forms.Application]::DoEvents()
    Start-Sleep -Milliseconds 200
    [System.Windows.Forms.Application]::DoEvents()
    Capture-FormToImages $cdForm "course_design_tab2_storedproc"

    # Switch to Tab 4 (Report Templates & .rpt)
    $tabCtrl.SelectedIndex = 3
    [System.Windows.Forms.Application]::DoEvents()
    Start-Sleep -Milliseconds 200
    [System.Windows.Forms.Application]::DoEvents()
    Capture-FormToImages $cdForm "course_design_tab4_crystal_reports"
}

$cdForm.Close()
$cdForm.Dispose()

Write-Host "All modern screenshots successfully captured!" -ForegroundColor Green
