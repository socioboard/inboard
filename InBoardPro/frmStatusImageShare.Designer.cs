namespace InBoardPro
{
    partial class frmStatusImageShare
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmStatusImageShare));
            this.gbProfilePhotoInput = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnStatusImage = new System.Windows.Forms.Button();
            this.label39 = new System.Windows.Forms.Label();
            this.txtProfilePicFolder_LinkedinProfileManager = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lstBoxLogsStatusImage = new System.Windows.Forms.ListBox();
            this.gbProfilePicThread = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtThreads = new System.Windows.Forms.TextBox();
            this.label65 = new System.Windows.Forms.Label();
            this.gbProfilePhotoSetting = new System.Windows.Forms.GroupBox();
            this.gbStatusUpdateDelaySetting = new System.Windows.Forms.GroupBox();
            this.label106 = new System.Windows.Forms.Label();
            this.txtStatusUpdateMaxDelay = new System.Windows.Forms.TextBox();
            this.label90 = new System.Windows.Forms.Label();
            this.label91 = new System.Windows.Forms.Label();
            this.txtStatusUpdateMinDelay = new System.Windows.Forms.TextBox();
            this.btnProfPicStop = new System.Windows.Forms.Button();
            this.btnAddStatusImage = new System.Windows.Forms.Button();
            this.gbProfilePhotoAction = new System.Windows.Forms.GroupBox();
            this.gbProfilePhotoInput.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.gbProfilePicThread.SuspendLayout();
            this.gbProfilePhotoSetting.SuspendLayout();
            this.gbStatusUpdateDelaySetting.SuspendLayout();
            this.gbProfilePhotoAction.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbProfilePhotoInput
            // 
            this.gbProfilePhotoInput.BackColor = System.Drawing.Color.Transparent;
            this.gbProfilePhotoInput.Controls.Add(this.label1);
            this.gbProfilePhotoInput.Controls.Add(this.btnStatusImage);
            this.gbProfilePhotoInput.Controls.Add(this.label39);
            this.gbProfilePhotoInput.Controls.Add(this.txtProfilePicFolder_LinkedinProfileManager);
            this.gbProfilePhotoInput.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbProfilePhotoInput.Location = new System.Drawing.Point(12, 25);
            this.gbProfilePhotoInput.Name = "gbProfilePhotoInput";
            this.gbProfilePhotoInput.Size = new System.Drawing.Size(814, 83);
            this.gbProfilePhotoInput.TabIndex = 31;
            this.gbProfilePhotoInput.TabStop = false;
            this.gbProfilePhotoInput.Text = "Add Status Image For Share";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(161, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(327, 13);
            this.label1.TabIndex = 54;
            this.label1.Text = "Note: You can upload a JPG, GIF or PNG file (File size limit is 4 MB).";
            // 
            // btnStatusImage
            // 
            this.btnStatusImage.BackColor = System.Drawing.SystemColors.MenuBar;
            this.btnStatusImage.BackgroundImage = global::InBoardPro.Properties.Resources.browse_btn;
            this.btnStatusImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnStatusImage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStatusImage.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnStatusImage.Location = new System.Drawing.Point(684, 29);
            this.btnStatusImage.Name = "btnStatusImage";
            this.btnStatusImage.Size = new System.Drawing.Size(75, 26);
            this.btnStatusImage.TabIndex = 53;
            this.btnStatusImage.UseVisualStyleBackColor = false;
            this.btnStatusImage.Click += new System.EventHandler(this.btnStatusImage_Click);
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label39.Location = new System.Drawing.Point(20, 36);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(128, 13);
            this.label39.TabIndex = 51;
            this.label39.Text = "Status Image Folder:";
            // 
            // txtProfilePicFolder_LinkedinProfileManager
            // 
            this.txtProfilePicFolder_LinkedinProfileManager.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.txtProfilePicFolder_LinkedinProfileManager.Location = new System.Drawing.Point(164, 33);
            this.txtProfilePicFolder_LinkedinProfileManager.Name = "txtProfilePicFolder_LinkedinProfileManager";
            this.txtProfilePicFolder_LinkedinProfileManager.ReadOnly = true;
            this.txtProfilePicFolder_LinkedinProfileManager.Size = new System.Drawing.Size(511, 20);
            this.txtProfilePicFolder_LinkedinProfileManager.TabIndex = 52;
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.Controls.Add(this.lstBoxLogsStatusImage);
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(12, 275);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(814, 163);
            this.groupBox2.TabIndex = 169;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Logger";
            // 
            // lstBoxLogsStatusImage
            // 
            this.lstBoxLogsStatusImage.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstBoxLogsStatusImage.FormattingEnabled = true;
            this.lstBoxLogsStatusImage.HorizontalScrollbar = true;
            this.lstBoxLogsStatusImage.Location = new System.Drawing.Point(14, 19);
            this.lstBoxLogsStatusImage.Name = "lstBoxLogsStatusImage";
            this.lstBoxLogsStatusImage.ScrollAlwaysVisible = true;
            this.lstBoxLogsStatusImage.Size = new System.Drawing.Size(790, 134);
            this.lstBoxLogsStatusImage.TabIndex = 33;
            // 
            // gbProfilePicThread
            // 
            this.gbProfilePicThread.Controls.Add(this.label2);
            this.gbProfilePicThread.Controls.Add(this.txtThreads);
            this.gbProfilePicThread.Controls.Add(this.label65);
            this.gbProfilePicThread.Location = new System.Drawing.Point(6, 21);
            this.gbProfilePicThread.Name = "gbProfilePicThread";
            this.gbProfilePicThread.Size = new System.Drawing.Size(204, 51);
            this.gbProfilePicThread.TabIndex = 170;
            this.gbProfilePicThread.TabStop = false;
            this.gbProfilePicThread.Text = "Thread Setting";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(132, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 169;
            this.label2.Text = "(Seconds)";
            // 
            // txtThreads
            // 
            this.txtThreads.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtThreads.Location = new System.Drawing.Point(77, 21);
            this.txtThreads.Name = "txtThreads";
            this.txtThreads.Size = new System.Drawing.Size(49, 21);
            this.txtThreads.TabIndex = 167;
            this.txtThreads.Text = "7";
            this.txtThreads.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label65
            // 
            this.label65.AutoSize = true;
            this.label65.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label65.Location = new System.Drawing.Point(14, 24);
            this.label65.Name = "label65";
            this.label65.Size = new System.Drawing.Size(58, 13);
            this.label65.TabIndex = 168;
            this.label65.Text = "Threads:";
            // 
            // gbProfilePhotoSetting
            // 
            this.gbProfilePhotoSetting.BackColor = System.Drawing.Color.Transparent;
            this.gbProfilePhotoSetting.Controls.Add(this.gbStatusUpdateDelaySetting);
            this.gbProfilePhotoSetting.Controls.Add(this.gbProfilePicThread);
            this.gbProfilePhotoSetting.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbProfilePhotoSetting.Location = new System.Drawing.Point(12, 114);
            this.gbProfilePhotoSetting.Name = "gbProfilePhotoSetting";
            this.gbProfilePhotoSetting.Size = new System.Drawing.Size(814, 86);
            this.gbProfilePhotoSetting.TabIndex = 171;
            this.gbProfilePhotoSetting.TabStop = false;
            this.gbProfilePhotoSetting.Text = "Settings";
            // 
            // gbStatusUpdateDelaySetting
            // 
            this.gbStatusUpdateDelaySetting.Controls.Add(this.label106);
            this.gbStatusUpdateDelaySetting.Controls.Add(this.txtStatusUpdateMaxDelay);
            this.gbStatusUpdateDelaySetting.Controls.Add(this.label90);
            this.gbStatusUpdateDelaySetting.Controls.Add(this.label91);
            this.gbStatusUpdateDelaySetting.Controls.Add(this.txtStatusUpdateMinDelay);
            this.gbStatusUpdateDelaySetting.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbStatusUpdateDelaySetting.Location = new System.Drawing.Point(228, 21);
            this.gbStatusUpdateDelaySetting.Name = "gbStatusUpdateDelaySetting";
            this.gbStatusUpdateDelaySetting.Size = new System.Drawing.Size(531, 52);
            this.gbStatusUpdateDelaySetting.TabIndex = 171;
            this.gbStatusUpdateDelaySetting.TabStop = false;
            this.gbStatusUpdateDelaySetting.Text = "Delay Between Status Update";
            // 
            // label106
            // 
            this.label106.AutoSize = true;
            this.label106.BackColor = System.Drawing.Color.Transparent;
            this.label106.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label106.Location = new System.Drawing.Point(201, 26);
            this.label106.Name = "label106";
            this.label106.Size = new System.Drawing.Size(311, 13);
            this.label106.TabIndex = 121;
            this.label106.Text = "(Its randomly delay between both values in seconds)";
            // 
            // txtStatusUpdateMaxDelay
            // 
            this.txtStatusUpdateMaxDelay.BackColor = System.Drawing.Color.White;
            this.txtStatusUpdateMaxDelay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtStatusUpdateMaxDelay.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtStatusUpdateMaxDelay.Location = new System.Drawing.Point(152, 23);
            this.txtStatusUpdateMaxDelay.Name = "txtStatusUpdateMaxDelay";
            this.txtStatusUpdateMaxDelay.Size = new System.Drawing.Size(43, 21);
            this.txtStatusUpdateMaxDelay.TabIndex = 119;
            this.txtStatusUpdateMaxDelay.Text = "25";
            this.txtStatusUpdateMaxDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label90
            // 
            this.label90.AutoSize = true;
            this.label90.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label90.Location = new System.Drawing.Point(9, 25);
            this.label90.Name = "label90";
            this.label90.Size = new System.Drawing.Size(45, 13);
            this.label90.TabIndex = 116;
            this.label90.Text = "Delay:";
            // 
            // label91
            // 
            this.label91.AutoSize = true;
            this.label91.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label91.Location = new System.Drawing.Point(121, 26);
            this.label91.Name = "label91";
            this.label91.Size = new System.Drawing.Size(18, 13);
            this.label91.TabIndex = 117;
            this.label91.Text = "to";
            // 
            // txtStatusUpdateMinDelay
            // 
            this.txtStatusUpdateMinDelay.BackColor = System.Drawing.Color.White;
            this.txtStatusUpdateMinDelay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtStatusUpdateMinDelay.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtStatusUpdateMinDelay.Location = new System.Drawing.Point(69, 23);
            this.txtStatusUpdateMinDelay.Name = "txtStatusUpdateMinDelay";
            this.txtStatusUpdateMinDelay.Size = new System.Drawing.Size(44, 21);
            this.txtStatusUpdateMinDelay.TabIndex = 118;
            this.txtStatusUpdateMinDelay.Text = "20";
            this.txtStatusUpdateMinDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnProfPicStop
            // 
            this.btnProfPicStop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnProfPicStop.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnProfPicStop.Image = global::InBoardPro.Properties.Resources.stop;
            this.btnProfPicStop.Location = new System.Drawing.Point(420, 20);
            this.btnProfPicStop.Name = "btnProfPicStop";
            this.btnProfPicStop.Size = new System.Drawing.Size(124, 30);
            this.btnProfPicStop.TabIndex = 169;
            this.btnProfPicStop.UseVisualStyleBackColor = true;
            // 
            // btnAddStatusImage
            // 
            this.btnAddStatusImage.BackColor = System.Drawing.Color.Transparent;
            this.btnAddStatusImage.BackgroundImage = global::InBoardPro.Properties.Resources.add_profilepicture;
            this.btnAddStatusImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnAddStatusImage.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAddStatusImage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Bold);
            this.btnAddStatusImage.Location = new System.Drawing.Point(244, 19);
            this.btnAddStatusImage.Name = "btnAddStatusImage";
            this.btnAddStatusImage.Size = new System.Drawing.Size(155, 31);
            this.btnAddStatusImage.TabIndex = 31;
            this.btnAddStatusImage.UseVisualStyleBackColor = false;
            this.btnAddStatusImage.Click += new System.EventHandler(this.btnAddStatusImage_Click);
            // 
            // gbProfilePhotoAction
            // 
            this.gbProfilePhotoAction.BackColor = System.Drawing.Color.Transparent;
            this.gbProfilePhotoAction.Controls.Add(this.btnAddStatusImage);
            this.gbProfilePhotoAction.Controls.Add(this.btnProfPicStop);
            this.gbProfilePhotoAction.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbProfilePhotoAction.Location = new System.Drawing.Point(12, 206);
            this.gbProfilePhotoAction.Name = "gbProfilePhotoAction";
            this.gbProfilePhotoAction.Size = new System.Drawing.Size(814, 61);
            this.gbProfilePhotoAction.TabIndex = 172;
            this.gbProfilePhotoAction.TabStop = false;
            this.gbProfilePhotoAction.Text = "Submit Action";
            // 
            // frmStatusImageShare
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(855, 450);
            this.Controls.Add(this.gbProfilePhotoAction);
            this.Controls.Add(this.gbProfilePhotoSetting);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.gbProfilePhotoInput);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmStatusImageShare";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmStatusImageShare";
            this.Load += new System.EventHandler(this.frmStatusImageShare_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.frmStatusImageShare_Paint);
            this.gbProfilePhotoInput.ResumeLayout(false);
            this.gbProfilePhotoInput.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.gbProfilePicThread.ResumeLayout(false);
            this.gbProfilePicThread.PerformLayout();
            this.gbProfilePhotoSetting.ResumeLayout(false);
            this.gbStatusUpdateDelaySetting.ResumeLayout(false);
            this.gbStatusUpdateDelaySetting.PerformLayout();
            this.gbProfilePhotoAction.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbProfilePhotoInput;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnStatusImage;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.TextBox txtProfilePicFolder_LinkedinProfileManager;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListBox lstBoxLogsStatusImage;
        private System.Windows.Forms.GroupBox gbProfilePicThread;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtThreads;
        private System.Windows.Forms.Label label65;
        private System.Windows.Forms.GroupBox gbProfilePhotoSetting;
        private System.Windows.Forms.Button btnProfPicStop;
        private System.Windows.Forms.Button btnAddStatusImage;
        private System.Windows.Forms.GroupBox gbProfilePhotoAction;
        private System.Windows.Forms.GroupBox gbStatusUpdateDelaySetting;
        private System.Windows.Forms.Label label106;
        private System.Windows.Forms.TextBox txtStatusUpdateMaxDelay;
        private System.Windows.Forms.Label label90;
        private System.Windows.Forms.Label label91;
        private System.Windows.Forms.TextBox txtStatusUpdateMinDelay;
    }
}