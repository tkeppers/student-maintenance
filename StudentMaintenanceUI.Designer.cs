
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.txtFirstName = new System.Windows.Forms.TextBox();
            this.gbStudentInfo = new System.Windows.Forms.GroupBox();
            this.btnDeleteStudent = new System.Windows.Forms.Button();
            this.btnSaveChanges = new System.Windows.Forms.Button();
            this.lblHomeDojo = new System.Windows.Forms.Label();
            this.txtHomeDojo = new System.Windows.Forms.TextBox();
            this.cbActiveStudent = new System.Windows.Forms.CheckBox();
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
            this.columnCumulativeHours = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnPromotionDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnPromotionHours = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnStudentArtID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnLastSignInDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.dgvStudentList = new System.Windows.Forms.DataGridView();
            this.gbQuickFilters = new System.Windows.Forms.GroupBox();
            this.lblLastNameFilter = new System.Windows.Forms.Label();
            this.lblFirstNameFilter = new System.Windows.Forms.Label();
            this.txtLastNameFilter = new System.Windows.Forms.TextBox();
            this.txtFirstNameFilter = new System.Windows.Forms.TextBox();
            this.cbNonWindsongStudents = new System.Windows.Forms.CheckBox();
            this.cbShowInactiveStudents = new System.Windows.Forms.CheckBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbSettings = new System.Windows.Forms.ToolStripDropDownButton();
            this.databasePathToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbAddNewStudent = new System.Windows.Forms.ToolStripButton();
            this.tsbPromotionSettings = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.gbArtsAndRank = new System.Windows.Forms.GroupBox();
            this.btnRemoveArt = new System.Windows.Forms.Button();
            this.btnModifyArt = new System.Windows.Forms.Button();
            this.btnAddArt = new System.Windows.Forms.Button();
            this.lblPromotionEligibility = new System.Windows.Forms.Label();
            this.btnPromoteStudent = new System.Windows.Forms.Button();
            this.richTextPromotionEligibility = new System.Windows.Forms.RichTextBox();
            this.gbStudentInfo.SuspendLayout();
            this.gbGender.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStudentList)).BeginInit();
            this.gbQuickFilters.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.gbArtsAndRank.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtFirstName
            // 
            this.txtFirstName.Location = new System.Drawing.Point(21, 63);
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new System.Drawing.Size(189, 29);
            this.txtFirstName.TabIndex = 1;
            // 
            // gbStudentInfo
            // 
            this.gbStudentInfo.Controls.Add(this.btnDeleteStudent);
            this.gbStudentInfo.Controls.Add(this.btnSaveChanges);
            this.gbStudentInfo.Controls.Add(this.lblHomeDojo);
            this.gbStudentInfo.Controls.Add(this.txtHomeDojo);
            this.gbStudentInfo.Controls.Add(this.cbActiveStudent);
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
            this.gbStudentInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbStudentInfo.Location = new System.Drawing.Point(471, 69);
            this.gbStudentInfo.Name = "gbStudentInfo";
            this.gbStudentInfo.Size = new System.Drawing.Size(637, 490);
            this.gbStudentInfo.TabIndex = 2;
            this.gbStudentInfo.TabStop = false;
            this.gbStudentInfo.Text = "Student Information";
            // 
            // btnDeleteStudent
            // 
            this.btnDeleteStudent.Image = ((System.Drawing.Image)(resources.GetObject("btnDeleteStudent.Image")));
            this.btnDeleteStudent.Location = new System.Drawing.Point(471, 419);
            this.btnDeleteStudent.Name = "btnDeleteStudent";
            this.btnDeleteStudent.Size = new System.Drawing.Size(130, 65);
            this.btnDeleteStudent.TabIndex = 29;
            this.btnDeleteStudent.Text = "Delete Student";
            this.btnDeleteStudent.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDeleteStudent.UseVisualStyleBackColor = true;
            this.btnDeleteStudent.Click += new System.EventHandler(this.btnDeleteStudent_Click);
            // 
            // btnSaveChanges
            // 
            this.btnSaveChanges.Enabled = false;
            this.btnSaveChanges.Image = ((System.Drawing.Image)(resources.GetObject("btnSaveChanges.Image")));
            this.btnSaveChanges.Location = new System.Drawing.Point(330, 419);
            this.btnSaveChanges.Name = "btnSaveChanges";
            this.btnSaveChanges.Size = new System.Drawing.Size(135, 64);
            this.btnSaveChanges.TabIndex = 28;
            this.btnSaveChanges.Text = "Save Changes";
            this.btnSaveChanges.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSaveChanges.UseVisualStyleBackColor = true;
            this.btnSaveChanges.Click += new System.EventHandler(this.btnSaveChanges_Click);
            // 
            // lblHomeDojo
            // 
            this.lblHomeDojo.AutoSize = true;
            this.lblHomeDojo.Location = new System.Drawing.Point(417, 36);
            this.lblHomeDojo.Name = "lblHomeDojo";
            this.lblHomeDojo.Size = new System.Drawing.Size(106, 24);
            this.lblHomeDojo.TabIndex = 27;
            this.lblHomeDojo.Text = "Home Dojo";
            // 
            // txtHomeDojo
            // 
            this.txtHomeDojo.Location = new System.Drawing.Point(421, 63);
            this.txtHomeDojo.Name = "txtHomeDojo";
            this.txtHomeDojo.Size = new System.Drawing.Size(180, 29);
            this.txtHomeDojo.TabIndex = 26;
            // 
            // cbActiveStudent
            // 
            this.cbActiveStudent.AutoSize = true;
            this.cbActiveStudent.Location = new System.Drawing.Point(421, 124);
            this.cbActiveStudent.Name = "cbActiveStudent";
            this.cbActiveStudent.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cbActiveStudent.Size = new System.Drawing.Size(80, 28);
            this.cbActiveStudent.TabIndex = 25;
            this.cbActiveStudent.Text = "Active";
            this.cbActiveStudent.UseVisualStyleBackColor = true;
            // 
            // btnSignInHistory
            // 
            this.btnSignInHistory.Image = ((System.Drawing.Image)(resources.GetObject("btnSignInHistory.Image")));
            this.btnSignInHistory.Location = new System.Drawing.Point(179, 419);
            this.btnSignInHistory.Name = "btnSignInHistory";
            this.btnSignInHistory.Size = new System.Drawing.Size(146, 64);
            this.btnSignInHistory.TabIndex = 24;
            this.btnSignInHistory.Text = "Sign-In History";
            this.btnSignInHistory.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSignInHistory.UseVisualStyleBackColor = true;
            this.btnSignInHistory.Click += new System.EventHandler(this.btnSignInHistory_Click);
            // 
            // btnPromotionHistory
            // 
            this.btnPromotionHistory.Image = ((System.Drawing.Image)(resources.GetObject("btnPromotionHistory.Image")));
            this.btnPromotionHistory.Location = new System.Drawing.Point(17, 417);
            this.btnPromotionHistory.Name = "btnPromotionHistory";
            this.btnPromotionHistory.Size = new System.Drawing.Size(155, 64);
            this.btnPromotionHistory.TabIndex = 7;
            this.btnPromotionHistory.Text = "Promotion History";
            this.btnPromotionHistory.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnPromotionHistory.UseVisualStyleBackColor = true;
            this.btnPromotionHistory.Click += new System.EventHandler(this.btnPromotionHistory_Click);
            // 
            // lblBirthDate
            // 
            this.lblBirthDate.AutoSize = true;
            this.lblBirthDate.Location = new System.Drawing.Point(299, 344);
            this.lblBirthDate.Name = "lblBirthDate";
            this.lblBirthDate.Size = new System.Drawing.Size(90, 24);
            this.lblBirthDate.TabIndex = 23;
            this.lblBirthDate.Text = "Birth Date";
            // 
            // dtBirthdate
            // 
            this.dtBirthdate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtBirthdate.Location = new System.Drawing.Point(303, 371);
            this.dtBirthdate.Name = "dtBirthdate";
            this.dtBirthdate.Size = new System.Drawing.Size(136, 29);
            this.dtBirthdate.TabIndex = 22;
            // 
            // txtSecondaryPhone
            // 
            this.txtSecondaryPhone.Location = new System.Drawing.Point(174, 371);
            this.txtSecondaryPhone.Name = "txtSecondaryPhone";
            this.txtSecondaryPhone.Size = new System.Drawing.Size(123, 29);
            this.txtSecondaryPhone.TabIndex = 21;
            // 
            // lblSecondaryPhone
            // 
            this.lblSecondaryPhone.AutoSize = true;
            this.lblSecondaryPhone.Location = new System.Drawing.Point(171, 344);
            this.lblSecondaryPhone.Name = "lblSecondaryPhone";
            this.lblSecondaryPhone.Size = new System.Drawing.Size(91, 24);
            this.lblSecondaryPhone.TabIndex = 20;
            this.lblSecondaryPhone.Text = "Phone #2";
            // 
            // lblPrimaryPhone
            // 
            this.lblPrimaryPhone.AutoSize = true;
            this.lblPrimaryPhone.Location = new System.Drawing.Point(15, 344);
            this.lblPrimaryPhone.Name = "lblPrimaryPhone";
            this.lblPrimaryPhone.Size = new System.Drawing.Size(134, 24);
            this.lblPrimaryPhone.TabIndex = 19;
            this.lblPrimaryPhone.Text = "Primary Phone";
            // 
            // txtPrimaryPhone
            // 
            this.txtPrimaryPhone.Location = new System.Drawing.Point(19, 371);
            this.txtPrimaryPhone.Name = "txtPrimaryPhone";
            this.txtPrimaryPhone.Size = new System.Drawing.Size(136, 29);
            this.txtPrimaryPhone.TabIndex = 18;
            // 
            // lblEmailAddress
            // 
            this.lblEmailAddress.AutoSize = true;
            this.lblEmailAddress.Location = new System.Drawing.Point(20, 285);
            this.lblEmailAddress.Name = "lblEmailAddress";
            this.lblEmailAddress.Size = new System.Drawing.Size(57, 24);
            this.lblEmailAddress.TabIndex = 17;
            this.lblEmailAddress.Text = "Email";
            // 
            // txtEmailAddress
            // 
            this.txtEmailAddress.Location = new System.Drawing.Point(19, 312);
            this.txtEmailAddress.Name = "txtEmailAddress";
            this.txtEmailAddress.Size = new System.Drawing.Size(370, 29);
            this.txtEmailAddress.TabIndex = 16;
            this.txtEmailAddress.MouseLeave += new System.EventHandler(this.txtEmailAddress_MouseLeave);
            // 
            // gbGender
            // 
            this.gbGender.Controls.Add(this.rbUnknown);
            this.gbGender.Controls.Add(this.rbFemale);
            this.gbGender.Controls.Add(this.rbMale);
            this.gbGender.Location = new System.Drawing.Point(422, 154);
            this.gbGender.Name = "gbGender";
            this.gbGender.Size = new System.Drawing.Size(127, 109);
            this.gbGender.TabIndex = 15;
            this.gbGender.TabStop = false;
            this.gbGender.Text = "Gender";
            // 
            // rbUnknown
            // 
            this.rbUnknown.AutoSize = true;
            this.rbUnknown.Location = new System.Drawing.Point(6, 75);
            this.rbUnknown.Name = "rbUnknown";
            this.rbUnknown.Size = new System.Drawing.Size(108, 28);
            this.rbUnknown.TabIndex = 2;
            this.rbUnknown.TabStop = true;
            this.rbUnknown.Text = "Unknown";
            this.rbUnknown.UseVisualStyleBackColor = true;
            // 
            // rbFemale
            // 
            this.rbFemale.AutoSize = true;
            this.rbFemale.Location = new System.Drawing.Point(6, 50);
            this.rbFemale.Name = "rbFemale";
            this.rbFemale.Size = new System.Drawing.Size(92, 28);
            this.rbFemale.TabIndex = 1;
            this.rbFemale.TabStop = true;
            this.rbFemale.Text = "Female";
            this.rbFemale.UseVisualStyleBackColor = true;
            // 
            // rbMale
            // 
            this.rbMale.AutoSize = true;
            this.rbMale.Location = new System.Drawing.Point(6, 26);
            this.rbMale.Name = "rbMale";
            this.rbMale.Size = new System.Drawing.Size(69, 28);
            this.rbMale.TabIndex = 0;
            this.rbMale.TabStop = true;
            this.rbMale.Text = "Male";
            this.rbMale.UseVisualStyleBackColor = true;
            // 
            // lblZip
            // 
            this.lblZip.AutoSize = true;
            this.lblZip.Location = new System.Drawing.Point(288, 226);
            this.lblZip.Name = "lblZip";
            this.lblZip.Size = new System.Drawing.Size(37, 24);
            this.lblZip.TabIndex = 14;
            this.lblZip.Text = "Zip";
            // 
            // txtZipCode
            // 
            this.txtZipCode.Location = new System.Drawing.Point(283, 253);
            this.txtZipCode.Name = "txtZipCode";
            this.txtZipCode.Size = new System.Drawing.Size(106, 29);
            this.txtZipCode.TabIndex = 13;
            // 
            // lblState
            // 
            this.lblState.AutoSize = true;
            this.lblState.Location = new System.Drawing.Point(205, 226);
            this.lblState.Name = "lblState";
            this.lblState.Size = new System.Drawing.Size(51, 24);
            this.lblState.TabIndex = 12;
            this.lblState.Text = "State";
            // 
            // txtState
            // 
            this.txtState.Location = new System.Drawing.Point(205, 253);
            this.txtState.Name = "txtState";
            this.txtState.Size = new System.Drawing.Size(72, 29);
            this.txtState.TabIndex = 11;
            // 
            // txtCity
            // 
            this.txtCity.Location = new System.Drawing.Point(19, 253);
            this.txtCity.Name = "txtCity";
            this.txtCity.Size = new System.Drawing.Size(180, 29);
            this.txtCity.TabIndex = 10;
            // 
            // lblCity
            // 
            this.lblCity.AutoSize = true;
            this.lblCity.Location = new System.Drawing.Point(23, 226);
            this.lblCity.Name = "lblCity";
            this.lblCity.Size = new System.Drawing.Size(40, 24);
            this.lblCity.TabIndex = 9;
            this.lblCity.Text = "City";
            // 
            // lblAddress2
            // 
            this.lblAddress2.AutoSize = true;
            this.lblAddress2.Location = new System.Drawing.Point(20, 158);
            this.lblAddress2.Name = "lblAddress2";
            this.lblAddress2.Size = new System.Drawing.Size(95, 24);
            this.lblAddress2.TabIndex = 8;
            this.lblAddress2.Text = "Address 2";
            // 
            // lblAddress1
            // 
            this.lblAddress1.AutoSize = true;
            this.lblAddress1.Location = new System.Drawing.Point(23, 95);
            this.lblAddress1.Name = "lblAddress1";
            this.lblAddress1.Size = new System.Drawing.Size(95, 24);
            this.lblAddress1.TabIndex = 7;
            this.lblAddress1.Text = "Address 1";
            // 
            // txtAddress2
            // 
            this.txtAddress2.Location = new System.Drawing.Point(18, 185);
            this.txtAddress2.Name = "txtAddress2";
            this.txtAddress2.Size = new System.Drawing.Size(371, 29);
            this.txtAddress2.TabIndex = 6;
            // 
            // txtAddress1
            // 
            this.txtAddress1.Location = new System.Drawing.Point(17, 122);
            this.txtAddress1.Name = "txtAddress1";
            this.txtAddress1.Size = new System.Drawing.Size(372, 29);
            this.txtAddress1.TabIndex = 5;
            // 
            // lblLastName
            // 
            this.lblLastName.AutoSize = true;
            this.lblLastName.Location = new System.Drawing.Point(212, 36);
            this.lblLastName.Name = "lblLastName";
            this.lblLastName.Size = new System.Drawing.Size(99, 24);
            this.lblLastName.TabIndex = 4;
            this.lblLastName.Text = "Last Name";
            // 
            // lblFirstName
            // 
            this.lblFirstName.AutoSize = true;
            this.lblFirstName.Location = new System.Drawing.Point(20, 36);
            this.lblFirstName.Name = "lblFirstName";
            this.lblFirstName.Size = new System.Drawing.Size(101, 24);
            this.lblFirstName.TabIndex = 3;
            this.lblFirstName.Text = "First Name";
            // 
            // txtLastName
            // 
            this.txtLastName.Location = new System.Drawing.Point(216, 63);
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.Size = new System.Drawing.Size(195, 29);
            this.txtLastName.TabIndex = 2;
            // 
            // lblArtsAndRank
            // 
            this.lblArtsAndRank.AutoSize = true;
            this.lblArtsAndRank.Location = new System.Drawing.Point(27, 387);
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
            this.columnCumulativeHours,
            this.columnPromotionDate,
            this.columnPromotionHours,
            this.columnStudentArtID,
            this.columnLastSignInDate});
            this.lvwArtsAndRanks.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvwArtsAndRanks.FullRowSelect = true;
            this.lvwArtsAndRanks.GridLines = true;
            this.lvwArtsAndRanks.HideSelection = false;
            this.lvwArtsAndRanks.Location = new System.Drawing.Point(13, 33);
            this.lvwArtsAndRanks.MultiSelect = false;
            this.lvwArtsAndRanks.Name = "lvwArtsAndRanks";
            this.lvwArtsAndRanks.Size = new System.Drawing.Size(638, 137);
            this.lvwArtsAndRanks.TabIndex = 6;
            this.lvwArtsAndRanks.UseCompatibleStateImageBehavior = false;
            this.lvwArtsAndRanks.View = System.Windows.Forms.View.Details;
            this.lvwArtsAndRanks.SelectedIndexChanged += new System.EventHandler(this.lvwArtsAndRanks_SelectedIndexChanged);
            // 
            // columnArt
            // 
            this.columnArt.Text = "Art";
            this.columnArt.Width = 91;
            // 
            // columnRank
            // 
            this.columnRank.Text = "Rank";
            this.columnRank.Width = 111;
            // 
            // columnStartDate
            // 
            this.columnStartDate.Text = "Start Date";
            this.columnStartDate.Width = 106;
            // 
            // columnCumulativeHours
            // 
            this.columnCumulativeHours.Text = "Hours in Art";
            // 
            // columnPromotionDate
            // 
            this.columnPromotionDate.Text = "Promotion Date";
            this.columnPromotionDate.Width = 109;
            // 
            // columnPromotionHours
            // 
            this.columnPromotionHours.Text = "Promotion Hours";
            this.columnPromotionHours.Width = 96;
            // 
            // columnStudentArtID
            // 
            this.columnStudentArtID.Width = 0;
            // 
            // columnLastSignInDate
            // 
            this.columnLastSignInDate.Width = 0;
            // 
            // dgvStudentList
            // 
            this.dgvStudentList.AllowUserToAddRows = false;
            this.dgvStudentList.AllowUserToDeleteRows = false;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvStudentList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dgvStudentList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvStudentList.DefaultCellStyle = dataGridViewCellStyle8;
            this.dgvStudentList.Location = new System.Drawing.Point(14, 223);
            this.dgvStudentList.MultiSelect = false;
            this.dgvStudentList.Name = "dgvStudentList";
            this.dgvStudentList.ReadOnly = true;
            this.dgvStudentList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvStudentList.Size = new System.Drawing.Size(445, 309);
            this.dgvStudentList.TabIndex = 7;
            this.dgvStudentList.SelectionChanged += new System.EventHandler(this.dgvStudentList_SelectionChanged);
            // 
            // gbQuickFilters
            // 
            this.gbQuickFilters.Controls.Add(this.lblLastNameFilter);
            this.gbQuickFilters.Controls.Add(this.lblFirstNameFilter);
            this.gbQuickFilters.Controls.Add(this.txtLastNameFilter);
            this.gbQuickFilters.Controls.Add(this.txtFirstNameFilter);
            this.gbQuickFilters.Controls.Add(this.cbNonWindsongStudents);
            this.gbQuickFilters.Controls.Add(this.cbShowInactiveStudents);
            this.gbQuickFilters.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbQuickFilters.Location = new System.Drawing.Point(8, 69);
            this.gbQuickFilters.Name = "gbQuickFilters";
            this.gbQuickFilters.Size = new System.Drawing.Size(457, 129);
            this.gbQuickFilters.TabIndex = 8;
            this.gbQuickFilters.TabStop = false;
            this.gbQuickFilters.Text = "Filters";
            // 
            // lblLastNameFilter
            // 
            this.lblLastNameFilter.AutoSize = true;
            this.lblLastNameFilter.Location = new System.Drawing.Point(212, 31);
            this.lblLastNameFilter.Name = "lblLastNameFilter";
            this.lblLastNameFilter.Size = new System.Drawing.Size(99, 24);
            this.lblLastNameFilter.TabIndex = 5;
            this.lblLastNameFilter.Text = "Last Name";
            // 
            // lblFirstNameFilter
            // 
            this.lblFirstNameFilter.AutoSize = true;
            this.lblFirstNameFilter.Location = new System.Drawing.Point(6, 31);
            this.lblFirstNameFilter.Name = "lblFirstNameFilter";
            this.lblFirstNameFilter.Size = new System.Drawing.Size(101, 24);
            this.lblFirstNameFilter.TabIndex = 4;
            this.lblFirstNameFilter.Text = "First Name";
            // 
            // txtLastNameFilter
            // 
            this.txtLastNameFilter.Location = new System.Drawing.Point(207, 58);
            this.txtLastNameFilter.Name = "txtLastNameFilter";
            this.txtLastNameFilter.Size = new System.Drawing.Size(244, 29);
            this.txtLastNameFilter.TabIndex = 3;
            this.txtLastNameFilter.TextChanged += new System.EventHandler(this.txtLastNameFilter_TextChanged);
            // 
            // txtFirstNameFilter
            // 
            this.txtFirstNameFilter.Location = new System.Drawing.Point(6, 58);
            this.txtFirstNameFilter.Name = "txtFirstNameFilter";
            this.txtFirstNameFilter.Size = new System.Drawing.Size(195, 29);
            this.txtFirstNameFilter.TabIndex = 2;
            this.txtFirstNameFilter.TextChanged += new System.EventHandler(this.txtFirstNameFilter_TextChanged);
            // 
            // cbNonWindsongStudents
            // 
            this.cbNonWindsongStudents.AutoSize = true;
            this.cbNonWindsongStudents.Location = new System.Drawing.Point(216, 91);
            this.cbNonWindsongStudents.Name = "cbNonWindsongStudents";
            this.cbNonWindsongStudents.Size = new System.Drawing.Size(235, 28);
            this.cbNonWindsongStudents.TabIndex = 1;
            this.cbNonWindsongStudents.Text = "Non-Windsong Students";
            this.cbNonWindsongStudents.UseVisualStyleBackColor = true;
            this.cbNonWindsongStudents.CheckedChanged += new System.EventHandler(this.cbNonWindsongStudents_CheckedChanged);
            // 
            // cbShowInactiveStudents
            // 
            this.cbShowInactiveStudents.AutoSize = true;
            this.cbShowInactiveStudents.Location = new System.Drawing.Point(10, 91);
            this.cbShowInactiveStudents.Name = "cbShowInactiveStudents";
            this.cbShowInactiveStudents.Size = new System.Drawing.Size(170, 28);
            this.cbShowInactiveStudents.TabIndex = 0;
            this.cbShowInactiveStudents.Text = "Inactive Students";
            this.cbShowInactiveStudents.UseVisualStyleBackColor = true;
            this.cbShowInactiveStudents.CheckedChanged += new System.EventHandler(this.cbShowInactiveStudents_CheckedChanged);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbSettings,
            this.tsbAddNewStudent,
            this.tsbPromotionSettings,
            this.toolStripButton2,
            this.toolStripButton3});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1120, 55);
            this.toolStrip1.TabIndex = 12;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbSettings
            // 
            this.tsbSettings.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.databasePathToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.tsbSettings.Image = ((System.Drawing.Image)(resources.GetObject("tsbSettings.Image")));
            this.tsbSettings.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbSettings.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSettings.Name = "tsbSettings";
            this.tsbSettings.Size = new System.Drawing.Size(62, 52);
            this.tsbSettings.Text = "Settings";
            this.tsbSettings.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // databasePathToolStripMenuItem
            // 
            this.databasePathToolStripMenuItem.Name = "databasePathToolStripMenuItem";
            this.databasePathToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.databasePathToolStripMenuItem.Text = "Application Settings";
            this.databasePathToolStripMenuItem.Click += new System.EventHandler(this.databasePathToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // tsbAddNewStudent
            // 
            this.tsbAddNewStudent.Image = ((System.Drawing.Image)(resources.GetObject("tsbAddNewStudent.Image")));
            this.tsbAddNewStudent.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbAddNewStudent.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAddNewStudent.Name = "tsbAddNewStudent";
            this.tsbAddNewStudent.Size = new System.Drawing.Size(104, 52);
            this.tsbAddNewStudent.Text = "Add New Student";
            this.tsbAddNewStudent.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbAddNewStudent.ToolTipText = "Add a new student to the database";
            this.tsbAddNewStudent.Click += new System.EventHandler(this.tsbAddNewStudent_Click);
            // 
            // tsbPromotionSettings
            // 
            this.tsbPromotionSettings.Image = ((System.Drawing.Image)(resources.GetObject("tsbPromotionSettings.Image")));
            this.tsbPromotionSettings.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbPromotionSettings.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbPromotionSettings.Name = "tsbPromotionSettings";
            this.tsbPromotionSettings.Size = new System.Drawing.Size(113, 52);
            this.tsbPromotionSettings.Text = "Promotion Settings";
            this.tsbPromotionSettings.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbPromotionSettings.Click += new System.EventHandler(this.tsbPromotionSettings_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.Enabled = false;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(99, 52);
            this.toolStripButton2.Text = "Art Maintenance";
            this.toolStripButton2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.Enabled = false;
            this.toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton3.Image")));
            this.toolStripButton3.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(51, 52);
            this.toolStripButton3.Text = "Reports";
            this.toolStripButton3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // gbArtsAndRank
            // 
            this.gbArtsAndRank.Controls.Add(this.btnRemoveArt);
            this.gbArtsAndRank.Controls.Add(this.btnModifyArt);
            this.gbArtsAndRank.Controls.Add(this.btnAddArt);
            this.gbArtsAndRank.Controls.Add(this.lvwArtsAndRanks);
            this.gbArtsAndRank.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbArtsAndRank.Location = new System.Drawing.Point(12, 565);
            this.gbArtsAndRank.Name = "gbArtsAndRank";
            this.gbArtsAndRank.Size = new System.Drawing.Size(657, 227);
            this.gbArtsAndRank.TabIndex = 13;
            this.gbArtsAndRank.TabStop = false;
            this.gbArtsAndRank.Text = "Arts and Rank";
            // 
            // btnRemoveArt
            // 
            this.btnRemoveArt.Image = ((System.Drawing.Image)(resources.GetObject("btnRemoveArt.Image")));
            this.btnRemoveArt.Location = new System.Drawing.Point(279, 182);
            this.btnRemoveArt.Name = "btnRemoveArt";
            this.btnRemoveArt.Size = new System.Drawing.Size(174, 39);
            this.btnRemoveArt.TabIndex = 9;
            this.btnRemoveArt.Text = "Remove Art";
            this.btnRemoveArt.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnRemoveArt.UseVisualStyleBackColor = true;
            this.btnRemoveArt.Click += new System.EventHandler(this.btnRemoveArt_Click);
            // 
            // btnModifyArt
            // 
            this.btnModifyArt.Image = ((System.Drawing.Image)(resources.GetObject("btnModifyArt.Image")));
            this.btnModifyArt.Location = new System.Drawing.Point(120, 182);
            this.btnModifyArt.Name = "btnModifyArt";
            this.btnModifyArt.Size = new System.Drawing.Size(153, 39);
            this.btnModifyArt.TabIndex = 8;
            this.btnModifyArt.Text = "Modify Art";
            this.btnModifyArt.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnModifyArt.UseVisualStyleBackColor = true;
            this.btnModifyArt.Click += new System.EventHandler(this.btnModifyArt_Click);
            // 
            // btnAddArt
            // 
            this.btnAddArt.Image = ((System.Drawing.Image)(resources.GetObject("btnAddArt.Image")));
            this.btnAddArt.Location = new System.Drawing.Point(10, 182);
            this.btnAddArt.Name = "btnAddArt";
            this.btnAddArt.Size = new System.Drawing.Size(104, 39);
            this.btnAddArt.TabIndex = 7;
            this.btnAddArt.Text = "Add Art";
            this.btnAddArt.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAddArt.UseVisualStyleBackColor = true;
            this.btnAddArt.Click += new System.EventHandler(this.btnAddArt_Click);
            // 
            // lblPromotionEligibility
            // 
            this.lblPromotionEligibility.AutoSize = true;
            this.lblPromotionEligibility.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPromotionEligibility.Location = new System.Drawing.Point(687, 565);
            this.lblPromotionEligibility.Name = "lblPromotionEligibility";
            this.lblPromotionEligibility.Size = new System.Drawing.Size(173, 24);
            this.lblPromotionEligibility.TabIndex = 15;
            this.lblPromotionEligibility.Text = "Promotion Eligibility";
            // 
            // btnPromoteStudent
            // 
            this.btnPromoteStudent.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPromoteStudent.Image = ((System.Drawing.Image)(resources.GetObject("btnPromoteStudent.Image")));
            this.btnPromoteStudent.Location = new System.Drawing.Point(691, 744);
            this.btnPromoteStudent.Name = "btnPromoteStudent";
            this.btnPromoteStudent.Size = new System.Drawing.Size(207, 42);
            this.btnPromoteStudent.TabIndex = 16;
            this.btnPromoteStudent.Text = "Promote Student";
            this.btnPromoteStudent.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnPromoteStudent.UseVisualStyleBackColor = true;
            this.btnPromoteStudent.Click += new System.EventHandler(this.btnPromoteStudent_Click);
            // 
            // richTextPromotionEligibility
            // 
            this.richTextPromotionEligibility.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextPromotionEligibility.Location = new System.Drawing.Point(680, 592);
            this.richTextPromotionEligibility.Name = "richTextPromotionEligibility";
            this.richTextPromotionEligibility.Size = new System.Drawing.Size(428, 143);
            this.richTextPromotionEligibility.TabIndex = 17;
            this.richTextPromotionEligibility.Text = "";
            // 
            // StudentMaintenanceUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1120, 804);
            this.Controls.Add(this.richTextPromotionEligibility);
            this.Controls.Add(this.btnPromoteStudent);
            this.Controls.Add(this.lblPromotionEligibility);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.gbQuickFilters);
            this.Controls.Add(this.dgvStudentList);
            this.Controls.Add(this.lblArtsAndRank);
            this.Controls.Add(this.gbStudentInfo);
            this.Controls.Add(this.gbArtsAndRank);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "StudentMaintenanceUI";
            this.Text = "Student Maintenance";
            this.Load += new System.EventHandler(this.StudentMaintenance_Load);
            this.gbStudentInfo.ResumeLayout(false);
            this.gbStudentInfo.PerformLayout();
            this.gbGender.ResumeLayout(false);
            this.gbGender.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStudentList)).EndInit();
            this.gbQuickFilters.ResumeLayout(false);
            this.gbQuickFilters.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.gbArtsAndRank.ResumeLayout(false);
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
        private System.Windows.Forms.GroupBox gbQuickFilters;
        private System.Windows.Forms.Label lblLastNameFilter;
        private System.Windows.Forms.Label lblFirstNameFilter;
        private System.Windows.Forms.TextBox txtLastNameFilter;
        private System.Windows.Forms.TextBox txtFirstNameFilter;
        private System.Windows.Forms.CheckBox cbNonWindsongStudents;
        private System.Windows.Forms.CheckBox cbShowInactiveStudents;
        private System.Windows.Forms.CheckBox cbActiveStudent;
        private System.Windows.Forms.Label lblHomeDojo;
        private System.Windows.Forms.TextBox txtHomeDojo;
        private System.Windows.Forms.ColumnHeader columnCumulativeHours;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbAddNewStudent;
        private System.Windows.Forms.ToolStripButton tsbPromotionSettings;
        private System.Windows.Forms.GroupBox gbArtsAndRank;
        private System.Windows.Forms.Button btnRemoveArt;
        private System.Windows.Forms.Button btnModifyArt;
        private System.Windows.Forms.Button btnAddArt;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.Button btnSaveChanges;
        private System.Windows.Forms.ToolStripDropDownButton tsbSettings;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem databasePathToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader columnStudentArtID;
        private System.Windows.Forms.ColumnHeader columnLastSignInDate;
        private System.Windows.Forms.Label lblPromotionEligibility;
        private System.Windows.Forms.Button btnPromoteStudent;
        private System.Windows.Forms.Button btnDeleteStudent;
        private System.Windows.Forms.RichTextBox richTextPromotionEligibility;
    }
}

