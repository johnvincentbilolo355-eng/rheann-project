using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static System.Collections.Specialized.BitVector32;

namespace An_Early_Warning_System_for_Student
{
    public partial class Dashboard : Form
    {
        string connStr = DBConfig.ConnectionString;
        private string teacherStrand;
        private string teacherSection;
        private string teacherYearLevel;

        public Dashboard(string strand, string section, string yearLevel)
        {
            InitializeComponent();
            teacherStrand = strand;
            teacherSection = section;
            teacherYearLevel = yearLevel; // important
            dgvRiskAnalytics.CellFormatting += dgvRiskAnalytics_CellFormatting;
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            LoadDashboardStats();

            LoadStudentRiskAnalytics("MIDTERM");

            LoadHighRiskCountFromGrid(); // 👈 AFTER grid loads

            cmbView.Items.Add("MIDTERM");
            cmbView.Items.Add("FINALS");
            cmbView.Items.Add("GPA");

            cmbView.SelectedItem = "MIDTERM";
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void dgvRiskAnalytics_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void StyleDataGridView()
        {
            // Font
            dgvRiskAnalytics.Font = new Font("Century Gothic", 12f, FontStyle.Regular);
            dgvRiskAnalytics.RowTemplate.Height = 35; // Taller rows for better readability
            dgvRiskAnalytics.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvRiskAnalytics.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dgvRiskAnalytics.EnableHeadersVisualStyles = false;
            dgvRiskAnalytics.RowHeadersVisible = false;

            //center text
            dgvRiskAnalytics.ColumnHeadersDefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;

            dgvRiskAnalytics.DefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;

            // Alternating row colors
            dgvRiskAnalytics.RowsDefaultCellStyle.BackColor = Color.White;
            dgvRiskAnalytics.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);

            // Header style
            dgvRiskAnalytics.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);
            dgvRiskAnalytics.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dgvRiskAnalytics.ColumnHeadersDefaultCellStyle.Font = new Font("Century Gothic", 13f, FontStyle.Bold);
            dgvRiskAnalytics.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvRiskAnalytics.EnableHeadersVisualStyles = false;

            // Cell style
            dgvRiskAnalytics.DefaultCellStyle.Font = new Font("Century Gothic", 12f);
            dgvRiskAnalytics.DefaultCellStyle.Padding = new Padding(5);
            dgvRiskAnalytics.DefaultCellStyle.SelectionBackColor = Color.FromArgb(100, 149, 237); // CornflowerBlue
            dgvRiskAnalytics.DefaultCellStyle.SelectionForeColor = Color.White;

            // Grid lines
            dgvRiskAnalytics.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvRiskAnalytics.GridColor = Color.Gainsboro;

            dgvRiskAnalytics.DefaultCellStyle.SelectionBackColor =
           dgvRiskAnalytics.DefaultCellStyle.BackColor;

            dgvRiskAnalytics.DefaultCellStyle.SelectionForeColor =
                dgvRiskAnalytics.DefaultCellStyle.ForeColor;

