using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace NU_Solver
{
    /// <summary>
    /// Helper engine for integrating SAP Crystal Reports (.rpt) templates with
    /// National University academic transcript and provisional certificate data.
    /// Provides dynamic runtime detection, schema (.xsd) export for report design,
    /// and data source binding without hard dependencies.
    /// </summary>
    public static class CrystalReportHelper
    {
        private static bool? _runtimeAvailable = null;
        private static string _runtimeVersion = "";
        private static Type _reportDocType = null;

        /// <summary>
        /// Checks whether the Crystal Reports .NET runtime engine is installed on this machine.
        /// </summary>
        public static bool IsRuntimeInstalled(out string versionInfo)
        {
            if (_runtimeAvailable.HasValue)
            {
                versionInfo = _runtimeVersion;
                return _runtimeAvailable.Value;
            }

            string[] knownAssemblyNames = new string[]
            {
                "CrystalDecisions.CrystalReports.Engine, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304",
                "CrystalDecisions.CrystalReports.Engine, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304",
                "CrystalDecisions.CrystalReports.Engine, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304",
                "CrystalDecisions.CrystalReports.Engine, Version=14.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304",
                "CrystalDecisions.CrystalReports.Engine, Version=11.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304",
                "CrystalDecisions.CrystalReports.Engine, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
            };

            foreach (string asmName in knownAssemblyNames)
            {
                try
                {
                    Assembly asm = Assembly.Load(asmName);
                    if (asm != null)
                    {
                        Type t = asm.GetType("CrystalDecisions.CrystalReports.Engine.ReportDocument");
                        if (t != null)
                        {
                            _reportDocType = t;
                            _runtimeVersion = asm.GetName().Version.ToString();
                            _runtimeAvailable = true;
                            versionInfo = "SAP Crystal Reports Runtime v" + _runtimeVersion;
                            return true;
                        }
                    }
                }
                catch
                {
                    // Continue scanning other versions
                }
            }

            _runtimeAvailable = false;
            _runtimeVersion = "Not Detected";
            versionInfo = "Not Detected (SAP Crystal Reports .NET runtime not found)";
            return false;
        }

        /// <summary>
        /// Exports complete database schema (.xsd) and sample dataset (.xml)
        /// so users can easily design their .rpt templates in SAP Crystal Reports Designer.
        /// </summary>
        public static string ExportSchemaForDesign(DataTable dt, string targetFolder, string courseKey)
        {
            try
            {
                if (string.IsNullOrEmpty(targetFolder))
                {
                    try
                    {
                        string baseDir = !string.IsNullOrEmpty(Application.StartupPath) 
                            ? Application.StartupPath 
                            : AppDomain.CurrentDomain.BaseDirectory;
                        targetFolder = Path.Combine(baseDir, "CrystalReports");
                        if (!Directory.Exists(targetFolder)) Directory.CreateDirectory(targetFolder);
                    }
                    catch
                    {
                        targetFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "NU_Solver_Reports", "CrystalReports");
                        if (!Directory.Exists(targetFolder)) Directory.CreateDirectory(targetFolder);
                    }
                }

                // If no real table was provided, generate a baseline schema table
                DataTable exportTable = dt != null ? dt.Copy() : CreateBaselineTranscriptTable();
                exportTable.TableName = "ExamineeTranscript";

                DataSet ds = new DataSet("NationalUniversityTranscriptDataSet");
                ds.Tables.Add(exportTable);

                string safeName = string.IsNullOrEmpty(courseKey) ? "NU_Transcript" : courseKey.Replace(" ", "_");
                string xsdPath = Path.Combine(targetFolder, safeName + "_Schema.xsd");
                string xmlPath = Path.Combine(targetFolder, safeName + "_SampleData.xml");

                ds.WriteXmlSchema(xsdPath);
                ds.WriteXml(xmlPath, XmlWriteMode.WriteSchema);

                return xsdPath;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to export Crystal Reports schema: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Tests loading a .rpt template and feeding it examinee data from the database.
        /// </summary>
        public static bool TestLoadRptData(string rptPath, DataTable dt, out string message)
        {
            if (string.IsNullOrEmpty(rptPath))
            {
                message = "No .rpt file specified. Please select a valid Crystal Report (.rpt) file path.";
                return false;
            }

            if (!File.Exists(rptPath))
            {
                message = "The specified .rpt file does not exist at path:\n" + rptPath;
                return false;
            }

            string ver;
            bool runtimeFound = IsRuntimeInstalled(out ver);

            if (!runtimeFound || _reportDocType == null)
            {
                FileInfo fi = new FileInfo(rptPath);
                message = "File Verified: " + fi.Name + " (" + (fi.Length / 1024) + " KB).\n" +
                          "Notice: SAP Crystal Reports .NET runtime engine is not installed on this system.\n\n" +
                          "To render this .rpt directly in application, install 'SAP Crystal Reports runtime engine for .NET Framework (x86/x64)'.\n" +
                          "Alternatively, the application will use its high-fidelity built-in engine with live QR and single-page fit.";
                return true;
            }

            try
            {
                // Instantiate ReportDocument dynamically
                object doc = Activator.CreateInstance(_reportDocType);
                if (doc == null)
                {
                    message = "Could not instantiate Crystal Reports ReportDocument.";
                    return false;
                }

                // Call rpt.Load(rptPath)
                MethodInfo loadMethod = _reportDocType.GetMethod("Load", new Type[] { typeof(string) });
                if (loadMethod != null)
                {
                    loadMethod.Invoke(doc, new object[] { rptPath });
                }

                // If dataTable provided, call rpt.SetDataSource(dt)
                if (dt != null && dt.Rows.Count > 0)
                {
                    MethodInfo setDataSourceMethod = _reportDocType.GetMethod("SetDataSource", new Type[] { typeof(DataTable) });
                    if (setDataSourceMethod != null)
                    {
                        setDataSourceMethod.Invoke(doc, new object[] { dt });
                    }
                }

                // Close/Dispose
                MethodInfo closeMethod = _reportDocType.GetMethod("Close");
                if (closeMethod != null) closeMethod.Invoke(doc, null);

                MethodInfo disposeMethod = _reportDocType.GetMethod("Dispose");
                if (disposeMethod != null) disposeMethod.Invoke(doc, null);

                message = "SUCCESS: Crystal Report (.rpt) template loaded and data source verified successfully!\n" +
                          "Template: " + Path.GetFileName(rptPath) + "\n" +
                          "Runtime: " + ver + "\n" +
                          "Records Mapped: " + (dt != null ? dt.Rows.Count : 0) + " examinee(s).";
                return true;
            }
            catch (Exception ex)
            {
                string innerMsg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                message = "Crystal Reports Load Test Result:\n" +
                          "File: " + Path.GetFileName(rptPath) + " exists.\n" +
                          "Diagnostic details: " + innerMsg + "\n\n" +
                          "Tip: In Crystal Reports Designer, ensure table fields match the database column names (std_name, reg_no, CGPA, noncredit_1..5, etc.).";
                return false;
            }
        }

        /// <summary>
        /// Attempts to open the .rpt file in the default system designer (SAP Crystal Reports / Visual Studio).
        /// </summary>
        public static void OpenInDesigner(string rptPath)
        {
            if (string.IsNullOrEmpty(rptPath) || !File.Exists(rptPath))
            {
                MessageBox.Show("Please select an existing .rpt file first.", "Open .rpt Template", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = rptPath,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not launch external report designer:\n" + ex.Message + "\n\nYou can manually open this file in SAP Crystal Reports Designer.", "Crystal Reports", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Creates a baseline DataTable schema containing all candidate, marks, and non-credit fields.
        /// </summary>
        public static DataTable CreateBaselineTranscriptTable()
        {
            DataTable dt = new DataTable("Examinees");
            dt.Columns.Add("std_name", typeof(string));
            dt.Columns.Add("fname", typeof(string));
            dt.Columns.Add("mname", typeof(string));
            dt.Columns.Add("col_name", typeof(string));
            dt.Columns.Add("exm_roll", typeof(string));
            dt.Columns.Add("reg_no", typeof(string));
            dt.Columns.Add("SESSION", typeof(string));
            dt.Columns.Add("exm_year", typeof(string));
            dt.Columns.Add("exm_code", typeof(string));
            dt.Columns.Add("SESS1", typeof(string));
            dt.Columns.Add("C_CREDIT", typeof(string));
            dt.Columns.Add("T_CREDIT", typeof(string));
            dt.Columns.Add("CGPA", typeof(string));
            dt.Columns.Add("RES_DATE", typeof(string));
            dt.Columns.Add("CRT_SL", typeof(string));

            // Semester papers P11..P20 for S1..S8
            for (int s = 1; s <= 8; s++)
            {
                for (int p = 1; p <= 10; p++)
                {
                    dt.Columns.Add("P" + (10 + p) + "_S" + s, typeof(string));
                }
                dt.Columns.Add("S" + s + "_GPA", typeof(string));
            }

            // Non-Credit Course Fields (noncredit_1 .. noncredit_5)
            for (int nc = 1; nc <= 5; nc++)
            {
                dt.Columns.Add("noncredit_" + nc, typeof(string));
            }

            // Add sample row
            DataRow r = dt.NewRow();
            r["std_name"] = "MD. RAHIM HOSSAIN";
            r["fname"] = "MD. KARIM HOSSAIN";
            r["mname"] = "RAHIMA BEGUM";
            r["col_name"] = "GOVT. TEACHERS' TRAINING COLLEGE, DHAKA";
            r["exm_roll"] = "2380371";
            r["reg_no"] = "18503004559";
            r["SESSION"] = "2022-23";
            r["exm_year"] = "2024";
            r["exm_code"] = "8773";
            r["SESS1"] = "22";
            r["C_CREDIT"] = "44";
            r["T_CREDIT"] = "44";
            r["CGPA"] = "3.45";
            r["RES_DATE"] = DateTime.Now.ToString("MMMM dd, yyyy");
            r["CRT_SL"] = "NU/Con/Certi/Trans/8773/2024/00037420";

            r["P11_S1"] = "810301=A-";
            r["P12_S1"] = "810303=B+";
            r["P13_S1"] = "810305=A";
            r["S1_GPA"] = "3.50";

            r["P11_S2"] = "810302=A";
            r["P12_S2"] = "810304=A-";
            r["P13_S2"] = "810306=B";
            r["S2_GPA"] = "3.40";

            r["noncredit_1"] = "510901=Satisfactory";
            r["noncredit_2"] = "520392=Satisfactory";
            r["noncredit_3"] = "";
            r["noncredit_4"] = "";
            r["noncredit_5"] = "";

            dt.Rows.Add(r);
            return dt;
        }
    }
}
