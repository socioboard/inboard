using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Data;
using BaseLib;

namespace ManageConnections
{
    public class InvitationEmails
    {
        #region Variable and Object Declaration

        #region Manage Connection Related Variable Declaration

        public static List<string> _lstInviteEmail = new List<string>();
        public string _ConnectionSearchKeyword = string.Empty;


        #endregion

        string _UserName = string.Empty;
        string _Password = string.Empty;
        string _ProxyAddress = string.Empty;
        string _ProxyPort = string.Empty;
        string _ProxyUserName = string.Empty;
        string _ProxyPassword = string.Empty;

        public int MinimumDelay = 20;
        public int MaximumDelay = 25;

        string _Email = string.Empty;
        List<string> _lstInviteEmails = new List<string>();
        Queue<string> UsingEmailQueue = new Queue<string>();
        public static readonly object locker_InvitataionEmail = new object();
        public static string UniqueEmailStatus = "False";

        public AccountInfo.LinkedinLoginAndLogout linkedinLoginAndLogout = new AccountInfo.LinkedinLoginAndLogout();
        GlobusHttpHelper HttpHelper = new GlobusHttpHelper();

        //public static Events InviteConnectionLogEvents = new Events();
        public static Events InviteConnectionLogEvents = new Events();
        public static List<string> listItemTempForInvitation = new List<string>();
        #endregion

        #region InvitationEmails
        public InvitationEmails()
        {

        } 
        #endregion

        #region InvitationEmails
        public InvitationEmails(string username, string password, string proxyaddress, string proxyport, string proxyusername, string proxypassword, Queue<string> QueueEmail, List<string> lstInviteEmail)
        {
            this._UserName = username;
            this._Password = password;
            this._ProxyAddress = proxyaddress;
            this._ProxyPort = proxyport;
            this._ProxyUserName = proxyusername;
            this._ProxyPassword = proxypassword;
            this.UsingEmailQueue = QueueEmail;
            this._lstInviteEmails = lstInviteEmail;
        } 
        #endregion


        #region InviteEmailConnect
        public static void InviteEmailConnect(ref ManageConnections.InvitationEmails Invitation_Emails, int noofemailRequest, int MaxDelay, int MinDelay)
        {
            try
            {
               Invitation_Emails.InviteFriendThroughEmail(noofemailRequest, MaxDelay, MinDelay);
            }
            catch
            {
            }
        } 
        #endregion

