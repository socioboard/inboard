using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace InBoardPro
{
    public partial class FrmFreeTrial : Form
    {
        #region FrmFreeTrial
        public FrmFreeTrial()
        {
            this.TopMost = true;
            InitializeComponent();
        } 
        #endregion

        #region pictureBoxBuyNow_Click
        private void pictureBoxBuyNow_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("iexplore", "http://www.linkeddominator.com/pricing.php");
            this.Close();
        } 
        #endregion

        
    }
}
