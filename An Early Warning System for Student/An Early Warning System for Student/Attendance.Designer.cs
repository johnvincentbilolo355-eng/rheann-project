namespace An_Early_Warning_System_for_Student
{
    partial class Attendance
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Attendance));
            dataGridView1 = new DataGridView();
            addstud = new Guna.UI2.WinForms.Guna2Button();
            submit = new Guna.UI2.WinForms.Guna2Button();
            strandcom = new Guna.UI2.WinForms.Guna2ComboBox();
            search = new Guna.UI2.WinForms.Guna2TextBox();
            pictureBox1 = new PictureBox();
            yearlevel = new Guna.UI2.WinForms.Guna2ComboBox();
            Edit = new DataGridViewImageColumn();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.BackgroundColor = Color.White;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { Edit });
            dataGridView1.Location = new Point(12, 44);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(1199, 649);
            dataGridView1.TabIndex = 0;
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
            // 
            // addstud
            // 
            addstud.BorderRadius = 10;
            addstud.CustomizableEdges = customizableEdges1;
            addstud.DisabledState.BorderColor = Color.DarkGray;
            addstud.DisabledState.CustomBorderColor = Color.DarkGray;
            addstud.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            addstud.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            addstud.FillColor = Color.FromArgb(101, 28, 28);
            addstud.Font = new Font("Century Gothic", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            addstud.ForeColor = Color.Gainsboro;
            addstud.Image = (Image)resources.GetObject("addstud.Image");
            addstud.ImageAlign = HorizontalAlignment.Left;
            addstud.ImageSize = new Size(28, 28);
            addstud.Location = new Point(1074, 5);
            addstud.Name = "addstud";
            addstud.ShadowDecoration.CustomizableEdges = customizableEdges2;
            addstud.Size = new Size(137, 33);
            addstud.TabIndex = 43;
            addstud.Text = "Add Student";
            addstud.TextAlign = HorizontalAlignment.Left;
            addstud.Click += addstud_Click;
            // 
            // submit
            // 
            submit.BorderRadius = 10;
            submit.CustomizableEdges = customizableEdges3;
            submit.DisabledState.BorderColor = Color.DarkGray;
            submit.DisabledState.CustomBorderColor = Color.DarkGray;
            submit.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            submit.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            submit.FillColor = Color.FromArgb(101, 28, 28);
            submit.Font = new Font("Century Gothic", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            submit.ForeColor = Color.Gainsboro;
            submit.Location = new Point(1093, 695);
            submit.Name = "submit";
            submit.ShadowDecoration.CustomizableEdges = customizableEdges4;
            submit.Size = new Size(118, 33);
            submit.TabIndex = 46;
            submit.Text = "Submit";
            submit.Click += submit_Click;
            // 
            // strandcom
            // 
            strandcom.BackColor = Color.Transparent;
            strandcom.BorderRadius = 10;
            strandcom.CustomizableEdges = customizableEdges5;
            strandcom.DrawMode = DrawMode.OwnerDrawFixed;
            strandcom.DropDownStyle = ComboBoxStyle.DropDownList;
            strandcom.FocusedColor = Color.FromArgb(94, 148, 255);
            strandcom.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            strandcom.Font = new Font("Segoe UI", 10F);
            strandcom.ForeColor = Color.FromArgb(68, 88, 112);
            strandcom.ItemHeight = 30;
            strandcom.Location = new Point(939, 5);
            strandcom.Name = "strandcom";
            strandcom.ShadowDecoration.CustomizableEdges = customizableEdges6;
            strandcom.Size = new Size(129, 36);
            strandcom.TabIndex = 48;
            strandcom.SelectedIndexChanged += strandcom_SelectedIndexChanged;
            // 
            // search
            // 
            search.BorderColor = Color.Silver;
            search.BorderRadius = 10;
            search.CustomizableEdges = customizableEdges7;
            search.DefaultText = "";
            search.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            search.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            search.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            search.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            search.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            search.Font = new Font("Segoe UI", 9F);
            search.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            search.Location = new Point(56, 8);
            search.Name = "search";
            search.PlaceholderForeColor = Color.Black;
            search.PlaceholderText = "";
            search.SelectedText = "";
            search.ShadowDecoration.CustomizableEdges = customizableEdges8;
            search.Size = new Size(558, 30);
            search.TabIndex = 49;
            search.TextChanged += search_TextChanged;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(12, 8);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(38, 30);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 50;
            pictureBox1.TabStop = false;
            // 
            // yearlevel
            // 
            yearlevel.BackColor = Color.Transparent;
            yearlevel.BorderRadius = 10;
            yearlevel.CustomizableEdges = customizableEdges9;
            yearlevel.DrawMode = DrawMode.OwnerDrawFixed;
            yearlevel.DropDownStyle = ComboBoxStyle.DropDownList;
            yearlevel.FocusedColor = Color.FromArgb(94, 148, 255);
            yearlevel.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            yearlevel.Font = new Font("Segoe UI", 10F);
            yearlevel.ForeColor = Color.FromArgb(68, 88, 112);
            yearlevel.ItemHeight = 30;
            yearlevel.Location = new Point(804, 5);
            yearlevel.Name = "yearlevel";
            yearlevel.ShadowDecoration.CustomizableEdges = customizableEdges10;
            yearlevel.Size = new Size(129, 36);
            yearlevel.TabIndex = 51;
            yearlevel.SelectedIndexChanged += yearlevel_SelectedIndexChanged;
            // 
            // Edit
            // 
            Edit.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            Edit.FillWeight = 60.9137F;
            Edit.HeaderText = "Edit";
            Edit.Image = (Image)resources.GetObject("Edit.Image");
            Edit.ImageLayout = DataGridViewImageCellLayout.Zoom;
            Edit.Name = "Edit";
            Edit.SortMode = DataGridViewColumnSortMode.Automatic;
            Edit.Width = 80;
            // 
            // Attendance
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1223, 740);
            Controls.Add(yearlevel);
            Controls.Add(pictureBox1);
            Controls.Add(search);
            Controls.Add(strandcom);
            Controls.Add(submit);
            Controls.Add(addstud);
            Controls.Add(dataGridView1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "Attendance";
            Text = "Attendance";
            Load += Attendance_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dataGridView1;
        private ComboBox DrpStrands;
        private ComboBox DrpSection;
        private Button button1;
        private Guna.UI2.WinForms.Guna2ComboBox strandcom;
        private Guna.UI2.WinForms.Guna2Button addstud;
        private Guna.UI2.WinForms.Guna2Button submit;
        private Guna.UI2.WinForms.Guna2TextBox search;
        private PictureBox pictureBox1;
        private Guna.UI2.WinForms.Guna2ComboBox yearlevel;
        private DataGridViewImageColumn Edit;
    }
}