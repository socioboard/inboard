using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using InBoardPro;
using System.Drawing.Drawing2D;
using System.IO;
using BaseLib;

namespace InBoardPro
{
    public partial class frmChangeDefaultFolderPath : Form
    {
        public System.Drawing.Image image;

        #region frmChangeDefaultFolderPath
        public frmChangeDefaultFolderPath()
        {
            InitializeComponent();
        } 
        #endregion

        #region chkChangeExportLocation_CheckedChanged
        private void chkChangeExportLocation_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkChangeExportLocation.Checked)
                {
                    using (FolderBrowserDialog ofd = new FolderBrowserDialog())
                    {
                        //ofd.Filter = "CSV Files (*.csv)|*.csv";
                        //ofd.InitialDirectory = Application.StartupPath;
                        if (ofd.ShowDialog() == DialogResult.OK)
                        {
                            txtExportLocation.Text = ofd.SelectedPath;

                        }
                    }
                }
                else
                {
                    txtExportLocation.Text = "C:\\Users\\user\\Desktop\\InBoardPro";
                }

            }
            catch
            {
            }
        } 
        #endregion

        #region btnSet_Click
        private void btnSet_Click(object sender, EventArgs e)
        {
            try
            {
                Globals.DesktopFolder = txtExportLocation.Text;
                string path = string.Empty;
                path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\InBoardPro\\LDDefaultFolderPath.txt";
                using (StreamWriter sw = new StreamWriter(path))
                {
                    sw.Write("");
                }

                GlobusFileHelper.AppendStringToTextfileNewLine(txtExportLocation.Text, Globals.Path_LinkedinDefaultSave);

                List<string> defaultpath = new List<string>();

                if (MessageBox.Show("Export Location Is >>>" + Globals.DesktopFolder, "Notification", MessageBoxButtons.OK) == DialogResult.OK)
                {
                    defaultpath = GlobusFileHelper.ReadFiletoStringList(Globals.Path_LinkedinDefaultSave);
                    txtExportLocation.Text = defaultpath[0];

                    this.Close();
                }
            }
            catch
            {
            }
        } 
        #endregion

        #region frmChangeDefaultFolderPath_Paint
        private void frmChangeDefaultFolderPath_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                Graphics g;

                g = e.Graphics;

                g.SmoothingMode = SmoothingMode.HighQuality;

                g.DrawImage(image, 0, 0, this.Width, this.Height);
            }
            catch (Exception ex)
            {

            }
        } 
        #endregion

        #region frmChangeDefaultFolderPath_Load
        private void frmChangeDefaultFolderPath_Load(object sender, EventArgs e)
        {
            image = Properties.Resources.background;

            List<string> defaultpath = new List<string>();

            try
            {
                defaultpath = GlobusFileHelper.ReadFiletoStringList(Globals.Path_LinkedinDefaultSave);
                txtExportLocation.Text = defaultpath[0];
            }
            catch { }


        } 
        #endregion
                
    }
}
