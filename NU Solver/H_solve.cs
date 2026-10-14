using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace NU_Solver
{
    public partial class H_solve : Form
    {
        public string filename = "";
        public string username = "";
        public string sub_code = "";
        public string current = "";
        Form caller;
        public bool value_back = false;
        public bool error_only = false;
        //H_file_list caller;
        public DataTable dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter();
        public H_solve(string filename, string username, string sub, Form caller, bool needToSpan = false)
        {
            this.filename = filename;
            this.sub_code = sub;
            this.username = username;
            this.caller = caller;
            InitializeComponent();
            UITheme.ApplyTheme(this, "Solving: " + filename + " (User: " + username + ")");
            CustomiseGridView();
            this.Text = "NU Solver - " + filename;    
        }
        private void CustomiseGridView()
        {
            try
            {                
                this.dataGridView1.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.cellValueChanged);

                SqlConnection con = Database.GetConnectionObj();
                SqlCommand cmd = new SqlCommand("", con);
                cmd.CommandText = string.Format("SELECT id, serial, err_dscr, pap_code, eb_no, sl_no,marks,hmarks,qr_code,binary,bdexcode,bhexcode,comments FROM dbo.H_solving where file_name = '{0}' order by serial ", filename);
               
                da.SelectCommand = cmd;
                da.Fill(dt);
                this.dataGridView1.DataSource = dt;

                this.dataGridView1.Columns[0].Visible = false; // id
                this.dataGridView1.Columns["qr_code"].Visible = false;
                this.dataGridView1.Columns["binary"].Visible = false;

                this.dataGridView1.Columns["serial"].ReadOnly = true;
                this.dataGridView1.Columns["err_dscr"].ReadOnly = true;

                // Explicit Column Header Labels
                this.dataGridView1.Columns["serial"].HeaderText = "Serial";
                this.dataGridView1.Columns["err_dscr"].HeaderText = "Error Description";
                this.dataGridView1.Columns["pap_code"].HeaderText = "Paper Code (pap_code)";
                this.dataGridView1.Columns["eb_no"].HeaderText = "Examiner (eb_no)";
                this.dataGridView1.Columns["sl_no"].HeaderText = "SL No (sl_no)";
                this.dataGridView1.Columns["marks"].HeaderText = "Marks (mark)";
                this.dataGridView1.Columns["hmarks"].HeaderText = "H-Marks (hmark)";
                this.dataGridView1.Columns["bdexcode"].HeaderText = "Litho Dex (litho1)";
                this.dataGridView1.Columns["bhexcode"].HeaderText = "Litho Hex (litho2)";
                this.dataGridView1.Columns["comments"].HeaderText = "Comments";

                // Column Width Sizing
                this.dataGridView1.Columns["serial"].Width = 65;
                this.dataGridView1.Columns["err_dscr"].Width = 180;
                this.dataGridView1.Columns["pap_code"].Width = 150;
                this.dataGridView1.Columns["eb_no"].Width = 130;
                this.dataGridView1.Columns["sl_no"].Width = 95;
                this.dataGridView1.Columns["marks"].Width = 95;
                this.dataGridView1.Columns["hmarks"].Width = 110;
                this.dataGridView1.Columns["bdexcode"].Width = 260;
                this.dataGridView1.Columns["bhexcode"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                ((DataGridViewTextBoxColumn)this.dataGridView1.Columns["pap_code"   ]).MaxInputLength = 6;
                ((DataGridViewTextBoxColumn)this.dataGridView1.Columns["eb_no"      ]).MaxInputLength = 6;
                ((DataGridViewTextBoxColumn)this.dataGridView1.Columns["sl_no"      ]).MaxInputLength = 3;
                ((DataGridViewTextBoxColumn)this.dataGridView1.Columns["marks"      ]).MaxInputLength = 3;
                ((DataGridViewTextBoxColumn)this.dataGridView1.Columns["hmarks"     ]).MaxInputLength = 3;
                ((DataGridViewTextBoxColumn)this.dataGridView1.Columns["bdexcode"   ]).MaxInputLength = 32;
                ((DataGridViewTextBoxColumn)this.dataGridView1.Columns["bhexcode"   ]).MaxInputLength = 32;
                ((DataGridViewTextBoxColumn)this.dataGridView1.Columns["comments"]).MaxInputLength = 50;

                con.Close();

                UITheme.StyleDataGridView(this.dataGridView1);
                this.dataGridView1.ColumnHeadersVisible = true;
                this.dataGridView1.ScrollBars = ScrollBars.Both;
                this.dataGridView1.RowPrePaint += DataGridView1_RowPrePaint;

                this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.cellValueChanged);

                InitTopStatsBar();
                UpdateTopStats();

                // Ensure DataGridView fills client area below top panels so column headers and scrollbars are 100% visible
                pnlTopStatsBar.SendToBack();
                foreach (Control c in this.Controls)
                {
                    if (c.Name == "pnlHeaderBanner") c.SendToBack();
                }
                this.dataGridView1.BringToFront();
            }
            catch (Exception ee)
            {
                MessageBox.Show("Database error.\n" + ee.StackTrace.ToString());
                Application.Exit();
            }
        }

        public Label lblTotalRowsVal;
        public Label lblErrorRowsVal;
        public Label lblCleanRowsVal;
        public Label lblAccuracyVal;
        public Panel pnlTopStatsBar;

        private void InitTopStatsBar()
        {
            if (pnlTopStatsBar != null) return;

            pnlTopStatsBar = new Panel
            {
                Name = "pnlTopStatsBar",
                Dock = DockStyle.Top,
                Height = 36,
                BackColor = Color.FromArgb(240, 253, 244),
                Padding = new Padding(12, 4, 12, 4)
            };

            Panel borderLine = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 1,
                BackColor = UITheme.BorderColor
            };
            pnlTopStatsBar.Controls.Add(borderLine);

            Label lblTotalTitle = new Label { Text = "📊 Total Rows: ", AutoSize = true, Location = new Point(16, 8), Font = new Font("Segoe UI", 9.5f, FontStyle.Bold), ForeColor = UITheme.TextBlack };
            lblTotalRowsVal = new Label { Text = "0", AutoSize = true, Location = new Point(115, 8), Font = new Font("Segoe UI", 10f, FontStyle.Bold), ForeColor = UITheme.GreenHeader };

            Label lblErrTitle = new Label { Text = "⚠️ Errors Remaining: ", AutoSize = true, Location = new Point(200, 8), Font = new Font("Segoe UI", 9.5f, FontStyle.Bold), ForeColor = UITheme.TextBlack };
            lblErrorRowsVal = new Label { Text = "0", AutoSize = true, Location = new Point(340, 8), Font = new Font("Segoe UI", 10f, FontStyle.Bold), ForeColor = UITheme.ErrorRed };

            Label lblCleanTitle = new Label { Text = "✨ Solved Rows: ", AutoSize = true, Location = new Point(430, 8), Font = new Font("Segoe UI", 9.5f, FontStyle.Bold), ForeColor = UITheme.TextBlack };
            lblCleanRowsVal = new Label { Text = "0", AutoSize = true, Location = new Point(540, 8), Font = new Font("Segoe UI", 10f, FontStyle.Bold), ForeColor = UITheme.GreenHeader };

            Label lblAccTitle = new Label { Text = "📈 Progress: ", AutoSize = true, Location = new Point(630, 8), Font = new Font("Segoe UI", 9.5f, FontStyle.Bold), ForeColor = UITheme.TextBlack };
            lblAccuracyVal = new Label { Text = "100.0%", AutoSize = true, Location = new Point(720, 8), Font = new Font("Segoe UI", 10f, FontStyle.Bold), ForeColor = UITheme.GreenHeader };

            pnlTopStatsBar.Controls.Add(lblTotalTitle);
            pnlTopStatsBar.Controls.Add(lblTotalRowsVal);
            pnlTopStatsBar.Controls.Add(lblErrTitle);
            pnlTopStatsBar.Controls.Add(lblErrorRowsVal);
            pnlTopStatsBar.Controls.Add(lblCleanTitle);
            pnlTopStatsBar.Controls.Add(lblCleanRowsVal);
            pnlTopStatsBar.Controls.Add(lblAccTitle);
            pnlTopStatsBar.Controls.Add(lblAccuracyVal);

            this.Controls.Add(pnlTopStatsBar);
            pnlTopStatsBar.BringToFront();
        }

        public void UpdateTopStats()
        {
            if (this.dataGridView1 == null || this.dataGridView1.DataSource == null) return;
            int total = 0;
            int errors = 0;
            int clean = 0;

            DataTable dtSource = this.dataGridView1.DataSource as DataTable;
            if (dtSource != null)
            {
                total = dtSource.Rows.Count;
                foreach (DataRow r in dtSource.Rows)
                {
                    if (r.RowState != DataRowState.Deleted)
                    {
                        string err = r["err_dscr"] != null ? r["err_dscr"].ToString().Trim() : "";
                        if (!string.IsNullOrEmpty(err)) errors++;
                        else clean++;
                    }
                }
            }

            double percent = total > 0 ? ((double)clean / total) * 100.0 : 100.0;

            if (lblTotalRowsVal != null) lblTotalRowsVal.Text = total.ToString();
            if (lblErrorRowsVal != null) lblErrorRowsVal.Text = errors.ToString();
            if (lblCleanRowsVal != null) lblCleanRowsVal.Text = clean.ToString();
            if (lblAccuracyVal != null) lblAccuracyVal.Text = string.Format("{0:F1}%", percent);
        }

        private void DataGridView1_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataGridView1.Rows.Count)
            {
                var row = dataGridView1.Rows[e.RowIndex];
                if (row.Cells["err_dscr"].Value != null && !string.IsNullOrEmpty(row.Cells["err_dscr"].Value.ToString()))
                {
                    row.DefaultCellStyle.ForeColor = UITheme.ErrorRed;
                    row.DefaultCellStyle.BackColor = Color.FromArgb(254, 242, 242);
                    row.DefaultCellStyle.Font = new Font("Consolas", 10.5f, FontStyle.Bold);
                }
                else
                {
                    row.DefaultCellStyle.ForeColor = UITheme.TextBlack;
                    row.DefaultCellStyle.BackColor = (e.RowIndex % 2 == 0) ? UITheme.LightSurface : UITheme.GreenLightTint;
                    row.DefaultCellStyle.Font = UITheme.FontGrid;
                }
            }
        }
        private void litho_correction()
        {

            LithoCorrection L = new LithoCorrection();

            var result = L.ShowDialog();
            if (result == DialogResult.OK)
            {
                //string val = form.ReturnValue1;            //values preserved after close
                //string dateString = form.ReturnValue2;
                ////Do something here with these values

                ////for example
                //this.txtSomething.Text = val;
                MessageBox.Show("ok");
            }
            else
                MessageBox.Show("cancel");
        }
        
        public void span()
        {
            //try
            //{
            //    SqlConnection con = Database.GetConnectionObj();
            //    if (con == null)
            //    {
            //        MessageBox.Show("Connection Problem");
            //        return;
            //    }
            //    SqlCommand cmd = new SqlCommand("", con);
            //    cmd.CommandText = string.Format("SELECT id, scanned_row, file_name FROM dbo.H_solving WHERE file_name = '{0}' order by id", filename);
            //    SqlDataReader reader = cmd.ExecuteReader();
            //    //int serial = 1;
            //    int affectedRow = 0;
            //    SqlConnection con2 = Database.GetConnectionObj();
            //    while (reader.Read())
            //    {
            //        //MessageBox.Show(reader[0].ToString());
            //        int id = Convert.ToInt32(reader["id"]);
            //        string line = reader["scanned_row"].ToString();

            //        //int n = dataGridView1.Rows.Add();
            //        //string scan_sl = line.Substring(0, 50);
            //        //string scan_sl = line.Substring(0, 50).Replace(' ', '*');
            //        string dexcode      = line.Substring(50, 32).Replace(' ', '0');
            //        string exam_code    = line.Substring(82, 3).Replace(' ', '*');
            //        string reg_no       = line.Substring(85, 11).Replace(' ', '*');
            //        string pap_code     = line.Substring(96, 6).Replace(' ', '*');
            //        string hexcode      = line.Substring(102, 32).Replace(' ', '0');
            //        //string err_dscr = "error goes here";
            //        string err_dscr = //checkExamCode(exam_code) +
            //            Database.checkSubjectCode(pap_code) +
            //            Database.checkRegi(reg_no) +
            //            Database.checkLitho("", "", dexcode, hexcode);

            //        string update = string.Format(
            //            //"UPDATE dbo.H_solving SET err_dscr = '{0}',exam_code = '{1}',pap_code = '{2}',regi = '{3}',litho_1 = '{4}',litho_2 = '{5}',updates = '{6}', serial = {7} WHERE id = '{8}'",
            //            "UPDATE dbo.H_solving SET err_dscr = '{0}',exam_code = '{1}',pap_code = '{2}',regi = '{3}',litho_1 = '{4}',litho_2 = '{5}',updates = '{6}' WHERE id = '{7}'",
            //            err_dscr, exam_code, pap_code, reg_no, dexcode, hexcode, "", id);
            //        //MessageBox.Show(update);
            //        //cmd.CommandText = update;
            //        SqlCommand command = new SqlCommand(update, con2);
            //        //SqlDataReader reader2 = command.ExecuteReader();
            //        //command.ExecuteReader();
            //        try
            //        {
            //            affectedRow = command.ExecuteNonQuery();
            //        }
            //        catch (Exception E)
            //        {
            //        }
            //        //MessageBox.Show(affectedRow.ToString()+" rows affected for id "+ id.ToString());
            //    }
            //    reader.Dispose();
            //}
            //catch (Exception ee)
            //{
            //    MessageBox.Show("Database error.\n" + ee.StackTrace.ToString());
            //    Application.Exit();
            //}
        }

        private void onSolveWindowClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
        //private int mach_count
        private void keyPressedOnCell(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                //MessageBox.Show(this.dataGridView1.CurrentCell.RowIndex.ToString());
                //L.Show();
                dataGridView1.Rows[dataGridView1.CurrentRow.Index].ErrorText = string.Empty;
                current = dataGridView1.CurrentCell.Value.ToString();
                //MessageBox.Show(current);
                dataGridView1.BeginEdit(true);
                //litho_correction();
                e.Handled = true;
            }
            else if (e.KeyChar == 'e' || e.KeyChar == 'E')
            {
                int row = -1, count = 0,sub=0,exm = 0, reg = 0, litho = 0,sl=0,marks=0,hm=0;
                string tmp = "";
                while (++row < dataGridView1.RowCount - 1)
                {
                    if (!string.IsNullOrEmpty(dataGridView1.Rows[row].Cells[2].Value.ToString()))
                    {
                        count++;
                        tmp = dataGridView1.Rows[row].Cells[2].Value.ToString();
                       
                        if(tmp.Contains("Sub")) sub++;
                        if (tmp.Contains("EB.")) reg++;
                        if (tmp.Contains("Litho")) litho++;
                        if (tmp.Contains("SL.")) sl++;
                        if (tmp.Contains("HMarks.")) hm++;
                        else if (tmp.Contains("Marks.")) marks++;  
                    }
                }

                //toolStripStatusLabel1.Text = string.Format(
                tmp = string.Format(
                    "Total Error Line       : {0}\n"+
                    "Examineer Error     : {1}\n" +
                    "Subject Code Error : {2}\n" +
                    "Marks Error   : {3}\n" +
                    "Litho Code Error     : {4}", count, reg, sub, reg, litho);
                MessageBox.Show(tmp);
                e.Handled = true;
            }
            else if (e.KeyChar == '-')
            {
                int col = this.dataGridView1.CurrentCell.ColumnIndex;
                int row = this.dataGridView1.CurrentCell.RowIndex;
                if (col == 3 || col == 4)
                {
                    if(this.dataGridView1.CurrentCell.RowIndex == 0)
                        e.Handled = true;
                    else if (!Regex.IsMatch(dataGridView1.CurrentCell.Value.ToString(), @"^\d+$"))
                    {
                        //dataGridView1.CurrentCell.Value = this.sub_code;
                        dataGridView1.CurrentCell.Value = dataGridView1.Rows[row-1].Cells[col].Value.ToString();
                        e.Handled = true;
                    }
                }
                ////int col = this.dataGridView1.CurrentCell.ColumnIndex;
                //if (col == 3)
                //    dataGridView1.CurrentCell.Value = "201";
                //else
            }
            else if (e.KeyChar == '+' )
            {
                int maxrow = this.dataGridView1.RowCount - 1;
                string cur_str = "";
                //string search_pattern = "";
                int col = this.dataGridView1.CurrentCell.ColumnIndex;
                int row = this.dataGridView1.CurrentCell.RowIndex;// +1;
                
                    for (; row < this.dataGridView1.RowCount; row++)
                        if (this.dataGridView1.Rows[row].Cells["err_dscr"].Value.ToString() != "")
                            {
                                cur_str = this.dataGridView1.Rows[row].Cells["err_dscr"].Value.ToString();
                                if (cur_str.Contains("Sub"))
                                    this.dataGridView1.CurrentCell = this.dataGridView1.Rows[row].Cells["pap_code"];
                                else if (cur_str.Contains("EB"))
                                    this.dataGridView1.CurrentCell = this.dataGridView1.Rows[row].Cells["eb_no"];
                                else if (cur_str.Contains("SL"))
                                    this.dataGridView1.CurrentCell = this.dataGridView1.Rows[row].Cells["sl_no"];
                                else if (cur_str.Contains("HMarks"))
                                    this.dataGridView1.CurrentCell = this.dataGridView1.Rows[row].Cells["hmarks"];
                                else if (cur_str.Contains("Marks"))
                                    this.dataGridView1.CurrentCell = this.dataGridView1.Rows[row].Cells["marks"];
                                else if (cur_str.Contains("Litho"))
                                    this.dataGridView1.CurrentCell = this.dataGridView1.Rows[row].Cells["bdexcode"];
                                e.Handled = true;
                                return;
                            }                
                
                e.Handled = true;
            }
                //int next_col = 1;
                //for (; row < this.dataGridView1.RowCount; row++)
                //{
                //    if (this.dataGridView1.Rows[row].Cells["err_dscr"].Value.ToString() != "")
                //    {
                //        cur_str = this.dataGridView1.Rows[row].Cells["err_dscr"].Value.ToString();
                //        if (cur_str.Contains("Sub"))
                //            next_col = 4;
                //        else if (cur_str.Contains("Regi"))
                //            next_col = 5;
                //        else if (cur_str.Contains("Litho"))
                //            next_col = 7;
                //        this.dataGridView1.CurrentCell = this.dataGridView1.Rows[row].Cells[next_col];
                //        e.Handled = true;
                //        return;
                //    }
                //}

                //int rowno,
                //next_col = 1,
                    //current_row = this.dataGridView1.CurrentCell.RowIndex,
                //if (col <= 3)
                //{ //exam code
                //    col = 3;
                //    for (; row < this.dataGridView1.RowCount; row++)
                //        if (this.dataGridView1.Rows[row].Cells[3].Value.ToString() != "201")
                //        {
                //            //MessageBox.Show(this.dataGridView1.Rows[row].Cells[3].Value.ToString());
                //            this.dataGridView1.CurrentCell = this.dataGridView1.Rows[row].Cells[3];
                //            e.Handled = true;
                //            return;
                //        }
                //    col = 4;
                //    row = 0;
                //}
                //if (col == 4)
            //    if (col <= 4)
            //        { //subject code
            //            col = 4;
            //            for (; row < this.dataGridView1.RowCount; row++)
            //                if (this.dataGridView1.Rows[row].Cells[col].Value.ToString() != this.sub_code)
            //                {
            //                    //MessageBox.Show(this.dataGridView1.Rows[row].Cells[col].Value.ToString());
            //                    this.dataGridView1.CurrentCell = this.dataGridView1.Rows[row].Cells[col];
            //                    e.Handled = true;
            //                    return;
            //                }
            //            col = 5;
            //            row = 0;
            //        }

            //    if (col == 5)
            //    { //subject code
            //        for (; row < this.dataGridView1.RowCount; row++)
            //            if (this.dataGridView1.Rows[row].Cells[2].Value.ToString().Contains("Regi"))
            //            {
            //                //MessageBox.Show(this.dataGridView1.Rows[row].Cells[col].Value.ToString());
            //                this.dataGridView1.CurrentCell = this.dataGridView1.Rows[row].Cells[col];
            //                e.Handled = true;
            //                return;
            //            }
            //        col = 7;
            //        row = 0;
            //    }
            //    if (col > 5)
            //    { //subject code
            //        //col = 6;//??
            //        //row = 0;//??
            //        for (; row < this.dataGridView1.RowCount; row++)
            //            if (this.dataGridView1.Rows[row].Cells[2].Value.ToString().Contains("Litho"))
            //            {
            //                //MessageBox.Show(this.dataGridView1.Rows[row].Cells[col].Value.ToString());
            //                this.dataGridView1.CurrentCell = this.dataGridView1.Rows[row].Cells[col];
            //                e.Handled = true;
            //                return;
            //            }
            //        //col = 6;
            //        //row = 0;
            //    }
            //    e.Handled = true;
            //}
            else if (e.KeyChar == '/')
            {
                finish();
                e.Handled = true;
            }
            else if (e.KeyChar == 'r' || e.KeyChar == 'R')
            {
                relese();
                e.Handled = true;
            }
            else if (e.KeyChar == 'a' || e.KeyChar == 'A')
            {
                int current_pos = Convert.ToInt32(dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[1].Value);
                if (current_pos == 0)
                    current_pos++;
                error_only = !error_only;
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = error_only? string.Format("err_dscr <> '{0}'", ""): string.Empty;
                int row = dataGridView1.RowCount - 1;
                while( Convert.ToInt32(dataGridView1.Rows[row].Cells[1].Value ) > current_pos){
                    if(row==0)
                        break; 
                    row--;
                }
                dataGridView1.CurrentCell = dataGridView1.Rows[row].Cells[2];

                //CustomiseGridView();
                //MessageBox.Show( );
                e.Handled = true;
            }
        }

        private void relese()
        {            //throw new NotImplementedException();
            if (MessageBox.Show("Are you really want to RELESE this file?\n" +
                "All Corrections made will be lost." +
                "Press 'Yes' to RELESE this file.\n" +
                "Press 'No' to return to solving window.\n",
                "Finish Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                try
                {
                    SqlConnection con = Database.GetConnectionObj();
                    if (con == null) throw new Exception("Can't create and open a connection");
                    SqlCommand cmd = new SqlCommand("", con);

                    cmd.CommandText = string.Format(
                        "UPDATE dbo.H_file_list " +
                        "SET solver_name = '__NONE', solver_comments = solver_comments +' {0} Relesed on ' + CURRENT_TIMESTAMP " +
                        "WHERE file_name = '{1}' AND solver_name = '{0}' AND solve_status = 'solving' ",
                        username, filename);
                    //cmd.CommandText = "SELECT dbo.H_file_list.file_name FROM dbo.H_file_list";
                    //SqlDataReader reader = cmd.ExecuteReader();
                    //while (reader.Read())
                    //{
                    //    this.H_file_list_box.Items.Add(reader[0].ToString());
                    //}
                    //reader.Dispose();
                    if (cmd.ExecuteNonQuery() == 0)
                        throw new Exception("DB Update failed");
                    con.Close();
                    con.Dispose();

                    H_file_list fl = new H_file_list(username, caller);
                    fl.Show();
                    this.Hide();
                    //this.Close();
                }
                catch (Exception ee)
                {
                    MessageBox.Show("Database error - Finishing failed.\n" + ee.StackTrace.ToString());
                    //Application.Exit();
                }
                // save file

                if (error_only)
                    (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = string.Empty;
                this.dataGridView1.Sort(dataGridView1.Columns["serial"], ListSortDirection.Ascending);

                if (!System.IO.Directory.Exists("Backup"))
                {
                    System.IO.Directory.CreateDirectory("Backup");
                }
                StreamWriter sw = new StreamWriter(@"Backup\" + filename + ".SLV");
                for (int row = 0; row < dataGridView1.RowCount; row++)
                {
                    sw.WriteLine("{0,12}{1,4}{3,3}{4,6}{5,11}{6,28}{7,10}{8,32}{9,32}{2,40}",
                        dataGridView1.Rows[row].Cells[0].Value, //id
                        dataGridView1.Rows[row].Cells[1].Value, //serial
                        dataGridView1.Rows[row].Cells[2].Value, //err_dscr
                        dataGridView1.Rows[row].Cells[3].Value, //exam_code
                        dataGridView1.Rows[row].Cells[4].Value, //pap_code
                        dataGridView1.Rows[row].Cells[5].Value, //regi
                        dataGridView1.Rows[row].Cells[6].Value, //qr_code
                        dataGridView1.Rows[row].Cells[7].Value, //scrpt_no
                        dataGridView1.Rows[row].Cells[8].Value, //litho_1
                        dataGridView1.Rows[row].Cells[9].Value, //litho_2
                        ""
                        );
                }
                sw.Dispose();
            }
        }
        private void finish()
        {            //throw new NotImplementedException();
            if (MessageBox.Show("Are you really want to finish?\n" +
                "You will be respnsible for any remaining error." +
                "Press 'Yes' to finish solving this file.\n" +
                "Press 'No' to return to solving window.\n",
                "Finish Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                try
                {
                    SqlConnection con = Database.GetConnectionObj();
                    if (con == null) throw new Exception("Can't create and open a connection");
                    SqlCommand cmd = new SqlCommand("", con);

                    cmd.CommandText = string.Format(
                        "UPDATE dbo.H_file_list " +
                        "SET solve_status = 'SOLVED', end_date = CURRENT_TIMESTAMP " +
                        "WHERE file_name = '{1}' AND solver_name = '{0}' ",
                        username, filename);
                    //cmd.CommandText = "SELECT dbo.H_file_list.file_name FROM dbo.H_file_list";
                    //SqlDataReader reader = cmd.ExecuteReader();
                    //while (reader.Read())
                    //{
                    //    this.file_list_box.Items.Add(reader[0].ToString());
                    //}
                    //reader.Dispose();
                    if (cmd.ExecuteNonQuery() == 0)
                        throw new Exception("DB Update failed");
                    con.Close();
                    con.Dispose();

                    H_file_list fl = new H_file_list(username, caller);
                    fl.Show();
                    this.Hide();
                    //this.Close();
                }
                catch (Exception ee)
                {
                    MessageBox.Show("Database error - Finishing failed.\n" + ee.StackTrace.ToString());
                    //Application.Exit();
                }
                // save file

                if (error_only)
                    (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = string.Empty;
                this.dataGridView1.Sort(dataGridView1.Columns["serial"], ListSortDirection.Ascending);

                if (!System.IO.Directory.Exists("Backup"))
                {
                    System.IO.Directory.CreateDirectory("Backup");
                }
                StreamWriter sw = new StreamWriter(@"Backup\" + filename + ".SLV");
                for (int row = 0; row < dataGridView1.RowCount; row++)
                {
                    sw.WriteLine("{0,12}{1,4}{3,3}{4,6}{5,11}{6,28}{7,10}{8,32}{9,32}{2,40}",
                        dataGridView1.Rows[row].Cells[0].Value, //id
                        dataGridView1.Rows[row].Cells[1].Value, //serial
                        dataGridView1.Rows[row].Cells[2].Value, //err_dscr
                        dataGridView1.Rows[row].Cells[3].Value, //exam_code
                        dataGridView1.Rows[row].Cells[4].Value, //pap_code
                        dataGridView1.Rows[row].Cells[5].Value, //regi
                        dataGridView1.Rows[row].Cells[6].Value, //qr_code
                        dataGridView1.Rows[row].Cells[7].Value, //scrpt_no
                        dataGridView1.Rows[row].Cells[8].Value, //litho_1
                        dataGridView1.Rows[row].Cells[9].Value, //litho_2
                        ""
                        );
                }
                sw.Dispose();
            }
        }
        //private string checkExamCode(string s) { return s != "201" ? "Exam code Err. " : ""; }
        

        private void cell_end_edit(object sender, DataGridViewCellEventArgs e)
        {
            //int r = e.RowIndex;
            //if( r != 0 && r!= dataGridView1.RowCount)
            //    r--;
            //r--;
            //MessageBox.Show("dwsdfs");
            //dataGridView1.CurrentCell = dataGridView1.Rows[r].Cells[e.ColumnIndex];
            //MessageBox.Show("cell_end_edit" + dataGridView1.CurrentCell.RowIndex.ToString() + " - " + dataGridView1.CurrentCell.ColumnIndex.ToString());
            //dataGridView1.Rows[e.RowIndex].ErrorText = String.Empty;            
        }
        private void cellValidaTING(object sender,
        DataGridViewCellValidatingEventArgs e)
        {
            switch (e.ColumnIndex)
            {
                case 4:
                    if ( Database.isNotValid(e.FormattedValue.ToString()) )
                    {
                        dataGridView1.Rows[e.RowIndex].ErrorText = e.FormattedValue.ToString() + "is Not a valid PaperCode";
                        MessageBox.Show( e.FormattedValue.ToString() + "is Not a valid PaperCode");
                        //MessageBox.Show("CellValidating error on  " + dataGridView1.CurrentCell.RowIndex.ToString() + " - " + dataGridView1.CurrentCell.ColumnIndex.ToString());
                        e.Cancel = true;
                    }
                    break;
                //case 3:
                //    if (e.FormattedValue.ToString() != "201")
                //    {
                //        dataGridView1.Rows[e.RowIndex].ErrorText = "Exam Code should 201";
                //        MessageBox.Show("CellValidating error on  " + dataGridView1.CurrentCell.RowIndex.ToString() + " - " + dataGridView1.CurrentCell.ColumnIndex.ToString());
                //        e.Cancel = true;
                //    }
                //    break;
            }
           
        } 

        private void cellValideTED(object sender, DataGridViewCellEventArgs e)
        {
            {
                //dataGridView1.Rows[e.RowIndex].ErrorText =
                //    "Company Name must not be empty";
                //MessageBox.Show("Cell " + dataGridView1.CurrentCell.RowIndex.ToString() + " - " + dataGridView1.CurrentCell.ColumnIndex.ToString() + "  cellValideTED");
            
            }
        }

        private string getError(int row)
        {
            return
                Database.checkSubjectCode   (dataGridView1.Rows[row].Cells["pap_code"].Value.ToString()) +
                Database.checkExaminer(dataGridView1.Rows[row].Cells["eb_no"].Value.ToString())+
          Database.checkSerial(dataGridView1.Rows[row].Cells["sl_no"].Value.ToString()) +
          Database.checkMarks(dataGridView1.Rows[row].Cells["marks"].Value.ToString()) +
          Database.checkhMarks(dataGridView1.Rows[row].Cells["hmarks"].Value.ToString()) +
          Database.checkLitho(
                      dataGridView1.Rows[row].Cells["qr_code"].Value.ToString(),
                      dataGridView1.Rows[row].Cells["binary"].Value.ToString(),
                      dataGridView1.Rows[row].Cells["bdexcode"].Value.ToString(),
                      dataGridView1.Rows[row].Cells["bhexcode"].Value.ToString()
          );
           
            //return
            //    //Database.checkExamCode(dataGridView1.Rows[row].Cells[3].Value.ToString()) +
            //    Database.checkSubjectCode(dataGridView1.Rows[row].Cells[4].Value.ToString()) +
            //    Database.checkRegi(dataGridView1.Rows[row].Cells[5].Value.ToString()) +
            //    Database.checkLitho(dataGridView1.Rows[row].Cells[6].Value.ToString(),
            //                dataGridView1.Rows[row].Cells[7].Value.ToString(),
            //                dataGridView1.Rows[row].Cells[8].Value.ToString(),
            //                dataGridView1.Rows[row].Cells[9].Value.ToString()
                
        }

        private void cellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0 || e.ColumnIndex < 3)
                return;

            string field_name = dataGridView1.Columns[e.ColumnIndex].Name;

            if (dataGridView1.Rows[e.RowIndex].ErrorText.ToString().Contains(field_name))
                return;

            string input = dataGridView1.CurrentCell.Value.ToString();

            if (field_name == "pap_code")
                if (Database.isNotValid(input))
                    MessageBox.Show(input + " is not a valid papercode.");

            string id = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            string error = getError(e.RowIndex);
            string qry = String.Format(
                "UPDATE H_solving SET " +
                "{0} = '{1}' " +
                ",err_dscr = '{4}' " +
                ",solver = '{2}' " +
                ",last_update = CURRENT_TIMESTAMP" +
                ",updates = " +
                //"'22' + '({0}{1}{2})' " +

                "'({0}{1}{2}' + CONVERT(VARCHAR,CURRENT_TIMESTAMP)+')' + H_solving.updates " +
                //"'({0}{1}{2}' + CONVERT(VARCHAR,CURRENT_TIMESTAMP)+')'+ updates " +
                " WHERE id = {3}",
                field_name,
                input,
                username,
                id,
                error
                );
            //MessageBox.Show(qry);
            try
            {
                SqlConnection con = Database.GetConnectionObj();
                if (con == null) throw new Exception("Can't create and open a connection");
                SqlCommand cmd = new SqlCommand(qry, con);
                //MessageBox.Show(con.State.ToString());
                if (cmd.ExecuteNonQuery() == 0)
                {
                    MessageBox.Show("Database Update Faild. Try again Latter.\n");
                    dataGridView1.Rows[e.RowIndex].ErrorText = String.Format(
                                    "{1} entered on {0} not updated on database." ,
                                    field_name,
                                    input                                    
                                    );
                    dataGridView1.CurrentCell.Value = current;
                    //TODO: error
                }
                else
                    dataGridView1.Rows[e.RowIndex].Cells[2].Value = error;
                    //cmd.CommandText = qry;               
                con.Close();
                con.Dispose();
                dataGridView1.Rows[e.RowIndex].Cells[2].Value = error;
                UpdateTopStats();
            }
            catch (Exception ee)
            {
                MessageBox.Show("Database Update Faild. Try again Latter.\n" + ee.StackTrace.ToString());
                dataGridView1.Rows[e.RowIndex].ErrorText = String.Format(
                                    "{1} entered on {0} not updated on database.",
                                    field_name,
                                    input
                                    );
                dataGridView1.CurrentCell.Value = current;
            }
           
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
