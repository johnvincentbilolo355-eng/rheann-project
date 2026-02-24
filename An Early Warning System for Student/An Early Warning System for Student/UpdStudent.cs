using MySql.Data.MySqlClient;
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

namespace An_Early_Warning_System_for_Student
{
    public partial class UpdStudent : Form
    {
        string connStr = DBConfig.ConnectionString;

        public UpdStudent()
        {
            InitializeComponent();
        }

        private void UpdStudent_Load(object sender, EventArgs e)
        {
            // Populate strands
            comstrand.Items.Clear();
            comstrand.Items.AddRange(new string[] { "STEM", "ABM", "ICT", "HE", "HUMS" });

            // Populate sections
            comsection.Items.Clear();
            comsection.Items.AddRange(new string[] { "None", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23" });

            yearlevel.Items.Clear();
            yearlevel.Items.AddRange(new string[] { "11", "12"});

            int radius = 20; // pwede mo palitan kung gusto mo mas round o less round
            this.Region = new Region(GetRoundedPath(this.ClientRectangle, radius));
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
        public void LoadStudentData(string sNo, string sName, string sStrand, string sSection, string sContact, string sEmail, string yearlevelValue)
        {
            studnum.Text = sNo;
            name.Text = sName;
            parentcontact.Text = sContact;
            parentemail.Text = sEmail;

            // Trim spaces dahil baka may trailing space sa DB
            string strand = sStrand.Trim();
            string section = sSection.Trim();
            string year = yearlevelValue.Trim();

            // --- Set Strand ---
            int strandIndex = comstrand.FindStringExact(strand);
            if (strandIndex >= 0)
                comstrand.SelectedIndex = strandIndex;

            // --- Set Section ---
            int sectionIndex = comsection.FindStringExact(section);
            if (sectionIndex >= 0)
                comsection.SelectedIndex = sectionIndex;

            // --- Set Year Level ---
            int yearIndex = yearlevel.FindStringExact(year);
            if (yearIndex >= 0)
                yearlevel.SelectedIndex = yearIndex;
        }

        private void studNo_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {

        }

        private void submit_Click(object sender, EventArgs e)
        {
            // Basic validations
            if (string.IsNullOrWhiteSpace(studnum.Text) ||
                string.IsNullOrWhiteSpace(name.Text) ||
                comstrand.SelectedItem == null ||
                comsection.SelectedItem == null ||
                yearlevel.SelectedItem == null ||
                string.IsNullOrWhiteSpace(parentcontact.Text) ||
                string.IsNullOrWhiteSpace(parentemail.Text))
            {
                MessageBox.Show("Please complete all fields.", "Update Failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    string query = @"UPDATE students
                             SET student_name = @name,
                                 strand = @strand,
                                 section = @section,
                                 parent_contact = @parent_contact,
                                 email = @email,
                                 year_level = @year
                             WHERE student_no = @studNo";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", name.Text.Trim());
                        cmd.Parameters.AddWithValue("@strand", comstrand.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@section", comsection.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@parent_contact", parentcontact.Text.Trim());
                        cmd.Parameters.AddWithValue("@email", parentemail.Text.Trim());
                        cmd.Parameters.AddWithValue("@studNo", studnum.Text.Trim());
                        cmd.Parameters.AddWithValue("@year", yearlevel.SelectedItem.ToString());


                        int rows = cmd.ExecuteNonQuery();

                        if (rows > 0)
                        {
                            MessageBox.Show("Student updated successfully!", "SUCCESS",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                            this.Close(); // Close update form
                        }
                        else
                        {
                            MessageBox.Show("No record updated.", "INFO",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating student: " + ex.Message);
            }
        }


        private void back_Click(object sender, EventArgs e)
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
