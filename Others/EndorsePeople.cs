using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseLib;
using System.Text.RegularExpressions;

namespace Others
{
   public class EndorsePeople
    {
        #region Global declarations
        public string proxyAddress = string.Empty;
        public string proxyPort = string.Empty;
        public string proxyUsername = string.Empty;
        public string proxyPassword = string.Empty;
        public string Username = string.Empty;
        public static Events logger = new Events();
        public static List<string[]> Endorse_excelData = new List<string[]>();
        public static string xls_Email = string.Empty;
        public static string xls_UserId = string.Empty;
        public static int no_of_Skils = 0;
        public static int MinDelay = 20;
        public static int MaxDelay = 25;
        public static string checkLoginEmail = string.Empty;
        #endregion

        #region Log
        private void Log(string log)
        {
            try
            {
                EventsArgs loggerEventsArgs = new EventsArgs(log);
                logger.LogText(loggerEventsArgs);
            }
            catch (Exception ex)
            {

            }
        } 
        #endregion

        #region EndorsingPeople
        
        public void EndorsingPeople(ref GlobusHttpHelper HttpHelper, string UserId)
        {
            try
            {
                int count = 0;
                int ProxyPort = 0;
                if (!string.IsNullOrEmpty(proxyPort) && NumberHelper.ValidateNumber(proxyPort))
                {
                    ProxyPort = Convert.ToInt32(proxyPort);
                }
                
                string EndorseLink = string.Empty;
                string modelNo = string.Empty;
                string csrfToken = string.Empty;
                string postdata = string.Empty;
                string _ed = string.Empty;
                string endorseUrl = string.Empty;
                string Link = string.Empty;
                string LinkSource = string.Empty;
                string PageSourceProfile = string.Empty;

                try
                {
                    Globals.tempDict.Add(UserId, "");
                    Log("[ " + DateTime.Now + " ] => [ Starting Getting Member Details For User Id Link - :" + UserId + " With : " + Username + " ]");
                }
                catch
                { 
                }
                

                if (!UserId.Contains("http"))
                {
                    Link = "http://www.linkedin.com/people/conn-details?i=&contactMemberID=" + UserId;

                    PageSourceProfile = HttpHelper.getHtmlfromUrlProxy(new Uri(Link), proxyAddress, 0, proxyUsername, proxyPassword);

                    try
                    {
                        int startindex = PageSourceProfile.IndexOf("/profile/view?id=");
                        if (startindex > 0)
                        {
                            string start = PageSourceProfile.Substring(startindex);
                            int endIndex = start.IndexOf("\">");
                            string end = start.Substring(0, endIndex).Replace("\\", "").Replace("&amp;", "&");

                            if (end.Contains("&trk"))
                            {
                                end = end.Split('&')[0].ToString();
                            }

                            LinkSource = "http://www.linkedin.com" + end;
                        }
                    }
                    catch (Exception ex)
                    {
                        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error -->EndorsingPeople() --> LinkSource >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> EndorsingPeople() --> LinkSource >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.path_EndorsePeople);
                    }

                    if (string.IsNullOrEmpty(UserId))
                    {
                        try
                        {
                            Globals.tempDict.Add(UserId, "");
                            Log("[ " + DateTime.Now + " ] => [ Cannot Find Data For Userid : " + UserId + " From " + Username + " ]");
                            return;
                        }
                        catch
                        { }
                    }

                    PageSourceProfile = HttpHelper.getHtmlfromUrlProxy(new Uri(LinkSource), proxyAddress, 0, proxyUsername, proxyPassword);
                }
                else
                {
                    Link = UserId;

                    PageSourceProfile = HttpHelper.getHtmlfromUrlProxy(new Uri(Link), proxyAddress, 0, proxyUsername, proxyPassword);
                }

            

                if (PageSourceProfile.Contains("csrfToken"))
                {
                    csrfToken = PageSourceProfile.Substring(PageSourceProfile.IndexOf("csrfToken"), 100);
                    string[] Arr = csrfToken.Split('&');

                    try
                    {
                        csrfToken = Arr[0].Replace("csrfToken=", "").Replace("\\", "");
                        csrfToken = csrfToken.Split('>')[0].Replace(">", string.Empty).Replace("\"",string.Empty);
                    }
                    catch (Exception ex)
                    {
                        csrfToken = Arr[0].Replace("csrfToken=", "").Replace("\\", "");
                    }
                    //csrfToken
                    if (csrfToken.Contains("updateTagsUrl"))
                    {
                        csrfToken = csrfToken.Split(',')[0].ToString();
                    }

                }

                
                Dictionary<string, string> lstskilldata = new Dictionary<string, string>();
                if (PageSourceProfile.Contains("/profile/unendorse?_ed="))
                {
                    _ed = Utils.getBetween(PageSourceProfile, "/profile/unendorse?_ed=", "&");
                }
                if (PageSourceProfile.Contains("endorseURL:"))
                {
                    endorseUrl = Utils.getBetween(PageSourceProfile, "endorseURL:", ",").Replace("\\","").Replace("'","").Trim();
                    endorseUrl = "https://www.linkedin.com"+endorseUrl;
                }

                try
                {
                    string[] arr = Regex.Split(PageSourceProfile, "fmt__skill_name\":");
                    arr = arr.Skip(1).ToArray();
                    foreach (string item in arr)
                    {
                        string SkillName = string.Empty;
                        string SkillId = string.Empty;
                        if (!item.Contains("viewerEndorsementId\":"))
                        {
                            if (item.Contains("id\":"))
                            {
                                SkillName = Utils.getBetween(item, "", ",").Replace("\\u002d", "-").Replace("\"", "").Replace(",", "").Replace("&amp;", "&").Replace("}", "").Trim();
                                //SkillId = Utils.getBetween(item, "id\":", ",").Replace("\"", "").Replace(",", "").Replace("&amp;", "&").Replace("}", "").Replace("\u002d", "-").Trim();
                                lstskilldata.Add(SkillName, SkillName);
                            }
                        }

                        
                    }
                    if (lstskilldata.Count == 0)
                    {
                        arr = Regex.Split(PageSourceProfile, "data-endorsed-item-name=\"");
                        arr = arr.Skip(1).ToArray();
                     
                        foreach (string item in arr)
                        {
                                                  
                            if (EndorsePeople.no_of_Skils == lstskilldata.Count)
                            {
                                if (EndorsePeople.no_of_Skils == 0 && lstskilldata.Count == 0)
                                {

                                }
                                else
                                {
                                    break;
                                }
                            }
                                string SkillName = string.Empty;
                                string SkillId = string.Empty;
                                if (!item.Contains("endorse-item has-endorsements endorsed-by-viewer"))
                                {
                                    //if (item.Contains("id\":"))
                                    {                                        
                                        SkillName = Utils.getBetween(item, "", "\"").Replace("\\u002d", "-").Replace("\"", "").Replace(",", "").Replace("&amp;", "&").Replace("}", "").Trim();
                                        //SkillId = Utils.getBetween(item, "id\":", ",").Replace("\"", "").Replace(",", "").Replace("&amp;", "&").Replace("}", "").Replace("\u002d", "-").Trim();
                                        lstskilldata.Add(SkillName, SkillName);
                                    }
                                }
                            

                        }
                    }
                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error -->EndorsingPeople() --> lstskilldata Data  >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> EndorsingPeople() --> lstskilldata Data >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.path_EndorsePeople);
                }

