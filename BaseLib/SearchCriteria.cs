using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace BaseLib
{
    public static class SearchCriteria
    {
        #region global declaration
        public static string Keyword = string.Empty;
        public static string FirstName = string.Empty;
        public static string LastName = string.Empty;
        public static string Title = string.Empty;
        public static string Company = string.Empty;
        public static string CurrentTitle = string.Empty;
        public static string CurrentCompany = string.Empty;
        public static string Location = string.Empty;
        public static string Country = string.Empty;
        //public static string LocationAreaCode = string.Empty;
        public static string LocationArea = string.Empty;
        public static string OtherLocation = string.Empty;
        public static string Education = string.Empty;
        public static string PostalCode = string.Empty;
        public static string FileName = string.Empty;
        public static int NumberOfRequestPerEmail = 0;
        public static int NumberOfRequestPerKeyword = 0;
        public static List<string> statusMsg = new List<string>();
        public static string CreateGroupStatus = string.Empty;
        public static string GroupType = string.Empty;
        public static string GroupLang = string.Empty;
        public static bool starter = false;
        public static bool SignIN = false;
        public static bool SignOut = true;
        public static bool find = false;
        public static bool Duplication = false;
        public static string pplSearchOrigin = string.Empty;
        public static string IndustryType = string.Empty;
        public static string Group = string.Empty;
        public static string SeniorLevel = string.Empty;
        public static string language = string.Empty;
        public static string Function = string.Empty;
        public static string CompanySize = string.Empty;
        public static string Relationship = string.Empty;
        public static string InerestedIn = string.Empty;
        public static string YearOfExperience = string.Empty;
        public static string RecentlyJoined = string.Empty;
        public static string Fortune1000 = string.Empty;
        public static string Follower = string.Empty;
        public static string loginREsponce = string.Empty;
        public static string LoginID = string.Empty;
        public static string CsrToken = string.Empty;
        public static string within = string.Empty;
        public static string TitleValue = string.Empty;
        public static string CompanyValue = string.Empty;
        public static int RequestLimit = 0;

        public static bool NumberOfrequestPerDay = false;
        public static int AlreadyRequestedUser = 0;

        public static int scraperMinDelay = 20;
        public static int scraperMaxDelay = 25;

        public static string AccountType = string.Empty;
        #endregion
            
    }
}
