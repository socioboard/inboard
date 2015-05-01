using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using InBoardPro;
using System.Text.RegularExpressions;
using System.Threading;
using Chilkat;
using System.Collections.Generic;
using System;
using System.Drawing.Drawing2D;
using InBoardProGetData;
using BaseLib;

namespace InBoardPro
{
    public partial class frmCampaignScraper : Form
    {
        public System.Drawing.Image image;
        ChilkatHttpHelpr objChilkat = new ChilkatHttpHelpr();
        public frmCampaignScraper()
        {
            InitializeComponent();
        }

        private void frmCampaignScraper_Load(object sender, EventArgs e)
        {
            image = Properties.Resources.background;

            clsDBQueryManager DQ = new clsDBQueryManager();
            DataSet DS = new DataSet();
            DataTable DT = new DataTable();
            DataGridViewCheckBoxColumn chk = new DataGridViewCheckBoxColumn();
            try
            {
                DS = new clsDBQueryManager().getAllCampaignScraperData();

                DT = DS.Tables[0];
                
                dgv_CampaignScraper.DataSource = DT.DefaultView;
            }
            catch
            { }
        }


        public static Events CampaignStopLogevents = new Events();
        public void RaiseCampaignSearchLogEvents(string log)
        {         

            EventsArgs eArgs = new EventsArgs(log);
            CampaignStopLogevents.LogText(eArgs);
        }


        public void AddToLogger(string Log)                                                                       //*------ Logger method------*//
        {
            {
                try
                {
                    if (lstLogger.Items.Count > 300)
                    {
                        lstLogger.Invoke(new MethodInvoker(delegate
                        {
                            lstLogger.Items.Clear();
                        }));
                    }

                    if (lstLogger.InvokeRequired)
                    {
                        try
                        {
                            lstLogger.Invoke(new MethodInvoker(delegate
                            {
                                lstLogger.Items.Add(Log);
                                lstLogger.SelectedIndex = lstLogger.Items.Count - 1;
                            }));
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.StackTrace);
                        }
                    }
                    else
                    {
                        try
                        {
                            lstLogger.Items.Add(Log);
                            lstLogger.SelectedIndex = lstLogger.Items.Count - 1;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error >>>" + ex.Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error >>>" + ex.Message);
                }
            }
        }



        private void StartCampaign_Click(object sender, System.EventArgs e)
        {
            SearchCriteria.starter = true;
           

            //string[] ParametersPassed = new string[] { };

            try
            {
                foreach (DataGridViewRow Dr in dgv_CampaignScraper.Rows)
                {
                    string campname = string.Empty;
                    string AccountData = string.Empty;
                    string FirstName = string.Empty;
                    string LastName = string.Empty;
                    string Location = string.Empty;
                    string Country = string.Empty;
                    string LocationArea = string.Empty;
                    string PostalCode = string.Empty;
                    string Company = string.Empty;
                    string Keyword = string.Empty;
                    string Title = string.Empty;
                    string IndustryType = string.Empty;
                    string Relationship = string.Empty;
                    string language = string.Empty;
                    string Groups = string.Empty;
                    string FileName = string.Empty;
                    string TitleValue = string.Empty;
                    string CompanyValue = string.Empty;
                    string within = string.Empty;
                    string YearsOfExperience = string.Empty;
                    string Function = string.Empty;
                    string SeniorLevel = string.Empty;
                    string IntrestedIn = string.Empty;
                    string CompanySize = string.Empty;
                    string Fortune1000 = string.Empty;
                    string RecentlyJoined = string.Empty;


                    //new Thread(() =>
                                     // {
                                          try
                                          {
                                              if (Convert.ToBoolean(Dr.Cells[0].Value) == true)
                                              {
                                                  try
                                                  {
                                                      
                                                      lock (this)
                                                      {
                                                          
                                                          campname = Dr.Cells[1].Value.ToString();
                                                          AccountData = Dr.Cells[2].Value.ToString();
                                                          FirstName = Dr.Cells[3].Value.ToString();
                                                          LastName = Dr.Cells[4].Value.ToString();
                                                          Location = Dr.Cells[5].Value.ToString();
                                                          Country = Dr.Cells[6].Value.ToString();
                                                          LocationArea = Dr.Cells[7].Value.ToString();
                                                          PostalCode = Dr.Cells[8].Value.ToString();
                                                          Company = Dr.Cells[9].Value.ToString();
                                                          Keyword = Dr.Cells[10].Value.ToString();
                                                          Title = Dr.Cells[11].Value.ToString();
                                                          IndustryType = Dr.Cells[12].Value.ToString();
                                                          Relationship = Dr.Cells[13].Value.ToString();
                                                          language = Dr.Cells[14].Value.ToString();
                                                          Groups = Dr.Cells[15].Value.ToString();
                                                          FileName = Dr.Cells[16].Value.ToString();
                                                          TitleValue = Dr.Cells[17].Value.ToString();
                                                          CompanyValue = Dr.Cells[18].Value.ToString();
                                                          within = Dr.Cells[19].Value.ToString();
                                                          YearsOfExperience = Dr.Cells[20].Value.ToString();
                                                          Function = Dr.Cells[21].Value.ToString();
                                                          SeniorLevel = Dr.Cells[22].Value.ToString();
                                                          IntrestedIn = Dr.Cells[23].Value.ToString();
                                                          CompanySize = Dr.Cells[24].Value.ToString();
                                                          Fortune1000 = Dr.Cells[25].Value.ToString();
                                                          RecentlyJoined = Dr.Cells[26].Value.ToString();

                                                          ThreadPool.QueueUserWorkItem(new WaitCallback(StartProcessWithMultiThread), new object[] { campname, AccountData, FirstName, LastName, Location, Country, LocationArea, PostalCode, Company, Keyword, Title, IndustryType, Relationship, language, Groups, FileName, TitleValue, CompanyValue, within, YearsOfExperience, Function, SeniorLevel, IntrestedIn, CompanySize, Fortune1000, RecentlyJoined });
                                                          //RaiseCampaignSearchLogEvents(AccountData + ":" + FirstName + ":" + LastName + ":" + Location + ":" + Country + ":" + LocationArea + ":" + PostalCode + ":" + Company + ":" + Keyword + ":" + Title + ":" + IndustryType + ":" + Relationship + ":" + language + ":" + Groups + ":" + FileName + ":" + TitleValue + ":" + CompanyValue + ":" + within + ":" + YearsOfExperience + ":" + Function + ":" + SeniorLevel + ":" + IntrestedIn + ":" + CompanySize + ":" + Fortune1000 + ":" + RecentlyJoined);
                                                      }
                                                  }
                                                  catch
                                                  { }
                                              }

                                              else
                                              {
                                              }
                                          }
                                          catch
                                          { }
                                      //}).Start();

                }
            }
            catch
            { }


        }


        private void StartProcessWithMultiThread(object parameters)
        {
            try
            {
                Array paramsArray = new object[25];

                paramsArray = (Array)parameters;

                string campname = (string)paramsArray.GetValue(0);
                string AccountData = (string)paramsArray.GetValue(1);
                string FirstName = (string)paramsArray.GetValue(2);
                string LastName = (string)paramsArray.GetValue(3);
                string Location = (string)paramsArray.GetValue(4);
                string Country = (string)paramsArray.GetValue(5);
                string LocationArea = (string)paramsArray.GetValue(6);
                string PostalCode = (string)paramsArray.GetValue(7);
                string Company = (string)paramsArray.GetValue(8);
                string Keyword = (string)paramsArray.GetValue(9);
                string Title = (string)paramsArray.GetValue(10);
                string IndustryType = (string)paramsArray.GetValue(11);
                string Relationship = (string)paramsArray.GetValue(12);
                string language = (string)paramsArray.GetValue(13);
                string Groups = (string)paramsArray.GetValue(14);
                string FileName = (string)paramsArray.GetValue(15);
                string TitleValue = (string)paramsArray.GetValue(16);
                string CompanyValue = (string)paramsArray.GetValue(17);
                string within = (string)paramsArray.GetValue(18);
                string YearsOfExperience = (string)paramsArray.GetValue(19);
                string Function = (string)paramsArray.GetValue(20);
                string SeniorLevel = (string)paramsArray.GetValue(21);
                string IntrestedIn = (string)paramsArray.GetValue(22);
                string CompanySize = (string)paramsArray.GetValue(23);
                string Fortune1000 = (string)paramsArray.GetValue(24);
                string RecentlyJoined = (string)paramsArray.GetValue(25);


                //GlobusHttpHelper HttpHelper = new GlobusHttpHelper();
                //LinkedInScrape objlinkscr = new LinkedInScrape();
                //bool isLoggedIn = Login_InBoardProGetDataForCampaignScraper(ref HttpHelper, Account, password, ProxyAddress, ProxyPort, ProxyUserName, ProxyPassword);

                StartInBoardProGetData(AccountData + ":" + FirstName + ":" + LastName + ":" + Location + ":" + Country + ":" + LocationArea + ":" + PostalCode + ":" + Company + ":" + Keyword + ":" + Title + ":" + IndustryType + ":" + Relationship + ":" + language + ":" + Groups + ":" + FileName + ":" + TitleValue + ":" + CompanyValue + ":" + within + ":" + YearsOfExperience + ":" + Function + ":" + SeniorLevel + ":" + IntrestedIn + ":" + CompanySize + ":" + Fortune1000 + ":" + RecentlyJoined);
                //RaiseCampaignSearchLogEvents(AccountData + ":" + FirstName + ":" + LastName + ":" + Location + ":" + Country + ":" + LocationArea + ":" + PostalCode + ":" + Company + ":" + Keyword + ":" + Title + ":" + IndustryType + ":" + Relationship + ":" + language + ":" + Groups + ":" + FileName + ":" + TitleValue + ":" + CompanyValue + ":" + within + ":" + YearsOfExperience + ":" + Function + ":" + SeniorLevel + ":" + IntrestedIn + ":" + CompanySize + ":" + Fortune1000 + ":" + RecentlyJoined);
            }
            catch { }
        }



