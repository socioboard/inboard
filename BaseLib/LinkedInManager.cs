using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaseLib
{
    public static class LinkedInManager
    {
        /// <summary>
        /// Contains all the accounts and related Information
        /// </summary>
        public static Dictionary<string, LinkedInMaster> linkedInDictionary = new Dictionary<string, LinkedInMaster>();

        public static Dictionary<string, LinkedInMaster> linkedInDictionaryExcelInput = new Dictionary<string, LinkedInMaster>();

        public static string Licence_Details = string.Empty;
    }
}
