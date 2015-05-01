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
using System.Text.RegularExpressions;
using BaseLib;

namespace InBoardPro
{
    public partial class FrmAccountVerification : Form
    {

        #region variable declaration
        bool IsStop = false;
        bool CheckNetConn = false;
        public System.Drawing.Image image;
        List<string> lstEmailVerification_AccountVerification = new List<string>();
        List<Thread> lstThread_AccountVerification = new List<Thread>(); 
        #endregion

        #region FrmAccountVerification
        public FrmAccountVerification()
        {
            InitializeComponent();
        } 
        #endregion

        #region AccountVerificationLogEvents_addToLogger
        void AccountVerificationLogEvents_addToLogger(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            if (e is EventsArgs)
            {
                EventsArgs eventArgs = e as EventsArgs;

                AddAccountVerificationLogger(eventArgs.log);
            }
        } 
        #endregion

        #region AddAccountVerificationLogger
        public void AddAccountVerificationLogger(string log)
        {
            try
            {
                if (LstBoxLogger_AccountVerification.Items.Count > 1000)
                {
                    LstBoxLogger_AccountVerification.Invoke(new MethodInvoker(delegate
                    {
                        LstBoxLogger_AccountVerification.Items.Clear();
                    }));
                }
                if (LstBoxLogger_AccountVerification.InvokeRequired)
                {
                    LstBoxLogger_AccountVerification.Invoke(new MethodInvoker(delegate
                    {
                        LstBoxLogger_AccountVerification.Items.Add(log);
                        LstBoxLogger_AccountVerification.SelectedIndex = LstBoxLogger_AccountVerification.Items.Count - 1;
                    }));
                }
                else
                {
                    LstBoxLogger_AccountVerification.Items.Add(log);
                    LstBoxLogger_AccountVerification.SelectedIndex = LstBoxLogger_AccountVerification.Items.Count - 1;
                }
            }
            catch
            {
            }
        } 
        #endregion

