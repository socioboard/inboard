using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.IO;
using System.Threading;
using InBoardPro;
using BaseLib;
using Others;

namespace InBoardPro
{
    public partial class frmStatusImageShare : Form
    {
        public System.Drawing.Image image;
        List<string> lstStatusImage = new List<string>();
        List<Thread> lstThreadForStatusImage = new List<Thread>();
        bool CheckNetConn = false;
        bool IsStop = false;
        bool IsCloseCalled = false;
        int threads = 7; 

        public frmStatusImageShare()
        {
            InitializeComponent();
        }

        private void frmStatusImageShare_Load(object sender, EventArgs e)
        {
            image = Properties.Resources.background;
        }

        #region logger_LinkedInProfileManageraddToLogger
        void logger_LinkedInStatusImageToLogger(object sender, EventArgs e)
        {
            try
            {

                if (e is EventsArgs)
                {
                    EventsArgs eventArgs = e as EventsArgs;
                    AddToLogStatusImage(eventArgs.log);
                }
            }
            catch { }
        }

        #endregion

        #region AddToListProfile
        private void AddToLogStatusImage(string log)
        {
            try
            {
                if (lstBoxLogsStatusImage.InvokeRequired)
                {
                    lstBoxLogsStatusImage.Invoke(new MethodInvoker(delegate
                    {
                        lstBoxLogsStatusImage.Items.Add(log);
                        lstBoxLogsStatusImage.SelectedIndex = lstBoxLogsStatusImage.Items.Count - 1;
                    }));
                }
                else
                {
                    lstBoxLogsStatusImage.Items.Add(log);
                    lstBoxLogsStatusImage.SelectedIndex = lstBoxLogsStatusImage.Items.Count - 1;
                }
            }
            catch
            {
            }
        }
        #endregion


        private void frmStatusImageShare_Paint(object sender, PaintEventArgs e)
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

      
        private void btnStatusImage_Click(object sender, EventArgs e)
        {
            try
            {
                using (FolderBrowserDialog ofd = new FolderBrowserDialog())
                {

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtProfilePicFolder_LinkedinProfileManager.Text = ofd.SelectedPath;
                        lstStatusImage.Clear();
                        string[] picsArray = Directory.GetFiles(ofd.SelectedPath);
                        lstStatusImage = picsArray.ToList();

                        List<string> lstwrongpic = new List<string>();

                        List<string> lstcorrectpics = new List<string>();

                        foreach (string item in lstStatusImage)
                        {
                            try
                            {
                                if (item.EndsWith(".ini"))
                                {
                                    continue;
                                }
                            }
                            catch { }
                            try
                            {

                                string items = item;
                                items = item.ToLower();
                                if (!items.Contains(".jpg") && !items.Contains(".png") && !items.Contains(".jpeg") && !items.Contains(".gif") && !items.Contains(".bmp"))
                                {
                                    MessageBox.Show("Your File Have Some Wrong Pic So Please Upload Correct ProfilePic With(.jpg,.png,.jpeg,and.gif extension) Image File !");
                                    lstStatusImage.Clear();
                                    return;
                                }
                                else
                                {
                                    lstcorrectpics.Add(item);
                                }

                            }
                            catch { }

                        }
                        ClsStatusImageShare.lstpicfile = lstcorrectpics;
                        Console.WriteLine(lstcorrectpics.Count + " Images loaded");
                        AddToLogStatusImage("[ " + DateTime.Now + " ] => [ " + lstcorrectpics.Count + " Images loaded ]");
                    }
                }
            }
            catch { }
        }

        static int counter_status_posting = 0;

