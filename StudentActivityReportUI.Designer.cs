
namespace DojoStudentManagement
{
    partial class StudentActivityReportUI
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle headerStyle = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle cellStyle = new System.Windows.Forms.DataGridViewCellStyle();
            this.cbActiveOnly = new System.Windows.Forms.CheckBox();
            this.cbEligibleOnly = new System.Windows.Forms.CheckBox();
            this.dgvActivity = new System.Windows.Forms.DataGridView();
            this.colStudentName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colArt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRank = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLastSignIn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHours = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEligible = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnExportCsv = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblSummary = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvActivity)).BeginInit();
            this.SuspendLayout();
            //
            // cbActiveOnly
            //
            this.cbActiveOnly.AutoSize = true;
            this.cbActiveOnly.Checked = true;
            this.cbActiveOnly.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbActiveOnly.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbActiveOnly.Location = new System.Drawing.Point(12, 17);
            this.cbActiveOnly.Name = "cbActiveOnly";
            this.cbActiveOnly.Size = new System.Drawing.Size(174, 22);
            this.cbActiveOnly.TabIndex = 0;
            this.cbActiveOnly.Text = "Active students only";
            this.cbActiveOnly.UseVisualStyleBackColor = true;
            this.cbActiveOnly.CheckedChanged += new System.EventHandler(this.ReportCriteria_Changed);
            //
            // cbEligibleOnly
            //
            this.cbEligibleOnly.AutoSize = true;
            this.cbEligibleOnly.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbEligibleOnly.Location = new System.Drawing.Point(204, 17);
            this.cbEligibleOnly.Name = "cbEligibleOnly";
            this.cbEligibleOnly.Size = new System.Drawing.Size(232, 22);
            this.cbEligibleOnly.TabIndex = 1;
            this.cbEligibleOnly.Text = "Eligible for promotion only";
            this.cbEligibleOnly.UseVisualStyleBackColor = true;
            this.cbEligibleOnly.CheckedChanged += new System.EventHandler(this.ReportCriteria_Changed);
            //
            // dgvActivity
            //
            this.dgvActivity.AllowUserToAddRows = false;
            this.dgvActivity.AllowUserToDeleteRows = false;
            this.dgvActivity.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            headerStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvActivity.ColumnHeadersDefaultCellStyle = headerStyle;
            this.dgvActivity.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvActivity.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colStudentName,
            this.colArt,
            this.colRank,
            this.colLastSignIn,
            this.colHours,
            this.colEligible});
            cellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvActivity.DefaultCellStyle = cellStyle;
            this.dgvActivity.Location = new System.Drawing.Point(12, 51);
            this.dgvActivity.MultiSelect = false;
            this.dgvActivity.Name = "dgvActivity";
            this.dgvActivity.ReadOnly = true;
            this.dgvActivity.RowHeadersVisible = false;
            this.dgvActivity.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvActivity.Size = new System.Drawing.Size(940, 500);
            this.dgvActivity.TabIndex = 2;
            //
            // colStudentName
            //
            this.colStudentName.HeaderText = "Student Name";
            this.colStudentName.Name = "colStudentName";
            this.colStudentName.ReadOnly = true;
            this.colStudentName.Width = 240;
            //
            // colArt
            //
            this.colArt.HeaderText = "Art";
            this.colArt.Name = "colArt";
            this.colArt.ReadOnly = true;
            this.colArt.Width = 120;
            //
            // colRank
            //
            this.colRank.HeaderText = "Rank";
            this.colRank.Name = "colRank";
            this.colRank.ReadOnly = true;
            this.colRank.Width = 130;
            //
            // colLastSignIn
            //
            this.colLastSignIn.HeaderText = "Last Sign-In";
            this.colLastSignIn.Name = "colLastSignIn";
            this.colLastSignIn.ReadOnly = true;
            this.colLastSignIn.Width = 140;
            //
            // colHours
            //
            this.colHours.HeaderText = "Hours";
            this.colHours.Name = "colHours";
            this.colHours.ReadOnly = true;
            this.colHours.Width = 100;
            //
            // colEligible
            //
            this.colEligible.HeaderText = "Eligible for Promotion";
            this.colEligible.Name = "colEligible";
            this.colEligible.ReadOnly = true;
            this.colEligible.Width = 190;
            //
            // btnExportCsv
            //
            this.btnExportCsv.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExportCsv.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExportCsv.Location = new System.Drawing.Point(12, 564);
            this.btnExportCsv.Name = "btnExportCsv";
            this.btnExportCsv.Size = new System.Drawing.Size(170, 38);
            this.btnExportCsv.TabIndex = 3;
            this.btnExportCsv.Text = "Export to CSV...";
            this.btnExportCsv.UseVisualStyleBackColor = true;
            this.btnExportCsv.Click += new System.EventHandler(this.btnExportCsv_Click);
            //
            // btnClose
            //
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(857, 564);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(95, 38);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            //
            // lblSummary
            //
            this.lblSummary.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblSummary.AutoSize = true;
            this.lblSummary.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSummary.Location = new System.Drawing.Point(196, 575);
            this.lblSummary.Name = "lblSummary";
            this.lblSummary.Size = new System.Drawing.Size(0, 18);
            this.lblSummary.TabIndex = 5;
            //
            // StudentActivityReportUI
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(964, 622);
            this.Controls.Add(this.lblSummary);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnExportCsv);
            this.Controls.Add(this.dgvActivity);
            this.Controls.Add(this.cbEligibleOnly);
            this.Controls.Add(this.cbActiveOnly);
            this.MinimizeBox = false;
            this.Name = "StudentActivityReportUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Windsong Student Activity";
            this.Load += new System.EventHandler(this.StudentActivityReportUI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvActivity)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cbActiveOnly;
        private System.Windows.Forms.CheckBox cbEligibleOnly;
        private System.Windows.Forms.DataGridView dgvActivity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStudentName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colArt;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRank;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLastSignIn;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHours;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEligible;
        private System.Windows.Forms.Button btnExportCsv;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblSummary;
    }
}
