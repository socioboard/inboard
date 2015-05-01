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
    public partial class frmGroupsInvitation : Form
    {
        public System.Drawing.Image image;
        Dictionary<string, Dictionary<string, string>> LinkdInContacts = new Dictionary<string, Dictionary<string, string>>();
        List<Thread> lstGroupInvitationThread = new List<Thread>();
        bool IsStop = false;
        bool CheckNetConn = false;
        List<string> lstEmails = new List<string>();
        public frmGroupsInvitation()
        {
            InitializeComponent();
        }


        #region frmRemovePendingGroups_Load
        private void frmGroupsInvitation_Load(object sender, EventArgs e)
        {
            image = Properties.Resources.background;

        }
        #endregion

        #region frmRemovePendingGroups_Paint
        private void frmRemovePendingGroups_Paint(object sender, PaintEventArgs e)
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

        #region AddLoggerPendingGroups

        private void AddLoggerGroupsInvitation(string log)
        {
            this.Invoke(new MethodInvoker(delegate
            {
                lstLogInviteGroups.Items.Add(log);
                lstLogInviteGroups.SelectedIndex = lstLogInviteGroups.Items.Count - 1;
            }));
        }

        #endregion

        #region ScrapeEvent_addToLogger

        void GroupStatus_addToLogger(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eventArgs = e as EventsArgs;
                AddLoggerGroupsInvitation(eventArgs.log);
            }
        }

        #endregion

        #region btnAddPendingGroups_Click
        private void btnAddPendingGroups_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {

                try
                {
                    if (LinkedInManager.linkedInDictionary.Count() == 0)
                    {
                        MessageBox.Show("Please Load LinkedIn Accounts From MyAccounts Menu");
                        AddLoggerGroupsInvitation("[ " + DateTime.Now + " ] => [ Please Load LinkedIn Accounts From MyAccounts Menu ]");

                        frmAccounts FrmAccount = new frmAccounts();
                        FrmAccount.Show();
                        return;
                    }

                    lstLogInviteGroups.Items.Clear();
                    chkOwnGroup.Items.Clear();

                    cmbUserPendingGroup.Items.Clear();
                    LinkdInContacts.Clear();

                    AddLoggerGroupsInvitation("[ " + DateTime.Now + " ] => [ Starting Search for Pending Groups.. ]");

                    btnAddPendingGroups.Cursor = Cursors.AppStarting;

                    new Thread(() =>
                    {
                        LinkedInAddOwnGroup();

                    }).Start();
                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Pending Groups --> btnAddPendingGroups_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Pending Groups --> btnAddPendingGroups_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPendingGroupErrorLogs);
                }
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddLoggerGroupsInvitation("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        }
        #endregion

        #region LinkedInAddOwnGroup()

        public int counter_GroupSearch = 0;

        private void LinkedInAddOwnGroup()
        {
            int numberofThreads = 5;

            try
            {
                counter_GroupSearch = LinkedInManager.linkedInDictionary.Count;

                if (LinkedInManager.linkedInDictionary.Count > 0)
                {
                    ThreadPool.SetMaxThreads(numberofThreads, 5);
                    foreach (KeyValuePair<string, LinkedInMaster> item in LinkedInManager.linkedInDictionary)
                    {
                        ThreadPool.SetMaxThreads(numberofThreads, 5);
                        ThreadPool.QueueUserWorkItem(new WaitCallback(StartMultiThreadedOwnGroupAdd), new object[] { item });
                        Thread.Sleep(1000);
                    }
                }
                else
                {
                    MessageBox.Show("Please Load LinkedIn Accounts From MyAccounts Menu");
                    AddLoggerGroupsInvitation("[ " + DateTime.Now + " ] => [ Please Load LinkedIn Accounts From MyAccounts Menu ]");
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Groups for Invitation --> LinkedInAddOwnGroup() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Groups for Invitation --> LinkedInAddOwnGroup() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPendingGroupErrorLogs);
            }
        }

        #endregion

        #region StartMultiThreadedOwnGroupAdd

        public void StartMultiThreadedOwnGroupAdd(object parameter)
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
                    AddLoggerGroupsInvitation("[ " + DateTime.Now + " ] => [ Own Groups Adding of Acount + " + Login.accountUser + " please wait for sometimes ]");
                    Dictionary<string, string> Groups = dataScrape.GetSelectedIDForOwnGroups(ref HttpHelper, Login.accountUser);

                    LinkdInContacts.Add(Login.accountUser, Groups);

                    try
                    {
                        if (Groups.Count > 0)
                        {
                            AddLoggerGroupsInvitation("[ " + DateTime.Now + " ] => [ Own Group List..Added Successfully..in " + Login.accountUser + " ]");
                        }
                        else
                        {
                            AddLoggerGroupsInvitation("[ " + DateTime.Now + " ] => [ Own Group List..Not Available..in " + Login.accountUser + " ]");
                        }
                    }
                    catch (Exception ex)
                    {
                        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Groups for Invitation --> StartMultiThreadedPendingGroupAdd() >> 1 >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Groups for Invitation --> StartMultiThreadedPendingGroupAdd() >> 2 >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPendingGroupErrorLogs);
                    }

                    if (cmbUserPendingGroup.InvokeRequired)
                    {
                        new Thread(() =>
                        {
                            cmbUserPendingGroup.Invoke(new MethodInvoker(delegate
                            {
                                cmbUserPendingGroup.Items.Add(Login.accountUser);
                            }));
                        }).Start();
                    }
                }
                else
                {
                    AddLoggerGroupsInvitation("[ " + DateTime.Now + " ] => [ LinkedIn Account : " + Login.accountUser + " has been temporarily restricted ]");
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Groups for Invitation --> StartMultiThreadedPendingGroupAdd() >> 2 >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Groups for Invitation --> StartMultiThreadedPendingGroupAdd() >> 2>>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPendingGroupErrorLogs);
            }
            finally
            {
                counter_GroupSearch--;

                if (counter_GroupSearch == 0)
                {
                    if (btnAddPendingGroups.InvokeRequired)
                    {
                        btnAddPendingGroups.Invoke(new MethodInvoker(delegate
                        {
                            btnAddPendingGroups.Enabled = true;
                            AddLoggerGroupsInvitation("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                            AddLoggerGroupsInvitation("------------------------------------------------------------------------------------------------------------------------------------");
                            btnAddPendingGroups.Cursor = Cursors.Default;
                        }));
                    }
                }
            }
        }

        #endregion

        
        private void btnEmails_Click(object sender, EventArgs e)
        {
            try
            {
                txtEmails.Text = "";
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "Text Files (*.txt)|*.txt";
                    ofd.InitialDirectory = Application.StartupPath;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtEmails.Text = ofd.FileName;
                        List<string> templist = GlobusFileHelper.ReadFile(ofd.FileName);

                        //JoinSearchGroup.JoinSearchGroup.lstLinkedinGroupURL.Clear();
                        lstEmails.Clear();
                        foreach (string item in templist)
                        {
                            //JoinSearchGroup.JoinSearchGroup.lstLinkedinGroupURL.Add(item);
                            GroupStatus.lstEmailsGroupInvite.Add(item);
                            lstEmails.Add(item);
                        }

                        //JoinSearchGroup.JoinSearchGroup.lstLinkedinGroupURL = JoinSearchGroup.JoinSearchGroup.lstLinkedinGroupURL.Distinct().ToList();
                        AddLoggerGroupsInvitation("[ " + DateTime.Now + " ] => [ " + lstEmails.Count + " Emails Added ]");
                    }
                }
            }
            catch
            {
            }
        }

        #region cmbUserPendingGroup_SelectedIndexChanged
        private void cmbUserPendingGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            chkOwnGroup.Items.Clear();
            lblTotGroupCount.Text = "(" + "0" + ")";
            label1.Text = cmbUserPendingGroup.SelectedItem.ToString();

            string GetUserID = cmbUserPendingGroup.SelectedItem.ToString();

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

                            if (GetUserID == group1[1].ToString())
                            {
                                chkOwnGroup.Items.Add(item1.Key.Replace(",", string.Empty));
                            }
                        }

                        AddLoggerGroupsInvitation("[ " + DateTime.Now + " ] => [ " + chkOwnGroup.Items.Count + " Adding Own Groups of User: " + GetUserID + " ]");
                        AddLoggerGroupsInvitation("[ " + DateTime.Now + " ] => [ Finished Adding Own Groups of User: " + GetUserID + " ]");
                        AddLoggerGroupsInvitation("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                        AddLoggerGroupsInvitation("------------------------------------------------------------------------------------------------------------------------------------");
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Own Groups --> cmbUserPendingGroup_SelectedIndexChanged() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Own Groups --> cmbUserPendingGroup_SelectedIndexChanged() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPendingGroupErrorLogs);
            }
        }
        #endregion

        #region chkSelectPendingGrp_CheckedChanged
        private void chkSelectPendingGrp_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkSelectPendingGrp.Checked)
                {
                    IsallaccChecked = true;
                    for (int i = 0; i < chkOwnGroup.Items.Count; i++)
                    {
                        try
                        {
                            string aaa = Convert.ToString(chkOwnGroup.Items[i]);
                            chkOwnGroup.SetItemChecked(i, true);
                            lblTotGroupCount.Text = "(" + chkOwnGroup.Items.Count + ")";
                        }
                        catch { }
                    }
                }
                else
                {
                    for (int i = 0; i < chkOwnGroup.Items.Count; i++)
                    {
                        try
                        {
                            string aaa = Convert.ToString(chkOwnGroup.Items[i]);
                            chkOwnGroup.SetItemChecked(i, false);
                            int TotSelectedFrinedList = 0;
                            lblTotGroupCount.Text = "(" + TotSelectedFrinedList + ")";
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

        #region chkPendingGroup_SelectedIndexChanged
        private void chkPendingGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int count = Convert.ToInt32(lblTotGroupCount.Text.Replace("(", string.Empty).Replace(")", string.Empty));
                int count2 = chkOwnGroup.CheckedItems.Count + 1;
                if (chkOwnGroup.CheckOnClick == true)
                {
                    if (count < count2)
                    {
                        lblTotGroupCount.Text = "(" + (count + 1).ToString() + ")";
                    }
                    else
                    {
                        lblTotGroupCount.Text = "(" + (count - 1).ToString() + ")";
                    }
                }
            }
            catch { }
        }
        #endregion

        #region btnRemovePendingGroup_Click
        private List<string> lstGroupInvitation = new List<string>();
        private void btnRemovePendingGroup_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {

                try
                {
                    lstGroupInvitation.Clear();
                    lstGroupInvitationThread.Clear();

                    if (IsStop)
                    {
                        IsStop = false;
                    }

                    if (LinkedInManager.linkedInDictionary.Count() == 0)
                    {
                        return;
                    }
                    else if (chkOwnGroup.CheckedItems.Count == 0)
                    {
                        AddLoggerGroupsInvitation("[ " + DateTime.Now + " ] => [ Please select Atleast One Group.. ]");
                        MessageBox.Show("Please select Atleast One Group..");
                        return;
                    }
                    else if (GroupStatus.lstEmailsGroupInvite.Count == 0)
                    {
                        AddLoggerGroupsInvitation("[ " + DateTime.Now + " ] => [ Please Add Emails to send Group Invitation ]");
                        MessageBox.Show("Please Add Emails to send Group Invitation..");
                        return;
                    }

                    counter_GroupSearch = 0;
                    AddLoggerGroupsInvitation("[ " + DateTime.Now + " ] => [ Process Running Please wait for sometimes.. ]");
                    btnRemovePendingGroup.Cursor = Cursors.AppStarting;

                    if (chkOwnGroup.CheckedItems.Count > 0)
                    {
                        new Thread(() =>
                        {
                            LinkedInGroupsInvitation();

                        }).Start();
                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Own Groups --> btnRemovePendingGroup_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Own Groups --> btnRemovePendingGroup_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPendingGroupErrorLogs);
                }
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddLoggerGroupsInvitation("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        }
        #endregion

        #region LinkedInRemovePendingGroups
        public bool IsallaccChecked = false;
        int counter_PendGroupRemoved = 0;
        private void LinkedInGroupsInvitation()
        {
            try
            {
                int numberOfThreads = 20;
                counter_PendGroupRemoved = LinkedInManager.linkedInDictionary.Count;

                string SelectedEmail = string.Empty;
                this.Invoke(new MethodInvoker(delegate
                {
                    SelectedEmail = cmbUserPendingGroup.SelectedItem.ToString();
                }));

                if (GroupStatus.lstEmailsGroupInvite.Count<=0)
                {
                    AddLoggerGroupsInvitation("[ " + DateTime.Now + " ] => [ Please Upload Emails.. ]");
                    return;
                }

                try
                {
                    if (LinkedInManager.linkedInDictionary.Count() > 0)
                    {
                        ThreadPool.SetMaxThreads(numberOfThreads, 5);

                        foreach (KeyValuePair<string, LinkedInMaster> item in LinkedInManager.linkedInDictionary)
                        {
                            if (!chkSelectPendingGrp.Checked)
                            {
                                if (SelectedEmail.Contains(item.Key))
                                {
                                    ThreadPool.SetMaxThreads(numberOfThreads, 5);

                                    ThreadPool.QueueUserWorkItem(new WaitCallback(PostGroupsInvitationMail), new object[] { item });

                                    Thread.Sleep(1000);
                                }
                            }
                            else
                            {
                                if (IsallaccChecked)
                                {

                                    ThreadPool.SetMaxThreads(numberOfThreads, 5);

                                    ThreadPool.QueueUserWorkItem(new WaitCallback(PostGroupsInvitationMail), new object[] { item });

                                    Thread.Sleep(1000);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Own Groups --> LinkedInRemovePendingGroups() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Own Groups --> LinkedInRemovePendingGroups() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPendingGroupErrorLogs);
                }
            }
            catch
            {
            }
        }

        #endregion

        #region PostGroupsInvitationMail

        public void PostGroupsInvitationMail(object parameter)
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
                        lstGroupInvitationThread.Add(Thread.CurrentThread);
                        lstGroupInvitationThread.Distinct().ToList();
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

                GroupStatus obj_GroupStatus = new GroupStatus();
                obj_GroupStatus = new GroupStatus(Login.accountUser, Login.accountPass, Login.proxyAddress, Login.proxyPort, Login.proxyUserName, Login.proxyPassword);
                obj_GroupStatus.loggerInviteGroups.addToLogger += new EventHandler(GroupStatus_addToLogger);

                if (!Login.IsLoggedIn)
                {
                    Login.LoginHttpHelper(ref HttpHelper);
                }

                List<string> SelectedItem = new List<string>();

                try
                {
                    if (label1.Text == Login.accountUser)
                    {

                        try
                        {

                            foreach (string SelectedGrp in chkOwnGroup.CheckedItems)
                            {
                                foreach (var itemID in LinkdInContacts)
                                {

                                    foreach (KeyValuePair<string, string> itemGid in itemID.Value)
                                    {
                                        if (SelectedGrp.Split(':')[1] == itemID.Key)
                                        {
                                            if (SelectedGrp.Split(':')[0] == itemGid.Key.Split(':')[0])
                                            {
                                                try
                                                {
                                                    lstGroupInvitation.Add(itemGid.Key + ":" + itemGid.Value);
                                                }
                                                catch
                                                {
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            if (lstGroupInvitation.Count > 0)
                            {
                                try
                                {
                                    GroupStatus.lstInvitationGroup = lstGroupInvitation;
                                }
                                catch
                                {
                                }
                            }


                            int minDelay = 20;
                            int maxDelay = 25;

                            if (!string.IsNullOrEmpty(txtPendingGroupMinDelay.Text) && NumberHelper.ValidateNumber(txtPendingGroupMinDelay.Text))
                            {
                                minDelay = Convert.ToInt32(txtPendingGroupMinDelay.Text);
                            }
                            if (!string.IsNullOrEmpty(txtPendingGroupMaxDelay.Text) && NumberHelper.ValidateNumber(txtPendingGroupMaxDelay.Text))
                            {
                                maxDelay = Convert.ToInt32(txtPendingGroupMaxDelay.Text);
                            }

                            string MessagePosted = obj_GroupStatus.PostInvitationGroups(Login.accountUser, Login.accountPass, minDelay, maxDelay);
                            Thread.Sleep(2000);
                        }
                        catch
                        {
                        }
                    }
                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Own Groups --> PostRemovePendingGroups() ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Own Groups --> PostRemovePendingGroups() ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPendingGroupErrorLogs);
                }

            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Own Groups --> PostAddGroups() ---2--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Own Groups --> PostAddGroups() ---2--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPendingGroupErrorLogs);
            }
            finally
            {
                counter_PendGroupRemoved--;
                if (counter_PendGroupRemoved == 0)
                {
                    if (btnRemovePendingGroup.InvokeRequired)
                    {
                        btnRemovePendingGroup.Invoke(new MethodInvoker(delegate
                        {
                            AddLoggerGroupsInvitation("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                            AddLoggerGroupsInvitation("------------------------------------------------------------------------------------------------------------------------------------");
                            btnRemovePendingGroup.Enabled = true;
                            btnRemovePendingGroup.Cursor = Cursors.Default;
                        }));
                    }
                }
            }
        }

        #endregion

        #region btnStopPendingGroup_Click
        private void btnStopPendingGroup_Click(object sender, EventArgs e)
        {
            try
            {
                IsStop = true;
                List<Thread> lstTemp = lstGroupInvitationThread.Distinct().ToList();
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
                AddLoggerGroupsInvitation("------------------------------------------------------------------------------------------------------------------------------------");
                AddLoggerGroupsInvitation("[ " + DateTime.Now + " ] => [ PROCESS STOPPED ]");
                AddLoggerGroupsInvitation("------------------------------------------------------------------------------------------------------------------------------------");
                btnRemovePendingGroup.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error >>> " + ex.StackTrace);
            }
        }
        #endregion


    }
}
