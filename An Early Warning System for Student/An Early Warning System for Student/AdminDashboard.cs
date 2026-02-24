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
    public partial class AdminDashboard : Form
    {
        private string loggedInUser;
        public AdminDashboard(string firstname)
        {
            InitializeComponent();
            loggedInUser = firstname;

            ResponsiveLayout.Attach(this);

        }
        public void switchPanel(Form form)
        {
            panelmain.Controls.Clear();
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            panelmain.Controls.Add(form);
            form.Show();

            ResponsiveLayout.Attach(form);
        }
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            label1.Text = "Admin Dashboard";
            label2.Text = loggedInUser;

            admindash dash = new admindash();
            switchPanel(dash);
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void AdminDashboard_Load(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;
            label1.Text = "Admin Dashboard";
            label2.Text = loggedInUser;

            admindash dash = new admindash();
            switchPanel(dash);
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            label1.Text = "Teacher Management";
            label2.Text = loggedInUser;

            AdminTeacher teach = new AdminTeacher();
            switchPanel(teach);
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            label1.Text = "All Students";
            label2.Text = loggedInUser;

            adminViewStudent stud = new adminViewStudent();
            switchPanel(stud);
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to logout?",
                "Confirm Logout",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                Form1 login = new Form1();
                login.Show();
                this.Close(); // or Hide(), depende sa flow mo
            }
        }

        private void panelmain_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            label1.Text = "All Reports";
            label2.Text = loggedInUser;

            submitReports rep = new submitReports();
            switchPanel(rep);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            Backup bck = new Backup();
            bck.Show();
        }

        private void guna2Button7_Click(object sender, EventArgs e)
        {
            //label1.Text = "Classess";
            //label2.Text = loggedInUser;

            //TeacherClasses tc = new TeacherClasses();
            //switchPanel(tc);
        }

        private void guna2Button8_Click(object sender, EventArgs e)
        {
            label1.Text = "Archived Students";
            label2.Text = loggedInUser;

            Archived arc = new Archived();
            switchPanel(arc);
        }

        private void guna2Button9_Click(object sender, EventArgs e)
        {
            ImportStud ip = new ImportStud();
            ip.Show();
        }

        private void guna2Button10_Click(object sender, EventArgs e)
        {
            label1.Text = "ML Analytics";
            label2.Text = loggedInUser;

            MLAnalytics analytics = new MLAnalytics();
            switchPanel(analytics);
        }
    }
}
