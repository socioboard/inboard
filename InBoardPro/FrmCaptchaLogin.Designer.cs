namespace InBoardPro
{
    partial class FrmCaptchaLogin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmCaptchaLogin));
            this.gbDBCLogin = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.CapchaLoginPassword = new System.Windows.Forms.Label();
            this.lblCaptchaID = new System.Windows.Forms.Label();
            this.txtCapchaPaswrd = new System.Windows.Forms.TextBox();
            this.txtCapchaID = new System.Windows.Forms.TextBox();
            this.btnSubmitCapchaLogin = new System.Windows.Forms.Button();
            this.gbPath = new System.Windows.Forms.GroupBox();
            this.txtDBpath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.gbDBCLogin.SuspendLayout();
            this.gbPath.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbDBCLogin
            // 
            this.gbDBCLogin.BackColor = System.Drawing.Color.Transparent;
            this.gbDBCLogin.Controls.Add(this.label1);
            this.gbDBCLogin.Controls.Add(this.CapchaLoginPassword);
            this.gbDBCLogin.Controls.Add(this.lblCaptchaID);
            this.gbDBCLogin.Controls.Add(this.txtCapchaPaswrd);
            this.gbDBCLogin.Controls.Add(this.txtCapchaID);
            this.gbDBCLogin.Controls.Add(this.btnSubmitCapchaLogin);
            this.gbDBCLogin.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbDBCLogin.Location = new System.Drawing.Point(12, 20);
            this.gbDBCLogin.Name = "gbDBCLogin";
            this.gbDBCLogin.Size = new System.Drawing.Size(420, 110);
            this.gbDBCLogin.TabIndex = 176;
            this.gbDBCLogin.TabStop = false;
            this.gbDBCLogin.Text = "Death By Captcha Login";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(276, 80);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "label1";
            this.label1.Visible = false;
            // 
            // CapchaLoginPassword
            // 
            this.CapchaLoginPassword.AutoSize = true;
            this.CapchaLoginPassword.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CapchaLoginPassword.Location = new System.Drawing.Point(51, 49);
            this.CapchaLoginPassword.Name = "CapchaLoginPassword";
            this.CapchaLoginPassword.Size = new System.Drawing.Size(61, 13);
            this.CapchaLoginPassword.TabIndex = 9;
            this.CapchaLoginPassword.Text = "Password";
            // 
            // lblCaptchaID
            // 
            this.lblCaptchaID.AutoSize = true;
            this.lblCaptchaID.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCaptchaID.Location = new System.Drawing.Point(23, 22);
            this.lblCaptchaID.Name = "lblCaptchaID";
            this.lblCaptchaID.Size = new System.Drawing.Size(98, 13);
            this.lblCaptchaID.TabIndex = 8;
            this.lblCaptchaID.Text = "CaptchaLoginID";
            // 
            // txtCapchaPaswrd
            // 
            this.txtCapchaPaswrd.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCapchaPaswrd.Location = new System.Drawing.Point(137, 45);
            this.txtCapchaPaswrd.Name = "txtCapchaPaswrd";
            this.txtCapchaPaswrd.PasswordChar = '*';
            this.txtCapchaPaswrd.Size = new System.Drawing.Size(180, 21);
            this.txtCapchaPaswrd.TabIndex = 7;
            // 
            // txtCapchaID
            // 
            this.txtCapchaID.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCapchaID.Location = new System.Drawing.Point(137, 18);
            this.txtCapchaID.Name = "txtCapchaID";
            this.txtCapchaID.Size = new System.Drawing.Size(180, 21);
            this.txtCapchaID.TabIndex = 6;
            // 
            // btnSubmitCapchaLogin
            // 
            this.btnSubmitCapchaLogin.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSubmitCapchaLogin.Location = new System.Drawing.Point(137, 71);
            this.btnSubmitCapchaLogin.Name = "btnSubmitCapchaLogin";
            this.btnSubmitCapchaLogin.Size = new System.Drawing.Size(75, 23);
            this.btnSubmitCapchaLogin.TabIndex = 5;
            this.btnSubmitCapchaLogin.Text = "Submit";
            this.btnSubmitCapchaLogin.UseVisualStyleBackColor = true;
            this.btnSubmitCapchaLogin.Click += new System.EventHandler(this.btnSubmitCapchaLogin_Click);
            // 
            // gbPath
            // 
            this.gbPath.BackColor = System.Drawing.Color.Transparent;
            this.gbPath.Controls.Add(this.txtDBpath);
            this.gbPath.Controls.Add(this.label2);
            this.gbPath.Location = new System.Drawing.Point(12, 136);
            this.gbPath.Name = "gbPath";
            this.gbPath.Size = new System.Drawing.Size(420, 100);
            this.gbPath.TabIndex = 177;
            this.gbPath.TabStop = false;
            this.gbPath.Text = "DB Location";
            // 
            // txtDBpath
            // 
            this.txtDBpath.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDBpath.Location = new System.Drawing.Point(45, 53);
            this.txtDBpath.Name = "txtDBpath";
            this.txtDBpath.Size = new System.Drawing.Size(368, 21);
            this.txtDBpath.TabIndex = 14;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(42, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(247, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Your LinkedDominator DB is located here ";
            // 
            // FrmCaptchaLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(458, 251);
            this.Controls.Add(this.gbPath);
            this.Controls.Add(this.gbDBCLogin);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FrmCaptchaLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DeathByCaptcha Login";
            this.Load += new System.EventHandler(this.FrmCapchaLogin_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FrmCaptchaLogin_Paint);
            this.gbDBCLogin.ResumeLayout(false);
            this.gbDBCLogin.PerformLayout();
            this.gbPath.ResumeLayout(false);
            this.gbPath.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbDBCLogin;
        private System.Windows.Forms.Label CapchaLoginPassword;
        private System.Windows.Forms.Label lblCaptchaID;
        private System.Windows.Forms.TextBox txtCapchaPaswrd;
        private System.Windows.Forms.TextBox txtCapchaID;
        private System.Windows.Forms.Button btnSubmitCapchaLogin;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox gbPath;
        private System.Windows.Forms.TextBox txtDBpath;
        private System.Windows.Forms.Label label2;


    }
}

