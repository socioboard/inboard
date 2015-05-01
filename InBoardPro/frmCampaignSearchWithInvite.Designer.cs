namespace InBoardPro
{
    partial class frmCampaignSearchWithInvite
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCampaignSearchWithInvite));
            this.grp_account = new System.Windows.Forms.GroupBox();
            this.lb_AccountFile = new System.Windows.Forms.Label();
            this.txt_accountfilepath = new System.Windows.Forms.TextBox();
            this.btn_uploadaccounts = new System.Windows.Forms.Button();
            this.grp_CmpName = new System.Windows.Forms.GroupBox();
            this.lb_CampaignName = new System.Windows.Forms.Label();
            this.txt_CampaignName = new System.Windows.Forms.TextBox();
            this.grp_listofcampaign = new System.Windows.Forms.GroupBox();
            this.dgv_campaign = new System.Windows.Forms.DataGridView();
            this.CampaignName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FeatureName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CampaignEdit = new System.Windows.Forms.DataGridViewLinkColumn();
            this.BtnOnOff = new System.Windows.Forms.DataGridViewImageColumn();
            this.gbCampaignSubmitAction = new System.Windows.Forms.GroupBox();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btn_savecampaign = new System.Windows.Forms.Button();
            this.txtInviteMsg = new System.Windows.Forms.TextBox();
            this.chkUseSpintax_InviteMessage = new System.Windows.Forms.CheckBox();
            this.gbAddConnectioDelaySetting = new System.Windows.Forms.GroupBox();
            this.txtThreadManageConnection = new System.Windows.Forms.TextBox();
            this.label107 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.txtSearchMaxDelay = new System.Windows.Forms.TextBox();
            this.txtSearchMindelay = new System.Windows.Forms.TextBox();
            this.label26 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.grpBoxInvitesPerDay = new System.Windows.Forms.GroupBox();
            this.txt_campMaximumNoRetweet = new System.Windows.Forms.TextBox();
            this.chk_InvitePerDay = new System.Windows.Forms.CheckBox();
            this.lb_campmaxretweet = new System.Windows.Forms.Label();
            this.groupBox56 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txt_campNoOfInvitesParAc = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtUploadKeyword = new System.Windows.Forms.TextBox();
            this.btnUploadKeyword = new System.Windows.Forms.Button();
            this.grpSearchWithInviteSheduling = new System.Windows.Forms.GroupBox();
            this.dateTimePicker_End = new System.Windows.Forms.DateTimePicker();
            this.btnShedulerStart = new System.Windows.Forms.Button();
            this.label89 = new System.Windows.Forms.Label();
            this.chkboxSearchWithInviteScheduledDaily = new System.Windows.Forms.CheckBox();
            this.dateTimePicker_Start_SearchWithInvite = new System.Windows.Forms.DateTimePicker();
            this.btnScheduleForLater_SearchWithInvite = new System.Windows.Forms.Button();
            this.gbAddConnectionLogger = new System.Windows.Forms.GroupBox();
            this.lbManageConnection = new System.Windows.Forms.ListBox();
            this.grp_account.SuspendLayout();
            this.grp_CmpName.SuspendLayout();
            this.grp_listofcampaign.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_campaign)).BeginInit();
            this.gbCampaignSubmitAction.SuspendLayout();
            this.gbAddConnectioDelaySetting.SuspendLayout();
            this.grpBoxInvitesPerDay.SuspendLayout();
            this.groupBox56.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.grpSearchWithInviteSheduling.SuspendLayout();
            this.gbAddConnectionLogger.SuspendLayout();
            this.SuspendLayout();
            // 
            // grp_account
            // 
            this.grp_account.BackColor = System.Drawing.Color.Transparent;
            this.grp_account.Controls.Add(this.lb_AccountFile);
            this.grp_account.Controls.Add(this.txt_accountfilepath);
            this.grp_account.Controls.Add(this.btn_uploadaccounts);
            this.grp_account.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grp_account.Location = new System.Drawing.Point(499, 12);
            this.grp_account.Name = "grp_account";
            this.grp_account.Size = new System.Drawing.Size(508, 61);
            this.grp_account.TabIndex = 11;
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
            this.btn_uploadaccounts.Click += new System.EventHandler(this.btn_uploadaccounts_Click);
            // 
            // grp_CmpName
            // 
            this.grp_CmpName.BackColor = System.Drawing.Color.Transparent;
            this.grp_CmpName.Controls.Add(this.lb_CampaignName);
            this.grp_CmpName.Controls.Add(this.txt_CampaignName);
            this.grp_CmpName.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grp_CmpName.Location = new System.Drawing.Point(23, 12);
            this.grp_CmpName.Name = "grp_CmpName";
            this.grp_CmpName.Size = new System.Drawing.Size(464, 61);
            this.grp_CmpName.TabIndex = 12;
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
            // grp_listofcampaign
            // 
            this.grp_listofcampaign.BackColor = System.Drawing.Color.Transparent;
            this.grp_listofcampaign.Controls.Add(this.dgv_campaign);
            this.grp_listofcampaign.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grp_listofcampaign.Location = new System.Drawing.Point(23, 272);
            this.grp_listofcampaign.Name = "grp_listofcampaign";
            this.grp_listofcampaign.Size = new System.Drawing.Size(430, 131);
            this.grp_listofcampaign.TabIndex = 90;
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
            this.dgv_campaign.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_campaign_CellClick);
            this.dgv_campaign.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_campaign_CellContentClick);
            this.dgv_campaign.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.dgv_campaign_UserDeletingRow);
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
            this.FeatureName.HeaderText = "Keyword";
            this.FeatureName.Name = "FeatureName";
            this.FeatureName.Width = 96;
            // 
            // CampaignEdit
            // 
            this.CampaignEdit.Frozen = true;
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
            this.gbCampaignSubmitAction.Controls.Add(this.btnUpdate);
            this.gbCampaignSubmitAction.Controls.Add(this.btn_savecampaign);
            this.gbCampaignSubmitAction.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbCampaignSubmitAction.Location = new System.Drawing.Point(459, 327);
            this.gbCampaignSubmitAction.Name = "gbCampaignSubmitAction";
            this.gbCampaignSubmitAction.Size = new System.Drawing.Size(548, 76);
            this.gbCampaignSubmitAction.TabIndex = 91;
            this.gbCampaignSubmitAction.TabStop = false;
            this.gbCampaignSubmitAction.Text = "Submit Action";
            // 
            // btnUpdate
            // 
            this.btnUpdate.BackColor = System.Drawing.Color.Transparent;
            this.btnUpdate.BackgroundImage = global::InBoardPro.Properties.Resources.UPDATE_CAMPAIGN;
            this.btnUpdate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnUpdate.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdate.Location = new System.Drawing.Point(269, 20);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(128, 32);
            this.btnUpdate.TabIndex = 12;
            this.btnUpdate.UseVisualStyleBackColor = false;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btn_savecampaign
            // 
            this.btn_savecampaign.BackColor = System.Drawing.Color.Transparent;
            this.btn_savecampaign.BackgroundImage = global::InBoardPro.Properties.Resources.SAVE_CAMPAIGN;
            this.btn_savecampaign.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btn_savecampaign.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_savecampaign.Location = new System.Drawing.Point(120, 20);
            this.btn_savecampaign.Name = "btn_savecampaign";
            this.btn_savecampaign.Size = new System.Drawing.Size(128, 32);
            this.btn_savecampaign.TabIndex = 8;
            this.btn_savecampaign.UseVisualStyleBackColor = false;
            this.btn_savecampaign.Click += new System.EventHandler(this.btn_savecampaign_Click);
            // 
            // txtInviteMsg
            // 
            this.txtInviteMsg.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInviteMsg.Location = new System.Drawing.Point(23, 83);
            this.txtInviteMsg.Multiline = true;
            this.txtInviteMsg.Name = "txtInviteMsg";
            this.txtInviteMsg.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtInviteMsg.Size = new System.Drawing.Size(464, 109);
            this.txtInviteMsg.TabIndex = 136;
            this.txtInviteMsg.Text = resources.GetString("txtInviteMsg.Text");
            this.txtInviteMsg.TextChanged += new System.EventHandler(this.txtInviteMsg_TextChanged);
            // 
            // chkUseSpintax_InviteMessage
            // 
            this.chkUseSpintax_InviteMessage.AutoSize = true;
            this.chkUseSpintax_InviteMessage.BackColor = System.Drawing.Color.Transparent;
            this.chkUseSpintax_InviteMessage.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkUseSpintax_InviteMessage.Location = new System.Drawing.Point(382, 198);
            this.chkUseSpintax_InviteMessage.Name = "chkUseSpintax_InviteMessage";
            this.chkUseSpintax_InviteMessage.Size = new System.Drawing.Size(94, 17);
            this.chkUseSpintax_InviteMessage.TabIndex = 135;
            this.chkUseSpintax_InviteMessage.Text = "Use Spintax";
            this.chkUseSpintax_InviteMessage.UseVisualStyleBackColor = false;
            this.chkUseSpintax_InviteMessage.CheckedChanged += new System.EventHandler(this.chkUseSpintax_InviteMessage_CheckedChanged);
            // 
            // gbAddConnectioDelaySetting
            // 
            this.gbAddConnectioDelaySetting.BackColor = System.Drawing.Color.Transparent;
            this.gbAddConnectioDelaySetting.Controls.Add(this.txtThreadManageConnection);
            this.gbAddConnectioDelaySetting.Controls.Add(this.label107);
            this.gbAddConnectioDelaySetting.Controls.Add(this.label11);
            this.gbAddConnectioDelaySetting.Controls.Add(this.txtSearchMaxDelay);
            this.gbAddConnectioDelaySetting.Controls.Add(this.txtSearchMindelay);
            this.gbAddConnectioDelaySetting.Controls.Add(this.label26);
            this.gbAddConnectioDelaySetting.Controls.Add(this.label18);
            this.gbAddConnectioDelaySetting.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbAddConnectioDelaySetting.Location = new System.Drawing.Point(23, 216);
            this.gbAddConnectioDelaySetting.Name = "gbAddConnectioDelaySetting";
            this.gbAddConnectioDelaySetting.Size = new System.Drawing.Size(732, 50);
            this.gbAddConnectioDelaySetting.TabIndex = 137;
            this.gbAddConnectioDelaySetting.TabStop = false;
            this.gbAddConnectioDelaySetting.Text = "Settings";
            // 
            // txtThreadManageConnection
            // 
            this.txtThreadManageConnection.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtThreadManageConnection.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtThreadManageConnection.Location = new System.Drawing.Point(658, 19);
            this.txtThreadManageConnection.Name = "txtThreadManageConnection";
            this.txtThreadManageConnection.Size = new System.Drawing.Size(45, 21);
            this.txtThreadManageConnection.TabIndex = 10;
            this.txtThreadManageConnection.Text = "5";
            this.txtThreadManageConnection.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtThreadManageConnection.Validating += new System.ComponentModel.CancelEventHandler(this.txtThreadManageConnection_Validating);
            // 
            // label107
            // 
            this.label107.AutoSize = true;
            this.label107.BackColor = System.Drawing.Color.Transparent;
            this.label107.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label107.Location = new System.Drawing.Point(237, 22);
            this.label107.Name = "label107";
            this.label107.Size = new System.Drawing.Size(305, 13);
            this.label107.TabIndex = 122;
            this.label107.Text = "Its randomly delay between both values in seconds.";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(563, 23);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(83, 13);
            this.label11.TabIndex = 0;
            this.label11.Text = "No Of Thread";
            // 
            // txtSearchMaxDelay
            // 
            this.txtSearchMaxDelay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearchMaxDelay.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearchMaxDelay.Location = new System.Drawing.Point(178, 19);
            this.txtSearchMaxDelay.Name = "txtSearchMaxDelay";
            this.txtSearchMaxDelay.Size = new System.Drawing.Size(43, 21);
            this.txtSearchMaxDelay.TabIndex = 9;
            this.txtSearchMaxDelay.Text = "25";
            this.txtSearchMaxDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtSearchMindelay
            // 
            this.txtSearchMindelay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearchMindelay.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearchMindelay.Location = new System.Drawing.Point(84, 19);
            this.txtSearchMindelay.Name = "txtSearchMindelay";
            this.txtSearchMindelay.Size = new System.Drawing.Size(44, 21);
            this.txtSearchMindelay.TabIndex = 8;
            this.txtSearchMindelay.Text = "20";
            this.txtSearchMindelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label26.Location = new System.Drawing.Point(146, 23);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(18, 13);
            this.label26.TabIndex = 1;
            this.label26.Text = "to";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(12, 20);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(40, 13);
            this.label18.TabIndex = 0;
            this.label18.Text = "Delay";
            // 
            // grpBoxInvitesPerDay
            // 
            this.grpBoxInvitesPerDay.BackColor = System.Drawing.Color.Transparent;
            this.grpBoxInvitesPerDay.Controls.Add(this.txt_campMaximumNoRetweet);
            this.grpBoxInvitesPerDay.Controls.Add(this.chk_InvitePerDay);
            this.grpBoxInvitesPerDay.Controls.Add(this.lb_campmaxretweet);
            this.grpBoxInvitesPerDay.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpBoxInvitesPerDay.ForeColor = System.Drawing.Color.Black;
            this.grpBoxInvitesPerDay.Location = new System.Drawing.Point(499, 146);
            this.grpBoxInvitesPerDay.Name = "grpBoxInvitesPerDay";
            this.grpBoxInvitesPerDay.Size = new System.Drawing.Size(508, 56);
            this.grpBoxInvitesPerDay.TabIndex = 138;
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
            this.txt_campMaximumNoRetweet.TextChanged += new System.EventHandler(this.txt_campMaximumNoRetweet_TextChanged);
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
            this.chk_InvitePerDay.CheckedChanged += new System.EventHandler(this.chk_InvitePerDay_CheckedChanged);
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
            // groupBox56
            // 
            this.groupBox56.BackColor = System.Drawing.Color.Transparent;
            this.groupBox56.Controls.Add(this.label9);
            this.groupBox56.Controls.Add(this.txt_campNoOfInvitesParAc);
            this.groupBox56.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox56.ForeColor = System.Drawing.Color.Black;
            this.groupBox56.Location = new System.Drawing.Point(768, 216);
            this.groupBox56.Name = "groupBox56";
            this.groupBox56.Size = new System.Drawing.Size(239, 50);
            this.groupBox56.TabIndex = 139;
            this.groupBox56.TabStop = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.Black;
            this.label9.Location = new System.Drawing.Point(28, 24);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(86, 13);
            this.label9.TabIndex = 56;
            this.label9.Text = "No. Of Invites";
            // 
            // txt_campNoOfInvitesParAc
            // 
            this.txt_campNoOfInvitesParAc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_campNoOfInvitesParAc.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_campNoOfInvitesParAc.ForeColor = System.Drawing.Color.Black;
            this.txt_campNoOfInvitesParAc.Location = new System.Drawing.Point(124, 20);
            this.txt_campNoOfInvitesParAc.Name = "txt_campNoOfInvitesParAc";
            this.txt_campNoOfInvitesParAc.Size = new System.Drawing.Size(45, 21);
            this.txt_campNoOfInvitesParAc.TabIndex = 55;
            this.txt_campNoOfInvitesParAc.Text = "50";
            this.txt_campNoOfInvitesParAc.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_campNoOfInvitesParAc.TextChanged += new System.EventHandler(this.txt_campNoOfInvitesParAc_TextChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtUploadKeyword);
            this.groupBox1.Controls.Add(this.btnUploadKeyword);
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(499, 79);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(508, 61);
            this.groupBox1.TabIndex = 140;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Keyword";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Keywowrd:";
            // 
            // txtUploadKeyword
            // 
            this.txtUploadKeyword.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUploadKeyword.Location = new System.Drawing.Point(99, 25);
            this.txtUploadKeyword.Name = "txtUploadKeyword";
            this.txtUploadKeyword.Size = new System.Drawing.Size(282, 21);
            this.txtUploadKeyword.TabIndex = 1;
            this.txtUploadKeyword.TextChanged += new System.EventHandler(this.txtUploadKeyword_TextChanged);
            // 
            // btnUploadKeyword
            // 
            this.btnUploadKeyword.BackgroundImage = global::InBoardPro.Properties.Resources.browse_btn;
            this.btnUploadKeyword.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnUploadKeyword.Location = new System.Drawing.Point(384, 21);
            this.btnUploadKeyword.Name = "btnUploadKeyword";
            this.btnUploadKeyword.Size = new System.Drawing.Size(80, 28);
            this.btnUploadKeyword.TabIndex = 0;
            this.btnUploadKeyword.UseVisualStyleBackColor = true;
            this.btnUploadKeyword.Visible = false;
            this.btnUploadKeyword.Click += new System.EventHandler(this.btnUploadKeyword_Click);
            // 
            // grpSearchWithInviteSheduling
            // 
            this.grpSearchWithInviteSheduling.BackColor = System.Drawing.Color.Transparent;
            this.grpSearchWithInviteSheduling.Controls.Add(this.dateTimePicker_End);
            this.grpSearchWithInviteSheduling.Controls.Add(this.btnShedulerStart);
            this.grpSearchWithInviteSheduling.Controls.Add(this.label89);
            this.grpSearchWithInviteSheduling.Controls.Add(this.chkboxSearchWithInviteScheduledDaily);
            this.grpSearchWithInviteSheduling.Controls.Add(this.dateTimePicker_Start_SearchWithInvite);
            this.grpSearchWithInviteSheduling.Controls.Add(this.btnScheduleForLater_SearchWithInvite);
            this.grpSearchWithInviteSheduling.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpSearchWithInviteSheduling.ForeColor = System.Drawing.Color.Black;
            this.grpSearchWithInviteSheduling.Location = new System.Drawing.Point(459, 272);
            this.grpSearchWithInviteSheduling.Name = "grpSearchWithInviteSheduling";
            this.grpSearchWithInviteSheduling.Size = new System.Drawing.Size(548, 49);
            this.grpSearchWithInviteSheduling.TabIndex = 141;
            this.grpSearchWithInviteSheduling.TabStop = false;
            this.grpSearchWithInviteSheduling.Text = "Scheduling";
            // 
            // dateTimePicker_End
            // 
            this.dateTimePicker_End.Enabled = false;
            this.dateTimePicker_End.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePicker_End.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dateTimePicker_End.Location = new System.Drawing.Point(255, 19);
            this.dateTimePicker_End.Name = "dateTimePicker_End";
            this.dateTimePicker_End.Size = new System.Drawing.Size(123, 21);
            this.dateTimePicker_End.TabIndex = 7;
            // 
            // btnShedulerStart
            // 
            this.btnShedulerStart.BackColor = System.Drawing.Color.Transparent;
            this.btnShedulerStart.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnShedulerStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnShedulerStart.Image = global::InBoardPro.Properties.Resources.Scheduler;
            this.btnShedulerStart.Location = new System.Drawing.Point(706, 17);
            this.btnShedulerStart.Name = "btnShedulerStart";
            this.btnShedulerStart.Size = new System.Drawing.Size(90, 32);
            this.btnShedulerStart.TabIndex = 6;
            this.btnShedulerStart.UseVisualStyleBackColor = false;
            this.btnShedulerStart.Visible = false;
            // 
            // label89
            // 
            this.label89.AutoSize = true;
            this.label89.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label89.ForeColor = System.Drawing.Color.Black;
            this.label89.Location = new System.Drawing.Point(10, 21);
            this.label89.Name = "label89";
            this.label89.Size = new System.Drawing.Size(91, 13);
            this.label89.TabIndex = 5;
            this.label89.Text = "Schedule Time";
            // 
            // chkboxSearchWithInviteScheduledDaily
            // 
            this.chkboxSearchWithInviteScheduledDaily.AutoSize = true;
            this.chkboxSearchWithInviteScheduledDaily.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkboxSearchWithInviteScheduledDaily.Location = new System.Drawing.Point(392, 21);
            this.chkboxSearchWithInviteScheduledDaily.Name = "chkboxSearchWithInviteScheduledDaily";
            this.chkboxSearchWithInviteScheduledDaily.Size = new System.Drawing.Size(133, 17);
            this.chkboxSearchWithInviteScheduledDaily.TabIndex = 3;
            this.chkboxSearchWithInviteScheduledDaily.Text = "Is Scheduled Daily";
            this.chkboxSearchWithInviteScheduledDaily.UseVisualStyleBackColor = true;
            this.chkboxSearchWithInviteScheduledDaily.CheckedChanged += new System.EventHandler(this.chkboxSearchWithInviteScheduledDaily_CheckedChanged);
            // 
            // dateTimePicker_Start_SearchWithInvite
            // 
            this.dateTimePicker_Start_SearchWithInvite.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePicker_Start_SearchWithInvite.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dateTimePicker_Start_SearchWithInvite.Location = new System.Drawing.Point(105, 19);
            this.dateTimePicker_Start_SearchWithInvite.Name = "dateTimePicker_Start_SearchWithInvite";
            this.dateTimePicker_Start_SearchWithInvite.Size = new System.Drawing.Size(123, 21);
            this.dateTimePicker_Start_SearchWithInvite.TabIndex = 0;
            // 
            // btnScheduleForLater_SearchWithInvite
            // 
            this.btnScheduleForLater_SearchWithInvite.BackColor = System.Drawing.Color.Transparent;
            this.btnScheduleForLater_SearchWithInvite.BackgroundImage = global::InBoardPro.Properties.Resources.Shedule_For_Later;
            this.btnScheduleForLater_SearchWithInvite.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnScheduleForLater_SearchWithInvite.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnScheduleForLater_SearchWithInvite.Location = new System.Drawing.Point(531, 15);
            this.btnScheduleForLater_SearchWithInvite.Name = "btnScheduleForLater_SearchWithInvite";
            this.btnScheduleForLater_SearchWithInvite.Size = new System.Drawing.Size(133, 32);
            this.btnScheduleForLater_SearchWithInvite.TabIndex = 1;
            this.btnScheduleForLater_SearchWithInvite.UseVisualStyleBackColor = false;
            this.btnScheduleForLater_SearchWithInvite.Visible = false;
            // 
            // gbAddConnectionLogger
            // 
            this.gbAddConnectionLogger.BackColor = System.Drawing.Color.Transparent;
            this.gbAddConnectionLogger.Controls.Add(this.lbManageConnection);
            this.gbAddConnectionLogger.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbAddConnectionLogger.Location = new System.Drawing.Point(23, 409);
            this.gbAddConnectionLogger.Name = "gbAddConnectionLogger";
            this.gbAddConnectionLogger.Size = new System.Drawing.Size(984, 148);
            this.gbAddConnectionLogger.TabIndex = 142;
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
            this.lbManageConnection.Size = new System.Drawing.Size(978, 128);
            this.lbManageConnection.TabIndex = 12;
            // 
            // frmCampaignSearchWithInvite
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1009, 564);
            this.Controls.Add(this.gbAddConnectionLogger);
            this.Controls.Add(this.grpSearchWithInviteSheduling);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox56);
            this.Controls.Add(this.grpBoxInvitesPerDay);
            this.Controls.Add(this.gbAddConnectioDelaySetting);
            this.Controls.Add(this.txtInviteMsg);
            this.Controls.Add(this.chkUseSpintax_InviteMessage);
            this.Controls.Add(this.gbCampaignSubmitAction);
            this.Controls.Add(this.grp_listofcampaign);
            this.Controls.Add(this.grp_CmpName);
            this.Controls.Add(this.grp_account);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmCampaignSearchWithInvite";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Campaign Search With Invite";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmCampaignSearchWithInvite_FormClosing);
            this.Load += new System.EventHandler(this.frmCampaignSearchWithInvite_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.frmCampaignSearchWithInvite_Paint);
            this.grp_account.ResumeLayout(false);
            this.grp_account.PerformLayout();
            this.grp_CmpName.ResumeLayout(false);
            this.grp_CmpName.PerformLayout();
            this.grp_listofcampaign.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_campaign)).EndInit();
            this.gbCampaignSubmitAction.ResumeLayout(false);
            this.gbAddConnectioDelaySetting.ResumeLayout(false);
            this.gbAddConnectioDelaySetting.PerformLayout();
            this.grpBoxInvitesPerDay.ResumeLayout(false);
            this.grpBoxInvitesPerDay.PerformLayout();
            this.groupBox56.ResumeLayout(false);
            this.groupBox56.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.grpSearchWithInviteSheduling.ResumeLayout(false);
            this.grpSearchWithInviteSheduling.PerformLayout();
            this.gbAddConnectionLogger.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grp_account;
        private System.Windows.Forms.Label lb_AccountFile;
        private System.Windows.Forms.TextBox txt_accountfilepath;
        private System.Windows.Forms.Button btn_uploadaccounts;
        private System.Windows.Forms.GroupBox grp_CmpName;
        private System.Windows.Forms.Label lb_CampaignName;
        private System.Windows.Forms.TextBox txt_CampaignName;
        private System.Windows.Forms.GroupBox grp_listofcampaign;
        private System.Windows.Forms.DataGridView dgv_campaign;
        private System.Windows.Forms.GroupBox gbCampaignSubmitAction;
        private System.Windows.Forms.Button btn_savecampaign;
        // private System.Windows.Forms.Button btn_UpdateCampaign;
        private System.Windows.Forms.TextBox txtInviteMsg;
        private System.Windows.Forms.CheckBox chkUseSpintax_InviteMessage;
        private System.Windows.Forms.GroupBox gbAddConnectioDelaySetting;
        private System.Windows.Forms.TextBox txtThreadManageConnection;
        private System.Windows.Forms.Label label107;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtSearchMaxDelay;
        private System.Windows.Forms.TextBox txtSearchMindelay;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.GroupBox grpBoxInvitesPerDay;
        private System.Windows.Forms.TextBox txt_campMaximumNoRetweet;
        private System.Windows.Forms.CheckBox chk_InvitePerDay;
        private System.Windows.Forms.Label lb_campmaxretweet;
        private System.Windows.Forms.GroupBox groupBox56;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txt_campNoOfInvitesParAc;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtUploadKeyword;
        private System.Windows.Forms.Button btnUploadKeyword;
        private System.Windows.Forms.GroupBox grpSearchWithInviteSheduling;
        private System.Windows.Forms.Label label89;
        private System.Windows.Forms.CheckBox chkboxSearchWithInviteScheduledDaily;
        private System.Windows.Forms.DateTimePicker dateTimePicker_Start_SearchWithInvite;
        private System.Windows.Forms.DateTimePicker dateTimePicker_End;
        private System.Windows.Forms.GroupBox gbAddConnectionLogger;
        private System.Windows.Forms.ListBox lbManageConnection;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnShedulerStart;
        private System.Windows.Forms.Button btnScheduleForLater_SearchWithInvite;
        private System.Windows.Forms.DataGridViewTextBoxColumn CampaignName;
        private System.Windows.Forms.DataGridViewTextBoxColumn FeatureName;
        private System.Windows.Forms.DataGridViewLinkColumn CampaignEdit;
        private System.Windows.Forms.DataGridViewImageColumn BtnOnOff;
    }
}