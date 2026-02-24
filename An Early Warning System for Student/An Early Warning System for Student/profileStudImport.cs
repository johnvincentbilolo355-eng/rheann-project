using ExcelDataReader;
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
    public partial class profileStudImport : Form
    {
        public profileStudImport()
        {
            InitializeComponent();
        }

        private void importfile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Excel Files|*.xls;*.xlsx;*.csv";
            if (ofd.ShowDialog() != DialogResult.OK) return;

            string filePath = ofd.FileName;

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                IExcelDataReader reader;

                if (Path.GetExtension(filePath).ToLower() == ".csv")
                {
                    reader = ExcelReaderFactory.CreateCsvReader(stream, new ExcelReaderConfiguration()
                    {
                        FallbackEncoding = Encoding.UTF8
                    });
                }
                else
                {
                    reader = ExcelReaderFactory.CreateReader(stream);
                }

                var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                {
                    ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                    {
                        UseHeaderRow = true
                    }
                });

                DataTable dt = result.Tables[0]; // unang sheet

                using (MySqlConnection conn = new MySqlConnection(DBConfig.ConnectionString))
                {
                    conn.Open();

                    foreach (DataRow row in dt.Rows)
                    {
                        string studentNo = row["Student ID"].ToString();
                        string yearLevel = row["Year Level"].ToString();
                        string strand = row["Strand"].ToString();
                        string gender = row["Gender"].ToString();
                        int age = int.TryParse(row["Age"].ToString(), out int a) ? a : 0;
                        string mEdu = row["M_Edu"].ToString();
                        string mJob = row["M_job"].ToString();
                        string fEdu = row["F_Edu"].ToString();
                        string fJob = row["Fathers_job"].ToString();
                        string illness = row["Illness"].ToString();
                        int siblings = int.TryParse(row["siblings"].ToString(), out int s) ? s : 0;

                        string query = @"
                    INSERT INTO student_profile 
                    (student_no, year_level, strand, gender, age, m_edu, m_job, f_edu, f_job, illness, siblings)
                    VALUES
                    (@student_no, @year_level, @strand, @gender, @age, @m_edu, @m_job, @f_edu, @f_job, @illness, @siblings)
                    ON DUPLICATE KEY UPDATE
                        year_level=@year_level,
                        strand=@strand,
                        gender=@gender,
                        age=@age,
                        m_edu=@m_edu,
                        m_job=@m_job,
                        f_edu=@f_edu,
                        f_job=@f_job,
                        illness=@illness,
                        siblings=@siblings;
                ";

                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@student_no", studentNo);
                            cmd.Parameters.AddWithValue("@year_level", yearLevel);
                            cmd.Parameters.AddWithValue("@strand", strand);
                            cmd.Parameters.AddWithValue("@gender", gender);
                            cmd.Parameters.AddWithValue("@age", age);
                            cmd.Parameters.AddWithValue("@m_edu", mEdu);
                            cmd.Parameters.AddWithValue("@m_job", mJob);
                            cmd.Parameters.AddWithValue("@f_edu", fEdu);
                            cmd.Parameters.AddWithValue("@f_job", fJob);
                            cmd.Parameters.AddWithValue("@illness", illness);
                            cmd.Parameters.AddWithValue("@siblings", siblings);

                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                MessageBox.Show("Import successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();   
        }

        private void profileStudImport_Load(object sender, EventArgs e)
        {
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
    }
}
