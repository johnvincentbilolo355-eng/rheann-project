namespace An_Early_Warning_System_for_Student
{
    partial class profileStudImport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(profileStudImport));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            pictureBox1 = new PictureBox();
            importfile = new Guna.UI2.WinForms.Guna2Button();
            label3 = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(655, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(29, 26);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 60;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            // 
            // importfile
            // 
            importfile.BorderRadius = 5;
            importfile.CustomizableEdges = customizableEdges1;
            importfile.DisabledState.BorderColor = Color.DarkGray;
            importfile.DisabledState.CustomBorderColor = Color.DarkGray;
            importfile.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            importfile.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            importfile.FillColor = Color.FromArgb(101, 28, 28);
            importfile.Font = new Font("Century Gothic", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            importfile.ForeColor = Color.Gainsboro;
            importfile.Location = new Point(200, 144);
            importfile.Name = "importfile";
            importfile.ShadowDecoration.CustomizableEdges = customizableEdges2;
            importfile.Size = new Size(293, 33);
            importfile.TabIndex = 59;
            importfile.Text = "Import Students Profile";
            importfile.Click += importfile_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Century Gothic", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.ForeColor = Color.Black;
            label3.Location = new Point(12, 9);
            label3.Name = "label3";
            label3.Size = new Size(295, 28);
            label3.TabIndex = 58;
            label3.Text = "IMPORT STUDENT PROFILE";
            // 
            // profileStudImport
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(696, 326);
            Controls.Add(pictureBox1);
            Controls.Add(importfile);
            Controls.Add(label3);
            FormBorderStyle = FormBorderStyle.None;
            Name = "profileStudImport";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "profileStudImport";
            Load += profileStudImport_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox1;
        private Guna.UI2.WinForms.Guna2Button importfile;
        private Label label3;
    }
}