                try
                {
                    string _UserId = string.Empty;
                    
                    if (lstskilldata.Count > 0)
                    {
                        
                        foreach (KeyValuePair<string, string> item in lstskilldata)
                        {
                            if (count > 4)
                            {
                                return;
                            }
                            if (!UserId.Contains("http"))
                            {
                                _UserId = UserId;
                            }
                            else
                            {
                                _UserId = Utils.getBetween(UserId+"@", "&id=", "@");
                                if (string.IsNullOrEmpty(_UserId))
                                {
                                    _UserId = Utils.getBetween(UserId + "@", "?id=", "@");
                                }
                                //_UserId = UserId.Substring(UserId.IndexOf("&id=")).Replace("&id=", string.Empty);
                                
                            }

                            postdata = "endorsementCount=1&recipientId-0=" + _UserId + "&recipientType-0=member&endorsedItemName-0=" + Uri.EscapeDataString(item.Key) + "&endorsementId-0=&endorserIds-0=";
                            string PostUrl = "http://www.linkedin.com/profile/endorse?csrfToken=" + csrfToken + "&_ed=" + _ed + "&goback=%2Enpv_" + UserId + "_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_contacts*5contacts*5list*5contact*4name*50_*1";
                            
                            string PostPageSource = HttpHelper.postFormDataRef(new Uri(endorseUrl), postdata, LinkSource, "", "", "", "", "1");
                            
                            if (!PostPageSource.Contains("fail") || !PostPageSource.Contains("failure"))
                            {
                                string[] ArrayPostedData = System.Text.RegularExpressions.Regex.Split(PostPageSource, "}");
                                //ArrayPostedData = ArrayPostedData.Skip(1).ToArray();
                                foreach (string itemdata in ArrayPostedData)
                                {
                                    foreach (KeyValuePair<string, string> item1 in lstskilldata)
                                    {
                                        if (itemdata.Contains(item1.Key))
                                        {
                                            string status = string.Empty;
                                            try
                                            {
                                                //int startindex = itemdata.IndexOf("endorsementStatus\":\"");
                                                int startindex = PostPageSource.IndexOf("status");
                                                if (startindex >= 0)
                                                {
                                                    //string start = itemdata.Substring(startindex).Replace("endorsementStatus\":\"", "");
                                                    //int endIndex = start.IndexOf("\",\"");
                                                    //string end = start.Substring(0, endIndex).Replace("&amp;", "&");
                                                    //status = end;

                                                    string start = PostPageSource.Substring(startindex).Replace("status", "");
                                                    int endIndex = start.IndexOf("}");
                                                    string end = start.Substring(0, endIndex).Replace("\"", string.Empty).Replace(":", string.Empty);
                                                    status = end;

                                                    string CSVHeader = "UserID" + "," + "Skill" + "," + "Status" + "," + "EndorseID";   //New Addedd
                                                    string CSV_Content = Username + "," + item1.Key + "," + status + "," + UserId;       //new adde
                                                    CSVUtilities.ExportDataCSVFile(CSVHeader, CSV_Content, Globals.path_EndorsedPeopleText);
                                                    Log("[ " + DateTime.Now + " ] => [ Status For Skill : " + item1.Key + " ==> " + status + " ]");
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error -->EndorsingPeople() --> PostPageSource posted correct >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                                                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> EndorsingPeople() --> PostPageSource posted correct >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.path_EndorsePeople);
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                string CSVHeader = "UserID" + "," + "Status" + "," + "EndorseID";
                                string CSV_Content = Username + "," + "Failure" + "," + UserId;
                                CSVUtilities.ExportDataCSVFile(CSVHeader, CSV_Content, Globals.path_NotEndorsedPeopleText);
                                GlobusFileHelper.AppendStringToTextfileNewLine(Username + ",Failure", Globals.path_NotEndorsedPeopleText);
                                Log("[ " + DateTime.Now + " ] => [ Failure for User id : " + item.Key + " -> " +  _UserId + " ]");

                            }
                            count++;
                        }
                    }
                    else
                    {
                        Log("[ " + DateTime.Now + " ] => [ No skills found for User id " + _UserId + " ]");
                    }
                    
                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error -->EndorsingPeople() --> postdata  >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> EndorsingPeople() --> postdata >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.path_EndorsePeople);
                }
          
                
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> EndorsingPeople() --> Full Data >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> EndorsingPeople() --> Full Data >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.path_EndorsePeople);
            }
        } 
        #endregion


