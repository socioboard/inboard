using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chilkat;

namespace BaseLib
{
    public static class ChilkatBasedRegex
    {
        #region GettingAllUrls
        public static List<string> GettingAllUrls(string PageSource, string MustMatchString)
        {
            List<string> suburllist = new List<string>();

            HtmlUtil htmlUtil = new HtmlUtil();
            PageSource = htmlUtil.EntityDecode(PageSource);
            StringArray datagoogle = htmlUtil.GetHyperlinkedUrls(PageSource);

            for (int i = 0; i < datagoogle.Length; i++)
            {
                string hreflink = datagoogle.GetString(i);

                if (hreflink.Contains(MustMatchString) && hreflink.Contains("pp_profile_name_link") || hreflink.Contains("trk=anetppl_profile"))
                {
                    suburllist.Add(hreflink);
                }
            }
            return suburllist.Distinct().ToList();

        } 
        #endregion

        #region GettingAllUrls1
        public static List<string> GettingAllUrls1(string PageSource, string MustMatchString)
        {
            List<string> suburllist1 = new List<string>();

            HtmlUtil htmlUtil = new HtmlUtil();
            PageSource = htmlUtil.EntityDecode(PageSource);

            try
            {
                string[] Dataconnection = System.Text.RegularExpressions.Regex.Split(PageSource, "/profile/view?");
                string DataImage = string.Empty;

                foreach (string item in Dataconnection)
                {
                    if (!item.Contains("!DOCTYPE "))
                    {
                        if (item.Contains("vsrp_people_res_name"))
                        {

                            //string finalurl = item.Substring(item.IndexOf("/profile/view?id="), item.IndexOf("url_unfollow_infl") - item.IndexOf(",\"link_nprofile_view")).Trim();
                            //string finalurls1 = finalurl.Substring(finalurl.IndexOf("/profile/view?id="), finalurl.IndexOf("%3Aprimary")+10 - finalurl.IndexOf("")).Trim();
                            int startindex = item.IndexOf("?");
                            string start = item.Substring(startindex);
                            int endIndex = start.IndexOf(",");
                            string finalurl = "http://www.linkedin.com/profile/view" + start.Substring(0, endIndex).Replace("\"", string.Empty);
                            suburllist1.Add(finalurl);
                            //http://www.linkedin.com?id=230385129&authType=OUT_OF_NETWORK&authToken=OWRO&locale=en_US&srchid=2732756961375079939471&srchindex=1&srchtotal=10&trk=vsrp_people_res_name&trkInfo=VSRPsearchId%3A2732756961375079939471%2CVSRPtargetId%3A230385129%2CVSRPcmpt%3Aprimary"
                            //http://www.linkedin.com/profile/view?id=6741047&authType=OUT_OF_NETWORK&authToken=MWyZ&locale=en_US&srchid=2732758471375077875757&srchindex=1&srchtotal=55&trk=vsrp_people_res_name&trkInfo=VSRPsearchId%3A2732758471375077875757%2CVSRPtargetId%3A6741047%2CVSRPcmpt%3Aprimary

                        }
                    }
                }
            }

            catch { }
            return suburllist1.Distinct().ToList();

        } 
        #endregion

        #region GettingAllUrls2
        public static List<string> GettingAllUrls2(string PageSource, string MustMatchString)
        {
            List<string> suburllist1 = new List<string>();

            HtmlUtil htmlUtil = new HtmlUtil();
            PageSource = htmlUtil.EntityDecode(PageSource);

            try
            {
                string[] Dataconnection = System.Text.RegularExpressions.Regex.Split(PageSource, "2nd degree contact\",");
                string DataImage = string.Empty;

                foreach (string item in Dataconnection)
                {
                    if (!item.Contains("!DOCTYPE "))
                    {
                        if (item.Contains("&pid="))
                        {
                            int startindex = item.IndexOf("&pid=");
                            string start = item.Substring(startindex).Replace("&pid=","");
                            int endIndex = start.IndexOf(",");
                            string finalurl = "http://www.linkedin.com/profile/view?id=" + start.Substring(0, endIndex).Replace("\"", string.Empty);
                            suburllist1.Add(finalurl);
                        }
                    }
                }
            }

            catch { }
            return suburllist1.Distinct().ToList();

        }
        #endregion

      }
  }
        
      
   


   

 