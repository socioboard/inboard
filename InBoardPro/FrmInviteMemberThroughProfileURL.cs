using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using InBoardPro;
using System.Drawing.Drawing2D;
using ManageConnections;
using BaseLib;
namespace InBoardPro
{
    public partial class FrmInviteMemberThroughProfileURL : Form
    {
        #region variable declaration
        bool IsStop = false;
        bool CheckNetConn = false;
        bool isDivideURLsAmongAccounts = false;
        public System.Drawing.Image image;
        List<Thread> lstThread_AcceptInvitation = new List<Thread>();
        List<string> lstProfileURL = new List<string>();
        readonly object lockr_ThreadController = new object();
        int count_ThreadController = 0; 
      //   public static bool spintaxsearch = false;
     //   public static List<string> Listgreetmsg = new List<string>();
        #endregion

        #region FrmInviteMemberThroughProfileURL
        public FrmInviteMemberThroughProfileURL()
        {
            InitializeComponent();
        } 
        #endregion

        #region AcceptInvitationsLogEvents_addToLogger
        void AcceptInvitationsLogEvents_addToLogger(object sender, EventArgs e)
        {
           if (e is EventsArgs)
            {
                EventsArgs eventArgs = e as EventsArgs;
                AddInviteMemberThroughProfUrlLogger(eventArgs.log);
            }
        } 
        #endregion

