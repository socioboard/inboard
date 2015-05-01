namespace InBoardPro
{
    partial class frmGroupUpdateLiker
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmGroupUpdateLiker));
            this.gbUpdateLikerAccountsInput = new System.Windows.Forms.GroupBox();
            this.label47 = new System.Windows.Forms.Label();
            this.chkGroupUpdate = new System.Windows.Forms.CheckBox();
            this.chkUpdateCollection = new System.Windows.Forms.CheckedListBox();
            this.btnGetUser = new System.Windows.Forms.Button();
            this.cmbGroupUser = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.btnStatusLikerStop = new System.Windows.Forms.Button();
            this.btnUpdateLike = new System.Windows.Forms.Button();
            this.gbUpdateLikerDelaySetting = new System.Windows.Forms.GroupBox();
            this.label113 = new System.Windows.Forms.Label();
            this.txtGroupUpdateMaxDelay = new System.Windows.Forms.TextBox();
            this.label94 = new System.Windows.Forms.Label();
            this.label95 = new System.Windows.Forms.Label();
            this.txtGroupUpdateMinDelay = new System.Windows.Forms.TextBox();
            this.gbUpdateLikerLogger = new System.Windows.Forms.GroupBox();
            this.lstLoglinkdinScarper = new System.Windows.Forms.ListBox();
            this.gbUpdateLikerAction = new System.Windows.Forms.GroupBox();
            this.gbUpdateLikerAccountsInput.SuspendLayout();
            this.gbUpdateLikerDelaySetting.SuspendLayout();
            this.gbUpdateLikerLogger.SuspendLayout();
            this.gbUpdateLikerAction.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbUpdateLikerAccountsInput
            // 
            this.gbUpdateLikerAccountsInput.BackColor = System.Drawing.Color.Transparent;
            this.gbUpdateLikerAccountsInput.Controls.Add(this.label47);
            this.gbUpdateLikerAccountsInput.Controls.Add(this.chkGroupUpdate);
            this.gbUpdateLikerAccountsInput.Controls.Add(this.chkUpdateCollection);
            this.gbUpdateLikerAccountsInput.Controls.Add(this.btnGetUser);
            this.gbUpdateLikerAccountsInput.Controls.Add(this.cmbGroupUser);
            this.gbUpdateLikerAccountsInput.Controls.Add(this.label14);
            this.gbUpdateLikerAccountsInput.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbUpdateLikerAccountsInput.Location = new System.Drawing.Point(12, 12);
            this.gbUpdateLikerAccountsInput.Name = "gbUpdateLikerAccountsInput";
            this.gbUpdateLikerAccountsInput.Size = new System.Drawing.Size(854, 212);
            this.gbUpdateLikerAccountsInput.TabIndex = 59;
            this.gbUpdateLikerAccountsInput.TabStop = false;
            this.gbUpdateLikerAccountsInput.Text = "Status Update Liker";
            // 
            // label47
            // 
            this.label47.AutoSize = true;
            this.label47.Location = new System.Drawing.Point(884, 311);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(27, 13);
            this.label47.TabIndex = 95;
            this.label47.Text = ".....";
            this.label47.Visible = false;
            // 
            // chkGroupUpdate
            // 
            this.chkGroupUpdate.AutoSize = true;
            this.chkGroupUpdate.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkGroupUpdate.Location = new System.Drawing.Point(156, 183);
            this.chkGroupUpdate.Name = "chkGroupUpdate";
            this.chkGroupUpdate.Size = new System.Drawing.Size(150, 17);
            this.chkGroupUpdate.TabIndex = 86;
            this.chkGroupUpdate.Text = "Select All/Unselect All";
            this.chkGroupUpdate.UseVisualStyleBackColor = true;
            this.chkGroupUpdate.CheckedChanged += new System.EventHandler(this.chkGroupUpdate_CheckedChanged);
            // 
            // chkUpdateCollection
            // 
            this.chkUpdateCollection.CheckOnClick = true;
            this.chkUpdateCollection.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkUpdateCollection.FormattingEnabled = true;
            this.chkUpdateCollection.HorizontalScrollbar = true;
            this.chkUpdateCollection.Location = new System.Drawing.Point(156, 61);
            this.chkUpdateCollection.Name = "chkUpdateCollection";
            this.chkUpdateCollection.Size = new System.Drawing.Size(630, 116);
            this.chkUpdateCollection.TabIndex = 3;
            // 
            // btnGetUser
            // 
            this.btnGetUser.BackColor = System.Drawing.Color.White;
            this.btnGetUser.BackgroundImage = global::InBoardPro.Properties.Resources.get_comment;
            this.btnGetUser.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnGetUser.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnGetUser.Font = new System.Drawing.Font("Cambria", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGetUser.Location = new System.Drawing.Point(19, 25);
            this.btnGetUser.Name = "btnGetUser";
            this.btnGetUser.Size = new System.Drawing.Size(120, 29);
            this.btnGetUser.TabIndex = 1;
            this.btnGetUser.UseVisualStyleBackColor = false;
            this.btnGetUser.Click += new System.EventHandler(this.btnGetUser_Click);
            // 
            // cmbGroupUser
            // 
            this.cmbGroupUser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGroupUser.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbGroupUser.FormattingEnabled = true;
            this.cmbGroupUser.Location = new System.Drawing.Point(156, 29);
            this.cmbGroupUser.Margin = new System.Windows.Forms.Padding(2);
            this.cmbGroupUser.Name = "cmbGroupUser";
            this.cmbGroupUser.Size = new System.Drawing.Size(326, 21);
            this.cmbGroupUser.TabIndex = 2;
            this.cmbGroupUser.SelectedIndexChanged += new System.EventHandler(this.cmbGroupUser_SelectedIndexChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(23, 109);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(0, 13);
            this.label14.TabIndex = 76;
            // 
            // btnStatusLikerStop
            // 
            this.btnStatusLikerStop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnStatusLikerStop.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnStatusLikerStop.Image = global::InBoardPro.Properties.Resources.stop;
            this.btnStatusLikerStop.Location = new System.Drawing.Point(422, 19);
            this.btnStatusLikerStop.Name = "btnStatusLikerStop";
            this.btnStatusLikerStop.Size = new System.Drawing.Size(120, 30);
            this.btnStatusLikerStop.TabIndex = 111;
            this.btnStatusLikerStop.UseVisualStyleBackColor = true;
            this.btnStatusLikerStop.Click += new System.EventHandler(this.btnStatusLikerStop_Click);
            // 
            // btnUpdateLike
            // 
            this.btnUpdateLike.BackColor = System.Drawing.Color.White;
            this.btnUpdateLike.BackgroundImage = global::InBoardPro.Properties.Resources.comment_liker;
            this.btnUpdateLike.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnUpdateLike.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnUpdateLike.Font = new System.Drawing.Font("Cambria", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdateLike.Location = new System.Drawing.Point(280, 18);
            this.btnUpdateLike.Name = "btnUpdateLike";
            this.btnUpdateLike.Size = new System.Drawing.Size(120, 31);
            this.btnUpdateLike.TabIndex = 8;
            this.btnUpdateLike.UseVisualStyleBackColor = false;
            this.btnUpdateLike.Click += new System.EventHandler(this.btnUpdateLike_Click);
            // 
            // gbUpdateLikerDelaySetting
            // 
            this.gbUpdateLikerDelaySetting.BackColor = System.Drawing.Color.Transparent;
            this.gbUpdateLikerDelaySetting.Controls.Add(this.label113);
            this.gbUpdateLikerDelaySetting.Controls.Add(this.txtGroupUpdateMaxDelay);
            this.gbUpdateLikerDelaySetting.Controls.Add(this.label94);
            this.gbUpdateLikerDelaySetting.Controls.Add(this.label95);
            this.gbUpdateLikerDelaySetting.Controls.Add(this.txtGroupUpdateMinDelay);
            this.gbUpdateLikerDelaySetting.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbUpdateLikerDelaySetting.Location = new System.Drawing.Point(12, 227);
            this.gbUpdateLikerDelaySetting.Name = "gbUpdateLikerDelaySetting";
            this.gbUpdateLikerDelaySetting.Size = new System.Drawing.Size(854, 49);
            this.gbUpdateLikerDelaySetting.TabIndex = 123;
            this.gbUpdateLikerDelaySetting.TabStop = false;
            this.gbUpdateLikerDelaySetting.Text = "Delay Setting";
            // 
            // label113
            // 
            this.label113.AutoSize = true;
            this.label113.BackColor = System.Drawing.Color.Transparent;
            this.label113.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label113.Location = new System.Drawing.Point(375, 22);
            this.label113.Name = "label113";
            this.label113.Size = new System.Drawing.Size(305, 13);
            this.label113.TabIndex = 125;
            this.label113.Text = "Its randomly delay between both values in seconds.";
            // 
            // txtGroupUpdateMaxDelay
            // 
            this.txtGroupUpdateMaxDelay.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGroupUpdateMaxDelay.Location = new System.Drawing.Point(316, 18);
            this.txtGroupUpdateMaxDelay.Name = "txtGroupUpdateMaxDelay";
            this.txtGroupUpdateMaxDelay.Size = new System.Drawing.Size(43, 21);
            this.txtGroupUpdateMaxDelay.TabIndex = 119;
            this.txtGroupUpdateMaxDelay.Text = "25";
            this.txtGroupUpdateMaxDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label94
            // 
            this.label94.AutoSize = true;
            this.label94.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label94.Location = new System.Drawing.Point(155, 22);
            this.label94.Name = "label94";
            this.label94.Size = new System.Drawing.Size(40, 13);
            this.label94.TabIndex = 116;
            this.label94.Text = "Delay";
            // 
            // label95
            // 
            this.label95.AutoSize = true;
            this.label95.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label95.Location = new System.Drawing.Point(277, 22);
            this.label95.Name = "label95";
            this.label95.Size = new System.Drawing.Size(18, 13);
            this.label95.TabIndex = 117;
            this.label95.Text = "to";
            // 
            // txtGroupUpdateMinDelay
            // 
            this.txtGroupUpdateMinDelay.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGroupUpdateMinDelay.Location = new System.Drawing.Point(214, 18);
            this.txtGroupUpdateMinDelay.Name = "txtGroupUpdateMinDelay";
            this.txtGroupUpdateMinDelay.Size = new System.Drawing.Size(44, 21);
            this.txtGroupUpdateMinDelay.TabIndex = 118;
            this.txtGroupUpdateMinDelay.Text = "20";
            this.txtGroupUpdateMinDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // gbUpdateLikerLogger
            // 
            this.gbUpdateLikerLogger.BackColor = System.Drawing.Color.Transparent;
            this.gbUpdateLikerLogger.Controls.Add(this.lstLoglinkdinScarper);
            this.gbUpdateLikerLogger.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbUpdateLikerLogger.Location = new System.Drawing.Point(12, 344);
            this.gbUpdateLikerLogger.Name = "gbUpdateLikerLogger";
            this.gbUpdateLikerLogger.Size = new System.Drawing.Size(854, 157);
            this.gbUpdateLikerLogger.TabIndex = 101;
            this.gbUpdateLikerLogger.TabStop = false;
            this.gbUpdateLikerLogger.Text = "Logger";
            // 
            // lstLoglinkdinScarper
            // 
            this.lstLoglinkdinScarper.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstLoglinkdinScarper.FormattingEnabled = true;
            this.lstLoglinkdinScarper.HorizontalScrollbar = true;
            this.lstLoglinkdinScarper.ItemHeight = 15;
            this.lstLoglinkdinScarper.Location = new System.Drawing.Point(15, 19);
            this.lstLoglinkdinScarper.Name = "lstLoglinkdinScarper";
            this.lstLoglinkdinScarper.ScrollAlwaysVisible = true;
            this.lstLoglinkdinScarper.Size = new System.Drawing.Size(829, 124);
            this.lstLoglinkdinScarper.TabIndex = 75;
            // 
            // gbUpdateLikerAction
            // 
            this.gbUpdateLikerAction.BackColor = System.Drawing.Color.Transparent;
            this.gbUpdateLikerAction.Controls.Add(this.btnStatusLikerStop);
            this.gbUpdateLikerAction.Controls.Add(this.btnUpdateLike);
            this.gbUpdateLikerAction.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbUpdateLikerAction.Location = new System.Drawing.Point(12, 280);
            this.gbUpdateLikerAction.Name = "gbUpdateLikerAction";
            this.gbUpdateLikerAction.Size = new System.Drawing.Size(854, 60);
            this.gbUpdateLikerAction.TabIndex = 124;
            this.gbUpdateLikerAction.TabStop = false;
            this.gbUpdateLikerAction.Text = "Submit Action";
            // 
            // frmGroupUpdateLiker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(885, 513);
            this.Controls.Add(this.gbUpdateLikerAction);
            this.Controls.Add(this.gbUpdateLikerDelaySetting);
            this.Controls.Add(this.gbUpdateLikerLogger);
            this.Controls.Add(this.gbUpdateLikerAccountsInput);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmGroupUpdateLiker";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "StatusUpdateLiker";
            this.Load += new System.EventHandler(this.frmGroupUpdateLiker_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.frmGroupUpdateLiker_Paint);
            this.gbUpdateLikerAccountsInput.ResumeLayout(false);
            this.gbUpdateLikerAccountsInput.PerformLayout();
            this.gbUpdateLikerDelaySetting.ResumeLayout(false);
            this.gbUpdateLikerDelaySetting.PerformLayout();
            this.gbUpdateLikerLogger.ResumeLayout(false);
            this.gbUpdateLikerAction.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbUpdateLikerAccountsInput;
        private System.Windows.Forms.GroupBox gbUpdateLikerDelaySetting;
        private System.Windows.Forms.TextBox txtGroupUpdateMaxDelay;
        private System.Windows.Forms.Label label94;
        private System.Windows.Forms.Label label95;
        private System.Windows.Forms.TextBox txtGroupUpdateMinDelay;
        private System.Windows.Forms.Button btnStatusLikerStop;
        private System.Windows.Forms.Label label47;
        private System.Windows.Forms.CheckBox chkGroupUpdate;
        private System.Windows.Forms.CheckedListBox chkUpdateCollection;
        private System.Windows.Forms.Button btnGetUser;
        private System.Windows.Forms.ComboBox cmbGroupUser;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button btnUpdateLike;
        private System.Windows.Forms.GroupBox gbUpdateLikerLogger;
        private System.Windows.Forms.ListBox lstLoglinkdinScarper;
        private System.Windows.Forms.Label label113;
        private System.Windows.Forms.GroupBox gbUpdateLikerAction;

    }
}