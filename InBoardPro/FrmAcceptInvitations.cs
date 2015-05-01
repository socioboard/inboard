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
using ManageConnections;
using BaseLib;

namespace InBoardPro
{
    public partial class FrmAcceptInvitations : Form
    {
        bool IsStop = false;
        bool CheckNetConn = false;
        public System.Drawing.Image image;
        
        List<Thread> lstThread_AcceptInvitation = new List<Thread>();
        int counter_AcceptInvitation;

        public FrmAcceptInvitations()
        {
            InitializeComponent();
        }

        void AcceptInvitationsLogEvents_addToLogger(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            if (e is EventsArgs)
            {
                EventsArgs eventArgs = e as EventsArgs;

                AddAcceptInvitationsLogger(eventArgs.log);
            }
        }

        public void AddAcceptInvitationsLogger(string log)
        {
            try
            {
                if (LstLogger_AcceptInvitations.Items.Count > 1000)
                {


                    LstLogger_AcceptInvitations.Invoke(new MethodInvoker(delegate
                    {
                        LstLogger_AcceptInvitations.Items.Clear();
                    }));
                }
                if (LstLogger_AcceptInvitations.InvokeRequired)
                {
                    LstLogger_AcceptInvitations.Invoke(new MethodInvoker(delegate
                    {
                        LstLogger_AcceptInvitations.Items.Add(log);
                        LstLogger_AcceptInvitations.SelectedIndex = LstLogger_AcceptInvitations.Items.Count - 1;
                    }));
                }
                else
                {
                    LstLogger_AcceptInvitations.Items.Add(log);
                    LstLogger_AcceptInvitations.SelectedIndex = LstLogger_AcceptInvitations.Items.Count - 1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error >>> " + ex.StackTrace);
            }
        }

        private void BtnAccectInvitation_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {

                try
                {
                    IsStop = false;
                    lstThread_AcceptInvitation.Clear();
                    counter_AcceptInvitation = 0;
                    BtnAccectInvitation.Cursor = Cursors.AppStarting;

                    Thread acceptInvitation_Thread = new Thread(StartAcceptInvitation);
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
                AddAcceptInvitationsLogger("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        }

        private void StartAcceptInvitation()
        {
            try
            {
                int NoofThread = 5;
                if (NumberHelper.ValidateNumber(textNoOfThread.Text.Trim()))
                {
                    NoofThread = int.Parse(textNoOfThread.Text.Trim());
                    if (NoofThread == 0)
                    {
                        AddAcceptInvitationsLogger("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                        AddAcceptInvitationsLogger("-------------------------------------------------------------------------------------------------------------------");
                        BtnAccectInvitation.Cursor = Cursors.Default;
                        return;
                    }
                    ThreadPool.SetMaxThreads(NoofThread, 5);
                }
                else
                {
                    AddAcceptInvitationsLogger("[ " + DateTime.Now + " ] => [ Please Fill Integer value in TextBox(No. of Threads) ]");
                    return;
                    textNoOfThread.Text = "5";
                    
                }

                counter_AcceptInvitation = LinkedInManager.linkedInDictionary.Count;

                if (LinkedInManager.linkedInDictionary.Count > 0)
                {
                    
                    try
                    {
                        foreach (KeyValuePair<string, LinkedInMaster> item in LinkedInManager.linkedInDictionary)
                        {
                            ThreadPool.SetMaxThreads(NoofThread, 5);

                            ThreadPool.QueueUserWorkItem(new WaitCallback(StartAcceptInvitationMultiThread), new object[] { item });
                           // StartAcceptInvitationMultiThread  ( new object[] { item });

                            Thread.Sleep(1000);
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
                    AddAcceptInvitationsLogger("[ " + DateTime.Now + " ] => [ Please Load Account ! ]");
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error >>> " + ex.StackTrace);
            }
        }

        private void StartAcceptInvitationMultiThread(object Parameter)
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

                ClsAcceptInvitations obj_ClsAcceptInvitations = new ClsAcceptInvitations(Login.accountUser, Login.accountPass, Login.proxyAddress, Login.proxyPort, Login.proxyUserName, Login.proxyPassword);

                // Add Event
                Login.logger.addToLogger += new EventHandler(AcceptInvitationsLogEvents_addToLogger);
                obj_ClsAcceptInvitations.acceptInvitationsLogEvents.addToLogger += new EventHandler(AcceptInvitationsLogEvents_addToLogger);

                if (!Login.IsLoggedIn)
                {
                    //AddAcceptInvitationsLogger("[ " + DateTime.Now + " ] => [ Logging In With Account " + item.Key + " ]");

                    Login.LoginHttpHelper(ref HttpHelper);
                }

                if (Login.IsLoggedIn)
                {
                    obj_ClsAcceptInvitations.StartAcceptInvitations(ref HttpHelper);
                }
                else
                {
                    AddAcceptInvitationsLogger("[ " + DateTime.Now + " ] => [ Couldn't Login With Account " + item.Key + " ]");
                }

                // Remove Event
                Login.logger.addToLogger -= new EventHandler(AcceptInvitationsLogEvents_addToLogger);
                obj_ClsAcceptInvitations.acceptInvitationsLogEvents.addToLogger -= new EventHandler(AcceptInvitationsLogEvents_addToLogger);


            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Accept Invitation --> StartAcceptInvitation() ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Accept Invitation --> StartAcceptInvitation() ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddConnectionErrorLogs);
            }

            finally
            {
                counter_AcceptInvitation--;
                if (counter_AcceptInvitation == 0)
                {
                    BtnAccectInvitation.Invoke(new MethodInvoker(delegate
                        {
                            AddAcceptInvitationsLogger("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                            AddAcceptInvitationsLogger("-------------------------------------------------------------------------------------------------------------------");
                            BtnAccectInvitation.Cursor = Cursors.Default;
                        }));
                }
            }
        }

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
                AddAcceptInvitationsLogger("------------------------------------------------------------------------------------------------------------------------------------");
                AddAcceptInvitationsLogger(DateTime.Now  + " [ PROCESS STOPPED ]");
                AddAcceptInvitationsLogger("------------------------------------------------------------------------------------------------------------------------------------");
                BtnAccectInvitation.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error >>> " + ex.StackTrace);

            }
        }

        private void FrmAcceptInvitations_Paint(object sender, PaintEventArgs e)
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

        private void FrmAcceptInvitations_Load(object sender, EventArgs e)
        {
            image = Properties.Resources.background;
        }
    }
}