        #region EndorsingPeople
        //public void EndorsingPeopleOld(ref GlobusHttpHelper HttpHelper, string UserId)
        //{
        //    try
        //    {
        //        int ProxyPort = 0;
        //        if (!string.IsNullOrEmpty(proxyPort) && NumberHelper.ValidateNumber(proxyPort))
        //        {
        //            ProxyPort = Convert.ToInt32(proxyPort);
        //        }
        //        string LinkSource = string.Empty;
        //        string EndorseLink = string.Empty;
        //        string modelNo = string.Empty;
        //        string csrfToken = string.Empty;
        //        string postdata = string.Empty;
        //        string _ed = string.Empty;

        //        Log("[ " + DateTime.Now + " ] => [ Starting Getting Member Details For User Id - :" + UserId + " With : " + Username + " ]");
        //        string Link = "http://www.linkedin.com/people/conn-details?i=&contactMemberID=" + UserId;
        //        string PageSource = HttpHelper.getHtmlfromUrlProxy(new Uri(Link), proxyAddress, 0, proxyUsername, proxyPassword);

        //        try
        //        {
        //            int startindex = PageSource.IndexOf("/profile/view?id=");
        //            if (startindex > 0)
        //            {
        //                string start = PageSource.Substring(startindex);
        //                int endIndex = start.IndexOf("\">");
        //                string end = start.Substring(0, endIndex).Replace("\\", "").Replace("&amp;", "&");
        //                LinkSource = "http://www.linkedin.com" + end;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error -->EndorsingPeople() --> LinkSource >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
        //            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> EndorsingPeople() --> LinkSource >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.path_EndorsePeople);
        //        }

