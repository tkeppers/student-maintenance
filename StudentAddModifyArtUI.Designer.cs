
namespace DojoStudentManagement
{
    partial class StudentAddModifyArtUI
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StudentAddModifyArtUI));
            this.gbModifyArt = new System.Windows.Forms.GroupBox();
            this.lblArt = new System.Windows.Forms.Label();
            this.cmbArtType = new System.Windows.Forms.ComboBox();
            this.lblCurrentRank = new System.Windows.Forms.Label();
            this.txtCurrentRank = new System.Windows.Forms.TextBox();
            this.lblLastPromotionDate = new System.Windows.Forms.Label();
            this.dtPromotionDate = new System.Windows.Forms.DateTimePicker();
            this.lblLastSignInDate = new System.Windows.Forms.Label();
            this.dtLastSignin = new System.Windows.Forms.DateTimePicker();
            this.lblPromotionHours = new System.Windows.Forms.Label();
            this.txtPromotionHours = new System.Windows.Forms.TextBox();
            this.lblBeginDate = new System.Windows.Forms.Label();
            this.dtBeginDate = new System.Windows.Forms.DateTimePicker();
            this.txtCumulativeHours = new System.Windows.Forms.TextBox();
            this.lblCumulativeHours = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.btnCancel = new System.Windows.Forms.Button();
            this.gbModifyArt.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbModifyArt
            // 
            this.gbModifyArt.Controls.Add(this.lblArt);
            this.gbModifyArt.Controls.Add(this.cmbArtType);
            this.gbModifyArt.Controls.Add(this.lblCurrentRank);
            this.gbModifyArt.Controls.Add(this.txtCurrentRank);
            this.gbModifyArt.Controls.Add(this.lblLastPromotionDate);
            this.gbModifyArt.Controls.Add(this.dtPromotionDate);
            this.gbModifyArt.Controls.Add(this.lblLastSignInDate);
            this.gbModifyArt.Controls.Add(this.dtLastSignin);
            this.gbModifyArt.Controls.Add(this.lblPromotionHours);
            this.gbModifyArt.Controls.Add(this.txtPromotionHours);
            this.gbModifyArt.Controls.Add(this.lblBeginDate);
            this.gbModifyArt.Controls.Add(this.dtBeginDate);
            this.gbModifyArt.Controls.Add(this.txtCumulativeHours);
            this.gbModifyArt.Controls.Add(this.lblCumulativeHours);
            this.gbModifyArt.Location = new System.Drawing.Point(12, 21);
            this.gbModifyArt.Name = "gbModifyArt";
            this.gbModifyArt.Size = new System.Drawing.Size(465, 150);
            this.gbModifyArt.TabIndex = 3;
            this.gbModifyArt.TabStop = false;
            this.gbModifyArt.Text = "Art Information";
            // 
            // lblArt
            // 
            this.lblArt.AutoSize = true;
            this.lblArt.Location = new System.Drawing.Point(7, 28);
            this.lblArt.Name = "lblArt";
            this.lblArt.Size = new System.Drawing.Size(20, 13);
            this.lblArt.TabIndex = 14;
            this.lblArt.Text = "Art";
            // 
            // cmbArtType
            // 
            this.cmbArtType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbArtType.FormattingEnabled = true;
            this.cmbArtType.Location = new System.Drawing.Point(10, 47);
            this.cmbArtType.Name = "cmbArtType";
            this.cmbArtType.Size = new System.Drawing.Size(121, 21);
            this.cmbArtType.TabIndex = 13;
            // 
            // lblCurrentRank
            // 
            this.lblCurrentRank.AutoSize = true;
            this.lblCurrentRank.Location = new System.Drawing.Point(7, 91);
            this.lblCurrentRank.Name = "lblCurrentRank";
            this.lblCurrentRank.Size = new System.Drawing.Size(73, 13);
            this.lblCurrentRank.TabIndex = 12;
            this.lblCurrentRank.Text = "Current Rank:";
            // 
            // txtCurrentRank
            // 
            this.txtCurrentRank.Location = new System.Drawing.Point(10, 107);
            this.txtCurrentRank.Name = "txtCurrentRank";
            this.txtCurrentRank.ReadOnly = true;
            this.txtCurrentRank.Size = new System.Drawing.Size(121, 20);
            this.txtCurrentRank.TabIndex = 11;
            // 
            // lblLastPromotionDate
            // 
            this.lblLastPromotionDate.AutoSize = true;
            this.lblLastPromotionDate.Location = new System.Drawing.Point(337, 91);
            this.lblLastPromotionDate.Name = "lblLastPromotionDate";
            this.lblLastPromotionDate.Size = new System.Drawing.Size(77, 13);
            this.lblLastPromotionDate.TabIndex = 10;
            this.lblLastPromotionDate.Text = "Last Promotion";
            // 
            // dtPromotionDate
            // 
            this.dtPromotionDate.Enabled = false;
            this.dtPromotionDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtPromotionDate.Location = new System.Drawing.Point(337, 107);
            this.dtPromotionDate.Name = "dtPromotionDate";
            this.dtPromotionDate.Size = new System.Drawing.Size(117, 20);
            this.dtPromotionDate.TabIndex = 9;
            // 
            // lblLastSignInDate
            // 
            this.lblLastSignInDate.AutoSize = true;
            this.lblLastSignInDate.Location = new System.Drawing.Point(135, 91);
            this.lblLastSignInDate.Name = "lblLastSignInDate";
            this.lblLastSignInDate.Size = new System.Drawing.Size(88, 13);
            this.lblLastSignInDate.TabIndex = 8;
            this.lblLastSignInDate.Text = "Last Sign in Date";
            // 
            // dtLastSignin
            // 
            this.dtLastSignin.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtLastSignin.Location = new System.Drawing.Point(138, 107);
            this.dtLastSignin.Name = "dtLastSignin";
            this.dtLastSignin.Size = new System.Drawing.Size(101, 20);
            this.dtLastSignin.TabIndex = 7;
            // 
            // lblPromotionHours
            // 
            this.lblPromotionHours.AutoSize = true;
            this.lblPromotionHours.Location = new System.Drawing.Point(246, 91);
            this.lblPromotionHours.Name = "lblPromotionHours";
            this.lblPromotionHours.Size = new System.Drawing.Size(85, 13);
            this.lblPromotionHours.TabIndex = 6;
            this.lblPromotionHours.Text = "Promotion Hours";
            // 
            // txtPromotionHours
            // 
            this.txtPromotionHours.Location = new System.Drawing.Point(246, 107);
            this.txtPromotionHours.Name = "txtPromotionHours";
            this.txtPromotionHours.ReadOnly = true;
            this.txtPromotionHours.Size = new System.Drawing.Size(85, 20);
            this.txtPromotionHours.TabIndex = 5;
            // 
            // lblBeginDate
            // 
            this.lblBeginDate.AutoSize = true;
            this.lblBeginDate.Location = new System.Drawing.Point(135, 32);
            this.lblBeginDate.Name = "lblBeginDate";
            this.lblBeginDate.Size = new System.Drawing.Size(60, 13);
            this.lblBeginDate.TabIndex = 3;
            this.lblBeginDate.Text = "Begin Date";
            // 
            // dtBeginDate
            // 
            this.dtBeginDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtBeginDate.Location = new System.Drawing.Point(138, 48);
            this.dtBeginDate.Name = "dtBeginDate";
            this.dtBeginDate.Size = new System.Drawing.Size(100, 20);
            this.dtBeginDate.TabIndex = 2;
            // 
            // txtCumulativeHours
            // 
            this.txtCumulativeHours.Location = new System.Drawing.Point(244, 48);
            this.txtCumulativeHours.Name = "txtCumulativeHours";
            this.txtCumulativeHours.Size = new System.Drawing.Size(87, 20);
            this.txtCumulativeHours.TabIndex = 0;
            // 
            // lblCumulativeHours
            // 
            this.lblCumulativeHours.AutoSize = true;
            this.lblCumulativeHours.Location = new System.Drawing.Point(241, 29);
            this.lblCumulativeHours.Name = "lblCumulativeHours";
            this.lblCumulativeHours.Size = new System.Drawing.Size(90, 13);
            this.lblCumulativeHours.TabIndex = 1;
            this.lblCumulativeHours.Text = "Cumulative Hours";
            // 
            // btnSave
            // 
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.Location = new System.Drawing.Point(132, 177);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Save";
            this.btnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.Location = new System.Drawing.Point(213, 177);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // StudentAddModifyArtUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(488, 218);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.gbModifyArt);
            this.Name = "StudentAddModifyArtUI";
            this.ShowIcon = false;
            this.Text = "StudentAddModifyArtUI";
            this.gbModifyArt.ResumeLayout(false);
            this.gbModifyArt.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbModifyArt;
        private System.Windows.Forms.Label lblBeginDate;
        private System.Windows.Forms.DateTimePicker dtBeginDate;
        private System.Windows.Forms.TextBox txtCumulativeHours;
        private System.Windows.Forms.Label lblCumulativeHours;
        private System.Windows.Forms.Label lblCurrentRank;
        private System.Windows.Forms.TextBox txtCurrentRank;
        private System.Windows.Forms.Label lblLastPromotionDate;
        private System.Windows.Forms.DateTimePicker dtPromotionDate;
        private System.Windows.Forms.Label lblLastSignInDate;
        private System.Windows.Forms.DateTimePicker dtLastSignin;
        private System.Windows.Forms.Label lblPromotionHours;
        private System.Windows.Forms.TextBox txtPromotionHours;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblArt;
        private System.Windows.Forms.ComboBox cmbArtType;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button btnCancel;
    }
}