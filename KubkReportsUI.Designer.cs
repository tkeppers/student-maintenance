
namespace DojoStudentManagement
{
    partial class KubkReportsUI
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KubkReportsUI));
            this.lblDojoPrompt = new System.Windows.Forms.Label();
            this.cmbDojo = new System.Windows.Forms.ComboBox();
            this.cbIncludeWindsong = new System.Windows.Forms.CheckBox();
            this.lblYearPrompt = new System.Windows.Forms.Label();
            this.numDuesYear = new System.Windows.Forms.NumericUpDown();
            this.cbActiveOnly = new System.Windows.Forms.CheckBox();
            this.dgvReport = new System.Windows.Forms.DataGridView();
            this.btnExportCsv = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblSummary = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numDuesYear)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).BeginInit();
            this.SuspendLayout();
            // 
            // lblDojoPrompt
            // 
            this.lblDojoPrompt.AutoSize = true;
            this.lblDojoPrompt.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDojoPrompt.Location = new System.Drawing.Point(12, 18);
            this.lblDojoPrompt.Name = "lblDojoPrompt";
            this.lblDojoPrompt.Size = new System.Drawing.Size(44, 18);
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
            this.cmbDojo.SelectedIndexChanged += new System.EventHandler(this.ReportCriteria_Changed);
            // 
            // cbIncludeWindsong
            // 
            this.cbIncludeWindsong.AutoSize = true;
            this.cbIncludeWindsong.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbIncludeWindsong.Location = new System.Drawing.Point(366, 17);
            this.cbIncludeWindsong.Name = "cbIncludeWindsong";
            this.cbIncludeWindsong.Size = new System.Drawing.Size(144, 22);
            this.cbIncludeWindsong.TabIndex = 2;
            this.cbIncludeWindsong.Text = "Include Windsong";
            this.cbIncludeWindsong.UseVisualStyleBackColor = true;
            this.cbIncludeWindsong.CheckedChanged += new System.EventHandler(this.ReportCriteria_Changed);
            // 
            // lblYearPrompt
            // 
            this.lblYearPrompt.AutoSize = true;
            this.lblYearPrompt.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblYearPrompt.Location = new System.Drawing.Point(12, 18);
            this.lblYearPrompt.Name = "lblYearPrompt";
            this.lblYearPrompt.Size = new System.Drawing.Size(81, 18);
            this.lblYearPrompt.TabIndex = 3;
            this.lblYearPrompt.Text = "Dues Year:";
            // 
            // numDuesYear
            // 
            this.numDuesYear.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numDuesYear.Location = new System.Drawing.Point(107, 16);
            this.numDuesYear.Maximum = new decimal(new int[] {
            2100,
            0,
            0,
            0});
            this.numDuesYear.Minimum = new decimal(new int[] {
            1900,
            0,
            0,
            0});
            this.numDuesYear.Name = "numDuesYear";
            this.numDuesYear.Size = new System.Drawing.Size(100, 24);
            this.numDuesYear.TabIndex = 4;
            this.numDuesYear.Value = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numDuesYear.ValueChanged += new System.EventHandler(this.ReportCriteria_Changed);
            // 
            // cbActiveOnly
            // 
            this.cbActiveOnly.AutoSize = true;
            this.cbActiveOnly.Checked = true;
            this.cbActiveOnly.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbActiveOnly.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbActiveOnly.Location = new System.Drawing.Point(232, 17);
            this.cbActiveOnly.Name = "cbActiveOnly";
            this.cbActiveOnly.Size = new System.Drawing.Size(157, 22);
            this.cbActiveOnly.TabIndex = 5;
            this.cbActiveOnly.Text = "Active students only";
            this.cbActiveOnly.UseVisualStyleBackColor = true;
            this.cbActiveOnly.CheckedChanged += new System.EventHandler(this.ReportCriteria_Changed);
            // 
            // dgvReport
            // 
            this.dgvReport.AllowUserToAddRows = false;
            this.dgvReport.AllowUserToDeleteRows = false;
            this.dgvReport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvReport.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvReport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvReport.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvReport.Location = new System.Drawing.Point(12, 55);
            this.dgvReport.MultiSelect = false;
            this.dgvReport.Name = "dgvReport";
            this.dgvReport.ReadOnly = true;
            this.dgvReport.RowHeadersVisible = false;
            this.dgvReport.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvReport.Size = new System.Drawing.Size(1160, 520);
            this.dgvReport.TabIndex = 6;
            // 
            // btnExportCsv
            // 
            this.btnExportCsv.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExportCsv.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExportCsv.Location = new System.Drawing.Point(12, 588);
            this.btnExportCsv.Name = "btnExportCsv";
            this.btnExportCsv.Size = new System.Drawing.Size(170, 38);
            this.btnExportCsv.TabIndex = 7;
            this.btnExportCsv.Text = "Export to CSV...";
            this.btnExportCsv.UseVisualStyleBackColor = true;
            this.btnExportCsv.Click += new System.EventHandler(this.btnExportCsv_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(1077, 588);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(95, 38);
            this.btnClose.TabIndex = 8;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblSummary
            // 
            this.lblSummary.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblSummary.AutoSize = true;
            this.lblSummary.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSummary.Location = new System.Drawing.Point(196, 599);
            this.lblSummary.Name = "lblSummary";
            this.lblSummary.Size = new System.Drawing.Size(0, 18);
            this.lblSummary.TabIndex = 9;
            // 
            // KubkReportsUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 646);
            this.Controls.Add(this.lblSummary);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnExportCsv);
            this.Controls.Add(this.dgvReport);
            this.Controls.Add(this.cbActiveOnly);
            this.Controls.Add(this.numDuesYear);
            this.Controls.Add(this.lblYearPrompt);
            this.Controls.Add(this.cbIncludeWindsong);
            this.Controls.Add(this.cmbDojo);
            this.Controls.Add(this.lblDojoPrompt);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "KubkReportsUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "KUBK Report";
            this.Load += new System.EventHandler(this.KubkReportsUI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numDuesYear)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblDojoPrompt;
        private System.Windows.Forms.ComboBox cmbDojo;
        private System.Windows.Forms.CheckBox cbIncludeWindsong;
        private System.Windows.Forms.Label lblYearPrompt;
        private System.Windows.Forms.NumericUpDown numDuesYear;
        private System.Windows.Forms.CheckBox cbActiveOnly;
        private System.Windows.Forms.DataGridView dgvReport;
        private System.Windows.Forms.Button btnExportCsv;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblSummary;
    }
}
