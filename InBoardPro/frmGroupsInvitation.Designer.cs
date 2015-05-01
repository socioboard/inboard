namespace InBoardPro
{
    partial class frmGroupsInvitation
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmGroupsInvitation));
            this.gbFriendsGroupInput = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnEmails = new System.Windows.Forms.Button();
            this.txtEmails = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnAddPendingGroups = new System.Windows.Forms.Button();
            this.chkOwnGroup = new System.Windows.Forms.CheckedListBox();
            this.lblTotGroupCount = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.cmbUserPendingGroup = new System.Windows.Forms.ComboBox();
            this.chkSelectPendingGrp = new System.Windows.Forms.CheckBox();
            this.label66 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.gbFriendsGroupLogger = new System.Windows.Forms.GroupBox();
            this.lstLogInviteGroups = new System.Windows.Forms.ListBox();
            this.gbFriendsGroupAction = new System.Windows.Forms.GroupBox();
            this.btnRemovePendingGroup = new System.Windows.Forms.Button();
            this.btnStopPendingGroup = new System.Windows.Forms.Button();
            this.gbJoinGroupUrlDelaySetting = new System.Windows.Forms.GroupBox();
            this.txtNoOfThread = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label113 = new System.Windows.Forms.Label();
            this.txtPendingGroupMaxDelay = new System.Windows.Forms.TextBox();
            this.label92 = new System.Windows.Forms.Label();
            this.label93 = new System.Windows.Forms.Label();
            this.txtPendingGroupMinDelay = new System.Windows.Forms.TextBox();
            this.gbFriendsGroupInput.SuspendLayout();
            this.gbFriendsGroupLogger.SuspendLayout();
            this.gbFriendsGroupAction.SuspendLayout();
            this.gbJoinGroupUrlDelaySetting.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbFriendsGroupInput
            // 
            this.gbFriendsGroupInput.BackColor = System.Drawing.Color.Transparent;
            this.gbFriendsGroupInput.Controls.Add(this.label4);
            this.gbFriendsGroupInput.Controls.Add(this.btnEmails);
            this.gbFriendsGroupInput.Controls.Add(this.txtEmails);
            this.gbFriendsGroupInput.Controls.Add(this.label3);
            this.gbFriendsGroupInput.Controls.Add(this.btnAddPendingGroups);
            this.gbFriendsGroupInput.Controls.Add(this.chkOwnGroup);
            this.gbFriendsGroupInput.Controls.Add(this.lblTotGroupCount);
            this.gbFriendsGroupInput.Controls.Add(this.label14);
            this.gbFriendsGroupInput.Controls.Add(this.cmbUserPendingGroup);
            this.gbFriendsGroupInput.Controls.Add(this.chkSelectPendingGrp);
            this.gbFriendsGroupInput.Controls.Add(this.label66);
            this.gbFriendsGroupInput.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.gbFriendsGroupInput.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbFriendsGroupInput.Location = new System.Drawing.Point(23, 12);
            this.gbFriendsGroupInput.Name = "gbFriendsGroupInput";
            this.gbFriendsGroupInput.Size = new System.Drawing.Size(733, 244);
            this.gbFriendsGroupInput.TabIndex = 122;
            this.gbFriendsGroupInput.TabStop = false;
            this.gbFriendsGroupInput.Text = "Add Group";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(428, 218);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(134, 13);
            this.label4.TabIndex = 138;
            this.label4.Text = "Format: abc@xyz.com";
            // 
            // btnEmails
            // 
            this.btnEmails.BackColor = System.Drawing.Color.White;
            this.btnEmails.BackgroundImage = global::InBoardPro.Properties.Resources.browse_btn;
            this.btnEmails.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnEmails.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEmails.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnEmails.ForeColor = System.Drawing.Color.Black;
            this.btnEmails.Location = new System.Drawing.Point(579, 191);
            this.btnEmails.Name = "btnEmails";
            this.btnEmails.Size = new System.Drawing.Size(77, 27);
            this.btnEmails.TabIndex = 136;
            this.btnEmails.UseVisualStyleBackColor = false;
            this.btnEmails.Click += new System.EventHandler(this.btnEmails_Click);
            // 
            // txtEmails
            // 
            this.txtEmails.BackColor = System.Drawing.Color.White;
            this.txtEmails.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEmails.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEmails.Location = new System.Drawing.Point(178, 194);
            this.txtEmails.Name = "txtEmails";
            this.txtEmails.ReadOnly = true;
            this.txtEmails.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtEmails.Size = new System.Drawing.Size(388, 21);
            this.txtEmails.TabIndex = 135;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(15, 196);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(153, 13);
            this.label3.TabIndex = 137;
            this.label3.Text = "Emails to send Invitation:";
            // 
            // btnAddPendingGroups
            // 
            this.btnAddPendingGroups.BackColor = System.Drawing.Color.White;
            this.btnAddPendingGroups.BackgroundImage = global::InBoardPro.Properties.Resources.AddPendingGroups;
            this.btnAddPendingGroups.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnAddPendingGroups.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnAddPendingGroups.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAddPendingGroups.Font = new System.Drawing.Font("Cambria", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddPendingGroups.Location = new System.Drawing.Point(48, 30);
            this.btnAddPendingGroups.Name = "btnAddPendingGroups";
            this.btnAddPendingGroups.Size = new System.Drawing.Size(120, 30);
            this.btnAddPendingGroups.TabIndex = 127;
            this.btnAddPendingGroups.UseVisualStyleBackColor = false;
            this.btnAddPendingGroups.Click += new System.EventHandler(this.btnAddPendingGroups_Click);
            // 
            // chkOwnGroup
            // 
            this.chkOwnGroup.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.chkOwnGroup.CheckOnClick = true;
            this.chkOwnGroup.Cursor = System.Windows.Forms.Cursors.Default;
            this.chkOwnGroup.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkOwnGroup.FormattingEnabled = true;
            this.chkOwnGroup.HorizontalScrollbar = true;
            this.chkOwnGroup.Location = new System.Drawing.Point(178, 64);
            this.chkOwnGroup.Name = "chkOwnGroup";
            this.chkOwnGroup.Size = new System.Drawing.Size(478, 98);
            this.chkOwnGroup.TabIndex = 126;
            this.chkOwnGroup.SelectedIndexChanged += new System.EventHandler(this.chkPendingGroup_SelectedIndexChanged);
            // 
            // lblTotGroupCount
            // 
            this.lblTotGroupCount.AutoSize = true;
            this.lblTotGroupCount.BackColor = System.Drawing.Color.Transparent;
            this.lblTotGroupCount.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotGroupCount.Location = new System.Drawing.Point(137, 82);
            this.lblTotGroupCount.Name = "lblTotGroupCount";
            this.lblTotGroupCount.Size = new System.Drawing.Size(24, 13);
            this.lblTotGroupCount.TabIndex = 125;
            this.lblTotGroupCount.Text = "(0)";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(181, 17);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(96, 13);
            this.label14.TabIndex = 115;
            this.label14.Text = "Select Account:";
            // 
            // cmbUserPendingGroup
            // 
            this.cmbUserPendingGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUserPendingGroup.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbUserPendingGroup.FormattingEnabled = true;
            this.cmbUserPendingGroup.Location = new System.Drawing.Point(178, 34);
            this.cmbUserPendingGroup.Margin = new System.Windows.Forms.Padding(2);
            this.cmbUserPendingGroup.Name = "cmbUserPendingGroup";
            this.cmbUserPendingGroup.Size = new System.Drawing.Size(478, 21);
            this.cmbUserPendingGroup.TabIndex = 2;
            this.cmbUserPendingGroup.SelectedIndexChanged += new System.EventHandler(this.cmbUserPendingGroup_SelectedIndexChanged);
            // 
            // chkSelectPendingGrp
            // 
            this.chkSelectPendingGrp.AutoSize = true;
            this.chkSelectPendingGrp.BackColor = System.Drawing.Color.Transparent;
            this.chkSelectPendingGrp.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSelectPendingGrp.Location = new System.Drawing.Point(178, 168);
            this.chkSelectPendingGrp.Name = "chkSelectPendingGrp";
            this.chkSelectPendingGrp.Size = new System.Drawing.Size(150, 17);
            this.chkSelectPendingGrp.TabIndex = 114;
            this.chkSelectPendingGrp.Text = "Select All/Unselect All";
            this.chkSelectPendingGrp.UseVisualStyleBackColor = false;
            this.chkSelectPendingGrp.CheckedChanged += new System.EventHandler(this.chkSelectPendingGrp_CheckedChanged);
            // 
            // label66
            // 
            this.label66.AutoSize = true;
            this.label66.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label66.Location = new System.Drawing.Point(44, 64);
            this.label66.Name = "label66";
            this.label66.Size = new System.Drawing.Size(118, 13);
            this.label66.TabIndex = 110;
            this.label66.Text = "Own Groups -->>>";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(703, 314);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 128;
            this.label1.Text = "label1";
            this.label1.Visible = false;
            // 
            // gbFriendsGroupLogger
            // 
            this.gbFriendsGroupLogger.BackColor = System.Drawing.Color.Transparent;
            this.gbFriendsGroupLogger.Controls.Add(this.lstLogInviteGroups);
            this.gbFriendsGroupLogger.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbFriendsGroupLogger.Location = new System.Drawing.Point(23, 400);
            this.gbFriendsGroupLogger.Name = "gbFriendsGroupLogger";
            this.gbFriendsGroupLogger.Size = new System.Drawing.Size(733, 145);
            this.gbFriendsGroupLogger.TabIndex = 123;
            this.gbFriendsGroupLogger.TabStop = false;
            this.gbFriendsGroupLogger.Text = "Logger";
            // 
            // lstLogInviteGroups
            // 
            this.lstLogInviteGroups.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstLogInviteGroups.FormattingEnabled = true;
            this.lstLogInviteGroups.HorizontalScrollbar = true;
            this.lstLogInviteGroups.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.lstLogInviteGroups.Location = new System.Drawing.Point(10, 21);
            this.lstLogInviteGroups.Name = "lstLogInviteGroups";
            this.lstLogInviteGroups.ScrollAlwaysVisible = true;
            this.lstLogInviteGroups.Size = new System.Drawing.Size(710, 108);
            this.lstLogInviteGroups.TabIndex = 8;
            // 
            // gbFriendsGroupAction
            // 
            this.gbFriendsGroupAction.BackColor = System.Drawing.Color.Transparent;
            this.gbFriendsGroupAction.Controls.Add(this.btnRemovePendingGroup);
            this.gbFriendsGroupAction.Controls.Add(this.btnStopPendingGroup);
            this.gbFriendsGroupAction.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbFriendsGroupAction.Location = new System.Drawing.Point(23, 325);
            this.gbFriendsGroupAction.Name = "gbFriendsGroupAction";
            this.gbFriendsGroupAction.Size = new System.Drawing.Size(733, 65);
            this.gbFriendsGroupAction.TabIndex = 124;
            this.gbFriendsGroupAction.TabStop = false;
            this.gbFriendsGroupAction.Text = "Submit Action";
            // 
            // btnRemovePendingGroup
            // 
            this.btnRemovePendingGroup.BackColor = System.Drawing.Color.Silver;
            this.btnRemovePendingGroup.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnRemovePendingGroup.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnRemovePendingGroup.Font = new System.Drawing.Font("Cambria", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRemovePendingGroup.Image = global::InBoardPro.Properties.Resources.InviteGroups;
            this.btnRemovePendingGroup.Location = new System.Drawing.Point(178, 20);
            this.btnRemovePendingGroup.Name = "btnRemovePendingGroup";
            this.btnRemovePendingGroup.Size = new System.Drawing.Size(146, 30);
            this.btnRemovePendingGroup.TabIndex = 6;
            this.btnRemovePendingGroup.UseVisualStyleBackColor = false;
            this.btnRemovePendingGroup.Click += new System.EventHandler(this.btnRemovePendingGroup_Click);
            // 
            // btnStopPendingGroup
            // 
            this.btnStopPendingGroup.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnStopPendingGroup.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnStopPendingGroup.Image = global::InBoardPro.Properties.Resources.stop;
            this.btnStopPendingGroup.Location = new System.Drawing.Point(347, 20);
            this.btnStopPendingGroup.Name = "btnStopPendingGroup";
            this.btnStopPendingGroup.Size = new System.Drawing.Size(126, 30);
            this.btnStopPendingGroup.TabIndex = 109;
            this.btnStopPendingGroup.UseVisualStyleBackColor = true;
            this.btnStopPendingGroup.Click += new System.EventHandler(this.btnStopPendingGroup_Click);
            // 
            // gbJoinGroupUrlDelaySetting
            // 
            this.gbJoinGroupUrlDelaySetting.BackColor = System.Drawing.Color.Transparent;
            this.gbJoinGroupUrlDelaySetting.Controls.Add(this.txtNoOfThread);
            this.gbJoinGroupUrlDelaySetting.Controls.Add(this.label2);
            this.gbJoinGroupUrlDelaySetting.Controls.Add(this.label113);
            this.gbJoinGroupUrlDelaySetting.Controls.Add(this.txtPendingGroupMaxDelay);
            this.gbJoinGroupUrlDelaySetting.Controls.Add(this.label92);
            this.gbJoinGroupUrlDelaySetting.Controls.Add(this.label93);
            this.gbJoinGroupUrlDelaySetting.Controls.Add(this.txtPendingGroupMinDelay);
            this.gbJoinGroupUrlDelaySetting.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbJoinGroupUrlDelaySetting.Location = new System.Drawing.Point(23, 262);
            this.gbJoinGroupUrlDelaySetting.Name = "gbJoinGroupUrlDelaySetting";
            this.gbJoinGroupUrlDelaySetting.Size = new System.Drawing.Size(733, 49);
            this.gbJoinGroupUrlDelaySetting.TabIndex = 139;
            this.gbJoinGroupUrlDelaySetting.TabStop = false;
            this.gbJoinGroupUrlDelaySetting.Text = "Setting";
            // 
            // txtNoOfThread
            // 
            this.txtNoOfThread.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNoOfThread.Location = new System.Drawing.Point(152, 19);
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
            this.label113.Size = new System.Drawing.Size(311, 13);
            this.label113.TabIndex = 126;
            this.label113.Text = "(Its randomly delay between both values in seconds)";
            // 
            // txtPendingGroupMaxDelay
            // 
            this.txtPendingGroupMaxDelay.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPendingGroupMaxDelay.Location = new System.Drawing.Point(343, 18);
            this.txtPendingGroupMaxDelay.Name = "txtPendingGroupMaxDelay";
            this.txtPendingGroupMaxDelay.Size = new System.Drawing.Size(43, 21);
            this.txtPendingGroupMaxDelay.TabIndex = 119;
            this.txtPendingGroupMaxDelay.Text = "25";
            this.txtPendingGroupMaxDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
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
            // txtPendingGroupMinDelay
            // 
            this.txtPendingGroupMinDelay.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPendingGroupMinDelay.Location = new System.Drawing.Point(261, 18);
            this.txtPendingGroupMinDelay.Name = "txtPendingGroupMinDelay";
            this.txtPendingGroupMinDelay.Size = new System.Drawing.Size(44, 21);
            this.txtPendingGroupMinDelay.TabIndex = 118;
            this.txtPendingGroupMinDelay.Text = "20";
            this.txtPendingGroupMinDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // frmGroupsInvitation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(775, 551);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.gbJoinGroupUrlDelaySetting);
            this.Controls.Add(this.gbFriendsGroupAction);
            this.Controls.Add(this.gbFriendsGroupLogger);
            this.Controls.Add(this.gbFriendsGroupInput);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmGroupsInvitation";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Group Invitation";
            this.Load += new System.EventHandler(this.frmGroupsInvitation_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.frmRemovePendingGroups_Paint);
            this.gbFriendsGroupInput.ResumeLayout(false);
            this.gbFriendsGroupInput.PerformLayout();
            this.gbFriendsGroupLogger.ResumeLayout(false);
            this.gbFriendsGroupAction.ResumeLayout(false);
            this.gbJoinGroupUrlDelaySetting.ResumeLayout(false);
            this.gbJoinGroupUrlDelaySetting.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbFriendsGroupInput;
        private System.Windows.Forms.Label lblTotGroupCount;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ComboBox cmbUserPendingGroup;
        private System.Windows.Forms.CheckBox chkSelectPendingGrp;
        private System.Windows.Forms.Label label66;
        private System.Windows.Forms.CheckedListBox chkOwnGroup;
        private System.Windows.Forms.Button btnAddPendingGroups;
        private System.Windows.Forms.GroupBox gbFriendsGroupLogger;
        private System.Windows.Forms.ListBox lstLogInviteGroups;
        private System.Windows.Forms.GroupBox gbFriendsGroupAction;
        private System.Windows.Forms.Button btnRemovePendingGroup;
        private System.Windows.Forms.Button btnStopPendingGroup;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox gbJoinGroupUrlDelaySetting;
        private System.Windows.Forms.TextBox txtNoOfThread;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label113;
        private System.Windows.Forms.TextBox txtPendingGroupMaxDelay;
        private System.Windows.Forms.Label label92;
        private System.Windows.Forms.Label label93;
        private System.Windows.Forms.TextBox txtPendingGroupMinDelay;
        private System.Windows.Forms.Button btnEmails;
        private System.Windows.Forms.TextBox txtEmails;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}