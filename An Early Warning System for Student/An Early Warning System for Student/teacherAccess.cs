using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Drawing.Drawing2D;
using System.Net;
using System.Net.Mail;

namespace An_Early_Warning_System_for_Student
{
    public partial class teacherAccess : Form
    {
        bool isPasswordVisible = false;
        Form1 mainForm; // field at class level

        public teacherAccess(Form1 form)
        {
            InitializeComponent();
            mainForm = form;

            Shown += (_, _) => ApplyRoundedCorners();
            SizeChanged += (_, _) => ApplyRoundedCorners();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
        private void teacherAccess_Load(object sender, EventArgs e)
        {
            eyeopen.Visible = false;
            eyeclose.Visible = true;
            password.UseSystemPasswordChar = true;

            id.KeyDown += textbox1_KeyDown;
            password.KeyDown += textbox2_KeyDown;

            ApplyRoundedCorners();

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
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();   
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
        private void textbox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                password.Focus(); // lipat sa textbox2
                e.SuppressKeyPress = true;
            }
        }

        private void textbox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                login.PerformClick(); // login pag enter sa 2nd textbox
                e.SuppressKeyPress = true;
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {

        }
        private void SendOTPEmail(string toEmail, string otp)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("seepathearlywarningsystem@gmail.com"); // PALITAN MO
                mail.To.Add(toEmail);
                mail.Subject = "Your OTP Code - Early Warning System";
                mail.Body = "Your OTP code is: " + otp + "\n\nThis code will expire in 5 minutes.";

                SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                smtp.Port = 587;
                smtp.Credentials = new NetworkCredential("seepathearlywarningsystem@gmail.com", "piov robn nnas ehim");
                smtp.EnableSsl = true;

                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to send OTP Email.\n" + ex.Message);
            }
        }
        private string GenerateOTP()
        {
            Random rnd = new Random();
            return rnd.Next(100000, 999999).ToString(); // 6-digit OTP
        }
        private void login_Click(object sender, EventArgs e)
        {
            // --- VALIDATION ---
            if (string.IsNullOrWhiteSpace(id.Text) && string.IsNullOrWhiteSpace(password.Text))
            {
                MessageBox.Show("Please enter your ID No. and Password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (string.IsNullOrWhiteSpace(id.Text))
            {
                MessageBox.Show("Please enter your ID No.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                id.Focus();
                return;
            }
            else if (string.IsNullOrWhiteSpace(password.Text))
            {
                MessageBox.Show("Please enter your Password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                password.Focus();
                return;
            }
            // --- DATABASE CHECK ---
            string hashedInputPassword = HashPassword(password.Text);

            try
            {
                using (MySqlConnection conn = new MySqlConnection(DBConfig.ConnectionString))
                {
                    conn.Open();

                    string query = @"
                    SELECT id_no, firstname, year_level, strand, section, email 
                    FROM users 
                    WHERE id_no = @id 
                      AND password = @pass 
                      AND role = 'teacher' 
                      AND status = 'active'
                    LIMIT 1";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id.Text);
                        cmd.Parameters.AddWithValue("@pass", hashedInputPassword);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string idNo = reader["id_no"].ToString();
                                string firstname = reader["firstname"].ToString();
                                string year = reader["year_level"].ToString();
                                string strand = reader["strand"].ToString();
                                string section = reader["section"].ToString();
                                string email = reader["email"].ToString();

                                reader.Close();

                                string otp = GenerateOTP();

                                string insertOtp = @"
    INSERT INTO login_otp (id_no, otp_code, expires_at, is_used)
    VALUES (@id, @otp, DATE_ADD(NOW(), INTERVAL 5 MINUTE), 0)";

                                MySqlCommand otpCmd = new MySqlCommand(insertOtp, conn);
                                otpCmd.Parameters.AddWithValue("@id", idNo);
                                otpCmd.Parameters.AddWithValue("@otp", otp);
                                otpCmd.ExecuteNonQuery();

                                // ✅ SEND EMAIL HERE
                                SendOTPEmail(email, otp);

                                MessageBox.Show("OTP has been sent to your email.", "Success");
                                this.Hide(); // hide teacherAccess

                                OTPVerification otpForm = new OTPVerification(mainForm, idNo, firstname, year, strand, section);

                                otpForm.Show();
                            }

                            else
                            {
                                MessageBox.Show("Invalid ID No. or Password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                id.Focus();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void password_TextChanged(object sender, EventArgs e)
        {

        }

        private void eyeclose_Click(object sender, EventArgs e)
        {
            password.UseSystemPasswordChar = false; // show password
            eyeclose.Visible = false;
            eyeopen.Visible = true;
            isPasswordVisible = true;
        }

        private void eyeopen_Click(object sender, EventArgs e)
        {
            password.UseSystemPasswordChar = true;  // hide password
            eyeopen.Visible = false;
            eyeclose.Visible = true;
            isPasswordVisible = false;
        }
    }
}
