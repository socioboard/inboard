using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Data.SQLite;

namespace BaseLib
{
    public class ProxyChecker
    {
        #region global declaration
        string proxyAddress { get; set; }
        string proxyPort { get; set; }
        string proxyUsername { get; set; }
        string proxyPassword { get; set; }
        public int IsPublic { get; set; }
        GlobusHttpHelper httpGbs = new GlobusHttpHelper(); 
        #endregion

        #region ProxyChecker
        public ProxyChecker(string proxyAddress, string proxyPort, string proxyUsername, string proxyPassword, int IsPublic)
        {
            this.proxyAddress = proxyAddress;
            this.proxyPort = proxyPort;
            this.proxyUsername = proxyUsername;
            this.proxyPassword = proxyPassword;
            this.IsPublic = IsPublic;
        } 
        #endregion

        #region CheckProxy
        public bool CheckProxy()
        {
            try
            {
                int Working = 0;
                string LoggedInIp = string.Empty;

                //BaseLib.ChilkatHttpHelpr HttpHelper = new BaseLib.ChilkatHttpHelpr();
                //string pageSource = HttpHelper.GetHtmlProxy("http://www.linkedin.com/", proxyAddress, proxyPort, proxyUsername, proxyPassword);

                GlobusHttpHelper HttpHelper = new GlobusHttpHelper();
                string pageSource = HttpHelper.getHtmlfromUrlProxy(new Uri("http://www.linkedin.com/"), proxyAddress, int.Parse(proxyPort), proxyUsername, proxyPassword);

                if (string.IsNullOrEmpty(pageSource))//(string.IsNullOrEmpty(pageSource) && string.IsNullOrEmpty(PgSrcHome))
                {
                    Thread.Sleep(500);
                    pageSource = HttpHelper.getHtmlfromUrlProxy(new Uri("http://www.linkedin.com/"), proxyAddress, int.Parse(proxyPort), proxyUsername, proxyPassword);
                    if (string.IsNullOrEmpty(pageSource))
                    {
                        return false;
                    }
                }
                ///Logic to check...
                if (pageSource.Contains("LinkedIn") && (pageSource.Contains("Sign Up")))
                {
                    try
                    {
                        using (SQLiteConnection con = new SQLiteConnection(DataBaseHandler.CONstr))
                        {
                            using (SQLiteDataAdapter ad = new SQLiteDataAdapter())
                            {
                                Working = 1;
                                string InsertQuery = "Insert into tb_Proxies values('" + proxyAddress + "','" + proxyPort + "','" + proxyUsername + "','" + proxyPassword + "', " + Working + "," + IsPublic + " , '" + LoggedInIp + "')";
                                DataBaseHandler.InsertQuery(InsertQuery, "tb_Proxies");
                                GlobusFileHelper.AppendStringToTextfileNewLine(proxyAddress + ":" + proxyPort + ":" + proxyUsername + ":" + proxyPassword, Globals.Path_ExsistingProxies);
                            }
                        }
                    }
                    catch { }

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return false;
            }
        } 
        #endregion
    }
}
