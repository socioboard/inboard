using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseLib;
using System.Text.RegularExpressions;


namespace AccountInfo
{
    public  class LinkedinLoginAndLogout
    {
        #region Variable Declaration
        string _Data = string.Empty;
        ChilkatHttpHelpr HttpChilkat = new ChilkatHttpHelpr();
        GlobusHttpHelper HttpHelper = new GlobusHttpHelper();

        public Events LoginEvent = new Events();
        public Events WallPosterLoginEvent = new Events();
       
        string _AccountUser = string.Empty;
        string _AccountPass = string.Empty;
        string _ProxyAddress = string.Empty;
        string _ProxyPort = string.Empty;
        string _ProxyUserName = string.Empty;
        string _ProxyPassword = string.Empty;

        #endregion
        public void Login()
        {
            string str = string.Empty;
            string accountUser = string.Empty;
            string accountPass = string.Empty;
            string proxyAddress = string.Empty;
            string proxyPort = string.Empty;
            string proxyUserName = string.Empty;
            string proxyPassword = string.Empty;
            string Url = string.Empty;
            Url = "https://www.linkedin.com/";
            string pageSrcLogin = HttpChilkat.GetHtmlProxy(Url, proxyAddress, proxyPort, proxyUserName, proxyPassword);

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

            accountUser = "riteshsatthya@globussoft.com";
            accountPass = "globussoft";
            postUrl = "https://www.linkedin.com/uas/login-submit";
            postdata = "session_key=" + accountUser + "&session_password=" + accountPass + "&source_app=&trk=guest_home_login&session_redirect=&csrfToken=" + csrfToken + "&sourceAlias=" + sourceAlias;
            ResLogin = HttpChilkat.PostFormData(Url, postdata, "");

            Url = "http://www.linkedin.com/home?trk=hb_tab_home_top";
            pageSrcLogin = HttpChilkat.GetHtmlProxy(Url, proxyAddress, proxyPort, proxyUserName, proxyPassword);

        }

        public void LoginHttpHelper()
        {
            string str = string.Empty;
            string accountUser = string.Empty;
            string accountPass = string.Empty;
            string proxyAddress = string.Empty;
            string proxyPort = string.Empty;
            string proxyUserName = string.Empty;
            string proxyPassword = string.Empty;
            string Url = string.Empty;
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
            accountUser = "riteshsatthya@globussoft.com";
            accountUser = accountUser.Replace("@", "%40");
            accountPass = "globussoft";
            postUrl = "https://www.linkedin.com/uas/login-submit";
            postdata = "session_key=" + accountUser + "&session_password=" + accountPass + "&source_app=&trk=guest_home_login&session_redirect=&csrfToken=" + csrfToken + "&sourceAlias=" + sourceAlias;

            ResLogin = HttpHelper.postFormData(new Uri(postUrl), postdata);

            Url = "http://www.linkedin.com/home?trk=hb_tab_home_top";
            pageSrcLogin = HttpHelper.getHtmlfromUrlProxy(new Uri(Url), proxyAddress, int.Parse(proxyPort), proxyUserName, proxyPassword);

            LogoutHttpHelper(ref HttpHelper);

            Url = "http://www.linkedin.com/home?trk=hb_tab_home_top";
            pageSrcLogin = HttpHelper.getHtmlfromUrlProxy(new Uri(Url), proxyAddress, int.Parse(proxyPort), proxyUserName, proxyPassword);
        }

