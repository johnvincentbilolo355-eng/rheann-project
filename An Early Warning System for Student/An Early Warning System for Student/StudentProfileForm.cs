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
    public partial class StudentProfileForm : Form
    {
        string studentNo;
        string connStr = DBConfig.ConnectionString;
        public StudentProfileForm(string studentNo)
        {
            InitializeComponent();
            this.studentNo = studentNo;
        }

        private void StudentProfileForm_Load(object sender, EventArgs e)
        {
            LoadProfile();
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
        private void LoadProfile()
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string query = "SELECT * FROM student_profile WHERE student_no=@student_no";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@student_no", studentNo);
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            lblStudentNo.Text = dr["student_no"].ToString();
                            lblYearLevel.Text = dr["year_level"].ToString();
                            lblStrand.Text = dr["strand"].ToString();
                            lblGender.Text = dr["gender"].ToString();
                            lblAge.Text = dr["age"].ToString();
                            lblMEdu.Text = dr["m_edu"].ToString();
                            lblMJob.Text = dr["m_job"].ToString();
                            lblFEdu.Text = dr["f_edu"].ToString();
                            lblFJob.Text = dr["f_job"].ToString();
                            lblIllness.Text = dr["illness"].ToString();
                            lblSiblings.Text = dr["siblings"].ToString();
                        }
                        else
                        {
                            MessageBox.Show("Profile not found for this student.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}