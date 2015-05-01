using System.Linq;
using System.Text;
using InBoardPro;
using System.Text.RegularExpressions;
using System.Threading;
using Chilkat;
using System.Collections.Generic;
using System;
using System.Data;
using Groups;
using BaseLib;


namespace InBoardProGetData
{
    public class LinkedInScrape
    {
        #region Global Declaration
        List<string> AllURL4page = new List<string>();
        GlobusHttpHelper _HttpHelper = new GlobusHttpHelper();
        ChilkatHttpHelpr objChilkat = new ChilkatHttpHelpr();
        List<string> RecordURL = new List<string>();
        Queue<string> queRecordUrl = new Queue<string>();
        string ResponceDeltaPost1 = string.Empty;
        string ResponseWallPostForPremiumAcc = string.Empty;
        string NewSearchPage = string.Empty;
        string PostResponce = string.Empty;
        string PostRequestURL = string.Empty;
        string PostdataForPagination = string.Empty;
        string csrfToken = string.Empty;
        string csrfToken1 = string.Empty;
        public static Events logger = new Events();
        private readonly object lockr = new object(); 
        #endregion

        #region StartCompanyEmployeeScraperWithPagination
        public void StartCompanyEmployeeScraperWithPagination(ref GlobusHttpHelper HttpHelper, string Url, int pagenumber)
        {
            string CheckString = "CampaignScraper";
            try
            {
                string[] Url_split = Regex.Split(Url, "##");
                if (Url_split.Count() > 1)
                {
                    Url = Url_split[0];
                    CheckString = Url_split[1];
                }
            }
            catch
            { }
            #region Login
            try
            {
               
                if (SearchCriteria.starter)
                {
                    #region Serch

                    string pageSourceaAdvanceSearch = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/search"));
                    NewSearchPage = string.Empty;

                   #endregion
                }
                ResponseWallPostForPremiumAcc = HttpHelper.getHtmlfromUrl1(new Uri(Url));

                #region commented code
                //int pagenumber = 0;
                //string strPageNumber = string.Empty;
                //string[] Arr12 = Regex.Split(ResponseWallPostForPremiumAcc, "<li");
                //foreach (string item in Arr12)
                //{
                //    if (SearchCriteria.starter)
                //    {
                //        #region Loop
                //        if (!item.Contains("<!DOCTYPE"))
                //        {
                //            if (item.Contains("results-summary"))
                //            {
                //                string data = RemoveAllHtmlTag.StripHTML(item);
                //                data = data.Replace("\n", "");
                //                if (data.Contains(">"))
                //                {
                //                    string[] ArrTemp = data.Split('>');
                //                    data = ArrTemp[1];
                //                    data = data.Replace("results", "");
                //                    data = data.Trim();
                //                    string[] ArrTemp1 = data.Split(' ');
                //                    data = ArrTemp1[0].Replace(',', ' ').Trim();
                //                    strPageNumber = data.Replace(" ", string.Empty);
                //                    break;
                //                }

                //            }
                //        }
                //        #endregion
                //    }
                //}

                //if (string.IsNullOrEmpty(strPageNumber))
                //{
                //    try
                //    {
                //        if (ResponseWallPostForPremiumAcc.Contains("resultCount"))
                //        {
                //            string[] countResultArr = Regex.Split(ResponseWallPostForPremiumAcc, "resultCount");

                //            if (countResultArr.Length > 1)
                //            {
                //                string tempResult = countResultArr[1].Substring(0, countResultArr[1].IndexOf(","));

                //                #region Commented
                //                //Regex IdCheck = new Regex("^[0-9]*$");

                //                //string[] tempResultArr = Regex.Split(tempResult, "[^0-9]");

                //                //foreach (string item in tempResultArr)
                //                //{
                //                //    try
                //                //    {
                //                //        if(IdCheck.IsMatch(item))
                //                //        {
                //                //            strPageNumber = item;

                //                //            break;
                //                //        }
                //                //    }
                //                //    catch (Exception ex)
                //                //    {
                //                //    }
                //                //} 
                //                #endregion

                //                if (tempResult.Contains("<strong>"))
                //                {
                //                    strPageNumber = tempResult.Substring(tempResult.IndexOf("<strong>"), tempResult.IndexOf("</strong>", tempResult.IndexOf("<strong>")) - tempResult.IndexOf("<strong>")).Replace("<strong>", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Trim();
                //                }
                //                else if (tempResult.Contains(":"))
                //                {
                //                    strPageNumber = tempResult.Replace(":", string.Empty).Replace("\"", string.Empty);
                //                }
                //            }
                //        }
                //    }
                //    catch (Exception)
                //    {

                //    }
                //}

                //if (string.IsNullOrEmpty(strPageNumber))
                //{
                //    try
                //    {
                //        if (ResponseWallPostForPremiumAcc.Contains("results_count_without_keywords_i18n"))
                //        {
                //            string[] countResultArr = Regex.Split(ResponseWallPostForPremiumAcc, "results_count_without_keywords_i18n");

                //            if (countResultArr.Length > 1)
                //            {
                //                string tempResult = countResultArr[1].Substring(0, countResultArr[1].IndexOf(","));

                //                if (tempResult.Contains("<strong>"))
                //                {
                //                    strPageNumber = tempResult.Substring(tempResult.IndexOf("<strong>"), tempResult.IndexOf("</strong>", tempResult.IndexOf("<strong>")) - tempResult.IndexOf("<strong>")).Replace("<strong>", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Trim();
                //                }
                //                else if (tempResult.Contains(":"))
                //                {
                //                    strPageNumber = tempResult.Replace(":", string.Empty).Replace("\"", string.Empty);
                //                }
                //            }
                //        }
                //    }
                //    catch (Exception)
                //    {

                //    }
                //}

                //string logtag = string.Empty;

                //try
                //{
                //    pagenumber = int.Parse(strPageNumber);

                //    Log("[ " + DateTime.Now + " ] => [ Total Results :  " + pagenumber + " ]");
                //}
                //catch (Exception)
                //{

                //}

                //pagenumber = (pagenumber / 10) + 1;

                //if (pagenumber == -1)
                //{
                //    pagenumber = 2;
                //}

                //if (pagenumber == 1)
                //{
                //    pagenumber = 2;
                //} 
                #endregion

                string GetResponce = string.Empty;
                string GetRequestURL = string.Empty;

                if (pagenumber >= 1)
                {
                    _HttpHelper = HttpHelper;

                    new Thread(() =>
                    {
                        if (SearchCriteria.starter)
                        {
                            finalUrlCollection(CheckString);

                        }

                    }).Start();

                    int count = 0;
                    Url = Url + "&page_num=";

                    for (int i = 1; i <= pagenumber; i++)
                    {
                        if (SearchCriteria.starter)
                        {
                            #region loop

                   
                            ResponseWallPostForPremiumAcc = HttpHelper.getHtmlfromUrl(new Uri(Url+i));
                            if (ResponseWallPostForPremiumAcc.Contains("/profile/view?id"))
                            {

                                List<string> PageSerchUrl = GettingAllUrl(ResponseWallPostForPremiumAcc);
                                PageSerchUrl.Distinct();

                                if (PageSerchUrl.Count == 0)
                                {
                                    Log("[ " + DateTime.Now + " ] => [ On the basis of your Account you can able to see " + RecordURL.Count + " Results ]");
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
                                                    //if (!queRecordUrl.Contains(urlSerch))
                                                    //{
                                                        queRecordUrl.Enqueue(urlSerch);
                                                    //}
                                                    RecordURL = RecordURL.Distinct().ToList();
                                                }

                                                try
                                                {
                                                    Log("[ " + DateTime.Now + " ] => [ " + urlSerch + " ]");
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

                                Log("[ " + DateTime.Now + " ] => [ On the basis of your Account or Your Input you can able to see " + RecordURL.Count + "  Results ]");

                            }

                            foreach (string item in PageSerchUrl)
                            {
                                if (SearchCriteria.starter)
                                {
                                    if (item.Contains("pp_profile_name_link"))
                                    {
                                        string urlSerch = "http://www.linkedin.com" + item;
                                        Log("[ " + DateTime.Now + " ] => [ " + urlSerch + " ]");
                                        RecordURL.Add(urlSerch);
                                        queRecordUrl.Enqueue(urlSerch);

                                    }
                                }
                            }
                        }
                        #endregion
                    }

                }

                //if (pagenumber != string.Empty)
                //{
                //    if (strPageNumber != "0")
                //    {
                //        Log("-------------------------------------------------------------------------------------------------------------------------------");
                //        Log("[ " + DateTime.Now + " ] => [ No Of Results Found >>> " + strPageNumber + " ]");
                //    }
                //}

                RecordURL.Distinct();
                //if (SearchCriteria.starter)
                //{
                //    finalUrlCollection(ref HttpHelper);
                //    //new Thread(() =>
                //    //{
                //    //    test();
                //    //}).Start();
                //}

            }

                #endregion

            catch { }
            #endregion
        }
        #endregion

        #region StartInBoardProGetDataWithPagination
        public void StartInBoardProGetDataWithPagination(ref GlobusHttpHelper HttpHelper)
        {
            #region Login
            try
            {
                //Temprary class
                //======================================================
                //string tempurl = "http://www.linkedin.com/profile/view?id=224916256&authType=OUT_OF_NETWORK&authToken=SWNz&locale=en_US&srchid=3387141351401255871148&srchindex=1&srchtotal=2017&trk=vsrp_people_res_name&trkInfo=VSRPsearchId%3A3387141351401255871148%2CVSRPtargetId%3A224916256%2CVSRPcmpt%3Aprimary";
                //CrawlingLinkedInPage(tempurl, ref HttpHelper);
                //======================================================
                if (SearchCriteria.AccountType == "RecuiterType")
                {
                    string pageSourceaAdvanceSearch = HttpHelper.getHtmlfromUrl1(new Uri("https://www.linkedin.com/recruiter/search"));
                    string referralUrl = string.Empty;
                    if (pageSourceaAdvanceSearch.Contains("csrfToken"))
                    {
                        try
                        {
                            int pagenumberrecruiter = 0;
                            bool IsShowLoggerPagecount = true;
                            int i = 1;
                            int startCount = 0;
                       

                            #region seniorLevel
                            string tempSeniorlevel = string.Empty;
                            if (SearchCriteria.SeniorLevel.Contains(","))
                            {
                                string[] arrseniorLevel = Regex.Split(SearchCriteria.SeniorLevel, ",");
                                if (arrseniorLevel.Count() > 1)
                                {
                                    foreach (string item in arrseniorLevel)
                                    {
                                        tempSeniorlevel += "&facet.SE=" + item;
                                    }
                                }
                                else
                                {
                                    tempSeniorlevel = "&facet.SE=";
                                }
                            }
                            else if (string.IsNullOrEmpty(SearchCriteria.SeniorLevel))
                            {
                                tempSeniorlevel = "&facet.SE=";
                            }
                            else
                            {
                                tempSeniorlevel = "&facet.SE=" + SearchCriteria.SeniorLevel;
                            } 
                            #endregion

                            #region Function
                            string tempFunction = string.Empty;
                            if (SearchCriteria.Function.Contains(","))
                            {
                                string[] arrFunction = Regex.Split(SearchCriteria.Function, ",");
                                if (arrFunction.Count() > 1)
                                {
                                    foreach (string item in arrFunction)
                                    {
                                        tempFunction += "&facet.FA=" + item;
                                    }
                                }
                                else
                                {
                                    tempFunction = "&facet.FA=";
                                }
                            }
                            else if (string.IsNullOrEmpty(SearchCriteria.Function))
                            {
                                tempFunction = "&facet.FA=";
                            }
                            else
                            {
                                tempFunction = "&facet.FA=" + SearchCriteria.Function;
                            }
                            #endregion

                            #region RelationShip
                            string tempRelationShip = string.Empty;
                            if (SearchCriteria.Relationship.Contains(","))
                            {
                                string[] arrRelationShip = Regex.Split(SearchCriteria.Relationship, ",");
                                if (arrRelationShip.Count() > 1)
                                {
                                    foreach (string item in arrRelationShip)
                                    {
                                        tempRelationShip += "&facet.N=" + item;
                                    }
                                }
                                else
                                {
                                    tempRelationShip = "&facet.N=";
                                }
                            }
                            else if (string.IsNullOrEmpty(SearchCriteria.Relationship))
                            {
                                tempRelationShip = "&facet.N=";
                            }
                            else
                            {
                                tempRelationShip = "&facet.N=" + SearchCriteria.Relationship;
                            }
                            #endregion

                            #region Language
                            string tempLanguage = string.Empty;
                            if (SearchCriteria.language.Contains(","))
                            {
                                string[] arrLanguage = Regex.Split(SearchCriteria.language, ",");
                                if (arrLanguage.Count() > 1)
                                {
                                    foreach (string item in arrLanguage)
                                    {
                                        tempLanguage += "&facet.L=" + item;
                                    }
                                }
                                else
                                {
                                    tempLanguage = "&facet.L=";
                                }
                            }
                            else if (string.IsNullOrEmpty(SearchCriteria.language))
                            {
                                tempLanguage = "&facet.L=";
                            }
                            else
                            {
                                tempLanguage = "&facet.L=" + SearchCriteria.language;
                            }
                            #endregion

                            #region Industry
                            string tempIndustry = string.Empty;
                            if (SearchCriteria.IndustryType.Contains(","))
                            {
                                string[] arrIndustry = Regex.Split(SearchCriteria.IndustryType, ",");
                                if (arrIndustry.Count() > 1)
                                {
                                    foreach (string item in arrIndustry)
                                    {
                                        tempIndustry += "&facet.L=" + item;
                                    }
                                }
                                else
                                {
                                    tempIndustry = "&facet.L=";
                                }
                            }
                            else if (string.IsNullOrEmpty(SearchCriteria.IndustryType))
                            {
                                tempIndustry = "&facet.L=";
                            }
                            else
                            {
                                tempIndustry = "&facet.L=" + SearchCriteria.IndustryType;
                            }
                            #endregion

                            #region Year of Experience
                            string tempExperience = string.Empty;
                            if (SearchCriteria.YearOfExperience.Contains(","))
                            {
                                string[] arrYearOfExperience = Regex.Split(SearchCriteria.YearOfExperience, ",");
                                if (arrYearOfExperience.Count() > 1)
                                {
                                    foreach (string item in arrYearOfExperience)
                                    {
                                        tempExperience += "&facet.TE=" + item;
                                    }
                                }
                                else
                                {
                                    tempExperience = "&facet.TE=";
                                }
                            }
                            else if (string.IsNullOrEmpty(SearchCriteria.YearOfExperience))
                            {
                                tempExperience = "&facet.TE=";
                            }
                            else
                            {
                                tempExperience = "&facet.TE=" + SearchCriteria.YearOfExperience;
                            }
                            #endregion

                            #region InterestedIN
                            string tempInteresedIn = string.Empty;
                            if (SearchCriteria.InerestedIn.Contains(","))
                            {
                                string[] arrInterestedIn = Regex.Split(SearchCriteria.InerestedIn, ",");
                                if (arrInterestedIn.Count() > 1)
                                {
                                    foreach (string item in arrInterestedIn)
                                    {
                                        tempInteresedIn += "&facet.P=" + item;
                                    }
                                }
                                else
                                {
                                    tempInteresedIn = "&facet.P=";
                                }
                            }
                            else if (string.IsNullOrEmpty(SearchCriteria.InerestedIn))
                            {
                                tempInteresedIn = "&facet.P=";
                            }
                            else
                            {
                                tempInteresedIn = "&facet.P=" + SearchCriteria.InerestedIn;
                            }
                            #endregion

                        StartAgain:

                            string recruiterUrl = string.Empty; //"https://www.linkedin.com/recruiter/api/search?keywords=" + Uri.EscapeDataString(SearchCriteria.Keyword) + "&page=" + i + "&start=" + startCount + "&count=25&countryCode=" + SearchCriteria.Country + "&postalCode=" + SearchCriteria.PostalCode + "&radiusMiles=" + SearchCriteria.within + "&jobTitle=" + SearchCriteria.Title + "&jobTitleTimeScope=" + SearchCriteria.TitleValue + "&company=" + SearchCriteria.Company + "&companyTimeScope=" + SearchCriteria.CompanyValue + "&firstName=" + SearchCriteria.FirstName + "&lastName=" + SearchCriteria.LastName + "&facet.TE=" + SearchCriteria.YearOfExperience + "&facet.CS=" + SearchCriteria.CompanySize + "&facet.L=" + SearchCriteria.language + "&facet.I=" + SearchCriteria.IndustryType + "&facet.FG=" + SearchCriteria.Group + "&facet.N=" + SearchCriteria.Relationship + "&facet.FA=" + SearchCriteria.Function + "&facet.SE=" + SearchCriteria.SeniorLevel + "&facet.P=" + SearchCriteria.InerestedIn + "&facet.F=" + SearchCriteria.Fortune1000 + "&facet.DR=" + SearchCriteria.RecentlyJoined + "&origin=ASAS"; //&facet.I=" + SearchCriteria.IndustryType + "&facet.FG=" + SearchCriteria.Group + "&facet.N=" + SearchCriteria.Relationship + "&facet.FA=" + SearchCriteria.Function + "&facet.SE=" + SearchCriteria.SeniorLevel + "&facet.P=" + SearchCriteria.InerestedIn + "&facet.F=" + SearchCriteria.Fortune1000 + "&facet.DR=" + SearchCriteria.RecentlyJoined + "
                            recruiterUrl = "https://www.linkedin.com/recruiter/api/search?keywords=" + Uri.EscapeDataString(SearchCriteria.Keyword) + "&page=" + i + "&start=" + startCount + "&count=25&countryCode=" + SearchCriteria.Country + "&postalCode=" + SearchCriteria.PostalCode + "&radiusMiles=" + SearchCriteria.within + "&jobTitle=" + SearchCriteria.Title + "&jobTitleTimeScope=" + SearchCriteria.TitleValue + "&company=" + SearchCriteria.Company + "&companyTimeScope=" + SearchCriteria.CompanyValue + "&firstName=" + SearchCriteria.FirstName + "&lastName=" + SearchCriteria.LastName + "" + tempExperience + "&facet.CS=" + SearchCriteria.CompanySize + "" + tempLanguage+ "" + tempIndustry + "&facet.FG=" + SearchCriteria.Group + "" + tempRelationShip + "" + tempFunction + "" + tempSeniorlevel + "" + tempInteresedIn + "&facet.F=" + SearchCriteria.Fortune1000 + "&facet.DR=" + SearchCriteria.RecentlyJoined + "&origin=ASAS"; //&facet.I=" + SearchCriteria.IndustryType + "&facet.FG=" + SearchCriteria.Group + "&facet.N=" + SearchCriteria.Relationship + "&facet.FA=" + SearchCriteria.Function + "&facet.SE=" + SearchCriteria.SeniorLevel + "&facet.P=" + SearchCriteria.InerestedIn + "&facet.F=" + SearchCriteria.Fortune1000 + "&facet.DR=" + SearchCriteria.RecentlyJoined + "
                            if (string.IsNullOrEmpty(SearchCriteria.Keyword))
                            {
                                recruiterUrl = recruiterUrl.Replace("keywords=", "");
                            }

                            if (string.IsNullOrEmpty(SearchCriteria.Country))
                            {
                                recruiterUrl = recruiterUrl.Replace("&countryCode=", "");
                            }

                            if (string.IsNullOrEmpty(SearchCriteria.PostalCode))
                            {
                                recruiterUrl = recruiterUrl.Replace("&postalCode=", "");
                            }
                            if (string.IsNullOrEmpty(SearchCriteria.within))
                            {
                                recruiterUrl = recruiterUrl.Replace("&radiusMiles=", "");
                            }

                            if (string.IsNullOrEmpty(SearchCriteria.Title))
                            {
                                recruiterUrl = recruiterUrl.Replace("&jobTitle=", "");
                            }
                            if (string.IsNullOrEmpty(SearchCriteria.TitleValue))
                            {
                                recruiterUrl = recruiterUrl.Replace("&jobTitleTimeScope=", "");
                            }
                            if (string.IsNullOrEmpty(SearchCriteria.Company))
                            {
                                recruiterUrl = recruiterUrl.Replace("&company=", "");
                            }
                            if (string.IsNullOrEmpty(SearchCriteria.CompanyValue))
                            {
                                recruiterUrl = recruiterUrl.Replace("&companyTimeScope=", "");
                            }

                            if (string.IsNullOrEmpty(SearchCriteria.FirstName))
                            {
                                recruiterUrl = recruiterUrl.Replace("&firstName=", "");
                            }
                            if (string.IsNullOrEmpty(SearchCriteria.LastName))
                            {
                                recruiterUrl = recruiterUrl.Replace("&lastName=", "");
                            }
                            if (string.IsNullOrEmpty(SearchCriteria.YearOfExperience))
                            {
                                recruiterUrl = recruiterUrl.Replace("&facet.TE=", "");
                            }
                            if (string.IsNullOrEmpty(SearchCriteria.CompanySize))
                            {
                                recruiterUrl = recruiterUrl.Replace("&facet.CS=", "");
                            }
                            if (string.IsNullOrEmpty(SearchCriteria.language))
                            {
                                recruiterUrl = recruiterUrl.Replace("&facet.L=", "");
                            }
                            if (string.IsNullOrEmpty(SearchCriteria.IndustryType))
                            {
                                recruiterUrl = recruiterUrl.Replace("&facet.I=", "");
                            }
                            if (string.IsNullOrEmpty(SearchCriteria.SeniorLevel))
                            {
                                recruiterUrl = recruiterUrl.Replace("&facet.SE=", "");
                            }
                            if (string.IsNullOrEmpty(SearchCriteria.Function))
                            {
                                recruiterUrl = recruiterUrl.Replace("&facet.FA=", "");
                            }
                            if (string.IsNullOrEmpty(SearchCriteria.Relationship))
                            {
                                recruiterUrl = recruiterUrl.Replace("&facet.N=", "");
                            }
                            if (string.IsNullOrEmpty(SearchCriteria.InerestedIn))
                            {
                                recruiterUrl = recruiterUrl.Replace("&facet.P=", "");
                            }
                            if (string.IsNullOrEmpty(SearchCriteria.Fortune1000))
                            {
                                recruiterUrl = recruiterUrl.Replace("&facet.F=", "");
                            }
                            if (string.IsNullOrEmpty(SearchCriteria.Group))
                            {
                                recruiterUrl = recruiterUrl.Replace("&facet.FG=", "");
                            }
                            if (string.IsNullOrEmpty(SearchCriteria.RecentlyJoined))
                            {
                                recruiterUrl = recruiterUrl.Replace("&facet.DR=", "");
                            }

                            recruiterUrl = recruiterUrl.Trim();

                            referralUrl = "https://www.linkedin.com/recruiter/search?searchHistoryId=1256614203&searchCacheKey=226602fa-2763-48a7-a2ff-dfddc87c4840%2CIwQE&linkContext=Controller%3ApeopleSearch%2CAction%3AresultsWithFacets%2CID%3A1256614203&page=1&start=0&count=25";
                            string pageSourceaAdvanceSearch11 = string.Empty;

                            try
                            {
                                pageSourceaAdvanceSearch11 = HttpHelper.getHtmlfromUrlNewRefre(new Uri(recruiterUrl), referralUrl);
                            }
                            catch { }
                            string strTotalPageNO = string.Empty;
                            
                            if (IsShowLoggerPagecount)
                            {
                                strTotalPageNO = Utils.getBetween(pageSourceaAdvanceSearch11, "total\":", ",").Trim();

                                
                                try
                                {
                                    pagenumberrecruiter = int.Parse(strTotalPageNO);


                                    Log("[ " + DateTime.Now + " ] => [ Total Results :  " + pagenumberrecruiter + " ]");

                                }
                                catch (Exception)
                                {

                                }

                                pagenumberrecruiter = (pagenumberrecruiter / 25) + 1;

                                if (pagenumberrecruiter == -1)
                                {
                                    pagenumberrecruiter = 2;
                                }

                                if (pagenumberrecruiter == 1)
                                {
                                    pagenumberrecruiter = 2;
                                }

                                //if (IsShowLoggerPagecount)
                                {
                                    if (pagenumberrecruiter >= 1)
                                    {
                                        _HttpHelper = HttpHelper;

                                        if (!Globals.scrapeWithoutGoingToMainProfile)
                                        {
                                            new Thread(() =>
                                            {
                                                if (SearchCriteria.starter)
                                                {
                                                    string CheckString = string.Empty;
                                                    finalUrlCollectionForRecruter(CheckString);
                                                }

                                            }).Start();
                                        }
                                    }
                                }
                            }
                            IsShowLoggerPagecount = false;
                            while (i <= pagenumberrecruiter)
                            {
                                if (SearchCriteria.starter)
                                {


                                    if (pageSourceaAdvanceSearch11.Contains("memberId"))
                                    {
                                        try
                                        {
                                            List<string> PageSerchUrl = GettingAllUrlRecruiterType(pageSourceaAdvanceSearch11);
                                            PageSerchUrl.Distinct();


                                            if (PageSerchUrl.Count == 0)
                                            {
                                                Log("[ " + DateTime.Now + " ] => [ On the basis of your Account you can able to see " + RecordURL.Count + " Results ]");
                                                break;
                                            }

                                            foreach (string item in PageSerchUrl)
                                            {
                                                if (SearchCriteria.starter)
                                                {
                                                    if (item.Contains("recruiter/profile"))
                                                    {
                                                        try
                                                        {
                                                            string urlSerch = item;
                                                            if (urlSerch.Contains("recruiter/profile"))
                                                            {
                                                                RecordURL.Add(urlSerch);
                                                                if (!queRecordUrl.Contains(urlSerch))
                                                                {
                                                                    queRecordUrl.Enqueue(urlSerch);
                                                                }
                                                                RecordURL = RecordURL.Distinct().ToList();
                                                            }

                                                            try
                                                            {
                                                                Log("[ " + DateTime.Now + " ] => [ " + urlSerch + " ]");
                                                            }
                                                            catch { }
                                                        }
                                                        catch { }
                                                    }
                                                }
                                            }

                                            if (i == pagenumberrecruiter)
                                            {
                                                break;
                                            }

                                            startCount += 25; ;
                                            i++;
                                            Thread.Sleep(4000);
                                            goto StartAgain;
                                        }
                                        catch { }
                                    }

                                }
                            }
                            #region MyRegion

                            //string searchHistoryId = Utils.getBetween(pageSourceaAdvanceSearch, "searchHistoryId\":", ",").Replace("\"", "").Trim();
                           // string searchCacheKey = Utils.getBetween(pageSourceaAdvanceSearch, "searchCacheKey\":", ",\"").Replace("\"", "").Trim();
                            //string linkContext = Utils.getBetween(pageSourceaAdvanceSearch, "linkContext\":", ",\"").Replace("\"", "").Trim();
                            //referralUrl = "https://www.linkedin.com/recruiter/search?searchHistoryId=" + Uri.EscapeDataString(searchHistoryId) + "&searchCacheKey=" + Uri.EscapeDataString(searchCacheKey).Replace(",", "%2C") + "&linkContext=" + Uri.EscapeDataString(linkContext) + "&page=1&start=0&count=25";

                            //pageSourceaAdvanceSearch = HttpHelper.getHtmlfromUrl1(new Uri(referralUrl));
                            //searchHistoryId = Utils.getBetween(pageSourceaAdvanceSearch, "searchHistoryId\":", ",").Replace("\"", "").Trim();
                            //searchCacheKey = Utils.getBetween(pageSourceaAdvanceSearch, "searchCacheKey\":", ",\"").Replace("\"", "").Trim();
                            //linkContext = Utils.getBetween(pageSourceaAdvanceSearch, "linkContext\":", ",\"").Replace("\"", "").Trim();


                            //int startindex = pageSourceaAdvanceSearch.IndexOf("csrfToken");
                            //if (startindex > 0)
                            //{
                            //    string start = pageSourceaAdvanceSearch.Substring(startindex);
                            //    int endindex = start.IndexOf(">");
                            //    string end = start.Substring(0, endindex);
                            //    csrfToken = end.Replace("csrfToken=", "").Replace("\\", "").Replace("\"", string.Empty).Replace("%3A", ":"); ;
                            //} 

                            //string postData = "{\"keywords\":\"hr\",\"locationParams\":{\"countryCode\":\"us\",\"postalCode\":[null]},\"metaParams\":{\"resetFacets\":true,\"reset\":[\"keywords\",\"countryCode\",\"postalCode\",\"company\",\"companyTimeScope\",\"jobTitle\",\"jobTitleTimeScope\",\"notes\",\"projects\",\"reviews\",\"reminders\"],\"origin\":\"GHDS\"},\"pagingParams\":{\"count\":1}}";
                            //postData = "{\"keywords\":\"hr\",\"locationParams\":{\"countryCode\":\"us\",\"postalCode\":[null]},\"metaParams\":{\"resetFacets\":true,\"reset\":[\"keywords\",\"countryCode\",\"postalCode\",\"company\",\"companyTimeScope\",\"jobTitle\",\"jobTitleTimeScope\",\"notes\",\"projects\",\"reviews\",\"reminders\"],\"origin\":\"GHDS\"},\"pagingParams\":{\"count\":1}}";

                            //string responsePostData = HttpHelper.postFormDataRefDemo(new Uri("https://www.linkedin.com/recruiter/search"), postData, referralUrl, csrfToken, "XMLHttpRequest", "", "https://www.linkedin.com", "");
                            //responsePostData = HttpHelper.postFormDataRefDemo(new Uri("https://www.linkedin.com/recruiter/search"), "", referralUrl, csrfToken, "", "XMLHttpRequest", "https://www.linkedin.com", "1");
                            //string demo = responsePostData;
                            #endregion

                        }
                        catch { }

                    }
                    return;
                   
                }

                
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
                        if (SearchCriteria.Location == "Y")
                        {
                            SearchCriteria.Country = string.Empty;
                        }
                       
                        //if (NewSearchPage == string.Empty)
                        //{
                        //    string PostDataForPrimiumAccount = "csrfToken=" + csrfToken + "&keepFacets=true&pplSearchOrigin=ADVS&keywords=" + Uri.EscapeDataString(SearchCriteria.Keyword) + "&fname=" + SearchCriteria.FirstName + "&lname=" + SearchCriteria.LastName + "&companyScope=" + SearchCriteria.CompanyValue + "&searchLocationType=" + SearchCriteria.Location + "&countryCode=" + SearchCriteria.Country + "&postalCode=" + SearchCriteria.PostalCode + "&distance=" + SearchCriteria.within + "&title=&company=" + SearchCriteria.Company + "&currentCompany=" + SearchCriteria.CompanyValue + "&school=&I=" + SearchCriteria.IndustryType + "&FG=" + SearchCriteria.Group + "&N=" + SearchCriteria.Relationship + "&L=" + SearchCriteria.language + "&FA=" + SearchCriteria.Function + "&CS=" + SearchCriteria.CompanySize + "&SE=" + SearchCriteria.SeniorLevel + "&P=" + SearchCriteria.InerestedIn + "&TE=" + SearchCriteria.YearOfExperience + "&DR=" + SearchCriteria.RecentlyJoined + "&F=" + SearchCriteria.Fortune1000 + "&sortCriteria=R&viewCriteria=2&%2Fsearch%2Ffpsearch=Search";
                        //    ResponseWallPostForPremiumAcc = HttpHelper.postFormData(new Uri("http://www.linkedin.com/search/fpsearch"), PostDataForPrimiumAccount);
                        //}
                        //else
                        {
                            string GetDataForPrimiumAccount = string.Empty;
                            GetDataForPrimiumAccount = "http://www.linkedin.com/vsearch/p?openAdvancedForm=true&keywords=" + Uri.EscapeDataString(SearchCriteria.Keyword) + "&title=" + SearchCriteria.Title + "&titleScope=" + SearchCriteria.TitleValue + "&firstName=" + SearchCriteria.FirstName + "&lastName=" + SearchCriteria.LastName + "&postalCode=" + SearchCriteria.PostalCode + "&companyScope=" + SearchCriteria.CompanyValue + "&locationType=" + SearchCriteria.Location + "&countryCode=" + SearchCriteria.Country + "&company=" + SearchCriteria.Company + "&distance=" + SearchCriteria.within + "&f_FG=" + SearchCriteria.Group + "&f_L=" + SearchCriteria.language + "&f_N=" + SearchCriteria.Relationship + "&f_G=" + SearchCriteria.LocationArea + "&f_I=" + SearchCriteria.IndustryType + "&f_TE=" + SearchCriteria.YearOfExperience + "&f_FA=" + SearchCriteria.Function + "&f_SE=" + SearchCriteria.SeniorLevel + "&f_P=" + SearchCriteria.InerestedIn + "&f_CS=" + SearchCriteria.CompanySize + "&f_F=" + SearchCriteria.Fortune1000 + "&f_DR=" + SearchCriteria.RecentlyJoined + "&orig=ADVS";
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

                                #region Commented
                                //Regex IdCheck = new Regex("^[0-9]*$");

                                //string[] tempResultArr = Regex.Split(tempResult, "[^0-9]");

                                //foreach (string item in tempResultArr)
                                //{
                                //    try
                                //    {
                                //        if(IdCheck.IsMatch(item))
                                //        {
                                //            strPageNumber = item;

                                //            break;
                                //        }
                                //    }
                                //    catch (Exception ex)
                                //    {
                                //    }
                                //} 
                                #endregion

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

                    Log("[ " + DateTime.Now + " ] => [ Total Results :  " + pagenumber + " ]");
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

                    if (!Globals.scrapeWithoutGoingToMainProfile)
                    {
                        new Thread(() =>
                        {
                            if (SearchCriteria.starter)
                            {
                                string CheckString = string.Empty;
                                finalUrlCollection(CheckString);
                            }

                        }).Start();
                    }
                    
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
                                    PostdataForPagination = "keywords=" + Uri.EscapeDataString(SearchCriteria.Keyword) + "&title=" + Uri.EscapeDataString(SearchCriteria.Title) + "&fname=" + SearchCriteria.FirstName + "&lname=" + SearchCriteria.LastName + "&searchLocationType=" + SearchCriteria.Location + "&f_FG=" + SearchCriteria.Group + "&companyScope=" + SearchCriteria.CompanyValue + "&countryCode=" + SearchCriteria.Country + "&company=" + SearchCriteria.Company + "&viewCriteria=1&sortCriteria=R&facetsOrder=CC%2CN%2CG%2CI%2CPC%2CED%2CL%2CFG%2CTE%2CFA%2CSE%2CP%2CCS%2CF%2CDR&page_num=" + i + "&openFacets=N%2CCC%2CG";
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
                                    PostdataForPagination = "keywords=" + Uri.EscapeDataString(SearchCriteria.Keyword) + "&title=" + Uri.EscapeDataString(SearchCriteria.Title) + "&fname=" + SearchCriteria.FirstName + "&lname=" + SearchCriteria.LastName + "&searchLocationType=" + SearchCriteria.Location + "&f_FG=" + SearchCriteria.Group + "&companyScope=" + SearchCriteria.CompanyValue + "&countryCode=" + SearchCriteria.Country + "&keepFacets=keepFacets&I=" + SearchCriteria.IndustryType + "&SE=" + SearchCriteria.SeniorLevel + "&pplSearchOrigin=ADVS&viewCriteria=2&sortCriteria=R&facetsOrder=N%2CCC%2CI%2CPC%2CED%2CL%2CFG%2CTE%2CFA%2CSE%2CP%2CCS%2CF%2CDR%2CG&page_num=" + i + "&openFacets=N%2CCC%2CI";

                                    PostResponce = HttpHelper.postFormData(new Uri(PostRequestURL), PostdataForPagination);
                                    //Temporosy code for client
                                    //GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Pagesource 3 >>>> " + PostResponce, Globals.Path_InBoardProGetDataPagesource);
                                }
                                catch { }
                            }
                            else if (ResponseWallPostForPremiumAcc.Contains("openAdvancedForm=true"))
                            {
                                PostRequestURL = "http://www.linkedin.com/vsearch/p?";
                                PostdataForPagination = "keywords=" + Uri.EscapeDataString(SearchCriteria.Keyword) + "&title=" + Uri.EscapeDataString(SearchCriteria.Title) + "&titleScope=" + SearchCriteria.TitleValue + "&firstName=" + SearchCriteria.FirstName + "&lastName=" + SearchCriteria.LastName + "&postalCode=" + SearchCriteria.PostalCode + "&openAdvancedForm=true&companyScope=" + SearchCriteria.CompanyValue + "&locationType=" + SearchCriteria.Location + "&f_FG=" + SearchCriteria.Group + "&countryCode=" + SearchCriteria.Country + "&company=" + SearchCriteria.Company + "&distance=" + SearchCriteria.within + "&f_N=" + SearchCriteria.Relationship + "&f_G=" + SearchCriteria.LocationArea + "&f_I=" + SearchCriteria.IndustryType + "&f_TE=" + SearchCriteria.YearOfExperience + "&f_FA=" + SearchCriteria.Function + "&f_SE=" + SearchCriteria.SeniorLevel + "&f_P=" + SearchCriteria.InerestedIn + "&f_CS=" + SearchCriteria.CompanySize + "&f_F=" + SearchCriteria.Fortune1000 + "&f_DR=" + SearchCriteria.RecentlyJoined + "&orig=ADVS&page_num=" + i + "";
                                PostResponce = HttpHelper.postFormData(new Uri(PostRequestURL), PostdataForPagination);
                                //Temporosy code for client
                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Pagesource 4 >>>> " + PostResponce, Globals.Path_InBoardProGetDataPagesource);
                            }
                            else
                            {
                                try
                                {
                                    PostRequestURL = "http://www.linkedin.com/search/hits";
                                    PostdataForPagination = "keywords=" + Uri.EscapeDataString(SearchCriteria.Keyword) + "&title=" + Uri.EscapeDataString(SearchCriteria.Title) + "&fname=" + SearchCriteria.FirstName + "&lname=" + SearchCriteria.LastName + "&searchLocationType=" + SearchCriteria.Location + "&f_FG=" + SearchCriteria.Group + "&countryCode=" + SearchCriteria.Country + "&viewCriteria=1&sortCriteria=R&facetsOrder=CC%2CN%2CG%2CI%2CPC%2CED%2CL%2CFG%2CTE%2CFA%2CSE%2CP%2CCS%2CF%2CDR&page_num=" + i + "&openFacets=N%2CCC%2CG";
                                    PostResponce = HttpHelper.postFormData(new Uri(PostRequestURL), PostdataForPagination);
                                }
                                catch { }
                            }

                            if (PostResponce.Contains("/profile/view?id"))
                            {

                                if (Globals.scrapeWithoutGoingToMainProfile)
                                {
                                    
                                    if (i > 10)
                                    {
                                        if(PostResponce.Contains("Upgrade your account to see more results"))
                                        {
                                            Log("[ " + DateTime.Now + " ] => [ Search result limit reached. ]");
                                            Log("[ " + DateTime.Now + " ] => [ PROCESS COMPLETE ]");
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        Log("[ " + DateTime.Now + " ] => [ Crawling search result page number : " + i + " ]");
                                        CrawlingProfileDataFromSearchPage(PostResponce, ref HttpHelper);
                                    }
                                }
                                else
                                {
                                    List<string> PageSerchUrl = GettingAllUrl(PostResponce);
                                    PageSerchUrl.Distinct();

                                    if (PageSerchUrl.Count == 0)
                                    {
                                        Log("[ " + DateTime.Now + " ] => [ On the basis of your Account you can able to see " + RecordURL.Count + " Results ]");
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
                                                        if (!queRecordUrl.Contains(urlSerch))
                                                        {
                                                            queRecordUrl.Enqueue(urlSerch);
                                                        }
                                                        RecordURL = RecordURL.Distinct().ToList();
                                                    }

                                                    try
                                                    {
                                                        Log("[ " + DateTime.Now + " ] => [ " + urlSerch + " ]");
                                                    }
                                                    catch { }
                                                }
                                                catch { }
                                            }
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
                }
                #region For Else
                else
                {
                    if (!Globals.scrapeWithoutGoingToMainProfile)
                    {
                        if (SearchCriteria.starter)
                        {

                            #region loop
                            if (ResponseWallPostForPremiumAcc.Contains("/profile/view?id"))
                            {

                                List<string> PageSerchUrl = ChilkatBasedRegex.GettingAllUrls(ResponseWallPostForPremiumAcc, "profile/view?id");
                                if (PageSerchUrl.Count == 0)
                                {

                                    Log("[ " + DateTime.Now + " ] => [ On the basis of your Account or Your Input you can able to see " + RecordURL.Count + "  Results ]");

                                }

                                foreach (string item in PageSerchUrl)
                                {
                                    if (SearchCriteria.starter)
                                    {
                                        if (item.Contains("pp_profile_name_link"))
                                        {
                                            string urlSerch = "http://www.linkedin.com" + item;
                                            Log("[ " + DateTime.Now + " ] => [ " + urlSerch + " ]");
                                            RecordURL.Add(urlSerch);
                                            queRecordUrl.Enqueue(urlSerch);

                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                    }

                }

                if (strPageNumber != string.Empty)
                {
                    if (strPageNumber != "0")
                    {
                        Log("-------------------------------------------------------------------------------------------------------------------------------");
                        Log("[ " + DateTime.Now + " ] => [ No Of Results Found >>> " + strPageNumber + " ]");
                    }
                }

                RecordURL.Distinct();
                //if (SearchCriteria.starter)
                //{
                //    finalUrlCollection(ref HttpHelper);
                //    //new Thread(() =>
                //    //{
                //    //    test();
                //    //}).Start();
                //}

            }

           #endregion

            catch { }
            #endregion
        } 
        #endregion

        #region StartCampaignInBoardProGetDataWithPagination
        public void StartCampaignInBoardProGetDataWithPagination(ref GlobusHttpHelper HttpHelper, string Account, string FirstName, string LastName, string Location, string Country, string LocationArea, string PostalCode, string Company, string Keyword, string Title, string IndustryType, string Relationship, string language, string Groups, string FileName, string TitleValue, string CompanyValue, string within, string YearsOfExperience, string Function, string SeniorLevel, string IntrestedIn, string CompanySize, string Fortune1000, string RecentlyJoined)
        {
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

                                #region Commented
                                //Regex IdCheck = new Regex("^[0-9]*$");

                                //string[] tempResultArr = Regex.Split(tempResult, "[^0-9]");

                                //foreach (string item in tempResultArr)
                                //{
                                //    try
                                //    {
                                //        if(IdCheck.IsMatch(item))
                                //        {
                                //            strPageNumber = item;

                                //            break;
                                //        }
                                //    }
                                //    catch (Exception ex)
                                //    {
                                //    }
                                //} 
                                #endregion

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

                    Log("[ " + DateTime.Now + " ] => [ Total Results :  " + pagenumber + " ]");
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

                    new Thread(() =>
                    {
                        if (SearchCriteria.starter)
                        {
                            
                            string CheckString = "CampaignScraper" + FileName;
                            finalUrlCollectionCampaignScraper(CheckString, Account);
                        }

                    }).Start();

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
                                    Log("[ " + DateTime.Now + " ] => [ On the basis of you Account your can able to see " + RecordURL.Count + " Results ]");
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

                                                    //if (!queRecordUrl.Contains(urlSerch))
                                                    //{
                                                    //    queRecordUrl.Enqueue(urlSerch);
                                                    //}
                                                    RecordURL = RecordURL.Distinct().ToList();
                                                }

                                                try
                                                {
                                                    Log("[ " + DateTime.Now + " ] => [ " + urlSerch + " ]");
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

                                Log("[ " + DateTime.Now + " ] => [ On the basis of your Account or Your Input you can able to see " + RecordURL.Count + "  Results ]");

                            }

                            foreach (string item in PageSerchUrl)
                            {
                                if (SearchCriteria.starter)
                                {
                                    if (item.Contains("pp_profile_name_link"))
                                    {
                                        string urlSerch = "http://www.linkedin.com" + item;
                                        Log("[ " + DateTime.Now + " ] => [ " + urlSerch + " ]");
                                        RecordURL.Add(urlSerch);
                                        queRecordUrl.Enqueue(urlSerch);

                                    }
                                }
                            }
                        }
                        #endregion
                    }

                }

                if (strPageNumber != string.Empty)
                {
                    if (strPageNumber != "0")
                    {
                        Log("-------------------------------------------------------------------------------------------------------------------------------");
                        Log("[ " + DateTime.Now + " ] => [ No Of Results Found >>> " + strPageNumber + " ]");
                    }
                }

                RecordURL.Distinct();
                //if (SearchCriteria.starter)
                //{
                //    finalUrlCollection(ref HttpHelper);
                //    //new Thread(() =>
                //    //{
                //    //    test();
                //    //}).Start();
                //}

            }

                #endregion

            catch { }
            #endregion
        }
        #endregion
        
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



