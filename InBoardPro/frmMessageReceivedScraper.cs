using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Threading;
using BaseLib;

namespace LinkedinDominator
{
    public partial class frmMessageReceivedScraper : Form
    {
        public System.Drawing.Image image;
        bool CheckNetConn = false;
        bool IsStop = false;
        Dictionary<string, Dictionary<string, string>> LinkedInContacts = new Dictionary<string, Dictionary<string, string>>();
        List<Thread> lstRemoveMessageReceivedGroupThread = new List<Thread>();
       
        public frmMessageReceivedScraper()
        {
            InitializeComponent();
        }

        private void frmMessageReceivedScraper_Load(object sender, EventArgs e)
        {
            image = Properties.Resources.center_bg;
        }

        #region frmRemovePendingGroups_Paint
        private void frmRemovePendingGroups_Paint(object sender, PaintEventArgs e)
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

        #region AddLoggerMessageReceivedScraper

        private void AddLoggerMessageReceivedScraper(string log)
        {
            this.Invoke(new MethodInvoker(delegate
            {

                lstLogMessageReceivedScraper.Items.Add(log);
                lstLogMessageReceivedScraper.SelectedIndex = lstLogMessageReceivedScraper.Items.Count - 1;
            }));
        }

        #endregion

        #region btnStartMessageReceivedScraper_Click
        private void btnStartMessageReceivedScraper_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
            AddLoggerMessageReceivedScraper("working");
            if (CheckNetConn)
            {

                try
                {
                    IsStop = false;
                    lstRemoveMessageReceivedGroupThread.Clear();
                    btnStartMessageReceivedScraper.Cursor = Cursors.AppStarting;

                    Thread acceptInvitation_Thread = new Thread(StartMessageReceivedSraper);
                    acceptInvitation_Thread.Start();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error >>> " + ex.StackTrace);
                }
            }

        }
        #endregion btnStartMessageReceivedScraper_Click
    }
}
