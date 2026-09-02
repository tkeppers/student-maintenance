
namespace DojoStudentManagement
{
    partial class SignInHistoryReportUI
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
            this.lblFromPrompt = new System.Windows.Forms.Label();
            this.dtFrom = new System.Windows.Forms.DateTimePicker();
            this.lblToPrompt = new System.Windows.Forms.Label();
            this.dtTo = new System.Windows.Forms.DateTimePicker();
            this.cbActiveOnly = new System.Windows.Forms.CheckBox();
            this.dgvSignIns = new System.Windows.Forms.DataGridView();
            this.colStudentName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colArt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRank = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSignInDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHours = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEligible = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnExportCsv = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblSummary = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSignIns)).BeginInit();
            this.SuspendLayout();
            //
            // lblFromPrompt
            //
            this.lblFromPrompt.AutoSize = true;
            this.lblFromPrompt.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFromPrompt.Location = new System.Drawing.Point(12, 18);
            this.lblFromPrompt.Name = "lblFromPrompt";
            this.lblFromPrompt.Size = new System.Drawing.Size(48, 18);
            this.lblFromPrompt.TabIndex = 0;
            this.lblFromPrompt.Text = "From:";
            //
            // dtFrom
            //
            this.dtFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtFrom.Location = new System.Drawing.Point(66, 15);
            this.dtFrom.Name = "dtFrom";
            this.dtFrom.Size = new System.Drawing.Size(150, 26);
            this.dtFrom.TabIndex = 1;
            this.dtFrom.ValueChanged += new System.EventHandler(this.ReportCriteria_Changed);
            //
            // lblToPrompt
            //
            this.lblToPrompt.AutoSize = true;
            this.lblToPrompt.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblToPrompt.Location = new System.Drawing.Point(232, 18);
            this.lblToPrompt.Name = "lblToPrompt";
            this.lblToPrompt.Size = new System.Drawing.Size(28, 18);
            this.lblToPrompt.TabIndex = 2;
            this.lblToPrompt.Text = "To:";
            //
            // dtTo
            //
            this.dtTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtTo.Location = new System.Drawing.Point(266, 15);
            this.dtTo.Name = "dtTo";
            this.dtTo.Size = new System.Drawing.Size(150, 26);
            this.dtTo.TabIndex = 3;
            this.dtTo.ValueChanged += new System.EventHandler(this.ReportCriteria_Changed);
            //
            // cbActiveOnly
            //
            this.cbActiveOnly.AutoSize = true;
            this.cbActiveOnly.Checked = true;
            this.cbActiveOnly.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbActiveOnly.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbActiveOnly.Location = new System.Drawing.Point(444, 17);
            this.cbActiveOnly.Name = "cbActiveOnly";
            this.cbActiveOnly.Size = new System.Drawing.Size(174, 22);
            this.cbActiveOnly.TabIndex = 4;
            this.cbActiveOnly.Text = "Active students only";
            this.cbActiveOnly.UseVisualStyleBackColor = true;
            this.cbActiveOnly.CheckedChanged += new System.EventHandler(this.ReportCriteria_Changed);
            //
            // dgvSignIns
            //
            this.dgvSignIns.AllowUserToAddRows = false;
            this.dgvSignIns.AllowUserToDeleteRows = false;
            this.dgvSignIns.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            headerStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvSignIns.ColumnHeadersDefaultCellStyle = headerStyle;
            this.dgvSignIns.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSignIns.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colStudentName,
            this.colArt,
            this.colRank,
            this.colSignInDate,
            this.colHours,
            this.colEligible});
            cellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvSignIns.DefaultCellStyle = cellStyle;
            this.dgvSignIns.Location = new System.Drawing.Point(12, 51);
            this.dgvSignIns.MultiSelect = false;
            this.dgvSignIns.Name = "dgvSignIns";
            this.dgvSignIns.ReadOnly = true;
            this.dgvSignIns.RowHeadersVisible = false;
            this.dgvSignIns.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSignIns.Size = new System.Drawing.Size(940, 500);
            this.dgvSignIns.TabIndex = 5;
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
            this.colArt.Width = 110;
            //
            // colRank
            //
            this.colRank.HeaderText = "Rank";
            this.colRank.Name = "colRank";
            this.colRank.ReadOnly = true;
            this.colRank.Width = 120;
            //
            // colSignInDate
            //
            this.colSignInDate.HeaderText = "Sign-In";
            this.colSignInDate.Name = "colSignInDate";
            this.colSignInDate.ReadOnly = true;
            this.colSignInDate.Width = 180;
            //
            // colHours
            //
            this.colHours.HeaderText = "Hours";
            this.colHours.Name = "colHours";
            this.colHours.ReadOnly = true;
            this.colHours.Width = 90;
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
            this.btnExportCsv.TabIndex = 6;
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
            this.btnClose.TabIndex = 7;
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
            this.lblSummary.TabIndex = 8;
            //
            // SignInHistoryReportUI
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(964, 622);
            this.Controls.Add(this.lblSummary);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnExportCsv);
            this.Controls.Add(this.dgvSignIns);
            this.Controls.Add(this.cbActiveOnly);
            this.Controls.Add(this.dtTo);
            this.Controls.Add(this.lblToPrompt);
            this.Controls.Add(this.dtFrom);
            this.Controls.Add(this.lblFromPrompt);
            this.MinimizeBox = false;
            this.Name = "SignInHistoryReportUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Windsong Sign-In History";
            this.Load += new System.EventHandler(this.SignInHistoryReportUI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSignIns)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblFromPrompt;
        private System.Windows.Forms.DateTimePicker dtFrom;
        private System.Windows.Forms.Label lblToPrompt;
        private System.Windows.Forms.DateTimePicker dtTo;
        private System.Windows.Forms.CheckBox cbActiveOnly;
        private System.Windows.Forms.DataGridView dgvSignIns;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStudentName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colArt;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRank;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSignInDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHours;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEligible;
        private System.Windows.Forms.Button btnExportCsv;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblSummary;
    }
}
