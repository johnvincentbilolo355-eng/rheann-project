namespace An_Early_Warning_System_for_Student
{
    partial class Archived
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Archived));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            dataGridView1 = new DataGridView();
            recover = new DataGridViewImageColumn();
            pictureBox1 = new PictureBox();
            search = new Guna.UI2.WinForms.Guna2TextBox();
            cmbType = new Guna.UI2.WinForms.Guna2ComboBox();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.BackgroundColor = Color.White;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { recover });
            dataGridView1.Location = new Point(29, 52);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(1164, 658);
            dataGridView1.TabIndex = 49;
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
            // 
            // recover
            // 
            recover.HeaderText = "Recovery";
            recover.Image = (Image)resources.GetObject("recover.Image");
            recover.ImageLayout = DataGridViewImageCellLayout.Zoom;
            recover.Name = "recover";
            recover.SortMode = DataGridViewColumnSortMode.Automatic;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(26, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(38, 30);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 57;
            pictureBox1.TabStop = false;
            // 
            // search
            // 
            search.BorderColor = Color.Silver;
            search.BorderRadius = 10;
            search.CustomizableEdges = customizableEdges1;
            search.DefaultText = "";
            search.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            search.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            search.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            search.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            search.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            search.Font = new Font("Segoe UI", 9F);
            search.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            search.Location = new Point(70, 12);
            search.Name = "search";
            search.PlaceholderForeColor = Color.Black;
            search.PlaceholderText = "";
            search.SelectedText = "";
            search.ShadowDecoration.CustomizableEdges = customizableEdges2;
            search.Size = new Size(358, 30);
            search.TabIndex = 56;
            search.TextChanged += search_TextChanged;
            // 
            // cmbType
            // 
            cmbType.BackColor = Color.Transparent;
            cmbType.BorderRadius = 10;
            cmbType.BorderThickness = 0;
            cmbType.CustomizableEdges = customizableEdges3;
            cmbType.DrawMode = DrawMode.OwnerDrawFixed;
            cmbType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbType.FocusedColor = Color.FromArgb(94, 148, 255);
            cmbType.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            cmbType.Font = new Font("Century Gothic", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmbType.ForeColor = Color.DimGray;
            cmbType.ItemHeight = 30;
            cmbType.Location = new Point(1075, 12);
            cmbType.Name = "cmbType";
            cmbType.ShadowDecoration.CustomizableEdges = customizableEdges4;
            cmbType.Size = new Size(118, 36);
            cmbType.TabIndex = 58;
            cmbType.SelectedIndexChanged += cmbType_SelectedIndexChanged;
            // 
            // Archived
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1223, 740);
            Controls.Add(cmbType);
            Controls.Add(pictureBox1);
            Controls.Add(search);
            Controls.Add(dataGridView1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "Archived";
            Text = "Archived";
            Load += Archived_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dataGridView1;
        private PictureBox pictureBox1;
        private Guna.UI2.WinForms.Guna2TextBox search;
        private DataGridViewImageColumn recover;
        private Guna.UI2.WinForms.Guna2ComboBox cmbType;
    }
}