#region namespaces
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
#endregion

namespace BaseLib
{
    public class GlobusRegex
    {
        #region GetDataFromString
        public string GetDataFromString(string HtmlData, string RegexPattern)
        {
            string strData = string.Empty;
            Regex anchorTextExtractor = new Regex(RegexPattern);
            foreach (Match url in anchorTextExtractor.Matches(HtmlData))
            {
                strData = url.Value;
            }
            return strData;
        }
        #endregion

        #region public List<string> GetListOfDataFromString(string HtmalData, string RegexPattern)
        public List<string> GetListOfDataFromString(string HtmalData, string RegexPattern)
        {
            List<string> lstData = new List<string>();
            var regex = new Regex(RegexPattern, RegexOptions.Compiled);
            foreach (Match url in regex.Matches(HtmalData))
            {
                lstData.Add(url.Value);
            }
            return lstData;
        }
        #endregion

        #region public string GetAnchorTag(string HtmlData)
        public string GetAnchorTag(string HtmlData)
        {
            string AnchorUrl = null;

            Regex anchorTextExtractor = new Regex(@"<a.*href=[""'](?<url>[^""^']+[.]*)[""'].*>(?<name>[^<]+[.]*)</a>");
            foreach (Match url in anchorTextExtractor.Matches(HtmlData))
            {
                AnchorUrl = url.Value;
            }
            return AnchorUrl;
        }
        #endregion

        #region public List<string> GetAnchorTags(string HtmlData)
        public List<string> GetAnchorTags(string HtmlData)
        {
            List<string> lstAnchorUrl = new List<string>();

            Regex anchorTextExtractor = new Regex(@"<a.*href=[""'](?<url>[^""^']+[.]*)[""'].*>(?<name>[^<]+[.]*)</a>");
            foreach (Match url in anchorTextExtractor.Matches(HtmlData))
            {
                lstAnchorUrl.Add(url.Value);
            }
            return lstAnchorUrl;
        }
        #endregion

        #region public string StripTagsRegex(string HtmlData)
        public string StripTagsRegex(string HtmlData)
        {
            return Regex.Replace(HtmlData, "<.*?>", string.Empty);
        }
        #endregion

        #region public string GetUrlFromString(string HtmlData)
        public string GetUrlFromString(string HtmlData)
        {
            string strUrl = string.Empty;
            var regex = new Regex(@"http://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", RegexOptions.Compiled);
            var ModifiedString = HtmlData.Replace("\"", " ").Replace("<", " ").Replace(">", " ");
            foreach (Match url in regex.Matches(ModifiedString))
            {
                strUrl = url.Value;
            }
            return strUrl;
        }
        #endregion

        #region public string GetHttpsUrlFromString(string HtmlData)
        public string GetHttpsUrlFromString(string HtmlData)
        {
            string strUrl = string.Empty;
            var regex = new Regex(@"https://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", RegexOptions.Compiled);
            var ModifiedString = HtmlData.Replace("\"", " ").Replace("<", " ").Replace(">", " ");
            foreach (Match url in regex.Matches(ModifiedString))
            {
                strUrl = url.Value;
            }
            return strUrl;
        }
        #endregion

        #region public List<string> GetUrlsFromString(string HtmlData)
        public List<string> GetUrlsFromString(string HtmlData)
        {
            List<string> lstUrl = new List<string>();
            var regex = new Regex(@"http://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", RegexOptions.Compiled);
            var ModifiedString = HtmlData.Replace("\"", " ").Replace("<", " ").Replace(">", " ");
            foreach (Match url in regex.Matches(ModifiedString))
            {
                lstUrl.Add(url.Value);
            }
            return lstUrl;
        }
        #endregion

        #region public List<string> GetEmailsFromString(string HtmlData)
        public List<string> GetEmailsFromString(string HtmlData)
        {
            List<string> lstEmail = new List<string>();
            var regex = new Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", RegexOptions.Compiled);
            var PageData = HtmlData.Replace("\"", " ").Replace("<", " ").Replace(">", " ").Replace(":", " ");
            foreach (Match email in regex.Matches(PageData))
            {
                lstEmail.Add(email.Value);
            }
            return lstEmail;
        }
        #endregion

        #region public string GetEmailFromString(string HtmlData)
        public string GetEmailFromString(string HtmlData)
        {
            string strEmail = string.Empty;
            var regex = new Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", RegexOptions.Compiled);
            var PageData = HtmlData.Replace("\"", " ").Replace("<", " ").Replace(">", " ").Replace(":", " ");
            foreach (Match email in regex.Matches(PageData))
            {
                strEmail = email.Value;
            }
            return strEmail;
        }
        #endregion

        #region public string GetH1Tag(string HtmlData)
        public string GetH1Tag(string HtmlData)
        {
            string strEmail = string.Empty;
            var regex = new Regex(@"<h1.*>(.*)</h1>", RegexOptions.Compiled);
            var PageData = HtmlData;//.Replace("\"", " ").Replace("<", " ").Replace(">", " ").Replace(":", " ");
            foreach (Match email in regex.Matches(PageData))
            {
                strEmail = email.Value;
            }
            return strEmail;
        }
        #endregion

