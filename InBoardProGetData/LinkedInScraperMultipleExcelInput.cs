using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InBoardPro;
using System.Threading;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using BaseLib;

namespace InBoardPro
{
   public class InBoardProGetDataMultipleExcelInput
    {
        #region Variable Declaration
        public string accountUser = string.Empty;
        public string accountPass = string.Empty;
        public string proxyAddress = string.Empty;
        public string proxyPort = string.Empty;
        public string proxyUserName = string.Empty;
        public string proxyPassword = string.Empty;
        public string postalCode = string.Empty;
        public string distance = string.Empty;
        public string industryType = string.Empty;
        public string lastName = string.Empty;
        public Events InBoardProGetDataLogEvents = new Events();
        public static int processNumber = 0;
        public static bool autopause = false;
        public static bool autopauseandresume = false;
        public static List<Thread> lstNumberOfThreads = new List<Thread>();
        public static int Mindelay = 20;
        public static int Maxdelay = 60;
        public static int counterforpause = 0;
        public static string SelectedMode = string.Empty;
        public static readonly object locker_pauseDialog = new object();
        #endregion

        #region ParsingOfInBoardProGetDataMultipleInput
        public void ParsingOfInBoardProGetDataMultipleInput(ref GlobusHttpHelper HttpHelper, string username, string password, string proxyaddress, string proxyport, string proxyUserName, string proxypassword, string postalcode, string distance, string industry, string lastname)
        {
            try
            {
                string csrfToken = string.Empty;
                string LastName = string.Empty;
                string FirstName = string.Empty;
                string Industry = string.Empty;
                string Postalcode = string.Empty;
                string Distance = string.Empty;
                string contentSummary = string.Empty;
                string Title = string.Empty;
                string Company = string.Empty;
                string school = string.Empty;
                string Country = string.Empty;
                string countrycode = string.Empty;
                string industrycode = string.Empty;
                string UserName = string.Empty;
                List<String> LstUrlrecords = new List<string>();
                LastName = lastname;
                Industry = industry;
                UserName = username;
                this.accountUser = username;
                this.accountPass = password;
                this.proxyAddress = proxyaddress;
                this.proxyPort = proxyport;
                this.proxyUserName = proxyUserName;
                this.proxyPassword = proxypassword;
                this.postalCode = postalcode;
                this.distance = distance;
                this.industryType = industry;
                this.lastName = lastname;
                string pageSource = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/home?trk=hb_tab_home_top"));

                try
                {
                    string[] Arr_Pst = Regex.Split(postalCode, "{");
                    try
                    {
                        Postalcode = postalCode.Substring(0, postalCode.IndexOf(" "));
                        Country = postalCode.Replace(Postalcode, string.Empty).Replace(")", string.Empty).Replace("(", string.Empty).Trim();
                    }
                    catch
                    {
                        if (Postalcode == "")
                        {
                            Postalcode = postalCode;
                        }

                        Country = postalCode.Replace(Postalcode, string.Empty).Replace(")", string.Empty).Replace("(", string.Empty).Trim();
                    }
                }
                catch { }
                if (pageSource.Contains("csrfToken"))
                {
                    csrfToken = pageSource.Substring(pageSource.IndexOf("csrfToken"), 100);
                    string[] Arr = csrfToken.Split('&');
                    csrfToken = Arr[0];
                    csrfToken = csrfToken.Replace("csrfToken=", "");
                    csrfToken = csrfToken.Replace("%3A", ":");
                }
                InBoardPro.GetIndustryCode objIndustry = new GetIndustryCode();
                Dictionary<string, string> Dict_IndustryCode = new Dictionary<string, string>();
                Dict_IndustryCode = objIndustry.getIndustry();
                foreach (KeyValuePair<string, string> item in Dict_IndustryCode)
                {
                    try
                    {
                        string toloweritem = item.Value.ToLower();
                        string tolowerindustrytype = industryType.ToLower();
                        if (toloweritem == tolowerindustrytype)
                        {
                            industrycode = item.Key;
                            break;
                        }
                    }
                    catch
                    {
                    }
                }

                Dictionary<string, string> Dict_CountryCode = new Dictionary<string, string>();
                Dict_CountryCode = objIndustry.getCountry();
                foreach (KeyValuePair<string, string> item in Dict_CountryCode)
                {
                    try
                    {
                        string toloweritem = item.Value.ToLower();
                        string tolowercountrytype = Country.ToLower();
                        if (toloweritem == tolowercountrytype)
                        {
                            countrycode = item.Key;
                            break;
                        }
                    }
                    catch
                    {
                    }
                }
                string Firstresponse = string.Empty;
                if (string.IsNullOrEmpty(countrycode))
                {
                    countrycode = "us";
                }
                try
                {
                    string FirstPostUrl = "http://www.linkedin.com/search/fpsearch";
                    string FirstPostData = "csrfToken=" + csrfToken + "&keepFacets=true&pplSearchOrigin=ADVS&keywords=&fname=" + FirstName + "&lname=" + lastName + "&searchLocationType=I&countryCode=" + countrycode + "&postalCode=" + Postalcode + "&distance=" + distance + "&title=" + Title + "&company=" + Company + "&school=" + school + "&facet_I=" + industrycode + "&N=&L=&sortCriteria=R&viewCriteria=2&%2Fsearch%2Ffpsearch=Search";
                    Firstresponse = HttpHelper.postFormData(new Uri(FirstPostUrl), FirstPostData);
                }
                catch { }
                string FirstGetRequestUrl = string.Empty;
                string FirstGetResponse = string.Empty;
                try
                {
                    FirstGetRequestUrl = "http://www.linkedin.com/search/fpsearch?lname=" + LastName + "&searchLocationType=I&countryCode=" + countrycode + "&postalCode=" + Postalcode + "&distance=" + distance + "&keepFacets=keepFacets&page_num=1&facet_I=" + industrycode + "&pplSearchOrigin=ADVS&viewCriteria=2&sortCriteria=R&redir=redir";
                    FirstGetResponse = HttpHelper.getHtmlfromUrl1(new Uri(FirstGetRequestUrl));
                }
                catch { }
                int RecordCount = 0;
                try
                {
                    RecordCount = objIndustry.GetPageNo(Firstresponse);
                }
                catch { }
              

                try
                {
                    LstUrlrecords = ScarppLinkedinDataByPageNumber(RecordCount, ref HttpHelper, Firstresponse, industrycode, Postalcode, countrycode);
                    LstUrlrecords = LstUrlrecords.Distinct().ToList();
                }
                catch { }
                List<string> lstshorturl = new List<string>();
                foreach (string itemurl in LstUrlrecords)
                {
                    try
                    {
                        lstshorturl.Add(itemurl);
                    }
                    catch { }

                }
                lstshorturl = lstshorturl.Distinct().ToList();
                
                LinkedinScrappDbManager objLsManager = new LinkedinScrappDbManager();
                int count = 1;
                foreach (string LinkUrl in LstUrlrecords)
                {
                    try
                    {

                        //string[] LinkUrll=Regex.Split(LinkUrl, "&authType");
                        objLsManager.InsertIntoLinkedInSearchUrlResult(postalCode, distance, industryType, lastName, accountUser, accountPass, proxyAddress, proxyPassword, RecordCount, LinkUrl);
                        Log("[ " + DateTime.Now + " ] => [ " + count + "Url Record Insert In Table ! ]");
                        count++;
                    }
                    catch { }

                }

                foreach (string LinkUrlParse in LstUrlrecords)
                {
                    lock (locker_pauseDialog)
                    {
                        try
                        {
                            CrawlingLinkedInPage(LinkUrlParse, ref HttpHelper);
                            objLsManager.Updatetb_LinkedinSearchUrlResult(postalCode, distance, industryType, lastName, accountUser, accountPass, proxyAddress, proxyPassword, RecordCount, LinkUrlParse);
                            counterforpause++;
                            if (counterforpause <= processNumber)
                            {
                                
                            }
                            else 
                            {

                                if (autopause)
                                {
                                    DialogResult dlgResult = MessageBox.Show("Do you want to continue?", "Continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                    if (dlgResult == DialogResult.Yes)
                                    {
                                        foreach (Thread itempause in lstNumberOfThreads)
                                        {
                                            try
                                            {
                                                itempause.Resume();
                                            }
                                            catch { }
                                        }
                                        Log("[ " + DateTime.Now + " ] => [ PROCESS START ]");
                                        counterforpause = 1;
                                    }
                                    else if (dlgResult == DialogResult.No)
                                    {
                                      {
                                            foreach (Thread item in lstNumberOfThreads)
                                            {
                                                try
                                                {
                                                    item.Abort();
                                                }
                                                catch { }
                                            }
                                            Log("[ " + DateTime.Now + " ] => [ PROCESS STOP ]");
                                            counterforpause = 1;
                                       }
                                    
                                    }

                                }
                            
                                if (autopauseandresume)
                                {
                                    try
                                    {

                                        try
                                        {
                                            int delayInSeconds = RandomNumberGenerator.GenerateRandom(Mindelay * 1000, Maxdelay * 1000);
                                            Log("[ " + DateTime.Now + " ] => [ Delaying for " + delayInSeconds / 1000 + " Seconds ]");
                                            Thread.Sleep(delayInSeconds);

                                        }
                                        catch { }
                                        counterforpause = 1;
                                    }
                                    catch { }
                               }
                            }
                        }
                        catch { }
                    }
                }
            }
            catch { }

            finally
            {
                Log("[ " + DateTime.Now + " ] => [ PROCESS COMPLETE ]");
            }


        }
        #endregion

        #region ScrapLinkedinDataByPageNumber
        private List<string> ScarppLinkedinDataByPageNumber(int pagerecords, ref GlobusHttpHelper HttpHelper, string Pagesource, string indstrycd, string postalcd, string countrycd)
        {

            List<string> lstRecordurl = new List<string>();
            try
            {
                string ResponseForPremiumAcc = Pagesource;
                int pagenumber = (pagerecords / 10) + 1;

                string GetResponce = string.Empty;
                string GetRequestURL = string.Empty;
                string PostRequestURL = string.Empty;
                string PostdataForPagination = string.Empty;
                string PostResponce = string.Empty;
                if (pagenumber >= 1)
                {
                    for (int i = 1; i <= pagenumber; i++)
                    {
                        PostRequestURL = string.Empty;
                        PostdataForPagination = string.Empty;
                        PostResponce = string.Empty;
                        {

                         
                            if (ResponseForPremiumAcc.Contains("Account Type:</span> Basic"))
                            {
                                try
                                {
                                    PostRequestURL = "http://www.linkedin.com/search/hits";
                                    PostdataForPagination = "lname=" + lastName + "&searchLocationType=I&countryCode=" + countrycd + "&postalCode=" + postalcd + "&distance=" + distance + "&keepFacets=keepFacets&facet_I=" + indstrycd + "&pplSearchOrigin=ADVS&viewCriteria=2&sortCriteria=R&facetsOrder=I%2CCC%2CN%2CPC%2CED%2CL%2CFG%2CTE%2CFA%2CSE%2CP%2CCS%2CF%2CDR%2CG&page_num=" + i + "&openFacets=I%2CCC%2CN";
                                    PostResponce = HttpHelper.postFormData(new Uri(PostRequestURL), PostdataForPagination);
                                }
                                catch { }
                            }

                            else if (ResponseForPremiumAcc.Contains(""))
                            {
                                try
                                {
                                    PostRequestURL = "http://www.linkedin.com/search/hits";
                                    PostdataForPagination = "lname=" + lastName + "&searchLocationType=I&countryCode=" + countrycd + "&postalCode=" + postalcd + "&distance=" + distance + "&keepFacets=keepFacets&facet_I=" + indstrycd + "&pplSearchOrigin=ADVS&viewCriteria=2&sortCriteria=R&facetsOrder=I%2CCC%2CN%2CPC%2CED%2CL%2CFG%2CTE%2CFA%2CSE%2CP%2CCS%2CF%2CDR%2CG&page_num=" + i + "&openFacets=I%2CCC%2CN";
                                    PostResponce = HttpHelper.postFormData(new Uri(PostRequestURL), PostdataForPagination);
                                    if (string.IsNullOrEmpty(PostResponce))
                                    {
                                        PostRequestURL = "http://www.linkedin.com/search/hits";
                                        PostdataForPagination = "lname=" + lastName + "&searchLocationType=I&countryCode=" + countrycd + "&postalCode=" + postalcd + "&distance=" + distance + "&keepFacets=keepFacets&facet_I=" + indstrycd + "&pplSearchOrigin=ADVS&viewCriteria=2&sortCriteria=R&facetsOrder=I%2CCC%2CN%2CPC%2CED%2CL%2CFG%2CTE%2CFA%2CSE%2CP%2CCS%2CF%2CDR%2CG&page_num=" + i + "&openFacets=I%2CCC%2CN";
                                        PostResponce = HttpHelper.postFormData(new Uri(PostRequestURL), PostdataForPagination);
                                    }
                                }
                                catch { }


                            }
                            else if (ResponseForPremiumAcc.Contains("Account Type:</span> Executive"))
                            {
                                try
                                {
                                    PostRequestURL = "http://www.linkedin.com/search/hits";
                                    PostdataForPagination = "lname=" + lastName + "&searchLocationType=I&countryCode=" + countrycd + "&postalCode=" + postalcd + "&distance=" + distance + "&keepFacets=keepFacets&facet_I=" + indstrycd + "&pplSearchOrigin=ADVS&viewCriteria=2&sortCriteria=R&facetsOrder=I%2CCC%2CN%2CPC%2CED%2CL%2CFG%2CTE%2CFA%2CSE%2CP%2CCS%2CF%2CDR%2CG&page_num=" + i + "&openFacets=I%2CCC%2CN";
                                    PostResponce = HttpHelper.postFormData(new Uri(PostRequestURL), PostdataForPagination);
                                }
                                catch { }

                            }


                            if (PostResponce.Contains("/profile/view?id"))
                            {

                                List<string> PageSerchUrl = ChilkatBasedRegex.GettingAllUrls(PostResponce, "profile/view?id");
                                if (PageSerchUrl.Count == 0)
                                {
                                    break;
                                }
                                foreach (string item in PageSerchUrl)
                                {
                                    try
                                    {
                                        {
                                            if (item.Contains("authType=") && item.Contains("pp_profile_name_link"))
                                            {

                                                try
                                                {
                                                    if (item.Contains("//www.linkedin.com"))
                                                    {
                                                        string urlSerch = item;
                                                        lstRecordurl.Add(urlSerch);
                                                        lstRecordurl = lstRecordurl.Distinct().ToList();
                                                    }
                                                    else
                                                    {
                                                        string urlSerch = "https://www.linkedin.com" + item;
                                                        lstRecordurl.Add(urlSerch);
                                                        lstRecordurl = lstRecordurl.Distinct().ToList();
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

                            else if (!PostResponce.Contains("authType=") && PostResponce.Contains("Custom views are no longer supported. Please select either Basic or Expanded view"))
                            {
                                break;
                            }
                        }
                    }
                }
                else
                {
                    if (ResponseForPremiumAcc.Contains("Account Type:</span> Basic"))
                    {
                        try
                        {
                            PostRequestURL = "http://www.linkedin.com/search/hits";
                            PostdataForPagination = "lname=" + lastName + "&searchLocationType=I&countryCode=" + countrycd + "&postalCode=" + postalcd + "&distance=" + distance + "&keepFacets=keepFacets&facet_I=" + indstrycd + "&pplSearchOrigin=ADVS&viewCriteria=2&sortCriteria=R&facetsOrder=I%2CCC%2CN%2CPC%2CED%2CL%2CFG%2CTE%2CFA%2CSE%2CP%2CCS%2CF%2CDR%2CG&page_num=1&openFacets=I%2CCC%2CN";
                            PostResponce = HttpHelper.postFormData(new Uri(PostRequestURL), PostdataForPagination);
                        }
                        catch { }
                    }
                    else if (ResponseForPremiumAcc.Contains("Account Type:</span> Executive"))
                    {
                        try
                        {
                            PostRequestURL = "http://www.linkedin.com/search/hits";
                            PostdataForPagination = "lname=" + lastName + "&searchLocationType=I&countryCode=" + countrycd + "&postalCode=" + postalcd + "&distance=" + distance + "&keepFacets=keepFacets&facet_I=" + indstrycd + "&pplSearchOrigin=ADVS&viewCriteria=2&sortCriteria=R&facetsOrder=I%2CCC%2CN%2CPC%2CED%2CL%2CFG%2CTE%2CFA%2CSE%2CP%2CCS%2CF%2CDR%2CG&page_num=1&openFacets=I%2CCC%2CN";
                            PostResponce = HttpHelper.postFormData(new Uri(PostRequestURL), PostdataForPagination);
                        }
                        catch { }

                    }

                    if (PostResponce.Contains("/profile/view?id"))
                    {

                        List<string> PageSerchUrl = ChilkatBasedRegex.GettingAllUrls(PostResponce, "profile/view?id");
                        if (PageSerchUrl.Count == 0)
                        {

                            //Log("On the basis of you Account you can able to see " + RecordURL.Count + "Results");
                            // break;
                        }
                        foreach (string item in PageSerchUrl)
                        {
                            try
                            {
                                {
                                    if (item.Contains("authType="))
                                    {

                                        string urlSerch = "http://www.linkedin.com" + item;
                                        // Log(urlSerch);
                                        lstRecordurl.Add(urlSerch);
                                    }
                                }
                            }
                            catch
                            {
                            }
                        }
                    }

                }
            }
            catch { }
            return lstRecordurl;
        }
        #endregion

        #region CrawlingLinkedInPage
        private void CrawlingLinkedInPage(string Url, ref GlobusHttpHelper HttpHelper)
        {
            #region Data Initialization
            string GroupMemId = string.Empty;
            string Industry = string.Empty;
            string URLprofile = string.Empty;
            string firstname = string.Empty;
            string lastname = string.Empty;
            string location = string.Empty;
            string state = string.Empty;
            string country = string.Empty;
            string postal = string.Empty;
            string phone = string.Empty;
            string USERemail = string.Empty;
            string code = string.Empty;
            string education1 = string.Empty;
            string education2 = string.Empty;
            string titlecurrent = string.Empty;
            string companycurrent = string.Empty;
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
            List<string> Checklist = new List<string>();
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
            string Company = string.Empty;
            List<string> lstpasttitle = new List<string>();
            List<string> checkpasttitle = new List<string>();
            #endregion

            #region

            string stringSource = HttpHelper.getHtmlfromUrl1(new Uri(Url));

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
                        strFamilyName = stringSource.Substring(stringSource.IndexOf("fmt__full_name\":"), (stringSource.IndexOf(",", stringSource.IndexOf("fmt__full_name\":")) - stringSource.IndexOf("fmt__full_name\":"))).Replace("fmt__full_name\":", string.Empty).Replace("\\u002d", "-").Replace("\"", string.Empty).Replace(",", string.Empty).Trim();

                    }
                    catch { }
                }

            }
            catch { }

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
            catch (Exception ex)
            {
                Console.WriteLine("Error >>> " + ex.StackTrace);
            }
            #endregion

            #region LastName

            lastname = NameArr[1];

            try
            {
                if (NameArr.Count() == 3)
                {
                    lastname = NameArr[1] + " " + NameArr[2];
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
                    Company = stringSource.Substring(stringSource.IndexOf("visible\":true,\"memberHeadline"), (stringSource.IndexOf("memberID", stringSource.IndexOf("visible\":true,\"memberHeadline")) - stringSource.IndexOf("visible\":true,\"memberHeadline"))).Replace("visible\":true,\"memberHeadline", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Replace("\\u002d", "-").Replace("   ", string.Empty).Trim();
                }
                catch { }

                if (string.IsNullOrEmpty(Company))
                {

                    try
                    {
                        Company = stringSource.Substring(stringSource.IndexOf("memberHeadline\":"), (stringSource.IndexOf(",", stringSource.IndexOf("memberHeadline\":")) - stringSource.IndexOf("memberHeadline\":"))).Replace("memberHeadline\":", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Replace("\\u002d", "-").Replace("   ", string.Empty).Replace("u002d", string.Empty).Trim();
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

            #region PastCompany
            string[] companylist = new string[] { };
            string[] companyUrlList = new string[] { };

            try
            {
                companylist = Regex.Split(stringSource, "title_highlight");
            }
            catch { }

            string AllComapny = string.Empty;

            string Companyname = string.Empty;
            string ComapnyUrl = string.Empty;
            foreach (string item in companylist)
            {
                try
                {
                    if (!item.Contains("<!DOCTYPE html>"))
                    {

                        try
                        {
                            int startindex = item.IndexOf("company_name");
                            string start = item.Substring(startindex);
                            int endIndex = start.IndexOf(",");
                            Companyname = (start.Substring(0, endIndex).Replace("\"", string.Empty).Replace(":", string.Empty).Replace("&amp", "&").Replace("company_name", string.Empty).Replace(";", string.Empty).Replace("\\u002d", "-").Replace("name:", string.Empty));

                            int startindex1 = item.IndexOf("biz_logo");
                            string start1 = item.Substring(startindex1);
                            int endIndex1 = start1.IndexOf("&trk");
                            ComapnyUrl = (start1.Substring(0, endIndex1).Replace("\"", string.Empty).Replace(":", string.Empty).Replace("&amp", "&").Replace("company_name", string.Empty).Replace(";", string.Empty).Replace("\\u002d", "-").Replace("name:", string.Empty).Replace("biz_logo", "http://www.linkedin.com"));

                        }
                        catch { }

                        checkerlst.Add(Companyname + "@" + ComapnyUrl);

                    }
                }
                catch { }
            }
            foreach (string item1 in checkerlst)
            {
                try
                {
                    string compUrl = item1.Split('@')[1];
                    string stringSourceCompUrl = HttpHelper.getHtmlfromUrl1(new Uri(compUrl));
                    string companyUrl = string.Empty;

                    try
                    {
                        companyUrl = stringSourceCompUrl.Substring(stringSourceCompUrl.IndexOf("<h4>Website</h4>"), (stringSourceCompUrl.IndexOf("</a>", stringSourceCompUrl.IndexOf("<h4>Website</h4>")) - stringSourceCompUrl.IndexOf("<h4>Website</h4>"))).Replace("<h4>Website</h4>", string.Empty).Replace("</a>", string.Empty).Trim();
                        companyUrl = Regex.Split(companyUrl, "target=\"_blank\">")[1];
                    }
                    catch { }

                    if (AllComapny == string.Empty)
                    {
                        AllComapny = item1.Split('@')[0] + "@" + companyUrl;
                    }
                    else
                    {
                        AllComapny = AllComapny + " : " + item1.Split('@')[0] + "@" + companyUrl;
                    }


                }
                catch { }
            }
            #endregion

            #endregion Company

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

                        if (!item.Contains("<!DOCTYPE html>"))
                        {
                            try
                            {
                                try
                                {
                                    int startindex = item.IndexOf("fmt__school_highlight");
                                    string start = item.Substring(startindex).Replace("fmt__school_highlight", "");
                                    int endindex = start.IndexOf(",");
                                    School = start.Substring(0, endindex).Replace("\\u002d", string.Empty).Replace(":", string.Empty).Replace("\"", string.Empty);
                                }
                                catch { }

                                try
                                {
                                    int startindex1 = item.IndexOf("degree");
                                    string start1 = item.Substring(startindex1).Replace("degree", "");
                                    int endindex1 = start1.IndexOf(",");
                                    Degree = start1.Substring(0, endindex1).Replace("\\u002d", string.Empty).Replace(":", string.Empty).Replace("\"", string.Empty);
                                }
                                catch { }

                                try
                                {
                                    int startindex2 = item.IndexOf("enddate_my");
                                    string start2 = item.Substring(startindex2).Replace("enddate_my", "");
                                    int endindex2 = start2.IndexOf(",");
                                    SessionEnd = start2.Substring(0, endindex2).Replace("\\u002d", string.Empty).Replace(":", string.Empty).Replace("\"", string.Empty);
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
                string[] str_Email = Regex.Split(stringSource, "email\"");
                USERemail = stringSource.Substring(stringSource.IndexOf("[{\"email\":"), (stringSource.IndexOf("}]", stringSource.IndexOf("[{\"email\":")) - stringSource.IndexOf("[{\"email\":"))).Replace("[{\"email\":", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Replace("\\u002d", "-").Trim();
            }

            catch { }

            #endregion Email

            #region Website

            #region Website
            try
            {
                Website = stringSource.Substring(stringSource.IndexOf("[{\"URL\":"), (stringSource.IndexOf(",", stringSource.IndexOf("[{\"URL\":")) - stringSource.IndexOf("[{\"URL\":"))).Replace("[{\"URL\":", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace("]", string.Empty).Replace("}", string.Empty).Trim();
            }
            catch { }
            #endregion Website

            #endregion Website

            #region location
            try
            {
                location = stringSource.Substring(stringSource.IndexOf("Country\",\"fmt__location\":"), (stringSource.IndexOf("i18n_no_location_matches", stringSource.IndexOf("Country\",\"fmt__location\":")) - stringSource.IndexOf("Country\",\"fmt__location\":"))).Replace("Country\",\"fmt__location\":", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Replace("\\u002d", "-").Replace("]", string.Empty).Replace("}", string.Empty).Trim();
            }
            catch { }
            // fmt_location

            if (string.IsNullOrEmpty(location))
            {
                try
                {
                    location = stringSource.Substring(stringSource.IndexOf("fmt_location\":"), (stringSource.IndexOf(",", stringSource.IndexOf("fmt_location\":")) - stringSource.IndexOf("fmt_location\":"))).Replace("fmt_location\":", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Replace("\\u002d", "-").Replace("]", string.Empty).Replace("}", string.Empty).Trim();

                }
                catch { }
            }


            #endregion location

            #region state

            int startindexState = stringSource.IndexOf("location_highlight");
            string Statestart = stringSource.Substring(startindexState);
            int StateendIndex = Statestart.IndexOf("industry_highlight");
            state = (Statestart.Substring(0, StateendIndex).Replace("location_highlight", string.Empty).Replace(":", string.Empty).Replace("\"", string.Empty).Replace("deferImgtrue,", string.Empty));
            try
            {
                state = state.Split(',')[1].Replace("Area", string.Empty).Replace("]", string.Empty).Replace("}", string.Empty).Trim();
            }
            catch { }

            #endregion

            #region Country
            try
            {
                int startindexCountry = stringSource.IndexOf("displayCountry");
                string Countrystart = stringSource.Substring(startindexCountry);
                int CountryendIndex = Countrystart.IndexOf("displayLanguage");
                country = (Countrystart.Substring(0, CountryendIndex).Replace("displayCountry", string.Empty).Replace(":", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace("]", string.Empty).Replace("}", string.Empty));

            }
            catch { }


            #endregion

            #region Industry
            try
            {
                Industry = stringSource.Substring(stringSource.IndexOf("fmt__industry_highlight\":"), (stringSource.IndexOf(",", stringSource.IndexOf("fmt__industry_highlight\":")) - stringSource.IndexOf("fmt__industry_highlight\":"))).Replace("fmt__industry_highlight\":", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Replace("<strong class=highlight>", string.Empty).Replace("</strong>", string.Empty).Replace("\\u002d", "-").Replace("]", string.Empty).Replace("}", string.Empty).Trim();
            }
            catch { }

            if (string.IsNullOrEmpty(Industry))
            {
                try
                {
                    Industry = stringSource.Substring(stringSource.IndexOf("industry_highlight\":"), (stringSource.IndexOf(",", stringSource.IndexOf("industry_highlight\":")) - stringSource.IndexOf("industry_highlight\":"))).Replace("industry_highlight\":", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Replace("<strong class=highlight>", string.Empty).Replace("</strong>", string.Empty).Replace("\\u002d", "-").Replace("]", string.Empty).Replace("}", string.Empty).Trim();
                }
                catch { }
                //industry_highlight":
            }
            #endregion Industry

            #region Connection
            try
            {
                Connection = stringSource.Substring(stringSource.IndexOf("_count_string\":"), (stringSource.IndexOf(",", stringSource.IndexOf("_count_string\":")) - stringSource.IndexOf("_count_string\":"))).Replace("_count_string\":", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Trim();
            }
            catch { }
            //numberOfConnections":
            if (string.IsNullOrEmpty(Connection))
            {
                try
                {
                    Connection = stringSource.Substring(stringSource.IndexOf("numberOfConnections\":"), (stringSource.IndexOf("}", stringSource.IndexOf("numberOfConnections\":")) - stringSource.IndexOf("numberOfConnections\":"))).Replace("numberOfConnections\":", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Replace("\\u002d", "-").Replace("connectionsBrowseabletrue", string.Empty).Trim();

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
                    RecomnedUrl = (start.Substring(0, endIndex).Replace(",", string.Empty).Replace("\"", string.Empty).Replace(":", string.Empty).Replace("]", string.Empty).Replace("}", string.Empty));

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
                                Heading = (start.Substring(0, endIndex).Replace("\"", string.Empty).Replace(":", string.Empty));
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
                    int endIndex = start.IndexOf("&goback");
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
                                    string Grp = item.Substring(item.IndexOf("<"), (item.IndexOf(">", item.IndexOf("<")) - item.IndexOf("<"))).Replace("<", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Trim();
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
                        try
                        {
                            if (Skill == string.Empty)
                            {
                                Skill = item;
                            }
                            else
                            {
                                Skill = Skill + "  :  " + item;
                            }
                        }
                        catch { }

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
                                try
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
                                catch { }


                            }

                            foreach (string item in checkgrplist)
                            {
                                try
                                {
                                    if (Skill == string.Empty)
                                    {
                                        Skill = item;
                                    }
                                    else
                                    {
                                        Skill = Skill + "  :  " + item;
                                    }
                                }
                                catch { }

                            }
                        }
                        catch { }
                    }
                }
            }
            catch { }

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

            #region ProfileUrl

            try
            {
                LDS_UserProfileLink = Url;

            }
            catch { }
            #endregion

            #region Data Saved In CSV File

            if (!string.IsNullOrEmpty(firstname) || !string.IsNullOrEmpty(lastname) || !string.IsNullOrEmpty(Company))
            {
                try
                {
                    string CSVHeader = "ProfileType" + "," + "UserProfileLink" + "," + "FirstName" + "," + "LastName" + "," + "PostalCode" + "," + "Distance From Postal" + "," + "HeadLineTitle" + "," + "CurrentTitle " + "," + "Company" + "," + "Connection" + "," + "Recommendations " + "," + "SkillAndExpertise " + "," + "Experience " + "," + " Education" + "," + "Groups" + "," + "UserEmail" + "," + "UserContactNumber" + "," + "PastTitles" + "," + "PastCompany With Company Urls" + "," + "City" + "," + "State" + "," + "Country" + "," + "Industry" + "," + "WebSites" + "," + "LinkedInLoginID" + "," + "DateTimeOfCrawl";
                    string CSV_Content = TypeOfProfile + "," + LDS_UserProfileLink + "," + firstname + "," + lastname + "," + postalCode + "," + distance + "," + Company.Replace(",", ";") + "," + titlecurrent.Replace(",", ";") + "," + companycurrent.Replace(",", ";") + "," + Connection.Replace(",", ";") + "," + recomandation.Replace(",", string.Empty) + "," + Skill.Replace(",", ";") + "," + LDS_Experience.Replace(",", string.Empty) + "," + EducationCollection.Replace(",", ";") + "," + groupscollectin.Replace(",", ";") + "," + USERemail.Replace(",", ";") + "," + LDS_UserContact.Replace(",", ";") + "," + LDS_PastTitles + "," + AllComapny.Replace(",", ";") + "," + location.Replace(",", ";") + "," + state.Replace(",", ";") + "," + country.Replace(",", ";") + "," + Industry.Replace(",", ";") + "," + Website.Replace(",", ";") + "," + accountUser + "," + DateTime.Now;// +TypeOfProfile + ",";

                    if (SelectedMode == "Mode1")
                    {
                        CSVUtilities.ExportDataCSVFile(CSVHeader, CSV_Content, Globals.path_InBoardProGetDataResultLastNameWiseData);
                    }
                    else if (SelectedMode == "Mode2")
                    {
                        CSVUtilities.ExportDataCSVFile(CSVHeader, CSV_Content, Globals.path_InBoardProGetDataResultIndustryZoneWiseData);
                    }
                    else if (SelectedMode == "Mode3")
                    {
                        CSVUtilities.ExportDataCSVFile(CSVHeader, CSV_Content, Globals.path_InBoardProGetDataResultZipCodeWiseData);
                    }

                    Log("[ " + DateTime.Now + " ] => [ " + "Data Saved In CSV File ! ]");
                }
                catch { }
            }

            #endregion
        }
        #endregion

        #region Log
        private void Log(string message)
        {
            EventsArgs eventArgs = new EventsArgs(message);
            InBoardProGetDataLogEvents.LogText(eventArgs);
        }
        #endregion
    }
}
