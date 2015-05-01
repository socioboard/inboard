using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using InBoardPro;
using System.Text.RegularExpressions;
using System.Threading;
using InBoardProGetData;
using BaseLib;

namespace InBoardPro
{
    public partial class FrmSearchWithSalesNavigator : Form
    {
        GlobusHttpHelper HttpHelper = new GlobusHttpHelper();
        public System.Drawing.Image image;
        Dictionary<string, string> CountryCode = new Dictionary<string, string>();
        bool IsStop_InBoardProGetDataThread = false;
        bool CheckNetConn = false;
        List<Thread> lstInBoardProGetDataThraed = new List<Thread>();
        string _LinkedinSearchSelectedEmailId = string.Empty;
        string _LinkedinSearchSelectedcmbLinkedinSearch = string.Empty;
        string _LinkedinSearchtxtSearchItem = string.Empty;
        bool AllRelationship_bool = true;
        bool AllIndustry_bool = true;
        bool AllCompnaySize_bool = true;
        bool AllFollower_bool = true;
        bool AllFortune_bool = true;

        string JobOportunity = "JO1:Hiring on LinkedIn";
        string RelationshipList = "F:1st Connections,S:2nd Connections,O:3rd + Everyone Else"; //N:All LinkedIn Members
        string IndustryList = "Accounting;47,Airlines/Aviation;94,Alternative Dispute Resolution;120,Alternative Medicine;125,Animation;127,Apparel & Fashion;19,Architecture & Planning;50,Arts and Crafts;111,Automotive;53,Aviation & Aerospace;52,Banking;41,Biotechnology;12,Broadcast Media;36,Building Materials;49,Business Supplies and Equipment;138,Capital Markets;129,Chemicals;54,Civic & Social Organization;90,Civil Engineering;51,Commercial Real Estate;128,Computer & Network Security;118,Computer Games;109,Computer Hardware;3,Computer Networking;5,Computer Software;4,Construction;48,Consumer Electronics;24,Consumer Goods;25,Consumer Services;91,Cosmetics;18,Dairy;65,Defense & Space;1,Design;99,Education Management;69,E-Learning;132,Electrical/Electronic Manufacturing;112,Entertainment;28,Environmental Services;86,Events Services;110,Executive Office;76,Facilities Services;122,Farming;63,Financial Services;43,Fine Art;38,Fishery;66,Food & Beverages;34,Food Production;23,Fund-Raising;101,Furniture;26,Gambling & casinos;29,Glass, Ceramics & Concrete;145,Government Administration;75,Government Relations;148,Graphic Design;140,Health, Wellness and Fitness;124,Higher Education;68,Hospital & Health Care;14,Hospitality;31,Human Resources;137,Import and Export;134,Individual & Family Services;88,Industrial Automation;147,Information Services;84,Information Technology and Services;96,Insurance;42,International Affairs;74,International Trade and Development;141,Internet;6,Investment Banking;45,Investment Management;46,Judiciary;73,Law Enforcement;77,Law Practice;9,Legal Services;10,Legislative Office;72,Leisure, Travel & Tourism;30,Libraries;85,Logistics and Supply Chain;116,Luxury Goods & Jewelry;143,Machinery;55,Management Consulting;11,Maritime;95,Marketing and Advertising;80,Market Research;97,Mechanical or Industrial Engineering;135,Media Production;126,Medical Devices;17,Medical Practice;13,Mental Health Care;139,Military;71,Mining & Metals;56,Motion Pictures and Film;35,Museums and Institution;37,Music;115,Nanotechnology;114,Newspapers;81,Nonprofit Organization Management;100,Oil & Energy;57,Online Media;113,Outsourcing/Offshoring;123,Package/Freight Delivery;87,Packaging and Containers;146,Paper & Forest Products;61,Performing Arts;39,Pharmaceuticals;15,Philanthropy;131,Photography;136,Plastics;117,Political Organization;107,Primary/Secondary Education;67,Printing;83,Professional Training & Coaching;105,Program Development;102,Public Policy;79,Public Relations and Communications;98,Public Safety;78,Publishing;82,Railroad Manufacture;62,Ranching;64,Real Estate;44,Recreational Facilities and Services;40,Religious Institutions;89,Renewables & Environment;144,Research;70,Restaurants;32,Retail;27,Security and Investigations;121,Semiconductors;7,Shipbuilding;58,Sporting Goods;20,Sports;33,Staffing and Recruiting;104,Supermarkets;22,Telecommunications;8,Textiles;60,Think Tanks;130,Tobacco;21,Translation and Localization;108,Transportation/Trucking/Railroad;92,Utilities;59,Venture Capital & Private Equity;106,Veterinary;16,Warehousing;93,Wholesale;133,Wine and Spirits;142,Wireless;119,Writing and Editing;103";//All Industry;Y
        string companysizelist = "B:1-10,C:11-50,D:51-200,E:201-500,F:501-1000,G:1001-5000,H:5001-10000,I:10000+";//Y:All Company Sizes
        string Followerlist = "NFR5:5001+,NFR4:1001-5000,NFR3:101-1000,NFR2:51-100,NFR1:1-50"; //Y:All Companies
        string FortuneList = "1:Fortune 50,2:Fortune 51-100,3:Fortune 101-250,4:Fortune 251-500,5:Fortune 501-1000"; //Y:All Companies