        #region AddAcceptInvitationsLogger
        public void AddInviteMemberThroughProfUrlLogger(string log)
        {
            try
            {
                if (LstBoxLogger.Items.Count > 1000)
                {
                    LstBoxLogger.Invoke(new MethodInvoker(delegate
                    {
                        LstBoxLogger.Items.Clear();
                    }));
                }
                if (LstBoxLogger.InvokeRequired)
                {
                    LstBoxLogger.Invoke(new MethodInvoker(delegate
                    {
                        LstBoxLogger.Items.Add(log);
                        LstBoxLogger.SelectedIndex = LstBoxLogger.Items.Count - 1;
                    }));
                }
                else
                {
                    LstBoxLogger.Items.Add(log);
                    LstBoxLogger.SelectedIndex = LstBoxLogger.Items.Count - 1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error >>> " + ex.StackTrace);
            }
        } 
        #endregion

        #region BtnSendInvitation_Click
        private void BtnSendInvitation_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {

                try
                {
                    IsStop = false;
                    lstThread_AcceptInvitation.Clear();

                    if (string.IsNullOrEmpty(TxtProfileNote.Text))
                    {
                        AddInviteMemberThroughProfUrlLogger("[ " + DateTime.Now + " ] => [ Please Fill Personal Note ! ]");
                    }
                    else
                    {
                        ClsInviteMemberThroughProfileURL.PersonalNote = TxtProfileNote.Text;
                    }
                    string ddlSelectedItem = DdlHowDoYouKnowThisPersion.SelectedItem.ToString();

                    if (string.IsNullOrEmpty(ddlSelectedItem))
                    {
                        AddInviteMemberThroughProfUrlLogger("[ " + DateTime.Now + " ] => [ Please Select How Do You Know This Persion ! ]");
                        MessageBox.Show("Please Select How Do You Know This Persion !");
                        return;
                    }
                    else
                    {
                        ClsInviteMemberThroughProfileURL.HowDoYouKnowThisPerson = ddlSelectedItem;
                    }

                    if (TxtProfileNote.Text.Contains("|"))
                    {
                        if (chkSpinTaxComposeMsg.Checked)
                        {
                            ClsInviteMemberThroughProfileURL.spintaxsearch = true;
                        }
                        if (!(chkSpinTaxComposeMsg.Checked))
                        {
                            AddInviteMemberThroughProfUrlLogger("[ " + DateTime.Now + " ] => [ Please Check SpinTax CheckBox.. ]");
                            MessageBox.Show("Please Check SpinTax CheckBox..");
                            return;
                        }

                        if (TxtProfileNote.Text.Contains("{") || TxtProfileNote.Text.Contains("}"))
                        {
                            AddInviteMemberThroughProfUrlLogger("[ " + DateTime.Now + " ] => [ Its a wrong SpinTax Format.. ]");
                            MessageBox.Show("Its a wrong SpinTax Format..");
                            return;
                        }
                    }

                    if (lstProfileURL.Count < 1)
                    {
                        AddInviteMemberThroughProfUrlLogger("[ " + DateTime.Now + " ] => [ Please Upload Profile URL ! ]");
                        MessageBox.Show("Please Upload Profile URL !");
                        return;
                    }
                    else
                    {
                        ClsInviteMemberThroughProfileURL.LstProfileURL = lstProfileURL;
                    }

                    if (NumberHelper.ValidateNumber(TxtMinDelay.Text.Trim()))
                    {
                        ClsInviteMemberThroughProfileURL.MinDelay = Convert.ToInt32(TxtMinDelay.Text.Trim());

                    }
                    else
                    {
                        AddInviteMemberThroughProfUrlLogger("[ " + DateTime.Now + " ] => [ Please Fill Integer value in TextBox(Dealy) ! ]");

                        TxtMinDelay.Text = "20";
                        ClsInviteMemberThroughProfileURL.MinDelay = 20;
                    }

                    if (NumberHelper.ValidateNumber(TxtMaxDelay.Text.Trim()))
                    {
                        ClsInviteMemberThroughProfileURL.MaxDelay = Convert.ToInt32(TxtMaxDelay.Text.Trim());

                    }
                    else
                    {
                        AddInviteMemberThroughProfUrlLogger("[ " + DateTime.Now + " ] => [ Please Fill Integer value in TextBox(Dealy) ! ]");

                        TxtMaxDelay.Text = "30";
                        ClsInviteMemberThroughProfileURL.MaxDelay = 30;
                    }

                    if (ChkDivURLsAccordingAcc.Checked)
                    {
                        isDivideURLsAmongAccounts = true;
                    }

                    BtnSendInvitation.Cursor = Cursors.AppStarting;

                    Thread acceptInvitation_Thread = new Thread(StartSendInvitation);
                    acceptInvitation_Thread.Start();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error >>> " + ex.StackTrace);
                }
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddInviteMemberThroughProfUrlLogger("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        } 
        #endregion

        #region StartSendInvitation
        private void StartSendInvitation()
        {
            try
            {
                int NoofThread = 5;
                if (NumberHelper.ValidateNumber(TxtNoOfThreads.Text.Trim()))
                {
                    NoofThread = Convert.ToInt32(TxtNoOfThreads.Text.Trim());
                    ThreadPool.SetMaxThreads(NoofThread, 5);
                }
                else
                {
                    AddInviteMemberThroughProfUrlLogger("[ " + DateTime.Now + " ] => [ Please Fill Integer value in TextBox(No. of Threads) ]");
                    TxtNoOfThreads.Text = "5";

                }

                if (isDivideURLsAmongAccounts && LinkedInManager.linkedInDictionary.Count > 0)
                {
                    List<List<string>> list_listTargetURLs = new List<List<string>>();

                    List<List<string>> list_listAccounts = new List<List<string>>();

                    try
                    {
                        list_listAccounts = ListUtilities.Split(Globals.listAccounts, NoofThread);
                    }
                    catch { }

                    #region Divide URLs among accounts

                    #region User Divude Checked
                    if (isDivideURLsAmongAccounts)
                    {
                        int splitNo = 0;
                        splitNo = lstProfileURL.Count / Globals.listAccounts.Count;

                        //int remainder = grouplist.Count % Globals.listAccounts.Count;
                        //if (remainder != 0 && splitNo != 0)
                        //{
                        //    splitNo = splitNo + 1;
                        //}

                        //else if (rdBtnDivideByGivenNo.Checked)
                        //{
                        //    if (!string.IsNullOrEmpty(txtScrapeNoOfUsers.Text) && NumberHelper.ValidateNumber(txtScrapeNoOfUsers.Text))
                        //    {
                        //        int res = Convert.ToInt32(txtScrapeNoOfUsers.Text);
                        //        splitNo = res;//listUserIDs.Count / res;
                        //    }
                        //}
                        if (splitNo == 0)
                        {
                            splitNo = Globals.listAccounts.Count; //RandomNumberGenerator.GenerateRandom(0, grouplist.Count - 1);
                        }
                        list_listTargetURLs = ListUtilities.Split(lstProfileURL, splitNo);
                    }


                    #endregion

                    #endregion


                    int index = 0;
                    while (index <= list_listTargetURLs.Count - 1)
                    {

                        foreach (List<string> listAccounts in list_listAccounts)
                        {
                            if (index > list_listTargetURLs.Count - 1)
                            {
                                break;
                            }
                            //int tempCounterAccounts = 0;
                            try
                            {
                                foreach (string account in listAccounts)
                                {
                                    try
                                    {
                                        if (index > list_listTargetURLs.Count - 1)
                                        {
                                            break;
                                        }

                                        List<string> listGrpURLs_PerAccount = new List<string>();
                                        listGrpURLs_PerAccount = list_listTargetURLs[index];

                                        lock (lockr_ThreadController)
                                        {
                                            try
                                            {
                                                if (count_ThreadController >= listAccounts.Count)
                                                {
                                                    Monitor.Wait(lockr_ThreadController);
                                                }


                                                string acc = account.Remove(account.IndexOf(':'));

                                                //Run a separate thread for each account

                                                LinkedInMaster item = null;
                                                LinkedInManager.linkedInDictionary.TryGetValue(acc, out item);

                                                if (item != null)
                                                {
                                                    Thread likerThread = null;

                                                    // else
                                                    {
                                                        //Send only 1 request on a url
                                                        //if ane account has sent request, no other account shud send more request
                                                        likerThread = new Thread(StartSendInvitationMultyThread_DivideUrlsAccordingAccount);

                                                        likerThread.Name = "workerThread_Requestsent_" + acc;
                                                        likerThread.IsBackground = true;

                                                        likerThread.Start(new object[] { item, listGrpURLs_PerAccount });
                                                        index++;
                                                    }

                                                    //likerThread.Name = "workerThread_Requestsent_" + acc;
                                                    //likerThread.IsBackground = true;

                                                    //try
                                                    //{
                                                    //    dictionary_GroupJoiningThreads.Add(likerThread.Name, likerThread);
                                                    //}
                                                    //catch { }

                                                    //likerThread.Start(new object[] { item, queueGrpurl });

                                                    count_ThreadController++;
                                                    //tempCounterAccounts++; 
                                                }
                                            }
                                            catch { }
                                        }
                                    }
                                    catch { }
                                }
                            }
                            catch { }

                        }

                    }
                }
                else
                {
                    if (LinkedInManager.linkedInDictionary.Count > 0)
                    {

                        try
                        {
                            foreach (KeyValuePair<string, LinkedInMaster> item in LinkedInManager.linkedInDictionary)
                            {
                                ThreadPool.SetMaxThreads(NoofThread, 5);

                                ThreadPool.QueueUserWorkItem(new WaitCallback(StartSendInvitationMultyThread), new object[] { item });
                                //StartSendInvitationMultyThread(new object[] { item });

                                // Thread.Sleep(1000);
                            }
                        }
                        catch (Exception ex)
                        {
                            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Accept Invitation --> StartAcceptInvitation() ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Accept Invitation --> StartAcceptInvitation() ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddConnectionErrorLogs);
                        }
                    }
                    else
                    {
                        AddInviteMemberThroughProfUrlLogger("[ " + DateTime.Now + " ] => [ Please Load Account ! ]");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error >>> " + ex.StackTrace);
            }
        } 
        #endregion

        #region StartSendInvitationMultyThread
        private void StartSendInvitationMultyThread(object Parameter)
        {

            string Account = string.Empty;
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
                        lstThread_AcceptInvitation.Add(Thread.CurrentThread);
                        lstThread_AcceptInvitation = lstThread_AcceptInvitation.Distinct().ToList();
                        Thread.CurrentThread.IsBackground = true;
                    }
                }
                catch
                {
                }

                Array paramsArray = new object[1];

                paramsArray = (Array)Parameter;

                KeyValuePair<string, LinkedInMaster> item = (KeyValuePair<string, LinkedInMaster>)paramsArray.GetValue(0);

                GlobusHttpHelper HttpHelper = new GlobusHttpHelper();
                LinkedinLogin Login = new LinkedinLogin();

                LinkedInMaster LinkedIn_Master = item.Value;
                string linkedInKey = item.Key;
                Account = item.Key;

                Login.accountUser = item.Key;
                Login.accountPass = item.Value._Password;
                Login.proxyAddress = item.Value._ProxyAddress;
                Login.proxyPort = item.Value._ProxyPort;
                Login.proxyUserName = item.Value._ProxyUsername;
                Login.proxyPassword = item.Value._ProxyPassword;

                ClsInviteMemberThroughProfileURL obj_ClsInviteMemberThroughProfileURL = new ClsInviteMemberThroughProfileURL(Login.accountUser, Login.accountPass, Login.proxyAddress, Login.proxyPort, Login.proxyUserName, Login.proxyPassword);

                // Add Event
                Login.logger.addToLogger += new EventHandler(AcceptInvitationsLogEvents_addToLogger);
                obj_ClsInviteMemberThroughProfileURL.acceptInvitationsLogEvents.addToLogger += new EventHandler(AcceptInvitationsLogEvents_addToLogger);

                if (!Login.IsLoggedIn)
                {
                    Login.LoginHttpHelper(ref HttpHelper);
                }

                if (Login.IsLoggedIn)
                {
                    obj_ClsInviteMemberThroughProfileURL.StartSendInvitations(ref HttpHelper);
                }
                else
                {
                    AddInviteMemberThroughProfUrlLogger("[ " + DateTime.Now + " ] => [ Couldn't Login With Account " + item.Key + " ]");
                }

                // Remove Event
                Login.logger.addToLogger -= new EventHandler(AcceptInvitationsLogEvents_addToLogger);
                obj_ClsInviteMemberThroughProfileURL.acceptInvitationsLogEvents.addToLogger -= new EventHandler(AcceptInvitationsLogEvents_addToLogger);


            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Accept Invitation --> StartAcceptInvitation() ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Accept Invitation --> StartAcceptInvitation() ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddConnectionErrorLogs);
            }

            finally
            {
                BtnSendInvitation.Invoke(new MethodInvoker(delegate
                    {
                        BtnSendInvitation.Cursor = Cursors.Default;
                    }));               
            }
        } 
        #endregion

