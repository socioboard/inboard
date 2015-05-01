using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using SelectionMethod;

namespace LinkedinDominator
{
    public partial class HelpVedeo : Form
    {
        public System.Drawing.Image image;

        public HelpVedeo()
        {
            InitializeComponent();
        }

        #region HelpVedeo_Load
        private void HelpVedeo_Load(object sender, EventArgs e)
        {
            image = Properties.Resources.center_bg;

            if (ClsSelect.getVed == "LDAccountCreator")
            {
                try
                {
                    gbHelpVideo.Text = "Account Creator";
                    axShockwaveFlash1.Movie = "http://www.youtube.com/v/tWATg7j-uJM";
                }
                catch { }
            }
            else if (ClsSelect.getVed == "LDScrapper")
            {
                try
                {
                    gbHelpVideo.Text = "Linkedin Scrapper";
                    axShockwaveFlash1.Movie = "http://www.youtube.com/v/PrthPB1Q84o";
                }
                catch { }
            }
            else if (ClsSelect.getVed == "LDStatusUpdate")
            {
                try
                {
                    gbHelpVideo.Text = "Linkedin Status Update";
                    axShockwaveFlash1.Movie = "http://www.youtube.com/v/3oxgdZXwadk";
                }
                catch { }
            }
            else if (ClsSelect.getVed == "LDAddConnectionEmail")
            {
                try
                {
                    gbHelpVideo.Text = "Linkedin Add Connection with Email";
                    axShockwaveFlash1.Movie = "http://www.youtube.com/v/qG8VVuC7ZZY";
                }
                catch { }
            }
            else if (ClsSelect.getVed == "LDAddConnectionKeyword")
            {
                try
                {
                    gbHelpVideo.Text = "Linkedin Add Connection with Keyword";
                    axShockwaveFlash1.Movie = "http://www.youtube.com/v/BhpGgUyXCYI";
                }
                catch { }
            }
            else if (ClsSelect.getVed == "LDCreateGroup")
            {
                try
                {
                    gbHelpVideo.Text = "Linkedin Create Group";
                    axShockwaveFlash1.Movie = "http://www.youtube.com/v/lKPuo-rooxw";
                }
                catch { }
            }
            else if (ClsSelect.getVed == "LDJoinFriendsGroup")
            {
                try
                {
                    gbHelpVideo.Text = "Linkedin Join Friends Group";
                    axShockwaveFlash1.Movie = "http://www.youtube.com/v/X-iG9RFKW8M";
                }
                catch { }
            }
            else if (ClsSelect.getVed == "LDJoinSearchGroup")
            {
                try
                {
                    gbHelpVideo.Text = "Linkedin Join Search Group";
                    axShockwaveFlash1.Movie = "http://www.youtube.com/v/6rivLU_21xw";
                }
                catch { }
            }
            else if (ClsSelect.getVed == "LDGroupStatusUpdate")
            {
                try
                {
                    gbHelpVideo.Text = "Linkedin Join Group Status Update";
                    axShockwaveFlash1.Movie = "http://www.youtube.com/v/Rx-C9SGPMGw";
                }
                catch { }
            }
            else if (ClsSelect.getVed == "LDComposeMsg")
            {
                try
                {
                    gbHelpVideo.Text = "Linkedin Compose Message";
                    axShockwaveFlash1.Movie = "http://www.youtube.com/v/Y3MmpJg31lQ";
                }
                catch { }
            }
            else if (ClsSelect.getVed == "LDMsgGroupMem")
            {
                try
                {
                    gbHelpVideo.Text = "Linkedin Message Group Member";
                    axShockwaveFlash1.Movie = "http://www.youtube.com/v/WRIr2ii5f_4";
                }
                catch { }
            }
        } 
        #endregion

        #region HelpVedeo_Paint
        private void HelpVedeo_Paint(object sender, PaintEventArgs e)
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
    }
}
