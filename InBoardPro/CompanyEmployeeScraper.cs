using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Text.RegularExpressions;
using System.Threading;
using InBoardProGetData;
using BaseLib;

namespace InBoardPro
{
    public partial class CompanyEmployeeScraper : Form
    {
        GlobusHttpHelper HttpHelper = new GlobusHttpHelper();
                
        public System.Drawing.Image image;
        bool CheckNetConn = false;
        bool IsStop_CompanyEmployeeScraperThread = false;
        bool _IsKeyword_CompanyEmployeeScraper = false;
        bool _IsProfileURL_CompanyEmployeeScraper = false;
        //public static List<string> listCompanyURL = new List<string>();
        Dictionary<string, string> CountryCode = new Dictionary<string, string>();
        List<Thread> lstCompanyEmployeeScraperThread = new List<Thread>();


        public CompanyEmployeeScraper()
        {
            InitializeComponent();
        }

        private void CompanyEmployeeScraper_Load(object sender, EventArgs e)
        {
            
            image = Properties.Resources.background;
            LinkedinSearch.loggersearch.addToLogger += new EventHandler(LinkedinSearchCompEmplLogEvents_addToLogger);
            LinkedinSearch.loggerscrap.addToLogger += new EventHandler(LinkedinSearchCompEmplLogEvents_addToLogger);
            LinkedInScrape.logger.addToLogger += new EventHandler(LinkedinSearchCompEmplLogEvents_addToLogger);

            LoadPreScrapper();

            comboBoxemail.SelectedIndex = 0;
            ClsSelect ObjSelectMethod = new ClsSelect();
            CountryCode = ObjSelectMethod.getCountry();

            foreach (KeyValuePair<string, string> pair in CountryCode)
            {
                try
                {
                    CombScraperCountry.Items.Add(pair.Value);
                }
                catch
                {
                }
            }
        }

        #region LinkedinSearchFilterLogEvents_addToLogger

        void LinkedinSearchCompEmplLogEvents_addToLogger(object sender, EventArgs e)
        {
            try
            {
                if (e is EventsArgs)
                {
                    EventsArgs eventArgs = e as EventsArgs;
                    AddLoggerCompanyEmployeeSearch(eventArgs.log);
                }
            }
            catch
            {
            }
        }

        #endregion

        private void comboBoxemail_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxemail.Invoke(new MethodInvoker(delegate
            {
                SearchCriteria.LoginID = comboBoxemail.SelectedItem.ToString();
            }));
        }

        #region LoadPreScrapper() method
        private void LoadPreScrapper()
        {
            
            try
            {
                if (LinkedInManager.linkedInDictionary.Count() == 0)
                {
                    MessageBox.Show("Please Load LinkedIn Accounts From MyAccounts Menu");
                    AddLoggerCompanyEmployeeSearch("[ " + DateTime.Now + " ] => [ Please Load LinkedIn Accounts From MyAccounts Menu ]");

                    //lbGeneralLogs.Items.Clear();
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
            catch
            {
            }
        }
        #endregion

        #region PopulateCmo()
        private void PopulateCmo()
        {
            try
            {
                comboBoxemail.Items.Clear();
                foreach (KeyValuePair<string, LinkedInMaster> item in LinkedInManager.linkedInDictionary)
                {
                    comboBoxemail.Items.Add(item.Key);
                }
                //AddLoggerScrapeUsers("[ " + DateTime.Now + " ] => [ Accounts Uploaded.. ]");
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> PopulateCmo() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> PopulateCmo() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPreScrapperErrorLogs);
            }
        }

        #endregion

        private void btnCompanyEmployeeSearch_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
            //LinkedinSearch obj_LinkedinSearch = new LinkedinSearch();
            if (CheckNetConn)
            {

                #region country code

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
                                   
                #endregion

                #region MyRegion
                SearchCriteria.Keyword = txtKeyword.Text.ToString();
                #endregion

                try
                {
                    SearchCriteria.starter = true;
                    if (comboBoxemail.Items.Count <= 0)
                    {
                        AddLoggerCompanyEmployeeSearch("[ " + DateTime.Now + " ] => [ Load The account From Menu and switch the tab and again come to InBoardProGetData Tab ]");
                        frmAccounts FrmAccount = new frmAccounts();
                        FrmAccount.Show();
                        return;
                    }

                    if (comboBoxemail.SelectedIndex < 0)
                    {
                        AddLoggerCompanyEmployeeSearch("[ " + DateTime.Now + " ] => [ Select The Email Id From The Dropdown and Fill All Mandatory (*) Fields ]");
                    }

                    if (string.IsNullOrEmpty(txtCompanyEmployeeScraperURL.Text))
                    {
                        AddLoggerCompanyEmployeeSearch("[ " + DateTime.Now + " ] => [ Please upload URL ]");
                        return;
                    }

                    AddLoggerCompanyEmployeeSearch("[ " + DateTime.Now + " ] => [ Starting Scraping ]");
                    IsStop_CompanyEmployeeScraperThread = false;
                    lstCompanyEmployeeScraperThread.Clear();


                    Thread thread_CompanyEmployeeSearch = new Thread(StartCompanyEmployeeSearch);
                    thread_CompanyEmployeeSearch.Start();
                }
                catch
                { }
            }
        }

