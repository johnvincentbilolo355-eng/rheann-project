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
    public partial class studentInfo : Form
    {
        private string studentNo;
        public studentInfo(string studentNo)
        {
            InitializeComponent();
            this.studentNo = studentNo;
        }

        private void studentInfo_Load(object sender, EventArgs e)
        {
            LoadStudentInfo();

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
        private void LoadStudentInfo()
        {
            using (MySqlConnection conn = new MySqlConnection(DBConfig.ConnectionString))
            {
                conn.Open();

                // 1️⃣ Student + latest report
                string query = @"
                        SELECT 
                            s.student_name,
                            s.student_no,
                            s.section,
                            s.strand,
                            s.violations,
                            s.parent_contact,
                            s.email,
                            sr.report_type,
                            sr.category,
                            sr.priority_level,
                            sr.report_title,
                            sr.description
                        FROM students s
                        LEFT JOIN student_reports sr ON s.student_no = sr.student_no
                        WHERE s.student_no = @studentNo
                        ORDER BY sr.report_date DESC
                        LIMIT 1;
                        ";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@studentNo", studentNo);

                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    name.Text = reader["student_name"].ToString();
                    studno.Text = reader["student_no"].ToString();
                    section.Text = reader["section"].ToString();
                    reportype.Text = reader["report_type"].ToString();
                    category.Text = reader["category"].ToString();
                    priority.Text = reader["priority_level"].ToString();
                    reportitle.Text = reader["report_title"].ToString();
                    strand.Text = reader["strand"].ToString();
                    violation.Text = reader["violations"].ToString();
                    parentContact.Text = reader["parent_contact"].ToString();

                    // ✅ Email Link
                    emailink.Text = reader["email"].ToString();
                    emailink.LinkClicked += Emailink_LinkClicked; // <-- add this line
                }
                reader.Close();


                // 2️⃣ All violations from behavior_logs
                string violationQuery = @"
                                    SELECT violation_type
                                    FROM behavior_logs
                                    WHERE student_no = @studentNo;
                                    ";

                MySqlCommand vcmd = new MySqlCommand(violationQuery, conn);
                vcmd.Parameters.AddWithValue("@studentNo", studentNo);

                MySqlDataReader vreader = vcmd.ExecuteReader();

                List<string> violations = new List<string>();

                while (vreader.Read())
                {
                    violations.Add(vreader["violation_type"].ToString());
                }

                vreader.Close();

                // join all violations with comma
                violationtype.Text = "" + string.Join(Environment.NewLine, violations);

            }
        }

        private void Emailink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string email = emailink.Text;

            // Read values from labels
            string studentName = name.Text;
            string studentNumber = studno.Text;
            string violationType = violationtype.Text;   // from your label
            string reportTitle = reportitle.Text;
            string categoryText = category.Text;
            string priorityLevel = priority.Text;

            // Professional letter body
            string subject = "Notice: Student Violation Report";
            string body = $"Hello Ma’am/Sir,\n\n" +
                          $"This is to inform you that your child, {studentName} ({studentNumber}), " +
                          $"has committed a violation as recorded in our school system.\n\n" +
                          $"Violation Details:\n" +
                          $"• Violation Type: {violationType}\n" +
                          $"• Report Title: {reportTitle}\n" +
                          $"• Category: {categoryText}\n" +
                          $"• Priority Level: {priorityLevel}\n\n" +
                          $"Please coordinate with the school for proper guidance and counseling.\n\n" +
                          $"Thank you,\n" +
                          $"School Administration";

            // Gmail compose link
            string mailto = $"https://mail.google.com/mail/?view=cm&fs=1&to={email}&su={Uri.EscapeDataString(subject)}&body={Uri.EscapeDataString(body)}";

            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = mailto,
                UseShellExecute = true
            });
        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