        //        if (string.IsNullOrEmpty(LinkSource))
        //        {
        //            Log("[ " + DateTime.Now + " ] => [ Cannot Find Data For Userid : " + UserId + " From " + Username + " ]");
        //            return;
        //        }

        //        string PageSourceProfile = HttpHelper.getHtmlfromUrlProxy(new Uri(LinkSource), proxyAddress, 0, proxyUsername, proxyPassword);

        //        if (PageSourceProfile.Contains("csrfToken"))
        //        {
        //            csrfToken = PageSourceProfile.Substring(PageSourceProfile.IndexOf("csrfToken"), 100);
        //            //string[] Arr = csrfToken.Split('"');
        //            string[] Arr = csrfToken.Split('&');

        //            try
        //            {
        //                //csrfToken = Arr[1].Replace(@"\", string.Empty).Replace("//", string.Empty);
        //                csrfToken = Arr[0].Replace("csrfToken=", "").Replace("\\", "");
        //                csrfToken = csrfToken.Split('>')[0].Replace(">", string.Empty).Replace("\"", string.Empty);
        //            }
        //            catch (Exception ex)
        //            {
        //                csrfToken = Arr[0].Replace("csrfToken=", "").Replace("\\", "");
        //            }
        //        }

        //        if (csrfToken.Contains("ajax%") && csrfToken.Contains("ajax%"))
        //        {
        //            try
        //            {
        //                csrfToken = csrfToken.Substring(csrfToken.IndexOf("ajax"), csrfToken.IndexOf("&amp") - csrfToken.IndexOf("ajax")).Replace("csrfToken=", "").Trim();

        //            }
        //            catch { }

        //        }

        //        try
        //        {
        //            int startindex = PageSourceProfile.IndexOf("/profile/endorse?_ed=");
        //            if (startindex > 0)
        //            {
        //                string start = PageSourceProfile.Substring(startindex).Replace("/profile/endorse?_ed=", "");
        //                int endIndex = start.IndexOf("&csrfToken=");
        //                string end = start.Substring(0, endIndex).Replace("&amp;", "&");
        //                _ed = end;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error -->EndorsingPeople() --> _ed >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
        //            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> EndorsingPeople() --> _ed >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.path_EndorsePeople);
        //        }

        //        try
        //        {
        //            int startindex = PageSourceProfile.IndexOf("/profile/suggested-member-skill-endorsements-for-single-member?");
        //            if (startindex > 0)
        //            {
        //                string start = PageSourceProfile.Substring(startindex);
        //                int endIndex = start.IndexOf("\',");
        //                string end = start.Substring(0, endIndex).Replace("&amp;", "&");
        //                EndorseLink = "http://www.linkedin.com" + end;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error -->EndorsingPeople() --> EndorseLink >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
        //            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> EndorsingPeople() --> EndorseLink >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.path_EndorsePeople);
        //        }


