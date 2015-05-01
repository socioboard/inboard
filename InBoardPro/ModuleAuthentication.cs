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
    public partial class ModuleAuthentication : Form
    {
        string module;
        public ModuleAuthentication(string mod)
        {
            InitializeComponent();
            module = mod;
        }

          public System.Drawing.Image image;
          
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            Authentication();
        }

        private void Authentication()
        {
            /* CLIENT PASSWORD DETAIL */
            /* AMIT HEERA: sankash */

           try
            {
                if (module == "ExcelInput3" && txtPasswod.Text == "sankash")
                {
                    FrmInBoardProGetDataZipIndustryLastNameWise obj_FrmInBoardProGetDataZipIndustryLastNameWise = new FrmInBoardProGetDataZipIndustryLastNameWise();
                    obj_FrmInBoardProGetDataZipIndustryLastNameWise.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Wrong Passwod Please Try Again..");
                    txtPasswod.Text = string.Empty;
                    txtPasswod.Focus();
                }

            }
            catch { }
        }

        private void ModuleAuthentication_Load(object sender, EventArgs e)
        {
            image = Properties.Resources.background;
        }

        private void ModuleAuthentication_Paint(object sender, PaintEventArgs e)
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

        private void txtPasswod_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                Authentication();
            }
        }
    }
}
