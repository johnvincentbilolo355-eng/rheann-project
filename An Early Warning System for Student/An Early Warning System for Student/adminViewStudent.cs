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
    public partial class adminViewStudent : Form
    {
        bool isInitializing = true;

        string year = "";
        string strand = "";
        string section = "";

        string gradeType = "MIDTERM";
        public adminViewStudent()
        {
            InitializeComponent();
        }

        private void search_TextChanged(object sender, EventArgs e)
        {
            LoadStudents(search.Text.Trim());
        }


        private void adminViewStudent_Load(object sender, EventArgs e)
        {
            isInitializing = true;

            LoadYearLevels();
            LoadStrands();
            LoadSections("ALL");

            cmbView.Items.Clear();
            cmbView.Items.Add("MIDTERM");
            cmbView.Items.Add("FINALS");
            cmbView.Items.Add("GPA");
            cmbView.SelectedIndex = 0;

            isInitializing = false;

            LoadStudents();
            FormatGrid();
        }

        private void LoadStudents(string keyword = "")
        {
            string gradeColumn = "midterm_grade";
            if (gradeType == "FINALS") gradeColumn = "finals_grade";
            else if (gradeType == "GPA") gradeColumn = "gpa";

            string query = $@"
SELECT 
    s.student_no, 
    s.student_name, 
    s.year_level, 
    sub.{gradeColumn} AS grade,
    sub.attendance_rate, 
    s.violations
FROM students s
JOIN subjects sub ON s.student_no = sub.student_no
WHERE
    (@year='' OR s.year_level=@year)
    AND (@strand='' OR s.strand=@strand)
    AND (@section='' OR s.section=@section)
    AND (@keyword='' OR s.student_name LIKE @keyword OR s.student_no LIKE @keyword)
ORDER BY 
    CAST(REPLACE(s.year_level, 'Year ', '') AS UNSIGNED) ASC,
    s.student_name ASC
";

            using (MySqlConnection con = new MySqlConnection(DBConfig.ConnectionString))
            using (MySqlCommand cmd = new MySqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@strand", strand);
                cmd.Parameters.AddWithValue("@section", section);
                cmd.Parameters.AddWithValue("@keyword",
                    string.IsNullOrEmpty(keyword) ? "" : "%" + keyword + "%");

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (!dt.Columns.Contains("Risk Status"))
                    dt.Columns.Add("Risk Status", typeof(string));

                foreach (DataRow row in dt.Rows)
                {
                    float grade = SafeFloat(row["grade"]);
                    float attendance = SafeFloat(row["attendance_rate"]);
                    int violations = SafeInt(row["violations"]);

                    row["Risk Status"] = ComputeRiskStatus(grade, attendance, violations);
                }

                dataGridView1.DataSource = dt;
                // Rename columns
                dataGridView1.Columns["student_no"].HeaderText = "Student NO.";
                dataGridView1.Columns["year_level"].HeaderText = "Year Level";
                dataGridView1.Columns["student_name"].HeaderText = "Student Name";
                dataGridView1.Columns["grade"].HeaderText = gradeType; // important
                dataGridView1.Columns["attendance_rate"].HeaderText = "Attendance Rate";
                dataGridView1.Columns["violations"].HeaderText = "Violations";
            }

            if (!dataGridView1.Columns.Contains("Action"))
            {
                DataGridViewButtonColumn editColumn = new DataGridViewButtonColumn();
                editColumn.Name = "Action";
                editColumn.HeaderText = "Action";
                editColumn.Text = "Action";
                editColumn.UseColumnTextForButtonValue = true;
                editColumn.Width = 80;
                dataGridView1.Columns.Add(editColumn);
            }

            dataGridView1.Columns["Risk Status"].DisplayIndex = dataGridView1.Columns.Count - 1;

            // Rename and rearrange columns
            dataGridView1.Columns["student_no"].HeaderText = "Student NO.";
            dataGridView1.Columns["student_name"].HeaderText = "Student Name";
            dataGridView1.Columns["attendance_rate"].HeaderText = "Attendance Rate";
            dataGridView1.Columns["violations"].HeaderText = "Violations";

            // Base styling (your existing code)
            dataGridView1.Font = new Font("Century Gothic", 12f, FontStyle.Regular);
            dataGridView1.RowTemplate.Height = 35; // Taller rows for better readability
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.RowHeadersVisible = false;

            //center text
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;

            dataGridView1.DefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;

            // Alternating row colors
            dataGridView1.RowsDefaultCellStyle.BackColor = Color.White;
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);

            // Header style
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Century Gothic", 13f, FontStyle.Bold);
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.EnableHeadersVisualStyles = false;

            // Cell style
            dataGridView1.DefaultCellStyle.Font = new Font("Century Gothic", 12f);
            dataGridView1.DefaultCellStyle.Padding = new Padding(5);
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.FromArgb(100, 149, 237); // CornflowerBlue
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.White;

            // Grid lines
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridView1.GridColor = Color.Gainsboro;

            // REMOVE BLUE SELECTION SA CELLS / ROWS
            dataGridView1.DefaultCellStyle.SelectionBackColor =
                dataGridView1.DefaultCellStyle.BackColor;

            dataGridView1.DefaultCellStyle.SelectionForeColor =
                dataGridView1.DefaultCellStyle.ForeColor;

            // Disable row headers
            dataGridView1.RowHeadersVisible = false;

        }
        private string ComputeRiskStatus(float grade, float attendance, int violations)
        {
            if (grade <= 0)
                return "Pending";

            // Academic failure = automatic High Risk
            if (grade < 75 || attendance < 75)
                return "High Risk";

            // Behavioral levels (if academics are good)
            if (violations >= 6)
                return "High Risk";

            if (violations == 5)
                return "Warning";

            if (violations >= 1 && violations <= 4)
                return "Low Risk";

            // Slight academic concern
            if ((grade >= 76 && grade < 80) ||
                (attendance >= 76 && attendance < 80))
                return "Warning";

            return "Low Risk";
        }

        //private string ComputeRiskStatus(float gpa, float attendance, int violations)
        //{
        //    if (gpa < 75 || attendance < 75 || violations >= 3)
        //        return "High Risk";

        //    else if ((gpa >= 76 && gpa < 80) ||
        //             (attendance >= 76 && attendance < 80) ||
        //             (violations == 1 || violations == 2))
        //        return "Warning";

        //    return "Low Risk";
        //}

        private float SafeFloat(object value)
        {
            float f;
            return float.TryParse(value.ToString(), out f) ? f : 0;
        }

        private int SafeInt(object value)
        {
            int i;
            return int.TryParse(value.ToString(), out i) ? i : 0;
        }

        private void FormatGrid()
        {
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
            dataGridView1.EnableHeadersVisualStyles = false;

            dataGridView1.DefaultCellStyle.Padding = new Padding(5);
            dataGridView1.RowTemplate.Height = 35;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.CellFormatting += DataGridView1_CellFormatting;
        }

        private void DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].HeaderText == "Risk Status")
            {
                string status = e.Value?.ToString();

                e.CellStyle.Font = new Font("Century Gothic", 12f);

                if (status == "Low Risk")
                {
                    e.CellStyle.BackColor = Color.Green;
                    e.CellStyle.ForeColor = Color.White;
                }
                else if (status == "Warning")
                {
                    e.CellStyle.BackColor = Color.Gold;
                    e.CellStyle.ForeColor = Color.Black;
                }
                else if (status == "High Risk")
                {
                    e.CellStyle.BackColor = Color.Maroon;
                    e.CellStyle.ForeColor = Color.White;
                }
            }
        }
        private void LoadYearLevels()
        {
            yearlevel.Items.Clear();
            yearlevel.Items.Add("ALL");

            string query = @"
        SELECT DISTINCT year_level 
        FROM students 
        WHERE year_level IS NOT NULL 
          AND TRIM(year_level) <> ''
        ORDER BY year_level";

            using (MySqlConnection conn = new MySqlConnection(DBConfig.ConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    yearlevel.Items.Add(dr["year_level"].ToString().Trim());
                }
            }

            yearlevel.SelectedIndex = 0;
        }



        private void LoadStrands()
        {
            guna2ComboBox1.Items.Clear();
            guna2ComboBox1.Items.Add("ALL");

            using (MySqlConnection con = new MySqlConnection(DBConfig.ConnectionString))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand(
                    "SELECT DISTINCT strand FROM students ORDER BY strand", con);

                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    guna2ComboBox1.Items.Add(dr["strand"].ToString());
                }
            }

            guna2ComboBox1.SelectedIndex = 0; // default ALL
        }
        private void LoadSections(string selectedStrand)
        {
            guna2ComboBox2.Items.Clear();
            guna2ComboBox2.Items.Add("ALL");

            using (MySqlConnection con = new MySqlConnection(DBConfig.ConnectionString))
            {
                con.Open();

                string query = "SELECT DISTINCT section FROM students";
                if (selectedStrand != "ALL")
                    query += " WHERE strand=@strand";

                MySqlCommand cmd = new MySqlCommand(query, con);

                if (selectedStrand != "ALL")
                    cmd.Parameters.AddWithValue("@strand", selectedStrand);

                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    guna2ComboBox2.Items.Add(dr["section"].ToString());
                }
            }

            guna2ComboBox2.SelectedIndex = 0; // default ALL
        }


        private void cmbView_SelectedIndexChanged(object sender, EventArgs e)
        {
            gradeType = cmbView.Text;
            LoadStudents(search.Text.Trim());
        }

        private void yearlevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isInitializing) return;

            year = yearlevel.Text == "ALL" ? "" : yearlevel.Text;
            LoadStudents(search.Text.Trim());
        }


        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isInitializing) return;

            strand = guna2ComboBox1.Text == "ALL" ? "" : guna2ComboBox1.Text;
            LoadSections(guna2ComboBox1.Text);
            section = "";
            LoadStudents(search.Text.Trim());
        }

        private void guna2ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isInitializing) return;

            section = guna2ComboBox2.Text == "ALL" ? "" : guna2ComboBox2.Text;
            LoadStudents(search.Text.Trim());
        }
        private void guna2Button4_Click(object sender, EventArgs e)
        {

        }


        private void export_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("No data to export.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel File (*.csv)|*.csv";
            sfd.FileName = "Student_Report.csv";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    StringBuilder sb = new StringBuilder();

                    // ===== HEADERS =====
                    for (int i = 0; i < dataGridView1.Columns.Count; i++)
                    {
                        if (dataGridView1.Columns[i].Name != "Action")
                        {
                            sb.Append(dataGridView1.Columns[i].HeaderText);
                            sb.Append(",");
                        }
                    }
                    sb.AppendLine();

                    // ===== ROWS =====
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (!row.IsNewRow)
                        {
                            for (int i = 0; i < dataGridView1.Columns.Count; i++)
                            {
                                if (dataGridView1.Columns[i].Name != "Action")
                                {
                                    sb.Append(row.Cells[i].Value?.ToString());
                                    sb.Append(",");
                                }
                            }
                            sb.AppendLine();
                        }
                    }

                    File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);

                    MessageBox.Show("Export successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Export failed: " + ex.Message);
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        { 
                if (e.ColumnIndex == dataGridView1.Columns["Action"].Index)
            {
                string studentNo = dataGridView1.Rows[e.RowIndex].Cells["student_no"].Value.ToString();
                string name = dataGridView1.Rows[e.RowIndex].Cells["student_name"].Value.ToString();
                string risk = dataGridView1.Rows[e.RowIndex].Cells["Risk Status"].Value.ToString();

                float gpa = SafeFloat(dataGridView1.Rows[e.RowIndex].Cells["grade"].Value);
                float attendance = SafeFloat(dataGridView1.Rows[e.RowIndex].Cells["attendance_rate"].Value);
                int violations = SafeInt(dataGridView1.Rows[e.RowIndex].Cells["violations"].Value);

                string reason = "";
                string solution = "";

                if (gpa < 75)
                {
                    reason = "• GPA is below 75.\n";
                    solution = "• Attend tutorial classes and review lessons.\n";
                }
                else if (attendance < 75)
                {
                    reason = "• Attendance rate is below 75%.\n";
                    solution = "• Improve attendance, join class regularly.\n";
                }
                else if (violations >= 3)
                {
                    reason = "• Violations reached 3 or more.\n";
                    solution = "• Follow school rules and improve behavior.\n";
                }
                else
                {
                    reason = "• No issues found.\n";
                    solution = "• Keep up the good work.\n";
                }

                MessageBox.Show(
                    $"Student: {name}\n\nRisk: {risk}\n\nReason:\n{reason}\n\nSolution:\n{solution}",
                    "Risk Details",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
        }
    }
}
