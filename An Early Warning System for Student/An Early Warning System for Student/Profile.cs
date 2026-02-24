using Microsoft.VisualBasic.ApplicationServices;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace An_Early_Warning_System_for_Student
{
    public partial class Profile : Form
    {
        string loggedInUserId;
        public event Action<string> OnProfileUpdated;

        public Profile(string loggedInUsername)
        {
            InitializeComponent();
            loggedInUserId = loggedInUsername;
        }



        private void Profile_Load(object sender, EventArgs e)
        {
            LoadProfile();
        }

        private void LoadProfile()
        {
            string connStr = DBConfig.ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string query = @"
            SELECT 
                id_no, 
                firstname, 
                lastname, 
                email
            FROM users
            WHERE id_no = @id_no
        ";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id_no", loggedInUserId);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Labels (existing)
                        idno.Text = reader["id_no"].ToString();
                        name.Text = reader["firstname"].ToString();
                        namelast.Text = reader["lastname"].ToString();
                        emailadd.Text = reader["email"].ToString();

                        // TEXTBOXES (NEW) — fill them too
                        teacher_id.Text = reader["id_no"].ToString();
                        guna2TextBox3.Text = reader["firstname"].ToString();
                        guna2TextBox4.Text = reader["lastname"].ToString();
                        emailtxt.Text = reader["email"].ToString();


                        // If "date" is a TextBox use this instead:
                        // date.Text = Convert.ToDateTime(reader["birthdate"]).ToString("yyyy-MM-dd");
                    }
                }
            }
        }


        private void UpdateProfile()
        {
            string connStr = DBConfig.ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string updateQuery = @"
            UPDATE users
            SET
                firstname = @firstname,
                lastname = @lastname,
                email = @email
            WHERE id_no = @id_no
        ";

                MySqlCommand cmd = new MySqlCommand(updateQuery, conn);
                cmd.Parameters.AddWithValue("@firstname", guna2TextBox3.Text);
                cmd.Parameters.AddWithValue("@lastname", guna2TextBox4.Text);
                cmd.Parameters.AddWithValue("@email", emailtxt.Text);
                cmd.Parameters.AddWithValue("@id_no", loggedInUserId);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Profile updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // 🔥 Auto refresh sa Profile form
                    LoadProfile();

                    // 🔥 Update sa MainPage label2 (First Name)
                    OnProfileUpdated?.Invoke(guna2TextBox3.Text);
                }
                else
                {
                    MessageBox.Show("Update failed. Please try again.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void ChangePass_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Changepass pass = new Changepass(loggedInUserId);
            pass.Show();
        }
        private void submit_Click(object sender, EventArgs e)
        {
            UpdateProfile();
        }

        private void submit_Click_1(object sender, EventArgs e)
        {
            UpdateProfile();
        }

        private void idno_Click(object sender, EventArgs e)
        {

        }
    }
}