        //        if (string.IsNullOrEmpty(EndorseLink))
        //        {
        //            Log("[ " + DateTime.Now + " ] => [ " + UserId + " Not In Contact So Cannot Endorse From " + Username + " ]");
        //            return;
        //        }
        //        string PageSourceEndorseLink = HttpHelper.getHtmlfromUrlProxy(new Uri(EndorseLink), proxyAddress, 0, proxyUsername, proxyPassword);

        //        try
        //        {
        //            int startindex = PageSourceEndorseLink.IndexOf("\"model\":\"");
        //            if (startindex > 0)
        //            {
        //                string start = PageSourceEndorseLink.Substring(startindex).Replace("\"model\":\"", "");
        //                int endIndex = start.IndexOf("\",");
        //                string end = start.Substring(0, endIndex).Replace("&amp;", "&");
        //                modelNo = end;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error -->EndorsingPeople() --> modelNo >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
        //            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> EndorsingPeople() --> modelNo >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.path_EndorsePeople);
        //        }
        //        Dictionary<string, string> lstskilldata = new Dictionary<string, string>();
        //        try
        //        {
        //            int index = PageSourceEndorseLink.IndexOf("suggestedEndorsements\":");
        //            if (index > 0)
        //            {
        //                string data = PageSourceEndorseLink.Substring(index).Replace("suggestedEndorsements\":", "");
        //                index = data.IndexOf("],");
        //                data = data.Substring(0, index);
        //                string[] ArrayData = System.Text.RegularExpressions.Regex.Split(data, "{\"");
        //                ArrayData = ArrayData.Skip(1).ToArray();
        //                foreach (string item in ArrayData)
        //                {
        //                    string SkillName = string.Empty;
        //                    string SkillId = string.Empty;
        //                    try
        //                    {
        //                        int startindex = item.IndexOf("skillName\":\"");
        //                        if (startindex >= 0)
        //                        {
        //                            string start = item.Substring(startindex).Replace("skillName\":\"", "");
        //                            int endIndex = start.IndexOf("\",\"");
        //                            string end = start.Substring(0, endIndex).Replace("&amp;", "&");
        //                            SkillName = end;
        //                        }
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error -->EndorsingPeople() --> SkillName >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
        //                        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> EndorsingPeople() --> SkillName >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.path_EndorsePeople);
        //                    }
        //                    try
        //                    {
        //                        int startindex = item.IndexOf("skillId\":");
        //                        if (startindex > 0)
        //                        {
        //                            string start = item.Substring(startindex).Replace("skillId\":", "");
        //                            int endIndex = start.IndexOf(",");
        //                            string end = start.Substring(0, endIndex).Replace("&amp;", "&");
        //                            SkillId = end;
        //                        }
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error -->EndorsingPeople() --> SkillId >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
        //                        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> EndorsingPeople() --> SkillId >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.path_EndorsePeople);
        //                    }
        //                    lstskilldata.Add(SkillName, SkillId);
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error -->EndorsingPeople() --> lstskilldata Data  >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
        //            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> EndorsingPeople() --> lstskilldata Data >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.path_EndorsePeople);
        //        }

        //        try
        //        {
        //            int i = 0;
        //            foreach (KeyValuePair<string, string> item in lstskilldata)
        //            {
        //                if (string.IsNullOrEmpty(postdata))
        //                {
        //                    postdata = "endorsedItemName-" + i + "=" + Uri.EscapeDataString(item.Key) + "&endorsedItemType-" + i + "=skill&endorsedItemId-" + i + "=" + item.Value + "&recipientType-" + i + "=member&recipientId-" + i + "=" + UserId;
        //                    i++;
        //                }
        //                else
        //                {
        //                    postdata = postdata + "&endorsedItemName-" + i + "=" + Uri.EscapeDataString(item.Key) + "&endorsedItemType-" + i + "=skill&endorsedItemId-" + i + "=" + item.Value + "&recipientType-" + i + "=member&recipientId-" + i + "=" + UserId;
        //                    i++;
        //                }
        //            }
        //            postdata = postdata + "&endorse=Endorse&endorsementCount=" + i++;
        //        }
        //        catch (Exception ex)
        //        {
        //            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error -->EndorsingPeople() --> postdata  >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
        //            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> EndorsingPeople() --> postdata >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.path_EndorsePeople);
        //        }

