namespace InBoardPro
{
    partial class FrmAcceptInvitations
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAcceptInvitations));
            this.gbAcceptInvitationAction = new System.Windows.Forms.GroupBox();
            this.BtnStop = new System.Windows.Forms.Button();
            this.BtnAccectInvitation = new System.Windows.Forms.Button();
            this.gbAcceptInvitationLogger = new System.Windows.Forms.GroupBox();
            this.LstLogger_AcceptInvitations = new System.Windows.Forms.ListBox();
            this.gbAcceptInvitationSetting = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textNoOfThread = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.gbAcceptInvitationAction.SuspendLayout();
            this.gbAcceptInvitationLogger.SuspendLayout();
            this.gbAcceptInvitationSetting.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbAcceptInvitationAction
            // 
            this.gbAcceptInvitationAction.BackColor = System.Drawing.Color.Transparent;
            this.gbAcceptInvitationAction.Controls.Add(this.BtnStop);
            this.gbAcceptInvitationAction.Controls.Add(this.BtnAccectInvitation);
            this.gbAcceptInvitationAction.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbAcceptInvitationAction.Location = new System.Drawing.Point(18, 104);
            this.gbAcceptInvitationAction.Name = "gbAcceptInvitationAction";
            this.gbAcceptInvitationAction.Size = new System.Drawing.Size(572, 66);
            this.gbAcceptInvitationAction.TabIndex = 0;
            this.gbAcceptInvitationAction.TabStop = false;
            this.gbAcceptInvitationAction.Text = "Submit Action";
            // 
            // BtnStop
            // 
            this.BtnStop.BackgroundImage = global::InBoardPro.Properties.Resources.stop;
            this.BtnStop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.BtnStop.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.BtnStop.Location = new System.Drawing.Point(272, 19);
            this.BtnStop.Name = "BtnStop";
            this.BtnStop.Size = new System.Drawing.Size(126, 31);
            this.BtnStop.TabIndex = 1;
            this.BtnStop.UseVisualStyleBackColor = true;
            this.BtnStop.Click += new System.EventHandler(this.BtnStop_Click);
            // 
            // BtnAccectInvitation
            // 
            this.BtnAccectInvitation.BackgroundImage = global::InBoardPro.Properties.Resources.AcceptInvitation;
            this.BtnAccectInvitation.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.BtnAccectInvitation.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.BtnAccectInvitation.Location = new System.Drawing.Point(118, 19);
            this.BtnAccectInvitation.Name = "BtnAccectInvitation";
            this.BtnAccectInvitation.Size = new System.Drawing.Size(140, 31);
            this.BtnAccectInvitation.TabIndex = 0;
            this.BtnAccectInvitation.UseVisualStyleBackColor = true;
            this.BtnAccectInvitation.Click += new System.EventHandler(this.BtnAccectInvitation_Click);
            // 
            // gbAcceptInvitationLogger
            // 
            this.gbAcceptInvitationLogger.BackColor = System.Drawing.Color.Transparent;
            this.gbAcceptInvitationLogger.Controls.Add(this.LstLogger_AcceptInvitations);
            this.gbAcceptInvitationLogger.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbAcceptInvitationLogger.Location = new System.Drawing.Point(18, 178);
            this.gbAcceptInvitationLogger.Name = "gbAcceptInvitationLogger";
            this.gbAcceptInvitationLogger.Size = new System.Drawing.Size(572, 238);
            this.gbAcceptInvitationLogger.TabIndex = 1;
            this.gbAcceptInvitationLogger.TabStop = false;
            this.gbAcceptInvitationLogger.Text = "Logger";
            // 
            // LstLogger_AcceptInvitations
            // 
            this.LstLogger_AcceptInvitations.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LstLogger_AcceptInvitations.FormattingEnabled = true;
            this.LstLogger_AcceptInvitations.HorizontalScrollbar = true;
            this.LstLogger_AcceptInvitations.Location = new System.Drawing.Point(6, 19);
            this.LstLogger_AcceptInvitations.Name = "LstLogger_AcceptInvitations";
            this.LstLogger_AcceptInvitations.Size = new System.Drawing.Size(550, 212);
            this.LstLogger_AcceptInvitations.TabIndex = 0;
            // 
            // gbAcceptInvitationSetting
            // 
            this.gbAcceptInvitationSetting.BackColor = System.Drawing.Color.Transparent;
            this.gbAcceptInvitationSetting.Controls.Add(this.label1);
            this.gbAcceptInvitationSetting.Controls.Add(this.textNoOfThread);
            this.gbAcceptInvitationSetting.Controls.Add(this.label23);
            this.gbAcceptInvitationSetting.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbAcceptInvitationSetting.Location = new System.Drawing.Point(18, 22);
            this.gbAcceptInvitationSetting.Name = "gbAcceptInvitationSetting";
            this.gbAcceptInvitationSetting.Size = new System.Drawing.Size(572, 73);
            this.gbAcceptInvitationSetting.TabIndex = 2;
            this.gbAcceptInvitationSetting.TabStop = false;
            this.gbAcceptInvitationSetting.Text = "Thread Setting";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(186, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(175, 13);
            this.label1.TabIndex = 143;
            this.label1.Text = "(Please Enter Numeric Value)";
            // 
            // textNoOfThread
            // 
            this.textNoOfThread.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textNoOfThread.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textNoOfThread.Location = new System.Drawing.Point(118, 30);
            this.textNoOfThread.Name = "textNoOfThread";
            this.textNoOfThread.Size = new System.Drawing.Size(52, 21);
            this.textNoOfThread.TabIndex = 142;
            this.textNoOfThread.Text = "5";
            this.textNoOfThread.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.Location = new System.Drawing.Point(19, 33);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(89, 13);
            this.label23.TabIndex = 141;
            this.label23.Text = "No Of Threads";
            // 
            // FrmAcceptInvitations
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(608, 427);
            this.Controls.Add(this.gbAcceptInvitationSetting);
            this.Controls.Add(this.gbAcceptInvitationLogger);
            this.Controls.Add(this.gbAcceptInvitationAction);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FrmAcceptInvitations";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Accept Invitations";
            this.Load += new System.EventHandler(this.FrmAcceptInvitations_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FrmAcceptInvitations_Paint);
            this.gbAcceptInvitationAction.ResumeLayout(false);
            this.gbAcceptInvitationLogger.ResumeLayout(false);
            this.gbAcceptInvitationSetting.ResumeLayout(false);
            this.gbAcceptInvitationSetting.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbAcceptInvitationAction;
        private System.Windows.Forms.Button BtnAccectInvitation;
        private System.Windows.Forms.GroupBox gbAcceptInvitationLogger;
        private System.Windows.Forms.ListBox LstLogger_AcceptInvitations;
        private System.Windows.Forms.GroupBox gbAcceptInvitationSetting;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textNoOfThread;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Button BtnStop;
    }
}