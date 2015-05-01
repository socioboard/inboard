namespace InBoardPro
{
    partial class FrmSearchWithSalesNavigator
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSearchWithSalesNavigator));
            this.grpBox_CompanySearch = new System.Windows.Forms.GroupBox();
            this.chkAllFortune = new System.Windows.Forms.CheckBox();
            this.chkAllFolowers = new System.Windows.Forms.CheckBox();
            this.chkAllCompanySize = new System.Windows.Forms.CheckBox();
            this.chkAllIndustry = new System.Windows.Forms.CheckBox();
            this.txtOtherLocation = new System.Windows.Forms.TextBox();
            this.txtKeyword = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkedListFortune = new System.Windows.Forms.CheckedListBox();
            this.lblFortune = new System.Windows.Forms.Label();
            this.checkedListJobOpportunity = new System.Windows.Forms.CheckedListBox();
            this.lblJobOpportunity = new System.Windows.Forms.Label();
            this.checkedListFollowers = new System.Windows.Forms.CheckedListBox();
            this.lblNoOffollowers = new System.Windows.Forms.Label();
            this.checkedListCompanySize = new System.Windows.Forms.CheckedListBox();
            this.lblcompanySize = new System.Windows.Forms.Label();
            this.checkedListIndustry = new System.Windows.Forms.CheckedListBox();
            this.lblIndustry = new System.Windows.Forms.Label();
            this.combScraperLocation = new System.Windows.Forms.ComboBox();
            this.label35 = new System.Windows.Forms.Label();
            this.comboBoxemail = new System.Windows.Forms.ComboBox();
            this.lblEmail = new System.Windows.Forms.Label();
            this.gbCompSearchFilterLogger = new System.Windows.Forms.GroupBox();
            this.lstLoglinkdinScarper = new System.Windows.Forms.ListBox();
            this.btnStopScraper = new System.Windows.Forms.Button();
            this.btnSearchScraper = new System.Windows.Forms.Button();
            this.btnSearchNewScraper = new System.Windows.Forms.Button();
            this.gbCompSearchFilterAction = new System.Windows.Forms.GroupBox();
            this.grpBox_CompanySearch.SuspendLayout();
            this.gbCompSearchFilterLogger.SuspendLayout();
            this.gbCompSearchFilterAction.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpBox_CompanySearch
            // 
            this.grpBox_CompanySearch.BackColor = System.Drawing.Color.Transparent;
            this.grpBox_CompanySearch.Controls.Add(this.chkAllFortune);
            this.grpBox_CompanySearch.Controls.Add(this.chkAllFolowers);
            this.grpBox_CompanySearch.Controls.Add(this.chkAllCompanySize);
            this.grpBox_CompanySearch.Controls.Add(this.chkAllIndustry);
            this.grpBox_CompanySearch.Controls.Add(this.txtOtherLocation);
            this.grpBox_CompanySearch.Controls.Add(this.txtKeyword);
            this.grpBox_CompanySearch.Controls.Add(this.label1);
            this.grpBox_CompanySearch.Controls.Add(this.checkedListFortune);
            this.grpBox_CompanySearch.Controls.Add(this.lblFortune);
            this.grpBox_CompanySearch.Controls.Add(this.checkedListJobOpportunity);
            this.grpBox_CompanySearch.Controls.Add(this.lblJobOpportunity);
            this.grpBox_CompanySearch.Controls.Add(this.checkedListFollowers);
            this.grpBox_CompanySearch.Controls.Add(this.lblNoOffollowers);
            this.grpBox_CompanySearch.Controls.Add(this.checkedListCompanySize);
            this.grpBox_CompanySearch.Controls.Add(this.lblcompanySize);
            this.grpBox_CompanySearch.Controls.Add(this.checkedListIndustry);
            this.grpBox_CompanySearch.Controls.Add(this.lblIndustry);
            this.grpBox_CompanySearch.Controls.Add(this.combScraperLocation);
            this.grpBox_CompanySearch.Controls.Add(this.label35);
            this.grpBox_CompanySearch.Controls.Add(this.comboBoxemail);
            this.grpBox_CompanySearch.Controls.Add(this.lblEmail);
            this.grpBox_CompanySearch.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpBox_CompanySearch.Location = new System.Drawing.Point(27, 17);
            this.grpBox_CompanySearch.Name = "grpBox_CompanySearch";
            this.grpBox_CompanySearch.Size = new System.Drawing.Size(754, 341);
            this.grpBox_CompanySearch.TabIndex = 0;
            this.grpBox_CompanySearch.TabStop = false;
            this.grpBox_CompanySearch.Text = "Getting Company With Filter";
            // 
            // chkAllFortune
            // 
            this.chkAllFortune.AutoSize = true;
            this.chkAllFortune.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAllFortune.Location = new System.Drawing.Point(471, 241);
            this.chkAllFortune.Name = "chkAllFortune";
            this.chkAllFortune.Size = new System.Drawing.Size(40, 17);
            this.chkAllFortune.TabIndex = 126;
            this.chkAllFortune.Text = "All";
            this.chkAllFortune.UseVisualStyleBackColor = true;
            this.chkAllFortune.CheckedChanged += new System.EventHandler(this.chkAllFortune_CheckedChanged);
            // 
            // chkAllFolowers
            // 
            this.chkAllFolowers.AutoSize = true;
            this.chkAllFolowers.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAllFolowers.Location = new System.Drawing.Point(108, 242);
            this.chkAllFolowers.Name = "chkAllFolowers";
            this.chkAllFolowers.Size = new System.Drawing.Size(40, 17);
            this.chkAllFolowers.TabIndex = 125;
            this.chkAllFolowers.Text = "All";
            this.chkAllFolowers.UseVisualStyleBackColor = true;
            this.chkAllFolowers.CheckedChanged += new System.EventHandler(this.chkAllFolowers_CheckedChanged);
            // 
            // chkAllCompanySize
            // 
            this.chkAllCompanySize.AutoSize = true;
            this.chkAllCompanySize.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAllCompanySize.Location = new System.Drawing.Point(471, 118);
            this.chkAllCompanySize.Name = "chkAllCompanySize";
            this.chkAllCompanySize.Size = new System.Drawing.Size(40, 17);
            this.chkAllCompanySize.TabIndex = 124;
            this.chkAllCompanySize.Text = "All";
            this.chkAllCompanySize.UseVisualStyleBackColor = true;
            this.chkAllCompanySize.CheckedChanged += new System.EventHandler(this.chkAllCompanySize_CheckedChanged);
            // 
            // chkAllIndustry
            // 
            this.chkAllIndustry.AutoSize = true;
            this.chkAllIndustry.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAllIndustry.Location = new System.Drawing.Point(108, 154);
            this.chkAllIndustry.Name = "chkAllIndustry";
            this.chkAllIndustry.Size = new System.Drawing.Size(40, 17);
            this.chkAllIndustry.TabIndex = 123;
            this.chkAllIndustry.Text = "All";
            this.chkAllIndustry.UseVisualStyleBackColor = true;
            this.chkAllIndustry.CheckedChanged += new System.EventHandler(this.chkAllIndustry_CheckedChanged);
            // 
            // txtOtherLocation
            // 
            this.txtOtherLocation.Enabled = false;
            this.txtOtherLocation.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOtherLocation.Location = new System.Drawing.Point(470, 50);
            this.txtOtherLocation.Name = "txtOtherLocation";
            this.txtOtherLocation.Size = new System.Drawing.Size(247, 21);
            this.txtOtherLocation.TabIndex = 121;
            // 
            // txtKeyword
            // 
            this.txtKeyword.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtKeyword.Location = new System.Drawing.Point(108, 53);
            this.txtKeyword.Name = "txtKeyword";
            this.txtKeyword.Size = new System.Drawing.Size(247, 21);
            this.txtKeyword.TabIndex = 120;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(21, 53);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 119;
            this.label1.Text = "Keyword:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // checkedListFortune
            // 
            this.checkedListFortune.CheckOnClick = true;
            this.checkedListFortune.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkedListFortune.FormattingEnabled = true;
            this.checkedListFortune.Location = new System.Drawing.Point(470, 264);
            this.checkedListFortune.Name = "checkedListFortune";
            this.checkedListFortune.Size = new System.Drawing.Size(247, 52);
            this.checkedListFortune.TabIndex = 118;
            this.checkedListFortune.SelectedIndexChanged += new System.EventHandler(this.checkedListFortune_SelectedIndexChanged);
            // 
            // lblFortune
            // 
            this.lblFortune.AutoSize = true;
            this.lblFortune.BackColor = System.Drawing.Color.Transparent;
            this.lblFortune.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFortune.Location = new System.Drawing.Point(405, 243);
            this.lblFortune.Name = "lblFortune";
            this.lblFortune.Size = new System.Drawing.Size(55, 13);
            this.lblFortune.TabIndex = 117;
            this.lblFortune.Text = "Fortune:";
            // 
            // checkedListJobOpportunity
            // 
            this.checkedListJobOpportunity.CheckOnClick = true;
            this.checkedListJobOpportunity.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkedListJobOpportunity.FormattingEnabled = true;
            this.checkedListJobOpportunity.Location = new System.Drawing.Point(108, 83);
            this.checkedListJobOpportunity.Name = "checkedListJobOpportunity";
            this.checkedListJobOpportunity.Size = new System.Drawing.Size(247, 52);
            this.checkedListJobOpportunity.TabIndex = 116;
            // 
            // lblJobOpportunity
            // 
            this.lblJobOpportunity.AutoSize = true;
            this.lblJobOpportunity.BackColor = System.Drawing.Color.Transparent;
            this.lblJobOpportunity.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblJobOpportunity.Location = new System.Drawing.Point(7, 82);
            this.lblJobOpportunity.Name = "lblJobOpportunity";
            this.lblJobOpportunity.Size = new System.Drawing.Size(102, 13);
            this.lblJobOpportunity.TabIndex = 115;
            this.lblJobOpportunity.Text = "Job Opportunity:";
            // 
            // checkedListFollowers
            // 
            this.checkedListFollowers.CheckOnClick = true;
            this.checkedListFollowers.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkedListFollowers.FormattingEnabled = true;
            this.checkedListFollowers.Location = new System.Drawing.Point(108, 265);
            this.checkedListFollowers.Name = "checkedListFollowers";
            this.checkedListFollowers.Size = new System.Drawing.Size(247, 52);
            this.checkedListFollowers.TabIndex = 114;
            this.checkedListFollowers.SelectedIndexChanged += new System.EventHandler(this.checkedListFollowers_SelectedIndexChanged);
            // 
            // lblNoOffollowers
            // 
            this.lblNoOffollowers.AutoSize = true;
            this.lblNoOffollowers.BackColor = System.Drawing.Color.Transparent;
            this.lblNoOffollowers.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNoOffollowers.Location = new System.Drawing.Point(8, 242);
            this.lblNoOffollowers.Name = "lblNoOffollowers";
            this.lblNoOffollowers.Size = new System.Drawing.Size(101, 13);
            this.lblNoOffollowers.TabIndex = 113;
            this.lblNoOffollowers.Text = "No Of Followers:";
            // 
            // checkedListCompanySize
            // 
            this.checkedListCompanySize.CheckOnClick = true;
            this.checkedListCompanySize.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkedListCompanySize.FormattingEnabled = true;
            this.checkedListCompanySize.Location = new System.Drawing.Point(469, 154);
            this.checkedListCompanySize.Name = "checkedListCompanySize";
            this.checkedListCompanySize.Size = new System.Drawing.Size(247, 52);
            this.checkedListCompanySize.TabIndex = 112;
            this.checkedListCompanySize.SelectedIndexChanged += new System.EventHandler(this.checkedListCompanySize_SelectedIndexChanged);
            // 
            // lblcompanySize
            // 
            this.lblcompanySize.AutoSize = true;
            this.lblcompanySize.BackColor = System.Drawing.Color.Transparent;
            this.lblcompanySize.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblcompanySize.Location = new System.Drawing.Point(374, 122);
            this.lblcompanySize.Name = "lblcompanySize";
            this.lblcompanySize.Size = new System.Drawing.Size(91, 13);
            this.lblcompanySize.TabIndex = 111;
            this.lblcompanySize.Text = "CompanySize:";
            // 
            // checkedListIndustry
            // 
            this.checkedListIndustry.CheckOnClick = true;
            this.checkedListIndustry.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkedListIndustry.FormattingEnabled = true;
            this.checkedListIndustry.Location = new System.Drawing.Point(108, 177);
            this.checkedListIndustry.Name = "checkedListIndustry";
            this.checkedListIndustry.Size = new System.Drawing.Size(247, 52);
            this.checkedListIndustry.TabIndex = 110;
            this.checkedListIndustry.SelectedIndexChanged += new System.EventHandler(this.checkedListIndustry_SelectedIndexChanged);
            // 
            // lblIndustry
            // 
            this.lblIndustry.AutoSize = true;
            this.lblIndustry.BackColor = System.Drawing.Color.Transparent;
            this.lblIndustry.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIndustry.Location = new System.Drawing.Point(48, 153);
            this.lblIndustry.Name = "lblIndustry";
            this.lblIndustry.Size = new System.Drawing.Size(60, 13);
            this.lblIndustry.TabIndex = 109;
            this.lblIndustry.Text = "Industry:";
            // 
            // combScraperLocation
            // 
            this.combScraperLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combScraperLocation.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.combScraperLocation.FormattingEnabled = true;
            this.combScraperLocation.Items.AddRange(new object[] {
            "All"});
            this.combScraperLocation.Location = new System.Drawing.Point(469, 20);
            this.combScraperLocation.Margin = new System.Windows.Forms.Padding(2);
            this.combScraperLocation.Name = "combScraperLocation";
            this.combScraperLocation.Size = new System.Drawing.Size(247, 21);
            this.combScraperLocation.TabIndex = 107;
            this.combScraperLocation.SelectedIndexChanged += new System.EventHandler(this.combScraperLocation_SelectedIndexChanged);
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.BackColor = System.Drawing.Color.Transparent;
            this.label35.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label35.Location = new System.Drawing.Point(406, 24);
            this.label35.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(59, 13);
            this.label35.TabIndex = 108;
            this.label35.Text = "Location:";
            this.label35.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // comboBoxemail
            // 
            this.comboBoxemail.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxemail.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxemail.FormattingEnabled = true;
            this.comboBoxemail.Location = new System.Drawing.Point(108, 22);
            this.comboBoxemail.Name = "comboBoxemail";
            this.comboBoxemail.Size = new System.Drawing.Size(247, 21);
            this.comboBoxemail.TabIndex = 105;
            this.comboBoxemail.SelectedIndexChanged += new System.EventHandler(this.comboBoxemail_SelectedIndexChanged);
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.BackColor = System.Drawing.Color.Transparent;
            this.lblEmail.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmail.Location = new System.Drawing.Point(16, 28);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(92, 13);
            this.lblEmail.TabIndex = 106;
            this.lblEmail.Text = "SelectEmailId*";
            // 
            // gbCompSearchFilterLogger
            // 
            this.gbCompSearchFilterLogger.BackColor = System.Drawing.Color.Transparent;
            this.gbCompSearchFilterLogger.Controls.Add(this.lstLoglinkdinScarper);
            this.gbCompSearchFilterLogger.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbCompSearchFilterLogger.Location = new System.Drawing.Point(27, 429);
            this.gbCompSearchFilterLogger.Name = "gbCompSearchFilterLogger";
            this.gbCompSearchFilterLogger.Size = new System.Drawing.Size(754, 140);
            this.gbCompSearchFilterLogger.TabIndex = 100;
            this.gbCompSearchFilterLogger.TabStop = false;
            this.gbCompSearchFilterLogger.Text = "Logger";
            // 
            // lstLoglinkdinScarper
            // 
            this.lstLoglinkdinScarper.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstLoglinkdinScarper.FormattingEnabled = true;
            this.lstLoglinkdinScarper.HorizontalScrollbar = true;
            this.lstLoglinkdinScarper.Location = new System.Drawing.Point(19, 19);
            this.lstLoglinkdinScarper.Name = "lstLoglinkdinScarper";
            this.lstLoglinkdinScarper.ScrollAlwaysVisible = true;
            this.lstLoglinkdinScarper.Size = new System.Drawing.Size(715, 108);
            this.lstLoglinkdinScarper.TabIndex = 75;
            // 
            // btnStopScraper
            // 
            this.btnStopScraper.BackgroundImage = global::InBoardPro.Properties.Resources.stop;
            this.btnStopScraper.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnStopScraper.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnStopScraper.Location = new System.Drawing.Point(450, 19);
            this.btnStopScraper.Name = "btnStopScraper";
            this.btnStopScraper.Size = new System.Drawing.Size(125, 30);
            this.btnStopScraper.TabIndex = 116;
            this.btnStopScraper.UseVisualStyleBackColor = true;
            this.btnStopScraper.Click += new System.EventHandler(this.btnStopScraper_Click);
            // 
            // btnSearchScraper
            // 
            this.btnSearchScraper.BackgroundImage = global::InBoardPro.Properties.Resources.search;
            this.btnSearchScraper.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnSearchScraper.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSearchScraper.Location = new System.Drawing.Point(307, 18);
            this.btnSearchScraper.Margin = new System.Windows.Forms.Padding(2);
            this.btnSearchScraper.Name = "btnSearchScraper";
            this.btnSearchScraper.Size = new System.Drawing.Size(125, 31);
            this.btnSearchScraper.TabIndex = 115;
            this.btnSearchScraper.UseVisualStyleBackColor = true;
            this.btnSearchScraper.Click += new System.EventHandler(this.btnSearchScraper_Click);
            // 
            // btnSearchNewScraper
            // 
            this.btnSearchNewScraper.BackgroundImage = global::InBoardPro.Properties.Resources.new_search;
            this.btnSearchNewScraper.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnSearchNewScraper.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSearchNewScraper.Location = new System.Drawing.Point(159, 18);
            this.btnSearchNewScraper.Margin = new System.Windows.Forms.Padding(2);
            this.btnSearchNewScraper.Name = "btnSearchNewScraper";
            this.btnSearchNewScraper.Size = new System.Drawing.Size(125, 31);
            this.btnSearchNewScraper.TabIndex = 114;
            this.btnSearchNewScraper.UseVisualStyleBackColor = true;
            this.btnSearchNewScraper.Click += new System.EventHandler(this.btnSearchNewScraper_Click);
            // 
            // gbCompSearchFilterAction
            // 
            this.gbCompSearchFilterAction.BackColor = System.Drawing.Color.Transparent;
            this.gbCompSearchFilterAction.Controls.Add(this.btnStopScraper);
            this.gbCompSearchFilterAction.Controls.Add(this.btnSearchScraper);
            this.gbCompSearchFilterAction.Controls.Add(this.btnSearchNewScraper);
            this.gbCompSearchFilterAction.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbCompSearchFilterAction.Location = new System.Drawing.Point(27, 360);
            this.gbCompSearchFilterAction.Name = "gbCompSearchFilterAction";
            this.gbCompSearchFilterAction.Size = new System.Drawing.Size(754, 63);
            this.gbCompSearchFilterAction.TabIndex = 117;
            this.gbCompSearchFilterAction.TabStop = false;
            this.gbCompSearchFilterAction.Text = "Submit Action";
            // 
            // FrmSearchWithSalesNavigator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(803, 592);
            this.Controls.Add(this.gbCompSearchFilterAction);
            this.Controls.Add(this.gbCompSearchFilterLogger);
            this.Controls.Add(this.grpBox_CompanySearch);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FrmSearchWithSalesNavigator";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CompanySearchWithFilter";
            this.Load += new System.EventHandler(this.FrmCompanySearchWithFilter_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FrmCompanySearchWithFilter_Paint);
            this.grpBox_CompanySearch.ResumeLayout(false);
            this.grpBox_CompanySearch.PerformLayout();
            this.gbCompSearchFilterLogger.ResumeLayout(false);
            this.gbCompSearchFilterAction.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpBox_CompanySearch;
        private System.Windows.Forms.ComboBox comboBoxemail;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.ComboBox combScraperLocation;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.CheckedListBox checkedListIndustry;
        private System.Windows.Forms.Label lblIndustry;
        private System.Windows.Forms.CheckedListBox checkedListFortune;
        private System.Windows.Forms.Label lblFortune;
        private System.Windows.Forms.CheckedListBox checkedListJobOpportunity;
        private System.Windows.Forms.Label lblJobOpportunity;
        private System.Windows.Forms.CheckedListBox checkedListFollowers;
        private System.Windows.Forms.Label lblNoOffollowers;
        private System.Windows.Forms.CheckedListBox checkedListCompanySize;
        private System.Windows.Forms.Label lblcompanySize;
        private System.Windows.Forms.GroupBox gbCompSearchFilterLogger;
        private System.Windows.Forms.ListBox lstLoglinkdinScarper;
        private System.Windows.Forms.Button btnStopScraper;
        private System.Windows.Forms.Button btnSearchScraper;
        private System.Windows.Forms.Button btnSearchNewScraper;
        private System.Windows.Forms.TextBox txtKeyword;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtOtherLocation;
        private System.Windows.Forms.CheckBox chkAllIndustry;
        private System.Windows.Forms.CheckBox chkAllCompanySize;
        private System.Windows.Forms.CheckBox chkAllFortune;
        private System.Windows.Forms.CheckBox chkAllFolowers;
        private System.Windows.Forms.GroupBox gbCompSearchFilterAction;
    }
}