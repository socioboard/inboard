using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using BaseLib;
using System.Text.RegularExpressions;
using System.IO;

namespace Others
{
    public class StatusUpdate
    {
        #region Global declaration
        public string accountUser = string.Empty;
        public string accountPass = string.Empty;
        public string proxyAddress = string.Empty;
        public string proxyPort = string.Empty;
        public string proxyUserName = string.Empty;
        public string proxyPassword = string.Empty;
        public string Post = string.Empty;
        public static Queue<string> Que_Message_Post = new Queue<string>();

        List<string> RecordURL = new List<string>();
        Queue<string> queRecordUrl = new Queue<string>();
        GlobusHttpHelper _HttpHelper = new GlobusHttpHelper();
        #endregion

        #region StatusUpdate
        public StatusUpdate()
        {
        }
        #endregion

        #region StatusUpdate
        public StatusUpdate(string UserName, string Password, string ProxyAddress, string ProxyPort, string ProxyUserName, string ProxyPassword)
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

        //public void PostStatus(ref GlobusHttpHelper HttpHelper,int mindelay, int maxdelay)
        //{
        //    //foreach (var itemMessage in lstUrlMessage)
        //    //{


        //        try
        //        {
        //            string csrfToken = string.Empty;
        //            string sourceAlias = string.Empty;
        //            string ImgCount = string.Empty;
        //            string LogoUrl = string.Empty;
        //            string EntityId = string.Empty;
        //            string contentTitle = string.Empty;
        //            string contentSummary = string.Empty;
        //            int port = 0;

        //            if (NumberHelper.ValidateNumber(proxyPort))
        //            {
        //                port = Convert.ToInt32(proxyPort);
        //            }

        //            string pageSource = HttpHelper.getHtmlfromUrl(new Uri("http://www.linkedin.com/home?trk=hb_tab_home_top"));
        //            if (pageSource.Contains("csrfToken"))
        //            {
        //                try
        //                {
        //                    csrfToken = pageSource.Substring(pageSource.IndexOf("csrfToken"), 100);
        //                    string[] Arr = csrfToken.Split('&');
        //                    csrfToken = Arr[0];
        //                    csrfToken = csrfToken.Replace("csrfToken=", "");
        //                    csrfToken = csrfToken.Replace("%3A", ":");
        //                }
        //                catch (Exception)
        //                {

        //                }
        //            }
        //            if (pageSource.Contains("sourceAlias"))
        //            {
        //                try
        //                {
        //                    sourceAlias = pageSource.Substring(pageSource.IndexOf("sourceAlias"), 100);
        //                    string[] ArrsourceAlias = sourceAlias.Split('"');
        //                    sourceAlias = ArrsourceAlias[2];
        //                }
        //                catch (Exception)
        //                {

        //                }
        //            }


        //            try
        //            {
        //                string ReqUrl = Post;
        //                string GetStatus = string.Empty;
        //                if (Post.Contains("//") || Post.Contains("wwww") || Post.Contains(".com") || Post.Contains("http://") || Post.Contains("https://"))
        //                {
        //                    ReqUrl = ReqUrl.Replace(":", "%3A").Replace("//", "%2F%2F");
        //                    GetStatus = HttpHelper.getHtmlfromUrl(new Uri("http://www.linkedin.com/share?getPreview=&url=" + ReqUrl));
        //                }

        //                try
        //                {
        //                    int StartinImgCnt = GetStatus.IndexOf("current");
        //                    string startImgCnt = GetStatus.Substring(StartinImgCnt);
        //                    int EndIndexImgCnt = startImgCnt.IndexOf("</span>");
        //                    string EndImgCnt = startImgCnt.Substring(0, EndIndexImgCnt).Replace("value\":", "").Replace("\"", "");
        //                    ImgCount = EndImgCnt.Replace("current", string.Empty).Replace(">", string.Empty);
        //                }
        //                catch
        //                {
        //                    ImgCount = "0";
        //                }

        //                try
        //                {
        //                    int StartinImgUrl = GetStatus.IndexOf("url");
        //                    string startImgUrl = GetStatus.Substring(StartinImgUrl).Replace("url=\"", "");
        //                    int EndIndexImgUrl = startImgUrl.IndexOf("\"");
        //                    string EndImgUrl = startImgUrl.Substring(0, EndIndexImgUrl).Replace("value\":", "").Replace("\"", "");
        //                    LogoUrl = EndImgUrl.Replace("url=", string.Empty);
        //                }
        //                catch { }

        //                try
        //                {
        //                    int StartinEntityId = GetStatus.IndexOf("data-entity-id");
        //                    string startEntityId = GetStatus.Substring(StartinEntityId);
        //                    int EndIndexEntityId = startEntityId.IndexOf("data-entity-url");
        //                    string EndEntityId = startEntityId.Substring(0, EndIndexEntityId).Replace("value\":", "").Replace("\"", "");
        //                    EntityId = EndEntityId.Replace("\"", string.Empty).Replace("data-entity-id", string.Empty).Replace("=", string.Empty);
        //                }
        //                catch { }

        //                try
        //                {
        //                    int StartinContent = GetStatus.IndexOf("share-view-title");
        //                    string startContent = GetStatus.Substring(StartinContent);
        //                    int EndIndexContent = startContent.IndexOf("<p id");
        //                    string EndContent = startContent.Substring(0, EndIndexContent).Replace("value\":", "").Replace("\"", "");
        //                    contentTitle = EndContent.Replace("\"", string.Empty).Replace("\n", string.Empty).Replace("share-view-title", string.Empty).Replace("id=", string.Empty).Replace(">", string.Empty).Replace("</h4", string.Empty);
        //                }
        //                catch { }

