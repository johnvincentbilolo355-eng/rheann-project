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
    public partial class TeacherClasses : Form
    {
        int selectedClassId = 0;
        bool isEditMode = false;
        public TeacherClasses()
        {
            InitializeComponent();
        }

        private void TeacherClasses_Load(object sender, EventArgs e)
        {
            // Strand
            DrpStrands.Items.AddRange(new string[]
            {
        "STEM", "ICT", "ABM", "HUMSS", "HE"
            });

            // Section (1–4)
            DrpSection.Items.AddRange(new string[]
         { "None", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23"});

            // Year Level
            yearlevel.Items.AddRange(new string[]
            {
        "11", "12"
            });

            // Status
            StatusDrp.Items.AddRange(new string[]
            {
        "active", "inactive"
            });

            LoadClasses();          // load for DataGridView1
            LoadTeacherClasses();   // <--- load for DataGridView2
            StyleDataGridView2();   // optional, styling for DataGridView2
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

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.RowTemplate.Height = 40;


        }


        private void SubjectName_TextChanged(object sender, EventArgs e)
        {

        }

        private void DrpStrands_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void DrpSection_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void yearlevel_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void SchoolYear_TextChanged(object sender, EventArgs e)
        {

        }

        private void StatusDrp_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void SubmitBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SubjectName.Text) ||
                DrpStrands.SelectedIndex == -1 ||
                DrpSection.SelectedIndex == -1 ||
                yearlevel.SelectedIndex == -1 ||
                string.IsNullOrWhiteSpace(SchoolYear.Text) ||
                StatusDrp.SelectedIndex == -1)
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            using (MySqlConnection conn = new MySqlConnection(DBConfig.ConnectionString))
            {
                conn.Open();

                string query;

                if (isEditMode)
                {
                    // UPDATE existing class
                    query = @"
                UPDATE classes SET
                    subject_name = @subject_name,
                    strand = @strand,
                    section = @section,
                    year_level = @year_level,
                    school_year = @school_year,
                    status = @status
                WHERE class_id = @id";
                }
                else
                {
                    // INSERT new class
                    query = @"
                INSERT INTO classes
                    (subject_name, strand, section, year_level, school_year, status)
                VALUES
                    (@subject_name, @strand, @section, @year_level, @school_year, @status)";
                }

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@subject_name", SubjectName.Text.Trim());
                    cmd.Parameters.AddWithValue("@strand", DrpStrands.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@section", DrpSection.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@year_level", yearlevel.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@school_year", SchoolYear.Text.Trim());
                    cmd.Parameters.AddWithValue("@status", StatusDrp.SelectedItem.ToString());

                    if (isEditMode)
                        cmd.Parameters.AddWithValue("@id", selectedClassId);

                    cmd.ExecuteNonQuery();

                    // 🔹 Auto-insert into student_classes only when creating a new class
                    if (!isEditMode)
                    {
                        long newClassId = cmd.LastInsertedId;

                        string insertStudentClasses = @"
                    INSERT INTO student_classes (student_no, subject_name, teacher_id, strand, section, year_level)
                    SELECT s.student_no, c.subject_name, NULL AS teacher_id, c.strand, c.section, c.year_level
                    FROM students s
                    JOIN classes c 
                      ON s.strand = c.strand 
                     AND s.section = c.section 
                     AND s.year_level = c.year_level
                    WHERE c.class_id = @newClassId
                      AND NOT EXISTS (
                          SELECT 1
                          FROM student_classes sc
                          WHERE sc.student_no = s.student_no
                            AND sc.subject_name = c.subject_name
                      )";

                        using (MySqlCommand cmd2 = new MySqlCommand(insertStudentClasses, conn))
                        {
                            cmd2.Parameters.AddWithValue("@newClassId", newClassId);
                            cmd2.ExecuteNonQuery();
                        }
                    }
                }
            }

            MessageBox.Show(isEditMode ? "Class updated!" : "Class added!");

            ClearInputs();
            LoadClasses();
            ResetEditMode();
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

                if (dataGridView1.Columns[e.ColumnIndex].Name == "Edit")
                {
                    selectedClassId = Convert.ToInt32(
                        dataGridView1.Rows[e.RowIndex].Cells["class_id"].Value
                    );

                    SubjectName.Text = dataGridView1.Rows[e.RowIndex].Cells["subject_name"].Value?.ToString() ?? "";
                    DrpStrands.SelectedItem = dataGridView1.Rows[e.RowIndex].Cells["strand"].Value?.ToString();
                    DrpSection.SelectedItem = dataGridView1.Rows[e.RowIndex].Cells["section"].Value?.ToString();
                    yearlevel.SelectedItem = dataGridView1.Rows[e.RowIndex].Cells["year_level"].Value?.ToString();
                    SchoolYear.Text = dataGridView1.Rows[e.RowIndex].Cells["school_year"].Value?.ToString() ?? "";
                    StatusDrp.SelectedItem = dataGridView1.Rows[e.RowIndex].Cells["status"].Value?.ToString();


                    isEditMode = true;
                    SubmitBtn.Text = "Update";

                    return;
                }

                // DELETE
                if (dataGridView1.Columns[e.ColumnIndex].Name == "Delete")
                {
                    int id = Convert.ToInt32(
                        dataGridView1.Rows[e.RowIndex].Cells["class_id"].Value
                    );

                    if (MessageBox.Show("Are you sure you want to delete this class?",
                        "Confirm Delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        DeleteClass(id);
                        LoadClasses();
                    }
                }
            }

            private void ResetEditMode()
            {
                isEditMode = false;
                selectedClassId = 0;
                SubmitBtn.Text = "Submit";
            }
            private void DeleteClass(int id)
            {
                using (MySqlConnection conn = new MySqlConnection(DBConfig.ConnectionString))
                {
                    conn.Open();

                    string query = "DELETE FROM classes WHERE class_id = @id";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }
                }
            }

            private void LoadClasses()
            {
                using (MySqlConnection conn = new MySqlConnection(DBConfig.ConnectionString))
                {
                    conn.Open();

                    string query = @"SELECT 
                            class_id,
                            subject_name,
                            strand,
                            section,
                            year_level,
                            school_year,
                            status
                            FROM classes";

                    using (MySqlDataAdapter da = new MySqlDataAdapter(query, conn))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        // IMPORTANT: Clear existing columns first
                        dataGridView1.Columns.Clear();

                        dataGridView1.DataSource = dt;
                        dataGridView1.Columns["class_id"].HeaderText = "Class ID";
                        dataGridView1.Columns["subject_name"].HeaderText = "Subject Name";
                        dataGridView1.Columns["strand"].HeaderText = "Strand";
                        dataGridView1.Columns["section"].HeaderText = "Section";
                        dataGridView1.Columns["year_level"].HeaderText = "Year Level";
                        dataGridView1.Columns["school_year"].HeaderText = "School Year";
                        dataGridView1.Columns["status"].HeaderText = "Status";

                        // Style Grid
                        StyleDataGridView();


                        // EDIT BUTTON
                        DataGridViewButtonColumn editColumn = new DataGridViewButtonColumn();
                        editColumn.Name = "Edit";
                        editColumn.HeaderText = "Edit";
                        editColumn.Text = "Edit";
                        editColumn.UseColumnTextForButtonValue = true;
                        editColumn.Width = 80;
                        dataGridView1.Columns.Add(editColumn);

                        // DELETE BUTTON
                        DataGridViewButtonColumn deleteColumn = new DataGridViewButtonColumn();
                        deleteColumn.Name = "Delete";
                        deleteColumn.HeaderText = "Delete";
                        deleteColumn.Text = "Delete";
                        deleteColumn.UseColumnTextForButtonValue = true;
                        deleteColumn.Width = 80;
                        dataGridView1.Columns.Add(deleteColumn);
                    }
                }
            }

            private void ClearInputs()
            {
                SubjectName.Clear();
                SchoolYear.Clear();

                DrpStrands.SelectedIndex = -1;
                DrpSection.SelectedIndex = -1;
                yearlevel.SelectedIndex = -1;
                StatusDrp.SelectedIndex = -1;
            }

        private void teacherID_TextChanged(object sender, EventArgs e)
        {

        }

        private void classID_TextChanged(object sender, EventArgs e)
        {

        }

        private void DeleteTeacherClass(int id)
        {
            using (MySqlConnection conn = new MySqlConnection(DBConfig.ConnectionString))
            {
                conn.Open();
                string query = "DELETE FROM teacher_classes WHERE id = @id";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }


        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (dataGridView2.Columns[e.ColumnIndex].Name == "Edit")
            {
                selectedTeacherClassId = Convert.ToInt32(
                    dataGridView2.Rows[e.RowIndex].Cells["id"].Value
                );

                teacherID.Text = dataGridView2.Rows[e.RowIndex].Cells["teacher_id"].Value?.ToString();
                classID.Text = dataGridView2.Rows[e.RowIndex].Cells["class_id"].Value?.ToString();

                isEditMode2 = true;
                SubmitBtn2.Text = "Update";
                return;
            }

            if (dataGridView2.Columns[e.ColumnIndex].Name == "Delete")
            {
                int id = Convert.ToInt32(
                    dataGridView2.Rows[e.RowIndex].Cells["id"].Value
                );

                if (MessageBox.Show("Are you sure you want to delete this record?",
                    "Confirm Delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    DeleteTeacherClass(id);
                    LoadTeacherClasses();
                }
            }
        }
        
            private void StyleDataGridView2()
        {
            dataGridView2.Font = new Font("Century Gothic", 8f, FontStyle.Regular);
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView2.RowTemplate.Height = 40;
            dataGridView2.BorderStyle = BorderStyle.None;
            dataGridView2.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dataGridView2.EnableHeadersVisualStyles = false;
            dataGridView2.RowHeadersVisible = false;
            dataGridView2.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridView2.ColumnHeadersDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#061d4a");
            dataGridView2.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView2.ColumnHeadersDefaultCellStyle.Font = new Font("Century Gothic", 10f, FontStyle.Bold);
            dataGridView2.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView2.ColumnHeadersHeight = 38;
            dataGridView2.DefaultCellStyle.Font = new Font("Century Gothic", 8f);
            dataGridView2.DefaultCellStyle.Padding = new Padding(8);
            dataGridView2.DefaultCellStyle.SelectionBackColor = Color.FromArgb(100, 149, 237);
            dataGridView2.DefaultCellStyle.SelectionForeColor = Color.White;
            dataGridView2.DefaultCellStyle.BackColor = Color.White;
            dataGridView2.DefaultCellStyle.ForeColor = Color.Black;
            dataGridView2.DefaultCellStyle.SelectionBackColor = ColorTranslator.FromHtml("#d9e3f3");
            dataGridView2.DefaultCellStyle.SelectionForeColor = Color.Black;
            dataGridView2.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView2.AlternatingRowsDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#f2f3fa");
            //center text
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;

            dataGridView1.DefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;


        }
        private bool TeacherExists(string teacherId, MySqlConnection conn)
        {
            string query = @"
        SELECT COUNT(*) 
        FROM users 
        WHERE id_no = @teacher_id 
          AND role = 'teacher'
          AND status = 'active'";

            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@teacher_id", teacherId);
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        private bool ClassExists(string classId, MySqlConnection conn)
        {
            string query = "SELECT COUNT(*) FROM classes WHERE class_id = @class_id";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@class_id", classId);
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }
        private void SubmitBtn2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(teacherID.Text) ||
                string.IsNullOrWhiteSpace(classID.Text))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            using (MySqlConnection conn = new MySqlConnection(DBConfig.ConnectionString))
            {
                conn.Open();

                // 🔴 VALIDATION PART
                if (!TeacherExists(teacherID.Text.Trim(), conn))
                {
                    MessageBox.Show("Teacher ID does not exist.", "Invalid Teacher ID",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!ClassExists(classID.Text.Trim(), conn))
                {
                    MessageBox.Show("Class ID does not exist.", "Invalid Class ID",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string query;

                if (isEditMode2)
                {
                    // UPDATE teacher_classes record
                    query = @"
                UPDATE teacher_classes SET
                    teacher_id = @teacher_id,
                    class_id = @class_id
                WHERE id = @id";
                }
                else
                {
                    // INSERT new teacher_classes record
                    query = @"
                INSERT INTO teacher_classes
                    (teacher_id, class_id)
                VALUES
                    (@teacher_id, @class_id)";
                }

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@teacher_id", teacherID.Text.Trim());
                    cmd.Parameters.AddWithValue("@class_id", classID.Text.Trim());

                    if (isEditMode2)
                        cmd.Parameters.AddWithValue("@id", selectedTeacherClassId);

                    cmd.ExecuteNonQuery();

                    // 🔹 Update student_classes to reflect assigned teacher for all students in the class
                    string updateStudentClasses = @"
                UPDATE student_classes sc
                JOIN classes c ON sc.subject_name = c.subject_name
                              AND sc.strand = c.strand
                              AND sc.section = c.section
                              AND sc.year_level = c.year_level
                SET sc.teacher_id = @teacher_id
                WHERE c.class_id = @class_id";

                    using (MySqlCommand cmd2 = new MySqlCommand(updateStudentClasses, conn))
                    {
                        cmd2.Parameters.AddWithValue("@teacher_id", teacherID.Text.Trim());
                        cmd2.Parameters.AddWithValue("@class_id", classID.Text.Trim());
                        cmd2.ExecuteNonQuery();
                    }
                }
            }

            MessageBox.Show(isEditMode2 ? "Record updated!" : "Record added!");

            ClearInputs2();
            LoadTeacherClasses();
            ResetEditMode2();
        }




        // Fields for DataGridView2
        int selectedTeacherClassId = 0;
        bool isEditMode2 = false;

        // Reset Edit Mode for DataGridView2
        private void ResetEditMode2()
        {
            isEditMode2 = false;
            selectedTeacherClassId = 0;
            SubmitBtn2.Text = "Submit";
        }

        // Clear inputs for Teacher-Class mapping
        private void ClearInputs2()
        {
            teacherID.Clear();
            classID.Clear();
        }


        // Load teacher-class assignments into DataGridView2
        private void LoadTeacherClasses()
        {
            using (MySqlConnection conn = new MySqlConnection(DBConfig.ConnectionString))
            {
                conn.Open();

                string query = @"SELECT 
                            id,
                            teacher_id,
                            class_id
                         FROM teacher_classes";

                using (MySqlDataAdapter da = new MySqlDataAdapter(query, conn))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dataGridView2.Columns.Clear();
                    dataGridView2.DataSource = dt;

                    dataGridView2.Columns["id"].HeaderText = "ID";
                    dataGridView2.Columns["teacher_id"].HeaderText = "Teacher ID";
                    dataGridView2.Columns["class_id"].HeaderText = "Class ID";

                    // Style grid (reuse from dataGridView1 if you want)
                    dataGridView2.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                    // EDIT BUTTON
                    DataGridViewButtonColumn editColumn = new DataGridViewButtonColumn();
                    editColumn.Name = "Edit";
                    editColumn.HeaderText = "Edit";
                    editColumn.Text = "Edit";
                    editColumn.UseColumnTextForButtonValue = true;
                    editColumn.Width = 80;
                    dataGridView2.Columns.Add(editColumn);

                    // DELETE BUTTON
                    DataGridViewButtonColumn deleteColumn = new DataGridViewButtonColumn();
                    deleteColumn.Name = "Delete";
                    deleteColumn.HeaderText = "Delete";
                    deleteColumn.Text = "Delete";
                    deleteColumn.UseColumnTextForButtonValue = true;
                    deleteColumn.Width = 80;
                    dataGridView2.Columns.Add(deleteColumn);
                }
            }
        }
    }
}
