using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseLib;
using System.Text.RegularExpressions;

namespace ProfileManager
{
    public class ProfileManager
    {
        
        #region Variable and Object Declaration

        string _UserName = string.Empty;
        string _Password = string.Empty;
        string _ProxyAddress = string.Empty;
        string _ProxyPort = string.Empty;
        string _ProxyUserName = string.Empty;
        string _ProxyPassword = string.Empty;

        string _SummaryGoals = string.Empty;
        string _SummarySpecialties = string.Empty;
        public static List<string> lstCmpNames = new List<string>();
        public static List<string> lstTitelNames = new List<string>();
        public static List<string> lstLocationNames = new List<string>();
        public static List<string> lstDescriptionNames = new List<string>();

        public static string StartMonthFromcmb = string.Empty;
        public static string EndMonthFromcmb = string.Empty;
        public static string StartYearFromcmb = string.Empty;
        public static string EndYearFromcmb = string.Empty;
        public static bool SameExperienceTime = false;

        public AccountInfo.LinkedinLoginAndLogout linkedinLoginAndLogout = new AccountInfo.LinkedinLoginAndLogout();
        
        GlobusHttpHelper HttpHelper = new GlobusHttpHelper();

        public static Events ProfileManagerLogEvents = new Events();
        public Events LinkedInAddExperienceLogEvents = new Events();


        #endregion

        #region ProfileManager
        public ProfileManager()
        {

        } 
        #endregion

        #region ProfileManager
        public ProfileManager(string username, string password, string proxyaddress, string proxyport, string proxyusername, string proxypassword, string summarygoals, string summaryspecialties)
        {
            this._UserName = username;
            this._Password = password;
            this._ProxyAddress = proxyaddress;
            this._ProxyPort = proxyport;
            this._ProxyUserName = proxyusername;
            this._ProxyPassword = proxypassword;
            this._SummaryGoals = summarygoals;
            this._SummarySpecialties = summaryspecialties;
        } 
        #endregion

        #region ProfileManager
        public ProfileManager(string username, string password, string proxyaddress, string proxyport, string proxyusername, string proxypassword)
        {
            this._UserName = username;
            this._Password = password;
            this._ProxyAddress = proxyaddress;
            this._ProxyPort = proxyport;
            this._ProxyUserName = proxyusername;
            this._ProxyPassword = proxypassword;

        } 
        #endregion

        #region ProFileManager
        public void ProFileManager()
        {
            ProfileManager Profile_Manage = new ProfileManager(_UserName, _Password, _ProxyAddress, _ProxyPort, _ProxyUserName, _ProxyPassword, _SummaryGoals, _SummarySpecialties);
            Profile_Manage.ProfileCreator();
        } 
        #endregion