        #region StartSendInvitationMultyThread_DivideUrlsAccordingAccount
        private void StartSendInvitationMultyThread_DivideUrlsAccordingAccount(object Parameter)
        {
            string Account = string.Empty;
            List<string> lstProfileUrl = new List<string>();

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
                        lstThread_AcceptInvitation.Add(Thread.CurrentThread);
                        lstThread_AcceptInvitation = lstThread_AcceptInvitation.Distinct().ToList();
                        Thread.CurrentThread.IsBackground = true;
                    }
                }
                catch
                {
                }

                Array paramsArray = new object[2];

                paramsArray = (Array)Parameter;

                // KeyValuePair<string, LinkedInMaster> item = (KeyValuePair<string, LinkedInMaster>)paramsArray.GetValue(0);

                lstProfileUrl = (List<string>)paramsArray.GetValue(1);

                GlobusHttpHelper HttpHelper = new GlobusHttpHelper();
                LinkedinLogin Login = new LinkedinLogin();

                LinkedInMaster item = (LinkedInMaster)paramsArray.GetValue(0); //item.Value;
                //string linkedInKey = item.Key;
                //Account = item.Key;

                //Login.accountUser = item.Key;
                //Login.accountPass = item.Value._Password;
                //Login.proxyAddress = item.Value._ProxyAddress;
                //Login.proxyPort = item.Value._ProxyPort;
                //Login.proxyUserName = item.Value._ProxyUsername;
                //Login.proxyPassword = item.Value._ProxyPassword;

