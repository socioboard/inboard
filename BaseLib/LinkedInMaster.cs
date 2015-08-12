using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using ManageConnections;
using System.Net;

namespace BaseLib
{
    public class LinkedInMaster
    {
        //** Account Related Variable Declaration *****************************************************************************
        #region Account Related Variable Declaration

        public bool IsLoggedIn = false;
        public string _Username = string.Empty;
        public string _Password = string.Empty;
        public string _ProxyAddress = string.Empty;
        public string _ProxyPort = string.Empty;
        public string _ProxyUsername = string.Empty;
        public string _ProxyPassword = string.Empty;
        public string _Postalcode = string.Empty;
        public string _LastName = string.Empty;
        public string _IndustryType = string.Empty;
        public string _Distance = string.Empty;
        #endregion
        public int profileStatus = 0;
        //** Manage Connection Related Variable Declaration *****************************************************************************
        

        //** Wall Poster Related Variable Declaration *****************************************************************************
        #region Wall Poster Related Variable Declaration

        public string _WallMessage = string.Empty;

        #endregion

        //** Wall Poster Related Variable Declaration *****************************************************************************
        #region Wall Poster Related Variable Declaration

        public string _SummaryGoals = string.Empty;
        public string _SummarySpecialties = string.Empty;

        #endregion


              

