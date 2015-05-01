using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chilkat;
using System.Text.RegularExpressions;
using System.Threading;

namespace BaseLib
{
    public class ChilkatIMAP
    {
        #region global declaration
        public string Username = string.Empty;
        public string Password = string.Empty;
        public string proxyAddress = string.Empty;
        public string proxyPort = string.Empty;
        public string proxyUser = string.Empty;
        public string proxyPass = string.Empty;
        Chilkat.Imap iMap = new Imap(); 
        #endregion

        #region Connect
        public string Connect(string yahooEmail, string yahooPassword)
        {
            iMap.UnlockComponent("THEBACIMAPMAILQ_OtWKOHoF1R0Q");
            iMap.Connect("imap.n.mail.yahoo.com");
            //iMap.Login("Karlawtt201@yahoo.com", "rga77qViNIV");
            iMap.Login(yahooEmail, yahooPassword);
            iMap.SelectMailbox("Inbox");

            // Get a message set containing all the message IDs
            // in the selected mailbox.
            Chilkat.MessageSet msgSet;
            msgSet = iMap.Search("ALL", true);

            // Fetch all the mail into a bundle object.
            Chilkat.Email email = new Chilkat.Email();
            //bundle = iMap.FetchBundle(msgSet);

            for (int i = msgSet.Count - 1; i > 0; i--)
            {
                email = iMap.FetchSingle(msgSet.GetId(i), true);
                if (email.Subject.Contains("Action Required: Confirm Your Facebook Account"))
                {
                    int startIndex = email.Body.IndexOf("http://www.facebook.com/confirmemail.php?e=");
                    int endIndex = email.Body.IndexOf(">", startIndex) - 1;
                    string activationLink = email.Body.Substring(startIndex, endIndex - startIndex).Replace("\r\n", "");
                    activationLink = activationLink.Replace("3D", "").Replace("hotmai=l", "hotmail").Replace("%40", "@");

                    string decodedActivationLink = System.Web.HttpUtility.UrlDecode(activationLink);

                    return decodedActivationLink;
                }
            }
            return string.Empty;

            //if (email.)
            //{

            //}

        } 
        #endregion


