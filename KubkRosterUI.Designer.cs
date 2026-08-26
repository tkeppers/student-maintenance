
namespace DojoStudentManagement
{
    partial class KubkRosterUI
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
            this.lblDojoPrompt = new System.Windows.Forms.Label();
            this.cmbDojo = new System.Windows.Forms.ComboBox();
            this.lblYearPrompt = new System.Windows.Forms.Label();
            this.numDuesYear = new System.Windows.Forms.NumericUpDown();
            this.cbActiveOnly = new System.Windows.Forms.CheckBox();
            this.cbUnpaidOnly = new System.Windows.Forms.CheckBox();
            this.dgvRoster = new System.Windows.Forms.DataGridView();
            this.colStudentName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDojo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colArt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRank = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRankVerified = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLastPromotion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colYearsAtRank = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDuesPaid = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.btnVerifyRank = new System.Windows.Forms.Button();
            this.btnCorrectRank = new System.Windows.Forms.Button();
            this.btnToggleActive = new System.Windows.Forms.Button();
            this.btnPromoteStudent = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numDuesYear)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRoster)).BeginInit();
            this.SuspendLayout();
            //
            // lblDojoPrompt
            //
            this.lblDojoPrompt.AutoSize = true;
            this.lblDojoPrompt.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDojoPrompt.Location = new System.Drawing.Point(12, 18);
            this.lblDojoPrompt.Name = "lblDojoPrompt";
            this.lblDojoPrompt.Size = new System.Drawing.Size(48, 18);
            this.lblDojoPrompt.TabIndex = 0;
            this.lblDojoPrompt.Text = "Dojo:";
            //
            // cmbDojo
            //
            this.cmbDojo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDojo.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbDojo.Location = new System.Drawing.Point(66, 15);
            this.cmbDojo.Name = "cmbDojo";
            this.cmbDojo.Size = new System.Drawing.Size(280, 26);
            this.cmbDojo.TabIndex = 1;
            this.cmbDojo.SelectedIndexChanged += new System.EventHandler(this.RosterCriteria_Changed);
            //
            // lblYearPrompt
            //
            this.lblYearPrompt.AutoSize = true;
            this.lblYearPrompt.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblYearPrompt.Location = new System.Drawing.Point(370, 18);
            this.lblYearPrompt.Name = "lblYearPrompt";
            this.lblYearPrompt.Size = new System.Drawing.Size(89, 18);
            this.lblYearPrompt.TabIndex = 2;
            this.lblYearPrompt.Text = "Dues Year:";
            //
            // numDuesYear
            //
            this.numDuesYear.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numDuesYear.Location = new System.Drawing.Point(465, 16);
            this.numDuesYear.Maximum = new decimal(new int[] { 2100, 0, 0, 0});
            this.numDuesYear.Minimum = new decimal(new int[] { 1900, 0, 0, 0});
            this.numDuesYear.Name = "numDuesYear";
            this.numDuesYear.Size = new System.Drawing.Size(100, 25);
            this.numDuesYear.TabIndex = 3;
            this.numDuesYear.Value = new decimal(new int[] { 2000, 0, 0, 0});
            this.numDuesYear.ValueChanged += new System.EventHandler(this.RosterCriteria_Changed);
            //
            // cbActiveOnly
            //
            this.cbActiveOnly.AutoSize = true;
            this.cbActiveOnly.Checked = true;
            this.cbActiveOnly.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbActiveOnly.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbActiveOnly.Location = new System.Drawing.Point(600, 17);
            this.cbActiveOnly.Name = "cbActiveOnly";
            this.cbActiveOnly.Size = new System.Drawing.Size(174, 22);
            this.cbActiveOnly.TabIndex = 4;
            this.cbActiveOnly.Text = "Active students only";
            this.cbActiveOnly.UseVisualStyleBackColor = true;
            this.cbActiveOnly.CheckedChanged += new System.EventHandler(this.RosterFilter_Changed);
            //
            // cbUnpaidOnly
            //
            this.cbUnpaidOnly.AutoSize = true;
            this.cbUnpaidOnly.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbUnpaidOnly.Location = new System.Drawing.Point(790, 17);
            this.cbUnpaidOnly.Name = "cbUnpaidOnly";
            this.cbUnpaidOnly.Size = new System.Drawing.Size(151, 22);
            this.cbUnpaidOnly.TabIndex = 5;
            this.cbUnpaidOnly.Text = "Unpaid dues only";
            this.cbUnpaidOnly.UseVisualStyleBackColor = true;
            this.cbUnpaidOnly.CheckedChanged += new System.EventHandler(this.RosterFilter_Changed);
            //
            // dgvRoster
            //
            this.dgvRoster.AllowUserToAddRows = false;
            this.dgvRoster.AllowUserToDeleteRows = false;
            this.dgvRoster.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvRoster.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvRoster.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRoster.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colStudentName,
            this.colDojo,
            this.colArt,
            this.colRank,
            this.colRankVerified,
            this.colLastPromotion,
            this.colYearsAtRank,
            this.colDuesPaid});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvRoster.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvRoster.Location = new System.Drawing.Point(12, 55);
            this.dgvRoster.MultiSelect = false;
            this.dgvRoster.Name = "dgvRoster";
            this.dgvRoster.RowHeadersVisible = false;
            this.dgvRoster.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRoster.Size = new System.Drawing.Size(1160, 520);
            this.dgvRoster.TabIndex = 6;
            this.dgvRoster.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvRoster_CellContentClick);
            this.dgvRoster.SelectionChanged += new System.EventHandler(this.dgvRoster_SelectionChanged);
            //
            // colStudentName
            //
            this.colStudentName.HeaderText = "Student Name";
            this.colStudentName.Name = "colStudentName";
            this.colStudentName.ReadOnly = true;
            this.colStudentName.Width = 230;
            //
            // colDojo
            //
            this.colDojo.HeaderText = "Dojo";
            this.colDojo.Name = "colDojo";
            this.colDojo.ReadOnly = true;
            this.colDojo.Width = 120;
            //
            // colArt
            //
            this.colArt.HeaderText = "Art";
            this.colArt.Name = "colArt";
            this.colArt.ReadOnly = true;
            this.colArt.Width = 110;
            //
            // colRank
            //
            this.colRank.HeaderText = "Current Rank";
            this.colRank.Name = "colRank";
            this.colRank.ReadOnly = true;
            this.colRank.Width = 130;
            //
            // colRankVerified
            //
            this.colRankVerified.HeaderText = "Rank Verified";
            this.colRankVerified.Name = "colRankVerified";
            this.colRankVerified.ReadOnly = true;
            this.colRankVerified.Width = 140;
            //
            // colLastPromotion
            //
            this.colLastPromotion.HeaderText = "Last Promotion";
            this.colLastPromotion.Name = "colLastPromotion";
            this.colLastPromotion.ReadOnly = true;
            this.colLastPromotion.Width = 140;
            //
            // colYearsAtRank
            //
            this.colYearsAtRank.HeaderText = "Years at Rank";
            this.colYearsAtRank.Name = "colYearsAtRank";
            this.colYearsAtRank.ReadOnly = true;
            this.colYearsAtRank.Width = 130;
            //
            // colDuesPaid
            //
            this.colDuesPaid.HeaderText = "Dues Paid";
            this.colDuesPaid.Name = "colDuesPaid";
            this.colDuesPaid.Width = 110;
            //
            // btnVerifyRank
            //
            this.btnVerifyRank.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnVerifyRank.Enabled = false;
            this.btnVerifyRank.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVerifyRank.Location = new System.Drawing.Point(12, 588);
            this.btnVerifyRank.Name = "btnVerifyRank";
            this.btnVerifyRank.Size = new System.Drawing.Size(140, 38);
            this.btnVerifyRank.TabIndex = 7;
            this.btnVerifyRank.Text = "Verify Rank";
            this.btnVerifyRank.UseVisualStyleBackColor = true;
            this.btnVerifyRank.Click += new System.EventHandler(this.btnVerifyRank_Click);
            //
            // btnPromoteStudent
            //
            this.btnCorrectRank.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCorrectRank.Enabled = false;
            this.btnCorrectRank.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCorrectRank.Location = new System.Drawing.Point(162, 588);
            this.btnCorrectRank.Name = "btnCorrectRank";
            this.btnCorrectRank.Size = new System.Drawing.Size(160, 38);
            this.btnCorrectRank.TabIndex = 8;
            this.btnCorrectRank.Text = "Correct Rank...";
            this.btnCorrectRank.UseVisualStyleBackColor = true;
            this.btnCorrectRank.Click += new System.EventHandler(this.btnCorrectRank_Click);
            //
            // btnToggleActive
            //
            this.btnToggleActive.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnToggleActive.Enabled = false;
            this.btnToggleActive.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnToggleActive.Location = new System.Drawing.Point(332, 588);
            this.btnToggleActive.Name = "btnToggleActive";
            this.btnToggleActive.Size = new System.Drawing.Size(170, 38);
            this.btnToggleActive.TabIndex = 9;
            this.btnToggleActive.Text = "Mark Inactive";
            this.btnToggleActive.UseVisualStyleBackColor = true;
            this.btnToggleActive.Click += new System.EventHandler(this.btnToggleActive_Click);
            //
            // btnPromoteStudent
            //
            this.btnPromoteStudent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPromoteStudent.Enabled = false;
            this.btnPromoteStudent.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPromoteStudent.Location = new System.Drawing.Point(512, 588);
            this.btnPromoteStudent.Name = "btnPromoteStudent";
            this.btnPromoteStudent.Size = new System.Drawing.Size(180, 38);
            this.btnPromoteStudent.TabIndex = 10;
            this.btnPromoteStudent.Text = "Promote Student...";
            this.btnPromoteStudent.UseVisualStyleBackColor = true;
            this.btnPromoteStudent.Click += new System.EventHandler(this.btnPromoteStudent_Click);
            //
            // btnClose
            //
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(1077, 588);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(95, 38);
            this.btnClose.TabIndex = 9;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            //
            // lblStatus
            //
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.Location = new System.Drawing.Point(12, 638);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 18);
            this.lblStatus.TabIndex = 10;
            //
            // KubkRosterUI
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 668);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnPromoteStudent);
            this.Controls.Add(this.btnToggleActive);
            this.Controls.Add(this.btnCorrectRank);
            this.Controls.Add(this.btnVerifyRank);
            this.Controls.Add(this.dgvRoster);
            this.Controls.Add(this.cbUnpaidOnly);
            this.Controls.Add(this.cbActiveOnly);
            this.Controls.Add(this.numDuesYear);
            this.Controls.Add(this.lblYearPrompt);
            this.Controls.Add(this.cmbDojo);
            this.Controls.Add(this.lblDojoPrompt);
            this.MinimizeBox = false;
            this.Name = "KubkRosterUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "KUBK Student Roster";
            this.Load += new System.EventHandler(this.KubkRosterUI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numDuesYear)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRoster)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblDojoPrompt;
        private System.Windows.Forms.ComboBox cmbDojo;
        private System.Windows.Forms.Label lblYearPrompt;
        private System.Windows.Forms.NumericUpDown numDuesYear;
        private System.Windows.Forms.CheckBox cbActiveOnly;
        private System.Windows.Forms.CheckBox cbUnpaidOnly;
        private System.Windows.Forms.DataGridView dgvRoster;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStudentName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDojo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colArt;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRank;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRankVerified;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLastPromotion;
        private System.Windows.Forms.DataGridViewTextBoxColumn colYearsAtRank;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colDuesPaid;
        private System.Windows.Forms.Button btnVerifyRank;
        private System.Windows.Forms.Button btnCorrectRank;
        private System.Windows.Forms.Button btnToggleActive;
        private System.Windows.Forms.Button btnPromoteStudent;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblStatus;
    }
}
