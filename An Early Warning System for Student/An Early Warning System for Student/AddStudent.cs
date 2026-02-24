using MySql.Data.MySqlClient;
using System;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms;

namespace An_Early_Warning_System_for_Student
{
    public partial class AddStudent : Form
    {
        string connStr = DBConfig.ConnectionString;
        public AddStudent()
        {
            InitializeComponent();
        }

        private void AddStudent_Load(object sender, EventArgs e)
        {
            {
                // Populate strands
                comstrand.Items.Clear();
                comstrand.Items.Add("Strands");
                comstrand.Items.AddRange(new string[] { "STEM", "ABM", "ICT", "HE", "HUMS" });

                // Populate sections
                comsection.Items.Clear();
                comsection.Items.Add("Sections");
                comsection.Items.AddRange(new string[] { "None", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23" });

                yearlevel.Items.Clear();
                yearlevel.Items.Add("Year Level");
                yearlevel.Items.AddRange(new string[] { "11", "12"});

                // Optionally select first item
                if (yearlevel.Items.Count > 0) yearlevel.SelectedIndex = 0;
                if (comstrand.Items.Count > 0) comstrand.SelectedIndex = 0;
                if (comsection.Items.Count > 0) comsection.SelectedIndex = 0;

                int radius = 20; // pwede mo palitan kung gusto mo mas round o less round
                this.Region = new Region(GetRoundedPath(this.ClientRectangle, radius));
            }

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

        private void btnSubmit_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void studNo_TextChanged(object sender, EventArgs e)
        {

        }

        private void submit_Click(object sender, EventArgs e)
        {
            string studentNo = studnum.Text.Trim();
            string studentName = name.Text.Trim();
            string strand = comstrand.SelectedItem?.ToString();
            string section = comsection.SelectedItem?.ToString();
            string yearlev = yearlevel.SelectedItem?.ToString();
            string email = parentemail.Text.Trim();
            string contact = parentcontact.Text.Trim();

            if (string.IsNullOrEmpty(studentNo) || string.IsNullOrEmpty(studentName))
            {
                MessageBox.Show("Please enter both Student No. and Name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                try
                {
                    conn.Open();

                    // Optional: check if student_no already exists
                    string checkQuery = "SELECT COUNT(*) FROM students WHERE student_no = @studNo";
                    using (MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@studNo", studentNo);
                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                        if (count > 0)
                        {
                            MessageBox.Show("Student No. already exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    // Insert student
                    string insertQuery = "INSERT INTO students (student_no, student_name, strand, section, parent_contact, email, year_level) VALUES (@studnum, @name, @strand, @section, @parent_contact, @email, @yearlevel)";
                    using (MySqlCommand cmd = new MySqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@studnum", studentNo);
                        cmd.Parameters.AddWithValue("@name", studentName);
                        cmd.Parameters.AddWithValue("@strand", strand);
                        cmd.Parameters.AddWithValue("@section", section);
                        cmd.Parameters.AddWithValue("@parent_contact", contact);
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@yearlevel", yearlev);

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Student added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Clear fields for next entry
                    studnum.Clear();
                    name.Clear();
                    comstrand.SelectedIndex = 0;
                    comsection.SelectedIndex = 0;
                    yearlevel.SelectedIndex = 0;
                    studnum.Focus();
                    parentemail.Clear();
                    parentcontact.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void back_Click(object sender, EventArgs e)
        {
        }

        private void name_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void yearlevel_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
