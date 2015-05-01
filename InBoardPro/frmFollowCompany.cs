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
using InBoardPro;
using Others;
using BaseLib;

namespace InBoardPro
{
    public partial class frmFollowCompany : Form
    {
        public frmFollowCompany()
        {
            InitializeComponent();
        }

        #region Global declaration
        public System.Drawing.Image image;
        bool IsStop = false;
        bool CheckNetConn = false;
        List<Thread> lstLinkedinFollowCompanyUrlThraed = new List<Thread>();
        Dictionary<string, Dictionary<string, string>> LinkdInContacts = new Dictionary<string, Dictionary<string, string>>();
        public int counter_AddFollowUrl = 0;
        public int counter_JoinFollowCompanyUrlID = 0;
        #endregion

        #region frmFollowCompany_Load
        private void frmFollowCompany_Load(object sender, EventArgs e)
        {
            image = Properties.Resources.background;
            LinkedinCompanyFollow.logger.addToLogger += new EventHandler(logger_SearchFollowCompanyaddToLogger);
            //LinkedinCompanyFollow.logger.addToLogger += logger_SearchFollowCompanyaddToLogger;

        } 
        #endregion

        #region BtnUploadCompanyURL_Click
        private void BtnUploadCompanyURL_Click(object sender, EventArgs e)
        {
            try
            {
                txtCompanyURL.Text = "";
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "Text Files (*.txt)|*.txt";
                    ofd.InitialDirectory = Application.StartupPath;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtCompanyURL.Text = ofd.FileName;
                        List<string> templist = GlobusFileHelper.ReadFile(ofd.FileName);

                        LinkedinCompanyFollow.lstLinkedinCompanyURL.Clear();
                        foreach (string item in templist)
                        {
                            LinkedinCompanyFollow.lstLinkedinCompanyURL.Add(item);
                        }
                        LinkedinCompanyFollow.lstLinkedinCompanyURL = LinkedinCompanyFollow.lstLinkedinCompanyURL.Distinct().ToList();
                        AddLoggerFollowCompanyUrl("[ " + DateTime.Now + " ] => [ " + LinkedinCompanyFollow.lstLinkedinCompanyURL.Count + " Company URLs Loaded ! ]");
                    }
                }
            }
            catch { }
        } 
        #endregion

        #region logger_SearchFollowCompanyaddToLogger

        void logger_SearchFollowCompanyaddToLogger(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eventArgs = e as EventsArgs;
                AddLoggerFollowCompanyUrl(eventArgs.log);
            }
        }

        #endregion

        #region AddLoggerJoinGroupUrl

        private void AddLoggerFollowCompanyUrl(string log)
        {
            try
            {
                if (listLoggerFollowCompany.InvokeRequired)
                {
                    listLoggerFollowCompany.Invoke(new MethodInvoker(delegate
                    {
                        listLoggerFollowCompany.Items.Add(log);
                        listLoggerFollowCompany.SelectedIndex = listLoggerFollowCompany.Items.Count - 1;
                    }));
                }
                else
                {
                    listLoggerFollowCompany.Items.Add(log);
                    listLoggerFollowCompany.SelectedIndex = listLoggerFollowCompany.Items.Count - 1;
                }
            }
            catch (Exception ex)
            {
                
            
            }
        }

        #endregion

        #region frmFollowCompany_Paint
        private void frmFollowCompany_Paint(object sender, PaintEventArgs e)
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


        #region btnFollowCompany_Click

        private void btnFollowCompany_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {

                try
                {
                    lstLinkedinFollowCompanyUrlThraed.Clear();

                    if (IsStop)
                    {
                        IsStop = false;
                    }

                    if (string.IsNullOrEmpty(txtNumberOfFollowPerAccount.Text))
                    {
                        AddLoggerFollowCompanyUrl("[ " + DateTime.Now + " ] => [ Please enter No of Company Per Account ]");
                        txtNumberOfFollowPerAccount.Focus();
                        return;
                    }

                    if (!string.IsNullOrEmpty(txtNumberOfFollowPerAccount.Text))
                    {
                        if (NumberHelper.ValidateNumber(txtNumberOfFollowPerAccount.Text))
                        {

                        }
                        else
                        {
                            AddLoggerFollowCompanyUrl("[ " + DateTime.Now + " ] => [ Invalid entry..please enter only numeric value ]");
                            return;
                        }
                    }

                    if (LinkedInManager.linkedInDictionary.Count() == 0)
                    {
                        frmAccounts FrmAccount = new frmAccounts();
                        FrmAccount.Show();
                        return;
                    }

                    counter_AddFollowUrl = 0;

                    AddLoggerFollowCompanyUrl("[ " + DateTime.Now + " ] => [ PROCESS START for Manage URL]");

                    if (LinkedinCompanyFollow.lstLinkedinCompanyURL.Count > 0)
                    {
                        new Thread(() =>
                        {
                            LinkedInAddSearchCompany();

                        }).Start();
                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Follow Company --> btnFollowCompany_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Follow Company --> btnFollowCompany_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddSearchGroupErrorLogs);
                }
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddLoggerFollowCompanyUrl("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }

        }
        #endregion

        #region LinkedInAddSearchCompany
        int numberOfThreads = 0;
        private void LinkedInAddSearchCompany()
        {           
            counter_AddFollowUrl = LinkedInManager.linkedInDictionary.Count;

            try
            {
                numberOfThreads =  Convert.ToInt32(txtNoOfThread.Text.ToString());
                if (LinkedInManager.linkedInDictionary.Count() > 0)
                {
                    //ThreadPool.SetMaxThreads(numberOfThreads, numberOfThreads);

                    foreach (KeyValuePair<string, LinkedInMaster> item in LinkedInManager.linkedInDictionary)
                    {
                        if (LinkedinCompanyFollow.lstLinkedinCompanyURL.Count() > 0)
                        {
                            ThreadPool.SetMaxThreads(numberOfThreads, numberOfThreads);

                            ThreadPool.QueueUserWorkItem(new WaitCallback(PostAddcompanyUrl), new object[] { item });

                            Thread.Sleep(500);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Follow Company --> LinkedInAddSearchCompany() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Follow Company --> LinkedInAddSearchCompany() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddSearchGroupErrorLogs);
            }
            
        }

        #endregion

        #region PostAddcompanyUrl

        public void PostAddcompanyUrl(object parameter)
        {
            try
            {
                try
                {
                    if (IsStop)
                    {
                        return;
                    }

                    if (!IsStop)
                    {
                        lstLinkedinFollowCompanyUrlThraed.Add(Thread.CurrentThread);
                        lstLinkedinFollowCompanyUrlThraed.Distinct().ToList();
                        Thread.CurrentThread.IsBackground = true;
                    }
                }
                catch
                {
                }

                if (!string.IsNullOrEmpty(txtNumberOfFollowPerAccount.Text) && NumberHelper.ValidateNumber(txtNumberOfFollowPerAccount.Text))
                {
                    LinkedinCompanyFollow.CountPerAccount = Convert.ToInt32(txtNumberOfFollowPerAccount.Text);
                }

                Array paramsArray = new object[1];
                paramsArray = (Array)parameter;
                KeyValuePair<string, LinkedInMaster> item = (KeyValuePair<string, LinkedInMaster>)paramsArray.GetValue(0);
            
                LinkedinLogin Login = new LinkedinLogin();
                Login.accountUser = item.Key;
                Login.accountPass = item.Value._Password;
                Login.proxyAddress = item.Value._ProxyAddress;
                Login.proxyPort = item.Value._ProxyPort;
                Login.proxyUserName = item.Value._ProxyUsername;
                Login.proxyPassword = item.Value._ProxyPassword;

                GlobusHttpHelper HttpHelper = new GlobusHttpHelper();
                ChilkatHttpHelpr ChilkatHttpHelper = new ChilkatHttpHelpr();
                LinkedinCompanyFollow obj_FollowCompany = new LinkedinCompanyFollow(Login.accountUser, Login.accountPass, Login.proxyAddress, Login.proxyPort, Login.proxyUserName, Login.proxyPassword);

                Login.logger.addToLogger += new EventHandler(logger_SearchFollowCompanyaddToLogger);
                
                int minDelay = 20;
                int maxDelay = 25;

                if (!string.IsNullOrEmpty(txtFollowCompanyMinDelay.Text) && NumberHelper.ValidateNumber(txtFollowCompanyMinDelay.Text))
                {
                    minDelay = Convert.ToInt32(txtFollowCompanyMinDelay.Text);
                }
                if (!string.IsNullOrEmpty(txtFollowCompanyMaxDelay.Text) && NumberHelper.ValidateNumber(txtFollowCompanyMaxDelay.Text))
                {
                    maxDelay = Convert.ToInt32(txtFollowCompanyMaxDelay.Text);
                }


                if (!Login.IsLoggedIn)
                {
                   Login.LoginHttpHelper(ref HttpHelper);
                    //Login.LoginWithChilkatHelper(ref ChilkatHttpHelper);
                }

                if (Login.IsLoggedIn)
                {
                    LinkedinCompanyFollow LinkdinFollowComp = new LinkedinCompanyFollow();
                    //LinkdinFollowComp.logger.addToLogger += new EventHandler(logger_SearchFollowCompanyaddToLogger);

                    try
                    {
                        Dictionary<string, string> Result = new Dictionary<string, string>();
                        string MessagePosted = obj_FollowCompany.PostAddCompanyUsingUrl(ref HttpHelper, Login.accountUser,minDelay, maxDelay);
                    }
                    catch { }    
                }
           
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Manage Follow Company URL --> PostAddcompanyUrl() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Manage Follow Company URL --> PostAddcompanyUrl() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddSearchGroupErrorLogs);
            }
            finally
            {
                counter_AddFollowUrl--;

                if (counter_AddFollowUrl == 0)
                {
                    btnFollowCompany.Invoke(new MethodInvoker(delegate
                    {
                        AddLoggerFollowCompanyUrl("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                        AddLoggerFollowCompanyUrl("----------------------------------------------------------------------------------------------------------");
                        btnFollowCompany.Cursor = Cursors.Default;
                    }));
                }
            }
            
        }
        #endregion

                      
        #region btnStop_Click
        private void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                IsStop = true;
                List<Thread> lstTemp = lstLinkedinFollowCompanyUrlThraed.Distinct().ToList();
                foreach (Thread item in lstTemp)
                {
                    try
                    {
                        item.Abort();
                    }
                    catch
                    {
                    }
                }
                AddLoggerFollowCompanyUrl("------------------------------------------------------------------------------------------------------------------------------------");
                AddLoggerFollowCompanyUrl("[ " + DateTime.Now + " ] => [ PROCESS STOPPED ]");
                AddLoggerFollowCompanyUrl("------------------------------------------------------------------------------------------------------------------------------------");
                btnFollowCompany.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error >>> " + ex.StackTrace);
            }
        } 
        #endregion
               
    }
}
