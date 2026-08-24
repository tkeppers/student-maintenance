
namespace DojoStudentManagement
{
    partial class DojoManagementUI
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
            this.dgvDojos = new System.Windows.Forms.DataGridView();
            this.colClubID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colClubName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colClubInstructor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colClubActive = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cbHideInactive = new System.Windows.Forms.CheckBox();
            this.btnAddDojo = new System.Windows.Forms.Button();
            this.btnDeleteDojo = new System.Windows.Forms.Button();
            this.gbDojoDetails = new System.Windows.Forms.GroupBox();
            this.lblClubID = new System.Windows.Forms.Label();
            this.txtClubID = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblInstructor = new System.Windows.Forms.Label();
            this.txtInstructor = new System.Windows.Forms.TextBox();
            this.lblInstructorEmail = new System.Windows.Forms.Label();
            this.txtInstructorEmail = new System.Windows.Forms.TextBox();
            this.lblPhone = new System.Windows.Forms.Label();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.lblAddress1 = new System.Windows.Forms.Label();
            this.txtAddress1 = new System.Windows.Forms.TextBox();
            this.lblAddress2 = new System.Windows.Forms.Label();
            this.txtAddress2 = new System.Windows.Forms.TextBox();
            this.lblAddress3 = new System.Windows.Forms.Label();
            this.txtAddress3 = new System.Windows.Forms.TextBox();
            this.lblAnnualDues = new System.Windows.Forms.Label();
            this.numAnnualDues = new System.Windows.Forms.NumericUpDown();
            this.cbActive = new System.Windows.Forms.CheckBox();
            this.lblNotes = new System.Windows.Forms.Label();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDojos)).BeginInit();
            this.gbDojoDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAnnualDues)).BeginInit();
            this.SuspendLayout();
            //
            // dgvDojos
            //
            this.dgvDojos.AllowUserToAddRows = false;
            this.dgvDojos.AllowUserToDeleteRows = false;
            this.dgvDojos.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDojos.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvDojos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDojos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colClubID,
            this.colClubName,
            this.colClubInstructor,
            this.colClubActive});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvDojos.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvDojos.Location = new System.Drawing.Point(12, 45);
            this.dgvDojos.MultiSelect = false;
            this.dgvDojos.Name = "dgvDojos";
            this.dgvDojos.ReadOnly = true;
            this.dgvDojos.RowHeadersVisible = false;
            this.dgvDojos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDojos.Size = new System.Drawing.Size(520, 545);
            this.dgvDojos.TabIndex = 1;
            this.dgvDojos.SelectionChanged += new System.EventHandler(this.dgvDojos_SelectionChanged);
            //
            // colClubID
            //
            this.colClubID.HeaderText = "Club ID";
            this.colClubID.Name = "colClubID";
            this.colClubID.ReadOnly = true;
            this.colClubID.Width = 110;
            //
            // colClubName
            //
            this.colClubName.HeaderText = "Name";
            this.colClubName.Name = "colClubName";
            this.colClubName.ReadOnly = true;
            this.colClubName.Width = 160;
            //
            // colClubInstructor
            //
            this.colClubInstructor.HeaderText = "Instructor";
            this.colClubInstructor.Name = "colClubInstructor";
            this.colClubInstructor.ReadOnly = true;
            this.colClubInstructor.Width = 160;
            //
            // colClubActive
            //
            this.colClubActive.HeaderText = "Active";
            this.colClubActive.Name = "colClubActive";
            this.colClubActive.ReadOnly = true;
            this.colClubActive.Width = 70;
            //
            // cbHideInactive
            //
            this.cbHideInactive.AutoSize = true;
            this.cbHideInactive.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbHideInactive.Location = new System.Drawing.Point(12, 14);
            this.cbHideInactive.Name = "cbHideInactive";
            this.cbHideInactive.Size = new System.Drawing.Size(178, 22);
            this.cbHideInactive.TabIndex = 0;
            this.cbHideInactive.Text = "Hide inactive dojos";
            this.cbHideInactive.UseVisualStyleBackColor = true;
            this.cbHideInactive.CheckedChanged += new System.EventHandler(this.cbHideInactive_CheckedChanged);
            //
            // btnAddDojo
            //
            this.btnAddDojo.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddDojo.Location = new System.Drawing.Point(272, 8);
            this.btnAddDojo.Name = "btnAddDojo";
            this.btnAddDojo.Size = new System.Drawing.Size(126, 32);
            this.btnAddDojo.TabIndex = 2;
            this.btnAddDojo.Text = "Add Dojo";
            this.btnAddDojo.UseVisualStyleBackColor = true;
            this.btnAddDojo.Click += new System.EventHandler(this.btnAddDojo_Click);
            //
            // btnDeleteDojo
            //
            this.btnDeleteDojo.Enabled = false;
            this.btnDeleteDojo.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteDojo.Location = new System.Drawing.Point(406, 8);
            this.btnDeleteDojo.Name = "btnDeleteDojo";
            this.btnDeleteDojo.Size = new System.Drawing.Size(126, 32);
            this.btnDeleteDojo.TabIndex = 3;
            this.btnDeleteDojo.Text = "Delete Dojo";
            this.btnDeleteDojo.UseVisualStyleBackColor = true;
            this.btnDeleteDojo.Click += new System.EventHandler(this.btnDeleteDojo_Click);
            //
            // gbDojoDetails
            //
            this.gbDojoDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbDojoDetails.Controls.Add(this.lblClubID);
            this.gbDojoDetails.Controls.Add(this.txtClubID);
            this.gbDojoDetails.Controls.Add(this.lblName);
            this.gbDojoDetails.Controls.Add(this.txtName);
            this.gbDojoDetails.Controls.Add(this.lblInstructor);
            this.gbDojoDetails.Controls.Add(this.txtInstructor);
            this.gbDojoDetails.Controls.Add(this.lblInstructorEmail);
            this.gbDojoDetails.Controls.Add(this.txtInstructorEmail);
            this.gbDojoDetails.Controls.Add(this.lblPhone);
            this.gbDojoDetails.Controls.Add(this.txtPhone);
            this.gbDojoDetails.Controls.Add(this.lblAddress1);
            this.gbDojoDetails.Controls.Add(this.txtAddress1);
            this.gbDojoDetails.Controls.Add(this.lblAddress2);
            this.gbDojoDetails.Controls.Add(this.txtAddress2);
            this.gbDojoDetails.Controls.Add(this.lblAddress3);
            this.gbDojoDetails.Controls.Add(this.txtAddress3);
            this.gbDojoDetails.Controls.Add(this.lblAnnualDues);
            this.gbDojoDetails.Controls.Add(this.numAnnualDues);
            this.gbDojoDetails.Controls.Add(this.cbActive);
            this.gbDojoDetails.Controls.Add(this.lblNotes);
            this.gbDojoDetails.Controls.Add(this.txtNotes);
            this.gbDojoDetails.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbDojoDetails.Location = new System.Drawing.Point(548, 45);
            this.gbDojoDetails.Name = "gbDojoDetails";
            this.gbDojoDetails.Size = new System.Drawing.Size(510, 545);
            this.gbDojoDetails.TabIndex = 4;
            this.gbDojoDetails.TabStop = false;
            this.gbDojoDetails.Text = "Dojo Details";
            //
            // lblClubID
            //
            this.lblClubID.AutoSize = true;
            this.lblClubID.Location = new System.Drawing.Point(15, 35);
            this.lblClubID.Name = "lblClubID";
            this.lblClubID.Size = new System.Drawing.Size(62, 18);
            this.lblClubID.TabIndex = 0;
            this.lblClubID.Text = "Club ID:";
            //
            // txtClubID
            //
            this.txtClubID.Location = new System.Drawing.Point(160, 32);
            this.txtClubID.MaxLength = 10;
            this.txtClubID.Name = "txtClubID";
            this.txtClubID.ReadOnly = true;
            this.txtClubID.Size = new System.Drawing.Size(150, 25);
            this.txtClubID.TabIndex = 1;
            this.txtClubID.TextChanged += new System.EventHandler(this.DojoField_Changed);
            //
            // lblName
            //
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(15, 73);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(52, 18);
            this.lblName.TabIndex = 2;
            this.lblName.Text = "Name:";
            //
            // txtName
            //
            this.txtName.Location = new System.Drawing.Point(160, 70);
            this.txtName.MaxLength = 30;
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(330, 25);
            this.txtName.TabIndex = 3;
            this.txtName.TextChanged += new System.EventHandler(this.DojoField_Changed);
            //
            // lblInstructor
            //
            this.lblInstructor.AutoSize = true;
            this.lblInstructor.Location = new System.Drawing.Point(15, 111);
            this.lblInstructor.Name = "lblInstructor";
            this.lblInstructor.Size = new System.Drawing.Size(80, 18);
            this.lblInstructor.TabIndex = 4;
            this.lblInstructor.Text = "Instructor:";
            //
            // txtInstructor
            //
            this.txtInstructor.Location = new System.Drawing.Point(160, 108);
            this.txtInstructor.MaxLength = 100;
            this.txtInstructor.Name = "txtInstructor";
            this.txtInstructor.Size = new System.Drawing.Size(330, 25);
            this.txtInstructor.TabIndex = 5;
            this.txtInstructor.TextChanged += new System.EventHandler(this.DojoField_Changed);
            //
            // lblInstructorEmail
            //
            this.lblInstructorEmail.AutoSize = true;
            this.lblInstructorEmail.Location = new System.Drawing.Point(15, 149);
            this.lblInstructorEmail.Name = "lblInstructorEmail";
            this.lblInstructorEmail.Size = new System.Drawing.Size(126, 18);
            this.lblInstructorEmail.TabIndex = 6;
            this.lblInstructorEmail.Text = "Instructor Email:";
            //
            // txtInstructorEmail
            //
            this.txtInstructorEmail.Location = new System.Drawing.Point(160, 146);
            this.txtInstructorEmail.MaxLength = 100;
            this.txtInstructorEmail.Name = "txtInstructorEmail";
            this.txtInstructorEmail.Size = new System.Drawing.Size(330, 25);
            this.txtInstructorEmail.TabIndex = 7;
            this.txtInstructorEmail.TextChanged += new System.EventHandler(this.DojoField_Changed);
            //
            // lblPhone
            //
            this.lblPhone.AutoSize = true;
            this.lblPhone.Location = new System.Drawing.Point(15, 187);
            this.lblPhone.Name = "lblPhone";
            this.lblPhone.Size = new System.Drawing.Size(56, 18);
            this.lblPhone.TabIndex = 8;
            this.lblPhone.Text = "Phone:";
            //
            // txtPhone
            //
            this.txtPhone.Location = new System.Drawing.Point(160, 184);
            this.txtPhone.MaxLength = 50;
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(330, 25);
            this.txtPhone.TabIndex = 9;
            this.txtPhone.TextChanged += new System.EventHandler(this.DojoField_Changed);
            //
            // lblAddress1
            //
            this.lblAddress1.AutoSize = true;
            this.lblAddress1.Location = new System.Drawing.Point(15, 225);
            this.lblAddress1.Name = "lblAddress1";
            this.lblAddress1.Size = new System.Drawing.Size(80, 18);
            this.lblAddress1.TabIndex = 10;
            this.lblAddress1.Text = "Address 1:";
            //
            // txtAddress1
            //
            this.txtAddress1.Location = new System.Drawing.Point(160, 222);
            this.txtAddress1.MaxLength = 30;
            this.txtAddress1.Name = "txtAddress1";
            this.txtAddress1.Size = new System.Drawing.Size(330, 25);
            this.txtAddress1.TabIndex = 11;
            this.txtAddress1.TextChanged += new System.EventHandler(this.DojoField_Changed);
            //
            // lblAddress2
            //
            this.lblAddress2.AutoSize = true;
            this.lblAddress2.Location = new System.Drawing.Point(15, 263);
            this.lblAddress2.Name = "lblAddress2";
            this.lblAddress2.Size = new System.Drawing.Size(80, 18);
            this.lblAddress2.TabIndex = 12;
            this.lblAddress2.Text = "Address 2:";
            //
            // txtAddress2
            //
            this.txtAddress2.Location = new System.Drawing.Point(160, 260);
            this.txtAddress2.MaxLength = 30;
            this.txtAddress2.Name = "txtAddress2";
            this.txtAddress2.Size = new System.Drawing.Size(330, 25);
            this.txtAddress2.TabIndex = 13;
            this.txtAddress2.TextChanged += new System.EventHandler(this.DojoField_Changed);
            //
            // lblAddress3
            //
            this.lblAddress3.AutoSize = true;
            this.lblAddress3.Location = new System.Drawing.Point(15, 301);
            this.lblAddress3.Name = "lblAddress3";
            this.lblAddress3.Size = new System.Drawing.Size(80, 18);
            this.lblAddress3.TabIndex = 14;
            this.lblAddress3.Text = "Address 3:";
            //
            // txtAddress3
            //
            this.txtAddress3.Location = new System.Drawing.Point(160, 298);
            this.txtAddress3.MaxLength = 25;
            this.txtAddress3.Name = "txtAddress3";
            this.txtAddress3.Size = new System.Drawing.Size(330, 25);
            this.txtAddress3.TabIndex = 15;
            this.txtAddress3.TextChanged += new System.EventHandler(this.DojoField_Changed);
            //
            // lblAnnualDues
            //
            this.lblAnnualDues.AutoSize = true;
            this.lblAnnualDues.Location = new System.Drawing.Point(15, 339);
            this.lblAnnualDues.Name = "lblAnnualDues";
            this.lblAnnualDues.Size = new System.Drawing.Size(101, 18);
            this.lblAnnualDues.TabIndex = 16;
            this.lblAnnualDues.Text = "Annual Dues:";
            //
            // numAnnualDues
            //
            this.numAnnualDues.DecimalPlaces = 2;
            this.numAnnualDues.Location = new System.Drawing.Point(160, 337);
            this.numAnnualDues.Maximum = new decimal(new int[] { 100000, 0, 0, 0});
            this.numAnnualDues.Name = "numAnnualDues";
            this.numAnnualDues.Size = new System.Drawing.Size(150, 25);
            this.numAnnualDues.TabIndex = 17;
            this.numAnnualDues.ValueChanged += new System.EventHandler(this.DojoField_Changed);
            //
            // cbActive
            //
            this.cbActive.AutoSize = true;
            this.cbActive.Location = new System.Drawing.Point(160, 377);
            this.cbActive.Name = "cbActive";
            this.cbActive.Size = new System.Drawing.Size(75, 22);
            this.cbActive.TabIndex = 18;
            this.cbActive.Text = "Active";
            this.cbActive.UseVisualStyleBackColor = true;
            this.cbActive.CheckedChanged += new System.EventHandler(this.DojoField_Changed);
            //
            // lblNotes
            //
            this.lblNotes.AutoSize = true;
            this.lblNotes.Location = new System.Drawing.Point(15, 415);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(53, 18);
            this.lblNotes.TabIndex = 19;
            this.lblNotes.Text = "Notes:";
            //
            // txtNotes
            //
            this.txtNotes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNotes.Location = new System.Drawing.Point(160, 412);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtNotes.Size = new System.Drawing.Size(330, 115);
            this.txtNotes.TabIndex = 20;
            this.txtNotes.TextChanged += new System.EventHandler(this.DojoField_Changed);
            //
            // btnSave
            //
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Enabled = false;
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(856, 600);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(95, 35);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            //
            // btnClose
            //
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(963, 600);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(95, 35);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            //
            // DojoManagementUI
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1070, 647);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.gbDojoDetails);
            this.Controls.Add(this.btnDeleteDojo);
            this.Controls.Add(this.btnAddDojo);
            this.Controls.Add(this.cbHideInactive);
            this.Controls.Add(this.dgvDojos);
            this.MinimizeBox = false;
            this.Name = "DojoManagementUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "KUBK Member Dojos";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DojoManagementUI_FormClosing);
            this.Load += new System.EventHandler(this.DojoManagementUI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDojos)).EndInit();
            this.gbDojoDetails.ResumeLayout(false);
            this.gbDojoDetails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAnnualDues)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvDojos;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClubID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClubName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClubInstructor;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClubActive;
        private System.Windows.Forms.CheckBox cbHideInactive;
        private System.Windows.Forms.Button btnAddDojo;
        private System.Windows.Forms.Button btnDeleteDojo;
        private System.Windows.Forms.GroupBox gbDojoDetails;
        private System.Windows.Forms.Label lblClubID;
        private System.Windows.Forms.TextBox txtClubID;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblInstructor;
        private System.Windows.Forms.TextBox txtInstructor;
        private System.Windows.Forms.Label lblInstructorEmail;
        private System.Windows.Forms.TextBox txtInstructorEmail;
        private System.Windows.Forms.Label lblPhone;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.Label lblAddress1;
        private System.Windows.Forms.TextBox txtAddress1;
        private System.Windows.Forms.Label lblAddress2;
        private System.Windows.Forms.TextBox txtAddress2;
        private System.Windows.Forms.Label lblAddress3;
        private System.Windows.Forms.TextBox txtAddress3;
        private System.Windows.Forms.Label lblAnnualDues;
        private System.Windows.Forms.NumericUpDown numAnnualDues;
        private System.Windows.Forms.CheckBox cbActive;
        private System.Windows.Forms.Label lblNotes;
        private System.Windows.Forms.TextBox txtNotes;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
    }
}
