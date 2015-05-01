using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using BaseLib;

namespace Campaign
{
   public class cls_variables
    {
        public static Dictionary<string, LinkedInMaster> linkedInDictionary = new Dictionary<string, LinkedInMaster>();

        public static Dictionary<string, Thread> Lst_WokingThreads = new Dictionary<string, Thread>();

        public static string _StartFrom { get; set; }

        public static string _EndTo { get; set; }

        public static int _DelayFrom = 20;

        public static int _DelayTo = 25;

    }
}
