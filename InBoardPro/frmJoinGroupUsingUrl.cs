using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using InBoardPro;
using System.Threading;
using Groups;
using BaseLib;

namespace InBoardPro
{
    public partial class frmJoinGroupUsingUrl : Form
    {
        #region frmJoinGroupUsingUrl
        public frmJoinGroupUsingUrl()
        {
            InitializeComponent();
        } 
        #endregion

        #region global declaration
        public System.Drawing.Image image;
        bool IsStop = false;
        bool CheckNetConn = false;
        bool IsDevideData = false;
        List<Thread> lstLinkedinJoinGroupUrlThraed = new List<Thread>();
        Dictionary<string, Dictionary<string, string>> LinkdInContacts = new Dictionary<string, Dictionary<string, string>>();
        List<string> lstJoinGroupUrl = new List<string>();
        #endregion

        #region frmJoinGroupUsingUrl_Load
        private void frmJoinGroupUsingUrl_Load(object sender, EventArgs e)
        {
            image = Properties.Resources.background;
            Groups.JoinSearchGroup.loggerGroupusingUrl.addToLogger += new EventHandler(logger_SearchGroupaddToLogger);
        } 
        #endregion

        #region frmJoinGroupUsingUrl_Paint
        private void frmJoinGroupUsingUrl_Paint(object sender, PaintEventArgs e)
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

