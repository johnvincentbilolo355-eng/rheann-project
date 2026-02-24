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
using static System.Collections.Specialized.BitVector32;

namespace An_Early_Warning_System_for_Student
{
    public partial class submitReports : Form
    {
        public submitReports()
        {
            InitializeComponent();
        }

        private void submitReports_Load(object sender, EventArgs e)
        {

            LoadReports();
            LoadYearLevels();
            LoadStrands();
            LoadSections();
            LoadReportsFiltered();
        }

        private void LoadYearLevels()
        {
            yearlevel.Items.Clear();
            yearlevel.Items.Add("ALL");

            string query = @"
        SELECT DISTINCT TRIM(year_level) AS year_level
        FROM students
        WHERE year_level IS NOT NULL AND TRIM(year_level) <> ''
        ORDER BY year_level";

            using (MySqlConnection conn = new MySqlConnection(DBConfig.ConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    string yl = dr["year_level"].ToString();
                    if (!yearlevel.Items.Contains(yl)) // check to avoid duplicate
                        yearlevel.Items.Add(yl);
                }
            }

            yearlevel.SelectedIndex = 0; // default ALL
        }

        private void LoadStrands()
        {
            guna2ComboBox1.Items.Clear();
            guna2ComboBox1.Items.Add("ALL");

            string query = @"
        SELECT DISTINCT TRIM(strand) AS strand
        FROM students
        WHERE strand IS NOT NULL AND TRIM(strand) <> ''
        ORDER BY strand";

            using (MySqlConnection conn = new MySqlConnection(DBConfig.ConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    string s = dr["strand"].ToString();
                    if (!guna2ComboBox1.Items.Contains(s))
                        guna2ComboBox1.Items.Add(s);
                }
            }

            guna2ComboBox1.SelectedIndex = 0;
        }

        private void LoadSections()
        {
            guna2ComboBox2.Items.Clear();
            guna2ComboBox2.Items.Add("ALL");

            string query = @"
        SELECT DISTINCT TRIM(section) AS section
        FROM students
        WHERE section IS NOT NULL AND TRIM(section) <> ''
        ORDER BY section";

            using (MySqlConnection conn = new MySqlConnection(DBConfig.ConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    string sec = dr["section"].ToString();
                    if (!guna2ComboBox2.Items.Contains(sec))
                        guna2ComboBox2.Items.Add(sec);
                }
            }

            guna2ComboBox2.SelectedIndex = 0;
        }

        private void LoadReportsFiltered()
        {
            string year = yearlevel.Text;
            string str = guna2ComboBox1.Text;
            string sec = guna2ComboBox2.Text;

            string query = @"
        SELECT 
            sr.id,
            sr.student_no,
            s.student_name,
            s.year_level,
            s.strand,
            s.section,
            sr.report_type,
            sr.category,
            sr.priority_level,
            sr.report_title,
            sr.description,
            sr.report_date,
            u.lastname AS teacher_lastname
        FROM student_reports sr
        LEFT JOIN students s ON sr.student_no = s.student_no
        LEFT JOIN users u ON sr.teacher_id = u.id_no
        WHERE 1=1
    ";

            if (year != "ALL")
                query += " AND s.year_level = @year";

            if (str != "ALL")
                query += " AND s.strand = @strand";

            if (sec != "ALL")
                query += " AND s.section = @section";

            using (MySqlConnection conn = new MySqlConnection(DBConfig.ConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);

                if (year != "ALL")
                    cmd.Parameters.AddWithValue("@year", year);

                if (str != "ALL")
                    cmd.Parameters.AddWithValue("@strand", str);

                if (sec != "ALL")
                    cmd.Parameters.AddWithValue("@section", sec);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView1.DataSource = dt;
                StyleDataGridView();
                // ========== DONE BUTTON ==========
                // Rename Columns
                if (dataGridView1.Columns.Contains("year_level"))
                    dataGridView1.Columns["year_level"].HeaderText = "Year Level";

                StyleDataGridView();

                // Add Buttons if missing
                if (!dataGridView1.Columns.Contains("done"))
                {
                    DataGridViewButtonColumn doneBtn = new DataGridViewButtonColumn();
                    doneBtn.Name = "done";
                    doneBtn.HeaderText = "Done";
                    doneBtn.Text = "Done";
                    doneBtn.UseColumnTextForButtonValue = true;
                    doneBtn.Width = 60;
                    doneBtn.FlatStyle = FlatStyle.Flat;
                    dataGridView1.Columns.Add(doneBtn);
                }
                if (!dataGridView1.Columns.Contains("view"))
                {
                    DataGridViewButtonColumn viewBtn = new DataGridViewButtonColumn();
                    viewBtn.Name = "view";
                    viewBtn.HeaderText = "Action";
                    viewBtn.Text = "View";
                    viewBtn.UseColumnTextForButtonValue = true;
                    viewBtn.Width = 60;
                    viewBtn.FlatStyle = FlatStyle.Flat;
                    dataGridView1.Columns.Add(viewBtn);
                }

                // Column Order
                dataGridView1.Columns["id"].Visible = false;
                dataGridView1.Columns["done"].DisplayIndex = 0;
                dataGridView1.Columns["student_no"].DisplayIndex = 1;
                dataGridView1.Columns["student_name"].DisplayIndex = 2;
                dataGridView1.Columns["year_level"].DisplayIndex = 3;
                dataGridView1.Columns["strand"].DisplayIndex = 4;
                dataGridView1.Columns["section"].DisplayIndex = 5;
                dataGridView1.Columns["view"].DisplayIndex = dataGridView1.Columns.Count - 1;
            }
        }

