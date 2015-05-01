namespace InBoardPro
{
    partial class frmJoinGroupUsingUrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmJoinGroupUsingUrl));
            this.gbJoinGroupUrlInput = new System.Windows.Forms.GroupBox();
            this.gbdivedDataSetting = new System.Windows.Forms.GroupBox();
            this.chkJoinGroupUsingUrlUseDivide = new System.Windows.Forms.CheckBox();
            this.grpFollowerDivideUserID = new System.Windows.Forms.GroupBox();
            this.label37 = new System.Windows.Forms.Label();
            this.txtJoinGroupUsingUrlNoOfUsers = new System.Windows.Forms.TextBox();
            this.rdBtnJoinGroupUsingUrlDivideByGivenNo = new System.Windows.Forms.RadioButton();
            this.label117 = new System.Windows.Forms.Label();
            this.rdBtnJoinGroupUsingUrlDivideEqually = new System.Windows.Forms.RadioButton();
            this.txtNumberOfGroupsPerAccount = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.TxtGroupURL = new System.Windows.Forms.TextBox();
            this.lblGrupURL_ScrapGroupMember = new System.Windows.Forms.Label();
            this.BtnUploadGroupURL = new System.Windows.Forms.Button();
            this.grpGroupPerAccount = new System.Windows.Forms.GroupBox();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnJoinSearchGroup = new System.Windows.Forms.Button();
            this.gbJoinGroupUrlLogger = new System.Windows.Forms.GroupBox();
            this.listLoggerGroupUrl = new System.Windows.Forms.ListBox();
            this.gbJoinGroupUrlAction = new System.Windows.Forms.GroupBox();
            this.gbJoinGroupUrlDelaySetting = new System.Windows.Forms.GroupBox();
            this.txtNoOfThread = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label113 = new System.Windows.Forms.Label();
            this.txtSearchGroupMaxDelay = new System.Windows.Forms.TextBox();
            this.label92 = new System.Windows.Forms.Label();
            this.label93 = new System.Windows.Forms.Label();
            this.txtSearchGroupMinDelay = new System.Windows.Forms.TextBox();
            this.gbJoinGroupUrlInput.SuspendLayout();
            this.gbdivedDataSetting.SuspendLayout();
            this.grpFollowerDivideUserID.SuspendLayout();
            this.gbJoinGroupUrlLogger.SuspendLayout();
            this.gbJoinGroupUrlAction.SuspendLayout();
            this.gbJoinGroupUrlDelaySetting.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbJoinGroupUrlInput
            // 
            this.gbJoinGroupUrlInput.BackColor = System.Drawing.Color.Transparent;
            this.gbJoinGroupUrlInput.Controls.Add(this.gbdivedDataSetting);
            this.gbJoinGroupUrlInput.Controls.Add(this.txtNumberOfGroupsPerAccount);
            this.gbJoinGroupUrlInput.Controls.Add(this.label1);
            this.gbJoinGroupUrlInput.Controls.Add(this.TxtGroupURL);
            this.gbJoinGroupUrlInput.Controls.Add(this.lblGrupURL_ScrapGroupMember);
            this.gbJoinGroupUrlInput.Controls.Add(this.BtnUploadGroupURL);
            this.gbJoinGroupUrlInput.Controls.Add(this.grpGroupPerAccount);
            this.gbJoinGroupUrlInput.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbJoinGroupUrlInput.Location = new System.Drawing.Point(25, 27);
            this.gbJoinGroupUrlInput.Name = "gbJoinGroupUrlInput";
            this.gbJoinGroupUrlInput.Size = new System.Drawing.Size(775, 141);
            this.gbJoinGroupUrlInput.TabIndex = 5;
            this.gbJoinGroupUrlInput.TabStop = false;
            this.gbJoinGroupUrlInput.Text = "Url Input For Join Group";
            // 
            // gbdivedDataSetting
            // 
            this.gbdivedDataSetting.Controls.Add(this.chkJoinGroupUsingUrlUseDivide);
            this.gbdivedDataSetting.Controls.Add(this.grpFollowerDivideUserID);
            this.gbdivedDataSetting.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbdivedDataSetting.Location = new System.Drawing.Point(169, 57);
            this.gbdivedDataSetting.Name = "gbdivedDataSetting";
            this.gbdivedDataSetting.Size = new System.Drawing.Size(601, 71);
            this.gbdivedDataSetting.TabIndex = 180;
            this.gbdivedDataSetting.TabStop = false;
            this.gbdivedDataSetting.Text = "Divide Data Setting";
            // 
            // chkJoinGroupUsingUrlUseDivide
            // 
            this.chkJoinGroupUsingUrlUseDivide.AutoSize = true;
            this.chkJoinGroupUsingUrlUseDivide.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.chkJoinGroupUsingUrlUseDivide.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkJoinGroupUsingUrlUseDivide.ForeColor = System.Drawing.Color.Black;
            this.chkJoinGroupUsingUrlUseDivide.Location = new System.Drawing.Point(11, 32);
            this.chkJoinGroupUsingUrlUseDivide.Name = "chkJoinGroupUsingUrlUseDivide";
            this.chkJoinGroupUsingUrlUseDivide.Size = new System.Drawing.Size(87, 17);
            this.chkJoinGroupUsingUrlUseDivide.TabIndex = 178;
            this.chkJoinGroupUsingUrlUseDivide.Text = "DivideData";
            this.chkJoinGroupUsingUrlUseDivide.UseVisualStyleBackColor = true;
            // 
            // grpFollowerDivideUserID
            // 
            this.grpFollowerDivideUserID.Controls.Add(this.label37);
            this.grpFollowerDivideUserID.Controls.Add(this.txtJoinGroupUsingUrlNoOfUsers);
            this.grpFollowerDivideUserID.Controls.Add(this.rdBtnJoinGroupUsingUrlDivideByGivenNo);
            this.grpFollowerDivideUserID.Controls.Add(this.label117);
            this.grpFollowerDivideUserID.Controls.Add(this.rdBtnJoinGroupUsingUrlDivideEqually);
            this.grpFollowerDivideUserID.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpFollowerDivideUserID.Location = new System.Drawing.Point(103, 20);
            this.grpFollowerDivideUserID.Name = "grpFollowerDivideUserID";
            this.grpFollowerDivideUserID.Size = new System.Drawing.Size(488, 45);
            this.grpFollowerDivideUserID.TabIndex = 177;
            this.grpFollowerDivideUserID.TabStop = false;
            this.grpFollowerDivideUserID.Text = "Divide Url";
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label37.Location = new System.Drawing.Point(276, 21);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(43, 13);
            this.label37.TabIndex = 66;
            this.label37.Text = ">>>>";
            // 
            // txtJoinGroupUsingUrlNoOfUsers
            // 
            this.txtJoinGroupUsingUrlNoOfUsers.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtJoinGroupUsingUrlNoOfUsers.Enabled = false;
            this.txtJoinGroupUsingUrlNoOfUsers.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtJoinGroupUsingUrlNoOfUsers.ForeColor = System.Drawing.Color.Black;
            this.txtJoinGroupUsingUrlNoOfUsers.Location = new System.Drawing.Point(420, 18);
            this.txtJoinGroupUsingUrlNoOfUsers.Name = "txtJoinGroupUsingUrlNoOfUsers";
            this.txtJoinGroupUsingUrlNoOfUsers.Size = new System.Drawing.Size(43, 21);
            this.txtJoinGroupUsingUrlNoOfUsers.TabIndex = 65;
            // 
            // rdBtnJoinGroupUsingUrlDivideByGivenNo
            // 
            this.rdBtnJoinGroupUsingUrlDivideByGivenNo.AutoSize = true;
            this.rdBtnJoinGroupUsingUrlDivideByGivenNo.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.rdBtnJoinGroupUsingUrlDivideByGivenNo.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdBtnJoinGroupUsingUrlDivideByGivenNo.ForeColor = System.Drawing.Color.Black;
            this.rdBtnJoinGroupUsingUrlDivideByGivenNo.Location = new System.Drawing.Point(122, 19);
            this.rdBtnJoinGroupUsingUrlDivideByGivenNo.Name = "rdBtnJoinGroupUsingUrlDivideByGivenNo";
            this.rdBtnJoinGroupUsingUrlDivideByGivenNo.Size = new System.Drawing.Size(146, 17);
            this.rdBtnJoinGroupUsingUrlDivideByGivenNo.TabIndex = 1;
            this.rdBtnJoinGroupUsingUrlDivideByGivenNo.TabStop = true;
            this.rdBtnJoinGroupUsingUrlDivideByGivenNo.Text = "Divide Given By User";
            this.rdBtnJoinGroupUsingUrlDivideByGivenNo.UseVisualStyleBackColor = true;
            this.rdBtnJoinGroupUsingUrlDivideByGivenNo.CheckedChanged += new System.EventHandler(this.rdBtnJoinGroupUsingUrlDivideByGivenNo_CheckedChanged);
            // 
            // label117
            // 
            this.label117.AutoSize = true;
            this.label117.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label117.ForeColor = System.Drawing.Color.Black;
            this.label117.Location = new System.Drawing.Point(331, 22);
            this.label117.Name = "label117";
            this.label117.Size = new System.Drawing.Size(80, 13);
            this.label117.TabIndex = 64;
            this.label117.Text = "No Of Users:";
            // 
            // rdBtnJoinGroupUsingUrlDivideEqually
            // 
            this.rdBtnJoinGroupUsingUrlDivideEqually.AutoSize = true;
            this.rdBtnJoinGroupUsingUrlDivideEqually.Checked = true;
            this.rdBtnJoinGroupUsingUrlDivideEqually.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.rdBtnJoinGroupUsingUrlDivideEqually.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdBtnJoinGroupUsingUrlDivideEqually.ForeColor = System.Drawing.Color.Black;
            this.rdBtnJoinGroupUsingUrlDivideEqually.Location = new System.Drawing.Point(7, 18);
            this.rdBtnJoinGroupUsingUrlDivideEqually.Name = "rdBtnJoinGroupUsingUrlDivideEqually";
            this.rdBtnJoinGroupUsingUrlDivideEqually.Size = new System.Drawing.Size(105, 17);
            this.rdBtnJoinGroupUsingUrlDivideEqually.TabIndex = 0;
            this.rdBtnJoinGroupUsingUrlDivideEqually.TabStop = true;
            this.rdBtnJoinGroupUsingUrlDivideEqually.Text = "Divide Equally";
            this.rdBtnJoinGroupUsingUrlDivideEqually.UseVisualStyleBackColor = true;
            // 
            // txtNumberOfGroupsPerAccount
            // 
            this.txtNumberOfGroupsPerAccount.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNumberOfGroupsPerAccount.Location = new System.Drawing.Point(57, 100);
            this.txtNumberOfGroupsPerAccount.Name = "txtNumberOfGroupsPerAccount";
            this.txtNumberOfGroupsPerAccount.Size = new System.Drawing.Size(53, 21);
            this.txtNumberOfGroupsPerAccount.TabIndex = 139;
            this.txtNumberOfGroupsPerAccount.Text = "0";
            this.txtNumberOfGroupsPerAccount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(9, 78);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(153, 13);
            this.label1.TabIndex = 138;
            this.label1.Text = "No of Group Per Account:";
            // 
            // TxtGroupURL
            // 
            this.TxtGroupURL.BackColor = System.Drawing.Color.White;
            this.TxtGroupURL.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TxtGroupURL.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtGroupURL.Location = new System.Drawing.Point(103, 30);
            this.TxtGroupURL.Name = "TxtGroupURL";
            this.TxtGroupURL.ReadOnly = true;
            this.TxtGroupURL.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.TxtGroupURL.Size = new System.Drawing.Size(550, 21);
            this.TxtGroupURL.TabIndex = 133;
            // 
            // lblGrupURL_ScrapGroupMember
            // 
            this.lblGrupURL_ScrapGroupMember.AutoSize = true;
            this.lblGrupURL_ScrapGroupMember.BackColor = System.Drawing.Color.Transparent;
            this.lblGrupURL_ScrapGroupMember.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGrupURL_ScrapGroupMember.Location = new System.Drawing.Point(20, 34);
            this.lblGrupURL_ScrapGroupMember.Name = "lblGrupURL_ScrapGroupMember";
            this.lblGrupURL_ScrapGroupMember.Size = new System.Drawing.Size(73, 13);
            this.lblGrupURL_ScrapGroupMember.TabIndex = 132;
            this.lblGrupURL_ScrapGroupMember.Text = "Group URL:";
            // 
            // BtnUploadGroupURL
            // 
            this.BtnUploadGroupURL.BackColor = System.Drawing.Color.White;
            this.BtnUploadGroupURL.BackgroundImage = global::InBoardPro.Properties.Resources.browse_btn;
            this.BtnUploadGroupURL.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.BtnUploadGroupURL.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.BtnUploadGroupURL.ForeColor = System.Drawing.Color.Black;
            this.BtnUploadGroupURL.Location = new System.Drawing.Point(660, 26);
            this.BtnUploadGroupURL.Name = "BtnUploadGroupURL";
            this.BtnUploadGroupURL.Size = new System.Drawing.Size(75, 29);
            this.BtnUploadGroupURL.TabIndex = 134;
            this.BtnUploadGroupURL.UseVisualStyleBackColor = false;
            this.BtnUploadGroupURL.Click += new System.EventHandler(this.BtnUploadGroupURL_Click);
            // 
            // grpGroupPerAccount
            // 
            this.grpGroupPerAccount.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpGroupPerAccount.ForeColor = System.Drawing.Color.Black;
            this.grpGroupPerAccount.Location = new System.Drawing.Point(6, 57);
            this.grpGroupPerAccount.Name = "grpGroupPerAccount";
            this.grpGroupPerAccount.Size = new System.Drawing.Size(162, 71);
            this.grpGroupPerAccount.TabIndex = 181;
            this.grpGroupPerAccount.TabStop = false;
            this.grpGroupPerAccount.Text = "Group Per Account";
            // 
            // btnStop
            // 
            this.btnStop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnStop.Image = global::InBoardPro.Properties.Resources.stop;
            this.btnStop.Location = new System.Drawing.Point(384, 20);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(126, 30);
            this.btnStop.TabIndex = 136;
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnJoinSearchGroup
            // 
            this.btnJoinSearchGroup.BackColor = System.Drawing.Color.White;
            this.btnJoinSearchGroup.BackgroundImage = global::InBoardPro.Properties.Resources.join_group;
            this.btnJoinSearchGroup.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnJoinSearchGroup.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnJoinSearchGroup.Font = new System.Drawing.Font("Cambria", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnJoinSearchGroup.Location = new System.Drawing.Point(239, 19);
            this.btnJoinSearchGroup.Name = "btnJoinSearchGroup";
            this.btnJoinSearchGroup.Size = new System.Drawing.Size(124, 31);
            this.btnJoinSearchGroup.TabIndex = 135;
            this.btnJoinSearchGroup.UseVisualStyleBackColor = false;
            this.btnJoinSearchGroup.Click += new System.EventHandler(this.btnJoinSearchGroup_Click);
            // 
            // gbJoinGroupUrlLogger
            // 
            this.gbJoinGroupUrlLogger.BackColor = System.Drawing.Color.Transparent;
            this.gbJoinGroupUrlLogger.Controls.Add(this.listLoggerGroupUrl);
            this.gbJoinGroupUrlLogger.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbJoinGroupUrlLogger.Location = new System.Drawing.Point(25, 299);
            this.gbJoinGroupUrlLogger.Name = "gbJoinGroupUrlLogger";
            this.gbJoinGroupUrlLogger.Size = new System.Drawing.Size(775, 166);
            this.gbJoinGroupUrlLogger.TabIndex = 100;
            this.gbJoinGroupUrlLogger.TabStop = false;
            this.gbJoinGroupUrlLogger.Text = "Logger";
            // 
            // listLoggerGroupUrl
            // 
            this.listLoggerGroupUrl.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listLoggerGroupUrl.FormattingEnabled = true;
            this.listLoggerGroupUrl.HorizontalScrollbar = true;
            this.listLoggerGroupUrl.Location = new System.Drawing.Point(7, 21);
            this.listLoggerGroupUrl.Name = "listLoggerGroupUrl";
            this.listLoggerGroupUrl.ScrollAlwaysVisible = true;
            this.listLoggerGroupUrl.Size = new System.Drawing.Size(752, 134);
            this.listLoggerGroupUrl.TabIndex = 8;
            // 
            // gbJoinGroupUrlAction
            // 
            this.gbJoinGroupUrlAction.BackColor = System.Drawing.Color.Transparent;
            this.gbJoinGroupUrlAction.Controls.Add(this.btnJoinSearchGroup);
            this.gbJoinGroupUrlAction.Controls.Add(this.btnStop);
            this.gbJoinGroupUrlAction.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbJoinGroupUrlAction.Location = new System.Drawing.Point(25, 230);
            this.gbJoinGroupUrlAction.Name = "gbJoinGroupUrlAction";
            this.gbJoinGroupUrlAction.Size = new System.Drawing.Size(775, 63);
            this.gbJoinGroupUrlAction.TabIndex = 101;
            this.gbJoinGroupUrlAction.TabStop = false;
            this.gbJoinGroupUrlAction.Text = "Submit Action";
            // 
            // gbJoinGroupUrlDelaySetting
            // 
            this.gbJoinGroupUrlDelaySetting.BackColor = System.Drawing.Color.Transparent;
            this.gbJoinGroupUrlDelaySetting.Controls.Add(this.txtNoOfThread);
            this.gbJoinGroupUrlDelaySetting.Controls.Add(this.label2);
            this.gbJoinGroupUrlDelaySetting.Controls.Add(this.label113);
            this.gbJoinGroupUrlDelaySetting.Controls.Add(this.txtSearchGroupMaxDelay);
            this.gbJoinGroupUrlDelaySetting.Controls.Add(this.label92);
            this.gbJoinGroupUrlDelaySetting.Controls.Add(this.label93);
            this.gbJoinGroupUrlDelaySetting.Controls.Add(this.txtSearchGroupMinDelay);
            this.gbJoinGroupUrlDelaySetting.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbJoinGroupUrlDelaySetting.Location = new System.Drawing.Point(25, 174);
            this.gbJoinGroupUrlDelaySetting.Name = "gbJoinGroupUrlDelaySetting";
            this.gbJoinGroupUrlDelaySetting.Size = new System.Drawing.Size(775, 49);
            this.gbJoinGroupUrlDelaySetting.TabIndex = 138;
            this.gbJoinGroupUrlDelaySetting.TabStop = false;
            this.gbJoinGroupUrlDelaySetting.Text = "Setting";
            // 
            // txtNoOfThread
            // 
            this.txtNoOfThread.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNoOfThread.Location = new System.Drawing.Point(175, 19);
            this.txtNoOfThread.Name = "txtNoOfThread";
            this.txtNoOfThread.Size = new System.Drawing.Size(44, 21);
            this.txtNoOfThread.TabIndex = 130;
            this.txtNoOfThread.Text = "5";
            this.txtNoOfThread.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(83, 22);
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
            this.label113.Location = new System.Drawing.Point(439, 21);
            this.label113.Name = "label113";
            this.label113.Size = new System.Drawing.Size(311, 13);
            this.label113.TabIndex = 126;
            this.label113.Text = "(Its randomly delay between both values in seconds)";
            // 
            // txtSearchGroupMaxDelay
            // 
            this.txtSearchGroupMaxDelay.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearchGroupMaxDelay.Location = new System.Drawing.Point(390, 18);
            this.txtSearchGroupMaxDelay.Name = "txtSearchGroupMaxDelay";
            this.txtSearchGroupMaxDelay.Size = new System.Drawing.Size(43, 21);
            this.txtSearchGroupMaxDelay.TabIndex = 119;
            this.txtSearchGroupMaxDelay.Text = "25";
            this.txtSearchGroupMaxDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label92
            // 
            this.label92.AutoSize = true;
            this.label92.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label92.Location = new System.Drawing.Point(259, 22);
            this.label92.Name = "label92";
            this.label92.Size = new System.Drawing.Size(45, 13);
            this.label92.TabIndex = 116;
            this.label92.Text = "Delay:";
            // 
            // label93
            // 
            this.label93.AutoSize = true;
            this.label93.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label93.Location = new System.Drawing.Point(361, 21);
            this.label93.Name = "label93";
            this.label93.Size = new System.Drawing.Size(26, 13);
            this.label93.TabIndex = 117;
            this.label93.Text = "To:";
            // 
            // txtSearchGroupMinDelay
            // 
            this.txtSearchGroupMinDelay.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearchGroupMinDelay.Location = new System.Drawing.Point(308, 18);
            this.txtSearchGroupMinDelay.Name = "txtSearchGroupMinDelay";
            this.txtSearchGroupMinDelay.Size = new System.Drawing.Size(44, 21);
            this.txtSearchGroupMinDelay.TabIndex = 118;
            this.txtSearchGroupMinDelay.Text = "20";
            this.txtSearchGroupMinDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // frmJoinGroupUsingUrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(826, 472);
            this.Controls.Add(this.gbJoinGroupUrlDelaySetting);
            this.Controls.Add(this.gbJoinGroupUrlAction);
            this.Controls.Add(this.gbJoinGroupUrlLogger);
            this.Controls.Add(this.gbJoinGroupUrlInput);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmJoinGroupUsingUrl";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Join Group Using Url";
            this.Load += new System.EventHandler(this.frmJoinGroupUsingUrl_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.frmJoinGroupUsingUrl_Paint);
            this.gbJoinGroupUrlInput.ResumeLayout(false);
            this.gbJoinGroupUrlInput.PerformLayout();
            this.gbdivedDataSetting.ResumeLayout(false);
            this.gbdivedDataSetting.PerformLayout();
            this.grpFollowerDivideUserID.ResumeLayout(false);
            this.grpFollowerDivideUserID.PerformLayout();
            this.gbJoinGroupUrlLogger.ResumeLayout(false);
            this.gbJoinGroupUrlAction.ResumeLayout(false);
            this.gbJoinGroupUrlDelaySetting.ResumeLayout(false);
            this.gbJoinGroupUrlDelaySetting.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbJoinGroupUrlInput;
        private System.Windows.Forms.TextBox TxtGroupURL;
        private System.Windows.Forms.Label lblGrupURL_ScrapGroupMember;
        private System.Windows.Forms.Button BtnUploadGroupURL;
        private System.Windows.Forms.Button btnJoinSearchGroup;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.GroupBox gbJoinGroupUrlLogger;
        private System.Windows.Forms.ListBox listLoggerGroupUrl;
        private System.Windows.Forms.GroupBox gbJoinGroupUrlAction;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtNumberOfGroupsPerAccount;
        private System.Windows.Forms.GroupBox gbJoinGroupUrlDelaySetting;
        private System.Windows.Forms.TextBox txtSearchGroupMaxDelay;
        private System.Windows.Forms.Label label92;
        private System.Windows.Forms.Label label93;
        private System.Windows.Forms.TextBox txtSearchGroupMinDelay;
        private System.Windows.Forms.Label label113;
        private System.Windows.Forms.TextBox txtNoOfThread;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox gbdivedDataSetting;
        private System.Windows.Forms.CheckBox chkJoinGroupUsingUrlUseDivide;
        private System.Windows.Forms.GroupBox grpFollowerDivideUserID;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.TextBox txtJoinGroupUsingUrlNoOfUsers;
        private System.Windows.Forms.RadioButton rdBtnJoinGroupUsingUrlDivideByGivenNo;
        private System.Windows.Forms.Label label117;
        private System.Windows.Forms.RadioButton rdBtnJoinGroupUsingUrlDivideEqually;
        private System.Windows.Forms.GroupBox grpGroupPerAccount;
    }
}