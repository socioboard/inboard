using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace InBoardPro
{
    public partial class frmHelp : Form
    {
        public System.Drawing.Image image;

        #region frmHelp
        public frmHelp()
        {
            InitializeComponent();
        } 
        #endregion

        #region frmHelp_Load
        private void frmHelp_Load(object sender, EventArgs e)
        {
            image = Properties.Resources.background;
        } 
        #endregion

        #region frmHelp_Paint
        private void frmHelp_Paint(object sender, PaintEventArgs e)
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

        #region LinkButAllFeature_LinkClicked
        private void LinkButAllFeature_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                this.LinkButAllFeature.LinkVisited = true;
                // Navigate to a URL.
                //System.Diagnostics.Process.Start("http://www.youtube.com/watch?v=lqsUxBlteB8");
                System.Diagnostics.Process.Start("http://www.youtube.com/v/lqsUxBlteB8");
            }
            catch { }
        } 
        #endregion

        #region LinkButAccountCreator_LinkClicked
        private void LinkButAccountCreator_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                this.LinkButAccountCreator.LinkVisited = true;
                // Navigate to a URL.
                //System.Diagnostics.Process.Start("http://www.youtube.com/watch?v=tWATg7j-uJM");
                System.Diagnostics.Process.Start("http://www.youtube.com/v/tWATg7j-uJM");
            }
            catch { }
        } 
        #endregion

        #region LinkButLinkdinScraper_LinkClicked
        private void LinkButLinkdinScraper_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                this.LinkButLinkdinScraper.LinkVisited = true;
                // Navigate to a URL.
                //System.Diagnostics.Process.Start("http://www.youtube.com/watch?v=PrthPB1Q84o");
                System.Diagnostics.Process.Start("http://www.youtube.com/v/PrthPB1Q84o");
            }
            catch { }
        } 
        #endregion

        #region LinkButStatusUpdate_LinkClicked
        private void LinkButStatusUpdate_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                this.LinkButStatusUpdate.LinkVisited = true;
                // Navigate to a URL.
                //System.Diagnostics.Process.Start("http://www.youtube.com/watch?v=3oxgdZXwadk");
                System.Diagnostics.Process.Start("http://www.youtube.com/v/3oxgdZXwadk");
            }
            catch { }
        } 
        #endregion

        #region LinkButAddConEmail_LinkClicked
        private void LinkButAddConEmail_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                this.LinkButAddConEmail.LinkVisited = true;
                // Navigate to a URL.
                //System.Diagnostics.Process.Start("http://www.youtube.com/watch?v=qG8VVuC7ZZY");
                System.Diagnostics.Process.Start("http://www.youtube.com/v/qG8VVuC7ZZY");
            }
            catch { }
        } 
        #endregion

        #region LinkButAddConKeyword_LinkClicked
        private void LinkButAddConKeyword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                this.LinkButAddConKeyword.LinkVisited = true;
                // Navigate to a URL.
                //System.Diagnostics.Process.Start("http://www.youtube.com/watch?v=BhpGgUyXCYI");
                System.Diagnostics.Process.Start("http://www.youtube.com/v/BhpGgUyXCYI");
            }
            catch { }
        } 
        #endregion

        #region LinkButCreatGroup_LinkClicked
        private void LinkButCreatGroup_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                this.LinkButCreatGroup.LinkVisited = true;
                // Navigate to a URL.
                //System.Diagnostics.Process.Start("http://www.youtube.com/watch?v=lKPuo-rooxw");
                System.Diagnostics.Process.Start("http://www.youtube.com/v/lKPuo-rooxw");
            }
            catch { }
        } 
        #endregion

        #region LinkButJoinFriendsGroup_LinkClicked
        private void LinkButJoinFriendsGroup_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                this.LinkButJoinFriendsGroup.LinkVisited = true;
                //// Navigate to a URL.
                //System.Diagnostics.Process.Start("http://www.youtube.com/watch?v=X-iG9RFKW8M");
                System.Diagnostics.Process.Start("http://www.youtube.com/v/X-iG9RFKW8M");
            }
            catch { }
        } 
        #endregion

        #region LinkButJoinSearchGroup_LinkClicked
        private void LinkButJoinSearchGroup_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                this.LinkButJoinSearchGroup.LinkVisited = true;
                // Navigate to a URL.
                //System.Diagnostics.Process.Start("http://www.youtube.com/watch?v=6rivLU_21xw");
                System.Diagnostics.Process.Start("http://www.youtube.com/v/6rivLU_21xw");
            }
            catch { }
        } 
        #endregion

        #region LinkButGroupUpdate_LinkClicked
        private void LinkButGroupUpdate_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                this.LinkButGroupUpdate.LinkVisited = true;
                // Navigate to a URL.
                //System.Diagnostics.Process.Start(" http://www.youtube.com/watch?v=Rx-C9SGPMGw");
                System.Diagnostics.Process.Start(" http://www.youtube.com/v/Rx-C9SGPMGw");
            }
            catch { }
        } 
        #endregion

        #region LinkButComposeMsg_LinkClicked
        private void LinkButComposeMsg_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                this.LinkButComposeMsg.LinkVisited = true;
                // Navigate to a URL.
                //System.Diagnostics.Process.Start(" http://www.youtube.com/watch?v=Y3MmpJg31lQ");
                System.Diagnostics.Process.Start(" http://www.youtube.com/v/Y3MmpJg31lQ");
            }
            catch { }
        } 
        #endregion

        #region LinkButMsgGroupMem_LinkClicked
        private void LinkButMsgGroupMem_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                this.LinkButMsgGroupMem.LinkVisited = true;
                // Navigate to a URL.
                //System.Diagnostics.Process.Start("http://www.youtube.com/watch?v=WRIr2ii5f_4");
                System.Diagnostics.Process.Start("http://www.youtube.com/v/WRIr2ii5f_4");
            }
            catch { }
        } 
        #endregion
              
    }
}
