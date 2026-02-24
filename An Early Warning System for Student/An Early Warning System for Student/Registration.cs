using MySql.Data.MySqlClient;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace An_Early_Warning_System_for_Student
{
    public partial class Registration : Form
    {
        private readonly string connStr = DBConfig.ConnectionString;
        bool isPasswordVisible = false;

        public Registration()
        {
            InitializeComponent();
            pass.UseSystemPasswordChar = true;
        }

        private void Registration_Load(object sender, EventArgs e)
        {
            eyeopen.Visible = false;
            eyeclose.Visible = true;
            // Populate Strands
            pictureBox3.Parent = pictureBox1;   // make panel the parent
            pictureBox3.BackColor = Color.Transparent;  // make picturebox transparent
           
            DrpStrands.Items.Clear();
            DrpStrands.Items.Add("Select Strand");
            DrpStrands.Items.AddRange(new string[] { "None", "STEM", "ABM", "ICT", "HE", "HUMS" });
            DrpStrands.SelectedIndex = 0;

            // Populate Sections
            DrpSection.Items.Clear();
            DrpSection.Items.Add("Select Section");
            DrpSection.Items.AddRange(new string[] { "None", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23"});
            DrpSection.SelectedIndex = 0;

            yearlevel.Items.Clear();
            yearlevel.Items.Add("Year Selection");
            yearlevel.Items.AddRange(new string[] { "None", "11", "12"});
            yearlevel.SelectedIndex = 0;

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
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // 🔐 HASH PASSWORD
        private string HashPassword(string plainPassword)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(plainPassword));
                StringBuilder sb = new StringBuilder();
                foreach (byte b in bytes)
                    sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {

        }

        private void date_ValueChanged(object sender, EventArgs e)
        {

        }

        private void submit_Click(object sender, EventArgs e)
        {
            // ===== VALIDATION =====
            if (string.IsNullOrWhiteSpace(id.Text) ||
                string.IsNullOrWhiteSpace(fname.Text) ||
                string.IsNullOrWhiteSpace(lname.Text) ||
                string.IsNullOrWhiteSpace(emailaddress.Text) ||
                string.IsNullOrWhiteSpace(pass.Text))
            {
                MessageBox.Show("Please fill out all required fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!emailaddress.Text.Contains("@"))
            {
                MessageBox.Show("Invalid email format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (DrpStrands.SelectedIndex == 0)
            {
                MessageBox.Show("Please select a Strand.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (DrpSection.SelectedIndex == 0)
            {
                MessageBox.Show("Please select a Section.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string hashedPassword = HashPassword(pass.Text);
            string selectedStrand = DrpStrands.SelectedIndex == 0 ? null : DrpStrands.SelectedItem.ToString();
            string selectedSection = DrpSection.SelectedIndex == 0 ? null : DrpSection.SelectedItem.ToString();
            string selectedYear = yearlevel.SelectedIndex == 0 ? null : yearlevel.SelectedItem.ToString();
            // ===== DATABASE INSERT =====
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    string query = @"INSERT INTO users 
    (id_no, firstname, lastname, email, password, role, status, strand, section, year_level)
    VALUES (@id, @first, @last, @mail, @pass, @role, @status, @strand, @section, @yearlevel)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id.Text);
                        cmd.Parameters.AddWithValue("@first", fname.Text);
                        cmd.Parameters.AddWithValue("@last", lname.Text);
                        cmd.Parameters.AddWithValue("@mail", emailaddress.Text);
                        cmd.Parameters.AddWithValue("@pass", hashedPassword);
                        cmd.Parameters.AddWithValue("@role", "teacher");
                        cmd.Parameters.AddWithValue("@status", "active");
                        cmd.Parameters.AddWithValue("@strand", selectedStrand);
                        cmd.Parameters.AddWithValue("@section", selectedSection);
                        cmd.Parameters.AddWithValue("@yearlevel", selectedYear);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Registration Successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // ===== CLEAR FIELDS =====
                id.Clear();
                fname.Clear();
                lname.Clear();
                emailaddress.Clear();
                pass.Clear();
                yearlevel.SelectedIndex = 0;
                DrpStrands.SelectedIndex = 0;
                DrpSection.SelectedIndex = 0;
                id.Focus();
            }
            catch (MySqlException ex) when (ex.Number == 1062)
            {
                MessageBox.Show("ID already registered.", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void id_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                fname.Focus();
                e.SuppressKeyPress = true;
            }
        }

        private void fname_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                lname.Focus();
                e.SuppressKeyPress = true;
            }
        }

        private void lname_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DrpStrands.Focus();
                e.SuppressKeyPress = true;
            }
        }

        private void emailaddress_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                pass.Focus();
                e.SuppressKeyPress = true;
            }
        }

        private void pass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                submit.Focus();
                e.SuppressKeyPress = true;
            }
        }

        private void eyeopen_Click_1(object sender, EventArgs e)
        {
            pass.UseSystemPasswordChar = true;
            eyeopen.Visible = false;
            eyeclose.Visible = true;
        }

        private void eyeclose_Click_1(object sender, EventArgs e)
        {
            pass.UseSystemPasswordChar = false;
            eyeclose.Visible = false;
            eyeopen.Visible = true;
        }

    }
}

