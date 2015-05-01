using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseLib;
using System.Text.RegularExpressions;

namespace ManageConnections
{
    public class ClsAcceptInvitations
    {

        #region Global declaration
        string _UserName = string.Empty;
        string _Password = string.Empty;
        string _ProxyAddress = string.Empty;
        string _ProxyPort = string.Empty;
        string _ProxyUserName = string.Empty;
        string _ProxyPassword = string.Empty;
        
        public Events acceptInvitationsLogEvents = new Events(); 
        #endregion

        #region UserFirstAndLastName
        public string FindTheUserName(string pagehtml)
        {
            string UserInfo = string.Empty;
            try
            {

                string getData = getBetweenUserName(pagehtml, "<img src=", "</a>");
                UserInfo = getData;

            }
            catch { };
            return UserInfo;
        }


        public static string getBetweenUserName(string strSource, string strStart, string strEnd)
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

        #region ClsAcceptInvitations
        public ClsAcceptInvitations(string username, string password, string proxyaddress, string proxyport, string proxyusername, string proxypassword)
        {
            _UserName = username;
            _Password = password;
            _ProxyAddress = proxyaddress;
            _ProxyPort = proxyport;
            _ProxyUserName = proxyusername;
            _ProxyPassword = proxypassword;

        } 
        #endregion

