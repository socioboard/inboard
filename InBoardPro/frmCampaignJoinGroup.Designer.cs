namespace InBoardPro
{
    partial class frmCampaignJoinGroup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCampaignJoinGroup));
            this.grp_CmpName = new System.Windows.Forms.GroupBox();
            this.lb_CampaignName = new System.Windows.Forms.Label();
            this.txt_CampaignName = new System.Windows.Forms.TextBox();
            this.grp_account = new System.Windows.Forms.GroupBox();
            this.lb_AccountFile = new System.Windows.Forms.Label();
            this.txt_accountfilepath = new System.Windows.Forms.TextBox();
            this.btn_uploadaccounts = new System.Windows.Forms.Button();
            this.TxtGroupURL = new System.Windows.Forms.TextBox();
            this.lblGrupURL_ScrapGroupMember = new System.Windows.Forms.Label();
            this.BtnUploadGroupURL = new System.Windows.Forms.Button();
            this.gbSelectionOptioninSearhScp = new System.Windows.Forms.GroupBox();
            this.rdbCampaignJoinGroupURL = new System.Windows.Forms.RadioButton();
            this.rdbCampaignJoinGroupKeyword = new System.Windows.Forms.RadioButton();
            this.grp_listofcampaign = new System.Windows.Forms.GroupBox();
            this.dgv_campaign = new System.Windows.Forms.DataGridView();
            this.CampaignName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FeatureName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CampaignEdit = new System.Windows.Forms.DataGridViewLinkColumn();
            this.BtnOnOff = new System.Windows.Forms.DataGridViewImageColumn();
            this.gbCampaignSubmitAction = new System.Windows.Forms.GroupBox();
            this.btn_UpdateCampaignJoinGroup = new System.Windows.Forms.Button();
            this.btn_saveCampaignJoinGroup = new System.Windows.Forms.Button();
            this.grpBoxInvitesPerDay = new System.Windows.Forms.GroupBox();
            this.txt_campMaximumNoRetweet = new System.Windows.Forms.TextBox();
            this.chk_InvitePerDay = new System.Windows.Forms.CheckBox();
            this.lb_campmaxretweet = new System.Windows.Forms.Label();
            this.gbAddConnectionLogger = new System.Windows.Forms.GroupBox();
            this.lbManageConnection = new System.Windows.Forms.ListBox();
            this.grp_CmpName.SuspendLayout();
            this.grp_account.SuspendLayout();
            this.gbSelectionOptioninSearhScp.SuspendLayout();
            this.grp_listofcampaign.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_campaign)).BeginInit();
            this.gbCampaignSubmitAction.SuspendLayout();
            this.grpBoxInvitesPerDay.SuspendLayout();
            this.gbAddConnectionLogger.SuspendLayout();
            this.SuspendLayout();
            // 
            // grp_CmpName
            // 
            this.grp_CmpName.BackColor = System.Drawing.Color.Transparent;
            this.grp_CmpName.Controls.Add(this.lb_CampaignName);
            this.grp_CmpName.Controls.Add(this.txt_CampaignName);
            this.grp_CmpName.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grp_CmpName.Location = new System.Drawing.Point(12, 12);
            this.grp_CmpName.Name = "grp_CmpName";
            this.grp_CmpName.Size = new System.Drawing.Size(464, 61);
            this.grp_CmpName.TabIndex = 13;
            this.grp_CmpName.TabStop = false;
            this.grp_CmpName.Text = "Campaign Name";
            // 
            // lb_CampaignName
            // 
            this.lb_CampaignName.AutoSize = true;
            this.lb_CampaignName.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_CampaignName.Location = new System.Drawing.Point(8, 31);
            this.lb_CampaignName.Name = "lb_CampaignName";
            this.lb_CampaignName.Size = new System.Drawing.Size(107, 13);
            this.lb_CampaignName.TabIndex = 2;
            this.lb_CampaignName.Text = "Campaign Name:";
            // 
            // txt_CampaignName
            // 
            this.txt_CampaignName.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_CampaignName.Location = new System.Drawing.Point(121, 28);
            this.txt_CampaignName.Name = "txt_CampaignName";
            this.txt_CampaignName.Size = new System.Drawing.Size(325, 21);
            this.txt_CampaignName.TabIndex = 1;
            // 
            // grp_account
            // 
            this.grp_account.BackColor = System.Drawing.Color.Transparent;
            this.grp_account.Controls.Add(this.lb_AccountFile);
            this.grp_account.Controls.Add(this.txt_accountfilepath);
            this.grp_account.Controls.Add(this.btn_uploadaccounts);
            this.grp_account.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grp_account.Location = new System.Drawing.Point(509, 12);
            this.grp_account.Name = "grp_account";
            this.grp_account.Size = new System.Drawing.Size(508, 61);
            this.grp_account.TabIndex = 14;
            this.grp_account.TabStop = false;
            this.grp_account.Text = "Accounts";
            // 
            // lb_AccountFile
            // 
            this.lb_AccountFile.AutoSize = true;
            this.lb_AccountFile.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_AccountFile.Location = new System.Drawing.Point(13, 28);
            this.lb_AccountFile.Name = "lb_AccountFile";
            this.lb_AccountFile.Size = new System.Drawing.Size(80, 13);
            this.lb_AccountFile.TabIndex = 3;
            this.lb_AccountFile.Text = "Account File:";
            // 
            // txt_accountfilepath
            // 
            this.txt_accountfilepath.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_accountfilepath.Location = new System.Drawing.Point(99, 25);
            this.txt_accountfilepath.Name = "txt_accountfilepath";
            this.txt_accountfilepath.ReadOnly = true;
            this.txt_accountfilepath.Size = new System.Drawing.Size(282, 21);
            this.txt_accountfilepath.TabIndex = 1;
            // 
            // btn_uploadaccounts
            // 
            this.btn_uploadaccounts.BackgroundImage = global::InBoardPro.Properties.Resources.browse_btn;
            this.btn_uploadaccounts.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btn_uploadaccounts.Location = new System.Drawing.Point(384, 21);
            this.btn_uploadaccounts.Name = "btn_uploadaccounts";
            this.btn_uploadaccounts.Size = new System.Drawing.Size(80, 28);
            this.btn_uploadaccounts.TabIndex = 0;
            this.btn_uploadaccounts.UseVisualStyleBackColor = true;
            // 
            // TxtGroupURL
            // 
            this.TxtGroupURL.BackColor = System.Drawing.Color.White;
            this.TxtGroupURL.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TxtGroupURL.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtGroupURL.Location = new System.Drawing.Point(121, 29);
            this.TxtGroupURL.Name = "TxtGroupURL";
            this.TxtGroupURL.ReadOnly = true;
            this.TxtGroupURL.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.TxtGroupURL.Size = new System.Drawing.Size(550, 21);
            this.TxtGroupURL.TabIndex = 136;
            // 
            // lblGrupURL_ScrapGroupMember
            // 
            this.lblGrupURL_ScrapGroupMember.AutoSize = true;
            this.lblGrupURL_ScrapGroupMember.BackColor = System.Drawing.Color.Transparent;
            this.lblGrupURL_ScrapGroupMember.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGrupURL_ScrapGroupMember.Location = new System.Drawing.Point(21, 31);
            this.lblGrupURL_ScrapGroupMember.Name = "lblGrupURL_ScrapGroupMember";
            this.lblGrupURL_ScrapGroupMember.Size = new System.Drawing.Size(92, 13);
            this.lblGrupURL_ScrapGroupMember.TabIndex = 135;
            this.lblGrupURL_ScrapGroupMember.Text = "URL / Keyword";
            // 
            // BtnUploadGroupURL
            // 
            this.BtnUploadGroupURL.BackColor = System.Drawing.Color.White;
            this.BtnUploadGroupURL.BackgroundImage = global::InBoardPro.Properties.Resources.browse_btn;
            this.BtnUploadGroupURL.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.BtnUploadGroupURL.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.BtnUploadGroupURL.ForeColor = System.Drawing.Color.Black;
            this.BtnUploadGroupURL.Location = new System.Drawing.Point(708, 23);
            this.BtnUploadGroupURL.Name = "BtnUploadGroupURL";
            this.BtnUploadGroupURL.Size = new System.Drawing.Size(75, 29);
            this.BtnUploadGroupURL.TabIndex = 137;
            this.BtnUploadGroupURL.UseVisualStyleBackColor = false;
            // 
            // gbSelectionOptioninSearhScp
            // 
            this.gbSelectionOptioninSearhScp.Controls.Add(this.rdbCampaignJoinGroupURL);
            this.gbSelectionOptioninSearhScp.Controls.Add(this.rdbCampaignJoinGroupKeyword);
            this.gbSelectionOptioninSearhScp.Controls.Add(this.lblGrupURL_ScrapGroupMember);
            this.gbSelectionOptioninSearhScp.Controls.Add(this.BtnUploadGroupURL);
            this.gbSelectionOptioninSearhScp.Controls.Add(this.TxtGroupURL);
            this.gbSelectionOptioninSearhScp.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbSelectionOptioninSearhScp.Location = new System.Drawing.Point(12, 79);
            this.gbSelectionOptioninSearhScp.Name = "gbSelectionOptioninSearhScp";
            this.gbSelectionOptioninSearhScp.Size = new System.Drawing.Size(1005, 97);
            this.gbSelectionOptioninSearhScp.TabIndex = 138;
            this.gbSelectionOptioninSearhScp.TabStop = false;
            this.gbSelectionOptioninSearhScp.Text = "Based on Keyword OR Url";
            // 
            // rdbCampaignJoinGroupURL
            // 
            this.rdbCampaignJoinGroupURL.AutoSize = true;
            this.rdbCampaignJoinGroupURL.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdbCampaignJoinGroupURL.ForeColor = System.Drawing.Color.Black;
            this.rdbCampaignJoinGroupURL.Location = new System.Drawing.Point(267, 68);
            this.rdbCampaignJoinGroupURL.Name = "rdbCampaignJoinGroupURL";
            this.rdbCampaignJoinGroupURL.Size = new System.Drawing.Size(47, 17);
            this.rdbCampaignJoinGroupURL.TabIndex = 139;
            this.rdbCampaignJoinGroupURL.Text = "URL";
            this.rdbCampaignJoinGroupURL.UseVisualStyleBackColor = true;
            // 
            // rdbCampaignJoinGroupKeyword
            // 
            this.rdbCampaignJoinGroupKeyword.AutoSize = true;
            this.rdbCampaignJoinGroupKeyword.Checked = true;
            this.rdbCampaignJoinGroupKeyword.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdbCampaignJoinGroupKeyword.ForeColor = System.Drawing.Color.Black;
            this.rdbCampaignJoinGroupKeyword.Location = new System.Drawing.Point(121, 68);
            this.rdbCampaignJoinGroupKeyword.Name = "rdbCampaignJoinGroupKeyword";
            this.rdbCampaignJoinGroupKeyword.Size = new System.Drawing.Size(75, 17);
            this.rdbCampaignJoinGroupKeyword.TabIndex = 138;
            this.rdbCampaignJoinGroupKeyword.TabStop = true;
            this.rdbCampaignJoinGroupKeyword.Text = "Keyword";
            this.rdbCampaignJoinGroupKeyword.UseVisualStyleBackColor = true;
            // 
            // grp_listofcampaign
            // 
            this.grp_listofcampaign.BackColor = System.Drawing.Color.Transparent;
            this.grp_listofcampaign.Controls.Add(this.dgv_campaign);
            this.grp_listofcampaign.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grp_listofcampaign.Location = new System.Drawing.Point(12, 182);
            this.grp_listofcampaign.Name = "grp_listofcampaign";
            this.grp_listofcampaign.Size = new System.Drawing.Size(430, 144);
            this.grp_listofcampaign.TabIndex = 139;
            this.grp_listofcampaign.TabStop = false;
            this.grp_listofcampaign.Text = "Campaigns";
            // 
            // dgv_campaign
            // 
            this.dgv_campaign.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_campaign.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CampaignName,
            this.FeatureName,
            this.CampaignEdit,
            this.BtnOnOff});
            this.dgv_campaign.Location = new System.Drawing.Point(6, 18);
            this.dgv_campaign.Name = "dgv_campaign";
            this.dgv_campaign.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgv_campaign.Size = new System.Drawing.Size(390, 100);
            this.dgv_campaign.TabIndex = 0;
            // 
            // CampaignName
            // 
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CampaignName.DefaultCellStyle = dataGridViewCellStyle1;
            this.CampaignName.Frozen = true;
            this.CampaignName.HeaderText = "Campaign Name";
            this.CampaignName.Name = "CampaignName";
            this.CampaignName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.CampaignName.Width = 134;
            // 
            // FeatureName
            // 
            this.FeatureName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FeatureName.DefaultCellStyle = dataGridViewCellStyle2;
            this.FeatureName.Frozen = true;
            this.FeatureName.HeaderText = "Keyword/URL";
            this.FeatureName.Name = "FeatureName";
            this.FeatureName.Width = 96;
            // 
            // CampaignEdit
            // 
            this.CampaignEdit.HeaderText = "";
            this.CampaignEdit.Name = "CampaignEdit";
            this.CampaignEdit.Text = "Edit";
            this.CampaignEdit.UseColumnTextForLinkValue = true;
            this.CampaignEdit.Width = 60;
            // 
            // BtnOnOff
            // 
            this.BtnOnOff.HeaderText = "";
            this.BtnOnOff.Name = "BtnOnOff";
            this.BtnOnOff.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.BtnOnOff.Width = 40;
            // 
            // gbCampaignSubmitAction
            // 
            this.gbCampaignSubmitAction.BackColor = System.Drawing.Color.Transparent;
            this.gbCampaignSubmitAction.Controls.Add(this.btn_UpdateCampaignJoinGroup);
            this.gbCampaignSubmitAction.Controls.Add(this.btn_saveCampaignJoinGroup);
            this.gbCampaignSubmitAction.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbCampaignSubmitAction.Location = new System.Drawing.Point(481, 250);
            this.gbCampaignSubmitAction.Name = "gbCampaignSubmitAction";
            this.gbCampaignSubmitAction.Size = new System.Drawing.Size(536, 76);
            this.gbCampaignSubmitAction.TabIndex = 140;
            this.gbCampaignSubmitAction.TabStop = false;
            this.gbCampaignSubmitAction.Text = "Submit Action";
            // 
            // btn_UpdateCampaignJoinGroup
            // 
            this.btn_UpdateCampaignJoinGroup.BackColor = System.Drawing.Color.Transparent;
            this.btn_UpdateCampaignJoinGroup.BackgroundImage = global::InBoardPro.Properties.Resources.UPDATE_CAMPAIGN;
            this.btn_UpdateCampaignJoinGroup.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btn_UpdateCampaignJoinGroup.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_UpdateCampaignJoinGroup.Location = new System.Drawing.Point(269, 20);
            this.btn_UpdateCampaignJoinGroup.Name = "btn_UpdateCampaignJoinGroup";
            this.btn_UpdateCampaignJoinGroup.Size = new System.Drawing.Size(128, 32);
            this.btn_UpdateCampaignJoinGroup.TabIndex = 12;
            this.btn_UpdateCampaignJoinGroup.UseVisualStyleBackColor = false;
            this.btn_UpdateCampaignJoinGroup.Click += new System.EventHandler(this.btn_UpdateCampaignJoinGroup_Click);
            // 
            // btn_saveCampaignJoinGroup
            // 
            this.btn_saveCampaignJoinGroup.BackColor = System.Drawing.Color.Transparent;
            this.btn_saveCampaignJoinGroup.BackgroundImage = global::InBoardPro.Properties.Resources.SAVE_CAMPAIGN;
            this.btn_saveCampaignJoinGroup.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btn_saveCampaignJoinGroup.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_saveCampaignJoinGroup.Location = new System.Drawing.Point(120, 20);
            this.btn_saveCampaignJoinGroup.Name = "btn_saveCampaignJoinGroup";
            this.btn_saveCampaignJoinGroup.Size = new System.Drawing.Size(128, 32);
            this.btn_saveCampaignJoinGroup.TabIndex = 8;
            this.btn_saveCampaignJoinGroup.UseVisualStyleBackColor = false;
            // 
            // grpBoxInvitesPerDay
            // 
            this.grpBoxInvitesPerDay.BackColor = System.Drawing.Color.Transparent;
            this.grpBoxInvitesPerDay.Controls.Add(this.txt_campMaximumNoRetweet);
            this.grpBoxInvitesPerDay.Controls.Add(this.chk_InvitePerDay);
            this.grpBoxInvitesPerDay.Controls.Add(this.lb_campmaxretweet);
            this.grpBoxInvitesPerDay.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpBoxInvitesPerDay.ForeColor = System.Drawing.Color.Black;
            this.grpBoxInvitesPerDay.Location = new System.Drawing.Point(481, 182);
            this.grpBoxInvitesPerDay.Name = "grpBoxInvitesPerDay";
            this.grpBoxInvitesPerDay.Size = new System.Drawing.Size(536, 56);
            this.grpBoxInvitesPerDay.TabIndex = 141;
            this.grpBoxInvitesPerDay.TabStop = false;
            this.grpBoxInvitesPerDay.Text = "Invites Per Day";
            // 
            // txt_campMaximumNoRetweet
            // 
            this.txt_campMaximumNoRetweet.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_campMaximumNoRetweet.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_campMaximumNoRetweet.ForeColor = System.Drawing.Color.Black;
            this.txt_campMaximumNoRetweet.Location = new System.Drawing.Point(437, 19);
            this.txt_campMaximumNoRetweet.Name = "txt_campMaximumNoRetweet";
            this.txt_campMaximumNoRetweet.Size = new System.Drawing.Size(45, 21);
            this.txt_campMaximumNoRetweet.TabIndex = 77;
            this.txt_campMaximumNoRetweet.Text = "10";
            this.txt_campMaximumNoRetweet.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // chk_InvitePerDay
            // 
            this.chk_InvitePerDay.AutoSize = true;
            this.chk_InvitePerDay.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chk_InvitePerDay.ForeColor = System.Drawing.Color.Black;
            this.chk_InvitePerDay.Location = new System.Drawing.Point(99, 23);
            this.chk_InvitePerDay.Name = "chk_InvitePerDay";
            this.chk_InvitePerDay.Size = new System.Drawing.Size(134, 17);
            this.chk_InvitePerDay.TabIndex = 79;
            this.chk_InvitePerDay.Text = "Use Invite Per Day";
            this.chk_InvitePerDay.UseVisualStyleBackColor = true;
            // 
            // lb_campmaxretweet
            // 
            this.lb_campmaxretweet.AutoSize = true;
            this.lb_campmaxretweet.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_campmaxretweet.ForeColor = System.Drawing.Color.Black;
            this.lb_campmaxretweet.Location = new System.Drawing.Point(239, 24);
            this.lb_campmaxretweet.Name = "lb_campmaxretweet";
            this.lb_campmaxretweet.Size = new System.Drawing.Size(191, 13);
            this.lb_campmaxretweet.TabIndex = 76;
            this.lb_campmaxretweet.Text = "Maximum No Of Invites Per Day";
            // 
            // gbAddConnectionLogger
            // 
            this.gbAddConnectionLogger.BackColor = System.Drawing.Color.Transparent;
            this.gbAddConnectionLogger.Controls.Add(this.lbManageConnection);
            this.gbAddConnectionLogger.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbAddConnectionLogger.Location = new System.Drawing.Point(12, 332);
            this.gbAddConnectionLogger.Name = "gbAddConnectionLogger";
            this.gbAddConnectionLogger.Size = new System.Drawing.Size(1005, 157);
            this.gbAddConnectionLogger.TabIndex = 143;
            this.gbAddConnectionLogger.TabStop = false;
            this.gbAddConnectionLogger.Text = "Process Logger";
            // 
            // lbManageConnection
            // 
            this.lbManageConnection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbManageConnection.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbManageConnection.FormattingEnabled = true;
            this.lbManageConnection.HorizontalScrollbar = true;
            this.lbManageConnection.Location = new System.Drawing.Point(3, 17);
            this.lbManageConnection.Name = "lbManageConnection";
            this.lbManageConnection.Size = new System.Drawing.Size(999, 137);
            this.lbManageConnection.TabIndex = 12;
            // 
            // frmCampaignJoinGroup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1088, 501);
            this.Controls.Add(this.gbAddConnectionLogger);
            this.Controls.Add(this.grpBoxInvitesPerDay);
            this.Controls.Add(this.gbCampaignSubmitAction);
            this.Controls.Add(this.grp_listofcampaign);
            this.Controls.Add(this.gbSelectionOptioninSearhScp);
            this.Controls.Add(this.grp_account);
            this.Controls.Add(this.grp_CmpName);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmCampaignJoinGroup";
            this.Text = "frmCampaignJoinGroup";
            this.Load += new System.EventHandler(this.frmCampaignJoinGroup_Load);
            this.grp_CmpName.ResumeLayout(false);
            this.grp_CmpName.PerformLayout();
            this.grp_account.ResumeLayout(false);
            this.grp_account.PerformLayout();
            this.gbSelectionOptioninSearhScp.ResumeLayout(false);
            this.gbSelectionOptioninSearhScp.PerformLayout();
            this.grp_listofcampaign.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_campaign)).EndInit();
            this.gbCampaignSubmitAction.ResumeLayout(false);
            this.grpBoxInvitesPerDay.ResumeLayout(false);
            this.grpBoxInvitesPerDay.PerformLayout();
            this.gbAddConnectionLogger.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grp_CmpName;
        private System.Windows.Forms.Label lb_CampaignName;
        private System.Windows.Forms.TextBox txt_CampaignName;
        private System.Windows.Forms.GroupBox grp_account;
        private System.Windows.Forms.Label lb_AccountFile;
        private System.Windows.Forms.TextBox txt_accountfilepath;
        private System.Windows.Forms.Button btn_uploadaccounts;
        private System.Windows.Forms.TextBox TxtGroupURL;
        private System.Windows.Forms.Label lblGrupURL_ScrapGroupMember;
        private System.Windows.Forms.Button BtnUploadGroupURL;
        private System.Windows.Forms.GroupBox gbSelectionOptioninSearhScp;
        private System.Windows.Forms.RadioButton rdbCampaignJoinGroupKeyword;
        private System.Windows.Forms.RadioButton rdbCampaignJoinGroupURL;
        private System.Windows.Forms.GroupBox grp_listofcampaign;
        private System.Windows.Forms.DataGridView dgv_campaign;
        private System.Windows.Forms.GroupBox gbCampaignSubmitAction;
        private System.Windows.Forms.Button btn_UpdateCampaignJoinGroup;
        private System.Windows.Forms.Button btn_saveCampaignJoinGroup;
        private System.Windows.Forms.GroupBox grpBoxInvitesPerDay;
        private System.Windows.Forms.TextBox txt_campMaximumNoRetweet;
        private System.Windows.Forms.CheckBox chk_InvitePerDay;
        private System.Windows.Forms.Label lb_campmaxretweet;
        private System.Windows.Forms.GroupBox gbAddConnectionLogger;
        private System.Windows.Forms.ListBox lbManageConnection;
        private System.Windows.Forms.DataGridViewTextBoxColumn CampaignName;
        private System.Windows.Forms.DataGridViewTextBoxColumn FeatureName;
        private System.Windows.Forms.DataGridViewLinkColumn CampaignEdit;
        private System.Windows.Forms.DataGridViewImageColumn BtnOnOff;
    }
}