$exePath = (Resolve-Path "NU Solver\bin\Debug\NU H-Type Solver.exe").Path
[System.Reflection.Assembly]::LoadFrom($exePath) | Out-Null
[NU_Solver.TranscriptGenerator]::ClearBrowserPrintHeadersAndFooters()

$reg = Get-ItemProperty -Path "HKCU:\Software\Microsoft\Internet Explorer\PageSetup"
Write-Host "Registry Header: [$($reg.header)]"
Write-Host "Registry Footer: [$($reg.footer)]"
if ([string]::IsNullOrEmpty($reg.header) -and [string]::IsNullOrEmpty($reg.footer)) {
    Write-Host "SUCCESS: Browser print header and footer are completely cleared!" -ForegroundColor Green
} else {
    Write-Host "FAILED: Registry headers/footers not cleared" -ForegroundColor Red
}
