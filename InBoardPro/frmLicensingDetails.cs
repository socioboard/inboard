using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using BaseLib;

namespace InBoardPro
{
    public partial class frmLicensingDetails : Form
    {
        public frmLicensingDetails()
        {
            InitializeComponent();
        }

        private void frmLicensingDetails_Load(object sender, EventArgs e)
        {
            string thisVersionNumber = GetAssemblyNumber();
            string activationdate = LinkedInManager.Licence_Details.Split(':')[1];
            string ValidationDate = string.Empty;
            //this.Text = this.Text + " Version No. : " + thisVersionNumber + "  License Provided To : (" + LinkedInManager.Licence_for + ") ";
            try
            {
                lblProductVer.Text = "Product Version: " + thisVersionNumber;
                lblLicensedTo.Text = LinkedInManager.Licence_Details.Split(':')[0];
                lblActivatioDate.Text = "Activation Date: " + activationdate.Split(' ')[0];

                //DateTime ActivationDate = Convert.ToDateTime(activationdate.Split(' ')[0]);
                //DateTime AfterOneYear = ActivationDate.AddYears(1);
                //ValidationDate = Convert.ToString(AfterOneYear);
                //ValidationDate = ValidationDate.Split(' ')[0].Replace("/","-");
                //ValidationDate = ValidationDate.Split('-')[2] + "-" + ValidationDate.Split('-')[0] + "-" + ValidationDate.Split('-')[1];
                //lblValidationDate.Text = "License valid till: " + ValidationDate;
            }
            catch { }

        }

        #region GetAssemblyNumber
        public string GetAssemblyNumber()
        {
            string appName = Assembly.GetAssembly(this.GetType()).Location;
            AssemblyName assemblyName = AssemblyName.GetAssemblyName(appName);
            string versionNumber = assemblyName.Version.ToString();
            return versionNumber;
        }
        #endregion
    }
}
