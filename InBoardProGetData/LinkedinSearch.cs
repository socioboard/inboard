using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InBoardPro;
using System.Text.RegularExpressions;
using BaseLib;
using Chilkat;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading;


namespace InBoardProGetData
{
  public class LinkedinSearch
    {
        #region Global declaration
        ChilkatHttpHelpr objChilkat = new ChilkatHttpHelpr();

        public static Events loggersearch = new Events();
        public static Events loggerscrap = new Events();
        
        string userName = string.Empty;
        string password = string.Empty;

        string proxyAddress = string.Empty;
        string proxyPort = string.Empty;
        string proxyUsername = string.Empty;
        string proxyPassword = string.Empty;

        public static string _Keyword = string.Empty;
        public static string _Search = string.Empty;

        public bool _RdbKeyword = false;
        public bool _RdbURL = false;

        public static List<string> lstLinkedinSearchProfileURL = new List<string>();
        List<string> RecordURL = new List<string>();

        string ResponceDeltaPost1 = string.Empty;
        string ResponseWallPostForPremiumAcc = string.Empty;
        string PostResponce = string.Empty;
        string PostRequestURL = string.Empty;
        string PostdataForPagination = string.Empty;
        string csrfToken = string.Empty;
        string csrfToken1 = string.Empty;
        string FollowingName = string.Empty;
        string FollowingOccupation = string.Empty;
        public bool _RdbCompanyEmployeeSearchKeyword = false;
        public bool _RdbCompanyEmployeeSearchURL = false;
        public static List<string> listCompanyURL = new List<string>();
        #endregion

        #region LinkedinSearch
        public LinkedinSearch()
        {
        } 
        #endregion

        #region LinkedinSearch with param signature
        public LinkedinSearch(string UserName, string Password, string ProxyAddress, string ProxyPort, string ProxyUserName, string ProxyPassword)
        {
            userName = UserName;
            password = Password;
            proxyAddress = ProxyAddress;
            proxyPort = ProxyPort;
            proxyUsername = ProxyUserName;
            proxyPassword = ProxyPassword;
        } 
        #endregion

