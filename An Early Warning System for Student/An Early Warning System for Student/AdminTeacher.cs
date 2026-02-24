using Guna.UI2.WinForms;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace An_Early_Warning_System_for_Student
{
    public partial class AdminTeacher : Form
    {
        string connStr = DBConfig.ConnectionString;
        public AdminTeacher()
        {
            InitializeComponent();
        }

        private void AdminTeacher_Load(object sender, EventArgs e)
        {
            guna2ComboBox1.Items.Clear();
            guna2ComboBox1.Items.Add("All"); // default option

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string query = "SELECT DISTINCT strand FROM users ORDER BY strand";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        string strand = dr["strand"].ToString();
                        if (!string.IsNullOrWhiteSpace(strand))
                            guna2ComboBox1.Items.Add(strand);
                    }
                }
            }

            // Set default selection
            guna2ComboBox1.SelectedIndex = 0;

            LoadUsers();
        }

        private void LoadUsers()
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string query = @"SELECT 
                            id_no,
                            firstname,
                            lastname,
                            email,
                            role,
                            strand,
                            isAdmin
                         FROM users";

                MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                guna2DataGridView1.DataSource = dt;

                // Add checkbox only once
                if (!guna2DataGridView1.Columns.Contains("Admin"))
                {
                    DataGridViewCheckBoxColumn chk = new DataGridViewCheckBoxColumn();
                    chk.Name = "Admin";
                    chk.HeaderText = "Admin";
                    chk.DataPropertyName = "isAdmin";
                    chk.TrueValue = 1;
                    chk.FalseValue = 0;

                    guna2DataGridView1.Columns.Remove("isAdmin");
                    guna2DataGridView1.Columns.Add(chk);
                }

                guna2DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                guna2DataGridView1.AllowUserToAddRows = false;

                if (guna2DataGridView1.Columns.Contains("id_no"))
                {
                    guna2DataGridView1.Columns["id_no"].Visible = false;
                }

                if (!guna2DataGridView1.Columns.Contains("Archive"))
                {
                    DataGridViewButtonColumn deleteColumn = new DataGridViewButtonColumn();
                    deleteColumn.Name = "Archive";
                    deleteColumn.HeaderText = "Archive";
                    deleteColumn.Text = "Archive";
                    deleteColumn.UseColumnTextForButtonValue = true;
                    deleteColumn.Width = 80;
                    guna2DataGridView1.Columns.Add(deleteColumn);
                }

                guna2DataGridView1.Columns["Archive"].DisplayIndex = guna2DataGridView1.Columns.Count - 1;

                // CHANGE HEADER TEXT
                guna2DataGridView1.Columns["firstname"].HeaderText = "FIRSTNAME";
                guna2DataGridView1.Columns["lastname"].HeaderText = "LASTNAME";
                guna2DataGridView1.Columns["email"].HeaderText = "EMAIL ADDRESS";
                guna2DataGridView1.Columns["role"].HeaderText = "ROLE";
                guna2DataGridView1.Columns["strand"].HeaderText = "STRAND";
                guna2DataGridView1.Columns["admin"].HeaderText = "ADMIN";

                // CENTER ALIGNMENT
                guna2DataGridView1.ColumnHeadersDefaultCellStyle.Alignment =
                    DataGridViewContentAlignment.MiddleCenter;

                guna2DataGridView1.DefaultCellStyle.Alignment =
                    DataGridViewContentAlignment.MiddleCenter;

                // HEADER FONT STYLE
                guna2DataGridView1.ColumnHeadersDefaultCellStyle.Font =
                    new Font("Century Gothic", 10, FontStyle.Bold);

                // LIGHT DESIGN WITH DARK HEADER
                guna2DataGridView1.Theme = Guna.UI2.WinForms.Enums.DataGridViewPresetThemes.Default;

                guna2DataGridView1.ThemeStyle.BackColor = Color.White;
                guna2DataGridView1.ThemeStyle.GridColor = Color.FromArgb(230, 230, 230);

                // DARK HEADER STYLE
                guna2DataGridView1.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(8, 30, 66);
                guna2DataGridView1.ThemeStyle.HeaderStyle.ForeColor = Color.White;

                guna2DataGridView1.ThemeStyle.RowsStyle.BackColor = Color.White;
                guna2DataGridView1.ThemeStyle.RowsStyle.ForeColor = Color.Black;
                guna2DataGridView1.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(220, 235, 255);
                guna2DataGridView1.ThemeStyle.RowsStyle.SelectionForeColor = Color.Black;

                guna2DataGridView1.ThemeStyle.AlternatingRowsStyle.BackColor = Color.FromArgb(250, 250, 250);
                guna2DataGridView1.ThemeStyle.AlternatingRowsStyle.ForeColor = Color.Black;

                guna2DataGridView1.BorderStyle = BorderStyle.None;
                guna2DataGridView1.EnableHeadersVisualStyles = false;
                guna2DataGridView1.RowHeadersVisible = false;

                // REMOVE BLUE SELECTION SA CELLS / ROWS
                guna2DataGridView1.DefaultCellStyle.SelectionBackColor =
                    guna2DataGridView1.DefaultCellStyle.BackColor;

                guna2DataGridView1.DefaultCellStyle.SelectionForeColor =
                    guna2DataGridView1.DefaultCellStyle.ForeColor;

                guna2DataGridView1.ColumnHeadersHeight = 45;
                guna2DataGridView1.RowTemplate.Height = 40;
                guna2DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                guna2DataGridView1.AllowUserToResizeColumns = false;
                guna2DataGridView1.AllowUserToResizeRows = false;

                guna2DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                guna2DataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            }
        }



        private void guna2Button1_Click(object sender, EventArgs e)
        {
        }

        private void SearchUsers(string keyword)
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string selectedStrand = guna2ComboBox1.SelectedItem.ToString();

                string query = @"
            SELECT 
                id_no,
                firstname,
                lastname,
                email,
                role,
                strand,
                isAdmin
            FROM users
            WHERE (firstname LIKE @keyword OR lastname LIKE @keyword)
        ";

                if (selectedStrand != "All")
                    query += " AND strand = @strand";

                MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                da.SelectCommand.Parameters.AddWithValue("@keyword", "%" + keyword + "%");

                if (selectedStrand != "All")
                    da.SelectCommand.Parameters.AddWithValue("@strand", selectedStrand);

                DataTable dt = new DataTable();
                da.Fill(dt);

                guna2DataGridView1.DataSource = dt;
            }
        }



        private void btnSaveAdmin_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                MySqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    string selectedAdminId = null;

                    // 1️⃣ Hanapin kung sino ang naka-check na Admin
                    foreach (DataGridViewRow row in guna2DataGridView1.Rows)
                    {
                        if (row.IsNewRow) continue;

                        bool isAdmin = Convert.ToBoolean(row.Cells["Admin"].Value);

                        if (isAdmin)
                        {
                            selectedAdminId = row.Cells["id_no"].Value.ToString();
                            break; // ISA LANG DAPAT
                        }
                    }

                    // 2️⃣ Reset lahat ng admin (isAdmin = 0)
                    string resetQuery = @"UPDATE users SET isAdmin = 0";
                    MySqlCommand resetCmd = new MySqlCommand(resetQuery, conn, transaction);
                    resetCmd.ExecuteNonQuery();

                    // 3️⃣ Set lang yung napiling admin
                    if (!string.IsNullOrEmpty(selectedAdminId))
                    {
                        string setAdminQuery = @"
                    UPDATE users 
                    SET isAdmin = 1 
                    WHERE id_no = @id_no";

                        MySqlCommand setAdminCmd = new MySqlCommand(setAdminQuery, conn, transaction);
                        setAdminCmd.Parameters.AddWithValue("@id_no", selectedAdminId);
                        setAdminCmd.ExecuteNonQuery();
                    }

                    transaction.Commit();

                    MessageBox.Show("Admin updated successfully (Single Admin enforced).",
                                    "Saved",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);

                    LoadUsers(); // refresh
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void search_TextChanged(object sender, EventArgs e)
        {
            SearchUsers(search.Text.Trim());
        }

        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SearchUsers(search.Text.Trim());
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (guna2DataGridView1.Columns[e.ColumnIndex].Name == "Archive")
            {
                string id = guna2DataGridView1.Rows[e.RowIndex]
                                .Cells["id_no"].Value.ToString();

                string firstName = guna2DataGridView1.Rows[e.RowIndex]
                                .Cells["firstname"].Value.ToString();

                string lastName = guna2DataGridView1.Rows[e.RowIndex]
                                .Cells["lastname"].Value.ToString();

                string fullName = firstName + " " + lastName;

                DialogResult dr = MessageBox.Show(
                    $"Are you sure you want to archive {fullName}?",
                    "Confirm Archive",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (dr == DialogResult.Yes)
                {
                    using (MySqlConnection conn = new MySqlConnection(connStr))
                    {
                        conn.Open();

                        using (MySqlTransaction tran = conn.BeginTransaction())
                        {
                            try
                            {
                                // 1️⃣ Copy user to users_archived
                                string insertQuery = @"
                            INSERT INTO users_archived
                            SELECT * FROM users
                            WHERE id_no = @id";

                                using (MySqlCommand cmd = new MySqlCommand(insertQuery, conn, tran))
                                {
                                    cmd.Parameters.AddWithValue("@id", id);
                                    cmd.ExecuteNonQuery();
                                }

                                // 2️⃣ Delete from users
                                string deleteQuery = "DELETE FROM users WHERE id_no = @id";

                                using (MySqlCommand cmd = new MySqlCommand(deleteQuery, conn, tran))
                                {
                                    cmd.Parameters.AddWithValue("@id", id);
                                    cmd.ExecuteNonQuery();
                                }

                                tran.Commit();

                                MessageBox.Show($"{fullName} has been archived.",
                                                "Archived",
                                                MessageBoxButtons.OK,
                                                MessageBoxIcon.Information);

                                LoadUsers(); // refresh
                            }
                            catch (Exception ex)
                            {
                                tran.Rollback();
                                MessageBox.Show("Error archiving user:\n" + ex.Message,
                                                "Error",
                                                MessageBoxButtons.OK,
                                                MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
        }


        private void guna2Button2_Click(object sender, EventArgs e)
        {
            AddTeacher add = new AddTeacher();
            add.Show();
        }
    }

}

