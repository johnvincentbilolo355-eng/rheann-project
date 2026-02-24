    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;
using System.Windows.Forms.Design.Behavior;
using static System.Collections.Specialized.BitVector32;

    namespace An_Early_Warning_System_for_Student
    {
    public partial class MainPage : Form
    {
        public string LoggedInId { get; private set; }
        public string LoggedInUser { get; private set; }
        private string teacherStrand;
        private string teacherSection;
        private string loggedInUser;
        private string teacherYearLevel;



        public MainPage(string idNo, string firstname, string year, string strand, string section)
        {
            InitializeComponent();

            loggedInUser = firstname;
            LoggedInUser = firstname;

            teacherYearLevel = year;
            teacherStrand = strand;
            teacherSection = section;
            LoggedInId = idNo;
            ResponsiveLayout.Attach(this);
        }


        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {

        }
        public void switchPanel(Form form)
        {
            panelmain.Controls.Clear();
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            panelmain.Controls.Add(form);
            form.Show();
        }
        private void btnProfile_Click(object sender, EventArgs e)
        {
            label1.Text = "Profile";
            label2.Text = loggedInUser;

            Profile profileForm = new Profile(LoggedInId); // ✔️ correct
            switchPanel(profileForm);
        }


        private void panelmain_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnGrade12_Click(object sender, EventArgs e)
        {
            // ✅ Change text ng labels when profile is clicked
            label1.Text = "Grade 12";
            label2.Text = loggedInUser;

            Reports g12 = new Reports();
            switchPanel(g12);

        }

        private void button2_Click(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label1.Text = "Attendance";
            label2.Text = loggedInUser;

            Attendance attend = new Attendance(loggedInUser);
            switchPanel(attend);
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            label1.Text = "Dashboard";
            label2.Text = loggedInUser;

            Dashboard dash = new Dashboard(teacherStrand, teacherSection, teacherYearLevel);
            switchPanel(dash);
        }
        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            // Show instructions first
            string instructions = "Welcome to the Grades Component!\n\n" +
                                  "Here's how to use it:\n" +
                                  "1. Click the 'Import' button to load an Excel file with your students' grades.\n" +
                                  "2. Ensure the first row of your Excel file contains the subject name, and the second row contains headers.\n" +
                                  "3. After importing, review the grades in the table.\n" +
                                  "4. Click 'Add' to save the grades to the database.\n" +
                                  "5. Use the term and semester dropdowns to filter grades if needed.\n\n" +
                                  "Make sure your data is correct before saving!";

            MessageBox.Show(instructions, "Grade Component Instructions", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Set labels
            label1.Text = "Grades Component";
            label2.Text = loggedInUser;

            // Switch panel to the GradeComponent
            GradeComponent grades = new GradeComponent(LoggedInId);
            switchPanel(grades);
        }

        private void pictureBox2_Click_1(object sender, EventArgs e)
        {
            label1.Text = "Profile";
            label2.Text = LoggedInUser;

            Profile profileForm = new Profile(LoggedInId);

            // subscribe
            profileForm.OnProfileUpdated += (newFirstName) =>
            {
                LoggedInUser = newFirstName; // update stored value
                label2.Text = newFirstName;  // update label
            };

            switchPanel(profileForm);
        }
        private void guna2Button4_Click(object sender, EventArgs e)
        {
            label1.Text = "Subject Students";
            label2.Text = loggedInUser;

            ViewStudents students = new ViewStudents(LoggedInId, teacherYearLevel, teacherStrand, teacherSection);

            switchPanel(students);
        }


        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void MainPage_Load(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;
            label1.Text = "Dashboard";
            label2.Text = loggedInUser;

            Dashboard dash = new Dashboard(teacherStrand, teacherSection, teacherYearLevel);
            switchPanel(dash);
        }


        private void guna2Button5_Click(object sender, EventArgs e)
        {
            label1.Text = "Behavior Logs";
            label2.Text = loggedInUser;

            BehaviorLog dash = new BehaviorLog(teacherStrand, teacherSection, teacherYearLevel);
            switchPanel(dash);
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to logout?",
                "Confirm Logout",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                // Show the original login form (Form1)
                Form1 login = new Form1();
                login.Show();
                ResponsiveLayout.Attach(login);

                // Close the current MainPage
                this.Close();
            }
        }


        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2Button7_Click(object sender, EventArgs e)
        {
            //label1.Text = "Subject Class";
            //label2.Text = loggedInUser;

            //SubjectClass dash = new SubjectClass(LoggedInId);
            //switchPanel(dash);
        }

        private void guna2Button8_Click(object sender, EventArgs e)
        {

            label1.Text = "ML Analytics";
            label2.Text = loggedInUser;

            MLAnalytics analytics = new MLAnalytics();
            switchPanel(analytics);
        }

        private void guna2Button9_Click(object sender, EventArgs e)
        {
            ImportStud ip = new ImportStud();
            ip.Show();
        }

        private void guna2Button7_Click_1(object sender, EventArgs e)
        {
            label1.Text = "Advised Student";
            label2.Text = loggedInUser;

            AllSubjects sub  = new AllSubjects(LoggedInId, teacherYearLevel, teacherStrand, teacherSection);

            switchPanel(sub);
        }
    }
}
