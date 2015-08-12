using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using BaseLib;

namespace Groups
{
    public class JoinSearchGroup
    {
        #region variable declaration
        public string accountUser = string.Empty;
        public string accountPass = string.Empty;
        public string proxyAddress = string.Empty;
        public string proxyPort = string.Empty;
        public string proxyUserName = string.Empty;
        public string proxyPassword = string.Empty;
        public string post_url = string.Empty;
        public static int BoxGroupCount = 5;

        public Dictionary<string, string> OpenGroupDtl = new Dictionary<string, string>();
        public static List<string> lstLinkedinGroupURL = new List<string>();
        public static int CountPerAccount = 0;
        public static Queue<string> Que_GroupUrl = new Queue<string>();
        public static readonly object Locker_GroupUrl = new object();

        public static int minDelay = 0;
        public static int maxDelay = 0;

        #endregion

        #region JoinSearchGroup
        public JoinSearchGroup()
        {
        }
        #endregion

        #region JoinSearchGroup
        public JoinSearchGroup(string UserName, string Password, string ProxyAddress, string ProxyPort, string ProxyUserName, string ProxyPassword)
        {
            this.accountUser = UserName;
            this.accountPass = Password;
            this.proxyAddress = ProxyAddress;
            this.proxyPort = ProxyPort;
            this.proxyUserName = ProxyUserName;
            this.proxyPassword = ProxyPassword;

        }
        #endregion

        #region Events logger
        public Events logger = new Events();
        public static Events loggerGroupusingUrl = new Events();

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

