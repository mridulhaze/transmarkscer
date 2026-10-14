try {
    $asm = [System.Reflection.Assembly]::Load("CrystalDecisions.CrystalReports.Engine, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304")
    $type = $asm.GetType("CrystalDecisions.CrystalReports.Engine.ReportDocument")
    $doc = [Activator]::CreateInstance($type)
    Write-Host "SUCCESS: CrystalDecisions.CrystalReports.Engine.ReportDocument instantiated!" -ForegroundColor Green
    Write-Host "Methods available on ReportDocument:"
    $type.GetMethods() | Where-Object { $_.Name -in @("Load", "SetDataSource", "ExportToDisk", "PrintToPrinter") } | Select-Object -Unique Name | ForEach-Object { Write-Host " - $($_.Name)" }
} catch {
    Write-Host "ERROR: $($_.Exception.ToString())" -ForegroundColor Red
}
