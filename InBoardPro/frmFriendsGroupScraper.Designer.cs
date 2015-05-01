namespace InBoardPro
{
    partial class frmFriendsGroupScraper
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmFriendsGroupScraper));
            this.gbFriendsGroupInput = new System.Windows.Forms.GroupBox();
            this.lblTotMemCount = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.cmbUser = new System.Windows.Forms.ComboBox();
            this.btnExistGroup = new System.Windows.Forms.Button();
            this.chkSelectMembers_JoinFriendsGrp = new System.Windows.Forms.CheckBox();
            this.chkMembers = new System.Windows.Forms.CheckedListBox();
            this.label66 = new System.Windows.Forms.Label();
            this.gbFriendsGroupSearchScrAction = new System.Windows.Forms.GroupBox();
            this.btnLinkedinSearch = new System.Windows.Forms.Button();
            this.btnLinkedinSearchStop = new System.Windows.Forms.Button();
            this.gbFriendsGroupLogger = new System.Windows.Forms.GroupBox();
            this.listLoggerFriendsGroup = new System.Windows.Forms.ListBox();
            this.gbFriendsGroupInput.SuspendLayout();
            this.gbFriendsGroupSearchScrAction.SuspendLayout();
            this.gbFriendsGroupLogger.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbFriendsGroupInput
            // 
            this.gbFriendsGroupInput.BackColor = System.Drawing.Color.Transparent;
            this.gbFriendsGroupInput.Controls.Add(this.lblTotMemCount);
            this.gbFriendsGroupInput.Controls.Add(this.label14);
            this.gbFriendsGroupInput.Controls.Add(this.cmbUser);
            this.gbFriendsGroupInput.Controls.Add(this.btnExistGroup);
            this.gbFriendsGroupInput.Controls.Add(this.chkSelectMembers_JoinFriendsGrp);
            this.gbFriendsGroupInput.Controls.Add(this.chkMembers);
            this.gbFriendsGroupInput.Controls.Add(this.label66);
            this.gbFriendsGroupInput.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.gbFriendsGroupInput.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbFriendsGroupInput.Location = new System.Drawing.Point(12, 12);
            this.gbFriendsGroupInput.Name = "gbFriendsGroupInput";
            this.gbFriendsGroupInput.Size = new System.Drawing.Size(674, 237);
            this.gbFriendsGroupInput.TabIndex = 122;
            this.gbFriendsGroupInput.TabStop = false;
            this.gbFriendsGroupInput.Text = "Friends Group Inputs";
            // 
            // lblTotMemCount
            // 
            this.lblTotMemCount.AutoSize = true;
            this.lblTotMemCount.BackColor = System.Drawing.Color.Transparent;
            this.lblTotMemCount.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotMemCount.Location = new System.Drawing.Point(106, 82);
            this.lblTotMemCount.Name = "lblTotMemCount";
            this.lblTotMemCount.Size = new System.Drawing.Size(24, 13);
            this.lblTotMemCount.TabIndex = 125;
            this.lblTotMemCount.Text = "(0)";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(144, 17);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(96, 13);
            this.label14.TabIndex = 115;
            this.label14.Text = "Select Account:";
            // 
            // cmbUser
            // 
            this.cmbUser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUser.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbUser.FormattingEnabled = true;
            this.cmbUser.Location = new System.Drawing.Point(147, 34);
            this.cmbUser.Margin = new System.Windows.Forms.Padding(2);
            this.cmbUser.Name = "cmbUser";
            this.cmbUser.Size = new System.Drawing.Size(447, 21);
            this.cmbUser.TabIndex = 2;
            this.cmbUser.SelectedIndexChanged += new System.EventHandler(this.cmbUser_SelectedIndexChanged);
            // 
            // btnExistGroup
            // 
            this.btnExistGroup.BackColor = System.Drawing.Color.White;
            this.btnExistGroup.BackgroundImage = global::InBoardPro.Properties.Resources.Add_friend;
            this.btnExistGroup.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnExistGroup.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnExistGroup.Font = new System.Drawing.Font("Cambria", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExistGroup.Location = new System.Drawing.Point(11, 28);
            this.btnExistGroup.Name = "btnExistGroup";
            this.btnExistGroup.Size = new System.Drawing.Size(122, 29);
            this.btnExistGroup.TabIndex = 1;
            this.btnExistGroup.UseVisualStyleBackColor = false;
            this.btnExistGroup.Click += new System.EventHandler(this.btnExistGroup_Click);
            // 
            // chkSelectMembers_JoinFriendsGrp
            // 
            this.chkSelectMembers_JoinFriendsGrp.AutoSize = true;
            this.chkSelectMembers_JoinFriendsGrp.BackColor = System.Drawing.Color.Transparent;
            this.chkSelectMembers_JoinFriendsGrp.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSelectMembers_JoinFriendsGrp.Location = new System.Drawing.Point(147, 212);
            this.chkSelectMembers_JoinFriendsGrp.Name = "chkSelectMembers_JoinFriendsGrp";
            this.chkSelectMembers_JoinFriendsGrp.Size = new System.Drawing.Size(150, 17);
            this.chkSelectMembers_JoinFriendsGrp.TabIndex = 114;
            this.chkSelectMembers_JoinFriendsGrp.Text = "Select All/Unselect All";
            this.chkSelectMembers_JoinFriendsGrp.UseVisualStyleBackColor = false;
            this.chkSelectMembers_JoinFriendsGrp.CheckedChanged += new System.EventHandler(this.chkSelectMembers_JoinFriendsGrp_CheckedChanged);
            // 
            // chkMembers
            // 
            this.chkMembers.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.chkMembers.CheckOnClick = true;
            this.chkMembers.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkMembers.FormattingEnabled = true;
            this.chkMembers.HorizontalScrollbar = true;
            this.chkMembers.Location = new System.Drawing.Point(147, 60);
            this.chkMembers.Name = "chkMembers";
            this.chkMembers.Size = new System.Drawing.Size(447, 146);
            this.chkMembers.TabIndex = 3;
            this.chkMembers.SelectedIndexChanged += new System.EventHandler(this.chkMembers_SelectedIndexChanged);
            // 
            // label66
            // 
            this.label66.AutoSize = true;
            this.label66.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label66.Location = new System.Drawing.Point(58, 60);
            this.label66.Name = "label66";
            this.label66.Size = new System.Drawing.Size(83, 13);
            this.label66.TabIndex = 110;
            this.label66.Text = "Friend -->>>";
            // 
            // gbFriendsGroupSearchScrAction
            // 
            this.gbFriendsGroupSearchScrAction.BackColor = System.Drawing.Color.Transparent;
            this.gbFriendsGroupSearchScrAction.Controls.Add(this.btnLinkedinSearch);
            this.gbFriendsGroupSearchScrAction.Controls.Add(this.btnLinkedinSearchStop);
            this.gbFriendsGroupSearchScrAction.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbFriendsGroupSearchScrAction.ForeColor = System.Drawing.Color.Black;
            this.gbFriendsGroupSearchScrAction.Location = new System.Drawing.Point(12, 255);
            this.gbFriendsGroupSearchScrAction.Name = "gbFriendsGroupSearchScrAction";
            this.gbFriendsGroupSearchScrAction.Size = new System.Drawing.Size(674, 64);
            this.gbFriendsGroupSearchScrAction.TabIndex = 130;
            this.gbFriendsGroupSearchScrAction.TabStop = false;
            this.gbFriendsGroupSearchScrAction.Text = "Submit Action";
            // 
            // btnLinkedinSearch
            // 
            this.btnLinkedinSearch.BackgroundImage = global::InBoardPro.Properties.Resources.search;
            this.btnLinkedinSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnLinkedinSearch.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnLinkedinSearch.Location = new System.Drawing.Point(147, 19);
            this.btnLinkedinSearch.Margin = new System.Windows.Forms.Padding(2);
            this.btnLinkedinSearch.Name = "btnLinkedinSearch";
            this.btnLinkedinSearch.Size = new System.Drawing.Size(120, 31);
            this.btnLinkedinSearch.TabIndex = 111;
            this.btnLinkedinSearch.UseVisualStyleBackColor = true;
            this.btnLinkedinSearch.Click += new System.EventHandler(this.btnLinkedinSearch_Click);
            // 
            // btnLinkedinSearchStop
            // 
            this.btnLinkedinSearchStop.BackgroundImage = global::InBoardPro.Properties.Resources.stop;
            this.btnLinkedinSearchStop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnLinkedinSearchStop.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnLinkedinSearchStop.Location = new System.Drawing.Point(286, 19);
            this.btnLinkedinSearchStop.Name = "btnLinkedinSearchStop";
            this.btnLinkedinSearchStop.Size = new System.Drawing.Size(115, 30);
            this.btnLinkedinSearchStop.TabIndex = 112;
            this.btnLinkedinSearchStop.UseVisualStyleBackColor = true;
            this.btnLinkedinSearchStop.Click += new System.EventHandler(this.btnLinkedinSearchStop_Click);
            // 
            // gbFriendsGroupLogger
            // 
            this.gbFriendsGroupLogger.BackColor = System.Drawing.Color.Transparent;
            this.gbFriendsGroupLogger.Controls.Add(this.listLoggerFriendsGroup);
            this.gbFriendsGroupLogger.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbFriendsGroupLogger.Location = new System.Drawing.Point(12, 320);
            this.gbFriendsGroupLogger.Name = "gbFriendsGroupLogger";
            this.gbFriendsGroupLogger.Size = new System.Drawing.Size(674, 146);
            this.gbFriendsGroupLogger.TabIndex = 142;
            this.gbFriendsGroupLogger.TabStop = false;
            this.gbFriendsGroupLogger.Text = "Logger";
            // 
            // listLoggerFriendsGroup
            // 
            this.listLoggerFriendsGroup.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listLoggerFriendsGroup.FormattingEnabled = true;
            this.listLoggerFriendsGroup.Location = new System.Drawing.Point(7, 22);
            this.listLoggerFriendsGroup.Name = "listLoggerFriendsGroup";
            this.listLoggerFriendsGroup.ScrollAlwaysVisible = true;
            this.listLoggerFriendsGroup.Size = new System.Drawing.Size(655, 108);
            this.listLoggerFriendsGroup.TabIndex = 8;
            // 
            // frmFriendsGroupScraper
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(698, 471);
            this.Controls.Add(this.gbFriendsGroupLogger);
            this.Controls.Add(this.gbFriendsGroupSearchScrAction);
            this.Controls.Add(this.gbFriendsGroupInput);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmFriendsGroupScraper";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Friends Group Scraper";
            this.Load += new System.EventHandler(this.frmFriendsGroupScraper_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.frmFriendsGroupScraper_Paint);
            this.gbFriendsGroupInput.ResumeLayout(false);
            this.gbFriendsGroupInput.PerformLayout();
            this.gbFriendsGroupSearchScrAction.ResumeLayout(false);
            this.gbFriendsGroupLogger.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbFriendsGroupInput;
        private System.Windows.Forms.Label lblTotMemCount;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ComboBox cmbUser;
        private System.Windows.Forms.Button btnExistGroup;
        private System.Windows.Forms.CheckBox chkSelectMembers_JoinFriendsGrp;
        private System.Windows.Forms.CheckedListBox chkMembers;
        private System.Windows.Forms.Label label66;
        private System.Windows.Forms.GroupBox gbFriendsGroupSearchScrAction;
        private System.Windows.Forms.Button btnLinkedinSearch;
        private System.Windows.Forms.Button btnLinkedinSearchStop;
        private System.Windows.Forms.GroupBox gbFriendsGroupLogger;
        private System.Windows.Forms.ListBox listLoggerFriendsGroup;
    }
}