        public FrmSearchWithSalesNavigator()
        {
            InitializeComponent();
        }


        #region LinkedinSearchLogEvents_addToLogger

        void LinkedinSearchLogEvents_addToLogger(object sender, EventArgs e)
        {
            try
            {
                if (e is EventsArgs)
                {
                    EventsArgs eventArgs = e as EventsArgs;
                    AddLoggerScrapeUsers(eventArgs.log);
                }
            }
            catch
            {
            }
        }

        #endregion

        #region LinkedinSearchFilterLogEvents_addToLogger

        void LinkedinSearchFilterLogEvents_addToLogger(object sender, EventArgs e)
        {
            try
            {
                if (e is EventsArgs)
                {
                    EventsArgs eventArgs = e as EventsArgs;
                    AddLoggerFilterScrapeUsers(eventArgs.log);
                }
            }
            catch
            {
            }
        }

        #endregion

        #region FrmCompanySearchWithFilter_Load method
        private void FrmCompanySearchWithFilter_Load(object sender, EventArgs e)
        {
            image = Properties.Resources.background;

            //LinkedinSearch.loggerscrap.addToLogger += new EventHandler(LinkedinSearchLogEvents_addToLogger);
            LinkedinSearch.loggerscrap.addToLogger += new EventHandler(LinkedinSearchFilterLogEvents_addToLogger);

            LoadPreScrapper();
            AddLocation();
            Uploaddata();

            try
            {
                comboBoxemail.SelectedIndex = 0;
                combScraperLocation.SelectedIndex = 0;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error >>> " + ex.StackTrace);
            }

        }
        #endregion

        #region AddLoggerLinkedinPremiumScrapeUsers

        private void AddLoggerScrapeUsers(string log)
        {
            this.Invoke(new MethodInvoker(delegate
            {
                lstLoglinkdinScarper.Items.Add(log);
                lstLoglinkdinScarper.SelectedIndex = lstLoglinkdinScarper.Items.Count - 1;
            }));
        }

        #endregion

        #region AddLoggerLinkedinFilterPremiumScrapeUsers

        private void AddLoggerFilterScrapeUsers(string log)
        {
            this.Invoke(new MethodInvoker(delegate
            {
                lstLoglinkdinScarper.Items.Add(log);
                lstLoglinkdinScarper.SelectedIndex = lstLoglinkdinScarper.Items.Count - 1;
            }));
        }

        #endregion

        #region FrmCompanySearchWithFilter_Paint methed
        private void FrmCompanySearchWithFilter_Paint(object sender, PaintEventArgs e)
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

