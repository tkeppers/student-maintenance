
namespace DojoStudentManagement
{
    partial class RankCorrectionUI
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
            this.lblStudent = new System.Windows.Forms.Label();
            this.lblArtPrompt = new System.Windows.Forms.Label();
            this.lblArt = new System.Windows.Forms.Label();
            this.lblCurrentRankPrompt = new System.Windows.Forms.Label();
            this.lblCurrentRank = new System.Windows.Forms.Label();
            this.lblNewRankPrompt = new System.Windows.Forms.Label();
            this.cmbNewRank = new System.Windows.Forms.ComboBox();
            this.lblExplanation = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // lblStudent
            //
            this.lblStudent.AutoSize = true;
            this.lblStudent.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStudent.Location = new System.Drawing.Point(12, 15);
            this.lblStudent.Name = "lblStudent";
            this.lblStudent.Size = new System.Drawing.Size(0, 18);
            this.lblStudent.TabIndex = 0;
            //
            // lblArtPrompt
            //
            this.lblArtPrompt.AutoSize = true;
            this.lblArtPrompt.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblArtPrompt.Location = new System.Drawing.Point(12, 52);
            this.lblArtPrompt.Name = "lblArtPrompt";
            this.lblArtPrompt.Size = new System.Drawing.Size(35, 18);
            this.lblArtPrompt.TabIndex = 1;
            this.lblArtPrompt.Text = "Art:";
            //
            // lblArt
            //
            this.lblArt.AutoSize = true;
            this.lblArt.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblArt.Location = new System.Drawing.Point(170, 52);
            this.lblArt.Name = "lblArt";
            this.lblArt.Size = new System.Drawing.Size(0, 18);
            this.lblArt.TabIndex = 2;
            //
            // lblCurrentRankPrompt
            //
            this.lblCurrentRankPrompt.AutoSize = true;
            this.lblCurrentRankPrompt.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentRankPrompt.Location = new System.Drawing.Point(12, 84);
            this.lblCurrentRankPrompt.Name = "lblCurrentRankPrompt";
            this.lblCurrentRankPrompt.Size = new System.Drawing.Size(112, 18);
            this.lblCurrentRankPrompt.TabIndex = 3;
            this.lblCurrentRankPrompt.Text = "Recorded Rank:";
            //
            // lblCurrentRank
            //
            this.lblCurrentRank.AutoSize = true;
            this.lblCurrentRank.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentRank.Location = new System.Drawing.Point(170, 84);
            this.lblCurrentRank.Name = "lblCurrentRank";
            this.lblCurrentRank.Size = new System.Drawing.Size(0, 18);
            this.lblCurrentRank.TabIndex = 4;
            //
            // lblNewRankPrompt
            //
            this.lblNewRankPrompt.AutoSize = true;
            this.lblNewRankPrompt.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNewRankPrompt.Location = new System.Drawing.Point(12, 120);
            this.lblNewRankPrompt.Name = "lblNewRankPrompt";
            this.lblNewRankPrompt.Size = new System.Drawing.Size(96, 18);
            this.lblNewRankPrompt.TabIndex = 5;
            this.lblNewRankPrompt.Text = "Actual Rank:";
            //
            // cmbNewRank
            //
            this.cmbNewRank.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbNewRank.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbNewRank.Location = new System.Drawing.Point(170, 117);
            this.cmbNewRank.Name = "cmbNewRank";
            this.cmbNewRank.Size = new System.Drawing.Size(220, 26);
            this.cmbNewRank.TabIndex = 6;
            //
            // lblExplanation
            //
            this.lblExplanation.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExplanation.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblExplanation.Location = new System.Drawing.Point(15, 162);
            this.lblExplanation.Name = "lblExplanation";
            this.lblExplanation.Size = new System.Drawing.Size(375, 60);
            this.lblExplanation.TabIndex = 7;
            this.lblExplanation.Text = "This is an administrative correction. It updates the recorded rank and marks it ve" +
                "rified today, without creating a promotion record. To record an actual promotion," +
                " use Promote Student instead.";
            //
            // btnOK
            //
            this.btnOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.Location = new System.Drawing.Point(170, 232);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(105, 35);
            this.btnOK.TabIndex = 8;
            this.btnOK.Text = "Correct";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            //
            // btnCancel
            //
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(285, 232);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(105, 35);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            //
            // RankCorrectionUI
            //
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(404, 284);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblExplanation);
            this.Controls.Add(this.cmbNewRank);
            this.Controls.Add(this.lblNewRankPrompt);
            this.Controls.Add(this.lblCurrentRank);
            this.Controls.Add(this.lblCurrentRankPrompt);
            this.Controls.Add(this.lblArt);
            this.Controls.Add(this.lblArtPrompt);
            this.Controls.Add(this.lblStudent);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RankCorrectionUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Correct Recorded Rank";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblStudent;
        private System.Windows.Forms.Label lblArtPrompt;
        private System.Windows.Forms.Label lblArt;
        private System.Windows.Forms.Label lblCurrentRankPrompt;
        private System.Windows.Forms.Label lblCurrentRank;
        private System.Windows.Forms.Label lblNewRankPrompt;
        private System.Windows.Forms.ComboBox cmbNewRank;
        private System.Windows.Forms.Label lblExplanation;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}