            // Disable row headers
            dgvRiskAnalytics.RowHeadersVisible = false;
        }
        private void LoadStudentRiskAnalytics(string mode)
        {
            string gradeColumn = "gpa";
            string gradeHeader = "GPA";

            if (mode == "MIDTERM")
            {
                gradeColumn = "midterm_grade";
                gradeHeader = "Midterm";
            }
            else if (mode == "FINALS")
            {
                gradeColumn = "final_grade";
                gradeHeader = "Finals";
            }
            else if (mode == "GPA")
            {
                gradeColumn = "gpa";
                gradeHeader = "GPA";
            }

            string query = $@"
                    SELECT
                        s.student_no AS 'Student No',
                        s.student_name AS 'Student Name',
                        sub.{gradeColumn} AS '{gradeHeader}',
                        sub.attendance_rate AS 'Attendance',
                        sub.absences AS 'Absences',
                        s.violations AS 'Violations'
                    FROM students s
                                        LEFT JOIN subjects sub ON s.student_no = sub.student_no
                                        WHERE UPPER(TRIM(CAST(s.strand AS CHAR))) = UPPER(TRIM(@strand))
                                            AND REPLACE(TRIM(CAST(s.section AS CHAR)), '.0', '') = REPLACE(TRIM(@section), '.0', '')
                                            AND (
                                                        REPLACE(TRIM(CAST(s.year_level AS CHAR)), '.0', '') = REPLACE(TRIM(@yearlevel), '.0', '')
                                                        OR TRIM(CAST(s.year_level AS CHAR)) LIKE CONCAT('%', TRIM(@yearlevel), '%')
                                                    )
                    ";

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                da.SelectCommand.Parameters.AddWithValue("@strand", teacherStrand);
                da.SelectCommand.Parameters.AddWithValue("@section", teacherSection);
                da.SelectCommand.Parameters.AddWithValue("@yearlevel", teacherYearLevel);

                DataTable dt = new DataTable();
                da.Fill(dt);

                dt.Columns.Add("Risk Status", typeof(string));
                foreach (DataRow row in dt.Rows)
                {
                    decimal grade = row[gradeHeader] == DBNull.Value ? 0 : Convert.ToDecimal(row[gradeHeader]);
                    decimal attendance = row["Attendance"] == DBNull.Value ? 0 : Convert.ToDecimal(row["Attendance"]);

                    // Kung wala pang violations sa DB, wag isali ang absences sa risk
                    int violations = row["Violations"] == DBNull.Value ? 0 : Convert.ToInt32(row["Violations"]);

                    row["Risk Status"] = ComputeRiskStatus(grade, attendance, violations);
                }

                dgvRiskAnalytics.DataSource = dt;
                StyleDataGridView();
            }
        }

        private string ComputeRiskStatus(decimal grade, decimal attendance, int violations)
        {
            if (grade <= 0)
                return "Pending";

            // Academic Critical
            if (grade < 75 || attendance < 75)
                return "High Risk";

            // Behavioral Critical
            if (violations >= 6)
                return "High Risk";

            // Warning Levels
            if ((grade >= 75 && grade < 80) ||
                (attendance >= 75 && attendance < 80) ||
                violations == 5)
                return "Warning";

            if (violations >= 1 && violations <= 4)
                return "Low Risk";

            return "Low Risk";
        }





        private void dgvRiskAnalytics_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvRiskAnalytics.Columns[e.ColumnIndex].HeaderText == "Risk Status")
            {
                string status = e.Value?.ToString();

                if (status == "High Risk")
                {
                    e.CellStyle.BackColor = Color.Maroon; // Bright red for high risk
                    e.CellStyle.ForeColor = Color.White;
                }
                else if (status == "Warning")
                {
                    e.CellStyle.BackColor = Color.Orange; // Orange is clear but not harsh
                    e.CellStyle.ForeColor = Color.Black;
                }
                else if (status == "Low Risk")
                {
                    e.CellStyle.BackColor = Color.LightGreen; // Soft green for safe
                    e.CellStyle.ForeColor = Color.Black;
                }
            }
        }
        private void LoadHighRiskCountFromGrid()
        {
            int highRiskCount = 0;

            foreach (DataGridViewRow row in dgvRiskAnalytics.Rows)
            {
                if (!row.IsNewRow)
                {
                    string risk = row.Cells["Risk Status"].Value?.ToString();

                    if (risk == "High Risk")
                    {
                        highRiskCount++;
                    }
                }
            }

            lblHighRisk.Text = highRiskCount.ToString();
        }
        private void LoadDashboardStats()
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string query = @"
                    SELECT 
                        COUNT(DISTINCT s.student_no) AS TotalStudents,
                        COALESCE(ROUND(AVG(sub.attendance_rate), 2), 0) AS AvgAttendance,
                        COALESCE(ROUND(AVG(sub.gpa), 2), 0) AS ClassAverage
                    FROM students s
                    LEFT JOIN subjects sub ON s.student_no = sub.student_no
                                        WHERE UPPER(TRIM(CAST(s.strand AS CHAR))) = UPPER(TRIM(@strand))
                                            AND REPLACE(TRIM(CAST(s.section AS CHAR)), '.0', '') = REPLACE(TRIM(@section), '.0', '')
                                            AND (
                                                        REPLACE(TRIM(CAST(s.year_level AS CHAR)), '.0', '') = REPLACE(TRIM(@yearlevel), '.0', '')
                                                        OR TRIM(CAST(s.year_level AS CHAR)) LIKE CONCAT('%', TRIM(@yearlevel), '%')
                                                    )
                ";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@strand", teacherStrand);
                cmd.Parameters.AddWithValue("@section", teacherSection);
                cmd.Parameters.AddWithValue("@yearlevel", teacherYearLevel);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        lblTotalStudents.Text = reader["TotalStudents"].ToString();
                        lblAttendRate.Text = reader["AvgAttendance"].ToString() + " %";
                        lblClassAverage.Text = reader["ClassAverage"].ToString() + " %";
                    }
                }
            }
        }


        private void cmbView_SelectedIndexChanged(object sender, EventArgs e)
        {
            string view = cmbView.SelectedItem.ToString();

            LoadStudentRiskAnalytics(view);

            // IMPORTANT: tawagin AFTER mag-load ng grid
            LoadHighRiskCountFromGrid();
        }

        private void lblClassAverage_Click(object sender, EventArgs e)
        {

        }

        private void lblHighRisk_Click(object sender, EventArgs e)
        {

        }
    }
}