        #region StartLinkedInSearch
        public void StartLinkedInSearch(ref GlobusHttpHelper HttpHelper)
        {
            if (Globals.IsStop_CompanyEmployeeScraperThread)
            {
                return;
            }

            Globals.lstCompanyEmployeeScraperThread.Add(Thread.CurrentThread);
            Globals.lstCompanyEmployeeScraperThread = Globals.lstCompanyEmployeeScraperThread.Distinct().ToList();
            Thread.CurrentThread.IsBackground = true;

            try
            {
                if (_RdbKeyword)
                {
                    if (_Search.Contains("People"))
                    {
                        try
                        {
                            SearchByPeople(ref HttpHelper);
                        }
                        catch
                        {
                        }
                    }

                    if (_Search.Contains("Companies"))
                    {
                        try
                        {
                            SearchByCompany(ref HttpHelper);
                        }
                        catch
                        {
                        }
                    }
                  
                }

                if (_RdbURL)
                {
                    if (lstLinkedinSearchProfileURL.Count > 0)
                    {
                        try
                        {                           
                            //LinkedinLogin Login = new LinkedinLogin();
                            //Login.logger.addToLogger += new EventHandler(logger_addSearchToLogger);
                            //Login.LoginHttpHelper(ref HttpHelper, userName, password, proxyAddress, proxyUsername, proxyPassword, proxyPort);
                            if (!Globals.scrapeWithoutLoggingIn)
                            {
                                LinkedinLogin Login = new LinkedinLogin();
                                Login.accountUser = userName;
                                Login.accountPass = password;
                                Login.proxyAddress = proxyAddress;
                                Login.proxyUserName = proxyUsername;
                                Login.proxyPassword = proxyPassword;
                                Login.proxyPort = proxyPort;

                                Login.logger.addToLogger += new EventHandler(logger_addSearchToLogger);

                                if (!Login.IsLoggedIn)
                                {
                                    Login.LoginHttpHelper(ref HttpHelper);

                                }

                                if (Login.IsLoggedIn)
                                {
                                    GetProfileData(ref HttpHelper);
                                }
                            }
                            else
                            {
                                GetProfileData(ref HttpHelper);
                            }
                           
                        }
                        catch
                        {
                        }
                    }
                }

                if (_Search.Contains("CompaniesWithFilter"))
                {
                    try
                    {
                        LinkedinLogin Login = new LinkedinLogin();
                        Login.accountUser = userName;
                        Login.accountPass = password;
                        Login.proxyAddress = proxyAddress;
                        Login.proxyUserName = proxyUsername;
                        Login.proxyPassword = proxyPassword;
                        Login.proxyPort = proxyPort;

                        Login.logger.addToLogger += new EventHandler(logger_addSearchToLogger);

                        if (!Login.IsLoggedIn)
                        {
                            Login.LoginHttpHelper(ref HttpHelper);

                        }

                        // Call function Companies
                        SearchByCompanyWithFilter(ref HttpHelper);
                    }
                    catch
                    {
                    }
                }

                if (_Search.Contains("CompanyEmployeeSearch"))
                {
                    try
                    {
                        LinkedinLogin Login = new LinkedinLogin();
                        Login.accountUser = userName;
                        Login.accountPass = password;
                        Login.proxyAddress = proxyAddress;
                        Login.proxyUserName = proxyUsername;
                        Login.proxyPassword = proxyPassword;
                        Login.proxyPort = proxyPort;

                        Login.logger.addToLogger += new EventHandler(logger_addSearchToLogger);

                    
                        if (_RdbCompanyEmployeeSearchKeyword)
                        {
                            try
                            {
                                SearchByCompanyWithFilter(ref HttpHelper);
                            }
                            catch
                            { }
                        }

                        if (_RdbCompanyEmployeeSearchURL)
                        {
                            
                            GetEmployeeDataFromCompanyURL(ref HttpHelper, listCompanyURL);
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
        } 
        #endregion

        #region SearchByPeople
        private void SearchByPeople(ref GlobusHttpHelper HttpHelper)
        {
            try
            {
                Log("[ " + DateTime.Now + " ] => [ Starting Search By People With UserName : " + userName + " ]");

                _Search = _Search.ToLower();
                _Keyword = _Keyword.ToLower();
                string Facets_Order = string.Empty;
                //if (SearchCriteria.starter)
                {
                    #region Serch

                    //For Data postURL
                    string pageSourceaAdvanceSearch = string.Empty;
                    try
                    {
                        pageSourceaAdvanceSearch = HttpHelper.getHtmlfromUrl1(new Uri("https://www.linkedin.com/vsearch/f?adv=true&trk=federated_advs"));

                        if (pageSourceaAdvanceSearch == "" || pageSourceaAdvanceSearch.Contains("Make sure you have cookies and Javascript enabled in your browser before signing in."))
                        {
                            pageSourceaAdvanceSearch = HttpHelper.getHtmlfromUrl(new Uri("https://www.linkedin.com/vsearch/f?adv=true&trk=federated_advs"));
                        }

                        try
                        {
                            int startindex = pageSourceaAdvanceSearch.IndexOf("csrfToken=");
                            string start = pageSourceaAdvanceSearch.Substring(startindex).Replace("csrfToken=", "");
                            int endindex = start.IndexOf("\">");
                            string end = start.Substring(0, endindex);
                            csrfToken = end;
                        }
                        catch { }

                        string PplsearchOrigin = string.Empty;
                        try
                        {
                            PplsearchOrigin = pageSourceaAdvanceSearch.Substring(pageSourceaAdvanceSearch.IndexOf("pplSearchOrigin"), 200);

                            string[] Arr2 = PplsearchOrigin.Split('"');
                            PplsearchOrigin = Arr2[4];
                            PplsearchOrigin = PplsearchOrigin.Replace(":", "%3A");
                        }
                        catch (Exception ex)
                        {

                        }
                        string tempsorceurl2 = "csrfToken=" + csrfToken1 + "&keepFacets=true&pplSearchOrigin=ADVS&keywords=&fname" + SearchCriteria.FirstName + "=&lname=" + SearchCriteria.LastName + "&searchLocationType=" + SearchCriteria.Location + "&countryCode=" + SearchCriteria.Country + "&postalCode=&title=" + SearchCriteria.Title + "&currentTitle=CP&company=" + SearchCriteria.Company + "&currentCompany=CP&school=&I=" + SearchCriteria.IndustryType + "select-all&FG=" + SearchCriteria.Group + "select-all&facet_N=F&L=select-all&sortCriteria=C&viewCriteria=2&%2Fsearch%2Ffpsearch=Search";
                        string tempsorceurl1 = "csrfToken=" + csrfToken1 + "&keepFacets=true&pplSearchOrigin=ADVS&keywords=" + SearchCriteria.Keyword + "&fname=" + SearchCriteria.FirstName + "&lname=" + SearchCriteria.LastName + "&searchLocationType=" + SearchCriteria.Location + "&countryCode=" + SearchCriteria.Country + "&postalCode=" + SearchCriteria.PostalCode + "&title=" + SearchCriteria.Title + "&company=" + SearchCriteria.Company + "&school=" + SearchCriteria.Education + "&%2Fsearch%2Ffpsearch=Search&I=select-all&N=select-all&L=select-all&CS=select-all&SE=select-all&P=select-all&F=select-all&sortCriteria=R&viewCriteria=1";

                        string Search = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/search?trk=advsrch"));
                    }
                    catch
                    {
                    }
                    if (_Keyword.Contains("or")||(_Keyword.Contains("and"))||(_Keyword.Contains("not")))
                    {
                        try
                        {
                            _Keyword = _Keyword.Replace(" or "," OR ");
                            _Keyword = _Keyword.Replace(" and ", " AND ");
                            _Keyword = _Keyword.Replace(" not ", " NOT ");
                        }
                        catch{}
                    }
                    try
                    {
                        ResponseWallPostForPremiumAcc = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/search/fpsearch?type=people&keywords=" + _Keyword + "&pplSearchOrigin=GLHD&pageKey=fps_results"));

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
                string[] Arr12 = Regex.Split(ResponseWallPostForPremiumAcc, "<p class=\"summary\">");

                if (Arr12.Count() == 1)
                {
                    Arr12 = Regex.Split(ResponseWallPostForPremiumAcc, "resultCount");  //formattedResultCount
                }

                foreach (string item in Arr12)
                {
                    try
                    {
                        if (!item.Contains("<!DOCTYPE"))
                        {
                            if (item.Contains("<strong>"))
                            {
                                try
                                {
                                    string pageNO = item.Substring(item.IndexOf("<strong>"), item.IndexOf("</strong>", item.IndexOf("<strong>")) - item.IndexOf("<strong>")).Replace("<strong>", string.Empty).Replace(",", string.Empty).Trim();

                                    string[] arrPageNO = Regex.Split(pageNO, "[^0-9]");

                                    foreach (string item1 in arrPageNO)
                                    {
                                        try
                                        {
                                            if (!string.IsNullOrEmpty(item1))
                                            {
                                                strPageNumber = item1;
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
                            }
                            else
                            {
                                try
                                {
                                    string pageNO = item.Substring(item.IndexOf("<p class=\"summary\">"), item.IndexOf("</p>", item.IndexOf("<p class=\"summary\">")) - item.IndexOf("<p class=\"summary\">")).Replace("<p class=\"summary\">", string.Empty).Replace(",", string.Empty).Trim();
                                    string[] arrPageNO = Regex.Split(pageNO, "[^0-9]");

                                    foreach (string item1 in arrPageNO)
                                    {
                                        try
                                        {
                                            if (!string.IsNullOrEmpty(item1))
                                            {
                                                strPageNumber = item1;
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
                            }
                        }
                    }
                    catch
                    {
                    }
                }

                foreach (string item in Arr12)
                {
                    try
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
                    catch
                    {
                    }
                }

                if (strPageNumber == string.Empty)
                {
                    foreach (string item in Arr12)
                    {
                        try
                        {

                            #region Loop
                            if (!item.Contains("<!DOCTYPE"))
                            {
                                if (item.Contains(":"))
                                {
                                    string data = RemoveAllHtmlTag.StripHTML(item);
                                    data = data.Replace("\n", "");
                                    if (data.Contains(">"))
                                    {
                                        string[] ArrTemp = Regex.Split(data,"primaryUrlAlias");
                                        data = ArrTemp[0];
                                        data = data.Replace(":", string.Empty);
                                        data = data.Trim();
                                        string[] ArrTemp1 = data.Split(' ');
                                        if (ArrTemp1[0].Contains(","))
                                        {
                                            data = ArrTemp1[0].Split(',')[0];
                                        }
                                        else
                                        {
                                            data = ArrTemp1[0].Replace(',', ' ').Trim();
                                        }


                                        strPageNumber = data.Replace(" ", string.Empty).Replace("\"", string.Empty).Replace("sorry_no_sim_job__i18n", string.Empty).Replace("i18n_author_contentPosts", string.Empty).Replace("i18n_survey_feedback_thanksThanks!", string.Empty).Replace("i18n_survey_feedback_thanks¡Gracias!", string.Empty).Replace(".", "").Trim();
                                        break;
                                    }

                                }
                            }
                            #endregion

                        }
                        catch
                        {
                        }
                    }
                }

                try
                {
                    strPageNumber = strPageNumber.Replace(".", string.Empty);
                    if (strPageNumber != string.Empty || strPageNumber == "0")
                    {
                        Log("[ " + DateTime.Now + " ] => [ Total Results found: " + strPageNumber + " ]");
                    }
                    pagenumber = int.Parse(strPageNumber);
                }
                catch (Exception)
                {

                }
                pagenumber = (pagenumber / 10) + 1;

                string GetResponce = string.Empty;
                string GetRequestURL = string.Empty;

                if (pagenumber >= 1)
                {
                    for (int i = 1; i <= pagenumber; i++)
                    {
                        //if (SearchCriteria.starter)
                        {

                            #region loop

                            if (ResponseWallPostForPremiumAcc.Contains("Account Type:</span> Basic"))
                            {
                                try
                                {
                                    PostRequestURL = "http://www.linkedin.com/search/hits";
                                    PostdataForPagination = "keywords=" + _Keyword + "&searchLocationType=Y&search=&pplSearchOrigin=GLHD&viewCriteria=2&sortCriteria=R&facetsOrder=CC%2CN%2CG%2CI%2CPC%2CED%2CL%2CFG%2CTE%2CFA%2CSE%2CP%2CCS%2CF%2CDR&page_num=" + i + "&openFacets=CC%2CN%2CG";//"fname=" + SearchCriteria.FirstName + "&lname=" + SearchCriteria.LastName + "&searchLocationType=" + SearchCriteria.Location + "&countryCode=" + SearchCriteria.Country + "&viewCriteria=1&sortCriteria=R&facetsOrder=CC%2CN%2CG%2CI%2CPC%2CED%2CL%2CFG%2CTE%2CFA%2CSE%2CP%2CCS%2CF%2CDR&page_num=" + i + "&openFacets=N%2CCC%2CG";
                                    PostResponce = HttpHelper.postFormData(new Uri(PostRequestURL), PostdataForPagination);
                                }
                                catch { }
                            }
                            else if (ResponseWallPostForPremiumAcc.Contains("Account: Basic") || ResponseWallPostForPremiumAcc.Contains("Cuenta: Básica"))
                            {
                                try
                                {
                                    PostRequestURL = "http://www.linkedin.com/search/fpsearch?";
                                    PostdataForPagination = "type=people&keywords="+_Keyword+"&page_num="+i;
                                    PostResponce = HttpHelper.postFormData(new Uri(PostRequestURL),PostdataForPagination);
                                }
                                catch { }

                            }

                            else if (ResponseWallPostForPremiumAcc.Contains("Account Type:</span> Executive"))
                            {
                                try
                                {
                                    PostRequestURL = "http://www.linkedin.com/search/hits";
                                    PostdataForPagination = "type=people&keywords=" + _Keyword + "&page_num=" + i;
                                    PostResponce = HttpHelper.postFormData(new Uri(PostRequestURL), PostdataForPagination);
                                }
                                catch { }

                            }
                            else
                            {
                                try
                                {
                                    
                                    PostRequestURL = "http://www.linkedin.com/search/fpsearch?";
                                    PostdataForPagination = "type=people&keywords=" + _Keyword + "&page_num=" + i;
                                    PostResponce = HttpHelper.postFormData(new Uri(PostRequestURL), PostdataForPagination);
                                }
                                catch { }
                            }


                            if (PostResponce.Contains("/profile/view?id"))
                            {
                                string[] URL = Regex.Split(PostResponce, "profile/view");
                                List<string> PageSerchUrl1 = URL.ToList<string>();
                                //PageSerchUrl = PageSerchUrl.Distinct().ToList();

                                List<string> PageSerchUrl = ChilkatBasedRegex.GettingAllUrls(PostResponce, "profile/view?id");

                                string[] profileUrl = Regex.Split(PostResponce, "linkAuto_voltron_people_search_1");

                                {
                                    foreach (string item in profileUrl)
                                    {
                                        if (!(item.Contains("<!DOCTYPE html>")))
                                        {
                                        try
                                        {
                                            int startindex = item.IndexOf("pid=");
                                            string start = item.Substring(startindex).Replace("pid=", string.Empty);
                                            int endindex = start.IndexOf(",");
                                            string end = start.Substring(0, endindex).Replace(",", string.Empty).Trim();
                                            string basicUrl = end;
                                            string modifiedProfUrl = "https://www.linkedin.com/profile/view?id=" + basicUrl;
                                            string urlSerch = modifiedProfUrl;
                                            Log("[ " + DateTime.Now + " ] => [ " + urlSerch + " ]");
                                            RecordURL.Add(urlSerch);
                                        }
                                        catch
                                        { }
                                        }
                                    }
                                }



                                //if (PageSerchUrl.Count == 0)
                                //{

                                //    Log("[ " + DateTime.Now + " ] => [ On the basis of you Account you can able to see " + RecordURL.Count + "Results ]");
                                //    break;
                                //}
                                //foreach (string item in PageSerchUrl)
                                //{
                                //    try
                                //    {
                                //        //if (SearchCriteria.starter)
                                //        {
                                //            if (item.Contains("authType="))
                                //            {

                                //                string modifiedProfUrl = item;

                                //                // string urlSerch = "http://www.linkedin.com" + modifiedProfUrl;
                                //                string urlSerch = modifiedProfUrl;
                                //                Log("[ " + DateTime.Now + " ] => [ " + urlSerch + " ]");
                                //                RecordURL.Add(urlSerch);
                                //            }
                                //        }
                                //    }
                                //    catch
                                //    {
                                //    }
                                //}
                            }

                            //else if (!PostResponce.Contains("authType=") && PostResponce.Contains("Custom views are no longer supported. Please select either Basic or Expanded view"))
                            //{
                            //    break;
                            //}

                            if (!PostResponce.Contains("authType=") && (PostResponce.Contains("Upgrade your account to see more results") || (PostResponce.Contains("Abónate para ver más resultados"))))
                            {
                                break;
                            }

                            #endregion

                        }

                    }

                }

                #region For Else
                else
                {

                    //if (SearchCriteria.starter)
                    {

                        #region loop
                        if (ResponseWallPostForPremiumAcc.Contains("/profile/view?id"))
                        {

                            List<string> PageSerchUrl = ChilkatBasedRegex.GettingAllUrls(ResponseWallPostForPremiumAcc, "profile/view?id");
                            if (PageSerchUrl.Count == 0)
                            {

                                Log("On the basis of your Account or Your Input you can able to see " + RecordURL.Count + "  Results");

                            }
                            foreach (string item in PageSerchUrl)
                            {
                                //if (SearchCriteria.starter)
                                {
                                    if (item.Contains("authType="))
                                    {


                                        string urlSerch = "http://www.linkedin.com" + item;
                                        Log(urlSerch);
                                        RecordURL.Add(urlSerch);
                                    }
                                }
                            }
                        }

                        #endregion

                    }

                }

                #endregion
                RecordURL.Distinct();

                //Log("Toatl Find URL >>> " + RecordURL.Count.ToString());
                //if (SearchCriteria.starter)
                {

                    forUrl(ref HttpHelper);
                    //new Thread(() =>
                    //{
                    //    test();
                    //}).Start();
                }
                               
            }

            catch
            {
            }
        } 
        #endregion

        #region forUrl
        private void forUrl(ref GlobusHttpHelper HttpHelper)
        {
            try
            {
                string status = "LinkedinSearch_People";
                List<string> numburlpp = new List<string>();

                // if (SearchCriteria.starter)
                {
                    ////RecordURL = RecordURL.Distinct().ToList();
                    //RecordURL.ForEach(s =>
                    //{
                    //    if (s.Contains("&"))
                    //    {
                    //        s.Remove(s.IndexOf("&"));
                    //    }
                    //});
                    RecordURL = RecordURL.Distinct().ToList();

                    List<string> lstRecordURLs = new List<string>();
                    foreach (string item in RecordURL)
                    {
                        try
                        {
                            if (item.Contains("&") && item.Contains("view?id="))
                            {
                                try
                                {
                                    string viewid = item.Substring(0, item.LastIndexOf("&")).Replace("&pvs=ps", string.Empty);
                                    lstRecordURLs.Add(viewid);
                                }
                                catch
                                {
                                }
                            }
                            else
                            {
                                lstRecordURLs.Add(item);
                            }
                        }
                        catch
                        {

                        }
                    }

                    lstRecordURLs = lstRecordURLs.Distinct().ToList();
                    Log("[ " + DateTime.Now + " ] => [ Total Find URL : " + RecordURL.Count.ToString() + " ]");

                    foreach (string url_item in lstRecordURLs)
                    {
                        try
                        {
                            Log("[ " + DateTime.Now + " ] => [ Fetching Data From URL : " + url_item + " ]");

                            if (!CrawlingLinkedInPage(url_item, ref HttpHelper, status))
                            {
                                CrawlingPageDataSource(url_item, ref HttpHelper, status);
                            }

                        }
                        catch
                        {
                        }
                    }

                }
            }
            catch (Exception ex)
            {

            }

            //Log("Process Completed With UserName >>> " + userName);
        } 
        #endregion

        #region CrawlingLinkedInPage
        public bool CrawlingLinkedInPage(string Url, ref GlobusHttpHelper HttpHelper, string status)
        {
            Log("[ " + DateTime.Now + " ] => [ Parsing For URL : " + Url + " ]");

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
            string memhead = string.Empty;
            string website1 = string.Empty;
            string web1 = string.Empty;
            string degreeConnection = string.Empty;
            string ImageUrl = string.Empty;
            string Follower = string.Empty;
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
            string LDS_HeadLineTitle = string.Empty;
            string LDS_CurrentCompany = string.Empty;

            string Company = string.Empty;
            List<string> lstpasttitle = new List<string>();
            List<string> checkpasttitle = new List<string>();
            List<string> titleList = new List<string>();
            List<string> companyList = new List<string>();
            #endregion

            if (!Url.Contains("http"))
            {
                Url = "http://" + Url;
            }

           string stringSource = HttpHelper.getHtmlfromUrl1(new Uri(Url));

           if (stringSource == "")
           {
               stringSource = HttpHelper.getHtmlfromUrl1(new Uri(Url));
           }

           #region ImageURL
           try
           {
               int startindex = stringSource.IndexOf("\"img_raw\":\"");
               string start = stringSource.Substring(startindex).Replace("\"img_raw\":\"", string.Empty);
               int endindex = start.IndexOf("\",\"");
               string end = start.Substring(0, endindex).Replace("\",\"", string.Empty);
               ImageUrl = end.Trim();
           }
           catch
           { }


           #endregion ImageURL

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
                    //strFamilyName = stringSource.Substring(stringSource.IndexOf("fmt__profileUserFullName\":\""), (stringSource.IndexOf("i18n__expand_your_network_to_see_more", stringSource.IndexOf("\"See Full Name\",\"i18n_Edit\":\"Edit\",\"fmt__full_name\":\"")) - stringSource.IndexOf("\"See Full Name\",\"i18n_Edit\":\"Edit\",\"fmt__full_name\":\""))).Replace("\"See Full Name\",\"i18n_Edit\":\"Edit\",\"fmt__full_name\":\"", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace("}", string.Empty).Replace("]", string.Empty).Trim();
                    int startindex = stringSource.IndexOf("fmt__profileUserFullName\":\"");
                    string start = stringSource.Substring(startindex).Replace("fmt__profileUserFullName\":\"", "");
                    int endindex = start.IndexOf("\",\"");
                    string end = start.Substring(0, endindex).Replace("\\u002d", "-");
                    strFamilyName = end;
                    if (string.IsNullOrEmpty(strFamilyName))
                    {
                        strFamilyName = stringSource.Substring(stringSource.IndexOf("fmt__full_name\":"), (stringSource.IndexOf(",", stringSource.IndexOf("fmt__full_name\":")) - stringSource.IndexOf("fmt__full_name\":"))).Replace("fmt__full_name\":", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace("}", string.Empty).Replace("]", string.Empty).Trim();
                    }
                }
                catch
                {
                    try
                    {
                        strFamilyName = stringSource.Substring(stringSource.IndexOf("fmt__full_name\":"), (stringSource.IndexOf(",", stringSource.IndexOf("fmt__full_name\":")) - stringSource.IndexOf("fmt__full_name\":"))).Replace("fmt__full_name\":", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace("}", string.Empty).Replace("]", string.Empty).Trim();

                    }
                    catch { }
                }
            }
            catch { }

            if (strFamilyName == string.Empty)
            {
                try
                {
                    int startindex = stringSource.IndexOf("<title>");
                    string start = stringSource.Substring(startindex).Replace("<title>", string.Empty);
                    int endindex = start.IndexOf("</title>");
                    string end = start.Substring(0, endindex).Replace("\\u002d", "-").Replace("| LinkedIn", string.Empty).Trim();
                    strFamilyName = end;
                }
                catch { }
            }

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
                    lastname = NameArr[1] + " " + NameArr[2];
                }
                else if (NameArr.Count() == 4)
                {
                    lastname = NameArr[1] + " " + NameArr[2] + " " + NameArr[3];
                }
                else if (NameArr.Count() == 5)
                {
                    lastname = NameArr[1] + " " + NameArr[2] + " " + NameArr[3] + " " + NameArr[4];
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

            #region company
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
                if (string.IsNullOrEmpty(Company))
                {
                    try
                    {
                        Company = stringSource.Substring(stringSource.IndexOf("class=\"title\">"), (stringSource.IndexOf("</p></div></div><div class=\"demographic-info adr editable-item\" id=\"demographics\">", stringSource.IndexOf("</p></div></div><div class=\"demographic-info adr editable-item\" id=\"demographics\">")) - stringSource.IndexOf("class=\"title\">"))).Replace("class=\"title\">", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace("<strong class=highlight>", string.Empty).Replace("</strong>", string.Empty).Trim();
                    }
                    catch { }
                }
            }


            string[] strdesigandcompany_temp = new string[4];
            if (Company.Contains(" at "))
            {
                try
                {
                    strdesigandcompany_temp = Regex.Split(Company, " at ");
                }
                catch { }

                #region Title
                try
                {
                    titlecurrent = strdesigandcompany_temp[0];
                }
                catch { }
                #endregion

                #region Current Company
                try
                {
                    companycurrent = strdesigandcompany_temp[1];
                }
                catch { }
                #endregion
            }
            #endregion


            #region titlecurrent
            try
            {
             try
                {
                    try
                    {
                        
                        int startindex = stringSource.IndexOf("\"title_highlight\":\"");
                        string start = stringSource.Substring(startindex).Replace("\"title_highlight\":\"", "");
                        int endindex = start.IndexOf("\",\"");
                        string end = start.Substring(0, endindex).Replace("\u002d", string.Empty).Replace("Å", "A").Replace("\\u002d", "-").Replace("\"\n", "").Replace("\n", "").Replace(";", ",").Replace("&amp", "");
                        titlecurrent = end; 
                    }
                    catch
                    {
                    }

                //    if (string.IsNullOrEmpty(Company))
                    if (string.IsNullOrEmpty(titlecurrent))
                    {
                        try
                        {
                            //memberHeadline
                          //  titlecurrent = stringSource.Substring(stringSource.IndexOf("memberHeadline\":"), (stringSource.IndexOf(",", stringSource.IndexOf("memberHeadline\":")) - stringSource.IndexOf("memberHeadline\":"))).Replace("memberHeadline\":", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Replace("   ", string.Empty).Replace(":", "").Replace("&dsh;", "").Replace("&amp", "").Replace(";", "").Replace("u002d",string.Empty).Trim();

                            int startindex = stringSource.IndexOf("memberHeadline");
                            string start = stringSource.Substring(startindex).Replace("memberHeadline", "");
                            int endindex = start.IndexOf("\",\"");
                            string end = start.Substring(0, endindex).Replace("\"", "").Replace(":", "");
                            memhead = end.Replace("\\u002d", "-");
                        }
                        catch
                        {
                        }

                    }
                    if (string.IsNullOrEmpty(titlecurrent))
                    {
                        string[] cmpny = Regex.Split(stringSource, "trk=prof-exp-title' name='title' title='Find others with this title'>");
                        foreach(string item in cmpny)
                        {
                            try
                            {
                                try
                                {
                                    if (!item.Contains("<!DOCTYPE html>"))
                                    {
                                        int startindex = item.IndexOf("");
                                        string start = item.Substring(startindex);
                                        int endindex = start.IndexOf("</a>");
                                        string end = start.Substring(0, endindex).Replace("</a>", String.Empty);
                                        string titles = end.Trim();
                                        titleList.Add(titles);
                                        titleList = titleList.Distinct().ToList();
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
                    }
                    if (titleList.Count > 0)
                    {
                        titlecurrent = titleList[0];
                    }
                    string[] strdesigandcompany1 = new string[4];
                    if (memhead.Contains(" at ") || memhead.Contains(" of "))
                    {
                        // titlecurrent = string.Empty;
                        companycurrent = string.Empty;

                        try
                        {
                            strdesigandcompany1 = Regex.Split(memhead, " at ");

                            if (strdesigandcompany1.Count() == 1)
                            {
                                strdesigandcompany1 = Regex.Split(memhead, " of ");
                            }
                        }
                        catch { }

                        try
                        {
                            titlecurrent = strdesigandcompany1[0];
                        }
                        catch { }

                    }

                    
                    string[] strdesigandcompany = new string[4];
                    if (Company.Contains(" at ") || Company.Contains(" of "))
                    {
                        titlecurrent = string.Empty;
                        companycurrent = string.Empty;

                        try
                        {
                            strdesigandcompany = Regex.Split(Company, " at ");

                            if (strdesigandcompany.Count() == 1)
                            {
                                strdesigandcompany = Regex.Split(Company, " of ");
                            }
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

                    if (string.IsNullOrEmpty(memhead))
                    {
                        int startindex = stringSource.IndexOf("title \">");
                        string start = stringSource.Substring(startindex).Replace("title\":", "").Replace("title \">","");
                        int endindex = start.IndexOf("</p>");
                        string end = start.Substring(0, endindex).Replace("\u002d", string.Empty).Replace("Å", "A").Replace("\\u002d", "-").Replace("\"\n", "").Replace("\n", "").Replace(";", ",").Replace("&amp", "");
                        titlecurrent = end;
                        //memhead = titlecurrent;

                    }


                }
                catch { }

                #region PastCompany
                string[] companylist = Regex.Split(stringSource, "companyName\"");
                string AllComapny = string.Empty;

                string Companyname = string.Empty;
                foreach (string item in companylist)
                {
                    try
                    {
                        if (!item.Contains("<!DOCTYPE html>"))
                        {
                            Companyname = item.Substring(item.IndexOf(":"), (item.IndexOf(",", item.IndexOf(":")) - item.IndexOf(":"))).Replace(":", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Replace("}", string.Empty).Replace("]", string.Empty).Trim();
                            //Checklist.Add(item);
                            string items = item;
                            checkerlst.Add(Companyname);
                            checkerlst = checkerlst.Distinct().ToList();
                        }
                    }
                    catch { }
                }
                
               // string AllComapny = string.Empty;
                if (checkerlst.Count > 0)
                {
                    foreach (string item1 in checkerlst)
                    {
                        try
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
                        catch
                        {


                        }
                    }
                }
                if (string.IsNullOrEmpty(AllComapny))
                {
                    string[] allCompany = Regex.Split(stringSource,"trk=prof-exp-company-name\" name=\"company\" title=\"Find others who have worked at this company\">");
                    foreach (string item in allCompany)
                    {
                        if (!item.Contains("<!DOCTYPE html>"))
                        {
                            try
                            {
                                int startindex = item.IndexOf("");
                                string start = item.Substring(startindex);
                                int endindex = start.IndexOf("</a>");
                                string end = start.Substring(0, endindex).Replace("</a>", string.Empty);
                                string companies = end.Trim();
                                companyList.Add(companies);
                                companyList = companyList.Distinct().ToList();
                            }
                            catch 
                            {                              
                               
                            }
                        }

                    }
                    string[] allcompany1 = Regex.Split(stringSource, "trk=prof-exp-company-name\">");
                    foreach (string item in allcompany1)
                    {
                        if (!item.Contains("<!DOCTYPE html>") && !item.Contains("<span data-tracking=\"mcp_profile_sum\""))
                        {
                            try
                            {
                                int startindex = item.IndexOf("");
                                string start = item.Substring(startindex);
                                int endindex = start.IndexOf("</a>");
                                string end = start.Substring(0, endindex).Replace("</a>", string.Empty);
                                string companies = end.Replace("&amp", "&").Trim();
                                companyList.Add(companies);
                                companyList = companyList.Distinct().ToList();
                            }
                            catch 
                            {
                                
                            }
                        }
                    }

                    if (string.IsNullOrEmpty(AllComapny))
                    {
                        allcompany1 = Regex.Split(stringSource, "class=\"editable-item section-item past-position\"");

                        foreach (string item in allcompany1)
                        {
                            if (!item.Contains("<!DOCTYPE html>"))
                            {
                                try
                                {
                                    int startindex = item.IndexOf("dir='auto'>");
                                    string start = item.Substring(startindex).Replace("dir='auto'>", "");
                                    int endindex = start.IndexOf("</a>");
                                    string end = start.Substring(0, endindex).Replace("</a>", string.Empty);
                                    string companies = end.Replace("&amp", "&").Trim();
                                    companyList.Add(companies);
                                    companyList = companyList.Distinct().ToList();
                                }
                                catch
                                {

                                }
                            }
                        }
                    }

                    if (companyList.Count > 0)
                    {
                        foreach (string item1 in companyList)
                        {
                            try
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
                            catch
                            {


                            }
                        }
                    }

                }
                if (companyList.Count > 0)
                {
                    companycurrent = companyList[0];
                }
            
     //           if (companycurrent == string.Empty)
     //           {
     //               try
     //               {
       //                 companycurrent = checkerlst[0].ToString();
       //             }
       //             catch { }
        //        }

                #endregion
               #endregion Company

            #region Education
                try
                {
                  //  string[] str_UniversityName = Regex.Split(stringSource, "link__school_name");
                    string[] str_UniversityName = Regex.Split(stringSource, "link__school_name_public");
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
                                        int startindex = item.IndexOf("\"schoolName\":");
                                        string start = item.Substring(startindex).Replace("\"schoolName\":", string.Empty);
                                        int endindex = start.IndexOf(",");
                                        School = start.Substring(0, endindex).Replace("\\u002d", string.Empty).Replace(":", string.Empty).Replace("\"", string.Empty).Replace("\\t", string.Empty);
                                    }
                                    catch { }

                                    try
                                    {
                                        int startindex1 = item.IndexOf("degree");
                                        string start1 = item.Substring(startindex1).Replace("degree", "");
                                        int endindex1 = start1.IndexOf(",");
                                        Degree = start1.Substring(0, endindex1).Replace("\\u002d", string.Empty).Replace(":", string.Empty).Replace("\"", string.Empty).Replace("_highlight",string.Empty);
                                    }
                                    catch { }

                                    try
                                    {
                                        int startindex2 = item.IndexOf("enddate_my");
                                        string start2 = item.Substring(startindex2).Replace("enddate_my", "");
                                        int endindex2 = start2.IndexOf(",");
                                        SessionEnd = start2.Substring(0, endindex2).Replace("\\u002d", string.Empty).Replace(":", string.Empty).Replace("\"", string.Empty).Replace("}",string.Empty);
                                    }
                                    catch { }

                                    try
                                    {
                                        int startindex3 = item.IndexOf("startdate_my");
                                        string start3 = item.Substring(startindex3).Replace("startdate_my", "");
                                        int endindex3 = start3.IndexOf(",");
                                        SessionStart = start3.Substring(0, endindex3).Replace("\\u002d", string.Empty).Replace(":", string.Empty).Replace("\"", string.Empty).Replace("}",string.Empty);
                                    }
                                    catch { }
                                    //if (Degree == string.Empty)
                                    //{
                                    //    try
                                    //    {
                                    //        if (!item.Contains("\"degree\":\""))
                                    //        {
                                    //            int startindex4 = item.IndexOf("name\":");
                                    //            string start4 = item.Substring(startindex4).Replace("name\":", "");
                                    //            int endindex4 = start4.IndexOf(",");
                                    //            Degree = start4.Substring(0, endindex4).Replace("\\u002d", string.Empty).Replace(":", string.Empty).Replace("\"", string.Empty);
                                    //        }
                                    //    }
                                    //    catch { }
                                    //}

                                    if (SessionStart == string.Empty && SessionEnd == string.Empty && !string.IsNullOrEmpty(Degree))
                                    {
                                        Education = " [" + School + " Degree: " + Degree + " ]";
                                    }
                                    else if (Degree == string.Empty && !string.IsNullOrEmpty(SessionStart) && !string.IsNullOrEmpty(SessionEnd))
                                    {
                                        Education = " [" + School + " Session: " + SessionStart + "-" + SessionEnd + " ]";
                                    }
                                    else
                                    {
                                        Education = " [" + School + " ]";
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
                    if (EducationList.Count == 0)
                    {
                        #region commented
                        //string[] uniName = Regex.Split(stringSource, "trk=prof-edu-school-name' title=\"More details for this school\">");
                        //foreach (string item in uniName)
                        //{
                        //    try
                        //    {
                        //        string School = string.Empty;
                        //        string Degree = string.Empty;
                        //        string SessionEnd = string.Empty;
                        //        string SessionStart = string.Empty;
                        //        string Education = string.Empty;
                        //        if (!item.Contains("<!DOCTYPE html>"))
                        //        {
                        //            try
                        //            {
                        //                try
                        //                {
                        //                    int startindex = item.IndexOf("");
                        //                    string start = item.Substring(startindex);
                        //                    int endindex = start.IndexOf("</a>");
                        //                    School = start.Substring(0, endindex).Replace("\\u002d", string.Empty).Replace(":", string.Empty).Replace("\"", string.Empty).Replace("\\t", string.Empty).Trim();
                        //                }
                        //                catch
                        //                { }

                        //                try
                        //                {
                        //                    int startindex1 = item.IndexOf("degree");
                        //                    string start1 = item.Substring(startindex1).Replace("degree", "");
                        //                    int endindex1 = start1.IndexOf(",");
                        //                    Degree = start1.Substring(0, endindex1).Replace("\\u002d", string.Empty).Replace(":", string.Empty).Replace("\"", string.Empty).Replace("_highlight", string.Empty).Replace(">",string.Empty);
                        //                }
                        //                catch { }

                        //                try
                        //                {
                        //                    int startindex2 = item.IndexOf("<time datetime=");
                        //                    string start2 = item.Substring(startindex2).Replace("<time datetime=", string.Empty);
                        //                    int endindex2 = start2.IndexOf(">");
                        //                    SessionStart = start2.Substring(0, endindex2).Replace("\\u002d", string.Empty).Replace(":", string.Empty).Replace("\"", string.Empty).Replace("'",string.Empty);
                        //                }
                        //                catch { }

                        //                try
                        //                {
                        //                    int startindex2 = item.IndexOf("</time><time datetime=");
                        //                    string start2 = item.Substring(startindex2).Replace("</time><time datetime=", string.Empty);
                        //                    int endindex2 = start2.IndexOf(">");
                        //                    SessionEnd = start2.Substring(0, endindex2).Replace("\\u002d", string.Empty).Replace(":", string.Empty).Replace("\"", string.Empty).Replace(">",string.Empty).Replace("'",string.Empty);
                        //                }
                        //                catch { }

                        //                if (SessionStart == string.Empty && SessionEnd == string.Empty)
                        //                {
                        //                    Education = " [" + School + "] Degree: " + Degree;
                        //                }
                        //                else
                        //                {
                        //                    Education = " [" + School + "] Degree: " + Degree + " Session: " + SessionStart + "-" + SessionEnd;
                        //                }
                        //            }

                        //            catch
                        //            { }
                        //            EducationList.Add(Education);


                        //        }
                        //    }
                        //    catch { }

                        //} 
                        #endregion

                        string[] uniName1 = Regex.Split(stringSource, "trk=prof-edu-school-name' title=");
                        foreach (string item in uniName1)
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
                                            int startindex = item.IndexOf(">");
                                            string start = item.Substring(startindex);
                                            int endindex = start.IndexOf("</a>");
                                            School = start.Substring(0, endindex).Replace("\\u002d", string.Empty).Replace(":", string.Empty).Replace("\"", string.Empty).Replace("\\t", string.Empty).Replace(">", string.Empty).Trim();
                                        }
                                        catch
                                        { }

                                        try
                                        {
                                            int startindex1 = item.IndexOf("degree");
                                            string start1 = item.Substring(startindex1).Replace("degree", "");
                                            int endindex1 = start1.IndexOf(",");
                                            Degree = start1.Substring(0, endindex1).Replace("\\u002d", string.Empty).Replace(":", string.Empty).Replace("\"", string.Empty).Replace("_highlight", string.Empty).Replace(">", string.Empty);
                                            if (Degree.Contains("connection") || Degree.Contains("title") || Degree.Contains("firstName") || Degree.Contains("authToken"))
                                            {
                                                Degree = string.Empty;
                                            }

                                        }
                                        catch { }

                                        try
                                        {
                                            int startindex2 = item.IndexOf("<time datetime=");
                                            string start2 = item.Substring(startindex2).Replace("<time datetime=", string.Empty);
                                            int endindex2 = start2.IndexOf(">");
                                            SessionStart = start2.Substring(0, endindex2).Replace("\\u002d", string.Empty).Replace(":", string.Empty).Replace("\"", string.Empty).Replace("'", string.Empty);
                                        }
                                        catch { }
                                            if (string.IsNullOrEmpty(SessionStart))
                                            {
                                                try
                                                {
                                                    int startindex3 = item.IndexOf("<span class=\"education-date\"><time>");
                                                    string start3 = item.Substring(startindex3).Replace("<span class=\"education-date\"><time>", string.Empty);
                                                    int endindex3 = start3.IndexOf("</time>");
                                                    SessionStart = start3.Substring(0, endindex3).Replace("</time>", string.Empty).Replace("\\u002d", string.Empty).Replace(":", string.Empty).Replace("\"", string.Empty).Replace(">", string.Empty).Replace("'", string.Empty);
                                                }
                                                catch
                                                { }

                                            }                                                                        

                                        try
                                        {
                                            int startindex2 = item.IndexOf("<time> &#8211;");
                                            string start2 = item.Substring(startindex2).Replace("<time> &#8211;", string.Empty);
                                            int endindex2 = start2.IndexOf("</time>");
                                            SessionEnd = start2.Substring(0, endindex2).Replace("</time>", string.Empty).Replace("\\u002d", string.Empty).Replace(":", string.Empty).Replace("\"", string.Empty).Replace(">", string.Empty).Replace("'", string.Empty);
                                                                                        
                                        }
                                        catch { }
                                            if (string.IsNullOrEmpty(SessionStart))
                                            {
                                                try
                                                {
                                                    int startindex3 = item.IndexOf("<span class=\"education-date\"><time>");
                                                    string start3 = item.Substring(startindex3).Replace("<span class=\"education-date\"><time>", string.Empty);
                                                    int endindex3 = start3.IndexOf("</time>");
                                                    SessionEnd = start3.Substring(0, endindex3).Replace("</time>", string.Empty).Replace("\\u002d", string.Empty).Replace(":", string.Empty).Replace("\"", string.Empty).Replace(">", string.Empty).Replace("'", string.Empty);
                                                }
                                                catch
                                                { }

                                            }


                                            if (SessionStart == string.Empty && SessionEnd == string.Empty && !string.IsNullOrEmpty(Degree))
                                            {
                                                Education = " [" + School + " Degree: " + Degree + " ]";
                                            }
                                            else if (Degree == string.Empty && !string.IsNullOrEmpty(SessionStart) && !string.IsNullOrEmpty(SessionEnd))
                                            {
                                                Education = " [" + School + " Session: " + SessionStart + "-" + SessionEnd + " ]";
                                            }
                                            else
                                            {
                                                Education = " [" + School + " ]";
                                            }
                                        
                                            
                                    
                                        
                                    }

                                    catch
                                    { }
                                    EducationList.Add(Education);


                                }
                            }
                            catch { }

                        }
                    }

                    if (EducationList.Count == 0)
                    {
                        string[] uniName1 = Regex.Split(stringSource, "fmt__school_highlight\":");
                        uniName1 = uniName1.Skip(1).ToArray();
                        foreach (string item in uniName1)
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
                                            int startindex = item.IndexOf("\"");
                                            string start = item.Substring(startindex);
                                            int endindex = start.IndexOf(",");
                                            School = start.Substring(0, endindex).Replace("\\u002d", string.Empty).Replace(":", string.Empty).Replace("\"", string.Empty).Replace("\\t", string.Empty).Replace(">", string.Empty).Trim();
                                        }
                                        catch
                                        { }

                                        try
                                        {
                                            int startindex1 = item.IndexOf("degree\":");
                                            string start1 = item.Substring(startindex1).Replace("degree\":", "");
                                            int endindex1 = start1.IndexOf(",");
                                            Degree = start1.Substring(0, endindex1).Replace("\\u002d", string.Empty).Replace(":", string.Empty).Replace("\"", string.Empty).Replace("_highlight", string.Empty).Replace(">", string.Empty);
                                            if (Degree.Contains("connection") || Degree.Contains("title") || Degree.Contains("firstName") || Degree.Contains("authToken"))
                                            {
                                                Degree = string.Empty;
                                            }

                                        }
                                        catch { }

                                        try
                                        {
                                            int startindex2 = item.IndexOf("startdate_my\":");
                                            string start2 = item.Substring(startindex2).Replace("startdate_my\":", string.Empty);
                                            int endindex2 = start2.IndexOf(",");
                                            SessionStart = start2.Substring(0, endindex2).Replace("\\u002d", string.Empty).Replace(":", string.Empty).Replace("\"", string.Empty).Replace("'", string.Empty);
                                        }
                                        catch { }
                                        if (string.IsNullOrEmpty(SessionStart))
                                        {
                                            try
                                            {
                                                int startindex3 = item.IndexOf("<span class=\"education-date\"><time>");
                                                string start3 = item.Substring(startindex3).Replace("<span class=\"education-date\"><time>", string.Empty);
                                                int endindex3 = start3.IndexOf("</time>");
                                                SessionStart = start3.Substring(0, endindex3).Replace("</time>", string.Empty).Replace("\\u002d", string.Empty).Replace(":", string.Empty).Replace("\"", string.Empty).Replace(">", string.Empty).Replace("'", string.Empty);
                                            }
                                            catch
                                            { }

                                        }

                                        try
                                        {
                                            int startindex2 = item.IndexOf("enddate_my\":");
                                            string start2 = item.Substring(startindex2).Replace("enddate_my\":", string.Empty);
                                            int endindex2 = start2.IndexOf(",");
                                            SessionEnd = start2.Substring(0, endindex2).Replace("</time>", string.Empty).Replace("\\u002d", string.Empty).Replace(":", string.Empty).Replace("\"", string.Empty).Replace(">", string.Empty).Replace("'", string.Empty);

                                        }
                                        catch { }
                                        if (string.IsNullOrEmpty(SessionStart))
                                        {
                                            try
                                            {
                                                int startindex3 = item.IndexOf("<span class=\"education-date\"><time>");
                                                string start3 = item.Substring(startindex3).Replace("<span class=\"education-date\"><time>", string.Empty);
                                                int endindex3 = start3.IndexOf("</time>");
                                                SessionEnd = start3.Substring(0, endindex3).Replace("</time>", string.Empty).Replace("\\u002d", string.Empty).Replace(":", string.Empty).Replace("\"", string.Empty).Replace(">", string.Empty).Replace("'", string.Empty);
                                            }
                                            catch
                                            { }

                                        }


                                        if (SessionStart == string.Empty && SessionEnd == string.Empty && !string.IsNullOrEmpty(Degree))
                                        {
                                            Education = " [" + School + " Degree: " + Degree + " ]";
                                        }
                                        else if (Degree == string.Empty && !string.IsNullOrEmpty(SessionStart) && !string.IsNullOrEmpty(SessionEnd))
                                        {
                                            Education = " [" + School + " Session: " + SessionStart + "-" + SessionEnd + " ]";
                                        }
                                        else
                                        {
                                            Education = " [" + School + " ]";
                                        }




                                    }

                                    catch
                                    { }
                                    EducationList.Add(Education);


                                }
                            }
                            catch { }

                        }
                    }

                    if (EducationList.Count == 0)
                    {
                        string[] uniName1 = Regex.Split(stringSource, "{\"schoolName\":");
                        uniName1 = uniName1.Skip(1).ToArray();
                        foreach (string item in uniName1)
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
                                            int startindex = item.IndexOf("");
                                            string start = item.Substring(startindex);
                                            int endindex = start.IndexOf(",");
                                            School = start.Substring(0, endindex).Replace("\\u002d", string.Empty).Replace(":", string.Empty).Replace("\"", string.Empty).Replace("\\t", string.Empty).Replace(">", string.Empty).Trim();
                                        }
                                        catch
                                        { }

                                        try
                                        {
                                            int startindex1 = item.IndexOf("degree\":");
                                            string start1 = item.Substring(startindex1).Replace("degree\":", "");
                                            int endindex1 = start1.IndexOf(",");
                                            Degree = start1.Substring(0, endindex1).Replace("\\u002d", string.Empty).Replace(":", string.Empty).Replace("\"", string.Empty).Replace("_highlight", string.Empty).Replace(">", string.Empty);
                                            if (Degree.Contains("connection") || Degree.Contains("title") || Degree.Contains("firstName") || Degree.Contains("authToken"))
                                            {
                                                Degree = string.Empty;
                                            }

                                        }
                                        catch { }

                                        try
                                        {
                                            int startindex2 = item.IndexOf("formattedStartMonthYear\":");
                                            string start2 = item.Substring(startindex2).Replace("formattedStartMonthYear\":", string.Empty);
                                            int endindex2 = start2.IndexOf(",");
                                            SessionStart = start2.Substring(0, endindex2).Replace("\\u002d", string.Empty).Replace(":", string.Empty).Replace("\"", string.Empty).Replace("'", string.Empty);
                                        }
                                        catch { }
                                        if (string.IsNullOrEmpty(SessionStart))
                                        {
                                            try
                                            {
                                                int startindex3 = item.IndexOf("<span class=\"education-date\"><time>");
                                                string start3 = item.Substring(startindex3).Replace("<span class=\"education-date\"><time>", string.Empty);
                                                int endindex3 = start3.IndexOf("</time>");
                                                SessionStart = start3.Substring(0, endindex3).Replace("</time>", string.Empty).Replace("\\u002d", string.Empty).Replace(":", string.Empty).Replace("\"", string.Empty).Replace(">", string.Empty).Replace("'", string.Empty);
                                            }
                                            catch
                                            { }

                                        }

                                        try
                                        {
                                            int startindex2 = item.IndexOf("formattedEndMonthYear\":");
                                            string start2 = item.Substring(startindex2).Replace("formattedEndMonthYear\":", string.Empty);
                                            int endindex2 = start2.IndexOf(",");
                                            SessionEnd = start2.Substring(0, endindex2).Replace("}", string.Empty).Replace("\\u002d", string.Empty).Replace(":", string.Empty).Replace("\"", string.Empty).Replace(">", string.Empty).Replace("'", string.Empty);

                                        }
                                        catch { }
                                        if (string.IsNullOrEmpty(SessionStart))
                                        {
                                            try
                                            {
                                                int startindex3 = item.IndexOf("<span class=\"education-date\"><time>");
                                                string start3 = item.Substring(startindex3).Replace("<span class=\"education-date\"><time>", string.Empty);
                                                int endindex3 = start3.IndexOf("</time>");
                                                SessionEnd = start3.Substring(0, endindex3).Replace("</time>", string.Empty).Replace("\\u002d", string.Empty).Replace(":", string.Empty).Replace("\"", string.Empty).Replace(">", string.Empty).Replace("'", string.Empty);
                                            }
                                            catch
                                            { }

                                        }


                                        if (SessionStart == string.Empty && SessionEnd == string.Empty && !string.IsNullOrEmpty(Degree))
                                        {
                                            Education = " [" + School + " Degree: " + Degree + " ]";
                                        }
                                        else if (Degree == string.Empty && !string.IsNullOrEmpty(SessionStart) && !string.IsNullOrEmpty(SessionEnd))
                                        {
                                            Education = " [" + School + " Session: " + SessionStart + "-" + SessionEnd + " ]";
                                        }
                                        else if (!string.IsNullOrEmpty(School)&&!string.IsNullOrEmpty(Degree) && !string.IsNullOrEmpty(SessionStart) && !string.IsNullOrEmpty(SessionEnd))
                                        {
                                            Education = "[" + School + " :Degree: " + Degree +" Session: " + SessionStart + "-" + SessionEnd + " ]";
                                        }
                                        else
                                        {
                                            Education = " [" + School + " ]";
                                        }




                                    }

                                    catch
                                    { }
                                    EducationList.Add(Education);


                                }
                            }
                            catch { }

                        }
                    }
                    EducationList = EducationList.Distinct().ToList();
                    if (EducationList.Count > 0)
                    {
                        foreach (string item in EducationList)
                        {
                            if (string.IsNullOrEmpty(EducationCollection))
                            {
                                // EducationCollection = item.Replace("}", "").Replace("]", "").Replace("&amp;", "&");
                                EducationCollection = item.Replace("}", "").Replace("&amp;", "&");
                            }
                            else
                            {
                                // EducationCollection = EducationCollection + "  -  " + item.Replace("}", "").Replace("]", "").Replace("&amp;", "&");
                                EducationCollection = EducationCollection + "  -  " + item.Replace("}", "").Replace("&amp;", "&");
                            }
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

            #region Contact
                try
                {
                    int startindex = stringSource.IndexOf("Phone:");
                    string start = stringSource.Substring(startindex).Replace("Phone:", "");
                    int endindex = start.IndexOf("Skype");
                    string end = start.Substring(0, endindex).Replace("\\u003cbr\\u003e", "").Replace("\\u002d", "").Replace("\\n", "");
                    LDS_UserContact = end;
                }
                catch { }
                if (string.IsNullOrEmpty(LDS_UserContact))
                {
                    try
                    {
                        int startindex = stringSource.IndexOf("[{\"number\":\"");
                        string start = stringSource.Substring(startindex).Replace("[{\"number\":\"", "");
                        int endindex = start.IndexOf("\"}]");
                        string end = start.Substring(0, endindex).Replace("\\u003cbr\\u003e", "").Replace("\\u002d", "").Replace("\\n", "");
                        LDS_UserContact = end;

                    }
                    catch { }
                }

                #region UserContact
                if (string.IsNullOrEmpty(LDS_UserContact))
                {
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
                }
                #endregion Email

                #endregion

            #region Website
                string Web1 = string.Empty;
                List<string> Websites = new List<string>();
                try
                {
                    int startindex = stringSource.IndexOf("\"websites\":");
                    string start = stringSource.Substring(startindex).Replace("\"websites\":", "");
                    int endindex = start.IndexOf("\"showTencent\":");
                    string end = start.Substring(0, endindex).Replace("[{\"", string.Empty).Replace("{", string.Empty).Trim();
                    Web1 = end;
                }
                catch
                { }
                string[] web = Regex.Split(Web1, "\"URL\":\"");
                foreach (var items in web)
                {
                    /*     try
                         {
                             int startindex = stringSource.IndexOf("\"websites\":[{\"name\":\"");
                             string start = stringSource.Substring(startindex).Replace("\"websites\":[{\"name\":\"", "");
                             int endindex = start.IndexOf("\",\"");
                             string end = start.Substring(0, endindex);
                             Website = end;
                         }
                         catch
                         { }  */
                    //  if (string.IsNullOrEmpty(Website))

                    try
                    {
                        int startindex = items.IndexOf("");
                        string start = items.Substring(startindex).Replace("\"", "");
                        int endindex = start.IndexOf("}");
                        string end = start.Substring(0, endindex).Replace("\\u002d", string.Empty).Replace("URL:", string.Empty);
                        Website = end;
                    }
                    catch { }

                    //  try
                    //  {
                    //     Website = stringSource.Substring(stringSource.IndexOf("[{\"URL\":"), (stringSource.IndexOf(",", stringSource.IndexOf("[{\"URL\":")) - stringSource.IndexOf("[{\"URL\":"))).Replace("[{\"URL\":", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace("\\u002d", string.Empty).Trim();
                    //  }
                    //  catch { }

                    try
                    {
                        if (Website.Contains("]") || Website.Contains("}") || Website.Contains("[") || Website.Contains("}")) 
                        {
                            Website = Website.Replace("]", string.Empty).Replace("}", string.Empty).Replace("\\u002d", string.Empty).Replace("[", string.Empty).Replace("{", string.Empty).Trim();
                        }
                    }
                    catch { }
                    Websites.Add(Website);
                }
                string item2 = string.Empty;
                website1 = Websites[0];
                //foreach (var item in Websites)
                int size = Websites.Count;

                for (int i=1; i<=size; i++)
                {
                    try
                    {
                        website1 += " - " + Websites[i];
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
                    string end = start.Substring(0, endindex).Replace("\u002d", string.Empty).Replace("Å", "A").Replace("\\u002d","-");
                    location = end;
                }
                catch (Exception ex)
                {

                }
                if (location == string.Empty)
                {
                    try
                    {
                        int startindex = stringSource.IndexOf("name='location' title=\"Find other members");
                        string start = stringSource.Substring(startindex).Replace("name='location' title=\"Find other members", string.Empty);
                        int startindex1 = start.IndexOf("\">");
                        string start1 = start.Substring(startindex1);
                        int endindex = start1.IndexOf("</a>");
                        string end = start1.Substring(0, endindex).Replace("\u002d", string.Empty).Replace("Å", "A").Replace("\\u002d", "-").Replace(">", string.Empty).Replace("/",string.Empty).Replace("\"",string.Empty).Trim();
                        location = end; 
                    }
                    catch
                    { }
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
                        string[] countLocation  = location.Split(',');

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
                        string end = start.Substring(0, endindex).Replace("&amp;", "&");
                        Industry = end;
                    }
                }
                catch (Exception ex)
                {
                }
                if (string.IsNullOrEmpty(Industry))
                {
                    try
                    {
                        int startindex = stringSource.IndexOf("name=\"industry\" title=\"Find other members in this industry\">");
                        string start = stringSource.Substring(startindex).Replace("name=\"industry\" title=\"Find other members in this industry\">",string.Empty);
                        int endindex = start.IndexOf("</a>");
                        string end = start.Substring(0, endindex).Replace("</a>",string.Empty).Replace("&amp;", "&");
                        Industry = end;
                    }
                    catch
                    { }
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
                        int endindex = start.IndexOf("},\"");
                        string end = start.Substring(0, endindex).Replace("&amp;", "&").Replace("\"", "").Replace("connectionsBrowseable:true", "").Replace(",", "");
                        Connection = end;
                    }

                    if (startindex < 0)
                    {
                        int startindex1 = stringSource.IndexOf("overview-connections");
                        if (startindex1 > 0)
                        {                          
                            string start = stringSource.Substring(startindex1).Replace("overview-connections", "").Replace("\n",string.Empty).Replace("<p>",string.Empty).Replace("</p>",string.Empty);
                            int endindex = start.IndexOf("</strong>");
                            string end = start.Substring(0, endindex).Replace("&amp;", "&").Replace("\"", string.Empty).Replace("<strong>", string.Empty).Replace(">",string.Empty).Trim();
                            Connection = end;
                        }
                    }
                    if (string.IsNullOrEmpty(Connection))
                    {
                        try
                        {
                            int startindex1 = stringSource.IndexOf("class=\"member-connections\"><strong>");
                            string start = stringSource.Substring(startindex1).Replace("class=\"member-connections\"><strong>", "").Replace("\n", string.Empty).Replace("<p>", string.Empty).Replace("</p>", string.Empty);
                            int endindex = start.IndexOf("</strong>");
                            string end = start.Substring(0, endindex).Replace("&amp;", "&").Replace("\"", string.Empty).Replace("<strong>", string.Empty).Replace(">", string.Empty).Replace("<a href=#connections class=connections-link",string.Empty).Replace("</a",string.Empty).Trim();
                            Connection = end;
                        }
                        catch
                        { }
                    }

                }
                catch (Exception ex)
                {
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
                if (string.IsNullOrEmpty(Recommendations))
                {
                    List<string> ListRecommendationName = new List<string>();
                    string[] recommend = Regex.Split(stringSource, "trk=prof-exp-snippet-endorsement-name'>");
                    foreach (string item in recommend)
                    {
                        if (!item.Contains("<!DOCTYPE html>"))
                        try
                        {
                            int startindex = item.IndexOf("");
                            string start = item.Substring(startindex);
                            int endindex = start.IndexOf("</a>");
                            string end = start.Substring(0, endindex).Replace("</a>", string.Empty);
                            string recmnd = end.Trim();
                            ListRecommendationName.Add(recmnd);
                        }
                        catch
                        { }
                    }
                    foreach (string item in ListRecommendationName)
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

                #endregion


            #region Following

                String pagesource = HttpHelper.getHtmlfromUrl(new Uri(Url));
                try
                {
                    if (!string.IsNullOrEmpty(pagesource))
                    {

                        try
                        {
                            string NewUrl = getBetween(pagesource, "profile_v2_connections\",\"url\":", "}").Replace("\"", "");
                            string Response = HttpHelper.getHtmlfromUrl(new Uri(NewUrl));
                            string[] MemberfullName = Regex.Split(Response, "memberFullName");
                            MemberfullName = MemberfullName.Skip(1).ToArray();

                            try
                            {
                                foreach (string _memberFullName in MemberfullName)
                                {
                                    string FullName = getBetween(_memberFullName, "\":\"", ",").Replace("\"", string.Empty);
                                    string ProfileDescription = getBetween(_memberFullName, "headline", "\",").Replace("\"", string.Empty).Replace(",", ":").Replace(":", "");
                                    if (string.IsNullOrEmpty(Follower))
                                    {
                                        Follower = FullName + "(" + ProfileDescription + ")";
                                    }
                                    else
                                    {
                                        Follower = Follower + ":" + FullName + "(" + ProfileDescription + ")";
                                    }
                                }
                            }
                            catch
                            { }
                        }
                        catch
                        { }
                    }
                }
                catch { }
                #endregion


            #region Group
                List<string> ListGroupName = new List<string>();
                try
                {
                    string GroupUrl = string.Empty;
                    try
                    {
                        int startindex = stringSource.IndexOf("templateId\":\"profile_v2_connections");
                        string start = stringSource.Substring(startindex);
                        int endIndex = start.IndexOf("&goback");
                        GroupUrl = (start.Substring(0, endIndex).Replace(",", string.Empty).Replace("\"", string.Empty).Replace("templateId:profile_v2_connectionsurl:", string.Empty));
                        if (GroupUrl.Contains("});"))
                        {
                            GroupUrl = GroupUrl.Substring(0, GroupUrl.IndexOf("});")).Replace("});", "");
                        }
                    }
                    catch { }

                    string PageSource = HttpHelper.getHtmlfromUrl1(new Uri(GroupUrl));

                    string[] array1 = Regex.Split(PageSource, "groupRegistration?");
                    
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
                                      //  ListGroupName.Add(start.Substring(0, endIndex).Replace("\"", string.Empty).Replace("amp", string.Empty).Replace("&", string.Empty).Replace(";", string.Empty).Replace("csrfToken", string.Empty).Replace("name:", string.Empty));
                                        ListGroupName.Add(start.Substring(0, endIndex).Replace("\\","").Replace("\"", string.Empty).Replace("&amp", string.Empty).Replace("&", string.Empty).Replace(";", string.Empty).Replace("csrfToken", string.Empty).Replace("name:", string.Empty));
                                    }
                                    catch { }
                                }
                            }
                        }
                        catch { }
                    }
                    if (ListGroupName.Count > 0)
                    {
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

                }
                catch { }
                //group name data - Abhishek 5/11/14
                try
                {
                    string GroupUrl = string.Empty;
                    if (string.IsNullOrEmpty(groupscollectin))
                    {
                        int StartIndex = stringSource.IndexOf("profile_v2_connections\",\"url\":\"");
                        string Start = stringSource.Substring(StartIndex).Replace("profile_v2_connections\",\"url\":\"",string.Empty);
                        int EndIndex = Start.IndexOf("\"}");
                        string End = Start.Substring(0, EndIndex).Replace("\"}", string.Empty);
                        GroupUrl = End.Trim();                            
                    }
                    string GroupUrlResponse = HttpHelper.getHtmlfromUrl(new Uri(GroupUrl));

                    string[] Groups_Split = Regex.Split(GroupUrlResponse, "link_groupRegistration\":\"");
                    foreach (string item in Groups_Split)
                    {
                        if (item.Contains("groupRegistration?"))
                        {
                            try
                            {
                                int StartIndex = item.IndexOf("\"name\":\"");
                                string Start = item.Substring(StartIndex).Replace("\"name\":\"",string.Empty);
                                int EndIndex = Start.IndexOf("\",\"");
                                string End = Start.Substring(0, EndIndex).Replace("\",\"", string.Empty);
                                ListGroupName.Add(End);
                            }
                            catch
                            { }
                        }
                    }

                    if (ListGroupName.Count > 0)
                    {
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
                }
                catch
                { }
                #endregion
                
            #region commented Experience
                //if (LDS_Experience == string.Empty)
                //{
                //    try
                //    {
                //        string[] array = Regex.Split(stringSource, "title_highlight");
                //        string exp = string.Empty;
                //        string comp = string.Empty;
                //        List<string> ListExperince = new List<string>();
                //        List<string> ListCompany = new List<string>();
                //        string SelItem = string.Empty;

                //        foreach (var itemGrps in array)
                //        {
                //            try
                //            {
                //                if (itemGrps.Contains("title_pivot") && !itemGrps.Contains("<!DOCTYPE html")) //">Join
                //                {
                //                    try
                //                    {
                //                        int startindex = itemGrps.IndexOf("\":\"");
                //                        string start = itemGrps.Substring(startindex);
                //                        int endIndex = start.IndexOf(",");
                //                        exp = (start.Substring(0, endIndex).Replace("\"", string.Empty).Replace(":", string.Empty).Replace("&amp", "&").Replace(";", string.Empty).Replace("\\u002d", "-").Replace("name:", string.Empty));

                //                    }
                //                    catch { }

                //                    try
                //                    {
                //                        int startindex1 = itemGrps.IndexOf("companyName");
                //                        string start1 = itemGrps.Substring(startindex1);
                //                        int endIndex1 = start1.IndexOf(",");
                //                        comp = (start1.Substring(0, endIndex1).Replace("\"", string.Empty).Replace("companyName", string.Empty).Replace(":", string.Empty).Replace(";", string.Empty).Replace("\\u002d", "-").Replace("name:", string.Empty));

                //                    }
                //                    catch { }

                //                    if (Company == string.Empty)
                //                    {
                //                        Company = comp;
                //                    }

                //                    if (LDS_HeadLineTitle == string.Empty)
                //                    {
                //                        LDS_HeadLineTitle = exp;
                //                    }

                //                    ListExperince.Add(exp + ":" + comp);
                //                    ListCompany.Add(comp);
                //                    companycurrent = ListCompany[0];

                //                }
                //            }
                //            catch { }
                //        }
                //        if (ListExperince.Count > 0)
                //        {
                //            foreach (var item in ListExperince)
                //            {
                //                if (LDS_Experience == string.Empty)
                //                {
                //                    LDS_Experience = item;
                //                }
                //                else
                //                {
                //                    LDS_Experience += "  -  " + item;
                //                }
                //            }
                //        }

                //    }
                //    catch { }
                //    try
                //    {
                //        string[] array = Regex.Split(stringSource, "name='title' title='Find others with this title'>");
                //        string exp = string.Empty;
                //        string comp = string.Empty;
                //        List<string> ListExperince = new List<string>();
                //        List<string> ListCompany = new List<string>();
                //        string SelItem = string.Empty;

                //        foreach (var itemGrps in array)
                //        {
                //            try
                //            {
                //                if (!itemGrps.Contains("<!DOCTYPE html")) //">Join
                //                {
                //                    try
                //                    {
                //                        int startindex = itemGrps.IndexOf("");
                //                        string start = itemGrps.Substring(startindex);
                //                        int endIndex = start.IndexOf("</a>");
                //                        exp = (start.Substring(0, endIndex).Replace("\"", string.Empty).Replace(":", string.Empty).Replace("&amp", "&").Replace(";", string.Empty).Replace("\\u002d", "-").Replace("</a>",string.Empty).Replace("name:", string.Empty));

                //                    }
                //                    catch { }

                //                    try
                //                    {
                //                        int startindex1 = itemGrps.IndexOf("trk=prof-exp-company-name\">");
                //                        string start1 = itemGrps.Substring(startindex1);
                //                        int endIndex1 = start1.IndexOf("</a>");
                //                        comp = (start1.Substring(0, endIndex1).Replace("trk=prof-exp-company-name\">", string.Empty).Replace(":", string.Empty).Replace(";", string.Empty).Replace("\\u002d", "-").Replace("</a>", string.Empty).Replace("name:", string.Empty)).Replace("\"", string.Empty);

                //                    }
                //                    catch { }

                //                    if (Company == string.Empty)
                //                    {
                //                        Company = comp;
                //                    }

                //                    if (LDS_HeadLineTitle == string.Empty)
                //                    {
                //                        LDS_HeadLineTitle = exp;
                //                    }

                //                    ListExperince.Add(exp + ":" + comp);
                //                    ListCompany.Add(comp);
                //                    companycurrent = ListCompany[0];
                //                    if (companycurrent.Contains("span data-tracking") || string.IsNullOrEmpty(companycurrent))
                //                    {
                //                        try
                //                        {
                //                            if (companyList.Count > 0)
                //                            {
                //                                companycurrent = companyList[0];
                //                            }
                //                        }
                //                        catch
                //                        { }
                //                    }

                //                }
                //            }
                //            catch { }
                //        }
                //        if (ListExperince.Count > 0)
                //        {
                //            foreach (var item in ListExperince)
                //            {
                //                if (LDS_Experience == string.Empty)
                //                {
                //                    LDS_Experience = item;
                //                }
                //                else
                //                {
                //                    LDS_Experience += "  -  " + item;
                //                }
                //            }
                //        }

                //    }
                //    catch { }


                //} 
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
                                        string Grp = item.Substring(item.IndexOf(">"), (item.IndexOf("<", item.IndexOf(">")) - item.IndexOf(">"))).Replace(">", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Replace("&amp","&").Trim();
                                        checkgrplist.Add(Grp);
                                        checkgrplist.Distinct().ToList();
                                    }
                                    catch { }
                                }

                            }
                            catch { }
                        }

                        if (checkgrplist.Count>0) 
                        {
                            foreach (string item in checkgrplist)
                            {
                                if (string.IsNullOrEmpty(Skill))
                                {
                                    Skill = item;
                                }
                                else
                                {
                                    Skill = Skill + "  :  " + item;
                                }
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
                                            // string Grp = skillitem.Substring(skillitem.IndexOf(":"), (skillitem.IndexOf("}", skillitem.IndexOf(":")) - skillitem.IndexOf(":"))).Replace(":", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Trim();
                                            //  checkgrplist.Add(Grp);
                                            //  checkgrplist.Distinct().ToList();
                                            int startindex = skillitem.IndexOf(":\"");
                                            string start = skillitem.Substring(startindex);
                                            int endindex = start.IndexOf("viewerEndorsementId");
                                            if (!start.Contains("viewerEndorsementId"))
                                            {
                                                int endindex1 = start.IndexOf("\"}");
                                                string end1 = skillitem.Substring(0, endindex1).Replace("\"}", "").Replace("\"", "").Replace("\":", "").Replace(",", "").Replace(":", "").Replace("\\u002d", "");
                                                string Grp1 = end1;
                                                checkgrplist.Add(Grp1);
                                                checkgrplist.Distinct().ToList();
                                            }
                                            else
                                            {
                                                string end = skillitem.Substring(0, endindex).Replace("viewerEndorsementId", string.Empty).Replace("\"", "").Replace("\":", "").Replace(",", "").Replace(":", "").Replace("\\u002d", "");
                                                string Grp = end;
                                                checkgrplist.Add(Grp);
                                                checkgrplist.Distinct().ToList();
                                            }
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
                                        Skill = Skill + "  :  " + item;
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


                if (checkgrplist.Count == 0)
                {
                    if (stringSource.Contains("skills\":["))
                    {
                        string _temppagesource = Utils.getBetween(stringSource, "skills\":[", "],");
                        string[] strarr_skill = Regex.Split(_temppagesource, ",");
                        foreach (string  item in strarr_skill)
                        {
                            checkgrplist.Add(item.Replace("\"","").Replace(",",""));
                            checkgrplist.Distinct().ToList();
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
                            Skill = Skill + "  :  " + item;
                        }
                    }
                }

                #endregion

            #region Pasttitle comment on 28.08.13
                //    string[] pasttitles = Regex.Split(stringSource, "{\"media_logo\":\"");
            //    string pstTitlesitem = string.Empty;
            //    pasttitles = pasttitles.Skip(1).ToArray();
            //    foreach (string item in pasttitles)
            //    {
            //        try
            //        {
            //            int startindex = item.IndexOf("companyName\":\"");
            //            if (startindex > 0)
            //            {
            //                string start = item.Substring(startindex).Replace("companyName\":\"", "");

            //                int endindex = start.IndexOf("\",");
            //                string end = start.Substring(0, endindex);
            //                pstTitlesitem = end.Replace("&amp;",string.Empty);
            //            }
            //            if (string.IsNullOrEmpty(LDS_PastTitles))
            //            {
            //                LDS_PastTitles = pstTitlesitem;
            //            }
            //            else if (LDS_PastTitles.Contains(pstTitlesitem))
            //            {
            //                continue;
            //            }
            //            else
            //            {
            //                LDS_PastTitles = LDS_PastTitles + "  :  " + pstTitlesitem;
            //            }
            //        }
            //        catch
            //        {
            //        }
                //    }
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
                if (string.IsNullOrEmpty(LDS_PastTitles))
                {
                    try
                    {
                        foreach (string item in titleList)
                        {
                            LDS_PastTitles = LDS_PastTitles + " : " + item;
                        }
                        
                    }
                    catch
                    { }
                }

                if (string.IsNullOrEmpty(LDS_PastTitles))
                {
                    if (stringSource.Contains("id=\"overview-summary-past\">"))
                    {
                        string _tempPageSourcePastTitle = Utils.getBetween(stringSource, "id=\"overview-summary-past\">", "</tr>");
                        string[] pasttitles1 = Regex.Split(_tempPageSourcePastTitle, "dir=\"auto\">");
                        string pstTitlesitem1 = string.Empty;
                        pasttitles1 = pasttitles1.Skip(1).ToArray();
                        foreach (string item in pasttitles1)
                        {
                            try
                            {
                                if (!item.Contains("<!DOCTYPE html>") && !item.Contains("Tip: You can also search by keyword"))
                                {
                                    try
                                    {
                                        string[] Past_Ttl = Regex.Split(item, "<");
                                        pstTitlesitem1 = Past_Ttl[0].Replace(":", string.Empty).Replace("\"", string.Empty).Replace("\\u002d", "-").Replace("&amp;", "&");
                                    }
                                    catch { }

                                    if (string.IsNullOrEmpty(LDS_PastTitles))
                                    {
                                        LDS_PastTitles = pstTitlesitem1;
                                    }
                                    else if (LDS_PastTitles.Contains(pstTitlesitem1))
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        LDS_PastTitles = LDS_PastTitles + "  :  " + pstTitlesitem1;
                                    }

                                }

                            }
                            catch
                            {
                            }
                        }
                    }


                }
                #endregion

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

            #region old code
                //#region Designation Company Current
                //try
                //{
                //    if (html.Contains("phonetic-full-name"))
                //    {
                //        int FirstPointForProfileCurrent = html.IndexOf("phonetic-full-name");
                //        string FirstSubStringForProfileCurrent = html.Substring(FirstPointForProfileCurrent);
                //        int SecondPointForProfileCurrent = FirstSubStringForProfileCurrent.IndexOf("display:block");
                //        int ThirdPointForProfileCurrent = FirstSubStringForProfileCurrent.IndexOf("</p>");

                //        string SecondSubStringForProfileCurrent = FirstSubStringForProfileCurrent.Substring(SecondPointForProfileCurrent, ThirdPointForProfileCurrent - SecondPointForProfileCurrent);
                //        titlecurrent = SecondSubStringForProfileCurrent.Replace("\">", "").Replace("display:block", string.Empty).Replace("<strong class=\"highlight\"", string.Empty).Replace("</strong", string.Empty).Trim();
                //        string[] tempCCurent = Regex.Split(titlecurrent, " at ");
                //        LDS_HeadLineTitle = titlecurrent.Replace(",", ";");
                //        LDS_CurrentCompany = tempCCurent[1].Replace(",", ";");

                //    }

                //    else if (html.Contains("<p class=\"title\""))
                //    {
                //        LDS_HeadLineTitle = html.Substring(html.IndexOf("<p class=\"title\""), 150);
                //        string[] HeadLineTitle = LDS_HeadLineTitle.Split('>');
                //        string tempHeadLineTitle = HeadLineTitle[1].Replace("\n", "").Replace(")</h3", "").Replace("</p", "");
                //        LDS_HeadLineTitle = tempHeadLineTitle;
                //        try
                //        {
                //            string[] tempCCurent = Regex.Split(tempHeadLineTitle, " at ");
                //            LDS_HeadLineTitle = tempCCurent[0];
                //            LDS_CurrentCompany = tempCCurent[1];
                //        }
                //        catch { }
                //    }


                //}
                //catch (Exception ex)
                //{
                //    Console.WriteLine(ex.Message);
                //}
                //#endregion
                #endregion

            #region Current Title Current Company

                try
                {
                    int startindex = stringSource.IndexOf("memberHeadline");
                    string start = stringSource.Substring(startindex).Replace("memberHeadline", "");
                    int endindex = start.IndexOf("\",\"");
                    string end = start.Substring(0, endindex).Replace("\"", "").Replace(":", "");
                    LDS_HeadLineTitle = end.Replace("\\u002d", "-");
                }
                catch { }
                if (LDS_HeadLineTitle.Contains(" at ") || LDS_HeadLineTitle.Contains(" of "))
                {
                    //  titlecurrent = string.Empty;
                    string[] strdesigandcompany1 = new string[4];
                    //    companycurrent = string.Empty;

                    try
                    {
                        strdesigandcompany1 = Regex.Split(LDS_HeadLineTitle, " at ");

                        if (strdesigandcompany1.Count() == 1)
                        {
                            strdesigandcompany1 = Regex.Split(LDS_HeadLineTitle, " of ");
                        }
                    }
                    catch { }

                    try
                    {
                        if (string.IsNullOrEmpty(companycurrent))
                        {
                            companycurrent = strdesigandcompany1[1];
                        }
                        if (companycurrent.Contains("span data-tracking") || string.IsNullOrEmpty(companycurrent))
                        {
                            if (companyList.Count > 0)
                            {
                                companycurrent = companyList[0];
                            }
                        }
                    }
                    catch { }
                }

                try
                {
                    try
                    {
                        Company = stringSource.Substring(stringSource.IndexOf("visible\":true,\"memberHeadline"), (stringSource.IndexOf("memberID", stringSource.IndexOf("visible\":true,\"memberHeadline")) - stringSource.IndexOf("visible\":true,\"memberHeadline"))).Replace("visible\":true,\"memberHeadline", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Replace("u002d", string.Empty).Replace("   ", string.Empty).Trim();
                    }
                    catch
                    {
                    }

                    if (string.IsNullOrEmpty(Company))
                    {
                        try
                        {
                           Company = stringSource.Substring(stringSource.IndexOf("memberHeadline\":"), (stringSource.IndexOf(",", stringSource.IndexOf("memberHeadline\":")) - stringSource.IndexOf("memberHeadline\":"))).Replace("memberHeadline\":", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Replace("   ", string.Empty).Replace(":", "").Replace("&dsh;", "").Replace("&amp", "").Replace(";", "").Replace("u002d",string.Empty).Trim();
                        }
                        catch
                        {
                        }

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
                            LDS_HeadLineTitle = strdesigandcompany[0].Replace("\\u002d", "-");
                        }
                        catch { }
                        #endregion

                        #region Current Company
                        try
                        {
                            LDS_CurrentCompany = strdesigandcompany[1];
                        }
                        catch { }
                        #endregion
                    }
                }
                catch { }

        /*        #region PastCompany
                checkerlst.Clear();
                foreach (string item in companylist)
                {
                    try
                    {
                        if (!item.Contains("<!DOCTYPE html>"))
                        {
                           // int startindex = item.IndexOf("\"},");
                            int startindex = item.IndexOf("\"}");
                            string start = item.Substring(0, startindex);
                            string data = start.Replace("\"", "").Replace(":", "");
                            checkerlst.Add(Company);
                            checkerlst = checkerlst.Distinct().ToList();
                        }
                    }
                    catch { }
                }
                AllComapny = string.Empty;
                foreach (string item1 in checkerlst)
                {
                    if (string.IsNullOrEmpty(AllComapny))
                    {
                        AllComapny = item1.Replace("}", "").Replace("]", "").Replace("&amp;", "&").Replace("u002d", string.Empty);
                    }
                    else if (AllComapny.Contains(item1))
                    {
                        continue;
                    }
                    else
                    {
                        AllComapny = AllComapny + " : " + item1.Replace("}", "").Replace("]", "").Replace("&amp;", "&").Replace("u002d", string.Empty);
                    }
                }
                #endregion */

                #endregion Company

            #region degreeConnection
                //try
                // {
                //    int startindex = stringSource.IndexOf("text_plain__NAME_is_");
                //    string start = stringSource.Substring(startindex).Replace("text_plain__NAME_is_", string.Empty);
                //    int endindex = start.IndexOf("_key");
                //    string end = start.Substring(0, endindex).Replace("_key", string.Empty);
                //    degreeConnection = end.Replace("_", " ").Trim();
                //    if (degreeConnection.Contains("your connection"))
                //    {
                //        degreeConnection = "1st degree contact";
                //    }
                //}
                //catch
                //{ }
                //if (string.IsNullOrEmpty(degreeConnection))
                //{
                //    try
                //    {
                //        int startindex = stringSource.IndexOf("class=\"fp-degree-icon\"><abbr title=\"");
                //        string start = stringSource.Substring(startindex).Replace("class=\"fp-degree-icon\"><abbr title=\"", string.Empty);
                //        int startindex1 = start.IndexOf("is a");
                //        string start1 = start.Substring(startindex1);
                //        int endindex = start1.IndexOf("class=\"");
                //        string end = start1.Substring(0, endindex).Replace("\"", string.Empty).Replace("is a",string.Empty);
                //        degreeConnection = end.Replace("_", " ").Trim();
                //    }
                //    catch
                //    { }
                //}

                //if (string.IsNullOrEmpty(degreeConnection))
                //{
                //    try
                //    {
                //        int startindex = stringSource.IndexOf("is your connection\" class=\"degree-icon \">");
                //        string start = stringSource.Substring(startindex).Replace("is your connection\" class=\"degree-icon \">", string.Empty);
                //        int endindex = start.IndexOf("</abbr>");
                //        string end = start.Substring(0, endindex).Replace("\"", string.Empty).Replace("is a", string.Empty).Replace("<sup>",string.Empty).Replace("</sup>",string.Empty);
                //        degreeConnection = end.Replace("_", " ").Trim();
                //    }
                //    catch
                //    { }
                //}

                string[] arr = Regex.Split(stringSource,"<span class=\"fp-degree-icon\">");
                arr = arr.Skip(1).ToArray();
                foreach (string tempItem in arr)
                {
                    if (tempItem.Contains("class=\"degree-icon \">"))
                    {
                        int startindex = tempItem.IndexOf("class=\"degree-icon \">");
                        string start = tempItem.Substring(startindex).Replace("class=\"degree-icon \">", string.Empty);
                        int endIndex = start.IndexOf("</abbr>");
                        string end = start.Substring(0, endIndex).Replace("</abbr>", string.Empty).Replace("</sup>", string.Empty).Replace("<sup>", string.Empty);
                        degreeConnection = end.Trim();
                        break;
                    }
                }

                if (string.IsNullOrEmpty(degreeConnection))
                {
                    try
                    {
                        int startindex = stringSource.IndexOf("class=\"degree-icon \">");
                        string start = stringSource.Substring(startindex).Replace("class=\"degree-icon \">", string.Empty);
                        int endIndex = start.IndexOf("</abbr>");
                        string end = start.Substring(0, endIndex).Replace("</abbr>", string.Empty).Replace("</sup>",string.Empty).Replace("<sup>",string.Empty);
                        degreeConnection = end.Trim();
                    }
                    catch
                    { }
                }

                #endregion degreeConnection


                if (string.IsNullOrEmpty(Website) || string.IsNullOrEmpty(USERemail) || string.IsNullOrEmpty(LDS_UserContact))
                {
                    try
                    {
                        string ProfileID = Utils.getBetween(stringSource, "contact_id\":", ",").Replace("\"", "").Replace("contact_id\":", "").Trim();
                        string tempUrl = "https://www.linkedin.com/contacts/api/contacts/" + ProfileID + "/?fields=name,emails_extended,phone_numbers,sites";
                        string tempPageSource = HttpHelper.getHtmlfromUrl1(new Uri(tempUrl));

                        if (tempPageSource.Contains("{\"status\": \"success"))
                        {
                            
                            try
                            {
                                Website = Utils.getBetween(tempPageSource, "url\":", ",").Replace("\"", "").Replace(",", "").Trim();
                                website1 = Website;
                                LDS_UserContact = Utils.getBetween(tempPageSource, "Mobile\", \"id\":", ",").Replace("\"", "").Replace(",", "").Replace("}]", "").Trim();
                                if (string.IsNullOrEmpty(LDS_UserContact))
                                {
                                    LDS_UserContact = Utils.getBetween(tempPageSource, "number\":", ",").Replace("\"", "").Replace(",", "").Replace("}]", "").Trim();
                                }
                                if (string.IsNullOrEmpty(USERemail))
                                {
                                    USERemail = Utils.getBetween(tempPageSource, "email\":", ",").Replace("\"", "").Replace(",", "").Trim();
                                }
                            }
                            catch { }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

                LDS_LoginID = SearchCriteria.LoginID;
                string LDS_FinalData = TypeOfProfile + "," + LDS_UserProfileLink + "," + firstname + "," + lastname + "," + Company.Replace(",", ";") + "," + titlecurrent.Replace(",", ";") + "," + companycurrent.Replace(",", ";") + "," + Connection.Replace(",", ";") + "," + recomandation.Replace(",", string.Empty) + "," + Skill.Replace(",", ";") + "," + LDS_Experience.Replace(",", string.Empty) + "," + EducationCollection.Replace(",", ";") + "," + groupscollectin.Replace(",", ";") + "," + USERemail.Replace(",", ";") + "," + LDS_UserContact.Replace(",", ";") + "," + LDS_PastTitles + "," + AllComapny.Replace(",", ";") + "," + country.Replace(",", ";") + "," + location.Replace(",", ";").Replace("Å", "A") + "," + Industry.Replace(",", ";") + "," + Website.Replace(",", ";") + "," + LDS_LoginID + ",";// +TypeOfProfile + ",";

                if (string.IsNullOrEmpty(LDS_HeadLineTitle))
                {
                    LDS_HeadLineTitle = Company;
                }


                Log("[ " + DateTime.Now + " ] => [ " + LDS_FinalData + " ]");
                if (string.IsNullOrEmpty(LDS_LoginID))
                {
                    LDS_LoginID = " ";
                }
                if (string.IsNullOrEmpty(firstname))
                {
                    firstname = string.Empty;
                }
                if (string.IsNullOrEmpty(lastname))
                {
                    lastname = string.Empty;
                }

                if (firstname == string.Empty) firstname = "LinkedIn";
                if (lastname == string.Empty) lastname = "Member";
                if (LDS_HeadLineTitle == string.Empty) LDS_HeadLineTitle = "--";
                if (titlecurrent == string.Empty) titlecurrent = "--";
                if (Company == string.Empty) Company = "--";
                if (Connection == string.Empty) Connection = "--";
                if (recomandation == string.Empty) recomandation = "--";
                if (Skill == string.Empty) Skill = "--";
                if (LDS_Experience == string.Empty) LDS_Experience = "--";
                if (EducationCollection == string.Empty) EducationCollection = "--";
                if (groupscollectin == string.Empty) groupscollectin = "--";
                if (USERemail == string.Empty) USERemail = "--";
                if (LDS_UserContact == string.Empty) LDS_UserContact = "--";
                if (AllComapny == string.Empty) AllComapny = "--";
                if (location == string.Empty) location = "--";
                if (country == string.Empty) country = "--";
                if (Industry == string.Empty) Industry = "--";
           //   if (Website == string.Empty) Website = "--";
                if (website1 == string.Empty) website1 = "--";
                if (degreeConnection == string.Empty) degreeConnection = "--";
                if (companycurrent == string.Empty) companycurrent = "--";

                //string tempFinalData = LDS_FinalData.Replace(";", "").Replace(LDS_UserProfileLink, "").Replace(TypeOfProfile, "").Replace(",", "").Replace(LDS_LoginID, "").Trim();

                //if (!string.IsNullOrEmpty(tempFinalData))
                //{
                    try
                    {
                        if (status.Contains("LinkedinSearch_ProfileData"))
                        {
                            try
                            {
                                //AppFileHelper.AddingLinkedInDataToCSVFile(LDS_FinalData, Globals.path_LinkedinSearchByProfileURL);

                                string CSVHeader = "ProfileType" + "," + "Degree Connection" + "," + "UserProfileLink" + "," + "FirstName" + "," + "LastName" + "," + "HeadLineTitle" + "," + "Current Title " + "," + "Current Company" + "," + "Connection" + "," + "Following" + "," + "Recommendations " + "," + "SkillAndExpertise " + "," + "Experience " + "," + " Education" + "," + "Groups" + "," + "UserEmail" + "," + "UserContactNumber" + "," + "PastTitles" + "," + "PastCompany" + "," + "Location" + "," + "Country" + "," + "Industry" + "," + "WebSites" + "," + "LinkedInLoginID" + ",";
                                string CSV_Content = TypeOfProfile.Replace(",", ";") + "," + degreeConnection.Replace(",", ";") + "," + LDS_UserProfileLink.Replace(",", ";") + "," + firstname.Replace(",", ";") + "," + lastname.Replace(",", ";") + "," + LDS_HeadLineTitle.Replace(",", ";").Replace("&amp;","&") + "," + titlecurrent.Replace(",", ";") + "," + companycurrent.Replace(",", ";").Replace("002", "-") + "," + Connection.Replace(",", ";") + "," + Follower + "," + recomandation.Replace(",", ";") + "," + Skill.Replace(",", ";") + "," + LDS_Experience.Replace(",", ";") + "," + EducationCollection.Replace(",", ";") + "," + groupscollectin.Replace(",", ";") + "," + USERemail.Replace(",", ";") + "," + LDS_UserContact.Replace(",", ";") + "," + LDS_PastTitles.Replace(",", ";") + "," + AllComapny.Replace(",", ";") + "," + location.Replace(",", ";") + "," + country.Replace(",", ";") + "," + Industry.Replace(",", ";") + "," + website1.Replace(",", ";") + "," + LDS_LoginID.Replace(",", ";");
                                //string CSV_Content = TypeOfProfile.Replace(",", ";") + "," + LDS_UserProfileLink.Replace(",", ";") + "," + firstname.Replace(",", ";") + "," + lastname.Replace(",", ";") + "," + LDS_HeadLineTitle.Replace(",", ";") + "," + titlecurrent.Replace(",", ";") + "," + Company.Replace(",", ";") + "," + Connection.Replace(",", ";") + "," + recomandation.Replace(",", ";") + "," + Skill.Replace(",", ";") + "," + LDS_Experience.Replace(",", ";") + "," + EducationCollection.Replace(",", ";") + "," + groupscollectin.Replace(",", ";") + "," + USERemail.Replace(",", ";") + "," + LDS_UserContact.Replace(",", ";") + "," + LDS_PastTitles.Replace(",", ";") + "," + AllComapny.Replace(",", ";") + "," + location.Replace(",", ";") + "," + country.Replace(",", ";") + "," + Industry.Replace(",", ";") + "," + Website.Replace(",", ";") + "," + LDS_LoginID.Replace(",", ";");
                                //string CSV_Content = TypeOfProfile + "," + LDS_UserProfileLink + "," + firstname + "," + lastname + "," + Company.Replace(",", ";") + "," + titlecurrent.Replace(",", ";") + "," + companycurrent.Replace(",", ";") + "," + Connection.Replace(",", ";") + "," + recomandation.Replace(",", string.Empty) + "," + Skill.Replace(",", ";") + "," + LDS_Experience.Replace(",", string.Empty) + "," + EducationCollection.Replace(",", ";") + "," + groupscollectin.Replace(",", ";") + "," + USERemail.Replace(",", ";") + "," + LDS_UserContact.Replace(",", ";") + "," + LDS_PastTitles + "," + AllComapny.Replace(",", ";") + "," + country.Replace(",", ";") + "," + location.Replace(",", ";") + "," + Industry.Replace(",", ";") + "," + Website.Replace(",", ";") + "," + LDS_LoginID + ",";// +TypeOfProfile + ",";

                                CSVUtilities.ExportDataCSVFile(CSVHeader, CSV_Content, Globals.path_LinkedinSearchByProfileURL);

                                Log("[ " + DateTime.Now + " ] => [ Record Saved In CSV File ! ]");
                            }
                            catch
                            {
                            }
                        }
                        else
                        {
                            try
                            {
                                //AppFileHelper.AddingLinkedInDataToCSVFile(LDS_FinalData, Globals.path_LinkedinSearchSearchByPeople);

                                string CSVHeader = "ProfileType" + "," + "Degree Connection" + "," + "UserProfileLink" + "," + "FirstName" + "," + "LastName" + "," + "HeadLineTitle" + "," + "CurrentTitle " + "," + "Company" + "," + "Connection" + "," + "Following" + "," + "Recommendations " + "," + "SkillAndExpertise " + "," + "Experience " + "," + " Education" + "," + "Groups" + "," + "UserEmail" + "," + "UserContactNumber" + "," + "PastTitles" + "," + "PastCompany" + "," + "Location" + "," + "Country" + "," + "Industry" + "," + "WebSites" + "," + "LinkedInLoginID" + ",";
                                //string CSV_Content = TypeOfProfile.Replace(",", ";") + "," + LDS_UserProfileLink.Replace(",", ";") + "," + firstname.Replace(",", ";") + "," + lastname.Replace(",", ";") + "," + LDS_HeadLineTitle.Replace(",", ";") + "," + titlecurrent.Replace(",", ";") + "," + Company.Replace(",", ";") + "," + Connection.Replace(",", ";") + "," + recomandation.Replace(",", ";") + "," + Skill.Replace(",", ";") + "," + LDS_Experience.Replace(",", ";") + "," + EducationCollection.Replace(",", ";") + "," + groupscollectin.Replace(",", ";") + "," + USERemail.Replace(",", ";") + "," + LDS_UserContact.Replace(",", ";") + "," + LDS_PastTitles.Replace(",", ";") + "," + AllComapny.Replace(",", ";") + "," + location.Replace(",", ";") + "," + country.Replace(",", ";") + "," + Industry.Replace(",", ";") + "," + Website.Replace(",", ";") + "," + LDS_LoginID.Replace(",", ";");
                                string CSV_Content = TypeOfProfile.Replace(",", ";") + "," + degreeConnection.Replace(",", ";") + "," + LDS_UserProfileLink.Replace(",", ";") + "," + firstname.Replace(",", ";") + "," + lastname.Replace(",", ";") + "," + LDS_HeadLineTitle.Replace(",", ";") + "," + titlecurrent.Replace(",", ";") + "," + companycurrent.Replace(",", ";").Replace("002", "-") + "," + Connection.Replace(",", ";") + "," + Follower.Replace(",",";") + "," + recomandation.Replace(",", ";") + "," + Skill.Replace(",", ";") + "," + LDS_Experience.Replace(",", ";") + "," + EducationCollection.Replace(",", ";") + "," + groupscollectin.Replace(",", ";") + "," + USERemail.Replace(",", ";") + "," + LDS_UserContact.Replace(",", ";") + "," + LDS_PastTitles.Replace(",", ";") + "," + AllComapny.Replace(",", ";") + "," + location.Replace(",", ";") + "," + country.Replace(",", ";") + "," + Industry.Replace(",", ";") + "," + website1.Replace(",", ";") + "," + LDS_LoginID.Replace(",", ";");

                                CSVUtilities.ExportDataCSVFile(CSVHeader, CSV_Content, Globals.path_LinkedinSearchByPeople);

                                Log("[ " + DateTime.Now + " ] => [ Record Saved In CSV File ! ]");
                            }
                            catch
                            {
                            }
                        }
                    }
                    catch
                    {
                    }

                    return true;
                //}
            }
            catch { }
            return false;
        } 
        #endregion

        #region CrawlingLinkedInPageWithoutLoggingIn
        public void CrawlingLinkedInPageWithoutLoggingIn()
        {
        }
        #endregion

        #region CrawlingPageDataSource
        public void CrawlingPageDataSource(string Url, ref GlobusHttpHelper HttpHelper, string status)
        {
            //if (SearchCriteria.starter)
            {
                //if (SearchCriteria.starter)
                {
                    try
                    {
                        Log("[ " + DateTime.Now + " ] => [ Start Parsing Process ]");

                        #region Data Initialization
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
                        string companyCurrenttitle1 = string.Empty;
                        string companyCurrenttitle2 = string.Empty;
                        string companyCurrenttitle3 = string.Empty;
                        string companyCurrenttitle4 = string.Empty;
                        string titleCurrenttitle = string.Empty;
                        string titleCurrenttitle2 = string.Empty;
                        string titleCurrenttitle3 = string.Empty;
                        string titleCurrenttitle4 = string.Empty;
                        string Skill = string.Empty;
                        string TypeOfProfile = "Public1";
                        string connection = string.Empty;
                        string Finaldata = string.Empty;
                        #endregion

                        #region LDS_DataInitialization
                        string LDS_FirstName = string.Empty;
                        string LDS_LastName = string.Empty;
                        string LDS_UserProfileLink = string.Empty;
                        string LDS_HeadLineTitle = string.Empty;
                        string LDS_CurrentTitle = string.Empty;
                        string LDS_PastTitles = string.Empty;
                        string LDS_Loction = string.Empty;
                        string LDS_Country = string.Empty;
                        string LDS_Connection = string.Empty;
                        string LDS_Recommendations = string.Empty;
                        string LDS_SkillAndExpertise = string.Empty;
                        string LDS_Education = string.Empty;
                        string LDS_Experience = string.Empty;
                        string LDS_ProfileType = "Public";
                        string LDS_Groups = string.Empty;
                        string LDS_UserEmail = string.Empty;
                        string LDS_UserContactNumber = string.Empty;
                        string LDS_CurrentCompany = string.Empty;
                        string LDS_PastCompany = string.Empty;
                        string LDS_LoginID = string.Empty;
                        string LDS_Websites = string.Empty;
                        string LDS_Industry = string.Empty;
                        #endregion

                        #region Chilkat Initialization
                        Chilkat.Http http = new Chilkat.Http();
                        ///Chilkat Http Request to be used in Http Post...
                        Chilkat.HttpRequest req = new Chilkat.HttpRequest();
                        Chilkat.HtmlUtil htmlUtil = new Chilkat.HtmlUtil();

                        // Any string unlocks the component for the 1st 30-days.
                        bool success = http.UnlockComponent("THEBACHttp_b3C9o9QvZQ06");
                        if (success != true)
                        {
                            Console.WriteLine(http.LastErrorText);
                            return;
                        }

                        http.CookieDir = "memory";
                        http.SendCookies = true;
                        http.SaveCookies = true;

                        //Url = "http://www.linkedin.com/profile/view?id=2259203&authType=OUT_OF_NETWORK&authToken=Iw-O&locale=en_US&srchid=2247217581371293676829&srchindex=1&srchtotal=2004&trk=vsrp_people_res_name&trkInfo=VSRPsearchId%3A2247217581371293676829%2CVSRPtargetId%3A2259203%2CVSRPcmpt%3Aprimary";

                        if (!Url.Contains("http"))
                        {
                            Url = "http://" + Url;
                        }

                        html = HttpHelper.getHtmlfromUrl1(new Uri(Url));

                        html = htmlUtil.EntityDecode(html);

                        ////  Convert the HTML to XML:
                        Chilkat.HtmlToXml htmlToXml = new Chilkat.HtmlToXml();
                        Chilkat.HtmlToXml htmlToXml1 = new Chilkat.HtmlToXml();
                        Chilkat.HtmlToXml htmlToXml2 = new Chilkat.HtmlToXml();
                        success = htmlToXml.UnlockComponent("THEBACHtmlToXml_7WY3A57sZH3O");
                        if ((success != true))
                        {
                            Console.WriteLine(htmlToXml.LastErrorText);
                            return;
                        }

                        string xHtml = null;
                        string xHtml1 = null;
                        //string xHtml2 = null;

                        htmlToXml.Html = html;
                        xHtml = htmlToXml.ToXml();

                        Chilkat.Xml xml = new Chilkat.Xml();
                        xml.LoadXml(xHtml);

                        ////  Iterate over all h1 tags:
                        Chilkat.Xml xNode = default(Chilkat.Xml);
                        Chilkat.Xml xBeginSearchAfter = default(Chilkat.Xml);
                        #endregion

                        #region for paRSING
                        List<string> list = new List<string>();
                        List<string> Grouplist = new List<string>();
                        List<string> listtitle = new List<string>();
                        List<string> Currentlist = new List<string>();
                        List<string> Skilllst = new List<string>();
                        list.Clear();

                        //new parshing code 

                        List<string> TempFirstName = objChilkat.GetDataTagAttributewithId(html, "div", "name-container");
                        xBeginSearchAfter = null;
                        xNode = xml.SearchForTag(xBeginSearchAfter, "dt");
                        Grouplist.Clear();
                        xBeginSearchAfter = null;
                        #region parsergroup
                        xNode = xml.SearchForAttribute(xBeginSearchAfter, "div", "class", "group-data");

                        while ((xNode != null))
                        {
                            Finaldata = xNode.AccumulateTagContent("text", "/text");
                            Grouplist.Add(Finaldata);
                            string[] tempC1 = Regex.Split(Finaldata, " at ");
                            xBeginSearchAfter = xNode;
                            xNode = xml.SearchForAttribute(xBeginSearchAfter, "div", "class", "group-data");
                        }

                        int groupcounter = 0;
                        string AllGRoup = string.Empty;
                        foreach (string item in Grouplist)
                        {
                            if (item.Contains("Join"))
                            {
                                if (groupcounter == 0)
                                {
                                    LDS_Groups = item;
                                    groupcounter++;
                                }
                                else
                                {
                                    LDS_Groups = AllGRoup + ";" + item;
                                }
                            }
                        }
                        #endregion

                        #region parserSkill
                        xNode = xml.SearchForTag(xBeginSearchAfter, "dt");

                        Skilllst.Clear();
                        xBeginSearchAfter = null;

                        xNode = xml.SearchForAttribute(xBeginSearchAfter, "div", "id", "profile-skills");
                        while ((xNode != null))
                        {
                            Finaldata = xNode.AccumulateTagContent("text", "/text");
                            if (Finaldata.Contains("extlib: _toggleclass"))
                            {
                                try
                                {
                                    string[] Temp = Finaldata.Split(';');
                                    LDS_SkillAndExpertise = Temp[4];
                                }
                                catch { }

                            }
                            else
                            {
                                try
                                {
                                    LDS_SkillAndExpertise = Finaldata.Replace("Skills & Expertise", " ");
                                    Skilllst.Add(Finaldata);
                                }
                                catch { }
                            }
                            xBeginSearchAfter = xNode;
                            xNode = xml.SearchForAttribute(xBeginSearchAfter, "div", "id", "profile-skills");
                        }

                        if (LDS_SkillAndExpertise.Contains(" Endorsements LI.i18n.register('section_skills_person_endorsed_tmpl"))
                        {
                            LDS_SkillAndExpertise = string.Empty;
                        }

                        Skilllst.Distinct();
                        #endregion

                        #region UrlProfile
                        try
                        {
                            if (html.Contains("webProfileURL"))
                            {
                                int FirstPointForProfileURL = html.IndexOf("webProfileURL");
                                string FirstSubStringForProfileURL = html.Substring(FirstPointForProfileURL);
                                int SecondPointForProfileURL = FirstSubStringForProfileURL.IndexOf(">");
                                int ThirdPointForProfileURL = FirstSubStringForProfileURL.IndexOf("</a>");

                                string SecondSubStringForProfileURL = FirstSubStringForProfileURL.Substring(SecondPointForProfileURL, ThirdPointForProfileURL - SecondPointForProfileURL);
                                LDS_UserProfileLink = SecondSubStringForProfileURL.Replace(">", string.Empty);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }



                        try
                        {
                            string[] UrlFull = System.Text.RegularExpressions.Regex.Split(Url, "&authType");
                            LDS_UserProfileLink = UrlFull[0];

                            LDS_UserProfileLink = Url;
                        }
                        catch (Exception ex)
                        {

                        }
                        #endregion

                        #region Connection
                        if (html.Contains("overview-connections"))
                        {
                            try
                            {
                                Connection = html.Substring(html.IndexOf("leo-module mod-util connections"), 500);
                                string[] Arr = Connection.Split('>');
                                string tempConnection = Arr[5].Replace("</strong", "").Replace(")</h3", "").Replace("(", "");
                                if (tempConnection.Length < 8)
                                {
                                    LDS_Connection = tempConnection + "Connection";
                                }
                                else
                                {
                                    LDS_Connection = string.Empty;
                                }
                            }
                            catch (Exception ex)
                            {
                                //overview-connections
                                try
                                {
                                    LDS_Connection = html.Substring(html.IndexOf("overview-connections"), 100);
                                    string[] Arr = LDS_Connection.Split('>');
                                    string tempConnection = Arr[3].Replace("</strong", "").Replace(")</h3", "").Replace("(", "");
                                    LDS_Connection = tempConnection + " Connection";
                                }
                                catch { }
                            }
                        }
                        #endregion

                        #region Recommendation
                        if (html.Contains("Recommendations"))
                        {

                            try
                            {
                                string[] rList = System.Text.RegularExpressions.Regex.Split(html, "Recommendations");
                                string[] R3List = rList[2].Split('\n');
                                string temprecomandation = R3List[4].Replace("</strong>", "").Replace("<strong>", "");
                                if (temprecomandation.Contains("recommended"))
                                {
                                    LDS_Recommendations = temprecomandation;
                                }
                                else
                                {
                                    LDS_Recommendations = "";
                                }

                            }
                            catch (Exception ex)
                            {
                                LDS_Recommendations = string.Empty;
                            }
                        }
                        #endregion

                        #region Websites
                        if (html.Contains("websites"))
                        {
                            try
                            {
                                string websitedem = html.Substring(html.IndexOf("websites"), 500);

                                string[] Arr = Regex.Split(websitedem, "href");
                                foreach (string item in Arr)
                                {
                                    if (item.Contains("redir/redirect?url"))
                                    {
                                        string tempArr = item.Substring(item.IndexOf("name="), 50);
                                        string[] temarr = tempArr.Split('\n');
                                        LDS_Websites = temarr[1];
                                    }
                                }

                            }
                            catch (Exception ex)
                            {
                                LDS_Websites = string.Empty;
                            }
                        }
                        #endregion

                        #region Getting Industry
                        try
                        {

                            string Industrytemp = html.Substring(html.IndexOf("Find users in this industry"), 100);
                            string[] TempIndustery = Industrytemp.Split('>');
                            LDS_Industry = TempIndustery[1].Replace("</strong", "").Replace("</a", "");

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        #endregion

                        #region Getting First Name
                        try
                        {
                            if (html.Contains("given-name"))
                            {
                                int FirstPointForProfilename = html.IndexOf("given-name");
                                string FirstSubStringForProfilename = html.Substring(FirstPointForProfilename);
                                int SecondPointForProfilename = FirstSubStringForProfilename.IndexOf(">");
                                int ThirdPointForProfilename = FirstSubStringForProfilename.IndexOf("</span>");

                                string SecondSubStringForProfilename = FirstSubStringForProfilename.Substring(SecondPointForProfilename, ThirdPointForProfilename - SecondPointForProfilename);
                                LDS_FirstName = SecondSubStringForProfilename.Replace(">", string.Empty);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        #endregion

                        #region LastName
                        try
                        {
                            if (html.Contains("family-name"))
                            {
                                int FirstPointForProfilelastname = html.IndexOf("family-name");
                                string FirstSubStringForProfilelastname = html.Substring(FirstPointForProfilelastname);
                                int SecondPointForProfilelastname = FirstSubStringForProfilelastname.IndexOf(">");
                                int ThirdPointForProfilelastname = FirstSubStringForProfilelastname.IndexOf("</span>");

                                string SecondSubStringForProfilelastname = FirstSubStringForProfilelastname.Substring(SecondPointForProfilelastname, ThirdPointForProfilelastname - SecondPointForProfilelastname);
                                string templastname = SecondSubStringForProfilelastname.Replace(">", string.Empty);
                                if (templastname.Contains(","))
                                {
                                    string[] arrylastname = templastname.Split(',');
                                    LDS_LastName = arrylastname[0];
                                }
                                else
                                {
                                    LDS_LastName = templastname;
                                }


                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                        #endregion

                        #region Designation Company Current
                        try
                        {
                            if (html.Contains("phonetic-full-name"))
                            {
                                int FirstPointForProfileCurrent = html.IndexOf("phonetic-full-name");
                                string FirstSubStringForProfileCurrent = html.Substring(FirstPointForProfileCurrent);
                                int SecondPointForProfileCurrent = FirstSubStringForProfileCurrent.IndexOf("display:block");
                                int ThirdPointForProfileCurrent = FirstSubStringForProfileCurrent.IndexOf("</p>");

                                string SecondSubStringForProfileCurrent = FirstSubStringForProfileCurrent.Substring(SecondPointForProfileCurrent, ThirdPointForProfileCurrent - SecondPointForProfileCurrent);
                                titlecurrent = SecondSubStringForProfileCurrent.Replace("\">", "").Replace("display:block", string.Empty).Replace("<strong class=\"highlight\"", string.Empty).Replace("</strong", string.Empty).Trim();
                                string[] tempCCurent = Regex.Split(titlecurrent, " at ");
                                LDS_HeadLineTitle = titlecurrent.Replace(",", ";");
                                LDS_CurrentCompany = tempCCurent[1].Replace(",", ";");

                            }

                            else if (html.Contains("<p class=\"title\""))
                            {
                                LDS_HeadLineTitle = html.Substring(html.IndexOf("<p class=\"title\""), 150);
                                string[] HeadLineTitle = LDS_HeadLineTitle.Split('>');
                                string tempHeadLineTitle = HeadLineTitle[1].Replace("\n", "").Replace(")</h3", "").Replace("</p", "");
                                LDS_HeadLineTitle = tempHeadLineTitle;
                                try
                                {
                                    string[] tempCCurent = Regex.Split(tempHeadLineTitle, " at ");
                                    LDS_HeadLineTitle = tempCCurent[0];
                                    LDS_CurrentCompany = tempCCurent[1];
                                }
                                catch { }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        #endregion

                        #region Education
                        try
                        {
                            if (html.Contains("summary-education"))
                            {
                                int FirstPointForProfileeducation1 = html.IndexOf("summary-education");
                                string FirstSubStringForProfileeducation1 = html.Substring(FirstPointForProfileeducation1);
                                int SecondPointForProfileeducation1 = FirstSubStringForProfileeducation1.IndexOf("<li>");
                                int ThirdPointForProfileeducation1 = FirstSubStringForProfileeducation1.IndexOf("</li>");

                                string SecondSubStringForProfileeducation1 = FirstSubStringForProfileeducation1.Substring(SecondPointForProfileeducation1, ThirdPointForProfileeducation1 - SecondPointForProfileeducation1);
                                education1 = SecondSubStringForProfileeducation1.Replace("<li>", string.Empty).Replace(",", string.Empty).Trim();
                            }

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }


                        #endregion

                        #region Country
                        try
                        {
                            if (html.Contains("locality"))
                            {
                                int FirstPointForlocality = html.IndexOf("locality");
                                string FirstSubStringForlocality = html.Substring(FirstPointForlocality);
                                int SecondPointForlocality = FirstSubStringForlocality.IndexOf("location");
                                int ThirdPointForlocality = FirstSubStringForlocality.IndexOf("</a>");

                                string SecondSubStringForlocality = FirstSubStringForlocality.Substring(SecondPointForlocality, ThirdPointForlocality - SecondPointForlocality);
                                string temlocation = SecondSubStringForlocality.Replace("location", string.Empty).Replace(">", string.Empty).Replace('"', ' ');
                                string[] temp = temlocation.Split(',');
                                LDS_Loction = temp[0].Replace("<strong class= highlight", string.Empty).Replace("</strong", string.Empty);
                                LDS_Country = temp[1].Replace("<strong class= highlight", string.Empty).Replace("</strong", string.Empty);
                                // country = temp[1].Replace("</strong", string.Empty);
                            }

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        #endregion

                        #region User Email
                        try
                        {
                            if (html.Contains("Email & Phone:"))
                            {
                                int FirstPointFortitlepast1 = html.IndexOf("abook-email");
                                string FirstSubStringFortitlepast1 = html.Substring(FirstPointFortitlepast1);
                                int SecondPointFortitlepast1 = FirstSubStringFortitlepast1.IndexOf("<a");
                                int ThirdPointFortitlepast1 = FirstSubStringFortitlepast1.IndexOf("</a>");

                                string SecondSubStringFortitlepast1 = FirstSubStringFortitlepast1.Substring(SecondPointFortitlepast1, ThirdPointFortitlepast1 - SecondPointFortitlepast1);
                                string[] tempEmail = SecondSubStringFortitlepast1.Split('>');
                                LDS_UserEmail = tempEmail[1];

                            }


                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                        #endregion

                        #region Type Of profile
                        try
                        {
                            if (html.Contains("profile-header"))
                            {
                                int FirstPointForProfileType = html.IndexOf("profile-header");
                                string FirstSubStringForProfileType = html.Substring(FirstPointForProfileType);
                                int SecondPointForProfileType = FirstSubStringForProfileType.IndexOf("class=\"n fn\"");
                                int ThirdPointForProfileType = FirstSubStringForProfileType.IndexOf("</span>");

                                string SecondSubStringForProfileType = FirstSubStringForProfileType.Substring(SecondPointForProfileType, ThirdPointForProfileType - SecondPointForProfileType);
                                string[] tempProfileType = SecondSubStringForProfileType.Split('>');
                                string ProfileType = tempProfileType[1];
                                LDS_ProfileType = ProfileType;
                            }
                            //<h1><span id="name" class="n fn">Private</span>
                            else if (html.Contains(" class=\"n fn\""))
                            {
                                try
                                {
                                    string ProfileTypetemp = html.Substring(html.IndexOf("class=\"n fn\""), 20);
                                    string[] TempProfileType = ProfileTypetemp.Split('>');
                                    LDS_ProfileType = TempProfileType[1].Replace("</strong", "").Replace("</a", "");
                                }
                                catch { }
                            }

                            if (LDS_ProfileType != "Public")
                            {
                                LDS_ProfileType = "Private";
                            }

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        #endregion

                        #region PhonNumber
                        try
                        {
                            if (html.Contains("<dt>Phone:</dt>"))
                            {
                                int FirstPointFortitlepast1 = html.IndexOf("profile-personal");
                                string FirstSubStringFortitlepast1 = html.Substring(FirstPointFortitlepast1);
                                int SecondPointFortitlepast1 = FirstSubStringFortitlepast1.IndexOf("<p>");
                                int ThirdPointFortitlepast1 = FirstSubStringFortitlepast1.IndexOf("<span");

                                string SecondSubStringFortitlepast1 = FirstSubStringFortitlepast1.Substring(SecondPointFortitlepast1, ThirdPointFortitlepast1 - SecondPointFortitlepast1);
                                LDS_UserContactNumber = SecondSubStringFortitlepast1.Replace("<p>", string.Empty);
                            }

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        #endregion

                        xNode = xml.SearchForTag(xBeginSearchAfter, "dt");
                        xBeginSearchAfter = xNode;


                        list.Clear();

                        #endregion

                        #region Regionfor PastCompney
                        try
                        {
                            if (html.Contains("summary-past"))
                            {

                                int FirstPointForPasttitle = html.IndexOf("summary-past");
                                string FirstSubStringForPasttitle = html.Substring(FirstPointForPasttitle);
                                int SecondPointForPasttitle = FirstSubStringForPasttitle.IndexOf("<li>");
                                int ThirdPointForPasttitle = FirstSubStringForPasttitle.IndexOf("summary-education");
                                string SecondSubStringForPasttitle = FirstSubStringForPasttitle.Substring(SecondPointForPasttitle, ThirdPointForPasttitle - SecondPointForPasttitle);
                                string FirstSubStringForPasttitlelast = htmlUtil.EntityDecode(SecondSubStringForPasttitle);

                                htmlToXml1.Html = FirstSubStringForPasttitlelast;
                                xHtml1 = htmlToXml1.ToXml();

                                Chilkat.Xml xml1 = new Chilkat.Xml();
                                xml1.LoadXml(xHtml1);

                                ////  Iterate over all h1 tags:
                                Chilkat.Xml xNode1 = default(Chilkat.Xml);
                                Chilkat.Xml xBeginSearchAfter1 = default(Chilkat.Xml);


                                list.Clear();
                                string[] tempC1 = null;
                                xNode1 = xml1.SearchForTag(xBeginSearchAfter1, "li");

                                while ((xNode1 != null))
                                {
                                    Finaldata = xNode1.AccumulateTagContent("text", "/text");
                                    listtitle.Add(Finaldata);
                                    // list.Add(Finaldata);

                                    try
                                    {
                                        tempC1 = Regex.Split(Finaldata, " at ");
                                    }
                                    catch { }
                                    if (tempC1 != null)
                                    {
                                        try
                                        {
                                            list.Add(tempC1[1]);
                                        }
                                        catch { }

                                    }

                                    xNode1 = xml1.SearchForTag(xBeginSearchAfter1, "li");
                                    xBeginSearchAfter1 = xNode1;
                                }

                                if (listtitle.Count > 0 || list.Count > 0)
                                {
                                    try
                                    {
                                        titlepast1 = listtitle[0] != null ? listtitle[0] : string.Empty;
                                        titlepast2 = listtitle[1] != null ? listtitle[1] : string.Empty;
                                        titlepast3 = listtitle[2] != null ? listtitle[2] : string.Empty;
                                        titlepast4 = listtitle[3] != null ? listtitle[3] : string.Empty;
                                    }
                                    catch { }

                                    try
                                    {
                                        companypast1 = list[0] != null ? list[0] : string.Empty;

                                        companypast2 = list[1] != null ? list[1] : string.Empty;

                                        companypast3 = list[2] != null ? list[2] : string.Empty;

                                        companypast4 = list[3] != null ? list[3] : string.Empty;
                                    }
                                    catch { }
                                }

                            }
                        }
                        catch { };

                        list.Clear();
                        #endregion

                        #region Regionfor summary-current
                        try
                        {
                            if (html.Contains("summary-current"))
                            {



                                int FirstPointForCurrenttitle = html.IndexOf("summary-current");
                                string FirstSubStringForCurrenttitle = html.Substring(FirstPointForCurrenttitle);
                                int SecondPointForCurrenttitle = FirstSubStringForCurrenttitle.IndexOf("<li>");
                                int ThirdPointForCurrenttitle = FirstSubStringForCurrenttitle.IndexOf("summary-past");
                                string SecondSubStringForCurrenttitle = FirstSubStringForCurrenttitle.Substring(SecondPointForCurrenttitle, ThirdPointForCurrenttitle - SecondPointForCurrenttitle);
                                string FirstSubStringForCurrenttitlelast = htmlUtil.EntityDecode(SecondSubStringForCurrenttitle);

                                htmlToXml1.Html = FirstSubStringForCurrenttitlelast;
                                xHtml1 = htmlToXml1.ToXml();

                                Chilkat.Xml xml1 = new Chilkat.Xml();
                                xml1.LoadXml(xHtml1);

                                ////  Iterate over all h1 tags:
                                Chilkat.Xml xNode1 = default(Chilkat.Xml);
                                Chilkat.Xml xBeginSearchAfter1 = default(Chilkat.Xml);


                                Currentlist.Clear();
                                list.Clear();
                                string[] tempC1 = null;
                                xNode1 = xml1.SearchForTag(xBeginSearchAfter1, "li");

                                while ((xNode1 != null))
                                {
                                    Finaldata = xNode1.AccumulateTagContent("text", "/text");
                                    Currentlist.Add(Finaldata);
                                    // list.Add(Finaldata);

                                    try
                                    {
                                        tempC1 = Regex.Split(Finaldata, " at ");
                                    }
                                    catch { }
                                    if (tempC1 != null)
                                    {
                                        try
                                        {
                                            list.Add(tempC1[1]);
                                        }
                                        catch { }

                                    }

                                    xNode1 = xml1.SearchForTag(xBeginSearchAfter1, "li");
                                    xBeginSearchAfter1 = xNode1;
                                }

                                if (Currentlist.Count > 0 || list.Count > 0)
                                {
                                    try
                                    {
                                        titleCurrenttitle = Currentlist[0] != null ? Currentlist[0] : string.Empty;
                                        titleCurrenttitle2 = Currentlist[1] != null ? Currentlist[1] : string.Empty;
                                        titleCurrenttitle3 = Currentlist[2] != null ? Currentlist[2] : string.Empty;
                                        titleCurrenttitle4 = Currentlist[3] != null ? Currentlist[3] : string.Empty;
                                    }
                                    catch { }

                                    try
                                    {
                                        companyCurrenttitle1 = list[0] != null ? list[0] : string.Empty;

                                        companyCurrenttitle2 = list[1] != null ? list[1] : string.Empty;

                                        companyCurrenttitle3 = list[2] != null ? list[2] : string.Empty;

                                        companyCurrenttitle4 = list[3] != null ? list[3] : string.Empty;
                                    }
                                    catch { }
                                }

                            }
                        }
                        catch { };

                        list.Clear();
                        #endregion

                        #region RegionForEDUCATION
                        try
                        {
                            if (html.Contains("summary-education"))
                            {

                                int FirstPointForEDUCATION = html.IndexOf("summary-education");
                                string FirstSubStringForEDUCATION = html.Substring(FirstPointForEDUCATION);
                                int SecondPointForEDUCATION = FirstSubStringForEDUCATION.IndexOf("<li>");
                                int ThirdPointForEDUCATION = FirstSubStringForEDUCATION.IndexOf("</ul>");
                                string SecondSubStringForEDUCATION = FirstSubStringForEDUCATION.Substring(SecondPointForEDUCATION, ThirdPointForEDUCATION - SecondPointForEDUCATION);
                                //string tempEDu = SecondSubStringForEDUCATION.Replace("<li>", string.Empty).Replace("</li>", string.Empty).Replace("  ", string.Empty).Replace("\n", string.Empty).Replace("\t", string.Empty).Trim();
                                string temptg = SecondSubStringForEDUCATION.Replace("<li>", "");

                                string[] templis6t = temptg.Split('/');
                                education1 = templis6t[0].Replace("\n", string.Empty).Replace("\t", string.Empty).Replace("<", string.Empty).Replace("span>", string.Empty).Replace(",", string.Empty).Trim();
                                education2 = templis6t[1].Replace("\n", string.Empty).Replace("\t", string.Empty).Replace("li>", string.Empty).Replace("<", string.Empty).Replace("span>", string.Empty).Replace(",", string.Empty).Trim();
                            }
                        }


                        catch { };

                        list.Clear();
                        #endregion

                        #region Designation Company Current
                        try
                        {
                            if (html.Contains("phonetic-full-name"))
                            {
                                int FirstPointForProfileCurrent = html.IndexOf("phonetic-full-name");
                                string FirstSubStringForProfileCurrent = html.Substring(FirstPointForProfileCurrent);
                                int SecondPointForProfileCurrent = FirstSubStringForProfileCurrent.IndexOf("display:block");
                                int ThirdPointForProfileCurrent = FirstSubStringForProfileCurrent.IndexOf("</p>");

                                string SecondSubStringForProfileCurrent = FirstSubStringForProfileCurrent.Substring(SecondPointForProfileCurrent, ThirdPointForProfileCurrent - SecondPointForProfileCurrent);
                                titlecurrent = SecondSubStringForProfileCurrent.Replace("\">", "").Replace("display:block", string.Empty).Replace("<strong class=\"highlight\"", string.Empty).Replace("</strong", string.Empty).Trim();
                                string[] tempCCurent = Regex.Split(titlecurrent, " at ");
                                LDS_HeadLineTitle = titlecurrent.Replace(",", ";");
                                LDS_CurrentCompany = tempCCurent[1].Replace(",", ";");

                            }

                            else if (html.Contains("<p class=\"title\""))
                            {
                                LDS_HeadLineTitle = html.Substring(html.IndexOf("<p class=\"title\""), 150);
                                string[] HeadLineTitle = LDS_HeadLineTitle.Split('>');
                                string tempHeadLineTitle = HeadLineTitle[1].Replace("\n", "").Replace(")</h3", "").Replace("</p", "");
                                LDS_HeadLineTitle = tempHeadLineTitle;
                                try
                                {
                                    string[] tempCCurent = Regex.Split(tempHeadLineTitle, " at ");
                                    LDS_HeadLineTitle = tempCCurent[0];
                                    LDS_CurrentCompany = tempCCurent[1];
                                }
                                catch { }
                            }


                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        #endregion

                        string GroupPastJob = string.Empty;
                        string GroupEduction = string.Empty;
                        LDS_PastTitles = titlepast1 + ";" + titlepast3;
                        LDS_PastCompany = companypast1 + ";" + companypast3;
                        LDS_Education = education1 + ";" + education2;
                        LDS_CurrentTitle = titleCurrenttitle;
                        LDS_LoginID = SearchCriteria.LoginID;                                                                                                                       //"ProfileType" + "," + "UserProfileLink" + "," + "FirstName" + "," + "LastName" + "," + "HeadLineTitle" + "," + "CurrentTitle " + "," + "Company" + "," + "Connection" + "," + "Recommendations " + "," + "SkillAndExpertise " + "," + "Experience " + "," + " Education" + "," + "Groups" + "," + "UserEmail" + "," + "UserContactNumbe" + "," + "PastTitles" + "," + "PastCompany" + "," + "Loction" + "," + "Country" + "," + "titlepast3" + "," + "companypast3" + "," + "titlepast4" + "," + "companypast4" + ",";
                        string LDS_FinalData = LDS_ProfileType.Replace(",", ";") + "," + LDS_UserProfileLink.Replace(",", ";") + "," + LDS_FirstName.Replace(",", ";") + "," + LDS_LastName.Replace(",", ";") + "," + LDS_HeadLineTitle.Replace(",", ";") + "," + LDS_CurrentTitle.Replace(",", ";") + "," + LDS_CurrentCompany.Replace(",", ";") + "," + LDS_Connection.Replace(",", ";") + "," + LDS_Recommendations.Replace(",", ";") + "," + LDS_SkillAndExpertise.Replace(",", ";") + "," + LDS_Experience.Replace(",", ";") + "," + LDS_Education.Replace(",", ";") + "," + LDS_Groups.Replace(",", ";") + "," + LDS_UserEmail.Replace(",", ";") + "," + LDS_UserContactNumber.Replace(",", ";") + "," + LDS_PastTitles.Replace(",", ";") + "," + LDS_PastCompany.Replace(",", ";") + "," + LDS_Loction.Replace(",", ";") + "," + LDS_Country.Replace(",", ";") + "," + LDS_Industry.Replace(",", ";") + "," + LDS_Websites.Replace(",", ";") + "," + LDS_LoginID.Replace(",", ";") + ",";

                        if (LDS_FinalData.Contains("<strong class=\"highlight\"") || LDS_FinalData.Contains("<span class=\"full-name\"") || LDS_FinalData.Contains("<strong class=\"highlight\"") || LDS_FinalData.Contains("overview-connections\">"))
                        {
                            LDS_FinalData = LDS_FinalData.Replace("<span class=\"full-name\"", "").Replace("\n", "").Replace("<strong class=\"highlight\"", "").Replace("overview-connections\">", "").Replace("</strong>", "").Replace("<strong>", "");
                        }
                        if (!string.IsNullOrEmpty(LDS_FirstName) || !string.IsNullOrEmpty(LDS_FirstName))
                        {
                            Log("[ " + DateTime.Now + " ] => [ " + LDS_FinalData + " ]");
                        }
                        // if (SearchCriteria.starter)
                        {
                            if (string.IsNullOrEmpty(LDS_LoginID))
                            {
                                LDS_LoginID = " ";
                            }

                            string tempFinalData = LDS_FinalData.Replace(";", "").Replace(LDS_UserProfileLink, "").Replace("Public", "").Replace(",", "").Replace(LDS_LoginID, "").Trim();

                            if (!string.IsNullOrEmpty(tempFinalData))
                            {


                                AppFileHelper.AddingLinkedInDataToCSVFile(LDS_FinalData, SearchCriteria.FileName);
                                Log("[ " + DateTime.Now + " ] => [ Data Saved In CSV File With URL : " + LDS_UserProfileLink + " ]");
                            }


                            if (!string.IsNullOrEmpty(tempFinalData))
                            {
                            try
                            {
                                if (status.Contains("LinkedinSearch_ProfileData"))
                                {
                                    try
                                    {
                                        AppFileHelper.AddingLinkedInDataToCSVFile(LDS_FinalData, Globals.path_LinkedinSearchByProfileURL);

                                        string CSVHeader = "ProfileType" + "," + "UserProfileLink" + "," + "FirstName" + "," + "LastName" + "," + "HeadLineTitle" + "," + "CurrentTitle " + "," + "Company" + "," + "Connection" + "," + "Recommendations " + "," + "SkillAndExpertise " + "," + "Experience " + "," + " Education" + "," + "Groups" + "," + "UserEmail" + "," + "UserContactNumber" + "," + "PastTitles" + "," + "PastCompany" + "," + "Location" + "," + "Country" + "," + "Industry" + "," + "WebSites" + "," + "LinkedInLoginID" + ",";
                                        string CSV_Content = LDS_ProfileType.Replace(",", ";") + "," + LDS_UserProfileLink.Replace(",", ";") + "," + LDS_FirstName.Replace(",", ";") + "," + LDS_LastName.Replace(",", ";") + "," + LDS_HeadLineTitle.Replace(",", ";") + "," + LDS_CurrentTitle.Replace(",", ";") + "," + LDS_CurrentCompany.Replace(",", ";") + "," + LDS_Connection.Replace(",", ";") + "," + LDS_Recommendations.Replace(",", ";") + "," + LDS_SkillAndExpertise.Replace(",", ";") + "," + LDS_Experience.Replace(",", ";") + "," + LDS_Education.Replace(",", ";") + "," + LDS_Groups.Replace(",", ";") + "," + LDS_UserEmail.Replace(",", ";") + "," + LDS_UserContactNumber.Replace(",", ";") + "," + LDS_PastTitles.Replace(",", ";") + "," + LDS_PastCompany.Replace(",", ";") + "," + LDS_Loction.Replace(",", ";") + "," + LDS_Country.Replace(",", ";") + "," + LDS_Industry.Replace(",", ";") + "," + LDS_Websites.Replace(",", ";") + "," + userName.Replace(",", ";") + ",";

                                        //string CSV_Content = TypeOfProfile + "," + LDS_UserProfileLink + "," + firstname + "," + lastname + "," + Company.Replace(",", ";") + "," + titlecurrent.Replace(",", ";") + "," + companycurrent.Replace(",", ";") + "," + Connection.Replace(",", ";") + "," + recomandation.Replace(",", string.Empty) + "," + Skill.Replace(",", ";") + "," + LDS_Experience.Replace(",", string.Empty) + "," + EducationCollection.Replace(",", ";") + "," + groupscollectin.Replace(",", ";") + "," + USERemail.Replace(",", ";") + "," + LDS_UserContact.Replace(",", ";") + "," + LDS_PastTitles + "," + AllComapny.Replace(",", ";") + "," + country.Replace(",", ";") + "," + location.Replace(",", ";") + "," + Industry.Replace(",", ";") + "," + Website.Replace(",", ";") + "," + LDS_LoginID + ",";// +TypeOfProfile + ",";


                                        CSVUtilities.ExportDataCSVFile(CSVHeader, CSV_Content, Globals.path_LinkedinSearchByProfileURL);

                                        Log("[ " + DateTime.Now + " ] => [ Record Saved In CSV File ! ]");
                                    }
                                    catch
                                    {
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        AppFileHelper.AddingLinkedInDataToCSVFile(LDS_FinalData, Globals.path_LinkedinSearchByPeople);

                                        string CSVHeader = "ProfileType" + "," + "UserProfileLink" + "," + "FirstName" + "," + "LastName" + "," + "HeadLineTitle" + "," + "CurrentTitle " + "," + "Company" + "," + "Connection" + "," + "Recommendations " + "," + "SkillAndExpertise " + "," + "Experience " + "," + " Education" + "," + "Groups" + "," + "UserEmail" + "," + "UserContactNumber" + "," + "PastTitles" + "," + "PastCompany" + "," + "Location" + "," + "Country" + "," + "Industry" + "," + "WebSites" + "," + "LinkedInLoginID" + ",";
                                        if (firstname == string.Empty) firstname = "LinkedIn";
                                        if (lastname == string.Empty) lastname = "Member";
                                        if (LDS_HeadLineTitle == string.Empty) LDS_HeadLineTitle = "--";
                                        if (LDS_CurrentTitle == string.Empty) LDS_CurrentTitle = "--";
                                        if (LDS_CurrentCompany == string.Empty) LDS_CurrentCompany = "--";
                                        if (LDS_Connection == string.Empty) LDS_Connection = "--";
                                        if (LDS_Recommendations == string.Empty) LDS_Recommendations = "--";
                                        if (LDS_SkillAndExpertise == string.Empty) LDS_SkillAndExpertise = "--";
                                        if (LDS_Experience == string.Empty) LDS_Experience = "--";
                                        if (LDS_Experience == string.Empty) LDS_Experience = "--";
                                        if (LDS_Education == string.Empty) LDS_Education = "--";
                                        if (LDS_Groups == string.Empty) LDS_Groups = "--";
                                        if (LDS_UserEmail == string.Empty) LDS_UserEmail = "--";
                                        if (LDS_UserContactNumber == string.Empty) LDS_UserContactNumber = "--";
                                        if (LDS_PastTitles == string.Empty) LDS_PastTitles = "--";
                                        if (LDS_Loction == string.Empty) LDS_Loction = "--";
                                        if (LDS_Country == string.Empty) LDS_Country = "--";
                                        if (LDS_Industry == string.Empty) LDS_Industry = "--";
                                        if (LDS_Websites == string.Empty) LDS_Websites = "--";

                                        string CSV_Content = LDS_ProfileType.Replace(",", ";") + "," + LDS_UserProfileLink.Replace(",", ";") + "," + LDS_FirstName.Replace(",", ";") + "," + LDS_LastName.Replace(",", ";") + "," + LDS_HeadLineTitle.Replace(",", ";") + "," + LDS_CurrentTitle.Replace(",", ";") + "," + LDS_CurrentCompany.Replace(",", ";") + "," + LDS_Connection.Replace(",", ";") + "," + LDS_Recommendations.Replace(",", ";") + "," + LDS_SkillAndExpertise.Replace(",", ";") + "," + LDS_Experience.Replace(",", ";") + "," + LDS_Education.Replace(",", ";") + "," + LDS_Groups.Replace(",", ";") + "," + LDS_UserEmail.Replace(",", ";") + "," + LDS_UserContactNumber.Replace(",", ";") + "," + LDS_PastTitles.Replace(",", ";") + "," + LDS_PastCompany.Replace(",", ";") + "," + LDS_Loction.Replace(",", ";") + "," + LDS_Country.Replace(",", ";") + "," + LDS_Industry.Replace(",", ";") + "," + LDS_Websites.Replace(",", ";") + "," + userName.Replace(",", ";") + ",";
                                        CSVUtilities.ExportDataCSVFile(CSVHeader, CSV_Content, Globals.path_LinkedinSearchByPeople);
                                        Log("[ " + DateTime.Now + " ] => [ Record Saved In CSV File ! ]");
                                    }
                                    catch
                                    {
                                    }
                                }
                            }
                            catch
                            {
                            }

                            //return true;
                        }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        } 
        #endregion

        #region SearchByCompany
        private void SearchByCompany(ref GlobusHttpHelper HttpHelper)
        {
            try
            {
              
                Log("[ " + DateTime.Now + " ] => [ Starting Search By Company With UserName : " + userName + " ]");
                _Search = _Search.ToLower();
                _Keyword = _Keyword.ToLower();
            
               string CompanySearchGetRequest = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/vsearch/c?type=companies&keywords=" + _Keyword.ToLower() + "&orig=GLHD&rsid=&pageKey=nprofile_v2_edit_fs"));
               //Temporosy code for client
               GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Comp Scpr Pagesource 1 >>>> " + CompanySearchGetRequest, Globals.Path_LinkedinCompanyScrapperPagesource);
               List<string> lstCompanyUrls = GettingAllUrlswithCompanyFilter(CompanySearchGetRequest);
               
                
                // call to GetDataFromCompanyURL
                GetDataFromCompanyURL(ref HttpHelper, lstCompanyUrls);

                // call to get page no

                int TotUrl = GetPageNo(CompanySearchGetRequest);
                int pagenumber = 0;

                try
                {
                    Log("[ " + DateTime.Now + " ] => [ Total Urls : " + TotUrl.ToString() + " ]");
                    pagenumber = int.Parse(TotUrl.ToString()) / 10 + 1;
                }
                catch (Exception)
                {

                }
           
                string GetResponce = string.Empty;
                string GetRequestURL = string.Empty;

                if (pagenumber > 1)
                {
                    try
                    {
                    
                        //string posturl = "http://www.linkedin.com/csearch/hits";
                        string posturl = "http://www.linkedin.com/vsearch/c?keywords=" + _Keyword + "";
                        for (int s = 2; s <= pagenumber; s++)
                        {
                            try
                            {
                                //l%26T
                                if (_Keyword.Contains("&"))
                                {
                                    _Keyword = _Keyword.Replace("&", "%26");

                                }
                                
                                string PostData = "&orig=GLHD&pageKey=voltron_company_search_internal_jsp&search=Search&openFacets=N,CCR,JO&page_num=" + s + "&pt=companies";
                                string Responses = HttpHelper.postFormData(new Uri(posturl), PostData);

                                //Temporosy code for client
                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Comp Scpr Pagesource 2 >>>> " + Responses, Globals.Path_LinkedinCompanyScrapperPagesource);

                                List<string> lstCompanyURLs = GettingAllUrlswithCompanyFilter(Responses);

                                if (lstCompanyURLs.Count == 0)
                                {
                                    lstCompanyURLs = GettingAllUrlswithCompanyFilter(Responses);
                                }

                                // call to GetDataFromCompanyURL
                                GetDataFromCompanyURL(ref HttpHelper, lstCompanyURLs);

                                int aa = s;

                            }
                            catch { }
                            
                        }
                    }
                    catch
                    {
                    }
                }

                string getsize = string.Empty;
                //obj_NewHireexcel.WritedataIntoExcel(Workbooks, Workbook, ComapanyName, link, industry, companysize, website, Founded, DateTime.Now.ToString(), Token1, Token2, Token3, Token4, starttime.ToString());
                try
                {
                    // getsize = companysize.Replace("employees", string.Empty).Trim();
                }
                catch { }
            }
            catch { }
            Log("[ " + DateTime.Now + " ] => [ " +  "PROCESS COMPLETED With UserName : " + userName + " ]");

        } 
        #endregion

        #region SearchByCompanyWithFilter
        private void SearchByCompanyWithFilter(ref GlobusHttpHelper HttpHelper)
        {
            try
            {
                LogScrap("[ " + DateTime.Now + " ] => [ Starting Search By Company With UserName : " + SearchCriteria.LoginID + " ]");
                _Search = _Search.ToLower();
                //_Keyword = _Keyword.ToLower();
                //_Keyword = _Keyword.Replace(" ", "+");
                string url = string.Empty;

                if (SearchCriteria.Relationship == "N")
                {
                    SearchCriteria.Relationship = "";
                }

                if (SearchCriteria.IndustryType == "Y")
                {
                    SearchCriteria.IndustryType = "";
                }

                if (SearchCriteria.CompanySize == "Y")
                {
                    SearchCriteria.CompanySize = "";
                }

                if (SearchCriteria.Follower == "Y")
                {
                    SearchCriteria.Follower = "";
                }
                if (SearchCriteria.Fortune1000 == "Y")
                {
                    SearchCriteria.Fortune1000 = "";
                }

                string getOthLocCode = string.Empty;
                string getlocurl = string.Empty;
                if (SearchCriteria.OtherLocation != string.Empty)
                {
                    getlocurl = "http://www.linkedin.com/ta/region?query="+ Uri.EscapeDataString(SearchCriteria.OtherLocation) + "";
                    getOthLocCode = HttpHelper.getHtmlfromUrl1(new Uri(getlocurl));
                    //Temporosy code for client
                    //GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Comp Scpr Pagesource 2 >>>> " + getOthLocCode, Globals.Path_LinkedinCompanyScrapperPagesource);

                    try
                    {
                        int startindex = getOthLocCode.IndexOf("id\":");
                        string start = getOthLocCode.Substring(startindex).Replace("id\":", string.Empty);
                        int endindex = start.IndexOf(",");
                        string othccountrycode = start.Substring(0, endindex).Replace("\"", string.Empty);
                        SearchCriteria.Country = othccountrycode.Replace(":", "%3A");
                    }
                    catch { }
             
                }

                if (SearchCriteria.Relationship == "" && SearchCriteria.Country == "" && SearchCriteria.IndustryType == "" && SearchCriteria.CompanySize == "" && SearchCriteria.Follower == "" && SearchCriteria.Fortune1000 == "")
                {
                    //url = "http://www.linkedin.com/vsearch/c?keywords=" + _Keyword + "&openFacets=N,CCR,JO,I,CS,NFR,F&orig=FCTD";
                    url = "http://www.linkedin.com/vsearch/c?type=companies&keywords=" + _Keyword + "&orig=GLHD&pageKey=voltron_company_search_internal_jsp&search=Search";
                }
                else
                {
                    //url = "http://www.linkedin.com/vsearch/c?keywords=" + _Keyword + "&openFacets=N,CCR,JO,I,CS,NFR,F&f_CCR=" + SearchCriteria.Country + "&f_I=" + SearchCriteria.IndustryType + "&f_CS=" + SearchCriteria.CompanySize + "&f_NFR=" + SearchCriteria.Follower + "&f_N=" + SearchCriteria.Relationship + "&f_F=" + SearchCriteria.Fortune1000 + "&orig=FCTD";
                    url = "http://www.linkedin.com/vsearch/c?type=companies&keywords=" + _Keyword + "&orig=GLHD&openFacets=N,CCR,JO,I,CS,NFR,F&f_CCR=" + SearchCriteria.Country + "&f_I=" + SearchCriteria.IndustryType + "&f_CS=" + SearchCriteria.CompanySize + "&f_NFR=" + SearchCriteria.Follower + "&f_N=" + SearchCriteria.Relationship + "&f_F=" + SearchCriteria.Fortune1000 + "";
                }
                       
               string CompanySearchGetRequest = HttpHelper.getHtmlfromUrl1(new Uri(url));

               //Temporosy code for client
               //GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Comp Scpr Pagesource 2 >>>> " + CompanySearchGetRequest, Globals.Path_LinkedinCompanyScrapperPagesource);

                List<string> lstCompanyUrls = GettingAllUrlswithCompanyFilter(CompanySearchGetRequest);

                ////// call to GetDataFromCompanyURL
                GetDataFromCompanyURLWithFilter(ref HttpHelper, lstCompanyUrls);

                // call to get page no
                int pagenumber = GetPageNo(CompanySearchGetRequest);

                try
                {
                    LogScrap("[ " + DateTime.Now + " ] => [ Total Urls : " + pagenumber.ToString() + " ]");
                    pagenumber = int.Parse(pagenumber.ToString())/10 + 1;
                }
                catch (Exception)
                {

                }
                //pagenumber = (pagenumber / 10) - 1;

                string GetResponce = string.Empty;
                string GetRequestURL = string.Empty;

                if (pagenumber > 1)
                {
                    try
                    {
                        string posturl = "http://www.linkedin.com/vsearch/c?keywords=" + _Keyword + "&orig=FCTD";

                      for (int s = 2; s <= pagenumber; s++)
                        {
                            try
                            {
                                //l%26T
                                if (_Keyword.Contains("&"))
                                {
                                    _Keyword = _Keyword.Replace("&", "%26");

                                }

                                string PostDAta = "&f_CCR=" + SearchCriteria.Country + "&f_I=" + SearchCriteria.IndustryType + "&f_CS=" + SearchCriteria.CompanySize + "&f_NFR=" + SearchCriteria.Follower + "&f_N=" + SearchCriteria.Relationship + "&f_F=" + SearchCriteria.Fortune1000 + "&page_num=" + s + "&pt=companies";
                                string Responses = HttpHelper.postFormData(new Uri(posturl), PostDAta);

                                //Temporosy code for client
                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Comp Scpr Pagesource 2 >>>> " + Responses, Globals.Path_LinkedinCompanyScrapperPagesource);

                                List<string> lstCompanyURLs = GettingAllUrlswithCompanyFilter(Responses);

                                if (lstCompanyURLs.Count == 0)
                                {
                                    lstCompanyURLs = GettingAllUrlswithCompanyFilter(Responses);
                                }

                                // call to GetDataFromCompanyURL
                                GetDataFromCompanyURLWithFilter(ref HttpHelper, lstCompanyURLs);
                            }
                            catch { }
                        }
                    }
                    catch
                    {
                    }
                }


                string getsize = string.Empty;
                //obj_NewHireexcel.WritedataIntoExcel(Workbooks, Workbook, ComapanyName, link, industry, companysize, website, Founded, DateTime.Now.ToString(), Token1, Token2, Token3, Token4, starttime.ToString());
                try
                {
                    // getsize = companysize.Replace("employees", string.Empty).Trim();
                }
                catch { }
            }
            catch { }

            LogScrap("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED With UserName : " + SearchCriteria.LoginID + " ]");

        }
        #endregion

        #region GettingAllUrls for parsing
        public List<string> GettingAllUrls(string PageSource, string MustMatchString)
        {

            List<string> suburllist = new List<string>();

            try
            {
                HtmlUtil htmlUtil = new HtmlUtil();
                PageSource = htmlUtil.EntityDecode(PageSource);
                StringArray datagoogle = htmlUtil.GetHyperlinkedUrls(PageSource);

                for (int i = 0; i < datagoogle.Length; i++)
                {
                    try
                    {
                        string hreflink = datagoogle.GetString(i);

                        if (hreflink.Contains(MustMatchString) && hreflink.Contains("vsrp_companies_res_sim"))
                        {


                            if (hreflink.Contains("http://www.linkedin.com"))
                            {
                                suburllist.Add(hreflink);
                                Log("[ " + DateTime.Now + " ] => [ URL : " + hreflink + " ]");
                            }
                            else
                            {
                                suburllist.Add("http://www.linkedin.com" + hreflink);
                                Log("[ " + DateTime.Now + " ] => [ URL : http://www.linkedin.com" + hreflink + " ]");
                            }
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
            suburllist = suburllist.Distinct().ToList();
            return suburllist.Distinct().ToList();
        }
        #endregion

        #region GettingAllUrlswithCompanyFilter
        public List<string> GettingAllUrlswithCompanyFilter(string PageSource)
        {

            List<string> suburllist = new List<string>();

            try
            {
               if (PageSource.Contains("&pid="))
                {
                   string[] trkArr = Regex.Split(PageSource, "&pid=");

                    foreach (string item in trkArr)
                    {
                        try
                        {
                            if (item.Contains("vsrp_companies_res_sim"))
                            {
                                //http://www.linkedin.com/company/1009?trk=vsrp_companies_res_name&trkInfo=VSRPsearchId%3A1652155531374817163809%2CVSRPtargetId%3A1009%2CVSRPcmpt%3Aprimary
                                string url = item.Substring(0, item.IndexOf("\"")).Replace("\"", string.Empty).Replace("vsrp_companies_res_sim","vsrp_companies_res_name").Replace("&","?").Trim();
                                string finalurl = "http://www.linkedin.com/company/" + url;
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

        #region GetPageNo
        private int GetPageNo(string PageSource)
        {
            int pageNo = 0;

            try
            {
                //int pagenumber = 0;
                string strPageNumber = string.Empty;
                //string[] Arr12 = Regex.Split(PageSource, "<p class=\"summary\">");
                string[] Arr12 = Regex.Split(PageSource, "resultCount");

                if (Arr12.Count() == 1)
                {
                    Arr12 = Regex.Split(PageSource, "<div id=\"results-summary\">");
                }

                foreach (string item in Arr12)
                {
                    try
                    {
                        if (!item.Contains("<!DOCTYPE"))
                        {
                            //if (item.Contains("Results"))
                            if (item.Contains("results for") || item.Contains("i18n_opportunities_interested_in"))
                            {
                                try
                                {
                                    string PageNO = Regex.Split(item, ",")[0].Replace(",", string.Empty).Replace("\"", string.Empty).Replace("<strong>", string.Empty).Replace("</strong>", string.Empty).Replace(":", string.Empty).Trim();
                                    pageNo = Convert.ToInt32(PageNO);
                                   
                                   

                                    string[] arrPageNO = Regex.Split(PageNO, "[^0-9]");
                                }
                                catch 
                                {
                                    string PageNO = Regex.Split(item, "i18n_opportunities_interested_in")[0].Replace(",", string.Empty).Replace("\"", string.Empty).Replace("<strong>", string.Empty).Replace("</strong>", string.Empty).Replace(":", string.Empty).Trim();
                                        pageNo = Convert.ToInt32(PageNO);
                                  
                                    string[] arrPageNO = Regex.Split(PageNO, "[^0-9]");
                                }

                                //try
                                //{
                                //    string pageNO = item.Substring(item.IndexOf("<strong>"), item.IndexOf("</strong>", item.IndexOf("<strong>")) - item.IndexOf("<strong>")).Replace("<strong>", string.Empty).Replace(",", string.Empty).Trim();

                                //    string[] arrPageNO = Regex.Split(pageNO, "[^0-9]");

                                //    foreach (string item1 in arrPageNO)
                                //    {
                                //        try
                                //        {
                                //            if (!string.IsNullOrEmpty(item1))
                                //            {
                                //                strPageNumber = item1;
                                //            }
                                //        }
                                //        catch
                                //        {
                                //        }
                                //    }
                                //}
                                //catch { }
                            }
                            else
                            {
                                try
                                {
                                    string pageNO = item.Substring(item.IndexOf("<p class=\"summary\">"), item.IndexOf("</p>", item.IndexOf("<p class=\"summary\">")) - item.IndexOf("<p class=\"summary\">")).Replace("<p class=\"summary\">", string.Empty).Replace(",", string.Empty).Replace("Results",string.Empty).Trim();
                                    string[] arrPageNO = Regex.Split(pageNO, "[^0-9]");
                                    pageNo = Convert.ToInt32(pageNO);
                                    foreach (string item1 in arrPageNO)
                                    {
                                        try
                                        {
                                            if (!string.IsNullOrEmpty(item1))
                                            {
                                                strPageNumber = item1;
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
                            }
                        }
                    }
                    catch
                    {
                    }
                }

                //if (Arr12.Count() == 1)
                //{
                //    Arr12 = Regex.Split(PageSource, "formattedResultCount");
                //}


                if (strPageNumber == string.Empty)
                {
                    foreach (string item in Arr12)
                    {
                        try
                        {

                            #region Loop
                            if (!item.Contains("<!DOCTYPE"))
                            {
                                if (item.Contains(":"))
                                {
                                    string data = RemoveAllHtmlTag.StripHTML(item);
                                    data = data.Replace("\n", "");
                                    if (data.Contains(">"))
                                    {
                                        string[] ArrTemp = data.Split(':');
                                        data = ArrTemp[1];
                                        data = data.Replace("results", "");
                                        data = data.Trim();
                                        string[] ArrTemp1 = data.Split(' ');
                                        data = ArrTemp1[0].Replace(',', ' ').Trim();
                                        strPageNumber = data.Replace(" ", string.Empty).Replace("\"", string.Empty).Replace("primaryUrlAlias", string.Empty).Replace("group_search_no_res_link", string.Empty).Replace("company_search_link", "").Trim();
                                        pageNo = Convert.ToInt32(strPageNumber);
                                        break;
                                    }

                                }
                            }
                            #endregion

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

            return pageNo;
        } 
        #endregion

        #region GetEmployeeDataFromCompanyURL
        private void GetEmployeeDataFromCompanyURL(ref GlobusHttpHelper HttpHelper, List<string> lstCompanyUrls)
        {
            try
            {
                foreach (string item in lstCompanyUrls)
                {
                    try
                    {
                        Log("[ " + DateTime.Now + " ] => [ Starting Parsing With UserName : " + SearchCriteria.LoginID + " ]");
                        //Log("Url >>> " + item);
                        string compCode = Regex.Split(item, "company/")[1];
                        compCode = compCode.Split('?')[0];
                        string EmployeeUrl = "https://www.linkedin.com/vsearch/p?keywords=" + SearchCriteria.Keyword + "&openAdvancedForm=true&locationType=" + SearchCriteria.Country + "&countryCode=" + SearchCriteria.Country + "&f_CC=" + compCode + "&rsid=&orig=ADVS";

                        string LinkPagesourceCompEmp = HttpHelper.getHtmlfromUrl1(new Uri(EmployeeUrl));

                        try
                        {
                            //string EmployeeUrl = string.Empty;
                            
                            #region commented old code
                            //string[] URL_Split = Regex.Split(LinkPagesource, "<li><a class=\"density\" ");
                            //foreach (string CompanyURl in URL_Split)
                            //{
                            //    try
                            //    {
                            //        if (CompanyURl.Contains("extra_biz_employees_deg_connected"))
                            //        {
                            //            int startIndex = CompanyURl.IndexOf("href=\"");
                            //            string start = CompanyURl.Substring(startIndex).Replace("href=\"", string.Empty);
                            //            int endIndex = start.IndexOf("\">");
                            //            string end = start.Substring(0, endIndex).Replace("\">", string.Empty).Trim();
                            //            EmployeeUrl = "https://www.linkedin.com" + end;
                            //        }
                            //    }
                            //    catch
                            //    { }
                            //} 
                            #endregion


                            int pagenumber = 0;
                            string strPageNumber = string.Empty;
                            string[] Arr12 = Regex.Split(LinkPagesourceCompEmp, "<p class=\"summary\">");

                            if (Arr12.Count() == 1)
                            {
                                Arr12 = Regex.Split(LinkPagesourceCompEmp, "formattedResultCount");
                            }

                            foreach (string item1 in Arr12)
                            {
                                try
                                {
                                    if (!item1.Contains("<!DOCTYPE"))
                                    {
                                        if (item1.Contains("<strong>"))
                                        {
                                            try
                                            {
                                                //":"15,439","i18n_survey_feedback_thanks":
                                                string pageNO = Regex.Split(item1, "i18n_survey")[0].Replace(":", string.Empty).Replace(",", string.Empty).Replace("\"", string.Empty);

                                                string[] arrPageNO = Regex.Split(pageNO, "[^0-9]");

                                                foreach (string item2 in arrPageNO)
                                                {
                                                    try
                                                    {
                                                        if (!string.IsNullOrEmpty(item2))
                                                        {
                                                            strPageNumber = item2;
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
                                        }
                                    }
                                }
                                catch
                                {
                                }
                            }

                            

                            try
                            {
                                strPageNumber = strPageNumber.Replace(".", string.Empty);
                                if (strPageNumber != string.Empty || strPageNumber == "0")
                                {
                                    Log("[ " + DateTime.Now + " ] => [ Total Results found: " + strPageNumber + " ]");
                                }
                                pagenumber = int.Parse(strPageNumber);
                            }
                            catch (Exception)
                            {

                            }

                            pagenumber = (pagenumber / 10) + 1;

                            LinkedInScrape obj_LinkedInScrape = new LinkedInScrape();

                            EmployeeUrl = EmployeeUrl + "##CompanyEmployeeScraper";

                            obj_LinkedInScrape.StartCompanyEmployeeScraperWithPagination(ref HttpHelper, EmployeeUrl, pagenumber);
                            
                        }
                        catch
                        { }
                    }
                    catch
                    { }
                }
            }
            catch { }
        }
            
        #endregion

        #region GetDataFromCompanyURL
        private void GetDataFromCompanyURL(ref GlobusHttpHelper HttpHelper, List<string> lstCompanyUrls)
        {
            try
            {
                foreach (string item in lstCompanyUrls)
                {
                    try
                    {
                        Log("[ " + DateTime.Now + " ] => [ Starting Parsing With UserName : " + userName + " ]");
                        //Log("Url >>> " + item);

                        string LinkPagesource = HttpHelper.getHtmlfromUrl1(new Uri(item));


                        #region Commented
                        //string titel = string.Empty;
                        //string type = string.Empty;
                        //string companysize = string.Empty;
                        //string website = string.Empty;
                        //string industry = string.Empty;
                        //string ComapanyName = string.Empty;
                        ////string Founded = string.Empty;

                        //bool success = false;
                        //string xHtml = string.Empty;

                        //Chilkat.HtmlToXml htmlToXml = new Chilkat.HtmlToXml();
                        //success = htmlToXml.UnlockComponent("THEBACHtmlToXml_7WY3A57sZH3O");
                        //if ((success != true))
                        //{
                        //    Console.WriteLine(htmlToXml.LastErrorText);
                        //    return;
                        //}

                        //htmlToXml.Html = LinkPagesource;

                        ////xHtml contain xml data 
                        //xHtml = htmlToXml.ToXml();

                        //Chilkat.Xml xml = new Chilkat.Xml();
                        //xml.LoadXml(xHtml);
                        //Chilkat.Xml xNode = default(Chilkat.Xml);

                        //List<string> list = new List<string>();
                        //Chilkat.Xml xBeginSearchAfter = null;
                        //string datacount = String.Empty;
                        //string datatype = string.Empty;

                        //xNode = xml.SearchForTag(xBeginSearchAfter, "h4");

                        //while ((xNode != null))
                        //{
                        //    try
                        //    {
                        //        datatype = xNode.AccumulateTagContent("text", "script|style");
                        //        list.Add(datatype);
                        //        xBeginSearchAfter = xNode;
                        //        xNode = xml.SearchForTag(xBeginSearchAfter, "h4");
                        //        titel = list[0];
                        //        xNode = xml.SearchForTag(xBeginSearchAfter, "p");
                        //        while ((xNode != null))
                        //        {
                        //            try
                        //            {
                        //                try
                        //                {
                        //                    datatype = xNode.AccumulateTagContent("text", "script|style");
                        //                    list.Add(datatype);
                        //                    xBeginSearchAfter = xNode;
                        //                    xNode = xml.SearchForTag(xBeginSearchAfter, "p");
                        //                }
                        //                catch { }
                        //                try
                        //                {
                        //                    ComapanyName = list[0];
                        //                }
                        //                catch { }
                        //                try
                        //                {
                        //                    type = list[1];
                        //                }
                        //                catch { }
                        //                try
                        //                {
                        //                    companysize = list[2];
                        //                   // empsize = empsize.Replace(" ", string.Empty).Trim();
                        //                }
                        //                catch { }
                        //                try
                        //                {
                        //                    website = list[3];
                        //                }
                        //                catch { }
                        //                try
                        //                {
                        //                    industry = list[4];
                        //                }
                        //                catch { }
                        //                try
                        //                {
                        //                   string Founded = list[5];
                        //                }
                        //                catch { }
                        //            }

                        //            catch (Exception ex)
                        //            {
                        //            }

                        //        }
                        //    }
                        //    catch (Exception ex)
                        //    {
                        //    }
                        //} 
                        #endregion

                        string CompanySize = string.Empty;
                        string Industry = string.Empty;
                        string Website = string.Empty;
                        string PhoneNo = string.Empty;
                        string Type = string.Empty;
                        string Founded = string.Empty;
                        string CompanyName = string.Empty;
                        string CompanyDesciption = string.Empty;
                        string Specialties = string.Empty;
                        string Addr1 = string.Empty;
                        string Addr2 = string.Empty;
                        string Addr3 = string.Empty;
                        string Addr4 = string.Empty;
                        string Addr5 = string.Empty;
                        string Address = string.Empty;

                        #region CompanyName
                        try
                        {
                            CompanyName = LinkPagesource.Substring(LinkPagesource.IndexOf("class=\"company-name\">"), (LinkPagesource.IndexOf("</h1>", LinkPagesource.IndexOf("class=\"company-name\">")) - LinkPagesource.IndexOf("class=\"company-name\">"))).Replace("class=\"company-name\">", string.Empty).Replace("amp;", string.Empty).Replace("<p>", string.Empty).Replace(": Overview | LinkedIn", string.Empty).Replace("&#x113;", "ē").Replace("&#39;", "'").Trim();
                        }
                        catch { }

                        try
                        {
                            CompanyName = LinkPagesource.Substring(LinkPagesource.IndexOf("<title>"), (LinkPagesource.IndexOf("</title>", LinkPagesource.IndexOf("<title>")) - LinkPagesource.IndexOf("<title>"))).Replace("<title>", string.Empty).Replace("<p>", string.Empty).Replace("amp;", string.Empty).Replace(": Overview | LinkedIn", string.Empty).Replace("&#x113;", "ē").Replace("&#39;", "'").Trim();

                            if (CompanyName.Contains("&"))
                            {
                                try
                                {
                                    CompanyName = CompanyName.Split('&')[0];
                                }
                                catch { }
                            }

                        }
                        catch { }
                        #endregion

                        #region CompanySize
                        try
                        {
                            CompanySize = LinkPagesource.Substring(LinkPagesource.IndexOf("<dt>Company Size</dt>"), (LinkPagesource.IndexOf("</dd>", LinkPagesource.IndexOf("<dt>Company Size</dt>")) - LinkPagesource.IndexOf("<dt>Company Size</dt>"))).Replace("<dt>Company Size</dt>", string.Empty).Replace("<dd>", string.Empty).Trim();
                        }
                        catch { }

                        try
                        {

                            CompanySize = LinkPagesource.Substring(LinkPagesource.IndexOf("<h4>Company Size</h4>"), (LinkPagesource.IndexOf("</p>", LinkPagesource.IndexOf("<h4>Company Size</h4>")) - LinkPagesource.IndexOf("<h4>Company Size</h4>"))).Replace("<h4>Company Size</h4>", string.Empty).Replace("<p>", string.Empty).Trim();
                        }
                        catch { }
                        #endregion

                        #region Website
                        try
                        {
                            Website = LinkPagesource.Substring(LinkPagesource.IndexOf("<dt>Website</dt>"), (LinkPagesource.IndexOf("</dd>", LinkPagesource.IndexOf("<dt>Website</dt>")) - LinkPagesource.IndexOf("<dt>Website</dt>"))).Replace("<dt>Website</dt>", string.Empty).Replace("<dd>", string.Empty).Trim();
                        }
                        catch { }

                        try
                        {
                            Website = LinkPagesource.Substring(LinkPagesource.IndexOf("<h4>Website</h4>"), (LinkPagesource.IndexOf("</p>", LinkPagesource.IndexOf("<h4>Website</h4>")) - LinkPagesource.IndexOf("<h4>Website</h4>"))).Replace("<h4>Website</h4>", string.Empty).Replace("<p>", string.Empty).Trim();
                            if (Website.Contains("a href="))
                            {
                                try
                                {
                                    string[] websArr = Regex.Split(Website, ">");
                                    Website = websArr[1].Replace("</a", string.Empty).Replace("\n", string.Empty).Trim(); ;
                                }
                                catch { }
                            }
                        }
                        catch { }
                        #endregion

                        
                        #region Industry
                        try
                        {
                            Industry = LinkPagesource.Substring(LinkPagesource.IndexOf("<dt>Industry</dt>"), (LinkPagesource.IndexOf("</dd>", LinkPagesource.IndexOf("<dt>Industry</dt>")) - LinkPagesource.IndexOf("<dt>Industry</dt>"))).Replace("<dt>Industry</dt>", string.Empty).Replace("<dd>", string.Empty).Replace("amp;", string.Empty).Trim();
                        }
                        catch { }

                        try
                        {
                            Industry = LinkPagesource.Substring(LinkPagesource.IndexOf("<h4>Industry</h4>"), (LinkPagesource.IndexOf("</p>", LinkPagesource.IndexOf("<h4>Industry</h4>")) - LinkPagesource.IndexOf("<h4>Industry</h4>"))).Replace("<h4>Industry</h4>", string.Empty).Replace("<p>", string.Empty).Replace("amp;", string.Empty).Trim();
                        }
                        catch { }
                        #endregion

                        #region Type
                        try
                        {
                            Type = LinkPagesource.Substring(LinkPagesource.IndexOf("<dt>Type</dt>"), (LinkPagesource.IndexOf("</dd>", LinkPagesource.IndexOf("<dt>Type</dt>")) - LinkPagesource.IndexOf("<dt>Type</dt>"))).Replace("<dt>Type</dt>", string.Empty).Replace("<dd>", string.Empty).Trim();

                        }
                        catch { }

                        try
                        {
                            Type = LinkPagesource.Substring(LinkPagesource.IndexOf("<h4>Type</h4>"), (LinkPagesource.IndexOf("</p>", LinkPagesource.IndexOf("<h4>Type</h4>")) - LinkPagesource.IndexOf("<h4>Type</h4>"))).Replace("<h4>Type</h4>", string.Empty).Replace("<p>", string.Empty).Trim();

                        }
                        catch { }
                        #endregion

                        #region Founded
                        try
                        {
                            Founded = LinkPagesource.Substring(LinkPagesource.IndexOf("<dt>Founded</dt>"), (LinkPagesource.IndexOf("</dd>", LinkPagesource.IndexOf("<dt>Founded</dt>")) - LinkPagesource.IndexOf("<dt>Founded</dt>"))).Replace("<dt>Founded</dt>", string.Empty).Replace("<dd>", string.Empty).Trim();
                        }
                        catch { }


                        try
                        {
                            Founded = LinkPagesource.Substring(LinkPagesource.IndexOf("<h4>Founded</h4>"), (LinkPagesource.IndexOf("</p>", LinkPagesource.IndexOf("<h4>Founded</h4>")) - LinkPagesource.IndexOf("<h4>Founded</h4>"))).Replace("<h4>Founded</h4>", string.Empty).Replace("<p>", string.Empty).Trim();
                        }
                        catch { }
                        #endregion

                        #region CompanyDesciption
                        try
                        {
                          //  CompanyDesciption = LinkPagesource.Substring(LinkPagesource.IndexOf("<div class=\"text-logo\">"), (LinkPagesource.IndexOf("</p>", LinkPagesource.IndexOf("<div class=\"text-logo\">")) - LinkPagesource.IndexOf("<div class=\"text-logo\">"))).Replace("<div class=\"text-logo\">", string.Empty).Replace("<p>", string.Empty).Replace("<br>", string.Empty).Replace("\n", string.Empty).Replace("\t", string.Empty).Replace("\r", string.Empty).Replace(",", ";").Trim();
                            CompanyDesciption = LinkPagesource.Substring(LinkPagesource.IndexOf("<div class=\"text-logo\">"), (LinkPagesource.IndexOf("</p>", LinkPagesource.IndexOf("<div class=\"text-logo\">")) - LinkPagesource.IndexOf("<div class=\"text-logo\">"))).Replace("<div class=\"text-logo\">", string.Empty).Replace("<p>", string.Empty).Replace("<br>", string.Empty).Replace("\n", string.Empty).Replace("\t", string.Empty).Replace("\r", string.Empty).Replace(",", ";").Replace("&quot;", "''").Replace("&#x2014;", "-").Replace("&#x201c;", "\"").Replace("&#x201d;", "\"").Replace("&#x2019;", "'").Replace("&#x2018;", "'").Replace("&#x113;", "ē").Replace("&#x2022;", "•").Replace("&#x2122;", "™").Replace("&#x20ac;", "€").Replace("&#xa9;", "©").Replace("&#xae;", "®")
                                                  .Replace("&#x2019;", "'").Replace("&#xe4;", "ä").Replace("&#x2013;", "-").Replace("&#39;", "'").Replace("&#x", "-").Replace("amp;", string.Empty).Trim();
                        }
                        catch { }
                        #endregion

                        #region Specialties
                        try
                        {
                           // Specialties = LinkPagesource.Substring(LinkPagesource.IndexOf("<h3>Specialties</h3>"), (LinkPagesource.IndexOf("</p>", LinkPagesource.IndexOf("<h3>Specialties</h3>")) - LinkPagesource.IndexOf("<h3>Specialties</h3>"))).Replace("<h3>Specialties</h3>", string.Empty).Replace("<p>", string.Empty).Replace("\n", string.Empty).Replace(",", ";").Trim();
                            Specialties = LinkPagesource.Substring(LinkPagesource.IndexOf("<h3>Specialties</h3>"), (LinkPagesource.IndexOf("</p>", LinkPagesource.IndexOf("<h3>Specialties</h3>")) - LinkPagesource.IndexOf("<h3>Specialties</h3>"))).Replace("<h3>Specialties</h3>", string.Empty).Replace("<p>", string.Empty).Replace("\n", string.Empty).Replace(",", ";").Replace("&quot;", "''").Replace("&#x2019;", "'").Replace("&#x2013;", "-").Replace("&#39;", "'").Replace("&#x", "-").Replace("amp;", string.Empty).Trim();
                        }
                        catch { }
                        #endregion

                        #region Headquarters
                        try
                        {

                            string[] vcard = Regex.Split(LinkPagesource, "class=\"vcard hq\">");

                            string[] finaladdr = Regex.Split(vcard[1], "class=");

                            foreach (string add in finaladdr)
                            {
                                if (add.Contains("street-address"))
                                {
                                    try
                                    {
                                        string a1 = add.Substring(add.IndexOf("\"street-address\">"), (add.IndexOf("</span>", add.IndexOf("\"street-address\">")) - add.IndexOf("\"street-address\">"))).Replace("\"street-address\">", string.Empty).Replace(",", ";").Trim();

                                        if (Addr1 == string.Empty)
                                        {
                                            Addr1 = a1;
                                        }
                                        else
                                        {
                                            Addr1 = Addr1 + ":" + a1;
                                        }
                                    }
                                    catch { }
                                }
                                else if (add.Contains("locality"))
                                {
                                    try
                                    {
                                        Addr2 = add.Substring(add.IndexOf("\"locality\">"), (add.IndexOf("</span>", add.IndexOf("\"locality\">")) - add.IndexOf("\"locality\">"))).Replace("\"locality\">", string.Empty).Replace(",", ";").Trim();
                                    }
                                    catch { }
                                }
                                else if (add.Contains("region"))
                                {
                                    try
                                    {
                                        Addr3 = add.Substring(add.IndexOf("\"region\" title=\""), (add.IndexOf(">", add.IndexOf("\"region\" title=\"")) - add.IndexOf("\"region\" title=\""))).Replace("\"region\" title=\"", string.Empty).Replace("\"", string.Empty).Replace(",", ";").Replace("itemprop=addressRegion", " ").Trim();
                                    }
                                    catch { }
                                }
                                else if (add.Contains("postal-code"))
                                {
                                    try
                                    {
                                        Addr4 = add.Substring(add.IndexOf("\"postal-code\">"), (add.IndexOf("</span>", add.IndexOf("\"postal-code\">")) - add.IndexOf("\"postal-code\">"))).Replace("\"postal-code\">", string.Empty).Replace(",", ";").Trim();
                                    }
                                    catch { }
                                }
                                else if (add.Contains("country-name"))
                                {
                                    try
                                    {
                                        Addr5 = add.Substring(add.IndexOf("\"country-name\">"), (add.IndexOf("</span>", add.IndexOf("\"country-name\">")) - add.IndexOf("\"country-name\">"))).Replace("\"country-name\">", string.Empty).Replace(",", ";").Trim();
                                        break;
                                    }
                                    catch { }
                                }
                            }

                            Address = Addr1 + " " + Addr2 + " " + Addr3 + " " + Addr4 + " " + Addr5;
                            Address = Address.Replace("itemprop=addressRegion", " ");

                        }
                        catch { }
                        #endregion

                        #region Csv writing
                        if (CompanyName != "Messages | LinkedIn")
                        {
                            string CSVHeader = "Company Name Title" + "," + "Company link to Linkedin Corp Profile" + "," + "Industry" + "," + "CompanyDescription" + "," + "Specialties" + "," + "Headquarters" + "," + "Company Size" + "," + "CORP Web Link" + "," + "Company Founded" + "," + "DateTime of Crawl" + "," + "LinkedInLoginID";
                            string CSV_Content = CompanyName.Replace(",", ";") + "," + item.Replace(",", ";") + "," + Industry.Replace(",", ";") + "," + CompanyDesciption.Replace(",", ";") + "," + Specialties.Replace(",", ";") + "," + Address.Replace(",", ";") + "," + CompanySize.Replace(",", ";") + "," + Website.Replace(",", ";") + "," + Founded.Replace(",", ";") + "," + DateTime.Now.ToString() + "," + userName;
                            CSVUtilities.ExportDataCSVFile(CSVHeader, CSV_Content, Globals.path_LinkedinSearchByCompany);
                            Log("[ " + DateTime.Now + " ] => [ Saved Data in URL : " + item + " ]");
                            Log("[ " + DateTime.Now + " ] => [ Company Name Title: " + CompanyName.ToString() + " ]");
                            Log("[ " + DateTime.Now + " ] => [ Industry: " + Industry + " ]");
                            Log("----------------------------------------------------------------------------------------------------------------------------------------------------");
                        } 
                        #endregion

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

        #region GetDataFromCompanyURLWithFilter
        private void GetDataFromCompanyURLWithFilter(ref GlobusHttpHelper HttpHelper, List<string> lstCompanyUrls)
        {
            try
            {
                foreach (string item in lstCompanyUrls)
                {
                    try
                    {
                        LogScrap("[ " + DateTime.Now + " ] => [ Starting Parsing Data With UserName : " + SearchCriteria.LoginID + " & URL : " + item + " ]");

                        string LinkPagesource = HttpHelper.getHtmlfromUrl1(new Uri(item));

                        string CompanySize = string.Empty;
                        string Industry = string.Empty;
                        string Website = string.Empty;
                        string PhoneNo = string.Empty;
                        string Type = string.Empty;
                        string Founded = string.Empty;
                        string CompanyName = string.Empty;
                        string CompanyDesciption = string.Empty;
                        string Specialties = string.Empty;
                        string StreetAddr = string.Empty;
                        string City = string.Empty;
                        string State = string.Empty;
                        string Zipcode = string.Empty;
                        string Country = string.Empty;
                        string Address = string.Empty;

                        #region CompanyName
                        try
                         {
                            CompanyName = LinkPagesource.Substring(LinkPagesource.IndexOf("class=\"company-name\">"), (LinkPagesource.IndexOf("</h1>", LinkPagesource.IndexOf("class=\"company-name\">")) - LinkPagesource.IndexOf("class=\"company-name\">"))).Replace("class=\"company-name\">", string.Empty).Replace("<p>", string.Empty).Replace(": Overview | LinkedIn", string.Empty).Replace("&#xe3;", "ã").Replace("&#xe7;", "ç").Replace("&#xf4;", "ô").Replace("&#xe9;", "é").Replace("&#xba;", "º").Replace("&#xc1;", "Á").Replace("&#xb4;", "'").Replace("&#xed;", "í").Replace("&#xf5;", "õ").Replace("&#xf3;", "ó").Replace("&#xe1;", "á").Replace("&#xea;", "ê").Replace("&#xe0;", "à").Replace("&#xfc;", "ü").Replace("&#xe4;", "ä").Replace("&#xf6;", "ö").Replace("&#xfa;", "ú").Replace("&#x113;", "e").Replace("&#39;", "'").Replace("&#xf4;", "ô").Replace("&#xc9;", "É").Replace("&#xe2;", "â").Trim();

                            if (CompanyName.Contains("&"))
                            {
                                CompanyName = CompanyName.Split('&')[0];
                            }
                        }
                        catch { }

                        try
                        {
                            CompanyName = LinkPagesource.Substring(LinkPagesource.IndexOf("<title>"), (LinkPagesource.IndexOf("</title>", LinkPagesource.IndexOf("<title>")) - LinkPagesource.IndexOf("<title>"))).Replace("<title>", string.Empty).Replace("<p>", string.Empty).Replace("amp;", string.Empty).Replace(": Overview | LinkedIn", string.Empty).Replace("&#xe3;", "ã").Replace("&#xe7;", "ç").Replace("&#xf4;", "ô").Replace("&#xe9;", "é").Replace("&#xba;", "º").Replace("&#xc1;", "Á").Replace("&#xb4;", "'").Replace("&#xed;", "í").Replace("&#xf5;", "õ").Replace("&#xf3;", "ó").Replace("&#xe1;", "á").Replace("&#xea;", "ê").Replace("&#xe0;", "à").Replace("&#xfc;", "ü").Replace("&#xe4;", "ä").Replace("&#xf6;", "ö").Replace("&#xfa;", "ú").Replace("&#xf4;", "ô").Replace("&#xc9;", "É").Replace("&#xe2;", "â").Replace("&#x113;", "e").Replace("&#39;", "'").Trim();

                            if (CompanyName.Contains("&"))
                            {
                                CompanyName = CompanyName.Split('&')[0];
                            }
                        }
                        catch { } 
                        #endregion

                        #region CompanySize
                        try
                        {
                            CompanySize = LinkPagesource.Substring(LinkPagesource.IndexOf("<dt>Company Size</dt>"), (LinkPagesource.IndexOf("</dd>", LinkPagesource.IndexOf("<dt>Company Size</dt>")) - LinkPagesource.IndexOf("<dt>Company Size</dt>"))).Replace("<dt>Company Size</dt>", string.Empty).Replace("<dd>", string.Empty).Replace("amp;", string.Empty).Trim();
                        }
                        catch { }

                        if (string.IsNullOrEmpty(CompanySize))
                        {
                            try
                            {

                                CompanySize = LinkPagesource.Substring(LinkPagesource.IndexOf("<h4>Company Size</h4>"), (LinkPagesource.IndexOf("</p>", LinkPagesource.IndexOf("<h4>Company Size</h4>")) - LinkPagesource.IndexOf("<h4>Company Size</h4>"))).Replace("<h4>Company Size</h4>", string.Empty).Replace("<p>", string.Empty).Replace("amp;", string.Empty).Trim();
                            }
                            catch { }
                        }

                        if (string.IsNullOrEmpty(CompanySize))
                        {
                            try
                            {

                                CompanySize = LinkPagesource.Substring(LinkPagesource.IndexOf("<li class=\"company-size\">"), (LinkPagesource.IndexOf("</p>", LinkPagesource.IndexOf("<li class=\"company-size\">")) - LinkPagesource.IndexOf("<li class=\"company-size\">"))).Replace("<li class=\"company-size\">", string.Empty).Replace("<p>", string.Empty).Replace("amp;", string.Empty).Trim();
                                string[] compsizeArr = Regex.Split(CompanySize, "</h4>");
                                CompanySize = compsizeArr[1].Replace("\n", string.Empty).ToString();
                            }
                            catch { }
                        }

                        #endregion

                        #region Website
                        try
                        {
                            Website = LinkPagesource.Substring(LinkPagesource.IndexOf("<dt>Website</dt>"), (LinkPagesource.IndexOf("</dd>", LinkPagesource.IndexOf("<dt>Website</dt>")) - LinkPagesource.IndexOf("<dt>Website</dt>"))).Replace("<dt>Website</dt>", string.Empty).Replace("<dd>", string.Empty).Trim();
                        }
                        catch { }

                        try
                        {
                            Website = LinkPagesource.Substring(LinkPagesource.IndexOf("<h4>Website</h4>"), (LinkPagesource.IndexOf("</p>", LinkPagesource.IndexOf("<h4>Website</h4>")) - LinkPagesource.IndexOf("<h4>Website</h4>"))).Replace("<h4>Website</h4>", string.Empty).Replace("<p>", string.Empty).Trim();
                            if (Website.Contains("a href="))
                            {
                                try
                                {
                                    string[] websArr = Regex.Split(Website, ">");
                                    Website = websArr[1].Replace("</a", string.Empty);
                                }
                                catch { }
                            }

                           
                        }
                        catch { }

                        try
                        {
                            if (string.IsNullOrEmpty(Website))
                            {
                                Website = LinkPagesource.Substring(LinkPagesource.IndexOf("<li class=\"website\">"), (LinkPagesource.IndexOf("</p>", LinkPagesource.IndexOf("<li class=\"website\">")) - LinkPagesource.IndexOf("<li class=\"website\">"))).Replace("<li class=\"website\">", string.Empty).Replace("<p>", string.Empty).Trim();
                                if (Website.Contains("a href="))
                                {
                                    try
                                    {
                                        string[] websArr = Regex.Split(Website, ">");
                                        Website = websArr[3].Replace("</a", string.Empty);
                                    }
                                    catch { }
                                }
                            }
                        }
                        catch { }

                        #endregion
                     
                        #region Industry
                        try
                        {
                            Industry = LinkPagesource.Substring(LinkPagesource.IndexOf("<dt>Industry</dt>"), (LinkPagesource.IndexOf("</dd>", LinkPagesource.IndexOf("<dt>Industry</dt>")) - LinkPagesource.IndexOf("<dt>Industry</dt>"))).Replace("<dt>Industry</dt>", string.Empty).Replace("<dd>", string.Empty).Replace("amp;", string.Empty).Trim();
                        }
                        catch { }

                        if (string.IsNullOrEmpty(Industry))
                        {
                            try
                            {
                                Industry = LinkPagesource.Substring(LinkPagesource.IndexOf("<h4>Industry</h4>"), (LinkPagesource.IndexOf("</p>", LinkPagesource.IndexOf("<h4>Industry</h4>")) - LinkPagesource.IndexOf("<h4>Industry</h4>"))).Replace("<h4>Industry</h4>", string.Empty).Replace("<p>", string.Empty).Replace("amp;", string.Empty).Trim();
                            }
                            catch { }
                        }

                        if (string.IsNullOrEmpty(Industry))
                        {
                            try
                            {
                                Industry = LinkPagesource.Substring(LinkPagesource.IndexOf("<li class=\"industry\">"), (LinkPagesource.IndexOf("</p>", LinkPagesource.IndexOf("<li class=\"industry\">")) - LinkPagesource.IndexOf("<li class=\"industry\">"))).Replace("<li class=\"industry\">", string.Empty).Replace("<p>", string.Empty).Replace("amp;", string.Empty).Trim();
                                string[] IndustryArr = Regex.Split(Industry, "</h4>");
                                Industry = IndustryArr[1].Replace("\n", string.Empty).ToString();
                            }
                            catch { }
                        }

                        #endregion

                        #region Type
                        try
                        {
                            Type = LinkPagesource.Substring(LinkPagesource.IndexOf("<dt>Type</dt>"), (LinkPagesource.IndexOf("</dd>", LinkPagesource.IndexOf("<dt>Type</dt>")) - LinkPagesource.IndexOf("<dt>Type</dt>"))).Replace("<dt>Type</dt>", string.Empty).Replace("<dd>", string.Empty).Trim();

                        }
                        catch { }

                        if (string.IsNullOrEmpty(Type))
                        {
                            try
                            {
                                Type = LinkPagesource.Substring(LinkPagesource.IndexOf("<h4>Type</h4>"), (LinkPagesource.IndexOf("</p>", LinkPagesource.IndexOf("<h4>Type</h4>")) - LinkPagesource.IndexOf("<h4>Type</h4>"))).Replace("<h4>Type</h4>", string.Empty).Replace("<p>", string.Empty).Trim();

                            }
                            catch { }
                        }

                        if (string.IsNullOrEmpty(Type))
                        {
                            try
                            {
                                Type = LinkPagesource.Substring(LinkPagesource.IndexOf("<h4>Tipo</h4>"), (LinkPagesource.IndexOf("</p>", LinkPagesource.IndexOf("<h4>Tipo</h4>")) - LinkPagesource.IndexOf("<h4>Tipo</h4>"))).Replace("<h4>Tipo</h4>", string.Empty).Replace("<p>", string.Empty).Trim();

                            }
                            catch { }
                        }

                        #endregion

                        #region Founded
                        try
                        {
                            Founded = LinkPagesource.Substring(LinkPagesource.IndexOf("<dt>Founded</dt>"), (LinkPagesource.IndexOf("</dd>", LinkPagesource.IndexOf("<dt>Founded</dt>")) - LinkPagesource.IndexOf("<dt>Founded</dt>"))).Replace("<dt>Founded</dt>", string.Empty).Replace("<dd>", string.Empty).Trim();
                        }
                        catch { }

                        if (string.IsNullOrEmpty(Founded))
                        {
                            try
                            {
                                Founded = LinkPagesource.Substring(LinkPagesource.IndexOf("<h4>Founded</h4>"), (LinkPagesource.IndexOf("</p>", LinkPagesource.IndexOf("<h4>Founded</h4>")) - LinkPagesource.IndexOf("<h4>Founded</h4>"))).Replace("<h4>Founded</h4>", string.Empty).Replace("<p>", string.Empty).Trim();
                            }
                            catch { }
                        }

                        if (string.IsNullOrEmpty(Founded))
                        {
                            try
                            {
                                Founded = LinkPagesource.Substring(LinkPagesource.IndexOf("<li class=\"founded\">"), (LinkPagesource.IndexOf("</p>", LinkPagesource.IndexOf("<li class=\"founded\">")) - LinkPagesource.IndexOf("<li class=\"founded\">"))).Replace("<li class=\"founded\">", string.Empty).Replace("<p>", string.Empty).Trim();
                                string[] FoundedArr = Regex.Split(Founded, "</h4>");
                                Founded = FoundedArr[1].Replace("\n", string.Empty).ToString();
                            }
                            catch { }
                        }

                        #endregion

                        #region CompanyDesciption
                        try
                        {
                            
                          //  CompanyDesciption = LinkPagesource.Substring(LinkPagesource.IndexOf("<div class=\"text-logo\">"), (LinkPagesource.IndexOf("</p>", LinkPagesource.IndexOf("<div class=\"text-logo\">")) - LinkPagesource.IndexOf("<div class=\"text-logo\">"))).Replace("<div class=\"text-logo\">", string.Empty).Replace("<p>", string.Empty).Replace("<br>", string.Empty).Replace("\n", string.Empty).Replace("\t", string.Empty).Replace("\r", string.Empty).Replace(",", ";").Replace("&quot;","''").Replace("amp;",string.Empty).Replace("&#x2019;","'").Trim();
                            CompanyDesciption = LinkPagesource.Substring(LinkPagesource.IndexOf("<div class=\"text-logo\">"), (LinkPagesource.IndexOf("</p>", LinkPagesource.IndexOf("<div class=\"text-logo\">")) - LinkPagesource.IndexOf("<div class=\"text-logo\">"))).Replace("<div class=\"text-logo\">", string.Empty).Replace("<p>", string.Empty).Replace("<br>", string.Empty).Replace("\n", string.Empty).Replace("\t", string.Empty).Replace("\r", string.Empty).Replace(",", ";").Replace("&quot;", "''").Replace("&#x2014;", "-").Replace("&#x201c;", "\"").Replace("&#x201d;", "\"").Replace("&#x2019;", "'").Replace("&#xe3;", "ã").Replace("&#xe7;", "ç").Replace("&#xf4;", "ô").Replace("&#xe9;", "é").Replace("&#xba;", "º").Replace("&#xc1;", "Á").Replace("&#xb4;", "'").Replace("&#xed;", "í").Replace("&#xf5;", "õ").Replace("&#xf3;", "ó").Replace("&#xe1;", "á").Replace("&#xea;", "ê").Replace("&#xe0;", "à").Replace("&#xfc;", "ü").Replace("&#xe4;", "ä").Replace("&#xf6;", "ö").Replace("&#xfa;", "ú").Replace("&#xf4;", "ô").Replace("&#xc9;", "É").Replace("&#xe2;", "â").Replace("&#x2018;", "'").Replace("&#x113;", "ē").Replace("&#x2022;", "•").Replace("&#x2122;", "").Replace("&#x20ac;", "").Replace("&#xa9;", "").Replace("&#xae;", "")
                                                   .Replace("&#x2019;", "'").Replace("&#x2013;", "-").Replace("&#39;", "'").Replace("&#x", "-").Replace("amp;", string.Empty).Trim();
                        }
                        catch { } 
                        #endregion

                        #region Specialties
                        try
                        {
                           Specialties = LinkPagesource.Substring(LinkPagesource.IndexOf("<h3>Specialties</h3>"), (LinkPagesource.IndexOf("</p>", LinkPagesource.IndexOf("<h3>Specialties</h3>")) - LinkPagesource.IndexOf("<h3>Specialties</h3>"))).Replace("<h3>Specialties</h3>", string.Empty).Replace("<p>", string.Empty).Replace("\n", string.Empty).Replace(",", ";").Replace("&quot;", "''").Replace("&#xe3;", "ã").Replace("&#xe7;", "ç").Replace("&#xf4;", "ô").Replace("&#xe9;", "é").Replace("&#xba;", "º").Replace("&#xc1;", "Á").Replace("&#xb4;", "'").Replace("&#xed;", "í").Replace("&#xf5;", "õ").Replace("&#xf3;", "ó").Replace("&#xe1;", "á").Replace("&#xea;", "ê").Replace("&#xe0;", "à").Replace("&#xfc;", "ü").Replace("&#xe4;", "ä").Replace("&#xf6;", "ö").Replace("&#xfa;", "ú").Replace("&#xf4;", "ô").Replace("&#xc9;", "É").Replace("&#xe2;", "â").Replace("&#x2019;", "'").Replace("&#x2013;", "-").Replace("&#39;", "'").Replace("&#x", "-").Replace("amp;", string.Empty).Trim();
                        }
                        catch { }

                        if (string.IsNullOrEmpty(Specialties))
                        {
                            try
                            {
                                Specialties = LinkPagesource.Substring(LinkPagesource.IndexOf("<div class=\"specialties\">"), (LinkPagesource.IndexOf("</p>", LinkPagesource.IndexOf("<div class=\"specialties\">")) - LinkPagesource.IndexOf("<div class=\"specialties\">"))).Replace("<div class=\"specialties\">", string.Empty).Replace("<p>", string.Empty).Replace("\n", string.Empty).Replace(",", ";").Replace("&quot;", "''").Replace("&#xe3;", "ã").Replace("&#xe7;", "ç").Replace("&#xf4;", "ô").Replace("&#xe9;", "é").Replace("&#xba;", "º").Replace("&#xc1;", "Á").Replace("&#xb4;", "'").Replace("&#xed;", "í").Replace("&#xf5;", "õ").Replace("&#xf3;", "ó").Replace("&#xe1;", "á").Replace("&#xea;", "ê").Replace("&#xe0;", "à").Replace("&#xfc;", "ü").Replace("&#xe4;", "ä").Replace("&#xf6;", "ö").Replace("&#xfa;", "ú").Replace("&#xf4;", "ô").Replace("&#xc9;", "É").Replace("&#xe2;", "â").Replace("&#x2019;", "'").Replace("&#x2013;", "-").Replace("&#39;", "'").Replace("&#x", "-").Replace("amp;", string.Empty).Trim();
                                string[] SpecialtiesArr = Regex.Split(Specialties, "</h3>");
                                Specialties = SpecialtiesArr[1].Replace("\n", string.Empty).ToString();
                            }
                            catch { }
                        }


                        #endregion

                        #region Headquarters
                        try
                        {
                            string[] vcard = Regex.Split(LinkPagesource, "class=\"vcard hq\">");
                            string[] finaladdr = Regex.Split(vcard[1], "class=");

                            foreach (string add in finaladdr)
                            {
                                if (add.Contains("street-address"))
                                {
                                    try
                                    {
                                        //string a1 = add.Substring(add.IndexOf("\"street-address\">"), (add.IndexOf("</span>", add.IndexOf("\"street-address\">")) - add.IndexOf("\"street-address\">"))).Replace("\"street-address\">", string.Empty).Replace(",", ";").Replace("&#xe3;", "ã").Replace("&#xe7;", "ç").Replace("&#xf4;", "ô").Replace("&#xe9;", "é").Replace("&#xba;", "º").Replace("&#xc1;", "Á").Replace("&#xb4;", "'").Replace("&#xed;", "í").Replace("&#xf5;", "õ").Replace("&#xf3;", "ó").Replace("&#xe1;", "á").Replace("&#xea;", "ê").Replace("&#xe0;", "à").Replace("&#xfc;", "ü").Replace("&#xe4;", "ä").Replace("&#xf6;", "ö").Replace("&#xfa;", "ú").Replace("&#xf4;", "ô").Replace("&#xc9;", "É").Replace("&#xe2;", "â").Trim();
                                        string a1 = add.Substring(add.IndexOf("\"streetAddress\">"), (add.IndexOf("</span>", add.IndexOf("\"streetAddress\">")) - add.IndexOf("\"streetAddress\">"))).Replace("\"streetAddress\">", string.Empty).Replace(",", ";").Replace("&#xe3;", "ã").Replace("&#xe7;", "ç").Replace("&#xf4;", "ô").Replace("&#xe9;", "é").Replace("&#xba;", "º").Replace("&#xc1;", "Á").Replace("&#xb4;", "'").Replace("&#xed;", "í").Replace("&#xf5;", "õ").Replace("&#xf3;", "ó").Replace("&#xe1;", "á").Replace("&#xea;", "ê").Replace("&#xe0;", "à").Replace("&#xfc;", "ü").Replace("&#xe4;", "ä").Replace("&#xf6;", "ö").Replace("&#xfa;", "ú").Replace("&#xf4;", "ô").Replace("&#xc9;", "É").Replace("&#xe2;", "â").Trim();
                                        int startindex = add.IndexOf("\"streetAddress\">");
                                        string start = add.Substring(startindex);
                                        int endindex = start.IndexOf("</span>");
                                        string a2 = start.Substring(0, endindex).Trim();
                                        if (StreetAddr == string.Empty)
                                        {
                                            StreetAddr = a1;
                                        }
                                        else
                                        {
                                            StreetAddr = StreetAddr + ":" + a1;
                                        }
                                    }
                                    catch { }
                                }
                                else if (add.Contains("locality"))
                                {
                                    try
                                    {
                                        //string aaa = add.Substring(add.IndexOf("\"locality\">"), (add.IndexOf("</span>", add.IndexOf("\"locality\">")) - add.IndexOf("\"locality\">"))).Replace("\"locality\">", string.Empty).Replace(",", ";");
                                        //City = add.Substring(add.IndexOf("\"locality\">"), (add.IndexOf("</span>", add.IndexOf("\"locality\">")) - add.IndexOf("\"locality\">"))).Replace("\"locality\">", string.Empty).Replace(",", ";").Replace("&#xe3;", "ã").Replace("&#xe7;", "ç").Replace("&#xf4;", "ô").Replace("&#xe9;", "é").Replace("&#xba;", "º").Replace("&#xc1;", "Á").Replace("&#xb4;", "'").Replace("&#xed;", "í").Replace("&#xf5;", "õ").Replace("&#xf3;", "ó").Replace("&#xe1;", "á").Replace("&#xea;", "ê").Replace("&#xe0;", "à").Replace("&#xfc;", "ü").Replace("&#xe4;", "ä").Replace("&#xf6;", "ö").Replace("&#xfa;", "ú").Replace("&#xf4;", "ô").Replace("&#xc9;", "É").Replace("&#xe2;", "â").Trim();
                                        string aaa = add.Substring(add.IndexOf("\"addressLocality\">"), (add.IndexOf("</span>", add.IndexOf("\"addressLocality\">")) - add.IndexOf("\"addressLocality\">"))).Replace("\"addressLocality\">", string.Empty).Replace(",", ";");
                                        City = add.Substring(add.IndexOf("\"addressLocality\">"), (add.IndexOf("</span>", add.IndexOf("\"addressLocality\">")) - add.IndexOf("\"addressLocality\">"))).Replace("\"addressLocality\">", string.Empty).Replace(",", ";").Replace("&#xe3;", "ã").Replace("&#xe7;", "ç").Replace("&#xf4;", "ô").Replace("&#xe9;", "é").Replace("&#xba;", "º").Replace("&#xc1;", "Á").Replace("&#xb4;", "'").Replace("&#xed;", "í").Replace("&#xf5;", "õ").Replace("&#xf3;", "ó").Replace("&#xe1;", "á").Replace("&#xea;", "ê").Replace("&#xe0;", "à").Replace("&#xfc;", "ü").Replace("&#xe4;", "ä").Replace("&#xf6;", "ö").Replace("&#xfa;", "ú").Replace("&#xf4;", "ô").Replace("&#xc9;", "É").Replace("&#xe2;", "â").Trim();
                                    }
                                    catch { }
                                }
                                else if (add.Contains("region"))
                                {
                                    try
                                    {
                                        //State = add.Substring(add.IndexOf("\"region\" title=\""), (add.IndexOf(">", add.IndexOf("\"region\" title=\"")) - add.IndexOf("\"region\" title=\""))).Replace("\"region\" title=\"", string.Empty).Replace("\"", string.Empty).Replace(",", ";").Replace("&#xe3;", "ã").Replace("&#xe7;", "ç").Replace("&#xf4;", "ô").Replace("&#xe9;", "é").Replace("&#xba;", "º").Replace("&#xc1;", "Á").Replace("&#xb4;", "'").Replace("&#xed;", "í").Replace("&#xf5;", "õ").Replace("&#xf3;", "ó").Replace("&#xe1;", "á").Replace("&#xea;", "ê").Replace("&#xe0;", "à").Replace("&#xfc;", "ü").Replace("&#xe4;", "ä").Replace("&#xf6;", "ö").Replace("&#xfa;", "ú").Replace("&#xf4;", "ô").Replace("&#xc9;", "É").Replace("&#xe2;", "â").Trim();
                                        State = add.Substring(add.IndexOf("\"addressRegion\">"), (add.IndexOf("<span", add.IndexOf("\"addressRegion\">")) - add.IndexOf("\"addressRegion\">"))).Replace("\"addressRegion\">", string.Empty).Replace("\"", string.Empty).Replace(",", ";").Replace("&#xe3;", "ã").Replace("&#xe7;", "ç").Replace("&#xf4;", "ô").Replace("&#xe9;", "é").Replace("&#xba;", "º").Replace("&#xc1;", "Á").Replace("&#xb4;", "'").Replace("&#xed;", "í").Replace("&#xf5;", "õ").Replace("&#xf3;", "ó").Replace("&#xe1;", "á").Replace("&#xea;", "ê").Replace("&#xe0;", "à").Replace("&#xfc;", "ü").Replace("&#xe4;", "ä").Replace("&#xf6;", "ö").Replace("&#xfa;", "ú").Replace("&#xf4;", "ô").Replace("&#xc9;", "É").Replace("&#xe2;", "â").Replace("</abbr>",string.Empty).Trim();
                                    }
                                    catch { }
                                }
                                else if (add.Contains("postal-code"))
                                {
                                    try
                                    {
                                        //Zipcode = add.Substring(add.IndexOf("\"postal-code\">"), (add.IndexOf("</span>", add.IndexOf("\"postal-code\">")) - add.IndexOf("\"postal-code\">"))).Replace("\"postal-code\">", string.Empty).Replace(",", ";").Trim();
                                        Zipcode = add.Substring(add.IndexOf("\"postalCode\">"), (add.IndexOf("</span>", add.IndexOf("\"postalCode\">")) - add.IndexOf("\"postalCode\">"))).Replace("\"postalCode\">", string.Empty).Replace(",", ";").Trim();
                                    }
                                    catch { }
                                }
                                else if (add.Contains("country-name"))
                                {
                                    try
                                    {
                                        //Country = add.Substring(add.IndexOf("\"country-name\">"), (add.IndexOf("</span>", add.IndexOf("\"country-name\">")) - add.IndexOf("\"country-name\">"))).Replace("\"country-name\">", string.Empty).Replace(",", ";").Replace("&#xe3;", "ã").Replace("&#xe7;", "ç").Replace("&#xf4;", "ô").Replace("&#xe9;", "é").Replace("&#xba;", "º").Replace("&#xc1;", "Á").Replace("&#xb4;", "'").Replace("&#xed;", "í").Replace("&#xf5;", "õ").Replace("&#xf3;", "ó").Replace("&#xe1;", "á").Replace("&#xea;", "ê").Replace("&#xe0;", "à").Replace("&#xfc;", "ü").Replace("&#xe4;", "ä").Replace("&#xf6;", "ö").Replace("&#xfa;", "ú").Replace("&#xf4;", "ô").Replace("&#xc9;", "É").Replace("&#xe2;", "â").Trim();
                                        Country = add.Substring(add.IndexOf("\"addressCountry\">"), (add.IndexOf("</span>", add.IndexOf("\"addressCountry\">")) - add.IndexOf("\"addressCountry\">"))).Replace("\"addressCountry\">", string.Empty).Replace(",", ";").Replace("&#xe3;", "ã").Replace("&#xe7;", "ç").Replace("&#xf4;", "ô").Replace("&#xe9;", "é").Replace("&#xba;", "º").Replace("&#xc1;", "Á").Replace("&#xb4;", "'").Replace("&#xed;", "í").Replace("&#xf5;", "õ").Replace("&#xf3;", "ó").Replace("&#xe1;", "á").Replace("&#xea;", "ê").Replace("&#xe0;", "à").Replace("&#xfc;", "ü").Replace("&#xe4;", "ä").Replace("&#xf6;", "ö").Replace("&#xfa;", "ú").Replace("&#xf4;", "ô").Replace("&#xc9;", "É").Replace("&#xe2;", "â").Trim();
                                        break;
                                    }
                                    catch { }
                                }
                            }

                            City = City.Replace(",", ";");
                            Address = StreetAddr + " " + City + " " + State + " " + Zipcode + " " + Country;


                        }
                        catch { } 
                        #endregion

                        #region Csv Writing
                        if (CompanyName != "Messages | LinkedIn")
                        {   //company name, industry, full address (separated in 4 fields: state, city, street and street number), phone number, website, e-mail, and company size (number of employees).
                            //string CSVHeader = "Company Name Title" + "," + "Company link to Linkedin Corp Profile" + "," + "Industry" + "," + "CompanyDescription" + "," + "Specialties" + "," + "Headquarters" + "," + "Company Size" + "," + "CORP Web Link" + "," + "Company Founded" + "," + "DateTime of Crawl" + "," + "LinkedInLoginID";
                            //string CSV_Content = CompanyName.Replace(",", ";") + "," + item.Replace(",", ";") + "," + Industry.Replace(",", ";") + "," + CompanyDesciption.Replace(",", ";") + "," + Specialties.Replace(",", ";") + "," + Address.Replace(",", ";") + "," + CompanySize.Replace(",", ";") + "," + Website.Replace(",", ";") + "," + Founded.Replace(",", ";") + "," + DateTime.Now.ToString() + "," + SearchCriteria.LoginID;

                            string CSVHeader = "Company Name" + "," + "Company link" + "," + "Industry" + "," + "Country" + "," + "State" + "," + "City" + "," + "ZipCode" + "," + "Street" + "," + "CompanyDescription" + "," + "Specialties" + "," + "Company Size" + "," + "CORP Web Link" + "," + "Company Founded" + "," + "DateTime of Crawl" + "," + "LinkedInLoginID";
                        //  System.IO.StreamReader CSV_Content = new System.IO.StreamReader();
                            string CSV_Content = (CompanyName.Replace(",", ";") + "," + item.Replace(",", ";") + "," + Industry.Replace(",", ";") + "," + Country.Replace(",", ";") + "," + State.Replace(",", ";") + "," + City.Replace(";", string.Empty) + "," + Zipcode.Replace(",", ";") + "," + StreetAddr.Replace(",", ";") + "," + CompanyDesciption.Replace(",", ";") + "," + Specialties.Replace(",", ";") + "," + CompanySize.Replace(",", ";") + "," + Website.Replace(",", ";") + "," + Founded.Replace(",", ";") + "," + DateTime.Now.ToString() + "," + SearchCriteria.LoginID);
                           
                            CSVUtilities.ExportDataCSVFile(CSVHeader, CSV_Content, Globals.path_LinkedinSearchByCompanyWithFilter);
                            LogScrap("[ " + DateTime.Now + " ] => [ Saved Data in csv File With UserName : " + SearchCriteria.LoginID + " and URL : " + item + " ]");
                        } 
                        #endregion
                        
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
            
        #region Scraper for Load Profile Data "GetProfileData"
        private void GetProfileData(ref GlobusHttpHelper HttpHelper)
        {
            try
            {
                string status = "LinkedinSearch_ProfileData";
                foreach (string item in lstLinkedinSearchProfileURL)
                {
                    try
                    {
                        //CrawlingLinkedInPage(item, ref HttpHelper,status);
                        //CrawlingPageDataSource(item, ref HttpHelper,status);
                        if (!CrawlingLinkedInPage(item, ref HttpHelper, status))
                        {
                            CrawlingPageDataSource(item, ref HttpHelper, status);
                        }
                    }
                    catch
                    {
                    }
                }

                Log("[ " + DateTime.Now + " ] => [ Parsing Completed For Profile Data ! ]");
            }
            catch
            {
            }
        } 
        #endregion

        #region GetProfileDataWithoutLoggingIn
        private void GetProfileDataWithoutLoggingIn(ref GlobusHttpHelper HttpHelper)
        {
            try
            {
                string status = "LinkedinSearch_ProfileData";
                foreach (string item in lstLinkedinSearchProfileURL)
                {
                    try
                    {
                        //CrawlingLinkedInPage(item, ref HttpHelper,status);
                        //CrawlingPageDataSource(item, ref HttpHelper,status);
                        if (!CrawlingLinkedInPage(item, ref HttpHelper, status))
                        {
                            CrawlingPageDataSource(item, ref HttpHelper, status);
                        }

                        

                    }
                    catch
                    {
                    }
                }

                Log("[ " + DateTime.Now + " ] => [ Parsing Completed For Profile Data ! ]");
            }
            catch
            {
            }
        }
        #endregion

        #region GetPhones
        public string GetPhones(string response)
        {
            List<string> lstPhones = new List<string>();
            string Phones = string.Empty;
            try
            {
                string data = response;//objSocialdata.Response;
                Regex regexObj = new Regex(@"^(?:(?:\+?1\s*(?:[.-]\s*)?)?(?:\(\s*([2-9]1[02-9]|[2-9][02-8]1|[2-9][02-8][02-9])\s*\)|([2-9]1[02-9]|[2-9][02-8]1|[2-9][02-8][02-9]))\s*(?:[.-]\s*)?)?([2-9]1[02-9]|[2-9][02-9]1|[2-9][02-9]{2})\s*(?:[.-]\s*)?([0-9]{4})(?:\s*(?:#|x\.?|ext\.?|extension)\s*(\d+))?$");

                //find items that matches with our pattern
                MatchCollection phonesMatches = regexObj.Matches(data);

                StringBuilder sb = new StringBuilder();

                foreach (Match emailMatch in phonesMatches)
                {
                    try
                    {
                        string phone = emailMatch.Value.Replace(response, "(+$1) $2 $3");

                        Phones = Phones + phone + "; ";
                        //lstPhones.Add(mp);
                    }
                    catch (Exception ex)
                    {
                        //GlobusLogHelper.log.Error(" Error : " + ex.StackTrace);
                    }

                }

                regexObj = new Regex(@"^\(?[+( ]?([0-9]{3})\)?[) ]?([0-9]{2})[- ]?([0-9]{7})$"); // For +type phone : "^\(?[+( ]?([0-9]{3})\)?[) ]?([0-9]{2})[- ]?([0-9]{7})$"

                //find items that matches with our pattern
                phonesMatches = regexObj.Matches(data);

                sb = new StringBuilder();

                foreach (Match emailMatch in phonesMatches)
                {
                    try
                    {
                        string phone = emailMatch.Value.Replace(response, "(+$1) $2 $3");

                        Phones = Phones + phone + "; ";
                        //lstPhones.Add(mp);
                    }
                    catch (Exception ex)
                    {
                        //GlobusLogHelper.log.Error(" Error : " + ex.StackTrace);
                    }

                }

                regexObj = new Regex(@"^([0-9\(\)\/\+ \-]*)$"); // For +type phone : "^\(?[+( ]?([0-9]{3})\)?[) ]?([0-9]{2})[- ]?([0-9]{7})$"

                //find items that matches with our pattern
                phonesMatches = regexObj.Matches(data);

                sb = new StringBuilder();

                foreach (Match emailMatch in phonesMatches)
                {
                    try
                    {
                        string phone = emailMatch.Value.Replace(response, "(+$1) $2 $3");

                        Phones = Phones + phone + "; ";
                        //lstPhones.Add(mp);
                    }
                    catch (Exception ex)
                    {
                        //GlobusLogHelper.log.Error(" Error : " + ex.StackTrace);
                    }

                }

                //Added by prabhat 14.02.14
                regexObj = new Regex("(?<NumOne>[0-9]+)(?<Operator>[^0-9])(?<NumTwo>[0-9]+)");
                phonesMatches = regexObj.Matches(data);

                sb = new StringBuilder();

                foreach (Match emailMatch in phonesMatches)
                {
                    try
                    {
                        string phone = emailMatch.Value.Replace(response, "(+$1) $2 $3");
                        if (phone.Contains("-"))
                        {
                            Phones = Phones + phone + "; ";
                        }
                        //lstPhones.Add(mp);
                    }
                    catch (Exception ex)
                    {
                        //GlobusLogHelper.log.Error(" Error : " + ex.StackTrace);
                    }

                }

                regexObj = new Regex(@"^\(?[+( ]?([0-9]{3})\)?[) ]?([0-9]{2})[- ]?([0-9]{7})$"); // For +type phone : "^\(?[+( ]?([0-9]{3})\)?[) ]?([0-9]{2})[- ]?([0-9]{7})$"

                Phones = Utils.ReplaceAllHtmlTagAndItsProperty(Phones).Replace(",", ";");
            }
            catch (Exception ex)
            {
                ////GlobusLogHelper.log.Error(" Error : " + ex.StackTrace);
            }

            //objSocialdata.ComapnyPhoneNumber = Phones;
            return Phones;
        } 
        #endregion

        #region logger Log
        public Events logger = new Events();

        private void Log(string log)
        {
            try
            {
                EventsArgs loggerEventsArgs = new EventsArgs(log);
                loggersearch.LogText(loggerEventsArgs);
            }
            catch (Exception ex)
            {

            }

        } 
        #endregion

        #region LogScrap Log
        // public Events logger = new Events();

        private void LogScrap(string log)
        {
            try
            {
                EventsArgs loggerEventsArgs = new EventsArgs(log);
                loggerscrap.LogText(loggerEventsArgs);
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region logger_addGroupCreateToLogger

        void logger_addSearchToLogger(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eventArgs = e as EventsArgs;
                Log(eventArgs.log);
            }
        }

        #endregion
        public static string getBetween(string strSource, string strStart, string strEnd)
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



     
    }
}
