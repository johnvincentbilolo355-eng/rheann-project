using ExcelDataReader;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace An_Early_Warning_System_for_Student
{
    public partial class GradeComponent : Form
    {
        public string LoggedInId { get; private set; }
        string connStr = DBConfig.ConnectionString;
        public GradeComponent(string idNo)
        {
            InitializeComponent();
            LoggedInId = idNo;
        }

        private void GradeComponent_Load(object sender, EventArgs e)
        {
            termcbox.Items.AddRange(new string[] { "Midterm", "Finals" });
            semcbox.Items.AddRange(new string[] { "1st", "2nd" });

            termcbox.SelectedIndex = 0;
            semcbox.SelectedIndex = 0;

        }

        // Compute button click: shows final grade
        private void btnCompute_Click(object sender, EventArgs e)
        {

        }

        private void btnCompute_Click_1(object sender, EventArgs e)
        {

        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
        }
        private void guna2ButtonAdd_Click(object sender, EventArgs e)
        {
            if (dataGridView1.DataSource == null) return;

            DataTable dt = (DataTable)dataGridView1.DataSource;
            string teacherId = LoggedInId;

            // Get subject name correctly
            string subjectName = dt.TableName;

            using (MySqlConnection conn = new MySqlConnection(DBConfig.ConnectionString))
            {
                conn.Open();

                var existingStudents = LoadStudentNumbers(conn);
                var missingStudents = new HashSet<string>(StringComparer.Ordinal);
                int inserted = 0;
                int skippedDuplicate = 0;

                foreach (DataRow row in dt.Rows)
                {
                    string studentNo = NormalizeStudentNo(row[0]);
                    if (string.IsNullOrWhiteSpace(studentNo))
                        continue; // assume first column is Student ID

                    // Use exact column names from DataTable
                    double midterm = 0, finals = 0, attendanceRate = 0;
                    int absences = 0;

                    // Try parse Midterm
                    if (dt.Columns.Contains("Midterm") || dt.Columns.Contains("Midterm Grade"))
                    {
                        string midCol = dt.Columns.Contains("Midterm") ? "Midterm" : "Midterm Grade";
                        double.TryParse(row[midCol].ToString().Trim(), out midterm);
                    }

                    // Try parse Finals
                    if (dt.Columns.Contains("Finals") || dt.Columns.Contains("Finals Grade"))
                    {
                        string finCol = dt.Columns.Contains("Finals") ? "Finals" : "Finals Grade";
                        double.TryParse(row[finCol].ToString().Trim(), out finals);
                    }

                    // Try parse Absences
                    if (dt.Columns.Contains("Absences"))
                        int.TryParse(row["Absences"].ToString().Trim(), out absences);

                    // Try parse Attendance Rate
                    if (dt.Columns.Contains("Attendance Rate"))
                        double.TryParse(row["Attendance Rate"].ToString().Trim(), out attendanceRate);

                    // ✅ Compute accurate GPA
                    double gpa = Math.Round((midterm + finals) / 2.0, 2);

                    // Check if student exists (avoid per-row DB query)
                    if (!existingStudents.Contains(studentNo))
                    {
                        missingStudents.Add(studentNo);
                        continue;
                    }

                    // Prevent duplicate subject per student
                    string duplicateCheck = @"SELECT COUNT(*) FROM subjects 
                                      WHERE student_no=@student_no AND subject_name=@subject_name";
                    MySqlCommand dupCmd = new MySqlCommand(duplicateCheck, conn);
                    dupCmd.Parameters.AddWithValue("@student_no", studentNo);
                    dupCmd.Parameters.AddWithValue("@subject_name", subjectName);

                    int exists = Convert.ToInt32(dupCmd.ExecuteScalar());
                    if (exists > 0)
                    {
                        skippedDuplicate++;
                        continue;
                    }

                    // Insert into database
                    string insertQuery = @"
                INSERT INTO subjects
                (student_no, subject_name, midterm_grade, final_grade, gpa, attendance_rate, absences, teacher_id)
                VALUES
                (@student_no, @subject_name, @midterm, @finals, @gpa, @attendance_rate, @absences, @teacher_id)";

                    MySqlCommand cmd = new MySqlCommand(insertQuery, conn);
                    cmd.Parameters.AddWithValue("@student_no", studentNo);
                    cmd.Parameters.AddWithValue("@subject_name", subjectName);
                    cmd.Parameters.AddWithValue("@midterm", midterm);
                    cmd.Parameters.AddWithValue("@finals", finals);
                    cmd.Parameters.AddWithValue("@gpa", gpa);
                    cmd.Parameters.AddWithValue("@attendance_rate", attendanceRate);
                    cmd.Parameters.AddWithValue("@absences", absences);
                    cmd.Parameters.AddWithValue("@teacher_id", teacherId);

                    cmd.ExecuteNonQuery();
                    inserted++;
                }

                if (missingStudents.Count > 0)
                {
                    string sample = string.Join(", ", missingStudents.Take(10));
                    string extra = missingStudents.Count > 10 ? $" (+{missingStudents.Count - 10} more)" : string.Empty;
                    MessageBox.Show(
                        $"Import finished. Some students were not found in the Students table.\n\nMissing: {sample}{extra}\n\nTip: Import/Add those students first, then re-import grades.",
                        "Import Warning",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }

                MessageBox.Show(
                    $"Subject import completed.\n\nInserted: {inserted}\nSkipped (duplicate): {skippedDuplicate}\nMissing students: {missingStudents.Count}",
                    "Import Result",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private static HashSet<string> LoadStudentNumbers(MySqlConnection conn)
        {
            var set = new HashSet<string>(StringComparer.Ordinal);
            using var cmd = new MySqlCommand("SELECT student_no FROM students", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var value = reader[0]?.ToString();
                if (!string.IsNullOrWhiteSpace(value))
                    set.Add(value.Trim());
            }
            return set;
        }

        private static string NormalizeStudentNo(object value)
        {
            if (value == null || value == DBNull.Value)
                return string.Empty;

            string s = value.ToString()?.Trim() ?? string.Empty;
            if (s.Length == 0)
                return string.Empty;

            // Remove non-breaking spaces sometimes coming from Excel.
            s = s.Replace("\u00A0", string.Empty).Trim();

            // Common Excel artifact when ID is numeric: "12345.0"
            if (s.EndsWith(".0", StringComparison.Ordinal))
                s = s[..^2];

            // Scientific notation: "1.26E+11" -> "126000000000"
            if (s.IndexOf('E') >= 0 || s.IndexOf('e') >= 0)
            {
                if (decimal.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out var dec) ||
                    decimal.TryParse(s, NumberStyles.Float, CultureInfo.CurrentCulture, out dec))
                {
                    s = decimal.Truncate(dec).ToString("0", CultureInfo.InvariantCulture);
                }
            }

            return s;
        }



        private void ClearInputs()
        {

        }


        private void guna2Button1_Click(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }
        private Dictionary<string, double> ComputeTermAverage(DataTable dt)
        {
            Dictionary<string, List<double>> studentGrades = new Dictionary<string, List<double>>();

            foreach (DataRow row in dt.Rows)
            {
                string studentNo = row[0].ToString();

                if (double.TryParse(row[5].ToString(), out double grade))
                {
                    if (!studentGrades.ContainsKey(studentNo))
                        studentGrades[studentNo] = new List<double>();

                    studentGrades[studentNo].Add(grade);
                }
            }

            Dictionary<string, double> result = new Dictionary<string, double>();

            foreach (var item in studentGrades)
                result[item.Key] = Math.Round(item.Value.Average(), 2);

            return result;
        }



        private void guna2Button1_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Excel Files|*.xlsx;*.xls";

            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            using (var stream = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                // Read all rows first
                var result = reader.AsDataSet();
                DataTable rawTable = result.Tables[0];

                // Get Subject Name (first row, first column)
                string subjectName = rawTable.Rows[0][0].ToString().Trim();

                // Create a new DataTable for the actual data
                DataTable dt = new DataTable();
                int headerRowIndex = 1; // second row contains headers

                // Add columns from header row
                for (int i = 0; i < rawTable.Columns.Count; i++)
                {
                    dt.Columns.Add(rawTable.Rows[headerRowIndex][i].ToString());
                }

                // Add data rows (starting from row 2)
                for (int i = headerRowIndex + 1; i < rawTable.Rows.Count; i++)
                {
                    DataRow row = dt.NewRow();
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        row[j] = rawTable.Rows[i][j];
                    }
                    dt.Rows.Add(row);
                }

                dt.TableName = subjectName;
                dataGridView1.DataSource = dt;
            }
        }


        private void txtStudentNo_TextChanged(object sender, EventArgs e)
        {

        }

        private void semcbox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void termcbox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
