using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace NU_Solver
{
    public class CourseInfo
    {
        public string Code { get; set; }
        public string Title { get; set; }
        public int Credits { get; set; }
    }

    public class PaperResult
    {
        public string Code { get; set; }
        public string Title { get; set; }
        public int Credit { get; set; }
        public string Grade { get; set; }
        public string Gpa { get; set; }
        public bool IsNonCredit { get; set; }
    }

    public class TranscriptPrintOptions
    {
        public string PageSize { get; set; } // "A4" or "Legal"
        public string RefPrefix { get; set; } // e.g. "NU/Con/Certi/Trans/{EXM_CODE}/{EXM_YEAR}/"
        public string StartRefNo { get; set; } // e.g. "00037420"
        public string StartBookSl { get; set; } // e.g. "0000167"
        public string EntranceQualification { get; set; } // e.g. "BACHELOR OF EDUCATION"
        public string DegreeConferred { get; set; } // e.g. "MASTER OF EDUCATION"
        public string DisciplineSubject { get; set; } // e.g. "MASTER OF EDUCATION"
        public string DurationOfCourse { get; set; } // e.g. "01 (One) Year"
        public string CertificateDegreeTitle { get; set; } // e.g. "Master of Education"
        public string CertificateSerialPrefix { get; set; } // e.g. "NUPC 20- "
        public string IssueDate { get; set; } // e.g. "October 08, 2026"
        public string CourseKey { get; set; } // e.g. "MED"
        public int SemestersCount { get; set; } // e.g. 2, 4, 8
        public int TotalCredits { get; set; }

        // Stored Procedure and Custom RDLC Path
        public string StoredProcedureName { get; set; }
        public string RdlcFilePath { get; set; }

        // Watermark & DEMO options
        public bool IsDemo { get; set; } // true = generate with "SAMPLE" background watermark
        public string WatermarkText { get; set; } // default "SAMPLE"

        // Crystal Reports Typography Customizations
        public string FontStudentName { get; set; }
        public int FontSizeStudentName { get; set; }
        public string FontStyleStudentName { get; set; }
        public string FontDegreeTitle { get; set; }
        public int FontSizeDegreeTitle { get; set; }
        public string FontCertificateBody { get; set; }
        public int FontSizeCertificateBody { get; set; }
        public string FontTranscriptTable { get; set; }
        public int FontSizeTranscriptTable { get; set; }

        public TranscriptPrintOptions()
        {
            PageSize = "A4";
            RefPrefix = "NU/Con/Certi/Trans/{EXM_CODE}/{EXM_YEAR}/";
            StartRefNo = "00037420";
            StartBookSl = "0000167";
            EntranceQualification = "BACHELOR OF EDUCATION";
            DegreeConferred = "MASTER OF EDUCATION";
            DisciplineSubject = "MASTER OF EDUCATION";
            DurationOfCourse = "01 (One) Year";
            CertificateDegreeTitle = "Master of Education";
            CertificateSerialPrefix = "NUPC 20- ";
            IssueDate = DateTime.Now.ToString("MMMM dd, yyyy");
            CourseKey = "MED";
            SemestersCount = 2;
            TotalCredits = 44;
            StoredProcedureName = "dbo.BBAMCPCTRANS_lot";
            RdlcFilePath = "";
            IsDemo = true;
            WatermarkText = "SAMPLE";

            FontStudentName = "Brush Script MT";
            FontSizeStudentName = 24;
            FontStyleStudentName = "Italic";
            FontDegreeTitle = "Old English Text MT";
            FontSizeDegreeTitle = 32;
            FontCertificateBody = "Times New Roman";
            FontSizeCertificateBody = 15;
            FontTranscriptTable = "Segoe UI";
            FontSizeTranscriptTable = 10;
        }

        public static TranscriptPrintOptions FromConfig(CourseDesignConfig cfg)
        {
            TranscriptPrintOptions opt = new TranscriptPrintOptions();
            if (cfg != null)
            {
                opt.CourseKey = cfg.CourseKey;
                opt.DegreeConferred = cfg.DegreeConferred;
                opt.DisciplineSubject = cfg.DisciplineSubject;
                opt.EntranceQualification = cfg.EntranceQualification;
                opt.DurationOfCourse = cfg.DurationOfCourse;
                opt.PageSize = cfg.PageSize ?? "A4";
                opt.RefPrefix = cfg.RefPrefix ?? "NU/Con/Certi/Trans/{EXM_CODE}/{EXM_YEAR}/";
                opt.CertificateDegreeTitle = cfg.CertificateDegreeTitle ?? cfg.DegreeConferred;
                opt.CertificateSerialPrefix = cfg.CertificateSerialPrefix ?? "NUPC 20- ";
                opt.SemestersCount = cfg.SemestersCount > 0 ? cfg.SemestersCount : 2;
                opt.TotalCredits = cfg.TotalCredits > 0 ? cfg.TotalCredits : 44;
                opt.StoredProcedureName = !string.IsNullOrEmpty(cfg.StoredProcedureName) ? cfg.StoredProcedureName : "dbo.BBAMCPCTRANS_lot";
                opt.RdlcFilePath = cfg.RdlcFilePath ?? "";

                opt.FontStudentName = !string.IsNullOrEmpty(cfg.FontStudentName) ? cfg.FontStudentName : "Brush Script MT";
                opt.FontSizeStudentName = cfg.FontSizeStudentName > 0 ? cfg.FontSizeStudentName : 24;
                opt.FontStyleStudentName = !string.IsNullOrEmpty(cfg.FontStyleStudentName) ? cfg.FontStyleStudentName : "Italic";
                opt.FontDegreeTitle = !string.IsNullOrEmpty(cfg.FontDegreeTitle) ? cfg.FontDegreeTitle : "Old English Text MT";
                opt.FontSizeDegreeTitle = cfg.FontSizeDegreeTitle > 0 ? cfg.FontSizeDegreeTitle : 32;
                opt.FontCertificateBody = !string.IsNullOrEmpty(cfg.FontCertificateBody) ? cfg.FontCertificateBody : "Times New Roman";
                opt.FontSizeCertificateBody = cfg.FontSizeCertificateBody > 0 ? cfg.FontSizeCertificateBody : 15;
                opt.FontTranscriptTable = !string.IsNullOrEmpty(cfg.FontTranscriptTable) ? cfg.FontTranscriptTable : "Segoe UI";
                opt.FontSizeTranscriptTable = cfg.FontSizeTranscriptTable > 0 ? cfg.FontSizeTranscriptTable : 10;
            }
            return opt;
        }
    }

    public static class TranscriptGenerator
    {
        private static string cachedLogoBase64 = null;
        private static string cachedSigBase64 = null;

        public static string GetMedConnectionString()
        {
            string baseConn = Database.GetConnectionString();
            try
            {
                SqlConnectionStringBuilder b = new SqlConnectionStringBuilder(baseConn);
                b.InitialCatalog = "m_ed";
                return b.ConnectionString;
            }
            catch
            {
                return "Data Source=192.168.0.230;Initial Catalog=m_ed;Integrated Security=True;";
            }
        }

        public static Dictionary<string, CourseInfo> LoadCourses(string connStr)
        {
            Dictionary<string, CourseInfo> dict = new Dictionary<string, CourseInfo>(StringComparer.OrdinalIgnoreCase);
            try
            {
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    con.Open();
                    string[] courseQueries = new string[]
                    {
                        "SELECT course_code, UPPER(course_name) AS course_name, credits FROM dbo.course",
                        "SELECT course_code, UPPER(course_name) AS course_name, credits FROM dbo.bedhons_courses",
                        "SELECT DISTINCT course_code, UPPER(course_name) AS course_name, credits FROM dbo.examinee_course WHERE course_name IS NOT NULL"
                    };

                    foreach (string query in courseQueries)
                    {
                        try
                        {
                            using (SqlCommand cmd = new SqlCommand(query, con))
                            using (SqlDataReader rdr = cmd.ExecuteReader())
                            {
                                while (rdr.Read())
                                {
                                    string code = rdr["course_code"].ToString().Trim();
                                    string name = rdr["course_name"].ToString().Trim();
                                    name = name.Replace("EXANINATION", "EXAMINATION").Replace("POLICY,PLANNING", "POLICY, PLANNING");

                                    int credits = 4;
                                    bool isNonCredit = false;
                                    if (rdr["credits"] != DBNull.Value)
                                    {
                                        double dVal;
                                        if (double.TryParse(rdr["credits"].ToString(), out dVal))
                                        {
                                            credits = (int)dVal;
                                            if (dVal == 0) isNonCredit = true;
                                        }
                                    }

                                    if (name.IndexOf("SATISFACTORY", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                        name.IndexOf("NON-CREDIT", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                        name.IndexOf("NON CREDIT", StringComparison.OrdinalIgnoreCase) >= 0)
                                    {
                                        isNonCredit = true;
                                        credits = 0;
                                    }

                                    if (!isNonCredit && credits <= 0)
                                    {
                                        credits = 4;
                                    }

                                    if (!dict.ContainsKey(code))
                                    {
                                        dict[code] = new CourseInfo
                                        {
                                            Code = code,
                                            Title = name,
                                            Credits = credits
                                        };
                                    }
                                }
                            }
                        }
                        catch { }
                    }
                }
            }
            catch { }
            return dict;
        }

        public static DataTable FetchTranscriptData(string connStr, string exmYear, string regNo, bool useFastTableIfAvailable)
        {
            return FetchTranscriptData(connStr, exmYear, regNo, useFastTableIfAvailable, null, null);
        }

        public static DataTable FetchTranscriptData(string connStr, string exmYear, string regNo, bool useFastTableIfAvailable, string procName, Action<SqlCommand> onCmdCreated)
        {
            string year = (exmYear ?? "").Trim();
            string reg = string.IsNullOrEmpty(regNo) ? null : regNo.Trim();
            string spName = string.IsNullOrEmpty(procName) ? "dbo.BBAMCPCTRANS_lot" : procName.Trim();

            // Check if user requested lot and fast precomputed table exists
            if (useFastTableIfAvailable && string.IsNullOrEmpty(reg) && (spName.IndexOf("BBAMCPCTRANS", StringComparison.OrdinalIgnoreCase) >= 0 || spName.IndexOf("MED", StringComparison.OrdinalIgnoreCase) >= 0))
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(connStr))
                    {
                        con.Open();
                        string fastTableName = "transcript_data_MED_" + year + "_final";
                        using (SqlCommand chk = new SqlCommand("SELECT OBJECT_ID(N'dbo." + fastTableName + "', N'U')", con))
                        {
                            object obj = chk.ExecuteScalar();
                            if (obj != null && obj != DBNull.Value)
                            {
                                DataTable dtFast = new DataTable();
                                using (SqlCommand cmdFast = new SqlCommand("SELECT * FROM dbo." + fastTableName + " ORDER BY reg_no", con))
                                using (SqlDataAdapter daFast = new SqlDataAdapter(cmdFast))
                                {
                                    daFast.Fill(dtFast);
                                    if (dtFast.Rows.Count > 0)
                                    {
                                        return dtFast;
                                    }
                                }
                            }
                        }
                    }
                }
                catch { }
            }

            // Execute stored procedure directly
            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(spName, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0; // Unlimited timeout for lot processing

                    if (onCmdCreated != null)
                    {
                        try { onCmdCreated(cmd); } catch { }
                    }

                    bool paramsPopulated = false;
                    try
                    {
                        SqlCommandBuilder.DeriveParameters(cmd);
                        if (cmd.Parameters.Count > 0)
                        {
                            foreach (SqlParameter p in cmd.Parameters)
                            {
                                if (p.Direction == ParameterDirection.ReturnValue) continue;
                                string pLower = p.ParameterName.ToLowerInvariant().Replace("@", "");
                                if (pLower.Contains("reg"))
                                {
                                    p.Value = string.IsNullOrEmpty(reg) ? (object)DBNull.Value : reg;
                                    paramsPopulated = true;
                                }
                                else if (pLower.Contains("year"))
                                {
                                    p.Value = year;
                                    paramsPopulated = true;
                                }
                            }
                        }
                    }
                    catch
                    {
                        paramsPopulated = false;
                    }

                    if (!paramsPopulated)
                    {
                        cmd.Parameters.Clear();
                        if (string.IsNullOrEmpty(reg))
                        {
                            cmd.Parameters.Add(new SqlParameter("@reg_no", SqlDbType.VarChar, 20) { Value = DBNull.Value });
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@reg_no", SqlDbType.VarChar, 20) { Value = reg });
                        }

                        cmd.Parameters.Add(new SqlParameter("@EXM_YEAR", SqlDbType.VarChar, 10) { Value = year });
                    }

                    DataTable dt = new DataTable();
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                    return dt;
                }
            }
        }

        public static PaperResult ParsePaper(string raw, Dictionary<string, CourseInfo> courseDict)
        {
            if (string.IsNullOrEmpty(raw)) return null;
            raw = raw.Trim();
            if (raw.Length == 0) return null;

            string code = "";
            string gpa = "";
            string grade = "";
            int credits = -1;

            if (raw.Contains("="))
            {
                string[] parts = raw.Split(new char[] { '=' }, 2);
                code = parts[0].Trim();
                grade = parts.Length > 1 ? parts[1].Trim() : "";
            }
            else if (raw.Length >= 13)
            {
                code = raw.Substring(0, Math.Min(6, raw.Length)).Trim();
                gpa = raw.Length >= 11 ? raw.Substring(7, 4).Trim() : "";
                grade = raw.Length >= 13 ? raw.Substring(11, 2).Trim() : "";
                if (raw.Length >= 27)
                {
                    int parsedCr;
                    if (int.TryParse(raw.Substring(25, 2), out parsedCr))
                    {
                        credits = parsedCr;
                    }
                }
            }
            else
            {
                string[] parts = raw.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length >= 1)
                {
                    code = parts[0].Trim();
                    if (parts.Length >= 2) grade = parts[1].Trim();
                    if (parts.Length >= 3) gpa = parts[2].Trim();
                }
                else
                {
                    return null;
                }
            }

            if (string.IsNullOrEmpty(code)) return null;

            string title = code;
            bool isNonCredit = false;

            if (courseDict != null && courseDict.ContainsKey(code))
            {
                title = courseDict[code].Title;
                if (courseDict[code].Credits == 0)
                {
                    isNonCredit = true;
                    credits = 0;
                }
                else if (credits < 0)
                {
                    credits = courseDict[code].Credits;
                }
            }

            title = title.Replace("EXANINATION", "EXAMINATION").Replace("POLICY,PLANNING", "POLICY, PLANNING");

            // Additional non-credit detection from title or grade
            if (title.IndexOf("SATISFACTORY", StringComparison.OrdinalIgnoreCase) >= 0 ||
                title.IndexOf("NON-CREDIT", StringComparison.OrdinalIgnoreCase) >= 0 ||
                title.IndexOf("NON CREDIT", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                isNonCredit = true;
                credits = 0;
            }

            if (grade.Equals("S", StringComparison.OrdinalIgnoreCase) ||
                grade.Equals("SATISFACTORY", StringComparison.OrdinalIgnoreCase) ||
                grade.Equals("NON-SATISFACTORY", StringComparison.OrdinalIgnoreCase) ||
                grade.Equals("NS", StringComparison.OrdinalIgnoreCase))
            {
                isNonCredit = true;
                credits = 0;
            }

            if (credits == 0)
            {
                isNonCredit = true;
            }
            else if (credits < 0)
            {
                credits = 4;
            }

            // Normalise grade if S
            if (grade.Equals("S", StringComparison.OrdinalIgnoreCase) || grade.Equals("SATISFACTORY", StringComparison.OrdinalIgnoreCase))
            {
                grade = "Satisfactory";
            }
            else if (grade.Equals("NS", StringComparison.OrdinalIgnoreCase) || grade.Equals("NON-SATISFACTORY", StringComparison.OrdinalIgnoreCase))
            {
                grade = "Non-Satisfactory";
            }

            return new PaperResult
            {
                Code = code,
                Title = title,
                Credit = credits,
                Grade = grade,
                Gpa = gpa,
                IsNonCredit = isNonCredit
            };
        }

        public static PaperResult ParseNonCreditField(string raw, int fieldIndex, Dictionary<string, CourseInfo> courseDict, out int assignedSem)
        {
            assignedSem = (fieldIndex >= 1 && fieldIndex <= 8) ? fieldIndex : 1;
            if (string.IsNullOrEmpty(raw)) return null;

            string text = raw.Trim();

            // Check if semester is explicitly embedded (e.g., "S1: 510901=S" or "1st Sem: 510901")
            if (text.StartsWith("S", StringComparison.OrdinalIgnoreCase) && text.Length > 2 && char.IsDigit(text[1]) && (text[2] == ':' || text[2] == ' ' || text[2] == '-'))
            {
                int sNum = text[1] - '0';
                if (sNum >= 1 && sNum <= 8) assignedSem = sNum;
                text = text.Substring(3).Trim();
            }

            string code = "";
            string title = "";
            string grade = "Satisfactory";

            if (text.Contains("="))
            {
                string[] parts = text.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length >= 2)
                {
                    code = parts[0].Trim();
                    if (parts.Length == 2)
                    {
                        grade = parts[1].Trim();
                    }
                    else
                    {
                        title = parts[1].Trim();
                        grade = parts[2].Trim();
                    }
                }
                else if (parts.Length == 1)
                {
                    code = parts[0].Trim();
                }
            }
            else if (text.Contains(":") && !text.Contains("http"))
            {
                string[] parts = text.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length >= 2)
                {
                    code = parts[0].Trim();
                    grade = parts[1].Trim();
                }
                else
                {
                    code = text;
                }
            }
            else
            {
                code = text;
            }

            // Normalize grade
            if (string.Equals(grade, "S", StringComparison.OrdinalIgnoreCase) || string.Equals(grade, "SATISFACTORY", StringComparison.OrdinalIgnoreCase))
            {
                grade = "Satisfactory";
            }
            else if (string.Equals(grade, "NS", StringComparison.OrdinalIgnoreCase) || string.Equals(grade, "NON-SATISFACTORY", StringComparison.OrdinalIgnoreCase))
            {
                grade = "Non-Satisfactory";
            }

            // Look up title from course dictionary if not specified
            if (string.IsNullOrEmpty(title) && courseDict != null && courseDict.ContainsKey(code))
            {
                title = courseDict[code].Title;
            }
            else if (string.IsNullOrEmpty(title))
            {
                title = "Non-Credit Foundation Course (" + code + ")";
            }

            return new PaperResult
            {
                Code = code,
                Title = title,
                Credit = 0,
                Grade = grade,
                Gpa = "",
                IsNonCredit = true
            };
        }

        public static string GetLogoBase64()
        {
            if (!string.IsNullOrEmpty(cachedLogoBase64)) return cachedLogoBase64;
            try
            {
                string baseDir = !string.IsNullOrEmpty(Application.StartupPath) ? Application.StartupPath : AppDomain.CurrentDomain.BaseDirectory;
                string[] candidates = new string[]
                {
                    Path.Combine(baseDir, "logo_NU.png"),
                    Path.Combine(baseDir, "..", "logo_NU.png"),
                    Path.Combine(baseDir, "..", "..", "logo_NU.png"),
                    Path.Combine(@"E:\projects\NU H-Ttanscript_Marksheet_Certificate", "logo_NU.png"),
                    Path.Combine(baseDir, "static", "nu_logo_small.png"),
                    Path.Combine(baseDir, "..", "..", "static", "nu_logo_small.png")
                };

                foreach (string c in candidates)
                {
                    if (File.Exists(c))
                    {
                        byte[] bytes = File.ReadAllBytes(c);
                        cachedLogoBase64 = "data:image/png;base64," + Convert.ToBase64String(bytes);
                        return cachedLogoBase64;
                    }
                }
            }
            catch { }
            return "";
        }

        public static string GetSignatureBase64()
        {
            if (!string.IsNullOrEmpty(cachedSigBase64)) return cachedSigBase64;
            try
            {
                string baseDir = !string.IsNullOrEmpty(Application.StartupPath) ? Application.StartupPath : AppDomain.CurrentDomain.BaseDirectory;
                string[] candidates = new string[]
                {
                    Path.Combine(baseDir, "controller_sinnature_enamulkarim.jpg"),
                    Path.Combine(baseDir, "..", "controller_sinnature_enamulkarim.jpg"),
                    Path.Combine(baseDir, "..", "..", "controller_sinnature_enamulkarim.jpg"),
                    Path.Combine(@"E:\projects\NU H-Ttanscript_Marksheet_Certificate", "controller_sinnature_enamulkarim.jpg")
                };

                foreach (string c in candidates)
                {
                    if (File.Exists(c))
                    {
                        byte[] bytes = File.ReadAllBytes(c);
                        cachedSigBase64 = "data:image/jpeg;base64," + Convert.ToBase64String(bytes);
                        return cachedSigBase64;
                    }
                }
            }
            catch { }
            return "";
        }

        public static string GenerateSvgQr(string text, int size = 68)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<svg width='{0}' height='{0}' viewBox='0 0 100 100' xmlns='http://www.w3.org/2000/svg'>", size);
            sb.Append("<rect width='100' height='100' fill='white'/>");
            
            // Finder patterns
            sb.Append("<rect x='6' y='6' width='28' height='28' fill='black'/><rect x='10' y='10' width='20' height='20' fill='white'/><rect x='14' y='14' width='12' height='12' fill='black'/>");
            sb.Append("<rect x='66' y='6' width='28' height='28' fill='black'/><rect x='70' y='10' width='20' height='20' fill='white'/><rect x='74' y='14' width='12' height='12' fill='black'/>");
            sb.Append("<rect x='6' y='66' width='28' height='28' fill='black'/><rect x='10' y='70' width='20' height='20' fill='white'/><rect x='14' y='74' width='12' height='12' fill='black'/>");
            
            // Timing patterns
            for (int i = 36; i <= 64; i += 4)
            {
                sb.AppendFormat("<rect x='{0}' y='20' width='2' height='2' fill='black'/>", i);
                sb.AppendFormat("<rect x='20' y='{0}' width='2' height='2' fill='black'/>", i);
            }
            
            // Deterministic module pattern per student text
            int hash = (text ?? "").GetHashCode();
            Random rnd = new Random(hash);
            for (int r = 0; r < 20; r++)
            {
                for (int c = 0; c < 20; c++)
                {
                    int x = 6 + c * 4 + 1;
                    int y = 6 + r * 4 + 1;
                    if ((x < 36 && y < 36) || (x > 62 && y < 36) || (x < 36 && y > 62)) continue;
                    if (rnd.Next(100) > 46)
                    {
                        sb.AppendFormat("<rect x='{0}' y='{1}' width='3' height='3' fill='black'/>", x, y);
                    }
                }
            }
            sb.Append("</svg>");
            return sb.ToString();
        }

        private static string GetSemesterOrdinal(int sem)
        {
            switch (sem)
            {
                case 1: return "1ST";
                case 2: return "2ND";
                case 3: return "3RD";
                default: return sem + "TH";
            }
        }

        public static string GetSafeReportsDirectory()
        {
            try
            {
                string baseDir = !string.IsNullOrEmpty(Application.StartupPath) 
                    ? Application.StartupPath 
                    : Path.GetDirectoryName(typeof(TranscriptGenerator).Assembly.Location);

                if (string.IsNullOrEmpty(baseDir))
                    baseDir = AppDomain.CurrentDomain.BaseDirectory;

                string testDir = Path.Combine(baseDir, "Reports");
                if (!Directory.Exists(testDir)) Directory.CreateDirectory(testDir);
                string testFile = Path.Combine(testDir, ".write_test");
                File.WriteAllText(testFile, "ok");
                File.Delete(testFile);
                return testDir;
            }
            catch
            {
                try
                {
                    string docs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    string testDir = Path.Combine(docs, "NU_Solver_Reports");
                    if (!Directory.Exists(testDir)) Directory.CreateDirectory(testDir);
                    return testDir;
                }
                catch
                {
                    return Path.GetTempPath();
                }
            }
        }

        /// <summary>
        /// Clears Internet Explorer / WebBrowser printing headers and footers in Windows Registry
        /// so official transcripts and certificates never print with browser titles, URLs, dates, or page numbers.
        /// </summary>
        public static void ClearBrowserPrintHeadersAndFooters()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Internet Explorer\PageSetup", true))
                {
                    if (key != null)
                    {
                        key.SetValue("header", "");
                        key.SetValue("footer", "");
                    }
                }
            }
            catch { }
        }

        public static string GenerateTranscriptHtml(
            DataTable dtExaminees, 
            Dictionary<string, CourseInfo> courseDict, 
            TranscriptPrintOptions options, 
            out string savedFilePath)
        {
            int nonCreditCount;
            return GenerateTranscriptHtml(dtExaminees, courseDict, options, out savedFilePath, out nonCreditCount);
        }

        public static string GenerateTranscriptHtml(
            DataTable dtExaminees, 
            Dictionary<string, CourseInfo> courseDict, 
            TranscriptPrintOptions options, 
            out string savedFilePath,
            out int totalNonCreditFound)
        {
            if (options == null) options = new TranscriptPrintOptions();
            totalNonCreditFound = 0;

            string reportsDir = GetSafeReportsDirectory();

            string fileTag = (dtExaminees.Rows.Count == 1) ? dtExaminees.Rows[0]["reg_no"].ToString().Trim() : "Lot_" + dtExaminees.Rows.Count;
            string fileName = string.Format("Academic_Transcript_{0}_{1:yyyyMMdd_HHmmss}.html", fileTag, DateTime.Now);
            savedFilePath = Path.Combine(reportsDir, fileName);

            string logoBase64 = GetLogoBase64();
            string sigBase64 = GetSignatureBase64();

            // Detect max semester present across examinees
            int maxSemDetected = 0;
            for (int s = 1; s <= 8; s++)
            {
                for (int p = 1; p <= 10; p++)
                {
                    string colP = "P" + (10 + p) + "_S" + s;
                    if (dtExaminees.Columns.Contains(colP))
                    {
                        for (int r = 0; r < dtExaminees.Rows.Count; r++)
                        {
                            if (dtExaminees.Rows[r][colP] != DBNull.Value && !string.IsNullOrEmpty(dtExaminees.Rows[r][colP].ToString().Trim()))
                            {
                                if (s > maxSemDetected) maxSemDetected = s;
                                break;
                            }
                        }
                    }
                }
            }

            bool is8Semester = (maxSemDetected > 4) || (options.SemestersCount >= 8) ||
                (options.DurationOfCourse != null && (options.DurationOfCourse.Contains("4") || options.DurationOfCourse.IndexOf("Four", StringComparison.OrdinalIgnoreCase) >= 0)) ||
                (options.DegreeConferred != null && (options.DegreeConferred.IndexOf("BBA", StringComparison.OrdinalIgnoreCase) >= 0 || (options.DegreeConferred.IndexOf("BACHELOR", StringComparison.OrdinalIgnoreCase) >= 0 && options.DegreeConferred.IndexOf("HONOURS", StringComparison.OrdinalIgnoreCase) >= 0)));

            bool isLegal = is8Semester || string.Equals(options.PageSize, "Legal", StringComparison.OrdinalIgnoreCase);
            if (is8Semester) options.PageSize = "Legal";

            string pageSizeCss = isLegal ? "legal portrait" : "a4 portrait";

            // Parse base start serials
            long startRefNum = 37420;
            if (!string.IsNullOrEmpty(options.StartRefNo))
            {
                long.TryParse(options.StartRefNo.Trim(), out startRefNum);
            }

            long startBookNum = 167;
            if (!string.IsNullOrEmpty(options.StartBookSl))
            {
                long.TryParse(options.StartBookSl.Trim(), out startBookNum);
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html><head><meta charset='utf-8'>");
            sb.AppendLine("<meta http-equiv='X-UA-Compatible' content='IE=edge'>");
            sb.AppendLine("<title></title>");
            sb.AppendLine("<style>");
            sb.AppendFormat("@page {{ size: {0}; margin: {1}; }}\r\n", pageSizeCss, isLegal ? "6mm 8mm 6mm 8mm" : "10mm 10mm 8mm 10mm");
            sb.AppendLine("*, *:before, *:after { box-sizing: border-box; }");

            string trFont = !string.IsNullOrEmpty(options.FontTranscriptTable) ? options.FontTranscriptTable : "Segoe UI";
            int trSize = options.FontSizeTranscriptTable > 0 ? options.FontSizeTranscriptTable : (isLegal ? 9 : 10);
            sb.AppendFormat("body {{ font-family: '{0}', 'Segoe UI', Arial, Helvetica, sans-serif; margin: 0; padding: 6px; color: #111; background: #eef2f6; font-size: {1}px; }}\r\n", 
                trFont, isLegal ? trSize : trSize + 2);

            sb.AppendLine(isLegal
                ? ".transcript-page { width: 216mm; max-width: 840px; margin: 18px auto; padding: 10px 18px; border: 1px solid #cbd5e1; box-shadow: 0 4px 18px rgba(0,0,0,0.10), 0 1px 3px rgba(0,0,0,0.06); background: #fff; position: relative; box-sizing: border-box; page-break-after: always; break-after: page; page-break-inside: avoid; break-inside: avoid; overflow: hidden; }"
                : ".transcript-page { width: 210mm; max-width: 820px; margin: 18px auto; padding: 14px 22px; border: 1px solid #cbd5e1; box-shadow: 0 4px 18px rgba(0,0,0,0.10), 0 1px 3px rgba(0,0,0,0.06); background: #fff; position: relative; box-sizing: border-box; page-break-after: always; break-after: page; page-break-inside: avoid; break-inside: avoid; overflow: hidden; }");

            sb.AppendLine(".watermark { position: absolute; top: 50%; left: 50%; -ms-transform: translate(-50%, -50%) rotate(-32deg); -webkit-transform: translate(-50%, -50%) rotate(-32deg); transform: translate(-50%, -50%) rotate(-32deg); font-size: 110px; font-weight: 900; font-family: 'Arial Black', Impact, 'Segoe UI', Arial, sans-serif; letter-spacing: 18px; color: rgba(220, 38, 38, 0.12); border: 7px dashed rgba(220, 38, 38, 0.15); padding: 10px 42px; border-radius: 14px; text-transform: uppercase; pointer-events: none; user-select: none; z-index: 10; white-space: nowrap; text-align: center; }");

            sb.AppendLine("@media print {");
            sb.AppendLine("  body { padding: 0 !important; margin: 0 !important; background: none !important; }");
            sb.AppendLine("  .transcript-page { border: none !important; box-shadow: none !important; padding: " + (isLegal ? "2mm 4mm 2mm 4mm" : "4mm 6mm 4mm 6mm") + " !important; margin: 0 !important; width: 100% !important; max-width: 100% !important; page-break-after: always; break-after: page; page-break-inside: avoid; break-inside: avoid; }");
            sb.AppendLine("  .watermark { color: rgba(185, 28, 28, 0.14) !important; border-color: rgba(185, 28, 28, 0.16) !important; -webkit-print-color-adjust: exact !important; print-color-adjust: exact !important; }");
            sb.AppendLine("  .no-print { display: none !important; }");
            sb.AppendLine("}");
            
            // Header Styles
            sb.AppendLine(".header-table { width: 100%; border-collapse: collapse; margin-bottom: 3px; }");
            sb.AppendLine(".header-table td { vertical-align: top; }");
            sb.AppendLine(".top-left { font-size: 11px; line-height: 1.3; padding-top: 2px; }");
            sb.AppendLine(".top-left-sl { font-size: 11px; font-weight: 500; }");
            sb.AppendLine(".top-left-lett { font-size: 10px; margin-top: 14px; color: #111; }");
            
            sb.AppendLine(".center-header { text-align: center; }");
            sb.AppendLine(".brand-row { display: inline-flex; align-items: center; justify-content: center; gap: 8px; }");
            sb.AppendLine(".nu-logo { width: 36px; height: auto; display: inline-block; vertical-align: middle; }");
            sb.AppendLine(".brand-text { display: inline-block; text-align: center; vertical-align: middle; }");
            sb.AppendLine(".nu-title { font-size: 19px; font-weight: bold; font-family: 'Times New Roman', Georgia, serif; line-height: 1; margin: 0; color: #000; }");
            sb.AppendLine(".nu-sub { font-size: 13px; font-family: 'Times New Roman', Georgia, serif; margin: 2px 0 0 0; color: #111; text-align: center; }");
            sb.AppendLine(".nu-heading { font-size: 12px; font-weight: bold; letter-spacing: 0.5px; margin: 5px 0 0 0; text-decoration: underline; text-align: center; }");
            
            // Grading Table Styles (Unified single-table scale bar with seamless border)
            sb.AppendLine(".grading-wrap { display: inline-block; width: " + (isLegal ? "194px" : "198px") + "; text-align: left; }");
            sb.AppendLine(".grading-table { width: 100%; border-collapse: collapse; font-family: 'Arial Narrow', Arial, sans-serif; font-size: " + (isLegal ? "7.2px" : "7.8px") + "; line-height: 1.1; border: 1.5px solid #000; table-layout: fixed; }");
            sb.AppendLine(".grading-table th, .grading-table td { border: 1px solid #000; padding: " + (isLegal ? "1px 2px" : "1.5px 2px") + "; white-space: nowrap; overflow: hidden; }");
            sb.AppendLine(".grading-table th.grading-title-th { text-align: center; font-weight: bold; font-size: " + (isLegal ? "8px" : "8.5px") + "; padding: 2px 0; background: #fff; letter-spacing: 0.5px; border: 1px solid #000; border-bottom: 1.5px solid #000; }");
            sb.AppendLine(".grading-table th.col-num { width: 60%; text-align: center; font-weight: bold; background: #f8fafc; font-size: " + (isLegal ? "7px" : "7.2px") + "; }");
            sb.AppendLine(".grading-table th.col-lg { width: 20%; text-align: center; font-weight: bold; background: #f8fafc; font-size: " + (isLegal ? "7px" : "7.2px") + "; line-height: 1.0; }");
            sb.AppendLine(".grading-table th.col-gp { width: 20%; text-align: center; font-weight: bold; background: #f8fafc; font-size: " + (isLegal ? "7px" : "7.2px") + "; line-height: 1.0; }");
            sb.AppendLine(".grading-table td.nowrap { white-space: nowrap; text-align: center; }");
            sb.AppendLine(".grading-table td.center { text-align: center; }");

            // Info Table Styles
            sb.AppendLine(".info-table { width: 100%; border-collapse: collapse; font-size: " + (isLegal ? "9.5px" : "11px") + "; margin: " + (isLegal ? "2px 0" : "4px 0 3px 0") + "; }");
            sb.AppendLine(".info-table td { padding: " + (isLegal ? "0.5px 2px" : "1px 3px") + "; vertical-align: top; line-height: " + (isLegal ? "1.15" : "1.25") + "; }");
            sb.AppendLine(".info-table td.lbl { width: " + (isLegal ? "180px" : "190px") + "; font-weight: normal; color: #111; }");
            sb.AppendLine(".info-table td.colon { width: 10px; text-align: center; }");
            sb.AppendLine(".info-table td.val { font-weight: bold; color: #000; }");

            // Marks Table Styles
            sb.AppendLine(".attain-title { text-align: center; font-size: " + (isLegal ? "11px" : "12px") + "; font-weight: bold; margin: " + (isLegal ? "2px 0 1px 0" : "4px 0 2px 0") + "; }");
            sb.AppendLine(".marks-table { width: 100%; border-collapse: collapse; font-size: " + (isLegal ? "8.5px" : "10px") + "; }");
            sb.AppendLine(".marks-table th, .marks-table td { border: 1px solid #000; padding: " + (isLegal ? "1px 3px" : "2px 4px") + "; line-height: " + (isLegal ? "1.12" : "1.15") + "; }");
            sb.AppendLine(".marks-table th { font-weight: bold; text-align: center; background: #fff; font-size: " + (isLegal ? "8px" : "9.5px") + "; }");
            sb.AppendLine(".marks-table td.center { text-align: center; }");
            sb.AppendLine(".marks-table td.bold { font-weight: bold; }");
            sb.AppendLine(".grade-val { display: inline-block; width: 24px; text-align: left; font-weight: bold; font-family: inherit; }");

            // Footer Styles
            sb.AppendLine(".footer-table { width: 100%; border-collapse: collapse; margin-top: " + (isLegal ? "4px" : "8px") + "; font-size: " + (isLegal ? "9.5px" : "10.5px") + "; }");
            sb.AppendLine(".footer-table td { vertical-align: bottom; }");
            sb.AppendLine(".sig-block { text-align: center; float: right; min-width: 180px; }");
            sb.AppendLine(".sig-img { width: " + (isLegal ? "105px" : "120px") + "; height: auto; display: block; margin: 0 auto 2px auto; }");
            sb.AppendLine("</style></head><body>");

            for (int rIdx = 0; rIdx < dtExaminees.Rows.Count; rIdx++)
            {
                DataRow r = dtExaminees.Rows[rIdx];

                string stdName = r["std_name"] != null ? r["std_name"].ToString().Trim() : "";
                string fname = r["fname"] != null ? r["fname"].ToString().Trim() : "";
                string mname = r["mname"] != null ? r["mname"].ToString().Trim() : "";
                string colName = r["col_name"] != null ? r["col_name"].ToString().Trim() : "";
                string exmRoll = r["exm_roll"] != null ? r["exm_roll"].ToString().Trim() : "";
                string regNo = r["reg_no"] != null ? r["reg_no"].ToString().Trim() : "";
                string session = r["SESSION"] != null ? r["SESSION"].ToString().Trim() : "";
                string exmYear = r["exm_year"] != null ? r["exm_year"].ToString().Trim() : "";
                string exmCode = r["exm_code"] != null ? r["exm_code"].ToString().Trim() : "8773";
                string sess1 = r["SESS1"] != null ? r["SESS1"].ToString().Trim() : (session.Length >= 4 ? session.Substring(2, 2) : "23");
                string cCredit = r["C_CREDIT"] != null ? r["C_CREDIT"].ToString().Trim() : "";
                string tCredit = r["T_CREDIT"] != null ? r["T_CREDIT"].ToString().Trim() : "";
                string cgpa = r["CGPA"] != null ? r["CGPA"].ToString().Trim() : "";
                string resDate = r["RES_DATE"] != null ? r["RES_DATE"].ToString().Trim() : "June 14 , 2026";

                if (string.IsNullOrEmpty(tCredit) || (is8Semester && tCredit == "44" && options.TotalCredits > 44))
                {
                    tCredit = options.TotalCredits.ToString();
                }
                if (string.IsNullOrEmpty(cCredit) || (is8Semester && cCredit == "44" && options.TotalCredits > 44))
                {
                    cCredit = tCredit;
                }

                // Generate book serial
                long currentBookNum = startBookNum + rIdx;
                string bookSlFormatted = string.Format("{0:D7}", currentBookNum);

                // Generate dynamic Reference for every different course and lot
                long currentRefNum = startRefNum + rIdx;
                string refSlFormatted = string.Format("{0:D8}(1)", currentRefNum);

                string formattedRef;
                if (!string.IsNullOrEmpty(r["CRT_SL"] != null ? r["CRT_SL"].ToString().Trim() : ""))
                {
                    formattedRef = r["CRT_SL"].ToString().Trim();
                    if (!formattedRef.StartsWith("NU/", StringComparison.OrdinalIgnoreCase) && !formattedRef.StartsWith("Lett", StringComparison.OrdinalIgnoreCase))
                    {
                        formattedRef = string.Format("NU/Con/Certi/Trans/{0}/{1}/{2}", exmCode, exmYear, formattedRef);
                    }
                }
                else
                {
                    string prefix = (options.RefPrefix ?? "").Trim();
                    prefix = prefix.Replace("{EXM_CODE}", exmCode).Replace("{EXM_YEAR}", exmYear);
                    if (!prefix.EndsWith("/")) prefix += "/";
                    formattedRef = prefix + refSlFormatted;
                }

                if (!formattedRef.StartsWith("Lett:", StringComparison.OrdinalIgnoreCase))
                {
                    formattedRef = "Lett: " + formattedRef;
                }

                string qrText = string.Format("Reg:{0},Roll:{1},Name:{2},CGPA:{3},Degree:{4},Year:{5},RES_DATE:{6}", 
                    regNo, exmRoll, stdName, cgpa, options.DegreeConferred, exmYear, resDate);
                string qrSvg = GenerateSvgQr(qrText, 64);

                // Parse Semester Papers (supports 1 to 8 semesters dynamically)
                // Separate into Credit Papers (for main table) and Non-Credit Papers (for separate section below)
                Dictionary<int, List<PaperResult>> semCreditPapers = new Dictionary<int, List<PaperResult>>();
                Dictionary<int, string> semGpa = new Dictionary<int, string>();
                List<KeyValuePair<int, PaperResult>> nonCreditPapers = new List<KeyValuePair<int, PaperResult>>();

                for (int sem = 1; sem <= 8; sem++)
                {
                    List<PaperResult> creditList = new List<PaperResult>();
                    for (int pIdx = 1; pIdx <= 10; pIdx++)
                    {
                        string colNameP = "P" + (10 + pIdx) + "_S" + sem;
                        if (dtExaminees.Columns.Contains(colNameP) && r[colNameP] != DBNull.Value)
                        {
                            PaperResult p = ParsePaper(r[colNameP].ToString(), courseDict);
                            if (p != null)
                            {
                                if (p.IsNonCredit || p.Credit == 0)
                                {
                                    nonCreditPapers.Add(new KeyValuePair<int, PaperResult>(sem, p));
                                    totalNonCreditFound++;
                                }
                                else
                                {
                                    creditList.Add(p);
                                }
                            }
                        }
                    }

                    if (creditList.Count > 0)
                    {
                        semCreditPapers[sem] = creditList;
                        string gpaCol = "S" + sem + "_GPA";
                        string gVal = (dtExaminees.Columns.Contains(gpaCol) && r[gpaCol] != DBNull.Value) 
                            ? r[gpaCol].ToString().Trim() : "";
                        semGpa[sem] = gVal;
                    }
                }

                // Check dedicated non-credit fields: noncredit_1 .. noncredit_5
                for (int nc = 1; nc <= 5; nc++)
                {
                    string[] possibleColNames = new string[]
                    {
                        "noncredit_" + nc,
                        "non_credit_" + nc,
                        "noncredit" + nc,
                        "non_credit" + nc
                    };

                    string rawNcVal = null;
                    foreach (string colCand in possibleColNames)
                    {
                        if (dtExaminees.Columns.Contains(colCand) && r[colCand] != DBNull.Value)
                        {
                            string s = r[colCand].ToString().Trim();
                            if (!string.IsNullOrEmpty(s))
                            {
                                rawNcVal = s;
                                break;
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(rawNcVal))
                    {
                        int semAssigned;
                        PaperResult ncPaper = ParseNonCreditField(rawNcVal, nc, courseDict, out semAssigned);
                        if (ncPaper != null)
                        {
                            // Avoid adding duplicate course code if already parsed from semester slots
                            bool alreadyExists = false;
                            for (int k = 0; k < nonCreditPapers.Count; k++)
                            {
                                if (string.Equals(nonCreditPapers[k].Value.Code, ncPaper.Code, StringComparison.OrdinalIgnoreCase))
                                {
                                    alreadyExists = true;
                                    break;
                                }
                            }

                            if (!alreadyExists)
                            {
                                nonCreditPapers.Add(new KeyValuePair<int, PaperResult>(semAssigned, ncPaper));
                                totalNonCreditFound++;
                            }
                        }
                    }
                }

                // Compute total row count across credit papers for CGPA spanning (NO DIVIDER)
                int totalAllPapers = 0;
                foreach (var list in semCreditPapers.Values)
                {
                    totalAllPapers += list.Count;
                }
                if (totalAllPapers == 0) totalAllPapers = 1;

                sb.AppendLine("<div class='transcript-page'>");
                if (options != null && options.IsDemo)
                {
                    string wm = string.IsNullOrEmpty(options.WatermarkText) ? "SAMPLE" : options.WatermarkText;
                    sb.AppendFormat("  <div class='watermark'>{0}</div>\r\n", wm);
                }

                // Header Table
                sb.AppendLine("  <table class='header-table'><tr>");
                
                // Top Left: .No. NUT 23- 0000167 and Lett: reference below
                sb.AppendLine("    <td style='width: 198px;' class='top-left'>");
                sb.AppendFormat("      <div class='top-left-sl'>.No. NUT {0}- {1}</div>\r\n", sess1, bookSlFormatted);
                sb.AppendFormat("      <div class='top-left-lett'>{0}</div>\r\n", formattedRef);
                sb.AppendLine("    </td>");

                // Top Center: Logo next to National University Bangladesh
                sb.AppendLine("    <td class='center-header'>");
                sb.AppendLine("      <div class='brand-row'>");
                if (!string.IsNullOrEmpty(logoBase64))
                {
                    sb.AppendFormat("        <img src='{0}' class='nu-logo' alt='NU Crest' />\r\n", logoBase64);
                }
                sb.AppendLine("        <div class='brand-text'>");
                sb.AppendLine("          <div class='nu-title'>National University</div>");
                sb.AppendLine("          <div class='nu-sub'>Bangladesh</div>");
                sb.AppendLine("        </div>");
                sb.AppendLine("      </div>");
                sb.AppendLine("      <div class='nu-heading'>ACADEMIC TRANSCRIPT</div>");
                sb.AppendLine("    </td>");

                // Top Right: Grading System Box (Unified single-table scale bar with seamless border)
                sb.AppendLine("    <td style='width: 198px; text-align: right; vertical-align: top;'>");
                sb.AppendLine("      <div class='grading-wrap'>");
                sb.AppendLine("        <table class='grading-table'>");
                sb.AppendLine("          <thead>");
                sb.AppendLine("            <tr>");
                sb.AppendLine("              <th colspan='3' class='grading-title-th'>GRADING SYSTEM</th>");
                sb.AppendLine("            </tr>");
                sb.AppendLine("            <tr>");
                sb.AppendLine("              <th class='col-num'>Numerical grade</th>");
                sb.AppendLine("              <th class='col-lg'>Letter<br/>Grade</th>");
                sb.AppendLine("              <th class='col-gp'>Grade<br/>Point</th>");
                sb.AppendLine("            </tr>");
                sb.AppendLine("          </thead>");
                sb.AppendLine("          <tbody>");
                sb.AppendLine("            <tr><td class='nowrap'>80% and Above</td><td class='center'>A+</td><td class='center'>4.00</td></tr>");
                sb.AppendLine("            <tr><td class='nowrap'>75% to less than 80%</td><td class='center'>A</td><td class='center'>3.75</td></tr>");
                sb.AppendLine("            <tr><td class='nowrap'>70% to less than 75%</td><td class='center'>A-</td><td class='center'>3.50</td></tr>");
                sb.AppendLine("            <tr><td class='nowrap'>65% to less than 70%</td><td class='center'>B+</td><td class='center'>3.25</td></tr>");
                sb.AppendLine("            <tr><td class='nowrap'>60% to less than 65%</td><td class='center'>B</td><td class='center'>3.00</td></tr>");
                sb.AppendLine("            <tr><td class='nowrap'>55% to less than 60%</td><td class='center'>B-</td><td class='center'>2.75</td></tr>");
                sb.AppendLine("            <tr><td class='nowrap'>50% to less than 55%</td><td class='center'>C+</td><td class='center'>2.50</td></tr>");
                sb.AppendLine("            <tr><td class='nowrap'>45% to less than 50%</td><td class='center'>C</td><td class='center'>2.25</td></tr>");
                sb.AppendLine("            <tr><td class='nowrap'>40% to less than 45%</td><td class='center'>D</td><td class='center'>2.00</td></tr>");
                sb.AppendLine("            <tr><td class='nowrap'>Below 40%</td><td class='center'>F</td><td class='center'>0.00</td></tr>");
                sb.AppendLine("          </tbody>");
                sb.AppendLine("        </table>");
                sb.AppendLine("      </div>");
                sb.AppendLine("    </td>");
                sb.AppendLine("  </tr></table>");

                // Student Info Table
                sb.AppendLine("  <table class='info-table'>");
                sb.AppendFormat("    <tr><td class='lbl'>Name of the Student</td><td class='colon'>:</td><td class='val'>{0}</td></tr>\r\n", stdName);
                sb.AppendFormat("    <tr><td class='lbl'>Father's Name</td><td class='colon'>:</td><td class='val'>{0}</td></tr>\r\n", fname);
                sb.AppendFormat("    <tr><td class='lbl'>Mother's Name</td><td class='colon'>:</td><td class='val'>{0}</td></tr>\r\n", mname);
                sb.AppendFormat("    <tr><td class='lbl'>Name of Institution</td><td class='colon'>:</td><td class='val'>{0}</td></tr>\r\n", colName);
                sb.AppendFormat("    <tr><td class='lbl'>Entrance Qualification</td><td class='colon'>:</td><td class='val'>{0}</td></tr>\r\n", options.EntranceQualification);
                sb.AppendFormat("    <tr><td class='lbl'>Degree Conferred</td><td class='colon'>:</td><td class='val'>{0}</td></tr>\r\n", options.DegreeConferred);
                sb.AppendFormat("    <tr><td class='lbl'>Discipline/Subject</td><td class='colon'>:</td><td class='val'>{0}</td></tr>\r\n", options.DisciplineSubject);
                sb.AppendFormat("    <tr><td class='lbl'>Duration of Course</td><td class='colon'>:</td><td class='val'>{0}</td></tr>\r\n", options.DurationOfCourse);
                sb.AppendFormat("    <tr><td class='lbl'>Examination Roll No.</td><td class='colon'>:</td><td class='val'>{0}</td></tr>\r\n", exmRoll);
                sb.AppendFormat("    <tr><td class='lbl'>Registration No.</td><td class='colon'>:</td><td class='val'>{0}</td></tr>\r\n", regNo);
                sb.AppendFormat("    <tr><td class='lbl'>Session</td><td class='colon'>:</td><td class='val'>{0}</td></tr>\r\n", session);
                sb.AppendFormat("    <tr><td class='lbl'>Year of Passing</td><td class='colon'>:</td><td class='val'>{0}</td></tr>\r\n", exmYear);
                sb.AppendFormat("    <tr><td class='lbl'>Earned Credit</td><td class='colon'>:</td><td class='val'>{0} Out of {1}</td></tr>\r\n", cCredit, tCredit);
                sb.AppendFormat("    <tr><td class='lbl'>CGPA</td><td class='colon'>:</td><td class='val'>{0} Out of 4.00</td></tr>\r\n", cgpa);
                sb.AppendLine("  </table>");

                // Academic Attainments Table
                sb.AppendLine("  <div class='attain-title'>Academic Attainments</div>");
                sb.AppendLine("  <table class='marks-table'>");
                sb.AppendLine("    <thead><tr>");
                sb.AppendLine("      <th style='width: 62px;'>SEMESTER</th>");
                sb.AppendLine("      <th style='width: 72px;'>COURSE CODE</th>");
                sb.AppendLine("      <th>COURSE TITLE</th>");
                sb.AppendLine("      <th style='width: 42px;'>CREDIT</th>");
                sb.AppendLine("      <th style='width: 50px;'>LETTER GRADE</th>");
                sb.AppendLine("      <th style='width: 58px;'>SEMESTER GPA</th>");
                sb.AppendLine("      <th style='width: 58px;'>CGPA</th>");
                sb.AppendLine("    </tr></thead>");
                sb.AppendLine("    <tbody>");

                // Render Semesters dynamically
                bool cgpaEmitted = false;
                foreach (var kvp in semCreditPapers)
                {
                    int semNum = kvp.Key;
                    List<PaperResult> pList = kvp.Value;
                    string sGpa = semGpa.ContainsKey(semNum) ? semGpa[semNum] : "";
                    string semLabel = GetSemesterOrdinal(semNum);

                    for (int pIdx = 0; pIdx < pList.Count; pIdx++)
                    {
                        PaperResult p = pList[pIdx];
                        sb.AppendLine("    <tr>");

                        if (pIdx == 0)
                        {
                            sb.AppendFormat("      <td class='center bold' rowspan='{0}'>{1}</td>\r\n", pList.Count, semLabel);
                        }

                        sb.AppendFormat("      <td class='center'>{0}</td>\r\n", p.Code);
                        sb.AppendFormat("      <td>{0}</td>\r\n", p.Title);
                        sb.AppendFormat("      <td class='center'>{0}</td>\r\n", p.Credit);
                        sb.AppendFormat("      <td class='center'><span class='grade-val'>{0}</span></td>\r\n", p.Grade);

                        if (pIdx == 0)
                        {
                            sb.AppendFormat("      <td class='center bold' rowspan='{0}'>{1}</td>\r\n", pList.Count, sGpa);
                        }

                        // CGPA column: rendered ONLY ONCE on the very first row with rowspan = totalAllPapers.
                        // Has NO horizontal divider across semesters!
                        if (!cgpaEmitted)
                        {
                            sb.AppendFormat("      <td class='center bold' rowspan='{0}' style='vertical-align: middle; font-size: {1}px;'>{2}</td>\r\n", 
                                totalAllPapers, isLegal ? 10 : 11, cgpa);
                            cgpaEmitted = true;
                        }

                        sb.AppendLine("    </tr>");
                    }
                }

                sb.AppendLine("    </tbody>");
                sb.AppendLine("  </table>");

                // Render Non-Credit Course(s) and Grades in another portion under main transcript grades if present
                if (nonCreditPapers.Count > 0)
                {
                    sb.AppendLine("  <div class='attain-title non-credit-title' style='margin-top: 5px; text-align: left; font-size: 10px; font-weight: bold;'>Non-Credit Course(s) and Grades :</div>");
                    sb.AppendLine("  <table class='marks-table non-credit-table' style='margin-bottom: 2px;'>");
                    sb.AppendLine("    <thead><tr>");
                    sb.AppendLine("      <th style='width: 62px;'>SEMESTER</th>");
                    sb.AppendLine("      <th style='width: 72px;'>COURSE CODE</th>");
                    sb.AppendLine("      <th>COURSE TITLE</th>");
                    sb.AppendLine("      <th style='width: 42px;'>CREDIT</th>");
                    sb.AppendLine("      <th colspan='3' style='width: 166px;'>LETTER GRADE / STATUS</th>");
                    sb.AppendLine("    </tr></thead>");
                    sb.AppendLine("    <tbody>");

                    for (int i = 0; i < nonCreditPapers.Count; i++)
                    {
                        var item = nonCreditPapers[i];
                        int ncSem = item.Key;
                        PaperResult ncPaper = item.Value;
                        string ncSemLabel = GetSemesterOrdinal(ncSem);

                        int semSpan = 1;
                        if (i == 0 || nonCreditPapers[i - 1].Key != ncSem)
                        {
                            while (i + semSpan < nonCreditPapers.Count && nonCreditPapers[i + semSpan].Key == ncSem)
                            {
                                semSpan++;
                            }
                        }
                        else
                        {
                            semSpan = 0;
                        }

                        sb.AppendLine("    <tr>");
                        if (semSpan > 0)
                        {
                            sb.AppendFormat("      <td class='center bold' rowspan='{0}'>{1}</td>\r\n", semSpan, ncSemLabel);
                        }
                        sb.AppendFormat("      <td class='center'>{0}</td>\r\n", ncPaper.Code);
                        sb.AppendFormat("      <td>{0}</td>\r\n", ncPaper.Title);
                        sb.AppendFormat("      <td class='center'>Non-Credit</td>\r\n");
                        string ncGrade = string.IsNullOrEmpty(ncPaper.Grade) ? "Satisfactory" : ncPaper.Grade;
                        if (ncGrade.Length <= 3)
                        {
                            sb.AppendFormat("      <td class='center' colspan='3'><span class='grade-val'>{0}</span></td>\r\n", ncGrade);
                        }
                        else
                        {
                            sb.AppendFormat("      <td class='center bold' colspan='3'>{0}</td>\r\n", ncGrade);
                        }
                        sb.AppendLine("    </tr>");
                    }

                    sb.AppendLine("    </tbody>");
                    sb.AppendLine("  </table>");
                }

                // Footer Table
                sb.AppendLine("  <table class='footer-table'><tr>");
                sb.AppendLine("    <td style='width: 52%;'>");
                sb.AppendFormat("      <!-- QR_PAYLOAD: {0} -->\r\n", qrText);
                sb.AppendFormat("      <div class='qr-box' data-qr='{0}' title='{0}'>{1}</div>\r\n", qrText.Replace("\"", "&quot;"), qrSvg);
                sb.AppendFormat("      <div style='font-size: 10px; margin-top: 3px;'><b>Result Published Date :</b> {0}</div>\r\n", resDate);
                sb.AppendFormat("      <div style='font-size: 9.5px; color: #444; margin-top: 1px;'>Generated on {0:dd-MM-yyyy} by ICT Department, NU</div>\r\n", DateTime.Now);
                sb.AppendLine("    </td>");

                sb.AppendLine("    <td style='width: 48%; text-align: right;'>");
                sb.AppendLine("      <div class='sig-block'>");
                if (!string.IsNullOrEmpty(sigBase64))
                {
                    sb.AppendFormat("        <img src='{0}' class='sig-img' alt='Controller of Examinations Signature' />\r\n", sigBase64);
                }
                else
                {
                    sb.AppendLine("        <div style='height: 38px;'></div>");
                }
                sb.AppendLine("        <div style='font-weight: bold;'>Controller of Examinations</div>");
                sb.AppendLine("        <div>National University, Bangladesh</div>");
                sb.AppendLine("        <div style='font-size: 9.5px; color: #333;'>E-mail: controller@nu.ac.bd</div>");
                sb.AppendLine("      </div>");
                sb.AppendLine("    </td>");
                sb.AppendLine("  </tr></table>");

                sb.AppendLine("</div>"); // .transcript-page
            }

            sb.AppendLine("</body></html>");

            File.WriteAllText(savedFilePath, sb.ToString(), Encoding.UTF8);

            // Log operation in print audit
            string lotReg = (dtExaminees.Rows.Count == 1) ? dtExaminees.Rows[0]["reg_no"].ToString().Trim() : "";
            string examYr = dtExaminees.Rows.Count > 0 && dtExaminees.Rows[0]["exm_year"] != null ? dtExaminees.Rows[0]["exm_year"].ToString().Trim() : "";
            PrintAuditManager.LogOperation("TRANSCRIPT", examYr, options.DegreeConferred, lotReg, dtExaminees, savedFilePath);

            return savedFilePath;
        }

        public static string GenerateCertificateHtml(
            DataTable dtExaminees, 
            TranscriptPrintOptions options, 
            out string savedFilePath)
        {
            if (options == null) options = new TranscriptPrintOptions();

            string reportsDir = GetSafeReportsDirectory();

            string fileTag = (dtExaminees.Rows.Count == 1) ? dtExaminees.Rows[0]["reg_no"].ToString().Trim() : "Lot_" + dtExaminees.Rows.Count;
            string fileName = string.Format("Certificate_{0}_{1:yyyyMMdd_HHmmss}.html", fileTag, DateTime.Now);
            savedFilePath = Path.Combine(reportsDir, fileName);

            string logoBase64 = GetLogoBase64();
            string sigBase64 = GetSignatureBase64();

            long startRefNum = 338680;
            if (!string.IsNullOrEmpty(options.StartRefNo))
            {
                long.TryParse(options.StartRefNo.Trim(), out startRefNum);
            }

            // Typography configurations from options
            string fontStudent = !string.IsNullOrEmpty(options.FontStudentName) ? options.FontStudentName : "Brush Script MT";
            int fontStudentSize = options.FontSizeStudentName > 0 ? options.FontSizeStudentName : 24;
            string fontStudentStyle = !string.IsNullOrEmpty(options.FontStyleStudentName) ? options.FontStyleStudentName.ToLower() : "italic";
            bool isStudentBold = fontStudentStyle.Contains("bold");
            bool isStudentItalic = fontStudentStyle.Contains("italic");

            string fontDegree = !string.IsNullOrEmpty(options.FontDegreeTitle) ? options.FontDegreeTitle : "Old English Text MT";
            int fontDegreeSize = options.FontSizeDegreeTitle > 0 ? options.FontSizeDegreeTitle : 32;

            string fontBody = !string.IsNullOrEmpty(options.FontCertificateBody) ? options.FontCertificateBody : "Times New Roman";
            int fontBodySize = options.FontSizeCertificateBody > 0 ? options.FontSizeCertificateBody : 15;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html><head><meta charset='utf-8'>");
            sb.AppendLine("<meta http-equiv='X-UA-Compatible' content='IE=edge'>");
            sb.AppendLine("<title></title>");
            sb.AppendLine("<style>");
            sb.AppendLine("@page { size: A4 landscape; margin: 8mm 10mm; }");
            sb.AppendLine("*, *:before, *:after { box-sizing: border-box; }");
            sb.AppendFormat("body {{ font-family: '{0}', 'Times New Roman', Georgia, serif; margin: 0; padding: 6px; color: #111; background: #eef2f6; font-size: {1}px; }}\r\n", fontBody, fontBodySize);
            
            // Certificate Container & Ornate Security Frame (Fixed Printable A4 Landscape Paper Sheet)
            sb.AppendLine(".cert-page { width: 297mm; max-width: 1060px; min-height: 640px; margin: 18px auto; padding: 34px 44px; background: #fff; position: relative; box-sizing: border-box; page-break-after: always; break-after: page; page-break-inside: avoid; break-inside: avoid; box-shadow: 0 4px 18px rgba(0,0,0,0.10), 0 1px 3px rgba(0,0,0,0.06); border: 1px solid #cbd5e1; overflow: hidden; }");
            sb.AppendLine(".cert-frame-svg { position: absolute; left: 0; top: 0; width: 100%; height: 100%; pointer-events: none; z-index: 0; }");
            sb.AppendLine(".cert-content-layer { position: relative; z-index: 1; }");
            sb.AppendLine(".cert-watermark { position: absolute; top: 50%; left: 50%; -ms-transform: translate(-50%, -50%) rotate(-25deg); -webkit-transform: translate(-50%, -50%) rotate(-25deg); transform: translate(-50%, -50%) rotate(-25deg); font-size: 120px; font-weight: 900; font-family: 'Arial Black', Impact, 'Segoe UI', Arial, sans-serif; letter-spacing: 20px; color: rgba(29, 78, 216, 0.12); border: 7px dashed rgba(29, 78, 216, 0.15); padding: 12px 50px; border-radius: 14px; text-transform: uppercase; pointer-events: none; user-select: none; z-index: 10; white-space: nowrap; text-align: center; }");
            sb.AppendLine("@media print {");
            sb.AppendLine("  body { padding: 0 !important; margin: 0 !important; background: none !important; }");
            sb.AppendLine("  .cert-page { border: none !important; box-shadow: none !important; padding: 34px 44px !important; margin: 0 !important; width: 100% !important; max-width: 100% !important; page-break-after: always; break-after: page; page-break-inside: avoid; break-inside: avoid; }");
            sb.AppendLine("  .cert-watermark { color: rgba(29, 78, 216, 0.14) !important; border-color: rgba(29, 78, 216, 0.16) !important; -webkit-print-color-adjust: exact !important; print-color-adjust: exact !important; }");
            sb.AppendLine("  .no-print { display: none !important; }");
            sb.AppendLine("}");

            // Header Layout
            sb.AppendLine(".cert-header-table { width: 100%; border-collapse: collapse; margin-bottom: 8px; }");
            sb.AppendLine(".cert-header-table td { vertical-align: top; }");
            
            sb.AppendLine(".cert-top-left { width: 28%; font-size: 14px; font-weight: bold; letter-spacing: 1px; padding-top: 2px; font-family: 'Segoe UI', Arial, sans-serif; }");
            sb.AppendLine(".sample-watermark { display: inline-block; transform: rotate(-25deg); color: #1d4ed8; font-size: 21px; font-weight: bold; border: 2px dashed #1d4ed8; padding: 2px 10px; border-radius: 4px; margin-top: 14px; opacity: 0.88; }");
            
            sb.AppendLine(".cert-top-center { width: 44%; text-align: center; }");
            sb.AppendLine(".cert-nu-logo { width: 50px; height: auto; display: block; margin: 0 auto 4px auto; }");
            sb.AppendLine(".cert-nu-name { font-size: 27px; font-weight: bold; line-height: 1.1; margin: 0; color: #000; letter-spacing: 0.5px; font-family: 'Times New Roman', Georgia, serif; }");
            sb.AppendLine(".cert-nu-country { font-size: 15px; margin: 2px 0 5px 0; color: #111; font-family: 'Times New Roman', Georgia, serif; }");
            sb.AppendLine(".cert-provisional-box { display: inline-block; border: 1.5px solid #000; padding: 3px 18px; font-size: 13px; font-weight: bold; letter-spacing: 2px; text-transform: uppercase; background: #fff; margin-top: 2px; font-family: Arial, sans-serif; }");

            sb.AppendLine(".cert-top-right { width: 28%; text-align: right; }");
            sb.AppendLine(".cert-barcode-txt { font-size: 9.5px; font-family: monospace; letter-spacing: 0.5px; margin-top: 2px; margin-bottom: 5px; text-align: right; font-weight: bold; }");
            sb.AppendLine(".cert-meta-wrap { display: inline-flex; align-items: center; gap: 8px; text-align: left; }");
            sb.AppendLine(".cert-qr-wrap { display: inline-block; width: 64px; height: 64px; }");
            sb.AppendLine(".cert-meta-table { font-size: 12px; border-collapse: collapse; line-height: 1.45; font-family: 'Segoe UI', Arial, sans-serif; }");
            sb.AppendLine(".cert-meta-table td.m-lbl { font-weight: 600; width: 62px; white-space: nowrap; }");
            sb.AppendLine(".cert-meta-table td.m-col { width: 10px; text-align: center; }");
            sb.AppendLine(".cert-meta-table td.m-val { font-weight: bold; padding: 0 4px; white-space: nowrap; }");
            sb.AppendLine(".cert-meta-table td.m-dots { border-bottom: 1px dotted #333; width: 70px; height: 14px; }");

            // Degree Heading
            sb.AppendFormat(".cert-degree-heading {{ text-align: center; font-size: {0}px; margin: 16px 0 14px 0; font-family: '{1}', 'Old English Text MT', 'Cloister Black', 'Cinzel Decorative', Georgia, serif; font-weight: normal; color: #000; line-height: 1.15; }}\r\n", fontDegreeSize, fontDegree);

            // Certification Body (Matching certificate sample with continuous dotted leaders)
            sb.AppendFormat(".cert-body {{ width: 94%; margin: 12px auto 0 auto; font-size: {0}px; line-height: 2.3; color: #111; }}\r\n", fontBodySize);
            sb.AppendLine(".cert-line { display: flex; align-items: baseline; width: 100%; margin-bottom: 2px; }");
            sb.AppendFormat(".cert-lbl {{ white-space: nowrap; font-size: {0}px; margin-right: 6px; font-family: '{1}', 'Times New Roman', Georgia, serif; }}\r\n", fontBodySize, fontBody);
            sb.AppendLine(".cert-fill { flex: 1; border-bottom: 1.5px dotted #111; text-align: center; min-height: 22px; line-height: 1.1; padding: 0 8px; }");
            sb.AppendFormat(".cert-fill-student {{ font-family: '{0}', 'Brush Script MT', 'Lucida Handwriting', 'Segoe Script', cursive, serif; font-size: {1}px; font-style: {2}; font-weight: {3}; color: #000; display: inline-block; }}\r\n", 
                fontStudent, fontStudentSize, isStudentItalic ? "italic" : "normal", isStudentBold ? "bold" : "normal");
            sb.AppendLine(".cert-fill-college { font-family: 'Times New Roman', Georgia, serif; font-weight: bold; font-size: 15px; color: #000; display: inline-block; }");
            sb.AppendFormat(".cert-fill-degree-inline {{ font-family: '{0}', 'Old English Text MT', 'Cloister Black', Georgia, serif; font-size: 21px; font-weight: normal; color: #000; display: inline-block; }}\r\n", fontDegree);
            sb.AppendLine(".cert-fill-fixed { border-bottom: 1.5px dotted #111; text-align: center; font-weight: bold; font-size: 16px; min-height: 22px; line-height: 1.1; padding: 0 8px; font-family: 'Times New Roman', Georgia, serif; }");

            // Footer
            sb.AppendLine(".cert-footer-table { width: 94%; margin: 26px auto 4px auto; border-collapse: collapse; }");
            sb.AppendLine(".cert-footer-table td { vertical-align: bottom; }");
            sb.AppendLine(".cert-foot-left { font-size: 12.5px; line-height: 1.45; color: #222; }");
            sb.AppendLine(".cert-nb { font-size: 9.5px; font-style: italic; color: #444; margin-top: 6px; }");
            sb.AppendLine(".cert-foot-right { text-align: right; }");
            sb.AppendLine(".cert-sig-block { text-align: center; display: inline-block; min-width: 180px; }");
            sb.AppendLine(".cert-sig-img { width: 140px; height: auto; display: block; margin: 0 auto 2px auto; }");
            sb.AppendLine(".cert-sig-title { font-size: 13px; font-weight: bold; font-family: 'Times New Roman', Georgia, serif; }");

            sb.AppendLine("</style></head><body>");

            for (int rIdx = 0; rIdx < dtExaminees.Rows.Count; rIdx++)
            {
                DataRow r = dtExaminees.Rows[rIdx];

                string stdName = r["std_name"] != null ? r["std_name"].ToString().Trim() : "";
                string fname = r["fname"] != null ? r["fname"].ToString().Trim() : "";
                string mname = r["mname"] != null ? r["mname"].ToString().Trim() : "";
                string colName = r["col_name"] != null ? r["col_name"].ToString().Trim() : "";
                string exmRoll = r["exm_roll"] != null ? r["exm_roll"].ToString().Trim() : "";
                string regNo = r["reg_no"] != null ? r["reg_no"].ToString().Trim() : "";
                string session = r["SESSION"] != null ? r["SESSION"].ToString().Trim() : "";
                string exmYear = r["exm_year"] != null ? r["exm_year"].ToString().Trim() : "";
                string exmCode = r["exm_code"] != null ? r["exm_code"].ToString().Trim() : "8773";
                string cgpa = r["CGPA"] != null ? r["CGPA"].ToString().Trim() : "";

                string sessPrefix = session.Length >= 4 ? session.Substring(2, 2) : "20";
                long currentSerial = startRefNum + rIdx;
                string certSlFormatted = string.Format("{0:D7}", currentSerial);

                string serialDisplay = !string.IsNullOrEmpty(r["CRT_SL"] != null ? r["CRT_SL"].ToString().Trim() : "")
                    ? r["CRT_SL"].ToString().Trim()
                    : string.Format("NUPC {0}- {1}", sessPrefix, certSlFormatted);

                if (!serialDisplay.StartsWith("NUPC", StringComparison.OrdinalIgnoreCase))
                {
                    serialDisplay = "NUPC " + serialDisplay;
                }

                string resDate = r["RES_DATE"] != null ? r["RES_DATE"].ToString().Trim() : (!string.IsNullOrEmpty(options.IssueDate) ? options.IssueDate : DateTime.Now.ToString("dd-MMM-yyyy"));
                string qrText = string.Format("NU-PROV-CERT|Reg:{0}|Roll:{1}|Name:{2}|Degree:{3}|Year:{4}|CGPA:{5}|RES_DATE:{6}",
                    regNo, exmRoll, stdName, options.CertificateDegreeTitle, exmYear, cgpa, resDate);
                string qrSvg = GenerateSvgQr(qrText, 64);
                string barcodeStr = string.Format("{0}N1E{1}W{2:D6}", sessPrefix, exmCode, currentSerial % 1000000);

                string degreeTitle = !string.IsNullOrEmpty(options.CertificateDegreeTitle)
                    ? options.CertificateDegreeTitle
                    : options.DegreeConferred;

                string issueDate = !string.IsNullOrEmpty(options.IssueDate)
                    ? options.IssueDate
                    : DateTime.Now.ToString("MMMM dd, yyyy");

                sb.AppendLine("<div class='cert-page'>");

                // Vector Guilloche Security Frame Background
                sb.AppendLine("  <svg class='cert-frame-svg' viewBox='0 0 1000 700' preserveAspectRatio='none'>");
                sb.AppendLine("    <defs>");
                sb.AppendLine("      <pattern id='guilloche-pat' width='20' height='10' patternUnits='userSpaceOnUse'>");
                sb.AppendLine("        <path d='M0,5 Q5,0 10,5 T20,5 M0,5 Q5,10 10,5 T20,5' fill='none' stroke='#1e3a5f' stroke-width='0.6' opacity='0.75'/>");
                sb.AppendLine("      </pattern>");
                sb.AppendLine("    </defs>");
                sb.AppendLine("    <rect x='10' y='10' width='980' height='680' fill='none' stroke='#1e3a5f' stroke-width='1.5'/>");
                sb.AppendLine("    <rect x='14' y='14' width='972' height='672' fill='none' stroke='#1e3a5f' stroke-width='0.5'/>");
                sb.AppendLine("    <rect x='16' y='16' width='968' height='10' fill='url(#guilloche-pat)'/>");
                sb.AppendLine("    <rect x='16' y='674' width='968' height='10' fill='url(#guilloche-pat)'/>");
                sb.AppendLine("    <rect x='16' y='26' width='10' height='648' fill='url(#guilloche-pat)'/>");
                sb.AppendLine("    <rect x='974' y='26' width='10' height='648' fill='url(#guilloche-pat)'/>");
                sb.AppendLine("    <rect x='28' y='28' width='944' height='644' fill='none' stroke='#1e3a5f' stroke-width='1.2'/>");
                sb.AppendLine("    <rect x='32' y='32' width='936' height='636' fill='none' stroke='#1e3a5f' stroke-width='0.5'/>");
                sb.AppendLine("    <line x1='10' y1='40' x2='40' y2='10' stroke='#1e3a5f' stroke-width='2'/>");
                sb.AppendLine("    <g stroke='#1e3a5f' stroke-width='1.2' fill='none'>");
                sb.AppendLine("      <path d='M32,46 L46,46 L46,32'/>");
                sb.AppendLine("      <circle cx='46' cy='46' r='2' fill='#1e3a5f'/>");
                sb.AppendLine("      <path d='M968,46 L954,46 L954,32'/>");
                sb.AppendLine("      <circle cx='954' cy='46' r='2' fill='#1e3a5f'/>");
                sb.AppendLine("      <path d='M32,654 L46,654 L46,668'/>");
                sb.AppendLine("      <circle cx='46' cy='654' r='2' fill='#1e3a5f'/>");
                sb.AppendLine("      <path d='M968,654 L954,654 L954,668'/>");
                sb.AppendLine("      <circle cx='954' cy='654' r='2' fill='#1e3a5f'/>");
                sb.AppendLine("    </g>");
                sb.AppendLine("  </svg>");

                sb.AppendLine("  <div class='cert-content-layer'>");
                if (options != null && options.IsDemo)
                {
                    string wm = string.IsNullOrEmpty(options.WatermarkText) ? "SAMPLE" : options.WatermarkText;
                    sb.AppendFormat("    <div class='cert-watermark'>{0}</div>\r\n", wm);
                }

                // Top Header Row
                sb.AppendLine("    <table class='cert-header-table'><tr>");
                
                // Top Left
                sb.AppendLine("      <td class='cert-top-left'>");
                sb.AppendFormat("        <div>{0}</div>\r\n", serialDisplay);
                if (options != null && options.IsDemo)
                {
                    sb.AppendLine("        <div class='sample-watermark'>Sample</div>");
                }
                sb.AppendLine("      </td>");

                // Top Center
                sb.AppendLine("      <td class='cert-top-center'>");
                if (!string.IsNullOrEmpty(logoBase64))
                {
                    sb.AppendFormat("        <img src='{0}' class='cert-nu-logo' alt='NU Crest' />\r\n", logoBase64);
                }
                sb.AppendLine("        <div class='cert-nu-name'>National University</div>");
                sb.AppendLine("        <div class='cert-nu-country'>Bangladesh</div>");
                sb.AppendLine("        <div class='cert-provisional-box'>PROVISIONAL CERTIFICATE</div>");
                sb.AppendLine("      </td>");

                // Top Right: Barcode string on top, then QR code & metadata table with dotted leaders
                sb.AppendLine("      <td class='cert-top-right'>");
                sb.AppendFormat("        <div class='cert-barcode-txt'>{0}</div>\r\n", barcodeStr);
                sb.AppendLine("        <div class='cert-meta-wrap'>");
                sb.AppendFormat("          <!-- QR_PAYLOAD: {0} -->\r\n", qrText);
                sb.AppendFormat("          <div class='cert-qr-wrap' data-qr='{0}' title='{0}'>{1}</div>\r\n", qrText.Replace("\"", "&quot;"), qrSvg);
                sb.AppendLine("          <table class='cert-meta-table'>");
                sb.AppendFormat("            <tr><td class='m-lbl'>Roll No.</td><td class='m-col'>:</td><td class='m-val'>{0}</td><td class='m-dots'></td></tr>\r\n", exmRoll);
                sb.AppendFormat("            <tr><td class='m-lbl'>Regn.No.</td><td class='m-col'>:</td><td class='m-val'>{0}</td><td class='m-dots'></td></tr>\r\n", regNo);
                sb.AppendFormat("            <tr><td class='m-lbl'>Session</td><td class='m-col'>:</td><td class='m-val'>{0}</td><td class='m-dots'></td></tr>\r\n", session);
                sb.AppendLine("          </table>");
                sb.AppendLine("        </div>");
                sb.AppendLine("      </td>");
                sb.AppendLine("    </tr></table>");

                // Prominent Conferred Title
                sb.AppendFormat("    <div class='cert-degree-heading'>{0}</div>\r\n", degreeTitle);

                // Body text with elegant script and continuous dotted leader fills across to right margin
                sb.AppendLine("    <div class='cert-body'>");
                
                sb.AppendLine("      <div class='cert-line'>");
                sb.AppendLine("        <span class='cert-lbl'>This is to certify that</span>");
                sb.AppendFormat("        <span class='cert-fill cert-fill-student'>{0}</span>\r\n", stdName);
                sb.AppendLine("      </div>");

                sb.AppendLine("      <div class='cert-line'>");
                sb.AppendLine("        <span class='cert-lbl'>Son/Daughter of</span>");
                sb.AppendFormat("        <span class='cert-fill cert-fill-student'>{0}</span>\r\n", fname);
                sb.AppendLine("      </div>");

                sb.AppendLine("      <div class='cert-line'>");
                sb.AppendLine("        <span class='cert-lbl'>and</span>");
                sb.AppendFormat("        <span class='cert-fill cert-fill-student'>{0}</span>\r\n", mname);
                sb.AppendLine("      </div>");

                sb.AppendLine("      <div class='cert-line'>");
                sb.AppendLine("        <span class='cert-lbl'>of</span>");
                sb.AppendFormat("        <span class='cert-fill cert-fill-college'>{0}</span>\r\n", colName);
                sb.AppendLine("      </div>");

                sb.AppendLine("      <div class='cert-line'>");
                sb.AppendLine("        <span class='cert-lbl'>obtained</span>");
                sb.AppendFormat("        <span class='cert-fill cert-fill-degree-inline'>{0}</span>\r\n", degreeTitle);
                sb.AppendLine("        <span class='cert-lbl' style='margin-left: 8px;'>Degree under National University</span>");
                sb.AppendLine("      </div>");

                sb.AppendLine("      <div class='cert-line'>");
                sb.AppendLine("        <span class='cert-lbl'>in the examination of</span>");
                sb.AppendFormat("        <span class='cert-fill-fixed' style='width: 105px;'>{0}</span>\r\n", exmYear);
                sb.AppendLine("        <span class='cert-lbl' style='margin-left: 14px;'>with CGPA</span>");
                sb.AppendFormat("        <span class='cert-fill-fixed' style='width: 95px;'>{0}</span>\r\n", cgpa);
                sb.AppendLine("        <span class='cert-lbl' style='margin-left: 14px;'>out of 4.00 .</span>");
                sb.AppendLine("      </div>");

                sb.AppendLine("    </div>"); // .cert-body

                // Footer with Date and Controller of Examinations signature
                sb.AppendLine("    <table class='cert-footer-table'><tr>");
                sb.AppendLine("      <td style='width: 58%;' class='cert-foot-left'>");
                sb.AppendLine("        <div style='font-weight: bold;'>National University, Bangladesh.</div>");
                sb.AppendFormat("        <div>Date of Issue: {0}</div>\r\n", issueDate);
                sb.AppendLine("        <div class='cert-nb'>N.B. This provisional certificate must be surrendered at the time of taking delivery of original certificate.</div>");
                sb.AppendLine("      </td>");

                sb.AppendLine("      <td style='width: 42%;' class='cert-foot-right'>");
                sb.AppendLine("        <div class='cert-sig-block'>");
                if (!string.IsNullOrEmpty(sigBase64))
                {
                    sb.AppendFormat("          <img src='{0}' class='cert-sig-img' alt='Controller of Examinations Signature' />\r\n", sigBase64);
                }
                else
                {
                    sb.AppendLine("          <div style='height: 44px;'></div>");
                }
                sb.AppendLine("          <div class='cert-sig-title'>Controller of Examinations</div>");
                sb.AppendLine("        </div>");
                sb.AppendLine("      </td>");
                sb.AppendLine("    </tr></table>");

                sb.AppendLine("  </div>"); // .cert-content-layer
                sb.AppendLine("</div>"); // .cert-page
            }

            sb.AppendLine("</body></html>");

            File.WriteAllText(savedFilePath, sb.ToString(), Encoding.UTF8);

            // Log operation in print audit
            string lotRegCert = (dtExaminees.Rows.Count == 1) ? dtExaminees.Rows[0]["reg_no"].ToString().Trim() : "";
            string examYrCert = dtExaminees.Rows.Count > 0 && dtExaminees.Rows[0]["exm_year"] != null ? dtExaminees.Rows[0]["exm_year"].ToString().Trim() : "";
            PrintAuditManager.LogOperation("CERTIFICATE", examYrCert, options.CertificateDegreeTitle, lotRegCert, dtExaminees, savedFilePath);

            return savedFilePath;
        }
    }
}
