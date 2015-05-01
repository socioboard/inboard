using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using BaseLib;

namespace Others
{
    public class LinkedinCompanyFollow
    {
        #region globla declaration
        public string accountUser = string.Empty;
        public string accountPass = string.Empty;
        public string proxyAddress = string.Empty;
        public string proxyPort = string.Empty;
        public string proxyUserName = string.Empty;
        public string proxyPassword = string.Empty;

        public static int BoxGroupCount = 5;

        Dictionary<string, string> CompanyDtl = new Dictionary<string, string>();
        public static List<string> lstLinkedinCompanyURL = new List<string>();
        public static int CountPerAccount = 0;
        public static Queue<string> Que_GroupUrl = new Queue<string>();
        public static readonly object Locker_GroupUrl = new object();
        public string post_url = string.Empty; 
        #endregion

        #region LinkedinCompanyFollow
        public LinkedinCompanyFollow(string UserName, string Password, string ProxyAddress, string ProxyPort, string ProxyUserName, string ProxyPassword)
        {
            this.accountUser = UserName;
            this.accountPass = Password;
            this.proxyAddress = ProxyAddress;
            this.proxyPort = ProxyPort;
            this.proxyUserName = ProxyUserName;
            this.proxyPassword = ProxyPassword;
        }

        public LinkedinCompanyFollow()
        {
           
        } 

        #endregion

        #region Events logger
        public static Events logger = new Events();
        //public  Events logger = new Events();
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
        