        /// <summary>
        /// Gets into Yahoo Email and fetches Activation Link and Sends Http Request to it
        /// Calls LoginVerfy() which sends Http Request
        /// Also sends sends Request to gif URL, and 2 more URLs
        /// </summary>
        /// <param name="yahooEmail"></param>
        /// <param name="yahooPassword"></param>
        #region GetFBMails
        public void GetFBMails(string yahooEmail, string yahooPassword)
        {
            Username = yahooEmail;
            Password = yahooPassword;
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
                    if (email.Subject.Contains("Action Required: Confirm Your Facebook Account"))
                    {
                        foreach (string href in GetUrlsFromString(email.Body))
                        {
                            try
                            {
                                string staticUrl = string.Empty;
                                string email_open_log_picUrl = string.Empty;

                                string strBody = email.Body;
                                string[] arr = Regex.Split(strBody, "src=");
                                foreach (string item in arr)
                                {
                                    if (!item.Contains("<!DOCTYPE"))
                                    {
                                        if (item.Contains("static"))
                                        {
                                            string[] arrStatic = item.Split('"');
                                            staticUrl = arrStatic[1];
                                        }
                                        if (item.Contains("email_open_log_pic"))
                                        {
                                            string[] arrlog_pic = item.Split('"');
                                            email_open_log_picUrl = arrlog_pic[1];
                                            email_open_log_picUrl = email_open_log_picUrl.Replace("amp;", "");
                                            break;
                                        }
                                    }
                                }

                                string href1 = href.Replace("&amp;report=1", "");
                                href1 = href.Replace("amp;", "");

                                //LoginVerfy(href1, staticUrl, email_open_log_picUrl);
                                break;
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.StackTrace);
                            }
                        }
                        //return;
                    }
                    else if (email.Subject.Contains("Just one more step to get started on Facebook"))
                    {
                        foreach (string href in GetUrlsFromString(email.Body))
                        {
                            try
                            {
                                string staticUrl = string.Empty;
                                string email_open_log_picUrl = string.Empty;

                                string strBody = email.Body;
                                string[] arr = Regex.Split(strBody, "src=");
                                foreach (string item in arr)
                                {
                                    if (!item.Contains("<!DOCTYPE"))
                                    {
                                        if (item.Contains("static"))
                                        {
                                            string[] arrStatic = item.Split('"');
                                            staticUrl = arrStatic[1];
                                        }
                                        if (item.Contains("email_open_log_pic"))
                                        {
                                            string[] arrlog_pic = item.Split('"');
                                            email_open_log_picUrl = arrlog_pic[1];
                                            email_open_log_picUrl = email_open_log_picUrl.Replace("amp;", "");
                                            break;
                                        }
                                    }
                                }


                                string href1 = href.Replace("&amp;report=1", "");
                                href1 = href.Replace("amp;", "");

                                //LoginVerfy(href1, staticUrl, email_open_log_picUrl);
                                break;
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.StackTrace);
                            }
                        }
                        //return;
                    }

                }
            }
        } 
        #endregion

        #region LoginVerifyChilkat
        public void LoginVerifyChilkat(ref Http http)
        {
            // Chilkat.Http http1 = new Chilkat.Http();
            // http1 = http;

            // ///Chilkat Http Request to be used in Http Post...
            // Chilkat.HttpRequest req = new Chilkat.HttpRequest();

            // bool success;

            // success = http.UnlockComponent("THEBACHttp_b3C9o9QvZQ06");
            // if (success != true)
            // {
            //     return;
            // }

            // ///Save Cookies...
            // http.CookieDir = "memory";
            // http.SendCookies = true;
            // http.SaveCookies = true;

            // string pageSource = http.QuickGetStr("http://www.facebook.com/login.php");
            // string valueLSD = "name=" + "\"lsd\"";
            // int startIndex = pageSource.IndexOf(valueLSD) + 18;
            // string value = pageSource.Substring(startIndex, 5);

            // ///Decode data as chilkat Request accepts only decoded data...
            // string charTest = "%E2%82%AC%2C%C2%B4%2C%E2%82%AC%2C%C2%B4%2C%E6%B0%B4%2C%D0%94%2C%D0%84";
            // string emailUser = Username.Split('@')[0] + "%40" + Username.Split('@')[1];
            // string DecodedCharTest = System.Web.HttpUtility.UrlDecode(charTest);
            // string DecodedEmail = System.Web.HttpUtility.UrlDecode(emailUser);


            // //  Build an HTTP POST Request:
            // req.UsePost();
            // //req.Path = "/login.php?login_attempt=1";

            // req.AddHeader("User-Agent", "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.16) Gecko/20110319 Firefox/3.6.16");
            // //req.SetFromUrl("http://www.facebook.com/login.php?login_attempt=1");

            // req.AddParam("charset_test", DecodedCharTest);
            // req.AddParam("lsd", value);
            // req.AddParam("locale", "en_US");
            // req.AddParam("email", DecodedEmail);
            // req.AddParam("pass", Password);
            // req.AddParam("persistent", "1");
            // req.AddParam("default_persistent", "1");
            // req.AddParam("charset_test", DecodedCharTest);
            // req.AddParam("lsd", value);

            // Chilkat.HttpResponse respUsingPostURLEncoded = http.PostUrlEncoded("http://www.facebook.com/login.php?login_attempt=1", req);

            // if (respUsingPostURLEncoded == null)
            // {
            //     Thread.Sleep(1000);
            //     respUsingPostURLEncoded = http.PostUrlEncoded("http://www.facebook.com/login.php?login_attempt=1", req);
            // }
            // if (respUsingPostURLEncoded == null)
            // {
            //     return;
            // }

            // string ResponseLoginPostURLEncoded = respUsingPostURLEncoded.BodyStr;

            // string ResponseLogin = http.QuickGetStr("http://www.facebook.com/home.php");

            // if (ResponseLogin == null)
            // {
            //     Console.WriteLine("not login");
            // }

            // string ResponseConfirmed = http.QuickGetStr("http://www.facebook.com/?email_confirmed=1");
            //// http://www.facebook.com/desktop/notifier/transfer.php?__a=1

        } 
        #endregion


        /// <summary>
        /// Sends Http Request to URL sent and also sends Request to gif URL, and 2 more URLs
        /// </summary>
        /// <param name="ConfemUrl">Facebook Confirmation URL</param>
        /// <param name="gif"></param>
        /// <param name="logpic"></param>
        //public void LoginVerfy(string ConfemUrl,string gif,string logpic)
        //{
        //    GlobusHttpHelper HttpHelper = new GlobusHttpHelper();

        //    int intProxyPort = 80;
        //    Regex IdCheck = new Regex("^[0-9]*$");

        //    if (!string.IsNullOrEmpty(proxyPort) && IdCheck.IsMatch(proxyPort))
        //    {
        //        intProxyPort = int.Parse(proxyPort);
        //    }

        //    string PageSourse1 = HttpHelper.getHtmlfromUrlProxy(new Uri(ConfemUrl), proxyAddress, intProxyPort, proxyUser, proxyPass);
        //    //string PageSourse1 = HttpHelper.getHtmlfromUrlProxy(new Uri(url), "127.0.0.1", 8888, "", "");

        //    string valueLSD = "name=" + "\"lsd\"";
        //    string pageSource = HttpHelper.getHtmlfromUrl(new Uri("https://www.facebook.com/login.php"));

        //    int startIndex = pageSource.IndexOf(valueLSD) + 18;
        //    string value = pageSource.Substring(startIndex, 5);

        //    //Log("Logging in with " + Username);

        //    string ResponseLogin = HttpHelper.postFormData(new Uri("https://www.facebook.com/login.php?login_attempt=1"), "charset_test=%E2%82%AC%2C%C2%B4%2C%E2%82%AC%2C%C2%B4%2C%E6%B0%B4%2C%D0%94%2C%D0%84&lsd=" + value + "&locale=en_US&email=" + Username.Split('@')[0] + "%40" + Username.Split('@')[1] + "&pass=" + Password + "&persistent=1&default_persistent=1&charset_test=%E2%82%AC%2C%C2%B4%2C%E2%82%AC%2C%C2%B4%2C%E6%B0%B4%2C%D0%94%2C%D0%84&lsd=" + value + "");
        //    /////ssss gif &s=a parse com  &s=a//////////////////////////
        //    string PageSourse12 = HttpHelper.getHtmlfromUrl(new Uri(ConfemUrl));
        //    string PageSourse13 = HttpHelper.getHtmlfromUrl(new Uri(gif));
        //    string PageSourse14 = HttpHelper.getHtmlfromUrl(new Uri(logpic+ "&s=a"));
        //    string PageSourse15 = HttpHelper.getHtmlfromUrl(new Uri(logpic));

        //    string PageSourse16 = HttpHelper.getHtmlfromUrl(new Uri(ConfemUrl));


        //    string PageSourceConfirmed = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/?email_confirmed=1"));

        //    string pageSourceCheck = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/gettingstarted.php?step=contact_importer"));
        //    //** FB Account Check email varified or not ***********************************************************************************//
        //    #region  FB Account Check email varified or not

        //    string pageSrc1 = string.Empty;
        //    string pageSrc2 = string.Empty;
        //    string pageSrc3 = string.Empty;
        //    string pageSrc4 = string.Empty;
        //    string substr1 = string.Empty;

        //    if (pageSourceCheck.Contains("Are your friends already on Facebook?") && pageSourceCheck.Contains("Skip this step"))
        //    {
        //        pageSrc1 = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/gettingstarted.php?step=classmates_coworkers"));
        //    }
        //    if (pageSrc1.Contains("Fill out your Profile Info") && pageSrc1.Contains("Skip"))
        //    {
        //        pageSrc2 = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/gettingstarted.php?step=upload_profile_pic"));
        //    }
        //    if (pageSrc2.Contains("Set your profile picture") && pageSrc2.Contains("Skip"))
        //    {
        //        pageSrc3 = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/gettingstarted.php?step=summary"));
        //    }
        //    if (pageSrc3.Contains("complete the sign-up process"))
        //    {
        //        //LoggerWallPoste("not varified through " + Username);

        //    }
        //    if (pageSourceCheck.Contains("complete the sign-up process"))
        //    {
        //        //LoggerWallPoste("not varified through Email" + Username);
        //    }
        //    #endregion
        //    //** FB Account Check email varified or not ***********************************************************************************//

        //    string pageSourceHome = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/home.php"));

        //    //** User Id ***************//////////////////////////////////
        //    string UsreId = string.Empty;
        //    string ProFilePost = string.Empty;
        //    ////**Post Message For User***********************/////////////////////////////////////////////////////
        //    int count = 0;
        //    if (pageSourceHome.Contains("http://www.facebook.com/profile.php?id="))
        //    {
        //        string[] arrUser = Regex.Split(pageSourceHome, "href");
        //        foreach (String itemUser in arrUser)
        //        {
        //            if (!itemUser.Contains("<!DOCTYPE"))
        //            {
        //                if (itemUser.Contains("http://www.facebook.com/profile.php?id="))
        //                {
                            
        //                        string[] arrhre = itemUser.Split('"');
        //                        ProFilePost = arrhre[1];
        //                        break;
                            
                            
        //                }
        //            }
        //        }
        //    }
        //    if (ProFilePost.Contains("http://www.facebook.com/profile.php?id="))
        //    {
        //        UsreId = ProFilePost.Replace("http://www.facebook.com/profile.php?id=", "");
        //    }



        //    //*** User Id **************//////////////////////////////////

        //    //*** Post Data **************//////////////////////////////////
        //    string fb_dtsg = pageSourceHome.Substring(pageSourceHome.IndexOf("fb_dtsg") + 16, 8);

        //    string post_form_id = pageSourceHome.Substring(pageSourceHome.IndexOf("post_form_id"), 200);
        //    string[] Arr = post_form_id.Split('"');
        //    post_form_id = Arr[4];
        //    post_form_id = post_form_id.Replace("\\", "");
        //    post_form_id = post_form_id.Replace("\\", "");
        //    post_form_id = post_form_id.Replace("\\", "");
        //    string Response1 = HttpHelper.postFormData(new Uri("http://www.facebook.com/desktop/notifier/transfer.php?__a=1"), "post_form_id="+post_form_id+"&fb_dtsg="+fb_dtsg+"&lsd&post_form_id_source=AsyncRequest&__user="+UsreId);

        //    string Response2 = HttpHelper.postFormData(new Uri("http://www.facebook.com/ajax/httponly_cookies.php?dc=snc2&__a=1"), "keys[0]=1150335208&post_form_id="+post_form_id+"&fb_dtsg="+fb_dtsg+"&lsd&post_form_id_source=AsyncRequest&__user="+UsreId);

        //    string PageSourceCon = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/ajax/contextual_help.php?__a=1&set_name=welcome&__user="+UsreId));


        //    string pageSourceCheck1111 = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/"));

        //    pageSourceCheck1111 = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/"));

        //    if (pageSourceCheck1111.Contains("complete the sign-up process"))
        //    {
        //        Console.WriteLine("the account is not verified");
        //        //LoggerWallPoste("not varified through Email" + Username);
        //    }

        //    string pageSource11 = HttpHelper.getHtmlfromUrl(new Uri("https://www.facebook.com/login.php"));

        //    startIndex = pageSource.IndexOf(valueLSD) + 18;
        //    value = pageSource.Substring(startIndex, 5);

        //    //Log("Logging in with " + Username);

        //    string ResponseLogin11 = HttpHelper.postFormData(new Uri("https://www.facebook.com/login.php?login_attempt=1"), "charset_test=%E2%82%AC%2C%C2%B4%2C%E2%82%AC%2C%C2%B4%2C%E6%B0%B4%2C%D0%94%2C%D0%84&lsd=" + value + "&locale=en_US&email=" + Username.Split('@')[0] + "%40" + Username.Split('@')[1] + "&pass=" + Password + "&persistent=1&default_persistent=1&charset_test=%E2%82%AC%2C%C2%B4%2C%E2%82%AC%2C%C2%B4%2C%E6%B0%B4%2C%D0%94%2C%D0%84&lsd=" + value + "");

        //    string PageSourceConfirmed11 = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/?email_confirmed=1"));

        //    if (PageSourceConfirmed11.Contains("complete the sign-up process"))
        //    {
        //        Console.WriteLine("the account is not verified");
        //        //LoggerWallPoste("not varified through Email" + Username);
        //    }
        //}

        public List<string> GetUrlsFromString(string HtmlData)
        {
            
            List<string> lstUrl = new List<string>();
            var regex = new Regex(@"http://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", RegexOptions.Compiled);
            var ModifiedString = HtmlData.Replace("\"", " ").Replace("<", " ").Replace(">", " ");
            foreach (Match url in regex.Matches(ModifiedString))
            {
                lstUrl.Add(url.Value);
            }
            //** Change Ritesh Satthya////////////////////////////////////////////
            //try
            //{
            //    string strData = HtmlData;
            //    string[] arr = Regex.Split(strData, "href");

            //    foreach (string strhref in arr)
            //    {
            //        if (!strhref.Contains("<!DOCTYPE"))
            //        {
            //            string[] tempArr = strhref.Split('"');
            //            lstUrl.Add(tempArr[1]);
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.StackTrace);
            //}
            ////** Change Ritesh Satthya////////////////////////////////////////////
            return lstUrl;
        }

    }
}


