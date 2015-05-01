using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseLib;
using System.Text.RegularExpressions;
using System.Threading;
using System.Data;




namespace ManageConnections
{
    public class ConnectUsingSearchKeywod
    {
        //** Variable and Object Declaration *****************************************************************************
        #region Variable and Object Declaration

        string _UserName = string.Empty;
        string _Password = string.Empty;
        string _ProxyAddress = string.Empty;
        string _ProxyPort = string.Empty;
        string _ProxyUserName = string.Empty;
        string _ProxyPassword = string.Empty;
        int SendInvitationCount = 0;
        string _ConnectSearchKeyword = string.Empty;
        public static readonly object locker_InvitataionKeyword = new object();
        int PageNumber = 0;
        bool valid = true;
        Queue<string> UsingKeyWordQueue = new Queue<string>();
        public static int SearchMinDelay = 20;
        public static int SeacrhMaxDelay = 25;

        //public int SearchMinDelay = 0;
        //public int SeacrhMaxDelay = 0;

        public static bool isSpinTaxSearchWithInvite = false;
        public static List<string> UpdatelistGreetMessatge = new List<string>();
        public static string MessageWithoutspintax = string.Empty;

        public AccountInfo.LinkedinLoginAndLogout linkedinLoginAndLogout = new AccountInfo.LinkedinLoginAndLogout();

        GlobusHttpHelper HttpHelper = new GlobusHttpHelper();
        

        public static Events ConnectSearchLogEvents = new Events();
        public static readonly object LockerConnection = new object();

        public static Queue<string> lstQueuKeywords = new Queue<string>();
        public static Queue<string> lstQueuEmail = new Queue<string>();
        public static int countofKeywords = 0;
        public static bool UseuniqueConn = false;
        public static bool UseUniqueEmailConn = false;

        #region CampaingSearchwithInivite
        public static DateTime SchedulerEndTime;
        public static bool _IsFollowProcessStart = true;
        public static bool IsSheduleDaily = true;
        public static string campaignName = string.Empty;
        public static string FeatureName = string.Empty;
        #endregion


        

        #endregion


        public static Events CampaignSearchLogevents = new Events();
        public static Events CampaignStartLogevents = new Events();
        public void RaiseCampaignSearchLogEvents(string log)
        {
            EventsArgs eArgs = new EventsArgs(log);
            CampaignSearchLogevents.LogText(eArgs);
        }

        public void RaiseCampaignStartLogEvents(string log)
        {
            EventsArgs eArgs = new EventsArgs(log);
            CampaignStartLogevents.LogText(eArgs);
        }

        #region ConnectUsingSearch
        public ConnectUsingSearchKeywod(string username, string password, string proxyaddress, string proxyport, string proxyusername, string proxypassword, Queue<string> QUEsearchName)
        {
            this._UserName = username;
            this._Password = password;
            this._ProxyAddress = proxyaddress;
            this._ProxyPort = proxyport;
            this._ProxyUserName = proxyusername;
            this._ProxyPassword = proxypassword;
            this.UsingKeyWordQueue = QUEsearchName;
            
        }
        #endregion

        #region ConnectUsingSearch
        public ConnectUsingSearchKeywod(string username, string password, string proxyaddress, string proxyport, string proxyusername, string proxypassword)
        {
            this._UserName = username;
            this._Password = password;
            this._ProxyAddress = proxyaddress;
            this._ProxyPort = proxyport;
            this._ProxyUserName = proxyusername;
            this._ProxyPassword = proxypassword;
            //this.UsingKeyWordQueue = QUEsearchName;
        }
        #endregion

