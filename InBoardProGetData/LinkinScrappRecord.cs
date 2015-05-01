using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InBoardPro;
using System.Text.RegularExpressions;
using BaseLib;

namespace InBoardPro
{
    public class LinkinScrappRecord
    {
        #region Global declaration
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
        public Events InBoardProGetDataResultLogEvents = new Events(); 
        #endregion

        #region LinkinScrappRecord
        public LinkinScrappRecord()
        {

        } 
        #endregion

        #region LinkinScrappRecord
        public LinkinScrappRecord(string UserName, string Password, string ProxyAddress, string ProxyPort, string ProxyUserName, string ProxyPassword, string PostalCode, string Distance, string Industry, string LastName)
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
        #endregion

        #region GetRecords
        public void GetRecords(ref GlobusHttpHelper HttpHelper)
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
                string rsid = string.Empty;

                string pageSource = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/home?trk=hb_tab_home_top"));

                try
                {
                    try
                    {
                        string[] Arr_Pst = Regex.Split(postalCode, "(");
                    }
                    catch { }
                    try
                    {
                        Postalcode = postalCode.Substring(0, postalCode.IndexOf(" "));
                        Country = postalCode.Replace(Postalcode, string.Empty).Replace(")", string.Empty).Replace("(", string.Empty).Trim();
                        //Postalcode = Arr_Pst[0].Replace(" ", string.Empty).Trim();
                        //Country = Arr_Pst[1].Replace("{", string.Empty).Replace("}", string.Empty).Trim();
                    }
                    catch
                    {
                        if (Postalcode == string.Empty)
                        {
                            Postalcode = postalCode;
                        }
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

                string FirstGetRequestUrl = string.Empty;
                string FirstGetResponse = string.Empty;
                {
                    try
                    {
                        FirstGetRequestUrl = "http://www.linkedin.com/search/fpsearch?lname=" + lastName + "&searchLocationType=I&countryCode=" + countrycode + "&postalCode=" + Postalcode + "&distance=" + distance + "&keepFacets=keepFacets&page_num=1&facet_I=" + industrycode + "&pplSearchOrigin=ADVS&viewCriteria=2&sortCriteria=R&redir=redir";
                        FirstGetResponse = HttpHelper.getHtmlfromUrl1(new Uri(FirstGetRequestUrl));
                    }
                    catch { }
                }

                int RecordCount = 0;
                try
                {
                    RecordCount = objIndustry.GetPageNo(FirstGetResponse);

                    if (RecordCount == 0)
                    {
                        string getAdvPagedata = HttpHelper.getHtmlfromUrl(new Uri("http://www.linkedin.com/vsearch/f?adv=true&trk=advsrch"), "http://www.linkedin.com/");

                        try
                        {
                            int startindex = getAdvPagedata.IndexOf("rsid=");
                            string start = getAdvPagedata.Substring(startindex).Replace("rsid=", "");
                            int endindex = start.IndexOf("&amp;");
                            string end = start.Substring(0, endindex);
                            rsid = end;
                        }
                        catch (Exception ex)
                        {

                        }

                        //FirstGetRequestUrl = "http://www.linkedin.com/vsearch/p?lastName=" + lastName + "&postalCode=" + postalCode + "&openAdvancedForm=true&locationType=I&countryCode=" + countrycode + "&distance=" + distance + "&facet_I=" + industrycode + "&sortBy=R&rsid=" + rsid + "&orig=MDYS";
                        // http://www.linkedin.com/vsearch/p?lastName=James&postalCode=44101&openAdvancedForm=true&locationType=I&countryCode=us&distance=50&f_N=F,S,A&rsid=2247217581372762829704&orig=MDYS";
                        //http://www.linkedin.com/vsearch/p?lastName=" + lastName + "&postalCode=" + postalCode + "&openAdvancedForm=true&locationType=I&countryCode=" + countrycode + "&distance=" + distance + "&facet_I=" + industrycode + "&sortBy=R&rsid=" + rsid + "&orig=MDYS";

                        try
                        {

                            FirstGetRequestUrl = "http://www.linkedin.com/vsearch/p?lastName=" + lastName + "&postalCode=" + postalCode + "&openAdvancedForm=true&locationType=I&countryCode=" + countrycode + "&distance=" + distance + "&f_I=" + industrycode.Replace(" ", "") + "&rsid=" + rsid + "&orig=ADVS";
                            FirstGetResponse = HttpHelper.getHtmlfromUrl1(new Uri(FirstGetRequestUrl));

                        }
                        catch { }
                    }


                    RecordCount = objIndustry.GetPageNo(FirstGetResponse);

                    Loger("[ " + DateTime.Now + " ] => [ Get RecordCount : " + RecordCount + " Using UserName : " + accountUser + " Postal Code : " + postalCode.ToString() + " Distance : " + distance + " Industry : " + industrycode + " ]");
                }
                catch { }
                try
                {
                    LinkedinScrappDbManager objLsManager = new LinkedinScrappDbManager();
                    objLsManager.InsertScarppRecordData(Postalcode, distance, industryType, lastName, RecordCount);

                }
                catch { }
                try
                {
                    string prxyadress = string.Empty;
                    try
                    {
                        if (!string.IsNullOrEmpty(proxyAddress) && !string.IsNullOrEmpty(proxyPort))
                        {
                            prxyadress = proxyAddress + ":" + proxyPort;
                        }

                    }
                    catch { }

                    string CSVHeader = "PostalCode" + "," + "Distance" + "," + "IndustryType" + "," + "LastName" + "," + "UserName" + "," + "Password" + "," + "Proxy" + "," + "ProxyPwd " + "," + "Number Of Result";// + "," + "Connection" + "," + "Recommendations " + "," + "SkillAndExpertise " + "," + "Experience " + "," + " Education" + "," + "Groups" + "," + "UserEmail" + "," + "UserContactNumber" + "," + "PastTitles" + "," + "PastCompany" + "," + "Location" + "," + "Country" + "," + "Industry" + "," + "WebSites" + "," + "LinkedinLogInID" + ",";
                    string CSV_Content = postalCode.Replace(",", ";") + "," + distance.Replace(",", ";") + "," + industryType.Replace(",", ";") + "," + lastName.Replace(",", ";") + "," + accountUser.Replace(",", ";") + "," + accountPass.Replace(",", ";") + "," + prxyadress.Replace(",", ";") + "," + proxyPassword.Replace(",", ";") + "," + RecordCount.ToString().Replace(",", ";");//+ "," + Connection.Replace(",", ";") + "," + recomandation.Replace(",", string.Empty) + "," + Skill.Replace(",", ";") + "," + LDS_Experience.Replace(",", string.Empty) + "," + EducationCollection.Replace(",", ";") + "," + groupscollectin.Replace(",", ";") + "," + USERemail.Replace(",", ";") + "," + LDS_UserContact.Replace(",", ";") + "," + LDS_PastTitles + "," + AllComapny.Replace(",", ";") + "," + country.Replace(",", ";") + "," + location.Replace(",", ";") + "," + Industry.Replace(",", ";") + "," + Website.Replace(",", ";") + "," + accountUser + ",";// +TypeOfProfile + ",";
                    CSVUtilities.ExportDataCSVFile(CSVHeader, CSV_Content, Globals.path_InBoardProGetDataResultCount);
                }
                catch { }


            }
            catch { }
        } 
        #endregion