                Login.accountUser = item._Username;
                Login.accountPass = item._Password;
                Login.proxyAddress = item._ProxyAddress;
                Login.proxyPort = item._ProxyPort;
                Login.proxyUserName = item._ProxyUsername;
                Login.proxyPassword = item._ProxyPassword;

                ClsInviteMemberThroughProfileURL obj_ClsInviteMemberThroughProfileURL = new ClsInviteMemberThroughProfileURL(Login.accountUser, Login.accountPass, Login.proxyAddress, Login.proxyPort, Login.proxyUserName, Login.proxyPassword);

                // Add Event
                Login.logger.addToLogger += new EventHandler(AcceptInvitationsLogEvents_addToLogger);
                obj_ClsInviteMemberThroughProfileURL.acceptInvitationsLogEvents.addToLogger += new EventHandler(AcceptInvitationsLogEvents_addToLogger);

                if (!Login.IsLoggedIn)
                {
                    // AddAcceptInvitationsLogger("Logging In With Account" + Login.accountUser);

                    Login.LoginHttpHelper(ref HttpHelper);
                }

                if (Login.IsLoggedIn)
                {
                    obj_ClsInviteMemberThroughProfileURL.StartSendInvitations_DivideUrlsAccordingAccount(ref HttpHelper, lstProfileUrl);
                }
                else
                {
                    AddInviteMemberThroughProfUrlLogger("[ " + DateTime.Now + " ] => [ Couldn't Login With Account " + Login.accountUser + " ]");
                }

