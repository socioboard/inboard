using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using InBoardPro;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;
using ProfileManager;
using BaseLib;


namespace InBoardPro
{
    public partial class FrmLinkedInImageUploader : Form
    {
        #region Global declaration
        private static bool IsCloseCalledForProfileManager = false;
        readonly object lockr_ThreadController = new object();
        bool IsCloseCalled = false;
        public System.Drawing.Image image;
        int count_ThreadController = 0;
        List<Thread> lstThreadForLinkedInProfileManager = new List<Thread>();
        List<string> lstProfilePic = new List<string>();
        bool IsStop = false;
        bool CheckNetConn = false;
        int threads = 7; 
        #endregion

        #region FrmLinkedInProfileManager()
        public FrmLinkedInImageUploader()
        {
            InitializeComponent();
        } 
        #endregion

        #region logger_LinkedInProfileManageraddToLogger
        void logger_LinkedInProfileManageraddToLogger(object sender, EventArgs e)
        {
            try
            {

                if (e is EventsArgs)
                {
                    EventsArgs eventArgs = e as EventsArgs;
                    AddLoggerLinkedInProfileManager(eventArgs.log);
                }
            }
            catch { }
        }

       #endregion

        #region AddLoggerLinkedInProfileManager
        private void AddLoggerLinkedInProfileManager(string log)
        {
            try
            {
                if (lstBoxLogsProfileManager.InvokeRequired)
                {
                    lstBoxLogsProfileManager.Invoke(new MethodInvoker(delegate
                    {
                        lstBoxLogsProfileManager.Items.Add(log);
                        lstBoxLogsProfileManager.SelectedIndex = lstBoxLogsProfileManager.Items.Count - 1;
                    }));
                }
                else
                {
                    lstBoxLogsProfileManager.Items.Add(log);
                    lstBoxLogsProfileManager.SelectedIndex = lstBoxLogsProfileManager.Items.Count - 1;
                }
            }
            catch { }
        } 
        #endregion