        private void LoadReports()
        {
            string connStr = DBConfig.ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string query = @"
                SELECT 
                    sr.id,
                    sr.student_no,
                    IFNULL(s.student_name,'N/A') AS student_name,
                    IFNULL(s.year_level,'N/A') AS year_level,
                    IFNULL(s.strand,'N/A') AS strand,
                    IFNULL(s.section,'N/A') AS section,
                    sr.report_type,
                    sr.category,
                    sr.priority_level,
                    sr.report_title,
                    sr.description,
                    sr.report_date,
                    sr.teacher_id,
                    IFNULL(u.lastname,'N/A') AS teacher_lastname
                FROM student_reports sr
                LEFT JOIN students s ON sr.student_no = s.student_no
                LEFT JOIN users u ON sr.teacher_id = u.id_no
                WHERE 1=1;
                ";

                MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView1.DataSource = dt;

                if (dataGridView1.Columns.Contains("year_level"))
                    dataGridView1.Columns["year_level"].HeaderText = "Year Level";
                dataGridView1.Columns["student_no"].HeaderText = "Student Number";
                dataGridView1.Columns["student_name"].HeaderText = "Student Name";
                dataGridView1.Columns["strand"].HeaderText = "Strand";
                dataGridView1.Columns["section"].HeaderText = "Section";
                dataGridView1.Columns["report_type"].HeaderText = "Report Type";
                dataGridView1.Columns["category"].HeaderText = "Category";
                dataGridView1.Columns["priority_level"].HeaderText = "Priority Level";
                dataGridView1.Columns["report_title"].HeaderText = "Title";
                dataGridView1.Columns["description"].HeaderText = "Description";
                dataGridView1.Columns["report_date"].HeaderText = "Date";
                if (dataGridView1.Columns.Contains("teacher_id"))
                {
                    dataGridView1.Columns["teacher_id"].HeaderText = "Teacher ID";
                }
                dataGridView1.Columns["teacher_lastname"].HeaderText = "Teacher Name";

                StyleDataGridView();

                // Add Done Button
                if (!dataGridView1.Columns.Contains("done"))
                {
                    DataGridViewButtonColumn doneBtn = new DataGridViewButtonColumn();
                    doneBtn.Name = "done";
                    doneBtn.HeaderText = "Done";
                    doneBtn.Text = "Done";
                    doneBtn.UseColumnTextForButtonValue = true;
                    doneBtn.Width = 60;
                    doneBtn.FlatStyle = FlatStyle.Flat;
                    dataGridView1.Columns.Add(doneBtn);
                }

                // Add View Button
                if (!dataGridView1.Columns.Contains("view"))
                {
                    DataGridViewButtonColumn viewBtn = new DataGridViewButtonColumn();
                    viewBtn.Name = "view";
                    viewBtn.HeaderText = "Action";
                    viewBtn.Text = "View";
                    viewBtn.UseColumnTextForButtonValue = true;
                    viewBtn.Width = 60;
                    viewBtn.FlatStyle = FlatStyle.Flat;
                    dataGridView1.Columns.Add(viewBtn);
                }

                // Set Column Order
                dataGridView1.Columns["done"].DisplayIndex = 0;
                dataGridView1.Columns["student_no"].DisplayIndex = 1;
                dataGridView1.Columns["student_name"].DisplayIndex = 2;
                dataGridView1.Columns["year_level"].DisplayIndex = 3;
                dataGridView1.Columns["strand"].DisplayIndex = 4;
                dataGridView1.Columns["section"].DisplayIndex = 5;
                dataGridView1.Columns["view"].DisplayIndex = dataGridView1.Columns.Count - 1;
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

        private void DeleteReport(string studentNo)
        {
            string connStr = DBConfig.ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string query = "DELETE FROM student_reports WHERE student_no = @student_no";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@student_no", studentNo);

                cmd.ExecuteNonQuery();
            }
        }


        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            // View Button
            if (dataGridView1.Columns[e.ColumnIndex].Name == "view")
            {
                string studentNo = dataGridView1.Rows[e.RowIndex].Cells["student_no"].Value.ToString();

                studentInfo frm = new studentInfo(studentNo);
                frm.Show();
            }

