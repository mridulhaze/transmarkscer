# Known Defects & Operational Issues Log

This file tracks defects and operational quirks identified in the National University Bangladesh Academic Transcript, Marksheet & Certificate system.

---

## DEFECT-001: SQL Scalar Function Parameter Mismatch on `dbo.GETCGPA`

- **Date Logged:** October 14, 2026
- **Status:** OPEN (Logged as known backend defect; UI must handle gracefully)
- **Severity:** High (Affects live stored procedure execution against certain database versions)
- **Component:** Backend SQL Database `m_ed` / Stored Procedure `[dbo].[BBAMCPCTRANS_lot]`

### Error Message
```text
An insufficient number of arguments were supplied for the procedure or function dbo.GETCGPA.
```

### Analysis & Technical Hypothesis
1. **Signature Evolution**: In historical or earlier schema revisions of database `m_ed`, scalar function `dbo.GETCGPA` accepted a single parameter:
   ```sql
   dbo.GETCGPA (@reg_no VARCHAR(11))
   ```
   In recent database updates or specific course schemas (e.g., M.Ed 2024 revisions or consolidated database variants), `dbo.GETCGPA` was modified to accept an additional examination year parameter:
   ```sql
   dbo.GETCGPA (@reg_no VARCHAR(11), @exm_year VARCHAR(4))
   ```
2. **Procedure Desynchronization**: The active stored procedure `[dbo].[BBAMCPCTRANS_lot]` line 12 still invokes:
   ```sql
   CAST(dbo.GETCGPA(examinee.reg_no) AS DECIMAL(10,2)) AS CGPA
   ```
   without passing `@EXM_YEAR`.
3. **Database Routing Discrepancy**: The active connection string in settings may route to a catalog where `dbo.GETCGPA` has the 2-parameter signature, while the course profile preset assumes the single-parameter legacy procedure.

### UI Handling Requirements (Hard Constraint)
- The UI layer **must not** attempt to alter or drop/create the SQL stored procedure in this redesign scope.
- When this error occurs during generation, the UI must intercept the `SqlException` gracefully:
  - Display a clean, non-disruptive inline error banner / snackbar (never crash or throw unhandled).
  - Provide a user-friendly message: *"Unable to retrieve examination records due to database function parameter mismatch."*
  - Provide a collapsible "Details" expander containing the raw technical stack trace and a one-click "Copy Error" button.