        public void StartInBoardProGetData(string args)
        {


            try
            {
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


                GlobusHttpHelper HttpHelper = new GlobusHttpHelper();
                LinkedInScrape objlinkscr = new LinkedInScrape();
                LinkedInMaster objLinkedInMaster = new LinkedInMaster();

                AddToLogger("[ " + DateTime.Now + " ] => [ Logging with Account :  " + Account + " ]");
                objLinkedInMaster.LoginHttpHelperForCampaignScraper(ref HttpHelper, Account, password, ProxyAddress, ProxyPort, ProxyUserName, ProxyPassword);

                if (objLinkedInMaster.IsLoggedIn)
                {
                    AddToLogger("[ " + DateTime.Now + " ] => [ Logged in  with Account :  " + Account + " ]");
                    AddToLogger("[ " + DateTime.Now + " ] => [ Start LinkedIn Crawling ]");
                    StartCampaignInBoardProGetDataWithPagination(ref HttpHelper, Account, FirstName, LastName, Location, Country, LocationArea, PostalCode, Company, Keyword, Title, IndustryType, Relationship, language, Groups, FileName, TitleValue, CompanyValue, within, YearsOfExperience, Function, SeniorLevel, IntrestedIn, CompanySize, Fortune1000, RecentlyJoined);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error >>> " + ex.StackTrace);
            }
            finally
            {
               
            }
        }


        GlobusHttpHelper _HttpHelper = new GlobusHttpHelper();
        public void StartCampaignInBoardProGetDataWithPagination(ref GlobusHttpHelper HttpHelper, string Account, string FirstName, string LastName, string Location, string Country, string LocationArea, string PostalCode, string Company, string Keyword, string Title, string IndustryType, string Relationship, string language, string Groups, string FileName, string TitleValue, string CompanyValue, string within, string YearsOfExperience, string Function, string SeniorLevel, string IntrestedIn, string CompanySize, string Fortune1000, string RecentlyJoined)
        {
            string ResponseWallPostForPremiumAcc = string.Empty;
            string NewSearchPage = string.Empty;
            string PostResponce = string.Empty;
            string PostRequestURL = string.Empty;
            string PostdataForPagination = string.Empty;
            string csrfToken = string.Empty;
            Queue<string> queRecordUrl = new Queue<string>();
            List<string> RecordURL = new List<string>();

            #region Login
            try
            {
                //Temprary class
                //======================================================
                //string tempurl = "http://www.linkedin.com/profile/view?id=224916256&authType=OUT_OF_NETWORK&authToken=SWNz&locale=en_US&srchid=3387141351401255871148&srchindex=1&srchtotal=2017&trk=vsrp_people_res_name&trkInfo=VSRPsearchId%3A3387141351401255871148%2CVSRPtargetId%3A224916256%2CVSRPcmpt%3Aprimary";
                //CrawlingLinkedInPage(tempurl, ref HttpHelper);
                //======================================================


                if (SearchCriteria.starter)
                {
                    #region Serch

                    string pageSourceaAdvanceSearch = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/search"));
                    NewSearchPage = string.Empty;

                    if (pageSourceaAdvanceSearch.Contains("csrfToken"))
                    {
                        try
                        {
                            int startindex = pageSourceaAdvanceSearch.IndexOf("csrfToken");
                            if (startindex > 0)
                            {
                                string start = pageSourceaAdvanceSearch.Substring(startindex);
                                int endindex = start.IndexOf(">");
                                string end = start.Substring(0, endindex);
                                csrfToken = end.Replace("csrfToken=", "").Replace("\\", "").Replace("\"", string.Empty); ;
                            }
                        }
                        catch { }

                    }



                    try
                    {
                        if (Location == "Y")
                        {
                            Country = string.Empty;
                        }

                        //if (NewSearchPage == string.Empty)
                        //{
                        //    string PostDataForPrimiumAccount = "csrfToken=" + csrfToken + "&keepFacets=true&pplSearchOrigin=ADVS&keywords=" + Uri.EscapeDataString(SearchCriteria.Keyword) + "&fname=" + SearchCriteria.FirstName + "&lname=" + SearchCriteria.LastName + "&companyScope=" + SearchCriteria.CompanyValue + "&searchLocationType=" + SearchCriteria.Location + "&countryCode=" + SearchCriteria.Country + "&postalCode=" + SearchCriteria.PostalCode + "&distance=" + SearchCriteria.within + "&title=&company=" + SearchCriteria.Company + "&currentCompany=" + SearchCriteria.CompanyValue + "&school=&I=" + SearchCriteria.IndustryType + "&FG=" + SearchCriteria.Group + "&N=" + SearchCriteria.Relationship + "&L=" + SearchCriteria.language + "&FA=" + SearchCriteria.Function + "&CS=" + SearchCriteria.CompanySize + "&SE=" + SearchCriteria.SeniorLevel + "&P=" + SearchCriteria.InerestedIn + "&TE=" + SearchCriteria.YearOfExperience + "&DR=" + SearchCriteria.RecentlyJoined + "&F=" + SearchCriteria.Fortune1000 + "&sortCriteria=R&viewCriteria=2&%2Fsearch%2Ffpsearch=Search";
                        //    ResponseWallPostForPremiumAcc = HttpHelper.postFormData(new Uri("http://www.linkedin.com/search/fpsearch"), PostDataForPrimiumAccount);
                        //}
                        //else
                        {
                            string GetDataForPrimiumAccount = string.Empty;
                            GetDataForPrimiumAccount = "http://www.linkedin.com/vsearch/p?openAdvancedForm=true&keywords=" + Uri.EscapeDataString(Keyword) + "&title=" + Title + "&titleScope=" + TitleValue + "&firstName=" + FirstName + "&lastName=" + LastName + "&postalCode=" + PostalCode + "&companyScope=" + CompanyValue + "&locationType=" + Location + "&countryCode=" + Country + "&company=" + Company + "&distance=" + within + "&f_FG=" + Groups + "&f_L=" + language + "&f_N=" + Relationship + "&f_G=" + LocationArea + "&f_I=" + IndustryType + "&f_TE=" + YearsOfExperience + "&f_FA=" + Function + "&f_SE=" + SeniorLevel + "&f_P=" + IntrestedIn + "&f_CS=" + CompanySize + "&f_F=" + Fortune1000 + "&f_DR=" + RecentlyJoined + "&orig=ADVS";
                            ResponseWallPostForPremiumAcc = HttpHelper.getHtmlfromUrl1(new Uri(GetDataForPrimiumAccount));

                        }

                    }
                    catch { }




                    string facetsOrder = string.Empty;
                    if (PostResponce.Contains("facetsOrder"))
                    {
                        facetsOrder = ResponseWallPostForPremiumAcc.Substring(PostResponce.IndexOf("facetsOrder"), 200);
                        string[] Arr3 = facetsOrder.Split('"');
                        facetsOrder = Arr3[2];
                        string DecodedCharTest = Uri.UnescapeDataString(facetsOrder);
                        string DecodedEmail = Uri.EscapeDataString(facetsOrder);
                        facetsOrder = DecodedEmail;
                    }
                    #endregion
                }
                int pagenumber = 0;
                string strPageNumber = string.Empty;
                string[] Arr12 = Regex.Split(ResponseWallPostForPremiumAcc, "<li");
                foreach (string item in Arr12)
                {
                    if (SearchCriteria.starter)
                    {
                        #region Loop
                        if (!item.Contains("<!DOCTYPE"))
                        {
                            if (item.Contains("results-summary"))
                            {
                                string data = RemoveAllHtmlTag.StripHTML(item);
                                data = data.Replace("\n", "");
                                if (data.Contains(">"))
                                {
                                    string[] ArrTemp = data.Split('>');
                                    data = ArrTemp[1];
                                    data = data.Replace("results", "");
                                    data = data.Trim();
                                    string[] ArrTemp1 = data.Split(' ');
                                    data = ArrTemp1[0].Replace(',', ' ').Trim();
                                    strPageNumber = data.Replace(" ", string.Empty);
                                    break;
                                }

                            }
                        }
                        #endregion
                    }
                }

                if (string.IsNullOrEmpty(strPageNumber))
                {
                    try
                    {
                        if (ResponseWallPostForPremiumAcc.Contains("resultCount"))
                        {
                            string[] countResultArr = Regex.Split(ResponseWallPostForPremiumAcc, "resultCount");

                            if (countResultArr.Length > 1)
                            {
                                string tempResult = countResultArr[1].Substring(0, countResultArr[1].IndexOf(","));

                                if (tempResult.Contains("<strong>"))
                                {
                                    strPageNumber = tempResult.Substring(tempResult.IndexOf("<strong>"), tempResult.IndexOf("</strong>", tempResult.IndexOf("<strong>")) - tempResult.IndexOf("<strong>")).Replace("<strong>", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Trim();
                                }
                                else if (tempResult.Contains(":"))
                                {
                                    strPageNumber = tempResult.Replace(":", string.Empty).Replace("\"", string.Empty);
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {

                    }
                }

                if (string.IsNullOrEmpty(strPageNumber))
                {
                    try
                    {
                        if (ResponseWallPostForPremiumAcc.Contains("results_count_without_keywords_i18n"))
                        {
                            string[] countResultArr = Regex.Split(ResponseWallPostForPremiumAcc, "results_count_without_keywords_i18n");

                            if (countResultArr.Length > 1)
                            {
                                string tempResult = countResultArr[1].Substring(0, countResultArr[1].IndexOf(","));

                                if (tempResult.Contains("<strong>"))
                                {
                                    strPageNumber = tempResult.Substring(tempResult.IndexOf("<strong>"), tempResult.IndexOf("</strong>", tempResult.IndexOf("<strong>")) - tempResult.IndexOf("<strong>")).Replace("<strong>", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Trim();
                                }
                                else if (tempResult.Contains(":"))
                                {
                                    strPageNumber = tempResult.Replace(":", string.Empty).Replace("\"", string.Empty);
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {

                    }
                }

                string logtag = string.Empty;

                try
                {
                    pagenumber = int.Parse(strPageNumber);

                    AddToLogger("[ " + DateTime.Now + " ] => [ Total Results :  " + pagenumber + " ]");
                }
                catch (Exception)
                {

                }

                pagenumber = (pagenumber / 10) + 1;

                if (pagenumber == -1)
                {
                    pagenumber = 2;
                }

                if (pagenumber == 1)
                {
                    pagenumber = 2;
                }

                string GetResponce = string.Empty;
                string GetRequestURL = string.Empty;

                if (pagenumber >= 1)
                {
                    _HttpHelper = HttpHelper;

                    //new Thread(() =>
                   // {
                        if (SearchCriteria.starter)
                        {
                            //string CheckString = "CampaignScraper" + FileName;
                            //finalUrlCollection(RecordURL);

                        }

                   // }).Start();

                    for (int i = 1; i <= pagenumber; i++)
                    {
                        if (SearchCriteria.starter)
                        {
                            #region loop

                            if (ResponseWallPostForPremiumAcc.Contains("Account Type:</span> Basic"))
                            {
                                try
                                {
                                    PostRequestURL = "http://www.linkedin.com/search/hits";
                                    PostdataForPagination = "keywords=" + Uri.EscapeDataString(Keyword) + "&title=" + Uri.EscapeDataString(Title) + "&fname=" + FirstName + "&lname=" + LastName + "&searchLocationType=" + Location + "&f_FG=" + Groups + "&companyScope=" + CompanyValue + "&countryCode=" + Country + "&company=" + Company + "&viewCriteria=1&sortCriteria=R&facetsOrder=CC%2CN%2CG%2CI%2CPC%2CED%2CL%2CFG%2CTE%2CFA%2CSE%2CP%2CCS%2CF%2CDR&page_num=" + i + "&openFacets=N%2CCC%2CG";
                                    PostResponce = HttpHelper.postFormData(new Uri(PostRequestURL), PostdataForPagination);
                                    //Temporosy code for client
                                    //GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Pagesource 3 >>>> " + PostResponce, Globals.Path_InBoardProGetDataPagesource);

                                }
                                catch { }
                            }
                            else if (ResponseWallPostForPremiumAcc.Contains("Account Type:</span> Executive"))
                            {
                                try
                                {
                                    PostRequestURL = "http://www.linkedin.com/search/hits";
                                    PostdataForPagination = "keywords=" + Uri.EscapeDataString(Keyword) + "&title=" + Uri.EscapeDataString(Title) + "&fname=" + FirstName + "&lname=" + LastName + "&searchLocationType=" + Location + "&f_FG=" + Groups + "&companyScope=" + CompanyValue + "&countryCode=" + Country + "&keepFacets=keepFacets&I=" + IndustryType + "&SE=" + SeniorLevel + "&pplSearchOrigin=ADVS&viewCriteria=2&sortCriteria=R&facetsOrder=N%2CCC%2CI%2CPC%2CED%2CL%2CFG%2CTE%2CFA%2CSE%2CP%2CCS%2CF%2CDR%2CG&page_num=" + i + "&openFacets=N%2CCC%2CI";

                                    PostResponce = HttpHelper.postFormData(new Uri(PostRequestURL), PostdataForPagination);
                                    //Temporosy code for client
                                    //GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Pagesource 3 >>>> " + PostResponce, Globals.Path_InBoardProGetDataPagesource);
                                }
                                catch { }
                            }
                            else if (ResponseWallPostForPremiumAcc.Contains("openAdvancedForm=true"))
                            {
                                PostRequestURL = "http://www.linkedin.com/vsearch/p?";
                                PostdataForPagination = "keywords=" + Uri.EscapeDataString(Keyword) + "&title=" + Uri.EscapeDataString(Title) + "&titleScope=" + TitleValue + "&firstName=" + FirstName + "&lastName=" + LastName + "&postalCode=" + PostalCode + "&openAdvancedForm=true&companyScope=" + CompanyValue + "&locationType=" + Location + "&f_FG=" + Groups + "&countryCode=" + Country + "&company=" + Company + "&distance=" + within + "&f_N=" + Relationship + "&f_G=" + LocationArea + "&f_I=" + IndustryType + "&f_TE=" + YearsOfExperience + "&f_FA=" + Function + "&f_SE=" + SeniorLevel + "&f_P=" + IntrestedIn + "&f_CS=" + CompanySize + "&f_F=" + Fortune1000 + "&f_DR=" + RecentlyJoined + "&orig=ADVS&page_num=" + i + "";
                                PostResponce = HttpHelper.postFormData(new Uri(PostRequestURL), PostdataForPagination);
                                //Temporosy code for client
                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Pagesource 4 >>>> " + PostResponce, Globals.Path_InBoardProGetDataPagesource);
                            }
                            else
                            {
                                try
                                {
                                    PostRequestURL = "http://www.linkedin.com/search/hits";
                                    PostdataForPagination = "keywords=" + Uri.EscapeDataString(SearchCriteria.Keyword) + "&title=" + Uri.EscapeDataString(Title) + "&fname=" + FirstName + "&lname=" + LastName + "&searchLocationType=" + Location + "&f_FG=" + Groups + "&countryCode=" + Country + "&viewCriteria=1&sortCriteria=R&facetsOrder=CC%2CN%2CG%2CI%2CPC%2CED%2CL%2CFG%2CTE%2CFA%2CSE%2CP%2CCS%2CF%2CDR&page_num=" + i + "&openFacets=N%2CCC%2CG";
                                    PostResponce = HttpHelper.postFormData(new Uri(PostRequestURL), PostdataForPagination);
                                }
                                catch { }
                            }
                            clsDBQueryManager QM = new clsDBQueryManager();
                            if (PostResponce.Contains("/profile/view?id"))
                            {

                                List<string> PageSerchUrl = GettingAllUrl(PostResponce);
                                PageSerchUrl.Distinct();

                                if (PageSerchUrl.Count == 0)
                                {
                                    AddToLogger("[ " + DateTime.Now + " ] => [ On the basis of your Account you can able to see " + RecordURL.Count + " Results ]");
                                    break;
                                }

                                foreach (string item in PageSerchUrl)
                                {
                                    if (SearchCriteria.starter)
                                    {
                                        if (item.Contains("pp_profile_photo_link") || item.Contains("vsrp_people_res_name") || item.Contains("profile/view?"))
                                        {
                                            try
                                            {
                                                string urlSerch = item;
                                                if (urlSerch.Contains("vsrp_people_res_name"))
                                                {
                                                    RecordURL.Add(urlSerch);
                                                    try
                                                    {
                                                        string query = "Insert Into tb_CampaignScraperURL (Url, Account, Status) Values ('" + urlSerch + "','" + Account + "','" + "Not Scraped');";
                                                        QM.InsertUrl(query);
                                                    }
                                                    catch
                                                    { }

                                                    RecordURL = RecordURL.Distinct().ToList();
                                                }

                                                try
                                                {
                                                    AddToLogger("[ " + DateTime.Now + " ] => [ " + urlSerch + " ]");
                                                }
                                                catch { }
                                            }
                                            catch { }
                                        }
                                    }
                                }
                            }

                            else if (!PostResponce.Contains("pp_profile_name_link") && PostResponce.Contains("Custom views are no longer supported. Please select either Basic or Expanded view"))
                            {
                                break;
                            }

                            #endregion
                        }
                    }
                    finalUrlCollection(RecordURL, ref HttpHelper, FileName);

                }
                #region For Else
                else
                {
                    if (SearchCriteria.starter)
                    {

                        #region loop
                        if (ResponseWallPostForPremiumAcc.Contains("/profile/view?id"))
                        {

                            List<string> PageSerchUrl = ChilkatBasedRegex.GettingAllUrls(ResponseWallPostForPremiumAcc, "profile/view?id");
                            if (PageSerchUrl.Count == 0)
                            {

                                AddToLogger("[ " + DateTime.Now + " ] => [ On the basis of your Account or Your Input you can able to see " + RecordURL.Count + "  Results ]");

                            }

                            foreach (string item in PageSerchUrl)
                            {
                                if (SearchCriteria.starter)
                                {
                                    if (item.Contains("pp_profile_name_link"))
                                    {
                                        string urlSerch = "http://www.linkedin.com" + item;
                                        AddToLogger("[ " + DateTime.Now + " ] => [ " + urlSerch + " ]");
                                        RecordURL.Add(urlSerch);
                                        queRecordUrl.Enqueue(urlSerch);

                                    }
                                }
                            }
                        }
                        #endregion
                    }

                }

                //if (strPageNumber != string.Empty)
                //{
                //    if (strPageNumber != "0")
                //    {
                //        AddToLogger("-------------------------------------------------------------------------------------------------------------------------------");
                //        AddToLogger("[ " + DateTime.Now + " ] => [ No Of Results Found >>> " + strPageNumber + " ]");
                //    }
                //}

                RecordURL.Distinct();
               
            }

                #endregion

            catch { }
            #endregion
        }



        #region GettingAllUrl
        public List<string> GettingAllUrl(string PageSource)
        {
            List<string> suburllist = new List<string>();

            try
            {

                if (PageSource.Contains("/profile/view?"))
                {
                    string[] trkArr = Regex.Split(PageSource, "/profile/view?");

                    foreach (string item in trkArr)
                    {
                        try
                        {
                            if (item.Contains("vsrp_people_res_name"))
                            {
                                string url = item.Substring(0, item.IndexOf("\"")).Replace("\"", string.Empty).Replace("vsrp_companies_res_sim", "vsrp_companies_res_name").Trim();
                                string finalurl = "http://www.linkedin.com/profile/view" + url;
                                suburllist.Add(finalurl);
                            }
                        }
                        catch
                        {
                        }
                    }

                }

            }
            catch
            {
            }
            suburllist = suburllist.Distinct().ToList();
            return suburllist.Distinct().ToList();
        }
        #endregion

        #region finalUrlCollection
        private void finalUrlCollection(List<string> RecordURL, ref GlobusHttpHelper HttpHelper,string FileName)
        {
            try
            {
                List<string> numburlpp = new List<string>();

                if (SearchCriteria.starter)
                {
                    RecordURL = RecordURL.Distinct().ToList();

                    AddToLogger("[ " + DateTime.Now + " ] => [ Total Find URL >>> " + RecordURL.Count.ToString() + " ]");
                    AddToLogger("-------------------------------------------------------------------------------------------------------------------------------");
                    Thread.Sleep(1 * 10 * 1000);
                    foreach (string item in RecordURL)
                    {

                        try
                        {
                            if (item.Contains("pp_profile_name_link"))
                            {
                                if (SearchCriteria.starter)
                                {
                                    string urltemp = item;
                                    numburlpp.Add(urltemp);

                                    AddToLogger("[ " + DateTime.Now + " ] => [ " + urltemp + " ]");

                                    AddToLogger("[ " + DateTime.Now + " ] => [ Fetching Data From URL ]");

                                    if (!CrawlingLinkedInPage(urltemp, ref HttpHelper, FileName))
                                    {
                                       //CrawlingPageDataSource(urltemp, ref HttpHelper);
                                    }
                                }

                            }
                            else if (item.Contains("/profile/view?"))
                            {
                                string urltemp = item;
                                numburlpp.Add(urltemp);


                                AddToLogger("[ " + DateTime.Now + " ] => [ " + urltemp + " ]");

                                AddToLogger("[ " + DateTime.Now + " ] => [ Fetching Data From URL ]");

                                if (!CrawlingLinkedInPage(urltemp, ref HttpHelper, FileName))
                                {
                                   // CrawlingPageDataSource(urltemp, ref HttpHelper);
                                }

                            }
                        }
                        catch
                        {
                        }


                    }


                    AddToLogger("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                    AddToLogger("-----------------------------------------------------------------------------------------------------------------------------------");
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion


        #region CrawlingLinkedInPage
        public bool CrawlingLinkedInPage(string Url, ref GlobusHttpHelper HttpHelper,string FileName)
        {
            //Url = "http://www.linkedin.com/profile/view?id=44952194&authType=OUT_OF_NETWORK&authToken=DYBR&locale=en_US&srchid=3817933251416230963594&srchindex=2&srchtotal=1949&trk=vsrp_people_res_name&trkInfo=VSRPsearchId%3A3817933251416230963594%2CVSRPtargetId%3A44952194%2CVSRPcmpt%3Aprimary";

            #region Data Initialization
            string GroupMemId = string.Empty;
            string Industry = string.Empty;
            string URLprofile = string.Empty;
            string firstname = string.Empty;
            string lastname = string.Empty;
            string location = string.Empty;
            string country = string.Empty;
            string postal = string.Empty;
            string phone = string.Empty;
            string USERemail = string.Empty;
            string code = string.Empty;
            string education1 = string.Empty;
            string education2 = string.Empty;
            string titlecurrent = string.Empty;
            string companycurrent = string.Empty;
            string CurrentCompUrl = string.Empty;
            string CurrentCompSite = string.Empty;
            string titlepast1 = string.Empty;
            string companypast1 = string.Empty;
            string titlepast2 = string.Empty;
            string html = string.Empty;
            string companypast2 = string.Empty;
            string titlepast3 = string.Empty;
            string companypast3 = string.Empty;
            string titlepast4 = string.Empty;
            string companypast4 = string.Empty;
            string Recommendations = string.Empty;
            string Connection = string.Empty;
            string Designation = string.Empty;
            string Website = string.Empty;
            string Contactsettings = string.Empty;
            string recomandation = string.Empty;

            string titleCurrenttitle = string.Empty;
            string titleCurrenttitle2 = string.Empty;
            string titleCurrenttitle3 = string.Empty;
            string titleCurrenttitle4 = string.Empty;
            string Skill = string.Empty;
            string TypeOfProfile = "Public";
            List<string> EducationList = new List<string>();
            string Finaldata = string.Empty;
            string EducationCollection = string.Empty;
            List<string> checkerlst = new List<string>();
            List<string> checkgrplist = new List<string>();
            string groupscollectin = string.Empty;
            string strFamilyName = string.Empty;
            string LDS_LoginID = string.Empty;
            string LDS_Websites = string.Empty;
            string LDS_UserProfileLink = string.Empty;
            string LDS_CurrentTitle = string.Empty;
            string LDS_Experience = string.Empty;
            string LDS_UserContact = string.Empty;
            string LDS_PastTitles = string.Empty;
            string LDS_BackGround_Summary = string.Empty;
            string LDS_Desc_AllComp = string.Empty;
            string Company = string.Empty;
            List<string> lstpasttitle = new List<string>();
            List<string> checkpasttitle = new List<string>();
            string DeegreeConn = string.Empty;
            string AccountType = string.Empty;
            bool CheckEmployeeScraper = false;
            string fileName = string.Empty;
            bool CampaignScraper = false;
            #endregion

            #region GetRequest
            if (Url.Contains("CompanyEmployeeScraper"))
            {
                try
                {
                    Url = Url.Replace("CompanyEmployeeScraper", string.Empty);
                    CheckEmployeeScraper = true;
                }
                catch
                { }
            }

            if (Url.Contains("CampaignScraper"))
            {
                try
                {
                    string[] Url_Split = Regex.Split(Url, "CampaignScraper");
                    Url = Url_Split[0];
                    fileName = Url_Split[1];
                    CampaignScraper = true;
                }
                catch
                { }
            }

            string stringSource = HttpHelper.getHtmlfromUrl(new Uri(Url.Replace("http", "https")));
            #endregion

            #region GroupMemId
            try
            {
                string[] gid = Url.Split('&');
                GroupMemId = gid[0].Replace("http://www.linkedin.com/profile/view?id=", string.Empty);
            }
            catch { }
            #endregion

            #region Name
            try
            {
                try
                {
                    strFamilyName = stringSource.Substring(stringSource.IndexOf("\"See Full Name\",\"i18n_Edit\":\"Edit\",\"fmt__full_name\":\""), (stringSource.IndexOf("i18n__expand_your_network_to_see_more", stringSource.IndexOf("\"See Full Name\",\"i18n_Edit\":\"Edit\",\"fmt__full_name\":\"")) - stringSource.IndexOf("\"See Full Name\",\"i18n_Edit\":\"Edit\",\"fmt__full_name\":\""))).Replace("\"See Full Name\",\"i18n_Edit\":\"Edit\",\"fmt__full_name\":\"", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Trim();
                }
                catch
                {
                    try
                    {
                        strFamilyName = stringSource.Substring(stringSource.IndexOf("fmt__full_name\":"), (stringSource.IndexOf(",", stringSource.IndexOf("fmt__full_name\":")) - stringSource.IndexOf("fmt__full_name\":"))).Replace("fmt__full_name\":", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Trim();

                    }
                    catch { }
                }

                if (string.IsNullOrEmpty(strFamilyName))
                {
                    try
                    {
                        strFamilyName = stringSource.Substring(stringSource.IndexOf("<span class=\"full-name\">"), (stringSource.IndexOf("</span><span></span></span></h1></div></div><div id=\"headline-container\" data-li-template=\"headline\">", stringSource.IndexOf("</span><span></span></span></h1></div></div><div id=\"headline-container\" data-li-template=\"headline\">")) - stringSource.IndexOf("<span class=\"full-name\">"))).Replace("<span class=\"full-name\">", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Trim();
                    }
                    catch
                    { }
                }

                if (string.IsNullOrEmpty(strFamilyName))
                {
                    try
                    {
                        int StartIndex = stringSource.IndexOf("profileFullName");
                        string Start = stringSource.Substring(StartIndex).Replace("profileFullName", string.Empty);
                        int EndIndex = Start.IndexOf("slideData");
                        string End = Start.Substring(0, EndIndex).Replace(":", string.Empty).Replace("'",string.Empty).Replace(",",string.Empty).Trim();
                        strFamilyName = End.Trim();
                    }
                    catch
                    { }
                }
            }
            catch { }

            if (string.IsNullOrEmpty(strFamilyName))
            {
                try
                {
                    int StartIndex = stringSource.IndexOf("</script><title>");
                    string Start = stringSource.Substring(StartIndex).Replace("</script><title>", string.Empty);
                    int EndIndex = Start.IndexOf("| LinkedIn</title>");
                    string End = Start.Substring(0, EndIndex).Replace(":", string.Empty).Replace("'", string.Empty).Replace(",", string.Empty).Trim();
                    strFamilyName = End.Trim();
                }
                catch
                { }
            }

            //

            #endregion

            #region Namesplitation
            string[] NameArr = new string[5];
            if (strFamilyName.Contains(" "))
            {
                try
                {
                    NameArr = Regex.Split(strFamilyName, " ");
                }
                catch { }
            }
            #endregion

            #region FirstName
            try
            {
                firstname = NameArr[0];
            }
            catch { }
            #endregion

            #region LastName

            try
            {
                lastname = NameArr[1];
            }
            catch { }

            try
            {
                if (NameArr.Count() == 3)
                {
                    try
                    {
                        lastname = NameArr[1] + " " + NameArr[2];
                    }
                    catch { }
                }

                if (lastname.Contains("}]"))
                {

                    #region Name
                    try
                    {
                        try
                        {
                            strFamilyName = stringSource.Substring(stringSource.IndexOf("<span class=\"n fn\">")).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Trim();
                        }
                        catch
                        {
                            try
                            {
                                strFamilyName = stringSource.Substring(stringSource.IndexOf("fmt__full_name\":"), (stringSource.IndexOf(",", stringSource.IndexOf("fmt__full_name\":")) - stringSource.IndexOf("fmt__full_name\":"))).Replace("fmt__full_name\":", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Trim();

                            }
                            catch { }
                        }
                    }
                    catch { }
                    #endregion
                }
            }
            catch { }
            #endregion

            #region Company
            try
            {
                try
                {
                    try
                    {
                        //soft engg at TCSi18n__LocationLocationi18n__Linkedin_memberLinkedIn Member
                        //Company = stringSource.Substring(stringSource.IndexOf("\"memberHeadline"), (stringSource.IndexOf("memberID", stringSource.IndexOf("\"memberHeadline")) - stringSource.IndexOf("\"memberHeadline"))).Replace("\"memberHeadline", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Replace("   ", string.Empty).Replace("visibletrue", string.Empty).Replace("isLNLedtrue",string.Empty).Replace("isPortfoliofalse",string.Empty).Replace("i18n__Location",string.Empty).Replace("Locationi18n__Linkedin_member",string.Empty).Replace("u002d","-").Replace("LinkedIn Member",string.Empty).Replace("--Location","--").Trim();
                        Company = stringSource.Substring(stringSource.IndexOf("\"memberHeadline"), (stringSource.IndexOf("memberID", stringSource.IndexOf("\"memberHeadline")) - stringSource.IndexOf("\"memberHeadline"))).Replace("\"memberHeadline", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Replace("   ", string.Empty).Replace("visibletrue", string.Empty).Replace("isLNLedtrue", string.Empty).Replace("isPortfoliofalse", string.Empty).Replace("i18n__Location", string.Empty).Replace("Locationi18n__Linkedin_member", string.Empty).Replace("u002d", "-").Replace("LinkedIn Member", string.Empty).Replace("--Location", "--").ToString().Trim();
                        if (Company.Contains("#Name?"))
                        {
                            Company = "--";
                        }
                        if (Company.Contains("i18n"))
                        {
                            Company = Regex.Split(Company, "i18n")[0];
                        }

                    }
                    catch
                    {
                    }

                    if (string.IsNullOrEmpty(Company))
                    {
                        try
                        {
                            Company = stringSource.Substring(stringSource.IndexOf("memberHeadline\":"), (stringSource.IndexOf(",", stringSource.IndexOf("memberHeadline\":")) - stringSource.IndexOf("memberHeadline\":"))).Replace("memberHeadline\":", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Replace("   ", string.Empty).Replace(":", "").Replace("visibletrue", string.Empty).Replace("&dsh;", "").Replace("&amp", "").Replace(";", "").Replace("isLNLedtrue", string.Empty).Replace("isPortfoliofalse", string.Empty).Trim();
                        }
                        catch { }

                    }

                    if (string.IsNullOrEmpty(Company))
                    {
                        try
                        {
                            Company = stringSource.Substring(stringSource.IndexOf("class=\"title \">"), (stringSource.IndexOf("</p></div></div><div class=\"demographic-info adr editable-item\" id=\"demographics\">", stringSource.IndexOf("</p></div></div><div class=\"demographic-info adr editable-item\" id=\"demographics\">")) - stringSource.IndexOf("class=\"title \">"))).Replace("class=\"title \">", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace("<strong class=highlight>", string.Empty).Replace("</strong>", string.Empty).Trim();
                        }
                        catch { }
                    }

                    string[] strdesigandcompany = new string[4];
                    if (Company.Contains(" at "))
                    {
                        try
                        {
                            strdesigandcompany = Regex.Split(Company, " at ");
                        }
                        catch { }

                        #region Title
                        try
                        {
                            titlecurrent = strdesigandcompany[0];
                        }
                        catch { }
                        #endregion

                        #region Current Company
                        try
                        {
                            companycurrent = strdesigandcompany[1];
                        }
                        catch { }
                        #endregion
                    }
                }
                catch { }

                #region Current Company Site
                try
                {
                    try
                    {
                        CurrentCompUrl = stringSource.Substring(stringSource.IndexOf("/companies"), (stringSource.IndexOf("/companies", stringSource.IndexOf("?miniprofile")) - stringSource.IndexOf("?miniprofile"))).Replace("?miniprofi", string.Empty).ToString().Trim();
                        CurrentCompUrl = "https://www.linkedin.com" + CurrentCompUrl;
                    }
                    catch { }

                    string CompanyUrl = HttpHelper.getHtmlfromUrl1(new Uri(CurrentCompUrl));


                    try
                    {
                        CurrentCompSite = CompanyUrl.Substring(CompanyUrl.IndexOf("<dt>Website</dt>"), (CompanyUrl.IndexOf("</dd>", CompanyUrl.IndexOf("<dt>Website</dt>")) - CompanyUrl.IndexOf("<dt>Website</dt>"))).Replace("<dt>Website</dt>", string.Empty).Replace("<dd>", string.Empty).Trim();
                    }
                    catch { }

                    try
                    {
                        CurrentCompSite = CompanyUrl.Substring(CompanyUrl.IndexOf("<h4>Website</h4>"), (CompanyUrl.IndexOf("</p>", CompanyUrl.IndexOf("<h4>Website</h4>")) - CompanyUrl.IndexOf("<h4>Website</h4>"))).Replace("<h4>Website</h4>", string.Empty).Replace("<p>", string.Empty).Trim();

                        if (CurrentCompSite.Contains("a href="))
                        {
                            try
                            {
                                string[] websArr = Regex.Split(CurrentCompSite, ">");
                                CurrentCompSite = websArr[1].Replace("</a", string.Empty).Replace("\n", string.Empty).Trim(); ;
                            }
                            catch { }
                        }
                    }
                    catch { }
                }
                catch { }

                #endregion

                #region PastCompany
                string[] companylist = Regex.Split(stringSource, "companyName\"");
                if (companylist.Count() == 1)
                {
                    companylist = Regex.Split(stringSource, "company-name");
                }
                if (companylist.Count() == 1)
                {
                    //companylist = Regex.Split(stringSource, "Companies");
                }

                string AllComapny = string.Empty;

                string Companyname = string.Empty;
                if (!stringSource.Contains("company-name") && companylist.Count() > 1)
                {
                    foreach (string item in companylist)
                    {
                        try
                        {
                            if (!item.Contains("<!DOCTYPE html>"))
                            {
                                Companyname = item.Substring(item.IndexOf(":"), (item.IndexOf(",", item.IndexOf(":")) - item.IndexOf(":"))).Replace(":", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Replace("}", string.Empty).Replace("]", string.Empty).Trim();
                                string items = item;
                                checkerlst.Add(Companyname);
                                checkerlst = checkerlst.Distinct().ToList();
                            }
                        }
                        catch { }
                    }
                }
                else
                {
                    foreach (string item in companylist)
                    {
                        try
                        {
                            if (!item.Contains("<!DOCTYPE html>"))
                            {
                                Companyname = item.Substring(item.IndexOf(">"), (item.IndexOf("<", item.IndexOf(">")) - item.IndexOf(">"))).Replace(">", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Replace("}", string.Empty).Replace("]", string.Empty).Trim();
                                string items = item;
                                if (!string.IsNullOrEmpty(Companyname))
                                {
                                    checkerlst.Add(Companyname);
                                    checkerlst = checkerlst.Distinct().ToList();
                                }
                            }
                        }
                        catch { }
                    }
                }
                AllComapny = string.Empty;
                foreach (string item1 in checkerlst)
                {
                    if (string.IsNullOrEmpty(AllComapny))
                    {
                        AllComapny = item1.Replace("}", "").Replace("]", "").Replace("&amp;", "&");
                    }
                    else
                    {
                        AllComapny = AllComapny + " : " + item1.Replace("}", "").Replace("]", "").Replace("&amp;", "&");
                    }
                }
                #endregion

            #endregion Company

                #region Company Descripription

                try
                {
                    string[] str_CompanyDesc = Regex.Split(stringSource, "showSummarySection");

                    foreach (string item in str_CompanyDesc)
                    {
                        try
                        {
                            string Current_Company = string.Empty;
                            if (!item.Contains("<!DOCTYPE html>"))
                            {
                                int startindex = item.IndexOf("specialties\":\"");

                                if (startindex > 0)
                                {
                                    try
                                    {
                                        string start = item.Substring(startindex).Replace("specialties\":", "");
                                        int endindex = start.IndexOf("\",\"associatedWith\"");
                                        string end = start.Substring(0, endindex);
                                        Current_Company = end.Replace(",\"specialties_lb\":", string.Empty).Replace("\"", string.Empty).Replace("summary_lb", "Summary").Replace("&#x2022;", ";").Replace("<br>", string.Empty).Replace("\\n", string.Empty).Replace("\"u002", "-");
                                        LDS_BackGround_Summary = Current_Company;
                                    }
                                    catch { }
                                }

                            }

                            if (!item.Contains("<!DOCTYPE html>"))
                            {
                                int startindex = item.IndexOf("\"summary_lb\"");

                                if (startindex > 0)
                                {
                                    try
                                    {
                                        string start = item.Substring(startindex).Replace("\"summary_lb\"", "");
                                        int endindex = start.IndexOf("\",\"associatedWith\"");
                                        string end = start.Substring(0, endindex);
                                        Current_Company = end.Replace(",\"specialties_lb\":", string.Empty).Replace("<br>", string.Empty).Replace("\n", string.Empty).Replace("\"", string.Empty).Replace("summary_lb", "Summary").Replace(",", ";").Replace("u002", "-").Replace("&#x2022;", string.Empty).Replace(":", string.Empty);
                                        LDS_BackGround_Summary = Current_Company;
                                    }
                                    catch { }
                                }

                            }

                        }
                        catch { }
                    }

                    if (string.IsNullOrEmpty(LDS_BackGround_Summary))
                    {
                        try
                        {
                            LDS_BackGround_Summary = HttpHelper.GetDataWithTagValueByTagAndAttributeNameWithId(stringSource, "div", "summary-item-view");
                            LDS_BackGround_Summary = Regex.Replace(LDS_BackGround_Summary, "<.*?>", string.Empty).Replace(",", "").Replace("\n", "").Replace("<![CDATA[", "").Trim();
                        }
                        catch { }
                    }
                }
                catch { }

                #endregion

                #region Education
                try
                {
                    string[] str_UniversityName = Regex.Split(stringSource, "link__school_name");
                    foreach (string item in str_UniversityName)
                    {
                        try
                        {
                            string School = string.Empty;
                            string Degree = string.Empty;
                            string SessionEnd = string.Empty;
                            string SessionStart = string.Empty;
                            string Education = string.Empty;
                            if (stringSource.Contains("link__school_name"))
                            {
                                if (!item.Contains("<!DOCTYPE html>"))
                                {
                                    try
                                    {
                                        try
                                        {
                                            int startindex = item.IndexOf("fmt__school_highlight");
                                            string start = item.Substring(startindex).Replace("fmt__school_highlight", "");
                                            int endindex = start.IndexOf(",");
                                            School = start.Substring(0, endindex).Replace("\\u002d", string.Empty).Replace(":", string.Empty).Replace("\"", string.Empty).Replace("_highlight", string.Empty);
                                        }
                                        catch { }

                                        try
                                        {
                                            int startindex1 = item.IndexOf("degree");
                                            string start1 = item.Substring(startindex1).Replace("degree", "");
                                            int endindex1 = start1.IndexOf(",");
                                            Degree = start1.Substring(0, endindex1).Replace("\\u002d", string.Empty).Replace(":", string.Empty).Replace("\"", string.Empty).Replace("_highlight", string.Empty);
                                        }
                                        catch { }

                                        try
                                        {
                                            int startindex2 = item.IndexOf("enddate_my");
                                            string start2 = item.Substring(startindex2).Replace("enddate_my", "");
                                            int endindex2 = start2.IndexOf(",");
                                            SessionEnd = start2.Substring(0, endindex2).Replace("\\u002d", string.Empty).Replace(":", string.Empty).Replace("\"", string.Empty).Replace("_highlight", string.Empty);
                                        }
                                        catch { }

                                        try
                                        {
                                            int startindex3 = item.IndexOf("startdate_my");
                                            string start3 = item.Substring(startindex3).Replace("startdate_my", "");
                                            int endindex3 = start3.IndexOf(",");
                                            SessionStart = start3.Substring(0, endindex3).Replace("\\u002d", string.Empty).Replace(":", string.Empty).Replace("\"", string.Empty);
                                        }
                                        catch { }

                                        if (SessionStart == string.Empty && SessionEnd == string.Empty)
                                        {
                                            Education = " [" + School + "] Degree: " + Degree;
                                        }
                                        else
                                        {
                                            Education = " [" + School + "] Degree: " + Degree + " Session: " + SessionStart + "-" + SessionEnd;
                                        }
                                        //University = item.Substring(item.IndexOf(":"), (item.IndexOf(",", item.IndexOf(":")) - item.IndexOf(":"))).Replace(":", string.Empty).Replace("\\u002d", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Trim();
                                    }
                                    catch { }
                                    EducationList.Add(Education);

                                }
                            }
                            else
                            {

                                str_UniversityName = Regex.Split(stringSource, "<div class=\"education");
                                foreach (string tempItem in str_UniversityName)
                                {
                                    try
                                    {
                                        if (!tempItem.Contains("<!DOCTYPE html>"))
                                        {
                                            List<string> lstSchool = HttpHelper.GetTextDataByTagAndAttributeName(tempItem, "h4", "summary fn org");
                                            List<string> lstDegree = HttpHelper.GetTextDataByTagAndAttributeName(tempItem, "span", "degree");
                                            List<string> lstSession = HttpHelper.GetTextDataByTagAndAttributeName(tempItem, "span", "education-date");

                                            if (lstSession.Count == 0)
                                            {
                                                Education = " [" + lstSchool[0] + "] Degree: " + lstDegree[0];
                                            }
                                            else
                                            {
                                                Education = " [" + lstSchool[0] + "] Degree: " + lstDegree[0] + " Session: " + lstSession[0].Replace("&#8211;", "-").Replace(",", "").Trim();
                                            }

                                            EducationList.Add(Education);
                                        }
                                    }
                                    catch { }
                                }
                            }
                        }
                        catch { }

                    }

                    EducationList = EducationList.Distinct().ToList();

                    foreach (string item in EducationList)
                    {
                        if (string.IsNullOrEmpty(EducationCollection))
                        {
                            EducationCollection = item.Replace("}", "").Replace("]", "").Replace("&amp;", "&");
                        }
                        else
                        {
                            EducationCollection = EducationCollection + "  -  " + item.Replace("}", "").Replace("]", "").Replace("&amp;", "&");
                        }
                    }
                    // string University1 = stringSource.Substring(stringSource.IndexOf("schoolName\":"), (stringSource.IndexOf(",", stringSource.IndexOf("schoolName\":")) - stringSource.IndexOf("schoolName\":"))).Replace("schoolName\":", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Trim();

                }

                catch { }

                #endregion Education

                #region Email
                try
                {
                    if (stringSource.Contains("mailto:"))
                    {
                        string[] str_Email = Regex.Split(stringSource, "mailto:");
                        USERemail = stringSource.Substring(stringSource.IndexOf("mailto:"), (stringSource.IndexOf(">", stringSource.IndexOf("mailto:")) - stringSource.IndexOf("mailto:"))).Replace("mailto:", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Trim();
                    }
                    else
                    {
                        string[] str_Email = Regex.Split(stringSource, "email\"");
                        USERemail = stringSource.Substring(stringSource.IndexOf("[{\"email\":"), (stringSource.IndexOf("}]", stringSource.IndexOf("[{\"email\":")) - stringSource.IndexOf("[{\"email\":"))).Replace("[{\"email\":", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Trim();

                    }
                }
                catch (Exception ex)
                {

                }
                #endregion Email

                #region UserContact
                try
                {
                    if (stringSource.Contains("<div id=\"phone-view\">"))
                    {
                        //string[] str_Contact = Regex.Split(stringSource, "<div id=\"phone-view\">");
                        LDS_UserContact = stringSource.Substring(stringSource.IndexOf("<div id=\"phone-view\">"), (stringSource.IndexOf("</li>", stringSource.IndexOf("<div id=\"phone-view\">")) - stringSource.IndexOf("<div id=\"phone-view\">"))).Replace("<div id=\"phone-view\">", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace("<ul><li>", string.Empty).Replace("&nbsp;", "").Trim();
                    }
                    else
                    {
                        //string[] str_Email = Regex.Split(stringSource, "<div id=\"phone-view\">");
                        LDS_UserContact = stringSource.Substring(stringSource.IndexOf("<div id=\"phone-view\">"), (stringSource.IndexOf("</li>", stringSource.IndexOf("<div id=\"phone-view\">")) - stringSource.IndexOf("<div id=\"phone-view\">"))).Replace("<div id=\"phone-view\">", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace("<ul><li>", string.Empty).Replace("&nbsp;", "").Trim();

                    }
                }
                catch (Exception ex)
                {

                }
                #endregion Email

                #region Website
                try
                {
                    Website = stringSource.Substring(stringSource.IndexOf("[{\"URL\":"), (stringSource.IndexOf(",", stringSource.IndexOf("[{\"URL\":")) - stringSource.IndexOf("[{\"URL\":"))).Replace("[{\"URL\":", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace("}", string.Empty).Replace("]", string.Empty).Trim();
                }
                catch { }
                if (string.IsNullOrEmpty(Website))
                {
                    try
                    {

                        Website = HttpHelper.GetDataWithTagValueByTagAndAttributeNameWithId(stringSource, "div", "website-view");
                        Website = Regex.Replace(Website, "<[^>]*>", String.Empty).Replace("\n", "").Replace("\r", "").Replace(" ", " ").Trim();
                        Website = Regex.Replace(Website, @"\s+", " ").Replace(",", " ").Trim();
                    }
                    catch { }
                }
                #endregion Website

                #region location
                try
                {
                    //location = stringSource.Substring(stringSource.IndexOf("Country\",\"fmt__location\":"), (stringSource.IndexOf("i18n_no_location_matches", stringSource.IndexOf("Country\",\"fmt__location\":")) - stringSource.IndexOf("Country\",\"fmt__location\":"))).Replace("Country\",\"fmt__location\":", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Trim();
                    int startindex = stringSource.IndexOf("fmt_location");
                    string start = stringSource.Substring(startindex).Replace("fmt_location\":\"", "");
                    int endindex = start.IndexOf("\"");
                    string end = start.Substring(0, endindex).Replace("\u002d", string.Empty);
                    location = end;
                }
                catch (Exception ex)
                {

                }
                if (string.IsNullOrEmpty(location))
                {
                    try
                    {
                        List<string> lstLocation = HttpHelper.GetTextDataByTagAndAttributeName(stringSource, "span", "locality");
                        if (lstLocation.Count > 0)
                        {
                            location = lstLocation[lstLocation.Count - 1].Trim();
                        }
                    }
                    catch { }
                }
                #endregion location

                #region Country
                try
                {
                    int startindex = stringSource.IndexOf("\"geo_region\":");
                    if (startindex > 0)
                    {
                        string start = stringSource.Substring(startindex).Replace("\"geo_region\":", "");
                        int endindex = start.IndexOf("\"i18n_geo_region\":\"Location\"");
                        string end = start.Substring(0, endindex);
                        country = end;

                        string[] array = Regex.Split(end, "\"name\":\"");
                        array = array.Skip(1).ToArray();
                        foreach (string item in array)
                        {
                            try
                            {
                                int startindex1 = item.IndexOf("\",\"");
                                string strat1 = item.Substring(0, startindex1);
                                country = strat1;
                                break;
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
                if (country == string.Empty)
                {
                    try
                    {
                        string[] countLocation = location.Split(',');

                        if (countLocation.Count() == 2)
                        {
                            country = location.Split(',')[1];
                        }
                        else if (countLocation.Count() == 3)
                        {
                            country = location.Split(',')[2];
                        }


                    }
                    catch { }

                }
                #endregion

                #region Industry
                try
                {
                    //Industry = stringSource.Substring(stringSource.IndexOf("fmt__industry_highlight\":"), (stringSource.IndexOf(",", stringSource.IndexOf("fmt__industry_highlight\":")) - stringSource.IndexOf("fmt__industry_highlight\":"))).Replace("fmt__industry_highlight\":", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Trim();
                    int startindex = stringSource.IndexOf("\"industry_highlight\":\"");
                    if (startindex > 0)
                    {
                        string start = stringSource.Substring(startindex).Replace("\"industry_highlight\":\"", "");
                        int endindex = start.IndexOf("\",");
                        string end = start.Substring(0, endindex).Replace("\"", string.Empty).Replace("</strong>", string.Empty).Replace("&amp;", "&");
                        if (end.Contains("strong class"))
                        {
                            Industry = end.Split('>')[1];
                        }
                        else
                        {
                            Industry = end;
                        }
                    }
                }
                catch (Exception ex)
                {
                }

                if (string.IsNullOrEmpty(Industry))
                {
                    try
                    {
                        List<string> lstIndustry = HttpHelper.GetTextDataByTagAndAttributeName(stringSource, "dd", "industry");
                        if (lstIndustry.Count > 0)
                        {
                            Industry = lstIndustry[0].Replace(",", ":").Trim();
                        }
                    }
                    catch { }
                }
                #endregion Industry

                #region Connection
                try
                {
                    //Connection = stringSource.Substring(stringSource.IndexOf("_count_string\":"), (stringSource.IndexOf(",", stringSource.IndexOf("_count_string\":")) - stringSource.IndexOf("_count_string\":"))).Replace("_count_string\":", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Trim();
                    int startindex = stringSource.IndexOf("\"numberOfConnections\":");
                    if (startindex > 0)
                    {
                        string start = stringSource.Substring(startindex).Replace("\"numberOfConnections\":", "");
                        int endindex = start.IndexOf(",");
                        string end = start.Substring(0, endindex).Replace("&amp;", "&").Replace("}", string.Empty);
                        Connection = end;
                    }
                }
                catch (Exception ex)
                {
                }

                if (string.IsNullOrEmpty(Connection))
                {
                    try
                    {
                        List<string> lstConnection = HttpHelper.GetTextDataByTagAndAttributeName(stringSource, "div", "member-connections");
                        if (lstConnection.Count > 0)
                        {
                            Connection = lstConnection[0].Replace(",", ":").Trim();
                        }
                    }
                    catch { }
                }
                #endregion Connection

                #region Recommendation


                try
                {
                    string RecomnedUrl = string.Empty;
                    try
                    {
                        int startindex = stringSource.IndexOf("endorsements?id=");
                        string start = stringSource.Substring(startindex);
                        int endIndex = start.IndexOf("\"mem_pic\":");
                        if (endIndex < 0)
                        {
                            endIndex = start.IndexOf(">");
                        }
                        RecomnedUrl = (start.Substring(0, endIndex).Replace(",", string.Empty).Replace("\"", string.Empty).Replace(":", string.Empty));

                    }
                    catch { }

                    string PageSource = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/profile/profile-v2-" + RecomnedUrl + ""));
                    string[] arrayRecommendedName = Regex.Split(PageSource, "headline");

                    if (arrayRecommendedName.Count() == 1)
                    {
                        arrayRecommendedName = Regex.Split(PageSource, "fmt__recommendeeFullName");
                    }


                    List<string> ListRecommendationName = new List<string>();

                    foreach (var itemRecomName in arrayRecommendedName)
                    {
                        try
                        {
                            if (!itemRecomName.Contains("Endorsements"))
                            {
                                string Heading = string.Empty;
                                string Name = string.Empty;

                                try
                                {

                                    int startindex = itemRecomName.IndexOf(":");
                                    string start = itemRecomName.Substring(startindex);
                                    int endIndex = start.IndexOf("\",");
                                    Heading = (start.Substring(0, endIndex).Replace("\"", string.Empty).Replace(":", string.Empty).Replace(",", ";"));
                                }
                                catch { }

                                try
                                {
                                    int startindex1 = itemRecomName.IndexOf("fmt__referrerfullName");
                                    string start1 = itemRecomName.Substring(startindex1);
                                    int endIndex1 = start1.IndexOf("\",");
                                    Name = (start1.Substring(0, endIndex1).Replace("\"", string.Empty).Replace("fmt__referrerfullName", string.Empty).Replace(":", string.Empty));
                                }
                                catch { }

                                if (Name == string.Empty)
                                {
                                    int startindex1 = itemRecomName.IndexOf("recommenderTitle\":");
                                    string start1 = itemRecomName.Substring(startindex1);
                                    int endIndex1 = start1.IndexOf("\",");
                                    Name = (start1.Substring(0, endIndex1).Replace("\"", string.Empty).Replace("recommenderTitle", string.Empty).Replace(":", string.Empty).Replace(",", string.Empty));
                                }

                                ListRecommendationName.Add(Name + " : " + Heading);

                            }
                        }
                        catch { }

                    }

                    foreach (var item in ListRecommendationName)
                    {
                        if (recomandation == string.Empty)
                        {
                            recomandation = item;
                        }
                        else
                        {
                            recomandation += "  -  " + item;
                        }
                    }

                }
                catch { }

                #endregion

                #region Following


                #endregion

                #region Experience
                if (LDS_Experience == string.Empty)
                {
                    try
                    {
                        string[] array = Regex.Split(stringSource, "title_highlight");
                        string exp = string.Empty;
                        string comp = string.Empty;
                        List<string> ListExperince = new List<string>();
                        string SelItem = string.Empty;
                        if (stringSource.Contains("title_highlight"))
                        {
                            foreach (var itemGrps in array)
                            {
                                try
                                {
                                    if (itemGrps.Contains("title_pivot") && !itemGrps.Contains("<!DOCTYPE html")) //">Join
                                    {
                                        try
                                        {

                                            int startindex = itemGrps.IndexOf("\":\"");
                                            string start = itemGrps.Substring(startindex);
                                            int endIndex = start.IndexOf(",");
                                            exp = (start.Substring(0, endIndex).Replace("\"", string.Empty).Replace(":", string.Empty).Replace("&amp", "&").Replace(";", string.Empty).Replace("\\u002d", "-").Replace("name:", string.Empty));

                                        }
                                        catch { }
                                        if ((exp.Contains("strong class")) || (exp.Contains("highlight")) || (string.IsNullOrEmpty(exp)))
                                        {
                                            try
                                            {
                                                int startindex1 = itemGrps.IndexOf("\"title\":");
                                                string start1 = itemGrps.Substring(startindex1).Replace("\"title\":", string.Empty);
                                                int endIndex1 = start1.IndexOf(",");
                                                exp = (start1.Substring(0, endIndex1).Replace("\"", string.Empty).Replace(":", string.Empty).Replace("&amp", "&").Replace(";", string.Empty).Replace("\\u002d", "-").Replace("name:", string.Empty));
                                            }
                                            catch
                                            { }
                                        }

                                        try
                                        {

                                            int startindex1 = itemGrps.IndexOf("companyName");
                                            string start1 = itemGrps.Substring(startindex1);
                                            int endIndex1 = start1.IndexOf(",");
                                            comp = (start1.Substring(0, endIndex1).Replace("\"", string.Empty).Replace("companyName", string.Empty).Replace(":", string.Empty).Replace(";", string.Empty).Replace("\\u002d", "-").Replace("name:", string.Empty));

                                        }
                                        catch { }

                                        if (titlecurrent == string.Empty)
                                        {
                                            titlecurrent = exp;
                                        }

                                        if (companycurrent == string.Empty)
                                        {
                                            companycurrent = comp;
                                        }

                                        ListExperince.Add(exp + ":" + comp);


                                    }
                                }
                                catch { }
                            }

                        }
                        else
                        {
                            array = Regex.Split(stringSource, "<header>");
                            foreach (string tempItem in array)
                            {
                                try
                                {
                                    if (!tempItem.Contains("<!DOCTYPE html>"))
                                    {

                                        List<string> lstExp = objChilkat.GetDataTag(tempItem, "h4");
                                        List<string> lstComp = objChilkat.GetDataTag(tempItem, "h5");
                                        if (lstExp.Count > 0)
                                        {
                                            exp = lstExp[0];
                                        }
                                        if (lstComp.Count > 0)
                                        {
                                            comp = lstComp[0];
                                            if (string.IsNullOrEmpty(comp))
                                            {
                                                comp = lstComp[1];
                                            }
                                        }
                                        if (titlecurrent == string.Empty)
                                        {
                                            titlecurrent = lstExp[0];
                                        }

                                        if (companycurrent == string.Empty)
                                        {
                                            companycurrent = lstComp[0];
                                        }

                                        ListExperince.Add(exp + ":" + comp);

                                    }
                                }
                                catch { }
                            }

                        }
                        foreach (var item in ListExperince)
                        {
                            if (LDS_Experience == string.Empty)
                            {
                                LDS_Experience = item;
                            }
                            else
                            {
                                LDS_Experience += "  -  " + item;
                            }
                        }

                    }

                    catch { }

                    try
                    {
                        if (string.IsNullOrEmpty(titlecurrent))
                        {
                            int StartIndex = stringSource.IndexOf("trk=prof-0-ovw-curr_pos\">");
                            string Start = stringSource.Substring(StartIndex).Replace("trk=prof-0-ovw-curr_pos\">", string.Empty);
                            int EndIndex = Start.IndexOf("</a>");
                            string End = Start.Substring(0, EndIndex).Replace("</a>", string.Empty);
                            titlecurrent = End.Trim();
                        }
                    }
                    catch
                    { }


                }

                #endregion

                #region Group

                try
                {
                    string GroupUrl = string.Empty;
                    try
                    {
                        int startindex = stringSource.IndexOf("templateId\":\"profile_v2_connections");
                        string start = stringSource.Substring(startindex);
                        //int endIndex = start.IndexOf("vsrp_people_res_name");
                        int endIndex = start.IndexOf("}");
                        GroupUrl = (start.Substring(0, endIndex).Replace(",", string.Empty).Replace("\"", string.Empty).Replace("templateId:profile_v2_connectionsurl:", string.Empty));

                    }
                    catch { }

                    string PageSource = HttpHelper.getHtmlfromUrl1(new Uri(GroupUrl));

                    string[] array1 = Regex.Split(PageSource, "groupRegistration?");
                    List<string> ListGroupName = new List<string>();
                    string SelItem = string.Empty;

                    foreach (var itemGrps in array1)
                    {
                        try
                        {
                            if (itemGrps.Contains("?gid=") && !itemGrps.Contains("<!DOCTYPE html")) //">Join
                            {
                                if (itemGrps.IndexOf("?gid=") == 0)
                                {
                                    try
                                    {
                                        int startindex = itemGrps.IndexOf("\"name\":");
                                        string start = itemGrps.Substring(startindex);
                                        int endIndex = start.IndexOf(",");
                                        ListGroupName.Add(start.Substring(0, endIndex).Replace("\"", string.Empty).Replace("amp", string.Empty).Replace("&", string.Empty).Replace(";", string.Empty).Replace("csrfToken", string.Empty).Replace("name:", string.Empty));
                                    }
                                    catch { }
                                }
                            }
                        }
                        catch { }
                    }

                    foreach (var item in ListGroupName)
                    {
                        if (groupscollectin == string.Empty)
                        {
                            groupscollectin = item;
                        }
                        else
                        {
                            groupscollectin += "  -  " + item;
                        }
                    }

                }
                catch { }

                if (string.IsNullOrEmpty(groupscollectin))
                {
                    List<string> lstGroupData = new List<string>();
                    string[] array1 = Regex.Split(stringSource, "link_groups_settings\":?");
                    array1 = array1.Skip(1).ToArray();
                    foreach (string item in array1)
                    {
                        string _item = Utils.getBetween(item, "name\":", ",").Replace("name\":", "").Replace(":", "").Replace("\"", "");
                        lstGroupData.Add(_item);
                    }

                    foreach (var item in lstGroupData)
                    {
                        if (groupscollectin == string.Empty)
                        {
                            groupscollectin = item;
                        }
                        else
                        {
                            groupscollectin += "  -  " + item;
                        }
                    }
                }

                if (string.IsNullOrEmpty(groupscollectin))
                {
                    List<string> lstGroupData = new List<string>();
                    string tempResponse = Utils.getBetween(stringSource, "<div id=\"groups\"", "<div>");
                    lstGroupData = HttpHelper.GetDataTag(tempResponse, "strong");

                    foreach (var item in lstGroupData)
                    {
                        if (groupscollectin == string.Empty)
                        {
                            groupscollectin = item;
                        }
                        else
                        {
                            groupscollectin += "  -  " + item;
                        }
                    }
                }

                #endregion

                #region skill and Expertise
                try
                {
                    string[] strarr_skill = Regex.Split(stringSource, "endorse-item-name-text\"");
                    string[] strarr_skill1 = Regex.Split(stringSource, "fmt__skill_name\"");
                    if (strarr_skill.Count() >= 2)
                    {
                        foreach (string item in strarr_skill)
                        {
                            try
                            {
                                if (!item.Contains("!DOCTYPE html"))
                                {
                                    try
                                    {
                                        //string Grp = item.Substring(item.IndexOf("<"), (item.IndexOf(">", item.IndexOf("<")) - item.IndexOf("<"))).Replace("<", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Replace("\"u002", "-").Trim();
                                        string Grp = item.Substring(item.IndexOf(">"), (item.IndexOf("<", item.IndexOf(">")) - item.IndexOf(">"))).Replace(">", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Replace("\"u002", "-").Trim();
                                        checkgrplist.Add(Grp);
                                        checkgrplist.Distinct().ToList();
                                    }
                                    catch { }
                                }

                            }
                            catch { }
                        }

                        foreach (string item in checkgrplist)
                        {
                            if (string.IsNullOrEmpty(Skill))
                            {
                                Skill = item.Replace("\"u002", "-").Trim();
                            }
                            else
                            {
                                Skill = Skill + "  -  " + item;
                            }
                        }
                    }
                    else
                    {
                        if (strarr_skill1.Count() >= 2)
                        {
                            try
                            {
                                foreach (string skillitem in strarr_skill1)
                                {
                                    if (!skillitem.Contains("!DOCTYPE html"))
                                    {
                                        try
                                        {
                                            string Grp = skillitem.Substring(skillitem.IndexOf(":"), (skillitem.IndexOf("}", skillitem.IndexOf(":")) - skillitem.IndexOf(":"))).Replace(":", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Trim();
                                            checkgrplist.Add(Grp);
                                            checkgrplist.Distinct().ToList();
                                        }
                                        catch { }
                                    }
                                }

                                foreach (string item in checkgrplist)
                                {
                                    if (string.IsNullOrEmpty(Skill))
                                    {
                                        Skill = item;
                                    }
                                    else
                                    {
                                        Skill = Skill + "  -  " + item;
                                    }
                                }
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
                Skill = Skill.Replace("a href=\";edu", "").Trim();
                #endregion

                #region Pasttitle
                string[] pasttitles = Regex.Split(stringSource, "title_highlight");
                string pstTitlesitem = string.Empty;
                pasttitles = pasttitles.Skip(1).ToArray();
                foreach (string item in pasttitles)
                {
                    try
                    {

                        if (!item.Contains("<!DOCTYPE html>") && !item.Contains("Tip: You can also search by keyword"))
                        {

                            try
                            {
                                string[] Past_Ttl = Regex.Split(item, ",");
                                pstTitlesitem = Past_Ttl[0].Replace(":", string.Empty).Replace("\"", string.Empty).Replace("\\u002d", "-").Replace("&amp;", "&");
                            }
                            catch { }
                            if ((pstTitlesitem.Contains("strong class")) || (pstTitlesitem.Contains("highlight")) || (string.IsNullOrEmpty(pstTitlesitem)))
                            {
                                try
                                {
                                    int startindex1 = item.IndexOf("\"title\":");
                                    string start1 = item.Substring(startindex1).Replace("\"title\":", string.Empty);
                                    int endIndex1 = start1.IndexOf(",");
                                    pstTitlesitem = (start1.Substring(0, endIndex1).Replace("\"", string.Empty).Replace(":", string.Empty).Replace("&amp", "&").Replace(";", string.Empty).Replace("\\u002d", "-").Replace("name:", string.Empty));
                                }
                                catch
                                { }
                            }

                            if (string.IsNullOrEmpty(LDS_PastTitles))
                            {
                                LDS_PastTitles = pstTitlesitem;
                            }
                            else if (LDS_PastTitles.Contains(pstTitlesitem))
                            {
                                continue;
                            }
                            else
                            {
                                LDS_PastTitles = LDS_PastTitles + "  :  " + pstTitlesitem;
                            }

                        }

                    }
                    catch
                    {
                    }
                }
                #endregion

                #region All Company Summary
                //string[] pasttitles = Regex.Split(stringSource, "company_name");
                //string pstTitlesitem = string.Empty;
                string pstDescCompitem = string.Empty;
                pasttitles = pasttitles.Skip(1).ToArray();
                foreach (string item in pasttitles)
                {
                    if (item.Contains("positionId"))
                    {
                        try
                        {
                            int startindex = item.IndexOf(":");
                            if (startindex > 0)
                            {
                                string start = item.Substring(startindex).Replace(":\"", "");
                                int endindex = start.IndexOf("\",");
                                string end = start.Substring(0, endindex);
                                pstTitlesitem = end.Replace(",", ";").Replace("&amp;", "&").Replace("\\u002d", "-");
                            }




                            int startindex1 = item.IndexOf("summary_lb\":\"");
                            if (startindex > 0)
                            {
                                string start1 = item.Substring(startindex1).Replace("summary_lb\":\"", "");
                                int endindex1 = 0;

                                if (start1.Contains("associatedWith"))
                                {
                                    endindex1 = start1.IndexOf("\",\"associatedWith\"");

                                    if (start1.Contains("\"}"))
                                    {
                                        endindex1 = start1.IndexOf("\"}");
                                    }

                                }
                                else if (start1.Contains("\"}"))
                                {
                                    endindex1 = start1.IndexOf("\"}");
                                }


                                string end1 = start1.Substring(0, endindex1);
                                pstDescCompitem = end1.Replace(",", ";").Replace("u002d", "-").Replace("<br>", string.Empty).Replace("\\n", string.Empty).Replace("\\", string.Empty).Replace("&#xf0a7;", "@").Replace("&#x2019;", "'").Replace("&#x2022", "@").Replace("&#x25cf;", "@");

                                if (pstDescCompitem.Contains("\";\"associatedWith"))
                                {
                                    pstDescCompitem = Regex.Split(pstDescCompitem, "\";\"associatedWith")[0];
                                }
                            }

                            if (string.IsNullOrEmpty(LDS_Desc_AllComp))
                            {
                                LDS_Desc_AllComp = pstTitlesitem + "-" + pstDescCompitem + " ++ ";
                            }
                            else
                            {
                                LDS_Desc_AllComp = LDS_Desc_AllComp + pstTitlesitem + "-" + pstDescCompitem + " ++ ";
                            }
                        }
                        catch
                        {
                        }
                    }

                }
                #endregion

                #region AccountType
                try
                {
                    if (stringSource.Contains("has a Premium Account") || stringSource.Contains("Account Holder"))
                    {
                        AccountType = "Premium Account";
                    }
                    else
                    {
                        AccountType = "Basic Account";
                    }
                }
                catch (Exception ex)
                {

                }
                #endregion Email
                #region FullUrl
                try
                {
                    string[] UrlFull = System.Text.RegularExpressions.Regex.Split(Url, "&authType");
                    LDS_UserProfileLink = UrlFull[0];

                    LDS_UserProfileLink = Url;
                    //  LDS_UserProfileLink = stringSource.Substring(stringSource.IndexOf("canonicalUrl\":"), (stringSource.IndexOf(",", stringSource.IndexOf("canonicalUrl\":")) - stringSource.IndexOf("canonicalUrl\":"))).Replace("canonicalUrl\":", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Trim();
                }
                catch { }
                #endregion

                LDS_LoginID = SearchCriteria.LoginID;
                if (string.IsNullOrEmpty(firstname))
                {
                    firstname = "Linkedin Member";
                }



                string endName = firstname + " " + lastname;
                //GroupStatus.GroupSpecMem.Add(GroupMemId, endName);
                if (firstname == string.Empty) firstname = "LinkedIn";
                if (lastname == string.Empty) lastname = "Member";
                if (Company == string.Empty) Company = "--";
                if (titlecurrent == string.Empty) titlecurrent = "--";
                if (companycurrent == string.Empty) companycurrent = "--";
                if (LDS_Desc_AllComp == string.Empty) LDS_Desc_AllComp = "--";
                if (LDS_BackGround_Summary == string.Empty) LDS_BackGround_Summary = "--";
                if (Connection == string.Empty) Connection = "--";
                if (recomandation == string.Empty) recomandation = "--";
                if (Skill == string.Empty) Skill = "--";
                if (LDS_Experience == string.Empty) LDS_Experience = "--";
                if (EducationCollection == string.Empty) EducationCollection = "--";
                if (groupscollectin == string.Empty) groupscollectin = "--";
                if (USERemail == string.Empty) USERemail = "--";
                if (LDS_UserContact == string.Empty) LDS_UserContact = "--";
                if (LDS_PastTitles == string.Empty) LDS_PastTitles = "--";
                if (AllComapny == string.Empty) AllComapny = "--";
                if (location == string.Empty) location = "--";
                if (country == string.Empty) country = "--";
                if (Industry == string.Empty) Industry = "--";
                if (Website == string.Empty) Website = "--";



                string LDS_FinalData = TypeOfProfile + "," + LDS_UserProfileLink + "," + firstname + "," + lastname + "," + Company.Replace(",", ";") + "," + titlecurrent.Replace(",", ";") + "," + companycurrent.Replace(",", ";") + "," + CurrentCompSite.Replace(",", ";") + "," + LDS_BackGround_Summary.Replace(",", ";") + "," + Connection.Replace(",", ";") + "," + recomandation.Replace(",", string.Empty) + "," + Skill.Replace(",", ";") + "," + LDS_Experience.Replace(",", string.Empty) + "," + EducationCollection.Replace(",", ";") + "," + groupscollectin.Replace(",", ";") + "," + USERemail.Replace(",", ";") + "," + LDS_UserContact.Replace(",", ";") + "," + LDS_PastTitles + "," + AllComapny.Replace(",", ";") + "," + location.Replace(",", ";") + "," + country.Replace(",", ";") + "," + Industry.Replace(",", ";") + "," + Website.Replace(",", ";") + "," + LDS_LoginID + "," + AccountType;

                if (!string.IsNullOrEmpty(firstname))
                {
                    //Log("[ " + DateTime.Now + " ] => [ Data : " + LDS_FinalData + " ]");
                }
                else
                {
                    AddToLogger("[ " + DateTime.Now + " ] => [ No Data For URL : " + Url + " ]");
                    GlobusFileHelper.AppendStringToTextfileNewLineWithCarat(Url, Globals.DesktopFolder + "\\UnScrapedList.txt");
                }

                if (SearchCriteria.starter)
                {
                    //string tempFinalData = LDS_FinalData.Replace(";", "").Replace(LDS_UserProfileLink, "").Replace(TypeOfProfile, "").Replace(",", "").Replace(LDS_LoginID, "").Trim();

                    //if (!string.IsNullOrEmpty(tempFinalData))
                    {
                        //if (CheckEmployeeScraper)
                        //{
                        //    string FileName = "CompanyEmployeeScraper";
                        //    AppFileHelper.AddingLinkedInDataToCSVFileCompanyEmployeeScraper(LDS_FinalData, FileName);
                        //    return true;
                        //}
                        //else if (CampaignScraper)
                        {
                            AppFileHelper.AddingLinkedInDataToCSVFile(LDS_FinalData, FileName);
                            return true;
                        }
                        //else
                        //{
                        //    AppFileHelper.AddingLinkedInDataToCSVFile(LDS_FinalData, SearchCriteria.FileName);
                        //    return true;
                        //}
                    }
                }
            }
            catch { }
            return false;
        }
        #endregion

        private void DeleteCampaign_Click(object sender, System.EventArgs e)
        {
            clsDBQueryManager obj_clsDBQueryManager = new clsDBQueryManager();
            string campname = string.Empty;
            foreach (DataGridViewRow Dr in dgv_CampaignScraper.Rows)
            {
                try
                {
                    if (Convert.ToBoolean(Dr.Cells[0].Value) == true)
                    {
                        campname = Dr.Cells[1].Value.ToString();
                        string query = "Delete from tb_CampaignScraper where CampaignName='" + campname + "';";
                        obj_clsDBQueryManager.DeleteCampaignScraperData(query);
                    }
                }
                catch
                { }
            }          

            DataSet DS = new DataSet();
            DataTable DT = new DataTable();
            try
            {
                DS = new clsDBQueryManager().getAllCampaignScraperData();

                DT = DS.Tables[0];

                dgv_CampaignScraper.DataSource = DT.DefaultView;
            }
            catch
            { }
        }

        private void frmCampaignScraper_Paint(object sender, PaintEventArgs e)
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
