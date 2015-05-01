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
using InBoardPro;
using System.Threading;
using Campaign;
using BaseLib.DB_Repository;
using ManageConnections;
using Groups;
using ProfileManager;
using BaseLib;

namespace InBoardPro
{
    public partial class frmCampaignAccountCreator : Form
    {
        static frmCampaignAccountCreator campaignAccountCreator;
        public frmCampaignAccountCreator()
        {
            InitializeComponent();
        }

        public static frmCampaignAccountCreator GetFrmCampaignAccountCreator()
        {
            if (campaignAccountCreator == null)
            {
                campaignAccountCreator = new frmCampaignAccountCreator();
            }
            return campaignAccountCreator;
        }

        public System.Drawing.Image image;
        clsLinkedINAccount objclsLinkedINAccount = new clsLinkedINAccount();
        private static bool IsCloseCalledForProfileManager = false;
        readonly object lockr_ThreadController = new object();
        bool IsCloseCalled = false;
        int count_ThreadController = 0;
        List<Thread> lstThreadForLinkedInProfileManager = new List<Thread>();
        public static List<Thread> Campaign_lstSearchconnectionThread = new List<Thread>();
        List<Thread> lstJoinSearchGroupThread = new List<Thread>();
               

        #region frmCampaignAccountCreator_Paint
        private void frmCampaignAccountCreator_Paint(object sender, PaintEventArgs e)
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

