namespace InBoardPro
{
    partial class frmVersionInfo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmVersionInfo));
            this.lblVerionInfo = new System.Windows.Forms.Label();
            this.grpBoxVersionDetails = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.grpBoxVersionDetails.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblVerionInfo
            // 
            this.lblVerionInfo.AutoSize = true;
            this.lblVerionInfo.BackColor = System.Drawing.Color.Transparent;
            this.lblVerionInfo.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVerionInfo.ForeColor = System.Drawing.Color.Maroon;
            this.lblVerionInfo.Location = new System.Drawing.Point(21, 26);
            this.lblVerionInfo.Name = "lblVerionInfo";
            this.lblVerionInfo.Size = new System.Drawing.Size(118, 13);
            this.lblVerionInfo.TabIndex = 0;
            this.lblVerionInfo.Text = "New Verion In LD";
            // 
            // grpBoxVersionDetails
            // 
            this.grpBoxVersionDetails.BackColor = System.Drawing.Color.Transparent;
            this.grpBoxVersionDetails.Controls.Add(this.lblVerionInfo);
            this.grpBoxVersionDetails.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpBoxVersionDetails.Location = new System.Drawing.Point(28, 26);
            this.grpBoxVersionDetails.Name = "grpBoxVersionDetails";
            this.grpBoxVersionDetails.Size = new System.Drawing.Size(879, 427);
            this.grpBoxVersionDetails.TabIndex = 1;
            this.grpBoxVersionDetails.TabStop = false;
            this.grpBoxVersionDetails.Text = "Latest Changes on this version";
            this.grpBoxVersionDetails.Enter += new System.EventHandler(this.grpBoxVersionDetails_Enter);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(117, 465);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(614, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "* If any suggestion/feedback related to LD Please contact \"socioboard.support\" " +
                "in skype.";
            // 
            // frmVersionInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(946, 494);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.grpBoxVersionDetails);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmVersionInfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Linkedin Version Info of ";
            this.Load += new System.EventHandler(this.frmVersionInfo_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.frmVersionInfo_Paint);
            this.grpBoxVersionDetails.ResumeLayout(false);
            this.grpBoxVersionDetails.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblVerionInfo;
        private System.Windows.Forms.GroupBox grpBoxVersionDetails;
        private System.Windows.Forms.Label label1;
    }
}