        //                try
        //                {
        //                    int StartinConSumm = GetStatus.IndexOf("share-view-summary\">");
        //                    string startConSumm = GetStatus.Substring(StartinConSumm);
        //                    int EndIndexConSumm = startConSumm.IndexOf("<");
        //                    string EndConSumm = startConSumm.Substring(0, EndIndexConSumm).Replace("value\":", "").Replace("\"", "");
        //                    contentSummary = EndConSumm.Replace("\"", string.Empty).Replace("\n", string.Empty).Replace("share-view-summary", string.Empty).Replace("id=", string.Empty).Replace(">", string.Empty).Replace("</span<a href=#", string.Empty).Trim();
        //                    contentSummary = contentSummary.Replace(",", "%2C").Replace(" ", "%20");
        //                }
        //                catch { }

        //                string PostStatusData = string.Empty;
        //                if (EntityId == string.Empty)
        //                {
        //                    PostStatusData = "ajax=true&contentImageCount=0&contentImageIndex=-1&contentImage=&contentEntityID=&contentUrl=&postText=" + Uri.EscapeDataString(ReqUrl) + "&contentTitle=&contentSummary=&contentImageIncluded=true&%23=&postVisibility2=EVERYONE&submitPost=&tetherAccountID=&tweetThisOn=false&postToMFeedDefaultPublic=true&csrfToken=" + csrfToken + "&sourceAlias=" + sourceAlias + "";
        //                }
        //                else
        //                {
        //                    //ajax=true&contentImageCount=0&contentImageIndex=-1&contentImage=false       &contentEntityID=ARTC_5708506036495716432&contentUrl=http%3A%2F%2Fen.wikipedia.org%2Fwiki%2FCompany&postText=http%3A%2F%2Fen.wikipedia.org%2Fwiki%2FCompany&contentUrl=http%3A%2F%2Fen.wikipedia.org%2Fwiki%2FCompany&contentTitle=Company&contentSummary=The%20examples%20and%20perspective%20in%20this%20article%20may%20not%20represent%20a%20worldwide%20view%20of%20the%20subject.%20Please%20improve%20this%20article%20and%20discuss%20the%20issue%20on%20the%20talk%20page.%20(April%202010)%20A%20company%20is%20an%20association%20or%20collection%20of...&postVisibility2=EVERYONE&submitPost=&tetherAccountID=&tweetThisOn=false&postToMFeedDefaultPublic=true&csrfToken=ajax%3A1855046023605804993&sourceAlias=0_0aUs533-gpztrJxp65YSDh
        //                    PostStatusData = "ajax=true&contentImageCount=" + ImgCount + "&contentImageIndex=0&contentImage=" + LogoUrl + "&contentEntityID=" + EntityId + "&contentUrl=" + ReqUrl + "&postText=" + Uri.EscapeDataString(Post) + "&contentTitle=" + contentTitle + "&contentSummary=" + contentSummary + "&contentImageIncluded=true&%23=&postVisibility2=EVERYONE&submitPost=&tetherAccountID=&tweetThisOn=false&postToMFeedDefaultPublic=true&csrfToken=" + Uri.EscapeDataString(csrfToken) + "&sourceAlias=" + sourceAlias + "";
        //                }
        //                //ajax=true&contentImageCount=0&contentImageIndex=-1&contentImage=false&contentEntityID=ARTC_5708506036495716432&contentUrl=http%3A%2F%2Fen.wikipedia.org%2Fwiki%2FCompany&postText=http%3A%2F%2Fen.wikipedia.org%2Fwiki%2FCompany&contentUrl=http%3A%2F%2Fen.wikipedia.org%2Fwiki%2FCompany&contentTitle=Company&contentSummary=The%20examples%20and%20perspective%20in%20this%20article%20may%20not%20represent%20a%20worldwide%20view%20of%20the%20subject.%20Please%20improve%20this%20article%20and%20discuss%20the%20issue%20on%20the%20talk%20page.%20(April%202010)%20A%20company%20is%20an%20association%20or%20collection%20of...&postVisibility2=EVERYONE&submitPost=&tetherAccountID=&tweetThisOn=false&postToMFeedDefaultPublic=true&csrfToken=ajax%3A1855046023605804993&sourceAlias=0_0aUs533-gpztrJxp65YSDh
        //                PostStatusData = PostStatusData.Replace(" ", "");
        //                string ResponseStatusMsg = HttpHelper.postFormData(new Uri("http://www.linkedin.com/nhome/submit-post"), PostStatusData);

        //                if (ResponseStatusMsg.Contains("There was a problem performing this action, please try again later."))
        //                {
        //                    string firstRepsonse = HttpHelper.getHtmlfromUrlProxy(new Uri("http://www.linkedin.com/share?getPreview=&url=" + Uri.EscapeDataString(Post)), proxyAddress, port, proxyUserName, proxyPassword);
        //                    string posturldata = "ajax=true&contentImageCount=0&contentImageIndex=0&contentImage=false&contentEntityID=" + EntityId + "&contentUrl=" + ReqUrl + "&postText=" + Uri.EscapeDataString(Post) + "&contentUrl=" + Uri.EscapeDataString(Post) + "&contentTitle=" + contentTitle + "&contentSummary=" + contentSummary + "&postVisibility2=EVERYONE&submitPost=&tetherAccountID=&tweetThisOn=false&postToMFeedDefaultPublic=true&csrfToken=" + Uri.EscapeDataString(csrfToken) + "&sourceAlias=" + sourceAlias;
        //                    ResponseStatusMsg = HttpHelper.postFormDataProxy(new Uri("http://www.linkedin.com/nhome/submit-post"), posturldata, proxyAddress, port, proxyUserName, proxyPassword);
        //                }

