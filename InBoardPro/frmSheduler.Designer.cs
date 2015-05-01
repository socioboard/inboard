namespace InBoardPro
{
    partial class frmSheduler
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSheduler));
            this.gbTaskStatus = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.gbSheduledTask = new System.Windows.Forms.GroupBox();
            this.dgvScheduler = new System.Windows.Forms.DataGridView();
            this.btnstartScheduler = new System.Windows.Forms.Button();
            this.gbSubmitAction = new System.Windows.Forms.GroupBox();
            this.gbTaskStatus.SuspendLayout();
            this.gbSheduledTask.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvScheduler)).BeginInit();
            this.gbSubmitAction.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbTaskStatus
            // 
            this.gbTaskStatus.Controls.Add(this.label3);
            this.gbTaskStatus.Controls.Add(this.label2);
            this.gbTaskStatus.Controls.Add(this.label1);
            this.gbTaskStatus.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbTaskStatus.Location = new System.Drawing.Point(12, 12);
            this.gbTaskStatus.Name = "gbTaskStatus";
            this.gbTaskStatus.Size = new System.Drawing.Size(729, 72);
            this.gbTaskStatus.TabIndex = 0;
            this.gbTaskStatus.TabStop = false;
            this.gbTaskStatus.Text = "Task Status";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(559, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Pending Tasks";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(280, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(170, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Tasks Executed Successfully";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(63, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Total Tasks in Queue";
            // 
            // gbSheduledTask
            // 
            this.gbSheduledTask.Controls.Add(this.dgvScheduler);
            this.gbSheduledTask.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbSheduledTask.Location = new System.Drawing.Point(12, 100);
            this.gbSheduledTask.Name = "gbSheduledTask";
            this.gbSheduledTask.Size = new System.Drawing.Size(729, 240);
            this.gbSheduledTask.TabIndex = 1;
            this.gbSheduledTask.TabStop = false;
            this.gbSheduledTask.Text = "Scheduled Task";
            // 
            // dgvScheduler
            // 
            this.dgvScheduler.BackgroundColor = System.Drawing.Color.AliceBlue;
            this.dgvScheduler.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvScheduler.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvScheduler.Location = new System.Drawing.Point(3, 17);
            this.dgvScheduler.Name = "dgvScheduler";
            this.dgvScheduler.Size = new System.Drawing.Size(723, 220);
            this.dgvScheduler.TabIndex = 1;
            // 
            // btnstartScheduler
            // 
            this.btnstartScheduler.BackColor = System.Drawing.Color.Transparent;
            this.btnstartScheduler.BackgroundImage = global::InBoardPro.Properties.Resources.start;
            this.btnstartScheduler.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnstartScheduler.Location = new System.Drawing.Point(129, 13);
            this.btnstartScheduler.Name = "btnstartScheduler";
            this.btnstartScheduler.Size = new System.Drawing.Size(97, 30);
            this.btnstartScheduler.TabIndex = 5;
            this.btnstartScheduler.UseVisualStyleBackColor = false;
            this.btnstartScheduler.Click += new System.EventHandler(this.btnstartScheduler_Click);
            // 
            // gbSubmitAction
            // 
            this.gbSubmitAction.Controls.Add(this.btnstartScheduler);
            this.gbSubmitAction.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbSubmitAction.Location = new System.Drawing.Point(15, 347);
            this.gbSubmitAction.Name = "gbSubmitAction";
            this.gbSubmitAction.Size = new System.Drawing.Size(723, 52);
            this.gbSubmitAction.TabIndex = 6;
            this.gbSubmitAction.TabStop = false;
            this.gbSubmitAction.Text = "Submit Action";
            // 
            // frmSheduler
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(754, 411);
            this.Controls.Add(this.gbSubmitAction);
            this.Controls.Add(this.gbSheduledTask);
            this.Controls.Add(this.gbTaskStatus);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmSheduler";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Scheduler Task";
            this.Load += new System.EventHandler(this.frmSheduler_Load);
            this.gbTaskStatus.ResumeLayout(false);
            this.gbTaskStatus.PerformLayout();
            this.gbSheduledTask.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvScheduler)).EndInit();
            this.gbSubmitAction.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbTaskStatus;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox gbSheduledTask;
        private System.Windows.Forms.DataGridView dgvScheduler;
        private System.Windows.Forms.Button btnstartScheduler;
        private System.Windows.Forms.GroupBox gbSubmitAction;
    }
}