        #region StartAcceptInvitations
        public void StartAcceptInvitations(ref GlobusHttpHelper httpHelper)
        {
            try
            {
                string csrfToken = string.Empty;
                string userFirstName = string.Empty;
                string UserLastName = string.Empty;
                string SenderName = string.Empty;
                string newPagesource = string.Empty;
                bool isTrue = false;
                int startRow = 1;
                
                string pageSource = httpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/inbox/invitations/pending"));
                            

                var resultForUserDetails = FindTheUserName(pageSource);
                try
                {
                    resultForUserDetails = resultForUserDetails.Substring(resultForUserDetails.IndexOf("alt="), resultForUserDetails.IndexOf("height") - resultForUserDetails.IndexOf("alt=")).Replace("alt=", string.Empty).Replace("/", string.Empty).Trim();
                    userFirstName = resultForUserDetails.Split(' ')[0].Replace("\"", string.Empty);
                    UserLastName = resultForUserDetails.Split(' ')[1].Replace("\"", string.Empty);
                }
                catch { }

                if (pageSource.Contains("csrfToken"))
                {
                    csrfToken = pageSource.Substring(pageSource.IndexOf("csrfToken"), 100);
                    string[] Arr = csrfToken.Split('"');
                    try
                    {
                        foreach (string item in Arr)
                        {
                            try
                            {
                                if (item.Contains("csrfToken="))
                                {
                                    csrfToken = item.Substring(item.IndexOf("csrfToken="), item.IndexOf("&", item.IndexOf("csrfToken=")) - item.IndexOf("csrfToken=")).Replace("csrfToken=", string.Empty).Replace("\"", string.Empty).Replace("\\", string.Empty).Trim();//Arr[2].Replace(@"\", string.Empty).Replace("//", string.Empty);

                                    break;
                                }
                            }
                            catch (Exception ex)
                            {
                            }

                            if (item.Contains("csrfToken="))
                            {
                                csrfToken = item.Replace("csrfToken=", string.Empty).Trim();

                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //csrfToken = Arr[0].Replace("csrfToken=", "").Replace("\\", "");
                    }
                }

                // For Show More

                //string postData1 = "pkey=inbox-invitations-pending&tcode=%5Bobject%20Arguments%5D&plist=";
                //string response1 = httpHelper.postFormData(new Uri("http://www.linkedin.com/lite/web-action-track?csrfToken="+csrfToken+""),postData1);
                //string pageSource2=httpHelper.getHtmlfromUrl(new Uri("http://www.linkedin.com/inbox/invitations/pending/more?sinceDate=1366351490125&startRow=6&count=20&showBlocked=false&ctx=inbox&rnd=1366353236172"));
                //*** Conver HTML to XML *******************************//
                #region Convert HTML to XML
                ChilkatHttpHelpr objhelper = new ChilkatHttpHelpr();
                //xHtml contain xml data 
                string xHtml = objhelper.ConvertHtmlToXml(pageSource);

                Chilkat.Xml xml = new Chilkat.Xml();
                xml.LoadXml(xHtml);
                //xHtml.

                ////  Iterate over all h1 tags:
                Chilkat.Xml xNode = default(Chilkat.Xml);
                Chilkat.Xml xBeginSearchAfter = default(Chilkat.Xml);
                #endregion

                #region Invitatin count
                xBeginSearchAfter = null;
                xNode = xml.SearchForAttribute(xBeginSearchAfter, "span", "class", "invitation-count count ");

                try
                {
                    while ((xNode != null))
                    {
                        string strvalue = xNode.AccumulateTagContent("text", "script|style");
                        string Invitatincount = strvalue;
                        Log("[ " + DateTime.Now + " ] => [ Invitation Count = " + Invitatincount + " UserName = " + _UserName + " ]");
                        Log("-----------------------------------------------------------------------------------------------------------------------------------");
                        break;
                    }
                }
                catch (Exception ex)
                {

                }

                #endregion
                
                
                do
                {
                    newPagesource = httpHelper.getHtmlfromUrl1(new Uri("https://www.linkedin.com/inbox/invitations?keywords=&sortBy=&startRow=" + startRow + "&subFilter=&trk=&showBlocked=false"));
                if (newPagesource.Contains("inbox-list"))
                {
                    string inbox_list = httpHelper.GetDataWithTagValueByTagAndAttributeNameWithClass(newPagesource, "ol", "inbox-list ");

                    if (inbox_list.Contains("<li"))
                    {
                        isTrue = true;
                        string[] srrLi = Regex.Split(inbox_list, "<li");

                        foreach (string item in srrLi)
                        {
                            try
                            {
                                if (item.Contains("data-gid=\""))
                                {
                                    string data_gid = item.Substring(item.IndexOf("data-gid=\"") + 10, item.IndexOf("\"", item.IndexOf("data-gid=\"") + 10) - (item.IndexOf("data-gid=\"") + 10)).Replace("\"", string.Empty).Replace("data-gid=\"", string.Empty).Trim();
                                    int startindex1 = item.IndexOf("alt=");
                                    string start1 = item.Substring(startindex1).Replace("alt=",string.Empty);
                                    int endindex1 = start1.IndexOf("height");
                                    string end1 = start1.Substring(0, endindex1).Replace("\"", string.Empty).Trim();
                                    SenderName = end1;

                                    string response2 = httpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/inbox/action?mboxItemGID=" + data_gid + "&actionType=invitationAccept&csrfToken=" + csrfToken + "&goback=%2Epiv_*1_*1_*1_*1_*1&trk=inbox-invitations-inv-accept&ctx=inbox&rnd=1366352095313"));

                                    if (response2.Contains(" are now connected"))
                                    {
                                        string SuccessMsg = string.Empty;
                                        int startindex = response2.IndexOf("<div class=\"confirmation\">");

                                        if (startindex > 0)
                                        {
                                            try
                                            {
                                                string start = response2.Substring(startindex).Replace("<div class=\"confirmation\">", string.Empty);
                                                int endindex = start.IndexOf("<ul>");
                                                string end = start.Substring(0, endindex);
                                                //SuccessMsg = end.Replace("<h4>", string.Empty).Replace("\"", string.Empty).Replace("</h4>", string.Empty).Replace("\n", string.Empty).Replace("\t", string.Empty).Replace("\"u002", "-").Replace("You", "User: " + _UserName).Trim();
                                                SuccessMsg = end.Replace("<h4>", string.Empty).Replace("\"", string.Empty).Replace("</h4>", string.Empty).Replace("\n", string.Empty).Replace("\t", string.Empty).Replace("\"u002", "-").Replace("You", "User: " + _UserName + "( Name:" + userFirstName + "  " + UserLastName + ") ").Trim();

                                            }
                                            catch { }
                                        }

                                        GlobusFileHelper.AppendStringToTextfileNewLine(SuccessMsg, Globals.path_AcceptInvitationEmail);
                                        Log("[ " + DateTime.Now + " ] => [ " + SuccessMsg + " ]");
                                        
                                    }
                                    if (!(response2.Contains(SenderName)))
                                    {
                                        Log("[ " + DateTime.Now + " ] => [ Invitation accepted from " + SenderName + " ]");
                                    }
                                    else
                                    {
                                        //Log("There is some error !");
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                        }

                    }
                    else
                    {
                        //Log("[ " + DateTime.Now + " ] => [ There is no invitation ! ]");
                        Log("[ " + DateTime.Now + " ] => [ No more invitations left to accept ! ]");

                    }
                }
                startRow = startRow + 10;
                } while (newPagesource.Contains("is now a connection."));
                //else
                //{
                //    Log("[ " + DateTime.Now + " ] => [ There is no invitation ! ]");
                //}

                //if (isTrue)
                //{
                //    StartAcceptInvitations(ref httpHelper);
                //}
            }
            catch (Exception ex)
            {
            }
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
    }
}
