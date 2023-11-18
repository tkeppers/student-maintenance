
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
            this.dgvPromotionHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPromotionHistory.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Art,
            this.Rank,
            this.PromoDate,
            this.PromoHours});
            this.dgvPromotionHistory.Location = new System.Drawing.Point(30, 24);
            this.dgvPromotionHistory.Name = "dgvPromotionHistory";
            this.dgvPromotionHistory.ReadOnly = true;
            this.dgvPromotionHistory.Size = new System.Drawing.Size(702, 388);
            this.dgvPromotionHistory.TabIndex = 0;
            // 
            // Art
            // 
            this.Art.HeaderText = "Art";
            this.Art.Name = "Art";
            // 
            // Rank
            // 
            this.Rank.HeaderText = "Rank";
            this.Rank.Name = "Rank";
            // 
            // PromoDate
            // 
            this.PromoDate.HeaderText = "Promo. Date";
            this.PromoDate.Name = "PromoDate";
            // 
            // PromoHours
            // 
            this.PromoHours.HeaderText = "Promo. Hours";
            this.PromoHours.Name = "PromoHours";
            // 
            // StudentPromotionHistoryUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(802, 481);
            this.Controls.Add(this.dgvPromotionHistory);
            this.Name = "StudentPromotionHistoryUI";
            this.Text = "StudentPromotionHistoryUI";
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