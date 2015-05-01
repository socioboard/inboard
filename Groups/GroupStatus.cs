#region namespace
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using BaseLib.DB_Repository;
using System.IO;
using Groups;
using BaseLib;
#endregion

namespace Groups
{
  public class GroupStatus
    {
        #region Variable Declaration

       // public Events loggergrpupdate = new Events();
        public Events loggergrpupdate = new Events();
        public Events loggerGroupMem = new Events();
        public Events loggerRemPendingGroup = new Events();
        public Events loggerInviteGroups = new Events();
        public static Events loggerEndorseCampaign = new Events();
        
        GlobusHttpHelper HttpHelper = new GlobusHttpHelper();
        string _Data = string.Empty;
        public string str = string.Empty;
        public string accountUser = string.Empty;
        public string accountPass = string.Empty;
        public string proxyAddress = string.Empty;
        public string proxyPort = string.Empty;
        public string proxyUserName = string.Empty;
        public string proxyPassword = string.Empty;
        public string Url = string.Empty;
        public bool IsLoggedIn = false;
        public static bool ManageTabGroupStatus = false;
        public static List<string> GroupNameLst = new List<string>();
        public static List<string> GroupKeyLst = new List<string>();
        public static Dictionary<string, string> GroupName = new Dictionary<string, string>();
        public static Dictionary<string, string> GroupSpecMem = new Dictionary<string, string>();
        public static Dictionary<string, string> MemberNameAndID = new Dictionary<string, string>();
        public static Dictionary<string, string> GroupDtl = new Dictionary<string, string>();
        public static Dictionary<string, string> GroupKey = new Dictionary<string, string>();
        public string totalPageNumber = string.Empty;
        public static string GrouppageSourcewithProxy = string.Empty;
        public static string moduleLog = string.Empty;
        /// <summary>
        /// Contains Group Name, Id and the id of Contact/Friend linked with this group
        /// </summary>
        public static Dictionary<string, Dictionary<string,string>> GrpAdd = new Dictionary<string, Dictionary<string,string>>();
        
        public static List<string> GroupUrl = new List<string>();
        public static List<string> GroupMemUrl = new List<string>();
        public static List<string> MemId = new List<string>();
        public static bool selectAllGroup = false;
        /// <summary>
        /// Groups checked/selected in frmMain >>> from CheckedListBox >>> chkExistGroup
        /// </summary>
        public static List<string> lstExistGroup = new List<string>();
        public static List<string> lstPendingGroup = new List<string>();

        public static List<string> lstInvitationGroup = new List<string>();
        public static List<string> lstEmailsGroupInvite = new List<string>();

        public static string FromID = string.Empty;
        public static string FromNam = string.Empty;
        public static bool WithCsvinAddFriend = false;
        public static string SearchKeyword = string.Empty;
        public static bool WithGroupSearch = false;
        public static bool withExcelInput = false;
        public static List<string[]> msgGroupMem_excelData = new List<string[]>();
        public static List<string[]> Cmpmsg_excelData = new List<string[]>();
        public static bool allAccount = false;

        int PageNo = 0;

        public static Queue<string> Que_GrpPostTitle_Post = new Queue<string>();
        public static Queue<string> Que_GrpMoreDtl_Post = new Queue<string>();
        public static Queue<string> Que_GrpKey_Post = new Queue<string>();
        public static readonly object Locked_GrpPostTitle_Post = new object();
        public static readonly object Locked_GrpMoreDtl_Post = new object();
        public static readonly object Locked_GrpKey_Post = new object();

        public static Queue<string> Que_ComposeSub_Post = new Queue<string>();
        public static Queue<string> Que_ComposeBody_Post = new Queue<string>();

        public static readonly object Locked_ComposeSub_Post = new object();
        public static readonly object Locked_ComposeBody_Post = new object();

        public static int minDelay = 20;
        public static int maxDelay = 25;
        #endregion

        #region GroupStatus
        public GroupStatus()
        {
        } 
        #endregion

        #region GroupStatus
         public GroupStatus(string UserName, string Password, string ProxyAddress, string ProxyPort, string ProxyUserName, string ProxyPassword)
         {
             this.accountUser = UserName;
             this.accountPass = Password;
             this.proxyAddress = ProxyAddress;
             this.proxyPort = ProxyPort;
             this.proxyUserName = ProxyUserName;
             this.proxyPassword = ProxyPassword;

         } 
         #endregion

        #region StartPage
         public static int StartPage
         {
             get;
             set;
         } 
         #endregion

        #region EndPage
         public static int EndPage
         {
             get;
             set;
         } 
         #endregion
     
        #region PostCreateGroupNames
        
