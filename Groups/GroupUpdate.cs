using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Text.RegularExpressions;
using BaseLib;

namespace Groups
{
    public class GroupUpdate
    {
        #region global declaration
        public string accountUser = string.Empty;
        public string accountPass = string.Empty;
        public string proxyAddress = string.Empty;
        public string proxyPort = string.Empty;
        public string proxyUserName = string.Empty;
        public string proxyPassword = string.Empty;

        public static Queue<string> Que_GrpPostTitle_Post = new Queue<string>();
        public static Queue<string> Que_GrpMoreDtl_Post = new Queue<string>();
        public static Queue<string> Que_GrpAttachLink_Post = new Queue<string>();
        public static Queue<string> Que_GrpKey_Post = new Queue<string>();
        public static readonly object Locked_GrpPostTitle_Post = new object();
        public static readonly object Locked_GrpMoreDtl_Post = new object();
        public static readonly object Locked_Que_GrpAttachLink_Post = new object();
        public static readonly object Locked_GrpKey_Post = new object();
        public static string AttachLink_Post = string.Empty;
        public static int BoxGroupCount = 5;

        public Dictionary<string, string> OpenGroupDtl = new Dictionary<string, string>();
        public static Dictionary<string, string> GroupName = new Dictionary<string, string>();
        public static List<string> LikeUrl = new List<string>();
        #endregion

        #region GroupUpdate
        public GroupUpdate()
        {

        }
        #endregion

        #region GroupUpdate
        public GroupUpdate(string UserName, string Password, string ProxyAddress, string ProxyPort, string ProxyUserName, string ProxyPassword)
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
        public static Events loggerLiker = new Events();

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