        //        //string PostUrl = "http://www.linkedin.com/profile/endorse?model=" + modelNo + "&csrfToken=" + csrfToken + "&_ed=" + _ed;
        //        string PostUrl = "http://www.linkedin.com/profile/endorse?csrfToken=" + csrfToken + "&_ed=" + _ed + "&goback=%2Enpv_" + UserId + "_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_contacts*5contacts*5list*5contact*4name*50_*1";
        //        string PostPageSource = HttpHelper.postFormDataRef(new Uri(PostUrl), postdata, LinkSource, "", "", "", "", "1");
        //        ////http://www.linkedin.com/profile/endorse?_ed=0_2KjZg6oXb2V2F89fO9b1Jr9vVefjgmMy5Eg5IR_1O8f&csrfToken=ajax%3A5408174125717314157&goback=%2Enpv_1274399_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_contacts*5contacts*5list*5contact*4name*50_*1

        //        if (!PostPageSource.Contains("fail") || !PostPageSource.Contains("failure"))
        //        {
        //            string[] ArrayPostedData = System.Text.RegularExpressions.Regex.Split(PostPageSource, "}");
        //            //ArrayPostedData = ArrayPostedData.Skip(1).ToArray();
        //            foreach (string itemdata in ArrayPostedData)
        //            {
        //                foreach (KeyValuePair<string, string> item in lstskilldata)
        //                {
        //                    if (itemdata.Contains(item.Key))
        //                    {
        //                        string status = string.Empty;
        //                        try
        //                        {
        //                            //int startindex = itemdata.IndexOf("endorsementStatus\":\"");
        //                            int startindex = PostPageSource.IndexOf("status");
        //                            if (startindex >= 0)
        //                            {
        //                                //string start = itemdata.Substring(startindex).Replace("endorsementStatus\":\"", "");
        //                                //int endIndex = start.IndexOf("\",\"");
        //                                //string end = start.Substring(0, endIndex).Replace("&amp;", "&");
        //                                //status = end;

        //                                string start = PostPageSource.Substring(startindex).Replace("status", "");
        //                                int endIndex = start.IndexOf("}");
        //                                string end = start.Substring(0, endIndex).Replace("\"", string.Empty).Replace(":", string.Empty);
        //                                status = end;

        //                                string CSVHeader = "UserID" + "," + "Skill" + "," + "Status" + "," + "EndorseID";   //New Addedd
        //                                string CSV_Content = Username + "," + item.Key + "," + status + "," + UserId;       //new adde
        //                                CSVUtilities.ExportDataCSVFile(CSVHeader, CSV_Content, Globals.path_EndorsedPeopleText);
        //                                Log("[ " + DateTime.Now + " ] => [ Status For Skill : " + item.Key + " ==> " + status + " ]");
        //                            }
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error -->EndorsingPeople() --> PostPageSource posted correct >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
        //                            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> EndorsingPeople() --> PostPageSource posted correct >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.path_EndorsePeople);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            string CSVHeader = "UserID" + "," + "Status" + "," + "EndorseID";
        //            string CSV_Content = Username + "," + "Failure" + "," + UserId;
        //            CSVUtilities.ExportDataCSVFile(CSVHeader, CSV_Content, Globals.path_NotEndorsedPeopleText);
        //            GlobusFileHelper.AppendStringToTextfileNewLine(Username + ",Failure", Globals.path_NotEndorsedPeopleText);
        //            Log("[ " + DateTime.Now + " ] => [ Failure for User id : " + UserId + " ]");

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> EndorsingPeople() --> Full Data >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
        //        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> EndorsingPeople() --> Full Data >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.path_EndorsePeople);
        //    }
        //}
        #endregion
    }
}
