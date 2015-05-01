using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Web;
using System.IO;
using System.Collections.Specialized;
using System.Text.RegularExpressions;



namespace BaseLib
{
    public class GlobusHttpHelper
    {
        #region Global Declarations
        public CookieCollection gCookies;
        
        HttpWebRequest gRequest;
        public HttpWebResponse gResponse;
        public string UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:18.0) Gecko/20100101 Firefox/18.0"; //"Mozilla/5.0 (Windows NT 6.1; WOW64; rv:30.0) Gecko/20100101 Firefox/30.0";
        public string Accept = "application/json,text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8, */*";
        string proxyAddress = string.Empty;
        int port = 80;
        string proxyUsername = string.Empty;
        string proxyPassword = string.Empty;
        public static List<string> lstpicfile = new List<string>();
        int Timeout = 90000;
        #endregion
       
        #region getHtmlfromUrl
        public string getHtmlfromUrl(Uri url)
        {
           
            string responseString = string.Empty;
            try
            {
                setExpect100Continue();
                gRequest = (HttpWebRequest)WebRequest.Create(url);

                gRequest.Accept = Accept;
                //gRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:29.0) Gecko/20100101 Firefox/29.0";//UserAgent;
                //gRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:28.0) Gecko/20100101 Firefox/28.0";

                gRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:18.0) Gecko/20100101 Firefox/18.0";
                gRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                //gRequest.Headers["Accept-Charset"] = "ISO-8859-1,utf-8;q=0.7,*;q=0.7";
                //gRequest.Headers["Cache-Control"] = "max-age=0";
                gRequest.Headers["Accept-Language"] = "en-us,en;q=0.5";
                //gRequest.Timeout = Timeout;

                gRequest.KeepAlive = true;

                gRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                gRequest.Headers.Add("Javascript-enabled", "true");

                gRequest.CookieContainer = new CookieContainer(); //gCookiesContainer;
                
                gRequest.Method = "GET";
                //gRequest.AllowAutoRedirect = false;
                ChangeProxy(proxyAddress, port, proxyUsername, proxyPassword);

                #region CookieManagment

                if (this.gCookies != null && this.gCookies.Count > 0)
                {
                    setExpect100Continue();
                    gRequest.CookieContainer.Add(gCookies);

                    try
                    {
                        //gRequest.CookieContainer.Add(url, new Cookie("__qca", "P0 - 2078004405 - 1321685323158", "/"));
                        //gRequest.CookieContainer.Add(url, new Cookie("__utma", "101828306.1814567160.1321685324.1322116799.1322206824.9", "/"));
                        //gRequest.CookieContainer.Add(url, new Cookie("__utmz", "101828306.1321685324.1.1.utmcsr=(direct)|utmccn=(direct)|utmcmd=(none)", "/"));
                        //gRequest.CookieContainer.Add(url, new Cookie("__utmb", "101828306.2.10.1321858563", "/"));
                        //gRequest.CookieContainer.Add(url, new Cookie("__utmc", "101828306", "/"));
                    }
                    catch (Exception ex)
                    {

                    }
                }
                //Get Response for this request url

                setExpect100Continue();
                using (gResponse = (HttpWebResponse)gRequest.GetResponse())

                //check if the status code is http 200 or http ok

                //using (HttpWebResponse webresponse = (HttpWebResponse) myRequest.GetResponse())
                //{
                //    if (webresponse.StatusCode == HttpStatusCode.OK)
                //    {
                //        continue;
                //    }
                //    ...
                //}
                {
                    if (gResponse.StatusCode == HttpStatusCode.OK)
                    {
                        //get all the cookies from the current request and add them to the response object cookies
                        setExpect100Continue();
                        gResponse.Cookies = gRequest.CookieContainer.GetCookies(gRequest.RequestUri);


                        //check if response object has any cookies or not

                        if (gResponse.Cookies.Count > 0)
                        {
                            //check if this is the first request/response, if this is the response of first request gCookies
                            //will be null
                            if (this.gCookies == null)
                            {
                                gCookies = gResponse.Cookies;
                            }
                            else
                            {
                                foreach (Cookie oRespCookie in gResponse.Cookies)
                                {
                                    bool bMatch = false;
                                    foreach (Cookie oReqCookie in this.gCookies)
                                    {
                                        if (oReqCookie.Name == oRespCookie.Name)
                                        {
                                            oReqCookie.Value = oRespCookie.Value;
                                            bMatch = true;
                                            break; // 
                                        }
                                    }
                                    if (!bMatch)
                                        this.gCookies.Add(oRespCookie);
                                }
                            }
                        }
                #endregion

                        StreamReader reader = new StreamReader(gResponse.GetResponseStream());
                        responseString = reader.ReadToEnd();
                        reader.Close();
                        return responseString;
                    }

                    else
                    {
                        return "Error";
                    }
                }
            }
            catch
            {
            }
            return responseString;
        } 
        #endregion

        #region getHtmlfromUrlFollow
        public string getHtmlfromUrlFollow(Uri url)
        {

            string responseString = string.Empty;
            try
            {
                setExpect100Continue();
                gRequest = (HttpWebRequest)WebRequest.Create(url);

                gRequest.Accept = "*/*";//Accept;
                gRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:30.0) Gecko/20100101 Firefox/30.0";//"Mozilla/5.0 (Windows NT 6.1; rv:29.0) Gecko/20100101 Firefox/29.0";//UserAgent;
                //gRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:28.0) Gecko/20100101 Firefox/28.0";

                //gRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:18.0) Gecko/20100101 Firefox/18.0";
                gRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                //gRequest.Headers["Accept-Charset"] = "ISO-8859-1,utf-8;q=0.7,*;q=0.7";
                //gRequest.Headers["Cache-Control"] = "max-age=0";
                gRequest.Headers["Accept-Language"] = "en-US,en;q=0.5";
                //gRequest.Timeout = Timeout;

                gRequest.KeepAlive = true;

                gRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                gRequest.Headers.Add("X-IsAJAXForm", "1");
                gRequest.Headers.Add("X-LinkedIn-traceDataContext", "X-LI-ORIGIN-UUID=mHw4yX2ZbhOQ2gnwtioAAA==");
                gRequest.Headers.Add("X-Requested-With", "XMLHttpRequest");
                //gRequest.Headers.Add("Javascript-enabled", "true");

                gRequest.Referer = "http://www.linkedin.com/company/16447?trk=vsrp_companies_res_name&trkInfo=VSRPsearchId%3A3088972471400225293326%2CVSRPtargetId%3A16447%2CVSRPcmpt%3Aprimary";

                gRequest.CookieContainer = new CookieContainer(); //gCookiesContainer;

                gRequest.Method = "GET";
                //gRequest.AllowAutoRedirect = false;
                ChangeProxy(proxyAddress, port, proxyUsername, proxyPassword);

                #region CookieManagment

                if (this.gCookies != null && this.gCookies.Count > 0)
                {
                    setExpect100Continue();
                    gRequest.CookieContainer.Add(gCookies);

                    try
                    {
                        gRequest.CookieContainer.Add(url, new Cookie("__qca", "P0-927206458-1394431435703", "/"));
                        gRequest.CookieContainer.Add(url, new Cookie("__utma", "23068709.936287534.1400223934.1400223934.1400223934.1", "/"));
                        gRequest.CookieContainer.Add(url, new Cookie("__utmz", "23068709.1400223934.1.1.utmcsr=(direct)|utmccn=(direct)|utmcmd=(none)", "/"));
                        //gRequest.CookieContainer.Add(url, new Cookie("__utmb", "101828306.2.10.1321858563", "/"));
                        gRequest.CookieContainer.Add(url, new Cookie("__utmc", "23068709", "/"));
                        gRequest.CookieContainer.Add(url, new Cookie("__utmv", "23068709.user", "/"));
                        //gRequest.CookieContainer.Add(url, new Cookie("_lipt", "0_c5NNBJTGjorqUgah9q17vj1ZXD4AnOatVuW9yOx_adVtL2EgbecJL7k5yp2Q5xAfCQ9BRSTIrh2Sy-fosa0aAAUYebJNjSgq-JS-SGtGTOyW8YC7GyLM-tSXVt1zVHlZL8el8RpLDzJhxmo0Ds-M__aCNDQkBIVVE0ksd1O6sMi9llvuGNxHoJNkFGzBP3rmSr0HwVDLU5kmbGl4pZNHXVgQYY70EA7T7qfG9civX57"));
                        //_lipt="0_c5NNBJTGjorqUgah9q17vj1ZXD4AnOatVuW9yOx_adVtL2EgbecJL7k5yp2Q5xAfCQ9BRSTIrh2Sy-fosa0aAAUYebJNjSgq-JS-SGtGTOyW8YC7GyLM-tSXVt1zVHlZL8el8RpLDzJhxmo0Ds-M__aCNDQkBIVVE0ksd1O6sMi9llvuGNxHoJNkFGzBP3rmSr0HwVDLU5kmbGl4pZNHXVgQYY70EA7T7qfG9civX57"
                    }
                    catch (Exception ex)
                    {

                    }
                }
                //Get Response for this request url

                setExpect100Continue();
                using (gResponse = (HttpWebResponse)gRequest.GetResponse())

                //check if the status code is http 200 or http ok

                //using (HttpWebResponse webresponse = (HttpWebResponse) myRequest.GetResponse())
                //{
                //    if (webresponse.StatusCode == HttpStatusCode.OK)
                //    {
                //        continue;
                //    }
                //    ...
                //}
                {
                    if (gResponse.StatusCode == HttpStatusCode.OK)
                    {
                        //get all the cookies from the current request and add them to the response object cookies
                        setExpect100Continue();
                        gResponse.Cookies = gRequest.CookieContainer.GetCookies(gRequest.RequestUri);


                        //check if response object has any cookies or not

                        if (gResponse.Cookies.Count > 0)
                        {
                            //check if this is the first request/response, if this is the response of first request gCookies
                            //will be null
                            if (this.gCookies == null)
                            {
                                gCookies = gResponse.Cookies;
                            }
                            else
                            {
                                foreach (Cookie oRespCookie in gResponse.Cookies)
                                {
                                    bool bMatch = false;
                                    foreach (Cookie oReqCookie in this.gCookies)
                                    {
                                        if (oReqCookie.Name == oRespCookie.Name)
                                        {
                                            oReqCookie.Value = oRespCookie.Value;
                                            bMatch = true;
                                            break; // 
                                        }
                                    }
                                    if (!bMatch)
                                        this.gCookies.Add(oRespCookie);
                                }
                            }
                        }
                #endregion

                        StreamReader reader = new StreamReader(gResponse.GetResponseStream());
                        responseString = reader.ReadToEnd();
                        reader.Close();
                        return responseString;
                    }

                    else
                    {
                        return "Error";
                    }
                }
            }
            catch
            {
            }
            return responseString;
        }       
        #endregion

        #region getHtmlfromUrl1
        public string getHtmlfromUrl1(Uri url)
        {

            string responseString = string.Empty;
            try
            {
                setExpect100Continue();
                gRequest = (HttpWebRequest)WebRequest.Create(url);

                gRequest.Accept = Accept;
                gRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:29.0) Gecko/20100101 Firefox/29.0";//UserAgent;
                //gRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:28.0) Gecko/20100101 Firefox/28.0";

                //gRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:18.0) Gecko/20100101 Firefox/18.0";
                gRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                //gRequest.Headers["Accept-Charset"] = "ISO-8859-1,utf-8;q=0.7,*;q=0.7";
                //gRequest.Headers["Cache-Control"] = "max-age=0";
                gRequest.Headers["Accept-Language"] = "en-us,en;q=0.5";
                //gRequest.Timeout = Timeout;

                gRequest.KeepAlive = true;

                gRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                gRequest.Headers.Add("Javascript-enabled", "true");

                gRequest.CookieContainer = new CookieContainer(); //gCookiesContainer;

                gRequest.Method = "GET";
                //gRequest.AllowAutoRedirect = false;
                ChangeProxy(proxyAddress, port, proxyUsername, proxyPassword);

                #region CookieManagment

                if (this.gCookies != null && this.gCookies.Count > 0)
                {
                    setExpect100Continue();
                    gRequest.CookieContainer.Add(gCookies);

                    try
                    {
                        //gRequest.CookieContainer.Add(url, new Cookie("__qca", "P0 - 2078004405 - 1321685323158", "/"));
                        //gRequest.CookieContainer.Add(url, new Cookie("__utma", "101828306.1814567160.1321685324.1322116799.1322206824.9", "/"));
                        //gRequest.CookieContainer.Add(url, new Cookie("__utmz", "101828306.1321685324.1.1.utmcsr=(direct)|utmccn=(direct)|utmcmd=(none)", "/"));
                        //gRequest.CookieContainer.Add(url, new Cookie("__utmb", "101828306.2.10.1321858563", "/"));
                        //gRequest.CookieContainer.Add(url, new Cookie("__utmc", "101828306", "/"));
                    }
                    catch (Exception ex)
                    {

                    }
                }
                //Get Response for this request url

                setExpect100Continue();
                using (gResponse = (HttpWebResponse)gRequest.GetResponse())

                //check if the status code is http 200 or http ok

                //using (HttpWebResponse webresponse = (HttpWebResponse) myRequest.GetResponse())
                //{
                //    if (webresponse.StatusCode == HttpStatusCode.OK)
                //    {
                //        continue;
                //    }
                //    ...
                //}
                {
                    if (gResponse.StatusCode == HttpStatusCode.OK)
                    {
                        //get all the cookies from the current request and add them to the response object cookies
                        setExpect100Continue();
                        gResponse.Cookies = gRequest.CookieContainer.GetCookies(gRequest.RequestUri);


                        //check if response object has any cookies or not

                        if (gResponse.Cookies.Count > 0)
                        {
                            //check if this is the first request/response, if this is the response of first request gCookies
                            //will be null
                            if (this.gCookies == null)
                            {
                                gCookies = gResponse.Cookies;
                            }
                            else
                            {
                                foreach (Cookie oRespCookie in gResponse.Cookies)
                                {
                                    bool bMatch = false;
                                    foreach (Cookie oReqCookie in this.gCookies)
                                    {
                                        if (oReqCookie.Name == oRespCookie.Name)
                                        {
                                            oReqCookie.Value = oRespCookie.Value;
                                            bMatch = true;
                                            break; // 
                                        }
                                    }
                                    if (!bMatch)
                                        this.gCookies.Add(oRespCookie);
                                }
                            }
                        }
                #endregion

                        StreamReader reader = new StreamReader(gResponse.GetResponseStream());
                        responseString = reader.ReadToEnd();
                        reader.Close();
                        return responseString;
                    }

                    else
                    {
                        return "Error";
                    }
                }
            }
            catch
            {
            }
            return responseString;
        }
        #endregion

        #region getHtmlfromUrlNewRefre
        public string getHtmlfromUrlNewRefre(Uri url, string refree)
        {
            string responseString = string.Empty;
            try
            {
                setExpect100Continue();
                gRequest = (HttpWebRequest)WebRequest.Create(url);

                gRequest.Accept = Accept;
                gRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:21.0) Gecko/20100101 Firefox/21.0";//UserAgent;             
                gRequest.Headers["Accept-Language"] = "en-us,en;q=0.5";
                gRequest.KeepAlive = true;
                gRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                gRequest.Headers.Add("Javascript-enabled", "true");
                gRequest.CookieContainer = new CookieContainer(); //gCookiesContainer;           
                gRequest.Referer = refree;
                //gRequest.Headers.Add("X-Requested-With", "XMLHttpRequest");
                //gRequest.Headers.Add("X-LinkedIn-traceDataContext", "X-LI-ORIGIN-UUID=EMOjM2GVChPwxBOpLysAAA==");
                //gRequest.Headers.Add("X-IsAJAXForm", "1");

                gRequest.Method = "GET";
                //gRequest.AllowAutoRedirect = false;
                ChangeProxy(proxyAddress, port, proxyUsername, proxyPassword);

                #region CookieManagment

                if (this.gCookies != null && this.gCookies.Count > 0)
                {
                    setExpect100Continue();
                    gRequest.CookieContainer.Add(gCookies);

                    try
                    {
                        //gRequest.CookieContainer.Add(url, new Cookie("__qca", "P0 - 2078004405 - 1321685323158", "/"));
                        //gRequest.CookieContainer.Add(url, new Cookie("__utma", "101828306.1814567160.1321685324.1322116799.1322206824.9", "/"));
                        //gRequest.CookieContainer.Add(url, new Cookie("__utmz", "101828306.1321685324.1.1.utmcsr=(direct)|utmccn=(direct)|utmcmd=(none)", "/"));
                        //gRequest.CookieContainer.Add(url, new Cookie("__utmb", "101828306.2.10.1321858563", "/"));
                        //gRequest.CookieContainer.Add(url, new Cookie("__utmc", "101828306", "/"));
                    }
                    catch (Exception ex)
                    {

                    }
                }
                //Get Response for this request url

                setExpect100Continue();
                gResponse = (HttpWebResponse)gRequest.GetResponse();

                //check if the status code is http 200 or http ok
                if (gResponse.StatusCode == HttpStatusCode.OK)
                {
                    //get all the cookies from the current request and add them to the response object cookies
                    setExpect100Continue();
                    gResponse.Cookies = gRequest.CookieContainer.GetCookies(gRequest.RequestUri);


                    //check if response object has any cookies or not
                    if (gResponse.Cookies.Count > 0)
                    {
                        //check if this is the first request/response, if this is the response of first request gCookies
                        //will be null
                        if (this.gCookies == null)
                        {
                            gCookies = gResponse.Cookies;
                        }
                        else
                        {
                            foreach (Cookie oRespCookie in gResponse.Cookies)
                            {
                                bool bMatch = false;
                                foreach (Cookie oReqCookie in this.gCookies)
                                {
                                    if (oReqCookie.Name == oRespCookie.Name)
                                    {
                                        oReqCookie.Value = oRespCookie.Value;
                                        bMatch = true;
                                        break; // 
                                    }
                                }
                                if (!bMatch)
                                    this.gCookies.Add(oRespCookie);
                            }
                        }
                    }
                #endregion

                    StreamReader reader = new StreamReader(gResponse.GetResponseStream());
                    responseString = reader.ReadToEnd();
                    reader.Close();
                    return responseString;
                }
                else
                {
                    return "Error";
                }
            }
            catch
            {
            }
            return responseString;
        } 
        #endregion

        #region getHtmlfromUrl
        public string getHtmlfromUrl(Uri url, string referer)
        {
            string responseString = string.Empty;
            try
            {
                setExpect100Continue();
                gRequest = (HttpWebRequest)WebRequest.Create(url);

                gRequest.Accept = Accept;
                gRequest.UserAgent = UserAgent;
                //gRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:18.0) Gecko/20100101 Firefox/18.0";
                //gRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                gRequest.Headers["Accept-Charset"] = "ISO-8859-1,utf-8;q=0.7,*;q=0.7";
                //gRequest.Headers["Cache-Control"] = "max-age=0";
                gRequest.Headers["Accept-Language"] = "en-us,en;q=0.5";
                //gRequest.Connection = "keep-alive";

                gRequest.KeepAlive = true;

                gRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                gRequest.CookieContainer = new CookieContainer(); //gCookiesContainer;

                gRequest.Method = "GET";
                //gRequest.AllowAutoRedirect = false;

                if (!string.IsNullOrEmpty(referer))
                {
                    gRequest.Referer = referer;
                }

                ChangeProxy(proxyAddress, port, proxyUsername, proxyPassword);

                #region CookieManagment

                if (this.gCookies != null && this.gCookies.Count > 0)
                {
                    setExpect100Continue();
                    gRequest.CookieContainer.Add(gCookies);

                    try
                    {
                        //gRequest.CookieContainer.Add(url, new Cookie("__qca", "P0 - 2078004405 - 1321685323158", "/"));
                        //gRequest.CookieContainer.Add(url, new Cookie("__utma", "101828306.1814567160.1321685324.1322116799.1322206824.9", "/"));
                        //gRequest.CookieContainer.Add(url, new Cookie("__utmz", "101828306.1321685324.1.1.utmcsr=(direct)|utmccn=(direct)|utmcmd=(none)", "/"));
                        //gRequest.CookieContainer.Add(url, new Cookie("__utmb", "101828306.2.10.1321858563", "/"));
                        //gRequest.CookieContainer.Add(url, new Cookie("__utmc", "101828306", "/"));
                    }
                    catch (Exception ex)
                    {

                    }
                }
                //Get Response for this request url

                setExpect100Continue();
                gResponse = (HttpWebResponse)gRequest.GetResponse();

                //check if the status code is http 200 or http ok
                if (gResponse.StatusCode == HttpStatusCode.OK)
                {
                    //get all the cookies from the current request and add them to the response object cookies
                    setExpect100Continue();
                    gResponse.Cookies = gRequest.CookieContainer.GetCookies(gRequest.RequestUri);


                    //check if response object has any cookies or not
                    if (gResponse.Cookies.Count > 0)
                    {
                        //check if this is the first request/response, if this is the response of first request gCookies
                        //will be null
                        if (this.gCookies == null)
                        {
                            gCookies = gResponse.Cookies;
                        }
                        else
                        {
                            foreach (Cookie oRespCookie in gResponse.Cookies)
                            {
                                bool bMatch = false;
                                foreach (Cookie oReqCookie in this.gCookies)
                                {
                                    if (oReqCookie.Name == oRespCookie.Name)
                                    {
                                        oReqCookie.Value = oRespCookie.Value;
                                        bMatch = true;
                                        break; // 
                                    }
                                }
                                if (!bMatch)
                                    this.gCookies.Add(oRespCookie);
                            }
                        }
                    }
                #endregion

                    using (StreamReader reader = new StreamReader(gResponse.GetResponseStream()))
                    {
                        responseString = reader.ReadToEnd();
                    }
                    return responseString;
                }
                else
                {
                    return "Error";
                }
            }
            catch
            {
            }
            return responseString;
        } 
        #endregion

        #region getHtmlfromUrlProxy
        public string getHtmlfromUrlProxy(Uri url, string proxyAddress, int port, string proxyUsername, string proxyPassword)
        {
            string responseString = string.Empty;

            try
            {

                setExpect100Continue();
                gRequest = (HttpWebRequest)WebRequest.Create(url);
                gRequest.Accept = Accept;
                gRequest.UserAgent = UserAgent;
                //gRequest.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.24) Gecko/20111103 Firefox/3.6.24";
                gRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:21.0) Gecko/20100101 Firefox/21.0";//UserAgent;  
               // gRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:30.0) Gecko/20100101 Firefox/30.0";
                gRequest.Headers["Accept-Language"] = "en-us,en;q=0.5";
                gRequest.KeepAlive = true;
                gRequest.Headers.Add("Javascript-enabled", "true");
                //gRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:18.0) Gecko/20100101 Firefox/18.0";
                //gRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                //gRequest.Headers["Accept-Charset"] = "ISO-8859-1,utf-8;q=0.7,*;q=0.7";
                //gRequest.Headers["Cache-Control"] = "max-age=0";
                gRequest.Headers["Accept-Language"] = "en-us,en;q=0.5";
                //gRequest.Connection = "keep-alive";
                //gRequest.Timeout = Timeout;
                gRequest.KeepAlive = true;

                gRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                // gRequest.Referer = "http://www.linkedin.com/people/pymk?trk=frontier-tabs_connections-new_pymk";
                //gRequest.Headers["X-IsAJAXForm"] = "1";
                //gRequest.Headers["X-Requested-With"] = "XMLHttpRequest";

                //gRequest.Headers.Add("Javascript-enabled", "true");

                ///Set Proxy
                this.proxyAddress = proxyAddress;
                this.port = port;
                this.proxyUsername = proxyUsername;
                this.proxyPassword = proxyPassword;

                ChangeProxy(proxyAddress, port, proxyUsername, proxyPassword);

                gRequest.CookieContainer = new CookieContainer(); //gCookiesContainer;

                gRequest.Method = "GET";
                ////gRequest.Accept = "image/jpeg, application/x-ms-application, image/gif, application/xaml+xml, image/pjpeg, application/x-ms-xbap, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*";
                gRequest.KeepAlive = true;
                gRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                #region CookieManagment

                if (this.gCookies != null && this.gCookies.Count > 0)
                {
                    setExpect100Continue();
                    gRequest.CookieContainer.Add(gCookies);

                    //try
                    //{
                    //    gRequest.CookieContainer.Add(url, new Cookie("__qca", "P0-2078004405-1321685323158", "/"));
                    //    gRequest.CookieContainer.Add(url, new Cookie("__utma", "101828306.1814567160.1321685324.1321697619.1321858563.3", "/"));
                    //    gRequest.CookieContainer.Add(url, new Cookie("__utmz", "101828306.1321685324.1.1.utmcsr=(direct)|utmccn=(direct)|utmcmd=(none)", "/"));
                    //    gRequest.CookieContainer.Add(url, new Cookie("__utmb", "101828306.2.10.1321858563", "/"));
                    //    gRequest.CookieContainer.Add(url, new Cookie("__utmc", "101828306", "/"));
                    //}
                    //catch (Exception ex)
                    //{

                    //}
                }
                //Get Response for this request url

                setExpect100Continue();
                gResponse = (HttpWebResponse)gRequest.GetResponse();
                
                //check if the status code is http 200 or http ok
                if (gResponse.StatusCode == HttpStatusCode.OK)
                {
                    //get all the cookies from the current request and add them to the response object cookies
                    setExpect100Continue();
                    gResponse.Cookies = gRequest.CookieContainer.GetCookies(gRequest.RequestUri);


                    //check if response object has any cookies or not
                    if (gResponse.Cookies.Count > 0)
                    {
                        //check if this is the first request/response, if this is the response of first request gCookies
                        //will be null
                        if (this.gCookies == null)
                        {
                            gCookies = gResponse.Cookies;
                        }
                        else
                        {
                            foreach (Cookie oRespCookie in gResponse.Cookies)
                            {
                                bool bMatch = false;
                                foreach (Cookie oReqCookie in this.gCookies)
                                {
                                    if (oReqCookie.Name == oRespCookie.Name)
                                    {
                                        oReqCookie.Value = oRespCookie.Value;
                                        bMatch = true;
                                        break;
                                    }
                                }
                                if (!bMatch)
                                    this.gCookies.Add(oRespCookie);
                            }
                        }
                    }
                #endregion

                    StreamReader reader = new StreamReader(gResponse.GetResponseStream());
                    responseString = reader.ReadToEnd();
                    reader.Close();
                    return responseString;
                }
                else
                {
                    return "Error";
                }
            }
            catch
            {
            }

            return responseString;
        } 
        #endregion

        #region getHtmlfromUrlProxy1
        public string getHtmlfromUrlProxy1(Uri url, string proxyAddress, int port, string proxyUsername, string proxyPassword)
        {
            string responseString = string.Empty;

            try
            {

                setExpect100Continue();
                gRequest = (HttpWebRequest)WebRequest.Create(url);
                gRequest.Accept = Accept;
                gRequest.UserAgent = UserAgent;
                //gRequest.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.24) Gecko/20111103 Firefox/3.6.24";
                //gRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:21.0) Gecko/20100101 Firefox/21.0";//UserAgent;             
                gRequest.Headers["Accept-Language"] = "en-us,en;q=0.5";
                gRequest.KeepAlive = true;
                gRequest.Headers.Add("Javascript-enabled", "true");
                //gRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:18.0) Gecko/20100101 Firefox/18.0";
                //gRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                //gRequest.Headers["Accept-Charset"] = "ISO-8859-1,utf-8;q=0.7,*;q=0.7";
                //gRequest.Headers["Cache-Control"] = "max-age=0";
                gRequest.Headers["Accept-Language"] = "en-us,en;q=0.5";
                //gRequest.Connection = "keep-alive";
                //gRequest.Timeout = Timeout;
                gRequest.KeepAlive = true;

                gRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                // gRequest.Referer = "http://www.linkedin.com/people/pymk?trk=frontier-tabs_connections-new_pymk";
                //gRequest.Headers["X-IsAJAXForm"] = "1";
                //gRequest.Headers["X-Requested-With"] = "XMLHttpRequest";

                //gRequest.Headers.Add("Javascript-enabled", "true");

                ///Set Proxy
                this.proxyAddress = proxyAddress;
                this.port = port;
                this.proxyUsername = proxyUsername;
                this.proxyPassword = proxyPassword;

                ChangeProxy(proxyAddress, port, proxyUsername, proxyPassword);

                gRequest.CookieContainer = new CookieContainer(); //gCookiesContainer;

                gRequest.Method = "GET";
                ////gRequest.Accept = "image/jpeg, application/x-ms-application, image/gif, application/xaml+xml, image/pjpeg, application/x-ms-xbap, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*";
                gRequest.KeepAlive = true;
                gRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                #region CookieManagment

                if (this.gCookies != null && this.gCookies.Count > 0)
                {
                    setExpect100Continue();
                    gRequest.CookieContainer.Add(gCookies);

                    //try
                    //{
                    //    gRequest.CookieContainer.Add(url, new Cookie("__qca", "P0-2078004405-1321685323158", "/"));
                    //    gRequest.CookieContainer.Add(url, new Cookie("__utma", "101828306.1814567160.1321685324.1321697619.1321858563.3", "/"));
                    //    gRequest.CookieContainer.Add(url, new Cookie("__utmz", "101828306.1321685324.1.1.utmcsr=(direct)|utmccn=(direct)|utmcmd=(none)", "/"));
                    //    gRequest.CookieContainer.Add(url, new Cookie("__utmb", "101828306.2.10.1321858563", "/"));
                    //    gRequest.CookieContainer.Add(url, new Cookie("__utmc", "101828306", "/"));
                    //}
                    //catch (Exception ex)
                    //{

                    //}
                }
                //Get Response for this request url

                setExpect100Continue();
                gResponse = (HttpWebResponse)gRequest.GetResponse();

                //check if the status code is http 200 or http ok
                if (gResponse.StatusCode == HttpStatusCode.OK)
                {
                    //get all the cookies from the current request and add them to the response object cookies
                    setExpect100Continue();
                    gResponse.Cookies = gRequest.CookieContainer.GetCookies(gRequest.RequestUri);


                    //check if response object has any cookies or not
                    if (gResponse.Cookies.Count > 0)
                    {
                        //check if this is the first request/response, if this is the response of first request gCookies
                        //will be null
                        if (this.gCookies == null)
                        {
                            gCookies = gResponse.Cookies;
                        }
                        else
                        {
                            foreach (Cookie oRespCookie in gResponse.Cookies)
                            {
                                bool bMatch = false;
                                foreach (Cookie oReqCookie in this.gCookies)
                                {
                                    if (oReqCookie.Name == oRespCookie.Name)
                                    {
                                        oReqCookie.Value = oRespCookie.Value;
                                        bMatch = true;
                                        break;
                                    }
                                }
                                if (!bMatch)
                                    this.gCookies.Add(oRespCookie);
                            }
                        }
                    }
                #endregion

                    StreamReader reader = new StreamReader(gResponse.GetResponseStream());
                    responseString = reader.ReadToEnd();
                    reader.Close();
                    return responseString;
                }
                else
                {
                    return "Error";
                }
            }
            catch
            {
            }

            return responseString;
        }
        #endregion

        #region getHtmlfromUrlProxy
        public string getHtmlfromUrlProxy(Uri url, string proxyAddress, int port, string proxyUsername, string proxyPassword, string Referes)
        {
            string responseString = string.Empty;

            try
            {
                setExpect100Continue();
                gRequest = (HttpWebRequest)WebRequest.Create(url);
                gRequest.Accept = Accept;
                gRequest.UserAgent = UserAgent;
                //gRequest.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.24) Gecko/20111103 Firefox/3.6.24";

                //gRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:18.0) Gecko/20100101 Firefox/18.0";
                //gRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                gRequest.Headers["Accept-Charset"] = "ISO-8859-1,utf-8;q=0.7,*;q=0.7";
                //gRequest.Headers["Cache-Control"] = "max-age=0";
                gRequest.Headers["Accept-Language"] = "en-us,en;q=0.5";
                //gRequest.Connection = "keep-alive";
                gRequest.Headers.Add("data-jsenabled", "true");
                gRequest.KeepAlive = true;

                gRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                // gRequest.Referer = "http://www.linkedin.com/people/pymk?trk=frontier-tabs_connections-new_pymk";
                //gRequest.Headers["X-IsAJAXForm"] = "1";
                //gRequest.Headers["X-Requested-With"] = "XMLHttpRequest";


                if (!string.IsNullOrEmpty(Referes))
                {
                    gRequest.Referer = Referes;
                }

                ///Set Proxy
                this.proxyAddress = proxyAddress;
                this.port = port;
                this.proxyUsername = proxyUsername;
                this.proxyPassword = proxyPassword;

                ChangeProxy(proxyAddress, port, proxyUsername, proxyPassword);

                gRequest.CookieContainer = new CookieContainer(); //gCookiesContainer;

                gRequest.Method = "GET";
                ////gRequest.Accept = "image/jpeg, application/x-ms-application, image/gif, application/xaml+xml, image/pjpeg, application/x-ms-xbap, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*";
                gRequest.KeepAlive = true;
                gRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                #region CookieManagment

                if (this.gCookies != null && this.gCookies.Count > 0)
                {
                    setExpect100Continue();
                    gRequest.CookieContainer.Add(gCookies);

                    try
                    {
                        //gRequest.CookieContainer.Add(url, new Cookie("__qca", "P0-2078004405-1321685323158", "/", ".linkedin.com"));
                        //gRequest.CookieContainer.Add(url, new Cookie("__utma", "101828306.1814567160.1321685324.1321697619.1321858563.3", "/"));
                        //gRequest.CookieContainer.Add(url, new Cookie("__utmz", "101828306.1321685324.1.1.utmcsr=(direct)|utmccn=(direct)|utmcmd=(none)", "/"));
                        //gRequest.CookieContainer.Add(url, new Cookie("__utmb", "101828306.2.10.1321858563", "/"));
                        //gRequest.CookieContainer.Add(url, new Cookie("__utmc", "101828306", "/"));
                        //gRequest.CookieContainer.Add(url, new Cookie("__utma","23068709.615412544.1388405447.1388405447.1388405447.1", "/", ".linkedin.com"));
                        //gRequest.CookieContainer.Add(url, new Cookie("utmc=23068709", "/", ".linkedin.com"));
                        //gRequest.CookieContainer.Add(url, new Cookie("__utmz=23068709.1388405447.1.1.utmcsr=(direct)|utmccn=(direct)|utmcmd=(none)", "/", ".linkedin.com"));
                        //gRequest.CookieContainer.Add(url, new Cookie("__qca=P0-479830185-1388405160910", "/", ".linkedin.com"));
                        //gRequest.CookieContainer.Add(url, new Cookie("L1e=19e93e8", "/", ".linkedin.com"));
                        //gRequest.CookieContainer.Add(url, new Cookie("__utmv=23068709.guest", "/", ".linkedin.com"));
                    }
                    catch (Exception ex)
                    {

                    }
                }
                //Get Response for this request url

                setExpect100Continue();
                gResponse = (HttpWebResponse)gRequest.GetResponse();

                //check if the status code is http 200 or http ok
                if (gResponse.StatusCode == HttpStatusCode.OK)
                {
                    //get all the cookies from the current request and add them to the response object cookies
                    setExpect100Continue();
                    gResponse.Cookies = gRequest.CookieContainer.GetCookies(gRequest.RequestUri);


                    //check if response object has any cookies or not
                    if (gResponse.Cookies.Count > 0)
                    {
                        //check if this is the first request/response, if this is the response of first request gCookies
                        //will be null
                        if (this.gCookies == null)
                        {
                            gCookies = gResponse.Cookies;
                        }
                        else
                        {
                            foreach (Cookie oRespCookie in gResponse.Cookies)
                            {
                                bool bMatch = false;
                                foreach (Cookie oReqCookie in this.gCookies)
                                {
                                    if (oReqCookie.Name == oRespCookie.Name)
                                    {
                                        oReqCookie.Value = oRespCookie.Value;
                                        bMatch = true;
                                        break; // 
                                    }
                                }
                                if (!bMatch)
                                    this.gCookies.Add(oRespCookie);
                            }
                        }
                    }
                #endregion

                    StreamReader reader = new StreamReader(gResponse.GetResponseStream());
                    responseString = reader.ReadToEnd();
                    reader.Close();
                    return responseString;
                }
                else
                {
                    return "Error";
                }
            }
            catch
            {
            }

            return responseString;
        } 
        #endregion

        #region getHtmlfromAsx
        public string getHtmlfromAsx(Uri url)
        {
            setExpect100Continue();
            gRequest = (HttpWebRequest)WebRequest.Create(url);
            //gRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:18.0) Gecko/20100101 Firefox/18.0";
            gRequest.CookieContainer = new CookieContainer();//new CookieContainer();
            gRequest.ContentType = "video/x-ms-asf";

            if (this.gCookies != null && this.gCookies.Count > 0)
            {
                gRequest.CookieContainer.Add(gCookies);
                setExpect100Continue();
            }
            //Get Response for this request url
            gResponse = (HttpWebResponse)gRequest.GetResponse();
            setExpect100Continue();
            //check if the status code is http 200 or http ok
            if (gResponse.StatusCode == HttpStatusCode.OK)
            {
                //get all the cookies from the current request and add them to the response object cookies
                gResponse.Cookies = gRequest.CookieContainer.GetCookies(gRequest.RequestUri);
                setExpect100Continue();
                //check if response object has any cookies or not
                if (gResponse.Cookies.Count > 0)
                {
                    //check if this is the first request/response, if this is the response of first request gCookies
                    //will be null
                    if (this.gCookies == null)
                    {
                        gCookies = gResponse.Cookies;
                    }
                    else
                    {
                        foreach (Cookie oRespCookie in gResponse.Cookies)
                        {
                            bool bMatch = false;
                            foreach (Cookie oReqCookie in this.gCookies)
                            {
                                if (oReqCookie.Name == oRespCookie.Name)
                                {
                                    oReqCookie.Value = oRespCookie.Value;
                                    bMatch = true;
                                    break; // 
                                }
                            }
                            if (!bMatch)
                                this.gCookies.Add(oRespCookie);
                        }
                    }
                }

                StreamReader reader = new StreamReader(gResponse.GetResponseStream());
                string responseString = reader.ReadToEnd();
                reader.Close();
                //Console.Write("Response String:" + responseString);
                return responseString;
            }
            else
            {
                return "Error";
            }
        } 
        #endregion

        #region HttpUploadFileBackground
        public string HttpUploadFileBackground(string url, string file, string paramName, string contentType, NameValueCollection nvc, bool IsLocalFile, ref string status)
        {
            string responseString = string.Empty;
            //ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            ////log.Debug(string.Format("Uploading {0} to {1}", file, url));
            //string boundary = "---------------------------" + DateTime.Now.Ticks.ToString();
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString();
            //string boundary = "---------------------------" + DateTime.Now.Ticks.ToString();
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            gRequest = (HttpWebRequest)WebRequest.Create("http://www.linkedin.com/mupld/upload");

            //gRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:12.0) Gecko/20100101 Firefox/12.0";
            //gRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            //gRequest.Headers["Accept-Charset"] = "ISO-8859-1,utf-8;q=0.7,*;q=0.7";
            gRequest.Headers["Accept-Language"] = "en-us,en;q=0.5";

            gRequest.KeepAlive = true;

            //gRequest.AllowAutoRedirect = false;

            gRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            string tempBoundary = boundary + "\r\n";
            byte[] tempBoundarybytes = System.Text.Encoding.ASCII.GetBytes("--" + boundary + "\r\n");

            //gRequest.ContentType = "multipart/form-data; boundary=" + boundary;
            gRequest.ContentType = "multipart/form-data; boundary=" + tempBoundary;
            gRequest.Method = "POST";
            gRequest.Credentials = System.Net.CredentialCache.DefaultCredentials;

            gRequest.Referer = "http://www.linkedin.com/mupld/upload";

            //ChangeProxy(proxyAddress, port, proxyUsername, proxyPassword);

            ChangeProxy(proxyAddress, port, proxyUsername, proxyPassword);

            gRequest.CookieContainer = new CookieContainer(); //gCookiesContainer;

            #region CookieManagment

            if (this.gCookies != null && this.gCookies.Count > 0)
            {
                gRequest.CookieContainer.Add(gCookies);

                //gRequest.CookieContainer.Add(new Cookie("__utma", "43838368.370306257.1336542481.1336542481.1336542481.1", "/", "twitter.com"));
                //gRequest.CookieContainer.Add(new Cookie("__utmb", "43838368.25.10.1336542481", "/", "twitter.com"));
                //gRequest.CookieContainer.Add(new Cookie("__utmc", "43838368", "/", "twitter.com"));
                //gRequest.CookieContainer.Add(new Cookie("__utmz", "43838368.1336542481.1.1.utmcsr=(direct)|utmccn=(direct)|utmcmd=(none)", "/", "twitter.com"));
            }
            #endregion

            Stream rs = gRequest.GetRequestStream();

            string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
            int temp = 0;
            foreach (string key in nvc.Keys)
            {
                if (temp == 0)
                {
                    rs.Write(tempBoundarybytes, 0, tempBoundarybytes.Length);
                    temp++;
                }
                else
                {
                    rs.Write(boundarybytes, 0, boundarybytes.Length);
                }
                string formitem = string.Format(formdataTemplate, key, nvc[key]);
                byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                rs.Write(formitembytes, 0, formitembytes.Length);
            }
            rs.Write(boundarybytes, 0, boundarybytes.Length);


            if (IsLocalFile)
            {
                string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"" + "Logo.png" + "\"\r\nContent-Type: image/png {2}\r\n\r\n";
                string header = string.Format(headerTemplate, "file", file, "image/png");
                byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
                rs.Write(headerbytes, 0, headerbytes.Length);

                if (!string.IsNullOrEmpty(file))
                {
                    FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
                    byte[] buffer = new byte[4096];
                    int bytesRead = 0;
                    while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        rs.Write(buffer, 0, bytesRead);
                    }
                    fileStream.Close();
                }
            }

            byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            rs.Write(trailer, 0, trailer.Length);
            rs.Close();

            #region CookieManagment

            if (this.gCookies != null && this.gCookies.Count > 0)
            {
                gRequest.CookieContainer.Add(gCookies);
            }

            #endregion

            WebResponse wresp = null;
            try
            {
                wresp = gRequest.GetResponse();
                gResponse = (HttpWebResponse)gRequest.GetResponse();
                StreamReader reader = new StreamReader(gResponse.GetResponseStream());
                responseString = reader.ReadToEnd();
                reader.Close();
                status = "okay";
                return responseString;
            }
            catch (Exception ex)
            {
                //log.Error("Error uploading file", ex);
                if (wresp != null)
                {
                    wresp.Close();
                    wresp = null;
                }
                return null;
            }
            //finally
            //{
            //    gRequest = null;
            //}
            //}


        } 
        #endregion

        #region HttpUploadFileBackground1
        public string HttpUploadFileBackground1(string url, string file, string paramName, string contentType, NameValueCollection nvc, bool IsLocalFile, ref string status)
        {
            string responseString = string.Empty;
            //ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            ////log.Debug(string.Format("Uploading {0} to {1}", file, url));
            //string boundary = "---------------------------" + DateTime.Now.Ticks.ToString();
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString();
            //string boundary = "---------------------------" + DateTime.Now.Ticks.ToString();
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            gRequest = (HttpWebRequest)WebRequest.Create("http://www.linkedin.com/mupld/upload");

            //gRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:12.0) Gecko/20100101 Firefox/12.0";
            //gRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            //gRequest.Headers["Accept-Charset"] = "ISO-8859-1,utf-8;q=0.7,*;q=0.7";
            gRequest.Headers["Accept-Language"] = "en-us,en;q=0.5";

            gRequest.KeepAlive = true;

            //gRequest.AllowAutoRedirect = false;

            gRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            string tempBoundary = boundary + "\r\n";
            byte[] tempBoundarybytes = System.Text.Encoding.ASCII.GetBytes("--" + boundary + "\r\n");

            //gRequest.ContentType = "multipart/form-data; boundary=" + boundary;
            gRequest.ContentType = "multipart/form-data; boundary=" + tempBoundary;
            gRequest.Method = "POST";
            gRequest.Credentials = System.Net.CredentialCache.DefaultCredentials;

            gRequest.Referer = "http://www.linkedin.com/mupld/upload";

            //ChangeProxy(proxyAddress, port, proxyUsername, proxyPassword);

            ChangeProxy(proxyAddress, port, proxyUsername, proxyPassword);

            gRequest.CookieContainer = new CookieContainer(); //gCookiesContainer;

            #region CookieManagment

            if (this.gCookies != null && this.gCookies.Count > 0)
            {
                gRequest.CookieContainer.Add(gCookies);

                //gRequest.CookieContainer.Add(new Cookie("__utma", "43838368.370306257.1336542481.1336542481.1336542481.1", "/", "twitter.com"));
                //gRequest.CookieContainer.Add(new Cookie("__utmb", "43838368.25.10.1336542481", "/", "twitter.com"));
                //gRequest.CookieContainer.Add(new Cookie("__utmc", "43838368", "/", "twitter.com"));
                //gRequest.CookieContainer.Add(new Cookie("__utmz", "43838368.1336542481.1.1.utmcsr=(direct)|utmccn=(direct)|utmcmd=(none)", "/", "twitter.com"));
            }
            #endregion

            Stream rs = gRequest.GetRequestStream();

            string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
            int temp = 0;
            foreach (string key in nvc.Keys)
            {
                if (temp == 0)
                {
                    rs.Write(tempBoundarybytes, 0, tempBoundarybytes.Length);
                    temp++;
                }
                else
                {
                    rs.Write(boundarybytes, 0, boundarybytes.Length);
                }
                string formitem = string.Format(formdataTemplate, key, nvc[key]);
                byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                rs.Write(formitembytes, 0, formitembytes.Length);
            }
            rs.Write(boundarybytes, 0, boundarybytes.Length);

            if (IsLocalFile)
            {
                string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"" + "Logo.png" + "\"\r\nContent-Type: image/png {2}\r\n\r\n";
                // by san string header = string.Format(headerTemplate, "file", file, "image/png");
                string header = string.Format(headerTemplate, "file", file, "image/jpeg");

                byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
                rs.Write(headerbytes, 0, headerbytes.Length);

                if (!string.IsNullOrEmpty(file))
                {
                    FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
                    byte[] buffer = new byte[4096];
                    int bytesRead = 0;
                    while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        rs.Write(buffer, 0, bytesRead);
                    }
                    fileStream.Close();
                }
            }

            byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            rs.Write(trailer, 0, trailer.Length);
            rs.Close();

            #region CookieManagment

            if (this.gCookies != null && this.gCookies.Count > 0)
            {
                gRequest.CookieContainer.Add(gCookies);
            }

            #endregion

            WebResponse wresp = null;
            try
            {
                wresp = gRequest.GetResponse();
                gResponse = (HttpWebResponse)gRequest.GetResponse();
                StreamReader reader = new StreamReader(gResponse.GetResponseStream());
                responseString = reader.ReadToEnd();
                reader.Close();
                status = "okay";
                return responseString;
            }
            catch (Exception ex)
            {
                //log.Error("Error uploading file", ex);
                if (wresp != null)
                {
                    wresp.Close();
                    wresp = null;
                }
                return null;
            }
            //finally
            //{
            //    gRequest = null;
            //}
            //}


        } 
        #endregion

        #region postFormDataRefWithoutAjax
        public string postFormDataRefWithoutAjax(Uri formActionUrl, string postData, string Referes, string Token, string AccountUserAgent)
        {
            gRequest = (HttpWebRequest)WebRequest.Create(formActionUrl);

            gRequest.Accept = Accept;
            gRequest.UserAgent = UserAgent;

            if (!string.IsNullOrEmpty(AccountUserAgent))
            {
                gRequest.UserAgent = AccountUserAgent;
            }
            else
            {
                gRequest.UserAgent = UserAgent;
            }


            //gRequest.UserAgent = "User-Agent: Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.16) Gecko/20110319 Firefox/3.6.16";

            gRequest.CookieContainer = new CookieContainer();// gCookiesContainer;
            gRequest.Method = "POST";

            gRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            gRequest.KeepAlive = true;
            gRequest.ContentType = @"application/x-www-form-urlencoded";
            gRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            //gRequest.Headers.Add("Accept-Encoding", "gzip");

            if (!string.IsNullOrEmpty(Referes))
            {
                gRequest.Referer = Referes;
            }
            if (!string.IsNullOrEmpty(Token))
            {
                gRequest.Headers.Add("X-CSRFToken", Token);
                gRequest.Headers.Add("X-Requested-With", "XMLHttpRequest");
            }

            //gRequest.Headers.Add("X-IsAJAXForm", "1");
            //gRequest.Headers.Add("X-Requested-With", "XMLHttpRequest");

            ///Modified BySumit 18-11-2011
            ChangeProxy(proxyAddress, port, proxyUsername, proxyPassword);

            //#region CookieManagement
            //if (this.gCookies != null && this.gCookies.Count > 0)
            //{
            //    setExpect100Continue();
            //    gRequest.CookieContainer.Add(gCookies);
            //}

              gRequest.CookieContainer = new CookieContainer(); //gCookiesContainer;

            #region CookieManagment

            if (this.gCookies != null && this.gCookies.Count > 0)
            {
               
                 setExpect100Continue();
                 gRequest.CookieContainer.Add(gCookies);

                 gRequest.CookieContainer.Add(formActionUrl, new Cookie("__qca", "P0 - 2078004405 - 1321685323158", "/"));
                 gRequest.CookieContainer.Add(formActionUrl, new Cookie("__utma", "101828306.1814567160.1321685324.1322116799.1322206824.9", "/"));
                 gRequest.CookieContainer.Add(formActionUrl, new Cookie("__utmz", "101828306.1321685324.1.1.utmcsr=(direct)|utmccn=(direct)|utmcmd=(none)", "/"));
                 gRequest.CookieContainer.Add(formActionUrl, new Cookie("__utmb", "101828306.2.10.1321858563", "/"));
                 gRequest.CookieContainer.Add(formActionUrl, new Cookie("__utmc", "101828306", "/"));
            }

            //Stream rs = gRequest.GetRequestStream();

         
            //logic to postdata to the form
            try
            {
                setExpect100Continue();
                string postdata = string.Format(postData);
                byte[] postBuffer = System.Text.Encoding.GetEncoding(1252).GetBytes(postData);
                gRequest.ContentLength = postBuffer.Length;
                Stream postDataStream = gRequest.GetRequestStream();
                postDataStream.Write(postBuffer, 0, postBuffer.Length);
                postDataStream.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // Logger.LogText("Internet Connectivity Exception : "+ ex.Message + "StackTrace --> >>>" + ex.StackTrace,null);
            }
            //post data logic ends

            //Get Response for this request url
            try
            {
                gResponse = (HttpWebResponse)gRequest.GetResponse();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //Logger.LogText("Response from "+formActionUrl + ":" + ex.Message + "StackTrace --> >>>" + ex.StackTrace,null);
            }



            //check if the status code is http 200 or http ok

            if (gResponse.StatusCode == HttpStatusCode.OK)
            {
                //get all the cookies from the current request and add them to the response object cookies
                setExpect100Continue();
                gResponse.Cookies = gRequest.CookieContainer.GetCookies(gRequest.RequestUri);
                //check if response object has any cookies or not
                //Added by sandeep pathak
                //gCookiesContainer = gRequest.CookieContainer;  

                if (gResponse.Cookies.Count > 0)
                {
                    //check if this is the first request/response, if this is the response of first request gCookies
                    //will be null
                    if (this.gCookies == null)
                    {
                        gCookies = gResponse.Cookies;
                    }
                    else
                    {
                        foreach (Cookie oRespCookie in gResponse.Cookies)
                        {
                            bool bMatch = false;
                            foreach (Cookie oReqCookie in this.gCookies)
                            {
                                if (oReqCookie.Name == oRespCookie.Name)
                                {
                                    oReqCookie.Value = oRespCookie.Value;
                                    bMatch = true;
                                    break; // 
                                }
                            }
                            if (!bMatch)
                                this.gCookies.Add(oRespCookie);
                        }
                    }
                }
            #endregion

                StreamReader reader = new StreamReader(gResponse.GetResponseStream());
                string responseString = reader.ReadToEnd();
                reader.Close();
                //Console.Write("Response String:" + responseString);
                return responseString;
            }
            else
            {
                return "Error in posting data";
            }

        } 
        #endregion

        #region postFormDataRef
        public string postFormDataRef(Uri formActionUrl, string postData, string Referes, string Token, string AccountUserAgent)
        {
            gRequest = (HttpWebRequest)WebRequest.Create(formActionUrl);

            gRequest.Accept = Accept;
            gRequest.UserAgent = UserAgent;

            if (!string.IsNullOrEmpty(AccountUserAgent))
            {
                gRequest.UserAgent = AccountUserAgent;
            }
            else
            {
                gRequest.UserAgent = UserAgent;
            }


            //gRequest.UserAgent = "User-Agent: Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.16) Gecko/20110319 Firefox/3.6.16";

            gRequest.CookieContainer = new CookieContainer();// gCookiesContainer;
            gRequest.Method = "POST";

            gRequest.Accept = "application/json,text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8, */*";
            gRequest.KeepAlive = true;
            gRequest.ContentType = @"application/x-www-form-urlencoded";
            gRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            //gRequest.Headers.Add("Accept-Encoding", "gzip");

            if (!string.IsNullOrEmpty(Referes))
            {
                gRequest.Referer = Referes;
            }
            if (!string.IsNullOrEmpty(Token))
            {

                gRequest.Headers.Add("X-CSRFToken", Token);
                gRequest.Headers.Add("X-Requested-With", "XMLHttpRequest");
            }

            //gRequest.Headers.Add("X-IsAJAXForm", "1");
            //gRequest.Headers.Add("X-Requested-With", "XMLHttpRequest");

            ///Modified BySumit 18-11-2011
            ChangeProxy(proxyAddress, port, proxyUsername, proxyPassword);

            #region CookieManagement
            if (this.gCookies != null && this.gCookies.Count > 0)
            {
                setExpect100Continue();
                gRequest.CookieContainer.Add(gCookies);
            }

            //logic to postdata to the form
            try
            {
                setExpect100Continue();
                string postdata = string.Format(postData);
                byte[] postBuffer = System.Text.Encoding.GetEncoding(1252).GetBytes(postData);
                gRequest.ContentLength = postBuffer.Length;
                Stream postDataStream = gRequest.GetRequestStream();
                postDataStream.Write(postBuffer, 0, postBuffer.Length);
                postDataStream.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // Logger.LogText("Internet Connectivity Exception : "+ ex.Message + "StackTrace --> >>>" + ex.StackTrace,null);
            }
            //post data logic ends

            //Get Response for this request url
            try
            {
                gResponse = (HttpWebResponse)gRequest.GetResponse();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //Logger.LogText("Response from "+formActionUrl + ":" + ex.Message + "StackTrace --> >>>" + ex.StackTrace,null);
            }



            //check if the status code is http 200 or http ok

            if (gResponse.StatusCode == HttpStatusCode.OK)
            {
                //get all the cookies from the current request and add them to the response object cookies
                setExpect100Continue();
                gResponse.Cookies = gRequest.CookieContainer.GetCookies(gRequest.RequestUri);
                //check if response object has any cookies or not
                //Added by sandeep pathak
                //gCookiesContainer = gRequest.CookieContainer;  

                if (gResponse.Cookies.Count > 0)
                {
                    //check if this is the first request/response, if this is the response of first request gCookies
                    //will be null
                    if (this.gCookies == null)
                    {
                        gCookies = gResponse.Cookies;
                    }
                    else
                    {
                        foreach (Cookie oRespCookie in gResponse.Cookies)
                        {
                            bool bMatch = false;
                            foreach (Cookie oReqCookie in this.gCookies)
                            {
                                if (oReqCookie.Name == oRespCookie.Name)
                                {
                                    oReqCookie.Value = oRespCookie.Value;
                                    bMatch = true;
                                    break; // 
                                }
                            }
                            if (!bMatch)
                                this.gCookies.Add(oRespCookie);
                        }
                    }
                }
            #endregion

                StreamReader reader = new StreamReader(gResponse.GetResponseStream());
                string responseString = reader.ReadToEnd();
                reader.Close();
                //Console.Write("Response String:" + responseString);
                return responseString;
            }
            else
            {
                return "Error in posting data";
            }

        } 
        #endregion

        #region HttpUploadFile
        public void HttpUploadFile(string url, string file, string paramName, string contentType, NameValueCollection nvc, string proxyAddress, int proxyPort, string proxyUsername, string proxyPassword)
        {

            ////log.Debug(string.Format("Uploading {0} to {1}", file, url));
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            //string boundary = "---------------------------" + DateTime.Now.Ticks.ToString();
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            gRequest = (HttpWebRequest)WebRequest.Create(url);
            gRequest.Accept = Accept;
            gRequest.UserAgent = UserAgent;
            gRequest.ContentType = "multipart/form-data; boundary=" + boundary;
            gRequest.Method = "POST";
            gRequest.KeepAlive = true;
            gRequest.Credentials = System.Net.CredentialCache.DefaultCredentials;

            ChangeProxy(proxyAddress, proxyPort, proxyUsername, proxyPassword);

            gRequest.CookieContainer = new CookieContainer(); //gCookiesContainer;

            #region CookieManagment

            if (this.gCookies != null && this.gCookies.Count > 0)
            {
                gRequest.CookieContainer.Add(gCookies);
            }
            #endregion

            Stream rs = gRequest.GetRequestStream();

            string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
            foreach (string key in nvc.Keys)
            {
                rs.Write(boundarybytes, 0, boundarybytes.Length);
                string formitem = string.Format(formdataTemplate, key, nvc[key]);
                byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                rs.Write(formitembytes, 0, formitembytes.Length);
            }
            rs.Write(boundarybytes, 0, boundarybytes.Length);

            string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
            string header = string.Format(headerTemplate, paramName, file, contentType);
            byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
            rs.Write(headerbytes, 0, headerbytes.Length);

            FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[4096];
            int bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                rs.Write(buffer, 0, bytesRead);
            }
            fileStream.Close();

            byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            rs.Write(trailer, 0, trailer.Length);
            rs.Close();

            #region CookieManagment

            if (this.gCookies != null && this.gCookies.Count > 0)
            {
                gRequest.CookieContainer.Add(gCookies);
            }

            #endregion

            WebResponse wresp = null;
            try
            {
                wresp = gRequest.GetResponse();
                Stream stream2 = wresp.GetResponseStream();
                StreamReader reader2 = new StreamReader(stream2);
                //log.Debug(string.Format("File uploaded, server response is: {0}", reader2.ReadToEnd()));
            }
            catch (Exception ex)
            {
                //log.Error("Error uploading file", ex);
                if (wresp != null)
                {
                    wresp.Close();
                    wresp = null;
                }
            }
            finally
            {
                gRequest = null;
            }
            //}

        } 
        #endregion

        #region MultiPartImageUpload
        public void MultiPartImageUpload(string Username, string Password, string localImagePath)
        {
            gRequest.Accept = Accept;
            gRequest.UserAgent = UserAgent;
            ///Login to FB

            //string valueLSD = "name=" + "\"lsd\"";
            //string pageSource = getHtmlfromUrl(new Uri("https://www.facebook.com/login.php"));
            ////string pageSource = getHtmlfromUrlProxy(new Uri("https://www.facebook.com/login.php"));
            //int startIndex = pageSource.IndexOf(valueLSD) + 18;
            //string value = pageSource.Substring(startIndex, 5);

            //string ResponseLogin = postFormData(new Uri("https://www.facebook.com/login.php?login_attempt=1"), "charset_test=%E2%82%AC%2C%C2%B4%2C%E2%82%AC%2C%C2%B4%2C%E6%B0%B4%2C%D0%94%2C%D0%84&lsd=" + value + "&locale=en_US&email=" + Username.Split('@')[0] + "%40" + Username.Split('@')[1] + "&pass=" + Password + "&persistent=1&default_persistent=1&charset_test=%E2%82%AC%2C%C2%B4%2C%E2%82%AC%2C%C2%B4%2C%E6%B0%B4%2C%D0%94%2C%D0%84&lsd=" + value + "");


            //string profileSource = getHtmlfromUrl(new Uri("http://www.facebook.com/editprofile.php?sk=basic"));

            //string valuepost_form_id = "name=" + "\"post_form_id\"";
            //string valuefb_dtsg = "name=" + "\"fb_dtsg\"";
            /////Setting Post Data Params...
            //int sIndex = profileSource.IndexOf(valuepost_form_id) + 27;
            //string post_form_id = profileSource.Substring(sIndex, 32);

            //int s1Index = profileSource.IndexOf(valuefb_dtsg) + 22;
            //string fb_dtsg = profileSource.Substring(s1Index, 8);

            //int s2Index = profileSource.IndexOf("user") + 5;
            //string userId = profileSource.Substring(s2Index, 15);

            //if (ResponseLogin.Contains("http://www.facebook.com/profile.php?id="))
            //{
            //    string[] arrUser = System.Text.RegularExpressions.Regex.Split(ResponseLogin, "href");
            //    foreach (String itemUser in arrUser)
            //    {
            //        if (!itemUser.Contains("<!DOCTYPE"))
            //        {
            //            if (itemUser.Contains("http://www.facebook.com/profile.php?id="))
            //            {

            //                string[] arrhre = itemUser.Split('"');
            //                userId = arrhre[1].Replace("http://www.facebook.com/profile.php?id=", "");
            //                if (userId.Contains("&"))
            //                {
            //                    string[] Arr = userId.Split('&');
            //                    userId = Arr[0];
            //                }

            //                break;

            //            }
            //        }
            //    }
            //}

            //NameValueCollection nvc = new NameValueCollection();
            //nvc.Add("post_form_id", post_form_id);
            //nvc.Add("fb_dtsg", fb_dtsg);
            //nvc.Add("id", userId);
            //nvc.Add("type", "profile");
            //nvc.Add("return", "/ajax/profile/picture/upload_iframe.php?pic_type=1&id=" + userId);

            ////UploadFilesToRemoteUrl("http://upload.facebook.com/pic_upload.php ", new string[] { @"C:\Users\Globus-n2\Desktop\Windows Photo Viewer Wallpaper.jpg" }, "", nvc);
            ////HttpUploadFile("http://upload.facebook.com/pic_upload.php ", localImagePath, "file", "image/jpeg", nvc, proxyAddress, );
        } 
        #endregion

        #region MultiPartImageUpload
        public void MultiPartImageUpload(string Username, string Password, string localImagePath, string proxyAddress, string proxyPort, string proxyUsername, string proxyPassword)
        {
            gRequest.Accept = Accept;
            gRequest.UserAgent = UserAgent;
            ///Login to FB

            //string valueLSD = "name=" + "\"lsd\"";

            //int intProxyPort = 80;

            //Regex IdCheck = new Regex("^[0-9]*$");

            //if (!string.IsNullOrEmpty(proxyPort) && IdCheck.IsMatch(proxyPort))
            //{
            //    intProxyPort = int.Parse(proxyPort);
            //}

            //string pageSource = getHtmlfromUrlProxy(new Uri("https://www.facebook.com/login.php"), proxyAddress, intProxyPort, proxyUsername, proxyPassword);
            //int startIndex = pageSource.IndexOf(valueLSD) + 18;
            //string value = pageSource.Substring(startIndex, 5);

            //string ResponseLogin = postFormData(new Uri("https://www.facebook.com/login.php?login_attempt=1"), "charset_test=%E2%82%AC%2C%C2%B4%2C%E2%82%AC%2C%C2%B4%2C%E6%B0%B4%2C%D0%94%2C%D0%84&lsd=" + value + "&locale=en_US&email=" + Username.Split('@')[0] + "%40" + Username.Split('@')[1] + "&pass=" + Password + "&persistent=1&default_persistent=1&charset_test=%E2%82%AC%2C%C2%B4%2C%E2%82%AC%2C%C2%B4%2C%E6%B0%B4%2C%D0%94%2C%D0%84&lsd=" + value + "");


            //string profileSource = getHtmlfromUrl(new Uri("http://www.facebook.com/editprofile.php?sk=basic"));

            //string valuepost_form_id = "name=" + "\"post_form_id\"";
            //string valuefb_dtsg = "name=" + "\"fb_dtsg\"";
            /////Setting Post Data Params...
            //int sIndex = profileSource.IndexOf(valuepost_form_id) + 27;
            //string post_form_id = profileSource.Substring(sIndex, 32);

            //int s1Index = profileSource.IndexOf(valuefb_dtsg) + 22;
            //string fb_dtsg = profileSource.Substring(s1Index, 8);

            //int s2Index = profileSource.IndexOf("user") + 5;
            //string userId = profileSource.Substring(s2Index, 15);

            //if (ResponseLogin.Contains("http://www.facebook.com/profile.php?id="))
            //{
            //    string[] arrUser = System.Text.RegularExpressions.Regex.Split(ResponseLogin, "href");
            //    foreach (String itemUser in arrUser)
            //    {
            //        if (!itemUser.Contains("<!DOCTYPE"))
            //        {
            //            if (itemUser.Contains("http://www.facebook.com/profile.php?id="))
            //            {

            //                string[] arrhre = itemUser.Split('"');
            //                userId = arrhre[1].Replace("http://www.facebook.com/profile.php?id=", "");
            //                if (userId.Contains("&"))
            //                {
            //                    string[] arr = userId.Split('&');
            //                    userId = arr[0];
            //                }

            //                break;

            //            }
            //        }
            //    }
            //}

            //NameValueCollection nvc = new NameValueCollection();
            //nvc.Add("post_form_id", post_form_id);
            //nvc.Add("fb_dtsg", fb_dtsg);
            //nvc.Add("id", userId);
            //nvc.Add("type", "profile");
            //nvc.Add("return", "/ajax/profile/picture/upload_iframe.php?pic_type=1&id=" + userId);

            ////UploadFilesToRemoteUrl("http://upload.facebook.com/pic_upload.php ", new string[] { @"C:\Users\Globus-n2\Desktop\Windows Photo Viewer Wallpaper.jpg" }, "", nvc);
            ////HttpUploadFile("http://upload.facebook.com/pic_upload.php ", localImagePath, "file", "image/jpeg", nvc);
            //HttpUploadFile("http://upload.facebook.com/pic_upload.php ", localImagePath, "file", "image/jpeg", nvc, proxyAddress, intProxyPort, proxyUsername, proxyPassword);
        } 
        #endregion

        #region postFormData
        public string postFormData(Uri formActionUrl, string postData)
        {
            string responseString = string.Empty;
            try
            {
                gRequest = (HttpWebRequest)WebRequest.Create(formActionUrl);
                //gRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:18.0) Gecko/20100101 Firefox/18.0";
                gRequest.Accept = Accept;
                gRequest.UserAgent = UserAgent;
                gRequest.CookieContainer = new CookieContainer();// gCookiesContainer;
                gRequest.Method = "POST";
                //gRequest.Accept = " text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8, */*";
                gRequest.KeepAlive = true;
                gRequest.ContentType = @"application/x-www-form-urlencoded";

                ///Modified BySumit 18-11-2011
                ChangeProxy(proxyAddress, port, proxyUsername, proxyPassword);

                #region CookieManagement
                if (this.gCookies != null && this.gCookies.Count > 0)
                {
                    setExpect100Continue();
                    gRequest.CookieContainer.Add(gCookies);
                }

                //logic to postdata to the form
                try
                {
                    setExpect100Continue();
                    string postdata = string.Format(postData);
                    byte[] postBuffer = System.Text.Encoding.GetEncoding(1252).GetBytes(postData);
                    gRequest.ContentLength = postBuffer.Length;
                    Stream postDataStream = gRequest.GetRequestStream();
                    postDataStream.Write(postBuffer, 0, postBuffer.Length);
                    postDataStream.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    // Logger.LogText("Internet Connectivity Exception : "+ ex.Message + "StackTrace --> >>>" + ex.StackTrace,null);
                }
                //post data logic ends

                //Get Response for this request url
                try
                {
                    gResponse = (HttpWebResponse)gRequest.GetResponse();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    //Logger.LogText("Response from "+formActionUrl + ":" + ex.Message + "StackTrace --> >>>" + ex.StackTrace,null);
                }



                //check if the status code is http 200 or http ok

                if (gResponse.StatusCode == HttpStatusCode.OK)
                {
                    //get all the cookies from the current request and add them to the response object cookies
                    setExpect100Continue();
                    gResponse.Cookies = gRequest.CookieContainer.GetCookies(gRequest.RequestUri);
                    //check if response object has any cookies or not
                    //Added by sandeep pathak
                    //gCookiesContainer = gRequest.CookieContainer;  

                    if (gResponse.Cookies.Count > 0)
                    {
                        //check if this is the first request/response, if this is the response of first request gCookies
                        //will be null
                        if (this.gCookies == null)
                        {
                            gCookies = gResponse.Cookies;
                        }
                        else
                        {
                            foreach (Cookie oRespCookie in gResponse.Cookies)
                            {
                                bool bMatch = false;
                                foreach (Cookie oReqCookie in this.gCookies)
                                {
                                    if (oReqCookie.Name == oRespCookie.Name)
                                    {
                                        oReqCookie.Value = oRespCookie.Value;
                                        bMatch = true;
                                        break; // 
                                    }
                                }
                                if (!bMatch)
                                    this.gCookies.Add(oRespCookie);
                            }
                        }
                    }
                #endregion

                    using (StreamReader reader = new StreamReader(gResponse.GetResponseStream()))
                    {
                        responseString = reader.ReadToEnd();
                    }
                    //Console.Write("Response String:" + responseString);
                    return responseString;
                }
                else
                {
                    return "Error in posting data";
                }
            }
            catch
            {
            }
            return responseString;
        } 
        #endregion

        #region postFormDataRef
        public string postFormDataRef(Uri formActionUrl, string postData, string Referes, string Token, string XRequestedWith, string XPhx, string Origin, string isAjaxForm)
        {

            gRequest = (HttpWebRequest)WebRequest.Create(formActionUrl);
            //gRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:12.0) Gecko/20100101 Firefox/12.0";
            gRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/37.0.2062.120 Safari/537.36";
            //Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/37.0.2062.120 Safari/537.36


            gRequest.CookieContainer = new CookieContainer();// gCookiesContainer;
            gRequest.Method = "POST";
            //gRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8, */*";
            gRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            gRequest.Headers["Accept-Language"] = "en-us,en;q=0.8";
            gRequest.KeepAlive = true;
            gRequest.ContentType = @"application/x-www-form-urlencoded";
            
            //gRequest.Headers.Add("Javascript-enabled", "true");

            gRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            if (!string.IsNullOrEmpty(Referes))
            {
                gRequest.Referer = Referes;
            }
            if (!string.IsNullOrEmpty(Token))
            {
                gRequest.Headers.Add("X-CSRFToken", Token);
            }
            if (!string.IsNullOrEmpty(XRequestedWith))
            {
                gRequest.Headers.Add("X-Requested-With", XRequestedWith);
            }
            if (!string.IsNullOrEmpty(XPhx))
            {
                gRequest.Headers.Add("X-PHX", XPhx);
            }
            if (!string.IsNullOrEmpty(Origin))
            {
                gRequest.Headers.Add("Origin", Origin);
            }
            if (!string.IsNullOrEmpty(isAjaxForm))
            {
                gRequest.Headers.Add("X-IsAJAXForm", isAjaxForm);
            }
            //if (!string.IsNullOrEmpty(Origin))
            //{
            //    gRequest.Headers.Add("Origin", Origin);
            //}
            //if (!string.IsNullOrEmpty(Origin))
            //{
            //    gRequest.Headers.Add("Origin", Origin);
            //}

            ///Modified BySumit 18-11-2011
            ChangeProxy(proxyAddress, port, proxyUsername, proxyPassword);

            #region CookieManagement
            if (this.gCookies != null && this.gCookies.Count > 0)
            {
                setExpect100Continue();
                gRequest.CookieContainer.Add(gCookies);
            }

            //logic to postdata to the form
            try
            {
                setExpect100Continue();
                //string postdata = string.Format(postData);
                byte[] postBuffer = System.Text.Encoding.GetEncoding(1252).GetBytes(postData);
                gRequest.ContentLength = postBuffer.Length;
                Stream postDataStream = gRequest.GetRequestStream();
                postDataStream.Write(postBuffer, 0, postBuffer.Length);
                postDataStream.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // Logger.LogText("Internet Connectivity Exception : "+ ex.Message + "StackTrace --> >>>" + ex.StackTrace,null);
            }
            //post data logic ends

            //Get Response for this request url
            try
            {
                gResponse = (HttpWebResponse)gRequest.GetResponse();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //Logger.LogText("Response from "+formActionUrl + ":" + ex.Message + "StackTrace --> >>>" + ex.StackTrace,null);
            }



            //check if the status code is http 200 or http ok

            if (gResponse.StatusCode == HttpStatusCode.OK)
            {
                //get all the cookies from the current request and add them to the response object cookies
                setExpect100Continue();
                gResponse.Cookies = gRequest.CookieContainer.GetCookies(gRequest.RequestUri);
                //check if response object has any cookies or not
                //Added by sandeep pathak
                //gCookiesContainer = gRequest.CookieContainer;  

                if (gResponse.Cookies.Count > 0)
                {
                    //check if this is the first request/response, if this is the response of first request gCookies
                    //will be null
                    if (this.gCookies == null)
                    {
                        gCookies = gResponse.Cookies;
                    }
                    else
                    {
                        foreach (Cookie oRespCookie in gResponse.Cookies)
                        {
                            bool bMatch = false;
                            foreach (Cookie oReqCookie in this.gCookies)
                            {
                                if (oReqCookie.Name == oRespCookie.Name)
                                {
                                    oReqCookie.Value = oRespCookie.Value;
                                    bMatch = true;
                                    break; // 
                                }
                            }
                            if (!bMatch)
                                this.gCookies.Add(oRespCookie);
                        }
                    }
                }
            #endregion



                StreamReader reader = new StreamReader(gResponse.GetResponseStream());
                string responseString = reader.ReadToEnd();
                reader.Close();
                //Console.Write("Response String:" + responseString);
                return responseString;
            }
            else
            {
                return "Error in posting data";
            }

        } 
        #endregion

        #region postFormDataProxy
        public string postFormDataProxy(Uri formActionUrl, string postData, string proxyAddress, int port, string proxyUsername, string proxyPassword)
        {

            gRequest = (HttpWebRequest)WebRequest.Create(formActionUrl);
            //gRequest.UserAgent = "User-Agent: Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.16) Gecko/20110319 Firefox/3.6.16";
            gRequest.Accept = Accept;
           // gRequest.Accept = gRequest.Accept.Replace(", */*", string.Empty);
            gRequest.UserAgent = UserAgent;
            gRequest.Headers["Origin"] = "https://www.linkedin.com";
            gRequest.CookieContainer = new CookieContainer();// gCookiesContainer;
            gRequest.Method = "POST";
            //gRequest.Accept = " text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8, */*";
            gRequest.KeepAlive = true;
            gRequest.ContentType = @"application/x-www-form-urlencoded";
            gRequest.Headers.Add("Javascript-enabled", "true");

            ///Modified BySumit 18-11-2011
            ChangeProxy(proxyAddress, port, proxyUsername, proxyPassword);

            #region CookieManagement
            if (this.gCookies != null && this.gCookies.Count > 0)
            {
                setExpect100Continue();
                gRequest.CookieContainer.Add(gCookies);
            }

            //logic to postdata to the form
            try
            {
                setExpect100Continue();
                string postdata = string.Format(postData);
                byte[] postBuffer = System.Text.Encoding.GetEncoding(1252).GetBytes(postData);
                gRequest.ContentLength = postBuffer.Length;
                Stream postDataStream = gRequest.GetRequestStream();
                postDataStream.Write(postBuffer, 0, postBuffer.Length);
                postDataStream.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // Logger.LogText("Internet Connectivity Exception : "+ ex.Message + "StackTrace --> >>>" + ex.StackTrace,null);
            }
            //post data logic ends

            //Get Response for this request url
            try
            {
                gResponse = (HttpWebResponse)gRequest.GetResponse();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //Logger.LogText("Response from "+formActionUrl + ":" + ex.Message + "StackTrace --> >>>" + ex.StackTrace,null);
            }



            //check if the status code is http 200 or http ok

            if (gResponse.StatusCode == HttpStatusCode.OK)
            {
                //get all the cookies from the current request and add them to the response object cookies
                setExpect100Continue();
                gResponse.Cookies = gRequest.CookieContainer.GetCookies(gRequest.RequestUri);
                //check if response object has any cookies or not
                //Added by sandeep pathak
                //gCookiesContainer = gRequest.CookieContainer;  

                if (gResponse.Cookies.Count > 0)
                {
                    //check if this is the first request/response, if this is the response of first request gCookies
                    //will be null
                    if (this.gCookies == null)
                    {
                        gCookies = gResponse.Cookies;
                    }
                    else
                    {
                        foreach (Cookie oRespCookie in gResponse.Cookies)
                        {
                            bool bMatch = false;
                            foreach (Cookie oReqCookie in this.gCookies)
                            {
                                if (oReqCookie.Name == oRespCookie.Name)
                                {
                                    oReqCookie.Value = oRespCookie.Value;
                                    bMatch = true;
                                    break; // 
                                }
                            }
                            if (!bMatch)
                                this.gCookies.Add(oRespCookie);
                        }
                    }
                }
            #endregion



                StreamReader reader = new StreamReader(gResponse.GetResponseStream());
                string responseString = reader.ReadToEnd();
                reader.Close();
                //Console.Write("Response String:" + responseString);
                return responseString;
            }
            else
            {
                return "Error in posting data";
            }

        } 
        #endregion

        #region setExpect100Continue
        public void setExpect100Continue()
        {
            if (ServicePointManager.Expect100Continue == true)
            {
                ServicePointManager.Expect100Continue = false;
            }
        } 
        #endregion

        #region setExpect100ContinueToTrue
        public void setExpect100ContinueToTrue()
        {
            if (ServicePointManager.Expect100Continue == false)
            {
                ServicePointManager.Expect100Continue = true;
            }
        } 
        #endregion

        #region ChangeProxy
        public void ChangeProxy(string proxyAddress, int port, string proxyUsername, string proxyPassword)
        {
            try
            {
                WebProxy myproxy = new WebProxy(proxyAddress, port);
                myproxy.BypassProxyOnLocal = false;

                if (!string.IsNullOrEmpty(proxyUsername) && !string.IsNullOrEmpty(proxyPassword))
                {
                    myproxy.Credentials = new NetworkCredential(proxyUsername, proxyPassword);
                }
                gRequest.Proxy = myproxy;
            }
            catch (Exception ex)
            {

            }

        } 
        #endregion

        #region GetUniqueKeyBasedValue
        public string GetUniqueKeyBasedValue(string pageSource, string uniqueKey)
        {
            string returnValue = string.Empty;
            try
            {
                string[] arr_uniqueKey = Regex.Split(pageSource, uniqueKey);

                for (int i = 0; i < arr_uniqueKey.Length; i++)
                {
                    try
                    {
                        if (i == 1)
                        {
                            try
                            {   
                                returnValue = arr_uniqueKey[i].Substring(0, arr_uniqueKey[i].IndexOf("&")).Replace("\"", string.Empty).Replace("<a href=/profile/view?id=", string.Empty).Trim();
                                break;
                            }
                            catch
                            {
                            }
                        }
                    }
                    catch
                    {
                    }
                }
            }
            catch
            {
            }
            return returnValue;
        } 
        #endregion

        #region SetProfilePic
        public bool SetProfilePic(ref GlobusHttpHelper HttpHelper, string profileId, string Username, string Password, string localImagePath, string proxyAddress, string proxyPort, string proxyUsername, string proxyPassword, ref string status, string UploadInfoData)
        {

            string FirstGetREsponse = string.Empty;
            try
            {
                FirstGetREsponse = HttpHelper.getHtmlfromUrl(new Uri("http://www.linkedin.com/profile/edit-picture-info?trk=prof-ovw-edit-photo"));
            }
            catch { }
            if (string.IsNullOrEmpty(FirstGetREsponse))
            {
                try
                {
                    FirstGetREsponse = HttpHelper.getHtmlfromUrl(new Uri("https://www.linkedin.com/profile/edit-picture-info?goback=%2Enpv_" + profileId + "_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1&trk=prof-ovw-edit-photo"));
                }
                catch { }
            }
            string[] upload_infoArr = Regex.Split(FirstGetREsponse, " name=\"upload_info");
            string upload_infow = string.Empty;
            string upload_infowithjs = string.Empty;
            if (true)
            {
                try
                {
                    upload_infow = upload_infoArr[1].Substring(upload_infoArr[1].IndexOf("value="), upload_infoArr[1].IndexOf(">") - upload_infoArr[1].IndexOf("value=")).Replace("value=", string.Empty).Replace("\"", string.Empty).Replace("\\", string.Empty).Replace("/", string.Empty).Trim();
                }
                catch { }
                try
                {
                    upload_infowithjs = upload_infoArr[2].Substring(upload_infoArr[2].IndexOf("value="), upload_infoArr[2].IndexOf(">") - upload_infoArr[2].IndexOf("value=")).Replace("value=", string.Empty).Replace("\"", string.Empty).Replace("\\", string.Empty).Replace("/", string.Empty).Trim();
                }
                catch { }
            }
            else
            {
                //try
                //{
                //    string[] upload_infoArr1 = Regex.Split(FirstGetREsponse, "id\":\"upload_info");
                //    upload_infow = upload_infoArr1[1].Substring(upload_infoArr1[1].IndexOf("value\":"), upload_infoArr1[1].IndexOf("type") - upload_infoArr1[1].IndexOf("value\":")).Replace("value\":", string.Empty).Replace("\"", string.Empty).Replace("\\", string.Empty).Replace(",", string.Empty).Trim();
                //}
                //catch { }
            }
            string posturi = string.Empty;
            if (FirstGetREsponse.Contains("name=\"uploadMemberPicture"))
            {
                try
                {
                    string[] MemberPicture = Regex.Split(FirstGetREsponse, "name=\"uploadMemberPicture");
                    posturi = MemberPicture[1].Substring(MemberPicture[1].IndexOf("action="), MemberPicture[1].IndexOf("method=") - MemberPicture[1].IndexOf("action=")).Replace("action=", string.Empty).Replace("\"", string.Empty).Replace("\\", string.Empty).Replace("/", string.Empty).Trim();
                }
                catch { }
            }

            bool isSetProfilePic = false;
            try
            {

                string upload_info = "";
                upload_info = upload_infow;

                string upload_info_with_js = "";
                upload_info_with_js = upload_infowithjs;

                //upload_info = upload_info.Replace("&dsh;", "-"); ;
                //upload_info_with_js = upload_info;

                string callback = "";
                callback = "profilePicture.processResponse";
                NameValueCollection nvc = new NameValueCollection();

                nvc.Add("upload_info", upload_info_with_js);
                nvc.Add("upload_info_with_js", upload_info_with_js);
                nvc.Add("callback", callback);
                //nvc.Add("Content-Type:", "image/jpeg");
                posturi = posturi.Replace("mupld", string.Empty);
                //string response = HttpHelper.HttpUploadProfilePic(ref HttpHelper, profileId, "http://www.linkedin.com/mupld/" + posturi, "pic", "image/jpeg", localImagePath, nvc, proxyAddress, Convert.ToInt32(proxyPort), proxyUsername, proxyPassword);
                string response = string.Empty;
                try
                {
                    response = HttpHelper.HttpUploadProfilePic(ref HttpHelper, profileId, "http://www.linkedin.com/mupld/upload", "pic", "image/jpeg", localImagePath, nvc, proxyAddress, Convert.ToInt32(proxyPort), proxyUsername, proxyPassword);
                }
                catch { }
                if (string.IsNullOrEmpty(response))
                {
                    response = HttpHelper.HttpUploadProfilePic(ref HttpHelper, profileId, "https://www.linkedin.com/mupld/upload?goback=%2Enpv_" + profileId + "_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1", "pic", "image/jpeg", localImagePath, nvc, proxyAddress, Convert.ToInt32(proxyPort), proxyUsername, proxyPassword);
                    //http://www.linkedin.com/mupld/process?filter=car450&return_type=html&mid=%2Fp/6/005/025/1b7/34bf4a0.jpg&callback=profilePicture.savePhoto&filters_scale_h=397&filters_scale_w=600&filters_crop_x=101&filters_crop_y=0&filters_crop_h=396.99966&filters_crop_w=396.99966&filters_rotate_t=
                }
                string Images1 = string.Empty;
                if (response.Contains("SUCCESS"))
                {
                    try
                    {
                        Images1 = response.Substring(response.IndexOf("value\":"), response.IndexOf("}") - response.IndexOf("value\":")).Replace("value\":", string.Empty).Replace("\"", string.Empty).Replace("\\", string.Empty).Trim();
                        Images1 = Images1.Substring(1, (Images1.Length - 1));
                        Images1 = Images1.Remove(Images1.IndexOf(","));
                        Images1 = "/" + Images1;
                    }
                    catch { }
                }
                string csrfToken = string.Empty;
                string sourceAlias = string.Empty;
                if (FirstGetREsponse.Contains("csrfToken"))
                {
                    try
                    {
                        int startindex = FirstGetREsponse.IndexOf("csrfToken");
                        if (startindex > 0)
                        {
                            string start = FirstGetREsponse.Substring(startindex);
                            int endindex = start.IndexOf("\">");
                            string end = start.Substring(0, endindex);
                            csrfToken = end.Replace("csrfToken=", "").Replace("\\", "");
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
                            csrfToken = FirstGetREsponse.Substring(FirstGetREsponse.IndexOf("csrfToken"), 100);
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
                if (FirstGetREsponse.Contains("sourceAlias"))
                {
                    try
                    {
                        sourceAlias = FirstGetREsponse.Substring(FirstGetREsponse.IndexOf("sourceAlias"), 100);
                        string[] Arr = sourceAlias.Split('"');
                        sourceAlias = Arr[2].Replace("\\", string.Empty);
                    }
                    catch { }
                }
                if (string.IsNullOrEmpty(sourceAlias))
                {
                    if (FirstGetREsponse.Contains("sourceAlias"))
                    {
                        try
                        {
                            sourceAlias = FirstGetREsponse.Substring(FirstGetREsponse.IndexOf("sourceAlias"), 100);
                            string[] Arr = sourceAlias.Split('"');
                            sourceAlias = Arr[2];
                        }
                        catch { }
                    }

                }
                string Images = string.Empty;
                string FirstGet = string.Empty;
                string setMaskWidth = string.Empty;
                string setMaskHeight = string.Empty;
                string setMaskTop = string.Empty;
                string setMaskLeft = string.Empty;
                string setCropFilter = string.Empty;


                if (FirstGetREsponse.Contains("profilePicture.setMaskWidth"))
                {
                    try
                    {
                        string[] setMaskWidthArr = Regex.Split(FirstGetREsponse, "profilePicture.setMaskWidth");
                        Match match = Regex.Match(setMaskWidthArr[1], @"\((.*?)\)");
                        if (match.Groups.Count > 1)
                        {
                            setMaskWidth = match.Groups[1].Value;
                        }
                    }
                    catch { }
                }
                if (FirstGetREsponse.Contains("profilePicture.setMaskHeight"))
                {
                    try
                    {
                        string[] setMaskHeightArr = Regex.Split(FirstGetREsponse, "profilePicture.setMaskHeight");
                        Match match = Regex.Match(setMaskHeightArr[1], @"\((.*?)\)");
                        if (match.Groups.Count > 1)
                        {
                            setMaskHeight = match.Groups[1].Value;
                        }
                    }
                    catch { }
                }

                if (FirstGetREsponse.Contains("profilePicture.setMaskTop"))
                {
                    try
                    {
                        string[] setMaskTopArr = Regex.Split(FirstGetREsponse, "profilePicture.setMaskTop");
                        Match match = Regex.Match(setMaskTopArr[1], @"\((.*?)\)");
                        if (match.Groups.Count > 1)
                        {
                            setMaskTop = match.Groups[1].Value;
                        }
                    }
                    catch { }
                }
                if (FirstGetREsponse.Contains("profilePicture.setMaskLeft"))
                {
                    try
                    {
                        string[] setMaskLeftArr = Regex.Split(FirstGetREsponse, "profilePicture.setMaskLeft");
                        Match match = Regex.Match(setMaskLeftArr[1], @"\((.*?)\)");
                        if (match.Groups.Count > 1)
                        {
                            setMaskLeft = match.Groups[1].Value;
                        }
                    }
                    catch { }
                }
                if (FirstGetREsponse.Contains("profilePicture.setCropFilter"))
                {
                    try
                    {
                        string[] setCropFilterArr = Regex.Split(FirstGetREsponse, "profilePicture.setCropFilter");
                        Match match = Regex.Match(setCropFilterArr[1], @"\((.*?)\)");
                        if (match.Groups.Count > 1)
                        {
                            setCropFilter = match.Groups[1].Value;
                        }
                    }
                    catch { }
                }
                //profilePicture.setCropFilter



                try
                {
                    // string FirstGet = HttpHelper.getHtmlfromUrl(new Uri("http://www.linkedin.com/mupld/process?goback=%2Enpv_" + profileId + "_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1&filter=car80&return_type=html&mid=%2Fp"+Images1+"&callback=profilePicture.savePhoto&filters_scale_h=80&filters_scale_w=335&filters_crop_x=0&filters_crop_y=0&filters_crop_h=335&filters_crop_w=335&filters_rotate_t="));
                    // string FirstGet = HttpHelper.getHtmlfromUrl(new Uri("https://www.linkedin.com/mupld/process?goback=%2Enpv_" + profileId + "_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1&filter=car80&return_type=html&mid=%2F" + Images1 + "&callback=profilePicture.savePhoto&filters_scale_h=80&filters_scale_w=346&filters_crop_x=0&filters_crop_y=0&filters_crop_h=346&filters_crop_w=346&filters_rotate_t="));

                    #region Setting Image Parameters

                    float w = 200, h = 200;
                    try
                    {
                        //GetJpegDimension(localImagePath, out h, out w);
                        System.Drawing.Size sz = ImageHelper.GetDimensions(localImagePath);

                        w = sz.Width;
                        h = sz.Height;
                    }
                    catch { }

                    float filters_scale_h = RandomNumberGenerator.GenerateRandom(187, 187);
                    float filters_scale_w = RandomNumberGenerator.GenerateRandom(187, 187);

                    int filters_crop_h = RandomNumberGenerator.GenerateRandom(187, 187);
                    int filters_crop_w = filters_crop_h;

                    if (w >= 3000)
                    {
                        w = 3000;
                    }
                    if (h >= 800)
                    {
                        h = 800;
                    }

                    float heightwidthratio = h / w;

                    filters_scale_w = w;
                    filters_scale_h = w * heightwidthratio;

                    if (filters_scale_w < filters_scale_h)
                    {
                        filters_crop_w = (int)filters_scale_w;
                    }
                    else
                    {
                        filters_crop_w = (int)filters_scale_h - 5;
                    }
                    if (filters_crop_w > 350)
                    {
                        filters_crop_w = RandomNumberGenerator.GenerateRandom(187,187);
                    }

                    #endregion


                    //if (string.IsNullOrEmpty(setMaskHeight))
                    {
                        try
                        {
                           FirstGet = HttpHelper.getHtmlfromUrl(new Uri("http://www.linkedin.com/mupld/process?filter=" + "car450" + "&return_type=html&mid=" + Images1 + "&callback=profilePicture.savePhoto&filters_scale_h=" + (int)filters_scale_h + "&filters_scale_w=" + (int)filters_scale_w + "&filters_crop_x=0&filters_crop_y=0&filters_crop_h=" + filters_scale_h + "&filters_crop_w=" + filters_scale_h + "&filters_rotate_t="));//filters_crop_h=135&filters_crop_w=135&filters_rotate_t="));

                            if (string.IsNullOrEmpty(FirstGet))
                            {
                                System.Threading.Thread.Sleep(500);
                                FirstGet = HttpHelper.getHtmlfromUrl(new Uri("http://www.linkedin.com/mupld/process?filter=" + "car450" + "&return_type=html&mid=" + Images1 + "&callback=profilePicture.savePhoto&filters_scale_h=" + (int)filters_scale_h + "&filters_scale_w=" + (int)filters_scale_w + "&filters_crop_x=0&filters_crop_y=0&filters_crop_h=" + filters_crop_w + "&filters_crop_w=" + filters_crop_w + "&filters_rotate_t="));//filters_crop_h=135&filters_crop_w=135&filters_rotate_t="));
                            }
                            //FirstGet = HttpHelper.getHtmlfromUrl(new Uri("http://www.linkedin.com/mupld/process?filter=" + "car450" + "&return_type=html&mid=" + Images1 + "&callback=profilePicture.savePhoto&filters_scale_h=" + 180 + "&filters_scale_w=" + 180 + "&filters_crop_x=0&filters_crop_y=0&filters_crop_h=" + 145 + "&filters_crop_w=" + 145 + "&filters_rotate_t="));//filters_crop_h=135&filters_crop_w=135&filters_rotate_t="));

                            string mid = "";

                            if (FirstGet.Contains("SUCCESS"))
                            {

                                try
                                {
                                    mid = FirstGet.Substring(FirstGet.IndexOf("value\":"), FirstGet.IndexOf("}") - FirstGet.IndexOf("value\":")).Replace("value\":", string.Empty).Replace("\"", string.Empty).Replace("\\", string.Empty).Trim();
                                    mid = mid.Substring(1, (mid.Length - 1));
                                    //mid = mid.Remove(mid.IndexOf(","));
                                    mid = "/" + mid;
                                    // Images = Uri.EscapeDataString(Images);
                                }
                                catch { }
                            }

                            string gup_savecroppic = HttpHelper.getHtmlfromUrl(new Uri("http://www.linkedin.com/profile/save-crop-picture?csrfToken=" + csrfToken + "&masterTempID=" + Images1 + "&croppedTempID=" + mid + "&xParam=0&yParam=0&xSizeParam=" + filters_crop_w + "&ySizeParam=" + filters_crop_w + "&nsave=n"));//xSizeParam=292&ySizeParam=292&nsave=n"));
                            //string gup_savecroppic = HttpHelper.getHtmlfromUrl(new Uri("http://www.linkedin.com/profile/save-crop-picture?csrfToken=" + csrfToken + "&masterTempID=" + Images1 + "&croppedTempID=" + mid + "&xParam=0&yParam=0&xSizeParam=" + 135 + "&ySizeParam=" + 135 + "&nsave=n"));//xSizeParam=292&ySizeParam=292&nsave=n"));

                            //http://www.linkedin.com/profile/edit-picture-info?trk=nprofile-save-picture-submit&report%2Esuccess=9dCbijuoBwCwpKulrwnkugn6gmZHePJOoeK5jlW7JYSHYSSp-w55jdor_TR6umL
                        }
                        catch { }

                        #region Old code
                        //if (string.IsNullOrEmpty(FirstGet))
                        //{
                        //    try
                        //    {
                        //        FirstGet = HttpHelper.getHtmlfromUrl(new Uri("http://www.linkedin.com/mupld/process?goback=%2Enpv_" + profileId + "_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1&filter=car80&return_type=html&mid=%2F" + Images1 + "&callback=profilePicture.savePhoto&filters_scale_h=80&filters_scale_w=80&filters_crop_x=0&filters_crop_y=0&filters_crop_h=335&filters_crop_w=335&filters_rotate_t="));
                        //    }
                        //    catch { }
                        //}
                        //if (FirstGet.Contains("SUCCESS"))
                        //{
                        //    try
                        //    {
                        //        Images = FirstGet.Substring(FirstGet.IndexOf("value\":"), FirstGet.IndexOf("}") - FirstGet.IndexOf("value\":")).Replace("value\":", string.Empty).Replace("\"", string.Empty).Replace("\\", string.Empty).Trim();
                        //        Images = Images.Substring(1, (Images.Length - 1));
                        //        // Images = Uri.EscapeDataString(Images);
                        //    }
                        //    catch { }
                        //} 
                        #endregion

                    }
                    #region Old Code
                    //else
                    //{
                    //    try
                    //    {
                    //        FirstGet = HttpHelper.getHtmlfromUrl(new Uri("http://www.linkedin.com/mupld/process?goback=%2Enpv_" + profileId + "_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1&filter=" + setCropFilter + "&return_type=html&mid=%2F" + Images1 + "&callback=profilePicture.savePhoto&filters_scale_h=" + setMaskHeight + "&filters_scale_w=" + setMaskWidth + "&filters_crop_x=" + setMaskLeft + "&filters_crop_y=" + setMaskTop + "&filters_crop_h=" + setMaskHeight + "&filters_crop_w=" + setMaskWidth + "&filters_rotate_t="));
                    //    }
                    //    catch { }
                    //    if (string.IsNullOrEmpty(FirstGet))
                    //    {
                    //        FirstGet = HttpHelper.getHtmlfromUrl(new Uri("https://www.linkedin.com/mupld/process?goback=%2Enpv_" + profileId + "_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1&filter=" + setCropFilter + "&return_type=html&mid=%2F" + Images1 + "&callback=profilePicture.savePhoto&filters_scale_h=80&filters_scale_w=80&filters_crop_x=" + setMaskLeft + "&filters_crop_y=" + setMaskTop + "&filters_crop_h=" + setMaskHeight + "&filters_crop_w=" + setMaskWidth + "&filters_rotate_t="));

                    //    }
                    //    if (FirstGet.Contains("SUCCESS"))
                    //    {
                    //        try
                    //        {
                    //            Images = FirstGet.Substring(FirstGet.IndexOf("value\":"), FirstGet.IndexOf("}") - FirstGet.IndexOf("value\":")).Replace("value\":", string.Empty).Replace("\"", string.Empty).Replace("\\", string.Empty).Trim();
                    //            Images = Images.Substring(1, (Images.Length - 1));
                    //            // Images = Uri.EscapeDataString(Images);
                    //        }
                    //        catch { }
                    //    }
                    //    if (string.IsNullOrEmpty(FirstGet))
                    //    {
                    //        try
                    //        {
                    //            FirstGet = HttpHelper.getHtmlfromUrl(new Uri("http://www.linkedin.com/mupld/process?goback=%2Enpv_" + profileId + "_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1&filter=car80&return_type=html&mid=%2F" + Images1 + "&callback=profilePicture.savePhoto&filters_scale_h=80&filters_scale_w=80&filters_crop_x=0&filters_crop_y=0&filters_crop_h=335&filters_crop_w=335&filters_rotate_t="));
                    //        }
                    //        catch { }
                    //    }
                    //} 
                    #endregion
                }
                catch { }

                #region Old code
                //string thirdresponse = string.Empty;
                //try
                //{
                //    string secondGetResponsee = string.Empty;
                //    try
                //    {
                //        secondGetResponsee = HttpHelper.getHtmlfromUrl(new Uri("http://www.linkedin.com/profile/save-crop-picture?csrfToken=" + csrfToken + "&masterTempID=%2F" + Images + "&croppedTempID=%2F" + Images1 + "&xParam=0&yParam=0&xSizeParam=80&ySizeParam=80&nsave=n"));
                //    }
                //    catch { }

                //    if (secondGetResponsee.Contains("name=\"currenturl"))
                //    {
                //        try
                //        {
                //            string[] currenturlArr = Regex.Split(secondGetResponsee, "name=\"currenturl");
                //            string currenturl = currenturlArr[1].Substring(currenturlArr[1].IndexOf("value=\""), currenturlArr[1].IndexOf(">") - currenturlArr[1].IndexOf("value=\"")).Replace("value=\"", string.Empty).Replace("\"", string.Empty).Replace("\\", string.Empty).Trim();
                //            currenturl = Uri.UnescapeDataString(currenturl);
                //            currenturl = Regex.Split(currenturl, "&urlhash")[0];
                //            // string thirdresponse = HttpHelper.getHtmlfromUrl(new Uri("http://www.linkedin.com/profile/edit-picture-info?trk=nprofile-save-picture-submit&report%2Esuccess=9dCbijuoBwCwpKulrwnkugn6gmZHePJOoeK5jlW7JYSHYSSp-w55jdor_TR6umL"));
                //             thirdresponse = HttpHelper.getHtmlfromUrl(new Uri(currenturl));
                //        }
                //        catch { }
                //    }
                //}
                //catch { }


                //try
                //{
                //    string postSavePictureData = "pictureVisibility=NETWORK&csrfToken=" + csrfToken + "&sourceAlias=" + sourceAlias + "&goback=.npv_" + profileId + "_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1.npe_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1";
                //    string PostSavePictureUrl = "http://www.linkedin.com/profile/edit-picture-visibility-submit";
                //    string responsess = HttpHelper.postFormData(new Uri(PostSavePictureUrl), postSavePictureData);

                //    string GetResponse=HttpHelper.getHtmlfromUrl(new Uri("http://www.linkedin.com/profile/edit?goback=%2Enpv_"+profileId+"_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1%2Enpe_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1&trk=nprofile-save-picture-settings-submit&report%2Esuccess=88GfZh25oYpGMv_ncXhqwkk2axsD1cOiNrQErX7HNxy7raoMejQEhfBsYJTYoZpJLJh9oM"));
                //}
                //catch { }

                //try
                //{
                //    //string secondpostUrl="https://www.linkedin.com/mupld/upload?goback=%2Enpv_" + profileId + "_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1%2Enpe_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1";
                //    //string posturl = "pkey=PAGEKEY_PLACEHOLDER&tcode=ntf_click_notifications_icon&plist=alert_count%3A0&prefix=false";
                //    //string secondres=HttpHelper.postFormData(new Uri(secondpostUrl), posturl);
                //}
                //catch { }
                ////name="sourceAlias" 
                #endregion

                if (!string.IsNullOrEmpty(FirstGet) && !string.IsNullOrEmpty(response))
                {
                    if (FirstGet.Contains("SUCCESS") && response.Contains("SUCCESS")) // && !thirdresponse.Contains("error") && !string.IsNullOrEmpty(response) && !response.Contains("error"))
                    {
                        isSetProfilePic = true;
                    }
                    if (!string.IsNullOrEmpty(response) && !response.Contains("error"))
                    {

                    }
                }
                else
                {
                    return isSetProfilePic;
                }
            }
            catch
            {
            }
            return isSetProfilePic;
        } 
        #endregion

        #region HttpUploadProfilePic
        public string HttpUploadProfilePic(ref GlobusHttpHelper HttpHelper, string profileId, string url, string paramName, string contentType, string localImagePath, NameValueCollection nvc, string proxyAddress, int proxyPort, string proxyUsername, string proxyPassword)
        {
            string response = string.Empty;
            try
            {
                paramName = "file";


                //int w, h;
                //try
                //{
                //    GetJpegDimension(localImagePath, out w, out h);
                //}
                //catch { }



                // System.Drawing.Image objImage = System.drImage.FromFile("C:\imagename.gif");
                //int width = objImage.Width;
                // height = objImage.Height;  
                //string localImagePath1 = localImagePath.Replace("C:\\Users\\user\\Desktop\\Desktop\\FEmale pics", string.Empty).Replace("\\", string.Empty);
                string localImagePath1 = localImagePath;
                #region PostData_ForUploadImage
                try
                {
                    //Bitmap b = new Bitmap(path);
                    //w = b.width; h = b.height;
                }
                catch { }
                //               -----------------------------187451459023968
                //Content-Disposition: form-data; name="upload_info"

                //Dvhp8F7dIlmSwUmzNPsW985izeEG5OTZ35YK4V9YA1GIwwTqLJ9ljQxuxzH-wSutLAMYYF9AAXbxo-

                //6PDqZtoi7TuqbxCAeqHX9NoQ_uvzd4zaRhmuzlTKkl-3VOiMuWDzlK003lzMDv5UwRLhQFXcLF-3v-

                //4qdfLwWwKyPTGvbIw4ektMKwVE5ists0ThihmuflKFiuAZrO7-

                //uWDzlK0037zwcC55XRhekF2EiiXObIlloWDq5ljWA5zanovadkQ5ilFH5_ui2VuP1
                //-----------------------------187451459023968
                //Content-Disposition: form-data; name="upload_info_with_js"

                //Dvhp8F7dIlmSwUmzNPsW985izeEG5OTZ35YK4V9YA1GIwwTqLJ9ljQxuxzH-wSutLAMYYF9AAXbxo-

                //6PDqZtoi7TuqbxCAeqHX9NoQ_uvzd4zaRhmuzlTKkl-3VOiMuWDzlK003lzMDv5UwRLhQFXcLF-3v-

                //4qdfLwWwKyPTGvbIw4ektMKwVE5ists0ThihmuflKFiuAZrO7-

                //uWDzlK0037zwcC55XRhekF2EiiXObIlloWDq5ljWA5zanovadkQ5ilFH5_ui2VuP1
                //-----------------------------187451459023968
                //Content-Disposition: form-data; name="callback"

                //profilePicture.processResponse
                //-----------------------------187451459023968
                //Content-Disposition: form-data; name="file"; filename="Koala.jpg"
                //Content-Type: image/jpeg

                //����

                #endregion

                try
                {
                    ////log.Debug(string.Format("Uploading {0} to {1}", file, url));
                    //string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("");
                    string boundary = "---------------------------" + DateTime.Now.Ticks.ToString();
                    byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

                    gRequest = (HttpWebRequest)WebRequest.Create(url);
                    gRequest.ContentType = "multipart/form-data; boundary=" + boundary;
                    gRequest.Referer = "http://www.linkedin.com/profile/edit-picture-info?goback=%2Enpv_" + profileId + "_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1%2Enpe_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1&trk=prof-ovw-edit-photo";
                    gRequest.Method = "POST";
                    gRequest.KeepAlive = true;
                    gRequest.Credentials = System.Net.CredentialCache.DefaultCredentials;

                    gRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:12.0) Gecko/20100101 Firefox/12.0";//"User-Agent: Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.16) Gecko/20110319 Firefox/3.6.16";

                    gRequest.Accept = " text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8, */*";

                    gRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                    ChangeProxy(proxyAddress, proxyPort, proxyUsername, proxyPassword);

                    gRequest.CookieContainer = new CookieContainer(); //gCookiesContainer;

                    #region CookieManagment

                    if (this.gCookies != null && this.gCookies.Count > 0)
                    {
                        gRequest.CookieContainer.Add(gCookies);
                    }
                    #endregion

                    using (Stream rs = gRequest.GetRequestStream())
                    {
                        //Stream rs = gRequest.GetRequestStream();

                        int tempi = 0;

                        string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
                        foreach (string key in nvc.Keys)
                        {
                            string formitem = string.Empty;
                            if (tempi == 0)
                            {
                                byte[] firstboundarybytes = System.Text.Encoding.ASCII.GetBytes("--" + boundary + "\r\n");
                                rs.Write(firstboundarybytes, 0, firstboundarybytes.Length);
                                formitem = string.Format(formdataTemplate, key, nvc[key]);
                                byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                                rs.Write(formitembytes, 0, formitembytes.Length);
                                tempi++;
                                continue;
                            }
                            rs.Write(boundarybytes, 0, boundarybytes.Length);
                            formitem = string.Format(formdataTemplate, key, nvc[key]);
                            byte[] formitembytes1 = System.Text.Encoding.UTF8.GetBytes(formitem);
                            rs.Write(formitembytes1, 0, formitembytes1.Length);
                        }
                        rs.Write(boundarybytes, 0, boundarybytes.Length);

                        string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
                        string header = string.Format(headerTemplate, paramName, localImagePath1, contentType);
                        byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
                        rs.Write(headerbytes, 0, headerbytes.Length);

                        using (FileStream fileStream = new FileStream(localImagePath, FileMode.Open, FileAccess.Read))
                        {
                            //FileStream fileStream = new FileStream(localImagePath, FileMode.Open, FileAccess.Read);
                            byte[] buffer = new byte[4096];
                            int bytesRead = 0;
                            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                            {
                                rs.Write(buffer, 0, bytesRead);
                            }
                            //fileStream.Close(); 
                        }

                        byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
                        rs.Write(trailer, 0, trailer.Length);
                        //rs.Close(); 
                    }

                    #region CookieManagment

                    if (this.gCookies != null && this.gCookies.Count > 0)
                    {
                        gRequest.CookieContainer.Add(gCookies);
                    }

                    #endregion

                    using (WebResponse wresp = gRequest.GetResponse())
                    {
                        //WebResponse wresp = null;
                        try
                        {
                            //wresp = gRequest.GetResponse();
                            using (Stream stream2 = wresp.GetResponseStream())
                            {
                                //Stream stream2 = wresp.GetResponseStream();
                                using (StreamReader reader2 = new StreamReader(stream2))
                                {
                                    //StreamReader reader2 = new StreamReader(stream2);
                                    response = reader2.ReadToEnd();
                                }
                            }
                            //log.Debug(string.Format("File uploaded, server response is: {0}", reader2.ReadToEnd()));
                            return response;
                        }
                        catch (Exception ex)
                        {
                            ////log.Error("Error uploading file", ex);
                            //if (wresp != null)
                            //{
                            //    wresp.Close();
                            //    wresp = null;
                            //}
                            //// return false;
                        }
                    }
                }
                catch
                {
                }

                finally
                {
                    gRequest = null;
                }
            }
            catch
            {
            }
            return response;
        } 
        #endregion

        #region GetJpegDimension
        static bool GetJpegDimension(string fileName, out int width, out int height)
        {

            width = height = 0;
            bool found = false;
            bool eof = false;

            FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            BinaryReader reader = new BinaryReader(stream);

            while (!found || eof)
            {

                // read 0xFF and the type
                reader.ReadByte();
                byte type = reader.ReadByte();

                // get length
                int len = 0;
                switch (type)
                {
                    // start and end of the image
                    case 0xD8:
                    case 0xD9:
                        len = 0;
                        break;

                    // restart interval
                    case 0xDD:
                        len = 2;
                        break;

                    // the next two bytes is the length
                    default:
                        int lenHi = reader.ReadByte();
                        int lenLo = reader.ReadByte();
                        len = (lenHi << 8 | lenLo) - 2;
                        break;
                }

                // EOF?
                if (type == 0xD9)
                    eof = true;

                // process the data
                if (len > 0)
                {
                    // read the data
                    byte[] data = reader.ReadBytes(len);

                    // this is what we are looking for
                    if (type == 0xC0)
                    {
                        width = data[1] << 8 | data[2];
                        height = data[3] << 8 | data[4];
                        found = true;
                    }

                }

            }

            reader.Close();
            stream.Close();
            return found;

        } 
        #endregion

        #region GetDataWithTagValueByTagAndAttributeNameWithClass
        public string GetDataWithTagValueByTagAndAttributeNameWithClass(string pageSrcHtml, string TagName, string AttributeName)
        {
            string dataDescription = string.Empty;
            try
            {
                bool success = false;
                string xHtml = string.Empty;

                Chilkat.HtmlToXml htmlToXml = new Chilkat.HtmlToXml();

                //*** Check DLL working or not **********************
                success = htmlToXml.UnlockComponent("THEBACHtmlToXml_7WY3A57sZH3O");
                if ((success != true))
                {
                    Console.WriteLine(htmlToXml.LastErrorText);
                    return null;
                }

                htmlToXml.Html = pageSrcHtml;

                //** Convert Data Html to XML ******************************************* 
                xHtml = htmlToXml.ToXml();

                //******************************************
                Chilkat.Xml xNode = default(Chilkat.Xml);
                Chilkat.Xml xBeginSearchAfter = default(Chilkat.Xml);
                Chilkat.Xml xml = new Chilkat.Xml();
                xml.LoadXml(xHtml);

                #region Data Save in list From using XML Tag and Attribut
                string DescriptionMain = string.Empty;

                string dataDescriptionValue = string.Empty;


                xBeginSearchAfter = null;

                xNode = xml.SearchForAttribute(xBeginSearchAfter, TagName, "class", AttributeName);
                while ((xNode != null))
                {
                    //** Get Data Under Tag only Text Value**********************************



                    dataDescription = xNode.GetXml();//.AccumulateTagContent("text", "script|style");

                    dataDescriptionValue = dataDescriptionValue + dataDescription;
                    //    string text = xNode.AccumulateTagContent("text", "script|style");
                    //    lstData.Add(text);

                    //    //** Get Data Under Tag All  Html value * *********************************
                    //    //dataDescription = xNode.GetXml();

                    xBeginSearchAfter = xNode;
                    xNode = xml.SearchForAttribute(xBeginSearchAfter, TagName, "class", AttributeName);
                    //if (dataDescription.Length > 500)
                    //{
                    //    break;
                    //}
                }
                #endregion
                return dataDescriptionValue;
            }
            catch (Exception)
            {
                return dataDescription = null;

            }
        } 
        #endregion

        #region GetDataWithTagValueByTagAndAttributeNameWithId
        public string GetDataWithTagValueByTagAndAttributeNameWithId(string pageSrcHtml, string TagName, string AttributeName)
        {
            string dataDescription = string.Empty;
            try
            {
                bool success = false;
                string xHtml = string.Empty;

                Chilkat.HtmlToXml htmlToXml = new Chilkat.HtmlToXml();

                //*** Check DLL working or not **********************
                success = htmlToXml.UnlockComponent("THEBACHtmlToXml_7WY3A57sZH3O");
                if ((success != true))
                {
                    Console.WriteLine(htmlToXml.LastErrorText);
                    return null;
                }

                htmlToXml.Html = pageSrcHtml;

                //** Convert Data Html to XML ******************************************* 
                xHtml = htmlToXml.ToXml();

                //******************************************
                Chilkat.Xml xNode = default(Chilkat.Xml);
                Chilkat.Xml xBeginSearchAfter = default(Chilkat.Xml);
                Chilkat.Xml xml = new Chilkat.Xml();
                xml.LoadXml(xHtml);

                #region Data Save in list From using XML Tag and Attribut
                string DescriptionMain = string.Empty;

                string dataDescriptionValue = string.Empty;


                xBeginSearchAfter = null;

                xNode = xml.SearchForAttribute(xBeginSearchAfter, TagName, "id", AttributeName);
                while ((xNode != null))
                {
                    //** Get Data Under Tag only Text Value**********************************



                    dataDescription = xNode.GetXml();//.AccumulateTagContent("text", "script|style");

                    dataDescriptionValue = dataDescriptionValue + dataDescription;
                    //    string text = xNode.AccumulateTagContent("text", "script|style");
                    //    lstData.Add(text);

                    //    //** Get Data Under Tag All  Html value * *********************************
                    //    //dataDescription = xNode.GetXml();

                    xBeginSearchAfter = xNode;
                    xNode = xml.SearchForAttribute(xBeginSearchAfter, TagName, "id", AttributeName);
                    //if (dataDescription.Length > 500)
                    //{
                    //    break;
                    //}
                }
                #endregion
                return dataDescriptionValue;
            }
            catch (Exception)
            {
                return dataDescription = null;

            }
        } 
        #endregion

        #region GetHrefsByTagAndAttributeName
        public List<string> GetHrefsByTagAndAttributeName(string pageSrcHtml, string TagName, string AttributeName)
        {
            List<string> lstData = new List<string>();
            try
            {
                bool success = false;
                string xHtml = string.Empty;

                Chilkat.HtmlToXml htmlToXml = new Chilkat.HtmlToXml();

                //*** Check DLL working or not **********************
                success = htmlToXml.UnlockComponent("THEBACHtmlToXml_7WY3A57sZH3O");
                if ((success != true))
                {
                    Console.WriteLine(htmlToXml.LastErrorText);
                    return null;
                }

                htmlToXml.Html = pageSrcHtml;

                //** Convert Data Html to XML ******************************************* 
                xHtml = htmlToXml.ToXml();

                //******************************************
                Chilkat.Xml xNode = default(Chilkat.Xml);
                Chilkat.Xml xBeginSearchAfter = default(Chilkat.Xml);
                Chilkat.Xml xml = new Chilkat.Xml();
                xml.LoadXml(xHtml);

                #region Data Save in list From using XML Tag and Attribut
                string DescriptionMain = string.Empty;
                string dataDescription = string.Empty;

                xBeginSearchAfter = null;

                xNode = xml.SearchForAttribute(xBeginSearchAfter, TagName, "class", AttributeName);
                while ((xNode != null))
                {
                    //** Get Data Under Tag only Text Value**********************************
                    dataDescription = xNode.GetXml();//.AccumulateTagContent("text", "script|style");

                    List<string> lstHrefs = GetHrefFromString(dataDescription);

                    lstData.AddRange(lstHrefs);//lstData.Add(dataDescription);

                    //** Get Data Under Tag All  Html value * *********************************
                    //dataDescription = xNode.GetXml();

                    xBeginSearchAfter = xNode;
                    xNode = xml.SearchForAttribute(xBeginSearchAfter, TagName, "class", AttributeName);
                }
                #endregion
                return lstData;
            }
            catch (Exception)
            {
                return lstData = null;

            }
        } 
        #endregion

        #region GetTextDataByTagAndAttributeName
        public List<string> GetTextDataByTagAndAttributeName(string pageSrcHtml, string TagName, string AttributeName)
        {
            List<string> lstData = new List<string>();
            try
            {
                bool success = false;
                string xHtml = string.Empty;

                Chilkat.HtmlToXml htmlToXml = new Chilkat.HtmlToXml();

                //*** Check DLL working or not **********************
                success = htmlToXml.UnlockComponent("THEBACHtmlToXml_7WY3A57sZH3O");
                if ((success != true))
                {
                    Console.WriteLine(htmlToXml.LastErrorText);
                    return null;
                }

                htmlToXml.Html = pageSrcHtml;

                //** Convert Data Html to XML ******************************************* 
                xHtml = htmlToXml.ToXml();

                //******************************************
                Chilkat.Xml xNode = default(Chilkat.Xml);
                Chilkat.Xml xBeginSearchAfter = default(Chilkat.Xml);
                Chilkat.Xml xml = new Chilkat.Xml();
                xml.LoadXml(xHtml);

                #region Data Save in list From using XML Tag and Attribut
                string DescriptionMain = string.Empty;
                string dataDescription = string.Empty;

                xBeginSearchAfter = null;

                xNode = xml.SearchForAttribute(xBeginSearchAfter, TagName, "class", AttributeName);
                while ((xNode != null))
                {
                    //** Get Data Under Tag only Text Value**********************************
                    dataDescription = xNode.GetXml();//.AccumulateTagContent("text", "script|style");

                    string text = xNode.AccumulateTagContent("text", "script|style");
                    lstData.Add(text);

                    //** Get Data Under Tag All  Html value * *********************************
                    //dataDescription = xNode.GetXml();

                    xBeginSearchAfter = xNode;
                    xNode = xml.SearchForAttribute(xBeginSearchAfter, TagName, "class", AttributeName);
                }
                #endregion
                return lstData;
            }
            catch (Exception)
            {
                return lstData = null;

            }
        } 
        #endregion

        #region GetHrefFromString
        public List<string> GetHrefFromString(string pageSrcHtml)
        {
            Chilkat.HtmlUtil obj = new Chilkat.HtmlUtil();

            Chilkat.StringArray dataImage = obj.GetHyperlinkedUrls(pageSrcHtml);

            List<string> list = new List<string>();

            for (int i = 0; i < dataImage.Length; i++)
            {
                string hreflink = dataImage.GetString(i);
                list.Add(hreflink);

            }
            return list;
        } 
        #endregion

        public List<string> GetDataTag(string pageSrcHtml, string TagName)
        {
            bool success = false;
            string xHtml = string.Empty;
            List<string> list = new List<string>();

            try
            {


                Chilkat.HtmlToXml htmlToXml = new Chilkat.HtmlToXml();
                success = htmlToXml.UnlockComponent("THEBACHtmlToXml_7WY3A57sZH3O");
                if ((success != true))
                {
                    Console.WriteLine(htmlToXml.LastErrorText);
                    return null;
                }

                htmlToXml.Html = pageSrcHtml;

                //xHtml contain xml data 
                xHtml = htmlToXml.ToXml();

                //******************************************
                Chilkat.Xml xNode = default(Chilkat.Xml);
                Chilkat.Xml xBeginSearchAfter = default(Chilkat.Xml);
                Chilkat.Xml xml = new Chilkat.Xml();
                xml.LoadXml(xHtml);

                xBeginSearchAfter = null;
                xNode = xml.SearchForTag(xBeginSearchAfter, TagName);
                while ((xNode != null))
                {

                    string TagText = xNode.AccumulateTagContent("text", "script|style");

                    list.Add(TagText);

                    xBeginSearchAfter = xNode;
                    xNode = xml.SearchForTag(xBeginSearchAfter, TagName);

                }
                //xHtml.
            }
            catch (Exception ex)
            {
                //GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
            return list;
        }



        #region postFormDataRef
        public string postFormDataRefDemo(Uri formActionUrl, string postData, string Referes, string Token, string XRequestedWith, string XPhx, string Origin, string isAjaxForm)
        {

            gRequest = (HttpWebRequest)WebRequest.Create(formActionUrl);
            //gRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:12.0) Gecko/20100101 Firefox/12.0";
            gRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2272.101 Safari/537.36";
            //Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/37.0.2062.120 Safari/537.36


            gRequest.CookieContainer = new CookieContainer();// gCookiesContainer;
            gRequest.Method = "POST";
            //gRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8, */*";
            gRequest.Accept = "application/json, text/javascript, */*; q=0.01";
            gRequest.Headers["Accept-Language"] = "en-us,en;q=0.8";
            gRequest.KeepAlive = true;
            gRequest.ContentType = @"application/x-www-form-urlencoded";

            //gRequest.Headers.Add("Javascript-enabled", "true");

            gRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            if (!string.IsNullOrEmpty(Referes))
            {
                gRequest.Referer = Referes;
            }
            if (!string.IsNullOrEmpty(Token))
            {
                gRequest.Headers.Add("X-CSRFToken", Token);
            }
            if (!string.IsNullOrEmpty(XRequestedWith))
            {
                gRequest.Headers.Add("X-Requested-With", XRequestedWith);
            }
            if (!string.IsNullOrEmpty(XPhx))
            {
                gRequest.Headers.Add("X-PHX", XPhx);
            }
            if (!string.IsNullOrEmpty(Origin))
            {
                gRequest.Headers.Add("Origin", Origin);
            }
            if (!string.IsNullOrEmpty(isAjaxForm))
            {
                gRequest.Headers.Add("X-IsAJAXForm", isAjaxForm);
            }
            //if (!string.IsNullOrEmpty(Origin))
            //{
            //    gRequest.Headers.Add("Origin", Origin);
            //}
            //if (!string.IsNullOrEmpty(Origin))
            //{
            //    gRequest.Headers.Add("Origin", Origin);
            //}

            ///Modified BySumit 18-11-2011
            ChangeProxy(proxyAddress, port, proxyUsername, proxyPassword);

            #region CookieManagement
            if (this.gCookies != null && this.gCookies.Count > 0)
            {
                setExpect100Continue();
                gRequest.CookieContainer.Add(gCookies);
            }

            //logic to postdata to the form
            try
            {
                setExpect100Continue();
                //string postdata = string.Format(postData);
                byte[] postBuffer = System.Text.Encoding.GetEncoding(1252).GetBytes(postData);
                gRequest.ContentLength = postBuffer.Length;
                Stream postDataStream = gRequest.GetRequestStream();
                postDataStream.Write(postBuffer, 0, postBuffer.Length);
                postDataStream.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // Logger.LogText("Internet Connectivity Exception : "+ ex.Message + "StackTrace --> >>>" + ex.StackTrace,null);
            }
            //post data logic ends

            //Get Response for this request url
            try
            {
                gResponse = (HttpWebResponse)gRequest.GetResponse();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //Logger.LogText("Response from "+formActionUrl + ":" + ex.Message + "StackTrace --> >>>" + ex.StackTrace,null);
            }



            //check if the status code is http 200 or http ok

            if (gResponse.StatusCode == HttpStatusCode.OK)
            {
                //get all the cookies from the current request and add them to the response object cookies
                setExpect100Continue();
                gResponse.Cookies = gRequest.CookieContainer.GetCookies(gRequest.RequestUri);
                //check if response object has any cookies or not
                //Added by sandeep pathak
                //gCookiesContainer = gRequest.CookieContainer;  

                if (gResponse.Cookies.Count > 0)
                {
                    //check if this is the first request/response, if this is the response of first request gCookies
                    //will be null
                    if (this.gCookies == null)
                    {
                        gCookies = gResponse.Cookies;
                    }
                    else
                    {
                        foreach (Cookie oRespCookie in gResponse.Cookies)
                        {
                            bool bMatch = false;
                            foreach (Cookie oReqCookie in this.gCookies)
                            {
                                if (oReqCookie.Name == oRespCookie.Name)
                                {
                                    oReqCookie.Value = oRespCookie.Value;
                                    bMatch = true;
                                    break; // 
                                }
                            }
                            if (!bMatch)
                                this.gCookies.Add(oRespCookie);
                        }
                    }
                }
            #endregion



                StreamReader reader = new StreamReader(gResponse.GetResponseStream());
                string responseString = reader.ReadToEnd();
                reader.Close();
                //Console.Write("Response String:" + responseString);
                return responseString;
            }
            else
            {
                return "Error in posting data";
            }

        }
        #endregion

    }

}
