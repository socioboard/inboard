using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinkedInDominator;
using System.Text.RegularExpressions;
using System.Threading;
using BaseLib;

namespace LinkedInDominator
{
    public class ClsStatusImageShare
    {
      
        #region global declaration
        string userName = string.Empty;
        string password = string.Empty;
        string proxyAddress = string.Empty;
        string proxyPort = string.Empty;
        string proxyUsername = string.Empty;
        string proxyPassword = string.Empty;

        public static bool AddStatusImage_Unique = false;
        public static bool AddStatusImage_Random = false;
        public static List<string> lstpicfile = new List<string>();
        public static Queue<string> Que_Image_Post = new Queue<string>();
        public Events LinkedInStatusImageLogEvents = new Events();
        static int counter_PicSelected = 0;
        #endregion

      

      
        #region Log
        private void Log(string message)
        {
            try
            {
                EventsArgs eventArgs = new EventsArgs(message);
                LinkedInStatusImageLogEvents.LogText(eventArgs);
            }
            catch
            {
            }
        }
        #endregion
    }
}