        #region LoadPreScrapper() method
        private void LoadPreScrapper()
        {
            try
            {
                if (LinkedInManager.linkedInDictionary.Count() == 0)
                {
                    MessageBox.Show("Please Load LinkedIn Accounts From MyAccounts Menu");
                    AddLoggerScrapeUsers("[ " + DateTime.Now + " ] => [ Please Load LinkedIn Accounts From MyAccounts Menu ]");

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

        #region AddLocation() method
        private void AddLocation()
        {
            ClsSelect ObjSelectMethod = new ClsSelect();

            CountryCode = ObjSelectMethod.getCountry();

            foreach (KeyValuePair<string, string> pair in CountryCode)
            {
                try
                {
                    combScraperLocation.Items.Add(pair.Value);
                }
                catch
                {
                }
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
                AddLoggerScrapeUsers("[ " + DateTime.Now + " ] => [ Accounts Uploaded.. ]");
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> PopulateCmo() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> PopulateCmo() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPreScrapperErrorLogs);
            }
        }

        #endregion

        #region Uploaddata()
        public void Uploaddata()
        {

            try
            {
                string[] arrayJobOportunity = Regex.Split(JobOportunity, ",");
                foreach (string item in arrayJobOportunity)
                {
                    string[] arraybobap = Regex.Split(item, ":");
                    if (arraybobap.Length == 2)
                    {
                        checkedListJobOpportunity.Items.Add(arraybobap[1]);
                    }
                }
            }
            catch { }

        //    try
       //     {
      //          string[] arrayRelationship = Regex.Split(RelationshipList, ",");
      //          foreach (string item in arrayRelationship)
      //          {
         //           string[] arrayrelat = Regex.Split(item, ":");
    //                if (arrayrelat.Length == 2)
           //         {
      //                  checkedListRelationship.Items.Add(arrayrelat[1]);
        //            }
      //          }
     //       }
     //       catch { }


            try
            {
                string[] arrayIndustry = Regex.Split(IndustryList, ",");
                foreach (string item in arrayIndustry)
                {
                    string[] arrayind = Regex.Split(item, ";");
                    if (arrayind.Length == 2)
                    {
                        checkedListIndustry.Items.Add(arrayind[0]);
                    }
                }
            }
            catch { }

            try
            {
                string[] arrayCompanysize = Regex.Split(companysizelist, ",");
                foreach (string item in arrayCompanysize)
                {
                    string[] arrayInds = Regex.Split(item, ":");
                    if (arrayInds.Length == 2)
                    {
                        checkedListCompanySize.Items.Add(arrayInds[1]);
                    }
                }
            }
            catch { }

            try
            {
                string[] arrayFolloweList = Regex.Split(Followerlist, ",");
                foreach (string item in arrayFolloweList)
                {
                    string[] arrayfollower = Regex.Split(item, ":");
                    if (arrayfollower.Length == 2)
                    {
                        checkedListFollowers.Items.Add(arrayfollower[1]);
                    }

                }
            }
            catch { }

            try
            {
                string[] arrayFortuneList = Regex.Split(FortuneList, ",");
                foreach (string item in arrayFortuneList)
                {
                    string[] arrayFortune = Regex.Split(item, ":");
                    if (arrayFortune.Length == 2)
                    {
                        checkedListFortune.Items.Add(arrayFortune[1]);
                    }
                }
            }
            catch { }

        }
        #endregion

        #region comboBoxemail_SelectedIndexChanged
        private void comboBoxemail_SelectedIndexChanged(object sender, EventArgs e)
        {

            comboBoxemail.Invoke(new MethodInvoker(delegate
            {
                SearchCriteria.LoginID = comboBoxemail.SelectedItem.ToString();
            }));
        }
        #endregion

        #region btnSearchNewScraper_Click
        private void btnSearchNewScraper_Click(object sender, EventArgs e)
        {
            combScraperLocation.SelectedIndex = 0;
            txtKeyword.Text = string.Empty;
            txtOtherLocation.Text = string.Empty;
            lstLoglinkdinScarper.Items.Clear();

            for (int i = 0; i < checkedListJobOpportunity.Items.Count; i++)
            {
                try
                {
                    string aaa = Convert.ToString(checkedListJobOpportunity.Items[i]);
                    checkedListJobOpportunity.SetItemChecked(i, false);
                }
                catch { }
            }

            chkAllIndustry.Checked = false;
            for (int i = 0; i < checkedListIndustry.Items.Count; i++)
            {
                try
                {
                    string aaa = Convert.ToString(checkedListIndustry.Items[i]);
                    checkedListIndustry.SetItemChecked(i, false);
                }
                catch { }
            }

            chkAllCompanySize.Checked = false;
            for (int i = 0; i < checkedListCompanySize.Items.Count; i++)
            {
                try
                {
                    string aaa = Convert.ToString(checkedListCompanySize.Items[i]);
                    checkedListCompanySize.SetItemChecked(i, false);
                }
                catch { }
            }

            chkAllFolowers.Checked = false;
            for (int i = 0; i < checkedListFollowers.Items.Count; i++)
            {
                try
                {
                    string aaa = Convert.ToString(checkedListFollowers.Items[i]);
                    checkedListFollowers.SetItemChecked(i, false);
                }
                catch { }
            }

            chkAllFortune.Checked = false;
            for (int i = 0; i < checkedListFortune.Items.Count; i++)
            {
                try
                {
                    string aaa = Convert.ToString(checkedListFortune.Items[i]);
                    checkedListFortune.SetItemChecked(i, false);
                }
                catch { }
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
                AddLoggerScrapeUsers("------------------------------------------------------------------------------------------------------------------------------------");
                AddLoggerScrapeUsers("[ " + DateTime.Now + " ] => [ PROCESS STOPPED ]");
                AddLoggerScrapeUsers("------------------------------------------------------------------------------------------------------------------------------------");
                btnSearchScraper.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        #region btnSearchScraper_Click method
        private void btnSearchScraper_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {

                try
                {
                    SearchCriteria.starter = true;
                    if (comboBoxemail.Items.Count <= 0)
                    {
                        AddLoggerScrapeUsers("[ " + DateTime.Now + " ] => [ Load The account From Menu and switch the tab and again come to InBoardProGetData Tab ]");
                        frmAccounts FrmAccount = new frmAccounts();
                        FrmAccount.Show();
                        return;
                    }

                    if (comboBoxemail.SelectedIndex < 0)
                    {
                        AddLoggerScrapeUsers("[ " + DateTime.Now + " ] => [ Select The Email Id From The Dropdown and Fill All Mandatory (*) Fields ]");
                    }

                    AddLoggerScrapeUsers("[ " + DateTime.Now + " ] => [ Starting Scraping ]");
                    IsStop_InBoardProGetDataThread = false;
                    lstInBoardProGetDataThraed.Clear();

                    LinkedinSearch._Keyword = txtKeyword.Text.ToString();

                    #region Country Location
                    try
                    {
                        foreach (KeyValuePair<string, string> item in CountryCode)
                        {
                            try
                            {
                                if (item.Value == combScraperLocation.SelectedItem.ToString())
                                {
                                    SearchCriteria.Country = item.Key + "%3A0";
                                    break;
                                }
                            }
                            catch (Exception ex)
                            {
                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> getting CountryCode in Company filter search ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error -->  getting CountryCode in Company filter search ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPreScrapperErrorLogs);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnSearchScraper_Click() in Company filter search ---2--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnSearchScraper_Click() in Company filter search ---2--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPreScrapperErrorLogs);
                    }
                    #endregion

                    #region OtherLocation

                    SearchCriteria.OtherLocation = txtOtherLocation.Text.ToString();

                    #endregion

       //             #region Relationship List

      //              if (chkAllLinkedinMem.Checked == false)
       //             {
        //                string Relationship = string.Empty;
        //                string[] arrRelation = Regex.Split(RelationshipList, ",");
         //               foreach (string RelationL in checkedListRelationship.CheckedItems)
          //              {
         //                   foreach (string item in arrRelation)
             //               {
               //                 try
              //                  {
              //                      if (item.Contains(RelationL))
               //                     {
                //                        string[] arryRelat = Regex.Split(item, ":");
                    //                    if (arryRelat.Length == 2)
                //                        {
                           //                 if (!string.IsNullOrEmpty(arryRelat[0]))
                             //               {
                    //                            if (string.IsNullOrEmpty(Relationship))
                    //                            {
                    //                                Relationship = arryRelat[0];
                    //                            }
                    //                            else
                     //                           {
                      //                              Relationship = arryRelat[0] + "," + Relationship;
                       //                         }
                        //                    }
                      //                  }
                   //                 }
                  //              }
                //                catch (Exception ex)
                //                {
               //                     GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnSearchScraper_Click() ---7--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
               //                     GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnSearchScraper_Click() ---7--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPreScrapperErrorLogs);
               //                 }
              //              }
              //          }
              //          if (string.IsNullOrEmpty(Relationship))
             //           {
             //               Relationship = "";
            //            }
           //             SearchCriteria.Relationship = Relationship;
           //         }
            //        #endregion

                    #region Industry List

                    if (chkAllIndustry.Checked == false)
                    {
                        string Industryvalue = string.Empty;
                        string[] arrIndustry = Regex.Split(IndustryList, ",");

                        foreach (string Industry in checkedListIndustry.CheckedItems)
                        {
                            foreach (string itemIndustry in arrIndustry)
                            {
                                try
                                {
                                    string matchindstry = itemIndustry.Split(';')[0];

                                    if (matchindstry == Industry)
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
                                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> arrIndustry 1 in Company filter search ---3--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> arrIndustry 1 in Company filter search ---3--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPreScrapperErrorLogs);
                                }
                            }
                        }

                        SearchCriteria.IndustryType = Industryvalue;
                    }
                    #endregion

                    #region Company size List

                    if (chkAllCompanySize.Checked == false)
                    {
                        string companysizevalue = string.Empty;
                        string[] arraycompnaysize = Regex.Split(companysizelist, ",");
                        foreach (string companysize in checkedListCompanySize.CheckedItems)
                        {
                            foreach (string item in arraycompnaysize)
                            {
                                try
                                {
                                    if (item.Contains(companysize))
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

                                    }
                                }
                                catch (Exception ex)
                                {
                                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnSearchScraper_Click() in Company filter search ---5--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnSearchScraper_Click() in Company filter search ---5--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPreScrapperErrorLogs);
                                }
                            }

                        }
                        SearchCriteria.CompanySize = companysizevalue;
                    }
                    #endregion

                    #region Follower List

                    if (chkAllFolowers.Checked == false)
                    {
                        string Followervalue = string.Empty;
                        string[] arrayFollowerList = Regex.Split(Followerlist, ",");
                        foreach (string follower in checkedListFollowers.CheckedItems)
                        {
                            foreach (string item in arrayFollowerList)
                            {
                                try
                                {
                                    string matchfollowitem = item.Split(':')[1];
                                    if (matchfollowitem == follower)
                                    {
                                        string[] ArrayFollower = Regex.Split(item, ":");

                                        if (!string.IsNullOrEmpty(ArrayFollower[0]))
                                        {
                                            if (string.IsNullOrEmpty(Followervalue))
                                            {
                                                Followervalue = ArrayFollower[0];
                                            }
                                            else
                                            {
                                                Followervalue = Followervalue + "," + ArrayFollower[0];
                                            }
                                        }

                                    }
                                }
                                catch (Exception ex)
                                {
                                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnSearchScraper_Click() in Company filter search ---4--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnSearchScraper_Click() in Company filter search ---4--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPreScrapperErrorLogs);
                                }
                            }

                        }
                        SearchCriteria.Follower = Followervalue;
                    }
                    #endregion

                    #region Fortune List
                    if (chkAllFortune.Checked == false)
                    {
                        string Fortunevalue = string.Empty;
                        string[] arrayFortuneList = Regex.Split(FortuneList, ",");
                        foreach (string fortune in checkedListFortune.CheckedItems)
                        {
                            foreach (string item in arrayFortuneList)
                            {
                                try
                                {
                                    if (item.Contains(fortune))
                                    {
                                        string[] ArrayFortune = Regex.Split(item, ":");


                                        if (!string.IsNullOrEmpty(ArrayFortune[0]))
                                        {
                                            if (string.IsNullOrEmpty(Fortunevalue))
                                            {
                                                Fortunevalue = ArrayFortune[0];
                                            }
                                            else
                                            {
                                                Fortunevalue = Fortunevalue + "," + ArrayFortune[0];
                                            }
                                        }

                                    }
                                }
                                catch (Exception ex)
                                {
                                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnSearchScraper_Click() in Company filter search ---5--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> btnSearchScraper_Click() in Company filter search ---5--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPreScrapperErrorLogs);
                                }

                            }

                        }
                        SearchCriteria.Fortune1000 = Fortunevalue;
                    }
                    #endregion

                    btnSearchScraper.Cursor = Cursors.AppStarting;

                    new Thread(() =>
                    {
                        StartLinkedInCompanySearch();

                    }).Start();

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

        #region StartLinkedInCompanySearch()

        private void StartLinkedInCompanySearch()
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
                LinkedinSearch objlinkscrCompanyFilter = new LinkedinSearch();
                bool isLoggedIn = Login_InBoardProGetData();

                LinkedinSearch._Search = "CompaniesWithFilter";

                //AddLoggerScrapeUsers("[ " + DateTime.Now + " ] => [ Start LinkedIn Crawling ]");

                if (isLoggedIn)
                {
                    AddLoggerScrapeUsers("[ " + DateTime.Now + " ] => [ Start LinkedIn Crawling ]");
                    objlinkscrCompanyFilter.StartLinkedInSearch(ref HttpHelper);
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

        #region Login_InBoardProGetData()
        private bool Login_InBoardProGetData()
        {
            bool isLoggedin = false;
            try
            {
                try
                {
                    AddLoggerScrapeUsers("[ " + DateTime.Now + " ] => [ Process Running Please wait...]");

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
                                        AddLoggerScrapeUsers("[ " + DateTime.Now + " ] => [ Logged in With : " + acc + " ]");
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

        #region combScraperLocation_SelectedIndexChanged
        private void combScraperLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            string aa = combScraperLocation.SelectedItem.ToString();

            if (aa == "Other")
            {
                txtOtherLocation.Enabled = true;
                txtOtherLocation.Focus();
            }
            else
            {
                txtOtherLocation.Clear();
                txtOtherLocation.Enabled = false;
            }

        }
        #endregion

  /*      #region checkedListRelationship_SelectedIndexChanged
        private void checkedListRelationship_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                AllRelationship_bool = false;
                chkAllLinkedinMem.Checked = false;

            }
            catch { }

        } 
        #endregion

        #region chkAllLinkedinMem_CheckedChanged
        private void chkAllLinkedinMem_CheckedChanged(object sender, EventArgs e)
        {
            SearchCriteria.Relationship = "N";

            try
            {
                if (chkAllLinkedinMem.Checked)
                {
                    for (int i = 0; i < checkedListRelationship.Items.Count; i++)
                    {
                        try
                        {
                            string aaa = Convert.ToString(checkedListRelationship.Items[i]);
                            checkedListRelationship.SetItemChecked(i, true);
                        }
                        catch { }
                    }
                    AllRelationship_bool = true;
                }
                else
                {
                    if (AllRelationship_bool != false)
                    {
                        for (int i = 0; i < checkedListRelationship.Items.Count; i++)
                        {
                            try
                            {
                                string aaa = Convert.ToString(checkedListRelationship.Items[i]);
                                checkedListRelationship.SetItemChecked(i, false);
                            }
                            catch { }
                        }
                    }
                    AllRelationship_bool = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error >>> " + ex.StackTrace);
            }

            //for (int i = 0; i < checkedListRelationship.Items.Count; i++)
            //{
            //    try
            //    {
            //        checkedListRelationship.SetItemChecked(i, false);
            //    }
            //    catch { }
            //}
        } 
        #endregion    */

        #region chkAllIndustry_CheckedChanged
        private void chkAllIndustry_CheckedChanged(object sender, EventArgs e)
        {
            SearchCriteria.IndustryType = "Y";

            try
            {
                if (chkAllIndustry.Checked)
                {
                    for (int i = 0; i < checkedListIndustry.Items.Count; i++)
                    {
                        try
                        {
                            string aaa = Convert.ToString(checkedListIndustry.Items[i]);
                            checkedListIndustry.SetItemChecked(i, true);
                        }
                        catch { }
                    }
                    AllIndustry_bool = true;
                }
                else
                {
                    if (AllIndustry_bool != false)
                    {
                        for (int i = 0; i < checkedListIndustry.Items.Count; i++)
                        {
                            try
                            {
                                string aaa = Convert.ToString(checkedListIndustry.Items[i]);
                                checkedListIndustry.SetItemChecked(i, false);
                            }
                            catch { }
                        }
                    }

                    AllIndustry_bool = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error >>> " + ex.StackTrace);
            }

          } 
        #endregion

        #region checkedListIndustry_SelectedIndexChanged
        private void checkedListIndustry_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                AllIndustry_bool = false;
                chkAllIndustry.Checked = false;
            }
            catch { }
        } 
        #endregion

        #region chkAllCompanySize_CheckedChanged
        private void chkAllCompanySize_CheckedChanged(object sender, EventArgs e)
        {
            SearchCriteria.CompanySize = "Y";

            try
            {
                if (chkAllCompanySize.Checked)
                {
                    for (int i = 0; i < checkedListCompanySize.Items.Count; i++)
                    {
                        try
                        {
                            string aaa = Convert.ToString(checkedListCompanySize.Items[i]);
                            checkedListCompanySize.SetItemChecked(i, true);
                        }
                        catch { }
                    }
                    AllCompnaySize_bool = true;
                }
                else
                {
                    if (AllCompnaySize_bool != false)
                    {
                        for (int i = 0; i < checkedListCompanySize.Items.Count; i++)
                        {
                            try
                            {
                                string aaa = Convert.ToString(checkedListCompanySize.Items[i]);
                                checkedListCompanySize.SetItemChecked(i, false);
                            }
                            catch { }
                        }
                    }
                    AllCompnaySize_bool = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error >>> " + ex.StackTrace);
            }

            //for (int i = 0; i < checkedListCompanySize.Items.Count; i++)
            //{
            //    try
            //    {
            //        checkedListCompanySize.SetItemChecked(i, false);
            //    }
            //    catch { }
            //}
        } 
        #endregion

        #region checkedListCompanySize_SelectedIndexChanged
        private void checkedListCompanySize_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                AllCompnaySize_bool = false;
                chkAllCompanySize.Checked = false;
            }
            catch { }
        } 
        #endregion

        #region chkAllFolowers_CheckedChanged
        private void chkAllFolowers_CheckedChanged(object sender, EventArgs e)
        {

            SearchCriteria.Follower = "Y";

            try
            {
                if (chkAllFolowers.Checked)
                {
                    for (int i = 0; i < checkedListFollowers.Items.Count; i++)
                    {
                        try
                        {
                            string aaa = Convert.ToString(checkedListFollowers.Items[i]);
                            checkedListFollowers.SetItemChecked(i, true);
                        }
                        catch { }
                    }
                    AllFollower_bool = true;
                }
                else
                {
                    if (AllFollower_bool != false)
                    {
                        for (int i = 0; i < checkedListFollowers.Items.Count; i++)
                        {
                            try
                            {
                                string aaa = Convert.ToString(checkedListCompanySize.Items[i]);
                                checkedListFollowers.SetItemChecked(i, false);
                            }
                            catch { }
                        }
                    }
                    AllFollower_bool = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error >>> " + ex.StackTrace);
            }

            //for (int i = 0; i < checkedListFollowers.Items.Count; i++)
            //{
            //    try
            //    {
            //        checkedListFollowers.SetItemChecked(i, false);
            //    }
            //    catch { }
            //}
        } 
        #endregion

        #region checkedListFollowers_SelectedIndexChanged
        private void checkedListFollowers_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                AllFollower_bool = false;
                chkAllFolowers.Checked = false;
            }
            catch { }
        } 
        #endregion

        #region chkAllFortune_CheckedChanged
        private void chkAllFortune_CheckedChanged(object sender, EventArgs e)
        {
            SearchCriteria.Fortune1000 = "Y";

            try
            {
                if (chkAllFortune.Checked)
                {
                    for (int i = 0; i < checkedListFortune.Items.Count; i++)
                    {
                        try
                        {
                            string aaa = Convert.ToString(checkedListFortune.Items[i]);
                            checkedListFortune.SetItemChecked(i, true);
                        }
                        catch { }
                    }
                    AllFortune_bool = true;
                }
                else
                {
                    if (AllFortune_bool != false)
                    {
                        for (int i = 0; i < checkedListFortune.Items.Count; i++)
                        {
                            try
                            {
                                string aaa = Convert.ToString(checkedListFortune.Items[i]);
                                checkedListFortune.SetItemChecked(i, false);
                            }
                            catch { }
                        }
                    }
                    AllFortune_bool = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error >>> " + ex.StackTrace);
            }

            //for (int i = 0; i < checkedListFortune.Items.Count; i++)
            //{
            //    try
            //    {
            //        checkedListFortune.SetItemChecked(i, false);
            //    }
            //    catch { }
            //}
        } 
        #endregion

        #region checkedListFortune_SelectedIndexChanged
        private void checkedListFortune_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                AllFortune_bool = false;
                chkAllFortune.Checked = false;
            }
            catch { }
        } 
        #endregion
    }
}