                // Remove Event
                Login.logger.addToLogger -= new EventHandler(AcceptInvitationsLogEvents_addToLogger);
                obj_ClsInviteMemberThroughProfileURL.acceptInvitationsLogEvents.addToLogger -= new EventHandler(AcceptInvitationsLogEvents_addToLogger);


            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Accept Invitation --> StartAcceptInvitation() ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Accept Invitation --> StartAcceptInvitation() ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddConnectionErrorLogs);
            }

            finally
            {
                lock (lockr_ThreadController)
                {
                    count_ThreadController--;
                    Monitor.Pulse(lockr_ThreadController);
                }
            }
        } 
        #endregion

        #region BtnStop_Click
        private void BtnStop_Click(object sender, EventArgs e)
        {
            try
            {
                IsStop = true;
                List<Thread> lstTemp = lstThread_AcceptInvitation.Distinct().ToList();
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
                AddInviteMemberThroughProfUrlLogger("-------------------------------------------------------------------------------------------------------------------------------");
                AddInviteMemberThroughProfUrlLogger("[ " + DateTime.Now + " ] => [ PROCESS STOPPED ]");
                AddInviteMemberThroughProfUrlLogger("-------------------------------------------------------------------------------------------------------------------------------");
                BtnSendInvitation.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error >>> " + ex.StackTrace);

            }
        } 
        #endregion

        #region BtnUploadProfileURL_Click
        private void BtnUploadProfileURL_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "Text Files (*.txt)|*.txt";
                    ofd.InitialDirectory = Application.StartupPath;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        TxtProfileURL.Text = ofd.FileName;


                        lstProfileURL.Clear();

                        List<string> templist = GlobusFileHelper.ReadFile(ofd.FileName);
                        foreach (string item in templist)
                        {
                            try
                            {
                                string newItem = item.Replace(" ", "").Replace("\t", "");
                                if (!lstProfileURL.Contains(item) && !string.IsNullOrEmpty(newItem))
                                {
                                    lstProfileURL.Add(item);
                                }
                            }
                            catch (Exception)
                            {

                            }
                        }

                        lstProfileURL = lstProfileURL.Distinct().ToList();
                        AddInviteMemberThroughProfUrlLogger("[ " + DateTime.Now + " ] => [ " + lstProfileURL.Count + "  Number Of Profile URL Added ! ]");
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Account Creator --> btnfname_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Account Creator --> btnfname_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAccountCraetionErrorLogs);
            }
        } 
        #endregion

        #region FrmInviteMemberThroughProfileURL_Paint
        private void FrmInviteMemberThroughProfileURL_Paint(object sender, PaintEventArgs e)
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
                Console.WriteLine("Error >>> " + ex.StackTrace);
            }
        } 
        #endregion

        #region FrmInviteMemberThroughProfileURL_Load
        private void FrmInviteMemberThroughProfileURL_Load(object sender, EventArgs e)
        {
            try
            {
                image = Properties.Resources.background;
                DdlHowDoYouKnowThisPersion.SelectedItem = DdlHowDoYouKnowThisPersion.Items[3].ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error >>> " + ex.StackTrace);
            }
        } 
        #endregion

        #region TxtProfileURL_KeyDown
        private void TxtProfileURL_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                e.SuppressKeyPress = true;
            }
            catch (Exception ex)
            {
            }
        } 
        #endregion

        #region DdlHowDoYouKnowThisPersion_KeyDown
        private void DdlHowDoYouKnowThisPersion_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                e.SuppressKeyPress = true;
            }
            catch (Exception ex)
            {
            }
        } 
        #endregion

        private void chkSpinTaxComposeMsg_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
