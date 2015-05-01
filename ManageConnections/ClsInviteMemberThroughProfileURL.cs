using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseLib;
using System.Threading;
using System.Text.RegularExpressions;

namespace ManageConnections
{
    public class ClsInviteMemberThroughProfileURL
    {
        #region variable declaration
        string _UserName = string.Empty;
        string _Password = string.Empty;
        string _ProxyAddress = string.Empty;
        string _ProxyPort = string.Empty;
        string _ProxyUserName = string.Empty;
        string _ProxyPassword = string.Empty;
        public static bool spintaxsearch = false;
        public static List<string> Listgreetmsg = new List<string>();

        public Events acceptInvitationsLogEvents = new Events(); 
        #endregion

        #region ClsInviteMemberThroughProfileURL
        public ClsInviteMemberThroughProfileURL(string username, string password, string proxyaddress, string proxyport, string proxyusername, string proxypassword)
        {
            _UserName = username;
            _Password = password;
            _ProxyAddress = proxyaddress;
            _ProxyPort = proxyport;
            _ProxyUserName = proxyusername;
            _ProxyPassword = proxypassword;

        } 
        #endregion

        #region PersonalNote
        public static string PersonalNote
        {
            get;
            set;
        } 
        #endregion

        #region HowDoYouKnowThisPerson
        public static string HowDoYouKnowThisPerson
        {
            get;
            set;
        } 
        #endregion

        #region LstProfileURL
        public static List<string> LstProfileURL
        {
            get;
            set;
        } 
        #endregion

        #region MinDelay
        public static int MinDelay
        {
            get;
            set;
        } 
        #endregion

        #region MaxDelay
        public static int MaxDelay
        {
            get;
            set;
        } 
        #endregion

