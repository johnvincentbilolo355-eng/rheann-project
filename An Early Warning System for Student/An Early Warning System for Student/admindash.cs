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
using System.Windows.Forms.DataVisualization.Charting;

namespace An_Early_Warning_System_for_Student
{
    public partial class admindash : Form
    {

        string connStr = DBConfig.ConnectionString;
        public admindash()
        {
            InitializeComponent();
            dgvRiskAnalytics.CellFormatting += dgvRiskAnalytics_CellFormatting;
        }

        private void admindash_Load(object sender, EventArgs e)
        {
            LoadStudentRiskAnalytics();
            UpdateHighRiskLabelFromGrid();
            //LoadHighRiskCount();
            LoadDashboardStats();
            LoadStudentStatusChart(); // ✅ ADD THIS
        }

        //private void LoadHighRiskCount()
        //{
        //    using (MySqlConnection conn = new MySqlConnection(DBConfig.ConnectionString))
        //    {
        //        conn.Open();

        //        string query = @"
        //    SELECT COUNT(DISTINCT s.student_no)
        //    FROM students s
        //    JOIN subjects sub ON s.student_no = sub.student_no
        //    WHERE sub.gpa < 75 
        //       OR sub.attendance_rate < 75 
        //       OR s.violations >= 3;
        //";

        //        MySqlCommand cmd = new MySqlCommand(query, conn);
        //        lblHighRisk.Text = cmd.ExecuteScalar().ToString();
        //    }
        //}
        private void LoadDashboardStats()
        {
            using (MySqlConnection conn = new MySqlConnection(DBConfig.ConnectionString))
            {
                conn.Open();

                // Query for total students and total teachers
                string query = @"
            SELECT 
                (SELECT COUNT(*) FROM students) AS TotalStudents,
                (SELECT COUNT(*) FROM users) AS TotalTeachers
        ";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        lblTotalStudents.Text = reader["TotalStudents"].ToString();
                        lblTotalTC.Text = reader["TotalTeachers"].ToString(); // total teachers
                    }
                }
            }
        }


        private void LoadStudentStatusChart()
        {
            using (MySqlConnection conn = new MySqlConnection(DBConfig.ConnectionString))
            {
                conn.Open();

                // Fetch raw student data
                string query = @"
            SELECT
                sub.gpa,
                sub.attendance_rate,
                s.violations
            FROM students s
            JOIN subjects sub ON s.student_no = sub.student_no;
        ";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                int safe = 0, warning = 0, atRisk = 0;

                while (reader.Read())
                {
                    float grade = reader["gpa"] == DBNull.Value ? 0f : Convert.ToSingle(reader["gpa"]);
                    float attendance = reader["attendance_rate"] == DBNull.Value ? 0f : Convert.ToSingle(reader["attendance_rate"]);
                    int violations = reader["violations"] == DBNull.Value ? 0 : Convert.ToInt32(reader["violations"]);

                    // Use ComputeRiskStatus
                    string risk = ComputeRiskStatus(grade, attendance, violations);

                    switch (risk)
                    {
                        case "High Risk":
                            atRisk++;
                            break;
                        case "Warning":
                            warning++;
                            break;
                        case "Low Risk":
                            safe++;
                            break;
                            // Ignore Pending
                    }
                }

                // Clear chart
                chart1.Series.Clear();
                chart1.ChartAreas.Clear();
                chart1.Legends.Clear();
                chart1.Titles.Clear();

                // Chart Area
                ChartArea area = new ChartArea();
                area.BackColor = Color.White;
                chart1.ChartAreas.Add(area);

                // Series
                Series series = new Series("Student Status");
                series.ChartType = SeriesChartType.Doughnut;
                series["DoughnutRadius"] = "60";
                series.IsValueShownAsLabel = false;

                // Add points
                series.Points.AddXY("Safe", safe);
                series.Points.AddXY("Warning", warning);
                series.Points.AddXY("At-Risk", atRisk);

                // Colors and legend text
                foreach (DataPoint point in series.Points)
                {
                    switch (point.AxisLabel)
                    {
                        case "Safe": point.Color = Color.FromArgb(6, 161, 17); break;
                        case "Warning": point.Color = Color.FromArgb(214, 156, 21); break;
                        case "At-Risk": point.Color = Color.FromArgb(171, 9, 30); break;
                    }

                    point.LegendText = $"{point.AxisLabel}   {point.YValues[0]}";

                    // Hide labels inside donut
                    point.Label = "";
                    point.IsValueShownAsLabel = false;
                    point.LabelForeColor = Color.Transparent;
                }

                chart1.Series.Add(series);

                // Legend
                Legend legend = new Legend();
                legend.Docking = Docking.Right;
                legend.Alignment = StringAlignment.Center;
                legend.Font = new Font("Century Gothic", 10);
                legend.IsTextAutoFit = false;
                chart1.Legends.Add(legend);

                // Title
                chart1.Titles.Add("Student Status Overview");
                chart1.Titles[0].Font = new Font("Century Gothic", 14, FontStyle.Bold);
            }
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

            // REMOVE BLUE SELECTION SA CELLS / ROWS
            dgvRiskAnalytics.DefaultCellStyle.SelectionBackColor =
                dgvRiskAnalytics.DefaultCellStyle.BackColor;

            dgvRiskAnalytics.DefaultCellStyle.SelectionForeColor =
                dgvRiskAnalytics.DefaultCellStyle.ForeColor;


            // Grid lines
            dgvRiskAnalytics.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvRiskAnalytics.GridColor = Color.Gainsboro;
            //center text
            dgvRiskAnalytics.ColumnHeadersDefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;

            dgvRiskAnalytics.DefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;

            // Disable row headers
            dgvRiskAnalytics.RowHeadersVisible = false;
        }
        private void LoadStudentRiskAnalytics()
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                // Fetch raw student data
                string query = @"
            SELECT
                s.student_no AS 'Student No',
                s.student_name AS 'Student Name',
                sub.gpa AS GPA,
                sub.attendance_rate AS 'Attendance %',
                s.violations AS Violations
            FROM students s
            JOIN subjects sub ON s.student_no = sub.student_no
            ORDER BY sub.attendance_rate ASC;
        ";

                MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Add Risk Status column
                dt.Columns.Add("Risk Status", typeof(string));

                foreach (DataRow row in dt.Rows)
                {
                    float grade = row["GPA"] == DBNull.Value ? 0f : Convert.ToSingle(row["GPA"]);
                    float attendance = row["Attendance %"] == DBNull.Value ? 0f : Convert.ToSingle(row["Attendance %"]);
                    int violations = row["Violations"] == DBNull.Value ? 0 : Convert.ToInt32(row["Violations"]);

                    // Use ComputeRiskStatus method
                    row["Risk Status"] = ComputeRiskStatus(grade, attendance, violations);
                }

                dgvRiskAnalytics.DataSource = dt;

                // Apply styling
                StyleDataGridView();
            }
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

        private void chart1_Click(object sender, EventArgs e)
        {

        }
        private void UpdateHighRiskLabelFromGrid()
        {
            int highRiskCount = 0;

            foreach (DataGridViewRow row in dgvRiskAnalytics.Rows)
            {
                if (row.Cells["Risk Status"].Value != null &&
                    row.Cells["Risk Status"].Value.ToString() == "High Risk")
                {
                    highRiskCount++;
                }
            }

            lblHighRisk.Text = highRiskCount.ToString();
        }

        private void lblHighRisk_Click(object sender, EventArgs e)
        {

        }
    }
}

