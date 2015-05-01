namespace LinkedinDominator
{
    partial class HelpVedeo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HelpVedeo));
            this.gbHelpVideo = new System.Windows.Forms.GroupBox();
            this.axShockwaveFlash1 = new AxShockwaveFlashObjects.AxShockwaveFlash();
            this.gbHelpVideo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axShockwaveFlash1)).BeginInit();
            this.SuspendLayout();
            // 
            // gbHelpVideo
            // 
            this.gbHelpVideo.BackColor = System.Drawing.Color.Transparent;
            this.gbHelpVideo.Controls.Add(this.axShockwaveFlash1);
            this.gbHelpVideo.Location = new System.Drawing.Point(12, 12);
            this.gbHelpVideo.Name = "gbHelpVideo";
            this.gbHelpVideo.Size = new System.Drawing.Size(933, 499);
            this.gbHelpVideo.TabIndex = 0;
            this.gbHelpVideo.TabStop = false;
            this.gbHelpVideo.Text = "Help";
            // 
            // axShockwaveFlash1
            // 
            this.axShockwaveFlash1.Enabled = true;
            this.axShockwaveFlash1.Location = new System.Drawing.Point(21, 30);
            this.axShockwaveFlash1.Name = "axShockwaveFlash1";
            this.axShockwaveFlash1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axShockwaveFlash1.OcxState")));
            this.axShockwaveFlash1.Size = new System.Drawing.Size(884, 450);
            this.axShockwaveFlash1.TabIndex = 31;
            // 
            // HelpVedeo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(966, 538);
            this.Controls.Add(this.gbHelpVideo);
            this.MaximizeBox = false;
            this.Name = "HelpVedeo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Linkedin Dominator Video Help";
            this.Load += new System.EventHandler(this.HelpVedeo_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.HelpVedeo_Paint);
            this.gbHelpVideo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.axShockwaveFlash1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbHelpVideo;
        private AxShockwaveFlashObjects.AxShockwaveFlash axShockwaveFlash1;
    }
}