        #region ConnectionSearch
        bool Isaccountvalid = true;
        public void ConnectUsing_SearchKeywod(int DelayStart, int DelayEnd)
        {
            try
            {

            }
            catch
            { }

            try
            {
                if (true)
                {
                    string strPageNumber = string.Empty;
                    string Textmessage = string.Empty;

                    //Login with User name and password ***********************

                    if (linkedinLoginAndLogout.LoginHttpHelper(_UserName, _Password, _ProxyAddress, _ProxyPort, _ProxyUserName, _ProxyPassword, ref HttpHelper, ref Textmessage))
                    {
                        

                        LoggerManageConnection("[ " + DateTime.Now + " ] => [ Logged in with " + _UserName + " ]");

                        int proxyport = 888;
                        string SummaryLink = string.Empty;

                        Regex PortCheck = new Regex("^[0-9]*$");

                        if (PortCheck.IsMatch(_ProxyPort) && !string.IsNullOrEmpty(_ProxyPort))
                        {
                            proxyport = int.Parse(_ProxyPort);
                        }

                        while (UsingKeyWordQueue.Count > 0)
                        {
                            lock (locker_InvitataionKeyword)
                            {
                                if (UsingKeyWordQueue.Count > 0)
                                {
                                    _ConnectSearchKeyword = UsingKeyWordQueue.Dequeue();
                                }
                                else
                                {
                                    LoggerManageConnection("[ " + DateTime.Now + " ] => [ All Loaded Keyword are used ]");
                                }
                            }


                            string Url = "http://www.linkedin.com/search/fpsearch?keywords=" + _ConnectSearchKeyword + "&keepFacets=keepFacets&page_num=1&pplSearchOrigin=ADVS&viewCriteria=2&sortCriteria=R&redir=redir";

                            string PgSrcMain = HttpHelper.getHtmlfromUrlProxy(new Uri(Url), _ProxyAddress, proxyport, _ProxyUserName, _ProxyPassword);
                            string[] Arr = Regex.Split(PgSrcMain, "results_count_with_keywords_i18n");
                            foreach (string item in Arr)
                            {
                                try
                                {
                                    if (!item.Contains("<!DOCTYPE"))
                                    {
                                        if (item.Contains("results for"))
                                        {
                                            string data = RemoveAllHtmlTag.StripHTML(item);
                                            data = data.Replace("\n", "");
                                            if (data.Contains(">"))
                                            {
                                                string[] ArrTemp = Regex.Split(data, "results");
                                                data = ArrTemp[0];
                                                data = data.Replace("\":\"", "").Replace(",", string.Empty);
                                                data = data.Trim();
                                                strPageNumber = data.ToString().Replace("&quot;", string.Empty).Replace(",", string.Empty).Replace("&", string.Empty).Replace(":", string.Empty).Replace("strong", string.Empty).Replace("\\u003e", "").Replace("\\u003c", "").Replace("/", "").Trim();
                                                break;
                                            }
                                        }
                                    }
                                }
                                catch { }
                            }


                            if (string.IsNullOrEmpty(strPageNumber))
                            {

                                string urlGetdata = "http://www.linkedin.com/vsearch/f?keywords=" + _ConnectSearchKeyword + "&orig=GLHD&pageKey=voltron_federated_search_internal_jsp&search=Search";
                                urlGetdata = "http://www.linkedin.com/vsearch/f?keywords=Jobs&orig=GLHD&pageKey=voltron_federated_search_internal_jsp&search=Search";
                                PgSrcMain = HttpHelper.getHtmlfromUrlProxy(new Uri(Url), _ProxyAddress, proxyport, _ProxyUserName, _ProxyPassword);
                                Arr = Regex.Split(PgSrcMain, "div");
                                foreach (string item in Arr)
                                {
                                    try
                                    {
                                        if (!item.Contains("<!DOCTYPE"))
                                        {
                                            if (item.Contains("results-summary"))
                                            {
                                                string data = RemoveAllHtmlTag.StripHTML(item);
                                                data = data.Replace("\n", "");
                                                if (data.Contains(">"))
                                                {
                                                    string[] ArrTemp = data.Split('>');
                                                    data = ArrTemp[1];
                                                    data = data.Replace("results", "");
                                                    data = data.Trim();
                                                    string[] ArrTemp1 = data.Split(' ');
                                                    data = ArrTemp1[0].Replace(',', ' ').Trim();

                                                    strPageNumber = data.Replace(" ", string.Empty).Replace(",", string.Empty).Trim();
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    catch { }
                                }
                            }

                            try
                            {
                                PageNumber = int.Parse(strPageNumber);//find the page number for each key word
                            }
                            catch (Exception)
                            {
                                PageNumber = 0;
                                //throw;
                            }
                            if ((PageNumber == 0))
                            {
                                LoggerManageConnection("[ " + DateTime.Now + " ] => [ Sorry No Search Results For  Keyword : " + _ConnectSearchKeyword + " From Account: " + _UserName + " ]");
                                break;
                                //TestConnectPageSearch("1");
                            }

                            PageNumber = (PageNumber / 10) - 1;//each page have 10 data so number of record divide by 10....

                            if (PageNumber == -1)
                            {
                                PageNumber = 2;
                            }

                            for (int i = 1; i <= PageNumber; i++)
                            {
                                //loop for send request
                                if (!Isaccountvalid)
                                {
                                    return;
                                }

                                if (!UseuniqueConn)
                                {
                                    //LoggerManageConnection("Adding Connections From Page : " + i + " From Account " + _UserName);
                                    TestConnectPageSearch(i.ToString(), DelayStart, DelayEnd);
                                }
                                else
                                {
                                    //LoggerManageConnection("Adding Connections From Page : " + i + " From Account " + _UserName);
                                    TestConnectPageSearchForUniqueUrl(i.ToString());
                                }

                            }
                            

                            if (valid)
                            {
                                string CSVHeader = "UserName" + "," + "SearchKeyword";
                                string CSV_Content = _UserName + "," + _ConnectSearchKeyword;
                                // CSVUtilities.ExportDataCSVFile(CSVHeader, CSV_Content, Globals.path_AddConnectionFail);
                                //LoggerManageConnection("*************************************************************************************************");
                                //LoggerManageConnection("*************************************************************************************************");

                                //LoggerManageConnection("*************************************************************************************************");
                                //LoggerManageConnection("*************************************************************************************************");
                                if (SendInvitationCount > 0)
                                {
                                    int limit = SearchCriteria.RequestLimit;
                                   // LoggerManageConnection("[ " + DateTime.Now + " ] => [ " + SendInvitationCount + "  Send Request Completed With " + _UserName + " and For Keyword " + _ConnectSearchKeyword + " ]");
                                    LoggerManageConnection("[ " + DateTime.Now + " ] => [ " + limit + "  Send Request Completed With " + _UserName + " and For Keyword " + _ConnectSearchKeyword + " ]");

                                    //GlobusFileHelper.AppendStringToTextfileNewLine(_ConnectSearchKeyword + "  " +_UserName , Globals.path_AddConnection);
                                    //CSVUtilities.ExportDataCSVFile(CSVHeader, CSV_Content, Globals.path_AddConnectionSuccess);


                                }
                                else
                                {
                                    CSVUtilities.ExportDataCSVFile(CSVHeader, CSV_Content, Globals.path_AddConnectionFail);
                                    LoggerManageConnection("[ " + DateTime.Now + " ] => [ Not Able To send Any Connection Request For Keyword " + _ConnectSearchKeyword + " ]");
                                    // GlobusFileHelper.AppendStringToTextfileNewLine(_ConnectSearchKeyword + "  " + _UserName, Globals.path_AddConnection);

                                }
                                //LoggerManageConnection("*************************************************************************************************");
                                //LoggerManageConnection("*************************************************************************************************");
                                SendInvitationCount = 0;
                                //LoggerManageConnection("Send Request Completed");
                            }
                        }

                    }
                    else if (Textmessage.Contains("This login attempt seems suspicious.Please enter the verification code sent to the email."))
                    {

                        LoggerManageConnection("[ " + DateTime.Now + " ] => [ " + _UserName + "  login attempt seems suspicious.Please enter the verification code sent to the email. ]");
                        return;
                    }
                    else
                    {

                        LoggerManageConnection("[ " + DateTime.Now + " ] => [ " + _UserName + " Your LinkedIn account has been temporarily restricted ]");
                        return;
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        #endregion

        #region TestConnectPageSearch
        public void TestConnectPageSearch(string pageNumber, int DelayStart, int DelayEnd)
        {
            int proxyport = 888;
            string SummaryLink = string.Empty;

            Regex PortCheck = new Regex("^[0-9]*$");

            if (PortCheck.IsMatch(_ProxyPort) && !string.IsNullOrEmpty(_ProxyPort))
            {
                proxyport = int.Parse(_ProxyPort);
            }

            //recive the page number and then hit page wise and find the url in which we need to send the request.....

            string UrlConnectpage = "http://www.linkedin.com/search/fpsearch?keywords=" + _ConnectSearchKeyword + "&keepFacets=keepFacets&page_num=" + pageNumber + "&pplSearchOrigin=ADVS&viewCriteria=2&sortCriteria=R&redir=redir";
//            string PgSrcMain1 = HttpHelper.getHtmlfromUrlProxy1(new Uri(UrlConnectpage), _ProxyAddress, proxyport, _ProxyUserName, _ProxyPassword);
            string PgSrcMain1 = HttpHelper.getHtmlfromUrlProxy1(new Uri(UrlConnectpage), _ProxyAddress, proxyport, _ProxyUserName, _ProxyPassword);


            string PostData = string.Empty;
            string page_num = string.Empty;
            string postUrl = string.Empty;
            string facetsOrder = string.Empty;

            if (PgSrcMain1.Contains("facetsOrder"))
            {
                facetsOrder = PgSrcMain1.Substring(PgSrcMain1.IndexOf("facetsOrder"), 200);
                string[] Arr = facetsOrder.Split('"');
                facetsOrder = Arr[2];
                string DecodedCharTest = Uri.UnescapeDataString(facetsOrder);
                string DecodedEmail = Uri.EscapeDataString(facetsOrder);
                facetsOrder = DecodedEmail;
            }

            page_num = pageNumber;

            PostData = "searchLocationType=Y&inNetworkSearch=inNetworkSearch&search=&viewCriteria=2&sortCriteria=C&facetsOrder=" + facetsOrder + "&page_num=" + page_num + "&openFacets=N%2CCC%2CG";
            postUrl = "http://www.linkedin.com/vsearch/p?page_num=" + page_num + "&orig=ADVS&keywords=" + _ConnectSearchKeyword + "";

            //Diclaration For Post Add Connection For Keyword
            string Val_goback = string.Empty;
            string Val_sourceAlias = string.Empty;
            string Val_key = string.Empty;
            string Val_defaultText = string.Empty;
            string Val_firstName = string.Empty;
            string Val_CsrToken = string.Empty;
            string Val_Subject = string.Empty;
            string Val_greeting = string.Empty;
            string Val_AuthToken = string.Empty;
            string Val_AuthType = string.Empty;
            string val_trk = string.Empty;
            string Val_lastName = string.Empty;

            //Post Response
            string postResponse = HttpHelper.postFormDataProxy(new Uri(postUrl), PostData, _ProxyAddress, proxyport, _ProxyUserName, _ProxyPassword);
          //  List<string> PageSerchUrl = ChilkatBasedRegex.GettingAllUrls2(postResponse, "profile/view?id");
            List<string> PageSerchUrl = ChilkatBasedRegex.GettingAllUrls2(postResponse, "profile/view?id");

            if (PageSerchUrl.Count == 0)
            {
                LoggerManageConnection("[--------------------------------------------------------------------------------------------------]");
                LoggerManageConnection("[ " + DateTime.Now + " ] => [ No url found in 2nd degree connection in page no : " + page_num + " ]");
                LoggerManageConnection("[--------------------------------------------------------------------------------------------------]");
            }
            else
            {
                LoggerManageConnection("[--------------------------------------------------------------------------------------------------]");
                LoggerManageConnection("[ " + DateTime.Now + " ] => [ " + PageSerchUrl.Count() + " url found in 2nd degree connection in page no : " + page_num + " ]");
                LoggerManageConnection("[--------------------------------------------------------------------------------------------------]");
            }

            string FrnAcceptUrL = string.Empty;
            foreach (string item in PageSerchUrl)
            {
               
                try
                {
                    if (IsSheduleDaily)
                    {
                        if (SchedulerEndTime.Hour == DateTime.Now.Hour && DateTime.Now.Minute >= SchedulerEndTime.Minute)
                        {
                            _IsFollowProcessStart = true;
                            new Thread(() =>
                            {
                                //frm_mixcampaignmanager frmcamp = new frm_mixcampaignmanager();
                                //frmcamp.StoprunningCampaign(CampaignName);

                                RaiseCampaignSearchLogEvents(campaignName);
                                RaiseCampaignStartLogEvents(campaignName);

                            }).Start();

                            LoggerManageConnection("[ " + DateTime.Now + " ] => [ Scheduled time completed  ]");
                            break;
                        }
                    }
                    FrnAcceptUrL = item;
                    string[] tempUrl = Regex.Split(FrnAcceptUrL, "&authType");
                    DataSet dt = new DataSet();
                    try
                    {
                        dt = DataBaseHandler.SelectQuery("SELECT * FROM tb_SearchWithInvite Where UserName = '" + _UserName + "' and InvitationUrl = '" + tempUrl[0] + "'", "tb_SearchWithInvite");
                    }
                    catch { }
                    int count_NO_RoWs = dt.Tables[0].Rows.Count;
                    if (count_NO_RoWs == 0)
                    {
                        string FRNURLresponce = HttpHelper.getHtmlfromUrl1(new Uri(FrnAcceptUrL));

                        if (FRNURLresponce.Contains("/profile/view?id="))
                        {
                            try
                            {
                                Val_key = FRNURLresponce.Substring(FRNURLresponce.IndexOf("/profile/view?id="), 30);
                                string[] Arr = Val_key.Split('&');
                                Val_key = Arr[0].Replace("/profile/view?id=", "");
                            }
                            catch (Exception ex)
                            {

                            }

                        }
                        if (FrnAcceptUrL.Contains("/profile/view?id="))
                        {
                            try
                            {
                                Val_key = FrnAcceptUrL.Substring(FrnAcceptUrL.IndexOf("id="), (FrnAcceptUrL.IndexOf("&", FrnAcceptUrL.IndexOf("id=")) - FrnAcceptUrL.IndexOf("id="))).Replace("id=", string.Empty).Trim();

                            }
                            catch (Exception ex)
                            {

                            }

                        }

                        if (FRNURLresponce.Contains("csrfToken"))
                        {
                            try
                            {
                                Val_CsrToken = FRNURLresponce.Substring(FRNURLresponce.IndexOf("csrfToken"), 100);
                                string[] Arr = Val_CsrToken.Split('>'); //Val_CsrToken.Split('&');
                                Val_CsrToken = Arr[0].Replace("csrfToken=", "").Replace("\"", "").Trim();
                            }
                            catch (Exception ex)
                            {
                            }

                        }

                        if (FRNURLresponce.Contains("authToken"))
                        {
                            try
                            {
                             //   Val_AuthToken = FRNURLresponce.Substring(FRNURLresponce.IndexOf("authToken"), 50);
                            //    string[] Arr = Val_AuthToken.Split('&');
                           //     Val_AuthToken = Arr[0].Replace("authToken=", "");
                                int startindex = FRNURLresponce.IndexOf("authToken");
                                string start = FRNURLresponce.Substring(startindex);
                                int endindex = start.IndexOf(",");
                                string end = start.Substring(0, endindex);
                                Val_AuthToken = end.Replace(",", string.Empty).Replace("authToken=", string.Empty).Replace("\"", string.Empty).Trim();
                                if (Val_AuthToken.Contains("&"))
                                {
                                    string[] Arr = Val_AuthToken.Split('&');
                                    Val_AuthToken = Arr[0].Replace("authToken=", string.Empty);
                                }
                                if (Val_AuthToken.Contains("</noscript>"))
                                {
                                    try
                                    {
                                        string[] Val_Auth = Regex.Split(Val_AuthToken, ">");
                                        Val_AuthToken = Val_Auth[0].Replace("\"", string.Empty);
                                    }
                                    catch { }
                                }
                                // Val_AuthToken="2p6r";
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        if (FRNURLresponce.Contains("authType"))
                        {
                            try
                            {
                                Val_AuthType = FRNURLresponce.Substring(FRNURLresponce.IndexOf("authType"), 50);
                                string[] Arr = Val_AuthType.Split('&');
                                Val_AuthType = Arr[0].Replace("authType=", "");
                                if (Val_AuthType.Contains("</noscript>"))
                                {
                                    Val_AuthType = Val_AuthType.Replace("</noscript>", string.Empty).Replace("\n", string.Empty).Replace(">", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Trim();
                                }
                                try
                                {
                                    if (string.IsNullOrEmpty(Val_AuthType))
                                    {
                                        Val_AuthType = "OUT_OF_NETWORK";
                                    }

                                }
                                catch { }
                                //authType=
                            }
                            catch (Exception ex)
                            {

                            }
                        }


                        if (FRNURLresponce.Contains("goback"))
                        {
                            try
                            {
                                Val_goback = FRNURLresponce.Substring(FRNURLresponce.IndexOf("goback"), 300);
                                string[] Arr = Val_goback.Split('"');
                                Val_goback = Arr[0].Replace("goback=", "");
                            }
                            catch (Exception ex)
                            {


                            }
                        }
                        if (FRNURLresponce.Contains("trk"))
                        {
                            try
                            {
                                val_trk = FRNURLresponce.Substring(FRNURLresponce.IndexOf("trk"), 100);
                                string[] Arr = val_trk.Split(',');
                                val_trk = Arr[0].Replace("trk=", "").Replace("\"", string.Empty);
                            }
                            catch (Exception ex)
                            {


                            }
                        }

                        if (FRNURLresponce.Contains("lastName"))
                        {
                            try
                            {

                                Val_lastName = FRNURLresponce.Substring(FRNURLresponce.IndexOf("View Profile"), 300);
                                try
                                {
                                    string[] Arr = Val_lastName.Split(':');
                                    Val_lastName = Arr[2].Replace("firstName", "").Replace(",", "").Replace("\"", "").Replace("&#225;", string.Empty).Trim();
                                }
                                catch { }
                            }
                            catch (Exception ex)
                            {


                            }
                        }

                        if (FRNURLresponce.Contains("firstName") || FRNURLresponce.Contains("ShortTitle"))
                        {
                            try
                            {
                                if (FRNURLresponce.Contains("firstName"))
                                {

                                    try
                                    {
                                        Val_firstName = FRNURLresponce.Substring(FRNURLresponce.IndexOf("View Profile"), 300);
                                        string[] Arr = Val_firstName.Split(':');
                                       // Val_firstName = Arr[3].Replace("lastName=", "").Replace("i18n_HEADLINE", string.Empty).Replace("\"", "").Replace(",", string.Empty).Replace("}", string.Empty).Trim();
                                        Val_firstName = Arr[3].Replace("lastName=", "").Replace("i18n_HEADLINE", string.Empty).Replace("\"", "").Replace(",", string.Empty).Replace("}", string.Empty).Replace("link__endpoint", "").Trim();
                                    }
                                    catch { }
                                }
                                else if (FRNURLresponce.Contains("ShortTitle"))
                                {
                                    Val_firstName = FRNURLresponce.Substring(FRNURLresponce.IndexOf("ShortTitle"), 30);
                                    string[] Arr = Val_firstName.Split('"');
                                    Val_firstName = Arr[2].Replace("ShortTitle=", "");
                                }

                            }
                            catch (Exception ex)
                            {


                            }


                        }

                        //Frnd Requset Send Option URl
                        //markAsReadOnClick
                        try
                        {
                            string[] Valuesss = Regex.Split(FRNURLresponce, "markAsReadOnClick");
                            try
                            {
                                Val_goback = Valuesss[1].Substring(Valuesss[1].IndexOf("goback="), (Valuesss[1].IndexOf(",", Valuesss[1].IndexOf("goback=")) - Valuesss[1].IndexOf("goback="))).Replace("goback=", string.Empty).Trim();

                            }
                            catch
                            { }


                        }
                        catch { }
                      
                      //  string thiredResponce = "http://www.linkedin.com/people/invite?from=profile&key=" + Val_key + "&firstName=" + Val_firstName + "&lastName=" + Val_lastName + "&authToken=" + Val_AuthToken + "&authType=" + Val_AuthType + "&goback=" + Val_goback + "&trk=prof-0-sb-connect-button";
                        string thiredResponce = "http://www.linkedin.com/people/invite?from=profile&key=" + Val_key + "&firstName=" + Val_firstName + "&lastName=" + Val_lastName + "&authToken=" + Val_AuthToken + "&authType=" + Val_AuthType + "&csrfToken=" + Val_CsrToken + "&goback=" + Val_goback;
                        string pageResponce2 = HttpHelper.getHtmlfromUrl1(new Uri(thiredResponce));
                      
                        if (pageResponce2.Contains("You and this LinkedIn user don’t know anyone in common"))
                        {
                            string CSVHeader = "UserName" + "," + "SearchKeyword" + "," + "Invitation Sent To" + "," + "InvitationUrl";
                            string[] url = Regex.Split(FrnAcceptUrL, "&authType");
                            string CSV_Content = _UserName + "," + _ConnectSearchKeyword + ", ," + url[0];
                            CSVUtilities.ExportDataCSVFile(CSVHeader, CSV_Content, Globals.path_ConnectionThroughUnkonwPeopleLink);
                            LoggerManageConnection("[ " + DateTime.Now + " ] => [ Failed To Send Invitation From " + _UserName + " ]");
                            valid = false;
                            Isaccountvalid = false;
                            continue;
                        }


                        if (pageResponce2.Contains("sourceAlias"))
                        {
                            try
                            {
                                Val_sourceAlias = pageResponce2.Substring(pageResponce2.IndexOf("sourceAlias"), 100);
                                string[] Arr = Val_sourceAlias.Split('"');
                                Val_sourceAlias = Arr[2];
                            }
                            catch (Exception ex)
                            {


                            }
                        }
                        string[] strNameValue = Regex.Split(pageResponce2, "name=");

                        #region ForspintaxMessage
                        string msgBodyCompose = string.Empty;
                        if (isSpinTaxSearchWithInvite)
                        {
                            try
                            {
                                msgBodyCompose = UpdatelistGreetMessatge[RandomNumberGenerator.GenerateRandom(0, UpdatelistGreetMessatge.Count - 1)];
                               
                            }
                            catch
                            {
                            }
                        }
                        else
                        {
                            msgBodyCompose = MessageWithoutspintax;
                        }
                        #endregion

                        #region DecareVariable By Sanjeev
                        string iweReconnectSubmit = string.Empty;
                        string iweLimitReached = string.Empty;
                        string companyID0 = string.Empty;
                        string companyName0 = string.Empty;
                        //companyName.0
                        string companyID1 = string.Empty;
                        string schoolID = string.Empty;
                        string schoolcountryCode = string.Empty;
                        string schoolprovinceCode = string.Empty;
                        string subject = string.Empty;
                        string defaultText = string.Empty;
                        string csrfToken = string.Empty;
                        string sourceAlias = string.Empty;
                        string goback = string.Empty;
                        string titleIC0 = string.Empty;
                        string greeting = string.Empty;
                        string startYearIC0 = string.Empty;
                        string endYearIC0 = string.Empty;
                        string schoolText = string.Empty;
                        string titleIB0 = string.Empty;
                        string startYearIB0 = string.Empty;
                        string trk = string.Empty;
                        string authType = string.Empty;
                        string otheremail = string.Empty;
                        string firstName = string.Empty;
                        string lastName = string.Empty;
                        #endregion


                        #region
                        try
                        {
                            string[] ArrForValue = Regex.Split(pageResponce2, "name=\"authType");
                            string strValue = (ArrForValue[1].Substring(ArrForValue[1].IndexOf("value="), ArrForValue[1].IndexOf("id=", ArrForValue[1].IndexOf("value=")) - ArrForValue[1].IndexOf("value=")).Replace("value=", string.Empty).Replace("\\\"", string.Empty).Replace("\"", string.Empty).Trim());
                            authType = (strValue);

                        }
                        catch { }
                        #endregion
                        #region
                        try
                        {
                            string[] ArrForValue = Regex.Split(pageResponce2, "name=\"authType");
                            string strValue = (ArrForValue[1].Substring(ArrForValue[1].IndexOf("value="), ArrForValue[1].IndexOf("id=", ArrForValue[1].IndexOf("value=")) - ArrForValue[1].IndexOf("value=")).Replace("value=", string.Empty).Replace("\\\"", string.Empty).Replace("\"", string.Empty).Trim());
                            authType = (strValue);

                        }
                        catch { }
                        #endregion
                        #region
                        try
                        {
                            string[] ArrForValue = Regex.Split(pageResponce2, "name=\"trk");
                            int tempstartIndex = ArrForValue[1].IndexOf("id=\"");
                            string strValue = ArrForValue[1].Substring(tempstartIndex).Replace("id=\"", "");
                            strValue = strValue.Substring(0, strValue.IndexOf("\""));
                            // string strValue = (ArrForValue[1].Substring(ArrForValue[1].IndexOf("type="), ArrForValue[1].IndexOf("id=", ArrForValue[1].IndexOf("type=")) - ArrForValue[1].IndexOf("type=")).Replace("type=", string.Empty).Replace("\\\"", string.Empty).Replace("\"", string.Empty).Trim());
                            trk = (strValue).Trim();

                        }
                        catch { }
                        #endregion

                        //schoolText titleIB.0=

                        #region
                        try
                        {
                            string[] ArrForValue = Regex.Split(pageResponce2, "name=\"subject");
                            string strValue = (ArrForValue[1].Substring(ArrForValue[1].IndexOf("value="), ArrForValue[1].IndexOf("id=", ArrForValue[1].IndexOf("value=")) - ArrForValue[1].IndexOf("value=")).Replace("value=", string.Empty).Replace("\\\"", string.Empty).Replace("\"", string.Empty).Trim());
                            subject = (strValue);
                        }
                        catch { }
                        #endregion
                        #region
                        try
                        {
                            string[] ArrForValue = Regex.Split(pageResponce2, "name=\"defaultText");
                            string strValue = (ArrForValue[1].Substring(ArrForValue[1].IndexOf("value="), ArrForValue[1].IndexOf("id=", ArrForValue[1].IndexOf("value=")) - ArrForValue[1].IndexOf("value=")).Replace("value=", string.Empty).Replace("\\\"", string.Empty).Replace("\"", string.Empty).Trim());
                            defaultText = (strValue);
                        }
                        catch { }
                        #endregion
                        #region
                        try
                        {
                            string[] ArrForValue = Regex.Split(pageResponce2, "name=\"csrfToken");
                            string strValue = (ArrForValue[1].Substring(ArrForValue[1].IndexOf("value="), ArrForValue[1].IndexOf("id=", ArrForValue[1].IndexOf("value=")) - ArrForValue[1].IndexOf("value=")).Replace("value=", string.Empty).Replace("\\\"", string.Empty).Replace("\"", string.Empty).Trim());
                            csrfToken = (strValue);
                        }
                        catch { }
                        #endregion
                        #region
                        try
                        {
                            string[] ArrForValue = Regex.Split(pageResponce2, "name=\"sourceAlias");
                            string strValue = (ArrForValue[1].Substring(ArrForValue[1].IndexOf("value="), ArrForValue[1].IndexOf("id=", ArrForValue[1].IndexOf("value=")) - ArrForValue[1].IndexOf("value=")).Replace("value=", string.Empty).Replace("\\\"", string.Empty).Replace("\"", string.Empty).Trim());
                            sourceAlias = (strValue);
                        }
                        catch { }
                        #endregion
                        #region
                        try
                        {
                            string[] ArrForValue = Regex.Split(pageResponce2, "name=\"goback");
                            string strValue = (ArrForValue[1].Substring(ArrForValue[1].IndexOf("value="), ArrForValue[1].IndexOf("id=", ArrForValue[1].IndexOf("value=")) - ArrForValue[1].IndexOf("value=")).Replace("value=", string.Empty).Replace("\\\"", string.Empty).Replace("\"", string.Empty).Trim());
                            goback = (strValue).Replace("&quot;", "%22").Replace(":", "%3A").Replace(",", "%2C").Replace("/", "%2F").Replace("{", "%7B").Replace(" ", "+");
                        }
                        catch { }
                        #endregion

                        #region
                        try
                        {
                            string[] ArrForValue = Regex.Split(pageResponce2, "name=\"otherEmail");
                            string strValue = (ArrForValue[1].Substring(ArrForValue[1].IndexOf("value="), ArrForValue[1].IndexOf("id=", ArrForValue[1].IndexOf("value=")) - ArrForValue[1].IndexOf("value=")).Replace("value=", string.Empty).Replace("\\\"", string.Empty).Replace("\"", string.Empty).Trim());
                            otheremail = (strValue);
                        }
                        catch { }
                        #endregion
                        #region
                        try
                        {
                            string[] ArrForValue = Regex.Split(pageResponce2, "name=\"iweLimitReached");
                            string strValue = (ArrForValue[1].Substring(ArrForValue[1].IndexOf("value="), ArrForValue[1].IndexOf("id=", ArrForValue[1].IndexOf("value=")) - ArrForValue[1].IndexOf("value=")).Replace("value=", string.Empty).Replace("\\\"", string.Empty).Replace("\"", string.Empty).Trim());
                            iweLimitReached = (strValue);
                        }
                        catch { }
                        #endregion
                        #region
                        try
                        {
                            try
                            {
                                string[] ArrForValue = Regex.Split(pageResponce2, "name=\"iweReconnectSubmit");
                                string strValue = (ArrForValue[1].Substring(ArrForValue[1].IndexOf("value="), ArrForValue[1].IndexOf("class=", ArrForValue[1].IndexOf("value=")) - ArrForValue[1].IndexOf("value=")).Replace("value=", string.Empty).Replace("\\\"", string.Empty).Replace("\"", string.Empty).Trim());
                                iweReconnectSubmit = (strValue);
                            }
                            catch { }
                            try
                            {
                                if (iweReconnectSubmit.Contains(">") || iweReconnectSubmit.Contains(">"))
                                {
                                    iweReconnectSubmit = "Send Invitation";
                                }
                            }
                            catch { }
                        }
                        catch { }
                        #endregion
                        #region
                        try
                        {
                            try
                            {
                                #region greetmessageparse
                                //string[] ArrForValue = Regex.Split(pageResponce2, "name=\"greeting");
                                //string strValue = (ArrForValue[1].Substring(ArrForValue[1].IndexOf("class=\"message\">"), ArrForValue[1].IndexOf("<", ArrForValue[1].IndexOf("class=\"message\">")) - ArrForValue[1].IndexOf("class=\"message\">")).Replace("class=\"message\">", string.Empty).Replace("\\\"", string.Empty).Replace("\"", string.Empty).Trim());
                                //greeting = (strValue);
                                //greeting = greeting.Replace(" ", "+");
                                //greeting = Uri.EscapeUriString(greeting);
                                //greeting = greeting.Remove(greeting.IndexOf("@"));  //added new 
                                #endregion
                                greeting = msgBodyCompose.Replace(",", "+").Replace(" ", "+").Replace("++", "+");

                            }
                            catch { }
                            try
                            {
                                if (iweReconnectSubmit.Contains(">") || iweReconnectSubmit.Contains(">"))
                                {
                                    //iweReconnectSubmit = "Send Invitation";
                                }
                            }
                            catch { }
                        }
                        catch { }
                        #endregion

                        #region
                        try
                        {
                            string[] ArrForValue = Regex.Split(pageResponce2, "name=\"firstName");
                            string strValue = (ArrForValue[1].Substring(ArrForValue[1].IndexOf("value="), ArrForValue[1].IndexOf("id=", ArrForValue[1].IndexOf("value=")) - ArrForValue[1].IndexOf("value=")).Replace("value=", string.Empty).Replace("\\\"", string.Empty).Replace("\"", string.Empty).Trim());
                            firstName = (strValue).Replace("&#225;", string.Empty);
                        }
                        catch { }
                        #endregion
                        #region
                        try
                        {
                            string[] ArrForValue = Regex.Split(pageResponce2, "name=\"lastName");
                            string strValue = (ArrForValue[1].Substring(ArrForValue[1].IndexOf("value="), ArrForValue[1].IndexOf("id=", ArrForValue[1].IndexOf("value=")) - ArrForValue[1].IndexOf("value=")).Replace("value=", string.Empty).Replace("\\\"", string.Empty).Replace(",", ";").Replace("\"", string.Empty).Trim());
                            lastName = (strValue);
                        }
                        catch { }
                        #endregion
                      /*  #region
                        try
                        {
                            try
                            {
                                string[] ArrForValue = Regex.Split(pageResponce2, "name=\"greeting");
                                string strValue = (ArrForValue[1].Substring(ArrForValue[1].IndexOf("class=\"message\">"), ArrForValue[1].IndexOf("<", ArrForValue[1].IndexOf("class=\"message\">")) - ArrForValue[1].IndexOf("class=\"message\">")).Replace("class=\"message\">", string.Empty).Replace("\\\"", string.Empty).Replace("\"", string.Empty).Trim());
                                greeting = (strValue);
                                greeting = greeting.Replace("&#39;", "'");
                                greeting = greeting.Replace(" ", "+");
                                greeting = Uri.EscapeUriString(greeting);
                                greeting = greeting.Remove(greeting.IndexOf("@"));  //added new
                            }
                            catch { }
                            try
                            {
                                if (iweReconnectSubmit.Contains(">") || iweReconnectSubmit.Contains(">"))
                                {
                                    //iweReconnectSubmit = "Send Invitation";
                                }
                            }
                            catch { }
                        }
                        catch { }
                        #endregion */


                        string lastPostUrl = "http://www.linkedin.com/people/iweReconnectAction";

                        //
                        //*********post request for last process of send request 

                        string NameOfSender = _UserName.Remove(_UserName.IndexOf("@"));
                        // tab-name username">
                        string SenderName = string.Empty;
                        try
                        {
                          //  SenderName = FRNURLresponce.Substring(FRNURLresponce.IndexOf("tab-name username\">"), (FRNURLresponce.IndexOf("<", FRNURLresponce.IndexOf("tab-name username\">")) - FRNURLresponce.IndexOf("tab-name username\">"))).Replace("tab-name username\">", string.Empty).Trim().Replace("tab-name username\">", string.Empty);
                            int startindex = FRNURLresponce.IndexOf("class=\"act-set-name-split-link\">");
                            string start = FRNURLresponce.Substring(startindex).Replace("class=\"act-set-name-split-link\">", string.Empty);
                            int endindex = start.IndexOf("</a>");
                            string end = start.Substring(0,endindex).Replace("</a>",string.Empty);
                            NameOfSender = end.Trim();
                         //   NameOfSender = Uri.EscapeUriString(SenderName);
                        }
                        catch { }
                        greeting = greeting.Replace("<FIRSTNAME>", Val_firstName).Replace("<PROFILEFIRSTNAME>", NameOfSender);
                  
                    //    greeting = greeting.Replace("<FIRSTNAME>", firstName).Replace("<PROFILEFIRSTNAME>", NameOfSender);

                       // string LastPostData = "companyName.0=" + companyName0 + "titleIC.0=" + titleIC0 + "&startYearIC.0=" + startYearIC0 + "&endYearIC.0=" + endYearIC0 + "&schoolText=" + schoolText + "&schoolID=" + schoolID + "&companyName.1=" + companyName0 + "&titleIB.0=" + titleIB0 + "&startYearIB.0=" + startYearIB0 + "&endYearIB.0=" + endYearIC0 + "&reason=IF&otherEmail=" + otheremail + "&greeting=" + greeting + "&iweReconnectSubmit=" + iweReconnectSubmit.Replace(" ", "+") + "&key=" + Val_key + "&firstName=" + firstName + "&lastName=" + lastName + "&authToken=" + Val_AuthToken + "&authType=" + Val_AuthType + "&trk=" + trk + "&iweLimitReached=false&companyID.0=" + companyID0 + "&companyID.1=" + companyID1 + "&schoolID=" + schoolID + "&schoolcountryCode=" + schoolcountryCode + "&schoolprovinceCode=" + schoolprovinceCode + "&javascriptEnabled=false&subject=" + subject.Replace(" ", "+") + "&defaultText=" + defaultText + "&csrfToken=" + Val_CsrToken + "&sourceAlias=" + sourceAlias + "&goback=" + goback;
                        string LastPostData = "companyName.0=" + companyName0 + "titleIC.0=" + titleIC0 + "&startYearIC.0=" + startYearIC0 + "&endYearIC.0=" + endYearIC0 + "&schoolText=" + schoolText + "&schoolID=" + schoolID + "&companyName.1=" + companyName0 + "&titleIB.0=" + titleIB0 + "&startYearIB.0=" + startYearIB0 + "&endYearIB.0=" + endYearIC0 + "&reason=IF&otherEmail=" + otheremail + "&greeting=" + greeting + "&iweReconnectSubmit=" + iweReconnectSubmit.Replace(" ", "+") + "&key=" + Val_key + "&firstName=" + firstName + "&lastName=" + lastName + "&authToken=" + Val_AuthToken + "&authType=" + Val_AuthType + "&trk=" + trk + "&iweLimitReached=false&companyID.0=" + companyID0 + "&companyID.1=" + companyID1 + "&schoolID=" + schoolID + "&schoolcountryCode=" + schoolcountryCode + "&schoolprovinceCode=" + schoolprovinceCode + "&javascriptEnabled=false&subject=" + subject.Replace(" ", "+") + "&defaultText=" + defaultText + "&csrfToken=" + Val_CsrToken + "&sourceAlias=" + sourceAlias + "&goback=" + Val_goback;
                        string postResponse1 = HttpHelper.postFormDataProxy(new Uri(lastPostUrl), LastPostData, _ProxyAddress, proxyport, _ProxyUserName, _ProxyPassword);
                        //Esuccess
                        string FinalValue = string.Empty;
                        try
                        {
                            FinalValue = postResponse1.Substring(postResponse1.IndexOf("Esuccess"), (postResponse1.IndexOf(">", postResponse1.IndexOf("Esuccess")) - postResponse1.IndexOf("Esuccess"))).Replace("Esuccess", string.Empty).Trim();
                            FinalValue = Uri.UnescapeDataString(FinalValue);
                            string[] valuess = Regex.Split(FinalValue, ",");
                            FinalValue = valuess[0].Replace("=", string.Empty).Replace("\"", string.Empty);

                        }
                        catch { }
                        try
                        {
                            string lastGetResponse = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/people/pymk?goback=" + Val_goback + "&trk=" + val_trk + "&report%2Esuccess=" + FinalValue));
                        }
                        catch { }
                        if (postResponse1.Contains("You must confirm your primary email address before sending an invitation."))
                        {
                            LoggerManageConnection("[ " + DateTime.Now + " ] => [ " + _UserName + "You must confirm your primary email address before sending an invitation. ]");
                            valid = false;
                            PageNumber = 0;
                            Isaccountvalid = false;
                            break;
                        }

                        if ((!(pageResponce2.Contains("emailAddress-invitee-invitation"))) && (!(pageResponce2.Contains("Why do I have to enter an email address when I send an Invitation?"))))
                        {

                            //else if (postResponse1.Contains("<strong>Invitation to"))
                            //  if (postResponse1.Contains("<strong>Invitation to"))
                            if (postResponse1.Contains("<strong>Invitations") || postResponse1.Contains("Invitation sent"))
                            {
                                if (string.IsNullOrEmpty(firstName) && string.IsNullOrEmpty(lastName))
                                {
                                    firstName = "linkedin";
                                    lastName = "Member";
                                }
                                if (!(postResponse1.Contains("Your invitation was not sent")))
                                {
                                    clsLDAccount objclsFBAccount = new clsLDAccount();
                                    LoggerManageConnection("[ " + DateTime.Now + " ] => [ Invitation to " + firstName + " " + lastName + " sent From Account :" + _UserName + " ]");
                                    string CSVHeader = "UserName" + "," + "SearchKeyword" + "," + "Invitation Sent To" + "," + "InvitationUrl";
                                    try
                                    {
                                        string[] url = Regex.Split(FrnAcceptUrL, "&authType");
                                        string CSV_Content = _UserName + "," + _ConnectSearchKeyword + "," + firstName + " " + lastName + "," + url[0];
                                        CSVUtilities.ExportDataCSVFile(CSVHeader, CSV_Content, Globals.path_AddConnectionSuccessWith2ndDegree);
                                        objclsFBAccount.InsertSerachWithInviteRecord(_UserName, _ConnectSearchKeyword, firstName + " " + lastName, url[0], url[0].Replace("http://www.linkedin.com/profile/view?id=", "").Trim());
                                        valid = true;
                                        SendInvitationCount++;
                                        int Delay = RandomNumberGenerator.GenerateRandom(DelayStart, DelayEnd);
                                        LoggerManageConnection("[ " + DateTime.Now + " ] => [ Invitation Delayed For : " + Delay + " Seconds ]");
                                        Thread.Sleep(Delay * 1000);
                                    }
                                    catch { }  
                                
                                }
                                
                            }
                            else if (postResponse1.Contains("You and this LinkedIn user don’t know anyone in common"))
                            {
                                string CSVHeader = "UserName" + "," + "SearchKeyword" + "," + "Invitation Sent To" + "," + "InvitationUrl";
                                string[] url = Regex.Split(FrnAcceptUrL, "&authType");
                                string CSV_Content = _UserName + "," + _ConnectSearchKeyword + ", ," + url[0];
                                CSVUtilities.ExportDataCSVFile(CSVHeader, CSV_Content, Globals.path_ConnectionThroughUnkonwPeopleLink);
                                LoggerManageConnection("[ " + DateTime.Now + " ] => [ Failed To Send Invitation From " + _UserName + " ]");
                                valid = false;
                                Isaccountvalid = false;
                            }
                            else if (postResponse1.Contains("Your invitation was not sent"))
                            {
                                string CSVHeader1 = "UserName" + "," + "SearchKeyword" + "," + "Invitation Sent To" + "," + "InvitationUrl";
                                string[] url = Regex.Split(FrnAcceptUrL, "&authType");
                                string CSV_Content = _UserName + "," + _ConnectSearchKeyword + "," + firstName + " " + lastName + "," + url[0];
                                CSVUtilities.ExportDataCSVFile(CSVHeader1, CSV_Content, Globals.path_ConnectionthroughKeywordSearchnotadded);
                                LoggerManageConnection("[ " + DateTime.Now + " ] => [ Failed To Send Invitation From " + _UserName + " ]");
                                valid = false;
                                //   Isaccountvalid = false;
                               // SendInvitationCount--;
                                
                            }
                            else
                            {
                                string CSVHeader = "UserName" + "," + "SearchKeyword" + "," + "Invitation Sent To" + "," + "InvitationUrl";
                                string[] url = Regex.Split(FrnAcceptUrL, "&authType");
                                string CSV_Content = _UserName + "," + _ConnectSearchKeyword + "," + firstName + " " + lastName + "," + url[0];
                                CSVUtilities.ExportDataCSVFile(CSVHeader, CSV_Content, Globals.path_ConnectionthroughKeywordSearchnotadded);
                                LoggerManageConnection("[ " + DateTime.Now + " ] => [ Failed To Send Invitation From " + _UserName + " ]");
                                valid = false;
                                //   Isaccountvalid = false;
                               //SendInvitationCount--;
                            }

                        }
                       

                            //
                     /*   else if (postResponse1.Contains("<strong>Invitation to"))
                        {
                            if (string.IsNullOrEmpty(firstName) && string.IsNullOrEmpty(lastName))
                            {
                                firstName = "linkedin";
                                lastName = "Member";
                            } 
                            clsLDAccount objclsFBAccount = new clsLDAccount();

                            LoggerManageConnection("[ " + DateTime.Now + " ] => [ Invitation to " + firstName + " " + lastName + " sent From Account :" + _UserName + " ]");
                            string CSVHeader = "UserName" + "," + "SearchKeyword" + "," + "Invitation Sent To" + "," + "InvitationUrl";
                            try
                            {
                                string[] url = Regex.Split(FrnAcceptUrL, "&authType");
                                string CSV_Content = _UserName + "," + _ConnectSearchKeyword + "," + firstName + " " + lastName + "," + url[0];
                                CSVUtilities.ExportDataCSVFile(CSVHeader, CSV_Content, Globals.path_AddConnectionSuccessWith2ndDegree);
                                objclsFBAccount.InsertSerachWithInviteRecord(_UserName, _ConnectSearchKeyword, firstName + " " + lastName, url[0], url[0].Replace("http://www.linkedin.com/profile/view?id=", "").Trim());
                                valid = true;
                            }
                            catch { }
                        }
                        else if (postResponse1.Contains("You and this LinkedIn user don’t know anyone in common"))
                        {
                            string CSVHeader = "UserName" + "," + "SearchKeyword" + "," + "Invitation Sent To" + "," + "InvitationUrl";
                            string[] url = Regex.Split(FrnAcceptUrL, "&authType");
                            string CSV_Content = _UserName + "," + _ConnectSearchKeyword + ", ," + url[0];
                            CSVUtilities.ExportDataCSVFile(CSVHeader, CSV_Content, Globals.path_ConnectionThroughUnkonwPeopleLink);
                            LoggerManageConnection("[ " + DateTime.Now + " ] => [ Failed To Send Invitation From " + _UserName + " ]");
                            valid = false;
                            Isaccountvalid = false;
                        }
                        else
                        {
                            string CSVHeader = "UserName" + "," + "SearchKeyword" + "," + "Invitation Sent To" + "," + "InvitationUrl";
                            string[] url = Regex.Split(FrnAcceptUrL, "&authType");
                            string CSV_Content = _UserName + "," + _ConnectSearchKeyword + "," + firstName + " " + lastName + "," + url[0];
                            CSVUtilities.ExportDataCSVFile(CSVHeader, CSV_Content, Globals.path_ConnectionthroughKeywordSearchnotadded);
                            LoggerManageConnection("[ " + DateTime.Now + " ] => [ Failed To Send Invitation From " + _UserName + " ]");
                            valid = false;
                            Isaccountvalid = false;
                        }
                        */

                        //int Delay = RandomNumberGenerator.GenerateRandom(SearchMinDelay, SeacrhMaxDelay);
                        //LoggerManageConnection("[ " + DateTime.Now + " ] => [ Invitation Delayed For : " + Delay + " Seconds ]");
                        //Thread.Sleep(Delay * 1000);

                     //   if (!(valid == false))
                  //      {
                            //SendInvitationCount++;
                        
                        {
                            if (SendInvitationCount >= SearchCriteria.NumberOfRequestPerKeyword)  //Here we check  that number of request send if counter is equal to the number of request put by user then it return to mathod and made pagenumber = 0; 
                            {  //SendInvitationCount--;
                                PageNumber = 0;

                                return;
                            }
                        }
                             
                      //  }
                    }
                   else
                   {
                      LoggerManageConnection("[ " + DateTime.Now + " ] => [ Invitation already sent from" + _UserName + " ]");
                  }
                }
                catch (Exception ex)
                {
                    string CSVHeader = "UserName" + "," + "SearchKeyword" + "," + "Invitation Sent To" + "," + "InvitationUrl";
                    string[] url = Regex.Split(FrnAcceptUrL, "&authType");
                    string CSV_Content = _UserName + "," + _ConnectSearchKeyword + ",," + url[0];
                    CSVUtilities.ExportDataCSVFile(CSVHeader, CSV_Content, Globals.path_ConnectionthroughKeywordSearchnotadded);
                    LoggerManageConnection("[ " + DateTime.Now + " ] => [ Failed To Send Invitation From " + _UserName + " ]");
                    valid = false;
                    Isaccountvalid = false;
                }
            }
        }
        #endregion

        #region ConnectionSearchPymknow()
        public void ConnectionSearchPymKnow()
        {
            try
            {
                //int PageNumber = 0;
                string strPageNumber = string.Empty;
                string Textmessage = string.Empty;

                if (linkedinLoginAndLogout.LoginHttpHelper(_UserName, _Password, _ProxyAddress, _ProxyPort, _ProxyUserName, _ProxyPassword, ref HttpHelper, ref Textmessage))
                {
                    // LoggerManageConnection("Logged in with " + _UserName);

                    int proxyport = 888;
                    string SummaryLink = string.Empty;

                    Regex PortCheck = new Regex("^[0-9]*$");

                    if (PortCheck.IsMatch(_ProxyPort) && !string.IsNullOrEmpty(_ProxyPort))
                    {
                        proxyport = int.Parse(_ProxyPort);
                    }

                    string Url = "http://www.linkedin.com/people/pymk?trk=frontier-tabs_connections-new_pymk";// "http://www.linkedin.com/search?search="+_ConnectSearchKeyword+"&sortCriteria=Distance_Relevance&viewCriteria=2&excoon=true&newnessType=M&searchLocationType=Y&proposalType=Y&trk=nmp_profile_network_stats_new_people#facets=searchLocationType%3DY%26inNetworkSearch%3DinNetworkSearch%26page_num%3D1%26search%3D%26viewCriteria%3D2%26facetsOrder%3DCC%252CN%252CG%252CI%252CPC%252CED%252CL%252CFG%252CTE%252CFA%252CSE%252CP%252CCS%252CF%252CDR%26sortCriteria%3DC%26clickAction%3Dsort%26openFacets%3DN%252CCC%252CG";
                    string PgSrcMain = HttpHelper.getHtmlfromUrlProxy(new Uri(Url), _ProxyAddress, proxyport, _ProxyUserName, _ProxyPassword);
                    ConnectPageSearchPymKnow();

                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        #endregion

        #region ConnectPageSearchPymknow
        public void ConnectPageSearchPymKnow()
        {
            int proxyport = 888;
            string SummaryLink = string.Empty;

            Regex PortCheck = new Regex("^[0-9]*$");

            if (PortCheck.IsMatch(_ProxyPort) && !string.IsNullOrEmpty(_ProxyPort))
            {
                proxyport = int.Parse(_ProxyPort);
            }
            string UrlConnectpage = "http://www.linkedin.com/people/pymk?trk=frontier-tabs_connections-new_pymk";

            string PgSrcMain1 = HttpHelper.getHtmlfromUrlProxy(new Uri(UrlConnectpage), _ProxyAddress, proxyport, _ProxyUserName, _ProxyPassword);
            //find peopleCount:12
            //find rnd



            string test = "http://www.linkedin.com/people/pymk-people-cards?locale=en_US&records=12&offset=133&facetID=&facetType=&decorate=false&trk=new_pymk-show_more&rnd=1343042316579";
            string PgSrcMain2 = HttpHelper.getHtmlfromUrlProxy(new Uri(test), _ProxyAddress, proxyport, _ProxyUserName, _ProxyPassword);
            string IDdata = string.Empty;
            List<string> lstIDForAddconnection = new List<string>();

            if (PgSrcMain2.Contains("peopleCount\":12"))
            {
                List<string> URLID = Regex.Split(PgSrcMain2, "/profile/connections?").ToList();
                foreach (string item in URLID)
                {
                    if (item.Contains("profileURL"))
                    {
                        //item.Substring(PgSrcMain1.IndexOf("profileURL"), 120);

                        string[] Arr = item.Split('&');
                        IDdata = Arr[0].Replace("?id=", "").Trim();
                        lstIDForAddconnection.Add("http://www.linkedin.com/profile/view?id=" + IDdata);
                    }

                }



                //List<string> PageSerchUrl1 = ChilkatBasedRegex.GettingAllUrls(PgSrcMain2, "/profile/connections");
                //List<string> URLID = Regex.Split(PgSrcMain2, "/profile/connections?").ToList();
                #region Declaration
                string PostData = string.Empty;
                string page_num = string.Empty;
                string postUrl = string.Empty;
                string facetsOrder = string.Empty;



                //Diclaration For Post Add Connection For Keyword
                string Val_goback = string.Empty;
                string Val_sourceAlias = string.Empty;
                string Val_key = string.Empty;
                string Val_defaultText = string.Empty;
                string Val_firstName = string.Empty;
                string Val_CsrToken = string.Empty;
                string Val_Subject = string.Empty;
                string Val_greeting = string.Empty;
                string Val_AuthToken = string.Empty;
                string Val_AuthType = string.Empty;
                string val_trk = string.Empty;
                string Val_lastName = string.Empty;

                string Rnd = string.Empty;
                #endregion

                //Post Response
                // string postResponse = HttpHelper.postFormDataProxy(new Uri(postUrl), PostData, _ProxyAddress, proxyport, _ProxyUserName, _ProxyPassword);
                List<string> PageSerchUrl = ChilkatBasedRegex.GettingAllUrls(PgSrcMain1, "profile/view?id");
                string FrnAcceptUrL = string.Empty;
                #region parsing
                foreach (string item in lstIDForAddconnection)
                {
                    if (item.Contains("http://www.linkedin.com/profile/view?id="))
                    {
                        FrnAcceptUrL = item;


                        string FRNURLresponce = HttpHelper.getHtmlfromUrl1(new Uri(FrnAcceptUrL));

                        if (FRNURLresponce.Contains("rnd"))
                        {
                            try
                            {
                                Rnd = FRNURLresponce.Substring(FRNURLresponce.IndexOf("rnd"), 100);
                                string[] Arr = Rnd.Split('&');
                                Rnd = Arr[0].Replace("rnd=", "");
                            }
                            catch (Exception ex)
                            {


                            }

                        }


                        if (FRNURLresponce.Contains("http://www.linkedin.com/profile/view?id="))
                        {
                            try
                            {
                                Val_key = FRNURLresponce.Substring(FRNURLresponce.IndexOf("/profile/view?id="), 30);
                                string[] Arr = Val_key.Split('&');
                                Val_key = Arr[0].Replace("/profile/view?id=", "");
                            }
                            catch (Exception ex)
                            {


                            }

                        }

                        if (FRNURLresponce.Contains("csrfToken"))
                        {
                            try
                            {
                                Val_CsrToken = FRNURLresponce.Substring(FRNURLresponce.IndexOf("csrfToken"), 100);
                                string[] Arr = Val_CsrToken.Split('&');
                                Val_CsrToken = Arr[0].Replace("csrfToken=", "");
                            }
                            catch (Exception ex)
                            {


                            }

                        }




                        if (FRNURLresponce.Contains("authToken"))
                        {
                            try
                            {
                                Val_AuthToken = FRNURLresponce.Substring(FRNURLresponce.IndexOf("authToken"), 50);
                                string[] Arr = Val_AuthToken.Split('"');
                                Val_AuthToken = Arr[0].Replace("authToken=", "");
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        if (FRNURLresponce.Contains("authType"))
                        {
                            try
                            {
                                Val_AuthType = FRNURLresponce.Substring(FRNURLresponce.IndexOf("authType"), 50);
                                string[] Arr = Val_AuthType.Split('&');
                                Val_AuthType = Arr[0].Replace("authType=", "");
                            }
                            catch (Exception ex)
                            {

                            }
                        }


                        if (FRNURLresponce.Contains("goback"))
                        {
                            try
                            {
                                Val_goback = FRNURLresponce.Substring(FRNURLresponce.IndexOf("goback"), 300);
                                string[] Arr = Val_goback.Split('"');
                                Val_goback = Arr[0].Replace("goback=", "");
                            }
                            catch (Exception ex)
                            {


                            }
                        }
                        if (FRNURLresponce.Contains("trk"))
                        {
                            try
                            {
                                val_trk = FRNURLresponce.Substring(FRNURLresponce.IndexOf("trk"), 100);
                                string[] Arr = val_trk.Split(';');
                                val_trk = Arr[0].Replace("trk=", "");
                            }
                            catch (Exception ex)
                            {


                            }
                        }

                        if (FRNURLresponce.Contains("lastName"))
                        {
                            try
                            {
                                Val_lastName = FRNURLresponce.Substring(FRNURLresponce.IndexOf("lastName"), 30);
                                string[] Arr = Val_lastName.Split('&');
                                Val_lastName = Arr[0].Replace("lastName=", "");
                            }
                            catch (Exception ex)
                            {


                            }
                        }

                        if (FRNURLresponce.Contains("firstName"))
                        {
                            try
                            {
                                Val_firstName = FRNURLresponce.Substring(FRNURLresponce.IndexOf("firstName"), 30);
                                string[] Arr = Val_firstName.Split('&');
                                Val_firstName = Arr[0].Replace("firstName=", "");
                            }
                            catch (Exception ex)
                            {


                            }



                        }


                #endregion                    //Frnd Requset Send Option URl
                        //http://www.linkedin.com/people/pymk-people-cards?locale=en_US&records=12&offset=13&facetID=&facetType=&decorate=false&trk=new_pymk-show_more&rnd=1343134697855
                        //



                        string thiredResponce = "http://www.linkedin.com/people/invite?from=profile&key=" + IDdata + "&firstName=" + Val_firstName + "&lastName=" + Val_lastName + "&authToken=" + Val_AuthToken + "&authType=" + Val_AuthType + "&goback=" + Val_goback + "%2Enpv_141450_*1_*1_OPENLINK_d7Ir_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1&trk=prof-0-sb-connect-button";
                        string SecondUrl = "http://www.linkedin.com/people/pymk-people-cards?locale=en_US&records=12&offset=37&facetID=&facetType=&decorate=false&trk=new_pymk-show_more&rnd=1343134804144";
                        string pageResponce2 = HttpHelper.getHtmlfromUrl1(new Uri(SecondUrl));

                        if (pageResponce2.Contains("sourceAlias"))
                        {
                            try
                            {
                                Val_sourceAlias = pageResponce2.Substring(pageResponce2.IndexOf("sourceAlias"), 100);
                                string[] Arr = Val_sourceAlias.Split('"');
                                Val_sourceAlias = Arr[2];
                            }
                            catch (Exception ex)
                            {


                            }
                        }


                        string lastPostUrl = "http://www.linkedin.com/people/iweReconnectAction";
                        //
                        //existingPositionIC=&companyName.0=&titleIC.0=&startYearIC.0=&endYearIC.0=&schoolText=&schoolID=&existingPositionIB=&companyName.1=&titleIB.0=&startYearIB.0=&endYearIB.0=&reason=IF&otherEmail=&greeting=I%27d+like+to+add+you+to+my+professional+network+on+LinkedIn.%0D%0A%0D%0A-+gaurav+agrawal&iweReconnectSubmit=Send+Invitation&key=141450&firstName=Ron&lastName=Bates&authToken=d7Ir&authType=OPENLINK&trk=prof-0-sb-connect-button&iweLimitReached=false&companyID.0=&companyID.1=&schoolID=&schoolcountryCode=&schoolprovinceCode=&javascriptEnabled=false&subject=gaurav+agrawal+wants+to+connect+on+LinkedIn+&defaultText=a908b3ba18701677a5879c1955035d05&csrfToken=ajax%3A8096156173388039261&                                                            sourceAlias=0_0ZD1VB-f52hFxNsCjPZdLV&goback=.fps_PBCK_*1_*1_*1_*1_*1_*1_*1_*2_*1_Y_*1_*1_*1_false_1_C_*1_*51_*1_*51_true_CC%2CN%2CG%2CI%2CPC%2CED%2CL%2CFG%2CTE%2CFA%2CSE%2CP%2CCS%2CF%2CDR_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2.npv_141450_*1_*1_OPENLINK_d7Ir_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1;
                        string LastPostData = "existingPositionIC=&companyName.0=&titleIC.0=&startYearIC.0=&endYearIC.0=&schoolText=&schoolID=&existingPositionIB=&companyName.1=&titleIB.0=&startYearIB.0=&endYearIB.0=&reason=IF&otherEmail=&greeting=I%27d+like+to+add+you+to+my+professional+network+on+LinkedIn.%0D%0A%0D%0A-+gaurav+agrawal&iweReconnectSubmit=Send+Invitation&key=" + Val_key + "&firstName=" + Val_firstName + "&lastName=" + Val_lastName + "&authToken=" + Val_AuthToken + "&authType=" + Val_AuthType + "&trk=prof-0-sb-connect-button&iweLimitReached=false&companyID.0=&companyID.1=&schoolID=&schoolcountryCode=&schoolprovinceCode=&javascriptEnabled=false&subject=gaurav+agrawal+wants+to+connect+on+LinkedIn+&defaultText=a908b3ba18701677a5879c1955035d05&csrfToken=" + Val_CsrToken + "&sourceAlias=" + Val_sourceAlias + "&goback=" + Val_goback.Replace("%2E", ".") + ".npv_" + Val_key + "_*1_*1_OPENLINK_d7Ir_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1";
                        string postResponse1 = HttpHelper.postFormDataProxy(new Uri(lastPostUrl), LastPostData, _ProxyAddress, proxyport, _ProxyUserName, _ProxyPassword);

                        if (postResponse1.Contains("Accept Invitations"))
                        {
                            LoggerManageConnection("[ " + DateTime.Now + " ] => [ Invitation to " + Val_firstName + " sent. ]");
                        }

                        else
                        {
                            LoggerManageConnection("[ " + DateTime.Now + " ] => [ Failed To Send Invitation To" + Val_firstName + " ]");
                        }


                    }
                }

            }
            else
            {
                LoggerManageConnection("[ " + DateTime.Now + " ] => [ Connection End ]");
            }

        }
        #endregion

        #region LoggerManageConnection
        private void LoggerManageConnection(string message)
        {
            EventsArgs eventArgs = new EventsArgs(message);
            ConnectSearchLogEvents.LogText(eventArgs);
        }
        #endregion

        #region TestConnectionUniqueSearch
        static List<string> AllPageSerchUrl = new List<string>();
        private List<string> TestConnectionUniqueSearch(string pageNumber)
        {
            List<string> PageSerchUrl = new List<string>();
            List<string> lstSearchResult = new List<string>();
            try
            {

                int proxyport = 888;
                string SummaryLink = string.Empty;

                Regex PortCheck = new Regex("^[0-9]*$");

                if (PortCheck.IsMatch(_ProxyPort) && !string.IsNullOrEmpty(_ProxyPort))
                {
                    proxyport = int.Parse(_ProxyPort);
                }

                //recive the page number and then hit page wise and find the url in which we need to send the request.....

                string UrlConnectpage = "http://www.linkedin.com/search/fpsearch?keywords=" + _ConnectSearchKeyword + "&searchLocationType=I&countryCode=in&keepFacets=keepFacets&page_num=" + pageNumber + "&pplSearchOrigin=ADVS&viewCriteria=2&sortCriteria=R&redir=redir";
                //  string UrlConnectpage = "http://www.linkedin.com/search?search=&sortCriteria=Distance_Relevance&viewCriteria=2&excoon=true&newnessType=M&searchLocationType=Y&proposalType=Y&trk=nmp_profile_network_stats_new_people#facets=searchLocationType%3DY%26inNetworkSearch%3DinNetworkSearch%26search%3D%26viewCriteria%3D2%26sortCriteria%3DC%26facetsOrder%3DCC%252CN%252CG%252CI%252CPC%252CED%252CL%252CFG%252CTE%252CFA%252CSE%252CP%252CCS%252CF%252CDR%26page_num%" + pageNumber + "D2%26openFacets%3DN%252CCC%252CG";
                string PgSrcMain1 = HttpHelper.getHtmlfromUrlProxy(new Uri(UrlConnectpage), _ProxyAddress, proxyport, _ProxyUserName, _ProxyPassword);


                string PostData = string.Empty;
                string page_num = string.Empty;
                string postUrl = string.Empty;
                string facetsOrder = string.Empty;

                if (PgSrcMain1.Contains("facetsOrder"))
                {
                    facetsOrder = PgSrcMain1.Substring(PgSrcMain1.IndexOf("facetsOrder"), 200);
                    string[] Arr = facetsOrder.Split('"');
                    facetsOrder = Arr[2];
                    string DecodedCharTest = Uri.UnescapeDataString(facetsOrder);
                    string DecodedEmail = Uri.EscapeDataString(facetsOrder);
                    facetsOrder = DecodedEmail;
                }

                page_num = pageNumber;

                PostData = "searchLocationType=Y&inNetworkSearch=inNetworkSearch&search=&viewCriteria=2&sortCriteria=C&facetsOrder=" + facetsOrder + "&page_num=" + page_num + "&openFacets=N%2CCC%2CG";

                //Post Url for Invite Message 
                postUrl = "http://www.linkedin.com/search/hits";

                //Diclaration For Post Add Connection For Keyword


                //Post Response
                string postResponse = HttpHelper.postFormDataProxy(new Uri(postUrl), PostData, _ProxyAddress, proxyport, _ProxyUserName, _ProxyPassword);
                PageSerchUrl = ChilkatBasedRegex.GettingAllUrls(PgSrcMain1, "profile/view?id");
                foreach (string item in PageSerchUrl)
                {
                    try
                    {
                        if (item.Contains("pp_profile_name_link"))  //this key word can only seprate the Url with all common urls....
                        {
                            try
                            {
                                string FrnAcceptUrL = "http://www.linkedin.com" + item;
                                lstSearchResult.Add(FrnAcceptUrL);
                            }
                            catch { }
                        }
                    }
                    catch { }
                }
                return lstSearchResult;
            }
            catch { }
            return lstSearchResult;


        }
        #endregion

        #region TestConnectPageSearchForUniqueUrl
        Queue<string> lstqueueUrlsForKeywords = new Queue<string>();

        //private void AddConnectionUnique()
        //{
        //    string strPageNumber = string.Empty;
        //    string Textmessage = string.Empty;


        //    //Login with User name and password ***********************

        //    if (linkedinLoginAndLogout.LoginHttpHelper(_UserName, _Password, _ProxyAddress, _ProxyPort, _ProxyUserName, _ProxyPassword, ref HttpHelper, ref Textmessage))
        //    {
        //        LoggerManageConnection("Logged in with " + _UserName);

        //        int proxyport = 888;
        //        string SummaryLink = string.Empty;

        //        Regex PortCheck = new Regex("^[0-9]*$");

        //        if (PortCheck.IsMatch(_ProxyPort) && !string.IsNullOrEmpty(_ProxyPort))
        //        {
        //            proxyport = int.Parse(_ProxyPort);
        //        }

        //        while (UsingKeyWordQueue.Count > 0)
        //        {

        //            lock (locker_InvitataionKeyword)
        //            {
        //                if (UsingKeyWordQueue.Count > 0)
        //                {
        //                    _ConnectSearchKeyword = UsingKeyWordQueue.Dequeue();//pass the keyword to the string
        //                }
        //                else
        //                {
        //                    LoggerManageConnection("All Loaded Keyword are used");
        //                }
        //            }

        //            //for search make a URL with our input and then used with get request********************* 
        //            //if(lstqueueUrlsForKeywords.Count>1)
        //            //{
        //            string Url = "http://www.linkedin.com/search/fpsearch?keywords=" + _ConnectSearchKeyword + "&searchLocationType=I&countryCode=in&keepFacets=keepFacets&page_num=1&pplSearchOrigin=ADVS&viewCriteria=2&sortCriteria=R&redir=redir";
        //            //string Url = "http://www.linkedin.com/search?search="+_ConnectSearchKeyword+"&sortCriteria=Distance_Relevance&viewCriteria=2&excoon=true&newnessType=M&searchLocationType=Y&proposalType=Y&trk=nmp_profile_network_stats_new_people#facets=searchLocationType%3DY%26inNetworkSearch%3DinNetworkSearch%26page_num%3D1%26search%3D%26viewCriteria%3D2%26facetsOrder%3DCC%252CN%252CG%252CI%252CPC%252CED%252CL%252CFG%252CTE%252CFA%252CSE%252CP%252CCS%252CF%252CDR%26sortCriteria%3DC%26clickAction%3Dsort%26openFacets%3DN%252CCC%252CG";
        //            string PgSrcMain = HttpHelper.getHtmlfromUrlProxy(new Uri(Url), _ProxyAddress, proxyport, _ProxyUserName, _ProxyPassword);

        //            string[] Arr = Regex.Split(PgSrcMain, "<li");
        //            foreach (string item in Arr)
        //            {
        //                if (!item.Contains("<!DOCTYPE"))
        //                {
        //                    if (item.Contains("results-summary"))
        //                    {
        //                        string data = RemoveAllHtmlTag.StripHTML(item);
        //                        data = data.Replace("\n", "");
        //                        if (data.Contains(">"))
        //                        {
        //                            string[] ArrTemp = data.Split('>');
        //                            data = ArrTemp[1];
        //                            data = data.Replace("results", "");
        //                            data = data.Trim();
        //                            string[] ArrTemp1 = data.Split(' ');
        //                            data = ArrTemp1[0].Replace(',', ' ').Trim();

        //                            strPageNumber = data.Replace(" ", string.Empty);
        //                            break;
        //                        }

        //                    }
        //                }
        //            }

        //            try
        //            {
        //                PageNumber = int.Parse(strPageNumber);//find the page number for each key word
        //            }
        //            catch (Exception)
        //            {

        //                throw;
        //            }
        //            if (!(PageNumber > 0))
        //            {

        //                TestConnectPageSearch("1");
        //            }

        //            PageNumber = (PageNumber / 10) - 1;//each page have 10 data so number of record divide by 10....

        //            for (int i = 1; i <= PageNumber; i++)
        //            {
        //                //loop for send request
        //                if (!Isaccountvalid)
        //                {
        //                    return;
        //                }

        //                if (true)
        //                {
        //                    List<string> lstUrls = TestConnectionUniqueSearch(i.ToString());
        //                    AllPageSerchUrl.AddRange(lstUrls);
        //                    int Account=Globals.listAccounts.Count;
        //                    int NeedLinkedinUrl = Account * countofKeywords;
        //                    if (AllPageSerchUrl.Count >= NeedLinkedinUrl)
        //                    {
        //                        foreach (string item in AllPageSerchUrl)
        //                        {
        //                            try
        //                            {
        //                                lstqueueUrlsForKeywords.Enqueue(item);
        //                            }
        //                            catch { }
        //                        }
        //                        break;
        //                    }
        //                }
        //                // TestConnectPageSearch(i.ToString());
        //            }



        //           // for (int i = 0; i <= AllPageSerchUrl.Count;i++)
        //            {
        //                try
        //                {
        //                    string item=lstqueueUrlsForKeywords.Dequeue();
        //                    SentAddConnection(item);
        //                }
        //                catch { }
        //            }

        //            if (valid)
        //            {
        //                string CSVHeader = "UserName" + "," + "SearchKeyword";
        //                string CSV_Content = _UserName + "," + _ConnectSearchKeyword;
        //                // CSVUtilities.ExportDataCSVFile(CSVHeader, CSV_Content, Globals.path_AddConnectionFail);
        //                //LoggerManageConnection("*************************************************************************************************");
        //                //LoggerManageConnection("*************************************************************************************************");

        //                //LoggerManageConnection("*************************************************************************************************");
        //                //LoggerManageConnection("*************************************************************************************************");
        //                if (SendInvitationCount > 0)
        //                {

        //                    LoggerManageConnection(SendInvitationCount + "  Send Request Completed With " + _UserName + " and For Keyword " + _ConnectSearchKeyword);
        //                    //GlobusFileHelper.AppendStringToTextfileNewLine(_ConnectSearchKeyword + "  " +_UserName , Globals.path_AddConnection);
        //                    //CSVUtilities.ExportDataCSVFile(CSVHeader, CSV_Content, Globals.path_AddConnectionSuccess);

        //                }
        //                else
        //                {
        //                    CSVUtilities.ExportDataCSVFile(CSVHeader, CSV_Content, Globals.path_AddConnectionFail);
        //                    LoggerManageConnection("Not Able To send Any Connection Request For Keyword " + _ConnectSearchKeyword);
        //                    // GlobusFileHelper.AppendStringToTextfileNewLine(_ConnectSearchKeyword + "  " + _UserName, Globals.path_AddConnection);

        //                }
        //                //LoggerManageConnection("*************************************************************************************************");
        //                //LoggerManageConnection("*************************************************************************************************");
        //                SendInvitationCount = 0;
        //                //LoggerManageConnection("Send Request Completed");
        //            }
        //        }

        //    }
        //    else
        //    {
        //        LoggerManageConnection(_UserName + "Your LinkedIn account has been temporarily restricted");
        //        return;
        //    }

        //}
        //private void SentAddConnection(string FrnAcceptUrL)
        //{

        //    string Val_goback = string.Empty;
        //    string Val_sourceAlias = string.Empty;
        //    string Val_key = string.Empty;
        //    string Val_defaultText = string.Empty;
        //    string Val_firstName = string.Empty;
        //    string Val_CsrToken = string.Empty;
        //    string Val_Subject = string.Empty;
        //    string Val_greeting = string.Empty;
        //    string Val_AuthToken = string.Empty;
        //    string Val_AuthType = string.Empty;
        //    string val_trk = string.Empty;
        //    string Val_lastName = string.Empty;
        //    string FRNURLresponce = HttpHelper.getHtmlfromUrl(new Uri(FrnAcceptUrL));

        //    if (FRNURLresponce.Contains("/profile/view?id="))
        //    {
        //        try
        //        {
        //            Val_key = FRNURLresponce.Substring(FRNURLresponce.IndexOf("/profile/view?id="), 30);
        //            string[] Arr = Val_key.Split('&');
        //            Val_key = Arr[0].Replace("/profile/view?id=", "");
        //        }
        //        catch (Exception ex)
        //        {


        //        }

        //    }

        //    if (FRNURLresponce.Contains("csrfToken"))
        //    {
        //        try
        //        {
        //            Val_CsrToken = FRNURLresponce.Substring(FRNURLresponce.IndexOf("csrfToken"), 100);
        //            string[] Arr = Val_CsrToken.Split('&');
        //            Val_CsrToken = Arr[0].Replace("csrfToken=", "");
        //        }
        //        catch (Exception ex)
        //        {


        //        }

        //    }

        //    if (FRNURLresponce.Contains("authToken"))
        //    {
        //        try
        //        {
        //            Val_AuthToken = FRNURLresponce.Substring(FRNURLresponce.IndexOf("authToken"), 50);
        //            string[] Arr = Val_AuthToken.Split('&');
        //            Val_AuthToken = Arr[0].Replace("authToken=", "");
        //        }
        //        catch (Exception ex)
        //        {

        //        }
        //    }
        //    if (FRNURLresponce.Contains("authType"))
        //    {
        //        try
        //        {
        //            //Val_AuthType = FRNURLresponce.Substring(FRNURLresponce.IndexOf("authType"), 50);
        //            //string[] Arr = Val_AuthType.Split('&');
        //            //Val_AuthType = Arr[0].Replace("authType=", "");
        //            //if (Val_AuthType.Contains("</noscript>"))
        //            //{
        //            //    Val_AuthType = Val_AuthType.Replace("</noscript>", string.Empty).Replace("\n", string.Empty).Replace(">", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Trim();
        //            //}
        //            try
        //            {
        //                Val_AuthType = "OUT_OF_NETWORK";
        //                // Val_AuthType = FRNURLresponce.Substring(FRNURLresponce.IndexOf("authType="), (FRNURLresponce.IndexOf(">", FRNURLresponce.IndexOf("authType=")) - FRNURLresponce.IndexOf("authType="))).Replace("authType=", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Trim();
        //            }
        //            catch { }
        //            //authType=
        //        }
        //        catch (Exception ex)
        //        {

        //        }
        //    }


        //    if (FRNURLresponce.Contains("goback"))
        //    {
        //        try
        //        {
        //            Val_goback = FRNURLresponce.Substring(FRNURLresponce.IndexOf("goback"), 300);
        //            string[] Arr = Val_goback.Split('"');
        //            Val_goback = Arr[0].Replace("goback=", "");
        //        }
        //        catch (Exception ex)
        //        {


        //        }
        //    }
        //    if (FRNURLresponce.Contains("trk"))
        //    {
        //        try
        //        {
        //            val_trk = FRNURLresponce.Substring(FRNURLresponce.IndexOf("trk"), 100);
        //            string[] Arr = val_trk.Split(';');
        //            val_trk = Arr[0].Replace("trk=", "");
        //        }
        //        catch (Exception ex)
        //        {


        //        }
        //    }

        //    if (FRNURLresponce.Contains("lastName"))
        //    {
        //        try
        //        {
        //            Val_lastName = FRNURLresponce.Substring(FRNURLresponce.IndexOf("lastName"), 30);
        //            string[] Arr = Val_lastName.Split('&');
        //            Val_lastName = Arr[0].Replace("lastName=", "");
        //        }
        //        catch (Exception ex)
        //        {


        //        }
        //    }

        //    if (FRNURLresponce.Contains("firstName") || FRNURLresponce.Contains("ShortTitle"))
        //    {
        //        try
        //        {
        //            if (FRNURLresponce.Contains("firstName"))
        //            {
        //                Val_firstName = FRNURLresponce.Substring(FRNURLresponce.IndexOf("firstName"), 30);
        //                string[] Arr = Val_firstName.Split('&');
        //                Val_firstName = Arr[0].Replace("firstName=", "");
        //            }
        //            else if (FRNURLresponce.Contains("ShortTitle"))
        //            {
        //                Val_firstName = FRNURLresponce.Substring(FRNURLresponce.IndexOf("ShortTitle"), 30);
        //                string[] Arr = Val_firstName.Split('"');
        //                Val_firstName = Arr[2].Replace("ShortTitle=", "");
        //            }

        //        }
        //        catch (Exception ex)
        //        {


        //        }



        //    }

        //    //Frnd Requset Send Option URl
        //    string thiredResponce = "http://www.linkedin.com/people/invite?from=profile&key=" + Val_key + "&firstName=" + Val_firstName + "&lastName=" + Val_lastName + "&authToken=" + Val_AuthToken + "&authType=" + Val_AuthType + "&goback=" + Val_goback + "%2Enpv_141450_*1_*1_OPENLINK_d7Ir_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1&trk=prof-0-sb-connect-button";
        //    //string SecondUrl = "http://www.linkedin.com/profile/view?id=" + Val_key + "&goback=" + Val_goback + "&authType=" + Val_AuthType + "&authToken=" + Val_AuthToken + "&trk=" + val_trk;
        //    string pageResponce2 = HttpHelper.getHtmlfromUrl(new Uri(thiredResponce));
        //    string ss = HttpHelper.getHtmlfromUrl(new Uri("http://www.linkedin.com/people/invite?from=profile&key=212815606&firstName=Dr%2E+Sanjeev&lastName=Puri&authToken=KIFZ&authType=OUT_OF_NETWORK&goback=%2Efps_PBCK_sanjeev_*1_*1_*1_*1_*1_*1_*2_*1_I_in_*1_*1_false_1_R_*1_*51_*1_*51_true_*1_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2%2Enpv_141450_*1_*1_OPENLINK_d7Ir_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1&trk=prof-0-sb-connect-button"));
        //    if (pageResponce2.Contains("sourceAlias"))
        //    {
        //        try
        //        {
        //            Val_sourceAlias = pageResponce2.Substring(pageResponce2.IndexOf("sourceAlias"), 100);
        //            string[] Arr = Val_sourceAlias.Split('"');
        //            Val_sourceAlias = Arr[2];
        //        }
        //        catch (Exception ex)
        //        {


        //        }
        //    }

        //    string lastPostUrl = "http://www.linkedin.com/people/iweReconnectAction";

        //    //
        //    //*********post request for last process of send request 

        //    //existingPositionIC=&companyName.0=&titleIC.0=&startYearIC.0=&endYearIC.0=&schoolText=&schoolID=&existingPositionIB=&companyName.1=&titleIB.0=&startYearIB.0=&endYearIB.0=&reason=IF&otherEmail=&greeting=I%27d+like+to+add+you+to+my+professional+network+on+LinkedIn.%0D%0A%0D%0A-+gaurav+agrawal&iweReconnectSubmit=Send+Invitation&key=141450&firstName=Ron&lastName=Bates&authToken=d7Ir&authType=OPENLINK&trk=prof-0-sb-connect-button&iweLimitReached=false&companyID.0=&companyID.1=&schoolID=&schoolcountryCode=&schoolprovinceCode=&javascriptEnabled=false&subject=gaurav+agrawal+wants+to+connect+on+LinkedIn+&defaultText=a908b3ba18701677a5879c1955035d05&csrfToken=ajax%3A8096156173388039261&                                                            sourceAlias=0_0ZD1VB-f52hFxNsCjPZdLV&goback=.fps_PBCK_*1_*1_*1_*1_*1_*1_*1_*2_*1_Y_*1_*1_*1_false_1_C_*1_*51_*1_*51_true_CC%2CN%2CG%2CI%2CPC%2CED%2CL%2CFG%2CTE%2CFA%2CSE%2CP%2CCS%2CF%2CDR_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2.npv_141450_*1_*1_OPENLINK_d7Ir_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1;
        //    string NameOfSender = _UserName.Remove(_UserName.IndexOf("@"));
        //    // tab-name username">
        //    string SenderName = string.Empty;
        //    try
        //    {
        //        SenderName = FRNURLresponce.Substring(FRNURLresponce.IndexOf("tab-name username\">"), (FRNURLresponce.IndexOf("<", FRNURLresponce.IndexOf("tab-name username\">")) - FRNURLresponce.IndexOf("tab-name username\">"))).Replace("tab-name username\">", string.Empty).Trim().Replace("tab-name username\">", string.Empty);
        //        NameOfSender = Uri.EscapeUriString(SenderName);
        //    }
        //    catch { }
        //    //if(string.IsNullOrEmpty(SenderName))
        //    //{
        //    //SenderName=NameOfSender;
        //    //}

        //    // catch { }
        //    //if (Val_AuthType.Contains("</noscript>"))
        //    //{
        //    //    Val_AuthType = Val_AuthType.Replace("</noscript>", string.Empty).Replace("\n",string.Empty).Replace(">",string.Empty).Replace("\\",string.Empty).Replace("\"",string.Empty).Trim();
        //    //}
        //    string LastPostData = "existingPositionIC=&companyName.0=&titleIC.0=&startYearIC.0=&endYearIC.0=&schoolText=&schoolID=&existingPositionIB=&companyName.1=&titleIB.0=&startYearIB.0=&endYearIB.0=&reason=IF&otherEmail=&greeting=I%27d+like+to+add+you+to+my+professional+network+on+LinkedIn.%0D%0A%0D%0A-" + NameOfSender + "&iweReconnectSubmit=Send+Invitation&key=" + Val_key + "&firstName=" + Val_firstName + "&lastName=" + Val_lastName + "&authToken=" + Val_AuthToken + "&authType=" + Val_AuthType + "&trk=prof-0-sb-connect-button&iweLimitReached=false&companyID.0=&companyID.1=&schoolID=&schoolcountryCode=&schoolprovinceCode=&javascriptEnabled=false&subject=" + NameOfSender + "+wants+to+connect+on+LinkedIn+&defaultText=a908b3ba18701677a5879c1955035d05&csrfToken=" + Val_CsrToken + "&sourceAlias=" + Val_sourceAlias + "&goback=" + Val_goback.Replace("%2E", ".") + ".npv_" + Val_key + "_*1_*1_OPENLINK_d7Ir_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1";
        //    string postResponse1 = HttpHelper.postFormDataProxy(new Uri(lastPostUrl), LastPostData, _ProxyAddress, proxyport, _ProxyUserName, _ProxyPassword);

        //    if (postResponse1.Contains("You must confirm your primary email address before sending an invitation."))
        //    {
        //        LoggerManageConnection(_UserName + "You must confirm your primary email address before sending an invitation.");
        //        valid = false;
        //        PageNumber = 0;
        //        Isaccountvalid = false;
        //        return; 
        //    }

        ////  else  if (postResponse1.Contains("Accept Invitations"))
        //    else if (postResponse1.Contains("<strong>Invitation to"))
        //    {
        //        LoggerManageConnection("Invitation to " + Val_firstName + " " + Val_lastName + " sent.");
        //        string CSVHeader = "UserName" + "," + "SearchKeyword" + "," + "Invitation Sent To" + "," + "InvitationUrl";
        //        try
        //        {
        //            string[] url = Regex.Split(FrnAcceptUrL, "&authType");
        //            string CSV_Content = _UserName + "," + _ConnectSearchKeyword + "," + Val_firstName + " " + Val_lastName + "," + url[0];
        //            CSVUtilities.ExportDataCSVFile(CSVHeader, CSV_Content, Globals.path_AddConnectionSuccess);
        //            valid = true;
        //        }
        //        catch { }
        //    }

        //    else
        //    {
        //        LoggerManageConnection("Failed To Send Invitation To" + Val_firstName + " " + Val_lastName);
        //        valid = false;
        //        Isaccountvalid = false;
        //    }

        //    int Delay = RandomNumberGenerator.GenerateRandom(SearchMinDelay, SeacrhMaxDelay);
        //    Thread.Sleep(Delay * 1000);
        //    LoggerManageConnection("Invitation Delayed For " + Delay + " Seconds");

        //    SendInvitationCount++;
        //    if (SendInvitationCount >= SearchCriteria.NumberOfRequest)  //Here we check  that number of request send if counter is equal to the number of request put by user then it return to mathod and made pagenumber = 0; 
        //    {
        //        //SendInvitationCount--;

        //        PageNumber = 0;

        //        return;
        //    }


        //}

        public void TestConnectPageSearchForUniqueUrl(string pageNumber)
        {
            int proxyport = 888;
            string SummaryLink = string.Empty;

            Regex PortCheck = new Regex("^[0-9]*$");

            if (PortCheck.IsMatch(_ProxyPort) && !string.IsNullOrEmpty(_ProxyPort))
            {
                proxyport = int.Parse(_ProxyPort);
            }

            //recive the page number and then hit page wise and find the url in which we need to send the request.....

            string UrlConnectpage = "http://www.linkedin.com/search/fpsearch?keywords=" + _ConnectSearchKeyword + "&keepFacets=keepFacets&page_num=" + pageNumber + "&pplSearchOrigin=ADVS&viewCriteria=2&sortCriteria=R&redir=redir";
            string PgSrcMain1 = HttpHelper.getHtmlfromUrlProxy(new Uri(UrlConnectpage), _ProxyAddress, proxyport, _ProxyUserName, _ProxyPassword);

            string PostData = string.Empty;
            string page_num = string.Empty;
            string postUrl = string.Empty;
            string facetsOrder = string.Empty;

            if (PgSrcMain1.Contains("facetsOrder"))
            {
                facetsOrder = PgSrcMain1.Substring(PgSrcMain1.IndexOf("facetsOrder"), 200);
                string[] Arr = facetsOrder.Split('"');
                facetsOrder = Arr[2];
                string DecodedCharTest = Uri.UnescapeDataString(facetsOrder);
                string DecodedEmail = Uri.EscapeDataString(facetsOrder);
                facetsOrder = DecodedEmail;
            }

            page_num = pageNumber;

            PostData = "search=&viewCriteria=2&sortCriteria=C&facetsOrder=" + facetsOrder + "&page_num=" + page_num + "&openFacets=N%2CCC%2CG";

            //Post Url for Invite Message 
            postUrl = "http://www.linkedin.com/search/hits";

            //Diclaration For Post Add Connection For Keyword
            string Val_goback = string.Empty;
            string Val_sourceAlias = string.Empty;
            string Val_key = string.Empty;
            string Val_defaultText = string.Empty;
            string Val_firstName = string.Empty;
            string Val_CsrToken = string.Empty;
            string Val_Subject = string.Empty;
            string Val_greeting = string.Empty;
            string Val_AuthToken = string.Empty;
            string Val_AuthType = string.Empty;
            string val_trk = string.Empty;
            string Val_lastName = string.Empty;

            //Post Response
            string postResponse = HttpHelper.postFormDataProxy(new Uri(postUrl), PostData, _ProxyAddress, proxyport, _ProxyUserName, _ProxyPassword);
            List<string> PageSerchUrl = ChilkatBasedRegex.GettingAllUrls1(PgSrcMain1, "/profile/view?id");
            ///profile/view?id=22828991&authType=OUT_OF_NETWORK&authToken=pYz-&locale=en_US&srchid=2361660011371725230773&srchindex=1&srchtotal=3010&trk=vsrp_people_res_name&trkInfo=VSRPsearchId%3A2361660011371725230773%2CVSRPtargetId%3A22828991%2CVSRPcmpt%3Aprimary

            string FrnAcceptUrL = string.Empty;
            foreach (string item in PageSerchUrl)
            {
                try
                {
                    if (item.Contains("/profile/view?id"))  //this key word can only seprate the Url with all common urls....
                    {
                        lock (LockerConnection)
                        {
                            if (!item.Contains("http://www.linkedin.com"))
                            {
                                FrnAcceptUrL = "http://www.linkedin.com" + item;
                            }
                            else
                            {
                                FrnAcceptUrL = item;
                            }
                            string[] urll = Regex.Split(FrnAcceptUrL, "&authType");
                            DataSet ds = new DataSet();
                            try
                            {
                                string Querystring = "Select * From tb_ManageAddConnection Where Keyword='" + _ConnectSearchKeyword + "' and LinkedinUrl='" + urll[0] + "'";//"Insert into tb_ManageAddConnection (Keyword,LinkedinUrl,Username,DateTime) Values('" + _ConnectSearchKeyword + "','" + url[0] + "','" + _UserName + "','" + DateTime.Now.ToString("dd-MM-yyyy") + "')";
                                ds = DataBaseHandler.SelectQuery(Querystring, "tb_ManageAddConnection");
                            }
                            catch { }
                            if (ds.Tables[0].Rows.Count < 1)
                            {
                                //FrnAcceptUrL = "http://www.linkedin.com/profile/view?id=231353962&authType=NAME_SEARCH&authToken=hf4m&locale=en_US&srchid=a7561e90-b7b3-4d02-9db0-640ccd7369cf-0&srchindex=1&srchtotal=1&goback=.fps_PBCK_*1_Ella_Roberts_*1_*1_*1_*1_*2_*1_I_hu_*1_*1_false_1_R_*1_*51_*1_*51_true_*1_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2&pvs=ps&trk=pp_profile_name_link";
                                string FRNURLresponce = HttpHelper.getHtmlfromUrl1(new Uri(FrnAcceptUrL));

                                if (FRNURLresponce.Contains("/profile/view?id="))
                                {
                                    try
                                    {
                                        Val_key = FRNURLresponce.Substring(FRNURLresponce.IndexOf("/profile/view?id="), 30);
                                        string[] Arr = Val_key.Split('&');
                                        Val_key = Arr[0].Replace("/profile/view?id=", "");
                                    }
                                    catch (Exception ex)
                                    {


                                    }

                                }
                                if (FrnAcceptUrL.Contains("/profile/view?id="))
                                {
                                    try
                                    {
                                        Val_key = FrnAcceptUrL.Substring(FrnAcceptUrL.IndexOf("id="), (FrnAcceptUrL.IndexOf("&", FrnAcceptUrL.IndexOf("id=")) - FrnAcceptUrL.IndexOf("id="))).Replace("id=", string.Empty).Trim();

                                        //Val_key = FRNURLresponce.Substring(FrnAcceptUrL.IndexOf("/profile/view?id="), 30);
                                        //string[] Arr = Val_key.Split('&');
                                        //Val_key = Arr[0].Replace("/profile/view?id=", "");
                                    }
                                    catch (Exception ex)
                                    {


                                    }

                                }

                                if (FRNURLresponce.Contains("csrfToken"))
                                {
                                    try
                                    {
                                        Val_CsrToken = FRNURLresponce.Substring(FRNURLresponce.IndexOf("csrfToken"), 100);
                                        string[] Arr = Val_CsrToken.Split('&');
                                        Val_CsrToken = Arr[0].Replace("csrfToken=", "");
                                    }
                                    catch (Exception ex)
                                    {


                                    }

                                }

                                if (FRNURLresponce.Contains("authToken"))
                                {
                                    try
                                    {
                                        Val_AuthToken = FRNURLresponce.Substring(FRNURLresponce.IndexOf("authToken"), 50);
                                        string[] Arr = Val_AuthToken.Split('&');
                                        Val_AuthToken = Arr[0].Replace("authToken=", "");
                                        if (Val_AuthToken.Contains("</noscript>"))
                                        {
                                            try
                                            {
                                                string[] Val_Auth = Regex.Split(Val_AuthToken, ">");
                                                Val_AuthToken = Val_Auth[0].Replace("\"", string.Empty);
                                            }
                                            catch { }
                                        }
                                        // Val_AuthToken="2p6r";
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                }
                                if (FRNURLresponce.Contains("authType"))
                                {
                                    try
                                    {
                                        Val_AuthType = FRNURLresponce.Substring(FRNURLresponce.IndexOf("authType"), 50);
                                        string[] Arr = Val_AuthType.Split('&');
                                        Val_AuthType = Arr[0].Replace("authType=", "");
                                        if (Val_AuthType.Contains("</noscript>"))
                                        {
                                            Val_AuthType = Val_AuthType.Replace("</noscript>", string.Empty).Replace("\n", string.Empty).Replace(">", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Trim();
                                        }
                                        try
                                        {
                                            if (string.IsNullOrEmpty(Val_AuthType))
                                            {
                                                Val_AuthType = "OUT_OF_NETWORK";
                                            }
                                            // Val_AuthType = FRNURLresponce.Substring(FRNURLresponce.IndexOf("authType="), (FRNURLresponce.IndexOf(">", FRNURLresponce.IndexOf("authType=")) - FRNURLresponce.IndexOf("authType="))).Replace("authType=", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Trim();
                                        }
                                        catch { }
                                        //authType=
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                }


                                if (FRNURLresponce.Contains("goback"))
                                {
                                    try
                                    {
                                        Val_goback = FRNURLresponce.Substring(FRNURLresponce.IndexOf("goback"), 300);
                                        string[] Arr = Val_goback.Split('"');
                                        Val_goback = Arr[0].Replace("goback=", "");
                                    }
                                    catch (Exception ex)
                                    {


                                    }
                                }
                                if (FRNURLresponce.Contains("trk"))
                                {
                                    try
                                    {
                                        val_trk = FRNURLresponce.Substring(FRNURLresponce.IndexOf("trk"), 100);
                                        string[] Arr = val_trk.Split(';');
                                        val_trk = Arr[0].Replace("trk=", "");
                                    }
                                    catch (Exception ex)
                                    {


                                    }
                                }

                                if (FRNURLresponce.Contains("lastName"))
                                {
                                    try
                                    {
                                        Val_lastName = FRNURLresponce.Substring(FRNURLresponce.IndexOf("lastName"), 30);
                                        string[] Arr = Val_lastName.Split('&');
                                        Val_lastName = Arr[0].Replace("lastName=", "");
                                    }
                                    catch (Exception ex)
                                    {


                                    }
                                }

                                if (FRNURLresponce.Contains("firstName") || FRNURLresponce.Contains("ShortTitle"))
                                {
                                    try
                                    {
                                        if (FRNURLresponce.Contains("firstName"))
                                        {
                                            Val_firstName = FRNURLresponce.Substring(FRNURLresponce.IndexOf("firstName"), 30);
                                            string[] Arr = Val_firstName.Split(':');
                                            Val_firstName = Arr[1].Replace("//", string.Empty).Replace("\"", string.Empty).Replace("}", string.Empty).Replace("link__en", string.Empty).Replace(",", string.Empty);
                                        }
                                        else if (FRNURLresponce.Contains("ShortTitle"))
                                        {
                                            Val_firstName = FRNURLresponce.Substring(FRNURLresponce.IndexOf("ShortTitle"), 30);
                                            string[] Arr = Val_firstName.Split('"');
                                            Val_firstName = Arr[2].Replace("ShortTitle=", "");
                                        }

                                    }
                                    catch (Exception ex)
                                    {


                                    }



                                }

                                //Frnd Requset Send Option URl
                                //markAsReadOnClick
                                try
                                {
                                    string[] Valuesss = Regex.Split(FRNURLresponce, "markAsReadOnClick");
                                    try
                                    {
                                        Val_goback = Valuesss[1].Substring(Valuesss[1].IndexOf("goback="), (Valuesss[1].IndexOf("}", Valuesss[1].IndexOf("goback=")) - Valuesss[1].IndexOf("goback="))).Replace("goback=", string.Empty).Trim();

                                    }
                                    catch
                                    { }// if(!string.IsNullOrEmpty(Val_goback1))
                                    //{
                                    //     Val_goback1 = Val_goback;
                                    // }
                                }
                                catch { }
                                // string thiredResponce = "http://www.linkedin.com/people/invite?from=profile&key=" + Val_key + "&firstName=" + Val_firstName + "&lastName=" + Val_lastName + "&authToken=" + Val_AuthToken + "&authType=" + Val_AuthType + "&pvs=ps&trk=pp_invite_member_action&goback=" + Val_goback;
                                string thiredResponce = "http://www.linkedin.com/people/invite?from=profile&key=" + Val_key + "&firstName=" + Val_firstName + "&lastName=" + Val_lastName + "&authToken=" + Val_AuthToken + "&authType=" + Val_AuthType + "&goback=" + Val_goback + "&trk=prof-0-sb-connect-button";
                                //  string thiredResponce = "http://www.linkedin.com/people/invite?from=profile&key=" + Val_key + "&firstName=" + Val_firstName + "&lastName=" + Val_lastName + "&authToken=" + Val_AuthToken + "&authType=" + Val_AuthType + "&goback=" + Val_goback + "%2Enpv_141450_*1_*1_OPENLINK_d7Ir_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1&trk=prof-0-sb-connect-button";
                                //string SecondUrl = "http://www.linkedin.com/profile/view?id=" + Val_key + "&goback=" + Val_goback + "&authType=" + Val_AuthType + "&authToken=" + Val_AuthToken + "&trk=" + val_trk;
                                string pageResponce2 = HttpHelper.getHtmlfromUrl1(new Uri(thiredResponce));
                                // string ss = HttpHelper.getHtmlfromUrl(new Uri("http://www.linkedin.com/people/invite?from=profile&key=212815606&firstName=Dr%2E+Sanjeev&lastName=Puri&authToken=KIFZ&authType=OUT_OF_NETWORK&goback=%2Efps_PBCK_sanjeev_*1_*1_*1_*1_*1_*1_*2_*1_I_in_*1_*1_false_1_R_*1_*51_*1_*51_true_*1_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2%2Enpv_141450_*1_*1_OPENLINK_d7Ir_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1&trk=prof-0-sb-connect-button"));
                                if (pageResponce2.Contains("You and this LinkedIn user don’t know anyone in common"))
                                {
                                    string CSVHeader = "UserName" + "," + "SearchKeyword" + "," + "Invitation Sent To" + "," + "InvitationUrl";
                                    string[] url = Regex.Split(FrnAcceptUrL, "&authType");
                                    string CSV_Content = _UserName + "," + _ConnectSearchKeyword + ", ," + url[0];
                                    CSVUtilities.ExportDataCSVFile(CSVHeader, CSV_Content, Globals.path_ConnectionThroughUnkonwPeopleLink);
                                    LoggerManageConnection("[ " + DateTime.Now + " ] => [ Failed To Send Invitation From " + _UserName + " ]");
                                    valid = false;
                                    Isaccountvalid = false;
                                    continue;
                                }
                                if (pageResponce2.Contains("sourceAlias"))
                                {
                                    try
                                    {
                                        Val_sourceAlias = pageResponce2.Substring(pageResponce2.IndexOf("sourceAlias"), 100);
                                        string[] Arr = Val_sourceAlias.Split('"');
                                        Val_sourceAlias = Arr[2];
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }
                                string[] strNameValue = Regex.Split(pageResponce2, "name=");

                                #region DecareVariable By Sanjeev
                                string iweReconnectSubmit = string.Empty;
                                string iweLimitReached = string.Empty;
                                string companyID0 = string.Empty;
                                string companyName0 = string.Empty;
                                //companyName.0
                                string companyID1 = string.Empty;
                                string schoolID = string.Empty;
                                string schoolcountryCode = string.Empty;
                                string schoolprovinceCode = string.Empty;
                                string subject = string.Empty;
                                string defaultText = string.Empty;
                                string csrfToken = string.Empty;
                                string sourceAlias = string.Empty;
                                string goback = string.Empty;
                                string titleIC0 = string.Empty;
                                string greeting = string.Empty;
                                string startYearIC0 = string.Empty;
                                string endYearIC0 = string.Empty;
                                string schoolText = string.Empty;
                                string titleIB0 = string.Empty;
                                string startYearIB0 = string.Empty;
                                string trk = string.Empty;
                                string authType = string.Empty;
                                string otheremail = string.Empty;
                                string firstName = string.Empty;
                                string lastName = string.Empty;
                                #endregion



                                #region
                                try
                                {
                                    string[] ArrForValue = Regex.Split(pageResponce2, "name=\"startYearIC.0");
                                    string strValue = (ArrForValue[1].Substring(ArrForValue[1].IndexOf("value="), ArrForValue[1].IndexOf(" ", ArrForValue[1].IndexOf("value=")) - ArrForValue[1].IndexOf("value=")).Replace("value=", string.Empty).Replace("\\\"", string.Empty).Replace("\"", string.Empty).Trim());
                                    startYearIC0 = (strValue);

                                }
                                catch { }
                                #endregion
                                #region
                                try
                                {
                                    string[] ArrForValue = Regex.Split(pageResponce2, "name=\"authType");
                                    string strValue = (ArrForValue[1].Substring(ArrForValue[1].IndexOf("value="), ArrForValue[1].IndexOf("id=", ArrForValue[1].IndexOf("value=")) - ArrForValue[1].IndexOf("value=")).Replace("value=", string.Empty).Replace("\\\"", string.Empty).Replace("\"", string.Empty).Trim());
                                    authType = (strValue);

                                }
                                catch { }
                                #endregion
                                #region
                                try
                                {
                                    string[] ArrForValue = Regex.Split(pageResponce2, "name=\"authType");
                                    string strValue = (ArrForValue[1].Substring(ArrForValue[1].IndexOf("value="), ArrForValue[1].IndexOf("id=", ArrForValue[1].IndexOf("value=")) - ArrForValue[1].IndexOf("value=")).Replace("value=", string.Empty).Replace("\\\"", string.Empty).Replace("\"", string.Empty).Trim());
                                    authType = (strValue);

                                }
                                catch { }
                                #endregion
                                #region
                                try
                                {
                                    //string[] ArrForValue = Regex.Split(pageResponce2, "name=\"trk");
                                    //string strValue = (ArrForValue[1].Substring(ArrForValue[1].IndexOf("value="), ArrForValue[1].IndexOf("id=", ArrForValue[1].IndexOf("value=")) - ArrForValue[1].IndexOf("value=")).Replace("value=", string.Empty).Replace("\\\"", string.Empty).Replace("\"", string.Empty).Trim());
                                    //trk = (strValue);

                                    string[] ArrForValue = Regex.Split(pageResponce2, "name=\"trk");
                                    int tempstartIndex = ArrForValue[1].IndexOf("id=\"");
                                    string strValue = ArrForValue[1].Substring(tempstartIndex).Replace("id=\"", "");
                                    strValue = strValue.Substring(0, strValue.IndexOf("\""));
                                    trk = (strValue).Trim();

                                }
                                catch { }
                                #endregion
                                #region
                                try
                                {
                                    string[] ArrForValue = Regex.Split(pageResponce2, "name=\"endYearIC.0");
                                    string strValue = (ArrForValue[1].Substring(ArrForValue[1].IndexOf("value="), ArrForValue[1].IndexOf(" ", ArrForValue[1].IndexOf("value=")) - ArrForValue[1].IndexOf("value=")).Replace("value=", string.Empty).Replace("\\\"", string.Empty).Replace("\"", string.Empty).Trim());
                                    endYearIC0 = (strValue);

                                }
                                catch { }
                                #endregion
                                #region
                                try
                                {
                                    string[] ArrForValue = Regex.Split(pageResponce2, "name=\"schoolText");
                                    string strValue = (ArrForValue[1].Substring(ArrForValue[1].IndexOf("value="), ArrForValue[1].IndexOf(" ", ArrForValue[1].IndexOf("value=")) - ArrForValue[1].IndexOf("value=")).Replace("value=", string.Empty).Replace("\\\"", string.Empty).Replace("\"", string.Empty).Trim());
                                    schoolText = (strValue);

                                }
                                catch { }
                                #endregion
                                #region
                                try
                                {
                                    string[] ArrForValue = Regex.Split(pageResponce2, "name=\"startYearIB.0");
                                    string strValue = (ArrForValue[1].Substring(ArrForValue[1].IndexOf("value="), ArrForValue[1].IndexOf(" ", ArrForValue[1].IndexOf("value=")) - ArrForValue[1].IndexOf("value=")).Replace("value=", string.Empty).Replace("\\\"", string.Empty).Replace("\"", string.Empty).Trim());
                                    startYearIB0 = (strValue);

                                }
                                catch { }
                                #endregion
                                #region
                                try
                                {
                                    string[] ArrForValue = Regex.Split(pageResponce2, "name=\"titleIB.0");
                                    string strValue = (ArrForValue[1].Substring(ArrForValue[1].IndexOf("value="), ArrForValue[1].IndexOf(" ", ArrForValue[1].IndexOf("value=")) - ArrForValue[1].IndexOf("value=")).Replace("value=", string.Empty).Replace("\\\"", string.Empty).Replace("\"", string.Empty).Trim());
                                    titleIB0 = (strValue);

                                }
                                catch { }
                                #endregion
                                //schoolText titleIB.0=
                                #region
                                try
                                {
                                    string[] ArrForValue = Regex.Split(pageResponce2, "name=\"companyID.0");
                                    string strValue = (ArrForValue[1].Substring(ArrForValue[1].IndexOf("value="), ArrForValue[1].IndexOf(" ", ArrForValue[1].IndexOf("value=")) - ArrForValue[1].IndexOf("value=")).Replace("value=", string.Empty).Replace("\\\"", string.Empty).Replace("\"", string.Empty).Trim());
                                    companyID0 = (strValue);

                                }
                                catch { }
                                #endregion
                                #region
                                try
                                {
                                    string[] ArrForValue = Regex.Split(pageResponce2, "name=\"companyID.1");
                                    string strValue = (ArrForValue[1].Substring(ArrForValue[1].IndexOf("value="), ArrForValue[1].IndexOf(" ", ArrForValue[1].IndexOf("value=")) - ArrForValue[1].IndexOf("value=")).Replace("value=", string.Empty).Replace("\\\"", string.Empty).Replace("\"", string.Empty).Trim());
                                    companyID1 = (strValue);

                                }
                                catch { }
                                #endregion
                                #region
                                try
                                {
                                    string[] ArrForValue = Regex.Split(pageResponce2, "name=\"schoolID");
                                    string strValue = (ArrForValue[1].Substring(ArrForValue[1].IndexOf("value="), ArrForValue[1].IndexOf(" ", ArrForValue[1].IndexOf("value=")) - ArrForValue[1].IndexOf("value=")).Replace("value=", string.Empty).Replace("\\\"", string.Empty).Replace("\"", string.Empty).Trim());
                                    schoolID = (strValue);

                                }
                                catch { }
                                #endregion
                                #region
                                try
                                {
                                    string[] ArrForValue = Regex.Split(pageResponce2, "name=\"schoolcountryCode");
                                    string strValue = (ArrForValue[1].Substring(ArrForValue[1].IndexOf("value="), ArrForValue[1].IndexOf(" ", ArrForValue[1].IndexOf("value=")) - ArrForValue[1].IndexOf("value=")).Replace("value=", string.Empty).Replace("\\\"", string.Empty).Replace("\"", string.Empty).Trim());
                                    schoolcountryCode = (strValue);

                                }
                                catch { }
                                #endregion
                                #region
                                try
                                {
                                    string[] ArrForValue = Regex.Split(pageResponce2, "name=\"schoolprovinceCode");
                                    string strValue = (ArrForValue[1].Substring(ArrForValue[1].IndexOf("value="), ArrForValue[1].IndexOf(" ", ArrForValue[1].IndexOf("value=")) - ArrForValue[1].IndexOf("value=")).Replace("value=", string.Empty).Replace("\\\"", string.Empty).Replace("\"", string.Empty).Trim());
                                    schoolprovinceCode = (strValue);
                                    if (!string.IsNullOrEmpty(schoolprovinceCode))
                                    {

                                        schoolprovinceCode = "";
                                    }
                                }
                                catch { }
                                #endregion
                                #region
                                try
                                {
                                    string[] ArrForValue = Regex.Split(pageResponce2, "name=\"subject");
                                    string strValue = (ArrForValue[1].Substring(ArrForValue[1].IndexOf("value="), ArrForValue[1].IndexOf("id=", ArrForValue[1].IndexOf("value=")) - ArrForValue[1].IndexOf("value=")).Replace("value=", string.Empty).Replace("\\\"", string.Empty).Replace("\"", string.Empty).Trim());
                                    subject = (strValue);
                                }
                                catch { }
                                #endregion
                                #region
                                try
                                {
                                    string[] ArrForValue = Regex.Split(pageResponce2, "name=\"defaultText");
                                    string strValue = (ArrForValue[1].Substring(ArrForValue[1].IndexOf("value="), ArrForValue[1].IndexOf("id=", ArrForValue[1].IndexOf("value=")) - ArrForValue[1].IndexOf("value=")).Replace("value=", string.Empty).Replace("\\\"", string.Empty).Replace("\"", string.Empty).Trim());
                                    defaultText = (strValue);
                                }
                                catch { }
                                #endregion
                                #region
                                try
                                {
                                    string[] ArrForValue = Regex.Split(pageResponce2, "name=\"csrfToken");
                                    string strValue = (ArrForValue[1].Substring(ArrForValue[1].IndexOf("value="), ArrForValue[1].IndexOf("id=", ArrForValue[1].IndexOf("value=")) - ArrForValue[1].IndexOf("value=")).Replace("value=", string.Empty).Replace("\\\"", string.Empty).Replace("\"", string.Empty).Trim());
                                    csrfToken = (strValue);
                                }
                                catch { }
                                #endregion
                                #region
                                try
                                {
                                    string[] ArrForValue = Regex.Split(pageResponce2, "name=\"sourceAlias");
                                    string strValue = (ArrForValue[1].Substring(ArrForValue[1].IndexOf("value="), ArrForValue[1].IndexOf("id=", ArrForValue[1].IndexOf("value=")) - ArrForValue[1].IndexOf("value=")).Replace("value=", string.Empty).Replace("\\\"", string.Empty).Replace("\"", string.Empty).Trim());
                                    sourceAlias = (strValue);
                                }
                                catch { }
                                #endregion
                                #region
                                try
                                {
                                    //string[] ArrForValue = Regex.Split(pageResponce2, "name=\"goback");
                                    //string strValue = (ArrForValue[1].Substring(ArrForValue[1].IndexOf("value="), ArrForValue[1].IndexOf("id=", ArrForValue[1].IndexOf("value=")) - ArrForValue[1].IndexOf("value=")).Replace("value=", string.Empty).Replace("\\\"", string.Empty).Replace("\"", string.Empty).Trim());
                                    //goback = (strValue);

                                    string[] ArrForValue = Regex.Split(pageResponce2, "name=\"goback");
                                    string strValue = (ArrForValue[1].Substring(ArrForValue[1].IndexOf("value="), ArrForValue[1].IndexOf("id=", ArrForValue[1].IndexOf("value=")) - ArrForValue[1].IndexOf("value=")).Replace("value=", string.Empty).Replace("\\\"", string.Empty).Replace("\"", string.Empty).Trim());
                                    goback = (strValue).Replace("&quot;", "%22").Replace(":", "%3A").Replace(",", "%2C").Replace("/", "%2F").Replace("{", "%7B").Replace(" ", "+");
                                }
                                catch { }
                                #endregion
                                #region
                                try
                                {
                                    string[] ArrForValue = Regex.Split(pageResponce2, "name=\"titleIC.0");
                                    string strValue = (ArrForValue[1].Substring(ArrForValue[1].IndexOf("value="), ArrForValue[1].IndexOf("id=", ArrForValue[1].IndexOf("value=")) - ArrForValue[1].IndexOf("value=")).Replace("value=", string.Empty).Replace("\\\"", string.Empty).Replace("\"", string.Empty).Trim());
                                    titleIC0 = (strValue);
                                }
                                catch { }
                                #endregion
                                #region
                                try
                                {
                                    string[] ArrForValue = Regex.Split(pageResponce2, "name=\"otherEmail");
                                    string strValue = (ArrForValue[1].Substring(ArrForValue[1].IndexOf("value="), ArrForValue[1].IndexOf("id=", ArrForValue[1].IndexOf("value=")) - ArrForValue[1].IndexOf("value=")).Replace("value=", string.Empty).Replace("\\\"", string.Empty).Replace("\"", string.Empty).Trim());
                                    otheremail = (strValue);
                                }
                                catch { }
                                #endregion
                                #region
                                try
                                {
                                    string[] ArrForValue = Regex.Split(pageResponce2, "name=\"iweLimitReached");
                                    string strValue = (ArrForValue[1].Substring(ArrForValue[1].IndexOf("value="), ArrForValue[1].IndexOf("id=", ArrForValue[1].IndexOf("value=")) - ArrForValue[1].IndexOf("value=")).Replace("value=", string.Empty).Replace("\\\"", string.Empty).Replace("\"", string.Empty).Trim());
                                    iweLimitReached = (strValue);
                                }
                                catch { }
                                #endregion
                                #region
                                try
                                {
                                    try
                                    {
                                        string[] ArrForValue = Regex.Split(pageResponce2, "name=\"iweReconnectSubmit");
                                        string strValue = (ArrForValue[1].Substring(ArrForValue[1].IndexOf("value="), ArrForValue[1].IndexOf("class=", ArrForValue[1].IndexOf("value=")) - ArrForValue[1].IndexOf("value=")).Replace("value=", string.Empty).Replace("\\\"", string.Empty).Replace("\"", string.Empty).Trim());
                                        iweReconnectSubmit = (strValue);
                                    }
                                    catch { }
                                    try
                                    {
                                        if (iweReconnectSubmit.Contains(">") || iweReconnectSubmit.Contains(">"))
                                        {
                                            iweReconnectSubmit = "Send Invitation";
                                        }
                                    }
                                    catch { }
                                }
                                catch { }
                                #endregion
                                #region
                                try
                                {
                                    try
                                    {
                                        string[] ArrForValue = Regex.Split(pageResponce2, "name=\"greeting");
                                        string strValue = (ArrForValue[1].Substring(ArrForValue[1].IndexOf("class=\"message\">"), ArrForValue[1].IndexOf("<", ArrForValue[1].IndexOf("class=\"message\">")) - ArrForValue[1].IndexOf("class=\"message\">")).Replace("class=\"message\">", string.Empty).Replace("\\\"", string.Empty).Replace("\"", string.Empty).Trim());
                                        greeting = (strValue);
                                        greeting = greeting.Replace(" ", "+");
                                        greeting = Uri.EscapeUriString(greeting);
                                    }
                                    catch { }
                                    try
                                    {
                                        if (iweReconnectSubmit.Contains(">") || iweReconnectSubmit.Contains(">"))
                                        {
                                            //iweReconnectSubmit = "Send Invitation";
                                        }
                                    }
                                    catch { }
                                }
                                catch { }
                                #endregion
                                #region
                                try
                                {
                                    string[] ArrForValue = Regex.Split(pageResponce2, "name=\"companyName.0");
                                    string strValue = (ArrForValue[1].Substring(ArrForValue[1].IndexOf("value="), ArrForValue[1].IndexOf("id=", ArrForValue[1].IndexOf("value=")) - ArrForValue[1].IndexOf("value=")).Replace("value=", string.Empty).Replace("\\\"", string.Empty).Replace("\"", string.Empty).Trim());
                                    companyName0 = (strValue);
                                }
                                catch { }
                                #endregion
                                #region
                                try
                                {
                                    string[] ArrForValue = Regex.Split(pageResponce2, "name=\"firstName");
                                    string strValue = (ArrForValue[1].Substring(ArrForValue[1].IndexOf("value="), ArrForValue[1].IndexOf("id=", ArrForValue[1].IndexOf("value=")) - ArrForValue[1].IndexOf("value=")).Replace("value=", string.Empty).Replace("\\\"", string.Empty).Replace("\"", string.Empty).Trim());
                                    firstName = (strValue);
                                }
                                catch { }
                                #endregion
                                #region
                                try
                                {
                                    string[] ArrForValue = Regex.Split(pageResponce2, "name=\"lastName");
                                    string strValue = (ArrForValue[1].Substring(ArrForValue[1].IndexOf("value="), ArrForValue[1].IndexOf("id=", ArrForValue[1].IndexOf("value=")) - ArrForValue[1].IndexOf("value=")).Replace("value=", string.Empty).Replace("\\\"", string.Empty).Replace("\"", string.Empty).Trim());
                                    lastName = (strValue);

                                    if (lastName.Contains("&quot"))
                                    {
                                        lastName = string.Empty;

                                    }
                                }
                                catch { }
                                #endregion


                                string lastPostUrl = "http://www.linkedin.com/people/iweReconnectAction";

                                //
                                //*********post request for last process of send request 

                                //existingPositionIC=&companyName.0=&titleIC.0=&startYearIC.0=&endYearIC.0=&schoolText=&schoolID=&existingPositionIB=&companyName.1=&titleIB.0=&startYearIB.0=&endYearIB.0=&reason=IF&otherEmail=&greeting=I%27d+like+to+add+you+to+my+professional+network+on+LinkedIn.%0D%0A%0D%0A-+gaurav+agrawal&iweReconnectSubmit=Send+Invitation&key=141450&firstName=Ron&lastName=Bates&authToken=d7Ir&authType=OPENLINK&trk=prof-0-sb-connect-button&iweLimitReached=false&companyID.0=&companyID.1=&schoolID=&schoolcountryCode=&schoolprovinceCode=&javascriptEnabled=false&subject=gaurav+agrawal+wants+to+connect+on+LinkedIn+&defaultText=a908b3ba18701677a5879c1955035d05&csrfToken=ajax%3A8096156173388039261&                                                            sourceAlias=0_0ZD1VB-f52hFxNsCjPZdLV&goback=.fps_PBCK_*1_*1_*1_*1_*1_*1_*1_*2_*1_Y_*1_*1_*1_false_1_C_*1_*51_*1_*51_true_CC%2CN%2CG%2CI%2CPC%2CED%2CL%2CFG%2CTE%2CFA%2CSE%2CP%2CCS%2CF%2CDR_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2_*2.npv_141450_*1_*1_OPENLINK_d7Ir_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1;
                                string NameOfSender = _UserName.Remove(_UserName.IndexOf("@"));
                                // tab-name username">
                                string SenderName = string.Empty;
                                try
                                {
                                    SenderName = FRNURLresponce.Substring(FRNURLresponce.IndexOf("tab-name username\">"), (FRNURLresponce.IndexOf("<", FRNURLresponce.IndexOf("tab-name username\">")) - FRNURLresponce.IndexOf("tab-name username\">"))).Replace("tab-name username\">", string.Empty).Trim().Replace("tab-name username\">", string.Empty);
                                    NameOfSender = Uri.EscapeUriString(SenderName);
                                }
                                catch { }
                                //if(string.IsNullOrEmpty(SenderName))
                                //{
                                //SenderName=NameOfSender;
                                //}

                                // catch { }
                                //if (Val_AuthType.Contains("</noscript>"))
                                //{
                                //    Val_AuthType = Val_AuthType.Replace("</noscript>", string.Empty).Replace("\n",string.Empty).Replace(">",string.Empty).Replace("\\",string.Empty).Replace("\"",string.Empty).Trim();
                                //}
                                string LastPostData = "companyName.0=" + companyName0 + "titleIC.0=" + titleIC0 + "&startYearIC.0=" + startYearIC0 + "&endYearIC.0=" + endYearIC0 + "&schoolText=" + schoolText + "&schoolID=" + schoolID + "&companyName.1=" + companyName0 + "&titleIB.0=" + titleIB0 + "&startYearIB.0=" + startYearIB0 + "&endYearIB.0=" + endYearIC0 + "&reason=IF&otherEmail=" + otheremail + "&greeting=" + greeting + "&iweReconnectSubmit=" + iweReconnectSubmit.Replace(" ", "+") + "&key=" + Val_key + "&firstName=" + firstName + "&lastName=" + lastName + "&authToken=" + Val_AuthToken + "&authType=" + Val_AuthType + "&trk=" + trk + "&iweLimitReached=false&companyID.0=" + companyID0 + "&companyID.1=" + companyID1 + "&schoolID=" + schoolID + "&schoolcountryCode=" + schoolcountryCode + "&schoolprovinceCode=" + schoolprovinceCode + "&javascriptEnabled=false&subject=" + subject.Replace(" ", "+") + "&defaultText=" + defaultText + "&csrfToken=" + Val_CsrToken + "&sourceAlias=" + sourceAlias + "&goback=" + goback;
                                //string LastPostData = "companyName.0=" + companyName0 + "titleIC.0=" + titleIC0 + "&startYearIC.0=" + startYearIC0 + "&endYearIC.0=" + endYearIC0 + "&schoolText=" + schoolText + "&schoolID=" + schoolID + "&companyName.1=" + companyName0 + "&titleIB.0=" + titleIB0 + "&startYearIB.0=" + startYearIB0 + "&endYearIB.0=" + endYearIC0 + "&reason=IF&otherEmail=" + otheremail + "&greeting=" + greeting + "&iweReconnectSubmit=" + iweReconnectSubmit.Replace(" ", "+") + "&key=" + Val_key + "&firstName=" + firstName + "&lastName=" + lastName + "&authToken=" + Val_AuthToken + "&authType=" + Val_AuthType + "&trk=" + trk + "&iweLimitReached=false&companyID.0=" + companyID0 + "&companyID.1=" + companyID1 + "&schoolID=" + schoolID + "&schoolcountryCode=" + schoolcountryCode + "&schoolprovinceCode=" + schoolprovinceCode + "&javascriptEnabled=false&subject=" + subject.Replace(" ", "+") + "&defaultText=" + defaultText + "&csrfToken=" + Val_CsrToken + "&sourceAlias=" + sourceAlias + "&goback=" + goback;
                                //string LastPostData = "existingPositionIC=&companyName.0=&titleIC.0=&startYearIC.0=&endYearIC.0=&schoolText=&schoolID=&existingPositionIB=&companyName.1=&titleIB.0=&startYearIB.0=&endYearIB.0=&reason=IF&otherEmail=&greeting=I%27d+like+to+add+you+to+my+professional+network+on+LinkedIn.%0D%0A%0D%0A-" + NameOfSender + "&iweReconnectSubmit=Send+Invitation&key=" + Val_key + "&firstName=" + Val_firstName + "&lastName=" + Val_lastName + "&authToken=" + Val_AuthToken + "&authType=" + Val_AuthType + "&trk=prof-0-sb-connect-button&iweLimitReached=false&companyID.0=&companyID.1=&schoolID=&schoolcountryCode=&schoolprovinceCode=&javascriptEnabled=false&subject=" + NameOfSender + "+wants+to+connect+on+LinkedIn+&defaultText=a908b3ba18701677a5879c1955035d05&csrfToken=" + Val_CsrToken + "&sourceAlias=" + Val_sourceAlias + "&goback=" + Val_goback.Replace("%2E", ".") + ".npv_" + Val_key + "_*1_*1_OPENLINK_d7Ir_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1";
                                string postResponse1 = HttpHelper.postFormDataProxy(new Uri(lastPostUrl), LastPostData, _ProxyAddress, proxyport, _ProxyUserName, _ProxyPassword);
                                //Esuccess
                                string FinalValue = string.Empty;
                                try
                                {
                                    FinalValue = postResponse1.Substring(postResponse1.IndexOf("Esuccess"), (postResponse1.IndexOf(">", postResponse1.IndexOf("Esuccess")) - postResponse1.IndexOf("Esuccess"))).Replace("Esuccess", string.Empty).Trim();
                                    FinalValue = Uri.UnescapeDataString(FinalValue);
                                    string[] valuess = Regex.Split(FinalValue, "&");
                                    FinalValue = valuess[0].Replace("=", string.Empty);
                                    // FinalValue = postResponse1.Substring(postResponse1.IndexOf("Esuccess"), (postResponse.IndexOf(">", postResponse1.IndexOf("Esuccess")) - postResponse1.IndexOf("Esuccess"))).Replace("Esuccess", string.Empty);
                                    // Val_AuthType = FRNURLresponce.Substring(FRNURLresponce.IndexOf("authType="), (FRNURLresponce.IndexOf(">", FRNURLresponce.IndexOf("authType=")) - FRNURLresponce.IndexOf("authType="))).Replace("authType=", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Trim();
                                }
                                catch { }
                                try
                                {
                                    string lastGetResponse = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/people/pymk?goback=" + Val_goback + "&trk=" + val_trk + "&report%2Esuccess=" + FinalValue));
                                }
                                catch { }
                                if (postResponse1.Contains("You must confirm your primary email address before sending an invitation."))
                                {
                                    LoggerManageConnection("[ " + DateTime.Now + " ] => [ " + _UserName + "You must confirm your primary email address before sending an invitation.]");
                                    valid = false;
                                    PageNumber = 0;
                                    Isaccountvalid = false;
                                    break;
                                }
                                else if (postResponse1.Contains("<strong>Invitation to"))
                                {
                                    LoggerManageConnection("[ " + DateTime.Now + " ] => [ Invitation to " + firstName + " " + lastName + " sent For Keyword : " + _ConnectSearchKeyword + " From Account : " + _UserName + " ]");
                                    string CSVHeader = "UserName" + "," + "SearchKeyword" + "," + "Invitation Sent To" + "," + "InvitationUrl";
                                    try
                                    {
                                        string[] url = Regex.Split(FrnAcceptUrL, "&authType");
                                        string CSV_Content = _UserName + "," + _ConnectSearchKeyword + "," + firstName + " " + lastName + "," + url[0];
                                        CSVUtilities.ExportDataCSVFile(CSVHeader, CSV_Content, Globals.path_AddConnectionSuccess);
                                        try
                                        {
                                            string Querystring = "Insert into tb_ManageAddConnection (Keyword,LinkedinUrl,Username,DateTime) Values('" + _ConnectSearchKeyword + "','" + url[0] + "','" + _UserName + "','" + DateTime.Now.ToString("dd-MM-yyyy") + "')";
                                            DataBaseHandler.InsertQuery(Querystring, "tb_ManageAddConnection");
                                        }
                                        catch { }
                                        valid = true;
                                    }
                                    catch { }
                                }
                                else if (postResponse1.Contains("You and this LinkedIn user don’t know anyone in common"))
                                {
                                    string CSVHeader = "UserName" + "," + "SearchKeyword" + "," + "Invitation Sent To" + "," + "InvitationUrl";
                                    string[] url = Regex.Split(FrnAcceptUrL, "&authType");
                                    string CSV_Content = _UserName + "," + _ConnectSearchKeyword + ", ," + url[0];
                                    CSVUtilities.ExportDataCSVFile(CSVHeader, CSV_Content, Globals.path_ConnectionThroughUnkonwPeopleLink);
                                    LoggerManageConnection("[ " + DateTime.Now + " ] => [ Failed To Send Invitation From " + _UserName + " ]");
                                    valid = false;
                                    Isaccountvalid = false;
                                    continue;
                                }
                                else
                                {
                                    string CSVHeader = "UserName" + "," + "SearchKeyword" + "," + "Invitation Sent To" + "," + "InvitationUrl";
                                    string[] url = Regex.Split(FrnAcceptUrL, "&authType");
                                    string CSV_Content = _UserName + "," + _ConnectSearchKeyword + "," + firstName + " " + lastName + "," + url[0];
                                    CSVUtilities.ExportDataCSVFile(CSVHeader, CSV_Content, Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\InBoardPro\\ConnnectionNotAddedbyKeyword.csv");
                                    LoggerManageConnection("[ " + DateTime.Now + " ] => [ Failed To Send Invitation To" + firstName + " " + lastName + " ]");
                                    valid = false;
                                    Isaccountvalid = false;
                                }

                                int Delay = RandomNumberGenerator.GenerateRandom(SearchMinDelay, SeacrhMaxDelay);
                                LoggerManageConnection("[ " + DateTime.Now + " ] => [ Invitation Delayed For : " + Delay + " Seconds ]");
                                Thread.Sleep(Delay * 1000);

                                SendInvitationCount++;
                                if (SendInvitationCount >= SearchCriteria.NumberOfRequestPerKeyword)  //Here we check  that number of request send if counter is equal to the number of request put by user then it return to mathod and made pagenumber = 0; 
                                {
                                    //SendInvitationCount--;
                                    PageNumber = 0;
                                    return;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (ex.Message == "Stream was not readable")
                    {

                    }
                    string CSVHeader = "UserName" + "," + "SearchKeyword" + "," + "Invitation Sent To" + "," + "InvitationUrl";
                    string[] url = Regex.Split(FrnAcceptUrL, "&authType");
                    string CSV_Content = _UserName + "," + _ConnectSearchKeyword + ",," + url[0];
                    CSVUtilities.ExportDataCSVFile(CSVHeader, CSV_Content, Globals.path_ConnectionthroughKeywordSearchnotadded);
                    valid = false;
                    Isaccountvalid = false;
                }
            }
        }
        #endregion
    }
}
