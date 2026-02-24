namespace An_Early_Warning_System_for_Student
{
    partial class Backup
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Backup));
            cmbrecord = new Guna.UI2.WinForms.Guna2ComboBox();
            label3 = new Label();
            label1 = new Label();
            backupbox = new Guna.UI2.WinForms.Guna2Button();
            restorebox = new Guna.UI2.WinForms.Guna2Button();
            pictureBox1 = new PictureBox();
            deleterec = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)deleterec).BeginInit();
            SuspendLayout();
            // 
            // cmbrecord
            // 
            cmbrecord.BackColor = Color.Transparent;
            cmbrecord.BorderRadius = 10;
            cmbrecord.CustomizableEdges = customizableEdges1;
            cmbrecord.DrawMode = DrawMode.OwnerDrawFixed;
            cmbrecord.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbrecord.FocusedColor = Color.FromArgb(94, 148, 255);
            cmbrecord.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            cmbrecord.Font = new Font("Segoe UI", 10F);
            cmbrecord.ForeColor = Color.Black;
            cmbrecord.ItemHeight = 30;
            cmbrecord.Location = new Point(202, 107);
            cmbrecord.Name = "cmbrecord";
            cmbrecord.ShadowDecoration.CustomizableEdges = customizableEdges2;
            cmbrecord.Size = new Size(394, 36);
            cmbrecord.TabIndex = 43;
            cmbrecord.SelectedIndexChanged += cmbrecord_SelectedIndexChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Century Gothic", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(12, 9);
            label3.Name = "label3";
            label3.Size = new Size(246, 28);
            label3.TabIndex = 44;
            label3.Text = "Backup and Restore";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Century Gothic", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.FromArgb(64, 64, 64);
            label1.Location = new Point(48, 113);
            label1.Name = "label1";
            label1.Size = new Size(151, 24);
            label1.TabIndex = 45;
            label1.Text = "Record Table:";
            // 
            // backupbox
            // 
            backupbox.BorderRadius = 5;
            backupbox.CustomizableEdges = customizableEdges3;
            backupbox.DisabledState.BorderColor = Color.DarkGray;
            backupbox.DisabledState.CustomBorderColor = Color.DarkGray;
            backupbox.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            backupbox.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            backupbox.FillColor = Color.FromArgb(101, 28, 28);
            backupbox.Font = new Font("Century Gothic", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            backupbox.ForeColor = Color.Gainsboro;
            backupbox.Location = new Point(48, 233);
            backupbox.Name = "backupbox";
            backupbox.ShadowDecoration.CustomizableEdges = customizableEdges4;
            backupbox.Size = new Size(293, 33);
            backupbox.TabIndex = 47;
            backupbox.Text = "Backup";
            backupbox.Click += backupbox_Click;
            // 
            // restorebox
            // 
            restorebox.BorderRadius = 5;
            restorebox.CustomizableEdges = customizableEdges5;
            restorebox.DisabledState.BorderColor = Color.DarkGray;
            restorebox.DisabledState.CustomBorderColor = Color.DarkGray;
            restorebox.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            restorebox.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            restorebox.FillColor = Color.FromArgb(101, 28, 28);
            restorebox.Font = new Font("Century Gothic", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            restorebox.ForeColor = Color.Gainsboro;
            restorebox.Location = new Point(351, 233);
            restorebox.Name = "restorebox";
            restorebox.ShadowDecoration.CustomizableEdges = customizableEdges6;
            restorebox.Size = new Size(293, 33);
            restorebox.TabIndex = 48;
            restorebox.Text = "Restore";
            restorebox.Click += restorebox_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(655, 9);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(29, 26);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 49;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            // 
            // deleterec
            // 
            deleterec.Image = (Image)resources.GetObject("deleterec.Image");
            deleterec.Location = new Point(592, 108);
            deleterec.Name = "deleterec";
            deleterec.Size = new Size(42, 35);
            deleterec.SizeMode = PictureBoxSizeMode.StretchImage;
            deleterec.TabIndex = 51;
            deleterec.TabStop = false;
            deleterec.Click += deleterec_Click;
            // 
            // Backup
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(696, 326);
            Controls.Add(pictureBox1);
            Controls.Add(restorebox);
            Controls.Add(backupbox);
            Controls.Add(label1);
            Controls.Add(label3);
            Controls.Add(cmbrecord);
            Controls.Add(deleterec);
            FormBorderStyle = FormBorderStyle.None;
            Name = "Backup";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Backup";
            Load += Backup_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)deleterec).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Guna.UI2.WinForms.Guna2ComboBox cmbrecord;
        private Label label3;
        private Label label1;
        private Guna.UI2.WinForms.Guna2Button backupbox;
        private Guna.UI2.WinForms.Guna2Button restorebox;
        private PictureBox pictureBox1;
        private PictureBox deleterec;
    }
}