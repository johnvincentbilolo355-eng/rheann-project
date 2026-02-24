namespace An_Early_Warning_System_for_Student
{
    partial class Changepass
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Changepass));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            submit = new Guna.UI2.WinForms.Guna2Button();
            currentpass = new Guna.UI2.WinForms.Guna2TextBox();
            pictureBox1 = new PictureBox();
            label3 = new Label();
            label1 = new Label();
            newpass = new Guna.UI2.WinForms.Guna2TextBox();
            confirmnewpass = new Guna.UI2.WinForms.Guna2TextBox();
            label2 = new Label();
            label4 = new Label();
            eyeclose = new PictureBox();
            eyeopen = new PictureBox();
            eyeopen1 = new PictureBox();
            eyeopen2 = new PictureBox();
            eyeclose1 = new PictureBox();
            eyeclose2 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)eyeclose).BeginInit();
            ((System.ComponentModel.ISupportInitialize)eyeopen).BeginInit();
            ((System.ComponentModel.ISupportInitialize)eyeopen1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)eyeopen2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)eyeclose1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)eyeclose2).BeginInit();
            SuspendLayout();
            // 
            // submit
            // 
            submit.BorderRadius = 10;
            submit.CustomizableEdges = customizableEdges1;
            submit.DisabledState.BorderColor = Color.DarkGray;
            submit.DisabledState.CustomBorderColor = Color.DarkGray;
            submit.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            submit.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            submit.FillColor = Color.FromArgb(101, 28, 28);
            submit.Font = new Font("Century Gothic", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            submit.ForeColor = Color.White;
            submit.Location = new Point(145, 315);
            submit.Name = "submit";
            submit.ShadowDecoration.CustomizableEdges = customizableEdges2;
            submit.Size = new Size(291, 36);
            submit.TabIndex = 19;
            submit.Text = "CONFIRM";
            submit.Click += submit_Click;
            // 
            // currentpass
            // 
            currentpass.BorderColor = Color.DarkGray;
            currentpass.BorderRadius = 10;
            currentpass.CustomizableEdges = customizableEdges3;
            currentpass.DefaultText = "";
            currentpass.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            currentpass.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            currentpass.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            currentpass.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            currentpass.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            currentpass.Font = new Font("Century Gothic", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            currentpass.ForeColor = Color.Black;
            currentpass.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            currentpass.Location = new Point(145, 104);
            currentpass.Name = "currentpass";
            currentpass.PlaceholderForeColor = Color.Black;
            currentpass.PlaceholderText = "";
            currentpass.SelectedText = "";
            currentpass.ShadowDecoration.CustomizableEdges = customizableEdges4;
            currentpass.Size = new Size(291, 36);
            currentpass.TabIndex = 18;
            currentpass.TextChanged += currentpass_TextChanged;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(546, 7);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(29, 26);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 17;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Century Gothic", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.ForeColor = Color.Black;
            label3.Location = new Point(12, 9);
            label3.Name = "label3";
            label3.Size = new Size(177, 22);
            label3.TabIndex = 16;
            label3.Text = "Change Password";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Century Gothic", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.DimGray;
            label1.Location = new Point(145, 84);
            label1.Name = "label1";
            label1.Size = new Size(144, 17);
            label1.TabIndex = 15;
            label1.Text = "CURRENT PASSWORD:";
            // 
            // newpass
            // 
            newpass.BorderColor = Color.DarkGray;
            newpass.BorderRadius = 10;
            newpass.CustomizableEdges = customizableEdges5;
            newpass.DefaultText = "";
            newpass.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            newpass.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            newpass.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            newpass.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            newpass.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            newpass.Font = new Font("Century Gothic", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            newpass.ForeColor = Color.Black;
            newpass.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            newpass.Location = new Point(145, 175);
            newpass.Name = "newpass";
            newpass.PlaceholderForeColor = Color.Black;
            newpass.PlaceholderText = "";
            newpass.SelectedText = "";
            newpass.ShadowDecoration.CustomizableEdges = customizableEdges6;
            newpass.Size = new Size(291, 36);
            newpass.TabIndex = 20;
            newpass.TextChanged += newpass_TextChanged;
            // 
            // confirmnewpass
            // 
            confirmnewpass.BorderColor = Color.DarkGray;
            confirmnewpass.BorderRadius = 10;
            confirmnewpass.CustomizableEdges = customizableEdges7;
            confirmnewpass.DefaultText = "";
            confirmnewpass.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            confirmnewpass.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            confirmnewpass.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            confirmnewpass.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            confirmnewpass.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            confirmnewpass.Font = new Font("Century Gothic", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            confirmnewpass.ForeColor = Color.Black;
            confirmnewpass.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            confirmnewpass.Location = new Point(145, 250);
            confirmnewpass.Name = "confirmnewpass";
            confirmnewpass.PlaceholderForeColor = Color.Black;
            confirmnewpass.PlaceholderText = "";
            confirmnewpass.SelectedText = "";
            confirmnewpass.ShadowDecoration.CustomizableEdges = customizableEdges8;
            confirmnewpass.Size = new Size(291, 36);
            confirmnewpass.TabIndex = 21;
            confirmnewpass.TextChanged += confirmnewpass_TextChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Century Gothic", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.DimGray;
            label2.Location = new Point(145, 155);
            label2.Name = "label2";
            label2.Size = new Size(117, 17);
            label2.TabIndex = 22;
            label2.Text = "NEW PASSWORD:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Century Gothic", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.ForeColor = Color.DimGray;
            label4.Location = new Point(145, 230);
            label4.Name = "label4";
            label4.Size = new Size(181, 17);
            label4.TabIndex = 23;
            label4.Text = "CONFIRM NEW PASSWORD:";
            // 
            // eyeclose
            // 
            eyeclose.Image = (Image)resources.GetObject("eyeclose.Image");
            eyeclose.Location = new Point(407, 113);
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
            eyeopen.Location = new Point(407, 113);
            eyeopen.Name = "eyeopen";
            eyeopen.Size = new Size(21, 19);
            eyeopen.SizeMode = PictureBoxSizeMode.StretchImage;
            eyeopen.TabIndex = 46;
            eyeopen.TabStop = false;
            eyeopen.Click += eyeopen_Click;
            // 
            // eyeopen1
            // 
            eyeopen1.Image = (Image)resources.GetObject("eyeopen1.Image");
            eyeopen1.Location = new Point(407, 183);
            eyeopen1.Name = "eyeopen1";
            eyeopen1.Size = new Size(21, 19);
            eyeopen1.SizeMode = PictureBoxSizeMode.StretchImage;
            eyeopen1.TabIndex = 48;
            eyeopen1.TabStop = false;
            eyeopen1.Click += eyeopen1_Click;
            // 
            // eyeopen2
            // 
            eyeopen2.Image = (Image)resources.GetObject("eyeopen2.Image");
            eyeopen2.Location = new Point(407, 258);
            eyeopen2.Name = "eyeopen2";
            eyeopen2.Size = new Size(21, 19);
            eyeopen2.SizeMode = PictureBoxSizeMode.StretchImage;
            eyeopen2.TabIndex = 49;
            eyeopen2.TabStop = false;
            eyeopen2.Click += eyeopen2_Click;
            // 
            // eyeclose1
            // 
            eyeclose1.Image = (Image)resources.GetObject("eyeclose1.Image");
            eyeclose1.Location = new Point(407, 184);
            eyeclose1.Name = "eyeclose1";
            eyeclose1.Size = new Size(20, 18);
            eyeclose1.SizeMode = PictureBoxSizeMode.StretchImage;
            eyeclose1.TabIndex = 50;
            eyeclose1.TabStop = false;
            eyeclose1.Click += eyeclose1_Click;
            // 
            // eyeclose2
            // 
            eyeclose2.Image = (Image)resources.GetObject("eyeclose2.Image");
            eyeclose2.Location = new Point(408, 259);
            eyeclose2.Name = "eyeclose2";
            eyeclose2.Size = new Size(20, 18);
            eyeclose2.SizeMode = PictureBoxSizeMode.StretchImage;
            eyeclose2.TabIndex = 51;
            eyeclose2.TabStop = false;
            eyeclose2.Click += eyeclose2_Click;
            // 
            // Changepass
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(583, 403);
            Controls.Add(eyeclose2);
            Controls.Add(eyeclose1);
            Controls.Add(eyeopen2);
            Controls.Add(eyeopen1);
            Controls.Add(eyeclose);
            Controls.Add(eyeopen);
            Controls.Add(label4);
            Controls.Add(label2);
            Controls.Add(confirmnewpass);
            Controls.Add(newpass);
            Controls.Add(submit);
            Controls.Add(currentpass);
            Controls.Add(pictureBox1);
            Controls.Add(label3);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "Changepass";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Changepass";
            Load += Changepass_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)eyeclose).EndInit();
            ((System.ComponentModel.ISupportInitialize)eyeopen).EndInit();
            ((System.ComponentModel.ISupportInitialize)eyeopen1).EndInit();
            ((System.ComponentModel.ISupportInitialize)eyeopen2).EndInit();
            ((System.ComponentModel.ISupportInitialize)eyeclose1).EndInit();
            ((System.ComponentModel.ISupportInitialize)eyeclose2).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Guna.UI2.WinForms.Guna2Button submit;
        private Guna.UI2.WinForms.Guna2TextBox currentpass;
        private PictureBox pictureBox1;
        private Label label3;
        private Label label1;
        private Guna.UI2.WinForms.Guna2TextBox newpass;
        private Guna.UI2.WinForms.Guna2TextBox confirmnewpass;
        private Label label2;
        private Label label4;
        private PictureBox eyeclose;
        private PictureBox eyeopen;
        private PictureBox eyeopen1;
        private PictureBox eyeopen2;
        private PictureBox eyeclose1;
        private PictureBox eyeclose2;
    }
}