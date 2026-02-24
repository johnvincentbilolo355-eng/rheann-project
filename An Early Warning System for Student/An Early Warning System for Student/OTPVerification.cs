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
    public partial class OTPVerification : Form
    {
        string userId;
        string firstname, strand, section, yearLevel;


        // Timer fields
        private System.Windows.Forms.Timer otpTimer;
        private int remainingSeconds = 300; // 5 minutes
        Form1 mainForm;
        public OTPVerification(Form1 form, string id, string fname, string year, string str, string sec)
        {
            InitializeComponent();
            mainForm = form;
            userId = id;
            firstname = fname;
            yearLevel = year;
            strand = str;
            section = sec;

            Shown += (_, _) => ApplyRoundedCorners();
            SizeChanged += (_, _) => ApplyRoundedCorners();
        }

        private void OTPVerification_Load(object sender, EventArgs e)
        {
            otpTimer = new System.Windows.Forms.Timer();
            otpTimer.Interval = 1000; // 1 second
            otpTimer.Tick += OtpTimer_Tick;

            remainingSeconds = 300;
            UpdateTimerLabel();

            otpTimer.Start();
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
                MessageBox.Show("OTP expired. Please request a new one.");
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

                        // ✅ STOP TIMER para hindi lumabas OTP expired
                        otpTimer.Stop();

                        // Hide Form1 and OTPVerification
                        mainForm.Hide(); // hide Form1
                        this.Close();     // hide OTPVerification

                        // Open MainPage
                        MainPage main = new MainPage(userId, firstname, yearLevel, strand, section);
                        main.Show();
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