using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Data;
using System.IO;
using BaseLib;

namespace ManageConnections
{
   public class StartCampaignConnectWithSearchKeyword
    {
       public Events CampaignConnectionLogEvents = new Events();
       String _CmpName = String.Empty;

       public void StartProcess(DataSet CompaignsDataSet, string CampaignName)
       {
           try
           {
               DataRow[] drModelDetails = CompaignsDataSet.Tables[0].Select("CampaignName = '" + CampaignName + "'");

               if (drModelDetails.Count() == 0)
               {

               }

               //Get 1st row from arrey 
               DataRow DrCampaignDetails = drModelDetails[0];

               //Set all Details 


               _CmpName = string.Empty;
               _CmpName = DrCampaignDetails.ItemArray[1].ToString();
               string AcFilePath = DrCampaignDetails.ItemArray[2].ToString();
               string Keyword = DrCampaignDetails.ItemArray[3].ToString();
               string Message = DrCampaignDetails.ItemArray[4].ToString();
               int NoOfInviteUser = Convert.ToInt32(DrCampaignDetails.ItemArray[5]);
               bool IsSpinTax = (Convert.ToInt32(DrCampaignDetails.ItemArray[6]) == 1) ? true : false;
               bool IsInviteParDay = (Convert.ToInt32(DrCampaignDetails.ItemArray[7]) == 1) ? true : false;
               int NoofInviteParDay = Convert.ToInt32(DrCampaignDetails.ItemArray[8]);
               bool IsSchedulDaily = (Convert.ToInt32(DrCampaignDetails.ItemArray[9]) == 1) ? true : false;
               int Threads = Convert.ToInt32(DrCampaignDetails.ItemArray[10]);
               DateTime SchedulerStartTime = Convert.ToDateTime(DrCampaignDetails.ItemArray[11].ToString());
               DateTime SchedulerEndTime = Convert.ToDateTime(DrCampaignDetails.ItemArray[12].ToString());
               int DelayStar = Convert.ToInt32(DrCampaignDetails.ItemArray[13]);
               int DelayEnd = Convert.ToInt32(DrCampaignDetails.ItemArray[14]);

               List<string> _lstUserAccounts = new List<string>();
               List<string> _lstFollowersName = new List<string>();
               List<List<string>> list_lstTargetUsers = new List<List<string>>();

               //classes.Cls_AccountsManager Obj_AccManger = new Cls_AccountsManager();

               //Check Files is existing...
               if (!File.Exists(Keyword))
               {
                   Log("[ " + DateTime.Now + " ] => [ Keyword File Doesn't Exixst. Please change account File. ]");
                   return;
               }
               else if (!File.Exists(AcFilePath))
               {
                   Log("[ " + DateTime.Now + " ] => [ Keyword File Doesn't Exixst. Please change account File. ]");
                   return;
               }

                 

               //set Max thread 
               ThreadPool.SetMaxThreads(Threads, Threads);

               int LstCounter = 0;
               foreach (var item in objCampaignFollowAccountContainer.dictionary_CampaignAccounts)
               {
                   try
                   {
                       //check list for breaking loop 
                       //if list of follow members list is completed.
                       if (LstCounter == list_lstTargetUsers.Count && (dividebyUser || divideEql))
                       {
                           Log("[ " + DateTime.Now + " ] => [ Account is grater than List of users. ]");
                           break;
                       }
            
                       //get event from accountmanager class
                       // and create Event for printing log messages 
                       ((CampaignAccountManager)item.Value).logEvents.addToLogger += new EventHandler(logEvents_addToLogger);


                       //Manage no of threads
                       if (counterThreadsCampaignFollow >= NoOfThreads)
                       {
                           lock (lockerThreadsCampaignFollow)
                           {
                               Monitor.Wait(lockerThreadsCampaignFollow);
                           }
                       }

                       Thread threadGetStartProcessForFollow = new Thread(GetStartProcessForFollow);
                       threadGetStartProcessForFollow.Name = CampaignName + "_" + item.Key;
                       threadGetStartProcessForFollow.IsBackground = true;
                       threadGetStartProcessForFollow.Start(new object[] { item, list_lstTargetUsers_item, NoOfFollowPerAc, DelayStar, DelayEnd, CampaignName, IsSchedulDaily, SchedulerEndTime, divideEql, dividebyUser, FollowFilePath });

                       Thread.Sleep(1000);

                       LstCounter++;
                   }
                   catch (Exception)
                   {
                   }
               }
           }
           catch (Exception)
           {
           }
       }

       void logEvents_addToLogger(object sender, EventArgs e)
       {
           if (e is EventsArgs)
           {
               EventsArgs eArgs = e as EventsArgs;
               Log(eArgs.log);
           }
       }

       private void Log(string message)
       {
           EventsArgs eventArgs = new EventsArgs(message);
           CampaignConnectionLogEvents.LogText(eventArgs);
       }
    }
}
