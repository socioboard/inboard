#region namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
#endregion

namespace BaseLib
{
    public class LinkedinLogin
    {
        #region Variable Declaration

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
        public string Postalcode = string.Empty;
        public string LastName = string.Empty;
        public string IndustryType = string.Empty;
        public string Distance = string.Empty;
        public string PostGrpName = string.Empty;
        public string PostGrpSummry = string.Empty;
        public string PostGrpDesc = string.Empty;
        public string PostIamge = string.Empty;
        public string PostGrpWebsite = string.Empty;
        public string csrfToken = string.Empty;
        public string Post = string.Empty;

        GlobusHttpHelper HttpHelper = new GlobusHttpHelper();

        public static Queue<string> Que_Message_Post = new Queue<string>();
        public static Queue<string> Que_Grpname_Post = new Queue<string>();
        public static Queue<string> Que_GrpSummary_Post = new Queue<string>();
        public static Queue<string> Que_GrpDesc_Post = new Queue<string>();
        public static Queue<string> Que_Grpwebsite_Post = new Queue<string>();

        public static readonly object Locker_Message_Post = new object();
        public static readonly object Locker_Grpname_Post = new object();
        public static readonly object Locked_GrpSummary_Post = new object();
        public static readonly object Locked_GrpDesc_Post = new object();
        public static readonly object Locked_Grpwebsite_Post = new object();

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

        #region Login(ref ChilkatHttpHelpr HttpChilkat)
        //public void Login(ref ChilkatHttpHelpr HttpChilkat)
        //{
        //    string str = string.Empty;
        //    string accountUser = string.Empty;
        //    string accountPass = string.Empty;
        //    string proxyAddress = string.Empty;
        //    string proxyPort = string.Empty;
        //    string proxyUserName = string.Empty;
        //    string proxyPassword = string.Empty;
        //    string Url = string.Empty;
        //    Url = "https://www.linkedin.com/";
        //    string pageSrcLogin = HttpChilkat.GetHtmlProxy(Url, proxyAddress, proxyPort, proxyUserName, proxyPassword);

        //    string postdata = string.Empty;
        //    string postUrl = string.Empty;
        //    string ResLogin = string.Empty;
        //    string csrfToken = string.Empty;
        //    string sourceAlias = string.Empty;

        //    if (pageSrcLogin.Contains("csrfToken"))
        //    {
        //        csrfToken = pageSrcLogin.Substring(pageSrcLogin.IndexOf("csrfToken"), 100);
        //        string[] Arr = csrfToken.Split('"');
        //        csrfToken = Arr[2];
        //    }

        //    if (pageSrcLogin.Contains("sourceAlias"))
        //    {
        //        sourceAlias = pageSrcLogin.Substring(pageSrcLogin.IndexOf("sourceAlias"), 100);
        //        string[] Arr = sourceAlias.Split('"');
        //        sourceAlias = Arr[2];
        //    }
        //    accountUser = "riteshsatthya@globussoft.com";
        //    accountPass = "globussoft";
        //    postUrl = "https://www.linkedin.com/uas/login-submit";
        //    postdata = "session_key=" + accountUser + "&session_password=" + accountPass + "&source_app=&trk=guest_home_login&session_redirect=&csrfToken=" + csrfToken + "&sourceAlias=" + sourceAlias;
        //    ResLogin = HttpChilkat.PostFormData(Url, postdata, "");

        //    Url = "http://www.linkedin.com/home?trk=hb_tab_home_top";
        //    pageSrcLogin = HttpChilkat.GetHtmlProxy(Url, proxyAddress, proxyPort, proxyUserName, proxyPassword);

        //}
        #endregion

        #region LoginHttpHelper()
        public bool LoginHttpHelper(ref string Message)
        {
            try
            {

                Url = "https://www.linkedin.com/";
                ////string pageSrcLogin = HttpChilkat.GetHtmlProxy(Url, proxyAddress, proxyPort, proxyUserName, proxyPassword);
                string pageSrcLogin = HttpHelper.getHtmlfromUrlProxy(new Uri(Url), proxyAddress, int.Parse(proxyPort), proxyUserName, proxyPassword);
                
                string postdata = string.Empty;
                string postUrl = string.Empty;
                string ResLogin = string.Empty;
                string csrfToken = string.Empty;
                string sourceAlias = string.Empty;

                if (pageSrcLogin.Contains("csrfToken"))
                {
                    csrfToken = pageSrcLogin.Substring(pageSrcLogin.IndexOf("csrfToken"), 100);
                    string[] Arr = csrfToken.Split('"');
                    csrfToken = Arr[2];

                }

                if (pageSrcLogin.Contains("sourceAlias"))
                {
                    sourceAlias = pageSrcLogin.Substring(pageSrcLogin.IndexOf("sourceAlias"), 100);
                    string[] Arr = sourceAlias.Split('"');
                    sourceAlias = Arr[2];
                }

                postUrl = "https://www.linkedin.com/uas/login-submit";
                postdata = "session_key=" + accountUser + "&session_password=" + accountPass + "&source_app=&trk=guest_home_login&session_redirect=&csrfToken=" + csrfToken + "&sourceAlias=" + sourceAlias;

                ResLogin = HttpHelper.postFormData(new Uri(postUrl), postdata);

                if (ResLogin.Contains("Sign Out") && ResLogin.Contains("class=\"signout\""))
                {
                    IsLoggedIn = true;


                }
                if (ResLogin.Contains("The email address or password you provided does not match our records"))
                {
                    Message = "The email address or password does not match our records";
                    return false;
                }
                else if (ResLogin.Contains("Sign Out") && ResLogin.Contains("Profiles"))
                {
                    Message = "Logged in";
                    return true;
                }
                else if (ResLogin.Contains("Your LinkedIn account has been temporarily restricted"))
                {
                    Message = "Your LinkedIn account has been temporarily restricted";
                    return false;
                }

                //Url = "http://www.linkedin.com/home?trk=hb_tab_home_top";
                //pageSrcLogin = HttpHelper.getHtmlfromUrlProxy(new Uri(Url), proxyAddress, 888, proxyUserName, proxyPassword);

                //LogoutHttpHelper(ref HttpHelper);

                //Url = "http://www.linkedin.com/home?trk=hb_tab_home_top";
                //pageSrcLogin = HttpHelper.getHtmlfromUrlProxy(new Uri(Url), proxyAddress, 888, proxyUserName, proxyPassword);
            }
            catch { }
            return true;
        }
        #endregion