        //                if (ResponseStatusMsg.Contains("Your update has been posted") || ResponseStatusMsg.Contains("You have successfully shared this update"))
        //                {
        //                    Log("Status Updated With: " + accountUser);
        //                    Log("Status Posted: " + ReqUrl);
        //                    Log("Status>> Updated Successfully.");
        //                    GlobusFileHelper.AppendStringToTextfileNewLine(accountUser + ":" + ReqUrl, Globals.path_PostStatus);
        //                }
        //                else if (ResponseStatusMsg.Contains("You have exceeded the maximum length by 965 character(s)."))
        //                {
        //                    Log(accountUser + " You have exceeded the maximum length by 965 character(s).");
        //                    GlobusFileHelper.AppendStringToTextfileNewLine(accountUser + ":" + ReqUrl, Globals.path_NonPostStatus);
        //                }
        //                else if (ResponseStatusMsg.Contains("We were unable to post your update since it is a duplicate of your most recent update"))
        //                {
        //                    Log(accountUser + " We were unable to post your update since it is a duplicate of your most recent update");
        //                    GlobusFileHelper.AppendStringToTextfileNewLine(accountUser + ":" + ReqUrl, Globals.path_NonPostStatus);
        //                }
        //                else
        //                {
        //                    Log(accountUser + " Status Not Posted");
        //                    GlobusFileHelper.AppendStringToTextfileNewLine(accountUser + ":" + ReqUrl, Globals.path_NonPostStatus);
        //                }

        //                int delay = RandomNumberGenerator.GenerateRandom(mindelay, maxdelay);
        //                Log("Delay for " + delay + " Seconds");
        //                Log("-----------------------------------------------------------------------------------------------------------------------------");
        //                Thread.Sleep(delay * 1000);

        //            }
        //            catch (Exception ex)
        //            {
        //                GlobusFileHelper.AppendStringToTextfileNewLine(accountUser + ":" + Post, Globals.path_NonPostStatus);
        //            }

        //            finally
        //            {

        //            }
        //        }
        //        catch {  }
        //    //}
        //}

