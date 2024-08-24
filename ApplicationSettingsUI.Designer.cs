
namespace DojoStudentManagement
{
    partial class ApplicationSettingsUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ApplicationSettingsUI));
            this.lblDatabasePath = new System.Windows.Forms.Label();
            this.txtDatabaseFilePath = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cbShowPromotionEligibility = new System.Windows.Forms.CheckBox();
            this.txtHoursBetweenSignIns = new System.Windows.Forms.TextBox();
            this.lblHoursBetweenSignIns = new System.Windows.Forms.Label();
            this.toolTipSignIn = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // lblDatabasePath
            // 
            this.lblDatabasePath.AutoSize = true;
            this.lblDatabasePath.Location = new System.Drawing.Point(12, 39);
            this.lblDatabasePath.Name = "lblDatabasePath";
            this.lblDatabasePath.Size = new System.Drawing.Size(78, 13);
            this.lblDatabasePath.TabIndex = 0;
            this.lblDatabasePath.Text = "Database Path";
            // 
            // txtDatabaseFilePath
            // 
            this.txtDatabaseFilePath.Location = new System.Drawing.Point(12, 55);
            this.txtDatabaseFilePath.Name = "txtDatabaseFilePath";
            this.txtDatabaseFilePath.ReadOnly = true;
            this.txtDatabaseFilePath.Size = new System.Drawing.Size(392, 20);
            this.txtDatabaseFilePath.TabIndex = 1;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Image = ((System.Drawing.Image)(resources.GetObject("btnBrowse.Image")));
            this.btnBrowse.Location = new System.Drawing.Point(317, 81);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(87, 26);
            this.btnBrowse.TabIndex = 2;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnOK
            // 
            this.btnOK.Image = ((System.Drawing.Image)(resources.GetObject("btnOK.Image")));
            this.btnOK.Location = new System.Drawing.Point(105, 211);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(82, 23);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.Location = new System.Drawing.Point(193, 211);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // cbShowPromotionEligibility
            // 
            this.cbShowPromotionEligibility.AutoSize = true;
            this.cbShowPromotionEligibility.Location = new System.Drawing.Point(15, 165);
            this.cbShowPromotionEligibility.Name = "cbShowPromotionEligibility";
            this.cbShowPromotionEligibility.Size = new System.Drawing.Size(247, 17);
            this.cbShowPromotionEligibility.TabIndex = 5;
            this.cbShowPromotionEligibility.Text = "Show Promotion Eligibility Messages on Sign-In";
            this.cbShowPromotionEligibility.UseMnemonic = false;
            this.cbShowPromotionEligibility.UseVisualStyleBackColor = true;
            this.cbShowPromotionEligibility.CheckedChanged += new System.EventHandler(this.cbShowPromotionEligibility_CheckedChanged);
            // 
            // txtHoursBetweenSignIns
            // 
            this.txtHoursBetweenSignIns.Location = new System.Drawing.Point(15, 120);
            this.txtHoursBetweenSignIns.Name = "txtHoursBetweenSignIns";
            this.txtHoursBetweenSignIns.Size = new System.Drawing.Size(172, 20);
            this.txtHoursBetweenSignIns.TabIndex = 6;
            this.toolTipSignIn.SetToolTip(this.txtHoursBetweenSignIns, "Time (in hours) before a student can sign in again for the same art.");
            this.txtHoursBetweenSignIns.Validating += new System.ComponentModel.CancelEventHandler(this.txtHoursBetweenSignIns_Validating);
            // 
            // lblHoursBetweenSignIns
            // 
            this.lblHoursBetweenSignIns.AutoSize = true;
            this.lblHoursBetweenSignIns.Location = new System.Drawing.Point(15, 101);
            this.lblHoursBetweenSignIns.Name = "lblHoursBetweenSignIns";
            this.lblHoursBetweenSignIns.Size = new System.Drawing.Size(161, 13);
            this.lblHoursBetweenSignIns.TabIndex = 7;
            this.lblHoursBetweenSignIns.Text = "Hours Between Student Sign-Ins";
            this.toolTipSignIn.SetToolTip(this.lblHoursBetweenSignIns, "Time (in hours) before a student can sign in again for the same art.");
            // 
            // toolTipSignIn
            // 
            this.toolTipSignIn.ToolTipTitle = "Time limit between sign-ins";
            // 
            // ApplicationSettingsUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(415, 246);
            this.Controls.Add(this.lblHoursBetweenSignIns);
            this.Controls.Add(this.txtHoursBetweenSignIns);
            this.Controls.Add(this.cbShowPromotionEligibility);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtDatabaseFilePath);
            this.Controls.Add(this.lblDatabasePath);
            this.Name = "ApplicationSettingsUI";
            this.ShowIcon = false;
            this.Text = "Application Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblDatabasePath;
        private System.Windows.Forms.TextBox txtDatabaseFilePath;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox cbShowPromotionEligibility;
        private System.Windows.Forms.TextBox txtHoursBetweenSignIns;
        private System.Windows.Forms.Label lblHoursBetweenSignIns;
        private System.Windows.Forms.ToolTip toolTipSignIn;
    }
}