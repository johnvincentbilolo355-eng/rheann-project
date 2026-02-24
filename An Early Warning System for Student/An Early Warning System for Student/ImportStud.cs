using ExcelDataReader;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace An_Early_Warning_System_for_Student
{
    public partial class ImportStud : Form
    {
        public ImportStud()
        {
            InitializeComponent();
        }

        private static string NormalizeNumericText(object value)
        {
            if (value == null) return "";
            string text = value.ToString().Trim();
            if (string.IsNullOrWhiteSpace(text)) return "";

            // Excel sometimes turns numbers into scientific notation or adds .0
            if (double.TryParse(text, out double num))
                return num.ToString("0");

            // Common cleanup
            if (text.EndsWith(".0", StringComparison.Ordinal))
                text = text.Substring(0, text.Length - 2);

            return text.Trim();
        }

        private static string NormalizeYearLevel(object value)
        {
            string text = (value ?? "").ToString().Trim();
            if (string.IsNullOrWhiteSpace(text)) return "";

            // If it's numeric like 11 or 11.0
            string numeric = NormalizeNumericText(text);
            if (numeric == "11" || numeric == "12")
                return numeric;

            // If it's like "Grade 11" / "G11" / etc.
            if (text.Contains("11")) return "11";
            if (text.Contains("12")) return "12";

            return text;
        }

        private static string NormalizeStrand(object value)
        {
            string text = (value ?? "").ToString().Trim();
            return string.IsNullOrWhiteSpace(text) ? "" : text.ToUpperInvariant();
        }

        private void ImportStud_Load(object sender, EventArgs e)
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
        private void importfile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "Excel Files|*.xls;*.xlsx;*.csv",
                Title = "Select Excel File"
            };

            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            string filePath = ofd.FileName;

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = (_) => new ExcelDataTableConfiguration() { UseHeaderRow = true }
                    });

                    DataTable dt = result.Tables[0]; // first sheet

                    using (MySqlConnection conn = new MySqlConnection(DBConfig.ConnectionString))
                    {
                        conn.Open();

                        int studentsWritten = 0;
                        int rosterRowsAdded = 0;

                        foreach (DataRow row in dt.Rows)
                        {
                            string surname = row[0].ToString().Trim();
                            string firstname = row[1].ToString().Trim();
                            string studentName = $"{surname} {firstname}";

                            // Handle student_no properly (prevent scientific notation)
                            string studentNo = NormalizeNumericText(row[2]);

                            string yearLevel = NormalizeYearLevel(row[3]);
                            string strand = NormalizeStrand(row[4]);

                            // Handle section (nullable)
                            string sectionText = NormalizeNumericText(row[5]);
                            object sectionValue = string.IsNullOrWhiteSpace(sectionText) ? (object)DBNull.Value : sectionText;

                            // Skip row if essential fields are empty
                            if (string.IsNullOrWhiteSpace(studentName) || string.IsNullOrWhiteSpace(studentNo))
                                continue;

                            string insertQuery = @"
INSERT INTO students (student_name, student_no, year_level, strand, section)
VALUES (@studentName, @studentNo, @yearLevel, @strand, @section)
ON DUPLICATE KEY UPDATE
student_name=@studentName, year_level=@yearLevel, strand=@strand, section=@section;
";

                            using (MySqlCommand cmd = new MySqlCommand(insertQuery, conn))
                            {
                                cmd.Parameters.AddWithValue("@studentName", studentName);
                                cmd.Parameters.AddWithValue("@studentNo", studentNo);
                                cmd.Parameters.AddWithValue("@yearLevel", yearLevel);
                                cmd.Parameters.AddWithValue("@strand", strand);
                                cmd.Parameters.AddWithValue("@section", sectionValue);

                                int affected = cmd.ExecuteNonQuery();
                                if (affected > 0)
                                    studentsWritten++;
                            }

                            // Ensure imported students appear in existing class rosters.
                            // If classes already exist for this strand/section/year level, add student to student_classes.
                            if (!string.IsNullOrWhiteSpace(sectionText))
                            {
                                string insertStudentClasses = @"
INSERT INTO student_classes (student_no, subject_name, teacher_id, strand, section, year_level)
SELECT @studentNo, c.subject_name, NULL AS teacher_id, c.strand, c.section, c.year_level
FROM classes c
WHERE c.strand = @strand
  AND c.section = @sectionText
  AND c.year_level = @yearLevel
  AND (c.status IS NULL OR c.status = 'active')
  AND NOT EXISTS (
      SELECT 1
      FROM student_classes sc
      WHERE sc.student_no = @studentNo
        AND sc.subject_name = c.subject_name
        AND sc.strand = c.strand
        AND sc.section = c.section
        AND sc.year_level = c.year_level
  );
";

                                using (MySqlCommand cmd2 = new MySqlCommand(insertStudentClasses, conn))
                                {
                                    cmd2.Parameters.AddWithValue("@studentNo", studentNo);
                                    cmd2.Parameters.AddWithValue("@strand", strand);
                                    cmd2.Parameters.AddWithValue("@sectionText", sectionText);
                                    cmd2.Parameters.AddWithValue("@yearLevel", yearLevel);
                                    rosterRowsAdded += cmd2.ExecuteNonQuery();
                                }
                            }
                        }

                        MessageBox.Show(
                            $"Students imported successfully!\n\nStudents written: {studentsWritten}\nRoster rows added: {rosterRowsAdded}",
                            "Success",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
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
