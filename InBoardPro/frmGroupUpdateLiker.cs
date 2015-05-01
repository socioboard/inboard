using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Drawing.Drawing2D;
using Groups;
using BaseLib;

namespace InBoardPro
{
    public partial class frmGroupUpdateLiker : Form
    {
        #region varaiable declaration
        public System.Drawing.Image image;
        List<Thread> lstGroupUpdateThread = new List<Thread>();
        bool IsStop = false;
        bool CheckNetConn = false;
        #endregion

        #region frmGroupUpdateLiker
        public frmGroupUpdateLiker()
        {
            InitializeComponent();
        } 
        #endregion

        #region frmGroupUpdateLiker_Load
        private void frmGroupUpdateLiker_Load(object sender, EventArgs e)
        {
            image = Properties.Resources.background;
            Groups.GroupUpdate.loggerLiker.addToLogger += new EventHandler(AddToLogger_GroupStatus);

            LoadPreScrapper();
        } 
        #endregion

        #region LoadPreScrapper() method
        private void LoadPreScrapper()
        {
            try
            {
                if (LinkedInManager.linkedInDictionary.Count() == 0)
                {
                    MessageBox.Show("Please Load LinkedIn Accounts From MyAccounts Menu");
                    AddLoggerCommentLiker("[ " + DateTime.Now + " ] => [ Please Load LinkedIn Accounts From MyAccounts Menu ]");

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

        #region PopulateCmo()
        private void PopulateCmo()
        {
            try
            {
                cmbGroupUser.Items.Clear();
                foreach (KeyValuePair<string, LinkedInMaster> item in LinkedInManager.linkedInDictionary)
                {
                    cmbGroupUser.Items.Add(item.Key);
                }
                AddLoggerCommentLiker("[ " + DateTime.Now + " ] => [ Accounts Uploaded..]");
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> PopulateCmo() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> PopulateCmo() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPreScrapperErrorLogs);
            }
        }

        #endregion

        #region AddToLogger_GroupStatus

        void AddToLogger_GroupStatus(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eventArgs = e as EventsArgs;
                AddLoggerCommentLiker(eventArgs.log);
            }
        }

        #endregion

        #region btnGetUser_Click
        public int counter_GroupMemberSearch = 0;
        private void btnGetUser_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {

                try
                {
                    if (LinkedInManager.linkedInDictionary.Count() == 0)
                    {
                        MessageBox.Show("Please Load LinkedIn Accounts From MyAccounts Menu");
                        AddLoggerCommentLiker("[ " + DateTime.Now + " ] => [ Please Load LinkedIn Accounts From MyAccounts Menu ]");
                        lstLoglinkdinScarper.Items.Clear();
                        frmAccounts FrmAccount = new frmAccounts();
                        FrmAccount.Show();
                        return;
                    }

                    GrpMess.Clear();
                    lstLoglinkdinScarper.Items.Clear();
                    chkUpdateCollection.Items.Clear();
                    cmbGroupUser.Items.Clear();
                    cmbGroupUser.Items.Add("Select All Account");
                    //pictureBox3Red.Visible = true;
                    //pictureBox3Green.Visible = false;
                    AddLoggerCommentLiker("[ " + DateTime.Now + " ] => [ Starting Process for Login... ]");
                    AddLoggerCommentLiker("[ " + DateTime.Now + " ] => [ Process is Running Now... ]");
                    btnGetUser.Cursor = Cursors.AppStarting;

                    new Thread(() =>
                    {
                        LinkdinGroupUpdate();

                    }).Start();
                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update --> btnGetUser_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update --> btnGetUser_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinGetGroupMemberErrorLogs);
                }
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddLoggerCommentLiker("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        } 
        #endregion

        #region LinkdinGroupUpdate()

        private void LinkdinGroupUpdate()
        {
            int numberofThreds = 5;
            counter_GroupMemberSearch = LinkedInManager.linkedInDictionary.Count;

            if (LinkedInManager.linkedInDictionary.Count() > 0)
            {
                ThreadPool.SetMaxThreads(numberofThreds, 5);
                foreach (KeyValuePair<string, LinkedInMaster> item in LinkedInManager.linkedInDictionary)
                {
                    try
                    {
                        ThreadPool.SetMaxThreads(numberofThreds, 5);

                        ThreadPool.QueueUserWorkItem(new WaitCallback(StartDMMultiThreadedGroupUser), new object[] { item });

                        Thread.Sleep(1000);
                    }
                    catch (Exception ex)
                    {
                        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update --> LinkdinGroupUpdate() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update --> LinkdinGroupUpdate() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinGetGroupMemberErrorLogs);
                    }

                }
            }
            else
            {
                MessageBox.Show("Please Load LinkedIn Accounts From MyAccounts Menu");
                AddLoggerCommentLiker("[ " + DateTime.Now + " ] => [ Please Load LinkedIn Accounts From MyAccounts Menu ]");
            }
        }

        #endregion

        #region StartDMMultiThreadedGroupUser
        Dictionary<string, Dictionary<string, string>> GrpMess = new Dictionary<string, Dictionary<string, string>>();
        public void StartDMMultiThreadedGroupUser(object parameter)
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

                Groups.GroupUpdate obj_GroupUpdate = new Groups.GroupUpdate(Login.accountUser, Login.accountPass, Login.proxyAddress, Login.proxyPort, Login.proxyUserName, Login.proxyPassword);

                obj_GroupUpdate.logger.addToLogger += new EventHandler(AddToLogger_GroupStatus);

                if (!Login.IsLoggedIn)
                {
                    Login.LoginHttpHelper(ref HttpHelper);
                    AddLoggerCommentLiker("[ " + DateTime.Now + " ] => [ Loggin In With Email : " + item.Value._Username);
                }

                AddLoggerCommentLiker("[ " + DateTime.Now + " ] => [ Starting Search For comments >>> To Like ]");

                try
                {
                    if (Login.IsLoggedIn)
                    {
                        GroupStatus dataScrape = new GroupStatus();
                        Dictionary<string, string> Data = new Dictionary<string, string>();
                        Data.Clear();

                        Data = obj_GroupUpdate.PostCommentsForLiker(ref HttpHelper, Login.accountUser);

                        GrpMess.Add(Login.accountUser, Data);

                        obj_GroupUpdate.logger.addToLogger -= new EventHandler(AddToLogger_GroupStatus);

                        if (cmbGroupUser.InvokeRequired)
                        {
                            new Thread(() =>
                            {
                                cmbGroupUser.Invoke(new MethodInvoker(delegate
                                {
                                    cmbGroupUser.Items.Add(Login.accountUser);
                                }));
                            }).Start();
                        }

                        AddLoggerCommentLiker("[ " + DateTime.Now + " ] => [ Comment Added in : " + item.Value._Username + " ]");
                    }
                    else
                    {
                        AddLoggerCommentLiker("[ " + DateTime.Now + " ] => [ LinkedIn account : " + Login.accountUser + " has been temporarily restricted ]");
                    }
                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update --> StartDMMultiThreadedGroupUser() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update --> StartDMMultiThreadedGroupUser() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinGetGroupMemberErrorLogs);
                }
                finally
                {
                    counter_GroupMemberSearch--;

                    if (counter_GroupMemberSearch == 0)
                    {
                        if (cmbGroupUser.InvokeRequired)
                        {
                            cmbGroupUser.Invoke(new MethodInvoker(delegate
                            {
                                AddLoggerCommentLiker("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                                AddLoggerCommentLiker("----------------------------------------------------------------------------------------------------------");
                                cmbGroupUser.Enabled = true;
                                btnGetUser.Cursor = Cursors.Default;
                             }));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update --> StartDMMultiThreadedGroupUser() >>>> " + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update --> StartDMMultiThreadedGroupUser() >>>> " + ex.StackTrace, Globals.Path_LinkedinGetGroupMemberErrorLogs);
            }
        }

        #endregion

        #region AddLoggerCommentLiker

        private void AddLoggerCommentLiker(string log)
        {
            this.Invoke(new MethodInvoker(delegate
            {
                lstLoglinkdinScarper.Items.Add(log);
                lstLoglinkdinScarper.SelectedIndex = lstLoglinkdinScarper.Items.Count - 1;
            }));
        }

        #endregion

        #region cmbGroupUser_SelectedIndexChanged

        private void cmbGroupUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<string> allacount = new List<string>();
            string GetUserID = string.Empty;
            chkUpdateCollection.Items.Clear();
            GroupStatus.GroupUrl.Clear();
            try
            {
                GetUserID = cmbGroupUser.SelectedItem.ToString();
                label47.Text = cmbGroupUser.SelectedItem.ToString();

               
                foreach (KeyValuePair<string, Dictionary<string, string>> item in GrpMess)
                {
                    if (GetUserID.Contains(item.Key))
                    {
                        List<string> GmUserIDs = new List<string>();
                        foreach (KeyValuePair<string, string> item1 in item.Value)
                        {
                            string group = item1.Key;
                            string[] group1 = group.Split('#');

                            if (GetUserID == group1[1].ToString())
                            {
                                chkUpdateCollection.Items.Add(group1[1] + '#' + group1[0].ToString());
                                GroupStatus.GroupUrl.Add(item1.Value);
                             
                            }
                        }
                    }

                    if (GetUserID == "Select All Account")
                    {
                        foreach (string Users in cmbGroupUser.Items)
                        {
                            string grpUser = Users;
                            foreach (KeyValuePair<string, string> item2 in item.Value)
                            {
                                
                                string group1 = item2.Key;
                                string[] group2 = group1.Split('#');
                                
                                
                                
 //#x2019;s

                                try
                                {
                                    //if (grpUser == group2[1].ToString())
                                    //{
                                        GroupStatus.GroupUrl.Add(item2.Value);
                                        allacount.Add(group2[1] + ':' + group2[0].ToString());
                                        GroupStatus.GroupUrl = GroupStatus.GroupUrl.Distinct().ToList();
                                        allacount = allacount.Distinct().ToList();
                                    //}
                                }
                                catch { }
                            }
                        }
                    }
                }
                //x2019;s
                //x2018;
                foreach (var AllGroups in allacount)
                {

                    var _AllGroups = AllGroups.Replace("x2018", string.Empty).Replace("x2019;s","").Trim();
                    chkUpdateCollection.Items.Add(_AllGroups);
                    //AddLoggerScrapeUsers(DateTime.Now + " [ " + GroupStatus.GroupUrl.Count() + " Groups List of :" + "" + GetUserID + " ]");
                    //AddLoggerScrapeUsers(DateTime.Now + " [ Finished Adding Groups of Usernames ]");
                }

            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update --> cmbGroupUser_SelectedIndexChanged() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update --> cmbGroupUser_SelectedIndexChanged() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinGetGroupMemberErrorLogs);
            }
        }

