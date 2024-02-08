namespace DojoStudentManagement
{
    partial class StudentSignIn
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StudentSignIn));
            this.listboxSelectArt = new System.Windows.Forms.ListBox();
            this.gbQuickFilters = new System.Windows.Forms.GroupBox();
            this.lblLastNameFilter = new System.Windows.Forms.Label();
            this.lblFirstNameFilter = new System.Windows.Forms.Label();
            this.txtLastNameFilter = new System.Windows.Forms.TextBox();
            this.txtFirstNameFilter = new System.Windows.Forms.TextBox();
            this.dgvStudentList = new System.Windows.Forms.DataGridView();
            this.listboxSignInList = new System.Windows.Forms.ListBox();
            this.gbQuickFilters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStudentList)).BeginInit();
            this.SuspendLayout();
            // 
            // listboxSelectArt
            // 
            this.listboxSelectArt.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listboxSelectArt.FormattingEnabled = true;
            this.listboxSelectArt.ItemHeight = 24;
            this.listboxSelectArt.Location = new System.Drawing.Point(32, 35);
            this.listboxSelectArt.Name = "listboxSelectArt";
            this.listboxSelectArt.Size = new System.Drawing.Size(152, 124);
            this.listboxSelectArt.TabIndex = 0;
            // 
            // gbQuickFilters
            // 
            this.gbQuickFilters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbQuickFilters.Controls.Add(this.lblLastNameFilter);
            this.gbQuickFilters.Controls.Add(this.lblFirstNameFilter);
            this.gbQuickFilters.Controls.Add(this.txtLastNameFilter);
            this.gbQuickFilters.Controls.Add(this.txtFirstNameFilter);
            this.gbQuickFilters.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbQuickFilters.Location = new System.Drawing.Point(203, 31);
            this.gbQuickFilters.Name = "gbQuickFilters";
            this.gbQuickFilters.Size = new System.Drawing.Size(466, 71);
            this.gbQuickFilters.TabIndex = 10;
            this.gbQuickFilters.TabStop = false;
            this.gbQuickFilters.Text = "Filters";
            // 
            // lblLastNameFilter
            // 
            this.lblLastNameFilter.AutoSize = true;
            this.lblLastNameFilter.Location = new System.Drawing.Point(201, 16);
            this.lblLastNameFilter.Name = "lblLastNameFilter";
            this.lblLastNameFilter.Size = new System.Drawing.Size(76, 17);
            this.lblLastNameFilter.TabIndex = 5;
            this.lblLastNameFilter.Text = "Last Name";
            // 
            // lblFirstNameFilter
            // 
            this.lblFirstNameFilter.AutoSize = true;
            this.lblFirstNameFilter.Location = new System.Drawing.Point(6, 16);
            this.lblFirstNameFilter.Name = "lblFirstNameFilter";
            this.lblFirstNameFilter.Size = new System.Drawing.Size(76, 17);
            this.lblFirstNameFilter.TabIndex = 4;
            this.lblFirstNameFilter.Text = "First Name";
            // 
            // txtLastNameFilter
            // 
            this.txtLastNameFilter.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLastNameFilter.Location = new System.Drawing.Point(204, 36);
            this.txtLastNameFilter.Name = "txtLastNameFilter";
            this.txtLastNameFilter.Size = new System.Drawing.Size(225, 26);
            this.txtLastNameFilter.TabIndex = 3;
            this.txtLastNameFilter.TextChanged += new System.EventHandler(this.txtLastNameFilter_TextChanged);
            // 
            // txtFirstNameFilter
            // 
            this.txtFirstNameFilter.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFirstNameFilter.Location = new System.Drawing.Point(6, 36);
            this.txtFirstNameFilter.Name = "txtFirstNameFilter";
            this.txtFirstNameFilter.Size = new System.Drawing.Size(182, 26);
            this.txtFirstNameFilter.TabIndex = 2;
            this.txtFirstNameFilter.TextChanged += new System.EventHandler(this.txtFirstNameFilter_TextChanged);
            // 
            // dgvStudentList
            // 
            this.dgvStudentList.AllowUserToAddRows = false;
            this.dgvStudentList.AllowUserToDeleteRows = false;
            this.dgvStudentList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvStudentList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStudentList.Location = new System.Drawing.Point(203, 108);
            this.dgvStudentList.MultiSelect = false;
            this.dgvStudentList.Name = "dgvStudentList";
            this.dgvStudentList.ReadOnly = true;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvStudentList.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvStudentList.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvStudentList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvStudentList.Size = new System.Drawing.Size(466, 292);
            this.dgvStudentList.TabIndex = 9;
            this.dgvStudentList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStudentList_CellContentClick);
            // 
            // listboxSignInList
            // 
            this.listboxSignInList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listboxSignInList.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listboxSignInList.FormattingEnabled = true;
            this.listboxSignInList.ItemHeight = 20;
            this.listboxSignInList.Location = new System.Drawing.Point(32, 425);
            this.listboxSignInList.MinimumSize = new System.Drawing.Size(630, 140);
            this.listboxSignInList.Name = "listboxSignInList";
            this.listboxSignInList.Size = new System.Drawing.Size(637, 144);
            this.listboxSignInList.TabIndex = 11;
            this.listboxSignInList.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listboxSignInList_DrawItem);
            // 
            // StudentSignIn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(720, 626);
            this.Controls.Add(this.listboxSignInList);
            this.Controls.Add(this.gbQuickFilters);
            this.Controls.Add(this.dgvStudentList);
            this.Controls.Add(this.listboxSelectArt);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "StudentSignIn";
            this.Text = "Windsong Student Sign In";
            this.Load += new System.EventHandler(this.StudentSignIn_Load);
            this.gbQuickFilters.ResumeLayout(false);
            this.gbQuickFilters.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStudentList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listboxSelectArt;
        private System.Windows.Forms.GroupBox gbQuickFilters;
        private System.Windows.Forms.Label lblLastNameFilter;
        private System.Windows.Forms.Label lblFirstNameFilter;
        private System.Windows.Forms.TextBox txtLastNameFilter;
        private System.Windows.Forms.TextBox txtFirstNameFilter;
        private System.Windows.Forms.DataGridView dgvStudentList;
        private System.Windows.Forms.ListBox listboxSignInList;
    }
}