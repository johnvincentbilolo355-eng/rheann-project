using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace An_Early_Warning_System_for_Student
{
    public partial class SubjectClass : Form
    {
        string strand, section;
        bool isInitializing = true;
        string teacherId;
        public string LoggedInId { get; private set; }
        string yearLevel = ""; // stores the selected year level

        public SubjectClass(string teacherId, string strand = "", string section = "")
        {
            InitializeComponent();
            this.teacherId = teacherId;
            this.strand = strand;
            this.section = section;
        }

        private void SubjectClass_Load(object sender, EventArgs e)
        {
            LoadYearLevels();
            LoadClassesForTeacher();

            LoadStudents(); // auto load filtered agad

            FormatGrid();

            cmbView.Items.Clear();
            cmbView.Items.Add("MIDTERM");
            cmbView.Items.Add("FINALS");
            cmbView.Items.Add("GPA");
            cmbView.SelectedItem = "MIDTERM";
        }

        // ----------- RISK STATUS COMPUTATION -------------
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

        // ----------- LOAD FILTERED STUDENTS -------------
        string gradeType = "MIDTERM";  // default


        private void LoadStudents(string keyword = "")
        {
            string gradeExpr = "AVG(sub.midterm_grade)";
            if (gradeType == "FINALS")
                gradeExpr = "AVG(sub.final_grade)";
            else if (gradeType == "GPA")
                gradeExpr = "AVG(sub.gpa)";

            string query = $@"
                        SELECT 
                            s.student_no,
                            s.student_name,
                            COALESCE({gradeExpr}, 0) AS grade,
                            COALESCE(AVG(sub.attendance_rate), 0) AS attendance_rate,
                            COALESCE(s.violations, 0) AS violations,
                            s.strand,
                            s.section,
                            s.year_level,
                            COALESCE(AVG(sub.gpa), 0) AS gwa,
                            COALESCE(SUM(CASE WHEN sub.gpa < 75 THEN 1 ELSE 0 END), 0) AS low_subject_count,
                            COALESCE(SUM(CASE WHEN sub.gpa >= 75 AND sub.gpa < 85 THEN 1 ELSE 0 END), 0) AS borderline_subject_count,
                            COALESCE(SUM(CASE WHEN sub.attendance_rate < 75 THEN 1 ELSE 0 END), 0) AS low_attendance_subject_count,
                            COALESCE(MIN(sub.attendance_rate), 0) AS min_attendance_rate
                        FROM students s
                        LEFT JOIN subjects sub ON sub.student_no = s.student_no
                        WHERE EXISTS (
                            SELECT 1
                            FROM student_classes sc
                            INNER JOIN teacher_classes tc
                                ON sc.strand = s.strand
                               AND sc.section = s.section
                               AND sc.year_level = s.year_level
                               AND tc.teacher_id = @teacherId
                            WHERE sc.student_no = s.student_no
                        )
                          AND (@strand='' OR s.strand=@strand)
                          AND (@section='' OR s.section=@section)
                          AND (@yearlevel='' OR s.year_level=@yearlevel)
                          AND (@keyword='' OR s.student_name LIKE @keyword OR s.student_no LIKE @keyword)
                        GROUP BY s.student_no, s.student_name, s.violations, s.strand, s.section, s.year_level
                        ORDER BY s.student_name ASC
                    ";

            using (MySqlConnection con = new MySqlConnection(DBConfig.ConnectionString))
            using (MySqlCommand cmd = new MySqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@teacherId", teacherId);
                cmd.Parameters.AddWithValue("@strand", strand == "ALL" ? "" : strand);
                cmd.Parameters.AddWithValue("@section", section == "ALL" ? "" : section);
                cmd.Parameters.AddWithValue("@yearlevel", yearLevel == "ALL" ? "" : yearLevel);
                cmd.Parameters.AddWithValue("@keyword", string.IsNullOrEmpty(keyword) ? "" : "%" + keyword + "%");

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Add Risk Status
                if (!dt.Columns.Contains("Risk Status"))
                    dt.Columns.Add("Risk Status", typeof(string));

                var aprioriModel = AprioriRiskModel.TryLoadDefault();

                foreach (DataRow row in dt.Rows)
                {
                    float grade = SafeFloat(row["grade"]);
                    float attendance = SafeFloat(row["attendance_rate"]);
                    int violations = SafeInt(row["violations"]);

                    float gwa = dt.Columns.Contains("gwa") ? SafeFloat(row["gwa"]) : grade;
                    int lowSubjectCount = dt.Columns.Contains("low_subject_count") ? SafeInt(row["low_subject_count"]) : 0;
                    int borderlineSubjectCount = dt.Columns.Contains("borderline_subject_count") ? SafeInt(row["borderline_subject_count"]) : 0;
                    float minAttendance = dt.Columns.Contains("min_attendance_rate") ? SafeFloat(row["min_attendance_rate"]) : attendance;

                    // Training-data-driven probability (Apriori confidence). Falls back to legacy heuristics if rules aren't available.
                    double prob = 0.0;
                    if (aprioriModel != null)
                    {
                        var eval = aprioriModel.Evaluate(gwa, attendance, lowSubjectCount, borderlineSubjectCount);
                        prob = eval.Probability;
                    }

                    bool hasFailedSubject = lowSubjectCount > 0;
                    bool hasLowAttendance = minAttendance > 0 && minAttendance < 75;

                    string status;
                    if (grade <= 0 && gwa <= 0)
                    {
                        status = "Pending";
                    }
                    else if (hasFailedSubject || (gwa > 0 && gwa < 75) || hasLowAttendance)
                    {
                        status = "High Risk";
                    }
                    else if (prob >= 0.70)
                    {
                        status = "High Risk";
                    }
                    else if (prob >= 0.50)
                    {
                        status = "Warning";
                    }
                    else
                    {
                        // If model isn't available, retain the old behavior
                        status = aprioriModel == null
                            ? ComputeRiskStatus(grade, attendance, violations)
                            : "Low Risk";
                    }

                    row["Risk Status"] = status;
                }

                dataGridView1.DataSource = dt;
                dataGridView1.AutoGenerateColumns = true;

                // Hide model-feature columns (keeps the existing UI)
                if (dataGridView1.Columns.Contains("gwa")) dataGridView1.Columns["gwa"].Visible = false;
                if (dataGridView1.Columns.Contains("low_subject_count")) dataGridView1.Columns["low_subject_count"].Visible = false;
                if (dataGridView1.Columns.Contains("borderline_subject_count")) dataGridView1.Columns["borderline_subject_count"].Visible = false;
                if (dataGridView1.Columns.Contains("low_attendance_subject_count")) dataGridView1.Columns["low_attendance_subject_count"].Visible = false;
                if (dataGridView1.Columns.Contains("min_attendance_rate")) dataGridView1.Columns["min_attendance_rate"].Visible = false;




                // Rename columns
                dataGridView1.Columns["student_no"].HeaderText = "Student NO.";
                dataGridView1.Columns["student_name"].HeaderText = "Student Name";
                dataGridView1.Columns["grade"].HeaderText = gradeType; // important
                dataGridView1.Columns["attendance_rate"].HeaderText = "Attendance Rate";
                dataGridView1.Columns["violations"].HeaderText = "Violations";
                dataGridView1.Columns["strand"].HeaderText = "Strand";
                dataGridView1.Columns["section"].HeaderText = "Section";
                dataGridView1.Columns["year_level"].HeaderText = "Year Level";
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

            // ===== COLUMN ORDER =====

            // Action pinaka-una
            dataGridView1.Columns["Action"].DisplayIndex = 0;

            dataGridView1.Columns["student_no"].DisplayIndex = 1;
            dataGridView1.Columns["student_name"].DisplayIndex = 2;
            dataGridView1.Columns["year_level"].DisplayIndex = 3;
            dataGridView1.Columns["strand"].DisplayIndex = 4;
            dataGridView1.Columns["section"].DisplayIndex = 5;
            dataGridView1.Columns["grade"].DisplayIndex = 6;
            dataGridView1.Columns["attendance_rate"].DisplayIndex = 7;
            dataGridView1.Columns["violations"].DisplayIndex = 8;
            dataGridView1.Columns["Risk Status"].DisplayIndex = 9;


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




        // ----------- SAFE VALUE METHODS (para iwas error) -------------
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

        // ----------- GRID UI FORMAT -------------
        private void FormatGrid()
        {
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
            dataGridView1.EnableHeadersVisualStyles = false;

            dataGridView1.DefaultCellStyle.Padding = new Padding(5);
            dataGridView1.RowTemplate.Height = 35;

            dataGridView1.CellFormatting += DataGridView1_CellFormatting;
        }

        // ----------- COLOR CODING FOR RISK STATUS -------------
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

        private void button1_Click(object sender, EventArgs e)
        {
            GradeComponent grade = new GradeComponent(LoggedInId);
            grade.Show();
        }
        private void LoadClassesForTeacher()
        {
            isInitializing = true;

            guna2ComboBox1.Items.Clear();

            using (MySqlConnection con = new MySqlConnection(DBConfig.ConnectionString))
            {
                con.Open();

                string query = @"
        SELECT DISTINCT c.strand, c.section
        FROM classes c
        INNER JOIN teacher_classes tc ON c.class_id = tc.class_id
        WHERE tc.teacher_id = @teacherId
        ORDER BY c.strand, c.section";

                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@teacherId", teacherId);

                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            string item = $"{dr["strand"]} - {dr["section"]}";
                            guna2ComboBox1.Items.Add(item);
                        }
                    }
                }
            }

            // 🔥 select first subject automatically
            if (guna2ComboBox1.Items.Count > 0)
            {
                guna2ComboBox1.SelectedIndex = 0;

                string[] parts = guna2ComboBox1.SelectedItem.ToString().Split(" - ");
                strand = parts[0];
                section = parts[1];
            }

            isInitializing = false;
        }

        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (guna2ComboBox1.SelectedItem == null) return;

            string[] parts = guna2ComboBox1.SelectedItem.ToString().Split(" - ");
            if (parts.Length < 2) return;

            strand = parts[0];
            section = parts[1];

            LoadStudents(search.Text.Trim());
        }


        private void LoadStrands()
        {
            isInitializing = true; // prevent SelectedIndexChanged from firing during load
            guna2ComboBox1.Items.Clear();
            guna2ComboBox1.Items.Add("ALL"); // optional: show all classes

            using (MySqlConnection con = new MySqlConnection(DBConfig.ConnectionString))
            {
                con.Open();

                // Get only strand-section combos that this teacher handles
                string query = @"
            SELECT DISTINCT c.strand, c.section
            FROM classes c
            INNER JOIN teacher_classes tc ON c.class_id = tc.class_id
            WHERE tc.teacher_id = @teacherId
            ORDER BY c.strand, c.section
        ";

                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@teacherId", teacherId);

                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            string item = $"{dr["strand"]} - {dr["section"]}";
                            guna2ComboBox1.Items.Add(item);
                        }
                    }
                }
            }

            // Select ALL by default
            guna2ComboBox1.SelectedIndex = 0;
            isInitializing = false; // allow selection changes now
        }


        private void guna2Button4_Click(object sender, EventArgs e)
        {

        }

        private void btnsearch_Click(object sender, EventArgs e)
        {
        }

        private void search_TextChanged(object sender, EventArgs e)
        {

        }

        private void cmbView_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void export_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void guna2ComboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (isInitializing) return;

            if (guna2ComboBox1.SelectedItem.ToString() == "ALL")
            {
                strand = "";
                section = "";
            }
            else
            {
                string[] parts = guna2ComboBox1.SelectedItem.ToString().Split(" - ");
                strand = parts[0];
                section = parts[1];
            }

            LoadStudents(search.Text.Trim());
        }

        private void export_Click_1(object sender, EventArgs e)
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

        private void guna2Button4_Click_1(object sender, EventArgs e)
        {
            Reports RP = new Reports();
            RP.Show();
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dataGridView1.Columns["Action"].Index)
            {
                string studentNo = dataGridView1.Rows[e.RowIndex].Cells["student_no"].Value.ToString();
                string name = dataGridView1.Rows[e.RowIndex].Cells["student_name"].Value.ToString();
                string risk = dataGridView1.Rows[e.RowIndex].Cells["Risk Status"].Value.ToString();

                float gpa = SafeFloat(dataGridView1.Rows[e.RowIndex].Cells["grade"].Value);
                float attendance = SafeFloat(dataGridView1.Rows[e.RowIndex].Cells["attendance_rate"].Value);
                int violations = SafeInt(dataGridView1.Rows[e.RowIndex].Cells["violations"].Value);

                float gwa = dataGridView1.Columns.Contains("gwa")
                    ? SafeFloat(dataGridView1.Rows[e.RowIndex].Cells["gwa"].Value)
                    : gpa;

                int lowSubjectCount = dataGridView1.Columns.Contains("low_subject_count")
                    ? SafeInt(dataGridView1.Rows[e.RowIndex].Cells["low_subject_count"].Value)
                    : 0;

                int borderlineSubjectCount = dataGridView1.Columns.Contains("borderline_subject_count")
                    ? SafeInt(dataGridView1.Rows[e.RowIndex].Cells["borderline_subject_count"].Value)
                    : 0;

                int lowAttendanceSubjectCount = dataGridView1.Columns.Contains("low_attendance_subject_count")
                    ? SafeInt(dataGridView1.Rows[e.RowIndex].Cells["low_attendance_subject_count"].Value)
                    : 0;

                float minAttendance = dataGridView1.Columns.Contains("min_attendance_rate")
                    ? SafeFloat(dataGridView1.Rows[e.RowIndex].Cells["min_attendance_rate"].Value)
                    : attendance;

                string reason = "";
                string solution = "";

                // Pull failed subjects from DB (keeps data intact; just reads)
                List<string> failedSubjects = new List<string>();
                try
                {
                    using (MySqlConnection con = new MySqlConnection(DBConfig.ConnectionString))
                    using (MySqlCommand cmd = new MySqlCommand(
                        "SELECT subject_name, gpa FROM subjects WHERE student_no=@studentNo AND gpa IS NOT NULL AND gpa < 75 ORDER BY subject_name ASC", con))
                    {
                        cmd.Parameters.AddWithValue("@studentNo", studentNo);
                        con.Open();
                        using (var dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                string subject = dr["subject_name"].ToString();
                                string sgpa = dr["gpa"].ToString();
                                failedSubjects.Add($"• {subject} (GPA {sgpa})");
                            }
                        }
                    }
                }
                catch
                {
                    // If DB read fails, we still show the basic risk info.
                }

                var aprioriModel = AprioriRiskModel.TryLoadDefault();
                double prob = 0.0;
                string matchedRules = "";
                if (aprioriModel != null)
                {
                    var eval = aprioriModel.Evaluate(gwa, attendance, lowSubjectCount, borderlineSubjectCount);
                    prob = eval.Probability;
                    if (eval.MatchedAntecedents.Count > 0)
                        matchedRules = string.Join("\n", eval.MatchedAntecedents.Select(r => "• " + r));
                }

                var rf = PythonRfScorer.Score(
                    gwa,
                    lowSubjectCount,
                    borderlineSubjectCount,
                    lowAttendanceSubjectCount,
                    minAttendance
                );

                if (failedSubjects.Count > 0 || lowSubjectCount > 0)
                {
                    reason += "• One or more subjects are below 75.\n";
                    solution += "• Attend tutorial classes and review lessons for the failed subjects.\n";
                }

                if (minAttendance > 0 && minAttendance < 75)
                {
                    reason += "• Attendance rate is below 75%.\n";
                    solution += "• Improve attendance, join class regularly.\n";
                }

                if (violations >= 3)
                {
                    reason += "• Violations reached 3 or more.\n";
                    solution += "• Follow school rules and improve behavior.\n";
                }

                if (string.IsNullOrEmpty(reason))
                {
                    reason = "• No issues found.\n";
                    solution = "• Keep up the good work.\n";
                }

                string failedSubjectsText = failedSubjects.Count > 0
                    ? string.Join("\n", failedSubjects)
                    : "• None";

                string modelText = aprioriModel != null
                    ? $"\nModel (training rules) probability: {(prob * 100):0.#}%\n\nMatched rule tags:\n{(string.IsNullOrEmpty(matchedRules) ? "• None" : matchedRules)}"
                    : "";

                string rfText = rf.Ok
                    ? $"\n\nRF model probability: {(rf.Probability * 100):0.#}%"
                    : "";



                MessageBox.Show(
                    $"Student: {name}\n\nRisk: {risk}\n\nFailed subjects:\n{failedSubjectsText}\n\nReason:\n{reason}\n\nSolution:\n{solution}{modelText}{rfText}",
                    "Risk Details",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
        }

        private void cmbView_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            gradeType = cmbView.Text;
            LoadStudents(search.Text.Trim());
        }

        private void search_TextChanged_1(object sender, EventArgs e)
        {
            LoadStudents(search.Text.Trim());
        }

        private void yearlevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            yearLevel = yearlevel.SelectedItem.ToString();
            LoadStudents(search.Text.Trim());
        }
        private void LoadYearLevels()
        {
            yearlevel.Items.Clear();

            yearlevel.Items.Add("11");
            yearlevel.Items.Add("12");

            yearlevel.SelectedIndex = 0; // default agad 11
            yearLevel = "11"; // important para naka-filter agad
        }


    }
}