         #region PostAddCompanyUsingUrl
         public string PostAddCompanyUsingUrl(ref GlobusHttpHelper HttpHelper, string username, int mindelay, int maxdelay)
         {
             string ReturnString = string.Empty;

             try
             {
                 string IncodePost = string.Empty;
                 string PostMessage = string.Empty;
                 string MemFullName = string.Empty;
                 string SearchId = string.Empty;
                 string CompanyName = string.Empty;
                 string IsOpenGrp = string.Empty;
                 string CompanyType = string.Empty;
                 string CompanyId = string.Empty;
                 string goback = string.Empty;
                 List<string> checkDupcompanyId = new List<string>();
                 List<string> tempcompanyUrl = new List<string>();
                 CompanyDtl.Clear();

                 //----------------------------------------------------------------------------------------------------------------------
                 string postdata = string.Empty;
                 string postUrl = string.Empty;
                 string ResLogin = string.Empty;
                 string csrfToken = string.Empty;
                 string sourceAlias = string.Empty;
                 string MessageText = string.Empty;
                 string PostedMessage = string.Empty;

                 string pageSource = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/home?trk=hb_tab_home_top"));

                 if (pageSource.Contains("csrfToken"))
                 {
                     csrfToken = pageSource.Substring(pageSource.IndexOf("csrfToken"), 50);
                     string[] Arr = csrfToken.Split('<');
                     csrfToken = Arr[0];
                     csrfToken = csrfToken.Replace(":", "%3A").Replace("csrfToken", "").Replace("\"", string.Empty).Replace("value", string.Empty).Replace("cs", string.Empty).Replace("id", string.Empty).Replace("=", string.Empty).Replace(">",string.Empty);
                     csrfToken = csrfToken.Replace("\n<script src","").Trim();
                 }

                 if (pageSource.Contains("sourceAlias"))
                 {
                     sourceAlias = pageSource.Substring(pageSource.IndexOf("sourceAlias"), 100);
                     string[] Arr = sourceAlias.Split('"');
                     sourceAlias = Arr[2];
                 }

                 //-------------------------------------------------------------------------------------------------------------------------------

                  foreach (var CompanywithUrl in lstLinkedinCompanyURL)
                  {
                     try
                     {
                         string finalurl = string.Empty;

                         if (!CompanywithUrl.Contains("http"))
                         {
                             finalurl = "http://" + CompanywithUrl;
                         }
                         else
                         {
                             finalurl = CompanywithUrl; ;
                         }

                         string pageSource1 = HttpHelper.getHtmlfromUrl1(new Uri(finalurl));

                         string[] RgxCompanyData1 = System.Text.RegularExpressions.Regex.Split(pageSource1, "<h1 class=\"name\"");

                         try
                         {
                             int startindex = RgxCompanyData1[1].IndexOf("<span itemprop=\"name\">");
                             string start = RgxCompanyData1[1].Substring(startindex);
                             int endIndex = start.IndexOf("</span>");
                             CompanyName = start.Substring(0, endIndex).Replace("<span itemprop=\"name\">", string.Empty).Replace("\n", string.Empty).Replace("\\u002d", "-").Trim();

                             if (CompanyName.Contains("<img src="))
                             {
                                 CompanyName = CompanyName.Split('>')[0];
                             }
                         }
                         catch
                         {
                             CompanyName = getBetween(pageSource1, "company_name", "</a>").Replace("\n", "").Replace(">", "").Replace("\"", "").Trim();
                         }
                       
                         try
                         {
                             int startindex1 = RgxCompanyData1[1].IndexOf("/company/follow/submit?id=");
                             string start1 = RgxCompanyData1[1].Substring(startindex1);
                             int endIndex1 = start1.IndexOf("&amp");
                             CompanyId = start1.Substring(0, endIndex1).Replace("/company/follow/submit?id=", string.Empty).Replace("&amp;", string.Empty).Trim();

                             //int startindex1 = pageSource1.IndexOf("X-FS-Origin-Request");
                             //string start1 = pageSource1.Substring(startindex1);
                             //int endIndex1 = start1.IndexOf("?");
                             //CompanyId = start1.Substring(0, endIndex1).Replace("X-FS-Origin-Request", string.Empty).Replace("?", string.Empty).Replace("\"", "").Replace("company", "").Replace("/", "").Replace(":", "").Trim();
                         }
                         catch 
                         {
                             CompanyId = getBetween(pageSource1, "submit?id=", "&amp;").Replace("\n", "").Replace(">", "").Replace("\"", "").Trim();
                         }

                         try
                         {   int startindex1 = pageSource1.IndexOf("goback=");
                             string start1 = pageSource1.Substring(startindex1);
                             int endIndex1 = start1.IndexOf("&");
                             goback = start1.Substring(0, endIndex1).Replace("goback=", string.Empty);

                         }
                         catch { }


                            string txid = (UnixTimestampFromDateTime(System.DateTime.Now) * 1000).ToString();

                             //postUrl = "https://www.linkedin.com/uas/login-submit";
                             //postdata = "session_key=" + username + "&session_password=" + pass + "&source_app=&trk=guest_home_login&session_redirect=&csrfToken=" + csrfToken + "&sourceAlias=" + sourceAlias;
                             //ResLogin = HttpHelper.postFormData(new Uri(postUrl), postdata);

                             try
                             {
                                 //string GoBack = "%2Ebzo_*1_*1_*1_*1_*1_*1_*1_" + CompanyId;
                                 Log("[ " + DateTime.Now + " ] => [ ID: " + username + " has Follow the Company: " + CompanyName.ToString() + " ]");
                                 //string pageGetreq = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/company/follow/submit?id=" + CompanyId + "&fl=start&version=2&ft=pageKey%3Dbiz-overview-internal%3Bmodule%3Dbutton&sp=biz-overview&csrfToken=" + csrfToken + "&goback=" + GoBack));
                                 string pageGetreq = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/company/follow/submit?id=" + CompanyId + "&fl=start&version=2&ft=pageKey%3Dbiz-overview%3Bmodule%3Dbutton&sp=biz-overview&csrfToken=" + csrfToken + "&goback=" + goback + "&ajax=&rnd=" + txid));
                                                                                                      
                                 if (pageGetreq.Contains("Following"))
                                 {
                                     ReturnString = "Your request to Follow the: " + CompanyName.ToString() + " company has been Successfully Followed.";
                                     Log("[ " + DateTime.Now + " ] => [ " + ReturnString + " ]");
                                     
                                     #region Data Saved In CSV File

                                     if (!string.IsNullOrEmpty(CompanywithUrl) || !string.IsNullOrEmpty(CompanyName))
                                     {
                                         try
                                         {
                                             string CSVHeader = "Companyurl" + "," + "CompanyName" + "," + "LoginID";
                                             string CSV_Content = CompanywithUrl.Replace(",", ";") + "," + CompanyName.Replace(",", ";") + "," + username;
                                             CSVUtilities.ExportDataCSVFile(CSVHeader, CSV_Content, Globals.path_LinkedinFollowCompanyUsingUrl);
                                             Log("[ " + DateTime.Now + " ] => [ Company Name: " + CompanyName.Replace(",", ";") + " in Account : " + username.Replace(",", ";") + " ]");
                                             Log("[ " + DateTime.Now + " ] => [ Data Saved In CSV File ! ]");

                                             tempcompanyUrl.Add(CompanywithUrl);

                                         }
                                         catch { }

                                     }
                                     #endregion

                                 }
                                 else if (pageGetreq.Contains("Error"))
                                 {
                                     Log("[ " + DateTime.Now + " ] => [ Error In Follow Company ]");
                                     GlobusFileHelper.AppendStringToTextfileNewLine(username + ":" + "Error In Follow Company", Globals.path_Not_FollowCompany);
                                 }
                                 else
                                 {
                                     Log("[ " + DateTime.Now + " ] => [ Company Could Not Be Followed ]");
                                     GlobusFileHelper.AppendStringToTextfileNewLine(username + ":" + "Company Could Not Be Followed", Globals.path_Not_FollowCompany);
                                 }
                             }
                             catch { }
                          

                         if (checkDupcompanyId.Count != 0)
                         {
                             if (checkDupcompanyId.Contains(CompanyId))
                             {

                             }
                             else
                             {
                                 checkDupcompanyId.Add(CompanyId);
                                 CompanyDtl.Add(CompanyName,CompanyId);
                                 Log("[ " + DateTime.Now + " ] => [ Founded Company Name : " + CompanyName + " ]");                                 
                             }

                         }
                         else
                         {
                             CompanyDtl.Add(CompanyName,CompanyId);
                         }
                     }
                     catch { }

                     if (tempcompanyUrl.Count() == CountPerAccount)
                     {
                         break;
                     }

                     int delay = RandomNumberGenerator.GenerateRandom(mindelay, maxdelay);
                     Log("[ " + DateTime.Now + " ] => [ Delay for : " + delay + " Seconds ]");
                     Thread.Sleep(delay * 1000);

                 }
             }
             catch (Exception ex)
             {
                 GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Search Company for follow --> PostAddCompanyUsingUrl() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                 GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Search Company for follow --> PostAddCompanyUsingUrl() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinFollowCompanyErrorLogs);
                 return ReturnString;
             }
             return ReturnString;

         }
         #endregion

