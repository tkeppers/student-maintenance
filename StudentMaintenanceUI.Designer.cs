
namespace DojoStudentManagement
{
    partial class StudentMaintenanceUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StudentMaintenanceUI));
            this.txtFirstName = new System.Windows.Forms.TextBox();
            this.gbStudentInfo = new System.Windows.Forms.GroupBox();
            this.btnSignInHistory = new System.Windows.Forms.Button();
            this.btnPromotionHistory = new System.Windows.Forms.Button();
            this.lblBirthDate = new System.Windows.Forms.Label();
            this.dtBirthdate = new System.Windows.Forms.DateTimePicker();
            this.txtSecondaryPhone = new System.Windows.Forms.TextBox();
            this.lblSecondaryPhone = new System.Windows.Forms.Label();
            this.lblPrimaryPhone = new System.Windows.Forms.Label();
            this.txtPrimaryPhone = new System.Windows.Forms.TextBox();
            this.lblEmailAddress = new System.Windows.Forms.Label();
            this.txtEmailAddress = new System.Windows.Forms.TextBox();
            this.gbGender = new System.Windows.Forms.GroupBox();
            this.rbUnknown = new System.Windows.Forms.RadioButton();
            this.rbFemale = new System.Windows.Forms.RadioButton();
            this.rbMale = new System.Windows.Forms.RadioButton();
            this.lblZip = new System.Windows.Forms.Label();
            this.txtZipCode = new System.Windows.Forms.TextBox();
            this.lblState = new System.Windows.Forms.Label();
            this.txtState = new System.Windows.Forms.TextBox();
            this.txtCity = new System.Windows.Forms.TextBox();
            this.lblCity = new System.Windows.Forms.Label();
            this.lblAddress2 = new System.Windows.Forms.Label();
            this.lblAddress1 = new System.Windows.Forms.Label();
            this.txtAddress2 = new System.Windows.Forms.TextBox();
            this.txtAddress1 = new System.Windows.Forms.TextBox();
            this.lblLastName = new System.Windows.Forms.Label();
            this.lblFirstName = new System.Windows.Forms.Label();
            this.txtLastName = new System.Windows.Forms.TextBox();
            this.lblArtsAndRank = new System.Windows.Forms.Label();
            this.lvwArtsAndRanks = new System.Windows.Forms.ListView();
            this.columnArt = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnRank = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnStartDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnPromotionDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnPromotionHours = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.dgvStudentList = new System.Windows.Forms.DataGridView();
            this.gbStudentInfo.SuspendLayout();
            this.gbGender.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStudentList)).BeginInit();
            this.SuspendLayout();
            // 
            // txtFirstName
            // 
            this.txtFirstName.Location = new System.Drawing.Point(24, 46);
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new System.Drawing.Size(155, 20);
            this.txtFirstName.TabIndex = 1;
            // 
            // gbStudentInfo
            // 
            this.gbStudentInfo.Controls.Add(this.btnSignInHistory);
            this.gbStudentInfo.Controls.Add(this.btnPromotionHistory);
            this.gbStudentInfo.Controls.Add(this.lblBirthDate);
            this.gbStudentInfo.Controls.Add(this.dtBirthdate);
            this.gbStudentInfo.Controls.Add(this.txtSecondaryPhone);
            this.gbStudentInfo.Controls.Add(this.lblSecondaryPhone);
            this.gbStudentInfo.Controls.Add(this.lblPrimaryPhone);
            this.gbStudentInfo.Controls.Add(this.txtPrimaryPhone);
            this.gbStudentInfo.Controls.Add(this.lblEmailAddress);
            this.gbStudentInfo.Controls.Add(this.txtEmailAddress);
            this.gbStudentInfo.Controls.Add(this.gbGender);
            this.gbStudentInfo.Controls.Add(this.lblZip);
            this.gbStudentInfo.Controls.Add(this.txtZipCode);
            this.gbStudentInfo.Controls.Add(this.lblState);
            this.gbStudentInfo.Controls.Add(this.txtState);
            this.gbStudentInfo.Controls.Add(this.txtCity);
            this.gbStudentInfo.Controls.Add(this.lblCity);
            this.gbStudentInfo.Controls.Add(this.lblAddress2);
            this.gbStudentInfo.Controls.Add(this.lblAddress1);
            this.gbStudentInfo.Controls.Add(this.txtAddress2);
            this.gbStudentInfo.Controls.Add(this.txtAddress1);
            this.gbStudentInfo.Controls.Add(this.lblLastName);
            this.gbStudentInfo.Controls.Add(this.lblFirstName);
            this.gbStudentInfo.Controls.Add(this.txtLastName);
            this.gbStudentInfo.Controls.Add(this.txtFirstName);
            this.gbStudentInfo.Location = new System.Drawing.Point(370, 31);
            this.gbStudentInfo.Name = "gbStudentInfo";
            this.gbStudentInfo.Size = new System.Drawing.Size(515, 305);
            this.gbStudentInfo.TabIndex = 2;
            this.gbStudentInfo.TabStop = false;
            this.gbStudentInfo.Text = "Student Information";
            // 
            // btnSignInHistory
            // 
            this.btnSignInHistory.Location = new System.Drawing.Point(363, 186);
            this.btnSignInHistory.Name = "btnSignInHistory";
            this.btnSignInHistory.Size = new System.Drawing.Size(124, 34);
            this.btnSignInHistory.TabIndex = 24;
            this.btnSignInHistory.Text = "Sign-In History";
            this.btnSignInHistory.UseVisualStyleBackColor = true;
            // 
            // btnPromotionHistory
            // 
            this.btnPromotionHistory.Image = ((System.Drawing.Image)(resources.GetObject("btnPromotionHistory.Image")));
            this.btnPromotionHistory.Location = new System.Drawing.Point(363, 146);
            this.btnPromotionHistory.Name = "btnPromotionHistory";
            this.btnPromotionHistory.Size = new System.Drawing.Size(124, 34);
            this.btnPromotionHistory.TabIndex = 7;
            this.btnPromotionHistory.Text = "Promotion History";
            this.btnPromotionHistory.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnPromotionHistory.UseVisualStyleBackColor = true;
            this.btnPromotionHistory.Click += new System.EventHandler(this.btnPromotionHistory_Click);
            // 
            // lblBirthDate
            // 
            this.lblBirthDate.AutoSize = true;
            this.lblBirthDate.Location = new System.Drawing.Point(247, 250);
            this.lblBirthDate.Name = "lblBirthDate";
            this.lblBirthDate.Size = new System.Drawing.Size(54, 13);
            this.lblBirthDate.TabIndex = 23;
            this.lblBirthDate.Text = "Birth Date";
            // 
            // dtBirthdate
            // 
            this.dtBirthdate.Location = new System.Drawing.Point(250, 265);
            this.dtBirthdate.Name = "dtBirthdate";
            this.dtBirthdate.Size = new System.Drawing.Size(200, 20);
            this.dtBirthdate.TabIndex = 22;
            // 
            // txtSecondaryPhone
            // 
            this.txtSecondaryPhone.Location = new System.Drawing.Point(136, 265);
            this.txtSecondaryPhone.Name = "txtSecondaryPhone";
            this.txtSecondaryPhone.Size = new System.Drawing.Size(100, 20);
            this.txtSecondaryPhone.TabIndex = 21;
            // 
            // lblSecondaryPhone
            // 
            this.lblSecondaryPhone.AutoSize = true;
            this.lblSecondaryPhone.Location = new System.Drawing.Point(133, 250);
            this.lblSecondaryPhone.Name = "lblSecondaryPhone";
            this.lblSecondaryPhone.Size = new System.Drawing.Size(92, 13);
            this.lblSecondaryPhone.TabIndex = 20;
            this.lblSecondaryPhone.Text = "Secondary Phone";
            // 
            // lblPrimaryPhone
            // 
            this.lblPrimaryPhone.AutoSize = true;
            this.lblPrimaryPhone.Location = new System.Drawing.Point(21, 250);
            this.lblPrimaryPhone.Name = "lblPrimaryPhone";
            this.lblPrimaryPhone.Size = new System.Drawing.Size(75, 13);
            this.lblPrimaryPhone.TabIndex = 19;
            this.lblPrimaryPhone.Text = "Primary Phone";
            // 
            // txtPrimaryPhone
            // 
            this.txtPrimaryPhone.Location = new System.Drawing.Point(24, 266);
            this.txtPrimaryPhone.Name = "txtPrimaryPhone";
            this.txtPrimaryPhone.Size = new System.Drawing.Size(100, 20);
            this.txtPrimaryPhone.TabIndex = 18;
            // 
            // lblEmailAddress
            // 
            this.lblEmailAddress.AutoSize = true;
            this.lblEmailAddress.Location = new System.Drawing.Point(24, 207);
            this.lblEmailAddress.Name = "lblEmailAddress";
            this.lblEmailAddress.Size = new System.Drawing.Size(32, 13);
            this.lblEmailAddress.TabIndex = 17;
            this.lblEmailAddress.Text = "Email";
            // 
            // txtEmailAddress
            // 
            this.txtEmailAddress.Location = new System.Drawing.Point(24, 223);
            this.txtEmailAddress.Name = "txtEmailAddress";
            this.txtEmailAddress.Size = new System.Drawing.Size(316, 20);
            this.txtEmailAddress.TabIndex = 16;
            // 
            // gbGender
            // 
            this.gbGender.Controls.Add(this.rbUnknown);
            this.gbGender.Controls.Add(this.rbFemale);
            this.gbGender.Controls.Add(this.rbMale);
            this.gbGender.Location = new System.Drawing.Point(363, 46);
            this.gbGender.Name = "gbGender";
            this.gbGender.Size = new System.Drawing.Size(87, 92);
            this.gbGender.TabIndex = 15;
            this.gbGender.TabStop = false;
            this.gbGender.Text = "Gender";
            // 
            // rbUnknown
            // 
            this.rbUnknown.AutoSize = true;
            this.rbUnknown.Location = new System.Drawing.Point(7, 69);
            this.rbUnknown.Name = "rbUnknown";
            this.rbUnknown.Size = new System.Drawing.Size(71, 17);
            this.rbUnknown.TabIndex = 2;
            this.rbUnknown.TabStop = true;
            this.rbUnknown.Text = "Unknown";
            this.rbUnknown.UseVisualStyleBackColor = true;
            // 
            // rbFemale
            // 
            this.rbFemale.AutoSize = true;
            this.rbFemale.Location = new System.Drawing.Point(6, 45);
            this.rbFemale.Name = "rbFemale";
            this.rbFemale.Size = new System.Drawing.Size(59, 17);
            this.rbFemale.TabIndex = 1;
            this.rbFemale.TabStop = true;
            this.rbFemale.Text = "Female";
            this.rbFemale.UseVisualStyleBackColor = true;
            // 
            // rbMale
            // 
            this.rbMale.AutoSize = true;
            this.rbMale.Location = new System.Drawing.Point(6, 22);
            this.rbMale.Name = "rbMale";
            this.rbMale.Size = new System.Drawing.Size(48, 17);
            this.rbMale.TabIndex = 0;
            this.rbMale.TabStop = true;
            this.rbMale.Text = "Male";
            this.rbMale.UseVisualStyleBackColor = true;
            // 
            // lblZip
            // 
            this.lblZip.AutoSize = true;
            this.lblZip.Location = new System.Drawing.Point(247, 157);
            this.lblZip.Name = "lblZip";
            this.lblZip.Size = new System.Drawing.Size(22, 13);
            this.lblZip.TabIndex = 14;
            this.lblZip.Text = "Zip";
            // 
            // txtZipCode
            // 
            this.txtZipCode.Location = new System.Drawing.Point(250, 173);
            this.txtZipCode.Name = "txtZipCode";
            this.txtZipCode.Size = new System.Drawing.Size(90, 20);
            this.txtZipCode.TabIndex = 13;
            // 
            // lblState
            // 
            this.lblState.AutoSize = true;
            this.lblState.Location = new System.Drawing.Point(188, 158);
            this.lblState.Name = "lblState";
            this.lblState.Size = new System.Drawing.Size(32, 13);
            this.lblState.TabIndex = 12;
            this.lblState.Text = "State";
            // 
            // txtState
            // 
            this.txtState.Location = new System.Drawing.Point(188, 174);
            this.txtState.Name = "txtState";
            this.txtState.Size = new System.Drawing.Size(55, 20);
            this.txtState.TabIndex = 11;
            // 
            // txtCity
            // 
            this.txtCity.Location = new System.Drawing.Point(24, 174);
            this.txtCity.Name = "txtCity";
            this.txtCity.Size = new System.Drawing.Size(155, 20);
            this.txtCity.TabIndex = 10;
            // 
            // lblCity
            // 
            this.lblCity.AutoSize = true;
            this.lblCity.Location = new System.Drawing.Point(24, 158);
            this.lblCity.Name = "lblCity";
            this.lblCity.Size = new System.Drawing.Size(24, 13);
            this.lblCity.TabIndex = 9;
            this.lblCity.Text = "City";
            // 
            // lblAddress2
            // 
            this.lblAddress2.AutoSize = true;
            this.lblAddress2.Location = new System.Drawing.Point(24, 112);
            this.lblAddress2.Name = "lblAddress2";
            this.lblAddress2.Size = new System.Drawing.Size(54, 13);
            this.lblAddress2.TabIndex = 8;
            this.lblAddress2.Text = "Address 2";
            // 
            // lblAddress1
            // 
            this.lblAddress1.AutoSize = true;
            this.lblAddress1.Location = new System.Drawing.Point(24, 72);
            this.lblAddress1.Name = "lblAddress1";
            this.lblAddress1.Size = new System.Drawing.Size(54, 13);
            this.lblAddress1.TabIndex = 7;
            this.lblAddress1.Text = "Address 1";
            // 
            // txtAddress2
            // 
            this.txtAddress2.Location = new System.Drawing.Point(24, 128);
            this.txtAddress2.Name = "txtAddress2";
            this.txtAddress2.Size = new System.Drawing.Size(316, 20);
            this.txtAddress2.TabIndex = 6;
            // 
            // txtAddress1
            // 
            this.txtAddress1.Location = new System.Drawing.Point(24, 88);
            this.txtAddress1.Name = "txtAddress1";
            this.txtAddress1.Size = new System.Drawing.Size(316, 20);
            this.txtAddress1.TabIndex = 5;
            // 
            // lblLastName
            // 
            this.lblLastName.AutoSize = true;
            this.lblLastName.Location = new System.Drawing.Point(185, 27);
            this.lblLastName.Name = "lblLastName";
            this.lblLastName.Size = new System.Drawing.Size(58, 13);
            this.lblLastName.TabIndex = 4;
            this.lblLastName.Text = "Last Name";
            // 
            // lblFirstName
            // 
            this.lblFirstName.AutoSize = true;
            this.lblFirstName.Location = new System.Drawing.Point(21, 30);
            this.lblFirstName.Name = "lblFirstName";
            this.lblFirstName.Size = new System.Drawing.Size(57, 13);
            this.lblFirstName.TabIndex = 3;
            this.lblFirstName.Text = "First Name";
            // 
            // txtLastName
            // 
            this.txtLastName.Location = new System.Drawing.Point(185, 46);
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.Size = new System.Drawing.Size(155, 20);
            this.txtLastName.TabIndex = 2;
            // 
            // lblArtsAndRank
            // 
            this.lblArtsAndRank.AutoSize = true;
            this.lblArtsAndRank.Location = new System.Drawing.Point(33, 343);
            this.lblArtsAndRank.Name = "lblArtsAndRank";
            this.lblArtsAndRank.Size = new System.Drawing.Size(75, 13);
            this.lblArtsAndRank.TabIndex = 5;
            this.lblArtsAndRank.Text = "Arts and Rank";
            // 
            // lvwArtsAndRanks
            // 
            this.lvwArtsAndRanks.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnArt,
            this.columnRank,
            this.columnStartDate,
            this.columnPromotionDate,
            this.columnPromotionHours});
            this.lvwArtsAndRanks.FullRowSelect = true;
            this.lvwArtsAndRanks.GridLines = true;
            this.lvwArtsAndRanks.HideSelection = false;
            this.lvwArtsAndRanks.Location = new System.Drawing.Point(24, 365);
            this.lvwArtsAndRanks.Name = "lvwArtsAndRanks";
            this.lvwArtsAndRanks.Size = new System.Drawing.Size(308, 139);
            this.lvwArtsAndRanks.TabIndex = 6;
            this.lvwArtsAndRanks.UseCompatibleStateImageBehavior = false;
            this.lvwArtsAndRanks.View = System.Windows.Forms.View.Details;
            // 
            // columnArt
            // 
            this.columnArt.Text = "Art";
            // 
            // columnRank
            // 
            this.columnRank.Text = "Rank";
            // 
            // columnStartDate
            // 
            this.columnStartDate.Text = "Start Date";
            // 
            // columnPromotionDate
            // 
            this.columnPromotionDate.Text = "Promotion Date";
            // 
            // columnPromotionHours
            // 
            this.columnPromotionHours.Text = "Promotion Hours";
            this.columnPromotionHours.Width = 64;
            // 
            // dgvStudentList
            // 
            this.dgvStudentList.AllowUserToAddRows = false;
            this.dgvStudentList.AllowUserToDeleteRows = false;
            this.dgvStudentList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStudentList.Location = new System.Drawing.Point(13, 31);
            this.dgvStudentList.MultiSelect = false;
            this.dgvStudentList.Name = "dgvStudentList";
            this.dgvStudentList.ReadOnly = true;
            this.dgvStudentList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvStudentList.Size = new System.Drawing.Size(319, 286);
            this.dgvStudentList.TabIndex = 7;
            this.dgvStudentList.SelectionChanged += new System.EventHandler(this.dgvStudentList_SelectionChanged);
            // 
            // StudentMaintenanceUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(989, 538);
            this.Controls.Add(this.dgvStudentList);
            this.Controls.Add(this.lvwArtsAndRanks);
            this.Controls.Add(this.lblArtsAndRank);
            this.Controls.Add(this.gbStudentInfo);
            this.Name = "StudentMaintenanceUI";
            this.Text = "Student Maintenance";
            this.Load += new System.EventHandler(this.StudentMaintenance_Load);
            this.gbStudentInfo.ResumeLayout(false);
            this.gbStudentInfo.PerformLayout();
            this.gbGender.ResumeLayout(false);
            this.gbGender.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStudentList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txtFirstName;
        private System.Windows.Forms.GroupBox gbStudentInfo;
        private System.Windows.Forms.Label lblEmailAddress;
        private System.Windows.Forms.TextBox txtEmailAddress;
        private System.Windows.Forms.GroupBox gbGender;
        private System.Windows.Forms.RadioButton rbUnknown;
        private System.Windows.Forms.RadioButton rbFemale;
        private System.Windows.Forms.RadioButton rbMale;
        private System.Windows.Forms.Label lblZip;
        private System.Windows.Forms.TextBox txtZipCode;
        private System.Windows.Forms.Label lblState;
        private System.Windows.Forms.TextBox txtState;
        private System.Windows.Forms.TextBox txtCity;
        private System.Windows.Forms.Label lblCity;
        private System.Windows.Forms.Label lblAddress2;
        private System.Windows.Forms.Label lblAddress1;
        private System.Windows.Forms.TextBox txtAddress2;
        private System.Windows.Forms.TextBox txtAddress1;
        private System.Windows.Forms.Label lblLastName;
        private System.Windows.Forms.Label lblFirstName;
        private System.Windows.Forms.TextBox txtLastName;
        private System.Windows.Forms.TextBox txtSecondaryPhone;
        private System.Windows.Forms.Label lblSecondaryPhone;
        private System.Windows.Forms.Label lblPrimaryPhone;
        private System.Windows.Forms.TextBox txtPrimaryPhone;
        private System.Windows.Forms.Label lblBirthDate;
        private System.Windows.Forms.DateTimePicker dtBirthdate;
        private System.Windows.Forms.Label lblArtsAndRank;
        private System.Windows.Forms.ListView lvwArtsAndRanks;
        private System.Windows.Forms.ColumnHeader columnArt;
        private System.Windows.Forms.ColumnHeader columnRank;
        private System.Windows.Forms.ColumnHeader columnStartDate;
        private System.Windows.Forms.ColumnHeader columnPromotionDate;
        private System.Windows.Forms.ColumnHeader columnPromotionHours;
        private System.Windows.Forms.Button btnSignInHistory;
        private System.Windows.Forms.Button btnPromotionHistory;
        private System.Windows.Forms.DataGridView dgvStudentList;
    }
}