        private void btnAddStatusImage_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {

                try
                {
                    lstThreadForStatusImage.Clear();
                    if (IsStop)
                    {
                        IsStop = false;
                    }

                    if (LinkedInManager.linkedInDictionary.Count() == 0)
                    {
                        MessageBox.Show("Please Load LinkedIn Accounts From MyAccounts Menu");
                        AddToLogStatusImage("[ " + DateTime.Now + " ] => [ Please Load LinkedIn Accounts From MyAccounts Menu ]");
                        frmAccounts FrmAccount = new frmAccounts();
                        FrmAccount.Show();
                        return;
                    }
                    else if (ClsStatusImageShare.lstpicfile.Count == 0)
                    {
                        MessageBox.Show("Please Add Status Image for Post");
                        AddToLogStatusImage("[ " + DateTime.Now + " ] => [ Please Add Status Image for Post ]");
                        return;
                    }

                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Status Update --> btnAddStatusImage_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Status Update --> btnAddStatusImage_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinStatusImageUpdateErrorLogs);
                }

                new Thread(() =>
                {
                    LinkdinStatusImageShare();

                }).Start();
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddToLogStatusImage("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
       
        }


        #region LinkdinStatusUpdate()
     
        private void LinkdinStatusImageShare()
        {
            try
            {
                int numberofThreds = 2;

                if (NumberHelper.ValidateNumber(txtThreads.Text.Trim()))
                {
                    numberofThreds = int.Parse(txtThreads.Text.Trim());

                }

                counter_status_posting = LinkedInManager.linkedInDictionary.Count;

                if (LinkedInManager.linkedInDictionary.Count() > 0)
                {
                    ThreadPool.SetMaxThreads(numberofThreds, 5);

                    Others.StatusUpdate.Que_Message_Post.Clear();

                    foreach (string Message in ClsStatusImageShare.lstpicfile)
                    {
                        Others.StatusUpdate.Que_Message_Post.Enqueue(Message);
                    }

                    foreach (KeyValuePair<string, LinkedInMaster> item in LinkedInManager.linkedInDictionary)
                    {
                        try
                        {

                            if (Others.StatusUpdate.Que_Message_Post.Count > 0)
                            {
                                try
                                {
                                    #region multithread
                                    //ThreadPool.SetMaxThreads(numberofThreds, numberofThreds);
                                    //ThreadPool.QueueUserWorkItem(new WaitCallback(PostStatus), new object[] { item });
                                    //Thread.Sleep(1000); 
                                    #endregion

                                    PostStatusImageShare(new object[] { item });
                                }
                                catch (Exception ex)
                                {
                                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Status Image Share --> LinkdinStatusImageShare() ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Status Image Share --> LinkdinStatusImageShare() ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinStatusImageUpdateErrorLogs);
                                }
                            }
                            else
                            {
                                AddToLogStatusImage("[ " + DateTime.Now + " ] => [ No Message To Post . Please Upload Message to be Posted ]");
                            }
                        }
                        catch (Exception ex)
                        {
                            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Status Image Share --> LinkdinStatusImageShare() ---2--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Status Image Share --> LinkdinStatusImageShare() ---2--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinStatusImageUpdateErrorLogs);
                        }
                    }
                }
                else
                {
                    AddToLogStatusImage("[ " + DateTime.Now + " ] => [ Please Load LinkedIn Accounts From MyAccounts Menu ]");
                    MessageBox.Show("Please Load LinkedIn Accounts From MyAccounts Menu");
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> LinkdinStatusImageShare() ---3--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> LinkdinStatusImageShare() ---3--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinStatusImageUpdateErrorLogs);
            }
        }

        #endregion


        #region for single thread
        public void PostStatusImageShare(object Parameter)
        {
            try
            {
                if (IsStop)
                {
                    return;
                }

                if (!IsStop)
                {
                    Thread.CurrentThread.Name = "StatusImageShareThread";
                    lstThreadForStatusImage.Add(Thread.CurrentThread);
                    lstThreadForStatusImage.Distinct().ToList();
                    Thread.CurrentThread.IsBackground = true;
                }
            }
            catch
            {
            }

            string account = string.Empty;
            try
            {
                string post = string.Empty;
                Array paramsArray = new object[1];

                paramsArray = (Array)Parameter;

                KeyValuePair<string, LinkedInMaster> item = (KeyValuePair<string, LinkedInMaster>)paramsArray.GetValue(0);
                account = item.Key;
                GlobusHttpHelper HttpHelper = new GlobusHttpHelper();
                LinkedinLogin Login = new LinkedinLogin();

                Login.accountUser = item.Key;
                Login.accountPass = item.Value._Password;
                Login.proxyAddress = item.Value._ProxyAddress;
                Login.proxyPort = item.Value._ProxyPort;
                Login.proxyUserName = item.Value._ProxyUsername;
                Login.proxyPassword = item.Value._ProxyPassword;

                Others.StatusUpdate obj_StatusUpdate = new Others.StatusUpdate(Login.accountUser, Login.accountPass, Login.proxyAddress, Login.proxyPort, Login.proxyUserName, Login.proxyPassword);

                Login.logger.addToLogger += new EventHandler(logger_LinkedInStatusImageToLogger);
                obj_StatusUpdate.logger.addToLogger += new EventHandler(logger_LinkedInStatusImageToLogger);

                if (!Login.IsLoggedIn)
                {
                    Login.LoginHttpHelper(ref HttpHelper);
                }

                
                if (!Login.IsLoggedIn)
                {
                    AddToLogStatusImage("[ " + DateTime.Now + " ] => [ Couldn't Login With Username >>> " + Login.accountUser + " ]");
                    return;
                }

                if (Login.IsLoggedIn)
                {
                    lock (LinkedinLogin.Locker_Message_Post)
                    {
                        if (ClsStatusImageShare.lstpicfile.Count > 1)
                        {
                            if (Others.StatusUpdate.Que_Message_Post.Count > 0)
                            {
                                obj_StatusUpdate.Post = Others.StatusUpdate.Que_Message_Post.Dequeue();
                            }
                            else
                            {
                                AddToLogStatusImage("[ " + DateTime.Now + " ] => [ All Loaded Post Updates ]");
                                return;
                            }
                        }
                        else
                        {
                            obj_StatusUpdate.Post = ClsStatusImageShare.lstpicfile[0];
                        }
                    }

                    int minDelay = 20;
                    int maxDelay = 25;
                    if (!string.IsNullOrEmpty(txtStatusUpdateMinDelay.Text) && NumberHelper.ValidateNumber(txtStatusUpdateMinDelay.Text))
                    {
                        minDelay = Convert.ToInt32(txtStatusUpdateMinDelay.Text);
                    }
                    if (!string.IsNullOrEmpty(txtStatusUpdateMaxDelay.Text) && NumberHelper.ValidateNumber(txtStatusUpdateMaxDelay.Text))
                    {
                        maxDelay = Convert.ToInt32(txtStatusUpdateMaxDelay.Text);
                    }
                                       
                        try
                        {
                            obj_StatusUpdate.UpdateStatusUsingAllurl(ref HttpHelper, minDelay, maxDelay);
                        }
                        catch
                        {
                        }
                   
                }

                Login.logger.addToLogger -= new EventHandler(logger_LinkedInStatusImageToLogger);
                obj_StatusUpdate.logger.addToLogger -= new EventHandler(logger_LinkedInStatusImageToLogger);
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> LinkdinStatusImageShare --> PostStatusImageShare() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> LinkdinStatusImageShare --> PostStatusImageShare() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinStatusImageUpdateErrorLogs);
            }
            finally
            {
                counter_status_posting--;

                if (counter_status_posting == 0)
                {
                    if (btnAddStatusImage.InvokeRequired)
                    {
                        btnAddStatusImage.Invoke(new MethodInvoker(delegate
                        {
                            AddToLogStatusImage("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                            AddToLogStatusImage("-------------------------------------------------------------------------------------------------------------------------------");
                            //btnStatusPost.Cursor = Cursors.Default;
                        }));
                    }
                }
            }
        }
        #endregion
       
    }
}