        #region public string GetH3Tag(string HtmlData)
        public string GetH3Tag(string HtmlData)
        {
            string strEmail = string.Empty;
            var regex = new Regex(@"<h3.*>(.*)</h3>", RegexOptions.Compiled);
            var PageData = HtmlData;//.Replace("\"", " ").Replace("<", " ").Replace(">", " ").Replace(":", " ");
            foreach (Match email in regex.Matches(PageData))
            {
                strEmail = email.Value;
            }
            return strEmail;
        }
        #endregion

        #region public string GetText(string HtmlData)
        public string GetText(string HtmlData)
        {
            string AnchorUrl = null;

            Regex anchorTextExtractor = new Regex(@"^\d{2}-\d{3}-\d{4}$", RegexOptions.Compiled);
            foreach (Match url in anchorTextExtractor.Matches(HtmlData))
            {
                AnchorUrl = url.Value;
            }
            return AnchorUrl;
        }
        #endregion

        #region public List<string> GetInputControlsNameAndValueInPage(string HtmlData)
        public List<string> GetInputControlsNameAndValueInPage(string HtmlData)
        {
            List<string> lst = new List<string>();
            string strRegExPatten = "<\\s*input.*?name\\s*=\\s*\"(?<Name>.*?)\".*?value\\s*=\\s*\"(?<Value>.*?)\".*?>";
            Regex reg = new Regex(strRegExPatten, RegexOptions.Multiline);
            MatchCollection mc = reg.Matches(HtmlData);
            string strTemp = string.Empty;
            foreach (Match m in mc)
            {
                strTemp = strTemp + m.Groups["Name"].Value + "=" + m.Groups["Value"].Value + "&";
                lst.Add(strTemp);
            }
            return lst;
        }
        #endregion

        #region public List<string> GetHrefUrlTags(string HtmlData)
        public List<string> GetHrefUrlTags(string HtmlData)
        {
            List<string> lstAnchorUrl = new List<string>();
            Regex anchorTextExtractor = new Regex(@"href=[""'](?<url>[^""^']+[.]*)[""'].*>");
            foreach (Match url in anchorTextExtractor.Matches(HtmlData))
            {
                lstAnchorUrl.Add(url.Value);
            }
            return lstAnchorUrl;
        }
        #endregion

        #region public static List<string> GetHrefsFromString(string HtmlData)
        public static List<string> GetHrefsFromString(string HtmlData)
        {
            List<string> lstUrl = new List<string>();
            var regex = new Regex(@"http://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", RegexOptions.Compiled);
            var ModifiedString = HtmlData.Replace("\"", " ").Replace("<", " ").Replace(">", " ");
            foreach (Match url in regex.Matches(ModifiedString))
            {
                lstUrl.Add(url.Value);
            }

            var regexhttps = new Regex(@"https://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", RegexOptions.Compiled);
            var ModifiedStringHttps = HtmlData.Replace("\"", " ").Replace("<", " ").Replace(">", " ");
            foreach (Match url in regexhttps.Matches(ModifiedStringHttps))
            {
                lstUrl.Add(url.Value);
            }

            return lstUrl;
        }
        #endregion

        #region public string GetHrefUrlTag(string HtmlData)
        public string GetHrefUrlTag(string HtmlData)
        {
            List<string> lstAnchorUrl = new List<string>();
            Regex anchorTextExtractor = new Regex(@"href=[""'](?<url>[^""^']+[.]*)");
            foreach (Match url in anchorTextExtractor.Matches(HtmlData))
            {
                return url.Value;
            }
            return null;
        }
        #endregion

        #region public List<string> GetLIAnchorTags(string HtmlData)
        public List<string> GetLIAnchorTags(string HtmlData)
        {
            List<string> lstAnchorUrl = new List<string>();

            Regex anchorTextExtractor = new Regex(@"<li><a.*href=[""'](?<url>[^""^']+[.]*)[""'].*>(?<name>[^<]+[.]*)</a></li>");
            foreach (Match url in anchorTextExtractor.Matches(HtmlData))
            {
                lstAnchorUrl.Add(url.Value);
            }
            return lstAnchorUrl;
        }
        #endregion

        #region public string GetBoldTag(string HtmlData)
        public string GetBoldTag(string HtmlData)
        {
            string Tag = string.Empty;
            Regex anchorTextExtractor = new Regex(@"<b\b[^>]*>(.*?)</b>");
            foreach (Match url in anchorTextExtractor.Matches(HtmlData))
            {
                Tag = url.Value;
                return Tag;
            }
            return Tag;
        }
        #endregion

        #region public static bool ValidateNumber(string inputString)
        public static bool ValidateNumber(string inputString)
        {
            Regex IdCheck = new Regex("^[0-9]*$");
            if (!string.IsNullOrEmpty(inputString) && IdCheck.IsMatch(inputString))
            {
                return true;
            }
            return false;
        }
        #endregion