        #region PostStatusMsg
    // public void PostStatusMsg(ref GlobusHttpHelper HttpHelper, Boolean Statuswithurl, int mindelay, int maxdelay)
     public void PostStatusMsg(ref GlobusHttpHelper HttpHelper, Boolean Statuswithurl, int mindelay, int maxdelay, string spinnedStatus, Boolean isSpinTrue)
     {
            try
            {
                string csrfToken = string.Empty;
                string sourceAlias = string.Empty;
                string ImgCount = string.Empty;
                string LogoUrl = string.Empty;
                string EntityId = string.Empty;
                string contentTitle = string.Empty;
                string contentSummary = string.Empty;
                int port = 0;
                
                if (NumberHelper.ValidateNumber(proxyPort))
                {
                    port = Convert.ToInt32(proxyPort);
                }

                string pageSource = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/home?trk=hb_tab_home_top"));
                if (pageSource.Contains("session_full_logout=&amp;"))
                {
                    try
                    {
                        csrfToken = pageSource.Substring(pageSource.IndexOf("session_full_logout=&amp;"), 500);
                        string[] Arr = csrfToken.Split('&');
                        csrfToken = Arr[1];
                        csrfToken = csrfToken.Replace("csrfToken=", "");
                        csrfToken = csrfToken.Replace("%3A", ":").Replace("\n", string.Empty).Replace("\"", string.Empty).Replace("amp;",string.Empty).Replace(">", string.Empty).Trim();
                    }
                    catch (Exception)
                    {

                    }
                }
                if (pageSource.Contains("sourceAlias"))
                {
                    try
                    {
                        sourceAlias = pageSource.Substring(pageSource.IndexOf("sourceAlias"), 100);
                        string[] ArrsourceAlias = sourceAlias.Split('"');
                        sourceAlias = ArrsourceAlias[2];
                    }
                    catch (Exception)
                    {

                    }
                }

                try
                {
                    string ReqUrl = Post;
                    string GetStatus = string.Empty;
                    //if (Statuswithurl == false)
                    //{
                    //    spinnedStatus = GlobusSpinHelper.spinLargeText(new Random(), spinnedStatus);
                    //    Post = spinnedStatus;
                    //}
                    if (isSpinTrue == true)
                    {
                        spinnedStatus = GlobusSpinHelper.spinLargeText(new Random(), spinnedStatus);
                        Post = spinnedStatus;
                    }
                    if (Post.Contains("//") || Post.Contains("www") || Post.Contains(".com") || Post.Contains("http://") || Post.Contains("https://"))
                    {
                        ReqUrl = ReqUrl.Replace(":", "%3A").Replace("//", "%2F%2F");
                        ReqUrl = ReqUrl.Replace(":", "%3A").Replace("/", "%2F");
                        GetStatus = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/share?getPreview=&url=" + ReqUrl));
                    }
                    if (!(GetStatus.Contains("<WSResponse>")))
                    {
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

                        try
                        {
                            //int StartinImgUrl = GetStatus.IndexOf("url");
                            //string startImgUrl = GetStatus.Substring(StartinImgUrl).Replace("url=\"", "");
                            //int EndIndexImgUrl = startImgUrl.IndexOf("\"");
                            //string EndImgUrl = startImgUrl.Substring(0, EndIndexImgUrl).Replace("value\":", "").Replace("\"", "");
                            //LogoUrl = EndImgUrl.Replace("url=", string.Empty);
                            int startindexImgurl = GetStatus.IndexOf("origImages:");
                            string startImgurl = GetStatus.Substring(startindexImgurl).Replace("origImages:", string.Empty);
                            int endindexImgurl = startImgurl.IndexOf("]");
                            string endImgUrl = startImgurl.Substring(0, endindexImgurl).Replace("\\", string.Empty).Replace("[", string.Empty).Replace("\"", string.Empty).Trim();
                            LogoUrl = endImgUrl;
                            LogoUrl = LogoUrl.Replace(":", "%3A").Replace("/", "%2F");
                        }
                        catch { }

                        try
                        {
                            int StartinEntityId = GetStatus.IndexOf("data-entity-id");
                            string startEntityId = GetStatus.Substring(StartinEntityId);
                            int EndIndexEntityId = startEntityId.IndexOf("data-entity-url");
                            string EndEntityId = startEntityId.Substring(0, EndIndexEntityId).Replace("value\":", "").Replace("\"", "");
                            EntityId = EndEntityId.Replace("\"", string.Empty).Replace("data-entity-id", string.Empty).Replace("=", string.Empty);
                        }
                        catch { }

                        try
                        {
                            int StartinContent = GetStatus.IndexOf("share-view-title");
                            string startContent = GetStatus.Substring(StartinContent);
                            int EndIndexContent = startContent.IndexOf("<p id");
                            string EndContent = startContent.Substring(0, EndIndexContent).Replace("value\":", "").Replace("\"", "");
                            contentTitle = EndContent.Replace("\"", string.Empty).Replace("\n", string.Empty).Replace("share-view-title", string.Empty).Replace("id=", string.Empty).Replace(">", string.Empty).Replace("</h4", string.Empty);
                        }
                        catch { }

                        try
                        {
                            int StartinConSumm = GetStatus.IndexOf("share-view-summary\">");
                            string startConSumm = GetStatus.Substring(StartinConSumm);
                            int EndIndexConSumm = startConSumm.IndexOf("<");
                            string EndConSumm = startConSumm.Substring(0, EndIndexConSumm).Replace("value\":", "").Replace("\"", "");
                            contentSummary = EndConSumm.Replace("\"", string.Empty).Replace("\n", string.Empty).Replace("share-view-summary", string.Empty).Replace("id=", string.Empty).Replace(">", string.Empty).Replace("</span<a href=#", string.Empty).Trim();
                            contentSummary = contentSummary.Replace(",", "%2C").Replace(" ", "%20");
                        }
                        catch { }

                        string PostStatusData = string.Empty;
                        if (EntityId == string.Empty)
                        {
                            PostStatusData = "ajax=true&contentImageCount=0&contentImageIndex=-1&contentImage=&contentEntityID=&contentUrl=&postText=" + Uri.EscapeDataString(Post) + "&contentTitle=&contentSummary=&contentImageIncluded=true&%23=&postVisibility2=EVERYONE&submitPost=&tetherAccountID=&tweetThisOn=false&postToMFeedDefaultPublic=true&csrfToken=" + csrfToken + "&sourceAlias=" + sourceAlias + "";
                        }
                        else
                        {
                            PostStatusData = "ajax=true&contentImageCount=" + ImgCount + "&contentImageIndex=0&contentImage=" + LogoUrl + "&contentEntityID=" + EntityId + "&contentUrl=" + ReqUrl + "&postText=" + Uri.EscapeDataString(Post) + "&contentTitle=" + contentTitle + "&contentSummary=" + contentSummary + "&contentImageIncluded=true&%23=&postVisibility2=EVERYONE&submitPost=&tetherAccountID=&tweetThisOn=false&postToMFeedDefaultPublic=true&csrfToken=" + Uri.EscapeDataString(csrfToken) + "&sourceAlias=" + sourceAlias + "";
                            // PostStatusData = "ajax=true&contentImageCount=" + ImgCount + "&contentImageIndex=0&contentImage=http%3A%2F%2Ftimesofindia.indiatimes.com%2Fphoto%2F37174983.cms&contentEntityID=" + EntityId + "&contentUrl=http%3A%2F%2Ftimesofindia.indiatimes.com%2Ftech%2Ftech-news%2FGoogles-Skybox-deal-raises-regulatory-concern%2Farticleshow%2F37174862.cms&mentions=%5B%5D&postText=" + Uri.EscapeDataString(Post) + "&contentTitle=" + contentTitle + "&contentSummary=" + contentSummary + "&contentImageIncluded=true&%23=&postVisibility2=EVERYONE&submitPost=&tetherAccountID=&tweetThisOn=false&postToMFeedDefaultPublic=true&csrfToken=" + Uri.EscapeDataString(csrfToken) + "&sourceAlias=" + sourceAlias + "";
                            //PostStatusData = "ajax=true&contentImageCount=1&contentImageIndex=0&contentImage=http%3A%2F%2Ftimesofindia.indiatimes.com%2Fphoto%2F37174983.cms&contentEntityID=ARTC_8537949489495079643&contentUrl=http%3A%2F%2Ftimesofindia.indiatimes.com%2Ftech%2Ftech-news%2FGoogles-Skybox-deal-raises-regulatory-concern%2Farticleshow%2F37174862.cms&mentions=%5B%5D&postText=http%3A%2F%2Ftimesofindia.indiatimes.com%2Ftech%2Ftech-news%2FGoogles-Skybox-deal-raises-regulatory-concern%2Farticleshow%2F37174862.cms&share-entity-typeahead=&contentUrl=http%3A%2F%2Ftimesofindia.indiatimes.com%2Ftech%2Ftech-news%2FGoogles-Skybox-deal-raises-regulatory-concern%2Farticleshow%2F37174862.cms&contentImageIncluded=true&contentTitle=Google's%20Skybox%20deal%20raises%20regulatory%20concern%20-%20The%20Times%20of%20India&contentSummary=Consumer%20watchdog%20Public%20Citizen%20has%20called%20on%20US%20regulators%20to%20conduct%20a%20review%20of%20Google%20Inc's%20recent%20acquisition%20of%20aerospace%20startup%20Skybox%20Imaging.&postVisibility2=EVERYONE&tetherAccountID=139016702&submitPost=&isDark=false&tweetThisOn=false&postToMFeedDefaultPublic=true&csrfToken=ajax%3A5539773821497530765&sourceAlias=0_0aUs533-gpztrJxp65YSDh&pageKey=member-home"; 
                        }

                        PostStatusData = PostStatusData.Replace(" ", "");
                        string ResponseStatusMsg = HttpHelper.postFormData(new Uri("http://www.linkedin.com/nhome/submit-post"), PostStatusData);
                        Thread.Sleep(2000);

                        if (ResponseStatusMsg.Contains("There was a problem performing this action, please try again later."))
                        {
                            string firstRepsonse = HttpHelper.getHtmlfromUrlProxy(new Uri("http://www.linkedin.com/share?getPreview=&url=" + Uri.EscapeDataString(Post)), proxyAddress, port, proxyUserName, proxyPassword);
                            string posturldata = "ajax=true&contentImageCount=0&contentImageIndex=0&contentImage=false&contentEntityID=" + EntityId + "&contentUrl=" + ReqUrl + "&postText=" + Uri.EscapeDataString(Post) + "&contentUrl=" + Uri.EscapeDataString(Post) + "&contentTitle=" + contentTitle + "&contentSummary=" + contentSummary + "&postVisibility2=EVERYONE&submitPost=&tetherAccountID=&tweetThisOn=false&postToMFeedDefaultPublic=true&csrfToken=" + Uri.EscapeDataString(csrfToken) + "&sourceAlias=" + sourceAlias;
                            ResponseStatusMsg = HttpHelper.postFormDataProxy(new Uri("http://www.linkedin.com/nhome/submit-post"), posturldata, proxyAddress, port, proxyUserName, proxyPassword);
                        }

                        if (ResponseStatusMsg.Contains("Your update has been posted") || ResponseStatusMsg.Contains("success"))
                        {
                            if (Statuswithurl == true)
                            {
                                Log("[ " + DateTime.Now + " ] => [ URL Status Updated With: " + accountUser + " ]");
                                Log("[ " + DateTime.Now + " ] => [ URL Status Posted: " + Post + " ]");
                                Log("[ " + DateTime.Now + " ] => [ URL Status : Updated Successfully. ]");
                            }
                            else
                            {
                                Log("[ " + DateTime.Now + " ] => [ Status Updated With: " + accountUser + " ]");
                                Log("[ " + DateTime.Now + " ] => [ Status Posted: " + Post + " ]");
                                Log("[ " + DateTime.Now + " ] => [ Status : Updated Successfully. ]");
                            }

                            GlobusFileHelper.AppendStringToTextfileNewLine(accountUser + ":" + Post, Globals.path_PostStatus);
                        }
                        else if (ResponseStatusMsg.Contains("You have exceeded the maximum length by 965 character(s)."))
                        {
                            Log("[ " + DateTime.Now + " ] => [ " + accountUser + " You have exceeded the maximum length by 965 character(s). ]");
                            GlobusFileHelper.AppendStringToTextfileNewLine(accountUser + ":" + Post, Globals.path_NonPostStatus);
                        }
                        else if (ResponseStatusMsg.Contains("We were unable to post your update since it is a duplicate of your most recent update"))
                        {
                            Log("[ " + DateTime.Now + " ] => [ " + accountUser + " We were unable to post your update since it is a duplicate of your most recent update ]");
                            GlobusFileHelper.AppendStringToTextfileNewLine(accountUser + ":" + Post, Globals.path_NonPostStatus);
                        }
                        else
                        {
                            if (Statuswithurl == true)
                            {
                                Log("[ " + DateTime.Now + " ] => [ " + accountUser + " URL Status Not Posted ]");
                            }
                            else
                            {
                                Log("[ " + DateTime.Now + " ] => [ " + accountUser + " Status Not Posted ]");
                            }
                            GlobusFileHelper.AppendStringToTextfileNewLine(accountUser + ":" + Post, Globals.path_NonPostStatus);
                        }

                        int delay = RandomNumberGenerator.GenerateRandom(mindelay, maxdelay);
                        Log("[ " + DateTime.Now + " ] => [ Delay for : " + delay + " Seconds ]");
                        Thread.Sleep(delay * 1000);
                    }


                    else
                    {
                        Log("[ " + DateTime.Now + " ] => [ " + accountUser + " Error in status update. Please try again ]");
                    }
                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine(accountUser + ":" + Post, Globals.path_NonPostStatus);
                }

                finally
                {

                }
            }
            catch { }
        }
        #endregion
                
