namespace InBoardPro
{
    partial class FrmAddBlackListUser
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAddBlackListUser));
            this.gbDBCLogin = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnBrowse_AddProfileID = new System.Windows.Forms.Button();
            this.btnLoadAccounts = new System.Windows.Forms.Button();
            this.lblCaptchaID = new System.Windows.Forms.Label();
            this.txtBlackListIDPath = new System.Windows.Forms.TextBox();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.DGridViewBlackListUserDetails = new System.Windows.Forms.DataGridView();
            this.ChkBtnSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.GroupBoxDetails = new System.Windows.Forms.GroupBox();
            this.chkSelectAll = new System.Windows.Forms.CheckBox();
            this.gbDBCLogin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGridViewBlackListUserDetails)).BeginInit();
            this.GroupBoxDetails.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbDBCLogin
            // 
            this.gbDBCLogin.BackColor = System.Drawing.Color.Transparent;
            this.gbDBCLogin.Controls.Add(this.label1);
            this.gbDBCLogin.Controls.Add(this.btnDelete);
            this.gbDBCLogin.Controls.Add(this.btnBrowse_AddProfileID);
            this.gbDBCLogin.Controls.Add(this.btnLoadAccounts);
            this.gbDBCLogin.Controls.Add(this.lblCaptchaID);
            this.gbDBCLogin.Controls.Add(this.txtBlackListIDPath);
            this.gbDBCLogin.Controls.Add(this.btnSubmit);
            this.gbDBCLogin.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbDBCLogin.Location = new System.Drawing.Point(23, 21);
            this.gbDBCLogin.Name = "gbDBCLogin";
            this.gbDBCLogin.Size = new System.Drawing.Size(495, 110);
            this.gbDBCLogin.TabIndex = 177;
            this.gbDBCLogin.TabStop = false;
            this.gbDBCLogin.Text = "Add BlackList User";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(113, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 13);
            this.label1.TabIndex = 131;
            this.label1.Text = "Format : (ProfileID)";
            // 
            // btnDelete
            // 
            this.btnDelete.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.Location = new System.Drawing.Point(294, 70);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(94, 23);
            this.btnDelete.TabIndex = 130;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnBrowse_AddProfileID
            // 
            this.btnBrowse_AddProfileID.BackColor = System.Drawing.Color.White;
            this.btnBrowse_AddProfileID.BackgroundImage = global::InBoardPro.Properties.Resources.browse_btn;
            this.btnBrowse_AddProfileID.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnBrowse_AddProfileID.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBrowse_AddProfileID.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnBrowse_AddProfileID.ForeColor = System.Drawing.Color.Black;
            this.btnBrowse_AddProfileID.Location = new System.Drawing.Point(401, 39);
            this.btnBrowse_AddProfileID.Name = "btnBrowse_AddProfileID";
            this.btnBrowse_AddProfileID.Size = new System.Drawing.Size(77, 27);
            this.btnBrowse_AddProfileID.TabIndex = 129;
            this.btnBrowse_AddProfileID.UseVisualStyleBackColor = false;
            this.btnBrowse_AddProfileID.Click += new System.EventHandler(this.btnBrowse_AddProfileID_Click);
            // 
            // btnLoadAccounts
            // 
            this.btnLoadAccounts.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoadAccounts.Location = new System.Drawing.Point(194, 70);
            this.btnLoadAccounts.Name = "btnLoadAccounts";
            this.btnLoadAccounts.Size = new System.Drawing.Size(94, 23);
            this.btnLoadAccounts.TabIndex = 9;
            this.btnLoadAccounts.Text = "Load Acounts";
            this.btnLoadAccounts.UseVisualStyleBackColor = true;
            this.btnLoadAccounts.Click += new System.EventHandler(this.btnLoadAccounts_Click);
            // 
            // lblCaptchaID
            // 
            this.lblCaptchaID.AutoSize = true;
            this.lblCaptchaID.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCaptchaID.Location = new System.Drawing.Point(42, 46);
            this.lblCaptchaID.Name = "lblCaptchaID";
            this.lblCaptchaID.Size = new System.Drawing.Size(66, 13);
            this.lblCaptchaID.TabIndex = 8;
            this.lblCaptchaID.Text = "Profile ID:";
            // 
            // txtBlackListIDPath
            // 
            this.txtBlackListIDPath.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBlackListIDPath.Location = new System.Drawing.Point(113, 43);
            this.txtBlackListIDPath.Name = "txtBlackListIDPath";
            this.txtBlackListIDPath.Size = new System.Drawing.Size(282, 21);
            this.txtBlackListIDPath.TabIndex = 6;
            // 
            // btnSubmit
            // 
            this.btnSubmit.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSubmit.Location = new System.Drawing.Point(113, 70);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(75, 23);
            this.btnSubmit.TabIndex = 5;
            this.btnSubmit.Text = "Submit";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // DGridViewBlackListUserDetails
            // 
            this.DGridViewBlackListUserDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGridViewBlackListUserDetails.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ChkBtnSelect});
            this.DGridViewBlackListUserDetails.Location = new System.Drawing.Point(21, 19);
            this.DGridViewBlackListUserDetails.Name = "DGridViewBlackListUserDetails";
            this.DGridViewBlackListUserDetails.Size = new System.Drawing.Size(459, 150);
            this.DGridViewBlackListUserDetails.TabIndex = 178;
            this.DGridViewBlackListUserDetails.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // ChkBtnSelect
            // 
            this.ChkBtnSelect.HeaderText = "Select";
            this.ChkBtnSelect.Name = "ChkBtnSelect";
            // 
            // GroupBoxDetails
            // 
            this.GroupBoxDetails.BackColor = System.Drawing.Color.Transparent;
            this.GroupBoxDetails.Controls.Add(this.chkSelectAll);
            this.GroupBoxDetails.Controls.Add(this.DGridViewBlackListUserDetails);
            this.GroupBoxDetails.Location = new System.Drawing.Point(23, 137);
            this.GroupBoxDetails.Name = "GroupBoxDetails";
            this.GroupBoxDetails.Size = new System.Drawing.Size(495, 203);
            this.GroupBoxDetails.TabIndex = 179;
            this.GroupBoxDetails.TabStop = false;
            this.GroupBoxDetails.Text = "User Details";
            // 
            // chkSelectAll
            // 
            this.chkSelectAll.AutoSize = true;
            this.chkSelectAll.Location = new System.Drawing.Point(21, 175);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Size = new System.Drawing.Size(70, 17);
            this.chkSelectAll.TabIndex = 131;
            this.chkSelectAll.Text = "Select All";
            this.chkSelectAll.UseVisualStyleBackColor = true;
            this.chkSelectAll.Click += new System.EventHandler(this.chkSelectAll_Click);
            // 
            // FrmAddBlackListUser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(542, 352);
            this.Controls.Add(this.GroupBoxDetails);
            this.Controls.Add(this.gbDBCLogin);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FrmAddBlackListUser";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BlackList User";
            this.Load += new System.EventHandler(this.FrmAddBlackListUser_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FrmAddBlackListUser_Paint);
            this.gbDBCLogin.ResumeLayout(false);
            this.gbDBCLogin.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGridViewBlackListUserDetails)).EndInit();
            this.GroupBoxDetails.ResumeLayout(false);
            this.GroupBoxDetails.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbDBCLogin;
        private System.Windows.Forms.Label lblCaptchaID;
        private System.Windows.Forms.TextBox txtBlackListIDPath;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.DataGridView DGridViewBlackListUserDetails;
        private System.Windows.Forms.GroupBox GroupBoxDetails;
        private System.Windows.Forms.Button btnLoadAccounts;
        private System.Windows.Forms.Button btnBrowse_AddProfileID;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ChkBtnSelect;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.CheckBox chkSelectAll;
        private System.Windows.Forms.Label label1;
    }
}