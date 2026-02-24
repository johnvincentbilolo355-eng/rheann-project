namespace An_Early_Warning_System_for_Student
{
    partial class adminAccess
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(adminAccess));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            label1 = new Label();
            label3 = new Label();
            pictureBox1 = new PictureBox();
            login = new Guna.UI2.WinForms.Guna2Button();
            admin = new Guna.UI2.WinForms.Guna2TextBox();
            eyeclose = new PictureBox();
            eyeopen = new PictureBox();
            DrpPosition = new Guna.UI2.WinForms.Guna2ComboBox();
            label2 = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)eyeclose).BeginInit();
            ((System.ComponentModel.ISupportInitialize)eyeopen).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Century Gothic", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(145, 114);
            label1.Name = "label1";
            label1.Size = new Size(92, 18);
            label1.TabIndex = 8;
            label1.Text = "ID NUMBER:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Century Gothic", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.ForeColor = Color.Black;
            label3.Location = new Point(3, 7);
            label3.Name = "label3";
            label3.Size = new Size(140, 22);
            label3.TabIndex = 11;
            label3.Text = "Admin Access";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(545, 7);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(29, 26);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 12;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            // 
            // login
            // 
            login.BorderRadius = 10;
            login.CustomizableEdges = customizableEdges1;
            login.DisabledState.BorderColor = Color.DarkGray;
            login.DisabledState.CustomBorderColor = Color.DarkGray;
            login.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            login.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            login.FillColor = Color.FromArgb(101, 28, 28);
            login.Font = new Font("Century Gothic", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            login.ForeColor = Color.White;
            login.Location = new Point(145, 250);
            login.Name = "login";
            login.ShadowDecoration.CustomizableEdges = customizableEdges2;
            login.Size = new Size(291, 36);
            login.TabIndex = 14;
            login.Text = "LOGIN";
            login.Click += login_Click;
            // 
            // admin
            // 
            admin.BorderColor = Color.Gray;
            admin.BorderRadius = 10;
            admin.CustomizableEdges = customizableEdges3;
            admin.DefaultText = "";
            admin.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            admin.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            admin.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            admin.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            admin.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            admin.Font = new Font("Century Gothic", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            admin.ForeColor = Color.Black;
            admin.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            admin.Location = new Point(145, 135);
            admin.Name = "admin";
            admin.PlaceholderForeColor = Color.Gray;
            admin.PlaceholderText = "";
            admin.SelectedText = "";
            admin.ShadowDecoration.CustomizableEdges = customizableEdges4;
            admin.Size = new Size(291, 36);
            admin.TabIndex = 13;
            admin.TextChanged += admin_TextChanged;
            admin.KeyDown += admin_KeyDown;
            // 
            // eyeclose
            // 
            eyeclose.Image = (Image)resources.GetObject("eyeclose.Image");
            eyeclose.Location = new Point(409, 141);
            eyeclose.Name = "eyeclose";
            eyeclose.Size = new Size(20, 18);
            eyeclose.SizeMode = PictureBoxSizeMode.StretchImage;
            eyeclose.TabIndex = 47;
            eyeclose.TabStop = false;
            eyeclose.Click += eyeclose_Click;
            // 
            // eyeopen
            // 
            eyeopen.Image = (Image)resources.GetObject("eyeopen.Image");
            eyeopen.Location = new Point(409, 141);
            eyeopen.Name = "eyeopen";
            eyeopen.Size = new Size(21, 19);
            eyeopen.SizeMode = PictureBoxSizeMode.StretchImage;
            eyeopen.TabIndex = 46;
            eyeopen.TabStop = false;
            eyeopen.Click += eyeopen_Click;
            // 
            // DrpPosition
            // 
            DrpPosition.BackColor = Color.Transparent;
            DrpPosition.BorderColor = Color.Black;
            DrpPosition.BorderRadius = 10;
            DrpPosition.CustomizableEdges = customizableEdges5;
            DrpPosition.DrawMode = DrawMode.OwnerDrawFixed;
            DrpPosition.DropDownStyle = ComboBoxStyle.DropDownList;
            DrpPosition.FocusedColor = Color.FromArgb(94, 148, 255);
            DrpPosition.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            DrpPosition.Font = new Font("Century Gothic", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            DrpPosition.ForeColor = Color.Black;
            DrpPosition.ItemHeight = 30;
            DrpPosition.Location = new Point(145, 195);
            DrpPosition.Name = "DrpPosition";
            DrpPosition.ShadowDecoration.CustomizableEdges = customizableEdges6;
            DrpPosition.Size = new Size(291, 36);
            DrpPosition.TabIndex = 60;
            DrpPosition.SelectedIndexChanged += DrpPosition_SelectedIndexChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Century Gothic", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(145, 174);
            label2.Name = "label2";
            label2.Size = new Size(49, 18);
            label2.TabIndex = 61;
            label2.Text = "ROLE:";
            // 
            // adminAccess
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(583, 403);
            Controls.Add(label2);
            Controls.Add(DrpPosition);
            Controls.Add(eyeclose);
            Controls.Add(eyeopen);
            Controls.Add(login);
            Controls.Add(admin);
            Controls.Add(pictureBox1);
            Controls.Add(label3);
            Controls.Add(label1);
            ForeColor = Color.Gray;
            FormBorderStyle = FormBorderStyle.None;
            Name = "adminAccess";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "adminAccess";
            Load += adminAccess_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)eyeclose).EndInit();
            ((System.ComponentModel.ISupportInitialize)eyeopen).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label1;
        private Label label3;
        private PictureBox pictureBox1;
        private Guna.UI2.WinForms.Guna2Button login;
        private Guna.UI2.WinForms.Guna2TextBox admin;
        private PictureBox eyeclose;
        private PictureBox eyeopen;
        private Guna.UI2.WinForms.Guna2ComboBox DrpPosition;
        private Label label2;
    }
}