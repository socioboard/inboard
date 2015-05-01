namespace LinkedinDominator
{
    partial class frmMessageReceivedScraper
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
            this.label1 = new System.Windows.Forms.Label();
            this.cmbAllUser = new System.Windows.Forms.ComboBox();
            this.gbJoinGroupUrlDelaySetting = new System.Windows.Forms.GroupBox();
            this.txtMessageReceivedScraperNoOfThread = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label113 = new System.Windows.Forms.Label();
            this.txtMessageReceivedScraperMaxDelay = new System.Windows.Forms.TextBox();
            this.label92 = new System.Windows.Forms.Label();
            this.label93 = new System.Windows.Forms.Label();
            this.txtMessageReceivedScraperMinDelay = new System.Windows.Forms.TextBox();
            this.gbFriendsGroupLogger = new System.Windows.Forms.GroupBox();
            this.lstLogMessageReceivedScraper = new System.Windows.Forms.ListBox();
            this.gbMultiExlInputAction = new System.Windows.Forms.GroupBox();
            this.btnStartMessageReceivedScraper = new System.Windows.Forms.Button();
            this.btnStopMessageReceivedScraper = new System.Windows.Forms.Button();
            this.gbJoinGroupUrlDelaySetting.SuspendLayout();
            this.gbFriendsGroupLogger.SuspendLayout();
            this.gbMultiExlInputAction.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(34, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 142;
            this.label1.Text = "User Account:";
            // 
            // cmbAllUser
            // 
            this.cmbAllUser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAllUser.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbAllUser.FormattingEnabled = true;
            this.cmbAllUser.Location = new System.Drawing.Point(148, 11);
            this.cmbAllUser.Margin = new System.Windows.Forms.Padding(2);
            this.cmbAllUser.Name = "cmbAllUser";
            this.cmbAllUser.Size = new System.Drawing.Size(306, 21);
            this.cmbAllUser.TabIndex = 141;
            // 
            // gbJoinGroupUrlDelaySetting
            // 
            this.gbJoinGroupUrlDelaySetting.BackColor = System.Drawing.Color.Transparent;
            this.gbJoinGroupUrlDelaySetting.Controls.Add(this.txtMessageReceivedScraperNoOfThread);
            this.gbJoinGroupUrlDelaySetting.Controls.Add(this.label2);
            this.gbJoinGroupUrlDelaySetting.Controls.Add(this.label113);
            this.gbJoinGroupUrlDelaySetting.Controls.Add(this.txtMessageReceivedScraperMaxDelay);
            this.gbJoinGroupUrlDelaySetting.Controls.Add(this.label92);
            this.gbJoinGroupUrlDelaySetting.Controls.Add(this.label93);
            this.gbJoinGroupUrlDelaySetting.Controls.Add(this.txtMessageReceivedScraperMinDelay);
            this.gbJoinGroupUrlDelaySetting.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbJoinGroupUrlDelaySetting.Location = new System.Drawing.Point(37, 71);
            this.gbJoinGroupUrlDelaySetting.Name = "gbJoinGroupUrlDelaySetting";
            this.gbJoinGroupUrlDelaySetting.Size = new System.Drawing.Size(733, 49);
            this.gbJoinGroupUrlDelaySetting.TabIndex = 144;
            this.gbJoinGroupUrlDelaySetting.TabStop = false;
            this.gbJoinGroupUrlDelaySetting.Text = "Setting";
            // 
            // txtMessageReceivedScraperNoOfThread
            // 
            this.txtMessageReceivedScraperNoOfThread.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMessageReceivedScraperNoOfThread.Location = new System.Drawing.Point(152, 19);
            this.txtMessageReceivedScraperNoOfThread.Name = "txtMessageReceivedScraperNoOfThread";
            this.txtMessageReceivedScraperNoOfThread.Size = new System.Drawing.Size(44, 21);
            this.txtMessageReceivedScraperNoOfThread.TabIndex = 130;
            this.txtMessageReceivedScraperNoOfThread.Text = "5";
            this.txtMessageReceivedScraperNoOfThread.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(60, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 13);
            this.label2.TabIndex = 129;
            this.label2.Text = "No of Thread:";
            // 
            // label113
            // 
            this.label113.AutoSize = true;
            this.label113.BackColor = System.Drawing.Color.Transparent;
            this.label113.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label113.Location = new System.Drawing.Point(392, 21);
            this.label113.Name = "label113";
            this.label113.Size = new System.Drawing.Size(285, 13);
            this.label113.TabIndex = 126;
            this.label113.Text = "(Random delay between both values in seconds)";
            // 
            // txtMessageReceivedScraperMaxDelay
            // 
            this.txtMessageReceivedScraperMaxDelay.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMessageReceivedScraperMaxDelay.Location = new System.Drawing.Point(343, 18);
            this.txtMessageReceivedScraperMaxDelay.Name = "txtMessageReceivedScraperMaxDelay";
            this.txtMessageReceivedScraperMaxDelay.Size = new System.Drawing.Size(43, 21);
            this.txtMessageReceivedScraperMaxDelay.TabIndex = 119;
            this.txtMessageReceivedScraperMaxDelay.Text = "25";
            this.txtMessageReceivedScraperMaxDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label92
            // 
            this.label92.AutoSize = true;
            this.label92.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label92.Location = new System.Drawing.Point(212, 22);
            this.label92.Name = "label92";
            this.label92.Size = new System.Drawing.Size(45, 13);
            this.label92.TabIndex = 116;
            this.label92.Text = "Delay:";
            // 
            // label93
            // 
            this.label93.AutoSize = true;
            this.label93.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label93.Location = new System.Drawing.Point(314, 21);
            this.label93.Name = "label93";
            this.label93.Size = new System.Drawing.Size(26, 13);
            this.label93.TabIndex = 117;
            this.label93.Text = "To:";
            // 
            // txtMessageReceivedScraperMinDelay
            // 
            this.txtMessageReceivedScraperMinDelay.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMessageReceivedScraperMinDelay.Location = new System.Drawing.Point(261, 18);
            this.txtMessageReceivedScraperMinDelay.Name = "txtMessageReceivedScraperMinDelay";
            this.txtMessageReceivedScraperMinDelay.Size = new System.Drawing.Size(44, 21);
            this.txtMessageReceivedScraperMinDelay.TabIndex = 118;
            this.txtMessageReceivedScraperMinDelay.Text = "20";
            this.txtMessageReceivedScraperMinDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // gbFriendsGroupLogger
            // 
            this.gbFriendsGroupLogger.BackColor = System.Drawing.Color.Transparent;
            this.gbFriendsGroupLogger.Controls.Add(this.lstLogMessageReceivedScraper);
            this.gbFriendsGroupLogger.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbFriendsGroupLogger.Location = new System.Drawing.Point(37, 212);
            this.gbFriendsGroupLogger.Name = "gbFriendsGroupLogger";
            this.gbFriendsGroupLogger.Size = new System.Drawing.Size(733, 240);
            this.gbFriendsGroupLogger.TabIndex = 131;
            this.gbFriendsGroupLogger.TabStop = false;
            this.gbFriendsGroupLogger.Text = "Logger";
            // 
            // lstLogMessageReceivedScraper
            // 
            this.lstLogMessageReceivedScraper.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstLogMessageReceivedScraper.FormattingEnabled = true;
            this.lstLogMessageReceivedScraper.HorizontalScrollbar = true;
            this.lstLogMessageReceivedScraper.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.lstLogMessageReceivedScraper.Location = new System.Drawing.Point(19, 21);
            this.lstLogMessageReceivedScraper.Name = "lstLogMessageReceivedScraper";
            this.lstLogMessageReceivedScraper.ScrollAlwaysVisible = true;
            this.lstLogMessageReceivedScraper.Size = new System.Drawing.Size(701, 199);
            this.lstLogMessageReceivedScraper.TabIndex = 8;
            // 
            // gbMultiExlInputAction
            // 
            this.gbMultiExlInputAction.BackColor = System.Drawing.Color.Transparent;
            this.gbMultiExlInputAction.Controls.Add(this.btnStartMessageReceivedScraper);
            this.gbMultiExlInputAction.Controls.Add(this.btnStopMessageReceivedScraper);
            this.gbMultiExlInputAction.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbMultiExlInputAction.Location = new System.Drawing.Point(37, 135);
            this.gbMultiExlInputAction.Name = "gbMultiExlInputAction";
            this.gbMultiExlInputAction.Size = new System.Drawing.Size(733, 71);
            this.gbMultiExlInputAction.TabIndex = 145;
            this.gbMultiExlInputAction.TabStop = false;
            this.gbMultiExlInputAction.Text = "Submit Action";
            // 
            // btnStartMessageReceivedScraper
            // 
            this.btnStartMessageReceivedScraper.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnStartMessageReceivedScraper.Image = global::LinkedinDominator.Properties.Resources.start;
            this.btnStartMessageReceivedScraper.Location = new System.Drawing.Point(182, 20);
            this.btnStartMessageReceivedScraper.Name = "btnStartMessageReceivedScraper";
            this.btnStartMessageReceivedScraper.Size = new System.Drawing.Size(123, 30);
            this.btnStartMessageReceivedScraper.TabIndex = 160;
            this.btnStartMessageReceivedScraper.UseVisualStyleBackColor = true;
            this.btnStartMessageReceivedScraper.Click += new System.EventHandler(this.btnStartMessageReceivedScraper_Click);
            // 
            // btnStopMessageReceivedScraper
            // 
            this.btnStopMessageReceivedScraper.BackgroundImage = global::LinkedinDominator.Properties.Resources.stop;
            this.btnStopMessageReceivedScraper.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnStopMessageReceivedScraper.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnStopMessageReceivedScraper.Location = new System.Drawing.Point(419, 19);
            this.btnStopMessageReceivedScraper.Name = "btnStopMessageReceivedScraper";
            this.btnStopMessageReceivedScraper.Size = new System.Drawing.Size(115, 31);
            this.btnStopMessageReceivedScraper.TabIndex = 159;
            this.btnStopMessageReceivedScraper.UseVisualStyleBackColor = true;
            // 
            // frmMessageReceivedScraper
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(830, 464);
            this.Controls.Add(this.gbMultiExlInputAction);
            this.Controls.Add(this.gbFriendsGroupLogger);
            this.Controls.Add(this.gbJoinGroupUrlDelaySetting);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbAllUser);
            this.Name = "frmMessageReceivedScraper";
            this.Text = "frmMessageReceivedScraper";
            this.Load += new System.EventHandler(this.frmMessageReceivedScraper_Load);
            this.gbJoinGroupUrlDelaySetting.ResumeLayout(false);
            this.gbJoinGroupUrlDelaySetting.PerformLayout();
            this.gbFriendsGroupLogger.ResumeLayout(false);
            this.gbMultiExlInputAction.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbAllUser;
        private System.Windows.Forms.GroupBox gbJoinGroupUrlDelaySetting;
        private System.Windows.Forms.TextBox txtMessageReceivedScraperNoOfThread;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label113;
        private System.Windows.Forms.TextBox txtMessageReceivedScraperMaxDelay;
        private System.Windows.Forms.Label label92;
        private System.Windows.Forms.Label label93;
        private System.Windows.Forms.TextBox txtMessageReceivedScraperMinDelay;
        private System.Windows.Forms.GroupBox gbFriendsGroupLogger;
        private System.Windows.Forms.ListBox lstLogMessageReceivedScraper;
        private System.Windows.Forms.GroupBox gbMultiExlInputAction;
        private System.Windows.Forms.Button btnStopMessageReceivedScraper;
        private System.Windows.Forms.Button btnStartMessageReceivedScraper;
    }
}