        #endregion

        #region frmGroupUpdateLiker_Paint

        private void frmGroupUpdateLiker_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                Graphics g;
                g = e.Graphics;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.DrawImage(image, 0, 0, this.Width, this.Height);
            }
            catch { }
        }

        #endregion

        #region btnStatusLikerStop_Click
        private void btnStatusLikerStop_Click(object sender, EventArgs e)
        {
            try
            {
                IsStop = true;
                List<Thread> lstTemp = lstGroupUpdateThread.Distinct().ToList();
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

                AddLoggerCommentLiker("-------------------------------------------------------------------------------------------------------------------------------");
                AddLoggerCommentLiker("[ " + DateTime.Now + " ] => [ PROCESS STOPPED ]");
                AddLoggerCommentLiker("-------------------------------------------------------------------------------------------------------------------------------");
                btnGetUser.Cursor = Cursors.Default;
                btnUpdateLike.Cursor = Cursors.Default;
            }

            catch (Exception ex)
            {
                Console.WriteLine("Error >>> " + ex.StackTrace);
            }
        } 
        #endregion

        #region btnUpdateLike_Click
        private void btnUpdateLike_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {

                try
                {
                    lstGroupUpdateThread.Clear();
                    if (IsStop)
                    {
                        IsStop = false;
                    }

                    if (chkUpdateCollection.CheckedItems.Count == 0)
                    {
                        AddLoggerCommentLiker("[ " + DateTime.Now + " ] => [ Please select Atleast One Group.. ]");
                        MessageBox.Show("Please select Atleast One Group..");
                        return;
                    }

                    btnUpdateLike.Cursor = Cursors.AppStarting;
                    counter_GroupMemberSearch = 0;

                    if (chkUpdateCollection.CheckedItems.Count > 0)
                    {
                        new Thread(() =>
                        {
                            LinkedInUpdateLiker();

                        }).Start();
                    }

                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update Liker --> btnUpdateLike_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update Liker--> btnUpdateLike_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinGetGroupMemberErrorLogs);
                }
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddLoggerCommentLiker("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        } 
        #endregion

        #region LinkedInGroupMessage()

        private void LinkedInUpdateLiker()
        {
            try
            {
                int numberOfThreads = 0;
             
                if (GroupStatus.GroupUrl.Count > 0)
                {
                    foreach (string grpKey in GroupStatus.GroupUrl)
                    {
                        Groups.GroupUpdate.Que_GrpKey_Post.Enqueue(grpKey);
                    }
                }
                else
                {
                    AddLoggerCommentLiker("[ " + DateTime.Now + " ] => [ Group User Key Invalid ]");
                    return;
                }

                counter_GroupMemberSearch = LinkedInManager.linkedInDictionary.Count;

                if (LinkedInManager.linkedInDictionary.Count() > 0)
                {
                    ThreadPool.SetMaxThreads(numberOfThreads, 5);

                    foreach (KeyValuePair<string, LinkedInMaster> item in LinkedInManager.linkedInDictionary)
                    {
                        string value = string.Empty;
                        cmbGroupUser.Invoke(new MethodInvoker(delegate
                        {
                            value = cmbGroupUser.SelectedItem.ToString();
                        }));

                        if (value.Contains(item.Key))
                        {
                            ThreadPool.SetMaxThreads(numberOfThreads, 5);

                            ThreadPool.QueueUserWorkItem(new WaitCallback(PostUpdateLiker), new object[] { item });

                            Thread.Sleep(1000);
                        }

                        if (value.Contains("Select All Account"))
                        {
                            ThreadPool.SetMaxThreads(numberOfThreads, 5);

                            ThreadPool.QueueUserWorkItem(new WaitCallback(PostUpdateLiker), new object[] { item });

                            Thread.Sleep(1000);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update --> LinkedInUpdateLiker() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update --> LinkedInUpdateLiker() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinGetGroupMemberErrorLogs);
            }
        }

        #endregion

        #region PostGroupMsgUpdate

        public void PostUpdateLiker(object parameter)
        {
            try
            {
                try
                {
                    if (!IsStop)
                    {
                        lstGroupUpdateThread.Add(Thread.CurrentThread);
                        lstGroupUpdateThread.Distinct().ToList();
                        Thread.CurrentThread.IsBackground = true;
                    }
                }
                catch
                {
                }

                Array paramsArray = new object[1];

                paramsArray = (Array)parameter;

                KeyValuePair<string, LinkedInMaster> item = (KeyValuePair<string, LinkedInMaster>)paramsArray.GetValue(0);

                GlobusHttpHelper HttpHelper = new GlobusHttpHelper();
                LinkedinLogin Login = new LinkedinLogin();
                Login.accountUser = item.Key;
                Login.accountPass = item.Value._Password;
                Login.proxyAddress = item.Value._ProxyAddress;
                Login.proxyPort = item.Value._ProxyPort;
                Login.proxyUserName = item.Value._ProxyUsername;
                Login.proxyPassword = item.Value._ProxyPassword;
                string user = item.Key;

                Groups.GroupUpdate obj_GroupUpdate = new Groups.GroupUpdate(Login.accountUser, Login.accountPass, Login.proxyAddress, Login.proxyPort, Login.proxyUserName, Login.proxyPassword);

                Login.logger.addToLogger += new EventHandler(AddToLogger_GroupStatus);
                obj_GroupUpdate.logger.addToLogger += new EventHandler(AddToLogger_GroupStatus);

                if (!Login.IsLoggedIn)
                {
                    Login.LoginHttpHelper(ref HttpHelper);
                    //AddLoggerGroupAdd("Login Process completed..  ");
                }

                List<string> GetCheckedItems = new List<string>();

                if (chkUpdateCollection.InvokeRequired)
                {
                    chkUpdateCollection.Invoke(new MethodInvoker(delegate
                    {
                        foreach (string Userid in chkUpdateCollection.CheckedItems)
                        {
                            string[] Uid = Userid.Split('#');
                            GetCheckedItems.Add(Uid[1]);
                        }
                    }));
                }

                List<string> SelectedItem = new List<string>();

                foreach (KeyValuePair<string, Dictionary<string, string>> NewValue in GrpMess)
                {
                    if (NewValue.Key == item.Key)
                    {
                        foreach (KeyValuePair<string, string> GroupId in NewValue.Value)
                        {
                            foreach (string selectedgroup in GetCheckedItems)
                            {
                                if (GroupId.Key.Contains(selectedgroup))
                                {
                                    SelectedItem.Add(GroupId + "#" + selectedgroup);

                                }
                            }
                        }
                    }
                }


                int minDelay = 20;
                int maxDelay = 25;

                if (!string.IsNullOrEmpty(txtGroupUpdateMinDelay.Text) && NumberHelper.ValidateNumber(txtGroupUpdateMinDelay.Text))
                {
                    minDelay = Convert.ToInt32(txtGroupUpdateMinDelay.Text);
                }
                if (!string.IsNullOrEmpty(txtGroupUpdateMaxDelay.Text) && NumberHelper.ValidateNumber(txtGroupUpdateMaxDelay.Text))
                {
                    maxDelay = Convert.ToInt32(txtGroupUpdateMaxDelay.Text);
                }

                obj_GroupUpdate.PostCommentLikerUpdate(ref HttpHelper, SelectedItem, new object[] { item, user }, minDelay, maxDelay);
              
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Update Liker --> PostUpdateLiker() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Update Liker --> PostUpdateLiker() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinGetGroupMemberErrorLogs);
            }
            finally
            {
                counter_GroupMemberSearch--;
                int cnt = LinkedInManager.linkedInDictionary.Count - 1;
                if (counter_GroupMemberSearch == cnt)
                {
                    if (btnUpdateLike.InvokeRequired)
                    {
                        btnUpdateLike.Invoke(new MethodInvoker(delegate
                        {
                            AddLoggerCommentLiker("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                            AddLoggerCommentLiker("---------------------------------------------------------------------------------------------------------------------------");
                            btnUpdateLike.Cursor = Cursors.Default;
                            
                        }));
                    }
                }
            }
        }

        #endregion

        #region chkGroupUpdate_CheckedChanged
        private void chkGroupUpdate_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkGroupUpdate.Checked == true)
                {
                    for (int i = 0; i < chkUpdateCollection.Items.Count; i++)
                    {
                        string aaa = Convert.ToString(chkUpdateCollection.Items[i]);
                        chkUpdateCollection.SetItemChecked(i, true);
                    }
                }
                else
                {
                    for (int i = 0; i < chkUpdateCollection.Items.Count; i++)
                    {
                        string aaa = Convert.ToString(chkUpdateCollection.Items[i]);
                        chkUpdateCollection.SetItemChecked(i, false);
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update --> chkGroupUpdate_CheckedChanged() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update --> chkGroupUpdate_CheckedChanged() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinGetGroupMemberErrorLogs);
            }
        } 
        #endregion
                       
    }
}
