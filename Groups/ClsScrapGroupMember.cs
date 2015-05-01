using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using BaseLib;

namespace Groups
{
    public class ClsScrapGroupMember
    {
        #region variable declaration
        ChilkatHttpHelpr objChilkat = new ChilkatHttpHelpr();

        string _UserName = string.Empty;
        string _Password = string.Empty;
        string _ProxyAddress = string.Empty;
        string _ProxyPort = string.Empty;
        string _ProxyUserName = string.Empty;
        string _ProxyPassword = string.Empty;
        static string UN = string.Empty;

        public Events scrapGroupMemberLogEvents = new Events();

        public static bool isPathCreated = false; 
        #endregion

        #region ClsScrapGroupMember
        public ClsScrapGroupMember(string username, string password, string proxyaddress, string proxyport, string proxyusername, string proxypassword)
        {
            _UserName = username;
            UN = username;
            _Password = password;
            _ProxyAddress = proxyaddress;
            _ProxyPort = proxyport;
            _ProxyUserName = proxyusername;
            _ProxyPassword = proxypassword;

        } 
        #endregion

        public ClsScrapGroupMember()
        { 
        
        }
        #region List
        public static List<string> LstGroupURL
        {
            get;
            set;
        } 
        #endregion

        #region GetGroupMember
        public void GetGroupMember(ref GlobusHttpHelper httpHelper)
        {
            try
            {

                foreach (string item in LstGroupURL)
                {
                    try
                    {
                        if (!isPathCreated)
                        {
                            //File Path At Runtime

                            isPathCreated = true;

                            Globals.path_ScrappedMembersFromGroup = DateTime.Now.ToString("MM_dd_yyyy hh_mm_ss");
                        }

                        Log("[ " + DateTime.Now + " ] => [ Start Finding Group Member With The URL >>> " + item + " With Username >>> " + _UserName + " ]");
                        string groupId = string.Empty;
                        string groupIdPart = item.Substring(item.IndexOf("/groups"), item.Length - item.IndexOf("/groups")).Trim();
                        
                        int startindex = groupIdPart.IndexOf("&gid=");
                        if (startindex < 0)
                        {
                            startindex = groupIdPart.IndexOf("gid=");
                        }
                        string start = groupIdPart.Substring(startindex).Replace("&gid=", string.Empty).Replace("gid=", string.Empty);
                        int endindex = start.IndexOf("&");
                        if (endindex < 0)
                        {
                            start = start + "&";
                            endindex = start.IndexOf("&");
                        }
                        string end = start.Substring(0, endindex);
                        groupId = end.Trim();
                        
                        //if (item.Contains("gid") && item.Contains("/groups"))
                        //{
                        //    string groupIdPart = item.Substring(item.IndexOf("/groups"), item.Length - item.IndexOf("/groups")).Trim();

                        //    string[] numArr = Regex.Split(groupIdPart, "^[0-9]*$");

                        //    foreach (string item1 in numArr)
                        //    {
                        //        try
                        //        {
                        //            if (!string.IsNullOrEmpty(item1))
                        //            {
                        //                groupId = item1.Trim();

                        //                break;
                        //            }
                        //        }
                        //        catch (Exception ex)
                        //        {
                        //        }
                        //    }
                        //}

                        //if (item.Contains("/groups"))
                        //{
                        //    string groupIdPart = item.Substring(item.IndexOf("/groups"), item.Length - item.IndexOf("/groups")).Trim();

                        //    string[] numArr = Regex.Split(groupIdPart, "[^0-9]");

                        //    foreach (string item1 in numArr)
                        //    {
                        //        try
                        //        {
                        //            if (!string.IsNullOrEmpty(item1))
                        //            {
                        //                groupId = item1.Trim();

                        //                break;
                        //            }
                        //        }
                        //        catch (Exception ex)
                        //        {
                        //        }
                        //    }

                        //}
                        Log("[ " + DateTime.Now + " ] => [ Please Wait........Process is Running ]");

                        GroupStatus dataScrape = new GroupStatus();
                        List<string> lstGrpMemberProfileURLs = dataScrape.GetAllGrpMember(ref httpHelper, groupId);
                        //Dictionary<string,string> dicGrpMem=dataScrape.AddSpecificGroupUser(ref httpHelper, groupId, 25);

                        //GetGroupMemberInfo(ref httpHelper, lstGrpMemberProfileURLs);

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error : " + ex.StackTrace);
                    }
                }

                Log("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED of Finding Member With Username >>> " + _UserName + " ]");
                Log("-----------------------------------------------------------------------------------------------------------------------------------");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.StackTrace);
            }
        } 
        #endregion

