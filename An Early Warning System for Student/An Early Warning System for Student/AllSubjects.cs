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
    public partial class AllSubjects : Form
    {
        bool isInitializing = true;

        private string year;
        private string strand;
        private string section;
        public string LoggedInId { get; private set; }
        public AllSubjects(string idNo, string year, string strand, string section)
        {
            InitializeComponent();
            this.year = year;
            this.strand = strand;
            this.section = section;
            LoggedInId = idNo;
        }

        private void search_TextChanged(object sender, EventArgs e)
        {
            if (isInitializing) return;

            string keyword = search.Text.Trim();
            LoadAllSubjects(keyword);
        }
        private void AllSubjects_Load(object sender, EventArgs e)
        {
            isInitializing = true;

            // --- Populate year, strand, section ---
            yearlevel.Items.Clear();
            yearlevel.Items.Add(year);
            yearlevel.SelectedIndex = 0;

            guna2ComboBox1.Items.Clear();
            guna2ComboBox1.Items.Add(strand);
            guna2ComboBox1.SelectedIndex = 0;

            guna2ComboBox2.Items.Clear();
            guna2ComboBox2.Items.Add(section);
            guna2ComboBox2.SelectedIndex = 0;

            // --- Populate grade type dropdown (MIDTERM / FINALS) ---
            cmbview.Items.Clear();
            cmbview.Items.Add("MIDTERM");
            cmbview.Items.Add("FINALS");
            cmbview.SelectedIndex = 0; // default MIDTERM

            // Load students/subjects
            LoadAllSubjects();
            FormatGrid();
            isInitializing = false;
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string columnName = dataGridView1.Columns[e.ColumnIndex].Name;

            if (columnName == "Risk Status")
            {
                string studentName = dataGridView1.Rows[e.RowIndex].Cells["student_name"].Value?.ToString();
                string risk = dataGridView1.Rows[e.RowIndex].Cells["Risk Status"].Value?.ToString();
                float gwa = SafeFloat(dataGridView1.Rows[e.RowIndex].Cells["GWA"].Value);

                StringBuilder reasonBuilder = new StringBuilder();
                StringBuilder borderlineBuilder = new StringBuilder();
                string tipMessage = "";

                foreach (DataGridViewColumn col in dataGridView1.Columns)
                {
                    if (col.Name == "student_no"
                        || col.Name == "student_name"
                        || col.Name == "GWA"
                        || col.Name == "Risk Status"
                        || col.Name == "Attendance Rate"
                        || col.Name == "Profile")
                        continue;

                    float grade = SafeFloat(dataGridView1.Rows[e.RowIndex].Cells[col.Name].Value);

                    if (grade < 75)
                        reasonBuilder.AppendLine($"• {col.HeaderText}: {grade} (Needs Attention)");
                    else if (grade == 75)
                        borderlineBuilder.AppendLine($"• {col.HeaderText}: {grade} (Borderline)");
                }

                // Check GWA separately
                if (risk == "Low Risk")
                {
                    tipMessage = "• Good job! Keep it up.";
                }
                else if (risk == "Warning")
                {
                    if (gwa < 75)
                        reasonBuilder.Insert(0, $"• GWA below 75 ({gwa})\n");

                    tipMessage = "• Focus on weak and borderline subjects and attend classes regularly.";
                }
                else // High Risk or others
                {
                    if (gwa < 75)
                        reasonBuilder.Insert(0, $"• GWA below 75 ({gwa})\n");

                    tipMessage = "• Review weak and borderline subjects and improve study habits.";
                }

                string message = $"Student: {studentName}\n\nOverall Status: {risk}\n";

                if (reasonBuilder.Length > 0)
                    message += $"\nMain Reasons (Grades):\n{reasonBuilder}";

                if (borderlineBuilder.Length > 0)
                    message += $"\nBorderline Subjects (Grade = 75):\n{borderlineBuilder}";

                message += $"\nTip:\n{tipMessage}";

                MessageBox.Show(message, "Risk Explanation", MessageBoxButtons.OK, MessageBoxIcon.Information);

                return;
            }
        
        

        // ✅ IF OTHER COLUMN (OPEN PROFILE)
        string studentNoProfile = dataGridView1.Rows[e.RowIndex].Cells["student_no"].Value?.ToString();
            if (!string.IsNullOrEmpty(studentNoProfile))
            {
                StudentProfileForm profileForm = new StudentProfileForm(studentNoProfile);
                profileForm.ShowDialog();
            }
        }

        private void FormatGrid()
        {
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
            dataGridView1.EnableHeadersVisualStyles = false;

            dataGridView1.DefaultCellStyle.Padding = new Padding(5);
            dataGridView1.RowTemplate.Height = 35;

            dataGridView1.CellFormatting += DataGridView1_CellFormatting;

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
        private void LoadAllSubjects(string keyword = "")
        {
            string gradeType = cmbview.SelectedItem?.ToString() ?? "MIDTERM";

            string query = @"
                    SELECT 
                        s.student_no,
                        st.student_name,
                        s.subject_name,
                        s.midterm_grade,
                        s.final_grade,
                        s.gpa
                    FROM subjects s
                    INNER JOIN students st ON s.student_no = st.student_no
                    WHERE st.year_level = @year
                      AND st.strand = @strand
                      AND st.section = @section
                      AND (COALESCE(@keyword,'') = '' 
                           OR st.student_name LIKE @keyword 
                           OR st.student_no LIKE @keyword)
                    ORDER BY st.student_name ASC, s.subject_name ASC";

            DataTable dt = new DataTable();

            using (MySqlConnection con = new MySqlConnection(DBConfig.ConnectionString))
            using (MySqlCommand cmd = new MySqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@strand", strand);
                cmd.Parameters.AddWithValue("@section", section);
                cmd.Parameters.AddWithValue("@keyword", string.IsNullOrEmpty(keyword) ? "" : "%" + keyword + "%");

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
            }

            // Add Grade column based on cmbview
            if (!dt.Columns.Contains("Grade"))
                dt.Columns.Add("Grade", typeof(float));

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
                }
                row["Grade"] = grade;
            }

            // Pivot the subjects table
            DataTable pivoted = PivotSubjects(dt);

            // Bind to DataGridView
            dataGridView1.DataSource = pivoted;

            // Styling
            FormatGrid();
            dataGridView1.Columns["student_no"].HeaderText = "Student NO.";
            dataGridView1.Columns["student_name"].HeaderText = "Student Name";
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        // Pivot function
        private DataTable PivotSubjects(DataTable dt)
        {
            DataTable pivot = new DataTable();

            // Add student_no, student_name
            pivot.Columns.Add("student_no");
            pivot.Columns.Add("student_name");

            // Get all subjects in the class
            var subjects = dt.AsEnumerable()
                             .Select(r => r.Field<string>("subject_name"))
                             .Distinct()
                             .OrderBy(s => s)
                             .ToList();

            // Add dynamic subject columns
            foreach (var subj in subjects)
                pivot.Columns.Add(subj, typeof(float));

            // Add GWA and Risk Status ONLY
            pivot.Columns.Add("GWA", typeof(float));
            pivot.Columns.Add("Risk Status");

            // Group by student_no
            var studentGroups = dt.AsEnumerable()
                                  .GroupBy(r => r.Field<string>("student_no"));

            foreach (var student in studentGroups)
            {
                DataRow newRow = pivot.NewRow();

                string studentNo = student.Key;
                string studentName = student.First().Field<string>("student_name");

                newRow["student_no"] = studentNo;
                newRow["student_name"] = studentName;

                float totalGrade = 0f;
                int totalSubjects = subjects.Count;

                foreach (var subj in subjects)
                {
                    var row = student.FirstOrDefault(r => r.Field<string>("subject_name") == subj);

                    float grade = 0f;

                    if (row != null)
                        grade = SafeFloat(row["Grade"]);

                    newRow[subj] = grade;
                    totalGrade += grade;
                }

                float gwa = totalSubjects > 0 ? totalGrade / totalSubjects : 0;

                newRow["GWA"] = gwa;
                newRow["Risk Status"] = ComputeRiskStatus(gwa);

                pivot.Rows.Add(newRow);
            }

            return pivot;
        }

        // Safe helpers
        private float SafeFloat(object value)
        {
            float f;
            return float.TryParse(value?.ToString(), out f) ? f : 0;
        }

        private int SafeInt(object value)
        {
            int i;
            return int.TryParse(value?.ToString(), out i) ? i : 0;
        }

        // Risk computation (reuse from ViewStudents)
        private string ComputeRiskStatus(float grade)
        {
            if (grade <= 0)
                return "Pending";

            if (grade < 75)
                return "High Risk";

            if (grade < 80)
                return "Warning";

            return "Low Risk";
        }



        private void yearlevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isInitializing) return; // do nothing
        }

        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isInitializing) return; // do nothing
        }

        private void guna2ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isInitializing) return; // do nothing
        }

        //private void LoadSections(string selectedStrand)
        //{
        //    guna2ComboBox2.Items.Clear();
        //    guna2ComboBox2.Items.Add("ALL");

        //    using (MySqlConnection con = new MySqlConnection(DBConfig.ConnectionString))
        //    {
        //        con.Open();
        //        string query = "SELECT DISTINCT section FROM students";
        //        if (!string.IsNullOrEmpty(selectedStrand) && selectedStrand != "ALL")
        //            query += " WHERE strand=@strand";

        //        MySqlCommand cmd = new MySqlCommand(query, con);
        //        if (!string.IsNullOrEmpty(selectedStrand) && selectedStrand != "ALL")
        //            cmd.Parameters.AddWithValue("@strand", selectedStrand);

        //        MySqlDataReader dr = cmd.ExecuteReader();
        //        while (dr.Read())
        //        {
        //            guna2ComboBox2.Items.Add(dr["section"].ToString());
        //        }
        //    }

        //    guna2ComboBox2.SelectedIndex = 0; // default ALL
        //}

        private void cmbview_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Reload the subjects table with the new grade type
            LoadAllSubjects(search.Text.Trim()); // optional: pass keyword from search box if needed
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

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            Reports RP = new Reports();
            RP.Show();
        }
    }
}