         public static long UnixTimestampFromDateTime(DateTime date)
         {
             long unixTimestamp = date.Ticks - new DateTime(1970, 1, 1).Ticks;
             unixTimestamp /= TimeSpan.TicksPerSecond;
             return unixTimestamp;
         }


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

        #region PostFollowCompanyUsingUrl
         public string PostFollowCompanyUsingUrl(ref GlobusHttpHelper HttpHelper, Dictionary<string, Dictionary<string, string>> LinkdInContacts, int mindelay, int maxdelay)
         {
             string postdata = string.Empty;
             string postUrl = string.Empty;
             string ResLogin = string.Empty;
             string csrfToken = string.Empty;
             string sourceAlias = string.Empty;
             string ReturnString = string.Empty;
    
             try
             {
                 string MessageText = string.Empty;
                 string PostedMessage = string.Empty;
                 string pageSource = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/home?trk=hb_tab_home_top"));
                 
                 if (pageSource.Contains("csrfToken"))
                 {
                     csrfToken = pageSource.Substring(pageSource.IndexOf("csrfToken"), 50);
                     string[] Arr = csrfToken.Split('&');
                     csrfToken = Arr[0];
                     csrfToken = csrfToken.Replace(":", "%3A").Replace("csrfToken", "").Replace("\"", string.Empty).Replace("value", string.Empty).Replace("cs", string.Empty).Replace("id", string.Empty).Replace("=", string.Empty);
                     csrfToken = csrfToken.Trim();
                 }

                 if (pageSource.Contains("sourceAlias"))
                 {
                     sourceAlias = pageSource.Substring(pageSource.IndexOf("sourceAlias"), 100);
                     string[] Arr = sourceAlias.Split('"');
                     sourceAlias = Arr[2];
                 }

                 try
                 {
                     int counter = 0;
                     foreach (KeyValuePair<string, Dictionary<string, string>> UserValue in LinkdInContacts)
                     {
                         counter = counter + 1;
                         foreach (KeyValuePair<string, string> GroupValue in UserValue.Value)
                         {
                             post_url = string.Empty;
                             post_url = (GroupValue.Key + ":" + GroupValue.Value);

                             string Screen_name = UserValue.Key.Split(':')[0];
                             string pass = UserValue.Key.Split(':')[1];

                             postUrl = "https://www.linkedin.com/uas/login-submit";
                             postdata = "session_key=" + Screen_name + "&session_password=" + pass + "&source_app=&trk=guest_home_login&session_redirect=&csrfToken=" + csrfToken + "&sourceAlias=" + sourceAlias;
                             ResLogin = HttpHelper.postFormData(new Uri(postUrl), postdata);

                             try
                             {
                                 string GoBack = "%2Ebzo_*1_*1_*1_*1_*1_*1_*1_" + post_url.Split(':')[1];
                                 Log("[ " + DateTime.Now + " ] => [ ID: " + Screen_name + " has Follow the Company: " + post_url.Split(':')[0] + " ]");
                                 string pageGetreq = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/company/follow/submit?id=" + post_url.Split(':')[1] + "&fl=start&version=2&ft=pageKey%3Dbiz-overview-internal%3Bmodule%3Dbutton&sp=biz-overview&csrfToken=" + csrfToken + "&goback="+ GoBack));
                               
                                 if (pageGetreq.Contains("Following"))
                                 {
                                     ReturnString = "Your request to Follow the: " + post_url.Split(':')[0] + " company has been Successfully Followed.";
                                     Log("[ " + DateTime.Now + " ] => [ " + ReturnString + " ]");
                                     //GlobusFileHelper.AppendStringToTextfileNewLine(Screen_name + ":" + ReturnString, Globals.path_FollowCompany);

                                 }
                                 else if (pageGetreq.Contains("Error"))
                                 {
                                     Log("[ " + DateTime.Now + " ] => [ Error In Follow Company ]");
                                     GlobusFileHelper.AppendStringToTextfileNewLine(Screen_name + ":" + "Error In Follow Company", Globals.path_Not_FollowCompany);
                                 }
                                 else
                                 {
                                     Log("[ " + DateTime.Now + " ] => [ Company Could Not Be Followed ]");
                                     GlobusFileHelper.AppendStringToTextfileNewLine(Screen_name + ":" + "Company Could Not Be Followed", Globals.path_Not_FollowCompany);
                                 }
                             }
                             catch { }

                             if (counter < LinkdInContacts.Count())
                             {
                                 int delay = RandomNumberGenerator.GenerateRandom(mindelay, maxdelay);
                                 Log("[ " + DateTime.Now + " ] => [ Delay for : " + delay + " Seconds ]");
                                 Thread.Sleep(delay * 1000);
                             }

                             
                         }
                     }

                 }
                 catch (Exception ex)
                 {
                     GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Follow Company--1 --> PostFollowCompanyUsingUrl() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                     GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Follow Company--1 --> PostFollowCompanyUsingUrl() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinFollowCompanyErrorLogs);
                 }
             }
             catch (Exception ex)
             {
                 GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Follow Company--2--> PostFollowCompanyUsingUrl() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                 GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Follow Company--2--> PostFollowCompanyUsingUrl() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinFollowCompanyErrorLogs);
                 ReturnString = "Error";
             }
             return ReturnString;
         }
         #endregion

    }
}
