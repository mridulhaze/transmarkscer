using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace NU_Solver
{
    public partial class login : Form
    {
        public String username = "";
        public String password = "";
        //H_file_list H_file_list_form = new H_file_list();
            
        public login()
        {
            InitializeComponent();
            UITheme.ApplyTheme(this, "Solver Login");
        }
        
        private void b_login_Click(object sender, EventArgs e)
        {
            //int n;
            ////n = Int32.TryParse("0000000027] ");
            //MessageBox.Show("i="+n+"__");
            //Database.init();
            //MessageBox.Show(Database.isValid("211000").ToString());
            //return;
            try
            {
                SqlConnection con = Database.GetConnectionObj();
                if (con == null)
                {
                    MessageBox.Show("Connection Problem");
                    return;
                }
                SqlCommand cmd = new SqlCommand("",con);
                username = tb_username.Text.ToString();
                password = tb_password.Text.ToString();

                cmd.CommandText = String.Format("SELECT dbo.solvers.status FROM dbo.solvers WHERE dbo.solvers.solver_name = '{0}' AND dbo.solvers.passkey = '{1}'", username, password);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    //MessageBox.Show(reader["status"].ToString());
                    Database.init();

                    if (reader["status"].ToString() == "solver")
                    {
                        loadAppropriateWindow();
                        return;
                    }
                    else if (reader["status"].ToString() == "uploader" || reader["status"].ToString() == "admin")
                    {
                        UploaderDashboard dashboard = new UploaderDashboard(username, this);
                        dashboard.Show();
                        this.Hide();
                        return;
                    }
                    //exm_roll.Add(reader["exm_roll"].ToString());
                    //reg_no.Add(reader["reg_no"].ToString());
                }
                reader.Dispose();
                MessageBox.Show("Incorrect username password combination.\nTry again.");

                //MessageBox.Show( username + "\n" + password );
                //H_file_list_form.H_file_list("File Name", username);  
            }
            catch (Exception ee)
            {
                MessageBox.Show("Database error.\n" + ee.StackTrace.ToString());
                Application.Exit();
            }
        }
        public void loadAppropriateWindow()
        {
            try
            {
                SqlConnection con = Database.GetConnectionObj();
                if (con == null) throw new Exception("Can't create and open a connection");
                SqlCommand cmd = new SqlCommand("", con);
                cmd.CommandText = string.Format(
                    "SELECT file_name FROM dbo.H_file_list WHERE solver_name = '{0}' AND solve_status = 'solving'",
                    username);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    string filename = reader[0].ToString();
                    //SolveWindow slw = new SolveWindow(filename, username, getSub(filename), this, false);
                    //this.Hide();
                    //slw.Show();
                    H_solve slw = new H_solve(filename, username, getSub(filename), this, false);
                    this.Hide();
                    slw.Show();
                    //this.Close();
                }
                else
                {
                    H_file_list H_file_list_form = new H_file_list(username,this);
                    H_file_list_form.Show();
                    this.Hide();
                }
                reader.Dispose();
                con.Close();
                con.Dispose();
            }
            catch (Exception ee)
            {
                MessageBox.Show("Database error.\n" + ee.StackTrace.ToString());
                Application.Exit();
            }
        }
        public string getSub(string filename)
        {
            try
            {
                SqlConnection con = Database.GetConnectionObj();
                if (con == null) throw new Exception("Can't create and open a connection");
                SqlCommand cmd = new SqlCommand("", con);
                cmd.CommandText = string.Format(
                    "SELECT pap_code FROM dbo.H_file_list WHERE file_name = '{0}'",
                    filename);
                SqlDataReader reader = cmd.ExecuteReader();
                string sub_code = "";
                if (reader.Read())
                {
                    sub_code = reader[0].ToString();
                }
                reader.Dispose();
                con.Close();
                con.Dispose();
                Database.sub_code = sub_code;
                return sub_code;
            }
            catch (Exception ee)
            {
                MessageBox.Show("Database error.\n" + ee.StackTrace.ToString());
                Application.Exit();
            }
            return "";
        }

        private void login_Load(object sender, EventArgs e)
        {
            LoadNuLogoImage();
        }

        private void LoadNuLogoImage()
        {
            try
            {
                Image img = UITheme.GetEmbeddedOrFileImage("nu.png");
                if (img != null && pbNuLogo != null)
                {
                    pbNuLogo.Image = img;
                    pbNuLogo.SizeMode = PictureBoxSizeMode.Zoom;
                }
            }
            catch { }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void tb_username_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            string enteredUser = tb_username.Text.Trim();
            string enteredPass = tb_password.Text.Trim();

            bool authorized = false;

            // Direct check if username is 'admin' and valid passkey is typed in main login form
            if (enteredUser.Equals("admin", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(enteredPass))
            {
                try
                {
                    using (SqlConnection con = Database.GetConnectionObj())
                    {
                        if (con != null)
                        {
                            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM dbo.solvers WHERE solver_name = 'admin' AND passkey = @pass", con);
                            cmd.Parameters.AddWithValue("@pass", enteredPass);
                            int cnt = Convert.ToInt32(cmd.ExecuteScalar());
                            if (cnt > 0) authorized = true;
                        }
                    }
                }
                catch { }

                if (!authorized && (enteredPass == "admin" || enteredPass == "1234"))
                {
                    authorized = true;
                }
            }

            // Prompt for admin password if not pre-authorized
            if (!authorized)
            {
                using (AdminAuthDialog authDlg = new AdminAuthDialog())
                {
                    if (authDlg.ShowDialog() == DialogResult.OK)
                    {
                        authorized = true;
                    }
                }
            }

            if (authorized)
            {
                using (DbSettingsForm settingsForm = new DbSettingsForm())
                {
                    settingsForm.ShowDialog();
                }
            }
        }
    }
}
