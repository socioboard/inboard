using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseLib;
using System.Text.RegularExpressions;
using Chilkat;


namespace ProfileManager
{
    public class ProfileRank
    {
        #region globaldeclaration

        public static Events loggerrank = new Events();
        string userName = string.Empty;
        string password = string.Empty;

        string proxyAddress = string.Empty;
        string proxyPort = string.Empty;
        string proxyUsername = string.Empty;
        string proxyPassword = string.Empty;
        #endregion

        public ProfileRank(string UserName, string Password, string ProxyAddress, string ProxyPort, string ProxyUserName, string ProxyPassword)
        {
            userName = UserName;
            password = Password;
            proxyAddress = ProxyAddress;
            proxyPort = ProxyPort;
            proxyUsername = ProxyUserName;  
            proxyPassword = ProxyPassword;
        }
        public void LinkedInProfilerank(ref GlobusHttpHelper HttpHelper)
        {      
                      
            
            #region PROFILE RANK
            try
            {
                string pagesource = string.Empty;
                string selfImageUrl = string.Empty;
                string rankUrl = "https://www.linkedin.com/wvmx/profile/rankings?trk=wvmp_how_you_rank_entry";
                string selfDetailsUrl = "https://www.linkedin.com/wvmx/profile/rankings/group?client=desktop-HYR";
                string percentileCompany = string.Empty;
                string percentileConnection = string.Empty;
                string rankCompany = string.Empty;
                string rankConnection = string.Empty;
                string mostviewedPgsource = string.Empty;
                string selfID = string.Empty;
                string selfDetails = string.Empty;
                string selfName = string.Empty;
                string[] selfDetailsArr = new string[] { };
                string memberdetails = string.Empty;                
                Log("[ " + DateTime.Now + " ] => [ Fetching your rank for profile views..]");
                pagesource = HttpHelper.getHtmlfromUrl1(new Uri(selfDetailsUrl));
                
                try
                {
                    int startindex = pagesource.IndexOf("fmt_fullName\":\"");
                    string start = pagesource.Substring(startindex).Replace("fmt_fullName\":\"", string.Empty);
                    int endindex = start.IndexOf("\",\"");
                    string end = start.Substring(0, endindex).Replace("\",\"", string.Empty);
                    selfName = end.Trim();
                    Log("[ " + DateTime.Now + " ] => [ Profile Name : " + selfName + " ]");
                }
                catch(Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> PROFILE RANK -  LinkedInProfilerank()  --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                    GlobusFileHelper.AppendStringToTextfileNewLine("Error --> PROFILE RANK -  LinkedInProfilerank() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + " || DateTime :- " + DateTime.Now, Globals.Path_LinkedInProfileRankErrorLogs);
                }
                try
                {
                    int startindex = pagesource.IndexOf("media_picture_link\":\"");
                    string start = pagesource.Substring(startindex).Replace("media_picture_link\":\"", string.Empty);
                    int endindex = start.IndexOf("\"}");
                    string end = start.Substring(0, endindex).Replace("\"}", string.Empty);
                    selfImageUrl = end.Trim();
                    Log("[ " + DateTime.Now + " ] => [ Profile Image URL : " + selfImageUrl + " ]");
                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> PROFILE RANK -  LinkedInProfilerank()  --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                    GlobusFileHelper.AppendStringToTextfileNewLine("Error --> PROFILE RANK -  LinkedInProfilerank() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + " || DateTime :- " + DateTime.Now, Globals.Path_LinkedInProfileRankErrorLogs);
                }
                try
                {
                    int startindex = pagesource.IndexOf("prof_url\":\"");
                    string start = pagesource.Substring(startindex).Replace("prof_url\":\"", string.Empty);
                    int startindex1 = start.IndexOf("id");
                    string start1 = start.Substring(startindex1).Replace("id", string.Empty);
                    int endindex = start1.IndexOf("authType");
                    string end = start1.Substring(0, endindex).Replace("authType", string.Empty).Replace("&", string.Empty).Replace("\"", string.Empty).Replace("=", string.Empty);
                    selfID = end.Trim();
                    Log("[ " + DateTime.Now + " ] => [ Profile ID : " + selfID + " ]");
                    selfID = "M" + selfID;
                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> PROFILE RANK -  LinkedInProfilerank()  --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                    GlobusFileHelper.AppendStringToTextfileNewLine("Error --> PROFILE RANK -  LinkedInProfilerank() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + " || DateTime :- " + DateTime.Now, Globals.Path_LinkedInProfileRankErrorLogs);
                }
                try
                {
                    string[] percentileArr = Regex.Split(pagesource, "i18n_hero_headline_connections_positive\":\"");
                    string percentileCmp = percentileArr[1];
                    string percentileCnnc = percentileArr[2];
                    try
                    {
                        //int startindex = percentileCmp.IndexOf("");
                        //string start = percentileCmp.Substring(startindex);
                        //int endindex = start.IndexOf("\",\"");
                        //string end = start.Substring(0, endindex).Replace("\", \"", string.Empty).Replace("<em class='percentile'>", string.Empty).Replace("</em>", string.Empty);
                        //percentileCompany = end.Trim();

                        //int startindex1 = pagesource.IndexOf("i18n_hero_headline_company_positive\":\"");
                        //string start1 = pagesource.Substring(startindex1).Replace("i18n_hero_headline_company_positive\":\"", string.Empty);
                        //int endindex1 = start1.IndexOf("\",\"");
                        //string end1 = start1.Substring(0, endindex1).Replace("\", \"", string.Empty).Replace("<em class='percentile'>", string.Empty).Replace("</em>", string.Empty);
                        //percentileCompany = end1.Trim();
                        //Log("[ " + DateTime.Now + " ] => [ " + percentileCompany + " ]");

                        int startindex = pagesource.IndexOf("groupType\":\"CONNECTION\",\"");
                        string start = pagesource.Substring(startindex).Replace("i18n_hero_headline_company_positive\":\"", string.Empty);
                        int startindex1 = start.IndexOf("i18n_hero_headline_connections_positive\":\"");
                        string start1 = start.Substring(startindex1).Replace("i18n_hero_headline_connections_positive\":\"", string.Empty);
                        int endindex = start1.IndexOf("\",\"");
                        string end = start1.Substring(0, endindex).Replace("\",\"", string.Empty).Replace("<em class='percentile'>", string.Empty).Replace("</em>", string.Empty);
                        percentileConnection = end.Trim();
                        Log("[ " + DateTime.Now + " ] => [ " + percentileConnection + " ]");


                        int startindex2 = start.IndexOf("<span class='first-char'>");
                        string start2 = start.Substring(startindex2).Replace("<span class='first-char'>", string.Empty);
                        int endindex2 = start2.IndexOf("\",\"");
                        string end2 = start2.Substring(0, endindex2).Replace("\",\"", string.Empty).Replace("</span>", string.Empty).Replace("<em class='percentile'>", string.Empty).Replace("</em>", string.Empty).Replace("</button>", string.Empty);
                        rankConnection = end2.Trim();
                        Log("[ " + DateTime.Now + " ] => [ Profile view rank in connection : " + rankConnection + " ]");

                    }
                    catch (Exception ex)
                    {
                        GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> PROFILE RANK -  LinkedInProfilerank()  --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                        GlobusFileHelper.AppendStringToTextfileNewLine("Error --> PROFILE RANK -  LinkedInProfilerank() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + " || DateTime :- " + DateTime.Now, Globals.Path_LinkedInProfileRankErrorLogs);
                    }
                    try
                    {
                        //int startindex = percentileCnnc.IndexOf("");
                        //string start = percentileCnnc.Substring(startindex);
                        //int endindex = start.IndexOf("\",\"");
                        //string end = start.Substring(0, endindex).Replace("\", \"", string.Empty).Replace("<em class='percentile'>", string.Empty).Replace("</em>", string.Empty);
                        //percentileConnection = end.Trim();
                        //Log("[ " + DateTime.Now + " ] => [ " + percentileConnection + " ]");

                        int startindex = pagesource.IndexOf("groupType\":\"COMPANY\",\"");
                        string start = pagesource.Substring(startindex).Replace("groupType\":\"COMPANY\",\"", string.Empty);
                        int startindex1 = start.IndexOf("i18n_hero_headline_company_positive\":\"");
                        string start1 = start.Substring(startindex1).Replace("i18n_hero_headline_company_positive\":\"", string.Empty);
                        int endindex = start1.IndexOf("\",\"");
                        string end = start1.Substring(0, endindex).Replace("\",\"", string.Empty).Replace("<em class='percentile'>", string.Empty).Replace("</em>", string.Empty);
                        percentileCompany = end.Trim();
                        Log("[ " + DateTime.Now + " ] => [ " + percentileCompany + " ]");


                        int startindex2 = start.IndexOf("<span class='first-char'>");
                        string start2 = start.Substring(startindex2).Replace("<span class='first-char'>", string.Empty);
                        int endindex2 = start2.IndexOf("\",\"");
                        string end2 = start2.Substring(0, endindex2).Replace("\",\"", string.Empty).Replace("</span>", string.Empty).Replace("<em class='percentile'>", string.Empty).Replace("</em>", string.Empty).Replace("</button>", string.Empty);
                        rankCompany = end2.Trim();
                        Log("[ " + DateTime.Now + " ] => [ Profile view rank in company : " + rankCompany + " ]");
                    }
                    catch (Exception ex)
                    {
                        GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> PROFILE RANK -  LinkedInProfilerank()  --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                        GlobusFileHelper.AppendStringToTextfileNewLine("Error --> PROFILE RANK -  LinkedInProfilerank() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + " || DateTime :- " + DateTime.Now, Globals.Path_LinkedInProfileRankErrorLogs);
                    }
                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> PROFILE RANK -  LinkedInProfilerank()  --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                    GlobusFileHelper.AppendStringToTextfileNewLine("Error --> PROFILE RANK -  LinkedInProfilerank() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + " || DateTime :- " + DateTime.Now, Globals.Path_LinkedInProfileRankErrorLogs);
                }
                
                selfDetails = selfName + " ; " + selfID + " ; " + selfImageUrl + " ; " + percentileCompany + " ; " + rankCompany + " ; " + percentileConnection + " ; " + rankConnection;
                selfDetailsArr = selfDetails.Split(';');
                #region PROFILE STATS
                try
                {



                    string prfViewOccupation =      string.Empty;
                    string prfViewOccupationValue = string.Empty;
                    string prfViewSource =          string.Empty;
                    string prfViewSourceValue =     string.Empty;
                    string prfViewCompany =         string.Empty;
                    string prfViewCompanyValue =    string.Empty;
                    string prfViewIndustryValue =   string.Empty;
                    string prfViewIndustry =        string.Empty;
                    string prfViewSearch =          string.Empty;
                    string prfViewSearchValue =     string.Empty;
                    string prfViewDate =            string.Empty;
                    string prfViewDateValue =       string.Empty;
                    string prfViewRegion =          string.Empty;
                    string prfViewRegionValue =     string.Empty;
                    string prfViewSearchKeyword =   string.Empty;
                    string viewOccupation =         string.Empty;
                    string viewSource =             string.Empty;
                    string viewCompany =            string.Empty;
                    string viewIndustry =           string.Empty;
                    string viewSearch =             string.Empty;
                    string viewDate =               string.Empty;
                    string viewRegion =             string.Empty;
                    string totalViewOccupation =    string.Empty;
                    string totalViewSource =        string.Empty;
                    string totalViewCompany =       string.Empty;
                    string totalViewIndustry =      string.Empty;
                    string totalViewSearch =        string.Empty;
                    string totalViewDate =          string.Empty;
                    string totalViewRegion =        string.Empty;
                    string profileViewStatsDetails = string.Empty;
                    int countOccupation = 0;
                    int countCompany = 0;
                    int countRegion = 0;
                    int countSearch = 0;
                    int countSource = 0;
                    int countIndustry = 0;
                    List<string> listOccupation = new List<string>();
                    List<string> listSource = new List<string>();
                    List<string> listCompany = new List<string>();
                    List<string> listIndustry = new List<string>();
                    List<string> listSearch = new List<string>();
                    List<string> listDate = new List<string>();
                    List<string> listRegion = new List<string>();
                    string[] arrOccupation = new string[] { };
                    string[] arrSource = new string[] { };
                    string[] arrCompany = new string[] { };
                    string[] arrIndustry = new string[] { };
                    string[] arrSearch = new string[] { };
                    string[] arrRegion = new string[] { };
                    string[] profileViewStatsDetail = new string[] { };
                    List<List<string>> listProfileStatsDetails = new List<List<string>>();
                    string profileviews = "https://www.linkedin.com/wvmx/profile?trk=how_you_rank_wvmp";
                    string viewPageSource = HttpHelper.getHtmlfromUrl1(new Uri(profileviews));
                    string[] profileviewCategories = Regex.Split(viewPageSource, "wvmx_profile_view_by_");
                    foreach (string item in profileviewCategories)
                    {
                        try
                        {
                            if (!item.Contains("<!DOCTYPE html>"))
                            {
                                if ((item.Contains("occupation\":{")) && (!(countOccupation > 0)))
                                {
                                                                       
                                    Log("[ " + DateTime.Now + " ] => [------OCCUPATION OF PROFILE VIEWERS------]");
                                    
                                    string[] itemSplit = Regex.Split(item, "\"i18n_key\":\"");
                                    foreach (string itemDetail in itemSplit)
                                    {
                                        try
                                        {
                                            if (!itemDetail.Contains("totalCount"))
                                            {
                                                try
                                                {

                                                    int startindex = itemDetail.IndexOf("");
                                                    string start = itemDetail.Substring(startindex).Replace("\"i18n_key\":\"", string.Empty);
                                                    int endindex = start.IndexOf("\",\"");
                                                    string end = start.Substring(0, endindex).Replace("\",\"", string.Empty);
                                                    prfViewOccupation = end.Trim();
                                                    if (prfViewOccupation == "SUPERTITLE.NONE")
                                                        prfViewOccupation = "Unknown";
                                                    //prfViewOccupation += "--" + prfViewOccupation; 

                                                    int startindex1 = itemDetail.IndexOf("i18n_value\":\"");
                                                    string start1 = itemDetail.Substring(startindex1).Replace("i18n_value\":\"", string.Empty);
                                                    int endindex1 = start1.IndexOf("\",\"");
                                                    if (endindex1 < 0)
                                                    {
                                                        endindex1 = start1.IndexOf("\","); 
                                                    }
                                                    string end1 = start1.Substring(0, endindex1).Replace("\",\"", string.Empty);
                                                    prfViewOccupationValue = end1.Trim();
                                                    //prfViewOccupationValue += "--" + prfViewOccupationValue; 
                                                    if (viewOccupation == string.Empty)
                                                    {
                                                        viewOccupation = prfViewOccupationValue + " profile viewers work as a " + prfViewOccupation;
                                                    }
                                                    else
                                                    {
                                                        viewOccupation += " :: " + prfViewOccupationValue + " profile viewers work as a " + prfViewOccupation;
                                                    }
                                                    Log("[ " + DateTime.Now + " ] => [ " + prfViewOccupationValue + " profile viewers work as a  : " + prfViewOccupation + " ]");
                                                    listOccupation.Add(viewOccupation);
                                                }
                                                catch (Exception ex)
                                                {
                                                    GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> PROFILE RANK -  LinkedInProfilerank()  --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                                    GlobusFileHelper.AppendStringToTextfileNewLine("Error --> PROFILE RANK -  LinkedInProfilerank() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + " || DateTime :- " + DateTime.Now, Globals.Path_LinkedInProfileRankErrorLogs);
                                                }
                                                
                                            }
                                            if(itemDetail.Contains("totalCount"))
                                            {
                                                try
                                                {
                                                    if (string.IsNullOrEmpty(totalViewOccupation))
                                                    {
                                                        int startindex = itemDetail.IndexOf("totalCount\":");
                                                        string start = itemDetail.Substring(startindex).Replace("totalCount\":", string.Empty);
                                                        int endindex = start.IndexOf(",");
                                                        string end = start.Substring(0, endindex).Replace(",", string.Empty).Replace("\"", string.Empty);
                                                        totalViewOccupation = end.Trim();
                                                        Log("[ " + DateTime.Now + " ] => [Total number of profile viewers under occupation : " + totalViewOccupation + " ]");
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> PROFILE RANK -  LinkedInProfilerank()  --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                                    GlobusFileHelper.AppendStringToTextfileNewLine("Error --> PROFILE RANK -  LinkedInProfilerank() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + " || DateTime :- " + DateTime.Now, Globals.Path_LinkedInProfileRankErrorLogs);
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> PROFILE RANK -  LinkedInProfilerank()  --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                            GlobusFileHelper.AppendStringToTextfileNewLine("Error --> PROFILE RANK -  LinkedInProfilerank() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + " || DateTime :- " + DateTime.Now, Globals.Path_LinkedInProfileRankErrorLogs);
                                        }
                                    }
                                    countOccupation++;
                                }
                                if ((item.Contains("source\":{"))&&(!(countSource > 0)))
                                {
                                    Log("[ " + DateTime.Now + " ] => [------SOURCE PROFILE VIEWERS USED TO VIEW YOUR PROFILE------]");
                                    
                                    string[] itemSplit = Regex.Split(item, "\"i18n_key\":\"");
                                    foreach (string itemDetail in itemSplit)
                                    {
                                        try
                                        {
                                            if (!itemDetail.Contains("totalCount"))
                                            {
                                                try
                                                {
                                                    int startindex = itemDetail.IndexOf("");
                                                    string start = itemDetail.Substring(startindex).Replace("\"i18n_key\":\"", string.Empty);
                                                    int endindex = start.IndexOf("\",\"");
                                                    string end = start.Substring(0, endindex).Replace("\",\"", string.Empty);
                                                    prfViewSource = end.Trim();
                                                    if (prfViewSource == "NONE")
                                                        prfViewSource = "Unknown";

                                                    //NONE","value":21,"key":"NONE","fmt_value":"21"},{"i18n_value":"6",
                                                    int startindex1 = itemDetail.IndexOf("fmt_value");
                                                    string start1 = itemDetail.Substring(startindex1).Replace("fmt_value", string.Empty);
                                                    int endindex1 = start1.IndexOf("}");
                                                    if (endindex1 < 0)
                                                    {
                                                        endindex1 = start1.IndexOf("\",");
                                                    }
                                                    string end1 = start1.Substring(0, endindex1).Replace("\"", string.Empty).Replace(":",string.Empty);
                                                    prfViewSourceValue = end1.Trim();
                                                    if (viewSource == string.Empty)
                                                    {
                                                        viewSource = prfViewSourceValue + " profile viewers came from " + prfViewSource;
                                                    }
                                                    else
                                                    {
                                                        viewSource += " : " + prfViewSourceValue + " profile viewers came from " + prfViewSource;
                                                    }
                                                    Log("[ " + DateTime.Now + " ] => [ " + prfViewSourceValue + " profile viewers came from " + prfViewSource + " ]");
                                                    listSource.Add(viewSource);
                                                }
                                                catch (Exception ex)
                                                {
                                                    GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> PROFILE RANK -  LinkedInProfilerank()  --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                                    GlobusFileHelper.AppendStringToTextfileNewLine("Error --> PROFILE RANK -  LinkedInProfilerank() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + " || DateTime :- " + DateTime.Now, Globals.Path_LinkedInProfileRankErrorLogs);
                                                }
                                                
                                            }
                                            if (itemDetail.Contains("totalCount"))
                                            {
                                                try
                                                {
                                                    if (string.IsNullOrEmpty(totalViewSource))
                                                    {
                                                        int startindex = itemDetail.IndexOf("totalCount\":");
                                                        string start = itemDetail.Substring(startindex).Replace("totalCount\":", string.Empty);
                                                        int endindex = start.IndexOf(",");
                                                        string end = start.Substring(0, endindex).Replace(",", string.Empty).Replace("\"",string.Empty);
                                                        totalViewSource = end.Trim();
                                                        Log("[ " + DateTime.Now + " ] => [Total number of profile viewers under source : " + totalViewSource + " ]");
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> PROFILE RANK -  LinkedInProfilerank()  --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                                    GlobusFileHelper.AppendStringToTextfileNewLine("Error --> PROFILE RANK -  LinkedInProfilerank() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + " || DateTime :- " + DateTime.Now, Globals.Path_LinkedInProfileRankErrorLogs);
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> PROFILE RANK -  LinkedInProfilerank()  --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                            GlobusFileHelper.AppendStringToTextfileNewLine("Error --> PROFILE RANK -  LinkedInProfilerank() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + " || DateTime :- " + DateTime.Now, Globals.Path_LinkedInProfileRankErrorLogs);
                                        }
                                    }
                                    countSource++;
                                }
                                if ((item.Contains("company\":{"))&&(!(countCompany > 0)))
                                {
                                    Log("[ " + DateTime.Now + " ] => [------COMPANY WHERE YOUR PROFILE VIEWERS WORK------]");
                                   
                                    string[] itemSplit = Regex.Split(item, "\"i18n_key\":\"");
                                    foreach (string itemDetail in itemSplit)
                                    {
                                        try
                                        {
                                            if (!itemDetail.Contains("totalCount"))
                                            {
                                                try
                                                {
                                                    int startindex = itemDetail.IndexOf("");
                                                    string start = itemDetail.Substring(startindex).Replace("\"i18n_key\":\"", string.Empty);
                                                    int endindex = start.IndexOf("\",\"");
                                                    string end = start.Substring(0, endindex).Replace("\",\"", string.Empty);
                                                    prfViewCompany = end.Trim();
                                                    if (prfViewCompany == "NONE")
                                                        prfViewCompany = "Unknown";

                                                    int startindex1 = itemDetail.IndexOf("i18n_value\":\"");
                                                    string start1 = itemDetail.Substring(startindex1).Replace("i18n_value\":\"", string.Empty);
                                                    int endindex1 = start1.IndexOf("\",\"");
                                                    if (endindex1 < 0)
                                                    {
                                                        endindex1 = start1.IndexOf("\",");
                                                    }
                                                    string end1 = start1.Substring(0, endindex1).Replace("\",\"", string.Empty);
                                                    prfViewCompanyValue = end1.Trim();
                                                    if (viewCompany == string.Empty)
                                                    {
                                                        viewCompany = prfViewCompanyValue + " profile viewers work at " + prfViewCompany;
                                                    }
                                                    else
                                                    {
                                                        viewCompany += " : " + prfViewCompanyValue + " profile viewers work at " + prfViewCompany;
                                                    }
                                                    Log("[ " + DateTime.Now + " ] => [ " + prfViewCompanyValue + " profile viewers work at " + prfViewCompany + " ]");
                                                    listCompany.Add(viewCompany);
                                                }
                                                catch (Exception ex)
                                                {
                                                    GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> PROFILE RANK -  LinkedInProfilerank()  --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                                    GlobusFileHelper.AppendStringToTextfileNewLine("Error --> PROFILE RANK -  LinkedInProfilerank() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + " || DateTime :- " + DateTime.Now, Globals.Path_LinkedInProfileRankErrorLogs);
                                                }
                                            }
                                            if (itemDetail.Contains("totalCount"))
                                            {
                                                try
                                                {
                                                    if (string.IsNullOrEmpty(totalViewCompany))
                                                    {
                                                        int startindex = itemDetail.IndexOf("totalCount\":");
                                                        string start = itemDetail.Substring(startindex).Replace("totalCount\":", string.Empty);
                                                        int endindex = start.IndexOf(",");
                                                        string end = start.Substring(0, endindex).Replace(",", string.Empty).Replace("\"", string.Empty);
                                                        totalViewCompany = end.Trim();
                                                        Log("[ " + DateTime.Now + " ] => [Total number of profile viewers under company : " + totalViewCompany + " ]");
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> PROFILE RANK -  LinkedInProfilerank()  --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                                    GlobusFileHelper.AppendStringToTextfileNewLine("Error --> PROFILE RANK -  LinkedInProfilerank() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + " || DateTime :- " + DateTime.Now, Globals.Path_LinkedInProfileRankErrorLogs);
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> PROFILE RANK -  LinkedInProfilerank()  --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                            GlobusFileHelper.AppendStringToTextfileNewLine("Error --> PROFILE RANK -  LinkedInProfilerank() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + " || DateTime :- " + DateTime.Now, Globals.Path_LinkedInProfileRankErrorLogs);
                                        }
                                    }
                                    countCompany++;
                                }
                                if ((item.Contains("industry\":{"))&&(!(countIndustry > 0)))
                                {
                                    Log("[ " + DateTime.Now + " ] => [------INDUSTRY IN WHICH YOUR PROFILE VIEWERS ARE------]");
                                    
                                    string[] itemSplit = Regex.Split(item, "\"i18n_key\":\"");
                                    foreach (string itemDetail in itemSplit)
                                    {
                                        try
                                        {
                                            if (!itemDetail.Contains("totalCount"))
                                            {
                                                try
                                                {
                                                    int startindex = itemDetail.IndexOf("");
                                                    string start = itemDetail.Substring(startindex).Replace("\"i18n_key\":\"", string.Empty);
                                                    int endindex = start.IndexOf("\",\"");
                                                    string end = start.Substring(0, endindex).Replace("\",\"", string.Empty);
                                                    prfViewIndustry = end.Trim();
                                                    if (prfViewIndustry == "NONE")
                                                        prfViewIndustry = "Unknown";

                                                    int startindex1 = itemDetail.IndexOf("i18n_value\":\"");
                                                    string start1 = itemDetail.Substring(startindex1).Replace("i18n_value\":\"", string.Empty);
                                                    int endindex1 = start1.IndexOf("\",\"");
                                                    if (endindex1 < 0)
                                                    {
                                                        endindex1 = start1.IndexOf("\",");
                                                    }
                                                    string end1 = start1.Substring(0, endindex1).Replace("\",\"", string.Empty);
                                                    prfViewIndustryValue = end1.Trim();
                                                    if (viewIndustry == string.Empty)
                                                    {
                                                        viewIndustry = prfViewIndustryValue + " profile viewers work in " + prfViewIndustry + " industry";
                                                    }
                                                    else
                                                    {
                                                        viewIndustry +=  " : " + prfViewIndustryValue + " profile viewers work in " + prfViewIndustry + " industry";
                                                    }
                                                    Log("[ " + DateTime.Now + " ] => [ " + prfViewIndustryValue + " profile viewers work in " + prfViewIndustry + " industry ]");
                                                    listIndustry.Add(viewIndustry);
                                                }
                                                catch (Exception ex)
                                                {
                                                    GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> PROFILE RANK -  LinkedInProfilerank()  --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                                    GlobusFileHelper.AppendStringToTextfileNewLine("Error --> PROFILE RANK -  LinkedInProfilerank() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + " || DateTime :- " + DateTime.Now, Globals.Path_LinkedInProfileRankErrorLogs);
                                                }
                                            }
                                            if (itemDetail.Contains("totalCount"))
                                            {
                                                try
                                                {
                                                    if (string.IsNullOrEmpty(totalViewIndustry))
                                                    {
                                                        int startindex = itemDetail.IndexOf("totalCount\":");
                                                        string start = itemDetail.Substring(startindex).Replace("totalCount\":", string.Empty);
                                                        int endindex = start.IndexOf(",");
                                                        string end = start.Substring(0, endindex).Replace(",", string.Empty).Replace("\"", string.Empty);
                                                        totalViewIndustry = end.Trim();
                                                        Log("[ " + DateTime.Now + " ] => [Total number of profile viewers under industry : " + totalViewIndustry + " ]");
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> PROFILE RANK -  LinkedInProfilerank()  --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                                    GlobusFileHelper.AppendStringToTextfileNewLine("Error --> PROFILE RANK -  LinkedInProfilerank() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + " || DateTime :- " + DateTime.Now, Globals.Path_LinkedInProfileRankErrorLogs);
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> PROFILE RANK -  LinkedInProfilerank()  --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                            GlobusFileHelper.AppendStringToTextfileNewLine("Error --> PROFILE RANK -  LinkedInProfilerank() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + " || DateTime :- " + DateTime.Now, Globals.Path_LinkedInProfileRankErrorLogs);
                                        }
                                    }
                                    countIndustry++;
                                }
                                if ((item.Contains("search\":{"))&&(!(countSearch > 0)))
                                {
                                    Log("[ " + DateTime.Now + " ] => [------KEYWORDS YOUR PROFILE VIEWERS USED TO GET TO YOUR PROFILE------]");
                                    
                                    string[] itemSplit = Regex.Split(item, "\"i18n_key\":\"");
                                    if (itemSplit.Count() < 2)
                                    {
                                        totalViewSearch = "0";
                                        Log("[ " + DateTime.Now + " ] => [Total number of profile viewers under search : " + totalViewSearch + " ]");
                                        break;                                        
                                    }

                                    foreach (string itemDetail in itemSplit)
                                    {
                                        try
                                        {
                                            if (!itemDetail.Contains("totalCount"))
                                            {
                                                try
                                                {
                                                    int startindex = itemDetail.IndexOf("");
                                                    string start = itemDetail.Substring(startindex).Replace("\"i18n_key\":\"", string.Empty);
                                                    int endindex = start.IndexOf("\",\"");
                                                    string end = start.Substring(0, endindex).Replace("\",\"", string.Empty);
                                                    prfViewSearch = end.Trim();
                                                    if (prfViewSearch == "NONE")
                                                        prfViewSearch = "Unknown";
                                                    //commented by vicky on 10/10
                                                    //int startindex1 = itemDetail.IndexOf("i18n_value\":\"");
                                                    //string start1 = itemDetail.Substring(startindex1).Replace("i18n_value\":\"", string.Empty);
                                                    //int endindex1 = start1.IndexOf("\",\"");
                                                    //string end1 = start1.Substring(0, endindex1).Replace("\",\"", string.Empty);

                                                    int startindex1 = itemDetail.IndexOf("value\":\"");
                                                    string start1 = itemDetail.Substring(startindex1).Replace("value\":\"", string.Empty);
                                                    int endindex1 = start1.IndexOf("\",\"");
                                                    string end1 = start1.Substring(0, endindex1).Replace("\",\"", string.Empty);
                                                    if (end1.Contains("i18n_2"))
                                                    {
                                                        endindex1 = start1.IndexOf("\"},");
                                                        end1 = start1.Substring(0, endindex1).Replace("\",\"", string.Empty).Replace("\"},",string.Empty);
                                                    }
                                                    prfViewSearchValue = end1.Trim();

                                                    int startindex2 = itemDetail.IndexOf("searchKeywords\":[");
                                                    string start2 = itemDetail.Substring(startindex2).Replace("searchKeywords\":[", string.Empty);
                                                    int endindex2 = start2.IndexOf("],");
                                                    string end2 = start2.Substring(0, endindex2).Replace("],", string.Empty);
                                                    prfViewSearchKeyword = end1.Trim();
                                                    if (viewSearch == string.Empty)
                                                    {
                                                        viewSearch = prfViewSearchValue + " profile viewers used " + prfViewSearch + " to view your profile ";
                                                    }
                                                    else
                                                    {
                                                        viewSearch += " : " + prfViewSearchValue + " profile viewers used " + prfViewSearch + " to view your profile " + " : ";
                                                    }
                                                    Log("[ " + DateTime.Now + " ] => [ " + prfViewSearchValue + " profile viewers used " + prfViewSearch + " to view your profile ]");
                                                    listSearch.Add(viewSearch);
                                                }
                                                catch (Exception ex)
                                                {
                                                    GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> PROFILE RANK -  LinkedInProfilerank()  --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                                    GlobusFileHelper.AppendStringToTextfileNewLine("Error --> PROFILE RANK -  LinkedInProfilerank() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + " || DateTime :- " + DateTime.Now, Globals.Path_LinkedInProfileRankErrorLogs);
                                                }
                                            }
                                            if (itemDetail.Contains("totalCount"))
                                            {
                                                try
                                                {
                                                    if (string.IsNullOrEmpty(totalViewSearch))
                                                    {
                                                        int startindex = itemDetail.IndexOf("totalCount\":");
                                                        string start = itemDetail.Substring(startindex).Replace("totalCount\":", string.Empty);
                                                        int endindex = start.IndexOf(",");
                                                        string end = start.Substring(0, endindex).Replace(",", string.Empty).Replace("\"", string.Empty);
                                                        totalViewSearch = end.Trim();
                                                        Log("[ " + DateTime.Now + " ] => [Total number of profile viewers under search : " + totalViewSearch + " ]");
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> PROFILE RANK -  LinkedInProfilerank()  --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                                    GlobusFileHelper.AppendStringToTextfileNewLine("Error --> PROFILE RANK -  LinkedInProfilerank() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + " || DateTime :- " + DateTime.Now, Globals.Path_LinkedInProfileRankErrorLogs);
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> PROFILE RANK -  LinkedInProfilerank()  --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                            GlobusFileHelper.AppendStringToTextfileNewLine("Error --> PROFILE RANK -  LinkedInProfilerank() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + " || DateTime :- " + DateTime.Now, Globals.Path_LinkedInProfileRankErrorLogs);
                                        }
                                    }
                                    countSearch++;
                                }
                               


                                if ((item.Contains("region\":{"))&&(!(countRegion > 0)))
                                {
                                    Log("[ " + DateTime.Now + " ] => [------AREA IN WHICH YOUR PROFILE VIEWERS LIVE------]");
                                    
                                    string[] itemSplit = Regex.Split(item, "\"i18n_key\":\"");
                                    foreach (string itemDetail in itemSplit)
                                    {
                                        try
                                        {
                                            if (!itemDetail.Contains("totalCount"))
                                            {
                                                try
                                                {
                                                    int startindex = itemDetail.IndexOf("");
                                                    string start = itemDetail.Substring(startindex).Replace("\"i18n_key\":\"", string.Empty);
                                                    int endindex = start.IndexOf("\",\"");
                                                    string end = start.Substring(0, endindex).Replace("\",\"", string.Empty);
                                                    prfViewRegion = end.Trim();
                                                    if (prfViewRegion == "NONE")
                                                        prfViewRegion = "Unknown";
                                                    #region commented
                                                    //commented by vicky on 10/10
                                                    //int startindex1 = itemDetail.IndexOf("i18n_value\":\"");
                                                    //string start1 = itemDetail.Substring(startindex1).Replace("i18n_value\":\"", string.Empty);
                                                    //int endindex1 = start1.IndexOf("\",\"");
                                                    //string end1 = start1.Substring(0, endindex1).Replace("\",\"", string.Empty); 
                                                    #endregion

                                                    int startindex1 = itemDetail.IndexOf("value\":\"");
                                                    string start1 = itemDetail.Substring(startindex1).Replace("value\":\"", string.Empty);
                                                    int endindex1 = start1.IndexOf("\"}");
                                                    string end1 = start1.Substring(0, endindex1).Replace("\",\"", string.Empty).Replace("\"","");
                                                    prfViewRegionValue = end1.Trim();
                                                    if (viewRegion == string.Empty)
                                                    {
                                                        viewRegion = prfViewRegionValue + " profile viewers live in " + prfViewRegion;
                                                    }
                                                    else
                                                    {
                                                        viewRegion +=  " : " + prfViewRegionValue + " profile viewers live in " + prfViewRegion;
                                                    }
                                                    Log("[ " + DateTime.Now + " ] => [ " + prfViewRegionValue + " profile viewers live in " + prfViewRegion + " ]");
                                                    listRegion.Add(viewRegion);
                                                }
                                                catch (Exception ex)
                                                {
                                                    GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> PROFILE RANK -  LinkedInProfilerank()  --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                                    GlobusFileHelper.AppendStringToTextfileNewLine("Error --> PROFILE RANK -  LinkedInProfilerank() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + " || DateTime :- " + DateTime.Now, Globals.Path_LinkedInProfileRankErrorLogs);
                                                }
                                            }
                                            if (itemDetail.Contains("totalCount"))
                                            {
                                                try
                                                {
                                                    if (string.IsNullOrEmpty(totalViewRegion))
                                                    {
                                                        int startindex = itemDetail.IndexOf("totalCount\":");
                                                        string start = itemDetail.Substring(startindex).Replace("totalCount\":", string.Empty);
                                                        int endindex = start.IndexOf(",");
                                                        string end = start.Substring(0, endindex).Replace(",", string.Empty).Replace("\"",string.Empty);
                                                        totalViewRegion = end.Trim();
                                                        Log("[ " + DateTime.Now + " ] => [Total number of profile viewers under region : " + totalViewRegion + " ]");
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> PROFILE RANK -  LinkedInProfilerank()  --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                                    GlobusFileHelper.AppendStringToTextfileNewLine("Error --> PROFILE RANK -  LinkedInProfilerank() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + " || DateTime :- " + DateTime.Now, Globals.Path_LinkedInProfileRankErrorLogs);
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> PROFILE RANK -  LinkedInProfilerank()  --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                            GlobusFileHelper.AppendStringToTextfileNewLine("Error --> PROFILE RANK -  LinkedInProfilerank() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + " || DateTime :- " + DateTime.Now, Globals.Path_LinkedInProfileRankErrorLogs);
                                        }
                                    }
                                    countRegion++;
                                }
                                
                                
                            }
                        }
                        catch (Exception ex)
                        {
                            GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> PROFILE RANK -  LinkedInProfilerank()  --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                            GlobusFileHelper.AppendStringToTextfileNewLine("Error --> PROFILE RANK -  LinkedInProfilerank() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + " || DateTime :- " + DateTime.Now, Globals.Path_LinkedInProfileRankErrorLogs);
                        }
                }
                        profileViewStatsDetails = viewOccupation + viewCompany + viewIndustry + viewRegion + viewSearch + viewSource;
                        try
                        {
                            //string[] arr = Regex.Split(profileViewStatsDetails, ":");
                            //foreach (string iten in arr)
                            {
                                AppFileHelper.AddingLinkedInDataToCSVFile(profileViewStatsDetails, Globals.profilestatsCsv);
                                string CSVHeader = "userName" + "," + "OccupationStats" + "," + "CompanyStats" + "," + "IndustryStats" + "," + "RegionStats" + "," + "SearchStats" + "," + "SourceStats" + "," + "Date";
                                string CSV_Content = userName.Replace(",", ";") + "," + viewOccupation.Replace(",", ";") + "," + viewCompany.Replace(",", ";") + "," + viewIndustry.Replace(",", ";") + "," + viewRegion.Replace(",", ";") + "," + viewSearch.Replace(",", ";") + "," + viewSource.Replace(",", ";") + "," + DateTime.Now;
                                CSVUtilities.ExportDataCSVFile(CSVHeader, CSV_Content, Globals.profilestatsCsv);
                            }

                        }
                        catch (Exception ex)
                        {
                            GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> PROFILE RANK -  LinkedInProfilerank()  --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                            GlobusFileHelper.AppendStringToTextfileNewLine("Error --> PROFILE RANK -  LinkedInProfilerank() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + " || DateTime :- " + DateTime.Now, Globals.Path_LinkedInProfileRankErrorLogs);
                        }                    
                    
                    arrOccupation = listOccupation.ToArray();
                    arrSource = listSource.ToArray();
                    arrCompany = listCompany.ToArray();
                    arrRegion = listRegion.ToArray();
                    arrSearch = listSearch.ToArray();
                    arrIndustry = listIndustry.ToArray();
                                        
                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> PROFILE RANK -  LinkedInProfilerank()  --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                    GlobusFileHelper.AppendStringToTextfileNewLine("Error --> PROFILE RANK -  LinkedInProfilerank() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + " || DateTime :- " + DateTime.Now, Globals.Path_LinkedInProfileRankErrorLogs);
                }
                #endregion PROFILE STATS

                #region MEMBER DETAILS
                try
                {
                    string mostviewedUrl = "https://www.linkedin.com/wvmx/profile/rankings/page?g=" + selfID + "&sa=1&count=10";

                    List<string> membernames = new List<string>();
                    List<string> memberheadlines = new List<string>();
                    List<string> listMemberdetails = new List<string>();

                    mostviewedPgsource = HttpHelper.getHtmlfromUrl1(new Uri(mostviewedUrl));
                    string[] members = Regex.Split(mostviewedPgsource, "i18n_connect_with_p0\":\"");
                    string name = string.Empty;
                    string headline = string.Empty;
                    string connection = string.Empty;
                    string imgUrl = string.Empty;
                    string[] memberdetailsArr = new string[] { };
                    Log("[ " + DateTime.Now + " ] => [ Fetching profile rank list of your connections in descending order ]");
                    foreach (string item in members)
                    {
                        if (!(item.Contains("HYRMembers")))
                        {
                            try
                            {
                                int startindex = item.IndexOf("");
                                string start = item.Substring(startindex);
                                int endindex = start.IndexOf("\",\"");
                                string end = start.Substring(0, endindex).Replace("\",\"", string.Empty).Replace("Connect with ", string.Empty);
                                name = end.Trim();
                            }
                            catch (Exception ex)
                            {
                                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> PROFILE RANK -  LinkedInProfilerank()  --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                GlobusFileHelper.AppendStringToTextfileNewLine("Error --> PROFILE RANK -  LinkedInProfilerank() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + " || DateTime :- " + DateTime.Now, Globals.Path_LinkedInProfileRankErrorLogs);
                            }
                            membernames.Add(name);
                            membernames = membernames.Distinct().ToList();
                            try
                            {
                                int startindex = item.IndexOf("headline\":\"");
                                string start = item.Substring(startindex).Replace("headline\":\"", string.Empty);
                                int endindex = start.IndexOf("\",\"");
                                string end = start.Substring(0, endindex).Replace("\",\"", string.Empty).Replace("Connect with ", string.Empty);
                                headline = end.Trim();
                            }
                            catch (Exception ex)
                            {
                                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> PROFILE RANK -  LinkedInProfilerank()  --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                GlobusFileHelper.AppendStringToTextfileNewLine("Error --> PROFILE RANK -  LinkedInProfilerank() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + " || DateTime :- " + DateTime.Now, Globals.Path_LinkedInProfileRankErrorLogs);
                            }
                            memberheadlines.Add(name);
                            memberheadlines = memberheadlines.Distinct().ToList();
                            try
                            {
                                int startindex = item.IndexOf("__NAME_is_your_connection\":\"");
                                string start = item.Substring(startindex).Replace("__NAME_is_your_connection\":\"", string.Empty);
                                int endindex = start.IndexOf("\",\"");
                                string end = start.Substring(0, endindex).Replace("\",\"", string.Empty).Replace("Connect with ", string.Empty);
                                connection = end.Trim();
                                if (connection.Contains("your connection"))
                                {
                                    connection = "1st connection";
                                }
                                if (connection.Contains("2nd connection"))
                                {
                                    connection = "2nd connection";
                                }
                                if (connection.Contains("3rd connection"))
                                {
                                    connection = "3rd connection";
                                }
                            }
                            catch (Exception ex)
                            {
                                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> PROFILE RANK -  LinkedInProfilerank()  --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                GlobusFileHelper.AppendStringToTextfileNewLine("Error --> PROFILE RANK -  LinkedInProfilerank() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + " || DateTime :- " + DateTime.Now, Globals.Path_LinkedInProfileRankErrorLogs);
                            }
                            try
                            {
                                int startindex = item.IndexOf("pictureId\":\"");
                                string start = item.Substring(startindex).Replace("pictureId\":\"", string.Empty);
                                int endindex = start.IndexOf("\",\"");
                                string end = start.Substring(0, endindex).Replace("\",\"", string.Empty).Replace("Connect with ", string.Empty);
                                imgUrl = "https://media.licdn.com/mpr/mpr/shrink_100_100" + end.Trim();
                            }
                            catch (Exception ex)
                            {
                                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> PROFILE RANK -  LinkedInProfilerank()  --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                GlobusFileHelper.AppendStringToTextfileNewLine("Error --> PROFILE RANK -  LinkedInProfilerank() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + " || DateTime :- " + DateTime.Now, Globals.Path_LinkedInProfileRankErrorLogs);
                            }
                            memberdetails = name + " ; " + headline + " ; " + connection + " ; " + imgUrl;
                            listMemberdetails.Add(memberdetails);
                            listMemberdetails = listMemberdetails.Distinct().ToList();
                            try
                            {
                                AppFileHelper.AddingLinkedInDataToCSVFile(memberdetails, Globals.selfRankCsv);
                                string CSVHeader = "userName" + "," + "MemberName" + "," + "MemberHeadline" + "," + "MemberDegreeOfConnection" + "," + "MemberImageURL" + "," + "Date";
                                string CSV_Content = userName.Replace(",", ";") + "," + name.Replace(",", ";") + "," + headline.Replace(",", ";") + "," + connection.Replace(",", ";") + "," + imgUrl.Replace(",", ";") + "," + DateTime.Now;
                                CSVUtilities.ExportDataCSVFile(CSVHeader, CSV_Content, Globals.memberRankCsv);
                                
                            }
                            catch (Exception ex)
                            {
                                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> PROFILE RANK -  LinkedInProfilerank()  --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                GlobusFileHelper.AppendStringToTextfileNewLine("Error --> PROFILE RANK -  LinkedInProfilerank() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + " || DateTime :- " + DateTime.Now, Globals.Path_LinkedInProfileRankErrorLogs);
                            }
                        }

                    }
                    
                    int memberrank = 1;
                    foreach (string item in membernames)
                    {
                        Log("[ " + DateTime.Now + " ] => [ #" + memberrank + " " + item + " ]");
                            memberrank++;
                    }
                    memberdetailsArr = listMemberdetails.ToArray();
                    System.IO.File.WriteAllLines(Globals.selfRanktxt, selfDetailsArr);
                    Log("[ " + DateTime.Now + " ] => [ Your profile view rank details are saved in text file ]");
                    foreach (string item in memberdetailsArr)
                    {
                        System.IO.File.WriteAllLines(Globals.memberRanktxt, memberdetailsArr);
                        //System.IO.File.WriteAllLines(@"C:\Users\Glb-112\Desktop\InBoardPro\profileRank.txt", membrdetails);
                    }
                    Log("[ " + DateTime.Now + " ] => [ Profile view rank details of your connection are saved in text file ]");
                    Log("[ " + DateTime.Now + " ] => [ Profile view rank details of your connection are saved in CSV file ]");
                    try
                    {
                        AppFileHelper.AddingLinkedInDataToCSVFile(selfDetails, Globals.selfRankCsv);
                        string CSVHeader = "UserName" + "," + "ProfileName" + "," + "ProfileID" + "," + "ProfileImageURL" + "," + "CompanyPercentile" + "," + "CompanyRank" + "," + "ConnectionPercentile" + "," + "ConnectionRank" + "," + "Date";
                        string CSV_Content = userName.Replace(",", ";") + "," + selfName.Replace(",", ";") + "," + selfID.Replace(",", ";").Replace("M",string.Empty) + "," + selfImageUrl + "," + percentileCompany.Replace(",", ";") + "," + rankCompany.Replace(",", ";") + "," + percentileConnection.Replace(",", ";") + "," + rankConnection.Replace(",", ";") + "," + DateTime.Now;
                        CSVUtilities.ExportDataCSVFile(CSVHeader, CSV_Content, Globals.selfRankCsv);
                        Log("[ " + DateTime.Now + " ] => [ Your profile view rank details are saved in CSV file ]");
                    
                    }
                    catch (Exception ex)
                    {
                        GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> PROFILE RANK -  LinkedInProfilerank()  --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                        GlobusFileHelper.AppendStringToTextfileNewLine("Error --> PROFILE RANK -  LinkedInProfilerank() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + " || DateTime :- " + DateTime.Now, Globals.Path_LinkedInProfileRankErrorLogs);
                    }
                                       
                }

                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> PROFILE RANK -  LinkedInProfilerank()  --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                    GlobusFileHelper.AppendStringToTextfileNewLine("Error --> PROFILE RANK -  LinkedInProfilerank() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + " || DateTime :- " + DateTime.Now, Globals.Path_LinkedInProfileRankErrorLogs);
                }
                #endregion MEMBER DETAILS

            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> PROFILE RANK -  LinkedInProfilerank()  --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("Error --> PROFILE RANK -  LinkedInProfilerank() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + " || DateTime :- " + DateTime.Now, Globals.Path_LinkedInProfileRankErrorLogs);
            }

            #endregion
        }

        
        #region logger Log
        public Events logger = new Events();

        private void Log(string log)
        {
            try
            {
                EventsArgs loggerEventsArgs = new EventsArgs(log);  
                loggerrank.LogText(loggerEventsArgs);
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> PROFILE RANK -  LinkedInProfilerank()  --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("Error --> PROFILE RANK -  Log() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + " || DateTime :- " + DateTime.Now, Globals.Path_LinkedInProfileRankErrorLogs);
            }

        }
        #endregion
    }
}
