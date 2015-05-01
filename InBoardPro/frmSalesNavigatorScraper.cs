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
using Campaign_GrpRequestAddConnections;
using Groups;
using InBoardProGetData;
using ProfileManager;
using Others;
using BaseLib;
using Campaign;

namespace InBoardPro
{
    public partial class frmSalesNavigatorScraper : Form
    {

        #region GlobalDeclaration
        public System.Drawing.Image image;
        GlobusHttpHelper objHttpHelper = new GlobusHttpHelper();
        string Language = "English;en,Spanish;es,German;de,French;fr,Italian;it,Portuguese;pt,Dutch;nl,Bahasa Indonesia;in,Malay;ms,Romanian;ro,Russian;ru,Turkish;tr,Swedish;sv,Polish;pl,Others;_o";
        string IndustryList = "Accounting;47,Airlines/Aviation;94,Alternative Dispute Resolution;120,Alternative Medicine;125,Animation;127,Apparel & Fashion;19,Architecture & Planning;50,Arts and Crafts;111,Automotive;53,Aviation & Aerospace;52,Banking;41,Biotechnology;12,Broadcast Media;36,Building Materials;49,Business Supplies and Equipment;138,Capital Markets;129,Chemicals;54,Civic & Social Organization;90,Civil Engineering;51,Commercial Real Estate;128,Computer & Network Security;118,Computer Games;109,Computer Hardware;3,Computer Networking;5,Computer Software;4,Construction;48,Consumer Electronics;24,Consumer Goods;25,Consumer Services;91,Cosmetics;18,Dairy;65,Defense & Space;1,Design;99,Education Management;69,E-Learning;132,Electrical/Electronic Manufacturing;112,Entertainment;28,Environmental Services;86,Events Services;110,Executive Office;76,Facilities Services;122,Farming;63,Financial Services;43,Fine Art;38,Fishery;66,Food & Beverages;34,Food Production;23,Fund-Raising;101,Furniture;26,Gambling & casinos;29,Glass, Ceramics & Concrete;145,Government Administration;75,Government Relations;148,Graphic Design;140,Health, Wellness and Fitness;124,Higher Education;68,Hospital & Health Care;14,Hospitality;31,Human Resources;137,Import and Export;134,Individual & Family Services;88,Industrial Automation;147,Information Services;84,Information Technology and Services;96,Insurance;42,International Affairs;74,International Trade and Development;141,Internet;6,Investment Banking;45,Investment Management;46,Judiciary;73,Law Enforcement;77,Law Practice;9,Legal Services;10,Legislative Office;72,Leisure, Travel & Tourism;30,Libraries;85,Logistics and Supply Chain;116,Luxury Goods & Jewelry;143,Machinery;55,Management Consulting;11,Maritime;95,Marketing and Advertising;80,Market Research;97,Mechanical or Industrial Engineering;135,Media Production;126,Medical Devices;17,Medical Practice;13,Mental Health Care;139,Military;71,Mining & Metals;56,Motion Pictures and Film;35,Museums and Institution;37,Music;115,Nanotechnology;114,Newspapers;81,Nonprofit Organization Management;100,Oil & Energy;57,Online Media;113,Outsourcing/Offshoring;123,Package/Freight Delivery;87,Packaging and Containers;146,Paper & Forest Products;61,Performing Arts;39,Pharmaceuticals;15,Philanthropy;131,Photography;136,Plastics;117,Political Organization;107,Primary/Secondary Education;67,Printing;83,Professional Training & Coaching;105,Program Development;102,Public Policy;79,Public Relations and Communications;98,Public Safety;78,Publishing;82,Railroad Manufacture;62,Ranching;64,Real Estate;44,Recreational Facilities and Services;40,Religious Institutions;89,Renewables & Environment;144,Research;70,Restaurants;32,Retail;27,Security and Investigations;121,Semiconductors;7,Shipbuilding;58,Sporting Goods;20,Sports;33,Staffing and Recruiting;104,Supermarkets;22,Telecommunications;8,Textiles;60,Think Tanks;130,Tobacco;21,Translation and Localization;108,Transportation/Trucking/Railroad;92,Utilities;59,Venture Capital & Private Equity;106,Veterinary;16,Warehousing;93,Wholesale;133,Wine and Spirits;142,Wireless;119,Writing and Editing;103";
        string RelationshipList = "N:All LinkedIn Members,F:1st Connections,S:2nd Connections,A:Group Members,O:3rd + Everyone Else";
        string Functionlist = "all:All Functions,1:Accounting,2:Administrative,3:Arts and Design,4:Business development,5:Community and Social Services,6:Consulting,7:Education,8:Engineering,9:Entrepreneurship,10:Finance,11:Healthcare Services,12:Human Resources,13:Information Technology,14:Legal,15:Marketing,16:Media and Communication,17:Military and Protective Services,18:Operations,19:Product Management,20:Program and Project Management,21:Purchasing,22:Quality Assurance,23:Real Estate,24:Research,25:Sales,26:Support";
        string companysizelist = "all:All Company Sizes,1:1-10,2:11-50,3:51-200,4:201-500,5:501-1000,6:1001-5000,7:5001-10000,8:10000+";
        string senioritylevel = "all:All Seniority Levels,1:Unpaid,2:Training,3:Entry,4:Senior,5:Manager,6:Director,7:VP,8:CXO,9:Partner,10:Owner";
        string WithinList = "10:10 mi (15km),25:25 mi (40 km),35:35 mi (55 km),50:50 mi (80 km),75:75 mi (120 km),100:100 mi (160 km)";
        string TitleValue = "CURRENT_OR_PAST:Current or past,CURRENT:Current,PAST:Past,PAST_NOT_CURRENT:Past not current";
        string expList = "1:Less than 1 year,2:1 to 2 years,3:3 to 5 years,4:6 to 10 years,5:More than 10 years";
        string RecentlyjoinedList = "select-all:Any Time,1:1 day ago,2:2-7 days ago,3:8-14 days ago,4:15-30 days ago,5:1-3 months ago";

