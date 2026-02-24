namespace An_Early_Warning_System_for_Student
{
    partial class SubjectClass
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SubjectClass));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            dataGridView1 = new DataGridView();
            action = new DataGridViewImageColumn();
            pictureBox1 = new PictureBox();
            cmbView = new Guna.UI2.WinForms.Guna2ComboBox();
            search = new Guna.UI2.WinForms.Guna2TextBox();
            guna2ComboBox1 = new Guna.UI2.WinForms.Guna2ComboBox();
            export = new Guna.UI2.WinForms.Guna2Button();
            guna2Button4 = new Guna.UI2.WinForms.Guna2Button();
            yearlevel = new Guna.UI2.WinForms.Guna2ComboBox();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.BackgroundColor = Color.White;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { action });
            dataGridView1.Location = new Point(30, 66);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(1164, 637);
            dataGridView1.TabIndex = 47;
            dataGridView1.CellContentClick += dataGridView1_CellContentClick_1;
            // 
            // action
            // 
            action.HeaderText = "Action";
            action.Image = (Image)resources.GetObject("action.Image");
            action.ImageLayout = DataGridViewImageCellLayout.Zoom;
            action.Name = "action";
            action.SortMode = DataGridViewColumnSortMode.Automatic;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(31, 15);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(38, 30);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 54;
            pictureBox1.TabStop = false;
            // 
            // cmbView
            // 
            cmbView.BackColor = Color.Transparent;
            cmbView.BorderRadius = 10;
            cmbView.BorderThickness = 0;
            cmbView.CustomizableEdges = customizableEdges1;
            cmbView.DrawMode = DrawMode.OwnerDrawFixed;
            cmbView.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbView.FocusedColor = Color.FromArgb(94, 148, 255);
            cmbView.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            cmbView.Font = new Font("Century Gothic", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmbView.ForeColor = Color.DimGray;
            cmbView.ItemHeight = 30;
            cmbView.Location = new Point(612, 15);
            cmbView.Name = "cmbView";
            cmbView.ShadowDecoration.CustomizableEdges = customizableEdges2;
            cmbView.Size = new Size(118, 36);
            cmbView.TabIndex = 53;
            cmbView.SelectedIndexChanged += cmbView_SelectedIndexChanged_1;
            // 
            // search
            // 
            search.BorderColor = Color.Silver;
            search.BorderRadius = 10;
            search.CustomizableEdges = customizableEdges3;
            search.DefaultText = "";
            search.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            search.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            search.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            search.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            search.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            search.Font = new Font("Segoe UI", 9F);
            search.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            search.Location = new Point(75, 15);
            search.Name = "search";
            search.PlaceholderForeColor = Color.Black;
            search.PlaceholderText = "";
            search.SelectedText = "";
            search.ShadowDecoration.CustomizableEdges = customizableEdges4;
            search.Size = new Size(460, 30);
            search.TabIndex = 52;
            search.TextChanged += search_TextChanged_1;
            // 
            // guna2ComboBox1
            // 
            guna2ComboBox1.BackColor = Color.Transparent;
            guna2ComboBox1.BorderRadius = 10;
            guna2ComboBox1.BorderThickness = 0;
            guna2ComboBox1.CustomizableEdges = customizableEdges5;
            guna2ComboBox1.DrawMode = DrawMode.OwnerDrawFixed;
            guna2ComboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            guna2ComboBox1.FocusedColor = Color.FromArgb(94, 148, 255);
            guna2ComboBox1.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            guna2ComboBox1.Font = new Font("Century Gothic", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            guna2ComboBox1.ForeColor = Color.DimGray;
            guna2ComboBox1.ItemHeight = 30;
            guna2ComboBox1.Location = new Point(860, 15);
            guna2ComboBox1.Name = "guna2ComboBox1";
            guna2ComboBox1.ShadowDecoration.CustomizableEdges = customizableEdges6;
            guna2ComboBox1.Size = new Size(118, 36);
            guna2ComboBox1.TabIndex = 50;
            guna2ComboBox1.SelectedIndexChanged += guna2ComboBox1_SelectedIndexChanged_1;
            // 
            // export
            // 
            export.BorderRadius = 10;
            export.CustomizableEdges = customizableEdges7;
            export.DisabledState.BorderColor = Color.DarkGray;
            export.DisabledState.CustomBorderColor = Color.DarkGray;
            export.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            export.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            export.FillColor = Color.DarkGreen;
            export.Font = new Font("Century Gothic", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            export.ForeColor = Color.White;
            export.Image = (Image)resources.GetObject("export.Image");
            export.Location = new Point(1100, 15);
            export.Name = "export";
            export.ShadowDecoration.CustomizableEdges = customizableEdges8;
            export.Size = new Size(98, 30);
            export.TabIndex = 49;
            export.Text = "Export";
            export.Click += export_Click_1;
            // 
            // guna2Button4
            // 
            guna2Button4.BorderRadius = 10;
            guna2Button4.CustomizableEdges = customizableEdges9;
            guna2Button4.DisabledState.BorderColor = Color.DarkGray;
            guna2Button4.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button4.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button4.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button4.FillColor = Color.FromArgb(101, 28, 28);
            guna2Button4.Font = new Font("Century Gothic", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            guna2Button4.ForeColor = Color.White;
            guna2Button4.Image = (Image)resources.GetObject("guna2Button4.Image");
            guna2Button4.Location = new Point(984, 15);
            guna2Button4.Name = "guna2Button4";
            guna2Button4.ShadowDecoration.CustomizableEdges = customizableEdges10;
            guna2Button4.Size = new Size(111, 30);
            guna2Button4.TabIndex = 48;
            guna2Button4.Text = "Reports";
            guna2Button4.Click += guna2Button4_Click_1;
            // 
            // yearlevel
            // 
            yearlevel.BackColor = Color.Transparent;
            yearlevel.BorderRadius = 10;
            yearlevel.BorderThickness = 0;
            yearlevel.CustomizableEdges = customizableEdges11;
            yearlevel.DrawMode = DrawMode.OwnerDrawFixed;
            yearlevel.DropDownStyle = ComboBoxStyle.DropDownList;
            yearlevel.FocusedColor = Color.FromArgb(94, 148, 255);
            yearlevel.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            yearlevel.Font = new Font("Century Gothic", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            yearlevel.ForeColor = Color.DimGray;
            yearlevel.ItemHeight = 30;
            yearlevel.Location = new Point(736, 15);
            yearlevel.Name = "yearlevel";
            yearlevel.ShadowDecoration.CustomizableEdges = customizableEdges12;
            yearlevel.Size = new Size(118, 36);
            yearlevel.TabIndex = 55;
            yearlevel.SelectedIndexChanged += yearlevel_SelectedIndexChanged;
            // 
            // SubjectClass
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1223, 728);
            Controls.Add(yearlevel);
            Controls.Add(dataGridView1);
            Controls.Add(pictureBox1);
            Controls.Add(cmbView);
            Controls.Add(search);
            Controls.Add(guna2ComboBox1);
            Controls.Add(export);
            Controls.Add(guna2Button4);
            FormBorderStyle = FormBorderStyle.None;
            Name = "SubjectClass";
            Text = "SubjectClass";
            Load += SubjectClass_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dataGridView1;
        private DataGridViewImageColumn action;
        private PictureBox pictureBox1;
        private Guna.UI2.WinForms.Guna2ComboBox cmbView;
        private Guna.UI2.WinForms.Guna2TextBox search;
        private Guna.UI2.WinForms.Guna2ComboBox guna2ComboBox1;
        private Guna.UI2.WinForms.Guna2Button export;
        private Guna.UI2.WinForms.Guna2Button guna2Button4;
        private Guna.UI2.WinForms.Guna2ComboBox yearlevel;
    }
}