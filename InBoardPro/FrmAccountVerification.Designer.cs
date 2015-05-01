namespace InBoardPro
{
    partial class FrmAccountVerification
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAccountVerification));
            this.gbEmailInputsAcountVerification = new System.Windows.Forms.GroupBox();
            this.btnEmailsBrowse_AccountVerification = new System.Windows.Forms.Button();
            this.txtEmails_AccountVerification = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textNoofThread_AccountVerification = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.gbAccountVerificationLogger = new System.Windows.Forms.GroupBox();
            this.LstBoxLogger_AccountVerification = new System.Windows.Forms.ListBox();
            this.gbAccountVerificationAction = new System.Windows.Forms.GroupBox();
            this.btnAcVerificationStop = new System.Windows.Forms.Button();
            this.btnResendVerfication = new System.Windows.Forms.Button();
            this.btnAccountVerification_AccountVerification = new System.Windows.Forms.Button();
            this.gbAccountVerificationThreadSetting = new System.Windows.Forms.GroupBox();
            this.gbEmailInputsAcountVerification.SuspendLayout();
            this.gbAccountVerificationLogger.SuspendLayout();
            this.gbAccountVerificationAction.SuspendLayout();
            this.gbAccountVerificationThreadSetting.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbEmailInputsAcountVerification
            // 
            this.gbEmailInputsAcountVerification.BackColor = System.Drawing.Color.Transparent;
            this.gbEmailInputsAcountVerification.Controls.Add(this.btnEmailsBrowse_AccountVerification);
            this.gbEmailInputsAcountVerification.Controls.Add(this.txtEmails_AccountVerification);
            this.gbEmailInputsAcountVerification.Controls.Add(this.label2);
            this.gbEmailInputsAcountVerification.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbEmailInputsAcountVerification.Location = new System.Drawing.Point(12, 20);
            this.gbEmailInputsAcountVerification.Name = "gbEmailInputsAcountVerification";
            this.gbEmailInputsAcountVerification.Size = new System.Drawing.Size(629, 66);
            this.gbEmailInputsAcountVerification.TabIndex = 0;
            this.gbEmailInputsAcountVerification.TabStop = false;
            this.gbEmailInputsAcountVerification.Text = "Email Inputs for Account Verification";
            // 
            // btnEmailsBrowse_AccountVerification
            // 
            this.btnEmailsBrowse_AccountVerification.BackColor = System.Drawing.Color.White;
            this.btnEmailsBrowse_AccountVerification.BackgroundImage = global::InBoardPro.Properties.Resources.browse_btn;
            this.btnEmailsBrowse_AccountVerification.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnEmailsBrowse_AccountVerification.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnEmailsBrowse_AccountVerification.ForeColor = System.Drawing.Color.Black;
            this.btnEmailsBrowse_AccountVerification.Location = new System.Drawing.Point(531, 21);
            this.btnEmailsBrowse_AccountVerification.Name = "btnEmailsBrowse_AccountVerification";
            this.btnEmailsBrowse_AccountVerification.Size = new System.Drawing.Size(75, 29);
            this.btnEmailsBrowse_AccountVerification.TabIndex = 136;
            this.btnEmailsBrowse_AccountVerification.UseVisualStyleBackColor = false;
            this.btnEmailsBrowse_AccountVerification.Click += new System.EventHandler(this.btnEmailsBrowse_AccountVerification_Click);
            // 
            // txtEmails_AccountVerification
            // 
            this.txtEmails_AccountVerification.BackColor = System.Drawing.Color.White;
            this.txtEmails_AccountVerification.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEmails_AccountVerification.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEmails_AccountVerification.Location = new System.Drawing.Point(105, 26);
            this.txtEmails_AccountVerification.Name = "txtEmails_AccountVerification";
            this.txtEmails_AccountVerification.ReadOnly = true;
            this.txtEmails_AccountVerification.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtEmails_AccountVerification.Size = new System.Drawing.Size(420, 21);
            this.txtEmails_AccountVerification.TabIndex = 135;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(48, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 137;
            this.label2.Text = "Emails*:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(163, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 13);
            this.label1.TabIndex = 140;
            this.label1.Text = "(Numeric Value)";
            // 
            // textNoofThread_AccountVerification
            // 
            this.textNoofThread_AccountVerification.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textNoofThread_AccountVerification.Location = new System.Drawing.Point(105, 28);
            this.textNoofThread_AccountVerification.Name = "textNoofThread_AccountVerification";
            this.textNoofThread_AccountVerification.Size = new System.Drawing.Size(38, 21);
            this.textNoofThread_AccountVerification.TabIndex = 139;
            this.textNoofThread_AccountVerification.Text = "5";
            this.textNoofThread_AccountVerification.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.Location = new System.Drawing.Point(10, 31);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(94, 13);
            this.label23.TabIndex = 138;
            this.label23.Text = "No Of Threads:";
            // 
            // gbAccountVerificationLogger
            // 
            this.gbAccountVerificationLogger.BackColor = System.Drawing.Color.Transparent;
            this.gbAccountVerificationLogger.Controls.Add(this.LstBoxLogger_AccountVerification);
            this.gbAccountVerificationLogger.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbAccountVerificationLogger.Location = new System.Drawing.Point(11, 228);
            this.gbAccountVerificationLogger.Name = "gbAccountVerificationLogger";
            this.gbAccountVerificationLogger.Size = new System.Drawing.Size(629, 166);
            this.gbAccountVerificationLogger.TabIndex = 2;
            this.gbAccountVerificationLogger.TabStop = false;
            this.gbAccountVerificationLogger.Text = "Account Verification Logger";
            // 
            // LstBoxLogger_AccountVerification
            // 
            this.LstBoxLogger_AccountVerification.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LstBoxLogger_AccountVerification.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LstBoxLogger_AccountVerification.FormattingEnabled = true;
            this.LstBoxLogger_AccountVerification.HorizontalScrollbar = true;
            this.LstBoxLogger_AccountVerification.Location = new System.Drawing.Point(6, 19);
            this.LstBoxLogger_AccountVerification.Name = "LstBoxLogger_AccountVerification";
            this.LstBoxLogger_AccountVerification.ScrollAlwaysVisible = true;
            this.LstBoxLogger_AccountVerification.Size = new System.Drawing.Size(617, 145);
            this.LstBoxLogger_AccountVerification.TabIndex = 0;
            // 
            // gbAccountVerificationAction
            // 
            this.gbAccountVerificationAction.BackColor = System.Drawing.Color.Transparent;
            this.gbAccountVerificationAction.Controls.Add(this.btnAcVerificationStop);
            this.gbAccountVerificationAction.Controls.Add(this.btnResendVerfication);
            this.gbAccountVerificationAction.Controls.Add(this.btnAccountVerification_AccountVerification);
            this.gbAccountVerificationAction.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbAccountVerificationAction.Location = new System.Drawing.Point(12, 162);
            this.gbAccountVerificationAction.Name = "gbAccountVerificationAction";
            this.gbAccountVerificationAction.Size = new System.Drawing.Size(628, 60);
            this.gbAccountVerificationAction.TabIndex = 7;
            this.gbAccountVerificationAction.TabStop = false;
            this.gbAccountVerificationAction.Text = "Submit Action";
            // 
            // btnAcVerificationStop
            // 
            this.btnAcVerificationStop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnAcVerificationStop.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAcVerificationStop.Image = global::InBoardPro.Properties.Resources.stop;
            this.btnAcVerificationStop.Location = new System.Drawing.Point(420, 19);
            this.btnAcVerificationStop.Name = "btnAcVerificationStop";
            this.btnAcVerificationStop.Size = new System.Drawing.Size(120, 30);
            this.btnAcVerificationStop.TabIndex = 112;
            this.btnAcVerificationStop.UseVisualStyleBackColor = true;
            this.btnAcVerificationStop.Click += new System.EventHandler(this.btnAcVerificationStop_Click);
            // 
            // btnResendVerfication
            // 
            this.btnResendVerfication.BackgroundImage = global::InBoardPro.Properties.Resources.resend_verification;
            this.btnResendVerfication.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnResendVerfication.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnResendVerfication.Location = new System.Drawing.Point(242, 19);
            this.btnResendVerfication.Name = "btnResendVerfication";
            this.btnResendVerfication.Size = new System.Drawing.Size(160, 30);
            this.btnResendVerfication.TabIndex = 2;
            this.btnResendVerfication.UseVisualStyleBackColor = true;
            this.btnResendVerfication.Click += new System.EventHandler(this.btnResendVerfication_Click);
            // 
            // btnAccountVerification_AccountVerification
            // 
            this.btnAccountVerification_AccountVerification.BackColor = System.Drawing.Color.Transparent;
            this.btnAccountVerification_AccountVerification.BackgroundImage = global::InBoardPro.Properties.Resources.account_verfication;
            this.btnAccountVerification_AccountVerification.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnAccountVerification_AccountVerification.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAccountVerification_AccountVerification.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAccountVerification_AccountVerification.Location = new System.Drawing.Point(66, 18);
            this.btnAccountVerification_AccountVerification.Name = "btnAccountVerification_AccountVerification";
            this.btnAccountVerification_AccountVerification.Size = new System.Drawing.Size(160, 31);
            this.btnAccountVerification_AccountVerification.TabIndex = 1;
            this.btnAccountVerification_AccountVerification.UseVisualStyleBackColor = false;
            this.btnAccountVerification_AccountVerification.Click += new System.EventHandler(this.btnAccountVerification_AccountVerification_Click);
            // 
            // gbAccountVerificationThreadSetting
            // 
            this.gbAccountVerificationThreadSetting.BackColor = System.Drawing.Color.Transparent;
            this.gbAccountVerificationThreadSetting.Controls.Add(this.label1);
            this.gbAccountVerificationThreadSetting.Controls.Add(this.textNoofThread_AccountVerification);
            this.gbAccountVerificationThreadSetting.Controls.Add(this.label23);
            this.gbAccountVerificationThreadSetting.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbAccountVerificationThreadSetting.Location = new System.Drawing.Point(12, 92);
            this.gbAccountVerificationThreadSetting.Name = "gbAccountVerificationThreadSetting";
            this.gbAccountVerificationThreadSetting.Size = new System.Drawing.Size(629, 64);
            this.gbAccountVerificationThreadSetting.TabIndex = 8;
            this.gbAccountVerificationThreadSetting.TabStop = false;
            this.gbAccountVerificationThreadSetting.Text = "Thread Setting";
            // 
            // FrmAccountVerification
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(652, 404);
            this.Controls.Add(this.gbAccountVerificationThreadSetting);
            this.Controls.Add(this.gbAccountVerificationAction);
            this.Controls.Add(this.gbAccountVerificationLogger);
            this.Controls.Add(this.gbEmailInputsAcountVerification);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FrmAccountVerification";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AccountVerification";
            this.Load += new System.EventHandler(this.FrmAccountVerification_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FrmAccountVerification_Paint);
            this.gbEmailInputsAcountVerification.ResumeLayout(false);
            this.gbEmailInputsAcountVerification.PerformLayout();
            this.gbAccountVerificationLogger.ResumeLayout(false);
            this.gbAccountVerificationAction.ResumeLayout(false);
            this.gbAccountVerificationThreadSetting.ResumeLayout(false);
            this.gbAccountVerificationThreadSetting.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbEmailInputsAcountVerification;
        private System.Windows.Forms.Button btnEmailsBrowse_AccountVerification;
        private System.Windows.Forms.TextBox txtEmails_AccountVerification;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textNoofThread_AccountVerification;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Button btnAccountVerification_AccountVerification;
        private System.Windows.Forms.GroupBox gbAccountVerificationLogger;
        private System.Windows.Forms.ListBox LstBoxLogger_AccountVerification;
        private System.Windows.Forms.Button btnResendVerfication;
        private System.Windows.Forms.GroupBox gbAccountVerificationAction;
        private System.Windows.Forms.Button btnAcVerificationStop;
        private System.Windows.Forms.GroupBox gbAccountVerificationThreadSetting;
    }
}