        public bool LoginHttpHelper(string username, string password, string proxyaddress, string proxyport, string proxyusername, string proxypassword, ref GlobusHttpHelper HttpHelper, ref string Message)
        {
            bool isLoggin = false;

            try
            {
                // LoggerLoginLogout("Logging Block.............");
                string Url = string.Empty;
                int port = 888;
                Regex PortCheck = new Regex("^[0-9]*$");

                _AccountUser = username;
                _AccountPass = password;
                _ProxyAddress = proxyaddress;
                _ProxyPort = proxyport;
                _ProxyUserName = proxyusername;
                _ProxyPassword = proxypassword;

                if (PortCheck.IsMatch(_ProxyPort) && !string.IsNullOrEmpty(_ProxyPort))
                {
                    port = int.Parse(_ProxyPort);
                }

                if (string.IsNullOrEmpty(_ProxyPort))
                {
                    _ProxyPort = "0";
                }

                Url = "https://www.linkedin.com/";
                // string pageSrcLogin = HttpChilkat.GetHtmlProxy(Url, proxyAddress, proxyPort, proxyUserName, proxyPassword);
                string pageSrcLogin = string.Empty;
               
                try
                {
                    pageSrcLogin = HttpHelper.getHtmlfromUrlProxy(new Uri(Url), _ProxyAddress, (port), _ProxyUserName, _ProxyPassword);//HttpHelper.getHtmlfromUrl(new Uri(Url));
                }
                catch { }


                string postdata = string.Empty;
                string postUrl = string.Empty;
                string ResLogin = string.Empty;
                string csrfToken = string.Empty;
                string sourceAlias = string.Empty;
                string regCsrfParam = string.Empty;

                #region commented
                //if (pageSrcLogin.Contains("csrfToken"))
                //{
                //    try
                //    {
                //        csrfToken = pageSrcLogin.Substring(pageSrcLogin.IndexOf("csrfToken"), 100);
                //        string[] Arr = csrfToken.Split('"');
                //        csrfToken = Arr[2].Replace("\\",string.Empty).Replace("\"",string.Empty).Trim();
                //    }
                //    catch
                //    {
                //        try
                //        {
                //            csrfToken = pageSrcLogin.Substring(pageSrcLogin.IndexOf("csrfToken"), 100);
                //            string[] Arr = csrfToken.Split('&');
                //            csrfToken = Arr[0].Replace("\\", string.Empty).Replace("\"", string.Empty).Replace("csrfToken=", "").Replace(">",string.Empty).Trim();
                //        }
                //        catch
                //        {
                //            try
                //            {
                //                csrfToken = pageSrcLogin.Substring(pageSrcLogin.IndexOf("csrfToken"), 100);
                //                string[] Arr = csrfToken.Split(',');
                //                csrfToken = Arr[0].Replace("\\", string.Empty).Replace("\"", string.Empty).Replace("csrfToken=", "").Replace(">", string.Empty).Trim();
                //            }
                //            catch { }
                //        }

                //    }
                //}

                //if (csrfToken.Contains(">") && csrfToken.Contains("csrfToken="))
                //{
                //    try
                //    {
                //        csrfToken = csrfToken.Substring(csrfToken.IndexOf("ajax"), csrfToken.IndexOf(">") - csrfToken.IndexOf("ajax")).Replace("csrfToken=", "").Trim();

                //    }
                //    catch { }

                //}

                //if (pageSrcLogin.Contains("sourceAlias"))
                //{
                //    try
                //    {

                //        sourceAlias = pageSrcLogin.Substring(pageSrcLogin.IndexOf("sourceAlias"), 100);
                //        string[] Arr = sourceAlias.Split('"');
                //        sourceAlias = Arr[2].Replace("\\", string.Empty).Replace("\"", string.Empty).Trim();
                //    }
                //    catch { }
                //}
                ////_AccountUser = "riteshsatthya@globussoft.com";
                ////_AccountPass = "globussoft";


                ////_AccountUser = "gargimishra@globussoft.com";

                // _AccountUser = Uri.EscapeDataString(_AccountUser);
                //postUrl = "https://www.linkedin.com/uas/login-submit";
                ////postdata = "session_key=" + _AccountUser + "&session_password=" + _AccountPass + "&source_app=&trk=guest_home_login&session_redirect=&csrfToken=" + csrfToken + "&sourceAlias=" + sourceAlias;
                //postdata = "session_key=" + _AccountUser + "&session_password=" + _AccountPass + "&source_app=&trk=guest_home_login&session_redirect=&csrfToken=" + csrfToken + "&sourceAlias=" + sourceAlias;
                
                #endregion

                if (pageSrcLogin.Contains("csrfToken"))
                {
                    try
                    {
                        int startIndex = pageSrcLogin.IndexOf("name=\"csrfToken\"");
                        string start = pageSrcLogin.Substring(startIndex).Replace("name=\"csrfToken\"", "");
                        int endIndex = start.IndexOf("\" ");
                        string end = start.Substring(0, endIndex).Replace("value=\"", "").Trim();
                        csrfToken = end;
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
                postdata = "isJsEnabled=true&source_app=&tryCount=&session_key=" + Uri.EscapeDataString(_AccountUser) + "&session_password=" + _AccountPass + "&signin=Sign%20In&session_redirect=&loginCsrfParam=" + regCsrfParam + "&csrfToken=" + csrfToken + "&sourceAlias=" + sourceAlias;
               
                try
                {
                    ResLogin = HttpHelper.postFormDataProxy(new Uri(postUrl), postdata, _ProxyAddress, int.Parse(_ProxyPort), _ProxyUserName, _ProxyPassword);//HttpHelper.postFormDataRef(new Uri(postUrl), postdata, "http://www.linkedin.com/uas/login?goback=&trk=hb_signin", "", "");

                }
                catch { }
                

                if (ResLogin.Contains("The email address or password you provided does not match our records"))
                {
                    GlobusFileHelper.WriteStringToTextfile(_AccountUser + ":" + _AccountPass + ":" + _ProxyAddress + ":" + _ProxyPort + ":" + _ProxyUserName + ":" + _ProxyPassword, Globals.path_FailLogin);
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
                else if (ResLogin.Contains("Sign Out"))
                {
                    Message = "Logged in";
                    return true;
                }
                else if (ResLogin.Contains("This login attempt seems suspicious") || ResLogin.Contains("Please enter the verification code sent to the email"))
                {
                    GlobusFileHelper.WriteStringToTextfile(_AccountUser + ":" + _AccountPass + ":" + _ProxyAddress + ":" + _ProxyPort + ":" + _ProxyUserName + ":" + _ProxyPassword, Globals.path_FailLogin);
                    Message = "This login attempt seems suspicious.Please enter the verification code sent to the email.";
                    return false;
                }
                else
                {
                    Message = "Couldn't Login";
                    GlobusFileHelper.WriteStringToTextfile(_AccountUser + ":" + _AccountPass + ":" + _ProxyAddress + ":" + _ProxyPort + ":" + _ProxyUserName + ":" + _ProxyPassword, Globals.path_FailLogin);
                    return false;
                }
                //Url = "http://www.linkedin.com/home?trk=hb_tab_home_top";
                //pageSrcLogin = HttpHelper.getHtmlfromUrlProxy(new Uri(Url), _ProxyPassword, port, _ProxyUserName, _ProxyPassword);

                //LogoutHttpHelper(ref HttpHelper);

                //Url = "http://www.linkedin.com/home?trk=hb_tab_home_top";
                //pageSrcLogin = HttpHelper.getHtmlfromUrlProxy(new Uri(Url), _ProxyPassword, 888, _ProxyUserName, _ProxyPassword);
                //return true;
            }
            catch
            {
            }
            return isLoggin;
        }

        //public bool LoginHttpHelper(string username, string password, string proxyaddress, string proxyport, string proxyusername, string proxypassword, ref GlobusHttpHelper HttpHelper, ref string Message)
        //{
        //    LoggerLoginLogout("Logging Block.............");
        //    string Url = string.Empty;
        //    int port = 888;
        //    Regex PortCheck = new Regex("^[0-9]*$");

        //    _AccountUser = username;
        //    _AccountPass = password;
        //    _ProxyAddress = proxyaddress;
        //    _ProxyPort = proxyport;
        //    _ProxyUserName = proxyusername;
        //    _ProxyPassword = proxypassword;

        //    if (PortCheck.IsMatch(_ProxyPort) && !string.IsNullOrEmpty(_ProxyPort))
        //    {
        //        port = int.Parse(_ProxyPort);
        //    }
        //    Url = "https://www.linkedin.com/";
        //    ////string pageSrcLogin = HttpChilkat.GetHtmlProxy(Url, proxyAddress, proxyPort, proxyUserName, proxyPassword);
        //    string pageSrcLogin = HttpHelper.getHtmlfromUrlProxy(new Uri(Url), _ProxyAddress, port, _ProxyUserName, _ProxyPassword);


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
        //    //_AccountUser = "riteshsatthya@globussoft.com";
        //    //_AccountPass = "globussoft";

        //    _AccountUser = _AccountUser.Replace("@", "%40");
        //    postUrl = "https://www.linkedin.com/uas/login-submit";
        //    postdata = "session_key=" + _AccountUser + "&session_password=" + _AccountPass + "&source_app=&trk=guest_home_login&session_redirect=&csrfToken=" + csrfToken + "&sourceAlias=" + sourceAlias;

        //    ResLogin = HttpHelper.postFormData(new Uri(postUrl), postdata);
        //    if (ResLogin.Contains("The email address or password you provided does not match our records"))
        //    {
        //        //Message = "The email address or password does not match our records";
        //        //return false;
        //    }
        //    else if (ResLogin.Contains("Sign Out") && ResLogin.Contains("Profiles"))
        //    {
        //        Message = "Logged in";
        //        return true;
        //    }
        //    else if (ResLogin.Contains("Your LinkedIn account has been temporarily restricted"))
        //    {
        //        Message = "Your LinkedIn account has been temporarily restricted";
        //        return false;
        //    }

        //    Url = "http://www.linkedin.com/home?trk=hb_tab_home_top";
        //    pageSrcLogin = HttpHelper.getHtmlfromUrlProxy(new Uri(Url), _ProxyPassword, port, _ProxyUserName, _ProxyPassword);

        //    //LogoutHttpHelper(ref HttpHelper);

        //    //Url = "http://www.linkedin.com/home?trk=hb_tab_home_top";
        //    //pageSrcLogin = HttpHelper.getHtmlfromUrlProxy(new Uri(Url), _ProxyPassword, 888, _ProxyUserName, _ProxyPassword);
        //    return true;
        //}

        public void LogoutHttpHelper(ref GlobusHttpHelper HttpHelper)
        {
            string Url = string.Empty;
            Url = "http://www.linkedin.com/home?trk=hb_tab_home_top";
            ////string pageSrcLogin = HttpChilkat.GetHtmlProxy(Url, proxyAddress, proxyPort, proxyUserName, proxyPassword);
            string pageSrcHome = HttpHelper.getHtmlfromUrl1(new Uri(Url));

            Url = "https://www.linkedin.com/secure/login?session_full_logout=&trk=hb_signout";
            string pageSrcLogout = HttpHelper.getHtmlfromUrl1(new Uri(Url));
        }

        ////public void LoginHttpHelper(string username,string password,string proxyAdderss, string proxyport,string proxyusername, string proxypassword, ref GlobusHttpHelper HttpHelper)
        //{
        //    string _accountUser = string.Empty;
        //    string _accountPass = string.Empty;
        //    string _ProxyAddress = string.Empty;
        //    int _proxyPort = 888;
        //    string _proxyUserName = string.Empty;
        //    string _proxyPassword = string.Empty;
        //    string Url = string.Empty;

        //    _accountUser = username;
        //    _accountPass = password;
        //    _ProxyAddress = proxyAdderss;
        //    if (!string.IsNullOrEmpty(proxyport))
        //    {
        //        _proxyPort = int.Parse(proxyport);
        //    }
            
        //    _proxyUserName = proxyusername;
        //    _proxyPassword = proxypassword;
        //    Url = "https://www.linkedin.com/";

        //    ////string pageSrcLogin = HttpChilkat.GetHtmlProxy(Url, proxyAddress, proxyPort, proxyUserName, proxyPassword);
        //    string pageSrcLogin = HttpHelper.getHtmlfromUrlProxy(new Uri(Url), _ProxyAddress, 888, _proxyUserName, _proxyPassword);


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
        //    //accountUser = "riteshsatthya@globussoft.com";
        //    _accountUser = _accountUser.Replace("@", "%40");
        //    //accountPass = "globussoft";
        //    postUrl = "https://www.linkedin.com/uas/login-submit";
        //    postdata = "session_key=" + _accountUser + "&session_password=" + _accountPass + "&source_app=&trk=guest_home_login&session_redirect=&csrfToken=" + csrfToken + "&sourceAlias=" + sourceAlias;

        //    ResLogin = HttpHelper.postFormData(new Uri(postUrl), postdata);

        //    Url = "http://www.linkedin.com/home?trk=hb_tab_home_top";
        //    pageSrcLogin = HttpHelper.getHtmlfromUrlProxy(new Uri(Url), _ProxyAddress, 888, _proxyUserName, _proxyPassword);

        //    LogoutHttpHelper(ref HttpHelper);

        //    Url = "http://www.linkedin.com/home?trk=hb_tab_home_top";
        //    pageSrcLogin = HttpHelper.getHtmlfromUrlProxy(new Uri(Url), _ProxyAddress, 888, _proxyUserName, _proxyPassword);
        //}

        private void LoggerLoginLogout(string message)
        {
            EventsArgs eventArgs = new EventsArgs(message);
            LoginEvent.LogText(eventArgs);
            WallPosterLoginEvent.LogText(eventArgs);
        }

        
    }
}