            // Done Button
            if (dataGridView1.Columns[e.ColumnIndex].Name == "done")
            {
                string studentNo = dataGridView1.Rows[e.RowIndex].Cells["student_no"].Value.ToString();

                DialogResult result = MessageBox.Show(
                     "Is the student's issue already resolved?",
                     "Confirm",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    DeleteReport(studentNo); // delete from database
                    dataGridView1.Rows.RemoveAt(e.RowIndex); // remove row sa grid
                }
            }
        }


        private void search_TextChanged(object sender, EventArgs e)
        {
            string connStr = DBConfig.ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string query = @"
            SELECT 
                sr.id,
                sr.student_no,
                IFNULL(s.student_name,'N/A') AS student_name,
                IFNULL(s.year_level,'N/A') AS year_level,
                IFNULL(s.strand,'N/A') AS strand,
                IFNULL(s.section,'N/A') AS section,
                sr.report_type,
                sr.category,
                sr.priority_level,
                sr.report_title,
                sr.description,
                sr.report_date,
                sr.teacher_id,
                IFNULL(u.lastname,'N/A') AS teacher_lastname
            FROM student_reports sr
            LEFT JOIN students s ON sr.student_no = s.student_no
            LEFT JOIN users u ON sr.teacher_id = u.id_no
            WHERE CONCAT(sr.student_no, ' ', IFNULL(s.student_name,'')) LIKE @search;
        ";

                MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                da.SelectCommand.Parameters.AddWithValue("@search", "%" + search.Text + "%");

                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView1.DataSource = null;
                dataGridView1.Columns.Clear();
                dataGridView1.DataSource = dt;

                // Column Headers
                if (dataGridView1.Columns.Contains("id")) dataGridView1.Columns["id"].Visible = false;
                if (dataGridView1.Columns.Contains("student_no")) dataGridView1.Columns["student_no"].HeaderText = "Student Number";
                if (dataGridView1.Columns.Contains("student_name")) dataGridView1.Columns["student_name"].HeaderText = "Student Name";
                if (dataGridView1.Columns.Contains("year_level")) dataGridView1.Columns["year_level"].HeaderText = "Year Level";
                if (dataGridView1.Columns.Contains("strand")) dataGridView1.Columns["strand"].HeaderText = "Strand";
                if (dataGridView1.Columns.Contains("section")) dataGridView1.Columns["section"].HeaderText = "Section";
                if (dataGridView1.Columns.Contains("report_type")) dataGridView1.Columns["report_type"].HeaderText = "Report Type";
                if (dataGridView1.Columns.Contains("category")) dataGridView1.Columns["category"].HeaderText = "Category";
                if (dataGridView1.Columns.Contains("priority_level")) dataGridView1.Columns["priority_level"].HeaderText = "Priority Level";
                if (dataGridView1.Columns.Contains("report_title")) dataGridView1.Columns["report_title"].HeaderText = "Title";
                if (dataGridView1.Columns.Contains("description")) dataGridView1.Columns["description"].HeaderText = "Description";
                if (dataGridView1.Columns.Contains("report_date")) dataGridView1.Columns["report_date"].HeaderText = "Date";
                if (dataGridView1.Columns.Contains("teacher_id")) dataGridView1.Columns["teacher_id"].HeaderText = "Teacher ID";
                if (dataGridView1.Columns.Contains("teacher_lastname")) dataGridView1.Columns["teacher_lastname"].HeaderText = "Teacher Name";

                StyleDataGridView();

                // Add Buttons safely
                if (!dataGridView1.Columns.Contains("done"))
                {
                    DataGridViewButtonColumn doneBtn = new DataGridViewButtonColumn();
                    doneBtn.Name = "done";
                    doneBtn.HeaderText = "Done";
                    doneBtn.Text = "Done";
                    doneBtn.UseColumnTextForButtonValue = true;
                    doneBtn.Width = 60;
                    doneBtn.FlatStyle = FlatStyle.Flat;
                    dataGridView1.Columns.Add(doneBtn);
                }
                if (!dataGridView1.Columns.Contains("view"))
                {
                    DataGridViewButtonColumn viewBtn = new DataGridViewButtonColumn();
                    viewBtn.Name = "view";
                    viewBtn.HeaderText = "Action";
                    viewBtn.Text = "View";
                    viewBtn.UseColumnTextForButtonValue = true;
                    viewBtn.Width = 60;
                    viewBtn.FlatStyle = FlatStyle.Flat;
                    dataGridView1.Columns.Add(viewBtn);
                }
                dataGridView1.Columns["student_no"].DisplayIndex = 1;
        dataGridView1.Columns["student_name"].DisplayIndex = 2;
        dataGridView1.Columns["year_level"].DisplayIndex = 3;
        dataGridView1.Columns["strand"].DisplayIndex = 4;
        dataGridView1.Columns["section"].DisplayIndex = 5;
        dataGridView1.Columns["view"].DisplayIndex = dataGridView1.Columns.Count - 1;
    }
}

        private void yearlevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadReportsFiltered();
        }

        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadReportsFiltered();
        }

        private void guna2ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadReportsFiltered();
        }
    }
}
