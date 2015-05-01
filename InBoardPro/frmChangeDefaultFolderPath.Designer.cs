namespace InBoardPro
{
    partial class frmChangeDefaultFolderPath
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmChangeDefaultFolderPath));
            this.txtExportLocation = new System.Windows.Forms.TextBox();
            this.chkChangeExportLocation = new System.Windows.Forms.CheckBox();
            this.btnSet = new System.Windows.Forms.Button();
            this.grpDefaultFolderPath = new System.Windows.Forms.GroupBox();
            this.grpDefaultFolderPath.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtExportLocation
            // 
            this.txtExportLocation.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtExportLocation.Location = new System.Drawing.Point(218, 39);
            this.txtExportLocation.Name = "txtExportLocation";
            this.txtExportLocation.ReadOnly = true;
            this.txtExportLocation.Size = new System.Drawing.Size(256, 21);
            this.txtExportLocation.TabIndex = 61;
            // 
            // chkChangeExportLocation
            // 
            this.chkChangeExportLocation.AutoSize = true;
            this.chkChangeExportLocation.BackColor = System.Drawing.Color.Transparent;
            this.chkChangeExportLocation.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkChangeExportLocation.Location = new System.Drawing.Point(50, 41);
            this.chkChangeExportLocation.Name = "chkChangeExportLocation";
            this.chkChangeExportLocation.Size = new System.Drawing.Size(162, 17);
            this.chkChangeExportLocation.TabIndex = 60;
            this.chkChangeExportLocation.Text = "Change Export Location";
            this.chkChangeExportLocation.UseVisualStyleBackColor = false;
            this.chkChangeExportLocation.CheckedChanged += new System.EventHandler(this.chkChangeExportLocation_CheckedChanged);
            // 
            // btnSet
            // 
            this.btnSet.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSet.Location = new System.Drawing.Point(218, 84);
            this.btnSet.Name = "btnSet";
            this.btnSet.Size = new System.Drawing.Size(75, 23);
            this.btnSet.TabIndex = 62;
            this.btnSet.Text = "Set";
            this.btnSet.UseVisualStyleBackColor = true;
            this.btnSet.Click += new System.EventHandler(this.btnSet_Click);
            // 
            // grpDefaultFolderPath
            // 
            this.grpDefaultFolderPath.BackColor = System.Drawing.Color.Transparent;
            this.grpDefaultFolderPath.Controls.Add(this.btnSet);
            this.grpDefaultFolderPath.Controls.Add(this.chkChangeExportLocation);
            this.grpDefaultFolderPath.Controls.Add(this.txtExportLocation);
            this.grpDefaultFolderPath.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpDefaultFolderPath.Location = new System.Drawing.Point(21, 12);
            this.grpDefaultFolderPath.Name = "grpDefaultFolderPath";
            this.grpDefaultFolderPath.Size = new System.Drawing.Size(507, 145);
            this.grpDefaultFolderPath.TabIndex = 63;
            this.grpDefaultFolderPath.TabStop = false;
            this.grpDefaultFolderPath.Text = "Default Folder Path";
            // 
            // frmChangeDefaultFolderPath
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(547, 169);
            this.Controls.Add(this.grpDefaultFolderPath);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmChangeDefaultFolderPath";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Change Default Folder Path";
            this.Load += new System.EventHandler(this.frmChangeDefaultFolderPath_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.frmChangeDefaultFolderPath_Paint);
            this.grpDefaultFolderPath.ResumeLayout(false);
            this.grpDefaultFolderPath.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtExportLocation;
        private System.Windows.Forms.CheckBox chkChangeExportLocation;
        private System.Windows.Forms.Button btnSet;
        private System.Windows.Forms.GroupBox grpDefaultFolderPath;
    }
}