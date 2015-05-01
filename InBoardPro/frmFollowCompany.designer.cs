namespace InBoardPro
{
    partial class frmFollowCompany
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmFollowCompany));
            this.gbFollowCompanyUrlInputs = new System.Windows.Forms.GroupBox();
            this.txtNumberOfFollowPerAccount = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtCompanyURL = new System.Windows.Forms.TextBox();
            this.lblGrupURL_ScrapGroupMember = new System.Windows.Forms.Label();
            this.BtnUploadCompanyURL = new System.Windows.Forms.Button();
            this.gbFollowCompanyDelaySetting = new System.Windows.Forms.GroupBox();
            this.txtNoOfThread = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label113 = new System.Windows.Forms.Label();
            this.txtFollowCompanyMaxDelay = new System.Windows.Forms.TextBox();
            this.label92 = new System.Windows.Forms.Label();
            this.label93 = new System.Windows.Forms.Label();
            this.txtFollowCompanyMinDelay = new System.Windows.Forms.TextBox();
            this.gbFollowCompanyAction = new System.Windows.Forms.GroupBox();
            this.btnFollowCompany = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.gbFollowCompanyLogger = new System.Windows.Forms.GroupBox();
            this.listLoggerFollowCompany = new System.Windows.Forms.ListBox();
            this.gbFollowCompanyUrlInputs.SuspendLayout();
            this.gbFollowCompanyDelaySetting.SuspendLayout();
            this.gbFollowCompanyAction.SuspendLayout();
            this.gbFollowCompanyLogger.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbFollowCompanyUrlInputs
            // 
            this.gbFollowCompanyUrlInputs.BackColor = System.Drawing.Color.Transparent;
            this.gbFollowCompanyUrlInputs.Controls.Add(this.txtNumberOfFollowPerAccount);
            this.gbFollowCompanyUrlInputs.Controls.Add(this.label1);
            this.gbFollowCompanyUrlInputs.Controls.Add(this.txtCompanyURL);
            this.gbFollowCompanyUrlInputs.Controls.Add(this.lblGrupURL_ScrapGroupMember);
            this.gbFollowCompanyUrlInputs.Controls.Add(this.BtnUploadCompanyURL);
            this.gbFollowCompanyUrlInputs.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbFollowCompanyUrlInputs.Location = new System.Drawing.Point(23, 27);
            this.gbFollowCompanyUrlInputs.Name = "gbFollowCompanyUrlInputs";
            this.gbFollowCompanyUrlInputs.Size = new System.Drawing.Size(775, 101);
            this.gbFollowCompanyUrlInputs.TabIndex = 6;
            this.gbFollowCompanyUrlInputs.TabStop = false;
            this.gbFollowCompanyUrlInputs.Text = "Follow Company Url Inputs";
            // 
            // txtNumberOfFollowPerAccount
            // 
            this.txtNumberOfFollowPerAccount.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNumberOfFollowPerAccount.Location = new System.Drawing.Point(274, 68);
            this.txtNumberOfFollowPerAccount.Name = "txtNumberOfFollowPerAccount";
            this.txtNumberOfFollowPerAccount.Size = new System.Drawing.Size(64, 21);
            this.txtNumberOfFollowPerAccount.TabIndex = 139;
            this.txtNumberOfFollowPerAccount.Text = "1";
            this.txtNumberOfFollowPerAccount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(100, 70);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(173, 13);
            this.label1.TabIndex = 138;
            this.label1.Text = "No of Company Per Account:";
            // 
            // txtCompanyURL
            // 
            this.txtCompanyURL.BackColor = System.Drawing.Color.White;
            this.txtCompanyURL.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCompanyURL.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCompanyURL.Location = new System.Drawing.Point(103, 30);
            this.txtCompanyURL.Name = "txtCompanyURL";
            this.txtCompanyURL.ReadOnly = true;
            this.txtCompanyURL.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtCompanyURL.Size = new System.Drawing.Size(550, 21);
            this.txtCompanyURL.TabIndex = 133;
            // 
            // lblGrupURL_ScrapGroupMember
            // 
            this.lblGrupURL_ScrapGroupMember.AutoSize = true;
            this.lblGrupURL_ScrapGroupMember.BackColor = System.Drawing.Color.Transparent;
            this.lblGrupURL_ScrapGroupMember.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGrupURL_ScrapGroupMember.Location = new System.Drawing.Point(9, 33);
            this.lblGrupURL_ScrapGroupMember.Name = "lblGrupURL_ScrapGroupMember";
            this.lblGrupURL_ScrapGroupMember.Size = new System.Drawing.Size(93, 13);
            this.lblGrupURL_ScrapGroupMember.TabIndex = 132;
            this.lblGrupURL_ScrapGroupMember.Text = "Company URL:";
            // 
            // BtnUploadCompanyURL
            // 
            this.BtnUploadCompanyURL.BackColor = System.Drawing.Color.White;
            this.BtnUploadCompanyURL.BackgroundImage = global::InBoardPro.Properties.Resources.browse_btn;
            this.BtnUploadCompanyURL.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.BtnUploadCompanyURL.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.BtnUploadCompanyURL.ForeColor = System.Drawing.Color.Black;
            this.BtnUploadCompanyURL.Location = new System.Drawing.Point(668, 25);
            this.BtnUploadCompanyURL.Name = "BtnUploadCompanyURL";
            this.BtnUploadCompanyURL.Size = new System.Drawing.Size(75, 29);
            this.BtnUploadCompanyURL.TabIndex = 134;
            this.BtnUploadCompanyURL.UseVisualStyleBackColor = false;
            this.BtnUploadCompanyURL.Click += new System.EventHandler(this.BtnUploadCompanyURL_Click);
            // 
            // gbFollowCompanyDelaySetting
            // 
            this.gbFollowCompanyDelaySetting.BackColor = System.Drawing.Color.Transparent;
            this.gbFollowCompanyDelaySetting.Controls.Add(this.txtNoOfThread);
            this.gbFollowCompanyDelaySetting.Controls.Add(this.label2);
            this.gbFollowCompanyDelaySetting.Controls.Add(this.label113);
            this.gbFollowCompanyDelaySetting.Controls.Add(this.txtFollowCompanyMaxDelay);
            this.gbFollowCompanyDelaySetting.Controls.Add(this.label92);
            this.gbFollowCompanyDelaySetting.Controls.Add(this.label93);
            this.gbFollowCompanyDelaySetting.Controls.Add(this.txtFollowCompanyMinDelay);
            this.gbFollowCompanyDelaySetting.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbFollowCompanyDelaySetting.Location = new System.Drawing.Point(23, 135);
            this.gbFollowCompanyDelaySetting.Name = "gbFollowCompanyDelaySetting";
            this.gbFollowCompanyDelaySetting.Size = new System.Drawing.Size(775, 49);
            this.gbFollowCompanyDelaySetting.TabIndex = 139;
            this.gbFollowCompanyDelaySetting.TabStop = false;
            this.gbFollowCompanyDelaySetting.Text = "Setting";
            // 
            // txtNoOfThread
            // 
            this.txtNoOfThread.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNoOfThread.Location = new System.Drawing.Point(149, 18);
            this.txtNoOfThread.Name = "txtNoOfThread";
            this.txtNoOfThread.Size = new System.Drawing.Size(44, 21);
            this.txtNoOfThread.TabIndex = 128;
            this.txtNoOfThread.Text = "5";
            this.txtNoOfThread.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(57, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 13);
            this.label2.TabIndex = 127;
            this.label2.Text = "No of Thread:";
            // 
            // label113
            // 
            this.label113.AutoSize = true;
            this.label113.BackColor = System.Drawing.Color.Transparent;
            this.label113.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label113.Location = new System.Drawing.Point(433, 21);
            this.label113.Name = "label113";
            this.label113.Size = new System.Drawing.Size(305, 13);
            this.label113.TabIndex = 126;
            this.label113.Text = "Its randomly delay between both values in seconds.";
            // 
            // txtFollowCompanyMaxDelay
            // 
            this.txtFollowCompanyMaxDelay.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFollowCompanyMaxDelay.Location = new System.Drawing.Point(381, 18);
            this.txtFollowCompanyMaxDelay.Name = "txtFollowCompanyMaxDelay";
            this.txtFollowCompanyMaxDelay.Size = new System.Drawing.Size(43, 21);
            this.txtFollowCompanyMaxDelay.TabIndex = 119;
            this.txtFollowCompanyMaxDelay.Text = "25";
            this.txtFollowCompanyMaxDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label92
            // 
            this.label92.AutoSize = true;
            this.label92.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label92.Location = new System.Drawing.Point(252, 21);
            this.label92.Name = "label92";
            this.label92.Size = new System.Drawing.Size(45, 13);
            this.label92.TabIndex = 116;
            this.label92.Text = "Delay:";
            // 
            // label93
            // 
            this.label93.AutoSize = true;
            this.label93.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label93.Location = new System.Drawing.Point(349, 21);
            this.label93.Name = "label93";
            this.label93.Size = new System.Drawing.Size(26, 13);
            this.label93.TabIndex = 117;
            this.label93.Text = "To:";
            // 
            // txtFollowCompanyMinDelay
            // 
            this.txtFollowCompanyMinDelay.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFollowCompanyMinDelay.Location = new System.Drawing.Point(301, 18);
            this.txtFollowCompanyMinDelay.Name = "txtFollowCompanyMinDelay";
            this.txtFollowCompanyMinDelay.Size = new System.Drawing.Size(44, 21);
            this.txtFollowCompanyMinDelay.TabIndex = 118;
            this.txtFollowCompanyMinDelay.Text = "20";
            this.txtFollowCompanyMinDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // gbFollowCompanyAction
            // 
            this.gbFollowCompanyAction.BackColor = System.Drawing.Color.Transparent;
            this.gbFollowCompanyAction.Controls.Add(this.btnFollowCompany);
            this.gbFollowCompanyAction.Controls.Add(this.btnStop);
            this.gbFollowCompanyAction.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbFollowCompanyAction.Location = new System.Drawing.Point(23, 190);
            this.gbFollowCompanyAction.Name = "gbFollowCompanyAction";
            this.gbFollowCompanyAction.Size = new System.Drawing.Size(775, 63);
            this.gbFollowCompanyAction.TabIndex = 140;
            this.gbFollowCompanyAction.TabStop = false;
            this.gbFollowCompanyAction.Text = "Submit Action";
            // 
            // btnFollowCompany
            // 
            this.btnFollowCompany.BackColor = System.Drawing.Color.White;
            this.btnFollowCompany.BackgroundImage = global::InBoardPro.Properties.Resources.follow_company;
            this.btnFollowCompany.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnFollowCompany.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnFollowCompany.Font = new System.Drawing.Font("Cambria", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFollowCompany.Location = new System.Drawing.Point(239, 19);
            this.btnFollowCompany.Name = "btnFollowCompany";
            this.btnFollowCompany.Size = new System.Drawing.Size(146, 31);
            this.btnFollowCompany.TabIndex = 135;
            this.btnFollowCompany.UseVisualStyleBackColor = false;
            this.btnFollowCompany.Click += new System.EventHandler(this.btnFollowCompany_Click);
            // 
            // btnStop
            // 
            this.btnStop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnStop.Image = global::InBoardPro.Properties.Resources.stop;
            this.btnStop.Location = new System.Drawing.Point(408, 20);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(126, 30);
            this.btnStop.TabIndex = 136;
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // gbFollowCompanyLogger
            // 
            this.gbFollowCompanyLogger.BackColor = System.Drawing.Color.Transparent;
            this.gbFollowCompanyLogger.Controls.Add(this.listLoggerFollowCompany);
            this.gbFollowCompanyLogger.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbFollowCompanyLogger.Location = new System.Drawing.Point(23, 259);
            this.gbFollowCompanyLogger.Name = "gbFollowCompanyLogger";
            this.gbFollowCompanyLogger.Size = new System.Drawing.Size(775, 200);
            this.gbFollowCompanyLogger.TabIndex = 141;
            this.gbFollowCompanyLogger.TabStop = false;
            this.gbFollowCompanyLogger.Text = "Logger";
            // 
            // listLoggerFollowCompany
            // 
            this.listLoggerFollowCompany.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listLoggerFollowCompany.FormattingEnabled = true;
            this.listLoggerFollowCompany.Location = new System.Drawing.Point(7, 22);
            this.listLoggerFollowCompany.Name = "listLoggerFollowCompany";
            this.listLoggerFollowCompany.ScrollAlwaysVisible = true;
            this.listLoggerFollowCompany.Size = new System.Drawing.Size(752, 160);
            this.listLoggerFollowCompany.TabIndex = 8;
            // 
            // frmFollowCompany
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(821, 474);
            this.Controls.Add(this.gbFollowCompanyLogger);
            this.Controls.Add(this.gbFollowCompanyAction);
            this.Controls.Add(this.gbFollowCompanyDelaySetting);
            this.Controls.Add(this.gbFollowCompanyUrlInputs);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmFollowCompany";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Follow Company Using URL";
            this.Load += new System.EventHandler(this.frmFollowCompany_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.frmFollowCompany_Paint);
            this.gbFollowCompanyUrlInputs.ResumeLayout(false);
            this.gbFollowCompanyUrlInputs.PerformLayout();
            this.gbFollowCompanyDelaySetting.ResumeLayout(false);
            this.gbFollowCompanyDelaySetting.PerformLayout();
            this.gbFollowCompanyAction.ResumeLayout(false);
            this.gbFollowCompanyLogger.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbFollowCompanyUrlInputs;
        private System.Windows.Forms.TextBox txtNumberOfFollowPerAccount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCompanyURL;
        private System.Windows.Forms.Label lblGrupURL_ScrapGroupMember;
        private System.Windows.Forms.Button BtnUploadCompanyURL;
        private System.Windows.Forms.GroupBox gbFollowCompanyDelaySetting;
        private System.Windows.Forms.Label label113;
        private System.Windows.Forms.TextBox txtFollowCompanyMaxDelay;
        private System.Windows.Forms.Label label92;
        private System.Windows.Forms.Label label93;
        private System.Windows.Forms.TextBox txtFollowCompanyMinDelay;
        private System.Windows.Forms.GroupBox gbFollowCompanyAction;
        private System.Windows.Forms.Button btnFollowCompany;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.GroupBox gbFollowCompanyLogger;
        private System.Windows.Forms.ListBox listLoggerFollowCompany;
        private System.Windows.Forms.TextBox txtNoOfThread;
        private System.Windows.Forms.Label label2;
    }
}