        #region GetGroupMemberInfo
        public void GetGroupMemberInfo(ref GlobusHttpHelper httpHelper, List<string> lstGrpMemProfileURLs)
        {
            try
            {
                foreach (string item in lstGrpMemProfileURLs)
                {
                    try
                    {
                        if (!CrawlingLinkedInPage(item, ref httpHelper))
                        {
                            CrawlingPageDataSource(item, ref httpHelper);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error : " + ex.StackTrace);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.StackTrace);
            }
        } 
        #endregion

        #region CrawlingLinkedInPage
        public bool CrawlingLinkedInPage(string Url, ref GlobusHttpHelper HttpHelper)
        {
             //new Thread(() =>
             //     {

            Log("[ " + DateTime.Now + " ] => [ Parsing For URL : " + Url + " ]");

            //  Workbooks myExcelWorkbooks = ClsExcelData.myExcelWorkbooks;
            #region Data Initialization
            string GroupMemId = string.Empty;
            string Industry = string.Empty;
            string URLprofile = string.Empty;
            string firstname = string.Empty;
            string lastname = string.Empty;
            string location = string.Empty;
            string country = string.Empty;
            string postal = string.Empty;
            string phone = string.Empty;
            string USERemail = string.Empty;
            string code = string.Empty;
            string education1 = string.Empty;
            string education2 = string.Empty;
            string titlecurrent = string.Empty;
            string companycurrent = string.Empty;
            string titlepast1 = string.Empty;
            string companypast1 = string.Empty;
            string titlepast2 = string.Empty;
            string html = string.Empty;
            string companypast2 = string.Empty;
            string titlepast3 = string.Empty;
            string companypast3 = string.Empty;
            string titlepast4 = string.Empty;
            string companypast4 = string.Empty;
            string Recommendations = string.Empty;
            string Connection = string.Empty;
            string Designation = string.Empty;
            string Website = string.Empty;
            string Contactsettings = string.Empty;
            string recomandation = string.Empty;
            string memhead = string.Empty;

            string titleCurrenttitle = string.Empty;
            string titleCurrenttitle2 = string.Empty;
            string titleCurrenttitle3 = string.Empty;
            string titleCurrenttitle4 = string.Empty;
            string Skill = string.Empty;
            string TypeOfProfile = "Public";
            List<string> EducationList = new List<string>();
            string Finaldata = string.Empty;
            string EducationCollection = string.Empty;
            List<string> checkerlst = new List<string>();
            List<string> checkgrplist = new List<string>();
            string groupscollectin = string.Empty;
            string strFamilyName = string.Empty;
            string LDS_LoginID = string.Empty;
            string LDS_Websites = string.Empty;
            string LDS_UserProfileLink = string.Empty;
            string LDS_CurrentTitle = string.Empty;
            string LDS_Experience = string.Empty;
            string LDS_UserContact = string.Empty;
            string LDS_PastTitles = string.Empty;
            string LDS_HeadLineTitle = string.Empty;
            string Company = string.Empty;
            string degreeConnection = string.Empty;
            string website1 = string.Empty;
            string LDS_CurrentCompany = string.Empty;

            List<string> lstpasttitle = new List<string>();
            List<string> checkpasttitle = new List<string>();
            List<string> titleList = new List<string>();
            List<string> companyList = new List<string>();
            #endregion

            string stringSource = HttpHelper.getHtmlfromUrl1(new Uri(Url));

            if (stringSource.Contains("Sign in to LinkedIn"))
            {
                Log("[ " + DateTime.Now + " ] => [ Sign in to LinkedIn ]");

            }

            #region GroupMemId
            try
            {
                string[] gid = Url.Split('&');
                GroupMemId = gid[0].Replace("http://www.linkedin.com/profile/view?id=", string.Empty);
            }
            catch { }
            #endregion


            #region Name
            try
            {
                try
                {
                    //strFamilyName = stringSource.Substring(stringSource.IndexOf("fmt__profileUserFullName\":\""), (stringSource.IndexOf("i18n__expand_your_network_to_see_more", stringSource.IndexOf("\"See Full Name\",\"i18n_Edit\":\"Edit\",\"fmt__full_name\":\"")) - stringSource.IndexOf("\"See Full Name\",\"i18n_Edit\":\"Edit\",\"fmt__full_name\":\""))).Replace("\"See Full Name\",\"i18n_Edit\":\"Edit\",\"fmt__full_name\":\"", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace("}", string.Empty).Replace("]", string.Empty).Trim();
                    int startindex = stringSource.IndexOf("fmt__profileUserFullName\":\"");
                    string start = stringSource.Substring(startindex).Replace("fmt__profileUserFullName\":\"", "");
                    int endindex = start.IndexOf("\",\"");
                    string end = start.Substring(0, endindex).Replace("\\u002d", "-");
                    strFamilyName = end;
                    if (string.IsNullOrEmpty(strFamilyName))
                    {
                        strFamilyName = stringSource.Substring(stringSource.IndexOf("fmt__full_name\":"), (stringSource.IndexOf(",", stringSource.IndexOf("fmt__full_name\":")) - stringSource.IndexOf("fmt__full_name\":"))).Replace("fmt__full_name\":", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace("}", string.Empty).Replace("]", string.Empty).Trim();
                    }
                }
                catch
                {
                    try
                    {
                        strFamilyName = stringSource.Substring(stringSource.IndexOf("fmt__full_name\":"), (stringSource.IndexOf(",", stringSource.IndexOf("fmt__full_name\":")) - stringSource.IndexOf("fmt__full_name\":"))).Replace("fmt__full_name\":", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace("}", string.Empty).Replace("]", string.Empty).Trim();

                    }
                    catch { }
                }
            }
            catch { }

            if (strFamilyName == string.Empty)
            {
                try
                {
                    int startindex = stringSource.IndexOf("<title>");
                    string start = stringSource.Substring(startindex).Replace("<title>", string.Empty);
                    int endindex = start.IndexOf("</title>");
                    string end = start.Substring(0, endindex).Replace("\\u002d", "-").Replace("| LinkedIn", string.Empty).Trim();
                    strFamilyName = end;
                }
                catch { }
            }

            #endregion

            #region Namesplitation
            string[] NameArr = new string[5];
            if (strFamilyName.Contains(" "))
            {
                try
                {
                    NameArr = Regex.Split(strFamilyName, " ");
                }
                catch { }
            }
            #endregion

            #region FirstName
            try
            {
                firstname = NameArr[0];
            }
            catch { }
            #endregion

            #region LastName
            try
            {
                lastname = NameArr[1];
            }
            catch { }

            try
            {
                if (NameArr.Count() == 3)
                {
                    lastname = NameArr[1] + " " + NameArr[2];
                }
                else if (NameArr.Count() == 4)
                {
                    lastname = NameArr[1] + " " + NameArr[2] + " " + NameArr[3];
                }
                else if (NameArr.Count() == 5)
                {
                    lastname = NameArr[1] + " " + NameArr[2] + " " + NameArr[3] + " " + NameArr[4];
                }

                if (lastname.Contains("}]"))
                {

                    #region Name
                    try
                    {
                        try
                        {
                            strFamilyName = stringSource.Substring(stringSource.IndexOf("<span class=\"n fn\">")).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Trim();
                        }
                        catch
                        {
                            try
                            {
                                strFamilyName = stringSource.Substring(stringSource.IndexOf("fmt__full_name\":"), (stringSource.IndexOf(",", stringSource.IndexOf("fmt__full_name\":")) - stringSource.IndexOf("fmt__full_name\":"))).Replace("fmt__full_name\":", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Trim();

                            }
                            catch { }
                        }
                    }
                    catch { }
                    #endregion
                }
            }
            catch { }
            #endregion

            #region titlecurrent
            //try
            //{
                try
                {
                    try
                    {
                        //Company = stringSource.Substring(stringSource.IndexOf("visible\":true,\"memberHeadline"), (stringSource.IndexOf("memberID", stringSource.IndexOf("visible\":true,\"memberHeadline")) - stringSource.IndexOf("visible\":true,\"memberHeadline"))).Replace("visible\":true,\"memberHeadline", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Replace("   ", string.Empty).Trim();
                        // int startindex = stringSource.IndexOf("memberHeadline");
                        // string start = stringSource.Substring(startindex).Replace("memberHeadline", "");
                        //  int endindex = start.IndexOf("\",\"");
                        //  string end = start.Substring(0, endindex).Replace("\"", "").Replace(":", "");
                        //  titlecurrent = end.Replace("\\u002d", "-");
                        int startindex = stringSource.IndexOf("\"title_highlight\":\"");
                        string start = stringSource.Substring(startindex).Replace("\"title_highlight\":\"", "");
                        int endindex = start.IndexOf("\",\"");
                        string end = start.Substring(0, endindex).Replace("\u002d", string.Empty).Replace("Å", "A").Replace("\\u002d", "-").Replace("\"\n", "").Replace("\n", "").Replace(";", ",").Replace("&amp", "");
                        titlecurrent = end;
                    }
                    catch
                    {
                    }

                    //    if (string.IsNullOrEmpty(Company))
                    if (string.IsNullOrEmpty(titlecurrent))
                    {
                        try
                        {
                            //memberHeadline
                            //  titlecurrent = stringSource.Substring(stringSource.IndexOf("memberHeadline\":"), (stringSource.IndexOf(",", stringSource.IndexOf("memberHeadline\":")) - stringSource.IndexOf("memberHeadline\":"))).Replace("memberHeadline\":", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Replace("   ", string.Empty).Replace(":", "").Replace("&dsh;", "").Replace("&amp", "").Replace(";", "").Replace("u002d",string.Empty).Trim();

                            int startindex = stringSource.IndexOf("memberHeadline");
                            string start = stringSource.Substring(startindex).Replace("memberHeadline", "");
                            int endindex = start.IndexOf("\",\"");
                            string end = start.Substring(0, endindex).Replace("\"", "").Replace(":", "");
                            memhead = end.Replace("\\u002d", "-");
                        }
                        catch
                        {
                        }

                    }
                    if (string.IsNullOrEmpty(titlecurrent))
                    {
                        string[] cmpny = Regex.Split(stringSource, "trk=prof-exp-title' name='title' title='Find others with this title'>");
                        foreach (string item in cmpny)
                        {
                            try
                            {
                                try
                                {
                                    if (!item.Contains("<!DOCTYPE html>"))
                                    {
                                        int startindex = item.IndexOf("");
                                        string start = item.Substring(startindex);
                                        int endindex = start.IndexOf("</a>");
                                        string end = start.Substring(0, endindex).Replace("</a>", String.Empty);
                                        string titles = end.Trim();
                                        titleList.Add(titles);
                                        titleList = titleList.Distinct().ToList();
                                    }
                                }
                                catch
                                {


                                }
                            }
                            catch
                            {


                            }
                        }
                    }
                    if (titleList.Count > 0)
                    {
                        titlecurrent = titleList[0];
                    }
                    string[] strdesigandcompany1 = new string[4];
                    if (memhead.Contains(" at ") || memhead.Contains(" of "))
                    {
                        // titlecurrent = string.Empty;
                        companycurrent = string.Empty;

                        try
                        {
                            strdesigandcompany1 = Regex.Split(memhead, " at ");

                            if (strdesigandcompany1.Count() == 1)
                            {
                                strdesigandcompany1 = Regex.Split(memhead, " of ");
                            }
                        }
                        catch { }

                        try
                        {
                            titlecurrent = strdesigandcompany1[0];
                        }
                        catch { }

                    }


                    string[] strdesigandcompany = new string[4];
                    if (Company.Contains(" at ") || Company.Contains(" of "))
                    {
                        titlecurrent = string.Empty;
                        companycurrent = string.Empty;

                        try
                        {
                            strdesigandcompany = Regex.Split(Company, " at ");

                            if (strdesigandcompany.Count() == 1)
                            {
                                strdesigandcompany = Regex.Split(Company, " of ");
                            }
                        }
                        catch { }

                        #region Title
                        try
                        {
                            titlecurrent = strdesigandcompany[0];
                        }
                        catch { }
                        #endregion

                        #region Current Company
                        try
                        {
                            companycurrent = strdesigandcompany[1];
                        }
                        catch { }
                        #endregion
                    }

                }
                catch { }
            //}
            //catch{}

                #region PastCompany
                string[] companylist = Regex.Split(stringSource, "companyName\"");
                string AllComapny = string.Empty;

                string Companyname = string.Empty;
                foreach (string item in companylist)
                {
                    try
                    {
                        if (!item.Contains("<!DOCTYPE html>"))
                        {
                            Companyname = item.Substring(item.IndexOf(":"), (item.IndexOf(",", item.IndexOf(":")) - item.IndexOf(":"))).Replace(":", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Replace("}", string.Empty).Replace("]", string.Empty).Trim();
                            //Checklist.Add(item);
                            string items = item;
                            checkerlst.Add(Companyname);
                            checkerlst = checkerlst.Distinct().ToList();
                        }
                    }
                    catch { }
                }

                // string AllComapny = string.Empty;
                if (checkerlst.Count > 0)
                {
                    foreach (string item1 in checkerlst)
                    {
                        try
                        {
                            if (string.IsNullOrEmpty(AllComapny))
                            {
                                AllComapny = item1.Replace("}", "").Replace("]", "").Replace("&amp;", "&");
                            }
                            else
                            {
                                AllComapny = AllComapny + " : " + item1.Replace("}", "").Replace("]", "").Replace("&amp;", "&");
                            }
                        }
                        catch
                        {


                        }
                    }
                }
                if (string.IsNullOrEmpty(AllComapny))
                {
                    string[] allCompany = Regex.Split(stringSource, "trk=prof-exp-company-name\" name=\"company\" title=\"Find others who have worked at this company\">");
                    foreach (string item in allCompany)
                    {
                        if (!item.Contains("<!DOCTYPE html>"))
                        {
                            try
                            {
                                int startindex = item.IndexOf("");
                                string start = item.Substring(startindex);
                                int endindex = start.IndexOf("</a>");
                                string end = start.Substring(0, endindex).Replace("</a>", string.Empty);
                                string companies = end.Trim();
                                companyList.Add(companies);
                                companyList = companyList.Distinct().ToList();
                            }
                            catch
                            {

                            }
                        }

                    }
                    string[] allcompany1 = Regex.Split(stringSource, "trk=prof-exp-company-name\">");
                    foreach (string item in allcompany1)
                    {
                        if (!item.Contains("<!DOCTYPE html>") && !item.Contains("<span data-tracking=\"mcp_profile_sum\""))
                        {
                            try
                            {
                                int startindex = item.IndexOf("");
                                string start = item.Substring(startindex);
                                int endindex = start.IndexOf("</a>");
                                string end = start.Substring(0, endindex).Replace("</a>", string.Empty);
                                string companies = end.Replace("&amp", "&").Trim();
                                companyList.Add(companies);
                                companyList = companyList.Distinct().ToList();
                            }
                            catch
                            {

                            }
                        }
                    }
                    if (companyList.Count > 0)
                    {
                        foreach (string item1 in companyList)
                        {
                            try
                            {
                                if (string.IsNullOrEmpty(AllComapny))
                                {
                                    AllComapny = item1.Replace("}", "").Replace("]", "").Replace("&amp;", "&");
                                }
                                else
                                {
                                    AllComapny = AllComapny + " : " + item1.Replace("}", "").Replace("]", "").Replace("&amp;", "&");
                                }
                            }
                            catch
                            {


                            }
                        }
                    }

                }
                if (companyList.Count > 0)
                {
                    companycurrent = companyList[0];
                }
                //           if (companycurrent == string.Empty)
                //           {
                //               try
                //               {
                //                 companycurrent = checkerlst[0].ToString();
                //             }
                //             catch { }
                //        }

                #endregion
            #endregion Company

                #region Company
                try
                {
                    try
                    {
                        try
                        {
                            Company = stringSource.Substring(stringSource.IndexOf("visible\":true,\"memberHeadline"), (stringSource.IndexOf("memberID", stringSource.IndexOf("visible\":true,\"memberHeadline")) - stringSource.IndexOf("visible\":true,\"memberHeadline"))).Replace("visible\":true,\"memberHeadline", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Replace("   ", string.Empty).Trim();
                        }
                        catch
                        {
                        }

                        if (string.IsNullOrEmpty(Company))
                        {
                            try
                            {
                                //memberHeadline
                                Company = stringSource.Substring(stringSource.IndexOf("memberHeadline\":"), (stringSource.IndexOf(",", stringSource.IndexOf("memberHeadline\":")) - stringSource.IndexOf("memberHeadline\":"))).Replace("memberHeadline\":", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Replace("   ", string.Empty).Replace(":", "").Replace("&dsh;", "").Replace("&amp", "").Replace(";", "").Trim();
                            }
                            catch
                            {
                            }

                        }

                        string[] strdesigandcompany = new string[4];
                        if (Company.Contains(" at "))
                        {
                            try
                            {
                                strdesigandcompany = Regex.Split(Company, " at ");
                            }
                            catch { }

                            #region Title
                            try
                            {
                                LDS_HeadLineTitle = strdesigandcompany[0];
                                titlecurrent = strdesigandcompany[0];
                            }
                            catch { }
                            #endregion

                            #region Current Company
                            try
                            {
                                companycurrent = strdesigandcompany[1];
                            }
                            catch { }
                            #endregion
                        }
                    }
                    catch { }
                    #region PastCompany
                    string[] grpcompanylist = Regex.Split(stringSource, "companyName\"");
                    string grpAllComapny = string.Empty;

                    string grpCompanyname = string.Empty;
                    foreach (string item in grpcompanylist)
                    {
                        try
                        {
                            if (!item.Contains("<!DOCTYPE html>"))
                            {
                                grpCompanyname = item.Substring(item.IndexOf(":"), (item.IndexOf(",", item.IndexOf(":")) - item.IndexOf(":"))).Replace(":", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Trim();
                                //Checklist.Add(item);
                                string items = item;
                                checkerlst.Add(grpCompanyname);
                                checkerlst = checkerlst.Distinct().ToList();
                            }
                        }
                        catch { }
                    }
                    grpAllComapny = string.Empty;
                    foreach (string item1 in checkerlst)
                    {
                        if (string.IsNullOrEmpty(grpAllComapny))
                        {
                            grpAllComapny = item1.Replace("}", "").Replace("]", "").Replace("&amp;", "&");
                        }
                        else
                        {
                            grpAllComapny = grpAllComapny + " : " + item1.Replace("}", "").Replace("]", "").Replace("&amp;", "&");
                        }
                    }
                    #endregion
                #endregion Company

                    #region Education
                    try
                    {
                        //  string[] str_UniversityName = Regex.Split(stringSource, "link__school_name");
                        string[] str_UniversityName = Regex.Split(stringSource, "link__school_name_public");
                        foreach (string item in str_UniversityName)
                        {
                            try
                            {
                                string School = string.Empty;
                                string Degree = string.Empty;
                                string SessionEnd = string.Empty;
                                string SessionStart = string.Empty;
                                string Education = string.Empty;

                                if (!item.Contains("<!DOCTYPE html>"))
                                {
                                    try
                                    {
                                        try
                                        {
                                            int startindex = item.IndexOf("\"schoolName\":");
                                            string start = item.Substring(startindex).Replace("\"schoolName\":", string.Empty);
                                            int endindex = start.IndexOf(",");
                                            School = start.Substring(0, endindex).Replace("\\u002d", string.Empty).Replace(":", string.Empty).Replace("\"", string.Empty).Replace("\\t", string.Empty);
                                        }
                                        catch { }

                                        try
                                        {
                                            int startindex1 = item.IndexOf("degree");
                                            string start1 = item.Substring(startindex1).Replace("degree", "");
                                            int endindex1 = start1.IndexOf(",");
                                            Degree = start1.Substring(0, endindex1).Replace("\\u002d", string.Empty).Replace(":", string.Empty).Replace("\"", string.Empty).Replace("_highlight", string.Empty);
                                        }
                                        catch { }

                                        try
                                        {
                                            int startindex2 = item.IndexOf("enddate_my");
                                            string start2 = item.Substring(startindex2).Replace("enddate_my", "");
                                            int endindex2 = start2.IndexOf(",");
                                            SessionEnd = start2.Substring(0, endindex2).Replace("\\u002d", string.Empty).Replace(":", string.Empty).Replace("\"", string.Empty);
                                        }
                                        catch { }

                                        try
                                        {
                                            int startindex3 = item.IndexOf("startdate_my");
                                            string start3 = item.Substring(startindex3).Replace("startdate_my", "");
                                            int endindex3 = start3.IndexOf(",");
                                            SessionStart = start3.Substring(0, endindex3).Replace("\\u002d", string.Empty).Replace(":", string.Empty).Replace("\"", string.Empty);
                                        }
                                        catch { }
                                        //if (Degree == string.Empty)
                                        //{
                                        //    try
                                        //    {
                                        //        if (!item.Contains("\"degree\":\""))
                                        //        {
                                        //            int startindex4 = item.IndexOf("name\":");
                                        //            string start4 = item.Substring(startindex4).Replace("name\":", "");
                                        //            int endindex4 = start4.IndexOf(",");
                                        //            Degree = start4.Substring(0, endindex4).Replace("\\u002d", string.Empty).Replace(":", string.Empty).Replace("\"", string.Empty);
                                        //        }
                                        //    }
                                        //    catch { }
                                        //}

                                        if (SessionStart == string.Empty && SessionEnd == string.Empty)
                                        {
                                            Education = " [" + School + "] Degree: " + Degree;
                                        }
                                        else
                                        {
                                            Education = " [" + School + "] Degree: " + Degree + " Session: " + SessionStart + "-" + SessionEnd;
                                        }
                                        //University = item.Substring(item.IndexOf(":"), (item.IndexOf(",", item.IndexOf(":")) - item.IndexOf(":"))).Replace(":", string.Empty).Replace("\\u002d", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Trim();
                                    }
                                    catch { }
                                    EducationList.Add(Education);

                                }
                            }
                            catch { }
                        }

                        EducationList = EducationList.Distinct().ToList();
                        if (EducationList.Count == 0)
                        {
                            //string[] uniName = Regex.Split(stringSource, "trk=prof-edu-school-name' title=\"More details for this school\">");
                            //foreach (string item in uniName)
                            //{
                            //    try
                            //    {
                            //        string School = string.Empty;
                            //        string Degree = string.Empty;
                            //        string SessionEnd = string.Empty;
                            //        string SessionStart = string.Empty;
                            //        string Education = string.Empty;
                            //        if (!item.Contains("<!DOCTYPE html>"))
                            //        {
                            //            try
                            //            {
                            //                try
                            //                {
                            //                    int startindex = item.IndexOf("");
                            //                    string start = item.Substring(startindex);
                            //                    int endindex = start.IndexOf("</a>");
                            //                    School = start.Substring(0, endindex).Replace("\\u002d", string.Empty).Replace(":", string.Empty).Replace("\"", string.Empty).Replace("\\t", string.Empty).Trim();
                            //                }
                            //                catch
                            //                { }

                            //                try
                            //                {
                            //                    int startindex1 = item.IndexOf("degree");
                            //                    string start1 = item.Substring(startindex1).Replace("degree", "");
                            //                    int endindex1 = start1.IndexOf(",");
                            //                    Degree = start1.Substring(0, endindex1).Replace("\\u002d", string.Empty).Replace(":", string.Empty).Replace("\"", string.Empty).Replace("_highlight", string.Empty).Replace(">",string.Empty);
                            //                }
                            //                catch { }

                            //                try
                            //                {
                            //                    int startindex2 = item.IndexOf("<time datetime=");
                            //                    string start2 = item.Substring(startindex2).Replace("<time datetime=", string.Empty);
                            //                    int endindex2 = start2.IndexOf(">");
                            //                    SessionStart = start2.Substring(0, endindex2).Replace("\\u002d", string.Empty).Replace(":", string.Empty).Replace("\"", string.Empty).Replace("'",string.Empty);
                            //                }
                            //                catch { }

                            //                try
                            //                {
                            //                    int startindex2 = item.IndexOf("</time><time datetime=");
                            //                    string start2 = item.Substring(startindex2).Replace("</time><time datetime=", string.Empty);
                            //                    int endindex2 = start2.IndexOf(">");
                            //                    SessionEnd = start2.Substring(0, endindex2).Replace("\\u002d", string.Empty).Replace(":", string.Empty).Replace("\"", string.Empty).Replace(">",string.Empty).Replace("'",string.Empty);
                            //                }
                            //                catch { }

                            //                if (SessionStart == string.Empty && SessionEnd == string.Empty)
                            //                {
                            //                    Education = " [" + School + "] Degree: " + Degree;
                            //                }
                            //                else
                            //                {
                            //                    Education = " [" + School + "] Degree: " + Degree + " Session: " + SessionStart + "-" + SessionEnd;
                            //                }
                            //            }

                            //            catch
                            //            { }
                            //            EducationList.Add(Education);


                            //        }
                            //    }
                            //    catch { }

                            //}
                            string[] uniName1 = Regex.Split(stringSource, "trk=prof-edu-school-name' title=");
                            foreach (string item in uniName1)
                            {
                                try
                                {
                                    string School = string.Empty;
                                    string Degree = string.Empty;
                                    string SessionEnd = string.Empty;
                                    string SessionStart = string.Empty;
                                    string Education = string.Empty;
                                    if (!item.Contains("<!DOCTYPE html>"))
                                    {
                                        try
                                        {
                                            try
                                            {
                                                int startindex = item.IndexOf(">");
                                                string start = item.Substring(startindex);
                                                int endindex = start.IndexOf("</a>");
                                                School = start.Substring(0, endindex).Replace("\\u002d", string.Empty).Replace(":", string.Empty).Replace("\"", string.Empty).Replace("\\t", string.Empty).Replace(">", string.Empty).Trim();
                                            }
                                            catch
                                            { }

                                            try
                                            {
                                                int startindex1 = item.IndexOf("degree");
                                                string start1 = item.Substring(startindex1).Replace("degree", "");
                                                int endindex1 = start1.IndexOf(",");
                                                Degree = start1.Substring(0, endindex1).Replace("\\u002d", string.Empty).Replace(":", string.Empty).Replace("\"", string.Empty).Replace("_highlight", string.Empty).Replace(">", string.Empty);
                                                if (Degree.Contains("connection") || Degree.Contains("title") || Degree.Contains("firstName") || Degree.Contains("authToken"))
                                                {
                                                    Degree = string.Empty;
                                                }

                                            }
                                            catch { }

                                            try
                                            {
                                                int startindex2 = item.IndexOf("<time datetime=");
                                                string start2 = item.Substring(startindex2).Replace("<time datetime=", string.Empty);
                                                int endindex2 = start2.IndexOf(">");
                                                SessionStart = start2.Substring(0, endindex2).Replace("\\u002d", string.Empty).Replace(":", string.Empty).Replace("\"", string.Empty).Replace("'", string.Empty);
                                            }
                                            catch { }
                                            if (string.IsNullOrEmpty(SessionStart))
                                            {
                                                try
                                                {
                                                    int startindex3 = item.IndexOf("<span class=\"education-date\"><time>");
                                                    string start3 = item.Substring(startindex3).Replace("<span class=\"education-date\"><time>", string.Empty);
                                                    int endindex3 = start3.IndexOf("</time>");
                                                    SessionStart = start3.Substring(0, endindex3).Replace("</time>", string.Empty).Replace("\\u002d", string.Empty).Replace(":", string.Empty).Replace("\"", string.Empty).Replace(">", string.Empty).Replace("'", string.Empty);
                                                }
                                                catch
                                                { }

                                            }

                                            try
                                            {
                                                int startindex2 = item.IndexOf("<time> &#8211;");
                                                string start2 = item.Substring(startindex2).Replace("<time> &#8211;", string.Empty);
                                                int endindex2 = start2.IndexOf("</time>");
                                                SessionEnd = start2.Substring(0, endindex2).Replace("</time>", string.Empty).Replace("\\u002d", string.Empty).Replace(":", string.Empty).Replace("\"", string.Empty).Replace(">", string.Empty).Replace("'", string.Empty);

                                            }
                                            catch { }
                                            if (string.IsNullOrEmpty(SessionStart))
                                            {
                                                try
                                                {
                                                    int startindex3 = item.IndexOf("<span class=\"education-date\"><time>");
                                                    string start3 = item.Substring(startindex3).Replace("<span class=\"education-date\"><time>", string.Empty);
                                                    int endindex3 = start3.IndexOf("</time>");
                                                    SessionEnd = start3.Substring(0, endindex3).Replace("</time>", string.Empty).Replace("\\u002d", string.Empty).Replace(":", string.Empty).Replace("\"", string.Empty).Replace(">", string.Empty).Replace("'", string.Empty);
                                                }
                                                catch
                                                { }

                                            }


                                            if (SessionStart == string.Empty && SessionEnd == string.Empty)
                                            {
                                                Education = " [" + School + "] Degree: " + Degree;
                                            }
                                            else
                                            {
                                                if (Degree == string.Empty)
                                                {
                                                    Education = " [" + School + "] Session: " + SessionStart + "-" + SessionEnd;
                                                }
                                                else
                                                {
                                                    Education = " [" + School + "] Degree: " + Degree + " Session: " + SessionStart + "-" + SessionEnd;
                                                }

                                            }




                                        }

                                        catch
                                        { }
                                        EducationList.Add(Education);


                                    }
                                }
                                catch { }

                            }
                        }
                        EducationList = EducationList.Distinct().ToList();
                        if (EducationList.Count > 0)
                        {
                            foreach (string item in EducationList)
                            {
                                if (string.IsNullOrEmpty(EducationCollection))
                                {
                                    // EducationCollection = item.Replace("}", "").Replace("]", "").Replace("&amp;", "&");
                                    EducationCollection = item.Replace("}", "").Replace("&amp;", "&");
                                }
                                else
                                {
                                    // EducationCollection = EducationCollection + "  -  " + item.Replace("}", "").Replace("]", "").Replace("&amp;", "&");
                                    EducationCollection = EducationCollection + "  -  " + item.Replace("}", "").Replace("&amp;", "&");
                                }
                            }
                        }
                        // string University1 = stringSource.Substring(stringSource.IndexOf("schoolName\":"), (stringSource.IndexOf(",", stringSource.IndexOf("schoolName\":")) - stringSource.IndexOf("schoolName\":"))).Replace("schoolName\":", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Trim();

                    }

                    catch { }

                    #endregion Education

                    #region Email
                    try
                    {

                        string[] str_Email = Regex.Split(stringSource, "email\"");
                        USERemail = stringSource.Substring(stringSource.IndexOf("[{\"email\":"), (stringSource.IndexOf("}]", stringSource.IndexOf("[{\"email\":")) - stringSource.IndexOf("[{\"email\":"))).Replace("[{\"email\":", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace("}]", string.Empty).Trim();
                    }
                    catch (Exception ex)
                    {

                    }
                    if (string.IsNullOrEmpty(USERemail))
                    {
                        try
                        {
                            int startindex = stringSource.IndexOf("Email:");
                            string start = stringSource.Substring(startindex).Replace("Email:", "");
                            int endindex = start.IndexOf("Phone");
                            string end = start.Substring(0, endindex).Replace("\u003cbr", "").Replace("\\u003cbr\\u003e", "").Replace("\\n", "").Replace(" ", "");
                            USERemail = end;
                        }
                        catch
                        {
                        }
                    }
                    #endregion Email

                    #region Contact
                    try
                    {
                        int startindex = stringSource.IndexOf("Phone:");
                        string start = stringSource.Substring(startindex).Replace("Phone:", "");
                        int endindex = start.IndexOf("Skype");
                        string end = start.Substring(0, endindex).Replace("\\u003cbr\\u003e", "").Replace("\\u002d", "").Replace("\\n", "");
                        LDS_UserContact = end;
                    }
                    catch { }
                    if (string.IsNullOrEmpty(LDS_UserContact))
                    {
                        try
                        {
                            int startindex = stringSource.IndexOf("[{\"number\":\"");
                            string start = stringSource.Substring(startindex).Replace("[{\"number\":\"", "");
                            int endindex = start.IndexOf("\"}]");
                            string end = start.Substring(0, endindex).Replace("\\u003cbr\\u003e", "").Replace("\\u002d", "").Replace("\\n", "");
                            LDS_UserContact = end;

                        }
                        catch { }
                    }

                    #endregion

                    #region Website
                    string Web1 = string.Empty;
                    List<string> Websites = new List<string>();
                    try
                    {
                        int startindex = stringSource.IndexOf("\"websites\":");
                        string start = stringSource.Substring(startindex).Replace("\"websites\":", "");
                        int endindex = start.IndexOf("\"showTencent\":");
                        string end = start.Substring(0, endindex).Replace("[{\"", string.Empty).Replace("{", string.Empty).Trim();
                        Web1 = end;
                    }
                    catch
                    { }
                    string[] web = Regex.Split(Web1, "\"URL\":\"");
                    foreach (var items in web)
                    {
                       

                        try
                        {
                            int startindex = items.IndexOf("");
                            string start = items.Substring(startindex).Replace("\"", "");
                            int endindex = start.IndexOf("}");
                            string end = start.Substring(0, endindex).Replace("\\u002d", string.Empty).Replace("URL:", string.Empty);
                            Website = end;
                        }
                        catch { }

                      

                        try
                        {
                            if (Website.Contains("]") || Website.Contains("}") || Website.Contains("[") || Website.Contains("}"))
                            {
                                Website = Website.Replace("]", string.Empty).Replace("}", string.Empty).Replace("\\u002d", string.Empty).Replace("[", string.Empty).Replace("{", string.Empty).Trim();
                            }
                        }
                        catch { }
                        Websites.Add(Website);
                    }
                    string item2 = string.Empty;
                    website1 = Websites[0];
                    //foreach (var item in Websites)
                    int size = Websites.Count;

                    for (int i = 1; i <= size; i++)
                    {
                        try
                        {
                            website1 += " - " + Websites[i];
                        }
                        catch { }
                    }

                    #endregion Website

                    #region location
                    try
                    {
                        //location = stringSource.Substring(stringSource.IndexOf("Country\",\"fmt__location\":"), (stringSource.IndexOf("i18n_no_location_matches", stringSource.IndexOf("Country\",\"fmt__location\":")) - stringSource.IndexOf("Country\",\"fmt__location\":"))).Replace("Country\",\"fmt__location\":", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Trim();
                        int startindex = stringSource.IndexOf("fmt_location");
                        string start = stringSource.Substring(startindex).Replace("fmt_location\":\"", "");
                        int endindex = start.IndexOf("\"");
                        string end = start.Substring(0, endindex).Replace("\u002d", string.Empty).Replace("Å", "A").Replace("\\u002d", "-");
                        location = end;
                    }
                    catch (Exception ex)
                    {

                    }
                    if (location == string.Empty)
                    {
                        try
                        {
                            int startindex = stringSource.IndexOf("name='location' title=\"Find other members");
                            string start = stringSource.Substring(startindex).Replace("name='location' title=\"Find other members", string.Empty);
                            int startindex1 = start.IndexOf("\">");
                            string start1 = start.Substring(startindex1);
                            int endindex = start1.IndexOf("</a>");
                            string end = start1.Substring(0, endindex).Replace("\u002d", string.Empty).Replace("Å", "A").Replace("\\u002d", "-").Replace(">", string.Empty).Replace("/", string.Empty).Replace("\"", string.Empty).Trim();
                            location = end;
                        }
                        catch
                        { }
                    }

                    #endregion location

                    #region Country
                    try
                    {
                        int startindex = stringSource.IndexOf("\"geo_region\":");
                        if (startindex > 0)
                        {
                            string start = stringSource.Substring(startindex).Replace("\"geo_region\":", "");
                            int endindex = start.IndexOf("\"i18n_geo_region\":\"Location\"");
                            string end = start.Substring(0, endindex);
                            country = end;

                            string[] array = Regex.Split(end, "\"name\":\"");
                            array = array.Skip(1).ToArray();
                            foreach (string item in array)
                            {
                                try
                                {
                                    int startindex1 = item.IndexOf("\",\"");
                                    string strat1 = item.Substring(0, startindex1);
                                    country = strat1;
                                    break;
                                }
                                catch (Exception ex)
                                {

                                }
                            }

                        }
                    }
                    catch (Exception ex)
                    {

                    }

                    if (country == string.Empty)
                    {
                        try
                        {
                            string[] countLocation = location.Split(',');

                            if (countLocation.Count() == 2)
                            {
                                country = location.Split(',')[1];
                            }
                            else if (countLocation.Count() == 3)
                            {
                                country = location.Split(',')[2];
                            }


                        }
                        catch { }

                    }

                    #endregion

                    #region Industry
                    try
                    {
                        //Industry = stringSource.Substring(stringSource.IndexOf("fmt__industry_highlight\":"), (stringSource.IndexOf(",", stringSource.IndexOf("fmt__industry_highlight\":")) - stringSource.IndexOf("fmt__industry_highlight\":"))).Replace("fmt__industry_highlight\":", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Trim();
                        int startindex = stringSource.IndexOf("\"industry_highlight\":\"");
                        if (startindex > 0)
                        {
                            string start = stringSource.Substring(startindex).Replace("\"industry_highlight\":\"", "");
                            int endindex = start.IndexOf("\",");
                            string end = start.Substring(0, endindex).Replace("&amp;", "&");
                            Industry = end;
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    if (string.IsNullOrEmpty(Industry))
                    {
                        try
                        {
                            int startindex = stringSource.IndexOf("name=\"industry\" title=\"Find other members in this industry\">");
                            string start = stringSource.Substring(startindex).Replace("name=\"industry\" title=\"Find other members in this industry\">", string.Empty);
                            int endindex = start.IndexOf("</a>");
                            string end = start.Substring(0, endindex).Replace("</a>", string.Empty).Replace("&amp;", "&");
                            Industry = end;
                        }
                        catch
                        { }
                    }
                    #endregion Industry

                    #region Connection
                    try
                    {
                        //Connection = stringSource.Substring(stringSource.IndexOf("_count_string\":"), (stringSource.IndexOf(",", stringSource.IndexOf("_count_string\":")) - stringSource.IndexOf("_count_string\":"))).Replace("_count_string\":", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Trim();
                        int startindex = stringSource.IndexOf("\"numberOfConnections\":");
                        if (startindex > 0)
                        {
                            string start = stringSource.Substring(startindex).Replace("\"numberOfConnections\":", "");
                            int endindex = start.IndexOf("},\"");
                            string end = start.Substring(0, endindex).Replace("&amp;", "&").Replace("\"", "").Replace("connectionsBrowseable:true", "").Replace(",", "");
                            Connection = end;
                        }

                        if (startindex < 0)
                        {
                            int startindex1 = stringSource.IndexOf("overview-connections");
                            if (startindex1 > 0)
                            {
                                string start = stringSource.Substring(startindex1).Replace("overview-connections", "").Replace("\n", string.Empty).Replace("<p>", string.Empty).Replace("</p>", string.Empty);
                                int endindex = start.IndexOf("</strong>");
                                string end = start.Substring(0, endindex).Replace("&amp;", "&").Replace("\"", string.Empty).Replace("<strong>", string.Empty).Replace(">", string.Empty).Trim();
                                Connection = end;
                            }
                        }
                        if (string.IsNullOrEmpty(Connection))
                        {
                            try
                            {
                                int startindex1 = stringSource.IndexOf("class=\"member-connections\"><strong>");
                                string start = stringSource.Substring(startindex1).Replace("class=\"member-connections\"><strong>", "").Replace("\n", string.Empty).Replace("<p>", string.Empty).Replace("</p>", string.Empty);
                                int endindex = start.IndexOf("</strong>");
                                string end = start.Substring(0, endindex).Replace("&amp;", "&").Replace("\"", string.Empty).Replace("<strong>", string.Empty).Replace("<a href=#connections class=connections-link>", string.Empty).Replace("más de", string.Empty).Replace("</a>", string.Empty).Replace(">",string.Empty).Trim();
                                Connection = end;
                            }
                            catch
                            { }
                        }

                    }
                    catch (Exception ex)
                    {
                    }
                    #endregion Connection

                    #region Recommendation

                    try
                    {
                        string RecomnedUrl = string.Empty;
                        try
                        {
                            int startindex = stringSource.IndexOf("endorsements?id=");
                            string start = stringSource.Substring(startindex);
                            int endIndex = start.IndexOf("\"mem_pic\":");
                            RecomnedUrl = (start.Substring(0, endIndex).Replace(",", string.Empty).Replace("\"", string.Empty).Replace(":", string.Empty));

                        }
                        catch { }

                        string PageSource = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/profile/profile-v2-" + RecomnedUrl + ""));
                        string[] arrayRecommendedName = Regex.Split(PageSource, "headline");

                        if (arrayRecommendedName.Count() == 1)
                        {
                            arrayRecommendedName = Regex.Split(PageSource, "fmt__recommendeeFullName");
                        }


                        List<string> ListRecommendationName = new List<string>();

                        foreach (var itemRecomName in arrayRecommendedName)
                        {
                            try
                            {
                                if (!itemRecomName.Contains("Endorsements"))
                                {
                                    string Heading = string.Empty;
                                    string Name = string.Empty;

                                    try
                                    {
                                        int startindex = itemRecomName.IndexOf(":");
                                        string start = itemRecomName.Substring(startindex);
                                        int endIndex = start.IndexOf("\",");
                                        Heading = (start.Substring(0, endIndex).Replace("\"", string.Empty).Replace(":", string.Empty));
                                    }
                                    catch { }

                                    try
                                    {
                                        int startindex1 = itemRecomName.IndexOf("fmt__referrerfullName");
                                        string start1 = itemRecomName.Substring(startindex1);
                                        int endIndex1 = start1.IndexOf("\",");
                                        Name = (start1.Substring(0, endIndex1).Replace("\"", string.Empty).Replace("fmt__referrerfullName", string.Empty).Replace(":", string.Empty));
                                    }
                                    catch { }

                                    if (Name == string.Empty)
                                    {
                                        int startindex1 = itemRecomName.IndexOf("recommenderTitle\":");
                                        string start1 = itemRecomName.Substring(startindex1);
                                        int endIndex1 = start1.IndexOf("\",");
                                        Name = (start1.Substring(0, endIndex1).Replace("\"", string.Empty).Replace("recommenderTitle", string.Empty).Replace(":", string.Empty).Replace(",", string.Empty));
                                    }

                                    ListRecommendationName.Add(Name + " : " + Heading);

                                }
                            }
                            catch { }

                        }

                        foreach (var item in ListRecommendationName)
                        {
                            if (recomandation == string.Empty)
                            {
                                recomandation = item;
                            }
                            else
                            {
                                recomandation += "  -  " + item;
                            }
                        }

                    }

                    catch { }
                    if (string.IsNullOrEmpty(Recommendations))
                    {
                        List<string> ListRecommendationName = new List<string>();
                        string[] recommend = Regex.Split(stringSource, "trk=prof-exp-snippet-endorsement-name'>");
                        foreach (string item in recommend)
                        {
                            if (!item.Contains("<!DOCTYPE html>"))
                                try
                                {
                                    int startindex = item.IndexOf("");
                                    string start = item.Substring(startindex);
                                    int endindex = start.IndexOf("</a>");
                                    string end = start.Substring(0, endindex).Replace("</a>", string.Empty);
                                    string recmnd = end.Trim();
                                    ListRecommendationName.Add(recmnd);
                                }
                                catch
                                { }
                        }
                        foreach (string item in ListRecommendationName)
                        {
                            if (recomandation == string.Empty)
                            {
                                recomandation = item;
                            }
                            else
                            {
                                recomandation += "  -  " + item;
                            }
                        }
                    }

                    #endregion

                    #region Group
                    try
                    {
                        #region commented by Prabhat 17/05/13 1:27 PM
                        //string groupdata = string.Empty;
                        //int startindex = stringSource.IndexOf("\"groupsMpr\":");
                        //if (startindex > 0)
                        //{
                        //    string start = stringSource.Substring(startindex);
                        //    int endindex = start.IndexOf("},\"");
                        //    string end = start.Substring(0, endindex);
                        //    groupdata = end;
                        //}

                        //string[] CheckList = Regex.Split(groupdata, "\"name\"");
                        //CheckList = CheckList.Skip(1).ToArray();

                        //foreach (string item in CheckList)
                        //{
                        //    try
                        //    {
                        //        int endindexdata = item.IndexOf("\",");
                        //        string enddata = item.Substring(0, endindexdata).Replace("\"","").Replace(":","");
                        //        if(string.IsNullOrEmpty(groupscollectin))
                        //        {
                        //            groupscollectin = enddata;
                        //        }
                        //        else
                        //        {
                        //            groupscollectin = groupscollectin + ":" + enddata;
                        //        }
                        //    }
                        //    catch(Exception ex)
                        //    {

                        //    }
                        //} 
                        #endregion


                        string PageSource = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/profile/mappers?x-a=profile_v2_groups%2Cprofile_v2_follow%2Cprofile_v2_connections&x-p=profile_v2_discovery%2Erecords%3A4%2Ctop_card%2EprofileContactsIntegrationStatus%3A0%2Cprofile_v2_comparison_insight%2Edistance%3A1%2Cprofile_v2_right_fixed_discovery%2Eoffset%3A0%2Cprofile_v2_connections%2Edistance%3A1%2Cprofile_v2_right_fixed_discovery%2Erecords%3A4%2Cprofile_v2_network_overview_insight%2Edistance%3A1%2Cprofile_v2_right_top_discovery_teamlinkv2%2Eoffset%3A0%2Cprofile_v2_right_top_discovery_teamlinkv2%2Erecords%3A4%2Cprofile_v2_discovery%2Eoffset%3A0%2Cprofile_v2_summary_upsell%2EsummaryUpsell%3Atrue%2Cprofile_v2_network_overview_insight%2EnumConn%3A1668%2Ctop_card%2Etc%3Atrue&x-oa=bottomAliases&id=" + GroupMemId + "&locale=&snapshotID=&authToken=&authType=name&invAcpt=&notContactable=&primaryAction=&isPublic=false&sfd=true&_=1366115853014"));

                        string[] array = Regex.Split(PageSource, "href=\"/groupRegistration?");
                        string[] array1 = Regex.Split(PageSource, "groupRegistration?");
                        List<string> ListGroupName = new List<string>();
                        string SelItem = string.Empty;

                        foreach (var itemGrps in array1)
                        {
                            try
                            {
                                if (itemGrps.Contains("?gid=") && !itemGrps.Contains("<!DOCTYPE html")) //">Join
                                {
                                    if (itemGrps.IndexOf("?gid=") == 0)
                                    {
                                        try
                                        {

                                            int startindex = itemGrps.IndexOf("\"name\":");
                                            string start = itemGrps.Substring(startindex);
                                            int endIndex = start.IndexOf(",");
                                            ListGroupName.Add(start.Substring(0, endIndex).Replace("\"", string.Empty).Replace("amp", string.Empty).Replace("&", string.Empty).Replace(";", string.Empty).Replace("csrfToken", string.Empty).Replace("name:", string.Empty));
                                        }
                                        catch { }
                                    }
                                }
                            }
                            catch { }
                        }

                        foreach (var item in ListGroupName)
                        {
                            if (groupscollectin == string.Empty)
                            {
                                groupscollectin = item;
                            }
                            else
                            {
                                groupscollectin += "  -  " + item;
                            }
                        }

                    }
                    catch { }

                    #endregion

                    #region Experience
                    if (LDS_Experience == string.Empty)
                    {
                        try
                        {
                            string[] array = Regex.Split(stringSource, "title_highlight");
                            string exp = string.Empty;
                            string comp = string.Empty;
                            List<string> ListExperince = new List<string>();
                            List<string> ListCompany = new List<string>();
                            string SelItem = string.Empty;

                            foreach (var itemGrps in array)
                            {
                                try
                                {
                                    if (itemGrps.Contains("title_pivot") && !itemGrps.Contains("<!DOCTYPE html")) //">Join
                                    {
                                        try
                                        {
                                            int startindex = itemGrps.IndexOf("\":\"");
                                            string start = itemGrps.Substring(startindex);
                                            int endIndex = start.IndexOf(",");
                                            exp = (start.Substring(0, endIndex).Replace("\"", string.Empty).Replace(":", string.Empty).Replace("&amp", "&").Replace(";", string.Empty).Replace("\\u002d", "-").Replace("name:", string.Empty));

                                        }
                                        catch { }

                                        try
                                        {
                                            int startindex1 = itemGrps.IndexOf("companyName");
                                            string start1 = itemGrps.Substring(startindex1);
                                            int endIndex1 = start1.IndexOf(",");
                                            comp = (start1.Substring(0, endIndex1).Replace("\"", string.Empty).Replace("companyName", string.Empty).Replace(":", string.Empty).Replace(";", string.Empty).Replace("\\u002d", "-").Replace("name:", string.Empty));

                                        }
                                        catch { }

                                        if (Company == string.Empty)
                                        {
                                            Company = comp;
                                        }

                                        if (LDS_HeadLineTitle == string.Empty)
                                        {
                                            LDS_HeadLineTitle = exp;
                                        }

                                        ListExperince.Add(exp + ":" + comp);
                                        ListCompany.Add(comp);
                                        companycurrent = ListCompany[0];

                                    }
                                }
                                catch { }
                            }
                            if (ListExperince.Count > 0)
                            {
                                foreach (var item in ListExperince)
                                {
                                    if (LDS_Experience == string.Empty)
                                    {
                                        LDS_Experience = item;
                                    }
                                    else
                                    {
                                        LDS_Experience += "  -  " + item;
                                    }
                                }
                            }

                        }
                        catch { }
                        try
                        {
                            string[] array = Regex.Split(stringSource, "name='title' title='Find others with this title'>");
                            string exp = string.Empty;
                            string comp = string.Empty;
                            List<string> ListExperince = new List<string>();
                            List<string> ListCompany = new List<string>();
                            string SelItem = string.Empty;

                            foreach (var itemGrps in array)
                            {
                                try
                                {
                                    if (!itemGrps.Contains("<!DOCTYPE html")) //">Join
                                    {
                                        try
                                        {
                                            int startindex = itemGrps.IndexOf("");
                                            string start = itemGrps.Substring(startindex);
                                            int endIndex = start.IndexOf("</a>");
                                            exp = (start.Substring(0, endIndex).Replace("\"", string.Empty).Replace(":", string.Empty).Replace("&amp", "&").Replace(";", string.Empty).Replace("\\u002d", "-").Replace("</a>", string.Empty).Replace("name:", string.Empty));

                                        }
                                        catch { }

                                        try
                                        {
                                            int startindex1 = itemGrps.IndexOf("trk=prof-exp-company-name\">");
                                            string start1 = itemGrps.Substring(startindex1);
                                            int endIndex1 = start1.IndexOf("</a>");
                                            comp = (start1.Substring(0, endIndex1).Replace("trk=prof-exp-company-name\">", string.Empty).Replace(":", string.Empty).Replace(";", string.Empty).Replace("\\u002d", "-").Replace("</a>", string.Empty).Replace("name:", string.Empty)).Replace("\"", string.Empty);

                                        }
                                        catch { }

                                        if (Company == string.Empty)
                                        {
                                            Company = comp;
                                        }

                                        if (LDS_HeadLineTitle == string.Empty)
                                        {
                                            LDS_HeadLineTitle = exp;
                                        }

                                        ListExperince.Add(exp + ":" + comp);
                                        ListCompany.Add(comp);
                                        companycurrent = ListCompany[0];
                                        if (companycurrent.Contains("span data-tracking") || string.IsNullOrEmpty(companycurrent))
                                        {
                                            try
                                            {
                                                if (companyList.Count > 0)
                                                {
                                                    companycurrent = companyList[0];
                                                }
                                            }
                                            catch
                                            { }
                                        }

                                    }
                                }
                                catch { }
                            }
                            if (ListExperince.Count > 0)
                            {
                                foreach (var item in ListExperince)
                                {
                                    if (LDS_Experience == string.Empty)
                                    {
                                        LDS_Experience = item;
                                    }
                                    else
                                    {
                                        LDS_Experience += "  -  " + item;
                                    }
                                }
                            }

                        }
                        catch { }


                    }
                    #endregion

                    #region skill and Expertise
                    try
                    {
                        string[] strarr_skill = Regex.Split(stringSource, "endorse-item-name-text\"");
                        string[] strarr_skill1 = Regex.Split(stringSource, "fmt__skill_name\"");
                        if (strarr_skill.Count() >= 2)
                        {
                            foreach (string item in strarr_skill)
                            {
                                try
                                {
                                    if (!item.Contains("!DOCTYPE html"))
                                    {
                                        try
                                        {
                                            string Grp = item.Substring(item.IndexOf(">"), (item.IndexOf("<", item.IndexOf(">")) - item.IndexOf(">"))).Replace(">", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Replace("&amp", "&").Trim();
                                            checkgrplist.Add(Grp);
                                            checkgrplist.Distinct().ToList();
                                        }
                                        catch { }
                                    }

                                }
                                catch { }
                            }

                            if (checkgrplist.Count > 0)
                            {
                                foreach (string item in checkgrplist)
                                {
                                    if (string.IsNullOrEmpty(Skill))
                                    {
                                        Skill = item;
                                    }
                                    else
                                    {
                                        Skill = Skill + "  :  " + item;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (strarr_skill1.Count() >= 2)
                            {
                                try
                                {
                                    foreach (string skillitem in strarr_skill1)
                                    {
                                        if (!skillitem.Contains("!DOCTYPE html"))
                                        {
                                            try
                                            {
                                                // string Grp = skillitem.Substring(skillitem.IndexOf(":"), (skillitem.IndexOf("}", skillitem.IndexOf(":")) - skillitem.IndexOf(":"))).Replace(":", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Trim();
                                                //  checkgrplist.Add(Grp);
                                                //  checkgrplist.Distinct().ToList();
                                                int startindex = skillitem.IndexOf(":\"");
                                                string start = skillitem.Substring(startindex);
                                                int endindex = start.IndexOf("viewerEndorsementId");
                                                if (!start.Contains("viewerEndorsementId"))
                                                {
                                                    int endindex1 = start.IndexOf("\"}");
                                                    string end1 = skillitem.Substring(0, endindex1).Replace("\"}", "").Replace("\"", "").Replace("\":", "").Replace(",", "").Replace(":", "").Replace("\\u002d", "");
                                                    string Grp1 = end1;
                                                    checkgrplist.Add(Grp1);
                                                    checkgrplist.Distinct().ToList();
                                                }
                                                else
                                                {
                                                    string end = skillitem.Substring(0, endindex).Replace("viewerEndorsementId", string.Empty).Replace("\"", "").Replace("\":", "").Replace(",", "").Replace(":", "").Replace("\\u002d", "");
                                                    string Grp = end;
                                                    checkgrplist.Add(Grp);
                                                    checkgrplist.Distinct().ToList();
                                                }
                                            }
                                            catch { }
                                        }
                                    }

                                    foreach (string item in checkgrplist)
                                    {
                                        if (string.IsNullOrEmpty(Skill))
                                        {
                                            Skill = item;
                                        }
                                        else
                                        {
                                            Skill = Skill + "  :  " + item;
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {

                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }

                    #endregion

                    #region Pasttitle
                    string[] pasttitles = Regex.Split(stringSource, "title_highlight");
                    string pstTitlesitem = string.Empty;
                    pasttitles = pasttitles.Skip(1).ToArray();
                    foreach (string item in pasttitles)
                    {
                        try
                        {
                            if (!item.Contains("<!DOCTYPE html>") && !item.Contains("Tip: You can also search by keyword"))
                            {
                                try
                                {
                                    string[] Past_Ttl = Regex.Split(item, ",");
                                    pstTitlesitem = Past_Ttl[0].Replace(":", string.Empty).Replace("\"", string.Empty).Replace("\\u002d", "-").Replace("&amp;", "&");
                                }
                                catch { }

                                if (string.IsNullOrEmpty(LDS_PastTitles))
                                {
                                    LDS_PastTitles = pstTitlesitem;
                                }
                                else if (LDS_PastTitles.Contains(pstTitlesitem))
                                {
                                    continue;
                                }
                                else
                                {
                                    LDS_PastTitles = LDS_PastTitles + "  :  " + pstTitlesitem;
                                }

                            }

                        }
                        catch
                        {
                        }
                    }
                    if (string.IsNullOrEmpty(LDS_PastTitles))
                    {
                        try
                        {
                            foreach (string item in titleList)
                            {
                                LDS_PastTitles = LDS_PastTitles + " : " + item;
                            }

                        }
                        catch
                        { }
                    }


                    if (string.IsNullOrEmpty(LDS_PastTitles))
                    {
                        if (stringSource.Contains("id=\"overview-summary-past\">"))
                        {
                            string _tempPageSourcePastTitle = Utils.getBetween(stringSource, "id=\"overview-summary-past\">", "</tr>");
                            string[] pasttitles1 = Regex.Split(_tempPageSourcePastTitle, "dir=\"auto\">");
                            string pstTitlesitem1 = string.Empty;
                            pasttitles1 = pasttitles1.Skip(1).ToArray();
                            foreach (string item in pasttitles1)
                            {
                                try
                                {
                                    if (!item.Contains("<!DOCTYPE html>") && !item.Contains("Tip: You can also search by keyword"))
                                    {
                                        try
                                        {
                                            string[] Past_Ttl = Regex.Split(item, "<");
                                            pstTitlesitem1 = Past_Ttl[0].Replace(":", string.Empty).Replace("\"", string.Empty).Replace("\\u002d", "-").Replace("&amp;", "&");
                                        }
                                        catch { }

                                        if (string.IsNullOrEmpty(LDS_PastTitles))
                                        {
                                            LDS_PastTitles = pstTitlesitem1;
                                        }
                                        else if (LDS_PastTitles.Contains(pstTitlesitem1))
                                        {
                                            continue;
                                        }
                                        else
                                        {
                                            LDS_PastTitles = LDS_PastTitles + "  :  " + pstTitlesitem1;
                                        }

                                    }

                                }
                                catch
                                {
                                }
                            }
                        }
                    
                    
                    }
                    #endregion

                    #region FullUrl
                    try
                    {
                        string[] UrlFull = System.Text.RegularExpressions.Regex.Split(Url, "&authType");
                        LDS_UserProfileLink = UrlFull[0];
                        LDS_UserProfileLink = Url;
                        //  LDS_UserProfileLink = stringSource.Substring(stringSource.IndexOf("canonicalUrl\":"), (stringSource.IndexOf(",", stringSource.IndexOf("canonicalUrl\":")) - stringSource.IndexOf("canonicalUrl\":"))).Replace("canonicalUrl\":", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Trim();
                    }
                    catch { }
                    #endregion

                    #region Current Title Current Company

                    try
                    {
                        int startindex = stringSource.IndexOf("memberHeadline");
                        if (startindex < 0)
                        {
                            try
                            {
                                int startindex1 = stringSource.IndexOf("<p class=\"title \">");
                                string start1 = stringSource.Substring(startindex1).Replace("<p class=\"title \">", string.Empty);
                                int endindex1 = start1.IndexOf("</p>");
                                string end1 = start1.Substring(0, endindex1).Replace("</p>", string.Empty).Replace("&#xf3;", "ó").Replace("&#xf1;", "ñ");
                                LDS_HeadLineTitle = end1.Trim();
                            }
                            catch
                            { }
                        }
                        else
                        {
                            try
                            {
                                string start = stringSource.Substring(startindex).Replace("memberHeadline", string.Empty);
                                int endindex = start.IndexOf("\",\"");
                                string end = start.Substring(0, endindex).Replace("\"", "").Replace(":", string.Empty);
                                LDS_HeadLineTitle = end.Replace("\\u002d", "-");
                            }
                            catch
                            { }
                        }
                        
                    }
                    catch { }
                    if (LDS_HeadLineTitle.Contains(" at ") || LDS_HeadLineTitle.Contains(" of "))
                    {
                        //  titlecurrent = string.Empty;
                        string[] strdesigandcompany1 = new string[4];
                        //    companycurrent = string.Empty;

                        try
                        {
                            strdesigandcompany1 = Regex.Split(LDS_HeadLineTitle, " at ");

                            if (strdesigandcompany1.Count() == 1)
                            {
                                strdesigandcompany1 = Regex.Split(LDS_HeadLineTitle, " of ");
                            }
                        }
                        catch { }

                        try
                        {
                            if (string.IsNullOrEmpty(companycurrent))
                            {
                                companycurrent = strdesigandcompany1[1];
                            }
                            if (companycurrent.Contains("span data-tracking") || string.IsNullOrEmpty(companycurrent))
                            {
                                if (companyList.Count > 0)
                                {
                                    companycurrent = companyList[0];
                                }
                            }
                        }
                        catch { }
                    }

                    try
                    {
                        try
                        {
                            Company = stringSource.Substring(stringSource.IndexOf("visible\":true,\"memberHeadline"), (stringSource.IndexOf("memberID", stringSource.IndexOf("visible\":true,\"memberHeadline")) - stringSource.IndexOf("visible\":true,\"memberHeadline"))).Replace("visible\":true,\"memberHeadline", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Replace("u002d", string.Empty).Replace("   ", string.Empty).Trim();
                        }
                        catch
                        {
                        }

                        if (string.IsNullOrEmpty(Company))
                        {
                            try
                            {
                                Company = stringSource.Substring(stringSource.IndexOf("memberHeadline\":"), (stringSource.IndexOf(",", stringSource.IndexOf("memberHeadline\":")) - stringSource.IndexOf("memberHeadline\":"))).Replace("memberHeadline\":", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Replace("   ", string.Empty).Replace(":", "").Replace("&dsh;", "").Replace("&amp", "").Replace(";", "").Replace("u002d", string.Empty).Trim();
                            }
                            catch
                            {
                            }

                        }

                        string[] strdesigandcompany = new string[4];
                        if (Company.Contains(" at "))
                        {
                            try
                            {
                                strdesigandcompany = Regex.Split(Company, " at ");
                            }
                            catch { }

                            #region Title
                            try
                            {
                                LDS_HeadLineTitle = strdesigandcompany[0].Replace("\\u002d", "-");
                            }
                            catch { }
                            #endregion

                            #region Current Company
                            try
                            {
                                LDS_CurrentCompany = strdesigandcompany[1];
                            }
                            catch { }
                            #endregion
                        }
                    }
                    catch { }



                    #endregion Company

                    #region degreeConnection
                    //try
                    //{
                    //    int startindex = stringSource.IndexOf("text_plain__NAME_is_");
                    //    string start = stringSource.Substring(startindex).Replace("text_plain__NAME_is_", string.Empty);
                    //    int endindex = start.IndexOf("_key");
                    //    string end = start.Substring(0, endindex).Replace("_key", string.Empty);
                    //    degreeConnection = end.Replace("_", " ").Trim();
                    //    if (degreeConnection.Contains("your connection"))
                    //    {
                    //        degreeConnection = "1st degree contact";
                    //    }
                    //}
                    //catch
                    //{ }
                    //if (string.IsNullOrEmpty(degreeConnection))
                    //{
                    //    try
                    //    {
                    //        int startindex = stringSource.IndexOf("class=\"fp-degree-icon\"><abbr title=\"");
                    //        string start = stringSource.Substring(startindex).Replace("class=\"fp-degree-icon\"><abbr title=\"", string.Empty);
                    //        int startindex1 = start.IndexOf("class=\"degree-icon \">");
                    //        string start1 = start.Substring(startindex1);
                    //        int endindex = start1.IndexOf("<sup>");
                    //        string end = start1.Substring(0, endindex).Replace("class=\"degree-icon \">", string.Empty).Replace("\"", string.Empty).Replace("<sup>", string.Empty);
                    //        degreeConnection = end.Replace("_", " ").Trim();
                    //    }
                    //    catch
                    //    { }
                    //}

                    string[] arr = Regex.Split(stringSource, "<span class=\"fp-degree-icon\">");
                    arr = arr.Skip(1).ToArray();
                    foreach (string tempItem in arr)
                    {
                        if (tempItem.Contains("class=\"degree-icon \">"))
                        {
                            int startindex = tempItem.IndexOf("class=\"degree-icon \">");
                            string start = tempItem.Substring(startindex).Replace("class=\"degree-icon \">", string.Empty);
                            int endIndex = start.IndexOf("</abbr>");
                            string end = start.Substring(0, endIndex).Replace("</abbr>", string.Empty).Replace("</sup>", string.Empty).Replace("<sup>", string.Empty);
                            degreeConnection = end.Trim();
                            break;
                        }
                    }

                    if (string.IsNullOrEmpty(degreeConnection))
                    {
                        try
                        {
                            int startindex = stringSource.IndexOf("class=\"degree-icon \">");
                            string start = stringSource.Substring(startindex).Replace("class=\"degree-icon \">", string.Empty);
                            int endIndex = start.IndexOf("</abbr>");
                            string end = start.Substring(0, endIndex).Replace("</abbr>", string.Empty).Replace("</sup>", string.Empty).Replace("<sup>", string.Empty);
                            degreeConnection = end.Trim();
                        }
                        catch
                        { }
                    }
                    #endregion degreeConnection

                    if (firstname == string.Empty) firstname = "LinkedIn";
                    if (lastname == string.Empty) lastname = "Member";
                    if (LDS_HeadLineTitle == string.Empty) LDS_HeadLineTitle = "--";
                    if (titlecurrent == string.Empty) titlecurrent = "--";
                    if (Company == string.Empty) Company = "--";
                    if (Connection == string.Empty) Connection = "--";
                    if (recomandation == string.Empty) recomandation = "--";
                    if (Skill == string.Empty) Skill = "--";
                    if (LDS_Experience == string.Empty) LDS_Experience = "--";
                    if (EducationCollection == string.Empty) EducationCollection = "--";
                    if (groupscollectin == string.Empty) groupscollectin = "--";
                    if (USERemail == string.Empty) USERemail = "--";
                    if (LDS_UserContact == string.Empty) LDS_UserContact = "--";
                    if (grpAllComapny == string.Empty) grpAllComapny = "--";
                    if (location == string.Empty) location = "--";
                    if (country == string.Empty) country = "--";
                    if (Industry == string.Empty) Industry = "--";
                    //   if (Website == string.Empty) Website = "--";
                    if (website1 == string.Empty) website1 = "--";
                    if (degreeConnection == string.Empty) degreeConnection = "--";
                    if (companycurrent == string.Empty) companycurrent = "--";
                    //  LDS_LoginID = _UserName;
                    LDS_LoginID = UN;
                    //SearchCriteria.LoginID;
                    //LDS_LoginID = UserDetails.UserName;
                    // string FinalData = firstname + "," + lastname + "," + Company.Replace(",", ";") + "," + titlecurrent.Replace(",", ";") + "," + Skill.Replace(",", ";") + "," + country.Replace(",", ";") + "," + location.Replace(",", ";") + "," + Website.Replace(",", ";") + "," + USERemail.Replace(",", ";") + "," + Industry.Replace(",", ";") + "," + groupscollectin.Replace(",", ";") + "," + EducationCollection.Replace(",", ";") + "," + recomandation.Replace(",", ";") + "," + Connection.Replace(",", ";") + "," + groupscollectin.Replace(",", ";") + "," + URLprofile.Replace(",", ";") + "," + UserDetails.UserName + "," + TypeOfProfile + ",";

                    string LDS_FinalData = TypeOfProfile + "," + LDS_UserProfileLink + "," + firstname + "," + lastname + "," + Company.Replace(",", ";") + "," + titlecurrent.Replace(",", ";") + "," + companycurrent.Replace(",", ";") + "," + Connection.Replace(",", ";") + "," + recomandation.Replace(",", string.Empty) + "," + Skill.Replace(",", ";") + "," + LDS_Experience.Replace(",", string.Empty) + "," + EducationCollection.Replace(",", ";") + "," + groupscollectin.Replace(",", ";") + "," + USERemail.Replace(",", ";") + "," + LDS_UserContact.Replace(",", ";") + "," + LDS_PastTitles + "," + grpAllComapny.Replace(",", ";") + "," + country.Replace(",", ";") + "," + location.Replace(",", ";") + "," + Industry.Replace(",", ";") + "," + Website.Replace(",", ";") + "," + LDS_LoginID + ",";// +TypeOfProfile + ",";

                    //if (!string.IsNullOrEmpty(firstname))
                    //{
                    //    Log(LDS_FinalData);
                    //}
                    //else
                    //{
                    //    Log("No Data For URL : " + Url);
                    //    GlobusFileHelper.AppendStringToTextfileNewLineWithCarat(Url, Globals.DesktopFolder + "\\UnScrapedList.txt");
                    //}

                    // if (SearchCriteria.starter)

                    //string tempFinalData = LDS_FinalData.Replace(";", "").Replace(LDS_UserProfileLink, "").Replace(TypeOfProfile, "").Replace(",", "").Replace(LDS_LoginID, "").Trim();

                    //if (!string.IsNullOrEmpty(tempFinalData))
                    //{
                    //AppFileHelper.AddingLinkedInDataToCSVFile(LDS_FinalData, Globals.path_ScrappedMembersFromGroup);
                    string CSVHeader = "ProfileType" + "," + "Degree Connection" + "," + "UserProfileLink" + "," + "FirstName" + "," + "LastName" + "," + "HeadLineTitle" + "," + "Current Title " + "," + "Current Company" + "," + "Connection" + "," + "Recommendations " + "," + "SkillAndExpertise " + "," + "Experience " + "," + " Education" + "," + "Groups" + "," + "UserEmail" + "," + "UserContactNumber" + "," + "PastTitles" + "," + "PastCompany" + "," + "Location" + "," + "Country" + "," + "Industry" + "," + "WebSites" + "," + "LinkedInLoginID" + ",";
                    string CSV_Content = TypeOfProfile.Replace(",", ";") + "," + degreeConnection.Replace(",", ";") + "," + LDS_UserProfileLink.Replace(",", ";") + "," + firstname.Replace(",", ";") + "," + lastname.Replace(",", ";") + "," + LDS_HeadLineTitle.Replace(",", ";") + "," + titlecurrent.Replace(",", ";") + "," + companycurrent.Replace(",", ";").Replace("002", "-") + "," + Connection.Replace(",", ";") + "," + recomandation.Replace(",", ";") + "," + Skill.Replace(",", ";") + "," + LDS_Experience.Replace(",", ";") + "," + EducationCollection.Replace(",", ";") + "," + groupscollectin.Replace(",", ";") + "," + USERemail.Replace(",", ";") + "," + LDS_UserContact.Replace(",", ";") + "," + LDS_PastTitles.Replace(",", ";") + "," + grpAllComapny.Replace(",", ";") + "," + location.Replace(",", ";") + "," + country.Replace(",", ";") + "," + Industry.Replace(",", ";") + "," + website1.Replace(",", ";") + "," + LDS_LoginID.Replace(",", ";");

                    //string CSV_Content = TypeOfProfile + "," + LDS_UserProfileLink + "," + firstname + "," + lastname + "," + Company.Replace(",", ";") + "," + titlecurrent.Replace(",", ";") + "," + companycurrent.Replace(",", ";") + "," + Connection.Replace(",", ";") + "," + recomandation.Replace(",", string.Empty) + "," + Skill.Replace(",", ";") + "," + LDS_Experience.Replace(",", string.Empty) + "," + EducationCollection.Replace(",", ";") + "," + groupscollectin.Replace(",", ";") + "," + USERemail.Replace(",", ";") + "," + LDS_UserContact.Replace(",", ";") + "," + LDS_PastTitles + "," + AllComapny.Replace(",", ";") + "," + country.Replace(",", ";") + "," + location.Replace(",", ";") + "," + Industry.Replace(",", ";") + "," + Website.Replace(",", ";") + "," + LDS_LoginID + ",";// +TypeOfProfile + ",";


                    CSVUtilities.ExportDataCSVFile(CSVHeader, CSV_Content, Globals.path_ScrappedMembersFromGroup);

                    Log("[ " + DateTime.Now + " ] => [ Data Saved In CSV File With URL >>> " + LDS_UserProfileLink + " ]");

                    return true;
                    //}
                }
                catch { };
                return false;
        }
   
          
       

        #endregion




        #region CrawlingPageDataSource
      public void CrawlingPageDataSource(string Url, ref GlobusHttpHelper HttpHelper)    
      {
            // if (SearchCriteria.starter)
            {
                // if (SearchCriteria.starter)
                {
                    try
                    {
                        Log("[ " + DateTime.Now + " ] => [ Start Parsing Process ]");

                        #region Data Initialization

                        string Industry = string.Empty;
                        string URLprofile = string.Empty;
                        string firstname = string.Empty;
                        string lastname = string.Empty;
                        string location = string.Empty;
                        string country = string.Empty;
                        string postal = string.Empty;
                        string phone = string.Empty;
                        string USERemail = string.Empty;
                        string code = string.Empty;
                        string education1 = string.Empty;
                        string education2 = string.Empty;
                        string titlecurrent = string.Empty;
                        string companycurrent = string.Empty;
                        string titlepast1 = string.Empty;
                        string companypast1 = string.Empty;
                        string titlepast2 = string.Empty;
                        string html = string.Empty;
                        string companypast2 = string.Empty;
                        string titlepast3 = string.Empty;
                        string companypast3 = string.Empty;
                        string titlepast4 = string.Empty;
                        string companypast4 = string.Empty;
                        string Recommendations = string.Empty;
                        string Connection = string.Empty;
                        string Designation = string.Empty;
                        string Website = string.Empty;
                        string Contactsettings = string.Empty;
                        string recomandation = string.Empty;

                        string titleCurrenttitle = string.Empty;
                        string titleCurrenttitle2 = string.Empty;
                        string titleCurrenttitle3 = string.Empty;
                        string titleCurrenttitle4 = string.Empty;
                        string Skill = string.Empty;
                        string TypeOfProfile = "Public1";

                        string Finaldata = string.Empty;
                        #endregion

                        #region LDS_DataInitialization
                        string LDS_FirstName = string.Empty;
                        string LDS_LastName = string.Empty;
                        string LDS_UserProfileLink = string.Empty;
                        string LDS_HeadLineTitle = string.Empty;
                        string LDS_CurrentTitle = string.Empty;
                        string LDS_PastTitles = string.Empty;
                        string LDS_Loction = string.Empty;
                        string LDS_Country = string.Empty;
                        string LDS_Connection = string.Empty;
                        string LDS_Recommendations = string.Empty;
                        string LDS_SkillAndExpertise = string.Empty;
                        string LDS_Education = string.Empty;
                        string LDS_Experience = string.Empty;
                        string LDS_ProfileType = "Public";
                        string LDS_Groups = string.Empty;
                        string LDS_UserEmail = string.Empty;
                        string LDS_UserContactNumber = string.Empty;
                        string LDS_CurrentCompany = string.Empty;
                        string LDS_PastCompany = string.Empty;
                        string LDS_LoginID = string.Empty;
                        string LDS_Websites = string.Empty;
                        string LDS_Industry = string.Empty;
                        #endregion

                        #region Chilkat Initialization



                        Chilkat.Http http = new Chilkat.Http();

                        ///Chilkat Http Request to be used in Http Post...
                        Chilkat.HttpRequest req = new Chilkat.HttpRequest();
                        Chilkat.HtmlUtil htmlUtil = new Chilkat.HtmlUtil();

                        // Any string unlocks the component for the 1st 30-days.
                        bool success = http.UnlockComponent("THEBACHttp_b3C9o9QvZQ06");
                        if (success != true)
                        {
                            Console.WriteLine(http.LastErrorText);
                            return;
                        }

                        http.CookieDir = "memory";
                        http.SendCookies = true;
                        http.SaveCookies = true;

                        html = HttpHelper.getHtmlfromUrl1(new Uri(Url));

                        html = htmlUtil.EntityDecode(html);

                        ////  Convert the HTML to XML:
                        Chilkat.HtmlToXml htmlToXml = new Chilkat.HtmlToXml();
                        Chilkat.HtmlToXml htmlToXml1 = new Chilkat.HtmlToXml();
                        Chilkat.HtmlToXml htmlToXml2 = new Chilkat.HtmlToXml();
                        success = htmlToXml.UnlockComponent("THEBACHtmlToXml_7WY3A57sZH3O");
                        if ((success != true))
                        {
                            Console.WriteLine(htmlToXml.LastErrorText);
                            return;
                        }


                        string xHtml = null;
                        string xHtml1 = null;
                        //string xHtml2 = null;

                        htmlToXml.Html = html;
                        xHtml = htmlToXml.ToXml();

                        Chilkat.Xml xml = new Chilkat.Xml();
                        xml.LoadXml(xHtml);

                        ////  Iterate over all h1 tags:
                        Chilkat.Xml xNode = default(Chilkat.Xml);
                        Chilkat.Xml xBeginSearchAfter = default(Chilkat.Xml);


                        #endregion

                        #region for paRSING
                        List<string> list = new List<string>();
                        List<string> Grouplist = new List<string>();
                        List<string> listtitle = new List<string>();
                        List<string> Currentlist = new List<string>();
                        List<string> Skilllst = new List<string>();
                        list.Clear();

                        //new parshing code 

                        List<string> TempFirstName = objChilkat.GetDataTagAttributewithId(html, "div", "name-container");




                        xBeginSearchAfter = null;

                        xNode = xml.SearchForTag(xBeginSearchAfter, "dt");


                        Grouplist.Clear();
                        xBeginSearchAfter = null;
                        #region parsergroup
                        xNode = xml.SearchForAttribute(xBeginSearchAfter, "div", "class", "group-data");

                        while ((xNode != null))
                        {
                            Finaldata = xNode.AccumulateTagContent("text", "/text");

                            Grouplist.Add(Finaldata);


                            string[] tempC1 = Regex.Split(Finaldata, " at ");

                            xBeginSearchAfter = xNode;
                            xNode = xml.SearchForAttribute(xBeginSearchAfter, "div", "class", "group-data");

                        }

                        int groupcounter = 0;
                        string AllGRoup = string.Empty;
                        foreach (string item in Grouplist)
                        {
                            if (item.Contains("Join"))
                            {
                                if (groupcounter == 0)
                                {
                                    LDS_Groups = item;
                                    groupcounter++;
                                }
                                else
                                {
                                    LDS_Groups = AllGRoup + ";" + item;
                                }

                            }

                        }
                        #endregion

                        #region parserSkill
                        xNode = xml.SearchForTag(xBeginSearchAfter, "dt");

                        Skilllst.Clear();
                        xBeginSearchAfter = null;

                        xNode = xml.SearchForAttribute(xBeginSearchAfter, "div", "id", "profile-skills");

                        while ((xNode != null))
                        {
                            Finaldata = xNode.AccumulateTagContent("text", "/text");
                            if (Finaldata.Contains("extlib: _toggleclass"))
                            {
                                try
                                {
                                    string[] Temp = Finaldata.Split(';');
                                    LDS_SkillAndExpertise = Temp[4];
                                }
                                catch { }

                            }
                            else
                            {
                                try
                                {
                                    LDS_SkillAndExpertise = Finaldata.Replace("Skills & Expertise", " ");
                                    Skilllst.Add(Finaldata);
                                }
                                catch { }
                            }


                            xBeginSearchAfter = xNode;
                            xNode = xml.SearchForAttribute(xBeginSearchAfter, "div", "id", "profile-skills");

                        }

                        if (LDS_SkillAndExpertise.Contains(" Endorsements LI.i18n.register('section_skills_person_endorsed_tmpl"))
                        {
                            LDS_SkillAndExpertise = string.Empty;
                        }

                        Skilllst.Distinct();
                        #endregion


                        #region UrlProfile
                        try
                        {
                            if (html.Contains("webProfileURL"))
                            {
                                int FirstPointForProfileURL = html.IndexOf("webProfileURL");
                                string FirstSubStringForProfileURL = html.Substring(FirstPointForProfileURL);
                                int SecondPointForProfileURL = FirstSubStringForProfileURL.IndexOf(">");
                                int ThirdPointForProfileURL = FirstSubStringForProfileURL.IndexOf("</a>");

                                string SecondSubStringForProfileURL = FirstSubStringForProfileURL.Substring(SecondPointForProfileURL, ThirdPointForProfileURL - SecondPointForProfileURL);
                                LDS_UserProfileLink = SecondSubStringForProfileURL.Replace(">", string.Empty);
                                //qm.AddProfileUrl(URLprofile, DateTime.Now.ToString(), "0");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }



                        try
                        {
                            string[] UrlFull = System.Text.RegularExpressions.Regex.Split(Url, "&authType");
                            LDS_UserProfileLink = UrlFull[0];

                            LDS_UserProfileLink = Url;
                        }
                        catch { }
                        #endregion




                        #region Connection
                        if (html.Contains("overview-connections"))
                        {
                            try
                            {
                                Connection = html.Substring(html.IndexOf("leo-module mod-util connections"), 500);
                                string[] Arr = Connection.Split('>');
                                string tempConnection = Arr[5].Replace("</strong", "").Replace(")</h3", "").Replace("(", "");
                                if (tempConnection.Length < 8)
                                {
                                    LDS_Connection = tempConnection + "Connection";
                                }
                                else
                                {
                                    LDS_Connection = string.Empty;
                                }


                            }
                            catch (Exception ex)
                            {
                                //overview-connections
                                try
                                {
                                    LDS_Connection = html.Substring(html.IndexOf("overview-connections"), 50);
                                    string[] Arr = Connection.Split('>');
                                    string tempConnection = Arr[3].Replace("</strong", "").Replace(")</h3", "").Replace("(", "");
                                    LDS_Connection = tempConnection + "Connection";
                                }
                                catch { }
                            }
                        }
                        #endregion

                        #region Recommendation
                        if (html.Contains("Recommendations"))
                        {

                            try
                            {
                                string[] rList = System.Text.RegularExpressions.Regex.Split(html, "Recommendations");
                                string[] R3List = rList[2].Split('\n');
                                string temprecomandation = R3List[4].Replace("</strong>", "").Replace("<strong>", "");
                                if (temprecomandation.Contains("recommended"))
                                {
                                    LDS_Recommendations = temprecomandation;
                                }
                                else
                                {
                                    LDS_Recommendations = "";
                                }

                            }
                            catch (Exception ex)
                            {
                                LDS_Recommendations = string.Empty;
                            }
                        }
                        #endregion



                        #region Websites
                        if (html.Contains("websites"))
                        {
                            try
                            {
                                string websitedem = html.Substring(html.IndexOf("websites"), 500);

                                string[] Arr = Regex.Split(websitedem, "href");
                                foreach (string item in Arr)
                                {
                                    if (item.Contains("redir/redirect?url"))
                                    {
                                        string tempArr = item.Substring(item.IndexOf("name="), 50);
                                        string[] temarr = tempArr.Split('\n');
                                        LDS_Websites = temarr[1];
                                    }
                                }

                            }
                            catch (Exception ex)
                            {
                                LDS_Websites = string.Empty;
                            }
                        }
                        #endregion

                        #region Getting Industry
                        try
                        {

                            string Industrytemp = html.Substring(html.IndexOf("Find users in this industry"), 100);
                            string[] TempIndustery = Industrytemp.Split('>');
                            LDS_Industry = TempIndustery[1].Replace("</strong", "").Replace("</a", "");

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        #endregion

                        #region Getting First Name
                        try
                        {
                            if (html.Contains("given-name"))
                            {
                                int FirstPointForProfilename = html.IndexOf("given-name");
                                string FirstSubStringForProfilename = html.Substring(FirstPointForProfilename);
                                int SecondPointForProfilename = FirstSubStringForProfilename.IndexOf(">");
                                int ThirdPointForProfilename = FirstSubStringForProfilename.IndexOf("</span>");

                                string SecondSubStringForProfilename = FirstSubStringForProfilename.Substring(SecondPointForProfilename, ThirdPointForProfilename - SecondPointForProfilename);
                                LDS_FirstName = SecondSubStringForProfilename.Replace(">", string.Empty);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        #endregion




                        #region LastName
                        try
                        {
                            if (html.Contains("family-name"))
                            {
                                int FirstPointForProfilelastname = html.IndexOf("family-name");
                                string FirstSubStringForProfilelastname = html.Substring(FirstPointForProfilelastname);
                                int SecondPointForProfilelastname = FirstSubStringForProfilelastname.IndexOf(">");
                                int ThirdPointForProfilelastname = FirstSubStringForProfilelastname.IndexOf("</span>");

                                string SecondSubStringForProfilelastname = FirstSubStringForProfilelastname.Substring(SecondPointForProfilelastname, ThirdPointForProfilelastname - SecondPointForProfilelastname);
                                string templastname = SecondSubStringForProfilelastname.Replace(">", string.Empty);
                                if (templastname.Contains(","))
                                {
                                    string[] arrylastname = templastname.Split(',');
                                    LDS_LastName = arrylastname[0];
                                }
                                else
                                {
                                    LDS_LastName = templastname;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                        #endregion

                        #region Designation Company Current
                        try
                        {
                            if (html.Contains("phonetic-full-name"))
                            {
                                int FirstPointForProfileCurrent = html.IndexOf("phonetic-full-name");
                                string FirstSubStringForProfileCurrent = html.Substring(FirstPointForProfileCurrent);
                                int SecondPointForProfileCurrent = FirstSubStringForProfileCurrent.IndexOf("display:block");
                                int ThirdPointForProfileCurrent = FirstSubStringForProfileCurrent.IndexOf("</p>");

                                string SecondSubStringForProfileCurrent = FirstSubStringForProfileCurrent.Substring(SecondPointForProfileCurrent, ThirdPointForProfileCurrent - SecondPointForProfileCurrent);
                                titlecurrent = SecondSubStringForProfileCurrent.Replace("\">", "").Replace("display:block", string.Empty).Replace("<strong class=\"highlight\"", string.Empty).Replace("</strong", string.Empty).Trim();
                                string[] tempCCurent = Regex.Split(titlecurrent, " at ");
                                LDS_HeadLineTitle = titlecurrent.Replace(",", ";");
                                LDS_CurrentCompany = tempCCurent[1].Replace(",", ";");

                            }

                            else if (html.Contains("<p class=\"title\""))
                            {
                                LDS_HeadLineTitle = html.Substring(html.IndexOf("<p class=\"title\""), 150);
                                string[] HeadLineTitle = LDS_HeadLineTitle.Split('>');
                                string tempHeadLineTitle = HeadLineTitle[1].Replace("\n", "").Replace(")</h3", "").Replace("</p", "");
                                LDS_HeadLineTitle = tempHeadLineTitle;
                                try
                                {
                                    string[] tempCCurent = Regex.Split(tempHeadLineTitle, " at ");
                                    LDS_HeadLineTitle = tempCCurent[0];
                                    LDS_CurrentCompany = tempCCurent[1];
                                }
                                catch { }
                            }


                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        #endregion




                        #region Education
                        try
                        {
                            if (html.Contains("summary-education"))
                            {
                                int FirstPointForProfileeducation1 = html.IndexOf("summary-education");
                                string FirstSubStringForProfileeducation1 = html.Substring(FirstPointForProfileeducation1);
                                int SecondPointForProfileeducation1 = FirstSubStringForProfileeducation1.IndexOf("<li>");
                                int ThirdPointForProfileeducation1 = FirstSubStringForProfileeducation1.IndexOf("</li>");

                                string SecondSubStringForProfileeducation1 = FirstSubStringForProfileeducation1.Substring(SecondPointForProfileeducation1, ThirdPointForProfileeducation1 - SecondPointForProfileeducation1);
                                education1 = SecondSubStringForProfileeducation1.Replace("<li>", string.Empty).Replace(",", string.Empty).Trim();
                            }

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }


                        #endregion


                        #region Country
                        try
                        {
                            if (html.Contains("locality"))
                            {
                                int FirstPointForlocality = html.IndexOf("locality");
                                string FirstSubStringForlocality = html.Substring(FirstPointForlocality);
                                int SecondPointForlocality = FirstSubStringForlocality.IndexOf("location");
                                int ThirdPointForlocality = FirstSubStringForlocality.IndexOf("</a>");

                                string SecondSubStringForlocality = FirstSubStringForlocality.Substring(SecondPointForlocality, ThirdPointForlocality - SecondPointForlocality);
                                string temlocation = SecondSubStringForlocality.Replace("location", string.Empty).Replace(">", string.Empty).Replace('"', ' ');
                                string[] temp = temlocation.Split(',');
                                LDS_Loction = temp[0].Replace("<strong class= highlight", string.Empty).Replace("</strong", string.Empty);
                                LDS_Country = temp[1].Replace("<strong class= highlight", string.Empty).Replace("</strong", string.Empty);
                                // country = temp[1].Replace("</strong", string.Empty);
                            }

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        #endregion


                        #region User Email
                        try
                        {
                            if (html.Contains("Email & Phone:"))
                            {
                                int FirstPointFortitlepast1 = html.IndexOf("abook-email");
                                string FirstSubStringFortitlepast1 = html.Substring(FirstPointFortitlepast1);
                                int SecondPointFortitlepast1 = FirstSubStringFortitlepast1.IndexOf("<a");
                                int ThirdPointFortitlepast1 = FirstSubStringFortitlepast1.IndexOf("</a>");

                                string SecondSubStringFortitlepast1 = FirstSubStringFortitlepast1.Substring(SecondPointFortitlepast1, ThirdPointFortitlepast1 - SecondPointFortitlepast1);
                                string[] tempEmail = SecondSubStringFortitlepast1.Split('>');
                                LDS_UserEmail = tempEmail[1];

                            }


                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                        #endregion

                        #region Type Of profile
                        try
                        {
                            if (html.Contains("profile-header"))
                            {
                                int FirstPointForProfileType = html.IndexOf("profile-header");
                                string FirstSubStringForProfileType = html.Substring(FirstPointForProfileType);
                                int SecondPointForProfileType = FirstSubStringForProfileType.IndexOf("class=\"n fn\"");
                                int ThirdPointForProfileType = FirstSubStringForProfileType.IndexOf("</span>");

                                string SecondSubStringForProfileType = FirstSubStringForProfileType.Substring(SecondPointForProfileType, ThirdPointForProfileType - SecondPointForProfileType);
                                string[] tempProfileType = SecondSubStringForProfileType.Split('>');
                                string ProfileType = tempProfileType[1];
                                LDS_ProfileType = ProfileType;
                            }
                            //<h1><span id="name" class="n fn">Private</span>
                            else if (html.Contains(" class=\"n fn\""))
                            {
                                try
                                {
                                    string ProfileTypetemp = html.Substring(html.IndexOf("class=\"n fn\""), 20);
                                    string[] TempProfileType = ProfileTypetemp.Split('>');
                                    LDS_ProfileType = TempProfileType[1].Replace("</strong", "").Replace("</a", "");
                                }
                                catch { }
                            }

                            if (LDS_ProfileType != "Public")
                            {
                                LDS_ProfileType = "Private";
                            }

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        #endregion


                        #region PhonNumber
                        try
                        {
                            if (html.Contains("<dt>Phone:</dt>"))
                            {
                                int FirstPointFortitlepast1 = html.IndexOf("profile-personal");
                                string FirstSubStringFortitlepast1 = html.Substring(FirstPointFortitlepast1);
                                int SecondPointFortitlepast1 = FirstSubStringFortitlepast1.IndexOf("<p>");
                                int ThirdPointFortitlepast1 = FirstSubStringFortitlepast1.IndexOf("<span");

                                string SecondSubStringFortitlepast1 = FirstSubStringFortitlepast1.Substring(SecondPointFortitlepast1, ThirdPointFortitlepast1 - SecondPointFortitlepast1);
                                LDS_UserContactNumber = SecondSubStringFortitlepast1.Replace("<p>", string.Empty);
                            }

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        #endregion



                        xNode = xml.SearchForTag(xBeginSearchAfter, "dt");
                        xBeginSearchAfter = xNode;



                        list.Clear();

                        #endregion


                        #region Regionfor PastCompney
                        try
                        {
                            if (html.Contains("summary-past"))
                            {



                                int FirstPointForPasttitle = html.IndexOf("summary-past");
                                string FirstSubStringForPasttitle = html.Substring(FirstPointForPasttitle);
                                int SecondPointForPasttitle = FirstSubStringForPasttitle.IndexOf("<li>");
                                int ThirdPointForPasttitle = FirstSubStringForPasttitle.IndexOf("summary-education");
                                string SecondSubStringForPasttitle = FirstSubStringForPasttitle.Substring(SecondPointForPasttitle, ThirdPointForPasttitle - SecondPointForPasttitle);
                                string FirstSubStringForPasttitlelast = htmlUtil.EntityDecode(SecondSubStringForPasttitle);

                                htmlToXml1.Html = FirstSubStringForPasttitlelast;
                                xHtml1 = htmlToXml1.ToXml();

                                Chilkat.Xml xml1 = new Chilkat.Xml();
                                xml1.LoadXml(xHtml1);

                                ////  Iterate over all h1 tags:
                                Chilkat.Xml xNode1 = default(Chilkat.Xml);
                                Chilkat.Xml xBeginSearchAfter1 = default(Chilkat.Xml);


                                list.Clear();
                                string[] tempC1 = null;
                                xNode1 = xml1.SearchForTag(xBeginSearchAfter1, "li");

                                while ((xNode1 != null))
                                {
                                    Finaldata = xNode1.AccumulateTagContent("text", "/text");
                                    listtitle.Add(Finaldata);
                                    // list.Add(Finaldata);

                                    try
                                    {
                                        tempC1 = Regex.Split(Finaldata, " at ");
                                    }
                                    catch { }
                                    if (tempC1 != null)
                                    {
                                        try
                                        {
                                            list.Add(tempC1[1]);
                                        }
                                        catch { }

                                    }

                                    xNode1 = xml1.SearchForTag(xBeginSearchAfter1, "li");
                                    xBeginSearchAfter1 = xNode1;
                                }

                                if (listtitle.Count > 0 || list.Count > 0)
                                {
                                    try
                                    {
                                        titlepast1 = listtitle[0] != null ? listtitle[0] : string.Empty;
                                        titlepast2 = listtitle[1] != null ? listtitle[1] : string.Empty;
                                        titlepast3 = listtitle[2] != null ? listtitle[2] : string.Empty;
                                        titlepast4 = listtitle[3] != null ? listtitle[3] : string.Empty;
                                    }
                                    catch { }

                                    try
                                    {
                                        companypast1 = list[0] != null ? list[0] : string.Empty;

                                        companypast2 = list[1] != null ? list[1] : string.Empty;

                                        companypast3 = list[2] != null ? list[2] : string.Empty;

                                        companypast4 = list[3] != null ? list[3] : string.Empty;
                                    }
                                    catch { }
                                }

                            }
                        }
                        catch { };

                        list.Clear();
                        #endregion


                        string companyCurrenttitle1 = string.Empty;

                        string companyCurrenttitle2 = string.Empty;

                        string companyCurrenttitle3 = string.Empty;

                        string companyCurrenttitle4 = string.Empty;


                        #region Regionfor summary-current
                        try
                        {
                            if (html.Contains("summary-current"))
                            {



                                int FirstPointForCurrenttitle = html.IndexOf("summary-current");
                                string FirstSubStringForCurrenttitle = html.Substring(FirstPointForCurrenttitle);
                                int SecondPointForCurrenttitle = FirstSubStringForCurrenttitle.IndexOf("<li>");
                                int ThirdPointForCurrenttitle = FirstSubStringForCurrenttitle.IndexOf("summary-past");
                                string SecondSubStringForCurrenttitle = FirstSubStringForCurrenttitle.Substring(SecondPointForCurrenttitle, ThirdPointForCurrenttitle - SecondPointForCurrenttitle);
                                string FirstSubStringForCurrenttitlelast = htmlUtil.EntityDecode(SecondSubStringForCurrenttitle);

                                htmlToXml1.Html = FirstSubStringForCurrenttitlelast;
                                xHtml1 = htmlToXml1.ToXml();

                                Chilkat.Xml xml1 = new Chilkat.Xml();
                                xml1.LoadXml(xHtml1);

                                ////  Iterate over all h1 tags:
                                Chilkat.Xml xNode1 = default(Chilkat.Xml);
                                Chilkat.Xml xBeginSearchAfter1 = default(Chilkat.Xml);


                                Currentlist.Clear();
                                list.Clear();
                                string[] tempC1 = null;
                                xNode1 = xml1.SearchForTag(xBeginSearchAfter1, "li");

                                while ((xNode1 != null))
                                {
                                    Finaldata = xNode1.AccumulateTagContent("text", "/text");
                                    Currentlist.Add(Finaldata);
                                    // list.Add(Finaldata);

                                    try
                                    {
                                        tempC1 = Regex.Split(Finaldata, " at ");
                                    }
                                    catch { }
                                    if (tempC1 != null)
                                    {
                                        try
                                        {
                                            list.Add(tempC1[1]);
                                        }
                                        catch { }

                                    }

                                    xNode1 = xml1.SearchForTag(xBeginSearchAfter1, "li");
                                    xBeginSearchAfter1 = xNode1;
                                }

                                if (Currentlist.Count > 0 || list.Count > 0)
                                {
                                    try
                                    {
                                        titleCurrenttitle = Currentlist[0] != null ? Currentlist[0] : string.Empty;
                                        titleCurrenttitle2 = Currentlist[1] != null ? Currentlist[1] : string.Empty;
                                        titleCurrenttitle3 = Currentlist[2] != null ? Currentlist[2] : string.Empty;
                                        titleCurrenttitle4 = Currentlist[3] != null ? Currentlist[3] : string.Empty;
                                    }
                                    catch { }

                                    try
                                    {
                                        companyCurrenttitle1 = list[0] != null ? list[0] : string.Empty;

                                        companyCurrenttitle2 = list[1] != null ? list[1] : string.Empty;

                                        companyCurrenttitle3 = list[2] != null ? list[2] : string.Empty;

                                        companyCurrenttitle4 = list[3] != null ? list[3] : string.Empty;
                                    }
                                    catch { }
                                }

                            }
                        }
                        catch { };

                        list.Clear();
                        #endregion

                        #region RegionForEDUCATION
                        try
                        {
                            if (html.Contains("summary-education"))
                            {

                                int FirstPointForEDUCATION = html.IndexOf("summary-education");
                                string FirstSubStringForEDUCATION = html.Substring(FirstPointForEDUCATION);
                                int SecondPointForEDUCATION = FirstSubStringForEDUCATION.IndexOf("<li>");
                                int ThirdPointForEDUCATION = FirstSubStringForEDUCATION.IndexOf("</ul>");
                                string SecondSubStringForEDUCATION = FirstSubStringForEDUCATION.Substring(SecondPointForEDUCATION, ThirdPointForEDUCATION - SecondPointForEDUCATION);
                                //string tempEDu = SecondSubStringForEDUCATION.Replace("<li>", string.Empty).Replace("</li>", string.Empty).Replace("  ", string.Empty).Replace("\n", string.Empty).Replace("\t", string.Empty).Trim();
                                string temptg = SecondSubStringForEDUCATION.Replace("<li>", "");

                                string[] templis6t = temptg.Split('/');
                                education1 = templis6t[0].Replace("\n", string.Empty).Replace("\t", string.Empty).Replace("<", string.Empty).Replace("span>", string.Empty).Replace(",", string.Empty).Trim();
                                education2 = templis6t[1].Replace("\n", string.Empty).Replace("\t", string.Empty).Replace("li>", string.Empty).Replace("<", string.Empty).Replace("span>", string.Empty).Replace(",", string.Empty).Trim();
                            }
                        }


                        catch { };

                        list.Clear();
                        #endregion

                        string GroupPastJob = string.Empty;
                        string GroupEduction = string.Empty;
                        LDS_PastTitles = titlepast1 + ";" + titlepast3;
                        LDS_PastCompany = companypast1 + ";" + companypast3;
                        LDS_Education = education1 + ";" + education2;
                        LDS_CurrentTitle = titleCurrenttitle;
                        LDS_LoginID = _UserName;//SearchCriteria.LoginID;                                                                                                                       //"ProfileType" + "," + "UserProfileLink" + "," + "FirstName" + "," + "LastName" + "," + "HeadLineTitle" + "," + "CurrentTitle " + "," + "Company" + "," + "Connection" + "," + "Recommendations " + "," + "SkillAndExpertise " + "," + "Experience " + "," + " Education" + "," + "Groups" + "," + "UserEmail" + "," + "UserContactNumbe" + "," + "PastTitles" + "," + "PastCompany" + "," + "Loction" + "," + "Country" + "," + "titlepast3" + "," + "companypast3" + "," + "titlepast4" + "," + "companypast4" + ",";
                        string LDS_FinalData = LDS_ProfileType.Replace(",", ";") + "," + LDS_UserProfileLink.Replace(",", ";") + "," + LDS_FirstName.Replace(",", ";") + "," + LDS_LastName.Replace(",", ";") + "," + LDS_HeadLineTitle.Replace(",", ";") + "," + LDS_CurrentTitle.Replace(",", ";") + "," + LDS_CurrentCompany.Replace(",", ";") + "," + LDS_Connection.Replace(",", ";") + "," + LDS_Recommendations.Replace(",", ";") + "," + LDS_SkillAndExpertise.Replace(",", ";") + "," + LDS_Experience.Replace(",", ";") + "," + LDS_Education.Replace(",", ";") + "," + LDS_Groups.Replace(",", ";") + "," + LDS_UserEmail.Replace(",", ";") + "," + LDS_UserContactNumber.Replace(",", ";") + "," + LDS_PastTitles.Replace(",", ";") + "," + LDS_PastCompany.Replace(",", ";") + "," + LDS_Loction.Replace(",", ";") + "," + LDS_Country.Replace(",", ";") + "," + LDS_Industry.Replace(",", ";") + "," + LDS_Websites.Replace(",", ";") + "," + LDS_LoginID.Replace(",", ";") + ",";

                        if (LDS_FinalData.Contains("<strong class=\"highlight\"") || LDS_FinalData.Contains("<span class=\"full-name\"") || LDS_FinalData.Contains("<strong class=\"highlight\"") || LDS_FinalData.Contains("overview-connections\">"))
                        {
                            LDS_FinalData = LDS_FinalData.Replace("<span class=\"full-name\"", "").Replace("\n", "").Replace("<strong class=\"highlight\"", "").Replace("overview-connections\">", "").Replace("</strong>", "").Replace("<strong>", "");
                        }
                        if (!string.IsNullOrEmpty(LDS_FirstName) || !string.IsNullOrEmpty(LDS_FirstName))
                        {
                            Log(LDS_FinalData);
                        }
                        // if (SearchCriteria.starter)
                        {

                            string tempFinalData = LDS_FinalData.Replace(";", "").Replace(LDS_UserProfileLink, "").Replace("Public", "").Replace(",", "").Replace(LDS_LoginID, "").Trim();

                            if (!string.IsNullOrEmpty(tempFinalData))
                            {
                                //AppFileHelper.AddingLinkedInDataToCSVFile(LDS_FinalData, Globals.path_ScrappedMembersFromGroup);

                                string CSVHeader = "ProfileType" + "," + "UserProfileLink" + "," + "FirstName" + "," + "LastName" + "," + "HeadLineTitle" + "," + "Current Title " + "," + "Current Company" + "," + "Connection" + "," + "Recommendations " + "," + "SkillAndExpertise " + "," + "Experience " + "," + " Education" + "," + "Groups" + "," + "UserEmail" + "," + "UserContactNumber" + "," + "PastTitles" + "," + "PastCompany" + "," + "Location" + "," + "Country" + "," + "Industry" + "," + "WebSites" + "," + "LinkedInLoginID" + ",";
                                string CSV_Content = TypeOfProfile.Replace(",", ";") + "," + LDS_UserProfileLink.Replace(",", ";") + "," + LDS_FirstName.Replace(",", ";") + "," + LDS_LastName.Replace(",", ";") + "," + LDS_HeadLineTitle.Replace(",", ";") + "," + LDS_CurrentTitle.Replace(",", ";") + "," + LDS_CurrentCompany.Replace(",", ";") + "," + LDS_Connection.Replace(",", ";") + "," + LDS_Recommendations.Replace(",", ";") + "," + LDS_SkillAndExpertise.Replace(",", ";") + "," + LDS_Experience.Replace(",", ";") + "," + LDS_Education.Replace(",", ";") + "," + LDS_Groups.Replace(",", ";") + "," + LDS_UserEmail.Replace(",", ";") + "," + LDS_UserContactNumber.Replace(",", ";") + "," + LDS_PastTitles.Replace(",", ";") + "," + LDS_PastCompany.Replace(",", ";") + "," + LDS_Loction.Replace(",", ";") + "," + LDS_Country.Replace(",", ";") + "," + LDS_Industry.Replace(",", ";") + "," + LDS_Websites.Replace(",", ";") + "," + LDS_LoginID.Replace(",", ";");

                                //string CSV_Content = TypeOfProfile + "," + LDS_UserProfileLink + "," + firstname + "," + lastname + "," + Company.Replace(",", ";") + "," + titlecurrent.Replace(",", ";") + "," + companycurrent.Replace(",", ";") + "," + Connection.Replace(",", ";") + "," + recomandation.Replace(",", string.Empty) + "," + Skill.Replace(",", ";") + "," + LDS_Experience.Replace(",", string.Empty) + "," + EducationCollection.Replace(",", ";") + "," + groupscollectin.Replace(",", ";") + "," + USERemail.Replace(",", ";") + "," + LDS_UserContact.Replace(",", ";") + "," + LDS_PastTitles + "," + AllComapny.Replace(",", ";") + "," + country.Replace(",", ";") + "," + location.Replace(",", ";") + "," + Industry.Replace(",", ";") + "," + Website.Replace(",", ";") + "," + LDS_LoginID + ",";// +TypeOfProfile + ",";

                                CSVUtilities.ExportDataCSVFile(CSVHeader, CSV_Content, Globals.path_ScrappedMembersFromGroup);
                                Log("[ " + DateTime.Now + " ] => [ Data Saved In CSV File With URL >>> " + LDS_UserProfileLink + " ]");
                            }

                            //if (!string.IsNullOrEmpty(LDS_FirstName) || !string.IsNullOrEmpty(LDS_FirstName))
                            //{
                            //    AppFileHelper.AddingLinkedInDataToCSVFile(LDS_FinalData, SearchCriteria.FileName);
                            //}
                        }

                    }
                    catch (Exception ex) { };

                }

            }
        } 
        #endregion

        #region Log
        private void Log(string message)
        {
            try
            {
                EventsArgs eventArgs = new EventsArgs(message);
                scrapGroupMemberLogEvents.LogText(eventArgs);
            }
            catch (Exception)
            {

            }
        } 
        #endregion


       
    }
}
