using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseLib;
using Chilkat;

namespace RegexForScraping
{
    public static class ChilkatBasedRegex
    {
        public static List<string> GettingAllUrls(string PageSource, string MustMatchString)
        {
            List<string> suburllist = new List<string>();
            
            HtmlUtil htmlUtil = new HtmlUtil();
            PageSource = htmlUtil.EntityDecode(PageSource);
            StringArray datagoogle = htmlUtil.GetHyperlinkedUrls(PageSource);

            for (int i = 0; i < datagoogle.Length; i++)
            {
                string hreflink = datagoogle.GetString(i);

                if (hreflink.Contains(MustMatchString) && hreflink.Contains("goback"))
                {
                    suburllist.Add(hreflink);
                }
            }
            return suburllist.Distinct().ToList();
        }
    }
}
