Add-Type -AssemblyName System.Windows.Forms
Add-Type -AssemblyName System.Drawing

$projectRoot = "E:\projects\NU H-Ttanscript_Marksheet_Certificate"
$assemblyPath = Join-Path $projectRoot "NU Solver\bin\Debug\NU H-Type Solver.exe"
[System.Reflection.Assembly]::LoadFrom($assemblyPath) | Out-Null

$results = @()

$testRoles = @(
    @{ Name="Admin"; Role="admin"; View=$true; Print=$true; Edit=$true; Admin=$true },
    @{ Name="Operator (Print Only)"; Role="operator"; View=$true; Print=$true; Edit=$false; Admin=$false },
    @{ Name="Editor (Edit Only)"; Role="editor"; View=$true; Print=$false; Edit=$true; Admin=$false },
    @{ Name="Auditor (View Only)"; Role="auditor"; View=$true; Print=$false; Edit=$false; Admin=$false }
)

foreach ($r in $testRoles) {
    # Set UserSession
    $user = New-Object NU_Solver.UserAccount
    $user.Username = "test_" + $r.Role
    $user.FullName = "Test " + $r.Name
    $user.Role = $r.Role
    $user.CanView = $r.View
    $user.CanPrint = $r.Print
    $user.CanEdit = $r.Edit
    $user.CanManageUsers = $r.Admin

    [NU_Solver.UserSession]::CurrentUser = $user

    $form = New-Object NU_Solver.TranscriptForm("2024", "")
    $form.StartPosition = [System.Windows.Forms.FormStartPosition]::Manual
    $form.Location = New-Object System.Drawing.Point(-32000, -32000)
    $form.Show()
    $form.ApplyUserPermissions()
    [System.Windows.Forms.Application]::DoEvents()

    $btnPrint = [NU_Solver.TranscriptForm].GetField("btnPrint", [System.Reflection.BindingFlags]::NonPublic -bor [System.Reflection.BindingFlags]::Instance).GetValue($form)
    $btnTranscript = [NU_Solver.TranscriptForm].GetField("btnGenerateTranscript", [System.Reflection.BindingFlags]::NonPublic -bor [System.Reflection.BindingFlags]::Instance).GetValue($form)
    $btnCourse = [NU_Solver.TranscriptForm].GetField("btnCourseSettings", [System.Reflection.BindingFlags]::NonPublic -bor [System.Reflection.BindingFlags]::Instance).GetValue($form)
    $btnAdmin = [NU_Solver.TranscriptForm].GetField("btnAdminPanel", [System.Reflection.BindingFlags]::NonPublic -bor [System.Reflection.BindingFlags]::Instance).GetValue($form)

    $results += [PSCustomObject]@{
        Role = $r.Name
        CanView = $r.View
        CanPrint = $r.Print
        CanEdit = $r.Edit
        CanAdmin = $r.Admin
        PrintBtnVisible = $btnPrint.Visible
        GenerateTranscriptEnabled = $btnTranscript.Enabled
        CourseSettingsEnabled = $btnCourse.Enabled
        AdminBtnVisible = $btnAdmin.Visible
    }

    $form.Close()
    $form.Dispose()
}

$results | Format-Table -AutoSize
