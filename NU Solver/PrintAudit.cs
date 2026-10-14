using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace NU_Solver
{
    [Serializable]
    public class PrintAuditEntry
    {
        public string Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string PrintedBy { get; set; }
        public string DocumentType { get; set; } // "TRANSCRIPT" or "CERTIFICATE"
        public string ExamYear { get; set; }
        public string CourseTitle { get; set; }
        public string RegNoOrLot { get; set; }
        public int TotalPrinted { get; set; }
        public string CollegesSummary { get; set; }
        public string FilePath { get; set; }
        public string Remarks { get; set; }

        public PrintAuditEntry()
        {
            Id = Guid.NewGuid().ToString("N");
            Timestamp = DateTime.Now;
            PrintedBy = Environment.UserName;
            DocumentType = "TRANSCRIPT";
            ExamYear = "";
            CourseTitle = "";
            RegNoOrLot = "LOT";
            TotalPrinted = 0;
            CollegesSummary = "";
            FilePath = "";
            Remarks = "";
        }
    }

    [Serializable]
    public class PrintAuditLog
    {
        public List<PrintAuditEntry> Entries { get; set; }

        public PrintAuditLog()
        {
            Entries = new List<PrintAuditEntry>();
        }
    }

    public static class PrintAuditManager
    {
        private static string GetAuditLogPath()
        {
            string reportsDir = TranscriptGenerator.GetSafeReportsDirectory();
            return Path.Combine(reportsDir, "PrintAuditLog.xml");
        }

        public static List<PrintAuditEntry> LoadEntries()
        {
            string path = GetAuditLogPath();
            try
            {
                if (File.Exists(path))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(PrintAuditLog));
                    using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                    {
                        PrintAuditLog log = (PrintAuditLog)serializer.Deserialize(fs);
                        if (log != null && log.Entries != null)
                        {
                            return log.Entries;
                        }
                    }
                }
            }
            catch { }
            return new List<PrintAuditEntry>();
        }

        public static void LogOperation(
            string docType, 
            string examYear, 
            string courseTitle, 
            string regNo, 
            DataTable dtExaminees, 
            string filePath, 
            string remarks = "")
        {
            try
            {
                int count = (dtExaminees != null) ? dtExaminees.Rows.Count : 1;
                string regLotText = string.IsNullOrEmpty(regNo) ? "LOT (" + count + " Students)" : "Single: " + regNo;

                // Summarize colleges and counts
                Dictionary<string, int> colCounts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
                if (dtExaminees != null && dtExaminees.Columns.Contains("col_name"))
                {
                    foreach (DataRow r in dtExaminees.Rows)
                    {
                        string cName = r["col_name"] != null ? r["col_name"].ToString().Trim() : "Unknown College";
                        if (!colCounts.ContainsKey(cName)) colCounts[cName] = 0;
                        colCounts[cName]++;
                    }
                }

                StringBuilder sbCol = new StringBuilder();
                foreach (var kvp in colCounts)
                {
                    if (sbCol.Length > 0) sbCol.Append("; ");
                    sbCol.AppendFormat("{0}: {1}", kvp.Key, kvp.Value);
                }

                PrintAuditEntry entry = new PrintAuditEntry
                {
                    Timestamp = DateTime.Now,
                    PrintedBy = Environment.UserName,
                    DocumentType = docType ?? "TRANSCRIPT",
                    ExamYear = examYear ?? "",
                    CourseTitle = courseTitle ?? "",
                    RegNoOrLot = regLotText,
                    TotalPrinted = count,
                    CollegesSummary = sbCol.ToString(),
                    FilePath = filePath ?? "",
                    Remarks = remarks ?? ""
                };

                List<PrintAuditEntry> list = LoadEntries();
                list.Insert(0, entry); // newest first

                // Keep up to 2000 log entries
                if (list.Count > 2000) list.RemoveRange(2000, list.Count - 2000);

                string path = GetAuditLogPath();
                XmlSerializer serializer = new XmlSerializer(typeof(PrintAuditLog));
                PrintAuditLog log = new PrintAuditLog { Entries = list };
                using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    serializer.Serialize(fs, log);
                }
            }
            catch { }
        }

        public static string GenerateAuditReportHtml()
        {
            List<PrintAuditEntry> list = LoadEntries();

            int totalTranscripts = 0;
            int totalCertificates = 0;
            int totalOperations = list.Count;

            foreach (var e in list)
            {
                if (string.Equals(e.DocumentType, "CERTIFICATE", StringComparison.OrdinalIgnoreCase))
                    totalCertificates += e.TotalPrinted;
                else
                    totalTranscripts += e.TotalPrinted;
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<!DOCTYPE html><html><head><meta charset='utf-8'>");
            sb.AppendLine("<title>National University - Print & Generation Audit Report</title>");
            sb.AppendLine("<style>");
            sb.AppendLine("body { font-family: 'Segoe UI', Arial, sans-serif; margin: 20px; color: #1e293b; background: #f8fafc; font-size: 12px; }");
            sb.AppendLine(".container { max-width: 1100px; margin: 0 auto; background: #fff; padding: 24px; border-radius: 8px; box-shadow: 0 4px 12px rgba(0,0,0,0.06); }");
            sb.AppendLine(".header { border-bottom: 2px solid #059669; padding-bottom: 12px; margin-bottom: 18px; }");
            sb.AppendLine(".title { font-size: 20px; font-weight: bold; color: #059669; margin: 0; }");
            sb.AppendLine(".subtitle { font-size: 12px; color: #64748b; margin-top: 4px; }");
            sb.AppendLine(".stats-grid { display: flex; gap: 16px; margin-bottom: 20px; }");
            sb.AppendLine(".stat-card { flex: 1; background: #f1f5f9; padding: 12px 16px; border-radius: 6px; border-left: 4px solid #059669; }");
            sb.AppendLine(".stat-val { font-size: 22px; font-weight: bold; color: #0f172a; }");
            sb.AppendLine(".stat-lbl { font-size: 11px; color: #64748b; text-transform: uppercase; font-weight: 600; }");
            sb.AppendLine("table { width: 100%; border-collapse: collapse; margin-top: 10px; font-size: 11.5px; }");
            sb.AppendLine("th, td { border: 1px solid #cbd5e1; padding: 7px 10px; text-align: left; }");
            sb.AppendLine("th { background: #f8fafc; font-weight: bold; color: #334155; }");
            sb.AppendLine("tr:nth-child(even) { background: #f8fafc; }");
            sb.AppendLine(".badge { display: inline-block; padding: 2px 7px; border-radius: 4px; font-size: 10.5px; font-weight: bold; }");
            sb.AppendLine(".badge-trans { background: #dcfce7; color: #166534; }");
            sb.AppendLine(".badge-cert { background: #e0e7ff; color: #3730a3; }");
            sb.AppendLine("</style></head><body>");

            sb.AppendLine("<div class='container'>");
            sb.AppendLine("  <div class='header'>");
            sb.AppendLine("    <div class='title'>📊 National University - Print & Lot Audit Report</div>");
            sb.AppendFormat("    <div class='subtitle'>Generated on {0:dd-MMM-yyyy HH:mm:ss} | Operator: {1}</div>\r\n", DateTime.Now, Environment.UserName);
            sb.AppendLine("  </div>");

            sb.AppendLine("  <div class='stats-grid'>");
            sb.AppendFormat("    <div class='stat-card'><div class='stat-val'>{0}</div><div class='stat-lbl'>Total Print Operations</div></div>\r\n", totalOperations);
            sb.AppendFormat("    <div class='stat-card'><div class='stat-val'>{0}</div><div class='stat-lbl'>Transcripts Printed</div></div>\r\n", totalTranscripts);
            sb.AppendFormat("    <div class='stat-card'><div class='stat-val'>{0}</div><div class='stat-lbl'>Certificates Printed</div></div>\r\n", totalCertificates);
            sb.AppendFormat("    <div class='stat-card'><div class='stat-val'>{0}</div><div class='stat-lbl'>Total Documents</div></div>\r\n", totalTranscripts + totalCertificates);
            sb.AppendLine("  </div>");

            sb.AppendLine("  <table>");
            sb.AppendLine("    <thead><tr>");
            sb.AppendLine("      <th style='width: 130px;'>Date & Time</th>");
            sb.AppendLine("      <th style='width: 90px;'>Printed By</th>");
            sb.AppendLine("      <th style='width: 105px;'>Document</th>");
            sb.AppendLine("      <th style='width: 65px;'>Year</th>");
            sb.AppendLine("      <th style='width: 60px; text-align: center;'>Qty</th>");
            sb.AppendLine("      <th style='width: 110px;'>Target / Lot</th>");
            sb.AppendLine("      <th>Colleges Breakdown</th>");
            sb.AppendLine("    </tr></thead>");
            sb.AppendLine("    <tbody>");

            if (list.Count == 0)
            {
                sb.AppendLine("      <tr><td colspan='7' style='text-align: center; color: #64748b; padding: 20px;'>No print or generation records logged yet.</td></tr>");
            }
            else
            {
                foreach (var e in list)
                {
                    bool isCert = string.Equals(e.DocumentType, "CERTIFICATE", StringComparison.OrdinalIgnoreCase);
                    sb.AppendLine("      <tr>");
                    sb.AppendFormat("        <td>{0:dd-MMM-yyyy HH:mm}</td>\r\n", e.Timestamp);
                    sb.AppendFormat("        <td>{0}</td>\r\n", e.PrintedBy);
                    sb.AppendFormat("        <td><span class='badge {0}'>{1}</span></td>\r\n", isCert ? "badge-cert" : "badge-trans", e.DocumentType);
                    sb.AppendFormat("        <td>{0}</td>\r\n", e.ExamYear);
                    sb.AppendFormat("        <td style='text-align: center; font-weight: bold;'>{0}</td>\r\n", e.TotalPrinted);
                    sb.AppendFormat("        <td>{0}</td>\r\n", e.RegNoOrLot);
                    sb.AppendFormat("        <td>{0}</td>\r\n", string.IsNullOrEmpty(e.CollegesSummary) ? "-" : e.CollegesSummary);
                    sb.AppendLine("      </tr>");
                }
            }

            sb.AppendLine("    </tbody>");
            sb.AppendLine("  </table>");
            sb.AppendLine("</div></body></html>");

            string reportsDir = TranscriptGenerator.GetSafeReportsDirectory();
            string reportPath = Path.Combine(reportsDir, "Print_Audit_Summary.html");
            File.WriteAllText(reportPath, sb.ToString(), Encoding.UTF8);
            return reportPath;
        }
    }
}
