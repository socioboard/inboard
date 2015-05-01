using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using InBoardPro;
using System.Threading;
using System.Reflection;
using BaseLib;

namespace InBoardPro
{
    public partial class frmSheduler : Form
    {
        clsDBQueryManager queryManager = new clsDBQueryManager();
        public static Events Event_StartScheduler = new Events();
        public static Events SchedulerLogger = new Events();

        #region Stopping Variables

        string threadNaming_SearchkeywordInvites_ = "SearchkeywordInvites_";
        //string threadNaming_Tweet_ = "Tweet_";
        //string threadNaming_Retweet_ = "Retweet_";
        //string threadNaming_Reply_ = "Reply_";
        //string threadNaming_Follow_ = "Follow_";
        //string threadNaming_Unfollow_ = "Unfollow_";
        //string threadNaming_ProfileManager_ = "ProfileManager_";

        #endregion

        public frmSheduler()
        {
            InitializeComponent();
        }

        private void frmSheduler_Load(object sender, EventArgs e)
        {
            dgvScheduler.DataError += new DataGridViewDataErrorEventHandler(dgvScheduler_DataError);
            LoadDataGrid();
        }

        void dgvScheduler_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            Console.WriteLine("Error in dgvScheduler_DataError");
        }

        public void LoadDataGrid()
        {
            try
            {
                dgvScheduler.Invoke(new MethodInvoker(delegate
                {
                    dgvScheduler.DataSource = queryManager.SelectAllFromTBScheduler();
                }));
            }
            catch { }
        }

        private void btnstartScheduler_Click(object sender, EventArgs e)
        {
            new Thread(() =>
            {
                //Check Scheduler is not Null or Empty ...
                try
                {
                    if ((dgvScheduler.Rows.Count) < 2)
                    {
                        MessageBox.Show("Not scheduled any task.");
                        return;
                    }
                }
                catch (Exception)
                {
                }


                MessageBox.Show("Please don't close this Form for Scheduler to keep running. You can minimize it and do other tasks on Main Form though.");

                btnstartScheduler.Invoke(new MethodInvoker(delegate
                {
                    btnstartScheduler.Enabled = false;
                }));


                while (true)
                {
                    RunScheduledTasks();

                    //Thread.Sleep(90000);
                }
            }).Start();
        }

        private void RunScheduledTasks()
        {
            try
            {
                List<string> listModules = queryManager.SelectUnaccomplishedPastScheduledTimeFromTBScheduler();

                foreach (string module in listModules)
                {
                    modModule(module);
                    //Update TBScheduler Set IsAccomplished = 1
                    queryManager.UpdateTBScheduler(module);

                    LoadDataGrid();
                }
            }
            catch { }
        }

        public string strModule(BaseLib.Module module)
        {
            switch (module)
            {
                case BaseLib.Module.SearchkeywordInvites:
                    return threadNaming_SearchkeywordInvites_;

                //case BaseLib.Module.Tweet:
                //    return threadNaming_Tweet_;

                //case BaseLib.Module.Retweet:
                //    return threadNaming_Retweet_;

                //case BaseLib.Module.Reply:
                //    return threadNaming_Reply_;

                //case BaseLib.Module.Follow:
                //    return threadNaming_Follow_;

                //case BaseLib.Module.Unfollow:
                //    return threadNaming_Unfollow_;

                //case BaseLib.Module.ProfileManager:
                //    return threadNaming_ProfileManager_;

                default:
                    return "";
            }



        }

      public void modModule(string module)
      {
            try
            {
                switch (module)
                {
                    case "SearchkeywordInvites_":
                        Log("[ " + DateTime.Now + " ] => [ Scheduler started for : " + module + " ]");
                        RaiseSchedulerEvent(BaseLib.Module.SearchkeywordInvites);
                        break;

                    //case "Tweet_":
                    //    Log("[ " + DateTime.Now + " ] => [ Scheduler started for : " + module + " ]");
                    //    RaiseSchedulerEvent(BaseLib.Module.Tweet);
                    //    //return Module.Tweet;
                    //    break;

                    //case "Retweet_":
                    //    //return Module.Retweet;
                    //    break;

                    //case "Follow":
                    //    //return Module.Follow;
                    //    Log("[ " + DateTime.Now + " ] => [ Scheduler started for : " + module + " ]");
                    //    RaiseSchedulerEvent(BaseLib.Module.Follow);
                    //    break;

                    //case "Unfollow_":
                    //    //return Module.Unfollow;
                    //    break;

                    //case "ProfileManager_":
                    //    //return Module.ProfileManager;
                    //    break;

                    //case Module.ProfileManager:
                    //    return threadNaming_ProfileManager_;

                    default:
                        break;
                    //return Module.None;
                }
            }
            catch (Exception ex)
            {
                //GlobusFileHelper.AppendStringToTextfileNewLine("Error -->  modModule(string module) switch (module)--> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
        }



      private void RaiseSchedulerEvent(BaseLib.Module module)
        {
            EventsArgs eArgs = new EventsArgs(module);
            Event_StartScheduler.RaiseScheduler(eArgs);
        }

        private void Log(string log)
        {
            EventsArgs eArgs = new EventsArgs(log);
            SchedulerLogger.LogText(eArgs);
        }

        private void dgvScheduler_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

    }
}
