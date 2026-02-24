namespace An_Early_Warning_System_for_Student
{
    partial class teacherAccess
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(teacherAccess));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            label1 = new Label();
            label2 = new Label();
            pictureBox1 = new PictureBox();
            pictureBox2 = new PictureBox();
            id = new Guna.UI2.WinForms.Guna2TextBox();
            password = new Guna.UI2.WinForms.Guna2TextBox();
            login = new Guna.UI2.WinForms.Guna2Button();
            eyeopen = new PictureBox();
            eyeclose = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)eyeopen).BeginInit();
            ((System.ComponentModel.ISupportInitialize)eyeclose).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Century Gothic", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(104, 152);
            label1.Name = "label1";
            label1.Size = new Size(91, 20);
            label1.TabIndex = 2;
            label1.Text = "ID Number:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Century Gothic", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(104, 219);
            label2.Name = "label2";
            label2.Size = new Size(83, 20);
            label2.TabIndex = 3;
            label2.Text = "Password:";
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(548, 6);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(29, 26);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 7;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            // 
            // pictureBox2
            // 
            pictureBox2.Image = (Image)resources.GetObject("pictureBox2.Image");
            pictureBox2.Location = new Point(86, 3);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(415, 128);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.TabIndex = 8;
            pictureBox2.TabStop = false;
            // 
            // id
            // 
            id.BackColor = Color.White;
            id.BorderColor = Color.Silver;
            id.BorderRadius = 10;
            id.CustomizableEdges = customizableEdges1;
            id.DefaultText = "";
            id.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            id.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            id.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            id.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            id.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            id.Font = new Font("Century Gothic", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            id.ForeColor = Color.Black;
            id.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            id.Location = new Point(104, 175);
            id.Name = "id";
            id.PlaceholderForeColor = Color.Black;
            id.PlaceholderText = "";
            id.SelectedText = "";
            id.ShadowDecoration.CustomizableEdges = customizableEdges2;
            id.Size = new Size(379, 41);
            id.TabIndex = 27;
            // 
            // password
            // 
            password.BackColor = Color.White;
            password.BorderColor = Color.Silver;
            password.BorderRadius = 10;
            password.CustomizableEdges = customizableEdges3;
            password.DefaultText = "";
            password.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            password.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            password.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            password.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            password.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            password.Font = new Font("Century Gothic", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            password.ForeColor = Color.Black;
            password.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            password.IconRightOffset = new Point(5, 0);
            password.Location = new Point(104, 242);
            password.Name = "password";
            password.PlaceholderForeColor = Color.Black;
            password.PlaceholderText = "";
            password.SelectedText = "";
            password.ShadowDecoration.CustomizableEdges = customizableEdges4;
            password.Size = new Size(379, 41);
            password.TabIndex = 28;
            password.TextChanged += password_TextChanged;
            // 
            // login
            // 
            login.BorderRadius = 10;
            login.CustomizableEdges = customizableEdges5;
            login.DisabledState.BorderColor = Color.DarkGray;
            login.DisabledState.CustomBorderColor = Color.DarkGray;
            login.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            login.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            login.FillColor = Color.FromArgb(101, 28, 28);
            login.Font = new Font("Century Gothic", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            login.ForeColor = Color.White;
            login.Location = new Point(104, 299);
            login.Name = "login";
            login.ShadowDecoration.CustomizableEdges = customizableEdges6;
            login.Size = new Size(374, 39);
            login.TabIndex = 43;
            login.Text = "LOGIN";
            login.Click += login_Click;
            // 
            // eyeopen
            // 
            eyeopen.Image = (Image)resources.GetObject("eyeopen.Image");
            eyeopen.Location = new Point(453, 254);
            eyeopen.Name = "eyeopen";
            eyeopen.Size = new Size(21, 19);
            eyeopen.SizeMode = PictureBoxSizeMode.StretchImage;
            eyeopen.TabIndex = 44;
            eyeopen.TabStop = false;
            eyeopen.Click += eyeopen_Click;
            // 
            // eyeclose
            // 
            eyeclose.Image = (Image)resources.GetObject("eyeclose.Image");
            eyeclose.Location = new Point(453, 255);
            eyeclose.Name = "eyeclose";
            eyeclose.Size = new Size(20, 18);
            eyeclose.SizeMode = PictureBoxSizeMode.StretchImage;
            eyeclose.TabIndex = 45;
            eyeclose.TabStop = false;
            eyeclose.Click += eyeclose_Click;
            // 
            // teacherAccess
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(583, 403);
            Controls.Add(eyeclose);
            Controls.Add(eyeopen);
            Controls.Add(login);
            Controls.Add(id);
            Controls.Add(pictureBox2);
            Controls.Add(pictureBox1);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(password);
            FormBorderStyle = FormBorderStyle.None;
            Name = "teacherAccess";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Login";
            Load += teacherAccess_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)eyeopen).EndInit();
            ((System.ComponentModel.ISupportInitialize)eyeclose).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label1;
        private Label label2;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private Guna.UI2.WinForms.Guna2TextBox id;
        private Guna.UI2.WinForms.Guna2TextBox password;
        private Guna.UI2.WinForms.Guna2Button login;
        private PictureBox eyeopen;
        private PictureBox eyeclose;
    }
}