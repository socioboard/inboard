using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseLib;
using System.Text.RegularExpressions;
using System.Threading;

namespace ProfileManager
{
   public class LinkedinProfileImgUploader
    {
        #region global declaration
        string userName = string.Empty;
        string password = string.Empty;
        string proxyAddress = string.Empty;
        string proxyPort = string.Empty;
        string proxyUsername = string.Empty;
        string proxyPassword = string.Empty;

        public static bool AddProfilePic_Unique = false;
        public static bool AddProfilePic_Random = false;
        public static List<string> lstpicfile = new List<string>();
        public Events LinkedInProfileManagerLogEvents = new Events();
        static int counter_PicSelected = 0; 
        #endregion

        #region LinkedinProfileImgUploader
        public LinkedinProfileImgUploader()
        {
        } 
        #endregion

        #region LinkedinProfileManager
        public LinkedinProfileImgUploader(string UserName, string Password, string ProxyAddress, string ProxyPort, string ProxyUserName, string ProxyPassword)
        {
            userName = UserName;
            password = Password;
            proxyAddress = ProxyAddress;
            proxyPort = ProxyPort;
            proxyUsername = ProxyUserName;
            proxyPassword = ProxyPassword;
        } 
        #endregion

        #region SetProfilePic
        public void SetProfilePic(ref GlobusHttpHelper httpHelper)
        {
            try
            {
                string homePageSource = httpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com"));
                string ProfileID = GetProfileId(ref httpHelper, homePageSource);
                string Url = "http://www.linkedin.com/profile/view?id=" + ProfileID + "&trk=tab_pro";
                string ProfilePageSource = httpHelper.getHtmlfromUrl1(new Uri(Url));

                if (!string.IsNullOrEmpty(homePageSource))
                {
                    try
                    {
                        if (ProfilePageSource.Contains("link__nprofileEdit"))
                        {
                            try
                            {
                                string link__nprofileEdit = httpHelper.GetUniqueKeyBasedValue(ProfilePageSource, "link__nprofileEdit");
                            }
                            catch { }
                        }

                        if (ProfilePageSource.Contains("link__editPictureInfo"))
                        {
                            try
                            {
                                string link__editPictureInfo = httpHelper.GetUniqueKeyBasedValue(ProfilePageSource, "link__editPictureInfo");
                            }
                            catch { }
                        }
                        string refere = string.Empty;
                        try
                        {
                            UploadPic(ref httpHelper, ProfilePageSource, refere);
                        }
                        catch { }

                    }
                    catch { }
                }
            }
            catch { }

            finally
            {
                Log("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                Log("--------------------------------------------------------------------------------------------------------------------------------------------");
            }
        } 
        #endregion

        #region UploadPic
        private void UploadPic(ref GlobusHttpHelper httpHelper, string loggedinPageSource, string referer)
        {
            try
            {
                string localImagePath = "";
                string imagePath = string.Empty;

                try
                {
                    Interlocked.Increment(ref counter_PicSelected);
                    if (counter_PicSelected >= lstpicfile.Count)
                    {
                        counter_PicSelected = 0;
                    }
                    //localImagePath = lstpicfile[new Random().Next(0, lstpicfile.Count)];
                    localImagePath = lstpicfile[counter_PicSelected];
                }
                catch { }

                Log("[ " + DateTime.Now + " ] => [ Images : " + localImagePath + " Select For UserName : " + userName + " ]");

                string status = string.Empty;
                string profileId = GetProfileId(ref httpHelper, loggedinPageSource);

                string postUrl = "http://www.linkedin.com/mupld/upload?goback=%2Enpv_" + profileId + "_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1%2Enpe_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1_*1";

                string[] upload_infoArr = Regex.Split(loggedinPageSource, " name=\"upload_info");  // name="upload_info"
                string upload_info = string.Empty;
                if (upload_infoArr.Count() > 1)
                {
                    try
                    {
                        upload_info = upload_infoArr[1].Substring(upload_infoArr[1].IndexOf("value="), upload_infoArr[1].IndexOf("id=") - upload_infoArr[1].IndexOf("value=")).Replace("value=", string.Empty).Replace("\"", string.Empty).Replace("\\", string.Empty).Trim();
                    }
                    catch { }
                }
                else
                {
                    try
                    {
                        string[] upload_infoArr1 = Regex.Split(loggedinPageSource, "id\":\"upload_info");
                        upload_info = upload_infoArr1[1].Substring(upload_infoArr1[1].IndexOf("value\":"), upload_infoArr1[1].IndexOf("type") - upload_infoArr1[1].IndexOf("value\":")).Replace("value\":", string.Empty).Replace("\"", string.Empty).Replace("\\", string.Empty).Replace(",", string.Empty).Trim();
                    }
                    catch { }
                }
                try
                {
                    bool PostStatus = false;
                    try
                    {
                        PostStatus = httpHelper.SetProfilePic(ref httpHelper, profileId, userName, password, localImagePath, proxyAddress, proxyPort, proxyUsername, proxyPassword, ref status, upload_info);
                    }
                    catch { }
                    if (PostStatus)
                    {
                        GlobusFileHelper.AppendStringToTextfileNewLine("Profile Picture Changed Successfully To UserName : " + userName + " In Image Path : " + localImagePath, Globals.path_ProfPicSuccess);
                        Log("[ " + DateTime.Now + " ] => [ Profile Picture Changed Successfully To UserName : " + userName + " In Image Path : " + localImagePath + " ]");
                    }
                    else
                    {
                        GlobusFileHelper.AppendStringToTextfileNewLine("Profile Picture Not Changed Successfully To UserName : " + userName + " In Image Path : " + localImagePath, Globals.path_ProfPicFail);
                        Log("[ " + DateTime.Now + " ] => [ Profile Picture Not Changed Successfully To UserName : " + userName + " In Image Path : " + localImagePath + " ]");
                    }

                }
                catch { }
            }
            catch
            {
            }
        } 
        #endregion

        #region GetProfileId
        public string GetProfileId(ref GlobusHttpHelper httpHelper, string loggedInPageSource)
        {
            string profileId = string.Empty;
            try
            {
                //profileId = httpHelper.GetUniqueKeyBasedValue(loggedInPageSource, "user_id:");
                profileId = httpHelper.GetUniqueKeyBasedValue(loggedInPageSource, "<li class=\"nav-item account-settings-tab\">");
            }
            catch
            {
            }
            return profileId;
        } 
        #endregion

        #region Log
        private void Log(string message)
        {
            try
            {
                EventsArgs eventArgs = new EventsArgs(message);
                LinkedInProfileManagerLogEvents.LogText(eventArgs);
            }
            catch
            {
            }
        } 
        #endregion
    }
}
