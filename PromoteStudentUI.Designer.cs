
namespace DojoStudentManagement
{
    partial class PromoteStudentUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PromoteStudentUI));
            this.lblStudent = new System.Windows.Forms.Label();
            this.txtCurrentRank = new System.Windows.Forms.TextBox();
            this.lblCurrentRank = new System.Windows.Forms.Label();
            this.lblNextRank = new System.Windows.Forms.Label();
            this.cmbNextRank = new System.Windows.Forms.ComboBox();
            this.dtPromotionDate = new System.Windows.Forms.DateTimePicker();
            this.lblPromotionDate = new System.Windows.Forms.Label();
            this.lblArt = new System.Windows.Forms.Label();
            this.txtStudentName = new System.Windows.Forms.TextBox();
            this.btnPromoteStudent = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtArt = new System.Windows.Forms.TextBox();
            this.chkEligibleForPromotion = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lblStudent
            // 
            this.lblStudent.AutoSize = true;
            this.lblStudent.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStudent.Location = new System.Drawing.Point(18, 27);
            this.lblStudent.Name = "lblStudent";
            this.lblStudent.Size = new System.Drawing.Size(53, 16);
            this.lblStudent.TabIndex = 1;
            this.lblStudent.Text = "Student";
            // 
            // txtCurrentRank
            // 
            this.txtCurrentRank.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCurrentRank.Location = new System.Drawing.Point(21, 110);
            this.txtCurrentRank.Name = "txtCurrentRank";
            this.txtCurrentRank.ReadOnly = true;
            this.txtCurrentRank.Size = new System.Drawing.Size(157, 22);
            this.txtCurrentRank.TabIndex = 2;
            // 
            // lblCurrentRank
            // 
            this.lblCurrentRank.AutoSize = true;
            this.lblCurrentRank.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentRank.Location = new System.Drawing.Point(18, 91);
            this.lblCurrentRank.Name = "lblCurrentRank";
            this.lblCurrentRank.Size = new System.Drawing.Size(85, 16);
            this.lblCurrentRank.TabIndex = 3;
            this.lblCurrentRank.Text = "Current Rank";
            // 
            // lblNextRank
            // 
            this.lblNextRank.AutoSize = true;
            this.lblNextRank.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNextRank.Location = new System.Drawing.Point(192, 91);
            this.lblNextRank.Name = "lblNextRank";
            this.lblNextRank.Size = new System.Drawing.Size(70, 16);
            this.lblNextRank.TabIndex = 4;
            this.lblNextRank.Text = "Next Rank";
            // 
            // cmbNextRank
            // 
            this.cmbNextRank.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbNextRank.FormattingEnabled = true;
            this.cmbNextRank.Location = new System.Drawing.Point(195, 110);
            this.cmbNextRank.Name = "cmbNextRank";
            this.cmbNextRank.Size = new System.Drawing.Size(131, 24);
            this.cmbNextRank.TabIndex = 5;
            // 
            // dtPromotionDate
            // 
            this.dtPromotionDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtPromotionDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtPromotionDate.Location = new System.Drawing.Point(21, 177);
            this.dtPromotionDate.Name = "dtPromotionDate";
            this.dtPromotionDate.Size = new System.Drawing.Size(109, 22);
            this.dtPromotionDate.TabIndex = 6;
            // 
            // lblPromotionDate
            // 
            this.lblPromotionDate.AutoSize = true;
            this.lblPromotionDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPromotionDate.Location = new System.Drawing.Point(18, 158);
            this.lblPromotionDate.Name = "lblPromotionDate";
            this.lblPromotionDate.Size = new System.Drawing.Size(101, 16);
            this.lblPromotionDate.TabIndex = 7;
            this.lblPromotionDate.Text = "Promotion Date";
            // 
            // lblArt
            // 
            this.lblArt.AutoSize = true;
            this.lblArt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblArt.Location = new System.Drawing.Point(192, 27);
            this.lblArt.Name = "lblArt";
            this.lblArt.Size = new System.Drawing.Size(24, 16);
            this.lblArt.TabIndex = 9;
            this.lblArt.Text = "Art";
            // 
            // txtStudentName
            // 
            this.txtStudentName.Location = new System.Drawing.Point(21, 46);
            this.txtStudentName.Name = "txtStudentName";
            this.txtStudentName.ReadOnly = true;
            this.txtStudentName.Size = new System.Drawing.Size(157, 20);
            this.txtStudentName.TabIndex = 10;
            // 
            // btnPromoteStudent
            // 
            this.btnPromoteStudent.Image = ((System.Drawing.Image)(resources.GetObject("btnPromoteStudent.Image")));
            this.btnPromoteStudent.Location = new System.Drawing.Point(21, 238);
            this.btnPromoteStudent.Name = "btnPromoteStudent";
            this.btnPromoteStudent.Size = new System.Drawing.Size(142, 45);
            this.btnPromoteStudent.TabIndex = 12;
            this.btnPromoteStudent.Text = "Confirm Promotion";
            this.btnPromoteStudent.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnPromoteStudent.UseVisualStyleBackColor = true;
            this.btnPromoteStudent.Click += new System.EventHandler(this.btnPromoteStudent_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.Location = new System.Drawing.Point(171, 238);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(85, 45);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // txtArt
            // 
            this.txtArt.Location = new System.Drawing.Point(195, 46);
            this.txtArt.Name = "txtArt";
            this.txtArt.ReadOnly = true;
            this.txtArt.Size = new System.Drawing.Size(131, 20);
            this.txtArt.TabIndex = 14;
            // 
            // chkEligibleForPromotion
            // 
            this.chkEligibleForPromotion.AutoSize = true;
            this.chkEligibleForPromotion.Location = new System.Drawing.Point(171, 182);
            this.chkEligibleForPromotion.Name = "chkEligibleForPromotion";
            this.chkEligibleForPromotion.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.chkEligibleForPromotion.Size = new System.Drawing.Size(124, 17);
            this.chkEligibleForPromotion.TabIndex = 15;
            this.chkEligibleForPromotion.Text = "Eligible for Promotion";
            this.chkEligibleForPromotion.UseVisualStyleBackColor = true;
            // 
            // PromoteStudentUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(356, 307);
            this.Controls.Add(this.chkEligibleForPromotion);
            this.Controls.Add(this.txtArt);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnPromoteStudent);
            this.Controls.Add(this.txtStudentName);
            this.Controls.Add(this.lblArt);
            this.Controls.Add(this.lblPromotionDate);
            this.Controls.Add(this.dtPromotionDate);
            this.Controls.Add(this.cmbNextRank);
            this.Controls.Add(this.lblNextRank);
            this.Controls.Add(this.lblCurrentRank);
            this.Controls.Add(this.txtCurrentRank);
            this.Controls.Add(this.lblStudent);
            this.Name = "PromoteStudentUI";
            this.ShowIcon = false;
            this.Text = "Promote Student";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblStudent;
        private System.Windows.Forms.TextBox txtCurrentRank;
        private System.Windows.Forms.Label lblCurrentRank;
        private System.Windows.Forms.Label lblNextRank;
        private System.Windows.Forms.ComboBox cmbNextRank;
        private System.Windows.Forms.DateTimePicker dtPromotionDate;
        private System.Windows.Forms.Label lblPromotionDate;
        private System.Windows.Forms.Label lblArt;
        private System.Windows.Forms.TextBox txtStudentName;
        private System.Windows.Forms.Button btnPromoteStudent;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtArt;
        private System.Windows.Forms.CheckBox chkEligibleForPromotion;
    }
}