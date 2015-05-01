using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using BaseLib;

namespace Campaign
{
   public class CampaignAccountManager
    {
        public string Username = string.Empty;
        public string Password = string.Empty;

        public string userID = string.Empty;
        public string postAuthenticityToken = string.Empty;

        public string Screen_name = string.Empty;
        public string FollowerCount = string.Empty;

        public string proxyAddress = string.Empty;
        public string proxyPort = string.Empty;
        public string proxyUsername = string.Empty;
        public string proxyPassword = string.Empty;

        public string proxyAddress_Socks5 = string.Empty;
        public string proxyPort_Socks5 = string.Empty;
        public string proxyUsername_Socks5 = string.Empty;
        public string proxyPassword_Socks5 = string.Empty;

        public string AccountStatus = string.Empty;
        public bool IsLoggedIn = false;
        public bool IsNotSuspended = false;
        public bool Isnonemailverifiedaccounts = false;

        Regex IdCheck1 = new Regex("^[0-9]*$");
        public Events logEvents = new Events();
        public GlobusHttpHelper globusHttpHelper = new GlobusHttpHelper();

        public CampaignAccountManager()
        {
        }

        public CampaignAccountManager(string Username, string Password, string Screen_name, string follower_Count, int numberOfMessages, string proxyAddress, string proxyPort, string proxyUsername, string proxyPassword, string currentCity, string HomeTown, string Birthday_Month, string BirthDay_Date, string BirthDay_Year, string AboutMe, string Employer, string College, string HighSchool, string Religion, string profilePic, string FamilyName, string Role, string language, string sex, string activities, string interests, string movies, string music, string books, string favoriteSports, string favoriteTeams, string GroupName, string status)
        {
            this.Username = Username;
            this.Password = Password;
            this.proxyAddress = proxyAddress;
            this.proxyPort = proxyPort;
            this.proxyUsername = proxyUsername;
            this.proxyPassword = proxyPassword;
            this.Screen_name = Screen_name;
            this.FollowerCount = follower_Count;
            //this.GroupName = GroupName;
            this.AccountStatus = status;
            //Log("[ " + DateTime.Now + " ] => [ Login in with " + Username + " ]");
        }
    }
}