        List<Thread> lstSalesNavigatorScraperThread = new List<Thread>();
        
        Dictionary<string, string> CountryCode = new Dictionary<string, string>();
        Dictionary<string, string> IndustryCode = new Dictionary<string, string>();

        bool CheckNetConn = false;
        bool stopScraping = false;

        //static string currentCompany = string.Empty;
        //static string currentTitle = string.Empty;
        //static string keyword = string.Empty;
        //static string location = string.Empty;
        //static string postalCode = string.Empty;
        //static string country = string.Empty;
        //static string relationship = string.Empty;
        //static string within = string.Empty;
        //static string function = string.Empty;
        //static string companySize = string.Empty;
        //static string seniorityLevel = string.Empty;
        //static string industry = string.Empty;

        //static bool signIn = false;
        //static bool signOut = true;

        #endregion


        public frmSalesNavigatorScraper()
        {
            InitializeComponent();
        }

        private void frmSalesNavigatorScraper_Load(object sender, EventArgs e)
        {
            try
            {
                image = Properties.Resources.background;
                LinkedInScrape.logger.addToLogger += new EventHandler(LinkedinScrapeSalesNavigatorLogEvents_addToLogger);
                SalesNavigatorScraper.loggerSalesNavigator.addToLogger += new EventHandler(LinkedinScrapeSalesNavigatorLogEvents_addToLogger);
                
                
                
                LoadControlItems();
                
                LoadAccount();
                
            }
            catch (Exception ex)
            {
            }
        }