        #region LoginHttpHelper
        public void LoginHttpHelper(ref GlobusHttpHelper HttpHelper)
        {
            try
            {                
                Log("[ " + DateTime.Now + " ] => [ Logging In With : " + _Username + " ]");
                Log("[ " + DateTime.Now + " ] => [ Login Process is Running... ]");
                
                //Check Login
                if (IsLoggedIn)  
                {
                    try
                    {
                        string homePage = HttpHelper.getHtmlfromUrlProxy(new Uri("http://www.linkedin.com/home"), _ProxyAddress, int.Parse(_ProxyPort), _ProxyUsername, _ProxyPassword);

                        if (homePage.Contains("Sign Out") && homePage.Contains("class=\"signout\"") && !homePage.Contains("Your LinkedIn account has been temporarily restricted"))
                        {
                            Log("[ " + DateTime.Now + " ] => [ Already Logged In With : " + _Username + " ]");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.StackTrace);
                    }
                }

                
                string Url = "https://www.linkedin.com/";
                ////string pageSrcLogin = HttpChilkat.GetHtmlProxy(Url, proxyAddress, proxyPort, proxyUserName, proxyPassword);
                string pageSrcLogin = HttpHelper.getHtmlfromUrlProxy(new Uri(Url), _ProxyAddress, int.Parse(_ProxyPort), _ProxyUsername, _ProxyPassword);

                if (string.IsNullOrEmpty(pageSrcLogin))
                {
                    try
                    {
                        System.Threading.Thread.Sleep(1000);
                        pageSrcLogin = HttpHelper.getHtmlfromUrlProxy(new Uri(Url), _ProxyAddress, int.Parse(_ProxyPort), _ProxyUsername, _ProxyPassword, "");
                    }
                    catch
                    {
                    }
                }
                if (string.IsNullOrEmpty(pageSrcLogin))
                {
                    Log("[ " + DateTime.Now + " ] => [ Couldn't Login In With : " + _Username + " ]");
                    return;
                }

                string postdata = string.Empty;
                string postUrl = string.Empty;
                string ResLogin = string.Empty;
                string csrfToken = string.Empty;
                string sourceAlias = string.Empty;
                string regCsrfParam = string.Empty;

                if (pageSrcLogin.Contains("csrfToken"))
                {
                    try
                    {
                        int startindex = pageSrcLogin.IndexOf("name=\"csrfToken\"");
                        if (startindex > 0)
                        {
                            string start = pageSrcLogin.Substring(startindex).Replace("name=\"csrfToken\"", "");
                            int endindex = start.IndexOf("\" ");
                            string end = start.Substring(0, endindex);
                            csrfToken = end.Replace("value=\"", "").Replace("\\", "").Replace(" ", "");
                            //csrfToken = Uri.EscapeDataString(csrfToken);
                            if (csrfToken.Contains("https://www.linkedin.com"))
                            {
                                csrfToken = Utils.getBetween("@@@@@@@" + csrfToken, "@@@@@@@", "\"/></form>");
                            }
                        }
                        else
                        {
                            string[] Arr = csrfToken.Split('"');
                            csrfToken = Arr[2].Replace("\\", string.Empty);
                        }
                    }
                    catch
                    {
                        try
                        {
                            csrfToken = pageSrcLogin.Substring(pageSrcLogin.IndexOf("csrfToken"), 100);
                            if (csrfToken.Contains("&"))
                            {
                                string[] Arr = csrfToken.Split('&');
                                csrfToken = Arr[0];
                            }
                            else if (csrfToken.Contains(","))
                            {
                                string[] Arr = csrfToken.Split(',');
                                csrfToken = Arr[0].Replace("\\", string.Empty);
                            }
                            else
                            {
                                string[] Arr = csrfToken.Split(',');
                                csrfToken = Arr[0].Replace("\\", string.Empty).Replace("csrfToken=", "").Replace("\n", "").Replace("\">", "");
                            }

                        }
                        catch { }
                    }
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

                SearchCriteria.CsrToken = csrfToken.Replace("\n", "").Replace("//", "").Replace("\">", "").Replace("csrfToken=", "");


                if (pageSrcLogin.Contains("sourceAlias"))
                {
                    try
                    {
                        sourceAlias = pageSrcLogin.Substring(pageSrcLogin.IndexOf("sourceAlias"), 100);
                        string[] Arr = sourceAlias.Split('"');
                        sourceAlias = Arr[2].Replace("\\", string.Empty);
                    }
                    catch { }
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
                    if (string.IsNullOrEmpty(regCsrfParam))
                    {
                        regCsrfParam = Utils.getBetween(pageSrcLogin, "loginCsrfParam", "/>");
                        regCsrfParam = Utils.getBetween(regCsrfParam, "value=\"", "\"");
                    }

                }
                catch { }

                
                postUrl = "https://www.linkedin.com/uas/login-submit";
                //postdata = "isJsEnabled=true&source_app=&tryCount=&session_key=" + Uri.EscapeDataString(_Username) + "&session_password=" + _Password + "&signin=Sign%20In&session_redirect=&loginCsrfParam=" + regCsrfParam + "&csrfToken=" + csrfToken + "&sourceAlias=" + sourceAlias;

                postdata = "isJsEnabled=true&source_app=&tryCount=&clickedSuggestion=false&session_key=" + Uri.EscapeDataString(_Username) + "&session_password=" + _Password + "&signin=Sign%20In&session_redirect=&trk=hb_signin&loginCsrfParam=" + regCsrfParam + "&fromEmail=&csrfToken=" + csrfToken + "&sourceAlias=" + sourceAlias;
               // postdata = "isJsEnabled=true&source_app=&session_key=" + Uri.EscapeDataString(_Username) + "&session_password=" + _Password + "&signin=Sign+In&session_redirect=&trk=&loginCsrfParam=7462d247-6d54-47de-8f22-29723ea15f9d&csrfToken=" + csrfToken + "&sourceAlias=" + sourceAlias;
                try
                {
                    ResLogin = HttpHelper.postFormDataProxy(new Uri(postUrl), postdata,_ProxyAddress,int.Parse(_ProxyPort),_ProxyUsername,_ProxyPassword);//HttpHelper.postFormDataRef(new Uri(postUrl), postdata, "http://www.linkedin.com/uas/login?goback=&trk=hb_signin", "", "");
                }
                catch { }


                //postUrl = "https://www.linkedin.com/uas/login-submit";
                //isJsEnabled=true&source_app=&tryCount=&session_key=gargimishra%40globussoft.com&session_password=globussoft&signin=Sign%20In&session_redirect=&csrfToken=ajax%3A7066152446927176852&sourceAlias=0_7r5yezRXCiA_H0CRD8sf6DhOjTKUNps5xGTqeX8EEoi
               //postdata = "isJsEnabled=true&source_app=&session_key=" + Uri.EscapeDataString(_Username) + "&session_password=" + Uri.EscapeDataString(_Password) + "&signin=Sign%20In&session_redirect=&csrfToken=" + csrfToken + "&sourceAlias=" + sourceAlias;
                // postdata = "session_key=" + Uri.EscapeDataString(_Username) + "&session_password=" + Uri.EscapeDataString(_Password) + "&source_app=&trk=guest_home_login&session_redirect=&csrfToken=" + csrfToken + "&sourceAlias=" + sourceAlias;
                
                //ResLogin = HttpHelper.postFormDataRef(new Uri(postUrl), postdata, "https://www.linkedin.com/uas/login?goback=&trk=hb_signin", "true", "");
                string dts = string.Empty;
                string captchachallengeid = string.Empty;
                string origActionAlias = string.Empty;
                string origSourceAlias = string.Empty;
                string irhf = string.Empty;
                string captchaText = string.Empty;
                string ImageUrl = string.Empty;
                //
                if (ResLogin.Contains("Your LinkedIn account has been temporarily restricted") || ResLogin.Contains("Change your password") || ResLogin.Contains("Please confirm your email address") && !ResLogin.Contains("Sign Out"))
                {
                    SearchCriteria.loginREsponce = ResLogin;
                }
                else if (ResLogin.Contains("Security Verification"))
                {
                    string dataforcapctha = HttpHelper.getHtmlfromUrl1(new Uri("https://www.google.com/recaptcha/api/noscript?k=6LcnacMSAAAAADoIuYvLUHSNLXdgUcq-jjqjBo5n"));
                    if (!string.IsNullOrEmpty(dataforcapctha))
                    {
                        int startindex = dataforcapctha.IndexOf("id=\"recaptcha_challenge_field\"");
                        if (startindex > 0)
                        {
                            string start = dataforcapctha.Substring(startindex).Replace("id=\"recaptcha_challenge_field\"", "");
                            int endindex = start.IndexOf("\">");
                            string end = start.Substring(0, endindex).Replace("value=", "").Replace("\"", "");
                            ImageUrl = "https://www.google.com/recaptcha/api/image?c=" + end;
                            WebClient webclient = new WebClient();
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

                        //postdata = "dts=" + dts + "&security-challenge-id=" + captchachallengeid + "&hr=&source_app=&csrfToken=" + csrfToken + "&trk=guest_home&isJsEnabled=true&session_redirect=&session_password=" + _Password + "&session_key=" + _Username + "&origSourceAlias=" + origSourceAlias + "&origActionAlias=" + origActionAlias + "&irhf=" + irhf +"&sourceAlias=" + sourceAlias;
                        if (!string.IsNullOrEmpty(ImageUrl) && !string.IsNullOrEmpty(captchaText))
                        {
                            postdata = "recaptcha_challenge_field=" + ImageUrl.Replace("https://www.google.com/recaptcha/api/image?c=", "") + "&recaptcha_response_field=" + captchaText.Replace(" ", "+") + "&dts=" + dts + "&security-challenge-id=" + captchachallengeid + "&hr=&source_app=&csrfToken=" + csrfToken + "&isJsEnabled=true&session_redirect=&session_password=" + _Password + "&session_key=" + Uri.EscapeDataString(_Username) + "&origSourceAlias=" + origSourceAlias + "&origActionAlias=" + origActionAlias + "&irhf=" + irhf + "&sourceAlias=" + sourceAlias;
                            postdata = postdata.Replace(" ", "");
                            ResLogin = HttpHelper.postFormDataRef(new Uri("https://www.linkedin.com/uas/captcha-submit"), postdata, "https://www.linkedin.com/uas/login-submit", "", "", "", "", "");
                        }
                        else
                        {
                            ResLogin = "";
                        }

                        if (ResLogin.Contains("The text you entered does not match the characters in the security image. Please try again with this new image") || string.IsNullOrEmpty(ResLogin))
                        {
                            Log("[ " + DateTime.Now + " ] => [ " + _Username + " Cannot Login because of Capctcha ]");
                            GlobusFileHelper.WriteStringToTextfile(_Username + ":" + _Password + ":" + _ProxyAddress + ":" + _ProxyPort + ":" + _ProxyUsername + ":" + _ProxyPassword, Globals.pathCapcthaLogin);
                            SearchCriteria.loginREsponce = string.Empty;
                        }
                    }
                }

                //ResLogin.Contains("Sign Out") && ResLogin.Contains("class=\"signout\"") && !ResLogin.Contains("Your LinkedIn account has been temporarily restricted")
                if (ResLogin.Contains("Sign Out") && !ResLogin.Contains("Your LinkedIn account has been temporarily restricted"))
                {
                    SearchCriteria.loginREsponce = string.Empty;
                    IsLoggedIn = true;
                    string Search = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/search?trk=advsrch"));
                    SearchCriteria.loginREsponce = Search;
                }
                else if (ResLogin.Contains("logout?session_full_logout"))
                {
                    SearchCriteria.loginREsponce = string.Empty;
                    IsLoggedIn = true;
                    string Search = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/search?trk=advsrch"));
                    SearchCriteria.loginREsponce = Search;
                }
                else
                {
                    //There was an unexpected problem that prevented us from completing your request.
                    SearchCriteria.loginREsponce = string.Empty;
                    IsLoggedIn = false;
                }


                //Url = "http://www.linkedin.com/home?trk=hb_tab_home_top";
                //pageSrcLogin = HttpHelper.getHtmlfromUrlProxy(new Uri(Url), proxyAddress, 888, proxyUserName, proxyPassword);

                //LogoutHttpHelper(ref HttpHelper);

                //Url = "http://www.linkedin.com/home?trk=hb_tab_home_top";
                //pageSrcLogin = HttpHelper.getHtmlfromUrlProxy(new Uri(Url), proxyAddress, 888, proxyUserName, proxyPassword);
            }
            catch { }
        } 
        #endregion

        #region LoginHttpHelperForCampaignScraper
        public void LoginHttpHelperForCampaignScraper(ref GlobusHttpHelper HttpHelper, string Account, string password, string ProxyAddress, string ProxyPort, string ProxyUserName, string ProxyPassword)
        {
            try
            {
                Log("[ " + DateTime.Now + " ] => [ Logging In With : " + Account + " ]");
                Log("[ " + DateTime.Now + " ] => [ Login Process is Running... ]");

                //Check Login
                if (IsLoggedIn)
                {
                    try
                    {
                        string homePage = HttpHelper.getHtmlfromUrlProxy(new Uri("http://www.linkedin.com/home"), ProxyAddress, int.Parse(ProxyPort), ProxyUserName, ProxyPassword);

                        if (homePage.Contains("Sign Out") && homePage.Contains("class=\"signout\"") && !homePage.Contains("Your LinkedIn account has been temporarily restricted"))
                        {
                            Log("[ " + DateTime.Now + " ] => [ Already Logged In With : " + Account + " ]");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.StackTrace);
                    }
                }


                string Url = "https://www.linkedin.com/";
                ////string pageSrcLogin = HttpChilkat.GetHtmlProxy(Url, proxyAddress, proxyPort, proxyUserName, proxyPassword);
                string pageSrcLogin = HttpHelper.getHtmlfromUrlProxy(new Uri(Url), ProxyAddress, int.Parse(ProxyPort), ProxyUserName, ProxyPassword);

                if (string.IsNullOrEmpty(pageSrcLogin))
                {
                    try
                    {
                        System.Threading.Thread.Sleep(1000);
                        pageSrcLogin = HttpHelper.getHtmlfromUrlProxy(new Uri(Url), ProxyAddress, int.Parse(ProxyPort), ProxyUserName, ProxyPassword, "");
                    }
                    catch
                    {
                    }
                }
                if (string.IsNullOrEmpty(pageSrcLogin))
                {
                    Log("[ " + DateTime.Now + " ] => [ Couldn't Login In With : " + Account + " ]");
                    return;
                }

                string postdata = string.Empty;
                string postUrl = string.Empty;
                string ResLogin = string.Empty;
                string csrfToken = string.Empty;
                string sourceAlias = string.Empty;
                string regCsrfParam = string.Empty;

                if (pageSrcLogin.Contains("csrfToken"))
                {
                    try
                    {
                        int startindex = pageSrcLogin.IndexOf("name=\"csrfToken\"");
                        if (startindex > 0)
                        {
                            string start = pageSrcLogin.Substring(startindex).Replace("name=\"csrfToken\"", "");
                            int endindex = start.IndexOf("\" ");
                            string end = start.Substring(0, endindex);
                            csrfToken = end.Replace("value=\"", "").Replace("\\", "").Replace(" ", "");
                            csrfToken = Uri.EscapeDataString(csrfToken);
                        }
                        else
                        {
                            string[] Arr = csrfToken.Split('"');
                            csrfToken = Arr[2].Replace("\\", string.Empty);
                        }
                    }
                    catch
                    {
                        try
                        {
                            csrfToken = pageSrcLogin.Substring(pageSrcLogin.IndexOf("csrfToken"), 100);
                            if (csrfToken.Contains("&"))
                            {
                                string[] Arr = csrfToken.Split('&');
                                csrfToken = Arr[0];
                            }
                            else if (csrfToken.Contains(","))
                            {
                                string[] Arr = csrfToken.Split(',');
                                csrfToken = Arr[0].Replace("\\", string.Empty);
                            }
                            else
                            {
                                string[] Arr = csrfToken.Split(',');
                                csrfToken = Arr[0].Replace("\\", string.Empty).Replace("csrfToken=", "").Replace("\n", "").Replace("\">", "");
                            }

                        }
                        catch { }
                    }
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

                SearchCriteria.CsrToken = csrfToken.Replace("\n", "").Replace("//", "").Replace("\">", "").Replace("csrfToken=", "");


                if (pageSrcLogin.Contains("sourceAlias"))
                {
                    try
                    {
                        sourceAlias = pageSrcLogin.Substring(pageSrcLogin.IndexOf("sourceAlias"), 100);
                        string[] Arr = sourceAlias.Split('"');
                        sourceAlias = Arr[2].Replace("\\", string.Empty);
                    }
                    catch { }
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
                postdata = "isJsEnabled=true&source_app=&tryCount=&session_key=" + Uri.EscapeDataString(Account) + "&session_password=" + password + "&signin=Sign%20In&session_redirect=&loginCsrfParam=" + regCsrfParam + "&csrfToken=" + csrfToken + "&sourceAlias=" + sourceAlias;
                // postdata = "isJsEnabled=true&source_app=&session_key=" + Uri.EscapeDataString(_Username) + "&session_password=" + _Password + "&signin=Sign+In&session_redirect=&trk=&loginCsrfParam=7462d247-6d54-47de-8f22-29723ea15f9d&csrfToken=" + csrfToken + "&sourceAlias=" + sourceAlias;
                try
                {
                    lock (this)
                    {
                        ResLogin = HttpHelper.postFormDataProxy(new Uri(postUrl), postdata, ProxyAddress, int.Parse(ProxyPort), ProxyUserName, ProxyPassword);//HttpHelper.postFormDataRef(new Uri(postUrl), postdata, "http://www.linkedin.com/uas/login?goback=&trk=hb_signin", "", ""); 
                    }
                }
                catch { }


                //postUrl = "https://www.linkedin.com/uas/login-submit";
                //isJsEnabled=true&source_app=&tryCount=&session_key=gargimishra%40globussoft.com&session_password=globussoft&signin=Sign%20In&session_redirect=&csrfToken=ajax%3A7066152446927176852&sourceAlias=0_7r5yezRXCiA_H0CRD8sf6DhOjTKUNps5xGTqeX8EEoi
                //postdata = "isJsEnabled=true&source_app=&session_key=" + Uri.EscapeDataString(_Username) + "&session_password=" + Uri.EscapeDataString(_Password) + "&signin=Sign%20In&session_redirect=&csrfToken=" + csrfToken + "&sourceAlias=" + sourceAlias;
                // postdata = "session_key=" + Uri.EscapeDataString(_Username) + "&session_password=" + Uri.EscapeDataString(_Password) + "&source_app=&trk=guest_home_login&session_redirect=&csrfToken=" + csrfToken + "&sourceAlias=" + sourceAlias;

                //ResLogin = HttpHelper.postFormDataRef(new Uri(postUrl), postdata, "https://www.linkedin.com/uas/login?goback=&trk=hb_signin", "true", "");
                string dts = string.Empty;
                string captchachallengeid = string.Empty;
                string origActionAlias = string.Empty;
                string origSourceAlias = string.Empty;
                string irhf = string.Empty;
                string captchaText = string.Empty;
                string ImageUrl = string.Empty;
                //
                if (ResLogin.Contains("Your LinkedIn account has been temporarily restricted") || ResLogin.Contains("Change your password") || ResLogin.Contains("Please confirm your email address") && !ResLogin.Contains("Sign Out"))
                {
                    SearchCriteria.loginREsponce = ResLogin;
                }
                else if (ResLogin.Contains("Security Verification"))
                {
                    string dataforcapctha = HttpHelper.getHtmlfromUrl1(new Uri("https://www.google.com/recaptcha/api/noscript?k=6LcnacMSAAAAADoIuYvLUHSNLXdgUcq-jjqjBo5n"));
                    if (!string.IsNullOrEmpty(dataforcapctha))
                    {
                        int startindex = dataforcapctha.IndexOf("id=\"recaptcha_challenge_field\"");
                        if (startindex > 0)
                        {
                            string start = dataforcapctha.Substring(startindex).Replace("id=\"recaptcha_challenge_field\"", "");
                            int endindex = start.IndexOf("\">");
                            string end = start.Substring(0, endindex).Replace("value=", "").Replace("\"", "");
                            ImageUrl = "https://www.google.com/recaptcha/api/image?c=" + end;
                            WebClient webclient = new WebClient();
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

                        //postdata = "dts=" + dts + "&security-challenge-id=" + captchachallengeid + "&hr=&source_app=&csrfToken=" + csrfToken + "&trk=guest_home&isJsEnabled=true&session_redirect=&session_password=" + _Password + "&session_key=" + _Username + "&origSourceAlias=" + origSourceAlias + "&origActionAlias=" + origActionAlias + "&irhf=" + irhf +"&sourceAlias=" + sourceAlias;
                        if (!string.IsNullOrEmpty(ImageUrl) && !string.IsNullOrEmpty(captchaText))
                        {
                            postdata = "recaptcha_challenge_field=" + ImageUrl.Replace("https://www.google.com/recaptcha/api/image?c=", "") + "&recaptcha_response_field=" + captchaText.Replace(" ", "+") + "&dts=" + dts + "&security-challenge-id=" + captchachallengeid + "&hr=&source_app=&csrfToken=" + csrfToken + "&isJsEnabled=true&session_redirect=&session_password=" + _Password + "&session_key=" + Uri.EscapeDataString(_Username) + "&origSourceAlias=" + origSourceAlias + "&origActionAlias=" + origActionAlias + "&irhf=" + irhf + "&sourceAlias=" + sourceAlias;
                            postdata = postdata.Replace(" ", "");
                            ResLogin = HttpHelper.postFormDataRef(new Uri("https://www.linkedin.com/uas/captcha-submit"), postdata, "https://www.linkedin.com/uas/login-submit", "", "", "", "", "");
                        }
                        else
                        {
                            ResLogin = "";
                        }

                        if (ResLogin.Contains("The text you entered does not match the characters in the security image. Please try again with this new image") || string.IsNullOrEmpty(ResLogin))
                        {
                            Log("[ " + DateTime.Now + " ] => [ " + Account + " Cannot Login because of Capctcha ]");
                            GlobusFileHelper.WriteStringToTextfile(Account + ":" + password + ":" + ProxyAddress + ":" + ProxyPort + ":" + ProxyUserName + ":" + ProxyPassword, Globals.pathCapcthaLogin);
                            SearchCriteria.loginREsponce = string.Empty;
                        }
                    }
                }

                //ResLogin.Contains("Sign Out") && ResLogin.Contains("class=\"signout\"") && !ResLogin.Contains("Your LinkedIn account has been temporarily restricted")
                if (ResLogin.Contains("Sign Out") && !ResLogin.Contains("Your LinkedIn account has been temporarily restricted"))
                {
                    SearchCriteria.loginREsponce = string.Empty;
                    IsLoggedIn = true;
                    string Search = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/search?trk=advsrch"));
                    SearchCriteria.loginREsponce = Search;
                }
                else if (ResLogin.Contains("logout?session_full_logout"))
                {
                    SearchCriteria.loginREsponce = string.Empty;
                    IsLoggedIn = true;
                    string Search = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/search?trk=advsrch"));
                    SearchCriteria.loginREsponce = Search;
                }
                else
                {
                    //There was an unexpected problem that prevented us from completing your request.
                    SearchCriteria.loginREsponce = string.Empty;
                    IsLoggedIn = false;
                }


                //Url = "http://www.linkedin.com/home?trk=hb_tab_home_top";
                //pageSrcLogin = HttpHelper.getHtmlfromUrlProxy(new Uri(Url), proxyAddress, 888, proxyUserName, proxyPassword);

                //LogoutHttpHelper(ref HttpHelper);

                //Url = "http://www.linkedin.com/home?trk=hb_tab_home_top";
                //pageSrcLogin = HttpHelper.getHtmlfromUrlProxy(new Uri(Url), proxyAddress, 888, proxyUserName, proxyPassword);
            }
            catch { }
        }
        #endregion

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

        #region Log
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

    }
}
