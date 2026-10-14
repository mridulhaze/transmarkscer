using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace NU_Solver
{
    public enum PrintJobStatus
    {
        Queued,      // TO DO (Navy Blue)
        Processing,  // In Progress (Amber)
        Paused,      // Paused / To Check (Red)
        Completed    // Done / Verified (Green)
    }

    /// <summary>
    /// Represents a course lot print job on the workflow Kanban board.
    /// Supports status lifecycle, progress tracking, and batch report generation.
    /// </summary>
    [Serializable]
    public class CoursePrintJob
    {
        public string JobId { get; set; }
        public string CourseKey { get; set; }
        public string CourseTitle { get; set; }
        public string DocType { get; set; } // Transcript, Certificate, Both
        public string ExamYear { get; set; }
        public string LotNumber { get; set; }
        public string CollegeScope { get; set; }
        public int TotalExaminees { get; set; }
        public int ProcessedCount { get; set; }
        public PrintJobStatus Status { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? CompletedTime { get; set; }
        public string AssignedUser { get; set; }
        public string Notes { get; set; }

        public int ProgressPercent
        {
            get
            {
                if (TotalExaminees <= 0) return 0;
                int p = (int)Math.Round((double)ProcessedCount / TotalExaminees * 100);
                return p > 100 ? 100 : p;
            }
        }

        public CoursePrintJob()
        {
            JobId = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
            Status = PrintJobStatus.Queued;
            StartTime = DateTime.Now;
            TotalExaminees = 100;
            ProcessedCount = 0;
            AssignedUser = "admin";
            CollegeScope = "All Colleges";
        }

        public CoursePrintJob(string id, string courseKey, string courseTitle, string docType, 
            string examYear, string lot, int total, int processed, PrintJobStatus status, string user, string notes = "")
        {
            JobId = id;
            CourseKey = courseKey;
            CourseTitle = courseTitle;
            DocType = docType;
            ExamYear = examYear;
            LotNumber = lot;
            TotalExaminees = total;
            ProcessedCount = processed;
            Status = status;
            StartTime = DateTime.Now;
            AssignedUser = user;
            CollegeScope = "All Colleges";
            Notes = notes;
            if (status == PrintJobStatus.Completed)
            {
                CompletedTime = DateTime.Now;
            }
        }
    }

    /// <summary>
    /// Workflow coordinator managing the list of printing jobs and their state transitions.
    /// Persists to Reports/CourseWorkflowQueue.xml.
    /// </summary>
    public static class CoursePrintWorkflow
    {
        private static List<CoursePrintJob> _jobs = new List<CoursePrintJob>();
        private static readonly string QueueFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports", "CourseWorkflowQueue.xml");

        public static event Action OnQueueChanged;

        static CoursePrintWorkflow()
        {
            LoadQueue();
            if (_jobs.Count == 0)
            {
                SeedDefaultJobs();
            }
        }

        public static List<CoursePrintJob> GetAllJobs()
        {
            return new List<CoursePrintJob>(_jobs);
        }

        private static void SeedDefaultJobs()
        {
            _jobs.Clear();

            // 1. TO DO / Queued (Navy Blue)
            _jobs.Add(new CoursePrintJob("JOB-8773", "MED", "Master of Education (M.Ed) - 2024 Lot #01", 
                "Transcript", "2024", "LOT-MED-24-01", 125, 0, PrintJobStatus.Queued, "operator1", "SP: [dbo].[BBAMCPCTRANS_lot]"));
            
            _jobs.Add(new CoursePrintJob("JOB-8101", "BED", "Bachelor of Education (B.Ed) - 2023 Lot #04", 
                "Certificate", "2023", "LOT-BED-23-04", 320, 0, PrintJobStatus.Queued, "operator1", "Pending signature approval"));

            // 2. PROCESSING / In Progress (Amber)
            _jobs.Add(new CoursePrintJob("JOB-5001", "BBA", "Bachelor of Business Administration (BBA) - 8 Sem Lot", 
                "Transcript", "2023", "LOT-BBA-23-02", 426, 240, PrintJobStatus.Processing, "operator1", "Active spooling on Printer #02"));

            _jobs.Add(new CoursePrintJob("JOB-6202", "CSE", "B.Sc (Hons) in Computer Science & Eng. - Lot #03", 
                "Both", "2023", "LOT-CSE-23-03", 180, 95, PrintJobStatus.Processing, "admin", "Printing with vector guilloche border"));

            // 3. PAUSED / To Check (Red)
            _jobs.Add(new CoursePrintJob("JOB-5501", "MBA", "Master of Business Administration (MBA) - Lot #01", 
                "Certificate", "2023", "LOT-MBA-23-01", 150, 45, PrintJobStatus.Paused, "editor1", "Halted: College #301 mark discrepancy"));

            // 4. DONE / Completed (Green)
            _jobs.Add(new CoursePrintJob("JOB-7001", "MED", "M.Ed First Semester Consolidated Transcript", 
                "Transcript", "2022", "LOT-MED-22-FINAL", 310, 310, PrintJobStatus.Completed, "admin", "Full lot verified & sealed"));

            _jobs.Add(new CoursePrintJob("JOB-4100", "BBA", "BBA 2022 Convocation Certificates", 
                "Certificate", "2022", "LOT-BBA-22-CERT", 580, 580, PrintJobStatus.Completed, "officer_nu", "Signed & dispatched"));

            SaveQueue();
        }

        public static void AddJob(CoursePrintJob job)
        {
            if (job == null) return;
            _jobs.Add(job);
            SaveQueue();
            NotifyChange();
        }

        public static void StartJob(string jobId)
        {
            CoursePrintJob job = _jobs.Find(j => j.JobId == jobId);
            if (job != null)
            {
                job.Status = PrintJobStatus.Processing;
                job.StartTime = DateTime.Now;
                SaveQueue();
                NotifyChange();
            }
        }

        public static void PauseJob(string jobId)
        {
            CoursePrintJob job = _jobs.Find(j => j.JobId == jobId);
            if (job != null)
            {
                job.Status = PrintJobStatus.Paused;
                SaveQueue();
                NotifyChange();
            }
        }

        public static void ResumeJob(string jobId)
        {
            CoursePrintJob job = _jobs.Find(j => j.JobId == jobId);
            if (job != null)
            {
                job.Status = PrintJobStatus.Processing;
                SaveQueue();
                NotifyChange();
            }
        }

        public static void StepJobProgress(string jobId, int step = 10)
        {
            CoursePrintJob job = _jobs.Find(j => j.JobId == jobId);
            if (job != null && job.Status == PrintJobStatus.Processing)
            {
                job.ProcessedCount = Math.Min(job.TotalExaminees, job.ProcessedCount + step);
                if (job.ProcessedCount >= job.TotalExaminees)
                {
                    job.Status = PrintJobStatus.Completed;
                    job.CompletedTime = DateTime.Now;
                }
                SaveQueue();
                NotifyChange();
            }
        }

        public static void MarkJobCompleted(string jobId)
        {
            CoursePrintJob job = _jobs.Find(j => j.JobId == jobId);
            if (job != null)
            {
                job.ProcessedCount = job.TotalExaminees;
                job.Status = PrintJobStatus.Completed;
                job.CompletedTime = DateTime.Now;
                SaveQueue();
                NotifyChange();
            }
        }

        public static void DeleteJob(string jobId)
        {
            _jobs.RemoveAll(j => j.JobId == jobId);
            SaveQueue();
            NotifyChange();
        }

        /// <summary>
        /// Generates a comprehensive HTML lot print report for a specific course print job.
        /// </summary>
        public static string GenerateJobReportHtml(CoursePrintJob job)
        {
            if (job == null) return "<html><body><h3>Job not found</h3></body></html>";

            StringBuilder sb = new StringBuilder();
            sb.Append("<!DOCTYPE html><html><head><meta charset='utf-8'>");
            sb.Append("<style>");
            sb.Append("body { font-family: 'Segoe UI', Tahoma, Arial, sans-serif; margin: 30px; background: #F8FAFC; color: #1E293B; }");
            sb.Append(".report-card { background: white; border-radius: 12px; padding: 30px; box-shadow: 0 4px 15px rgba(0,0,0,0.08); max-width: 800px; margin: 0 auto; border-top: 6px solid #059669; }");
            sb.Append(".header { display: flex; align-items: center; border-bottom: 2px solid #E2E8F0; padding-bottom: 20px; margin-bottom: 20px; }");
            sb.Append(".header h1 { margin: 0; font-size: 22px; color: #0F172A; }");
            sb.Append(".header p { margin: 4px 0 0 0; color: #64748B; font-size: 13px; }");
            sb.Append(".badge { display: inline-block; padding: 4px 12px; border-radius: 20px; font-weight: bold; font-size: 12px; text-transform: uppercase; }");
            sb.Append(".badge-queued { background: #DBEAFE; color: #1E40AF; }");
            sb.Append(".badge-processing { background: #FEF3C7; color: #92400E; }");
            sb.Append(".badge-paused { background: #FEE2E2; color: #991B1B; }");
            sb.Append(".badge-completed { background: #D1FAE5; color: #065F46; }");
            sb.Append(".metrics-grid { display: grid; grid-template-columns: repeat(4, 1fr); gap: 15px; margin: 25px 0; }");
            sb.Append(".metric-box { background: #F1F5F9; border-radius: 8px; padding: 15px; text-align: center; }");
            sb.Append(".metric-box .val { font-size: 24px; font-weight: bold; color: #059669; }");
            sb.Append(".metric-box .lbl { font-size: 11px; color: #64748B; text-transform: uppercase; font-weight: 600; margin-top: 4px; }");
            sb.Append("table { width: 100%; border-collapse: collapse; margin-top: 20px; font-size: 13px; }");
            sb.Append("th, td { border: 1px solid #E2E8F0; padding: 10px 14px; text-align: left; }");
            sb.Append("th { background: #F8FAFC; color: #475569; font-weight: 600; width: 30%; }");
            sb.Append(".progress-bar { width: 100%; height: 16px; background: #E2E8F0; border-radius: 8px; overflow: hidden; margin-top: 15px; }");
            sb.Append(".progress-fill { height: 100%; background: linear-gradient(90deg, #10B981, #059669); transition: width 0.3s; }");
            sb.Append(".footer { margin-top: 30px; text-align: right; color: #94A3B8; font-size: 11px; }");
            sb.Append("</style></head><body>");

            string badgeClass = "badge-queued";
            if (job.Status == PrintJobStatus.Processing) badgeClass = "badge-processing";
            else if (job.Status == PrintJobStatus.Paused) badgeClass = "badge-paused";
            else if (job.Status == PrintJobStatus.Completed) badgeClass = "badge-completed";

            sb.Append("<div class='report-card'>");
            sb.Append("<div class='header'>");
            sb.Append("<div style='flex: 1;'>");
            sb.Append("<h1>NATIONAL UNIVERSITY EXAMINATION DIVISION</h1>");
            sb.Append("<p>Official Course Lot Printing Progress & Audit Report</p>");
            sb.Append("</div>");
            sb.AppendFormat("<div><span class='badge {0}'>{1}</span></div>", badgeClass, job.Status.ToString());
            sb.Append("</div>");

            sb.Append("<div class='metrics-grid'>");
            sb.AppendFormat("<div class='metric-box'><div class='val'>{0}</div><div class='lbl'>Total Lots</div></div>", job.TotalExaminees);
            sb.AppendFormat("<div class='metric-box'><div class='val'>{0}</div><div class='lbl'>Printed</div></div>", job.ProcessedCount);
            sb.AppendFormat("<div class='metric-box'><div class='val'>{0}</div><div class='lbl'>Remaining</div></div>", Math.Max(0, job.TotalExaminees - job.ProcessedCount));
            sb.AppendFormat("<div class='metric-box'><div class='val'>{0}%</div><div class='lbl'>Completed</div></div>", job.ProgressPercent);
            sb.Append("</div>");

            sb.Append("<div class='progress-bar'>");
            sb.AppendFormat("<div class='progress-fill' style='width: {0}%;'></div>", job.ProgressPercent);
            sb.Append("</div>");

            sb.Append("<table>");
            sb.AppendFormat("<tr><th>Job Identifier</th><td><b>{0}</b></td></tr>", job.JobId);
            sb.AppendFormat("<tr><th>Course / Degree</th><td>{0} ({1})</td></tr>", job.CourseTitle, job.CourseKey);
            sb.AppendFormat("<tr><th>Document Format</th><td>{0}</td></tr>", job.DocType);
            sb.AppendFormat("<tr><th>Examination Year</th><td>{0}</td></tr>", job.ExamYear);
            sb.AppendFormat("<tr><th>Lot Sequence</th><td>{0}</td></tr>", job.LotNumber);
            sb.AppendFormat("<tr><th>College Scope</th><td>{0}</td></tr>", job.CollegeScope);
            sb.AppendFormat("<tr><th>Operator / Auditor</th><td>{0}</td></tr>", job.AssignedUser);
            sb.AppendFormat("<tr><th>Started Timestamp</th><td>{0}</td></tr>", job.StartTime.ToString("yyyy-MM-dd HH:mm:ss"));
            sb.AppendFormat("<tr><th>Completed Timestamp</th><td>{0}</td></tr>", job.CompletedTime.HasValue ? job.CompletedTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "In Progress / Pending");
            sb.AppendFormat("<tr><th>Job Notes / Stored Procedure</th><td><code>{0}</code></td></tr>", job.Notes);
            sb.Append("</table>");

            sb.Append("<div class='footer'>");
            sb.AppendFormat("Report generated on {0} by {1} &bull; National University IT Cell", DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss"), UserSession.CurrentUser != null ? UserSession.CurrentUser.FullName : "System Admin");
            sb.Append("</div>");
            sb.Append("</div></body></html>");

            return sb.ToString();
        }

        private static void NotifyChange()
        {
            if (OnQueueChanged != null)
            {
                try { OnQueueChanged(); } catch { }
            }
        }

        public static void SaveQueue()
        {
            try
            {
                string dir = Path.GetDirectoryName(QueueFilePath);
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

                XmlSerializer serializer = new XmlSerializer(typeof(List<CoursePrintJob>));
                using (StreamWriter writer = new StreamWriter(QueueFilePath, false, Encoding.UTF8))
                {
                    serializer.Serialize(writer, _jobs);
                }
            }
            catch { }
        }

        public static void LoadQueue()
        {
            try
            {
                if (!File.Exists(QueueFilePath)) return;
                XmlSerializer serializer = new XmlSerializer(typeof(List<CoursePrintJob>));
                using (StreamReader reader = new StreamReader(QueueFilePath, Encoding.UTF8))
                {
                    _jobs = (List<CoursePrintJob>)serializer.Deserialize(reader);
                }
            }
            catch
            {
                _jobs = new List<CoursePrintJob>();
            }
        }
    }
}