        public void LoadControlItems()
        {
            try
            {
                ClsSelect ObjSelectMethod = new ClsSelect();
                CountryCode = ObjSelectMethod.getCountry();
                IndustryCode = ObjSelectMethod.getIndustry();

                #region LoadCountryItems
                foreach (KeyValuePair<string, string> pair in CountryCode)
                {
                    try
                    {
                        cmbScraperCountry.Items.Add(pair.Value);
                    }
                    catch
                    {
                    }
                }
                try
                {
                    cmbScraperCountry.SelectedIndex = 0;
                }
                catch (Exception ex)
                {
                }
                #endregion

                #region LoadWithinItems
                try
                {
                    string[] arraywithin = Regex.Split(WithinList, ",");
                    foreach (string item in arraywithin)
                    {
                        string[] arrayPostalwithin = Regex.Split(item, ":");
                        if (arrayPostalwithin.Length == 2)
                        {
                            cmbScraperWithin.Items.Add(arrayPostalwithin[1]);
                        }
                    }
                }
                catch (Exception ex)
                {
                }
                #endregion

                #region LoadIndustryList
                try
                {
                    string[] arrayIndustry = Regex.Split(IndustryList, ",");
                    foreach (string item in arrayIndustry)
                    {
                        string[] arrayInds = Regex.Split(item, ";");
                        if (arrayInds.Length == 2)
                        {
                            chkListScraperIndustry.Items.Add(arrayInds[0]);
                        }
                    }
                }
                catch (Exception ex)
                {
                }
                for (int i = 0; i < chkListScraperIndustry.Items.Count; i++)
                {
                    chkListScraperIndustry.SetItemChecked(i, false);
                }
                #endregion

                #region LoadFunctionList
                try
                {
                    string[] arrayFunction = Regex.Split(Functionlist, ",");
                    foreach (string item in arrayFunction)
                    {
                        string[] arrayInds = Regex.Split(item, ":");
                        if (arrayInds.Length == 2)
                        {
                            chkListScraperFunction.Items.Add(arrayInds[1]);
                        }
                    }

                    for (int i = 0; i < chkListScraperFunction.Items.Count; i++)
                    {
                        chkListScraperFunction.SetItemChecked(i, false);
                    }
                }
                catch (Exception ex)
                {

                    throw;
                }
                #endregion

                #region LoadCompanySizeList
                try
                {
                    string[] arrayCompanysize = Regex.Split(companysizelist, ",");
                    foreach (string item in arrayCompanysize)
                    {
                        string[] arrayInds = Regex.Split(item, ":");
                        if (arrayInds.Length == 2)
                        {
                            chkListScraperCompanySize.Items.Add(arrayInds[1]);
                        }
                    }

                    for (int i = 0; i < chkListScraperCompanySize.Items.Count; i++)
                    {
                        chkListScraperCompanySize.SetItemChecked(i, false);
                    }
                }
                catch (Exception ex)
                {
                }
                #endregion

                #region LoadSeniorityLevelList
                try
                {
                    string[] arraySenoirtyLevel = Regex.Split(senioritylevel, ",");
                    foreach (string item in arraySenoirtyLevel)
                    {
                        string[] arrayInds = Regex.Split(item, ":");
                        if (arrayInds.Length == 2)
                        {
                            chkListScraperSeniorityLevel.Items.Add(arrayInds[1]);
                        }
                    }

                    for (int i = 0; i < chkListScraperSeniorityLevel.Items.Count; i++)
                    {
                        chkListScraperSeniorityLevel.SetItemChecked(i, false);
                    }
                }
                catch (Exception ex)
                {

                    throw;
                }
                #endregion

                #region LoadRelationshipList
                try
                {
                    string[] arrayRelationship = Regex.Split(RelationshipList, ",");
                    foreach (string item in arrayRelationship)
                    {
                        string[] arrayrelat = Regex.Split(item, ":");
                        if (arrayrelat.Length == 2)
                        {
                            chkListRelationship.Items.Add(arrayrelat[1]);
                        }
                    }

                    for (int i = 0; i < chkListScraperSeniorityLevel.Items.Count; i++)
                    {
                        chkListScraperSeniorityLevel.SetItemChecked(i, false);
                    }
                }
                catch (Exception ex)
                {
                }
                #endregion

                #region LoadCompanyAndTitleValue
                try
                {
                    string[] arrayTitleValue = Regex.Split(TitleValue, ",");
                    foreach (string item in arrayTitleValue)
                    {
                        string[] arraytitleValue = Regex.Split(item, ":");
                        if (arraytitleValue.Length == 2)
                        {
                            cmbScraperTitle.Items.Add(arraytitleValue[1]);
                            cmbScraperCompany.Items.Add(arraytitleValue[1]);
                        }
                    }

                    cmbScraperCompany.SelectedIndex = 0;
                    cmbScraperTitle.SelectedIndex = 0;
                }
                catch (Exception ex)
                {
                }
                #endregion

                #region Load ProfileLanguages
                try
                {
                    string[] arrayLanguage = Regex.Split(Language, ",");
                    foreach (string item in arrayLanguage)
                    {
                        string[] arraylang = Regex.Split(item, ";");
                        if (arraylang.Length == 2)
                        {
                            chkListScraperProfileLanguage.Items.Add(arraylang[0]);
                        }
                    }                    
                }                    
                catch (Exception ex)
                {
                }
                #endregion
                
                #region Load YearsOfExperience
                try
                {
                    string[] arrayexpList = Regex.Split(expList, ",");
                    foreach (string item in arrayexpList)
                    {
                        string[] arrayInds = Regex.Split(item, ":");
                        if (arrayInds.Length == 2)
                        {
                            chkListScraperYearsOfExperience.Items.Add(arrayInds[1]);
                        }
                    }
                }
                catch (Exception ex)
                {
                }
                #endregion  

                #region Load When Joined
                try
                {
                    string[] arrayRecentlyjoined = Regex.Split(RecentlyjoinedList, ",");
                    foreach (string item in arrayRecentlyjoined)
                    {
                        string[] arrayInds = Regex.Split(item, ":");
                        if (arrayInds.Length == 2)
                        {
                            chkListScraperWhenJoined.Items.Add(arrayInds[1]);
                        }
                    }
                }
                catch (Exception ex)
                {
                }
                #endregion


            }
            catch (Exception ex)
            {
            }
        }

