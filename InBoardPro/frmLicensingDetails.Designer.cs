namespace InBoardPro
{
    partial class frmLicensingDetails
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLicensingDetails));
            this.PictureBoxLogo = new System.Windows.Forms.PictureBox();
            this.lblAbout = new System.Windows.Forms.Label();
            this.gbLicenceDetail = new System.Windows.Forms.GroupBox();
            this.lblActivatioDate = new System.Windows.Forms.Label();
            this.lblLicensedTo = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblProductVer = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBoxLogo)).BeginInit();
            this.gbLicenceDetail.SuspendLayout();
            this.SuspendLayout();
            // 
            // PictureBoxLogo
            // 
            this.PictureBoxLogo.BackColor = System.Drawing.Color.Transparent;
            this.PictureBoxLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.PictureBoxLogo.Image = global::InBoardPro.Properties.Resources.inboard_logo_1;
            this.PictureBoxLogo.Location = new System.Drawing.Point(8, 5);
            this.PictureBoxLogo.Name = "PictureBoxLogo";
            this.PictureBoxLogo.Size = new System.Drawing.Size(365, 74);
            this.PictureBoxLogo.TabIndex = 2;
            this.PictureBoxLogo.TabStop = false;
            // 
            // lblAbout
            // 
            this.lblAbout.AutoSize = true;
            this.lblAbout.Font = new System.Drawing.Font("Trebuchet MS", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAbout.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.lblAbout.Location = new System.Drawing.Point(320, 82);
            this.lblAbout.Name = "lblAbout";
            this.lblAbout.Size = new System.Drawing.Size(153, 24);
            this.lblAbout.TabIndex = 3;
            this.lblAbout.Text = "About Licensing";
            // 
            // gbLicenceDetail
            // 
            this.gbLicenceDetail.Controls.Add(this.lblActivatioDate);
            this.gbLicenceDetail.Controls.Add(this.lblLicensedTo);
            this.gbLicenceDetail.Controls.Add(this.label1);
            this.gbLicenceDetail.Controls.Add(this.lblProductVer);
            this.gbLicenceDetail.Location = new System.Drawing.Point(12, 104);
            this.gbLicenceDetail.Name = "gbLicenceDetail";
            this.gbLicenceDetail.Size = new System.Drawing.Size(467, 152);
            this.gbLicenceDetail.TabIndex = 4;
            this.gbLicenceDetail.TabStop = false;
            // 
            // lblActivatioDate
            // 
            this.lblActivatioDate.AutoSize = true;
            this.lblActivatioDate.Location = new System.Drawing.Point(17, 115);
            this.lblActivatioDate.Name = "lblActivatioDate";
            this.lblActivatioDate.Size = new System.Drawing.Size(83, 13);
            this.lblActivatioDate.TabIndex = 3;
            this.lblActivatioDate.Text = "Activation Date:";
            // 
            // lblLicensedTo
            // 
            this.lblLicensedTo.AutoSize = true;
            this.lblLicensedTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLicensedTo.Location = new System.Drawing.Point(17, 83);
            this.lblLicensedTo.Name = "lblLicensedTo";
            this.lblLicensedTo.Size = new System.Drawing.Size(62, 16);
            this.lblLicensedTo.TabIndex = 2;
            this.lblLicensedTo.Text = "Prabhat";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(133, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "This product is licensed to:";
            // 
            // lblProductVer
            // 
            this.lblProductVer.AutoSize = true;
            this.lblProductVer.Location = new System.Drawing.Point(16, 27);
            this.lblProductVer.Name = "lblProductVer";
            this.lblProductVer.Size = new System.Drawing.Size(85, 13);
            this.lblProductVer.TabIndex = 0;
            this.lblProductVer.Text = "Product Version:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.label2.Location = new System.Drawing.Point(4, 270);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(485, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Copyright © 2011 Linkeddominator. All Rights Reserved.    Support: \"facedominator" +
                "support\" in skype.";
            // 
            // frmLicensingDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(497, 286);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.gbLicenceDetail);
            this.Controls.Add(this.lblAbout);
            this.Controls.Add(this.PictureBoxLogo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmLicensingDetails";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "About InBoardPro Licensing";
            this.Load += new System.EventHandler(this.frmLicensingDetails_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PictureBoxLogo)).EndInit();
            this.gbLicenceDetail.ResumeLayout(false);
            this.gbLicenceDetail.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox PictureBoxLogo;
        private System.Windows.Forms.Label lblAbout;
        private System.Windows.Forms.GroupBox gbLicenceDetail;
        private System.Windows.Forms.Label lblActivatioDate;
        private System.Windows.Forms.Label lblLicensedTo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblProductVer;
        private System.Windows.Forms.Label label2;
    }
}