        private void LogGroupUrl(string log)
        {
            try
            {
                EventsArgs loggerEventsArgs = new EventsArgs(log);
                loggerGroupusingUrl.LogText(loggerEventsArgs);
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region PostAddOpenGroups
        public Dictionary<string, string> PostAddOpenGroups(ref GlobusHttpHelper HttpHelper, string SearchKeyword, string username)
        {
            try
            {
                int count = 5;
                string IncodePost = string.Empty;
                string PostMessage = string.Empty;
                string PostMessage1 = string.Empty;
                string MemFullName = string.Empty;
                string SearchId = string.Empty;
                string TotResult = string.Empty;
                string Grpurl = string.Empty;
                string GrpName = string.Empty;
                string IsOpenGrp = string.Empty;
                string GrpType = string.Empty;
                string GrpId = string.Empty;
                string GroupMember = string.Empty;
                List<string> checkDupGrpId = new List<string>();

                OpenGroupDtl.Clear();

                {
                    count = BoxGroupCount;
                }

                PostMessage = "http://www.linkedin.com/vsearch/g?orig=TRNV&keywords=" + SearchKeyword;

                //PostMessage = "http://www.linkedin.com/vsearch/g?type=groups&keywords=" + SearchKeyword + "&orig=GLHD&rsid=&pageKey=member-home";

                IncodePost = Uri.EscapeUriString(PostMessage);
                string pageSource = HttpHelper.getHtmlfromUrl1(new Uri(IncodePost));

                string[] RgxGroupDataResult = System.Text.RegularExpressions.Regex.Split(pageSource, "results_count_with_keywords");

                foreach (var itemResult in RgxGroupDataResult)
                {
                    try
                    {
                        if (!itemResult.Contains("<!DOCTYPE html>"))
                        {
                            int startindexRes = itemResult.IndexOf("strong");
                            string startRes = itemResult.Substring(startindexRes);
                            int endIndexRes = startRes.IndexOf("/strong");
                            TotResult = startRes.Substring(0, endIndexRes).Replace("&quot;", string.Empty).Replace(",", string.Empty).Replace("&", string.Empty).Replace(":", string.Empty).Replace("strong", string.Empty).Replace("\\u003e", "").Replace("\\u003c", "");
                        }
                    }
                    catch { }
                }
                if (TotResult.Contains("."))
                {
                    TotResult = TotResult.Replace(".", "").Trim();
                }
                int pageNo = Convert.ToInt32(TotResult);
                pageNo = (pageNo / 10) + 2;
                bool BreakingLoop = false;

                for (int i = 1; i < pageNo; i++)
                {
                    string PageWiseUrl = "vsearch/g?orig=TRNV&keywords=" + SearchKeyword + "&openFacets=N,G,L&page_num=" + i + "&pt=groups";
                    PostMessage1 = "http://www.linkedin.com/" + PageWiseUrl + "";

                    string pageSource1 = HttpHelper.getHtmlfromUrl1(new Uri(PostMessage1));
                    string[] RgxGroupData1 = System.Text.RegularExpressions.Regex.Split(pageSource1, "fmt_name");

                    foreach (var OpenGrps in RgxGroupData1)
                    {
                        try
                        {
                            if (!OpenGrps.Contains("<!DOCTYPE html>"))
                            {
                                try
                                {
                                    int startindex = OpenGrps.IndexOf("\":\"");
                                    string start = OpenGrps.Substring(startindex);
                                    int endIndex = start.IndexOf(",");
                                    GrpName = start.Substring(0, endIndex).Replace("\"", string.Empty).Replace("id", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Replace("<B>", string.Empty).Replace("/B", string.Empty).Replace("\\u002d", "-").Replace("\\u003e", "").Replace("\\u003cB", "").Replace("\\u003c", "").Replace("&amp;", "");
                                }
                                catch { }

                                try
                                {
                                    int startinGrTyp = OpenGrps.IndexOf("isOpen");
                                    string startGrTyp = OpenGrps.Substring(startinGrTyp);
                                    int endIndexGrTyp = startGrTyp.IndexOf(",");
                                    IsOpenGrp = startGrTyp.Substring(0, endIndexGrTyp).Replace("isOpen", string.Empty).Replace("\"", string.Empty).Replace(":", string.Empty).Replace(",", string.Empty).Trim();

                                    if (IsOpenGrp == "true")
                                    {
                                        GrpType = "Open Group";
                                    }
                                    else
                                    {
                                        GrpType = "Member Only Group";
                                    }
                                }
                                catch { }

                                if (GrpName == "group" || GrpName == "_postGroupLink" || GrpName == "_viewGroupLink" || GrpName == "_similarGroupLink" || GrpName == "_searchWithinLink" || GrpName == "_joinGroupLink" || GrpName == "N" || GrpName == "Remove All")
                                {
                                    continue;
                                }

                                try
                                {
                                    int startindex1 = OpenGrps.IndexOf("&gid=");
                                    string start1 = OpenGrps.Substring(startindex1);
                                    int endIndex1 = start1.IndexOf("trk=");
                                    GrpId = start1.Substring(0, endIndex1).Replace("&gid=", string.Empty).Replace("&", string.Empty).Trim();
                                }
                                catch { }

                                try
                                {
                                    int startindex2 = OpenGrps.IndexOf("membersCount");
                                    string start2 = OpenGrps.Substring(startindex2);
                                    int endIndex2 = start2.IndexOf(",");
                                    GroupMember = start2.Substring(0, endIndex2).Replace("membersCount\":", string.Empty).Trim();
                                }
                                catch { }

                                try
                                {
                                    int startindex = OpenGrps.IndexOf("gid=");
                                    string start = OpenGrps.Substring(startindex);
                                    int endIndex = start.IndexOf(",");
                                    string Grpfinalurl = start.Substring(0, endIndex).Replace("\"", string.Empty).Replace("gid=", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Replace("<B>", string.Empty).Replace("</B>", string.Empty).Replace("\\u002d", "-");
                                    Grpurl = "http://www.linkedin.com/groups?gid=" + Grpfinalurl;
                                }
                                catch { }


                                if (checkDupGrpId.Count != 0)
                                {
                                    if (checkDupGrpId.Contains(GrpId))
                                    {

                                    }
                                    else
                                    {
                                        checkDupGrpId.Add(GrpId);
                                        if (OpenGroupDtl.Count < count)
                                        {
                                            #region Data Saved In CSV File

                                            if (!string.IsNullOrEmpty(Grpurl) || !string.IsNullOrEmpty(GrpName))
                                            {
                                                try
                                                {
                                                    string CSVHeader = "Grpurl" + "," + "GrpName" + "," + "ID" + "," + "GroupMember";
                                                    string CSV_Content = Grpurl + "," + GrpName + "," + username + "," + GroupMember;
                                                    CSVUtilities.ExportDataCSVFile(CSVHeader, CSV_Content, Globals.path_LinkedinJoinSearchGroup);
                                                    Log("[ " + DateTime.Now + " ] => [ Data Saved In CSV File ! ]");

                                                }
                                                catch { }

                                            }
                                            #endregion

                                            OpenGroupDtl.Add(GrpName + " (" + GrpType + ")", GrpId);
                                        }
                                        else
                                        {
                                            BreakingLoop = true;
                                            break;
                                        }
                                        Log("[ " + DateTime.Now + " ] => [ Founded Group Name : " + GrpName + " ]");
                                    }

                                    int delay = RandomNumberGenerator.GenerateRandom(JoinSearchGroup.minDelay, JoinSearchGroup.maxDelay);
                                    Log("[ " + DateTime.Now + " ] => [ Delay for : " + delay + " Seconds ]");
                                    Thread.Sleep(delay * 1000);
                                }
                                else
                                {
                                    checkDupGrpId.Add(GrpId);
                                    if (OpenGroupDtl.Count < count)
                                    {
                                        #region Data Saved In CSV File

                                        if (!string.IsNullOrEmpty(Grpurl) || !string.IsNullOrEmpty(GrpName))
                                        {
                                            try
                                            {
                                                string CSVHeader = "Grpurl" + "," + "GrpName" + "," + "ID" + "," + "GrpMembr";
                                                string CSV_Content = Grpurl + "," + GrpName + "," + username + "," + GroupMember;
                                                CSVUtilities.ExportDataCSVFile(CSVHeader, CSV_Content, Globals.path_LinkedinJoinSearchGroup);
                                                Log("[ " + DateTime.Now + " ] => [ Data Saved In CSV File ! ]");

                                            }
                                            catch { }

                                        }
                                        #endregion

                                        OpenGroupDtl.Add(GrpName + " (" + GrpType + ")", GrpId);
                                    }
                                    else
                                    {
                                        BreakingLoop = true;
                                        break;
                                    }

                                    int delay = RandomNumberGenerator.GenerateRandom(JoinSearchGroup.minDelay, JoinSearchGroup.maxDelay);
                                    Log("[ " + DateTime.Now + " ] => [ Delay for : " + delay + " Seconds ]");
                                    Thread.Sleep(delay * 1000);
                                }
                            }

                        }
                        catch { }

                    }
                    if (BreakingLoop)
                    {
                        break;
                    }
                }

                return OpenGroupDtl;
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Search Groups --> PostAddOpenGroups() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Search Groups --> PostAddOpenGroups() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddSearchGroupErrorLogs);
                return OpenGroupDtl;
            }

        }
        #endregion

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

        #region PostAddOpenGroupsUsingUrl
        public Dictionary<string, string> PostAddOpenGroupsUsingUrl(ref GlobusHttpHelper HttpHelper, string username, int mindelay, int maxdelay, List<string> lstLinkedinGroupURL, bool IsDevideData)
        {
            try
            {
                string postdata = string.Empty;
                string postUrl = string.Empty;
                string ResLogin = string.Empty;
                string csrfToken = string.Empty;
                string sourceAlias = string.Empty;

                string ReturnString = string.Empty;
                string GroupName = string.Empty;
                string IncodePost = string.Empty;
                string PostMessage = string.Empty;
                string MemFullName = string.Empty;
                string SearchId = string.Empty;
                string GrpName = string.Empty;
                string IsOpenGrp = string.Empty;
                string GrpType = string.Empty;
                string GrpId = string.Empty;
                List<string> checkDupGrpId = new List<string>();
                OpenGroupDtl.Clear();
                List<string> tempGrpUrl = new List<string>();

                //-----------------------------------------------------------------------------------------------------------------------------------------
                string MessageText = string.Empty;
                string PostedMessage = string.Empty;

                string pageSource = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/home?trk=hb_tab_home_top"));
                if (pageSource.Contains("csrfToken"))
                {
                    csrfToken = pageSource.Substring(pageSource.IndexOf("csrfToken"), 50);
                    //string[] Arr = csrfToken.Split('&');
                    string[] Arr = csrfToken.Split('>');
                    csrfToken = Arr[0];
                    csrfToken = csrfToken.Replace(":", "%3A").Replace("csrfToken", "").Replace("\"", string.Empty).Replace("value", string.Empty).Replace("cs", string.Empty).Replace("id", string.Empty).Replace("=", string.Empty).Replace(">", string.Empty);
                    csrfToken = csrfToken.Trim();
                }

                if (pageSource.Contains("sourceAlias"))
                {
                    sourceAlias = pageSource.Substring(pageSource.IndexOf("sourceAlias"), 100);
                    string[] Arr = sourceAlias.Split('"');
                    sourceAlias = Arr[2];
                }
                //-----------------------------------------------------------------------------------------------------------------------------------------

                foreach (var GrpswithUrl in lstLinkedinGroupURL)
                {
                    try
                    {
                        string pageSource1 = HttpHelper.getHtmlfromUrl1(new Uri(GrpswithUrl));
                        string[] RgxGroupData1 = System.Text.RegularExpressions.Regex.Split(pageSource1, "<h1 class=\"group-name public\">");

                        if (RgxGroupData1.Count() == 1)
                        {
                            RgxGroupData1 = System.Text.RegularExpressions.Regex.Split(pageSource1, "<h1 class=\"group-name private\">");
                        }

                        try
                        {
                            int startindex = RgxGroupData1[1].IndexOf("title=");
                            string start = RgxGroupData1[1].Substring(startindex);
                            int endIndex = start.IndexOf("</a>");
                            GrpName = start.Substring(0, endIndex).Replace("\"", string.Empty).Replace("title=", string.Empty).Replace("This group is members only", string.Empty).Replace("This is an open group", string.Empty).Replace(">", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Replace("<B>", string.Empty).Replace("</B>", string.Empty).Replace("\\u002d", "-").Trim();

                            if (GrpName.Contains("<img src="))
                            {
                                GrpName = GrpName.Split('>')[0];
                            }
                        }
                        catch { }

                        try
                        {
                            if (RgxGroupData1[1].Contains("This group is members only"))
                            {
                                GrpType = "Member Only Group";
                            }
                            else
                            {
                                GrpType = "Open Group";
                            }
                        }
                        catch { }

                        if (GrpName == "group" || GrpName == "_postGroupLink" || GrpName == "_viewGroupLink" || GrpName == "_similarGroupLink" || GrpName == "_searchWithinLink" || GrpName == "_joinGroupLink" || GrpName == "N" || GrpName == "Remove All")
                        {
                            continue;
                        }

                        try
                        {
                            int startindex1 = RgxGroupData1[1].IndexOf("gid=");
                            string start1 = RgxGroupData1[1].Substring(startindex1);
                            int endIndex1 = start1.IndexOf("trk=");
                            GrpId = start1.Substring(0, endIndex1).Replace("gid=", string.Empty).Replace("&amp;", string.Empty).Trim();
                        }
                        catch { }

                        //post_url = string.Empty;
                        //post_url = (GroupValue.Key + ":" + GroupValue.Value);

                        try
                        {
                            string GoBack = "/%2Eanb_" + GrpId;
                            LogGroupUrl("[ " + DateTime.Now + " ] => [ ID: " + username + " has Joining the Group: " + GrpName + " ]");
                            string pageGetreq = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/groupRegistration?gid=" + GrpId + "&csrfToken=" + csrfToken + "&trk=group-join-button"));


                            if (pageGetreq.Contains("Your request to join the"))
                            {
                                ReturnString = "Your request to join the: " + GrpName + " group has been received.";
                                LogGroupUrl("[ " + DateTime.Now + " ] => [ " + ReturnString + " ]");
                                GlobusFileHelper.AppendStringToTextfileNewLine(username + ":" + ReturnString, Globals.path_JoinSearchGroupSuccess);

                                #region Data Saved In CSV File

                                if (!string.IsNullOrEmpty(GrpswithUrl) || !string.IsNullOrEmpty(GrpName))
                                {
                                    try
                                    {
                                        string CSVHeader = "Grpurl" + "," + "GrpName" + "," + "ID";
                                        string CSV_Content = GrpswithUrl + "," + GrpName + "," + username;
                                        CSVUtilities.ExportDataCSVFile(CSVHeader, CSV_Content, Globals.path_LinkedinJoinGroupUsingUrl);
                                        Log("[ " + DateTime.Now + " ] => [ GroupName: " + GrpName + " ]");
                                        Log("[ " + DateTime.Now + " ] => [ Data Saved In CSV File ! ]");
                                        LogGroupUrl("[ " + DateTime.Now + " ] => [ GroupName: " + GrpName + " ]");
                                        LogGroupUrl("[ " + DateTime.Now + " ] => [ Data Saved In CSV File ! ]");
                                        Log(GrpswithUrl + " " + username);
                                        tempGrpUrl.Add(GrpswithUrl);

                                    }
                                    catch { }

                                }
                                #endregion
                            }
                            else if (pageGetreq.Contains("Welcome to the"))
                            {
                                ReturnString = "Welcome to the: " + GrpName + " group on LinkedIn.";
                                LogGroupUrl("[ " + DateTime.Now + " ] => [ " + ReturnString + " ]");
                                GlobusFileHelper.AppendStringToTextfileNewLine(username + ":" + ReturnString, Globals.path_JoinSearchGroupSuccess);

                                #region Data Saved In CSV File

                                if (!string.IsNullOrEmpty(GrpswithUrl) || !string.IsNullOrEmpty(GrpName))
                                {
                                    try
                                    {
                                        string CSVHeader = "Grpurl" + "," + "GrpName" + "," + "ID";
                                        string CSV_Content = GrpswithUrl + "," + GrpName + "," + username;
                                        CSVUtilities.ExportDataCSVFile(CSVHeader, CSV_Content, Globals.path_LinkedinJoinGroupUsingUrl);
                                        Log("[ " + DateTime.Now + " ] => [ GroupName: " + GrpName + " ]");
                                        Log("[ " + DateTime.Now + " ] => [ Data Saved In CSV File ! ]");
                                        LogGroupUrl("[ " + DateTime.Now + " ] => [ GroupName: " + GrpName + " ]");
                                        LogGroupUrl("[ " + DateTime.Now + " ] => [ Data Saved In CSV File ! ]");

                                        tempGrpUrl.Add(GrpswithUrl);

                                    }
                                    catch { }

                                }
                                #endregion
                            }
                            else if (pageGetreq.Contains("You're already a member of the"))
                            {
                                ReturnString = "You're already a member of the: " + GrpName + "";
                                LogGroupUrl("[ " + DateTime.Now + " ] => [ " + ReturnString + " ]");
                                GlobusFileHelper.AppendStringToTextfileNewLine(username + ":" + ReturnString, Globals.path_JoinSearchGroupSuccess);
                            }
                            else if (pageGetreq.Contains("Your settings have been updated"))
                            {
                                ReturnString = "Your request to join the: " + GrpName + " group has been received.";
                                LogGroupUrl("[ " + DateTime.Now + " ] => [ " + ReturnString + " ]");
                                GlobusFileHelper.AppendStringToTextfileNewLine(username + ":" + ReturnString, Globals.path_JoinSearchGroupSuccess);
                            }
                            else if (pageGetreq.Contains("We’re sorry, this group has reached its maximum number of members allowed. If you have any questions, please contact the group manager for more information"))
                            {
                                LogGroupUrl("[ " + DateTime.Now + " ] => [ We’re sorry, this group has reached its maximum number of members allowed. ]");
                                GlobusFileHelper.AppendStringToTextfileNewLine(username + ":" + "We’re sorry, this group has reached its maximum number of members allowed.", Globals.path_JoinSearchGroupSuccess);
                            }
                            else if (pageGetreq.Contains("reached or exceeded the maximum number of pending group applications."))
                            {
                                LogGroupUrl("[ " + DateTime.Now + " ] => [ We’re sorry...You’ve reached or exceeded the maximum number of pending group applications. Please wait for your pending requests to be approved by a group manager or withdraw pending requests before joining new groups. ]");
                                GlobusFileHelper.AppendStringToTextfileNewLine(username + ":" + "We’re sorry...You’ve reached or exceeded the maximum number of pending group applications. Please wait for your pending requests to be approved by a group manager or withdraw pending requests before joining new groups. ", Globals.path_JoinSearchGroupSuccess);
                                //break;
                            }
                            else if (pageGetreq.Contains("reached or exceeded the maximum number of confirmed and pending groups."))
                            {
                                ReturnString = "You’ve reached or exceeded the maximum number of confirmed and pending groups.";
                                LogGroupUrl("[ " + DateTime.Now + " ] => [ You’ve reached or exceeded the maximum number of confirmed and pending groups.]");
                                GlobusFileHelper.AppendStringToTextfileNewLine(username + ":" + ReturnString, Globals.path_JoinSearchGroupSuccess);
                                //break;
                            }
                            else if (pageGetreq.Contains("Error"))
                            {
                                LogGroupUrl("[ " + DateTime.Now + " ] => [ Error In Joining Group ]");
                                GlobusFileHelper.AppendStringToTextfileNewLine(username + ":" + "Error In Joining Group", Globals.path_JoinSearchGroupSuccess);
                            }
                            else
                            {
                                LogGroupUrl("[ " + DateTime.Now + " ] => [ Group Could Not Be Joined ]");
                                GlobusFileHelper.AppendStringToTextfileNewLine(username + ":" + "Group Could Not Be Joined", Globals.path_JoinSearchGroupSuccess);
                                //LogGroupUrl(username + "   " + GrpswithUrl);
                            }

                        }
                        catch { }

                        if (checkDupGrpId.Count != 0)
                        {
                            if (checkDupGrpId.Contains(GrpId))
                            {

                            }
                            else
                            {
                                checkDupGrpId.Add(GrpId);
                                OpenGroupDtl.Add(GrpName + " (" + GrpType + ")", GrpId);
                                Log("[ " + DateTime.Now + " ] => [ Founded Group Name : " + GrpName + " ]");
                                LogGroupUrl("[ " + DateTime.Now + " ] => [ Founded Group Name : " + GrpName + " ]");
                            }

                        }
                        else
                        {
                            OpenGroupDtl.Add(GrpName + " (" + GrpType + ")", GrpId);
                        }
                    }
                    catch { }
                    if (!IsDevideData)
                    {
                        if (tempGrpUrl.Count() == CountPerAccount)
                        {
                            break;
                        }
                    }

                    int delay = RandomNumberGenerator.GenerateRandom(mindelay, maxdelay);
                    LogGroupUrl("[ " + DateTime.Now + " ] => [ Delay for : " + delay + " Seconds ]");
                    Thread.Sleep(delay * 1000);
                }

                //foreach (var Removable in tempGrpUrl)
                //{
                //    lstLinkedinGroupURL.Remove(Removable);
                //}

                return OpenGroupDtl;
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Search Groups --> PostAddOpenGroups() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Search Groups --> PostAddOpenGroups() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddSearchGroupErrorLogs);
                return OpenGroupDtl;
            }

        }
        #endregion

        #region PostSearchGroupAddFinal
        public string PostSearchGroupAddFinal(ref GlobusHttpHelper HttpHelper, string Screen_name, string pass, List<string> SelectedItem, int mindelay, int maxdelay)
        {
            string postdata = string.Empty;
            string postUrl = string.Empty;
            string ResLogin = string.Empty;
            string csrfToken = string.Empty;
            string sourceAlias = string.Empty;
            string ReturnString = string.Empty;
            string GroupName = string.Empty;

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
                    csrfToken = csrfToken.Replace(":", "%3A").Replace("csrfToken", "").Replace("\"", string.Empty).Replace("value", string.Empty).Replace("cs", string.Empty).Replace("id", string.Empty).Replace("=", string.Empty).Replace(">", string.Empty).Replace("\n", string.Empty).Replace("<script src", string.Empty).Replace("<meta http-", string.Empty);
                    csrfToken = csrfToken.Trim();
                }

                if (pageSource.Contains("sourceAlias"))
                {
                    sourceAlias = pageSource.Substring(pageSource.IndexOf("sourceAlias"), 100);
                    string[] Arr = sourceAlias.Split('"');
                    sourceAlias = Arr[2];
                }

                postUrl = "https://www.linkedin.com/uas/login-submit";
                postdata = "session_key=" + Screen_name + "&session_password=" + pass + "&source_app=&trk=guest_home_login&session_redirect=&csrfToken=" + csrfToken + "&sourceAlias=" + sourceAlias;
                ResLogin = HttpHelper.postFormData(new Uri(postUrl), postdata);

                try
                {
                    foreach (string Itemgid in SelectedItem)
                    {
                        try
                        {
                            string GoBack = "/%2Eanb_" + Itemgid.Split(':')[1];
                            Log("[ " + DateTime.Now + " ] => [ Joining Group: " + Itemgid.Split(':')[0] + " ]");
                            string pageGetreq = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/groupRegistration?gid=" + Itemgid.Split(':')[1] + "&csrfToken=" + csrfToken + "&trk=group-join-button"));

                            Thread.Sleep(2000);

                            if (pageGetreq.Contains("Your request to join the"))
                            {
                                ReturnString = "Your request to join the: " + Itemgid.Split(':')[0] + " group has been received.";
                                Log("[ " + DateTime.Now + " ] => [ " + ReturnString + " ]");
                                GlobusFileHelper.AppendStringToTextfileNewLine(Screen_name + ":" + ReturnString, Globals.path_JoinSearchGroupSuccess);
                            }
                            else if (pageGetreq.Contains("Welcome to the"))
                            {
                                ReturnString = "Welcome to the: " + Itemgid.Split(':')[0] + " group on LinkedIn.";
                                Log("[ " + DateTime.Now + " ] => [ " + ReturnString + " ]");
                                GlobusFileHelper.AppendStringToTextfileNewLine(Screen_name + ":" + ReturnString, Globals.path_JoinSearchGroupSuccess);
                            }
                            else if (pageGetreq.Contains("You're already a member of the"))
                            {
                                ReturnString = "[ " + DateTime.Now + " ] => [ You're already a member of the: " + Itemgid.Split(':')[0] + " ]";
                                Log("" + ReturnString + "");
                                GlobusFileHelper.AppendStringToTextfileNewLine(Screen_name + ":" + ReturnString, Globals.path_JoinSearchGroupSuccess);
                            }
                            else if (pageGetreq.Contains("Your settings have been updated"))
                            {
                                ReturnString = "Your request to join the: " + Itemgid.Split(':')[0] + " group has been received.";
                                Log("[ " + DateTime.Now + " ] => [ " + ReturnString + " ]");
                                GlobusFileHelper.AppendStringToTextfileNewLine(Screen_name + ":" + ReturnString, Globals.path_JoinSearchGroupSuccess);
                            }
                            else if (pageGetreq.Contains("We’re sorry, this group has reached its maximum number of members allowed. If you have any questions, please contact the group manager for more information"))
                            {
                                Log("[ " + DateTime.Now + " ] => [ We’re sorry, this group has reached its maximum number of members allowed. ]");
                                GlobusFileHelper.AppendStringToTextfileNewLine(Screen_name + ":" + "We’re sorry, this group has reached its maximum number of members allowed.", Globals.path_JoinSearchGroupFail);
                            }
                            else if (pageGetreq.Contains("reached or exceeded the maximum number of pending group applications."))
                            {
                                Log("[ " + DateTime.Now + " ] => [ We’re sorry...You’ve reached or exceeded the maximum number of pending group applications. Please wait for your pending requests to be approved by a group manager or withdraw pending requests before joining new groups. ]");
                                GlobusFileHelper.AppendStringToTextfileNewLine(Screen_name + ":" + "We’re sorry...You’ve reached or exceeded the maximum number of pending group applications. Please wait for your pending requests to be approved by a group manager or withdraw pending requests before joining new groups. ", Globals.path_JoinSearchGroupFail);
                                break;
                            }
                            else if (pageGetreq.Contains("reached or exceeded the maximum number of confirmed and pending groups."))
                            {
                                ReturnString = "You’ve reached or exceeded the maximum number of confirmed and pending groups.";
                                Log("[ " + DateTime.Now + " ] => [ You’ve reached or exceeded the maximum number of confirmed and pending groups. ]");
                                GlobusFileHelper.AppendStringToTextfileNewLine(Screen_name + ":" + ReturnString, Globals.path_JoinSearchGroupFail);
                                break;
                            }
                            else if (pageGetreq.Contains("Error"))
                            {
                                Log("[ " + DateTime.Now + " ] => [ Error In Joining Group ]");
                                GlobusFileHelper.AppendStringToTextfileNewLine(Screen_name + ":" + "Error In Joining Group", Globals.path_JoinSearchGroupFail);
                            }
                            else
                            {
                                Log("[ " + DateTime.Now + " ] => [ Group Could Not Be Joined ]");
                                GlobusFileHelper.AppendStringToTextfileNewLine(Screen_name + ":" + "Group Could Not Be Joined", Globals.path_JoinSearchGroupFail);
                            }

                            if (SelectedItem.Count > 1)
                            {
                                int delay = RandomNumberGenerator.GenerateRandom(mindelay, maxdelay);
                                Log("[ " + DateTime.Now + " ] => [ Delay for : " + delay + " Seconds ]");
                                Thread.Sleep(delay * 1000);
                            }
                        }
                        catch { }
                    }
                    Log("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                    Log("------------------------------------------------------------------------------------------------------------------------------------");
                }
                catch (Exception ex)
                {
                    Log("[ " + DateTime.Now + " ] => [ Error:" + ex.Message + "StackTrace --> >>>" + ex.StackTrace + " ]");
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Search Groups --> PostSearchGroupAddFinal() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Search Groups --> PostSearchGroupAddFinal() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddSearchGroupErrorLogs);
                ReturnString = "Error";
            }
            return ReturnString;
        }
        #endregion

        #region PostAddGroupUsingUrl
        public string PostAddGroupUsingUrl(ref GlobusHttpHelper HttpHelper, Dictionary<string, Dictionary<string, string>> LinkdInContacts, string Screen_name, string pass, int mindelay, int maxdelay)
        {
            string postdata = string.Empty;
            string postUrl = string.Empty;
            string ResLogin = string.Empty;
            string csrfToken = string.Empty;
            string sourceAlias = string.Empty;

            string ReturnString = string.Empty;
            string GroupName = string.Empty;

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

                postUrl = "https://www.linkedin.com/uas/login-submit";
                postdata = "session_key=" + Screen_name + "&session_password=" + pass + "&source_app=&trk=guest_home_login&session_redirect=&csrfToken=" + csrfToken + "&sourceAlias=" + sourceAlias;
                ResLogin = HttpHelper.postFormData(new Uri(postUrl), postdata);

                try
                {
                    foreach (KeyValuePair<string, Dictionary<string, string>> UserValue in LinkdInContacts)
                    {

                        foreach (KeyValuePair<string, string> GroupValue in UserValue.Value)
                        {
                            post_url = string.Empty;
                            post_url = (GroupValue.Key + ":" + GroupValue.Value);
                            try
                            {
                                string GoBack = "/%2Eanb_" + post_url.Split(':')[1];
                                LogGroupUrl("[ " + DateTime.Now + " ] => [ ID: " + Screen_name + " has Joining the Group: " + post_url.Split(':')[0] + " ]");
                                string pageGetreq = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/groupRegistration?gid=" + post_url.Split(':')[1] + "&csrfToken=" + csrfToken + "&trk=group-join-button"));


                                if (pageGetreq.Contains("Your request to join the"))
                                {
                                    ReturnString = "Your request to join the: " + post_url.Split(':')[0] + " group has been received.";
                                    LogGroupUrl("[ " + DateTime.Now + " ] => [ " + ReturnString + " ]");
                                    GlobusFileHelper.AppendStringToTextfileNewLine(Screen_name + ":" + ReturnString, Globals.path_JoinSearchGroupSuccess);
                                }
                                else if (pageGetreq.Contains("Welcome to the"))
                                {
                                    ReturnString = "Welcome to the: " + post_url.Split(':')[0] + " group on LinkedIn.";
                                    LogGroupUrl("[ " + DateTime.Now + " ] => [ " + ReturnString + " ]");
                                    GlobusFileHelper.AppendStringToTextfileNewLine(Screen_name + ":" + ReturnString, Globals.path_JoinSearchGroupSuccess);
                                }
                                else if (pageGetreq.Contains("You're already a member of the"))
                                {
                                    ReturnString = "You're already a member of the: " + post_url.Split(':')[0] + "";
                                    LogGroupUrl("[ " + DateTime.Now + " ] => [ " + ReturnString + " ]");
                                    GlobusFileHelper.AppendStringToTextfileNewLine(Screen_name + ":" + ReturnString, Globals.path_JoinSearchGroupSuccess);
                                }
                                else if (pageGetreq.Contains("Your settings have been updated"))
                                {
                                    ReturnString = "Your request to join the: " + post_url.Split(':')[0] + " group has been received.";
                                    LogGroupUrl("[ " + DateTime.Now + " ] => [ " + ReturnString + " ]");
                                    GlobusFileHelper.AppendStringToTextfileNewLine(Screen_name + ":" + ReturnString, Globals.path_JoinSearchGroupSuccess);
                                }
                                else if (pageGetreq.Contains("We’re sorry, this group has reached its maximum number of members allowed. If you have any questions, please contact the group manager for more information"))
                                {
                                    LogGroupUrl("[ " + DateTime.Now + " ] => [ We’re sorry, this group has reached its maximum number of members allowed. ]");
                                    GlobusFileHelper.AppendStringToTextfileNewLine(Screen_name + ":" + "We’re sorry, this group has reached its maximum number of members allowed.", Globals.path_JoinSearchGroupFail);
                                }
                                else if (pageGetreq.Contains("reached or exceeded the maximum number of pending group applications."))
                                {
                                    LogGroupUrl("[ " + DateTime.Now + " ] => [ We’re sorry...You’ve reached or exceeded the maximum number of pending group applications. Please wait for your pending requests to be approved by a group manager or withdraw pending requests before joining new groups. ]");
                                    GlobusFileHelper.AppendStringToTextfileNewLine(Screen_name + ":" + "We’re sorry...You’ve reached or exceeded the maximum number of pending group applications. Please wait for your pending requests to be approved by a group manager or withdraw pending requests before joining new groups. ", Globals.path_JoinSearchGroupFail);

                                }
                                else if (pageGetreq.Contains("reached or exceeded the maximum number of confirmed and pending groups."))
                                {
                                    ReturnString = "You’ve reached or exceeded the maximum number of confirmed and pending groups.";
                                    LogGroupUrl("[ " + DateTime.Now + " ] => [ You’ve reached or exceeded the maximum number of confirmed and pending groups.]");
                                    GlobusFileHelper.AppendStringToTextfileNewLine(Screen_name + ":" + ReturnString, Globals.path_JoinSearchGroupFail);
                                }
                                else if (pageGetreq.Contains("Error"))
                                {
                                    LogGroupUrl("[ " + DateTime.Now + " ] => [ Error In Joining Group ]");
                                    GlobusFileHelper.AppendStringToTextfileNewLine(Screen_name + ":" + "Error In Joining Group", Globals.path_JoinSearchGroupFail);
                                }
                                else
                                {
                                    LogGroupUrl("[ " + DateTime.Now + " ] => [ Group Could Not Be Joined ]");
                                    GlobusFileHelper.AppendStringToTextfileNewLine(Screen_name + ":" + "Group Could Not Be Joined", Globals.path_JoinSearchGroupFail);
                                }

                            }
                            catch { }

                            int delay = RandomNumberGenerator.GenerateRandom(mindelay, maxdelay);
                            LogGroupUrl("[ " + DateTime.Now + " ] => [ Delay for : " + delay + " Seconds ]");
                            Thread.Sleep(delay * 1000);
                        }
                    }

                }
                catch (Exception ex)
                {
                    LogGroupUrl(DateTime.Now + " [ Error:" + ex.Message + "StackTrace --> >>>" + ex.StackTrace + " ]");
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Search Groups --> PostSearchGroupAddFinal() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Search Groups --> PostSearchGroupAddFinal() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddSearchGroupErrorLogs);
                ReturnString = "Error";
            }
            return ReturnString;
        }
        #endregion
    }
}