        #region btnProfilePicsFolder_LinkedInProfileManager_Click
        private void btnProfilePicsFolder_LinkedInProfileManager_Click(object sender, EventArgs e)
        {
            try
            {
                using (FolderBrowserDialog ofd = new FolderBrowserDialog())
                {

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtProfilePicFolder_LinkedinProfileManager.Text = ofd.SelectedPath;
                        lstProfilePic.Clear();
                        string[] picsArray = Directory.GetFiles(ofd.SelectedPath);
                        lstProfilePic = picsArray.ToList();

                        List<string> lstwrongpic = new List<string>();

                        List<string> lstcorrectpics = new List<string>();

                        foreach (string item in lstProfilePic)
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
                               // items = item.ToLower();
                                item.ToUpper();
                                if (!items.Contains(".jpg") && !items.Contains(".png") && !items.Contains(".JPEG") && !items.Contains(".gif") && !items.Contains(".bmp"))
                                {
                                    MessageBox.Show("Your File Have Some Wrong Pic So Please Upload Correct ProfilePic With(.jpg,.png,.JPEG,and.gif extension) Image File !");
                                    lstProfilePic.Clear();
                                    return;
                                }
                                else
                                {
                                    lstcorrectpics.Add(item);
                                }

                            }
                            catch { }

                        }
                        LinkedinProfileImgUploader.lstpicfile = lstcorrectpics;
                        Console.WriteLine(lstcorrectpics.Count + " Profile Pics loaded");
                        AddToListProfile("[ " + DateTime.Now + " ] => [ " + lstcorrectpics.Count + " Profile Images loaded ]");
                    }
                }
            }
            catch { }
        } 
        #endregion

        #region AddToListProfile
        private void AddToListProfile(string log)
        {
            try
            {
                if (lstBoxLogsProfileManager.InvokeRequired)
                {
                    lstBoxLogsProfileManager.Invoke(new MethodInvoker(delegate
                    {
                        lstBoxLogsProfileManager.Items.Add(log);
                        lstBoxLogsProfileManager.SelectedIndex = lstBoxLogsProfileManager.Items.Count - 1;
                    }));
                }
                else
                {
                    lstBoxLogsProfileManager.Items.Add(log);
                    lstBoxLogsProfileManager.SelectedIndex = lstBoxLogsProfileManager.Items.Count - 1;
                }
            }
            catch
            {
            }
        } 
        #endregion

        #region btnAddProfilePic_Click
        private void btnAddProfilePic_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {
                try
                {
                    IsCloseCalledForProfileManager = false;
                    lstThreadForLinkedInProfileManager.Clear();

                    if (lstProfilePic.Count < 1)
                    {
                        MessageBox.Show("Please Upload Profile Image !");
                        return;
                    }

                    if (rdbUnique.Checked)
                    {
                        try
                        {
                            LinkedinProfileImgUploader.AddProfilePic_Unique = true;
                        }
                        catch { }
                    }
                    else
                    {
                        try
                        {
                            LinkedinProfileImgUploader.AddProfilePic_Random = true;
                        }
                        catch { }
                    }

                    if (NumberHelper.ValidateNumber(txtThreads.Text))
                    {
                        threads = int.Parse(txtThreads.Text);
                    }

                    //Thread thread_AddaCover = new Thread(thread_AddaCoverStart);
                    // thread_AddaCover.Start();

                    thread_AddaCoverStart();
                }
                catch { }
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddToListProfile("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        } 
        #endregion

        #region thread_AddaCoverStart
        private void thread_AddaCoverStart()
        {
            try
            {
                if (!IsCloseCalledForProfileManager)
                {
                    try
                    {
                        //lstThreadForLinkedInProfileManager.Add(Thread.CurrentThread);
                        //lstThreadForLinkedInProfileManager.Distinct();
                        //Thread.CurrentThread.IsBackground = true;
                    }
                    catch { }

                    List<List<string>> list_listAccounts = new List<List<string>>();

                    #region
                    if (Globals.listAccounts.Count > 0)
                    {
                        if (threads == 0)
                        {
                            // numberOfAccountPatch = RandomNumberGenerator.GenerateRandom(0, lstGroupURLsFile.Count - 1);
                        }
                        list_listAccounts = ListUtilities.Split(Globals.listAccounts, threads);
                        foreach (List<string> listAccounts in list_listAccounts)
                        {
                            try
                            {
                                foreach (string account in listAccounts)
                                {
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
                                                Thread AddaCoverThread = new Thread(AddaCoverMulti_List);
                                                AddaCoverThread.Name = "workerThread_AddaCover_" + acc;
                                                AddaCoverThread.IsBackground = true;

                                                try
                                                {
                                                    // dictionary_LikerThreads.Add(AddaCoverThread.Name, AddaCoverThread);
                                                }
                                                catch { }

                                                AddaCoverThread.Start(new object[] { item });

                                                count_ThreadController++;
                                                //tempCounterAccounts++; 
                                            }
                                        }
                                        catch { }
                                    }
                                }
                            }
                            catch { }
                        }
                    }

                    else
                    {
                        MessageBox.Show("Please Upload The Accounts !");
                        return;
                    }
                    #endregion
                }
            }
            catch { }
        } 
        #endregion

        #region AddaCoverMulti_List
        private void AddaCoverMulti_List(object oo)
        {
            try
            {
                if (!IsCloseCalledForProfileManager)
                {
                    try
                    {
                        lstThreadForLinkedInProfileManager.Add(Thread.CurrentThread);
                        lstThreadForLinkedInProfileManager.Distinct();
                        Thread.CurrentThread.IsBackground = true;
                    }
                    catch
                    {
                    }
                    Array paramArr = new object[2];
                    paramArr = (Array)oo;

                    LinkedInMaster linkedInMaster = (LinkedInMaster)paramArr.GetValue(0);//KeyValuePair<string, Facebooker> item = (KeyValuePair<string, Facebooker>)paramArr.GetValue(0);
                    //  KeyValuePair<string, LinkedInMaster> item = (KeyValuePair<string, LinkedInMaster>)paramArr.GetValue(0);

                    //Facebooker facebooker = item.Value;

                    GlobusHttpHelper HttpHelper = new GlobusHttpHelper();
                    LinkedinLogin Login = new LinkedinLogin();


                    string accountUser = linkedInMaster._Username;
                    string accountPass = linkedInMaster._Password;
                    string proxyAddress = linkedInMaster._ProxyAddress;
                    string proxyPort = linkedInMaster._ProxyPort;
                    string proxyUserName = linkedInMaster._ProxyUsername;
                    string proxyPassword = linkedInMaster._ProxyPassword;

                    linkedInMaster.logger.addToLogger += new EventHandler(logger_LinkedInProfileManageraddToLogger);
                    linkedInMaster.LoginHttpHelper(ref HttpHelper);

                    if (linkedInMaster.IsLoggedIn)
                    {
                        LinkedinProfileImgUploader obj_LinkedinProfileManager = new LinkedinProfileImgUploader(accountUser, accountPass, proxyAddress, proxyPort, proxyUserName, proxyPassword);

                        obj_LinkedinProfileManager.LinkedInProfileManagerLogEvents.addToLogger += logger_LinkedInProfileManageraddToLogger;

                        NativeMethods.DeleteUrlCacheEntry("http://www.linkedin.com/");

                        obj_LinkedinProfileManager.SetProfilePic(ref HttpHelper);

                        NativeMethods.DeleteUrlCacheEntry("http://www.linkedin.com/");

                        obj_LinkedinProfileManager.LinkedInProfileManagerLogEvents.addToLogger -= new EventHandler(logger_LinkedInProfileManageraddToLogger);
                    }

                    linkedInMaster.logger.addToLogger -= new EventHandler(logger_LinkedInProfileManageraddToLogger);
                }
            }
            catch
            {
            }

            finally
            {
                if (!IsCloseCalled)
                {
                    count_ThreadController--;
                    lock (lockr_ThreadController)
                    {
                        if (!IsCloseCalled)
                        {
                            Monitor.Pulse(lockr_ThreadController);
                        }
                    }
                }
            }
        } 
        #endregion

        #region FrmLinkedInProfileManager_Load
        private void FrmLinkedInProfileManager_Load(object sender, EventArgs e)
        {
            image = Properties.Resources.background;
        } 
        #endregion

        #region FrmLinkedInProfileManager_Paint
        private void FrmLinkedInProfileManager_Paint(object sender, PaintEventArgs e)
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

        #region btnProfPicStop_Click
        private void btnProfPicStop_Click(object sender, EventArgs e)
        {
            try
            {
                IsStop = true;
                List<Thread> lstTemp = lstThreadForLinkedInProfileManager.Distinct().ToList();
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

                AddToListProfile("------------------------------------------------------------------------------------------------------------------------------------");
                AddToListProfile("[ " + DateTime.Now + " ] => [ PROCESS STOPPED ]");
                AddToListProfile("------------------------------------------------------------------------------------------------------------------------------------");
            }

            catch (Exception ex)
            {
                Console.WriteLine("Error >>> " + ex.StackTrace);
            }
        } 
        #endregion

        
    }

    #region Class NativeMethods
    public static class NativeMethods
    {
        [DllImport("WinInet.dll", PreserveSig = true, SetLastError = true)]
        public static extern bool DeleteUrlCacheEntry(string url);
    } 
    #endregion
}
