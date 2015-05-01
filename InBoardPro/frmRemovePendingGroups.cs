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
    public partial class frmRemovePendingGroups : Form
    {
        public System.Drawing.Image image;
        Dictionary<string, Dictionary<string, string>> LinkdInContacts = new Dictionary<string, Dictionary<string, string>>();
        List<Thread> lstRemovePendingGroupThread = new List<Thread>();
        bool IsStop = false;
        bool CheckNetConn = false;

        public frmRemovePendingGroups()
        {
            InitializeComponent();
        }

        #region frmRemovePendingGroups_Load
        private void frmRemovePendingGroups_Load(object sender, EventArgs e)
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

        private void AddLoggerPendingGroups(string log)
        {
            this.Invoke(new MethodInvoker(delegate
            {
                lstLogPendingGroups.Items.Add(log);
                lstLogPendingGroups.SelectedIndex = lstLogPendingGroups.Items.Count - 1;
            }));
        }

        #endregion

        #region ScrapeEvent_addToLogger

        void GroupStatus_addToLogger(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eventArgs = e as EventsArgs;
                AddLoggerPendingGroups(eventArgs.log);
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
                        AddLoggerPendingGroups("[ " + DateTime.Now + " ] => [ Please Load LinkedIn Accounts From MyAccounts Menu ]");

                        frmAccounts FrmAccount = new frmAccounts();
                        FrmAccount.Show();
                        return;
                    }

                    lstLogPendingGroups.Items.Clear();
                    chkPendingGroup.Items.Clear();

                    cmbUserPendingGroup.Items.Clear();
                    LinkdInContacts.Clear();

                    AddLoggerPendingGroups("[ " + DateTime.Now + " ] => [ Starting Search for Pending Groups.. ]");

                    btnAddPendingGroups.Cursor = Cursors.AppStarting;

                    new Thread(() =>
                    {
                        LinkedInPendingGroupSearch();

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
                AddLoggerPendingGroups("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        } 
        #endregion

        #region LinkedInPendingGroupSearch()

        public int counter_GroupPendingSearch = 0;

        private void LinkedInPendingGroupSearch()
        {
            int numberofThreads = 5;

            try
            {
                counter_GroupPendingSearch = LinkedInManager.linkedInDictionary.Count;

                if (LinkedInManager.linkedInDictionary.Count > 0)
                {
                    ThreadPool.SetMaxThreads(numberofThreads, 5);
                    foreach (KeyValuePair<string, LinkedInMaster> item in LinkedInManager.linkedInDictionary)
                    {
                        ThreadPool.SetMaxThreads(numberofThreads, 5);
                        ThreadPool.QueueUserWorkItem(new WaitCallback(StartMultiThreadedPendingGroupAdd), new object[] { item });
                        Thread.Sleep(1000);
                    }
                }
                else
                {
                    MessageBox.Show("Please Load LinkedIn Accounts From MyAccounts Menu");
                    AddLoggerPendingGroups("[ " + DateTime.Now + " ] => [ Please Load LinkedIn Accounts From MyAccounts Menu ]");
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Pending Groups --> LinkedInGroupMemberSearch() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Pending Groups --> LinkedInGroupMemberSearch() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPendingGroupErrorLogs);
            }
        }

        #endregion

        #region StartMultiThreadedPendingGroupAdd

        public void StartMultiThreadedPendingGroupAdd(object parameter)
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
                    //string MemIdwithuser = dataScrape.GetSelectedID(ref HttpHelper, Login.accountUser);
                    //Dictionary<string, string> Groups = dataScrape.PostAddPendingGroupNames(ref HttpHelper, MemIdwithuser);
                    AddLoggerPendingGroups("[ " + DateTime.Now + " ] => [ Groups Adding of Acount + " + Login.accountUser + " please wait for sometime ]");
                    Dictionary<string, string> Groups  = dataScrape.GetSelectedIDForPendingGroups(ref HttpHelper, Login.accountUser);

                    LinkdInContacts.Add(Login.accountUser, Groups);

                    try
                    {
                        if (Groups.Count > 0)
                        {
                            AddLoggerPendingGroups("[ " + DateTime.Now + " ] => [ Pending Group List..Added Successfully..in " + Login.accountUser + " ]");
                        }
                        else
                        {
                            AddLoggerPendingGroups("[ " + DateTime.Now + " ] => [ Pending Group List..Not Available..in " + Login.accountUser + " ]");
                        }
                    }
                    catch (Exception ex)
                    {
                        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Pending Groups --> StartMultiThreadedPendingGroupAdd() >> 1 >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Pending Groups --> StartMultiThreadedPendingGroupAdd() >> 2 >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPendingGroupErrorLogs);
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
                    AddLoggerPendingGroups("[ " + DateTime.Now + " ] => [ LinkedIn Account : " + Login.accountUser + " has been temporarily restricted ]");
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Pending Groups --> StartMultiThreadedPendingGroupAdd() >> 2 >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Pending Groups --> StartMultiThreadedPendingGroupAdd() >> 2>>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPendingGroupErrorLogs);
            }
            finally
            {
                counter_GroupPendingSearch--;

                if (counter_GroupPendingSearch == 0)
                {
                    if (btnAddPendingGroups.InvokeRequired)
                    {
                        btnAddPendingGroups.Invoke(new MethodInvoker(delegate
                        {
                            btnAddPendingGroups.Enabled = true;
                            AddLoggerPendingGroups("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                            AddLoggerPendingGroups("------------------------------------------------------------------------------------------------------------------------------------");
                            btnAddPendingGroups.Cursor = Cursors.Default;
                        }));
                    }
                }
            }
        }

        #endregion

        #region cmbUserPendingGroup_SelectedIndexChanged
        private void cmbUserPendingGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            chkPendingGroup.Items.Clear();
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
                                chkPendingGroup.Items.Add(item1.Key.Replace(",", string.Empty));
                            }
                        }

                        AddLoggerPendingGroups("[ " + DateTime.Now + " ] => [ " + chkPendingGroup.Items.Count + " Pending Groups in User: " + GetUserID + " ]");
                        AddLoggerPendingGroups("[ " + DateTime.Now + " ] => [ Finished Adding Pending Groups of User: " + GetUserID + " ]");
                        AddLoggerPendingGroups("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                        AddLoggerPendingGroups("------------------------------------------------------------------------------------------------------------------------------------");
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Pending Groups --> cmbUserPendingGroup_SelectedIndexChanged() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Pending Groups --> cmbUserPendingGroup_SelectedIndexChanged() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPendingGroupErrorLogs);
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
                    for (int i = 0; i < chkPendingGroup.Items.Count; i++)
                    {
                        try
                        {
                            string aaa = Convert.ToString(chkPendingGroup.Items[i]);
                            chkPendingGroup.SetItemChecked(i, true);
                            lblTotGroupCount.Text = "(" + chkPendingGroup.Items.Count + ")";
                        }
                        catch { }
                    }
                }
                else
                {
                    for (int i = 0; i < chkPendingGroup.Items.Count; i++)
                    {
                        try
                        {
                            string aaa = Convert.ToString(chkPendingGroup.Items[i]);
                            chkPendingGroup.SetItemChecked(i, false);
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
                int count2 = chkPendingGroup.CheckedItems.Count + 1;
                if (chkPendingGroup.CheckOnClick == true)
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
        private List<string> lstPendingGroup = new List<string>();
        private void btnRemovePendingGroup_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {

                try
                {
                    lstPendingGroup.Clear();
                    lstRemovePendingGroupThread.Clear();

                    if (IsStop)
                    {
                        IsStop = false;
                    }

                    if (LinkedInManager.linkedInDictionary.Count() == 0)
                    {
                        return;
                    }
                    else if (chkPendingGroup.CheckedItems.Count == 0)
                    {
                        AddLoggerPendingGroups("[ " + DateTime.Now + " ] => [ Please select Atleast One Group.. ]");
                        MessageBox.Show("Please select Atleast One Group..");
                        return;
                    }

                    counter_GroupPendingSearch = 0;
                    AddLoggerPendingGroups("[ " + DateTime.Now + " ] => [ Process Running Please wait.. ]");
                    btnRemovePendingGroup.Cursor = Cursors.AppStarting;

                    if (chkPendingGroup.CheckedItems.Count > 0)
                    {
                        new Thread(() =>
                        {
                            LinkedInRemovePendingGroups();

                        }).Start();
                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Pending Groups --> btnRemovePendingGroup_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Pending Groups --> btnRemovePendingGroup_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPendingGroupErrorLogs);
                }
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddLoggerPendingGroups("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        } 
        #endregion

        #region LinkedInRemovePendingGroups
        public bool IsallaccChecked = false;
        int counter_PendGroupRemoved = 0;
        private void LinkedInRemovePendingGroups()
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

                                    ThreadPool.QueueUserWorkItem(new WaitCallback(PostRemovePendingGroups), new object[] { item });

                                    Thread.Sleep(1000);
                                }
                            }
                            else
                            {
                                if (IsallaccChecked)
                                {

                                    ThreadPool.SetMaxThreads(numberOfThreads, 5);

                                    ThreadPool.QueueUserWorkItem(new WaitCallback(PostRemovePendingGroups), new object[] { item });

                                    Thread.Sleep(1000);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Pending Groups --> LinkedInRemovePendingGroups() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Pending Groups --> LinkedInRemovePendingGroups() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPendingGroupErrorLogs);
                }
            }
            catch
            {
            }
        }

        #endregion

        #region PostRemovePendingGroups

        public void PostRemovePendingGroups(object parameter)
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
                        lstRemovePendingGroupThread.Add(Thread.CurrentThread);
                        lstRemovePendingGroupThread.Distinct().ToList();
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
                obj_GroupStatus.loggerRemPendingGroup.addToLogger += new EventHandler(GroupStatus_addToLogger);

                if (!Login.IsLoggedIn)
                {
                    Login.LoginHttpHelper(ref HttpHelper);
                }

                List<string> SelectedItem = new List<string>();

                try
                {
                    if (label1.Text == Login.accountUser)
                    {
                        #region old commented code
                        //if (IsallaccChecked)
                        //{
                        //    try
                        //    {

                        //        foreach (string SelectedGrp in chkPendingGroup.CheckedItems)
                        //        {
                        //            foreach (var itemID in LinkdInContacts)
                        //            {
                        //                if (SelectedGrp.Split(':')[0] == itemID.Key)
                        //                {
                        //                    if (label1.Text == itemID.Key)
                        //                    {
                        //                        foreach (KeyValuePair<string, string> itemGid in itemID.Value)
                        //                        {
                        //                            if (SelectedGrp.Split(':')[1] == itemGid.Key.Split(':')[1])
                        //                            {
                        //                                try
                        //                                {
                        //                                    lstPendingGroup.Add(itemGid.Key + ":" + itemGid.Value);
                        //                                }
                        //                                catch
                        //                                {
                        //                                }
                        //                            }
                        //                        }
                        //                    }

                        //                }
                        //            }
                        //        }


                        //        if (lstPendingGroup.Count > 0)
                        //        {
                        //            try
                        //            {
                        //                GroupStatus.lstPendingGroup = lstPendingGroup;
                        //            }
                        //            catch
                        //            {
                        //            }
                        //        }



                        //        int minDelay = 20;
                        //        int maxDelay = 25;

                        //        if (!string.IsNullOrEmpty(txtPendingGroupMinDelay.Text) && NumberHelper.ValidateNumber(txtPendingGroupMinDelay.Text))
                        //        {
                        //            minDelay = Convert.ToInt32(txtPendingGroupMinDelay.Text);
                        //        }
                        //        if (!string.IsNullOrEmpty(txtPendingGroupMaxDelay.Text) && NumberHelper.ValidateNumber(txtPendingGroupMaxDelay.Text))
                        //        {
                        //            maxDelay = Convert.ToInt32(txtPendingGroupMaxDelay.Text);
                        //        }

                        //        string MessagePosted = obj_GroupStatus.PostRemovePendingGroups(Login.accountUser, Login.accountPass, minDelay, maxDelay);
                        //        Thread.Sleep(2000);
                        //    }
                        //    catch
                        //    {
                        //    }
                        //}
                        //else
                        //{ 
                        #endregion

                            try
                            {

                                foreach (string SelectedGrp in chkPendingGroup.CheckedItems)
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
                                                            lstPendingGroup.Add(itemGid.Key + ":" + itemGid.Value);
                                                        }
                                                        catch
                                                        {
                                                        }
                                                    }
                                                }
                                        }
                                    }
                                }

                                if (lstPendingGroup.Count > 0)
                                {
                                    try
                                    {
                                        GroupStatus.lstPendingGroup = lstPendingGroup;
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
                                                                
                                string MessagePosted = obj_GroupStatus.PostRemovePendingGroups(Login.accountUser, Login.accountPass, minDelay, maxDelay);
                                Thread.Sleep(2000);
                            }
                            catch
                            {
                            }
                        //}
                    }
                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Pending Groups --> PostRemovePendingGroups() ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Pending Groups --> PostRemovePendingGroups() ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPendingGroupErrorLogs);
                }
                
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> PostAddGroups() ---2--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> PostAddGroups() ---2--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPendingGroupErrorLogs);
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
                            AddLoggerPendingGroups("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                            AddLoggerPendingGroups("------------------------------------------------------------------------------------------------------------------------------------");
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
                List<Thread> lstTemp = lstRemovePendingGroupThread.Distinct().ToList();
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
                AddLoggerPendingGroups("------------------------------------------------------------------------------------------------------------------------------------");
                AddLoggerPendingGroups("[ " + DateTime.Now + " ] => [ PROCESS STOPPED ]");
                AddLoggerPendingGroups("------------------------------------------------------------------------------------------------------------------------------------");
                btnRemovePendingGroup.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error >>> " + ex.StackTrace);
            }
        } 
        #endregion

        string MemGrp = string.Empty;
        string openGrp = string.Empty;
        private void chkMemberGroup_CheckedChanged(object sender, EventArgs e)
        {
            chkPendingGroup.Items.Clear();

            if (chkMemberGroup.Checked == true)
            {
                MemGrp = "true";
            }
            else
            {
                MemGrp = "false";
            }

            try
            {
                string GetUserID = cmbUserPendingGroup.SelectedItem.ToString();
                foreach (KeyValuePair<string, Dictionary<string, string>> item in LinkdInContacts)
                {
                    if (GetUserID.Contains(item.Key))
                    {
                        Dictionary<string, string> GmUserIDs = item.Value;

                        foreach (KeyValuePair<string, string> item1 in item.Value)
                        {
                            if (openGrp == "true" && MemGrp == "true")
                            {
                                AddLoggerPendingGroups("[ " + DateTime.Now + " ] => [ " + item1.Key + " ]");
                                chkPendingGroup.Items.Add(item1.Key);
                            }
                            else if (openGrp == "true" && MemGrp == "false")
                            {
                                if (item1.Key.Contains("Open Group"))
                                {
                                    AddLoggerPendingGroups("[ " + DateTime.Now + " ] => [ " + item1.Key + " ]");
                                    chkPendingGroup.Items.Add(item1.Key);
                                }
                            }
                            else if (openGrp == "false" && MemGrp == "true")
                            {
                                if (item1.Key.Contains("Pending"))
                                {
                                    AddLoggerPendingGroups("[ " + DateTime.Now + " ] => [ " + item1.Key + " ]");
                                    chkPendingGroup.Items.Add(item1.Key);
                                }
                            }
                            else if (openGrp == "false" && MemGrp == "false")
                            {
                            }
                        }
                    }
                }

                if (chkPendingGroup.Items.Count == 0)
                {
                    chkSelectPendingGrp.Enabled = false;
                }
                else
                {
                    chkSelectPendingGrp.Enabled = true;
                }
                                
                AddLoggerPendingGroups("[ " + DateTime.Now + " ] => [ Finished Finding Pending Groups of Selected User ]");
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Search Groups --> chkOpenGroup_CheckedChanged() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Search Groups --> chkOpenGroup_CheckedChanged() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddSearchGroupErrorLogs);
            }
        }

        private void chkOpenGroup_CheckedChanged(object sender, EventArgs e)
        {
            chkPendingGroup.Items.Clear();
            if (chkOpenGroup.Checked == true)
            {
                openGrp = "true";
            }
            else
            {
                openGrp = "false";
            }

            try
            {
                string GetUserID = cmbUserPendingGroup.SelectedItem.ToString();
                foreach (KeyValuePair<string, Dictionary<string, string>> item in LinkdInContacts)
                {
                    if (GetUserID.Contains(item.Key))
                    {
                        Dictionary<string, string> GmUserIDs = item.Value;

                        foreach (KeyValuePair<string, string> item1 in item.Value)
                        {
                            if (openGrp == "true" && MemGrp == "true")
                            {
                                AddLoggerPendingGroups("[ " + DateTime.Now + " ] => [ " + item1.Key + " ]");
                                chkPendingGroup.Items.Add(item1.Key);
                            }
                            else if (openGrp == "true" && MemGrp == "false")
                            {
                                if (item1.Key.Contains("Open Group"))
                                {
                                    AddLoggerPendingGroups("[ " + DateTime.Now + " ] => [ " + item1.Key + " ]");
                                    chkPendingGroup.Items.Add(item1.Key);
                                }
                            }
                            else if (openGrp == "false" && MemGrp == "true")
                            {
                                if (item1.Key.Contains("Member Only Group"))
                                {
                                    AddLoggerPendingGroups("[ " + DateTime.Now + " ] => [ " + item1.Key + " ]");
                                    chkPendingGroup.Items.Add(item1.Key);
                                }
                            }
                            else if (openGrp == "false" && MemGrp == "false")
                            {
                            }
                        }
                    }
                }

                if (chkPendingGroup.Items.Count == 0)
                {
                    chkSelectPendingGrp.Enabled = false;
                }
                else
                {
                    chkSelectPendingGrp.Enabled = true;
                }

              
                AddLoggerPendingGroups("[ " + DateTime.Now + " ] => [ Finished Finding Group of Selected User ]");
                AddLoggerPendingGroups("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                AddLoggerPendingGroups("------------------------------------------------------------------------------------------------------------------------------------");
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Search Groups --> chkOpenGroup_CheckedChanged() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Search Groups --> chkOpenGroup_CheckedChanged() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddSearchGroupErrorLogs);
            }
        }



    }
}