        #region BtnUploadGroupURL_Click
        private void BtnUploadGroupURL_Click(object sender, EventArgs e)
        {
            try
            {
                TxtGroupURL.Text = "";
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "Text Files (*.txt)|*.txt";
                    ofd.InitialDirectory = Application.StartupPath;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        TxtGroupURL.Text = ofd.FileName;
                        List<string> templist = GlobusFileHelper.ReadFile(ofd.FileName);

                        Groups.JoinSearchGroup.lstLinkedinGroupURL.Clear();
                        lstJoinGroupUrl.Clear();
                        foreach (string item in templist)
                        {
                            Groups.JoinSearchGroup.lstLinkedinGroupURL.Add(item);
                            lstJoinGroupUrl.Add(item);
                        }
                        Groups.JoinSearchGroup.lstLinkedinGroupURL = Groups.JoinSearchGroup.lstLinkedinGroupURL.Distinct().ToList();
                        AddLoggerJoinGroupUrl("[ " + DateTime.Now + " ] => [ " + Groups.JoinSearchGroup.lstLinkedinGroupURL.Count + " Group URLs Loaded ! ]");
                    }
                }
            }
            catch
            {
            }
        } 
        #endregion

        #region logger_SearchGroupaddToLogger

        void logger_SearchGroupaddToLogger(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eventArgs = e as EventsArgs;
                AddLoggerJoinGroupUrl(eventArgs.log);
            }
        }

        #endregion
        
        #region AddLoggerJoinGroupUrl

        private void AddLoggerJoinGroupUrl(string log)
        {
            this.Invoke(new MethodInvoker(delegate
            {
                listLoggerGroupUrl.Items.Add(log);
                listLoggerGroupUrl.SelectedIndex = listLoggerGroupUrl.Items.Count - 1;
            }));
        }

        #endregion

        #region btnStop_Click
        private void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                IsStop = true;
                List<Thread> lstTemp = lstLinkedinJoinGroupUrlThraed.Distinct().ToList();
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
                AddLoggerJoinGroupUrl("------------------------------------------------------------------------------------------------------------------------------------");
                AddLoggerJoinGroupUrl("[ " + DateTime.Now + " ] => [ PROCESS STOPPED ]");
                AddLoggerJoinGroupUrl("------------------------------------------------------------------------------------------------------------------------------------");
                btnJoinSearchGroup.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error >>> " + ex.StackTrace);
            }
        } 
        #endregion

        #region btnJoinSearchGroup_Click
        public int counter_JoinGroupUrlID = 0;
        private void btnJoinSearchGroup_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {
                try
                {
                    lstLinkedinJoinGroupUrlThraed.Clear();

                    if (IsStop)
                    {
                        IsStop = false;
                    }

                    if (LinkedInManager.linkedInDictionary.Count() == 0)
                    {
                        frmAccounts FrmAccount = new frmAccounts();
                        FrmAccount.Show();
                        return;
                    }
                    else if (TxtGroupURL.Text == string.Empty)
                    {
                        AddLoggerJoinGroupUrl("[ " + DateTime.Now + " ] => [ Please Add Group Urls ]");
                        MessageBox.Show("Please Add Group Urls");
                        TxtGroupURL.Focus();
                        return;
                    }
                    else if (!NumberHelper.ValidateNumber(txtNumberOfGroupsPerAccount.Text))
                    {
                        AddLoggerJoinGroupUrl("[ " + DateTime.Now + " ] => [ Please Enter Valid Number, in no of group per Account option ]");
                        MessageBox.Show("Please Enter Valid Number, in no of group per Account option");
                        txtNumberOfGroupsPerAccount.Focus();
                        return;
                    }
                    else if (!NumberHelper.ValidateNumber(txtSearchGroupMinDelay.Text))
                    {
                        AddLoggerJoinGroupUrl("[ " + DateTime.Now + " ] => [ Please Enter Valid Value in Minimum Delay ]");
                        MessageBox.Show("Please Enter Valid Value in Minimum Delay");
                        txtSearchGroupMinDelay.Focus();
                        return;
                    }
                    else if (!NumberHelper.ValidateNumber(txtSearchGroupMaxDelay.Text))
                    {
                        AddLoggerJoinGroupUrl("[ " + DateTime.Now + " ] => [ Please Enter Valid Value in Minimum Delay ]");
                        MessageBox.Show("Please Enter Valid Value in Minimum Delay");
                        txtSearchGroupMaxDelay.Focus();
                        return;
                    }

                    counter_JoinGroupUrlID = 0;
                    btnJoinSearchGroup.Cursor = Cursors.AppStarting;
                    AddLoggerJoinGroupUrl("[ " + DateTime.Now + " ] => [ PROCESS START ]");

                    if (Groups.JoinSearchGroup.lstLinkedinGroupURL.Count > 0)
                    {
                        new Thread(() =>
                        {
                            LinkedInAddSearchGroups();

                        }).Start();
                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Search Groups --> btnJoinSearchGroup_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Search Groups --> btnJoinSearchGroup_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddSearchGroupErrorLogs);
                }
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddLoggerJoinGroupUrl("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        } 
        #endregion

        #region LinkedInAddSearchGroups

        private void LinkedInAddSearchGroups()
        {
            int numberOfThreads = 0;
            counter_JoinGroupUrlID = LinkedInManager.linkedInDictionary.Count;
            numberOfThreads = Convert.ToInt16(txtNoOfThread.Text.ToString());

            #region User Divude Checked
            List<List<string>> list_listTargetUrls = new List<List<string>>();
            int index = 0;
            if (chkJoinGroupUsingUrlUseDivide.Checked)
            {
                IsDevideData = true;
                int splitNo = 0;
                if (rdBtnJoinGroupUsingUrlDivideEqually.Checked)
                {
                    splitNo = Groups.JoinSearchGroup.lstLinkedinGroupURL.Count / LinkedInManager.linkedInDictionary.Count;
                }
                else if (rdBtnJoinGroupUsingUrlDivideByGivenNo.Checked)
                {
                    if (!string.IsNullOrEmpty(txtJoinGroupUsingUrlNoOfUsers.Text) && NumberHelper.ValidateNumber(txtJoinGroupUsingUrlNoOfUsers.Text))
                    {
                        int res = Convert.ToInt32(txtJoinGroupUsingUrlNoOfUsers.Text);
                        splitNo = res;//listUserIDs.Count / res;
                    }
                }
                if (splitNo == 0)
                {
                    splitNo = RandomNumberGenerator.GenerateRandom(0, Groups.JoinSearchGroup.lstLinkedinGroupURL.Count - 1);
                }
                list_listTargetUrls = Split(Groups.JoinSearchGroup.lstLinkedinGroupURL, splitNo);
                //TweetAccountManager.noOFFollows = splitNo;
            }
            #endregion


            try
            {
                if (LinkedInManager.linkedInDictionary.Count() > 0)
                {
                    ThreadPool.SetMaxThreads(numberOfThreads, 5);

                    foreach (KeyValuePair<string, LinkedInMaster> item in LinkedInManager.linkedInDictionary)
                    {
                        if (Groups.JoinSearchGroup.lstLinkedinGroupURL.Count() > 0)
                        {
                            ThreadPool.SetMaxThreads(numberOfThreads, 5);
                            if (chkJoinGroupUsingUrlUseDivide.Checked)
                            {
                                lstJoinGroupUrl = list_listTargetUrls[index];
                            }
                            ThreadPool.QueueUserWorkItem(new WaitCallback(PostAddGroupUrl), new object[] { item,lstJoinGroupUrl });
                            index++;
                            Thread.Sleep(1000);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Search Groups --> LinkedInAddSearchGroups() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Search Groups --> LinkedInAddSearchGroups() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddSearchGroupErrorLogs);
            }
        }

        #endregion

        #region PostAddGroupUrl

        public void PostAddGroupUrl(object parameter)
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
                        lstLinkedinJoinGroupUrlThraed.Add(Thread.CurrentThread);
                        lstLinkedinJoinGroupUrlThraed.Distinct().ToList();
                        Thread.CurrentThread.IsBackground = true;
                    }
                }
                catch { }

                if (!string.IsNullOrEmpty(txtNumberOfGroupsPerAccount.Text) && NumberHelper.ValidateNumber(txtNumberOfGroupsPerAccount.Text))
                {
                    Groups.JoinSearchGroup.CountPerAccount = Convert.ToInt32(txtNumberOfGroupsPerAccount.Text);
                }

                Array paramsArray = new object[1];
                paramsArray = (Array)parameter;
                KeyValuePair<string, LinkedInMaster> item = (KeyValuePair<string, LinkedInMaster>)paramsArray.GetValue(0);
                List<string> lstJoinGroupUrl = (List<string>)paramsArray.GetValue(1);

                GlobusHttpHelper HttpHelper = new GlobusHttpHelper();
                LinkedinLogin Login = new LinkedinLogin();
                Login.accountUser = item.Key;
                Login.accountPass = item.Value._Password;
                Login.proxyAddress = item.Value._ProxyAddress;
                Login.proxyPort = item.Value._ProxyPort;
                Login.proxyUserName = item.Value._ProxyUsername;
                Login.proxyPassword = item.Value._ProxyPassword;

                Groups.JoinSearchGroup obj_JoinSearchGroup = new Groups.JoinSearchGroup(Login.accountUser, Login.accountPass, Login.proxyAddress, Login.proxyPort, Login.proxyUserName, Login.proxyPassword);
                Login.logger.addToLogger += new EventHandler(logger_SearchGroupaddToLogger);


                int minDelay = 20;
                int maxDelay = 25;

                if (!string.IsNullOrEmpty(txtSearchGroupMinDelay.Text) && NumberHelper.ValidateNumber(txtSearchGroupMinDelay.Text))
                {
                    minDelay = Convert.ToInt32(txtSearchGroupMinDelay.Text);
                }
                if (!string.IsNullOrEmpty(txtSearchGroupMaxDelay.Text) && NumberHelper.ValidateNumber(txtSearchGroupMaxDelay.Text))
                {
                    maxDelay = Convert.ToInt32(txtSearchGroupMaxDelay.Text);
                }

                //if (lstJoinGroupUrl.Count > 0)
                //{
                //    JoinSearchGroup.JoinSearchGroup.lstLinkedinGroupURL = lstJoinGroupUrl;
                //}
                if (!Login.IsLoggedIn)
                {
                    Login.LoginHttpHelper(ref HttpHelper);                   
                }

                if (Login.IsLoggedIn)
                {
                    GroupStatus dataScrape = new GroupStatus();
                    Dictionary<string, string> Result = obj_JoinSearchGroup.PostAddOpenGroupsUsingUrl(ref HttpHelper, Login.accountUser, minDelay, maxDelay, lstJoinGroupUrl, IsDevideData);
                    LinkdInContacts.Add(Login.accountUser, Result);                    
                }

                //AddLoggerJoinGroupUrl("[ " + DateTime.Now + " ] => [ Now Joining Groups Process Running ]");


                //string MessagePosted = obj_JoinSearchGroup.PostAddGroupUsingUrl(ref HttpHelper,LinkdInContacts, Login.accountUser, Login.accountPass, minDelay, maxDelay);

            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Search Groups --> PostAddSearchGroups() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Search Groups --> PostAddSearchGroups() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddSearchGroupErrorLogs);
            }
            finally 
            {
                counter_JoinGroupUrlID--;

                if (counter_JoinGroupUrlID == 0)
                {
                    btnJoinSearchGroup.Invoke(new MethodInvoker(delegate
                        {
                            AddLoggerJoinGroupUrl("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                            AddLoggerJoinGroupUrl("----------------------------------------------------------------------------------------------------------");
                            btnJoinSearchGroup.Cursor = Cursors.Default;
                        }));
                }
            }
        }

        #endregion

        private void rdBtnJoinGroupUsingUrlDivideByGivenNo_CheckedChanged(object sender, EventArgs e)
        {
            if (rdBtnJoinGroupUsingUrlDivideByGivenNo.Checked)
            {
                txtJoinGroupUsingUrlNoOfUsers.Enabled = true;
            }
            else 
            {
                txtJoinGroupUsingUrlNoOfUsers.Enabled = false;
            }
        }

        public static List<List<string>> Split(List<string> source, int splitNumber)
        {
            if (splitNumber <= 0)
            {
                splitNumber = 1;
            }

            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / splitNumber)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }
     
    }
}
