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
using MySql.Data.MySqlClient;

namespace An_Early_Warning_System_for_Student
{
    public partial class BehaviorLog : Form
    {
        string connStr = DBConfig.ConnectionString;
        string teacherYearLevel;
        string teacherStrand;
        string teacherSection;

        public BehaviorLog(string strand, string section, string yearLevel)
        {
            InitializeComponent();
            teacherStrand = strand;
            teacherSection = section;
            teacherYearLevel = yearLevel;
        }
        private void BehaviorLog_Load(object sender, EventArgs e)
        {
            // Populate violation combo box
            guna2ComboBoxViolation.Items.Clear();
            guna2ComboBoxViolation.Items.AddRange(new string[]
            {
        "Fighting / Physical Assault",
        "Bullying / Harassment / Intimidation",
        "Cheating / Academic Dishonesty",
        "Vandalism / Property Damage",
        "Stealing / Theft",
        "Drug / Substance Abuse",
        "Smoking / Alcohol on Campus",
        "Threatening Staff or Students",
        "Sexual Misconduct / Inappropriate Behavior",
        "Others"
            });

            LoadBehaviorLogs();  // load data first

            // REMOVE ID COLUMN
            if (guna2DataGridViewLogs.Columns.Contains("id"))
            {
                guna2DataGridViewLogs.Columns["id"].Visible = false;
            }

            // CHANGE HEADER TEXT
            guna2DataGridViewLogs.Columns["student_no"].HeaderText = "STUDENT NUMBER";
            guna2DataGridViewLogs.Columns["violation_type"].HeaderText = "VIOLATION TYPE";
            guna2DataGridViewLogs.Columns["date"].HeaderText = "DATE";
            guna2DataGridViewLogs.Columns["notes"].HeaderText = "NOTES";

            guna2DataGridViewLogs.Columns["Delete"].DisplayIndex = guna2DataGridViewLogs.Columns.Count - 1;
            // CENTER ALIGNMENT
            guna2DataGridViewLogs.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            guna2DataGridViewLogs.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // HEADER FONT STYLE
            guna2DataGridViewLogs.ColumnHeadersDefaultCellStyle.Font = new Font("Century Gothic",11 , FontStyle.Bold);
            // CELL FONT STYLE (THIS IS WHAT YOU NEED)
            guna2DataGridViewLogs.DefaultCellStyle.Font = new Font("Century Gothic", 11, FontStyle.Regular);
            // LIGHT DESIGN WITH DARK HEADER
            guna2DataGridViewLogs.Theme = Guna.UI2.WinForms.Enums.DataGridViewPresetThemes.Default;
            guna2DataGridViewLogs.ThemeStyle.BackColor = Color.White;
            guna2DataGridViewLogs.ThemeStyle.GridColor = Color.FromArgb(230, 230, 230);

            // DARK HEADER STYLE
            guna2DataGridViewLogs.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(101, 28, 28);
            guna2DataGridViewLogs.ThemeStyle.HeaderStyle.ForeColor = Color.White;

            // FIX BLUE HEADER SELECTION
            guna2DataGridViewLogs.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(101, 28, 28);
            guna2DataGridViewLogs.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;

            guna2DataGridViewLogs.ThemeStyle.RowsStyle.BackColor = Color.White;
            guna2DataGridViewLogs.ThemeStyle.RowsStyle.ForeColor = Color.Black;
            guna2DataGridViewLogs.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(220, 235, 255);
            guna2DataGridViewLogs.ThemeStyle.RowsStyle.SelectionForeColor = Color.Black;

            guna2DataGridViewLogs.ThemeStyle.AlternatingRowsStyle.BackColor = Color.FromArgb(250, 250, 250);
            guna2DataGridViewLogs.ThemeStyle.AlternatingRowsStyle.ForeColor = Color.Black;

            guna2DataGridViewLogs.BorderStyle = BorderStyle.None;
            guna2DataGridViewLogs.EnableHeadersVisualStyles = false;
            guna2DataGridViewLogs.RowHeadersVisible = false;

            guna2DataGridViewLogs.ColumnHeadersHeight = 45;
            guna2DataGridViewLogs.RowTemplate.Height = 40;
            guna2DataGridViewLogs.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            guna2DataGridViewLogs.AllowUserToResizeColumns = false;
            guna2DataGridViewLogs.AllowUserToResizeRows = false;

            guna2DataGridViewLogs.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            guna2DataGridViewLogs.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            guna2DataGridViewLogs.CellClick += guna2DataGridViewLogs_CellClick;

        }



