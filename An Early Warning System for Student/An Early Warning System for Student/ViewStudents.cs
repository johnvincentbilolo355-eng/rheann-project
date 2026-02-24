using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace An_Early_Warning_System_for_Student
{
    public partial class ViewStudents : Form
    {
        bool isInitializing = true;

        private string year;
        private string strand;
        private string section;
        public string LoggedInId { get; private set; }

        public ViewStudents(string idNo, string year, string strand, string section)
        {
            InitializeComponent();
            this.year = year;
            this.strand = strand;
            this.section = section;
            LoggedInId = idNo;
        }
        private void ViewStudents_Load(object sender, EventArgs e)
        {
            isInitializing = true;

            // ===== YEAR =====
            yearlevel.Items.Clear();
            if (!string.IsNullOrEmpty(year) && year != "None")
            {
                yearlevel.Items.Add(year);
                yearlevel.SelectedIndex = 0;
                yearlevel.Enabled = false;
            }
            else
            {
                LoadYearLevels();
            }

            // ===== STRAND =====
            guna2ComboBox1.Items.Clear();
            if (!string.IsNullOrEmpty(strand) && strand != "None")
            {
                guna2ComboBox1.Items.Add(strand);
                guna2ComboBox1.SelectedIndex = 0;
                guna2ComboBox1.Enabled = false;
            }
            else
            {
                LoadStrands();
            }

            // ===== SECTION =====
            guna2ComboBox2.Items.Clear();
            if (!string.IsNullOrEmpty(section) && section != "None")
            {
                guna2ComboBox2.Items.Add(section);
                guna2ComboBox2.SelectedIndex = 0;
                guna2ComboBox2.Enabled = false;
            }
            else
            {
                LoadSections(strand ?? "ALL");
            }

            // ===== COMBOBOX VIEW (MIDTERM, FINAL, AVERAGE) =====
            cmbview.Items.Clear();
            cmbview.Items.Add("MIDTERM");
            cmbview.Items.Add("FINALS");
            cmbview.Items.Add("GPA");
            cmbview.SelectedItem = "MIDTERM";

            isInitializing = false;

            // ===== LOAD STUDENTS BASED ON FILTERS =====
            LoadStudents();
            FormatGrid();
        }




        private void LoadYearLevels()
        {
            yearlevel.Items.Clear();

            using (MySqlConnection con = new MySqlConnection(DBConfig.ConnectionString))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand(
                    "SELECT DISTINCT year_level FROM students ORDER BY year_level", con);

                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    yearlevel.Items.Add(dr["year_level"].ToString());
                }
            }
        }
        string gradeType = "MIDTERM";
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
        private DataTable allStudentsTable;
        private void LoadStudents(string keyword = "")
        {
            gradeType = cmbview.SelectedItem?.ToString() ?? "MIDTERM";
            string query = @"
        SELECT 
            st.student_no,
            st.student_name,
            sc.subject_name,
            sub.midterm_grade,
            sub.final_grade,
            sub.gpa,
            sub.attendance_rate,
            sub.absences,
            sc.teacher_id,
            st.violations
        FROM students st
        INNER JOIN student_classes sc
            ON sc.student_no = st.student_no
        LEFT JOIN subjects sub
            ON sub.student_no = st.student_no
           AND sub.subject_name = sc.subject_name
        WHERE (COALESCE(@year,'') = '' OR (
                    REPLACE(TRIM(CAST(st.year_level AS CHAR)), '.0', '') = REPLACE(TRIM(@year), '.0', '')
                 OR TRIM(CAST(st.year_level AS CHAR)) LIKE CONCAT('%', TRIM(@year), '%')
              ))
          AND (COALESCE(@strand,'') = '' OR UPPER(TRIM(CAST(st.strand AS CHAR))) = UPPER(TRIM(@strand)))
          AND (COALESCE(@section,'') = '' OR REPLACE(TRIM(CAST(st.section AS CHAR)), '.0', '') = REPLACE(TRIM(@section), '.0', ''))
          AND (COALESCE(@keyword,'') = '' OR st.student_name LIKE @keyword OR st.student_no LIKE @keyword)
        ORDER BY st.student_name ASC, sc.subject_name ASC
    ";

            using (MySqlConnection con = new MySqlConnection(DBConfig.ConnectionString))
            using (MySqlCommand cmd = new MySqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@year", string.IsNullOrEmpty(year) || year == "ALL" ? "" : year);
                cmd.Parameters.AddWithValue("@strand", string.IsNullOrEmpty(strand) || strand == "ALL" ? "" : strand);
                cmd.Parameters.AddWithValue("@section", string.IsNullOrEmpty(section) || section == "ALL" ? "" : section);
                cmd.Parameters.AddWithValue("@keyword", string.IsNullOrEmpty(keyword) ? "" : "%" + keyword + "%");

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Add Risk Status column if missing
                if (!dt.Columns.Contains("Risk Status"))
                    dt.Columns.Add("Risk Status", typeof(string));

                // Compute Risk Status
                foreach (DataRow row in dt.Rows)
                {
                    float studentGpa = SafeFloat(row["gpa"]);
                    float attendance = SafeFloat(row["attendance_rate"]);
                    int violations = SafeInt(row["violations"]);

                    row["Risk Status"] = ComputeRiskStatus(studentGpa, attendance, violations);
                }

                // ===== NEW: Grade Column Based on cmbview =====
                if (!dt.Columns.Contains("Grade"))
                    dt.Columns.Add("Grade", typeof(float));

                // Compute Risk Status based on selected grade type
                foreach (DataRow row in dt.Rows)
                {
                    float grade = 0f;

                    switch (gradeType)
                    {
                        case "MIDTERM":
                            grade = SafeFloat(row["midterm_grade"]);
                            break;
                        case "FINALS":
                            grade = SafeFloat(row["final_grade"]);
                            break;
                        case "GPA":
                            grade = SafeFloat(row["gpa"]);
                            break;
                    }

                    float attendance = SafeFloat(row["attendance_rate"]);
                    int violations = SafeInt(row["violations"]);

                    row["Risk Status"] = ComputeRiskStatus(grade, attendance, violations);

                    // Also set the dynamic Grade column
                    row["Grade"] = grade;
                }

                allStudentsTable = dt.Copy();
                dataGridView1.DataSource = allStudentsTable;
                LoadSubjectsCombo(dt);
                ApplySubjectFilter();
            }


            if (!dataGridView1.Columns.Contains("Archive"))
            {
                DataGridViewButtonColumn deleteColumn = new DataGridViewButtonColumn();
                deleteColumn.Name = "Archive";
                deleteColumn.HeaderText = "Archive";
                deleteColumn.Text = "Archive";
                deleteColumn.UseColumnTextForButtonValue = true;
                deleteColumn.Width = 80;
                dataGridView1.Columns.Add(deleteColumn);
            }


            dataGridView1.Columns["student_no"].HeaderText = "Student NO.";
            dataGridView1.Columns["student_name"].HeaderText = "Student Name";
            //dataGridView1.Columns["subject_name"].HeaderText = "Subject Name";
            //dataGridView1.Columns["midterm_grade"].HeaderText = "Midterm Grade";
            //dataGridView1.Columns["final_grade"].HeaderText = "Final Grade";
            //dataGridView1.Columns["gpa"].HeaderText = "Average";
            dataGridView1.Columns["Grade"].HeaderText = gradeType == "MIDTERM" ? "Midterm Grade" :
                                                       gradeType == "FINALS" ? "Final Grade" : "Average";
            dataGridView1.Columns["attendance_rate"].HeaderText = "Attendance Rate";
            dataGridView1.Columns["absences"].HeaderText = "Absences";
            dataGridView1.Columns["teacher_id"].HeaderText = "Teacher ID";
            dataGridView1.Columns["Risk Status"].DisplayIndex = dataGridView1.Columns.Count - 1;
            dataGridView1.Columns["Archive"].DisplayIndex = dataGridView1.Columns.Count - 2;

            // Optional: hide violations column
            dataGridView1.Columns["violations"].Visible = false;
            dataGridView1.Columns["subject_name"].Visible = false;
            dataGridView1.Columns["midterm_grade"].Visible = false;
            dataGridView1.Columns["final_grade"].Visible = false;
            dataGridView1.Columns["gpa"].Visible = false;


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
            // --------- Attendance Rate as % ---------
            if (dataGridView1.Columns[e.ColumnIndex].Name == "attendance_rate" && e.Value != null)
            {
                if (float.TryParse(e.Value.ToString(), out float val))
                {
                    e.Value = val.ToString("0.##") + "%"; // 80 -> 80%
                    e.FormattingApplied = true;
                }
            }

            // --------- Risk Status Coloring ---------
            if (dataGridView1.Columns[e.ColumnIndex].HeaderText == "Risk Status")
            {
                string status = e.Value?.ToString();

                e.CellStyle.Font = new Font("Century Gothic", 12f);

                if (status == "Low Risk")
                {
                    e.CellStyle.BackColor = Color.LightGreen; // Soft green for safe
                    e.CellStyle.ForeColor = Color.Black;
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

        private void guna2ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isInitializing) return;

            section = guna2ComboBox2.Text == "ALL" ? "" : guna2ComboBox2.Text;
            LoadStudents(search.Text.Trim());
        }

        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isInitializing) return;

            strand = guna2ComboBox1.Text;
            LoadSections(strand);
            section = "";
            LoadStudents(search.Text.Trim());
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


        private void guna2Button4_Click(object sender, EventArgs e)
        {
            Reports RP = new Reports();
            RP.Show();
        }

        private void btnsearch_Click(object sender, EventArgs e)
        {
        }

        private void search_TextChanged(object sender, EventArgs e)
        {
            LoadStudents(search.Text.Trim());
        }

        private void cmbView_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            // ARCHIVE BUTTON
            if (dataGridView1.Columns[e.ColumnIndex].Name == "Archive") // rename to Archive if needed
            {
                string studNo = dataGridView1.Rows[e.RowIndex].Cells["student_no"].Value.ToString();
                string studName = dataGridView1.Rows[e.RowIndex].Cells["student_name"].Value.ToString();

                DialogResult dr = MessageBox.Show(
                    $"Are you sure you want to archive {studName}?",
                    "Confirm Archive",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (dr == DialogResult.Yes)
                {
                    using (MySqlConnection conn = new MySqlConnection(DBConfig.ConnectionString))
                    {
                        conn.Open();

                        // Use a transaction to ensure both queries succeed together
                        using (MySqlTransaction tran = conn.BeginTransaction())
                        {
                            try
                            {
                                // 1️⃣ Insert student into archived_students (simpler working version)
                                string insertQuery = @"
                                            INSERT INTO archived_students 
                                            (student_no, student_name, strand, section, year_level, 
                                             midterm_grade, finals_grade, gpa, attendance_rate, violations,
                                             term, sem, school_year, parent_contact, email)
                                            SELECT 
                                             student_no, student_name, strand, IFNULL(section,0), year_level,
                                             midterm_grade, finals_grade, gpa, attendance_rate, violations,
                                             term, sem, school_year, parent_contact, email
                                            FROM students
                                            WHERE student_no = @stud";
                                using (MySqlCommand cmd = new MySqlCommand(insertQuery, conn, tran))
                                {
                                    cmd.Parameters.AddWithValue("@stud", studNo.Trim());
                                    cmd.ExecuteNonQuery();
                                }

                                // 2️⃣ Delete student from students
                                string deleteQuery = "DELETE FROM students WHERE student_no = @stud";
                                using (MySqlCommand cmd = new MySqlCommand(deleteQuery, conn, tran))
                                {
                                    cmd.Parameters.AddWithValue("@stud", studNo.Trim());
                                    int rowsDeleted = cmd.ExecuteNonQuery();
                                    if (rowsDeleted == 0)
                                        MessageBox.Show("No rows deleted. Check student_no value.");
                                }

                                // Commit transaction
                                tran.Commit();

                                MessageBox.Show($"{studName} has been archived.", "Archived", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadStudents(search.Text.Trim()); // refresh DataGridView
                            }
                            catch (Exception ex)
                            {
                                tran.Rollback();
                                MessageBox.Show("Error archiving student:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }

                    return; // stop further Action logic
                }
            }

            if (dataGridView1.Columns[e.ColumnIndex].Name == "Risk Status") // click on Risk Status
            {
                string studentNo = dataGridView1.Rows[e.RowIndex].Cells["student_no"].Value.ToString();
                string name = dataGridView1.Rows[e.RowIndex].Cells["student_name"].Value.ToString();
                string risk = dataGridView1.Rows[e.RowIndex].Cells["Risk Status"].Value.ToString();

                float gpa = SafeFloat(dataGridView1.Rows[e.RowIndex].Cells["gpa"].Value);
                float attendance = SafeFloat(dataGridView1.Rows[e.RowIndex].Cells["attendance_rate"].Value);
                int violations = SafeInt(dataGridView1.Rows[e.RowIndex].Cells["violations"].Value);

                string reason = "";
                string solution = "";

                if (gpa < 75)
                {
                    reason += "• GPA is below 75.\n";
                    solution += "• Attend tutorial classes and review lessons.\n";
                }

                if (attendance < 75)
                {
                    reason += "• Attendance rate is below 75%.\n";
                    solution += "• Improve attendance, join class regularly.\n";
                }

                if (violations >= 5)
                {
                    reason += "• Violations reached 5 or more.\n";
                    solution += "• Follow school rules and improve behavior.\n";
                }
                else if (violations >= 3)
                {
                    reason += "• Violations reached 3 or more.\n";
                    solution += "• Follow school rules and improve behavior.\n";
                }

                if (string.IsNullOrEmpty(reason))
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

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void yearlevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isInitializing) return;

            // The year is fixed, so just assign it directly
            year = yearlevel.Text;
            LoadStudents(search.Text.Trim());
        }

        private void guna2ComboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void ApplySubjectFilter()
        {
            if (allStudentsTable == null) return;

            string selectedSubject = subjectcom.Text.Trim();

            DataView dv = new DataView(allStudentsTable);

            if (!string.IsNullOrEmpty(selectedSubject))
                dv.RowFilter = $"subject_name = '{selectedSubject.Replace("'", "''")}'";

            dataGridView1.DataSource = dv;
        }
        private void subjectcom_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplySubjectFilter();
        }
        private void LoadSubjectsCombo(DataTable dt)
        {
            string previousSelection = subjectcom.Text; // store current selection

            subjectcom.SelectedIndexChanged -= subjectcom_SelectedIndexChanged;

            subjectcom.Items.Clear();

            var subjects = dt.AsEnumerable()
                             .Select(r => r.Field<string>("subject_name"))
                             .Distinct()
                             .OrderBy(s => s);

            foreach (var subj in subjects)
                subjectcom.Items.Add(subj);

            // Try to restore previous selection if still exists
            if (!string.IsNullOrEmpty(previousSelection) && subjectcom.Items.Contains(previousSelection))
                subjectcom.SelectedItem = previousSelection;
            else if (subjectcom.Items.Count > 0)
                subjectcom.SelectedIndex = 0;

            subjectcom.SelectedIndexChanged += subjectcom_SelectedIndexChanged;
        }

        private void cmbview_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            gradeType = cmbview.Text;

            // Update column header dynamically
            if (dataGridView1.Columns.Contains("Grade"))
            {
                switch (gradeType)
                {
                    case "MIDTERM":
                        dataGridView1.Columns["Grade"].HeaderText = "Midterm Grade";
                        break;
                    case "FINALS":
                        dataGridView1.Columns["Grade"].HeaderText = "Final Grade";
                        break;
                    case "GPA":
                        dataGridView1.Columns["Grade"].HeaderText = "Average";
                        break;
                }
            }

            LoadStudents(search.Text.Trim());
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