        #region UpdateStatusUsingAllurl
        public void UpdateStatusUsingAllurl(ref GlobusHttpHelper HttpHelper, int mindelay, int maxdelay)
        {
            try
            {
                string csrfToken = string.Empty;
                string sourceAlias = string.Empty;
                string ImgCount = string.Empty;
                string LogoUrl = string.Empty;
                string mentioned = string.Empty;
                string EntityId = string.Empty;
                string contentTitle = string.Empty;
                string contentSummary = string.Empty;
                string progressId = string.Empty;

                string pageSource = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/home?trk=hb_tab_home_top"));
                if (pageSource.Contains("csrfToken"))
                {
                    csrfToken = pageSource.Substring(pageSource.IndexOf("csrfToken"), 100);
                    string[] Arr = csrfToken.Split('&');
                    csrfToken = Arr[0];
                    csrfToken = csrfToken.Replace("csrfToken=", "");
                    csrfToken = csrfToken.Replace("%3A", ":");
                }
                if (pageSource.Contains("sourceAlias"))
                {
                    sourceAlias = pageSource.Substring(pageSource.IndexOf("sourceAlias"), 100);
                    string[] ArrsourceAlias = sourceAlias.Split('"');
                    sourceAlias = ArrsourceAlias[2];
                }

                if (pageSource.Contains("X-Progress-ID"))
                {
                    progressId = pageSource.Substring(pageSource.IndexOf("X-Progress-ID"), 100);
                    string[] ArrsprogressId = progressId.Split('"');
                    progressId = ArrsprogressId[0].Replace("X-Progress-ID=", string.Empty).Replace("=",string.Empty);
                }
                string aaa = "lite/web-action-track?csrfToken=" + csrfToken +"";
                string post = HttpHelper.postFormData(new Uri("http://www.linkedin.com/"),aaa);

                string post1 = "X-Progress-ID=" + progressId + "&iframe_jsonp=true&window_post=true&post_window=parent&jsonp_callback=SlideshareUploader"+progressId;
                string pageSource1 = HttpHelper.postFormDataRef(new Uri("http://slideshare.www.linkedin.com/upload?"), post1, "http://www.linkedin.com/", csrfToken,"");
               // string pageSource1 = HttpHelper.getHtmlfromUrl(new Uri("http://www.linkedin.com/nhome/uscp-poll?queryAfter=1387628141050&goback=%2Enmp_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1&orderBy=Time&showHidden=false&realTimeTest=C"));
                try
                {
                    mentioned = "%5B%5D";
                }
                catch { }

                try
                {
 
                    string ReqUrl = Post;
                    //ReqUrl = ReqUrl.Replace(":", "%3A").Replace("//", "%2F%2F");
                   // string pageSource1 = HttpHelper.getHtmlfromUrl(new Uri("http://www.linkedin.com/nhome/uscp-poll?queryAfter=1387606658969&goback=%2Enmp_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1&orderBy=Time&showHidden=false&realTimeTest=C"));
                    csrfToken = csrfToken.Replace(":","%3A");
                    string postUrlData = " https://www.linkedin.com/lite/web-action-track?csrfToken=" + csrfToken;
                    string postData = "pkey=member-home&tcode=hp-shr-actvt-msg&plist=";
                    string ResponseStatusMsg1 = HttpHelper.postFormData(new Uri(postUrlData), postData);
                    string pageSource11 = "";
                    //string GetStatus = HttpHelper.getHtmlfromUrl(new Uri("http://www.linkedin.com/share?getPreview=&url=" + ReqUrl));
                    //string GetStatus = HttpHelper.getHtmlfromUrl(new Uri("http://www.linkedin.com/nhome/uscp-poll?queryAfter=1387606658969&goback=%2Enmp_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1&orderBy=Time&showHidden=false&realTimeTest=C"));
                   /* try
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

                    try
                    {
                        int StartinImgUrl = GetStatus.IndexOf("url");
                        string startImgUrl = GetStatus.Substring(StartinImgUrl);
                        int EndIndexImgUrl = startImgUrl.IndexOf("\"");
                        string EndImgUrl = startImgUrl.Substring(0, EndIndexImgUrl).Replace("value\":", "").Replace("\"", "");
                        LogoUrl = EndImgUrl.Replace("url=", string.Empty).Trim();
                    }
                    catch
                    {
                        LogoUrl = "false";
                    }

                    try
                    {
                        int StartinEntityId = GetStatus.IndexOf("data-entity-id");
                        string startEntityId = GetStatus.Substring(StartinEntityId);
                        int EndIndexEntityId = startEntityId.IndexOf("data-entity-url");
                        string EndEntityId = startEntityId.Substring(0, EndIndexEntityId).Replace("value\":", "").Replace("\"", "");
                        EntityId = EndEntityId.Replace("\"", string.Empty).Replace("data-entity-id", string.Empty).Replace("=", string.Empty).Trim();
                    }
                    catch { }

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
                    */
                    string PostStatusData = string.Empty;
                    if (EntityId == string.Empty)
                    {
                        //PostStatusData = "ajax=true&contentImageCount=0&contentImageIndex=-1&contentImage=&contentEntityID=&contentUrl=&postText=" + Uri.EscapeDataString(Post) + "&contentTitle=&contentSummary=&contentImageIncluded=true&%23=&postVisibility=EVERYONE&submitPost=&tetherAccountID=&tweetThisOn=false&postToMFeedDefaultPublic=true&csrfToken=" + csrfToken + "&sourceAlias=" + sourceAlias + "";
                        PostStatusData="POST /lite/web-action-track?csrfToken=ajax%3A3714738396404051762";
                    }
                    else
                    {
                        PostStatusData = "ajax=true&contentImageCount=" + ImgCount + "&contentImageIndex=-1&contentImage=" + Uri.EscapeDataString(LogoUrl) + "&contentEntityID=" + EntityId + "&contentUrl=" + ReqUrl + "&mentions=" + mentioned + "&postText=" + Uri.EscapeDataString(Post) + "&share-entity-typeahead=&contentUrl=" + ReqUrl + "&contentImageIncluded=true&contentTitle=" + contentTitle + "&contentSummary=" + contentSummary + "&postVisibility2=EVERYONE&submitPost=&tetherAccountID=&tweetThisOn=false&postToMFeedDefaultPublic=true&csrfToken=" + csrfToken + "&sourceAlias=" + sourceAlias + "";

                    }

                    //string ResponseStatusMsg = HttpHelper.postFormData(new Uri("http://www.linkedin.com/nhome/submit-post"), PostStatusData);
                    string ResponseStatusMsg = HttpHelper.postFormData(new Uri("http://www.linkedin.com/home?trk=nav_responsive_tab_home"), PostStatusData);

                    if (ResponseStatusMsg.Contains("Your update has been posted") || ResponseStatusMsg.Contains("success"))
                    {
                        Log("[ " + DateTime.Now + " ] => [ Url Status Updated With: " + accountUser + " ]");
                        Log("[ " + DateTime.Now + " ] => [ Url Status Posted: " + Post + " ]");
                        Log("[ " + DateTime.Now + " ] => [ Url Status: Updated Successfully. ]");
                        GlobusFileHelper.AppendStringToTextfileNewLine(accountUser + ":" + Post, Globals.path_PostStatus);
                    }
                    else if (ResponseStatusMsg.Contains("You have exceeded the maximum length by 965 character(s)."))
                    {
                        Log("[ " + DateTime.Now + " ] => [ " + accountUser + " You have exceeded the maximum length by 965 character(s). ]");
                    }
                    else
                    {
                        Log("[ " + DateTime.Now + " ] => [ " + accountUser + " Url Status Not Posted ]");
                    }

                    int delay = RandomNumberGenerator.GenerateRandom(mindelay, maxdelay);
                    Log("[ " + DateTime.Now + " ] => [ Delay for : " + delay + " Seconds ]");
                    Thread.Sleep(delay * 1000);
                }
                catch (Exception ex)
                {

                }

                finally
                {

                }
            }
            catch { }
        }
        #endregion