                throw;
            }
        }
        #endregion
        
        #region frmCampaignAccountCreator_Load
        private void frmCampaignAccountCreator_Load(object sender, EventArgs e)
        {
            CampainGroupCreate.Campaign_ChkProfilePict = true;
            image = Properties.Resources.background;


            //FrmLinkedInProfileManager.LinkedInProfileManagerLogEvents.
            //ConnectUsingSearch.ConnectSearchLogEvents.addToLogger += new EventHandler(logger_LinkedInProfileManageraddToLogger);
          
           
        }
        #endregion

       
        private void frmCampaignAccountCreator_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Hiding the window, because closing it makes the window unaccessible.
            this.Hide();
            this.Parent = null;
            e.Cancel = true; //hides the form, cancels closing event
        }

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

        private void AddLoggerLinkedInProfileManager(string log)
        {
            try
            {
                if (lstBoxLogsAccountCreatorCampain.InvokeRequired)
                {
                    lstBoxLogsAccountCreatorCampain.Invoke(new MethodInvoker(delegate
                    {
                        lstBoxLogsAccountCreatorCampain.Items.Add(log);
                        lstBoxLogsAccountCreatorCampain.SelectedIndex = lstBoxLogsAccountCreatorCampain.Items.Count - 1;
                    }));
                }
                else
                {
                    lstBoxLogsAccountCreatorCampain.Items.Add(log);
                    lstBoxLogsAccountCreatorCampain.SelectedIndex = lstBoxLogsAccountCreatorCampain.Items.Count - 1;
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
                    //ofd.SelectedPath = Application.StartupPath + "\\Profile\\Pics";

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtProfilePicFolder_LinkedinProfileManager.Text = ofd.SelectedPath;
                        CampainGroupCreate.lstProfilePic.Clear();
                        string[] picsArray = Directory.GetFiles(ofd.SelectedPath);
                        CampainGroupCreate.lstProfilePic = picsArray.ToList();
                        List<string> lstwrongpic = new List<string>();

                        foreach (string item in CampainGroupCreate.lstProfilePic)
                        {
                            try
                            {

                                string items = item;
                                items = item.ToLower();
                                if (!items.Contains(".jpg") && !items.Contains(".png") && !items.Contains(".jpeg") && !items.Contains(".gif"))
                                {
                                    // lstCoverPics.Add(item);
                                    MessageBox.Show("Your File Have Some Wrong Pic So Please Upload Correct ProfilePic With(.jpg,.png,.jpeg,and.gif extension) Image File !");
                                    CampainGroupCreate.lstProfilePic.Clear();
                                    return;

                                }


                            }
                            catch { }

                        }

                        LinkedinProfileImgUploader.lstpicfile = CampainGroupCreate.lstProfilePic;
                        Console.WriteLine(CampainGroupCreate.lstProfilePic.Count + " Profile Pics loaded");
                        AddLoggerLinkedInProfileManager("[ " + DateTime.Now + " ] => [ " + CampainGroupCreate.lstProfilePic.Count + " Profile Images loaded ]");

                      
                    }
                }
            }
            catch { }

        }
        #endregion

        #region AddToListCampaintAccountCreator
        private void AddToListCampainAccountCreator(string log)
        {
            try
            {
                if (lstBoxLogsAccountCreatorCampain.InvokeRequired)
                {
                    lstBoxLogsAccountCreatorCampain.Invoke(new MethodInvoker(delegate
                    {
                        lstBoxLogsAccountCreatorCampain.Items.Add(log);
                        lstBoxLogsAccountCreatorCampain.SelectedIndex = lstBoxLogsAccountCreatorCampain.Items.Count - 1;
                    }));
                }
                else
                {
                    lstBoxLogsAccountCreatorCampain.Items.Add(log);
                    lstBoxLogsAccountCreatorCampain.SelectedIndex = lstBoxLogsAccountCreatorCampain.Items.Count - 1;
                }
            }
            catch { }
        }
        #endregion

        //*********Use Profile Picture*****************//

        #region chkUseProfilePict_CheckedChanged
        private void chkUseProfilePict_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUseProfilePict.Checked == true)
            {
                CampainGroupCreate.Campaign_ChkProfilePict = true;
            }
            else
            {
                CampainGroupCreate.Campaign_ChkProfilePict = false;
            }
        }
        #endregion

        #region AddProfilePicture
        int threads = 7;
        public void AddProfilePicture(ref GlobusHttpHelper GlobusHttpHelper, string Email, string Password, string proxyAddress, int ProxyPort, string proxyUsername, string proxyPassword)
        {
            try
            {


                IsCloseCalledForProfileManager = false;
                lstThreadForLinkedInProfileManager.Clear();

                if (CampainGroupCreate.lstProfilePic.Count < 1)
                {
                    MessageBox.Show("Please Upload Profile Image !");
                    AddLoggerLinkedInProfileManager("[ " + DateTime.Now + " ] => [ Please Upload Profile Image ! ]");
                    //return;
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


                LinkedinProfileImgUploader obj_LinkedinProfileManager = new LinkedinProfileImgUploader(Email, Password, proxyAddress, ProxyPort.ToString(), proxyUsername, proxyPassword);
                obj_LinkedinProfileManager.LinkedInProfileManagerLogEvents.addToLogger += new EventHandler(logger_LinkedInProfileManageraddToLogger);
                

                obj_LinkedinProfileManager.SetProfilePic(ref GlobusHttpHelper);
                
                

                //if (NumberHelper.ValidateNumber(txtThreads.Text))
                //{
                //    threads = int.Parse(txtThreads.Text);
                //}

                //Thread thread_AddaCover = new Thread(thread_AddaCoverStart);
                //thread_AddaCover.Start();
                //thread_AddaCoverStart();
            }
            catch { }
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
                        lstThreadForLinkedInProfileManager.Add(Thread.CurrentThread);
                        lstThreadForLinkedInProfileManager.Distinct();
                        Thread.CurrentThread.IsBackground = true;
                    }
                    catch
                    {
                    }

                    //int numberOfAccountPatch = 20;

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
                            catch
                            {
                            }
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
            catch
            {
            }
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

                    if (!Login.IsLoggedIn)
                    {
                        //Login.LoginHttpHelper(ref HttpHelper);
                        Login.logger.addToLogger += new EventHandler(logger_LinkedInProfileManageraddToLogger);
                        Login.LoginHttpHelper(ref HttpHelper, accountUser, accountPass, proxyAddress, proxyUserName, proxyPassword, proxyPort);
                    }

                    if (Login.IsLoggedIn)
                    {
                        LinkedinProfileImgUploader obj_LinkedinProfileManager = new LinkedinProfileImgUploader(accountUser, accountPass, proxyAddress, proxyPort, proxyUserName, proxyPassword);

                        ////facebooker.loginChecker.pumpMessageEvent.addToLogger += ProfileManagerLogEvents_addToLogger;

                        obj_LinkedinProfileManager.LinkedInProfileManagerLogEvents.addToLogger += new EventHandler(logger_LinkedInProfileManageraddToLogger);

                        //facebooker.StartAddaCover(ref obj_AddaCover, lstCoverPics);

                        obj_LinkedinProfileManager.SetProfilePic(ref HttpHelper);


                        ////facebooker.loginChecker.pumpMessageEvent.addToLogger -= ProfileManagerLogEvents_addToLogger;
                        Login.logger.addToLogger -= new EventHandler(logger_LinkedInProfileManageraddToLogger);
                        obj_LinkedinProfileManager.LinkedInProfileManagerLogEvents.addToLogger -= new EventHandler(logger_LinkedInProfileManageraddToLogger);
                    }
                    else
                    {
                        AddLoggerLinkedInProfileManager("[ " + DateTime.Now + " ] => [ Couldn't Login With Username >>> " + accountUser + " ]");
                        return;
                    }
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

        //*********Manage Connetion with search keword*****************//

        #region btnKeywordLoadFile_Click
        private void btnKeywordLoadFile_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "Text Files (*.txt)|*.txt";
                    ofd.InitialDirectory = Application.StartupPath;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtKeywordLoad.Text = ofd.FileName;
                        CampainGroupCreate.Campaign_lstConnectionSearchKeyword.Clear();
                        List<string> templist = GlobusFileHelper.ReadFiletoStringList(ofd.FileName);
                        foreach (string item in templist)
                        {
                            if (!CampainGroupCreate.Campaign_lstConnectionSearchKeyword.Contains(item))
                            {
                                if (!string.IsNullOrEmpty(item.Replace(" ", "").Replace("\t", "")))
                                {
                                    CampainGroupCreate.Campaign_lstConnectionSearchKeyword.Add(item);
                                    //SearchCriteria.Que_Keyword.Enqueue(item);
                                }
                            }
                        }
                    }
                    AddLoggerLinkedInProfileManager("[ " + DateTime.Now + " ] => [ " + CampainGroupCreate.Campaign_lstConnectionSearchKeyword.Count + " Keywords Loaded  ]");
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Connection --> btnKeywordLoadFile_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Connection --> btnKeywordLoadFile_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddConnectionErrorLogs);
            }
        } 
        #endregion
     
        #region SearchConnection
        public int counter_GroupMemberSearch = 0;
        public void SearchConnection(ref GlobusHttpHelper GlobusHttpHelper, string Email, string Password, string proxyAddress, int ProxyPort, string proxyUsername, string proxyPassword)
        {
           
            try
            {
                Campaign_lstSearchconnectionThread.Clear();


                int SearchMinDelay = 0;
                int SearchMaxDelay = 0;
                bool DelayReset = true;
                if (chkUniqueConnectin.Checked)
                {
                    ManageConnections.ConnectUsing_Search.UseuniqueConn = true;
                    foreach (string itemKeyword in CampainGroupCreate.Campaign_lstConnectionSearchKeyword)
                    {
                        try
                        {
                            ManageConnections.ConnectUsing_Search.lstQueuKeywords.Enqueue(itemKeyword);

                        }
                        catch { }
                    }
                }

                if (CampainGroupCreate.Campaign_lstConnectionSearchKeyword.Count == 0)
                {
                    MessageBox.Show("Please Add Keywords!");
                    AddLoggerLinkedInProfileManager("[ " + DateTime.Now + " ] => [ Please Add Keywords! ]");
                    return;
                }
                else
                {
                    Queue<string> que_SearchKeywords = new Queue<string>();
                    foreach (string itemKeyword in CampainGroupCreate.Campaign_lstConnectionSearchKeyword)
                    {
                        que_SearchKeywords.Enqueue(itemKeyword);
                    }

                    LinkedInMaster LinkedIn_Master = new LinkedInMaster();
                    ManageConnections.ConnectUsing_Search ConnectUsing_Search = new ConnectUsing_Search(Email, Password, proxyAddress, ProxyPort.ToString(), proxyUsername, proxyPassword, que_SearchKeywords);
                    ManageConnections.ConnectUsing_Search.ConnectSearchUsingkeyword(ref ConnectUsing_Search, SearchMinDelay, SearchMaxDelay);
       
                    //ConnectUsing_Search.ConnectionSearch();

                    //ConnectUsingSearch obj_ConnectUsingSearch = new ConnectUsingSearch(Email, Password, proxyAddress, ProxyPort.ToString(), proxyUsername, proxyPassword);
                    //obj_ConnectUsingSearch.ConnectionSearch();
                }
               
               

            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Connection --> btnSearchConnection_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Connection --> btnSearchConnection_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddConnectionErrorLogs);
            }
        } 
        #endregion
       

        //****************for Search Group**********************//

        #region AddSearchGroup
        public void AddSearchGroup(ref GlobusHttpHelper GlobusHttpHelper, string Email, string Password, string proxyAddress, int ProxyPort, string proxyUsername, string proxyPassword)
        {
             LinkedinLogin Login = new LinkedinLogin();

             try
             {
                 if (!string.IsNullOrEmpty(CampainGroupCreate.GroupCount) && NumberHelper.ValidateNumber(CampainGroupCreate.GroupCount))
                 {
                     Groups.JoinSearchGroup.BoxGroupCount = Convert.ToInt32(CampainGroupCreate.GroupCount);

                 }
                 else
                 {
                     if (string.IsNullOrEmpty(CampainGroupCreate.GroupCount))
                     {

                     }
                     else
                     {
                         MessageBox.Show("Enter No. of Group in Numeric Form !");
                         return;
                     }
                 }

                 lstJoinSearchGroupThread.Clear();

                 if (CampainGroupCreate.SearchKeyword == string.Empty)
                 {
                     AddLoggerLinkedInProfileManager("[ " + DateTime.Now + " ] => [ Please Enter Keyword to Search.. ]");
                     MessageBox.Show("Please Enter Keyword to Search..");
                     txtSearchKeyword.Focus();
                     return;
                 }

                 AddLoggerLinkedInProfileManager("[ " + DateTime.Now + " ] => [ Starting Search for Groups.. ]");
 
                 Login.accountUser = Email;
                 Login.accountPass = Password;
                 Login.proxyAddress = proxyAddress;
                 Login.proxyPort = ProxyPort.ToString();
                 Login.proxyUserName = proxyUsername;
                 Login.proxyPassword = proxyPassword;


                 Groups.JoinSearchGroup obj_JoinSearchGroup = new Groups.JoinSearchGroup(Email, Password, proxyAddress, ProxyPort.ToString(), proxyUsername, proxyPassword);

                 obj_JoinSearchGroup.logger.addToLogger += new EventHandler(logger_LinkedInProfileManageraddToLogger);

                 GlobusHttpHelper HttpHelper = new GlobusHttpHelper();

                 if (!Login.IsLoggedIn)
                 {
                     Login.LoginHttpHelper(ref HttpHelper);
                     AddLoggerLinkedInProfileManager("[ " + DateTime.Now + " ] => [ Logging In With Email : " + Email + " ]");
                 }

                 AddLoggerLinkedInProfileManager("[ " + DateTime.Now + " ] => [ Searching Groups Process Running..Please wait.. ]");

                 if (Login.IsLoggedIn)
                 {
                     GroupStatus dataScrape = new GroupStatus();
                     try
                     {
                         CampainGroupCreate.Result = obj_JoinSearchGroup.PostAddOpenGroups(ref HttpHelper, CampainGroupCreate.SearchKeyword.ToString().Trim(), Email);

                         int count = 5;
                         if (!string.IsNullOrEmpty(CampainGroupCreate.GroupCount) && NumberHelper.ValidateNumber(CampainGroupCreate.GroupCount))
                         {
                             count = Convert.ToInt32(CampainGroupCreate.GroupCount);
                         }

                         CampainGroupCreate.LinkdInContacts.Add(Login.accountUser, CampainGroupCreate.Result);
                     }
                     catch (Exception ex)
                     {
                         GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Search Groups --> --1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                         GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Search Groups --> --1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddSearchGroupErrorLogs);
                     }


                     if (CampainGroupCreate.Result.Count == 0)
                     {
                         AddLoggerLinkedInProfileManager("[ " + DateTime.Now + " ] => [ Sorry, there are no group Search results matching your search criteria.. ]");
                     }
                     else
                     {
                         AddLoggerLinkedInProfileManager("[ " + DateTime.Now + " ] => [ Groups..Searched Successfully..For " +  Login.accountUser + " ]");

                    }

                     foreach (var item in CampainGroupCreate.Result)
                     {
                        CampainGroupCreate.lstSearchGroup.Add(item.Key + ":" + item.Value);
                     }


                     AddLoggerLinkedInProfileManager("[ " + DateTime.Now + " ] => [ Now Joining Groups ]");

                     string MessagePosted = obj_JoinSearchGroup.PostSearchGroupAddFinal(ref HttpHelper, Email, Password, CampainGroupCreate.lstSearchGroup,0,0);

                     Login.logger.addToLogger -= new EventHandler(logger_LinkedInProfileManageraddToLogger);
                     obj_JoinSearchGroup.logger.addToLogger -= new EventHandler(logger_LinkedInProfileManageraddToLogger);
                    


                 }
             }
             catch (Exception ex)
             {
                 GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Search Groups --> btnAddSearchGroup_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                 GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Search Groups --> btnAddSearchGroup_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddSearchGroupErrorLogs);
             }
        } 
        #endregion

        

        //****************Start Process**********************//
        
        #region btnStop_Click
        private void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                CampainGroupCreate.Campaign_IsStop_AccountCraterThread = true;
                foreach (Thread item in CampainGroupCreate.Campaign_lstAccountCraterThread)
                {
                    try
                    {
                        item.Abort();
                    }
                    catch
                    {
                    }
                }
                AddToListCampainAccountCreator("-------------------------------------------------------------------------------------------------------------------------------");
                AddToListCampainAccountCreator("[ " + DateTime.Now + " ] => [ PROCESS STOPPED ]");
                AddToListCampainAccountCreator("-------------------------------------------------------------------------------------------------------------------------------");
            }
            catch { }
        }

       

     }
}
        #endregion
