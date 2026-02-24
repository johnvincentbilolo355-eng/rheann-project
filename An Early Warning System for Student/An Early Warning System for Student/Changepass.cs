using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace An_Early_Warning_System_for_Student
{

    public partial class Changepass : Form
    {
        private string loggedInUserId;
        bool isCurrentVisible = false;
        bool isNewVisible = false;
        bool isConfirmVisible = false;

        public Changepass(string userId)
        {
            InitializeComponent();
            loggedInUserId = userId;
        }
        private void currentpass_TextChanged(object sender, EventArgs e)
        {

        }
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                    builder.Append(bytes[i].ToString("x2"));

                return builder.ToString();
            }
        }
        private void Changepass_Load(object sender, EventArgs e)
        {
            // initial eye
            eyeopen.Visible = false;
            eyeclose.Visible = true;

            eyeopen1.Visible = false;
            eyeclose1.Visible = true;

            eyeopen2.Visible = false;
            eyeclose2.Visible = true;
            // Optional: make password chars hidden
            currentpass.UseSystemPasswordChar = true;
            newpass.UseSystemPasswordChar = true;
            confirmnewpass.UseSystemPasswordChar = true;
            int radius = 20;
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
        private void newpass_TextChanged(object sender, EventArgs e)
        {

        }

        private void confirmnewpass_TextChanged(object sender, EventArgs e)
        {

        }

        private void submit_Click(object sender, EventArgs e)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(currentpass.Text) ||
                string.IsNullOrWhiteSpace(newpass.Text) ||
                string.IsNullOrWhiteSpace(confirmnewpass.Text))
            {
                MessageBox.Show("Please fill all fields.");
                return;
            }

            if (newpass.Text != confirmnewpass.Text)
            {
                MessageBox.Show("New password and confirm password do not match.");
                return;
            }

            // Check current password
            string hashedCurrent = HashPassword(currentpass.Text);

            using (MySqlConnection conn = new MySqlConnection(DBConfig.ConnectionString))
            {
                conn.Open();

                string checkQuery = @"
                    SELECT password 
                    FROM users 
                    WHERE id_no = @id
                ";

                MySqlCommand cmd = new MySqlCommand(checkQuery, conn);
                cmd.Parameters.AddWithValue("@id", loggedInUserId);

                string dbPassword = cmd.ExecuteScalar()?.ToString();

                if (dbPassword != hashedCurrent)
                {
                    MessageBox.Show("Current password is incorrect.");
                    return;
                }

                // Update password
                string hashedNew = HashPassword(newpass.Text);

                string updateQuery = @"
                    UPDATE users 
                    SET password = @pass
                    WHERE id_no = @id
                ";

                MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn);
                updateCmd.Parameters.AddWithValue("@pass", hashedNew);
                updateCmd.Parameters.AddWithValue("@id", loggedInUserId);

                int rows = updateCmd.ExecuteNonQuery();

                if (rows > 0)
                {
                    MessageBox.Show("Password updated successfully!");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Update failed. Try again.");
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void eyeopen_Click(object sender, EventArgs e)
        {
            currentpass.UseSystemPasswordChar = true;
            eyeopen.Visible = false;
            eyeclose.Visible = true;
            isCurrentVisible = false;
        }

        private void eyeclose_Click(object sender, EventArgs e)
        {
            currentpass.UseSystemPasswordChar = false;
            eyeclose.Visible = false;
            eyeopen.Visible = true;
            isCurrentVisible = true;
        }

        private void eyeopen1_Click(object sender, EventArgs e)
        {
            currentpass.UseSystemPasswordChar = true;
            eyeopen.Visible = false;
            eyeclose.Visible = true;
            isCurrentVisible = false;
        }

        private void eyeclose1_Click(object sender, EventArgs e)
        {
            newpass.UseSystemPasswordChar = false;
            eyeclose1.Visible = false;
            eyeopen1.Visible = true;
            isNewVisible = true;
        }

        private void eyeopen2_Click(object sender, EventArgs e)
        {
            confirmnewpass.UseSystemPasswordChar = true;
            eyeopen2.Visible = false;
            eyeclose2.Visible = true;
            isConfirmVisible = false;
        }

        private void eyeclose2_Click(object sender, EventArgs e)
        {
            confirmnewpass.UseSystemPasswordChar = false;
            eyeclose2.Visible = false;
            eyeopen2.Visible = true;
            isConfirmVisible = true;
        }
    }
}
