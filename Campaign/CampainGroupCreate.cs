using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Data;


namespace Campaign
{
  public class CampainGroupCreate
    {
     
        #region global declaration for account create and profile picture upload

        public static List<string> Campaign_listFirstName = new List<string>();
        public static List<string> Campaign_listLastName = new List<string>();
        public static List<string> Campaign_Email = new List<string>();
        public static List<string> Campaign_Country = new List<string>();
        public static List<string> Campaign_Pincode = new List<string>();
        public static List<string> Campaign_Jobtitle = new List<string>();
        public static List<string> Campaign_Company = new List<string>();
        public static List<string> Campaign_Industry = new List<string>();
        public static List<string> Campaign_ProxyList = new List<string>();
        public static List<string> Campaign_CollegeList = new List<string>();
        public static List<Thread> Campaign_lstAccountCraterThread = new List<Thread>();

        public static bool Campaign_IsStop_AccountCraterThread = false;
        public static bool Campaign_ChkCountry = false;
        public static bool Campaign_ChkIndustry = false;
        public static bool Campaign_ChkType = false;
        public static bool Campaign_chkManualCaptcha = false;
        public static bool Campaign_chkUseProxyPool = false;

        public static string Campain_selected_Proxy = string.Empty;
        public static string Campain_Account_Creator_Threads = string.Empty;
        public static string Campain_First_Name = string.Empty;
        public static string Campain_Last_Name = string.Empty;
        public static string Campain_Emails = string.Empty;
        public static string Campain_Pincode = string.Empty;
        public static string Campain_JobTitle = string.Empty;
        public static string Campain_Company = string.Empty;
        public static string Campain_College = string.Empty;

        public string Campaign_firstNameSettings = string.Empty;
        public string Campaign_lastNameSettings = string.Empty;
        public string Campaign_emailSettings = string.Empty;
        public string Campaign_PincodeSetting = string.Empty;
        public string Campaign_jobTitleSetting = string.Empty;
        public string Campaign_companySetting = string.Empty;
        public string Campaign_ProxySetting = string.Empty;
        public string Campaign_CollegeSetting = string.Empty;

        public static string Campaign_IndustrySetting = string.Empty;
        public static string Campaign_AccountSetting = string.Empty;
        public static string Campaign_CountrySetting = string.Empty;

        public static List<string> Campaign_lstAccountType = new List<string>();  //string.Empty;
        public static List<string> Campaign_lstIndustry = new List<string>();//string.Empty;
        public static List<string> Campaign_lstCountry = new List<string>();//string.Empty; 

        public static bool Campaign_ChkProfilePict = false;
        public static List<string> lstProfilePic = new List<string>();

        #endregion

        #region global declaration for Connection Search Keyword
        public static List<string> Campaign_lstConnectionSearchKeyword = new List<string>(); 
        #endregion
     
        #region global declaration for Search Group
        public static string SearchKeyword = string.Empty;
        public static string GroupCount = string.Empty;
        public static List<string> lstSearchGroup = new List<string>();
        public static List<string> lstAddSearchGroup = new List<string>();
        public static Dictionary<string, string> Result = new Dictionary<string, string>();
        public static Dictionary<string, Dictionary<string, string>> LinkdInContacts = new Dictionary<string, Dictionary<string, string>>(); 
        #endregion
    }
}
