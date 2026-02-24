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
using System.Net;
using System.Net.Mail;

namespace An_Early_Warning_System_for_Student
{
    public partial class adminAccess : Form
    {
        bool isPasswordVisible = false;
        Form1 mainForm; // field at class level

        public adminAccess(Form1 form)
        {
            InitializeComponent();
            mainForm = form;

            Shown += (_, _) => ApplyRoundedCorners();
            SizeChanged += (_, _) => ApplyRoundedCorners();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // Optional: you can leave this empty
        }

        private void button2_Click(object sender, EventArgs e)
        {

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

        private void adminAccess_Load(object sender, EventArgs e)
        {
            ApplyRoundedCorners();

            // PASSWORD DEFAULT
            admin.UseSystemPasswordChar = true;

            eyeopen.Visible = false;
            eyeclose.Visible = true;

            // ✅ ADD THIS
            DrpPosition.Items.Clear();
            DrpPosition.Items.Add("SuperAdmin");
            DrpPosition.Items.Add("Guidance");
            DrpPosition.SelectedIndex = 0; // default selection
        }

        private void ApplyRoundedCorners()
        {
            if (WindowState == FormWindowState.Maximized)
            {
                Region = null;
                return;
            }

            int radius = 20;
            Region = new Region(GetRoundedPath(ClientRectangle, radius));
        }


        private void GenerateAndSendOTP(string idNo, string role)
        {
            string otp = GenerateOTP();

            using (MySqlConnection conn = new MySqlConnection(DBConfig.ConnectionString))
            {
                conn.Open();

                // 1️⃣ Save OTP to database
                string insert = @"
        INSERT INTO login_otp (id_no, otp_code, expires_at, is_used)
        VALUES (@id, @otp, DATE_ADD(NOW(), INTERVAL 5 MINUTE), 0)";

                MySqlCommand cmd = new MySqlCommand(insert, conn);
                cmd.Parameters.AddWithValue("@id", idNo);
                cmd.Parameters.AddWithValue("@otp", otp);
                cmd.ExecuteNonQuery();

                // 2️⃣ Get Email
                string emailQuery = "";

                if (role == "SuperAdmin")
                    emailQuery = "SELECT email FROM admin WHERE id_no = @id";
                else
                    emailQuery = "SELECT email FROM users WHERE id_no = @id";

                MySqlCommand emailCmd = new MySqlCommand(emailQuery, conn);
                emailCmd.Parameters.AddWithValue("@id", idNo);

                object result = emailCmd.ExecuteScalar();

                if (result != null)
                {
                    string email = result.ToString();
                    SendOTPEmail(email, otp);
                }
                else
                {
                    MessageBox.Show("Email not found for this account.");
                }
            }
        }

        private void SendOTPEmail(string toEmail, string otp)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("seepathearlywarningsystem@gmail.com");
                mail.To.Add(toEmail);
                mail.Subject = "Your OTP Code - Early Warning System";
                mail.Body = "Your OTP code is: " + otp +
                            "\n\nThis code will expire in 5 minutes.";

                SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                smtp.Port = 587;
                smtp.Credentials = new NetworkCredential(
                    "seepathearlywarningsystem@gmail.com",
                    "piov robn nnas ehim"   // app password
                );
                smtp.EnableSsl = true;

                smtp.Send(mail);

                MessageBox.Show("OTP has been sent to your email.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to send OTP Email.\n" + ex.Message);
            }
        }

        private string GenerateOTP()
        {
            Random rnd = new Random();
            return rnd.Next(100000, 999999).ToString();
        }
        private void Login()
        {
            using (MySqlConnection conn = new MySqlConnection(DBConfig.ConnectionString))
            {
                try
                {
                    conn.Open();

                    string inputId = admin.Text.Trim();
                    string selectedRole = DrpPosition.SelectedItem?.ToString();

                    if (string.IsNullOrEmpty(selectedRole))
                    {
                        MessageBox.Show("Please select a role first.");
                        return;
                    }

                    string query = "";

                    if (selectedRole == "SuperAdmin")
                    {
                        query = "SELECT id_no FROM admin WHERE id_no = @id_no";
                    }
                    else if (selectedRole == "Guidance")
                    {
                        query = "SELECT id_no FROM users WHERE id_no = @id_no AND isAdmin = 1";
                    }

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id_no", inputId);

                    if (cmd.ExecuteScalar() != null)
                    {
                        GenerateAndSendOTP(inputId, selectedRole);

                        AdminOTPVerification otpForm =
                            new AdminOTPVerification(mainForm, inputId, selectedRole);

                        otpForm.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Invalid ID for selected role!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database error: " + ex.Message);
                }
            }
        }


        private void login_Click(object sender, EventArgs e)
        {
            Login();
        }
        private void admin_TextChanged(object sender, EventArgs e)
        {

        }

        private void admin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Login();
                e.SuppressKeyPress = true;
            }
        }

        private void eyeopen_Click(object sender, EventArgs e)
        {
            admin.UseSystemPasswordChar = true;
            eyeopen.Visible = false;
            eyeclose.Visible = true;
            isPasswordVisible = false;
        }

        private void eyeclose_Click(object sender, EventArgs e)
        {
            admin.UseSystemPasswordChar = false;
            eyeclose.Visible = false;
            eyeopen.Visible = true;
            isPasswordVisible = true;
        }

        private void DrpPosition_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ROLE IF GUIDANCE - ADMIN OR ADMIN - SUPER ADMIN
        }
    }
}