        private void LoadBehaviorLogs()
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string query = @"
            SELECT bl.id,
                   bl.student_no,
                   s.student_name,
                   bl.violation_type,
                   bl.date,
                   bl.notes
            FROM behavior_logs bl
            INNER JOIN students s ON bl.student_no = s.student_no
            WHERE s.year_level = @year
              AND s.strand = @strand
              AND s.section = @section
            ORDER BY bl.date DESC
        ";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@year", teacherYearLevel);
                cmd.Parameters.AddWithValue("@strand", teacherStrand);
                cmd.Parameters.AddWithValue("@section", Convert.ToInt32(teacherSection)); // important: int

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                guna2DataGridViewLogs.DataSource = dt;

                // ADD DELETE BUTTON (ONCE ONLY)
                if (!guna2DataGridViewLogs.Columns.Contains("delete"))
                {
                    DataGridViewButtonColumn deleteColumn = new DataGridViewButtonColumn();
                    deleteColumn.Name = "delete";
                    deleteColumn.HeaderText = "DELETE";
                    deleteColumn.Text = "DELETE";
                    deleteColumn.UseColumnTextForButtonValue = true;
                    guna2DataGridViewLogs.Columns.Add(deleteColumn);
                }

                guna2DataGridViewLogs.AllowUserToAddRows = false;
                guna2DataGridViewLogs.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
        }

        private void guna2ButtonAdd_Click(object sender, EventArgs e)
        {
            string studentNo = guna2TextBoxStudentNo.Text.Trim();
            string violationType = guna2ComboBoxViolation.SelectedItem?.ToString();
            DateTime violationDate = guna2DateTimePicker.Value;
            string notes = guna2TextBoxNotes.Text.Trim();

            if (string.IsNullOrEmpty(studentNo) || string.IsNullOrEmpty(violationType))
            {
                MessageBox.Show("Please enter Student No and Violation Type.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                // 🔎 CHECK IF STUDENT BELONGS TO TEACHER’S SECTION
                string checkQuery = @"
            SELECT COUNT(*) 
            FROM students
            WHERE student_no = @student_no
              AND year_level = @year
              AND strand = @strand
              AND section = @section
        ";

                MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@student_no", studentNo);
                checkCmd.Parameters.AddWithValue("@year", teacherYearLevel);
                checkCmd.Parameters.AddWithValue("@strand", teacherStrand);
                checkCmd.Parameters.AddWithValue("@section", Convert.ToInt32(teacherSection)); // important

                int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (count == 0)
                {
                    MessageBox.Show("You can only submit logs for your advised students.",
                                    "Access Denied",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                    return;
                }

                // ✅ INSERT LOG
                string insertQuery = @"INSERT INTO behavior_logs 
                               (student_no, violation_type, date, notes)
                               VALUES 
                               (@student_no, @violation_type, @date, @notes)";

                MySqlCommand cmd = new MySqlCommand(insertQuery, conn);
                cmd.Parameters.AddWithValue("@student_no", studentNo);
                cmd.Parameters.AddWithValue("@violation_type", violationType);
                cmd.Parameters.AddWithValue("@date", violationDate);
                cmd.Parameters.AddWithValue("@notes", notes);
                cmd.ExecuteNonQuery();

                // ✅ INCREMENT VIOLATION COUNT
                string updateStudentQuery = @"UPDATE students
                                      SET violations = violations + 1
                                      WHERE student_no = @student_no";

                MySqlCommand updateCmd = new MySqlCommand(updateStudentQuery, conn);
                updateCmd.Parameters.AddWithValue("@student_no", studentNo);
                updateCmd.ExecuteNonQuery();
            }

            MessageBox.Show("Behavior log added successfully!", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

            LoadBehaviorLogs();
        }
        private void guna2DataGridViewLogs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (guna2DataGridViewLogs.Columns[e.ColumnIndex].Name == "delete")
            {
                string logId = guna2DataGridViewLogs.Rows[e.RowIndex].Cells["id"].Value.ToString();
                string studentNo = guna2DataGridViewLogs.Rows[e.RowIndex].Cells["student_no"].Value.ToString();

                DialogResult dr = MessageBox.Show("Are you sure you want to delete this log?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dr == DialogResult.Yes)
                {
                    using (MySqlConnection conn = new MySqlConnection(connStr))
                    {
                        conn.Open();

                        string deleteQuery = @"DELETE FROM behavior_logs WHERE id = @id";
                        MySqlCommand deleteCmd = new MySqlCommand(deleteQuery, conn);
                        deleteCmd.Parameters.AddWithValue("@id", logId);
                        deleteCmd.ExecuteNonQuery();

                        string updateStudentQuery = @"UPDATE students
                                              SET violations = violations - 1
                                              WHERE student_no = @student_no AND violations > 0";
                        MySqlCommand updateCmd = new MySqlCommand(updateStudentQuery, conn);
                        updateCmd.Parameters.AddWithValue("@student_no", studentNo);
                        updateCmd.ExecuteNonQuery();
                    }
                    LoadBehaviorLogs();
                }
            }
        }



    }
}
