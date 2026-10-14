# UI Redesign Assumptions Log

This document records operational, architectural, and design assumptions made during the Material 3 UI modernization for the National University Academic Transcript & Certificate System.

---

## Baseline Assumptions (Phase 0)

| ID | Category | Assumption | Evidence / Rationale | Status |
| :--- | :--- | :--- | :--- | :--- |
| **ASM-001** | **Target Framework** | .NET Framework 4.0 Client Profile remains the mandatory target. No modern framework features (C# 6+, .NET Core/5+, WPF) are available. | Verified via `NU H-Type Solver.csproj` (`<TargetFrameworkVersion>v4.0</TargetFrameworkVersion>`, `<TargetFrameworkProfile>Client</TargetFrameworkProfile>`). | **VERIFIED** |
| **ASM-002** | **Document Freezing** | Generated HTML/CSS output from `TranscriptGenerator.cs` must remain 100% byte-identical before and after UI redesign. | Verified via `tools/GenerateBaselineFixture.ps1` and `docs/BASELINE_HASHES.json`. SHA256 hashes generated from fake data fixture. | **VERIFIED** |
| **ASM-003** | **DPI Awareness** | WinForms scaling must function cleanly at 100% (96 DPI), 125% (120 DPI), and 150% (144 DPI) without clipping labels or overlapping buttons. | Needs `app.manifest` DPI awareness and `AutoScaleMode = AutoScaleMode.Dpi` in Phase 1-2. | **ASSUMED** |
| **ASM-004** | **Fast Mode Semantics** | "Fast Mode" checks for a precomputed snapshot table `dbo.transcript_data_MED_{year}_final` when processing a full lot (`reg_no` blank). If present, loads instantly from snapshot rather than executing stored procedure for all rows. | Verified directly in `TranscriptGenerator.cs` lines 174-203. One-line helper text formulated. | **VERIFIED** |
| **ASM-005** | **RBAC Governance** | User permissions (`CanView`, `CanPrint`, `CanEdit`, `CanAdmin`) stored in `user_permissions.json` govern control visibility and enablement identically. | Verified via `UserPermissionManager.cs` and `TranscriptForm.ApplyUserPermissions()`. | **VERIFIED** |
| **ASM-006** | **Offline Operation** | The application operates in closed air-gapped university intranet environments. No web fonts (Google Fonts) or external CDN resources can be used; Segoe UI system font is standard across Windows 7–11. | Specified in `docs/UI_REDESIGN_PROMPT.md` Hard Constraint 6. | **VERIFIED** |
| **ASM-007** | **Database Pre-flight** | Pre-flight validation will compare the current active connection catalog (e.g. `m_ed`) against the course profile's required catalog without issuing destructive queries. | To be verified during Phase 2 implementation. | **ASSUMED** |
