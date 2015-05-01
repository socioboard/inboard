using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Threading;
using System.IO;
using BaseLib;

namespace Groups
{
    public class CreateGroup
    {
        #region variable declaration
        public string accountUser = string.Empty;
        public string accountPass = string.Empty;
        public string proxyAddress = string.Empty;
        public string proxyPort = string.Empty;
        public string proxyUserName = string.Empty;
        public string proxyPassword = string.Empty;

        public string csrfToken = string.Empty;
        public string PostIamge = string.Empty;
        public string PostGrpName = string.Empty;
        public string PostGrpSummry = string.Empty;
        public string PostGrpDesc = string.Empty;
        public string PostGrpWebsite = string.Empty;
        public static Queue<string> Que_Message_Post = new Queue<string>();
        #endregion

        #region For Send Invitation
        public static List<string> LstGroupUrls
        {
            get;
            set;
        }
        #endregion

        #region CreateGroup
        public CreateGroup()
        {

        }
        #endregion

        #region CreateGroup
        public CreateGroup(string UserName, string Password, string ProxyAddress, string ProxyPort, string ProxyUserName, string ProxyPassword)
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

        #region createGroup
        public void StartCreateGroup(ref GlobusHttpHelper HttpHelper, int mindelay, int maxdelay)
        {
            try
            {
                 string UploadInfo = string.Empty;
                string status = string.Empty;
                string TempID = string.Empty;
                string image = string.Empty;
                string PostCreateGroup = string.Empty;
                string ResponseStatusMsg = string.Empty;
                string urlForNewGroupCreated = string.Empty;
                Log("[ " + DateTime.Now + " ] => [ Group Creation In Account :" + accountUser + " ]");
                try
                {
                    string PageSource1 = string.Empty;
                      for (int i = 0; i <= 5;i++)
                      {
                          PageSource1 = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/createGroup?displayCreate=&trk=anet_creategrp"));
                          if (!string.IsNullOrEmpty(PageSource1))
                          {
                              break;
                          }
                          Thread.Sleep(2 * 1000);
                      }

                    if (PageSource1.Contains("Confirm Your Email Address:"))
                    {
                        //
                        GlobusFileHelper.AppendStringToTextfileNewLine(accountUser + ":" + accountPass + ":" + proxyAddress + ":" + proxyPort + ":" + proxyUserName + ":" + proxyPassword, Globals.path_NotCreateGroup);
                        Log("[ " + DateTime.Now + " ] => [ Cannot Create Group , Confirm Your Email Address for Create group you must have at least one confirmed email address: " + accountUser + " ]");
                        return;
                    }

                    csrfToken = GetCsrfToken(PageSource1);
                    try
                    {
                        int StartIndex = PageSource1.IndexOf("name=\"upload_info\"");
                        string start = PageSource1.Substring(StartIndex).Replace("name=\"upload_info\"", "").Replace("type=\"hidden\"", "").Replace("value=\"", "");
                        int EndIndex = start.IndexOf("\"/>");
                        string end = start.Substring(0, EndIndex).Replace("value=\"", "").Replace(" ", "");
                        UploadInfo = end;
                    }
                    catch (Exception ex)
                    {

                    }

                    NameValueCollection nvc = new NameValueCollection();
                    nvc.Add("csrfToken", csrfToken);
                    nvc.Add("upload_info", UploadInfo);
                    nvc.Add("callback", "logo.processResponseLargeLogo");
                    nvc.Add("isCSMode", "false");
                    nvc.Add("_method", "PUT");

                    string[] array = Regex.Split(PostIamge, ".");
                    if (!string.IsNullOrEmpty(PostIamge))
                    {
                        Log("[ " + DateTime.Now + " ] => [ Uploading Logo ]");
                    }
                    image = array[array.Length - 1];
                    string result = HttpHelper.HttpUploadFileBackground("http://www.linkedin.com/mupld/upload", PostIamge, "file", "image/" + image, nvc, true, ref status);
                    if (result.Contains("{\"status\":\"ERROR\""))
                    {
                        Log("[ " + DateTime.Now + " ] => [ Error In Uploading Logo ! ]");
                        return;
                    }
                    try
                    {
                        int Startindex = result.IndexOf("value\":");
                        string start = result.Substring(Startindex);
                        int EndIndex = start.IndexOf("\",");
                        string End = start.Substring(0, EndIndex).Replace("value\":", "").Replace("\"", "");
                        TempID = End;
                    }
                    catch (Exception ex)
                    {

                    }
                    string CSVHeader = "UserName" + "," + "GroupName" + "," +"CreatedGroupUrl";
                    string GetRequest = HttpHelper.getHtmlfromUrl1(new Uri("http://media03.linkedin.com/media/" + TempID));
                    TempID = Uri.EscapeDataString(TempID.Replace("\\", ""));

                    if (SearchCriteria.CreateGroupStatus == "Member")
                    {
                        Log("[ " + DateTime.Now + " ] => [ Creating Member Group ]");
                        PostCreateGroup = "csrfToken=" + Uri.EscapeDataString(csrfToken) + "&acceptLogoTerms=acceptLogoTerms&groupName=" + PostGrpName + "&groupCategory=" + SearchCriteria.GroupType + "&otherGroupCategory=&shortDesc=" + PostGrpSummry + "&longDesc=" + PostGrpDesc + "&homeSite=" + PostGrpWebsite + "&groupEmail=" + Uri.EscapeDataString(accountUser) + "&groupInDirectory-open=groupInDirectory-open&logoInProfiles-open=logoInProfiles-open&membersSendInvites-open=membersSendInvites-open&access=request&groupInDirectory-request=groupInDirectory-request&logoInProfiles-request=logoInProfiles-request&emailDomains=&language=" + SearchCriteria.GroupLang + "&countryCode=&postalCode=&acceptContract=acceptContract&create=Create+a+Members-Only+Group&gid=&largeLogoTempID=" + TempID + "&discVisibility=false&tetherAccountID=&facebookTetherID=&uncroppedHeroImageID=&croppedHeroImageID=&heroImageCropParams=";
                        ResponseStatusMsg = HttpHelper.postFormDataRef(new Uri("http://www.linkedin.com/createGroup"), PostCreateGroup, "http://www.linkedin.com/createGroup", "", "");
                        
                        Thread.Sleep(2000);

                        if (ResponseStatusMsg.Contains("Please choose a different group name.") || ResponseStatusMsg.Contains("Sorry this group name is not available. Please choose a different one."))
                        {
                            PostGrpName = PostGrpName + "New";
                            string CSV_Content = accountUser + "," + PostGrpName;
                            PostCreateGroup = "csrfToken=" + Uri.EscapeDataString(csrfToken) + "&acceptLogoTerms=acceptLogoTerms&groupName=" + PostGrpName + "&groupCategory=" + SearchCriteria.GroupType + "&otherGroupCategory=&shortDesc=" + PostGrpSummry + "&longDesc=" + PostGrpDesc + "&homeSite=" + PostGrpWebsite + "&groupEmail=" + Uri.EscapeDataString(accountUser) + "&groupInDirectory-open=groupInDirectory-open&logoInProfiles-open=logoInProfiles-open&membersSendInvites-open=membersSendInvites-open&access=request&groupInDirectory-request=groupInDirectory-request&logoInProfiles-request=logoInProfiles-request&emailDomains=&language=" + SearchCriteria.GroupLang + "&countryCode=&postalCode=&acceptContract=acceptContract&create=Create+a+Members-Only+Group&gid=&largeLogoTempID=" + TempID + "&discVisibility=false&tetherAccountID=&facebookTetherID=&uncroppedHeroImageID=&croppedHeroImageID=&heroImageCropParams=";
                            ResponseStatusMsg = HttpHelper.postFormDataRef(new Uri("http://www.linkedin.com/createGroup"), PostCreateGroup, "http://www.linkedin.com/createGroup", "", "");
                            Thread.Sleep(2000);
                            if (ResponseStatusMsg.Contains("Send Invitation"))
                            {
                                Log("[ " + DateTime.Now + " ] => [ New Member-Only-Group Create : " + PostGrpName + " Place of " + PostGrpName + " has Successfully Created on: " + accountUser + " ]");
                                CSV_Content = accountUser + "," + PostGrpName;
                                CSVUtilities.ExportDataCSVFile(CSVHeader, CSV_Content, Globals.path_CreateGroups);
                            }
                        }
                        else if (ResponseStatusMsg.Contains("Send Invitation"))
                        {
                            
                            Log("[ " + DateTime.Now + " ] => [ Member-Only-Group: " + PostGrpName + " has Successfully Created on: " + accountUser + " has Successfully Created on: " + urlForNewGroupCreated + " ]");
                            string CSV_Content = accountUser + "," + PostGrpName + "," + urlForNewGroupCreated;
                            CSVUtilities.ExportDataCSVFile(CSVHeader, CSV_Content, Globals.path_CreateGroups);
                            urlForNewGroupCreated = string.Empty;
                        }
                        else if (ResponseStatusMsg.Contains("You must confirm your primary email address before creating a group."))
                        {
                            Log("[ " + DateTime.Now + " ] => [ User: " + accountUser + "  must confirm his/her primary email address before creating a group. ]");
                            GlobusFileHelper.AppendStringToTextfileNewLine(accountUser + ":" + accountPass + ":" + proxyAddress + ":" + proxyPort + ":" + proxyUserName + ":" + proxyPassword, Globals.path_NotCreatedGroups);
                            return;
                        }
                        else if (ResponseStatusMsg.Contains("Sorry you cannot create more groups on LinkedIn because you already own too many groups."))
                        {
                            Log("[ " + DateTime.Now + " ] => [ Sorry User: " + accountUser + " cannot create more groups on LinkedIn because his/her already own too many groups. ]");
                            GlobusFileHelper.AppendStringToTextfileNewLine(accountUser + ":" + accountPass + ":" + proxyAddress + ":" + proxyPort + ":" + proxyUserName + ":" + proxyPassword, Globals.path_NotCreatedGroups);
                            return;
                        }
                        else if (ResponseStatusMsg.Contains("You cannot create new groups because you've exceeded the maximum number of group memberships."))
                        {
                            Log("[ " + DateTime.Now + " ] => [ Sorry User: " + accountUser + " You cannot create new groups because you've exceeded the maximum number of group memberships. ]");
                            GlobusFileHelper.AppendStringToTextfileNewLine("Sorry User: " + accountUser + " You cannot create new groups because you've exceeded the maximum number of group memberships.", Globals.path_NotCreatedGroups);
                            return;
                        }
                        else if (ResponseStatusMsg.Contains("Please enter a valid URL."))
                        {
                            Log("[ " + DateTime.Now + " ] => [ Sorry User: " + accountUser + " You cannot create new groups because your Web Site URL is not Valid. ]");
                            GlobusFileHelper.AppendStringToTextfileNewLine(accountUser + ":" + accountPass + ":" + proxyAddress + ":" + proxyPort + ":" + proxyUserName + ":" + proxyPassword, Globals.path_NotCreatedGroups);
                            return;
                        }
                        else
                        {
                            Log("[ " + DateTime.Now + " ] => [ Group Not Created With Username >>> " + accountUser + " ]");
                            GlobusFileHelper.AppendStringToTextfileNewLine(accountUser + ":" + accountPass + ":" + proxyAddress + ":" + proxyPort + ":" + proxyUserName + ":" + proxyPassword, Globals.path_NotCreatedGroups);
                        }
                    }
                    else if (SearchCriteria.CreateGroupStatus == "Open")
                    {
                        try
                        {
                            Log("[ " + DateTime.Now + " ] => [ Creating Open Group ]");
                            PostCreateGroup = "csrfToken=" + Uri.EscapeDataString(csrfToken) + "&acceptLogoTerms=acceptLogoTerms&groupName=" + PostGrpName + "&groupCategory=" + SearchCriteria.GroupType + "&otherGroupCategory=&shortDesc=" + PostGrpSummry + "&longDesc=" + PostGrpDesc + "&homeSite=" + PostGrpWebsite + "&groupEmail=" + Uri.EscapeDataString(accountUser) + "&access=open&groupInDirectory-open=groupInDirectory-open&logoInProfiles-open=logoInProfiles-open&membersSendInvites-open=membersSendInvites-open&groupInDirectory-request=groupInDirectory-request&logoInProfiles-request=logoInProfiles-request&emailDomains=&language=" + SearchCriteria.GroupLang + "&countryCode=&postalCode=&acceptContract=acceptContract&create=Create+an+Open+Group&gid=&largeLogoTempID=" + TempID + "&discVisibility=true&tetherAccountID=&facebookTetherID=&uncroppedHeroImageID=&croppedHeroImageID=&heroImageCropParams=";
                            ResponseStatusMsg = HttpHelper.postFormDataRef(new Uri("http://www.linkedin.com/createGroup"), PostCreateGroup, "http://www.linkedin.com/createGroup?displayCreate=&displayCreate=&trk=hb_side_crgrp", "", "");

                            if (ResponseStatusMsg.Contains("Please choose a different group name.") || ResponseStatusMsg.Contains("Sorry this group name is not available. Please choose a different one."))
                            {
                                //Log("[ " + DateTime.Now + " ] => [ No more unique data avaialble for creating groups -- Please insert data ]");
                                Log("[ " + DateTime.Now + " ] => [ No more unique data available for creating groups -- Please insert data ]");
                                return;
                                PostGrpName = PostGrpName + " New";
                                PostCreateGroup = "csrfToken=" + Uri.EscapeDataString(csrfToken) + "&acceptLogoTerms=acceptLogoTerms&groupName=" + PostGrpName + "&groupCategory=" + SearchCriteria.GroupType + "&otherGroupCategory=&shortDesc=" + PostGrpSummry + "&longDesc=" + PostGrpDesc + "&homeSite=" + PostGrpWebsite + "&groupEmail=" + Uri.EscapeDataString(accountUser) + "&access=open&groupInDirectory-open=groupInDirectory-open&logoInProfiles-open=logoInProfiles-open&membersSendInvites-open=membersSendInvites-open&groupInDirectory-request=groupInDirectory-request&logoInProfiles-request=logoInProfiles-request&emailDomains=&language=" + SearchCriteria.GroupLang + "&countryCode=&postalCode=&acceptContract=acceptContract&create=Create+an+Open+Group&gid=&largeLogoTempID=" + TempID + "&discVisibility=true&tetherAccountID=&facebookTetherID=&uncroppedHeroImageID=&croppedHeroImageID=&heroImageCropParams=";
                                ResponseStatusMsg = HttpHelper.postFormDataRef(new Uri("http://www.linkedin.com/createGroup"), PostCreateGroup, "http://www.linkedin.com/createGroup?displayCreate=&displayCreate=&trk=hb_side_crgrp", "", "");

                                Thread.Sleep(2000);

                                if (ResponseStatusMsg.Contains("Send Invitation"))
                                {
                                    Log("[ " + DateTime.Now + " ] => [ New Open-Group Create : " + PostGrpName + " Place of " + PostGrpName + " has Successfully Created on: " + accountUser + " ]");
                                    string CSV_Content = accountUser + "," + PostGrpName;
                                    CSVUtilities.ExportDataCSVFile(CSVHeader, CSV_Content, Globals.path_CreateGroups);
                                }
                                else
                                {
                                    Log("[ " + DateTime.Now + " ] => [ New Open-Group Not Created : " + PostGrpName + " Place of " + PostGrpName + " has Successfully Created on: " + accountUser + " ]");
                                }
                            }
                            else if (ResponseStatusMsg.Contains("Send Invitation"))
                            {
                                string End = string.Empty;
                                string[] arrForGid = Regex.Split(ResponseStatusMsg, "gid");
                                //arrForGid.Skip(1).ToArray();
                                
                                foreach (string  item in arrForGid)
                                {
                                    if (!item.Contains("<!DOCTYPE html"))
                                    {
                                        int Startindex = item.IndexOf("=");
                                        string start = item.Substring(Startindex);
                                        int EndIndex = start.IndexOf("&");
                                        End = start.Substring(0, EndIndex).Replace("=", "").Replace("\"", "").Replace("&amp", "");
                                        break;
                                    }
                                }
                                urlForNewGroupCreated = "http://www.linkedin.com/groups?home=&gid=" + End;
                                Log("[ " + DateTime.Now + " ] => [ Open-Group: " + PostGrpName + " has  Successfully Created on: " + accountUser + " ]");
                                string CSV_Content = accountUser + "," + PostGrpName+ "," +urlForNewGroupCreated;
                                CSVUtilities.ExportDataCSVFile(CSVHeader, CSV_Content, Globals.path_CreateGroups);
                            }
                            else if (ResponseStatusMsg.Contains("You must confirm your primary email address before creating a group."))
                            {
                                Log("[ " + DateTime.Now + " ] => [ User: " + accountUser + "  must confirm his/her primary email address before creating a group.  ]");
                                GlobusFileHelper.AppendStringToTextfileNewLine(accountUser + ":" + accountPass + ":" + proxyAddress + ":" + proxyPort + ":" + proxyUserName + ":" + proxyPassword, Globals.path_NotCreatedGroups);
                                return;
                            }
                            else if (ResponseStatusMsg.Contains("Sorry you cannot create more groups on LinkedIn because you already own too many groups."))
                            {
                                Log("[ " + DateTime.Now + " ] => [ Sorry User: " + accountUser + " cannot create more groups on LinkedIn because his/her already own too many groups. ]");
                                GlobusFileHelper.AppendStringToTextfileNewLine(accountUser + ":" + accountPass + ":" + proxyAddress + ":" + proxyPort + ":" + proxyUserName + ":" + proxyPassword, Globals.path_NotCreatedGroups);
                            }
                            else if (ResponseStatusMsg.Contains("You cannot create new groups because you've exceeded the maximum number of group memberships."))
                            {
                                Log("[ " + DateTime.Now + " ] => [ Sorry User: " + accountUser + " You cannot create new groups because you've exceeded the maximum number of group memberships. ]");
                                GlobusFileHelper.AppendStringToTextfileNewLine(accountUser + ":" + accountPass + ":" + proxyAddress + ":" + proxyPort + ":" + proxyUserName + ":" + proxyPassword, Globals.path_NotCreatedGroups);
                                //return;
                            }
                            else if (ResponseStatusMsg.Contains("Please enter a valid URL."))
                            {
                                Log("[ " + DateTime.Now + " ] => [ Sorry User: " + accountUser + " You cannot create new groups because your Web Site URL is not Valid. ]");
                                GlobusFileHelper.AppendStringToTextfileNewLine(accountUser + ":" + accountPass + ":" + proxyAddress + ":" + proxyPort + ":" + proxyUserName + ":" + proxyPassword, Globals.path_NotCreatedGroups);
                                //return;
                            }
                            else if (ResponseStatusMsg.Contains("Sorry, but group management is currently unavailable. Please try again later"))
                            {
                                Log("[ " + DateTime.Now + " ] => [ Sorry User: " + accountUser + " Sorry, but group management is currently unavailable. Please try again later. ]");
                                GlobusFileHelper.AppendStringToTextfileNewLine(accountUser + ":" + accountPass + ":" + proxyAddress + ":" + proxyPort + ":" + proxyUserName + ":" + proxyPassword, Globals.path_NotCreatedGroups);
                                //return;
                            }
                            else
                            {
                                GlobusFileHelper.AppendStringToTextfileNewLine(accountUser + ":" + accountPass + ":" + proxyAddress + ":" + proxyPort + ":" + proxyUserName + ":" + proxyPassword, Globals.path_NotCreatedGroups);
                                Log("[ " + DateTime.Now + " ] => [ Group Not Created With Username >>> " + accountUser + " ]");
                            }
                        }
                        catch (Exception ex)
                        {

                        }
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
            catch
            {
            }
        }
        #endregion

        #region GetCsrfToken
        public string GetCsrfToken(string PageSource)
        {
            string ajax = string.Empty;
            try
            {
                int StartIndex = PageSource.IndexOf("name=\"csrfToken\"");
                string start = PageSource.Substring(StartIndex).Replace("name=\"csrfToken\"", "").Replace("value=\"", "");
                int EndIndex = start.IndexOf("\"");
                string end = start.Substring(0, EndIndex).Replace("value=\"", "").Replace(" ", "");
                ajax = end;
            }
            catch (Exception ex)
            {

            }
            return ajax;
        }
        #endregion

        #region StartSendInvitation
        public void StartSendInvitation(ref GlobusHttpHelper HttpHelper)
        {
            try
            {
                

                Log("[ " + DateTime.Now + " ] => [ Starting Send Invitation With Account :" + accountUser + " ]");
                try
                {

                    foreach (string item in LstGroupUrls)
                    {
                        try
                        {
                            if (item.Contains("gid="))
                            {
                                string subGroupUrl = item.Substring(item.IndexOf("gid="), item.Length - item.IndexOf("gid="));

                                string groupId = string.Empty;

                                if (subGroupUrl.Contains("&"))
                                {
                                    groupId = item.Substring(item.IndexOf("gid="), item.IndexOf("&", item.IndexOf("gid=")) - item.IndexOf("gid=")).Replace("gid=", string.Empty).Replace("\"", string.Empty).Trim();
                                }
                                else
                                {
                                    groupId = item.Substring(item.IndexOf("gid="), item.Length - item.IndexOf("gid=")).Replace("gid=", string.Empty).Replace("\"", string.Empty).Trim();
                                }

                                if (!string.IsNullOrEmpty(groupId))
                                {

                                    try
                                    {
                                        SendInvitation(ref HttpHelper, item, groupId);
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine("Error >>> " + ex.StackTrace);
                                    }
                                }
                            }
                            else
                            {
                                if (!File.Exists(Globals.path_InviationUrlIncorrect))
                                {
                                    GlobusFileHelper.AppendStringToTextfileNewLine("Account, URL", Globals.path_InviationUrlIncorrect);
                                }
                                Log("[ " + DateTime.Now + " ] => [ Group URL Is Invalid With URL : " + item + " ]");
                                GlobusFileHelper.AppendStringToTextfileNewLine(accountUser + "," + item, Globals.path_InviationUrlIncorrect);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error >>> " + ex.StackTrace);
                        }
                    }

                    Log("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED With Username : " + accountUser + " ]");
                    Log("-----------------------------------------------------------------------------------------------------------------------------------");

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error >>> " + ex.StackTrace);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error >>> " + ex.StackTrace);
            }
        }
        #endregion

        #region SendInvitation
        public void SendInvitation(ref GlobusHttpHelper HttpHelper, string grpURL, string grpId)
        {
            try
            {
                string FString = string.Empty;
                string Nstring = string.Empty;
                string connId = string.Empty;
                string FullName = string.Empty;
                string ToMsg = string.Empty;
                string ContactName = string.Empty;

                string URL = "http://www.linkedin.com/manageGroup?dispAddMbrs=&gid=" + grpId + "&invtActn=im-invite&cntactSrc=cs-connections";

                string grpPageSource = HttpHelper.getHtmlfromUrl1(new Uri(URL));

                //string csrfToken = GetCsrfToken(grpPageSource);
                try
                {
                    int startindex = grpPageSource.IndexOf("csrfToken=");
                    string start = grpPageSource.Substring(startindex).Replace("csrfToken=",string.Empty);
                    int endindex = start.IndexOf("\"");
                    string end = start.Substring(0, endindex).Replace("\"", string.Empty);
                    csrfToken = end.Trim();
                }
                catch
                { }
                ClsLinkedinMain clsLinkedinMain = new ClsLinkedinMain();
                Dictionary<string, string> dTotalFriends = clsLinkedinMain.PostAddMembers(ref HttpHelper, accountUser);

                // To manage the code for friends in which invitation already sent

                // SucessfullySendInvitationToFriend();

                //int counter = 1;

                foreach (KeyValuePair<string, string> itemChecked in dTotalFriends)
                {
                    try
                    {
                        //if (!IsAllAccounts)
                        //{
                        //    if (counter > 50)
                        //    {
                        //        break;
                        //    }
                        //}

                        //counter++;

                        string FName = string.Empty;
                        string Lname = string.Empty;

                        FName = itemChecked.Value.Split(' ')[0];
                        Lname = itemChecked.Value.Split(' ')[1];

                        FullName = FName + " " + Lname;
                        try
                        {
                            ContactName = ContactName + "  :  " + FullName;
                        }
                        catch { }
                        if (ToMsg == string.Empty)
                        {
                            ToMsg += FullName;
                        }
                        else
                        {
                            ToMsg += ";" + FullName;
                        }

                        Log("[ " + DateTime.Now + " ] => [ Adding Contact : " + FullName + " ]");


                        string ToCd = itemChecked.Key;

                        if (ToCd.Contains(":"))
                        {
                            try
                            {
                                ToCd = ToCd.Substring(ToCd.IndexOf(":"), ToCd.Length - ToCd.IndexOf(":")).Replace(":", string.Empty).Trim();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Error >>> " + ex.StackTrace);
                            }

                        }

                        List<string> AddAllString = new List<string>();

                        if (FString == string.Empty)
                        {
                            string CompString = "{" + "\"" + "_" + ToCd.Trim() + "\"" + ":" + "{" + "\"" + "memberId" + "\"" + ":" + "\"" + ToCd.Trim() + "\"" + "," + "\"" + "firstName" + "\"" + ":" + "\"" + FName + "\"" + "," + "\"" + "lastName" + "\"" + ":" + "\"" + Lname + "\"" + "}";
                            FString = CompString;
                        }
                        else
                        {
                            string CompString = "\"" + "_" + ToCd.Trim() + "\"" + ":" + "{" + "\"" + "memberId" + "\"" + ":" + "\"" + ToCd.Trim() + "\"" + "," + "\"" + "firstName" + "\"" + ":" + "\"" + FName + "\"" + "," + "\"" + "lastName" + "\"" + ":" + "\"" + Lname + "\"" + "}";
                            FString = CompString;
                        }

                        if (Nstring == string.Empty)
                        {
                            Nstring = FString;
                            connId = ToCd;
                        }
                        else
                        {
                            Nstring += "," + FString;
                            connId += " " + ToCd;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("ex.Message >>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + "  ex.Stack Trace >>> " + ex.StackTrace);
                    }

                    //}
                    Nstring += "}";


                    string postData = "csrfToken=" + csrfToken + "&emailRecipients=&subAddMbrs=Send+Invitations&gid=" + grpId + "&invtActn=im-invite&cntactSrc=cs-connections&remIntives=999&connectionIds=" + connId + "&connectionNames=" + Uri.EscapeUriString(Nstring) + "&contactIDs=&newGroup=false";

                    string response = HttpHelper.postFormData(new Uri("http://www.linkedin.com/manageGroup"), postData);

                    //if (response.Contains("You have successfully sent invitations to this group") || response.Contains("Upgrade Your Account"))
                        if (response.Contains("You have successfully sent invitations to this group"))
                    {
                        Log("[ " + DateTime.Now + " ] => [ You have successfully sent invitations to Group : " + grpURL + " With Username : " + accountUser + "Invite user : " + FullName + " ]");
                        string CSVHeader = "GroupUrl" + "," + "UserName" + "," + "Invite User" ;
                        string CSVContent = grpURL + "," + accountUser + "," + FullName;
                        CSVUtilities.ExportDataCSVFile(CSVHeader, CSVContent, Globals.path_SentInvitationGroup);
                       // GlobusFileHelper.AppendStringToTextfileNewLine(content, Globals.path_SentInvitationGroup);

                    }
                    else
                    {
                        Log("[ " + DateTime.Now + " ] => [ Couldn't Send Invitation With Username : " + accountUser + " ]");
                        string CSVHeader = "GroupUrl" + "," + "UserName";
                        string CSVContent = grpURL + "," + accountUser;
                        CSVUtilities.ExportDataCSVFile(CSVHeader, CSVContent, Globals.path_NotSentInvitationGroup);
                        //GlobusFileHelper.AppendStringToTextfileNewLine(content, Globals.path_NotSentInvitationGroup);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error >>> " + ex.StackTrace);
            }
        }
        #endregion

     
    }
}
