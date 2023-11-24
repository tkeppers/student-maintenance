
namespace DojoStudentManagement
{
    partial class StudentArtsAndRankUI
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
            this.txtCumulativeHours = new System.Windows.Forms.TextBox();
            this.lblCumulativeHours = new System.Windows.Forms.Label();
            this.gbModifyArt = new System.Windows.Forms.GroupBox();
            this.gbArt = new System.Windows.Forms.GroupBox();
            this.rdoIaido = new System.Windows.Forms.RadioButton();
            this.rdoJodo = new System.Windows.Forms.RadioButton();
            this.rdoJudo = new System.Windows.Forms.RadioButton();
            this.rdoAikido = new System.Windows.Forms.RadioButton();
            this.lblBeginDate = new System.Windows.Forms.Label();
            this.dtBeginDate = new System.Windows.Forms.DateTimePicker();
            this.gbModifyArt.SuspendLayout();
            this.gbArt.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtCumulativeHours
            // 
            this.txtCumulativeHours.Location = new System.Drawing.Point(244, 45);
            this.txtCumulativeHours.Name = "txtCumulativeHours";
            this.txtCumulativeHours.Size = new System.Drawing.Size(100, 20);
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
            // gbModifyArt
            // 
            this.gbModifyArt.Controls.Add(this.gbArt);
            this.gbModifyArt.Controls.Add(this.lblBeginDate);
            this.gbModifyArt.Controls.Add(this.dtBeginDate);
            this.gbModifyArt.Controls.Add(this.txtCumulativeHours);
            this.gbModifyArt.Controls.Add(this.lblCumulativeHours);
            this.gbModifyArt.Location = new System.Drawing.Point(22, 26);
            this.gbModifyArt.Name = "gbModifyArt";
            this.gbModifyArt.Size = new System.Drawing.Size(554, 146);
            this.gbModifyArt.TabIndex = 2;
            this.gbModifyArt.TabStop = false;
            this.gbModifyArt.Text = "Art Information";
            // 
            // gbArt
            // 
            this.gbArt.Controls.Add(this.rdoIaido);
            this.gbArt.Controls.Add(this.rdoJodo);
            this.gbArt.Controls.Add(this.rdoJudo);
            this.gbArt.Controls.Add(this.rdoAikido);
            this.gbArt.Location = new System.Drawing.Point(6, 19);
            this.gbArt.Name = "gbArt";
            this.gbArt.Size = new System.Drawing.Size(85, 122);
            this.gbArt.TabIndex = 4;
            this.gbArt.TabStop = false;
            this.gbArt.Text = "Art Type";
            // 
            // rdoIaido
            // 
            this.rdoIaido.AutoSize = true;
            this.rdoIaido.Location = new System.Drawing.Point(7, 96);
            this.rdoIaido.Name = "rdoIaido";
            this.rdoIaido.Size = new System.Drawing.Size(48, 17);
            this.rdoIaido.TabIndex = 3;
            this.rdoIaido.TabStop = true;
            this.rdoIaido.Text = "Iaido";
            this.rdoIaido.UseVisualStyleBackColor = true;
            // 
            // rdoJodo
            // 
            this.rdoJodo.AutoSize = true;
            this.rdoJodo.Location = new System.Drawing.Point(7, 73);
            this.rdoJodo.Name = "rdoJodo";
            this.rdoJodo.Size = new System.Drawing.Size(48, 17);
            this.rdoJodo.TabIndex = 2;
            this.rdoJodo.TabStop = true;
            this.rdoJodo.Text = "Jodo";
            this.rdoJodo.UseVisualStyleBackColor = true;
            // 
            // rdoJudo
            // 
            this.rdoJudo.AutoSize = true;
            this.rdoJudo.Location = new System.Drawing.Point(7, 50);
            this.rdoJudo.Name = "rdoJudo";
            this.rdoJudo.Size = new System.Drawing.Size(48, 17);
            this.rdoJudo.TabIndex = 1;
            this.rdoJudo.TabStop = true;
            this.rdoJudo.Text = "Judo";
            this.rdoJudo.UseVisualStyleBackColor = true;
            // 
            // rdoAikido
            // 
            this.rdoAikido.AutoSize = true;
            this.rdoAikido.Location = new System.Drawing.Point(6, 26);
            this.rdoAikido.Name = "rdoAikido";
            this.rdoAikido.Size = new System.Drawing.Size(54, 17);
            this.rdoAikido.TabIndex = 0;
            this.rdoAikido.TabStop = true;
            this.rdoAikido.Text = "Aikido";
            this.rdoAikido.UseVisualStyleBackColor = true;
            // 
            // lblBeginDate
            // 
            this.lblBeginDate.AutoSize = true;
            this.lblBeginDate.Location = new System.Drawing.Point(115, 29);
            this.lblBeginDate.Name = "lblBeginDate";
            this.lblBeginDate.Size = new System.Drawing.Size(60, 13);
            this.lblBeginDate.TabIndex = 3;
            this.lblBeginDate.Text = "Begin Date";
            // 
            // dtBeginDate
            // 
            this.dtBeginDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtBeginDate.Location = new System.Drawing.Point(118, 45);
            this.dtBeginDate.Name = "dtBeginDate";
            this.dtBeginDate.Size = new System.Drawing.Size(100, 20);
            this.dtBeginDate.TabIndex = 2;
            // 
            // StudentArtsAndRankUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.gbModifyArt);
            this.Name = "StudentArtsAndRankUI";
            this.Text = "Modify Arts for <StudentName>";
            this.gbModifyArt.ResumeLayout(false);
            this.gbModifyArt.PerformLayout();
            this.gbArt.ResumeLayout(false);
            this.gbArt.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtCumulativeHours;
        private System.Windows.Forms.Label lblCumulativeHours;
        private System.Windows.Forms.GroupBox gbModifyArt;
        private System.Windows.Forms.DateTimePicker dtBeginDate;
        private System.Windows.Forms.GroupBox gbArt;
        private System.Windows.Forms.RadioButton rdoIaido;
        private System.Windows.Forms.RadioButton rdoJodo;
        private System.Windows.Forms.RadioButton rdoJudo;
        private System.Windows.Forms.RadioButton rdoAikido;
        private System.Windows.Forms.Label lblBeginDate;
    }
}