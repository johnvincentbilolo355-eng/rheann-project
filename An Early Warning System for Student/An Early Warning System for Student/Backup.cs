using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace An_Early_Warning_System_for_Student
{

    public partial class Backup : Form
    {
        private string connStr = DBConfig.ConnectionString;
        public Backup()
        {
            InitializeComponent();
        }

        private void cmbrecord_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private string EscapeSQL(object value)
        {
            if (value == null || value == DBNull.Value)
                return "NULL";

            if (value is DateTime dt)
                return $"'{dt:yyyy-MM-dd HH:mm:ss}'";

            if (value is TimeSpan ts)
                return $"'{ts:hh\\:mm\\:ss}'";

            if (value is string s)
                return $"'{s.Replace("'", "''")}'";

            return value.ToString(); // numbers
        }


        private void backupbox_Click(object sender, EventArgs e)
        {
            if (cmbrecord.SelectedItem == null)
            {
                MessageBox.Show("Please select a table to backup.");
                return;
            }

            string tableName = cmbrecord.SelectedItem.ToString();

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "SQL Files (*.sql)|*.sql";
                sfd.FileName = tableName + "_backup.sql";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (MySqlConnection conn = new MySqlConnection(connStr))
                        {
                            conn.Open();

                            // 1. Get CREATE TABLE statement
                            MySqlCommand createCmd = new MySqlCommand("SHOW CREATE TABLE " + tableName, conn);
                            string createSQL = "";
                            using (MySqlDataReader reader = createCmd.ExecuteReader())
                            {
                                if (reader.Read())
                                    createSQL = reader["Create Table"].ToString() + ";";
                            }

                            // 2. Get table data and build INSERT statements
                            MySqlCommand selectCmd = new MySqlCommand("SELECT * FROM " + tableName, conn);
                            StringBuilder insertSQL = new StringBuilder();

                            using (MySqlDataReader reader = selectCmd.ExecuteReader())
                            {
                                string[] columnNames = Enumerable.Range(0, reader.FieldCount)
                                    .Select(i => reader.GetName(i))
                                    .ToArray();

                                while (reader.Read())
                                {
                                    string[] values = Enumerable.Range(0, reader.FieldCount)
                                        .Select(i => EscapeSQL(reader.GetValue(i)))
                                        .ToArray();

                                    insertSQL.AppendLine($"INSERT INTO {tableName} ({string.Join(", ", columnNames)}) VALUES ({string.Join(", ", values)});");
                                }
                            }

                            // 3. Write all to file
                            File.WriteAllText(sfd.FileName, createSQL + Environment.NewLine + insertSQL.ToString());

                            MessageBox.Show("Table backed up successfully.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Backup failed: " + ex.Message);
                    }
                }
            }
        }


        private void restorebox_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Filter = "SQL Files (*.sql)|*.sql",
                Title = "Select SQL File to Restore"
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string sqlContent = File.ReadAllText(ofd.FileName);
                    string[] commands = sqlContent.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                    using (MySqlConnection conn = new MySqlConnection(connStr))
                    {
                        conn.Open();

                        foreach (string cmdText in commands)
                        {
                            string trimmed = cmdText.Trim();
                            if (!string.IsNullOrEmpty(trimmed) && trimmed.StartsWith("INSERT INTO"))
                            {
                                MySqlCommand cmd = new MySqlCommand(trimmed, conn);
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }

                    MessageBox.Show("Data restored successfully from the backup file.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Restore failed: " + ex.Message);
                }
            }
        }

        private void Backup_Load(object sender, EventArgs e)
        {
            LoadTables();

            // Optional: focus sa combo box para hindi automatic mag-click ang button
            this.ActiveControl = cmbrecord;
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

        private void LoadTables()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("SHOW TABLES", conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    cmbrecord.Items.Clear();
                    while (reader.Read())
                        cmbrecord.Items.Add(reader[0].ToString());

                    if (cmbrecord.Items.Count > 0)
                        cmbrecord.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading tables: " + ex.Message);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnClearRecords_Click(object sender, EventArgs e)
        {
          
        }

        private void deleterec_Click(object sender, EventArgs e)
        {
            if (cmbrecord.SelectedItem == null)
            {
                MessageBox.Show("Please select a table first.");
                return;
            }

            string tableName = cmbrecord.SelectedItem.ToString();

            DialogResult result = MessageBox.Show(
                $"This will permanently DELETE ALL records from `{tableName}`.\n\nContinue?",
                "WARNING",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result != DialogResult.Yes)
                return;

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;

                try
                {
                    cmd.CommandText = "SET FOREIGN_KEY_CHECKS = 0;";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = $"DELETE FROM `{tableName}`;";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "SET FOREIGN_KEY_CHECKS = 1;";
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Records deleted successfully.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Delete failed: " + ex.Message);
                }
            }
        }
    }
}
