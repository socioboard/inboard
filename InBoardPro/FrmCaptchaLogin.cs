using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using InBoardPro;
using BaseLib.DB_Repository;
using System.Drawing.Drawing2D;
using BaseLib;

namespace InBoardPro
{
    public partial class FrmCaptchaLogin : Form
    {
        public System.Drawing.Image image;

        public FrmCaptchaLogin()
        {
            InitializeComponent();
        }

        #region btnSubmitCapchaLogin_Click
        public void btnSubmitCapchaLogin_Click(object sender, EventArgs e)
        {
            try
            {
                Globals.CapchaTag = true;
                Globals.CapchaLoginID = txtCapchaID.Text;
                Globals.CapchaLoginPassword = txtCapchaPaswrd.Text;

                clsSettingDB ObjclsSettingDB = new clsSettingDB();
                ObjclsSettingDB.InsertOrUpdateSetting("Captcha", "Captcha_ID", StringEncoderDecoder.Encode(txtCapchaID.Text));
                ObjclsSettingDB.InsertOrUpdateSetting("Captcha", "Captcha_Password", StringEncoderDecoder.Encode(txtCapchaPaswrd.Text));

            

                if (MessageBox.Show("Settings Saved !", "Notification", MessageBoxButtons.OK) == DialogResult.OK)
                {

                    this.Close();
                }
            }
            catch
            {
            }
        } 
        #endregion

        #region FrmCapchaLogin_Load
        private void FrmCapchaLogin_Load(object sender, EventArgs e)
        {
            try
            {
                Globals.CapchaTag = true;
                image = Properties.Resources.background;
                txtDBpath.Text = Globals.path_AppDataFolder;

                clsDBQueryManager DataBase = new clsDBQueryManager();
                DataTable dt = DataBase.SelectSettingData();
                foreach (DataRow dRow in dt.Rows)
                {

                    if ("Captcha_ID" == dRow[1].ToString())
                    {
                        txtCapchaID.Text = StringEncoderDecoder.Decode(dRow[2].ToString());
                    }
                    if ("Captcha_Password" == dRow[1].ToString())
                    {
                        txtCapchaPaswrd.Text = StringEncoderDecoder.Decode(dRow[2].ToString());
                    }

                    Globals.CapchaLoginID = txtCapchaID.Text;
                    Globals.CapchaLoginPassword = txtCapchaPaswrd.Text;
                }

                //string[] arr1 = new string[] { BaseLib.Globals.CapchaLoginID, BaseLib.Globals.CapchaLoginPassword};
                //DeathByCaptcha.Client clnt = null;
                //string capcthaResponse = DecodeDBC(arr1, out clnt);
            }
            catch
            {
            }
        } 
        #endregion

        public string DecodeDBC(string[] args, out DeathByCaptcha.Client client)
        {
            try
            {
                client = (DeathByCaptcha.Client)new DeathByCaptcha.SocketClient(args[0], args[1]);
                client.Verbose = true;

                Console.WriteLine("Your balance is {0:F2} US cents", client.Balance);//Log("Your balance is " + client.Balance + " US cents ");

                if (!client.User.LoggedIn)
                {
                   return null;
                }

                if (client.Balance == 0.0)
                {
                   return null;
                }
            }
            catch
            {
            }

            client = null;
            return null;
        }


        #region FrmCaptchaLogin_Paint
        private void FrmCaptchaLogin_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                Graphics g;
                g = e.Graphics;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.DrawImage(image, 0, 0, this.Width, this.Height);
            }
            catch
            {
            }
        } 
        #endregion

        #region LoadDecaptchaSettings
        public static void LoadDecaptchaSettings()
        {
            try
            {
                Globals.CapchaTag = true;

                clsDBQueryManager DataBase = new clsDBQueryManager();
                DataTable dt = DataBase.SelectSettingData();
                foreach (DataRow dRow in dt.Rows)
                {
                    if ("Captcha_ID" == dRow[1].ToString())
                    {
                        Globals.CapchaLoginID = StringEncoderDecoder.Decode(dRow[2].ToString());
                    }
                    if ("Captcha_Password" == dRow[1].ToString())
                    {
                        Globals.CapchaLoginPassword = StringEncoderDecoder.Decode(dRow[2].ToString());
                    }
                }

            }
            catch
            {
            }
        } 
        #endregion

               
    }
}
