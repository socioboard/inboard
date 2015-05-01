using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InBoardPro;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using BaseLib;

namespace InBoardPro
{
    public class StartLinkedinScrapping
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
        public static readonly object locker_pauseDialog = new object(); 
        #endregion

        #region StartLinkedinScrapping()
        public StartLinkedinScrapping()
        {

        } 
        #endregion

        #region StartLinkedScrapping
        public StartLinkedinScrapping(string UserName, string Password, string ProxyAddress, string ProxyPort, string ProxyUserName, string ProxyPassword, string PostalCode, string Distance, string Industry, string LastName)
        {
            try
            {
                this.accountUser = UserName;
                this.accountPass = Password;
                this.proxyAddress = ProxyAddress;
                this.proxyPort = ProxyPort;
                this.proxyUserName = ProxyUserName;
                this.proxyPassword = ProxyPassword;
                this.postalCode = PostalCode;
                this.distance = Distance;
                this.industryType = Industry;
                this.lastName = LastName;
            }
            catch { }
        } 
        #endregion

        #region ParsingInBoardProGetData
        public void ParsingOfInBoardProGetData(ref GlobusHttpHelper HttpHelper, string username, string password, string proxyaddress, string proxyport, string proxyUserName, string proxypassword, string postalcode, string distance, string industry, string lastname)
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
                        // Postalcode = Arr_Pst[0].Replace(" ", string.Empty).Trim();
                        // Country = Arr_Pst[1].Replace("{", string.Empty).Replace("}", string.Empty).Trim();
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
                    csrfToken = csrfToken.Replace("csrfToken=", string.Empty);
                    csrfToken = csrfToken.Replace("%3A", ":");
                    try
                    {
                        int endindex = csrfToken.IndexOf("\"");
                        string end = csrfToken.Substring(0, endindex).Replace("\"", string.Empty);
                        csrfToken = end.Trim();
                    }
                    catch
                    { }
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
                            //SearchCriteria.Country = item.Key;
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
                            //SearchCriteria.Country = item.Key;
                            countrycode = item.Key;
                            // countrycode = "us";
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
                    //LinkedinScrappDbManager objLsManager = new LinkedinScrappDbManager();
                    // objLsManager.InsertScarppRecordData(Postalcode, distance, industryType, lastName, RecordCount);

                }
                catch { }
                try
                {
                    LstUrlrecords = ScarppLinkedinDataByPageNumber(RecordCount, ref HttpHelper, Firstresponse, LastName, countrycode, Postalcode, distance, industrycode);
                    LstUrlrecords = LstUrlrecords.Distinct().ToList();
                }
                catch { }
                List<string> lstshorturl = new List<string>();
                foreach (string itemurl in LstUrlrecords)
                {
                    try
                    {
                        //string[] LinkUrll = Regex.Split(itemurl, "&authType");
                        lstshorturl.Add(itemurl);
                    }
                    catch { }

                }
                lstshorturl = lstshorturl.Distinct().ToList();
                // Log(" Get : " + lstshorturl.Count + " Linkedin Url Using UserName : " + accountUser);

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
                            //if (counterforpause <= processNumber)
                            //{
                            //    CrawlingLinkedInPage(LinkUrlParse, ref HttpHelper);
                            //    CrawlingPageDataSource(LinkUrlParse, ref HttpHelper);
                            //   string[] LinkUrllParse = Regex.Split(LinkUrlParse, "&authType");
                            //   objLsManager.Updatetb_LinkedinSearchUrlResult(postalCode, distance, industryType, lastName, accountUser, accountPass, proxyAddress, proxyPassword, RecordCount, LinkUrllParse[0]);
                            //    counterforpause++;
                            //}
                            CrawlingLinkedInPage(LinkUrlParse, ref HttpHelper);
                            //CrawlingPageDataSource(LinkUrlParse, ref HttpHelper);
                            //string[] LinkUrllParse = Regex.Split(LinkUrlParse, "&authType");
                            objLsManager.Updatetb_LinkedinSearchUrlResult(postalCode, distance, industryType, lastName, accountUser, accountPass, proxyAddress, proxyPassword, RecordCount, LinkUrlParse);
                            counterforpause++;
                            if (counterforpause <= processNumber)
                            {
                                //go on
                            }
                            else //go to delay
                            {

                                //{
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
                                        // IsStopScrapper = true;
                                        // DialogResult dlgResultforstop = MessageBox.Show("Do you want to Abort Process ?", " Stop ?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                        //if (dlgResultforstop == DialogResult.Yes)
                                        {
                                            foreach (Thread item in lstNumberOfThreads)
                                            {
                                                try
                                                {
                                                    item.Abort();
                                                }
                                                catch { }
                                            }
                                            Log("[ " + DateTime.Now + " ] => [ Process Stop ! ]");
                                            counterforpause = 1;
                                        }
                                        //    else if (dlgResultforstop == DialogResult.No)
                                        //    {
                                        //        foreach (Thread item in lstNumberOfThreads)
                                        //        {
                                        //            try
                                        //            {
                                        //                item.Suspend();
                                        //            }
                                        //            catch { }
                                        //        }
                                        //        Log("Process Pause !");
                                        //        counterforpause = 1;
                                        //    }
                                    }




                                    //foreach (Thread item in lstNumberOfThreads)
                                    //{
                                    //    try
                                    //    {
                                    //        item.Suspend();
                                    //    }
                                    //    catch { }
                                    //}


                                }
                                // }
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
        private List<string> ScarppLinkedinDataByPageNumber(int pagerecords, ref GlobusHttpHelper HttpHelper, string Pagesource, string LastName, string countrycd, string postalcd, string distance, string indstrycd)
        {                                                                                                                                                   
            List<string> lstRecordurl = new List<string>();
            try
            {
                string ResponseForPremiumAcc = Pagesource;
                int pagenumber = (pagerecords / 10) + 1;// (pagerecords / 10) - 1;

                string GetResponce = string.Empty;
                string GetRequestURL = string.Empty;
                string PostRequestURL = string.Empty;
                string PostdataForPagination = string.Empty;
                string PostResponce = string.Empty;
                if (pagenumber >= 1)//(pagenumber > 1)
                {
                    for (int i = 1; i <= pagenumber; i++)
                    {
                        PostRequestURL = string.Empty;
                        PostdataForPagination = string.Empty;
                        PostResponce = string.Empty;
                        //if (SearchCriteria.starter)
                        {
                            // #region loop

                            if (ResponseForPremiumAcc.Contains("Account Type:</span> Basic"))
                            {
                                try
                                {
                                    PostRequestURL = "http://www.linkedin.com/search/hits";
                                    PostdataForPagination = "lname=" + lastName + "&searchLocationType=I&countryCode=" + countrycd + "&postalCode=" + postalcd + "&distance=" + distance + "&keepFacets=keepFacets&facet_I=" + indstrycd + "&pplSearchOrigin=ADVS&viewCriteria=2&sortCriteria=R&facetsOrder=I%2CCC%2CN%2CPC%2CED%2CL%2CFG%2CTE%2CFA%2CSE%2CP%2CCS%2CF%2CDR%2CG&page_num=" + i + "&openFacets=I%2CCC%2CN";
                                    //PostdataForPagination = "keywords=" + _Keyword + "&searchLocationType=Y&search=&pplSearchOrigin=GLHD&viewCriteria=2&sortCriteria=R&facetsOrder=CC%2CN%2CG%2CI%2CPC%2CED%2CL%2CFG%2CTE%2CFA%2CSE%2CP%2CCS%2CF%2CDR&page_num=" + i + "&openFacets=CC%2CN%2CG";//"fname=" + SearchCriteria.FirstName + "&lname=" + SearchCriteria.LastName + "&searchLocationType=" + SearchCriteria.Location + "&countryCode=" + SearchCriteria.Country + "&viewCriteria=1&sortCriteria=R&facetsOrder=CC%2CN%2CG%2CI%2CPC%2CED%2CL%2CFG%2CTE%2CFA%2CSE%2CP%2CCS%2CF%2CDR&page_num=" + i + "&openFacets=N%2CCC%2CG";
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
                                    //PostdataForPagination = "keywords=" + _Keyword + "&searchLocationType=Y&search=&pplSearchOrigin=GLHD&viewCriteria=2&sortCriteria=R&facetsOrder=CC%2CN%2CG%2CI%2CPC%2CED%2CL%2CFG%2CTE%2CFA%2CSE%2CP%2CCS%2CF%2CDR&page_num=" + i + "&openFacets=CC%2CN%2CG";//"fname=" + SearchCriteria.FirstName + "&lname=" + SearchCriteria.LastName + "&searchLocationType=" + SearchCriteria.Location + "&countryCode=" + SearchCriteria.Country + "&viewCriteria=1&sortCriteria=R&facetsOrder=CC%2CN%2CG%2CI%2CPC%2CED%2CL%2CFG%2CTE%2CFA%2CSE%2CP%2CCS%2CF%2CDR&page_num=" + i + "&openFacets=N%2CCC%2CG";
                                    PostResponce = HttpHelper.postFormData(new Uri(PostRequestURL), PostdataForPagination);
                                    if (string.IsNullOrEmpty(PostResponce)) 
                                    {
                                        PostRequestURL = "http://www.linkedin.com/search/hits";
                                        //PostRequestURL = "http://www.linkedin.com/vsearch/p?";
                                        PostdataForPagination = "lname=" + lastName + "&searchLocationType=I&countryCode=" + countrycd + "&postalCode=" + postalcd + "&distance=" + distance + "&keepFacets=keepFacets&facet_I=" + indstrycd + "&pplSearchOrigin=ADVS&viewCriteria=2&sortCriteria=R&facetsOrder=I%2CCC%2CN%2CPC%2CED%2CL%2CFG%2CTE%2CFA%2CSE%2CP%2CCS%2CF%2CDR%2CG&page_num=" + i + "&openFacets=I%2CCC%2CN";
                                        //PostdataForPagination = "keywords=" + _Keyword + "&searchLocationType=Y&search=&pplSearchOrigin=GLHD&viewCriteria=2&sortCriteria=R&facetsOrder=CC%2CN%2CG%2CI%2CPC%2CED%2CL%2CFG%2CTE%2CFA%2CSE%2CP%2CCS%2CF%2CDR&page_num=" + i + "&openFacets=CC%2CN%2CG";//"fname=" + SearchCriteria.FirstName + "&lname=" + SearchCriteria.LastName + "&searchLocationType=" + SearchCriteria.Location + "&countryCode=" + SearchCriteria.Country + "&viewCriteria=1&sortCriteria=R&facetsOrder=CC%2CN%2CG%2CI%2CPC%2CED%2CL%2CFG%2CTE%2CFA%2CSE%2CP%2CCS%2CF%2CDR&page_num=" + i + "&openFacets=N%2CCC%2CG";
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
                                    //PostdataForPagination = "fname=" + SearchCriteria.FirstName + "&lname=" + SearchCriteria.LastName + "&searchLocationType=" + SearchCriteria.Location + "&countryCode=" + SearchCriteria.Country + "&keepFacets=keepFacets&I=" + SearchCriteria.IndustryType + "&SE=" + SearchCriteria.SeniorLevel + "&pplSearchOrigin=ADVS&viewCriteria=2&sortCriteria=R&facetsOrder=N%2CCC%2CI%2CPC%2CED%2CL%2CFG%2CTE%2CFA%2CSE%2CP%2CCS%2CF%2CDR%2CG&page_num=" + i + "&openFacets=N%2CCC%2CI";
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
                                    string[] Url_Split = Regex.Split(PostResponce, "link_nprofile_view_3");
                                    foreach(string item in Url_Split)
                                    {
                                        if (!item.Contains("<!DOCTYPE html>"))
                                        {
                                            try
                                            {
                                                int startIndex = item.IndexOf("\":\"");
                                                string Start = item.Substring(startIndex).Replace("\":\"", string.Empty);
                                                int endIndex = Start.IndexOf("\",\"");
                                                string End = Start.Substring(0, endIndex).Replace("\",\"", string.Empty).Trim();
                                                PageSerchUrl.Add(End);
                                            }
                                            catch
                                            { }
                                        }
                                    }
                                }

                                if (PageSerchUrl.Count == 0)
                                {

                                    //Log("On the basis of you Account you can able to see " + RecordURL.Count + "Results");
                                    break;
                                }
                                foreach (string item in PageSerchUrl)
                                {
                                    try
                                    {
                                        //if (SearchCriteria.starter)
                                        {
                                            if (item.Contains("authType=") && item.Contains("profile/view?id"))
                                            {
                                                string urlSerch = string.Empty;
                                                try
                                                {
                                                    //if (!item.Contains("http://www.linkedin.com"))
                                                    if (!item.Contains("//www.linkedin.com"))
                                                    {
                                                         urlSerch = "https://www.linkedin.com" + item;
                                                    }
                                                    else
                                                    {
                                                        urlSerch = item;
                                                    }
                                                    // Log(urlSerch);
                                                    //string[] aaa = Regex.Split(urlSerch, "&pvs");
                                                    lstRecordurl.Add(urlSerch);
                                                    lstRecordurl = lstRecordurl.Distinct().ToList();
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

                            else if (!string.IsNullOrEmpty(PostResponce) && !PostResponce.Contains("authType=") && PostResponce.Contains("Custom views are no longer supported. Please select either Basic or Expanded view"))
                            {
                                break;
                            }
                            if (string.IsNullOrEmpty(PostResponce))
                            {
                                string GetRequestUrl = "https://www.linkedin.com/vsearch/p?page_num=" + i + "&orig=ADVS&lastName=" + LastName + "&distance=" + distance + "&countryCode=" + countrycd + "&postalCode=" + postalcd + "&f_I=" + indstrycd;
                                string GetResponse = HttpHelper.getHtmlfromUrl1(new Uri(GetRequestUrl));
                                try
                                {
                                    if (GetResponse.Contains("link_nprofile_view_3"))
                                    {
                                        string[] PageSerchUrl = Regex.Split(GetResponse, "link_nprofile_view_3");
                                        if (PageSerchUrl.Length == 0)
                                        {
                                            break;
                                        }                                        
                                        foreach (string item in PageSerchUrl)
                                        {
                                            if (!item.Contains("<!DOCTYPE html>"))
                                            {
                                                try
                                                {
                                                    int startindex = item.IndexOf("\":\"");
                                                    string start = item.Substring(startindex).Replace("\":\"", string.Empty);
                                                    int endindex = start.IndexOf(",");
                                                    string end = start.Substring(0, endindex).Replace(",", string.Empty);
                                                    string Url = end.Trim();
                                                    if (Url.Contains("authType="))
                                                    {
                                                        string urlSerch = string.Empty;
                                                        try
                                                        {
                                                            if (!Url.Contains("//www.linkedin.com"))
                                                            {
                                                                urlSerch = "https://www.linkedin.com" + Url;
                                                            }
                                                            else
                                                            {
                                                                urlSerch = Url;
                                                            }
                                                            Log("[ URL scraped : " + urlSerch + " ]");
                                                            lstRecordurl.Add(urlSerch);
                                                            lstRecordurl = lstRecordurl.Distinct().ToList();
                                                        }
                                                        catch { }
                                                    }
                                                }
                                                catch
                                                {
                                                }
                                            }                                            
                                        }
                                        if (GetResponse.Contains("link_nprofile_view_headless"))
                                        {
                                            string[] PagerSerchUrl1 = Regex.Split(GetResponse, "link_nprofile_view_headless");
                                            foreach (string item in PagerSerchUrl1)
                                            {
                                                if (!item.Contains("<!DOCTYPE html>"))
                                                {
                                                    try
                                                    {
                                                        int startindex = item.IndexOf("\":\"");
                                                        string start = item.Substring(startindex).Replace("\":\"", string.Empty);
                                                        int endindex = start.IndexOf(",");
                                                        string end = start.Substring(0, endindex).Replace(",", string.Empty);
                                                        string Url = end.Trim();
                                                        if (Url.Contains("authType="))
                                                        {
                                                            string urlSerch = string.Empty;
                                                            try
                                                            {
                                                                if (!Url.Contains("//www.linkedin.com"))
                                                                {
                                                                    urlSerch = "https://www.linkedin.com" + Url;
                                                                }
                                                                else
                                                                {
                                                                    urlSerch = Url;
                                                                }
                                                                Log("[ URL scraped : " + urlSerch + " ]");
                                                                lstRecordurl.Add(urlSerch);
                                                                lstRecordurl = lstRecordurl.Distinct().ToList();
                                                            }
                                                            catch { }
                                                        }
                                                    }
                                                    catch
                                                    { }

                                                }
                                            }
                                        }

                                    }

                                }
                                catch
                                { }
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
                            //PostdataForPagination = "keywords=" + _Keyword + "&searchLocationType=Y&search=&pplSearchOrigin=GLHD&viewCriteria=2&sortCriteria=R&facetsOrder=CC%2CN%2CG%2CI%2CPC%2CED%2CL%2CFG%2CTE%2CFA%2CSE%2CP%2CCS%2CF%2CDR&page_num=" + i + "&openFacets=CC%2CN%2CG";//"fname=" + SearchCriteria.FirstName + "&lname=" + SearchCriteria.LastName + "&searchLocationType=" + SearchCriteria.Location + "&countryCode=" + SearchCriteria.Country + "&viewCriteria=1&sortCriteria=R&facetsOrder=CC%2CN%2CG%2CI%2CPC%2CED%2CL%2CFG%2CTE%2CFA%2CSE%2CP%2CCS%2CF%2CDR&page_num=" + i + "&openFacets=N%2CCC%2CG";
                            PostResponce = HttpHelper.postFormData(new Uri(PostRequestURL), PostdataForPagination);
                        }
                        catch { }
                    }
                    else if (ResponseForPremiumAcc.Contains("Account Type:</span> Executive"))
                    {
                        try
                        {
                            PostRequestURL = "http://www.linkedin.com/search/hits";
                            //PostdataForPagination = "fname=" + SearchCriteria.FirstName + "&lname=" + SearchCriteria.LastName + "&searchLocationType=" + SearchCriteria.Location + "&countryCode=" + SearchCriteria.Country + "&keepFacets=keepFacets&I=" + SearchCriteria.IndustryType + "&SE=" + SearchCriteria.SeniorLevel + "&pplSearchOrigin=ADVS&viewCriteria=2&sortCriteria=R&facetsOrder=N%2CCC%2CI%2CPC%2CED%2CL%2CFG%2CTE%2CFA%2CSE%2CP%2CCS%2CF%2CDR%2CG&page_num=" + i + "&openFacets=N%2CCC%2CI";
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
                                //if (SearchCriteria.starter)
                                {
                                    if (item.Contains("authType="))
                                    {
                                        string urlSerch = "http://www.linkedin.com" + item;
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

        #region Commented Old Code
        //private void ParsingUrlData(ref GlobusHttpHelper httpHelper, string ScrapUrl)
        //{
        //    #region Data Initialization

        //    string Industry = string.Empty;
        //    string URLprofile = string.Empty;
        //    string firstname = string.Empty;
        //    string lastname = string.Empty;
        //    string location = string.Empty;
        //    string country = string.Empty;
        //    string postal = string.Empty;
        //    string phone = string.Empty;
        //    string USERemail = string.Empty;
        //    string code = string.Empty;
        //    string education1 = string.Empty;
        //    string education2 = string.Empty;
        //    string titlecurrent = string.Empty;
        //    string companycurrent = string.Empty;
        //    string titlepast1 = string.Empty;
        //    string companypast1 = string.Empty;
        //    string titlepast2 = string.Empty;
        //    string html = string.Empty;
        //    string companypast2 = string.Empty;
        //    string titlepast3 = string.Empty;
        //    string companypast3 = string.Empty;
        //    string titlepast4 = string.Empty;
        //    string companypast4 = string.Empty;
        //    string Recommendations = string.Empty;
        //    string Connection = string.Empty;
        //    string Designation = string.Empty;
        //    string Website = string.Empty;
        //    string Contactsettings = string.Empty;
        //    string recomandation = string.Empty;

        //    string titleCurrenttitle = string.Empty;
        //    string titleCurrenttitle2 = string.Empty;
        //    string titleCurrenttitle3 = string.Empty;
        //    string titleCurrenttitle4 = string.Empty;
        //    string Skill = string.Empty;
        //    string TypeOfProfile = "Public";
        //    List<string> EducationList = new List<string>();
        //    string Finaldata = string.Empty;
        //    string EducationCollection = string.Empty;
        //    List<string> checkerlst = new List<string>();
        //    List<string> Checklist = new List<string>();
        //    List<string> checkgrplist = new List<string>();
        //    string groupscollectin = string.Empty;
        //    string strFamilyName = string.Empty;
        //    string LDS_LoginID = string.Empty;
        //    string LDS_Websites = string.Empty;
        //    string LDS_UserProfileLink = string.Empty;
        //    string LDS_CurrentTitle = string.Empty;
        //    string LDS_Experience = string.Empty;
        //    string LDS_UserContact = string.Empty;
        //    string LDS_PastTitles = string.Empty;
        //    string Company = string.Empty;
        //    List<string> lstpasttitle = new List<string>();
        //    List<string> checkpasttitle = new List<string>();
        //    #endregion
        //    string Pagesourceresponse = httpHelper.getHtmlfromUrl(new Uri(ScrapUrl));
        //    try
        //    {
        //        try
        //        {
        //            strFamilyName = Pagesourceresponse.Substring(Pagesourceresponse.IndexOf("fmt__full_name\":"), (Pagesourceresponse.IndexOf(",", Pagesourceresponse.IndexOf("fmt__full_name\":")) - Pagesourceresponse.IndexOf("fmt__full_name\":"))).Replace("fmt__full_name\":", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Trim();
        //        }
        //        catch
        //        {

        //        }
        //        #region Namesplitation
        //        string[] NameArr = new string[5];
        //        if (strFamilyName.Contains(" "))
        //        {
        //            try
        //            {
        //                NameArr = Regex.Split(strFamilyName, " ");
        //            }
        //            catch { }
        //        }
        //        #endregion

        //        #region FirstName
        //        firstname = NameArr[0];
        //        #endregion

        //        #region LastName
        //        lastname = NameArr[1];
        //        #endregion


        //        #region Company
        //        try
        //        {
        //            try
        //            {
        //                Company = Pagesourceresponse.Substring(Pagesourceresponse.IndexOf("visible\":true,\"memberHeadline"), (Pagesourceresponse.IndexOf("memberID", Pagesourceresponse.IndexOf("visible\":true,\"memberHeadline")) - Pagesourceresponse.IndexOf("visible\":true,\"memberHeadline"))).Replace("visible\":true,\"memberHeadline", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Replace("   ", string.Empty).Trim();
        //                string[] strdesigandcompany = new string[4];
        //                if (Company.Contains(" at "))
        //                {
        //                    try
        //                    {
        //                        strdesigandcompany = Regex.Split(Company, " at ");
        //                    }
        //                    catch { }

        //                    #region Title
        //                    try
        //                    {
        //                        titlecurrent = strdesigandcompany[0];
        //                    }
        //                    catch { }
        //                    #endregion

        //                    #region Current Company
        //                    try
        //                    {
        //                        companycurrent = strdesigandcompany[1];
        //                    }
        //                    catch { }
        //                    #endregion
        //                }
        //            }
        //            catch { }
        //            #region PastCompany
        //            string[] companylist = Regex.Split(Pagesourceresponse, "companyName\"");
        //            string AllComapny = string.Empty;

        //            string Companyname = string.Empty;
        //            foreach (string item in companylist)
        //            {
        //                try
        //                {
        //                    if (!item.Contains("<!DOCTYPE html>"))
        //                    {
        //                        Companyname = item.Substring(item.IndexOf(":"), (item.IndexOf(",", item.IndexOf(":")) - item.IndexOf(":"))).Replace(":", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Trim();
        //                        //Checklist.Add(item);
        //                        string items = item;
        //                        checkerlst.Add(Companyname);
        //                        checkerlst = checkerlst.Distinct().ToList();


        //                    }
        //                }
        //                catch { }
        //            }
        //            foreach (string item1 in checkerlst)
        //            {
        //                try
        //                {
        //                    //AllComapny = AllComapny + " : " + Companyname;
        //                    AllComapny = AllComapny + " : " + Companyname;
        //                }
        //                catch { }
        //            }
        //            #endregion
        //        #endregion Company

        //            #region Education
        //            try
        //            {
        //                string[] str_UniversityName = Regex.Split(Pagesourceresponse, "schoolName\"");
        //                foreach (string item in str_UniversityName)
        //                {
        //                    try
        //                    {
        //                        string University = string.Empty;
        //                        if (!item.Contains("<!DOCTYPE html>"))
        //                        {
        //                            try
        //                            {
        //                                University = item.Substring(item.IndexOf(":"), (item.IndexOf(",", item.IndexOf(":")) - item.IndexOf(":"))).Replace(":", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Trim();
        //                            }
        //                            catch { }
        //                            EducationList.Add(University);
        //                            EducationList = EducationList.Distinct().ToList();
        //                        }
        //                    }
        //                    catch { }
        //                }
        //                foreach (string item in EducationList)
        //                {
        //                    try
        //                    {
        //                        EducationCollection = EducationCollection + "  :  " + item;
        //                    }
        //                    catch { }
        //                }
        //                // string University1 = stringSource.Substring(stringSource.IndexOf("schoolName\":"), (stringSource.IndexOf(",", stringSource.IndexOf("schoolName\":")) - stringSource.IndexOf("schoolName\":"))).Replace("schoolName\":", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Trim();

        //            }

        //            catch { }

        //            #endregion Education
        //        }
        //        catch { }
        //    }
        //    catch { }
        //} 
        #endregion

        #region CrawlingLinkedInPage
        private void CrawlingLinkedInPage(string Url, ref GlobusHttpHelper HttpHelper)
        {
            //Log("Start Parsing Process");
            //  Workbooks myExcelWorkbooks = ClsExcelData.myExcelWorkbooks;
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
            string website1 = string.Empty;

            string titleCurrenttitle = string.Empty;
            string titleCurrenttitle2 = string.Empty;
            string titleCurrenttitle3 = string.Empty;
            string titleCurrenttitle4 = string.Empty;
            string Skill = string.Empty;
            string LDS_HeadLineTitle = string.Empty;
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
            List<string>titleList = new List<string>();
            List<string> companyList = new List<string>();
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
                    int startindex = stringSource.IndexOf("<span class=\"full-name\">");
                    if (startindex > 0)
                    {
                        string start = stringSource.Substring(startindex).Replace("<span class=\"full-name\">",string.Empty);
                        int endindex = start.IndexOf("</span>");
                        string end = start.Substring(0, endindex);
                        strFamilyName = end.Trim();
                    }
                }
                catch
                { }
                if (string.IsNullOrEmpty(strFamilyName))
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
                    
                        string[] cmpny = Regex.Split(stringSource, "trk=prof-exp-title' name='title' title='Find others with this title'>");
                        foreach (string item in cmpny)
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
                            { }
                        }
              
                    if (string.IsNullOrEmpty(Company))
                    {
                        try
                        {
                            Company = stringSource.Substring(stringSource.IndexOf("visible\":true,\"memberHeadline"), (stringSource.IndexOf("memberID", stringSource.IndexOf("visible\":true,\"memberHeadline")) - stringSource.IndexOf("visible\":true,\"memberHeadline"))).Replace("visible\":true,\"memberHeadline", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Replace("\\u002d", "-").Replace("   ", string.Empty).Trim();
                        }
                        catch { }
                    }

                    if (string.IsNullOrEmpty(Company))
                    {

                        try
                        {
                            Company = stringSource.Substring(stringSource.IndexOf("memberHeadline\":"), (stringSource.IndexOf(",", stringSource.IndexOf("memberHeadline\":")) - stringSource.IndexOf("memberHeadline\":"))).Replace("memberHeadline\":", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Replace("\\u002d", "-").Replace("   ", string.Empty).Replace("u002d",string.Empty).Trim();
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
                            if(!string.IsNullOrEmpty(strdesigandcompany[0]))
                            titlecurrent = strdesigandcompany[0];
                        }
                        catch { }
                        //if (string.IsNullOrEmpty(titlecurrent))
                        //{
                        //    titlecurrent = titleList[0];
                        //}
                        #endregion

                        #region Current Company
                        try
                        {
                            companycurrent = strdesigandcompany[1];
                        }
                        catch { }
                        #endregion
                    }

                    if (string.IsNullOrEmpty(titlecurrent))
                    {
                        titlecurrent = titleList[0];
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
                                ComapnyUrl = (start1.Substring(0, endIndex1).Replace("\"", string.Empty).Replace(":", string.Empty).Replace("&amp", "&").Replace("company_name", string.Empty).Replace(";", string.Empty).Replace("\\u002d", "-").Replace("name:", string.Empty).Replace("biz_logo","http://www.linkedin.com"));

                            }
                            catch { }

                            ////company_name":"Fathom: A Digital Marketing &amp; Analytics Agency","positionId":355409459
                            //Companyname = item.Substring(item.IndexOf("company_name"), (item.IndexOf(",", item.IndexOf(":")) - item.IndexOf(":"))).Replace(":", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Replace("]", string.Empty).Replace("}", string.Empty).Trim();
                            //string items = Companyname.Replace("company_name", string.Empty).Replace("positionId",string.Empty);
                            //checkerlst = checkerlst.Distinct().ToList();
                            checkerlst.Add(Companyname + "@" + ComapnyUrl);
                            checkerlst.Distinct().ToList();

                        }
                    }
                    catch { }
                }
                foreach (string item1 in checkerlst)
                {
                    try
                    {
                        string tempCompanylist = string.Empty;
                        tempCompanylist = item1.Split('@')[0];
                        companyList.Add(tempCompanylist);
                        companyList = companyList.Distinct().ToList();
                        
                        #region Commented code
                        //string compUrl = item1.Split('@')[1];
                        //string stringSourceCompUrl = HttpHelper.getHtmlfromUrl1(new Uri(compUrl));
                        //string companyUrl = string.Empty;

                        //try
                        //{
                        //    companyUrl = stringSourceCompUrl.Substring(stringSourceCompUrl.IndexOf("<h4>Website</h4>"), (stringSourceCompUrl.IndexOf("</a>", stringSourceCompUrl.IndexOf("<h4>Website</h4>")) - stringSourceCompUrl.IndexOf("<h4>Website</h4>"))).Replace("<h4>Website</h4>", string.Empty).Replace("</a>", string.Empty).Trim();
                        //    companyUrl = Regex.Split(companyUrl,"target=\"_blank\">")[1];
                        //}
                        //catch { }

                        //if (AllComapny == string.Empty)
                        //{
                        //AllComapny = item1.Split('@')[0] + "@" + companyUrl;
                        //tempCompanylist = item1.Split('@')[0];
                        //companyList.Add(tempCompanylist);
                        //companyList.Distinct().ToList();
                        //}
                        //else
                        //{
                        //    //AllComapny = AllComapny + " : " + item1.Split('@')[0] + "@" + companyUrl;
                        //    tempCompanylist = AllComapny + " : " + item1.Split('@')[0];
                        //    companyList.Add(tempCompanylist);
                        //    companyList.Distinct().ToList();
                        //} 
                        #endregion
                                               
                    }
                    catch { }
                }
                //if (string.IsNullOrEmpty(AllComapny) || AllComapny.Contains("<a href="))
                try
                {
                    string[] allCompany = Regex.Split(stringSource, "trk=prof-exp-company-name\" name=\"company\" title=\"Find others who have worked at this company\">");
                    foreach (string item in allCompany)
                    {
                        if (!item.Contains("<!DOCTYPE html>"))
                        {
                            try
                            {
                                int startindex = item.IndexOf("");
                                string start = item.Substring(startindex);
                                int endindex = start.IndexOf("</a>");
                                string end = start.Substring(0, endindex).Replace("</a>", string.Empty).Replace("<strong class=", string.Empty).Replace("</strong>", " ").Replace("\"highlight\"", string.Empty).Replace(">",string.Empty);
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
                    if (Company.Contains("<span") || string.IsNullOrEmpty(Company))
                    {
                        int startindex = stringSource.IndexOf("<p class=\"title \">");
                        string Start = stringSource.Substring(startindex).Replace("<p class=\"title \">", string.Empty);
                        int EndIndex = Start.IndexOf("</p>");
                        string End = Start.Substring(0, EndIndex).Replace("</p>", string.Empty).Replace("<strong class=\"highlight\">", string.Empty).Replace("</strong>", string.Empty).Replace("&#39;","'");
                        Company = End.Trim();
                    }
                    if (companyList.Count > 0)
                    {
                        AllComapny = string.Empty;
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
                catch { }
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

                    if (EducationList.Count == 0)
                    {
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

                                        try
                                        {
                                            int startindex2 = item.IndexOf("</time><time datetime=");
                                            string start2 = item.Substring(startindex2).Replace("</time><time datetime=", string.Empty);
                                            int endindex2 = start2.IndexOf(">");
                                            SessionEnd = start2.Substring(0, endindex2).Replace("\\u002d", string.Empty).Replace(":", string.Empty).Replace("\"", string.Empty).Replace(">", string.Empty).Replace("'", string.Empty);
                                        }
                                        catch { }

                                        if (SessionStart == string.Empty && SessionEnd == string.Empty)
                                        {
                                            Education = " [" + School + "] Degree: " + Degree;
                                        }
                                        else
                                        {
                                            if (Degree == string.Empty)
                                            {
                                                Education = " [" + School + "] Session: " + SessionStart + "-" + SessionEnd;
                                            }
                                            else
                                            {
                                                Education = " [" + School + "] Degree: " + Degree + " Session: " + SessionStart + "-" + SessionEnd;
                                            }

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
                if (string.IsNullOrEmpty(USERemail))
                {
                    try
                    {
                        int startindex = stringSource.IndexOf("Email:");
                        string start = stringSource.Substring(startindex).Replace("Email:", "");
                        int endindex = start.IndexOf("Phone");
                        string end = start.Substring(0, endindex).Replace("\u003cbr", "").Replace("\\u003cbr\\u003e", "").Replace("\\n", "").Replace(" ", "");
                        USERemail = end;
                    }
                    catch
                    {
                    }
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

                #endregion

            #region Website
               
               #region Website
                //try
                //{
                //    Website = stringSource.Substring(stringSource.IndexOf("[{\"URL\":"), (stringSource.IndexOf(",", stringSource.IndexOf("[{\"URL\":")) - stringSource.IndexOf("[{\"URL\":"))).Replace("[{\"URL\":", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace("]", string.Empty).Replace("}", string.Empty).Trim();
                //}
                //catch { }
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

                for (int i = 1; i <= size; i++)
                {
                    try
                    {
                        website1 += " - " + Websites[i];
                    }
                    catch { }
                }
                #endregion Website

            #endregion Website

            #region location
                try
                {
                    location = stringSource.Substring(stringSource.IndexOf("Country\",\"fmt__location\":"), (stringSource.IndexOf("i18n_no_location_matches", stringSource.IndexOf("Country\",\"fmt__location\":")) - stringSource.IndexOf("Country\",\"fmt__location\":"))).Replace("Country\",\"fmt__location\":", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Replace("\\u002d", "-").Trim();
                }
                catch { }
                // fmt_location

                if (string.IsNullOrEmpty(location))
                {
                    try
                    {
                        location = stringSource.Substring(stringSource.IndexOf("fmt_location\":"), (stringSource.IndexOf(",", stringSource.IndexOf("fmt_location\":")) - stringSource.IndexOf("fmt_location\":"))).Replace("fmt_location\":", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Replace("\\u002d", "-").Trim();

                    }
                    catch { }
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
                        string end = start1.Substring(0, endindex).Replace("\u002d", string.Empty).Replace("Å", "A").Replace("\\u002d", "-").Replace(">", string.Empty).Replace("/", string.Empty).Replace("\"", string.Empty).Replace("<strong class=",string.Empty).Replace("highlight",string.Empty).Replace("<strong",string.Empty);
                        location = end;
                    }
                    catch
                    { }
                }


                #endregion location

            #region state
                try
                {
                int startindexState = stringSource.IndexOf("location_highlight");
                string Statestart = stringSource.Substring(startindexState);
                int StateendIndex = Statestart.IndexOf("industry_highlight");
                state = (Statestart.Substring(0, StateendIndex).Replace("location_highlight",string.Empty).Replace(":",string.Empty).Replace("\"",string.Empty).Replace("deferImgtrue,",string.Empty));

                state = state.Split(',')[1].Replace("Area", string.Empty).Trim();
                }
                catch { }

                #endregion

            #region Country
                try
                {
                int startindexCountry = stringSource.IndexOf("displayCountry");
                string Countrystart = stringSource.Substring(startindexCountry);
                int CountryendIndex = Countrystart.IndexOf("displayLanguage");
                country = (Countrystart.Substring(0, CountryendIndex).Replace("displayCountry", string.Empty).Replace(":", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty));
               
                }
                catch { }
            if (string.IsNullOrEmpty(country))
            {
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
                    Industry = stringSource.Substring(stringSource.IndexOf("fmt__industry_highlight\":"), (stringSource.IndexOf(",", stringSource.IndexOf("fmt__industry_highlight\":")) - stringSource.IndexOf("fmt__industry_highlight\":"))).Replace("fmt__industry_highlight\":", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Replace("<strong class=highlight>", string.Empty).Replace("</strong>", string.Empty).Replace("\\u002d", "-").Trim();
                }
                catch { }

                if (string.IsNullOrEmpty(Industry))
                {
                    try
                    {
                        Industry = stringSource.Substring(stringSource.IndexOf("industry_highlight\":"), (stringSource.IndexOf(",", stringSource.IndexOf("industry_highlight\":")) - stringSource.IndexOf("industry_highlight\":"))).Replace("industry_highlight\":", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Replace("<strong class=highlight>", string.Empty).Replace("</strong>", string.Empty).Replace("\\u002d", "-").Trim();
                    }
                    catch { }
                    //industry_highlight":
                }
                if (string.IsNullOrEmpty(Industry))
                {
                    try
                    {
                        int startindex = stringSource.IndexOf("name=\"industry\" title=\"Find other members in this industry\">");
                        string start = stringSource.Substring(startindex).Replace("name=\"industry\" title=\"Find other members in this industry\">", string.Empty);
                        int endindex = start.IndexOf("</a>");
                        string end = start.Substring(0, endindex).Replace("</a>", string.Empty).Replace("&amp;", "&").Replace("<strong class=", string.Empty).Replace("\"highlight\"", string.Empty).Replace("</strong>", string.Empty).Replace(">", string.Empty);
                        Industry = end;
                    }
                    catch
                    { }
                }
                #endregion Industry

            #region Connection
                //try
                //{
                //    Connection = stringSource.Substring(stringSource.IndexOf("_count_string\":"), (stringSource.IndexOf(",", stringSource.IndexOf("_count_string\":")) - stringSource.IndexOf("_count_string\":"))).Replace("_count_string\":", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Trim();
                //}
                //catch { }
                ////numberOfConnections":
                //if (string.IsNullOrEmpty(Connection))
                //{
                //    try
                //    {
                //        Connection = stringSource.Substring(stringSource.IndexOf("numberOfConnections\":"), (stringSource.IndexOf("}", stringSource.IndexOf("numberOfConnections\":")) - stringSource.IndexOf("numberOfConnections\":"))).Replace("numberOfConnections\":", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Replace("\\u002d", "-").Replace("connectionsBrowseabletrue",string.Empty).Trim();

                //    }
                //    catch { }
                //}
                ////try
                ////{
                ////  Connection = stringSource.Substring(stringSource.IndexOf("_count_string\":"), (stringSource.IndexOf(",", stringSource.IndexOf("_count_string\":")) - stringSource.IndexOf("_count_string\":"))).Replace("_count_string\":", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Trim();
                ////}
                ////catch { }
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
                            string start = stringSource.Substring(startindex1).Replace("overview-connections", "").Replace("\n", string.Empty).Replace("<p>", string.Empty).Replace("</p>", string.Empty);
                            int endindex = start.IndexOf("</strong>");
                            string end = start.Substring(0, endindex).Replace("&amp;", "&").Replace("\"", string.Empty).Replace("<strong>", string.Empty).Replace(">", string.Empty).Trim();
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
                            string end = start.Substring(0, endindex).Replace("&amp;", "&").Replace("\"", string.Empty).Replace("<strong>", string.Empty).Replace(">", string.Empty).Trim();
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
                        RecomnedUrl = (start.Substring(0, endIndex).Replace(",",string.Empty).Replace("\"", string.Empty).Replace(":", string.Empty));

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
                    try
                    {
                        string[] array = Regex.Split(stringSource, "name='title' title='Find others with this title'>");
                        string exp = string.Empty;
                        string comp = string.Empty;
                        List<string> ListExperince = new List<string>();
                        List<string> ListCompany = new List<string>();
                        string SelItem = string.Empty;

                        foreach (var itemGrps in array)
                        {
                            try
                            {
                                if (!itemGrps.Contains("<!DOCTYPE html")) //">Join
                                {
                                    try
                                    {
                                        int startindex = itemGrps.IndexOf("");
                                        string start = itemGrps.Substring(startindex);
                                        int endIndex = start.IndexOf("</a>");
                                        exp = (start.Substring(0, endIndex).Replace("\"", string.Empty).Replace(":", string.Empty).Replace("&amp", "&").Replace(";", string.Empty).Replace("\\u002d", "-").Replace("</a>", string.Empty).Replace("name:", string.Empty));

                                    }
                                    catch { }

                                    try
                                    {
                                        int startindex1 = itemGrps.IndexOf("trk=prof-exp-company-name\">");
                                        string start1 = itemGrps.Substring(startindex1);
                                        int endIndex1 = start1.IndexOf("</a>");
                                        comp = (start1.Substring(0, endIndex1).Replace("trk=prof-exp-company-name\">", string.Empty).Replace(":", string.Empty).Replace(";", string.Empty).Replace("\\u002d", "-").Replace("</a>", string.Empty).Replace("name:", string.Empty)).Replace("\"", string.Empty);
                                        if (comp.Contains("<span"))
                                        {
                                            try
                                            {
                                                startindex1 = itemGrps.IndexOf("Find others who have worked at this company\">");
                                                start1 = itemGrps.Substring(startindex1);
                                                endIndex1 = start1.IndexOf("</a>");
                                                comp = (start1.Substring(0, endIndex1).Replace("Find others who have worked at this company\">",string.Empty).Replace("trk=prof-exp-company-name\">", string.Empty).Replace(":", string.Empty).Replace(";", string.Empty).Replace("\\u002d", "-").Replace("</a>", string.Empty).Replace("name:", string.Empty)).Replace("\"", string.Empty).Replace("<strong class=\"highlight\">", string.Empty).Replace("</strong>", string.Empty).Trim().Replace("<strong class=highlight>", string.Empty);
                                            }
                                            catch { }
                                        }
                                    }
                                    catch { }

                                    if (Company == string.Empty)
                                    {
                                        Company = comp;
                                    }

                                    if (LDS_HeadLineTitle == string.Empty)
                                    {
                                        LDS_HeadLineTitle = exp;
                                    }

                                    ListExperince.Add(exp + ":" + comp);
                                    ListCompany.Add(comp);
                                    companycurrent = ListCompany[0];
                                    if (companycurrent.Contains("span data-tracking") || string.IsNullOrEmpty(companycurrent))
                                    {
                                        try
                                        {
                                            if (companyList.Count > 0)
                                            {
                                                companycurrent = companyList[0];
                                            }
                                        }
                                        catch
                                        { }
                                    }

                                }
                            }
                            catch { }
                        }
                        if (ListExperince.Count > 0)
                        {
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
                        GroupUrl = (start.Substring(0, endIndex).Replace(",", string.Empty).Replace("\"", string.Empty).Replace("templateId:profile_v2_connectionsurl:",string.Empty));

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
                                        string Grp = item.Substring(item.IndexOf(">"), (item.IndexOf("<", item.IndexOf(">")) - item.IndexOf(">"))).Replace(">", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Trim();
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
                                                //string Grp = skillitem.Substring(skillitem.IndexOf(":"), (skillitem.IndexOf("}", skillitem.IndexOf(":")) - skillitem.IndexOf(":"))).Replace(":", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Trim();
                                                //checkgrplist.Add(Grp);
                                                //checkgrplist.Distinct().ToList();
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
                                pstTitlesitem = Past_Ttl[0].Replace(":",string.Empty).Replace("\"",string.Empty).Replace("\\u002d", "-").Replace("&amp;","&");
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
                    string[] UrlFull = System.Text.RegularExpressions.Regex.Split(Url, "&authType");
                    LDS_UserProfileLink = UrlFull[0];
                    LDS_UserProfileLink = Url;
                  
                }
                catch { }
                #endregion

            #region Data Saved In CSV File

                companycurrent = companycurrent.Replace("&amp;","&").Replace("<strong class=highlight>", string.Empty).Replace("</strong>", string.Empty);
                LDS_Experience = LDS_Experience.Replace("&amp;","&").Replace("<strong class=highlight>", string.Empty).Replace("</strong>", string.Empty);
                AllComapny = AllComapny.Replace("&amp;","&").Replace("<strong class=highlight>", string.Empty).Replace("</strong>", string.Empty);
                LDS_PastTitles = LDS_PastTitles.Replace("<strong class=\"highlight\">", string.Empty).Replace("<strong class=highlight>", string.Empty);

                if (!string.IsNullOrEmpty(firstname) || !string.IsNullOrEmpty(lastname) || !string.IsNullOrEmpty(Company))
                {
                    if (postalCode == string.Empty) postalCode = "--";
                    if (distance == string.Empty) distance = "--";
                    if (Company == string.Empty) Company = "--";
                    if (titlecurrent == string.Empty) titlecurrent = "--";
                    if (companycurrent == string.Empty) companycurrent = "--";
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
                    if (state == string.Empty) state = "--";
                    if (country == string.Empty) country = "--";
                    if (Industry == string.Empty) Industry = "--";
                    if (website1 == string.Empty) website1 = "--";
                    try
                    {
                           string CSVHeader = "ProfileType" + "," + "UserProfileLink" + "," + "FirstName" + "," + "LastName" + "," + "PostalCode" + "," + "Distance From Postal" + "," + "HeadLineTitle" + "," + "CurrentTitle " + "," + "Company" + "," + "Connection" + "," + "Recommendations " + "," + "SkillAndExpertise " + "," + "Experience " + "," + " Education" + "," + "Groups" + "," + "UserEmail" + "," + "UserContactNumber" + "," + "PastTitles" + "," + "PastCompany" + "," + "City" + "," + "State" + "," + "Country" + "," + "Industry" + "," + "WebSites" + "," + "LinkedInLoginID" + "," + "DateTimeOfCrawl" +",";
                           string CSV_Content = TypeOfProfile + "," + LDS_UserProfileLink + "," + firstname.Replace(",", ";").Trim() + "," + lastname.Replace(",", ";").Trim() + "," + postalCode + "," + distance + "," + Company.Replace("&amp;", "&").Replace(",", ";") + "," + titlecurrent.Replace(",", ";") + "," + companycurrent.Replace("&amp;", "&").Replace(",", ";") + "," + Connection.Replace(",", ";") + "," + recomandation.Replace(",", string.Empty) + "," + Skill.Replace(",", ";") + "," + LDS_Experience.Replace(",", string.Empty) + "," + EducationCollection.Replace(",", ";") + "," + groupscollectin.Replace(",", ";") + "," + USERemail.Replace(",", ";") + "," + LDS_UserContact.Replace(",", ";") + "," + LDS_PastTitles + "," + AllComapny.Replace(",", ";") + "," + location.Replace(",", ";") + "," + state.Replace(",", ";") + "," + country.Replace(",", ";") + "," + Industry.Replace(",", ";") + "," + website1.Replace(",", ";") + "," + accountUser + "," + DateTime.Now;// +TypeOfProfile + ",";
                        CSVUtilities.ExportDataCSVFile(CSVHeader, CSV_Content, Globals.path_InBoardProGetDataResultUrlData);
                        Log("[ " + DateTime.Now + " ] => [ Data Saved In CSV File ! ]");
                    }
                    catch { }
                } 

                #endregion
                

            } 

        #endregion

        #region CrawlingPageDataSource
        private void CrawlingPageDataSource(string Url, ref GlobusHttpHelper HttpHelper)
        {
            //if (SearchCriteria.starter)
            {
                // if (SearchCriteria.starter)
                {
                    try
                    {
                        //  Log("Start Parsing Process");

                        #region Data Initialization
                        ChilkatHttpHelpr objChilkat = new ChilkatHttpHelpr();
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

                        string titleCurrenttitle = string.Empty;
                        string titleCurrenttitle2 = string.Empty;
                        string titleCurrenttitle3 = string.Empty;
                        string titleCurrenttitle4 = string.Empty;
                        string Skill = string.Empty;
                        string TypeOfProfile = "Public1";

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
                                //qm.AddProfileUrl(URLprofile, DateTime.Now.ToString(), "0");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }



                        try
                        {
                            //string[] UrlFull = System.Text.RegularExpressions.Regex.Split(Url, "&authType");
                            LDS_UserProfileLink = Url;//UrlFull[0];
                        }
                        catch { }
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
                                    LDS_Connection = html.Substring(html.IndexOf("overview-connections"), 50);
                                    string[] Arr = Connection.Split('>');
                                    string tempConnection = Arr[3].Replace("</strong", "").Replace(")</h3", "").Replace("(", "");
                                    LDS_Connection = tempConnection + "Connection";
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


                        string companyCurrenttitle1 = string.Empty;

                        string companyCurrenttitle2 = string.Empty;

                        string companyCurrenttitle3 = string.Empty;

                        string companyCurrenttitle4 = string.Empty;


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

                        string GroupPastJob = string.Empty;
                        string GroupEduction = string.Empty;
                        LDS_PastTitles = titlepast1 + ";" + titlepast3;
                        LDS_PastCompany = companypast1 + ";" + companypast3;
                        LDS_Education = education1 + ";" + education2;
                        LDS_CurrentTitle = titleCurrenttitle;
                        LDS_LoginID = SearchCriteria.LoginID;                                                                                                                       //"ProfileType" + "," + "UserProfileLink" + "," + "FirstName" + "," + "LastName" + "," + "HeadLineTitle" + "," + "CurrentTitle " + "," + "Company" + "," + "Connection" + "," + "Recommendations " + "," + "SkillAndExpertise " + "," + "Experience " + "," + " Education" + "," + "Groups" + "," + "UserEmail" + "," + "UserContactNumbe" + "," + "PastTitles" + "," + "PastCompany" + "," + "Loction" + "," + "Country" + "," + "titlepast3" + "," + "companypast3" + "," + "titlepast4" + "," + "companypast4" + ",";
                        string LDS_FinalData = LDS_ProfileType.Replace(",", ";") + "," + LDS_UserProfileLink.Replace(",", ";") + "," + LDS_FirstName.Replace(",", ";") + "," + LDS_LastName.Replace(",", ";") + "," + LDS_HeadLineTitle.Replace(",", ";") + "," + LDS_CurrentTitle.Replace(",", ";") + "," + LDS_CurrentCompany.Replace(",", ";") + "," + LDS_Connection.Replace(",", ";") + "," + LDS_Recommendations.Replace(",", ";") + "," + LDS_SkillAndExpertise.Replace(",", ";") + "," + LDS_Experience.Replace(",", ";") + "," + LDS_Education.Replace(",", ";") + "," + LDS_Groups.Replace(",", ";") + "," + LDS_UserEmail.Replace(",", ";") + "," + LDS_UserContactNumber.Replace(",", ";") + "," + LDS_PastTitles.Replace(",", ";") + "," + LDS_PastCompany.Replace(",", ";") + "," + LDS_Loction.Replace(",", ";") + "," + LDS_Country.Replace(",", ";") + "," + LDS_Industry.Replace(",", ";") + "," + LDS_Websites.Replace(",", ";") + "," + accountUser.Replace(",", ";") + ",";

                        if (LDS_FinalData.Contains("<strong class=\"highlight\"") || LDS_FinalData.Contains("<span class=\"full-name\"") || LDS_FinalData.Contains("<strong class=\"highlight\"") || LDS_FinalData.Contains("overview-connections\">"))
                        {
                            LDS_FinalData = LDS_FinalData.Replace("<span class=\"full-name\"", "").Replace("\n", "").Replace("<strong class=\"highlight\"", "").Replace("overview-connections\">", "").Replace("</strong>", "").Replace("<strong>", "");
                        }
                        if (!string.IsNullOrEmpty(LDS_FirstName) || !string.IsNullOrEmpty(LDS_FirstName))
                        {
                            //Log(LDS_FinalData);
                        }
                        // if (SearchCriteria.starter)
                        {
                            if (!string.IsNullOrEmpty(LDS_FirstName) || !string.IsNullOrEmpty(LDS_FirstName))
                            {
                                try
                                {
                                    // if (status.Contains("LinkedinSearch_ProfileData"))
                                    {
                                        try
                                        {
                                            string CSVHeader = "ProfileType" + "," + "UserProfileLink" + "," + "FirstName" + "," + "LastName" + "," + "PostalCode" + "," + "Distance From Postal" + "," + "HeadLineTitle" + "," + "CurrentTitle " + "," + "Company" + "," + "Connection" + "," + "Recommendations " + "," + "SkillAndExpertise " + "," + "Experience " + "," + " Education" + "," + "Groups" + "," + "UserEmail" + "," + "UserContactNumber" + "," + "PastTitles" + "," + "PastCompany" + "," + "Location" + "," + "Country" + "," + "Industry" + "," + "WebSites" + "," + "LinkedInLoginID" + ",";
                                            string CSV_Content = LDS_ProfileType.Replace(",", ";") + "," + LDS_UserProfileLink.Replace(",", ";") + "," + LDS_FirstName.Replace(",", ";") + "," + LDS_LastName.Replace(",", ";") + "," + postalCode + "," + distance + "," + LDS_HeadLineTitle.Replace(",", ";") + "," + LDS_CurrentTitle.Replace(",", ";") + "," + LDS_CurrentCompany.Replace(",", ";") + "," + LDS_Connection.Replace(",", ";") + "," + LDS_Recommendations.Replace(",", ";") + "," + LDS_SkillAndExpertise.Replace(",", ";") + "," + LDS_Experience.Replace(",", ";") + "," + LDS_Education.Replace(",", ";") + "," + LDS_Groups.Replace(",", ";") + "," + LDS_UserEmail.Replace(",", ";") + "," + LDS_UserContactNumber.Replace(",", ";") + "," + LDS_PastTitles.Replace(",", ";") + "," + LDS_PastCompany.Replace(",", ";") + "," + LDS_Loction.Replace(",", ";") + "," + LDS_Country.Replace(",", ";") + "," + LDS_Industry.Replace(",", ";") + "," + LDS_Websites.Replace(",", ";") + "," + accountUser.Replace(",", ";") + ",";
                                            CSVUtilities.ExportDataCSVFile(CSVHeader, CSV_Content, Globals.path_InBoardProGetDataResultUrlData);
                                            Log("[ " + DateTime.Now + " ] => [ Record Saved In CSV File ! ]");
                                        }
                                        catch
                                        {
                                        }

                                    }
                                    // else
                                    //{
                                    //    try
                                    //    {
                                    //       // AppFileHelper.AddingLinkedInDataToCSVFile(LDS_FinalData, Globals.path_LinkedinSearchSearchByPeople);
                                    //        string CSVHeader = "ProfileType" + "," + "UserProfileLink" + "," + "FirstName" + "," + "LastName" + "," + "PostalCode" + "," + "Distance From Postal" + "," + "HeadLineTitle" + "," + "CurrentTitle " + "," + "Company" + "," + "Connection" + "," + "Recommendations " + "," + "SkillAndExpertise " + "," + "Experience " + "," + " Education" + "," + "Groups" + "," + "UserEmail" + "," + "UserContactNumber" + "," + "PastTitles" + "," + "PastCompany" + "," + "Location" + "," + "Country" + "," + "Industry" + "," + "WebSites" + "," + "LinkedinLogInID" + ",";
                                    //        string CSV_Content = LDS_ProfileType.Replace(",", ";") + "," + LDS_UserProfileLink.Replace(",", ";") + "," + LDS_FirstName.Replace(",", ";") + "," + LDS_LastName.Replace(",", ";") + "," + postalCode + "," + distance + "," + LDS_HeadLineTitle.Replace(",", ";") + "," + LDS_CurrentTitle.Replace(",", ";") + "," + LDS_CurrentCompany.Replace(",", ";") + "," + LDS_Connection.Replace(",", ";") + "," + LDS_Recommendations.Replace(",", ";") + "," + LDS_SkillAndExpertise.Replace(",", ";") + "," + LDS_Experience.Replace(",", ";") + "," + LDS_Education.Replace(",", ";") + "," + LDS_Groups.Replace(",", ";") + "," + LDS_UserEmail.Replace(",", ";") + "," + LDS_UserContactNumber.Replace(",", ";") + "," + LDS_PastTitles.Replace(",", ";") + "," + LDS_PastCompany.Replace(",", ";") + "," + LDS_Loction.Replace(",", ";") + "," + LDS_Country.Replace(",", ";") + "," + LDS_Industry.Replace(",", ";") + "," + LDS_Websites.Replace(",", ";") + "," + accountUser.Replace(",", ";") + ",";
                                    //        CSVUtilities.ExportDataCSVFile(CSVHeader, CSV_Content, Globals.path_LinkedinSearchSearchByPeople);
                                    //        Log("Record Saved In CSV File !");

                                    //        //string CSVHeader = "ProfileType" + "," + "UserProfileLink" + "," + "FirstName" + "," + "LastName" + "," + "HeadLineTitle" + "," + "CurrentTitle " + "," + "Company" + "," + "Connection" + "," + "Recommendations " + "," + "SkillAndExpertise " + "," + "Experience " + "," + " Education" + "," + "Groups" + "," + "UserEmail" + "," + "UserContactNumber" + "," + "PastTitles" + "," + "PastCompany" + "," + "Location" + "," + "Country" + "," + "Industry" + "," + "WebSites" + "," + "LinkedinLogInID" + ",";

                                    //        //string CSV_Content = TypeOfProfile + "," + LDS_UserProfileLink + "," + firstname + "," + lastname + "," + Company.Replace(",", ";") + "," + titlecurrent.Replace(",", ";") + "," + companycurrent.Replace(",", ";") + "," + Connection.Replace(",", ";") + "," + recomandation.Replace(",", string.Empty) + "," + Skill.Replace(",", ";") + "," + LDS_Experience.Replace(",", string.Empty) + "," + EducationCollection.Replace(",", ";") + "," + groupscollectin.Replace(",", ";") + "," + USERemail.Replace(",", ";") + "," + LDS_UserContact.Replace(",", ";") + "," + LDS_PastTitles + "," + AllComapny.Replace(",", ";") + "," + country.Replace(",", ";") + "," + location.Replace(",", ";") + "," + Industry.Replace(",", ";") + "," + Website.Replace(",", ";") + "," + LDS_LoginID + ",";// +TypeOfProfile + ",";



                                    //        //string CSVHeader = "ProfileType" + "," + "UserProfileLink" + "," + "FirstName" + "," + "LastName" + "," + "PostalCode" + "," + "Distance From Postal" + "," + "HeadLineTitle" + "," + "CurrentTitle " + "," + "Company" + "," + "Connection" + "," + "Recommendations " + "," + "SkillAndExpertise " + "," + "Experience " + "," + " Education" + "," + "Groups" + "," + "UserEmail" + "," + "UserContactNumber" + "," + "PastTitles" + "," + "PastCompany" + "," + "Location" + "," + "Country" + "," + "Industry" + "," + "WebSites" + "," + "LinkedinLogInID" + ",";
                                    //        //string CSV_Content = TypeOfProfile + "," + LDS_UserProfileLink + "," + firstname + "," + lastname + "," + postalCode + "," + distance + "," + Company.Replace(",", ";") + "," + titlecurrent.Replace(",", ";") + "," + companycurrent.Replace(",", ";") + "," + Connection.Replace(",", ";") + "," + recomandation.Replace(",", string.Empty) + "," + Skill.Replace(",", ";") + "," + LDS_Experience.Replace(",", string.Empty) + "," + EducationCollection.Replace(",", ";") + "," + groupscollectin.Replace(",", ";") + "," + USERemail.Replace(",", ";") + "," + LDS_UserContact.Replace(",", ";") + "," + LDS_PastTitles + "," + AllComapny.Replace(",", ";") + "," + country.Replace(",", ";") + "," + location.Replace(",", ";") + "," + Industry.Replace(",", ";") + "," + Website.Replace(",", ";") + "," + accountUser + ",";// +TypeOfProfile + ",";
                                    //        //CSVUtilities.ExportDataCSVFile(CSVHeader, CSV_Content, Globals.path_InBoardProGetDataResultUrlData);//path_LinkedinSearchSearchByProfileURL);
                                    //        //Log("Data Saved In CSV File !");
                                    //    }
                                    //    catch
                                    //    {
                                    //    }
                                    //}
                                }
                                catch
                                {
                                }


                            }
                        }

                    }
                    catch (Exception ex) { };

                }

            }
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
        
        
        
    