        #region GetRecords1
        public void GetRecords1(ref GlobusHttpHelper HttpHelper)
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
                string rsid = string.Empty;

                string pageSource = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/home?trk=hb_tab_home_top"));

                try
                {
                    try
                    {
                        string[] Arr_Pst = Regex.Split(postalCode, "(");
                    }
                    catch { }
                    try
                    {
                        Postalcode = postalCode.Substring(0, postalCode.IndexOf(" "));
                        Country = postalCode.Replace(Postalcode, string.Empty).Replace(")", string.Empty).Replace("(", string.Empty).Trim();
                        //Postalcode = Arr_Pst[0].Replace(" ", string.Empty).Trim();
                        //Country = Arr_Pst[1].Replace("{", string.Empty).Replace("}", string.Empty).Trim();
                    }
                    catch
                    {
                        if (Postalcode == string.Empty)
                        {
                            Postalcode = postalCode;
                        }
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

                string FirstGetRequestUrl = string.Empty;
                string FirstGetResponse = string.Empty;
                {
                    try
                    {
                        FirstGetRequestUrl = "http://www.linkedin.com/search/fpsearch?lname=" + lastName + "&searchLocationType=I&countryCode=" + countrycode + "&postalCode=" + Postalcode + "&distance=" + distance + "&keepFacets=keepFacets&page_num=1&facet_I=" + industrycode + "&pplSearchOrigin=ADVS&viewCriteria=2&sortCriteria=R&redir=redir";
                        FirstGetResponse = HttpHelper.getHtmlfromUrl1(new Uri(FirstGetRequestUrl));
                    }
                    catch { }
                }

                int RecordCount = 0;
                try
                {
                    RecordCount = objIndustry.GetPageNo(FirstGetResponse);

                    if (RecordCount == 0)
                    {
                        string getAdvPagedata = HttpHelper.getHtmlfromUrl(new Uri("http://www.linkedin.com/vsearch/f?adv=true&trk=advsrch"), "http://www.linkedin.com/");

                        try
                        {
                            int startindex = getAdvPagedata.IndexOf("rsid=");
                            string start = getAdvPagedata.Substring(startindex).Replace("rsid=", "");
                            int endindex = start.IndexOf("&amp;");
                            string end = start.Substring(0, endindex);
                            rsid = end;
                        }
                        catch (Exception ex)
                        {

                        }

                        try
                        {

                            FirstGetRequestUrl = "http://www.linkedin.com/vsearch/p?lastName=" + lastName + "&postalCode=" + postalCode + "&openAdvancedForm=true&locationType=I&countryCode=" + countrycode + "&distance=" + distance + "&f_I=" + industrycode.Replace(" ", "") + "&rsid=" + rsid + "&orig=ADVS";
                            FirstGetResponse = HttpHelper.getHtmlfromUrl1(new Uri(FirstGetRequestUrl));

                        }
                        catch { }
                    }


                    RecordCount = objIndustry.GetPageNo(FirstGetResponse);

                    Loger("[ " + DateTime.Now + " ] => [ Get RecordCount : " + RecordCount + " Using UserName : " + accountUser + " Postal Code : " + postalCode.ToString() + " Distance : " + distance + " Industry : " + industrycode + " ]");
                }
                catch { }
                try
                {
                    LinkedinScrappDbManager objLsManager = new LinkedinScrappDbManager();
                    objLsManager.InsertScarppRecordData(Postalcode, distance, industryType, lastName, RecordCount);

                }
                catch { }
                try
                {
                    string prxyadress = string.Empty;
                    try
                    {
                        if (!string.IsNullOrEmpty(proxyAddress) && !string.IsNullOrEmpty(proxyPort))
                        {
                            prxyadress = proxyAddress + ":" + proxyPort;
                        }

                    }
                    catch { }

                    string CSVHeader = "PostalCode" + "," + "Distance" + "," + "IndustryType" + "," + "LastName" + "," + "UserName" + "," + "Password" + "," + "Proxy" + "," + "ProxyPwd " + "," + "Number Of Result";// + "," + "Connection" + "," + "Recommendations " + "," + "SkillAndExpertise " + "," + "Experience " + "," + " Education" + "," + "Groups" + "," + "UserEmail" + "," + "UserContactNumber" + "," + "PastTitles" + "," + "PastCompany" + "," + "Location" + "," + "Country" + "," + "Industry" + "," + "WebSites" + "," + "LinkedinLogInID" + ",";
                    string CSV_Content = postalCode.Replace(",", ";") + "," + distance.Replace(",", ";") + "," + industryType.Replace(",", ";") + "," + lastName.Replace(",", ";") + "," + accountUser.Replace(",", ";") + "," + accountPass.Replace(",", ";") + "," + prxyadress.Replace(",", ";") + "," + proxyPassword.Replace(",", ";") + "," + RecordCount.ToString().Replace(",", ";");//+ "," + Connection.Replace(",", ";") + "," + recomandation.Replace(",", string.Empty) + "," + Skill.Replace(",", ";") + "," + LDS_Experience.Replace(",", string.Empty) + "," + EducationCollection.Replace(",", ";") + "," + groupscollectin.Replace(",", ";") + "," + USERemail.Replace(",", ";") + "," + LDS_UserContact.Replace(",", ";") + "," + LDS_PastTitles + "," + AllComapny.Replace(",", ";") + "," + country.Replace(",", ";") + "," + location.Replace(",", ";") + "," + Industry.Replace(",", ";") + "," + Website.Replace(",", ";") + "," + accountUser + ",";// +TypeOfProfile + ",";
                    CSVUtilities.ExportDataCSVFile(CSVHeader, CSV_Content, Globals.path_InBoardProGetDataResultCountZipDistanceIndustrySirnameWise);
                }
                catch { }


            }
            catch { }
        }
        #endregion

        #region Loger
        private void Loger(string message)
        {
            EventsArgs eventArgs = new EventsArgs(message);
            InBoardProGetDataResultLogEvents.LogText(eventArgs);
        } 
        #endregion
    }
}
