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

    public partial class Guidance : Form
    {
        private string userId;

        public Guidance(string id)
        {
            InitializeComponent();
            userId = id;

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

        private void panelmain_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            label1.Text = "All Reports";

            submitReports rep = new submitReports();
            switchPanel(rep);
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {

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

        private void Guidance_Load(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;
            submitReports rep = new submitReports();
            switchPanel(rep);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            profileStudImport ip = new profileStudImport();
            ip.Show();
        }
    }
}
