namespace InBoardPro
{
    partial class frmCampaignScraper
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCampaignScraper));
            this.dgv_CampaignScraper = new System.Windows.Forms.DataGridView();
            this.chkBoxSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.lstLogger = new System.Windows.Forms.ListBox();
            this.DeleteCampaign = new System.Windows.Forms.Button();
            this.StartCampaign = new System.Windows.Forms.Button();
            this.GbLogger = new System.Windows.Forms.GroupBox();
            this.gbSubmitAction = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_CampaignScraper)).BeginInit();
            this.GbLogger.SuspendLayout();
            this.gbSubmitAction.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgv_CampaignScraper
            // 
            this.dgv_CampaignScraper.AllowUserToAddRows = false;
            this.dgv_CampaignScraper.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_CampaignScraper.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.chkBoxSelect});
            this.dgv_CampaignScraper.Location = new System.Drawing.Point(12, 12);
            this.dgv_CampaignScraper.Name = "dgv_CampaignScraper";
            this.dgv_CampaignScraper.Size = new System.Drawing.Size(1015, 365);
            this.dgv_CampaignScraper.TabIndex = 0;
            // 
            // chkBoxSelect
            // 
            this.chkBoxSelect.HeaderText = "Select";
            this.chkBoxSelect.Name = "chkBoxSelect";
            // 
            // lstLogger
            // 
            this.lstLogger.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstLogger.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstLogger.FormattingEnabled = true;
            this.lstLogger.Location = new System.Drawing.Point(3, 17);
            this.lstLogger.Name = "lstLogger";
            this.lstLogger.Size = new System.Drawing.Size(1009, 89);
            this.lstLogger.TabIndex = 3;
            // 
            // DeleteCampaign
            // 
            this.DeleteCampaign.BackgroundImage = global::InBoardPro.Properties.Resources.delete;
            this.DeleteCampaign.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.DeleteCampaign.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.DeleteCampaign.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DeleteCampaign.Location = new System.Drawing.Point(469, 19);
            this.DeleteCampaign.Name = "DeleteCampaign";
            this.DeleteCampaign.Size = new System.Drawing.Size(117, 30);
            this.DeleteCampaign.TabIndex = 2;
            this.DeleteCampaign.UseVisualStyleBackColor = true;
            this.DeleteCampaign.Click += new System.EventHandler(this.DeleteCampaign_Click);
            // 
            // StartCampaign
            // 
            this.StartCampaign.BackgroundImage = global::InBoardPro.Properties.Resources.start;
            this.StartCampaign.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.StartCampaign.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.StartCampaign.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StartCampaign.Location = new System.Drawing.Point(327, 19);
            this.StartCampaign.Name = "StartCampaign";
            this.StartCampaign.Size = new System.Drawing.Size(120, 30);
            this.StartCampaign.TabIndex = 1;
            this.StartCampaign.UseVisualStyleBackColor = true;
            this.StartCampaign.Click += new System.EventHandler(this.StartCampaign_Click);
            // 
            // GbLogger
            // 
            this.GbLogger.BackColor = System.Drawing.Color.Transparent;
            this.GbLogger.Controls.Add(this.lstLogger);
            this.GbLogger.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GbLogger.Location = new System.Drawing.Point(12, 441);
            this.GbLogger.Name = "GbLogger";
            this.GbLogger.Size = new System.Drawing.Size(1015, 109);
            this.GbLogger.TabIndex = 4;
            this.GbLogger.TabStop = false;
            this.GbLogger.Text = "Logger";
            // 
            // gbSubmitAction
            // 
            this.gbSubmitAction.BackColor = System.Drawing.Color.Transparent;
            this.gbSubmitAction.Controls.Add(this.StartCampaign);
            this.gbSubmitAction.Controls.Add(this.DeleteCampaign);
            this.gbSubmitAction.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbSubmitAction.Location = new System.Drawing.Point(12, 383);
            this.gbSubmitAction.Name = "gbSubmitAction";
            this.gbSubmitAction.Size = new System.Drawing.Size(1015, 59);
            this.gbSubmitAction.TabIndex = 5;
            this.gbSubmitAction.TabStop = false;
            this.gbSubmitAction.Text = "Submit Action";
            // 
            // frmCampaignScraper
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1039, 553);
            this.Controls.Add(this.gbSubmitAction);
            this.Controls.Add(this.GbLogger);
            this.Controls.Add(this.dgv_CampaignScraper);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmCampaignScraper";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GettingCampaign";
            this.Load += new System.EventHandler(this.frmCampaignScraper_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.frmCampaignScraper_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_CampaignScraper)).EndInit();
            this.GbLogger.ResumeLayout(false);
            this.gbSubmitAction.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv_CampaignScraper;
        private System.Windows.Forms.Button StartCampaign;
        private System.Windows.Forms.Button DeleteCampaign;
        private System.Windows.Forms.DataGridViewCheckBoxColumn chkBoxSelect;
        private System.Windows.Forms.ListBox lstLogger;
        private System.Windows.Forms.GroupBox GbLogger;
        private System.Windows.Forms.GroupBox gbSubmitAction;
    }
}