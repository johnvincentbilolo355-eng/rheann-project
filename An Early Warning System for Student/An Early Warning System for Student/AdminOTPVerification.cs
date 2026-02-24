using MySql.Data.MySqlClient;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace An_Early_Warning_System_for_Student
{
    public partial class AdminOTPVerification : Form
    {
        private string userId;
        private string role; // SuperAdmin or Guidance

        private System.Windows.Forms.Timer otpTimer;
        private int remainingSeconds = 300; // 5 minutes
        Form1 mainForm;
        public AdminOTPVerification(Form1 form,  string idNo, string userRole)
        {
            InitializeComponent();
            mainForm = form;
            userId = idNo;
            role = userRole;

            Shown += (_, _) => ApplyRoundedCorners();
            SizeChanged += (_, _) => ApplyRoundedCorners();
        }

        private void AdminOTPVerification_Load(object sender, EventArgs e)
        {
            ApplyRoundedCorners();

            // Start Timer
            otpTimer = new System.Windows.Forms.Timer();
            otpTimer.Interval = 1000;
            otpTimer.Tick += OtpTimer_Tick;

            remainingSeconds = 300;
            UpdateTimerLabel();
            otpTimer.Start();
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

            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();

            return path;
        }

        private void UpdateTimerLabel()
        {
            int minutes = remainingSeconds / 60;
            int seconds = remainingSeconds % 60;
            lblTimer.Text = $"{minutes:D2}:{seconds:D2}";
        }

        private void OtpTimer_Tick(object sender, EventArgs e)
        {
            if (remainingSeconds > 0)
            {
                remainingSeconds--;
                UpdateTimerLabel();
            }
            else
            {
                otpTimer.Stop();
                MessageBox.Show("OTP expired. Please login again.");
                btnVerify.Enabled = false;
            }
        }

        private void btnVerify_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtOTP.Text))
            {
                MessageBox.Show("Please enter OTP.");
                return;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(DBConfig.ConnectionString))
                {
                    conn.Open();

                    string query = @"
                SELECT * FROM login_otp
                WHERE id_no = @id
                AND otp_code = @otp
                AND is_used = 0
                AND expires_at >= NOW()
                LIMIT 1";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", userId);
                    cmd.Parameters.AddWithValue("@otp", txtOTP.Text);

                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        reader.Close();

                        string update = @"
                        UPDATE login_otp
                        SET is_used = 1
                        WHERE id_no = @id AND otp_code = @otp";

                        MySqlCommand updateCmd = new MySqlCommand(update, conn);
                        updateCmd.Parameters.AddWithValue("@id", userId);
                        updateCmd.Parameters.AddWithValue("@otp", txtOTP.Text);
                        updateCmd.ExecuteNonQuery();

                        MessageBox.Show("OTP Verified!");

                        // Stop timer properly
                        otpTimer.Stop();
                        otpTimer.Dispose();

                        btnVerify.Enabled = false;

                        mainForm.Hide();
                        this.Hide();

                        // Redirect based on role
                        if (role == "SuperAdmin")
                        {
                            AdminDashboard dash = new AdminDashboard(userId);
                            dash.Show();
                        }
                        else if (role == "Guidance")
                        {
                            Guidance guide = new Guidance(userId);
                            guide.Show();
                        }

                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Invalid or expired OTP.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void txtOTP_TextChanged(object sender, EventArgs e)
        {
        }

        private void lblTimer_Click(object sender, EventArgs e)
        {
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}