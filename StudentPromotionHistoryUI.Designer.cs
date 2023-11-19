
namespace DojoStudentManagement
{
    partial class StudentPromotionHistoryUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StudentPromotionHistoryUI));
            this.dgvPromotionHistory = new System.Windows.Forms.DataGridView();
            this.Art = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Rank = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PromoDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PromoHours = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPromotionHistory)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvPromotionHistory
            // 
            this.dgvPromotionHistory.AllowUserToAddRows = false;
            this.dgvPromotionHistory.AllowUserToDeleteRows = false;
            this.dgvPromotionHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPromotionHistory.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Art,
            this.Rank,
            this.PromoDate,
            this.PromoHours});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvPromotionHistory.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvPromotionHistory.Location = new System.Drawing.Point(21, 40);
            this.dgvPromotionHistory.MultiSelect = false;
            this.dgvPromotionHistory.Name = "dgvPromotionHistory";
            this.dgvPromotionHistory.ReadOnly = true;
            this.dgvPromotionHistory.Size = new System.Drawing.Size(702, 388);
            this.dgvPromotionHistory.TabIndex = 0;
            // 
            // Art
            // 
            this.Art.HeaderText = "Art";
            this.Art.Name = "Art";
            this.Art.ReadOnly = true;
            // 
            // Rank
            // 
            this.Rank.HeaderText = "Rank";
            this.Rank.Name = "Rank";
            this.Rank.ReadOnly = true;
            // 
            // PromoDate
            // 
            this.PromoDate.HeaderText = "Promo. Date";
            this.PromoDate.Name = "PromoDate";
            this.PromoDate.ReadOnly = true;
            // 
            // PromoHours
            // 
            this.PromoHours.HeaderText = "Promo. Hours";
            this.PromoHours.Name = "PromoHours";
            this.PromoHours.ReadOnly = true;
            // 
            // StudentPromotionHistoryUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(802, 481);
            this.Controls.Add(this.dgvPromotionHistory);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "StudentPromotionHistoryUI";
            this.Text = "Promotion History";
            this.Load += new System.EventHandler(this.StudentPromotionHistoryUI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPromotionHistory)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvPromotionHistory;
        private System.Windows.Forms.DataGridViewTextBoxColumn Art;
        private System.Windows.Forms.DataGridViewTextBoxColumn Rank;
        private System.Windows.Forms.DataGridViewTextBoxColumn PromoDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn PromoHours;
    }
}