using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenPOP.POP3;
using Chilkat;
using System.Text.RegularExpressions;

namespace BaseLib
{
    public class ClsEmailActivator
    {
        #region Global declaration
        public string accountUser = string.Empty;
        public string accountPass = string.Empty;
        public string proxyAddress = string.Empty;
        public string proxyPort = string.Empty;
        public string proxyUserName = string.Empty;
        public string proxyPassword = string.Empty;

        private readonly POPClient popClient = new POPClient();
        string responce = string.Empty;
        public static Events loggerEvents = new Events(); 
        #endregion

        #region Log
        private void Log(string log)
        {
            EventsArgs args = new EventsArgs(log);
            loggerEvents.LogText(args);
        } 
        #endregion

        #region ClsEmailActivator
        public ClsEmailActivator()
        {
        } 
        #endregion

        #region ClsEmailActivator
        public ClsEmailActivator(string UserName, string Password, string ProxyAddress, string ProxyPort, string ProxyUserName, string ProxyPassword)
        {
            this.accountUser = UserName;
            this.accountPass = Password;
            this.proxyAddress = ProxyAddress;
            this.proxyPort = ProxyPort;
            this.proxyUserName = ProxyUserName;
            this.proxyPassword = ProxyPassword;
        } 
        #endregion

        #region EmailVerification
        public bool EmailVerification(string Email, string Password, ref GlobusHttpHelper globushttpHelper)
        {
            bool IsActivated = false;
            try
            {
                Log("[ " + DateTime.Now + " ] => [ Please Wait Account Verification Start... ]");

                System.Windows.Forms.Application.DoEvents();
                Chilkat.Http http = new Chilkat.Http();

                if (Email.Contains("@yahoo"))
                {
                    #region Yahoo Verification Steps

                    GlobusHttpHelper HttpHelper = new GlobusHttpHelper();
                    bool activate = false;
                    try
                    {
                        bool emaildata = false;
                        //Chilkat.Http http = new Chilkat.Http();
                        ///Chilkat Http Request to be used in Http Post...
                        Chilkat.HttpRequest req = new Chilkat.HttpRequest();
                        bool success;

                        // Any string unlocks the component for the 1st 30-days.
                        success = http.UnlockComponent("THEBACHttp_b3C9o9QvZQ06");
                        if (success != true)
                        {
                            Console.WriteLine(http.LastErrorText);
                            return false;
                        }

                        http.CookieDir = "memory";
                        http.SendCookies = true;
                        http.SaveCookies = true;

                        //http.ProxyDomain = "127.0.0.1";
                        //http.ProxyPort = 8888;
                        http.SetRequestHeader("Accept-Encoding", "gzip,deflate");
                        Chilkat.Imap iMap = new Imap();
                        string Username = Email;

                        iMap.UnlockComponent("THEBACIMAPMAILQ_OtWKOHoF1R0Q");
                        //iMap.
                        //iMap.HttpProxyHostname = "127.0.0.1";
                        //iMap.HttpProxyPort = 8888;

                        iMap.Port = 993;
                        iMap.Connect("imap.n.mail.yahoo.com");
                        iMap.Login(Email, Password);
                        iMap.SelectMailbox("Inbox");

                        // Get a message set containing all the message IDs
                        // in the selected mailbox.
                        Chilkat.MessageSet msgSet;
                        //msgSet = iMap.Search("FROM \"facebookmail.com\"", true);

                        msgSet = iMap.GetAllUids();
                    

                        if (msgSet.Count <= 0)
                        {
                            msgSet = iMap.GetAllUids();
                        }

                        // Fetch all the mail into a bundle object.
                        Chilkat.Email email = new Chilkat.Email();
                        //bundle = iMap.FetchBundle(msgSet);
                        string strEmail = string.Empty;
                        List<string> lstData = new List<string>();
                        if (msgSet != null)
                        {
                            for (int i = 0; i < msgSet.Count; i++)
                            {
                                try
                                {
                                    email = iMap.FetchSingle(msgSet.GetId(i), true);
                                    strEmail = email.Subject;
                                    string emailHtml = email.GetHtmlBody();
                                    lstData.Add(strEmail);

                                    string from = email.From.ToString().ToLower();

                                    if (from.Contains("@linkedin.com"))
                                    {
                                        foreach (string href in GetUrlsFromString(email.Body))
                                        {
                                            try
                                            {
                                                
                                                if (href.Contains("http://www.linkedin.com/e/csrf") || href.Contains("http://www.linkedin.com/e/ato") || href.Contains("http://www.linkedin.com/e/v2?e"))
                                                {
                                                    string EscapeEmail = Uri.EscapeDataString(Email).Replace(".", "%2E").Trim();
                                                    {
                                                        string ConfirmationResponse = globushttpHelper.getHtmlfromUrl1(new Uri(href));
                                                        IsActivated = true;
                                                        break;
                                                    }

                                                }

                                            }
                                            catch (Exception ex)
                                            {
                                                Console.WriteLine("6 :" + ex.StackTrace);
                                            }
                                        }
                                    }
                                    if (IsActivated)
                                    {
                                        Log("[ " + DateTime.Now + " ] => [ Account : " + Email + " verified ]");
                                        break;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Error >>> " + ex.StackTrace);
                                }
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("7 :" + ex.StackTrace);
                        Log("[ " + DateTime.Now + " ] => [ Email Verification Exception : " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + " with : " + Email + " ]");
                        Log("[ " + DateTime.Now + " ] => [ Please check your Login Id and Password ]");
                    }
                    return IsActivated;
                    #endregion
                }
                else
                {
                    string Host = string.Empty;
                    int Port = 0;
                    if (Email.Contains("@gmail"))
                    {
                        Host = "pop.gmail.com";
                        Port = 995;
                    }
                    else if (Email.Contains("@hotmail"))
                    {
                        Host = "pop3.live.com";
                        Port = 995;
                    }
                    else if (Email.Contains("@gmx"))
                    {
                        Host = "pop.gmx.com";
                        Port = 995;
                    }
                    if (!string.IsNullOrEmpty(Host))
                    {
                        try
                        {
                            if (popClient.Connected)
                                popClient.Disconnect();
                            popClient.Connect(Host, Port, true);
                            popClient.Authenticate(Email, Password);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.StackTrace);

                            //retry once
                            System.Threading.Thread.Sleep(1000);

                            if (popClient.Connected)
                                popClient.Disconnect();
                            popClient.Connect(Host, Port, true);
                            popClient.Authenticate(Email, Password);
                        }


                        //if (!)
                        //{

                        //}

                        int Count = popClient.GetMessageCount();

                        for (int i = Count; i >= 1; i--)
                        {
                            try
                            {
                                OpenPOP.MIME.Message Message = popClient.GetMessage(i);
                                string subject = string.Empty;
                                subject = Message.Headers.Subject;
                                string frowwm = Message.Headers.From.ToString();
                                bool GoIntoEmail = false;

                                if (string.IsNullOrEmpty(subject))
                                {
                                    string from = Message.Headers.From.ToString().ToLower();
                                    if (from.Contains("linkedin.com"))
                                    {
                                        GoIntoEmail = true;
                                    }
                                }
                                try
                                {
                                    if (frowwm.Contains("linkedin.com"))
                                    //if(GoIntoEmail)
                                    {
                                        string Messagebody = Message.MessageBody[0];

                                        foreach (string href in GetUrlsFromStringGmail(Messagebody))
                                        {
                                            try
                                            {
                                                if (href.Contains("http://www.linkedin.com/e/csrf") || href.Contains("http://www.linkedin.com/e/ato") || href.Contains("http://www.linkedin.com/e/v2?e"))
                                                {
                                                  string  href1 = href.Replace("amp;",string.Empty);
                                                    string EscapeEmail = Uri.EscapeDataString(Email).Replace(".", "%2E").Trim();
                                                    {
                                                        string ConfirmationResponse = globushttpHelper.getHtmlfromUrl1(new Uri(href1));
                                                        IsActivated = true;
                                                        break;
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                Console.WriteLine("5 :" + ex.StackTrace);
                                            };
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("10 :" + ex.StackTrace);
                                    Log("[ " + DateTime.Now + " ] => [ Email Verification Exception : " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + " with : " + Email + " ]");
                                }
                                if (IsActivated)
                                {

                                    break;
                                }
                            }
                            catch (Exception ex)
                            {
                                {
                                    Log("[ " + DateTime.Now + " ] => [ Email Verification Exception : " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + " with : " + Email + " ]");
                                    Log("[ " + DateTime.Now + " ] => [ Please check your Login Id and Password ]");
                                }
                            }
                        }
                    }
                }
                return IsActivated;
            }
            catch (Exception ex)
            {
                Console.WriteLine("4 :" + ex.StackTrace);
                return IsActivated;
            }
        } 
        #endregion

        #region GetUrlFromString
        public string GetUrlFromString(string HtmlData)
        {
            System.Windows.Forms.Application.DoEvents();
            string Url = string.Empty;

            string strData = HtmlData;
            string[] arr = Regex.Split(strData, "\r\n");

            Log("[ " + DateTime.Now + " ] => [ Fetching url for email verification ]");
            foreach (string strhref in arr)
            {
                if (!strhref.Contains("<!DOCTYPE"))
                {
                    if (strhref.Contains("please click here to delete the account"))
                    {

                        break;
                    }
                    else if (strhref.Contains("[WordPress] Activate") && strhref.Contains("wordpress.com"))
                    {
                        Url = strhref;
                    }
                }
            }

            return Url;
        } 
        #endregion

        #region ActivationProcess
        public bool ActivationProcess(string ActivationUrl)
        {
            bool IsActivated = false;

            try
            {
                Chilkat.Http http = new Chilkat.Http();

                ///Chilkat Http Request to be used in Http Post...
                Chilkat.HttpRequest req = new Chilkat.HttpRequest();
                bool success;

                // Any string unlocks the component for the 1st 30-days.
                success = http.UnlockComponent("THEBACHttp_b3C9o9QvZQ06");
                if (success != true)
                {
                    Console.WriteLine(http.LastErrorText);
                }
                http.CookieDir = "memory";
                http.SendCookies = true;
                http.SaveCookies = true;


                http.SetRequestHeader("Accept-Encoding", "gzip,deflate");


                //complete url with website and address
                string siteurl = "http://blog.com/wp-login.php";
                //siteurl = Website;

                //  Send the HTTP GET and return the content in a string.
                string html = null;
                //html = http.QuickGetStr(siteurl);
                string actvateUrl = ActivationUrl.Substring(ActivationUrl.IndexOf("http"));

                html = http.QuickGetStr(actvateUrl);

                if (responce.Contains("Your account is now active"))
                {
                    Log("[ " + DateTime.Now + " ] => [ Account activated ]");
                    IsActivated = true;
                }

                if (!string.IsNullOrEmpty(html))
                {
                    if (html.Contains("Your account is now active") || html.Contains("Back to"))
                    {
                        //                   Log("blog account verified");

                        return true;
                    }
                    else
                    {
                        //                  Log("blog account not verified");
                        return false;
                    }
                }
                else
                {

                }

            }
            catch (Exception ex)
            {

            }


            return IsActivated;

        } 
        #endregion

        #region Deleted
        public bool gmail(string Email, string Password)
        {

            GlobusHttpHelper HttpHelper = new GlobusHttpHelper();
            bool activate = false;

            Chilkat.Http http = new Chilkat.Http();



            ///Chilkat Http Request to be used in Http Post...
            Chilkat.HttpRequest req = new Chilkat.HttpRequest();
            bool success;

            // Any string unlocks the component for the 1st 30-days.
            success = http.UnlockComponent("THEBACHttp_b3C9o9QvZQ06");
            if (success != true)
            {
                Console.WriteLine(http.LastErrorText);
                return false;
            }
            http.CookieDir = "memory";
            http.SendCookies = true;
            http.SaveCookies = true;

            //http.ProxyDomain = "127.0.0.1";
            //http.ProxyPort = 8888;


            http.SetRequestHeader("Accept-Encoding", "gzip,deflate");
           

            if (Email.Contains("@gmail"))
            {
                if (popClient.Connected)
                    popClient.Disconnect();
                popClient.Connect("pop.gmail.com", int.Parse("995"), true);
                popClient.Authenticate(Email, Password);
                int Count = popClient.GetMessageCount();

                for (int i = Count; i >= 1; i--)
                {
                    OpenPOP.MIME.Message Message = popClient.GetMessage(i);

                    string subject = Message.Headers.Subject;

                    if (Message.Headers.Subject.Contains("[WordPress] Activate") && Message.Headers.Subject.Contains("wordpress.com"))
                    {
                        foreach (string href in GetUrlsFromStringGmail(Message.MessageBody[0]))
                        {
                            try
                            {
                                string staticUrl = string.Empty;

                                staticUrl = href;

                                responce = http.QuickGetStr(staticUrl);
                                if (responce.Contains("Your account is now active"))
                                {
                                    Log("[ " + DateTime.Now + " ] => [ Account activated ]");
                                    activate = true;
                                }
                                
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                }
            }
            return activate;
        }

        public bool gmailWithoutReference(string Email, string Password)
        {
            bool activate = false;
            try
            {
                Chilkat.Http http = new Chilkat.Http();
                //HttpHelper = new GlobusHttpHelper();

                if (Email.Contains("@gmail"))
                {
                    if (popClient.Connected)
                        popClient.Disconnect();
                    popClient.Connect("pop.gmail.com", int.Parse("995"), true);
                    popClient.Authenticate(Email, Password);
                    int Count = popClient.GetMessageCount();

                    for (int i = Count; i >= 1; i--)
                    {
                        OpenPOP.MIME.Message Message = popClient.GetMessage(i);

                        string subject = Message.Headers.Subject;

                        if (Message.Headers.Subject.Contains("[WordPress] Activate") && Message.Headers.Subject.Contains("wordpress.com"))
                        {
                            foreach (string href in GetUrlsFromStringGmail(Message.MessageBody[0]))
                            {
                                try
                                {
                                    string staticUrl = string.Empty;

                                    staticUrl = href;

                                    responce = http.QuickGetStr(staticUrl);
                                    if (responce.Contains("Your account is now active"))
                                    {
                                        Log("[ " + DateTime.Now + " ] => [ Account activated ]");
                                        activate = true;
                                    }

                                }
                                catch (Exception ex)
                                {

                                }
                            }
                        }
                    }
                }
            }
            catch { };
            return activate;
        }

        public bool GetYahooMails(string yahooEmail, string yahooPassword)
        {
            GlobusHttpHelper HttpHelper = new GlobusHttpHelper();
            bool activate = false;
            try
            {
                bool emaildata = false;

                Chilkat.Http http = new Chilkat.Http();



                ///Chilkat Http Request to be used in Http Post...
                Chilkat.HttpRequest req = new Chilkat.HttpRequest();
                bool success;

                // Any string unlocks the component for the 1st 30-days.
                success = http.UnlockComponent("THEBACHttp_b3C9o9QvZQ06");
                if (success != true)
                {
                    Console.WriteLine(http.LastErrorText);
                    return false;
                }
                http.CookieDir = "memory";
                http.SendCookies = true;
                http.SaveCookies = true;

                //http.ProxyDomain = "127.0.0.1";
                //http.ProxyPort = 8888;




                http.SetRequestHeader("Accept-Encoding", "gzip,deflate");



                Chilkat.Imap iMap = new Imap();
                string Username = yahooEmail;
                string Password = yahooPassword;
                //Username = "Karlawtt201@yahoo.com";
                //Password = "rga77qViNIV";
                iMap.UnlockComponent("THEBACIMAPMAILQ_OtWKOHoF1R0Q");

                //iMap.
                //iMap.HttpProxyHostname = "127.0.0.1";
                //iMap.HttpProxyPort = 8888;

                iMap.Connect("imap.n.mail.yahoo.com");
                iMap.Login(yahooEmail, yahooPassword);
                iMap.SelectMailbox("Inbox");

                // Get a message set containing all the message IDs
                // in the selected mailbox.
                Chilkat.MessageSet msgSet;
                //msgSet = iMap.Search("FROM \"facebookmail.com\"", true);
                msgSet = iMap.GetAllUids();

                // Fetch all the mail into a bundle object.
                Chilkat.Email email = new Chilkat.Email();
                //bundle = iMap.FetchBundle(msgSet);
                string strEmail = string.Empty;
                List<string> lstData = new List<string>();
                if (msgSet != null)
                {
                    for (int i = msgSet.Count; i > 0; i--)
                    {
                        email = iMap.FetchSingle(msgSet.GetId(i), true);
                        strEmail = email.Subject;
                        string emailHtml = email.GetHtmlBody();
                        lstData.Add(strEmail);


                        if (email.Subject.Contains("[WordPress] Activate") && email.Subject.Contains("wordpress.com"))
                        {
                            foreach (string href in GetUrlsFromString(email.Body))
                            {
                                try
                                {

                                    string staticUrl = string.Empty;


                                    staticUrl = href;
                                    responce = http.QuickGetStr(staticUrl);

                                    if (responce.Contains("Your account is now active"))
                                    {
                                        emaildata = true;
                                        //Log("your Account is activate now");
                                        activate = true;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.StackTrace);
                                }
                            }
                        }
                    }
                }
                if (emaildata == false)
                {
                    Log("[ " + DateTime.Now + " ] => [ activation link is not find in user account so your Account is not activate please activate now ]");
                }
            }
            catch (Exception ex)
            {
            }
            return activate;
        }
        
        public bool Hotmails(string Email, string Password, ref Chilkat.Http http1)
        {
            bool activate = false;
            Chilkat.Http http = http1;
            GlobusHttpHelper HttpHelper = new GlobusHttpHelper();
            #region Hotmail
            if (Email.Contains("@hotmail"))
            {
                if (popClient.Connected)
                    popClient.Disconnect();
                popClient.Connect("pop3.live.com", int.Parse("995"), true);
                popClient.Authenticate(Email, Password);
                int Count = popClient.GetMessageCount();

                for (int i = Count; i >= 1; i--)
                {
                    OpenPOP.MIME.Message Message = popClient.GetMessage(i);
                    string subject = string.Empty;
                    subject = Message.Headers.Subject;

                    if (Message.Headers.Subject.Contains("[WordPress] Activate") && Message.Headers.Subject.Contains("wordpress.com"))
                    {
                        foreach (string href in GetUrlsFromStringHotmail(Message.MessageBody[0]))
                        {
                            try
                            {
                                string staticUrl = string.Empty;
                                staticUrl = href;

                                responce = http.QuickGetStr(staticUrl);
                                if (responce.Contains("Your account is now active"))
                                {
                                    Log("Account activated");
                                    activate = true;
                                }
                            }
                            catch (Exception ex)
                            {
                                
                            }
                        }
                    }
                }
            }
            #endregion
            return activate;
        }

        public bool HotmailWithoutReference(string Email, string Password)
        {
            GlobusHttpHelper HttpHelper = new GlobusHttpHelper();
            bool activate = false;
            Chilkat.Http http =new Chilkat.Http();
           
            #region Hotmail
            try
            {
                if (Email.Contains("@hotmail"))
                {
                    if (popClient.Connected)
                        popClient.Disconnect();
                    popClient.Connect("pop3.live.com", int.Parse("995"), true);
                    popClient.Authenticate(Email, Password);
                    int Count = popClient.GetMessageCount();

                    for (int i = Count; i >= 1; i--)
                    {
                        OpenPOP.MIME.Message Message = popClient.GetMessage(i);
                        string subject = string.Empty;
                        subject = Message.Headers.Subject;

                        if (Message.Headers.Subject.Contains("[WordPress] Activate") && Message.Headers.Subject.Contains("wordpress.com"))
                        {
                            foreach (string href in GetUrlsFromStringHotmail(Message.MessageBody[0]))
                            {
                                try
                                {
                                    string staticUrl = string.Empty;
                                    staticUrl = href;

                                    string res = HttpHelper.getHtmlfromUrl1(new Uri(staticUrl));

                                    responce = http.QuickGetStr(staticUrl);
                                    if (responce.Contains("Your account is now active"))
                                    {
                                        Log("Account activated");
                                        activate = true;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    
                                }
                            }
                        }
                    }
                }
            }
            catch { };
            #endregion
            return activate; 
        }

        public List<string> GetUrlsFromStringGmail(string HtmlData)
        {
            List<string> lstUrl = new List<string>();
            // *** Change by ritesh 10-9-11 ********************************************
            string strData = HtmlData;
            string[] arr = Regex.Split(strData, "\n");
            try
            {
                foreach (string strhref in arr)
                {
                    if (!strhref.Contains("<!DOCTYPE"))
                    {
                        if (strhref.Contains("http://www.linkedin.com/"))
                        {
                            //string tempString = strhref.Substring(0, strhref.IndexOf("\">"));
                            string tempString = strhref.Trim();
                            string DataUrl = tempString.Replace("=\"", "");
                            //if (tempString.Contains("https://twitter.com/account/confirm_email/"))
                            //{
                            lstUrl.Add(DataUrl);
                            //}
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("9 :" + ex.StackTrace);
            };
            // *** Change by ritesh 10-9-11 ********************************************
            return lstUrl;
        }

        public List<string> GetUrlsFromStringHotmail(string HtmlData)
        {
            List<string> lstUrl = new List<string>();
    

            // *** Change by ritesh 10-9-11 ********************************************
            string strData = HtmlData;
            string[] arr = Regex.Split(strData, "\n");

            foreach (string strhref in arr)
            {
                if (!strhref.Contains("<!DOCTYPE"))
                {
                    if (strhref.Contains("http://signup.wordpress.com"))
                    {
                        string tempString = strhref;
                        //string[] tempArr = tempString.Split('"');
                        //string DataUrl = tempArr[1];
                        tempString = tempString.Replace("\r", "");
                        if (tempString.Contains("http://signup.wordpress.com"))
                        {
                            lstUrl.Add(tempString);
                        }

                    }
                }
            }
            // *** Change by ritesh 10-9-11 ********************************************
            return lstUrl;
        }

        public List<string> GetUrlsFromString(string HtmlData)
        {
            List<string> lstUrl = new List<string>();
            try
            {
                // *** Change by ritesh 10-9-11 ********************************************
                string strData = HtmlData;
                string[] arr = Regex.Split(strData, "href");

                foreach (string strhref in arr)
                {
                    if (!strhref.Contains("<!DOCTYPE"))
                    {
                        if (strhref.Contains("http://www.linkedin.com/"))
                        {
                            string tempString = strhref.Substring(0, strhref.IndexOf("\">"));
                            string DataUrl = tempString.Replace("=\"","");

                            if (DataUrl.Contains("confirm_your_email"))
                            {
                                if (DataUrl.Contains("email="))
                                {
                                    DataUrl = Regex.Split(DataUrl, "style=")[0].Replace("=\"", string.Empty).Replace("&amp;", "&").Replace("\"", string.Empty).Trim();
                                    lstUrl.Add(DataUrl);
                                    break;
                                }

                            }
                        }
                    }
                }
                return lstUrl;
            }
            catch (Exception ex)
            {
               Console.WriteLine("8 :" + ex.StackTrace);
               return lstUrl;
            }
        }
        #endregion
    }
}