        #region btnAccountVerification_AccountVerification_Click
        int counter_AccVerification = 0;
        private void btnAccountVerification_AccountVerification_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {
                try
                {
                    LstBoxLogger_AccountVerification.ClearSelected();

                    int NoofThread = 5;
                    IsStop = false;
                    if (NumberHelper.ValidateNumber(textNoofThread_AccountVerification.Text.Trim()))
                    {
                        NoofThread = int.Parse(textNoofThread_AccountVerification.Text.Trim());
                        ThreadPool.SetMaxThreads(NoofThread, 5);
                    }
                    else
                    {
                        AddAccountVerificationLogger("[ " + DateTime.Now + " ] => [ Please Fill Integer value in TextBox(No. of Threads) ]");
                        MessageBox.Show("Please Fill Integer value in TextBox(No. of Threads) ");
                        textNoofThread_AccountVerification.Text = "5";
                        return;
                    }
                    counter_AccVerification = lstEmailVerification_AccountVerification.Count();
                    btnAccountVerification_AccountVerification.Cursor = Cursors.AppStarting;

                    if (lstEmailVerification_AccountVerification.Count > 0)
                    {
                        foreach (string account in lstEmailVerification_AccountVerification)
                        {
                            ThreadPool.QueueUserWorkItem(new WaitCallback(VerifyAccountMultiThread), new object[] { account });
                        }
                    }
                    else
                    {
                        AddAccountVerificationLogger("[ " + DateTime.Now + " ] => [ Please Load Account ! ]");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error >>> " + ex.StackTrace);
                }
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddAccountVerificationLogger("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        } 
        #endregion

        #region VerifyAccountMultiThread
        private void VerifyAccountMultiThread(object Parameter)
        {
            string account = string.Empty;
            string tempEmail = string.Empty;
            
            try
            {
                if (!IsStop)
                {
                    lstThread_AccountVerification.Add(Thread.CurrentThread);
                    lstThread_AccountVerification.Distinct().ToList();
                    Thread.CurrentThread.IsBackground = true;
                }
                if (IsStop)
                {
                    return;
                }
            }
            catch
            {
            }

            try
            {
                string post = string.Empty;
                Array paramsArray = new object[1];

                paramsArray = (Array)Parameter;

                //KeyValuePair<string, LinkedInMaster> item = (KeyValuePair<string, LinkedInMaster>)paramsArray.GetValue(0);

                string item = paramsArray.GetValue(0).ToString();

                account = item;//item.Key;
                GlobusHttpHelper HttpHelper = new GlobusHttpHelper();
                LinkedinLogin Login = new LinkedinLogin();

                try
                {
                    string[] arrItem = Regex.Split(item, ":");
                    tempEmail = arrItem[0];
                    if (arrItem.Length == 2)
                    {
                        Login.accountUser = arrItem[0]; //item.Key;
                        Login.accountPass = arrItem[1];//item.Value._Password;
                    }
                    else if (arrItem.Length == 4)
                    {
                        Login.accountUser = arrItem[0]; //item.Key;
                        Login.accountPass = arrItem[1];//item.Value._Password;
                        Login.proxyAddress = arrItem[2];//item.Value._ProxyAddress;
                        Login.proxyPort = arrItem[3];
                    }
                    else if (arrItem.Length == 6)
                    {
                        Login.accountUser = arrItem[0]; //item.Key;
                        Login.accountPass = arrItem[1];//item.Value._Password;
                        Login.proxyAddress = arrItem[2];//item.Value._ProxyAddress;
                        Login.proxyPort = arrItem[3];//item.Value._ProxyPort;
                        Login.proxyUserName = arrItem[4];//item.Value._ProxyUsername;
                        Login.proxyPassword = arrItem[5];//item.Value._ProxyPassword;
                    }
                    else
                    {
                        AddAccountVerificationLogger("[ " + DateTime.Now + " ] => [ Wrong Format For Email Password ]");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error >>> " + ex.StackTrace);
                }

                ClsEmailActivator obj_ClsEmailActivator = new ClsEmailActivator(Login.accountUser, Login.accountPass, Login.proxyAddress, Login.proxyPort, Login.proxyUserName, Login.proxyPassword);

                Login.logger.addToLogger += new EventHandler(AccountVerificationLogEvents_addToLogger);
                //obj_StatusUpdate.logger.addToLogger += new EventHandler(logger_StatusUpdateaddToLogger);

                if (!Login.IsLoggedIn)
                {
                    Login.LoginHttpHelper(ref HttpHelper);
                }

                if (!Login.IsLoggedIn)
                {
                    AddAccountVerificationLogger("[ " + DateTime.Now + " ] => [ Couldn't Login With Username >>> " + Login.accountUser + " ]");
                    return;
                }
                if (Login.IsLoggedIn)
                {
                    bool isActivated = obj_ClsEmailActivator.EmailVerification(Login.accountUser, Login.accountPass, ref HttpHelper);

                    if (isActivated)
                    {
                        AddAccountVerificationLogger("[ " + DateTime.Now + " ] => [ Account Activated With Username : " + Login.accountUser + " ]");
                        GlobusFileHelper.AppendStringToTextfileNewLine(Login.accountUser + ":" + Login.accountPass + ":" + Login.proxyAddress + ":" + Login.proxyPort + ":" + Login.proxyUserName + ":" + Login.proxyPassword, Globals.path_VerifiedAccounts);
                    }
                    else
                    {
                        AddAccountVerificationLogger("[ " + DateTime.Now + " ] => [ Account Couldn't Activated With Username >>> " + Login.accountUser + " ]");
                        GlobusFileHelper.AppendStringToTextfileNewLine(Login.accountUser + ":" + Login.accountPass + ":" + Login.proxyAddress + ":" + Login.proxyPort + ":" + Login.proxyUserName + ":" + Login.proxyPassword, Globals.path_NonVerifiedAccounts);
                    }

                    Login.logger.addToLogger -= new EventHandler(AccountVerificationLogEvents_addToLogger);
                }
                AddAccountVerificationLogger("[ " + DateTime.Now + " ] => [ Process Completed With Username : " + tempEmail + " ]");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error >>> " + ex.StackTrace);
            }
            finally
            {
                counter_AccVerification--;
                if (counter_AccVerification == 0)
                {
                        btnAccountVerification_AccountVerification.Invoke(new MethodInvoker(delegate
                        {
                            AddAccountVerificationLogger("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                            AddAccountVerificationLogger("--------------------------------------------------------------------------------------------------------------------------");
                            btnAccountVerification_AccountVerification.Cursor = Cursors.Default;
                        }));
                }

            }
        } 
        #endregion

        #region btnEmailsBrowse_AccountVerification_Click
        private void btnEmailsBrowse_AccountVerification_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "Text Files (*.txt)|*.txt";
                    ofd.InitialDirectory = Application.StartupPath;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtEmails_AccountVerification.Text = ofd.FileName;
                        string FilePath = ofd.FileName;
                        List<string> templist = GlobusFileHelper.ReadFile(ofd.FileName);// ReadFiletoStringList
                        lstEmailVerification_AccountVerification.Clear();
                        foreach (string item in templist)
                        {
                            //lstRemoveDuplicate.Add(item);
                            lstEmailVerification_AccountVerification.Add(item);

                        }
                        lstEmailVerification_AccountVerification = lstEmailVerification_AccountVerification.Distinct().ToList();
                        Console.WriteLine(lstEmailVerification_AccountVerification.Count + " Emails loaded");
                        AddAccountVerificationLogger("[ " + DateTime.Now + " ] => [ " + lstEmailVerification_AccountVerification.Count + " Emails loaded ]");

                    }
                }
            }
            catch
            {
            }
        } 
        #endregion

        #region FrmAccountVerification_Load
        private void FrmAccountVerification_Load(object sender, EventArgs e)
        {
            image = Properties.Resources.background;

            ClsEmailActivator.loggerEvents.addToLogger += new EventHandler(AccountVerificationLogEvents_addToLogger);
        } 
        #endregion

        #region FrmAccountVerification_Paint
        private void FrmAccountVerification_Paint(object sender, PaintEventArgs e)
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

        #region btnResendVerification_Click
        private void btnResendVerfication_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {

                try
                {
                    int NoofThread = 5;
                    if (NumberHelper.ValidateNumber(textNoofThread_AccountVerification.Text.Trim()))
                    {
                        NoofThread = int.Parse(textNoofThread_AccountVerification.Text.Trim());
                        ThreadPool.SetMaxThreads(NoofThread, 5);
                    }
                    else
                    {
                        AddAccountVerificationLogger("[ " + DateTime.Now + " ] => [ Please Fill Integer value in TextBox(No. of Threads) ]");
                        MessageBox.Show("Please Fill Integer value in TextBox(No. of Threads) ");
                        textNoofThread_AccountVerification.Text = "5";
                        return;
                    }
                    counter_AccVerification = lstEmailVerification_AccountVerification.Count();
                    btnResendVerfication.Cursor = Cursors.AppStarting;

                    if (lstEmailVerification_AccountVerification.Count > 0)
                    {
                        foreach (string account in lstEmailVerification_AccountVerification)
                        {
                            ThreadPool.QueueUserWorkItem(new WaitCallback(ResendAccountMultiThread), new object[] { account });
                        }
                    }
                    else
                    {
                        AddAccountVerificationLogger("[ " + DateTime.Now + " ] => [ Please Load Account ! ]");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error >>> " + ex.StackTrace);
                }
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddAccountVerificationLogger("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        } 
        #endregion

        #region ResendAccountMultiThread
        private void ResendAccountMultiThread(object Parameter)
        {
            try
            {
                string account = string.Empty;
                string tempEmail = string.Empty;
                string post = string.Empty;
                Array paramsArray = new object[1];

                paramsArray = (Array)Parameter;
                string item = paramsArray.GetValue(0).ToString();
                account = item;//item.Key;
                GlobusHttpHelper HttpHelper = new GlobusHttpHelper();
                LinkedinLogin Login = new LinkedinLogin();
                try
                {
                    string[] arrItem = Regex.Split(item, ":");

                    tempEmail = arrItem[0];

                    if (arrItem.Length == 2)
                    {
                        Login.accountUser = arrItem[0]; //item.Key;
                        Login.accountPass = arrItem[1];//item.Value._Password;
                    }
                    else if (arrItem.Length == 4)
                    {
                        Login.accountUser = arrItem[0]; //item.Key;
                        Login.accountPass = arrItem[1];//item.Value._Password;
                        Login.proxyAddress = arrItem[2];//item.Value._ProxyAddress;
                        Login.proxyPort = arrItem[3];//item.Value._ProxyPort;
                    }
                    else if (arrItem.Length == 6)
                    {
                        Login.accountUser = arrItem[0]; //item.Key;
                        Login.accountPass = arrItem[1];//item.Value._Password;
                        Login.proxyAddress = arrItem[2];//item.Value._ProxyAddress;
                        Login.proxyPort = arrItem[3];//item.Value._ProxyPort;
                        Login.proxyUserName = arrItem[4];//item.Value._ProxyUsername;
                        Login.proxyPassword = arrItem[5];//item.Value._ProxyPassword;
                    }
                    else
                    {
                        AddAccountVerificationLogger("[ " + DateTime.Now + " ] => [ Account Not in Correct format " + item + " ]");
                    }
                    Login.logger.addToLogger += new EventHandler(AccountVerificationLogEvents_addToLogger);
                    Login.LoginHttpHelper(ref HttpHelper);
                    if (Login.IsLoggedIn)
                    {
                        Login.ResendConfirmation(ref HttpHelper);
                    }
                    Login.logger.addToLogger += new EventHandler(AccountVerificationLogEvents_addToLogger);
                }
                catch (Exception ex)
                {


                    Console.WriteLine("Error  Resnd Confirmation I >>> " + ex.StackTrace);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Resnd Confirmation II >>> " + ex.StackTrace);
            }
            finally
            {
                counter_AccVerification--;
                if (counter_AccVerification == 0)
                {
                        btnResendVerfication.Invoke(new MethodInvoker(delegate
                        {
                            AddAccountVerificationLogger("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                            AddAccountVerificationLogger("--------------------------------------------------------------------------------------------------------------------------");
                            btnResendVerfication.Cursor = Cursors.Default;
                        }));
                }

            }
        } 
        #endregion

        #region btnAcVerificationStop_Click
        private void btnAcVerificationStop_Click(object sender, EventArgs e)
        {
            try
            {
                IsStop = true;
                List<Thread> lstTemp = lstThread_AccountVerification.Distinct().ToList();
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

                AddAccountVerificationLogger("-------------------------------------------------------------------------------------------------------------------------------");
                AddAccountVerificationLogger("[ " + DateTime.Now + " ] => [ PROCESS STOPPED ]");
                AddAccountVerificationLogger("-------------------------------------------------------------------------------------------------------------------------------");
                btnAccountVerification_AccountVerification.Cursor = Cursors.Default;
                btnResendVerfication.Cursor = Cursors.Default;
            }

            catch (Exception ex)
            {
                Console.WriteLine("Error >>> " + ex.StackTrace);
            }
        } 
        #endregion

       
    }
}
