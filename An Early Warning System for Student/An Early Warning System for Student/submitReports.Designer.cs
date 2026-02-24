namespace An_Early_Warning_System_for_Student
{
    partial class submitReports
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(submitReports));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            dataGridView1 = new DataGridView();
            view = new DataGridViewImageColumn();
            done = new DataGridViewImageColumn();
            search = new Guna.UI2.WinForms.Guna2TextBox();
            pictureBox1 = new PictureBox();
            yearlevel = new Guna.UI2.WinForms.Guna2ComboBox();
            guna2ComboBox2 = new Guna.UI2.WinForms.Guna2ComboBox();
            guna2ComboBox1 = new Guna.UI2.WinForms.Guna2ComboBox();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.BackgroundColor = Color.White;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { view, done });
            dataGridView1.Location = new Point(20, 57);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(1180, 658);
            dataGridView1.TabIndex = 0;
            dataGridView1.CellContentClick += dataGridView1_CellContentClick_1;
            // 
            // view
            // 
            view.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            view.FillWeight = 60F;
            view.HeaderText = "View";
            view.Image = (Image)resources.GetObject("view.Image");
            view.ImageLayout = DataGridViewImageCellLayout.Zoom;
            view.Name = "view";
            view.SortMode = DataGridViewColumnSortMode.Automatic;
            view.Width = 80;
            // 
            // done
            // 
            done.HeaderText = "Done";
            done.Image = (Image)resources.GetObject("done.Image");
            done.ImageLayout = DataGridViewImageCellLayout.Zoom;
            done.Name = "done";
            done.SortMode = DataGridViewColumnSortMode.Automatic;
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
            search.ForeColor = Color.Black;
            search.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            search.Location = new Point(64, 21);
            search.Name = "search";
            search.PlaceholderForeColor = Color.Black;
            search.PlaceholderText = "";
            search.SelectedText = "";
            search.ShadowDecoration.CustomizableEdges = customizableEdges2;
            search.Size = new Size(563, 30);
            search.TabIndex = 45;
            search.TextChanged += search_TextChanged;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(20, 21);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(38, 30);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 47;
            pictureBox1.TabStop = false;
            // 
            // yearlevel
            // 
            yearlevel.BackColor = Color.Transparent;
            yearlevel.BorderRadius = 10;
            yearlevel.BorderThickness = 0;
            yearlevel.CustomizableEdges = customizableEdges3;
            yearlevel.DrawMode = DrawMode.OwnerDrawFixed;
            yearlevel.DropDownStyle = ComboBoxStyle.DropDownList;
            yearlevel.FocusedColor = Color.FromArgb(94, 148, 255);
            yearlevel.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            yearlevel.Font = new Font("Century Gothic", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            yearlevel.ForeColor = Color.DimGray;
            yearlevel.ItemHeight = 30;
            yearlevel.Location = new Point(828, 12);
            yearlevel.Name = "yearlevel";
            yearlevel.ShadowDecoration.CustomizableEdges = customizableEdges4;
            yearlevel.Size = new Size(118, 36);
            yearlevel.TabIndex = 50;
            yearlevel.SelectedIndexChanged += yearlevel_SelectedIndexChanged;
            // 
            // guna2ComboBox2
            // 
            guna2ComboBox2.BackColor = Color.Transparent;
            guna2ComboBox2.BorderRadius = 10;
            guna2ComboBox2.BorderThickness = 0;
            guna2ComboBox2.CustomizableEdges = customizableEdges5;
            guna2ComboBox2.DrawMode = DrawMode.OwnerDrawFixed;
            guna2ComboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            guna2ComboBox2.FocusedColor = Color.FromArgb(94, 148, 255);
            guna2ComboBox2.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            guna2ComboBox2.Font = new Font("Century Gothic", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            guna2ComboBox2.ForeColor = Color.DimGray;
            guna2ComboBox2.ItemHeight = 30;
            guna2ComboBox2.Location = new Point(1076, 12);
            guna2ComboBox2.Name = "guna2ComboBox2";
            guna2ComboBox2.ShadowDecoration.CustomizableEdges = customizableEdges6;
            guna2ComboBox2.Size = new Size(118, 36);
            guna2ComboBox2.TabIndex = 49;
            guna2ComboBox2.SelectedIndexChanged += guna2ComboBox2_SelectedIndexChanged;
            // 
            // guna2ComboBox1
            // 
            guna2ComboBox1.BackColor = Color.Transparent;
            guna2ComboBox1.BorderRadius = 10;
            guna2ComboBox1.BorderThickness = 0;
            guna2ComboBox1.CustomizableEdges = customizableEdges7;
            guna2ComboBox1.DrawMode = DrawMode.OwnerDrawFixed;
            guna2ComboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            guna2ComboBox1.FocusedColor = Color.FromArgb(94, 148, 255);
            guna2ComboBox1.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            guna2ComboBox1.Font = new Font("Century Gothic", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            guna2ComboBox1.ForeColor = Color.DimGray;
            guna2ComboBox1.ItemHeight = 30;
            guna2ComboBox1.Location = new Point(952, 12);
            guna2ComboBox1.Name = "guna2ComboBox1";
            guna2ComboBox1.ShadowDecoration.CustomizableEdges = customizableEdges8;
            guna2ComboBox1.Size = new Size(118, 36);
            guna2ComboBox1.TabIndex = 48;
            guna2ComboBox1.SelectedIndexChanged += guna2ComboBox1_SelectedIndexChanged;
            // 
            // submitReports
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1223, 740);
            Controls.Add(yearlevel);
            Controls.Add(guna2ComboBox2);
            Controls.Add(guna2ComboBox1);
            Controls.Add(pictureBox1);
            Controls.Add(search);
            Controls.Add(dataGridView1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "submitReports";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "submitReports";
            Load += submitReports_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dataGridView1;
        private Guna.UI2.WinForms.Guna2TextBox search;
        private PictureBox pictureBox1;
        private DataGridViewImageColumn view;
        private DataGridViewImageColumn done;
        private Guna.UI2.WinForms.Guna2ComboBox yearlevel;
        private Guna.UI2.WinForms.Guna2ComboBox guna2ComboBox2;
        private Guna.UI2.WinForms.Guna2ComboBox guna2ComboBox1;
    }
}