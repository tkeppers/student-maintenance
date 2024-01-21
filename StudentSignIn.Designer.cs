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
            this.gbQuickFilters.Controls.Add(this.lblLastNameFilter);
            this.gbQuickFilters.Controls.Add(this.lblFirstNameFilter);
            this.gbQuickFilters.Controls.Add(this.txtLastNameFilter);
            this.gbQuickFilters.Controls.Add(this.txtFirstNameFilter);
            this.gbQuickFilters.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbQuickFilters.Location = new System.Drawing.Point(203, 31);
            this.gbQuickFilters.Name = "gbQuickFilters";
            this.gbQuickFilters.Size = new System.Drawing.Size(371, 71);
            this.gbQuickFilters.TabIndex = 10;
            this.gbQuickFilters.TabStop = false;
            this.gbQuickFilters.Text = "Filters";
            // 
            // lblLastNameFilter
            // 
            this.lblLastNameFilter.AutoSize = true;
            this.lblLastNameFilter.Location = new System.Drawing.Point(167, 16);
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
            this.txtLastNameFilter.Location = new System.Drawing.Point(170, 35);
            this.txtLastNameFilter.Name = "txtLastNameFilter";
            this.txtLastNameFilter.Size = new System.Drawing.Size(181, 23);
            this.txtLastNameFilter.TabIndex = 3;
            // 
            // txtFirstNameFilter
            // 
            this.txtFirstNameFilter.Location = new System.Drawing.Point(6, 36);
            this.txtFirstNameFilter.Name = "txtFirstNameFilter";
            this.txtFirstNameFilter.Size = new System.Drawing.Size(150, 23);
            this.txtFirstNameFilter.TabIndex = 2;
            // 
            // dgvStudentList
            // 
            this.dgvStudentList.AllowUserToAddRows = false;
            this.dgvStudentList.AllowUserToDeleteRows = false;
            this.dgvStudentList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStudentList.Location = new System.Drawing.Point(203, 108);
            this.dgvStudentList.MultiSelect = false;
            this.dgvStudentList.Name = "dgvStudentList";
            this.dgvStudentList.ReadOnly = true;
            this.dgvStudentList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvStudentList.Size = new System.Drawing.Size(371, 292);
            this.dgvStudentList.TabIndex = 9;
            // 
            // listboxSignInList
            // 
            this.listboxSignInList.FormattingEnabled = true;
            this.listboxSignInList.Location = new System.Drawing.Point(32, 425);
            this.listboxSignInList.Name = "listboxSignInList";
            this.listboxSignInList.Size = new System.Drawing.Size(542, 147);
            this.listboxSignInList.TabIndex = 11;
            // 
            // StudentSignIn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1143, 610);
            this.Controls.Add(this.listboxSignInList);
            this.Controls.Add(this.gbQuickFilters);
            this.Controls.Add(this.dgvStudentList);
            this.Controls.Add(this.listboxSelectArt);
            this.Name = "StudentSignIn";
            this.Text = "StudentSignIn";
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