        #region ProfileCreator
        public void ProfileCreator()
        {
            try
            {
                //LoggerManageConnection("Logging in with " + _UserName);
                string Textmessage = string.Empty;

                if (linkedinLoginAndLogout.LoginHttpHelper(_UserName, _Password, _ProxyAddress, _ProxyPort, _ProxyUserName, _ProxyPassword, ref HttpHelper, ref Textmessage))
                {
                    SummaryEdit();
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        } 
        #endregion

        #region SummaryEdit
        public void SummaryEdit()
        {
            try
            {
                int proxyport = 888;
                string SummaryLink = string.Empty;

                Regex PortCheck = new Regex("^[0-9]*$");

                if (PortCheck.IsMatch(_ProxyPort) && !string.IsNullOrEmpty(_ProxyPort))
                {
                    proxyport = int.Parse(_ProxyPort);
                }
                string PgSrcProfile = string.Empty;

                //For going to LinkedIn  Profile Likn 
                PgSrcProfile = HttpHelper.getHtmlfromUrlProxy(new Uri("http://www.linkedin.com/profile/edit?trk=tab_pro"), _ProxyAddress, proxyport, _ProxyUserName, _ProxyPassword);

                if (PgSrcProfile.Contains("/profile/edit-summary"))
                {
                    string SummaryLinkTemp = PgSrcProfile.Substring(PgSrcProfile.IndexOf("/profile/edit-summary"), 200);

                    string[] Arr = SummaryLinkTemp.Split('"');

                    SummaryLink = Arr[0];

                    SummaryLink = "http://www.linkedin.com" + SummaryLink;

                    //string DecodedCharTest = Uri.UnescapeDataString(SummaryLink);
                    SummaryLink = Uri.UnescapeDataString(SummaryLink);
                    SummaryLink = SummaryLink.Replace("amp;", "");
                }

                string pgSrcProfileSummaryEdit = HttpHelper.getHtmlfromUrlProxy(new Uri(SummaryLink), _ProxyAddress, proxyport, _ProxyUserName, _ProxyPassword);

                //** Post Data For Summary Edit in profile ***************************************************************

                string postData = string.Empty;
                string postUrl = string.Empty;
                string expertise_comments = string.Empty;
                string specialties = string.Empty;
                string csrfToken = string.Empty;
                string sourceAlias = string.Empty;
                string goback = string.Empty;

                // For goback ** post data  
                if (SummaryLink.Contains("goback"))
                {
                    string[] ArrgoBacek = SummaryLink.Split('=');
                    goback = ArrgoBacek[2];

                }

                // For csrfToken ** post data
                string[] ArrCsrfToken = Regex.Split(pgSrcProfileSummaryEdit, "input");
                foreach (string item in ArrCsrfToken)
                {
                    if (!item.Contains("<!DOCTYPE"))
                    {
                        if (item.Contains("csrfToken") && item.Contains("value="))
                        {
                            csrfToken = item;
                            break;
                        }
                    }
                }


                if (csrfToken.Contains("csrfToken"))
                {
                    csrfToken = csrfToken.Substring(csrfToken.IndexOf("csrfToken"));
                    string[] Arr = csrfToken.Split('"');
                    csrfToken = Arr[2];
                    csrfToken = csrfToken.Replace(":", "%3A");
                }

                // For sourceAlias ** post data
                if (pgSrcProfileSummaryEdit.Contains("sourceAlias"))
                {
                    sourceAlias = pgSrcProfileSummaryEdit.Substring(pgSrcProfileSummaryEdit.IndexOf("sourceAlias"), 100);
                    string[] Arr = sourceAlias.Split('"');
                    sourceAlias = Arr[2];
                }


                // For expertise_comments ** post data  
                expertise_comments = _SummaryGoals.Replace(" ", "%20");

                // For specialties ** post data  
                specialties = _SummarySpecialties.Replace(" ", "%20");

                //Post Data for Summary_Edit in profile
                postData = "expertise_comments=" + expertise_comments + "&specialties=" + specialties + "&button=Save%20Changes&locale=en_US&nudgeID=&timestamp=&csrfToken=" + csrfToken + "&sourceAlias=" + sourceAlias + "&goback=" + goback;


                //Post Uri for Summary_Edit in profile
                postUrl = "http://www.linkedin.com/profile/editSummarySubmit";

                //Post Response
                string postResponse = HttpHelper.postFormDataProxy(new Uri(postUrl), postData, _ProxyAddress, proxyport, _ProxyUserName, _ProxyPassword);

                if (postResponse.Contains("Your professional summary has been updated"))
                {

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        } 
        #endregion

        #region AddExperience
        public void AddExperience(ref GlobusHttpHelper HttpHelper)
        {

            string FirstGetResponse = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/profile/edit?trk=tab_pro"));
            string postData = string.Empty;
            string postUrl = string.Empty;
            string expertise_comments = string.Empty;
            string specialties = string.Empty;
            string csrfToken = string.Empty;
            string sourceAlias = string.Empty;
            string goback = string.Empty;
            string Companyname = string.Empty;
            string Title = string.Empty;
            string Description = string.Empty;
            string Location = string.Empty;
            string startDateMonth = string.Empty;
            string startDateYear = string.Empty;
            string endDateMonth = string.Empty;
            string endDateYear = string.Empty;
            // birthday_day = RandomNumberGenerator.GenerateRandom(1, 28).ToString();
            if (!SameExperienceTime && EndMonthFromcmb != string.Empty)
            {
                try
                {
                    startDateMonth = RandomNumberGenerator.GenerateRandom(1, 12).ToString();
                    startDateYear = RandomNumberGenerator.GenerateRandom(1980, 2013).ToString();
                    endDateMonth = RandomNumberGenerator.GenerateRandom(1, 12).ToString();
                    endDateYear = RandomNumberGenerator.GenerateRandom(1980, 2013).ToString();
                }
                catch { }

                if (Convert.ToInt32(startDateYear) < Convert.ToInt32(endDateYear))
                {

                }
                else
                {
                    startDateYear = "2008";
                    endDateYear = "2011";
                }
            }
            else
            {
                startDateMonth = StartMonthFromcmb;
                startDateYear = StartYearFromcmb;
                endDateMonth = EndMonthFromcmb;
                endDateYear = EndYearFromcmb;
            }

            try
            {
                Companyname = lstCmpNames[RandomNumberGenerator.GenerateRandom(0, lstCmpNames.Count)];
                Loger("[ " + DateTime.Now + " ] => [ Company Name : " + Companyname + " ]");
            }
            catch { }
            try
            {
                Title = lstTitelNames[RandomNumberGenerator.GenerateRandom(0, lstTitelNames.Count)];
                Loger("[ " + DateTime.Now + " ] => [ Company Title : " + Title + " ]");
            }
            catch { }
            try
            {
                Location = lstLocationNames[RandomNumberGenerator.GenerateRandom(0, lstLocationNames.Count)];
                Loger("[ " + DateTime.Now + " ] => [ Location : " + Location + " ]");
            }
            catch { }
            try
            {
                Description = lstDescriptionNames[RandomNumberGenerator.GenerateRandom(0, lstDescriptionNames.Count)];
                Loger("[ " + DateTime.Now + " ] => [ Description : " + Description + " ]");
            }
            catch { }


            string[] ArrCsrfToken = Regex.Split(FirstGetResponse, "input");

            if (FirstGetResponse.Contains("csrfToken"))
            {
                try
                {
                    csrfToken = FirstGetResponse.Substring(FirstGetResponse.IndexOf("csrfToken"), 100);
                    string[] Arr = csrfToken.Split('>');
                    csrfToken = Arr[0].Replace("csrfToken=", "").Replace("\"","").Trim();
                }
                catch (Exception ex)
                {
                }
            }

            try
            {
                string[] ArrForValue = Regex.Split(FirstGetResponse, "name\":\"sourceAlias");
                string strValue = (ArrForValue[1].Substring(ArrForValue[1].IndexOf("value\":"), ArrForValue[1].IndexOf("type\":", ArrForValue[1].IndexOf("value\":")) - ArrForValue[1].IndexOf("value\":")).Replace("value\":", string.Empty).Replace("\\\"", string.Empty).Replace("\"", string.Empty).Replace(",","").Trim());
                sourceAlias = (strValue);
            }
            catch { }
            if (FirstGetResponse.Contains("goback"))
            {
                try
                {
                    goback = FirstGetResponse.Substring(FirstGetResponse.IndexOf("goback"), 300);
                    string[] Arr = goback.Split(',');
                    goback = Arr[0].Replace("goback=", "").Replace("\n", string.Empty).Replace("\"", string.Empty);
                }
                catch (Exception ex)
                {
                }
            }

            try
            {
                string postdataa = "csrfToken=" + csrfToken + "&goback=" + goback + "&trk=view-topcard&startTask=&futureOffset=";             
                //string CheckRes = HttpHelper.postFormDataRef(new Uri("http://www.linkedin.com/profile/guided-edit-entry-point"), postdataa, "http://www.linkedin.com/profile/edit?trk=tab_pro", "", "XMLHttpRequest", "", "", "1");
                string CheckRes = HttpHelper.postFormDataRef(new Uri("http://www.linkedin.com/profile/guided-edit-entry-point"), postdataa, "http://www.linkedin.com/profile/edit?trk=nav_responsive_sub_nav_edit_profile", "", "XMLHttpRequest", "", "", "1");
                string FirstPostUrlForAddExperience = "http://www.linkedin.com/lite/web-action-track?csrfToken=" + csrfToken;
                string FirstPostDataForAddExperience = "pkey=nprofile_v2_edit_fs&tcode=nprofile-edit-position-submit&plist=activityType%3Aadd";
                string FirstPostResponseForAddExperience = HttpHelper.postFormDataRef(new Uri(FirstPostUrlForAddExperience), FirstPostDataForAddExperience, "http://www.linkedin.com/profile/edit?trk=nav_responsive_sub_nav_edit_profile", "", "XMLHttpRequest", "", "", "1");
            }
            catch { }

            try
            {
                string SecondPostUrlForAddExperience = "http://www.linkedin.com/lite/web-action-track?csrfToken=" + csrfToken;
                string SecondPostDataForAddExperience = "pkey=nprofile_v2_edit_fs&tcode=nprofile-edit-position-submit&plist=activityType%3Aadd";
                SecondPostDataForAddExperience = "pkey=nprofile_v2_edit_fs&tcode=profile-edit-position-form&plist=source%3Aprof-edit-background-add_position-link";
                string SecondPostResponseForAddExperience = HttpHelper.postFormDataRef(new Uri(SecondPostUrlForAddExperience), SecondPostDataForAddExperience, "http://www.linkedin.com/profile/edit?trk=nav_responsive_sub_nav_edit_profile", "", "XMLHttpRequest", "", "", "1");
            }
            catch { }
            string ThirdPostResponseForAddExperience = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(endDateYear) && !string.IsNullOrEmpty(endDateMonth))
                {

                    string ThirdPostUrlForAddExperience = "http://www.linkedin.com/profile/edit-position-submit?goback=" + goback;
                    string ThirdPostDataForAddExperience = "companyName=" + Companyname + "&companyDisplayName=" + Companyname + "&title=" + Title + "&positionLocationName=" + Location + "&startDateMonth=" + startDateMonth + "&startDateYear=" + startDateYear + "&endDateMonth=" + endDateMonth + "&endDateYear=" + endDateYear + "&updateHeadline=true&updatedHeadline=&summary=" + Description + "&trk-infoParams=&submit=Save&locale=en_US&timestamp=0&useJsonResponse=true&positionID=&experienceId=&defaultLocaleParam=en_US&companyID=0&positionLocation=0&checkboxValue=&sendMailCheckboxValue=&csrfToken=" + csrfToken + "&sourceAlias=" + sourceAlias;
                    ThirdPostResponseForAddExperience = HttpHelper.postFormDataRef(new Uri(ThirdPostUrlForAddExperience), ThirdPostDataForAddExperience, "http://www.linkedin.com/profile/edit?trk=tab_pro", "", "XMLHttpRequest", "", "", "1");

                    Loger("[ " + DateTime.Now + " ] => [ Experience Add to UserName : " + _UserName + " ]");
                    Loger("[ " + DateTime.Now + " ] => [ Company Name : " + Companyname + " Title : " + Title + " Set Using UserName : " + _UserName + " ]");

                    try
                    {

                        string CSVHeader = "User Name" + "," + "Company Name" + "," + "Title" + "," + "Location" + "," + "Description";
                        string CSV_Content = _UserName + "," + Companyname + "," + Title + "," + Location + "," + Description.Replace(",",string.Empty) ;
                        CSVUtilities.ExportDataCSVFile(CSVHeader, CSV_Content, Globals.path_LinkedinAddExperience);
                        Loger("[ " + DateTime.Now + " ] => [ Data Saved In CSV File ! ]");

                    }
                    catch { }


                }
                else
                {
                    string ThirdPostUrlForAddExperience = "http://www.linkedin.com/profile/edit-position-submit?goback=" + goback;
                    string ThirdPostDataForAddExperience = "companyName=" + Companyname + "&companyDisplayName=" + Companyname + "&title=" + Title + "&positionLocationName=" + Location + "&startDateMonth=" + startDateMonth + "&startDateYear=" + startDateYear + "&endDateMonth=&endDateYear=&isCurrent=isCurrent&updateHeadline=true&updatedHeadline=" + Title + "&summary=" + Description + "&trk-infoParams=&submit=Save&locale=en_US&timestamp=1360644826453&useJsonResponse=true&positionID=&experienceId=&defaultLocaleParam=en_US&companyID=0&positionLocation=0&checkboxValue=&sendMailCheckboxValue=&csrfToken=" + csrfToken + "&sourceAlias=" + sourceAlias;
                    ThirdPostResponseForAddExperience = HttpHelper.postFormDataRef(new Uri(ThirdPostUrlForAddExperience), ThirdPostDataForAddExperience, "http://www.linkedin.com/profile/edit?trk=tab_pro", "", "XMLHttpRequest", "", "", "1");
                    Loger("[ " + DateTime.Now + " ] => [ Experience Add to UserName : " + _UserName + " ]");
                    Loger("[ " + DateTime.Now + " ] => [ Company Name : " + Companyname + " Title : " + Title + " Set Using UserName : " + _UserName + " ]");

                    try
                    {

                        string CSVHeader = "User Name" + "," + "Company Name" + "," + "Title" + "," + "Location" + "," + "Description" ;
                        string CSV_Content = _UserName + "," + Companyname + "," + Title + "," + Location + "," + Description.Replace(",","");
                        CSVUtilities.ExportDataCSVFile(CSVHeader, CSV_Content, Globals.path_LinkedinAddExperience);
                        Loger("[ " + DateTime.Now + " ] => [ Data Saved In CSV File ! ]");

                    }
                    catch { }

                }
            }
            catch { }
        } 
        #endregion

        #region Loger
        private void Loger(string message)
        {
            EventsArgs eventArgs = new EventsArgs(message);
            LinkedInAddExperienceLogEvents.LogText(eventArgs);
        } 
        #endregion
    }
}
