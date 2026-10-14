using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace NU_Solver
{
    class Database
    {
        public static string sub_code { get; set; }
        // public static string DefaultConnectionString = @"Data Source=192.168.0.230;Initial Catalog=solving;Integrated Security=true;";
           public static string DefaultConnectionString = @"Data Source=192.168.0.230;User id=sa;Password=1234;Initial Catalog=Solving_5;";

        private static string configFilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "connection.config");
        private static string inMemoryConnectionString = null;

        public static string GetConnectionString()
        {
            if (!string.IsNullOrEmpty(inMemoryConnectionString))
            {
                return inMemoryConnectionString;
            }

            try
            {
                if (System.IO.File.Exists(configFilePath))
                {
                    string stored = System.IO.File.ReadAllText(configFilePath).Trim();
                    if (!string.IsNullOrEmpty(stored))
                    {
                        return stored;
                    }
                }
            }
            catch { }
            return DefaultConnectionString;
        }

        public static bool SaveConnectionString(string newConnStr)
        {
            try
            {
                if (string.IsNullOrEmpty(newConnStr)) return false;
                inMemoryConnectionString = newConnStr.Trim();
                try
                {
                    System.IO.File.WriteAllText(configFilePath, newConnStr.Trim());
                }
                catch { }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving connection string:\n" + ex.Message, "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public static bool TestConnection(string connStr, out string errorMsg)
        {
            errorMsg = "";
            try
            {
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    con.Open();
                    return true;
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return false;
            }
        }

        public static SqlConnection GetConnectionObj()
        {
            try
            {
                SqlConnection cn = new SqlConnection();
                cn.ConnectionString = GetConnectionString();
                cn.Open();
                return cn;
            }
            catch (Exception e)
            {
                MessageBox.Show("Database connection failed.\n\nTarget Server: " + GetConnectionString() + "\n\nError Details:\n" + e.Message + "\n\nTroubleshooting for Remote PCs:\n1. Integrated Security=true may fail on another PC. Use SQL Auth (User Id=sa;Password=...) instead.\n2. Ensure SQL Server allows remote connections and TCP/IP port 1433 is enabled.\n3. Verify Windows Firewall on host PC permits incoming connections on port 1433.\n4. Click ⚙ Settings on Login screen to edit connection string.", "Database Connection Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return null;
        }
        public static List<string> all_subjects = new List<string>();
        public static void init()
        {
            try
            {
                SqlConnection con = Database.GetConnectionObj();
               // SqlCommand cmd = new SqlCommand("SELECT SUBSTRING(PAP_CODE, 3, 4)PAP_CODE FROM dbo.p1syllabas1314 WHERE type='T'", con);
                //SqlCommand cmd = new SqlCommand("SELECT PAP_CODE FROM dbo.p1syllabas1314 WHERE type='T' AND SEMESTER='1'", con);
                SqlCommand cmd = new SqlCommand("SELECT PAP_CODE FROM dbo.p1syllabas1314 WHERE type='T'", con);
 
                // SqlCommand cmd = new SqlCommand("SELECT PAP_CODE FROM dbo.p1syllabas1314 WHERE type='T' AND SEMESTER='4'", con);
                //cmd.CommandText = "SELECT DISTINCT dbo.H_file_list.pap_code FROM dbo.H_file_list";
                SqlDataReader reader = cmd.ExecuteReader();
                all_subjects.Clear();
                while (reader.Read())
                {
                    //MessageBox.Show(reader["pap_code"].ToString());
                    //all_subjects.Add( reader["pap_code"].ToString().Substring(1,3 ).ToString() );
                    all_subjects.Add(reader["pap_code"].ToString());


                }
                reader.Dispose();
                con.Close();
            }
            catch (Exception ee)
            {
                MessageBox.Show("Database error occured when fatching Papercodes.\n" + ee.StackTrace.ToString());
                Application.Exit();
            }
            //all_subjects.Add("fdsf");
        }
        public static bool isNotValid(string papercode)
        {
            
            for (int i = 0; i < all_subjects.Count; i++)
            {
                if (papercode == all_subjects[i])
                    return false;
            }
            return true;
        }

        //public static string checkExamCode(string s) { return ""; }
       // public static string checkSubjectCode(string s) { return s != sub_code ? "" : ""; }
        public static string checkSubjectCode(string subjectCode)
        {
            if (!Regex.IsMatch(subjectCode, @"^\d{6}$"))
                return "Sub. Invalid";

            return "";
        }
        public static string checkExaminer(string Examiner)
        {
            if (!Regex.IsMatch(Examiner, @"^\d+$"))
                return "EB. IC."; // Invalid char
            else if (Examiner.Length != 5 && Examiner.Length != 6)
                return "EB. Len.";
            return "";
        }
        public static string checkMarks(string Marks)
        {
            if (!Regex.IsMatch(Marks, @"^\d+$"))
                return "Marks. IC."; // Invalid char //return "Regi Contains Invalid Caracter.";            
            else if (Marks.Length != 3)
                return "Marks. Len.";
            return "";
        }
        public static string checkhMarks(string hMarks)
        {
            //if (!Regex.IsMatch(hMarks, @"^\d+$"))
            //    return "HMarks. IC."; // Invalid char //return "Regi Contains Invalid Caracter.";            
            //else if (hMarks.Length != 2)
            //    return "HMarks. Len.";
            return "";
        }
        public static string checkSerial(string Serial)
        {
            if (!Regex.IsMatch(Serial, @"^\d+$"))
                return "SL. IC. "; // Invalid char //return "Regi Contains Invalid Caracter.";            
            else if (Serial.Length != 3)
                return "SL. Len.";
            return "";
        }
        public static string checkLitho(string qr, string binary, string litho1, string litho2)
        {
            // uncoment if qr used

            //if (qr != "")
            //{
            //    if (scrpt_no.Length != 28)
            //        return "Litho QR length should 28.";
            //}
            //else 
            if (binary != "")
            {
                if (!Regex.IsMatch(binary, @"^[\01]+$"))
                    return "Litho Script IC";
                if (binary.Length != 32)
                    return "Litho Script Len";
            }
            else if (!Regex.IsMatch(litho1, @"^[10]+$"))
                return "Litho 1 IC.";
            else if (!Regex.IsMatch(litho2, @"^[10]+$"))
                return "Litho 2 IC.";
            else if (litho1.Length != 32)
                return "Litho 1 32";
            else if (litho2.Length != 32)
                return "Litho 2 32";
            else if (litho1 != litho2)
                return "Litho 1,2 NE";
            else //litho1 == litho2
            {
                //MessageBox.Show(litho1.Substring(0, 3) + "-" + litho1.Substring(28, 29));
                if (litho1.Substring(0, 4) == "1111" && litho1.Substring(28, 4) == "0010") //R8 F2 H type
                    return "";
                else if (litho1.Substring(0, 4) == "1111" && litho1.Substring(28, 4) == "1000") //R8 FC H type
                    return "";
                else if (litho1.Substring(0, 4) == "1111" && litho1.Substring(28, 4) == "0011") //R8 test H type
                    return "";
                else if (litho1.Substring(0, 4) == "1100" && litho1.Substring(28, 4) == "1110") // test H type
                    return "";

                else if (litho1.Substring(0, 4) == "1100" && litho1.Substring(28, 4) == "0001") // test H type
                    return "";

                else if (litho1.Substring(0, 4) == "0010" && litho1.Substring(28, 4) == "1111") // test H type
                    return "";

                else if (litho1.Substring(0, 4) == "1010" && litho1.Substring(28, 4) == "0100") // test H type
                    return "";

                else if (litho1.Substring(0, 4) == "1010" && litho1.Substring(28, 4) == "0101") // test H type
                    return "";

                else if (litho1.Substring(0, 4) == "1010" && litho1.Substring(28, 4) == "0110") // test H type
                    return "";

                else if (litho1.Substring(0, 4) == "1011" && litho1.Substring(28, 4) == "0100") // test H type
                    return "";

                else if (litho1.Substring(0, 4) == "0011" && litho1.Substring(28, 4) == "0100") // test H type
                    return "";

                else if (litho1.Substring(0, 4) == "1011" && litho1.Substring(28, 4) == "0101") // test H type
                    return "";

                else if (litho1.Substring(0, 4) == "1011" && litho1.Substring(28, 4) == "0110") // test H type
                    return "";

                else if (litho1.Substring(0, 4) == "1100" && litho1.Substring(28, 4) == "0100") // test H type
                    return "";

                else if (litho1.Substring(0, 4) == "1011" && litho1.Substring(28, 4) == "1011") // test H type
                    return "";

                else if (litho1.Substring(0, 4) == "1100" && litho1.Substring(28, 4) == "0100") // test H type
                    return "";

                else if (litho1.Substring(0, 4) == "1100" && litho1.Substring(28, 4) == "0101") // test H type
                    return "";

                else if (litho1.Substring(0, 4) == "1100" && litho1.Substring(28, 4) == "0110") // test H type
                    return "";

                else if (litho1.Substring(0, 4) == "1100" && litho1.Substring(28, 4) == "0101") // test H type
                    return "";

                else if (litho1.Substring(0, 4) == "1101" && litho1.Substring(28, 4) == "0100") // test H type
                    return "";

                else if (litho1.Substring(0, 4) == "1101" && litho1.Substring(28, 4) == "0101") // test H type
                    return "";

                else if (litho1.Substring(0, 4) == "1101" && litho1.Substring(28, 4) == "0110") // test H type
                    return "";

                else if (litho1.Substring(0, 4) == "1101" && litho1.Substring(28, 4) == "1011") // test H type
                    return "";

                else if (litho1.Substring(0, 4) == "1101" && litho1.Substring(28, 4) == "1101") // test H type
                    return "";

                else if (litho1.Substring(0, 4) == "1101" && litho1.Substring(28, 4) == "1110") // test H type
                    return "";
                else if (litho1.Substring(0, 4) == "1110" && litho1.Substring(28, 4) == "0110") // test H type
                    return "";
                else if (litho1.Substring(0, 4) == "1110" && litho1.Substring(28, 4) == "0100") // test H type
                    return "";
                else if (litho1.Substring(0, 4) == "1110" && litho1.Substring(28, 4) == "0101") // test H type
                    return "";
                else if (litho1.Substring(0, 4) == "1110" && litho1.Substring(28, 4) == "1011") // test H type
                    return "";
                else return "Litho Shift.";
            }
            return "";
        }

        public static string ConvertBinaryToHex(string binary)
        {
            if (string.IsNullOrEmpty(binary)) return null;
            binary = binary.Replace(' ', '0').Trim();
            if (binary.Length < 32)
                binary = binary.PadRight(32, '0');
            else if (binary.Length > 32)
                binary = binary.Substring(0, 32);

            StringBuilder sb = new StringBuilder(8);
            for (int i = 0; i < 32; i += 4)
            {
                int val = 0;
                for (int b = 0; b < 4; b++)
                {
                    val = (val << 1) | (binary[i + b] == '1' ? 1 : 0);
                }
                sb.Append(val.ToString("X1"));
            }
            return sb.ToString();
        }

        public static int RunBinToHexConversion()
        {
            using (SqlConnection con = GetConnectionObj())
            {
                if (con == null) throw new Exception("Unable to connect to database.");

                string sql = @"
UPDATE dbo.H_solving
SET 
    hexcodea = 
        SUBSTRING('0123456789ABCDEF', (CASE WHEN SUBSTRING(ISNULL(bdexcode, bhexcode), 1, 1)='1' THEN 8 ELSE 0 END + CASE WHEN SUBSTRING(ISNULL(bdexcode, bhexcode), 2, 1)='1' THEN 4 ELSE 0 END + CASE WHEN SUBSTRING(ISNULL(bdexcode, bhexcode), 3, 1)='1' THEN 2 ELSE 0 END + CASE WHEN SUBSTRING(ISNULL(bdexcode, bhexcode), 4, 1)='1' THEN 1 ELSE 0 END + 1), 1) +
        SUBSTRING('0123456789ABCDEF', (CASE WHEN SUBSTRING(ISNULL(bdexcode, bhexcode), 5, 1)='1' THEN 8 ELSE 0 END + CASE WHEN SUBSTRING(ISNULL(bdexcode, bhexcode), 6, 1)='1' THEN 4 ELSE 0 END + CASE WHEN SUBSTRING(ISNULL(bdexcode, bhexcode), 7, 1)='1' THEN 2 ELSE 0 END + CASE WHEN SUBSTRING(ISNULL(bdexcode, bhexcode), 8, 1)='1' THEN 1 ELSE 0 END + 1), 1) +
        SUBSTRING('0123456789ABCDEF', (CASE WHEN SUBSTRING(ISNULL(bdexcode, bhexcode), 9, 1)='1' THEN 8 ELSE 0 END + CASE WHEN SUBSTRING(ISNULL(bdexcode, bhexcode), 10, 1)='1' THEN 4 ELSE 0 END + CASE WHEN SUBSTRING(ISNULL(bdexcode, bhexcode), 11, 1)='1' THEN 2 ELSE 0 END + CASE WHEN SUBSTRING(ISNULL(bdexcode, bhexcode), 12, 1)='1' THEN 1 ELSE 0 END + 1), 1) +
        SUBSTRING('0123456789ABCDEF', (CASE WHEN SUBSTRING(ISNULL(bdexcode, bhexcode), 13, 1)='1' THEN 8 ELSE 0 END + CASE WHEN SUBSTRING(ISNULL(bdexcode, bhexcode), 14, 1)='1' THEN 4 ELSE 0 END + CASE WHEN SUBSTRING(ISNULL(bdexcode, bhexcode), 15, 1)='1' THEN 2 ELSE 0 END + CASE WHEN SUBSTRING(ISNULL(bdexcode, bhexcode), 16, 1)='1' THEN 1 ELSE 0 END + 1), 1) +
        SUBSTRING('0123456789ABCDEF', (CASE WHEN SUBSTRING(ISNULL(bdexcode, bhexcode), 17, 1)='1' THEN 8 ELSE 0 END + CASE WHEN SUBSTRING(ISNULL(bdexcode, bhexcode), 18, 1)='1' THEN 4 ELSE 0 END + CASE WHEN SUBSTRING(ISNULL(bdexcode, bhexcode), 19, 1)='1' THEN 2 ELSE 0 END + CASE WHEN SUBSTRING(ISNULL(bdexcode, bhexcode), 20, 1)='1' THEN 1 ELSE 0 END + 1), 1) +
        SUBSTRING('0123456789ABCDEF', (CASE WHEN SUBSTRING(ISNULL(bdexcode, bhexcode), 21, 1)='1' THEN 8 ELSE 0 END + CASE WHEN SUBSTRING(ISNULL(bdexcode, bhexcode), 22, 1)='1' THEN 4 ELSE 0 END + CASE WHEN SUBSTRING(ISNULL(bdexcode, bhexcode), 23, 1)='1' THEN 2 ELSE 0 END + CASE WHEN SUBSTRING(ISNULL(bdexcode, bhexcode), 24, 1)='1' THEN 1 ELSE 0 END + 1), 1) +
        SUBSTRING('0123456789ABCDEF', (CASE WHEN SUBSTRING(ISNULL(bdexcode, bhexcode), 25, 1)='1' THEN 8 ELSE 0 END + CASE WHEN SUBSTRING(ISNULL(bdexcode, bhexcode), 26, 1)='1' THEN 4 ELSE 0 END + CASE WHEN SUBSTRING(ISNULL(bdexcode, bhexcode), 27, 1)='1' THEN 2 ELSE 0 END + CASE WHEN SUBSTRING(ISNULL(bdexcode, bhexcode), 28, 1)='1' THEN 1 ELSE 0 END + 1), 1) +
        SUBSTRING('0123456789ABCDEF', (CASE WHEN SUBSTRING(ISNULL(bdexcode, bhexcode), 29, 1)='1' THEN 8 ELSE 0 END + CASE WHEN SUBSTRING(ISNULL(bdexcode, bhexcode), 30, 1)='1' THEN 4 ELSE 0 END + CASE WHEN SUBSTRING(ISNULL(bdexcode, bhexcode), 31, 1)='1' THEN 2 ELSE 0 END + CASE WHEN SUBSTRING(ISNULL(bdexcode, bhexcode), 32, 1)='1' THEN 1 ELSE 0 END + 1), 1),
    hexcodeb = 
        SUBSTRING('0123456789ABCDEF', (CASE WHEN SUBSTRING(ISNULL(bhexcode, bdexcode), 1, 1)='1' THEN 8 ELSE 0 END + CASE WHEN SUBSTRING(ISNULL(bhexcode, bdexcode), 2, 1)='1' THEN 4 ELSE 0 END + CASE WHEN SUBSTRING(ISNULL(bhexcode, bdexcode), 3, 1)='1' THEN 2 ELSE 0 END + CASE WHEN SUBSTRING(ISNULL(bhexcode, bdexcode), 4, 1)='1' THEN 1 ELSE 0 END + 1), 1) +
        SUBSTRING('0123456789ABCDEF', (CASE WHEN SUBSTRING(ISNULL(bhexcode, bdexcode), 5, 1)='1' THEN 8 ELSE 0 END + CASE WHEN SUBSTRING(ISNULL(bhexcode, bdexcode), 6, 1)='1' THEN 4 ELSE 0 END + CASE WHEN SUBSTRING(ISNULL(bhexcode, bdexcode), 7, 1)='1' THEN 2 ELSE 0 END + CASE WHEN SUBSTRING(ISNULL(bhexcode, bdexcode), 8, 1)='1' THEN 1 ELSE 0 END + 1), 1) +
        SUBSTRING('0123456789ABCDEF', (CASE WHEN SUBSTRING(ISNULL(bhexcode, bdexcode), 9, 1)='1' THEN 8 ELSE 0 END + CASE WHEN SUBSTRING(ISNULL(bhexcode, bdexcode), 10, 1)='1' THEN 4 ELSE 0 END + CASE WHEN SUBSTRING(ISNULL(bhexcode, bdexcode), 11, 1)='1' THEN 2 ELSE 0 END + CASE WHEN SUBSTRING(ISNULL(bhexcode, bdexcode), 12, 1)='1' THEN 1 ELSE 0 END + 1), 1) +
        SUBSTRING('0123456789ABCDEF', (CASE WHEN SUBSTRING(ISNULL(bhexcode, bdexcode), 13, 1)='1' THEN 8 ELSE 0 END + CASE WHEN SUBSTRING(ISNULL(bhexcode, bdexcode), 14, 1)='1' THEN 4 ELSE 0 END + CASE WHEN SUBSTRING(ISNULL(bhexcode, bdexcode), 15, 1)='1' THEN 2 ELSE 0 END + CASE WHEN SUBSTRING(ISNULL(bhexcode, bdexcode), 16, 1)='1' THEN 1 ELSE 0 END + 1), 1) +
        SUBSTRING('0123456789ABCDEF', (CASE WHEN SUBSTRING(ISNULL(bhexcode, bdexcode), 17, 1)='1' THEN 8 ELSE 0 END + CASE WHEN SUBSTRING(ISNULL(bhexcode, bdexcode), 18, 1)='1' THEN 4 ELSE 0 END + CASE WHEN SUBSTRING(ISNULL(bhexcode, bdexcode), 19, 1)='1' THEN 2 ELSE 0 END + CASE WHEN SUBSTRING(ISNULL(bhexcode, bdexcode), 20, 1)='1' THEN 1 ELSE 0 END + 1), 1) +
        SUBSTRING('0123456789ABCDEF', (CASE WHEN SUBSTRING(ISNULL(bhexcode, bdexcode), 21, 1)='1' THEN 8 ELSE 0 END + CASE WHEN SUBSTRING(ISNULL(bhexcode, bdexcode), 22, 1)='1' THEN 4 ELSE 0 END + CASE WHEN SUBSTRING(ISNULL(bhexcode, bdexcode), 23, 1)='1' THEN 2 ELSE 0 END + CASE WHEN SUBSTRING(ISNULL(bhexcode, bdexcode), 24, 1)='1' THEN 1 ELSE 0 END + 1), 1) +
        SUBSTRING('0123456789ABCDEF', (CASE WHEN SUBSTRING(ISNULL(bhexcode, bdexcode), 25, 1)='1' THEN 8 ELSE 0 END + CASE WHEN SUBSTRING(ISNULL(bhexcode, bdexcode), 26, 1)='1' THEN 4 ELSE 0 END + CASE WHEN SUBSTRING(ISNULL(bhexcode, bdexcode), 27, 1)='1' THEN 2 ELSE 0 END + CASE WHEN SUBSTRING(ISNULL(bhexcode, bdexcode), 28, 1)='1' THEN 1 ELSE 0 END + 1), 1) +
        SUBSTRING('0123456789ABCDEF', (CASE WHEN SUBSTRING(ISNULL(bhexcode, bdexcode), 29, 1)='1' THEN 8 ELSE 0 END + CASE WHEN SUBSTRING(ISNULL(bhexcode, bdexcode), 30, 1)='1' THEN 4 ELSE 0 END + CASE WHEN SUBSTRING(ISNULL(bhexcode, bdexcode), 31, 1)='1' THEN 2 ELSE 0 END + CASE WHEN SUBSTRING(ISNULL(bhexcode, bdexcode), 32, 1)='1' THEN 1 ELSE 0 END + 1), 1),
    hex2 = 
        SUBSTRING('0123456789ABCDEF', (CASE WHEN SUBSTRING(ISNULL(bhexcode, bdexcode), 1, 1)='1' THEN 8 ELSE 0 END + CASE WHEN SUBSTRING(ISNULL(bhexcode, bdexcode), 2, 1)='1' THEN 4 ELSE 0 END + CASE WHEN SUBSTRING(ISNULL(bhexcode, bdexcode), 3, 1)='1' THEN 2 ELSE 0 END + CASE WHEN SUBSTRING(ISNULL(bhexcode, bdexcode), 4, 1)='1' THEN 1 ELSE 0 END + 1), 1) +
        SUBSTRING('0123456789ABCDEF', (CASE WHEN SUBSTRING(ISNULL(bhexcode, bdexcode), 5, 1)='1' THEN 8 ELSE 0 END + CASE WHEN SUBSTRING(ISNULL(bhexcode, bdexcode), 6, 1)='1' THEN 4 ELSE 0 END + CASE WHEN SUBSTRING(ISNULL(bhexcode, bdexcode), 7, 1)='1' THEN 2 ELSE 0 END + CASE WHEN SUBSTRING(ISNULL(bhexcode, bdexcode), 8, 1)='1' THEN 1 ELSE 0 END + 1), 1) +
        SUBSTRING('0123456789ABCDEF', (CASE WHEN SUBSTRING(ISNULL(bhexcode, bdexcode), 9, 1)='1' THEN 8 ELSE 0 END + CASE WHEN SUBSTRING(ISNULL(bhexcode, bdexcode), 10, 1)='1' THEN 4 ELSE 0 END + CASE WHEN SUBSTRING(ISNULL(bhexcode, bdexcode), 11, 1)='1' THEN 2 ELSE 0 END + CASE WHEN SUBSTRING(ISNULL(bhexcode, bdexcode), 12, 1)='1' THEN 1 ELSE 0 END + 1), 1) +
        SUBSTRING('0123456789ABCDEF', (CASE WHEN SUBSTRING(ISNULL(bhexcode, bdexcode), 13, 1)='1' THEN 8 ELSE 0 END + CASE WHEN SUBSTRING(ISNULL(bhexcode, bdexcode), 14, 1)='1' THEN 4 ELSE 0 END + CASE WHEN SUBSTRING(ISNULL(bhexcode, bdexcode), 15, 1)='1' THEN 2 ELSE 0 END + CASE WHEN SUBSTRING(ISNULL(bhexcode, bdexcode), 16, 1)='1' THEN 1 ELSE 0 END + 1), 1) +
        SUBSTRING('0123456789ABCDEF', (CASE WHEN SUBSTRING(ISNULL(bhexcode, bdexcode), 17, 1)='1' THEN 8 ELSE 0 END + CASE WHEN SUBSTRING(ISNULL(bhexcode, bdexcode), 18, 1)='1' THEN 4 ELSE 0 END + CASE WHEN SUBSTRING(ISNULL(bhexcode, bdexcode), 19, 1)='1' THEN 2 ELSE 0 END + CASE WHEN SUBSTRING(ISNULL(bhexcode, bdexcode), 20, 1)='1' THEN 1 ELSE 0 END + 1), 1) +
        SUBSTRING('0123456789ABCDEF', (CASE WHEN SUBSTRING(ISNULL(bhexcode, bdexcode), 21, 1)='1' THEN 8 ELSE 0 END + CASE WHEN SUBSTRING(ISNULL(bhexcode, bdexcode), 22, 1)='1' THEN 4 ELSE 0 END + CASE WHEN SUBSTRING(ISNULL(bhexcode, bdexcode), 23, 1)='1' THEN 2 ELSE 0 END + CASE WHEN SUBSTRING(ISNULL(bhexcode, bdexcode), 24, 1)='1' THEN 1 ELSE 0 END + 1), 1) +
        SUBSTRING('0123456789ABCDEF', (CASE WHEN SUBSTRING(ISNULL(bhexcode, bdexcode), 25, 1)='1' THEN 8 ELSE 0 END + CASE WHEN SUBSTRING(ISNULL(bhexcode, bdexcode), 26, 1)='1' THEN 4 ELSE 0 END + CASE WHEN SUBSTRING(ISNULL(bhexcode, bdexcode), 27, 1)='1' THEN 2 ELSE 0 END + CASE WHEN SUBSTRING(ISNULL(bhexcode, bdexcode), 28, 1)='1' THEN 1 ELSE 0 END + 1), 1) +
        SUBSTRING('0123456789ABCDEF', (CASE WHEN SUBSTRING(ISNULL(bhexcode, bdexcode), 29, 1)='1' THEN 8 ELSE 0 END + CASE WHEN SUBSTRING(ISNULL(bhexcode, bdexcode), 30, 1)='1' THEN 4 ELSE 0 END + CASE WHEN SUBSTRING(ISNULL(bhexcode, bdexcode), 31, 1)='1' THEN 2 ELSE 0 END + CASE WHEN SUBSTRING(ISNULL(bhexcode, bdexcode), 32, 1)='1' THEN 1 ELSE 0 END + 1), 1)
WHERE bhexcode IS NOT NULL OR bdexcode IS NOT NULL";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.CommandTimeout = 120;
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        private static string EscapeXml(string input)
        {
            if (string.IsNullOrEmpty(input)) return "";
            StringBuilder sb = new StringBuilder(input.Length + 10);
            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];
                if (c == '&') sb.Append("&amp;");
                else if (c == '<') sb.Append("&lt;");
                else if (c == '>') sb.Append("&gt;");
                else if (c == '"') sb.Append("&quot;");
                else if (c == '\'') sb.Append("&apos;");
                else if (c >= 32 || c == '\t' || c == '\n' || c == '\r') sb.Append(c);
            }
            return sb.ToString();
        }

        private static string EscapeCsv(string input)
        {
            if (string.IsNullOrEmpty(input)) return "\"\"";
            return "\"" + input.Replace("\"", "\"\"") + "\"";
        }

        public static int ExportHSolvingToExcel(string destinationFilePath, string fileNameFilter = null)
        {
            using (SqlConnection con = GetConnectionObj())
            {
                if (con == null) throw new Exception("Unable to connect to database.");

                string sql = "SELECT * FROM dbo.H_solving ";

                if (!string.IsNullOrEmpty(fileNameFilter))
                {
                    sql += " WHERE file_name = @fn ";
                }
                sql += " ORDER BY file_name, serial, id";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.CommandTimeout = 300;
                    if (!string.IsNullOrEmpty(fileNameFilter))
                    {
                        cmd.Parameters.AddWithValue("@fn", fileNameFilter);
                    }

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        bool isCsv = destinationFilePath.EndsWith(".csv", StringComparison.OrdinalIgnoreCase);
                        int rowCount = 0;

                        using (System.IO.StreamWriter writer = new System.IO.StreamWriter(destinationFilePath, false, Encoding.UTF8))
                        {
                            if (isCsv)
                            {
                                // CSV Export
                                int fieldCount = reader.FieldCount;
                                StringBuilder headerLine = new StringBuilder();
                                for (int i = 0; i < fieldCount; i++)
                                {
                                    if (i > 0) headerLine.Append(",");
                                    headerLine.Append(EscapeCsv(reader.GetName(i)));
                                }
                                writer.WriteLine(headerLine.ToString());

                                while (reader.Read())
                                {
                                    rowCount++;
                                    StringBuilder rowLine = new StringBuilder();
                                    for (int i = 0; i < fieldCount; i++)
                                    {
                                        if (i > 0) rowLine.Append(",");
                                        string val = reader.IsDBNull(i) ? "" : reader.GetValue(i).ToString();
                                        rowLine.Append(EscapeCsv(val));
                                    }
                                    writer.WriteLine(rowLine.ToString());
                                }
                            }
                            else
                            {
                                // SpreadsheetML (.xls XML) Export
                                writer.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                                writer.WriteLine("<?mso-application progid=\"Excel.Sheet\"?>");
                                writer.WriteLine("<Workbook xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\"");
                                writer.WriteLine(" xmlns:o=\"urn:schemas-microsoft-com:office:office\"");
                                writer.WriteLine(" xmlns:x=\"urn:schemas-microsoft-com:office:excel\"");
                                writer.WriteLine(" xmlns:ss=\"urn:schemas-microsoft-com:office:spreadsheet\"");
                                writer.WriteLine(" xmlns:html=\"http://www.w3.org/TR/REC-html40\">");
                                writer.WriteLine(" <Styles>");
                                writer.WriteLine("  <Style ss:ID=\"Default\" ss:Name=\"Normal\">");
                                writer.WriteLine("   <Alignment ss:Vertical=\"Center\"/>");
                                writer.WriteLine("   <Font ss:FontName=\"Calibri\" ss:Size=\"11\" ss:Color=\"#000000\"/>");
                                writer.WriteLine("  </Style>");
                                writer.WriteLine("  <Style ss:ID=\"Header\">");
                                writer.WriteLine("   <Alignment ss:Horizontal=\"Center\" ss:Vertical=\"Center\"/>");
                                writer.WriteLine("   <Borders>");
                                writer.WriteLine("    <Border ss:Position=\"Bottom\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\" ss:Color=\"#047857\"/>");
                                writer.WriteLine("   </Borders>");
                                writer.WriteLine("   <Font ss:FontName=\"Calibri\" ss:Size=\"11\" ss:Color=\"#FFFFFF\" ss:Bold=\"1\"/>");
                                writer.WriteLine("   <Interior ss:Color=\"#059669\" ss:Pattern=\"Solid\"/>");
                                writer.WriteLine("  </Style>");
                                writer.WriteLine("  <Style ss:ID=\"TextCell\">");
                                writer.WriteLine("   <NumberFormat ss:Format=\"@\"/>");
                                writer.WriteLine("  </Style>");
                                writer.WriteLine("  <Style ss:ID=\"NumCell\">");
                                writer.WriteLine("   <NumberFormat ss:Format=\"0\"/>");
                                writer.WriteLine("  </Style>");
                                writer.WriteLine("  <Style ss:ID=\"DateCell\">");
                                writer.WriteLine("   <NumberFormat ss:Format=\"yyyy-mm-dd hh:mm:ss\"/>");
                                writer.WriteLine("  </Style>");
                                writer.WriteLine(" </Styles>");
                                writer.WriteLine(" <Worksheet ss:Name=\"H_solving\">");
                                writer.WriteLine("  <Table>");

                                int fieldCount = reader.FieldCount;
                                writer.WriteLine("   <Row ss:Height=\"24\">");
                                for (int i = 0; i < fieldCount; i++)
                                {
                                    writer.WriteLine(string.Format("    <Cell ss:StyleID=\"Header\"><Data ss:Type=\"String\">{0}</Data></Cell>", EscapeXml(reader.GetName(i))));
                                }
                                writer.WriteLine("   </Row>");

                                while (reader.Read())
                                {
                                    rowCount++;
                                    writer.WriteLine("   <Row>");
                                    for (int i = 0; i < fieldCount; i++)
                                    {
                                        string colName = reader.GetName(i);
                                        if (reader.IsDBNull(i))
                                        {
                                            writer.WriteLine("    <Cell ss:StyleID=\"TextCell\"><Data ss:Type=\"String\"></Data></Cell>");
                                        }
                                        else if (colName.Equals("id", StringComparison.OrdinalIgnoreCase) || colName.Equals("serial", StringComparison.OrdinalIgnoreCase))
                                        {
                                            long numVal;
                                            if (long.TryParse(reader.GetValue(i).ToString(), out numVal))
                                            {
                                                writer.WriteLine(string.Format("    <Cell ss:StyleID=\"NumCell\"><Data ss:Type=\"Number\">{0}</Data></Cell>", numVal));
                                            }
                                            else
                                            {
                                                string strVal = EscapeXml(reader.GetValue(i).ToString());
                                                writer.WriteLine(string.Format("    <Cell ss:StyleID=\"TextCell\"><Data ss:Type=\"String\">{0}</Data></Cell>", strVal));
                                            }
                                        }
                                        else if (reader.GetValue(i) is DateTime)
                                        {
                                            DateTime dt = (DateTime)reader.GetValue(i);
                                            writer.WriteLine(string.Format("    <Cell ss:StyleID=\"DateCell\"><Data ss:Type=\"DateTime\">{0:yyyy-MM-ddTHH:mm:ss.fff}</Data></Cell>", dt));
                                        }
                                        else
                                        {
                                            // Format as explicit String to preserve leading zeroes (e.g. eb_no, sl_no, marks, pap_code, hex codes)
                                            string strVal = EscapeXml(reader.GetValue(i).ToString());
                                            writer.WriteLine(string.Format("    <Cell ss:StyleID=\"TextCell\"><Data ss:Type=\"String\">{0}</Data></Cell>", strVal));
                                        }
                                    }
                                    writer.WriteLine("   </Row>");
                                }

                                writer.WriteLine("  </Table>");
                                writer.WriteLine(" </Worksheet>");
                                writer.WriteLine("</Workbook>");
                            }
                        }

                        return rowCount;
                    }
                }
            }
        }

        public class TableCreationResult
        {
            public List<string> CreatedTables { get; set; }
            public List<string> ExistingTables { get; set; }
            public List<string> Errors { get; set; }
            public bool Success { get { return Errors == null || Errors.Count == 0; } }

            public TableCreationResult()
            {
                CreatedTables = new List<string>();
                ExistingTables = new List<string>();
                Errors = new List<string>();
            }
        }

        public static TableCreationResult CreateDatabaseTablesIfNotExists(string connectionString = null)
        {
            TableCreationResult result = new TableCreationResult();

            var tableDefinitions = new Dictionary<string, string>
            {
                {
                    "solvers",
                    @"CREATE TABLE [dbo].[solvers] (
                        [solver_name] varchar(20) NOT NULL PRIMARY KEY,
                        [passkey] varchar(50) NOT NULL,
                        [status] varchar(10) NOT NULL,
                        [full_name] varchar(60) NULL
                    )"
                },
                {
                    "file_list",
                    @"CREATE TABLE [dbo].[file_list] (
                        [file_name] varchar(30) NOT NULL PRIMARY KEY,
                        [solver_name] varchar(20) NULL,
                        [solve_status] varchar(15) NULL,
                        [start_date] date NULL,
                        [end_date] date NULL,
                        [solver_comments] nchar(255) NULL,
                        [pap_code] varchar(6) NULL,
                        [insert_date] date NULL,
                        [rows] int NULL,
                        [corrected_rows] int NULL
                    )"
                },
                {
                    "file_list_other",
                    @"CREATE TABLE [dbo].[file_list_other] (
                        [file_name] varchar(30) NOT NULL PRIMARY KEY,
                        [solver_name] varchar(20) NULL,
                        [solve_status] varchar(15) NULL,
                        [start_date] date NULL,
                        [end_date] date NULL,
                        [solver_comments] nchar(255) NULL,
                        [pap_code] varchar(6) NULL,
                        [insert_date] date NULL,
                        [rows] int NULL,
                        [corrected_rows] int NULL
                    )"
                },
                {
                    "H_file_list",
                    @"CREATE TABLE [dbo].[H_file_list] (
                        [file_name] varchar(30) NOT NULL PRIMARY KEY,
                        [solver_name] varchar(20) NULL,
                        [solve_status] varchar(15) NULL,
                        [start_date] date NULL,
                        [end_date] date NULL,
                        [solver_comments] varchar(255) NULL,
                        [pap_code] varchar(6) NULL,
                        [insert_date] datetime NULL,
                        [rows] int NULL,
                        [corrected_rows] int NULL,
                        [master_file] varchar(255) NULL,
                        [exm_code] varchar(6) NULL,
                        [exm_year] varchar(4) NULL
                    )"
                },
                {
                    "H_solving",
                    @"CREATE TABLE [dbo].[H_solving] (
                        [id] bigint NOT NULL IDENTITY(1,1) PRIMARY KEY,
                        [file_name] varchar(30) NOT NULL,
                        [scanned_row] varchar(160) NOT NULL,
                        [serial] int NULL,
                        [err_dscr] varchar(255) NULL,
                        [exam_code] varchar(10) NULL,
                        [pap_code] varchar(6) NULL,
                        [bdexcode] char(32) NULL,
                        [bhexcode] char(32) NULL,
                        [qr_code] varchar(28) NULL,
                        [solver] varchar(15) NULL,
                        [updates] varchar(1024) NOT NULL,
                        [last_update] datetime NULL,
                        [binary] varchar(32) NULL,
                        [marks] varchar(3) NULL,
                        [hmarks] varchar(3) NULL,
                        [eb_no] varchar(6) NULL,
                        [sl_no] varchar(3) NULL,
                        [comments] varchar(50) NULL,
                        [hexcodea] varchar(8) NULL,
                        [hexcodeb] varchar(8) NULL,
                        [reg_no] varchar(256) NULL,
                        [Ehex_no] varchar(8) NULL,
                        [qr1] varchar(18) NULL,
                        [qr2] varchar(18) NULL,
                        [qr3] varchar(18) NULL,
                        [form_sl] varchar(11) NULL,
                        [hex1] varchar(8) NULL,
                        [hex2] varchar(8) NULL,
                        [hex3] varchar(8) NULL,
                        [LU_status] varchar(11) NULL,
                        [exm_year] varchar(4) NULL,
                        [exm_code] varchar(10) NULL
                    )"
                },
                {
                    "H_solving_bedhons_5s_24_lot2",
                    @"CREATE TABLE [dbo].[H_solving_bedhons_5s_24_lot2] (
                        [id] bigint NOT NULL IDENTITY(1,1) PRIMARY KEY,
                        [file_name] varchar(30) NOT NULL,
                        [scanned_row] varchar(160) NOT NULL,
                        [serial] int NULL,
                        [err_dscr] varchar(100) NULL,
                        [exam_code] char(4) NULL,
                        [pap_code] varchar(6) NULL,
                        [bdexcode] char(32) NULL,
                        [bhexcode] char(32) NULL,
                        [qr_code] varchar(28) NULL,
                        [solver] varchar(15) NULL,
                        [updates] varchar(1024) NOT NULL,
                        [last_update] datetime NULL,
                        [binary] varchar(32) NULL,
                        [marks] varchar(3) NULL,
                        [hmarks] varchar(3) NULL,
                        [eb_no] varchar(6) NULL,
                        [sl_no] varchar(3) NULL,
                        [comments] varchar(50) NULL,
                        [hexcodea] varchar(8) NULL,
                        [hexcodeb] varchar(8) NULL,
                        [reg_no] varchar(256) NULL,
                        [Ehex_no] varchar(8) NULL,
                        [qr1] varchar(18) NULL,
                        [qr2] varchar(18) NULL,
                        [qr3] varchar(18) NULL,
                        [form_sl] varchar(11) NULL,
                        [hex1] varchar(8) NULL,
                        [hex2] varchar(8) NULL,
                        [hex3] varchar(8) NULL,
                        [LU_status] varchar(11) NULL,
                        [exm_year] varchar(4) NULL,
                        [exm_code] varchar(10) NULL
                    )"
                },
                {
                    "E_solving",
                    @"CREATE TABLE [dbo].[E_solving] (
                        [id] int NOT NULL IDENTITY(1,1) PRIMARY KEY,
                        [file_name] varchar(30) NOT NULL,
                        [scanned_row] nvarchar(255) NOT NULL,
                        [serial] nvarchar(255) NULL,
                        [err_dscr] nvarchar(255) NULL,
                        [exam_code] nvarchar(255) NULL,
                        [pap_code] nvarchar(255) NULL,
                        [regi] nvarchar(255) NULL,
                        [litho_1] nvarchar(255) NULL,
                        [litho_2] nvarchar(255) NULL,
                        [qr_code] nvarchar(255) NULL,
                        [solver] nvarchar(255) NULL,
                        [updates] nvarchar(255) NULL,
                        [last_update] nvarchar(255) NULL,
                        [scrpt_no] nvarchar(255) NULL,
                        [hexcodea] nvarchar(255) NULL,
                        [hexcodeb] nvarchar(255) NULL,
                        [htthex] nvarchar(255) NULL
                    )"
                },
                {
                    "mcq",
                    @"CREATE TABLE [dbo].[mcq] (
                        [sl] nvarchar(255) NULL,
                        [filename] nvarchar(255) NULL,
                        [col_code] nvarchar(255) NULL,
                        [reg_no] nvarchar(255) NULL,
                        [year] nvarchar(255) NULL,
                        [semester] nvarchar(255) NULL,
                        [exm_year] nvarchar(255) NULL,
                        [course_code] nvarchar(255) NULL,
                        [mca1] nvarchar(255) NULL,
                        [mca2] nvarchar(255) NULL,
                        [mca3] nvarchar(255) NULL,
                        [mca4] nvarchar(255) NULL,
                        [mca5] nvarchar(255) NULL,
                        [mca6] nvarchar(255) NULL,
                        [mca7] nvarchar(255) NULL,
                        [mca8] nvarchar(255) NULL,
                        [mca9] nvarchar(255) NULL,
                        [mca10] nvarchar(255) NULL,
                        [mca11] nvarchar(255) NULL,
                        [mca12] nvarchar(255) NULL,
                        [mca13] nvarchar(255) NULL,
                        [mca14] nvarchar(255) NULL,
                        [mca15] nvarchar(255) NULL,
                        [mca16] nvarchar(255) NULL,
                        [mca17] nvarchar(255) NULL,
                        [mca18] nvarchar(255) NULL,
                        [mca19] nvarchar(255) NULL,
                        [mca20] nvarchar(255) NULL,
                        [mark] nvarchar(255) NULL
                    )"
                },
                {
                    "mtemp",
                    @"CREATE TABLE [dbo].[mtemp] (
                        [reg_no] varchar(255) NULL,
                        [exm_roll] varchar(255) NULL,
                        [session] varchar(513) NULL,
                        [std_type] varchar(255) NULL,
                        [semester] varchar(1) NULL,
                        [year] varchar(4) NULL,
                        [sub_code] varchar(5) NULL,
                        [c_code] varchar(255) NULL,
                        [center] varchar(255) NULL,
                        [pap_code] varchar(255) NULL,
                        [col_name] varchar(255) NULL,
                        [col_code] varchar(255) NULL,
                        [mcq_marks] smallint NULL,
                        [incourse_marks] smallint NULL,
                        [marks1] smallint NULL,
                        [marks2] varchar(3) NULL,
                        [mcq_inc_total] smallint NULL,
                        [final_marks] smallint NULL,
                        [grade] varchar(2) NULL,
                        [grade_point] real NULL,
                        [sgpa] varchar(4) NULL,
                        [status] varchar(20) NULL,
                        [total_marks] varchar(10) NULL,
                        [eb_no] varchar(10) NULL,
                        [sl_no] varchar(10) NULL,
                        [hexcodea] varchar(8) NULL,
                        [hexcodeb] varchar(8) NULL,
                        [script_no] varchar(32) NULL,
                        [exm_code] varchar(10) NULL,
                        [id] int NULL,
                        [inc_mark] varchar(255) NULL,
                        [total_sub] varchar(255) NULL,
                        [ems_status] varchar(255) NULL,
                        [Ehex_no] varchar(8) NULL,
                        [qr1] varchar(255) NULL,
                        [qr2] varchar(255) NULL,
                        [qr3] varchar(255) NULL,
                        [form_sl] varchar(11) NULL,
                        [sub_type] varchar(20) NULL,
                        [ems_present_status] varchar(255) NULL
                    )"
                },
                {
                    "mtemp_format",
                    @"CREATE TABLE [dbo].[mtemp_format] (
                        [reg_no] varchar(255) NULL,
                        [exm_roll] varchar(255) NULL,
                        [session] varchar(513) NULL,
                        [std_type] varchar(255) NULL,
                        [semester] varchar(1) NULL,
                        [year] varchar(4) NULL,
                        [sub_code] varchar(5) NULL,
                        [c_code] varchar(255) NULL,
                        [center] varchar(255) NULL,
                        [pap_code] varchar(255) NULL,
                        [col_name] varchar(255) NULL,
                        [col_code] varchar(255) NULL,
                        [mcq_marks] smallint NULL,
                        [incourse_marks] smallint NULL,
                        [marks1] smallint NULL,
                        [marks2] varchar(3) NULL,
                        [mcq_inc_total] smallint NULL,
                        [final_marks] smallint NULL,
                        [grade] varchar(2) NULL,
                        [grade_point] real NULL,
                        [sgpa] varchar(4) NULL,
                        [status] varchar(20) NULL,
                        [total_marks] varchar(10) NULL,
                        [eb_no] varchar(10) NULL,
                        [sl_no] varchar(10) NULL,
                        [hexcodea] varchar(8) NULL,
                        [hexcodeb] varchar(8) NULL,
                        [script_no] varchar(32) NULL,
                        [exm_code] varchar(10) NULL,
                        [id] int NULL,
                        [inc_mark] varchar(255) NULL,
                        [total_sub] varchar(255) NULL,
                        [ems_status] varchar(255) NULL,
                        [Ehex_no] varchar(8) NULL,
                        [qr1] varchar(255) NULL,
                        [qr2] varchar(255) NULL,
                        [qr3] varchar(255) NULL,
                        [form_sl] varchar(11) NULL,
                        [sub_type] varchar(20) NULL,
                        [ems_present_status] varchar(255) NULL
                    )"
                },
                {
                    "BEDHONS_5685_ff_split_data_24_NEW",
                    @"CREATE TABLE [dbo].[BEDHONS_5685_ff_split_data_24_NEW] (
                        [reg_no] varchar(255) NULL,
                        [inc_mark] varchar(255) NULL,
                        [sif_sub] varchar(255) NULL,
                        [std_type] varchar(1) NULL
                    )"
                },
                {
                    "examinee_course",
                    @"CREATE TABLE [dbo].[examinee_course] (
                        [exm_roll] varchar(11) NOT NULL,
                        [semester] varchar(1) NOT NULL,
                        [type] varchar(1) NOT NULL,
                        [exm_year] varchar(10) NOT NULL,
                        [course_code] varchar(6) NOT NULL,
                        [incourse_marks] smallint NULL,
                        [final_marks] smallint NULL,
                        [mcq_marks] smallint NULL,
                        [total_marks] varchar(50) NULL,
                        [grade] varchar(2) NULL,
                        [grade_point] float(53) NULL,
                        [credits] float(53) NULL,
                        [course_point] float(53) NULL,
                        [status] varchar(100) NULL,
                        [date] datetime2(7) NULL,
                        [satisfactory] varchar(2) NULL,
                        [reg_no] varchar(11) NULL,
                        [session] varchar(20) NULL,
                        [col_code] varchar(20) NULL,
                        [ff_status] varchar(20) NULL,
                        [gender] varchar(255) NULL,
                        [course_name] varchar(255) NULL,
                        [total_check] varchar(255) NULL,
                        [missmatch] varchar(255) NULL,
                        [update_to_EC] int NULL,
                        [ready_to_publish] varchar(1) NULL,
                        CONSTRAINT [PK_examinee_course] PRIMARY KEY ([exm_roll], [semester], [type], [exm_year], [course_code])
                    )"
                },
                {
                    "bedhons_courses",
                    @"CREATE TABLE [dbo].[bedhons_courses] (
                        [course_code] varchar(6) NOT NULL PRIMARY KEY,
                        [course_name] varchar(200) NULL,
                        [semester] varchar(1) NULL,
                        [credits] float(53) NULL,
                        [full_marks] float(53) NULL,
                        [type] varchar(4) NULL,
                        [exm_code] varchar(5) NULL,
                        [inc_mark] varchar(4) NULL,
                        [sub_type] varchar(20) NULL,
                        [sub_area] varchar(50) NULL,
                        [prac_mark] varchar(4) NULL,
                        [theo_mark] varchar(4) NULL
                    )"
                },
                {
                    "FF",
                    @"CREATE TABLE [dbo].[FF] (
                        [exm_code] nvarchar(255) NULL,
                        [exm_year] nvarchar(255) NULL,
                        [exam_roll_no] nvarchar(255) NULL,
                        [degree] nvarchar(255) NULL,
                        [course] nvarchar(255) NULL,
                        [course_code] nvarchar(255) NULL,
                        [sub_name] nvarchar(255) NULL,
                        [sub_code] nvarchar(255) NULL,
                        [reg_no] nvarchar(255) NULL,
                        [adm_roll] nvarchar(255) NULL,
                        [exm_roll] nvarchar(255) NULL,
                        [sess1] nvarchar(255) NULL,
                        [sess2] nvarchar(255) NULL,
                        [ac_session] nvarchar(255) NULL,
                        [reg_type] nvarchar(255) NULL,
                        [std_type] nvarchar(255) NULL,
                        [col_code] nvarchar(255) NULL,
                        [col_name] nvarchar(255) NULL,
                        [cen_code] nvarchar(255) NULL,
                        [cen_col_code] nvarchar(255) NULL,
                        [cen_name] nvarchar(255) NULL,
                        [std_name] nvarchar(255) NULL,
                        [fname] nvarchar(255) NULL,
                        [mname] nvarchar(255) NULL,
                        [pap_code] nvarchar(255) NULL,
                        [inc_marks] nvarchar(255) NULL,
                        [ict_sub] nvarchar(255) NULL,
                        [thesis] nvarchar(255) NULL,
                        [nu_fee] nvarchar(255) NULL,
                        [con_status] nvarchar(255) NULL,
                        [app_date] nvarchar(255) NULL,
                        [con_date] nvarchar(255) NULL,
                        [inc_mark] varchar(255) NULL,
                        [total_sub] varchar(255) NULL
                    )"
                },
                {
                    "attandance",
                    @"CREATE TABLE [dbo].[attandance] (
                        [exam_code] nvarchar(255) NULL,
                        [exam_year] nvarchar(255) NULL,
                        [att_date] nvarchar(255) NULL,
                        [reg_no] nvarchar(255) NULL,
                        [exm_roll] nvarchar(255) NULL,
                        [cen_code] nvarchar(255) NULL,
                        [paper_code] nvarchar(255) NULL,
                        [script_qr] nvarchar(255) NULL,
                        [script_sl] nvarchar(255) NULL
                    )"
                },
                {
                    "reject_entry",
                    @"CREATE TABLE [dbo].[reject_entry] (
                        [qr_bar_code] varchar(255) NULL,
                        [course_code] varchar(255) NULL,
                        [TIMS_EB_CODE] varchar(255) NULL,
                        [Serial_no] int NULL,
                        [mark1] varchar(255) NULL,
                        [mark2] varchar(255) NULL,
                        [litho1] varchar(255) NULL,
                        [litho2] varchar(255) NULL,
                        [id] int NULL,
                        [created_at] varchar(255) NULL,
                        [saved_by] varchar(255) NULL,
                        [course_name] varchar(255) NULL,
                        [year] varchar(255) NULL
                    )"
                },
                {
                    "reject_entry_courses",
                    @"CREATE TABLE [dbo].[reject_entry_courses] (
                        [course_name] varchar(255) NULL,
                        [user] varchar(255) NULL,
                        [year] varchar(255) NULL
                    )"
                },
                {
                    "p1syllabas1314_med",
                    @"CREATE TABLE [dbo].[p1syllabas1314_med] (
                        [PAP_CODE] varchar(10) NOT NULL PRIMARY KEY,
                        [type] varchar(2) NULL,
                        [semester] varchar(1) NULL,
                        [course_code] varchar(255) NULL
                    )"
                },
                {
                    "p1syllabas1314",
                    @"CREATE TABLE [dbo].[p1syllabas1314] (
                        [PAP_CODE] varchar(10) NOT NULL PRIMARY KEY,
                        [type] varchar(2) NULL,
                        [semester] varchar(1) NULL,
                        [COURSE_CODE] varchar(255) NULL
                    )"
                },
                {
                    "absent_Entry",
                    @"CREATE TABLE [dbo].[absent_Entry] (
                        [reg_no] nvarchar(255) NULL,
                        [col_code] nvarchar(255) NULL,
                        [pap_code] nvarchar(255) NULL,
                        [script_no] nvarchar(255) NULL,
                        [hexcodea] nvarchar(255) NULL,
                        [hexcodeb] nvarchar(255) NULL,
                        [mark] varchar(10) NULL,
                        [status] varchar(10) NULL,
                        [KEY] varchar(256) NULL,
                        [SEMESTER] varchar(255) NULL,
                        [YEAR] varchar(255) NULL,
                        [EXM_NAME] varchar(255) NULL,
                        [qr1] varchar(18) NULL,
                        [qr2] varchar(18) NULL,
                        [qr3] varchar(18) NULL,
                        [hex1] varchar(8) NULL,
                        [hex2] varchar(8) NULL,
                        [hex3] varchar(8) NULL,
                        [form_sl] varchar(11) NULL,
                        [LU_status] varchar(11) NULL,
                        [e_hex] varchar(255) NULL,
                        [h_hex] varchar(255) NULL,
                        [pstatus] varchar(255) NULL,
                        [hmark] varchar(10) NULL,
                        [sl_no] int NULL,
                        [eb_no] int NULL,
                        [c_code] varchar(255) NULL
                    )"
                },
                {
                    "absent",
                    @"CREATE TABLE [dbo].[absent] (
                        [reg_no] nvarchar(255) NULL,
                        [col_code] nvarchar(255) NULL,
                        [pap_code] nvarchar(255) NULL,
                        [script_no] nvarchar(255) NULL,
                        [hexcodea] varchar(255) NULL,
                        [hexcodeb] varchar(255) NULL,
                        [mark] varchar(10) NULL,
                        [status] varchar(10) NULL,
                        [KEY] varchar(256) NULL,
                        [SEMESTER] varchar(255) NULL,
                        [YEAR] varchar(255) NULL,
                        [EXM_NAME] varchar(255) NULL,
                        [qr1] varchar(18) NULL,
                        [qr2] varchar(18) NULL,
                        [qr3] varchar(18) NULL,
                        [hex1] varchar(8) NULL,
                        [hex2] varchar(8) NULL,
                        [hex3] varchar(8) NULL,
                        [form_sl] varchar(11) NULL,
                        [LU_status] varchar(11) NULL,
                        [e_hex] varchar(255) NULL,
                        [h_hex] varchar(255) NULL,
                        [pstatus] varchar(255) NULL,
                        [hmark] varchar(10) NULL,
                        [sl_no] int NULL,
                        [eb_no] int NULL,
                        [c_code] varchar(255) NULL
                    )"
                },
                {
                    "ABSENT_wanting",
                    @"CREATE TABLE [dbo].[ABSENT_wanting] (
                        [no] bigint NULL,
                        [col_code] varchar(255) NULL,
                        [c_code] varchar(255) NULL,
                        [reg_no] varchar(255) NULL,
                        [pap_code] varchar(255) NULL,
                        [SL_no] varchar(255) NULL
                    )"
                },
                {
                    "sysdiagrams",
                    @"CREATE TABLE [dbo].[sysdiagrams] (
                        [name] sysname NOT NULL,
                        [principal_id] int NOT NULL,
                        [diagram_id] int NOT NULL IDENTITY(1,1) PRIMARY KEY,
                        [version] int NULL,
                        [definition] varbinary(MAX) NULL,
                        CONSTRAINT [UK_sysdiagrams_principal_name] UNIQUE ([principal_id] ASC, [name] ASC)
                    )"
                }
            };

            try
            {
                using (SqlConnection con = string.IsNullOrWhiteSpace(connectionString) ? GetConnectionObj() : new SqlConnection(connectionString))
                {
                    if (con == null)
                    {
                        result.Errors.Add("Failed to establish a connection to the SQL Server database.");
                        return result;
                    }
                    if (con.State != System.Data.ConnectionState.Open)
                    {
                        con.Open();
                    }

                    // 1. Create tables if not exists
                    foreach (var kvp in tableDefinitions)
                    {
                        string tableName = kvp.Key;
                        string createDdl = kvp.Value;

                        try
                        {
                            bool tableExists = false;
                            string checkSql = "SELECT COUNT(*) FROM sys.objects WHERE object_id = OBJECT_ID(@tbl) AND type = 'U'";
                            using (SqlCommand checkCmd = new SqlCommand(checkSql, con))
                            {
                                checkCmd.Parameters.AddWithValue("@tbl", "[dbo].[" + tableName + "]");
                                int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                                tableExists = (count > 0);
                            }

                            if (!tableExists)
                            {
                                using (SqlCommand createCmd = new SqlCommand(createDdl, con))
                                {
                                    createCmd.CommandTimeout = 60;
                                    createCmd.ExecuteNonQuery();
                                }
                                result.CreatedTables.Add(tableName);
                            }
                            else
                            {
                                result.ExistingTables.Add(tableName);
                            }
                        }
                        catch (Exception exTable)
                        {
                            result.Errors.Add("Error checking/creating table [" + tableName + "]: " + exTable.Message);
                        }
                    }

                    // 2. Ensure indexes exist safely
                    try
                    {
                        string checkIndexSql = @"
                            IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[reject_entry]') AND type in (N'U'))
                               AND NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'UQ_reject_entry_qr' AND object_id = OBJECT_ID('[dbo].[reject_entry]'))
                            BEGIN
                                CREATE UNIQUE INDEX [UQ_reject_entry_qr] ON [dbo].[reject_entry]
                                ([qr_bar_code] ASC)
                                WHERE ([qr_bar_code] IS NOT NULL AND [qr_bar_code]<>'')
                                WITH (IGNORE_DUP_KEY = ON)
                            END

                            IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[mtemp]') AND type in (N'U'))
                               AND NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_mtemp_reg_pap' AND object_id = OBJECT_ID('[dbo].[mtemp]'))
                            BEGIN
                                CREATE INDEX [IX_mtemp_reg_pap] ON [dbo].[mtemp] ([reg_no] ASC, [pap_code] ASC)
                            END

                            IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[mtemp]') AND type in (N'U'))
                               AND NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_mtemp_qr1' AND object_id = OBJECT_ID('[dbo].[mtemp]'))
                            BEGIN
                                CREATE INDEX [IX_mtemp_qr1] ON [dbo].[mtemp] ([qr1] ASC)
                            END

                            IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[mtemp_format]') AND type in (N'U'))
                               AND NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_mtemp_reg_pap' AND object_id = OBJECT_ID('[dbo].[mtemp_format]'))
                            BEGIN
                                CREATE INDEX [IX_mtemp_reg_pap] ON [dbo].[mtemp_format] ([reg_no] ASC, [pap_code] ASC)
                            END

                            IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[mtemp_format]') AND type in (N'U'))
                               AND NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_mtemp_qr1' AND object_id = OBJECT_ID('[dbo].[mtemp_format]'))
                            BEGIN
                                CREATE INDEX [IX_mtemp_qr1] ON [dbo].[mtemp_format] ([qr1] ASC)
                            END";
                        using (SqlCommand indexCmd = new SqlCommand(checkIndexSql, con))
                        {
                            indexCmd.ExecuteNonQuery();
                        }
                    }
                    catch (Exception exIdx)
                    {
                        result.Errors.Add("Notice on indexes: " + exIdx.Message);
                    }

                    // 3. Ensure Foreign Keys exist safely
                    try
                    {
                        string fkHSolving = @"
                            IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[H_solving]') AND type in (N'U'))
                               AND EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[H_file_list]') AND type in (N'U'))
                               AND NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_H_solving_H_file_list' AND parent_object_id = OBJECT_ID(N'[dbo].[H_solving]'))
                            BEGIN
                                ALTER TABLE [dbo].[H_solving] ADD CONSTRAINT [FK_H_solving_H_file_list]
                                FOREIGN KEY ([file_name]) REFERENCES [dbo].[H_file_list] ([file_name]) ON DELETE CASCADE ON UPDATE NO ACTION
                            END";
                        using (SqlCommand fkCmd = new SqlCommand(fkHSolving, con))
                        {
                            fkCmd.ExecuteNonQuery();
                        }
                    }
                    catch (Exception exFk)
                    {
                        result.Errors.Add("Notice on FK [FK_H_solving_H_file_list]: " + exFk.Message);
                    }

                    try
                    {
                        string fkESolving = @"
                            IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[E_solving]') AND type in (N'U'))
                               AND EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[file_list]') AND type in (N'U'))
                               AND NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_E_solving_file_list' AND parent_object_id = OBJECT_ID(N'[dbo].[E_solving]'))
                            BEGIN
                                ALTER TABLE [dbo].[E_solving] ADD CONSTRAINT [FK_E_solving_file_list]
                                FOREIGN KEY ([file_name]) REFERENCES [dbo].[file_list] ([file_name]) ON DELETE CASCADE ON UPDATE CASCADE
                            END";
                        using (SqlCommand fkCmd2 = new SqlCommand(fkESolving, con))
                        {
                            fkCmd2.ExecuteNonQuery();
                        }
                    }
                    catch (Exception exFk2)
                    {
                        result.Errors.Add("Notice on FK [FK_E_solving_file_list]: " + exFk2.Message);
                    }

                    // 4. Seed default admin user if missing
                    try
                    {
                        string seedAdminSql = @"
                            IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[solvers]') AND type in (N'U'))
                               AND NOT EXISTS (SELECT 1 FROM [dbo].[solvers] WHERE [solver_name] = 'admin')
                            BEGIN
                                INSERT INTO [dbo].[solvers] ([solver_name], [passkey], [status], [full_name])
                                VALUES ('admin', '1234', 'admin', 'System Administrator')
                            END";
                        using (SqlCommand seedCmd = new SqlCommand(seedAdminSql, con))
                        {
                            seedCmd.ExecuteNonQuery();
                        }
                    }
                    catch (Exception exAdmin)
                    {
                        result.Errors.Add("Notice on default admin seed: " + exAdmin.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                result.Errors.Add("General error during table creation: " + ex.Message);
            }

            return result;
        }

        public class MtempCreationResult
        {
            public bool Success { get; set; }
            public int RowsInserted { get; set; }
            public string Message { get; set; }
            public List<string> Details { get; set; }

            public MtempCreationResult()
            {
                Details = new List<string>();
            }
        }

        public static MtempCreationResult CreateMtempFromAttendance(string connectionString = null)
        {
            MtempCreationResult result = new MtempCreationResult();
            string connStr = connectionString ?? GetConnectionString();

            try
            {
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    con.Open();

                    // 1. Check if 'attandance' source table exists
                    using (SqlCommand chkAttCmd = new SqlCommand("SELECT COUNT(*) FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[attandance]') AND type in (N'U')", con))
                    {
                        int count = Convert.ToInt32(chkAttCmd.ExecuteScalar());
                        if (count == 0)
                        {
                            result.Success = false;
                            result.Message = "Source table 'attandance' was not found in the database.";
                            result.Details.Add("Please ensure 'attandance' table is imported or created before running this operation.");
                            return result;
                        }
                    }

                    // 2. Check if 'FF' table exists
                    bool hasFF = false;
                    using (SqlCommand chkFFCmd = new SqlCommand("SELECT COUNT(*) FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FF]') AND type in (N'U')", con))
                    {
                        hasFF = Convert.ToInt32(chkFFCmd.ExecuteScalar()) > 0;
                    }

                    if (hasFF)
                    {
                        result.Details.Add("Found 'FF' table. Verifying required columns...");
                        // Check & add inc_mark and total_sub if missing
                        string alterFF = @"
                            IF COL_LENGTH('dbo.FF', 'inc_mark') IS NULL
                                ALTER TABLE [dbo].[FF] ADD [inc_mark] VARCHAR(255) NULL;
                            IF COL_LENGTH('dbo.FF', 'total_sub') IS NULL
                                ALTER TABLE [dbo].[FF] ADD [total_sub] VARCHAR(255) NULL;";
                        using (SqlCommand alterCmd = new SqlCommand(alterFF, con))
                        {
                            alterCmd.ExecuteNonQuery();
                        }
                        result.Details.Add("Verified 'FF' columns (inc_mark, total_sub).");
                    }
                    else
                    {
                        result.Details.Add("Notice: 'FF' table not found. Default academic values will be used.");
                    }

                    // 3. Check if 'bedhons_courses' table exists
                    bool hasBedhons = false;
                    using (SqlCommand chkBedCmd = new SqlCommand("SELECT COUNT(*) FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[bedhons_courses]') AND type in (N'U')", con))
                    {
                        hasBedhons = Convert.ToInt32(chkBedCmd.ExecuteScalar()) > 0;
                    }

                    if (hasBedhons)
                    {
                        result.Details.Add("Found 'bedhons_courses' table for course information matching.");
                    }
                    else
                    {
                        result.Details.Add("Notice: 'bedhons_courses' table not found. Default semester '5' will be used.");
                    }

                    // 4. Drop and recreate 'mtemp' table with exact 40 columns
                    string ddlMtemp = @"
                        IF OBJECT_ID(N'[dbo].[mtemp]', N'U') IS NOT NULL
                            DROP TABLE [dbo].[mtemp];

                        CREATE TABLE [dbo].[mtemp] (
                            [reg_no] varchar(255) NULL,
                            [exm_roll] varchar(255) NULL,
                            [session] varchar(513) NULL,
                            [std_type] varchar(255) NULL,
                            [semester] varchar(1) NULL,
                            [year] varchar(4) NULL,
                            [sub_code] varchar(5) NULL,
                            [c_code] varchar(255) NULL,
                            [center] varchar(255) NULL,
                            [pap_code] varchar(255) NULL,
                            [col_name] varchar(255) NULL,
                            [col_code] varchar(255) NULL,
                            [mcq_marks] smallint NULL,
                            [incourse_marks] smallint NULL,
                            [marks1] smallint NULL,
                            [marks2] varchar(3) NULL,
                            [mcq_inc_total] smallint NULL,
                            [final_marks] smallint NULL,
                            [grade] varchar(2) NULL,
                            [grade_point] real NULL,
                            [sgpa] varchar(4) NULL,
                            [status] varchar(20) NULL,
                            [total_marks] varchar(10) NULL,
                            [eb_no] varchar(10) NULL,
                            [sl_no] varchar(10) NULL,
                            [hexcodea] varchar(8) NULL,
                            [hexcodeb] varchar(8) NULL,
                            [script_no] varchar(32) NULL,
                            [exm_code] varchar(10) NULL,
                            [id] int NULL,
                            [inc_mark] varchar(255) NULL,
                            [total_sub] varchar(255) NULL,
                            [ems_status] varchar(255) NULL,
                            [Ehex_no] varchar(8) NULL,
                            [qr1] varchar(255) NULL,
                            [qr2] varchar(255) NULL,
                            [qr3] varchar(255) NULL,
                            [form_sl] varchar(11) NULL,
                            [sub_type] varchar(20) NULL,
                            [ems_present_status] varchar(255) NULL
                        );";

                    using (SqlCommand ddlCmd = new SqlCommand(ddlMtemp, con))
                    {
                        ddlCmd.CommandTimeout = 120;
                        ddlCmd.ExecuteNonQuery();
                    }
                    result.Details.Add("Created 'mtemp' table with exact 40-column schema matching mtemp.sql.");

                    // 5. Populate 'mtemp' from 'attandance' (with FF and bedhons_courses joins)
                    string sessionExpr = hasFF
                        ? @"CASE 
                                WHEN f.sess1 IS NOT NULL AND f.sess2 IS NOT NULL THEN CONCAT('20', f.sess1, '-', f.sess2)
                                WHEN LEN(a.reg_no) >= 2 AND ISNUMERIC(SUBSTRING(a.reg_no, 1, 2)) = 1 
                                     THEN CONCAT('20', SUBSTRING(a.reg_no, 1, 2), '-', CAST(CAST(SUBSTRING(a.reg_no, 1, 2) AS INT) + 1 AS VARCHAR))
                                ELSE '' 
                            END"
                        : @"CASE 
                                WHEN LEN(a.reg_no) >= 2 AND ISNUMERIC(SUBSTRING(a.reg_no, 1, 2)) = 1 
                                     THEN CONCAT('20', SUBSTRING(a.reg_no, 1, 2), '-', CAST(CAST(SUBSTRING(a.reg_no, 1, 2) AS INT) + 1 AS VARCHAR))
                                ELSE '' 
                            END";

                    string stdTypeExpr = hasFF ? "ISNULL(f.std_type, '')" : "''";
                    string semesterExpr = hasBedhons ? "ISNULL(bc.semester, '5')" : "'5'";
                    string yearExpr = "ISNULL(a.exam_year, '2024')";
                    string subCodeExpr = hasFF ? "ISNULL(f.sub_code, '502')" : "'502'";
                    string centerExpr = hasFF ? "ISNULL(f.cen_name, '')" : "''";
                    string colNameExpr = hasFF ? "ISNULL(f.col_name, '')" : "''";
                    string colCodeExpr = hasFF ? "ISNULL(f.col_code, '')" : "''";
                    string exmCodeExpr = "ISNULL(a.exam_code, '5685')";
                    string incMarkExpr = hasFF ? "ISNULL(f.inc_mark, '')" : "''";
                    string totalSubExpr = hasFF ? "ISNULL(f.total_sub, '')" : "''";
                    string subTypeExpr = hasBedhons ? "bc.sub_type" : "NULL";

                    string fromClause = "FROM dbo.attandance a WITH (NOLOCK)\n";
                    if (hasFF)
                        fromClause += "LEFT JOIN dbo.FF f WITH (NOLOCK) ON a.reg_no = f.reg_no\n";
                    if (hasBedhons)
                        fromClause += "LEFT JOIN dbo.bedhons_courses bc WITH (NOLOCK) ON a.paper_code = bc.course_code\n";

                    string insertSql = string.Format(@"
                        INSERT INTO [dbo].[mtemp] (
                            [reg_no], [exm_roll], [session], [std_type], [semester], [year], [sub_code],
                            [c_code], [center], [pap_code], [col_name], [col_code],
                            [mcq_marks], [incourse_marks], [marks1], [marks2], [mcq_inc_total], [final_marks],
                            [grade], [grade_point], [sgpa], [status], [total_marks], [eb_no], [sl_no],
                            [hexcodea], [hexcodeb], [script_no], [exm_code], [id], [inc_mark], [total_sub],
                            [ems_status], [Ehex_no], [qr1], [qr2], [qr3], [form_sl], [sub_type], [ems_present_status]
                        )
                        SELECT 
                            a.reg_no,
                            a.exm_roll,
                            {0} AS [session],
                            {1} AS [std_type],
                            {2} AS [semester],
                            {3} AS [year],
                            {4} AS [sub_code],
                            a.cen_code AS [c_code],
                            {5} AS [center],
                            a.paper_code AS [pap_code],
                            {6} AS [col_name],
                            {7} AS [col_code],
                            NULL AS [mcq_marks],
                            NULL AS [incourse_marks],
                            NULL AS [marks1],
                            NULL AS [marks2],
                            NULL AS [mcq_inc_total],
                            NULL AS [final_marks],
                            NULL AS [grade],
                            NULL AS [grade_point],
                            NULL AS [sgpa],
                            NULL AS [status],
                            NULL AS [total_marks],
                            NULL AS [eb_no],
                            '' AS [sl_no],
                            NULL AS [hexcodea],
                            NULL AS [hexcodeb],
                            NULL AS [script_no],
                            {8} AS [exm_code],
                            NULL AS [id],
                            {9} AS [inc_mark],
                            {10} AS [total_sub],
                            NULL AS [ems_status],
                            NULL AS [Ehex_no],
                            CASE WHEN a.script_qr = 'null' OR a.script_qr IS NULL THEN '' ELSE a.script_qr END AS [qr1],
                            NULL AS [qr2],
                            NULL AS [qr3],
                            CASE WHEN a.script_sl = 'null' OR a.script_sl IS NULL THEN '' ELSE a.script_sl END AS [form_sl],
                            {11} AS [sub_type],
                            'P' AS [ems_present_status]
                        {12};",
                        sessionExpr,
                        stdTypeExpr,
                        semesterExpr,
                        yearExpr,
                        subCodeExpr,
                        centerExpr,
                        colNameExpr,
                        colCodeExpr,
                        exmCodeExpr,
                        incMarkExpr,
                        totalSubExpr,
                        subTypeExpr,
                        fromClause);

                    using (SqlCommand insCmd = new SqlCommand(insertSql, con))
                    {
                        insCmd.CommandTimeout = 300;
                        result.RowsInserted = insCmd.ExecuteNonQuery();
                    }

                    result.Details.Add(string.Format("Populated {0:N0} records from 'attandance' into 'mtemp'.", result.RowsInserted));

                    // 6. Create performance indexes on mtemp
                    try
                    {
                        string idxSql = @"
                            CREATE NONCLUSTERED INDEX [IX_mtemp_reg_pap] ON [dbo].[mtemp] ([reg_no], [pap_code]);
                            CREATE NONCLUSTERED INDEX [IX_mtemp_qr1] ON [dbo].[mtemp] ([qr1]);";
                        using (SqlCommand idxCmd = new SqlCommand(idxSql, con))
                        {
                            idxCmd.CommandTimeout = 120;
                            idxCmd.ExecuteNonQuery();
                        }
                        result.Details.Add("Created lookup indexes [IX_mtemp_reg_pap] and [IX_mtemp_qr1].");
                    }
                    catch (Exception exIdx)
                    {
                        result.Details.Add("Notice on index creation: " + exIdx.Message);
                    }

                    result.Success = true;
                    result.Message = string.Format("Successfully created 'mtemp' table and imported {0:N0} records from 'attandance'.", result.RowsInserted);
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error creating 'mtemp' from 'attandance': " + ex.Message;
                result.Details.Add("Exception details: " + ex.ToString());
            }

            return result;
        }

        private static string mtempConfigFilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "mtemp_connections.config");

        public static string GetMtempOpConnection(string key, string fallback = null)
        {
            try
            {
                if (System.IO.File.Exists(mtempConfigFilePath))
                {
                    string[] lines = System.IO.File.ReadAllLines(mtempConfigFilePath);
                    foreach (string line in lines)
                    {
                        if (string.IsNullOrWhiteSpace(line)) continue;
                        int eqIdx = line.IndexOf('=');
                        if (eqIdx > 0)
                        {
                            string k = line.Substring(0, eqIdx).Trim();
                            string v = line.Substring(eqIdx + 1).Trim();
                            if (string.Equals(k, key, StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(v))
                            {
                                return v;
                            }
                        }
                    }
                }
            }
            catch { }
            return fallback ?? GetConnectionString();
        }

        public static bool SaveMtempOpConnection(string key, string value)
        {
            try
            {
                Dictionary<string, string> dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                if (System.IO.File.Exists(mtempConfigFilePath))
                {
                    string[] lines = System.IO.File.ReadAllLines(mtempConfigFilePath);
                    foreach (string line in lines)
                    {
                        if (string.IsNullOrWhiteSpace(line)) continue;
                        int eqIdx = line.IndexOf('=');
                        if (eqIdx > 0)
                        {
                            string k = line.Substring(0, eqIdx).Trim();
                            string v = line.Substring(eqIdx + 1).Trim();
                            dict[k] = v;
                        }
                    }
                }

                dict[key] = value ?? "";

                List<string> outLines = new List<string>();
                foreach (KeyValuePair<string, string> kvp in dict)
                {
                    outLines.Add(kvp.Key + "=" + kvp.Value);
                }
                System.IO.File.WriteAllLines(mtempConfigFilePath, outLines.ToArray());
                return true;
            }
            catch { return false; }
        }

        public static MtempCreationResult UpdateIncourseFromFF(string connectionString = null)
        {
            MtempCreationResult result = new MtempCreationResult();
            string connStr = connectionString ?? GetConnectionString();

            try
            {
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    con.Open();

                    // 1. Check if 'mtemp' exists
                    using (SqlCommand chkCmd = new SqlCommand("SELECT COUNT(*) FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[mtemp]') AND type in (N'U')", con))
                    {
                        if (Convert.ToInt32(chkCmd.ExecuteScalar()) == 0)
                        {
                            result.Success = false;
                            result.Message = "Target table 'mtemp' does not exist in the database.";
                            result.Details.Add("Please run 'Create mtemp from FF' first.");
                            return result;
                        }
                    }

                    // 2. Check if 'FF' exists
                    using (SqlCommand chkFF = new SqlCommand("SELECT COUNT(*) FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FF]') AND type in (N'U')", con))
                    {
                        if (Convert.ToInt32(chkFF.ExecuteScalar()) == 0)
                        {
                            result.Success = false;
                            result.Message = "Source table 'FF' does not exist in the database.";
                            return result;
                        }
                    }

                    // 3. Ensure columns inc_mark and total_sub exist on FF
                    string alterFF = @"
                        IF COL_LENGTH('dbo.FF', 'inc_mark') IS NULL
                            ALTER TABLE [dbo].[FF] ADD [inc_mark] VARCHAR(255) NULL;
                        IF COL_LENGTH('dbo.FF', 'total_sub') IS NULL
                            ALTER TABLE [dbo].[FF] ADD [total_sub] VARCHAR(255) NULL;";
                    using (SqlCommand alterCmd = new SqlCommand(alterFF, con))
                    {
                        alterCmd.ExecuteNonQuery();
                    }

                    // 4. Ensure split table BEDHONS_5685_ff_split_data_24_NEW exists and is cleared
                    string prepSplitTable = @"
                        IF OBJECT_ID(N'dbo.BEDHONS_5685_ff_split_data_24_NEW', N'U') IS NULL
                        BEGIN
                            CREATE TABLE [dbo].[BEDHONS_5685_ff_split_data_24_NEW] (
                                [reg_no] varchar(255) NULL,
                                [inc_mark] varchar(255) NULL,
                                [sif_sub] varchar(255) NULL,
                                [std_type] varchar(1) NULL
                            );
                        END
                        ELSE
                        BEGIN
                            DELETE FROM [dbo].[BEDHONS_5685_ff_split_data_24_NEW];
                        END;";
                    using (SqlCommand prepCmd = new SqlCommand(prepSplitTable, con))
                    {
                        prepCmd.ExecuteNonQuery();
                    }
                    result.Details.Add("Prepared staging table [BEDHONS_5685_ff_split_data_24_NEW].");

                    // 5. Populate split table
                    string splitInsertSql = @"
                        ;WITH S AS (
                          SELECT reg_no, inc_mark, total_sub, STD_TYPE
                          FROM dbo.FF WITH (NOLOCK)
                          WHERE STD_TYPE <> '3' AND inc_mark IS NOT NULL AND total_sub IS NOT NULL
                        )
                        INSERT INTO dbo.BEDHONS_5685_ff_split_data_24_NEW (reg_no, inc_mark, sif_sub, std_type)
                        SELECT reg_no,
                               LTRIM(RTRIM(SUBSTRING(inc_mark,  2, 2))) AS inc_mark,
                               LTRIM(RTRIM(SUBSTRING(total_sub, 1, 7))) AS sif_sub,
                               STD_TYPE
                        FROM S
                        UNION ALL
                        SELECT reg_no,
                               LTRIM(RTRIM(SUBSTRING(inc_mark,  6, 2))),
                               LTRIM(RTRIM(SUBSTRING(total_sub, 8, 7))),
                               STD_TYPE
                        FROM S
                        UNION ALL
                        SELECT reg_no,
                               LTRIM(RTRIM(SUBSTRING(inc_mark, 10, 2))),
                               LTRIM(RTRIM(SUBSTRING(total_sub,16, 7))),
                               STD_TYPE
                        FROM S
                        UNION ALL
                        SELECT reg_no,
                               LTRIM(RTRIM(SUBSTRING(inc_mark, 14, 2))),
                               LTRIM(RTRIM(SUBSTRING(total_sub,24, 7))),
                               STD_TYPE
                        FROM S
                        UNION ALL
                        SELECT reg_no,
                               LTRIM(RTRIM(SUBSTRING(inc_mark, 18, 2))),
                               LTRIM(RTRIM(SUBSTRING(total_sub,32, 7))),
                               STD_TYPE
                        FROM S;";

                    using (SqlCommand insSplit = new SqlCommand(splitInsertSql, con))
                    {
                        insSplit.CommandTimeout = 180;
                        int splitRows = insSplit.ExecuteNonQuery();
                        result.Details.Add(string.Format("Extracted {0:N0} split marks into staging table.", splitRows));
                    }

                    // 6. Update mtemp.incourse_marks
                    string updateMtempSql = @"
                        UPDATE t1
                        SET t1.incourse_marks = CASE WHEN ISNUMERIC(t2.inc_mark) = 1 THEN CAST(t2.inc_mark AS SMALLINT) ELSE NULL END
                        FROM dbo.mtemp t1
                        INNER JOIN dbo.BEDHONS_5685_ff_split_data_24_NEW t2
                        ON t1.reg_no = t2.reg_no AND t1.pap_code = t2.sif_sub;";

                    using (SqlCommand updateCmd = new SqlCommand(updateMtempSql, con))
                    {
                        updateCmd.CommandTimeout = 180;
                        result.RowsInserted = updateCmd.ExecuteNonQuery();
                    }

                    result.Success = true;
                    result.Message = string.Format("Successfully updated incourse marks for {0:N0} records in 'mtemp'.", result.RowsInserted);
                    result.Details.Add(string.Format("Matched and updated {0:N0} mtemp entries from FF split data.", result.RowsInserted));
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error updating incourse marks from FF: " + ex.Message;
                result.Details.Add("Exception details: " + ex.ToString());
            }

            return result;
        }

        public static MtempCreationResult UpdateType3IncourseFromExamineeCourse(string mtempConnStr = null, string examineeConnStr = null)
        {
            MtempCreationResult result = new MtempCreationResult();
            string mConnStr = mtempConnStr ?? GetConnectionString();
            string exConnStr = string.IsNullOrEmpty(examineeConnStr) ? mConnStr : examineeConnStr;

            try
            {
                SqlConnectionStringBuilder bMtemp = new SqlConnectionStringBuilder(mConnStr);
                SqlConnectionStringBuilder bExam = new SqlConnectionStringBuilder(exConnStr);

                bool isSameServer = string.Equals(bMtemp.DataSource.Trim(), bExam.DataSource.Trim(), StringComparison.OrdinalIgnoreCase);

                using (SqlConnection conMtemp = new SqlConnection(mConnStr))
                {
                    conMtemp.Open();

                    // 1. Verify mtemp exists
                    using (SqlCommand chk = new SqlCommand("SELECT COUNT(*) FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[mtemp]') AND type in (N'U')", conMtemp))
                    {
                        if (Convert.ToInt32(chk.ExecuteScalar()) == 0)
                        {
                            result.Success = false;
                            result.Message = "Target table 'mtemp' does not exist in database '" + bMtemp.InitialCatalog + "'.";
                            return result;
                        }
                    }

                    if (isSameServer)
                    {
                        // Direct server execution
                        string examTableRef = string.Equals(bMtemp.InitialCatalog.Trim(), bExam.InitialCatalog.Trim(), StringComparison.OrdinalIgnoreCase)
                            ? "[dbo].[examinee_course]"
                            : "[" + bExam.InitialCatalog + "].[dbo].[examinee_course]";

                        string updateSql = string.Format(@"
                            UPDATE T1 
                            SET T1.incourse_marks = CASE WHEN ISNUMERIC(T2.incourse_marks) = 1 THEN CAST(T2.incourse_marks AS SMALLINT) ELSE NULL END
                            FROM dbo.mtemp AS T1
                            INNER JOIN {0} AS T2 WITH (NOLOCK)
                            ON T1.reg_no = T2.reg_no AND T1.pap_code = T2.course_code AND T1.semester = T2.semester AND T1.std_type = '3';",
                            examTableRef);

                        using (SqlCommand cmd = new SqlCommand(updateSql, conMtemp))
                        {
                            cmd.CommandTimeout = 180;
                            result.RowsInserted = cmd.ExecuteNonQuery();
                        }

                        result.Success = true;
                        result.Message = string.Format("Successfully updated Type 3 incourse marks for {0:N0} records from examinee_course.", result.RowsInserted);
                        result.Details.Add(string.Format("Executed cross-database join on server '{0}' with table '{1}'.", bMtemp.DataSource, examTableRef));
                    }
                    else
                    {
                        // Separate server connection: load matched records into temp table
                        using (SqlConnection conExam = new SqlConnection(exConnStr))
                        {
                            conExam.Open();
                            string selectSql = @"
                                SELECT reg_no, course_code, semester, incourse_marks 
                                FROM dbo.examinee_course WITH (NOLOCK) 
                                WHERE incourse_marks IS NOT NULL AND LTRIM(RTRIM(incourse_marks)) <> ''";

                            System.Data.DataTable dt = new System.Data.DataTable();
                            using (SqlCommand selCmd = new SqlCommand(selectSql, conExam))
                            using (SqlDataAdapter da = new SqlDataAdapter(selCmd))
                            {
                                da.Fill(dt);
                            }

                            // Temp table on target
                            string createTmp = @"
                                CREATE TABLE #tmp_examinee_course (
                                    reg_no varchar(255),
                                    course_code varchar(255),
                                    semester varchar(10),
                                    incourse_marks varchar(255)
                                );";
                            using (SqlCommand createCmd = new SqlCommand(createTmp, conMtemp))
                            {
                                createCmd.ExecuteNonQuery();
                            }

                            using (SqlBulkCopy bcp = new SqlBulkCopy(conMtemp))
                            {
                                bcp.DestinationTableName = "#tmp_examinee_course";
                                bcp.WriteToServer(dt);
                            }

                            string updateSql = @"
                                UPDATE T1 
                                SET T1.incourse_marks = CASE WHEN ISNUMERIC(T2.incourse_marks) = 1 THEN CAST(T2.incourse_marks AS SMALLINT) ELSE NULL END
                                FROM dbo.mtemp AS T1
                                INNER JOIN #tmp_examinee_course AS T2
                                ON T1.reg_no = T2.reg_no AND T1.pap_code = T2.course_code AND T1.semester = T2.semester AND T1.std_type = '3';
                                DROP TABLE #tmp_examinee_course;";

                            using (SqlCommand cmd = new SqlCommand(updateSql, conMtemp))
                            {
                                cmd.CommandTimeout = 180;
                                result.RowsInserted = cmd.ExecuteNonQuery();
                            }

                            result.Success = true;
                            result.Message = string.Format("Successfully transferred and updated Type 3 incourse marks for {0:N0} records.", result.RowsInserted);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error updating Type 3 incourse marks: " + ex.Message;
                result.Details.Add("Exception details: " + ex.ToString());
            }

            return result;
        }

        public static MtempCreationResult UpdateMarksFromHsolving(string mtempConnStr = null, string hsolvingConnStr = null)
        {
            MtempCreationResult result = new MtempCreationResult();
            string mConnStr = mtempConnStr ?? GetConnectionString();
            string hConnStr = string.IsNullOrEmpty(hsolvingConnStr) ? mConnStr : hsolvingConnStr;

            try
            {
                SqlConnectionStringBuilder bMtemp = new SqlConnectionStringBuilder(mConnStr);
                SqlConnectionStringBuilder bHsolv = new SqlConnectionStringBuilder(hConnStr);

                bool isSameServer = string.Equals(bMtemp.DataSource.Trim(), bHsolv.DataSource.Trim(), StringComparison.OrdinalIgnoreCase);

                using (SqlConnection conMtemp = new SqlConnection(mConnStr))
                {
                    conMtemp.Open();

                    // 1. Verify mtemp exists
                    using (SqlCommand chk = new SqlCommand("SELECT COUNT(*) FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[mtemp]') AND type in (N'U')", conMtemp))
                    {
                        if (Convert.ToInt32(chk.ExecuteScalar()) == 0)
                        {
                            result.Success = false;
                            result.Message = "Target table 'mtemp' does not exist in database '" + bMtemp.InitialCatalog + "'.";
                            return result;
                        }
                    }

                    if (isSameServer)
                    {
                        string hsolvTableRef = string.Equals(bMtemp.InitialCatalog.Trim(), bHsolv.InitialCatalog.Trim(), StringComparison.OrdinalIgnoreCase)
                            ? "[dbo].[H_solving]"
                            : "[" + bHsolv.InitialCatalog + "].[dbo].[H_solving]";

                        string updateSql = string.Format(@"
                            UPDATE t1
                            SET t1.marks1 = CASE WHEN ISNUMERIC(h.marks) = 1 THEN CAST(h.marks AS SMALLINT) ELSE t1.marks1 END,
                                t1.eb_no = ISNULL(NULLIF(h.eb_no, ''), t1.eb_no),
                                t1.sl_no = ISNULL(NULLIF(h.sl_no, ''), t1.sl_no),
                                t1.hexcodea = ISNULL(NULLIF(h.hexcodea, ''), t1.hexcodea),
                                t1.hexcodeb = ISNULL(NULLIF(h.hexcodeb, ''), t1.hexcodeb),
                                t1.Ehex_no = ISNULL(NULLIF(h.Ehex_no, ''), t1.Ehex_no),
                                t1.qr2 = ISNULL(NULLIF(h.qr2, ''), t1.qr2),
                                t1.qr3 = ISNULL(NULLIF(h.qr3, ''), t1.qr3)
                            FROM dbo.mtemp t1
                            INNER JOIN {0} h WITH (NOLOCK)
                            ON (t1.qr1 = h.qr1 AND t1.qr1 IS NOT NULL AND t1.qr1 <> '' AND t1.qr1 <> 'null')
                               OR (t1.form_sl = h.form_sl AND t1.form_sl IS NOT NULL AND t1.form_sl <> '' AND t1.form_sl <> 'null')
                               OR (t1.reg_no = h.reg_no AND t1.pap_code = h.pap_code AND t1.reg_no IS NOT NULL AND t1.reg_no <> '');",
                            hsolvTableRef);

                        using (SqlCommand cmd = new SqlCommand(updateSql, conMtemp))
                        {
                            cmd.CommandTimeout = 300;
                            result.RowsInserted = cmd.ExecuteNonQuery();
                        }

                        result.Success = true;
                        result.Message = string.Format("Successfully updated marks from H_solving for {0:N0} records in 'mtemp'.", result.RowsInserted);
                        result.Details.Add(string.Format("Matched on qr1, form_sl, and reg_no+pap_code with '{0}'.", hsolvTableRef));
                    }
                    else
                    {
                        // Separate server: query H_solving and bulk copy to temp table
                        using (SqlConnection conH = new SqlConnection(hConnStr))
                        {
                            conH.Open();
                            string selSql = @"
                                SELECT qr1, form_sl, reg_no, pap_code, marks, eb_no, sl_no, hexcodea, hexcodeb, Ehex_no, qr2, qr3
                                FROM dbo.H_solving WITH (NOLOCK)
                                WHERE (marks IS NOT NULL AND LTRIM(RTRIM(marks)) <> '')
                                   OR (hexcodea IS NOT NULL AND LTRIM(RTRIM(hexcodea)) <> '')";

                            System.Data.DataTable dt = new System.Data.DataTable();
                            using (SqlCommand cmdSel = new SqlCommand(selSql, conH))
                            using (SqlDataAdapter da = new SqlDataAdapter(cmdSel))
                            {
                                da.Fill(dt);
                            }

                            string crtTmp = @"
                                CREATE TABLE #tmp_h_solving (
                                    qr1 varchar(255), form_sl varchar(255), reg_no varchar(255), pap_code varchar(255),
                                    marks varchar(50), eb_no varchar(50), sl_no varchar(50), hexcodea varchar(50),
                                    hexcodeb varchar(50), Ehex_no varchar(50), qr2 varchar(255), qr3 varchar(255)
                                );";
                            using (SqlCommand cmdCrt = new SqlCommand(crtTmp, conMtemp))
                            {
                                cmdCrt.ExecuteNonQuery();
                            }

                            using (SqlBulkCopy bcp = new SqlBulkCopy(conMtemp))
                            {
                                bcp.DestinationTableName = "#tmp_h_solving";
                                bcp.WriteToServer(dt);
                            }

                            string updSql = @"
                                UPDATE t1
                                SET t1.marks1 = CASE WHEN ISNUMERIC(h.marks) = 1 THEN CAST(h.marks AS SMALLINT) ELSE t1.marks1 END,
                                    t1.eb_no = ISNULL(NULLIF(h.eb_no, ''), t1.eb_no),
                                    t1.sl_no = ISNULL(NULLIF(h.sl_no, ''), t1.sl_no),
                                    t1.hexcodea = ISNULL(NULLIF(h.hexcodea, ''), t1.hexcodea),
                                    t1.hexcodeb = ISNULL(NULLIF(h.hexcodeb, ''), t1.hexcodeb),
                                    t1.Ehex_no = ISNULL(NULLIF(h.Ehex_no, ''), t1.Ehex_no),
                                    t1.qr2 = ISNULL(NULLIF(h.qr2, ''), t1.qr2),
                                    t1.qr3 = ISNULL(NULLIF(h.qr3, ''), t1.qr3)
                                FROM dbo.mtemp t1
                                INNER JOIN #tmp_h_solving h
                                ON (t1.qr1 = h.qr1 AND t1.qr1 IS NOT NULL AND t1.qr1 <> '' AND t1.qr1 <> 'null')
                                   OR (t1.form_sl = h.form_sl AND t1.form_sl IS NOT NULL AND t1.form_sl <> '' AND t1.form_sl <> 'null')
                                   OR (t1.reg_no = h.reg_no AND t1.pap_code = h.pap_code AND t1.reg_no IS NOT NULL AND t1.reg_no <> '');
                                DROP TABLE #tmp_h_solving;";

                            using (SqlCommand cmdUpd = new SqlCommand(updSql, conMtemp))
                            {
                                cmdUpd.CommandTimeout = 300;
                                result.RowsInserted = cmdUpd.ExecuteNonQuery();
                            }

                            result.Success = true;
                            result.Message = string.Format("Successfully updated marks from remote H_solving for {0:N0} records.", result.RowsInserted);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error updating marks from H_solving: " + ex.Message;
                result.Details.Add("Exception details: " + ex.ToString());
            }

            return result;
        }

        // ==================== OPERATION 5: UPDATE MCQ MARKS ====================
        public static MtempCreationResult UpdateMcqMarks(string mtempConnStr = null, string mcqConnStr = null)
        {
            MtempCreationResult result = new MtempCreationResult();
            string mConnStr = mtempConnStr ?? GetConnectionString();
            string mcqConn = string.IsNullOrEmpty(mcqConnStr) ? mConnStr : mcqConnStr;

            try
            {
                SqlConnectionStringBuilder bMtemp = new SqlConnectionStringBuilder(mConnStr);
                SqlConnectionStringBuilder bMcq = new SqlConnectionStringBuilder(mcqConn);

                bool isSameServer = string.Equals(bMtemp.DataSource.Trim(), bMcq.DataSource.Trim(), StringComparison.OrdinalIgnoreCase);

                using (SqlConnection conMtemp = new SqlConnection(mConnStr))
                {
                    conMtemp.Open();

                    // Check mtemp table exists
                    using (SqlCommand chk = new SqlCommand("SELECT COUNT(*) FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[mtemp]') AND type in (N'U')", conMtemp))
                    {
                        if (Convert.ToInt32(chk.ExecuteScalar()) == 0)
                        {
                            result.Success = false;
                            result.Message = "Target table 'mtemp' does not exist in database '" + bMtemp.InitialCatalog + "'.";
                            return result;
                        }
                    }

                    if (isSameServer)
                    {
                        string mcqTableRef = string.Equals(bMtemp.InitialCatalog.Trim(), bMcq.InitialCatalog.Trim(), StringComparison.OrdinalIgnoreCase)
                            ? "[dbo].[mcq]"
                            : "[" + bMcq.InitialCatalog + "].[dbo].[mcq]";

                        string sql = string.Format(@"
                            UPDATE t1
                            SET t1.mcq_marks = CASE WHEN ISNUMERIC(t2.mark) = 1 THEN CAST(t2.mark AS SMALLINT) ELSE NULL END
                            FROM dbo.mtemp t1
                            INNER JOIN {0} t2 WITH (NOLOCK)
                            ON t1.reg_no = t2.reg_no AND t1.pap_code = t2.course_code;",
                            mcqTableRef);

                        using (SqlCommand cmd = new SqlCommand(sql, conMtemp))
                        {
                            cmd.CommandTimeout = 180;
                            result.RowsInserted = cmd.ExecuteNonQuery();
                        }

                        result.Success = true;
                        result.Message = string.Format("Successfully updated MCQ marks for {0:N0} records in 'mtemp'.", result.RowsInserted);
                        result.Details.Add(string.Format("Matched on reg_no and pap_code=course_code with '{0}'.", mcqTableRef));
                    }
                    else
                    {
                        using (SqlConnection conMcq = new SqlConnection(mcqConn))
                        {
                            conMcq.Open();
                            string selSql = "SELECT reg_no, course_code, mark FROM dbo.mcq WITH (NOLOCK) WHERE mark IS NOT NULL AND LTRIM(RTRIM(mark)) <> ''";
                            DataTable dt = new DataTable();
                            using (SqlCommand cmdSel = new SqlCommand(selSql, conMcq))
                            using (SqlDataAdapter da = new SqlDataAdapter(cmdSel))
                            {
                                da.Fill(dt);
                            }

                            string crtTmp = @"
                                CREATE TABLE #tmp_mcq (
                                    reg_no varchar(255), course_code varchar(255), mark varchar(50)
                                );
                                CREATE INDEX IX_tmp_mcq ON #tmp_mcq(reg_no, course_code);";
                            using (SqlCommand cmdCrt = new SqlCommand(crtTmp, conMtemp))
                            {
                                cmdCrt.ExecuteNonQuery();
                            }

                            using (SqlBulkCopy bcp = new SqlBulkCopy(conMtemp))
                            {
                                bcp.DestinationTableName = "#tmp_mcq";
                                bcp.WriteToServer(dt);
                            }

                            string updSql = @"
                                UPDATE t1
                                SET t1.mcq_marks = CASE WHEN ISNUMERIC(t2.mark) = 1 THEN CAST(t2.mark AS SMALLINT) ELSE NULL END
                                FROM dbo.mtemp t1
                                INNER JOIN #tmp_mcq t2
                                ON t1.reg_no = t2.reg_no AND t1.pap_code = t2.course_code;
                                DROP TABLE #tmp_mcq;";

                            using (SqlCommand cmdUpd = new SqlCommand(updSql, conMtemp))
                            {
                                cmdUpd.CommandTimeout = 180;
                                result.RowsInserted = cmdUpd.ExecuteNonQuery();
                            }

                            result.Success = true;
                            result.Message = string.Format("Successfully updated MCQ marks for {0:N0} records from remote mcq database.", result.RowsInserted);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error updating MCQ marks: " + ex.Message;
                result.Details.Add("Exception details: " + ex.ToString());
            }

            return result;
        }

        // ==================== OPERATION 6: CALCULATE MARKS & GRADES ====================
        public static MtempCreationResult CalculateMtempMarksAndGrades(string mtempConnStr = null)
        {
            MtempCreationResult result = new MtempCreationResult();
            string connStr = mtempConnStr ?? GetConnectionString();

            try
            {
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    con.Open();

                    using (SqlCommand chk = new SqlCommand("SELECT COUNT(*) FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[mtemp]') AND type in (N'U')", con))
                    {
                        if (Convert.ToInt32(chk.ExecuteScalar()) == 0)
                        {
                            result.Success = false;
                            result.Message = "Target table 'mtemp' does not exist in the database.";
                            return result;
                        }
                    }

                    // Step 1: Calculate Total Marks
                    string sqlMarks = @"
                        UPDATE dbo.mtemp SET mcq_marks = 0 WHERE mcq_marks IS NULL;
                        UPDATE dbo.mtemp SET marks1 = -999 WHERE marks1 IS NULL;
                        UPDATE dbo.mtemp SET mcq_marks = marks1 WHERE marks1 = -999;
                        UPDATE dbo.mtemp SET final_marks = marks1;
                        UPDATE dbo.mtemp SET final_marks = CASE WHEN ISNUMERIC(marks2)=1 THEN CAST(marks2 AS smallint) ELSE final_marks END 
                        WHERE marks2 IS NOT NULL AND LTRIM(RTRIM(marks2)) <> '';
                        UPDATE dbo.mtemp SET final_marks = marks1 WHERE final_marks IS NULL;
                        UPDATE dbo.mtemp SET final_marks = -888 WHERE marks1 = -888;
                        UPDATE dbo.mtemp SET final_marks = -999 WHERE marks1 = -999;
                        UPDATE dbo.mtemp SET mcq_inc_total = ISNULL(mcq_marks, 0) + ISNULL(incourse_marks, 0);
                        UPDATE dbo.mtemp SET total_marks = CAST((ISNULL(final_marks, 0) + ISNULL(mcq_inc_total, 0)) AS varchar(10));
                        UPDATE dbo.mtemp SET final_marks = marks1, total_marks = CAST(marks1 AS varchar(10)) WHERE marks1 = -999;
                        UPDATE dbo.mtemp SET final_marks = marks1, total_marks = CAST(marks1 AS varchar(10)) WHERE marks1 = -888;";

                    using (SqlCommand cmdMarks = new SqlCommand(sqlMarks, con))
                    {
                        cmdMarks.CommandTimeout = 300;
                        cmdMarks.ExecuteNonQuery();
                    }
                    result.Details.Add("Successfully recalculated marks1, final_marks, mcq_inc_total, and total_marks.");

                    // Step 2: Scale Non-100 full marks and compute letter grades & grade points
                    string sqlGrades = @"
                        -- Scale Non-100 full marks courses to 100-mark equivalent percentage
                        UPDATE dbo.mtemp
                        SET total_marks = CAST(CAST(CEILING(CAST(total_marks AS float) / 
                            CASE 
                                WHEN pap_code IN ('000106','000107','510804','510808','510810','530826','530830') THEN 50.0
                                WHEN pap_code = '810313' THEN 75.0
                                WHEN pap_code = '810314' THEN 25.0
                                WHEN pap_code = '810338' THEN 200.0
                                ELSE 100.0
                            END * 100.0) AS int) AS varchar(10))
                        WHERE pap_code IN ('530826','530830','000106','000107','510804','510808','510810','810313','810314','810338')
                          AND ISNUMERIC(total_marks) = 1
                          AND CAST(total_marks AS int) >= 0;

                        -- Calculate Letter Grades and Grade Points
                        UPDATE dbo.mtemp
                        SET grade = CASE 
                                WHEN ISNUMERIC(total_marks) = 1 AND CAST(total_marks AS int) >= 80 AND CAST(total_marks AS int) <= 100 THEN 'A+'
                                WHEN ISNUMERIC(total_marks) = 1 AND CAST(total_marks AS int) >= 75 AND CAST(total_marks AS int) <= 79 THEN 'A'
                                WHEN ISNUMERIC(total_marks) = 1 AND CAST(total_marks AS int) >= 70 AND CAST(total_marks AS int) <= 74 THEN 'A-'
                                WHEN ISNUMERIC(total_marks) = 1 AND CAST(total_marks AS int) >= 65 AND CAST(total_marks AS int) <= 69 THEN 'B+'
                                WHEN ISNUMERIC(total_marks) = 1 AND CAST(total_marks AS int) >= 60 AND CAST(total_marks AS int) <= 64 THEN 'B'
                                WHEN ISNUMERIC(total_marks) = 1 AND CAST(total_marks AS int) >= 55 AND CAST(total_marks AS int) <= 59 THEN 'B-'
                                WHEN ISNUMERIC(total_marks) = 1 AND CAST(total_marks AS int) >= 50 AND CAST(total_marks AS int) <= 54 THEN 'C+'
                                WHEN ISNUMERIC(total_marks) = 1 AND CAST(total_marks AS int) >= 45 AND CAST(total_marks AS int) <= 49 THEN 'C'
                                WHEN ISNUMERIC(total_marks) = 1 AND CAST(total_marks AS int) >= 40 AND CAST(total_marks AS int) <= 44 THEN 'D'
                                ELSE 'F'
                            END,
                            grade_point = CASE 
                                WHEN ISNUMERIC(total_marks) = 1 AND CAST(total_marks AS int) >= 80 AND CAST(total_marks AS int) <= 100 THEN 4.00
                                WHEN ISNUMERIC(total_marks) = 1 AND CAST(total_marks AS int) >= 75 AND CAST(total_marks AS int) <= 79 THEN 3.75
                                WHEN ISNUMERIC(total_marks) = 1 AND CAST(total_marks AS int) >= 70 AND CAST(total_marks AS int) <= 74 THEN 3.50
                                WHEN ISNUMERIC(total_marks) = 1 AND CAST(total_marks AS int) >= 65 AND CAST(total_marks AS int) <= 69 THEN 3.25
                                WHEN ISNUMERIC(total_marks) = 1 AND CAST(total_marks AS int) >= 60 AND CAST(total_marks AS int) <= 64 THEN 3.00
                                WHEN ISNUMERIC(total_marks) = 1 AND CAST(total_marks AS int) >= 55 AND CAST(total_marks AS int) <= 59 THEN 2.75
                                WHEN ISNUMERIC(total_marks) = 1 AND CAST(total_marks AS int) >= 50 AND CAST(total_marks AS int) <= 54 THEN 2.50
                                WHEN ISNUMERIC(total_marks) = 1 AND CAST(total_marks AS int) >= 45 AND CAST(total_marks AS int) <= 49 THEN 2.25
                                WHEN ISNUMERIC(total_marks) = 1 AND CAST(total_marks AS int) >= 40 AND CAST(total_marks AS int) <= 44 THEN 2.00
                                ELSE 0.00
                            END;

                        -- Clean up negative / absent marks
                        UPDATE dbo.mtemp
                        SET grade = 'F', grade_point = 0.00
                        WHERE marks1 IN (-999, -888) OR total_marks IN ('-999', '-888');";

                    using (SqlCommand cmdGrades = new SqlCommand(sqlGrades, con))
                    {
                        cmdGrades.CommandTimeout = 300;
                        result.RowsInserted = cmdGrades.ExecuteNonQuery();
                    }
                    result.Details.Add("Calculated letter grades (A+ to F) and grade points (4.00 to 0.00).");

                    result.Success = true;
                    result.Message = "Successfully calculated total marks, scaled scores, grades, and grade points for 'mtemp'.";
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error calculating marks and grades: " + ex.Message;
                result.Details.Add("Exception details: " + ex.ToString());
            }

            return result;
        }

        // ==================== OPERATION 7: ABSENT WANTING & PRINTABLE VERIFICATION ====================
        public static MtempCreationResult ProcessAbsentWantingAndExport(string mtempConnStr, out DataTable dtWanting, out string htmlReportPath)
        {
            MtempCreationResult result = new MtempCreationResult();
            dtWanting = new DataTable();
            htmlReportPath = null;
            string connStr = mtempConnStr ?? GetConnectionString();

            try
            {
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    con.Open();

                    // 1. Update students with > 4 absents to status = 'abs', marks1 = -999
                    string sqlUpdateAbs = @"
                        UPDATE t
                        SET status = 'abs', marks1 = -999
                        FROM dbo.mtemp t
                        WHERE (t.marks1 = -999 OR t.marks1 IS NULL)
                          AND t.reg_no IN (
                                SELECT reg_no
                                FROM dbo.mtemp
                                WHERE marks1 = -999 OR marks1 IS NULL
                                GROUP BY reg_no 
                                HAVING COUNT(*) > 4
                          );";

                    int updatedFullAbs = 0;
                    using (SqlCommand cmdAbs = new SqlCommand(sqlUpdateAbs, con))
                    {
                        cmdAbs.CommandTimeout = 180;
                        updatedFullAbs = cmdAbs.ExecuteNonQuery();
                    }
                    result.Details.Add(string.Format("Updated {0:N0} records to full absent status ('abs') for examinees absent in >4 subjects.", updatedFullAbs));

                    // 2. Populate / recreate dbo.ABSENT_wanting
                    string sqlCreateWantingTable = @"
                        IF OBJECT_ID(N'dbo.ABSENT_wanting', N'U') IS NOT NULL
                            DROP TABLE dbo.ABSENT_wanting;

                        SELECT 
                            ROW_NUMBER() OVER (ORDER BY col_code, c_code, reg_no, pap_code) AS [no],
                            col_code,
                            c_code,
                            reg_no,
                            pap_code,
                            '           ' AS [SL_no]
                        INTO dbo.ABSENT_wanting
                        FROM dbo.mtemp
                        WHERE (marks1 = -999 OR marks1 IS NULL)
                          AND (status != 'abs' OR status IS NULL)
                        GROUP BY col_code, c_code, reg_no, pap_code;

                        SELECT * FROM dbo.ABSENT_wanting ORDER BY [no];";

                    using (SqlCommand cmdWanting = new SqlCommand(sqlCreateWantingTable, con))
                    {
                        cmdWanting.CommandTimeout = 180;
                        using (SqlDataAdapter da = new SqlDataAdapter(cmdWanting))
                        {
                            da.Fill(dtWanting);
                        }
                    }
                    result.Details.Add(string.Format("Extracted {0:N0} courses into [dbo].[ABSENT_wanting].", dtWanting.Rows.Count));

                    // 3. Generate clean printable HTML report file
                    string reportsDir = null;
                    try
                    {
                        string baseDir = string.IsNullOrEmpty(Application.StartupPath) ? AppDomain.CurrentDomain.BaseDirectory : Application.StartupPath;
                        reportsDir = System.IO.Path.Combine(baseDir, "Reports");
                        if (!System.IO.Directory.Exists(reportsDir))
                        {
                            System.IO.Directory.CreateDirectory(reportsDir);
                        }
                    }
                    catch
                    {
                        try
                        {
                            reportsDir = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "NU_Solver_Reports");
                            if (!System.IO.Directory.Exists(reportsDir))
                            {
                                System.IO.Directory.CreateDirectory(reportsDir);
                            }
                        }
                        catch
                        {
                            reportsDir = System.IO.Path.GetTempPath();
                        }
                    }

                    string fileName = string.Format("Absent_Wanting_Verification_{0:yyyyMMdd_HHmmss}.html", DateTime.Now);
                    htmlReportPath = System.IO.Path.Combine(reportsDir, fileName);

                    StringBuilder html = new StringBuilder();
                    html.AppendLine("<!DOCTYPE html>");
                    html.AppendLine("<html>");
                    html.AppendLine("<head>");
                    html.AppendLine("<meta charset='utf-8'/>");
                    html.AppendLine("<title>Absent Wanting Verification List</title>");
                    html.AppendLine("<style>");
                    html.AppendLine("  body { font-family: 'Segoe UI', Arial, sans-serif; margin: 20px; background: #fff; color: #111; }");
                    html.AppendLine("  .header { text-align: center; margin-bottom: 20px; border-bottom: 2px solid #059669; padding-bottom: 12px; }");
                    html.AppendLine("  .header h2 { margin: 0; color: #059669; font-size: 22px; }");
                    html.AppendLine("  .header h3 { margin: 4px 0; font-size: 16px; font-weight: normal; color: #475569; }");
                    html.AppendLine("  .meta { display: flex; justify-content: space-between; font-size: 13px; margin-bottom: 15px; color: #334155; }");
                    html.AppendLine("  table { width: 100%; border-collapse: collapse; font-size: 13px; margin-top: 10px; }");
                    html.AppendLine("  th, td { border: 1px solid #94a3b8; padding: 6px 10px; text-align: center; }");
                    html.AppendLine("  th { background-color: #f1f5f9; color: #0f172a; font-weight: bold; }");
                    html.AppendLine("  tr:nth-child(even) { background-color: #f8fafc; }");
                    html.AppendLine("  .sl-box { border-bottom: 1px dashed #64748b; height: 18px; display: inline-block; width: 80px; }");
                    html.AppendLine("  .btn-print { padding: 8px 18px; font-size: 14px; font-weight: bold; background: #059669; color: white; border: none; border-radius: 4px; cursor: pointer; }");
                    html.AppendLine("  @media print { .no-print { display: none; } body { margin: 0; } th { background-color: #eee !important; -webkit-print-color-adjust: exact; } }");
                    html.AppendLine("</style>");
                    html.AppendLine("</head>");
                    html.AppendLine("<body>");
                    html.AppendLine("<div class='no-print' style='margin-bottom: 15px; text-align: right;'>");
                    html.AppendLine("  <button class='btn-print' onclick='window.print()'>🖨️ Print Verification List</button>");
                    html.AppendLine("</div>");
                    html.AppendLine("<div class='header'>");
                    html.AppendLine("  <h2>NATIONAL UNIVERSITY, BANGLADESH</h2>");
                    html.AppendLine("  <h3>ABSENT WANTING VERIFICATION LIST (CONFIDENTIAL)</h3>");
                    html.AppendLine("</div>");
                    html.AppendLine("<div class='meta'>");
                    html.AppendLine(string.Format("  <div><b>Generated:</b> {0:dd-MMM-yyyy hh:mm tt}</div>", DateTime.Now));
                    html.AppendLine(string.Format("  <div><b>Total Full Absents (>4 codes):</b> {0:N0}</div>", updatedFullAbs));
                    html.AppendLine(string.Format("  <div><b>Total Wanting Courses:</b> {0:N0}</div>", dtWanting.Rows.Count));
                    html.AppendLine("</div>");
                    html.AppendLine("<table>");
                    html.AppendLine("  <thead>");
                    html.AppendLine("    <tr>");
                    html.AppendLine("      <th style='width: 60px;'>SL No</th>");
                    html.AppendLine("      <th>College Code</th>");
                    html.AppendLine("      <th>Center Code</th>");
                    html.AppendLine("      <th>Registration No</th>");
                    html.AppendLine("      <th>Paper Code</th>");
                    html.AppendLine("      <th style='width: 140px;'>Script / Form SL No</th>");
                    html.AppendLine("    </tr>");
                    html.AppendLine("  </thead>");
                    html.AppendLine("  <tbody>");

                    for (int i = 0; i < dtWanting.Rows.Count; i++)
                    {
                        DataRow r = dtWanting.Rows[i];
                        html.AppendLine("    <tr>");
                        html.AppendLine(string.Format("      <td>{0}</td>", r["no"]));
                        html.AppendLine(string.Format("      <td>{0}</td>", r["col_code"]));
                        html.AppendLine(string.Format("      <td>{0}</td>", r["c_code"]));
                        html.AppendLine(string.Format("      <td><b>{0}</b></td>", r["reg_no"]));
                        html.AppendLine(string.Format("      <td><b>{0}</b></td>", r["pap_code"]));
                        html.AppendLine("      <td><span class='sl-box'></span></td>");
                        html.AppendLine("    </tr>");
                    }

                    html.AppendLine("  </tbody>");
                    html.AppendLine("</table>");
                    html.AppendLine("<div style='margin-top: 40px; display: flex; justify-content: space-between; font-size: 13px;'>");
                    html.AppendLine("  <div>_______________________<br/>Verified By</div>");
                    html.AppendLine("  <div>_______________________<br/>Section Officer / Incharge</div>");
                    html.AppendLine("  <div>_______________________<br/>Controller of Examinations</div>");
                    html.AppendLine("</div>");
                    html.AppendLine("</body>");
                    html.AppendLine("</html>");

                    System.IO.File.WriteAllText(htmlReportPath, html.ToString(), Encoding.UTF8);
                    result.Details.Add(string.Format("Created printable absent verification report: {0}", fileName));

                    result.Success = true;
                    result.RowsInserted = dtWanting.Rows.Count;
                    result.Message = string.Format("Absent Wanting generated: {0:N0} wanting courses identified. Report saved.", dtWanting.Rows.Count);
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error processing Absent Wanting: " + ex.Message;
                result.Details.Add("Exception details: " + ex.ToString());
            }

            return result;
        }

        public static bool ExportDataTableToCsv(DataTable dt, string filePath)
        {
            try
            {
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(filePath, false, Encoding.UTF8))
                {
                    int colCount = dt.Columns.Count;
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < colCount; i++)
                    {
                        if (i > 0) sb.Append(",");
                        sb.Append(EscapeCsv(dt.Columns[i].ColumnName));
                    }
                    sw.WriteLine(sb.ToString());

                    foreach (DataRow row in dt.Rows)
                    {
                        sb.Length = 0;
                        for (int i = 0; i < colCount; i++)
                        {
                            if (i > 0) sb.Append(",");
                            sb.Append(EscapeCsv(row[i] != null ? row[i].ToString() : ""));
                        }
                        sw.WriteLine(sb.ToString());
                    }
                }
                return true;
            }
            catch { return false; }
        }
    }
}
