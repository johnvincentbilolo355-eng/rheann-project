namespace An_Early_Warning_System_for_Student
{
    partial class OTPVerification
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OTPVerification));
            txtOTP = new Guna.UI2.WinForms.Guna2TextBox();
            btnVerify = new Guna.UI2.WinForms.Guna2Button();
            label19 = new Label();
            lblTimer = new Label();
            label1 = new Label();
            pictureBox1 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // txtOTP
            // 
            txtOTP.BorderColor = Color.DarkGray;
            txtOTP.BorderRadius = 10;
            txtOTP.CustomizableEdges = customizableEdges1;
            txtOTP.DefaultText = "";
            txtOTP.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtOTP.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtOTP.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtOTP.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtOTP.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtOTP.Font = new Font("Century Gothic", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtOTP.ForeColor = Color.Black;
            txtOTP.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtOTP.Location = new Point(153, 166);
            txtOTP.Name = "txtOTP";
            txtOTP.PlaceholderForeColor = Color.Black;
            txtOTP.PlaceholderText = "";
            txtOTP.SelectedText = "";
            txtOTP.ShadowDecoration.CustomizableEdges = customizableEdges2;
            txtOTP.Size = new Size(291, 36);
            txtOTP.TabIndex = 53;
            txtOTP.TextChanged += txtOTP_TextChanged;
            // 
            // btnVerify
            // 
            btnVerify.BorderRadius = 10;
            btnVerify.CustomizableEdges = customizableEdges3;
            btnVerify.DisabledState.BorderColor = Color.DarkGray;
            btnVerify.DisabledState.CustomBorderColor = Color.DarkGray;
            btnVerify.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnVerify.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnVerify.FillColor = Color.FromArgb(101, 28, 28);
            btnVerify.Font = new Font("Century Gothic", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnVerify.ForeColor = Color.White;
            btnVerify.Location = new Point(153, 219);
            btnVerify.Name = "btnVerify";
            btnVerify.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnVerify.Size = new Size(291, 36);
            btnVerify.TabIndex = 52;
            btnVerify.Text = "CONFIRM";
            btnVerify.Click += btnVerify_Click;
            // 
            // label19
            // 
            label19.AutoSize = true;
            label19.Font = new Font("Century Gothic", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label19.Location = new Point(102, 172);
            label19.Name = "label19";
            label19.Size = new Size(53, 24);
            label19.TabIndex = 54;
            label19.Text = "OTP:";
            // 
            // lblTimer
            // 
            lblTimer.AutoSize = true;
            lblTimer.Font = new Font("Century Gothic", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblTimer.Location = new Point(362, 267);
            lblTimer.Name = "lblTimer";
            lblTimer.Size = new Size(50, 21);
            lblTimer.TabIndex = 55;
            lblTimer.Text = "04:11";
            lblTimer.Click += lblTimer_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Century Gothic", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(189, 267);
            label1.Name = "label1";
            label1.Size = new Size(177, 21);
            label1.TabIndex = 56;
            label1.Text = "This code will expire in";
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(542, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(29, 26);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 57;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            // 
            // OTPVerification
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(583, 403);
            Controls.Add(pictureBox1);
            Controls.Add(label1);
            Controls.Add(lblTimer);
            Controls.Add(txtOTP);
            Controls.Add(btnVerify);
            Controls.Add(label19);
            FormBorderStyle = FormBorderStyle.None;
            Name = "OTPVerification";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "OTPVerification";
            Load += OTPVerification_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Guna.UI2.WinForms.Guna2TextBox txtOTP;
        private Guna.UI2.WinForms.Guna2Button btnVerify;
        private Label label19;
        private Label lblTimer;
        private Label label1;
        private PictureBox pictureBox1;
    }
}