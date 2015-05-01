namespace InBoardPro
{
    partial class FrmScrapGroupMember
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmScrapGroupMember));
            this.gbScrpGroupMemLogger = new System.Windows.Forms.GroupBox();
            this.LstBoxLogger = new System.Windows.Forms.ListBox();
            this.gbScrpGroupMemInputs = new System.Windows.Forms.GroupBox();
            this.TxtGroupURL_ScrapGroupMember = new System.Windows.Forms.TextBox();
            this.lblGrupURL_ScrapGroupMember = new System.Windows.Forms.Label();
            this.BtnUploadGroupURLScrapGroupMember = new System.Windows.Forms.Button();
            this.gbScrpGroupMemSetting = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.TxtNoOfThreads = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.gbScrpGroupMemAction = new System.Windows.Forms.GroupBox();
            this.btnStopScrapGroupMember = new System.Windows.Forms.Button();
            this.btnSearchScrapGroupMember = new System.Windows.Forms.Button();
            this.gbScrpGroupMemLogger.SuspendLayout();
            this.gbScrpGroupMemInputs.SuspendLayout();
            this.gbScrpGroupMemSetting.SuspendLayout();
            this.gbScrpGroupMemAction.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbScrpGroupMemLogger
            // 
            this.gbScrpGroupMemLogger.BackColor = System.Drawing.Color.Transparent;
            this.gbScrpGroupMemLogger.Controls.Add(this.LstBoxLogger);
            this.gbScrpGroupMemLogger.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbScrpGroupMemLogger.Location = new System.Drawing.Point(14, 259);
            this.gbScrpGroupMemLogger.Name = "gbScrpGroupMemLogger";
            this.gbScrpGroupMemLogger.Size = new System.Drawing.Size(751, 144);
            this.gbScrpGroupMemLogger.TabIndex = 3;
            this.gbScrpGroupMemLogger.TabStop = false;
            this.gbScrpGroupMemLogger.Text = "Logger";
            // 
            // LstBoxLogger
            // 
            this.LstBoxLogger.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LstBoxLogger.FormattingEnabled = true;
            this.LstBoxLogger.HorizontalScrollbar = true;
            this.LstBoxLogger.Location = new System.Drawing.Point(16, 20);
            this.LstBoxLogger.Name = "LstBoxLogger";
            this.LstBoxLogger.ScrollAlwaysVisible = true;
            this.LstBoxLogger.Size = new System.Drawing.Size(717, 108);
            this.LstBoxLogger.TabIndex = 0;
            // 
            // gbScrpGroupMemInputs
            // 
            this.gbScrpGroupMemInputs.BackColor = System.Drawing.Color.Transparent;
            this.gbScrpGroupMemInputs.Controls.Add(this.TxtGroupURL_ScrapGroupMember);
            this.gbScrpGroupMemInputs.Controls.Add(this.lblGrupURL_ScrapGroupMember);
            this.gbScrpGroupMemInputs.Controls.Add(this.BtnUploadGroupURLScrapGroupMember);
            this.gbScrpGroupMemInputs.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbScrpGroupMemInputs.Location = new System.Drawing.Point(13, 14);
            this.gbScrpGroupMemInputs.Name = "gbScrpGroupMemInputs";
            this.gbScrpGroupMemInputs.Size = new System.Drawing.Size(752, 86);
            this.gbScrpGroupMemInputs.TabIndex = 4;
            this.gbScrpGroupMemInputs.TabStop = false;
            this.gbScrpGroupMemInputs.Text = "Group Url Input";
            // 
            // TxtGroupURL_ScrapGroupMember
            // 
            this.TxtGroupURL_ScrapGroupMember.BackColor = System.Drawing.Color.White;
            this.TxtGroupURL_ScrapGroupMember.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TxtGroupURL_ScrapGroupMember.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtGroupURL_ScrapGroupMember.Location = new System.Drawing.Point(105, 34);
            this.TxtGroupURL_ScrapGroupMember.Name = "TxtGroupURL_ScrapGroupMember";
            this.TxtGroupURL_ScrapGroupMember.ReadOnly = true;
            this.TxtGroupURL_ScrapGroupMember.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.TxtGroupURL_ScrapGroupMember.Size = new System.Drawing.Size(524, 21);
            this.TxtGroupURL_ScrapGroupMember.TabIndex = 133;
            // 
            // lblGrupURL_ScrapGroupMember
            // 
            this.lblGrupURL_ScrapGroupMember.AutoSize = true;
            this.lblGrupURL_ScrapGroupMember.BackColor = System.Drawing.Color.Transparent;
            this.lblGrupURL_ScrapGroupMember.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGrupURL_ScrapGroupMember.Location = new System.Drawing.Point(20, 35);
            this.lblGrupURL_ScrapGroupMember.Name = "lblGrupURL_ScrapGroupMember";
            this.lblGrupURL_ScrapGroupMember.Size = new System.Drawing.Size(73, 13);
            this.lblGrupURL_ScrapGroupMember.TabIndex = 132;
            this.lblGrupURL_ScrapGroupMember.Text = "Group URL:";
            // 
            // BtnUploadGroupURLScrapGroupMember
            // 
            this.BtnUploadGroupURLScrapGroupMember.BackColor = System.Drawing.Color.White;
            this.BtnUploadGroupURLScrapGroupMember.BackgroundImage = global::InBoardPro.Properties.Resources.browse_btn;
            this.BtnUploadGroupURLScrapGroupMember.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.BtnUploadGroupURLScrapGroupMember.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnUploadGroupURLScrapGroupMember.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.BtnUploadGroupURLScrapGroupMember.ForeColor = System.Drawing.Color.Black;
            this.BtnUploadGroupURLScrapGroupMember.Location = new System.Drawing.Point(647, 31);
            this.BtnUploadGroupURLScrapGroupMember.Name = "BtnUploadGroupURLScrapGroupMember";
            this.BtnUploadGroupURLScrapGroupMember.Size = new System.Drawing.Size(75, 29);
            this.BtnUploadGroupURLScrapGroupMember.TabIndex = 134;
            this.BtnUploadGroupURLScrapGroupMember.UseVisualStyleBackColor = false;
            this.BtnUploadGroupURLScrapGroupMember.Click += new System.EventHandler(this.BtnUploadGroupURLScrapGroupMember_Click);
            // 
            // gbScrpGroupMemSetting
            // 
            this.gbScrpGroupMemSetting.BackColor = System.Drawing.Color.Transparent;
            this.gbScrpGroupMemSetting.Controls.Add(this.label1);
            this.gbScrpGroupMemSetting.Controls.Add(this.TxtNoOfThreads);
            this.gbScrpGroupMemSetting.Controls.Add(this.label23);
            this.gbScrpGroupMemSetting.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbScrpGroupMemSetting.Location = new System.Drawing.Point(13, 106);
            this.gbScrpGroupMemSetting.Name = "gbScrpGroupMemSetting";
            this.gbScrpGroupMemSetting.Size = new System.Drawing.Size(752, 70);
            this.gbScrpGroupMemSetting.TabIndex = 5;
            this.gbScrpGroupMemSetting.TabStop = false;
            this.gbScrpGroupMemSetting.Text = "Setting";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(234, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(175, 13);
            this.label1.TabIndex = 149;
            this.label1.Text = "(Please Enter Numeric Value)";
            // 
            // TxtNoOfThreads
            // 
            this.TxtNoOfThreads.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtNoOfThreads.Location = new System.Drawing.Point(144, 26);
            this.TxtNoOfThreads.Name = "TxtNoOfThreads";
            this.TxtNoOfThreads.Size = new System.Drawing.Size(42, 21);
            this.TxtNoOfThreads.TabIndex = 148;
            this.TxtNoOfThreads.Text = "5";
            this.TxtNoOfThreads.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.Location = new System.Drawing.Point(44, 29);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(94, 13);
            this.label23.TabIndex = 147;
            this.label23.Text = "No Of Threads:";
            // 
            // gbScrpGroupMemAction
            // 
            this.gbScrpGroupMemAction.BackColor = System.Drawing.Color.Transparent;
            this.gbScrpGroupMemAction.Controls.Add(this.btnStopScrapGroupMember);
            this.gbScrpGroupMemAction.Controls.Add(this.btnSearchScrapGroupMember);
            this.gbScrpGroupMemAction.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbScrpGroupMemAction.Location = new System.Drawing.Point(13, 180);
            this.gbScrpGroupMemAction.Name = "gbScrpGroupMemAction";
            this.gbScrpGroupMemAction.Size = new System.Drawing.Size(752, 71);
            this.gbScrpGroupMemAction.TabIndex = 6;
            this.gbScrpGroupMemAction.TabStop = false;
            this.gbScrpGroupMemAction.Text = "Submit Action";
            // 
            // btnStopScrapGroupMember
            // 
            this.btnStopScrapGroupMember.BackgroundImage = global::InBoardPro.Properties.Resources.stop;
            this.btnStopScrapGroupMember.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnStopScrapGroupMember.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnStopScrapGroupMember.Location = new System.Drawing.Point(254, 24);
            this.btnStopScrapGroupMember.Name = "btnStopScrapGroupMember";
            this.btnStopScrapGroupMember.Size = new System.Drawing.Size(105, 31);
            this.btnStopScrapGroupMember.TabIndex = 1;
            this.btnStopScrapGroupMember.UseVisualStyleBackColor = true;
            this.btnStopScrapGroupMember.Click += new System.EventHandler(this.btnStopScrapGroupMember_Click);
            // 
            // btnSearchScrapGroupMember
            // 
            this.btnSearchScrapGroupMember.BackgroundImage = global::InBoardPro.Properties.Resources.search;
            this.btnSearchScrapGroupMember.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnSearchScrapGroupMember.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSearchScrapGroupMember.Location = new System.Drawing.Point(134, 24);
            this.btnSearchScrapGroupMember.Name = "btnSearchScrapGroupMember";
            this.btnSearchScrapGroupMember.Size = new System.Drawing.Size(105, 31);
            this.btnSearchScrapGroupMember.TabIndex = 0;
            this.btnSearchScrapGroupMember.UseVisualStyleBackColor = true;
            this.btnSearchScrapGroupMember.Click += new System.EventHandler(this.btnSearchScrapGroupMember_Click);
            // 
            // FrmScrapGroupMember
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(787, 414);
            this.Controls.Add(this.gbScrpGroupMemAction);
            this.Controls.Add(this.gbScrpGroupMemSetting);
            this.Controls.Add(this.gbScrpGroupMemInputs);
            this.Controls.Add(this.gbScrpGroupMemLogger);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FrmScrapGroupMember";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GroupMember";
            this.Load += new System.EventHandler(this.FrmScrapGroupMember_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FrmScrapGroupMember_Paint);
            this.gbScrpGroupMemLogger.ResumeLayout(false);
            this.gbScrpGroupMemInputs.ResumeLayout(false);
            this.gbScrpGroupMemInputs.PerformLayout();
            this.gbScrpGroupMemSetting.ResumeLayout(false);
            this.gbScrpGroupMemSetting.PerformLayout();
            this.gbScrpGroupMemAction.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbScrpGroupMemLogger;
        private System.Windows.Forms.ListBox LstBoxLogger;
        private System.Windows.Forms.GroupBox gbScrpGroupMemInputs;
        private System.Windows.Forms.GroupBox gbScrpGroupMemSetting;
        private System.Windows.Forms.GroupBox gbScrpGroupMemAction;
        private System.Windows.Forms.TextBox TxtGroupURL_ScrapGroupMember;
        private System.Windows.Forms.Label lblGrupURL_ScrapGroupMember;
        private System.Windows.Forms.Button BtnUploadGroupURLScrapGroupMember;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TxtNoOfThreads;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Button btnStopScrapGroupMember;
        private System.Windows.Forms.Button btnSearchScrapGroupMember;
    }
}