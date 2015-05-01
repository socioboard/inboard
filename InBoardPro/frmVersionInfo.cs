using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Reflection;

namespace InBoardPro
{
    public partial class frmVersionInfo : Form
    {
        public System.Drawing.Image image;

        public frmVersionInfo()
        {
            InitializeComponent();
        }

        #region frmVersionInfo_Load
        private void frmVersionInfo_Load(object sender, EventArgs e)
        {
            image = Properties.Resources.background;
            string thisVersionNumber = GetAssemblyNumber();
            this.Text = this.Text + " Version No. : " + thisVersionNumber;
            //
            //lblVerionInfo.Text = "1. Msg group member (New Feature) - mantain db of users for which one time message has been sent." + Environment.NewLine + Environment.NewLine +
            //                     "2. Linkedid scraper using Excel Input (New Feature) - based on Surname (Last Name), Industry, Distance and " + Environment.NewLine + Environment.NewLine + " Zip code wise." + Environment.NewLine + Environment.NewLine +
            //                     "3. Campaign Msg group member (New Feature) - multiple accounts can send message at a time For msg group memebers." + Environment.NewLine + Environment.NewLine +
            //                     "4. Msg group member (New Feature) - msg group member - with excel  can send message" + Environment.NewLine + Environment.NewLine +
            //                     "5. follow company using url -  its working with proper threads and url." + Environment.NewLine + Environment.NewLine +
            //                     "6. Compose message - not fetching friends  " + Environment.NewLine + Environment.NewLine +
            //                     "7. Company Scrapper - company scraper - not working properly." + Environment.NewLine + Environment.NewLine +
            //                     "8. Msg Group Member - accounts are showing restricted in software but its not." + Environment.NewLine + Environment.NewLine +
            //                     "9. Linkedin Scrapper - 'Title' section giving wrong output." + Environment.NewLine + Environment.NewLine +
            //                     "10. Follow Company - Follow company Using URL: logger issue its not showing detail of following company Url.";
            lblVerionInfo.Text = "1. Profile Ranking (New Feature) - Get your profile rank based on number of views" + Environment.NewLine + Environment.NewLine +
                                 "2. Get the number of profile views from different categories like - occupation, keywords, region etc." + Environment.NewLine + Environment.NewLine + 
                                 "3. Get the top 10 profile rank list of your connections" + Environment.NewLine + Environment.NewLine +
                                 "4. Msg Group Member - Get the members of all joined groups from a particular account" + Environment.NewLine + Environment.NewLine +
                                 "5. Msg Group Member - Send message to all 1st or 2nd or 3rd connections" + Environment.NewLine + Environment.NewLine +
                                 "6. Campaign Invite Using Search - Scheduler bug fixed" + Environment.NewLine + Environment.NewLine +
                                 "7. Linkedin Scraper - bug fixed" + Environment.NewLine + Environment.NewLine +
                                 "8. Other Scraper - Software scraps more members now" + Environment.NewLine + Environment.NewLine +
                                 "9. Join Friends Group - bug fixed ";
                                 
        } 
        #endregion

        #region frmVersionInfo_Paint
        private void frmVersionInfo_Paint(object sender, PaintEventArgs e)
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

        #region GetAssemblyNumber
        public string GetAssemblyNumber()
        {
            string appName = Assembly.GetAssembly(this.GetType()).Location;
            AssemblyName assemblyName = AssemblyName.GetAssemblyName(appName);
            string versionNumber = assemblyName.Version.ToString();
            return versionNumber;
        } 
        #endregion

        private void grpBoxVersionDetails_Enter(object sender, EventArgs e)
        {

        }
 
    }
}
