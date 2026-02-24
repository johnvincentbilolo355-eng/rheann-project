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
    public partial class Archived : Form
    {
        public Archived()
        {
            InitializeComponent();
        }
        private void Archived_Load(object sender, EventArgs e)
        {
            cmbType.Items.Add("Students");
            cmbType.Items.Add("Tearchers");
            cmbType.SelectedIndex = 0;

            LoadData();
            StyleDataGridView();
        }

        private void LoadData(string searchText = "")
        {
            using (MySqlConnection conn = new MySqlConnection(DBConfig.ConnectionString))
            {
                conn.Open();

                string type = cmbType.SelectedItem.ToString();
                string query = "";

                if (type == "Students")
                {
                    query = @"
                SELECT student_no AS ID,
                       student_name AS Name,
                       strand,
                       year_level,
                       'Student' AS Type
                FROM archived_students
                WHERE @search = '' 
                   OR student_name LIKE @pattern";
                }
                else // Users
                {
                    query = @"
                SELECT id_no AS ID,
                       CONCAT(firstname,' ',lastname) AS Name,
                       strand,
                       role AS year_level,
                       'User' AS Type
                FROM users_archived
                WHERE @search = '' 
                   OR firstname LIKE @pattern 
                   OR lastname LIKE @pattern";
                }

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@search", searchText.Trim());
                    cmd.Parameters.AddWithValue("@pattern", "%" + searchText.Trim() + "%");

                    DataTable dt = new DataTable();
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }

                    dataGridView1.DataSource = dt;

                    if (!dataGridView1.Columns.Contains("recover"))
                    {
                        DataGridViewButtonColumn recoverColumn = new DataGridViewButtonColumn
                        {
                            Name = "recover",
                            HeaderText = "RECOVER",
                            Text = "Recover",
                            UseColumnTextForButtonValue = true
                        };
                        dataGridView1.Columns.Add(recoverColumn);
                    }

                    dataGridView1.Columns["ID"].Visible = false;
                }
            }
        }
        private void StyleDataGridView()
        {
            dataGridView1.Font = new Font("Century Gothic", 8f, FontStyle.Regular);
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.RowTemplate.Height = 40;
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#061d4a");
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Century Gothic", 10f, FontStyle.Bold);
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.ColumnHeadersHeight = 38;
            dataGridView1.DefaultCellStyle.Font = new Font("Century Gothic", 8f);
            dataGridView1.DefaultCellStyle.Padding = new Padding(8);
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.FromArgb(100, 149, 237);
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.White;
            dataGridView1.DefaultCellStyle.BackColor = Color.White;
            dataGridView1.DefaultCellStyle.ForeColor = Color.Black;
            dataGridView1.DefaultCellStyle.SelectionBackColor = ColorTranslator.FromHtml("#d9e3f3");
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.Black;
            dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#f2f3fa");
            //center text
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;

            dataGridView1.DefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;


        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (dataGridView1.Columns[e.ColumnIndex].Name == "recover")
            {
                string id = dataGridView1.Rows[e.RowIndex].Cells["ID"].Value.ToString();
                string name = dataGridView1.Rows[e.RowIndex].Cells["Name"].Value.ToString();
                string type = cmbType.SelectedItem.ToString();

                DialogResult result = MessageBox.Show(
                    $"Do you want to recover {name}?",
                    "Confirm Recovery",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result != DialogResult.Yes) return;

                using (MySqlConnection conn = new MySqlConnection(DBConfig.ConnectionString))
                {
                    conn.Open();
                    using (MySqlTransaction tran = conn.BeginTransaction())
                    {
                        try
                        {
                            string insertQuery = "";
                            string deleteQuery = "";

                            if (type == "Students")
                            {
                                insertQuery = "INSERT INTO students SELECT * FROM archived_students WHERE student_no = @id";
                                deleteQuery = "DELETE FROM archived_students WHERE student_no = @id";
                            }
                            else // Users
                            {
                                insertQuery = "INSERT INTO users SELECT * FROM users_archived WHERE id_no = @id";
                                deleteQuery = "DELETE FROM users_archived WHERE id_no = @id";
                            }

                            using (MySqlCommand cmd = new MySqlCommand(insertQuery, conn, tran))
                            {
                                cmd.Parameters.AddWithValue("@id", id);
                                cmd.ExecuteNonQuery();
                            }

                            using (MySqlCommand cmd = new MySqlCommand(deleteQuery, conn, tran))
                            {
                                cmd.Parameters.AddWithValue("@id", id);
                                cmd.ExecuteNonQuery();
                            }

                            tran.Commit();

                            MessageBox.Show($"{name} has been recovered.",
                                            "Recovered",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Information);

                            LoadData(search.Text);
                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                            MessageBox.Show("Error recovering:\n" + ex.Message);
                        }
                    }
                }
            }
        }

        private void search_TextChanged(object sender, EventArgs e)
        {
            LoadData(search.Text);
        }

        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData(search.Text);
        }
    }
}
