using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using InBoardPro;
using System.Threading;
using System.Drawing.Drawing2D;
using Groups;
using BaseLib;

namespace InBoardPro
{
    public partial class FrmScrapGroupMember : Form
    {
        #region global declaration
        bool IsStop = false;
        bool CheckNetConn = false;
        public static bool ChangeToNextAccount = false;
        List<Thread> lstScrapGrpMemberThread = new List<Thread>();
        public List<string> lstGroupURL = new List<string>();
        public System.Drawing.Image image; 
        #endregion

        #region FrmScrapGroupMember
        public FrmScrapGroupMember()
        {
            InitializeComponent();
        } 
        #endregion

        #region ScrapGroupMemberLogEvents_addToLogger
        void ScrapGroupMemberLogEvents_addToLogger(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eventArgs = e as EventsArgs;

                AddScrapGroupMemberLogger(eventArgs.log);
            }
        } 
        #endregion

        #region AddScrapGroupMemberLogger
        public void AddScrapGroupMemberLogger(string log)
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

        #region FrmScrapGroupMember_Load
        private void FrmScrapGroupMember_Load(object sender, EventArgs e)
        {
            try
            {
                image = Properties.Resources.background;
                GroupStatus.logger.addToLogger += new EventHandler(ScrapGroupMemberLogEvents_addToLogger);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error >>> " + ex.StackTrace);
            }
        } 
        #endregion

        #region BtnUploadGroupURLScrapGroupMember_Click
        private void BtnUploadGroupURLScrapGroupMember_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "Text Files (*.txt)|*.txt";
                    ofd.InitialDirectory = Application.StartupPath;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        TxtGroupURL_ScrapGroupMember.Text = ofd.FileName;


                        lstGroupURL.Clear();

                        List<string> templist = GlobusFileHelper.ReadFile(ofd.FileName).Distinct().ToList();
                        foreach (string item in templist)
                        {
                            try
                            {
                                string newItem = item.Replace(" ", "").Replace("\t", "");
                                if (!lstGroupURL.Contains(item) && !string.IsNullOrEmpty(newItem))
                                {
                                    lstGroupURL.Add(item);
                                }
                            }
                            catch (Exception)
                            {

                            }
                        }

                        lstGroupURL = lstGroupURL.Distinct().ToList();
                        AddScrapGroupMemberLogger("[ " + DateTime.Now + " ] => [ " + lstGroupURL.Count + "  Number Of Group URL Added ! ]");
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

