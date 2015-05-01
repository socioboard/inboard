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
using Groups;
using BaseLib;

namespace InBoardPro
{
    public partial class frmFriendsGroupScraper : Form
    {
        #region global declaration
        public System.Drawing.Image image;
        Dictionary<string, Dictionary<string, string>> LinkdInContacts = new Dictionary<string, Dictionary<string, string>>();
        List<Thread> lstJoinFriendGroupThread = new List<Thread>();
        bool IsStop = false;
        bool CheckNetConn = false;
        #endregion

        #region frmFriendsGroupScraper
        public frmFriendsGroupScraper()
        {
            InitializeComponent();
        } 
        #endregion

        #region frmFriendsGroupScraper_Load
        private void frmFriendsGroupScraper_Load(object sender, EventArgs e)
        {
            image = Properties.Resources.background;
        } 
        #endregion

        #region frmFriendsGroupScraper_Paint
        private void frmFriendsGroupScraper_Paint(object sender, PaintEventArgs e)
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

        #region AddLoggerGroupAdd

        private void AddLoggerGroupAdd(string log)
        {
            this.Invoke(new MethodInvoker(delegate
            {
                listLoggerFriendsGroup.Items.Add(log);
                listLoggerFriendsGroup.SelectedIndex = listLoggerFriendsGroup.Items.Count - 1;
            }));
        }

        #endregion

        #region ScrapeEvent_addToLogger

        void GroupStatus_addToLogger(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eventArgs = e as EventsArgs;
                AddLoggerGroupAdd(eventArgs.log);
            }
        }

        #endregion