        public void startJobScrapingDetails(ref GlobusHttpHelper httpHelper , List<string> lstUrlJobScraper)
        {
            try
            {
                int countPageNum = 1;
                int pageCount = 0;
                int i = 1;
                bool IsCheckCount = true; ;
                foreach (string item in lstUrlJobScraper)
                {
                    string tempItem = string.Empty;
                    if (item.Contains("&page_num=1"))
                    {
                    StartAgain:
                        tempItem = item.Replace("&page_num=1", "");
                        tempItem = tempItem + "&page_num=" + countPageNum;


                    string PageSource = httpHelper.getHtmlfromUrl(new Uri(tempItem));
                    string tempCount = string.Empty;
                    if (IsCheckCount)
                    {
                        tempCount = Utils.getBetween(PageSource, "resultCount\":", ",");


                        try
                        {
                            pageCount = int.Parse(tempCount);

                            Log("[ " + DateTime.Now + " ] => [ Total Results :  " + pageCount + " ]");

                        }
                        catch (Exception)
                        {

                        }

                        pageCount = (pageCount / 25) + 1;

                        if (pageCount == -1)
                        {
                            pageCount = 2;
                        }

                        if (pageCount == 1)
                        {
                            pageCount = 2;
                        }

                    }

                    if (pageCount >= 1)
                    {
                        _HttpHelper = httpHelper;

                            new Thread(() =>
                            {
                                if (IsCheckCount)
                                {
                                    finalUrlCollectionForRecruter();
                                }

                            }).Start();
                       
                    }


                        while (i <= pageCount)
                        {
                            if (true)
                            {


                                if (PageSource.Contains("&jobId="))
                                {
                                    try
                                    {
                                        List<string> PageSerchUrl = GettingAllUrl(PageSource);
                                        PageSerchUrl.Distinct();

                                        if (PageSerchUrl.Count == 0)
                                        {
                                            Log("[ " + DateTime.Now + " ] => [ On the basis of your Account you can able to see " + RecordURL.Count + " Results ]");
                                            break;
                                        }

                                        foreach (string tempitem in PageSerchUrl)
                                        {
                                            if (true)
                                            {
                                                if (tempitem.Contains("jobs2/view/"))
                                                {
                                                    try
                                                    {
                                                        string urlSerch = tempitem;
                                                        if (urlSerch.Contains("jobs2/view/"))
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

                                        if (i == pageCount)
                                        {
                                            IsCheckCount = true;
                                            break;
                                        }

                                        countPageNum ++ ;
                                        i++;
                                        Thread.Sleep(4000);
                                        IsCheckCount = false;
                                        goto StartAgain;
                                    }
                                    catch { }
                                }

                            }
                        }


                    }
                }
            }
            catch { }
        }

        #region finalUrlCollection
        private void finalUrlCollectionForRecruter()
        {
            string Account = string.Empty;
           
            try
            {
                try
                {
                    Globals.lstCompanyEmployeeScraperThread.Add(Thread.CurrentThread);
                    Globals.lstCompanyEmployeeScraperThread = Globals.lstCompanyEmployeeScraperThread.Distinct().ToList();
                    Thread.CurrentThread.IsBackground = true;
                }
                catch { }

                List<string> numburlpp = new List<string>();
                GlobusHttpHelper HttpHelper = _HttpHelper;
                if (true)
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

                                if (item.Contains("/view/"))
                                {

                                    string urltemp = item;
                                    numburlpp.Add(urltemp);


                                    Log("[ " + DateTime.Now + " ] => [ " + urltemp + " ]");

                                    Log("[ " + DateTime.Now + " ] => [ Fetching Data From URL ]");
                                   
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


        public bool CrawlingLinkedInPageRecruiter(string Url, ref GlobusHttpHelper HttpHelper)
        {
            bool isscraped = false;
            string Jobtitle = string.Empty;
            string Location = string.Empty;
            string PersonUrlLink = string.Empty;
            string FirstName = string.Empty;
            string LastName = string.Empty;
            string specialites = string.Empty;
            string Website = string.Empty;
            string Industry = string.Empty;
            string ProfileUrl = string.Empty;
            string strFamilyName = string.Empty;
            string careerDetails = string.Empty;
            try
            {
                //Url = "https://www.linkedin.com/jobs2/view/38612041?trk=vsrp_jobs_res_name&trkInfo=VSRPsearchId%3A82134271427382117065%2CVSRPtargetId%3A38612041%2CVSRPcmpt%3Aprimary";
                string pagesource = HttpHelper.getHtmlfromUrl(new Uri(Url));

                if (!pagesource.Contains("Contact the job poster"))
                {
                    Log("[ " + DateTime.Now + " ] => [ No Data Found For Url "+Url+" ] ");
                    return false; ;
                }
                Jobtitle = Utils.getBetween(pagesource, "itemprop=\"title\">", "</h1>").Replace(",",";").Replace("&amp;","&");
                Location = Utils.getBetween(pagesource, "itemprop=\"description\">", "</span>").Replace(",", ";").Replace("&amp;", "&");
                if (string.IsNullOrEmpty(Location))
                {
                    Location = Utils.getBetween(pagesource, "location\":", ",").Replace(",", ";").Replace("&amp;", "&");
                }
                careerDetails = Utils.getBetween(pagesource, "companyPageNameLink\":", ",").Replace("\"", "").Replace(",", ";").Replace("&amp;", "&").Replace("careers?", "home?");
                string subPagedetails = HttpHelper.getHtmlfromUrl(new Uri(careerDetails));
                List<string> websiteAddress = HttpHelper.GetTextDataByTagAndAttributeName(subPagedetails, "li", "website");
                if (websiteAddress.Count > 0)
                {
                    Website = websiteAddress[0].Replace("Website", "").Replace(",", ";").Replace("&amp;", "&");
                }

                List<string> specialtiesAddress = HttpHelper.GetTextDataByTagAndAttributeName(subPagedetails, "div", "specialties");
                if (specialtiesAddress.Count > 0)
                {
                    specialites = specialtiesAddress[0].Replace("specialties", "").Replace(",", ";").Replace("&amp;", "&");
                }
                List<string> lstIndustry = HttpHelper.GetTextDataByTagAndAttributeName(subPagedetails, "li", "industry");
                if (lstIndustry.Count > 0)
                {
                    Industry = lstIndustry[0].Replace("Industry", "").Replace(",", ";").Replace("&amp;", "&");
                }

                string tempPagesource = Utils.getBetween(pagesource, "<div class=\"poster\"", "</div>");
                ProfileUrl = Utils.getBetween(tempPagesource, "<a href=", ">").Replace("\"", "").Trim();
                if (!string.IsNullOrEmpty(ProfileUrl))
                {
                    string pagesourceProfildetails = HttpHelper.getHtmlfromUrl(new Uri(ProfileUrl));


                    #region Name
                    try
                    {
                        try
                        {
                            try
                            {
                                int StartIndex = pagesourceProfildetails.IndexOf("<title>");
                                string Start = pagesourceProfildetails.Substring(StartIndex).Replace("<title>", string.Empty);
                                int EndIndex = Start.IndexOf("| LinkedIn</title>");
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
                                strFamilyName = pagesourceProfildetails.Substring(pagesourceProfildetails.IndexOf("fmt__full_name\":"), (pagesourceProfildetails.IndexOf(",", pagesourceProfildetails.IndexOf("fmt__full_name\":")) - pagesourceProfildetails.IndexOf("fmt__full_name\":"))).Replace("fmt__full_name\":", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Trim();

                            }
                            catch { }
                        }

                        if (string.IsNullOrEmpty(strFamilyName))
                        {
                            try
                            {
                                strFamilyName = pagesourceProfildetails.Substring(pagesourceProfildetails.IndexOf("<span class=\"full-name\">"), (pagesourceProfildetails.IndexOf("</span><span></span></span></h1></div></div><div id=\"headline-container\" data-li-template=\"headline\">", pagesourceProfildetails.IndexOf("</span><span></span></span></h1></div></div><div id=\"headline-container\" data-li-template=\"headline\">")) - pagesourceProfildetails.IndexOf("<span class=\"full-name\">"))).Replace("<span class=\"full-name\">", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Trim();
                            }
                            catch
                            { }
                        }

                        if (string.IsNullOrEmpty(strFamilyName))
                        {
                            try
                            {
                                int StartIndex = pagesourceProfildetails.IndexOf("<span class=\"full-name\">");
                                string Start = pagesourceProfildetails.Substring(StartIndex).Replace("<span class=\"full-name\">", string.Empty);
                                int EndIndex = Start.IndexOf("</span>");
                                string End = Start.Substring(0, EndIndex).Replace("</span>", string.Empty);
                                strFamilyName = End.Trim();
                            }
                            catch
                            { }
                        }

                        if (string.IsNullOrEmpty(strFamilyName) && pagesourceProfildetails.Contains("<span class=\"full-name\""))
                        {
                            try
                            {
                                int StartIndex = pagesourceProfildetails.IndexOf("<span class=\"full-name\"");
                                string Start = pagesourceProfildetails.Substring(StartIndex).Replace("<span class=\"full-name\"", string.Empty);
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
                                int StartIndex = pagesourceProfildetails.IndexOf("<title>");
                                string Start = pagesourceProfildetails.Substring(StartIndex).Replace("</title>", string.Empty);
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
                        FirstName = NameArr[0];
                    }
                    catch { }
                    #endregion

                    #region LastName

                    try
                    {
                        LastName = NameArr[1];
                    }
                    catch { }

                    try
                    {
                        if (NameArr.Count() == 3)
                        {
                            try
                            {
                                LastName = NameArr[1] + " " + NameArr[2];
                            }
                            catch { }
                        }

                        
                    }
                    catch { }
                    #endregion

                    if (string.IsNullOrEmpty(FirstName)) FirstName = "--";
                    if (string.IsNullOrEmpty(LastName)) LastName = "--";
                    if (string.IsNullOrEmpty(ProfileUrl)) ProfileUrl = "--";
                    if (string.IsNullOrEmpty(Jobtitle)) Jobtitle = "--";
                    if (string.IsNullOrEmpty(Location)) Location = "--";
                    if (string.IsNullOrEmpty(Website)) Website = "--";
                    if (string.IsNullOrEmpty(specialites)) specialites = "--";
                    if (string.IsNullOrEmpty(Industry)) Industry = "--";
                    if (string.IsNullOrEmpty(Website)) Website = "--";


                    string LDS_FinalData = Url.Replace(",", ";") + "," + Jobtitle.Replace(",", ";") + "," + Location.Replace(",", ";") + "," + FirstName.Replace(",", ";") + "," + LastName.Replace(",", ";") + "," + ProfileUrl.Replace(",", ";") + "," + Website.Replace(",", ";") + "," + specialites.Replace(",", ";") + "," + Industry.Replace(",", ";");
                    AddingLinkedInDataToCSVFileCompanyEmployeeScraper(LDS_FinalData);
                }
                
            }
            catch (Exception ex)
            {

            }
            return isscraped;
        }


        
        public static void AddingLinkedInDataToCSVFileCompanyEmployeeScraper(string Data)
        {
            try
            {
                //string LinkedInAppData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "LinkedInJobScraper.csv");
                string LinkedInDeskTop = Globals.DesktopFolder + "\\LinkedInJobScraper.csv";

                
                if (!File.Exists(LinkedInDeskTop))
                {
                    string Header = "JobUrl" + "," + "JobTitle" + "," + "Location" + "," + "FirstName" + "," + "LastName" + "," + "ProfileUrl" + "," + "Website" + "," + "specialites" + "," + "Industry";
                    GlobusFileHelper.AppendStringToTextfileNewLine(Header, LinkedInDeskTop);
                }

                if (!string.IsNullOrEmpty(Data))
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine(Data, LinkedInDeskTop);
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        public List<string> GettingAllUrl(string PageSource)
        {
            List<string> lstGettingUrl = new List<string>();
            try
            {
                if (PageSource.Contains("&jobId="))
                {
                    string[] trkArr = Regex.Split(PageSource, "&jobId=");
                   
                    trkArr = trkArr.Skip(1).ToArray();
                    foreach (string item in trkArr)
                    {
                        if (item.Contains("&"))
                        {
                            string tempitem = Utils.getBetween(item,"","&");
                            tempitem = "https://www.linkedin.com/jobs2/view/" + tempitem;
                            lstGettingUrl.Add(tempitem);
                            lstGettingUrl = lstGettingUrl.Distinct().ToList();
                        }
                    }
                }
            }

            catch { }
            return lstGettingUrl;
        }




    }

}
