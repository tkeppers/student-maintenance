﻿
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
            this.cmbFilterByArt = new System.Windows.Forms.ComboBox();
            this.lblFilterByArt = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPromotionSettings)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvPromotionSettings
            // 
            this.dgvPromotionSettings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPromotionSettings.Location = new System.Drawing.Point(15, 62);
            this.dgvPromotionSettings.MultiSelect = false;
            this.dgvPromotionSettings.Name = "dgvPromotionSettings";
            this.dgvPromotionSettings.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPromotionSettings.Size = new System.Drawing.Size(789, 311);
            this.dgvPromotionSettings.TabIndex = 0;
            this.dgvPromotionSettings.CellValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPromotionSettings_CellValidated);
            this.dgvPromotionSettings.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dgvPromotionSettings_CellValidating);
            this.dgvPromotionSettings.SelectionChanged += new System.EventHandler(this.dgvPromotionSettings_SelectionChanged);
            // 
            // cmbFilterByArt
            // 
            this.cmbFilterByArt.FormattingEnabled = true;
            this.cmbFilterByArt.Location = new System.Drawing.Point(304, 31);
            this.cmbFilterByArt.Name = "cmbFilterByArt";
            this.cmbFilterByArt.Size = new System.Drawing.Size(174, 21);
            this.cmbFilterByArt.TabIndex = 1;
            this.cmbFilterByArt.SelectedIndexChanged += new System.EventHandler(this.cmbFilterByArt_SelectedIndexChanged);
            // 
            // lblFilterByArt
            // 
            this.lblFilterByArt.AutoSize = true;
            this.lblFilterByArt.Location = new System.Drawing.Point(304, 15);
            this.lblFilterByArt.Name = "lblFilterByArt";
            this.lblFilterByArt.Size = new System.Drawing.Size(59, 13);
            this.lblFilterByArt.TabIndex = 2;
            this.lblFilterByArt.Text = "Filter by Art";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(343, 378);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // PromotionCriteriaUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(827, 429);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.lblFilterByArt);
            this.Controls.Add(this.cmbFilterByArt);
            this.Controls.Add(this.dgvPromotionSettings);
            this.Name = "PromotionCriteriaUI";
            this.ShowIcon = false;
            this.Text = "Promotion Settings";
            this.Load += new System.EventHandler(this.PromotionSettingsUI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPromotionSettings)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvPromotionSettings;
        private System.Windows.Forms.ComboBox cmbFilterByArt;
        private System.Windows.Forms.Label lblFilterByArt;
        private System.Windows.Forms.Button btnSave;
    }
}