        #region InviteFriendThroughEmail
        public void InviteFriendThroughEmail(int NoofemailRequest, int MaxDelay, int MinDelay)
        {
            try
            {
                int proxyport = 888;
                Regex PortCheck = new Regex("^[0-9]*$");

                if (PortCheck.IsMatch(_ProxyPort) && !string.IsNullOrEmpty(_ProxyPort))
                {
                    proxyport = int.Parse(_ProxyPort);
                }
                                
                string xMESSAGE = string.Empty;
                LoggerManageConnection("[ " + DateTime.Now + " ] => [ Logging In With Account : " + _UserName + " ]");

                if (linkedinLoginAndLogout.LoginHttpHelper(_UserName, _Password, _ProxyAddress, _ProxyPort, _ProxyUserName, _ProxyPassword, ref HttpHelper, ref xMESSAGE))
                {
                    LoggerManageConnection("[ " + DateTime.Now + " ] => [ Logged in with " + _UserName + " ]");
                    //int CounterInvite = 1;

                    if (UniqueEmailStatus == "False")
                    {
                        DataSet ds_bList = new DataSet();
                        foreach (string Email in _lstInviteEmails)
                        {
                            string emailtosend = string.Empty;

                            try
                            {
                                string Querystring = "Select UserID From tb_BlackListAccount Where UserID ='" + Email + "'";
                                ds_bList = DataBaseHandler.SelectQuery(Querystring, "tb_BlackListAccount");
                            }
                            catch { }

                            if (ds_bList.Tables.Count > 0)
                            {
                                if (ds_bList.Tables[0].Rows.Count > 0)
                                {
                                    LoggerManageConnection("[ " + DateTime.Now + " ] => [ User: " + Email + " is Added BalckListed List For Send Connection Pls Check ]");
                                    return;
                                }
                            }
                            else
                            {
                                if (Email.Contains(":"))
                                {
                                    emailtosend = Email.Split(':')[0];
                                }
                                else
                                {
                                    emailtosend = Email;
                                }

                                try
                                {
                                    LoggerManageConnection("[ " + DateTime.Now + " ] => [ Sending Invitation To :" + emailtosend + " ]");

                                    string emailAddresses = string.Empty;
                                    string csrfToken = string.Empty;
                                    string sourceAlias = string.Empty;
                                    string PostData = string.Empty;
                                    string PgSrcInviteMessgae = string.Empty;
                                    string postUrl = string.Empty;

                                    // https://www.linkedin.com/fetch/manual-invite-create

                                    PgSrcInviteMessgae = HttpHelper.getHtmlfromUrlProxy(new Uri("https://www.linkedin.com/fetch/importAndInviteEntry"), _ProxyAddress, proxyport, _ProxyUserName, _ProxyPassword);
                                    emailAddresses = emailtosend;

                                    // For csrfToken
                                    string[] ArrCsrfToken = Regex.Split(PgSrcInviteMessgae, "input");
                                    foreach (string item in ArrCsrfToken)
                                    {
                                        try
                                        {
                                            if (!item.Contains("<!DOCTYPE"))
                                            {
                                                if (item.Contains("csrfToken") && item.Contains("value="))
                                                {
                                                    csrfToken = item;
                                                    break;
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                    }
                                    if (csrfToken.Contains("csrfToken"))
                                    {
                                        try
                                        {
                                            csrfToken = csrfToken.Substring(csrfToken.IndexOf("csrfToken"), 44);
                                            string[] Arr = csrfToken.Split('&');
                                            csrfToken = Arr[0].Replace("csrfToken=", string.Empty);
                                            csrfToken = csrfToken.Replace(":", "%3A").Replace("\\", string.Empty).Replace("csrfToken", "").Replace("=", "").Replace("value", "").Replace("\"", "").Trim();
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                    }

                                    // For sourceAlias
                                    if (PgSrcInviteMessgae.Contains("sourceAlias"))
                                    {
                                        try
                                        {
                                            sourceAlias = PgSrcInviteMessgae.Substring(PgSrcInviteMessgae.IndexOf("sourceAlias"), 100);
                                            string[] Arr = sourceAlias.Split('"');
                                            sourceAlias = Arr[2].Replace("\\", string.Empty).Trim();
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                    }

                                    //Post Data for Invite Message 
                                    PostData = "emailAddresses=" + Uri.EscapeDataString(emailAddresses) + "&subject=Invitation+to+connect+on+LinkedIn&csrfToken=" + csrfToken + "&sourceAlias=" + sourceAlias;
                                    //Post Url for Invite Message 

                                    postUrl = "https://www.linkedin.com/fetch/manual-invite-create";

                                    string postResponse = HttpHelper.postFormDataRef(new Uri(postUrl), PostData, "https://www.linkedin.com/fetch/manual-invite-create", "", "", "", "", "");

                                    if (postResponse.Contains("One invitation has been sent.") && postResponse.Contains("alert success"))
                                    {
                                        LoggerManageConnection("[ " + DateTime.Now + " ] => [ Invitation sent to :" + emailtosend + " from ID : " + _UserName + " ]");
                                        GlobusFileHelper.AppendStringToTextfileNewLine(_UserName + "-->" + emailtosend, Globals.path_AddConnectionEmail);
                                    }
                                    else
                                    {
                                        LoggerManageConnection("[ " + DateTime.Now + " ] => [ Some Problem for sending Invitation :" + emailtosend + " from ID : " + _UserName + " ]");
                                        GlobusFileHelper.AppendStringToTextfileNewLine(_UserName + "-->" + emailtosend, Globals.path_NonAddConnectionEmail);
                                    }

                                    MinimumDelay = MinDelay;
                                    MaximumDelay = MaxDelay;

                                    int Delay = RandomNumberGenerator.GenerateRandom(MinimumDelay, MaximumDelay);
                                    LoggerManageConnection("[ " + DateTime.Now + " ] => [ Delay For : " + Delay + " Seconds ]");
                                    Thread.Sleep(Delay * 1000);
                                }
                                catch (Exception ex)
                                {

                                }
                            }
                        }
                    }
                    else
                    {
                        int counter = 0;

                        while (UsingEmailQueue.Count > 0)
                        {
                            string emailtosend = string.Empty;

                            lock (locker_InvitataionEmail)
                            {
                                if (UsingEmailQueue.Count > 0)
                                {
                                    emailtosend = UsingEmailQueue.Dequeue();
                                    if (listItemTempForInvitation.Contains(emailtosend)) { continue; }
                                }

                                else
                                {
                                    LoggerManageConnection("[ " + DateTime.Now + " ] => [ All Loaded Email are used ]");
                                }

                                try
                                {

                                    LoggerManageConnection("[ " + DateTime.Now + " ] => [ Sending Invitation To :" + emailtosend + " from ID : " + _UserName + " ]");

                                    string emailAddresses = string.Empty;
                                    string csrfToken = string.Empty;
                                    string sourceAlias = string.Empty;
                                    string PostData = string.Empty;
                                    string PgSrcInviteMessgae = string.Empty;
                                    string postUrl = string.Empty;

                                    // https://www.linkedin.com/fetch/manual-invite-create

                                    PgSrcInviteMessgae = HttpHelper.getHtmlfromUrlProxy(new Uri("https://www.linkedin.com/fetch/importAndInviteEntry"), _ProxyAddress, proxyport, _ProxyUserName, _ProxyPassword);
                                    emailAddresses = emailtosend.Replace("@", "%40");

                                    // For csrfToken
                                    string[] ArrCsrfToken = Regex.Split(PgSrcInviteMessgae, "input");
                                    foreach (string item in ArrCsrfToken)
                                    {
                                        try
                                        {
                                            if (!item.Contains("<!DOCTYPE"))
                                            {
                                                if (item.Contains("csrfToken") && item.Contains("value="))
                                                {
                                                    csrfToken = item;
                                                    break;
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                    }
                                    if (csrfToken.Contains("csrfToken"))
                                    {
                                        try
                                        {
                                            csrfToken = csrfToken.Substring(csrfToken.IndexOf("csrfToken"), 40);
                                            string[] Arr = csrfToken.Split('&');
                                            csrfToken = Arr[0].Replace("csrfToken=", string.Empty);
                                            csrfToken = csrfToken.Replace(":", "%3A").Replace("\\", string.Empty).Trim(); ;
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                    }

                                    // For sourceAlias
                                    if (PgSrcInviteMessgae.Contains("sourceAlias"))
                                    {
                                        try
                                        {
                                            sourceAlias = PgSrcInviteMessgae.Substring(PgSrcInviteMessgae.IndexOf("sourceAlias"), 100);
                                            string[] Arr = sourceAlias.Split('"');
                                            sourceAlias = Arr[2].Replace("\\", string.Empty).Trim();
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                    }

                                    //Post Data for Invite Message 
                                    // emailAddresses=adanu%40femail.com%2Cagantuk%40gmail.com%2C&subject=Invitation+to+connect+on+LinkedIn&csrfToken=ajax%3A3731156693650115835&sourceAlias=0_7MU6tu3FMvMnMAXccRg8dcLvCi6FWQRq-jJrq2WCIlH
                                    PostData = "emailAddresses=" + emailAddresses + "&csrfToken=" + csrfToken + "&sourceAlias=" + sourceAlias;

                                    //Post Url for Invite Message 
                                    postUrl = "https://www.linkedin.com/fetch/manual-invite-create";

                                    //Post Response
                                    string postResponse = HttpHelper.postFormDataRef(new Uri(postUrl), PostData, "https://www.linkedin.com/fetch/manual-invite-create", "", "", "", "", "");

                                    if (postResponse.Contains("One invitation has been sent.") && postResponse.Contains("alert success"))
                                    {
                                        LoggerManageConnection("[ " + DateTime.Now + " ] => [ Invitation sent to :" + emailtosend + " from ID : " + _UserName);
                                        GlobusFileHelper.AppendStringToTextfileNewLine(_UserName + "-->" + emailtosend, Globals.path_AddConnectionEmail);
                                    }
                                    else
                                    {
                                        LoggerManageConnection("[ " + DateTime.Now + " ] => [ Some Problem for sending Invitation :" + emailtosend + " from ID : " + _UserName + " ]");
                                        GlobusFileHelper.AppendStringToTextfileNewLine(_UserName + "-->" + emailtosend, Globals.path_NonAddConnectionEmail);
                                    }
                                    counter = counter + 1;

                                    MinimumDelay = MinDelay;
                                    MaximumDelay = MaxDelay;

                                    int Delay = RandomNumberGenerator.GenerateRandom(MinimumDelay, MaximumDelay);
                                    LoggerManageConnection("[ " + DateTime.Now + " ] => [ Delay For : " + Delay + " Seconds ]");
                                    Thread.Sleep(Delay * 1000);
                                    listItemTempForInvitation.Add(emailtosend);
                                }
                                catch (Exception ex)
                                {

                                }

                            }
                        }
                    }
                }
                else
                {
                    LoggerManageConnection("[ " + DateTime.Now + " ] => [ " + xMESSAGE + " With Username >>> " + _UserName + " ]");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            } 
        } 
        #endregion

        #region LoggerManageConnection
        private void LoggerManageConnection(string message)
        {
            EventsArgs eventArgs = new EventsArgs(message);
            InviteConnectionLogEvents.LogText(eventArgs);
        } 
        #endregion


    }
}