        #region btnExistGroup_Click
        private void btnExistGroup_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {

                try
                {
                    if (LinkedInManager.linkedInDictionary.Count() == 0)
                    {
                        MessageBox.Show("Please Load LinkedIn Accounts From MyAccounts Menu");
                        AddLoggerGroupAdd("[ " + DateTime.Now + " ] => [ Please Load LinkedIn Accounts From MyAccounts Menu ]");

                        frmAccounts FrmAccount = new frmAccounts();
                        FrmAccount.Show();
                        return;
                    }

                    chkMembers.Items.Clear();
                    cmbUser.Items.Clear();
                    AddLoggerGroupAdd("[ " + DateTime.Now + " ] => [ Starting Search for Members.. ]");
                    btnExistGroup.Cursor = Cursors.AppStarting;

                    new Thread(() =>
                    {
                        LinkedInGroupMemberSearch();

                    }).Start();
                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Group Scraper --> btnExistGroup_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Group Scraper --> btnExistGroup_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddFriendsGroupScraperErrorLogs);
                }
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddLoggerGroupAdd("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        } 
        #endregion

        #region LinkedInGroupMemberSearch()

        public int counter_GroupMemberSearch = 0;

        private void LinkedInGroupMemberSearch()
        {
            int numberofThreads = 5;

            try
            {
                counter_GroupMemberSearch = LinkedInManager.linkedInDictionary.Count;

                if (LinkedInManager.linkedInDictionary.Count > 0)
                {
                    ThreadPool.SetMaxThreads(numberofThreads, 5);
                    foreach (KeyValuePair<string, LinkedInMaster> item in LinkedInManager.linkedInDictionary)
                    {
                        ThreadPool.SetMaxThreads(numberofThreads, 5);
                        ThreadPool.QueueUserWorkItem(new WaitCallback(StartDMMultiThreadedGroupMemmberAdd), new object[] { item });
                        Thread.Sleep(1000);
                    }
                }
                else
                {
                    MessageBox.Show("Please Load LinkedIn Accounts From MyAccounts Menu");
                    AddLoggerGroupAdd("[ " + DateTime.Now + " ] => [ Please Load LinkedIn Accounts From MyAccounts Menu ]");
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Group Scraper --> LinkedInGroupMemberSearch() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Group Scraper --> LinkedInGroupMemberSearch() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddFriendsGroupScraperErrorLogs);
            }
        }

        #endregion

        #region StartDMMultiThreadedGroupMemmberAdd

        public void StartDMMultiThreadedGroupMemmberAdd(object parameter)
        {
            try
            {
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

                Login.logger.addToLogger += new EventHandler(GroupStatus_addToLogger);

                if (!Login.IsLoggedIn)
                {
                    Login.LoginHttpHelper(ref HttpHelper);
                }

                if (Login.IsLoggedIn)
                {
                    GroupStatus dataScrape = new GroupStatus();
                    GroupStatus.moduleLog = "FriendsGroupScapper";
                    Dictionary<string, string> Result = dataScrape.PostAddMembers(ref HttpHelper, Login.accountUser);

                    LinkdInContacts.Add(Login.accountUser, Result);

                    try
                    {
                        if (Result.Count > 0)
                        {
                            AddLoggerGroupAdd("[ " + DateTime.Now + " ] => [ Friends List..Added Successfully..in " + Login.accountUser + " ]");
                        }
                        else
                        {
                            AddLoggerGroupAdd("[ " + DateTime.Now + " ] => [ Friends List..Not Available..in " + Login.accountUser + " ]");
                        }
                    }
                    catch (Exception ex)
                    {
                        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> StartDMMultiThreadedGroupMemmberAdd() >> 1 >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> StartDMMultiThreadedGroupMemmberAdd() >> 2 >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddFriendsGroupErrorLogs);
                    }

                    if (cmbUser.InvokeRequired)
                    {
                        new Thread(() =>
                        {
                            cmbUser.Invoke(new MethodInvoker(delegate
                            {
                                cmbUser.Items.Add(Login.accountUser);
                            }));
                        }).Start();
                    }
                }
                else
                {
                    AddLoggerGroupAdd("[ " + DateTime.Now + " ] => [ LinkedIn Account : " + Login.accountUser + " has been temporarily restricted ]");
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> StartDMMultiThreadedGroupMemmberAdd() >> 2 >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> StartDMMultiThreadedGroupMemmberAdd() >> 2>>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddFriendsGroupErrorLogs);
            }
            finally
            {
                counter_GroupMemberSearch--;

                if (counter_GroupMemberSearch == 0)
                {
                    if (btnLinkedinSearch.InvokeRequired)
                    {
                        btnLinkedinSearch.Invoke(new MethodInvoker(delegate
                        {
                            btnLinkedinSearch.Enabled = true;
                            AddLoggerGroupAdd("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                            AddLoggerGroupAdd("------------------------------------------------------------------------------------------------------------------------------------");
                            btnExistGroup.Cursor = Cursors.Default;
                        }));
                    }
                }
            }
        }

        #endregion

        #region cmbUser_SelectedIndexChanged
        private void cmbUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            chkMembers.Items.Clear();
            lblTotMemCount.Text = "(" + "0" + ")";
            string GetUserID = cmbUser.SelectedItem.ToString();

            try
            {
                foreach (KeyValuePair<string, Dictionary<string, string>> item in LinkdInContacts)
                {
                    if (GetUserID.Contains(item.Key))
                    {
                        Dictionary<string, string> GmUserIDs = item.Value;
                        foreach (KeyValuePair<string, string> item1 in item.Value)
                        {
                            string group = item1.Key;
                            string[] group1 = group.Split(':');

                            if (GetUserID == group1[0].ToString())
                            {
                                chkMembers.Items.Add(item1.Value.Replace(",", string.Empty));
                            }
                        }

                        AddLoggerGroupAdd("[ " + DateTime.Now + " ] => [ " + chkMembers.Items.Count + " Friends in User: " + GetUserID + " ]");
                        AddLoggerGroupAdd("[ " + DateTime.Now + " ] => [ Finished Adding Friends of Selected User ]");
                        AddLoggerGroupAdd("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                        AddLoggerGroupAdd("------------------------------------------------------------------------------------------------------------------------------------");
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> cmbUser_SelectedIndexChanged() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> cmbUser_SelectedIndexChanged() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddFriendsGroupErrorLogs);
            }
        } 
        #endregion

        #region chkSelectMembers_JoinFriendsGrp_CheckedChanged
        private void chkSelectMembers_JoinFriendsGrp_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkSelectMembers_JoinFriendsGrp.Checked)
                {
                    for (int i = 0; i < chkMembers.Items.Count; i++)
                    {
                        try
                        {
                            string aaa = Convert.ToString(chkMembers.Items[i]);
                            chkMembers.SetItemChecked(i, true);
                            lblTotMemCount.Text = "(" + chkMembers.Items.Count + ")";
                        }
                        catch { }
                    }
                }
                else
                {
                    for (int i = 0; i < chkMembers.Items.Count; i++)
                    {
                        try
                        {
                            string aaa = Convert.ToString(chkMembers.Items[i]);
                            chkMembers.SetItemChecked(i, false);
                            int TotSelectedFrinedList = 0;
                            lblTotMemCount.Text = "(" + TotSelectedFrinedList + ")";
                        }
                        catch { }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error >>> " + ex.StackTrace);
            }
        } 
        #endregion

        #region chkMembers_SelectedIndexChanged
        private void chkMembers_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int count = Convert.ToInt32(lblTotMemCount.Text.Replace("(", string.Empty).Replace(")", string.Empty));
                int count2 = chkMembers.CheckedItems.Count + 1;
                if (chkMembers.CheckOnClick == true)
                {
                    if (count < count2)
                    {
                        lblTotMemCount.Text = "(" + (count + 1).ToString() + ")";
                    }
                    else
                    {
                        lblTotMemCount.Text = "(" + (count - 1).ToString() + ")";
                    }
                }
            }
            catch { }
        } 
        #endregion

        #region btnLinkedinSearch_Click
        Dictionary<string, Dictionary<string, string>> GrpAdd = new Dictionary<string, Dictionary<string, string>>();
        private void btnLinkedinSearch_Click(object sender, EventArgs e) 
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {

                try
                {
                    lstJoinFriendGroupThread.Clear();

                    if (IsStop)
                    {
                        IsStop = false;
                    }

                    if (LinkedInManager.linkedInDictionary.Count() == 0)
                    {
                        return;
                    }
                    else if (cmbUser.Items.Count == 0)
                    {
                        AddLoggerGroupAdd("[ " + DateTime.Now + " ] => [ Please click Add Friend button.. ]");
                        MessageBox.Show("Please click Add Friend button..");
                        return;
                    }
                    else if (chkMembers.Items.Count == 0)
                    {
                        AddLoggerGroupAdd("[ " + DateTime.Now + " ] => [ Please select ID.. ]");
                        MessageBox.Show("Please select ID..");
                        return;
                    }
                    if (chkMembers.CheckedItems.Count == 0)
                    {
                        AddLoggerGroupAdd("[ " + DateTime.Now + " ] => [ Please select Atleast One Friend.. ]");
                        MessageBox.Show("Please select Atleast One Friend..");
                        return;
                    }

                    GroupStatus.GroupDtl.Clear();
                    AddLoggerGroupAdd("[ " + DateTime.Now + " ] => [ Search Groups for Selected Friends.. ]");
                    btnLinkedinSearch.Cursor = Cursors.AppStarting;

                    new Thread(() =>
                    {
                        LinkedInMembersGroupSearch();

                    }).Start();
                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Group Scraper --> btnAddGroups_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Group Scraper --> btnAddGroups_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddFriendsGroupScraperErrorLogs);
                }
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddLoggerGroupAdd("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        }
        #endregion

        #region LinkedInMembersGroupSearch()

        private void LinkedInMembersGroupSearch()
        {
            int numberofThreads = 5;
            string SelectedEmail = string.Empty;

            this.Invoke(new MethodInvoker(delegate
            {
                SelectedEmail = cmbUser.SelectedItem.ToString();
            }));

            try
            {
                if (LinkedInManager.linkedInDictionary.Count > 0)
                {
                    ThreadPool.SetMaxThreads(numberofThreads, 5);
                    foreach (KeyValuePair<string, LinkedInMaster> item in LinkedInManager.linkedInDictionary)
                    {
                        if (SelectedEmail.Contains(item.Key))
                        {
                            ThreadPool.SetMaxThreads(numberofThreads, 5);
                            ThreadPool.QueueUserWorkItem(new WaitCallback(StartDMMultiThreadedMembersGroupScrpAdd), new object[] { item });
                            Thread.Sleep(1000);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Group Scraper --> LinkedInMembersGroupSearch() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Group Scraper --> LinkedInMembersGroupSearch() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddFriendsGroupScraperErrorLogs);
            }

        }

        #endregion

        #region StartDMMultiThreadedMemmbersGroupAdd
        List<string> MemId = new List<string>();
        public void StartDMMultiThreadedMembersGroupScrpAdd(object parameter)
        {
            try
            {
                try
                {
                    if (!IsStop)
                    {
                        lstJoinFriendGroupThread.Add(Thread.CurrentThread);
                        lstJoinFriendGroupThread.Distinct().ToList();
                        Thread.CurrentThread.IsBackground = true;
                    }
                }
                catch
                {
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

                string SelectedEmail = string.Empty;
                this.Invoke(new MethodInvoker(delegate
                {
                    SelectedEmail = cmbUser.SelectedItem.ToString();
                }));

                if (!Login.IsLoggedIn)
                {
                    Login.LoginHttpHelper(ref HttpHelper);
                }

                AddLoggerGroupAdd("[ " + DateTime.Now + " ] => [ Add Groups Process Running..Please wait.. ]");

                if (Login.IsLoggedIn)
                {
                    List<string> SelectedItem = new List<string>();
                    try
                    {
                        foreach (KeyValuePair<string, Dictionary<string, string>> UserValue in LinkdInContacts)
                        {
                            string SelectedValue = SelectedEmail;
                            if (UserValue.Key.Contains(SelectedValue))
                            {
                                foreach (KeyValuePair<string, string> GroupValue in UserValue.Value)
                                {
                                    foreach (string Userid in chkMembers.CheckedItems)
                                    {
                                        if (GroupValue.Value.Replace(",", string.Empty).Contains(Userid))
                                        {
                                            MemId.Add(GroupValue.Key.Split(':')[1]);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Group Scraper --> StartDMMultiThreadedMemmbersGroupAdd() ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Group Scraper --> StartDMMultiThreadedMemmbersGroupAdd() ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddFriendsGroupScraperErrorLogs);
                    }
                }

                GroupStatus dataScrape = new GroupStatus();
                GroupStatus.loggerFriendsGroup.addToLogger += new EventHandler(GroupStatus_addToLogger);
                            
                dataScrape.ScrapeFriendsGroup(ref HttpHelper, MemId, Login.accountUser);
               
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Group Scraper --> StartDMMultiThreadedMemmbersGroupAdd() ---4--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Group Scraper --> StartDMMultiThreadedMemmbersGroupAdd() ---4--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddFriendsGroupScraperErrorLogs);
            }
            finally
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    AddLoggerGroupAdd("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                    AddLoggerGroupAdd("------------------------------------------------------------------------------------------------------------------------------------");
                    btnLinkedinSearch.Cursor = Cursors.Default;
                }));


            }
        }

        #endregion

        #region btnLinkedinSearchStop_Click
        private void btnLinkedinSearchStop_Click(object sender, EventArgs e)
        {
            try
            {
                IsStop = true;
                foreach (Thread item in lstJoinFriendGroupThread)
                {
                    try
                    {
                        item.Abort();
                    }
                    catch
                    {
                    }
                }
                AddLoggerGroupAdd("-------------------------------------------------------------------------------------------------------------------------------");
                AddLoggerGroupAdd("[ " + DateTime.Now + " ] => [ PROCESS STOPPED ]");
                AddLoggerGroupAdd("-------------------------------------------------------------------------------------------------------------------------------");

                btnExistGroup.Cursor = Cursors.Default;
                btnLinkedinSearch.Cursor = Cursors.Default;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error >>> " + ex.StackTrace);
            }
        } 
        #endregion
 
    }
}
