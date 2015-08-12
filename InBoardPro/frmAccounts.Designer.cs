namespace InBoardPro
{
    partial class frmAccounts
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAccounts));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.gbManageAccount = new System.Windows.Forms.GroupBox();
            this.txtNoOf_Thread = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnCheckAccount = new System.Windows.Forms.Button();
            this.btnExportAccounts = new System.Windows.Forms.Button();
            this.txtAccountsPerProxy = new System.Windows.Forms.TextBox();
            this.ButonClearProxies = new System.Windows.Forms.Button();
            this.lblRequestThreads = new System.Windows.Forms.Label();
            this.btnAssignProxy = new System.Windows.Forms.Button();
            this.btnClearAccounts = new System.Windows.Forms.Button();
            this.lblAccountFile = new System.Windows.Forms.Label();
            this.txtAccountFile = new System.Windows.Forms.TextBox();
            this.btnLoadAccounts = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblAccountStatus = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lstLogger = new System.Windows.Forms.ListBox();
            this.dgvAccount = new System.Windows.Forms.DataGridView();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.gbManageAccount.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAccount)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Location = new System.Drawing.Point(12, 12);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.gbManageAccount);
            this.splitContainer1.Panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.splitContainer1_Panel1_Paint);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.label6);
            this.splitContainer1.Panel2.Controls.Add(this.label5);
            this.splitContainer1.Panel2.Controls.Add(this.lblAccountStatus);
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Panel2.Controls.Add(this.lstLogger);
            this.splitContainer1.Panel2.Controls.Add(this.dgvAccount);
            this.splitContainer1.Panel2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.splitContainer1.Panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.splitContainer1_Panel2_Paint);
            this.splitContainer1.Size = new System.Drawing.Size(928, 512);
            this.splitContainer1.SplitterDistance = 308;
            this.splitContainer1.TabIndex = 56;
            // 
            // gbManageAccount
            // 
            this.gbManageAccount.Controls.Add(this.txtNoOf_Thread);
            this.gbManageAccount.Controls.Add(this.label2);
            this.gbManageAccount.Controls.Add(this.btnCheckAccount);
            this.gbManageAccount.Controls.Add(this.btnExportAccounts);
            this.gbManageAccount.Controls.Add(this.txtAccountsPerProxy);
            this.gbManageAccount.Controls.Add(this.ButonClearProxies);
            this.gbManageAccount.Controls.Add(this.lblRequestThreads);
            this.gbManageAccount.Controls.Add(this.btnAssignProxy);
            this.gbManageAccount.Controls.Add(this.btnClearAccounts);
            this.gbManageAccount.Controls.Add(this.lblAccountFile);
            this.gbManageAccount.Controls.Add(this.txtAccountFile);
            this.gbManageAccount.Controls.Add(this.btnLoadAccounts);
            this.gbManageAccount.Location = new System.Drawing.Point(11, 3);
            this.gbManageAccount.Name = "gbManageAccount";
            this.gbManageAccount.Size = new System.Drawing.Size(299, 491);
            this.gbManageAccount.TabIndex = 15;
            this.gbManageAccount.TabStop = false;
            this.gbManageAccount.Text = "Manage Account";
            this.gbManageAccount.Paint += new System.Windows.Forms.PaintEventHandler(this.gbManageAccount_Paint);
            // 
            // txtNoOf_Thread
            // 
            this.txtNoOf_Thread.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNoOf_Thread.Location = new System.Drawing.Point(178, 266);
            this.txtNoOf_Thread.Name = "txtNoOf_Thread";
            this.txtNoOf_Thread.Size = new System.Drawing.Size(55, 21);
            this.txtNoOf_Thread.TabIndex = 59;
            this.txtNoOf_Thread.Text = "5";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(73, 269);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 13);
            this.label2.TabIndex = 58;
            this.label2.Text = "No. Of Thread :";
            // 
            // btnCheckAccount
            // 
            this.btnCheckAccount.BackColor = System.Drawing.Color.Transparent;
            this.btnCheckAccount.BackgroundImage = global::InBoardPro.Properties.Resources.check_accounts;
            this.btnCheckAccount.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnCheckAccount.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCheckAccount.Location = new System.Drawing.Point(87, 111);
            this.btnCheckAccount.Name = "btnCheckAccount";
            this.btnCheckAccount.Size = new System.Drawing.Size(145, 27);
            this.btnCheckAccount.TabIndex = 57;
            this.btnCheckAccount.UseVisualStyleBackColor = false;
            this.btnCheckAccount.Click += new System.EventHandler(this.btnCheckAccount_Click);
            // 
            // btnExportAccounts
            // 
            this.btnExportAccounts.BackColor = System.Drawing.Color.Transparent;
            this.btnExportAccounts.BackgroundImage = global::InBoardPro.Properties.Resources.export_accounts;
            this.btnExportAccounts.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnExportAccounts.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnExportAccounts.Location = new System.Drawing.Point(87, 147);
            this.btnExportAccounts.Name = "btnExportAccounts";
            this.btnExportAccounts.Size = new System.Drawing.Size(145, 27);
            this.btnExportAccounts.TabIndex = 21;
            this.btnExportAccounts.UseVisualStyleBackColor = false;
            this.btnExportAccounts.Click += new System.EventHandler(this.btnExportAccounts_Click);
            // 
            // txtAccountsPerProxy
            // 
            this.txtAccountsPerProxy.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAccountsPerProxy.Location = new System.Drawing.Point(178, 302);
            this.txtAccountsPerProxy.Name = "txtAccountsPerProxy";
            this.txtAccountsPerProxy.Size = new System.Drawing.Size(55, 21);
            this.txtAccountsPerProxy.TabIndex = 56;
            this.txtAccountsPerProxy.Text = "10";
            // 
            // ButonClearProxies
            // 
            this.ButonClearProxies.BackColor = System.Drawing.Color.Transparent;
            this.ButonClearProxies.BackgroundImage = global::InBoardPro.Properties.Resources.clear_proxies;
            this.ButonClearProxies.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ButonClearProxies.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ButonClearProxies.Location = new System.Drawing.Point(88, 222);
            this.ButonClearProxies.Name = "ButonClearProxies";
            this.ButonClearProxies.Size = new System.Drawing.Size(145, 27);
            this.ButonClearProxies.TabIndex = 20;
            this.ButonClearProxies.UseVisualStyleBackColor = false;
            this.ButonClearProxies.Click += new System.EventHandler(this.ButonClearProxies_Click);
            // 
            // lblRequestThreads
            // 
            this.lblRequestThreads.AutoSize = true;
            this.lblRequestThreads.BackColor = System.Drawing.Color.Transparent;
            this.lblRequestThreads.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRequestThreads.Location = new System.Drawing.Point(12, 305);
            this.lblRequestThreads.Name = "lblRequestThreads";
            this.lblRequestThreads.Size = new System.Drawing.Size(136, 13);
            this.lblRequestThreads.TabIndex = 55;
            this.lblRequestThreads.Text = "No. Of accounts per IP";
            // 
            // btnAssignProxy
            // 
            this.btnAssignProxy.BackColor = System.Drawing.Color.Transparent;
            this.btnAssignProxy.BackgroundImage = global::InBoardPro.Properties.Resources.Assign_proxy;
            this.btnAssignProxy.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnAssignProxy.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAssignProxy.Location = new System.Drawing.Point(88, 342);
            this.btnAssignProxy.Name = "btnAssignProxy";
            this.btnAssignProxy.Size = new System.Drawing.Size(145, 26);
            this.btnAssignProxy.TabIndex = 19;
            this.btnAssignProxy.UseVisualStyleBackColor = false;
            this.btnAssignProxy.Click += new System.EventHandler(this.btnAssignProxy_Click);
            // 
            // btnClearAccounts
            // 
            this.btnClearAccounts.BackColor = System.Drawing.Color.Transparent;
            this.btnClearAccounts.BackgroundImage = global::InBoardPro.Properties.Resources.clear_accounts;
            this.btnClearAccounts.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnClearAccounts.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnClearAccounts.Location = new System.Drawing.Point(87, 184);
            this.btnClearAccounts.Name = "btnClearAccounts";
            this.btnClearAccounts.Size = new System.Drawing.Size(146, 27);
            this.btnClearAccounts.TabIndex = 17;
            this.btnClearAccounts.UseVisualStyleBackColor = false;
            this.btnClearAccounts.Click += new System.EventHandler(this.btnClearAccounts_Click);
            // 
            // lblAccountFile
            // 
            this.lblAccountFile.AutoSize = true;
            this.lblAccountFile.BackColor = System.Drawing.Color.Transparent;
            this.lblAccountFile.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAccountFile.ForeColor = System.Drawing.Color.Black;
            this.lblAccountFile.Location = new System.Drawing.Point(1, 40);
            this.lblAccountFile.Name = "lblAccountFile";
            this.lblAccountFile.Size = new System.Drawing.Size(81, 13);
            this.lblAccountFile.TabIndex = 16;
            this.lblAccountFile.Text = "Accounts File";
            // 
            // txtAccountFile
            // 
            this.txtAccountFile.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAccountFile.Location = new System.Drawing.Point(88, 37);
            this.txtAccountFile.Name = "txtAccountFile";
            this.txtAccountFile.ReadOnly = true;
            this.txtAccountFile.Size = new System.Drawing.Size(175, 21);
            this.txtAccountFile.TabIndex = 15;
            // 
            // btnLoadAccounts
            // 
            this.btnLoadAccounts.BackColor = System.Drawing.Color.Transparent;
            this.btnLoadAccounts.BackgroundImage = global::InBoardPro.Properties.Resources.load_accounts;
            this.btnLoadAccounts.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnLoadAccounts.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnLoadAccounts.Location = new System.Drawing.Point(88, 75);
            this.btnLoadAccounts.Name = "btnLoadAccounts";
            this.btnLoadAccounts.Size = new System.Drawing.Size(145, 27);
            this.btnLoadAccounts.TabIndex = 14;
            this.btnLoadAccounts.UseVisualStyleBackColor = false;
            this.btnLoadAccounts.Click += new System.EventHandler(this.btnLoadAccounts_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(544, 323);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(14, 13);
            this.label6.TabIndex = 62;
            this.label6.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(401, 323);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(148, 13);
            this.label5.TabIndex = 61;
            this.label5.Text = "Total Loaded Accounts =";
            // 
            // lblAccountStatus
            // 
            this.lblAccountStatus.AutoSize = true;
            this.lblAccountStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblAccountStatus.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAccountStatus.Location = new System.Drawing.Point(242, 322);
            this.lblAccountStatus.Name = "lblAccountStatus";
            this.lblAccountStatus.Size = new System.Drawing.Size(43, 13);
            this.lblAccountStatus.TabIndex = 60;
            this.lblAccountStatus.Text = "Status";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(125, 321);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(126, 13);
            this.label1.TabIndex = 58;
            this.label1.Text = "Process Status >>> ";
            // 
            // lstLogger
            // 
            this.lstLogger.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lstLogger.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstLogger.FormattingEnabled = true;
            this.lstLogger.HorizontalScrollbar = true;
            this.lstLogger.Location = new System.Drawing.Point(0, 378);
            this.lstLogger.Name = "lstLogger";
            this.lstLogger.Size = new System.Drawing.Size(616, 134);
            this.lstLogger.TabIndex = 57;
            // 
            // dgvAccount
            // 
            this.dgvAccount.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgvAccount.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAccount.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvAccount.Location = new System.Drawing.Point(3, 3);
            this.dgvAccount.Name = "dgvAccount";
            this.dgvAccount.ReadOnly = true;
            this.dgvAccount.Size = new System.Drawing.Size(609, 306);
            this.dgvAccount.TabIndex = 1;
            this.dgvAccount.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAccount_CellContentClick);
            // 
            // frmAccounts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(976, 529);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAccounts";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "My Accounts(Load Accounts For InBoardPro)";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmAccounts_FormClosed);
            this.Load += new System.EventHandler(this.frmAccounts_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.frmAccounts_Paint);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.gbManageAccount.ResumeLayout(false);
            this.gbManageAccount.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAccount)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox gbManageAccount;
        private System.Windows.Forms.Button btnExportAccounts;
        private System.Windows.Forms.Button ButonClearProxies;
        private System.Windows.Forms.Button btnAssignProxy;
        private System.Windows.Forms.Button btnClearAccounts;
        private System.Windows.Forms.Label lblAccountFile;
        private System.Windows.Forms.TextBox txtAccountFile;
        private System.Windows.Forms.Button btnLoadAccounts;
        private System.Windows.Forms.ListBox lstLogger;
        private System.Windows.Forms.TextBox txtAccountsPerProxy;
        private System.Windows.Forms.Label lblRequestThreads;
        private System.Windows.Forms.DataGridView dgvAccount;
        private System.Windows.Forms.Label lblAccountStatus;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnCheckAccount;
        private System.Windows.Forms.TextBox txtNoOf_Thread;
        private System.Windows.Forms.Label label2;


    }
}