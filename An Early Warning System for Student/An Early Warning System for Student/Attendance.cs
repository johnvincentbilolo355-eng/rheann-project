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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace An_Early_Warning_System_for_Student
{
    public partial class Attendance : Form
    {
        string connStr = DBConfig.ConnectionString;
        string teacherId;

        public Attendance(string id)
        {
            InitializeComponent();
            teacherId = id;
        }

        private void Attendance_Load(object sender, EventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                // ===== Populate Strand & Section Dropdown =====
                string query = @"
            SELECT DISTINCT c.strand, c.section
            FROM classes c
            INNER JOIN teacher_classes tc ON c.class_id = tc.class_id
            WHERE tc.teacher_id = @teacherId
            ORDER BY c.strand, c.section";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@teacherId", teacherId);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        strandcom.Items.Clear();

                        // Add ALL as first option
                        strandcom.Items.Add("All");

                        while (reader.Read())
                        {
                            string strand = reader["strand"].ToString();
                            string section = reader["section"].ToString();
                            strandcom.Items.Add($"{strand} - {section}");
                        }

                        // ✅ Set default selected item to ALL
                        strandcom.SelectedIndex = 0;
                    }
                }

                // ===== Populate Year Level Dropdown =====
                yearlevel.Items.Clear();
                yearlevel.Items.Add("Year Level");
                yearlevel.Items.Add("11");
                yearlevel.Items.Add("12");

                // ✅ Default selected item
                yearlevel.SelectedIndex = 0;
            }

            // ===== Initial Load =====
            LoadStudentAttendance();
            UpdateAttendanceRate();
        }




        private void LoadStudentAttendance()
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                // Get selected values
                string selectedStrandSection = strandcom.SelectedItem?.ToString();
                string selectedYearLevel = yearlevel.SelectedItem?.ToString();
                string searchText = search.Text.Trim();

                string strand = null;
                string section = null;
                string year = null;

                // --- Handle "All" option ---
                if (!string.IsNullOrEmpty(selectedStrandSection) && selectedStrandSection != "All")
                {
                    string[] parts = selectedStrandSection.Split(new string[] { " - " }, StringSplitOptions.None);
                    if (parts.Length == 2)
                    {
                        strand = parts[0];
                        section = parts[1];
                    }
                }

                if (!string.IsNullOrEmpty(selectedYearLevel) && selectedYearLevel != "All" && selectedYearLevel != "Year Level")
                {
                    year = selectedYearLevel;
                }

                string query = @"
                                SELECT 
                                    s.student_no,
                                    s.student_name,
                                    s.strand,
                                    s.section,
                                    s.year_level,
                                    s.parent_contact,
                                    s.email AS parent_email,
                                    IFNULL(SUM(CASE WHEN a.status='Present' THEN 1 ELSE 0 END), 0) AS TotalPresent,
                                    IFNULL(SUM(CASE WHEN a.status='Absent' THEN 1 ELSE 0 END), 0) AS TotalAbsent
                                FROM students s
                               INNER JOIN classes c 
                                ON s.strand = c.strand 
                               AND s.section = c.section
                               AND s.year_level = c.year_level
                                INNER JOIN teacher_classes tc 
                                    ON c.class_id = tc.class_id
                                LEFT JOIN attendance a 
                                    ON s.student_no = a.student_no
                                WHERE tc.teacher_id = @teacherId
                                  AND (@strand IS NULL OR s.strand = @strand)
                                  AND (@section IS NULL OR s.section = @section)
                                  AND (@year IS NULL OR s.year_level = @year)
                                  AND (@search IS NULL OR s.student_name LIKE @search)
                                GROUP BY 
                                    s.student_no,
                                    s.student_name,
                                    s.strand,
                                    s.section,
                                    s.year_level,
                                    s.parent_contact,
                                    s.email
                                ORDER BY s.student_name;";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@teacherId", teacherId);
                    cmd.Parameters.AddWithValue("@strand", (object)strand ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@section", (object)section ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@year", (object)year ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@search", !string.IsNullOrEmpty(searchText) ? "%" + searchText + "%" : DBNull.Value);

                    DataTable dt = new DataTable();
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }

                    dataGridView1.DataSource = dt;
                    dataGridView1.AllowUserToAddRows = false;

                    // Add Present checkbox column if not already added
                    if (!dataGridView1.Columns.Contains("PresentCheck"))
                    {
                        DataGridViewCheckBoxColumn checkColumn = new DataGridViewCheckBoxColumn
                        {
                            Name = "PresentCheck",
                            HeaderText = "Mark Present Today",
                            Width = 120
                        };
                        dataGridView1.Columns.Add(checkColumn);
                    }

                    // Add Edit button column if not already added
                    if (!dataGridView1.Columns.Contains("Edit"))
                    {
                        DataGridViewButtonColumn editColumn = new DataGridViewButtonColumn
                        {
                            Name = "Edit",
                            HeaderText = "Edit",
                            Text = "Edit",
                            UseColumnTextForButtonValue = true,
                            Width = 80
                        };
                        dataGridView1.Columns.Add(editColumn);
                    }


                    // Rename and rearrange columns
                    dataGridView1.Columns["student_no"].HeaderText = "STUDENT NUMBER";
                    dataGridView1.Columns["student_name"].HeaderText = "STUDENT NAME";
                    dataGridView1.Columns["strand"].HeaderText = "STRAND";
                    dataGridView1.Columns["section"].HeaderText = "SECTION";
                    dataGridView1.Columns["TotalPresent"].HeaderText = "PRESENT";
                    dataGridView1.Columns["TotalAbsent"].HeaderText = "ABSENCES";
                    dataGridView1.Columns["year_level"].HeaderText = "YEAR LEVEL";
                    dataGridView1.Columns["PresentCheck"].DisplayIndex = dataGridView1.Columns.Count - 2;
                    dataGridView1.Columns["Edit"].DisplayIndex = dataGridView1.Columns.Count - 1;

                    dataGridView1.Columns["parent_contact"].Visible = false;
                    dataGridView1.Columns["parent_email"].Visible = false;

                    // Styling
                    dataGridView1.Font = new Font("Century Gothic", 20f, FontStyle.Regular);
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dataGridView1.RowTemplate.Height = 40;
                    dataGridView1.BorderStyle = BorderStyle.None;
                    dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.None;
                    dataGridView1.EnableHeadersVisualStyles = false;
                    dataGridView1.RowHeadersVisible = false;
                    dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
                    dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#651c1c");
                    dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                    dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Century Gothic", 10f, FontStyle.Bold);
                    dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dataGridView1.ColumnHeadersHeight = 38;
                    dataGridView1.DefaultCellStyle.Font = new Font("Century Gothic", 14f);
                    dataGridView1.DefaultCellStyle.Padding = new Padding(8);
                    dataGridView1.DefaultCellStyle.BackColor = Color.White;
                    dataGridView1.DefaultCellStyle.ForeColor = Color.Black;
                    dataGridView1.DefaultCellStyle.SelectionBackColor = ColorTranslator.FromHtml("#f3d9d9");
                    dataGridView1.DefaultCellStyle.SelectionForeColor = Color.Black;
                    dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#faf2f2");

                    foreach (DataGridViewColumn col in dataGridView1.Columns)
                    {
                        col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                }
            }
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return; // Ignore header clicks

            // Edit button
            if (dataGridView1.Columns[e.ColumnIndex].Name == "Edit")
            {
                string studNo = dataGridView1.Rows[e.RowIndex].Cells["student_no"].Value.ToString();
                string studName = dataGridView1.Rows[e.RowIndex].Cells["student_name"].Value.ToString();
                string strand = dataGridView1.Rows[e.RowIndex].Cells["strand"].Value.ToString();
                string section = dataGridView1.Rows[e.RowIndex].Cells["section"].Value.ToString();

                // Add these from your DB columns
                string contact = dataGridView1.Rows[e.RowIndex].Cells["parent_contact"].Value.ToString();
                string email = dataGridView1.Rows[e.RowIndex].Cells["parent_email"].Value.ToString();
                string year = dataGridView1.Rows[e.RowIndex].Cells["year_level"].Value.ToString();

                UpdStudent upd = new UpdStudent();
                upd.Load += (s, ev) =>
                {
                    upd.LoadStudentData(studNo, studName, strand, section, contact, email, year);
                };
                upd.Show();
            }
        }


            // Delete button
         
                        

                


        private void saveAttendance_Click(object sender, EventArgs e)
        {
            SaveAttendance();
        }
        private void SaveAttendance()
        {
            string today = DateTime.Now.ToString("yyyy-MM-dd");
            string timeNow = DateTime.Now.ToString("HH:mm:ss");

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                // Loop only visible rows (filtered)
                foreach (DataGridViewRow row in dataGridView1.Rows.Cast<DataGridViewRow>().Where(r => r.Visible))
                {
                    string studentNo = row.Cells["student_no"].Value.ToString();
                    bool isPresent = row.Cells["PresentCheck"].Value != null && (bool)row.Cells["PresentCheck"].Value;

                    string status = isPresent ? "Present" : "Absent";
                    string timeIn = isPresent ? timeNow : null;

                    // Delete old attendance for today
                    string deleteQuery = "DELETE FROM attendance WHERE student_no = @stud AND date = @date";
                    using (MySqlCommand delCmd = new MySqlCommand(deleteQuery, conn))
                    {
                        delCmd.Parameters.AddWithValue("@stud", studentNo);
                        delCmd.Parameters.AddWithValue("@date", today);
                        delCmd.ExecuteNonQuery();
                    }

                    // Insert new attendance
                    string insertQuery = "INSERT INTO attendance (student_no, date, status, time_in) VALUES (@stud, @date, @status, @time)";
                    using (MySqlCommand insertCmd = new MySqlCommand(insertQuery, conn))
                    {
                        insertCmd.Parameters.AddWithValue("@stud", studentNo);
                        insertCmd.Parameters.AddWithValue("@date", today);
                        insertCmd.Parameters.AddWithValue("@status", status);
                        insertCmd.Parameters.AddWithValue("@time", timeIn);
                        insertCmd.ExecuteNonQuery();
                    }
                }

                UpdateAttendanceRate();
                MessageBox.Show("Attendance saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadStudentAttendance();
            }
        }


        private void UpdateAttendanceRate()
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string updateAttendanceRate = @"
                UPDATE subjects s
                JOIN (
                    SELECT student_no,
                           SUM(CASE WHEN status='Present' THEN 1 ELSE 0 END) AS present,
                           COUNT(*) AS total
                    FROM attendance
                    GROUP BY student_no
                ) a ON s.student_no = a.student_no
                SET s.attendance_rate = (a.present / a.total) * 100;
            ";

                MySqlCommand cmd = new MySqlCommand(updateAttendanceRate, conn);
                cmd.ExecuteNonQuery();
            }
        }


        private void DrpStrands_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadStudentAttendance();
        }

        private void DrpSection_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadStudentAttendance();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void addstud_Click(object sender, EventArgs e)
        {
            AddStudent add = new AddStudent();
            add.Show();
        }

        private void strandcom_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadStudentAttendance();
        }

        private void sectioncom_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadStudentAttendance();
        }

        private void submit_Click(object sender, EventArgs e)
        {
            SaveAttendance();
        }

        private void search_TextChanged(object sender, EventArgs e)
        {
            LoadStudentAttendance();
        }

        private void yearlevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadStudentAttendance();
        }
    }
}