        public void LoadAccount()
        {
            try
            {
                if (LinkedInManager.linkedInDictionary.Count() == 0)
                {
                    MessageBox.Show("Please Load LinkedIn Accounts From MyAccounts Menu");
                    AddLoggerSalesNavigatorScraping("[ " + DateTime.Now + " ] => [ Please Load LinkedIn Accounts From MyAccounts Menu ]");                    
                    frmAccounts FrmAccount = new frmAccounts();
                    FrmAccount.Show();
                    return;
                }
                else
                {
                    try
                    {
                        PopulateCmo();

                        cmbSelectAccountSalesNavigator.SelectedIndex = 0;
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

        #region PopulateCmo()
        private void PopulateCmo()
        {
            try
            {
                cmbSelectAccountSalesNavigator.Items.Clear();
                foreach (KeyValuePair<string, LinkedInMaster> item in LinkedInManager.linkedInDictionary)
                {
                    cmbSelectAccountSalesNavigator.Items.Add(item.Key);
                }
                AddLoggerSalesNavigatorScraping("[ " + DateTime.Now + " ] => [ Accounts Uploaded.. ]");
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> PopulateCmo() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> PopulateCmo() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPreScrapperErrorLogs);
            }
        }

        #endregion

        #region LinkedinScrapeSalesNavigatorLogEvents_addToLogger
        void LinkedinScrapeSalesNavigatorLogEvents_addToLogger(object sender, EventArgs e)
        {
            try
            {
                if (e is EventsArgs)
                {
                    EventsArgs eventArgs = e as EventsArgs;
                    AddLoggerSalesNavigatorScraping(eventArgs.log);
                }
            }
            catch
            {
            }
        }
        #endregion

        private void AddLoggerSalesNavigatorScraping(string log)
        {
            
            this.Invoke(new MethodInvoker(delegate
            {
                listLoggerSalesNavigatorScraper.Items.Add(log);
                listLoggerSalesNavigatorScraper.SelectedIndex = listLoggerSalesNavigatorScraper.Items.Count - 1;
            }));
        }

        private void frmSalesNavigatorScraper_Paint(object sender, PaintEventArgs e)
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

        private void btnSearchScrapeSalesNavigator_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
            if (CheckNetConn)
            {
                if (cmbSelectAccountSalesNavigator.Items.Count <= 0)
                {
                    MessageBox.Show("Load The account From Menu and switch the tab and again come to InBoardProGetData Tab");
                    AddLoggerSalesNavigatorScraping("[ " + DateTime.Now + " ] => [ Load The account From Menu and switch the tab and again come to InBoardProGetData Tab ]");
                    frmAccounts FrmAccount = new frmAccounts();
                    FrmAccount.Show();
                    return;
                }
                else if (cmbSelectAccountSalesNavigator.SelectedIndex < 0)
                {
                    MessageBox.Show("Select The Email Id From The Dropdown and Fill All Mandatory (*) Fields");
                    AddLoggerSalesNavigatorScraping("[ " + DateTime.Now + " ] => [ Select The Email Id From The Dropdown and Fill All Mandatory (*) Fields ]");
                    return;
                }
                
                AddLoggerSalesNavigatorScraping("[ " + DateTime.Now + " ] => [ Starting Scraping ]");
                stopScraping = false;
                lstSalesNavigatorScraperThread.Clear();

                #region CountrySelected
                foreach (KeyValuePair<string, string> item in CountryCode)
                {
                    try
                    {
                        if (item.Value == cmbScraperCountry.SelectedItem.ToString())
                        {
                            SalesNavigatorGlobals.country = item.Key;
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
                
                #region Language List
                string language = string.Empty;
                string[] arrLang = Regex.Split(Language, ",");
                foreach (string LanguageL in chkListScraperProfileLanguage.CheckedItems)
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
                SalesNavigatorGlobals.language = language;
                #endregion

                #region Relationship List
                string Relationship = string.Empty;
                string[] arrRelation = Regex.Split(RelationshipList, ",");
                foreach (string RelationL in chkListRelationship.CheckedItems)
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
                    SalesNavigatorGlobals.relationship = string.Empty;
                }
                else
                {
                    SalesNavigatorGlobals.relationship = Relationship;
                }
                #endregion

                #region Industry List
                string Industryvalue = string.Empty;
                string[] arrIndustry = Regex.Split(IndustryList, ",");

                foreach (string Industry in chkListScraperIndustry.CheckedItems)
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

                SalesNavigatorGlobals.industry = Industryvalue;
                #endregion

                #region Company size List
                string companysizevalue = string.Empty;
                string[] arraycompnaysize = Regex.Split(companysizelist, ",");
                foreach (string companysize in chkListScraperCompanySize.CheckedItems)
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
                    SalesNavigatorGlobals.companySize = string.Empty;
                }
                else
                {
                    SalesNavigatorGlobals.companySize = companysizevalue;
                }
                #endregion
                
                #region Senority List
                string Seniorlevelvalue = string.Empty;
                string[] senoirLevelList = Regex.Split(senioritylevel, ",");
                foreach (string Seniorlevel in chkListScraperSeniorityLevel.CheckedItems)
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
                    SalesNavigatorGlobals.seniorityLevel = string.Empty;
                }
                else
                {
                    SalesNavigatorGlobals.seniorityLevel = Seniorlevelvalue;
                }
                #endregion
                
                #region Function List
                string Function = string.Empty;
                string[] FunctionList = Regex.Split(Functionlist, ",");
                foreach (string FunctionL in chkListScraperFunction.CheckedItems)
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
                    SalesNavigatorGlobals.function = string.Empty;
                }
                else
                {
                    SalesNavigatorGlobals.function = Function;
                }
                #endregion
                
                #region Within List
                try
                {
                    string[] arraywithinList = Regex.Split(WithinList, ",");
                    foreach (string item in arraywithinList)
                    {
                        if (item.Contains(cmbScraperWithin.SelectedItem.ToString()))
                        {
                            string[] arrayWithin = Regex.Split(item, ":");
                            SalesNavigatorGlobals.within = arrayWithin[0];
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
                        if (arrayTitleValue[1] == cmbScraperTitle.SelectedItem.ToString())
                        {
                            SalesNavigatorGlobals.titleValue = arrayTitleValue[0];
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
                    if (cmbScraperCompany.Enabled == true)
                    {
                        string[] arrayTitleList = Regex.Split(TitleValue, ",");
                        foreach (string item in arrayTitleList)
                        {
                            string[] arrayTitleValue = Regex.Split(item, ":");

                            if (arrayTitleValue[1] == cmbScraperCompany.SelectedItem.ToString())
                            {
                                SalesNavigatorGlobals.companyValue = arrayTitleValue[0];
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                }
                #endregion
                
                #region Experience list
                string TotalExperience = string.Empty;
                string[] arratExpericne = Regex.Split(expList, ",");
                foreach (string YearOfExperienceL in chkListScraperYearsOfExperience.CheckedItems)
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
                    SalesNavigatorGlobals.yearOfExperience = string.Empty;
                }
                else
                {
                    SalesNavigatorGlobals.yearOfExperience = TotalExperience;
                }
                #endregion

                #region When Joined
                try
                {
                     
                    string RecentlyJoined = string.Empty;
                    string[] RecetlyJoinedArr = Regex.Split(RecentlyjoinedList, ",");
                    foreach (string RecentlyJoinedL in chkListScraperWhenJoined.CheckedItems)
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
                        SalesNavigatorGlobals.whenJoined = string.Empty;
                    }
                    else
                    {
                        SalesNavigatorGlobals.whenJoined = RecentlyJoined.Trim();
                    }
                   
                }
                catch (Exception ex)
                {
                }
                #endregion





                SalesNavigatorGlobals.keyword = txtScraperKeywords.Text;

                SalesNavigatorGlobals.currentCompany = txtScraperCurrentCompany.Text;

                SalesNavigatorGlobals.currentTitle = txtScraperCurrentTitle.Text;

                SalesNavigatorGlobals.location = txtScraperLocation.Text;

                SalesNavigatorGlobals.postalCode = txtScarperPostalCode.Text;

                SalesNavigatorGlobals.firstName = txtScraperFirstName.Text;

                SalesNavigatorGlobals.lastName = txtScraperLastName.Text;


                if (SalesNavigatorGlobals.keyword.Contains("or") || (SalesNavigatorGlobals.keyword.Contains("and")) || (SalesNavigatorGlobals.keyword.Contains("not")))
                {
                    try
                    {
                        SalesNavigatorGlobals.keyword = SalesNavigatorGlobals.keyword.Replace(" or ", " OR ");
                        SalesNavigatorGlobals.keyword = SalesNavigatorGlobals.keyword.Replace(" and ", " AND ");
                        SalesNavigatorGlobals.keyword = SalesNavigatorGlobals.keyword.Replace(" not ", " NOT ");
                    }
                    catch { }
                }

                #region CountrySelectedTrial
                if (cmbScraperCountry.SelectedItem.ToString() != null && cmbScraperCountry.SelectedIndex > 0)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(cmbScraperCountry.SelectedItem.ToString()))
                        {
                            string countryValue = string.Empty;
                            string temCOuntry = cmbScraperCountry.SelectedItem.ToString();
                            GetCountryNameValue(ref temCOuntry, ref countryValue);
                            SalesNavigatorGlobals.country = countryValue;
                        }
                    }
                    catch (Exception ex)
                    {
                        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnSearchScraper_Click() ---13--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnSearchScraper_Click() ---13--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPreScrapperErrorLogs);
                    }
                }
                #endregion

                btnSearchScrapeSalesNavigator.Cursor = Cursors.AppStarting;

                
                string url = FormURL();

                new Thread(() =>
                {
                    StartSalesNavigatorScraping(url);

                }).Start();


            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled or not working, Please Check Your Internet Connection...");
                AddLoggerSalesNavigatorScraping("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        }
        
        public void StartSalesNavigatorScraping(string mainUrl)
        {
            try
            {
                if (stopScraping)
                {
                    return;
                }

                lstSalesNavigatorScraperThread.Add(Thread.CurrentThread);
                lstSalesNavigatorScraperThread = lstSalesNavigatorScraperThread.Distinct().ToList();
                Thread.CurrentThread.IsBackground = true;

                SalesNavigatorScraper objSalesNavigatorScraper = new SalesNavigatorScraper();

                bool isLoggedIn = Login_SalesNavigatorScraper();
                //string response = SearchCriteria.loginREsponce;

                if (isLoggedIn)
                {
                    AddLoggerSalesNavigatorScraping("[ " + DateTime.Now + " ] => [ Start LinkedIn Crawling ]");
                    objSalesNavigatorScraper.StartSalesavigatorScraper(ref objHttpHelper, mainUrl);
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                AddLoggerSalesNavigatorScraping("-------------------------------------------------------------------------------------------------------------------------------");
                AddLoggerSalesNavigatorScraping("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                AddLoggerSalesNavigatorScraping("-------------------------------------------------------------------------------------------------------------------------------");
            }
        }

        public string  FormURL()
        {
            string url = "https://www.linkedin.com/sales/search/?";
            try
            {                
                if (!string.IsNullOrEmpty(txtScraperCurrentCompany.Text))
                {
                    if (!string.IsNullOrEmpty(cmbScraperCompany.Text))
                    {
                        url = url + "&companyScope=" + SalesNavigatorGlobals.companyValue;
                    }
                    url = url + "&company=" + SalesNavigatorGlobals.currentCompany;
                }
                if (!string.IsNullOrEmpty(SalesNavigatorGlobals.relationship))
                {
                    int i=0;
                    string[] rawRelationship = Regex.Split(SalesNavigatorGlobals.relationship, ",");
                    string addRelationshipValue = "&facet=N";
                    for (i = 0; i < rawRelationship.Count(); i++)
                    {
                        addRelationshipValue = addRelationshipValue + "&facet.N=" + rawRelationship[i]; 
                    }
                    url = url + addRelationshipValue;
                }
                if (!string.IsNullOrEmpty(txtScraperLocation.Text))
                {
                    string locationResponse = objHttpHelper.getHtmlfromUrl(new Uri("https://www.linkedin.com/ta/region?query=" + Uri.EscapeDataString(SalesNavigatorGlobals.location)));
                    string rawLocationValue = Utils.getBetween(locationResponse, "\"id\":\"", "\",\"");
                    url = url + "&facet=G&facet.G=" + rawLocationValue;
                }                
                if (!string.IsNullOrEmpty(txtScraperCurrentTitle.Text))
                {
                    if (!string.IsNullOrEmpty(cmbScraperTitle.Text))
                    {
                        url = url + "&titleScope=" + SalesNavigatorGlobals.titleValue;
                    }
                    url = url + "&jobTitle=" + SalesNavigatorGlobals.currentTitle;
                }
                if (!string.IsNullOrEmpty(SalesNavigatorGlobals.industry))
                {
                    int i = 0;
                    string[] rawIndustry = Regex.Split(SalesNavigatorGlobals.industry, ",");
                    string addIndustryValue = "&facet=I";
                    for (i = 0; i < rawIndustry.Count(); i++)
                    {
                        addIndustryValue = addIndustryValue + "&facet.I=" + rawIndustry[i];
                    }
                    url = url + addIndustryValue;
                }
                if (!string.IsNullOrEmpty(cmbScraperCountry.Text))
                {
                    url = url + "&countryCode=" + SalesNavigatorGlobals.country;
                }
                if (!string.IsNullOrEmpty(cmbScraperWithin.Text))
                {
                    url = url + "&radiusMiles=" + SalesNavigatorGlobals.within;
                }
                if (!string.IsNullOrEmpty(txtScarperPostalCode.Text))
                {
                    url = url + "&postalCode=" + SalesNavigatorGlobals.postalCode;
                }
                if (!string.IsNullOrEmpty(txtScraperFirstName.Text))
                {
                    url = url + "&firstName=" + SalesNavigatorGlobals.firstName;
                }
                if (!string.IsNullOrEmpty(txtScraperLastName.Text))
                {
                    url = url + "&lastName=" + SalesNavigatorGlobals.lastName;
                }
                if (!string.IsNullOrEmpty(SalesNavigatorGlobals.seniorityLevel))
                {
                    int i = 0;
                    string[] rawSeniorityLevel = Regex.Split(SalesNavigatorGlobals.seniorityLevel, ",");
                    string addSeniorityValue = "&facet=SE";
                    for (i = 0; i < rawSeniorityLevel.Count(); i++)
                    {
                        addSeniorityValue = addSeniorityValue + "&facet.SE=" + rawSeniorityLevel[i];
                    }
                    url = url + addSeniorityValue;
                }
                if (!string.IsNullOrEmpty(SalesNavigatorGlobals.function))
                {
                    int i = 0;
                    string[] rawFunction = Regex.Split(SalesNavigatorGlobals.function, ",");
                    string addFunctionValue = "&facet=FA";
                    for (i = 0; i < rawFunction.Count(); i++)
                    {
                        addFunctionValue = addFunctionValue + "&facet.FA=" + rawFunction[i];
                    }
                    url = url + addFunctionValue;
                }
                if (!string.IsNullOrEmpty(SalesNavigatorGlobals.companySize))
                {
                    int i = 0;
                    string[] rawCompanySize = Regex.Split(SalesNavigatorGlobals.companySize, ",");
                    string addCompanySizeValue = "&facet=CS";
                    for (i = 0; i < rawCompanySize.Count(); i++)
                    {
                        addCompanySizeValue = addCompanySizeValue + "&facet.CS=" + rawCompanySize[i];
                    }
                    url = url + addCompanySizeValue;
                }
                if (!string.IsNullOrEmpty(SalesNavigatorGlobals.yearOfExperience))
                {
                    int i = 0;
                    string[] rawYearsOfExperience = Regex.Split(SalesNavigatorGlobals.yearOfExperience, ",");
                    string addYearsOfExperienceValue = "&facet=TE";
                    for (i = 0; i < rawYearsOfExperience.Count(); i++)
                    {
                        addYearsOfExperienceValue = addYearsOfExperienceValue + "&facet.TE=" + rawYearsOfExperience[i];
                    }
                    url = url + addYearsOfExperienceValue;
                }
                if (!string.IsNullOrEmpty(SalesNavigatorGlobals.language))
                {
                    int i = 0;
                    string[] rawlanguage = Regex.Split(SalesNavigatorGlobals.language, ",");
                    string addLanguageValue = "&facet=L";
                    for (i = 0; i < rawlanguage.Count(); i++)
                    {
                        addLanguageValue = addLanguageValue + "&facet.L=" + rawlanguage[i];
                    }
                    url = url + addLanguageValue;
                }
                if (!string.IsNullOrEmpty(SalesNavigatorGlobals.whenJoined))
                {
                    int i = 0;
                    string[] rawWhenJoined = Regex.Split(SalesNavigatorGlobals.whenJoined, ",");
                    string addWhenJoinedValue = "&facet=DR";
                    for (i = 0; i < rawWhenJoined.Count(); i++)
                    {
                        addWhenJoinedValue = addWhenJoinedValue + "&facet.DR=" + rawWhenJoined[i];
                    }
                    url = url + addWhenJoinedValue;
                }
                url = url + "&defaultSelection=false&start=replaceVariableCounter&count=100";
                if (!string.IsNullOrEmpty(txtScraperKeywords.Text))
                {
                    url = url + "&keywords=" + Uri.EscapeDataString(SalesNavigatorGlobals.keyword);
                }
            }
            catch (Exception ex)
            {
            }
            return url;
        }

        private bool Login_SalesNavigatorScraper()
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
                        AddLoggerSalesNavigatorScraping("[ " + DateTime.Now + " ] => [ Process Running Please wait for sometimes... ]");
                    }));

                    if (SalesNavigatorGlobals.signIn)
                    {
                        LinkedinLogin Login = new LinkedinLogin();
                        //For Sign Out 
                        Login.LogoutHttpHelper();
                        SalesNavigatorGlobals.signOut = true;
                    }
                    
                    if (SalesNavigatorGlobals.signOut)
                    {
                        SearchCriteria.LoginID = string.Empty;
                        if (LinkedInManager.linkedInDictionary.Count() > 0)
                        {
                            try
                            {
                                object temp = null;
                                cmbSelectAccountSalesNavigator.Invoke(new MethodInvoker(delegate
                                {
                                    temp = cmbSelectAccountSalesNavigator.SelectedItem;
                                }));

                                if (temp != null)
                                {
                                    //GlobusHttpHelper httpHelper = new GlobusHttpHelper();
                                    string acc = "";
                                    cmbSelectAccountSalesNavigator.Invoke(new MethodInvoker(delegate
                                    {
                                        acc = cmbSelectAccountSalesNavigator.SelectedItem.ToString();
                                        SalesNavigatorGlobals.loginId = cmbSelectAccountSalesNavigator.SelectedItem.ToString();//change 21/08/12
                                    }));
                                    //string acc = account.Remove(account.IndexOf(':'));

                                    //Run a separate thread for each account
                                    LinkedInMaster item = null;
                                    LinkedInManager.linkedInDictionary.TryGetValue(acc, out item);

                                    //item.logger.addToLogger += ScrapeEvent_addToLogger;
                                    item.LoginHttpHelper(ref objHttpHelper);

                                    if (item.IsLoggedIn)
                                    {
                                        AddLoggerSalesNavigatorScraping("[ " + DateTime.Now + " ] => [ Logged in With Account : " + acc + " ]");
                                        isLoggedin = true;
                                    }

                                    if (SearchCriteria.loginREsponce.Contains("Your LinkedIn account has been temporarily restricted"))
                                    {
                                        AddLoggerSalesNavigatorScraping("[ " + DateTime.Now + " ] => [ " + SalesNavigatorGlobals.loginId + "Your LinkedIn account has been temporarily restricted ]");
                                    }

                                    if (SearchCriteria.loginREsponce.Contains("Please confirm your email address"))
                                    {
                                        AddLoggerSalesNavigatorScraping("[ " + DateTime.Now + " ] => [ " + SalesNavigatorGlobals.loginId + " account has been temporarily restricted Please confirm your email address ]");
                                    }

                                    if (!item.IsLoggedIn)
                                    {
                                        AddLoggerSalesNavigatorScraping("[ " + DateTime.Now + " ] => [ Couldn't Login With : " + acc + " ]");
                                    }

                                    SalesNavigatorGlobals.signIn = true;
                                    SalesNavigatorGlobals.signOut = false;
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


        private void txtScarperPostalCode_TextChanged(object sender, EventArgs e)
        {
            //if (txtScarperPostalCode.Text != string.Empty)
            //{
            //    cmbScraperWithin.Enabled = true;
            //}
            //else
            //{
            //    cmbScraperWithin.Enabled = false;
            //}
        }

        private void txtScraperCurrentCompany_TextChanged(object sender, EventArgs e)
        {
            if (txtScraperCurrentCompany.Text != string.Empty)
            {
                cmbScraperCompany.Enabled = true;
            }
            else
            {
                cmbScraperCompany.Enabled = false;
            }
        }

        private void txtScrperCurrentTitle_TextChanged(object sender, EventArgs e)
        {
            if (txtScraperCurrentTitle.Text != string.Empty)
            {
                cmbScraperTitle.Enabled = true;
            }
            else
            {
                cmbScraperTitle.Enabled = false;
            }
        }

        private void btnStopSalesNavigator_Click(object sender, EventArgs e)
        {
            try
            {
                stopScraping = true;
                SalesNavigatorGlobals.isStop = true;

                List<Thread> lstTemp = lstSalesNavigatorScraperThread.Distinct().ToList();
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

                AddLoggerSalesNavigatorScraping("-------------------------------------------------------------------------------------------------------------------------------");
                AddLoggerSalesNavigatorScraping("[ " + DateTime.Now + " ] => [ PROCESS STOPPED ]");
                AddLoggerSalesNavigatorScraping("-------------------------------------------------------------------------------------------------------------------------------");
            }
            catch (Exception ex)
            {
            }
        }

    }
}
