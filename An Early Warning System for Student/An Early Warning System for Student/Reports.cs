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
using System.Drawing.Drawing2D;

namespace An_Early_Warning_System_for_Student
{
    public partial class Reports : Form
    {
        private string teacherId;
        public Reports()
        {
            InitializeComponent();
            this.teacherId = teacherId;
        }

        private void Grade12_Load(object sender, EventArgs e)
        {
            // Populate report type
            cmbreport.Items.Add("Academic");
            cmbreport.Items.Add("Behavioral");
            cmbreport.SelectedIndex = 0; // optional: select the first item by default

            // Populate category
            cmbcategory.Items.Add("Exam");
            cmbcategory.Items.Add("Homework");
            cmbcategory.Items.Add("Attendance");
            cmbcategory.Items.Add("Discipline");
            cmbcategory.Items.Add("Bullying");
            cmbcategory.SelectedIndex = 0; // optional: default selection

            // Populate priority
            cmbprio.Items.Add("Low");
            cmbprio.Items.Add("Medium");
            cmbprio.Items.Add("High");
            cmbprio.SelectedIndex = 0; // optional: default selection

            int radius = 20; // pwede mo palitan kung gusto mo mas round o less round
            this.Region = new Region(GetRoundedPath(this.ClientRectangle, radius));
            this.ActiveControl = teacher_id;
        }
        private GraphicsPath GetRoundedPath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int d = radius * 2;

            path.AddArc(rect.X, rect.Y, d, d, 180, 90); // top-left
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90); // top-right
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90); // bottom-right
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90); // bottom-left
            path.CloseFigure();

            return path;
        }


        private void guna2Button4_Click(object sender, EventArgs e)
        {
            // --- VALIDATION ---
            if (string.IsNullOrWhiteSpace(student.Text) ||
                cmbreport.SelectedIndex == -1 ||
                cmbcategory.SelectedIndex == -1 ||
                cmbprio.SelectedIndex == -1 ||
                string.IsNullOrWhiteSpace(reportle.Text) ||
                string.IsNullOrWhiteSpace(description.Text))
            {
                MessageBox.Show("Please fill out all fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // --- DATABASE CONNECTION ---
            string connStr = DBConfig.ConnectionString;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(DBConfig.ConnectionString))
                {
                    conn.Open();

                    string query = @"INSERT INTO student_reports 
                            (student_no, report_type, category, priority_level, report_title, description, teacher_id) 
                            VALUES (@student_no, @report_type, @category, @priority_level, @report_title, @description, @teacher_id)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@student_no", student.Text.Trim());
                        cmd.Parameters.AddWithValue("@report_type", cmbreport.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@category", cmbcategory.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@priority_level", cmbprio.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@report_title", reportle.Text.Trim());
                        cmd.Parameters.AddWithValue("@description", description.Text.Trim());
                        cmd.Parameters.AddWithValue("@teacher_id", teacher_id.Text);

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Report submitted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Optional: clear fields after submission
                    student.Clear();
                    cmbreport.SelectedIndex = -1;
                    cmbcategory.SelectedIndex = -1;
                    cmbprio.SelectedIndex = -1;
                    reportle.Clear();
                    description.Clear();
                    teacher_id.Clear();
                    teacher_id.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error inserting report: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void teacher_id_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                student.Focus();
                e.SuppressKeyPress = true;
            }
        }

        private void student_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                cmbreport.Focus();
                e.SuppressKeyPress = true;
            }
        }
        
        private void reportle_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                description.Focus();
                e.SuppressKeyPress = true;
            }
        }

        private void description_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                guna2Button4.Focus();
                e.SuppressKeyPress = true;
            }
        }
    }
}