        #region LoginHttpHelper
        public void LoginHttpHelper(ref GlobusHttpHelper HttpHelper)
        {
            try
            {
                this.HttpHelper = HttpHelper;

                //Log("[ " + DateTime.Now + " ] => [ Logging In With Account : " + SearchCriteria.LoginID + " ]");
                Log("[ " + DateTime.Now + " ] => [ Logging In With Account : " + accountUser + " ]");
                Log("[ " + DateTime.Now + " ] => [ Login Process is Running... ]");
                Url = "https://www.linkedin.com/";
                string pageSrcLogin = string.Empty;
                int ProxyPort = 0;
                if (!string.IsNullOrEmpty(proxyPort) && NumberHelper.ValidateNumber(proxyPort))
                {
                    ProxyPort = int.Parse(proxyPort);
                }
                pageSrcLogin = HttpHelper.getHtmlfromUrlProxy(new Uri(Url), proxyAddress, ProxyPort, proxyUserName, proxyPassword);

                string postdata = string.Empty;
                string postUrl = string.Empty;
                string ResLogin = string.Empty;
                string csrfToken = string.Empty;
                string regCsrfParam = string.Empty;
                string sourceAlias = string.Empty;
               

                if (pageSrcLogin.Contains("csrfToken"))
                {
                    try
                    {
                        int startIndex = pageSrcLogin.IndexOf("name=\"csrfToken\"");
                        string start = pageSrcLogin.Substring(startIndex).Replace("name=\"csrfToken\"", "");
                        int endIndex = start.IndexOf("\" ");
                        string end = start.Substring(0, endIndex).Replace("value=\"", "").Trim();
                        csrfToken = end;
                        //csrfToken = csrfToken;
                    }
                    catch (Exception ex)
                    {

                    }

                }

                try
                {
                    if (csrfToken.Contains("&"))
                    {
                        string[] Arr = csrfToken.Split('&');
                        csrfToken = Arr[0].Replace("\"", string.Empty);

                    }
                    
                }
                catch { }

                if (pageSrcLogin.Contains("sourceAlias"))
                {
                    sourceAlias = pageSrcLogin.Substring(pageSrcLogin.IndexOf("sourceAlias"), 100);
                    string[] Arr = sourceAlias.Split('"');
                    sourceAlias = Arr[2].Replace(@"\", string.Empty).Replace("//", string.Empty).Trim();
                }

               
                try
                {
                    int SourceAliasStart = pageSrcLogin.IndexOf("regCsrfParam");
                    if (SourceAliasStart > 0)
                    {
                        try
                        {

                            regCsrfParam = pageSrcLogin.Substring(pageSrcLogin.IndexOf("regCsrfParam"), 100);
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
                postdata = "isJsEnabled=true&source_app=&tryCount=&session_key=" + Uri.EscapeDataString(accountUser) + "&session_password=" + Uri.EscapeDataString(accountPass) + "&signin=Sign%20In&session_redirect=&loginCsrfParam=" + regCsrfParam + "&csrfToken=" + csrfToken + "&sourceAlias=" + sourceAlias;

                try
                {
                    ResLogin = HttpHelper.postFormDataProxy(new Uri(postUrl), postdata, proxyAddress, ProxyPort, proxyUserName, proxyPassword);//HttpHelper.postFormDataRef(new Uri(postUrl), postdata, "http://www.linkedin.com/uas/login?goback=&trk=hb_signin", "", "");
                }
                catch { }

                //if (GroupStatus.GrouppageSourcewithProxy == string.Empty)
                //{
                //    GroupStatus.GrouppageSourcewithProxy = HttpHelper.getHtmlfromUrl(new Uri("http://www.linkedin.com/grp/"));
                //}
   
                string ImageUrl = string.Empty;
                string captchaText = string.Empty;
                string captchachallengeid = string.Empty;
                string dts = string.Empty;
                string origActionAlias = string.Empty;
                string origSourceAlias = string.Empty;
                string irhf = string.Empty;
                string submissionID = string.Empty;
                string CAPTCHAfwdcsrftoken = string.Empty;
                string CAPTCHAfwdsignin = string.Empty;
                string CAPTCHAfwdsession_password = string.Empty;
                string CAPTCHAfwdsession_key = string.Empty;
                string CAPTCHAfwdisJsEnabled = string.Empty;
                string CAPTCHAfwdloginCsrfParam = string.Empty;
                


                if (ResLogin.Contains("Security Verification"))
                {
                    string dataforcapctha = HttpHelper.getHtmlfromUrl1(new Uri("https://www.google.com/recaptcha/api/noscript?k=6LcnacMSAAAAADoIuYvLUHSNLXdgUcq-jjqjBo5n"));
                    if (!string.IsNullOrEmpty(dataforcapctha))
                    {
                        int startindex = dataforcapctha.IndexOf("id=\"recaptcha_challenge_field\"");
                        if (startindex > 0)
                        {
                            string start = dataforcapctha.Substring(startindex).Replace("id=\"recaptcha_challenge_field\"", "");
                            int endindex = start.IndexOf("\">");
                            string end = start.Substring(0, endindex).Replace("value=", string.Empty).Replace("\"", string.Empty).Trim();
                            ImageUrl = "https://www.google.com/recaptcha/api/image?c=" + end;
                            System.Net.WebClient webclient = new System.Net.WebClient();
                            byte[] args = webclient.DownloadData(ImageUrl);
                            string[] arr1 = new string[] { Globals.CapchaLoginID, Globals.CapchaLoginPassword, "" };
                            captchaText = DecodeDBC(arr1, args);
                        }
                        
                        if (ResLogin.Contains("name=\"security-challenge-id\""))
                        {
                            int startindexnew = ResLogin.IndexOf("name=\"security-challenge-id\"");
                            if (startindexnew > 0)
                            {
                                string start = ResLogin.Substring(startindexnew).Replace("name=\"security-challenge-id\"", "").Replace("value=\"", "");
                                int endindex = start.IndexOf("\"");
                                string end = start.Substring(0, endindex);
                                captchachallengeid = end.Replace("\"", string.Empty).Trim();
                            }
                        }

                        if (ResLogin.Contains("name=\"dts\""))
                        {
                            int startindexnew = ResLogin.IndexOf("name=\"dts\"");
                            if (startindexnew > 0)
                            {
                                string start = ResLogin.Substring(startindexnew).Replace("name=\"dts\"", "").Replace("value=\"", "");
                                int endindex = start.IndexOf("\"");
                                string end = start.Substring(0, endindex);
                                dts = end.Replace("\"", string.Empty).Trim();
                            }
                        }

                        if (ResLogin.Contains("name=\"origActionAlias\""))
                        {
                            int startindexnew = ResLogin.IndexOf("name=\"origActionAlias\"");
                            if (startindexnew > 0)
                            {
                                string start = ResLogin.Substring(startindexnew).Replace("name=\"origActionAlias\"", "").Replace("value=\"", "");
                                int endindex = start.IndexOf("\"");
                                string end = start.Substring(0, endindex);
                                origActionAlias = end.Replace("\"", string.Empty).Trim();
                            }
                        }

                        if (ResLogin.Contains("name=\"submissionId\""))
                        {
                            int startindexnew = ResLogin.IndexOf("name=\"submissionId\"");
                            if (startindexnew > 0)
                            {
                                string start = ResLogin.Substring(startindexnew).Replace("name=\"submissionId\"", string.Empty).Replace("value=\"",string.Empty);
                                int endindex = start.IndexOf("\"");
                                string end = start.Substring(0, endindex);
                                submissionID = end.Replace("\"", string.Empty).Trim();
                            }
                        }
                        if (ResLogin.Contains("name=\"CAPTCHA-fwd-csrfToken\""))
                        {
                            int startindexnew = ResLogin.IndexOf("name=\"CAPTCHA-fwd-csrfToken\"");
                                if(startindexnew>0)
                                {
                                    string start = ResLogin.Substring(startindexnew).Replace("name=\"CAPTCHA-fwd-csrfToken\"", string.Empty).Replace("value=\"",string.Empty);
                                    int endindex = start.IndexOf("\"");
                                    string end = start.Substring(0, endindex);
                                    CAPTCHAfwdcsrftoken = end.Replace("\"", string.Empty).Trim();
                                }
                        }
                        if (ResLogin.Contains("name=\"CAPTCHA-fwd-signin\""))
                        {
                            int startindexnew = ResLogin.IndexOf("name=\"CAPTCHA-fwd-signin\"");
                            if (startindexnew > 0)
                            {
                                string start = ResLogin.Substring(startindexnew).Replace("name=\"CAPTCHA-fwd-signin\"", string.Empty).Replace("value=\"",string.Empty);
                                int endindex = start.IndexOf("\"");
                                string end = start.Substring(0, endindex);
                                CAPTCHAfwdsignin = end.Replace("\"", string.Empty).Trim();
                            }
                        }
                        if (ResLogin.Contains("name=\"CAPTCHA-fwd-session_password\""))
                        {
                            int startindexnew = ResLogin.IndexOf("name=\"CAPTCHA-fwd-session_password\"");
                            if (startindexnew > 0)
                            {
                                string start = ResLogin.Substring(startindexnew).Replace("name=\"CAPTCHA-fwd-session_password\"", string.Empty).Replace("value=\"",string.Empty);
                                int endindex = start.IndexOf("\"");
                                string end = start.Substring(0, endindex);
                                CAPTCHAfwdsession_password = end.Replace("\"", string.Empty).Trim();
                             }
                        }
                        if (ResLogin.Contains("name=\"CAPTCHA-fwd-session_key\""))
                        {
                            int startindexnew = ResLogin.IndexOf("name=\"CAPTCHA-fwd-session_key\"");
                            if (startindexnew > 0)
                            {
                                string start = ResLogin.Substring(startindexnew).Replace("name=\"CAPTCHA-fwd-session_key\"", string.Empty).Replace("value=\"",string.Empty);
                                int endindex = start.IndexOf("\"");
                                string end = start.Substring(0, endindex);
                                CAPTCHAfwdsession_key = end.Replace("\"", string.Empty).Trim();
                            }
                        }
                        if (ResLogin.Contains("name=\"CAPTCHA-fwd-isJsEnabled\""))
                        {
                            int startindexnew = ResLogin.IndexOf("name=\"CAPTCHA-fwd-isJsEnabled\"");
                            if (startindexnew > 0)
                            {
                                string start = ResLogin.Substring(startindexnew).Replace("name=\"CAPTCHA-fwd-isJsEnabled\"", string.Empty).Replace("value=\"",string.Empty);
                                int endindex = start.IndexOf("\"");
                                string end = start.Substring(0, endindex);
                                CAPTCHAfwdisJsEnabled = end.Replace("\"", string.Empty).Trim();
                            }
                        }
                        if (ResLogin.Contains("name=\"CAPTCHA-fwd-loginCsrfParam\""))
                        {
                            int startindexnew = ResLogin.IndexOf("name=\"CAPTCHA-fwd-loginCsrfParam\"");
                            if (startindexnew > 0) ;
                            {
                                string start = ResLogin.Substring(startindexnew).Replace("name=\"CAPTCHA-fwd-loginCsrfParam\"", string.Empty).Replace("value=\"",string.Empty);
                                int endindex = start.IndexOf("\"");
                                string end = start.Substring(0, endindex);
                                CAPTCHAfwdloginCsrfParam = end.Replace("\"", string.Empty).Trim();
                            }
                        }


                        if (ResLogin.Contains("name=\"origSourceAlias\""))
                        {
                            int startindexnew = ResLogin.IndexOf("name=\"origSourceAlias\"");
                            if (startindexnew > 0)
                            {
                                string start = ResLogin.Substring(startindexnew).Replace("name=\"origSourceAlias\"", "").Replace("value=\"", "");
                                int endindex = start.IndexOf("\"");
                                string end = start.Substring(0, endindex);
                                origSourceAlias = end.Replace("\"", string.Empty).Trim();
                            }
                        }

                        if (ResLogin.Contains("name=\"irhf\""))
                        {
                            int startindexnew = ResLogin.IndexOf("name=\"irhf\"");
                            if (startindexnew > 0)
                            {
                                string start = ResLogin.Substring(startindexnew).Replace("name=\"irhf\"", "").Replace("value=\"", "");
                                int endindex = start.IndexOf("\"");
                                string end = start.Substring(0, endindex);
                                irhf = end.Replace("\"", string.Empty).Trim();
                            }
                        }

                        if (!string.IsNullOrEmpty(ImageUrl) && !string.IsNullOrEmpty(captchaText))
                        {
                            //postdata = "recaptcha_challenge_field=" + ImageUrl.Replace("https://www.google.com/recaptcha/api/image?c=", string.Empty) + "&recaptcha_response_field=" + captchaText.Replace(" ", "+") + "&dts=" + dts + "&security-challenge-id=" + captchachallengeid + "&hr=&source_app=&csrfToken=" + csrfToken + "&isJsEnabled=true&session_redirect=&session_password=" + accountPass + "&session_key=" + Uri.EscapeDataString(accountUser) + "&origSourceAlias=" + origSourceAlias + "&origActionAlias=" + origActionAlias + "&irhf=" + irhf + "&sourceAlias=" + sourceAlias + "&submissionId=" + submissionID + ;
                            postdata = "recaptcha_challenge_field=" + ImageUrl.Replace("https://www.google.com/recaptcha/api/image?c=", string.Empty) + "&recaptcha_response_field=" + captchaText.Replace(" ", "+") + "&irhf=" + irhf + "&dts=" + dts + "&security-challenge-id=" + captchachallengeid + "&submissionId=" + submissionID + "&CAPTCHA-fwd-csrfToken=" + CAPTCHAfwdcsrftoken + "&CAPTCHA-fwd-isJsEnabled=" + CAPTCHAfwdisJsEnabled + "&CAPTCHA-fwd-signin=" + CAPTCHAfwdsignin + "&CAPTCHA-fwd-loginCsrfParam=" + CAPTCHAfwdloginCsrfParam + "&CAPTCHA-fwd-session_password=" + CAPTCHAfwdsession_password + "&CAPTCHAfwd-session_key=" + CAPTCHAfwdsession_key + "&session_password=" + accountPass + "&session_key=" + Uri.EscapeDataString(accountUser) + "&origSourceAlias=" + origSourceAlias + "&origActionAlias=" + origActionAlias + "&csrfToken=" + csrfToken + "&sourceAlias=" + sourceAlias;
                            postdata = postdata.Replace(" ", string.Empty);
                            ResLogin = HttpHelper.postFormDataRef(new Uri("https://www.linkedin.com/uas/captcha-submit"), postdata, "https://www.linkedin.com/uas/login-submit", string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                        }
                        else
                        {
                            ResLogin = string.Empty;
                        }

                        if (ResLogin.Contains("The text you entered does not match the characters in the security image. Please try again with this new image") || string.IsNullOrEmpty(ResLogin))
                        {
                            Log("[ " + DateTime.Now + " ] => [ " + accountUser + "  Cannot Login because of Capctcha ]");
                            GlobusFileHelper.WriteStringToTextfile(accountUser + ":" + accountPass + ":" + proxyAddress + ":" + proxyPort + ":" + proxyUserName + ":" + proxyPassword, Globals.pathCapcthaLogin);
                            SearchCriteria.loginREsponce = string.Empty;
                            //FrmScrapGroupMember.ChangeToNextAccount = true;
                        }
                    }
                }

                if (ResLogin.Contains("Sign Out") || ResLogin.Contains("class=\"signout\"") || ResLogin.Contains("Cerrar sesión"))
                {
                    IsLoggedIn = true;
                    Log("[ " + DateTime.Now + " ] => [ Logged In With Account : " + accountUser + " ]");
                    try
                    {
                        string Search = HttpHelper.getHtmlfromUrl(new Uri("https://www.linkedin.com/search?trk=advsrch"));
                        SearchCriteria.loginREsponce = Search;

                     
                    }
                    catch { }
                }
                else if (ResLogin.Contains("logout?session_full_logout"))
                {
                    SearchCriteria.loginREsponce = string.Empty;
                    IsLoggedIn = true;
                    string Search = HttpHelper.getHtmlfromUrl(new Uri("https://www.linkedin.com/search?trk=advsrch"));
                    SearchCriteria.loginREsponce = Search;

                }
                else if (ResLogin.Contains("Sign-In Verification"))
                {
                    Log("[ " + DateTime.Now + " ] => [ Verification required : " + accountUser + " ]");
                }
                else if (ResLogin.Contains("Your LinkedIn account has been temporarily restricted"))
                {
                    Log("[ " + DateTime.Now + " ] => [ Your LinkedIn account : " + accountUser + " has been temporarily restricted ]");
                    //FrmScrapGroupMember.ChangeToNextAccount = true;
                }
                else
                {
                    //Log("[ " + DateTime.Now + " ] => [ NotLogged In With Account : " + SearchCriteria.LoginID + " ]");
                    Log("[ " + DateTime.Now + " ] => [ NotLogged In With Account : " + accountUser + " ]");
                    //FrmScrapGroupMember.ChangeToNextAccount = true;
                }

                #region MyRegion
                //Url = "http://www.linkedin.com/home?trk=hb_tab_home_top";
                //pageSrcLogin = HttpHelper.getHtmlfromUrlProxy(new Uri(Url), proxyAddress, 888, proxyUserName, proxyPassword);

                //LogoutHttpHelper(ref HttpHelper);

                //Url = "http://www.linkedin.com/home?trk=hb_tab_home_top";
                //pageSrcLogin = HttpHelper.getHtmlfromUrlProxy(new Uri(Url), proxyAddress, 888, proxyUserName, proxyPassword);
                #endregion
            }
            catch (Exception ex)
            {
                //Log("[ " + DateTime.Now + " ] => [ NotLogged In With Account : " + SearchCriteria.LoginID + " ]");
                //Log("[ " + DateTime.Now + " ] => [ NotLogged In With Account : " + accountUser + " ]");
                //FrmScrapGroupMember.ChangeToNextAccount = true;
            }
        }

        #endregion

        public void LoginWithChilkatHelper(ref ChilkatHttpHelpr HttpHelper1)
        {
            try
            {
               
                ChilkatHttpHelpr HttpHelper = HttpHelper1;

                Log("[ " + DateTime.Now + " ] => [ Logging In With Account : " + accountUser + " ]");
                Log("[ " + DateTime.Now + " ] => [ Login Process is Running... ]");
                Url = "https://www.linkedin.com/";
                string pageSrcLogin = string.Empty;
                int ProxyPort = 0;
                if (!string.IsNullOrEmpty(proxyPort) && NumberHelper.ValidateNumber(proxyPort))
                {
                    ProxyPort = int.Parse(proxyPort);
                }
                pageSrcLogin = HttpHelper.GetHtml(Url);

                if (pageSrcLogin.Contains("redirectUrl\":\"https://www.linkedin.com/nhome/"))
                {
                    pageSrcLogin = HttpHelper.GetHtml("https://www.linkedin.com/nhome/");
                }

                string postdata = string.Empty;
                string postUrl = string.Empty;
                string ResLogin = string.Empty;
                string csrfToken = string.Empty;
                string regCsrfParam = string.Empty;
                string sourceAlias = string.Empty;


                if (pageSrcLogin.Contains("csrfToken"))
                {
                    try
                    {
                        int startIndex = pageSrcLogin.IndexOf("name=\"csrfToken\"");
                        string start = pageSrcLogin.Substring(startIndex).Replace("name=\"csrfToken\"", "");
                        int endIndex = start.IndexOf("\" ");
                        string end = start.Substring(0, endIndex).Replace("value=\"", "").Trim();
                        csrfToken = end;
                        csrfToken = csrfToken.Replace("ajax:", "");
                    }
                    catch (Exception ex)
                    {

                    }

                }

                try
                {
                    if (csrfToken.Contains("&"))
                    {
                        string[] Arr = csrfToken.Split('&');
                        csrfToken = Arr[0].Replace("\"", string.Empty);

                    }

                }
                catch { }

                if (pageSrcLogin.Contains("sourceAlias"))
                {
                    sourceAlias = pageSrcLogin.Substring(pageSrcLogin.IndexOf("sourceAlias"), 100);
                    string[] Arr = sourceAlias.Split('"');
                    sourceAlias = Arr[2].Replace(@"\", string.Empty).Replace("//", string.Empty).Trim();
                }


                try
                {
                    int SourceAliasStart = pageSrcLogin.IndexOf("regCsrfParam");
                    if (SourceAliasStart > 0)
                    {
                        try
                        {

                            regCsrfParam = pageSrcLogin.Substring(pageSrcLogin.IndexOf("regCsrfParam"), 100);
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
                postdata = "isJsEnabled=true&source_app=&tryCount=&session_key=" + Uri.EscapeDataString(accountUser) + "&session_password=" + accountPass + "&signin=Sign%20In&session_redirect=&loginCsrfParam=" + regCsrfParam + "&csrfToken=" + csrfToken + "&sourceAlias=" + sourceAlias;

                try
                {
                    ResLogin = HttpHelper.PostFormData(postUrl, postdata, "https://www.linkedin.com/uas/login?goback=&trk=hb_signin");//HttpHelper.postFormDataRef(new Uri(postUrl), postdata, "http://www.linkedin.com/uas/login?goback=&trk=hb_signin", "", "");
                }
                catch { }

                //if (GroupStatus.GrouppageSourcewithProxy == string.Empty)
                //{
                //    GroupStatus.GrouppageSourcewithProxy = HttpHelper.getHtmlfromUrl(new Uri("http://www.linkedin.com/grp/"));
                //}

                string ImageUrl = string.Empty;
                string captchaText = string.Empty;
                string captchachallengeid = string.Empty;
                string dts = string.Empty;
                string origActionAlias = string.Empty;
                string origSourceAlias = string.Empty;
                string irhf = string.Empty;
                if (ResLogin.Contains("Security Verification"))
                {
                    string dataforcapctha = HttpHelper.GetHtml("https://www.google.com/recaptcha/api/noscript?k=6LcnacMSAAAAADoIuYvLUHSNLXdgUcq-jjqjBo5n");
                    if (!string.IsNullOrEmpty(dataforcapctha))
                    {
                        int startindex = dataforcapctha.IndexOf("id=\"recaptcha_challenge_field\"");
                        if (startindex > 0)
                        {
                            string start = dataforcapctha.Substring(startindex).Replace("id=\"recaptcha_challenge_field\"", "");
                            int endindex = start.IndexOf("\">");
                            string end = start.Substring(0, endindex).Replace("value=", "").Replace("\"", "");
                            ImageUrl = "https://www.google.com/recaptcha/api/image?c=" + end;
                            System.Net.WebClient webclient = new System.Net.WebClient();
                            byte[] args = webclient.DownloadData(ImageUrl);
                            string[] arr1 = new string[] { Globals.CapchaLoginID, Globals.CapchaLoginPassword, "" };
                            captchaText = DecodeDBC(arr1, args);
                        }

                        if (ResLogin.Contains("name=\"security-challenge-id\""))
                        {
                            int startindexnew = ResLogin.IndexOf("name=\"security-challenge-id\"");
                            if (startindexnew > 0)
                            {
                                string start = ResLogin.Substring(startindexnew).Replace("name=\"security-challenge-id\"", "").Replace("value=\"", "");
                                int endindex = start.IndexOf("\"");
                                string end = start.Substring(0, endindex);
                                captchachallengeid = end.Replace("\"", "");
                            }
                        }

                        if (ResLogin.Contains("name=\"dts\""))
                        {
                            int startindexnew = ResLogin.IndexOf("name=\"dts\"");
                            if (startindexnew > 0)
                            {
                                string start = ResLogin.Substring(startindexnew).Replace("name=\"dts\"", "").Replace("value=\"", "");
                                int endindex = start.IndexOf("\"");
                                string end = start.Substring(0, endindex);
                                dts = end.Replace("\"", "");
                            }
                        }

                        if (ResLogin.Contains("name=\"origActionAlias\""))
                        {
                            int startindexnew = ResLogin.IndexOf("name=\"origActionAlias\"");
                            if (startindexnew > 0)
                            {
                                string start = ResLogin.Substring(startindexnew).Replace("name=\"origActionAlias\"", "").Replace("value=\"", "");
                                int endindex = start.IndexOf("\"");
                                string end = start.Substring(0, endindex);
                                origActionAlias = end.Replace("\"", "");
                            }
                        }

                        if (ResLogin.Contains("name=\"origSourceAlias\""))
                        {
                            int startindexnew = ResLogin.IndexOf("name=\"origSourceAlias\"");
                            if (startindexnew > 0)
                            {
                                string start = ResLogin.Substring(startindexnew).Replace("name=\"origSourceAlias\"", "").Replace("value=\"", "");
                                int endindex = start.IndexOf("\"");
                                string end = start.Substring(0, endindex);
                                origSourceAlias = end.Replace("\"", "");
                            }
                        }

                        if (ResLogin.Contains("name=\"irhf\""))
                        {
                            int startindexnew = ResLogin.IndexOf("name=\"irhf\"");
                            if (startindexnew > 0)
                            {
                                string start = ResLogin.Substring(startindexnew).Replace("name=\"irhf\"", "").Replace("value=\"", "");
                                int endindex = start.IndexOf("\"");
                                string end = start.Substring(0, endindex);
                                irhf = end.Replace("\"", "");
                            }
                        }

                        if (!string.IsNullOrEmpty(ImageUrl) && !string.IsNullOrEmpty(captchaText))
                        {
                            postdata = "recaptcha_challenge_field=" + ImageUrl.Replace("https://www.google.com/recaptcha/api/image?c=", "") + "&recaptcha_response_field=" + captchaText.Replace(" ", "+") + "&dts=" + dts + "&security-challenge-id=" + captchachallengeid + "&hr=&source_app=&csrfToken=" + csrfToken + "&isJsEnabled=true&session_redirect=&session_password=" + accountPass + "&session_key=" + Uri.EscapeDataString(accountUser) + "&origSourceAlias=" + origSourceAlias + "&origActionAlias=" + origActionAlias + "&irhf=" + irhf + "&sourceAlias=" + sourceAlias;
                            postdata = postdata.Replace(" ", "");
                            ResLogin = HttpHelper.PostFormData("https://www.linkedin.com/uas/captcha-submit", postdata, "https://www.linkedin.com/uas/login-submit");                       }
                        else
                        {
                            ResLogin = "";
                        }

                        if (ResLogin.Contains("The text you entered does not match the characters in the security image. Please try again with this new image") || string.IsNullOrEmpty(ResLogin))
                        {
                            Log("[ " + DateTime.Now + " ] => [ " + accountUser + "  Cannot Login because of Capctcha ]");
                            GlobusFileHelper.WriteStringToTextfile(accountUser + ":" + accountPass + ":" + proxyAddress + ":" + proxyPort + ":" + proxyUserName + ":" + proxyPassword, Globals.pathCapcthaLogin);
                            SearchCriteria.loginREsponce = string.Empty;
                        }
                    }
                }

                if (ResLogin.Contains("Sign Out") || ResLogin.Contains("class=\"signout\""))
                {
                    IsLoggedIn = true;
                    Log("[ " + DateTime.Now + " ] => [ Logged In With Account : " + accountUser + " ]");
                    try
                    {
                        //string Search = HttpHelper.getHtmlfromUrl(new Uri("http://www.linkedin.com/search?trk=advsrch"));
                        //SearchCriteria.loginREsponce = Search;


                    }
                    catch { }
                }
                else if (ResLogin.Contains("logout?session_full_logout"))
                {
                    SearchCriteria.loginREsponce = string.Empty;
                    IsLoggedIn = true;
                    //string Search = HttpHelper.getHtmlfromUrl(new Uri("http://www.linkedin.com/search?trk=advsrch"));
                    //SearchCriteria.loginREsponce = Search;

                }
                else if (ResLogin.Contains("Sign-In Verification"))
                {
                    Log("[ " + DateTime.Now + " ] => [ Verification required : " + accountUser + " ]");
                }
                else if (ResLogin.Contains("Your LinkedIn account has been temporarily restricted"))
                {
                    Log("[ " + DateTime.Now + " ] => [ Your LinkedIn account : " + accountUser + " has been temporarily restricted ]");
                    //GlobusFileHelper.WriteStringToTextfile(accountUser + ":" + accountPass + ":" + proxyAddress + ":" + proxyPort + ":" + proxyUserName + ":" + proxyPassword, Globals.path_FailLogin);
                }
                else
                {
                    Log("[ " + DateTime.Now + " ] => [ NotLogged In With Account : " + accountUser + " ]");
                    GlobusFileHelper.WriteStringToTextfile(accountUser + ":" + accountPass + ":" + proxyAddress + ":" + proxyPort + ":" + proxyUserName + ":" + proxyPassword, Globals.path_FailLogin);
                }

                #region MyRegion
                //Url = "http://www.linkedin.com/home?trk=hb_tab_home_top";
                //pageSrcLogin = HttpHelper.getHtmlfromUrlProxy(new Uri(Url), proxyAddress, 888, proxyUserName, proxyPassword);

                //LogoutHttpHelper(ref HttpHelper);

                //Url = "http://www.linkedin.com/home?trk=hb_tab_home_top";
                //pageSrcLogin = HttpHelper.getHtmlfromUrlProxy(new Uri(Url), proxyAddress, 888, proxyUserName, proxyPassword);
                #endregion
            }
            catch (Exception ex)
            {
                Log("[ " + DateTime.Now + " ] => [ NotLogged In With Account : " + accountUser + " ]");
                GlobusFileHelper.WriteStringToTextfile(accountUser + ":" + accountPass + ":" + proxyAddress + ":" + proxyPort + ":" + proxyUserName + ":" + proxyPassword, Globals.path_FailLogin);
            }
        }

        #region DecodeDBC
        public string DecodeDBC(string[] args, byte[] imageBytes)
        {
            try
            {
                // Put your DBC username & password here:
                DeathByCaptcha.Client client = (DeathByCaptcha.Client)new DeathByCaptcha.SocketClient(args[0], args[1]);
                client.Verbose = true;

                Console.WriteLine("Your balance is {0:F2} US cents", client.Balance);//Log("Your balance is " + client.Balance + " US cents ");

                if (!client.User.LoggedIn)
                {
                    Log("[ " + DateTime.Now + " ] => [ Please check your DBC Account Details ]");
                    return null;
                }
                if (client.Balance == 0.0)
                {
                    Log("[ " + DateTime.Now + " ] => [ You have 0 Balance in your DBC Account ]");
                    return null;
                }

                for (int i = 2, l = args.Length; i < l; i++)
                {
                    Console.WriteLine("Solving CAPTCHA {0}", args[i]);

                    // Upload a CAPTCHA and poll for its status.  Put the CAPTCHA image
                    // file name, file object, stream, or a vector of bytes, and desired
                    // solving timeout (in seconds) here:
                    DeathByCaptcha.Captcha captcha = client.Decode(imageBytes, 2 * DeathByCaptcha.Client.DefaultTimeout);
                    if (null != captcha)
                    {
                        Console.WriteLine("CAPTCHA {0:D} solved: {1}", captcha.Id, captcha.Text);

                        //// Report an incorrectly solved CAPTCHA.
                        //// Make sure the CAPTCHA was in fact incorrectly solved, do not
                        //// just report it at random, or you might be banned as abuser.
                        //if (client.Report(captcha))
                        //{
                        //    Console.WriteLine("Reported as incorrectly solved");
                        //}
                        //else
                        //{
                        //    Console.WriteLine("Failed reporting as incorrectly solved");
                        //}

                        return captcha.Text;
                    }
                    else
                    {
                        Log("[ " + DateTime.Now + " ] => [ CAPTCHA was not solved ]");
                        Console.WriteLine("CAPTCHA was not solved");
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TwitterSignup -  SignupMultiThreaded() -- DecodeDBC()  --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("Error --> TwitterSignup -  DecodeDBC() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + " || DateTime :- " + DateTime.Now, Globals.Path_LinkedinAccountCraetionErrorLogs);
            }
            return null;
        }
        #endregion

        #region LoginHttpHelper
        public void LoginHttpHelper(ref GlobusHttpHelper HttpHelper, string userName, string password, string Proxyaddress, string ProxyUserName, string ProxyPass, string port)
        {
            try
            {
                this.accountUser = userName;
                this.accountPass = password;
                this.proxyAddress = Proxyaddress;
                this.proxyPassword = ProxyPass;
                this.proxyUserName = ProxyUserName;
                this.proxyPort = port;
                Log("[ " + DateTime.Now + " ] => [ Logging In With : " + accountUser + " ]");
                Log("[ " + DateTime.Now + " ] => [ Login Process is Running... ]");
                Url = "https://www.linkedin.com/";
                string pageSrcLogin = string.Empty;
                if (string.IsNullOrEmpty(proxyPort))
                {
                    proxyPort = (0).ToString();
                }
                pageSrcLogin = HttpHelper.getHtmlfromUrlProxy(new Uri(Url), proxyAddress, Convert.ToInt32(proxyPort), proxyUserName, proxyPassword);

                string postdata = string.Empty;
                string postUrl = string.Empty;
                string ResLogin = string.Empty;
                string csrfToken = string.Empty;
                string sourceAlias = string.Empty;

                if (pageSrcLogin.Contains("csrfToken"))
                {
                    int startCsrfToken = pageSrcLogin.IndexOf("name=\"csrfToken\"");
                    if (startCsrfToken > 0)
                    {
                        string startcsrfToken = pageSrcLogin.Substring(startCsrfToken).Replace("name=\"csrfToken\"", "").Replace("value=\"", "");
                        int endCsrfToken = startcsrfToken.IndexOf("\"");
                        string endcsrftoken = startcsrfToken.Substring(0, endCsrfToken);
                        csrfToken = endcsrftoken.Replace(@"\", string.Empty).Replace("//", string.Empty).Replace(" ", "");
                    }
                }

                if (pageSrcLogin.Contains("sourceAlias"))
                {
                    sourceAlias = pageSrcLogin.Substring(pageSrcLogin.IndexOf("sourceAlias"), 100);
                    string[] Arr = sourceAlias.Split('"');
                    sourceAlias = Arr[2].Replace(@"\", string.Empty).Replace("//", string.Empty);
                }
              
                postUrl = "https://www.linkedin.com/uas/login-submit";
                postdata = "session_key=" + accountUser + "&session_password=" + accountPass + "&source_app=&trk=guest_home_login&session_redirect=&csrfToken=" + csrfToken + "&sourceAlias=" + sourceAlias;

                //ResLogin = HttpHelper.postFormData(new Uri(postUrl), postdata);

                ResLogin = HttpHelper.postFormDataProxy(new Uri(postUrl), postdata, proxyAddress, int.Parse(proxyPort), proxyUserName, proxyPassword);

                if (ResLogin.Contains("Sign Out") && ResLogin.Contains("class=\"signout\""))
                {
                    IsLoggedIn = true;
                    Log("[ " + DateTime.Now + " ] => [ Logged In With Account : " + accountUser + " ]");
                    string Search = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/search?trk=advsrch"));
                    SearchCriteria.loginREsponce = Search;
                }

                #region MyRegion
                //Url = "http://www.linkedin.com/home?trk=hb_tab_home_top";
                //pageSrcLogin = HttpHelper.getHtmlfromUrlProxy(new Uri(Url), proxyAddress, 888, proxyUserName, proxyPassword);

                //LogoutHttpHelper(ref HttpHelper);

                //Url = "http://www.linkedin.com/home?trk=hb_tab_home_top";
                //pageSrcLogin = HttpHelper.getHtmlfromUrlProxy(new Uri(Url), proxyAddress, 888, proxyUserName, proxyPassword);
                #endregion
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region LogoutHttpHelper()
        public void LogoutHttpHelper()
        {
            string Url = string.Empty;
            Url = "https://www.linkedin.com/home?trk=hb_tab_home_top";
            //string pageSrcLogin = HttpChilkat.GetHtmlProxy(Url, proxyAddress, proxyPort, proxyUserName, proxyPassword);
            string pageSrcHome = HttpHelper.getHtmlfromUrl1(new Uri(Url));

            Url = "https://www.linkedin.com/secure/login?session_full_logout=&trk=hb_signout";
            string pageSrcLogout = HttpHelper.getHtmlfromUrl1(new Uri(Url));
        }
        #endregion

        #region ResendConfirmation
        public void ResendConfirmation(ref GlobusHttpHelper HttpHelper)
        {
            string postdata = string.Empty;
            string postUrl = string.Empty;
            string ResLogin = string.Empty;
            string csrfToken = string.Empty;
            string emailid = string.Empty;
            try
            {
                Log("[ " + DateTime.Now + " ] => [ Starting Sending Invitation To Account : " + accountUser + " ]");
                if (!NumberHelper.ValidateNumber(proxyPort))
                {
                    proxyPort = "0";
                }

                string GetHomePageSOurce = HttpHelper.getHtmlfromUrlProxy(new Uri("http://www.linkedin.com/settings/manage-email?goback=%2Enas_*1_*1_*1%2Enas_profile_nsettings*5manage*5email_*1"), proxyAddress, int.Parse(proxyPort), proxyUserName, proxyPassword);
                string LinkToVerfiy = string.Empty;
                if (GetHomePageSOurce.Contains(accountUser) && !GetHomePageSOurce.Contains("Primary address"))
                //if (GetHomePageSOurce.Contains(accountUser))
                {
                    try
                    {
                        if (GetHomePageSOurce.Contains("csrfToken"))
                        {
                            csrfToken = GetHomePageSOurce.Substring(GetHomePageSOurce.IndexOf("csrfToken"), 100);
                            string[] Arr = csrfToken.Split('"');
                            try
                            {
                                csrfToken = Arr[2].Replace(@"\", string.Empty).Replace("//", string.Empty);
                            }
                            catch (Exception ex)
                            {
                                csrfToken = Arr[0].Replace("csrfToken=", "").Replace("\\", "");
                            }
                    
                            try
                            {
                                if (csrfToken.Contains("&"))
                                {
                                    string[] Arr1 = csrfToken.Split('&');
                                    csrfToken = Arr1[0].Replace("\"", string.Empty);

                                }
                            }
                            catch { }
                        }

                        if (GetHomePageSOurce.Contains("entry primary") || GetHomePageSOurce.Contains("CHOOSE YOUR PRIMARY EMAIL ADDRESS"))
                        {
                            try
                            {
                                emailid = GetHomePageSOurce.Substring(GetHomePageSOurce.IndexOf("truncated-"), (GetHomePageSOurce.IndexOf("callout-content") - GetHomePageSOurce.IndexOf("truncated-")));
                                emailid = emailid.Replace("truncated-", string.Empty).Replace(">",string.Empty).Replace("class=",string.Empty).Replace("callout-container",string.Empty).Replace("email",string.Empty).Replace("style=",string.Empty).Replace("display: none",string.Empty).Replace("\"",string.Empty).Replace("\n",string.Empty).Replace("<div",string.Empty).Trim();
                               
                            }
                            catch { }
                        }

                        postdata = "https://www.linkedin.com/settings/manageEmailConfirmation-submit?emailID=" + emailid.Replace(" ", "") + "&csrfToken=" + csrfToken + "&goback=%2Enas_*1_*1_*1%2Enas_*1_*1_*1%2Enas_profile_nsettings*5manage*5email_*1";
                        ResLogin = HttpHelper.getHtmlfromUrlProxy(new Uri(postdata), proxyAddress, int.Parse(proxyPort), proxyUserName, proxyPassword);
                        if (ResLogin.Contains("We've sent a confirmation email to") || ResLogin.Contains("\"status\":\"ok\"}") || ResLogin.Contains("find the email from LinkedIn, and click the confirm button"))
                        {
                            Log("[ " + DateTime.Now + " ] => [ Resent Confirmation To Account : " + accountUser + " ]");
                            GlobusFileHelper.AppendStringToTextfileNewLine(accountUser + ":" + accountPass + ":" + proxyAddress + ":" + proxyPort + ":" + proxyUserName + ":" + proxyPassword, Globals.path_ResentVerfication);
                        }
                        else
                        {
                            Log("[ " + DateTime.Now + " ] => [ Confirmation Not Sent To Account : " + accountUser + " ]");
                            GlobusFileHelper.AppendStringToTextfileNewLine(accountUser + ":" + accountPass + ":" + proxyAddress + ":" + proxyPort + ":" + proxyUserName + ":" + proxyPassword, Globals.path_NonResentVerfication);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log("[ " + DateTime.Now + " ] => [ Error in Sending Confirmation , Not Sent To Account : " + accountUser + " ]");
                        GlobusFileHelper.AppendStringToTextfileNewLine(accountUser + ":" + accountPass + ":" + proxyAddress + ":" + proxyPort + ":" + proxyUserName + ":" + proxyPassword, Globals.path_NonResentVerfication);
                    }
                }
                else
                {
                    Log("[ " + DateTime.Now + " ] => [ Email Already Verfied For Account : " + accountUser + " ]");
                    GlobusFileHelper.AppendStringToTextfileNewLine(accountUser + ":" + accountPass + ":" + proxyAddress + ":" + proxyPort + ":" + proxyUserName + ":" + proxyPassword, Globals.path_NonResentVerfication);
                }
            }
            catch (Exception ex)
            {
                Log("[ " + DateTime.Now + " ] => [ Error in Sending Confirmation , Not Sent To Account : " + accountUser + " ]");
                GlobusFileHelper.AppendStringToTextfileNewLine(accountUser + ":" + accountPass + ":" + proxyAddress + ":" + proxyPort + ":" + proxyUserName + ":" + proxyPassword, Globals.path_NonResentVerfication);
            }
            Log("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED With Account : " + accountUser + " ]");
            Log("-----------------------------------------------------------------------------------------------------------------------------------");
        }
        #endregion

        #region PostAddMembers
        //public Dictionary<string, string> PostAddMembers(ref GlobusHttpHelper HttpHelper, string user)
        //{
        //    try
        //    {
        //        string MemId = string.Empty;
        //        string MemFName = string.Empty;
        //        string MemLName = string.Empty;
        //        string MemFullName = string.Empty;


        //        #region Commented Universal code
        //        string pageSource = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/contacts/api/contacts/"));

        //        if (!pageSource.Contains("success"))
        //        {
        //            pageSource = HttpHelper.getHtmlfromUrl1(new Uri("https://www.linkedin.com/contacts/#?filter=recent"));
        //        }


        //        string[] RgxGroupData = System.Text.RegularExpressions.Regex.Split(pageSource, "{\"name\"");

        //        foreach (var Members in RgxGroupData)
        //        {
        //            string Fname = string.Empty;

        //            if (Members.Contains("title"))
        //            {
        //                try
        //                {
        //                    int startindex = Members.IndexOf(", \"id\": \"li_");
        //                    string start = Members.Substring(startindex).Replace(", \"id\": \"li_", string.Empty);
        //                    int endIndex = start.IndexOf("\"}");
        //                    MemId = start.Substring(0, endIndex).Trim();
        //                    MemId = user + ':' + MemId;



        //                    int StartIndex1 = Members.IndexOf(": \"");
        //                    if (StartIndex1 == 0)
        //                    {
        //                        string Start = Members.Substring(StartIndex1).Replace(": \"", string.Empty);
        //                        int EndIndex = Start.IndexOf("\",");
        //                        string End = Start.Substring(0, EndIndex);
        //                        Fname = End;
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> PostAddMembers() >> 1 >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
        //                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> PostAddMembers() >> 1 >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddFriendsGroupErrorLogs);
        //                }
        //        #endregion

        //                MemFullName = Fname;

        //                try
        //                {
        //                    MemberNameAndID.Add(MemId, MemFullName);
        //                }
        //                catch (Exception ex)
        //                {
        //                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> PostAddMembers() >> 2 >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
        //                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> PostAddMembers() >> 2 >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddFriendsGroupErrorLogs);
        //                }
        //            }
        //        }

        //        #region (Marry loadness) proxy issue


        //        // while (i < Convert.ToInt16(totalPageNumber))
        //        {
        //            if (MemberNameAndID.Count() == 0)
        //            {
        //                string pageSource1 = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/contacts/api/contacts/?start=0&count=1"));
        //                //string pageSource1 = HttpHelper.getHtmlfromUrl(new Uri("http://www.linkedin.com/contacts/api/contacts/?start="+i+"&count="+y));
        //                if (pageSource1.Contains("total"))
        //                {
        //                    int sIndex = pageSource1.IndexOf("\"total\"");
        //                    string _start = pageSource1.Substring(sIndex);
        //                    int eIndex = pageSource1.IndexOf(",");
        //                    totalPageNumber = _start.Substring(0, eIndex).Replace("last", string.Empty).Replace(":", string.Empty).Replace("\"", string.Empty).Replace("total", string.Empty).Replace(",", string.Empty).Replace("}", string.Empty).Trim();
        //                }
        //                for (int start = 0; start < Convert.ToInt16(totalPageNumber); start += 10)
        //                {
        //                    pageSource1 = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/contacts/api/contacts/?start=" + start + "&count=10"));

        //                    string[] RgxGroupData1 = System.Text.RegularExpressions.Regex.Split(pageSource1, "{\"name\"");
        //                    foreach (var Members1 in RgxGroupData1)
        //                    {

        //                        if (Members1.Contains("title"))
        //                        {
        //                            string Fname1 = string.Empty;
        //                            try
        //                            {

        //                                int startindex2 = Members1.IndexOf(", \"id\"");
        //                                string start2 = Members1.Substring(startindex2);
        //                                int endIndex2 = start2.IndexOf("}");
        //                                // int endIndex2 = start2.IndexOf("class=");
        //                                //MemId = start2.Substring(0, endIndex2).Replace("\"", string.Empty).Trim().Replace(",", string.Empty);
        //                                MemId = start2.Substring(0, endIndex2).Replace("\"", string.Empty).Trim().Replace("li_", string.Empty).Replace(",", string.Empty).Split(':')[1];
        //                                MemId = user + ':' + MemId;

        //                                int StartIndex3 = Members1.IndexOf(":");
        //                                string Start3 = Members1.Substring(StartIndex3).Replace(":", string.Empty);
        //                                int EndIndex3 = Start3.IndexOf("title");
        //                                string End = Start3.Substring(0, EndIndex3).Replace("\"", string.Empty).Trim().Replace(",", string.Empty); ;
        //                                Fname1 = End;

        //                            }
        //                            catch (Exception ex)
        //                            {
        //                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> PostAddMembers() >> 3 >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
        //                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> PostAddMembers() >> 3 >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddFriendsGroupErrorLogs);
        //                            }

        //                            MemFullName = Fname1;

        //                            try
        //                            {
        //                                MemberNameAndID.Add(MemId, MemFullName);
        //                            }
        //                            catch (Exception ex)
        //                            {
        //                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> PostAddMembers() >> 4 >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
        //                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> PostAddMembers() >> 4 >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddFriendsGroupErrorLogs);
        //                            }

        //                        }
        //                    }
        //                }

        //            }
        //        }
        //        #endregion

        //        return MemberNameAndID;
        //    }
        //    catch (Exception ex)
        //    {
        //        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> PostAddMembers() >> 5 >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
        //        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Friends Groups --> PostAddMembers() >> 5 >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddFriendsGroupErrorLogs);
        //        return MemberNameAndID;
        //    }
        //}
        #endregion

        #region LoginHttpHelper
        public void LoginHttpHelper_Checker(ref GlobusHttpHelper HttpHelper)
        {
            try
            {
                this.HttpHelper = HttpHelper;

                //Log("[ " + DateTime.Now + " ] => [ Logging In With Account : " + SearchCriteria.LoginID + " ]");
                Log("[ " + DateTime.Now + " ] => [ Logging In With Account : " + accountUser + " ]");
                Log("[ " + DateTime.Now + " ] => [ Login Process is Running... ]");
                Url = "https://www.linkedin.com/";
                string pageSrcLogin = string.Empty;
                int ProxyPort = 0;
                if (!string.IsNullOrEmpty(proxyPort) && NumberHelper.ValidateNumber(proxyPort))
                {
                    ProxyPort = int.Parse(proxyPort);
                }
                pageSrcLogin = HttpHelper.getHtmlfromUrlProxy(new Uri(Url), proxyAddress, ProxyPort, proxyUserName, proxyPassword);

                string postdata = string.Empty;
                string postUrl = string.Empty;
                string ResLogin = string.Empty;
                string csrfToken = string.Empty;
                string regCsrfParam = string.Empty;
                string sourceAlias = string.Empty;


                if (pageSrcLogin.Contains("csrfToken"))
                {
                    try
                    {
                        int startIndex = pageSrcLogin.IndexOf("name=\"csrfToken\"");
                        string start = pageSrcLogin.Substring(startIndex).Replace("name=\"csrfToken\"", "");
                        int endIndex = start.IndexOf("\" ");
                        string end = start.Substring(0, endIndex).Replace("value=\"", "").Trim();
                        csrfToken = end;
                        //csrfToken = csrfToken;
                    }
                    catch (Exception ex)
                    {

                    }

                }

                try
                {
                    if (csrfToken.Contains("&"))
                    {
                        string[] Arr = csrfToken.Split('&');
                        csrfToken = Arr[0].Replace("\"", string.Empty);

                    }

                }
                catch { }

                if (pageSrcLogin.Contains("sourceAlias"))
                {
                    sourceAlias = pageSrcLogin.Substring(pageSrcLogin.IndexOf("sourceAlias"), 100);
                    string[] Arr = sourceAlias.Split('"');
                    sourceAlias = Arr[2].Replace(@"\", string.Empty).Replace("//", string.Empty).Trim();
                }


                try
                {
                    int SourceAliasStart = pageSrcLogin.IndexOf("regCsrfParam");
                    if (SourceAliasStart > 0)
                    {
                        try
                        {

                            regCsrfParam = pageSrcLogin.Substring(pageSrcLogin.IndexOf("regCsrfParam"), 100);
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
                postdata = "isJsEnabled=true&source_app=&tryCount=&session_key=" + Uri.EscapeDataString(accountUser) + "&session_password=" + Uri.EscapeDataString(accountPass) + "&signin=Sign%20In&session_redirect=&loginCsrfParam=" + regCsrfParam + "&csrfToken=" + csrfToken + "&sourceAlias=" + sourceAlias;

                try
                {
                    ResLogin = HttpHelper.postFormDataProxy(new Uri(postUrl), postdata, proxyAddress, ProxyPort, proxyUserName, proxyPassword);//HttpHelper.postFormDataRef(new Uri(postUrl), postdata, "http://www.linkedin.com/uas/login?goback=&trk=hb_signin", "", "");
                }
                catch { }

                //if (GroupStatus.GrouppageSourcewithProxy == string.Empty)
                //{
                //    GroupStatus.GrouppageSourcewithProxy = HttpHelper.getHtmlfromUrl(new Uri("http://www.linkedin.com/grp/"));
                //}

                string ImageUrl = string.Empty;
                string captchaText = string.Empty;
                string captchachallengeid = string.Empty;
                string dts = string.Empty;
                string origActionAlias = string.Empty;
                string origSourceAlias = string.Empty;
                string irhf = string.Empty;
                string submissionID = string.Empty;
                string CAPTCHAfwdcsrftoken = string.Empty;
                string CAPTCHAfwdsignin = string.Empty;
                string CAPTCHAfwdsession_password = string.Empty;
                string CAPTCHAfwdsession_key = string.Empty;
                string CAPTCHAfwdisJsEnabled = string.Empty;
                string CAPTCHAfwdloginCsrfParam = string.Empty;



                if (ResLogin.Contains("Security Verification"))
                {
                    string dataforcapctha = HttpHelper.getHtmlfromUrl1(new Uri("https://www.google.com/recaptcha/api/noscript?k=6LcnacMSAAAAADoIuYvLUHSNLXdgUcq-jjqjBo5n"));
                    if (!string.IsNullOrEmpty(dataforcapctha))
                    {
                        int startindex = dataforcapctha.IndexOf("id=\"recaptcha_challenge_field\"");
                        if (startindex > 0)
                        {
                            string start = dataforcapctha.Substring(startindex).Replace("id=\"recaptcha_challenge_field\"", "");
                            int endindex = start.IndexOf("\">");
                            string end = start.Substring(0, endindex).Replace("value=", string.Empty).Replace("\"", string.Empty).Trim();
                            ImageUrl = "https://www.google.com/recaptcha/api/image?c=" + end;
                            System.Net.WebClient webclient = new System.Net.WebClient();
                            byte[] args = webclient.DownloadData(ImageUrl);
                            string[] arr1 = new string[] { Globals.CapchaLoginID, Globals.CapchaLoginPassword, "" };
                            captchaText = DecodeDBC(arr1, args);
                        }

                        if (ResLogin.Contains("name=\"security-challenge-id\""))
                        {
                            int startindexnew = ResLogin.IndexOf("name=\"security-challenge-id\"");
                            if (startindexnew > 0)
                            {
                                string start = ResLogin.Substring(startindexnew).Replace("name=\"security-challenge-id\"", "").Replace("value=\"", "");
                                int endindex = start.IndexOf("\"");
                                string end = start.Substring(0, endindex);
                                captchachallengeid = end.Replace("\"", string.Empty).Trim();
                            }
                        }

                        if (ResLogin.Contains("name=\"dts\""))
                        {
                            int startindexnew = ResLogin.IndexOf("name=\"dts\"");
                            if (startindexnew > 0)
                            {
                                string start = ResLogin.Substring(startindexnew).Replace("name=\"dts\"", "").Replace("value=\"", "");
                                int endindex = start.IndexOf("\"");
                                string end = start.Substring(0, endindex);
                                dts = end.Replace("\"", string.Empty).Trim();
                            }
                        }

                        if (ResLogin.Contains("name=\"origActionAlias\""))
                        {
                            int startindexnew = ResLogin.IndexOf("name=\"origActionAlias\"");
                            if (startindexnew > 0)
                            {
                                string start = ResLogin.Substring(startindexnew).Replace("name=\"origActionAlias\"", "").Replace("value=\"", "");
                                int endindex = start.IndexOf("\"");
                                string end = start.Substring(0, endindex);
                                origActionAlias = end.Replace("\"", string.Empty).Trim();
                            }
                        }

                        if (ResLogin.Contains("name=\"submissionId\""))
                        {
                            int startindexnew = ResLogin.IndexOf("name=\"submissionId\"");
                            if (startindexnew > 0)
                            {
                                string start = ResLogin.Substring(startindexnew).Replace("name=\"submissionId\"", string.Empty).Replace("value=\"", string.Empty);
                                int endindex = start.IndexOf("\"");
                                string end = start.Substring(0, endindex);
                                submissionID = end.Replace("\"", string.Empty).Trim();
                            }
                        }
                        if (ResLogin.Contains("name=\"CAPTCHA-fwd-csrfToken\""))
                        {
                            int startindexnew = ResLogin.IndexOf("name=\"CAPTCHA-fwd-csrfToken\"");
                            if (startindexnew > 0)
                            {
                                string start = ResLogin.Substring(startindexnew).Replace("name=\"CAPTCHA-fwd-csrfToken\"", string.Empty).Replace("value=\"", string.Empty);
                                int endindex = start.IndexOf("\"");
                                string end = start.Substring(0, endindex);
                                CAPTCHAfwdcsrftoken = end.Replace("\"", string.Empty).Trim();
                            }
                        }
                        if (ResLogin.Contains("name=\"CAPTCHA-fwd-signin\""))
                        {
                            int startindexnew = ResLogin.IndexOf("name=\"CAPTCHA-fwd-signin\"");
                            if (startindexnew > 0)
                            {
                                string start = ResLogin.Substring(startindexnew).Replace("name=\"CAPTCHA-fwd-signin\"", string.Empty).Replace("value=\"", string.Empty);
                                int endindex = start.IndexOf("\"");
                                string end = start.Substring(0, endindex);
                                CAPTCHAfwdsignin = end.Replace("\"", string.Empty).Trim();
                            }
                        }
                        if (ResLogin.Contains("name=\"CAPTCHA-fwd-session_password\""))
                        {
                            int startindexnew = ResLogin.IndexOf("name=\"CAPTCHA-fwd-session_password\"");
                            if (startindexnew > 0)
                            {
                                string start = ResLogin.Substring(startindexnew).Replace("name=\"CAPTCHA-fwd-session_password\"", string.Empty).Replace("value=\"", string.Empty);
                                int endindex = start.IndexOf("\"");
                                string end = start.Substring(0, endindex);
                                CAPTCHAfwdsession_password = end.Replace("\"", string.Empty).Trim();
                            }
                        }
                        if (ResLogin.Contains("name=\"CAPTCHA-fwd-session_key\""))
                        {
                            int startindexnew = ResLogin.IndexOf("name=\"CAPTCHA-fwd-session_key\"");
                            if (startindexnew > 0)
                            {
                                string start = ResLogin.Substring(startindexnew).Replace("name=\"CAPTCHA-fwd-session_key\"", string.Empty).Replace("value=\"", string.Empty);
                                int endindex = start.IndexOf("\"");
                                string end = start.Substring(0, endindex);
                                CAPTCHAfwdsession_key = end.Replace("\"", string.Empty).Trim();
                            }
                        }
                        if (ResLogin.Contains("name=\"CAPTCHA-fwd-isJsEnabled\""))
                        {
                            int startindexnew = ResLogin.IndexOf("name=\"CAPTCHA-fwd-isJsEnabled\"");
                            if (startindexnew > 0)
                            {
                                string start = ResLogin.Substring(startindexnew).Replace("name=\"CAPTCHA-fwd-isJsEnabled\"", string.Empty).Replace("value=\"", string.Empty);
                                int endindex = start.IndexOf("\"");
                                string end = start.Substring(0, endindex);
                                CAPTCHAfwdisJsEnabled = end.Replace("\"", string.Empty).Trim();
                            }
                        }
                        if (ResLogin.Contains("name=\"CAPTCHA-fwd-loginCsrfParam\""))
                        {
                            int startindexnew = ResLogin.IndexOf("name=\"CAPTCHA-fwd-loginCsrfParam\"");
                            if (startindexnew > 0) ;
                            {
                                string start = ResLogin.Substring(startindexnew).Replace("name=\"CAPTCHA-fwd-loginCsrfParam\"", string.Empty).Replace("value=\"", string.Empty);
                                int endindex = start.IndexOf("\"");
                                string end = start.Substring(0, endindex);
                                CAPTCHAfwdloginCsrfParam = end.Replace("\"", string.Empty).Trim();
                            }
                        }


                        if (ResLogin.Contains("name=\"origSourceAlias\""))
                        {
                            int startindexnew = ResLogin.IndexOf("name=\"origSourceAlias\"");
                            if (startindexnew > 0)
                            {
                                string start = ResLogin.Substring(startindexnew).Replace("name=\"origSourceAlias\"", "").Replace("value=\"", "");
                                int endindex = start.IndexOf("\"");
                                string end = start.Substring(0, endindex);
                                origSourceAlias = end.Replace("\"", string.Empty).Trim();
                            }
                        }

                        if (ResLogin.Contains("name=\"irhf\""))
                        {
                            int startindexnew = ResLogin.IndexOf("name=\"irhf\"");
                            if (startindexnew > 0)
                            {
                                string start = ResLogin.Substring(startindexnew).Replace("name=\"irhf\"", "").Replace("value=\"", "");
                                int endindex = start.IndexOf("\"");
                                string end = start.Substring(0, endindex);
                                irhf = end.Replace("\"", string.Empty).Trim();
                            }
                        }

                        if (!string.IsNullOrEmpty(ImageUrl) && !string.IsNullOrEmpty(captchaText))
                        {
                            postdata = "recaptcha_challenge_field=" + ImageUrl.Replace("https://www.google.com/recaptcha/api/image?c=", string.Empty) + "&recaptcha_response_field=" + captchaText.Replace(" ", "+") + "&irhf=" + irhf + "&dts=" + dts + "&security-challenge-id=" + captchachallengeid + "&submissionId=" + submissionID + "&CAPTCHA-fwd-csrfToken=" + CAPTCHAfwdcsrftoken + "&CAPTCHA-fwd-isJsEnabled=" + CAPTCHAfwdisJsEnabled + "&CAPTCHA-fwd-signin=" + CAPTCHAfwdsignin + "&CAPTCHA-fwd-loginCsrfParam=" + CAPTCHAfwdloginCsrfParam + "&CAPTCHA-fwd-session_password=" + CAPTCHAfwdsession_password + "&CAPTCHAfwd-session_key=" + CAPTCHAfwdsession_key + "&session_password=" + accountPass + "&session_key=" + Uri.EscapeDataString(accountUser) + "&origSourceAlias=" + origSourceAlias + "&origActionAlias=" + origActionAlias + "&csrfToken=" + csrfToken + "&sourceAlias=" + sourceAlias;
                            postdata = postdata.Replace(" ", string.Empty);
                            ResLogin = HttpHelper.postFormDataRef(new Uri("https://www.linkedin.com/uas/captcha-submit"), postdata, "https://www.linkedin.com/uas/login-submit", string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                        }
                        else
                        {
                            ResLogin = string.Empty;
                        }

                        if (ResLogin.Contains("The text you entered does not match the characters in the security image. Please try again with this new image") || string.IsNullOrEmpty(ResLogin))
                        {
                            Log("[ " + DateTime.Now + " ] => [ " + accountUser + "  Cannot Login because of Capctcha ]");
                            GlobusFileHelper.WriteStringToTextfile(accountUser + ":" + accountPass + ":" + proxyAddress + ":" + proxyPort + ":" + proxyUserName + ":" + proxyPassword, Globals.pathCapcthaLogin);
                            SearchCriteria.loginREsponce = string.Empty;
                            //FrmScrapGroupMember.ChangeToNextAccount = true;
                        }
                    }
                }

                string CheckedAccount = accountUser + ":" + accountPass;

                if (ResLogin.Contains("Sign Out") || ResLogin.Contains("class=\"signout\"") || ResLogin.Contains("Cerrar sesión"))
                {
                    IsLoggedIn = true;
                    Log("[ " + DateTime.Now + " ] => [ Logged In With Account : " + accountUser + " ]");
                    GlobusFileHelper.AppendStringToTextfileNewLine(CheckedAccount, Globals.Path_WorkingAccount_AccountChecker);
                }
                else if (ResLogin.Contains("logout?session_full_logout"))
                {
                    SearchCriteria.loginREsponce = string.Empty;
                    IsLoggedIn = true;
                    string Search = HttpHelper.getHtmlfromUrl(new Uri("https://www.linkedin.com/search?trk=advsrch"));
                    SearchCriteria.loginREsponce = Search;
                }
                else if (ResLogin.Contains("Sign-In Verification"))
                {
                    Log("[ " + DateTime.Now + " ] => [ Verification required : " + accountUser + " ]");
                }
                else if (ResLogin.Contains("Your LinkedIn account has been temporarily restricted"))
                {
                    Log("[ " + DateTime.Now + " ] => [ Your LinkedIn account : " + accountUser + " has been temporarily restricted ]");
                    //FrmScrapGroupMember.ChangeToNextAccount = true;
                }
                else
                {
                    Log("[ " + DateTime.Now + " ] => [ NotLogged In With Account : " + accountUser + " ]");
                    //FrmScrapGroupMember.ChangeToNextAccount = true;
                    GlobusFileHelper.AppendStringToTextfileNewLine(CheckedAccount, Globals.Path_NonWorkingAccount_AccountChecker);
                }

            }
            catch (Exception ex)
            {
            }
        }

        #endregion

    }
}
