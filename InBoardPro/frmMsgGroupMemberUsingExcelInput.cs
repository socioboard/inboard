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
using System.Text.RegularExpressions;
using BaseLib;

namespace InBoardPro
{
    public partial class frmMsgGroupMemberUsingExcelInput : Form
    {
        public frmMsgGroupMemberUsingExcelInput()
        {
            InitializeComponent();
        }

         #region global declaration
        List<string[]> excelData = new List<string[]>();
        public System.Drawing.Image image;
        List<string[]> listArrSftwareinputData = new List<string[]>();
        List<string> listSftwareinputData = new List<string>();
        List<Thread> lstScrapperStopThread = new List<Thread>();
        bool IsStopScrapper = false;
        bool CheckNetConn = false;
        InBoardPro.StartLinkedinScrapping objstartscrapp = new InBoardPro.StartLinkedinScrapping(); 
        #endregion

        private void frmMsgGroupMemberUsingExcelInput_Load(object sender, EventArgs e)
        {
            image = Properties.Resources.background;
            LoadPreLoadedAccount();
        }

        private void btnMsgGroupMemExlinput_Click(object sender, EventArgs e)
        {
            try
            {
                clsEmailParser objparser = new clsEmailParser();
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    excelData.Clear();
                    ofd.InitialDirectory = System.Windows.Forms.Application.StartupPath;
                    ofd.Filter = "Text Files (*.xlsx)|*.xlsx";

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtMsgGroupMemInput.Text = ofd.FileName;
                        excelData = objparser.parseExcel(txtMsgGroupMemInput.Text);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        #region LoadPreLoadedAccount
        private void LoadPreLoadedAccount()
        {
            try
            {
                if (LinkedInManager.linkedInDictionary.Count() == 0)
                {
                    MessageBox.Show("Please Load LinkedIn Accounts From MyAccounts Menu");
                    AddLoggerMsgGroupMemWithExcelInput("[ " + DateTime.Now + " ] => [ Please Load LinkedIn Accounts From MyAccounts Menu ]");

                    frmAccounts FrmAccount = new frmAccounts();
                    FrmAccount.Show();
                    return;
                }
                else
                {
                    try
                    {
                        PopulateCmo();

                    }
                    catch
                    {
                    }
                }
            }
            catch
            {
            }
        }
        #endregion

        #region AddLoggerMsgGroupMemWithExcelInput

        private void AddLoggerMsgGroupMemWithExcelInput(string log)
        {
            this.Invoke(new MethodInvoker(delegate
            {
                lstLoglinkdinScarper.Items.Add(log);
                lstLoglinkdinScarper.SelectedIndex = lstLoglinkdinScarper.Items.Count - 1;
            }));
        }

        #endregion

        #region PopulateCmo()
        private void PopulateCmo()
        {
            try
            {
                cmbAllUser.Items.Clear();
                foreach (KeyValuePair<string, LinkedInMaster> item in LinkedInManager.linkedInDictionary)
                {
                    cmbAllUser.Items.Add(item.Key);
                }
                AddLoggerMsgGroupMemWithExcelInput("[ " + DateTime.Now + " ] => [ Accounts Uploaded..]");
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> PopulateCmo() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> PopulateCmo() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPreScrapperErrorLogs);
            }
        }

        #endregion

        private void frmMsgGroupMemberUsingExcelInput_Paint(object sender, PaintEventArgs e)
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

        private void cmbAllUser_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnMsgGroupMessage_Click(object sender, EventArgs e)
        {

        }

        
    }
}
