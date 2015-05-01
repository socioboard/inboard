
#region namespaces

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using InBoardPro;
using BaseLib.DB_Repository;
using ManageConnections;
using System.Web;
using System.Net;
using Microsoft.Win32;
using WatiN.Core;
using Campaign_GrpRequestAddConnections;
using Groups;
using InBoardProGetData;
using ProfileManager;
using Others;
using BaseLib;
using Campaign;
#endregion

namespace InBoardPro
{
    public partial class frmMain : System.Windows.Forms.Form
    {

        #region Global Declaration

        GlobusHttpHelper HttpHelper = new GlobusHttpHelper();
        ClsSelect ObjSelectMethod = new ClsSelect();
        frmAccounts FrmAccount = new frmAccounts();
        List<string> listFirstName = new List<string>();
        List<string> listLastName = new List<string>();
        List<string> Email = new List<string>();
        List<string> Country = new List<string>();
        List<string> Pincode = new List<string>();
        List<string> Jobtitle = new List<string>();
        List<string> Company = new List<string>();
        List<string> Industry = new List<string>();
        List<string> ProxyList = new List<string>();
        List<string> CollegeList = new List<string>();
        List<string> listLogo { get; set; }
        List<string> listGroupName { get; set; }
        List<string> listGroupURLs_CreateGroup { get; set; }
        List<string> listSummary { get; set; }
        List<string> listDesciption { get; set; }
        List<string> listWebsite { get; set; }
        List<string> GrpMemSubjectlist = new List<string>();
        List<string> GrpMemMessagelist = new List<string>();
        List<string> ListGrpDiscussion = new List<string>();
        List<string> ListGrpMoreDetails = new List<string>();
        List<string> ListAttachLinkGrpDetails = new List<string>();
        List<string> ListMsgTo = new List<string>();
        List<string> ListMsgSubject = new List<string>();
        List<string> ListMsgBody = new List<string>();
        List<string> ListGropKeyword = new List<string>();
        List<string> MemId = new List<string>();
        List<string> GroupId = new List<string>();
        List<string> list_pvtProxy = new List<string>();
        List<string> lstPublicProxyWOTest = new List<string>();
        Queue<string> lstLogoImage = new Queue<string>();
        List<string> lstEndorseUserIds = new List<string>();
        List<Thread> lstworkingThread = new List<Thread>();
        List<Thread> lstGroupMessageThread = new List<Thread>();
        List<Thread> lstAddConnectionThread = new List<Thread>();
        List<Thread> lstSentFriendMessageThread = new List<Thread>();
        List<Thread> lstSearchconnectionThread = new List<Thread>();
        List<Thread> lstEmailInviteThread = new List<Thread>();
        List<Thread> lstJoinFriendGroupThread = new List<Thread>();
        List<Thread> lstJoinSearchGroupThread = new List<Thread>();
        List<Thread> lstGroupUpdateThread = new List<Thread>();
        List<Thread> lstComposeMessageThread = new List<Thread>();
        List<Thread> lstStatusUpdateThread = new List<Thread>();
        List<Thread> lstAccountCraterThread = new List<Thread>();
        List<Thread> lstEndorsementThread = new List<Thread>();
        List<Thread> lstStopPrivateProxyTest = new List<Thread>();
        List<Thread> lstStopPublicProxyTest = new List<Thread>();
        List<Thread> lstProfileRankThread = new List<Thread>();
        List<Thread> lstShareThread = new List<Thread>();
        public static readonly Queue<string> queueproxy = new Queue<string>();
        public static readonly object lockr_Proxy_AccCreator = new object();
        //public static readonly object proxy_lockerdeq = new object();
        public static List<string> lstProxy_AccCreator = new List<string>();
        Dictionary<string, string> CountryCode = new Dictionary<string, string>();
        public static readonly object lockr_lstBoxEndorsePeople = new object();
        int min = 10;
        bool IsStop_Proxy = false;
        bool proxy_stop = false;
        bool IsStop_AccountCraterThread = false;
        bool IsStop_InBoardProGetDataThread = false;
        string _Country_AccountCreatorUsingIE = string.Empty;
        string _AccountType_AccountCreatorUsingIE = string.Empty;
        string _Industry_AccountCreatorUsingIE = string.Empty;
        bool _IsIEVisible = false;
        int _WaitForCompleteTimeOut = 30 * 1000;
        bool CheckNetConn = false;
        bool IsStop = false;
        bool _IsKeyword_LinkedinSearch = false;
        bool _IsProfileURL_LinkedinSearch = false;
        bool isSpinTrue = false;
        string Language = "English;en,Spanish;es,German;de,French;fr,Italian;it,Portuguese;pt,Dutch;nl,Bahasa Indonesia;in,Malay;ms,Romanian;ro,Russian;ru,Turkish;tr,Swedish;sv,Polish;pl,Others;_o";
        string RelationshipList = "N:All LinkedIn Members,F:1st Connections,S:2nd Connections,A:Group Members,O:3rd + Everyone Else";
        string IndustryList = "Accounting;47,Airlines/Aviation;94,Alternative Dispute Resolution;120,Alternative Medicine;125,Animation;127,Apparel & Fashion;19,Architecture & Planning;50,Arts and Crafts;111,Automotive;53,Aviation & Aerospace;52,Banking;41,Biotechnology;12,Broadcast Media;36,Building Materials;49,Business Supplies and Equipment;138,Capital Markets;129,Chemicals;54,Civic & Social Organization;90,Civil Engineering;51,Commercial Real Estate;128,Computer & Network Security;118,Computer Games;109,Computer Hardware;3,Computer Networking;5,Computer Software;4,Construction;48,Consumer Electronics;24,Consumer Goods;25,Consumer Services;91,Cosmetics;18,Dairy;65,Defense & Space;1,Design;99,Education Management;69,E-Learning;132,Electrical/Electronic Manufacturing;112,Entertainment;28,Environmental Services;86,Events Services;110,Executive Office;76,Facilities Services;122,Farming;63,Financial Services;43,Fine Art;38,Fishery;66,Food & Beverages;34,Food Production;23,Fund-Raising;101,Furniture;26,Gambling & casinos;29,Glass, Ceramics & Concrete;145,Government Administration;75,Government Relations;148,Graphic Design;140,Health, Wellness and Fitness;124,Higher Education;68,Hospital & Health Care;14,Hospitality;31,Human Resources;137,Import and Export;134,Individual & Family Services;88,Industrial Automation;147,Information Services;84,Information Technology and Services;96,Insurance;42,International Affairs;74,International Trade and Development;141,Internet;6,Investment Banking;45,Investment Management;46,Judiciary;73,Law Enforcement;77,Law Practice;9,Legal Services;10,Legislative Office;72,Leisure, Travel & Tourism;30,Libraries;85,Logistics and Supply Chain;116,Luxury Goods & Jewelry;143,Machinery;55,Management Consulting;11,Maritime;95,Marketing and Advertising;80,Market Research;97,Mechanical or Industrial Engineering;135,Media Production;126,Medical Devices;17,Medical Practice;13,Mental Health Care;139,Military;71,Mining & Metals;56,Motion Pictures and Film;35,Museums and Institution;37,Music;115,Nanotechnology;114,Newspapers;81,Nonprofit Organization Management;100,Oil & Energy;57,Online Media;113,Outsourcing/Offshoring;123,Package/Freight Delivery;87,Packaging and Containers;146,Paper & Forest Products;61,Performing Arts;39,Pharmaceuticals;15,Philanthropy;131,Photography;136,Plastics;117,Political Organization;107,Primary/Secondary Education;67,Printing;83,Professional Training & Coaching;105,Program Development;102,Public Policy;79,Public Relations and Communications;98,Public Safety;78,Publishing;82,Railroad Manufacture;62,Ranching;64,Real Estate;44,Recreational Facilities and Services;40,Religious Institutions;89,Renewables & Environment;144,Research;70,Restaurants;32,Retail;27,Security and Investigations;121,Semiconductors;7,Shipbuilding;58,Sporting Goods;20,Sports;33,Staffing and Recruiting;104,Supermarkets;22,Telecommunications;8,Textiles;60,Think Tanks;130,Tobacco;21,Translation and Localization;108,Transportation/Trucking/Railroad;92,Utilities;59,Venture Capital & Private Equity;106,Veterinary;16,Warehousing;93,Wholesale;133,Wine and Spirits;142,Wireless;119,Writing and Editing;103";
        string Functionlist = "all:All Functions,1:Academics,2:Accounting,3:Administrative,4:Business development,5:Buyer,6:Consultant,7:Creative,8:Engineering,9:Entrepreneur,10:Finance,11:Human resources,12:Information technology,13:Legal,14:Marketing,15:Medical,16:Operations,17:Product,18:Public relations,19:Real estate,20:Sales,21:Support";
        string Functionlistrecruiter = "all:All Functions,1:Accounting,2:Administrative,3:Arts & Design,4:Business development,5:Community and Social service,6:Consultant,7:Education,8:Engineering,9:Entrepreneur,10:Finance,11:Healthcare Services,12:Human resources,13:Information technology,14:Legal,15:Marketing,16:Media and communication,17:Military and protective,18:Operation,19:product management ,20:Program and product management,21:Purchasing,22:Quality assurance,23:Real estate,24:Researh,25:Sales,26:Support";
        
        
        
        
        string companysizelist = "all:All Company Sizes,1:1-10,2:11-50,3:51-200,4:201-500,5:501-1000,6:1001-5000,7:5001-10000,8:10000+";
        string senioritylevel = "all:All Seniority Levels,0:Manager,1:Owner,2:Partner,3:CXO,4:VP,5:Director,6:Senior,7:Entry,8:Students & Interns,9:Volunteer";

        string senioritylevelRecruiterType = "all:All Seniority Levels,10:Owner,9:Partner,8:CXO,7:VP,6:Director,5:Manager,4:Senior,3:Entry,2:Training ,1:Unpaid";


        string IntrestedinList = "select-all:All LinkedIn Members,1:Potential employees,2:Consultants/contractors,4:Entrepreneurs,8:Hiring managers,16:Industry experts,32:Deal-making contacts,64:Reference check,128:Reconnect";
        string expList = "1:Less than 1 year,2:1 to 2 years,3:3 to 5 years,4:6 to 10 years,5:More than 10 years";
        string fortuneList = "select-all:All Companies,1:Fortune 50,2:Fortune 51-100,3:Fortune 101-250,4:Fortune 251-500,5:Fortune 501-1000";
        string RecentlyjoinedList = "select-all:Any Time,1:1 day ago,2:2-7 days ago,3:8-14 days ago,4:15-30 days ago,5:1-3 months ago";
        string WithingList = "10:10 mi (15km),25:25 mi (40 km),35:35 mi (55 km),50:50 mi (80 km),75:75 mi (120 km),100:100 mi (160 km)";
        string TitleValue = "CP:Current or past,C:Current,P:Past,PNC:Past not current";
        string openGrp = string.Empty;
        string MemGrp = string.Empty;
        string FromemailId = string.Empty;
        string FromEmailNam = string.Empty;
        string MemID = string.Empty;
        string AddGroupGid = string.Empty;
        string messageSpin = string.Empty;
        string statusSpin = string.Empty;
        string msgBodycomposePass = string.Empty;
        public static int noOfRepliesPerKeyword = 10;
        public static string SearchGroup = string.Empty;

        public bool preventMsgSameUser = false;
        public bool preventMsgGlobalUser = false;

        public bool preventMsgSameGroup = false;
        public bool preventMsgWithoutGroup = false;
        public bool preventMsgGlobal = false;



        //>>>>>>>>>>>>>>> Proxy Setting>>>>>>>>>>>>>>>>>>>>>>>>
        Int64 workingproxiesCount = 0;
        int numberOfProxyThreads = 4;
        int countParseProxiesThreads = 0;
        int accountsPerProxy = 10;
        int EmailCounter = 0;
        public static int WaitAndReplyInterval = 10 * 1000 * 2 * 30; //in minutes
        Dictionary<string, string> Result = new Dictionary<string, string>();
        ProxyUtilitiesFromDataBase proxyFetcher = new ProxyUtilitiesFromDataBase();
        private static readonly object proxiesThreadLockr = new object();
        public static Queue<string> queWorkingProxies = new Queue<string>();
        public static readonly object proxyListLockr = new object();
        public static readonly object locker_LogoIamge = new object();
        //public static Queue<string> queWorkingProxies { get; set; }
        string msg = string.Empty;
        string GroupType = "Alumni Group:alumni;Corporate Group:corporate;Conference Group:conference;Networking Group:network;Nonprofit Group:phil;Professional Group:prof;Other...:other";
        List<string> MessagelistCompose = new List<string>();
        List<string> subjectlistCompose = new List<string>();
        List<string> UpdatelistStatusUpdate = new List<string>();
        public static Dictionary<string, string> OpenGroupDtl = new Dictionary<string, string>();
        Dictionary<string, Dictionary<string, string>> LinkdInContacts = new Dictionary<string, Dictionary<string, string>>();
        Dictionary<string, Dictionary<string, string>> MessageContacts = new Dictionary<string, Dictionary<string, string>>();
        string _proxyAddress = string.Empty;
        string _proxyPort = string.Empty;
        string _proxyUsername = string.Empty;
        string _proxyPassword = string.Empty;
        int accountCounter = -1;
        //>>>>>>>>>>>>>>For Image painting in form>>>>>>>>>>>>>>>>>>>>>>
        public System.Drawing.Image image;
        private System.Drawing.Image recSideImage;

        #endregion

        #region Account Creator Variable

        public List<string> lstAccountType = new List<string>();  //string.Empty;
        public List<string> lstIndustry = new List<string>();//string.Empty;
        public List<string> lstCountry = new List<string>();//string.Empty;

        #endregion

        #region Account Checker

        public List<string> lstAccountCheckerEmail = new List<string>();

        #endregion

        #region CreateGroupDeclarations

        public static Queue<string> Que_Message_Post = new Queue<string>();
        public static Queue<string> Que_Grpname_Post = new Queue<string>();
        public static Queue<string> Que_GrpSummary_Post = new Queue<string>();
        public static Queue<string> Que_GrpDesc_Post = new Queue<string>();
        public static Queue<string> Que_Grpwebsite_Post = new Queue<string>();

        public static readonly object Locker_Message_Post = new object();
        public static readonly object Locker_Grpname_Post = new object();
        public static readonly object Locked_GrpSummary_Post = new object();
        public static readonly object Locked_GrpDesc_Post = new object();
        public static readonly object Locked_Grpwebsite_Post = new object();

        #endregion

        #region frmMain()

        public frmMain()
        {
            InitializeComponent();
        }

        #endregion

        //** Wall Poster Module ************************************************************************************

        #region Wall Poster

        List<string> _lstWallMessage = new List<string>();

        #endregion

        //** Account Info Module ************************************************************************************

        #region Account Info Module

        #endregion

        //** Manage Connection ******************************************************************

        #region Manage Connection

        List<string> _lstInviteEmailConnection = new List<string>();
        List<string> _lstConnectionSearchKeyword = new List<string>();
        List<string> lstInviteEmailConnection_temp = new List<string>();

        ManageConnections.InvitationEmails obj = new ManageConnections.InvitationEmails();

        #endregion

        //** Profile Manager Module************************************************************************

        #region Profile Manager Module

        List<string> _lstProfilePhoto = new List<string>();
        List<string> _lstSummaryGoals = new List<string>();
        List<string> _lstSummarySpecialties = new List<string>();

        #endregion

        //** Join Group Module************************************************************************

        #region

        /// <summary>
        /// Selected Groups from CheckedListBox >>> chkExistGroup
        /// </summary>
        private List<string> lstExistGroup = new List<string>();

        #endregion

        #region LinkedinSearch Gloval Variables

        bool _IsStop_LinkedinSearch = false;

        string _LinkedinSearchSelectedEmailId = string.Empty;
        string _LinkedinSearchSelectedcmbLinkedinSearch = string.Empty;
        string _LinkedinSearchtxtSearchItem = string.Empty;

        List<string> lstLinkedinSearchProfileURL = new List<string>();
        List<Thread> lstLinkedinSearchThraed = new List<Thread>();
        List<Thread> lstInBoardProGetDataThraed = new List<Thread>();
        List<string> lstLinkedinShareWebsiteUrl = new List<string>();

        List<string> lstLinkedinJobScraperInputUrl = new List<string>();

        #endregion

        #region ProfileRank Global Variables
        bool _IsStop_ProfileRank = false;
        string _ProfileRankSelectedEmailId = string.Empty;
        #endregion

        #region Share GlobalVariables
        bool _isStop_Share = false;
        string shareSelectedEmailId = string.Empty;
        #endregion

        #region frmMain_Load

        private void frmMain_Load(object sender, EventArgs e)
        {
            try
            {
                frmCampaignScraper.CampaignStopLogevents.addToLogger += new EventHandler(CampaignnameLog);
                GroupStatus.loggerFriendsGroup.addToLogger += new EventHandler(loggerGroupStatusEvent);
                CheckVersion();
                CopyFolder();
                CopyDatabase();

                btnLinkedinSearchProfileURLLoad.Enabled = false;
                //tabMain.TabPages.Remove(tabViewProfileRank);
                //tabMain.TabPages.Remove(tabInboxManager);
                FrmCaptchaLogin.LoadDecaptchaSettings();
                //image = Properties.Resources.background;
                //recSideImage = Properties.Resources.background;

                image = Properties.Resources.background;
                recSideImage = Properties.Resources.background;

                Color clr = ColorTranslator.FromHtml("#F1713C");
                
                CopyUpdateStatus();
                DeleteManageAddConnection();
                Uploaddata();

                txtExportedFile1.Text = Globals.DesktopFolder;
                txtExportedFile2.Text = Globals.DesktopFolder;
                // calling btnSubmitCapchaLogin_Click(sender, e) of FrmCaptchaLogin;
                //FrmCaptchaLogin obj_FrmCaptchaLogin = new FrmCaptchaLogin();
                //obj_FrmCaptchaLogin.Show();

                //obj_FrmCaptchaLogin.btnSubmitCapchaLogin_Click(sender, e);

                //obj_FrmCaptchaLogin.Close();

                #region Event Method subscription
                frmAccounts.AccountsLogEvents.addToLogger += new EventHandler(AccountsLogEvents_addToLogger);
                obj.linkedinLoginAndLogout.LoginEvent.addToLogger += new EventHandler(LoginEvent_addToLogger);
                LinkedInScrape.logger.addToLogger += new EventHandler(ScrapeEvent_addToLogger);
                GroupStatus.logger.addToLogger += new EventHandler(GroupStatus_addToLogger); // AddLoggerGroupAdd
                LinkedinSearch.loggersearch.addToLogger += new EventHandler(LinkedinSearchLogEvents_addToLogger);
                GroupStatus.logger.addToLogger += new EventHandler(LinkedingrpLogEvents_addToLogger);
                LinkedinSearch.loggerscrap.addToLogger += new EventHandler(linkedinFilterSearchlogEvent_addToLogger);
                ProfileRank.loggerrank.addToLogger += new EventHandler(ProfileRankLogEvents_addToLogger);


                #endregion

                copyGroupCreator();

                Dictionary<string, string> IndustryCode = new Dictionary<string, string>();
                Dictionary<string, string> Langaugecode = new Dictionary<string, string>();

                ClsSelect ObjSelectMethod = new ClsSelect();
                CountryCode = ObjSelectMethod.getCountry();

                IndustryCode = ObjSelectMethod.getIndustry();
                Langaugecode = ObjSelectMethod.getLangauge();

                foreach (KeyValuePair<string, string> pair in CountryCode)
                {
                    try
                    {
                        CombScraperCountry.Items.Add(pair.Value);


                        lstCountry.Add(pair.Value);
                        CampainGroupCreate.Campaign_lstCountry.Add(pair.Value);
                        //CombCountry.Items.Add(pair.Value);
                    }
                    catch
                    {
                    }
                }

                //CombCountry.SelectedIndex = 1;

                try
                {
                    CombScraperCountry.SelectedIndex = 1;
                    combScraperLocation.SelectedIndex = 1;
                    CombScraperCountry.SelectedIndex = 0;
                    combScraperLocation.SelectedIndex = 0;
                    cmbMsgFrom.SelectedIndex = 1;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error >>> " + ex.StackTrace);
                }
                foreach (KeyValuePair<string, string> pair in IndustryCode)
                {
                    try
                    {
                        lstIndustry.Add(pair.Value);
                        CampainGroupCreate.Campaign_lstIndustry.Add(pair.Value);
                    }
                    catch
                    {
                    }
                }

                foreach (KeyValuePair<string, string> pair in Langaugecode)
                {
                    cmbLanguage.Items.Add(pair.Key);
                }

                ConnectUsing_Search.ConnectSearchLogEvents.addToLogger += new EventHandler(loggerAddConnection_addToLogger);
                InvitationEmails.InviteConnectionLogEvents.addToLogger += new EventHandler(loggerAddConnection_addToLogger);
                EndorsePeople.logger.addToLogger += new EventHandler(EndorsePeople_addToLogger);
                GroupStatus.loggerEndorseCampaign.addToLogger += new EventHandler(EndorsePeople_addToLogger);
                //comboBoxlocation.SelectedItem = comboBoxlocation.Items[0];
                //CombCountry.SelectedItem = CombCountry.Items[1];
                //listStatusMessages = new List<string>();

                LinkedinLogin Login = new LinkedinLogin();

                Login.logger.addToLogger += new EventHandler(logger_addToLogger);

                ToolTip Tooltip = new ToolTip();

                Tooltip.AutoPopDelay = 5000;
                Tooltip.InitialDelay = 1000;
                Tooltip.ReshowDelay = 500;

                Tooltip.ShowAlways = true;

                Tooltip.SetToolTip(this.linkLblJoinGroupUrl, "New Feature Join Group Using Url");
                Tooltip.SetToolTip(this.linkbtnScrapeinExcelInput, "LinkedIn Scraper Using Excel Input File With Mode1 and Mode2 option");

            }
            catch
            {
            }
        }

        #endregion


        #region check version

        #region Checking OS

        static bool is64BitProcess = (IntPtr.Size == 8);
        static bool is64BitOperatingSystem = is64BitProcess || InternalCheckIsWow64();

        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsWow64Process(
            [In]
            IntPtr hProcess,
            [Out]
            out bool wow64Process);

        public static bool InternalCheckIsWow64()
        {
            if ((Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor >= 1) ||
                Environment.OSVersion.Version.Major >= 6)
            {
                using (System.Diagnostics.Process p = System.Diagnostics.Process.GetCurrentProcess())
                {
                    bool retVal;
                    if (!IsWow64Process(p.Handle, out retVal))
                    {
                        return false;
                    }
                    return retVal;
                }
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region CheckVersion()
        private void CheckVersion()
        {
            try
            {
                string textFileLocationOnServer = "http://linkeddominator.com/licensing/LDLatestVersion.txt";

                if (is64BitOperatingSystem)
                {
                    textFileLocationOnServer = "http://linkeddominator.com/licensing/LDLatestVersion_64.txt";
                }

                string thisVersionNumber = GetAssemblyNumber();

                string verstatus = LinkedInManager.Licence_Details.Split('&')[1];

                this.Text = this.Text + "-" + verstatus + " (" + thisVersionNumber + ")";

                GlobusHttpHelper httpHelper = new GlobusHttpHelper();
                string textFileData = httpHelper.getHtmlfromUrl1(new Uri(textFileLocationOnServer));

                string latestVersion = Regex.Split(textFileData, "<:>")[0];
                string updateVersionPath = Regex.Split(textFileData, "<:>")[1];

                if (thisVersionNumber == latestVersion)
                {
                    MessageBox.Show("You have the Updated LD Version : " + thisVersionNumber, "Information");
                }
                else
                {
                    if (MessageBox.Show("An Updated Version Available - Do you Want to Upgrade!", "Update Available", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start("iexplore", updateVersionPath);

                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                AddLoggerGeneral("[ " + DateTime.Now + " ] => [ Error in Auto Update Module ]");
            }
        }
        #endregion

        #region CheckVersion1
        private void CheckVersion1()
        {
            try
            {
                string thisVersionNumber = GetAssemblyNumber();

                string textFileLocationOnServer = "http://faced.extrem-hosting.net/LDLatestVersion.txt";//"http://cmswebusa.com/developers/SumitGupta/FDLatestVersion.txt";

                GlobusHttpHelper httpHelper = new GlobusHttpHelper();
                string textFileData = httpHelper.getHtmlfromUrl1(new Uri(textFileLocationOnServer));

                string latestVersion = Regex.Split(textFileData, "<:>")[0];
                string updateVersionPath = Regex.Split(textFileData, "<:>")[1];

                if (thisVersionNumber == latestVersion)
                {
                    MessageBox.Show("You have the Updated LD Version : " + thisVersionNumber, "Information");
                }
                else
                {
                    if (MessageBox.Show("An Updated Version Available - Do you Want to Upgrade!", "Update Available", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start("iexplore", updateVersionPath);

                        this.Close();
                        //FrmUpgrade.updateVersionPath = updateVersionPath;
                        //FrmUpgrade frm_updater = new FrmUpgrade();
                        //frm_updater.ShowDialog();
                        //if (FrmUpgrade.IsUpdated)
                        //{
                        //    //Install the software 
                        //    //Process Install = new Process();
                        //    string Path = Globals.FD_DownloadSetupPath + @"\setup.exe";
                        //    Process.Start(Path);
                        //    this.Close();
                        //    Application.Exit();
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                AddLoggerGeneral("[ " + DateTime.Now + " ] => [ Error in Auto Update Module ]");
            }
        }
        #endregion

        #region GetAssemblyNumber
        public string GetAssemblyNumber()
        {
            string appName = Assembly.GetAssembly(this.GetType()).Location;
            AssemblyName assemblyName = AssemblyName.GetAssemblyName(appName);
            string versionNumber = assemblyName.Version.ToString();
            return versionNumber;
        }
        #endregion

        #endregion

        #region frmMain_KeyDown Shortcut for all tabs option

        private void frmMain_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.)
            //{
            //if (e.KeyCode == Keys.F1)
            //{
            //    tabMain.SelectTab(0);
            //    e.Handled = true;
            //}
            //else if (e.KeyCode == Keys.F2)
            //{
            //    tabMain.SelectTab(1);
            //    e.Handled = true;
            //}
            //else if (e.KeyCode == Keys.F3)
            //{
            //    tabMain.SelectTab(2);
            //    e.Handled = true;
            //}
            //else if (e.KeyCode == Keys.F4)
            //{
            //    tabMain.SelectTab(3);
            //    e.Handled = true;
            //}
            //else if (e.KeyCode == Keys.F5)
            //{
            //    tabMain.SelectTab(4);
            //    e.Handled = true;
            //}
            //else if (e.KeyCode == Keys.F6)
            //{
            //    tabMain.SelectTab(5);
            //    e.Handled = true;
            //}
            //else if (e.KeyCode == Keys.F7)
            //{
            //    tabMain.SelectTab(6);
            //    e.Handled = true;
            //}
            //else if (e.KeyCode == Keys.F8)
            //{
            //    tabMain.SelectTab(7);
            //    e.Handled = true;
            //}
            //else if (e.KeyCode == Keys.F9)
            //{
            //    tabMain.SelectTab(8);
            //    e.Handled = true;
            //}
            //else if (e.KeyCode == Keys.F10)
            //{
            //    tabMain.SelectTab(9);
            //    e.Handled = true;
            //}
            //else if (e.KeyCode == Keys.F11)
            //{
            //    tabMain.SelectTab(10);
            //    e.Handled = true;
            //}
            //else if (e.KeyCode == Keys.F12)
            //{
            //    //tabMai.SelectTab(11);

            //    tabMain.SelectTab("tabProxySetting");
            //    e.Handled = true;
            //}

        }

        #endregion

        #region Uploaddata()
        public void Uploaddata()
        {
            txtScraperMinDelay.Enabled = true;
            txtScraperMinDelay.Visible = true;
            checkedListBoxFortune1000.Enabled = false;
            checkedListBoxCompanySize.Enabled = false;
            checkedListBoxInterestedIN.Enabled = false;
            checkedListBoxSeniorlevel.Enabled = false;
            checkedListBoxRecentlyJoined.Enabled = false;
            GetDataForPrimiumAccount.Enabled = false;
            checkedListFunction.Enabled = false;

            string[] arrayRelationship = Regex.Split(RelationshipList, ",");
            foreach (string item in arrayRelationship)
            {
                string[] arrayrelat = Regex.Split(item, ":");
                if (arrayrelat.Length == 2)
                {
                    checkedListRelationship.Items.Add(arrayrelat[1]);
                }
            }
            string[] arrayLanguage = Regex.Split(Language, ",");
            foreach (string item in arrayLanguage)
            {
                string[] arraylang = Regex.Split(item, ";");
                if (arraylang.Length == 2)
                {
                    checkedListBoxLanguage.Items.Add(arraylang[0]);
                }
            }
            string[] arrayIndustry = Regex.Split(IndustryList, ",");
            foreach (string item in arrayIndustry)
            {
                string[] arrayInds = Regex.Split(item, ";");
                if (arrayInds.Length == 2)
                {
                    checkedListBoxIndustry.Items.Add(arrayInds[0]);
                }
            }



            if (SearchCriteria.AccountType == "RecuiterType")
            {
                Functionlist = Functionlistrecruiter;
            }
            string[] arrayFunction = Regex.Split(Functionlist, ",");
            foreach (string item in arrayFunction)
            {
                string[] arrayInds = Regex.Split(item, ":");
                if (arrayInds.Length == 2)
                {
                    checkedListFunction.Items.Add(arrayInds[1]);
                }
            }

            if (SearchCriteria.AccountType == "RecuiterType")
            {
                senioritylevel = senioritylevelRecruiterType;
            }
            string[] arraySenoirtyLevel = Regex.Split(senioritylevel, ",");
            foreach (string item in arraySenoirtyLevel)
            {
                string[] arrayInds = Regex.Split(item, ":");
                if (arrayInds.Length == 2)
                {
                    checkedListBoxSeniorlevel.Items.Add(arrayInds[1]);
                }
            }
            string[] arrayCompanysize = Regex.Split(companysizelist, ",");
            foreach (string item in arrayCompanysize)
            {
                string[] arrayInds = Regex.Split(item, ":");
                if (arrayInds.Length == 2)
                {
                    checkedListBoxCompanySize.Items.Add(arrayInds[1]);
                }
            }
            string[] arrayIntrestedin = Regex.Split(IntrestedinList, ",");
            foreach (string item in arrayIntrestedin)
            {
                string[] arrayInds = Regex.Split(item, ":");
                if (arrayInds.Length == 2)
                {
                    checkedListBoxInterestedIN.Items.Add(arrayInds[1]);
                }
            }
            string[] arrayexpList = Regex.Split(expList, ",");
            foreach (string item in arrayexpList)
            {
                string[] arrayInds = Regex.Split(item, ":");
                if (arrayInds.Length == 2)
                {
                    GetDataForPrimiumAccount.Items.Add(arrayInds[1]);
                }
            }
            string[] arrayfortuneList = Regex.Split(fortuneList, ",");
            foreach (string item in arrayfortuneList)
            {
                string[] arrayInds = Regex.Split(item, ":");
                if (arrayInds.Length == 2)
                {
                    checkedListBoxFortune1000.Items.Add(arrayInds[1]);
                }
            }
            string[] arrayRecentlyjoined = Regex.Split(RecentlyjoinedList, ",");
            foreach (string item in arrayRecentlyjoined)
            {
                string[] arrayInds = Regex.Split(item, ":");
                if (arrayInds.Length == 2)
                {
                    checkedListBoxRecentlyJoined.Items.Add(arrayInds[1]);
                }
            }
            string[] arraywithin = Regex.Split(WithingList, ",");
            foreach (string item in arraywithin)
            {
                string[] arrayPostalwithin = Regex.Split(item, ":");
                if (arrayPostalwithin.Length == 2)
                {
                    cmbboxWithin.Items.Add(arrayPostalwithin[1]);
                }
            }
            string[] arrayTitleValue = Regex.Split(TitleValue, ",");
            foreach (string item in arrayTitleValue)
            {
                string[] arraytitleValue = Regex.Split(item, ":");
                if (arraytitleValue.Length == 2)
                {
                    cmbboxTitle.Items.Add(arraytitleValue[1]);
                    cmbboxCompanyValue.Items.Add(arraytitleValue[1]);
                }
            }



            if (SearchCriteria.AccountType == "RecuiterType")
            {
                txtScraperMinDelay.Visible = true;
                checkedListBoxFortune1000.Enabled = true;
                checkedListBoxCompanySize.Enabled = true;
                checkedListBoxInterestedIN.Enabled = true;
                checkedListBoxSeniorlevel.Enabled = true;
                checkedListBoxRecentlyJoined.Enabled = true;
                GetDataForPrimiumAccount.Enabled = true;
                checkedListFunction.Enabled = true;
            }

            cmbboxCompanyValue.SelectedIndex = 0;
            cmbboxTitle.SelectedIndex = 0;

        }
        #endregion

        #region DeleteManageAddConnection()
        private void DeleteManageAddConnection()
        {
            try
            {
                string Querystring = "Delete From tb_ManageAddConnection";
                DataBaseHandler.DeleteQuery(Querystring, "tb_ManageAddConnection");
            }
            catch { }
        }
        #endregion

        #region Updatestatus

        private void CopyUpdateStatus()
        {
            try
            {
                clsDBQueryManager DataBase = new clsDBQueryManager();
                DataTable dt = DataBase.SelectSettingData();
                foreach (DataRow row in dt.Rows)
                {
                    if ("StatusUpdate" == row[0].ToString() && "StatusMessage" == row[1].ToString())
                    {
                        txtStatusUpd.Text = StringEncoderDecoder.Decode(row[2].ToString());
                        if (File.Exists(txtStatusUpd.Text))
                        {
                            List<string> temp = GlobusFileHelper.ReadFiletoStringList(txtStatusUpd.Text);
                            foreach (string item in temp)
                            {
                                if (!string.IsNullOrEmpty(item.Replace(" ", "").Replace("\t", "")))
                                {
                                    listStatusMessages.Add(item);
                                }
                            }
                            AddLoggerStatusUpdate("[ " + DateTime.Now + " ] => [ " + listStatusMessages.Count + " Status Update Messages loaded ]");
                            break;
                        }
                        else
                        {
                            AddLoggerStatusUpdate("[ " + DateTime.Now + " ] => [ File Does not Exsist ]");
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Status Update --> CopyUpdateStatus() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Status Update --> CopyUpdateStatus() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinStatusUpdateErrorLogs);
            }
        }

        #endregion
        
        #region copyGroupCreator

        public void copyGroupCreator()
        {
            listGroupName = new List<string>();
            listWebsite = new List<string>();
            listSummary = new List<string>();
            listDesciption = new List<string>();

            string groupNameSettings = string.Empty;
            string groupWebsiteSettings = string.Empty;
            string groupSummarySettings = string.Empty;
            string groupDescriptionSetting = string.Empty;

            clsDBQueryManager DataBase = new clsDBQueryManager();
            DataTable dt = DataBase.SelectSettingData();
            foreach (DataRow row in dt.Rows)
            {
                if ("GroupName" == row[1].ToString())
                {
                    groupNameSettings = StringEncoderDecoder.Decode(row[2].ToString());
                }
                if ("GroupSummary" == row[1].ToString())
                {
                    groupSummarySettings = StringEncoderDecoder.Decode(row[2].ToString());
                }
                if ("GroupDescription" == row[1].ToString())
                {
                    groupDescriptionSetting = StringEncoderDecoder.Decode(row[2].ToString());
                }
                if ("GroupWebsite" == row[1].ToString())
                {
                    groupWebsiteSettings = StringEncoderDecoder.Decode(row[2].ToString());
                }
            }

            if (File.Exists(groupNameSettings))
            {
                listGroupName = GlobusFileHelper.ReadFiletoStringList(groupNameSettings);
                txtGroupName.Text = groupNameSettings;
                AddLoggerCreateGroup("[ " + DateTime.Now + " ] => [ " + listGroupName.Count + " Group Names loaded ]");
            }
            if (File.Exists(groupSummarySettings))
            {
                listSummary = GlobusFileHelper.ReadFiletoStringList(groupSummarySettings);
                txtSummary.Text = groupSummarySettings;
                AddLoggerCreateGroup("[ " + DateTime.Now + " ] => [ " + listSummary.Count + " Group Summary loaded ]");
            }
            if (File.Exists(groupDescriptionSetting))
            {
                listDesciption = GlobusFileHelper.ReadFiletoStringList(groupDescriptionSetting);
                txtDesc.Text = groupDescriptionSetting;
                AddLoggerCreateGroup("[ " + DateTime.Now + " ] => [ " + listDesciption.Count + " Group Description loaded ]");
            }
            if (File.Exists(groupWebsiteSettings))
            {
                listWebsite = GlobusFileHelper.ReadFiletoStringList(groupWebsiteSettings);
                txtWebsite.Text = groupWebsiteSettings;
                AddLoggerCreateGroup("[ " + DateTime.Now + " ] => [ " + listWebsite.Count + " Group Websites loaded ]");
            }
        }

        #endregion

        #region logger_addToLogger

        void logger_addToLogger(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eventArgs = e as EventsArgs;
                AddLoggerStatusUpdate(eventArgs.log);
            }
        }

        #endregion

        #region logger_SearchGroupaddToLogger

        void logger_SearchGroupaddToLogger(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eventArgs = e as EventsArgs;
                AddLoggerSearchGroup(eventArgs.log);
            }
        }

        #endregion

        #region logger_StatusUpdateaddToLogger

        void logger_StatusUpdateaddToLogger(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eventArgs = e as EventsArgs;
                AddLoggerStatusUpdate(eventArgs.log);
            }
        }

        #endregion

        #region logger_addGroupCreateToLogger

        void logger_addGroupCreateToLogger(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eventArgs = e as EventsArgs;
                AddLoggerCreateGroup(eventArgs.log);
            }
        }

        #endregion

        #region logEvents_addToLogger

        void logEvents_addToLogger(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eventArgs = e as EventsArgs;
            }
        }

        #endregion

        #region log_events_CreateGroup

        void log_events_CreateGroup(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eventArgs = e as EventsArgs;
                AddLoggerCreateGroup(eventArgs.log);
            }
        }

        #endregion

        #region CopyFolder()

        public void CopyFolder()
        {
            if (!Directory.Exists(Globals.DesktopFolder))
            {
                Directory.CreateDirectory(Globals.DesktopFolder);
            }
            if (!Directory.Exists(Globals.AppDataFolder))
            {
                Directory.CreateDirectory(Globals.AppDataFolder);
            }
            if (!Directory.Exists(Globals.profileRankFolder))
            {
                Directory.CreateDirectory(Globals.profileRankFolder);
            }
        }

        #endregion

        #region LoginEvent_addToLogger

        void LoginEvent_addToLogger(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eventArgs = e as EventsArgs;
                AddLoggerManageConnection(eventArgs.log);
            }
        }

        #endregion

        #region ScrapeEvent_addToLogger

        void ScrapeEvent_addToLogger(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eventArgs = e as EventsArgs;
                AddLoggerScrapeUsers(eventArgs.log);
            }
        }

        #endregion

        #region StatusUpdate_addToLogger

        void StatusUpdate_addToLogger(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eventArgs = e as EventsArgs;
                AddLoggerStatusUpdate(eventArgs.log);
            }
        }

        #endregion

        #region CopyDatabase()
        private void CopyDatabase()
        {
            try
            {
                string startUpDB = Application.StartupPath + "\\DB_InBoardPro.db";
                string localAppDataDB = Globals.path_AppDataFolder + "\\DB_InBoardPro.db";

                string startUpDB64 = Environment.GetEnvironmentVariable("ProgramFiles(x86)") + "\\DB_InBoardPro.db";

                if (!File.Exists(localAppDataDB))
                {
                    if (File.Exists(startUpDB))
                    {
                        try
                        {
                            File.Copy(startUpDB, localAppDataDB);
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message.Contains("Could not find a part of the path"))
                            {
                                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\InBoardPro");
                                File.Copy(startUpDB, localAppDataDB);
                                GlobusFileHelper.AppendStringToTextfileNewLine("Error : CopyDataBase1 : " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + " :: " + DateTime.Now, Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\InBoardPro\\ErrorDB.txt");
                            }
                        }
                    }
                    else if (File.Exists(startUpDB64))   //for 64 Bit
                    {
                        try
                        {
                            File.Copy(startUpDB64, localAppDataDB);
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message.Contains("Could not find a part of the path"))
                            {
                                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\InBoardPro");
                                File.Copy(startUpDB64, localAppDataDB);
                                GlobusFileHelper.AppendStringToTextfileNewLine("Error : CopyDataBase2 : " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + " :: " + DateTime.Now, Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\InBoardPro\\ErrorDB.txt");

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("Error : CopyDataBase3 : " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + " :: " + DateTime.Now, Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\InBoardPro\\ErrorDB.txt");
            }
        }

        //private void CopyDatabase()
        //{
        //    try
        //    {
        //        string startUpDB = Application.StartupPath + "\\DB_InBoardPro.db";
        //        string localAppDataDB = Globals.path_AppDataFolder + "\\DB_InBoardPro.db";

        //        if (!File.Exists(localAppDataDB))
        //        {
        //            if (File.Exists(startUpDB))
        //            {
        //                try
        //                {
        //                    File.Copy(startUpDB, localAppDataDB);
        //                }
        //                catch (Exception ex)
        //                {
        //                    if (ex.Message.Contains("Could not find a part of the path"))
        //                    {
        //                        Directory.CreateDirectory(Globals.path_AppDataFolder);
        //                        File.Copy(startUpDB, localAppDataDB);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch
        //    {
        //    }
        //}

        #endregion

        #region toolStripMenuItem1_Click
        public static bool IsAccountopen = true;
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (IsAccountopen)
            {
                IsAccountopen = false;
                LinkedInManager.linkedInDictionary.Clear();
                //frmAccounts NewAcc = new frmAccounts();
                //lbGeneralLogs.Items.Clear();
                //NewAcc.Show();
                Thread obj_BindData_LinkedinSearch_cmbEmailId = new Thread(BindData_LinkedinSearch_cmbEmailId);
                obj_BindData_LinkedinSearch_cmbEmailId.Start(new object[] { "tabMsgGroupMember", cmbAllUser });
            }

        }

        #endregion

        #region InviteConnectionLogEvents_addToLogger

        void InviteConnectionLogEvents_addToLogger(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eventArgs = e as EventsArgs;
                AddLoggerManageConnection(eventArgs.log);
            }
        }

        #endregion

        #region AccountsLogEvents_addToLogger

        void AccountsLogEvents_addToLogger(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eventArgs = e as EventsArgs;
                AddLoggerGeneral(eventArgs.log);
            }
        }

        #endregion

        #region AddToLogger_JoinSearchGroup

        void AddToLogger_JoinSearchGroup(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eventArgs = e as EventsArgs;
                AddLoggerSearchGroup(eventArgs.log);
            }
        }

        #endregion

        #region AddToLogger_GroupStatus

        void AddToLogger_GroupStatus(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eventArgs = e as EventsArgs;
                AddLoggerGroupStatus(eventArgs.log);
            }
        }

        #endregion

        #region AddLoggerManageConnection

        private void AddLoggerManageConnection(string log)
        {
            this.Invoke(new MethodInvoker(delegate
            {
                lbManageConnection.Items.Add(log);
                lbManageConnection.SelectedIndex = lbManageConnection.Items.Count - 1;
            }));
        }

        void loggerAddConnection_addToLogger(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eventArgs = e as EventsArgs;
                AddLoggerManageConnection(eventArgs.log);
            }
        }
        #endregion

        #region AddLoggerStatusUpdate

        private void AddLoggerStatusUpdate(string log)
        {
            this.Invoke(new MethodInvoker(delegate
            {
                lstStatusUpdate.Items.Add(log);
                lstStatusUpdate.SelectedIndex = lstStatusUpdate.Items.Count - 1;
            }));
        }

        #endregion

        #region LinkedinSearchLogEvents_addToLogger

        void LinkedinSearchLogEvents_addToLogger(object sender, EventArgs e)
        {
            try
            {
                if (e is EventsArgs)
                {
                    EventsArgs eventArgs = e as EventsArgs;
                    AddLoggerLinkedinSearch(eventArgs.log);
                }
            }
            catch
            {
            }
        }

        #endregion
        #region ProfileRankLogEvents_addToLogger

        void ProfileRankLogEvents_addToLogger(object sender, EventArgs e)
        {
            try
            {
                if (e is EventsArgs)
                {
                    EventsArgs eventArgs = e as EventsArgs;
                    AddLoggerProfileRank(eventArgs.log);
                }
            }
            catch
            {
            }
        }

        #endregion

        #region ShareLogEvents_addToLogger
        void ShareLogEvents_addToLogger(object sender, EventArgs e)
        {
            try
            {
                if (e is EventsArgs)
                {
                    EventsArgs eventArgs = e as EventsArgs;
                    AddLoggerShare(eventArgs.log);
                }
            }
            catch
            {
            }
        }
        #endregion

        #region LinkedinFilterSearch_LogEvents_addToLogger

        void linkedinFilterSearchlogEvent_addToLogger(object sender, EventArgs e)
        {
            try
            {
                if (e is EventsArgs)
                {
                    EventsArgs eventArgs = e as EventsArgs;
                    AddLoggerLinkedinSearchFilter(eventArgs.log);
                }
            }
            catch
            {
            }
        }

        #endregion

        #region LinkedingrpLogEvents_addToLogger

        void LinkedingrpLogEvents_addToLogger(object sender, EventArgs e)
        {
            try
            {
                if (e is EventsArgs)
                {
                    EventsArgs eventArgs = e as EventsArgs;
                    AddLoggerLinkedingrp(eventArgs.log);
                }
            }
            catch
            {
            }
        }

        private void AddLoggerLinkedingrp(string log)
        {
            try
            {
                if (lstGrpMemMsgLoger.InvokeRequired)
                {
                    lstGrpMemMsgLoger.Invoke(new MethodInvoker(delegate
                    {
                        lstGrpMemMsgLoger.Items.Add(log);
                        lstGrpMemMsgLoger.SelectedIndex = lstGrpMemMsgLoger.Items.Count - 1;
                    }));
                }
                else
                {
                    lstGrpMemMsgLoger.Items.Add(log);
                    lstGrpMemMsgLoger.SelectedIndex = lstGrpMemMsgLoger.Items.Count - 1;
                }
            }
            catch
            {
            }
        }

        #endregion

        #region AddLoggerLinkedinSearch

        private void AddLoggerLinkedinSearchFilter(string log)
        {
            try
            {
                if (lbLinkedinSearchLogger.InvokeRequired)
                {
                    lbLinkedinSearchLogger.Invoke(new MethodInvoker(delegate
                    {
                        lbLinkedinSearchLogger.Items.Add(log);
                        lbLinkedinSearchLogger.SelectedIndex = lbLinkedinSearchLogger.Items.Count - 1;
                    }));
                }
                else
                {
                    lbLinkedinSearchLogger.Items.Add(log);
                    lbLinkedinSearchLogger.SelectedIndex = lbLinkedinSearchLogger.Items.Count - 1;
                }
            }
            catch
            {
            }
        }

        #endregion

        #region AddLoggerLinkedinSearch

        private void AddLoggerLinkedinSearch(string log)
        {
            try
            {
                if (lbLinkedinSearchLogger.InvokeRequired)
                {
                    lbLinkedinSearchLogger.Invoke(new MethodInvoker(delegate
                    {
                        lbLinkedinSearchLogger.Items.Add(log);
                        lbLinkedinSearchLogger.SelectedIndex = lbLinkedinSearchLogger.Items.Count - 1;
                    }));
                }
                else
                {
                    lbLinkedinSearchLogger.Items.Add(log);
                    lbLinkedinSearchLogger.SelectedIndex = lbLinkedinSearchLogger.Items.Count - 1;
                }
            }
            catch
            {
            }
        }

        #endregion

        #region AddLoggerLinkedinPremiumScrapeUsers

        private void AddLoggerScrapeUsers(string log)
        {
            this.Invoke(new MethodInvoker(delegate
            {
                lstLoglinkdinPreScarper.Items.Add(log);
                lstLoglinkdinPreScarper.SelectedIndex = lstLoglinkdinPreScarper.Items.Count - 1;
            }));
        }

        #endregion

        #region AddLoggerAccountCreater

        public static readonly object lockr_lstBoxAccountsLogs = new object();


        #endregion

        #region AddloggerInBoardProGetData
        private void AddLoggerInBoardProGetData(string log)
        {
            try
            {
                try
                {
                    if (lstLoglinkdinPreScarper.Items.Count >= 1000)
                    {
                        lock (lockr_lstBoxAccountsLogs)
                        {
                            lstLoglinkdinPreScarper.Invoke(new MethodInvoker(delegate
                            {
                                lstLoglinkdinPreScarper.Items.Clear();
                            }));
                        }
                    }
                }
                catch
                {
                }

                if (lstLoglinkdinPreScarper.InvokeRequired)
                {
                    lstLoglinkdinPreScarper.Invoke(new MethodInvoker(delegate
                    {
                        lstLoglinkdinPreScarper.Items.Add(log);
                    }));
                }
                else
                {
                    lstLoglinkdinPreScarper.Items.Add(log);
                }
            }
            catch
            {
            }
        }

        #endregion

        #region AddLoggerGeneral

        private static readonly object lockr_lstBoxGeneralLogs = new object();

        private void AddLoggerGeneral(string log)
        {
            try
            {
                if (lbGeneralLogs.Items.Count >= 1000)
                {
                    lock (lockr_lstBoxGeneralLogs)
                    {
                        lbGeneralLogs.Invoke(new MethodInvoker(delegate
                        {
                            lbGeneralLogs.Items.Clear();
                        }));
                    }
                }
            }
            catch
            {
            }

            if (lbGeneralLogs.InvokeRequired)
            {
                lbGeneralLogs.Invoke(new MethodInvoker(delegate
                {
                    lbGeneralLogs.Items.Add(log);
                    lbGeneralLogs.SelectedIndex = lbGeneralLogs.Items.Count - 1;////Change lbGeneralLogs with  lstLogger 
                }));
            }
            else
            {
                lbGeneralLogs.Items.Add(log);
                lbGeneralLogs.SelectedIndex = lbGeneralLogs.Items.Count - 1;////Change lbGeneralLogs with  lstLogger 
            }
        }

        #endregion

        #region AddLoggerCreateGroup

        private void AddLoggerCreateGroup(string log)
        {
            this.Invoke(new MethodInvoker(delegate
            {
                lstCreateGroup.Items.Add(log);
                lstCreateGroup.SelectedIndex = lstCreateGroup.Items.Count - 1;
            }));
        }

        #endregion

        #region AddLoggerGroupStatus

        private void AddLoggerGroupStatus(string log)
        {
            this.Invoke(new MethodInvoker(delegate
            {
                lstGroupUpdate.Items.Add(log);
                lstGroupUpdate.SelectedIndex = lstGroupUpdate.Items.Count - 1;
            }));
        }

        #endregion

        #region ScrapeEvent_addToLogger

        void GroupStatus_addToLogger(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eventArgs = e as EventsArgs;
                AddLoggerGroupAdd(eventArgs.log);
            }
        }

        #endregion

        #region AddLoggerGroupAdd

        private void AddLoggerGroupAdd(string log)
        {
            this.Invoke(new MethodInvoker(delegate
            {
                lstLogAddGroup.Items.Add(log);
                lstLogAddGroup.SelectedIndex = lstLogAddGroup.Items.Count - 1;
            }));
        }

        #endregion

        #region AddLoggerSearchGroup

        private void AddLoggerSearchGroup(string log)
        {
            this.Invoke(new MethodInvoker(delegate
            {
                listLoggerSearchGroup.Items.Add(log);
                listLoggerSearchGroup.SelectedIndex = listLoggerSearchGroup.Items.Count - 1;
            }));
        }

        #endregion

        #region ComposeMessage_addToLogger

        void ComposeMessage_addToLogger(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eventArgs = e as EventsArgs;
                AddLoggerComposeMessage(eventArgs.log);
            }
        }

        #endregion

        #region AddLoggerComposeMessage

        private void AddLoggerComposeMessage(string log)
        {
            try
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    LstComposeMsg.Items.Add(log);
                    LstComposeMsg.SelectedIndex = LstComposeMsg.Items.Count - 1;
                }));
            }
            catch
            {
            }
        }

        #endregion

        #region GroupMemMessage_addToLogger

        void GroupMemMessage_addToLogger(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eventArgs = e as EventsArgs;
                AddLoggerGroupMemMessage(eventArgs.log);
            }
        }

        #endregion

        #region AddLoggerGroupMemMessage

        private void AddLoggerGroupMemMessage(string log)
        {
            try
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    lstGrpMemMsgLoger.Items.Add(log);
                    lstGrpMemMsgLoger.SelectedIndex = lstGrpMemMsgLoger.Items.Count - 1;
                }));
            }
            catch
            {
            }
        }

        #endregion

        #region AddToProxysLogs

        private void AddToProxysLogs(string log)
        {
            try
            {
                if (lstLoggerProxy.InvokeRequired)
                {
                    lstLoggerProxy.Invoke(new MethodInvoker(delegate
                    {
                        lstLoggerProxy.Items.Add(log);
                        lstLoggerProxy.SelectedIndex = lstLoggerProxy.Items.Count - 1;
                    }));
                }
                else
                {
                    lstLoggerProxy.Items.Add(log);
                    lstLoggerProxy.SelectedIndex = lstLoggerProxy.Items.Count - 1;
                }

                lbltotalworkingproxies.Invoke(new MethodInvoker(delegate
                {
                    lbltotalworkingproxies.Text = "Total Working Proxies : " + workingproxiesCount.ToString();
                }));
            }
            catch
            {
            }
        }

        #endregion

        #region btnStartProfileCreation_Click

        private void btnStartProfileCreation_Click(object sender, EventArgs e)
        {
            string username = string.Empty;
            string password = string.Empty;
            // BaseLib.clsSettingDB objClsSettingDb = new BaseLib.clsSettingDB();
            // objClsSettingDb.InsertOrUpdateSetting("ProfileManager", "PicsFolder", StringEncoderDecoder.Encode(txtProfilePicsFolder.Text));
            DataSet DS = DataBaseHandler.SelectQuery("select UserName , Password from tb_LinkedInAccount", "tb_LinkedInAccount");
            foreach (DataRow theRow in DS.Tables["tb_LinkedInAccount"].Rows)
            {
                username = theRow.ItemArray[0].ToString();
                LinkedInMaster obj = new LinkedInMaster();
                password = theRow.ItemArray[1].ToString();
                obj._Password = password;
            }

            new Thread(() => ProfileCreationThread()).Start();
        }

        #endregion

        #region ProfileCreationThread

        public void ProfileCreationThread()
        {
            if (LinkedInManager.linkedInDictionary.Count > 0)
            {
                foreach (KeyValuePair<string, LinkedInMaster> item in LinkedInManager.linkedInDictionary)
                {
                    LinkedInMaster LinkedIn_Master = item.Value;
                    string inkedInKey = item.Key;
                    try
                    {
                        if (_lstSummaryGoals.Count >= 1)
                        {
                            try
                            {
                                LinkedIn_Master._SummaryGoals = _lstSummaryGoals[RandomNumberGenerator.GenerateRandom(0, _lstSummaryGoals.Count - 1)];
                            }
                            catch (Exception)
                            {
                                LinkedIn_Master._SummaryGoals = string.Empty;
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }

                    try
                    {
                        if (_lstSummarySpecialties.Count >= 1)
                        {
                            try
                            {
                                LinkedIn_Master._SummarySpecialties = _lstSummarySpecialties[RandomNumberGenerator.GenerateRandom(0, _lstSummarySpecialties.Count - 1)];
                            }
                            catch (Exception)
                            {
                                LinkedIn_Master._SummarySpecialties = string.Empty;
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }

                    ProfileManager.ProfileManager obj_ProfileManager = new ProfileManager.ProfileManager();
                    obj_ProfileManager.ProFileManager();
                }
            }
            else
            {
                MessageBox.Show("Please Enter Profile Folder, Profile Photo and LinkedIn Accounts!");
                //AddLoggerProfileManager("Please Enter Profile Folder, Profile Photo and LinkedIn Accounts!");
            }
        }

        #endregion

        #region EndorsePeople
        private void AddLoggerEndorsePeople(string log)
        {
            try
            {
                try
                {
                    if (lstboxEndorse.Items.Count >= 1000)
                    {
                        lock (lockr_lstBoxEndorsePeople)
                        {
                            lstboxEndorse.Invoke(new MethodInvoker(delegate
                            {
                                lstboxEndorse.Items.Clear();
                            }));
                        }
                    }
                }
                catch
                {
                }

                if (lstboxEndorse.InvokeRequired)
                {
                    lstboxEndorse.Invoke(new MethodInvoker(delegate
                    {
                        lstboxEndorse.Items.Add(log);
                        lstboxEndorse.SelectedIndex = lstboxEndorse.Items.Count - 1;
                    }));
                }
                else
                {
                    lstboxEndorse.Items.Add(log);
                    lstboxEndorse.SelectedIndex = lstboxEndorse.Items.Count - 1;
                }
            }
            catch
            {
            }
        }

        void EndorsePeople_addToLogger(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eventArgs = e as EventsArgs;
                AddLoggerEndorsePeople(eventArgs.log);
            }
        }
        #endregion

        //*===================================================Account Creator===================================================================================================*/

        #region btnfname_Click

        private void btnfname_Click(object sender, EventArgs e)
        {
            try
            {
                //txtFirstName.Text = "";
                listFirstName.Clear();      //clear old list value
                Thread thread_FirstName = new Thread(FirstName);
                thread_FirstName.SetApartmentState(ApartmentState.STA);
                thread_FirstName.Start();
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Account Creator --> btnfname_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Account Creator --> btnfname_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAccountCraetionErrorLogs);
            }
        }

        public void FirstName()
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Text Files (*.txt)|*.txt";
                ofd.InitialDirectory = Application.StartupPath;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    this.Invoke(new MethodInvoker(delegate
                    {
                        //txtFirstName.Text = ofd.FileName;
                    }));
                    //CampainGroupCreate.Campain_First_Name = txtFirstName.Text;

                    listFirstName.Clear();

                    List<string> templist = GlobusFileHelper.ReadFile(ofd.FileName);
                    foreach (string item in templist)
                    {
                        string newItem = item.Replace(" ", "").Replace("\t", "");
                        if (!listFirstName.Contains(newItem) && !string.IsNullOrEmpty(newItem))
                        {
                            listFirstName.Add(newItem);
                            CampainGroupCreate.Campaign_listFirstName.Add(newItem);
                        }
                    }

                    listFirstName = listFirstName.Distinct().ToList();
                }
            }
        }

        #endregion

        #region btnlname_Click

        private void btnlname_Click(object sender, EventArgs e)
        {
            try
            {
                //txtLastName.Text = "";
                listLastName.Clear();       //clear old list value
                Thread thread_LastName = new Thread(LastName);
                thread_LastName.SetApartmentState(ApartmentState.STA);
                thread_LastName.Start();
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Account Creator --> btnlname_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Account Creator --> btnlname_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAccountCraetionErrorLogs);
            }
        }

        public void LastName()
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Text Files (*.txt)|*.txt";
                ofd.InitialDirectory = Application.StartupPath;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    this.Invoke(new MethodInvoker(delegate
                    {
                        //txtLastName.Text = ofd.FileName;
                    }));
                    //CampainGroupCreate.Campain_Last_Name = txtLastName.Text;

                    listLastName.Clear();

                    List<string> templist = GlobusFileHelper.ReadFiletoStringList(ofd.FileName);
                    foreach (string item in templist)
                    {
                        string newItem = item.Replace(" ", "").Replace("\t", "");
                        if (!listLastName.Contains(newItem) && !string.IsNullOrEmpty(newItem))
                        {
                            listLastName.Add(newItem);
                            CampainGroupCreate.Campaign_listLastName.Add(newItem);
                        }
                    }

                    listLastName = listLastName.Distinct().ToList();
                }
            }
        }

        #endregion

        #region btnEmails_Click

        private void btnEmails_Click(object sender, EventArgs e)
        {
            try
            {
                //txtEmails.Text = "";
                Email.Clear();
                Thread thread_Emails = new Thread(Emails);
                thread_Emails.SetApartmentState(ApartmentState.STA);
                thread_Emails.Start();
            }
            catch
            {
            }
        }

        public static Queue<string> Qemails = new Queue<string>();

        private void Emails()
        {
            try
            {
                counter_GroupMemberSearch = 0;
                this.Invoke(new MethodInvoker(delegate
                {
                    using (OpenFileDialog ofd = new OpenFileDialog())
                    {
                        ofd.Filter = "Text Files (*.txt)|*.txt";
                        ofd.InitialDirectory = Application.StartupPath;
                        if (ofd.ShowDialog() == DialogResult.OK)
                        {
                            this.Invoke(new MethodInvoker(delegate
                            {
                                //txtEmails.Text = ofd.FileName;
                            }));
                            //CampainGroupCreate.Campain_Emails = txtEmails.Text;
                            Email.Clear();

                            List<string> templist = GlobusFileHelper.ReadFile(ofd.FileName);

                            templist = templist.Distinct().ToList();

                            templist.Remove("");

                            Email.AddRange(templist);
                            Email = Email.Distinct().ToList();
                            CampainGroupCreate.Campaign_Email = Email.Distinct().ToList();

                            int countEmail = Email.Count();

                            if (Globals.IsFreeVersion)
                            {
                                try
                                {
                                    Email.RemoveRange(5, Email.Count - 5);
                                }
                                catch { }


                                if (countEmail > 5)
                                {
                                    try
                                    {
                                        this.Invoke(new MethodInvoker(delegate
                                        {

                                            FrmFreeTrial frm_freetrail = new FrmFreeTrial();
                                            frm_freetrail.TopMost = true;
                                            frm_freetrail.BringToFront();
                                            frm_freetrail.Show();
                                        }));
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }
                            }


                            //Thread FillEmailsInQueue_Thread = new Thread(FillEmailsInQueue);
                            //FillEmailsInQueue_Thread.Start();

                            //foreach (string item in templist)
                            //{
                            //    string newItem = item.Replace(" ", "").Replace("\t", "");
                            //    if (!Email.Contains(item) && !string.IsNullOrEmpty(newItem))
                            //    {
                            //        Email.Add(item);
                            //    }
                            //}
                            counter_GroupMemberSearch = Email.Count;
                        }
                    }
                }));
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Account Creator --> btnEmails_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Account Creator --> btnEmails_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAccountCraetionErrorLogs);
            }
        }

        #endregion

        #region FillEmailsInQuue
        private void FillEmailsInQueue()
        {
            try
            {
                List<string> lstTemp = Email.Distinct().ToList();
                foreach (string item in lstTemp)
                {
                    try
                    {
                        Qemails.Enqueue(item);
                    }
                    catch (Exception ex)
                    {
                        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Account Creator --> btnEmails_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Account Creator --> btnEmails_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAccountCraetionErrorLogs);

                    }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Account Creator --> btnEmails_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Account Creator --> btnEmails_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAccountCraetionErrorLogs);

            }

        }
        #endregion

        #region btnjobtitle_Click

        private void btnjobtitle_Click(object sender, EventArgs e)
        {
            try
            {
                //txtjobTitle.Text = "";
                Jobtitle.Clear();
                Thread thread_JobTitle = new Thread(JobTitle);
                thread_JobTitle.SetApartmentState(ApartmentState.STA);
                thread_JobTitle.Start();
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Account Creator --> btnjobtitle_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Account Creator --> btnjobtitle_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAccountCraetionErrorLogs);
            }
        }

        public void JobTitle()
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Text Files (*.txt)|*.txt";
                ofd.InitialDirectory = Application.StartupPath;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    this.Invoke(new MethodInvoker(delegate
                    {
                        //txtjobTitle.Text = ofd.FileName;
                    }));
                    //CampainGroupCreate.Campain_JobTitle = txtjobTitle.Text;
                    Jobtitle.Clear();
                    List<string> templist = GlobusFileHelper.ReadFiletoStringList(ofd.FileName);
                    foreach (string item in templist)
                    {
                        string newItem = item.Replace(" ", "").Replace("\t", "");
                        if (!Jobtitle.Contains(item) && !string.IsNullOrEmpty(newItem))
                        {
                            Jobtitle.Add(item);
                            CampainGroupCreate.Campaign_Jobtitle.Add(item);
                        }
                    }
                }
            }
        }

        #endregion

        #region btnPincode_Click

        private void btnPincode_Click(object sender, EventArgs e)
        {
            try
            {
                //txtPincode.Text = "";
                Pincode.Clear();
                Thread thread_PincodeNumber = new Thread(PincodeNumber);
                thread_PincodeNumber.SetApartmentState(ApartmentState.STA);
                thread_PincodeNumber.Start();
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Account Creator --> btnPincode_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Account Creator --> btnPincode_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAccountCraetionErrorLogs);
            }
        }

        public void PincodeNumber()
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Text Files (*.txt)|*.txt";
                ofd.InitialDirectory = Application.StartupPath;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    this.Invoke(new MethodInvoker(delegate
                    {
                        //txtPincode.Text = ofd.FileName;
                    }));
                    //CampainGroupCreate.Campain_Pincode = txtPincode.Text;
                    Pincode.Clear();
                    List<string> templist = GlobusFileHelper.ReadFiletoStringList(ofd.FileName);
                    foreach (string item in templist)
                    {
                        string newItem = item.Replace(" ", "").Replace("\t", "");
                        if (!Pincode.Contains(item) && !string.IsNullOrEmpty(newItem))
                        {
                            Pincode.Add(item);
                            CampainGroupCreate.Campaign_Pincode.Add(item);

                        }
                    }
                }
            }
        }

        #endregion

        #region btnCompany_Click

        private void btnCompany_Click(object sender, EventArgs e)
        {
            try
            {
                //txtCompany.Text = "";
                Company.Clear();
                Thread thread_CompanyName = new Thread(CompanyNameUpload);
                thread_CompanyName.SetApartmentState(ApartmentState.STA);
                thread_CompanyName.Start();
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Account Creator --> btnCompany_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Account Creator --> btnCompany_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAccountCraetionErrorLogs);
            }
        }

        public void CompanyNameUpload()
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Text Files (*.txt)|*.txt";
                ofd.InitialDirectory = Application.StartupPath;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    this.Invoke(new MethodInvoker(delegate
                    {
                        //txtCompany.Text = ofd.FileName;
                    }));
                    //CampainGroupCreate.Campain_Company = txtCompany.Text;
                    Company.Clear();
                    List<string> templist = GlobusFileHelper.ReadFiletoStringList(ofd.FileName);
                    foreach (string item in templist)
                    {
                        string newItem = item.Replace(" ", "").Replace("\t", "");
                        if (!Company.Contains(item) && !string.IsNullOrEmpty(newItem))
                        {
                            Company.Add(item);
                            CampainGroupCreate.Campaign_Company.Add(item);
                        }
                    }
                }
            }
        }

        #endregion

        #region btnCollege_Click

        private void btnCollege_Click(object sender, EventArgs e)
        {
            try
            {
                //txtCollege.Text = "";
                Thread thread_college = new Thread(College);
                thread_college.SetApartmentState(ApartmentState.STA);
                thread_college.Start();
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Account Creator --> btnCollege_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Account Creator --> btnCollege_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAccountCraetionErrorLogs);
            }
        }

        private void College()
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Text Files (*.txt)|*.txt";
                ofd.InitialDirectory = Application.StartupPath;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    this.Invoke(new MethodInvoker(delegate
                    {
                        //txtCollege.Text = ofd.FileName;
                    }));
                    //CampainGroupCreate.Campain_College = txtCollege.Text;
                    CollegeList.Clear();
                    List<string> templist = GlobusFileHelper.ReadFiletoStringList(ofd.FileName);
                    foreach (string item in templist)
                    {
                        string newItem = item.Replace(" ", "").Replace("\t", "");
                        if (!CollegeList.Contains(item) && !string.IsNullOrEmpty(newItem))
                        {
                            CollegeList.Add(item);
                            CampainGroupCreate.Campaign_CollegeList.Add(item);
                        }
                    }
                }
            }
        }

        #endregion

        #region ddlSelectType_SelectedIndexChanged

        private void ddlSelectType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (cmoSelectType.SelectedItem.ToString() == "Student")
            //{
            //    cmoIndustryCode.Enabled = false;
            //}

            //CampainGroupCreate.Campaign_AccountSetting = cmoSelectType.SelectedItem.ToString();

        }

        #endregion

        #region rbUsePublicProxies_CheckedChanged

        private void rbUsePublicProxies_CheckedChanged(object sender, EventArgs e)
        {
            if (lstProxies.Count > 0)
            {
                CampainGroupCreate.Campain_selected_Proxy = "0";
            }
            else
            {
                Thread.Sleep(3000);
                tabMain.SelectedIndex = 11;
            }
        }

        #endregion

        #region rbUsePrivateProxies_CheckedChanged

        private void rbUsePrivateProxies_CheckedChanged(object sender, EventArgs e)
        {
            if (pvtProxy.Count > 0)
            {
                CampainGroupCreate.Campain_selected_Proxy = "1";
            }
            else
            {
                Thread.Sleep(3000);
                tabMain.SelectedIndex = 11;
            }
        }

        #endregion

        #region btnClearProxySetting_Click

        private void btnClearProxySetting_Click(object sender, EventArgs e)
        {
            //if (rbUsePrivateProxies.Checked == true)
            //{
            //    pvtProxy.Clear();
            //    AddLoggerAccountCreator("[ " + DateTime.Now + " ] => [ Private Proxies Cleared ]");
            //}
            //else if (rbUsePublicProxies.Checked == true)
            //{
            //    lstProxies.Clear();
            //    AddLoggerAccountCreator("[ " + DateTime.Now + " ] => [ Public Proxies Cleared ]");
            //}
            //rbUsePrivateProxies.Checked = false;
            //rbUsePublicProxies.Checked = false;
            //CampainGroupCreate.Campain_selected_Proxy = string.Empty;
        }

        #endregion



        //*===================================================Add Connection===================================================================================================*/

        #region btnInviteEmailLoad_Click

        private void btnInviteEmailLoad_Click(object sender, EventArgs e)
        {
            try
            {
                txtInviteEmailLoad.Text = "";
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "Text Files (*.txt)|*.txt";
                    ofd.InitialDirectory = Application.StartupPath;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtInviteEmailLoad.Text = ofd.FileName;
                        _lstConnectionSearchKeyword.Clear();
                        List<string> templist = GlobusFileHelper.ReadFiletoStringList(ofd.FileName);
                        _lstInviteEmailConnection.Clear();
                        foreach (string item in templist)
                        {
                            if (!string.IsNullOrEmpty(item.Replace(" ", "").Replace("\t", "")))
                            {
                                _lstInviteEmailConnection.Add(item);
                            }
                        }

                        AddLoggerManageConnection("[ " + DateTime.Now + " ] => [ " + _lstInviteEmailConnection.Count + " Invite Emails Loaded  ]");
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Connection --> btnCollege_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Connection --> btnCollege_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddConnectionErrorLogs);
            }
        }

        #endregion

        private void chkAddConnectionUseDivide_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAddConnectionUseDivide.Checked)
            {
                rdBtnAddConnectionDivideEqually.Enabled = true;
                rdBtnAddConnectionDivideByGivenNo.Enabled = true;
            }
            else
            {
                rdBtnAddConnectionDivideEqually.Enabled = false;
                rdBtnAddConnectionDivideByGivenNo.Enabled = false;
            }

        }

        private void rdBtnAddConnectionDivideByGivenNo_CheckedChanged(object sender, EventArgs e)
        {
            if (rdBtnAddConnectionDivideByGivenNo.Checked)
            {
                txtAddConnectionNoOfUsers.Enabled = true;
            }
            else
            {
                txtAddConnectionNoOfUsers.Enabled = false;
            }
        }

        private void txtAddConnectionNoOfUsers_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
        #region btnSendEmailInvite_Click

        static int counter_EmailInvite = 0;

        private void btnSendEmailInvite_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {

                try
                {
                    lstSearchconnectionThread.Clear();
                    if (IsStop)
                    {
                        IsStop = false;
                    }

                    if (chkUniqueEmailConnection.Checked)
                    {

                        ManageConnections.ConnectUsing_Search.UseUniqueEmailConn = true;
                        foreach (string itemKeyword in _lstInviteEmailConnection)
                        {
                            try
                            {
                                ManageConnections.ConnectUsing_Search.lstQueuEmail.Enqueue(itemKeyword);

                            }
                            catch { }
                        }
                    }

                    if (LinkedInManager.linkedInDictionary.Count() == 0)
                    {
                        try
                        {
                            MessageBox.Show("Please Load LinkedIn Accounts From MyAccounts Menu");
                            AddLoggerStatusUpdate("[ " + DateTime.Now + " ] => [ Please Load LinkedIn Accounts From MyAccounts Menu ]");

                            lbGeneralLogs.Items.Clear();
                            frmAccounts FrmAccount = new frmAccounts();
                            FrmAccount.Show();
                            return;
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    else if (_lstInviteEmailConnection.Count == 0)
                    {
                        try
                        {
                            MessageBox.Show("Please Add InviteEmail!");
                            AddLoggerManageConnection("[ " + DateTime.Now + " ] => [ Please Add InviteEmail! ]");
                            return;
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    else if (!chkUniqueEmailConnection.Checked && !chkAddConnectionUseDivide.Checked)
                    {

                        if (string.IsNullOrEmpty(txtNumberOfRequestPerEmail.Text))
                        {

                            try
                            {
                                MessageBox.Show("Please enter correct value in field number of request per email.");
                                return;

                            }
                            catch (Exception ex)
                            {
                            }

                        }

                        else if (NumberHelper.ValidateNumber(txtNumberOfRequestPerEmail.Text.Trim()))
                        {
                            try
                            {
                                int NoofRequest = int.Parse(txtNumberOfRequestPerEmail.Text.Trim());
                                if (!chkAddConnectionUseDivide.Checked)
                                {
                                    if (NoofRequest == 0)
                                    {
                                        MessageBox.Show("Number of request per email must be greater than zero.");
                                        //AddLoggerManageConnection("Number of request per email must be greater than zero.");                       
                                        return;
                                    }
                                }

                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        //}
                        else
                        {
                            try
                            {
                                MessageBox.Show("Please Fill Integer value in TextBox(Number of request per email).");
                                AddLoggerManageConnection("[ " + DateTime.Now + " ] => [ Please Fill Integer value in TextBox(Number of request per email) ]");
                                return;
                            }
                            catch (Exception ex)
                            {
                            }

                        }
                    }
                    btnSendEmailInvite.Cursor = Cursors.AppStarting;

                    new Thread(() => InviteEmailConnectionThread()).Start();

                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Connection --> btnSendEmailInvite_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Connection --> btnSendEmailInvite_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddConnectionErrorLogs);
                }
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddLoggerManageConnection("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        }

        #endregion

        #region InviteConnectionThread()

        public void InviteEmailConnectionThread()
        {
            List<List<string>> list_listTargetEmails = new List<List<string>>();
            try
            {
                int SetThread = 5;
                counter_EmailInvite = LinkedInManager.linkedInDictionary.Count;
                int SearchMinDelay = 0;
                int SearchMaxDelay = 0;
                if (!string.IsNullOrEmpty(txtSearchMindelay.Text) && NumberHelper.ValidateNumber(txtSearchMindelay.Text))
                {
                    SearchMinDelay = Convert.ToInt32(txtSearchMindelay.Text);
                }
                if (!string.IsNullOrEmpty(txtSearchMaxDelay.Text) && NumberHelper.ValidateNumber(txtSearchMaxDelay.Text))
                {
                    SearchMaxDelay = Convert.ToInt32(txtSearchMaxDelay.Text);
                }

                try
                {
                    SearchCriteria.NumberOfRequestPerEmail = int.Parse(txtNumberOfRequestPerEmail.Text);
                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Connection --> btnSendEmailInvite_Click() --> ---1--- >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Connection --> btnSendEmailInvite_Click() --> ---1--- >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddConnectionErrorLogs);
                }

                if (!string.IsNullOrEmpty(txtThreadManageConnection.Text) && NumberHelper.ValidateNumber(txtThreadManageConnection.Text))
                {
                    SetThread = Convert.ToInt32(txtThreadManageConnection.Text);
                }

                #region User Divide Checked
                int index = 0;
                lstInviteEmailConnection_temp = _lstInviteEmailConnection;
                if (chkAddConnectionUseDivide.Checked)
                {
                    int splitNo = 0;
                    if (rdBtnAddConnectionDivideEqually.Checked)
                    {
                        splitNo = _lstInviteEmailConnection.Count / LinkedInManager.linkedInDictionary.Count;
                    }
                    else if (rdBtnAddConnectionDivideByGivenNo.Checked)
                    {
                        if (!string.IsNullOrEmpty(txtAddConnectionNoOfUsers.Text) && NumberHelper.ValidateNumber(txtAddConnectionNoOfUsers.Text))
                        {
                            int res = Convert.ToInt32(txtAddConnectionNoOfUsers.Text);
                            splitNo = res;//listUserIDs.Count / res;
                        }
                    }
                    if (splitNo == 0)
                    {
                        splitNo = RandomNumberGenerator.GenerateRandom(0, _lstInviteEmailConnection.Count - 1);
                    }
                    list_listTargetEmails = Split(_lstInviteEmailConnection, splitNo);
                    //TweetAccountManager.noOFFollows = splitNo;
                }
                #endregion

                if (LinkedInManager.linkedInDictionary.Count > 0 && _lstInviteEmailConnection.Count > 0)
                {
                    AddLoggerManageConnection("[ " + DateTime.Now + " ] => [ Starting Sending Email Invites ]");

                    try
                    {
                        foreach (KeyValuePair<string, LinkedInMaster> item in LinkedInManager.linkedInDictionary)
                        {
                            ThreadPool.SetMaxThreads(SetThread, 5);

                            if (chkAddConnectionUseDivide.Checked)
                            {
                                //  _lstInviteEmailConnection = list_listTargetEmails[index];
                                lstInviteEmailConnection_temp = list_listTargetEmails[index];
                            }
                            //      ThreadPool.QueueUserWorkItem(new WaitCallback(SendInviteUsingEmails), new object[] { item, _lstInviteEmailConnection });
                            ThreadPool.QueueUserWorkItem(new WaitCallback(SendInviteUsingEmails), new object[] { item, lstInviteEmailConnection_temp, SearchMinDelay, SearchMaxDelay });
                            index++;
                            Thread.Sleep(1000);
                        }
                    }
                    catch (Exception ex)
                    {
                        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Connection --> InviteConnectionThread() ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Connection --> InviteConnectionThread() ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddConnectionErrorLogs);
                    }
                }
                else
                {
                    if (LinkedInManager.linkedInDictionary.Count <= 0)
                    {
                        AddLoggerManageConnection("[ " + DateTime.Now + " ] => [ Please Upload LinkedIn Accounts ]");
                    }
                    else if (_lstInviteEmailConnection.Count <= 0)
                    {
                        AddLoggerManageConnection("[ " + DateTime.Now + " ] => [ Please Upload Email Connection List ]");
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Connection --> InviteConnectionThread() ---2--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Connection --> InviteConnectionThread() ---2--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddConnectionErrorLogs);
            }
        }

        #endregion

        #region SendInviteUsingEmails

        public void SendInviteUsingEmails(object Parameter)
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
                        lstSearchconnectionThread.Add(Thread.CurrentThread);
                        lstSearchconnectionThread.Distinct().ToList();
                        Thread.CurrentThread.IsBackground = true;
                    }
                }
                catch
                {
                }

                Array paramsArray = new object[1];

                paramsArray = (Array)Parameter;

                KeyValuePair<string, LinkedInMaster> item = (KeyValuePair<string, LinkedInMaster>)paramsArray.GetValue(0);
                //  List<string> _lstInviteEmailConnection = (List<string>)paramsArray.GetValue(1);

                lstInviteEmailConnection_temp = (List<string>)paramsArray.GetValue(1);
                int SearchMinDelay = Convert.ToInt32(paramsArray.GetValue(2));
                int SearchMaxDelay = Convert.ToInt32(paramsArray.GetValue(3));
                GlobusHttpHelper HttpHelper = new GlobusHttpHelper();
                LinkedinLogin Login = new LinkedinLogin();

                LinkedInMaster LinkedIn_Master = item.Value;
                string linkedInKey = item.Key;
                Account = item.Key;

                Queue<string> que_SearchEmail = new Queue<string>();
                foreach (string itemKeyword in lstInviteEmailConnection_temp)
                {
                    que_SearchEmail.Enqueue(itemKeyword);
                }


                ManageConnections.InvitationEmails invitationEmails = new InvitationEmails(item.Value._Username, item.Value._Password, item.Value._ProxyAddress, item.Value._ProxyPort, item.Value._ProxyUsername, item.Value._ProxyPassword, que_SearchEmail, lstInviteEmailConnection_temp);

                ManageConnections.InvitationEmails._lstInviteEmail = lstInviteEmailConnection_temp;

                ManageConnections.InvitationEmails Email = new ManageConnections.InvitationEmails();

                if (!string.IsNullOrEmpty(txtSearchMindelay.Text) && NumberHelper.ValidateNumber(txtSearchMindelay.Text))
                {
                    Email.MinimumDelay = Convert.ToInt32(txtSearchMindelay.Text);
                }
                if (!string.IsNullOrEmpty(txtSearchMaxDelay.Text) && NumberHelper.ValidateNumber(txtSearchMaxDelay.Text))
                {
                    Email.MaximumDelay = Convert.ToInt32(txtSearchMaxDelay.Text);
                }

                if (chkUniqueEmailConnection.Checked == true)
                {
                    ManageConnections.InvitationEmails.UniqueEmailStatus = "True";
                }


                ManageConnections.InvitationEmails.InviteEmailConnect(ref invitationEmails, SearchCriteria.NumberOfRequestPerEmail, Email.MaximumDelay, Email.MinimumDelay);

                //invitationEmails.InviteConnectionLogEvents.addToLogger -= new EventHandler(InviteConnectionLogEvents_addToLogger);
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Connection --> SendInviteUsingEmails() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Connection --> SendInviteUsingEmails() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddConnectionErrorLogs);
            }
            finally
            {
                AddLoggerManageConnection("[ " + DateTime.Now + " ] => [ Finished Email Invites For Account :" + Account + " ]");

                counter_EmailInvite--;
                if (counter_EmailInvite == 0)
                {
                    InvitationEmails.listItemTempForInvitation.Clear();

                    if (btnSendEmailInvite.InvokeRequired)
                    {
                        btnSendEmailInvite.Invoke(new MethodInvoker(delegate
                        {
                            AddLoggerManageConnection("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                            AddLoggerManageConnection("--------------------------------------------------------------------------------------------------------------------------");
                            btnSendEmailInvite.Cursor = Cursors.Default;


                        }));
                    }
                }
            }
        }

        public static List<List<string>> Split(List<string> source, int splitNumber)
        {
            if (splitNumber <= 0)
            {
                splitNumber = 1;
            }

            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / splitNumber)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }

        #endregion

        #region btnKeywordLoadFile_Click

        private void btnKeywordLoadFile_Click(object sender, EventArgs e)
        {
            try
            {
                txtKeywordLoad.Text = "";
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "Text Files (*.txt)|*.txt";
                    ofd.InitialDirectory = Application.StartupPath;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtKeywordLoad.Text = ofd.FileName;
                        _lstConnectionSearchKeyword.Clear();
                        List<string> templist = GlobusFileHelper.ReadFiletoStringList(ofd.FileName);
                        foreach (string item in templist)
                        {
                            if (!_lstConnectionSearchKeyword.Contains(item))
                            {
                                if (!string.IsNullOrEmpty(item.Replace(" ", "").Replace("\t", "")))
                                {
                                    _lstConnectionSearchKeyword.Add(item);
                                    //SearchCriteria.Que_Keyword.Enqueue(item);
                                }
                            }
                        }

                        AddLoggerManageConnection("[ " + DateTime.Now + " ] => [ " + _lstConnectionSearchKeyword.Count + " Keywords Loaded  ]");
                    }

                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Connection --> btnKeywordLoadFile_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Connection --> btnKeywordLoadFile_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddConnectionErrorLogs);
            }
        }

        #endregion

        #region btnSearchConnection_Click

        static int counter_Search_connection = 0;

        private void btnSearchConnection_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {

                try
                {
                    lstSearchconnectionThread.Clear();
                    if (IsStop)
                    {
                        IsStop = false;
                    }
                    if (chkUniqueConnectin.Checked)
                    {
                        txtNumberOfRequestPerKeyword.Enabled = false;
                        ManageConnections.ConnectUsing_Search.UseuniqueConn = true;
                        foreach (string itemKeyword in _lstConnectionSearchKeyword)
                        {
                            try
                            {
                                ManageConnections.ConnectUsing_Search.lstQueuKeywords.Enqueue(itemKeyword);

                            }
                            catch { }
                        }
                    }

                    if ((LinkedInManager.linkedInDictionary.Count > 0 && _lstConnectionSearchKeyword.Count > 0 && (!string.IsNullOrEmpty(txtNumberOfRequestPerKeyword.Text))))
                    {
                        counter_GroupMemberSearch = LinkedInManager.linkedInDictionary.Count;
                        btnSearchConnection.Cursor = Cursors.AppStarting;

                        new Thread(() => SearchConnectionKeywordThread()).Start();
                    }
                    else if (_lstConnectionSearchKeyword.Count == 0)
                    {
                        MessageBox.Show("Please Add Keywords!");
                        AddLoggerManageConnection("[ " + DateTime.Now + " ] => [ Please Add Keywords! ]");
                        return;
                    }
                    else if ((string.IsNullOrEmpty(txtNumberOfRequestPerKeyword.Text)))
                    {
                        MessageBox.Show("Please Enter the Number Of ConnectionRequest!");
                        AddLoggerManageConnection("[ " + DateTime.Now + " ] => [ Please Enter the Number Of ConnectionRequest! ]");
                        return;
                    }
                    else if ((LinkedInManager.linkedInDictionary.Count == 0))
                    {
                        MessageBox.Show("Please Load LinkedIn Accounts From MyAccounts Menu");
                        AddLoggerManageConnection("[ " + DateTime.Now + " ] => [ Please Load LinkedIn Accounts From MyAccounts Menu ]");
                        lbGeneralLogs.Items.Clear();
                        frmAccounts FrmAccount = new frmAccounts();
                        FrmAccount.Show();
                        return;
                    }

                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Connection --> btnSearchConnection_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Connection --> btnSearchConnection_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddConnectionErrorLogs);
                }
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddLoggerManageConnection("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        }

        #endregion

        #region SearchKeywordConnectionThread()

        public void SearchConnectionKeywordThread()
        {
            //int w = 0;
            int SearchMinDelay = 0;
            int SearchMaxDelay = 0;
            if (!string.IsNullOrEmpty(txtSearchMindelay.Text) && NumberHelper.ValidateNumber(txtSearchMindelay.Text))
            {
                SearchMinDelay = Convert.ToInt32(txtSearchMindelay.Text);
            }
            if (!string.IsNullOrEmpty(txtSearchMaxDelay.Text) && NumberHelper.ValidateNumber(txtSearchMaxDelay.Text))
            {
                SearchMaxDelay = Convert.ToInt32(txtSearchMaxDelay.Text);
            }

            try
            {
                SearchCriteria.NumberOfRequestPerKeyword = int.Parse(txtNumberOfRequestPerKeyword.Text);
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Connection --> btnSearchConnection_Click() --> ---1--- >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Connection --> btnSearchConnection_Click() --> ---1--- >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddConnectionErrorLogs);
            }

            if (LinkedInManager.linkedInDictionary.Count > 0 && _lstConnectionSearchKeyword.Count > 0)
            {
                int SetThread = 5;
                if (!string.IsNullOrEmpty(txtThreadManageConnection.Text) && NumberHelper.ValidateNumber(txtThreadManageConnection.Text))
                {
                    SetThread = Convert.ToInt32(txtThreadManageConnection.Text);
                }

                if (LinkedInManager.linkedInDictionary.Count > 0 && _lstConnectionSearchKeyword.Count > 0)
                {
                    if (!(chkOnlyVisitProfile.Checked))
                    {
                        AddLoggerManageConnection("[ " + DateTime.Now + " ] => [ Starting To Send Invites ]");
                        counter_Search_connection = LinkedInManager.linkedInDictionary.Count;
                    }
                    if (chkOnlyVisitProfile.Checked)
                    {
                        AddLoggerManageConnection("[ " + DateTime.Now + " ] => [ Starting To Visit Profiles ]");
                        counter_Search_connection = LinkedInManager.linkedInDictionary.Count;
                    }

                    try
                    {
                        foreach (KeyValuePair<string, LinkedInMaster> item in LinkedInManager.linkedInDictionary)
                        {
                            ThreadPool.SetMaxThreads(SetThread, 5);
                            ThreadPool.QueueUserWorkItem(new WaitCallback(SendInviteUsingKeyWords), new object[] { item, SearchMinDelay, SearchMaxDelay });
                        }
                    }
                    catch (Exception ex)
                    {
                        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Connection --> btnSearchConnection_Click() --> ---2--- >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Connection --> btnSearchConnection_Click() --> ---2--- >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddConnectionErrorLogs);
                    }
                }
                else
                {
                    AddLoggerManageConnection("[ " + DateTime.Now + " ] => [ Please Enter Keywords! ]");
                }
            }
        }

        #endregion

        #region SendInviteUsingKeyWords

        public void SendInviteUsingKeyWords(object Parameter)
        {
            try
            {
                if (IsStop)
                {
                    return;
                }

                if (!IsStop)
                {
                    lstSearchconnectionThread.Add(Thread.CurrentThread);
                    lstSearchconnectionThread.Distinct().ToList();
                    Thread.CurrentThread.IsBackground = true;
                }
            }
            catch
            {
            }
            string Account = string.Empty;
            Array paramsArray = new object[1];
            paramsArray = (Array)Parameter;

            KeyValuePair<string, LinkedInMaster> item = (KeyValuePair<string, LinkedInMaster>)paramsArray.GetValue(0);

            int SearchMinDelay = Convert.ToInt32(paramsArray.GetValue(1));
            int SearchMaxDelay = Convert.ToInt32(paramsArray.GetValue(2));
            GlobusHttpHelper HttpHelper = new GlobusHttpHelper();
            LinkedinLogin Login = new LinkedinLogin();

            LinkedInMaster LinkedIn_Master = item.Value;
            string linkedInKey = item.Key;
            Account = item.Key;

            Login.logger.addToLogger += new EventHandler(loggerAddConnection_addToLogger);

            try
            {
                {
                    Queue<string> que_SearchKeywords = new Queue<string>();
                    foreach (string itemKeyword in _lstConnectionSearchKeyword)
                    {
                        que_SearchKeywords.Enqueue(itemKeyword);
                    }

                    ManageConnections.ConnectUsing_Search ConnectUsing_Search = new ConnectUsing_Search(item.Value._Username, item.Value._Password, item.Value._ProxyAddress, item.Value._ProxyPort, item.Value._ProxyUsername, item.Value._ProxyPassword, que_SearchKeywords);
                    ManageConnections.ConnectUsing_Search.ConnectSearchUsingkeyword(ref ConnectUsing_Search, SearchMinDelay, SearchMaxDelay);
                }

            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Connection --> SendInviteUsingKeyWords() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Connection --> SendInviteUsingKeyWords() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddConnectionErrorLogs);
            }
            finally
            {
                counter_Search_connection--;
                if (counter_Search_connection == 0)
                {
                    if (btnSearchConnection.InvokeRequired)
                    {
                        btnSearchConnection.Invoke(new MethodInvoker(delegate
                        {
                            AddLoggerManageConnection("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                            AddLoggerManageConnection("---------------------------------------------------------------------------------------------------------------------------");
                            btnSearchConnection.Cursor = Cursors.Default;

                        }));
                    }
                }
            }
            // }

            // }
            //else
            //{
            //    AddLoggerManageConnection("Please Enter Keywords!");
            //}
            //AddLoggerManageConnection("Process Completed ");
        }

        #endregion

        private void chkOnlyVisitProfile_CheckedChanged(object sender, EventArgs e)
        {
            if (chkOnlyVisitProfile.Checked)
            {
                ConnectUsing_Search.OnlyVisitProfile = true;
            }
            else
            {
                ConnectUsing_Search.OnlyVisitProfile = false;
            }

        }

        private void chkClearVisitProfileUrl_CheckedChanged(object sender, EventArgs e)
        {
            if (chkClearVisitProfileUrl.Checked)
            {
                txtClearVisitProfileUrl.Visible = true;
                btnClearVisitProfileUrl.Visible = true;

                txtClearVisitProfileUrl.Focus();
                ToolTip toolTip = new ToolTip();
                toolTip.SetToolTip(txtClearVisitProfileUrl, "Please enter userId!");

            }
            else
            {
                txtClearVisitProfileUrl.Visible = false;
                btnClearVisitProfileUrl.Visible = false;
            }
        }

        private void btnClearVisitProfileUrl_Click(object sender, EventArgs e)
        {
            try
            {

                if (string.IsNullOrEmpty(txtClearVisitProfileUrl.Text.Trim()))
                {
                    MessageBox.Show("Please enter valid userId");
                    return;
                }

                DataSet ds = new DataSet();
                string Querystring = "Delete From tb_OnlyVisitProfile Where Email ='" + txtClearVisitProfileUrl.Text.Trim() + "'";
                DataBaseHandler.DeleteQuery(Querystring, "tb_OnlyVisitProfile");
                MessageBox.Show("Successfully deleted details of user " + txtClearVisitProfileUrl.Text.Trim());
                txtClearDatabase.Text = string.Empty;

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                MessageBox.Show("Some problem in deleting details of user " + txtClearVisitProfileUrl.Text.Trim());
                return;
            }
        }

        //*===================================================Status Update===================================================================================================*/

        #region btnUploadStatusMessage_Click

        private void btnUploadStatusMessage_Click(object sender, EventArgs e)
        {
            try
            {
                txtStatusUpd.Text = "";
                listStatusMessages.Clear();
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "Text Files (*.txt)|*.txt";
                    ofd.InitialDirectory = Application.StartupPath;

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtStatusUpd.Text = ofd.FileName;
                        listStatusMessages = new List<string>();

                        List<string> temp = GlobusFileHelper.ReadFiletoStringList(ofd.FileName);

                        foreach (string item in temp)
                        {
                            if (!string.IsNullOrEmpty(item.Replace(" ", "").Replace("\t", "")))
                            {
                                listStatusMessages.Add(item);
                            }
                        }
                        AddLoggerStatusUpdate("[ " + DateTime.Now + " ] => [ " + listStatusMessages.Count + " Status Update Messages loaded ]");
                        AddLoggerStatusUpdate("-------------------------------------------------------------------------------------------------------------------------------");
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Status Update --> btnUploadStatusMessage_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Status Update --> btnUploadStatusMessage_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinStatusUpdateErrorLogs);
            }
        }

        #endregion

        #region btnStatusPost_Click

        static int counter_status_posting = 0;

        public List<string> listStatusMessages = new List<string>();
        bool msg_spintaxtForStatusUpdate = false;
        private void btnStatusPost_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {

                try
                {
                    lstStatusUpdateThread.Clear();
                    if (IsStop)
                    {
                        IsStop = false;
                    }

                    if (txtStatusUpd.Text.Contains("|"))
                    {
                        if (!(chkStatusSpintax.Checked))
                        {
                            AddLoggerStatusUpdate("[ " + DateTime.Now + " ] => [ Please Check SpinTax CheckBox.. ]");
                            MessageBox.Show("Please Check SpinTax CheckBox..");
                            return;
                        }

                        if (txtStatusUpd.Text.Contains("{") || txtStatusUpd.Text.Contains("}"))
                        {
                            AddLoggerStatusUpdate("[ " + DateTime.Now + " ] => [ Its a wrong SpinTax Format.. ]");
                            MessageBox.Show("Its a wrong SpinTax Format..");
                            return;
                        }
                    }


                    if (chkStatusSpintax.Checked)
                    {
                        //listStatusMessages.Add("Status")
                        msg_spintaxtForStatusUpdate = true;
                        //UpdatelistStatusUpdate = SpinnedListGenerator.GetSpinnedList(new List<string> { txtStatusUpd.Text });
                        statusSpin = GlobusSpinHelper.spinLargeText(new Random(), txtStatusUpd.Text);
                    }
                    else
                    {
                        msg_spintaxtForStatusUpdate = false;
                    }

                    if (!chkStatusSpintax.Checked)
                    {
                        if (listStatusMessages.Count == 0)
                        {
                            MessageBox.Show("Please Add Status Message for Post");
                            AddLoggerStatusUpdate("[ " + DateTime.Now + " ] => [ Please Add Status Message for Post ]");
                            return;
                        }
                    }
                    // else if (UpdatelistStatusUpdate.Count == 0)
                    else if (string.IsNullOrEmpty(statusSpin))
                    {
                        MessageBox.Show("Please Add Status Message for Post");
                        AddLoggerStatusUpdate("[ " + DateTime.Now + " ] => [ Please Add Status Message for Post ]");
                        return;

                    }

                    if (LinkedInManager.linkedInDictionary.Count() == 0)
                    {
                        MessageBox.Show("Please Load LinkedIn Accounts From MyAccounts Menu");
                        AddLoggerStatusUpdate("[ " + DateTime.Now + " ] => [ Please Load LinkedIn Accounts From MyAccounts Menu ]");

                        lbGeneralLogs.Items.Clear();
                        frmAccounts FrmAccount = new frmAccounts();
                        FrmAccount.Show();
                        return;
                    }
                    else if (!NumberHelper.ValidateNumber(txtStatusUpdateMinDelay.Text))
                    {
                        AddLoggerSearchGroup("[ " + DateTime.Now + " ] => [ Please Enter Valid Value in Minimum Delay ]");
                        MessageBox.Show("Please Enter Valid Value in Minimum Delay");
                        txtStatusUpdateMinDelay.Focus();
                        return;
                    }
                    else if (!NumberHelper.ValidateNumber(txtStatusUpdateMaxDelay.Text))
                    {
                        AddLoggerSearchGroup("[ " + DateTime.Now + " ] => [ Please Enter Valid Value in Maximum Delay ]");
                        MessageBox.Show("Please Enter Valid Value in Maximum Delay");
                        txtStatusUpdateMaxDelay.Focus();
                        return;
                    }

                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Status Update --> btnStatusPost_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Status Update --> btnStatusPost_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinStatusUpdateErrorLogs);
                }


                if (listStatusMessages.Count >= 0)
                {
                    if (!chkStatusSpintax.Checked)
                    {
                        try
                        {
                            clsSettingDB ObjclsSettingDB = new clsSettingDB();
                            ObjclsSettingDB.InsertOrUpdateSetting("StatusUpdate", "StatusMessage", StringEncoderDecoder.Encode(txtStatusUpd.Text));
                        }
                        catch (Exception ex)
                        {
                            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Status Update --> Storing Default File() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Status Update --> Storing Default File() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinStatusUpdateErrorLogs);
                        }
                    }
                    btnStatusPost.Cursor = Cursors.AppStarting;

                    new Thread(() =>
                    {
                        LinkdinStatusUpdate();

                    }).Start();


                }
                else
                {
                    MessageBox.Show("Please upload Status Messages File");
                    AddLoggerStatusUpdate("[ " + DateTime.Now + " ] => [ Please upload Status Messages File ]");
                }
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddLoggerStatusUpdate("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        }

        #endregion

        #region LinkdinStatusUpdate()

        private void LinkdinStatusUpdate()
        {
            try
            {
                int numberofThreds = 2;

                if (NumberHelper.ValidateNumber(txtStatusUpdNoOfThread.Text.Trim()))
                {
                    numberofThreds = int.Parse(txtStatusUpdNoOfThread.Text.Trim());

                }

                counter_status_posting = LinkedInManager.linkedInDictionary.Count;
                if (LinkedInManager.linkedInDictionary.Count() > 0)
                {
                    ThreadPool.SetMaxThreads(numberofThreds, 5);

                    Others.StatusUpdate.Que_Message_Post.Clear();
                    if (!chkStatusSpintax.Checked)
                    {
                        foreach (string Message in listStatusMessages)
                        {
                            Others.StatusUpdate.Que_Message_Post.Enqueue(Message);
                        }
                    }
                    else
                    {
                        isSpinTrue = true;
                        //foreach (string Message in UpdatelistStatusUpdate)
                        //{
                        //    StatusUpdate.StatusUpdate.Que_Message_Post.Enqueue(Message);
                        //}

                    }

                    foreach (KeyValuePair<string, LinkedInMaster> item in LinkedInManager.linkedInDictionary)
                    {
                        try
                        {
                            //if (StatusUpdate.StatusUpdate.Que_Message_Post.Count > 0)
                            if ((Others.StatusUpdate.Que_Message_Post.Count > 0) || !(string.IsNullOrEmpty(statusSpin)))
                            {
                                try
                                {
                                    #region multithread
                                    ThreadPool.SetMaxThreads(numberofThreds, numberofThreds);
                                    ThreadPool.QueueUserWorkItem(new WaitCallback(PostStatus), new object[] { item });
                                    Thread.Sleep(1000);
                                    #endregion

                                    //ThreadPool.SetMaxThreads(numberofThreds, numberofThreds);
                                    //PostStatus(new object[] { item });



                                }
                                catch (Exception ex)
                                {
                                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Status Update --> LinkdinStatusUpdate() ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Status Update --> LinkdinStatusUpdate() ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinStatusUpdateErrorLogs);
                                }
                            }
                            else
                            {
                                AddLoggerStatusUpdate("[ " + DateTime.Now + " ] => [ No Message To Post . Please Upload Message to be Posted ]");
                            }
                        }
                        catch (Exception ex)
                        {
                            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Status Update --> LinkdinStatusUpdate() ---2--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Status Update --> LinkdinStatusUpdate() ---2--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinStatusUpdateErrorLogs);
                        }
                    }
                }
                else
                {
                    AddLoggerStatusUpdate("[ " + DateTime.Now + " ] => [ Please Load LinkedIn Accounts From MyAccounts Menu ]");
                    MessageBox.Show("Please Load LinkedIn Accounts From MyAccounts Menu");
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> LinkdinStatusUpdate() ---3--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> LinkdinStatusUpdate() ---3--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinStatusUpdateErrorLogs);
            }
        }

        #endregion

        #region PostStatus


        #region For Multithread
        //public void PostStatus(object Parameter)
        //{
        //    try
        //    {
        //        if (IsStop)
        //        {
        //            return;
        //        }

        //        if (!IsStop)
        //        {
        //            Thread.CurrentThread.Name = "StatusUpdateThread";
        //            lstStatusUpdateThread.Add(Thread.CurrentThread);
        //            lstStatusUpdateThread.Distinct().ToList();
        //            Thread.CurrentThread.IsBackground = true;
        //        }
        //    }
        //    catch
        //    {
        //    }

        //    string account = string.Empty;
        //    try
        //    {
        //        string post = string.Empty;
        //        Array paramsArray = new object[1];

        //        paramsArray = (Array)Parameter;

        //        KeyValuePair<string, LinkedInMaster> item = (KeyValuePair<string, LinkedInMaster>)paramsArray.GetValue(0);
        //        account = item.Key;
        //        GlobusHttpHelper HttpHelper = new GlobusHttpHelper();
        //        LinkedinLogin Login = new LinkedinLogin();

        //        Login.accountUser = item.Key;
        //        Login.accountPass = item.Value._Password;
        //        Login.proxyAddress = item.Value._ProxyAddress;
        //        Login.proxyPort = item.Value._ProxyPort;
        //        Login.proxyUserName = item.Value._ProxyUsername;
        //        Login.proxyPassword = item.Value._ProxyPassword;

        //        StatusUpdate.StatusUpdate obj_StatusUpdate = new StatusUpdate.StatusUpdate(Login.accountUser, Login.accountPass, Login.proxyAddress, Login.proxyPort, Login.proxyUserName, Login.proxyPassword);

        //        Login.logger.addToLogger += new EventHandler(logger_StatusUpdateaddToLogger);
        //        obj_StatusUpdate.logger.addToLogger += new EventHandler(logger_StatusUpdateaddToLogger);

        //        if (!Login.IsLoggedIn)
        //        {
        //            Login.LoginHttpHelper(ref HttpHelper);
        //        }

        //        if (!Login.IsLoggedIn)
        //        {
        //            AddLoggerStatusUpdate("[ " + DateTime.Now + " ] => [ Couldn't Login With Username >>> " + Login.accountUser + " ]");
        //            return;
        //        }

        //        if (Login.IsLoggedIn)
        //        {
        //            lock (LinkedinLogin.Locker_Message_Post)
        //            {
        //                if (listStatusMessages.Count > 1)
        //                {
        //                    if (StatusUpdate.StatusUpdate.Que_Message_Post.Count > 0)
        //                    {
        //                        obj_StatusUpdate.Post = StatusUpdate.StatusUpdate.Que_Message_Post.Dequeue();
        //                    }
        //                    else
        //                    {
        //                        AddLoggerStatusUpdate("[ " + DateTime.Now + " ] => [ All Loaded Post Updates ]");
        //                        return;
        //                    }
        //                }
        //                else
        //                {
        //                    obj_StatusUpdate.Post = listStatusMessages[0];
        //                }
        //            }

        //            int minDelay = 20;
        //            int maxDelay = 25;
        //            if (!string.IsNullOrEmpty(txtStatusUpdateMinDelay.Text) && NumberHelper.ValidateNumber(txtStatusUpdateMinDelay.Text))
        //            {
        //                minDelay = Convert.ToInt32(txtStatusUpdateMinDelay.Text);
        //            }
        //            if (!string.IsNullOrEmpty(txtStatusUpdateMaxDelay.Text) && NumberHelper.ValidateNumber(txtStatusUpdateMaxDelay.Text))
        //            {
        //                maxDelay = Convert.ToInt32(txtStatusUpdateMaxDelay.Text);
        //            }

        //            if (!chkUpdateStatus.Checked)
        //            {
        //                obj_StatusUpdate.PostStatus(ref HttpHelper, minDelay, maxDelay);
        //            }
        //            else
        //            {
        //                try
        //                {
        //                    obj_StatusUpdate.UpdateStatusUsingAllurl(ref HttpHelper, minDelay, maxDelay);
        //                }
        //                catch
        //                {
        //                }
        //            }
        //        }

        //        Login.logger.addToLogger -= new EventHandler(logger_StatusUpdateaddToLogger);
        //        obj_StatusUpdate.logger.addToLogger -= new EventHandler(logger_StatusUpdateaddToLogger);
        //    }
        //    catch (Exception ex)
        //    {
        //        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Status Update --> PostStatus() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
        //        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Status Update --> PostStatus() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinStatusUpdateErrorLogs);
        //    }
        //    finally
        //    {
        //        counter_status_posting--;

        //        if (counter_status_posting == 0)
        //        {
        //            if (btnStatusPost.InvokeRequired)
        //            {
        //                btnStatusPost.Invoke(new MethodInvoker(delegate
        //                {
        //                    AddLoggerStatusUpdate("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
        //                    AddLoggerStatusUpdate("-------------------------------------------------------------------------------------------------------------------------------");
        //                    btnStatusPost.Cursor = Cursors.Default;
        //                }));
        //            }
        //        }
        //    }
        //}

        // for single thread 
        #endregion

        #region for single thread
        public void PostStatus(object Parameter)
        {
            try
            {
                if (IsStop)
                {
                    return;
                }

                if (!IsStop)
                {
                    Thread.CurrentThread.Name = "StatusUpdateThread";
                    lstStatusUpdateThread.Add(Thread.CurrentThread);
                    lstStatusUpdateThread.Distinct().ToList();
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

                Login.logger.addToLogger += new EventHandler(logger_StatusUpdateaddToLogger);
                obj_StatusUpdate.logger.addToLogger += new EventHandler(logger_StatusUpdateaddToLogger);

                if (!Login.IsLoggedIn)
                {
                    Login.LoginHttpHelper(ref HttpHelper);
                }

                if (!Login.IsLoggedIn)
                {
                    AddLoggerStatusUpdate("[ " + DateTime.Now + " ] => [ Couldn't Login With Username >>> " + Login.accountUser + " ]");
                    return;
                }

                if (Login.IsLoggedIn)
                {
                    lock (LinkedinLogin.Locker_Message_Post)
                    {
                        if (!chkStatusSpintax.Checked)
                        {
                            if (listStatusMessages.Count > 1)
                            {
                                if (Others.StatusUpdate.Que_Message_Post.Count > 0)
                                {
                                    obj_StatusUpdate.Post = Others.StatusUpdate.Que_Message_Post.Dequeue();
                                }
                                else
                                {
                                    AddLoggerStatusUpdate("[ " + DateTime.Now + " ] => [ All Loaded Post Updates ]");
                                    return;
                                }
                            }
                            else
                            {
                                obj_StatusUpdate.Post = listStatusMessages[0];
                            }
                        }
                        else
                        {

                            //if (UpdatelistStatusUpdate.Count > 1)
                            //{
                            //    if (StatusUpdate.StatusUpdate.Que_Message_Post.Count > 0)
                            //    {
                            //        obj_StatusUpdate.Post = StatusUpdate.StatusUpdate.Que_Message_Post.Dequeue();
                            //    }
                            //    else
                            //    {
                            //        AddLoggerStatusUpdate("[ " + DateTime.Now + " ] => [ All Loaded Post Updates ]");
                            //        return;
                            //    }
                            //}
                            //else
                            //{
                            //    obj_StatusUpdate.Post = UpdatelistStatusUpdate[0];
                            //}

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

                    Boolean Statuswithurl = false;
                    if (chkUpdateStatus.Checked)
                    {
                        Statuswithurl = true;
                    }
                    else
                    {
                        Statuswithurl = false;
                    }

                    try
                    {
                        //obj_StatusUpdate.PostStatusMsg(ref HttpHelper, Statuswithurl, minDelay, maxDelay);
                        obj_StatusUpdate.PostStatusMsg(ref HttpHelper, Statuswithurl, minDelay, maxDelay, statusSpin, isSpinTrue);
                    }
                    catch { }


                    //if (!chkUpdateStatus.Checked)
                    //{
                    //    obj_StatusUpdate.PostStatus(ref HttpHelper, minDelay, maxDelay);
                    //}
                    //else
                    //{
                    //    try
                    //    {
                    //        obj_StatusUpdate.UpdateStatusUsingAllurl(ref HttpHelper, minDelay, maxDelay);
                    //    }
                    //    catch
                    //    {
                    //    }
                    //}
                }

                Login.logger.addToLogger -= new EventHandler(logger_StatusUpdateaddToLogger);
                obj_StatusUpdate.logger.addToLogger -= new EventHandler(logger_StatusUpdateaddToLogger);
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Status Update --> PostStatus() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Status Update --> PostStatus() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinStatusUpdateErrorLogs);
            }
            finally
            {
                counter_status_posting--;

                if (counter_status_posting == 0)
                {
                    if (btnStatusPost.InvokeRequired)
                    {
                        btnStatusPost.Invoke(new MethodInvoker(delegate
                        {
                            AddLoggerStatusUpdate("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                            AddLoggerStatusUpdate("-------------------------------------------------------------------------------------------------------------------------------");
                            btnStatusPost.Cursor = Cursors.Default;
                        }));
                    }
                }
            }
        }
        #endregion

        #endregion

        //*======================================================================Create Group=======================================================================================*/

        #region BtnGroupBrowseLogo

        private void btnGroupLogo_Click(object sender, EventArgs e)
        {
            try
            {
                txtGroupLogo.Text = "";
                using (FolderBrowserDialog ofd = new FolderBrowserDialog())
                {
                    ofd.SelectedPath = Application.StartupPath + "\\Profile\\Pics";

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtGroupLogo.Text = ofd.SelectedPath;
                        lstLogoImage = new Queue<string>();
                        string[] picsArray = Directory.GetFiles(ofd.SelectedPath);
                        foreach (string picFile in picsArray)
                        {
                            if (picFile.Contains(".png") || picFile.Contains(".gif") || picFile.Contains(".jpeg") || picFile.Contains(".jpg") || picFile.Contains(".bmp"))
                            {
                                lstLogoImage.Enqueue(picFile);
                            }
                            else
                            {
                                AddLoggerCreateGroup("[ " + DateTime.Now + " ] => [ Not Correct Image File ]");
                                AddLoggerCreateGroup("[ " + DateTime.Now + " ] => [ " + picFile + " ]");
                            }
                        }
                        Console.WriteLine(lstLogoImage.Count + " Group Logo loaded");
                        AddLoggerCreateGroup("[ " + DateTime.Now + " ] => [ " + lstLogoImage.Count + " Group Logo loaded ]");
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Create Group --> btnGroupLogo_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Create Group --> btnGroupLogo_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinCreateGroupErrorLogs);
            }
        }

        #endregion

        #region btnGrpName_Click

        private void btnGrpName_Click(object sender, EventArgs e)
        {
            try
            {
                txtGroupName.Text = "";
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "Text Files (*.txt)|*.txt";
                    ofd.InitialDirectory = Application.StartupPath;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtGroupName.Text = ofd.FileName;
                        listGroupName = new List<string>();

                        listGroupName = GlobusFileHelper.ReadFiletoStringList(ofd.FileName);

                        AddLoggerCreateGroup("[ " + DateTime.Now + " ] => [ " + listGroupName.Count + " Group Names Added ]");
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Create Group --> btnGrpName_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Create Group --> btnGrpName_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinCreateGroupErrorLogs);
            }
        }

        #endregion

        #region cmbGroupType_SelectedIndexChanged

        private void cmbGroupType_SelectedIndexChanged(object sender, EventArgs e)
        {
            AddLoggerCreateGroup("[ " + DateTime.Now + " ] => [ " + cmbGroupType.SelectedItem.ToString() + " Type Selected ]");
        }

        #endregion

        #region btnSummary_Click

        private void btnSummary_Click(object sender, EventArgs e)
        {
            try
            {
                txtSummary.Text = "";
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "Text Files (*.txt)|*.txt";
                    ofd.InitialDirectory = Application.StartupPath;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtSummary.Text = ofd.FileName;
                        listSummary = new List<string>();

                        listSummary = GlobusFileHelper.ReadFiletoStringList(ofd.FileName);
                        AddLoggerCreateGroup("[ " + DateTime.Now + " ] => [ " + listSummary.Count + " Summaries Loaded ]");
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Create Group --> btnSummary_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Create Group --> btnSummary_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinCreateGroupErrorLogs);
            }
        }

        #endregion

        #region btDesc_Click

        private void btDesc_Click(object sender, EventArgs e)
        {
            try
            {
                txtDesc.Text = "";
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "Text Files (*.txt)|*.txt";
                    ofd.InitialDirectory = Application.StartupPath;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtDesc.Text = ofd.FileName;
                        listDesciption = new List<string>();

                        listDesciption = GlobusFileHelper.ReadFiletoStringList(ofd.FileName);
                        AddLoggerCreateGroup("[ " + DateTime.Now + " ] => [ " + listDesciption.Count + " Description Loded ]");
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Create Group --> btDesc_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Create Group --> btDesc_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinCreateGroupErrorLogs);
            }
        }

        #endregion

        #region btnWbsite_Click

        private void btnWbsite_Click(object sender, EventArgs e)
        {
            try
            {
                txtWebsite.Text = "";
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "Text Files (*.txt)|*.txt";
                    ofd.InitialDirectory = Application.StartupPath;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtWebsite.Text = ofd.FileName;
                        listWebsite = new List<string>();

                        listWebsite = GlobusFileHelper.ReadFiletoStringList(ofd.FileName);
                        AddLoggerCreateGroup("[ " + DateTime.Now + " ] => [ " + listWebsite.Count + " Websites Loaded ]");
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Create Group --> btnWbsite_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Create Group --> btnWbsite_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinCreateGroupErrorLogs);
            }
        }

        #endregion

        #region cmbLanguage_SelectedIndexChanged

        private void cmbLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            AddLoggerCreateGroup("[ " + DateTime.Now + " ] => [ " + cmbLanguage.SelectedItem.ToString() + " Language Selected ]");

        }

        #endregion

        #region btnCreateOpenGroup_Click

        static int counter_group_creator = 0;

        private void btnCreateOpenGroup_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {

                try
                {
                    lstworkingThread.Clear();
                    if (IsStop)
                    {
                        IsStop = false;
                    }

                    if (!NumberHelper.ValidateNumber(txtGroupCreateMinDelay.Text))
                    {
                        AddLoggerSearchGroup("[ " + DateTime.Now + " ] => [ Please Enter Valid Value in Minimum Delay ]");
                        MessageBox.Show("Please Enter Valid Value in Minimum Delay");
                        txtGroupCreateMinDelay.Focus();
                        return;
                    }
                    else if (!NumberHelper.ValidateNumber(txtGroupCreateMaxDelay.Text))
                    {
                        AddLoggerSearchGroup("[ " + DateTime.Now + " ] => [ Please Enter Valid Value in Maximum Delay ]");
                        MessageBox.Show("Please Enter Valid Value in Maximum Delay");
                        txtGroupCreateMaxDelay.Focus();
                        return;
                    }

                    if (LinkedInManager.linkedInDictionary.Count() > 0 && lstLogoImage.Count > 0 && listGroupName.Count > 0 && listDesciption.Count > 0 && listSummary.Count > 0 && cmbLanguage.SelectedIndex > -1 && cmbGroupType.SelectedIndex > -1)
                    {
                        try
                        {
                            clsSettingDB ObjclsSettingDB = new clsSettingDB();
                            ObjclsSettingDB.InsertOrUpdateSetting("GroupCreator", "GroupName", StringEncoderDecoder.Encode(txtGroupName.Text));
                            ObjclsSettingDB.InsertOrUpdateSetting("GroupCreator", "GroupSummary", StringEncoderDecoder.Encode(txtSummary.Text));
                            ObjclsSettingDB.InsertOrUpdateSetting("GroupCreator", "GroupDescription", StringEncoderDecoder.Encode(txtDesc.Text));
                            ObjclsSettingDB.InsertOrUpdateSetting("GroupCreator", "GroupWebsite", StringEncoderDecoder.Encode(txtWebsite.Text));
                        }
                        catch (Exception ex)
                        {
                            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnCreateOpenGroup_Click --> Storing Default File() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnCreateOpenGroup_Click --> Storing Default File() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinCreateGroupErrorLogs);
                        }

                        if (chkboxCreateOpenGrp.Checked)
                        {
                            SearchCriteria.CreateGroupStatus = "Open";
                            AddLoggerCreateGroup("[ " + DateTime.Now + " ] => [ Starting Open Group Creation ]");
                        }
                        else if (chkboxCreateMemGrp.Checked)
                        {
                            SearchCriteria.CreateGroupStatus = "Member";
                            AddLoggerCreateGroup("[ " + DateTime.Now + " ] => [ Starting Member only Group Creation ]");
                        }

                        try
                        {
                            string[] array = Regex.Split(GroupType, ";");
                            foreach (string item in array)
                            {
                                if (item.Contains(cmbGroupType.SelectedItem.ToString()))
                                {
                                    string[] newArray = Regex.Split(item, ":");
                                    SearchCriteria.GroupType = newArray[1];
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Create Group --> btnCreateOpenGroup_Click() ---cmbGroupType.SelectedItem--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Create Group --> btnCreateOpenGroup_Click() ---cmbGroupType.SelectedItem--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinCreateGroupErrorLogs);
                        }

                        try
                        {
                            Dictionary<string, string> GroupLanguage = new Dictionary<string, string>();
                            GroupLanguage = ObjSelectMethod.getLangauge();
                            string language = string.Empty;
                            foreach (KeyValuePair<string, string> pair in GroupLanguage)
                            {
                                string CheckedString = cmbLanguage.SelectedItem.ToString();
                                if (CheckedString == pair.Key)
                                {
                                    language = pair.Value;
                                    SearchCriteria.GroupLang = language;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Create Group --> btnCreateOpenGroup_Click() ---cmbLanguage.SelectedItem--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Create Group --> btnCreateOpenGroup_Click() ---cmbLanguage.SelectedItem--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinCreateGroupErrorLogs);
                        }

                        try
                        {
                            foreach (string item in listGroupName)
                            {
                                Que_Grpname_Post.Enqueue(item);
                            }
                        }
                        catch
                        {
                        }

                        try
                        {
                            foreach (string item in listDesciption)
                            {
                                Que_GrpDesc_Post.Enqueue(item);
                            }
                        }
                        catch { }

                        try
                        {
                            foreach (string item in listSummary)
                            {
                                Que_GrpSummary_Post.Enqueue(item);
                            }
                        }
                        catch { }

                        try
                        {
                            foreach (string item in listWebsite)
                            {
                                Que_Grpwebsite_Post.Enqueue(item);
                            }
                        }
                        catch { }

                        btnCreateOpenGroup.Cursor = Cursors.AppStarting;

                        if (string.IsNullOrEmpty(txtCountPerAccount.Text))
                        {
                            MessageBox.Show("Please enter the number of groups to be created per account");
                            AddLoggerCreateGroup("[ " + DateTime.Now + " ] => [ Please enter the number of groups to be created per account ]");
                            return;
                        }

                        new Thread(() =>
                        {
                            LinkdinCreateGroup();

                        }).Start();
                    }
                    else
                    {
                        if (LinkedInManager.linkedInDictionary.Count() == 0)
                        {
                            MessageBox.Show("Please Load LinkedIn Accounts From MyAccounts Menu");
                            AddLoggerCreateGroup("[ " + DateTime.Now + " ] => [ Please Load LinkedIn Accounts From MyAccounts Menu ]");

                            lbGeneralLogs.Items.Clear();
                            frmAccounts FrmAccount = new frmAccounts();
                            FrmAccount.Show();
                            return;
                        }

                        if (lstLogoImage.Count <= 0)
                        {
                            MessageBox.Show("Please Add Logo..");
                            AddLoggerCreateGroup("[ " + DateTime.Now + " ] => [ Please Add Logo.. ]");
                            txtGroupLogo.Focus();
                            return;
                        }
                        else if (listGroupName.Count <= 0)
                        {
                            MessageBox.Show("Please Add GroupName..");
                            AddLoggerCreateGroup("[ " + DateTime.Now + " ] => [ Please Add GroupName.. ]");
                            txtGroupName.Focus();
                            return;
                        }
                        else if (listSummary.Count <= 0)
                        {
                            MessageBox.Show("Please Add SelectGroupSummary..");
                            AddLoggerCreateGroup("[ " + DateTime.Now + " ] => [ Please Add SelectGroupSummary..]");
                            txtSummary.Focus();
                            return;
                        }
                        else if (listDesciption.Count <= 0)
                        {
                            MessageBox.Show("Please Add GroupDesciption..");
                            AddLoggerCreateGroup("[ " + DateTime.Now + " ] => [ Please Add GroupDesciption.. ]");
                            txtDesc.Focus();
                            return;
                        }
                        else if (cmbLanguage.SelectedIndex == -1)
                        {
                            MessageBox.Show("Please Select Language..");
                            AddLoggerCreateGroup("[ " + DateTime.Now + " ] => [ Please Select Language.. ]");
                            cmbLanguage.Focus();
                            return;
                        }
                        else if (cmbGroupType.SelectedIndex == -1)
                        {
                            MessageBox.Show("Please Select GroupType..");
                            AddLoggerCreateGroup("[ " + DateTime.Now + " ] => [ Please Select GroupType.. ]");
                            cmbGroupType.Focus();
                            return;
                        }
                    }
                }
                catch { }

            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddLoggerCreateGroup("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        }

        #endregion

        #region LinkdinCreateGroup()

        private void LinkdinCreateGroup()
        {
            try
            {
                int numberofThreds = 0;
                counter_GroupMemberSearch = LinkedInManager.linkedInDictionary.Count;

                if (LinkedInManager.linkedInDictionary.Count() > 0)
                {
                    ThreadPool.SetMaxThreads(numberofThreds, 5);

                    try
                    {
                        foreach (KeyValuePair<string, LinkedInMaster> item in LinkedInManager.linkedInDictionary)
                        {
                            ThreadPool.SetMaxThreads(5, 5);

                            // ThreadPool.QueueUserWorkItem(new WaitCallback(PostCreateGroup), new object[] { item });
                            PostCreateGroup(new object[] { item });

                            Thread.Sleep(1000);
                        }
                    }
                    catch (Exception ex)
                    {
                        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Create Group --> LinkdinCreateGroup() ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Create Group --> LinkdinCreateGroup() ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinCreateGroupErrorLogs);
                    }
                }
                else
                {
                    AddLoggerCreateGroup("[ " + DateTime.Now + " ] => [ Please Load LinkedIn Accounts From MyAccounts Menu ]");
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Create Group --> LinkdinCreateGroup() ---2--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Create Group --> LinkdinCreateGroup() ---2--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinCreateGroupErrorLogs);
            }
        }

        #endregion

        #region PostCreateGroup

        Dictionary<string, Dictionary<string, string>> LinkdInGroupFriends = new Dictionary<string, Dictionary<string, string>>();
        string FromGid = string.Empty;

        public void PostCreateGroup(object Parameter)
        {
            try
            {
                if (IsStop)
                {
                    return;
                }
                if (!IsStop)
                {
                    lstworkingThread.Add(Thread.CurrentThread);
                    lstworkingThread.Distinct();
                    Thread.CurrentThread.IsBackground = true;
                }

                string Account = string.Empty;
                try
                {
                    int CountPerAccount = 0;
                    if (string.IsNullOrEmpty(txtCountPerAccount.Text))
                    {
                        MessageBox.Show("Please enter the number of groups to be created per account");
                        AddLoggerCreateGroup("[ " + DateTime.Now + " ] => [ Please enter the number of groups to be created per account ]");
                        return;
                    }
                    if (!string.IsNullOrEmpty(txtCountPerAccount.Text) && NumberHelper.ValidateNumber(txtCountPerAccount.Text))
                    {
                        CountPerAccount = Convert.ToInt32(txtCountPerAccount.Text);
                    }

                    Array paramsArray = new object[1];

                    paramsArray = (Array)Parameter;

                    KeyValuePair<string, LinkedInMaster> item = (KeyValuePair<string, LinkedInMaster>)paramsArray.GetValue(0);
                    Account = item.Key;
                    GlobusHttpHelper HttpHelper = new GlobusHttpHelper();
                    LinkedinLogin Login = new LinkedinLogin();
                    Login.accountUser = item.Key;
                    Login.accountPass = item.Value._Password;
                    Login.proxyAddress = item.Value._ProxyAddress;
                    Login.proxyPort = item.Value._ProxyPort;
                    Login.proxyUserName = item.Value._ProxyUsername;
                    Login.proxyPassword = item.Value._ProxyPassword;

                    Groups.CreateGroup obj_CreateGroup = new Groups.CreateGroup(Login.accountUser, Login.accountPass, Login.proxyAddress, Login.proxyPort, Login.proxyUserName, Login.proxyPassword);

                    Login.logger.addToLogger += new EventHandler(logger_addGroupCreateToLogger);
                    obj_CreateGroup.logger.addToLogger += new EventHandler(logger_addGroupCreateToLogger);

                    if (!Login.IsLoggedIn)
                    {
                        Login.LoginHttpHelper(ref HttpHelper);
                    }

                    if (Login.IsLoggedIn)
                    {
                        while (CountPerAccount > 0)
                        {
                            lock (Locker_Grpname_Post)
                            {
                                if (Que_Grpname_Post.Count > 0)
                                {
                                    obj_CreateGroup.PostGrpName = Que_Grpname_Post.Dequeue();
                                }
                            }

                            lock (Locked_GrpSummary_Post)
                            {
                                if (Que_GrpSummary_Post.Count > 0)
                                {
                                    obj_CreateGroup.PostGrpSummry = Que_GrpSummary_Post.Dequeue();
                                }
                            }

                            lock (Locked_GrpDesc_Post)
                            {
                                if (Que_GrpDesc_Post.Count > 0)
                                {
                                    obj_CreateGroup.PostGrpDesc = Que_GrpDesc_Post.Dequeue();
                                }
                            }

                            lock (Locked_Grpwebsite_Post)
                            {
                                if (Que_Grpwebsite_Post.Count > 0)
                                {
                                    obj_CreateGroup.PostGrpWebsite = Que_Grpwebsite_Post.Dequeue();
                                }
                            }

                            lock (locker_LogoIamge)
                            {
                                if (lstLogoImage.Count > 0)
                                {
                                    obj_CreateGroup.PostIamge = lstLogoImage.Dequeue();
                                }
                            }

                            int minDelay = 20;
                            int maxDelay = 25;

                            if (!string.IsNullOrEmpty(txtGroupCreateMinDelay.Text) && NumberHelper.ValidateNumber(txtGroupCreateMinDelay.Text))
                            {
                                minDelay = Convert.ToInt32(txtGroupCreateMinDelay.Text);
                            }
                            if (!string.IsNullOrEmpty(txtGroupCreateMaxDelay.Text) && NumberHelper.ValidateNumber(txtGroupCreateMaxDelay.Text))
                            {
                                maxDelay = Convert.ToInt32(txtGroupCreateMaxDelay.Text);
                            }

                            obj_CreateGroup.StartCreateGroup(ref HttpHelper, minDelay, maxDelay);
                            CountPerAccount--;

                        }

                        Login.logger.addToLogger -= new EventHandler(logger_addGroupCreateToLogger);
                        obj_CreateGroup.logger.addToLogger -= new EventHandler(logger_addGroupCreateToLogger);
                    }
                    else
                    {
                        AddLoggerCreateGroup("[ " + DateTime.Now + " ] => [ Could Not Login With Id: " + Account + " ]");
                    }
                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Create Group --> PostCreateGroup() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Create Group --> PostCreateGroup() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinCreateGroupErrorLogs);
                }
                finally
                {
                    AddLoggerCreateGroup("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED For :" + Account + " ]");
                    AddLoggerCreateGroup("-----------------------------------------------------------------------------------------------------------------------------------");
                    counter_group_creator++;

                    if (counter_group_creator >= LinkedInManager.linkedInDictionary.Count)
                    {
                        if (btnCreateOpenGroup.InvokeRequired)
                        {
                            btnCreateOpenGroup.Invoke(new MethodInvoker(delegate
                            {
                                AddLoggerCreateGroup("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                                AddLoggerCreateGroup("----------------------------------------------------------------------------------------------------------------------------");
                                btnCreateOpenGroup.Cursor = Cursors.Default;
                            }));
                        }
                    }


                }
            }
            catch
            {
            }
        }

        #endregion

        #region LinkedInInviteGroups()

        private void LinkedInInviteGroups()
        {
            int numberOfThreads = 0;

            try
            {
                if (LinkedInManager.linkedInDictionary.Count() > 0)
                {
                    ThreadPool.SetMaxThreads(numberOfThreads, 5);

                    foreach (KeyValuePair<string, LinkedInMaster> item in LinkedInManager.linkedInDictionary)
                    {
                        ThreadPool.SetMaxThreads(numberOfThreads, 5);

                        ThreadPool.QueueUserWorkItem(new WaitCallback(PostGroupInvitation), new object[] { item });

                        Thread.Sleep(1000);
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Create Group --> LinkedInInviteGroups() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Create Group --> LinkedInInviteGroups() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinCreateGroupErrorLogs);
            }
        }

        #endregion

        #region PostGroupInvitation

        public void PostGroupInvitation(object parameter)
        {
            try
            {
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

                if (!Login.IsLoggedIn)
                {
                    Login.LoginHttpHelper(ref HttpHelper);
                }

                AddLoggerCreateGroup("[ " + DateTime.Now + " ] => [ Invitation Message From " + Login.accountUser + " ]");
                string MessagePosted = PostFinalGroupInvitation(Login.accountUser, Login.accountPass);

                if (MessagePosted.Contains("[ " + DateTime.Now + " ] => [ Your Invitation was successfully sent to all friends ]"))
                {
                    AddLoggerCreateGroup("[ " + DateTime.Now + " ] => [ " + MessagePosted + " ]");
                }
                else if (MessagePosted.Contains("Error"))
                {
                    AddLoggerCreateGroup("[ " + DateTime.Now + " ] => [ Error in Invitation Post ]");
                }
                else
                {
                    AddLoggerCreateGroup("[ " + DateTime.Now + " ] => [ Invitation Not Posted ]");
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Create Group --> PostGroupInvitation() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Create Group --> PostGroupInvitation() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinCreateGroupErrorLogs);
            }
        }

        #endregion

        #region PostFinalGroupInvitation

        public string PostFinalGroupInvitation(string Screen_name, string pass)
        {
            string postdata = string.Empty;
            string postUrl = string.Empty;
            string ResLogin = string.Empty;
            string csrfToken = string.Empty;
            string sourceAlias = string.Empty;

            string ReturnString = string.Empty;
            string PostMsgSubject = string.Empty;
            string PostMsgBody = string.Empty;
            string FString = string.Empty;
            string Nstring = string.Empty;
            string connId = string.Empty;
            string FullName = string.Empty;
            try
            {
                string MessageText = string.Empty;
                string PostedMessage = string.Empty;

                string pageSource = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/home?trk=hb_tab_home_top"));
                if (pageSource.Contains("csrfToken"))
                {
                    csrfToken = pageSource.Substring(pageSource.IndexOf("csrfToken"), 50);
                    string[] Arr = csrfToken.Split('&');
                    csrfToken = Arr[0];
                    csrfToken = csrfToken.Replace("csrfToken", "").Replace("\"", string.Empty).Replace("value", string.Empty).Replace("cs", string.Empty).Replace("id", string.Empty).Replace("=", string.Empty);
                    csrfToken = csrfToken.Trim();
                }

                if (pageSource.Contains("sourceAlias"))
                {
                    sourceAlias = pageSource.Substring(pageSource.IndexOf("sourceAlias"), 100);
                    string[] Arr = sourceAlias.Split('"');
                    sourceAlias = Arr[2];
                }

                postUrl = "https://www.linkedin.com/uas/login-submit";
                postdata = "session_key=" + Screen_name + "&session_password=" + pass + "&source_app=&trk=guest_home_login&session_redirect=&csrfToken=" + csrfToken + "&sourceAlias=" + sourceAlias;
                ResLogin = HttpHelper.postFormData(new Uri(postUrl), postdata);

                foreach (KeyValuePair<string, Dictionary<string, string>> itemgid in LinkdInGroupFriends)
                {
                    foreach (KeyValuePair<string, string> item1 in itemgid.Value)
                    {
                        string FName = item1.Value.Split(',')[0].Trim();
                        string Lname = item1.Value.Split(',')[1].Trim();
                        FullName = FName + "-" + Lname;
                        string ToCd = item1.Key.Split(':')[1].Trim();
                        List<string> AddAllString = new List<string>();

                        if (FString == string.Empty)
                        {
                            string CompString = "{" + "\"" + "_" + ToCd.Trim() + "\"" + ":" + "{" + "\"" + "memberId" + "\"" + ":" + "\"" + ToCd.Trim() + "\"" + "," + "\"" + "firstName" + "\"" + ":" + "\"" + FName + "\"" + "," + "\"" + "lastName" + "\"" + ":" + "\"" + Lname + "\"" + "}";
                            FString = CompString;
                        }
                        else
                        {
                            string CompString = "\"" + "_" + ToCd.Trim() + "\"" + ":" + "{" + "\"" + "memberId" + "\"" + ":" + "\"" + ToCd.Trim() + "\"" + "," + "\"" + "firstName" + "\"" + ":" + "\"" + FName + "\"" + "," + "\"" + "lastName" + "\"" + ":" + "\"" + Lname + "\"" + "}";
                            FString = CompString;
                        }

                        if (Nstring == string.Empty)
                        {
                            Nstring = FString;
                            connId = ToCd;
                        }
                        else
                        {
                            Nstring += "," + FString;
                            connId += " " + ToCd;
                        }
                    }
                    Nstring += "}";

                    try
                    {
                        string PostMessage;
                        string ResponseStatusMsg;
                        string IncodePost;
                        PostMessage = "csrfToken=" + csrfToken + "&emailRecipients=&subAddMbrs=Send Invitations&gid=" + FromGid + "&invtActn=im-invite&cntactSrc=cs-connections&remIntives=19999&connectionIds=" + connId + "&connectionNames=" + Nstring + "&contactIDs=&newGroup=true";
                        IncodePost = Uri.EscapeUriString(PostMessage).Replace(":", "%3A").Replace("%20", "+").Replace("++", "+");
                        ResponseStatusMsg = HttpHelper.postFormData(new Uri("http://www.linkedin.com/manageGroup"), IncodePost);

                        if (ResponseStatusMsg.Contains("You have successfully sent invitations to this group."))
                        {
                            ReturnString = " Your Invitation was successfully sent to all friends ";
                        }
                    }
                    catch (Exception ex)
                    {
                        //AddLoggerCreateGroup(" Error:" + ex.Message + "StackTrace --> >>>" + ex.StackTrace);
                        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Create Group --> PostFinalGroupInvitation() ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Create Group --> PostFinalGroupInvitation() ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinCreateGroupErrorLogs);
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnString = "Error";
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Create Group --> PostFinalGroupInvitation() ---2--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Create Group --> PostFinalGroupInvitation() ---2--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinCreateGroupErrorLogs);
            }
            return ReturnString;
        }

        #endregion

        //*=====================================================================Add Friends Groups========================================================================================*/

        #region btnExistGroup_Click

        Dictionary<string, Dictionary<string, string>> GrpAdd = new Dictionary<string, Dictionary<string, string>>();

        private void btnExistGroup_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {
                try
                {
                    if (LinkedInManager.linkedInDictionary.Count() == 0)
                    {
                        MessageBox.Show("Please Load LinkedIn Accounts From MyAccounts Menu");
                        AddLoggerGroupAdd("[ " + DateTime.Now + " ] => [ Please Load LinkedIn Accounts From MyAccounts Menu ]");

                        lbGeneralLogs.Items.Clear();
                        frmAccounts FrmAccount = new frmAccounts();
                        FrmAccount.Show();
                        return;
                    }

                    lstLogAddGroup.Items.Clear();
                    chkExistGroup.Items.Clear();
                    chkMembers.Items.Clear();
                    cmbUser.Items.Clear();
                    LinkdInContacts.Clear();

                    AddLoggerGroupAdd("[ " + DateTime.Now + " ] => [ Starting Search for Members.. ]");

                    btnExistGroup.Cursor = Cursors.AppStarting;

                    new Thread(() =>
                    {
                        LinkedInGroupMemberSearch();

                    }).Start();
                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> btnExistGroup_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> btnExistGroup_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddFriendsGroupErrorLogs);
                }
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddLoggerGroupAdd("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        }

        #endregion

        #region LinkedInGroupMemberSearch()

        public int counter_GroupMemberSearch = 0;

        private void LinkedInGroupMemberSearch()
        {
            int numberofThreads = 5;

            try
            {
                counter_GroupMemberSearch = LinkedInManager.linkedInDictionary.Count;

                if (LinkedInManager.linkedInDictionary.Count > 0)
                {
                    ThreadPool.SetMaxThreads(numberofThreads, 5);
                    foreach (KeyValuePair<string, LinkedInMaster> item in LinkedInManager.linkedInDictionary)
                    {
                        ThreadPool.SetMaxThreads(numberofThreads, 5);
                        ThreadPool.QueueUserWorkItem(new WaitCallback(StartDMMultiThreadedGroupMemmberAdd), new object[] { item });
                        Thread.Sleep(2000);
                    }
                }
                else
                {
                    MessageBox.Show("Please Load LinkedIn Accounts From MyAccounts Menu");
                    AddLoggerGroupAdd("[ " + DateTime.Now + " ] => [ Please Load LinkedIn Accounts From MyAccounts Menu ]");
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> LinkedInGroupMemberSearch() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> LinkedInGroupMemberSearch() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddFriendsGroupErrorLogs);
            }
        }

        #endregion

        #region StartDMMultiThreadedGroupMemmberAdd

        public void StartDMMultiThreadedGroupMemmberAdd(object parameter)
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
                    GroupStatus.moduleLog = "FriendsGroup";
                    Dictionary<string, string> Result = dataScrape.PostAddMembers(ref HttpHelper, Login.accountUser);
                    LinkdInContacts.Add(Login.accountUser, Result);

                    try
                    {
                        if (Result.Count > 0)
                        {
                            AddLoggerGroupAdd("[ " + DateTime.Now + " ] => [ Friends List..Added Successfully..in " + Login.accountUser + " ]");
                        }
                        else
                        {
                            AddLoggerGroupAdd("[ " + DateTime.Now + " ] => [ Friends List..Not Available..in " + Login.accountUser + " ]");
                        }
                    }
                    catch (Exception ex)
                    {
                        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> StartDMMultiThreadedGroupMemmberAdd() >> 1 >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> StartDMMultiThreadedGroupMemmberAdd() >> 2 >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddFriendsGroupErrorLogs);
                    }

                    if (cmbUser.InvokeRequired)
                    {
                        new Thread(() =>
                        {
                            cmbUser.Invoke(new MethodInvoker(delegate
                            {
                                cmbUser.Items.Add(Login.accountUser);
                            }));
                        }).Start();
                    }
                }
                else
                {
                    AddLoggerGroupAdd("[ " + DateTime.Now + " ] => [ LinkedIn Account : " + Login.accountUser + " has been temporarily restricted ]");
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> StartDMMultiThreadedGroupMemmberAdd() >> 2 >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> StartDMMultiThreadedGroupMemmberAdd() >> 2>>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddFriendsGroupErrorLogs);
            }
            finally
            {
                counter_GroupMemberSearch--;

                if (counter_GroupMemberSearch == 0)
                {
                    if (btnAddGroups.InvokeRequired)
                    {
                        btnAddGroups.Invoke(new MethodInvoker(delegate
                        {
                            btnAddGroups.Enabled = true;
                            AddLoggerGroupAdd("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                            AddLoggerGroupAdd("------------------------------------------------------------------------------------------------------------------------------------");
                            btnExistGroup.Cursor = Cursors.Default;
                        }));
                    }
                }
            }
        }

        #endregion

        #region cmbUser_SelectedIndexChanged

        private void cmbUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            chkMembers.Items.Clear();
            LblFromEmailId.Text = FromemailId.ToString();
            lblTotMemCount.Text = "(" + "0" + ")";

            string GetUserID = cmbUser.SelectedItem.ToString();
            label48.Text = cmbUser.SelectedItem.ToString();

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

                            if (GetUserID == group1[0].ToString())
                            {
                                //chkMembers.Items.Add(item1.Value + ':' + group1[1].ToString());
                                chkMembers.Items.Add(item1.Value.Replace(",", string.Empty));
                                //break;
                            }
                        }

                        AddLoggerGroupAdd("[ " + DateTime.Now + " ] => [ " + chkMembers.Items.Count + " Friends in User: " + GetUserID + " ]");
                        AddLoggerGroupAdd("[ " + DateTime.Now + " ] => [ Finished Adding Friends of Selected User ]");
                        AddLoggerGroupAdd("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                        AddLoggerGroupAdd("------------------------------------------------------------------------------------------------------------------------------------");
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> cmbUser_SelectedIndexChanged() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> cmbUser_SelectedIndexChanged() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddFriendsGroupErrorLogs);
            }
        }

        #endregion

        #region chkMembers_SelectedIndexChanged
        private void chkMembers_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int count = Convert.ToInt32(lblTotMemCount.Text.Replace("(", string.Empty).Replace(")", string.Empty));
                int count2 = chkMembers.CheckedItems.Count + 1;
                if (chkMembers.CheckOnClick == true)
                {
                    if (count < count2)
                    {
                        lblTotMemCount.Text = "(" + (count + 1).ToString() + ")";
                    }
                    else
                    {
                        lblTotMemCount.Text = "(" + (count - 1).ToString() + ")";
                    }
                }
            }
            catch { }
        }
        #endregion

        #region LinkedInMembersGroupSearch()

        private void LinkedInMembersGroupSearch()
        {
            int numberofThreads = 5;
            string SelectedEmail = string.Empty;

            this.Invoke(new MethodInvoker(delegate
            {
                SelectedEmail = cmbUser.SelectedItem.ToString();
            }));

            try
            {
                if (LinkedInManager.linkedInDictionary.Count > 0)
                {
                    ThreadPool.SetMaxThreads(numberofThreads, 5);
                    foreach (KeyValuePair<string, LinkedInMaster> item in LinkedInManager.linkedInDictionary)
                    {
                        if (SelectedEmail.Contains(item.Key))
                        {
                            ThreadPool.SetMaxThreads(numberofThreads, 5);
                            ThreadPool.QueueUserWorkItem(new WaitCallback(StartDMMultiThreadedMemmbersGroupAdd), new object[] { item });
                            Thread.Sleep(1000);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> LinkedInMembersGroupSearch() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> LinkedInMembersGroupSearch() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddFriendsGroupErrorLogs);
            }

        }

        #endregion

        #region StartDMMultiThreadedMemmbersGroupAdd

        public void StartDMMultiThreadedMemmbersGroupAdd(object parameter)
        {
            try
            {
                try
                {
                    if (!IsStop)
                    {
                        lstJoinFriendGroupThread.Add(Thread.CurrentThread);
                        lstJoinFriendGroupThread.Distinct().ToList();
                        Thread.CurrentThread.IsBackground = true;
                    }
                }
                catch
                {
                }


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

                string SelectedEmail = string.Empty;
                this.Invoke(new MethodInvoker(delegate
                {
                    SelectedEmail = cmbUser.SelectedItem.ToString();
                }));

                if (!Login.IsLoggedIn)
                {
                    Login.LoginHttpHelper(ref HttpHelper);
                }

                AddLoggerGroupAdd("[ " + DateTime.Now + " ] => [ Add Groups Process Running..Please wait.. ]");

                if (Login.IsLoggedIn)
                {
                    List<string> SelectedItem = new List<string>();
                    try
                    {
                        foreach (KeyValuePair<string, Dictionary<string, string>> UserValue in LinkdInContacts)
                        {
                            string SelectedValue = SelectedEmail;
                            if (UserValue.Key.Contains(SelectedValue))
                            {
                                foreach (KeyValuePair<string, string> GroupValue in UserValue.Value)
                                {
                                    foreach (string Userid in chkMembers.CheckedItems)
                                    {
                                        if (GroupValue.Value.Replace(",", string.Empty).Contains(Userid))
                                        {
                                            MemId.Add(GroupValue.Key.Split(':')[1]);

                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> StartDMMultiThreadedMemmbersGroupAdd() ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> StartDMMultiThreadedMemmbersGroupAdd() ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddFriendsGroupErrorLogs);
                    }
                }

                GroupStatus dataScrape = new GroupStatus();
                Dictionary<string, string> Groups = dataScrape.PostAddGroupNames(ref HttpHelper, MemId);

                try
                {
                    GrpAdd.Add(Login.accountUser, Groups);
                    AddLoggerGroupAdd("[ " + DateTime.Now + " ] => [ Groups..Added Successfully.. ]");
                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> StartDMMultiThreadedMemmbersGroupAdd() ---2--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> StartDMMultiThreadedMemmbersGroupAdd() ---2--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddFriendsGroupErrorLogs);
                }

                string GetUserID = string.Empty;
                this.Invoke(new MethodInvoker(delegate
                {
                    chkExistGroup.Items.Clear();
                    GetUserID = cmbUser.SelectedItem.ToString();
                }));

                try
                {
                    foreach (KeyValuePair<string, Dictionary<string, string>> Grps in GrpAdd)
                    {
                        try
                        {
                            if (GetUserID.Contains(Grps.Key))
                            {
                                Dictionary<string, string> GmUserIDs = Grps.Value;
                                AddLoggerGroupAdd("[ " + DateTime.Now + " ] => [ " + GmUserIDs.Count() + " Groups in List ]");

                                foreach (KeyValuePair<string, string> item1 in Grps.Value)
                                {
                                    try
                                    {
                                        chkExistGroup.Invoke(new MethodInvoker(delegate
                                        {
                                            chkExistGroup.Items.Add(item1.Key);
                                            GetUserID = cmbUser.SelectedItem.ToString();
                                        }));
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> StartDMMultiThreadedMemmbersGroupAdd() ---3--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> StartDMMultiThreadedMemmbersGroupAdd() ---3--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddFriendsGroupErrorLogs);
                }

                AddLoggerComposeMessage("[ " + DateTime.Now + " ] => [ Finished Adding Groups of Users ]");
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> StartDMMultiThreadedMemmbersGroupAdd() ---4--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> StartDMMultiThreadedMemmbersGroupAdd() ---4--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddFriendsGroupErrorLogs);
            }
            finally
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    AddLoggerGroupAdd("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                    AddLoggerGroupAdd("------------------------------------------------------------------------------------------------------------------------------------");
                    btnAddGroups.Cursor = Cursors.Default;
                }));


            }
        }

        #endregion

        #region btnJoinMemGroup_Click

        private void btnJoinMemGroup_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {

                try
                {
                    lstExistGroup.Clear();
                    lstJoinFriendGroupThread.Clear();
                    if (IsStop)
                    {
                        IsStop = false;
                    }

                    if (LinkedInManager.linkedInDictionary.Count() == 0)
                    {
                        return;
                    }
                    else if (cmbUser.Items.Count == 0)
                    {
                        AddLoggerGroupAdd("[ " + DateTime.Now + " ] => [ Please click Add Friend button.. ]");
                        MessageBox.Show("Please click Add Friend button..");
                        return;
                    }
                    else if (chkMembers.Items.Count == 0)
                    {
                        AddLoggerGroupAdd("[ " + DateTime.Now + " ] => [ Please select ID.. ]");
                        MessageBox.Show("Please select ID..");
                        return;
                    }
                    else if (chkExistGroup.CheckedItems.Count == 0)
                    {
                        AddLoggerGroupAdd("[ " + DateTime.Now + " ] => [ Please select Atleast One Group.. ]");
                        MessageBox.Show("Please select Atleast One Group..");
                        return;
                    }

                    counter_GroupMemberSearch = 0;
                    AddLoggerGroupAdd("[ " + DateTime.Now + " ] => [ Process Running Please wait for sometimes.. ]");
                    btnJoinMemGroup.Cursor = Cursors.AppStarting;

                    if (chkExistGroup.CheckedItems.Count > 0)
                    {
                        new Thread(() =>
                        {
                            LinkedInAddGroups();
                        }).Start();
                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> btnJoinMemGroup_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> btnJoinMemGroup_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddFriendsGroupErrorLogs);
                }
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddLoggerGroupAdd("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        }

        #endregion

        #region LinkedInAddGroups
        public bool IsallaccChecked = false;
        private void LinkedInAddGroups()
        {
            try
            {
                int numberOfThreads = 20;
                counter_GroupMemberSearch = LinkedInManager.linkedInDictionary.Count;

                string SelectedEmail = string.Empty;
                this.Invoke(new MethodInvoker(delegate
                {
                    SelectedEmail = cmbUser.SelectedItem.ToString();
                }));

                try
                {
                    if (LinkedInManager.linkedInDictionary.Count() > 0)
                    {
                        ThreadPool.SetMaxThreads(numberOfThreads, 5);

                        foreach (KeyValuePair<string, LinkedInMaster> item in LinkedInManager.linkedInDictionary)
                        {
                            if (!ChkAllAccount_JoinFriendsGroup.Checked)
                            {
                                if (SelectedEmail.Contains(item.Key))
                                {
                                    ThreadPool.SetMaxThreads(numberOfThreads, 5);

                                    ThreadPool.QueueUserWorkItem(new WaitCallback(PostAddGroups), new object[] { item });

                                    Thread.Sleep(1000);
                                }
                            }
                            else
                            {
                                IsallaccChecked = true;

                                ThreadPool.SetMaxThreads(numberOfThreads, 5);

                                ThreadPool.QueueUserWorkItem(new WaitCallback(PostAddGroups), new object[] { item });

                                Thread.Sleep(1000);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> LinkedInAddGroups() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> LinkedInAddGroups() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddFriendsGroupErrorLogs);
                }
            }
            catch
            {
            }
        }

        #endregion

        #region PostAddGroups

        public void PostAddGroups(object parameter)
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
                        lstJoinFriendGroupThread.Add(Thread.CurrentThread);
                        lstJoinFriendGroupThread.Distinct().ToList();
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

                GroupStatus obj_GroupStatus = new GroupStatus(Login.accountUser, Login.accountPass, Login.proxyAddress, Login.proxyPort, Login.proxyUserName, Login.proxyPassword);

                if (!Login.IsLoggedIn)
                {
                    Login.LoginHttpHelper(ref HttpHelper);
                }

                List<string> SelectedItem = new List<string>();

                try
                {
                    if (IsallaccChecked)
                    {
                        try
                        {
                            if (MemId.Count > 0)
                            {
                                GroupStatus.MemId = MemId;
                            }
                            foreach (string SelectedGrp in chkExistGroup.CheckedItems)
                            {
                                try
                                {
                                    lstExistGroup.Add(SelectedGrp);
                                }
                                catch
                                {
                                }
                            }

                            if (lstExistGroup.Count > 0)
                            {
                                try
                                {
                                    GroupStatus.lstExistGroup = lstExistGroup;
                                }
                                catch
                                {
                                }
                            }

                            try
                            {
                                GroupStatus.GrpAdd = GrpAdd;
                            }
                            catch
                            {
                            }

                            int minDelay = 20;
                            int maxDelay = 25;

                            if (!string.IsNullOrEmpty(txtGroupjoinMinDelay.Text) && NumberHelper.ValidateNumber(txtGroupjoinMinDelay.Text))
                            {
                                minDelay = Convert.ToInt32(txtGroupjoinMinDelay.Text);
                            }
                            if (!string.IsNullOrEmpty(txtGroupjoinMaxDelay.Text) && NumberHelper.ValidateNumber(txtGroupjoinMaxDelay.Text))
                            {
                                maxDelay = Convert.ToInt32(txtGroupjoinMaxDelay.Text);
                            }

                            string MessagePosted = obj_GroupStatus.PostGroupAddFinal(Login.accountUser, Login.accountPass, minDelay, maxDelay);
                            Thread.Sleep(2000);
                        }
                        catch
                        {
                        }
                    }
                    else
                    {
                        if (label48.Text == Login.accountUser)
                        {
                            try
                            {
                                if (MemId.Count > 0)
                                {
                                    GroupStatus.MemId = MemId;
                                }
                                foreach (string SelectedGrp in chkExistGroup.CheckedItems)
                                {
                                    try
                                    {
                                        lstExistGroup.Add(SelectedGrp);
                                    }
                                    catch
                                    {
                                    }
                                }

                                if (lstExistGroup.Count > 0)
                                {
                                    try
                                    {
                                        GroupStatus.lstExistGroup = lstExistGroup;
                                    }
                                    catch
                                    {
                                    }
                                }

                                try
                                {
                                    GroupStatus.GrpAdd = GrpAdd;
                                }
                                catch
                                {
                                }

                                int minDelay = 20;
                                int maxDelay = 25;

                                if (!string.IsNullOrEmpty(txtGroupjoinMinDelay.Text) && NumberHelper.ValidateNumber(txtGroupjoinMinDelay.Text))
                                {
                                    minDelay = Convert.ToInt32(txtGroupjoinMinDelay.Text);
                                }
                                if (!string.IsNullOrEmpty(txtGroupjoinMaxDelay.Text) && NumberHelper.ValidateNumber(txtGroupjoinMaxDelay.Text))
                                {
                                    maxDelay = Convert.ToInt32(txtGroupjoinMaxDelay.Text);
                                }

                                string MessagePosted = obj_GroupStatus.PostGroupAddFinal(Login.accountUser, Login.accountPass, minDelay, maxDelay);
                                Thread.Sleep(2000);
                            }
                            catch
                            {
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> PostAddGroups() ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> PostAddGroups() ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddFriendsGroupErrorLogs);
                }
                finally
                {
                    counter_GroupMemberSearch--;
                    if (counter_GroupMemberSearch == 0)
                    {
                        if (btnJoinMemGroup.InvokeRequired)
                        {
                            btnJoinMemGroup.Invoke(new MethodInvoker(delegate
                            {
                                AddLoggerGroupAdd("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                                AddLoggerGroupAdd("------------------------------------------------------------------------------------------------------------------------------------");
                                btnJoinMemGroup.Enabled = true;
                                btnJoinMemGroup.Cursor = Cursors.Default;
                            }));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> PostAddGroups() ---2--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> PostAddGroups() ---2--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddFriendsGroupErrorLogs);
            }
        }

        #endregion

        #region PostGroupAddFinal

        public string PostGroupAddFinal(string Screen_name, string pass)
        {
            string postdata = string.Empty;
            string postUrl = string.Empty;
            string ResLogin = string.Empty;
            string csrfToken = string.Empty;
            string sourceAlias = string.Empty;

            string ReturnString = string.Empty;
            string GroupName = string.Empty;

            try
            {
                string MessageText = string.Empty;
                string PostedMessage = string.Empty;

                string pageSource = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/home?trk=hb_tab_home_top"));
                if (pageSource.Contains("csrfToken"))
                {
                    csrfToken = pageSource.Substring(pageSource.IndexOf("csrfToken"), 50);
                    string[] Arr = csrfToken.Split('&');
                    csrfToken = Arr[0];
                    csrfToken = csrfToken.Replace(":", "%3A").Replace("csrfToken", "").Replace("\"", string.Empty).Replace("value", string.Empty).Replace("cs", string.Empty).Replace("id", string.Empty).Replace("=", string.Empty);
                    csrfToken = csrfToken.Trim();
                }

                if (pageSource.Contains("sourceAlias"))
                {
                    sourceAlias = pageSource.Substring(pageSource.IndexOf("sourceAlias"), 100);
                    string[] Arr = sourceAlias.Split('"');
                    sourceAlias = Arr[2];
                }

                postUrl = "https://www.linkedin.com/uas/login-submit";
                postdata = "session_key=" + Screen_name + "&session_password=" + pass + "&source_app=&trk=guest_home_login&session_redirect=&csrfToken=" + csrfToken + "&sourceAlias=" + sourceAlias;
                ResLogin = HttpHelper.postFormData(new Uri(postUrl), postdata);

                try
                {
                    foreach (var ItemMemId in MemId)
                    {
                        string PageSource = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/profile/view?id=" + ItemMemId));

                        string[] array = Regex.Split(PageSource, "href=\"/groupRegistration?");
                        string post = "http://www.linkedin.com/groupRegistration";
                        string PostGroupstatus;
                        string ResponseStatusMsg;
                        string GroupId = string.Empty;
                        string SelItem = string.Empty;
                        bool alreadyExst = true;

                        foreach (var itemGrps in array)
                        {
                            if (itemGrps.Contains("?gid=") && !itemGrps.Contains("<!DOCTYPE html")) //">Join
                            {
                                if (itemGrps.IndexOf("?gid=") == 0)
                                {
                                    int startindex = itemGrps.IndexOf("?gid=");
                                    string start = itemGrps.Substring(startindex);
                                    int endIndex = start.IndexOf("csrfToken");  //groupRegistration?gid=85746&amp;csrfToken
                                    GroupId = start.Substring(0, endIndex).Replace("?gid=", string.Empty).Replace("&amp", string.Empty).Replace(";", string.Empty).Replace("csrfToken", string.Empty);
                                }

                                foreach (string SelectedGrp in chkExistGroup.CheckedItems)
                                {
                                    //GroupName = SelectedGrp.Split(':')[1];
                                    foreach (KeyValuePair<string, Dictionary<string, string>> GrpsValue in GrpAdd)
                                    {
                                        foreach (KeyValuePair<string, string> GroupValue in GrpsValue.Value)
                                        {
                                            foreach (string Userid in chkExistGroup.CheckedItems)
                                            {
                                                string nnn = GroupValue.Key.Split(':')[1];
                                                string aaa = Userid.Split(':')[1];
                                                GroupName = SelectedGrp.Split(':')[1];
                                                if (nnn == aaa)
                                                {
                                                    SelItem = GroupValue.Value.Replace(":", string.Empty);

                                                    if (GroupId == SelItem)
                                                    {
                                                        PostGroupstatus = post + itemGrps.Split('\"')[0].Replace("amp;", string.Empty);
                                                        ResponseStatusMsg = HttpHelper.getHtmlfromUrl1(new Uri(PostGroupstatus));

                                                        if (ResponseStatusMsg.Contains("Welcome to the"))
                                                        {
                                                            AddLoggerGroupAdd("[ " + DateTime.Now + " ] => [ Group Added : " + aaa + " ]");
                                                            AddLoggerGroupAdd("[ " + DateTime.Now + " ] => [ Group Added Successfully ]");
                                                            ReturnString = "Welcome to the " + aaa + " group on LinkedIn. You can adjust your settings";
                                                            GlobusFileHelper.AppendStringToTextfileNewLine(Screen_name + ":" + ReturnString, Globals.path_JoinFriendsGroup);
                                                        }
                                                        else if (ResponseStatusMsg.Contains("Your request to join the"))
                                                        {
                                                            AddLoggerGroupAdd("[ " + DateTime.Now + " ] => [ Group Added : " + aaa + " ]");
                                                            AddLoggerGroupAdd("[ " + DateTime.Now + " ] => [ Your request has send Successfully to join ]");
                                                            ReturnString = "Your request to join the " + aaa + " group has been received";
                                                            GlobusFileHelper.AppendStringToTextfileNewLine(Screen_name + ":" + ReturnString, Globals.path_JoinFriendsGroup);
                                                        }
                                                        else if (ResponseStatusMsg.Contains("exceeded the maximum number of confirmed and pending groups."))
                                                        {
                                                            AddLoggerGroupAdd("[ " + DateTime.Now + " ] => [ SORRY..You’ve reached or exceeded the maximum number of confirmed and pending groups. ]");
                                                            ReturnString = "You’ve reached or exceeded the maximum number of confirmed and pending groups.";
                                                            GlobusFileHelper.AppendStringToTextfileNewLine(Screen_name + ":" + ReturnString, Globals.path_JoinFriendsGroup);
                                                        }
                                                        else if (ResponseStatusMsg.Contains("You're already a member of the "))
                                                        {
                                                            if (alreadyExst == true)
                                                            {
                                                                AddLoggerGroupAdd("[ " + DateTime.Now + " ] => [ You're already a member of the " + aaa + " Group ]");
                                                                GlobusFileHelper.AppendStringToTextfileNewLine(Screen_name + ":" + "You're already a member of the " + aaa + " Group", Globals.path_JoinFriendsGroup);
                                                                alreadyExst = false;
                                                            }
                                                        }
                                                        else if (ResponseStatusMsg.Contains("Error"))
                                                        {
                                                            AddLoggerGroupAdd("[ " + DateTime.Now + " ] => [ Error in Request ]");
                                                            GlobusFileHelper.AppendStringToTextfileNewLine(Screen_name + ":" + "Error in Request", Globals.path_JoinFriendsGroup);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    AddLoggerGroupAdd("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                    AddLoggerGroupAdd("------------------------------------------------------------------------------------------------------------------------------------------");
                }
                catch (Exception ex)
                {
                    //AddLoggerGroupAdd("[ " + DateTime.Now + " ] => [ Error:" + ex.Message + "StackTrace --> >>>" + ex.StackTrace + " ]");
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> PostGroupAddFinal() ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> PostGroupAddFinal() ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddFriendsGroupErrorLogs);
                }
            }
            catch (Exception ex)
            {
                ReturnString = "Error";
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> PostGroupAddFinal() ---2--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> PostGroupAddFinal() ---2--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddFriendsGroupErrorLogs);
            }

            return ReturnString;
        }

        #endregion

        //*=====================================================================Add Search Groups========================================================================================*/

        #region btnAddSearchGroup_Click

        private void btnAddSearchGroup_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {

                try
                {
                    SearchGroup = "Start";

                    if (!string.IsNullOrEmpty(txtBoxGroupCount.Text) && NumberHelper.ValidateNumber(txtBoxGroupCount.Text))
                    {
                        Groups.JoinSearchGroup.BoxGroupCount = Convert.ToInt32(txtBoxGroupCount.Text);

                    }
                    else
                    {
                        if (string.IsNullOrEmpty(txtBoxGroupCount.Text))
                        {
                        }
                        else
                        {
                            MessageBox.Show("Enter No. of Group in Numeric Form !");
                            return;
                        }
                    }

                    lstJoinSearchGroupThread.Clear();
                    if (IsStop)
                    {
                        IsStop = false;
                    }

                    if (LinkedInManager.linkedInDictionary.Count == 0)
                    {
                        MessageBox.Show("Please Load LinkedIn Accounts From MyAccounts Menu");
                        AddLoggerSearchGroup("[ " + DateTime.Now + " ] => [ Please Load LinkedIn Accounts From MyAccounts Menu ]");

                        lbGeneralLogs.Items.Clear();
                        frmAccounts FrmAccount = new frmAccounts();
                        FrmAccount.Show();
                        return;
                    }
                    else if (txtSearchKeyword.Text == string.Empty)
                    {
                        AddLoggerSearchGroup("[ " + DateTime.Now + " ] => [ Please Enter Keyword to Search.. ]");
                        MessageBox.Show("Please Enter Keyword to Search..");
                        txtSearchKeyword.Focus();
                        return;
                    }
                    else if (txtBoxGroupCount.Text == string.Empty)
                    {
                        AddLoggerSearchGroup("[ " + DateTime.Now + " ] => [ Please Enter Number of Groups.. ]");
                        MessageBox.Show("Please Enter Number of Groups..");
                        txtBoxGroupCount.Focus();
                        return;
                    }
                    else if (Convert.ToInt32(txtBoxGroupCount.Text) < 1)
                    {
                        AddLoggerSearchGroup("[ " + DateTime.Now + " ] => [ Please Enter Minimum Number of 1 Group.. ]");
                        MessageBox.Show("Please Enter Minimum Number of 1 Group..");
                        txtBoxGroupCount.Focus();
                        return;
                    }

                    listLoggerSearchGroup.Items.Clear();
                    chkListSearchGroup.Items.Clear();
                    cmbSearchGroup.Items.Clear();
                    LinkdInContacts.Clear();

                    AddLoggerSearchGroup("[ " + DateTime.Now + " ] => [ Starting Search for Groups.. ]");
                    btnAddSearchGroup.Cursor = Cursors.AppStarting;

                    new Thread(() =>
                    {
                        LinkedInOpenGroupSearch();

                    }).Start();
                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Search Groups --> btnAddSearchGroup_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Search Groups --> btnAddSearchGroup_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddSearchGroupErrorLogs);
                }
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddLoggerSearchGroup("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        }

        #endregion

        #region LinkedInOpenGroupSearch()

        private void LinkedInOpenGroupSearch()
        {
            int numberofThreads = 5;
            counter_GroupMemberSearch = LinkedInManager.linkedInDictionary.Count;
            try
            {
                if (LinkedInManager.linkedInDictionary.Count > 0)
                {
                    ThreadPool.SetMaxThreads(numberofThreads, 5);
                    foreach (KeyValuePair<string, LinkedInMaster> item in LinkedInManager.linkedInDictionary)
                    {
                        ThreadPool.SetMaxThreads(numberofThreads, 5);
                        ThreadPool.QueueUserWorkItem(new WaitCallback(StartDMMultiThreadedSearchOpenGroups), new object[] { item });
                        Thread.Sleep(1000);
                    }
                }
                else
                {
                    MessageBox.Show("Please Load LinkedIn Accounts From MyAccounts Menu");
                    AddLoggerSearchGroup("[ " + DateTime.Now + " ] => [ Please Load LinkedIn Accounts From MyAccounts Menu ]");
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Search Groups --> LinkedInOpenGroupSearch() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Search Groups --> LinkedInOpenGroupSearch() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddSearchGroupErrorLogs);
            }
        }

        #endregion

        #region StartDMMultiThreadedSearchOpenGroups

        public void StartDMMultiThreadedSearchOpenGroups(object parameter)
        {
            try
            {
                if (IsStop)
                {
                    return;
                }

                if (!IsStop)
                {
                    lstJoinSearchGroupThread.Add(Thread.CurrentThread);
                    lstJoinSearchGroupThread.Distinct().ToList();
                    Thread.CurrentThread.IsBackground = true;
                }
            }
            catch
            {
            }
            Array paramsArray = new object[1];
            int notLoged = 0;
            paramsArray = (Array)parameter;

            KeyValuePair<string, LinkedInMaster> item = (KeyValuePair<string, LinkedInMaster>)paramsArray.GetValue(0);
            LinkedinLogin Login = new LinkedinLogin();

            Login.accountUser = item.Key;
            Login.accountPass = item.Value._Password;
            Login.proxyAddress = item.Value._ProxyAddress;
            Login.proxyPort = item.Value._ProxyPort;
            Login.proxyUserName = item.Value._ProxyUsername;
            Login.proxyPassword = item.Value._ProxyPassword;

            Groups.JoinSearchGroup obj_JoinSearchGroup = new Groups.JoinSearchGroup(Login.accountUser, Login.accountPass, Login.proxyAddress, Login.proxyPort, Login.proxyUserName, Login.proxyPassword);

            Login.logger.addToLogger += new EventHandler(logger_SearchGroupaddToLogger);
            obj_JoinSearchGroup.logger.addToLogger += new EventHandler(logger_SearchGroupaddToLogger);

            GlobusHttpHelper HttpHelper = new GlobusHttpHelper();

            if (!Login.IsLoggedIn)
            {
                Login.LoginHttpHelper(ref HttpHelper);
            }

            AddLoggerSearchGroup("[ " + DateTime.Now + " ] => [ Searching Groups Process Running..Please wait for sometimes.. ]");

            if (Login.IsLoggedIn)
            {
                GroupStatus dataScrape = new GroupStatus();
                try
                {
                    Result = obj_JoinSearchGroup.PostAddOpenGroups(ref HttpHelper, txtSearchKeyword.Text.ToString().Trim(), item.Value._Username);

                    int count = 5;
                    if (!string.IsNullOrEmpty(txtBoxGroupCount.Text) && NumberHelper.ValidateNumber(txtBoxGroupCount.Text))
                    {
                        count = Convert.ToInt32(txtBoxGroupCount.Text);
                    }

                    LinkdInContacts.Add(Login.accountUser, Result);
                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Search Groups --> StartDMMultiThreadedSearchOpenGroups() ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Search Groups --> StartDMMultiThreadedSearchOpenGroups() ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddSearchGroupErrorLogs);
                }

                try
                {
                    if (Result.Count == 0)
                    {
                        AddLoggerSearchGroup("[ " + DateTime.Now + " ] => [ Sorry, there are no group Search results matching your search criteria.. ]");
                    }
                    else
                    {
                        AddLoggerSearchGroup("[ " + DateTime.Now + " ] => [ Groups..Searched Successfully..For " + Login.accountUser + " ]");

                        if (cmbSearchGroup.InvokeRequired)
                        {
                            new Thread(() =>
                            {
                                cmbSearchGroup.Invoke(new MethodInvoker(delegate
                                {
                                    cmbSearchGroup.Items.Add(Login.accountUser);
                                }));
                            }).Start();
                        }
                    }
                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Search Groups --> StartDMMultiThreadedSearchOpenGroups() ---2--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Search Groups --> StartDMMultiThreadedSearchOpenGroups() ---2--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddSearchGroupErrorLogs);
                }
                finally
                {
                    obj_JoinSearchGroup.logger.addToLogger -= new EventHandler(logger_SearchGroupaddToLogger);

                    counter_GroupMemberSearch--;
                    int aaa = counter_GroupMemberSearch - notLoged;
                    if (counter_GroupMemberSearch == aaa)
                    {
                        if (cmbSearchGroup.InvokeRequired)
                        {
                            cmbSearchGroup.Invoke(new MethodInvoker(delegate
                            {
                                AddLoggerSearchGroup("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                                AddLoggerSearchGroup("----------------------------------------------------------------------------------------------------------------------------------------");
                                cmbSearchGroup.Enabled = true;
                                btnAddSearchGroup.Cursor = Cursors.Default;

                            }));
                        }
                    }
                }
            }
            else
            {
                AddLoggerSearchGroup("[ " + DateTime.Now + " ] => [ Account : " + Login.accountUser + " Not Logged In ]");
                if (notLoged == 0)
                {
                    notLoged = 1;
                }
                else
                {
                    notLoged = +notLoged;
                }
            }
        }

        #endregion

        #region PostAddOpenGroups

        public Dictionary<string, string> PostAddOpenGroups(ref GlobusHttpHelper HttpHelper)
        {
            try
            {
                int count = 5;
                string IncodePost = string.Empty;
                string PostMessage = string.Empty;
                string PostMessage1 = string.Empty;
                string MemFullName = string.Empty;
                string SearchId = string.Empty;
                string TotResult = string.Empty;
                string GrpName = string.Empty;
                string IsOpenGrp = string.Empty;
                string GrpType = string.Empty;
                string GrpId = string.Empty;
                List<string> checkDupGrpId = new List<string>();
                OpenGroupDtl.Clear();

                if (!string.IsNullOrEmpty(txtBoxGroupCount.Text) && NumberHelper.ValidateNumber(txtBoxGroupCount.Text))
                {
                    count = Convert.ToInt32(txtBoxGroupCount.Text);
                }

                PostMessage = "http://www.linkedin.com/search-fe/group_search?pplSearchOrigin=GLHD&keywords=" + txtSearchKeyword.Text.ToString().Trim() + "";

                IncodePost = Uri.EscapeUriString(PostMessage);
                string pageSource = HttpHelper.getHtmlfromUrl1(new Uri(IncodePost));
                string[] RgxGroupData = System.Text.RegularExpressions.Regex.Split(pageSource, "GLHD");

                foreach (var refSearchId in RgxGroupData)
                {
                    if (refSearchId.Contains("refSearchId"))
                    {
                        try
                        {
                            int startindex = refSearchId.IndexOf("=");
                            string start = refSearchId.Substring(startindex);
                            int endIndex = start.IndexOf("page_num");
                            SearchId = start.Substring(0, endIndex).Replace("&quot;", string.Empty).Replace("=", string.Empty).Replace("&", string.Empty).Replace(":", string.Empty).Replace("amp;", string.Empty);
                        }
                        catch
                        {
                        }
                    }
                }

                string[] RgxGroupDataResult = System.Text.RegularExpressions.Regex.Split(pageSource, "results_count");

                foreach (var itemResult in RgxGroupDataResult)
                {
                    try
                    {
                        int startindexRes = itemResult.IndexOf("&quot");
                        string startRes = itemResult.Substring(startindexRes);
                        int endIndexRes = startRes.IndexOf("results_summary");
                        TotResult = startRes.Substring(0, endIndexRes).Replace("&quot;", string.Empty).Replace(",", string.Empty).Replace("&", string.Empty).Replace(":", string.Empty).Replace("amp;", string.Empty);
                    }
                    catch
                    {
                    }
                }

                int pageNo = Convert.ToInt32(TotResult);
                pageNo = (pageNo / 10) + 2;
                bool BreakingLoop = false;

                for (int i = 1; i < pageNo; i++)
                {
                    string PageWiseUrl = "search-fe/group_search?keywords=" + txtSearchKeyword.Text.ToString().Trim() + "&pplSearchOrigin=GLHD&refSearchId=" + SearchId + "&page_num= " + i + "";
                    PostMessage1 = "http://www.linkedin.com/" + PageWiseUrl + "";

                    string pageSource1 = HttpHelper.getHtmlfromUrl1(new Uri(PostMessage1));
                    string[] RgxGroupData1 = System.Text.RegularExpressions.Regex.Split(pageSource1, "name&quot");

                    foreach (var OpenGrps in RgxGroupData1)
                    {
                        try
                        {
                            if (OpenGrps.Contains(":&quot"))
                            {
                                try
                                {
                                    int startindex = OpenGrps.IndexOf(":&quot");
                                    string start = OpenGrps.Substring(startindex);
                                    int endIndex = start.IndexOf("&quot;,");
                                    GrpName = start.Substring(0, endIndex).Replace("&quot;", string.Empty).Replace("id", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Replace("amp;", string.Empty);
                                }
                                catch
                                {
                                }

                                try
                                {
                                    int startinGrTyp = OpenGrps.IndexOf("&quot;is_open&quot");
                                    string startGrTyp = OpenGrps.Substring(startinGrTyp);
                                    int endIndexGrTyp = startGrTyp.IndexOf("&quot;description");
                                    IsOpenGrp = startGrTyp.Substring(0, endIndexGrTyp).Replace("&quot;is_open&quot", string.Empty).Replace(";", string.Empty).Replace(":", string.Empty).Replace(",", string.Empty).Trim();

                                    if (IsOpenGrp == "true")
                                    {
                                        GrpType = "Open Group";
                                    }
                                    else
                                    {
                                        GrpType = "Member Only Group";
                                    }
                                }
                                catch
                                {
                                }

                                if (GrpName == "group" || GrpName == "_postGroupLink" || GrpName == "_viewGroupLink" || GrpName == "_similarGroupLink" || GrpName == "_searchWithinLink" || GrpName == "_joinGroupLink" || GrpName == "N" || GrpName == "Remove All")
                                {
                                    continue;
                                }

                                try
                                {
                                    int startindex1 = OpenGrps.IndexOf("/groups?");
                                    string start1 = OpenGrps.Substring(startindex1);
                                    int endIndex1 = start1.IndexOf("&quot;");
                                    GrpId = start1.Substring(0, endIndex1).Replace("/groups?gid=", string.Empty).Trim();
                                }
                                catch
                                {
                                }

                                if (checkDupGrpId.Count != 0)
                                {
                                    if (checkDupGrpId.Contains(GrpId))
                                    {
                                    }
                                    else
                                    {
                                        checkDupGrpId.Add(GrpId);
                                        if (OpenGroupDtl.Count < count)
                                        {
                                            OpenGroupDtl.Add(GrpName + " (" + GrpType + ")", GrpId);
                                        }
                                        else
                                        {
                                            BreakingLoop = true;
                                            break;
                                        }
                                        AddLoggerSearchGroup("[ " + DateTime.Now + " ] => [ Founded Group Name : " + GrpName + " ]");
                                    }
                                }
                                else
                                {
                                    checkDupGrpId.Add(GrpId);
                                    if (OpenGroupDtl.Count <= count)
                                    {
                                        OpenGroupDtl.Add(GrpName + " (" + GrpType + ")", GrpId);
                                    }
                                    else
                                    {
                                        BreakingLoop = true;
                                        break;
                                    }
                                }
                            }
                        }
                        catch
                        {
                        }
                    }
                    if (BreakingLoop)
                    {
                        break;
                    }
                    //return OpenGroupDtl;
                }
                return OpenGroupDtl;
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Search Groups --> PostAddOpenGroups() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Search Groups --> PostAddOpenGroups() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddSearchGroupErrorLogs);
                return OpenGroupDtl;
            }
        }

        #endregion

        #region cmbSearchGroup_SelectedIndexChanged

        private void cmbSearchGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            chkListSearchGroup.Items.Clear();

            try
            {
                string GetUserID = cmbSearchGroup.SelectedItem.ToString();
                label44.Text = cmbSearchGroup.SelectedItem.ToString();
                foreach (KeyValuePair<string, Dictionary<string, string>> item in LinkdInContacts)
                {
                    if (SearchGroup == "Start")
                    {
                        if (GetUserID.Contains(item.Key))
                        {
                            Dictionary<string, string> GmUserIDs = item.Value;
                            //AddLoggerSearchGroup(" " + GmUserIDs.Count() + " Groups Added in List");
                            //AddLoggerSearchGroup("Founded Group Names For Account : " + item.Key);
                            foreach (KeyValuePair<string, string> item1 in item.Value)
                            {
                                AddLoggerSearchGroup("[ " + DateTime.Now + " ] => [ " + item1.Key + " ]");
                                chkListSearchGroup.Items.Add(item1.Key);
                            }

                            AddLoggerSearchGroup("[ " + DateTime.Now + " ] => [ Finished Finding Group of Selected User ]");
                        }
                    }


                }

                //AddLoggerSearchGroup("Finished Finding Group of Selected User");
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Search Groups --> cmbSearchGroup_SelectedIndexChanged() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Search Groups --> cmbSearchGroup_SelectedIndexChanged() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddSearchGroupErrorLogs);
            }
        }

        #endregion

        #region chkSelectSearchGrp_CheckedChanged

        private void chkSelectSearchGrp_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkSelectSearchGrp.Checked == true)
                {
                    for (int i = 0; i < chkListSearchGroup.Items.Count; i++)
                    {
                        string aaa = Convert.ToString(chkListSearchGroup.Items[i]);
                        chkListSearchGroup.SetItemChecked(i, true);
                    }
                }
                else
                {
                    for (int i = 0; i < chkListSearchGroup.Items.Count; i++)
                    {
                        string aaa = Convert.ToString(chkListSearchGroup.Items[i]);
                        chkListSearchGroup.SetItemChecked(i, false);
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Search Groups --> chkSelectSearchGrp_CheckedChanged() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Search Groups --> chkSelectSearchGrp_CheckedChanged() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddSearchGroupErrorLogs);
            }
        }

        #endregion

        #region btnJoinSearchGroup_Click

        private void btnJoinSearchGroup_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {

                try
                {
                    lstJoinSearchGroupThread.Clear();
                    if (IsStop)
                    {
                        IsStop = false;
                    }

                    if (LinkedInManager.linkedInDictionary.Count() == 0)
                    {
                        return;
                    }
                    else if (txtSearchKeyword.Text == string.Empty)
                    {
                        AddLoggerSearchGroup("[ " + DateTime.Now + " ] => [ Please Enter Keyword to Search.. ]");
                        MessageBox.Show("Please Enter Keyword to Search..");
                        txtSearchKeyword.Focus();
                        return;
                    }
                    else if (cmbSearchGroup.Items.Count == 0)
                    {
                        AddLoggerGroupAdd("[ " + DateTime.Now + " ] => [ Please click Add Groups button.. ]");
                        MessageBox.Show("Please click Add Groups button..");
                        return;
                    }
                    else if (chkListSearchGroup.CheckedItems.Count == 0)
                    {
                        AddLoggerSearchGroup("[ " + DateTime.Now + " ] => [ Please select Atleast One Group in List.. ]");
                        MessageBox.Show("Please select Atleast One Group in List..");
                        return;
                    }
                    else if (!NumberHelper.ValidateNumber(txtSearchGroupMinDelay.Text.Trim()))
                    {

                        AddLoggerSearchGroup("[ " + DateTime.Now + " ] => [ Please Enter Valid Value in Minimum Delay ]");
                        MessageBox.Show("Please Enter Valid Value in Minimum Delay");
                        txtSearchGroupMinDelay.Focus();
                        return;
                    }
                    else if (!NumberHelper.ValidateNumber(txtSearchGroupMaxDelay.Text.Trim()))
                    {
                        AddLoggerSearchGroup("[ " + DateTime.Now + " ] => [ Please Enter Valid Value in Maximum Delay ]");
                        MessageBox.Show("Please Enter Valid Value in Maximum Delay");
                        txtSearchGroupMaxDelay.Focus();
                        return;
                    }

                    btnJoinSearchGroup.Cursor = Cursors.AppStarting;
                    counter_GroupMemberSearch = 0;

                    if (chkListSearchGroup.CheckedItems.Count > 0)
                    {
                        new Thread(() =>
                        {
                            LinkedInAddSearchGroups();

                        }).Start();
                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Search Groups --> btnJoinSearchGroup_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Search Groups --> btnJoinSearchGroup_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddSearchGroupErrorLogs);
                }
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddLoggerSearchGroup("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        }

        #endregion

        #region LinkedInAddSearchGroups

        string UserSlecetedDetails = string.Empty;

        private void LinkedInAddSearchGroups()
        {
            int numberOfThreads = 0;
            counter_GroupMemberSearch = LinkedInManager.linkedInDictionary.Count;
            try
            {
                cmbSearchGroup.Invoke(new MethodInvoker(delegate
                {
                    UserSlecetedDetails = cmbSearchGroup.SelectedItem.ToString();
                }));

                if (LinkedInManager.linkedInDictionary.Count() > 0)
                {
                    ThreadPool.SetMaxThreads(numberOfThreads, 5);

                    foreach (KeyValuePair<string, LinkedInMaster> item in LinkedInManager.linkedInDictionary)
                    {
                        if (ChkAllAccounts_JoinSearchGroup.Checked)
                        {
                            ThreadPool.SetMaxThreads(numberOfThreads, 5);

                            ThreadPool.QueueUserWorkItem(new WaitCallback(PostAddSearchGroups), new object[] { item });

                            Thread.Sleep(1000);
                        }
                        else
                        {
                            if ((UserSlecetedDetails.Contains(item.Key)))
                            {
                                ThreadPool.SetMaxThreads(numberOfThreads, 5);

                                ThreadPool.QueueUserWorkItem(new WaitCallback(PostAddSearchGroups), new object[] { item });

                                Thread.Sleep(1000);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Search Groups --> LinkedInAddSearchGroups() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Search Groups --> LinkedInAddSearchGroups() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddSearchGroupErrorLogs);
            }
        }

        #endregion

        #region PostAddSearchGroups

        public void PostAddSearchGroups(object parameter)
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
                        lstJoinSearchGroupThread.Add(Thread.CurrentThread);
                        lstJoinSearchGroupThread.Distinct().ToList();
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

                Groups.JoinSearchGroup obj_JoinSearchGroup = new Groups.JoinSearchGroup(Login.accountUser, Login.accountPass, Login.proxyAddress, Login.proxyPort, Login.proxyUserName, Login.proxyPassword);

                Login.logger.addToLogger += new EventHandler(logger_SearchGroupaddToLogger);
                obj_JoinSearchGroup.logger.addToLogger += new EventHandler(logger_SearchGroupaddToLogger);

                if (!Login.IsLoggedIn)
                {
                    Login.LoginHttpHelper(ref HttpHelper);
                    if (Login.IsLoggedIn)
                    {
                        // AddLoggerSearchGroup("Logged In With Account : " + Login.accountUser);
                    }
                }

                AddLoggerSearchGroup("[ " + DateTime.Now + " ] => [ Finding Selected Group Ids ]");
                List<string> SelectedItem = new List<string>();

                foreach (KeyValuePair<string, Dictionary<string, string>> UserValue in LinkdInContacts)
                {
                    string SelectedValue = UserSlecetedDetails;
                    if (UserValue.Key.Contains(SelectedValue))
                    {
                        foreach (KeyValuePair<string, string> GroupValue in UserValue.Value)
                        {
                            foreach (string Userid in chkListSearchGroup.CheckedItems)
                            {
                                if (GroupValue.Key.Contains(Userid))
                                {
                                    SelectedItem.Add(GroupValue.Key + ":" + GroupValue.Value);
                                    break;
                                }
                            }
                        }
                    }
                }

                AddLoggerSearchGroup("[ " + DateTime.Now + " ] => [ Now Group Joining Process Start  ]");

                int minDelay = 20;
                int maxDelay = 25;

                if (!string.IsNullOrEmpty(txtSearchGroupMinDelay.Text) && NumberHelper.ValidateNumber(txtSearchGroupMinDelay.Text))
                {
                    minDelay = Convert.ToInt32(txtSearchGroupMinDelay.Text);
                }
                if (!string.IsNullOrEmpty(txtSearchGroupMaxDelay.Text) && NumberHelper.ValidateNumber(txtSearchGroupMaxDelay.Text))
                {
                    maxDelay = Convert.ToInt32(txtSearchGroupMaxDelay.Text);
                }

                string MessagePosted = obj_JoinSearchGroup.PostSearchGroupAddFinal(ref HttpHelper, Login.accountUser, Login.accountPass, SelectedItem, minDelay, maxDelay);

                Login.logger.addToLogger -= new EventHandler(logger_SearchGroupaddToLogger);
                obj_JoinSearchGroup.logger.addToLogger -= new EventHandler(logger_SearchGroupaddToLogger);
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Search Groups --> PostAddSearchGroups() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Search Groups --> PostAddSearchGroups() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddSearchGroupErrorLogs);
            }
            finally
            {
                btnJoinSearchGroup.Invoke(new MethodInvoker(delegate
                {
                    btnJoinSearchGroup.Cursor = Cursors.Default;
                }));
            }
        }

        #endregion


        //*===================================================Group Update=============================================================================================*/

        #region btnGroupDiscussion_Click

        private void btnGroupDiscussion_Click(object sender, EventArgs e)
        {
            msg = string.Empty;

            try
            {
                txtGroupDiscussion.Text = "";
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "Text Files (*.txt)|*.txt";
                    ofd.InitialDirectory = Application.StartupPath;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtGroupDiscussion.Text = ofd.FileName;
                        ListGrpDiscussion = new List<string>();

                        //ListGrpDiscussion = BaseLib.GlobusFileHelper.ReadFiletoStringList(ofd.FileName);

                        List<string> tempListGrpDiscussion = GlobusFileHelper.ReadFiletoStringList(ofd.FileName);

                        foreach (string item in tempListGrpDiscussion)
                        {
                            try
                            {
                                string aa = item.Length.ToString();
                                int bb = int.Parse(aa.ToString());

                                if (bb < 200)
                                {
                                    //if (msg == string.Empty)
                                    //{
                                    //    msg = "STATUS HEADER: " + item + " IS EXCEEDED 200 CHARACTERS..";
                                    //}
                                    //else
                                    //{
                                    //    msg += ";" + "STATUS HEADER: " + item + " IS EXCEEDED 200 CHARACTERS..";
                                    //}

                                    ListGrpDiscussion.Add(item);
                                }
                                else
                                {
                                    AddLoggerGroupStatus("[ " + DateTime.Now + " ] => [ " + item.Length + " Is EXCEEDED 200 CHARACTERS ! ]");
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Error >>> " + ex.StackTrace);
                            }
                        }
                    }

                    //if (msg != string.Empty)
                    //{
                    //    MessageBox.Show(msg);
                    //    return;
                    //}

                    AddLoggerGroupStatus("[ " + DateTime.Now + " ] => [ " + ListGrpDiscussion.Count + " Group Status Header Loaded  ]");
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update --> btnGroupDiscussion_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update --> btnGroupDiscussion_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinGetGroupMemberErrorLogs);
            }
        }

        #endregion

        #region btnGroupMoreDetail_Click

        private void btnGroupMoreDetail_Click(object sender, EventArgs e)
        {
            try
            {
                txtGroupMoreDtl.Text = "";
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "Text Files (*.txt)|*.txt";
                    ofd.InitialDirectory = Application.StartupPath;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtGroupMoreDtl.Text = ofd.FileName;
                        ListGrpMoreDetails = new List<string>();
                        ListGrpMoreDetails = GlobusFileHelper.ReadFiletoStringList(ofd.FileName);
                    }
                    AddLoggerGroupStatus("[ " + DateTime.Now + " ] => [ " + ListGrpMoreDetails.Count + " Group Status More Details Loaded  ]");
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update --> btnGroupMoreDetail_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update --> btnGroupMoreDetail_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinGetGroupMemberErrorLogs);
            }
        }

        #endregion

        #region btnAttaceLink_Click
        private void btnAttachLinkGroupUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                txtAttachLinkGroupUpdate.Text = "";
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "Text Files (*.txt)|*.txt";
                    ofd.InitialDirectory = Application.StartupPath;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtAttachLinkGroupUpdate.Text = ofd.FileName;
                        ListAttachLinkGrpDetails = new List<string>();
                        ListAttachLinkGrpDetails = GlobusFileHelper.ReadFiletoStringList(ofd.FileName);
                    }
                    AddLoggerGroupStatus("[ " + DateTime.Now + " ] => [ " + ListAttachLinkGrpDetails.Count + " Url for Attach Link Loaded ]");
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update --> btnAttaceLink_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update --> btnAttaceLink_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinGetGroupMemberErrorLogs);
            }
        }
        #endregion

        #region btnGetUser_Click

        Dictionary<string, Dictionary<string, string>> GrpMess = new Dictionary<string, Dictionary<string, string>>();

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
                        AddLoggerGroupStatus("[ " + DateTime.Now + " ] => [ Please Load LinkedIn Accounts From MyAccounts Menu ]");
                        lbGeneralLogs.Items.Clear();
                        frmAccounts FrmAccount = new frmAccounts();
                        FrmAccount.Show();
                        return;
                    }

                    GrpMess.Clear();
                    lstGroupUpdateThread.Clear();
                    if (IsStop)
                    {
                        IsStop = false;
                    }
                    lstGroupUpdate.Items.Clear();
                    chkGroupCollection.Items.Clear();
                    cmbGroupUser.Items.Clear();
                    cmbGroupUser.Items.Add("Select All Account");
                    AddLoggerGroupStatus("[ " + DateTime.Now + " ] => [ Process Start for Login...]");
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
                AddLoggerGroupStatus("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        }

        #endregion

        #region cmbGroupUser_SelectedIndexChanged

        private void cmbGroupUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<string> allacount = new List<string>();
            string GetUserID = string.Empty;
            chkGroupCollection.Items.Clear();
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
                            string[] group1 = group.Split(':');

                            if (GetUserID == group1[1].ToString())
                            {
                                chkGroupCollection.Items.Add(group1[1] + ':' + group1[0].ToString());
                                GroupStatus.GroupUrl.Add(item1.Value);
                            }
                        }
                    }

                    if (GetUserID == "Select All Account")
                    {
                        foreach (string Users in cmbGroupUser.Items)
                        {
                            string grpUser = Users;

                            //GetUserID = cmbGroupUser.SelectedItem.ToString();
                            foreach (KeyValuePair<string, string> item2 in item.Value)
                            {
                                string group1 = item2.Key;
                                string[] group2 = group1.Split(':');

                                if (grpUser == group2[1].ToString())
                                {
                                    //      chkGroupCollection.Items.Add(group2[1] + ':' + group2[0].ToString());
                                    GroupStatus.GroupUrl.Add(item2.Value);
                                    allacount.Add(group2[1] + ':' + group2[0].ToString());

                                    GroupStatus.GroupUrl = GroupStatus.GroupUrl.Distinct().ToList();
                                    allacount = allacount.Distinct().ToList();
                                }
                            }

                            AddLoggerGroupStatus("[ " + DateTime.Now + " ] => [ " + GroupStatus.GroupUrl.Count() + " Groups List of :" + "" + GetUserID + " ]");
                            AddLoggerGroupStatus("[ " + DateTime.Now + " ] => [ Finished Adding Groups of Usernames ]");
                        }
                    }
                }



                foreach (var aaaa in allacount)
                {
                    chkGroupCollection.Items.Add(aaaa);

                }


            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update --> cmbGroupUser_SelectedIndexChanged() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update --> cmbGroupUser_SelectedIndexChanged() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinGetGroupMemberErrorLogs);
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
                    for (int i = 0; i < chkGroupCollection.Items.Count; i++)
                    {
                        string aaa = Convert.ToString(chkGroupCollection.Items[i]);
                        chkGroupCollection.SetItemChecked(i, true);
                    }
                }
                else
                {
                    for (int i = 0; i < chkGroupCollection.Items.Count; i++)
                    {
                        string aaa = Convert.ToString(chkGroupCollection.Items[i]);
                        chkGroupCollection.SetItemChecked(i, false);
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
                AddLoggerGroupStatus("[ " + DateTime.Now + " ] => [ Please Load LinkedIn Accounts From MyAccounts Menu ]");
            }
        }

        #endregion

        #region StartDMMultiThreadedGroupUser

        public void StartDMMultiThreadedGroupUser(object parameter)
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

                Login.logger.addToLogger += new EventHandler(AddToLogger_GroupStatus);

                if (!Login.IsLoggedIn)
                {
                    Login.LoginHttpHelper(ref HttpHelper);
                }

                AddLoggerGroupStatus("[ " + DateTime.Now + " ] => [ Starting Search For Groups >>> To Send Group Messages ]");

                try
                {
                    if (Login.IsLoggedIn)
                    {
                        GroupStatus dataScrape = new GroupStatus();
                        Dictionary<string, string> Data = new Dictionary<string, string>();
                        Data.Clear();

                        Data = obj_GroupUpdate.PostCreateGroupNames(ref HttpHelper, Login.accountUser);
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

                        AddLoggerGroupStatus("[ " + DateTime.Now + " ] => [ Groups Added in : " + item.Value._Username + "]");
                    }
                    else
                    {
                        AddLoggerGroupStatus("[ " + DateTime.Now + " ] => [ LinkedIn account : " + Login.accountUser + " has been temporarily restricted ]");
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
                                AddLoggerGroupStatus("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                                AddLoggerGroupStatus("-------------------------------------------------------------------------------------------------------------------------");
                                cmbGroupUser.Enabled = true;
                                btnGetUser.Cursor = Cursors.Default;

                            }));
                        }

                        //btnGetUser.Invoke(new MethodInvoker(delegate
                        //{
                        //    AddLoggerGroupStatus("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                        //    AddLoggerGroupStatus("-------------------------------------------------------------------------------------------------------------------------");

                        //    btnGetUser.Cursor = Cursors.Default;
                        //}));     
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

        #region btnGroupUpdate_Click

        private void btnGroupUpdate_Click(object sender, EventArgs e)
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

                    if (chkGroupCollection.CheckedItems.Count == 0)
                    {
                        AddLoggerGroupStatus("[ " + DateTime.Now + " ] => [ Please select Atleast One Group.. ]");
                        MessageBox.Show("Please select Atleast One Group..");
                        return;
                    }

                    if (!chkAttachLinkGroupUpdate.Checked == true)
                    {

                        if (string.IsNullOrEmpty(txtGroupDiscussion.Text))
                        {
                            AddLoggerGroupStatus("[ " + DateTime.Now + " ] => [ Please Add Group Header.. ]");
                            MessageBox.Show("Please Add Group Header..");
                            btnGrpName.Focus();
                            return;
                        }
                        else if (string.IsNullOrEmpty(txtGroupMoreDtl.Text))
                        {
                            AddLoggerGroupStatus("[ " + DateTime.Now + " ] => [ Please Add Group More Details.. ]");
                            MessageBox.Show("[ " + DateTime.Now + " ] => [ Please Add Group More Details.. ]");
                            return;
                        }

                        if (msg != string.Empty)
                        {
                            MessageBox.Show(msg);
                            return;
                        }

                        //btnGroupUpdate.Enabled = false;
                        counter_GroupMemberSearch = 0;
                        btnGroupUpdate.Cursor = Cursors.AppStarting;

                        if (chkGroupCollection.CheckedItems.Count > 0 && ListGrpDiscussion.Count > 0)
                        {
                            // new Thread(() =>
                            {
                                LinkedInGroupMessage();
                            }
                            //}).Start();
                        }
                        else
                        {
                        }
                    }
                    else
                    {
                        if (chkAttachLinkGroupUpdate.Checked == true)
                        {
                            if (string.IsNullOrEmpty(txtAttachLinkGroupUpdate.Text))
                            {
                                AddLoggerGroupStatus("[ " + DateTime.Now + " ] => [ Please Add Url for Attach Link ]");
                                MessageBox.Show("[ " + DateTime.Now + " ] => [ Please Add Url for Attach Link ]");
                                btnGrpName.Focus();
                                return;
                            }
                        }

                        if (chkGroupCollection.CheckedItems.Count > 0 && ListAttachLinkGrpDetails.Count > 0)
                        {
                            new Thread(() =>
                            {
                                LinkedInGroupAttachLink();

                            }).Start();
                        }
                        else
                        {
                        }
                    }
                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update --> btnGroupUpdate_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update --> btnGroupUpdate_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinGetGroupMemberErrorLogs);
                }
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddLoggerGroupStatus("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        }

        #endregion

        #region LinkedInGroupMessage()

        private void LinkedInGroupMessage()
        {
            try
            {
                int numberOfThreads = 0;

                if (ListGrpDiscussion.Count > 0)
                {
                    foreach (string Message in ListGrpDiscussion)
                    {
                        Groups.GroupUpdate.Que_GrpPostTitle_Post.Enqueue(Message);
                    }
                }
                else
                {
                    AddLoggerGroupStatus("[ " + DateTime.Now + " ] => [ Please Add Group Title Message Text ]");
                    return;
                }

                if (GroupStatus.GroupUrl.Count > 0)
                {
                    foreach (string grpKey in GroupStatus.GroupUrl)
                    {
                        Groups.GroupUpdate.Que_GrpKey_Post.Enqueue(grpKey);
                    }
                }
                else
                {
                    AddLoggerGroupStatus("[ " + DateTime.Now + " ] => [ Group User Key Invalid ]");
                    return;
                }

                if (ListGrpMoreDetails.Count > 0)
                {
                    foreach (string grpmoredtl in ListGrpMoreDetails)
                    {
                        Groups.GroupUpdate.Que_GrpMoreDtl_Post.Enqueue(grpmoredtl);
                    }
                }
                else
                {
                    AddLoggerGroupStatus("[ " + DateTime.Now + " ] => [ Please Add Group MoreDetails Message Text ]");
                    return;
                }

                if (chkSameMessageForAllGroup.Checked)
                {
                    Groups.GroupUpdate.Que_GrpPostTitle_Post.Clear();
                    Groups.GroupUpdate.Que_GrpMoreDtl_Post.Clear();
                    if (ListGrpDiscussion.Count > 0)
                    {
                        foreach (string Message in ListGrpDiscussion)
                        {
                            Groups.GroupUpdate.Que_GrpPostTitle_Post.Enqueue(Message);
                            break;
                        }
                    }
                    else
                    {
                        AddLoggerGroupStatus("[ " + DateTime.Now + " ] => [ Please Add Group Title Message Text ]");
                        return;
                    }



                    if (ListGrpMoreDetails.Count > 0)
                    {
                        foreach (string grpmoredtl in ListGrpMoreDetails)
                        {
                            Groups.GroupUpdate.Que_GrpMoreDtl_Post.Enqueue(grpmoredtl);
                            break;
                        }
                    }
                    else
                    {
                        AddLoggerGroupStatus("[ " + DateTime.Now + " ] => [ Please Add Group MoreDetails Message Text ]");
                        return;
                    }
                }
                if (chkAttachLinkGroupUpdate.Checked == true)
                {
                    if (ListAttachLinkGrpDetails.Count > 0)
                    {
                        if (ListAttachLinkGrpDetails.Count > 1)
                        {
                            foreach (string Message in ListAttachLinkGrpDetails)
                            {

                                Groups.GroupUpdate.Que_GrpAttachLink_Post.Enqueue(Message);
                            }
                        }
                        else
                        {
                            Groups.GroupUpdate.AttachLink_Post = ListAttachLinkGrpDetails[0];
                        }
                    }
                    else
                    {
                        AddLoggerGroupStatus("[ " + DateTime.Now + " ] => [ Please Add Url for Attach Link ]");
                        return;
                    }
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

                            ThreadPool.QueueUserWorkItem(new WaitCallback(PostGroupMsgUpdate), new object[] { item });

                            Thread.Sleep(1000);
                        }

                        if (value.Contains("Select All Account"))
                        {
                            ThreadPool.SetMaxThreads(numberOfThreads, 5);

                            ThreadPool.QueueUserWorkItem(new WaitCallback(PostGroupMsgUpdate), new object[] { item });

                            Thread.Sleep(1000);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update --> LinkedInGroupMessage() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update --> LinkedInGroupMessage() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinGetGroupMemberErrorLogs);
            }
        }

        #endregion

        #region PostGroupMsgUpdate

        public void PostGroupMsgUpdate(object parameter)
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
                }

                List<string> GetCheckedItems = new List<string>();

                if (chkGroupCollection.InvokeRequired)
                {
                    chkGroupCollection.Invoke(new MethodInvoker(delegate
                    {
                        foreach (string Userid in chkGroupCollection.CheckedItems)
                        {
                            string[] Uid = Userid.Split(':');
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
                                    SelectedItem.Add(GroupId + ":" + selectedgroup);

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

                if (chkSameMessageForAllGroup.Checked)
                {

                    obj_GroupUpdate.PostGroupSameMessageForAllGroup(ref HttpHelper, SelectedItem, new object[] { item, user }, minDelay, maxDelay);
                    return;
                }


                if (!chkAttachLinkGroupUpdate.Checked == true)
                {
                    obj_GroupUpdate.PostGroupMsg(ref HttpHelper, SelectedItem, new object[] { item, user }, minDelay, maxDelay);

                }
                else
                {
                    obj_GroupUpdate.PostAttachLinkGroupUpdate(ref HttpHelper, SelectedItem, new object[] { item, user }, minDelay, maxDelay);
                }

                Thread.Sleep(2000);
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update --> PostGroupMsgUpdate() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update --> PostGroupMsgUpdate() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinGetGroupMemberErrorLogs);
            }
            finally
            {
                counter_GroupMemberSearch--;
                int cnt = LinkedInManager.linkedInDictionary.Count - 1;
                if (counter_GroupMemberSearch == cnt)
                {
                    if (btnGroupUpdate.InvokeRequired)
                    {
                        btnGroupUpdate.Invoke(new MethodInvoker(delegate
                        {
                            AddLoggerGroupStatus("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                            AddLoggerGroupStatus("----------------------------------------------------------------------------------------------------------------------------");
                            btnGroupUpdate.Enabled = true;
                            btnGroupUpdate.Cursor = Cursors.Default;

                        }));
                    }
                }

            }
        }

        #endregion

        #region LinkedInGroupAttachLink
        private void LinkedInGroupAttachLink()
        {
            try
            {
                int numberOfThreads = 0;

                if (chkAttachLinkGroupUpdate.Checked == true)
                {
                    if (ListAttachLinkGrpDetails.Count > 0)
                    {
                        if (ListAttachLinkGrpDetails.Count > 1)
                        {
                            foreach (string Message in ListAttachLinkGrpDetails)
                            {

                                Groups.GroupUpdate.Que_GrpAttachLink_Post.Enqueue(Message);
                            }
                        }
                        else
                        {
                            Groups.GroupUpdate.AttachLink_Post = ListAttachLinkGrpDetails[0];
                        }
                    }

                    else
                    {
                        AddLoggerGroupStatus("[ " + DateTime.Now + " ] => [ Please Add Url for Attach Link ]");
                        return;
                    }
                }

                if (GroupStatus.GroupUrl.Count > 0)
                {
                    foreach (string grpKey in GroupStatus.GroupUrl)
                    {
                        Groups.GroupUpdate.Que_GrpKey_Post.Enqueue(grpKey);
                    }
                }
                else
                {
                    AddLoggerGroupStatus("[ " + DateTime.Now + " ] => [ Group User Key Invalid ]");
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

                            ThreadPool.QueueUserWorkItem(new WaitCallback(PostGroupMsgUpdate), new object[] { item });

                            Thread.Sleep(1000);
                        }

                        if (value.Contains("Select All Account"))
                        {
                            ThreadPool.SetMaxThreads(numberOfThreads, 5);

                            ThreadPool.QueueUserWorkItem(new WaitCallback(PostGroupMsgUpdate), new object[] { item });

                            Thread.Sleep(1000);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update --> PostGroupMsgUpdate() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update --> PostGroupMsgUpdate() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinGetGroupMemberErrorLogs);
            }
        }
        #endregion

        #region PostGroupMsg

        public void PostGroupMsg(List<string> selectedItems, object parameter)
        {
            string postdata = string.Empty;
            string postUrl = string.Empty;
            string ResLogin = string.Empty;
            string csrfToken = string.Empty;
            string sourceAlias = string.Empty;

            string ReturnString = string.Empty;
            string PostGrpDiscussion = string.Empty;
            string PostGrpMoreDetails = string.Empty;
            string PostGrpKey = string.Empty;

            try
            {
                string MessageText = string.Empty;
                string PostedMessage = string.Empty;
                string pageSource = string.Empty;

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

                if (!Login.IsLoggedIn)
                {
                    Login.LoginHttpHelper(ref HttpHelper);
                }

                pageSource = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/home?trk=hb_tab_home_top"));

                if (pageSource.Contains("csrfToken"))
                {
                    string pattern = @"\";
                    csrfToken = pageSource.Substring(pageSource.IndexOf("csrfToken"), 50);
                    string[] Arr = csrfToken.Split('&');
                    csrfToken = Arr[0];
                    csrfToken = csrfToken.Replace("csrfToken", "").Replace("\"", string.Empty).Replace("value", string.Empty).Replace("cs", string.Empty).Replace("id", string.Empty).Replace("=", string.Empty);
                    csrfToken = csrfToken.Replace(pattern, string.Empty.Trim());
                }

                if (pageSource.Contains("sourceAlias"))
                {
                    string pattern1 = @"\";
                    sourceAlias = pageSource.Substring(pageSource.IndexOf("sourceAlias"), 100);
                    string[] Arr = sourceAlias.Split('"');
                    sourceAlias = Arr[2];
                    sourceAlias = sourceAlias.Replace(pattern1, string.Empty.Trim());
                }

                //postUrl = "https://www.linkedin.com/uas/login-submit";
                //postdata = "session_key=" + Login.accountUser + "&session_password=" + Login.accountPass + "&source_app=&trk=guest_home_login&session_redirect=&csrfToken=" + csrfToken + "&sourceAlias=" + sourceAlias;

                //ResLogin = HttpHelper.postFormData(new Uri(postUrl), postdata);

                try
                {
                    string PostGroupstatus;
                    string ResponseStatusMsg;
                    foreach (var Itegid in selectedItems)
                    {
                        lock (GroupStatus.Locked_GrpKey_Post)
                        {
                            if (GroupStatus.Que_GrpKey_Post.Count > 0)
                            {
                                try
                                {
                                    PostGrpKey = GroupStatus.Que_GrpKey_Post.Dequeue();
                                }
                                catch
                                {
                                }
                            }
                        }

                        lock (GroupStatus.Locked_GrpPostTitle_Post)
                        {
                            if (GroupStatus.Que_GrpPostTitle_Post.Count > 0)
                            {
                                try
                                {
                                    PostGrpDiscussion = GroupStatus.Que_GrpPostTitle_Post.Dequeue();
                                }
                                catch
                                {
                                }
                            }
                        }

                        lock (GroupStatus.Locked_GrpMoreDtl_Post)
                        {
                            if (GroupStatus.Que_GrpMoreDtl_Post.Count > 0)
                            {
                                try
                                {
                                    PostGrpMoreDetails = GroupStatus.Que_GrpMoreDtl_Post.Dequeue();
                                }
                                catch
                                {
                                }
                            }
                        }

                        string[] grpDisplay = Itegid.Split(':');
                        string GrpName = Itegid.ToString().Replace(",", ":").Replace("[", string.Empty).Replace("]", string.Empty).Trim();
                        string[] PostGid = GrpName.Split(':');
                        string Gid = string.Empty;

                        try
                        {
                            if (NumberHelper.ValidateNumber(PostGid[1].Trim()))
                            {
                                Gid = PostGid[1].Trim();
                            }
                            else if (NumberHelper.ValidateNumber(PostGid[2].Trim()))
                            {
                                Gid = PostGid[2].Trim();
                            }
                            else if (NumberHelper.ValidateNumber(PostGid[3].Trim()))
                            {
                                Gid = PostGid[3].Trim();
                            }
                            else if (NumberHelper.ValidateNumber(PostGid[4].Trim()))
                            {
                                Gid = PostGid[4].Trim();
                            }
                            else if (NumberHelper.ValidateNumber(PostGid[5].Trim()))
                            {
                                Gid = PostGid[5].Trim();
                            }
                            else if (NumberHelper.ValidateNumber(PostGid[6].Trim()))
                            {
                                Gid = PostGid[6].Trim();
                            }
                        }
                        catch
                        {
                        }

                        PostGroupstatus = "csrfToken=" + csrfToken + "&postTitle=" + PostGrpDiscussion + "&postText=" + PostGrpMoreDetails + "&pollChoice1-ANetPostForm=&pollChoice2-ANetPostForm=&pollChoice3-ANetPostForm=&pollChoice4-ANetPostForm=&pollChoice5-ANetPostForm=&pollEndDate-ANetPostForm=0&contentImageCount=0&contentImageIndex=-1&contentImage=&contentEntityID=&contentUrl=&contentTitle=&contentSummary=&contentImageIncluded=true&%23=&gid=" + Gid.Trim() + "&postItem=&ajax=true&tetherAccountID=&facebookTetherID=";

                        ResponseStatusMsg = HttpHelper.postFormData(new Uri("http://www.linkedin.com/groups"), PostGroupstatus);
                        Thread.Sleep(2000);

                        if (ResponseStatusMsg.Contains("SUCCESS"))
                        {
                            AddLoggerGroupStatus("[ " + DateTime.Now + " ] => [ Message Header Posted : " + PostGrpDiscussion + " Successfully on Group : " + grpDisplay[2] + " ]");
                            AddLoggerGroupStatus("[ " + DateTime.Now + " ] => [ Message More Details Posted : " + PostGrpMoreDetails + " Successfully on Group : " + grpDisplay[2] + " ]");

                            GlobusFileHelper.AppendStringToTextfileNewLine("Message Header Posted : " + PostGrpDiscussion + " Successfully on Group : " + grpDisplay[2], Globals.path_GroupUpdate);
                            GlobusFileHelper.AppendStringToTextfileNewLine("Message More Details Posted : " + PostGrpMoreDetails + " Successfully on Group : " + grpDisplay[2], Globals.path_GroupUpdate);
                        }
                        else if (ResponseStatusMsg.Contains("Your request to join is still pending"))
                        {
                            AddLoggerGroupStatus("[ " + DateTime.Now + " ] => [ Your membership is pending approval on a Group:" + grpDisplay[2] + " ]");
                            AddLoggerGroupStatus("[ " + DateTime.Now + " ] => [ Message Header: " + PostGrpDiscussion + " Not Posted on Group:" + grpDisplay[2] + " Because Your membership is pending for approval. ]");
                            AddLoggerGroupStatus("[ " + DateTime.Now + " ] => [ Message More Details: " + PostGrpMoreDetails + " Not Posted on Group:" + grpDisplay[2] + " Because Your membership is pending for approval. ]");

                            GlobusFileHelper.AppendStringToTextfileNewLine("Your membership is pending approval on a Group:" + grpDisplay[2], Globals.path_GroupUpdate);
                            GlobusFileHelper.AppendStringToTextfileNewLine("Message Header: " + PostGrpDiscussion + " Not Posted on Group:" + grpDisplay[2] + " Because Your membership is pending for approval. ", Globals.path_GroupUpdate);
                            GlobusFileHelper.AppendStringToTextfileNewLine("Message More Details: " + PostGrpMoreDetails + " Not Posted on Group:" + grpDisplay[2] + " Because Your membership is pending for approval. ", Globals.path_GroupUpdate);
                        }
                        else if (ResponseStatusMsg.Contains("Your post has been submitted for review"))
                        {
                            AddLoggerGroupStatus("[ " + DateTime.Now + " ] => [ Message Header Posted : " + PostGrpDiscussion + " Successfully on Group : " + grpDisplay[2] + " ]");
                            AddLoggerGroupStatus("[ " + DateTime.Now + " ] => [ Message More Details Posted : " + PostGrpMoreDetails + " Successfully on Group : " + grpDisplay[2] + " ]");
                            AddLoggerGroupStatus("[ " + DateTime.Now + " ] => [ Your post has been submitted for review ]");

                            GlobusFileHelper.AppendStringToTextfileNewLine("Message Header Posted : " + PostGrpDiscussion + " Successfully on Group : " + grpDisplay[2], Globals.path_GroupUpdate);
                            GlobusFileHelper.AppendStringToTextfileNewLine("Message More Details Posted : " + PostGrpMoreDetails + " Successfully on Group : " + grpDisplay[2], Globals.path_GroupUpdate);
                            GlobusFileHelper.AppendStringToTextfileNewLine("Your post has been submitted for review", Globals.path_GroupUpdate);
                        }
                        else if (ResponseStatusMsg.Contains("Error"))
                        {
                            AddLoggerCreateGroup("[ " + DateTime.Now + " ] => [ Error in Post ]");
                            GlobusFileHelper.AppendStringToTextfileNewLine("Error in Post", Globals.path_GroupUpdate);
                        }
                        else
                        {
                            AddLoggerCreateGroup("[ " + DateTime.Now + " ] => [ Message Not Posted ]");
                            GlobusFileHelper.AppendStringToTextfileNewLine("Message Not Posted", Globals.path_GroupUpdate);
                        }
                    }
                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update --> cmbGroupUser_SelectedIndexChanged() ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update --> cmbGroupUser_SelectedIndexChanged() ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinGetGroupMemberErrorLogs);
                    //AddLoggerGroupStatus("[ " + DateTime.Now + " ] => [ Error:" + ex.Message + "StackTrace --> >>>" + ex.StackTrace + " ]");
                }
                finally
                {
                    counter_GroupMemberSearch--;
                    int cnt = LinkedInManager.linkedInDictionary.Count - 1;
                    if (counter_GroupMemberSearch == cnt)
                    {
                        if (btnGroupUpdate.InvokeRequired)
                        {
                            btnGroupUpdate.Invoke(new MethodInvoker(delegate
                            {
                                //AddLoggerGroupStatus("Process Complete..");
                                btnGroupUpdate.Enabled = true;

                            }));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update --> cmbGroupUser_SelectedIndexChanged() ---2--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update --> cmbGroupUser_SelectedIndexChanged() ---2--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinGetGroupMemberErrorLogs);
            }
        }

        #endregion

        //*=====================================================================Compose Message========================================================================================*/

        #region btnMessSubject_Click

        private void btnMessSubject_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "Text Files (*.txt)|*.txt";
                    ofd.InitialDirectory = Application.StartupPath;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtMsgSubject.Text = ofd.FileName;
                        ListMsgSubject = new List<string>();

                        ListMsgSubject = GlobusFileHelper.ReadFiletoStringList(ofd.FileName);
                    }

                    AddLoggerComposeMessage("[ " + DateTime.Now + " ] => [ " + ListMsgSubject.Count + " Message Subject Loaded  ]");
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnMessSubject_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnMessSubject_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinComposeMessageErrorLogs);
            }
        }

        #endregion

        #region btnMsgBody_Click

        private void btnMsgBody_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "Text Files (*.txt)|*.txt";
                    ofd.InitialDirectory = Application.StartupPath;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtMsgBody.Text = ofd.FileName;
                        ListMsgBody = new List<string>();

                        ListMsgBody = GlobusFileHelper.ReadFiletoStringList(ofd.FileName);
                    }

                    AddLoggerComposeMessage("[ " + DateTime.Now + " ] => [ " + ListMsgBody.Count + " Message Body Loaded  ]");
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnMsgBody_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnMsgBody_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinComposeMessageErrorLogs);
            }
        }

        #endregion

        #region btnMsgFrom_Click

        static int counter_compose_msg = 0;

        private void btnMsgFrom_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {

                try
                {
                    lstComposeMessageThread.Clear();
                    if (Globals.IsStop)
                    {
                        IsStop = false;
                    }
                    AddLoggerComposeMessage("[ " + DateTime.Now + " ] => [ Starting Compose Message ]");
                    try
                    {
                        if (LinkedInManager.linkedInDictionary.Count() == 0)
                        {
                            MessageBox.Show("Please Load LinkedIn Accounts From MyAccounts Menu");
                            AddLoggerComposeMessage("[ " + DateTime.Now + " ] => [ Please Load LinkedIn Accounts From MyAccounts Menu ]");

                            lbGeneralLogs.Items.Clear();
                            frmAccounts FrmAccount = new frmAccounts();
                            FrmAccount.Show();
                            return;
                        }

                        MessageContacts.Clear();
                        GrpMess.Clear();
                        LinkdInContacts.Clear();
                        LstComposeMsg.Items.Clear();
                        chkMessageTo.Items.Clear();
                        cmbMsgFrom.Items.Clear();
                        btnMsgFrom.Cursor = Cursors.AppStarting;

                        new Thread(() =>
                        {
                            LinkdinAddFromID();

                        }).Start();
                    }
                    catch (Exception ex)
                    {
                        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnMsgFrom_Click() --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnMsgFrom_Click() --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinComposeMessageErrorLogs);
                    }
                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnMsgFrom_Click() --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnMsgFrom_Click() --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinComposeMessageErrorLogs);
                }
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddLoggerComposeMessage("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        }

        #endregion

        #region LinkdinAddFromID()

        private void LinkdinAddFromID()
        {
            try
            {
                if (!Globals.IsStop)
                {
                    Globals.lstComposeMessageThread.Add(Thread.CurrentThread);
                    Globals.lstComposeMessageThread.Distinct().ToList();
                    Thread.CurrentThread.IsBackground = true;
                }
            }
            catch
            { }
            try
            {
                AddLoggerComposeMessage("[ " + DateTime.Now + " ] => [ Starting Search For Contacts To Send Messages ]");

                int numberofThreds = 5;
                if (LinkedInManager.linkedInDictionary.Count() > 0)
                {
                    counter_compose_msg = LinkedInManager.linkedInDictionary.Count;

                    ThreadPool.SetMaxThreads(numberofThreds, 5);

                    foreach (KeyValuePair<string, LinkedInMaster> item in LinkedInManager.linkedInDictionary)
                    {
                        try
                        {
                            ThreadPool.SetMaxThreads(5, 5);

                            ThreadPool.QueueUserWorkItem(new WaitCallback(StartDMMultiThreadedComposeMessage), new object[] { item });

                            Thread.Sleep(2*1000);
                        }
                        catch (Exception ex)
                        {
                            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> LinkdinAddFromID() --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> LinkdinAddFromID() --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinComposeMessageErrorLogs);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please Load LinkedIn Accounts From MyAccounts Menu");
                    AddLoggerComposeMessage("[ " + DateTime.Now + " ] => [ Please Load LinkedIn Accounts From MyAccounts Menu ]");
                }
            }
            catch
            {
            }
        }

        #endregion

        #region StartDMMultiThreadedComposeMessage
        bool IsStopMessage = false;
        public void StartDMMultiThreadedComposeMessage(object parameter)
        {
            try
            {
                if (Globals.IsStop)
                {
                    if (IsStopMessage)
                    {
                        return;
                    }
                    return;
                }
                Globals.lstComposeMessageThread.Add(Thread.CurrentThread);
                Globals.lstComposeMessageThread.Distinct().ToList();
                Thread.CurrentThread.IsBackground = true;
            }
            catch
            { }
            string Account = string.Empty;
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

                Account = item.Key;

                GlobusHttpHelper HttpHelper = new GlobusHttpHelper();
                GroupStatus MemberScrape = new GroupStatus();
                Login.logger.addToLogger += new EventHandler(ComposeMessage_addToLogger);
                MemberScrape.loggergrpupdate.addToLogger += new EventHandler(ComposeMessage_addToLogger);


                if (!Login.IsLoggedIn)
                {
                    //AddLoggerComposeMessage("[ " + DateTime.Now + " ] => [ Logging In With Email : " + item.Value._Username + " ]");
                    //                    AddLoggerComposeMessage("[ " + DateTime.Now + " ] => [ Login Process is Running... ]");
                    Login.LoginHttpHelper(ref HttpHelper);
                    if (Login.IsLoggedIn)
                    {
                        //AddLoggerComposeMessage("[ " + DateTime.Now + " ] => [ LoggedIn With Email : " + item.Value._Username + "]");
                    }
                    else
                    {
                        Login.LoginHttpHelper(ref HttpHelper);
                    }
                }

                try
                {
                    if (Login.IsLoggedIn)
                    {
                        //GroupStatus MemberScrape = new GroupStatus();
                        AddLoggerComposeMessage("[ " + DateTime.Now + " ] => [ Process Adding Members From Uploaded Emails is Running... ]");
                        if (GroupStatus.withExcelInput == true)
                        {
                            chkMessageTo.Invoke(new MethodInvoker(delegate
                            {
                                chkMessageTo.Items.Clear();
                                MessageContacts.Clear();
                            }));

                            Dictionary<string, string> Result = MemberScrape.PostaddMembersWithExcelInput(ref HttpHelper, Login.accountUser);
                            MessageContacts.Add(Login.accountUser, Result);
                        }
                        else
                        {
                            chkMessageTo.Invoke(new MethodInvoker(delegate
                            {
                                chkMessageTo.Items.Clear();
                                MessageContacts.Clear();
                            }));

                            GroupStatus.moduleLog = "composemsg";
                            Dictionary<string, string> Result = MemberScrape.PostAddMembers(ref HttpHelper, Login.accountUser);
                            MessageContacts.Add(Login.accountUser, Result);
                        }

                        AddLoggerComposeMessage("[ " + DateTime.Now + " ] => [ Member Added Successfully : " + Login.accountUser + " ]");

                        if (cmbMsgFrom.InvokeRequired)
                        {
                            new Thread(() =>
                            {
                                cmbMsgFrom.Invoke(new MethodInvoker(delegate
                                {
                                    cmbMsgFrom.Items.Add(Login.accountUser);
                                    cmbMsgFrom.SelectedIndex = 0;
                                }));
                            }).Start();
                        }
                    }
                    else
                    {
                        AddLoggerComposeMessage("[ " + DateTime.Now + " ] => [ Account : " + Login.accountUser + " Not Logged In ]");
                    }


                    Login.logger.addToLogger -= new EventHandler(ComposeMessage_addToLogger);
                    MemberScrape.loggergrpupdate.addToLogger += new EventHandler(ComposeMessage_addToLogger);


                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> StartDMMultiThreadedComposeMessage() -->  Getting Contact Values --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> StartDMMultiThreadedComposeMessage() -->  Getting Contact Values -->" + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinComposeMessageErrorLogs);
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> StartDMMultiThreadedComposeMessage() --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> StartDMMultiThreadedComposeMessage() --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinComposeMessageErrorLogs);
            }
            finally
            {
                if (IsStopMessage)
                {
                    AddLoggerComposeMessage("[ " + DateTime.Now + " ] => [ Finished Adding Contacts To Email : " + Account + " ]");

                    counter_compose_msg--;
                    if (counter_compose_msg == 0)
                    {
                        if (btnMsgFrom.InvokeRequired)
                        {
                            btnMsgFrom.Invoke(new MethodInvoker(delegate
                            {
                                AddLoggerComposeMessage("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                                AddLoggerComposeMessage("--------------------------------------------------------------------------------------------------------------------------------");
                                btnMsgFrom.Cursor = Cursors.Default;
                            }));
                        }
                    }
                }
            }
        }

        #endregion

        #region cmbMsgFrom_SelectedIndexChanged

        string FromSendMsg = string.Empty;

        private void cmbMsgFrom_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                chkMessageTo.Items.Clear();
                GroupStatus.GroupUrl.Clear();

                string GetUserID = cmbMsgFrom.SelectedItem.ToString();
                FromSendMsg = cmbMsgFrom.SelectedItem.ToString();
                lblTotFriendList.Text = "(" + "0" + ")";

                try
                {
                    foreach (KeyValuePair<string, Dictionary<string, string>> item in MessageContacts)
                    {
                        if (GetUserID.Contains(item.Key))
                        {
                            //AddLoggerComposeMessage(" Please Wait While We Add Username To Send Group Message");
                            List<string> GmUserID = new List<string>();
                            foreach (KeyValuePair<string, string> item1 in item.Value)
                            {
                                string group = item1.Key;
                                string[] group1 = group.Split(':');

                                //if (GetUserID == item.Key.ToString())
                                if (GetUserID == group1[0])
                                {
                                    chkMessageTo.Items.Add(item1.Value.Replace(",", ""));
                                    GmUserID.Add(item1.Value.Replace(",", ""));
                                }
                            }
                            AddLoggerComposeMessage("[ " + DateTime.Now + " ] => [ " + GmUserID.Count() + " Friend List of :" + " " + item.Key + " ]");
                            AddLoggerComposeMessage("[ " + DateTime.Now + " ] => [ Finished Adding Friend List of :" + "" + item.Key + " ]");
                        }
                    }
                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Compose Message --> cmbMsgFrom_SelectedIndexChanged() --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Compose Message --> cmbMsgFrom_SelectedIndexChanged() --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinComposeMessageErrorLogs);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error >>> " + ex.StackTrace);
            }


        }

        #endregion

        #region chkSelectAll_CheckedChanged

        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkSelectAll.Checked == true)
                {
                    for (int i = 0; i < chkMessageTo.Items.Count; i++)
                    {
                        string aaa = Convert.ToString(chkMessageTo.Items[i]);
                        chkMessageTo.SetItemChecked(i, true);
                        lblTotFriendList.Text = "(" + chkMessageTo.Items.Count + ")";
                    }
                }
                else
                {
                    for (int i = 0; i < chkMessageTo.Items.Count; i++)
                    {
                        string aaa = Convert.ToString(chkMessageTo.Items[i]);
                        chkMessageTo.SetItemChecked(i, false);
                        int TotSelected = 0;
                        lblTotFriendList.Text = "(" + TotSelected + ")";
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Compose Message --> chkSelectAll_CheckedChanged() --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Compose Message --> chkSelectAll_CheckedChanged() --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinComposeMessageErrorLogs);
            }
        }

        #endregion

        #region chkMessageTo_SelectedIndexChanged
        private void chkMessageTo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int count = Convert.ToInt32(lblTotFriendList.Text.Replace("(", string.Empty).Replace(")", string.Empty));
                int count2 = chkMessageTo.CheckedItems.Count + 1;
                if (chkMessageTo.CheckOnClick == true)
                {
                    if (count < count2)
                    {
                        lblTotFriendList.Text = "(" + (count + 1).ToString() + ")";
                    }
                    else
                    {
                        lblTotFriendList.Text = "(" + (count - 1).ToString() + ")";
                    }
                }
            }
            catch { }
        }
        #endregion

        #region btnSendMsg_Click

        int NoOfAccountsLoggedin = 0;
        string SelectedEmail = string.Empty;
        private void btnSendMsg_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {

                try
                {
                    MessagelistCompose.Clear();
                    lstComposeMessageThread.Clear();
                    if (IsStop)
                    {
                        IsStop = false;
                    }

                    NoOfAccountsLoggedin = LinkedInManager.linkedInDictionary.Count();

                    if (!ChkAllAccounts_ComposeMessage.Checked)
                    {
                        if (LinkedInManager.linkedInDictionary.Count() == 0)
                        {
                            return;
                        }
                        else if (cmbMsgFrom.Items.Count == 0)
                        {
                            AddLoggerComposeMessage("[ " + DateTime.Now + " ] => [ Please click message from button.. ]");
                            MessageBox.Show("Please click Message From button..");
                            return;
                        }
                        else if (chkMessageTo.Items.Count == 0)
                        {
                            AddLoggerComposeMessage("[ " + DateTime.Now + " ] => [ Please select message from ID.. ]");
                            MessageBox.Show("Please select message from ID..");
                            return;
                        }
                        else if (chkMessageTo.CheckedItems.Count == 0)
                        {
                            AddLoggerComposeMessage("[ " + DateTime.Now + " ] => [ Please select Atleast One ID.. ]");
                            MessageBox.Show("Please select Atleast One ID..");
                            return;
                        }

                    }

                    if (string.IsNullOrEmpty(txtMsgSubject.Text))
                    {
                        AddLoggerComposeMessage("[ " + DateTime.Now + " ] => [ Please Add Subject.. ]");
                        MessageBox.Show("Please Add Subject..");
                        btnGrpName.Focus();
                        return;
                    }
                    else if (string.IsNullOrEmpty(txtMsgBody.Text))
                    {
                        AddLoggerComposeMessage("[ " + DateTime.Now + " ] => [ Please Add Message Body.. ]");
                        MessageBox.Show("Please Add Message Body..");
                        return;
                    }

                    if (txtMsgBody.Text.Contains("|"))
                    {
                        if (!(chkSpinTaxComposeMsg.Checked))
                        {
                            AddLoggerComposeMessage("[ " + DateTime.Now + " ] => [ Please Check SpinTax CheckBox.. ]");
                            MessageBox.Show("Please Check SpinTax CheckBox..");
                            return;
                        }

                        if (txtMsgBody.Text.Contains("{") || txtMsgBody.Text.Contains("}"))
                        {
                            AddLoggerComposeMessage("[ " + DateTime.Now + " ] => [ Its a wrong SpinTax Format.. ]");
                            MessageBox.Show("Its a wrong SpinTax Format..");
                            return;
                        }
                    }

                    if (chkSpinTaxComposeMsg.Checked)
                    {
                        msg_spintaxt = true;
                        //MessagelistCompose = SpinnedListGenerator.GetSpinnedList(new List<string> { txtMsgBody.Text });
                        subjectlistCompose = SpinnedListGenerator.GetSpinnedList(new List<string> { txtMsgSubject.Text });
                        //MessagelistCompose = GlobusSpinHelper.spinLargeText(new Random(), txtMsgBody.Text);
                        messageSpin = GlobusSpinHelper.spinLargeText(new Random(), txtMsgBody.Text);
                    }
                    else
                    {
                        msg_spintaxt = false;
                    }
                    if (ChkAllAccounts_ComposeMessage.Checked)
                    {
                        try
                        {
                            ComposeMessage.ComposeMessage.IsAllAccounts = true;

                            try
                            {
                                ComposeMessage.ComposeMessage.NoOfFriends = Convert.ToInt32(TxtNoOfFriends_ComposeMessage.Text);
                            }
                            catch
                            {
                                AddLoggerComposeMessage("[ " + DateTime.Now + " ] => [ Please Enter Numeric Value In No. Of Friends Field ! ]");
                                ComposeMessage.ComposeMessage.NoOfFriends = 5;
                                TxtNoOfFriends_ComposeMessage.Text = "5";
                            }
                        }
                        catch
                        {
                        }
                    }

                    btnSendMsg.Cursor = Cursors.AppStarting;

                    new Thread(() =>
                    {
                        LinkedInComposeMessage();

                    }).Start();

                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnSendMsg_Click() --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnSendMsg_Click() --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinComposeMessageErrorLogs);
                }
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddLoggerComposeMessage("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        }

        #endregion

        #region LinkedInComposeMessage()

        private void LinkedInComposeMessage()
        {
            AddLoggerComposeMessage("[ " + DateTime.Now + " ] => [ Starting Sending Message To Selected Contacts ]");
            int numberOfThreads = 0;

            try
            {
                try
                {
                    if (!Globals.IsStop)
                    {
                        Globals.lstComposeMessageThread.Add(Thread.CurrentThread);
                        Globals.lstComposeMessageThread.Distinct().ToList();
                        Thread.CurrentThread.IsBackground = true;
                    }
                }
                catch
                { }
                try
                {
                    cmbMsgFrom.Invoke(new MethodInvoker(delegate
                    {
                        UserSlecetedDetails = cmbMsgFrom.SelectedItem.ToString();
                    }));
                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Compose Message --> LinkedInComposeMessage() --> Getting EMail from Combobox--> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Compose Message --> LinkedInComposeMessage() --> Getting EMail from Combobox " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinComposeMessageErrorLogs);
                }

                if (LinkedInManager.linkedInDictionary.Count() > 0)
                {
                    ThreadPool.SetMaxThreads(numberOfThreads, 5);
                    string value = string.Empty;
                    cmbMsgFrom.Invoke(new MethodInvoker(delegate
                    {
                        value = cmbMsgFrom.SelectedItem.ToString();
                    }));


                    foreach (KeyValuePair<string, LinkedInMaster> item in LinkedInManager.linkedInDictionary)
                    {
                        if (!ChkAllAccounts_ComposeMessage.Checked)
                        {
                            if (item.Key == value)
                            {
                                PostMessageBulk(new object[] { item });
                            }

                        }
                        else
                        {
                            try
                            {
                                ThreadPool.SetMaxThreads(numberOfThreads, 5);
                                ThreadPool.QueueUserWorkItem(new WaitCallback(PostMessageBulk), new object[] { item });
                                Thread.Sleep(1000);

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Error >>> " + ex.StackTrace);
                            }
                        }

                        #region old code

                        //if (value.Contains(item.Key))
                        //{
                        //    PostMessageBulk(new object[] { item });
                        //    //ThreadPool.SetMaxThreads(numberOfThreads, 5);
                        //    //ThreadPool.QueueUserWorkItem(new WaitCallback(PostMessageBulk), new object[] { item });
                        //    //Thread.Sleep(1000);
                        //} 

                        #endregion

                    }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Compose Message --> LinkedInComposeMessage() --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Compose Message --> LinkedInComposeMessage() --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinComposeMessageErrorLogs);
            }
        }

        #endregion

        #region PostMessageBulk

        public void PostMessageBulk(object parameter)
        {
            try
            {
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

                ComposeMessage.ComposeMessage obj_ComposeMessage = new ComposeMessage.ComposeMessage();
                obj_ComposeMessage.logger.addToLogger += new EventHandler(ComposeMessage_addToLogger);

                AddLoggerComposeMessage("[ " + DateTime.Now + " ] => [ Sending Message From Account : " + item.Key + " ]");
                string UserEmail = string.Empty;
                UserEmail = item.Key;
                try
                {
                    if (!Login.IsLoggedIn)
                    {
                        Login.LoginHttpHelper(ref HttpHelper);
                    }
                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Compose Message --> PostMessageBulk() -->  Login Process >>>>" + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Compose Message --> PostMessageBulk() -->  Login Process >>>>" + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinComposeMessageErrorLogs);
                }

                try
                {
                    if (Login.IsLoggedIn)
                    {
                        AddLoggerComposeMessage("[ " + DateTime.Now + " ] => [ Logged In With Email : " + item.Value._Username + " ]");
                        AddLoggerComposeMessage("[ " + DateTime.Now + " ] => [ Getting Contacts to Send Message ]");

                        List<string> SelectedItem = new List<string>();
                        string Userid = string.Empty;
                        if (cmbMsgFrom.InvokeRequired)
                        {
                            cmbMsgFrom.Invoke(new MethodInvoker(delegate
                            {
                                Userid = cmbMsgFrom.SelectedItem.ToString();
                            }));
                        }

                        GroupStatus MemberScrape = new GroupStatus();
                        //string FromEmailId = MemberScrape.FromEmailCode(ref HttpHelper, SpeGroupId);
                        string FromEmailId = MemberScrape.FromEmailCodeComposeMsg(ref HttpHelper, Userid);
                        string FromEmailName = MemberScrape.FromName(ref HttpHelper);

                        Dictionary<string, string> SelectedContacts = new Dictionary<string, string>();


                        foreach (KeyValuePair<string, Dictionary<string, string>> contacts in MessageContacts)
                        {
                            if (contacts.Key == item.Key)
                            {
                                foreach (KeyValuePair<string, string> Details in contacts.Value)
                                {
                                    foreach (string itemChecked in chkMessageTo.CheckedItems)
                                    {

                                        if (itemChecked == Details.Value)
                                        {
                                            try
                                            {
                                                string id = Regex.Split(Details.Key, ":")[1];
                                                SelectedContacts.Add(id, Details.Value);
                                            }
                                            catch
                                            {

                                                SelectedContacts.Add(Details.Key, Details.Value);
                                            }
                                        }
                                        if (!(itemChecked == Details.Value))
                                        {
                                            try
                                            {
                                                string Value = Details.Value.Replace(",", string.Empty);
                                                if (itemChecked == Value)
                                                {
                                                    try
                                                    {
                                                        string id = Regex.Split(Details.Key, ":")[1];
                                                        SelectedContacts.Add(id, Details.Value);
                                                    }
                                                    catch
                                                    {
                                                    }
                                                }
                                            }
                                            catch
                                            { }
                                        }
                                    }
                                }
                            }
                        }

                        string msgBodyCompose = txtMsgBody.Text;
                        string msgSubCompose = txtMsgSubject.Text;
                        if (chkSpinTaxComposeMsg.Checked)
                        {
                            try
                            {
                                //msgBodyCompose = MessagelistCompose[RandomNumberGenerator.GenerateRandom(0, MessagelistCompose.Count - 1)];
                                msgBodyCompose = GlobusSpinHelper.spinLargeText(new Random(), txtMsgBody.Text);
                                msgBodycomposePass = txtMsgBody.Text;
                                msgSubCompose = subjectlistCompose[RandomNumberGenerator.GenerateRandom(0, subjectlistCompose.Count - 1)];
                            }
                            catch
                            {
                            }
                        }

                        // Calling PostFinalMsg(ref HttpHelper, SelectedContacts, txtMsgSubject.Text, msgBodyCompose.ToString(),UserEmail); from Compose Messge Module
                        int minDelay = 20;
                        int maxDelay = 25;

                        if (!string.IsNullOrEmpty(txtComposeMsgMinDelay.Text) && NumberHelper.ValidateNumber(txtComposeMsgMinDelay.Text))
                        {
                            minDelay = Convert.ToInt32(txtComposeMsgMinDelay.Text);
                        }
                        if (!string.IsNullOrEmpty(txtComposeMsgMaxDelay.Text) && NumberHelper.ValidateNumber(txtComposeMsgMaxDelay.Text))
                        {
                            maxDelay = Convert.ToInt32(txtComposeMsgMaxDelay.Text);
                        }

                        if (RdbMessagingWithTag_ComposeMessage.Checked)
                        {
                            // obj_ComposeMessage.PostFinalMsg_1By1(ref HttpHelper, SelectedContacts, txtMsgSubject.Text, msgBodyCompose.ToString(), UserEmail, FromemailId, FromEmailNam, minDelay, maxDelay);
                            obj_ComposeMessage.PostFinalMsg_1By1(ref HttpHelper, SelectedContacts, subjectlistCompose, msgBodycomposePass, msgSubCompose.ToString(), msgBodyCompose.ToString(), UserEmail, FromEmailId, FromEmailName, msg_spintaxt, minDelay, maxDelay, preventMsgSameUser, preventMsgGlobalUser);
                        }
                        else
                        {
                            //obj_ComposeMessage.PostFinalMsg(ref HttpHelper, SelectedContacts, txtMsgSubject.Text, msgBodyCompose.ToString(), UserEmail, FromemailId, FromEmailNam, minDelay, maxDelay);
                            obj_ComposeMessage.PostFinalMsg(ref HttpHelper, SelectedContacts, msgBodycomposePass, subjectlistCompose, msgSubCompose.ToString(), msgBodyCompose.ToString(), UserEmail, FromEmailId, FromEmailName, msg_spintaxt, minDelay, maxDelay, preventMsgSameUser, preventMsgGlobalUser);


                            int counter = ComposeMessage.ComposeMessage.SlectedContacts1.Count();

                            if (counter > 0)
                            {
                                do
                                {
                                    // obj_ComposeMessage.PostFinalMsg(ref HttpHelper, ComposeMessage.ComposeMessage.SlectedContacts1, txtMsgSubject.Text, msgBodyCompose.ToString(), UserEmail, FromemailId, FromEmailNam, minDelay, maxDelay);
                                    obj_ComposeMessage.PostFinalMsg(ref HttpHelper, ComposeMessage.ComposeMessage.SlectedContacts1, msgBodycomposePass, subjectlistCompose, msgSubCompose.ToString(), msgBodyCompose.ToString(), UserEmail, FromEmailId, FromEmailNam, msg_spintaxt, minDelay, maxDelay, preventMsgSameUser, preventMsgGlobalUser);
                                    counter = ComposeMessage.ComposeMessage.SlectedContacts1.Count();

                                } while (counter > 0);
                            }
                        }

                        obj_ComposeMessage.logger.addToLogger -= new EventHandler(ComposeMessage_addToLogger);
                        //PostFinalMsg(ref HttpHelper, SelectedContacts, txtMsgSubject.Text, msgBodyCompose.ToString(),UserEmail);
                    }
                    else if (!Login.IsLoggedIn)
                    {
                        AddLoggerComposeMessage("[ " + DateTime.Now + " ] => [ Couldn't Login With Email : " + item.Value._Username + "]");
                    }
                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Compose Message --> PostMessageBulk() -->  PostFinalMsg >>>>" + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Compose Message --> PostMessageBulk() -->  PostFinalMsg >>>>" + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinComposeMessageErrorLogs);
                }

                #region old code

                //AddLoggerComposeMessage("Compose Message From: " + Userid);
                //string Mposted = MessagePosted.Split(':')[0];
                //if (Mposted.Contains("Your message was successfully sent."))
                //{
                //    AddLoggerComposeMessage("Message Posted To : All Selected Accounts");
                //}
                //else if (MessagePosted.Contains("Error"))
                //{
                //    AddLoggerComposeMessage("Error in Post");
                //}
                //else
                //{
                //    AddLoggerComposeMessage("Message Not Posted");
                //} 

                #endregion

            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Compose Message --> PostMessageBulk() --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Compose Message --> PostMessageBulk() --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinComposeMessageErrorLogs);
            }
            finally
            {
                if (chkSelectAll.Checked)
                {
                    NoOfAccountsLoggedin--;
                    if (NoOfAccountsLoggedin == 0)
                    {
                        if (btnSendMsg.InvokeRequired)
                        {
                            btnSendMsg.Invoke(new MethodInvoker(delegate
                            {
                                AddLoggerComposeMessage("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                                AddLoggerComposeMessage("-------------------------------------------------------------------------------------------------------------------------------");
                                btnSendMsg.Cursor = Cursors.Default;
                            }));
                        }
                    }
                }
                else
                {
                    if (btnSendMsg.InvokeRequired)
                    {
                        btnSendMsg.Invoke(new MethodInvoker(delegate
                        {
                            AddLoggerComposeMessage("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                            AddLoggerComposeMessage("-----------------------------------------------------------------------------------------------------------------------------------");
                            btnSendMsg.Cursor = Cursors.Default;
                        }));
                    }
                }
            }
        }

        #endregion


        //********************************LinkedinPreScraper************************************************

        #region btnCheckPremAccount_Click
        private void btnCheckPremAccount_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {

                try
                {
                    AddLoggerScrapeUsers("[ " + DateTime.Now + " ] => [ Process Running Please wait for sometimes... ]");

                    if (SearchCriteria.SignIN)
                    {
                        LinkedinLogin Login = new LinkedinLogin();
                        //For Sign Out 
                        Login.LogoutHttpHelper();
                        SearchCriteria.SignOut = true;
                    }

                    if (SearchCriteria.SignOut)
                    {
                        SearchCriteria.LoginID = string.Empty;
                        if (LinkedInManager.linkedInDictionary.Count() > 0)
                        {
                            try
                            {
                                object temp = null;
                                comboBoxemail.Invoke(new MethodInvoker(delegate
                                {
                                    temp = comboBoxemail.SelectedItem;
                                }));

                                if (temp != null)
                                {

                                    string acc = "";
                                    comboBoxemail.Invoke(new MethodInvoker(delegate
                                    {
                                        acc = comboBoxemail.SelectedItem.ToString();
                                        SearchCriteria.LoginID = comboBoxemail.SelectedItem.ToString();
                                    }));

                                    //Run a separate thread for each account
                                    LinkedInMaster item = new LinkedInMaster();
                                    LinkedInManager.linkedInDictionary.TryGetValue(acc, out item);

                                    item.logger.addToLogger += ScrapeEvent_addToLogger;
                                    item.LoginHttpHelper(ref HttpHelper);

                                    if (SearchCriteria.loginREsponce.Contains("Your LinkedIn account has been temporarily restricted"))
                                    {
                                        AddLoggerScrapeUsers("[ " + DateTime.Now + " ] => [ " + SearchCriteria.LoginID + "Your LinkedIn account has been temporarily restricted ]");
                                    }

                                    if (SearchCriteria.loginREsponce.Contains("Please confirm your email address"))
                                    {
                                        AddLoggerScrapeUsers("[ " + DateTime.Now + " ] => [ " + SearchCriteria.LoginID + " account has been temporarily restricted Please confirm your email address ]");
                                    }

                                    if (!item.IsLoggedIn)
                                    {
                                        AddLoggerScrapeUsers("[ " + DateTime.Now + " ] => [ Couldn't Login With : " + acc + " ]");
                                        return;
                                    }

                                    SearchCriteria.SignIN = true;
                                    SearchCriteria.SignOut = false;
                                    try
                                    {
                                        this.Invoke(new MethodInvoker(delegate
                                        {
                                            LoadCmboData();
                                        }));
                                    }
                                    catch
                                    {
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Check_Premium_Account() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Check_Premium_Account() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPreScrapperErrorLogs);
                            }
                        }
                    }
                }
                catch { }
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddLoggerScrapeUsers("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }

        }
        #endregion

        #region LoadPreScrapper
        private void LoadPreScrapper()
        {
            try
            {
                if (LinkedInManager.linkedInDictionary.Count() == 0)
                {
                    MessageBox.Show("Please Load LinkedIn Accounts From MyAccounts Menu");
                    AddLoggerStatusUpdate("[ " + DateTime.Now + " ] => [ Please Load LinkedIn Accounts From MyAccounts Menu ]");

                    lbGeneralLogs.Items.Clear();
                    frmAccounts FrmAccount = new frmAccounts();
                    FrmAccount.Show();
                    return;
                }
                else
                {
                    try
                    {
                        //Thread obj_PopulateCmo = new Thread(PopulateCmo);
                        //obj_PopulateCmo.Start();
                        PopulateCmo();
                    }
                    catch
                    {
                    }
                }
            }
            catch { }
        }
        #endregion


        #region btnSearchScraper_Click

        private void btnSearchScraper_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {
                try
                {
                    try
                    {
                        SearchCriteria.starter = true;
                        if (comboBoxemail.Items.Count <= 0)
                        {
                            AddLoggerScrapeUsers("[ " + DateTime.Now + " ] => [ Load The account From Menu and switch the tab and again come to InBoardProGetData Tab ]");
                            lbGeneralLogs.Items.Clear();
                            frmAccounts FrmAccount = new frmAccounts();
                            FrmAccount.Show();
                            return;
                        }

                        if (comboBoxemail.SelectedIndex < 0)
                        {
                            AddLoggerScrapeUsers("[ " + DateTime.Now + " ] => [ Select The Email Id From The Dropdown and Fill All Mandatory (*) Fields  ]");
                        }

                        if (string.IsNullOrEmpty(txtScraperExportFilename.Text))
                        {
                            MessageBox.Show("Please Add any Name to Export File Name");
                            return;
                        }

                        AddLoggerScrapeUsers("[ " + DateTime.Now + " ] => [ Starting Scraping ]");
                        IsStop_InBoardProGetDataThread = false;
                        lstInBoardProGetDataThraed.Clear();

                        if (combScraperLocation.SelectedIndex == 0)
                        {
                            SearchCriteria.Location = "I";
                            lblScraperCountry.Visible = true;
                            CombScraperCountry.Visible = true;
                        }
                        else if (combScraperLocation.SelectedIndex == 1)
                        {
                            SearchCriteria.Location = "Y";
                            lblScraperCountry.Visible = false;
                            CombScraperCountry.Visible = false;
                        }

                        foreach (KeyValuePair<string, string> item in CountryCode)
                        {
                            try
                            {
                                if (item.Value == CombScraperCountry.SelectedItem.ToString())
                                {
                                    SearchCriteria.Country = item.Key;
                                    break;
                                }
                            }
                            catch (Exception ex)
                            {
                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> getting CountryCode ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error -->  getting CountryCode ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPreScrapperErrorLogs);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnSearchScraper_Click() ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnSearchScraper_Click() ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPreScrapperErrorLogs);
                    }


                    #region AreaWiseLocation

                    foreach (var item in listAreaLocation)
                    {
                        string getAreWiseCode = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/ta/region?query=" + item.ToString()));

                        int startindex = getAreWiseCode.IndexOf("\"id\":");
                        int startindex1 = getAreWiseCode.IndexOf("displayName");
                        //{"resultList":[{"id":"in:6498","headLine":"<strong>Bhilai<\/strong> Area, India","displayName":"Bhilai Area, India","subLine":""}]}

                        if (startindex > 0)
                        {
                            try
                            {
                                string start = getAreWiseCode.Substring(startindex).Replace("\"id\":", string.Empty);
                                int endindex = start.IndexOf(",");
                                string end = start.Substring(0, endindex).Replace(":", "%3A").Replace("\"", "");
                                //SearchCriteria.LocationAreaCode = end.Replace(":", "%").Replace("\"", "");

                                string start1 = getAreWiseCode.Substring(startindex1).Replace("displayName", string.Empty);
                                int endindex1 = start1.IndexOf("\",\"");
                                string end1 = start1.Substring(0, endindex1).Replace(":", "").Replace("\"", "");


                                if (string.IsNullOrEmpty(SearchCriteria.LocationArea))
                                {
                                    SearchCriteria.LocationArea = end;
                                }
                                else
                                {
                                    SearchCriteria.LocationArea = SearchCriteria.LocationArea + "," + end;
                                }

                                string contrycd = end.Split('%')[0].Replace("3A", string.Empty);
                                if (contrycd != SearchCriteria.Country)
                                {
                                    AddLoggerScrapeUsers("[ " + DateTime.Now + " ] => [ Wrong Selection of Country or Your Area Location is not matched the country ]");
                                    return;
                                }
                                else
                                {
                                    AddLoggerScrapeUsers("[ " + DateTime.Now + " ] => [ Added Area Location: " + end1.ToString() + " ]");

                                }


                            }
                            catch { }

                        }
                    }


                    #endregion

                    #region Industry List
                    string Industryvalue = string.Empty;
                    string[] arrIndustry = Regex.Split(IndustryList, ",");

                    foreach (string Industry in checkedListBoxIndustry.CheckedItems)
                    {
                        foreach (string itemIndustry in arrIndustry)
                        {
                            try
                            {
                                if (itemIndustry.Contains(Industry))
                                {
                                    string[] arryIndustry = Regex.Split(itemIndustry, ";");
                                    if (arryIndustry.Length == 2)
                                    {
                                        if (!string.IsNullOrEmpty(arryIndustry[1]))
                                        {
                                            if (string.IsNullOrEmpty(Industryvalue))
                                            {
                                                Industryvalue = arryIndustry[1];
                                            }
                                            else
                                            {
                                                Industryvalue = Industryvalue + "," + arryIndustry[1];
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> arrIndustry 1 ---2--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> arrIndustry 1 ---2--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPreScrapperErrorLogs);
                            }
                        }
                    }
                    //if (string.IsNullOrEmpty(Industryvalue))
                    //{
                    //    Industryvalue = "select-all";
                    //}
                    SearchCriteria.IndustryType = Industryvalue;
                    #endregion

                    #region Company size List
                    string companysizevalue = string.Empty;
                    string[] arraycompnaysize = Regex.Split(companysizelist, ",");
                    foreach (string companysize in checkedListBoxCompanySize.CheckedItems)
                    {
                        foreach (string item in arraycompnaysize)
                        {
                            try
                            {
                                if (item.Split(':')[1] == companysize)
                                {
                                    string[] ArrayCompnay = Regex.Split(item, ":");
                                    if (!string.IsNullOrEmpty(ArrayCompnay[0]))
                                    {
                                        if (string.IsNullOrEmpty(companysizevalue))
                                        {
                                            companysizevalue = ArrayCompnay[0];
                                        }
                                        else
                                        {
                                            companysizevalue = companysizevalue + "," + ArrayCompnay[0];
                                        }
                                    }
                                    //break;
                                }
                            }
                            catch (Exception ex)
                            {
                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnSearchScraper_Click() ---3--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnSearchScraper_Click() ---3--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPreScrapperErrorLogs);
                            }
                        }
                        //break;
                    }

                    if (companysizevalue == "all")
                    {
                        SearchCriteria.CompanySize = string.Empty;
                    }
                    else
                    {
                        SearchCriteria.CompanySize = companysizevalue;
                    }
                    #endregion

                    #region Group List
                    string Groupsvalue = string.Empty;
                    foreach (string Groups in checkedListGroups.CheckedItems)
                    {
                        //foreach (var item1 in GroupStatus.GroupMemUrl)
                        //{
                        //    string[] grp = item1.Split(':');
                        //    if (SelUser[1] == grp[0])
                        //    {
                        //        SpeGroupId = grp[2];
                        //        break;
                        //    }
                        //}


                        foreach (var item in GroupStatus.GroupMemUrl)
                        {
                            try
                            {
                                string[] grp = item.Split(':');
                                if (grp[0].Equals(Groups))
                                {
                                    if (!string.IsNullOrEmpty(grp[2]))
                                    {
                                        if (string.IsNullOrEmpty(Groupsvalue))
                                        {
                                            Groupsvalue = grp[2];
                                        }
                                        else
                                        {
                                            Groupsvalue = Groupsvalue + "," + grp[2];
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnSearchScraper_Click() ---4--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnSearchScraper_Click() ---4--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPreScrapperErrorLogs);
                            }
                        }
                    }
                    SearchCriteria.Group = Groupsvalue;
                    #endregion

                    #region Senority List


                    if (SearchCriteria.AccountType == "RecuiterType")
                    {
                        senioritylevel = senioritylevelRecruiterType;
                    }
                    string Seniorlevelvalue = string.Empty;
                    string[] senoirLevelList = Regex.Split(senioritylevel, ",");
                    foreach (string Seniorlevel in checkedListBoxSeniorlevel.CheckedItems)
                    {
                        foreach (string itemSeniorLevel in senoirLevelList)
                        {
                            try
                            {
                                if (itemSeniorLevel.Contains(Seniorlevel))
                                {
                                    string[] arrysenoirLevel = Regex.Split(itemSeniorLevel, ":");
                                    if (arrysenoirLevel.Length == 2)
                                    {
                                        if (!string.IsNullOrEmpty(arrysenoirLevel[0]))
                                        {
                                            if (string.IsNullOrEmpty(Seniorlevelvalue))
                                            {
                                                Seniorlevelvalue = arrysenoirLevel[0];
                                            }
                                            else
                                            {
                                                Seniorlevelvalue = Seniorlevelvalue + "," + arrysenoirLevel[0];
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> arrIndustry 1 ---2--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> arrIndustry 1 ---2--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPreScrapperErrorLogs);
                            }
                        }
                    }

                    if (Seniorlevelvalue == "all")
                    {
                        SearchCriteria.SeniorLevel = string.Empty;
                    }
                    else
                    {
                        SearchCriteria.SeniorLevel = Seniorlevelvalue;
                    }
                    #endregion

                    #region Language List
                    string language = string.Empty;
                    string[] arrLang = Regex.Split(Language, ",");
                    foreach (string LanguageL in checkedListBoxLanguage.CheckedItems)
                    {
                        foreach (string item in arrLang)
                        {
                            try
                            {
                                if (item.Contains(LanguageL))
                                {
                                    string[] arryLang = Regex.Split(item, ";");
                                    if (arryLang.Length == 2)
                                    {
                                        if (!string.IsNullOrEmpty(arryLang[1]))
                                        {
                                            if (string.IsNullOrEmpty(language))
                                            {
                                                language = arryLang[1];
                                            }
                                            else
                                            {
                                                language = language + "," + arryLang[1];
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnSearchScraper_Click() ---6--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnSearchScraper_Click() ---6--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPreScrapperErrorLogs);
                            }
                        }
                    }
                    if (string.IsNullOrEmpty(language))
                    {
                        //language = "select-all";
                        language = "";
                    }
                    SearchCriteria.language = language;
                    #endregion

                    #region Relationship List
                    string Relationship = string.Empty;
                    string[] arrRelation = Regex.Split(RelationshipList, ",");
                    foreach (string RelationL in checkedListRelationship.CheckedItems)
                    {
                        foreach (string item in arrRelation)
                        {
                            try
                            {
                                if (item.Contains(RelationL))
                                {
                                    string[] arryRelat = Regex.Split(item, ":");
                                    if (arryRelat.Length == 2)
                                    {
                                        if (!string.IsNullOrEmpty(arryRelat[0]))
                                        {
                                            if (string.IsNullOrEmpty(Relationship))
                                            {
                                                Relationship = arryRelat[0];
                                            }
                                            else
                                            {
                                                Relationship = arryRelat[0] + "," + Relationship;
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnSearchScraper_Click() ---7--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnSearchScraper_Click() ---7--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPreScrapperErrorLogs);
                            }
                        }
                    }

                    if (Relationship == "N")
                    {
                        SearchCriteria.Relationship = string.Empty;
                    }
                    else
                    {
                        SearchCriteria.Relationship = Relationship;
                    }
                    #endregion

                    #region Function List
                    string Function = string.Empty;
                    if (SearchCriteria.AccountType == "RecuiterType")
                    {
                        Functionlist = Functionlistrecruiter;
                    }
                    
                    string[] FunctionList = Regex.Split(Functionlist, ",");
                    foreach (string FunctionL in checkedListFunction.CheckedItems)
                    {
                        foreach (string itemFunction in FunctionList)
                        {
                            try
                            {
                                if (itemFunction.Contains(FunctionL))
                                {
                                    string[] functionItem = Regex.Split(itemFunction, ":");
                                    if (!string.IsNullOrEmpty(functionItem[0]))
                                    {
                                        if (string.IsNullOrEmpty(Function))
                                        {
                                            Function = functionItem[0];
                                        }
                                        else
                                        {
                                            Function = Function + "," + functionItem[0];
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnSearchScraper_Click() ---8--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnSearchScraper_Click() ---8--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPreScrapperErrorLogs);
                            }
                        }
                    }
                    if (Function == "all")
                    {
                        SearchCriteria.Function = string.Empty;
                    }
                    else
                    {
                        SearchCriteria.Function = Function;
                    }
                    #endregion

                    #region IntrestedIn
                    string InterestedIn = string.Empty;
                    string[] IntrestesList = Regex.Split(IntrestedinList, ",");
                    foreach (string InterestedL in checkedListBoxInterestedIN.CheckedItems)
                    {
                        foreach (string Intresteditem in IntrestesList)
                        {
                            try
                            {
                                if (Intresteditem.Contains(InterestedL))
                                {
                                    string[] arrayIntrst = Regex.Split(Intresteditem, ":");
                                    if (!string.IsNullOrEmpty(Intresteditem))
                                    {
                                        if (string.IsNullOrEmpty(InterestedIn))
                                        {
                                            InterestedIn = arrayIntrst[0];
                                        }
                                        else
                                        {
                                            InterestedIn = InterestedIn + "," + arrayIntrst[0];
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnSearchScraper_Click() ---9--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnSearchScraper_Click() ---9--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPreScrapperErrorLogs);
                            }
                        }
                    }
                    if (InterestedIn == "select-all")
                    {
                        SearchCriteria.InerestedIn = string.Empty;
                    }
                    else
                    {
                        SearchCriteria.InerestedIn = InterestedIn;
                    }

                    #endregion

                    #region Explerience list
                    string TotalExperience = string.Empty;
                    string[] arratExpericne = Regex.Split(expList, ",");
                    foreach (string YearOfExperienceL in GetDataForPrimiumAccount.CheckedItems)
                    {
                        foreach (string itemExp in arratExpericne)
                        {
                            try
                            {
                                if (itemExp.Contains(YearOfExperienceL))
                                {
                                    string[] arrayitem = Regex.Split(itemExp, ":");
                                    if (!string.IsNullOrEmpty(arrayitem[1]))
                                    {
                                        if (string.IsNullOrEmpty(TotalExperience))
                                        {
                                            TotalExperience = arrayitem[0];
                                        }
                                        else
                                        {
                                            TotalExperience = TotalExperience + "," + arrayitem[0];
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnSearchScraper_Click() ---10--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnSearchScraper_Click() ---10--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPreScrapperErrorLogs);
                            }
                        }
                    }

                    if (TotalExperience == "select-all")
                    {
                        SearchCriteria.YearOfExperience = string.Empty;
                    }
                    else
                    {
                        SearchCriteria.YearOfExperience = TotalExperience;
                    }
                    #endregion

                    #region Recently Joined
                    string RecentlyJoined = string.Empty;
                    string[] RecetlyJoinedArr = Regex.Split(RecentlyjoinedList, ",");
                    foreach (string RecentlyJoinedL in checkedListBoxRecentlyJoined.CheckedItems)
                    {
                        foreach (string item in RecetlyJoinedArr)
                        {
                            try
                            {
                                string[] arrayitem = Regex.Split(item, ":");
                                if (item.Contains(RecentlyJoinedL))
                                {
                                    if (!string.IsNullOrEmpty(arrayitem[0]))
                                    {
                                        if (string.IsNullOrEmpty(RecentlyJoined))
                                        {
                                            RecentlyJoined = arrayitem[0];
                                        }
                                        else
                                        {
                                            RecentlyJoined = RecentlyJoined + "," + arrayitem[0];
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnSearchScraper_Click() ---11--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnSearchScraper_Click() ---11--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPreScrapperErrorLogs);
                            }
                        }
                    }
                    if (RecentlyJoined == "select-all")
                    {
                        SearchCriteria.RecentlyJoined = string.Empty;
                    }
                    else
                    {
                        SearchCriteria.RecentlyJoined = RecentlyJoined.Trim();
                    }
                    #endregion

                    #region Fortune List
                    string Fortune1000 = string.Empty;
                    string[] FortuneArr = Regex.Split(fortuneList, ",");
                    foreach (string Fortune1000L in checkedListBoxFortune1000.CheckedItems)
                    {
                        foreach (string item in FortuneArr)
                        {
                            try
                            {
                                string[] arrayItem = Regex.Split(item, ":");
                                if (item.Contains(Fortune1000L))
                                {
                                    if (!string.IsNullOrEmpty(arrayItem[0]))
                                    {
                                        if (string.IsNullOrEmpty(Fortune1000))
                                        {
                                            Fortune1000 = arrayItem[0];
                                        }
                                        else
                                        {
                                            Fortune1000 = Fortune1000 + "," + arrayItem[0];
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnSearchScraper_Click() ---12--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnSearchScraper_Click() ---12--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPreScrapperErrorLogs);
                            }
                        }
                    }
                    if (RecentlyJoined == "select-all")
                    {
                        SearchCriteria.Fortune1000 = string.Empty;
                    }
                    else
                    {
                        SearchCriteria.Fortune1000 = Fortune1000;
                    }
                    #endregion

                    #region Within the PostalCode
                    try
                    {
                        string[] arraywithinList = Regex.Split(WithingList, ",");
                        foreach (string item in arraywithinList)
                        {
                            if (item.Contains(cmbboxWithin.SelectedItem.ToString()))
                            {
                                string[] arrayWithin = Regex.Split(item, ":");
                                SearchCriteria.within = arrayWithin[0];
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    #endregion

                    #region within Title value
                    try
                    {
                        string[] arrayTitleList = Regex.Split(TitleValue, ",");
                        foreach (string item in arrayTitleList)
                        {
                            string[] arrayTitleValue = Regex.Split(item, ":");
                            if (arrayTitleValue[1] == cmbboxTitle.SelectedItem.ToString())
                            {
                                SearchCriteria.TitleValue = arrayTitleValue[0];
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    #endregion

                    #region within Companyvalue
                    try
                    {
                        if (cmbboxCompanyValue.Enabled == true)
                        {
                            string[] arrayTitleList = Regex.Split(TitleValue, ",");
                            foreach (string item in arrayTitleList)
                            {
                                string[] arrayTitleValue = Regex.Split(item, ":");

                                if (arrayTitleValue[1] == cmbboxCompanyValue.SelectedItem.ToString())
                                {
                                    SearchCriteria.CompanyValue = arrayTitleValue[0];
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    #endregion

                    SearchCriteria.FileName = (txtScraperExportFilename.Text).Replace("\"", string.Empty).Replace(",", string.Empty).ToString();
                    SearchCriteria.FirstName = txtScrperFirstname.Text;
                    SearchCriteria.LastName = txtScrperlastname.Text;
                    SearchCriteria.Keyword = txtKeyword.Text;
                    SearchCriteria.Title = txtTitle.Text;
                    if (SearchCriteria.Keyword.Contains("or") || (SearchCriteria.Keyword.Contains("and")) || (SearchCriteria.Keyword.Contains("not")))
                    {
                        try
                        {
                            SearchCriteria.Keyword = SearchCriteria.Keyword.Replace(" or ", " OR ");
                            SearchCriteria.Keyword = SearchCriteria.Keyword.Replace(" and ", " AND ");
                            SearchCriteria.Keyword = SearchCriteria.Keyword.Replace(" not ", " NOT ");
                        }
                        catch { }
                    }
                    if (SearchCriteria.Title.Contains("or") || (SearchCriteria.Title.Contains("and")) || (SearchCriteria.Title.Contains("not")))
                    {
                        try
                        {
                            SearchCriteria.Title = SearchCriteria.Title.Replace(" or ", " OR ");
                            SearchCriteria.Title = SearchCriteria.Title.Replace(" and ", " AND ");
                            SearchCriteria.Title = SearchCriteria.Title.Replace(" not ", " NOT ");
                        }
                        catch { }
                    }
                    if (!string.IsNullOrEmpty(txtPostalcode.Text))
                    {
                        SearchCriteria.PostalCode = txtPostalcode.Text;
                    }
                    if (!string.IsNullOrEmpty(txtCompnayName.Text))
                    {
                        SearchCriteria.Company = txtCompnayName.Text;
                    }

                    if (CombScraperCountry.SelectedItem.ToString() != null && CombScraperCountry.SelectedIndex > 0)
                    {
                        try
                        {
                            if (!string.IsNullOrEmpty(CombScraperCountry.SelectedItem.ToString()))
                            {
                                string countryValue = string.Empty;
                                string temCOuntry = CombScraperCountry.SelectedItem.ToString();
                                GetCountryNameValue(ref temCOuntry, ref countryValue);
                                SearchCriteria.Country = countryValue;
                            }
                        }
                        catch (Exception ex)
                        {
                            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnSearchScraper_Click() ---13--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnSearchScraper_Click() ---13--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPreScrapperErrorLogs);
                        }
                    }

                    if (string.IsNullOrEmpty(txtScraperMinimumDelay.Text) || string.IsNullOrEmpty(txtScraperMaximumDelay.Text))
                    {
                        MessageBox.Show("Please enter proper delay time.");
                        return;
                    }
                    else
                    {
                        SearchCriteria.scraperMinDelay = Convert.ToInt32(txtScraperMinimumDelay.Text.Trim());
                        SearchCriteria.scraperMaxDelay = Convert.ToInt32(txtScraperMaximumDelay.Text.Trim());
                    }


                    btnSearchScraper.Cursor = Cursors.AppStarting;

                    new Thread(() =>
                    {
                        StartInBoardProGetData();

                    }).Start();

                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnSearchScraper_Click() ---13--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnSearchScraper_Click() ---13--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPreScrapperErrorLogs);
                }
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddLoggerScrapeUsers("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        }

        #endregion

        #region GetCountryNameValue

        public void GetCountryNameValue(ref string countryName, ref string countryValue)
        {
            try
            {
                ClsSelect CountryList = new ClsSelect();
                string[] arrayCountry = Regex.Split(CountryList.country, ",");

                foreach (string CountryValue in arrayCountry)
                {
                    if (CountryValue.Contains(countryName))
                    {
                        if (countryName == "India")
                        {
                            string[] value = Regex.Split(CountryValue, ":");
                            countryName = value[1].ToString();
                            countryValue = value[0].ToString();
                        }
                        else
                        {
                            string[] value = Regex.Split(CountryValue, ":");
                            countryName = value[1].ToString();
                            countryValue = value[0].ToString();
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> GetCountryNameValue() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> GetCountryNameValue() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPreScrapperErrorLogs);
            }
        }

        #endregion

        #region StartInBoardProGetData()

        public void StartInBoardProGetData()
        {
            try
            {
                if (IsStop_InBoardProGetDataThread)
                {
                    return;
                }

                lstInBoardProGetDataThraed.Add(Thread.CurrentThread);
                lstInBoardProGetDataThraed = lstInBoardProGetDataThraed.Distinct().ToList();
                Thread.CurrentThread.IsBackground = true;

                HttpHelper = new GlobusHttpHelper();
                LinkedInScrape objlinkscr = new LinkedInScrape();
                bool isLoggedIn = Login_InBoardProGetData();

                if (isLoggedIn)
                {
                    AddLoggerScrapeUsers("[ " + DateTime.Now + " ] => [ Start LinkedIn Crawling ]");
                    objlinkscr.StartInBoardProGetDataWithPagination(ref HttpHelper);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error >>> " + ex.StackTrace);
            }
            finally
            {
                try
                {
                    DisableEnableScrapperControls();

                    this.Invoke(new MethodInvoker(delegate
                    {
                        btnStopScraper.Enabled = true;
                        btnSearchScraper.Enabled = true;
                        btnSearchScraper.Cursor = Cursors.Default;
                    }));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }
            }
        }

        void CampaignnameLog(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eArgs = e as EventsArgs;
                StartInBoardProGetData(eArgs.log);
            }
        }

        void loggerGroupStatusEvent(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eArgs = e as EventsArgs;
                AddLoggerComposeMessage(eArgs.log);
            }
        }


        public void StartInBoardProGetData(string args)
        {


            try
            {
                if (IsStop_InBoardProGetDataThread)
                {
                    return;
                }

                string[] ParameterData_Split = Regex.Split(args, ":");
                string Account = ParameterData_Split[0];
                string password = ParameterData_Split[1];
                string ProxyAddress = ParameterData_Split[2];
                string ProxyPort = ParameterData_Split[3];
                string ProxyUserName = ParameterData_Split[4];
                string ProxyPassword = ParameterData_Split[5];
                string FirstName = ParameterData_Split[7];
                string LastName = ParameterData_Split[8];
                string Location = ParameterData_Split[9];
                string Country = ParameterData_Split[10];
                string LocationArea = ParameterData_Split[11];
                string PostalCode = ParameterData_Split[12];
                string Company = ParameterData_Split[13];
                string Keyword = ParameterData_Split[14];
                string Title = ParameterData_Split[15];
                string IndustryType = ParameterData_Split[16];
                string Relationship = ParameterData_Split[17];
                string language = ParameterData_Split[18];
                string Groups = ParameterData_Split[19];
                string FileName = ParameterData_Split[20];
                string TitleValue = ParameterData_Split[21];
                string CompanyValue = ParameterData_Split[22];
                string within = ParameterData_Split[23];
                string YearsOfExperience = ParameterData_Split[24];
                string Function = ParameterData_Split[25];
                string SeniorLevel = ParameterData_Split[26];
                string IntrestedIn = ParameterData_Split[27];
                string CompanySize = ParameterData_Split[28];
                string Fortune1000 = ParameterData_Split[29];
                string RecentlyJoined = ParameterData_Split[30];




                lstInBoardProGetDataThraed.Add(Thread.CurrentThread);
                lstInBoardProGetDataThraed = lstInBoardProGetDataThraed.Distinct().ToList();
                Thread.CurrentThread.IsBackground = true;

                GlobusHttpHelper HttpHelper = new GlobusHttpHelper();
                LinkedInScrape objlinkscr = new LinkedInScrape();
                bool isLoggedIn = Login_InBoardProGetDataForCampaignScraper(ref HttpHelper, Account, password, ProxyAddress, ProxyPort, ProxyUserName, ProxyPassword);

                if (isLoggedIn)
                {
                    AddLoggerScrapeUsers("[ " + DateTime.Now + " ] => [ Start LinkedIn Crawling ]");
                    objlinkscr.StartCampaignInBoardProGetDataWithPagination(ref HttpHelper, Account, FirstName, LastName, Location, Country, LocationArea, PostalCode, Company, Keyword, Title, IndustryType, Relationship, language, Groups, FileName, TitleValue, CompanyValue, within, YearsOfExperience, Function, SeniorLevel, IntrestedIn, CompanySize, Fortune1000, RecentlyJoined);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error >>> " + ex.StackTrace);
            }
            finally
            {
                try
                {
                    DisableEnableScrapperControls();

                    this.Invoke(new MethodInvoker(delegate
                    {
                        btnStopScraper.Enabled = true;
                        btnSearchScraper.Enabled = true;
                        btnSearchScraper.Cursor = Cursors.Default;
                    }));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }
            }
        }


        #endregion



        #region comboBoxemail_SelectedIndexChanged

        private void comboBoxemail_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisableEnableScrapperControls();

            //Thread thread_Method_comboBoxemail_SelectedIndexChanged = new Thread(Method_comboBoxemail_SelectedIndexChanged);
            //thread_Method_comboBoxemail_SelectedIndexChanged.Start();

            // Adding Groups Related User

            if (chkGroupsAdd.Checked == true)
            {
                cmbMemberGroup.Items.Clear();
                GroupStatus.GroupMemUrl.Clear();

                try
                {
                    string GetUserID = comboBoxemail.SelectedItem.ToString();
                    label47.Text = comboBoxemail.SelectedItem.ToString();

                    foreach (KeyValuePair<string, Dictionary<string, string>> item in GrpMemMess)
                    {
                        if (GetUserID.Contains(item.Key))
                        {
                            List<string> GmUserIDs = new List<string>();
                            foreach (KeyValuePair<string, string> item1 in item.Value)
                            {
                                string group = item1.Key;
                                string[] group1 = group.Split(':');

                                if (GetUserID == group1[1].ToString())
                                {
                                    checkedListGroups.Items.Add(group1[0].ToString());
                                    GroupStatus.GroupMemUrl.Add(item1.Key + ":" + item1.Value);
                                }
                            }

                        }
                    }

                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Added in Scrapper --> comboBoxemail_SelectedIndexChanged() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Added in Scrapper --> comboBoxemail_SelectedIndexChanged() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPreScrapperErrorLogs);
                }

                GroupStatus.ManageTabGroupStatus = true;
            }
        }

        private void Method_comboBoxemail_SelectedIndexChanged()
        {
            try
            {
                //this.Invoke(new MethodInvoker(delegate
                //{
                try
                {
                    AddLoggerScrapeUsers("[ " + DateTime.Now + " ] => [ Process Running Please wait for sometimes... ]");

                    if (SearchCriteria.SignIN)
                    {
                        LinkedinLogin Login = new LinkedinLogin();
                        //For Sign Out 
                        Login.LogoutHttpHelper();

                        SearchCriteria.SignOut = true;
                    }

                    if (SearchCriteria.SignOut)
                    {
                        SearchCriteria.LoginID = string.Empty;
                        if (LinkedInManager.linkedInDictionary.Count() > 0)
                        {
                            try
                            {
                                object temp = null;
                                comboBoxemail.Invoke(new MethodInvoker(delegate
                                {
                                    temp = comboBoxemail.SelectedItem;
                                }));

                                if (temp != null)
                                {
                                    //GlobusHttpHelper httpHelper = new GlobusHttpHelper();
                                    string acc = "";
                                    comboBoxemail.Invoke(new MethodInvoker(delegate
                                    {
                                        acc = comboBoxemail.SelectedItem.ToString();
                                        SearchCriteria.LoginID = comboBoxemail.SelectedItem.ToString();//change 21/08/12
                                    }));
                                    //string acc = account.Remove(account.IndexOf(':'));

                                    //Run a separate thread for each account
                                    LinkedInMaster item = new LinkedInMaster();
                                    LinkedInManager.linkedInDictionary.TryGetValue(acc, out item);

                                    item.logger.addToLogger += ScrapeEvent_addToLogger;
                                    item.LoginHttpHelper(ref HttpHelper);

                                    if (item.IsLoggedIn)
                                    {
                                        AddLoggerScrapeUsers("[ " + DateTime.Now + " ] => [ Logged in With : " + acc + " ]");

                                    }

                                    if (SearchCriteria.loginREsponce.Contains("Your LinkedIn account has been temporarily restricted"))
                                    {
                                        AddLoggerScrapeUsers("[ " + DateTime.Now + " ] => [ " + SearchCriteria.LoginID + "Your LinkedIn account has been temporarily restricted ]");
                                    }

                                    if (SearchCriteria.loginREsponce.Contains("Please confirm your email address"))
                                    {
                                        AddLoggerScrapeUsers("[ " + DateTime.Now + " ] => [ " + SearchCriteria.LoginID + " account has been temporarily restricted Please confirm your email address ]");
                                    }

                                    if (!item.IsLoggedIn)
                                    {
                                        AddLoggerScrapeUsers("[ " + DateTime.Now + " ] => [ Couldn't Login With : " + acc + " ]");
                                        return;
                                    }

                                    SearchCriteria.SignIN = true;
                                    SearchCriteria.SignOut = false;
                                    try
                                    {
                                        this.Invoke(new MethodInvoker(delegate
                                        {
                                            LoadCmboData();
                                        }));
                                    }
                                    catch
                                    {
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> comboBoxemail_SelectedIndexChanged() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> comboBoxemail_SelectedIndexChanged() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPreScrapperErrorLogs);
                            }
                        }
                    }
                }
                catch
                {
                }
                //}));
                //DisableEnableScrapperControls();
            }
            catch
            {
            }
        }

        private bool Login_InBoardProGetData()
        {
            bool isLoggedin = false;
            try
            {
                //this.Invoke(new MethodInvoker(delegate
                //{
                try
                {
                    if (!this.IsHandleCreated)
                    {
                        string abc = "handle not created";
                    }

                    this.Invoke(new MethodInvoker(delegate
                    {
                        AddLoggerScrapeUsers("[ " + DateTime.Now + " ] => [ Process Running Please wait for sometimes... ]");
                    }));

                    if (SearchCriteria.SignIN)
                    {
                        LinkedinLogin Login = new LinkedinLogin();
                        //For Sign Out 
                        Login.LogoutHttpHelper();
                        SearchCriteria.SignOut = true;
                    }

                    if (SearchCriteria.SignOut)
                    {
                        SearchCriteria.LoginID = string.Empty;
                        if (LinkedInManager.linkedInDictionary.Count() > 0)
                        {
                            try
                            {
                                object temp = null;
                                comboBoxemail.Invoke(new MethodInvoker(delegate
                                {
                                    temp = comboBoxemail.SelectedItem;
                                }));

                                if (temp != null)
                                {
                                    //GlobusHttpHelper httpHelper = new GlobusHttpHelper();
                                    string acc = "";
                                    comboBoxemail.Invoke(new MethodInvoker(delegate
                                    {
                                        acc = comboBoxemail.SelectedItem.ToString();
                                        SearchCriteria.LoginID = comboBoxemail.SelectedItem.ToString();//change 21/08/12
                                    }));
                                    //string acc = account.Remove(account.IndexOf(':'));

                                    //Run a separate thread for each account
                                    LinkedInMaster item = null;
                                    LinkedInManager.linkedInDictionary.TryGetValue(acc, out item);

                                    item.logger.addToLogger += ScrapeEvent_addToLogger;
                                    item.LoginHttpHelper(ref HttpHelper);

                                    if (item.IsLoggedIn)
                                    {
                                        AddLoggerScrapeUsers("[ " + DateTime.Now + " ] => [ Logged in With Account : " + acc + " ]");
                                        isLoggedin = true;
                                    }

                                    if (SearchCriteria.loginREsponce.Contains("Your LinkedIn account has been temporarily restricted"))
                                    {
                                        AddLoggerScrapeUsers("[ " + DateTime.Now + " ] => [ " + SearchCriteria.LoginID + "Your LinkedIn account has been temporarily restricted ]");
                                    }

                                    if (SearchCriteria.loginREsponce.Contains("Please confirm your email address"))
                                    {
                                        AddLoggerScrapeUsers("[ " + DateTime.Now + " ] => [ " + SearchCriteria.LoginID + " account has been temporarily restricted Please confirm your email address ]");
                                    }

                                    if (!item.IsLoggedIn)
                                    {
                                        AddLoggerScrapeUsers("[ " + DateTime.Now + " ] => [ Couldn't Login With : " + acc + " ]");
                                    }

                                    SearchCriteria.SignIN = true;
                                    SearchCriteria.SignOut = false;
                                    try
                                    {
                                        this.Invoke(new MethodInvoker(delegate
                                        {
                                            //LoadCmboData();
                                        }));
                                    }
                                    catch
                                    {
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> comboBoxemail_SelectedIndexChanged() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> comboBoxemail_SelectedIndexChanged() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPreScrapperErrorLogs);
                            }
                        }
                    }
                }
                catch
                {
                }
                //}));
                //DisableEnableScrapperControls();
            }
            catch
            {
            }

            return isLoggedin;
        }

        private bool Login_InBoardProGetDataForCampaignScraper(ref GlobusHttpHelper HttpHelper, string Account, string password, string ProxyAddress, string ProxyPort, string ProxyUserName, string ProxyPassword)
        {
            //Thread.Sleep(1 * 60 * 1000);

            bool isLoggedin = false;
            try
            {
                //this.Invoke(new MethodInvoker(delegate
                //{
                try
                {
                    if (!this.IsHandleCreated)
                    {
                        string abc = "handle not created";
                    }

                    this.Invoke(new MethodInvoker(delegate
                    {
                        AddLoggerScrapeUsers("[ " + DateTime.Now + " ] => [ Process Running Please wait for sometimes... ]");
                    }));

                    if (SearchCriteria.SignIN)
                    {
                        LinkedinLogin Login = new LinkedinLogin();
                        //For Sign Out 
                        Login.LogoutHttpHelper();
                        SearchCriteria.SignOut = true;
                    }

                    if (SearchCriteria.SignOut)
                    {
                        SearchCriteria.LoginID = string.Empty;
                        //   if (LinkedInManager.linkedInDictionary.Count() > 0)
                        //   {
                        try
                        {
                            object temp = null;
                            //  comboBoxemail.Invoke(new MethodInvoker(delegate
                            // {
                            //    temp = comboBoxemail.SelectedItem;
                            //  }));

                            //   if (temp != null)
                            //  {
                            //GlobusHttpHelper httpHelper = new GlobusHttpHelper();
                            string acc = "";
                            //comboBoxemail.Invoke(new MethodInvoker(delegate
                            //  {
                            //    acc = comboBoxemail.SelectedItem.ToString();
                            //     SearchCriteria.LoginID = comboBoxemail.SelectedItem.ToString();//change 21/08/12
                            // }));
                            //string acc = account.Remove(account.IndexOf(':'));

                            //Run a separate thread for each account

                            SearchCriteria.LoginID = Account;
                            //LinkedInMaster item = null;
                            LinkedInMaster item = new LinkedInMaster();
                            //LinkedInManager.linkedInDictionary.TryGetValue(acc, out item);

                            //item.logger.addToLogger += ScrapeEvent_addToLogger;
                            item.LoginHttpHelperForCampaignScraper(ref HttpHelper, Account, password, ProxyAddress, ProxyPort, ProxyUserName, ProxyPassword);

                            if (item.IsLoggedIn)
                            {
                                AddLoggerScrapeUsers("[ " + DateTime.Now + " ] => [ Logged in With Account : " + Account + " ]");
                                isLoggedin = true;
                            }

                            if (SearchCriteria.loginREsponce.Contains("Your LinkedIn account has been temporarily restricted"))
                            {
                                AddLoggerScrapeUsers("[ " + DateTime.Now + " ] => [ " + SearchCriteria.LoginID + "Your LinkedIn account has been temporarily restricted ]");
                            }

                            if (SearchCriteria.loginREsponce.Contains("Please confirm your email address"))
                            {
                                AddLoggerScrapeUsers("[ " + DateTime.Now + " ] => [ " + SearchCriteria.LoginID + " account has been temporarily restricted Please confirm your email address ]");
                            }

                            if (!item.IsLoggedIn)
                            {
                                AddLoggerScrapeUsers("[ " + DateTime.Now + " ] => [ Couldn't Login With : " + Account + " ]");
                            }

                            SearchCriteria.SignIN = true;
                            SearchCriteria.SignOut = false;
                            try
                            {
                                this.Invoke(new MethodInvoker(delegate
                                {
                                    //LoadCmboData();
                                }));
                            }
                            catch
                            {
                            }
                            // }
                        }
                        catch (Exception ex)
                        {
                            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> comboBoxemail_SelectedIndexChanged() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> comboBoxemail_SelectedIndexChanged() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPreScrapperErrorLogs);
                        }
                        // }
                    }
                }
                catch
                {
                }
                //}));
                //DisableEnableScrapperControls();
            }
            catch
            {
            }

            return isLoggedin;
        }

        #endregion

        #region LoadCmboData()

        public void LoadCmboData()
        {
            if (!string.IsNullOrEmpty(SearchCriteria.loginREsponce))
            {
                string responce = SearchCriteria.loginREsponce;

                if (!responce.Contains("premium-member"))
                {
                    txtScraperMinDelay.Visible = true;
                    checkedListBoxFortune1000.Enabled = false;
                    checkedListBoxCompanySize.Enabled = false;
                    checkedListBoxInterestedIN.Enabled = false;
                    checkedListBoxSeniorlevel.Enabled = false;
                    checkedListBoxRecentlyJoined.Enabled = false;
                    GetDataForPrimiumAccount.Enabled = false;
                    checkedListFunction.Enabled = false;
                    AddLoggerScrapeUsers("[ " + DateTime.Now + " ] => [ Selected Account is not a Premium Account.. ]");
                }

                if (responce.Contains("premium-member") || responce.Contains("Recruiter Account"))
                {
                    
                    txtScraperMinDelay.Visible = true;
                    checkedListBoxFortune1000.Enabled = true;
                    checkedListBoxCompanySize.Enabled = true;
                    checkedListBoxInterestedIN.Enabled = true;
                    checkedListBoxSeniorlevel.Enabled = true;
                    checkedListBoxRecentlyJoined.Enabled = true;
                    GetDataForPrimiumAccount.Enabled = true;
                    checkedListFunction.Enabled = true;
                    if (responce.Contains("premium-member"))
                    {
                        AddLoggerScrapeUsers("[ " + DateTime.Now + " ] => [ Selected Account is a Premium Account.. ]");
                    }
                    else if (responce.Contains("Recruiter Account"))
                    {
                        AddLoggerScrapeUsers("[ " + DateTime.Now + " ] => [ Selected Account is a Recruiter Account.. ]");
                        SearchCriteria.AccountType = "RecuiterType";
                        checkedListBoxFortune1000.Items.Clear();
                        checkedListBoxCompanySize.Items.Clear();
                        checkedListBoxInterestedIN.Items.Clear();
                        checkedListBoxSeniorlevel.Items.Clear();
                        checkedListBoxRecentlyJoined.Items.Clear();
                        GetDataForPrimiumAccount.Items.Clear();
                        checkedListFunction.Items.Clear();
                        Uploaddata();
                    }
                }

                string[] combodata = System.Text.RegularExpressions.Regex.Split(responce, "input");
                foreach (string item in combodata)
                {
                    if (item.Contains("facet.I-id-"))
                    {
                        try
                        {
                            string value = item.Substring(item.IndexOf("value=\""), item.IndexOf("name=") - item.IndexOf("value=\"")).Replace("value=\"", "").Replace("\"", "").Trim();
                            string tempkey = item.Substring(item.IndexOf("label for=\"facet.I-id"), item.IndexOf("</label>") - item.IndexOf("label for=\"facet.I-id")).Replace("label for=\"facet.I-id", "").Replace("\"", "").Trim();
                            string[] arrkey = tempkey.Split('>');
                            string key = arrkey[1];
                            Globals.Industry.Add(key, value);
                            checkedListBoxIndustry.Items.Add(key);
                        }
                        catch (Exception ex)
                        {
                            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> LoadCmboData() ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> LoadCmboData() ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPreScrapperErrorLogs);
                        }
                    }
                    //facet.FG-id

                    if (item.Contains("facet.FG-id"))
                    {
                        try
                        {
                            string value = item.Substring(item.IndexOf("value=\""), item.IndexOf("name=") - item.IndexOf("value=\"")).Replace("value=\"", "").Replace("\"", "").Trim();
                            string tempkey = item.Substring(item.IndexOf("label for=\"facet.FG-id-"), item.IndexOf("</label>") - item.IndexOf("label for=\"facet.FG-id-")).Replace("label for=\"facet.FG-id-", "").Replace("\"", "").Trim();
                            string[] arrkey = tempkey.Split('>');
                            string key = arrkey[1];
                            Globals.Groups.Add(key, value);
                            checkedListBoxGroups.Items.Add(key);
                        }
                        catch (Exception ex)
                        {
                            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> LoadCmboData() ---2--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> LoadCmboData() ---2--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPreScrapperErrorLogs);
                        }
                    }
                    //facet.CS-id
                    if (item.Contains("facet.CS-id"))
                    {
                        try
                        {
                            string value = item.Substring(item.IndexOf("value=\""), item.IndexOf("name=") - item.IndexOf("value=\"")).Replace("value=\"", "").Replace("\"", "").Trim();
                            string tempkey = item.Substring(item.IndexOf("for=\"facet.CS-id"), item.IndexOf("</label>") - item.IndexOf("for=\"facet.CS-id")).Replace("for=\"facet.CS-id", "").Replace("\"", "").Trim();
                            string[] arrkey = tempkey.Split('>');
                            string key = arrkey[1];
                            Globals.CompanySize.Add(key, value);
                            checkedListBoxCompanySize.Items.Add(key);
                        }
                        catch (Exception ex)
                        {
                            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> LoadCmboData() ---3--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> LoadCmboData() ---3--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPreScrapperErrorLogs);
                        }
                    }

                    if (item.Contains("facet.SE-id"))
                    {
                        try
                        {
                            string value = item.Substring(item.IndexOf("value=\""), item.IndexOf("name=") - item.IndexOf("value=\"")).Replace("value=\"", "").Replace("\"", "").Trim();
                            string tempkey = item.Substring(item.IndexOf("for=\"facet.SE-id"), item.IndexOf("</label>") - item.IndexOf("for=\"facet.SE-id")).Replace("for=\"facet.SE-id", "").Replace("\"", "").Trim();
                            string[] arrkey = tempkey.Split('>');
                            string key = arrkey[1];
                            Globals.SeniorLevel.Add(key, value);
                            checkedListBoxSeniorlevel.Items.Add(key);
                        }
                        catch (Exception ex)
                        {
                            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> LoadCmboData() ---4--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> LoadCmboData() ---4--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPreScrapperErrorLogs);
                        }
                    }
                    //Intrust
                    if (item.Contains("facet.P-id"))
                    {
                        try
                        {
                            string value = item.Substring(item.IndexOf("value=\""), item.IndexOf("name=") - item.IndexOf("value=\"")).Replace("value=\"", "").Replace("\"", "").Trim();
                            string tempkey = item.Substring(item.IndexOf("for=\"facet.P-id"), item.IndexOf("</label>") - item.IndexOf("for=\"facet.P-id")).Replace("for=\"facet.P-id", "").Replace("\"", "").Trim();
                            string[] arrkey = tempkey.Split('>');
                            string key = arrkey[1];
                            Globals.InterestedIN.Add(key, value);
                            checkedListBoxInterestedIN.Items.Add(key);
                        }
                        catch (Exception ex)
                        {
                            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> LoadCmboData() ---5--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> LoadCmboData() ---5--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPreScrapperErrorLogs);
                        }
                    }
                    //total Exp
                    if (item.Contains("facet.TE-id"))
                    {
                        try
                        {
                            string value = item.Substring(item.IndexOf("value=\""), item.IndexOf("name=") - item.IndexOf("value=\"")).Replace("value=\"", "").Replace("\"", "").Trim();
                            string tempkey = item.Substring(item.IndexOf("for=\"facet.TE-id"), item.IndexOf("</label>") - item.IndexOf("for=\"facet.TE-id")).Replace("for=\"facet.TE-id", "").Replace("\"", "").Trim();
                            string[] arrkey = tempkey.Split('>');
                            string key = arrkey[1];
                            Globals.YearOfExperience.Add(key, value);
                            GetDataForPrimiumAccount.Items.Add(key);
                        }
                        catch (Exception ex)
                        {
                            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> LoadCmboData() ---6--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> LoadCmboData() ---6--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPreScrapperErrorLogs);
                        }
                    }

                    if (item.Contains("facet.F-id"))
                    {
                        try
                        {
                            string value = item.Substring(item.IndexOf("value=\""), item.IndexOf("name=") - item.IndexOf("value=\"")).Replace("value=\"", "").Replace("\"", "").Trim();
                            string tempkey = item.Substring(item.IndexOf("for=\"facet.F-id"), item.IndexOf("</label>") - item.IndexOf("for=\"facet.F-id")).Replace("for=\"facet.F-id", "").Replace("\"", "").Trim();
                            string[] arrkey = tempkey.Split('>');
                            string key = arrkey[1];
                            Globals.Fortune1000.Add(key, value);
                            checkedListBoxFortune1000.Items.Add(key);
                        }
                        catch (Exception ex)
                        {
                            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> LoadCmboData() ---7--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> LoadCmboData() ---7--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPreScrapperErrorLogs);
                        }
                    }

                    if (item.Contains("facet.FA-id"))
                    {
                        try
                        {
                            string value = item.Substring(item.IndexOf("value=\""), item.IndexOf("name=") - item.IndexOf("value=\"")).Replace("value=\"", "").Replace("\"", "").Trim();
                            string tempkey = item.Substring(item.IndexOf("for=\"facet.FA-id"), item.IndexOf("</label>") - item.IndexOf("for=\"facet.FA-id")).Replace("for=\"facet.FA-id", "").Replace("\"", "").Trim();
                            string[] arrkey = tempkey.Split('>');
                            string key = arrkey[1];
                            Globals.Function.Add(key, value);
                            checkedListFunction.Items.Add(key);
                        }
                        catch (Exception ex)
                        {
                            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> LoadCmboData() ---8--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> LoadCmboData() ---8--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPreScrapperErrorLogs);
                        }
                    }//facet.FA-id

                    //Resentlly joined
                    if (item.Contains("facet.DR-id"))
                    {
                        try
                        {
                            string value = item.Substring(item.IndexOf("value=\""), item.IndexOf("name=") - item.IndexOf("value=\"")).Replace("value=\"", "").Replace("\"", "").Trim();
                            string tempkey = item.Substring(item.IndexOf("for=\"facet.DR-id"), item.IndexOf("</label>") - item.IndexOf("for=\"facet.DR-id")).Replace("for=\"facet.DR-id", "").Replace("\"", "").Trim();
                            string[] arrkey = tempkey.Split('>');
                            string key = arrkey[1];
                            Globals.RecentlyJoined.Add(key, value);
                            checkedListBoxRecentlyJoined.Items.Add(key);
                        }
                        catch (Exception ex)
                        {
                            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> LoadCmboData() ---9--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> LoadCmboData() ---9--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPreScrapperErrorLogs);
                        }
                    }//facet.FA-id

                    if (item.Contains("facet.L-id"))
                    {
                        try
                        {
                            string value = item.Substring(item.IndexOf("value=\""), item.IndexOf("name=") - item.IndexOf("value=\"")).Replace("value=\"", "").Replace("\"", "").Trim();
                            string tempkey = item.Substring(item.IndexOf("for=\"facet.L-id"), item.IndexOf("</label>") - item.IndexOf("for=\"facet.L-id")).Replace("for=\"facet.L-id", "").Replace("\"", "").Trim();
                            string[] arrkey = tempkey.Split('>');
                            string key = arrkey[1];
                            Globals.Language.Add(key, value);
                            checkedListBoxLanguage.Items.Add(key);
                        }
                        catch (Exception ex)
                        {
                            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> LoadCmboData() ---10--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> LoadCmboData() ---10--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPreScrapperErrorLogs);
                        }
                    }//facet.FA-id facet.N-id

                    if (item.Contains("facet.N-id"))
                    {
                        try
                        {
                            string value = item.Substring(item.IndexOf("value=\""), item.IndexOf("name=") - item.IndexOf("value=\"")).Replace("value=\"", "").Replace("\"", "").Trim();
                            string tempkey = item.Substring(item.IndexOf("for=\"facet.N-id"), item.IndexOf("</label>") - item.IndexOf("for=\"facet.N-id")).Replace("for=\"facet.N-id", "").Replace("\"", "").Trim();
                            string[] arrkey = tempkey.Split('>');
                            string key = arrkey[1];
                            Globals.RelationShip.Add(key, value);
                            checkedListRelationship.Items.Add(key);
                        }
                        catch (Exception ex)
                        {
                            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> LoadCmboData() ---11--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> LoadCmboData() ---11--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPreScrapperErrorLogs);
                        }
                    }//facet.
                }
            }

            AddLoggerScrapeUsers("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
            AddLoggerScrapeUsers("--------------------------------------------------------------------------------------------------------------------------");
            //DisableEnableScrapperControls();
        }

        #endregion

        #region combScraperLocation_SelectedIndexChanged

        private void combScraperLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (combScraperLocation.SelectedIndex == 0)
                {
                    SearchCriteria.Location = "I";
                    lblScraperCountry.Visible = true;
                    CombScraperCountry.Visible = true;
                }
                else if (combScraperLocation.SelectedIndex == 1)
                {
                    SearchCriteria.Location = "Y";
                    SearchCriteria.Country = string.Empty;
                    lblScraperCountry.Visible = false;
                    CombScraperCountry.Visible = false;
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> combScraperLocation_SelectedIndexChanged() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> combScraperLocation_SelectedIndexChanged() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPreScrapperErrorLogs);
            }
        }

        #endregion

        #region btnSearchNewScraper_Click

        private void btnSearchNewScraper_Click(object sender, EventArgs e)
        {
            try
            {
                combScraperLocation.SelectedIndex = 0;
                txtScrperFirstname.Text = string.Empty;
                txtScrperlastname.Text = string.Empty;

                txtPostalcode.Text = string.Empty;
                txtCompnayName.Text = string.Empty;

                txtKeyword.Text = string.Empty;
                txtTitle.Text = string.Empty;
                txtAreaWiseLocation.Text = string.Empty;


                txtScraperExportFilename.Text = string.Empty;
                CombScraperCountry.SelectedIndex = 0;

                lstLoglinkdinPreScarper.Items.Clear();

                try
                {
                    for (int i = 0; i < checkedListBoxCompanySize.Items.Count; i++)
                    {
                        checkedListBoxCompanySize.SetItemChecked(i, false);
                    }

                    for (int i = 0; i < checkedListBoxIndustry.Items.Count; i++)
                    {
                        checkedListBoxIndustry.SetItemChecked(i, false);
                    }

                    for (int i = 0; i < checkedListBoxSeniorlevel.Items.Count; i++)
                    {
                        checkedListBoxSeniorlevel.SetItemChecked(i, false);
                    }
                    for (int i = 0; i < checkedListBoxGroups.Items.Count; i++)
                    {
                        checkedListBoxGroups.SetItemChecked(i, false);
                    }

                    for (int i = 0; i < checkedListBoxFortune1000.Items.Count; i++)
                    {
                        checkedListBoxFortune1000.SetItemChecked(i, false);
                    }

                    for (int i = 0; i < checkedListBoxInterestedIN.Items.Count; i++)
                    {
                        checkedListBoxInterestedIN.SetItemChecked(i, false);
                    }

                    for (int i = 0; i < checkedListBoxLanguage.Items.Count; i++)
                    {
                        checkedListBoxLanguage.SetItemChecked(i, false);
                    }
                    for (int i = 0; i < checkedListBoxRecentlyJoined.Items.Count; i++)
                    {
                        checkedListBoxRecentlyJoined.SetItemChecked(i, false);
                    }

                    for (int i = 0; i < GetDataForPrimiumAccount.Items.Count; i++)
                    {
                        GetDataForPrimiumAccount.SetItemChecked(i, false);
                    }
                    for (int i = 0; i < checkedListFunction.Items.Count; i++)
                    {
                        checkedListFunction.SetItemChecked(i, false);
                    }
                    for (int i = 0; i < checkedListRelationship.Items.Count; i++)
                    {
                        checkedListRelationship.SetItemChecked(i, false);
                    }
                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnSearchNewScraper_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnSearchNewScraper_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPreScrapperErrorLogs);
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnSearchNewScraper_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnSearchNewScraper_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPreScrapperErrorLogs);
            }
        }

        #endregion

        #region PopulateCmo()

        private void PopulateCmo()
        {
            try
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    cmbLinkedinSelectEmailId.Items.Clear();
                    comboBoxemail.Items.Clear();
                    cmbSelAccount.Items.Clear();
                    cmbSelAccountShare.Items.Clear();
                }));

                foreach (KeyValuePair<string, LinkedInMaster> item in LinkedInManager.linkedInDictionary)
                {
                    this.Invoke(new MethodInvoker(delegate
                    {
                        cmbLinkedinSelectEmailId.Items.Add(item.Key);
                        comboBoxemail.Items.Add(item.Key);
                        cmbSelAccount.Items.Add(item.Key);
                        cmbSelAccountShare.Items.Add(item.Key);
                    }));
                }
                AddLoggerScrapeUsers("[ " + DateTime.Now + " ] => [ Accounts Uploaded.. ]");
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> PopulateCmo() >>>> " + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> PopulateCmo() >>>> " + ex.StackTrace, Globals.Path_LinkedinPreScrapperErrorLogs);
            }
        }

        #endregion

        #region btnStopScraper_Click

        private void btnStopScraper_Click(object sender, EventArgs e)
        {
            try
            {
                IsStop_InBoardProGetDataThread = true;
                List<Thread> lstTemp = lstInBoardProGetDataThraed.Distinct().ToList();
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

                List<Thread> lstTemp1 = Globals.lstCompanyEmployeeScraperThread.Distinct().ToList();
                foreach (Thread item in lstTemp1)
                {
                    try
                    {
                        item.Abort();
                    }
                    catch
                    { }
                }

                AddLoggerInBoardProGetData("-------------------------------------------------------------------------------------------------------------------------------");
                AddLoggerInBoardProGetData("[ " + DateTime.Now + " ] => [ PROCESS STOPPED ]");
                AddLoggerInBoardProGetData("-------------------------------------------------------------------------------------------------------------------------------");
                btnSearchScraper.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
            }

        }


        private void btnScraperPauseResume_Click(object sender, EventArgs e)
        {
            if (btnScraperPauseResume.Text == "Pause")
            {

                btnScraperPauseResume.Text = "Resume";
                try
                {
                    IsStop_InBoardProGetDataThread = true;
                    List<Thread> lstTemp = lstInBoardProGetDataThraed.Distinct().ToList();
                    foreach (Thread item in lstTemp)
                    {
                        try
                        {
                            item.Suspend();
                        }
                        catch
                        {
                        }
                    }

                    List<Thread> lstTemp1 = Globals.lstCompanyEmployeeScraperThread.Distinct().ToList();
                    foreach (Thread item in lstTemp1)
                    {
                        try
                        {
                            item.Suspend();
                        }
                        catch
                        { }
                    }

                    AddLoggerInBoardProGetData("-------------------------------------------------------------------------------------------------------------------------------");
                    AddLoggerInBoardProGetData("[ " + DateTime.Now + " ] => [ Process Paused ]");
                    AddLoggerInBoardProGetData("-------------------------------------------------------------------------------------------------------------------------------");
                    btnSearchScraper.Cursor = Cursors.Default;
                }
                catch (Exception ex)
                {
                }
            }

            else
            {
                if (btnScraperPauseResume.Text == "Resume")
                {

                    btnScraperPauseResume.Text = "Pause";
                    try
                    {
                        IsStop_InBoardProGetDataThread = true;
                        List<Thread> lstTemp = lstInBoardProGetDataThraed.Distinct().ToList();
                        foreach (Thread item in lstTemp)
                        {
                            try
                            {
                                item.Resume();
                            }
                            catch
                            {
                            }
                        }

                        List<Thread> lstTemp1 = Globals.lstCompanyEmployeeScraperThread.Distinct().ToList();
                        foreach (Thread item in lstTemp1)
                        {
                            try
                            {
                                item.Resume();
                            }
                            catch
                            { }
                        }

                        AddLoggerInBoardProGetData("-------------------------------------------------------------------------------------------------------------------------------");
                        AddLoggerInBoardProGetData("[ " + DateTime.Now + " ] => [ Process resumed ]");
                        AddLoggerInBoardProGetData("-------------------------------------------------------------------------------------------------------------------------------");
                        btnSearchScraper.Cursor = Cursors.Default;
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }

        }

        #endregion

        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> PROXY SETTINGS >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

        #region GetValidProxies

        private void GetValidProxies(List<string> lstProxies)
        {
            try
            {
                try
                {
                    lstStopPublicProxyTest.Add(Thread.CurrentThread);
                    lstStopPublicProxyTest = lstStopPublicProxyTest.Distinct().ToList();
                    Thread.CurrentThread.IsBackground = true;
                }
                catch { }

                if (GlobusRegex.ValidateNumber(txtNumberOfProxyThreads.Text))
                {
                    numberOfProxyThreads = int.Parse(txtNumberOfProxyThreads.Text);
                }


                // WaitCallback waitCallBack = new WaitCallback(ThreadPoolMethod_Proxies);

                foreach (string item in lstProxies)
                {
                    //if (countParseProxiesThreads >= numberOfProxyThreads)
                    //{
                    //    lock (proxiesThreadLockr)
                    //    {
                    //        Monitor.Wait(proxiesThreadLockr);
                    //    }
                    //}

                    //Thread GetStartProcessForChAngeAcPassword = new Thread(ThreadPoolMethod_Proxies);
                    ////Code for checking and then adding proxies to FinalProxyList...
                    // ThreadPool.SetMaxThreads(numberOfProxyThreads, numberOfProxyThreads);

                    //  ThreadPool.QueueUserWorkItem(waitCallBack, item);
                    ////Thread.Sleep(3000);
                    if (!proxy_stop)
                    {
                        ThreadPool.SetMaxThreads(numberOfProxyThreads, 500);
                        ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadPoolMethod_Proxies), new object[] { item });
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Proxy Setting --> GetValidProxies() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Proxy Setting --> GetValidProxies() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinProxySettingErrorLogs);
            }



        }


        private void GetPrivateValidProxies_ProxySetting(List<string> lstProxies)
        {
            try
            {
                if (GlobusRegex.ValidateNumber(txtNumberOfProxyThreads.Text))
                {
                    numberOfProxyThreads = int.Parse(txtNumberOfProxyThreads.Text);
                }

                WaitCallback waitCallBack = new WaitCallback(ThreadPoolMethod_PrivateProxies);
                foreach (string item in lstProxies)
                {
                    if (countParseProxiesThreads >= numberOfProxyThreads)
                    {
                        lock (proxiesThreadLockr)
                        {
                            Monitor.Wait(proxiesThreadLockr);
                        }
                    }

                    //Code for checking and then adding proxies to FinalProxyList...
                    if (!proxy_stop)
                    {
                        Thread GetStartProcessForChAngeAcPassword = new Thread(ThreadPoolMethod_Proxies);
                        ThreadPool.QueueUserWorkItem(waitCallBack, item);
                        Thread.Sleep(3000);
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Proxy Setting --> GetValidProxies() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Proxy Setting --> GetValidProxies() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinProxySettingErrorLogs);
            }
            //AddToProxysLogs(ValidPublicProxies.Count() + " Public Proxies Valid");
        }

        #endregion

        #region ThreadPoolMethod_Proxies

        private void ThreadPoolMethod_Proxies(object objProxy)
        {

            try
            {
                if (!proxy_stop)
                {
                    try
                    {
                        lstStopPublicProxyTest.Add(Thread.CurrentThread);
                        lstStopPublicProxyTest = lstStopPublicProxyTest.Distinct().ToList();
                        Thread.CurrentThread.IsBackground = true;
                    }
                    catch { }

                    countParseProxiesThreads++;

                    string item = string.Empty;


                    Array paramsArray = new Array[1];
                    paramsArray = (Array)objProxy;

                    try
                    {
                        item = (string)paramsArray.GetValue(0);
                    }
                    catch { };
                    int IsPublic = 0;
                    //int Working = 0;
                    string LoggedInIp = string.Empty;
                    string proxyAddress = string.Empty;
                    string proxyPort = string.Empty;
                    string proxyUserName = string.Empty;
                    string proxyPassword = string.Empty;
                    string account = item;
                    int DataCount = account.Split(':').Length;
                    #region Check condition Thread is Stop or Not


                    #endregion

                    if (DataCount == 1)
                    {
                        proxyAddress = account.Split(':')[0];
                        AddToProxysLogs("[ " + DateTime.Now + " ] => [ Proxy Not In correct Format ]");
                        AddToProxysLogs(account);
                        return;
                    }
                    if (DataCount == 2)
                    {
                        proxyAddress = account.Split(':')[0];
                        proxyPort = account.Split(':')[1];
                    }
                    else if (DataCount > 2)
                    {
                        //proxyAddress = account.Split(':')[0];
                        //proxyPort = account.Split(':')[1];
                        //proxyUserName = account.Split(':')[2];
                        //proxyPassword = account.Split(':')[3];
                        AddToProxysLogs("[ " + DateTime.Now + " ] => [ Proxy Not In correct Format ]");
                        AddToProxysLogs(account);
                        return;
                    }

                    ProxyChecker proxyChecker = new ProxyChecker(proxyAddress, proxyPort, proxyUserName, proxyPassword, IsPublic);
                    if (proxyChecker.CheckProxy())
                    {
                        //lock (((System.Collections.ICollection)listWorkingProxies).SyncRoot)
                        {
                            //if (!listWorkingProxies.Contains(proxy))
                            {
                                workingproxiesCount++;
                                //listWorkingProxies.Add(proxy);
                                lock (proxyListLockr)
                                {
                                    queWorkingProxies.Enqueue(item);
                                    Monitor.Pulse(proxyListLockr);
                                }
                                AddToProxysLogs("[ " + DateTime.Now + " ] => [ Added " + item + " to working proxies list ]");
                                //Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(item, Globals.FilePathWorkingProxies);
                            }
                        }
                    }
                    else
                    {
                        AddToProxysLogs("[ " + DateTime.Now + " ] => [ Non Working Proxy: " + proxyAddress + ":" + proxyPort + " ]");
                        GlobusFileHelper.AppendStringToTextfileNewLine(item, Globals.Path_Non_ExsistingProxies);
                    }

                    #region Commented

                    //string pageSource = string.Empty;
                    //try
                    //{
                    //    GlobusHttpHelper httpHelper = new GlobusHttpHelper();
                    //    pageSource = httpHelper.getHtmlfromUrlProxy(new Uri("https://twitter.com/"), "", proxyAddress, proxyPort, proxyUserName, proxyPassword);
                    //}
                    //catch (Exception ex)
                    //{
                    //    GlobusFileHelper.AppendStringToTextfileNewLine(item, Globals.Path_Non_ExsistingProxies);
                    //}
                    //if (pageSource.Contains("class=\"signin\"") && pageSource.Contains("class=\"signup\"") && pageSource.Contains("Twitter"))
                    //{
                    //    using (SQLiteConnection con = new SQLiteConnection(DataBaseHandler.CONstr))
                    //    {
                    //        //using (SQLiteDataAdapter ad = new SQLiteDataAdapter("SELECT * FROM tb_FBAccount WHERE ProxyAddress = '" + proxyAddress + "'", con))
                    //        using (SQLiteDataAdapter ad = new SQLiteDataAdapter())
                    //        {
                    //            if (DataCount >= 2)
                    //            {
                    //                //0 is true
                    //                IsPublic = 0;
                    //            }
                    //            else
                    //            {
                    //                //1 is false
                    //                IsPublic = 1;
                    //            }
                    //            Working = 1;
                    //            string InsertQuery = "Insert into tb_Proxies values('" + proxyAddress + "','" + proxyPort + "','" + proxyUserName + "','" + proxyPassword + "', " + Working + "," + IsPublic + " , '" + LoggedInIp + "')";
                    //            DataBaseHandler.InsertQuery(InsertQuery, "tb_Proxies");
                    //        }
                    //    }
                    //    ValidPublicProxies.Add(item);
                    //} 

                    #endregion

                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Proxy Setting --> ThreadPoolMethod_Proxies() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Proxy Setting --> ThreadPoolMethod_Proxies() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinProxySettingErrorLogs);

            }

            finally
            {

                lock (proxiesThreadLockr)
                {
                    countParseProxiesThreads--;
                    Monitor.Pulse(proxiesThreadLockr);
                }

                Proxystatus--;
                if (Proxystatus == 0)
                {
                    if (btnTestPublicProxy.InvokeRequired)
                    {
                        btnTestPublicProxy.Invoke(new MethodInvoker(delegate
                        {
                            AddToProxysLogs("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                            AddToProxysLogs("-----------------------------------------------------------------------------------------------------------------------------------");
                        }));
                    }
                }
            }
        }
        #endregion

        #region ThreadPoolMethod_PrivateProxies
        private void ThreadPoolMethod_PrivateProxies(object objProxy)
        {
            if (!IsStop_Proxy)
                try
                {
                    countParseProxiesThreads++;
                    string item = (string)objProxy;
                    int IsPublic = 0;
                    string LoggedInIp = string.Empty;
                    string proxyAddress = string.Empty;
                    string proxyPort = string.Empty;
                    string proxyUserName = string.Empty;
                    string proxyPassword = string.Empty;
                    string account = item;
                    int DataCount = account.Split(':').Length;
                    #region Check condition Thread is Stop or Not

                    if (IsStop_Proxy)
                    {
                        return;
                    }

                    lstStopPrivateProxyTest.Add(Thread.CurrentThread);
                    lstStopPrivateProxyTest = lstStopPrivateProxyTest.Distinct().ToList();
                    Thread.CurrentThread.IsBackground = true;

                    #endregion

                    if (DataCount == 4)
                    {
                        proxyAddress = account.Split(':')[0];
                        proxyPort = account.Split(':')[1];
                        proxyUserName = account.Split(':')[2];
                        proxyPassword = account.Split(':')[3];
                    }
                    else
                    {
                        AddToProxysLogs("[ " + DateTime.Now + " ] => [ " + account + " Proxy Not In correct Format ]");
                    }
                    if (!IsStop_Proxy)
                    {
                        ProxyChecker proxyChecker = new ProxyChecker(proxyAddress, proxyPort, proxyUserName, proxyPassword, IsPublic);

                        if (proxyChecker.CheckProxy())
                        {
                            //lock (((System.Collections.ICollection)listWorkingProxies).SyncRoot)
                            {
                                //if (!listWorkingProxies.Contains(proxy))
                                {
                                    workingproxiesCount++;
                                    //listWorkingProxies.Add(proxy);
                                    lock (proxyListLockr)
                                    {
                                        queWorkingProxies.Enqueue(item);
                                        Monitor.Pulse(proxyListLockr);
                                    }
                                    AddToProxysLogs("[ " + DateTime.Now + " ] => [ Added " + item + " to working proxies list ]");
                                    GlobusFileHelper.AppendStringToTextfileNewLine(item, Globals.Path_WorkingPvtProxies);
                                }
                            }
                        }
                        else
                        {
                            AddToProxysLogs("[ " + DateTime.Now + " ] => [ Non Working Proxy: " + proxyAddress + ":" + proxyPort + " ]");
                            GlobusFileHelper.AppendStringToTextfileNewLine(item, Globals.Path_Non_ExsistingProxies);
                        }

                        #region Commented

                        //string pageSource = string.Empty;
                        //try
                        //{
                        //    GlobusHttpHelper httpHelper = new GlobusHttpHelper();
                        //    pageSource = httpHelper.getHtmlfromUrlProxy(new Uri("https://twitter.com/"), "", proxyAddress, proxyPort, proxyUserName, proxyPassword);
                        //}
                        //catch (Exception ex)
                        //{
                        //    GlobusFileHelper.AppendStringToTextfileNewLine(item, Globals.Path_Non_ExsistingProxies);
                        //}
                        //if (pageSource.Contains("class=\"signin\"") && pageSource.Contains("class=\"signup\"") && pageSource.Contains("Twitter"))
                        //{
                        //    using (SQLiteConnection con = new SQLiteConnection(DataBaseHandler.CONstr))
                        //    {
                        //        //using (SQLiteDataAdapter ad = new SQLiteDataAdapter("SELECT * FROM tb_FBAccount WHERE ProxyAddress = '" + proxyAddress + "'", con))
                        //        using (SQLiteDataAdapter ad = new SQLiteDataAdapter())
                        //        {
                        //            if (DataCount >= 2)
                        //            {
                        //                //0 is true
                        //                IsPublic = 0;
                        //            }
                        //            else
                        //            {
                        //                //1 is false
                        //                IsPublic = 1;
                        //            }
                        //            Working = 1;
                        //            string InsertQuery = "Insert into tb_Proxies values('" + proxyAddress + "','" + proxyPort + "','" + proxyUserName + "','" + proxyPassword + "', " + Working + "," + IsPublic + " , '" + LoggedInIp + "')";
                        //            DataBaseHandler.InsertQuery(InsertQuery, "tb_Proxies");
                        //        }
                        //    }
                        //    ValidPublicProxies.Add(item);
                        //} 

                        #endregion
                    }
                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Proxy Setting --> ThreadPoolMethod_Proxies() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Proxy Setting --> ThreadPoolMethod_Proxies() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinProxySettingErrorLogs);

                }
                finally
                {
                    if (!IsStop_Proxy)
                    {
                        lock (proxiesThreadLockr)
                        {
                            countParseProxiesThreads--;
                            Monitor.Pulse(proxiesThreadLockr);
                        }
                    }

                    Proxystatus--;
                    if (Proxystatus == 0)
                    {
                        if (btnTestPublicProxy.InvokeRequired)
                        {
                            btnTestPublicProxy.Invoke(new MethodInvoker(delegate
                            {
                                AddToProxysLogs("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                                AddToProxysLogs("-------------------------------------------------------------------------------------------------------------------------------------");
                            }));
                        }
                    }
                }
        }
        #endregion

        #region ReloadAccountsFromDataBase()

        private void ReloadAccountsFromDataBase()
        {
            try
            {
                clsLDAccount objclsFBAccount = new clsLDAccount();
                DataTable dt = objclsFBAccount.SelectAccoutsForGridView();

                if (dt.Rows.Count > 0)
                {
                    Globals.listAccounts.Clear();
                    LinkedInManager.linkedInDictionary.Clear();
                    ///Add LinkedIn instances to LinkedInAccountContainer.dictionary_LinkedInDictionary
                    foreach (DataRow dRow in dt.Rows)
                    {
                        try
                        {
                            LinkedInMaster linkedin = new LinkedInMaster();
                            linkedin._Username = dRow[0].ToString();
                            linkedin._Password = dRow[1].ToString();
                            linkedin._ProxyUsername = dRow[2].ToString();
                            linkedin._ProxyPort = dRow[3].ToString();
                            linkedin._ProxyUsername = dRow[4].ToString();
                            linkedin._ProxyPassword = dRow[5].ToString();

                            if (!string.IsNullOrEmpty(dRow[5].ToString()))
                            {
                                linkedin.profileStatus = int.Parse(dRow[5].ToString());
                            }

                            Globals.listAccounts.Add(linkedin._Username + ":" + linkedin._Password + ":" + linkedin._ProxyAddress + ":" + linkedin._ProxyPort + ":" + linkedin._ProxyUsername + ":" + linkedin._ProxyPassword);
                            LinkedInManager.linkedInDictionary.Add(linkedin._Username, linkedin);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.StackTrace);
                            GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> ReloadAccountsFromDataBase() -- Rows From DB --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_ProxySettingErroLog);
                            GlobusFileHelper.AppendStringToTextfileNewLine("Error --> ReloadAccountsFromDataBase() -- Rows From DB --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                        }
                    }
                    Console.WriteLine(Globals.listAccounts.Count + " Accounts loaded");
                    AddLoggerGeneral("[ " + DateTime.Now + " ] => [ " + Globals.listAccounts.Count + " Accounts loaded ]");
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> ReloadAccountsFromDataBase() --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_ProxySettingErroLog);
                GlobusFileHelper.AppendStringToTextfileNewLine("Error --> ReloadAccountsFromDataBase() --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
            }
        }

        #endregion

        #region btnPublicProxy_Click

        //List<string> lstProxiesNumber = new List<string>();
        private void btnPublicProxy_Click(object sender, EventArgs e)
        {
            try
            {
                txtPublicProxy.Text = "";
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "Text Files (*.txt)|*.txt";
                    ofd.InitialDirectory = Application.StartupPath;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtPublicProxy.Text = ofd.FileName;
                        lstProxies = GlobusFileHelper.ReadFiletoStringLst(txtPublicProxy.Text);

                        lstProxy_AccCreator.Clear();
                        lstProxy_AccCreator.AddRange(lstProxies);

                        AddToProxysLogs("[ " + DateTime.Now + " ] => [ " + lstProxies.Count + " Public Proxy Uploaded ]");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error >>> " + ex.StackTrace);
            }
        }

        #endregion

        #region btnTestPublicProxy_Click

        List<string> lstProxies = new List<string>();
        static int Proxystatus = 0;

        private void btnTestPublicProxy_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();


            if (CheckNetConn)
            {
                IsStop_Proxy = false;
                lstStopPublicProxyTest.Clear();
                try
                {
                    AddToProxysLogs("[ " + DateTime.Now + " ] => [ Exsisting Proxies Saved To : ]");
                    AddToProxysLogs("[ " + DateTime.Now + " ] => [ " + Globals.Path_ExsistingProxies + " ]");

                    if (!string.IsNullOrEmpty(txtPublicProxy.Text))
                    {
                        // lstProxies = BaseLib.GlobusFileHelper.ReadFiletoStringLst(txtPublicProxy.Text);
                        if (lstProxies.Count == 0)
                        {
                            AddToProxysLogs("[ " + DateTime.Now + " ] => [ Please Select a text File With Public Proxies ]");
                            return;
                        }

                        AddToProxysLogs("[ " + DateTime.Now + " ] => [ " + lstProxies.Count() + " Public Proxies Uploaded ]");
                        Proxystatus = lstProxies.Count;

                        new Thread(() =>
                        {
                            GetValidProxies(lstProxies);
                        }).Start();
                    }
                    else
                    {
                        AddToProxysLogs("[ " + DateTime.Now + " ] => [ Please Select a text File With Public Proxies ]");
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
                AddToProxysLogs("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        }

        #endregion

        #region btnClearPublicProxies_Click

        private void btnClearPublicProxies_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {
                try
                {
                    if (MessageBox.Show("Do you really want to delete all the Proxies from Database", "Proxy", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        clsDBQueryManager setting = new clsDBQueryManager();
                        setting.DeletePublicProxyData();
                        AddToProxysLogs("[ " + DateTime.Now + " ] => [ All Public Proxies Deleted from the DataBase ]");
                        workingproxiesCount = 0;
                        lbltotalworkingproxies.Invoke(new MethodInvoker(delegate
                        {
                            lbltotalworkingproxies.Text = "Total Working Proxies : " + workingproxiesCount.ToString();
                        }));
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
                AddToProxysLogs("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        }

        #endregion

        #region btnAsssignPublicProxy_Click

        private void btnAsssignPublicProxy_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {
                try
                {
                    if (LinkedInManager.linkedInDictionary.Count() == 0)
                    {
                        MessageBox.Show("Please Upload Accounts..");
                        return;
                    }

                    if (!string.IsNullOrEmpty(txtPublicProxy.Text))
                    {
                        // if (MessageBox.Show("Assign Public Proxies from Database???", "Proxy", MessageBoxButtons.OKCancel) == DialogResult.Yes)
                        // bool yes = MessageBox.Show("Assign Public Proxies from Database???", "Proxy", MessageBoxButtons.YesNo) == DialogResult.Yes;
                        // if (yes)
                        if (MessageBox.Show("Assign Public Proxies from Database???", "Proxy", MessageBoxButtons.OKCancel) == DialogResult.OK)
                        {
                            try
                            {
                                List<string> lstProxies = proxyFetcher.GetPublicProxies();
                                if (lstProxies.Count > 0)
                                {
                                    if (!string.IsNullOrEmpty(txtAccountsPerProxy.Text) && GlobusRegex.ValidateNumber(txtAccountsPerProxy.Text))
                                    {
                                        accountsPerProxy = int.Parse(txtAccountsPerProxy.Text);
                                    }
                                    proxyFetcher.AssignProxiesToAccounts(lstProxies, accountsPerProxy);//AssignProxiesToAccounts(lstProxies);
                                    ReloadAccountsFromDataBase();
                                    AddToProxysLogs("[ " + DateTime.Now + " ] => [ Proxies Assigned To Accounts ]");
                                }
                                else
                                {
                                    MessageBox.Show("please click on test button or import without testing.");
                                }
                            }
                            catch (Exception ex)
                            {
                                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> txtAsssignPublicProxy_Click  --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_ProxySettingErroLog);
                                GlobusFileHelper.AppendStringToTextfileNewLine("Error --> txtAsssignPublicProxy_Click() --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                            }
                        }
                        // else

                    // bool no=(bool)DialogResult.No;  
                        else
                        {
                            using (OpenFileDialog ofd = new OpenFileDialog())
                            {
                                ofd.Filter = "Text Files (*.txt)|*.txt";
                                ofd.InitialDirectory = Application.StartupPath;
                                if (ofd.ShowDialog() == DialogResult.OK)
                                {
                                    list_pvtProxy = new List<string>();

                                    list_pvtProxy = GlobusFileHelper.ReadFiletoStringList(ofd.FileName);

                                    if (!string.IsNullOrEmpty(txtAccountsPerProxy.Text) && GlobusRegex.ValidateNumber(txtAccountsPerProxy.Text))
                                    {
                                        accountsPerProxy = int.Parse(txtAccountsPerProxy.Text);
                                    }
                                    proxyFetcher.AssignProxiesToAccounts(list_pvtProxy, accountsPerProxy);//AssignProxiesToAccounts(lstProxies);
                                    ReloadAccountsFromDataBase();
                                    AddToProxysLogs("[ " + DateTime.Now + " ] => [ Proxies Assigned To Accounts ]");
                                }
                            }
                        }

                    }
                    else
                    {
                        MessageBox.Show("Please Select Proxy File To Assign Proxy");
                        AddToProxysLogs("[ " + DateTime.Now + " ] => [ Please Select Proxy File To Assign Proxy ]");
                    }
                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> txtAsssignPublicProxy_Click  --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_ProxySettingErroLog);
                    GlobusFileHelper.AppendStringToTextfileNewLine("Error --> txtAsssignPublicProxy_Click() --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                }
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddToProxysLogs("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        }

        #endregion

        #region btnPvtProxyFour_Click

        List<string> pvtProxy = new List<string>();
        List<string> lstValidProxyList = new List<string>();

        private void btnPvtProxyFour_Click(object sender, EventArgs e)
        {
            try
            {
                txtPvtProxyFour.Text = "";
                int IsPublic = 0;
                int Working = 0;
                string LoggedInIp = string.Empty;
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "Text Files (*.txt)|*.txt";
                    ofd.InitialDirectory = Application.StartupPath;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtPvtProxyFour.Text = ofd.FileName;

                        pvtProxy = GlobusFileHelper.ReadFiletoStringList(ofd.FileName);

                        foreach (string Proxylst in pvtProxy)
                        {
                            string account = Proxylst;
                            string proxyAddress = string.Empty;
                            string proxyPort = string.Empty;
                            string proxyUserName = string.Empty;
                            string proxyPassword = string.Empty;
                            int DataCount = account.Split(':').Length;

                            using (SQLiteConnection con = new SQLiteConnection(DataBaseHandler.CONstr))
                            {
                                //using (SQLiteDataAdapter ad = new SQLiteDataAdapter("SELECT * FROM tb_FBAccount WHERE ProxyAddress = '" + proxyAddress + "'", con))
                                using (SQLiteDataAdapter ad = new SQLiteDataAdapter())
                                {
                                    if (DataCount == 4)
                                    {
                                        lstValidProxyList.Add(Proxylst);
                                        string[] Data = account.Split(':');
                                        proxyAddress = Data[0];
                                        proxyPort = Data[1];
                                        proxyUserName = Data[2];
                                        proxyPassword = Data[3];
                                        LoggedInIp = "NoIP";
                                        IsPublic = 1;
                                        Working = 1;
                                        string InsertQuery = "Insert into tb_Proxies values('" + proxyAddress + "','" + proxyPort + "','" + proxyUserName + "','" + proxyPassword + "', " + Working + "," + IsPublic + " , '" + LoggedInIp + "')";
                                        DataBaseHandler.InsertQuery(InsertQuery, "tb_Proxies");
                                    }
                                    else
                                    {
                                        AddToProxysLogs("[ " + DateTime.Now + " ] => [ Only Private Proxies allowed using this option ]");
                                    }
                                }
                            }
                        }
                        AddToProxysLogs("[ " + DateTime.Now + " ] => [ " + lstValidProxyList.Count() + " Private Proxies File Uploaded ]");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error >>> " + ex.StackTrace);
            }
        }

        #endregion

        #region btnAssignProxy_Click

        private void btnAssignProxy_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {

                try
                {
                    if (LinkedInManager.linkedInDictionary.Count() == 0)
                    {
                        MessageBox.Show("Please Upload Accounts..");
                        return;
                    }

                    if (!string.IsNullOrEmpty(txtPvtProxyFour.Text))
                    {
                        if (MessageBox.Show("Assign Private Proxies from Database???", "Proxy", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            try
                            {
                                List<string> lstProxies = proxyFetcher.GetPrivateProxies();
                                if (lstProxies.Count > 0)
                                {
                                    if (!string.IsNullOrEmpty(txtAccountsPerProxy.Text) && GlobusRegex.ValidateNumber(txtAccountsPerProxy.Text))
                                    {
                                        accountsPerProxy = int.Parse(txtAccountsPerProxy.Text);
                                    }
                                    proxyFetcher.AssignProxiesToAccounts(lstProxies, accountsPerProxy);//AssignProxiesToAccounts(lstProxies);
                                    ReloadAccountsFromDataBase();
                                    AddToProxysLogs("[ " + DateTime.Now + " ] => [ Proxies Assigned To Accounts ]");
                                }
                                else
                                {
                                    MessageBox.Show("Please assign private proxies from Proxies Tab in Main Page OR Upload a proxies Text File");
                                }
                            }
                            catch (Exception ex)
                            {
                                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> btnAssignProxy_Click() --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_ProxySettingErroLog);
                                GlobusFileHelper.AppendStringToTextfileNewLine("Error --> btnAssignProxy_Click() --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                            }
                        }
                        else
                        {
                            using (OpenFileDialog ofd = new OpenFileDialog())
                            {
                                ofd.Filter = "Text Files (*.txt)|*.txt";
                                ofd.InitialDirectory = Application.StartupPath;
                                if (ofd.ShowDialog() == DialogResult.OK)
                                {
                                    list_pvtProxy = new List<string>();
                                    list_pvtProxy = GlobusFileHelper.ReadFiletoStringList(ofd.FileName);

                                    if (!string.IsNullOrEmpty(txtAccountsPerProxy.Text) && GlobusRegex.ValidateNumber(txtAccountsPerProxy.Text))
                                    {
                                        accountsPerProxy = int.Parse(txtAccountsPerProxy.Text);
                                    }
                                    proxyFetcher.AssignProxiesToAccounts(list_pvtProxy, accountsPerProxy);//AssignProxiesToAccounts(lstProxies);
                                    ReloadAccountsFromDataBase();
                                    AddToProxysLogs("[ " + DateTime.Now + " ] => [ Proxies Assigned To Accounts ]");
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please Select Proxy File To Assign Proxy");
                        AddToProxysLogs("[ " + DateTime.Now + " ] => [ Please Select Proxy File To Assign Proxy ]");
                    }
                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> btnAssignProxy_Click() --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_ProxySettingErroLog);
                    GlobusFileHelper.AppendStringToTextfileNewLine("Error --> btnAssignProxy_Click() --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                }
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddToProxysLogs("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        }

        #endregion

        #region btnClearPrivateProxies_Click

        private void btnClearPrivateProxies_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {
                try
                {
                    if (MessageBox.Show("Do you really want to delete all the Private Proxies from Database???", "Proxy", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        clsDBQueryManager setting = new clsDBQueryManager();
                        setting.DeletePrivateProxyData();
                        workingproxiesCount = 0;
                        AddToProxysLogs("[ " + DateTime.Now + " ] => [ All Private Proxies Deleted from the DataBase ]");
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
                AddToProxysLogs("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        }

        #endregion

        #region ImportingProxy

        public void ImportingProxy(string item)
        {
            if (proxy_stop)
            {
                return;
            }
            try
            {
                lstStopPublicProxyTest.Add(Thread.CurrentThread);
                lstStopPublicProxyTest = lstStopPublicProxyTest.Distinct().ToList();
                Thread.CurrentThread.IsBackground = true;
            }
            catch { }

            string proxyAddress = string.Empty;
            string proxyPort = string.Empty;
            string proxyUsername = string.Empty;
            string proxyPassword = string.Empty;
            int Working = 0;
            int IsPublic = 0;
            string LoggedInIp = string.Empty;
            string account = item;
            int DataCount = account.Split(':').Length;

            if (DataCount == 1)
            {
                proxyAddress = account.Split(':')[0];
                AddToProxysLogs("[ " + DateTime.Now + " ] => [ Proxy Not In correct Format ]");
                AddToProxysLogs(account);
                return;
            }
            if (DataCount == 2)
            {
                proxyAddress = account.Split(':')[0];
                proxyPort = account.Split(':')[1];
            }
            else if (DataCount > 2)
            {
                AddToProxysLogs("[ " + DateTime.Now + " ] => [ Proxy Not In correct Format ]");
                AddToProxysLogs(account);
                return;
            }
            try
            {
                if (!string.IsNullOrEmpty(txtAccountsPerProxy.Text) && GlobusRegex.ValidateNumber(txtAccountsPerProxy.Text))
                {
                    accountsPerProxy = int.Parse(txtAccountsPerProxy.Text);
                }
                proxyFetcher.AssignProxiesToAccounts(lstPublicProxyWOTest, accountsPerProxy);

                using (SQLiteConnection con = new SQLiteConnection(DataBaseHandler.CONstr))
                {
                    using (SQLiteDataAdapter ad = new SQLiteDataAdapter())
                    {
                        AddToProxysLogs("[ " + DateTime.Now + " ] => [ Added Proxy -> " + proxyAddress + ":" + proxyPort + " ]");
                        string InsertQuery = "Insert into tb_Proxies values('" + proxyAddress + "','" + proxyPort + "','" + proxyUsername + "','" + proxyPassword + "', " + Working + "," + IsPublic + " , '" + LoggedInIp + "')";
                        DataBaseHandler.InsertQuery(InsertQuery, "tb_Proxies");
                        GlobusFileHelper.AppendStringToTextfileNewLine(proxyAddress + ":" + proxyPort, Globals.Path_ExsistingProxies);
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("Error Importing Public Proxy W/o testing --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
            }
        }

        #endregion

        #region chkboxImportPublicProxy_CheckedChanged

        private void chkboxImportPublicProxy_CheckedChanged(object sender, EventArgs e)
        {
            if (chkboxImportPublicProxy.Checked)
            {
                if (!string.IsNullOrEmpty(txtPublicProxy.Text.Replace("\0", "").Replace(" ", "")))
                {
                    lstPublicProxyWOTest = GlobusFileHelper.ReadFiletoStringList(txtPublicProxy.Text);
                    AddToProxysLogs("[ " + DateTime.Now + " ] => [ " + lstPublicProxyWOTest.Count + " Public Proxies Loaded ]");
                    if (lstPublicProxyWOTest.Count > 0)
                    {
                        new Thread(() =>
                        {
                            foreach (string item in lstPublicProxyWOTest)
                            {
                                ImportingProxy(item);
                            }
                        }).Start();
                    }
                    else
                    {
                        AddToProxysLogs("[ " + DateTime.Now + " ] => [ Sorry No Proxies Available ]");
                    }
                }
                else
                {
                    AddToProxysLogs("[ " + DateTime.Now + " ] => [ Please Select File To Get Data Imported ]");
                }
            }
        }

        #endregion

        //>>>>>>>>>>>>>>>>>>>>>> Image Painting code for controls >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

        #region tabMai_DrawItem

        private void tabMai_DrawItem(object sender, DrawItemEventArgs e)
        {
            try
            {
                Graphics g = e.Graphics;
                Brush _TextBrush;

                // Get the item from the collection.
                TabPage _TabPage = tabMain.TabPages[e.Index];

                // Get the real bounds for the tab rectangle.
                Rectangle _TabBounds = tabMain.GetTabRect(e.Index);

                if (e.State == DrawItemState.Selected)
                {
                    // Draw a different background color, and don't paint a focus rectangle.
                    Brush background_brush1 = new SolidBrush(Color.FromArgb(164,49,21));//(95, 181, 232));
                    _TextBrush = new SolidBrush(Color.White);
                    g.FillRectangle(background_brush1, e.Bounds);
                }
                else
                {
                    _TextBrush = new System.Drawing.SolidBrush(Color.Brown);
                    g.FillRectangle(Brushes.Brown, e.Bounds);
                    e.DrawBackground();
                }

                // Use our own font. Because we CAN.
                Font _TabFont = new Font("Verdana", 12, FontStyle.Bold, GraphicsUnit.Pixel);


                //Set On tab For hiding Side Space of Tab button....

                Brush background_brush = new SolidBrush(Color.FromArgb(86, 137, 194));

                Rectangle LastTabRect = tabMain.GetTabRect(tabMain.TabPages.Count - 1);

                Rectangle rect = new Rectangle();

                rect.Location = new Point(0, LastTabRect.Bottom + 3);

                rect.Size = new Size(_TabPage.Right - (_TabPage.Width + 3), _TabPage.Height);

                //e.Graphics.FillRectangle(background_brush, rect);
                e.Graphics.DrawImage(recSideImage, rect);


                // Draw string. Center the text.
                StringFormat _StringFlags = new StringFormat();
                _StringFlags.Alignment = StringAlignment.Center;
                _StringFlags.LineAlignment = StringAlignment.Center;
                g.DrawString(_TabPage.Text, _TabFont, _TextBrush,
                _TabBounds, new StringFormat(_StringFlags));
            }
            catch { }           

        }

        #endregion

        #region frmMain_Paint

        private void frmMain_Paint(object sender, PaintEventArgs e)
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
        
        #region LinkedInpreScraper_Paint

        private void LinkedInpreScraper_Paint(object sender, PaintEventArgs e)
        {
            Graphics g;
            g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.DrawImage(image, 0, 0, LinkedInpreScraper.Width, LinkedInpreScraper.Height);
        }

        #endregion

        #region TabStatus_Paint

        private void TabStatus_Paint(object sender, PaintEventArgs e)
        {
            Graphics g;
            g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.DrawImage(image, 0, 0, TabStatus.Width, TabStatus.Height);
        }

        #endregion

        #region tabAddConnection_Paint

        private void tabAddConnection_Paint(object sender, PaintEventArgs e)
        {
            Graphics g;
            g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.DrawImage(image, 0, 0, tabAddConnection.Width, tabAddConnection.Height);
        }

        #endregion

        #region tabPageCreateGrp_Paint

        private void tabPageCreateGrp_Paint(object sender, PaintEventArgs e)
        {
            Graphics g;
            g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.DrawImage(image, 0, 0, tabPageCreateGrp.Width, tabPageCreateGrp.Height);
        }

        #endregion

        #region tabAddGroup_Paint

        private void tabAddGroup_Paint(object sender, PaintEventArgs e)
        {
            Graphics g;
            g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.DrawImage(image, 0, 0, tabAddGroup.Width, tabAddGroup.Height);
        }

        #endregion

        #region tabPageAddGroup_Paint

        private void tabPageAddGroup_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                Graphics g;
                g = e.Graphics;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.DrawImage(image, 0, 0, tabPageAddGroup.Width, tabPageAddGroup.Height);
            }
            catch { }
        }

        #endregion

        #region tabPageGroupUpdate_Paint

        private void tabPageGroupUpdate_Paint(object sender, PaintEventArgs e)
        {
            Graphics g;
            g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.DrawImage(image, 0, 0, tabPageGroupUpdate.Width, tabPageGroupUpdate.Height);
        }

        #endregion

        #region tabComposeMessage_Paint

        private void tabComposeMessage_Paint(object sender, PaintEventArgs e)
        {
            Graphics g;
            g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.DrawImage(image, 0, 0, tabComposeMessage.Width, tabComposeMessage.Height);
        }

        #endregion

        #region tabProxySetting_Paint

        private void tabProxySetting_Paint(object sender, PaintEventArgs e)
        {
            Graphics g;
            g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.DrawImage(image, 0, 0, tabProxySetting.Width, tabProxySetting.Height);
        }

        #endregion

        #region splitContainer1_Panel1_Paint

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g;

            g = e.Graphics;

            g.SmoothingMode = SmoothingMode.HighQuality;

            g.DrawImage(image, 0, 0, splitContainer1.Width, splitContainer1.Height);
        }

        #endregion

        #region tabMai_SelectedIndexChanged

        private void tabMai_SelectedIndexChanged(object sender, EventArgs e)
        {

            //if (tabMain.SelectedTab.Name == "tabPageAccountCreator")
            //{
            //    GroupStatus.ManageTabGroupStatus = false;
            //    pictureBoxHeader.Image = InBoardPro.Properties.Resources.Account_creator;
            //}
            if (tabMain.SelectedTab.Name == "LinkedinSearch")
            {
                try
                {
                    GroupStatus.ManageTabGroupStatus = false;
                    pictureBoxHeader.Image = Properties.Resources.linkedin_search;

                    // Thread for bind data in cmbLinkedin Search Email Id

                    //Thread obj_BindData_LinkedinSearch_cmbEmailId = new Thread(BindData_LinkedinSearch_cmbEmailId);
                    //obj_BindData_LinkedinSearch_cmbEmailId.Start();
                    Thread obj_BindData_LinkedinSearch_cmbEmailId = new Thread(BindData_LinkedinSearch_cmbEmailId);
                    obj_BindData_LinkedinSearch_cmbEmailId.Start(new object[] { "LinkedinSearch", cmbLinkedinSelectEmailId });
                    txtExportedFile1.Text = Globals.DesktopFolder;

                }
                catch
                {
                }
            }
            else if (tabMain.SelectedTab.Name == "LinkedInpreScraper")
            {
                GroupStatus.ManageTabGroupStatus = false;
                pictureBoxHeader.Image = Properties.Resources.linkedin_prescrapper;

                Thread obj_BindData_LinkedinSearch_cmbEmailId = new Thread(BindData_LinkedinSearch_cmbEmailId);
                obj_BindData_LinkedinSearch_cmbEmailId.Start(new object[] { "LinkedInpreScraper", comboBoxemail });


                if (SearchCriteria.loginREsponce.Contains("premium-member"))
                {

                    txtScraperMinDelay.Visible = true;
                    checkedListBoxFortune1000.Enabled = true;
                    checkedListBoxCompanySize.Enabled = true;
                    checkedListBoxInterestedIN.Enabled = true;
                    checkedListBoxSeniorlevel.Enabled = true;
                    checkedListBoxRecentlyJoined.Enabled = true;
                    GetDataForPrimiumAccount.Enabled = true;
                    checkedListFunction.Enabled = true;

                }

            }
            else if (tabMain.SelectedTab.Name == "tabViewProfileRank")
            {
                GroupStatus.ManageTabGroupStatus = false;
                pictureBoxHeader.Image = Properties.Resources.Profile_Ranking;

                Thread obj_BindData_LinkedinSearch_cmbEmailId = new Thread(BindData_LinkedinSearch_cmbEmailId);
                obj_BindData_LinkedinSearch_cmbEmailId.Start(new object[] { "LinkedInpreScraper", cmbSelAccount });

            }
            else if (tabMain.SelectedTab.Name == "tabShare")
            {
                GroupStatus.ManageTabGroupStatus = false;
                //pictureBoxHeader.Image = Properties.Resources.Share_Link;

                Thread obj_BindData_LinkedinSearch_cmbEmailId = new Thread(BindData_LinkedinSearch_cmbEmailId);
                obj_BindData_LinkedinSearch_cmbEmailId.Start(new object[] { "LinkedInpreScraper", cmbSelAccountShare });
            }

            else if (tabMain.SelectedTab.Name == "TabStatus")
            {
                GroupStatus.ManageTabGroupStatus = false;
                pictureBoxHeader.Image = Properties.Resources.Status_Update;
            }
            else if (tabMain.SelectedTab.Name == "tabAddConnection")
            {
                GroupStatus.ManageTabGroupStatus = false;
                pictureBoxHeader.Image = Properties.Resources.add_connection;
            }
            else if (tabMain.SelectedTab.Name == "tabPageCreateGrp")
            {
                GroupStatus.ManageTabGroupStatus = false;
                pictureBoxHeader.Image = Properties.Resources.create_group;
            }
            else if (tabMain.SelectedTab.Name == "tabAddGroup")
            {
                GroupStatus.ManageTabGroupStatus = false;
                pictureBoxHeader.Image = Properties.Resources.join_friend_group;

                Thread obj_BindData_LinkedinSearch_cmbEmailId = new Thread(BindData_LinkedinSearch_cmbEmailId);
                obj_BindData_LinkedinSearch_cmbEmailId.Start(new object[] { "tabAddGroup", cmbUser });
            }
            else if (tabMain.SelectedTab.Name == "tabPageAddGroup")
            {
                GroupStatus.ManageTabGroupStatus = false;
                SearchGroup = "";
                pictureBoxHeader.Image = Properties.Resources.join_search_group;

                Thread obj_BindData_LinkedinSearch_cmbEmailId = new Thread(BindData_LinkedinSearch_cmbEmailId);
                obj_BindData_LinkedinSearch_cmbEmailId.Start(new object[] { "tabPageAddGroup", cmbSearchGroup });
            }
            else if (tabMain.SelectedTab.Name == "tabPageGroupUpdate")
            {
                GroupStatus.ManageTabGroupStatus = false;

                pictureBoxHeader.Image = Properties.Resources.group_update;

                Thread obj_BindData_LinkedinSearch_cmbEmailId = new Thread(BindData_LinkedinSearch_cmbEmailId);
                obj_BindData_LinkedinSearch_cmbEmailId.Start(new object[] { "tabPageGroupUpdate", cmbGroupUser });
            }
            else if (tabMain.SelectedTab.Name == "tabComposeMessage")
            {
                GroupStatus.ManageTabGroupStatus = false;

                pictureBoxHeader.Image = Properties.Resources.compose_message;

                Thread obj_BindData_LinkedinSearch_cmbEmailId = new Thread(BindData_LinkedinSearch_cmbEmailId);
                obj_BindData_LinkedinSearch_cmbEmailId.Start(new object[] { "tabComposeMessage", cmbMsgFrom });
            }
            else if (tabMain.SelectedTab.Name == "tabProxySetting")
            {
                GroupStatus.ManageTabGroupStatus = false;
                pictureBoxHeader.Image = Properties.Resources.proxy_setting;
            }
            else if (tabMain.SelectedTab.Name == "tabMsgGroupMember")
            {
                pictureBoxHeader.Image = Properties.Resources.msg_group_member;

                Thread obj_BindData_LinkedinSearch_cmbEmailId = new Thread(BindData_LinkedinSearch_cmbEmailId);
                obj_BindData_LinkedinSearch_cmbEmailId.Start(new object[] { "tabMsgGroupMember", cmbAllUser });
            }
            else if (tabMain.SelectedTab.Name == "tabEndorse")
            {
                GroupStatus.ManageTabGroupStatus = false;
                pictureBoxHeader.Image = Properties.Resources.EndorseYourProfile;
            }

        }

        #endregion

        #region BindData_LinkedinSearch_cmbEmailId

        private void BindData_LinkedinSearch_cmbEmailId(object parameters)
        {
            try
            {
                try
                {
                    Array paramsArray = new object[2];
                    paramsArray = (Array)parameters;

                    string strTbtoSelect = (string)paramsArray.GetValue(0);
                    ComboBox cmo = (ComboBox)paramsArray.GetValue(1);

                    if (LinkedInManager.linkedInDictionary.Count() == 0)
                    {
                        this.Invoke(new MethodInvoker(delegate
                        {
                            lbGeneralLogs.Items.Clear();
                            frmAccounts FrmAccount = new frmAccounts();

                            FrmAccount.Show();

                            //tabMain.SelectTab("LinkedinSearch");
                            //tabMain.SelectedTab.Show();



                            // This condition is failed if there is No Accounts Uploaded in LinkedIn Accounts !
                            //tabMai.SelectTab(strTbtoSelect);
                            //tabMai.SelectedTab.Show();


                            return;
                        }));
                    }
                    else
                    {
                        try
                        {
                            //Thread obj_PopulateCmo = new Thread(PopulateCmo);
                            //obj_PopulateCmo.Start();
                            PopulateCmo();
                            BindData_LinkedinSearch(cmo);
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
            catch
            {
            }
        }

        #endregion

        #region BindData_LinkedinSearch

        private void BindData_LinkedinSearch()
        {
            try
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    cmbLinkedinSelectEmailId.Items.Clear();
                    foreach (KeyValuePair<string, LinkedInMaster> item in LinkedInManager.linkedInDictionary)
                    {
                        cmbLinkedinSelectEmailId.Items.Add(item.Key);
                        comboBoxemail.Items.Add(item.Key);
                    }
                    cmbLinkedinSelectEmailId.SelectedItem = cmbLinkedinSelectEmailId.Items[0];
                    comboBoxemail.SelectedItem = comboBoxemail.Items[0];
                    //cmbLinkedinSearch.SelectedItem = cmbLinkedinSearch.Items[0];

                }));

                //AddLoggerLinkedinSearch("[ " + DateTime.Now + " ] => [ Accounts Uploaded.. ]");
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> PopulateCmo() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> PopulateCmo() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPreScrapperErrorLogs);
            }
        }

        private void BindData_LinkedinSearch(ComboBox cmo)
        {
            try
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    try
                    {
                        cmo.Items.Clear();
                        foreach (KeyValuePair<string, LinkedInMaster> item in LinkedInManager.linkedInDictionary)
                        {
                            cmo.Items.Add(item.Key);
                        }
                        cmo.SelectedItem = cmo.Items[0];
                        cmo.SelectedItem = cmo.Items[0];
                    }
                    catch { }
                }));

                //AddLoggerLinkedinSearch("Accounts Uploaded..");
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> PopulateCmo() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> PopulateCmo() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPreScrapperErrorLogs);
            }
        }

        #endregion


        #region tabMsgGroupMember_Paint

        private void tabMsgGroupMember_Paint(object sender, PaintEventArgs e)
        {
            Graphics g;
            g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.DrawImage(image, 0, 0, tabMsgGroupMember.Width, tabProxySetting.Height);
        }

        #endregion

        #region tabPagEndorse_Paint

        private void tabEndorse_Paint(object sender, PaintEventArgs e)
        {
            Graphics g;

            g = e.Graphics;

            g.SmoothingMode = SmoothingMode.HighQuality;

            g.DrawImage(image, 0, 0, tabPageGroupUpdate.Width, tabPageGroupUpdate.Height);
        }


        #endregion

        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>Specified Group Member Update >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

        #region btnGetGroup_Click

        Dictionary<string, Dictionary<string, string>> GrpMemMess = new Dictionary<string, Dictionary<string, string>>();

        string cmbSelectedUser_MessageGroupMember = string.Empty;

        private void btnGetGroup_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {

                try
                {
                    GroupStatus.ManageTabGroupStatus = true;

                    try
                    {
                        cmbSelectedUser_MessageGroupMember = cmbAllUser.SelectedItem.ToString();
                    }
                    catch (Exception ex)
                    {

                    }

                    lstGroupMessageThread.Clear();
                    if (IsStop)
                    {
                        IsStop = false;
                    }

                    if (LinkedInManager.linkedInDictionary.Count() == 0)
                    {
                        MessageBox.Show("Please Load LinkedIn Accounts From MyAccounts Menu");
                        AddLoggerLinkedingrp("[ " + DateTime.Now + " ] => [ Please Load LinkedIn Accounts From MyAccounts Menu ]");
                        lbGeneralLogs.Items.Clear();
                        frmAccounts FrmAccount = new frmAccounts();
                        FrmAccount.Show();
                        return;
                    }

                    GrpMemMess.Clear();
                    lstGrpMemMsgLoger.Items.Clear();
                    chkGroupCollection.Items.Clear();

                    AddLoggerLinkedingrp("[ " + DateTime.Now + " ] => [ Start Process... ]");
                    btnGetGroup.Cursor = Cursors.AppStarting;

                    new Thread(() =>
                    {
                        LinkdinGroupMemberUpdate();

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
                AddLoggerLinkedingrp("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        }

        #endregion

        #region LinkdinGroupUpdate()

        private void LinkdinGroupMemberUpdate()
        {
            try
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

                            ThreadPool.QueueUserWorkItem(new WaitCallback(StartDMMultiThreadedGroupMemberUser), new object[] { item });

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
                    AddLoggerLinkedingrp("[ " + DateTime.Now + " ] => [ Please Load LinkedIn Accounts From MyAccounts Menu ]");
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update --> LinkdinGroupUpdate() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update --> LinkdinGroupUpdate() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinGetGroupMemberErrorLogs);
            }
        }

        #endregion

        #region StartDMMultiThreadedGroupMemberUser

        public void StartDMMultiThreadedGroupMemberUser(object parameter)
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
                        lstGroupMessageThread.Add(Thread.CurrentThread);
                        lstGroupMessageThread.Distinct().ToList();
                        Thread.CurrentThread.IsBackground = true;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error >>> " + ex.StackTrace);
                }
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

                Login.logger.addToLogger += new EventHandler(LinkedingrpLogEvents_addToLogger);


                if (!Login.IsLoggedIn)
                {
                    Login.LoginHttpHelper(ref HttpHelper);
                }

                try
                {
                    if (Login.IsLoggedIn)
                    {
                        GroupStatus dataScrape = new GroupStatus();
                        Thread.Sleep(2000);
                        Dictionary<string, string> Data = dataScrape.PostCreateGroupNames(ref HttpHelper, Login.accountUser);

                        GrpMemMess.Add(Login.accountUser, Data);

                        if (cmbAllUser.InvokeRequired)
                        {
                            new Thread(() =>
                            {
                                cmbAllUser.Invoke(new MethodInvoker(delegate
                                {
                                    if (!cmbAllUser.Items.Contains(Login.accountUser))
                                    {
                                        cmbAllUser.Items.Add(Login.accountUser);
                                    }
                                }));
                            }).Start();
                        }
                    }
                    else
                    {
                        //AddLoggerGroupMemMessage("LinkedIn account : " + Login.accountUser + " has been temporarily restricted");
                        //AddLoggerLinkedingrp("[ " + DateTime.Now + " ] => [ LinkedIn account : " + Login.accountUser + " has been temporarily restricted ]");
                    }
                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> frmmain() --> StartDMMultiThreadedGroupMemberUser() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> frmmain() --> StartDMMultiThreadedGroupMemberUser() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinGetGroupMemberErrorLogs);
                }

                finally
                {
                    counter_GroupMemberSearch--;

                    if (counter_GroupMemberSearch == 0)
                    {
                        if (cmbAllUser.InvokeRequired)
                        {
                            cmbAllUser.Invoke(new MethodInvoker(delegate
                            {
                                cmbAllUser.Enabled = true;
                                AddLoggerLinkedingrp("[ " + DateTime.Now + " ] => [ PROCESS COMPLETE..Please select Account ]");
                                AddLoggerLinkedingrp("----------------------------------------------------------------------------------------------------------------------------------------");
                                btnGetGroup.Cursor = Cursors.Default;
                            }));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error >>> " + ex.StackTrace);
            }
        }

        #endregion

        #region cmbAllUser_SelectedIndexChanged
        string GetUserIDRepeat = string.Empty;
        private void cmbAllUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GroupStatus.ManageTabGroupStatus == true)
            {
                cmbMemberGroup.Items.Clear();
                GroupStatus.GroupMemUrl.Clear();

                try
                {
                    string GetUserID = cmbAllUser.SelectedItem.ToString();
                    label47.Text = cmbAllUser.SelectedItem.ToString();

                    foreach (KeyValuePair<string, Dictionary<string, string>> item in GrpMemMess)
                    {
                        if (GetUserID.Contains(item.Key))
                        {
                            List<string> GmUserIDs = new List<string>();
                            foreach (KeyValuePair<string, string> item1 in item.Value)
                            {
                                string group = item1.Key;
                                if (!string.IsNullOrEmpty(group))
                                {
                                    string[] group1 = group.Split(':');

                                    if (GetUserID == group1[1].ToString())
                                    {
                                        cmbMemberGroup.Items.Add(group1[1] + ':' + group1[0].ToString());
                                        GroupStatus.GroupMemUrl.Add(item1.Key + ":" + item1.Value);
                                    }
                                }

                            }

                            AddLoggerLinkedingrp("[ " + DateTime.Now + " ] => [ Finished Adding Groups of Usernames  Please Select User Groups..]");
                            AddLoggerLinkedingrp("----------------------------------------------------------------------------------------------------------------------------------------");
                        }
                    }

                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update --> cmbGroupUser_SelectedIndexChanged() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update --> cmbGroupUser_SelectedIndexChanged() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinGetGroupMemberErrorLogs);
                }

                GroupStatus.ManageTabGroupStatus = true;
            }
            GroupStatus.ManageTabGroupStatus = true;
        }

        private void cmbAllUser_MessageGroupMember(string accountUser)
        {
            try
            {
                GroupStatus.GroupMemUrl.Clear();
                try
                {
                    string GetUserID = accountUser;//cmbAllUser.SelectedItem.ToString();
                    label47.Text = accountUser; //cmbAllUser.SelectedItem.ToString();
                    foreach (KeyValuePair<string, Dictionary<string, string>> item in GrpMemMess)
                    {
                        if (GetUserID.Contains(item.Key))
                        {
                            List<string> GmUserIDs = new List<string>();
                            foreach (KeyValuePair<string, string> item1 in item.Value)
                            {
                                string group = item1.Key;
                                string[] group1 = group.Split(':');

                                if (GetUserID == group1[1].ToString())
                                {
                                    cmbMemberGroup.Items.Add(group1[1] + ':' + group1[0].ToString());
                                    GroupStatus.GroupMemUrl.Add(item1.Key + ":" + item1.Value);
                                }
                            }
                        }
                    }

                    AddLoggerLinkedingrp("[ " + DateTime.Now + " ] => [ " + GroupStatus.GroupMemUrl.Count() + " Groups List of :" + "" + GetUserID + " ]");
                    AddLoggerLinkedingrp("[ " + DateTime.Now + " ] => [ Finished Adding Groups of Usernames ]");

                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update --> cmbGroupUser_SelectedIndexChanged() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update --> cmbGroupUser_SelectedIndexChanged() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinGetGroupMemberErrorLogs);
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update --> cmbGroupUser_SelectedIndexChanged() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update --> cmbGroupUser_SelectedIndexChanged() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinGetGroupMemberErrorLogs);

            }
        }


        #endregion

        #region cmbMemberGroup_SelectedIndexChanged

        string selectedSpeUser = string.Empty;

        private void cmbMemberGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                AddLoggerLinkedingrp("[ " + DateTime.Now + " ] => [ Please click get members button ]");
                selectedSpeUser = cmbMemberGroup.SelectedItem.ToString();
                chkListGroupMembers.Items.Clear();
                txtPages.Text = "";

            }
            catch
            {
            }

            //cmbMemberGroup.Cursor = Cursors.AppStarting;

            //try
            //{
            //    new Thread(() =>
            //    {
            //        LinkedAddSpecifiedGroupPages();
            //    }).Start();
            //}
            //catch
            //{
            //}


        }

        #endregion

        private void chkAllAcounts_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAllAcounts.Checked == true)
            {
                GroupStatus.selectAllGroup = true;
            }
            else
            {
                GroupStatus.selectAllGroup = true;
            }
        }


        #region LinkedAddSpecifiedGroupMem()

        string SelGrpUserFPages = string.Empty;
        string SpeGroupIdFPages = string.Empty;

        private void LinkedAddSpecifiedGroupPages()
        {
            try
            {
                if (LinkedInManager.linkedInDictionary.Count() > 0)
                {
                    string[] SelUser = selectedSpeUser.Split(':');

                    foreach (var item1 in GroupStatus.GroupMemUrl)
                    {
                        string[] grp = item1.Split(':');
                        if (SelUser[1] == grp[0])
                        {
                            SpeGroupIdFPages = grp[2];
                            break;
                        }
                    }

                    foreach (KeyValuePair<string, LinkedInMaster> item in LinkedInManager.linkedInDictionary)
                    {
                        if (SelUser[0] == item.Key)
                        {
                            StartCrawlSpecificGroupUserPages(new object[] { item });
                            break;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("[ " + DateTime.Now + " ] => [ Please Load LinkedIn Accounts From MyAccounts Menu ]");
                    //AddLoggerGroupMemMessage("Please Load LinkedIn Accounts From MyAccounts Menu");
                    AddLoggerLinkedingrp("[ " + DateTime.Now + " ] => [ Please Load LinkedIn Accounts From MyAccounts Menu ]");
                }
            }
            catch
            {
            }
            finally
            {
                if (cmbMemberGroup.InvokeRequired)
                {
                    cmbMemberGroup.Invoke(new MethodInvoker(delegate
                    {
                        AddLoggerGroupMemMessage("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                        AddLoggerGroupMemMessage("--------------------------------------------------------------------------------------------------------------------------");

                    }));
                }
            }
        }

        #endregion

        #region StartDMMultiThreadedGroupMemberUser

        string GroupPages;

        public void StartCrawlSpecificGroupUserPages(object parameter)
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

            if (!Login.IsLoggedIn)
            {
                Login.LoginHttpHelper(ref HttpHelper);
            }

            try
            {
                if (Login.IsLoggedIn)
                {
                    GroupStatus dataScrape = new GroupStatus();
                    GroupPages = dataScrape.PageNoSpecGroup(ref HttpHelper, SpeGroupIdFPages);

                    if (txtPages.InvokeRequired)
                    {
                        new Thread(() =>
                        {
                            txtPages.Invoke(new MethodInvoker(delegate
                            {
                                txtPages.Text = GroupPages;
                            }));
                        }).Start();
                    }

                    //AddLoggerGroupMemMessage("Loggin In With Email : " + item.Value._Username);
                    AddLoggerLinkedingrp("[ " + DateTime.Now + " ] => [ Loggin In With Email : " + item.Value._Username + " ]");
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
                    if (cmbAllUser.InvokeRequired)
                    {
                        cmbAllUser.Invoke(new MethodInvoker(delegate
                        {
                            AddLoggerLinkedingrp("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                            AddLoggerLinkedingrp("--------------------------------------------------------------------------------------------------------------------------");
                            cmbAllUser.Enabled = true;
                            cmbMemberGroup.Cursor = Cursors.Default;

                        }));
                    }
                }
            }
        }

        #endregion

        #region btnAddMember_Click

        Dictionary<string, Dictionary<string, string>> GrpMemNameWithGid = new Dictionary<string, Dictionary<string, string>>();
        Dictionary<string, string> GroupMemData = new Dictionary<string, string>();

        private void btnAddMember_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {
                try
                {
                    lstGroupMessageThread.Clear();
                    if (IsStop)
                    {
                        IsStop = false;
                    }

                    if (LinkedInManager.linkedInDictionary.Count() == 0)
                    {
                        return;
                    }
                    else if (cmbAllUser.Items.Count == 0)
                    {
                        MessageBox.Show("Please click Get Group button..");
                        return;
                    }
                    if (selectedSpeUser == string.Empty)
                    {

                        if (chkAllAcounts.Checked == false)
                        {
                            MessageBox.Show("Please select any User Group");
                            return;

                        }
                    }

                    chkListGroupMembers.Items.Clear();
                    GroupMemData.Clear();
                    GrpMemNameWithGid.Clear();
                    btnAddMember.Cursor = Cursors.AppStarting;
                    lblTotMemList.Text = "(" + "0" + ")";
                    GroupStatus.SearchKeyword = txtSearchKey.Text.ToString();
                    try
                    {
                        new Thread(() =>
                        {
                            LinkedAddSpecifiedGroupMem();

                        }).Start();
                    }
                    catch
                    {
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
                AddLoggerLinkedingrp("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        }

        #endregion

        #region LinkedAddSpecifiedGroupMem()

        string SelGrpUser = string.Empty;
        string SpeGroupId = string.Empty;
        string Specgroupdetails = string.Empty;

        private void LinkedAddSpecifiedGroupMem()
        {
            AddLoggerLinkedingrp("[ " + DateTime.Now + " ] => [ Please wait...Group Members have being added ]");

            try
            {
                if (LinkedInManager.linkedInDictionary.Count() > 0)
                {
                    string[] SelUser = new string[] { };

                    try
                    {
                        SelUser = selectedSpeUser.Split(':');
                    }
                    catch { }

                    foreach (var item1 in GroupStatus.GroupMemUrl)
                    {
                        try
                        {
                            string[] grp = item1.Split(':');
                            if (SelUser[1] == grp[0])
                            {
                                Specgroupdetails = item1;
                                SpeGroupId = grp[2];
                                break;
                            }
                        }
                        catch { }
                    }

                    foreach (KeyValuePair<string, LinkedInMaster> item in LinkedInManager.linkedInDictionary)
                    {
                        try
                        {
                            string selUser = string.Empty;

                            cmbAllUser.Invoke(new MethodInvoker(delegate
                            {
                                selUser = cmbAllUser.SelectedItem.ToString();
                            }));


                            if (chkAllAcounts.Checked)
                            {
                                if (selUser == item.Key)
                                {
                                    StartCrawlSpecificGroupUser(new object[] { item });
                                    break;
                                }
                            }
                            else
                            {

                                if (SelUser[0] == item.Key)
                                {
                                    StartCrawlSpecificGroupUser(new object[] { item });
                                    break;
                                }
                            }
                        }
                        catch { }
                    }
                }
                else
                {
                    MessageBox.Show("[ " + DateTime.Now + " ] => [ Please Load LinkedIn Accounts From MyAccounts Menu ]");
                    AddLoggerLinkedingrp("[ " + DateTime.Now + " ] => [ Please Load LinkedIn Accounts From MyAccounts Menu ]");
                }
            }
            catch
            {
            }
            finally
            {
                if (chkListGroupMembers.InvokeRequired)
                {
                    chkListGroupMembers.Invoke(new MethodInvoker(delegate
                    {
                        AddLoggerLinkedingrp("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                        AddLoggerLinkedingrp("------------------------------------------------------------------------------------------------------------------------------------");
                        cmbAllUser.Enabled = true;
                        btnAddMember.Cursor = Cursors.Default;
                    }));
                }
            }
        }

        #endregion

        #region StartDMMultiThreadedGroupMemberUser

        public void StartCrawlSpecificGroupUser(object parameter)
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
                        lstGroupMessageThread.Add(Thread.CurrentThread);
                        lstGroupMessageThread.Distinct().ToList();
                        Thread.CurrentThread.IsBackground = true;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error >>> " + ex.StackTrace);
                }

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

                if (!Login.IsLoggedIn)
                {
                    Login.LoginHttpHelper(ref HttpHelper);
                }

                try
                {
                    if (Login.IsLoggedIn)
                    {
                        string selecteduser = string.Empty;
                        GroupStatus dataScrape = new GroupStatus();
                        dataScrape.loggerGroupMem.addToLogger += new EventHandler(GroupMemMessage_addToLogger);


                        cmbAllUser.Invoke(new MethodInvoker(delegate
                        {
                            selecteduser = cmbAllUser.SelectedItem.ToString();

                        }));

                        if (Login.accountUser == selecteduser)
                        {
                            //Login.LoginHttpHelper(ref HttpHelper);
                            if (GroupStatus.withExcelInput == true)
                            {
                                GroupMemData = dataScrape.AddSpecificGroupUserWithExcelInput(ref HttpHelper, Login.accountUser, SpeGroupId);
                            }
                            else
                            {
                                //GroupStatus.GroupMemUrl
                                if (chkAllAcounts.Checked)
                                {
                                    foreach (var SpeGroupId1 in GroupStatus.GroupMemUrl)
                                    {
                                        Dictionary<string, string> GroupMem = dataScrape.AddSpecificGroupUser(ref HttpHelper, Login.accountUser, SpeGroupId1);
                                        foreach (var GroupMem_item in GroupMem)
                                        {
                                            try
                                            {
                                                string GrpMemberKey = GroupMem_item.Key;
                                                string GrpMemberValue = GroupMem_item.Value;

                                                GroupMemData.Add(GrpMemberKey, GrpMemberValue);

                                            }
                                            catch { };

                                        }

                                    }
                                }
                                else
                                {
                                    //SpeGroupId = GroupStatus.GroupMemUrl[0];
                                    // GroupMemData = dataScrape.AddSpecificGroupUser(ref HttpHelper, Login.accountUser, SpeGroupId);
                                    GroupMemData = dataScrape.AddSpecificGroupUser(ref HttpHelper, Login.accountUser, Specgroupdetails);
                                }
                            }

                            if (GroupMemData.Count == 0)
                            {
                                string[] SelUser = selectedSpeUser.Split(':');
                                AddLoggerLinkedingrp("[ " + DateTime.Now + " ] => [ Group : " + SelUser[1] + " Has No Other Member than You ]");
                                return;
                            }
                            GrpMemNameWithGid.Add(Login.accountUser, GroupMemData);
                        }



                        if (chkListGroupMembers.InvokeRequired)
                        {
                            new Thread(() =>
                            {
                                chkListGroupMembers.Invoke(new MethodInvoker(delegate
                                {
                                    foreach (var itemM in GroupMemData)
                                    {
                                        chkListGroupMembers.Items.Add(itemM.Value);
                                    }
                                }));
                            }).Start();
                        }
                        AddLoggerLinkedingrp("[ " + DateTime.Now + " ] => [ Added Group Members of : " + item.Value._Username + " ]");
                    }
                    else
                    {
                        AddLoggerLinkedingrp("[ " + DateTime.Now + " ] => [ LinkedIn account : " + Login.accountUser + " has been temporarily restricted ]");
                    }
                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update --> StartDMMultiThreadedGroupUser() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update --> StartDMMultiThreadedGroupUser() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinGetGroupMemberErrorLogs);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error >>> " + ex.StackTrace);
            }
        }

        #endregion

        #region chkWithCsv_CheckedChanged
        private void chkWithCsv_CheckedChanged(object sender, EventArgs e)
        {

            if (chkWithCsv.Checked == true)
            {
                GroupStatus.WithCsvinAddFriend = true;
            }
            else
            {
                GroupStatus.WithCsvinAddFriend = false;
            }
        }
        #endregion

        private void chkWithSearch_CheckedChanged(object sender, EventArgs e)
        {

            if (chkWithSearch.Checked == true)
            {
                GroupStatus.WithGroupSearch = true;
                txtSearchKey.Visible = true;
                chkWithExcelInput.Checked = false;
            }
            else
            {
                GroupStatus.WithGroupSearch = false;
                txtSearchKey.Visible = false;
            }
        }

        #region chkGroupMem_CheckedChanged

        private void chkGroupMem_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                chk1stconn.Checked = false;
                chk2ndConn.Checked = false;
                chk3rdConn.Checked = false;

                if (chkGroupMem.Checked == true)
                {
                    for (int i = 0; i < chkListGroupMembers.Items.Count; i++)
                    {
                        string aaa = Convert.ToString(chkListGroupMembers.Items[i]);
                        chkListGroupMembers.SetItemChecked(i, true);
                        lblTotMemList.Text = "(" + chkListGroupMembers.Items.Count + ")";
                    }
                }
                else
                {
                    for (int i = 0; i < chkListGroupMembers.Items.Count; i++)
                    {
                        string aaa = Convert.ToString(chkListGroupMembers.Items[i]);
                        chkListGroupMembers.SetItemChecked(i, false);
                        int TotUnselectedMem = 0;
                        lblTotMemList.Text = "(" + TotUnselectedMem + ")";
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Search Groups --> chkGroupMem_CheckedChanged() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Search Groups --> chkGroupMem_CheckedChanged() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddSearchGroupErrorLogs);
            }
        }

        #endregion

        #region chk1stconn_CheckedChanged
        int counter = 0;
        private void chk1stconn_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                chkGroupMem.Checked = false;

                if (chk1stconn.Checked == true)
                {
                    //counter = 0;
                    //int counter = 0;
                    for (int i = 0; i < chkListGroupMembers.Items.Count; i++)
                    {
                        string itemName = chkListGroupMembers.Items[i].ToString();

                        if (itemName.Contains("1st"))
                        {
                            counter++;
                            chkListGroupMembers.SetItemChecked(i, true);
                            lblTotMemList.Text = "(" + counter + ")";
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < chkListGroupMembers.Items.Count; i++)
                    {
                        string itemName = chkListGroupMembers.Items[i].ToString();
                        if (itemName.Contains("1st"))
                        {
                            counter--;
                            chkListGroupMembers.SetItemChecked(i, false);

                            if (counter < 0)
                            {
                                counter = 0;
                            }

                            lblTotMemList.Text = "(" + counter + ")";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Search Groups --> chk1stconn_CheckedChanged() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Search Groups --> chk1stconn_CheckedChanged() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddSearchGroupErrorLogs);
            }
        }
        #endregion

        #region chk2ndConn_CheckedChanged
        private void chk2ndConn_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                chkGroupMem.Checked = false;
                if (chk2ndConn.Checked == true)
                {

                    for (int i = 0; i < chkListGroupMembers.Items.Count; i++)
                    {
                        string itemName = chkListGroupMembers.Items[i].ToString();

                        if (itemName.Contains("2nd"))
                        {
                            counter++;
                            chkListGroupMembers.SetItemChecked(i, true);
                            lblTotMemList.Text = "(" + counter + ")";
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < chkListGroupMembers.Items.Count; i++)
                    {
                        string itemName = chkListGroupMembers.Items[i].ToString();
                        if (itemName.Contains("2nd"))
                        {
                            counter--;
                            chkListGroupMembers.SetItemChecked(i, false);

                            if (counter < 0)
                            {
                                counter = 0;
                            }

                            lblTotMemList.Text = "(" + counter + ")";


                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Search Groups --> chk2ndConn_CheckedChanged() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Search Groups --> chk2ndConn_CheckedChanged() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddSearchGroupErrorLogs);
            }
        }
        #endregion

        #region chk3rdConn_CheckedChanged
        private void chk3rdConn_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                chkGroupMem.Checked = false;
                if (chk3rdConn.Checked == true)
                {

                    for (int i = 0; i < chkListGroupMembers.Items.Count; i++)
                    {
                        string itemName = chkListGroupMembers.Items[i].ToString();

                        if (itemName.Contains("3rd"))
                        {
                            counter++;
                            chkListGroupMembers.SetItemChecked(i, true);
                            lblTotMemList.Text = "(" + counter + ")";
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < chkListGroupMembers.Items.Count; i++)
                    {
                        string itemName = chkListGroupMembers.Items[i].ToString();
                        if (itemName.Contains("3rd"))
                        {
                            counter--;
                            chkListGroupMembers.SetItemChecked(i, false);

                            if (counter < 0)
                            {
                                counter = 0;
                            }

                            lblTotMemList.Text = "(" + counter + ")";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Search Groups --> chk3rdConn_CheckedChanged() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Search Groups --> chk3rdConn_CheckedChanged() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddSearchGroupErrorLogs);
            }

        }
        #endregion

        #region chkListGroupMembers_SelectedIndexChanged
        private void chkListGroupMembers_SelectedIndexChanged(object sender, EventArgs e)
        {
            int count = Convert.ToInt32(lblTotMemList.Text.Replace("(", string.Empty).Replace(")", string.Empty));
            int count2 = chkListGroupMembers.CheckedItems.Count + 1;
            if (chkMessageTo.CheckOnClick == true)
            {
                if (count < count2)
                {
                    lblTotMemList.Text = "(" + (count + 1).ToString() + ")";
                }
                else
                {
                    lblTotMemList.Text = "(" + (count - 1).ToString() + ")";
                }
            }
        }
        #endregion

        #region For -- btnGroupMessage_Click --

        string _SelectedGroupName_MessageGroupMember = string.Empty;
        bool mesg_with_tag = false;
        bool msg_spintaxt = false;
        private void btnGroupMessage_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {

                try
                {
                    lstGroupMessageThread.Clear();
                    if (IsStop)
                    {
                        IsStop = false;
                    }
                    try
                    {
                        if (GroupStatus.selectAllGroup == false)
                        {
                            try
                            {
                                _SelectedGroupName_MessageGroupMember = cmbMemberGroup.SelectedItem.ToString();
                            }
                            catch { }
                        }
                        //if (RdbMessagingWithTag_MessageGroupMember.Checked)
                        //{
                        if (string.IsNullOrEmpty(_SelectedGroupName_MessageGroupMember))
                        {
                            if (GroupStatus.selectAllGroup == false)
                            {
                                MessageBox.Show("Please Select User Groups !");
                                return;
                            }
                        }
                        //}

                        if (ChkMessagingWithTag_MessageGroupMember.Checked)
                        {
                            mesg_with_tag = true;
                        }
                        else
                        {
                            mesg_with_tag = false;
                        }

                        if (LinkedInManager.linkedInDictionary.Count() == 0)
                        {
                            return;
                        }
                        else if (cmbAllUser.Items.Count == 0)
                        {
                            if (GroupStatus.selectAllGroup == false)
                            {
                                MessageBox.Show("Please click Get Group button..");
                                return;
                            }
                        }
                        else if (selectedSpeUser == string.Empty)
                        {
                            if (GroupStatus.selectAllGroup == false)
                            {
                                MessageBox.Show("Please select any User Group");
                                return;
                            }
                        }
                        else if (chkListGroupMembers.CheckedItems.Count == 0)
                        {
                            if (GroupStatus.selectAllGroup == false)
                            {
                                AddLoggerGroupMemMessage("[ " + DateTime.Now + " ] => [ Please select Atleast One Member.. ]");
                                MessageBox.Show("Please select Atleast One Member..");
                                return;
                            }
                        }

                        else if (string.IsNullOrEmpty(txtSubject.Text))
                        {
                            AddLoggerGroupMemMessage("[ " + DateTime.Now + " ] => [ Please Add Subject.. ]");
                            MessageBox.Show("Please Add Subject..");
                            btnGrpName.Focus();
                            return;
                        }
                        else if (string.IsNullOrEmpty(txtBody.Text))
                        {
                            AddLoggerGroupMemMessage("[ " + DateTime.Now + " ] => [ Please Add Message Body.. ]");
                            MessageBox.Show("Please Add Message Body..");
                            return;
                        }

                        if (txtSubject.Text.Contains("|") && !chkUseSpintax_GroupMessage.Checked)
                        {
                            AddLoggerGroupMemMessage("[ " + DateTime.Now + " ] => [ Please Check SpinTax CheckBox.. ]");
                            MessageBox.Show("Please Check SpinTax CheckBox..");
                            return;
                        }
                        else if (txtBody.Text.Contains("|") && !chkUseSpintax_GroupMessage.Checked)
                        {
                            AddLoggerGroupMemMessage("[ " + DateTime.Now + " ] => [ Please Check SpinTax CheckBox.. ]");
                            MessageBox.Show("Please Check SpinTax CheckBox..");
                            return;
                        }

                        if (chkUseSpintax_GroupMessage.Checked)
                        {
                            msg_spintaxt = true;
                            GrpMemSubjectlist = SpinnedListGenerator.GetSpinnedList(new List<string> { txtSubject.Text });
                            GrpMemMessagelist = SpinnedListGenerator.GetSpinnedList(new List<string> { txtBody.Text });
                        }
                        else
                        {
                            msg_spintaxt = false;
                        }

                        btnGroupMessage.Cursor = Cursors.AppStarting;

                        if (chkListGroupMembers.CheckedItems.Count > 0)
                        {
                            new Thread(() =>
                            {
                                LinkedInComposeMessageGroupMem();

                            }).Start();
                        }
                        else
                        {
                        }
                    }
                    catch (Exception ex)
                    {
                        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnGroupMessage_Click() --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnGroupMessage_Click() --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinComposeMessageErrorLogs);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error >>> " + ex.StackTrace);
                }
                finally
                {
                    AddLoggerGroupMemMessage("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                    AddLoggerGroupMemMessage("--------------------------------------------------------------------------------------------------------------------------");
                }
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddLoggerGroupMemMessage("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        }

        #endregion

        #region For --LinkedInComposeMessageGroupMem --

        private void LinkedInComposeMessageGroupMem()
        {
            AddLoggerGroupMemMessage("[ " + DateTime.Now + " ] => [ Starting Sending Message To Selected Contacts ]");

            try
            {
                cmbMemberGroup.Invoke(new MethodInvoker(delegate
                {
                    if (GroupStatus.selectAllGroup == false)
                    {
                        UserSlecetedDetails = cmbMemberGroup.SelectedItem.ToString();
                    }
                }));

                if (LinkedInManager.linkedInDictionary.Count() > 0)
                {
                    string value = string.Empty;
                    cmbMemberGroup.Invoke(new MethodInvoker(delegate
                    {
                        if (GroupStatus.selectAllGroup == false)
                        {
                            value = cmbMemberGroup.SelectedItem.ToString();
                            string[] cId = value.Split(':');
                            value = cId[0].Trim();
                        }
                    }));


                    foreach (KeyValuePair<string, LinkedInMaster> item in LinkedInManager.linkedInDictionary)
                    {
                        if (GroupStatus.selectAllGroup == false)
                        {
                            if (item.Key.Trim() == value.Trim())
                            {
                                PostMessageGroupMembers(new object[] { item });
                            }

                        }
                        else
                        {
                            PostMessageGroupMembers(new object[] { item });

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Compose Message --> LinkedInComposeMessageGroupMem() --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Compose Message --> LinkedInComposeMessageGroupMem() --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinComposeMessageErrorLogs);
            }
        }

        #endregion

        #region For -- PostMessageGroupMembers --

        public void PostMessageGroupMembers(object parameter)
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
                        lstGroupMessageThread.Add(Thread.CurrentThread);
                        lstGroupMessageThread.Distinct().ToList();
                        Thread.CurrentThread.IsBackground = true;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error >>> " + ex.StackTrace);
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

                AddLoggerGroupMemMessage("[ " + DateTime.Now + " ] => [ Sending Message From Account : " + item.Key + " ]");
                string selectedusername = item.Key;
                try
                {
                    if (!Login.IsLoggedIn)
                    {
                        Login.LoginHttpHelper(ref HttpHelper);
                    }
                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Compose Message --> PostMessageBulk() -->  Login Process >>>>" + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Compose Message --> PostMessageBulk() -->  Login Process >>>>" + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinComposeMessageErrorLogs);
                }

                try
                {
                    if (Login.IsLoggedIn)
                    {
                        AddLoggerGroupMemMessage("[ " + DateTime.Now + " ] => [ Getting Contacts to Send Message ]");
                        List<string> SelectedItem = new List<string>();
                        string Userid = string.Empty;
                        if (cmbMemberGroup.InvokeRequired)
                        {
                            cmbMemberGroup.Invoke(new MethodInvoker(delegate
                            {
                                if (GroupStatus.selectAllGroup == false)
                                {
                                    Userid = cmbMemberGroup.SelectedItem.ToString();
                                }
                            }));
                        }

                        GroupStatus MemberScrape = new GroupStatus();
                        string FromEmailId = MemberScrape.FromEmailCodeMsgGroupMem(ref HttpHelper, SpeGroupId);
                        string FromEmailName = MemberScrape.FromName(ref HttpHelper);

                        FromemailId = FromEmailId;
                        FromEmailNam = FromEmailName;

                        Dictionary<string, string> SelectedGroupMem = new Dictionary<string, string>();
                        foreach (KeyValuePair<string, Dictionary<string, string>> contacts in GrpMemNameWithGid)
                        {
                            if (contacts.Key.Trim() == item.Key.Trim())
                            {
                                foreach (KeyValuePair<string, string> Details in contacts.Value)
                                {
                                    foreach (string itemChecked in chkListGroupMembers.CheckedItems)
                                    {
                                        if (itemChecked == Details.Value)
                                        {
                                            try
                                            {
                                                string id = Details.Key;
                                                SelectedGroupMem.Add(id, Details.Value);
                                            }
                                            catch
                                            {
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        string msgSub = txtSubject.Text;
                        string msgBody = txtBody.Text;


                        if (chkUseSpintax_GroupMessage.Checked)
                        {
                            try
                            {
                                msgSub = GrpMemSubjectlist[RandomNumberGenerator.GenerateRandom(0, GrpMemSubjectlist.Count - 1)];
                                msgBody = GrpMemMessagelist[RandomNumberGenerator.GenerateRandom(0, GrpMemMessagelist.Count - 1)];
                            }
                            catch
                            {
                            }
                        }

                        int minDelay = 20;
                        int maxDelay = 25;

                        if (!string.IsNullOrEmpty(txtMsgGroupMemMinDelay.Text) && NumberHelper.ValidateNumber(txtMsgGroupMemMinDelay.Text))
                        {
                            minDelay = Convert.ToInt32(txtMsgGroupMemMinDelay.Text);
                        }
                        if (!string.IsNullOrEmpty(txtMsgGroupMemMaxDelay.Text) && NumberHelper.ValidateNumber(txtMsgGroupMemMaxDelay.Text))
                        {
                            maxDelay = Convert.ToInt32(txtMsgGroupMemMaxDelay.Text);
                        }

                        try
                        {
                            Groups.MessageGroupMember obj_MessageGroupMember = new Groups.MessageGroupMember();
                            obj_MessageGroupMember.logger.addToLogger += new EventHandler(GroupMemMessage_addToLogger);
                            obj_MessageGroupMember.PostFinalMsgGroupMember_1By1(ref HttpHelper, SelectedGroupMem, GrpMemSubjectlist, GrpMemMessagelist, msgSub, msgBody, selectedusername, FromemailId, FromEmailNam, _SelectedGroupName_MessageGroupMember, SpeGroupId, mesg_with_tag, msg_spintaxt, minDelay, maxDelay, preventMsgSameGroup, preventMsgWithoutGroup, preventMsgGlobal);
                            obj_MessageGroupMember.logger.addToLogger -= new EventHandler(GroupMemMessage_addToLogger);
                        }
                        catch
                        {
                        }
                        finally
                        {
                            if (btnGroupMessage.InvokeRequired)
                            {
                                btnGroupMessage.Invoke(new MethodInvoker(delegate
                                {
                                    AddLoggerGroupMemMessage("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                                    AddLoggerGroupMemMessage("----------------------------------------------------------------------------------------------------------------------------------------");
                                    btnGroupMessage.Cursor = Cursors.Default;
                                }));
                            }
                        }
                    }
                    else if (!Login.IsLoggedIn)
                    {
                    }
                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Compose Message --> PostMessageBulk() -->  PostFinalMsg >>>>" + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Compose Message --> PostMessageBulk() -->  PostFinalMsg >>>>" + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinComposeMessageErrorLogs);
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Compose Message --> PostMessageBulk() --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Compose Message --> PostMessageBulk() --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinComposeMessageErrorLogs);
            }
        }

        #endregion

        #region PostFinalMsgGroupMember

        public void PostFinalMsgGroupMember(ref GlobusHttpHelper HttpHelper, Dictionary<string, string> SlectedContacts, string msg, string body, string UserName)
        {
            try
            {
                string postdata = string.Empty;
                string postUrl = string.Empty;
                string ResLogin = string.Empty;
                string csrfToken = string.Empty;
                string sourceAlias = string.Empty;
                string ReturnString = string.Empty;
                string PostMsgSubject = string.Empty;
                string PostMsgBody = string.Empty;
                string FString = string.Empty;
                string Nstring = string.Empty;
                string connId = string.Empty;
                string FullName = string.Empty;
                try
                {
                    string MessageText = string.Empty;
                    string PostedMessage = string.Empty;

                    string pageSource = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/home?trk=hb_tab_home_top"));
                    if (pageSource.Contains("csrfToken"))
                    {
                        csrfToken = pageSource.Substring(pageSource.IndexOf("csrfToken"), 50);
                        string[] Arr = csrfToken.Split('&');
                        csrfToken = Arr[0];
                        csrfToken = csrfToken.Replace("csrfToken", "").Replace("\"", string.Empty).Replace("value", string.Empty).Replace("cs", string.Empty).Replace("id", string.Empty).Replace("=", string.Empty);
                        csrfToken = csrfToken.Trim();
                    }

                    if (pageSource.Contains("sourceAlias"))
                    {
                        sourceAlias = pageSource.Substring(pageSource.IndexOf("sourceAlias"), 100);
                        string[] Arr = sourceAlias.Split('"');
                        sourceAlias = Arr[2];
                    }

                    if (chkListGroupMembers.CheckedItems.Count > 0)
                    {
                        string ContactName = string.Empty;
                        foreach (KeyValuePair<string, string> itemChecked in SlectedContacts)
                        {
                            try
                            {
                                string FName = string.Empty;
                                string Lname = string.Empty;

                                FName = itemChecked.Value.Split(' ')[0];
                                Lname = itemChecked.Value.Split(' ')[1];

                                FullName = FName + " " + Lname;
                                try
                                {
                                    ContactName = ContactName + "  :  " + FullName;
                                }
                                catch
                                {
                                }
                                AddLoggerComposeMessage("[ " + DateTime.Now + " ] => [ Adding Contact " + FullName + " ]");

                                string ToCd = itemChecked.Key;
                                List<string> AddAllString = new List<string>();

                                if (FString == string.Empty)
                                {
                                    string CompString = "{" + "\"" + "_" + ToCd.Trim() + "\"" + ":" + "{" + "\"" + "memberId" + "\"" + ":" + "\"" + ToCd.Trim() + "\"" + "," + "\"" + "firstName" + "\"" + ":" + "\"" + FName + "\"" + "," + "\"" + "lastName" + "\"" + ":" + "\"" + Lname + "\"" + "}";
                                    FString = CompString;
                                }
                                else
                                {
                                    string CompString = "\"" + "_" + ToCd.Trim() + "\"" + ":" + "{" + "\"" + "memberId" + "\"" + ":" + "\"" + ToCd.Trim() + "\"" + "," + "\"" + "firstName" + "\"" + ":" + "\"" + FName + "\"" + "," + "\"" + "lastName" + "\"" + ":" + "\"" + Lname + "\"" + "}";
                                    FString = CompString;
                                }

                                if (Nstring == string.Empty)
                                {
                                    Nstring = FString;
                                    connId = ToCd;
                                }
                                else
                                {
                                    Nstring += "," + FString;
                                    connId += " " + ToCd;
                                }
                            }
                            catch
                            {
                            }
                        }
                        Nstring += "}";

                        try
                        {
                            string PostMessage;
                            string ResponseStatusMsg;
                            AddLoggerGroupMemMessage("[ " + DateTime.Now + " ] => [ Sending Message ]");
                            //PostMessage = "csrfToken=" + csrfToken + "&subject=" + txtMsgSubject.Text.ToString() + "&body=" + txtMsgBody.Text.ToString() + "&submit=Send Message&showRecipeints=showRecipeints&ccSender=ccSender&fromName=" + FromEmailNam + "&fromEmail=" + FromemailId + "&connectionIds=" + connId + "&connectionNames=" + Nstring + "&allowEditRcpts=true&addMoreRcpts=false&openSocialAppBodySuffix=&st=&viewerDestinationUrl=&goback=.rmg_*1_*1_*1_*1_*1_*1_*1_*1_*1";
                            PostMessage = "csrfToken=" + csrfToken + "&subject=" + msg.ToString() + "&body=" + body.ToString() + "&submit=Send+Message&showRecipeints=showRecipeints&fromName=" + FromEmailNam + "&fromEmail=" + FromemailId + "&connectionIds=" + connId + "&connectionNames=" + Uri.EscapeUriString(Nstring) + "&allowEditRcpts=true&addMoreRcpts=false&openSocialAppBodySuffix=&st=&viewerDestinationUrl=&goback=.smg_*1_*1_*1_*1_*1_*1_*1_*1_*1";
                            //IncodePost = Uri.EscapeUriString(PostMessage).Replace(":", "%3A").Replace("%20", "+").Replace("++", "+");
                            ResponseStatusMsg = HttpHelper.postFormData(new Uri("http://www.linkedin.com/msgToConns"), PostMessage);

                            if (ResponseStatusMsg.Contains("Your message was successfully sent."))
                            {
                                AddLoggerGroupMemMessage("[ " + DateTime.Now + " ] => [ Subject Posted : " + txtSubject.Text.ToString() + " ]");
                                AddLoggerGroupMemMessage("[ " + DateTime.Now + " ] => [ Body Text Posted : " + body.ToString() + " ]");
                                AddLoggerGroupMemMessage("[ " + DateTime.Now + " ] => [ Message Posted To All Selected Accounts ]");
                                ReturnString = "Your message was successfully sent.";

                                #region CSV

                                string CSVHeader = "UserName" + "," + "Subject" + "," + "Body Text" + "," + "ContactName";
                                string CSV_Content = UserName + "," + txtSubject.Text.ToString() + "," + body.ToString() + "," + ContactName;
                                CSVUtilities.ExportDataCSVFile(CSVHeader, CSV_Content, Globals.path_AddConnectionSuccessWith2ndDegree);

                                #endregion

                                //GlobusFileHelper.AppendStringToTextfileNewLine("Subject Posted : " + textMsg.Text.ToString(), Globals.path_MessageGroupMember);
                                //GlobusFileHelper.AppendStringToTextfileNewLine("Body Text Posted : " + txtBody.Text.ToString(), Globals.path_MessageGroupMember);
                                //GlobusFileHelper.AppendStringToTextfileNewLine("Message Posted To All Selected Accounts", Globals.path_MessageGroupMember);
                            }
                            else if (ResponseStatusMsg.Contains("There was an unexpected problem that prevented us from completing your request."))
                            {
                                AddLoggerGroupMemMessage("[ " + DateTime.Now + " ] => [ Error In Message Posting ]");
                                GlobusFileHelper.AppendStringToTextfileNewLine("Error In Message Posting", Globals.path_MessageGroupMember);
                            }
                        }
                        catch (Exception ex)
                        {
                            //AddLoggerGroupMemMessage("[ " + DateTime.Now + " ] => [ Error:" + ex.Message + "StackTrace --> >>>" + ex.StackTrace + " ]");
                            GlobusFileHelper.AppendStringToTextfileNewLine(" Error:" + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.path_MessageGroupMember);
                        }
                    }
                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error -->  PostFinalMsgGroupMember() --> 1 --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> PostFinalMsgGroupMember() --> 1 --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinComposeMessageErrorLogs);
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error -->  PostFinalMsgGroupMember() --> 2 --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> PostFinalMsgGroupMember() --> 2 --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinComposeMessageErrorLogs);
            }
            finally
            {
                //counter_compose_message--;
                //if (counter_compose_message == 0)
                //{
                if (btnGroupMessage.InvokeRequired)
                {
                    btnGroupMessage.Invoke(new MethodInvoker(delegate
                    {
                        //pictureBox10Red.Visible = false;
                        //pictureBox10Green.Visible = true;
                        AddLoggerGroupMemMessage("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                        AddLoggerGroupMemMessage("-------------------------------------------------------------------------------------------------------------------------------");
                    }));
                }
                //}
            }
        }

        #endregion

        #region frmMain_FormClosing
        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dialogresult = MessageBox.Show("Sure you want to exit LinkedDominator App !", "LinkedDominator", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dialogresult == DialogResult.Yes)
            {
                try
                {
                    var prc = System.Diagnostics.Process.GetProcesses();
                    foreach (var item in prc)
                    {
                        try
                        {
                            if (item.ProcessName.Contains("LD_LicensingManager"))
                            {
                                item.Kill();
                            }
                        }
                        catch
                        {
                        }
                    }
                    Application.ExitThread();
                    Application.Exit();

                    Thread.Sleep(1000);

                    var prc1 = System.Diagnostics.Process.GetProcesses();
                    foreach (var item in prc1)
                    {
                        try
                        {
                            if (item.ProcessName.Contains("LD_LicensingManager"))
                            {
                                item.Kill();
                            }
                        }
                        catch
                        {
                        }
                    }

                    Application.Exit();
                }
                catch
                {
                }
            }
            else
            {

                e.Cancel = true;
                this.Activate();

            }
        }
        #endregion

        #region chkOpenGroup_CheckedChanged
        private void chkOpenGroup_CheckedChanged(object sender, EventArgs e)
        {
            chkListSearchGroup.Items.Clear();
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
                string GetUserID = cmbSearchGroup.SelectedItem.ToString();
                foreach (KeyValuePair<string, Dictionary<string, string>> item in LinkdInContacts)
                {
                    if (GetUserID.Contains(item.Key))
                    {
                        Dictionary<string, string> GmUserIDs = item.Value;

                        foreach (KeyValuePair<string, string> item1 in item.Value)
                        {
                            if (openGrp == "true" && MemGrp == "true")
                            {
                                AddLoggerSearchGroup("[ " + DateTime.Now + " ] => [ " + item1.Key + " ]");
                                chkListSearchGroup.Items.Add(item1.Key);
                            }
                            else if (openGrp == "true" && MemGrp == "false")
                            {
                                if (item1.Key.Contains("Open Group"))
                                {
                                    AddLoggerSearchGroup("[ " + DateTime.Now + " ] => [ " + item1.Key + " ]");
                                    chkListSearchGroup.Items.Add(item1.Key);
                                }
                            }
                            else if (openGrp == "false" && MemGrp == "true")
                            {
                                if (item1.Key.Contains("Member Only Group"))
                                {
                                    AddLoggerSearchGroup("[ " + DateTime.Now + " ] => [ " + item1.Key + " ]");
                                    chkListSearchGroup.Items.Add(item1.Key);
                                }
                            }
                            else if (openGrp == "false" && MemGrp == "false")
                            {
                            }
                        }
                    }
                }

                AddLoggerSearchGroup("[ " + DateTime.Now + " ] => [ Finished Finding Group of Selected User ]");
                AddLoggerSearchGroup("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                AddLoggerSearchGroup("------------------------------------------------------------------------------------------------------------------------------------");
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Search Groups --> chkOpenGroup_CheckedChanged() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Search Groups --> chkOpenGroup_CheckedChanged() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddSearchGroupErrorLogs);
            }
        }
        #endregion

        #region chkMemberGroup_CheckedChanged
        private void chkMemberGroup_CheckedChanged(object sender, EventArgs e)
        {
            chkListSearchGroup.Items.Clear();

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
                string GetUserID = cmbSearchGroup.SelectedItem.ToString();
                foreach (KeyValuePair<string, Dictionary<string, string>> item in LinkdInContacts)
                {
                    if (GetUserID.Contains(item.Key))
                    {
                        Dictionary<string, string> GmUserIDs = item.Value;

                        foreach (KeyValuePair<string, string> item1 in item.Value)
                        {
                            if (openGrp == "true" && MemGrp == "true")
                            {
                                AddLoggerSearchGroup("[ " + DateTime.Now + " ] => [ " + item1.Key + " ]");
                                chkListSearchGroup.Items.Add(item1.Key);
                            }
                            else if (openGrp == "true" && MemGrp == "false")
                            {
                                if (item1.Key.Contains("Open Group"))
                                {
                                    AddLoggerSearchGroup("[ " + DateTime.Now + " ] => [ " + item1.Key + " ]");
                                    chkListSearchGroup.Items.Add(item1.Key);
                                }
                            }
                            else if (openGrp == "false" && MemGrp == "true")
                            {
                                if (item1.Key.Contains("Member Only Group"))
                                {
                                    AddLoggerSearchGroup("[ " + DateTime.Now + " ] => [ " + item1.Key + " ]");
                                    chkListSearchGroup.Items.Add(item1.Key);
                                }
                            }
                            else if (openGrp == "false" && MemGrp == "false")
                            {
                            }
                        }
                    }
                }

                AddLoggerSearchGroup("[ " + DateTime.Now + " ] => [ Finished Finding Group of Selected User ]");
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Search Groups --> chkOpenGroup_CheckedChanged() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Search Groups --> chkOpenGroup_CheckedChanged() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddSearchGroupErrorLogs);
            }
        }
        #endregion

        #region btnstop_Click
        private void btnstop_Click(object sender, EventArgs e)
        {
            try
            {
                IsStop = true;
                foreach (Thread item in lstworkingThread)
                {
                    try
                    {
                        item.Abort();
                    }
                    catch
                    {
                    }
                }
                AddLoggerCreateGroup("[ " + DateTime.Now + " ] => [ PROCESS STOPPED ]");
                AddLoggerCreateGroup("--------------------------------------------------------------------------------------------------------------------------");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error >>> " + ex.StackTrace);
            }
        }
        #endregion

        #region btnstopSentMessage_Click
        private void btnstopSentMessage_Click(object sender, EventArgs e)
        {
            try
            {
                IsStop = true;
                List<Thread> lstTemp = lstGroupMessageThread.Distinct().ToList();
                foreach (Thread item in lstTemp)
                {
                    try
                    {
                        item.Abort();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error >>> " + ex.StackTrace);
                    }
                }

                AddLoggerGroupMemMessage("-------------------------------------------------------------------------------------------------------------------------------");
                AddLoggerGroupMemMessage("[ " + DateTime.Now + " ] => [ PROCESS STOPPED ]");
                AddLoggerGroupMemMessage("-------------------------------------------------------------------------------------------------------------------------------");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error >>> " + ex.StackTrace);
            }
        }
        #endregion

        #region btnEmailInvite_Click
        private void btnEmailInvite_Click(object sender, EventArgs e)
        {
            try
            {
                IsStop = true;
                IsStop = true;
                List<Thread> lstTemp = lstSearchconnectionThread.Distinct().ToList();

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

                AddLoggerManageConnection("-------------------------------------------------------------------------------------------------------------------------------");
                AddLoggerManageConnection("[ " + DateTime.Now + " ] => [ PROCESS STOPPED ]");
                AddLoggerManageConnection("-------------------------------------------------------------------------------------------------------------------------------");
                btnSendEmailInvite.Cursor = Cursors.Default;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error >>> " + ex.StackTrace);
            }
        }

        #endregion

        #region btnsearch_Click
        private void btnsearch_Click(object sender, EventArgs e)
        {
            IsStop = true;
            foreach (Thread item in lstSearchconnectionThread)
            {
                try
                {
                    item.Abort();
                }
                catch
                {
                }
            }
            AddLoggerManageConnection("-------------------------------------------------------------------------------------------------------------------------------");
            AddLoggerManageConnection("[ " + DateTime.Now + " ] => [ PROCESS STOPPED ]");
            AddLoggerManageConnection("-------------------------------------------------------------------------------------------------------------------------------");
        }
        #endregion

        #region btnjoinfriendGroup_Click
        private void btnjoinfriendGroup_Click(object sender, EventArgs e)
        {
            try
            {
                IsStop = true;
                foreach (Thread item in lstJoinFriendGroupThread)
                {
                    try
                    {
                        item.Abort();
                    }
                    catch
                    {
                    }
                }
                AddLoggerGroupAdd("-------------------------------------------------------------------------------------------------------------------------------");
                AddLoggerGroupAdd("[ " + DateTime.Now + " ] => [ PROCESS STOPPED ]");
                AddLoggerGroupAdd("-------------------------------------------------------------------------------------------------------------------------------");

                btnExistGroup.Cursor = Cursors.Default;
                btnAddGroups.Cursor = Cursors.Default;
                btnJoinMemGroup.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error >>> " + ex.StackTrace);
            }
        }
        #endregion

        #region btnSrpGrpMemStop_Click
        private void btnSrpGrpMemStop_Click(object sender, EventArgs e)
        {
            try
            {
                IsStop = true;
                List<Thread> lstTemp = lstJoinSearchGroupThread.Distinct().ToList();
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
                AddLoggerSearchGroup("-------------------------------------------------------------------------------------------------------------------------------");
                AddLoggerSearchGroup("[ " + DateTime.Now + " ] => [ PROCESS STOPPED ]");
                AddLoggerSearchGroup("-------------------------------------------------------------------------------------------------------------------------------");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error >>> " + ex.StackTrace);
            }
        }
        #endregion

        #region btngrpupdate_Click
        private void btngrpupdate_Click(object sender, EventArgs e)
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

                btnGroupUpdate.Enabled = true;
                btnGetUser.Cursor = Cursors.Default;
                btnGroupUpdate.Cursor = Cursors.Default;
                counter_GroupMemberSearch = 0;
                //counter_GroupMemberSearch = 1;
                AddLoggerGroupStatus("-------------------------------------------------------------------------------------------------------------------------------");
                AddLoggerGroupStatus("[ " + DateTime.Now + " ] => [ PROCESS STOPPED ]");
                AddLoggerGroupStatus("-------------------------------------------------------------------------------------------------------------------------------");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error >>> " + ex.StackTrace);
            }
        }
        #endregion

        #region btncomposemsg_Click
        private void btncomposemsg_Click(object sender, EventArgs e)
        {
            try
            {
                IsStop = true;
                List<Thread> lstTemp = lstComposeMessageThread.Distinct().ToList();
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
                AddLoggerComposeMessage("-------------------------------------------------------------------------------------------------------------------------------");
                AddLoggerComposeMessage("[ " + DateTime.Now + " ] => [ PROCESS STOPPED ]");
                AddLoggerComposeMessage("-------------------------------------------------------------------------------------------------------------------------------");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error >>> " + ex.StackTrace);
            }
        }
        #endregion

        #region btnstatusstop_Click
        private void btnstatusstop_Click(object sender, EventArgs e)
        {
            try
            {
                IsStop = true;
                List<Thread> lstTemp = lstStatusUpdateThread.Distinct().ToList();
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
                AddLoggerStatusUpdate("-------------------------------------------------------------------------------------------------------------------------------");
                AddLoggerStatusUpdate("[ " + DateTime.Now + " ] => [ PROCESS STOPPED ]");
                AddLoggerStatusUpdate("-------------------------------------------------------------------------------------------------------------------------------");
                btnStatusPost.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error >>> " + ex.StackTrace);
            }
        }
        #endregion

        #region StatusStop
        private void StatusStop()
        {
            try
            {
                IsStop = true;
                List<Thread> lstTemp = lstStatusUpdateThread.Distinct().ToList();
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
                AddLoggerStatusUpdate("-------------------------------------------------------------------------------------------------------------------------------");
                AddLoggerStatusUpdate("[ " + DateTime.Now + " ] => [ PROCESS STOPPED ]");
                AddLoggerStatusUpdate("-------------------------------------------------------------------------------------------------------------------------------");
            }
            catch
            {
            }
        }
        #endregion
        
        #region choseDefaultFolderPathToolStripMenuItem_Click
        private void choseDefaultFolderPathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                frmChangeDefaultFolderPath obj_frmChangeDefaultFolderPath = new frmChangeDefaultFolderPath();
                obj_frmChangeDefaultFolderPath.Show();
            }
            catch
            {
            }
        }
        #endregion

        #region DisableEnableScrapperControls

        private void DisableEnableScrapperControls()
        {
            this.Invoke(new MethodInvoker(delegate
            {
                //btnSearchNewScraper.Enabled = !(btnSearchNewScraper.Enabled);
                //btnSearchScraper.Enabled = !(btnSearchScraper.Enabled);
                //btnStopScraper.Enabled = !(btnStopScraper.Enabled);
                txtScraperMinDelay.Visible = true;
                checkedListBoxFortune1000.Enabled = false;
                checkedListBoxCompanySize.Enabled = false;
                checkedListBoxInterestedIN.Enabled = false;
                checkedListBoxSeniorlevel.Enabled = false;
                checkedListBoxRecentlyJoined.Enabled = false;
                GetDataForPrimiumAccount.Enabled = false;
                checkedListFunction.Enabled = false;
            }));
        }



        #endregion
        
        #region LinkedinSearch_Paint
        private void LinkedinSearch_Paint(object sender, PaintEventArgs e)
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

        #region btnLinkedinSearch_Click
        private void btnLinkedinSearch_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {

                try
                {
                    _LinkedinSearchtxtSearchItem = string.Empty;

                    AddLoggerLinkedinSearch("[ " + DateTime.Now + " ] => [ Starting Search ]");
                    _IsStop_LinkedinSearch = false;
                    lstLinkedinSearchThraed.Clear();

                    if (chkLinkedinSearchRandomEmailId.Checked)
                    {
                        AddLoggerLinkedinSearch("[ " + DateTime.Now + " ] => [ Starting Search For Random Email Id ]");
                        _LinkedinSearchSelectedEmailId = cmbLinkedinSelectEmailId.Items[new Random().Next(0, cmbLinkedinSelectEmailId.Items.Count)].ToString();
                    }
                    else
                    {
                        AddLoggerLinkedinSearch("[ " + DateTime.Now + " ] => [ Starting Search For All Email Id ]");
                        _LinkedinSearchSelectedEmailId = cmbLinkedinSelectEmailId.SelectedItem.ToString();
                    }
                    try
                    {
                        if (string.IsNullOrEmpty(cmbLinkedinSearch.SelectedItem.ToString()))
                        {
                            cmbLinkedinSearch.SelectedItem = cmbLinkedinSearch.Items[0];
                        }
                    }
                    catch
                    {
                        cmbLinkedinSearch.SelectedItem = cmbLinkedinSearch.Items[0];
                    }

                    if (rdbLinkedinSearchKeyword.Checked)
                    {
                        AddLoggerLinkedinSearch("[ " + DateTime.Now + " ] => [ Search Through Keyword ]");
                        _IsKeyword_LinkedinSearch = true;
                    }

                    if (rdbLinkedinSearchURL.Checked)
                    {
                        AddLoggerLinkedinSearch("[ " + DateTime.Now + " ] => [ Search Through Url ]");
                        _IsProfileURL_LinkedinSearch = true;
                    }

                    _LinkedinSearchSelectedcmbLinkedinSearch = cmbLinkedinSearch.SelectedItem.ToString();

                    InBoardProGetData.LinkedinSearch._Search = _LinkedinSearchSelectedcmbLinkedinSearch;

                    _LinkedinSearchtxtSearchItem = txtLinkedinSearch.Text;

                    InBoardProGetData.LinkedinSearch._Keyword = _LinkedinSearchtxtSearchItem;

                    if (rdbLinkedinSearchURL.Checked)
                    {
                        if (lstLinkedinSearchProfileURL.Count > 0)
                        {
                            InBoardProGetData.LinkedinSearch.lstLinkedinSearchProfileURL = lstLinkedinSearchProfileURL;
                        }
                        else
                        {
                            MessageBox.Show(" Please Upload The Profile URL File !");
                            return;
                        }
                    }

                    if (rdbLinkedinSearchKeyword.Checked)
                    {
                        if ((string.IsNullOrEmpty(_LinkedinSearchSelectedEmailId)) || (string.IsNullOrEmpty(_LinkedinSearchSelectedcmbLinkedinSearch)) || (string.IsNullOrEmpty(_LinkedinSearchtxtSearchItem)))
                        {
                            MessageBox.Show(" Please Select Email Id , Search and Enter Search Keyword !");
                            return;
                        }
                    }
                    btnLinkedinSearch.Cursor = Cursors.AppStarting;

                    Thread thread_LinkedinSearch = new Thread(StartLinkedinSearch);
                    thread_LinkedinSearch.Start();
                }
                catch { }
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddLoggerLinkedinSearch("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        }
        #endregion

        #region StartLinkedinSearch

        private void StartLinkedinSearch()
        {
            try
            {
                #region Check condition Thread is Stop or Not

                if (_IsStop_LinkedinSearch)
                {
                    return;
                }

                lstLinkedinSearchThraed.Add(Thread.CurrentThread);
                lstLinkedinSearchThraed = lstLinkedinSearchThraed.Distinct().ToList();
                Thread.CurrentThread.IsBackground = true;

                #endregion

                //foreach (KeyValuePair<string, LinkedInMaster> item in LinkedInManager.linkedInDictionary)
                {
                    try
                    {
                        if (Globals.scrapeWithoutLoggingIn)
                        {

                        }

                        if (SearchCriteria.SignIN)
                        {
                            LinkedinLogin Login = new LinkedinLogin();
                            //For Sign Out 
                            Login.LogoutHttpHelper();

                            SearchCriteria.SignOut = true;
                        }
                        //Run a separate thread for each account

                        if (SearchCriteria.SignOut)
                        {
                            LinkedInMaster item = null;
                            LinkedInManager.linkedInDictionary.TryGetValue(_LinkedinSearchSelectedEmailId, out item);

                            if (rdbLinkedinSearchKeyword.Checked)
                            {
                                //AddLoggerLinkedinSearch("[ " + DateTime.Now + " ] => [ Please Wait Login In With UserName >>> " + _LinkedinSearchSelectedEmailId + " ]");
                                item.LoginHttpHelper(ref HttpHelper);

                                if (item.IsLoggedIn)
                                {
                                    if (SearchCriteria.loginREsponce.Contains("[ " + DateTime.Now + " ] => [ Your LinkedIn account has been temporarily restricted ]"))
                                    {
                                        AddLoggerLinkedinSearch("[ " + DateTime.Now + " ] => [ " + _LinkedinSearchSelectedEmailId + "Your LinkedIn account has been temporarily restricted ]");

                                        if (rdbLinkedinSearchKeyword.Checked)
                                        {
                                            return;
                                        }
                                    }

                                    if (SearchCriteria.loginREsponce.Contains("Please confirm your email address"))
                                    {
                                        AddLoggerLinkedinSearch("[ " + DateTime.Now + " ] => [ " + _LinkedinSearchSelectedEmailId + " account has been temporarily restricted Please confirm your email address ]");

                                        if (rdbLinkedinSearchKeyword.Checked)
                                        {
                                            return;
                                        }
                                    }
                                    SearchCriteria.SignIN = true;
                                    SearchCriteria.SignOut = false;
                                    if (item.IsLoggedIn)
                                    {
                                        AddLoggerLinkedinSearch("[ " + DateTime.Now + " ] => [ Logged in Successfully With UserName >>> " + _LinkedinSearchSelectedEmailId + " ]");
                                    }
                                    else
                                    {
                                        AddLoggerLinkedinSearch("[ " + DateTime.Now + " ] => [ Couldn't Login   With UserName >>> " + _LinkedinSearchSelectedEmailId + " ]");
                                    }
                                }
                                else
                                {
                                    AddLoggerLinkedinSearch("[ " + DateTime.Now + " ] => [ Couldn't Login With UserName >>> " + _LinkedinSearchSelectedEmailId + " ]");
                                    return;
                                }
                            }

                            if (rdbLinkedinSearchKeyword.Checked)
                            {
                                AddLoggerLinkedinSearch("[ " + DateTime.Now + " ] => [ Keyword Search Process Starting..... ]");
                            }
                            if (rdbLinkedinSearchURL.Checked)
                            {
                                AddLoggerLinkedinSearch("[ " + DateTime.Now + " ] => [ Profile Search Process Starting..... ]");
                            }

                            InBoardProGetData.LinkedinSearch obj_LinkedinSearch = new InBoardProGetData.LinkedinSearch(item._Username, item._Password, item._ProxyAddress, item._ProxyPort, item._ProxyUsername, item._ProxyPassword);

                            SearchCriteria.LoginID = item._Username;

                            if (_IsKeyword_LinkedinSearch)
                            {
                                obj_LinkedinSearch._RdbKeyword = true;
                                obj_LinkedinSearch._RdbURL = false;
                            }
                            if (_IsProfileURL_LinkedinSearch)
                            {
                                obj_LinkedinSearch._RdbKeyword = false;
                                obj_LinkedinSearch._RdbURL = true;
                            }

                            obj_LinkedinSearch.logger.addToLogger += new EventHandler(LinkedinSearchLogEvents_addToLogger);

                            obj_LinkedinSearch.StartLinkedInSearch(ref HttpHelper);

                            obj_LinkedinSearch.logger.addToLogger -= new EventHandler(LinkedinSearchLogEvents_addToLogger);
                        }
                    }
                    catch
                    {
                    }
                }
            }
            catch
            {
            }
            finally
            {
                btnLinkedinSearch.Invoke(new MethodInvoker(delegate
                {
                    AddLoggerLinkedinSearch("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                    AddLoggerLinkedinSearch("------------------------------------------------------------------------------------------------------------------------------------");
                    btnLinkedinSearch.Cursor = Cursors.Default;
                }));
            }
        }

        #endregion

        #region cmbLinkedinSelectEmailId_SelectedIndexChanged
        private void cmbLinkedinSelectEmailId_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxemail.Invoke(new MethodInvoker(delegate
            {
                SearchCriteria.LoginID = cmbLinkedinSelectEmailId.SelectedItem.ToString();
            }));
        }
        #endregion

        #region btnLinkedinSearchProfileURLLoad_Click
        private void btnLinkedinSearchProfileURLLoad_Click(object sender, EventArgs e)
        {
            try
            {
                txtLinkedinSearchProfileURL.Text = "";
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "Text Files (*.txt)|*.txt";
                    ofd.InitialDirectory = Application.StartupPath;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtLinkedinSearchProfileURL.Text = ofd.FileName;
                        List<string> templist = GlobusFileHelper.ReadFile(ofd.FileName);
                        //lstEventURLsFile = new List<string>();
                        lstLinkedinSearchProfileURL.Clear();
                        foreach (string item in templist)
                        {
                            lstLinkedinSearchProfileURL.Add(item);
                        }
                        lstLinkedinSearchProfileURL = lstLinkedinSearchProfileURL.Distinct().ToList();
                        AddLoggerLinkedinSearch("[ " + DateTime.Now + " ] => [ " + lstLinkedinSearchProfileURL.Count + " Profile URLs Loaded ! ]");
                        //AddToListWallPost(lstWallMessage.Count + " Wall post message ");
                    }
                }
            }
            catch
            {
            }
        }
        #endregion

        private void rdbLinkedinSearchKeyword_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                btnLinkedinSearchProfileURLLoad.Enabled = false;
            }
            catch { }
        }

        private void rdbLinkedinSearchURL_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                btnLinkedinSearchProfileURLLoad.Enabled = true;
            }
            catch { }
        }

        private void DemoVediotoolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                frmHelp obj_frmHelp = new frmHelp();
                obj_frmHelp.Show();
            }
            catch
            {
            }
        }

        private void AboutLDtoolStripMenuItem2_Click(object sender, EventArgs e)
        {
            try
            {
                frmLicensingDetails obj_frmLicensingDtl = new frmLicensingDetails();
                obj_frmLicensingDtl.Show();
            }
            catch { }
        }

        #region btnLinkedinSearchStop_Click
        private void btnLinkedinSearchStop_Click(object sender, EventArgs e)
        {
            try
            {
                _IsStop_LinkedinSearch = false;

                List<Thread> lstTemp = lstLinkedinSearchThraed.Distinct().ToList();

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

                cmbLinkedinSearch.SelectedIndex = -1;
                txtLinkedinSearch.Text = string.Empty;
                lstLinkedinSearchProfileURL.Clear();
                InBoardProGetData.LinkedinSearch.lstLinkedinSearchProfileURL.Clear();
                AddLoggerLinkedinSearch("-------------------------------------------------------------------------------------------------------------------------------");
                AddLoggerLinkedinSearch("[ " + DateTime.Now + " ] => [ PROCESS STOPPED ]");
                AddLoggerLinkedinSearch("-------------------------------------------------------------------------------------------------------------------------------");
                btnLinkedinSearch.Cursor = Cursors.Default;
            }
            catch
            {
            }
        }
        #endregion
                
        #region CheckAccountMultiThread
        private void CheckAccountMultiThread(object obj_chk)
        {
            try
            {
                string Account = string.Empty;
                Array ArrayChkAcc = new object[3];
                ArrayChkAcc = (Array)obj_chk;
                Account = (string)ArrayChkAcc.GetValue(0);
                CheckAccount Check_Account = new CheckAccount();

                Check_Account.loggerEvent_AccountChecker.addToLogger += new EventHandler(loggerEvent_AccountChecker_addToLogger);

                Check_Account.CheckLDAccount(Account);

                Check_Account.loggerEvent_AccountChecker.addToLogger -= new EventHandler(loggerEvent_AccountChecker_addToLogger);
            }
            catch
            {
            }
        }
        #endregion

        #region loggerEvent_AccountChecker_addToLogger
        void loggerEvent_AccountChecker_addToLogger(object sender, EventArgs e)
        {
            try
            {
                if (e is EventsArgs)
                {
                    EventsArgs eargs = e as EventsArgs;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }
        #endregion

        #region btnAccountCheckerFileUpload_Click
        private void btnAccountCheckerFileUpload_Click(object sender, EventArgs e)
        {
            try
            {
                Thread thread_AccountCheckerFileUpload = new Thread(AccountCheckerFileUpload);
                thread_AccountCheckerFileUpload.Start();
            }
            catch
            {
            }
        }
        #endregion

        #region AccountCheckerFileUpload
        private void AccountCheckerFileUpload()
        {
            try
            {
                counter_GroupMemberSearch = 0;

                this.Invoke(new MethodInvoker(delegate
                {
                    using (OpenFileDialog ofd = new OpenFileDialog())
                    {
                        ofd.Filter = "Text Files (*.txt)|*.txt";
                        ofd.InitialDirectory = Application.StartupPath;
                        if (ofd.ShowDialog() == DialogResult.OK)
                        {
                            //txtAccountCheckerFile.Text = ofd.FileName;
                            lstAccountCheckerEmail.Clear();

                            List<string> templist = GlobusFileHelper.ReadFile(ofd.FileName);

                            //templist = templist.Distinct().ToList();
                            //templist.Remove("");
                            lstAccountCheckerEmail.AddRange(templist);
                            //foreach (string item in templist)
                            //{
                            //    string newItem = item.Replace(" ", "").Replace("\t", "");
                            //    if (!lstAccountCheckerEmail.Contains(item) && !string.IsNullOrEmpty(newItem))
                            //    {
                            //        lstAccountCheckerEmail.Add(item);
                            //    }
                            //}
                            //lstAccountCheckerEmail = lstAccountCheckerEmail.Distinct().ToList();
                            counter_GroupMemberSearch = lstAccountCheckerEmail.Count;
                        }
                    }
                }));
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Account Creator --> btnEmails_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Account Creator --> btnEmails_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAccountCraetionErrorLogs);
            }
        }
        #endregion
        
        #region CheckEmailMultiThread
        private void CheckEmailMultiThread(object obj_chk)
        {
            try
            {
                string Account = string.Empty;
                Array ArrayChkAcc = new object[3];
                ArrayChkAcc = (Array)obj_chk;
                Account = (string)ArrayChkAcc.GetValue(0);
                CheckAccount Check_Account = new CheckAccount();

                Check_Account.loggerEvent_AccountChecker.addToLogger += new EventHandler(loggerEvent_AccountChecker_addToLogger);
                Check_Account.CheckLDEmail(Account);
                Check_Account.loggerEvent_AccountChecker.addToLogger -= new EventHandler(loggerEvent_AccountChecker_addToLogger);
            }
            catch
            {
            }
        }
        #endregion
        
        //private string GetCsrfToken(string pgSrc)
        //{
        //    //string CsrfToken = string.Empty;
        //    //try
        //    //{
        //    //    int CsrfTokenStart = pgSrc.IndexOf("csrfToken");
        //    //    if (CsrfTokenStart > 0)
        //    //    {
        //    //        string StartCrfToken = pgSrc.Substring(CsrfTokenStart);
        //    //        int CsrfTokenEnd = StartCrfToken.IndexOf("csrfToken-coldRegistrationForm");
        //    //        string EndCrfToken = StartCrfToken.Substring(0, CsrfTokenEnd).Replace("csrfToken", "").Replace("value=", "").Replace("\"", "").Replace(" ", "").Replace("id=", "");
        //    //        CsrfToken = EndCrfToken.Replace("\\", "");
        //    //    }
        //    //}
        //    //catch(Exception ex)
        //    //{
        //    //    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Account Craetor --> GetCsrfToken() --> >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
        //    //    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Account Craetor --> GetCsrfToken() --> >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAccountCraetionErrorLogs);
        //    //}
        //    //return CsrfToken;

        //    string CsrfToken = string.Empty;
        //    try
        //    {
        //        int CsrfTokenStart = pgSrc.IndexOf("csrfToken");
        //        if (CsrfTokenStart > 0)
        //        {
        //            string StartCrfToken = pgSrc.Substring(CsrfTokenStart);
        //            int CsrfTokenEnd = StartCrfToken.IndexOf("csrfToken-login");
        //            string EndCrfToken = StartCrfToken.Substring(0, CsrfTokenEnd).Replace("csrfToken", "").Replace("value=", "").Replace("\"", "").Replace(" ", "").Replace("id=", "");
        //            CsrfToken = EndCrfToken.Replace("\\", "");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Account Craetor --> GetCsrfToken() --> >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
        //        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Account Craetor --> GetCsrfToken() --> >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAccountCraetionErrorLogs);
        //    }
        //    return CsrfToken;
        //}

        #region GetDetails
        public void GetDetails(ref string FirstName, ref string LastName, ref string pincode, ref string jobTitle, ref string companyName, ref string College)
        {
            try
            {
                if (listFirstName.Count > 0)
                {
                    try
                    {
                        FirstName = listFirstName[RandomNumberGenerator.GenerateRandom(0, listFirstName.Count)];
                    }
                    catch (Exception ex)
                    {
                        FirstName = string.Empty;
                    }
                }
                if (listLastName.Count > 0)
                {
                    try
                    {
                        LastName = listLastName[RandomNumberGenerator.GenerateRandom(0, listLastName.Count)];
                    }
                    catch (Exception ex)
                    {
                        LastName = string.Empty;
                    }
                }

                if (Pincode.Count > 0)
                {
                    try
                    {
                        pincode = Pincode[RandomNumberGenerator.GenerateRandom(0, Pincode.Count)];
                    }
                    catch (Exception ex)
                    {
                        pincode = string.Empty;
                    }
                }

                if (Jobtitle.Count > 0)
                {
                    try
                    {
                        jobTitle = Jobtitle[RandomNumberGenerator.GenerateRandom(0, Jobtitle.Count)];
                    }
                    catch (Exception ex)
                    {
                        jobTitle = string.Empty;
                    }
                }

                if (Company.Count > 0)
                {
                    try
                    {
                        companyName = Company[RandomNumberGenerator.GenerateRandom(0, Company.Count)];
                    }
                    catch (Exception ex)
                    {
                        companyName = string.Empty;
                    }
                }

                if (CollegeList.Count > 0)
                {
                    try
                    {
                        College = CollegeList[RandomNumberGenerator.GenerateRandom(0, CollegeList.Count)];
                    }
                    catch (Exception ex)
                    {
                        College = string.Empty;
                    }
                }
            }
            catch
            {
            }
        }
        #endregion

        #region GetDetails1By1
        public void GetDetails1By1(ref string FirstName, ref string LastName, ref string pincode, ref string jobTitle, ref string companyName, ref string College)
        {
            try
            {
                if (listFirstName.Count > 0)
                {
                    foreach (var itemFirstName in listFirstName)
                    {
                        try
                        {
                            FirstName = itemFirstName;
                        }
                        catch (Exception ex)
                        {
                            FirstName = string.Empty;
                        }
                    }

                }
                if (listLastName.Count > 0)
                {

                    foreach (var itemLastName in listLastName)
                    {
                        try
                        {
                            LastName = itemLastName;
                        }
                        catch (Exception ex)
                        {
                            LastName = string.Empty;
                        }
                    }

                }

                if (Pincode.Count > 0)
                {
                    foreach (var itemPinCode in Pincode)
                    {
                        try
                        {
                            pincode = itemPinCode;
                        }
                        catch (Exception ex)
                        {
                            pincode = string.Empty;
                        }
                    }
                }

                if (Jobtitle.Count > 0)
                {
                    foreach (var itemJobTitle in Jobtitle)
                    {
                        try
                        {
                            jobTitle = itemJobTitle;
                        }
                        catch (Exception ex)
                        {
                            jobTitle = string.Empty;
                        }

                    }
                }

                if (Company.Count > 0)
                {
                    foreach (var itemCompany in Company)
                    {
                        try
                        {
                            companyName = itemCompany;
                        }
                        catch (Exception ex)
                        {
                            companyName = string.Empty;
                        }
                    }
                }

                if (CollegeList.Count > 0)
                {
                    foreach (var itemCollege in CollegeList)
                    {
                        try
                        {
                            College = itemCollege;
                        }
                        catch (Exception ex)
                        {
                            College = string.Empty;
                        }
                    }
                }
            }
            catch
            {
            }
        }
        #endregion

        #region GetCaptch
        private string GetCaptch(string TempCreatingAccountPageSource)
        {
            string captcha = string.Empty;
            try
            {
                string GetURLforcaptcha = string.Empty;
                string CaptchaChallenge = string.Empty;
                string proxyAddress = string.Empty;
                string ProxyPort = "0";
                string proxyUsername = string.Empty;
                string proxyPassword = string.Empty;

                GlobusHttpHelper GlobusHttpHelper = new GlobusHttpHelper();

                #region GetURLCapcha

                if (TempCreatingAccountPageSource.Contains("https://www.google.com/recaptcha/api/challenge"))
                {
                    try
                    {
                        string CaptchaReq = TempCreatingAccountPageSource.Substring(TempCreatingAccountPageSource.IndexOf("https://www.google.com/recaptcha/api/challenge"), 120);
                        string[] CaptchaReqIDArrry = CaptchaReq.Split('"');
                        GetURLforcaptcha = CaptchaReqIDArrry[0];
                    }
                    catch
                    {
                    }
                }

                #endregion

                #region CapchaChallenge

                if (!string.IsNullOrEmpty(GetURLforcaptcha))
                {
                    string PagesourceForCapchafterGetreq = GlobusHttpHelper.getHtmlfromUrlProxy(new Uri(GetURLforcaptcha), proxyAddress, Convert.ToInt32(ProxyPort), proxyUsername, proxyPassword);
                    if (PagesourceForCapchafterGetreq.Contains("challenge :"))
                    {
                        try
                        {
                            int startIndx = PagesourceForCapchafterGetreq.IndexOf("challenge :") + "challenge :".Length;
                            int endIndx = PagesourceForCapchafterGetreq.IndexOf("'", startIndx + 10);
                            CaptchaChallenge = PagesourceForCapchafterGetreq.Substring(startIndx, endIndx - startIndx).Replace("'", "").Trim();
                            //string CaptchaSecondReq = PagesourceForCapchafterGetreq.Substring(PagesourceForCapchafterGetreq.IndexOf("challenge :"), PagesourceForCapchafterGetreq.IndexOf("'", PagesourceForCapchafterGetreq.IndexOf("challenge :") + 10));
                            ////string[] CaptchaSecondReqIDArrry = CaptchaSecondReq.Split(',');
                            //CaptchaChallenge = CaptchaSecondReq.Replace("challenge : ", "").Replace("'", "");
                        }
                        catch
                        {
                        }
                    }
                }

                #endregion

                if (!string.IsNullOrEmpty(GetURLforcaptcha))
                {

                    #region For With Capcha Account Creation

                    try
                    {
                        string abc = "https://www.google.com/recaptcha/api/image?c=" + CaptchaChallenge;
                        //string TepGetCapcha = GlobusHttpHelper.getHtmlfromUrlProxy(new Uri("https://www.google.com/recaptcha/api/image?c="+CaptchaChallenge), proxyAddress, ProxyPort, proxyUsername, proxyPassword);
                        //for decaptcha..

                        int count_refreshcaptcha = 0;

                        WebClient webclient = new WebClient();
                        byte[] args = webclient.DownloadData(abc);

                        string captchaText = string.Empty;

                        string[] arr1 = new string[] { Globals.CapchaLoginID, Globals.CapchaLoginPassword, "" };


                        captchaText = DecodeDBC(arr1, args);

                        captcha = captchaText;
                    }
                    catch
                    {
                    }

                    #endregion

                }
            }
            catch
            {
            }

            return captcha;
        }
        #endregion

        #region DecodeDBC
        public string DecodeDBC(string[] args, byte[] imageBytes)
        {
            try
            {
                // Put your DBC username & password here:
                DeathByCaptcha.Client client = new DeathByCaptcha.SocketClient(args[0], args[1]);
                client.Verbose = true;

                Console.WriteLine("Your balance is {0:F2} US cents", client.Balance);

                if (!client.User.LoggedIn)
                {
                    // Log("Please check your DBC Account Details");
                    return null;
                }
                if (client.Balance == 0.0)
                {
                    // Log("You have 0 Balance in your DBC Account");
                    return null;
                }

                for (int i = 2, l = args.Length; i < l; i++)
                {
                    Console.WriteLine("Solving CAPTCHA {0}", args[i]);

                    // Upload a CAPTCHA and poll for its status.  Put the CAPTCHA image
                    // file name, file object, stream, or a vector of bytes, and desired
                    // solving timeout (in seconds) here:
                    DeathByCaptcha.Captcha captcha = client.Decode(imageBytes, 2 * DeathByCaptcha.Client.DefaultTimeout);
                    if (null != captcha)
                    {
                        Console.WriteLine("CAPTCHA {0:D} solved: {1}", captcha.Id, captcha.Text);

                        //// Report an incorrectly solved CAPTCHA.
                        //// Make sure the CAPTCHA was in fact incorrectly solved, do not
                        //// just report it at random, or you might be banned as abuser.
                        //if (client.Report(captcha))
                        //{
                        //    Console.WriteLine("Reported as incorrectly solved");
                        //}
                        //else
                        //{
                        //    Console.WriteLine("Failed reporting as incorrectly solved");
                        //}

                        return captcha.Text;
                    }
                    else
                    {
                        // Log("CAPTCHA was not solved");
                        Console.WriteLine("CAPTCHA was not solved");
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> LinkedinSignup -  SignupMultiThreaded() -- DecodeDBC()  --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("Error --> LinkedinSignup -  DecodeDBC() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + " || DateTime :- " + DateTime.Now, Globals.Path_LinkedinAccountCraetionErrorLogs);
            }
            return null;
        }
        #endregion

        //private void SetProfileForEmployee()
        //{
        //    try
        //    {
        //        string Flow = GetFlow(UseCapchaCreatingAccountPageSource);

        //        #region SourceAliasNewEmoply
        //        try
        //        {
        //            int SourceAliasStart = UseCapchaCreatingAccountPageSource.IndexOf("sourceAlias");
        //            if (SourceAliasStart > 0)
        //            {
        //                string StartSourceAlias = UseCapchaCreatingAccountPageSource.Substring(SourceAliasStart);
        //                if (StartSourceAlias.Contains("id=\"sourceAlias-employedProfileForm\""))
        //                {
        //                    try
        //                    {
        //                        int SourceAliasEnd = StartSourceAlias.IndexOf("id=\"sourceAlias-employedProfileForm\"");
        //                        string EndSourceAlias = StartSourceAlias.Substring(0, SourceAliasEnd).Replace("sourceAlias", "").Replace("value=", "").Replace("\"", "").Replace(" ", "").Replace("id=", "");
        //                        SourceAliasNewEmoply = EndSourceAlias.Replace("\\", "");
        //                    }
        //                    catch
        //                    {
        //                    }
        //                }
        //                //else
        //                //{
        //                //    try
        //                //    {
        //                //        int SourceAliasEnd = StartSourceAlias.IndexOf("id=");
        //                //        string EndSourceAlias = StartSourceAlias.Substring(StartSourceAlias.IndexOf("value="), SourceAliasEnd).Replace("sourceAlias", "").Replace("value=", "").Replace("\"", "").Replace(" ", "").Replace("id=", "");
        //                //        SourceAliasNewEmoply = EndSourceAlias.Replace("\\", "");
        //                //    }
        //                //    catch
        //                //    {
        //                //    }
        //                //}

        //            }

        //        }
        //        catch (Exception ex)
        //        {
        //            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Account Craetor --> GetSourceAlias() --> >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
        //            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Account Craetor --> GetSourceAlias() --> >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAccountCraetionErrorLogs);
        //        }
        //        #endregion

        //        if (AccountType == "Employed")
        //        {
        //            #region Employed type
        //            Log("Creating Account With Type : Employed");
        //            try
        //            {

        //                string CreatingAccountPageSource = string.Empty;
        //                string page = string.Empty;
        //                //changes for Account creator
        //                // string postdata = "phonFirst=&phonLast=&countryCode=" + countryValue + "&postalCode=" + Pincode + "&status-chooser=" + AccountType + "&workCompanyTitle=" + JobTitle + "&companyName=" + Company + "&industryChooser=" + IndustryValue + "&jsEnabled=&fandango=&companyID=&csrfToken=" + CsrfToken + "&sourceAlias=" + SourceAliasNewEmoply + "&flow=1ofgqp7-16k61ld" + Flow;
        //                string AfterCapchaPostUrl = "https://www.linkedin.com/reg/basic-profile-create-ne";
        //                string AfterCapchaNewPostData = "phonFirst=&phonLast=&countryCode=" + countryValue + "&postalCode=" + Pincode + "&status-chooser=" + AccountType + "&workCompanyTitle=" + HttpUtility.HtmlEncode(JobTitle) + "&companyName=" + HttpUtility.HtmlEncode(IndustryValue) + "&industryChooser=&jsEnabled=&fandango=&companyID=&csrfToken=" + CsrfToken + "&sourceAlias=" + SourceAliasNewEmoply + "&flow=" + Flow;
        //                string REsponceAfterCapcha = GlobusHttpHelper.postFormDataProxy(new Uri(AfterCapchaPostUrl), AfterCapchaNewPostData, proxyAddress, ProxyPort, proxyUsername, proxyPassword);
        //                page = GlobusHttpHelper.getHtmlfromUrlProxy(new Uri("https://www.linkedin.com"), proxyAddress, ProxyPort, proxyUsername, proxyPassword);
        //                //

        //                //string postdata = "phonFirst=&phonLast=&countryCode=" + countryValue + "&postalCode=" + Pincode + "&status-chooser=" + AccountType + "&workCompanyTitle=" + JobTitle + "&companyName=" + Company + "&industryChooser=" + IndustryValue + "&jsEnabled=&fandango=&companyID=&csrfToken=" + CsrfToken + "&sourceAlias=" + SourceAliasNewEmoply + "&flow=1ofgqp7-16k61ld" + Flow;

        //                string postData = "isJsEnabled=true&firstName=" + HttpUtility.UrlEncode(FirstName) + "&lastName=" + HttpUtility.UrlEncode(LastName) + "&email=" + HttpUtility.UrlEncode(Email) + "&password=" + HttpUtility.UrlEncode(Password) + "&webmailImport=true&trcode=hb_join&key=&authToken=&authType=&genie-reg=&csrfToken=" + CsrfToken + "&sourceAlias=" + SourceAlias;

        //                CreatingAccountPageSource = GlobusHttpHelper.postFormDataProxy(new Uri("https://www.linkedin.com/reg/join-create"), postData, proxyAddress, ProxyPort, proxyUsername, proxyPassword);

        //                if (page.Contains("Please confirm your email address. We've sent a confirmation link to"))
        //                {
        //                    Log("Account created With Email : " + Email);

        //                    ClsEmailActivator EmailActivator = new ClsEmailActivator();
        //                    bool Verified = EmailActivator.EmailVerification(Email, Password, ref GlobusHttpHelper);

        //                    if (Verified)
        //                    {
        //                        Log("Email : " + Email + " Verified");
        //                    }
        //                    else if (!Verified)
        //                    {
        //                        Log("Email : " + Email + " Not Verified");
        //                    }

        //                    GlobusFileHelper.AppendStringToTextfileNewLine(Email + ":" + Password + ":" + proxyAddress + ":" + ProxyPort + ":" + proxyUsername + ":" + proxyPassword, Globals.path_CreatedAccounts);

        //                    string AccountQuery = "INSERT INTO tb_LinkedInAccount (username , password) VALUES ('" + Email + "','" + Password + "')";
        //                    DataBaseHandler.InsertQuery(AccountQuery, "tb_LinkedInAccount");
        //                    string Query = "INSERT INTO tb_profileDetails (username , password , country , pincode , AccountType , JobTitle , Company , Industry) VALUES ('" + Email + "' , '" + Password + "' , '" + countryName + "' , '" + Pincode + "' , '" + AccountType + "' , '" + JobTitle + "' , '" + Company + "' , '" + IndustryName + "')";
        //                    DataBaseHandler.InsertQuery(Query, "tb_profileDetails");
        //                }
        //                else
        //                {
        //                    // Not Created Accounts
        //                    GlobusFileHelper.AppendStringToTextfileNewLine(Email + ":" + Password + ":" + proxyAddress + ":" + ProxyPort + ":" + proxyUsername + ":" + proxyPassword, Globals.path_NonCreatedAccounts);

        //                    Log("Email : " + Email + " Couldn't Created  !");
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Account Craetor --> StartAccountCreator() --> Account Type Employed >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
        //                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Account Craetor --> StartAccountCreator() --> Account Type Employed >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAccountCraetionErrorLogs);

        //                // Not Created Accounts
        //                GlobusFileHelper.AppendStringToTextfileNewLine(Email + ":" + Password + ":" + proxyAddress + ":" + ProxyPort + ":" + proxyUsername + ":" + proxyPassword, Globals.path_NonCreatedAccounts);

        //                Log("Email : " + Email + " Couldn't Created  !");
        //            }
        //            #endregion
        //        }

        //    }
        //    catch
        //    {
        //    }
        //}

        #region GetFlow
        private string GetFlow(string pgSrc)
        {
            string SourceGetFlow = string.Empty;
            try
            {
                int SourceGetFlowStart = pgSrc.IndexOf("flow");
                if (SourceGetFlowStart > 0)
                {
                    string StartSourceGetFlow = pgSrc.Substring(SourceGetFlowStart);
                    int SourceGetFlowEnd = StartSourceGetFlow.IndexOf(" id=\"flow-studentProfileForm");
                    int SourceGetFlowEndSecond = StartSourceGetFlow.IndexOf("class=\"");
                    if (SourceGetFlowEnd > 0)
                    {
                        string EndGetFlow = StartSourceGetFlow.Substring(0, SourceGetFlowEnd).Replace("-employedProfileForm", "").Replace("value=", "").Replace("flow", "").Replace("id=", "").Replace("\"", "").Replace(" ", "");
                        string LastSOurceFlow = EndGetFlow.Remove(EndGetFlow.IndexOf(">"));
                        SourceGetFlow = LastSOurceFlow;
                    }
                    else if (SourceGetFlowEndSecond > 0)
                    {
                        string EndGetFlow = StartSourceGetFlow.Substring(0, SourceGetFlowEndSecond).Replace("class=\"", "").Replace("value=", "").Replace("flow", "").Replace("id=", "").Replace("\"", "").Replace(" ", "");
                        string LastSOurceFlow = EndGetFlow.Replace("=", "");
                        SourceGetFlow = LastSOurceFlow;
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Account Craetor --> GetFlow() --> >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Account Craetor --> GetFlow() --> >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAccountCraetionErrorLogs);
            }
            return SourceGetFlow;
        }
        #endregion
        
        #region SetProxy
        public void SetProxy(string servername, string portnumber)
        {
            try
            {
                string key = "Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings";
                string serverName = servername;//your proxy server name;

                string port = portnumber; //your proxy port;

                string proxy = serverName + ":" + port;

                RegistryKey RegKey = Registry.CurrentUser.OpenSubKey(key, true);

                RegKey.SetValue("ProxyServer", proxy);

                RegKey.SetValue("ProxyEnable", 1);
                
                Thread.Sleep(min * 1000);
            }
            catch
            {
            }
        }
        #endregion
        
        #region linkedInProfileManagerToolStripMenuItem_Click
        private void linkedInProfileManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                FrmLinkedInImageUploader obj_FrmLinkedInProfileManager = new FrmLinkedInImageUploader();
                obj_FrmLinkedInProfileManager.Show();
            }
            catch
            {
            }
        }
        #endregion
        
        #region chkSelectMembers_JoinFriendsGrp_CheckedChanged
        private void chkSelectMembers_JoinFriendsGrp_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkSelectMembers_JoinFriendsGrp.Checked)
                {
                    for (int i = 0; i < chkMembers.Items.Count; i++)
                    {
                        try
                        {
                            string aaa = Convert.ToString(chkMembers.Items[i]);
                            chkMembers.SetItemChecked(i, true);
                            lblTotMemCount.Text = "(" + chkMembers.Items.Count + ")";
                        }
                        catch { }
                    }
                }
                else
                {
                    for (int i = 0; i < chkMembers.Items.Count; i++)
                    {
                        try
                        {
                            string aaa = Convert.ToString(chkMembers.Items[i]);
                            chkMembers.SetItemChecked(i, false);
                            int TotSelectedFrinedList = 0;
                            lblTotMemCount.Text = "(" + TotSelectedFrinedList + ")";
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

        #region chkSelectExistGrps_JoinFriendsGrp_CheckedChanged
        private void chkSelectExistGrps_JoinFriendsGrp_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkSelectExistGrps_JoinFriendsGrp.Checked)
                {
                    for (int i = 0; i < chkExistGroup.Items.Count; i++)
                    {
                        try
                        {
                            string aaa = Convert.ToString(chkExistGroup.Items[i]);
                            chkExistGroup.SetItemChecked(i, true);
                            lblTotGroups.Text = "(" + chkExistGroup.Items.Count + ")";
                        }
                        catch { }
                    }
                }
                else
                {
                    for (int i = 0; i < chkExistGroup.Items.Count; i++)
                    {
                        try
                        {
                            string aaa = Convert.ToString(chkExistGroup.Items[i]);
                            chkExistGroup.SetItemChecked(i, false);
                            int TotSelectedGroups = 0;
                            lblTotGroups.Text = "(" + TotSelectedGroups + ")";
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

        #region chkExistGroup_SelectedIndexChanged
        private void chkExistGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int count = Convert.ToInt32(lblTotGroups.Text.Replace("(", string.Empty).Replace(")", string.Empty));
                int count2 = chkExistGroup.CheckedItems.Count + 1;
                if (chkExistGroup.CheckOnClick == true)
                {
                    if (count < count2)
                    {
                        lblTotGroups.Text = "(" + (count + 1).ToString() + ")";
                    }
                    else
                    {
                        lblTotGroups.Text = "(" + (count - 1).ToString() + ")";
                    }
                }
            }
            catch { }
        }
        #endregion

        #region linkedInAddExperienceFieldToolStripMenuItem_Click
        private void linkedInAddExperienceFieldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                FrmAddExperience objExperience = new FrmAddExperience();
                objExperience.Show();
            }
            catch { }
        }
        #endregion

        #region MyReBtnSentInvitation_CreateGroup_Clickgion
        private void BtnSentInvitation_CreateGroup_Click(object sender, EventArgs e)
        {
            try
            {
                lstworkingThread.Clear();

                if (IsStop)
                {
                    IsStop = false;
                }

                if (LinkedInManager.linkedInDictionary.Count() > 0)
                {

                }
                if (LinkedInManager.linkedInDictionary.Count() == 0)
                {
                    MessageBox.Show("Please Load LinkedIn Accounts From MyAccounts Menu");
                    AddLoggerCreateGroup("[ " + DateTime.Now + " ] => [ Please Load LinkedIn Accounts From MyAccounts Menu ]");

                    lbGeneralLogs.Items.Clear();
                    frmAccounts FrmAccount = new frmAccounts();
                    FrmAccount.Show();
                    return;
                }

                if (listGroupURLs_CreateGroup.Count > 0)
                {

                    Groups.CreateGroup.LstGroupUrls = listGroupURLs_CreateGroup;
                }
                else
                {
                    MessageBox.Show("Please Upload Group URLs !");
                    return;
                }

                Thread AddFriendsInYrGroup_Thread = new Thread(LinkdinAddFriendsInYrGroup_CreateGroup);

                AddFriendsInYrGroup_Thread.Start();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error >>> " + ex.StackTrace);
            }
        }
        #endregion

        #region LinkdinAddFriendsInYrGroup_CreateGroup
        private void LinkdinAddFriendsInYrGroup_CreateGroup()
        {
            try
            {
                int numberofThreds = 0;
                counter_GroupMemberSearch = LinkedInManager.linkedInDictionary.Count;

                if (LinkedInManager.linkedInDictionary.Count() > 0)
                {
                    ThreadPool.SetMaxThreads(numberofThreds, 5);

                    try
                    {
                        foreach (KeyValuePair<string, LinkedInMaster> item in LinkedInManager.linkedInDictionary)
                        {
                            ThreadPool.SetMaxThreads(5, 5);

                            // ThreadPool.QueueUserWorkItem(new WaitCallback(LinkdinMulThreadAddFriendsInYrGroup_CreateGroup), new object[] { item });

                            LinkdinMulThreadAddFriendsInYrGroup_CreateGroup(new object[] { item });

                            Thread.Sleep(1000);
                        }
                    }
                    catch (Exception ex)
                    {
                        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Create Group --> LinkdinCreateGroup() ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Create Group --> LinkdinCreateGroup() ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinCreateGroupErrorLogs);
                    }
                }
                else
                {
                    AddLoggerCreateGroup("[ " + DateTime.Now + " ] => [ Please Load LinkedIn Accounts From MyAccounts Menu ]");
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Create Group --> LinkdinCreateGroup() ---2--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Create Group --> LinkdinCreateGroup() ---2--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinCreateGroupErrorLogs);
            }
        }
        #endregion

        #region LinkdinMulThreadAddFriendsInYrGroup_CreateGroup
        public void LinkdinMulThreadAddFriendsInYrGroup_CreateGroup(object Parameter)
        {
            try
            {

                if (IsStop)
                {
                    return;
                }
                if (!IsStop)
                {
                    lstworkingThread.Add(Thread.CurrentThread);
                    lstworkingThread.Distinct();
                    Thread.CurrentThread.IsBackground = true;
                }

                string Account = string.Empty;
                try
                {
                    int CountPerAccount = 2;
                    if (!string.IsNullOrEmpty(txtCountPerAccount.Text) && NumberHelper.ValidateNumber(txtCountPerAccount.Text))
                    {
                        CountPerAccount = Convert.ToInt32(txtCountPerAccount.Text);
                    }

                    Array paramsArray = new object[1];

                    paramsArray = (Array)Parameter;

                    KeyValuePair<string, LinkedInMaster> item = (KeyValuePair<string, LinkedInMaster>)paramsArray.GetValue(0);
                    Account = item.Key;
                    GlobusHttpHelper HttpHelper = new GlobusHttpHelper();
                    LinkedinLogin Login = new LinkedinLogin();
                    Login.accountUser = item.Key;
                    Login.accountPass = item.Value._Password;
                    Login.proxyAddress = item.Value._ProxyAddress;
                    Login.proxyPort = item.Value._ProxyPort;
                    Login.proxyUserName = item.Value._ProxyUsername;
                    Login.proxyPassword = item.Value._ProxyPassword;

                    Groups.CreateGroup obj_CreateGroup = new Groups.CreateGroup(Login.accountUser, Login.accountPass, Login.proxyAddress, Login.proxyPort, Login.proxyUserName, Login.proxyPassword);

                    Login.logger.addToLogger += new EventHandler(logger_addGroupCreateToLogger);
                    obj_CreateGroup.logger.addToLogger += new EventHandler(logger_addGroupCreateToLogger);

                    if (!Login.IsLoggedIn)
                    {
                        Login.LoginHttpHelper(ref HttpHelper);
                    }

                    if (Login.IsLoggedIn)
                    {
                        // while (CountPerAccount > 0)
                        {


                            //Login.CreateGroup(ref HttpHelper);

                            obj_CreateGroup.StartSendInvitation(ref HttpHelper);
                            CountPerAccount--;

                            Login.logger.addToLogger -= new EventHandler(logger_addGroupCreateToLogger);
                            obj_CreateGroup.logger.addToLogger -= new EventHandler(logger_addGroupCreateToLogger);
                        }
                    }
                    else
                    {
                        AddLoggerCreateGroup("[ " + DateTime.Now + " ] => [ Could Not Login With Id: " + Account + " ]");
                    }
                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Create Group --> PostCreateGroup() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Create Group --> PostCreateGroup() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinCreateGroupErrorLogs);
                }
                finally
                {
                    AddLoggerCreateGroup("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED For :" + Account + " ]");
                    AddLoggerCreateGroup("------------------------------------------------------------------------------------------------------------------------------------");

                    counter_group_creator--;

                    if (counter_group_creator == 0)
                    {
                        if (btnCreateOpenGroup.InvokeRequired)
                        {
                            btnCreateOpenGroup.Invoke(new MethodInvoker(delegate
                            {
                                //pictureBox6Red.Visible = false;
                                //pictureBox6Green.Visible = true;
                                //AddLoggerCreateGroup("Process Completed..");
                            }));
                        }
                    }
                }
            }
            catch
            {
            }
        }
        #endregion

        #region BtnUploadGroupURLs_CreateGroup_Click
        private void BtnUploadGroupURLs_CreateGroup_Click(object sender, EventArgs e)
        {
            try
            {

                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "Text Files (*.txt)|*.txt";
                    ofd.InitialDirectory = Application.StartupPath;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        TxtGroupUrls_CreateGroup.Text = ofd.FileName;
                        listGroupURLs_CreateGroup = new List<string>();
                        listGroupURLs_CreateGroup.Clear();

                        listGroupURLs_CreateGroup = GlobusFileHelper.ReadFiletoStringList(ofd.FileName);

                        listGroupURLs_CreateGroup = listGroupURLs_CreateGroup.Distinct().ToList();

                        AddLoggerCreateGroup("[ " + DateTime.Now + " ] => [ " + listGroupURLs_CreateGroup.Count + " Group Names Added ]");
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Create Group --> btnGrpName_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Create Group --> btnGrpName_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinCreateGroupErrorLogs);
            }
        }
        #endregion
        
        #region dBCSettingsToolStripMenuItem_Click
        private void dBCSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                FrmCaptchaLogin CapchaAcc = new FrmCaptchaLogin();
                //lbGeneralLogs.Items.Clear();
                CapchaAcc.ShowDialog();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }
        #endregion

        #region downloadRequiredFilesToolStripMenuItem_Click
        private void downloadRequiredFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("iexplore", "http://faced.extrem-hosting.net/LDFiles.zip");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }
        #endregion

        #region accountVerificationToolStripMenuItem_Click
        private void accountVerificationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                FrmAccountVerification obj_FrmAccountVerification = new FrmAccountVerification();
                obj_FrmAccountVerification.Show();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error >>> " + ex.StackTrace);
            }

        }
        #endregion
        
        #region BtnTestPrivateProxy_ProxySetting_Click
        private void BtnTestPrivateProxy_ProxySetting_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {
                IsStop_Proxy = false;
                lstStopPrivateProxyTest.Clear();
                try
                {
                    AddToProxysLogs("[ " + DateTime.Now + " ] => [ Exsisting Proxies Saved To : ]");
                    AddToProxysLogs("[ " + DateTime.Now + " ] => [ " + Globals.Path_ExsistingProxies + " ]");

                    if (!string.IsNullOrEmpty(txtPvtProxyFour.Text))
                    {
                        // lstProxies = BaseLib.GlobusFileHelper.ReadFiletoStringLst(txtPublicProxy.Text);
                        if (lstValidProxyList.Count == 0)
                        {
                            AddToProxysLogs("[ " + DateTime.Now + " ] => [ Please Select a text File With Private Proxies ]");
                            return;
                        }

                        AddToProxysLogs("[ " + DateTime.Now + " ] => [ " + lstValidProxyList.Count() + " Private Proxies Uploaded ]");
                        Proxystatus = lstValidProxyList.Count;

                        new Thread(() =>
                        {
                            GetPrivateValidProxies_ProxySetting(lstValidProxyList);
                        }).Start();
                    }
                    else
                    {
                        AddToProxysLogs("[ " + DateTime.Now + " ] => [ Please Select a text File With Private Proxies ]");
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
                AddToProxysLogs("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        }
        #endregion
        
        #region btnStopmanageconnection_Click
        private void btnStopmanageconnection_Click(object sender, EventArgs e)
        {
            try
            {
                IsStop = true;
                List<Thread> lstTemp = lstSearchconnectionThread.Distinct().ToList();
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
                AddLoggerManageConnection("-------------------------------------------------------------------------------------------------------------------------------");
                AddLoggerManageConnection("[ " + DateTime.Now + " ] => [ PROCESS STOPPED ]");
                AddLoggerManageConnection("-------------------------------------------------------------------------------------------------------------------------------");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error >>> " + ex.StackTrace);
            }


        }
        #endregion

        #region MyRelbtnInviteMemProfUrl_LinkClickedgion
        private void lbtnInviteMemProfUrl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FrmInviteMemberThroughProfileURL obj_FrmInviteMemberThroughProfileURL = new FrmInviteMemberThroughProfileURL();
            obj_FrmInviteMemberThroughProfileURL.Show();
        }
        #endregion

        #region btnstopCreateGroup_Click
        private void btnstopCreateGroup_Click(object sender, EventArgs e)
        {
            try
            {
                IsStop = true;
                List<Thread> lstTemp = lstworkingThread.Distinct().ToList();
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
                AddLoggerCreateGroup("-------------------------------------------------------------------------------------------------------------------------------");
                AddLoggerCreateGroup("[ " + DateTime.Now + " ] => [ PROCESS STOPPED ]");
                AddLoggerCreateGroup("-------------------------------------------------------------------------------------------------------------------------------");
                btnCreateOpenGroup.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error >>> " + ex.StackTrace);
            }

        }
        #endregion

        #region btnStopcomposemsg_Click
        private void btnStopcomposemsg_Click(object sender, EventArgs e)
        {
            try
            {
                Globals.IsStop = true;
                //IsStopMessage = true;
                List<Thread> lstTemp = Globals.lstComposeMessageThread.Distinct().ToList();
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
                AddLoggerComposeMessage("-------------------------------------------------------------------------------------------------------------------------------");
                AddLoggerComposeMessage("[ " + DateTime.Now + " ] => [ PROCESS STOPPED ]");
                AddLoggerComposeMessage("-------------------------------------------------------------------------------------------------------------------------------");
            }

            catch (Exception ex)
            {
                Console.WriteLine("Error >>> " + ex.StackTrace);
            }
        }
        #endregion

        #region acceptToolStripMenuItem_Click
        private void acceptToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region txtTitle_TextChanged
        private void txtTitle_TextChanged(object sender, EventArgs e)
        {
            if (txtTitle.Text != "")
            {
                cmbboxTitle.Enabled = true;
            }
            else
            {
                cmbboxTitle.Enabled = false;
            }
        }
        #endregion

        #region txtPostalcode_TextChanged
        private void txtPostalcode_TextChanged(object sender, EventArgs e)
        {
            if (txtPostalcode.Text != "")
            {
                cmbboxWithin.Enabled = true;
            }
            else
            {
                cmbboxWithin.Enabled = false;
            }
        }
        #endregion

        #region txtCompnayName_TextChanged
        private void txtCompnayName_TextChanged(object sender, EventArgs e)
        {
            if (txtCompnayName.Text != "")
            {
                cmbboxCompanyValue.Enabled = true;
            }
            else
            {
                cmbboxCompanyValue.Enabled = false;
            }
        }
        #endregion

        #region btnUploadUserids_Click
        private void btnUploadUserids_Click(object sender, EventArgs e)
        {
            #region old ommented code
            //try
            //{
            //    using (OpenFileDialog ofd = new OpenFileDialog())
            //    {
            //        ofd.Filter = "Text Files (*.txt)|*.txt";
            //        if (ofd.ShowDialog() == DialogResult.OK)
            //        {
            //            txtEndorseUpload.Text = ofd.FileName;
            //            List<string> templist = GlobusFileHelper.ReadFile(ofd.FileName);
            //            lstEndorseUserIds.Clear();
            //            foreach (string item in templist)
            //            {

            //               bool res = NumberHelper.ValidateNumber(item);

            //                string newItem = item.Replace(" ", "").Replace("\t", "");
            //                if (res)
            //                {
            //                    if (!lstEndorseUserIds.Contains(item) && !string.IsNullOrEmpty(newItem))
            //                    {
            //                        lstEndorseUserIds.Add(item);
            //                    }
            //                }
            //                else
            //                {
            //                    AddLoggerEndorsePeople("[ " + DateTime.Now + " ] => [ Please enter a valid User ID ]");
            //                }
            //            }
            //            AddLoggerEndorsePeople("[ " + DateTime.Now + " ] => [ " + lstEndorseUserIds.Count + " UserIds Uploaded ]");
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error -->btnUploadUserids_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
            //    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnUploadUserids_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.path_EndorsePeople);
            //} 
            #endregion

            try
            {
                clsEmailParser objparser = new clsEmailParser();
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    GroupStatus.Cmpmsg_excelData.Clear();
                    ofd.InitialDirectory = System.Windows.Forms.Application.StartupPath;
                    ofd.Filter = "Text Files (*.xlsx)|*.xlsx";
                    AddLoggerEndorsePeople("[ " + DateTime.Now + " ] => [ Add Excel Input file please wait... ]");

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtEndorseUpload.Text = ofd.FileName;
                        EndorsePeople.Endorse_excelData = objparser.parseExcel(txtEndorseUpload.Text);
                        AddLoggerEndorsePeople("[ " + DateTime.Now + " ] => [ Add Excel Input file completed... ]");
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error -->btnUploadUserids_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnUploadUserids_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.path_EndorsePeople);
            }
        }
        #endregion

        #region btnstartEndorse_Click
        private void btnstartEndorse_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {

                lstEndorsementThread.Clear();

                if (IsStop)
                {
                    IsStop = false;
                }
                //EndorsePeople.Endorse_excelData
                AddLoggerEndorsePeople("[ " + DateTime.Now + " ] => [ Please Wait. Starting process.. ]");
                foreach (string[] itemArr in EndorsePeople.Endorse_excelData)
                {
                    try
                    {

                        try
                        {
                            if (!string.IsNullOrEmpty(itemArr[0]))
                            {
                                EndorsePeople.xls_Email = itemArr[0];
                            }
                        }
                        catch { }
                        try
                        {
                            if (!string.IsNullOrEmpty(itemArr[1]))
                            {
                                EndorsePeople.xls_UserId = itemArr[1];
                            }
                        }
                        catch { }
                    }
                    catch { }



                    if (EndorsePeople.xls_Email.Count() > 0 && EndorsePeople.xls_UserId.Count() > 0 && LinkedInManager.linkedInDictionary.Count > 0 && (!string.IsNullOrEmpty(txtEndorsmentMinDelay.Text) && !string.IsNullOrEmpty(txtEndorsmentMaxDelay.Text)
                       && NumberHelper.ValidateNumber(txtEndorsmentMinDelay.Text) && NumberHelper.ValidateNumber(txtEndorsmentMaxDelay.Text)))
                    {
                        
                        btnstartEndorse.Cursor = Cursors.AppStarting;
                        Thread thread_Endorsment = new Thread(StartEndorsingPeople);
                        thread_Endorsment.Start();
                    }
                    else
                    {
                        if (EndorsePeople.xls_Email.Count() == 0)
                        {
                            AddLoggerEndorsePeople("[ " + DateTime.Now + " ] => [ Please Upload Email ID ]");
                        }
                        else if (EndorsePeople.xls_UserId.Count() == 0)
                        {
                            AddLoggerEndorsePeople("[ " + DateTime.Now + " ] => [ Please Upload User ID ]");
                        }
                        else if (LinkedInManager.linkedInDictionary.Count == 0)
                        {
                            AddLoggerEndorsePeople("[ " + DateTime.Now + " ] => [ Please Upload Accounts ]");
                        }
                        else if (string.IsNullOrEmpty(txtEndorsmentMinDelay.Text))
                        {
                            AddLoggerEndorsePeople("[ " + DateTime.Now + " ] => [ Please Enter Minimum Delay ]");
                            MessageBox.Show("Please Enter Minimum Delay");
                        }
                        else if (string.IsNullOrEmpty(txtEndorsmentMaxDelay.Text))
                        {
                            AddLoggerEndorsePeople("[ " + DateTime.Now + " ] => [ Please Enter Maximum Delay ]");
                            MessageBox.Show("Please Enter Maximum Delay");
                        }
                        else if (!NumberHelper.ValidateNumber(txtEndorsmentMinDelay.Text))
                        {
                            AddLoggerEndorsePeople("[ " + DateTime.Now + " ] => [ Please Enter a Number in Minimum Delay ]");
                            MessageBox.Show("Please Enter a Number in Minimum Delay");
                        }
                        else if (!NumberHelper.ValidateNumber(txtEndorsmentMaxDelay.Text))
                        {
                            AddLoggerEndorsePeople("[ " + DateTime.Now + " ] => [ Please Enter a Number in Maximum Delay ]");
                            MessageBox.Show("Please Enter a Number in Maxiimum Delay");
                        }
                    }
                }

            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddLoggerEndorsePeople("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }


        }

        int Count_Endorse = 0;
        private void btnStartCampaingEndorsement_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {
                if (!string.IsNullOrEmpty(txtEndorsmentMinDelay.Text) && !string.IsNullOrEmpty(txtEndorsmentMaxDelay.Text)
                       && NumberHelper.ValidateNumber(txtEndorsmentMinDelay.Text) && NumberHelper.ValidateNumber(txtEndorsmentMaxDelay.Text))
                {
                    lstEndorsementThread.Clear();

                    if (IsStop)
                    {
                        IsStop = false;
                    }

                    try
                    {
                        Thread _EndorsementPerDay = new Thread(EndorsementPerDay);
                        _EndorsementPerDay.Start();
                    }
                    catch { }
                }
                else
                {
                    if (string.IsNullOrEmpty(txtEndorsmentMinDelay.Text))
                    {
                        AddLoggerEndorsePeople("[ " + DateTime.Now + " ] => [ Please Enter Minimum Delay ]");
                        MessageBox.Show("Please Enter Minimum Delay");
                    }
                    else if (string.IsNullOrEmpty(txtEndorsmentMaxDelay.Text))
                    {
                        AddLoggerEndorsePeople("[ " + DateTime.Now + " ] => [ Please Enter Maximum Delay ]");
                        MessageBox.Show("Please Enter Maximum Delay");
                    }
                    else if (!NumberHelper.ValidateNumber(txtEndorsmentMinDelay.Text))
                    {
                        AddLoggerEndorsePeople("[ " + DateTime.Now + " ] => [ Please Enter a Number in Minimum Delay ]");
                        MessageBox.Show("Please Enter a Number in Minimum Delay");
                    }
                    else if (!NumberHelper.ValidateNumber(txtEndorsmentMaxDelay.Text))
                    {
                        AddLoggerEndorsePeople("[ " + DateTime.Now + " ] => [ Please Enter a Number in Maximum Delay ]");
                        MessageBox.Show("Please Enter a Number in Maxiimum Delay");
                    }
                }
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddLoggerEndorsePeople("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        }


        private void EndorsementPerDay()
        {
            int NoOfThreads = 5;
            int NoOfEndorsementPerDay = 10;
            int NoOfSkillsPerDay = 0;
            int MinDelay = 20;
            int MaxDelay = 25;

            if (!string.IsNullOrEmpty(txtEndorsmentMinDelay.Text))
            {
                txtEndorsmentMinDelay.Invoke(new MethodInvoker(delegate
                {
                    MinDelay = int.Parse(txtEndorsmentMinDelay.Text);
                }));
            }
            if (!string.IsNullOrEmpty(txtEndorsmentMaxDelay.Text))
            {
                txtEndorsmentMaxDelay.Invoke(new MethodInvoker(delegate
                {
                    MaxDelay = int.Parse(txtEndorsmentMaxDelay.Text);
                }));
            }

            if (!string.IsNullOrEmpty(txtEndorsmentNoOfThread.Text))
            {
                txtEndorsmentNoOfThread.Invoke(new MethodInvoker(delegate
                {
                    NoOfThreads = int.Parse(txtEndorsmentNoOfThread.Text);
                }));
            }

            if (!string.IsNullOrEmpty(txtNoOfEndorsementPerDay.Text))
            {
                txtNoOfEndorsementPerDay.Invoke(new MethodInvoker(delegate
                {
                    NoOfEndorsementPerDay = int.Parse(txtNoOfEndorsementPerDay.Text);
                }));
            }

            if (!string.IsNullOrEmpty(txtNoOfSkills.Text))
            {
                txtNoOfSkills.Invoke(new MethodInvoker(delegate
                {
                    NoOfSkillsPerDay = int.Parse(txtNoOfSkills.Text);
                }));
            }

            Count_Endorse = LinkedInManager.linkedInDictionary.Count();

            try
            {
                foreach (KeyValuePair<string, LinkedInMaster> item in LinkedInManager.linkedInDictionary)
                {
                    try
                    {
                        ThreadPool.SetMaxThreads(NoOfThreads, 5);

                        ThreadPool.QueueUserWorkItem(new WaitCallback(EndorsingPerDay), new object[] { item, NoOfEndorsementPerDay, NoOfSkillsPerDay, MinDelay, MaxDelay });

                        Thread.Sleep(1000);
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message == "Thread was being aborted.")
                        {
                            AddLoggerEndorsePeople("[ " + DateTime.Now + " ] => [ Profile Endorsing  Process Stopped ]");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message == "Thread was being aborted.")
                {
                    AddLoggerEndorsePeople("[ " + DateTime.Now + " ] => [ Unfollowing Process Stopped ]");
                }
            }

        }


        public void EndorsingPerDay(object parameter)
        {
            try
            {

                lstEndorsementThread.Add(Thread.CurrentThread);
                lstEndorsementThread = lstEndorsementThread.Distinct().ToList();
                Thread.CurrentThread.IsBackground = true;

                int NoOfEndorsementPerDay = 10;
                int Counter = 0;
                int MinDelay = 20;
                int MaxDelay = 25;

                List<string> LstFriendId = new List<string>();

                //NoOfEndorsementPerDay = int.Parse(txtNoOfEndorsementPerDay.Text);
                Array paramsArray = new object[1];
                paramsArray = (Array)parameter;

                NoOfEndorsementPerDay = (int)paramsArray.GetValue(1);
                EndorsePeople.no_of_Skils = (int)paramsArray.GetValue(2);
                MinDelay = (int)paramsArray.GetValue(3);
                MaxDelay = (int)paramsArray.GetValue(4);



                KeyValuePair<string, LinkedInMaster> item = (KeyValuePair<string, LinkedInMaster>)paramsArray.GetValue(0);
                AddLoggerEndorsePeople("[ " + DateTime.Now + " ] => [ Logging In With " + item.Key + " ]");

                if (!item.Value.IsLoggedIn)
                {
                    GlobusHttpHelper HttpHelper = new GlobusHttpHelper();
                    LinkedinLogin Login = new LinkedinLogin();
                    Login.accountUser = item.Key;
                    Login.accountPass = item.Value._Password;
                    Login.proxyAddress = item.Value._ProxyAddress;
                    Login.proxyPort = item.Value._ProxyPort;
                    Login.proxyUserName = item.Value._ProxyUsername;
                    Login.proxyPassword = item.Value._ProxyPassword;
                    Login.LoginHttpHelper(ref HttpHelper);

                    if (Login.IsLoggedIn)
                    {
                        AddLoggerEndorsePeople("[ " + DateTime.Now + " ] => [ Logged In With " + item.Key + " ]");
                    }
                    else
                    {
                        AddLoggerEndorsePeople("[ " + DateTime.Now + " ] => [ Not Logged In With " + item.Key + " ]");
                        return;
                    }


                    if (Login.IsLoggedIn)
                    {
                        //GroupStatus dataScrape = new GroupStatus();
                        string query = "Select * From tb_endorsement WHERE  (Username = '" + item.Key + "')";
                        DataSet ds = DataBaseHandler.SelectQuery(query, "tb_endorsement");
                        DataTable dt = ds.Tables["tb_endorsement"];
                        string Username = string.Empty;
                        if (dt.Rows.Count > 0)
                        {
                            Username = dt.Rows[0]["Username"].ToString();
                        }

                        if (Username != item.Key)
                        {
                            AddLoggerEndorsePeople("[ " + DateTime.Now + " ] => [ Scraping 1st Connection from Account : " + item.Key + " ]");
                            GroupStatus MemberScrape = new GroupStatus();

                            GroupStatus.moduleLog = "endorsecamp";
                            Dictionary<string, string> Result = MemberScrape.PostAddMembers(ref HttpHelper, Login.accountUser);
                            AddLoggerEndorsePeople("[ " + DateTime.Now + " ] => [ Scraping process completed from Account : " + item.Key + " ]");
                        }

                        AddLoggerEndorsePeople("[ " + DateTime.Now + " ] => [ Starting Endorsing People ]");

                        string SelectQuery = "select * from tb_endorsement where Username = '" + item.Key + "' and Status=0";
                        DataSet dst = DataBaseHandler.SelectQuery(SelectQuery, "tb_endorsement");
                        DataTable dt1 = dst.Tables["tb_endorsement"];

                        foreach (DataRow dr in dt1.Rows)
                        {
                            if (Counter <= NoOfEndorsementPerDay)
                            {
                                string FriendId = dr["FriendId"].ToString();

                                LstFriendId.Add(FriendId);
                                Counter++;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }


                    EndorsePeople EnorsePeople = new EndorsePeople();
                    EnorsePeople.Username = item.Key;

                    foreach (string _FriendId in LstFriendId)
                    {
                        int delay = RandomNumberGenerator.GenerateRandom(MinDelay, MaxDelay);

                        try
                        {
                            EnorsePeople.EndorsingPeople(ref HttpHelper, _FriendId);

                            string strQuery = "UPDATE tb_endorsement SET Status='" + "1" + "' WHERE Status='0' AND FriendId ='" + _FriendId + "' AND Username ='" + item.Key + "' ";
                            DataBaseHandler.UpdateQuery(strQuery, "tb_endorsement");

                            AddLoggerEndorsePeople("[ " + DateTime.Now + " ] => [ Delay for : " + delay + " Seconds ]");
                            Thread.Sleep(delay * 1000);
                        }
                        catch { }
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error -->EndorsingPeople() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> EndorsingPeople() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.path_EndorsePeople);
            }
            finally
            {
                Count_Endorse--;

                if (Count_Endorse == 0)
                {

                    btnstartEndorse.Invoke(new MethodInvoker(delegate
                    {
                        AddLoggerEndorsePeople("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                        AddLoggerEndorsePeople("-------------------------------------------------------------------------------------------------------------------------------");
                        btnStartCampaingEndorsement.Cursor = Cursors.Default;
                    }));
                }
            }
        }
        #endregion

        #region StartEndorsingPeople()
        public void StartEndorsingPeople()
        {
            int MinDelay = 20;
            int MaxDelay = 25;

            if (!string.IsNullOrEmpty(txtEndorsmentMinDelay.Text))
            {
                txtEndorsmentMinDelay.Invoke(new MethodInvoker(delegate
                {
                    MinDelay = int.Parse(txtEndorsmentMinDelay.Text);
                }));
            }
            if (!string.IsNullOrEmpty(txtEndorsmentMaxDelay.Text))
            {
                txtEndorsmentMaxDelay.Invoke(new MethodInvoker(delegate
                {
                    MaxDelay = int.Parse(txtEndorsmentMaxDelay.Text);
                }));
            }

            foreach (KeyValuePair<string, LinkedInMaster> item in LinkedInManager.linkedInDictionary)
            {
                if (item.Key == EndorsePeople.xls_Email)
                {
                    ThreadPool.SetMaxThreads(5, 5);
                    //ThreadPool.QueueUserWorkItem(new WaitCallback(EndorsingPeople), new object[] { item });
                    //Thread.Sleep(1000);

                    EndorsingPeople(new object[] { item, MinDelay, MaxDelay });
                }
            }
        }
        #endregion
        
        #region EndorsingPeople
        int counter1 = 0;

        Dictionary<string, string> CheckDuplicate = new Dictionary<string, string>();
        public void EndorsingPeople(object parameter)
        {
            try
            {
                int MinDelay = 20;
                int MaxDelay = 25;

                lstEndorsementThread.Add(Thread.CurrentThread);
                lstEndorsementThread = lstEndorsementThread.Distinct().ToList();
                Thread.CurrentThread.IsBackground = true;

                Array paramsArray = new object[1];
                paramsArray = (Array)parameter;
                KeyValuePair<string, LinkedInMaster> item = (KeyValuePair<string, LinkedInMaster>)paramsArray.GetValue(0);
                MinDelay = (int)paramsArray.GetValue(1);
                MaxDelay = (int)paramsArray.GetValue(2);



                //AddLoggerEndorsePeople("[ " + DateTime.Now + " ] => [ Logging In With " + item.Key + " ]");

                
                try
                {
                    if (!item.Value.IsLoggedIn)
                    {
                        GlobusHttpHelper HttpHelper = new GlobusHttpHelper();

                        LinkedinLogin Login = new LinkedinLogin();
                        Login.accountUser = item.Key;
                        try
                        {
                            CheckDuplicate.Add(Login.accountUser, Login.accountUser);
                        }
                        catch (Exception)
                        {

                            return;
                        }
                       


                        Login.accountPass = item.Value._Password;
                        Login.proxyAddress = item.Value._ProxyAddress;
                        Login.proxyPort = item.Value._ProxyPort;
                        Login.proxyUserName = item.Value._ProxyUsername;
                        Login.proxyPassword = item.Value._ProxyPassword;
                        Login.LoginHttpHelper(ref HttpHelper);


                        if (Login.IsLoggedIn)
                        {
                            try
                            {
                                Globals.tempDict.Add(item.Key, "");
                                AddLoggerEndorsePeople("[ " + DateTime.Now + " ] => [ Logged In With " + item.Key + " ]");
                                AddLoggerEndorsePeople("[ " + DateTime.Now + " ] => [ Starting Endorsing People ]");
                            }
                            catch
                            { }
                        }
                        else
                        {
                            AddLoggerEndorsePeople("[ " + DateTime.Now + " ] => [ Not Logged In With " + item.Key + " ]");
                            return;
                        }

                        
                        EndorsePeople EnorsePeople = new EndorsePeople();
                        EnorsePeople.Username = item.Key;

                        counter1 = EndorsePeople.Endorse_excelData.Count;

                        foreach (string[] itemArr in EndorsePeople.Endorse_excelData)
                        {
                            int delay = RandomNumberGenerator.GenerateRandom(MinDelay, MaxDelay);

                            EnorsePeople.EndorsingPeople(ref HttpHelper, itemArr[1]);

                            AddLoggerEndorsePeople("[ " + DateTime.Now + " ] => [ Delay for : " + delay + " Seconds ]");
                            Thread.Sleep(delay * 1000);
                        }
                    }
                }
                catch
                { 
                    //repeated ID
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error -->EndorsingPeople() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> EndorsingPeople() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.path_EndorsePeople);
            }
            finally
            {
                counter1--;

                if (counter1 == 0)
                {
                    btnstartEndorse.Invoke(new MethodInvoker(delegate
                    {
                        AddLoggerEndorsePeople("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                        AddLoggerEndorsePeople("----------------------------------------------------------------------------------------------------------------------------------------");
                        btnstartEndorse.Cursor = Cursors.Default;
                    }));
                }
            }
        }
        #endregion

        #region btnStopEndorseProfile_Click
        private void btnStopEndorseProfile_Click(object sender, EventArgs e)
        {
            try
            {
                IsStop = true;
                List<Thread> lstTemp = lstEndorsementThread.Distinct().ToList();
                foreach (Thread item in lstTemp)
                {
                    try
                    {
                        item.Abort();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error >>> " + ex.StackTrace);
                    }
                }

                AddLoggerEndorsePeople("-------------------------------------------------------------------------------------------------------------------------------");
                AddLoggerEndorsePeople("[ " + DateTime.Now + " ] => [ PROCESS STOPPED ]");
                AddLoggerEndorsePeople("-------------------------------------------------------------------------------------------------------------------------------");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error >>> " + ex.StackTrace);
            }
        }
        #endregion

        #region campaignToolStripMenuItem_click
        private void campaignToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                frmCampaignAccountCreator obj_frmCampainAccountCreator = frmCampaignAccountCreator.GetFrmCampaignAccountCreator();
                obj_frmCampainAccountCreator.Show();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error >>> " + ex.StackTrace);
            }
        }
        #endregion
        
        #region chkIndustry_CheckedChanged
        private void chkIndustry_CheckedChanged(object sender, EventArgs e)
        {
            //if (chkIndustry.Checked == true)
            //{
            //    CampainGroupCreate.Campaign_ChkCountry = true;
            //}
            //else
            //{
            //    CampainGroupCreate.Campaign_ChkCountry = false;
            //}
        }
        #endregion

        #region chkType_CheckedChanged
        private void chkType_CheckedChanged(object sender, EventArgs e)
        {
            //if (chkType.Checked == true)
            //{
            //    CampainGroupCreate.Campaign_ChkType = true;
            //}
            //else
            //{
            //    CampainGroupCreate.Campaign_ChkType = false;
            //}
        }
        #endregion

        #region chkCountry_CheckedChanged
        private void chkCountry_CheckedChanged(object sender, EventArgs e)
        {
            //if (chkCountry.Checked == true)
            //{
            //    CampainGroupCreate.Campaign_ChkCountry = true;
            //}
            //else
            //{
            //    CampainGroupCreate.Campaign_ChkCountry = false;
            //}
        }
        #endregion
        
        #region inviteMemberThroughProfileToolStripMenuItem_Click
        private void inviteMemberThroughProfileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                FrmInviteMemberThroughProfileURL obj_FrmInviteMemberThroughProfileURL = new FrmInviteMemberThroughProfileURL();
                obj_FrmInviteMemberThroughProfileURL.Show();
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        #region btnAddGroups_Click
        private void btnAddGroups_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {

                try
                {
                    lstJoinFriendGroupThread.Clear();
                    lblTotGroups.Text = "(0)";
                    if (IsStop)
                    {
                        IsStop = false;
                    }

                    if (!string.IsNullOrEmpty(txtGroupjoinMinDelay.Text) && NumberHelper.ValidateNumber(txtGroupjoinMinDelay.Text))
                    {
                        GroupStatus.minDelay = Convert.ToInt32(txtGroupjoinMinDelay.Text);
                    }
                    if (!string.IsNullOrEmpty(txtGroupjoinMaxDelay.Text) && NumberHelper.ValidateNumber(txtGroupjoinMaxDelay.Text))
                    {
                        GroupStatus.maxDelay = Convert.ToInt32(txtGroupjoinMaxDelay.Text);
                    }

                    if (LinkedInManager.linkedInDictionary.Count() == 0)
                    {
                        return;
                    }
                    else if (cmbUser.Items.Count == 0)
                    {
                        AddLoggerGroupAdd("[ " + DateTime.Now + " ] => [ Please click Add Friend button.. ]");
                        MessageBox.Show("Please click Add Friend button..");
                        return;
                    }
                    else if (chkMembers.Items.Count == 0)
                    {
                        AddLoggerGroupAdd("[ " + DateTime.Now + " ] => [ Please select ID.. ]");
                        MessageBox.Show("Please select ID..");
                        return;
                    }
                    if (chkMembers.CheckedItems.Count == 0)
                    {
                        AddLoggerGroupAdd("[ " + DateTime.Now + " ] => [ Please select Atleast One Friend.. ]");
                        MessageBox.Show("Please select Atleast One Friend..");
                        return;
                    }
                    

                    MemId.Clear();
                    chkExistGroup.Items.Clear();
                    GroupStatus.GroupDtl.Clear();
                    AddLoggerGroupAdd("[ " + DateTime.Now + " ] => [ Search Groups for Selected Friends.. ]");
                    btnAddGroups.Cursor = Cursors.AppStarting;

                    new Thread(() =>
                    {
                        LinkedInMembersGroupSearch();

                    }).Start();
                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> btnAddGroups_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> btnAddGroups_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddFriendsGroupErrorLogs);
                }
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddLoggerGroupAdd("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        }
        #endregion

        #region linkedinCompanyToolStripMenuItem_Click
        private void linkedinCompanyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                FrmSearchWithSalesNavigator obj_FrmScrapWithFilters = new FrmSearchWithSalesNavigator();
                obj_FrmScrapWithFilters.Show();
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region groupUpdateLikerToolStripMenuItem_Click
        private void groupUpdateLikerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                frmGroupUpdateLiker obj_frmGroupUpdateLiker = new frmGroupUpdateLiker();
                obj_frmGroupUpdateLiker.Show();
            }
            catch { }
        }
        #endregion

        public EventHandler linkedinFilterSearchLogEvent_addToLogger { get; set; }

        #region chkMessageTo_MouseDoubleClick
        private void chkMessageTo_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int cnt = chkMessageTo.CheckedItems.Count;
            lblTotFriendList.Text = "(" + cnt + ")";
        }
        #endregion

        #region linkLblJoinGroupUrl_Click
        private void linkLblJoinGroupUrl_Click(object sender, EventArgs e)
        {
            try
            {
                frmJoinGroupUsingUrl obj_frmJoinGroupUsingUrl = new frmJoinGroupUsingUrl();
                obj_frmJoinGroupUsingUrl.Show();
            }
            catch { }
        }
        #endregion

        #region linkbtnScrapeinExcelInput_LinkClicked
        private void linkbtnScrapeinExcelInput_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                FrmInBoardProGetDataUsingExcelInput obj_FrmInBoardProGetDataUsingUrl = new FrmInBoardProGetDataUsingExcelInput();
                obj_FrmInBoardProGetDataUsingUrl.Show();
            }
            catch { }
        }
        #endregion

        #region linkbtnScrapeinMultipleExcelInput_LinkClicked
        private void linkbtnScrapeinMultipleExcelInput_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                FrmLdScraperMultipleExcelInput obj_FrmLdScraperMultipleInput = new FrmLdScraperMultipleExcelInput();
                obj_FrmLdScraperMultipleInput.Show();
            }
            catch { }

        }
        #endregion

        #region linkbtncompanySearch_LinkClicked
        private void linkbtncompanySearch_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {

                FrmSearchWithSalesNavigator obj_FrmScrapWithFilters = new FrmSearchWithSalesNavigator();
                obj_FrmScrapWithFilters.Show();
            }
            catch { }
        }
        #endregion

        #region linkbtnScrapGroupMem_LinkClicked
        private void linkbtnScrapGroupMem_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                FrmScrapGroupMember obj_FrmScrapGroupMember = new FrmScrapGroupMember();
                obj_FrmScrapGroupMember.Show();
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        #region followCompanyUsingUrlToolStripMenuItem_Click
        private void followCompanyUsingUrlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                frmFollowCompany obj_frmFollowCompany = new frmFollowCompany();
                obj_frmFollowCompany.Show();
            }
            catch { }
        }
        #endregion

        #region btnLinkAccountCreatorHelp_Click
        private void btnLinkAccountCreatorHelp_Click(object sender, EventArgs e)
        {
            //ClsSelect.getVed = "LDAccountCreator";
            //HelpVedeo obj_help = new HelpVedeo();
            //obj_help.Show();
        }
        #endregion

        #region LinkScrpHelp_Click
        private void LinkScrpHelp_Click(object sender, EventArgs e)
        {
            //ClsSelect.getVed = "LDScrapper";
            //HelpVedeo obj_help = new HelpVedeo();
            //obj_help.Show();
        }
        #endregion

        #region btnLinkStatusUpdateHelp_Click
        private void btnLinkStatusUpdateHelp_Click(object sender, EventArgs e)
        {
            //ClsSelect.getVed = "LDStatusUpdate";
            //HelpVedeo obj_help = new HelpVedeo();
            //obj_help.Show();
        }
        #endregion

        #region btnHelpAddConEmailHelp_Click
        private void btnHelpAddConEmailHelp_Click(object sender, EventArgs e)
        {
            //ClsSelect.getVed = "LDAddConnectionEmail";
            //HelpVedeo obj_help = new HelpVedeo();
            //obj_help.Show();
        }
        #endregion

        #region btnHelpAddConKeywordHelp_Click
        private void btnHelpAddConKeywordHelp_Click(object sender, EventArgs e)
        {
            //ClsSelect.getVed = "LDAddConnectionKeyword";
            //HelpVedeo obj_help = new HelpVedeo();
            //obj_help.Show();
        }
        #endregion

        #region btnCreateGroupHelp_Click
        private void btnCreateGroupHelp_Click(object sender, EventArgs e)
        {
            //ClsSelect.getVed = "LDCreateGroup";
            //HelpVedeo obj_help = new HelpVedeo();
            //obj_help.Show();
        }
        #endregion

        #region btnJoinFriendsGroupHelp_Click
        private void btnJoinFriendsGroupHelp_Click(object sender, EventArgs e)
        {
            //ClsSelect.getVed = "LDJoinFriendsGroup";
            //HelpVedeo obj_help = new HelpVedeo();
            //obj_help.Show();
        }
        #endregion

        #region btnJoinSearchGroupHelp_Click
        private void btnJoinSearchGroupHelp_Click(object sender, EventArgs e)
        {
            //ClsSelect.getVed = "LDJoinSearchGroup";
            //HelpVedeo obj_help = new HelpVedeo();
            //obj_help.Show();
        }
        #endregion

        #region btnGroupStatusUpdateHelp_Click
        private void btnGroupStatusUpdateHelp_Click(object sender, EventArgs e)
        {
            //ClsSelect.getVed = "LDGroupStatusUpdate";
            //HelpVedeo obj_help = new HelpVedeo();
            //obj_help.Show();
        }
        #endregion

        #region btnComposeMsgHelp_Click
        private void btnComposeMsgHelp_Click(object sender, EventArgs e)
        {
            //ClsSelect.getVed = "LDComposeMsg";
            //HelpVedeo obj_help = new HelpVedeo();
            //obj_help.Show();
        }
        #endregion

        #region btnMsgGroupMemHelp_Click
        private void btnMsgGroupMemHelp_Click(object sender, EventArgs e)
        {
            //ClsSelect.getVed = "LDMsgGroupMem";
            //HelpVedeo obj_help = new HelpVedeo();
            //obj_help.Show();
        }
        #endregion

        #region NewVertoolStripMenuItem_Click
        private void NewVertoolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                frmVersionInfo obj_verinfo = new frmVersionInfo();
                obj_verinfo.Show();
            }
            catch { }
        }
        #endregion


        public List<string> listAreaLocation = new List<string>();
        private void btnBrowseLocationArea_Click(object sender, EventArgs e)
        {
            try
            {
                txtStatusUpd.Text = "";
                listAreaLocation.Clear();
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "Text Files (*.txt)|*.txt";
                    ofd.InitialDirectory = Application.StartupPath;

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtAreaWiseLocation.Text = ofd.FileName;
                        listAreaLocation = new List<string>();

                        List<string> temp = GlobusFileHelper.ReadFiletoStringList(ofd.FileName);

                        foreach (string item in temp)
                        {
                            if (!string.IsNullOrEmpty(item.Replace("\t", "")))
                            {
                                listAreaLocation.Add(item);
                            }
                        }

                        AddLoggerScrapeUsers("[ " + DateTime.Now + " ] => [ " + listAreaLocation.Count + " Location wise area loaded ]");

                    }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Linkedin Scraper --> btnBrowseLocationArea_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Linkedin Scraper --> btnBrowseLocationArea_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPreScrapperErrorLogs);
            }
        }

        private void statusImageShareToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                frmStatusImageShare obj_frmStatusImageShare = new frmStatusImageShare();
                obj_frmStatusImageShare.Show();
            }
            catch { }
        }


        private void InBoardProGetDataUsingExcelInputFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                FrmInBoardProGetDataUsingExcelInput obj_FrmInBoardProGetDataUsingUrl = new FrmInBoardProGetDataUsingExcelInput();
                obj_FrmInBoardProGetDataUsingUrl.Show();
            }
            catch { }
        }

        private void InBoardProGetDataUsingExcelInputFileMultipleOptionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                FrmLdScraperMultipleExcelInput obj_FrmLdScraperMultipleInput = new FrmLdScraperMultipleExcelInput();
                obj_FrmLdScraperMultipleInput.Show();
            }
            catch { }
        }

        private void InBoardProGetDataUsingExcelInputToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string mod = "ExcelInput3";
                ModuleAuthentication obj_ModuleAuthentication = new ModuleAuthentication(mod);
                obj_ModuleAuthentication.Show();

            }
            catch { }

        }


        private void scrapeGroupMembersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                FrmScrapGroupMember obj_FrmScrapGroupMember = new FrmScrapGroupMember();
                obj_FrmScrapGroupMember.Show();
            }
            catch (Exception ex)
            {
            }
        }

        private void linkedinCompanyScraperToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                FrmSearchWithSalesNavigator obj_FrmScrapWithFilters = new FrmSearchWithSalesNavigator();
                obj_FrmScrapWithFilters.Show();
            }
            catch (Exception ex)
            {

            }
        }

        #region friendsGroupScraperToolStripMenuItem_Click
        private void friendsGroupScraperToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                frmFriendsGroupScraper obj_frmFriendsGroupScraper = new frmFriendsGroupScraper();
                obj_frmFriendsGroupScraper.Show();
            }
            catch { }
        }
        #endregion

        private void joinGroupUsingUrlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                frmJoinGroupUsingUrl objfrmJoinGroupUsingUrl = new frmJoinGroupUsingUrl();
                objfrmJoinGroupUsingUrl.Show();
            }
            catch { }

        }


        private void removePendingGroupsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                frmRemovePendingGroups obj_frmRemovePendingGroups = new frmRemovePendingGroups();
                obj_frmRemovePendingGroups.Show();
            }
            catch { }
        }

        private void inviteUsingSearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //frmSearchWithInvites obj_frmSearchWithInvites = new frmSearchWithInvites();
                frmCampaignSearchWithInvite objfrmCampaignSearchWithInvite = new frmCampaignSearchWithInvite();
                objfrmCampaignSearchWithInvite.Show();
            }
            catch { }
        }

        private void chkUniqueEmailConnection_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUniqueEmailConnection.Checked)
            {

                txtNumberOfRequestPerEmail.Visible = false;
                label114.Visible = false;
            }
            else
            {
                txtNumberOfRequestPerEmail.Visible = true;
                label114.Visible = true;
            }

        }

        private void chkDeathByCaptcha_CheckedChanged(object sender, EventArgs e)
        {

            //if (chkDeathByCaptcha.Checked)
            //{
            //    FrmCaptchaLogin CapchaAcc = new FrmCaptchaLogin();
            //    CapchaAcc.ShowDialog();
            //    //chkDeathByCaptcha.Checked = false;             
            //}

            //else
            //{
            //    FrmCaptchaLogin CapchaAcc = new FrmCaptchaLogin();
            //    CapchaAcc.Close();
            //}
        }

        private void chkStatusSpintax_CheckedChanged(object sender, EventArgs e)
        {
            if (chkStatusSpintax.Checked)
            {
                txtStatusUpd.Multiline = true;
                this.txtStatusUpd.Size = new System.Drawing.Size(569, 115);
                btnUploadStatusMessage.Visible = false;
                chkUpdateStatus.Location = new Point(120, 167);
                chkStatusSpintax.Location = new Point(289, 167);
                label52.Location = new Point(384, 167);
                //GroupBoxStatus.Location = new Point(1025, 190);
                this.GroupBoxStatus.Size = new System.Drawing.Size(1025, 190);
                txtStatusUpd.ReadOnly = false;
                txtStatusUpd.Text = string.Empty;

                //gbStatusUpdateThreadSetting.Location = new Point(15, 155);
                //gbStatusUpdateDelaySetting.Location = new Point(443, 155);
                //gbStatusUpdateAction.Location = new Point(18, 244);
                //gbStatusUpdateLogger.Location = new Point(15, 329);
            }
            else
            {

                txtStatusUpd.Multiline = false;
                this.txtStatusUpd.Size = new System.Drawing.Size(569, 21);
                btnUploadStatusMessage.Visible = true;
                chkUpdateStatus.Location = new Point(120, 79);
                chkStatusSpintax.Location = new Point(289, 79);
                label52.Location = new Point(394, 79);
                // GroupBoxStatus.Location = new Point(1025, 121);
                this.GroupBoxStatus.Size = new System.Drawing.Size(1025, 105);
                txtStatusUpd.ReadOnly = true;
                txtStatusUpd.Text = string.Empty;

                //gbStatusUpdateThreadSetting.Location = new Point(15, 213);
                //gbStatusUpdateDelaySetting.Location = new Point(443, 213);
                //gbStatusUpdateAction.Location = new Point(18, 297);
                //gbStatusUpdateLogger.Location = new Point(15, 356);
            }
        }


        private void rbPreventMsgHaveAllreadysentWithGroup_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rbPreventMsgHaveAllreadysentWithGroup.Checked == true)
                {
                    preventMsgSameGroup = true;
                    preventMsgWithoutGroup = false;
                    preventMsgGlobal = false;

                }
                else
                {
                    preventMsgSameGroup = false;
                }

            }
            catch { }
        }

        private void rbchkPreventMsgHaveAllreadysentWithoutGroup_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rbchkPreventMsgHaveAllreadysentWithoutGroup.Checked == true)
                {
                    preventMsgWithoutGroup = true;
                    preventMsgSameGroup = false;
                    preventMsgGlobal = false;
                }
                else
                {
                    preventMsgWithoutGroup = false;
                }

            }
            catch { }
        }

        private void rbchkPreventMsgHaveAllreadysentGlobalGroup_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rbchkPreventMsgHaveAllreadysentGlobalGroup.Checked == true)
                {
                    preventMsgGlobal = true;
                    preventMsgWithoutGroup = false;
                    preventMsgSameGroup = false;
                }
                else
                {
                    preventMsgGlobal = false;
                }
            }
            catch
            { }

        }

        private void chkClearDatabase_CheckedChanged(object sender, EventArgs e)
        {
            if (chkClearDatabase.Checked)
            {
                txtClearDatabase.Visible = true;
                btnClearDatabase.Visible = true;

                txtClearDatabase.Focus();
                ToolTip toolTip = new ToolTip();
                toolTip.SetToolTip(txtClearDatabase, "Please enter userId!");

            }
            else
            {
                txtClearDatabase.Visible = false;
                btnClearDatabase.Visible = false;
            }
        }

        private void btnClearDatabase_Click(object sender, EventArgs e)
        {
            try
            {

                if (string.IsNullOrEmpty(txtClearDatabase.Text.Trim()))
                {
                    MessageBox.Show("Please enter valid userId");
                    return;
                }

                DataSet ds = new DataSet();
                string Querystring = "Delete From tb_ManageMsgGroupMem Where MsgFrom ='" + txtClearDatabase.Text.Trim() + "'"; // and MsgSubject = '" + msg.ToString() + "' and MsgBody = '" + tempBody + "'
                DataBaseHandler.DeleteQuery(Querystring, "tb_ManageMsgGroupMem");
                MessageBox.Show("Successfully deleted details of user " + txtClearDatabase.Text.Trim());
                txtClearDatabase.Text = string.Empty;

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                MessageBox.Show("Some problem in deleting details of user " + txtClearDatabase.Text.Trim());
                return;
            }
        }

        private void chkGetAllScrappedMem_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                string selectedSpeUser = cmbMemberGroup.SelectedItem.ToString();
                try
                {
                    GrpMemNameWithGid.Add(cmbAllUser.SelectedItem.ToString(), GroupStatus.GroupSpecMem);
                }
                catch { }

                if (chkGetAllScrappedMem.Checked == true)
                {
                    string[] SelUser = selectedSpeUser.Split(':');

                    foreach (var item1 in GroupStatus.GroupMemUrl)
                    {
                        string[] grp = item1.Split(':');
                        if (SelUser[1] == grp[0])
                        {
                            SpeGroupId = grp[2];
                            break;
                        }
                    }

                    foreach (var itemM in GroupStatus.GroupSpecMem)
                    {
                        chkListGroupMembers.Items.Add(itemM.Value);
                    }

                    AddLoggerLinkedingrp("[ " + DateTime.Now + " ] => [ Added All Scrapped Group Members ]");
                }
                else
                {
                    chkListGroupMembers.Items.Clear();
                }
            }
            catch { }

        }

        private void msgGroupMemberUsingExcelInputToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                frmMsgGroupMemberUsingExcelInput obj_frmMsgGroupMemberUsingExcelInput = new frmMsgGroupMemberUsingExcelInput();
                obj_frmMsgGroupMemberUsingExcelInput.Show();
            }
            catch { }
        }


        private void chkWithExcelInput_CheckedChanged(object sender, EventArgs e)
        {
            if (chkWithExcelInput.Checked == true)
            {
                GroupStatus.withExcelInput = true;
                txtExcelInputPath.Visible = true;
                btnBrowseExcelInput.Visible = true;
                chkWithSearch.Checked = false;
            }
            else
            {
                GroupStatus.withExcelInput = false;
                txtExcelInputPath.Visible = false;
                btnBrowseExcelInput.Visible = false;
            }
        }

        private void btnBrowseExcelInput_Click(object sender, EventArgs e)
        {
            try
            {
                clsEmailParser objparser = new clsEmailParser();
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    GroupStatus.msgGroupMem_excelData.Clear();
                    ofd.InitialDirectory = System.Windows.Forms.Application.StartupPath;
                    ofd.Filter = "Text Files (*.xlsx)|*.xlsx";
                    AddLoggerLinkedingrp("[ " + DateTime.Now + " ] => [ Add Excel Input file please wait... ]");
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtExcelInputPath.Text = ofd.FileName;
                        GroupStatus.msgGroupMem_excelData = objparser.parseExcel(txtExcelInputPath.Text);
                        AddLoggerLinkedingrp("[ " + DateTime.Now + " ] => [ Add Excel Input file completed... ]");
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void btnStopPublicProxy_Click(object sender, EventArgs e)
        {
            try
            {
                Thread objStopRunningPublicProxy = new Thread(PublicProxy_stopThread);
                objStopRunningPublicProxy.Start();
                proxy_stop = false;
            }
            catch { }

        }

        private void PublicProxy_stopThread()
        {
            try
            {
                IsStop_Proxy = true;
                proxy_stop = true;
                List<Thread> lst = lstStopPublicProxyTest.Distinct().ToList();
                foreach (Thread items in lst)
                {
                    try
                    {
                        items.Abort();
                    }
                    catch
                    { }

                }

                btnStopPublicProxy.Invoke(new MethodInvoker(delegate
                {
                    btnTestPublicProxy.Cursor = Cursors.Default;
                    AddToProxysLogs("-------------------------------------------------------------------------------------------------------------------------------");
                    AddToProxysLogs("[ " + DateTime.Now + " ] => [ PROCESS STOPPED ]");
                    AddToProxysLogs("-------------------------------------------------------------------------------------------------------------------------------");
                }));


            }
            catch (Exception ex)
            {
                Console.WriteLine("Error >>> " + ex.StackTrace);
            }
        }

        //public void stopPublicproxy()
        //{
        //    try
        //    {
        //        IsStop_Proxy = true;
        //        List<Thread> lst = lstStopPublicProxyTest.Distinct().ToList();
        //        foreach (Thread items in lst)
        //        {
        //            try
        //            {
        //                items.Abort();
        //            }
        //            catch
        //            { }

        //        }
        //        btnTestPublicProxy.Cursor = Cursors.Default;
        //        AddToProxysLogs("-------------------------------------------------------------------------------------------------------------------------------");
        //        AddToProxysLogs("[ " + DateTime.Now + " ] => [ PROCESS STOPPED ]");
        //        AddToProxysLogs("-------------------------------------------------------------------------------------------------------------------------------");
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Error >>> " + ex.StackTrace);
        //    }
        //}

        private void btnStopPrivateProxy_Click(object sender, EventArgs e)
        {
            try
            {
                IsStop_Proxy = true;
                List<Thread> lst = lstStopPrivateProxyTest.Distinct().ToList();
                foreach (Thread items in lst)
                {
                    try
                    {
                        items.Abort();
                    }
                    catch
                    { }

                }

                AddToProxysLogs("-------------------------------------------------------------------------------------------------------------------------------");
                AddToProxysLogs("[ " + DateTime.Now + " ] => [ PROCESS STOPPED ]");
                AddToProxysLogs("-------------------------------------------------------------------------------------------------------------------------------");
                btnTestPublicProxy.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error >>> " + ex.StackTrace);
            }
        }

        private void LstComposeMsg_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void chkWithExcelInputCmpMsg_CheckedChanged(object sender, EventArgs e)
        {
            if (chkWithExcelInputCmpMsg.Checked == true)
            {
                GroupStatus.withExcelInput = true;
                txtExcelinputpathCmpMsg.Visible = true;
                btnBrowseWithExcelInputCmpMsg.Visible = true;
            }
            else
            {
                GroupStatus.withExcelInput = false;
                txtExcelinputpathCmpMsg.Visible = false;
                btnBrowseWithExcelInputCmpMsg.Visible = false;
            }

        }


        private void btnBrowseWithExcelInputCmpMsg_Click(object sender, EventArgs e)
        {
            try
            {
                clsEmailParser objparser = new clsEmailParser();
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    GroupStatus.Cmpmsg_excelData.Clear();
                    ofd.InitialDirectory = System.Windows.Forms.Application.StartupPath;
                    ofd.Filter = "Text Files (*.xlsx)|*.xlsx";
                    AddLoggerComposeMessage("[ " + DateTime.Now + " ] => [ Add Excel Input file please wait... ]");
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtExcelinputpathCmpMsg.Text = ofd.FileName;
                        GroupStatus.Cmpmsg_excelData = objparser.parseExcel(txtExcelinputpathCmpMsg.Text);
                    }
                    AddLoggerComposeMessage("[ " + DateTime.Now + " ] => [ Add Excel Input file completed... ]");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        private void btnBrowseWithExcelInputCmpMsg_Click_1(object sender, EventArgs e)
        {
            try
            {
                clsEmailParser objparser = new clsEmailParser();
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    GroupStatus.Cmpmsg_excelData.Clear();
                    ofd.InitialDirectory = System.Windows.Forms.Application.StartupPath;
                    ofd.Filter = "Text Files (*.xlsx)|*.xlsx";
                    AddLoggerComposeMessage("[ " + DateTime.Now + " ] => [ Add Excel Input file please wait... ]");
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtExcelinputpathCmpMsg.Text = ofd.FileName;
                        GroupStatus.Cmpmsg_excelData = objparser.parseExcel(txtExcelinputpathCmpMsg.Text);
                        AddLoggerComposeMessage("[ " + DateTime.Now + " ] => [ Add Excel Input file completed... ]");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void ChkAllAccounts_ComposeMessage_CheckedChanged(object sender, EventArgs e)
        {

        }

        #region tabViewProfileRank_Paint
        private void tabViewProfileRank_Paint(object sender, PaintEventArgs e)
        {
            Graphics g;
            g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.DrawImage(image, 0, 0, tabPageGroupUpdate.Width, tabPageGroupUpdate.Height);
        }
        #endregion

        private void btnStartRank_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
            if (CheckNetConn)
            {
                lstProfileRankThread.Clear();
                if (_IsStop_ProfileRank)
                {
                    _IsStop_ProfileRank = false;
                }
                try
                {
                    AddLoggerLinkedinSearch("[ " + DateTime.Now + " ] => [ Starting Process ]");
                    _IsStop_ProfileRank = false;
                    _ProfileRankSelectedEmailId = cmbSelAccount.SelectedItem.ToString();
                    btnStartRank.Cursor = Cursors.AppStarting;

                    Thread thread_ProfileRank = new Thread(StartProfileRank);
                    thread_ProfileRank.Start();
                }

                catch
                { }
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddLoggerProfileRank("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        }

        #region StartProfileRank

        private void StartProfileRank()
        {
            GlobusHttpHelper objGlobusHttpHelper = new GlobusHttpHelper();
            try
            {
                #region thread is stopped or not
                try
                {
                    if (_IsStop_ProfileRank)
                    {
                        return;
                    }
                    lstProfileRankThread.Add(Thread.CurrentThread);
                    lstProfileRankThread = lstProfileRankThread.Distinct().ToList();
                    Thread.CurrentThread.IsBackground = true;
                }
                catch
                { }
                #endregion

                try
                {
                    AddLoggerProfileRank("[ " + DateTime.Now + " ] => [ Logging in With UserName >>> " + _ProfileRankSelectedEmailId + " ]");
                    if (SearchCriteria.SignIN)
                    {
                        LinkedinLogin Login = new LinkedinLogin();
                        //For Sign Out +9+
                        Login.LoginHttpHelper(ref objGlobusHttpHelper);

                        SearchCriteria.SignOut = true;
                    }


                    if (SearchCriteria.SignOut)
                    {
                        LinkedInMaster item = null;
                        LinkedInManager.linkedInDictionary.TryGetValue(_ProfileRankSelectedEmailId, out item);
                        item.LoginHttpHelper(ref objGlobusHttpHelper);


                        AddLoggerProfileRank("[ " + DateTime.Now + " ] => [ Login Process is running... ]");
                        if (item.IsLoggedIn)
                        {
                            if (SearchCriteria.loginREsponce.Contains("[ " + DateTime.Now + " ] => [ Your LinkedIn account has been temporarily restricted ]"))
                            {
                                AddLoggerProfileRank("[ " + DateTime.Now + " ] => [ " + _ProfileRankSelectedEmailId + "Your LinkedIn account has been temporarily restricted ]");


                            }

                            if (SearchCriteria.loginREsponce.Contains("Please confirm your email address"))
                            {
                                AddLoggerProfileRank("[ " + DateTime.Now + " ] => [ " + _ProfileRankSelectedEmailId + " account has been temporarily restricted Please confirm your email address ]");


                            }
                            SearchCriteria.SignIN = true;
                            SearchCriteria.SignOut = false;
                            if (item.IsLoggedIn)
                            {
                                AddLoggerProfileRank("[ " + DateTime.Now + " ] => [ Logged in Successfully With UserName >>> " + _ProfileRankSelectedEmailId + " ]");
                            }
                            else
                            {
                                AddLoggerProfileRank("[ " + DateTime.Now + " ] => [ Couldn't Login   With UserName >>> " + _ProfileRankSelectedEmailId + " ]");
                            }
                        }
                        else
                        {
                            AddLoggerProfileRank("[ " + DateTime.Now + " ] => [ Couldn't Login With UserName >>> " + _ProfileRankSelectedEmailId + " ]");
                            return;
                        }

                        ProfileRank obj_profileRank = new ProfileRank(item._Username, item._Password, item._ProxyAddress, item._ProxyPort, item._ProxyUsername, item._ProxyPassword);
                        SearchCriteria.LoginID = item._Username;

                        obj_profileRank.logger.addToLogger += new EventHandler(ProfileRankLogEvents_addToLogger);

                        obj_profileRank.LinkedInProfilerank(ref objGlobusHttpHelper);

                        obj_profileRank.logger.addToLogger -= new EventHandler(ProfileRankLogEvents_addToLogger);


                    }

                    //InBoardPro.LinkedinSearch obj_LinkedinSearch = new InBoardPro.LinkedinSearch(item._Username, item._Password, item._ProxyAddress, item._ProxyPort, item._ProxyUsername, item._ProxyPassword);
                }
                catch
                { }
            }
            catch
            { }
            finally
            {
                btnStartRank.Invoke(new MethodInvoker(delegate
                {
                    AddLoggerProfileRank("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                    AddLoggerProfileRank("------------------------------------------------------------------------------------------------------------------------------------");
                    btnStartRank.Cursor = Cursors.Default;
                }));
            }

        }
        #endregion StartProfileRank

        #region AddLoggerProfileRank

        private void AddLoggerProfileRank(string log)
        {
            try
            {
                if (lstboxRank.InvokeRequired)
                {
                    lstboxRank.Invoke(new MethodInvoker(delegate
                    {
                        lstboxRank.Items.Add(log);
                        lstboxRank.SelectedIndex = lstboxRank.Items.Count - 1;
                    }));
                }
                else
                {
                    lstboxRank.Items.Add(log);
                    lstboxRank.SelectedIndex = lstboxRank.Items.Count - 1;
                }
            }
            catch
            {
            }
        }

        #endregion

        #region AddLoggerShare
        private void AddLoggerShare(string log)
        {
            try
            {
                if (lstboxShareLogger.InvokeRequired)
                {
                    lstboxShareLogger.Invoke(new MethodInvoker(delegate
                    {
                        lstboxShareLogger.Items.Add(log);
                        lstboxShareLogger.SelectedIndex = lstboxShareLogger.Items.Count - 1;
                    }));
                }
                else
                {
                    lstboxShareLogger.Items.Add(log);
                    lstboxShareLogger.SelectedIndex = lstboxShareLogger.Items.Count - 1;
                }
            }
            catch
            {
            }
        }
        #endregion

        private void cmbSelAccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            //comboBoxemail.Invoke(new MethodInvoker(delegate
            //{
            //    SearchCriteria.LoginID = cmbSelAccount.SelectedItem.ToString();
            //}));
        }

        private void btnStopRank_Click(object sender, EventArgs e)
        {
            try
            {
                _IsStop_ProfileRank = true;
                List<Thread> lstTemp = lstProfileRankThread.Distinct().ToList();
                foreach (Thread item in lstTemp)
                {
                    try
                    {
                        item.Abort();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error >>> " + ex.StackTrace);
                    }
                }

                AddLoggerProfileRank("-------------------------------------------------------------------------------------------------------------------------------");
                AddLoggerProfileRank("[ " + DateTime.Now + " ] => [ PROCESS STOPPED ]");
                AddLoggerProfileRank("-------------------------------------------------------------------------------------------------------------------------------");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error >>> " + ex.StackTrace);
            }
        }

        private void createCompany_Click(object sender, EventArgs e)
        {
            try
            {
                //frmCreateCompany objfrmCreateCompany = new frmCreateCompany();
                //objfrmCreateCompany.Show();
            }
            catch { }
        }

        private void inviteGroupsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                frmGroupsInvitation obj_frmGroupsInvitation = new frmGroupsInvitation();
                obj_frmGroupsInvitation.Show();
            }
            catch { }
        }

        private void MessageReceivedScraper_Click(object sender, EventArgs e)
        {
            try
            {

                // frmMessageReceivedScraper obj_frmMessageReceivedScraper = new frmMessageReceivedScraper();
                //obj_frmMessageReceivedScraper.Show();
            }
            catch
            { }
        }

        private void CompanyEmployeeScraperToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                CompanyEmployeeScraper obj_frmCompanyEmployeeScraper = new CompanyEmployeeScraper();
                obj_frmCompanyEmployeeScraper.Show();
            }
            catch
            { }
        }


        private void chkUniqueConnectin_CheckedChanged(object sender, EventArgs e)
        {
            //if (chkUniqueConnectin.Checked)
            //{
            //    txtNumberOfRequestPerKeyword.Enabled = false;
            //}
            //else
            //{
            //    txtNumberOfRequestPerKeyword.Enabled = true;
            //}
        }


        private void btn_SaveCampaignScraper_Click(object sender, EventArgs e)
        {
            string Location = string.Empty;
            string LocationArea = string.Empty;
            string Country = string.Empty;
            string PostalCode = string.Empty;
            string IndustryType = string.Empty;
            string CompanySize = string.Empty;
            string Group = string.Empty;
            string SeniorLevel = string.Empty;
            string Language = string.Empty;
            string Relationship = string.Empty;
            string Function = string.Empty;
            string IntrestedIn = string.Empty;
            string YearsOfExperience = string.Empty;
            string RecentlyJoined = string.Empty;
            string Fortune1000 = string.Empty;
            string within = string.Empty;
            string TitleValue = string.Empty;
            string CompanyValue = string.Empty;
            string FileName = string.Empty;
            string FirstName = string.Empty;
            string LastName = string.Empty;
            string Keyword = string.Empty;
            string Title = string.Empty;
            string Company = string.Empty;

            string CampaignName = string.Empty;
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
            if (CheckNetConn)
            {
                try
                {
                    try
                    {
                        SearchCriteria.starter = true;
                        if (comboBoxemail.Items.Count <= 0)
                        {
                            AddLoggerScrapeUsers("[ " + DateTime.Now + " ] => [ Load The account From Menu and switch the tab and again come to InBoardProGetData Tab ]");
                            lbGeneralLogs.Items.Clear();
                            frmAccounts FrmAccount = new frmAccounts();
                            FrmAccount.Show();
                            return;
                        }

                        if (comboBoxemail.SelectedIndex < 0)
                        {
                            AddLoggerScrapeUsers("[ " + DateTime.Now + " ] => [ Select The Email Id From The Dropdown and Fill All Mandatory (*) Fields  ]");
                        }

                        if (string.IsNullOrEmpty(txtScraperExportFilename.Text))
                        {
                            MessageBox.Show("Please Add any Name to Export File Name");
                            return;
                        }

                        AddLoggerScrapeUsers("[ " + DateTime.Now + " ] => [ Starting Scraping ]");
                        IsStop_InBoardProGetDataThread = false;
                        lstInBoardProGetDataThraed.Clear();

                        if (string.IsNullOrEmpty(txtBoxCampaignScraperName.Text))
                        {
                            MessageBox.Show("Please write the Campaign Name and then click on Save Campaign button.");
                            return;
                        }


                        if (combScraperLocation.SelectedIndex == 0)
                        {
                            Location = "I";
                            lblScraperCountry.Visible = true;
                            CombScraperCountry.Visible = true;
                        }
                        else if (combScraperLocation.SelectedIndex == 1)
                        {
                            Location = "Y";
                            lblScraperCountry.Visible = false;
                            CombScraperCountry.Visible = false;
                        }

                        foreach (KeyValuePair<string, string> item in CountryCode)
                        {
                            try
                            {
                                if (item.Value == CombScraperCountry.SelectedItem.ToString())
                                {
                                    Country = item.Key;
                                    break;
                                }
                            }
                            catch (Exception ex)
                            {
                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> getting CountryCode ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error -->  getting CountryCode ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPreScrapperErrorLogs);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnSearchScraper_Click() ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnSearchScraper_Click() ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPreScrapperErrorLogs);
                    }


                    #region AreaWiseLocation

                    foreach (var item in listAreaLocation)
                    {
                        string getAreWiseCode = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/ta/region?query=" + item.ToString()));

                        int startindex = getAreWiseCode.IndexOf("\"id\":");
                        int startindex1 = getAreWiseCode.IndexOf("displayName");
                        //{"resultList":[{"id":"in:6498","headLine":"<strong>Bhilai<\/strong> Area, India","displayName":"Bhilai Area, India","subLine":""}]}

                        if (startindex > 0)
                        {
                            try
                            {
                                string start = getAreWiseCode.Substring(startindex).Replace("\"id\":", string.Empty);
                                int endindex = start.IndexOf(",");
                                string end = start.Substring(0, endindex).Replace(":", "%3A").Replace("\"", "");
                                //SearchCriteria.LocationAreaCode = end.Replace(":", "%").Replace("\"", "");

                                string start1 = getAreWiseCode.Substring(startindex1).Replace("displayName", string.Empty);
                                int endindex1 = start1.IndexOf("\",\"");
                                string end1 = start1.Substring(0, endindex1).Replace(":", "").Replace("\"", "");


                                if (string.IsNullOrEmpty(LocationArea))
                                {
                                    LocationArea = end;
                                }
                                else
                                {
                                    LocationArea = LocationArea + "," + end;
                                }

                                string contrycd = end.Split('%')[0].Replace("3A", string.Empty);
                                if (contrycd != Country)
                                {
                                    AddLoggerScrapeUsers("[ " + DateTime.Now + " ] => [ Wrong Selection of Country or Your Area Location is not matched the country ]");
                                    return;
                                }
                                else
                                {
                                    AddLoggerScrapeUsers("[ " + DateTime.Now + " ] => [ Added Area Location: " + end1.ToString() + " ]");

                                }


                            }
                            catch { }

                        }
                    }


                    #endregion

                    #region Industry List
                    string Industryvalue = string.Empty;
                    string[] arrIndustry = Regex.Split(IndustryList, ",");

                    foreach (string Industry in checkedListBoxIndustry.CheckedItems)
                    {
                        foreach (string itemIndustry in arrIndustry)
                        {
                            try
                            {
                                if (itemIndustry.Contains(Industry))
                                {
                                    string[] arryIndustry = Regex.Split(itemIndustry, ";");
                                    if (arryIndustry.Length == 2)
                                    {
                                        if (!string.IsNullOrEmpty(arryIndustry[1]))
                                        {
                                            if (string.IsNullOrEmpty(Industryvalue))
                                            {
                                                Industryvalue = arryIndustry[1];
                                            }
                                            else
                                            {
                                                Industryvalue = Industryvalue + "," + arryIndustry[1];
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> arrIndustry 1 ---2--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> arrIndustry 1 ---2--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPreScrapperErrorLogs);
                            }
                        }
                    }
                    //if (string.IsNullOrEmpty(Industryvalue))
                    //{
                    //    Industryvalue = "select-all";
                    //}
                    IndustryType = Industryvalue;
                    #endregion

                    #region Company size List
                    string companysizevalue = string.Empty;
                    string[] arraycompnaysize = Regex.Split(companysizelist, ",");
                    foreach (string companysize in checkedListBoxCompanySize.CheckedItems)
                    {
                        foreach (string item in arraycompnaysize)
                        {
                            try
                            {
                                if (item.Split(':')[1] == companysize)
                                {
                                    string[] ArrayCompnay = Regex.Split(item, ":");
                                    if (!string.IsNullOrEmpty(ArrayCompnay[0]))
                                    {
                                        if (string.IsNullOrEmpty(companysizevalue))
                                        {
                                            companysizevalue = ArrayCompnay[0];
                                        }
                                        else
                                        {
                                            companysizevalue = companysizevalue + "," + ArrayCompnay[0];
                                        }
                                    }
                                    //break;
                                }
                            }
                            catch (Exception ex)
                            {
                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnSearchScraper_Click() ---3--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnSearchScraper_Click() ---3--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPreScrapperErrorLogs);
                            }
                        }
                        //break;
                    }

                    if (companysizevalue == "all")
                    {
                        CompanySize = string.Empty;
                    }
                    else
                    {
                        CompanySize = companysizevalue;
                    }
                    #endregion

                    #region Group List
                    string Groupsvalue = string.Empty;
                    foreach (string Groups in checkedListGroups.CheckedItems)
                    {
                        //foreach (var item1 in GroupStatus.GroupMemUrl)
                        //{
                        //    string[] grp = item1.Split(':');
                        //    if (SelUser[1] == grp[0])
                        //    {
                        //        SpeGroupId = grp[2];
                        //        break;
                        //    }
                        //}


                        foreach (var item in GroupStatus.GroupMemUrl)
                        {
                            try
                            {
                                string[] grp = item.Split(':');
                                if (grp[0].Equals(Groups))
                                {
                                    if (!string.IsNullOrEmpty(grp[2]))
                                    {
                                        if (string.IsNullOrEmpty(Groupsvalue))
                                        {
                                            Groupsvalue = grp[2];
                                        }
                                        else
                                        {
                                            Groupsvalue = Groupsvalue + "," + grp[2];
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnSearchScraper_Click() ---4--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnSearchScraper_Click() ---4--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPreScrapperErrorLogs);
                            }
                        }
                    }
                    Group = Groupsvalue;
                    #endregion

                    #region Senority List
                    string Seniorlevelvalue = string.Empty;
                    string[] senoirLevelList = Regex.Split(senioritylevel, ",");
                    foreach (string Seniorlevel in checkedListBoxSeniorlevel.CheckedItems)
                    {
                        foreach (string itemSeniorLevel in senoirLevelList)
                        {
                            try
                            {
                                if (itemSeniorLevel.Contains(Seniorlevel))
                                {
                                    string[] arrysenoirLevel = Regex.Split(itemSeniorLevel, ":");
                                    if (arrysenoirLevel.Length == 2)
                                    {
                                        if (!string.IsNullOrEmpty(arrysenoirLevel[0]))
                                        {
                                            if (string.IsNullOrEmpty(Seniorlevelvalue))
                                            {
                                                Seniorlevelvalue = arrysenoirLevel[0];
                                            }
                                            else
                                            {
                                                Seniorlevelvalue = Seniorlevelvalue + "," + arrysenoirLevel[0];
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> arrIndustry 1 ---2--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> arrIndustry 1 ---2--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPreScrapperErrorLogs);
                            }
                        }
                    }

                    if (Seniorlevelvalue == "all")
                    {
                        SeniorLevel = string.Empty;
                    }
                    else
                    {
                        SeniorLevel = Seniorlevelvalue;
                    }
                    #endregion

                    #region Language List
                    string language = string.Empty;
                    string[] arrLang = Regex.Split(Language, ",");
                    foreach (string LanguageL in checkedListBoxLanguage.CheckedItems)
                    {
                        foreach (string item in arrLang)
                        {
                            try
                            {
                                if (item.Contains(LanguageL))
                                {
                                    string[] arryLang = Regex.Split(item, ";");
                                    if (arryLang.Length == 2)
                                    {
                                        if (!string.IsNullOrEmpty(arryLang[1]))
                                        {
                                            if (string.IsNullOrEmpty(language))
                                            {
                                                language = arryLang[1];
                                            }
                                            else
                                            {
                                                language = language + "," + arryLang[1];
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnSearchScraper_Click() ---6--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnSearchScraper_Click() ---6--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPreScrapperErrorLogs);
                            }
                        }
                    }
                    if (string.IsNullOrEmpty(language))
                    {
                        //language = "select-all";
                        language = "";
                    }
                    language = language;
                    #endregion

                    #region Relationship List
                    string Relationships = string.Empty;
                    string[] arrRelation = Regex.Split(RelationshipList, ",");
                    foreach (string RelationL in checkedListRelationship.CheckedItems)
                    {
                        foreach (string item in arrRelation)
                        {
                            try
                            {
                                if (item.Contains(RelationL))
                                {
                                    string[] arryRelat = Regex.Split(item, ":");
                                    if (arryRelat.Length == 2)
                                    {
                                        if (!string.IsNullOrEmpty(arryRelat[0]))
                                        {
                                            if (string.IsNullOrEmpty(Relationships))
                                            {
                                                Relationships = arryRelat[0];
                                            }
                                            else
                                            {
                                                Relationships = arryRelat[0] + "," + Relationships;
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnSearchScraper_Click() ---7--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnSearchScraper_Click() ---7--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPreScrapperErrorLogs);
                            }
                        }
                    }

                    if (Relationships == "N")
                    {
                        Relationship = string.Empty;
                    }
                    else
                    {
                        Relationship = Relationships;
                    }
                    #endregion

                    #region Function List
                    string Functions = string.Empty;
                    string[] FunctionList = Regex.Split(Functionlist, ",");
                    foreach (string FunctionL in checkedListFunction.CheckedItems)
                    {
                        foreach (string itemFunction in FunctionList)
                        {
                            try
                            {
                                if (itemFunction.Contains(FunctionL))
                                {
                                    string[] functionItem = Regex.Split(itemFunction, ":");
                                    if (!string.IsNullOrEmpty(functionItem[0]))
                                    {
                                        if (string.IsNullOrEmpty(Functions))
                                        {
                                            Functions = functionItem[0];
                                        }
                                        else
                                        {
                                            Functions = Functions + "," + functionItem[0];
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnSearchScraper_Click() ---8--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnSearchScraper_Click() ---8--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPreScrapperErrorLogs);
                            }
                        }
                    }
                    if (Functions == "all")
                    {
                        Function = string.Empty;
                    }
                    else
                    {
                        Function = Functions;
                    }
                    #endregion

                    #region IntrestedIn
                    string Interested_In = string.Empty;
                    string[] IntrestesList = Regex.Split(IntrestedinList, ",");
                    foreach (string InterestedL in checkedListBoxInterestedIN.CheckedItems)
                    {
                        foreach (string Intresteditem in IntrestesList)
                        {
                            try
                            {
                                if (Intresteditem.Contains(InterestedL))
                                {
                                    string[] arrayIntrst = Regex.Split(Intresteditem, ":");
                                    if (!string.IsNullOrEmpty(Intresteditem))
                                    {
                                        if (string.IsNullOrEmpty(Interested_In))
                                        {
                                            Interested_In = arrayIntrst[0];
                                        }
                                        else
                                        {
                                            Interested_In = Interested_In + "," + arrayIntrst[0];
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnSearchScraper_Click() ---9--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnSearchScraper_Click() ---9--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPreScrapperErrorLogs);
                            }
                        }
                    }
                    if (Interested_In == "select-all")
                    {
                        IntrestedIn = string.Empty;
                    }
                    else
                    {
                        IntrestedIn = Interested_In;
                    }

                    #endregion

                    #region Explerience list
                    string TotalExperience = string.Empty;
                    string[] arratExpericne = Regex.Split(expList, ",");
                    foreach (string YearOfExperienceL in GetDataForPrimiumAccount.CheckedItems)
                    {
                        foreach (string itemExp in arratExpericne)
                        {
                            try
                            {
                                if (itemExp.Contains(YearOfExperienceL))
                                {
                                    string[] arrayitem = Regex.Split(itemExp, ":");
                                    if (!string.IsNullOrEmpty(arrayitem[1]))
                                    {
                                        if (string.IsNullOrEmpty(TotalExperience))
                                        {
                                            TotalExperience = arrayitem[0];
                                        }
                                        else
                                        {
                                            TotalExperience = TotalExperience + "," + arrayitem[0];
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnSearchScraper_Click() ---10--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnSearchScraper_Click() ---10--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPreScrapperErrorLogs);
                            }
                        }
                    }

                    if (TotalExperience == "select-all")
                    {
                        YearsOfExperience = string.Empty;
                    }
                    else
                    {
                        YearsOfExperience = TotalExperience;
                    }
                    #endregion

                    #region Recently Joined
                    string Recently_Joined = string.Empty;
                    string[] RecetlyJoinedArr = Regex.Split(RecentlyjoinedList, ",");
                    foreach (string RecentlyJoinedL in checkedListBoxRecentlyJoined.CheckedItems)
                    {
                        foreach (string item in RecetlyJoinedArr)
                        {
                            try
                            {
                                string[] arrayitem = Regex.Split(item, ":");
                                if (item.Contains(RecentlyJoinedL))
                                {
                                    if (!string.IsNullOrEmpty(arrayitem[0]))
                                    {
                                        if (string.IsNullOrEmpty(Recently_Joined))
                                        {
                                            Recently_Joined = arrayitem[0];
                                        }
                                        else
                                        {
                                            Recently_Joined = Recently_Joined + "," + arrayitem[0];
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnSearchScraper_Click() ---11--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnSearchScraper_Click() ---11--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPreScrapperErrorLogs);
                            }
                        }
                    }
                    if (Recently_Joined == "select-all")
                    {
                        Recently_Joined = string.Empty;
                    }
                    else
                    {
                        RecentlyJoined = Recently_Joined.Trim();
                    }
                    #endregion

                    #region Fortune List
                    string Fortune_1000 = string.Empty;
                    string[] FortuneArr = Regex.Split(fortuneList, ",");
                    foreach (string Fortune1000L in checkedListBoxFortune1000.CheckedItems)
                    {
                        foreach (string item in FortuneArr)
                        {
                            try
                            {
                                string[] arrayItem = Regex.Split(item, ":");
                                if (item.Contains(Fortune1000L))
                                {
                                    if (!string.IsNullOrEmpty(arrayItem[0]))
                                    {
                                        if (string.IsNullOrEmpty(Fortune_1000))
                                        {
                                            Fortune_1000 = arrayItem[0];
                                        }
                                        else
                                        {
                                            Fortune_1000 = Fortune_1000 + "," + arrayItem[0];
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnSearchScraper_Click() ---12--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnSearchScraper_Click() ---12--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPreScrapperErrorLogs);
                            }
                        }
                    }
                    if (RecentlyJoined == "select-all")
                    {
                        Fortune1000 = string.Empty;
                    }
                    else
                    {
                        Fortune1000 = Fortune_1000;
                    }
                    #endregion

                    #region Within the PostalCode
                    try
                    {
                        string[] arraywithinList = Regex.Split(WithingList, ",");
                        foreach (string item in arraywithinList)
                        {
                            if (item.Contains(cmbboxWithin.SelectedItem.ToString()))
                            {
                                string[] arrayWithin = Regex.Split(item, ":");
                                within = arrayWithin[0];
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    #endregion

                    #region within Title value
                    try
                    {
                        string[] arrayTitleList = Regex.Split(TitleValue, ",");
                        foreach (string item in arrayTitleList)
                        {
                            string[] arrayTitleValue = Regex.Split(item, ":");
                            if (arrayTitleValue[1] == cmbboxTitle.SelectedItem.ToString())
                            {
                                TitleValue = arrayTitleValue[0];
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    #endregion

                    #region within Companyvalue
                    try
                    {
                        if (cmbboxCompanyValue.Enabled == true)
                        {
                            string[] arrayTitleList = Regex.Split(TitleValue, ",");
                            foreach (string item in arrayTitleList)
                            {
                                string[] arrayTitleValue = Regex.Split(item, ":");

                                if (arrayTitleValue[1] == cmbboxCompanyValue.SelectedItem.ToString())
                                {
                                    CompanyValue = arrayTitleValue[0];
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    #endregion

                    FileName = (txtScraperExportFilename.Text).Replace("\"", string.Empty).Replace(",", string.Empty).ToString();
                    FirstName = txtScrperFirstname.Text;
                    LastName = txtScrperlastname.Text;
                    Keyword = txtKeyword.Text;
                    Title = txtTitle.Text;
                    if (Keyword.Contains("or") || (Keyword.Contains("and")) || (Keyword.Contains("not")))
                    {
                        try
                        {
                            Keyword = Keyword.Replace(" or ", " OR ");
                            Keyword = Keyword.Replace(" and ", " AND ");
                            Keyword = Keyword.Replace(" not ", " NOT ");
                        }
                        catch { }
                    }
                    if (Title.Contains("or") || (Title.Contains("and")) || (Title.Contains("not")))
                    {
                        try
                        {
                            Title = Title.Replace(" or ", " OR ");
                            Title = Title.Replace(" and ", " AND ");
                            Title = Title.Replace(" not ", " NOT ");
                        }
                        catch { }
                    }
                    if (!string.IsNullOrEmpty(txtPostalcode.Text))
                    {
                        PostalCode = txtPostalcode.Text;
                    }
                    if (!string.IsNullOrEmpty(txtCompnayName.Text))
                    {
                        Company = txtCompnayName.Text;
                    }

                    if (CombScraperCountry.SelectedItem.ToString() != null && CombScraperCountry.SelectedIndex > 0)
                    {
                        try
                        {
                            if (!string.IsNullOrEmpty(CombScraperCountry.SelectedItem.ToString()))
                            {
                                string countryValue = string.Empty;
                                string temCOuntry = CombScraperCountry.SelectedItem.ToString();
                                GetCountryNameValue(ref temCOuntry, ref countryValue);
                                Country = countryValue;
                            }
                        }
                        catch (Exception ex)
                        {
                            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnSearchScraper_Click() ---13--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnSearchScraper_Click() ---13--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPreScrapperErrorLogs);
                        }
                    }
                    CampaignName = txtBoxCampaignScraperName.Text;
                    string Account = comboBoxemail.SelectedItem.ToString();
                    clsDBQueryManager obj_clsDBQueryManager = new clsDBQueryManager();
                    DataTable DT = new DataTable();
                    DataSet DS = new DataSet();
                    DS = obj_clsDBQueryManager.getDataFromAccountsTable(Account);
                    DT = DS.Tables[0];
                    string Password = string.Empty;
                    string ProxyAddress = string.Empty;
                    string ProxyPort = string.Empty;
                    string ProxyUserName = string.Empty;
                    string ProxyPassword = string.Empty;
                    foreach (DataRow itemAccount in DT.Rows)
                    {
                        try
                        {
                            Password = itemAccount["Password"].ToString();
                            ProxyAddress = itemAccount["ProxyAddress"].ToString();
                            ProxyPort = itemAccount["ProxyPort"].ToString();
                            ProxyUserName = itemAccount["ProxyUserName"].ToString();
                            ProxyPassword = itemAccount["ProxyPassword"].ToString();
                        }
                        catch
                        { }
                    }

                    //database query to insert data
                    string query = "Insert into tb_CampaignScraper (CampaignName, Account, FirstName, LastName, Location, Country, AreaWiseLocation, PostalCode, Company, Keyword, Title, Industry, Relationship, Language, Groups, ExportedFileName, TitleValue, CompanyValue, within, YearsOfExperience, Function, SeniorLevel, IntrestedIn, CompanySize, Fortune1000, RecentlyJoined) Values ('" + CampaignName + "','" + Account + ":" + Password + ":" + ProxyAddress + ":" + ProxyPort + ":" + ":" + ProxyUserName + ":" + ProxyPassword + "','" + FirstName + "','" + LastName + "','" + Location + "','" + Country + "','" + LocationArea + "','" + PostalCode + "','" + Company + "','" + Keyword + "','" + Title + "','" + IndustryType + "','" + Relationship + "','" + language + "','" + Group + "','" + FileName + "','" + TitleValue + "','" + CompanyValue + "','" + within + "','" + YearsOfExperience + "','" + Function + "','" + SeniorLevel + "','" + IntrestedIn + "','" + CompanySize + "','" + Fortune1000 + "','" + RecentlyJoined + "');";


                    obj_clsDBQueryManager.InsertCampaignScraperData(query, "tb_CampaignScraper");

                    frmCampaignScraper obj_frmCampaignScraper = new frmCampaignScraper();
                    obj_frmCampaignScraper.Show();

                    AddLoggerScrapeUsers("[ " + DateTime.Now + " ] => [ Campaign Details saved with the campaign name :" + CampaignName + " ]");
                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnSearchScraper_Click() ---13--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnSearchScraper_Click() ---13--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPreScrapperErrorLogs);
                }
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddLoggerScrapeUsers("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        }

        private void btn_ShowCampaignScraperDetails_Click(object sender, EventArgs e)
        {
            frmCampaignScraper obj_frmCampaignScraper = new frmCampaignScraper();
            obj_frmCampaignScraper.Show();
        }

        private void btnStopCampaingEndorsement_Click(object sender, EventArgs e)
        {
            try
            {
                IsStop = true;
                List<Thread> lstTemp = lstEndorsementThread.Distinct().ToList();
                foreach (Thread item in lstTemp)
                {
                    try
                    {
                        item.Abort();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error >>> " + ex.StackTrace);
                    }
                }

                AddLoggerEndorsePeople("-------------------------------------------------------------------------------------------------------------------------------");
                AddLoggerEndorsePeople("[ " + DateTime.Now + " ] => [ PROCESS STOPPED ]");
                AddLoggerEndorsePeople("-------------------------------------------------------------------------------------------------------------------------------");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error >>> " + ex.StackTrace);
            }
        }

        private void btnBrowseWebsiteUrl_Click(object sender, EventArgs e)
        {
            try
            {
                txtWebsiteUrl.Text = "";
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "Text Files (*.txt)|*.txt";
                    ofd.InitialDirectory = Application.StartupPath;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtWebsiteUrl.Text = ofd.FileName;
                        List<string> templist = GlobusFileHelper.ReadFile(ofd.FileName);
                        //lstEventURLsFile = new List<string>();
                        lstLinkedinShareWebsiteUrl.Clear();
                        foreach (string item in templist)
                        {
                            lstLinkedinShareWebsiteUrl.Add(item);
                        }
                        lstLinkedinShareWebsiteUrl = lstLinkedinShareWebsiteUrl.Distinct().ToList();
                        AddLoggerLinkedinSearch("[ " + DateTime.Now + " ] => [ " + lstLinkedinShareWebsiteUrl.Count + " URLs loaded to share. ]");
                        //AddToListWallPost(lstWallMessage.Count + " Wall post message ");
                    }
                }
            }
            catch
            {
            }
        }

        private void btnShareStart_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {
                try
                {
                    lstShareThread.Clear();

                    #region thread is stopped or not
                    try
                    {

                        if (_isStop_Share)
                        {
                            _isStop_Share = false;
                        }

                        AddLoggerShare("[ " + DateTime.Now + " ] => [ Starting Process ]");

                        _isStop_Share = false;
                        shareSelectedEmailId = cmbSelAccountShare.SelectedItem.ToString();
                        btnStartRank.Cursor = Cursors.AppStarting;

                        Thread thread_ProfileRank = new Thread(StartLinkedinShare);
                        thread_ProfileRank.Start();

                    }
                    catch
                    { }
                    #endregion
                }
                catch
                { }
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddLoggerShare("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        }

        public void StartLinkedinShare()
        {
            GlobusHttpHelper objGlobusHttpHelper = new GlobusHttpHelper();

            #region thread is stopped or not
            try
            {
                if (!_isStop_Share)
                {
                    lstShareThread.Add(Thread.CurrentThread);
                    lstShareThread.Distinct().ToList();
                    Thread.CurrentThread.IsBackground = true;
                }
            }
            catch
            { }
            #endregion

            try
            {
                AddLoggerShare("[ " + DateTime.Now + " ] => [ Logging in With UserName >>> " + shareSelectedEmailId + " ]");

                //objGlobusHttpHelper.gCookies = null;
                if (SearchCriteria.SignIN)
                {
                    LinkedinLogin Login = new LinkedinLogin();
                    //For Sign Out +9+
                    Login.LoginHttpHelper(ref objGlobusHttpHelper);

                    SearchCriteria.SignOut = true;
                }
                if (SearchCriteria.SignOut)
                {
                    LinkedInMaster item = null;
                    LinkedInManager.linkedInDictionary.TryGetValue(shareSelectedEmailId, out item);

                    item.LoginHttpHelper(ref objGlobusHttpHelper);


                    AddLoggerShare("[ " + DateTime.Now + " ] => [ Login Process is running... ]");
                    if (item.IsLoggedIn)
                    {
                        if (SearchCriteria.loginREsponce.Contains("[ " + DateTime.Now + " ] => [ Your LinkedIn account has been temporarily restricted ]"))
                        {
                            AddLoggerShare("[ " + DateTime.Now + " ] => [ " + shareSelectedEmailId + "Your LinkedIn account has been temporarily restricted ]");


                        }

                        if (SearchCriteria.loginREsponce.Contains("Please confirm your email address"))
                        {
                            AddLoggerShare("[ " + DateTime.Now + " ] => [ " + shareSelectedEmailId + " account has been temporarily restricted Please confirm your email address ]");


                        }
                        SearchCriteria.SignIN = true;
                        SearchCriteria.SignOut = false;
                        if (item.IsLoggedIn)
                        {
                            AddLoggerShare("[ " + DateTime.Now + " ] => [ Logged in Successfully With UserName >>> " + shareSelectedEmailId + " ]");
                        }
                        else
                        {
                            AddLoggerShare("[ " + DateTime.Now + " ] => [ Couldn't Login   With UserName >>> " + shareSelectedEmailId + " ]");
                        }
                    }
                    else
                    {
                        AddLoggerShare("[ " + DateTime.Now + " ] => [ Couldn't Login With UserName >>> " + shareSelectedEmailId + " ]");
                        return;
                    }

                    SearchCriteria.LoginID = item._Username;

                    //Check HERE!!!!
                    try
                    {
                        foreach (string webUrlAndShareText in lstLinkedinShareWebsiteUrl)
                        {
                            UrlShare(ref objGlobusHttpHelper, item._Username, item._Password, item._ProxyAddress, item._ProxyPort, item._ProxyUsername, item._ProxyPassword, webUrlAndShareText);
                        }
                    }
                    catch { }

                    finally
                    {
                        if (btnShareStart.InvokeRequired)
                            btnShareStart.Invoke(new MethodInvoker(delegate
                            {
                                AddLoggerShare("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                            }));
                    }
                }
            }
            catch
            { }
        }

        #region Start Sharing
        public void UrlShare(ref GlobusHttpHelper objGlobusHttpHelper, string username, string password, string proxyAddress, string proxyPort, string proxyUserName, string proxyPassword, string webUrlAndText)
        {
            try
            {
                if (_isStop_Share)
                {
                    return;
                }
                lstShareThread.Add(Thread.CurrentThread);
                lstShareThread = lstShareThread.Distinct().ToList();
                Thread.CurrentThread.IsBackground = true;

                AddLoggerShare("[ " + DateTime.Now + " ] => [ Starting process to share the link on account : " + username + " ]");

                string getResponseWebUrl = string.Empty;
                string shareUrl = string.Empty;
                string csrfToken = string.Empty;
                string contentId = string.Empty;
                string contentUrl = string.Empty;
                string contentResolvedUrl = string.Empty;
                string contentType = string.Empty;
                string contentTitle = string.Empty;
                string contentDescription = string.Empty;
                string mentions = "%5B%5D";
                string shareText = string.Empty;
                string webUrl = string.Empty;
                string distNetwork = string.Empty;
                try
                {
                    if (webUrlAndText.Contains(";"))
                    {
                        string[] url_Text_Split = Regex.Split(webUrlAndText, ";");
                        webUrl = url_Text_Split[0];
                        shareText = url_Text_Split[1];
                    }
                    else
                    {
                        webUrl = webUrlAndText;
                    }

                }
                catch
                { }

                try
                {
                    if (!webUrl.Contains("http"))
                    {
                        webUrl = "http://" + webUrl;
                    }
                }
                catch
                { }

                getResponseWebUrl = objGlobusHttpHelper.getHtmlfromUrl(new Uri(webUrl));
                shareUrl = "https://www.linkedin.com/sharing/share?url=" + Uri.EscapeDataString(webUrl) + "&trk=LI_BADGE";
                string getResponseShareUrl = objGlobusHttpHelper.getHtmlfromUrl(new Uri(shareUrl));
                if (!getResponseShareUrl.Contains("csrfToken"))
                {
                    Thread.Sleep(3000);
                    getResponseShareUrl = objGlobusHttpHelper.getHtmlfromUrl(new Uri(shareUrl));
                    if (!getResponseShareUrl.Contains("csrfToken"))
                    {
                        Thread.Sleep(3000);
                        getResponseShareUrl = objGlobusHttpHelper.getHtmlfromUrl(new Uri(shareUrl));
                    }
                }
                csrfToken = getBetween(getResponseShareUrl, "\"csrfToken\":\"", "\"");
                contentId = getBetween(getResponseShareUrl, "name=\"content.id\" value=\"", "\"");
                contentUrl = getBetween(getResponseShareUrl, "name=\"content.url\" value=\"", "\"");
                contentResolvedUrl = getBetween(getResponseShareUrl, "name=\"content.resolvedUrl\" value=\"", "\"");
                contentType = getBetween(getResponseShareUrl, "name=\"contentType\" value=\"", "\"");
                contentTitle = getBetween(getResponseShareUrl, "name=\"content.title\" value=\"", "\"");
                distNetwork = "PUBLIC";

                string webActionTrackUrl = "https://www.linkedin.com/lite/web-action-track?csrfToken=" + Uri.EscapeDataString(csrfToken);
                string webActionPostData = "pkey=cws-share-widget&tcode=shr-unwnd-sccss&plist=";
                string postResponseWebAction = objGlobusHttpHelper.postFormData(new Uri(webActionTrackUrl), webActionPostData);

                string postResponseInfluencer = objGlobusHttpHelper.postFormData(new Uri("https://www.linkedin.com/nhome/influencer-entitlement"), string.Empty);

                string finalPostUrl = "https://www.linkedin.com/sharing/share?trk=LI_BADGE";
                string postDataFinalPost = "csrfToken=" + csrfToken + "&content.id=" + contentId + "&content.url=" + Uri.EscapeDataString(contentUrl) + "&content.resolvedUrl=" + Uri.EscapeDataString(contentResolvedUrl) + "&contentType=" + contentType + "&content.title=" + contentTitle + "&content.description=" + contentDescription + "&mentions=" + mentions + "&shareText=" + shareText.Replace(" ", "+") + "&dist.networks%5B0%5D=" + distNetwork;
                string responseFinalPost = objGlobusHttpHelper.postFormData(new Uri(finalPostUrl), postDataFinalPost);

                if (responseFinalPost.Contains("\"status\":\"SUCCESS\""))
                {
                    AddLoggerShare("[ " + DateTime.Now + " ] => [ Successfully shared URL" + shareUrl + " on the account : " + username + " ]");
                }
                else
                {
                    AddLoggerShare("[ " + DateTime.Now + " ] => [ Sharing unsuccessful URL" + shareUrl + " on the account : " + username + " ]");
                }

            }
            catch
            { }
        }
        #endregion



        #region GetBetween
        public string getBetween(string strSource, string strStart, string strEnd)
        {
            int Start, End;
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }
            else
            {
                return "";
            }
        }
        #endregion

        #region tabShare_Paint
        private void tabShare_Paint(object sender, PaintEventArgs e)
        {
            Graphics g;
            g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.DrawImage(image, 0, 0, tabPageGroupUpdate.Width, tabPageGroupUpdate.Height);
        }
        #endregion

        private void btnShareStop_Click(object sender, EventArgs e)
        {
            try
            {
                _isStop_Share = true;

                List<Thread> lstTemp = lstShareThread.Distinct().ToList();

                foreach (Thread item in lstTemp)
                {
                    try
                    {
                        item.Abort();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error >>> " + ex.StackTrace);
                    }
                }

                AddLoggerShare("-------------------------------------------------------------------------------------------------------------------------------");
                AddLoggerShare("[ " + DateTime.Now + " ] => [ PROCESS STOPPED ]");
                AddLoggerShare("-------------------------------------------------------------------------------------------------------------------------------");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error >>> " + ex.StackTrace);
            }
        }

        private void rbPreventMsgHaveAllreadysentWithUSer_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rbPreventMsgHaveAllreadysentWithUSer.Checked == true)
                {
                    preventMsgSameUser = true;
                    preventMsgGlobalUser = false;

                }
                else
                {
                    preventMsgSameUser = false;
                }

            }
            catch { }
        }

        private void rbchkPreventMsgHaveAllreadysentGlobalUser_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rbchkPreventMsgHaveAllreadysentGlobalUser.Checked == true)
                {
                    preventMsgGlobalUser = true;
                    preventMsgSameUser = false;

                }
                else
                {
                    preventMsgGlobalUser = false;
                }

            }
            catch { }
        }


        private void chkClearDbComposeMgs_CheckedChanged(object sender, EventArgs e)
        {
            if (chkClearDbComposeMgs.Checked)
            {
                txtClearDatabaseComposeMgs.Visible = true;
                btnClearDatabaseCompose.Visible = true;

                txtClearDatabaseComposeMgs.Focus();
                ToolTip toolTip = new ToolTip();
                toolTip.SetToolTip(txtClearDatabaseComposeMgs, "Please enter userId!");

            }
            else
            {
                txtClearDatabaseComposeMgs.Visible = false;
                btnClearDatabaseCompose.Visible = false;
            }
        }

        private void btnClearDatabaseCompose_Click(object sender, EventArgs e)
        {
            try
            {

                if (string.IsNullOrEmpty(txtClearDatabaseComposeMgs.Text.Trim()))
                {
                    MessageBox.Show("Please enter valid userId");
                    return;
                }

                DataSet ds = new DataSet();
                string Querystring = "Delete From tb_ManageComposeMsg Where MsgFrom ='" + txtClearDatabaseComposeMgs.Text.Trim() + "'";
                DataBaseHandler.DeleteQuery(Querystring, "tb_ManageComposeMsg");
                MessageBox.Show("Successfully deleted details of user " + txtClearDatabaseComposeMgs.Text.Trim());
                txtClearDatabase.Text = string.Empty;

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                MessageBox.Show("Some problem in deleting details of user " + txtClearDatabaseComposeMgs.Text.Trim());
                return;
            }
        }

        private void blackListedUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                FrmAddBlackListUser frmBlackListedUser = new FrmAddBlackListUser();
                frmBlackListedUser.ShowDialog();
            }
            catch { }
        }

        private void chk_scrpWithoutGoingToMainProf_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_scrpWithoutGoingToMainProf.Checked == true)
            {
                Globals.scrapeWithoutGoingToMainProfile = true;
            }
            else
            {
                Globals.scrapeWithoutGoingToMainProfile = false;
            }
        }

        private void chkBoxScrapeWithoutLoggingIn_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBoxScrapeWithoutLoggingIn.Checked == true)
            {
                Globals.scrapeWithoutLoggingIn = true;
            }
            else
            {
                Globals.scrapeWithoutLoggingIn = false;
            }
        }

        private void salesNavigatorAccountScraper_Click(object sender, EventArgs e)
        {
            try
            {
                frmSalesNavigatorScraper frmSalesNavigatorScraper = new frmSalesNavigatorScraper();
                frmSalesNavigatorScraper.Show();
            }
            catch (Exception ex)
            {
            }
        }

        private void PicBanner_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("http://pvadomination.com/");
            }
            catch { }
        }

        private void btnJobScraperStart_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {

                try
                {
                    
                    if (LinkedInManager.linkedInDictionary.Count() == 0)
                    {
                        try
                        {
                            MessageBox.Show("Please Load LinkedIn Accounts From MyAccounts Menu");
                            AddLoggerShare("[ " + DateTime.Now + " ] => [ Please Load LinkedIn Accounts From MyAccounts Menu ]");

                            lbGeneralLogs.Items.Clear();
                            frmAccounts FrmAccount = new frmAccounts();
                            FrmAccount.Show();
                            return;
                        }
                        catch (Exception ex)
                        {
                        }
                    }

                    if (lstLinkedinJobScraperInputUrl.Count() > 0)
                    {

                    }
                    else
                    {
                        AddLoggerShare("[ " + DateTime.Now + " ] => [ Please Load LinkedIn Url for job scraper.]");
                        return;
                    }
                    btnSendEmailInvite.Cursor = Cursors.AppStarting;

                    new Thread(() => StartJobScraper()).Start();

                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Connection --> btnSendEmailInvite_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Connection --> btnSendEmailInvite_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddConnectionErrorLogs);
                }
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddLoggerManageConnection("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        }

        public void StartJobScraper()
        {
            try
            {
                try
                {
                    lstInBoardProGetDataThraed.Add(Thread.CurrentThread);
                    lstInBoardProGetDataThraed = lstInBoardProGetDataThraed.Distinct().ToList();
                    Thread.CurrentThread.IsBackground = true;
                }
                catch { }

                if (LinkedInManager.linkedInDictionary.Count > 0)
                {
                    AddLoggerShare("[ " + DateTime.Now + " ] => [ Starting Job Scraping]");

                    try
                    {
                        foreach (KeyValuePair<string, LinkedInMaster> item in LinkedInManager.linkedInDictionary)
                        {
                            ThreadPool.SetMaxThreads(5, 5);

                            //ThreadPool.QueueUserWorkItem(new WaitCallback(SendInviteUsingEmails), new object[] { item });
                            StartScrapingJobDetails(new object[] { item });
                           
                            Thread.Sleep(1000);
                        }
                    }
                    catch (Exception ex)
                    {
                        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Connection --> InviteConnectionThread() ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Connection --> InviteConnectionThread() ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddConnectionErrorLogs);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        public void StartScrapingJobDetails(object Parameter)
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
                        try
                        {
                            lstInBoardProGetDataThraed.Add(Thread.CurrentThread);
                            lstInBoardProGetDataThraed = lstInBoardProGetDataThraed.Distinct().ToList();
                            Thread.CurrentThread.IsBackground = true;
                        }
                        catch { }
                    }
                }
                catch
                {
                }

                string account = string.Empty;

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

                //Login.logger.addToLogger += new EventHandler(ShareLogEvents_addToLogger);
                obj_StatusUpdate.logger.addToLogger += new EventHandler(ShareLogEvents_addToLogger);

                AddLoggerShare("[ " + DateTime.Now + " ] => [ Logging with Account: " + Login.accountUser + " ]");
                if (!Login.IsLoggedIn)
                {
                    Login.LoginHttpHelper(ref HttpHelper);
                }

                if (!Login.IsLoggedIn)
                {
                    AddLoggerShare("[ " + DateTime.Now + " ] => [ Couldn't Login With Username >>> " + Login.accountUser + " ]");
                    return;
                }
                AddLoggerShare("[ " + DateTime.Now + " ] => [ Logged with Account: " + Login.accountUser + " ]");
                obj_StatusUpdate.startJobScrapingDetails(ref HttpHelper, lstLinkedinJobScraperInputUrl);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        private void btnJobScraperUploadUrl_Click(object sender, EventArgs e)
        {
            try
            {
                txtJobScraperUrl.Text = "";
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "Text Files (*.txt)|*.txt";
                    ofd.InitialDirectory = Application.StartupPath;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtJobScraperUrl.Text = ofd.FileName;
                        List<string> templist = GlobusFileHelper.ReadFile(ofd.FileName);
                        //lstEventURLsFile = new List<string>();
                        lstLinkedinJobScraperInputUrl.Clear();
                        foreach (string item in templist)
                        {
                            lstLinkedinJobScraperInputUrl.Add(item);
                        }
                        lstLinkedinJobScraperInputUrl = lstLinkedinJobScraperInputUrl.Distinct().ToList();
                        AddLoggerShare("[ " + DateTime.Now + " ] => [ " + lstLinkedinJobScraperInputUrl.Count + " URLs loaded for JOb Scraper. ]");
                        //AddToListWallPost(lstWallMessage.Count + " Wall post message ");
                    }
                }
            }
            catch
            {
            }
        }

        private void btnJobScraperStop_Click(object sender, EventArgs e)
        {
            try
            {
                IsStop_InBoardProGetDataThread = true;
                List<Thread> lstTemp = lstInBoardProGetDataThraed.Distinct().ToList();
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

                List<Thread> lstTemp1 = Globals.lstCompanyEmployeeScraperThread.Distinct().ToList();
                foreach (Thread item in lstTemp1)
                {
                    try
                    {
                        item.Abort();
                    }
                    catch
                    { }
                }

                AddLoggerShare("-------------------------------------------------------------------------------------------------------------------------------");
                AddLoggerShare("[ " + DateTime.Now + " ] => [ PROCESS STOPPED ]");
                AddLoggerShare("-------------------------------------------------------------------------------------------------------------------------------");
                
            }
            catch (Exception ex)
            {
            }
        }

        




    }
}