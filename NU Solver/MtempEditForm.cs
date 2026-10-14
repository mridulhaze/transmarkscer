using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace NU_Solver
{
    public class MtempEditForm : Form
    {
        private Panel pnlTopBar;
        private Label lblSearch;
        private TextBox tbSearch;
        private Button btnSearch;
        private Button btnClearSearch;
        private Label lblLimit;
        private ComboBox cbLimit;
        private Button btnSave;
        private Button btnAddRow;
        private Button btnDeleteRow;
        private Button btnReload;

        private DataGridView dgvMtemp;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel statusLabel;
        private ToolStripStatusLabel countLabel;

        private SqlConnection connection;
        private SqlDataAdapter dataAdapter;
        private SqlCommandBuilder commandBuilder;
        private DataTable dataTable;
        private string activeConnectionString;

        public MtempEditForm(string connStr = null)
        {
            activeConnectionString = connStr ?? Database.GetMtempOpConnection("CreateMtempConn", Database.GetConnectionString());
            InitializeComponent();
            UITheme.ApplyTheme(this, "Edit MTEMP Table");

            UITheme.StyleButton(btnSave, UITheme.StyleEmerald);
            UITheme.StyleButton(btnAddRow, UITheme.StyleSky);
            UITheme.StyleButton(btnDeleteRow, UITheme.StyleRose);
            UITheme.StyleButton(btnReload, UITheme.StyleTeal);
            UITheme.StyleButton(btnSearch, UITheme.StyleIndigo);
            UITheme.StyleButton(btnClearSearch, UITheme.StyleSlate);

            LoadData();
        }

        private void InitializeComponent()
        {
            this.Text = "Edit MTEMP Data - Interactive Editor";
            this.Size = new Size(1100, 720);
            this.MinimumSize = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterParent;

            // Top Toolbar Panel
            pnlTopBar = new Panel();
            pnlTopBar.Dock = DockStyle.Top;
            pnlTopBar.Height = 52;
            pnlTopBar.Padding = new Padding(10, 8, 10, 8);
            pnlTopBar.BackColor = Color.FromArgb(248, 250, 252);

            lblSearch = new Label();
            lblSearch.Text = "Filter:";
            lblSearch.AutoSize = true;
            lblSearch.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            lblSearch.Location = new Point(12, 16);

            tbSearch = new TextBox();
            tbSearch.Location = new Point(58, 13);
            tbSearch.Size = new Size(180, 25);
            tbSearch.Font = new Font("Segoe UI", 9.5f);
            tbSearch.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { btnSearch.PerformClick(); e.Handled = true; e.SuppressKeyPress = true; } };

            btnSearch = new Button();
            btnSearch.Text = "🔍 Find";
            btnSearch.Location = new Point(244, 11);
            btnSearch.Size = new Size(70, 29);
            btnSearch.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            btnSearch.Click += (s, e) => ApplyFilter();

            btnClearSearch = new Button();
            btnClearSearch.Text = "❌ Clear";
            btnClearSearch.Location = new Point(318, 11);
            btnClearSearch.Size = new Size(72, 29);
            btnClearSearch.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            btnClearSearch.Click += (s, e) => { tbSearch.Clear(); ApplyFilter(); };

            lblLimit = new Label();
            lblLimit.Text = "Rows:";
            lblLimit.AutoSize = true;
            lblLimit.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            lblLimit.Location = new Point(400, 16);

            cbLimit = new ComboBox();
            cbLimit.DropDownStyle = ComboBoxStyle.DropDownList;
            cbLimit.Items.AddRange(new object[] { "Top 200", "Top 500", "Top 1000", "All Records" });
            cbLimit.SelectedIndex = 1; // Top 500
            cbLimit.Location = new Point(448, 13);
            cbLimit.Size = new Size(105, 25);
            cbLimit.Font = new Font("Segoe UI", 9f);
            cbLimit.SelectedIndexChanged += (s, e) => LoadData();

            btnSave = new Button();
            btnSave.Text = "💾 Save Changes";
            btnSave.Location = new Point(570, 10);
            btnSave.Size = new Size(130, 31);
            btnSave.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            btnSave.Click += btnSave_Click;

            btnAddRow = new Button();
            btnAddRow.Text = "➕ Add Row";
            btnAddRow.Location = new Point(706, 10);
            btnAddRow.Size = new Size(105, 31);
            btnAddRow.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            btnAddRow.Click += btnAddRow_Click;

            btnDeleteRow = new Button();
            btnDeleteRow.Text = "🗑 Delete Row";
            btnDeleteRow.Location = new Point(817, 10);
            btnDeleteRow.Size = new Size(110, 31);
            btnDeleteRow.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            btnDeleteRow.Click += btnDeleteRow_Click;

            btnReload = new Button();
            btnReload.Text = "🔄 Reload";
            btnReload.Location = new Point(933, 10);
            btnReload.Size = new Size(95, 31);
            btnReload.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            btnReload.Click += (s, e) => LoadData();

            pnlTopBar.Controls.Add(lblSearch);
            pnlTopBar.Controls.Add(tbSearch);
            pnlTopBar.Controls.Add(btnSearch);
            pnlTopBar.Controls.Add(btnClearSearch);
            pnlTopBar.Controls.Add(lblLimit);
            pnlTopBar.Controls.Add(cbLimit);
            pnlTopBar.Controls.Add(btnSave);
            pnlTopBar.Controls.Add(btnAddRow);
            pnlTopBar.Controls.Add(btnDeleteRow);
            pnlTopBar.Controls.Add(btnReload);

            // DataGridView
            dgvMtemp = new DataGridView();
            dgvMtemp.Dock = DockStyle.Fill;
            dgvMtemp.AllowUserToAddRows = false;
            dgvMtemp.AllowUserToDeleteRows = true;
            dgvMtemp.EditMode = DataGridViewEditMode.EditOnEnter;
            dgvMtemp.CellValueChanged += (s, e) => { statusLabel.Text = "⚠️ Unsaved changes pending. Click 'Save Changes' to commit."; statusLabel.ForeColor = Color.FromArgb(180, 83, 9); };

            // StatusStrip
            statusStrip = new StatusStrip();
            statusLabel = new ToolStripStatusLabel("Ready.");
            statusLabel.Spring = true;
            statusLabel.TextAlign = ContentAlignment.MiddleLeft;

            countLabel = new ToolStripStatusLabel("0 rows");
            countLabel.TextAlign = ContentAlignment.MiddleRight;

            statusStrip.Items.Add(statusLabel);
            statusStrip.Items.Add(countLabel);

            this.Controls.Add(dgvMtemp);
            this.Controls.Add(pnlTopBar);
            this.Controls.Add(statusStrip);

            this.FormClosing += MtempEditForm_FormClosing;
        }

        private void LoadData()
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                if (connection != null)
                {
                    connection.Dispose();
                }

                connection = new SqlConnection(activeConnectionString);
                connection.Open();

                string limitClause = "";
                switch (cbLimit.SelectedIndex)
                {
                    case 0: limitClause = "TOP 200"; break;
                    case 1: limitClause = "TOP 500"; break;
                    case 2: limitClause = "TOP 1000"; break;
                    case 3: limitClause = ""; break;
                    default: limitClause = "TOP 500"; break;
                }

                string query = string.Format("SELECT {0} * FROM dbo.mtemp ORDER BY reg_no, pap_code", limitClause);
                dataAdapter = new SqlDataAdapter(query, connection);
                commandBuilder = new SqlCommandBuilder(dataAdapter);

                dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                dgvMtemp.DataSource = dataTable;
                UITheme.StyleDataGridView(dgvMtemp);

                countLabel.Text = string.Format("Loaded {0:N0} records", dataTable.Rows.Count);
                statusLabel.Text = "Data loaded successfully. You can edit cells directly.";
                statusLabel.ForeColor = Color.FromArgb(5, 150, 105);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading mtemp data:\n" + ex.Message, "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                statusLabel.Text = "Error: " + ex.Message;
                statusLabel.ForeColor = Color.FromArgb(220, 38, 38);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void ApplyFilter()
        {
            if (dataTable == null) return;

            string query = tbSearch.Text.Trim().Replace("'", "''");
            if (string.IsNullOrEmpty(query))
            {
                dataTable.DefaultView.RowFilter = "";
            }
            else
            {
                dataTable.DefaultView.RowFilter = string.Format(
                    "reg_no LIKE '%{0}%' OR exm_roll LIKE '%{0}%' OR pap_code LIKE '%{0}%' OR col_code LIKE '%{0}%' OR c_code LIKE '%{0}%' OR status LIKE '%{0}%' OR qr1 LIKE '%{0}%' OR form_sl LIKE '%{0}%'",
                    query);
            }

            countLabel.Text = string.Format("Showing {0:N0} of {1:N0} records", dataTable.DefaultView.Count, dataTable.Rows.Count);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (dataAdapter == null || dataTable == null) return;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                dgvMtemp.EndEdit();
                int changes = dataAdapter.Update(dataTable);
                dataTable.AcceptChanges();

                statusLabel.Text = string.Format("✅ Successfully saved {0:N0} changes to database!", changes);
                statusLabel.ForeColor = Color.FromArgb(5, 150, 105);
                MessageBox.Show(string.Format("Successfully saved {0:N0} modified records to mtemp.", changes), "Changes Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving changes:\n" + ex.Message, "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                statusLabel.Text = "Save error: " + ex.Message;
                statusLabel.ForeColor = Color.FromArgb(220, 38, 38);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnAddRow_Click(object sender, EventArgs e)
        {
            if (dataTable == null) return;
            DataRow newRow = dataTable.NewRow();
            dataTable.Rows.InsertAt(newRow, 0);
            dgvMtemp.CurrentCell = dgvMtemp.Rows[0].Cells[0];
            dgvMtemp.BeginEdit(true);
            statusLabel.Text = "New row inserted at top. Fill values and click 'Save Changes'.";
            statusLabel.ForeColor = Color.FromArgb(37, 99, 235);
        }

        private void btnDeleteRow_Click(object sender, EventArgs e)
        {
            if (dgvMtemp.SelectedRows.Count == 0 && dgvMtemp.CurrentRow == null)
            {
                MessageBox.Show("Please select a row to delete.", "Select Row", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult dr = MessageBox.Show(
                "Are you sure you want to delete the selected row(s)?\nClick 'Save Changes' afterwards to commit deletion to the database.",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (dr != DialogResult.Yes) return;

            foreach (DataGridViewRow r in dgvMtemp.SelectedRows)
            {
                if (!r.IsNewRow)
                {
                    DataRowView drv = r.DataBoundItem as DataRowView;
                    if (drv != null) drv.Row.Delete();
                }
            }

            if (dgvMtemp.SelectedRows.Count == 0 && dgvMtemp.CurrentRow != null && !dgvMtemp.CurrentRow.IsNewRow)
            {
                DataRowView drv = dgvMtemp.CurrentRow.DataBoundItem as DataRowView;
                if (drv != null) drv.Row.Delete();
            }

            statusLabel.Text = "Row(s) marked for deletion. Click 'Save Changes' to commit.";
            statusLabel.ForeColor = Color.FromArgb(180, 83, 9);
        }

        private void MtempEditForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (dataTable != null && dataTable.GetChanges() != null)
            {
                DialogResult dr = MessageBox.Show(
                    "You have unsaved changes in MTEMP table. Do you want to save them before closing?",
                    "Unsaved Changes",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);

                if (dr == DialogResult.Yes)
                {
                    btnSave_Click(this, EventArgs.Empty);
                }
                else if (dr == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
            }

            if (connection != null)
            {
                try { connection.Close(); connection.Dispose(); } catch { }
            }
        }
    }
}
