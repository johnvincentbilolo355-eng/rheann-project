namespace An_Early_Warning_System_for_Student
{
    partial class GradeComponent
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
            guna2ButtonAdd = new Guna.UI2.WinForms.Guna2Button();
            guna2Button1 = new Guna.UI2.WinForms.Guna2Button();
            dataGridView1 = new DataGridView();
            label1 = new Label();
            termcbox = new Guna.UI2.WinForms.Guna2ComboBox();
            label3 = new Label();
            semcbox = new Guna.UI2.WinForms.Guna2ComboBox();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // guna2ButtonAdd
            // 
            guna2ButtonAdd.BorderRadius = 10;
            guna2ButtonAdd.CustomizableEdges = customizableEdges1;
            guna2ButtonAdd.DisabledState.BorderColor = Color.DarkGray;
            guna2ButtonAdd.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2ButtonAdd.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2ButtonAdd.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2ButtonAdd.FillColor = Color.FromArgb(101, 28, 28);
            guna2ButtonAdd.Font = new Font("Century Gothic", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            guna2ButtonAdd.ForeColor = Color.White;
            guna2ButtonAdd.Location = new Point(25, 405);
            guna2ButtonAdd.Name = "guna2ButtonAdd";
            guna2ButtonAdd.ShadowDecoration.CustomizableEdges = customizableEdges2;
            guna2ButtonAdd.Size = new Size(129, 30);
            guna2ButtonAdd.TabIndex = 42;
            guna2ButtonAdd.Text = "Submit";
            guna2ButtonAdd.Click += guna2ButtonAdd_Click;
            // 
            // guna2Button1
            // 
            guna2Button1.BorderRadius = 10;
            guna2Button1.CustomizableEdges = customizableEdges3;
            guna2Button1.DisabledState.BorderColor = Color.DarkGray;
            guna2Button1.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button1.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button1.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button1.FillColor = Color.DarkGreen;
            guna2Button1.Font = new Font("Century Gothic", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            guna2Button1.ForeColor = Color.White;
            guna2Button1.Location = new Point(293, 73);
            guna2Button1.Name = "guna2Button1";
            guna2Button1.ShadowDecoration.CustomizableEdges = customizableEdges4;
            guna2Button1.Size = new Size(129, 30);
            guna2Button1.TabIndex = 43;
            guna2Button1.Text = "IMPORT";
            guna2Button1.Click += guna2Button1_Click_1;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(25, 109);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(397, 290);
            dataGridView1.TabIndex = 44;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Century Gothic", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(244, 20);
            label1.Name = "label1";
            label1.Size = new Size(62, 22);
            label1.TabIndex = 47;
            label1.Text = "TERM:";
            // 
            // termcbox
            // 
            termcbox.BackColor = Color.Transparent;
            termcbox.BorderRadius = 10;
            termcbox.CustomizableEdges = customizableEdges5;
            termcbox.DrawMode = DrawMode.OwnerDrawFixed;
            termcbox.DropDownStyle = ComboBoxStyle.DropDownList;
            termcbox.FocusedColor = Color.FromArgb(94, 148, 255);
            termcbox.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            termcbox.Font = new Font("Segoe UI", 10F);
            termcbox.ForeColor = Color.FromArgb(68, 88, 112);
            termcbox.ItemHeight = 30;
            termcbox.Location = new Point(307, 12);
            termcbox.Name = "termcbox";
            termcbox.ShadowDecoration.CustomizableEdges = customizableEdges6;
            termcbox.Size = new Size(152, 36);
            termcbox.TabIndex = 49;
            termcbox.SelectedIndexChanged += termcbox_SelectedIndexChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Century Gothic", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(11, 20);
            label3.Name = "label3";
            label3.Size = new Size(51, 22);
            label3.TabIndex = 50;
            label3.Text = "SEM:";
            // 
            // semcbox
            // 
            semcbox.BackColor = Color.Transparent;
            semcbox.BorderRadius = 10;
            semcbox.CustomizableEdges = customizableEdges7;
            semcbox.DrawMode = DrawMode.OwnerDrawFixed;
            semcbox.DropDownStyle = ComboBoxStyle.DropDownList;
            semcbox.FocusedColor = Color.FromArgb(94, 148, 255);
            semcbox.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            semcbox.Font = new Font("Segoe UI", 10F);
            semcbox.ForeColor = Color.FromArgb(68, 88, 112);
            semcbox.ItemHeight = 30;
            semcbox.Location = new Point(68, 12);
            semcbox.Name = "semcbox";
            semcbox.ShadowDecoration.CustomizableEdges = customizableEdges8;
            semcbox.Size = new Size(152, 36);
            semcbox.TabIndex = 51;
            semcbox.SelectedIndexChanged += semcbox_SelectedIndexChanged;
            // 
            // GradeComponent
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1205, 655);
            Controls.Add(label3);
            Controls.Add(semcbox);
            Controls.Add(label1);
            Controls.Add(dataGridView1);
            Controls.Add(guna2Button1);
            Controls.Add(guna2ButtonAdd);
            Controls.Add(termcbox);
            FormBorderStyle = FormBorderStyle.None;
            Name = "GradeComponent";
            Text = "GradeComponent";
            Load += GradeComponent_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Guna.UI2.WinForms.Guna2Button guna2ButtonAdd;
        private Guna.UI2.WinForms.Guna2Button guna2Button1;
        private DataGridView dataGridView1;
        private Label label1;
        private Guna.UI2.WinForms.Guna2ComboBox termcbox;
        private Label label3;
        private Guna.UI2.WinForms.Guna2ComboBox semcbox;
    }
}