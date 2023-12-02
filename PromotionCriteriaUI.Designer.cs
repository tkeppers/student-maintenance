
namespace DojoStudentManagement
{
    partial class PromotionCriteriaUI
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
            this.dgvPromotionSettings = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPromotionSettings)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvPromotionSettings
            // 
            this.dgvPromotionSettings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPromotionSettings.Location = new System.Drawing.Point(12, 41);
            this.dgvPromotionSettings.Name = "dgvPromotionSettings";
            this.dgvPromotionSettings.Size = new System.Drawing.Size(588, 350);
            this.dgvPromotionSettings.TabIndex = 0;
            // 
            // PromotionCriteriaUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(633, 476);
            this.Controls.Add(this.dgvPromotionSettings);
            this.Name = "PromotionCriteriaUI";
            this.ShowIcon = false;
            this.Text = "Promotion Settings";
            this.Load += new System.EventHandler(this.PromotionSettingsUI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPromotionSettings)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvPromotionSettings;
    }
}