        #region public string GetAnchorUrl(string Url)
        public string GetAnchorUrl(string Url)
        {
            string url = GetUrlFromString(Url);
            if (!string.IsNullOrEmpty(url))
            {
                string myString = string.Empty;
                Regex r = new Regex("(http://[^ ]+)");
                myString = r.Replace(Url, "<a href=\"$1\" target=\"_blank\">$1</a>");
                string NewString = Url.Replace(Url, myString);
                return NewString;
            }
            else
            {
                return Url;
            }
        }
        #endregion
        /// <summary>
        /// Count Part Of String Occurrences in String
        /// </summary>
        /// <param name="text">String</param>
        /// <param name="pattern">Part Of String</param>
        /// <returns></returns>
        #region public int CountStringOccurrences(string WholeText, string Pattern)
        public int CountStringOccurrences(string WholeText, string Pattern)
        {
            try
            {
                int count = 0;
                Regex r = new Regex(Pattern, RegexOptions.IgnoreCase);
                Match m = r.Match(WholeText);
                while (m.Success)
                {
                    count++;
                }
                return count;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        #endregion

        #region public int CountExactStringMatches(string WholeText, string Pattern)
        public int CountExactStringMatches(string WholeText, string Pattern)
        {
            Pattern = "\b" + Pattern.ToLower() + "\b";
            MatchCollection mc = Regex.Matches(WholeText.ToLower(), Pattern);
            return mc.Count;
        }
        #endregion

        #region ParseJson
        public static string ParseJson(string data, string paramName)
        {
            try
            {
                int startIndx = data.IndexOf(paramName) + paramName.Length + 3;
                int endIndx = data.IndexOf("\"", startIndx);

                string value = data.Substring(startIndx, endIndx - startIndx);
                return value;
            }
            catch (Exception)
            {

                return null;
            }
        } 
        #endregion

        #region ParseEncodedJson
        public static string ParseEncodedJson(string data, string paramName)
        {
            try
            {
                data = data.Replace("&quot;", "\"");
                int startIndx = data.IndexOf("\"" + paramName + "\"") + ("\"" + paramName + "\"").Length + 1;
                int endIndx = data.IndexOf("\"", startIndx);

                string value = data.Substring(startIndx, endIndx - startIndx);
                value = value.Replace(",", "");
                return value;
            }
            catch (Exception)
            {

                return null;
            }
        } 
        #endregion

        #region GetParamValue
        public static string GetParamValue(string pgSrc, string paramName)
        {
            try
            {
                string temp1 = "name=\\\\\\\"" + paramName + "\\\\\\\"";
                string temp = "name=\\\"" + paramName + "\\\"";

                if (pgSrc.Contains("name='" + paramName + "'"))
                {
                    string param = "name='" + paramName + "'";
                    int startparamName = pgSrc.IndexOf(param) + param.Length;
                    startparamName = pgSrc.IndexOf("value=", startparamName) + "value=".Length + 1;
                    int endparamName = pgSrc.IndexOf("'", startparamName);
                    string valueparamName = pgSrc.Substring(startparamName, endparamName - startparamName);
                    return valueparamName;
                }
                else if (pgSrc.Contains("name=\"" + paramName + "\""))
                {
                    string param = "name=\"" + paramName + "\"";
                    int startparamName = pgSrc.IndexOf(param) + param.Length;
                    startparamName = pgSrc.IndexOf("value=", startparamName) + "value=".Length + 1;
                    int endcommentPostID = pgSrc.IndexOf("\"", startparamName);
                    string valueparamName = pgSrc.Substring(startparamName, endcommentPostID - startparamName);
                    return valueparamName;
                }
                else if (pgSrc.Contains("name=\\\"" + paramName + "\\\""))
                {
                    string param = "name=\\\"" + paramName + "\\\"";
                    int startparamName = pgSrc.IndexOf(param) + param.Length;
                    startparamName = pgSrc.IndexOf("value=\\", startparamName) + "value=\\".Length + 1;
                    int endcommentPostID = pgSrc.IndexOf("\\\"", startparamName);
                    string valueparamName = pgSrc.Substring(startparamName, endcommentPostID - startparamName);
                    return valueparamName;
                }

                else if (pgSrc.Contains("name=\\\\\\\"" + paramName + "\\\\\\\""))
                {
                    string param = "name=\\\\\\\"" + paramName + "\\\\\\\"";
                    int startparamName = pgSrc.IndexOf(param) + param.Length;
                    startparamName = pgSrc.IndexOf("value=\\\\\\\"", startparamName) + "value=\\\\\\\"".Length;
                    int endcommentPostID = pgSrc.IndexOf("\\\\\\\"", startparamName);
                    string valueparamName = pgSrc.Substring(startparamName, endcommentPostID - startparamName);
                    return valueparamName;
                }
                return "";
            }
            catch (Exception)
            {
                return "";
            }
        } 
        #endregion
    }
}
