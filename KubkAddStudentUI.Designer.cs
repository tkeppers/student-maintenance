
namespace DojoStudentManagement
{
    partial class KubkAddStudentUI
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lblFirstNamePrompt = new System.Windows.Forms.Label();
            this.txtFirstName = new System.Windows.Forms.TextBox();
            this.lblLastNamePrompt = new System.Windows.Forms.Label();
            this.txtLastName = new System.Windows.Forms.TextBox();
            this.lblDojoPrompt = new System.Windows.Forms.Label();
            this.cmbDojo = new System.Windows.Forms.ComboBox();
            this.lblEmailPrompt = new System.Windows.Forms.Label();
            this.txtEmailAddress = new System.Windows.Forms.TextBox();
            this.lblPhonePrompt = new System.Windows.Forms.Label();
            this.txtPhoneNumber = new System.Windows.Forms.TextBox();
            this.cbActiveStudent = new System.Windows.Forms.CheckBox();
            this.lblRankHeader = new System.Windows.Forms.Label();
            this.lblArtPrompt = new System.Windows.Forms.Label();
            this.cmbArt = new System.Windows.Forms.ComboBox();
            this.lblRankPrompt = new System.Windows.Forms.Label();
            this.cmbRank = new System.Windows.Forms.ComboBox();
            this.lblRankSincePrompt = new System.Windows.Forms.Label();
            this.dtRankHeldSince = new System.Windows.Forms.DateTimePicker();
            this.lblExplanation = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // lblFirstNamePrompt
            //
            this.lblFirstNamePrompt.AutoSize = true;
            this.lblFirstNamePrompt.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFirstNamePrompt.Location = new System.Drawing.Point(18, 20);
            this.lblFirstNamePrompt.Name = "lblFirstNamePrompt";
            this.lblFirstNamePrompt.Size = new System.Drawing.Size(88, 18);
            this.lblFirstNamePrompt.TabIndex = 0;
            this.lblFirstNamePrompt.Text = "First Name:";
            //
            // txtFirstName
            //
            this.txtFirstName.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFirstName.Location = new System.Drawing.Point(175, 17);
            // Matches Students.stud_firstname TEXT(15); a longer value fails the insert.
            this.txtFirstName.MaxLength = 15;
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new System.Drawing.Size(270, 25);
            this.txtFirstName.TabIndex = 1;
            //
            // lblLastNamePrompt
            //
            this.lblLastNamePrompt.AutoSize = true;
            this.lblLastNamePrompt.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLastNamePrompt.Location = new System.Drawing.Point(18, 56);
            this.lblLastNamePrompt.Name = "lblLastNamePrompt";
            this.lblLastNamePrompt.Size = new System.Drawing.Size(86, 18);
            this.lblLastNamePrompt.TabIndex = 2;
            this.lblLastNamePrompt.Text = "Last Name:";
            //
            // txtLastName
            //
            this.txtLastName.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLastName.Location = new System.Drawing.Point(175, 53);
            // Matches Students.stud_lastname TEXT(20).
            this.txtLastName.MaxLength = 20;
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.Size = new System.Drawing.Size(270, 25);
            this.txtLastName.TabIndex = 3;
            //
            // lblDojoPrompt
            //
            this.lblDojoPrompt.AutoSize = true;
            this.lblDojoPrompt.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDojoPrompt.Location = new System.Drawing.Point(18, 92);
            this.lblDojoPrompt.Name = "lblDojoPrompt";
            this.lblDojoPrompt.Size = new System.Drawing.Size(48, 18);
            this.lblDojoPrompt.TabIndex = 4;
            this.lblDojoPrompt.Text = "Dojo:";
            //
            // cmbDojo
            //
            this.cmbDojo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDojo.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbDojo.Location = new System.Drawing.Point(175, 89);
            this.cmbDojo.Name = "cmbDojo";
            this.cmbDojo.Size = new System.Drawing.Size(270, 26);
            this.cmbDojo.TabIndex = 5;
            //
            // lblEmailPrompt
            //
            this.lblEmailPrompt.AutoSize = true;
            this.lblEmailPrompt.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmailPrompt.Location = new System.Drawing.Point(18, 128);
            this.lblEmailPrompt.Name = "lblEmailPrompt";
            this.lblEmailPrompt.Size = new System.Drawing.Size(52, 18);
            this.lblEmailPrompt.TabIndex = 6;
            this.lblEmailPrompt.Text = "Email:";
            //
            // txtEmailAddress
            //
            this.txtEmailAddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEmailAddress.Location = new System.Drawing.Point(175, 125);
            // Matches Students.stud_email TEXT(70).
            this.txtEmailAddress.MaxLength = 70;
            this.txtEmailAddress.Name = "txtEmailAddress";
            this.txtEmailAddress.Size = new System.Drawing.Size(270, 25);
            this.txtEmailAddress.TabIndex = 7;
            //
            // lblPhonePrompt
            //
            this.lblPhonePrompt.AutoSize = true;
            this.lblPhonePrompt.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPhonePrompt.Location = new System.Drawing.Point(18, 164);
            this.lblPhonePrompt.Name = "lblPhonePrompt";
            this.lblPhonePrompt.Size = new System.Drawing.Size(58, 18);
            this.lblPhonePrompt.TabIndex = 8;
            this.lblPhonePrompt.Text = "Phone:";
            //
            // txtPhoneNumber
            //
            this.txtPhoneNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPhoneNumber.Location = new System.Drawing.Point(175, 161);
            // Matches Students.stud_homephone TEXT(17).
            this.txtPhoneNumber.MaxLength = 17;
            this.txtPhoneNumber.Name = "txtPhoneNumber";
            this.txtPhoneNumber.Size = new System.Drawing.Size(270, 25);
            this.txtPhoneNumber.TabIndex = 9;
            //
            // cbActiveStudent
            //
            this.cbActiveStudent.AutoSize = true;
            this.cbActiveStudent.Checked = true;
            this.cbActiveStudent.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbActiveStudent.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbActiveStudent.Location = new System.Drawing.Point(175, 197);
            this.cbActiveStudent.Name = "cbActiveStudent";
            this.cbActiveStudent.Size = new System.Drawing.Size(135, 22);
            this.cbActiveStudent.TabIndex = 10;
            this.cbActiveStudent.Text = "Active student";
            this.cbActiveStudent.UseVisualStyleBackColor = true;
            //
            // lblRankHeader
            //
            this.lblRankHeader.AutoSize = true;
            this.lblRankHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRankHeader.Location = new System.Drawing.Point(18, 233);
            this.lblRankHeader.Name = "lblRankHeader";
            this.lblRankHeader.Size = new System.Drawing.Size(147, 18);
            this.lblRankHeader.TabIndex = 11;
            this.lblRankHeader.Text = "Rank on joining";
            //
            // lblArtPrompt
            //
            this.lblArtPrompt.AutoSize = true;
            this.lblArtPrompt.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblArtPrompt.Location = new System.Drawing.Point(18, 268);
            this.lblArtPrompt.Name = "lblArtPrompt";
            this.lblArtPrompt.Size = new System.Drawing.Size(35, 18);
            this.lblArtPrompt.TabIndex = 12;
            this.lblArtPrompt.Text = "Art:";
            //
            // cmbArt
            //
            this.cmbArt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbArt.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbArt.Location = new System.Drawing.Point(175, 265);
            this.cmbArt.Name = "cmbArt";
            this.cmbArt.Size = new System.Drawing.Size(270, 26);
            this.cmbArt.TabIndex = 13;
            //
            // lblRankPrompt
            //
            this.lblRankPrompt.AutoSize = true;
            this.lblRankPrompt.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRankPrompt.Location = new System.Drawing.Point(18, 304);
            this.lblRankPrompt.Name = "lblRankPrompt";
            this.lblRankPrompt.Size = new System.Drawing.Size(109, 18);
            this.lblRankPrompt.TabIndex = 14;
            this.lblRankPrompt.Text = "Current Rank:";
            //
            // cmbRank
            //
            this.cmbRank.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRank.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbRank.Location = new System.Drawing.Point(175, 301);
            this.cmbRank.Name = "cmbRank";
            this.cmbRank.Size = new System.Drawing.Size(270, 26);
            this.cmbRank.TabIndex = 15;
            //
            // lblRankSincePrompt
            //
            this.lblRankSincePrompt.AutoSize = true;
            this.lblRankSincePrompt.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRankSincePrompt.Location = new System.Drawing.Point(18, 340);
            this.lblRankSincePrompt.Name = "lblRankSincePrompt";
            this.lblRankSincePrompt.Size = new System.Drawing.Size(151, 18);
            this.lblRankSincePrompt.TabIndex = 16;
            this.lblRankSincePrompt.Text = "Held Since:";
            //
            // dtRankHeldSince
            //
            this.dtRankHeldSince.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtRankHeldSince.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtRankHeldSince.Location = new System.Drawing.Point(175, 337);
            this.dtRankHeldSince.Name = "dtRankHeldSince";
            this.dtRankHeldSince.Size = new System.Drawing.Size(270, 25);
            this.dtRankHeldSince.TabIndex = 17;
            //
            // lblExplanation
            //
            this.lblExplanation.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExplanation.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblExplanation.Location = new System.Drawing.Point(21, 378);
            this.lblExplanation.Name = "lblExplanation";
            this.lblExplanation.Size = new System.Drawing.Size(424, 52);
            this.lblExplanation.TabIndex = 18;
            this.lblExplanation.Text = "The rank recorded here is what the home dojo reports. It is saved unverified, so t" +
                "he student appears on the roster awaiting confirmation. Further arts are added wh" +
                "en the student is first promoted in them.";
            //
            // btnSave
            //
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(225, 442);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(105, 35);
            this.btnSave.TabIndex = 19;
            this.btnSave.Text = "Add";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            //
            // btnCancel
            //
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(340, 442);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(105, 35);
            this.btnCancel.TabIndex = 20;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            //
            // KubkAddStudentUI
            //
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(470, 492);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.lblExplanation);
            this.Controls.Add(this.dtRankHeldSince);
            this.Controls.Add(this.lblRankSincePrompt);
            this.Controls.Add(this.cmbRank);
            this.Controls.Add(this.lblRankPrompt);
            this.Controls.Add(this.cmbArt);
            this.Controls.Add(this.lblArtPrompt);
            this.Controls.Add(this.lblRankHeader);
            this.Controls.Add(this.cbActiveStudent);
            this.Controls.Add(this.txtPhoneNumber);
            this.Controls.Add(this.lblPhonePrompt);
            this.Controls.Add(this.txtEmailAddress);
            this.Controls.Add(this.lblEmailPrompt);
            this.Controls.Add(this.cmbDojo);
            this.Controls.Add(this.lblDojoPrompt);
            this.Controls.Add(this.txtLastName);
            this.Controls.Add(this.lblLastNamePrompt);
            this.Controls.Add(this.txtFirstName);
            this.Controls.Add(this.lblFirstNamePrompt);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "KubkAddStudentUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add Student to Member Dojo";
            this.Load += new System.EventHandler(this.KubkAddStudentUI_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblFirstNamePrompt;
        private System.Windows.Forms.TextBox txtFirstName;
        private System.Windows.Forms.Label lblLastNamePrompt;
        private System.Windows.Forms.TextBox txtLastName;
        private System.Windows.Forms.Label lblDojoPrompt;
        private System.Windows.Forms.ComboBox cmbDojo;
        private System.Windows.Forms.Label lblEmailPrompt;
        private System.Windows.Forms.TextBox txtEmailAddress;
        private System.Windows.Forms.Label lblPhonePrompt;
        private System.Windows.Forms.TextBox txtPhoneNumber;
        private System.Windows.Forms.CheckBox cbActiveStudent;
        private System.Windows.Forms.Label lblRankHeader;
        private System.Windows.Forms.Label lblArtPrompt;
        private System.Windows.Forms.ComboBox cmbArt;
        private System.Windows.Forms.Label lblRankPrompt;
        private System.Windows.Forms.ComboBox cmbRank;
        private System.Windows.Forms.Label lblRankSincePrompt;
        private System.Windows.Forms.DateTimePicker dtRankHeldSince;
        private System.Windows.Forms.Label lblExplanation;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
    }
}
