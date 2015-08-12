namespace InBoardPro
{
    partial class ModuleAuthentication
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
            this.grpBoxModuleAuthentication = new System.Windows.Forms.GroupBox();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPasswod = new System.Windows.Forms.TextBox();
            this.lblModuleAuthNote = new System.Windows.Forms.Label();
            this.grpBoxModuleAuthentication.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpBoxModuleAuthentication
            // 
            this.grpBoxModuleAuthentication.BackColor = System.Drawing.Color.Transparent;
            this.grpBoxModuleAuthentication.Controls.Add(this.btnSubmit);
            this.grpBoxModuleAuthentication.Controls.Add(this.label2);
            this.grpBoxModuleAuthentication.Controls.Add(this.txtPasswod);
            this.grpBoxModuleAuthentication.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpBoxModuleAuthentication.Location = new System.Drawing.Point(12, 12);
            this.grpBoxModuleAuthentication.Name = "grpBoxModuleAuthentication";
            this.grpBoxModuleAuthentication.Size = new System.Drawing.Size(380, 115);
            this.grpBoxModuleAuthentication.TabIndex = 0;
            this.grpBoxModuleAuthentication.TabStop = false;
            this.grpBoxModuleAuthentication.Text = "Please Enter Password";
            // 
            // btnSubmit
            // 
            this.btnSubmit.BackgroundImage = global::InBoardPro.Properties.Resources.submit;
            this.btnSubmit.Location = new System.Drawing.Point(77, 69);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(79, 28);
            this.btnSubmit.TabIndex = 2;
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Passwod:";
            // 
            // txtPasswod
            // 
            this.txtPasswod.Location = new System.Drawing.Point(77, 43);
            this.txtPasswod.Name = "txtPasswod";
            this.txtPasswod.PasswordChar = '#';
            this.txtPasswod.Size = new System.Drawing.Size(225, 21);
            this.txtPasswod.TabIndex = 0;
            this.txtPasswod.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPasswod_KeyPress);
            // 
            // lblModuleAuthNote
            // 
            this.lblModuleAuthNote.AutoSize = true;
            this.lblModuleAuthNote.BackColor = System.Drawing.Color.Transparent;
            this.lblModuleAuthNote.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblModuleAuthNote.ForeColor = System.Drawing.Color.Blue;
            this.lblModuleAuthNote.Location = new System.Drawing.Point(12, 131);
            this.lblModuleAuthNote.Name = "lblModuleAuthNote";
            this.lblModuleAuthNote.Size = new System.Drawing.Size(380, 13);
            this.lblModuleAuthNote.TabIndex = 1;
            this.lblModuleAuthNote.Text = "Note: Please contact \"socioboard.support\" in skype for details.";
            // 
            // ModuleAuthentication
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(409, 163);
            this.Controls.Add(this.lblModuleAuthNote);
            this.Controls.Add(this.grpBoxModuleAuthentication);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "ModuleAuthentication";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Module Authentication";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.ModuleAuthentication_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.ModuleAuthentication_Paint);
            this.grpBoxModuleAuthentication.ResumeLayout(false);
            this.grpBoxModuleAuthentication.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpBoxModuleAuthentication;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPasswod;
        private System.Windows.Forms.Label lblModuleAuthNote;
        private System.Windows.Forms.Button btnSubmit;
    }
}