        #region GettingAllUrlRecruiteryType
        public List<string> GettingAllUrlRecruiterType(string PageSource)
        {
            List<string> suburllist = new List<string>();
            List<string> subtitlelist = new List<string>();
            try
            {

                if (PageSource.Contains("memberId\":"))
                {
                    string[] trkArr = Regex.Split(PageSource, "memberId\":");
                    if (trkArr[0].Contains("authToken"))
                    {
                        trkArr = Regex.Split(PageSource, "authToken\":");
                    }
                    trkArr = trkArr.Skip(1).ToArray();
                    foreach (string item in trkArr)
                    {
                        try
                        {
                            if (item.Contains("authToken") || item.Contains("memberId\":"))
                            {
                                string authToken = Utils.getBetween(item, "authToken\":", ",").Replace("\"","").Replace("}","").Trim();
                                string memberId = Utils.getBetween(item, "memberId\":", ",").Replace("\"", "").Replace("}", "").Trim();
                                //string headline = Utils.getBetween(item, "headline\":", ",\"").Replace("\"", "").Replace("}", "").Replace("&amp;","&").Trim();
                                if (string.IsNullOrEmpty(memberId))
                                {
                                    memberId = Utils.getBetween(item, "", ",").Replace("\"", "").Replace("}", "").Trim();
                                }
                                if (string.IsNullOrEmpty(authToken))
                                {
                                    authToken = Utils.getBetween(item, "", ",").Replace("\"", "").Replace("}", "").Trim();
                                }
                                //string finalurl = "https://www.linkedin.com/recruiter/profile/" + memberId + "," + authToken + ",CAP" + "<:>" + headline;
                                string finalurl = "https://www.linkedin.com/recruiter/profile/" + memberId + "," + authToken + ",CAP";
                                suburllist.Add(finalurl);
                            }
                        }
                        catch
                        {
                        }
                    }

                }

                if (PageSource.Contains("headline\":"))
                {
                    string[] trkArrtitle = Regex.Split(PageSource, "headline\":");
                    trkArrtitle = trkArrtitle.Skip(1).ToArray();
                    foreach (string titleItem in trkArrtitle)
                    {
                        string title = Utils.getBetween(titleItem, "", ",\"").Replace("\"","").Replace("}","").Trim();
                        subtitlelist.Add(title);
                    }
                }

                if (suburllist.Count == subtitlelist.Count)
                {
                    for (int i = 0; i < suburllist.Count; i++)
                    {
                        suburllist[i] = suburllist[i] + "<:>" + subtitlelist[i];
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
        private void finalUrlCollection(string CheckString)
        {
            string Account = string.Empty;
            if (Globals.IsStop_CompanyEmployeeScraperThread)
            {
                return;
            }

            if (CheckString.Contains("CampaignScraper"))
            {
                int startIndex = CheckString.IndexOf("#");
                string Start = CheckString.Substring(startIndex);
                int endIndex = Start.IndexOf("*");
                string end = Start.Substring(0, endIndex).Replace("*", string.Empty);
                Account = end;
            }

            Globals.lstCompanyEmployeeScraperThread.Add(Thread.CurrentThread);
            Globals.lstCompanyEmployeeScraperThread = Globals.lstCompanyEmployeeScraperThread.Distinct().ToList();
            Thread.CurrentThread.IsBackground = true;

            
            try
            {
                List<string> numburlpp = new List<string>();
                GlobusHttpHelper HttpHelper = _HttpHelper;
                if (SearchCriteria.starter)
                {
                    RecordURL = RecordURL.Distinct().ToList();

                    //Log("[ " + DateTime.Now + " ] => [ Total Find URL >>> " + RecordURL.Count.ToString() + " ]");
                    //Log("-------------------------------------------------------------------------------------------------------------------------------");
                    Thread.Sleep(1 * 10 * 1000);
                    //foreach (string item in RecordURL)
                    while(true)                    
                    {
                        if (queRecordUrl.Count > 0)
                        {
                            string item = queRecordUrl.Dequeue();

                            try
                            {
                                if (item.Contains("pp_profile_name_link"))
                                {
                                    if (SearchCriteria.starter)
                                    {

                                        string urltemp = item;
                                        numburlpp.Add(urltemp);

                                        Log("[ " + DateTime.Now + " ] => [ " + urltemp + " ]");

                                        Log("[ " + DateTime.Now + " ] => [ Fetching Data From URL ]");
                                        urltemp = urltemp + CheckString;
                                        bool check = CrawlingLinkedInPage(urltemp, ref HttpHelper);

                                        int delay = RandomNumberGenerator.GenerateRandom(SearchCriteria.scraperMinDelay, SearchCriteria.scraperMaxDelay);
                                        Log("[ " + DateTime.Now + " ] => [ Delay for : " + delay + " Seconds ]");
                                        Thread.Sleep(delay * 1000);

                                        if (!check)
                                        {
                                            string stop = string.Empty;
                                        }


                                        //if (!CrawlingLinkedInPage(urltemp, ref HttpHelper))
                                        //{
                                        //    CrawlingPageDataSource(urltemp, ref HttpHelper);
                                        //    Thread.Sleep(500);
                                        //}
                                    }

                                }
                                else if (item.Contains("/profile/view?"))
                                {

                                    string urltemp = item;
                                    numburlpp.Add(urltemp);


                                    Log("[ " + DateTime.Now + " ] => [ " + urltemp + " ]");

                                    Log("[ " + DateTime.Now + " ] => [ Fetching Data From URL ]");
                                    urltemp = urltemp + CheckString;
                                    bool check = CrawlingLinkedInPage(urltemp, ref HttpHelper);

                                    int delay = RandomNumberGenerator.GenerateRandom(SearchCriteria.scraperMinDelay, SearchCriteria.scraperMaxDelay);
                                    Log("[ " + DateTime.Now + " ] => [ Delay for : " + delay + " Seconds ]");
                                    Thread.Sleep(delay * 1000);

                                    if (!check)
                                    {
                                        string stop = string.Empty;
                                    }

                                    //if (!CrawlingLinkedInPage(urltemp, ref HttpHelper))
                                    //{
                                    //    CrawlingPageDataSource(urltemp, ref HttpHelper);
                                    //    Thread.Sleep(500);
                                    //}

                                }
                            }
                            catch
                            {
                            }
                        }
                        else
                        {
                            Thread.Sleep(1 * 30 * 1000);

                            if (queRecordUrl.Count == 0)
                            {
                                Thread.Sleep(1 * 60 * 1000);

                                if (queRecordUrl.Count == 0)
                                {
                                    Log("[ " + DateTime.Now + " ] => [ Find All the Data ]");
                                    break;
                                }
                            }
                        }
                    }

                   
                    Log("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                    Log("-----------------------------------------------------------------------------------------------------------------------------------");
                }
            }
            catch (Exception ex)
            {

            }
        } 
        #endregion



        #region finalUrlCollection
        private void finalUrlCollectionForRecruter(string CheckString)
        {
            string Account = string.Empty;
            if (Globals.IsStop_CompanyEmployeeScraperThread)
            {
                return;
            }

           

            Globals.lstCompanyEmployeeScraperThread.Add(Thread.CurrentThread);
            Globals.lstCompanyEmployeeScraperThread = Globals.lstCompanyEmployeeScraperThread.Distinct().ToList();
            Thread.CurrentThread.IsBackground = true;


            try
            {
                List<string> numburlpp = new List<string>();
                GlobusHttpHelper HttpHelper = _HttpHelper;
                if (SearchCriteria.starter)
                {
                    RecordURL = RecordURL.Distinct().ToList();

                    //Log("[ " + DateTime.Now + " ] => [ Total Find URL >>> " + RecordURL.Count.ToString() + " ]");
                    //Log("-------------------------------------------------------------------------------------------------------------------------------");
                    Thread.Sleep(1 * 10 * 1000);
                    //foreach (string item in RecordURL)
                    while (true)
                    {
                        if (queRecordUrl.Count > 0)
                        {
                            string item = queRecordUrl.Dequeue();

                            try
                            {

                                if (item.Contains("recruiter/profile"))
                                {

                                    string urltemp = item;
                                    numburlpp.Add(urltemp);


                                    Log("[ " + DateTime.Now + " ] => [ " + urltemp + " ]");

                                    Log("[ " + DateTime.Now + " ] => [ Fetching Data From URL ]");
                                    urltemp = urltemp + CheckString;
                                    bool check = CrawlingLinkedInPageRecruiter(urltemp, ref HttpHelper);

                                    int delay = RandomNumberGenerator.GenerateRandom(SearchCriteria.scraperMinDelay, SearchCriteria.scraperMaxDelay);
                                    Log("[ " + DateTime.Now + " ] => [ Delay for : " + delay + " Seconds ]");
                                    Thread.Sleep(delay * 1000);

                                }
                            }
                            catch
                            {
                            }
                        }
                        else
                        {
                            Thread.Sleep(1 * 30 * 1000);

                            if (queRecordUrl.Count == 0)
                            {
                                Thread.Sleep(1 * 60 * 1000);

                                if (queRecordUrl.Count == 0)
                                {
                                    Log("[ " + DateTime.Now + " ] => [ Find All the Data ]");
                                    break;
                                }
                            }
                        }
                    }


                    Log("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                    Log("-----------------------------------------------------------------------------------------------------------------------------------");
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region finalUrlCollection
        private void finalUrlCollection(ref GlobusHttpHelper HttpHelper)
        {
            try
            {
                List<string> numburlpp = new List<string>();
                
                if (SearchCriteria.starter)
                {
                    RecordURL = RecordURL.Distinct().ToList();

                    Log("[ " + DateTime.Now + " ] => [ Total Find URL >>> " + RecordURL.Count.ToString() + " ]");
                    Log("-------------------------------------------------------------------------------------------------------------------------------");
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

                                        Log("[ " + DateTime.Now + " ] => [ " + urltemp + " ]");

                                        Log("[ " + DateTime.Now + " ] => [ Fetching Data From URL ]");

                                        if (!CrawlingLinkedInPage(urltemp, ref HttpHelper))
                                        {
                                            CrawlingPageDataSource(urltemp, ref HttpHelper);
                                        }
                                    }

                                }
                                else if (item.Contains("/profile/view?"))
                                {
                                    string urltemp = item;
                                    numburlpp.Add(urltemp);


                                    Log("[ " + DateTime.Now + " ] => [ " + urltemp + " ]");

                                    Log("[ " + DateTime.Now + " ] => [ Fetching Data From URL ]");

                                    if (!CrawlingLinkedInPage(urltemp, ref HttpHelper))
                                    {
                                        CrawlingPageDataSource(urltemp, ref HttpHelper);
                                    }

                                }
                            }
                            catch
                            {
                            }
                        
                        
                    }


                    Log("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                    Log("-----------------------------------------------------------------------------------------------------------------------------------");
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion


        #region finalUrlCollectionCampaignScraper
        private void finalUrlCollectionCampaignScraper(string CheckString, string Account)
        {
            Thread.Sleep(1 * 30 * 1000);
            if (Globals.IsStop_CompanyEmployeeScraperThread)
            {
                return;
            }
            Globals.lstCompanyEmployeeScraperThread.Add(Thread.CurrentThread);
            Globals.lstCompanyEmployeeScraperThread = Globals.lstCompanyEmployeeScraperThread.Distinct().ToList();
            Thread.CurrentThread.IsBackground = true;

            DataSet DS = new DataSet();
            DataTable DT = new DataTable();
            clsDBQueryManager DQ = new clsDBQueryManager();

            try
            {
                DS = DQ.SelectUrl(Account);
                DT = DS.Tables["tb_CampaignScraperURL"];
            }
            catch
            { }
            try
            {
                List<string> numburlpp = new List<string>();
                GlobusHttpHelper HttpHelper = _HttpHelper;
                if (SearchCriteria.starter)
                {

                    if (DT.Rows.Count > 0)
                    {
                        foreach (DataRow item in DT.Rows)
                        {
                            string urltemp = string.Empty;
                            try
                            {
                                string Url = Convert.ToString(item["Url"]);

                                if (Url.Contains("pp_profile_name_link"))
                                {
                                    if (SearchCriteria.starter)
                                    {
                                        urltemp = Url;
                                        numburlpp.Add(urltemp);

                                        Log("[ " + DateTime.Now + " ] => [ " + urltemp + " ]");

                                        Log("[ " + DateTime.Now + " ] => [ Fetching Data From URL ]");
                                        urltemp = urltemp + CheckString;
                                        bool check = CrawlingLinkedInPage(urltemp, ref HttpHelper);
                                        if (!check)
                                        {
                                            string stop = string.Empty;
                                        }
                                    }
                                }
                                else if (Url.Contains("/profile/view?"))
                                {
                                    urltemp = Url;
                                    numburlpp.Add(urltemp);

                                    Log("[ " + DateTime.Now + " ] => [ " + urltemp + " ]");

                                    Log("[ " + DateTime.Now + " ] => [ Fetching Data From URL ]");
                                    urltemp = urltemp + CheckString;
                                    bool check = CrawlingLinkedInPage(urltemp, ref HttpHelper);
                                    if (!check)
                                    {
                                        string stop = string.Empty;
                                    }
                                }

                                try
                                {
                                    urltemp = urltemp + "##";
                                    string Db_Url = string.Empty;
                                    int startindex = urltemp.IndexOf("CampaignScraper");
                                    string start = urltemp.Substring(startindex);
                                    int EndIndex = start.IndexOf("##");
                                    string End = start.Substring(0, EndIndex);
                                    string Rep = End;

                                    urltemp = urltemp.Replace(Rep, string.Empty).Replace("CampaignScraper", string.Empty).Replace("##",string.Empty);

                                    DQ.UpdateCampaignScraperUrl(urltemp);
                                }
                                catch
                                { }

                            }
                            catch
                            { }
                        }
                        Thread.Sleep(30 * 1000);
                        DS = DQ.SelectUrl(Account);
                        DT = DS.Tables["tb_CampaignScraperURL"];
                        if (DT.Rows.Count == 0)
                        {
                            //break;
                        }
                    }
                    else
                    {
                        Thread.Sleep(1 * 30 * 1000);

                        if (queRecordUrl.Count == 0)
                        {
                            Thread.Sleep(1 * 60 * 1000);

                            if (queRecordUrl.Count == 0)
                            {
                                Log("--------------------Find All the Data-------------------");
                                //break;
                            }
                        }
                    }
                }
                Log("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                Log("-----------------------------------------------------------------------------------------------------------------------------------");
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region CrawlingProfileDataFromSearchPage
        public void CrawlingProfileDataFromSearchPage(string searchResultPageSource, ref GlobusHttpHelper HttpHelper)
        {
            
           
            try
            {
                string[] person_split = Regex.Split(searchResultPageSource, "{\"person\":");
                foreach (string personData in person_split)
                {
                    string firstName = string.Empty;
                    string lastName = string.Empty;
                    string headlineTitle = string.Empty;
                    string industry = string.Empty;
                    string location = string.Empty;
                    string currentTitle = string.Empty;
                    string pastTitle = string.Empty;
                    string degreeOfConnection = string.Empty;

                    if (!personData.Contains("<!DOCTYPE html>"))
                    {
                        firstName = getBetween(personData, "\"firstName\":\"", "\",\"").Replace("&amp;", "&").Replace("\\u002d", "-");
                        lastName = getBetween(personData, "\"lastName\":\"", "\",\"");
                        headlineTitle = getBetween(personData, "\"fmt_headline\":\"", "\",\"").Replace("\\u002d", "-").Replace("\\u003cstrong class=\\\"highlight\\\"\\u003e", string.Empty).Replace("\\u003c/strong\\u003e", string.Empty).Replace("\",\"", string.Empty).Replace("&amp;", "&").Replace("\\u002d", "-").Trim();
                        location = getBetween(personData, "\"fmt_location\":\"", "\",\"").Replace("&amp;", "&").Replace("\\u002d", "-");
                        industry = getBetween(personData, "\"fmt_industry\":\"", "\",\"").Replace("&amp;", "&").Replace("\\u002d", "-");
                        if (personData.Contains("[{\"fieldName\":\"Current\",\""))
                        {
                            string currentTitleScrape = getBetween(personData, "[{\"fieldName\":\"Current\",\"", "],");
                            currentTitle = getBetween(currentTitleScrape, "\"fmt_heading\":\"", "\"}").Replace("\\u003cstrong class=\\\"highlight\\\"\\u003e", string.Empty).Replace("\\u003c/strong\\u003e", string.Empty).Replace("\\u003cB", string.Empty).Replace("\\u003e", string.Empty).Replace("\\u003c", string.Empty).Replace("/B", string.Empty).Replace("\",\"fmt_body\":\"", ".").Replace("\",\"", ", ").Replace("&amp;", "&").Replace("\\u002d", "-").Trim();
                            if (string.IsNullOrEmpty(currentTitle))
                            {
                                currentTitle = getBetween(currentTitleScrape, "bodyList\":[\"", "\"]}").Replace("\\u003cstrong class=\\\"highlight\\\"\\u003e", string.Empty).Replace("\\u003c/strong\\u003e", string.Empty).Replace("\\u003cB", string.Empty).Replace("\\u003e", string.Empty).Replace("\\u003c", string.Empty).Replace("/B", string.Empty).Replace("\",\"fmt_body\":\"", ".").Replace("\",\"", ", ").Replace("&amp;", "&").Replace("\\u002d", "-").Trim();
                            }
                        }
                        if (personData.Contains("\"fieldName\":\"Past\",\"bodyList\":"))
                        {
                            string pastTitleScrape = getBetween(personData, "\"fieldName\":\"Past\",\"bodyList\":", "}]");
                            pastTitle = getBetween(pastTitleScrape, "[\"", "\"]").Replace("\\u003cstrong class=\\\"highlight\\\"\\u003e", string.Empty).Replace("\\u003c/strong\\u003e", string.Empty).Replace("\",\"", ", ").Replace("&amp;", "&").Replace("\\u002d", "-").Trim();
                        }
                        try
                        {
                            string degreeConnectionScrape = getBetween(personData, "degree_contact_key\":\"", "\",\"");
                            if (degreeConnectionScrape.Contains(" 1st "))
                            {
                                degreeOfConnection = "1st";
                            }
                            else if (degreeConnectionScrape.Contains(" 2nd "))
                            {
                                degreeOfConnection = "2nd";
                            }
                            else if (degreeConnectionScrape.Contains(" 3rd "))
                            {
                                degreeOfConnection = "3rd";
                            }
                        }
                        catch
                        { }

                        try
                        {
                            if (string.IsNullOrEmpty(firstName)) firstName = "LinkedIn";
                            if (string.IsNullOrEmpty(lastName)) lastName = "Member";
                            if (string.IsNullOrEmpty(headlineTitle)) headlineTitle = "--";
                            if (string.IsNullOrEmpty(location)) location = "--";
                            if (string.IsNullOrEmpty(industry)) industry = "--";
                            if(string.IsNullOrEmpty(currentTitle)) currentTitle = "--";
                            if(string.IsNullOrEmpty(pastTitle)) pastTitle = "--";
                            if (string.IsNullOrEmpty(degreeOfConnection)) degreeOfConnection = "--";

                            Log("[ " + DateTime.Now + " ] => [ Writing data in the CSV for the profile of " + firstName + " " + lastName + " ]");

                            string LDS_FinalData = firstName.Replace(",", ";") + "," + lastName.Replace(",", ";") + "," + headlineTitle.Replace(",", ";") + "," + location.Replace(",", ";") + "," + industry.Replace(",", ";") + "," + currentTitle.Replace(",", ";") + "," + pastTitle.Replace(",", ";") + "," + degreeOfConnection.Replace(",", ";");
                            string FileName = "ScrapeWithoutGoingToMainProfile-" + SearchCriteria.FileName;
                            AppFileHelper.AddingLinkedInDataToCSVFileWithoutGoingToMainProfile(LDS_FinalData, FileName);
                            //return true;
                        }
                        catch
                        { }
                    }
                }
            }
            catch
            { }
        }
        #endregion

        #region CrawlingPageDataSource
        public void CrawlingPageDataSource(string Url, ref GlobusHttpHelper HttpHelper)
        {
            
            if (SearchCriteria.starter)
            {
                if (SearchCriteria.starter)
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

                        string titleCurrenttitle = string.Empty;
                        string titleCurrenttitle2 = string.Empty;
                        string titleCurrenttitle3 = string.Empty;
                        string titleCurrenttitle4 = string.Empty;
                        string Skill = string.Empty;
                        //string TypeOfProfile = "Public1";

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

                        string companyCurrenttitle1 = string.Empty;
                        string companyCurrenttitle2 = string.Empty;
                        string companyCurrenttitle3 = string.Empty;
                        string companyCurrenttitle4 = string.Empty;

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
                            System.Console.WriteLine(http.LastErrorText);
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
                            string[] UrlFull = System.Text.RegularExpressions.Regex.Split(Url, "&authType");
                            LDS_UserProfileLink = UrlFull[0];

                            LDS_UserProfileLink = Url;
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
                        string LDS_FinalData = LDS_ProfileType.Replace(",", ";") + "," + LDS_UserProfileLink.Replace(",", ";") + "," + LDS_FirstName.Replace(",", ";") + "," + LDS_LastName.Replace(",", ";") + "," + LDS_HeadLineTitle.Replace(",", ";") + "," + LDS_CurrentTitle.Replace(",", ";") + "," + LDS_CurrentCompany.Replace(",", ";") + "," + LDS_Connection.Replace(",", ";") + "," + LDS_Recommendations.Replace(",", ";") + "," + LDS_SkillAndExpertise.Replace(",", ";") + "," + LDS_Experience.Replace(",", ";") + "," + LDS_Education.Replace(",", ";") + "," + LDS_Groups.Replace(",", ";") + "," + LDS_UserEmail.Replace(",", ";") + "," + LDS_UserContactNumber.Replace(",", ";") + "," + LDS_PastTitles.Replace(",", ";") + "," + LDS_PastCompany.Replace(",", ";") + "," + LDS_Loction.Replace(",", ";") + "," + LDS_Country.Replace(",", ";") + "," + LDS_Industry.Replace(",", ";") + "," + LDS_Websites.Replace(",", ";") + "," + LDS_LoginID.Replace(",", ";") + ",";

                        if (LDS_FinalData.Contains("<strong class=\"highlight\"") || LDS_FinalData.Contains("<span class=\"full-name\"") || LDS_FinalData.Contains("<strong class=\"highlight\"") || LDS_FinalData.Contains("overview-connections\">"))
                        {
                            LDS_FinalData = LDS_FinalData.Replace("<span class=\"full-name\"", "").Replace("\n", "").Replace("<strong class=\"highlight\"", "").Replace("overview-connections\">", "").Replace("</strong>", "").Replace("<strong>", "");
                        }
                        if (!string.IsNullOrEmpty(LDS_FirstName) || !string.IsNullOrEmpty(LDS_FirstName))
                        {
                            Log("[ " + DateTime.Now + " ] => [ " + LDS_FinalData + " ]");
                        }
                        if (SearchCriteria.starter)
                        {

                            string tempFinalData = LDS_FinalData.Replace(";", "").Replace(LDS_UserProfileLink, "").Replace("Public", "").Replace(",", "").Replace(LDS_LoginID, "").Trim();

                            if (!string.IsNullOrEmpty(tempFinalData))
                            {
                                AppFileHelper.AddingLinkedInDataToCSVFile(LDS_FinalData, SearchCriteria.FileName);
                                //Log("Data Saved In CSV File With URL >>> " + LDS_UserProfileLink);
                            }

                            //if (!string.IsNullOrEmpty(LDS_FirstName) || !string.IsNullOrEmpty(LDS_FirstName))
                            //{
                            //    AppFileHelper.AddingLinkedInDataToCSVFile(LDS_FinalData, SearchCriteria.FileName);
                            //}
                        }

                    }
                    catch (Exception ex) { };

                }

            }
        } 
        #endregion

        #region Log
        private void Log(string log)
        {
            try
            {
                EventsArgs loggerEventsArgs = new EventsArgs(log);
                logger.LogText(loggerEventsArgs);
            }
            catch (Exception ex)
            {

            }

        } 
        #endregion

        #region CrawlingLinkedInPage
        public bool CrawlingLinkedInPage(string Url, ref GlobusHttpHelper HttpHelper)
        {
            //Url = "http://www.linkedin.com/profile/view?id=156004&authType=OUT_OF_NETWORK&authToken=6dZc&locale=en_US&srchid=3817933251417760999809&srchindex=22&srchtotal=1367893&trk=vsrp_people_res_name&trkInfo=VSRPsearchId%3A3817933251417760999809%2CVSRPtargetId%3A156004%2CVSRPcmpt%3Aprimary";

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
            string HeadlineTitle = string.Empty;
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

            string stringSource = HttpHelper.getHtmlfromUrl(new Uri(Url)); 
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
                            int StartIndex = stringSource.IndexOf("<span class=\"full-name\">");
                            string Start = stringSource.Substring(StartIndex).Replace("<span class=\"full-name\">", string.Empty);
                            int EndIndex = Start.IndexOf("</span>");
                            string End = Start.Substring(0, EndIndex).Replace("</span>", string.Empty);
                            strFamilyName = End.Trim();
                        }
                        catch
                        { }
                    }

                    if (string.IsNullOrEmpty(strFamilyName) && stringSource.Contains("<span class=\"full-name\""))
                    {
                        try
                        {
                            int StartIndex = stringSource.IndexOf("<span class=\"full-name\"");
                            string Start = stringSource.Substring(StartIndex).Replace("<span class=\"full-name\"", string.Empty);
                            int EndIndex = Start.IndexOf("</span>");
                            string End = Start.Substring(0, EndIndex).Replace("</span>", string.Empty);
                            strFamilyName = End.Replace("dir=\"auto\">","").Replace("\"","").Trim();
                        }
                        catch
                        { }
                    }


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

            #region Current Company
           try
            {
                int startindex = stringSource.IndexOf("<tr id=\"overview-summary-current\">");
                string start = stringSource.Substring(startindex).Replace("<tr id=\"overview-summary-current\">", string.Empty);
                int endindex = start.IndexOf("<tr id=\"overview-summary-past\">");
                string end = start.Substring(0, endindex).Replace("\u002d", string.Empty);
                string[] finalresult = Regex.Split(end, "\"auto\">");
                finalresult = finalresult.Skip(1).ToArray();

                foreach (var item in finalresult)
                {
                    if (string.IsNullOrEmpty(companycurrent))
                    {
                        try
                        {
                            companycurrent = Regex.Split(item, "</a>")[0].Replace("&amp;", "&").Replace("<strong class=\"highlight\">", string.Empty).Replace("</strong>", string.Empty);
                        }
                        catch { }

                    }
                    else
                    {
                        try
                        {
                            companycurrent = companycurrent + " : " + Regex.Split(item, "</a>")[0].Replace("&amp;", "&").Replace("<strong class=\"highlight\">", string.Empty).Replace("</strong>", string.Empty);
                        }
                        catch { }
                    }
                }

            }
            catch { }
            #endregion


           #region HeadlineTitle
           try
            {
                try
                {
                    try
                    {
                        HeadlineTitle = stringSource.Substring(stringSource.IndexOf("\"memberHeadline"), (stringSource.IndexOf("memberID", stringSource.IndexOf("\"memberHeadline")) - stringSource.IndexOf("\"memberHeadline"))).Replace("\"memberHeadline", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Replace("   ", string.Empty).Replace("visibletrue", string.Empty).Replace("isLNLedtrue", string.Empty).Replace("isPortfoliofalse", string.Empty).Replace("i18n__Location", string.Empty).Replace("Locationi18n__Linkedin_member", string.Empty).Replace("u002d", "-").Replace("LinkedIn Member", string.Empty).Replace("--Location", "--").ToString().Trim();
                        if (HeadlineTitle.Contains("#Name?"))
                        {
                            HeadlineTitle = "--";
                        }
                        if (HeadlineTitle.Contains("i18n"))
                        {
                            HeadlineTitle = Regex.Split(HeadlineTitle, "i18n")[0];
                        }

                    }
                    catch
                    {
                    }

                    if (string.IsNullOrEmpty(HeadlineTitle))
                    {
                        try
                        {
                           HeadlineTitle = stringSource.Substring(stringSource.IndexOf("memberHeadline\":"), (stringSource.IndexOf(",", stringSource.IndexOf("memberHeadline\":")) - stringSource.IndexOf("memberHeadline\":"))).Replace("memberHeadline\":", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Replace("   ", string.Empty).Replace(":", "").Replace("visibletrue", string.Empty).Replace("&dsh;", "").Replace("&amp", "").Replace(";", "").Replace("isLNLedtrue", string.Empty).Replace("isPortfoliofalse", string.Empty).Trim();
                        }
                        catch{ }

                    }

                    if (string.IsNullOrEmpty(HeadlineTitle))
                    {
                        try
                        {
                            HeadlineTitle = stringSource.Substring(stringSource.IndexOf("<p class=\"title\">"), (stringSource.IndexOf("</p></div></div><div class=\"demographic-info adr editable-item\" id=\"demographics\">", stringSource.IndexOf("</p></div></div><div class=\"demographic-info adr editable-item\" id=\"demographics\">")) - stringSource.IndexOf("<p class=\"title\">"))).Replace("<p class=\"title\">", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace("<strong class=highlight>", string.Empty).Replace("</strong>", string.Empty).Trim();
                        }
                        catch { }
                    }

                    string[] strdesigandcompany = new string[4];
                    if (HeadlineTitle.Contains(" at "))
                    {
                        try
                        {
                            strdesigandcompany = Regex.Split(HeadlineTitle, " at ");
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
                            if (string.IsNullOrEmpty(companycurrent))
                            {
                                companycurrent = strdesigandcompany[1];
                            }
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
                        CurrentCompUrl = stringSource.Substring(stringSource.IndexOf("<strong><a href=\"/company"), (stringSource.IndexOf("<strong><a href=\"/company", stringSource.IndexOf("<strong><a href=\"/company")) - stringSource.IndexOf("dir=\"auto\">"))).Replace("<a href=\"", string.Empty).ToString().Trim();
                        CurrentCompUrl = "https://www.linkedin.com" + CurrentCompUrl;
                        CurrentCompUrl = CurrentCompUrl.Split('?')[0].Replace("<strong>", string.Empty).Trim();
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
                if (!stringSource.Contains("company-name") && companylist.Count()>1)
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
                                        Current_Company = end.Replace(",\"specialties_lb\":", string.Empty).Replace("<br>", string.Empty).Replace("\n", string.Empty).Replace("\"", string.Empty).Replace("summary_lb", "Summary").Replace(",", ";").Replace("u002", "-").Replace("&#x2022;",string.Empty).Replace(":", string.Empty);
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
                    Website = stringSource.Substring(stringSource.IndexOf("[{\"URL\":"), (stringSource.IndexOf(",", stringSource.IndexOf("[{\"URL\":")) - stringSource.IndexOf("[{\"URL\":"))).Replace("[{\"URL\":", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace("}", string.Empty).Replace("]",string.Empty).Trim();
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
                            location = lstLocation[lstLocation.Count-1].Trim();
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
                        string end = start.Substring(0, endindex).Replace("\"",string.Empty).Replace("</strong>",string.Empty).Replace("&amp;", "&");
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
                        string end = start.Substring(0, endindex).Replace("&amp;", "&").Replace("}",string.Empty);
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
                    string[] arrayRecommendedName = Regex.Split(PageSource, "fmt__referrerfullName");

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
                                    Name = (start.Substring(0, endIndex).Replace("\"", string.Empty).Replace(":", string.Empty).Replace(",", ";")).Trim();
                                }
                                catch { }

                                try
                                {
                                    int startindex1 = itemRecomName.IndexOf("headline");
                                    string start1 = itemRecomName.Substring(startindex1);
                                    int endIndex1 = start1.IndexOf("memberID");
                                    Heading = (start1.Substring(0, endIndex1).Replace("\"", string.Empty).Replace(":",string.Empty).Replace("headline", string.Empty).Replace(",", string.Empty)).Trim();
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
                            string End = Start.Substring(0,EndIndex).Replace("</a>", string.Empty);
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
                        string _item = Utils.getBetween(item, "name\":", ",").Replace("name\":", "").Replace(":","").Replace("\"","");
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
                                pstTitlesitem = end.Replace(",", ";").Replace("&amp;","&").Replace("\\u002d","-");
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
                                pstDescCompitem = end1.Replace(",", ";").Replace("u002d","-").Replace("<br>", string.Empty).Replace("\\n", string.Empty).Replace("\\",string.Empty).Replace("&#xf0a7;","@").Replace("&#x2019;","'").Replace("&#x2022","@").Replace("&#x25cf;","@");

                                if (pstDescCompitem.Contains("\";\"associatedWith"))
                                {
                                    pstDescCompitem = Regex.Split(pstDescCompitem,"\";\"associatedWith")[0];
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
                GroupStatus.GroupSpecMem.Add(GroupMemId, endName);
                if (firstname == string.Empty) firstname = "LinkedIn";
                if (lastname == string.Empty||lastname==null) lastname = "Member";
                if (HeadlineTitle == string.Empty) HeadlineTitle = "--";
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



                string LDS_FinalData = TypeOfProfile.Replace(",", ";") + "," + LDS_UserProfileLink.Replace(",", ";") + "," + GroupMemId.Replace(",",";") +"," + firstname.Replace(",", ";") + "," + lastname.Replace(",", ";") + "," + HeadlineTitle.Replace(",", ";") + "," + titlecurrent.Replace(",", ";") + "," + companycurrent.Replace(",", ";") + "," + CurrentCompSite.Replace(",", ";") + "," + LDS_BackGround_Summary.Replace(",", ";") + "," + Connection.Replace(",", ";") + "," + recomandation.Replace(",", string.Empty) + "," + Skill.Replace(",", ";") + "," + LDS_Experience.Replace(",", string.Empty) + "," + EducationCollection.Replace(",", ";") + "," + groupscollectin.Replace(",", ";") + "," + USERemail.Replace(",", ";") + "," + LDS_UserContact.Replace(",", ";") + "," + LDS_PastTitles + "," + AllComapny.Replace(",", ";") + "," + location.Replace(",", ";") + "," + country.Replace(",", ";") + "," + Industry.Replace(",", ";") + "," + Website.Replace(",", ";") + "," + LDS_LoginID + "," + AccountType;

                if (!string.IsNullOrEmpty(firstname))
                {
                    //Log("[ " + DateTime.Now + " ] => [ Data : " + LDS_FinalData + " ]");
                }
                else
                {
                    Log("[ " + DateTime.Now + " ] => [ No Data For URL : " + Url + " ]");
                    GlobusFileHelper.AppendStringToTextfileNewLineWithCarat(Url, Globals.DesktopFolder + "\\UnScrapedList.txt");
                }

                if (SearchCriteria.starter)
                {
                    string tempFinalData = LDS_FinalData.Replace(";", "").Replace(LDS_UserProfileLink, "").Replace(TypeOfProfile, "").Replace(",", "").Replace(LDS_LoginID, "").Trim();

                    if (!string.IsNullOrEmpty(tempFinalData))
                    {
                        if (CheckEmployeeScraper)
                        {
                            string FileName = "CompanyEmployeeScraper";
                            AppFileHelper.AddingLinkedInDataToCSVFileCompanyEmployeeScraper(LDS_FinalData, FileName);
                            return true;
                        }
                        else if(CampaignScraper)
                        {
                            AppFileHelper.AddingLinkedInDataToCSVFile(LDS_FinalData, fileName);
                            return true;
                        }
                        else
                        {
                            AppFileHelper.AddingLinkedInDataToCSVFile(LDS_FinalData, SearchCriteria.FileName);
                            return true;
                        }
                    }
                }
            }
            catch { }
            return false;
        } 
        #endregion


        #region CrawlingLinkedInPage
        public bool CrawlingLinkedInPageRecruiter(string Url, ref GlobusHttpHelper HttpHelper)
        {
            
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
            string HeadlineTitle = string.Empty;
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
            string GetDataPageSource = string.Empty;
            string[] arrtemp = Regex.Split(Url, "<:>");
            Url = arrtemp[0];
            string stringSource = HttpHelper.getHtmlfromUrl(new Uri(Url));
            if (string.IsNullOrEmpty(stringSource))
            {
                stringSource = HttpHelper.getHtmlfromUrl(new Uri(Url));
            }
            #endregion

            #region GroupMemId
            try
            {
                string[] gid = Url.Split(',');
                GroupMemId = gid[0].Replace("https://www.linkedin.com/recruiter/profile", string.Empty).Replace("\"","");
            }
            catch { }
            #endregion

            #region Name
            try
            {
                try
                {
                    try
                    {
                        int StartIndex = stringSource.IndexOf("<title>");
                        string Start = stringSource.Substring(StartIndex).Replace("<title>", string.Empty);
                        int EndIndex = Start.IndexOf("| LinkedIn Recruiter</title>");
                        string End = Start.Substring(0, EndIndex).Replace(":", string.Empty).Replace("'", string.Empty).Replace(",", string.Empty).Trim();
                        strFamilyName = End.Trim();
                    }
                    catch
                    { }
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
                        int StartIndex = stringSource.IndexOf("<span class=\"full-name\">");
                        string Start = stringSource.Substring(StartIndex).Replace("<span class=\"full-name\">", string.Empty);
                        int EndIndex = Start.IndexOf("</span>");
                        string End = Start.Substring(0, EndIndex).Replace("</span>", string.Empty);
                        strFamilyName = End.Trim();
                    }
                    catch
                    { }
                }

                if (string.IsNullOrEmpty(strFamilyName) && stringSource.Contains("<span class=\"full-name\""))
                {
                    try
                    {
                        int StartIndex = stringSource.IndexOf("<span class=\"full-name\"");
                        string Start = stringSource.Substring(StartIndex).Replace("<span class=\"full-name\"", string.Empty);
                        int EndIndex = Start.IndexOf("</span>");
                        string End = Start.Substring(0, EndIndex).Replace("</span>", string.Empty);
                        strFamilyName = End.Replace("dir=\"auto\">", "").Replace("\"", "").Trim();
                    }
                    catch
                    { }
                }


                if (string.IsNullOrEmpty(strFamilyName))
                {
                    try
                    {
                        int StartIndex = stringSource.IndexOf("<title>");
                        string Start = stringSource.Substring(StartIndex).Replace("</title>", string.Empty);
                        int EndIndex = Start.IndexOf("| LinkedIn Recruiter</title>");
                        string End = Start.Substring(0, EndIndex).Replace(":", string.Empty).Replace("'", string.Empty).Replace(",", string.Empty).Trim();
                        strFamilyName = End.Trim();
                    }
                    catch
                    { }
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

            #region Current Company
            try
            {
                int startindex = stringSource.IndexOf("<tr id=\"overview-summary-current\">");
                string start = stringSource.Substring(startindex).Replace("<tr id=\"overview-summary-current\">", string.Empty);
                int endindex = start.IndexOf("<tr id=\"overview-summary-past\">");
                string end = start.Substring(0, endindex).Replace("\u002d", string.Empty);
                string[] finalresult = Regex.Split(end, "\"auto\">");
                finalresult = finalresult.Skip(1).ToArray();

                foreach (var item in finalresult)
                {
                    if (string.IsNullOrEmpty(companycurrent))
                    {
                        try
                        {
                            companycurrent = Regex.Split(item, "</a>")[0].Replace("&amp;", "&").Replace("<strong class=\"highlight\">", string.Empty).Replace("</strong>", string.Empty);
                        }
                        catch { }

                    }
                    else
                    {
                        try
                        {
                            companycurrent = companycurrent + " : " + Regex.Split(item, "</a>")[0].Replace("&amp;", "&").Replace("<strong class=\"highlight\">", string.Empty).Replace("</strong>", string.Empty);
                        }
                        catch { }
                    }
                }

            }
            catch { }
            #endregion


            #region HeadlineTitle
            try
            {
                try
                {
                    try
                    {
                        HeadlineTitle = stringSource.Substring(stringSource.IndexOf("\"memberHeadline"), (stringSource.IndexOf("memberID", stringSource.IndexOf("\"memberHeadline")) - stringSource.IndexOf("\"memberHeadline"))).Replace("\"memberHeadline", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Replace("   ", string.Empty).Replace("visibletrue", string.Empty).Replace("isLNLedtrue", string.Empty).Replace("isPortfoliofalse", string.Empty).Replace("i18n__Location", string.Empty).Replace("Locationi18n__Linkedin_member", string.Empty).Replace("u002d", "-").Replace("LinkedIn Member", string.Empty).Replace("--Location", "--").ToString().Trim();
                        if (HeadlineTitle.Contains("#Name?"))
                        {
                            HeadlineTitle = "--";
                        }
                        if (HeadlineTitle.Contains("i18n"))
                        {
                            HeadlineTitle = Regex.Split(HeadlineTitle, "i18n")[0];
                        }

                    }
                    catch
                    {
                    }

                    if (string.IsNullOrEmpty(HeadlineTitle))
                    {
                        try
                        {
                            HeadlineTitle = stringSource.Substring(stringSource.IndexOf("memberHeadline\":"), (stringSource.IndexOf(",", stringSource.IndexOf("memberHeadline\":")) - stringSource.IndexOf("memberHeadline\":"))).Replace("memberHeadline\":", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Replace("   ", string.Empty).Replace(":", "").Replace("visibletrue", string.Empty).Replace("&dsh;", "").Replace("&amp", "").Replace(";", "").Replace("isLNLedtrue", string.Empty).Replace("isPortfoliofalse", string.Empty).Trim();
                        }
                        catch { }

                    }

                    if (string.IsNullOrEmpty(HeadlineTitle))
                    {
                        try
                        {
                            HeadlineTitle = stringSource.Substring(stringSource.IndexOf("<p class=\"title\">"), (stringSource.IndexOf("</p></div></div><div class=\"demographic-info adr editable-item\" id=\"demographics\">", stringSource.IndexOf("</p></div></div><div class=\"demographic-info adr editable-item\" id=\"demographics\">")) - stringSource.IndexOf("<p class=\"title\">"))).Replace("<p class=\"title\">", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace("<strong class=highlight>", string.Empty).Replace("</strong>", string.Empty).Trim();
                        }
                        catch { }
                    }

                    if (string.IsNullOrEmpty(HeadlineTitle))
                    {
                        HeadlineTitle = arrtemp[1];
                    }

                    string[] strdesigandcompany = new string[4];
                    if (HeadlineTitle.Contains(" at "))
                    {
                        try
                        {
                            strdesigandcompany = Regex.Split(HeadlineTitle, " at ");
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
                            if (string.IsNullOrEmpty(companycurrent))
                            {
                                companycurrent = strdesigandcompany[1];
                            }
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
                        CurrentCompUrl = stringSource.Substring(stringSource.IndexOf("<strong><a href=\"/company"), (stringSource.IndexOf("<strong><a href=\"/company", stringSource.IndexOf("<strong><a href=\"/company")) - stringSource.IndexOf("dir=\"auto\">"))).Replace("<a href=\"", string.Empty).ToString().Trim();
                        CurrentCompUrl = "https://www.linkedin.com" + CurrentCompUrl;
                        CurrentCompUrl = CurrentCompUrl.Split('?')[0].Replace("<strong>", string.Empty).Trim();
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
                string[] companylist = Regex.Split(stringSource, "companyName\":");
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
                                Companyname = Utils.getBetween(item,"",",").Replace(">", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Replace("}", string.Empty).Replace("]", string.Empty).Trim();
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

                

                

                

                #region location
                try
                {
                    //location = stringSource.Substring(stringSource.IndexOf("Country\",\"fmt__location\":"), (stringSource.IndexOf("i18n_no_location_matches", stringSource.IndexOf("Country\",\"fmt__location\":")) - stringSource.IndexOf("Country\",\"fmt__location\":"))).Replace("Country\",\"fmt__location\":", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Trim();
                    int startindex = stringSource.IndexOf("location\":");
                    string start = stringSource.Substring(startindex).Replace("location\":", "");
                    int endindex = start.IndexOf(",");
                    string end = start.Substring(0, endindex).Replace("\u002d", string.Empty).Replace("\"","").Replace("","").Trim();
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
                if (!string.IsNullOrEmpty(location))
                {
                    country = location;
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

                    string PageSource = Utils.getBetween(stringSource, "<h2 class=\"title\">Recommendations</h2>", "/ul></li></ul></div>");
                    string[] arrayRecommendedName = Regex.Split(PageSource, "target=\"_blank\">");

                    arrayRecommendedName = arrayRecommendedName.Skip(1).ToArray();
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

                                    Name = Utils.getBetween(itemRecomName, "", "</a>");
                                    Name = Name.Replace("\"", string.Empty).Replace(":", string.Empty).Replace(",", ";").Trim();
                                }
                                catch { }

                                try
                                {
                                    Heading = Utils.getBetween(itemRecomName, "<h5 class=\"searchable\">", "</h5>");
                                    Heading = Heading.Replace(":", string.Empty).Replace("headline", string.Empty).Replace(",", string.Empty).Trim();
                                }
                                catch { }

                               
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

                    string PageSource = Utils.getBetween(stringSource,"<h2 class=\"title\">Groups</h2>","</li></ul></div>");

                    string[] array1 = Regex.Split(PageSource, "target=\"_blank\">");
                    List<string> ListGroupName = new List<string>();
                    string SelItem = string.Empty;

                    foreach (var itemGrps in array1)
                    {
                        try
                        {
                            if (itemGrps.Contains("</a>") && !itemGrps.Contains("<!DOCTYPE html")&&!itemGrps.StartsWith("<img")) //">Join
                            {
                                //if (itemGrps.IndexOf("?gid=") == 0)
                                {
                                    try
                                    {
                                        string _grpname = Utils.getBetween(itemGrps, "", "</a>");
                                    _grpname = _grpname.Replace("\"", string.Empty).Replace(":", string.Empty).Replace(",", ";").Replace("\"", string.Empty).Replace("amp", string.Empty).Replace("&", string.Empty).Replace(";", string.Empty).Replace("csrfToken", string.Empty).Replace("name:", string.Empty).Trim();
                                        ListGroupName.Add(_grpname);
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
                    string skillSource = Utils.getBetween(stringSource, "skills\":[", "],");
                    string[] strarr_skill = Regex.Split(skillSource, ",");
                    string[] strarr_skill1 = Regex.Split(stringSource, "fmt__skill_name\"");
                    if (strarr_skill.Count() >= 1)
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
                                        string Grp = item.Replace(">", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Replace("\"u002", "-").Trim();
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

                if (!string.IsNullOrEmpty(location))
                {
                    country = location;
                }

                string endName = firstname + " " + lastname;
                GroupStatus.GroupSpecMem.Add(GroupMemId, endName);
                if (firstname == string.Empty) firstname = "LinkedIn";
                if (lastname == string.Empty || lastname == null) lastname = "Member";
                if (HeadlineTitle == string.Empty) HeadlineTitle = "--";
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
                if (Website == string.Empty||Website==null) Website = "--";

                if (!string.IsNullOrEmpty(location))
                {
                    country = location;
                }

                string LDS_FinalData = TypeOfProfile.Replace(",", ";") + "," + LDS_UserProfileLink.Replace(",", ";") + "," + GroupMemId.Replace(",", ";") + "," + firstname.Replace(",", ";") + "," + lastname.Replace(",", ";") + "," + HeadlineTitle.Replace(",", ";") + "," + titlecurrent.Replace(",", ";") + "," + companycurrent.Replace(",", ";") + "," + CurrentCompSite.Replace(",", ";") + "," + LDS_BackGround_Summary.Replace(",", ";") + "," + Connection.Replace(",", ";") + "," + recomandation.Replace(",", string.Empty) + "," + Skill.Replace(",", ";") + "," + LDS_Experience.Replace(",", string.Empty) + "," + EducationCollection.Replace(",", ";") + "," + groupscollectin.Replace(",", ";") + "," + USERemail.Replace(",", ";") + "," + LDS_UserContact.Replace(",", ";") + "," + LDS_PastTitles + "," + AllComapny.Replace(",", ";") + "," + location.Replace(",", ";") + "," + country.Replace(",", ";") + "," + Industry.Replace(",", ";") + "," + Website.Replace(",", ";") + "," + LDS_LoginID + "," + AccountType;

                if (!string.IsNullOrEmpty(firstname))
                {
                    //Log("[ " + DateTime.Now + " ] => [ Data : " + LDS_FinalData + " ]");
                }
                else
                {
                    Log("[ " + DateTime.Now + " ] => [ No Data For URL : " + Url + " ]");
                    GlobusFileHelper.AppendStringToTextfileNewLineWithCarat(Url, Globals.DesktopFolder + "\\UnScrapedList.txt");
                }

                if (SearchCriteria.starter)
                {
                    string tempFinalData = LDS_FinalData.Replace(";", "").Replace(LDS_UserProfileLink, "").Replace(TypeOfProfile, "").Replace(",", "").Replace(LDS_LoginID, "").Trim();

                    if (!string.IsNullOrEmpty(tempFinalData))
                    {
                        if (CheckEmployeeScraper)
                        {
                            string FileName = "CompanyEmployeeScraper";
                            AppFileHelper.AddingLinkedInDataToCSVFileCompanyEmployeeScraper(LDS_FinalData, FileName);
                            return true;
                        }
                        else if (CampaignScraper)
                        {
                            AppFileHelper.AddingLinkedInDataToCSVFile(LDS_FinalData, fileName);
                            return true;
                        }
                        else
                        {
                            AppFileHelper.AddingLinkedInDataToCSVFile(LDS_FinalData, SearchCriteria.FileName);
                            return true;
                        }
                    }
                }
            }
            catch { }
            return false;
        }
        #endregion
      
        #region SearchByKeyword
        private void SearchByKeyword(ref GlobusHttpHelper HttpHelper)
        {
            try
            {                
                if (SearchCriteria.starter)
                {
                    #region Serch
                    //For Data postURL
                    string pageSourceaAdvanceSearch = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/search"));

                    try
                    {
                        csrfToken1 = pageSourceaAdvanceSearch.Substring(pageSourceaAdvanceSearch.IndexOf("csrfToken"), 200);
                        if (csrfToken1.Contains("&amp"))
                        {
                            try
                            {
                                string[] arrcsrfToken1 = Regex.Split(csrfToken1, "&amp");

                                foreach (string item in arrcsrfToken1)
                                {
                                    try
                                    {
                                        if (!string.IsNullOrEmpty(item))
                                        {
                                            try
                                            {
                                                csrfToken1 = item.Replace("csrfToken=", string.Empty).Replace("\"", string.Empty).Trim();
                                            
                                                break;
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
                            catch
                            {
                            }
                        }
                        else
                        {
                            string[] Arr1 = csrfToken.Split('"');
                            csrfToken1 = Arr1[0];
                            csrfToken1 = csrfToken.Replace(":", "%3A");
                        }
                    }
                    catch
                    {
                    }

                    NewSearchPage = string.Empty;
                    if (csrfToken1 == string.Empty)
                    {
                        string pageSourceaAdvanceSearch1 = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/search"));
                        csrfToken1 = pageSourceaAdvanceSearch.Substring(pageSourceaAdvanceSearch1.IndexOf("csrfToken"), 200);
                        if (csrfToken1.Contains(">"))
                        {
                            csrfToken1 = csrfToken1.Replace("\n", string.Empty).Replace("csrfToken=", string.Empty).Replace("\"", string.Empty).Trim();
                            csrfToken1 = csrfToken1.Split('>')[0];
                            NewSearchPage = "NewPage";
                        }
                    }

                    string PplsearchOrigin = string.Empty;
                    try
                    {
                        PplsearchOrigin = pageSourceaAdvanceSearch.Substring(pageSourceaAdvanceSearch.IndexOf("pplSearchOrigin"), 200);
                    }
                    catch { }

                    try
                    {
                        string[] Arr2 = PplsearchOrigin.Split('"');
                        PplsearchOrigin = Arr2[4];
                        PplsearchOrigin = PplsearchOrigin.Replace(":", "%3A");
                    }
                    catch { }

                        string KeywordAdvPage = string.Empty;

                        if (SearchCriteria.Relationship == string.Empty || SearchCriteria.Relationship == "N")
                        {
                            KeywordAdvPage = "http://www.linkedin.com/vsearch/p?keywords=" + Uri.EscapeDataString(SearchCriteria.Keyword) + "&firstName=" + SearchCriteria.FirstName + "&lastName=" + SearchCriteria.LastName + "&company=" + SearchCriteria.Company + "&countryCode=" + SearchCriteria.Country + "&companyScope=" + SearchCriteria.CompanyValue + "&title=" + SearchCriteria.Title + "&openAdvancedForm=true&locationType=" + SearchCriteria.Location + "&countryCode=" + SearchCriteria.Country + "&f_I=" + SearchCriteria.IndustryType + "&f_L=" + SearchCriteria.language + "&postalCode=" + SearchCriteria.PostalCode + "&distance=" + SearchCriteria.within + "&orig=ADVS";
                                                        
                        }
                        else
                        {
                            //KeywordAdvPage = "http://www.linkedin.com/vsearch/p?keywords=" + Uri.EscapeDataString(SearchCriteria.Keyword) + "&firstName=" + SearchCriteria.FirstName + "&lastName=" + SearchCriteria.LastName + "&company=" + SearchCriteria.Company + "&countryCode=" + SearchCriteria.Country + "&companyScope=" + SearchCriteria.CompanyValue + "&title=" + SearchCriteria.Title + "&titleScope=CP&openAdvancedForm=true&locationType=" + SearchCriteria.Location + "&countryCode=" + SearchCriteria.Country + "&f_N=" + SearchCriteria.Relationship + "&f_I=" + SearchCriteria.IndustryType + "&f_L=" + SearchCriteria.language + "&postalCode=" + SearchCriteria.PostalCode + "&distance=" + SearchCriteria.within + "&orig=ADVS";
                            KeywordAdvPage = "http://www.linkedin.com/vsearch/p?firstName=" + SearchCriteria.FirstName + "&lastName=" + SearchCriteria.LastName + "&postalCode=" + SearchCriteria.PostalCode + "&openAdvancedForm=true&companyScope=" + SearchCriteria.CompanyValue + "&locationType=" + SearchCriteria.Location + "&countryCode=" + SearchCriteria.Country + "&company=" + SearchCriteria.Company + "&distance=" + SearchCriteria.within + "&f_N=" + SearchCriteria.Relationship + "&f_G=" + SearchCriteria.LocationArea + "&f_I=" + SearchCriteria.IndustryType + "&f_TE=" + SearchCriteria.YearOfExperience + "&f_FA=" + SearchCriteria.Function + "&f_SE=" + SearchCriteria.SeniorLevel + "&f_P=" + SearchCriteria.InerestedIn + "&f_CS=" + SearchCriteria.CompanySize + "&f_F=" + SearchCriteria.Fortune1000 + "&f_DR=" + SearchCriteria.RecentlyJoined + "&orig=MDYS";
                            
                        }


                        try
                        {
                            ResponseWallPostForPremiumAcc = HttpHelper.getHtmlfromUrl1(new Uri(KeywordAdvPage));
                        }
                        catch { }
                  

                    string facetsOrder = string.Empty;

                    if (PostResponce.Contains("facetsOrder"))
                    {
                        try
                        {
                            facetsOrder = ResponseWallPostForPremiumAcc.Substring(PostResponce.IndexOf("facetsOrder"), 200);
                            string[] Arr3 = facetsOrder.Split('"');
                            facetsOrder = Arr3[2];
                            string DecodedCharTest = Uri.UnescapeDataString(facetsOrder);
                            string DecodedEmail = Uri.EscapeDataString(facetsOrder);
                            facetsOrder = DecodedEmail;
                        }
                        catch
                        {
                        }
                    }
                    #endregion
                }

                int pagenumber = 0;
                string strPageNumber = string.Empty;

                //if (NewSearchPage == string.Empty)
                //{
                    string[] Arr12 = Regex.Split(ResponseWallPostForPremiumAcc, "<li");
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

                    if (string.IsNullOrEmpty(strPageNumber))
                    {
                        try
                        {

                            if (ResponseWallPostForPremiumAcc.Contains("results_count"))
                            {
                                string[] countResultArr = Regex.Split(ResponseWallPostForPremiumAcc, "results_count");

                                if (countResultArr.Length > 1)
                                {
                                    string tempResult = countResultArr[1].Substring(0, countResultArr[1].IndexOf("results for"));

                                    #region Commented
                                    //Regex IdCheck = new Regex("^[0-9]*$");

                                    //string[] tempResultArr = Regex.Split(tempResult, "[^0-9]");

                                    //foreach (string item in tempResultArr)
                                    //{
                                    //    try
                                    //    {
                                    //        if(IdCheck.IsMatch(item))
                                    //        {
                                    //            strPageNumber = item;

                                    //            break;
                                    //        }
                                    //    }
                                    //    catch (Exception ex)
                                    //    {
                                    //    }
                                    //} 
                                    #endregion

                                    if (tempResult.Contains("<strong>"))
                                    {
                                        strPageNumber = tempResult.Substring(tempResult.IndexOf("<strong>"), tempResult.IndexOf("</strong>", tempResult.IndexOf("<strong>")) - tempResult.IndexOf("<strong>")).Replace("<strong>", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Trim();
                                    }
                                }
                            }
                        }
                        catch (Exception)
                        {

                        }
                    }
                    else
                    {
                                            
                    }

                    try
                    {
                        if (strPageNumber != string.Empty)
                        {
                            Log("[ " + DateTime.Now + " ] => [ " +  "No Of Results Found : " + strPageNumber + " ]");
                            pagenumber = Convert.ToInt32(strPageNumber);
                        }
                    }
                    catch (Exception)
                    {

                    }

                    try
                    {
                        pagenumber = (pagenumber / 10) + 1;
                    }
                    catch { }

                    string GetResponce = string.Empty;
                    string GetRequestURL = string.Empty;
                    RecordURL.Clear();
                    queRecordUrl.Clear();
                    List<string> lstids = new List<string>();
                    if (pagenumber > 1)
                    {
                        for (int i = 1; i <= pagenumber; i++)
                        {
                            try
                            {
                                if (SearchCriteria.starter)
                                {
                                    #region loop

                                    //string pageUrl = "http://www.linkedin.com/search/fpsearch?keywords=" + SearchCriteria.Keyword + "&title=" + SearchCriteria.Title + "&searchLocationType=" + SearchCriteria.Location + "&countryCode=" + SearchCriteria.Country + "&page_num=" + i + "";
                                    string pageUrl = string.Empty;

                                    if (SearchCriteria.Relationship == string.Empty || SearchCriteria.Relationship == "N")
                                    {
                                        pageUrl = "http://www.linkedin.com/vsearch/p?keywords=" + Uri.EscapeDataString(SearchCriteria.Keyword) + "&firstName=" + SearchCriteria.FirstName + "&lastName=" + SearchCriteria.LastName + "&title=" + SearchCriteria.Title + "&openAdvancedForm=true&locationType=" + SearchCriteria.Location + "&countryCode=" + SearchCriteria.Country + "&f_I=" + SearchCriteria.IndustryType + "&f_L=" + SearchCriteria.language + "&postalCode=" + SearchCriteria.PostalCode + "&distance=" + SearchCriteria.within + "&orig=ADVS&page_num=" + i + "";
                                    }
                                    else
                                    {
                                        pageUrl = "http://www.linkedin.com/vsearch/p?keywords=" + Uri.EscapeDataString(SearchCriteria.Keyword) + "&firstName=" + SearchCriteria.FirstName + "&lastName=" + SearchCriteria.LastName + "&title=" + SearchCriteria.Title + "&openAdvancedForm=true&locationType=" + SearchCriteria.Location + "&countryCode=" + SearchCriteria.Country + "&f_N=" + SearchCriteria.Relationship + "&f_I=" + SearchCriteria.IndustryType + "&f_L=" + SearchCriteria.language + "&postalCode=" + SearchCriteria.PostalCode + "&distance=" + SearchCriteria.within + "&orig=ADVS&page_num=" + i + "";
                                    }
                                                    
                                    string pageSource = HttpHelper.getHtmlfromUrl1(new Uri(pageUrl));

                                    if (pageSource.Contains("Upgrade your account to see more results"))
                                    {
                                       break;
                                    }

                                    if (!string.IsNullOrEmpty(pageSource))
                                    {
                                        try
                                        {
                                            List<string> lstProfileURLs = ChilkatBasedRegex.GettingAllUrls1(pageSource, "/profile/view?id");
                                            
                                            foreach (string itemlist in lstProfileURLs)
                                            {
                                              RecordURL.Add(itemlist);
                                              if (!queRecordUrl.Contains(itemlist))
                                              {
                                                  queRecordUrl.Enqueue(itemlist);
                                              }
                                            }
                                            RecordURL = RecordURL.Distinct().ToList();
                                           
                                        }
                                        catch
                                        {
                                        }
                                    }
                                    else
                                    {
                                        if (pageSource.Contains("Upgrade your account to see more results"))
                                        {
                                            Log("[ " + DateTime.Now + " ] => [ Upgrade your account to see more results ]");
                                            break;
                                        }
                                    }
                                  
                                    #endregion
                                }
                            }
                            catch
                            {
                            }
                        }
                    }

                    #region For Else
                    //else
                    //{

                    //    if (SearchCriteria.starter)
                    //    {

                    //        #region loop
                    //        if (ResponseWallPostForPremiumAcc.Contains("/profile/view?id"))
                    //        {

                    //            List<string> PageSerchUrl = ChilkatBasedRegex.GettingAllUrls(ResponseWallPostForPremiumAcc, "profile/view?id");
                    //            if (PageSerchUrl.Count == 0)
                    //            {

                    //                Log("On the basis of your Account or Your Input you can able to see " + RecordURL.Count + "  Results");

                    //            }
                    //            foreach (string item in PageSerchUrl)
                    //            {
                    //                try
                    //                {
                    //                    if (SearchCriteria.starter)
                    //                    {
                    //                        if (item.Contains("pp_profile_name_link"))
                    //                        {


                    //                            string urlSerch = "http://www.linkedin.com" + item;

                    //                            Log(urlSerch);
                    //                            RecordURL.Add(urlSerch);
                    //                        }
                    //                    }
                    //                }
                    //                catch
                    //                {
                    //                }
                    //            }
                    //        }
                    //        #endregion
                    //    }
                    //}
                //}
                //else
                //{
                    if (string.IsNullOrEmpty(strPageNumber))
                    {
                        try
                        {

                            if (ResponseWallPostForPremiumAcc.Contains("results_count_with_keywords") || ResponseWallPostForPremiumAcc.Contains("results_count_without_keywords"))
                            {
                                string[] countResultArr = new string[] { };

                                if (ResponseWallPostForPremiumAcc.Contains("results_count_with_keywords"))
                                {
                                    countResultArr = Regex.Split(ResponseWallPostForPremiumAcc, "results_count_with_keywords");
                                }

                                if (ResponseWallPostForPremiumAcc.Contains("results_count_without_keywords"))
                                {
                                    countResultArr = Regex.Split(ResponseWallPostForPremiumAcc, "results_count_without_keywords");
                                }

                                if (countResultArr.Length > 1)
                                {
                                    string tempResult = countResultArr[1].Substring(0, countResultArr[1].IndexOf("</strong>"));


                                    #region Commented
                                    //Regex IdCheck = new Regex("^[0-9]*$");

                                    //string[] tempResultArr = Regex.Split(tempResult, "[^0-9]");

                                    //foreach (string item in tempResultArr)
                                    //{
                                    //    try
                                    //    {
                                    //        if(IdCheck.IsMatch(item))
                                    //        {
                                    //            strPageNumber = item;

                                    //            break;
                                    //        }
                                    //    }
                                    //    catch (Exception ex)
                                    //    {
                                    //    }
                                    //} 
                                    #endregion

                                    if (tempResult.Contains("<strong>"))
                                    {
                                        string aa = tempResult.Split('>')[1].Replace(",", string.Empty);
                                        strPageNumber = aa;
                                    }
                                }
                            }
                        }
                        catch (Exception)
                        {

                        }

                        if (SearchCriteria.starter)
                        {
                            int totPage = 0;
                            Log("[ " + DateTime.Now + " ] => [ No Of Results Found : " + strPageNumber + " ]");

                            try
                            {
                                totPage = Convert.ToInt32(strPageNumber);
                                totPage = totPage / 10 + 1;
                            }
                            catch { }

                            for (int i = 1; i <= totPage; i++)
                            {
                                string KeywordAdvPage = string.Empty;
                                if (SearchCriteria.Relationship == string.Empty)
                                {
                                    //KeywordAdvPage = "http://www.linkedin.com/vsearch/p?keywords=" + SearchCriteria.Keyword + "&title= " + SearchCriteria.Title + "&openAdvancedForm=true&locationType=" + SearchCriteria.Location + "&countryCode=" + SearchCriteria.Country + "&f_I=" + SearchCriteria.IndustryType + "&f_L=" + SearchCriteria.language + "&page_num=" + i + "&orig=ADVS";
                                    KeywordAdvPage = "http://www.linkedin.com/vsearch/p?keywords=" + Uri.EscapeDataString(SearchCriteria.Keyword) + "&firstName=" + SearchCriteria.FirstName + "&lastName=" + SearchCriteria.LastName + "&title=" + SearchCriteria.Title + "&openAdvancedForm=true&locationType=" + SearchCriteria.Location + "&countryCode=" + SearchCriteria.Country + "&f_I=" + SearchCriteria.IndustryType + "&f_L=" + SearchCriteria.language + "&postalCode=" + SearchCriteria.PostalCode + "&distance=" + SearchCriteria.within + "&page_num=" + i + "&orig=ADVS";

                                }
                                else
                                {
                                    KeywordAdvPage = "http://www.linkedin.com/vsearch/p?keywords=" + Uri.EscapeDataString(SearchCriteria.Keyword) + "&title=" + SearchCriteria.Title + "&openAdvancedForm=true&locationType=" + SearchCriteria.Location + "&countryCode=" + SearchCriteria.Country + "&f_N=" + SearchCriteria.Relationship + "&f_I=" + SearchCriteria.IndustryType + "&f_L=" + SearchCriteria.language + "&page_num=" + i + "&orig=ADVS";
                                    //KeywordAdvPage = "http://www.linkedin.com/vsearch/p?keywords=" + SearchCriteria.Keyword + "&firstName=" + SearchCriteria.FirstName + "&lastName=" + SearchCriteria.LastName + "&title= " + SearchCriteria.Title + "&openAdvancedForm=true&locationType=" + SearchCriteria.Location + "&countryCode=" + SearchCriteria.Country + "&f_N=" + SearchCriteria.Relationship + "&f_I=" + SearchCriteria.IndustryType + "&f_L=" + SearchCriteria.language + "&postalCode=" + SearchCriteria.PostalCode + "&distance=" + SearchCriteria.within + "&page_num=" + i + "&orig=ADVS";
                                }


                                ResponseWallPostForPremiumAcc = HttpHelper.getHtmlfromUrl1(new Uri(KeywordAdvPage));


                                if (ResponseWallPostForPremiumAcc.Contains("Upgrade your account to see more results"))
                                {
                                    break;
                                }

                                #region loop
                                if (ResponseWallPostForPremiumAcc.Contains("pageURL") || (ResponseWallPostForPremiumAcc.Contains("/profile/view?id")))
                                {
                                    //List<string> PageSerchUrl = new List<string>();
                                    // string[] PageSerchUrl = Regex.Split(ResponseWallPostForPremiumAcc, "pageURL");

                                    List<string> PageSerchUrl = ChilkatBasedRegex.GettingAllUrls1(ResponseWallPostForPremiumAcc, "/profile/view?");

                                    if (PageSerchUrl.Count == 0)
                                    {
                                        PageSerchUrl = ChilkatBasedRegex.GettingAllUrls1(ResponseWallPostForPremiumAcc, "/profile/view?");
                                    }

                                    //    List<string> PageSerchUrl = ChilkatBasedRegex.GettingAllUrls1(PgSrcMain1, "/profile/view?id");

                                    //if (PageSerchUrl.Count == 0)
                                    //{

                                    //    Log("On the basis of your Account or Your Input you can able to see " + RecordURL.Count + "  Results");

                                    //}
                                    foreach (string item in PageSerchUrl)
                                    {
                                        try
                                        {
                                            if (SearchCriteria.starter)
                                            {
                                                if (item.Contains("vsearch/p?keywords") && !item.Contains("content") || (item.Contains("/profile/view?id")))
                                                {
                                                    try
                                                    {
                                                        //string[] aa = item.Split('"');

                                                        // string urlSerch = "http://www.linkedin.com" + aa[2];
                                                        //string urlSerch = Regex.Split(item, "&srchid")[0];

                                                        if (item.Contains("view?id"))
                                                        {
                                                            RecordURL.Add(item);
                                                            if (!queRecordUrl.Contains(item))
                                                            {
                                                                queRecordUrl.Enqueue(item);
                                                            }
                                                        }

                                                        Log("[ " + DateTime.Now + " ] => [ " + item + " ]" );

                                                        //string pageSource = HttpHelper.getHtmlfromUrl(new Uri(urlSerch));

                                                        //string[] PageFinalUrl = Regex.Split(pageSource, "link_nprofile_view_");

                                                        //foreach (var itemGrps in PageFinalUrl)
                                                        //{
                                                        //    try
                                                        //    {
                                                        //        if (itemGrps.Contains(":\"") && !itemGrps.Contains("<!DOCTYPE html"))
                                                        //        {

                                                        //            try
                                                        //            {

                                                        //                int startindex = itemGrps.IndexOf(":\"");
                                                        //                string start = itemGrps.Substring(startindex);
                                                        //                int endIndex = start.IndexOf(",");
                                                        //                string FinalUrl = "http://www.linkedin.com" + start.Substring(0, endIndex).Replace("\"", string.Empty).Replace(":", string.Empty).Replace("amp", string.Empty).Replace(";", string.Empty);
                                                        //                FinalUrl = Regex.Split(FinalUrl, "&")[0];

                                                        //if (FinalUrl.Contains("view?id"))
                                                        //{
                                                        //    RecordURL.Add(FinalUrl);
                                                        //}


                                                        //            //Log(FinalUrl);
                                                        //        }
                                                        //        catch { }

                                                        //    }
                                                        //}
                                                        //catch { }
                                                        // }


                                                        //RecordURL.Add(urlSerch);
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


                            }
                                #endregion
                        }
                    }
                //}
                    #endregion
           
                RecordURL = RecordURL.Distinct().ToList();
        
                {

                }
                if (SearchCriteria.starter)
                {
                    finalUrlCollection(ref HttpHelper);
                    //new Thread(() =>
                    //{
                    //    test(ref HttpHelper);
                    //}).Start();
                }
            }
            catch
            {
            }
        } 
        #endregion

        #region GettingAllUrls
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

                        if (hreflink.Contains(MustMatchString) && hreflink.Contains("&authType="))
                        {
                            if (hreflink.Contains("http://www.linkedin.com"))
                            {
                                suburllist.Add(hreflink);
                                Log("[ " + DateTime.Now + " ] => [ URL >>> " + hreflink + " ]");
                            }
                            else
                            {
                                suburllist.Add("http://www.linkedin.com" + hreflink);
                                Log("[ " + DateTime.Now + " ] => [ URL >>> http://www.linkedin.com" + hreflink + " ]");
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
 
    }
}