        #region btnSearchScrapGroupMember_Click
        private void btnSearchScrapGroupMember_Click(object sender, EventArgs e)
        {
             CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {

                try
                {
                    IsStop = false;
                    lstScrapGrpMemberThread.Clear();

                    if (lstGroupURL.Count > 0)
                    {
                        ClsScrapGroupMember.LstGroupURL = lstGroupURL.Distinct().ToList();
                    }
                    else
                    {
                        AddScrapGroupMemberLogger("[ " + DateTime.Now + " ] => [ Please Upload Group URLs ! ]");
                        MessageBox.Show("Please Upload Group URLs !");
                    }

                    btnSearchScrapGroupMember.Cursor = Cursors.AppStarting;

                    Thread acceptInvitation_Thread = new Thread(StartScrapGroupMember);
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
                AddScrapGroupMemberLogger("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        } 
        #endregion

        #region StartScrapGroupMember
        int counter = 0;
        public void StartScrapGroupMember()
        {
            try
            {
                int NoofThread = 5;
                counter = LinkedInManager.linkedInDictionary.Count();

                if (NumberHelper.ValidateNumber(TxtNoOfThreads.Text.Trim()))
                {
                    NoofThread = Convert.ToInt32(TxtNoOfThreads.Text.Trim());
                    ThreadPool.SetMaxThreads(NoofThread, 5);
                }
                else
                {
                    AddScrapGroupMemberLogger("Please Fill Integer value in TextBox(No. of Threads) ");
                    NoofThread = 0;
                    TxtNoOfThreads.Text = "5";
                    return;

                }

                if (LinkedInManager.linkedInDictionary.Count > 0)
                {

                    try
                    {
                        foreach (KeyValuePair<string, LinkedInMaster> item in LinkedInManager.linkedInDictionary)
                        {
                            ThreadPool.SetMaxThreads(NoofThread, 5);

                            //ThreadPool.QueueUserWorkItem(new WaitCallback(StartScrapGrpMemberMultiThread), new object[] { item });
                            StartScrapGrpMemberMultiThread(new object[] { item });

                            Thread.Sleep(1000);
                            if (!ChangeToNextAccount)
                            {
                                break;
                            }
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
                    AddScrapGroupMemberLogger("[ " + DateTime.Now + " ] => [ Please Load Account ! ]");
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error >>> " + ex.StackTrace);
            }
        } 
        #endregion

        #region StartScrapGrpMemberMultiThread
        public void StartScrapGrpMemberMultiThread(object parameter)
        {
            try
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
                            lstScrapGrpMemberThread.Add(Thread.CurrentThread);
                            lstScrapGrpMemberThread = lstScrapGrpMemberThread.Distinct().ToList();
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

                    LinkedInMaster LinkedIn_Master = item.Value;
                    string linkedInKey = item.Key;
                    Account = item.Key;

                    Login.accountUser = item.Key;
                    Login.accountPass = item.Value._Password;
                    Login.proxyAddress = item.Value._ProxyAddress;
                    Login.proxyPort = item.Value._ProxyPort;
                    Login.proxyUserName = item.Value._ProxyUsername;
                    Login.proxyPassword = item.Value._ProxyPassword;

                    ClsScrapGroupMember obj_ClsScrapGroupMember = new ClsScrapGroupMember(Login.accountUser, Login.accountPass, Login.proxyAddress, Login.proxyPort, Login.proxyUserName, Login.proxyPassword);

                    // Add Event
                    Login.logger.addToLogger += new EventHandler(ScrapGroupMemberLogEvents_addToLogger);
                    obj_ClsScrapGroupMember.scrapGroupMemberLogEvents.addToLogger += new EventHandler(ScrapGroupMemberLogEvents_addToLogger);

                   
                    if (!Login.IsLoggedIn)
                    {
                        Login.LoginHttpHelper(ref HttpHelper);
                    }

                    if (Login.IsLoggedIn)
                    {
                        obj_ClsScrapGroupMember.GetGroupMember(ref HttpHelper);
                    }
                    else
                    {
                        AddScrapGroupMemberLogger("[ " + DateTime.Now + " ] => [ Couldn't Login With Account " + item.Key + " ]");
                    }

                    // Remove Event
                    Login.logger.addToLogger -= new EventHandler(ScrapGroupMemberLogEvents_addToLogger);
                    obj_ClsScrapGroupMember.scrapGroupMemberLogEvents.addToLogger -= new EventHandler(ScrapGroupMemberLogEvents_addToLogger);
                    

                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Accept Invitation --> StartAcceptInvitation() ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Accept Invitation --> StartAcceptInvitation() ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddConnectionErrorLogs);
                }
                finally
                {
                    //GroupStatus.logger.addToLogger -= new EventHandler(ScrapGroupMemberLogEvents_addToLogger);
                    counter--;
                  
                    if (counter == 0)
                    {
                        btnSearchScrapGroupMember.Invoke(new MethodInvoker(delegate
                        {
                            AddScrapGroupMemberLogger("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                            AddScrapGroupMemberLogger("----------------------------------------------------------------------------------------------------------------------------------------");
                            btnSearchScrapGroupMember.Cursor = Cursors.Default;
                        }));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error >>> " + ex.StackTrace);
            }
        } 
        #endregion

        #region FrmScrapGroupMember_Paint
        private void FrmScrapGroupMember_Paint(object sender, PaintEventArgs e)
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

        #region btnStopScrapGroupMember_Click
        private void btnStopScrapGroupMember_Click(object sender, EventArgs e)
        {
            try
            {
                IsStop = true;
                List<Thread> lstTemp = lstScrapGrpMemberThread.Distinct().ToList();
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
                AddScrapGroupMemberLogger("-------------------------------------------------------------------------------------------------------------------------------");
                AddScrapGroupMemberLogger("[ " + DateTime.Now + " ] => [ PROCESS STOPPED ]");
                AddScrapGroupMemberLogger("-------------------------------------------------------------------------------------------------------------------------------");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error >>> " + ex.StackTrace);

            }
        } 
        #endregion

    }
}