        private void Log1(string log1)
        {
            try
            {
                EventsArgs loggerEventsArgs = new EventsArgs(log1);
                loggerLiker.LogText(loggerEventsArgs);
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region PostGroupMsg
        public void PostGroupMsg(ref GlobusHttpHelper HttpHelper, List<string> selectedItems, object parameter, int mindelay, int maxdelay)
        {
            string postdata = string.Empty;
            string postUrl = string.Empty;
            string ResLogin = string.Empty;
            string csrfToken = string.Empty;
            string sourceAlias = string.Empty;
            string referal = string.Empty;

            string ReturnString = string.Empty;
            string PostGrpDiscussion = string.Empty;
            string PostGrpMoreDetails = string.Empty;
            string PostGrpAttachLink = string.Empty;
            string PostGrpKey = string.Empty;

            try
            {
                string MessageText = string.Empty;
                string PostedMessage = string.Empty;
                string pageSource = string.Empty;

                Array paramsArray = (Array)parameter;

                string user = string.Empty;
                try
                {
                    user = paramsArray.GetValue(1).ToString();
                }
                catch { }

                pageSource = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/home?trk=hb_tab_home_top"));

                if (pageSource.Contains("csrfToken"))
                {
                    string pattern = @"\";
                    csrfToken = pageSource.Substring(pageSource.IndexOf("csrfToken"), 50);
                    string[] Arr = csrfToken.Split('&');
                    csrfToken = Arr[0];
                    csrfToken = csrfToken.Replace("csrfToken", "").Replace("\"", string.Empty).Replace("value", string.Empty).Replace("cs", string.Empty).Replace("id", string.Empty).Replace("=", string.Empty).Replace(">\n<", "").Replace("script src", "").Replace("meta http-", string.Empty).Trim();
                    csrfToken = csrfToken.Replace(pattern, string.Empty.Trim());
                }

                if (pageSource.Contains("sourceAlias"))
                {
                    string pattern1 = @"\";
                    sourceAlias = pageSource.Substring(pageSource.IndexOf("sourceAlias"), 100);
                    string[] Arr = sourceAlias.Split('"');
                    sourceAlias = Arr[2];
                    sourceAlias = sourceAlias.Replace(pattern1, string.Empty.Trim());
                }

                try
                {

                    foreach (var Itegid in selectedItems)
                    {
                        string[] grpNameWithDetails = Itegid.Split(':');

                        try
                        {
                            lock (Locked_GrpKey_Post)
                            {
                                if (Que_GrpKey_Post.Count > 0)
                                {
                                    try
                                    {
                                        PostGrpKey = Que_GrpKey_Post.Dequeue();
                                    }
                                    catch { }
                                }
                            }

                            lock (Locked_GrpPostTitle_Post)
                            {
                                if (Que_GrpPostTitle_Post.Count > 0)
                                {
                                    try
                                    {
                                        PostGrpDiscussion = Que_GrpPostTitle_Post.Dequeue();
                                    }
                                    catch { }
                                }
                                else
                                {
                                    Log("[ " + DateTime.Now + " ] => [ NO  Header Message available for " + grpNameWithDetails[2] + " ]");
                                    return;
                                }

                            }

                            lock (Locked_GrpMoreDtl_Post)
                            {
                                if (Que_GrpMoreDtl_Post.Count > 0)
                                {
                                    try
                                    {
                                        PostGrpMoreDetails = Que_GrpMoreDtl_Post.Dequeue();
                                    }
                                    catch { }
                                }
                                else
                                {
                                    PostGrpMoreDetails = string.Empty;
                                }

                            }

                            lock (Locked_Que_GrpAttachLink_Post)
                            {
                                if (Que_GrpAttachLink_Post.Count > 0)
                                {
                                    try
                                    {
                                        PostGrpAttachLink = Que_GrpAttachLink_Post.Dequeue();
                                    }
                                    catch { }
                                }

                            }

                            string[] grpDisplay = Itegid.Split(':');
                            string GrpName = Itegid.ToString().Replace(",", ":").Replace("[", string.Empty).Replace("]", string.Empty).Trim();
                            string[] PostGid = GrpName.Split(':');
                            string Gid = string.Empty;

                            try
                            {
                                if (NumberHelper.ValidateNumber(PostGid[1].Trim()))
                                {
                                    Gid = PostGid[1].Trim();
                                }
                                else if (NumberHelper.ValidateNumber(PostGid[2].Trim()))
                                {
                                    Gid = PostGid[2].Trim();
                                }
                                else if (NumberHelper.ValidateNumber(PostGid[3].Trim()))
                                {
                                    Gid = PostGid[3].Trim();
                                }
                                else if (NumberHelper.ValidateNumber(PostGid[4].Trim()))
                                {
                                    Gid = PostGid[4].Trim();
                                }
                                else if (NumberHelper.ValidateNumber(PostGid[5].Trim()))
                                {
                                    Gid = PostGid[5].Trim();
                                }
                                else if (NumberHelper.ValidateNumber(PostGid[6].Trim()))
                                {
                                    Gid = PostGid[6].Trim();
                                }
                                else if (NumberHelper.ValidateNumber(PostGid[7].Trim()))
                                {
                                    Gid = PostGid[7].Trim();
                                }
                                else if (NumberHelper.ValidateNumber(PostGid[8].Trim()))
                                {
                                    Gid = PostGid[8].Trim();
                                }
                                else if (NumberHelper.ValidateNumber(PostGid[9].Trim()))
                                {
                                    Gid = PostGid[9].Trim();
                                }
                                else if (NumberHelper.ValidateNumber(PostGid[10].Trim()))
                                {
                                    Gid = PostGid[10].Trim();
                                }
                                else if (NumberHelper.ValidateNumber(PostGid[11].Trim()))
                                {
                                    Gid = PostGid[11].Trim();
                                }
                                else if (NumberHelper.ValidateNumber(PostGid[12].Trim()))
                                {
                                    Gid = PostGid[12].Trim();
                                }
                            }
                            catch { }

                            //string ReqUrl = PostGrpAttachLink;
                            string ReqUrl = PostGrpMoreDetails;
                            ReqUrl = ReqUrl.Replace(":", "%3A").Replace("//", "%2F%2F");
                            referal = "http://www.linkedin.com/groups/" + grpDisplay[2].Replace(" ", "-") + "-" + Gid + "?goback=%2Egmr_" + Gid;
                            string GetStatus = HttpHelper.getHtmlfromUrl(new Uri("http://www.linkedin.com/share?getPreview=&url=" + ReqUrl), referal);

                            string ImgCount = string.Empty;
                            try
                            {
                                int StartinImgCnt = GetStatus.IndexOf("current");
                                string startImgCnt = GetStatus.Substring(StartinImgCnt);
                                int EndIndexImgCnt = startImgCnt.IndexOf("</span>");
                                string EndImgCnt = startImgCnt.Substring(0, EndIndexImgCnt).Replace("value\":", "").Replace("\"", "");
                                ImgCount = EndImgCnt.Replace("current", string.Empty).Replace(">", string.Empty);
                            }
                            catch
                            {
                                ImgCount = "0";
                            }

                            string LogoUrl = string.Empty;
                            try
                            {
                                int StartinImgUrl = GetStatus.IndexOf("url");
                                string startImgUrl = GetStatus.Substring(StartinImgUrl);
                                int EndIndexImgUrl = startImgUrl.IndexOf("border=");
                                string EndImgUrl = startImgUrl.Substring(0, EndIndexImgUrl).Replace("value\":", "").Replace("\"", "");
                                LogoUrl = EndImgUrl.Replace("url=", string.Empty).Trim();
                            }
                            catch
                            {
                                LogoUrl = "false";
                            }

                            string EntityId = string.Empty;
                            try
                            {
                                int StartinEntityId = GetStatus.IndexOf("data-entity-id");
                                string startEntityId = GetStatus.Substring(StartinEntityId);
                                int EndIndexEntityId = startEntityId.IndexOf("data-entity-url");
                                string EndEntityId = startEntityId.Substring(0, EndIndexEntityId).Replace("value\":", "").Replace("\"", "");
                                EntityId = EndEntityId.Replace("\"", string.Empty).Replace("data-entity-id", string.Empty).Replace("=", string.Empty).Trim();
                            }
                            catch { }

                            string contentTitle = string.Empty;
                            try
                            {
                                int StartinContent = GetStatus.IndexOf("share-view-title");
                                string startContent = GetStatus.Substring(StartinContent);
                                int EndIndexContent = startContent.IndexOf("</h4>");
                                string EndContent = startContent.Substring(0, EndIndexContent).Replace("value\":", "").Replace("\"", "");
                                contentTitle = EndContent.Replace("\"", string.Empty).Replace("\n", string.Empty).Replace("share-view-title", string.Empty).Replace("id=", string.Empty).Replace(">", string.Empty).Replace("&", "and").Replace("amp;", string.Empty).Trim();

                                if (contentTitle.Contains("#"))
                                {
                                    contentTitle = contentTitle.Replace("and", "&");
                                    contentTitle = Uri.EscapeDataString(contentTitle);
                                }

                            }
                            catch { }

                            string contentSummary = string.Empty;
                            try
                            {
                                int StartinConSumm = GetStatus.IndexOf("share-view-summary\">");
                                string startConSumm = GetStatus.Substring(StartinConSumm);
                                int EndIndexConSumm = startConSumm.IndexOf("</span>");
                                string EndConSumm = startConSumm.Substring(0, EndIndexConSumm).Replace("value\":", "").Replace("\"", "");
                                contentSummary = EndConSumm.Replace("\"", string.Empty).Replace("\n", string.Empty).Replace("share-view-summary", string.Empty).Replace("id=", string.Empty).Replace(">", string.Empty).Replace("</span<a href=#", string.Empty).Trim();
                                contentSummary = contentSummary.Replace(",", "%2C").Replace(" ", "%20");

                                if (contentSummary.Contains("#"))
                                {
                                    contentSummary = contentSummary.Replace("and", "&");
                                    contentSummary = Uri.EscapeDataString(contentSummary);
                                }
                            }
                            catch { }

                            string PostGroupstatus = string.Empty;
                            string ResponseStatusMsg = string.Empty;
                            try
                            {
                                //PostGroupstatus = "csrfToken=" + csrfToken + "&postTitle=" + PostGrpDiscussion + "&postText=" + PostGrpMoreDetails + "&pollChoice1-ANetPostForm=&pollChoice2-ANetPostForm=&pollChoice3-ANetPostForm=&pollChoice4-ANetPostForm=&pollChoice5-ANetPostForm=&pollEndDate-ANetPostForm=0&contentImageCount=0&contentImageIndex=-1&contentImage=&contentEntityID=&contentUrl=&contentTitle=&contentSummary=&contentImageIncluded=true&%23=&gid=" + Gid.Trim() + "&postItem=&ajax=true&tetherAccountID=&facebookTetherID=";
                                PostGroupstatus = "csrfToken=" + csrfToken + "&postTitle=" + PostGrpDiscussion + "&postText=" + PostGrpMoreDetails + "&pollChoice1-ANetPostForm=&pollChoice2-ANetPostForm=&pollChoice3-ANetPostForm=&pollChoice4-ANetPostForm=&pollChoice5-ANetPostForm=&pollEndDate-ANetPostForm=0&contentImageCount=" + ImgCount + "&contentImageIndex=-1&contentImage=" + LogoUrl + "&contentEntityID=" + EntityId + "&contentUrl=" + ReqUrl + "&contentTitle=" + contentTitle + "&contentSummary=" + contentSummary + "&contentImageIncluded=true&%23=&gid=" + Gid + "&postItem=&ajax=true&tetherAccountID=&facebookTetherID=";
                                ResponseStatusMsg = HttpHelper.postFormData(new Uri("http://www.linkedin.com/groups"), PostGroupstatus);
                            }
                            catch { }

                            string CSVHeader = "UserName" + "," + "HeaderPost" + "," + "Details Post" + "," + "ToGroup";

                            if (ResponseStatusMsg.Contains("SUCCESS") || ResponseStatusMsg.Contains("Accept  the description According to you"))
                            {
                                Log("[ " + DateTime.Now + " ] => [ Message Header Posted : " + PostGrpDiscussion + " Successfully on Group : " + grpDisplay[2] + " ]");
                                Log("[ " + DateTime.Now + " ] => [ Message More Details Posted : " + PostGrpMoreDetails + " Successfully on Group : " + grpDisplay[2] + " ]");

                                string CSV_Content = user + "," + PostGrpDiscussion.Replace(",", ";") + "," + PostGrpMoreDetails.Replace(",", ";") + "," + grpDisplay[2].Replace(",", string.Empty);
                                CSVUtilities.ExportDataCSVFile(CSVHeader, CSV_Content, Globals.path_GroupUpdates);

                            }
                            else if (ResponseStatusMsg.Contains("Your request to join is still pending"))
                            {
                                Log("[ " + DateTime.Now + " ] => [ Your membership is pending approval on a Group:" + grpDisplay[2] + " ]");
                                Log("[ " + DateTime.Now + " ] => [ Message Header: " + PostGrpDiscussion + " Not Posted on Group:" + grpDisplay[2] + " Because Your membership is pending for approval. ]");
                                Log("[ " + DateTime.Now + " ] => [ Message More Details: " + PostGrpMoreDetails + " Not Posted on Group:" + grpDisplay[2] + " Because Your membership is pending for approval. ]");

                                GlobusFileHelper.AppendStringToTextfileNewLine("Your membership is pending approval on a Group:" + grpDisplay[2], Globals.path_GroupUpdate);
                                GlobusFileHelper.AppendStringToTextfileNewLine("Message Header: " + PostGrpDiscussion + " Not Posted on Group:" + grpDisplay[2] + " Because Your membership is pending for approval. ", Globals.path_GroupUpdate);
                                GlobusFileHelper.AppendStringToTextfileNewLine("Message More Details: " + PostGrpMoreDetails + " Not Posted on Group:" + grpDisplay[2] + " Because Your membership is pending for approval. ", Globals.path_GroupUpdate);
                            }
                            else if (ResponseStatusMsg.Contains("Your post has been submitted for review"))
                            {
                                Log("[ " + DateTime.Now + " ] => [ Message Header Posted : " + PostGrpDiscussion + " Successfully on Group : " + grpDisplay[2] + " ]");
                                Log("[ " + DateTime.Now + " ] => [ Message More Details Posted : " + PostGrpMoreDetails + " Successfully on Group : " + grpDisplay[2] + " ]");
                                Log("[ " + DateTime.Now + " ] => [ Your post has been submitted for review ]");
                                string CSV_Content = user + "," + PostGrpDiscussion.Replace(",", ";") + "," + PostGrpMoreDetails.Replace(",", ";") + "," + grpDisplay[2];

                            }
                            else if (ResponseStatusMsg.Contains("Error"))
                            {
                                //Log("[ " + DateTime.Now + " ] => [ Error in Post ]");
                                GlobusFileHelper.AppendStringToTextfileNewLine("Error in Post", Globals.path_GroupUpdate);

                            }
                            else
                            {
                                Log("[ " + DateTime.Now + " ] => [ Message Not Posted ]");
                                GlobusFileHelper.AppendStringToTextfileNewLine("Message Not Posted", Globals.path_GroupUpdate);
                            }

                            int delay = RandomNumberGenerator.GenerateRandom(mindelay, maxdelay);
                            Log("[ " + DateTime.Now + " ] => [ Delay for : " + delay + " Seconds ]");
                            Thread.Sleep(delay * 1000);

                        }
                        catch (Exception ex)
                        {
                            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update --> cmbGroupUser_SelectedIndexChanged() ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + "    Stack Trace >>> " + ex.StackTrace, Globals.Path_LinkedinGetGroupMemberErrorLogs);
                            //Log("[ " + DateTime.Now + " ] => [ Error:" + ex.Message + "StackTrace --> >>>" + ex.StackTrace + " ]");
                        }
                    }

                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update --> cmbGroupUser_SelectedIndexChanged() ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update --> cmbGroupUser_SelectedIndexChanged() ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinGetGroupMemberErrorLogs);
                    // Log("[ " + DateTime.Now + " ] => [ Error:" + ex.Message + "StackTrace --> >>>" + ex.StackTrace + " ]");
                }

            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update --> cmbGroupUser_SelectedIndexChanged() ---2--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update --> cmbGroupUser_SelectedIndexChanged() ---2--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinGetGroupMemberErrorLogs);
            }
        }
        #endregion

