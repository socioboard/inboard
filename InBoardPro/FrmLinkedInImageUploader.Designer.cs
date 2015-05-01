namespace InBoardPro
{
    partial class FrmLinkedInImageUploader
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmLinkedInImageUploader));
            this.gbProfilePhotoInput = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnProfilePicsFolder_LinkedInProfileManager = new System.Windows.Forms.Button();
            this.label39 = new System.Windows.Forms.Label();
            this.txtProfilePicFolder_LinkedinProfileManager = new System.Windows.Forms.TextBox();
            this.gbProfilePicThread = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtThreads = new System.Windows.Forms.TextBox();
            this.label65 = new System.Windows.Forms.Label();
            this.btnProfPicStop = new System.Windows.Forms.Button();
            this.btnAddProfilePic = new System.Windows.Forms.Button();
            this.gbProfilePicSelMode = new System.Windows.Forms.GroupBox();
            this.rdbRandom = new System.Windows.Forms.RadioButton();
            this.rdbUnique = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lstBoxLogsProfileManager = new System.Windows.Forms.ListBox();
            this.gbProfilePhotoSetting = new System.Windows.Forms.GroupBox();
            this.gbProfilePhotoAction = new System.Windows.Forms.GroupBox();
            this.gbProfilePhotoInput.SuspendLayout();
            this.gbProfilePicThread.SuspendLayout();
            this.gbProfilePicSelMode.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.gbProfilePhotoSetting.SuspendLayout();
            this.gbProfilePhotoAction.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbProfilePhotoInput
            // 
            this.gbProfilePhotoInput.BackColor = System.Drawing.Color.Transparent;
            this.gbProfilePhotoInput.Controls.Add(this.label1);
            this.gbProfilePhotoInput.Controls.Add(this.btnProfilePicsFolder_LinkedInProfileManager);
            this.gbProfilePhotoInput.Controls.Add(this.label39);
            this.gbProfilePhotoInput.Controls.Add(this.txtProfilePicFolder_LinkedinProfileManager);
            this.gbProfilePhotoInput.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbProfilePhotoInput.Location = new System.Drawing.Point(12, 12);
            this.gbProfilePhotoInput.Name = "gbProfilePhotoInput";
            this.gbProfilePhotoInput.Size = new System.Drawing.Size(814, 83);
            this.gbProfilePhotoInput.TabIndex = 30;
            this.gbProfilePhotoInput.TabStop = false;
            this.gbProfilePhotoInput.Text = "Add Profile Photo Input";
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
            // btnProfilePicsFolder_LinkedInProfileManager
            // 
            this.btnProfilePicsFolder_LinkedInProfileManager.BackColor = System.Drawing.SystemColors.MenuBar;
            this.btnProfilePicsFolder_LinkedInProfileManager.BackgroundImage = global::InBoardPro.Properties.Resources.browse_btn;
            this.btnProfilePicsFolder_LinkedInProfileManager.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnProfilePicsFolder_LinkedInProfileManager.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnProfilePicsFolder_LinkedInProfileManager.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnProfilePicsFolder_LinkedInProfileManager.Location = new System.Drawing.Point(684, 29);
            this.btnProfilePicsFolder_LinkedInProfileManager.Name = "btnProfilePicsFolder_LinkedInProfileManager";
            this.btnProfilePicsFolder_LinkedInProfileManager.Size = new System.Drawing.Size(75, 26);
            this.btnProfilePicsFolder_LinkedInProfileManager.TabIndex = 53;
            this.btnProfilePicsFolder_LinkedInProfileManager.UseVisualStyleBackColor = false;
            this.btnProfilePicsFolder_LinkedInProfileManager.Click += new System.EventHandler(this.btnProfilePicsFolder_LinkedInProfileManager_Click);
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label39.Location = new System.Drawing.Point(20, 36);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(123, 13);
            this.label39.TabIndex = 51;
            this.label39.Text = "Profile Photo Folder:";
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
            // gbProfilePicThread
            // 
            this.gbProfilePicThread.Controls.Add(this.label2);
            this.gbProfilePicThread.Controls.Add(this.txtThreads);
            this.gbProfilePicThread.Controls.Add(this.label65);
            this.gbProfilePicThread.Location = new System.Drawing.Point(164, 20);
            this.gbProfilePicThread.Name = "gbProfilePicThread";
            this.gbProfilePicThread.Size = new System.Drawing.Size(284, 51);
            this.gbProfilePicThread.TabIndex = 170;
            this.gbProfilePicThread.TabStop = false;
            this.gbProfilePicThread.Text = "Thread Setting";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(203, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 169;
            this.label2.Text = "(Seconds)";
            // 
            // txtThreads
            // 
            this.txtThreads.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtThreads.Location = new System.Drawing.Point(148, 20);
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
            this.label65.Location = new System.Drawing.Point(85, 23);
            this.label65.Name = "label65";
            this.label65.Size = new System.Drawing.Size(58, 13);
            this.label65.TabIndex = 168;
            this.label65.Text = "Threads:";
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
            this.btnProfPicStop.Click += new System.EventHandler(this.btnProfPicStop_Click);
            // 
            // btnAddProfilePic
            // 
            this.btnAddProfilePic.BackColor = System.Drawing.Color.Transparent;
            this.btnAddProfilePic.BackgroundImage = global::InBoardPro.Properties.Resources.add_profilepicture;
            this.btnAddProfilePic.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnAddProfilePic.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAddProfilePic.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Bold);
            this.btnAddProfilePic.Location = new System.Drawing.Point(244, 19);
            this.btnAddProfilePic.Name = "btnAddProfilePic";
            this.btnAddProfilePic.Size = new System.Drawing.Size(155, 31);
            this.btnAddProfilePic.TabIndex = 31;
            this.btnAddProfilePic.UseVisualStyleBackColor = false;
            this.btnAddProfilePic.Click += new System.EventHandler(this.btnAddProfilePic_Click);
            // 
            // gbProfilePicSelMode
            // 
            this.gbProfilePicSelMode.Controls.Add(this.rdbRandom);
            this.gbProfilePicSelMode.Controls.Add(this.rdbUnique);
            this.gbProfilePicSelMode.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbProfilePicSelMode.Location = new System.Drawing.Point(454, 20);
            this.gbProfilePicSelMode.Name = "gbProfilePicSelMode";
            this.gbProfilePicSelMode.Size = new System.Drawing.Size(245, 51);
            this.gbProfilePicSelMode.TabIndex = 50;
            this.gbProfilePicSelMode.TabStop = false;
            this.gbProfilePicSelMode.Text = "Selection Mode";
            // 
            // rdbRandom
            // 
            this.rdbRandom.AutoSize = true;
            this.rdbRandom.Checked = true;
            this.rdbRandom.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdbRandom.Location = new System.Drawing.Point(152, 20);
            this.rdbRandom.Name = "rdbRandom";
            this.rdbRandom.Size = new System.Drawing.Size(72, 17);
            this.rdbRandom.TabIndex = 1;
            this.rdbRandom.TabStop = true;
            this.rdbRandom.Text = "Random";
            this.rdbRandom.UseVisualStyleBackColor = true;
            // 
            // rdbUnique
            // 
            this.rdbUnique.AutoSize = true;
            this.rdbUnique.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdbUnique.Location = new System.Drawing.Point(69, 20);
            this.rdbUnique.Name = "rdbUnique";
            this.rdbUnique.Size = new System.Drawing.Size(64, 17);
            this.rdbUnique.TabIndex = 0;
            this.rdbUnique.Text = "Unique";
            this.rdbUnique.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.Controls.Add(this.lstBoxLogsProfileManager);
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(12, 254);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(814, 155);
            this.groupBox2.TabIndex = 168;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Logger";
            // 
            // lstBoxLogsProfileManager
            // 
            this.lstBoxLogsProfileManager.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstBoxLogsProfileManager.FormattingEnabled = true;
            this.lstBoxLogsProfileManager.HorizontalScrollbar = true;
            this.lstBoxLogsProfileManager.Location = new System.Drawing.Point(14, 19);
            this.lstBoxLogsProfileManager.Name = "lstBoxLogsProfileManager";
            this.lstBoxLogsProfileManager.ScrollAlwaysVisible = true;
            this.lstBoxLogsProfileManager.Size = new System.Drawing.Size(790, 121);
            this.lstBoxLogsProfileManager.TabIndex = 33;
            // 
            // gbProfilePhotoSetting
            // 
            this.gbProfilePhotoSetting.BackColor = System.Drawing.Color.Transparent;
            this.gbProfilePhotoSetting.Controls.Add(this.gbProfilePicThread);
            this.gbProfilePhotoSetting.Controls.Add(this.gbProfilePicSelMode);
            this.gbProfilePhotoSetting.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbProfilePhotoSetting.Location = new System.Drawing.Point(12, 96);
            this.gbProfilePhotoSetting.Name = "gbProfilePhotoSetting";
            this.gbProfilePhotoSetting.Size = new System.Drawing.Size(814, 86);
            this.gbProfilePhotoSetting.TabIndex = 170;
            this.gbProfilePhotoSetting.TabStop = false;
            this.gbProfilePhotoSetting.Text = "Settings";
            // 
            // gbProfilePhotoAction
            // 
            this.gbProfilePhotoAction.BackColor = System.Drawing.Color.Transparent;
            this.gbProfilePhotoAction.Controls.Add(this.btnAddProfilePic);
            this.gbProfilePhotoAction.Controls.Add(this.btnProfPicStop);
            this.gbProfilePhotoAction.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbProfilePhotoAction.Location = new System.Drawing.Point(12, 187);
            this.gbProfilePhotoAction.Name = "gbProfilePhotoAction";
            this.gbProfilePhotoAction.Size = new System.Drawing.Size(814, 61);
            this.gbProfilePhotoAction.TabIndex = 171;
            this.gbProfilePhotoAction.TabStop = false;
            this.gbProfilePhotoAction.Text = "Submit Action";
            // 
            // FrmLinkedInImageUploader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(838, 414);
            this.Controls.Add(this.gbProfilePhotoAction);
            this.Controls.Add(this.gbProfilePhotoSetting);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.gbProfilePhotoInput);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FrmLinkedInImageUploader";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LinkedInProfile Image Uploader";
            this.Load += new System.EventHandler(this.FrmLinkedInProfileManager_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FrmLinkedInProfileManager_Paint);
            this.gbProfilePhotoInput.ResumeLayout(false);
            this.gbProfilePhotoInput.PerformLayout();
            this.gbProfilePicThread.ResumeLayout(false);
            this.gbProfilePicThread.PerformLayout();
            this.gbProfilePicSelMode.ResumeLayout(false);
            this.gbProfilePicSelMode.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.gbProfilePhotoSetting.ResumeLayout(false);
            this.gbProfilePhotoAction.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbProfilePhotoInput;
        private System.Windows.Forms.Button btnProfilePicsFolder_LinkedInProfileManager;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.TextBox txtProfilePicFolder_LinkedinProfileManager;
        private System.Windows.Forms.GroupBox gbProfilePicSelMode;
        private System.Windows.Forms.RadioButton rdbRandom;
        private System.Windows.Forms.RadioButton rdbUnique;
        private System.Windows.Forms.Button btnAddProfilePic;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label65;
        private System.Windows.Forms.TextBox txtThreads;
        private System.Windows.Forms.Button btnProfPicStop;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListBox lstBoxLogsProfileManager;
        private System.Windows.Forms.GroupBox gbProfilePicThread;
        private System.Windows.Forms.GroupBox gbProfilePhotoSetting;
        private System.Windows.Forms.GroupBox gbProfilePhotoAction;
        private System.Windows.Forms.Label label2;
    }
}