        #region StartSendInvitations
        public void StartSendInvitations(ref GlobusHttpHelper httpHelper)
        {
            try
            {
                foreach (var item in LstProfileURL)
                {
                    Connect(ref httpHelper, item);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error >>> " + ex.StackTrace);
            }

            Log("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED With Username >>> " + _UserName + " ]");
        } 
        #endregion

        #region StartSendInvitations_DivideUrlsAccordingAccount
        public void StartSendInvitations_DivideUrlsAccordingAccount(ref GlobusHttpHelper httpHelper, List<string> lstProfileURL)
        {
            try
            {
                foreach (var item in lstProfileURL)
                {
                    try
                    {
                        Connect(ref httpHelper, item);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error >>> " + ex.StackTrace);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error >>> " + ex.StackTrace);
                Log("[ " + DateTime.Now + " ] => [ Error >>> ex.Message >>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + " ex.StackTrace >>> " + ex.StackTrace + " ]");

            }

            Log("[ " + DateTime.Now + " ] => [ Process Completed With Username >>> " + _UserName + " ]");
        } 
        #endregion

        #region Connect
        private void Connect(ref GlobusHttpHelper httpHelper, string profileURL)
        {
            try
            {
                Log("[ " + DateTime.Now + " ] => [ Start Sending Connect With Username >>> " + _UserName + " ]");


                string key = string.Empty;
                string firstName_Guest = string.Empty;
                string lastName_Guest = string.Empty;
                string authToken = string.Empty;
                string goback = string.Empty;
                string fullName = string.Empty;

                string pageSource = httpHelper.getHtmlfromUrl1(new Uri(profileURL));

                //if (!pageSource.Contains("\"i18n__Connect\":\"Connect\",\"deferImg\":true"))
                //{
                //    Log("[ " + DateTime.Now + " ] => [ You can not send invitation to profile url >>> " + profileURL + " with username >>> " + _UserName + " ]");
                //    return;

                //}

                if (string.IsNullOrEmpty(pageSource))
                {
                    Thread.Sleep(1 * 1000);

                    pageSource = httpHelper.getHtmlfromUrl1(new Uri(profileURL));
                }

                if (string.IsNullOrEmpty(pageSource))
                {
                    Log("[ " + DateTime.Now + " ] => [ Page Source Is Null ! ]");

                    Log("[ " + DateTime.Now + " ] => [ Process Completed With Username >>> " + _UserName + " ]");

                    return;
                }

                if (pageSource.Contains("memberId:"))
                {
                    key = pageSource.Substring(pageSource.IndexOf("memberId:"), pageSource.IndexOf("</script>", pageSource.IndexOf("memberId:")) - pageSource.IndexOf("memberId:")).Replace("memberId:", string.Empty).Replace("\"", string.Empty).Replace("};", string.Empty).Trim();
                }

                if (pageSource.Contains("snapshotID=&authToken="))
                {
                    authToken = pageSource.Substring(pageSource.IndexOf("snapshotID=&authToken=") + 22, pageSource.IndexOf("&", (pageSource.IndexOf("snapshotID=&authToken=") + 22)) - (pageSource.IndexOf("snapshotID=&authToken=") + 22)).Replace("snapshotID=&authToken=", string.Empty).Replace("\"", string.Empty).Trim();
                }

                if (String.IsNullOrEmpty(authToken) && pageSource.Contains("authToken="))
                {
                    authToken = pageSource.Substring(pageSource.IndexOf("authToken="), pageSource.IndexOf(",", (pageSource.IndexOf("authToken="))) - (pageSource.IndexOf("authToken="))).Replace("authToken=", string.Empty).Replace("\"", string.Empty).Trim();

                    if (authToken.Contains("authType"))
                    {
                        try
                        {
                            authToken = authToken.Split('&')[0];
                        }
                        catch { }
                       
                    }
                    if (authToken.Contains("&"))
                    {
                        try
                        {
                            authToken = authToken.Split('&')[0];
                        }
                        catch { }
                    }
                }

                if (pageSource.Contains("goback="))
                {
                    goback = pageSource.Substring(pageSource.IndexOf("goback="), pageSource.IndexOf("\"", pageSource.IndexOf("goback=")) - pageSource.IndexOf("goback=")).Replace("goback=", string.Empty).Replace("\"", string.Empty).Trim();
                }
                if (string.IsNullOrEmpty(goback))
                {
                    string[] gobck = Regex.Split(pageSource, "&goback=");
                    int startindex = gobck[2].IndexOf("");
                    string start = gobck[2].Substring(startindex);
                    int endindex = start.IndexOf(",");
                    string end = start.Substring(0, endindex).Replace("\"",string.Empty);
                    goback = end.Trim();
                }


                if (pageSource.Contains("class=\"full-name\""))
                {
                    fullName = pageSource.Substring(pageSource.IndexOf("class=\"full-name\""), pageSource.IndexOf("<", pageSource.IndexOf("class=\"full-name\"")) - pageSource.IndexOf("class=\"full-name\"")).Replace("class=\"full-name\"", string.Empty).Replace("\"", string.Empty).Replace("dir=auto>", string.Empty).Trim();
                }

                if (string.IsNullOrEmpty(fullName))
                {
                    try
                    {
                        int startindex = pageSource.IndexOf("<span class=\"full-name\">");
                        string start = pageSource.Substring(startindex).Replace("<span class=\"full-name\">", string.Empty);
                        int endindex = start.IndexOf("</span>");
                        string end = start.Substring(0, endindex).Replace("</span>", string.Empty);
                        fullName = end.Trim();
                    }
                    catch
                    { }
                }
                if (fullName.Contains(" "))
                {
                    firstName_Guest = fullName.Substring(0, fullName.LastIndexOf(" ")).Replace("&quot;", "\"").Trim();

                    lastName_Guest = fullName.Substring(fullName.LastIndexOf(" "), fullName.Length - fullName.LastIndexOf(" ")).Replace("&quot;", "\"").Trim();

                }

                string pageSource1 = httpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/people/invite?from=profile&key=" + key + "&firstName=" + firstName_Guest + "&lastName=" + lastName_Guest + "&authToken=" + authToken + "&authType=name&goback=" + goback + "&trk=prof-0-sb-connect-button"));

                if (string.IsNullOrEmpty(pageSource1))
                {
                    Thread.Sleep(1 * 1000);

                    pageSource1 = httpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/people/invite?from=profile&key=" + key + "&firstName=" + firstName_Guest + "&lastName=" + lastName_Guest + "&authToken=" + authToken + "&authType=name&goback=" + goback + "&trk=prof-0-sb-connect-button"));
                }

                if (string.IsNullOrEmpty(pageSource1))
                {
                    Log("[ " + DateTime.Now + " ] => [ Page Source Is Null ! ]");

                    Log("[ " + DateTime.Now + " ] => [ Process Completed With Username >>> " + _UserName + " ]");

                    return;
                }
                if (!pageSource1.Contains("emailAddress-invitee-invitation"))
                {
                    SendInvitation(ref httpHelper, pageSource1, key, authToken, goback, firstName_Guest, lastName_Guest);
                }

                if (pageSource1.Contains("emailAddress-invitee-invitation"))
                {
                    Log("[ " + DateTime.Now + " ] => [ Invitation already sent to " + firstName_Guest + lastName_Guest + " from >>> " + _UserName + " ]");
                    Log("[ " + DateTime.Now + " ] => [ -----------------------------------------------------------------------------------------------]");
                }
            }
            catch (Exception ex)
            {
                //Log("[ " + DateTime.Now + " ] => [ Error >>> ex.Message >>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + " ex.StackTrace >>> " + ex.StackTrace + " ]");
            }
        } 
        #endregion

        #region SendInvitation
        private void SendInvitation(ref GlobusHttpHelper httpHelper, string pageSource, string key1, string authToken1, string goback1, string firstName_Guest1, string lastName_Guest1)
        {
            try
            {
                Log("[ " + DateTime.Now + " ] => [ Start Sending Invitation With Username >>> " + _UserName + " ]");

                string reason = string.Empty;
                string existingPositionIC = string.Empty;
                string greeting = string.Empty;
                string key = string.Empty;
                string firstName_Guest = string.Empty;
                string lastName_Guest = string.Empty;
                string authToken = string.Empty;
                string subject = string.Empty;
                string defaultText = string.Empty;
                string csrfToken = string.Empty;
                string sourceAlias = string.Empty;
                string goback = string.Empty;
                string userName = string.Empty;
                string userFirstName = string.Empty;
                string userLastName = string.Empty;
                string resultForUserDetails = FindTheUserName(pageSource);

                try
                {
                    resultForUserDetails = resultForUserDetails.Substring(resultForUserDetails.IndexOf("alt="), resultForUserDetails.IndexOf("height") - resultForUserDetails.IndexOf("alt=")).Replace("alt=", string.Empty).Replace("/", string.Empty).Trim();
                    userFirstName = resultForUserDetails.Split(' ')[0].Replace("\"", string.Empty).Replace(",", string.Empty);
                    userLastName = resultForUserDetails.Split(' ')[1].Replace("\"", string.Empty).Replace(",", string.Empty);
                }
                catch { }

                firstName_Guest = GetValue(pageSource, "firstName");

                if (string.IsNullOrEmpty(firstName_Guest))
                {
                    firstName_Guest = firstName_Guest1;
                }

                lastName_Guest = GetValue(pageSource, "lastName");

                if (string.IsNullOrEmpty(lastName_Guest))
                {
                    lastName_Guest = lastName_Guest1;
                }

                reason = GetValue(pageSource, "reason");

                existingPositionIC = GetValue(pageSource, "existingPositionIC");
                firstName_Guest = firstName_Guest.Replace("&quot;", "\"");

                greeting = PersonalNote.Replace("<FIRSTNAME>", firstName_Guest).Replace("<PROFILEFIRSTNAME>", userFirstName).Trim();
                ClsInviteMemberThroughProfileURL.Listgreetmsg = SpinnedListGenerator.GetSpinnedList(new List<string> { greeting });
                string messagebody = string.Empty;
                messagebody = greeting;

                if (spintaxsearch)
                {
                    try
                    {
                        messagebody = Listgreetmsg[RandomNumberGenerator.GenerateRandom(0, Listgreetmsg.Count - 1)];
                    }
                    catch
                    { }

                }

                key = GetValue(pageSource, "key");

                if (string.IsNullOrEmpty(key))
                {
                    key = key1;
                }

                authToken = GetValue(pageSource, "authToken");

                if (string.IsNullOrEmpty(authToken))
                {
                    authToken = authToken1;
                }
                
                subject = GetValue(pageSource, "subject");

                defaultText = GetValue(pageSource, "defaultText");

                csrfToken = GetValue(pageSource, "csrfToken");

                sourceAlias = GetValue(pageSource, "sourceAlias");

                goback = GetValue(pageSource, "goback").Replace("/", "%2F");

                if (string.IsNullOrEmpty(goback))
                {
                    goback = goback1;
                }
                
                string postData = "existingPositionIC=&companyName.0=&titleIC.0=&startYearIC.0=&endYearIC.0=&schoolText=&schoolID=&existingPositionIB=&companyName.1=&titleIB.0=&startYearIB.0=&endYearIB.0=&reason=IF&otherEmail=&greeting=" + Uri.EscapeDataString(messagebody) + "&iweReconnectSubmit=Send+Invitation&key=" + key + "&firstName=" + firstName_Guest + "&lastName=" + lastName_Guest + "&authToken=" + authToken + "&authType=name&trk=prof-0-sb-connect-button&iweLimitReached=false&companyID.0=&companyID.1=&schoolID=&schoolcountryCode=&schoolprovinceCode=&javascriptEnabled=false&existingAssociation=Job+Openings%2C+Job+Leads+and+Job+Connections%21&subject=" + subject + "&defaultText=" + defaultText + "&csrfToken=" + csrfToken + "&sourceAlias=" + sourceAlias + "&goback=" + goback + "";
                string postResponse = httpHelper.postFormData(new Uri("http://www.linkedin.com/people/iweReconnectAction"), postData);

                if (postResponse.Contains("Invitation to") || postResponse.Contains("invitación a"))
                {
                    Log("[ " + DateTime.Now + " ] => [ Invitation Sent successfully To >>> " + firstName_Guest + " With Username >>> " + _UserName + " ]");

                    #region Data Saved In CSV File

                    if (!string.IsNullOrEmpty(firstName_Guest) || !string.IsNullOrEmpty(_UserName))
                    {
                        try

                        {
                            string CSVHeader = "_UserName" + "," + "firstName_Guest" + "," + "lastName_Guest" + "," + "userFirstName" + "," + "userLastName" + ",";
                            string CSV_Content = _UserName + "," + firstName_Guest + "," + lastName_Guest + "," + userFirstName + "," + userLastName + ",";// +TypeOfProfile + ",";
                            CSVUtilities.ExportDataCSVFile(CSVHeader, CSV_Content, Globals.path_LinkedininvitememberResultUrlData);//path_LinkedinSearchSearchByProfileURL);
                            Log("[ " + DateTime.Now + " ] => [ Data Saved In CSV File ! ]");
                        }
                        catch { }

                    }

                    #endregion

                    int delay = new Random().Next(MinDelay, MaxDelay);
                    Log("[ " + DateTime.Now + " ] => [ Delay >>> " + delay + " Seconds With Username >>> " + _UserName + " ]");
                    Log("[ " + DateTime.Now + " ] => [---------------------------------------------------------------------]");
                    Thread.Sleep(delay * 1000);

                }
                else if (postResponse.Contains("Request Error") || postResponse.Contains("Solicitud de error"))
                {
                    Log("[ " + DateTime.Now + " ] => [ Request Error With Username >>> " + _UserName + " ]");

                    if (pageSource.Contains("Sorry, there was a problem processing your request. Please try again"))
                    {
                        Log("[ " + DateTime.Now + " ] => [ Sorry, there was a problem processing your request. Please try again With Username >>> " + _UserName + " ]");
                    }

                    if (pageSource.Contains("You have no confirmed email addresses"))
                    {
                        Log("[ " + DateTime.Now + " ] => [ You have no confirmed email addresses With Username >>> " + _UserName + " ]");
                    }
                }
                else
                {
                    if (pageSource.Contains("Sorry, there was a problem processing your request. Please try again"))
                    {
                        Log("[ " + DateTime.Now + " ] => [ Sorry, there was a problem processing your request. Please try again With Username >>> " + _UserName + " ]");
                    }

                    else if (pageSource.Contains("You have no confirmed email addresses"))
                    {
                        Log("[ " + DateTime.Now + " ] => [ You have no confirmed email addresses With Username >>> " + _UserName + " ]");
                    }
                    else
                    {
                        Log("[ " + DateTime.Now + " ] => [ Error in request With Username >>> " + _UserName + " ]");
                    }
                    int delay = new Random().Next(MinDelay, MaxDelay);
                    Log("[ " + DateTime.Now + " ] => [ Delay >>> " + delay + " Seconds With Username >>> " + _UserName + " ]");
                    Log("[ " + DateTime.Now + " ] => [---------------------------------------------------------------------]");
                    Thread.Sleep(delay * 1000);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error >>> " + ex.StackTrace);
            }
        } 
        #endregion

        #region GetValue
        public string GetValue(string pageSource, string name)
        {
            string value = string.Empty;
            try
            {
                if (pageSource.Contains("name=\"" + name + "\""))
                {
                    string[] arrName = Regex.Split(pageSource, "name=\"" + name + "\"");

                    if (arrName.Length > 1)
                    {
                        value = arrName[1].Substring(arrName[1].IndexOf("value=\"") + 7, arrName[1].IndexOf("\"", (arrName[1].IndexOf("value=\"") + 7)) - (arrName[1].IndexOf("value=\"") + 7)).Replace("value=\"", string.Empty).Replace("\"", string.Empty).Trim();
                       
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return value;
        } 
        #endregion

        #region Log
        private void Log(string message)
        {
            try
            {
                EventsArgs eventArgs = new EventsArgs(message);
                acceptInvitationsLogEvents.LogText(eventArgs);
            }
            catch (Exception)
            {

            }
        } 
        #endregion

        #region FindTheUserName
        public string FindTheUserName(string pagehtml)
        {
            string UserInfo = string.Empty;
            try
            {
                string getData = getBetweenFirstNameLastName(pagehtml, "<img src=", "</a>");
                UserInfo = getData;

            }
            catch { };
            return UserInfo;
        } 
        #endregion

        #region getBetweenFirstNameLastName
        public static string getBetweenFirstNameLastName(string strSource, string strStart, string strEnd)
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