        #region PostGroupMsg"Same Message For all group"
        public void PostGroupSameMessageForAllGroup(ref GlobusHttpHelper HttpHelper, List<string> selectedItems, object parameter, int mindelay, int maxdelay)
        {
            string postdata = string.Empty;
            string postUrl = string.Empty;
            string ResLogin = string.Empty;
            string csrfToken = string.Empty;
            string sourceAlias = string.Empty;
            string referal = string.Empty;

            string ReturnString = string.Empty;
            string PostGrpDiscussion = string.Empty;
            string PostGrpMoreDetails = string.Empty;
            string PostGrpAttachLink = string.Empty;
            string PostGrpKey = string.Empty;

            try
            {
                string MessageText = string.Empty;
                string PostedMessage = string.Empty;
                string pageSource = string.Empty;

                Array paramsArray = (Array)parameter;

                string user = string.Empty;
                try
                {
                    user = paramsArray.GetValue(1).ToString();
                }
                catch { }

                pageSource = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/home?trk=hb_tab_home_top"));

                if (pageSource.Contains("csrfToken"))
                {
                    string pattern = @"\";
                    csrfToken = pageSource.Substring(pageSource.IndexOf("csrfToken"), 50);
                    string[] Arr = csrfToken.Split('&');
                    csrfToken = Arr[0];
                    csrfToken = csrfToken.Replace("csrfToken", "").Replace("\"", string.Empty).Replace("value", string.Empty).Replace("cs", string.Empty).Replace("id", string.Empty).Replace("=", string.Empty);
                    csrfToken = csrfToken.Replace(pattern, string.Empty.Trim());
                }

                if (pageSource.Contains("sourceAlias"))
                {
                    string pattern1 = @"\";
                    sourceAlias = pageSource.Substring(pageSource.IndexOf("sourceAlias"), 100);
                    string[] Arr = sourceAlias.Split('"');
                    sourceAlias = Arr[2];
                    sourceAlias = sourceAlias.Replace(pattern1, string.Empty.Trim());
                }

                try
                {

                    foreach (var Itegid in selectedItems)
                    {
                        string[] grpNameWithDetails = Itegid.Split(':');

                        try
                        {
                            lock (Locked_GrpKey_Post)
                            {
                                if (Que_GrpKey_Post.Count > 0)
                                {
                                    try
                                    {
                                        PostGrpKey = Que_GrpKey_Post.Dequeue();

                                    }
                                    catch { }
                                }
                            }

                            lock (Locked_GrpPostTitle_Post)
                            {
                                if (Que_GrpPostTitle_Post.Count > 0)
                                {
                                    try
                                    {
                                        PostGrpDiscussion = Que_GrpPostTitle_Post.Dequeue();
                                        //Que_GrpPostTitle_Post.Clear();

                                    }
                                    catch { }
                                }


                            }

                            lock (Locked_GrpMoreDtl_Post)
                            {
                                if (Que_GrpMoreDtl_Post.Count > 0)
                                {
                                    try
                                    {
                                        PostGrpMoreDetails = Que_GrpMoreDtl_Post.Dequeue();
                                        //Que_GrpMoreDtl_Post.Clear();
                                    }
                                    catch { }
                                }


                            }

                            lock (Locked_Que_GrpAttachLink_Post)
                            {
                                if (Que_GrpAttachLink_Post.Count > 0)
                                {
                                    try
                                    {
                                        PostGrpAttachLink = Que_GrpAttachLink_Post.Dequeue();
                                    }
                                    catch { }
                                }

                            }

                            string[] grpDisplay = Itegid.Split(':');
                            string GrpName = Itegid.ToString().Replace(",", ":").Replace("[", string.Empty).Replace("]", string.Empty).Trim();
                            string[] PostGid = GrpName.Split(':');
                            string Gid = string.Empty;

                            try
                            {
                                if (NumberHelper.ValidateNumber(PostGid[1].Trim()))
                                {
                                    Gid = PostGid[1].Trim();
                                }
                                else if (NumberHelper.ValidateNumber(PostGid[2].Trim()))
                                {
                                    Gid = PostGid[2].Trim();
                                }
                                else if (NumberHelper.ValidateNumber(PostGid[3].Trim()))
                                {
                                    Gid = PostGid[3].Trim();
                                }
                                else if (NumberHelper.ValidateNumber(PostGid[4].Trim()))
                                {
                                    Gid = PostGid[4].Trim();
                                }
                                else if (NumberHelper.ValidateNumber(PostGid[5].Trim()))
                                {
                                    Gid = PostGid[5].Trim();
                                }
                                else if (NumberHelper.ValidateNumber(PostGid[6].Trim()))
                                {
                                    Gid = PostGid[6].Trim();
                                }
                                else if (NumberHelper.ValidateNumber(PostGid[7].Trim()))
                                {
                                    Gid = PostGid[7].Trim();
                                }
                                else if (NumberHelper.ValidateNumber(PostGid[8].Trim()))
                                {
                                    Gid = PostGid[8].Trim();
                                }
                                else if (NumberHelper.ValidateNumber(PostGid[9].Trim()))
                                {
                                    Gid = PostGid[9].Trim();
                                }
                                else if (NumberHelper.ValidateNumber(PostGid[10].Trim()))
                                {
                                    Gid = PostGid[10].Trim();
                                }
                                else if (NumberHelper.ValidateNumber(PostGid[11].Trim()))
                                {
                                    Gid = PostGid[11].Trim();
                                }
                                else if (NumberHelper.ValidateNumber(PostGid[12].Trim()))
                                {
                                    Gid = PostGid[12].Trim();
                                }
                            }
                            catch { }

                            //string ReqUrl = PostGrpAttachLink;
                            string ReqUrl = PostGrpMoreDetails;
                            ReqUrl = ReqUrl.Replace(":", "%3A").Replace("//", "%2F%2F");
                            referal = "http://www.linkedin.com/groups/" + grpDisplay[2].Replace(" ", "-") + "-" + Gid + "?goback=%2Egmr_" + Gid;
                            string GetStatus = HttpHelper.getHtmlfromUrl(new Uri("http://www.linkedin.com/share?getPreview=&url=" + ReqUrl), referal);

                            string ImgCount = string.Empty;
                            try
                            {
                                int StartinImgCnt = GetStatus.IndexOf("current");
                                string startImgCnt = GetStatus.Substring(StartinImgCnt);
                                int EndIndexImgCnt = startImgCnt.IndexOf("</span>");
                                string EndImgCnt = startImgCnt.Substring(0, EndIndexImgCnt).Replace("value\":", "").Replace("\"", "");
                                ImgCount = EndImgCnt.Replace("current", string.Empty).Replace(">", string.Empty);
                            }
                            catch
                            {
                                ImgCount = "0";
                            }

                            string LogoUrl = string.Empty;
                            try
                            {
                                int StartinImgUrl = GetStatus.IndexOf("url");
                                string startImgUrl = GetStatus.Substring(StartinImgUrl);
                                int EndIndexImgUrl = startImgUrl.IndexOf("border=");
                                string EndImgUrl = startImgUrl.Substring(0, EndIndexImgUrl).Replace("value\":", "").Replace("\"", "");
                                LogoUrl = EndImgUrl.Replace("url=", string.Empty).Trim();
                            }
                            catch
                            {
                                LogoUrl = "false";
                            }

                            string EntityId = string.Empty;
                            try
                            {
                                int StartinEntityId = GetStatus.IndexOf("data-entity-id");
                                string startEntityId = GetStatus.Substring(StartinEntityId);
                                int EndIndexEntityId = startEntityId.IndexOf("data-entity-url");
                                string EndEntityId = startEntityId.Substring(0, EndIndexEntityId).Replace("value\":", "").Replace("\"", "");
                                EntityId = EndEntityId.Replace("\"", string.Empty).Replace("data-entity-id", string.Empty).Replace("=", string.Empty).Trim();
                            }
                            catch { }

                            string contentTitle = string.Empty;
                            try
                            {
                                int StartinContent = GetStatus.IndexOf("share-view-title");
                                string startContent = GetStatus.Substring(StartinContent);
                                int EndIndexContent = startContent.IndexOf("</h4>");
                                string EndContent = startContent.Substring(0, EndIndexContent).Replace("value\":", "").Replace("\"", "");
                                contentTitle = EndContent.Replace("\"", string.Empty).Replace("\n", string.Empty).Replace("share-view-title", string.Empty).Replace("id=", string.Empty).Replace(">", string.Empty).Replace("&", "and").Replace("amp;", string.Empty).Trim();

                                if (contentTitle.Contains("#"))
                                {
                                    contentTitle = contentTitle.Replace("and", "&");
                                    contentTitle = Uri.EscapeDataString(contentTitle);
                                }

                            }
                            catch { }

                            string contentSummary = string.Empty;
                            try
                            {
                                int StartinConSumm = GetStatus.IndexOf("share-view-summary\">");
                                string startConSumm = GetStatus.Substring(StartinConSumm);
                                int EndIndexConSumm = startConSumm.IndexOf("</span>");
                                string EndConSumm = startConSumm.Substring(0, EndIndexConSumm).Replace("value\":", "").Replace("\"", "");
                                contentSummary = EndConSumm.Replace("\"", string.Empty).Replace("\n", string.Empty).Replace("share-view-summary", string.Empty).Replace("id=", string.Empty).Replace(">", string.Empty).Replace("</span<a href=#", string.Empty).Trim();
                                contentSummary = contentSummary.Replace(",", "%2C").Replace(" ", "%20");

                                if (contentSummary.Contains("#"))
                                {
                                    contentSummary = contentSummary.Replace("and", "&");
                                    contentSummary = Uri.EscapeDataString(contentSummary);
                                }
                            }
                            catch { }

                            string PostGroupstatus = string.Empty;
                            string ResponseStatusMsg = string.Empty;
                            csrfToken = csrfToken.Replace("<meta http-", "").Replace(">", "").Trim();
                            try
                            {
                                //PostGroupstatus = "csrfToken=" + csrfToken + "&postTitle=" + PostGrpDiscussion + "&postText=" + PostGrpMoreDetails + "&pollChoice1-ANetPostForm=&pollChoice2-ANetPostForm=&pollChoice3-ANetPostForm=&pollChoice4-ANetPostForm=&pollChoice5-ANetPostForm=&pollEndDate-ANetPostForm=0&contentImageCount=0&contentImageIndex=-1&contentImage=&contentEntityID=&contentUrl=&contentTitle=&contentSummary=&contentImageIncluded=true&%23=&gid=" + Gid.Trim() + "&postItem=&ajax=true&tetherAccountID=&facebookTetherID=";
                                PostGroupstatus = "csrfToken=" + csrfToken + "&postTitle=" + PostGrpDiscussion + "&postText=" + PostGrpMoreDetails + "&pollChoice1-ANetPostForm=&pollChoice2-ANetPostForm=&pollChoice3-ANetPostForm=&pollChoice4-ANetPostForm=&pollChoice5-ANetPostForm=&pollEndDate-ANetPostForm=0&contentImageCount=" + ImgCount + "&contentImageIndex=-1&contentImage=" + LogoUrl + "&contentEntityID=" + EntityId + "&contentUrl=" + ReqUrl + "&contentTitle=" + contentTitle + "&contentSummary=" + contentSummary + "&contentImageIncluded=true&%23=&gid=" + Gid + "&postItem=&ajax=true&tetherAccountID=&facebookTetherID=";
                                ResponseStatusMsg = HttpHelper.postFormData(new Uri("http://www.linkedin.com/groups"), PostGroupstatus);
                            }
                            catch { }

                            string CSVHeader = "UserName" + "," + "HeaderPost" + "," + "Details Post" + "," + "ToGroup";

                            if (ResponseStatusMsg.Contains("SUCCESS") || ResponseStatusMsg.Contains("Accept  the description According to you"))
                            {
                                Log("[ " + DateTime.Now + " ] => [ Message Header Posted : " + PostGrpDiscussion + " Successfully on Group : " + grpDisplay[2] + " ]");
                                Log("[ " + DateTime.Now + " ] => [ Message More Details Posted : " + PostGrpMoreDetails + " Successfully on Group : " + grpDisplay[2] + " ]");

                                string CSV_Content = user + "," + PostGrpDiscussion.Replace(",", ";") + "," + PostGrpMoreDetails.Replace(",", ";") + "," + grpDisplay[2].Replace(",", string.Empty);
                                CSVUtilities.ExportDataCSVFile(CSVHeader, CSV_Content, Globals.path_GroupUpdates);

                            }
                            else if (ResponseStatusMsg.Contains("Your request to join is still pending"))
                            {
                                Log("[ " + DateTime.Now + " ] => [ Your membership is pending approval on a Group:" + grpDisplay[2] + " ]");
                                Log("[ " + DateTime.Now + " ] => [ Message Header: " + PostGrpDiscussion + " Not Posted on Group:" + grpDisplay[2] + " Because Your membership is pending for approval. ]");
                                Log("[ " + DateTime.Now + " ] => [ Message More Details: " + PostGrpMoreDetails + " Not Posted on Group:" + grpDisplay[2] + " Because Your membership is pending for approval. ]");

                                GlobusFileHelper.AppendStringToTextfileNewLine("Your membership is pending approval on a Group:" + grpDisplay[2], Globals.path_GroupUpdate);
                                GlobusFileHelper.AppendStringToTextfileNewLine("Message Header: " + PostGrpDiscussion + " Not Posted on Group:" + grpDisplay[2] + " Because Your membership is pending for approval. ", Globals.path_GroupUpdate);
                                GlobusFileHelper.AppendStringToTextfileNewLine("Message More Details: " + PostGrpMoreDetails + " Not Posted on Group:" + grpDisplay[2] + " Because Your membership is pending for approval. ", Globals.path_GroupUpdate);
                            }
                            else if (ResponseStatusMsg.Contains("Your post has been submitted for review"))
                            {
                                Log("[ " + DateTime.Now + " ] => [ Message Header Posted : " + PostGrpDiscussion + " Successfully on Group : " + grpDisplay[2] + " ]");
                                Log("[ " + DateTime.Now + " ] => [ Message More Details Posted : " + PostGrpMoreDetails + " Successfully on Group : " + grpDisplay[2] + " ]");
                                Log("[ " + DateTime.Now + " ] => [ Your post has been submitted for review ]");
                                string CSV_Content = user + "," + PostGrpDiscussion.Replace(",", ";") + "," + PostGrpMoreDetails.Replace(",", ";") + "," + grpDisplay[2];

                            }
                            else if (ResponseStatusMsg.Contains("Error"))
                            {
                                //Log("[ " + DateTime.Now + " ] => [ Error in Post ]");
                                GlobusFileHelper.AppendStringToTextfileNewLine("Error in Post", Globals.path_GroupUpdate);

                            }
                            else
                            {
                                Log("[ " + DateTime.Now + " ] => [ Message Not Posted ]");
                                GlobusFileHelper.AppendStringToTextfileNewLine("Message Not Posted", Globals.path_GroupUpdate);
                            }

                            int delay = RandomNumberGenerator.GenerateRandom(mindelay, maxdelay);
                            Log("[ " + DateTime.Now + " ] => [ Delay for : " + delay + " Seconds ]");
                            Thread.Sleep(delay * 1000);

                        }
                        catch (Exception ex)
                        {
                            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update --> cmbGroupUser_SelectedIndexChanged() ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + "    Stack Trace >>> " + ex.StackTrace, Globals.Path_LinkedinGetGroupMemberErrorLogs);
                            //Log("[ " + DateTime.Now + " ] => [ Error:" + ex.Message + "StackTrace --> >>>" + ex.StackTrace + " ]");
                        }
                    }

                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update --> cmbGroupUser_SelectedIndexChanged() ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update --> cmbGroupUser_SelectedIndexChanged() ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinGetGroupMemberErrorLogs);
                    // Log("[ " + DateTime.Now + " ] => [ Error:" + ex.Message + "StackTrace --> >>>" + ex.StackTrace + " ]");
                }

            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update --> cmbGroupUser_SelectedIndexChanged() ---2--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update --> cmbGroupUser_SelectedIndexChanged() ---2--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinGetGroupMemberErrorLogs);
            }
        }
        #endregion

        #region PostCreateGroupNames
        public Dictionary<string, string> PostCreateGroupNames(ref GlobusHttpHelper HttpHelper, string user)
        {
            try
            {

                string pageSource = HttpHelper.getHtmlfromUrl(new Uri("http://www.linkedin.com/grp/"));

                if (pageSource == "")
                {
                    Thread.Sleep(2000);
                    pageSource = HttpHelper.getHtmlfromUrl(new Uri("http://www.linkedin.com/myGroups?trk=nav_responsive_sub_nav_groups"));
                }

                //string[] RgxGroupData = Regex.Split(pageSource, "media-content");
                string[] RgxGroupData = Regex.Split(pageSource, "group-activity"); //media-content


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
                                    //int startindex = GrpName.IndexOf("class=\"public\"");
                                    //string start = GrpName.Substring(startindex);
                                    //int endIndex = start.IndexOf("</a>");

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
                                        //;gid=846797&
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
                                    GroupName.Add(endName, endKey);
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
                                    //    int startindex = GrpName.IndexOf("class=\"private\"");
                                    //    string start = GrpName.Substring(startindex);
                                    //    int endIndex = start.IndexOf("</a>");

                                    int startindex = GrpName.IndexOf("class=\"public\"");
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
                                    endKey = start1.Substring(0, endIndex1).Replace("gid=", string.Empty).Replace("/", string.Empty).Replace("<a href=", string.Empty).Replace("\"", string.Empty);

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
                                    GroupName.Add(endName, endKey);
                                }
                                catch { }
                            }

                        }
                        if (GrpName.Contains("<a href=\"/groups/") || (GrpName.Contains("<a href=\"/groups?")))
                        {
                            // if ((GrpName.Contains("private")))
                            {
                                try
                                {
                                    string GrpNametemp = Utils.getBetween(GrpName, "<h3 class=\"title\">", "</h3></a>");
                                    endName = GrpNametemp + ':' + user;
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
                                    endKey = start1.Substring(0, endIndex1).Replace("gid=", string.Empty).Replace("/", string.Empty).Replace("<a href=", string.Empty).Replace("\"", string.Empty);

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
                                    GroupName.Add(endName, endKey);
                                }
                                catch { }
                            }

                        }
                    }
                    catch { }
                }
                return GroupName;

            }
            catch (Exception ex)
            {
                return GroupName;
            }

        }
        #endregion

        #region PostAttachLinkGroupUpdate
        public void PostAttachLinkGroupUpdate(ref GlobusHttpHelper HttpHelper, List<string> selectedItems, object parameter, int mindelay, int maxdelay)
        {
            string postdata = string.Empty;
            string referal = string.Empty;
            string postUrl = string.Empty;
            string ResLogin = string.Empty;
            string csrfToken = string.Empty;
            string sourceAlias = string.Empty;
            string ReturnString = string.Empty;
            string PostGrpAttachLink = string.Empty;
            string PostGrpKey = string.Empty;
            string Poststatus = string.Empty;

            try
            {
                string MessageText = string.Empty;
                string PostedMessage = string.Empty;
                string pageSource = string.Empty;

                Array paramsArray = (Array)parameter;

                string user = string.Empty;
                try
                {
                    user = paramsArray.GetValue(1).ToString();
                }
                catch { }


                pageSource = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/home?trk=hb_tab_home_top"));

                if (pageSource.Contains("csrfToken"))
                {
                    string pattern = @"\";
                    csrfToken = pageSource.Substring(pageSource.IndexOf("csrfToken"), 50);
                    string[] Arr = csrfToken.Split('&');
                    csrfToken = Arr[0];
                    csrfToken = csrfToken.Replace("csrfToken", "").Replace("\"", string.Empty).Replace("value", string.Empty).Replace("cs", string.Empty).Replace("id", string.Empty).Replace("=", string.Empty);
                    csrfToken = csrfToken.Replace(pattern, string.Empty.Trim());
                }

                if (pageSource.Contains("sourceAlias"))
                {
                    string pattern1 = @"\";
                    sourceAlias = pageSource.Substring(pageSource.IndexOf("sourceAlias"), 100);
                    string[] Arr = sourceAlias.Split('"');
                    sourceAlias = Arr[2];
                    sourceAlias = sourceAlias.Replace(pattern1, string.Empty.Trim());
                }

                try
                {
                    int counter = 0;
                    foreach (var Itegid in selectedItems)
                    {
                        counter = counter + 1;
                        try
                        {

                            lock (Locked_Que_GrpAttachLink_Post)
                            {
                                if (Que_GrpAttachLink_Post.Count > 0)
                                {
                                    try
                                    {
                                        PostGrpAttachLink = Que_GrpAttachLink_Post.Dequeue();
                                    }
                                    catch { }
                                }
                                else
                                {
                                    PostGrpAttachLink = AttachLink_Post;
                                }

                            }

                            string[] grpDisplay = Itegid.Split(':');
                            string GrpName = Itegid.ToString().Replace(",", ":").Replace("[", string.Empty).Replace("]", string.Empty).Trim();
                            string[] PostGid = GrpName.Split(':');
                            string Gid = string.Empty;

                            try
                            {
                                if (NumberHelper.ValidateNumber(PostGid[1].Trim()))
                                {
                                    Gid = PostGid[1].Trim();
                                }
                                else if (NumberHelper.ValidateNumber(PostGid[2].Trim()))
                                {
                                    Gid = PostGid[2].Trim();
                                }
                                else if (NumberHelper.ValidateNumber(PostGid[3].Trim()))
                                {
                                    Gid = PostGid[3].Trim();
                                }
                                else if (NumberHelper.ValidateNumber(PostGid[4].Trim()))
                                {
                                    Gid = PostGid[4].Trim();
                                }
                                else if (NumberHelper.ValidateNumber(PostGid[5].Trim()))
                                {
                                    Gid = PostGid[5].Trim();
                                }
                                else if (NumberHelper.ValidateNumber(PostGid[6].Trim()))
                                {
                                    Gid = PostGid[6].Trim();
                                }
                                else if (NumberHelper.ValidateNumber(PostGid[7].Trim()))
                                {
                                    Gid = PostGid[7].Trim();
                                }
                                else if (NumberHelper.ValidateNumber(PostGid[8].Trim()))
                                {
                                    Gid = PostGid[8].Trim();
                                }
                                else if (NumberHelper.ValidateNumber(PostGid[9].Trim()))
                                {
                                    Gid = PostGid[9].Trim();
                                }
                                else if (NumberHelper.ValidateNumber(PostGid[10].Trim()))
                                {
                                    Gid = PostGid[10].Trim();
                                }
                                else if (NumberHelper.ValidateNumber(PostGid[11].Trim()))
                                {
                                    Gid = PostGid[11].Trim();
                                }
                                else if (NumberHelper.ValidateNumber(PostGid[12].Trim()))
                                {
                                    Gid = PostGid[12].Trim();
                                }
                            }
                            catch { }


                            string ReqUrl = PostGrpAttachLink;
                            ReqUrl = ReqUrl.Replace(":", "%3A").Replace("//", "%2F%2F");
                            referal = "http://www.linkedin.com/groups/" + grpDisplay[2].Replace(" ", "-") + "-" + Gid + "?goback=%2Egmr_" + Gid;
                            string GetStatus = HttpHelper.getHtmlfromUrl(new Uri("http://www.linkedin.com/share?getPreview=&url=" + PostGrpAttachLink), referal);

                            string ImgCount = string.Empty;
                            try
                            {
                                int StartinImgCnt = GetStatus.IndexOf("current");
                                string startImgCnt = GetStatus.Substring(StartinImgCnt);
                                int EndIndexImgCnt = startImgCnt.IndexOf("</span>");
                                string EndImgCnt = startImgCnt.Substring(0, EndIndexImgCnt).Replace("value\":", "").Replace("\"", "");
                                ImgCount = EndImgCnt.Replace("current", string.Empty).Replace(">", string.Empty);
                            }
                            catch
                            {
                                ImgCount = "0";
                            }

                            string LogoUrl = string.Empty;
                            try
                            {
                                int StartinImgUrl = GetStatus.IndexOf("url");
                                string startImgUrl = GetStatus.Substring(StartinImgUrl);
                                int EndIndexImgUrl = startImgUrl.IndexOf("border=");
                                string EndImgUrl = startImgUrl.Substring(0, EndIndexImgUrl).Replace("value\":", "").Replace("\"", "");
                                LogoUrl = EndImgUrl.Replace("url=", string.Empty).Trim();
                            }
                            catch
                            {
                                LogoUrl = "false";
                            }

                            string EntityId = string.Empty;
                            try
                            {
                                int StartinEntityId = GetStatus.IndexOf("data-entity-id");
                                string startEntityId = GetStatus.Substring(StartinEntityId);
                                int EndIndexEntityId = startEntityId.IndexOf("data-entity-url");
                                string EndEntityId = startEntityId.Substring(0, EndIndexEntityId).Replace("value\":", "").Replace("\"", "");
                                EntityId = EndEntityId.Replace("\"", string.Empty).Replace("data-entity-id", string.Empty).Replace("=", string.Empty).Trim();
                            }
                            catch { }

                            string contentTitle = string.Empty;
                            try
                            {
                                int StartinContent = GetStatus.IndexOf("share-view-title");
                                string startContent = GetStatus.Substring(StartinContent);
                                int EndIndexContent = startContent.IndexOf("</h4>");
                                string EndContent = startContent.Substring(0, EndIndexContent).Replace("value\":", "").Replace("\"", "");
                                contentTitle = EndContent.Replace("\"", string.Empty).Replace("\n", string.Empty).Replace("share-view-title", string.Empty).Replace("id=", string.Empty).Replace(">", string.Empty).Replace("&", "and").Replace("amp;", string.Empty).Trim();

                                if (contentTitle.Contains("#"))
                                {
                                    contentTitle = contentTitle.Replace("and", "&");
                                    contentTitle = Uri.EscapeDataString(contentTitle);
                                }

                            }
                            catch { }

                            string contentSummary = string.Empty;
                            try
                            {
                                int StartinConSumm = GetStatus.IndexOf("share-view-summary\">");
                                string startConSumm = GetStatus.Substring(StartinConSumm);
                                int EndIndexConSumm = startConSumm.IndexOf("</span>");
                                string EndConSumm = startConSumm.Substring(0, EndIndexConSumm).Replace("value\":", "").Replace("\"", "");
                                contentSummary = EndConSumm.Replace("\"", string.Empty).Replace("\n", string.Empty).Replace("share-view-summary", string.Empty).Replace("id=", string.Empty).Replace(">", string.Empty).Replace("</span<a href=#", string.Empty).Trim();
                                contentSummary = contentSummary.Replace(",", "%2C").Replace(" ", "%20");

                                if (contentSummary.Contains("#"))
                                {
                                    contentSummary = contentSummary.Replace("and", "&");
                                    contentSummary = Uri.EscapeDataString(contentSummary);
                                }
                            }
                            catch { }

                            string PostAttachLink = string.Empty;
                            string ResponseStatusMsg = string.Empty;

                            try
                            {
                                PostAttachLink = "csrfToken=" + csrfToken + "&postTitle=&postText=&pollChoice1-ANetPostForm=&pollChoice2-ANetPostForm=&pollChoice3-ANetPostForm=&pollChoice4-ANetPostForm=&pollChoice5-ANetPostForm=&pollEndDate-ANetPostForm=0&contentImageCount=" + ImgCount + "&contentImageIndex=0&contentImage=" + LogoUrl + "&contentEntityID=" + EntityId + "&contentUrl=" + ReqUrl + "&contentTitle=" + contentTitle + "&contentSummary=" + contentSummary + "&contentImageIncluded=true&%23=&gid=" + Gid + "&postItem=&ajax=true&tetherAccountID=&facebookTetherID=";
                                ResponseStatusMsg = HttpHelper.postFormData(new Uri("http://www.linkedin.com/groups"), PostAttachLink);
                            }
                            catch { }


                            string CSVHeader = "UserName" + "," + "HeaderPost" + "," + "Details Post" + "," + "ToGroup";

                            if (ResponseStatusMsg.Contains("SUCCESS") || ResponseStatusMsg.Contains("Accept  the description According to you"))
                            {
                                Log("[ " + DateTime.Now + " ] => [ Attach Link : " + PostGrpAttachLink + " Added Successfully on Group : " + grpDisplay[2] + " ]");

                                string CSV_Content = user + "," + PostGrpAttachLink + "," + PostGrpAttachLink + "," + grpDisplay[2];
                                CSVUtilities.ExportDataCSVFile(CSVHeader, CSV_Content, Globals.path_GroupUpdates);

                            }
                            else if (ResponseStatusMsg.Contains("Your request to join is still pending"))
                            {
                                Log("[ " + DateTime.Now + " ] => [ Your membership is pending approval on a Group:" + grpDisplay[2] + " ]");
                                Log("[ " + DateTime.Now + " ] => [ Attach Link : " + PostGrpAttachLink + " Not Posted on Group:" + grpDisplay[2] + " Because Your membership is pending for approval. ]");

                                GlobusFileHelper.AppendStringToTextfileNewLine("Your membership is pending approval on a Group:" + grpDisplay[2], Globals.path_GroupUpdate);
                                GlobusFileHelper.AppendStringToTextfileNewLine("Message Header: " + PostGrpAttachLink + " Not Posted on Group:" + grpDisplay[2] + " Because Your membership is pending for approval. ", Globals.path_GroupUpdate);
                                GlobusFileHelper.AppendStringToTextfileNewLine("Message More Details: " + PostGrpAttachLink + " Not Posted on Group:" + grpDisplay[2] + " Because Your membership is pending for approval. ", Globals.path_GroupUpdate);
                            }
                            else if (ResponseStatusMsg.Contains("Your post has been submitted for review"))
                            {
                                Log("[ " + DateTime.Now + " ] => [ Message Header Posted : " + PostGrpAttachLink + " Successfully on Group : " + grpDisplay[2] + " ]");
                                Log("[ " + DateTime.Now + " ] => [ Your post has been submitted for review ]");
                                string CSV_Content = user + "," + PostGrpAttachLink + "," + PostGrpAttachLink + "," + grpDisplay[2];

                            }

                            else if (ResponseStatusMsg.Contains("Error"))
                            {
                                Log("[ " + DateTime.Now + " ] => [ Error in Post ]");
                                GlobusFileHelper.AppendStringToTextfileNewLine("Error in Post", Globals.path_GroupUpdate);

                            }
                            else
                            {
                                Log("[ " + DateTime.Now + " ] => [ Message Not Posted ]");
                                GlobusFileHelper.AppendStringToTextfileNewLine("Message Not Posted", Globals.path_GroupUpdate);
                            }

                            if (counter < selectedItems.Count())
                            {
                                int delay = RandomNumberGenerator.GenerateRandom(mindelay, maxdelay);
                                Log("[ " + DateTime.Now + " ] => [ Delay for : " + delay + " Seconds ]");
                                Thread.Sleep(delay * 1000);
                            }

                        }
                        catch (Exception ex)
                        {
                            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update --> cmbGroupUser_SelectedIndexChanged() ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + "    Stack Trace >>> " + ex.StackTrace, Globals.Path_LinkedinGetGroupMemberErrorLogs);
                            Log("[ " + DateTime.Now + " ] => [ Error:" + ex.Message + "StackTrace --> >>>" + ex.StackTrace + " ]");
                        }
                    }
                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update --> cmbGroupUser_SelectedIndexChanged() ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update --> cmbGroupUser_SelectedIndexChanged() ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinGetGroupMemberErrorLogs);
                    Log("[ " + DateTime.Now + " ] => [ Error:" + ex.Message + "StackTrace --> >>>" + ex.StackTrace + " ]");
                }

            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update --> cmbGroupUser_SelectedIndexChanged() ---2--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Group Update --> cmbGroupUser_SelectedIndexChanged() ---2--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinGetGroupMemberErrorLogs);
            }
        }
        #endregion

        #region PostCommentsForLiker
        public Dictionary<string, string> PostCommentsForLiker(ref GlobusHttpHelper HttpHelper, string user)
        {
            try
            {
                string pageSource = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/home?trk=nav_responsive_tab_home"));
                string[] RgxGroupData = new string[] { };

                try
                {
                    RgxGroupData = System.Text.RegularExpressions.Regex.Split(pageSource, "<li class=\"feed-item");
                }
                catch { }

                foreach (var GrpName in RgxGroupData)
                {
                    try
                    {
                        if (GrpName.Contains("<!DOCTYPE html>"))
                        {
                            continue;
                        }

                        //if (GrpName.Contains("<li class=\"feed-like\">"))
                        if (GrpName.Contains("<li class=\"feed-like\""))
                        {

                            string[] RgxFinalData = new string[] { };

                            try
                            {
                                RgxFinalData = System.Text.RegularExpressions.Regex.Split(GrpName, "<div class=\"annotated-body\">");
                            }
                            catch { }

                            string DispMsg = string.Empty;

                            if (RgxFinalData.Count() == 3)
                            {
                                DispMsg = RgxFinalData[2];

                                if (DispMsg.Contains("Like") && (DispMsg.Contains("Comment")))
                                {
                                    string endName = string.Empty;
                                    string endName1 = string.Empty;
                                    string endKey = string.Empty;

                                    try
                                    {
                                        //int startindex = DispMsg.IndexOf("objectType%3D\">");
                                        int startindex = DispMsg.IndexOf("objectType%3D");
                                        string start = DispMsg.Substring(startindex);
                                        int endIndex = start.IndexOf("</a>");
                                        endName = start.Substring(0, endIndex).Replace("objectType%3D\">", string.Empty).Replace("&quot;", string.Empty);
                                    }
                                    catch { }

                                    try
                                    {
                                        int startindex1 = DispMsg.IndexOf("</strong>");
                                        string start1 = DispMsg.Substring(startindex1);
                                        int endIndex1 = start1.IndexOf("</span>");
                                        endName1 = start1.Substring(0, endIndex1).Replace("</strong>", string.Empty).Replace("</span>", string.Empty).Replace("<span>", string.Empty);
                                    }
                                    catch { }

                                    if (endName1 == string.Empty)
                                    {
                                        if (DispMsg.Contains("data-contentPermalink="))
                                        {
                                            try
                                            {
                                                int startindex1 = DispMsg.IndexOf("data-contentPermalink=");
                                                string start1 = DispMsg.Substring(startindex1);
                                                int endIndex1 = start1.IndexOf("\">");
                                                endName1 = start1.Substring(0, endIndex1).Replace("data-contentPermalink=", string.Empty).Replace("\"", string.Empty).Replace("<span>", string.Empty);
                                            }
                                            catch { }
                                        }
                                    }

                                    if (endName1 == string.Empty)
                                    {
                                        try
                                        {
                                            int startindex1 = DispMsg.IndexOf("<span class=\"commentary\">");
                                            string start1 = DispMsg.Substring(startindex1);
                                            int endIndex1 = start1.IndexOf("</span>");
                                            endName1 = start1.Substring(0, endIndex1).Replace("<span class=\"commentary\">", string.Empty).Replace("\n", string.Empty).Replace("                    ", string.Empty).Replace("<span>", string.Empty);
                                        }
                                        catch { }
                                    }

                                    if (endName1 == string.Empty)
                                    {
                                        string[] aaaa = System.Text.RegularExpressions.Regex.Split(DispMsg, "<span>");
                                        string[] bbbb = System.Text.RegularExpressions.Regex.Split(aaaa[1], "</span>");

                                        endName1 = bbbb[0];
                                    }

                                    string LikerEvent = endName + "-" + endName1 + '#' + user;

                                    try
                                    {
                                        //int startindex2 = DispMsg.IndexOf("?id=");
                                        //string start2 = DispMsg.Substring(startindex2);
                                        //int endIndex2 = start2.IndexOf("&amp");
                                        //endKey = start2.Substring(0, endIndex2).Replace("?id=", string.Empty).Replace("/", string.Empty).Replace("<a href=", string.Empty).Replace("\"", string.Empty);
                                        if (endKey == string.Empty)
                                        {
                                            try
                                            {
                                                int startindex3 = DispMsg.IndexOf("discussionID");
                                                string start3 = DispMsg.Substring(startindex3);
                                                int endIndex3 = start3.IndexOf("gid");
                                                endKey = start3.Substring(0, endIndex3).Replace("discussionID", string.Empty).Replace("%3D", string.Empty);
                                            }
                                            catch { }
                                        }

                                        if (endKey.Contains("memberPhoto"))
                                        {
                                            endKey = endKey.Split('&')[0];
                                        }
                                    }
                                    catch { }

                                    if (endKey.Contains("analyticsURL"))
                                    {
                                        continue;
                                    }

                                    if (endKey == string.Empty)
                                    {
                                        try
                                        {
                                            int startindex3 = DispMsg.IndexOf("gid=");
                                            string start3 = DispMsg.Substring(startindex3);
                                            int endIndex3 = start3.IndexOf("&");
                                            endKey = start3.Substring(0, endIndex3).Replace("gid", string.Empty).Replace("=", string.Empty);
                                        }
                                        catch { }
                                    }

                                    try
                                    {
                                        GroupName.Add(LikerEvent, endKey);
                                    }
                                    catch { }
                                }
                            }
                            else if (RgxFinalData.Count() == 2)
                            {
                                DispMsg = RgxFinalData[1];

                                if (DispMsg.Contains("Like") && (DispMsg.Contains("Comment")))
                                {
                                    string endName = string.Empty;
                                    string endName1 = string.Empty;
                                    string endKey = string.Empty;

                                    try
                                    {
                                        int startindex = DispMsg.IndexOf("Aprofile-snapshot\">");
                                        string start = DispMsg.Substring(startindex);
                                        int endIndex = start.IndexOf("</a>");
                                        endName = start.Substring(0, endIndex).Replace("Aprofile-snapshot\">", string.Empty).Replace("&quot;", string.Empty);
                                    }
                                    catch { }

                                    if (endName == string.Empty)
                                    {
                                        try
                                        {
                                            int startindex = DispMsg.IndexOf("Aarticle\">");
                                            string start = DispMsg.Substring(startindex);
                                            int endIndex = start.IndexOf("</a>");
                                            endName = start.Substring(0, endIndex).Replace("Aarticle\">", string.Empty).Replace("&quot;", string.Empty);
                                        }
                                        catch { }
                                    }

                                    if (endName == string.Empty)
                                    {
                                        try
                                        {
                                            int startindex = DispMsg.IndexOf("objectType%3D\">");
                                            string start = DispMsg.Substring(startindex);
                                            int endIndex = start.IndexOf("</a>");
                                            endName = start.Substring(0, endIndex).Replace("objectType%3D\">", string.Empty).Replace("&quot;", string.Empty).Replace("&#x2605;", string.Empty);
                                        }
                                        catch { }
                                    }

                                    if (endName == string.Empty)
                                    {
                                        try
                                        {
                                            int startindex = DispMsg.IndexOf("discussion\">");
                                            string start = DispMsg.Substring(startindex);
                                            int endIndex = start.IndexOf("</a>");
                                            endName = start.Substring(0, endIndex).Replace("discussion\">", string.Empty).Replace("&quot;", string.Empty).Replace("&#x2605;", string.Empty);
                                        }
                                        catch { }
                                    }

                                    try
                                    {
                                        int startindex1 = DispMsg.IndexOf("<span>");
                                        string start1 = DispMsg.Substring(startindex1);
                                        int endIndex1 = start1.IndexOf("</span>");
                                        endName1 = start1.Substring(0, endIndex1).Replace("</strong>", string.Empty).Replace("</span>", string.Empty).Replace("<span>", string.Empty);
                                    }
                                    catch { }

                                    //if (endName1 == string.Empty)
                                    //{
                                    try
                                    {
                                        int startindex1 = DispMsg.IndexOf("<span class=\"commentary\">");
                                        string start1 = DispMsg.Substring(startindex1);
                                        int endIndex1 = start1.IndexOf("</span>");
                                        endName1 = start1.Substring(0, endIndex1).Replace("<span class=\"commentary\">", string.Empty).Replace("\n", string.Empty).Replace("                    ", string.Empty).Replace("<span>", string.Empty);
                                    }
                                    catch { }
                                    //}


                                    if (endName1 == string.Empty)
                                    {
                                        try
                                        {
                                            if (DispMsg.Contains("data-contentPermalink="))
                                            {
                                                int startindex1 = DispMsg.IndexOf("data-contentPermalink=");
                                                string start1 = DispMsg.Substring(startindex1);
                                                int endIndex1 = start1.IndexOf("\">");
                                                endName1 = start1.Substring(0, endIndex1).Replace("data-contentPermalink=", string.Empty).Replace("\"", string.Empty).Replace("<span>", string.Empty);

                                            }
                                        }
                                        catch { }
                                    }

                                    if (endName1 == string.Empty)
                                    {
                                        try
                                        {
                                            if (DispMsg.Contains("<span class=\"description\">"))
                                            {
                                                int startindex1 = DispMsg.IndexOf("<span class=\"description\">");
                                                string start1 = DispMsg.Substring(startindex1);
                                                int endIndex1 = start1.IndexOf("</span>");
                                                endName1 = start1.Substring(0, endIndex1).Replace("<span class=\"description\">", string.Empty).Replace("\"", string.Empty).Replace("<span>", string.Empty);
                                            }
                                        }
                                        catch { }
                                    }

                                    string[] Subject = new string[] { };
                                    string discsubj = string.Empty;
                                    try
                                    {
                                        Subject = DispMsg.Split('>');
                                        if (!(Subject[9].Contains("<div")))        //added new
                                        {
                                            discsubj = " " + Subject[9].Replace("</a", string.Empty).Replace("\n", string.Empty).Replace("</span", string.Empty).Trim();
                                        }
                                    }
                                    catch { }


                                    string LikerEvent = endName + "-" + endName1 + discsubj + '#' + user;

                                    try
                                    {
                                        //int startindex2 = DispMsg.IndexOf("?id=");
                                        //string start2 = DispMsg.Substring(startindex2);
                                        //int endIndex2 = start2.IndexOf("&amp");
                                        //endKey = start2.Substring(0, endIndex2).Replace("?id=", string.Empty).Replace("/", string.Empty).Replace("<a href=", string.Empty).Replace("\"", string.Empty);
                                    }
                                    catch { }

                                    if (endKey.Contains("analyticsURL"))
                                    {
                                        continue;
                                    }

                                    if (endKey == string.Empty)
                                    {
                                        try
                                        {
                                            int startindex3 = DispMsg.IndexOf("gid=");
                                            string start3 = DispMsg.Substring(startindex3);
                                            int endIndex3 = start3.IndexOf("&");
                                            endKey = start3.Substring(0, endIndex3).Replace("gid", string.Empty).Replace("=", string.Empty);
                                        }
                                        catch { }
                                    }
                                    if (endKey == string.Empty)
                                    {
                                        try
                                        {
                                            int startindex3 = DispMsg.IndexOf("topicId=");
                                            string start3 = DispMsg.Substring(startindex3);
                                            int endIndex3 = start3.IndexOf("&");
                                            endKey = start3.Substring(0, endIndex3).Replace("topicId", string.Empty).Replace("=", string.Empty);
                                        }
                                        catch { }
                                    }

                                    /*if (endKey == string.Empty)
                                    {
                                        try
                                        {
                                            int startindex3 = DispMsg.IndexOf("topicId=");
                                            string start3 = DispMsg.Substring(startindex3);
                                            int endIndex3 = start3.IndexOf("&");
                                            endKey = start3.Substring(0, endIndex3).Replace("topicId", string.Empty).Replace("=", string.Empty);
                                        }
                                        catch { }
                                    }*/

                                    if (endKey == string.Empty)
                                    {
                                        try
                                        {
                                            int startindex3 = DispMsg.IndexOf("discussionID");
                                            string start3 = DispMsg.Substring(startindex3);
                                            int endIndex3 = start3.IndexOf("gid");
                                            endKey = start3.Substring(0, endIndex3).Replace("discussionID", string.Empty).Replace("%3D", string.Empty);
                                        }
                                        catch { }
                                    }

                                    try
                                    {
                                        GroupName.Add(LikerEvent, endKey);
                                    }
                                    catch { }
                                }
                            }

                        }
                    }
                    catch { }

                }
                return GroupName;

            }
            catch (Exception ex)
            {
                return GroupName;
            }

        }
        #endregion

        #region PostCommentLikerUpdate
        public void PostCommentLikerUpdate(ref GlobusHttpHelper HttpHelper, List<string> selectedItems, object parameter, int mindelay, int maxdelay)
        {
            string postdata = string.Empty;
            string referal = string.Empty;
            string postUrl = string.Empty;

            try
            {
                string MessageText = string.Empty;
                string PostedMessage = string.Empty;
                string pageSource = string.Empty;
                Array paramsArray = (Array)parameter;

                string user = string.Empty;
                try
                {
                    user = paramsArray.GetValue(1).ToString();
                    //user = paramsArray.GetValue(0).ToString();
                }
                catch { }

                pageSource = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/home?trk=nav_responsive_tab_home"));
                string[] RgxGroupData = System.Text.RegularExpressions.Regex.Split(pageSource, "<li class=\"feed-item");

                foreach (var GrpName in RgxGroupData)
                {
                    try
                    {
                        if (GrpName.Contains("<!DOCTYPE html>"))
                        {
                            continue;
                        }

                        if (GrpName.Contains("<li class=\"feed-like\">"))
                        {
                            string UpdateLikerUrl = string.Empty;

                            try
                            {
                                int startindexLiker = GrpName.IndexOf("data-li-like-url=");
                                string startliker = GrpName.Substring(startindexLiker);
                                int endliker = startliker.IndexOf("Click to like this update");
                                UpdateLikerUrl = startliker.Substring(0, endliker).Replace("data-li-like-url=", string.Empty).Replace("\"", "").Replace("&amp;", "&").Replace("title=", string.Empty).Trim();

                                if (!UpdateLikerUrl.Contains("www.linkedin.com"))
                                {
                                    UpdateLikerUrl = "http://www.linkedin.com" + UpdateLikerUrl;
                                }

                                LikeUrl.Add(UpdateLikerUrl);
                            }
                            catch { }
                        }
                    }
                    catch { }
                }

                try
                {
                    string[] CommentDisplay = new string[] { };
                    int counter = 0;
                    foreach (var Itegid in selectedItems)
                    {
                        try
                        {
                            counter = counter + 1;
                            string Gid = string.Empty;
                            try
                            {
                                CommentDisplay = Itegid.Split('#');
                                string[] PostGid = CommentDisplay[1].Replace("]", string.Empty).Split(',');

                                if (NumberHelper.ValidateNumber(PostGid[1].Trim()))
                                {
                                    Gid = PostGid[1].Trim();
                                }

                            }
                            catch { }

                            foreach (var liker in LikeUrl)
                            {
                                if (liker.Contains(Gid))
                                {

                                    string ResponseStatusMsg = string.Empty;
                                    try
                                    {
                                        referal = "http://www.linkedin.com/home?trk=nav_responsive_tab_home";
                                        ResponseStatusMsg = HttpHelper.getHtmlfromUrl(new Uri(liker), referal);
                                    }
                                    catch { }

                                    string CSVHeader = "UserName" + "," + "StatusUpdate";

                                    if (ResponseStatusMsg.Contains("You"))
                                    {
                                        Log("[ " + DateTime.Now + " ] => [ Comment : " + CommentDisplay[2] + " Liked Successfully ]");

                                        string CSV_Content = user + "," + CommentDisplay[2];
                                        CSVUtilities.ExportDataCSVFile(CSVHeader, CSV_Content, Globals.path_ComentLiker);
                                        break;

                                    }
                                    else if (ResponseStatusMsg.Contains("Error"))
                                    {
                                        Log("[ " + DateTime.Now + " ] => [ Error in Comment Like ]");
                                        GlobusFileHelper.AppendStringToTextfileNewLine("Error in Post", Globals.path_ComentLiker);

                                    }

                                    if (counter < selectedItems.Count())
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
                            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Comment Liker --> ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + "    Stack Trace >>> " + ex.StackTrace, Globals.Path_LinkedinCommentLikerErrorLogs);
                            //Log("[ " + DateTime.Now + " ] => [ Error:" + ex.Message + "StackTrace --> >>>" + ex.StackTrace + " ]");
                        }
                    }

                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Comment Liker --> ---2--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Comment Liker --> ---2--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinCommentLikerErrorLogs);
                    //Log("[ " + DateTime.Now + " ] => [ Error:" + ex.Message + "StackTrace --> >>>" + ex.StackTrace + " ]");
                }

            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Comment Liker --> ---3--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Comment Liker --> ---3--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinCommentLikerErrorLogs);
            }
        }
        #endregion

    }
}
