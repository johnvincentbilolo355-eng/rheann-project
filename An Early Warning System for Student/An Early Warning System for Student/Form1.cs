namespace An_Early_Warning_System_for_Student
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ResponsiveLayout.Attach(this);
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            adminAccess adminForm = new adminAccess(this);
            adminForm.Show();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide(); // hide Form1
            teacherAccess teacherForm = new teacherAccess(this); // pass Form1
            teacherForm.Show();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;
            pictureBox1.Parent = panel1;   // make panel the parent
            pictureBox1.BackColor = Color.Transparent;  // make picturebox transparent
        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            teacherAccess teacherForm = new teacherAccess(this); // pass Form1
            teacherForm.Show();

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            adminAccess adminForm = new adminAccess(this);
            adminForm.Show();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        public void LoadToPanel(UserControl uc)
        {
            panel1.Controls.Clear();
            uc.Dock = DockStyle.Fill;
            panel1.Controls.Add(uc);
        }
        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Registration register = new Registration();
            register.Show();
        }

        private void pictureBox3_Click_1(object sender, EventArgs e)
        {

        }
    }
}
