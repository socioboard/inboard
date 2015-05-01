using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaseLib
{
    public class ClsLinkedinMain
    {
        #region global declaration
        public string accountUser = string.Empty;
        public string accountPass = string.Empty;
        public string proxyAddress = string.Empty;
        public string proxyPort = string.Empty;
        public string proxyUserName = string.Empty;
        public string proxyPassword = string.Empty;
        public Dictionary<string, string> MemberNameAndID = new Dictionary<string, string>(); 
        #endregion

        #region ClsLinkedinMain
        public ClsLinkedinMain()
        {

        } 
        #endregion

        #region ClsLinkedinMain
        public ClsLinkedinMain(string UserName, string Password, string ProxyAddress, string ProxyPort, string ProxyUserName, string ProxyPassword)
        {
            this.accountUser = UserName;
            this.accountPass = Password;
            this.proxyAddress = ProxyAddress;
            this.proxyPort = ProxyPort;
            this.proxyUserName = ProxyUserName;
            this.proxyPassword = ProxyPassword;
        } 
        #endregion

        #region PostAddMembers
        public Dictionary<string, string> PostAddMembers(ref GlobusHttpHelper HttpHelper, string user)
        {
            try
            {
                string MemId = string.Empty;
                string MemFName = string.Empty;
                string MemLName = string.Empty;
                string MemFullName = string.Empty;
                string pageSource = string.Empty;
                string Name = string.Empty;
                int start1 = 0;
                #region old code
                //MemberDtl.Clear();
                //string pageGetreq = HttpHelper.getHtmlfromUrl(new Uri("http://www.linkedin.com/groups?gid=28410&csrfToken=ajax%3A1826913778783200924")); 
                //http://www.linkedin.com/connections2?displayFilteredConns=&fetchConnsFromDB=false
                #endregion

                //string pageSource = HttpHelper.getHtmlfromUrl(new Uri("http://www.linkedin.com/connections2?displayFilteredConns"));
                //string pageSource = HttpHelper.getHtmlfromUrl(new Uri("http://www.linkedin.com/connections"));
                //string pageSource = HttpHelper.getHtmlfromUrl(new Uri("https://www.linkedin.com/contacts/api/contacts/?start=0&count=10"));
                do
                {
                    
                    pageSource = HttpHelper.getHtmlfromUrl(new Uri("https://www.linkedin.com/contacts/api/contacts/?start="+start1+"&count=10"));
                    //string[] RgxGroupData = System.Text.RegularExpressions.Regex.Split(pageSource, "{&quot;");
                    string[] RgxGroupData = System.Text.RegularExpressions.Regex.Split(pageSource, "{\"name\":");

                    foreach (var Members in RgxGroupData)
                    {
                        try
                        {
                            if (Members.Contains(", \"id\":"))
                            {
                                int startindex = Members.IndexOf(", \"id\":");
                                string start = Members.Substring(startindex);
                                int endIndex = start.IndexOf("}");
                                MemId = start.Substring(0, endIndex).Replace("&quot;", string.Empty).Replace("id", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Replace("li_",string.Empty).Replace(", \"title\"",string.Empty).Replace("\"",string.Empty).Trim();
                                MemId = user + ':' + MemId;
                                string Fname = string.Empty;
                                string Lname = string.Empty;
                                try
                                {
                                    int StartIndex = Members.IndexOf("&quot;firstName&quot;:&quot;");
                                    if (StartIndex > 0)
                                    {
                                        string Start = Members.Substring(StartIndex).Replace("&quot;firstName&quot;:&quot;", "");
                                        int EndIndex = Start.IndexOf("&quot;,");
                                        string End = Start.Substring(0, EndIndex);
                                        Fname = End;
                                    }
                                }
                                catch (Exception ex)
                                {

                                }
                                try
                                {
                                    int StartIndex = Members.IndexOf("&quot;lastName&quot;:&quot;");
                                    if (StartIndex > 0)
                                    {
                                        string Start = Members.Substring(StartIndex).Replace("&quot;lastName&quot;:&quot;", "");
                                        int EndIndex = Start.IndexOf("&quot;,");
                                        string End = Start.Substring(0, EndIndex);
                                        Lname = End;
                                    }
                                }
                                catch (Exception ex)
                                {

                                }
                                try
                                {
                                    int StartIndex = Members.IndexOf("\"");
                                    if (StartIndex > 0)
                                    {
                                        string Start = Members.Substring(StartIndex).Replace("&quot;firstName&quot;:&quot;", "");
                                        int EndIndex = Start.IndexOf(", \"title\"");
                                        string End = Start.Substring(0, EndIndex).Replace(", \"title\"",string.Empty).Replace("\"",string.Empty).Trim();
                                        Name = End;
                                    }
                                }
                                catch (Exception ex)
                                {

                                }

                                //MemFullName = Fname + " " + Lname;
                                MemFullName = Name;

                                #region old code
                                //int startindex1 = Members.IndexOf("formattedName");
                                //string start1 = Members.Substring(startindex1);
                                //int endIndex1 = start1.IndexOf("}");
                                //MemFullName = start1.Substring(0, endIndex1).Replace("formattedName", string.Empty).Replace("&quot;", string.Empty).Replace(":", string.Empty).Trim();

                                //.Replace("&quot;", string.Empty).Replace(":", string.Empty).Replace(",", string.Empty).Trim();

                                //&quot;:&quot;agrawal, gaurav&quot;
                                //int startindex2 = Members.IndexOf("lastName");
                                //string start2 = Members.Substring(startindex2);
                                //int endIndex2 = start2.IndexOf("company\":\"");
                                //MemLName = start2.Substring(0, endIndex1).Replace("lastName", string.Empty).Replace("&quot", string.Empty).Replace(";",string.Empty).Replace(":", string.Empty).Replace(",", string.Empty).Replace("&quo",string.Empty).Trim();

                                //MemFullName = MemFName +":" + MemLName; 
                                #endregion
                                try
                                {
                                    MemberNameAndID.Add(MemId, MemFullName);
                                }
                                catch (Exception ex)
                                {

                                }
                            }
                        }
                        catch
                        {
                        }
                    }
                    start1++;
                } while (pageSource.Contains("name"));
                return MemberNameAndID;
            }
            catch (Exception ex)
            {
                return MemberNameAndID;
            }
        }
        #endregion
    }
}