        public Dictionary<string, string> PostCreateGroupNames(ref GlobusHttpHelper HttpHelper, string user)
        {
           
            try
            {
                 string pageSource = HttpHelper.getHtmlfromUrl(new Uri("http://www.linkedin.com/grp/"));

                 
                if (pageSource == "" || pageSource.Contains("Make sure you have cookies and Javascript enabled in your browser before signing in") || pageSource.Contains("manual_redirect_link"))
                {
                    Thread.Sleep(2000);
                    pageSource = HttpHelper.getHtmlfromUrl(new Uri("http://www.linkedin.com/myGroups?trk=nav_responsive_sub_nav_groups"));
                }

                if (pageSource == "" || pageSource.Contains("Make sure you have cookies and Javascript enabled in your browser before signing in") || pageSource.Contains("manual_redirect_link"))
                {
                    Thread.Sleep(2000);
                    pageSource = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/grp/"));
                }

                if (pageSource == "" || pageSource.Contains("Make sure you have cookies and Javascript enabled in your browser before signing in") || pageSource.Contains("manual_redirect_link"))
                {
                    Thread.Sleep(2000);
                    pageSource = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/myGroups?trk=nav_responsive_sub_nav_groups"));
                }



                if (pageSource.Contains("media-block has-activity"))
                {

                    string[] RgxGroupData = Regex.Split(pageSource, "media-content");

                    foreach (var GrpName in RgxGroupData)
                    {
                        string endName = string.Empty;
                        string endKey = string.Empty;

                        try
                        {
                            if (GrpName.Contains("<!DOCTYPE html>") || GrpName.Contains("Membership Pending"))
                            {
                                continue;
                            }

                            if (GrpName.Contains("<a href=\"/groups/") || (GrpName.Contains("<a href=\"/groups?")))
                            {
                                if ((GrpName.Contains("public")))
                                {
                                    try
                                    {
                                        int startindex = GrpName.IndexOf("class=\"public\"");
                                        string start = GrpName.Substring(startindex);
                                        int endIndex = start.IndexOf("</a>");
                                        endName = start.Substring(0, endIndex).Replace("title", string.Empty).Replace("=", string.Empty).Replace(">", string.Empty).Replace("groups", string.Empty).Replace("/", string.Empty).Replace("\"", string.Empty).Replace("&amp;", "&").Replace("classpublic", string.Empty).Replace("&quot;", "'").Replace(":", ";").Replace("This is an open group", string.Empty);
                                        endName = endName + ':' + user;
                                    }
                                    catch (Exception ex)
                                    {
                                        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> GroupStatus --> PostCreateGroupNames() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update1 --> PostCreateGroupNames() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinGetGroupMemberErrorLogs);
                                    }

                                    try
                                    {
                                        endKey = "";
                                        int startindex1 = GrpName.IndexOf("group");
                                        string start1 = GrpName.Substring(startindex1);
                                        int endIndex1 = start1.IndexOf("?");
                                        endKey = start1.Substring(0, endIndex1).Replace("groups", string.Empty).Replace("/", string.Empty).Replace("<a href=", string.Empty).Replace("\"", string.Empty);

                                        if (endKey == string.Empty)
                                        {
                                            startindex1 = GrpName.IndexOf("gid=");
                                            start1 = GrpName.Substring(startindex1);
                                            endIndex1 = start1.IndexOf("&");
                                            endKey = start1.Substring(0, endIndex1).Replace("gid=", string.Empty).Trim();

                                            if (!NumberHelper.ValidateNumber(endKey))
                                            {
                                                try
                                                {
                                                    endKey = endKey.Split('\"')[0];
                                                }
                                                catch { }
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> GroupStatus2 --> PostCreateGroupNames() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update2 --> PostCreateGroupNames() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinGetGroupMemberErrorLogs);
                                    }

                                    if (endKey.Contains("analyticsURL"))
                                    {
                                        continue;
                                    }

                                    if (endKey == string.Empty)
                                    {
                                        try
                                        {
                                            int startindex2 = GrpName.IndexOf("gid=");
                                            string start2 = GrpName.Substring(startindex2);
                                            int endIndex2 = start2.IndexOf("&");
                                            endKey = start2.Substring(0, endIndex2).Replace("gid", string.Empty).Replace("=", string.Empty);
                                        }
                                        catch (Exception ex)
                                        {
                                            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> GroupStatus3 --> PostCreateGroupNames() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update3 --> PostCreateGroupNames() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinGetGroupMemberErrorLogs);
                                        }
                                    }
                                    else
                                    {
                                        string[] endKeyLast = endKey.Split('-');
                                        try
                                        {
                                            if (NumberHelper.ValidateNumber(endKeyLast[1]))
                                            {
                                                endKey = endKeyLast[1];
                                            }
                                            else if (NumberHelper.ValidateNumber(endKeyLast[2]))
                                            {
                                                endKey = endKeyLast[2];
                                            }
                                            else if (NumberHelper.ValidateNumber(endKeyLast[3]))
                                            {
                                                endKey = endKeyLast[3];
                                            }
                                            else if (NumberHelper.ValidateNumber(endKeyLast[4]))
                                            {
                                                endKey = endKeyLast[4];
                                            }
                                            else if (NumberHelper.ValidateNumber(endKeyLast[5]))
                                            {
                                                endKey = endKeyLast[5];
                                            }
                                            else if (NumberHelper.ValidateNumber(endKeyLast[6]))
                                            {
                                                endKey = endKeyLast[6];
                                            }
                                        }
                                        catch { }
                                    }

                                    try
                                    {
                                        if (NumberHelper.ValidateNumber(endKey))
                                        {
                                            GroupName.Add(endName, endKey);
                                        }
                                    }
                                    catch { }
                                }

                            }

                            if (GrpName.Contains("<a href=\"/groups/") || (GrpName.Contains("<a href=\"/groups?")))
                            {
                                if ((GrpName.Contains("private")))
                                {
                                    try
                                    {
                                        int startindex = GrpName.IndexOf("class=\"private\"");
                                        string start = GrpName.Substring(startindex);
                                        int endIndex = start.IndexOf("</a>");
                                        endName = start.Substring(0, endIndex).Replace("=", string.Empty).Replace(">", string.Empty).Replace("groups", string.Empty).Replace("/", string.Empty).Replace("\"", string.Empty).Replace("&amp;", "&").Replace("&quot;", "'").Replace(":", ";").Replace("classprivate", string.Empty);
                                        endName = endName + ':' + user;
                                    }
                                    catch (Exception ex)
                                    {
                                        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> GroupStatus4 --> PostCreateGroupNames() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update4 --> PostCreateGroupNames() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinGetGroupMemberErrorLogs);
                                    }

                                    try
                                    {
                                        int startindex1 = GrpName.IndexOf("gid=");
                                        string start1 = GrpName.Substring(startindex1);
                                        int endIndex1 = start1.IndexOf("&");
                                        endKey = start1.Substring(0, endIndex1).Replace("gid=", string.Empty).Replace("/", string.Empty).Replace("<a href=", string.Empty).Replace("\"", string.Empty).Replace("class=blanket-target><a>groups?home=", string.Empty).Trim();

                                        if (endKey == string.Empty)
                                        {
                                            try
                                            {
                                                endKey = endKey.Split(' ')[0].Trim();
                                            }
                                            catch { }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> GroupStatus5 --> PostCreateGroupNames() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update5 --> PostCreateGroupNames() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinGetGroupMemberErrorLogs);
                                    }

                                    if (endKey.Contains("analyticsURL"))
                                    {
                                        continue;
                                    }

                                    if (endKey == string.Empty)
                                    {
                                        try
                                        {
                                            int startindex2 = GrpName.IndexOf("gid=");
                                            string start2 = GrpName.Substring(startindex2);
                                            int endIndex2 = start2.IndexOf("&");
                                            endKey = start2.Substring(0, endIndex2).Replace("gid", string.Empty).Replace("=", string.Empty);

                                            if (!NumberHelper.ValidateNumber(endKey))
                                            {
                                                try
                                                {
                                                    endKey = endKey.Split('\"')[0];
                                                }
                                                catch { }
                                            }

                                        }
                                        catch (Exception ex)
                                        {
                                            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> GroupStatus6 --> PostCreateGroupNames() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update6 --> PostCreateGroupNames() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinGetGroupMemberErrorLogs);
                                        }
                                    }
                                    else
                                    {
                                        string[] endKeyLast = endKey.Split('-');
                                        try
                                        {
                                            if (NumberHelper.ValidateNumber(endKeyLast[1]))
                                            {
                                                endKey = endKeyLast[1];
                                            }
                                            else if (NumberHelper.ValidateNumber(endKeyLast[2]))
                                            {
                                                endKey = endKeyLast[2];
                                            }
                                            else if (NumberHelper.ValidateNumber(endKeyLast[3]))
                                            {
                                                endKey = endKeyLast[3];
                                            }
                                            else if (NumberHelper.ValidateNumber(endKeyLast[4]))
                                            {
                                                endKey = endKeyLast[4];
                                            }
                                            else if (NumberHelper.ValidateNumber(endKeyLast[5]))
                                            {
                                                endKey = endKeyLast[5];
                                            }
                                            else if (NumberHelper.ValidateNumber(endKeyLast[6]))
                                            {
                                                endKey = endKeyLast[6];
                                            }
                                        }
                                        catch { }
                                    }

                                    try
                                    {
                                        if (NumberHelper.ValidateNumber(endKey))
                                        {
                                            GroupName.Add(endName, endKey);
                                        }
                                    }
                                    catch { }
                                }

                            }
                        }
                        catch { }
                    }

                }
                else
                {
                    if (pageSource.Contains("View More</span>"))
                    {
                        pageSource = Utils.getBetween(pageSource, "<ul class=\"group-list\">", "View More</span>");
                    }
                    else
                    {
                        pageSource = Utils.getBetween(pageSource, "<ul class=\"group-list\">", "<li class=\"find-more\">");
                    }
                    string[] RgxGroupData = Regex.Split(pageSource, "<li class=");
                    RgxGroupData = RgxGroupData.Skip(1).ToArray();
                    foreach (var GrpName in RgxGroupData)
                    {
                        string endName = string.Empty;
                        string endKey = string.Empty;

                        try
                        {
                            if (GrpName.Contains("<!DOCTYPE html>") || GrpName.Contains("Pending"))
                            {
                                continue;
                            }

                            if (GrpName.Contains("<a href=\"/groups/") || (GrpName.Contains("<a href=\"/groups?")))
                            {
                                if ((GrpName.Contains("title")))
                                {
                                    try
                                    {
                                        int startindex = GrpName.IndexOf("class=\"title\"");
                                        string start = GrpName.Substring(startindex).Replace("class=\"title\"",string.Empty);
                                        int endIndex = start.IndexOf("<span");
                                        endName = start.Substring(0, endIndex).Replace("title", string.Empty).Replace("=", string.Empty).Replace(">", string.Empty).Replace("groups", string.Empty).Replace("/", string.Empty).Replace("\"", string.Empty).Replace("&amp;", "&").Replace("classpublic", string.Empty).Replace("&quot;", "'").Replace(":", ";").Replace("This is an open group", string.Empty).Replace("class", "").Replace("&#39;","'");
                                        if (endName.Contains("<div group-activity"))
                                        { 
                                            endIndex = endName.IndexOf("<h3");
                                            endName = endName.Substring(0, endIndex).Replace("<h3", "").Replace("<div group-activity", "");
                                        }
                                        endName = endName + ':' + user;
                                    }
                                    catch (Exception ex)
                                    {
                                        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> GroupStatus --> PostCreateGroupNames() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update1 --> PostCreateGroupNames() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinGetGroupMemberErrorLogs);
                                    }

                                    try
                                    {
                                        endKey = "";
                                        //int startindex1 = GrpName.IndexOf("group");
                                        //string start1 = GrpName.Substring(startindex1);
                                        //int endIndex1 = start1.IndexOf("?");
                                        //endKey = start1.Substring(0, endIndex1).Replace("groups", string.Empty).Replace("/", string.Empty).Replace("<a href=", string.Empty).Replace("\"", string.Empty);

                                        if (endKey == string.Empty)
                                        {
                                            int startindex1 = GrpName.IndexOf("gid=");
                                            string start1 = GrpName.Substring(startindex1);
                                            int endIndex1 = start1.IndexOf("&");
                                            endKey = start1.Substring(0, endIndex1).Replace("gid=", string.Empty).Trim();

                                            if (!NumberHelper.ValidateNumber(endKey))
                                            {
                                                try
                                                {
                                                    endKey = endKey.Split('\"')[0];
                                                }
                                                catch { }
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> GroupStatus2 --> PostCreateGroupNames() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update2 --> PostCreateGroupNames() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinGetGroupMemberErrorLogs);
                                    }

                                    if (endKey.Contains("analyticsURL"))
                                    {
                                        continue;
                                    }

                                    if (endKey == string.Empty)
                                    {
                                        try
                                        {
                                            int startindex2 = GrpName.IndexOf("gid=");
                                            string start2 = GrpName.Substring(startindex2);
                                            int endIndex2 = start2.IndexOf("&");
                                            endKey = start2.Substring(0, endIndex2).Replace("gid", string.Empty).Replace("=", string.Empty);
                                        }
                                        catch (Exception ex)
                                        {
                                            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> GroupStatus3 --> PostCreateGroupNames() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update3 --> PostCreateGroupNames() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinGetGroupMemberErrorLogs);
                                        }
                                    }
                                    else
                                    {
                                        string[] endKeyLast = endKey.Split('-');
                                        try
                                        {
                                            if (NumberHelper.ValidateNumber(endKeyLast[1]))
                                            {
                                                endKey = endKeyLast[1];
                                            }
                                            else if (NumberHelper.ValidateNumber(endKeyLast[2]))
                                            {
                                                endKey = endKeyLast[2];
                                            }
                                            else if (NumberHelper.ValidateNumber(endKeyLast[3]))
                                            {
                                                endKey = endKeyLast[3];
                                            }
                                            else if (NumberHelper.ValidateNumber(endKeyLast[4]))
                                            {
                                                endKey = endKeyLast[4];
                                            }
                                            else if (NumberHelper.ValidateNumber(endKeyLast[5]))
                                            {
                                                endKey = endKeyLast[5];
                                            }
                                            else if (NumberHelper.ValidateNumber(endKeyLast[6]))
                                            {
                                                endKey = endKeyLast[6];
                                            }
                                        }
                                        catch { }
                                    }

                                    try
                                    {
                                        if (NumberHelper.ValidateNumber(endKey))
                                        {
                                            GroupName.Add(endName, endKey);
                                        }
                                    }
                                    catch { }
                                }

                            }
                        }
                        catch { }
                    }
                }

                return GroupName;
            }
            catch (Exception ex)
            {
                return GroupName;
            }

        }
        #endregion

        #region PageNoSpecGroup
        public string PageNoSpecGroup(ref GlobusHttpHelper HttpHelper, string gid)
        {
            string pageSourcePage = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/groups?viewMembers=&gid=" + gid + "&split_page=1"));
            string[] RgxTotPages = System.Text.RegularExpressions.Regex.Split(pageSourcePage, "groups_members-h-mbr-cnt\"");

            foreach (var ItemData in RgxTotPages)
            {
                try
                {
                    if (ItemData.Contains("<!DOCTYPE html>"))
                    {
                        continue;
                    }
                    if (ItemData.Contains("class=\"member-count identified\">"))
                    {
                        int startindexName = ItemData.IndexOf("class=\"member-count identified\">");
                        string startName = ItemData.Substring(startindexName);
                        int endIndexName = startName.IndexOf("members</a>");
                        string page = startName.Substring(0, endIndexName).Replace("\"", string.Empty).Replace(">", string.Empty).Replace("class=member-count identified", string.Empty).Replace(",", string.Empty).Trim();
                        PageNo = int.Parse(page);
                        PageNo = (PageNo / 20) + 1;

                        //if (PageNo > 25)
                        //{
                        //    PageNo = 25;
                        //}
                    }
                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> GroupStatus --> PageNoSpecGroup() --> 1 >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> GroupStatus --> PageNoSpecGroup() --> 1 >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinGroupstatusErrorLogs);
                }
            }

            if (PageNo == 0)
            {
                RgxTotPages = System.Text.RegularExpressions.Regex.Split(pageSourcePage, "<h1 class=\"ptitle\">Members");

                foreach (var ItemData in RgxTotPages)
                {
                    try
                    {
                        if (ItemData.Contains("<!DOCTYPE html>"))
                        {
                            continue;
                        }

                        int startindexName = ItemData.IndexOf("<span>");
                        string startName = ItemData.Substring(startindexName);
                        int endIndexName = startName.IndexOf("</span>");
                        string page = startName.Substring(0, endIndexName).Replace("\"", string.Empty).Replace("<span>", string.Empty).Replace("class=member-count identified", string.Empty).Replace(",", string.Empty).Replace("(",string.Empty).Replace(")",string.Empty).Trim();
                        PageNo = int.Parse(page);
                        PageNo = (PageNo / 20) + 1;
                    }
                    catch (Exception ex)
                    {
                        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> GroupStatus --> PageNoSpecGroup() --> 1 >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> GroupStatus --> PageNoSpecGroup() --> 1 >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinGroupstatusErrorLogs);
                    }
                }
            }

            return Convert.ToString(PageNo);
        } 
        #endregion

        #region AddSpecificGroupUser
        public Dictionary<string, string> AddSpecificGroupUser(ref GlobusHttpHelper HttpHelper, string UserID, string gid)
        {
            string endName = string.Empty;
            string DeegreeConn = string.Empty;
            string endKey = string.Empty;
            string Locality = string.Empty;
            string Val_sourceAlias = string.Empty;
            string Val_key = string.Empty;
            string Val_defaultText = string.Empty;
            string Name = string.Empty;
            string Val_CsrToken = string.Empty;
            string Val_Subject = string.Empty;
            string Val_greeting = string.Empty;
            string Val_AuthToken = string.Empty;
            string Val_AuthType = string.Empty;
            string val_trk = string.Empty;
            string Val_lastName = string.Empty;
            string html = string.Empty;
            string Title = string.Empty;

            #region Data Initialization
            string GroupMemId = string.Empty;
            string GroupName = string.Empty;
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
            string csrfToken = string.Empty;
            string pageSource = string.Empty;
            string[] RgxSikValue = new string[] { };
            string[] RgxPageNo = new string[] { };
            string sikvalue = string.Empty;
            int pageno = 25;
            int counter = 0;
            #endregion

            try
            {
                GroupSpecMem.Clear();
                GroupName = gid.Split(':')[0];

                string pageSource1 = HttpHelper.getHtmlfromUrl(new Uri("http://www.linkedin.com/home?trk=hb_tab_home_top"));

                if (pageSource1.Contains("csrfToken"))
                {
                    csrfToken = pageSource1.Substring(pageSource1.IndexOf("csrfToken"), 50);
                    string[] Arr = csrfToken.Split('>');
                    csrfToken = Arr[0];
                    csrfToken = csrfToken.Replace(":", "%3A").Replace("csrfToken", "").Replace("\"", string.Empty).Replace("value", string.Empty).Replace("cs", string.Empty).Replace("id", string.Empty).Replace("=", string.Empty).Replace("\n", string.Empty).Replace(">", string.Empty).Replace("<script src", string.Empty);
                    csrfToken = csrfToken.Trim();
                }

              

                for (int i = 1; i <= pageno; i++)
                {
                    counter++;

                    string[] RgxGroupData = new string[] { };

                    if (WithGroupSearch == true)
                    {

                        string txid = (UnixTimestampFromDateTime(System.DateTime.Now) * 1000).ToString();

                        if (counter == 1)
                        {
                            pageSource = HttpHelper.getHtmlfromUrl(new Uri("http://www.linkedin.com/groups?viewMembers=&gid=" + gid.Split(':')[2]));
                            RgxSikValue = System.Text.RegularExpressions.Regex.Split(pageSource, "sik");
                            try
                            {
                                sikvalue = RgxSikValue[1].Split('&')[0].Replace("=", string.Empty);
                            }
                            catch { }

                            try
                            {
                                if (NumberHelper.ValidateNumber(sikvalue))
                                {
                                    sikvalue = sikvalue.Split('\"')[0];
                                }
                                else
                                {
                                    sikvalue = sikvalue.Split('\"')[0];
                                }
                            }
                            catch
                            {
                                sikvalue = sikvalue.Split('\"')[0];
                            }

                            if (!string.IsNullOrEmpty(sikvalue))
                            {
                                string postdata = "csrfToken=" + csrfToken + "&searchField=" + SearchKeyword + "&searchMembers=submit&searchMembers=Search&gid=" + gid.Split(':')[2] + "&goback=.gna_" + gid.Split(':')[2] + "";
                                pageSource = HttpHelper.postFormDataRef(new Uri("http://www.linkedin.com/groups"), postdata, "http://www.linkedin.com/groups?viewMembers=&gid=" + gid.Split(':')[2] + "&sik=" + txid + "&split_page=" + i + "&goback=%2Egna_" + gid.Split(':')[2] + "", "", "");

                            }
                            else
                            {
                                pageSource = HttpHelper.getHtmlfromUrl(new Uri("https://www.linkedin.com/grp/members?csrfToken="+csrfToken+"&search="+SearchKeyword.Replace(" ","+")+"&gid=" + gid.Split(':')[2]));
                            }
                        }

                        if (!string.IsNullOrEmpty(sikvalue))
                        {
                            if (counter > 1)
                            {

                                string getdata = "http://www.linkedin.com/groups?viewMembers=&gid=" + gid.Split(':')[2] + "&sik=" + txid + "&split_page=" + i + "&goback=%2Egna_" + gid.Split(':')[2] + "";
                                pageSource = HttpHelper.getHtmlfromUrl(new Uri(getdata));
                            }



                            RgxGroupData = System.Text.RegularExpressions.Regex.Split(pageSource, "<li class=\"member\" id=\"");


                            if (counter == 1)
                            {
                                try
                                {
                                    RgxPageNo = System.Text.RegularExpressions.Regex.Split(pageSource, "<h3 class=\"page-title\">Search Results: <span>");
                                    pageno = Convert.ToInt32(RgxPageNo[1].Split('<')[0].Replace("(", string.Empty).Replace(")", string.Empty).Replace("+", string.Empty).Trim());
                                    pageno = pageno / 20 + 1;
                                }
                                catch { }

                                if (pageno > 25)
                                {
                                    pageno = 25;
                                }
                            }
                        }
                        else
                        {
                           // RgxGroupData = System.Text.RegularExpressions.Regex.Split(pageSource, "<li class=\"member\">");
                            if (counter > 1)
                            {

                                string getdata = "https://www.linkedin.com/grp/members?csrfToken=" + csrfToken + "&search=" + SearchKeyword.Replace(" ", "+") + "&gid=" + gid.Split(':')[2] + "&page="+i;
                                pageSource = HttpHelper.getHtmlfromUrl(new Uri(getdata));
                            }

                            RgxGroupData = System.Text.RegularExpressions.Regex.Split(pageSource, "<li class=\"member\">");

                            if (counter == 1)
                            {
                                try
                                {
                                    RgxPageNo = System.Text.RegularExpressions.Regex.Split(pageSource, "<h3 class=\"page-title\">Search Results <span>");
                                    pageno = Convert.ToInt32(RgxPageNo[1].Split('<')[0].Replace("(", string.Empty).Replace(")", string.Empty).Replace("+", string.Empty).Trim());
                                    pageno = pageno / 20 + 1;
                                }
                                catch { }

                                if (pageno > 25)
                                {
                                    pageno = 25;
                                }
                            }
                        }
                    }
                    else
                    {
                        //pageSource = HttpHelper.getHtmlfromUrl(new Uri("http://www.linkedin.com/groups?viewMembers=&gid=" + gid.Split(':')[2] + "&split_page=" + i + ""));
                        pageSource = HttpHelper.getHtmlfromUrl(new Uri("https://www.linkedin.com/grp/members?gid=" + gid.Split(':')[2] + "&page=" + i));

                        
                        RgxGroupData = System.Text.RegularExpressions.Regex.Split(pageSource, "<li class=\"member\" id=\"");
                        if (RgxGroupData.Length == 1)
                        {
                            RgxGroupData = Regex.Split(pageSource, "<span class=\"new-miniprofile-container\"");
                            if (RgxGroupData.Length == 1)
                            {
                                break;
                            }
                        }
                    }

                    #region for csv
                    if (WithCsvinAddFriend == true)
                    {
                        List<string> PageSerchUrl = ChilkatBasedRegex.GettingAllUrls(pageSource, "profile/view?id");
                        string FrnAcceptUrL = string.Empty;
                        foreach (string item in PageSerchUrl)
                        {
                            try
                            {
                                if (item.Contains("/profile/view?id"))
                                {
                                    FrnAcceptUrL = "http://www.linkedin.com" + item;
                                    string[] urll = Regex.Split(FrnAcceptUrL, "&authType");
                                    Log("[ " + DateTime.Now + " ] => [ " + FrnAcceptUrL + " ]");

                                    string stringSource = HttpHelper.getHtmlfromUrl1(new Uri(FrnAcceptUrL));

                                    #region GroupMemId
                                    try
                                    {
                                        string[] gid1 = FrnAcceptUrL.Split('&');
                                        GroupMemId = gid1[0].Replace("http://www.linkedin.com/profile/view?id=", string.Empty);
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
                                                strFamilyName = stringSource.Substring(stringSource.IndexOf("i18n__Overview_for_X"), (stringSource.IndexOf(",", stringSource.IndexOf("i18n__Overview_for_X")) - stringSource.IndexOf("i18n__Overview_for_X"))).Replace("i18n__Overview_for_X", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":Overview for", "").Trim();

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
                                    catch { }
                                    #endregion

                                    #region LastName



                                    try
                                    {
                                        lastname = NameArr[1];

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
                                    Company = string.Empty;
                                    try
                                    {
                                        try
                                        {
                                            try
                                            {
                                                //Company = stringSource.Substring(stringSource.IndexOf("visible\":true,\"memberHeadline"), (stringSource.IndexOf("memberID", stringSource.IndexOf("visible\":true,\"memberHeadline")) - stringSource.IndexOf("visible\":true,\"memberHeadline"))).Replace("visible\":true,\"memberHeadline", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Replace("   ", string.Empty).Trim();
                                                Company = stringSource.Substring(stringSource.IndexOf("\"memberHeadline"), (stringSource.IndexOf("memberID", stringSource.IndexOf("\"memberHeadline")) - stringSource.IndexOf("\"memberHeadline"))).Replace("\"memberHeadline", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Replace("i18n__LocationLocationcompletenessLevel4", string.Empty).Replace("visibletrue", "").Replace("isPortfoliofalse", string.Empty).Replace("isLNLedtrue", string.Empty).Trim();
                                            }
                                            catch
                                            {
                                            }

                                            if (string.IsNullOrEmpty(Company))
                                            {
                                                try
                                                {
                                                    //memberHeadline
                                                    Company = stringSource.Substring(stringSource.IndexOf("memberHeadline\":"), (stringSource.IndexOf(",", stringSource.IndexOf("memberHeadline\":")) - stringSource.IndexOf("memberHeadline\":"))).Replace("memberHeadline\":", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Replace("   ", string.Empty).Replace(":", "").Replace("&dsh;", "").Replace("&amp", "").Replace(";", "").Replace("isPortfoliofalse", string.Empty).Replace("isLNLedtrue", string.Empty).Trim();
                                                }
                                                catch
                                                {
                                                }

                                            }

                                            titlecurrent = string.Empty;
                                            companycurrent = string.Empty;
                                            string[] strdesigandcompany = new string[4];
                                            if (Company.Contains(" at ") || Company.Contains(" of "))
                                            {
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

                                            if (titlecurrent == string.Empty)
                                            {
                                                titlecurrent = Company;
                                            }
                                        }
                                        catch { }

                                        #region PastCompany
                                        string[] companylist = Regex.Split(stringSource, "companyName\"");
                                        string AllComapny = string.Empty;

                                        string Companyname = string.Empty;
                                        checkerlst.Clear();
                                        foreach (string item1 in companylist)
                                        {
                                            try
                                            {
                                                if (!item1.Contains("<!DOCTYPE html>"))
                                                {
                                                    Companyname = item1.Substring(item1.IndexOf(":"), (item1.IndexOf(",", item1.IndexOf(":")) - item1.IndexOf(":"))).Replace(":", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Replace("]", string.Empty).Replace("}", string.Empty).Trim();
                                                    //Checklist.Add(item);
                                                    string items = item1;
                                                    checkerlst.Add(Companyname);
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
                                                AllComapny = item1.Replace("}", "").Replace("]", "").Replace("&amp;", "&");
                                            }
                                            else
                                            {
                                                AllComapny = AllComapny + " : " + item1.Replace("}", "").Replace("]", "").Replace("&amp;", "&");
                                            }
                                        }

                                        if (companycurrent == string.Empty)
                                        {
                                            companycurrent = checkerlst[0].ToString();
                                        }
                                        #endregion

                                    #endregion Company

                                        #region Company Descripription

                                        try
                                        {
                                            string[] str_CompanyDesc = Regex.Split(stringSource, "showSummarySection");

                                            foreach (string item2 in str_CompanyDesc)
                                            {
                                                try
                                                {
                                                    string Current_Company = string.Empty;
                                                    if (!item2.Contains("<!DOCTYPE html>"))
                                                    {
                                                        int startindex = item2.IndexOf("specialties\":\"");

                                                        if (startindex > 0)
                                                        {
                                                            try
                                                            {
                                                                string start = item2.Substring(startindex).Replace("specialties\":", "");
                                                                int endindex = start.IndexOf("\",\"associatedWith\"");
                                                                string end = start.Substring(0, endindex);
                                                                Current_Company = end.Replace(",\"specialties_lb\":", string.Empty).Replace("\"", string.Empty).Replace("summary_lb", "Summary").Replace(",", ";").Replace("\"u002", "-");
                                                                LDS_BackGround_Summary = Current_Company;
                                                            }
                                                            catch { }
                                                        }

                                                    }

                                                    if (!item2.Contains("<!DOCTYPE html>"))
                                                    {
                                                        int startindex = item2.IndexOf("\"summary_lb\"");

                                                        if (startindex > 0)
                                                        {
                                                            try
                                                            {
                                                                string start = item2.Substring(startindex).Replace("\"summary_lb\"", "");
                                                                int endindex = start.IndexOf("\",\"associatedWith\"");
                                                                string end = start.Substring(0, endindex);
                                                                Current_Company = end.Replace(",\"specialties_lb\":", string.Empty).Replace("<br>", string.Empty).Replace("\n\"", string.Empty).Replace("\"", string.Empty).Replace("summary_lb", "Summary").Replace(",", ";").Replace("u002", "-").Replace(":", string.Empty);
                                                                LDS_BackGround_Summary = Current_Company;
                                                            }
                                                            catch { }
                                                        }

                                                    }

                                                }
                                                catch { }
                                            }
                                        }
                                        catch { }

                                        #endregion

                                        #region Education
                                        EducationCollection = string.Empty;
                                        try
                                        {
                                            try
                                            {
                                                EducationCollection = stringSource.Substring(stringSource.IndexOf("\"schoolName\":"), (stringSource.IndexOf(",", stringSource.IndexOf("\"schoolName\":")) - stringSource.IndexOf("\"schoolName\":"))).Replace("\"schoolName\":", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Trim();
                                            }
                                            catch
                                            {
                                                try
                                                {
                                                    //  education1 = stringSource.Substring(stringSource.IndexOf("i18n__Overview_for_X"), (stringSource.IndexOf(",", stringSource.IndexOf("i18n__Overview_for_X")) - stringSource.IndexOf("i18n__Overview_for_X"))).Replace("i18n__Overview_for_X", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":Overview for", "").Trim();

                                                }
                                                catch { }
                                            }
                                        }
                                        catch { }

                                        #endregion

                                        #region Email
                                        try
                                        {
                                            string[] str_Email = Regex.Split(stringSource, "email\"");
                                            USERemail = stringSource.Substring(stringSource.IndexOf("[{\"email\":"), (stringSource.IndexOf("}]", stringSource.IndexOf("[{\"email\":")) - stringSource.IndexOf("[{\"email\":"))).Replace("[{\"email\":", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Trim();
                                        }
                                        catch (Exception ex)
                                        {

                                        }
                                        #endregion Email

                                        #region Website
                                        Website = string.Empty;
                                        try
                                        {
                                            Website = stringSource.Substring(stringSource.IndexOf("[{\"URL\":"), (stringSource.IndexOf(",", stringSource.IndexOf("[{\"URL\":")) - stringSource.IndexOf("[{\"URL\":"))).Replace("[{\"URL\":", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace("}]", string.Empty).Trim();
                                        }
                                        catch { }
                                        #endregion Website

                                        #region location
                                        location = string.Empty;
                                        try
                                        {
                                            location = stringSource.Substring(stringSource.IndexOf("Country\",\"fmt__location\":"), (stringSource.IndexOf("i18n_no_location_matches", stringSource.IndexOf("Country\",\"fmt__location\":")) - stringSource.IndexOf("Country\",\"fmt__location\":"))).Replace("Country\",\"fmt__location\":", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(":", string.Empty).Replace(",", string.Empty).Trim();
                                        }
                                        catch (Exception ex)
                                        {
                                            try
                                            {
                                                if (string.IsNullOrEmpty(location))
                                                {
                                                    int startindex = stringSource.IndexOf("\"fmt_location\":\"");
                                                    if (startindex > 0)
                                                    {
                                                        string start = stringSource.Substring(startindex).Replace("\"fmt_location\":\"", "");
                                                        int endindex = start.IndexOf("\",");
                                                        string end = start.Substring(0, endindex);
                                                        country = end;
                                                    }
                                                }
                                            }
                                            catch (Exception ex1)
                                            {

                                            }
                                        }

                                        #endregion location

                                        #region Country
                                        try
                                        {
                                            int startindex = stringSource.IndexOf("\"locationName\":\"");
                                            if (startindex > 0)
                                            {
                                                string start = stringSource.Substring(startindex).Replace("\"locationName\":\"", "");
                                                int endindex = start.IndexOf("\",");
                                                string end = start.Substring(0, endindex);
                                                location = end;
                                            }
                                        }
                                        catch (Exception ex)
                                        {

                                        }
                                        #endregion

                                        #region Industry
                                        Industry = string.Empty;
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
                                        #endregion Industry

                                        #region Connection
                                        Connection = string.Empty;
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
                                        #endregion Connection

                                        #region Recommendation
                                        try
                                        {
                                            //recomandation = stringSource.Substring(stringSource.IndexOf("i18n__Recommend_Query\":"), (stringSource.IndexOf(",", stringSource.IndexOf("i18n__Recommend_Query\":")) - stringSource.IndexOf("i18n__Recommend_Query\":"))).Replace("i18n__Recommend_Query\":", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Trim();         
                                            string PageSource = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/profile/profile-v2-endorsements?id=" + GroupMemId + "&authType=OUT_OF_NETWORK&authToken=rXRG&goback=%2Efps_PBCK_*1_*1_*1_*1_*1_*1_tcs_*2_CP_I_us_*1_*1_false_1_R_*1_*51_*1_*51_true_*1_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2"));
                                            string[] arrayRecommendedName = Regex.Split(PageSource, "headline");
                                            List<string> ListRecommendationName = new List<string>();
                                            recomandation = string.Empty;
                                            foreach (var itemRecomName in arrayRecommendedName)
                                            {
                                                try
                                                {
                                                    if (!itemRecomName.Contains("Endorsements"))
                                                    {
                                                        try
                                                        {

                                                            int startindex = itemRecomName.IndexOf(":");
                                                            string start = itemRecomName.Substring(startindex);
                                                            int endIndex = start.IndexOf("\",");
                                                            string Heading = (start.Substring(0, endIndex).Replace("\"", string.Empty).Replace(":", string.Empty));

                                                            int startindex1 = itemRecomName.IndexOf("fmt__referrerfullName");
                                                            string start1 = itemRecomName.Substring(startindex1);
                                                            int endIndex1 = start1.IndexOf("\",");
                                                            Name = (start1.Substring(0, endIndex1).Replace("\"", string.Empty).Replace("fmt__referrerfullName", string.Empty).Replace(":", string.Empty));

                                                            ListRecommendationName.Add(Name + " : " + Heading);
                                                        }
                                                        catch { }
                                                    }
                                                }
                                                catch { }

                                            }

                                            foreach (var item5 in ListRecommendationName)
                                            {
                                                if (recomandation == string.Empty)
                                                {
                                                    recomandation = item5;
                                                }
                                                else
                                                {
                                                    recomandation += "  -  " + item5;
                                                }
                                            }

                                        }
                                        catch { }
                                        #endregion

                                        #region Experience
                                        LDS_Experience = string.Empty;
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

                                                foreach (var itemExp in ListExperince)
                                                {
                                                    if (LDS_Experience == string.Empty)
                                                    {
                                                        LDS_Experience = itemExp;
                                                    }
                                                    else
                                                    {
                                                        LDS_Experience += "  -  " + itemExp;
                                                    }
                                                }

                                            }
                                            catch { }
                                        }

                                        #endregion

                                        #region Group
                                        try
                                        {
                                            string PageSource = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/profile/mappers?x-a=profile_v2_groups%2Cprofile_v2_follow%2Cprofile_v2_connections&x-p=profile_v2_discovery%2Erecords%3A4%2Ctop_card%2EprofileContactsIntegrationStatus%3A0%2Cprofile_v2_comparison_insight%2Edistance%3A1%2Cprofile_v2_right_fixed_discovery%2Eoffset%3A0%2Cprofile_v2_connections%2Edistance%3A1%2Cprofile_v2_right_fixed_discovery%2Erecords%3A4%2Cprofile_v2_network_overview_insight%2Edistance%3A1%2Cprofile_v2_right_top_discovery_teamlinkv2%2Eoffset%3A0%2Cprofile_v2_right_top_discovery_teamlinkv2%2Erecords%3A4%2Cprofile_v2_discovery%2Eoffset%3A0%2Cprofile_v2_summary_upsell%2EsummaryUpsell%3Atrue%2Cprofile_v2_network_overview_insight%2EnumConn%3A1668%2Ctop_card%2Etc%3Atrue&x-oa=bottomAliases&id=" + GroupMemId + "&locale=&snapshotID=&authToken=&authType=name&invAcpt=&notContactable=&primaryAction=&isPublic=false&sfd=true&_=1366115853014"));

                                            string[] array = Regex.Split(PageSource, "href=\"/groupRegistration?");
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

                                            foreach (var item6 in ListGroupName)
                                            {
                                                if (groupscollectin == string.Empty)
                                                {
                                                    groupscollectin = item6;
                                                }
                                                else
                                                {
                                                    groupscollectin += "  -  " + item6;

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
                                                foreach (string item7 in strarr_skill)
                                                {
                                                    try
                                                    {
                                                        if (!item7.Contains("!DOCTYPE html"))
                                                        {
                                                            try
                                                            {
                                                                string Grp = item7.Substring(item7.IndexOf("<"), (item7.IndexOf(">", item7.IndexOf("<")) - item7.IndexOf("<"))).Replace("<", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Trim();
                                                                checkgrplist.Add(Grp);
                                                                checkgrplist.Distinct().ToList();
                                                            }
                                                            catch { }
                                                        }

                                                    }
                                                    catch { }
                                                }

                                                foreach (string item8 in checkgrplist)
                                                {
                                                    if (string.IsNullOrEmpty(Skill))
                                                    {
                                                        Skill = item8;
                                                    }
                                                    else
                                                    {
                                                        Skill = Skill + "  -  " + item8;
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

                                                        foreach (string item9 in checkgrplist)
                                                        {
                                                            if (string.IsNullOrEmpty(Skill))
                                                            {
                                                                Skill = item9;
                                                            }
                                                            else
                                                            {
                                                                Skill = Skill + "  -  " + item9;
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

                                        #endregion

                                        #region Pasttitle and All Company Summary
                                        string[] pasttitles = Regex.Split(stringSource, "company_name");
                                        string pstTitlesitem = string.Empty;
                                        string pstDescCompitem = string.Empty;
                                        LDS_PastTitles = string.Empty;
                                        pasttitles = pasttitles.Skip(1).ToArray();
                                        foreach (string item10 in pasttitles)
                                        {
                                            if (item10.Contains("positionId"))
                                            {
                                                try
                                                {
                                                    int startindex = item10.IndexOf(":");
                                                    if (startindex > 0)
                                                    {
                                                        string start = item10.Substring(startindex).Replace(":\"", "");
                                                        int endindex = start.IndexOf("\",");
                                                        string end = start.Substring(0, endindex);
                                                        pstTitlesitem = end.Replace(",", ";");
                                                    }

                                                    if (string.IsNullOrEmpty(LDS_PastTitles))
                                                    {
                                                        LDS_PastTitles = pstTitlesitem;
                                                    }
                                                    else
                                                    {
                                                        LDS_PastTitles = LDS_PastTitles + "  :  " + pstTitlesitem;
                                                    }


                                                    int startindex1 = item10.IndexOf("summary_lb\":\"");
                                                    if (startindex > 0)
                                                    {
                                                        string start1 = item10.Substring(startindex1).Replace("summary_lb\":\"", "");
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
                                                        pstDescCompitem = end1.Replace(",", ";").Replace("u002d", "-").Replace("<br>", string.Empty).Replace("\n\"", string.Empty);

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

                                        #region FullUrl
                                        try
                                        {
                                            string[] UrlFull = System.Text.RegularExpressions.Regex.Split(FrnAcceptUrL, "&authType");
                                            LDS_UserProfileLink = UrlFull[0];

                                            LDS_UserProfileLink = UrlFull[0];
                                            //  LDS_UserProfileLink = stringSource.Substring(stringSource.IndexOf("canonicalUrl\":"), (stringSource.IndexOf(",", stringSource.IndexOf("canonicalUrl\":")) - stringSource.IndexOf("canonicalUrl\":"))).Replace("canonicalUrl\":", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Trim();
                                        }
                                        catch { }
                                        #endregion

                                        LDS_LoginID = UserID;

                                        if (string.IsNullOrEmpty(firstname))
                                        {
                                            firstname = "Linkedin Member";
                                        }

                                        LDS_BackGround_Summary = LDS_BackGround_Summary.Replace("\n", "").Replace("-", "").Replace("d", "").Replace("&#x2022", "").Replace(";", "").Replace("\n", "").Replace(",", "").Replace("&#x201", "").Trim();

                                        LDS_Desc_AllComp = "NA";
                                        Skill = "NA";

                                        string LDS_FinalData = TypeOfProfile + "," + LDS_UserProfileLink + "," + firstname + "," + lastname + "," + Company.Replace(",", ";") + "," + titlecurrent.Replace(",", ";") + "," + companycurrent.Replace(",", ";") + "," + LDS_Desc_AllComp + "," + LDS_BackGround_Summary.Replace(",", ";") + "," + Connection.Replace(",", ";") + "," + recomandation.Replace(",", string.Empty) + "," + Skill.Replace(",", ";") + "," + LDS_Experience.Replace(",", string.Empty) + "," + EducationCollection.Replace(",", ";") + "," + groupscollectin.Replace(",", ";") + "," + USERemail.Replace(",", ";") + "," + LDS_UserContact.Replace(",", ";") + "," + LDS_PastTitles + "," + AllComapny.Replace(",", ";") + "," + country.Replace(",", ";") + "," + location.Replace(",", ";") + "," + Industry.Replace(",", ";") + "," + Website.Replace(",", ";") + "," + LDS_LoginID.Replace(",", ";") + ",";
                                        AppFileHelper.AddingLinkedInGroupMemberDataToCSVFile(LDS_FinalData, SearchCriteria.FileName);

                                    }
                                    catch { }
                                }

                            }
                            catch { }
                        }
                    }
                    #endregion

                    foreach (var GrpUser in RgxGroupData)
                    {
                       try
                        {
                            if (GrpUser.Contains("member"))
                            {
                                if (GrpUser.Contains("title=\"YOU") || GrpUser.Contains("<!DOCTYPE html>"))
                                {
                                    if (GrpUser.Contains("title=\"YOU"))
                                    {

                                    }
                                    continue;
                                }

                                try
                                {
                                    //data-li-fullName="Kashish Arora">Send message</a>

                                    int startindex = GrpUser.IndexOf("fullName=");
                                    if (startindex > 0)
                                    {
                                        endName = string.Empty;
                                        string start = GrpUser.Substring(startindex);
                                        int endIndex = start.IndexOf(">Send message<");
                                        if (endIndex == -1)
                                        {
                                            endIndex = start.IndexOf(">");
                                        }
                                        endName = start.Substring(0, endIndex).Replace("fullName=", string.Empty).Replace("'", string.Empty).Replace(",", string.Empty).Replace("/", string.Empty).Replace("\"", string.Empty).Replace("&amp;", "&").Replace("&quot;", "'").Replace("tracking=anetppl_sendmsg", string.Empty).Replace("tracking=anetppl_invite", string.Empty).Replace("\\u00e9","é").Trim();
                                    }
                                    else
                                    {
                                        endName = string.Empty;
                                        int startindex1 = GrpUser.IndexOf("alt=");
                                        string start = GrpUser.Substring(startindex1).Replace("alt=\"", "");
                                        int endIndex = start.IndexOf("\"");
                                        try
                                        {
                                            endName = start.Substring(0, endIndex).Replace("alt=", string.Empty).Replace("'", string.Empty).Replace(",", string.Empty).Replace(">", string.Empty).Replace("\"", string.Empty).Replace("&amp;", "&").Replace("&quot;", "'").Replace("tracking=anetppl_sendmsg", string.Empty).Replace("tracking=anetppl_invite", string.Empty).Replace("\\u00e9", "é").Trim();
                                        }
                                        catch { }
                                        try
                                        {
                                            if (string.IsNullOrEmpty(endName))
                                            {
                                                endName = start.Substring(start.IndexOf("alt="), start.IndexOf("class=", start.IndexOf("alt=")) - start.IndexOf("alt=")).Replace("alt=", string.Empty).Replace("alt=", string.Empty).Replace("\"", string.Empty).Replace("&quot;", "'").Replace("tracking=anetppl_sendmsg", string.Empty).Replace("tracking=anetppl_invite", string.Empty).Replace("\\u00e9", "é").Trim();
                                            }

                                        }
                                        catch { }
                                    }
                                }
                                catch
                                {

                                }

                                //Deegree connection
                                try
                                {
                                    int startindex = GrpUser.IndexOf("<span class=\"degree-icon\">");
                                    if (startindex > 0)
                                    {
                                        DeegreeConn = string.Empty;
                                        string start = GrpUser.Substring(startindex);
                                        int endIndex = start.IndexOf("<sup>");
                                        DeegreeConn = start.Substring(0, endIndex).Replace("<span class=\"degree-icon\">", string.Empty);

                                        if (DeegreeConn == "1")
                                        {
                                            DeegreeConn = DeegreeConn + "st";
                                        }
                                        else if (DeegreeConn == "2")
                                        {
                                            DeegreeConn = DeegreeConn + "nd";
                                        }
                                        else if (DeegreeConn == "3")
                                        {
                                            DeegreeConn = DeegreeConn + "rd";
                                        }
                                    }
                                    else
                                    {
                                        startindex = GrpUser.IndexOf("span class=\"degree-icon group\">");
                                        DeegreeConn = string.Empty;

                                        if (startindex > 0)
                                        {
                                            DeegreeConn = string.Empty;
                                            string start = GrpUser.Substring(startindex);
                                            int endIndex = start.IndexOf("</span>");
                                            DeegreeConn = start.Substring(0, endIndex).Replace("span class=\"degree-icon group\">", string.Empty);

                                        }

                                        if (DeegreeConn == string.Empty)
                                        {

                                            DeegreeConn = "3rd";
                                        }
                                    }

                                }

                                catch { }

                                try
                                {
                                    int startindex2 = GrpUser.IndexOf("memberId=");
                                    if (startindex2 > 0)
                                    {
                                        endKey = string.Empty;
                                        string start1 = GrpUser.Substring(startindex2);
                                        int endIndex1 = start1.IndexOf("data-li-fullName=");
                                        endKey = start1.Substring(0, endIndex1).Replace("memberId=", string.Empty).Replace("'", string.Empty).Replace(",", string.Empty).Replace("/", string.Empty).Replace("\"", string.Empty).Trim();
                                    }
                                    else
                                    {
                                        endKey = string.Empty;
                                        int startindex3 = GrpUser.IndexOf("member-");
                                        string start1 = GrpUser.Substring(startindex3);
                                        int endIndex1 = start1.IndexOf(">");
                                        endKey = start1.Substring(0, endIndex1).Replace("member-", string.Empty).Replace("'", string.Empty).Replace(",", string.Empty).Replace(">", string.Empty).Replace("\"", string.Empty).Trim();
                                    }
                                }
                                catch
                                {

                                }

                                try
                                {
                                    GroupSpecMem.Add(endKey, " [" + GroupName.Replace(",", string.Empty) + " ] " + endName + " (" + DeegreeConn.Replace(",", string.Empty) + ")");
                                    string item = UserID + "," + GroupName.Replace(",", string.Empty) + "," + endName + "," + DeegreeConn.Replace(",", string.Empty);
                                    AddingLinkedInDataToCSVFile1(item);
                                    if (WithGroupSearch == true)
                                    {
                                        Loggergrppmem("[ " + DateTime.Now + " ] => [ Added Group Member : " + endName + " (" + DeegreeConn + ") with Search keyword : " + SearchKeyword + " ]");
                                    }
                                    else
                                    {
                                        Loggergrppmem("[ " + DateTime.Now + " ] => [ Added Group Member : " + endName + " ]");
                                    }
                                }
                                catch { }
                            }
                            else
                            {

                            }
                        }
                        catch { }
                    }

                }
                return GroupSpecMem;
            }
            catch { }
            return GroupSpecMem;
        }
        #endregion

        
        public static long UnixTimestampFromDateTime(DateTime date)
        {
            long unixTimestamp = date.Ticks - new DateTime(1970, 1, 1).Ticks;
            unixTimestamp /= TimeSpan.TicksPerSecond;
            return unixTimestamp;
        }

        #region AddingLinkedInDataToCSVFile
        public static void AddingLinkedInDataToCSVFile1(string Data)
        {
            try
            {
                //string LinkedInAppData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "InBoardProGetData\\" + SearchCriteria.FileName + ".csv");
                string LinkedInDeskTop = Globals.DesktopFolder + "\\GroupMemberList.csv";

                #region LinkedIn Writer

                //Checking File Exixtance
                if (!File.Exists(LinkedInDeskTop))
                {
                    string Header = "Account" + "," + "GroupName" + "," + "ContactPerson" + "," + "Degree of Connection";
                    GlobusFileHelper.AppendStringToTextfileNewLine(Header, LinkedInDeskTop);
                }

                if (!string.IsNullOrEmpty(Data))
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine(Data, LinkedInDeskTop);
                }
                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        #endregion


        #region AddSpecificGroupUserWithExcelInput
        public Dictionary<string, string> AddSpecificGroupUserWithExcelInput(ref GlobusHttpHelper HttpHelper, string UserID, string gid)
        {
            string endName = string.Empty;
            string DeegreeConn = string.Empty;
            string endKey = string.Empty;
            string Locality = string.Empty;
            string Val_sourceAlias = string.Empty;
            string Val_key = string.Empty;
            string Val_defaultText = string.Empty;
            string Name = string.Empty;
            string Val_CsrToken = string.Empty;
            string Val_Subject = string.Empty;
            string Val_greeting = string.Empty;
            string Val_AuthToken = string.Empty;
            string Val_AuthType = string.Empty;
            string val_trk = string.Empty;
            string Val_lastName = string.Empty;
            string html = string.Empty;
            string Title = string.Empty;

            #region Data Initialization
            string csrfToken = string.Empty;
            string pageSource = string.Empty;
            string[] RgxSikValue = new string[] { };
            string[] RgxPageNo = new string[] { };
            string sikvalue = string.Empty;
            int counter = 0;
            #endregion

            try
            {
                GroupSpecMem.Clear();
                string pageSource1 = HttpHelper.getHtmlfromUrl(new Uri("http://www.linkedin.com/home?trk=hb_tab_home_top"));

                if (pageSource1.Contains("csrfToken"))
                {
                    csrfToken = pageSource1.Substring(pageSource1.IndexOf("csrfToken"), 50);
                    string[] Arr = csrfToken.Split('>');
                    csrfToken = Arr[0];
                    csrfToken = csrfToken.Replace(":", "%3A").Replace("csrfToken", "").Replace("\"", string.Empty).Replace("value", string.Empty).Replace("cs", string.Empty).Replace("id", string.Empty).Replace("=", string.Empty).Replace("\n", string.Empty).Replace(">", string.Empty).Replace("<script src", string.Empty);
                    csrfToken = csrfToken.Trim();
                }

           
                    string[] RgxGroupData = new string[] { };
             
                    foreach (string[] itemArr in msgGroupMem_excelData)
                    {
                        try
                        {
                            pageSource = HttpHelper.getHtmlfromUrl(new Uri("http://www.linkedin.com/groups?viewMembers=&gid=" + gid));
                        }
                        catch { }

                        string group = string.Empty;
                        string mem = string.Empty;

                        try
                        {
                            group = itemArr[0].ToString();
                            mem = itemArr[1].ToString();
                            endKey = itemArr[1].ToString();
                        }
                        catch { }

                        if (!string.IsNullOrEmpty(group) || !string.IsNullOrEmpty(mem))
                        {

                            try
                            {
                                RgxSikValue = System.Text.RegularExpressions.Regex.Split(pageSource, "sik");
                            }
                            catch { }

                            try
                            {
                                sikvalue = RgxSikValue[1].Split('&')[0].Replace("=", string.Empty);
                            }
                            catch { }

                            try
                            {
                                if (NumberHelper.ValidateNumber(sikvalue))
                                {
                                    sikvalue = sikvalue.Split('\"')[0];
                                }
                                else
                                {
                                    sikvalue = sikvalue.Split('\"')[0];
                                }
                            }
                            catch
                            {
                                sikvalue = sikvalue.Split('\"')[0];
                            }

                            string postdata = "csrfToken=" + csrfToken + "&searchField=" + Uri.EscapeDataString(mem) + "&searchMembers=submit&searchMembers=Search&gid=" + gid + "&goback=.gna_" + gid + "";

                            #region Commented old code
                            try
                            {
                                pageSource = HttpHelper.postFormDataRef(new Uri("http://www.linkedin.com/groups"), postdata, "http://www.linkedin.com/groups?viewMembers=&gid=" + gid + "&sik=" + sikvalue + "&split_page=1&goback=%2Egna_" + gid + "", "", "");
                            }
                            catch { }

                            if (pageSource.Contains("Sorry, we found 0 members matching your search."))
                            {
                                Loggergrppmem("[ " + DateTime.Now + " ] => [ Sorry, we found 0 members matching your search : " + mem + " ]");
                                continue;
                            }

                            if (counter > 1)
                            {
                                RgxSikValue = System.Text.RegularExpressions.Regex.Split(pageSource, "sik");
                                try
                                {
                                    sikvalue = RgxSikValue[1].Split('&')[0].Replace("=", string.Empty);
                                }
                                catch { }

                                try
                                {
                                    if (NumberHelper.ValidateNumber(sikvalue))
                                    {
                                        sikvalue = sikvalue.Split('\"')[0];
                                    }
                                    else
                                    {
                                        sikvalue = sikvalue.Split('\"')[0];
                                    }
                                }
                                catch
                                {
                                    sikvalue = sikvalue.Split('\"')[0];
                                }

                                string getdata = "http://www.linkedin.com/groups?viewMembers=&gid=" + gid + "&sik=" + sikvalue + "&split_page=1&goback=%2Egna_" + gid + "";

                                try
                                {
                                    pageSource = HttpHelper.getHtmlfromUrl(new Uri(getdata));
                                }
                                catch { }
                            }


                            try
                            {
                                RgxGroupData = System.Text.RegularExpressions.Regex.Split(pageSource, "<li class=\"member\" id=\"");
                            }
                            catch { } 
                            #endregion


                            string Url = endKey;
                            //var GrpUser = HttpHelper.getHtmlfromUrl(new Uri(Url));
                            foreach (var GrpUser in RgxGroupData)
                            {
                                try
                                {
                                    if (GrpUser.Contains("member"))
                                    {
                                        if (GrpUser.Contains("title=\"YOU") || GrpUser.Contains("<!DOCTYPE html>"))
                                        {
                                            if (GrpUser.Contains("title=\"YOU"))
                                            {

                                            }
                                            continue;
                                        }

                                        try
                                        {
                                            #region Name
                                            try
                                            {
                                                try
                                                {
                                                    endName = GrpUser.Substring(GrpUser.IndexOf("\"See Full Name\",\"i18n_Edit\":\"Edit\",\"fmt__full_name\":\""), (GrpUser.IndexOf("i18n__expand_your_network_to_see_more", GrpUser.IndexOf("\"See Full Name\",\"i18n_Edit\":\"Edit\",\"fmt__full_name\":\"")) - GrpUser.IndexOf("\"See Full Name\",\"i18n_Edit\":\"Edit\",\"fmt__full_name\":\""))).Replace("\"See Full Name\",\"i18n_Edit\":\"Edit\",\"fmt__full_name\":\"", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Trim();
                                                }
                                                catch
                                                {
                                                    try
                                                    {
                                                        endName = GrpUser.Substring(GrpUser.IndexOf("fmt__full_name\":"), (GrpUser.IndexOf(",", GrpUser.IndexOf("fmt__full_name\":")) - GrpUser.IndexOf("fmt__full_name\":"))).Replace("fmt__full_name\":", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Trim();

                                                    }
                                                    catch { }
                                                }

                                                if (string.IsNullOrEmpty(endName))
                                                {
                                                    try
                                                    {
                                                        //endName = Utils.getBetween(GrpUser, "<span class=\"full-name\">", "</span>");
                                                        endName = GrpUser.Substring(GrpUser.IndexOf("<span class=\"full-name\">"), (GrpUser.IndexOf("</span><span></span></span></h1></div></div><div id=\"headline-container\" data-li-template=\"headline\">", GrpUser.IndexOf("</span><span></span></span></h1></div></div><div id=\"headline-container\" data-li-template=\"headline\">")) - GrpUser.IndexOf("<span class=\"full-name\">"))).Replace("<span class=\"full-name\">", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Trim();
                                                    }
                                                    catch
                                                    { }
                                                }

                                                if (string.IsNullOrEmpty(endName))
                                                {
                                                    try
                                                    {
                                                        //endName = Utils.getBetween(GrpUser, "<span class=\"full-name\">", "</span>");
                                                        endName = GrpUser.Substring(GrpUser.IndexOf("data-li-fullName="), (GrpUser.IndexOf(">Send message</a>", GrpUser.IndexOf(">Send message</a>")) - GrpUser.IndexOf("data-li-fullName="))).Replace("data-li-fullName=", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace("\n", string.Empty).Trim();
                                                    }
                                                    catch
                                                    { }
                                                }
                                                //anetppl_profile">Christian A. Kenyeres</a>
                                            }
                                            catch { }

                                            #endregion
                                        }
                                        catch
                                        {

                                        }

                                        //Deegree connection
                                        try
                                        {
                                            int startindex = GrpUser.IndexOf("class=\"degree-icon");
                                            if (startindex > 0)
                                            {
                                                DeegreeConn = string.Empty;
                                                string start = GrpUser.Substring(startindex);
                                                int endIndex = start.IndexOf("<sup>");
                                                DeegreeConn = start.Substring(0, endIndex).Replace("\n",string.Empty).Replace("class=\"degree-icon", string.Empty).Replace("\"",string.Empty).Replace(">",string.Empty).Trim().ToString();

                                                if (DeegreeConn == "1")
                                                {
                                                    DeegreeConn = DeegreeConn + "st";
                                                }
                                                else if (DeegreeConn == "2")
                                                {
                                                    DeegreeConn = DeegreeConn + "nd";
                                                }
                                                else if (DeegreeConn == "3")
                                                {
                                                    DeegreeConn = DeegreeConn + "rd";
                                                }
                                                else if (DeegreeConn == "")
                                                {
                                                    DeegreeConn = "3rd";
                                                }
                                            }
                                            else
                                            {
                                                startindex = GrpUser.IndexOf("class=\"degree-icon group\">");
                                                DeegreeConn = string.Empty;

                                                if (startindex > 0)
                                                {
                                                    DeegreeConn = string.Empty;
                                                    string start = GrpUser.Substring(startindex);
                                                    int endIndex = start.IndexOf("</span>");
                                                    DeegreeConn = start.Substring(0, endIndex).Replace("span class=\"degree-icon group\">", string.Empty);

                                                }
                                                else 
                                                {
                                                    DeegreeConn = "3rd";
                                                }

                                              
                                            }

                                        }

                                        catch { }

                                        try
                                        {
                                            //endKey = Utils.getBetween(GrpUser, "view?id=", "&").Replace("view?id=", "");
                                            int startindex2 = GrpUser.IndexOf("memberId=");
                                            if (startindex2 > 0)
                                            {
                                                endKey = string.Empty;
                                                string start1 = GrpUser.Substring(startindex2);
                                                int endIndex1 = start1.IndexOf("data-li-fullName=");
                                                endKey = start1.Substring(0, endIndex1).Replace("memberId=", string.Empty).Replace("'", string.Empty).Replace(",", string.Empty).Replace("/", string.Empty).Replace("\"", string.Empty).Trim();
                                            }
                                            else
                                            {
                                                endKey = string.Empty;
                                                int startindex3 = GrpUser.IndexOf("member-");
                                                string start1 = GrpUser.Substring(startindex3);
                                                int endIndex1 = start1.IndexOf(">");
                                                endKey = start1.Substring(0, endIndex1).Replace("member-", string.Empty).Replace("'", string.Empty).Replace(",", string.Empty).Replace(">", string.Empty).Replace("\"", string.Empty).Trim();
                                            }
                                        }
                                        catch
                                        {

                                        }

                                        try
                                        {
                                            string endNamedisp = endName;
                                            endName = Uri.EscapeDataString(endName);
                                            GroupSpecMem.Add(endKey, endNamedisp + " (" + DeegreeConn + ")");

                                            if (WithGroupSearch == true)
                                            {
                                                Loggergrppmem("[ " + DateTime.Now + " ] => [ Added Group Member : " + endNamedisp + " (" + DeegreeConn + ") with Search keyword : " + SearchKeyword + " ]");
                                            }
                                            else
                                            {
                                                Loggergrppmem("[ " + DateTime.Now + " ] => [ Added Group Member : " + endNamedisp + " ]");
                                            }
                                            endKey = "";
                                            endName = "";
                                        }
                                        catch { }
                                    }
                                    else
                                    {

                                    }
                                }
                                catch { }
                            }
                        }
                    }
                 
                return GroupSpecMem;
            }
            catch { }
            return GroupSpecMem;
        }
        #endregion
      
        #region PostAddMembers
        public Dictionary<string, string> PostAddMembers(ref GlobusHttpHelper HttpHelper, string user)
        {

            try
            {
                if (!Globals.IsStop)
                {
                    Globals.lstComposeMessageThread.Add(Thread.CurrentThread);
                    Globals.lstComposeMessageThread.Distinct().ToList();
                    Thread.CurrentThread.IsBackground = true;
                }
            }
            catch
            { }
            try
            {

                string MemId = string.Empty;
                string MemFName = string.Empty;
                string MemLName = string.Empty;
                string MemFullName = string.Empty;
                string Memfullname1 = string.Empty;
                string memID1 = string.Empty;
                string MemId1 = string.Empty;

                this.HttpHelper = HttpHelper;

                #region Commented Universal code

                string pageSource = HttpHelper.getHtmlfromUrl(new Uri("https://www.linkedin.com/contacts/api/contacts/?&sortOrder=recent&source=LinkedIn"));


                if (!pageSource.Contains("success"))
                {
                    for (int i = 1; i <= 2; i++)
                    {
                        Thread.Sleep(4000);
                        pageSource = HttpHelper.getHtmlfromUrl(new Uri("https://www.linkedin.com/contacts/api/contacts/?&sortOrder=recent&source=LinkedIn"));
                        if (pageSource.Contains("success"))
                        {
                            break;
                        }

                    }
                }
                

                string[] RgxGroupData = System.Text.RegularExpressions.Regex.Split(pageSource, "{\"name\"");

                foreach (var Members in RgxGroupData)
                {
                    string Fname = string.Empty;
                    string Title = string.Empty;

                    if (Members.Contains("title"))
                    {
                        try
                        {
                            try
                            {
                                int startindex = Members.IndexOf(", \"id\": \"li_");
                                string start = Members.Substring(startindex).Replace(", \"id\": \"li_", string.Empty);
                                int endIndex = start.IndexOf("\"}");
                                MemId1 = start.Substring(0, endIndex).Replace("{[", string.Empty).Replace("}]", string.Empty).Trim();
                                MemId = user + ':' + MemId1;
                                Globals.groupStatusString = "withoutAPI because of li_";
                            }
                            catch 
                            {
                                int startindex = Members.IndexOf(", \"id\":");
                                string start = Members.Substring(startindex).Replace(", \"id\":", string.Empty);
                                int endIndex = start.IndexOf("},");
                                MemId1 = start.Substring(0, endIndex).Replace("{[", string.Empty).Replace("},", string.Empty).Trim();
                                MemId = user + ':' + MemId1;

                                Globals.groupStatusString = "API";
                            }


                            int StartIndex1 = Members.IndexOf(": \"");
                            if (StartIndex1 == 0)
                            {
                                string Start = Members.Substring(StartIndex1).Replace(": \"", string.Empty);
                                int EndIndex = Start.IndexOf("\",");
                                string End = Start.Substring(0, EndIndex);
                                Fname = End.Replace(",",";").Replace("&#xe3;", "ã").Replace("&#xe7;", "ç").Replace("&#xf4;", "ô").Replace("&#xe9;", "é").Replace("&#xba;", "º").Replace("&#xc1;", "Á").Replace("&#xb4;", "'").Replace("&#xed;", "í").Replace("&#xf5;", "õ").Replace("&#xf3;", "ó").Replace("&#xe1;", "á").Replace("&#xea;", "ê").Replace("&#xe0;", "à").Replace("&#xfc;", "ü").Replace("&#xe4;", "ä").Replace("&#xf6;", "ö").Replace("&#xfa;", "ú").Replace("&#xf4;", "ô").Replace("&#xc9;", "É").Replace("&#xe2;", "â").Replace("&#x113;", "ē").Replace("&#xd3;", "Ó").Replace("&#xf1;", "ñ").Replace("&#x20ac;", "€").Replace("&#xd1;", "Ñ").Replace("&#xe8;", "è").Replace("&#xd3;", "Ó").Replace("&#xaa;", "ª").Replace("&#x2605;", "★").Replace("&#x2606;", "☆").Replace("&#xf1;", "ñ").Replace("&#xc0;", "À").Replace("&#x263a;", "☺").Replace("&#xbf;", "¿").Replace("\\u00ae", "®").Replace("{[", string.Empty).Replace("}]", string.Empty);
                            }


                            int StartIndextemp = Members.IndexOf("title\":");
                            if (StartIndextemp > 0)
                            {
                                string Start = Members.Substring(StartIndextemp).Replace("title\":", string.Empty);
                                int EndIndex = Start.IndexOf(",");
                                string End = Start.Substring(0, EndIndex).Replace("\"","").Trim();
                                Title = End.Replace(",", ";").Replace("&#xe3;", "ã").Replace("&#xe7;", "ç").Replace("&#xf4;", "ô").Replace("&#xe9;", "é").Replace("&#xba;", "º").Replace("&#xc1;", "Á").Replace("&#xb4;", "'").Replace("&#xed;", "í").Replace("&#xf5;", "õ").Replace("&#xf3;", "ó").Replace("&#xe1;", "á").Replace("&#xea;", "ê").Replace("&#xe0;", "à").Replace("&#xfc;", "ü").Replace("&#xe4;", "ä").Replace("&#xf6;", "ö").Replace("&#xfa;", "ú").Replace("&#xf4;", "ô").Replace("&#xc9;", "É").Replace("&#xe2;", "â").Replace("&#x113;", "ē").Replace("&#xd3;", "Ó").Replace("&#xf1;", "ñ").Replace("&#x20ac;", "€").Replace("&#xd1;", "Ñ").Replace("&#xe8;", "è").Replace("&#xd3;", "Ó").Replace("&#xaa;", "ª").Replace("&#x2605;", "★").Replace("&#x2606;", "☆").Replace("&#xf1;", "ñ").Replace("&#xc0;", "À").Replace("&#x263a;", "☺").Replace("&#xbf;", "¿").Replace("\\u00ae", "®").Replace("{[", string.Empty).Replace("}]", string.Empty);
                                if (Title == "null")
                                {
                                    Title = "N/A";
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> PostAddMembers() >> 1 >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> PostAddMembers() >> 1 >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddFriendsGroupErrorLogs);
                        }
                #endregion

                        MemFullName = Fname;

                        try
                        {
                    
                            if (!string.IsNullOrEmpty(MemId1))
                            {
                                string MemUrl = "https://www.linkedin.com/contacts/view?id=" + MemId1; //"https://www.linkedin.com/profile/view?id=" + MemId1;
                                //string memResponse = HttpHelper.getHtmlfromUrl(new Uri(MemUrl));
                                //if (memResponse.Contains("is your connection"))
                                //{

                                    //if (!string.IsNullOrEmpty(Title))
                                    {
                                        MemberNameAndID.Add(MemId, MemFullName + ":" + Title);
                                    }
                                    
                                    GlobusFileHelper.AppendStringToTextfileNewLine(user + " : " + MemFullName, Globals.path_ComposeMessage_FriendList);
                                    Logger("[ " + DateTime.Now + " ] => [ Added member : " + MemFullName + " ]");

                                    string tempFinalData = user + "," + MemFullName + "," + MemId1 + "," + MemUrl + "," + Title;
                                    AddingLinkedInDataToCSVFile(tempFinalData);

                                    string Query = "INSERT INTO tb_endorsement (FriendName, FriendId,Username,Status) VALUES ('" + MemFullName + "', '" + MemId1 + "','" + user + "','0')";
                                    DataBaseHandler.InsertQuery(Query, "tb_endorsement");

                                    if (moduleLog == "endorsecamp")
                                    {
                                        Log_Endorse("[ " + DateTime.Now + " ] => [ Insert Member ID " + MemId1 + " of " + (MemFullName) + " ]");
                                    }
                               // }
                            }
                        }
                        catch (Exception ex)
                        {
                            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> PostAddMembers() >> 2 >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> PostAddMembers() >> 2 >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddFriendsGroupErrorLogs);
                        }
                    }
                }



                #region (Marry loadness) proxy issue


                // while (i < Convert.ToInt16(totalPageNumber))
                {
                    if (MemberNameAndID.Count() == 0)
                    {

                        #region commented
                        //string pageSource1 = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/contacts/api/contacts/?start=0&count=1"));
                        ////string pageSource1 = HttpHelper.getHtmlfromUrl(new Uri("http://www.linkedin.com/contacts/api/contacts/?start="+i+"&count="+y));
                        //if (pageSource1.Contains("total"))
                        //{
                        //    int sIndex = pageSource1.IndexOf("\"total\"");
                        //    string _start = pageSource1.Substring(sIndex);
                        //    int eIndex = pageSource1.IndexOf(",");
                        //    totalPageNumber = _start.Substring(0, eIndex).Replace("last", string.Empty).Replace(":", string.Empty).Replace("\"", string.Empty).Replace("total", string.Empty).Replace(",", string.Empty).Replace("}", string.Empty).Trim();
                        //}
                        //for (int start = 0; start < Convert.ToInt16(totalPageNumber); start += 10)
                        //{
                        //    pageSource1 = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/contacts/api/contacts/?start=" + start + "&count=10"));

                        //    string[] RgxGroupData1 = System.Text.RegularExpressions.Regex.Split(pageSource1, "{\"name\"");
                        //    foreach (var Members1 in RgxGroupData1)
                        //    {

                        //        if (Members1.Contains("title"))
                        //        {
                        //            string Fname1 = string.Empty;
                        //            try
                        //            {

                        //                int startindex2 = Members1.IndexOf(", \"id\"");
                        //                string start2 = Members1.Substring(startindex2);
                        //                int endIndex2 = start2.IndexOf("}");
                        //                // int endIndex2 = start2.IndexOf("class=");
                        //                //MemId = start2.Substring(0, endIndex2).Replace("\"", string.Empty).Trim().Replace(",", string.Empty);
                        //                MemId = start2.Substring(0, endIndex2).Replace("\"", string.Empty).Trim().Replace("li_", string.Empty).Replace(",", string.Empty).Split(':')[1];
                        //                MemId = user + ':' + MemId;

                        //                int StartIndex3 = Members1.IndexOf(":");
                        //                string Start3 = Members1.Substring(StartIndex3).Replace(":", string.Empty);
                        //                int EndIndex3 = Start3.IndexOf("title");
                        //                string End = Start3.Substring(0, EndIndex3).Replace("\"", string.Empty).Trim().Replace(",", string.Empty); ;
                        //                Fname1 = End;

                        //            }
                        //            catch (Exception ex)
                        //            {
                        //                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> PostAddMembers() >> 3 >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                        //                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> PostAddMembers() >> 3 >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddFriendsGroupErrorLogs);
                        //            }

                        //            MemFullName = Fname1;

                        //            try
                        //            {
                        //                MemberNameAndID.Add(MemId, MemFullName);
                        //            }
                        //            catch (Exception ex)
                        //            {
                        //                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> PostAddMembers() >> 4 >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                        //                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> PostAddMembers() >> 4 >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddFriendsGroupErrorLogs);
                        //            }

                        //        }
                        //    }
                        //} 
                        #endregion

                        int pagenumber = 0;
                        //   string pagesource1 = HttpHelper.getHtmlfromUrl(new Uri("https://www.linkedin.com/people/conn-list-view?fetchConnsFromDB=true"));

                        string pagesource1 = string.Empty;
                        int tempcount = 0;
                        //  while(!((pagesource1.Contains("No se han encontrado contactos que coincidan con estos filtros") || (pagesource1.Contains("No contacts found")))

                        do
                        {
                            grpmem("[ " + DateTime.Now + " ] => [ Adding members from page " + (pagenumber+1)+ " ]");

                            pagesource1 = HttpHelper.getHtmlfromUrl(new Uri("https://www.linkedin.com/people/conn-list-view?fetchConnsFromDB=true&pageNum=" + pagenumber));
                            if (string.IsNullOrEmpty(pagesource1))
                            {
                                for (int i = 1; i <= 2; i++)
                                {
                                    Thread.Sleep(5000);
                                    pagesource1 = HttpHelper.getHtmlfromUrl(new Uri("https://www.linkedin.com/people/conn-list-view?fetchConnsFromDB=true&pageNum=" + pagenumber));
                                    if (pagesource1.Contains("conx-list"))
                                    {
                                        break;
                                    }

                                }
                            }
                            string[] RgxGroupData1 = System.Text.RegularExpressions.Regex.Split(pagesource1, "<li id=\"");
                            foreach (var members1 in RgxGroupData1)
                            {
                                string Fname1 = string.Empty;
                                string memID2 = string.Empty;
                                string title = string.Empty;
                                if (members1.Contains("<div class=\"conn-wrapper\">"))
                                {
                                    try
                                    {
                                        int startindex1 = members1.IndexOf("");
                                        string start1 = members1.Substring(startindex1);
                                        int endIndex = start1.IndexOf("\"");
                                        memID2 = start1.Substring(0, endIndex).Trim();
                                        string ID = memID2;
                                        memID1 = user + ':' + memID2;

                                        #region commented

                                        //int StartIndex2 = members1.IndexOf("conn-name_" + ID + "\"");

                                        //string Start = members1.Substring(StartIndex2).Replace(">", string.Empty);
                                        //int EndIndex = Start.IndexOf("<");
                                        //string End = Start.Substring(0, EndIndex).Replace("conn-name_", "").Replace(ID, "").Replace("\"", "");
                                        //Fname1 = End.Replace("&#xe3;", "ã").Replace("&#xe7;", "ç").Replace("&#xf4;", "ô").Replace("&#xe9;", "é").Replace("&#xba;", "º").Replace("&#xc1;", "Á").Replace("&#xb4;", "'").Replace("&#xed;", "í").Replace("&#xf5;", "õ").Replace("&#xf3;", "ó").Replace("&#xe1;", "á").Replace("&#xea;", "ê").Replace("&#xe0;", "à").Replace("&#xfc;", "ü").Replace("&#xe4;", "ä").Replace("&#xf6;", "ö").Replace("&#xfa;", "ú").Replace("&#xf4;", "ô").Replace("&#xc9;", "É").Replace("&#xe2;", "â").Replace("&#x113;", "ē").Replace("&#xd3;", "Ó").Replace("&#xf1;", "ñ").Replace("&#x20ac;", "€").Replace("&#xd1;", "Ñ").Replace("&#xe8;", "è").Replace("&#xd3;", "Ó").Replace("&#xaa;", "ª").Replace("&#x2605;", "★").Replace("&#x2606;", "☆").Replace("&#xf1;", "ñ").Replace("&#xc0;", "À").Replace("&#x263a;", "☺").Replace("&#xbf;", "¿");
                                        
                                        #endregion

                                        int StartIndex2 = members1.IndexOf("type=\"checkbox\" value=");

                                        string Start = members1.Substring(StartIndex2).Replace("type=\"checkbox\" value=", string.Empty);
                                        int EndIndex = Start.IndexOf("/>");
                                        string End = Start.Substring(0, EndIndex).Replace("conn-name_", "").Replace(ID, "").Replace("\"", "").Replace("\"","").Replace(">","");
                                        Fname1 = End.Replace(",", ";").Replace("&#xe3;", "ã").Replace("&#xe7;", "ç").Replace("&#xf4;", "ô").Replace("&#xe9;", "é").Replace("&#xba;", "º").Replace("&#xc1;", "Á").Replace("&#xb4;", "'").Replace("&#xed;", "í").Replace("&#xf5;", "õ").Replace("&#xf3;", "ó").Replace("&#xe1;", "á").Replace("&#xea;", "ê").Replace("&#xe0;", "à").Replace("&#xfc;", "ü").Replace("&#xe4;", "ä").Replace("&#xf6;", "ö").Replace("&#xfa;", "ú").Replace("&#xf4;", "ô").Replace("&#xc9;", "É").Replace("&#xe2;", "â").Replace("&#x113;", "ē").Replace("&#xd3;", "Ó").Replace("&#xf1;", "ñ").Replace("&#x20ac;", "€").Replace("&#xd1;", "Ñ").Replace("&#xe8;", "è").Replace("&#xd3;", "Ó").Replace("&#xaa;", "ª").Replace("&#x2605;", "★").Replace("&#x2606;", "☆").Replace("&#xf1;", "ñ").Replace("&#xc0;", "À").Replace("&#x263a;", "☺").Replace("&#xbf;", "¿");

                                        int starttemp = members1.IndexOf("<span class=\"company-name\">");
                                        string startonetemp = members1.Substring(starttemp).Replace("<span class=\"company-name\">", "");
                                        int endIndextemp = startonetemp.IndexOf("</span>");
                                        title = startonetemp.Substring(0, endIndextemp).Trim();

                                        if (title == "null")
                                        {
                                            title = "N/A";
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> PostAddMembers() >> 1 >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> PostAddMembers() >> 1 >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddFriendsGroupErrorLogs);
                                    }

                                }
                                //      #endregion

                                Memfullname1 = Fname1;

                                try
                                {
                                   

                                    if (!string.IsNullOrEmpty(memID2))
                                    {

                                        //if (!string.IsNullOrEmpty(title))
                                        {
                                            MemberNameAndID.Add(memID1, Memfullname1 + ":" + title);
                                        }
                                        
                                        GlobusFileHelper.AppendStringToTextfileNewLine(user + " : " + Fname1, Globals.path_ComposeMessage_FriendList);
                                        Globals.groupStatusString = "withoutAPI";
                                        string MemUrl = "https://www.linkedin.com/profile/view?id="+memID2;

                                        string tempFinalData = user + "," + Memfullname1 + "," + memID2 + "," + MemUrl + "," + title;
                                        AddingLinkedInDataToCSVFile(tempFinalData);

                                        string Query = "INSERT INTO tb_endorsement (FriendName, FriendId,Username,Status) VALUES ('" + Memfullname1 + "', '" + memID2 + "','" + user + "','0')";
                                        DataBaseHandler.InsertQuery(Query, "tb_endorsement");
                                        tempcount = 0;
                                        if (moduleLog == "endorsecamp")
                                        {
                                            Log_Endorse("[ " + DateTime.Now + " ] => [ Insert Member ID " + memID2 + " of " + (Memfullname1) + " ]");
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    tempcount++;
                                    if (tempcount == 15)
                                    {
                                        return MemberNameAndID;
                                    }
                                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> PostAddMembers() >> 2 >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> PostAddMembers() >> 2 >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddFriendsGroupErrorLogs);
                                }

                            }
                            #region comment
                            // string pageSource1 = HttpHelper.getHtmlfromUrl(new Uri("https://www.linkedin.com/people/connections?trk=nav_responsive_sub_nav_network"));
                            //  pageSource1 = HttpHelper.getHtmlfromUrl(new Uri("https://www.linkedin.com/home?goback="));
                            //if (pageSource1.Contains("total"))
                            //{
                            //    int sIndex = pageSource1.IndexOf("\"total\"");
                            //    string _start = pageSource1.Substring(sIndex);
                            //    int eIndex = pageSource1.IndexOf(",");
                            //    totalPageNumber = _start.Substring(0, eIndex).Replace("last", string.Empty).Replace(":", string.Empty).Replace("\"", string.Empty).Replace("total", string.Empty).Replace(",", string.Empty).Replace("}", string.Empty).Trim();
                            //}
                            //for (int start = 0; start < Convert.ToInt16(totalPageNumber); start += 10)
                            /*   {
                                   //pageSource1 = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/contacts/api/contacts/?start=" + start + "&count=10"));

                                   string[] RgxGroupData1 = System.Text.RegularExpressions.Regex.Split(pageSource1, "memberId:");
                                   RgxGroupData1 = RgxGroupData1.Skip(1).ToArray();
                                   foreach (var Members1 in RgxGroupData1)
                                   {

                                       if (Members1.Contains("fullName"))
                                       {
                                           string Fname1 = string.Empty;
                                           try
                                           {

                                               int startindex2 = Members1.IndexOf("");
                                               string start2 = Members1.Substring(startindex2);
                                               int endIndex2 = start2.IndexOf(",");
                                               // int endIndex2 = start2.IndexOf("class=");
                                               //MemId = start2.Substring(0, endIndex2).Replace("\"", string.Empty).Trim().Replace(",", string.Empty);
                                               MemId = start2.Substring(0, endIndex2).Replace("\"", string.Empty).Trim().Replace("li_", string.Empty).Replace(",", string.Empty).Replace("'","");
                                               MemId = user + ':' + MemId;

                                               int StartIndex3 = Members1.IndexOf("fullName:");
                                               string Start3 = Members1.Substring(StartIndex3).Replace("fullName:", string.Empty);
                                               int EndIndex3 = Start3.IndexOf(",");
                                               string End = Start3.Substring(0, EndIndex3).Replace("\"", string.Empty).Trim().Replace(",", string.Empty).Replace("'","").Trim();
                                               Fname1 = End;

                                           }
                                           catch (Exception ex)
                                           {
                                               GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> PostAddMembers() >> 3 >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                               GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> PostAddMembers() >> 3 >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddFriendsGroupErrorLogs);
                                           }

                                           MemFullName = Fname1;

                                           try
                                           {
                                               MemberNameAndID.Add(MemId, MemFullName);
                                           }
                                           catch (Exception ex)
                                           {
                                               GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> PostAddMembers() >> 4 >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                               GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> PostAddMembers() >> 4 >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddFriendsGroupErrorLogs);
                                           }

                                       }
                                   }
                               } */
                            #endregion comment

                            pagenumber++;

                        } while (!(pagesource1.Contains("No se han encontrado contactos que coincidan con estos filtros") || (pagesource1.Contains("No contacts found")) || (pagesource1.Contains("No connections match your filter criteria"))));
                    }                   //if loop ends
                }
                #endregion

                return MemberNameAndID;
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> PostAddMembers() >> 5 >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> PostAddMembers() >> 5 >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddFriendsGroupErrorLogs);
                return MemberNameAndID;
            }
        }
        #endregion


        #region AddingLinkedInDataToCSVFile
        public static void AddingLinkedInDataToCSVFile(string Data)
        {
            try
            {
                //string LinkedInAppData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "InBoardProGetData\\" + SearchCriteria.FileName + ".csv");
                string LinkedInDeskTop = Globals.DesktopFolder + "\\FriendsList.csv";

                #region LinkedIn Writer
                
                //Checking File Exixtance
                if (!File.Exists(LinkedInDeskTop))
                {
                    string Header = "Account" + "," + "ContactPerson" + "," + "ID" + "," + "ProfileUrl" + "," + "Title";
                    GlobusFileHelper.AppendStringToTextfileNewLine(Header, LinkedInDeskTop);
                }

                if (!string.IsNullOrEmpty(Data))
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine(Data, LinkedInDeskTop);
                }
                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        #endregion



        public Dictionary<string, string> PostaddMembersWithExcelInput(ref GlobusHttpHelper Httphelper, string user)
        {

            try
            {
                if (!Globals.IsStop)
                {
                    Globals.lstComposeMessageThread.Add(Thread.CurrentThread);
                    Globals.lstComposeMessageThread.Distinct().ToList();
                    Thread.CurrentThread.IsBackground = true;
                }
            }
            catch
            { }
            try
            {
                MemberNameAndID.Clear();

             
                this.HttpHelper = HttpHelper;
               // string pageSource = HttpHelper.getHtmlfromUrl(new Uri("https://www.linkedin.com/contacts/api/"));
                string pagesource = Httphelper.getHtmlfromUrl(new Uri("https://www.linkedin.com/contacts/?filter=recent&trk=nav_responsive_tab_network#?filter=recent&trk=nav_responsive_tab_network"));
               
                foreach (string[] itemArr in Cmpmsg_excelData)
                {
                    string MemId = string.Empty;
                    string MemFName = string.Empty;
                    string MemLName = string.Empty;
                    string MemFullName = string.Empty;
                    string Memfullname1 = string.Empty;
                    string memID1 = string.Empty;
                    string Name = string.Empty;

                    if (user == itemArr.GetValue(0).ToString())
                    {
                        string URL = itemArr.GetValue(1).ToString();
                        if (!URL.Contains("https://www.linkedin.com/profile/"))
                        {
                            URL = "https://www.linkedin.com/profile/view?id=" + URL;
                        }
                        string PagesrcProfileExcel = Httphelper.getHtmlfromUrl(new Uri(URL));
                      
                        try
                        {
                            int startindex = URL.IndexOf("?id=");
                            string start = URL.Substring(startindex).Replace("?id=", string.Empty);
                            int endindex = start.IndexOf("&");
                            string end = start.Substring(0, endindex).Replace("&", string.Empty);
                            MemId = end.Trim();

                            MemId = user + ":" + MemId;
                        }
                        catch
                        { }

                        if (string.IsNullOrEmpty(MemId))
                        {
                            if (URL.Contains("&id"))
                            {
                                try
                                {
                                    URL = URL + "&&*@";
                                    int startindex1 = URL.IndexOf("&id=");
                                    string start1 = URL.Substring(startindex1).Replace("&id=", string.Empty);
                                    int endindex1 = start1.IndexOf("&&*@");
                                    string end1 = start1.Substring(0, endindex1).Replace("&&*@", string.Empty);
                                    MemId = end1.Trim();
                                    MemId = user + ":" + MemId;
                                }
                                catch
                                { }
                            }
                        }

                        if (string.IsNullOrEmpty(MemId))
                        {
                            if (URL.Contains("?id="))
                            {
                                try
                                {

                                    MemId = URL.Split('=')[1].ToString();
                                    MemId = user + ":" + MemId;
                                }
                                catch
                                { }
                            }
                        }

                        try
                        {
                            int startindex1 = PagesrcProfileExcel.IndexOf("fmt__profileUserFullName\":\"");
                            string start1 = PagesrcProfileExcel.Substring(startindex1).Replace("fmt__profileUserFullName\":\"", string.Empty);
                            int endindex1 = start1.IndexOf(",");
                            string end1 = start1.Substring(0, endindex1).Replace("\"", string.Empty);
                            Name = end1.Trim();

                        }
                        catch
                        { }

                        if (string.IsNullOrEmpty(Name))
                        {
                            try
                            {
                                int startindex1 = PagesrcProfileExcel.IndexOf("span class=\"full-name\">");
                                string start1 = PagesrcProfileExcel.Substring(startindex1).Replace("span class=\"full-name\">", string.Empty);
                                int endindex1 = start1.IndexOf("</span>");
                                string end1 = start1.Substring(0, endindex1).Replace("</span>", string.Empty);
                                Name = end1.Trim();
                            }
                            catch
                            { }
                        }

                        //<span class="full-name" dir="auto">Karla (Keyser) Silver</span><span></span></span></h1>
                        if (string.IsNullOrEmpty(Name))
                        {
                            try
                            {
                                int StartIndex = PagesrcProfileExcel.IndexOf("<span class=\"full-name\"");
                                string Start = PagesrcProfileExcel.Substring(StartIndex).Replace("<span class=\"full-name\"", string.Empty);
                                int EndIndex = Start.IndexOf("</span>");
                                string End = Start.Substring(0, EndIndex).Replace("dir=\"auto\"", string.Empty).Replace(">", string.Empty).Replace(",", string.Empty).Trim();
                                Name = End.Trim();
                            }
                            catch
                            { }
                        }

                        Memfullname1 = Name;

                        try
                        {
                            if (!string.IsNullOrEmpty(Name))
                            {
                                MemberNameAndID.Add(MemId, Memfullname1);
                            }
                        }
                        catch (Exception ex)
                        {
                            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> PostAddMembers() >> 2 >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> PostAddMembers() >> 2 >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddFriendsGroupErrorLogs);
                        }
                    }
                }
                return MemberNameAndID;
                
            }
            catch { return MemberNameAndID; }


        }

        #region GetSelectedIDForPendingGroups
        public Dictionary<string, string> GetSelectedIDForPendingGroups_old(ref GlobusHttpHelper HttpHelper, string user)
        {
            string GetID = string.Empty;
            Dictionary<string, string> GroupPendingDtl = new Dictionary<string, string>();
          

            try
            {
                     string pageSourceforGroup = string.Empty;
                     //pageSourceforGroup = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/profile/view?id=" + GetID));

                    try
                    {
                        //pageSourceforGroup = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/myGroups?trk=nav_responsive_sub_nav_groups"));
                        pageSourceforGroup = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/grp/"));
                        //if (pageSourceforGroup == "")
                        //{
                        //    Thread.Sleep(2000);
                        //    pageSourceforGroup = HttpHelper.getHtmlfromUrl(new Uri("http://www.linkedin.com/myGroups?trk=nav_responsive_sub_nav_groups"));
                        //}
                        //if (pageSourceforGroup == "")
                        //{
                        //    Thread.Sleep(2000);
                        //    pageSourceforGroup = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/grp/"));
                        //}
                        //if (pageSourceforGroup == "")
                        //{
                        //    Thread.Sleep(2000);
                        //    pageSourceforGroup = HttpHelper.getHtmlfromUrl(new Uri("http://www.linkedin.com/grp/"));
                        //}
                    }
                    catch { }

                    
                    //string[] RgxGroupDataforGroup = System.Text.RegularExpressions.Regex.Split(pageSourceforGroup, "<li class=\"media-block \">");
                      string[] RgxGroupDataforGroup = System.Text.RegularExpressions.Regex.Split(pageSourceforGroup, "<div class=\"media-content\">");
                    
                    foreach (string item in RgxGroupDataforGroup)
                    {
                        string GroupUrl = string.Empty;
                        string GroupNames = string.Empty;
                        string GroupIds = string.Empty;
                        string CheckOwnGroup = string.Empty;

                        try
                        {

                          

                            //if (!item.Contains("<!DOCTYPE html>") && item.Contains("groups") && item.Contains("Membership Pending"))
                            if (!item.Contains("<!DOCTYPE html>") && item.Contains("groups"))
                            {

                                #region commented old code
                                //try
                                //{
                                //    if (item.Contains("class=\"public\""))
                                //    {
                                //        int startindex = item.IndexOf("class=\"public\"");
                                //        string start = item.Substring(startindex).Replace("class=\"public\"", string.Empty);
                                //        int endindex = start.IndexOf("</a>");
                                //        string end = start.Substring(0, endindex).Replace(">", string.Empty).Replace(":", "-").Replace(",", string.Empty);
                                //        GroupNames = user + ":" + end;
                                //    }
                                //    else if (item.Contains("class=\"private\""))
                                //    {
                                //        int startindex = item.IndexOf("class=\"private\"");
                                //        string start = item.Substring(startindex).Replace("class=\"private\"", string.Empty);
                                //        int endindex = start.IndexOf("</a>");
                                //        string end = start.Substring(0, endindex).Replace(">", string.Empty).Replace(":", "-").Replace(",", string.Empty);
                                //        GroupNames = user + ":" + end;
                                //    }
                                //}
                                //catch { }


                                //if (string.IsNullOrEmpty(GroupIds))
                                //{
                                //    int startindex = item.IndexOf("groups/");
                                //    if (startindex > 0)
                                //    {
                                //        try
                                //        {
                                //            string start = item.Substring(startindex).Replace("groups/", "");
                                //            int endindex = start.IndexOf("?trk");
                                //            string endKey = start.Substring(0, endindex);

                                //            string[] endKeyLast = endKey.Split('-');
                                //            try
                                //            {
                                //                if (NumberHelper.ValidateNumber(endKeyLast[1]))
                                //                {
                                //                    endKey = endKeyLast[1];
                                //                }
                                //                else if (NumberHelper.ValidateNumber(endKeyLast[2]))
                                //                {
                                //                    endKey = endKeyLast[2];
                                //                }
                                //                else if (NumberHelper.ValidateNumber(endKeyLast[3]))
                                //                {
                                //                    endKey = endKeyLast[3];
                                //                }
                                //                else if (NumberHelper.ValidateNumber(endKeyLast[4]))
                                //                {
                                //                    endKey = endKeyLast[4];
                                //                }
                                //                else if (NumberHelper.ValidateNumber(endKeyLast[5]))
                                //                {
                                //                    endKey = endKeyLast[5];
                                //                }
                                //                else if (NumberHelper.ValidateNumber(endKeyLast[6]))
                                //                {
                                //                    endKey = endKeyLast[6];
                                //                }
                                //            }
                                //            catch { }

                                //            GroupIds = endKey;
                                //        }
                                //        catch { }
                                //    }

                                //}

                                //try
                                //{
                                //    GroupPendingDtl.Add(GroupNames, GroupIds);
                                //}
                                //catch { } 
                                #endregion

                                try
                                {
                                    string[] UrlCollection = System.Text.RegularExpressions.Regex.Split(item, "<a href=");

                                    foreach (var itemUrl in UrlCollection)
                                    {
                                        if (itemUrl.Contains("trk=my_groups-tile-grp"))
                                        {
                                            try
                                            {
                                                int startind = itemUrl.IndexOf("/groups");
                                                string star = itemUrl.Substring(startind);
                                                int endInd = star.IndexOf("class=");
                                                GroupUrl = "https://www.linkedin.com" + star.Substring(0, endInd).Replace("\"", string.Empty).Replace("amp;", string.Empty).Trim();
                                                break;
                                            }
                                            catch { }
                                        }
                                    }

                                    CheckOwnGroup = HttpHelper.getHtmlfromUrl1(new Uri(GroupUrl));

                                }
                                catch { }

                                if (CheckOwnGroup.Contains("Leave this group.") || CheckOwnGroup.Contains("Your Membership is pending approval by the group owner."))
                                {
                                    if ((item.Contains("public")))
                                    {
                                        try
                                        {
                                            int startindex = item.IndexOf("class=\"public\"");
                                            string start = item.Substring(startindex);
                                            int endIndex = start.IndexOf("</a>");
                                            GroupNames = start.Substring(0, endIndex).Replace("title", string.Empty).Replace("=", string.Empty).Replace(">", string.Empty).Replace("groups", string.Empty).Replace("/", string.Empty).Replace("\"", string.Empty).Replace("&amp;", "&").Replace("classpublic", string.Empty).Replace("&quot;", "'").Replace(":", ";").Replace("This is an open group", string.Empty);

                                            if (item.Contains("Membership Pending"))
                                            {
                                                GroupNames = GroupNames + " (Pending Group)" + ':' + user;
                                            }
                                            else
                                            {
                                                GroupNames = GroupNames + " (Open Group)" + ':' + user;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> GroupStatus --> PostCreateGroupNames() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update1 --> PostCreateGroupNames() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinGetGroupMemberErrorLogs);
                                        }

                                        try
                                        {
                                            GroupIds = "";
                                            int startindex1 = item.IndexOf("gid=");
                                            string start1 = item.Substring(startindex1);
                                            int endIndex1 = start1.IndexOf("&");
                                            GroupIds = start1.Substring(0, endIndex1).Replace("gid=", string.Empty).Trim();
                                            if (!NumberHelper.ValidateNumber(GroupIds))
                                            {
                                                try
                                                {
                                                    GroupIds = GroupIds.Split('\"')[0];
                                                }
                                                catch { }
                                            }

                                            if (GroupIds == string.Empty)
                                            {
                                                startindex1 = item.IndexOf("group");
                                                start1 = item.Substring(startindex1);
                                                endIndex1 = start1.IndexOf("?");
                                                GroupIds = start1.Substring(0, endIndex1).Replace("groups", string.Empty).Replace("/", string.Empty).Replace("<a href=", string.Empty).Replace("\"", string.Empty);

                                                if (!NumberHelper.ValidateNumber(GroupIds))
                                                {
                                                    try
                                                    {
                                                        GroupIds = GroupIds.Split('\"')[0];
                                                    }
                                                    catch { }
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> GroupStatus2 --> PostCreateGroupNames() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update2 --> PostCreateGroupNames() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinGetGroupMemberErrorLogs);
                                        }

                                        if (GroupIds.Contains("analyticsURL"))
                                        {
                                            continue;
                                        }

                                        if (GroupIds == string.Empty)
                                        {
                                            try
                                            {
                                                int startindex2 = item.IndexOf("gid=");
                                                string start2 = item.Substring(startindex2);
                                                int endIndex2 = start2.IndexOf("&");
                                                GroupIds = start2.Substring(0, endIndex2).Replace("gid", string.Empty).Replace("=", string.Empty);
                                            }
                                            catch (Exception ex)
                                            {
                                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> GroupStatus3 --> PostCreateGroupNames() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update3 --> PostCreateGroupNames() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinGetGroupMemberErrorLogs);
                                            }
                                        }
                                        else
                                        {
                                            string[] endKeyLast = GroupIds.Split('-');
                                            try
                                            {
                                                if (NumberHelper.ValidateNumber(endKeyLast[1]))
                                                {
                                                    GroupIds = endKeyLast[1];
                                                }
                                                else if (NumberHelper.ValidateNumber(endKeyLast[2]))
                                                {
                                                    GroupIds = endKeyLast[2];
                                                }
                                                else if (NumberHelper.ValidateNumber(endKeyLast[3]))
                                                {
                                                    GroupIds = endKeyLast[3];
                                                }
                                                else if (NumberHelper.ValidateNumber(endKeyLast[4]))
                                                {
                                                    GroupIds = endKeyLast[4];
                                                }
                                                else if (NumberHelper.ValidateNumber(endKeyLast[5]))
                                                {
                                                    GroupIds = endKeyLast[5];
                                                }
                                                else if (NumberHelper.ValidateNumber(endKeyLast[6]))
                                                {
                                                    GroupIds = endKeyLast[6];
                                                }
                                            }
                                            catch { }
                                        }

                                        try
                                        {
                                            GroupPendingDtl.Add(GroupNames, GroupIds);
                                        }
                                        catch { }
                                    }



                                    //if (!item.Contains("<!DOCTYPE html>") && item.Contains("groups") && item.Contains("Membership Pending"))
                                    if (!item.Contains("<!DOCTYPE html>") && item.Contains("groups"))
                                    {
                                        if ((item.Contains("private")))
                                        {
                                            try
                                            {
                                                int startindex = item.IndexOf("class=\"private\"");
                                                string start = item.Substring(startindex);
                                                int endIndex = start.IndexOf("</a>");
                                                GroupNames = start.Substring(0, endIndex).Replace("=", string.Empty).Replace(">", string.Empty).Replace("groups", string.Empty).Replace("/", string.Empty).Replace("\"", string.Empty).Replace("&amp;", "&").Replace("&quot;", "'").Replace(":", ";").Replace("classprivate", string.Empty);

                                                if (item.Contains("Membership Pending"))
                                                {
                                                    GroupNames = GroupNames + " (Pending Group)" + ':' + user;
                                                }
                                                else
                                                {
                                                    GroupNames = GroupNames + " (Open Group)" + ':' + user;
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> GroupStatus4 --> PostCreateGroupNames() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update4 --> PostCreateGroupNames() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinGetGroupMemberErrorLogs);
                                            }

                                            try
                                            {
                                                int startindex1 = item.IndexOf("gid=");
                                                string start1 = item.Substring(startindex1);
                                                int endIndex1 = start1.IndexOf("&");
                                                GroupIds = start1.Substring(0, endIndex1).Replace("gid=", string.Empty).Replace("/", string.Empty).Replace("<a href=", string.Empty).Replace("\"", string.Empty).Replace("class=blanket-target><a>groups?home=", string.Empty).Trim();

                                                if (GroupIds == string.Empty)
                                                {
                                                    try
                                                    {
                                                        GroupIds = GroupIds.Split(' ')[0].Trim();
                                                    }
                                                    catch { }
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> GroupStatus5 --> PostCreateGroupNames() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update5 --> PostCreateGroupNames() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinGetGroupMemberErrorLogs);
                                            }

                                            if (GroupIds.Contains("analyticsURL"))
                                            {
                                                continue;
                                            }

                                            if (GroupIds == string.Empty)
                                            {
                                                try
                                                {
                                                    int startindex2 = item.IndexOf("gid=");
                                                    string start2 = item.Substring(startindex2);
                                                    int endIndex2 = start2.IndexOf("&");
                                                    GroupIds = start2.Substring(0, endIndex2).Replace("gid", string.Empty).Replace("=", string.Empty);

                                                    if (!NumberHelper.ValidateNumber(GroupIds))
                                                    {
                                                        try
                                                        {
                                                            GroupIds = GroupIds.Split('\"')[0];
                                                        }
                                                        catch { }
                                                    }

                                                }
                                                catch (Exception ex)
                                                {
                                                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> GroupStatus6 --> PostCreateGroupNames() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update6 --> PostCreateGroupNames() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinGetGroupMemberErrorLogs);
                                                }
                                            }
                                            else
                                            {
                                                string[] endKeyLast = GroupIds.Split('-');
                                                try
                                                {
                                                    if (NumberHelper.ValidateNumber(endKeyLast[1]))
                                                    {
                                                        GroupIds = endKeyLast[1];
                                                    }
                                                    else if (NumberHelper.ValidateNumber(endKeyLast[2]))
                                                    {
                                                        GroupIds = endKeyLast[2];
                                                    }
                                                    else if (NumberHelper.ValidateNumber(endKeyLast[3]))
                                                    {
                                                        GroupIds = endKeyLast[3];
                                                    }
                                                    else if (NumberHelper.ValidateNumber(endKeyLast[4]))
                                                    {
                                                        GroupIds = endKeyLast[4];
                                                    }
                                                    else if (NumberHelper.ValidateNumber(endKeyLast[5]))
                                                    {
                                                        GroupIds = endKeyLast[5];
                                                    }
                                                    else if (NumberHelper.ValidateNumber(endKeyLast[6]))
                                                    {
                                                        GroupIds = endKeyLast[6];
                                                    }
                                                }
                                                catch { }
                                            }

                                            try
                                            {
                                                GroupPendingDtl.Add(GroupNames, GroupIds);
                                            }
                                            catch { }
                                        }

                                        #region Commented old code

                                        //try
                                        //{
                                        //    if (item.Contains("class=\"public\""))
                                        //    {
                                        //        int startindex = item.IndexOf("class=\"public\"");
                                        //        string start = item.Substring(startindex).Replace("class=\"public\"", string.Empty);
                                        //        int endindex = start.IndexOf("</a>");
                                        //        string end = start.Substring(0, endindex).Replace(">", string.Empty).Replace(":", "-").Replace(",", string.Empty);
                                        //        GroupNames = user + ":" + end;
                                        //    }
                                        //    else if (item.Contains("class=\"private\""))
                                        //    {
                                        //        int startindex = item.IndexOf("class=\"private\"");
                                        //        string start = item.Substring(startindex).Replace("class=\"private\"", string.Empty);
                                        //        int endindex = start.IndexOf("</a>");
                                        //        string end = start.Substring(0, endindex).Replace(">", string.Empty).Replace(":", "-").Replace(",", string.Empty);
                                        //        GroupNames = user + ":" + end;
                                        //    }
                                        //}
                                        //catch { }

                                        //if (string.IsNullOrEmpty(GroupIds))
                                        //{
                                        //    try
                                        //    {
                                        //        int startindex = item.IndexOf("gid=");
                                        //        if (startindex > 0)
                                        //        {
                                        //            try
                                        //            {
                                        //                string start = item.Substring(startindex).Replace("gid=", "");
                                        //                int endindex = start.IndexOf("&amp");
                                        //                string end = start.Substring(0, endindex);
                                        //                GroupIds = end;
                                        //            }
                                        //            catch { }
                                        //        }
                                        //    }
                                        //    catch { }
                                        //}

                                        //try
                                        //{
                                        //    GroupPendingDtl.Add(GroupNames, GroupIds);
                                        //}
                                        //catch { } 
                                        #endregion
                                    }
                                }
                            }
                        }
                        catch { }
                    }
                   
                    
                return GroupPendingDtl;
            }
            catch (Exception ex)
            {
                return GroupPendingDtl;
            }

        }
        #endregion

        #region GetSelectedIDForPendingGroups
        public Dictionary<string, string> GetSelectedIDForPendingGroups(ref GlobusHttpHelper HttpHelper, string user)
        {
            string GetID = string.Empty;
            Dictionary<string, string> GroupPendingDtl = new Dictionary<string, string>();


            try
            {
                string pageSourceforGroup = string.Empty;

                try
                {
                    pageSourceforGroup = HttpHelper.getHtmlfromUrl1(new Uri("https://www.linkedin.com/anet?dispSortAnets=&trk=my_groups-h_gn-settings"));
                    //pageSourceforGroup = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/grp/"));

                }
                catch { }


                string[] RgxGroupDataforGroup = System.Text.RegularExpressions.Regex.Split(pageSourceforGroup, "<td class=\"group-name\">");

                foreach (string item in RgxGroupDataforGroup)
                {
                    string GroupUrl = string.Empty;
                    string GroupNames = string.Empty;
                    string GroupIds = string.Empty;

                    try
                    {

                        if (!item.Contains("<!DOCTYPE html>") && item.Contains("groups") && !item.Contains("manageGroup"))
                        {

                            try
                            {
                                int startindex = item.IndexOf("<strong>");
                                string start = item.Substring(startindex);
                                int endIndex = start.IndexOf("</strong>");
                                GroupNames = start.Substring(0, endIndex).Replace("<strong>", string.Empty).Trim();

                                if (item.Contains("Pending"))
                                {
                                    GroupNames = GroupNames + " (Pending Group)" + ':' + user;
                                }
                                else
                                {
                                    GroupNames = GroupNames + " (Open Group)" + ':' + user;
                                } 
                                

                            }
                            catch (Exception ex)
                            {
                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> GroupStatus --> PostCreateGroupNames() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update1 --> PostCreateGroupNames() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinGetGroupMemberErrorLogs);
                            }

                            try
                            {
                                GroupIds = "";
                                int startindex1 = item.IndexOf("?gid=");
                                string start1 = item.Substring(startindex1);
                                int endIndex1 = start1.IndexOf("\">");
                                GroupIds = start1.Substring(0, endIndex1).Replace("?gid=", string.Empty).Trim();
                                if (!NumberHelper.ValidateNumber(GroupIds))
                                {
                                    try
                                    {
                                        GroupIds = GroupIds.Split('\"')[0];
                                    }
                                    catch { }
                                }

                                if (GroupIds == string.Empty)
                                {
                                    startindex1 = item.IndexOf("group");
                                    start1 = item.Substring(startindex1);
                                    endIndex1 = start1.IndexOf("?");
                                    GroupIds = start1.Substring(0, endIndex1).Replace("groups", string.Empty).Replace("/", string.Empty).Replace("<a href=", string.Empty).Replace("\"", string.Empty);

                                    if (!NumberHelper.ValidateNumber(GroupIds))
                                    {
                                        try
                                        {
                                            GroupIds = GroupIds.Split('\"')[0];
                                        }
                                        catch { }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> GroupStatus2 --> PostCreateGroupNames() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update2 --> PostCreateGroupNames() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinGetGroupMemberErrorLogs);
                            }

                            if (GroupIds.Contains("analyticsURL"))
                            {
                                continue;
                            }

                            if (GroupIds == string.Empty)
                            {
                                try
                                {
                                    int startindex2 = item.IndexOf("gid=");
                                    string start2 = item.Substring(startindex2);
                                    int endIndex2 = start2.IndexOf("&");
                                    GroupIds = start2.Substring(0, endIndex2).Replace("gid", string.Empty).Replace("=", string.Empty);
                                }
                                catch (Exception ex)
                                {
                                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> GroupStatus3 --> PostCreateGroupNames() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update3 --> PostCreateGroupNames() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinGetGroupMemberErrorLogs);
                                }
                            }
                            else
                            {
                                string[] endKeyLast = GroupIds.Split('-');
                                try
                                {
                                    if (NumberHelper.ValidateNumber(endKeyLast[1]))
                                    {
                                        GroupIds = endKeyLast[1];
                                    }
                                    else if (NumberHelper.ValidateNumber(endKeyLast[2]))
                                    {
                                        GroupIds = endKeyLast[2];
                                    }
                                    else if (NumberHelper.ValidateNumber(endKeyLast[3]))
                                    {
                                        GroupIds = endKeyLast[3];
                                    }
                                    else if (NumberHelper.ValidateNumber(endKeyLast[4]))
                                    {
                                        GroupIds = endKeyLast[4];
                                    }
                                    else if (NumberHelper.ValidateNumber(endKeyLast[5]))
                                    {
                                        GroupIds = endKeyLast[5];
                                    }
                                    else if (NumberHelper.ValidateNumber(endKeyLast[6]))
                                    {
                                        GroupIds = endKeyLast[6];
                                    }
                                }
                                catch { }
                            }

                            try
                            {
                                GroupPendingDtl.Add(GroupNames, GroupIds);
                            }
                            catch { }

                        }
                    }
                    catch { }
                }


                return GroupPendingDtl;
            }
            catch (Exception ex)
            {
                return GroupPendingDtl;
            }

        }
        #endregion

        #region GetSelectedIDForOwnGroups
        public Dictionary<string, string> GetSelectedIDForOwnGroups(ref GlobusHttpHelper HttpHelper, string user)
        {
            string GetID = string.Empty;
            Dictionary<string, string> GroupPendingDtl = new Dictionary<string, string>();


            try
            {
                string pageSourceforGroup = string.Empty;
                
                try
                {
                    pageSourceforGroup = HttpHelper.getHtmlfromUrl1(new Uri("https://www.linkedin.com/anet?dispSortAnets=&trk=my_groups-h_gn-settings"));
                    //pageSourceforGroup = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/grp/"));

                }
                catch { }


               string[] RgxGroupDataforGroup = System.Text.RegularExpressions.Regex.Split(pageSourceforGroup, "<td class=\"group-name\">");

                foreach (string item in RgxGroupDataforGroup)
                {
                    string GroupUrl = string.Empty;
                    string GroupNames = string.Empty;
                    string GroupIds = string.Empty;
                    
                    try
                    {

                        if (!item.Contains("<!DOCTYPE html>") && item.Contains("groups") && item.Contains("manageGroup"))
                        {

                            try
                            {
                                int startindex = item.IndexOf("<strong>");
                                string start = item.Substring(startindex);
                                int endIndex = start.IndexOf("</strong>");
                                GroupNames = start.Substring(0, endIndex).Replace("<strong>", string.Empty).Trim();
                                GroupNames = GroupNames + ':' + user;

                            }
                            catch (Exception ex)
                            {
                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> GroupStatus --> PostCreateGroupNames() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update1 --> PostCreateGroupNames() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinGetGroupMemberErrorLogs);
                            }

                            try
                            {
                                GroupIds = "";
                                int startindex1 = item.IndexOf("?gid=");
                                string start1 = item.Substring(startindex1);
                                int endIndex1 = start1.IndexOf("\">");
                                GroupIds = start1.Substring(0, endIndex1).Replace("?gid=", string.Empty).Trim();
                                if (!NumberHelper.ValidateNumber(GroupIds))
                                {
                                    try
                                    {
                                        GroupIds = GroupIds.Split('\"')[0];
                                    }
                                    catch { }
                                }

                                if (GroupIds == string.Empty)
                                {
                                    startindex1 = item.IndexOf("group");
                                    start1 = item.Substring(startindex1);
                                    endIndex1 = start1.IndexOf("?");
                                    GroupIds = start1.Substring(0, endIndex1).Replace("groups", string.Empty).Replace("/", string.Empty).Replace("<a href=", string.Empty).Replace("\"", string.Empty);

                                    if (!NumberHelper.ValidateNumber(GroupIds))
                                    {
                                        try
                                        {
                                            GroupIds = GroupIds.Split('\"')[0];
                                        }
                                        catch { }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> GroupStatus2 --> PostCreateGroupNames() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update2 --> PostCreateGroupNames() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinGetGroupMemberErrorLogs);
                            }

                            if (GroupIds.Contains("analyticsURL"))
                            {
                                continue;
                            }

                            if (GroupIds == string.Empty)
                            {
                                try
                                {
                                    int startindex2 = item.IndexOf("gid=");
                                    string start2 = item.Substring(startindex2);
                                    int endIndex2 = start2.IndexOf("&");
                                    GroupIds = start2.Substring(0, endIndex2).Replace("gid", string.Empty).Replace("=", string.Empty);
                                }
                                catch (Exception ex)
                                {
                                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> GroupStatus3 --> PostCreateGroupNames() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update3 --> PostCreateGroupNames() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinGetGroupMemberErrorLogs);
                                }
                            }
                            else
                            {
                                string[] endKeyLast = GroupIds.Split('-');
                                try
                                {
                                    if (NumberHelper.ValidateNumber(endKeyLast[1]))
                                    {
                                        GroupIds = endKeyLast[1];
                                    }
                                    else if (NumberHelper.ValidateNumber(endKeyLast[2]))
                                    {
                                        GroupIds = endKeyLast[2];
                                    }
                                    else if (NumberHelper.ValidateNumber(endKeyLast[3]))
                                    {
                                        GroupIds = endKeyLast[3];
                                    }
                                    else if (NumberHelper.ValidateNumber(endKeyLast[4]))
                                    {
                                        GroupIds = endKeyLast[4];
                                    }
                                    else if (NumberHelper.ValidateNumber(endKeyLast[5]))
                                    {
                                        GroupIds = endKeyLast[5];
                                    }
                                    else if (NumberHelper.ValidateNumber(endKeyLast[6]))
                                    {
                                        GroupIds = endKeyLast[6];
                                    }
                                }
                                catch { }
                            }

                            try
                            {
                                GroupPendingDtl.Add(GroupNames, GroupIds);
                            }
                            catch { }

                        }
                    }
                    catch { }
                }


                return GroupPendingDtl;
            }
            catch (Exception ex)
            {
                return GroupPendingDtl;
            }

        }
        #endregion
      
        #region FromEmailCodeComposeMsg
        public string FromEmailCodeComposeMsg(ref GlobusHttpHelper HttpHelper, string email)
        {
            string FromId = string.Empty;
            //string FromNm = string.Empty;
            string namewithid = string.Empty;
            string pageSource = string.Empty;
            string[] RgxGroupData = new string[] { };
            

            if (string.IsNullOrEmpty(namewithid))
            {
                try
                {

                    pageSource = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/inbox/compose?trk=inbox_messages-comm-left_nav-compose"));
                    RgxGroupData = Regex.Split(pageSource, "\"name\":\"senderEmail\"");


                    try
                    {
                        if (RgxGroupData[1].Contains("\"value\":"))
                        {
                            try
                            {
                                int StartIndex = RgxGroupData[1].IndexOf("\"value\":");
                                string start = RgxGroupData[1].Substring(StartIndex);
                                int endIndex = start.IndexOf(",");
                                FromId = start.Substring(0, endIndex).Replace("\"", string.Empty).Replace("value:", string.Empty).Replace("}", string.Empty).Trim();

                            }
                            catch
                            {
                            }


                            //try
                            //{
                            //    int StartIndex1 = RgxGroupData[1].IndexOf("label\":");
                            //    string start = RgxGroupData[1].Substring(StartIndex1);
                            //    int endIndex1 = start.IndexOf("<");
                            //    FromNm = start.Substring(0, endIndex1).Replace("\"", string.Empty).Replace("label:", string.Empty).Trim();

                            //}
                            //catch
                            //{
                            //}

                        }

                        //namewithid = FromId + ":" + FromNm;
                        namewithid = FromId;

                    }
                    catch { }



                }
                catch (Exception ex)
                {
                    return namewithid;
                }
            }

            return namewithid;
        }
        #endregion

        #region FromEmailCodeMsgGroupMem
        public string FromEmailCodeMsgGroupMem(ref GlobusHttpHelper HttpHelper, string gid)
        {
            string FromId = string.Empty;
            string pageSource = string.Empty;
            string[] RgxGroupData = new string[] { };
            List<string> lstpasttitle = new List<string>();
            List<string> checkpasttitle = new List<string>();
            string GroupName = string.Empty;
            string csrfToken = string.Empty;
            string[] RgxSikValue = new string[] { };
            string[] RgxPageNo = new string[] { };
            string sikvalue = string.Empty;


            try
            {
               string pageSource1 = HttpHelper.getHtmlfromUrl(new Uri("http://www.linkedin.com/home?trk=hb_tab_home_top"));

                if (pageSource1.Contains("csrfToken"))
                {
                    csrfToken = pageSource1.Substring(pageSource1.IndexOf("csrfToken"), 50);
                    string[] Arr = csrfToken.Split('>');
                    csrfToken = Arr[0];
                    csrfToken = csrfToken.Replace(":", "%3A").Replace("csrfToken", "").Replace("\"", string.Empty).Replace("value", string.Empty).Replace("cs", string.Empty).Replace("id", string.Empty).Replace("=", string.Empty).Replace("\n", string.Empty).Replace(">", string.Empty).Replace("<script src", string.Empty);
                    csrfToken = csrfToken.Trim();
                }

                pageSource = HttpHelper.getHtmlfromUrl(new Uri("http://www.linkedin.com/groups?viewMembers=&gid=" + gid));
                RgxSikValue = System.Text.RegularExpressions.Regex.Split(pageSource, "sik");

                try
                {
                    sikvalue = RgxSikValue[1].Split('&')[0].Replace("=", string.Empty);
                }
                catch { }

                try
                {
                    if (NumberHelper.ValidateNumber(sikvalue))
                    {
                        sikvalue = sikvalue.Split('\"')[0];
                    }
                    else
                    {
                        sikvalue = sikvalue.Split('\"')[0];
                    }
                }
                catch
                {
                    sikvalue = sikvalue.Split('\"')[0];
                }


                string getdata = "http://www.linkedin.com/groups?viewMembers=&gid=" + gid + "&sik=" + sikvalue + "&split_page=1";
                pageSource = HttpHelper.getHtmlfromUrl(new Uri(getdata));
                RgxGroupData = System.Text.RegularExpressions.Regex.Split(pageSource, "name=\"fromEmail\"");

                try
                {
                    FromId = RgxGroupData[1].Split('=')[1];
                    FromId = FromId.Replace("id", string.Empty).Replace("\"", string.Empty).Trim().ToString();
                }
                catch { }
            }
            catch
            {
                return FromId;
            }


            return FromId;
        }
        #endregion

        #region FromName
        public string FromName(ref GlobusHttpHelper HttpHelper)
        {
            try
            {
                string FromNm = string.Empty;

                string pageSource = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/profile/edit?trk=nav_responsive_sub_nav_edit_profile"));

                string[] RgxGroupData = System.Text.RegularExpressions.Regex.Split(pageSource, "fmt_full_display_name");

               
                foreach (var fromname in RgxGroupData)
                {
                    if (fromname.Contains("\":\""))
                    {
                        try
                        {
                            if (!(fromname.Contains("<!DOCTYPE html>")))
                            {
                                try
                                {
                                    int StartIndex = fromname.IndexOf("\":\"");
                                    string start = fromname.Substring(StartIndex);
                                    int endIndex = start.IndexOf("i18n_optional_not_pinyin");
                                    FromNm = start.Substring(0, endIndex).Replace("\"", string.Empty).Replace("\":\"", string.Empty);
                                    FromNam = FromNm.Split(',')[0].Replace(":", string.Empty).Replace("\\u002d","-");
                                }
                                catch
                                { }
                                try
                                {
                                    if (string.IsNullOrEmpty(FromNm) || FromNm.Contains("LastName"))
                                    {
                                    int StartIndex = fromname.IndexOf("\":\"");
                                    string start = fromname.Substring(StartIndex);
                                    int endIndex = start.IndexOf(",");
                                    FromNm = start.Substring(0, endIndex).Replace("\"", string.Empty).Replace("\":\"", string.Empty);
                                    FromNam = FromNm.Split(',')[0].Replace(":", string.Empty);
                                    }
                                }
                                catch
                                { }
                            }
                        }
                        catch { }
                    }
                }



                return FromNam;


            }
            catch (Exception ex)
            {
                return FromNam;
            }



        }
        #endregion

        #region PostCreateGroupNames
        public Dictionary<string, string> PostAddGroupNames(ref GlobusHttpHelper HttpHelper, List<string> FriendsProfileIDs)
        {
            try
            {
                FriendsProfileIDs = FriendsProfileIDs.Distinct().ToList();
                foreach (var friendID in FriendsProfileIDs)
                {
                    try
                    {

                        string ProfileID = string.Empty;
                        if (Globals.groupStatusString == "API")
                        {
                            string  ProfileUrl = "https://www.linkedin.com/contacts/view?id=" + ProfileID + "";
                            string profilePageSource = HttpHelper.getHtmlfromUrl1(new Uri(ProfileUrl));
                            ProfileID = Utils.getBetween(profilePageSource, "id=", ",").Replace("\"", "");
                            
                        }
                        else
                        {

                            ProfileID = friendID;
                        }

                        string pageSourceforGroup = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/profile/view?id=" + ProfileID));

                        if (pageSourceforGroup.Contains("Your address book is currently unavailable. Please check again later"))
                        {
                            Log("[ " + DateTime.Now + " ] => [ Your address book is currently unavailable. Please check again later ! ]");
                            Log("[ " + DateTime.Now + " ] => [ Please Wait for sometimes ! ]");
                            System.Threading.Thread.Sleep(1 * 60 * 1000);

                            pageSourceforGroup = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/profile/view?id=" + ProfileID));
                        }

                        if (pageSourceforGroup.Contains("You and this LinkedIn user don’t know anyone in common") || pageSourceforGroup.Contains("You and this LinkedIn user don&#8217;t know anyone in common"))
                        {
                            Log("[ " + DateTime.Now + " ] => [ You and this LinkedIn user don’t know anyone in common With Profile URL >>> " + "http://www.linkedin.com/profile/view?id=" + ProfileID + " ]");
                            continue;
                        }

                        //groups-name group-data
                        string[] RgxGroupDataforGroup = System.Text.RegularExpressions.Regex.Split(pageSourceforGroup, "link_groupRegistration");
                        string[] RgxGroupDataName = System.Text.RegularExpressions.Regex.Split(pageSourceforGroup, "fmt__profileUserFullName");

                        #region By Prabhat Sir Commented By san
                        //foreach (var Nm in RgxGroupDataName)
                        //{
                        //    if (Nm.Contains("class=\"given-name\">"))
                        //    {
                        //        if (Nm.IndexOf(">") > 0)
                        //        {
                        //            int SIndex = Nm.IndexOf(">");
                        //            string Start = Nm.Substring(SIndex);
                        //            int EndIndex = Start.IndexOf("class=\"family-name\"");
                        //            MemFName = Start.Substring(0, EndIndex).Replace(">", string.Empty).Replace("<", string.Empty).Replace("/", string.Empty).Replace("span", string.Empty).Trim();

                        //            int SIndex2 = Nm.IndexOf("class=\"family-name\"");
                        //            string Start2 = Nm.Substring(SIndex2);
                        //            int EndIndex2 = Start2.IndexOf("</h1>");
                        //            MemLName = Start2.Substring(0, EndIndex2).Replace("family-name", string.Empty).Replace("class=", string.Empty).Replace("<", string.Empty).Replace(">", string.Empty).Replace("/", string.Empty).Replace("span", string.Empty).Replace("\"", string.Empty).Replace("\n", string.Empty).Trim();
                        //            MemFullName = MemFName + "-" + MemLName;
                        //        }
                        //    }
                        //}


                        //foreach (var Groups in RgxGroupDataforGroup)
                        //{
                        //    if (Groups.Contains("class=\"fn org\""))
                        //    {
                        //        if (Groups.IndexOf("groupRegistration") > 0)
                        //        {
                        //            int startindex2 = Groups.IndexOf("groupRegistration");
                        //            string start2 = Groups.Substring(startindex2);
                        //            int endIndex2 = start2.IndexOf("csrfToken");  //groupRegistration?gid=85746&amp;csrfToken
                        //            GroupId = start2.Substring(0, endIndex2).Replace("groupRegistration", string.Empty).Replace("?gid=", string.Empty).Replace("&amp", string.Empty).Replace(";", string.Empty).Replace("csrfToken", string.Empty);
                        //            GroupId = ":" + GroupId;


                        //            int startindex1 = Groups.IndexOf("class=\"fn org\"");
                        //            string start1 = Groups.Substring(startindex1);
                        //            int endIndex1 = start1.IndexOf("</strong>");
                        //            GroupName = start1.Substring(0, endIndex1).Replace("class=\"fn org\"", string.Empty).Replace(">", string.Empty);
                        //            GroupName = MemFullName + ":" + GroupName;

                        //            try
                        //            {
                        //                GroupDtl.Add(GroupName, GroupId);
                        //            }
                        //            catch { }

                        //        }
                        //        else
                        //        {
                        //            continue;
                        //        }




                        //    }

                        //}
                        #endregion

                        #region CodeChabgeBysanjeev
                        string MemberName = string.Empty;
                        try
                        {
                            MemberName = pageSourceforGroup.Substring(pageSourceforGroup.IndexOf("<span class=\"full-name\" dir=\"auto\">"), (pageSourceforGroup.IndexOf(",", pageSourceforGroup.IndexOf("<span class=\"full-name\" dir=\"auto\">")) - pageSourceforGroup.IndexOf("<span class=\"full-name\" dir=\"auto\">"))).Replace("<span class=\"full-name\" dir=\"auto\">", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(":", string.Empty).Trim();
                            MemberName = MemberName.Replace(" ", "-");

                            if (MemberName.Contains("</span>"))
                            {
                                MemberName = MemberName.Split('<')[0];
                            }
                        }
                        catch { }

                        if (RgxGroupDataforGroup.Count() < 2)
                        {
                            try
                            {
                                pageSourceforGroup = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/profile/mappers?x-a=profile_v2_groups%2Cprofile_v2_follow%2Cprofile_v2_connections&x-p=profile_v2_discovery%2Erecords%3A4%2Ctop_card%2EprofileContactsIntegrationStatus%3A0%2Cprofile_v2_comparison_insight%2Edistance%3A1%2Cprofile_v2_right_fixed_discovery%2Eoffset%3A0%2Cprofile_v2_connections%2Edistance%3A1%2Cprofile_v2_right_fixed_discovery%2Erecords%3A4%2Cprofile_v2_network_overview_insight%2Edistance%3A1%2Cprofile_v2_right_top_discovery_teamlinkv2%2Eoffset%3A0%2Cprofile_v2_right_top_discovery_teamlinkv2%2Erecords%3A4%2Cprofile_v2_discovery%2Eoffset%3A0%2Cprofile_v2_summary_upsell%2EsummaryUpsell%3Atrue%2Cprofile_v2_network_overview_insight%2EnumConn%3A1668%2Ctop_card%2Etc%3Atrue&x-oa=bottomAliases&id=" + ProfileID + "&locale=&snapshotID=&authToken=&authType=name&invAcpt=&notContactable=&primaryAction=&isPublic=false&sfd=true&_=1366115853014"));

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Error >>> " + ex.StackTrace);
                            }
                        }

                        RgxGroupDataforGroup = System.Text.RegularExpressions.Regex.Split(pageSourceforGroup, "link_groupRegistration");
                        RgxGroupDataName = System.Text.RegularExpressions.Regex.Split(pageSourceforGroup, "class=\"full-name\">");
                        RgxGroupDataforGroup = RgxGroupDataforGroup.Skip(1).ToArray();
                        if (pageSourceforGroup.Contains("link_groupRegistration"))
                        {
                            foreach (string item in RgxGroupDataforGroup)
                            {
                                string GroupNames = string.Empty;
                                string GroupIds = string.Empty;
                                string GroupMemberCount = string.Empty;
                                string tempGroupNames = string.Empty;
                                try
                                {
                                    if (!item.Contains("<!DOCTYPE html>") && item.Contains("\"name\":\""))
                                    {
                                        try
                                        {
                                            tempGroupNames = item.Substring(item.IndexOf("\"name\":\""), (item.IndexOf("\",\"", item.IndexOf("\"name\":\"")) - item.IndexOf("\"name\":\""))).Replace("\"name\":\"", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Trim();
                                            GroupNames = MemberName + ":" + tempGroupNames;

                                        }
                                        catch { }
                                        try
                                        {
                                            #region commented
                                            //int startindex = item.IndexOf("gid=");
                                            //if (startindex > 0)
                                            //{
                                            //    string start = item.Substring(startindex).Replace("gid=", "");
                                            //    int endindex = start.IndexOf("&csrfToken=");
                                            //    if (endindex == -1)
                                            //    {
                                            //        endindex = start.IndexOf("&");
                                            //    }
                                            //    string end = start.Substring(0, endindex);
                                            //    GroupIds = end;
                                            //} 
                                            #endregion

                                            int startindex = item.IndexOf("/groups?gid=");
                                            if (startindex > 0)
                                            {
                                                string start = item.Substring(startindex).Replace("/groups?gid=", "");
                                                int endindex = start.IndexOf("&");
                                                if (endindex == -1)
                                                {
                                                    endindex = start.IndexOf("&goback=");
                                                }
                                                string end = start.Substring(0, endindex);
                                                GroupIds = end;
                                            }
                                        }
                                        catch { }

                                        if (string.IsNullOrEmpty(GroupIds))
                                        {
                                            try
                                            {
                                                GroupIds = item.Substring(item.IndexOf("groupRegistration?gid="), (item.IndexOf("&", item.IndexOf("groupRegistration?gid=")) - item.IndexOf("groupRegistration?gid="))).Replace("groupRegistration?gid=", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace("&amp", "").Trim();
                                            }
                                            catch { }
                                        }

                                        if (!string.IsNullOrEmpty(GroupIds))
                                        {
                                            try
                                            {
                                                string membercountPageSource = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/groups?gid=" + GroupIds));
                                                GroupMemberCount = getBetween(membercountPageSource, "class=\"member-count identified\">", "</a>").Replace(",","").Trim(); ;
                                            }
                                            catch { }
                                        }
                                        try
                                        {
                                            GroupDtl.Add(GroupNames, GroupIds);

                                            string LinkedInDeskTop = Globals.DesktopFolder + "\\InBoardProGetData" + "FriendsGroupInfo" + ".csv";
                                            if (!File.Exists(LinkedInDeskTop))
                                            {
                                                string Header = "FriendName" + "," + "GroupNames" + "," + "GroupUrl" + "," + "Total Member Of Group";
                                                GlobusFileHelper.AppendStringToTextfileNewLine(Header, LinkedInDeskTop);
                                            }

                                            string LDS_FinalData = MemberName.Replace(",", ";") + "," + tempGroupNames.Replace(",", ";") + "," + "https://www.linkedin.com/groups?gid=" + GroupIds + "," + GroupMemberCount;
                                            GlobusFileHelper.AppendStringToTextfileNewLine(LDS_FinalData, LinkedInDeskTop);
                                        }
                                        catch { }
                                    }
                                }
                                catch { }
                            }
                        }
                        #endregion
                    }
                    catch { }



                    int delay = RandomNumberGenerator.GenerateRandom(minDelay, maxDelay);
                    Log("[ " + DateTime.Now + " ] => [ Delay for : " + delay + " Seconds ]");
                    Thread.Sleep(delay * 1000);
                }
                return GroupDtl;
             }
            catch (Exception ex)
            {
                return GroupDtl;
            }
        }
        #endregion

        #region PostCreateGroupNames
        public Dictionary<string, string> PostAddPendingGroupNames(ref GlobusHttpHelper HttpHelper,  string FriendsProfileIDs)
        {
            try
            {
                string emailid = FriendsProfileIDs.Split(':')[0].ToString();
                string friendID = FriendsProfileIDs.Split(':')[1].ToString();

                    try
                    {
                        string pageSourceforGroup = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/profile/view?id=" + friendID));

                        if (pageSourceforGroup.Contains("Your address book is currently unavailable. Please check again later"))
                        {
                            Log("[ " + DateTime.Now + " ] => [ Your address book is currently unavailable. Please check again later ! ]");
                            Log("[ " + DateTime.Now + " ] => [ Please Wait for sometimes ! ]");
                            System.Threading.Thread.Sleep(1 * 60 * 1000);

                            pageSourceforGroup = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/profile/view?id=" + friendID));
                        }

                        if (pageSourceforGroup.Contains("You and this LinkedIn user don’t know anyone in common") || pageSourceforGroup.Contains("You and this LinkedIn user don&#8217;t know anyone in common"))
                        {
                            Log("[ " + DateTime.Now + " ] => [ You and this LinkedIn user don’t know anyone in common With Profile URL >>> " + "http://www.linkedin.com/profile/view?id=" + friendID + " ]");
                            //continue;
                        }

                        //groups-name group-data
                        string[] RgxGroupDataforGroup = System.Text.RegularExpressions.Regex.Split(pageSourceforGroup, "link_groupRegistration");
                        string[] RgxGroupDataName = System.Text.RegularExpressions.Regex.Split(pageSourceforGroup, "fmt__profileUserFullName");

                  
                        #region CodeChabgeBysanjeev
                        string MemberName = string.Empty;
                        try
                        {
                            MemberName = pageSourceforGroup.Substring(pageSourceforGroup.IndexOf("fmt__profileUserFullName"), (pageSourceforGroup.IndexOf(",", pageSourceforGroup.IndexOf("fmt__profileUserFullName")) - pageSourceforGroup.IndexOf("fmt__profileUserFullName"))).Replace("fmt__profileUserFullName", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(":", string.Empty).Trim();
                            MemberName = MemberName.Replace(" ", "-");
                        }
                        catch { }


                        try
                        {
                            pageSourceforGroup = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/myGroups?trk=nav_responsive_sub_nav_groups"));
                        }
                        catch { }

                        RgxGroupDataforGroup = System.Text.RegularExpressions.Regex.Split(pageSourceforGroup, "<div class=\"media-content\">");
                        RgxGroupDataName = System.Text.RegularExpressions.Regex.Split(pageSourceforGroup, "class=\"full-name\">");

                        foreach (string item in RgxGroupDataforGroup)
                        {
                            string GroupNames = string.Empty;
                            string GroupIds = string.Empty;

                            try
                            {
                                if (!item.Contains("<!DOCTYPE html>") && item.Contains("/groups") && item.Contains("Membership Pending"))
                                {
                                    try
                                    {
                                        if (item.Contains("class=\"public\""))
                                        {
                                            int startindex = item.IndexOf("class=\"public\"");
                                            string start = item.Substring(startindex).Replace("class=\"public\"", string.Empty);
                                            int endindex = start.IndexOf("</a>");
                                            string end = start.Substring(0, endindex).Replace(">",string.Empty);
                                            GroupNames = emailid + ":" + end;
                                        }
                                        else if (item.Contains("class=\"private\""))
                                        {
                                            int startindex = item.IndexOf("class=\"private\"");
                                            string start = item.Substring(startindex).Replace("class=\"private\"", string.Empty);
                                            int endindex = start.IndexOf("</a>");
                                            string end = start.Substring(0, endindex).Replace(">", string.Empty);
                                            GroupNames = emailid + ":" + end;
                                                                                    }
                                    }
                                    catch { }
                                    try
                                    {
                                        int startindex = item.IndexOf("gid=");
                                        if (startindex > 0)
                                        {
                                            string start = item.Substring(startindex).Replace("gid=", "");
                                            int endindex = start.IndexOf("&amp");
                                            string end = start.Substring(0, endindex);
                                            GroupIds = end;
                                        }
                                    }
                                    catch { }

                                    if (string.IsNullOrEmpty(GroupIds))
                                    {
                                        try
                                        {
                                            GroupIds = item.Substring(item.IndexOf("groupRegistration?gid="), (item.IndexOf("&", item.IndexOf("groupRegistration?gid=")) - item.IndexOf("groupRegistration?gid="))).Replace("groupRegistration?gid=", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace("&amp", "").Trim();
                                        }
                                        catch { }
                                    }

                                    try
                                    {
                                        GroupDtl.Add(GroupNames, GroupIds);
                                    }
                                    catch { }
                                }
                            }
                            catch { }
                        }
                        #endregion
                    }
                    catch { }
                //}
                return GroupDtl;
            }
            catch (Exception ex)
            {
                return GroupDtl;
            }
        }
        #endregion

        #region PostCreateGroupNames
        public void ScrapeFriendsGroup(ref GlobusHttpHelper HttpHelper, List<string> FriendsProfileIDs, string username)
        {
            try
            {
                string MainUrl = string.Empty;

                FriendsProfileIDs = FriendsProfileIDs.Distinct().ToList();
                
                foreach (var friendID in FriendsProfileIDs)
                {
                    try
                    {
                        MainUrl = "http://www.linkedin.com/profile/view?id=" + friendID;
                        string pageSourceforGroup = HttpHelper.getHtmlfromUrl1(new Uri(MainUrl));

                        if (pageSourceforGroup.Contains("Your address book is currently unavailable. Please check again later"))
                        {
                            Logger("[ " + DateTime.Now + " ] => [ Your address book is currently unavailable. Please check again later ! ]");
                            Logger("[ " + DateTime.Now + " ] => [ Please Wait for sometimes ! ]");
                            System.Threading.Thread.Sleep(1 * 60 * 1000);

                            pageSourceforGroup = HttpHelper.getHtmlfromUrl1(new Uri(MainUrl));
                        }

                        if (pageSourceforGroup.Contains("You and this LinkedIn user don’t know anyone in common") || pageSourceforGroup.Contains("You and this LinkedIn user don&#8217;t know anyone in common"))
                        {
                            Logger("[ " + DateTime.Now + " ] => [ You and this LinkedIn user don’t know anyone in common With Profile URL >>> " + "http://www.linkedin.com/profile/view?id=" + friendID + " ]");
                            continue;
                        }

                         
                        string[] RgxGroupDataforGroup = System.Text.RegularExpressions.Regex.Split(pageSourceforGroup, "link_groupRegistration");
                        string[] RgxGroupDataName = System.Text.RegularExpressions.Regex.Split(pageSourceforGroup, "fmt__profileUserFullName");

                        #region CodeChabgeBysanjeev
                        string MemberName = string.Empty;
                        try
                        {
                            int startIndex = pageSourceforGroup.IndexOf("<span class=\"full-name\">");
                            string start = pageSourceforGroup.Substring(startIndex).Replace("<span class=\"full-name\">", string.Empty);
                            int endIndex = start.IndexOf("</span>");
                            string end = start.Substring(0, endIndex).Replace("</span>", string.Empty);
                            MemberName = end.Trim();

                            //MemberName = pageSourceforGroup.Substring(pageSourceforGroup.IndexOf("<span class=\"full-name\">"), (pageSourceforGroup.IndexOf("</span>", pageSourceforGroup.IndexOf("fmt__profileUserFullName")) - pageSourceforGroup.IndexOf("<span class=\"full-name\">"))).Replace("<span class=\"full-name\">", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(":", string.Empty).Trim();
                            //MemberName = MemberName.Replace(" ", "-");
                        }
                        catch { }

                        if (RgxGroupDataforGroup.Count() < 2)
                        {
                            try
                            {
                                pageSourceforGroup = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/profile/mappers?x-a=profile_v2_groups%2Cprofile_v2_follow%2Cprofile_v2_connections&x-p=profile_v2_discovery%2Erecords%3A4%2Ctop_card%2EprofileContactsIntegrationStatus%3A0%2Cprofile_v2_comparison_insight%2Edistance%3A1%2Cprofile_v2_right_fixed_discovery%2Eoffset%3A0%2Cprofile_v2_connections%2Edistance%3A1%2Cprofile_v2_right_fixed_discovery%2Erecords%3A4%2Cprofile_v2_network_overview_insight%2Edistance%3A1%2Cprofile_v2_right_top_discovery_teamlinkv2%2Eoffset%3A0%2Cprofile_v2_right_top_discovery_teamlinkv2%2Erecords%3A4%2Cprofile_v2_discovery%2Eoffset%3A0%2Cprofile_v2_summary_upsell%2EsummaryUpsell%3Atrue%2Cprofile_v2_network_overview_insight%2EnumConn%3A1668%2Ctop_card%2Etc%3Atrue&x-oa=bottomAliases&id=" + friendID + "&locale=&snapshotID=&authToken=&authType=name&invAcpt=&notContactable=&primaryAction=&isPublic=false&sfd=true&_=1366115853014"));

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Error >>> " + ex.StackTrace);
                            }
                        }
                        if (pageSourceforGroup.Contains("link_groupRegistration"))
                        {
                            RgxGroupDataforGroup = System.Text.RegularExpressions.Regex.Split(pageSourceforGroup, "link_groupRegistration");
                            RgxGroupDataName = System.Text.RegularExpressions.Regex.Split(pageSourceforGroup, "class=\"full-name\">");

                            foreach (string item in RgxGroupDataforGroup)
                            {
                                string GroupUrl = string.Empty;
                                string GroupNames = string.Empty;
                                string GroupIds = string.Empty;

                                try
                                {
                                    if (!item.Contains("<!DOCTYPE html>") && item.Contains("\"name\":\""))
                                    {
                                        try
                                        {
                                            GroupNames = item.Substring(item.IndexOf("\"name\":\""), (item.IndexOf("\",\"", item.IndexOf("\"name\":\"")) - item.IndexOf("\"name\":\""))).Replace("\"name\":\"", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Trim();

                                        }
                                        catch { }

                                        try
                                        {
                                            //int startindex = item.IndexOf("groups?gid=");
                                            //if (startindex > 0)
                                            //{
                                            //    string start = item.Substring(startindex).Replace("groups?gid=","&");
                                            //    int endindex = start.IndexOf("&");
                                            //    string end = start.Substring(0, endindex);
                                            //    GroupIds = end;

                                            //}
                                            GroupIds = getBetween(item, "groups?gid=", "&");
                                        }
                                        catch { }

                                        if (string.IsNullOrEmpty(GroupIds))
                                        {
                                            try
                                            {
                                                GroupIds = item.Substring(item.IndexOf("groupRegistration?gid="), (item.IndexOf("&", item.IndexOf("groupRegistration?gid=")) - item.IndexOf("groupRegistration?gid="))).Replace("groupRegistration?gid=", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace("&amp", "").Trim();

                                            }
                                            catch { }
                                        }

                                        if (string.IsNullOrEmpty(GroupIds))
                                        {
                                            try
                                            {
                                                //GroupIds = item.Substring(item.IndexOf("groupRegistration?gid="), (item.IndexOf("&", item.IndexOf("groupRegistration?gid=")) - item.IndexOf("groupRegistration?gid="))).Replace("groupRegistration?gid=", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace("&amp", "").Trim();
                                                int startIndex = item.IndexOf("groupRegistration?gid=");
                                                string start = item.Substring(startIndex);
                                                int endIndex = start.IndexOf("&");
                                                string end = start.Substring(0, endIndex).Replace("&", string.Empty);
                                                GroupIds = end.Trim();
                                            }
                                            catch { }
                                        }

                                        GroupUrl = "http://www.linkedin.com/groups?gid=" + GroupIds;


                                        try
                                        {
                                            #region Data Saved In CSV File

                                            if (!string.IsNullOrEmpty(GroupNames))
                                            {
                                                try
                                                {
                                                    string CSVHeader = "GroupUrl" + "," + "GroupName" + "," + "FriendName" + "," + "SearchByID";
                                                    string CSV_Content = GroupUrl.Replace(",", ";") + "," + GroupNames.Replace(",", ";") + "," + MemberName.Replace(",", ";") + "," + username;
                                                    CSVUtilities.ExportDataCSVFile(CSVHeader, CSV_Content, Globals.path_LinkedinFriendsGroupScraper);
                                                    Logger("[ " + DateTime.Now + " ] => [ GroupUrl: " + GroupUrl + " ]");
                                                    Logger("[ " + DateTime.Now + " ] => [ GroupName: " + GroupNames + " ]");
                                                    Logger("[ " + DateTime.Now + " ] => [ FriendName: " + MemberName +" ]");
                                                    Logger("[ " + DateTime.Now + " ] => [ SearchByID: " + username + " ]");
                                                    Logger("[ " + DateTime.Now + " ] => [ Data Saved In CSV File ! ]");


                                                }
                                                catch { }

                                            }
                                            #endregion
                                        }
                                        catch { }
                                    }
                                }
                                catch { }
                            }
                        }
                        else
                        {
                            RgxGroupDataforGroup = System.Text.RegularExpressions.Regex.Split(pageSourceforGroup, "link_media");
                            RgxGroupDataName = System.Text.RegularExpressions.Regex.Split(pageSourceforGroup, "class=\"full-name\">");

                            foreach (string item in RgxGroupDataforGroup)
                            {
                                string GroupUrl = string.Empty;
                                string GroupNames = string.Empty;
                                string GroupIds = string.Empty;

                                try
                                {
                                    if (!item.Contains("<!DOCTYPE html>") && item.Contains("\"name\":\""))
                                    {
                                        try
                                        {
                                            //GroupNames = item.Substring(item.IndexOf("\"name\":\""), (item.IndexOf("\",\"", item.IndexOf("\"name\":\"")) - item.IndexOf("\"name\":\""))).Replace("\"name\":\"", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Trim();
                                            int startindex = item.IndexOf("\"name\"");
                                            string start = item.Substring(startindex).Replace("\"name\"",string.Empty).Replace(":", string.Empty);
                                            int endindex = start.IndexOf(",");
                                            string end = start.Substring(0, endindex).Replace("\"",string.Empty);
                                            GroupNames = end.Trim();
                                        }
                                        catch { }

                                        try
                                        {                                           
                                                int startindex1 = item.IndexOf("\"groupID\"");
                                                string start = item.Substring(startindex1).Replace("\"groupID\"",string.Empty).Replace(":", string.Empty);
                                                int endindex = start.IndexOf(",");
                                                string end = start.Substring(0, endindex);
                                                GroupIds = end.Trim();
                                          
                                        }
                                        catch { }

                                        if (string.IsNullOrEmpty(GroupIds))
                                        {
                                            try
                                            {
                                                GroupIds = item.Substring(item.IndexOf("groupRegistration?gid="), (item.IndexOf("&", item.IndexOf("groupRegistration?gid=")) - item.IndexOf("groupRegistration?gid="))).Replace("groupRegistration?gid=", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace("&amp", "").Trim();

                                            }
                                            catch { }
                                        }

                                        GroupUrl = "http://www.linkedin.com/groups?gid=" + GroupIds;


                                        try
                                        {
                                            #region Data Saved In CSV File

                                            if (!string.IsNullOrEmpty(GroupNames))
                                            {
                                                try
                                                {
                                                    string CSVHeader = "GroupUrl" + "," + "GroupName" + "," + "FriendName" + "," + "SearchByID";
                                                    string CSV_Content = GroupUrl.Replace(",", ";") + "," + GroupNames.Replace(",", ";") + "," + MemberName.Replace(",", ";") + "," + username;
                                                    CSVUtilities.ExportDataCSVFile(CSVHeader, CSV_Content, Globals.path_LinkedinFriendsGroupScraper);
                                                    Logger("[ " + DateTime.Now + " ] => [ GroupUrl: " + GroupUrl.Replace(",", ";") + " ]");
                                                    Logger("[ " + DateTime.Now + " ] => [ GroupName: " + GroupNames.Replace(",", ";") + " ]");
                                                    Logger("[ " + DateTime.Now + " ] => [ FriendName: " + MemberName.Replace(",", ";") + " ]");
                                                    Logger("[ " + DateTime.Now + " ] => [ SearchByID: " + username + " ]");
                                                    Logger("[ " + DateTime.Now + " ] => [ Data Saved In CSV File ! ]");


                                                }
                                                catch { }

                                            }
                                            #endregion
                                        }
                                        catch { }
                                    }
                                }
                                catch { }
                            }
                        }
                        #endregion
                    }
                    catch { }
                }
                
            }
            catch (Exception ex)
            {
               
            }
        }
        #endregion

        #region Fromgid
        public string Fromgid(ref GlobusHttpHelper HttpHelper, string PostGrpName,string ResponseStatusMsg)
        {
            try
            {
                string Gid = string.Empty;
                string Gnm = string.Empty;

                //string pageSource = HttpHelper.getHtmlfromUrl(new Uri(ResponseStatusMsg));
                string pageSource = ResponseStatusMsg;
                string[] RgxGroupData = System.Text.RegularExpressions.Regex.Split(pageSource, "<a href=\"/groups");

                foreach (var FromEmlId in RgxGroupData)
                {
                    try
                    {
                        if (FromEmlId.Contains("?gid"))
                        {
                            int StartIndex = FromEmlId.IndexOf("?gid");
                            string start = FromEmlId.Substring(StartIndex);
                            int endIndex = start.IndexOf("trk=hb_side_g");
                            Gid = start.Substring(0, endIndex).Replace("?gid=", string.Empty).Replace("&amp;", string.Empty);

                            int StartIndex1 = FromEmlId.IndexOf("hb_side_g");
                            string start1 = FromEmlId.Substring(StartIndex1);
                            int endIndex1 = start1.IndexOf("</a>");
                            FromNam = start1.Substring(0, endIndex1).Replace("hb_side_g\">", string.Empty).Replace("&amp;", string.Empty);

                            if (PostGrpName == FromNam)
                            {
                                FromID = Gid;
                            }
                        }
                    }
                    catch { }

                }

                return FromID;
            }
            catch (Exception ex)
            {
                return FromID;
            }
        }
        #endregion

        #region Events logger
        public static Events logger = new Events();
        
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

        #region Events logger
        public static Events loggerFriendsGroup = new Events();

        private void Logger(string log)
        {
            try
            {
                EventsArgs loggerEventsArgs = new EventsArgs(log);
                loggerFriendsGroup.LogText(loggerEventsArgs);
            }
            catch (Exception ex)
            {

            }

        }
        #endregion

        #region ScriptHTML
        protected string StripHTML(string Txt)
        {
            return Regex.Replace(Txt, "<(.|\\n)*?>", string.Empty);
        }
        #endregion

        #region PostGroupAddFinal
        public string PostGroupAddFinal(string Screen_name, string pass , int mindelay , int maxdelay)
        {
            string postdata = string.Empty;
            string postUrl = string.Empty;
            string ResLogin = string.Empty;
            string csrfToken = string.Empty;
            string sourceAlias = string.Empty;
            string regCsrfParam = string.Empty;
            string ReturnString = string.Empty;
            string GroupName = string.Empty;

            try
            {
                string MessageText = string.Empty;
                string PostedMessage = string.Empty;

                string pageSource = HttpHelper.getHtmlfromUrl1(new Uri("https://www.linkedin.com/home?trk=hb_tab_home_top"));
                if (pageSource.Contains("csrfToken"))
                {
                    try
                    {
                        csrfToken = pageSource.Substring(pageSource.IndexOf("csrfToken"), 50);
                        string[] Arr = csrfToken.Split('&');
                        csrfToken = Arr[0];
                        csrfToken = csrfToken.Replace(":", "%3A").Replace("csrfToken", "").Replace("\"", string.Empty).Replace("value", string.Empty).Replace("\n",string.Empty).Replace(">",string.Empty).Replace("<script src",string.Empty).Replace("cs", string.Empty).Replace("id", string.Empty).Replace("=", string.Empty);
                        csrfToken = csrfToken.Trim();
                       
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine("Error >>> " + ex.StackTrace);
                    }
                }

                if (pageSource.Contains("sourceAlias"))
                {
                    try
                    {
                        sourceAlias = pageSource.Substring(pageSource.IndexOf("sourceAlias"), 100);
                        string[] Arr = sourceAlias.Split('"');
                        sourceAlias = Arr[2];
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error >>> " + ex.StackTrace);
                    }
                }

                try
                {
                    int SourceAliasStart = pageSource.IndexOf("regCsrfParam");
                    if (SourceAliasStart > 0)
                    {
                        try
                        {

                            regCsrfParam = pageSource.Substring(pageSource.IndexOf("regCsrfParam"), 100);
                            string[] Arr = regCsrfParam.Split('"');
                            regCsrfParam = Arr[2].Replace(@"\", string.Empty).Replace("//", string.Empty);
                        }
                        catch
                        {
                        }
                    }

                }
                catch { }


               
                //postUrl = "https://www.linkedin.com/uas/login-submit";
                //postdata = "session_key=" + Screen_name + "&session_password=" + pass + "&source_app=&trk=guest_home_login&session_redirect=&csrfToken=" + csrfToken + "&sourceAlias=" + sourceAlias;
                
                postUrl = "https://www.linkedin.com/uas/login-submit";
                postdata = "isJsEnabled=true&source_app=&tryCount=&session_key=" + Uri.EscapeDataString(Screen_name) + "&session_password=" + pass + "&signin=Sign%20In&session_redirect=&loginCsrfParam=" + regCsrfParam + "&csrfToken=" + csrfToken + "&sourceAlias=" + sourceAlias;

                ResLogin = HttpHelper.postFormData(new Uri(postUrl), postdata);

                try
                {
                    foreach (var ItemMemId in MemId) // MemId ~= FriendsId
                    {
                        try
                        {
                            //string PageSource = HttpHelper.getHtmlfromUrl(new Uri("http://www.linkedin.com/profile/view?id=" + ItemMemId));
                            string PageSource = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/profile/mappers?x-a=profile_v2_groups%2Cprofile_v2_follow%2Cprofile_v2_connections&x-p=profile_v2_discovery%2Erecords%3A4%2Ctop_card%2EprofileContactsIntegrationStatus%3A0%2Cprofile_v2_comparison_insight%2Edistance%3A1%2Cprofile_v2_right_fixed_discovery%2Eoffset%3A0%2Cprofile_v2_connections%2Edistance%3A1%2Cprofile_v2_right_fixed_discovery%2Erecords%3A4%2Cprofile_v2_network_overview_insight%2Edistance%3A1%2Cprofile_v2_right_top_discovery_teamlinkv2%2Eoffset%3A0%2Cprofile_v2_right_top_discovery_teamlinkv2%2Erecords%3A4%2Cprofile_v2_discovery%2Eoffset%3A0%2Cprofile_v2_summary_upsell%2EsummaryUpsell%3Atrue%2Cprofile_v2_network_overview_insight%2EnumConn%3A1668%2Ctop_card%2Etc%3Atrue&x-oa=bottomAliases&id=" + ItemMemId + "&locale=&snapshotID=&authToken=&authType=name&invAcpt=&notContactable=&primaryAction=&isPublic=false&sfd=true"));

                            string[] array = Regex.Split(PageSource, "href=\"/groupRegistration?");
                            string[] array1 = Regex.Split(PageSource, "groupRegistration?");
                            string post = "http://www.linkedin.com/groupRegistration";
                            string PostGroupstatus;
                            string ResponseStatusMsg;
                            string GroupId = string.Empty;
                            string SelItem = string.Empty;
                            bool alreadyExst = true;

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
                                                int startindex = itemGrps.IndexOf("?gid=");
                                                string start = itemGrps.Substring(startindex);
                                                int endIndex = start.IndexOf("csrfToken");  //groupRegistration?gid=85746&amp;csrfToken
                                                GroupId = start.Substring(0, endIndex).Replace("?gid=", string.Empty).Replace("amp", string.Empty).Replace("&",string.Empty).Replace(";", string.Empty).Replace("csrfToken", string.Empty);
                                            }
                                            catch { }
                                        }

                                        foreach (string SelectedGrp in lstExistGroup)//Groups checked/selected in frmMain
                                        {
                                            try
                                            {
                                                foreach (KeyValuePair<string, Dictionary<string, string>> GrpsValue in GrpAdd)
                                                {
                                                    try
                                                    {
                                                        foreach (KeyValuePair<string, string> GroupValue in GrpsValue.Value)
                                                        {
                                                            try
                                                            {
                                                             
                                                                {
                                                                    string ExistGroup = GroupValue.Key.Split(':')[1];
                                                                    string SelectedGroup = SelectedGrp.Split(':')[1];
                                                                    if (ExistGroup == SelectedGroup)
                                                                    {

                                                                        SelItem = GroupValue.Value.Replace(":", string.Empty);

                                                                        string emailID = Screen_name;
                                                                        string groupID = SelItem;
                                                                        string status = "";

                                                                        if (GroupId == SelItem)
                                                                        {
                                                                            PostGroupstatus = post + itemGrps.Split('\"')[0].Replace("amp;", string.Empty).Replace("&goback=","&trk=group-join-button").Replace(" ","");
                                                                            ResponseStatusMsg = HttpHelper.getHtmlfromUrl1(new Uri(PostGroupstatus));

                                                                            if (ResponseStatusMsg.Contains("Welcome to the"))
                                                                            {
                                                                                Log("[ " + DateTime.Now + " ] => [ Group Added : " + SelectedGroup + " With Account : " + Screen_name + " ]");

                                                                                Log("[ " + DateTime.Now + " ] => [ Group Added Successfully ]");
                                                                                ReturnString = "Welcome to the " + SelectedGroup + " group on LinkedIn.";
                                                                                status = ReturnString;
                                                                            }
                                                                            else if (ResponseStatusMsg.Contains("Your request to join the"))
                                                                            {

                                                                                Log("[ " + DateTime.Now + " ] => [ Group Added : " + SelectedGroup + " With Account : " + Screen_name + " ]");
                                                                                Log("[ " + DateTime.Now + " ] => [ Your request has send Successfully to join ]");
                                                                                ReturnString = "Your request to join the " + SelectedGroup + " group has been received";
                                                                                status = ReturnString;
                                                                            }
                                                                            else if (ResponseStatusMsg.Contains("exceeded the maximum number"))
                                                                            {
                                                                                Log("[ " + DateTime.Now + " ] => [ SORRY..You’ve reached or exceeded the maximum number of confirmed and pending groups." + " with : " + Screen_name +" ]" );
                                                                                ReturnString = "You’ve reached or exceeded the maximum number of confirmed and pending groups.";
                                                                                status = ReturnString;

                                                                                GlobusFileHelper.AppendStringToTextfileNewLine(Screen_name + ":" + groupID + ":" + status, Globals.path_JoinFriendsGroup);
                                                                                Log("[ " + DateTime.Now + " ] => [ Looping out of the account as reached maximum" + " With Account : " + Screen_name + " ]");
                                                                                return "";
                                                                            }
                                                                            else if (ResponseStatusMsg.Contains("You're already a member of the "))
                                                                            {
                                                                                if (alreadyExst == true)
                                                                                {
                                                                                    alreadyExst = false;
                                                                                }
                                                                                Log("[ " + DateTime.Now + " ] => [ You're already a member of the " + SelectedGroup + " Group" + " With Account : " + Screen_name + " ]");
                                                                                status = "You're already a member of the " + SelectedGroup + " Group";
                                                                            }
                                                                            else if (ResponseStatusMsg.Contains("Error"))
                                                                            {
                                                                                Log("[ " + DateTime.Now + " ] => [ Error in Request" + " With Account : " + Screen_name + " ]");
                                                                                status = "Error in Request";
                                                                            }
                                                                            else if (ResponseStatusMsg.Contains("Your settings have been updated") || ResponseStatusMsg.Contains("Your membership is pending approval"))
                                                                            {
                                                                                status = "Your settings have been updated. Your membership is pending approval";
                                                                                Log("[ " + DateTime.Now + " ] => [ " + status + " with : " + Screen_name + " ]");
                                                                            }
                                                                            else if (ResponseStatusMsg.Contains("this group has reached its maximum number of members allowed"))
                                                                            {
                                                                                status = "No More Members Allowed";
                                                                                Log("[ " + DateTime.Now + " ] => [ " + status + " with : " + Screen_name + " ]");
                                                                            }
                                                                            else
                                                                            {
                                                                                Log("[ " + DateTime.Now + " ] => [ Request to Group: " + SelectedGroup + " failed with : " + Screen_name + " ]");
                                                                                status = "Failed";
                                                                            }

                                                                            int delay = RandomNumberGenerator.GenerateRandom(mindelay, maxdelay);
                                                                            Log("[ " + DateTime.Now + " ] => [ Delay for : " + delay + " Seconds ]");
                                                                            Thread.Sleep(delay * 1000);

                                                                            GlobusFileHelper.AppendStringToTextfileNewLine(Screen_name + ":" + groupID + ":" + status, Globals.path_JoinFriendsGroup);

                                                                            //clsSettingDB ObjclsSettingDB = new clsSettingDB();
                                                                            //ObjclsSettingDB.InsertOrUpdateSetting("StatusUpdate", "StatusMessage", StringEncoderDecoder.Encode(txtStatusUpd.Text));

                                                                        }
                                                                        else
                                                                        {
                                                                            //Log("Request to Group: " + aaa + " failed with : " + Screen_name);
                                                                            //status = "Failed";
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            catch (Exception ex)
                                                            {
                                                                //Log("[ " + DateTime.Now + " ] => [ Error : " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + " ]");
                                                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> PostGroupAddFinal() ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> PostGroupAddFinal() ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddFriendsGroupErrorLogs);
                                                            }
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        //Log("[ " + DateTime.Now + " ] => [ Error : " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + " ]");
                                                        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> PostGroupAddFinal() ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                                        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> PostGroupAddFinal() ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddFriendsGroupErrorLogs);
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                //Log("[ " + DateTime.Now + " ] => [ Error : " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + " ]");
                                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> PostGroupAddFinal() ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> PostGroupAddFinal() ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddFriendsGroupErrorLogs);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //Log(" failed with : " + Screen_name);
                                        //status = "Failed";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    //Log("[ " + DateTime.Now + " ] => [ Error : " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + " ]");
                                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> PostGroupAddFinal() ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> PostGroupAddFinal() ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddFriendsGroupErrorLogs);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            //Log("[ " + DateTime.Now + " ] => [ Error : " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + " ]");
                            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> PostGroupAddFinal() ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> PostGroupAddFinal() ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddFriendsGroupErrorLogs);
                        }
                    }

                    Log("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED FOR ACCOUNT : " + accountUser + " ]");
                  }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime : " + DateTime.Now + " :: Error --> Add Friends Groups --> PostGroupAddFinal() ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime : " + DateTime.Now + " :: Error --> Add Friends Groups --> PostGroupAddFinal() ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddFriendsGroupErrorLogs);
                }

            }
            catch (Exception ex)
            {
                ReturnString = "Error";
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime : " + DateTime.Now + " :: Error --> Add Friends Groups --> PostGroupAddFinal() ---2--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime : " + DateTime.Now + " :: Error --> Add Friends Groups --> PostGroupAddFinal() ---2--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddFriendsGroupErrorLogs);
            }

            return ReturnString;
        }
        #endregion

        #region PostRemovePendingGroups
        public string PostRemovePendingGroups(string Screen_name, string pass, int mindelay, int maxdelay)
        {
            string postdata = string.Empty;
            string postUrl = string.Empty;
            string ResLogin = string.Empty;
            string csrfToken = string.Empty;
            string sourceAlias = string.Empty;
            string regCsrfParam = string.Empty;
            string ReturnString = string.Empty;
            string GroupName = string.Empty;

            try
            {
                string MessageText = string.Empty;
                string PostedMessage = string.Empty;

                string pageSource = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/home?trk=hb_tab_home_top"));
                if (pageSource.Contains("csrfToken"))
                {
                    try
                    {
                        csrfToken = pageSource.Substring(pageSource.IndexOf("csrfToken"), 50);
                        string[] Arr = csrfToken.Split('&');
                        csrfToken = Arr[0];
                        csrfToken = csrfToken.Replace(":", "%3A").Replace("csrfToken", "").Replace("\"", string.Empty).Replace("value", string.Empty).Replace("cs", string.Empty).Replace("id", string.Empty).Replace("=", string.Empty).Replace(">",string.Empty).Replace("\n",string.Empty).Replace("<script src",string.Empty).Replace("<script typ",string.Empty);
                        csrfToken = csrfToken.Trim();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error >>> " + ex.StackTrace);
                    }
                }

                if (pageSource.Contains("sourceAlias"))
                {
                    try
                    {
                        sourceAlias = pageSource.Substring(pageSource.IndexOf("sourceAlias"), 100);
                        string[] Arr = sourceAlias.Split('"');
                        sourceAlias = Arr[2];
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error >>> " + ex.StackTrace);
                    }
                }

                try
                {
                    int SourceAliasStart = pageSource.IndexOf("regCsrfParam");
                    if (SourceAliasStart > 0)
                    {
                        try
                        {
                            regCsrfParam = pageSource.Substring(pageSource.IndexOf("regCsrfParam"), 100);
                            string[] Arr = regCsrfParam.Split('"');
                            regCsrfParam = Arr[2].Replace(@"\", string.Empty).Replace("//", string.Empty);
                        }
                        catch
                        {
                        }
                    }

                }
                catch { }


                //postUrl = "https://www.linkedin.com/uas/login-submit";
                //postdata = "session_key=" + Screen_name + "&session_password=" + pass + "&source_app=&trk=guest_home_login&session_redirect=&csrfToken=" + csrfToken + "&sourceAlias=" + sourceAlias;

                postUrl = "https://www.linkedin.com/uas/login-submit";
                postdata = "isJsEnabled=true&source_app=&tryCount=&session_key=" + Uri.EscapeDataString(accountUser) + "&session_password=" + pass + "&signin=Sign%20In&session_redirect=&loginCsrfParam=" + regCsrfParam + "&csrfToken=" + csrfToken + "&sourceAlias=" + sourceAlias;

                ResLogin = HttpHelper.postFormData(new Uri(postUrl), postdata);

                string PostGroupstatus;
                string ResponseStatusMsg;
                int counter = 0;

                foreach (string SelectedGrp in lstPendingGroup)
                {
                    counter++;
                    //PostGroupstatus = "http://www.linkedin.com/anet?withdrawJoinRequest=&gid=" + SelectedGrp.Split(':')[2];
                    PostGroupstatus = "http://www.linkedin.com/anet?withdrawJoinRequest=&gid=" + SelectedGrp.Split(':')[2] + "&csrfToken=" + csrfToken;
                    //https://www.linkedin.com/anet?withdrawJoinRequest=&gid=66325&csrfToken=ajax%3A3851816744471823500
                    ResponseStatusMsg = HttpHelper.getHtmlfromUrl1(new Uri(PostGroupstatus));

                    if (ResponseStatusMsg.Contains("Your group request has been successfully withdrawn."))
                    {
                        LoggerRemPendingGrp("[ " + DateTime.Now + " ] => [ Group : " + SelectedGrp.Split(':')[0] + ", Withdrawn With Account : " + Screen_name + " ]");
                        LoggerRemPendingGrp("[ " + DateTime.Now + " ] => [ Group Withdrawn Successfully ]");
                        ReturnString = "Group Withdrawn : " + SelectedGrp.Split(':')[0] + "";
                        GlobusFileHelper.AppendStringToTextfileNewLine(Screen_name + ":" + ReturnString, Globals.path_PendingGroupWithdrawn);
                    }

                    if (counter != lstPendingGroup.Count())
                    {
                        int delay = RandomNumberGenerator.GenerateRandom(mindelay, maxdelay);
                        LoggerRemPendingGrp("[ " + DateTime.Now + " ] => [ Delay for : " + delay + " Seconds ]");
                        Thread.Sleep(delay * 1000);
                    }

                }

            }
            catch (Exception ex)
            {
                ReturnString = "Error";
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime : " + DateTime.Now + " :: Error --> Add Removed Groups --> PostRemovePendingGroups() ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime : " + DateTime.Now + " :: Error --> Add Removed Groups --> PostRemovePendingGroups() ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPendingGroupErrorLogs);
            }

            return ReturnString;

        }
        #endregion

        #region PostInvitationGroups
        public string PostInvitationGroups(string Screen_name, string pass, int mindelay, int maxdelay)
        {
            string postdata = string.Empty;
            string postUrl = string.Empty;
            string ResLogin = string.Empty;
            string csrfToken = string.Empty;
            string sourceAlias = string.Empty;
            string regCsrfParam = string.Empty;
            string ReturnString = string.Empty;
            string GroupName = string.Empty;

            try
            {
                string MessageText = string.Empty;
                string PostedMessage = string.Empty;

                string pageSource = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/home?trk=hb_tab_home_top"));
                if (pageSource.Contains("csrfToken"))
                {
                    try
                    {
                        csrfToken = pageSource.Substring(pageSource.IndexOf("csrfToken"), 50);
                        string[] Arr = csrfToken.Split('&');
                        csrfToken = Arr[0];
                        csrfToken = csrfToken.Replace(":", "%3A").Replace("csrfToken", "").Replace("\"", string.Empty).Replace("value", string.Empty).Replace("cs", string.Empty).Replace("id", string.Empty).Replace("=", string.Empty).Replace(">", string.Empty).Replace("\n", string.Empty).Replace("<script src", string.Empty).Replace("<script typ", string.Empty);
                        csrfToken = csrfToken.Trim();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error >>> " + ex.StackTrace);
                    }
                }

                if (pageSource.Contains("sourceAlias"))
                {
                    try
                    {
                        sourceAlias = pageSource.Substring(pageSource.IndexOf("sourceAlias"), 100);
                        string[] Arr = sourceAlias.Split('"');
                        sourceAlias = Arr[2];
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error >>> " + ex.StackTrace);
                    }
                }

                try
                {
                    int SourceAliasStart = pageSource.IndexOf("regCsrfParam");
                    if (SourceAliasStart > 0)
                    {
                        try
                        {
                            regCsrfParam = pageSource.Substring(pageSource.IndexOf("regCsrfParam"), 100);
                            string[] Arr = regCsrfParam.Split('"');
                            regCsrfParam = Arr[2].Replace(@"\", string.Empty).Replace("//", string.Empty);
                        }
                        catch
                        {
                        }
                    }

                }
                catch { }




                postUrl = "https://www.linkedin.com/uas/login-submit";
                postdata = "isJsEnabled=true&source_app=&tryCount=&session_key=" + Uri.EscapeDataString(accountUser) + "&session_password=" + pass + "&signin=Sign%20In&session_redirect=&loginCsrfParam=" + regCsrfParam + "&csrfToken=" + csrfToken + "&sourceAlias=" + sourceAlias;

                ResLogin = HttpHelper.postFormData(new Uri(postUrl), postdata);

                string PostGroupInvite = string.Empty;
                   
                int counter = 0;

                foreach (string SelectedGrp in lstInvitationGroup)
                {
                    counter++;

                   foreach (var itemEmail in lstEmailsGroupInvite)
	               {
                       try
                       {
                           string GetRemInvitePageSource = HttpHelper.getHtmlfromUrl1(new Uri("https://www.linkedin.com/manageGroup?dispAddMbrs=&gid=" + SelectedGrp.Split(':')[2] + "&invtActn=im-invite&cntactSrc=cs-connections&trk=grpmgr_invite"));
                           string[] Reminvite1 = Regex.Split(GetRemInvitePageSource, "name=\"remIntives\"");
                           string[] Reminvite2 = Regex.Split(Reminvite1[1], "id=");
                           string reminvite = Reminvite2[0].Replace("value=", string.Empty).Replace("\"", string.Empty).Trim();

                           string postData = "csrfToken=" + csrfToken + "&emailRecipients=" + itemEmail.Replace("@", "%40") + "&subAddMbrs=Send+Invitations&gid=" + SelectedGrp.Split(':')[2] + "&invtActn=im-invite&cntactSrc=cs-connections&remIntives=" + reminvite + "&connectionIds=&connectionNames=&contactIDs=&newGroup=false";
                           PostGroupInvite = HttpHelper.postFormData(new Uri("https://www.linkedin.com/manageGroup"), postData);
                       }
                       catch { }
                     
                     if (PostGroupInvite.Contains("You have successfully sent invitations to this group."))
                     {
                         LoggerInviteGrp("[ " + DateTime.Now + " ] => [ Group : " + SelectedGrp.Split(':')[0] + ", Invited to Account : " + itemEmail + " from : " + Screen_name + " ]");
                        LoggerInviteGrp("[ " + DateTime.Now + " ] => [ Group Invited Successfully ]");
                        ReturnString = "Group Invited : " + SelectedGrp.Split(':')[0] + "";
                        GlobusFileHelper.AppendStringToTextfileNewLine(Screen_name + ":" + ReturnString, Globals.path_PendingGroupWithdrawn);
                     }

                   }

                   if (counter != lstInvitationGroup.Count())
                    {
                        int delay = RandomNumberGenerator.GenerateRandom(mindelay, maxdelay);
                        LoggerInviteGrp("[ " + DateTime.Now + " ] => [ Delay for : " + delay + " Seconds ]");
                        Thread.Sleep(delay * 1000);
                    }

                }

            }
            catch (Exception ex)
            {
                ReturnString = "Error";
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime : " + DateTime.Now + " :: Error --> Add Removed Groups --> PostRemovePendingGroups() ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime : " + DateTime.Now + " :: Error --> Add Removed Groups --> PostRemovePendingGroups() ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinPendingGroupErrorLogs);
            }

            return ReturnString;

        }
        #endregion

        #region GetAllGrpMember
        ClsScrapGroupMember obj_ClsScrapGroupMember = new ClsScrapGroupMember();
        public List<string> GetAllGrpMember(ref GlobusHttpHelper httpHelper, string grpId)
        {      
            List<string> lstFriendProfileURLs = new List<string>();
            try
            {
                String pageSource = string.Empty;
                int endPageNo = 26;
                int count = 0;
                bool Repeat = false;
                bool countRepeat = false;
                int repeatCount = 0;
                string repeatMember = string.Empty;

                for (int i = 1; i < endPageNo; i++)
                {
                    int countRepeaT = 0;
                    try
                    {
                        string addedurl = "http://www.linkedin.com/groups?viewMembers=&gid=" + grpId + "&split_page=" + i + "";

                        pageSource = httpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/groups?viewMembers=&gid=" + grpId + "&split_page=" + i + ""));



                        if (string.IsNullOrEmpty(pageSource) || pageSource.Contains("Sorry you are not authorized to perform this action"))
                        {
                            pageSource = httpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/groups?viewMembers=&gid=" + grpId + "&split_page=" + i + ""));

                            if (string.IsNullOrEmpty(pageSource) || pageSource.Contains("Sorry you are not authorized to perform this action"))
                            {
                                break;
                            }
                        }

                        if (i >= 25)
                        {
                            if ((i >= 25) && (!pageSource.Contains("<strong>next&nbsp;&#")) && (!pageSource.Contains("siguiente&nbsp;&#187;")))
                            {
                                break;
                            }
                            else
                            {
                                endPageNo++;
                            }
                        }

                        // string[] RgxGroupData = System.Text.RegularExpressions.Regex.Split(pageSource, "<li class=\"member\" id=\"");
                        string[] RgxGroupData = System.Text.RegularExpressions.Regex.Split(pageSource, "<span class=\"new-miniprofile-container");
                        int aa = RgxGroupData.Count();
                        if (aa < 2)
                        {
                            RgxGroupData = System.Text.RegularExpressions.Regex.Split(pageSource, "<span class=\"miniprofile-container");
                            aa = RgxGroupData.Count();
                        }

                        if (aa > 1)

                            foreach (string item in RgxGroupData)
                            {


                                try
                                {
                                    
                                    if (!(item.Contains("<!DOCTYPE html>")))
                                    {
                                        if (item.Contains("/profile/view?id="))
                                        {
                                            string profileURL = item.Substring(item.IndexOf("/profile/view?id="), item.IndexOf("\"", item.IndexOf("/profile/view?id=")) - item.IndexOf("/profile/view?id=")).Replace("\"", string.Empty).Trim();
                                            string[] checkdup = Regex.Split(profileURL, "&goback");
                                            string itemForScrap = "http://www.linkedin.com" + checkdup[0] + "&goback";

                                            count++;
                                            Log("[ " + DateTime.Now + " ] => [ Added URL for Scrape : " + itemForScrap + " ]");
                                            lstFriendProfileURLs.Add(itemForScrap);
                                            if (!obj_ClsScrapGroupMember.CrawlingLinkedInPage(itemForScrap, ref httpHelper))
                                            {
                                                obj_ClsScrapGroupMember.CrawlingPageDataSource(itemForScrap, ref httpHelper);

                                            }
                                        }
                                    }
                                }
                                catch { }
                            }

                        if (!pageSource.Contains("next&nbsp;&#187;") && !pageSource.Contains("siguiente&nbsp;&#187;"))
                        {
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                    }

                }
            }
            catch (Exception ex)
            {
            }

            lstFriendProfileURLs = lstFriendProfileURLs.Distinct().ToList();

            return lstFriendProfileURLs;
        } 
        #endregion

        #region StartScrapGrpMemberMultiThread
        public void StartScrapGrpMemberMultiThread(object parameter)
        {
            try
            {
                Array paramsArray = new object[1];
                paramsArray = (Array)parameter;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error >>> " + ex.StackTrace);
            }
        }
        #endregion

        #region logger grpmem

        private void grpmem(string log)
        {
            try
            {
                EventsArgs loggerEventsArgs = new EventsArgs(log);
                loggergrpupdate.LogText(loggerEventsArgs);
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region logger Endorse

        private void Log_Endorse(string log)
        {
            try
            {
                EventsArgs loggerEventsArgs = new EventsArgs(log);
                loggerEndorseCampaign.LogText(loggerEventsArgs);
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        

        #region logger Loggergrppmem

        private void Loggergrppmem(string log)
        {
            try
            {
                EventsArgs loggerEventsArgs = new EventsArgs(log);
                loggerGroupMem.LogText(loggerEventsArgs);
            }
            catch (Exception ex)
            {

            }

        }
        #endregion

        #region logger LoggerRemPendingGrp

        private void LoggerRemPendingGrp(string log)
        {
            try
            {
                EventsArgs loggerEventsArgs = new EventsArgs(log);
                loggerRemPendingGroup.LogText(loggerEventsArgs);
            }
            catch (Exception ex)
            {

            }

        }
        #endregion

        #region logger LoggerRemPendingGrp

        private void LoggerInviteGrp(string log)
        {
            try
            {
                EventsArgs loggerEventsArgs = new EventsArgs(log);
                loggerInviteGroups.LogText(loggerEventsArgs);
            }
            catch (Exception ex)
            {

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