        private void StartCompanyEmployeeSearch()
        {
            if (IsStop_CompanyEmployeeScraperThread)
            {
                return;
            }

            lstCompanyEmployeeScraperThread.Add(Thread.CurrentThread);
            lstCompanyEmployeeScraperThread = lstCompanyEmployeeScraperThread.Distinct().ToList();
            Thread.CurrentThread.IsBackground = true;

            HttpHelper = new GlobusHttpHelper();
            LinkedinSearch objlinkscrCompanyFilter = new LinkedinSearch();
            bool isLoggedIn = Login_InBoardProGetData();
            
            if (rdbCompanyEmployeeScraperKeyword.Checked)
            {
                rdbCompanyEmployeeScraperKeyword.Checked = false;
                //AddLoggerCompanyEmployeeSearch("[ " + DateTime.Now + " ] => [ Search Through Keyword ]");
                //_IsKeyword_CompanyEmployeeScraper = true;
                //objlinkscrCompanyFilter._RdbCompanyEmployeeSearchKeyword = true;
                //InBoardPro.LinkedinSearch._Keyword = txtCompanyEmployeeScraperKeyword.Text;
            }

            if (!(rdbCompnayEmployeeScraperURL.Checked))
            {
                rdbCompnayEmployeeScraperURL.Checked = true;
                AddLoggerCompanyEmployeeSearch("[ " + DateTime.Now + " ] => [ Search Through Url ]");
                _IsProfileURL_CompanyEmployeeScraper = true;
                objlinkscrCompanyFilter._RdbCompanyEmployeeSearchURL = true;
            }

            LinkedinSearch._Search = "CompanyEmployeeSearch";
            if (isLoggedIn)
            {
                AddLoggerCompanyEmployeeSearch("[ " + DateTime.Now + " ] => [ Start LinkedIn Crawling ]");
                objlinkscrCompanyFilter.StartLinkedInSearch(ref HttpHelper);
            }
        }

        #region Login_InBoardProGetData()
        private bool Login_InBoardProGetData()
        {
            bool isLoggedin = false;
            try
            {
                try
                {
                    AddLoggerCompanyEmployeeSearch("[ " + DateTime.Now + " ] => [ Process Running Please wait...]");

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

                                    //item.logger.addToLogger += LinkedinSearchLogEvents_addToLogger;
                                    item.LoginHttpHelper(ref HttpHelper);

                                    if (item.IsLoggedIn)
                                    {
                                        AddLoggerCompanyEmployeeSearch("[ " + DateTime.Now + " ] => [ Logged in With : " + acc + " ]");
                                        isLoggedin = true;
                                    }

                                    if (SearchCriteria.loginREsponce.Contains("Your LinkedIn account has been temporarily restricted"))
                                    {
                                        AddLoggerCompanyEmployeeSearch("[ " + DateTime.Now + " ] => [ " + SearchCriteria.LoginID + "Your LinkedIn account has been temporarily restricted ]");
                                    }

                                    if (SearchCriteria.loginREsponce.Contains("Please confirm your email address"))
                                    {
                                        AddLoggerCompanyEmployeeSearch("[ " + DateTime.Now + " ] => [ " + SearchCriteria.LoginID + " account has been temporarily restricted Please confirm your email address ]");
                                    }

                                    if (!item.IsLoggedIn)
                                    {
                                        AddLoggerCompanyEmployeeSearch("[ " + DateTime.Now + " ] => [ Couldn't Login With : " + acc + " ]");
                                    }
                                   // InBoardPro.LinkedinSearch obj_LinkedinSearch = new InBoardPro.LinkedinSearch(item._Username, item._Password, item._ProxyAddress, item._ProxyPort, item._ProxyUsername, item._ProxyPassword);
                                    SearchCriteria.SignIN = true;
                                    SearchCriteria.SignOut = false;

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

            }
            catch
            {
            }

            return isLoggedin;
        }
        #endregion

        private void btnCompanyEmployeeScraperURLBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                txtCompanyEmployeeScraperURL.Text = "";
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "Text Files (*.txt)|*.txt";
                    ofd.InitialDirectory = Application.StartupPath;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtCompanyEmployeeScraperURL.Text = ofd.FileName;
                        List<string> templist = GlobusFileHelper.ReadFile(ofd.FileName);
                        //lstEventURLsFile = new List<string>();
                        LinkedinSearch.listCompanyURL.Clear();
                        foreach (string item in templist)
                        {
                            LinkedinSearch.listCompanyURL.Add(item);
                        }
                        LinkedinSearch.listCompanyURL = LinkedinSearch.listCompanyURL.Distinct().ToList();
                        AddLoggerCompanyEmployeeSearch("[ " + DateTime.Now + " ] => [ " + LinkedinSearch.listCompanyURL.Count + " Company URLs Loaded ! ]");
                        //AddToListWallPost(lstWallMessage.Count + " Wall post message ");
                    }
                }
            }
            catch
            {
            }
        }

        private void AddLoggerCompanyEmployeeSearch(string log)
        {
            this.Invoke(new MethodInvoker(delegate
            {
                lstLogCompanyEmployeeScraper.Items.Add(log);
                lstLogCompanyEmployeeScraper.SelectedIndex = lstLogCompanyEmployeeScraper.Items.Count - 1;
            }));
        }

        private void btnCompanyEmployeeStop_Click(object sender, EventArgs e)
        {
            Globals.IsStop_CompanyEmployeeScraperThread = true;

            List<Thread> lstTemp = Globals.lstCompanyEmployeeScraperThread.Distinct().ToList();
            CompanyEmployeeScraperStop();
        }

        private void CompanyEmployeeScraperStop()
        {
            List<Thread> lstTemp = Globals.lstCompanyEmployeeScraperThread.Distinct().ToList();
            foreach (Thread item in lstTemp)
            {
                try
                {
                    item.Abort();
                }
                catch
                { }
            }

            AddLoggerCompanyEmployeeSearch("[ " + DateTime.Now + " ] => [ Process Stopped ]");
        }

        private void CompanyEmployeeScraper_Paint(object sender, PaintEventArgs e)
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
    }
}
