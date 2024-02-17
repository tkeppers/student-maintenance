
namespace DojoStudentManagement
{
    partial class StudentSignInHistoryUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StudentSignInHistoryUI));
            this.dgvStudentSignInHistory = new System.Windows.Forms.DataGridView();
            this.colStudentID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStudentName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSignInArt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSignInDateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSignInHours = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStudentSignInHistory)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvStudentSignInHistory
            // 
            this.dgvStudentSignInHistory.AllowUserToAddRows = false;
            this.dgvStudentSignInHistory.AllowUserToDeleteRows = false;
            this.dgvStudentSignInHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvStudentSignInHistory.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvStudentSignInHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStudentSignInHistory.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colStudentID,
            this.colStudentName,
            this.colSignInArt,
            this.colSignInDateTime,
            this.colSignInHours});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvStudentSignInHistory.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvStudentSignInHistory.Location = new System.Drawing.Point(12, 46);
            this.dgvStudentSignInHistory.MultiSelect = false;
            this.dgvStudentSignInHistory.Name = "dgvStudentSignInHistory";
            this.dgvStudentSignInHistory.ReadOnly = true;
            this.dgvStudentSignInHistory.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvStudentSignInHistory.Size = new System.Drawing.Size(653, 296);
            this.dgvStudentSignInHistory.TabIndex = 0;
            // 
            // colStudentID
            // 
            this.colStudentID.HeaderText = "Student ID";
            this.colStudentID.Name = "colStudentID";
            this.colStudentID.ReadOnly = true;
            // 
            // colStudentName
            // 
            this.colStudentName.HeaderText = "Student Name";
            this.colStudentName.Name = "colStudentName";
            this.colStudentName.ReadOnly = true;
            // 
            // colSignInArt
            // 
            this.colSignInArt.HeaderText = "Art";
            this.colSignInArt.Name = "colSignInArt";
            this.colSignInArt.ReadOnly = true;
            // 
            // colSignInDateTime
            // 
            this.colSignInDateTime.HeaderText = "Date / Time";
            this.colSignInDateTime.Name = "colSignInDateTime";
            this.colSignInDateTime.ReadOnly = true;
            // 
            // colSignInHours
            // 
            this.colSignInHours.HeaderText = "Hours";
            this.colSignInHours.Name = "colSignInHours";
            this.colSignInHours.ReadOnly = true;
            // 
            // StudentSignInHistoryUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(674, 479);
            this.Controls.Add(this.dgvStudentSignInHistory);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "StudentSignInHistoryUI";
            this.Text = "Sign In History";
            this.Load += new System.EventHandler(this.StudentSignInHistoryUI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvStudentSignInHistory)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvStudentSignInHistory;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStudentID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStudentName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSignInArt;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSignInDateTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSignInHours;
    }
}