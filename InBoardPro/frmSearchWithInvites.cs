using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using InBoardPro;
using ManageConnections;
using System.Drawing.Drawing2D;
using BaseLib;

namespace InBoardPro
{
    public partial class frmSearchWithInvites : Form
    {
        public System.Drawing.Image image;
        bool CheckNetConn = false;
        bool IsStop = false;
        List<Thread> lstSearchconnectionThread = new List<Thread>();
        List<string> UpdatelistGreetMessatge = new List<string>();
        List<string> MessagelistCompose = new List<string>();
        public int counter_GroupMemberSearch = 0;
        public static Events Event_StartScheduler = new Events();
        List<string> _lstConnectionSearchKeyword = new List<string>();
        static int counter_Search_connection = 0;
     
        public frmSearchWithInvites()
        {
            InitializeComponent();
        }

        void event_StartScheduler_raiseScheduler(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eArgs = e as EventsArgs;
                //AddToGeneralLogs(eArgs.log);
                StartScheduler(eArgs.module);
            }
        }

        private void StartScheduler(Module module)
        {
            switch (module)
            {
                case Module.SearchkeywordInvites:
                    SearchConnectionKeywordThread();
                    break;

                default:
                    break;
            }
        }

        private void frmSearchWithInvites_Load(object sender, EventArgs e)
        {
            image = Properties.Resources.background;
            ConnectUsingSearchKeywod.ConnectSearchLogEvents.addToLogger += new EventHandler(loggerAddConnection_addToLogger);
            frmSheduler.Event_StartScheduler.raiseScheduler += new EventHandler(event_StartScheduler_raiseScheduler);
            frmSheduler.SchedulerLogger.addToLogger += new EventHandler(loggerAddConnection_addToLogger);
        }


        #region AddLoggerManageConnection

        private void AddLoggerManageConnection(string log)
        {
            this.Invoke(new MethodInvoker(delegate
            {
                lbManageConnection.Items.Add(log);
                lbManageConnection.SelectedIndex = lbManageConnection.Items.Count - 1;
            }));
        }

        void loggerAddConnection_addToLogger(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eventArgs = e as EventsArgs;
                AddLoggerManageConnection(eventArgs.log);
            }
        }
        #endregion

        private void BtnUploadProfileURL_Click(object sender, EventArgs e)
        {
            try
            {
                txtInviteKeyword.Text = "";
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "Text Files (*.txt)|*.txt";
                    ofd.InitialDirectory = Application.StartupPath;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtInviteKeyword.Text = ofd.FileName;
                        _lstConnectionSearchKeyword.Clear();
                        List<string> templist = GlobusFileHelper.ReadFiletoStringList(ofd.FileName);

                        foreach (string item in templist)
                        {
                            if (!_lstConnectionSearchKeyword.Contains(item))
                            {
                                if (!string.IsNullOrEmpty(item.Replace(" ", "").Replace("\t", "")))
                                {
                                    _lstConnectionSearchKeyword.Add(item);
                                    
                                }
                            }
                        }
                    }
                    AddLoggerManageConnection("[ " + DateTime.Now + " ] => [ " + _lstConnectionSearchKeyword.Count + " Keywords Loaded  ]");
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Connection --> btnKeywordLoadFile_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Connection --> btnKeywordLoadFile_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddConnectionErrorLogs);
            }
        }


        private void btnSendEmailInvite_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {

                try
                {
                    lstSearchconnectionThread.Clear();
                    if (IsStop)
                    {
                        IsStop = false;
                    }

                    if (_lstConnectionSearchKeyword.Count == 0)
                    {
                        MessageBox.Show("Please Add Keywords!");
                        AddLoggerManageConnection("[ " + DateTime.Now + " ] => [ Please Add Keywords! ]");
                        return;
                    }

                    else if (txtInviteMsg.Text.Contains("|"))
                    {
                        if (!(chkUseSpintax_InviteMessage.Checked))
                        {
                            AddLoggerManageConnection("[ " + DateTime.Now + " ] => [ Please Check SpinTax CheckBox.. ]");
                            MessageBox.Show("Please Check SpinTax CheckBox..");
                            return;
                        }

                        if (txtInviteMsg.Text.Contains("{") || txtInviteMsg.Text.Contains("}"))
                        {
                            AddLoggerManageConnection("[ " + DateTime.Now + " ] => [ Its a wrong SpinTax Format.. ]");
                            MessageBox.Show("Its a wrong SpinTax Format..");
                            return;
                        }
                    }

                    if (chkUseSpintax_InviteMessage.Checked)
                    {
                        ConnectUsingSearchKeywod.isSpinTaxSearchWithInvite = true;
                        ConnectUsingSearchKeywod.UpdatelistGreetMessatge = SpinnedListGenerator.GetSpinnedList(new List<string> { txtInviteMsg.Text });
                    }

                    if (!string.IsNullOrEmpty(txt_campNoOfRetweetsParAc.Text) && NumberHelper.ValidateNumber(txt_campNoOfRetweetsParAc.Text))
                    {
                        SearchCriteria.NumberOfRequestPerKeyword = Convert.ToInt32(txt_campNoOfRetweetsParAc.Text);
                    }

                    if ((LinkedInManager.linkedInDictionary.Count > 0 && !string.IsNullOrEmpty(txtInviteKeyword.Text.ToString())))
                    {
                        counter_GroupMemberSearch = LinkedInManager.linkedInDictionary.Count;
                        btnSearchKeyword.Cursor = Cursors.AppStarting;

                        new Thread(() => SearchConnectionKeywordThread()).Start();
                    }
                    else if (string.IsNullOrEmpty(txtInviteKeyword.Text.ToString()))
                    {
                        MessageBox.Show("Please Enter Keywords for search !");
                        AddLoggerManageConnection("[ " + DateTime.Now + " ] => [ Please Enter Keywords for search ! ]");
                        return;
                    }
                  
                    else if ((LinkedInManager.linkedInDictionary.Count == 0))
                    {
                        MessageBox.Show("Please Load LinkedIn Accounts From MyAccounts Menu");
                        AddLoggerManageConnection("[ " + DateTime.Now + " ] => [ Please Load LinkedIn Accounts From MyAccounts Menu ]");
                        frmAccounts FrmAccount = new frmAccounts();
                        FrmAccount.Show();
                        return;
                    }

                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Connection --> btnSearchConnection_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Connection --> btnSearchConnection_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddConnectionErrorLogs);
                }
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddLoggerManageConnection("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        }



        #region SearchKeywordConnectionThread()

        public void SearchConnectionKeywordThread()
        {
            //ConnectUsingSearchKeywod obj_ConnectUsingSearchKeywod = new ConnectUsingSearchKeywod
            int SearchMinDelay = 0;
            int SearchMaxDelay = 0;
            if (!string.IsNullOrEmpty(txtSearchMindelay.Text) && NumberHelper.ValidateNumber(txtSearchMindelay.Text))
            {
                SearchMinDelay = Convert.ToInt32(txtSearchMindelay.Text);
            }
            if (!string.IsNullOrEmpty(txtSearchMaxDelay.Text) && NumberHelper.ValidateNumber(txtSearchMaxDelay.Text))
            {
                SearchMaxDelay = Convert.ToInt32(txtSearchMaxDelay.Text);
            }

       
            if (LinkedInManager.linkedInDictionary.Count > 0 && !string.IsNullOrEmpty(txtInviteKeyword.Text.ToString()))
            {
                int SetThread = 5;
                if (!string.IsNullOrEmpty(txtThreadManageConnection.Text) && NumberHelper.ValidateNumber(txtThreadManageConnection.Text))
                {
                    SetThread = Convert.ToInt32(txtThreadManageConnection.Text);
                }

                if (LinkedInManager.linkedInDictionary.Count > 0 && !string.IsNullOrEmpty(txtInviteKeyword.Text.ToString()))
                {
                    AddLoggerManageConnection("[ " + DateTime.Now + " ] => [ Process Start... ]");

                    counter_Search_connection = LinkedInManager.linkedInDictionary.Count;

                    try
                    {
                        foreach (KeyValuePair<string, LinkedInMaster> item in LinkedInManager.linkedInDictionary)
                        {
                            ThreadPool.SetMaxThreads(SetThread, 5);
                            ThreadPool.QueueUserWorkItem(new WaitCallback(SendInviteUsingKeyWords), new object[] { item, SearchMinDelay, SearchMaxDelay });
                        }
                    }
                    catch (Exception ex)
                    {
                        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Connection --> btnSearchConnection_Click() --> ---2--- >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                        GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Connection --> btnSearchConnection_Click() --> ---2--- >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddConnectionErrorLogs);
                    }
                }
                else
                {
                    AddLoggerManageConnection("[ " + DateTime.Now + " ] => [ Please Enter Keywords! ]");
                }
            }
        }

        #endregion

        #region SendInviteUsingKeyWords

        public void SendInviteUsingKeyWords(object Parameter)
        {
            try
            {
                if (IsStop)
                {
                    return;
                }

                if (!IsStop)
                {
                    lstSearchconnectionThread.Add(Thread.CurrentThread);
                    lstSearchconnectionThread.Distinct().ToList();
                    Thread.CurrentThread.IsBackground = true;
                }
            }
            catch
            {
            }
            
            string Account = string.Empty;
            Array paramsArray = new object[1];
            paramsArray = (Array)Parameter;

            KeyValuePair<string, LinkedInMaster> item = (KeyValuePair<string, LinkedInMaster>)paramsArray.GetValue(0);

            int SearchMinDelay = Convert.ToInt32(paramsArray.GetValue(1));
            int SearchMaxDelay = Convert.ToInt32(paramsArray.GetValue(2));
            GlobusHttpHelper HttpHelper = new GlobusHttpHelper();
            LinkedinLogin Login = new LinkedinLogin();

            LinkedInMaster LinkedIn_Master = item.Value;
            string linkedInKey = item.Key;
            Account = item.Key;

            Login.logger.addToLogger += new EventHandler(loggerAddConnection_addToLogger);

            if (chk_InvitePerDay.Checked)
            {
                AddLoggerManageConnection("[ " + DateTime.Now + " ] => [ Checking Invites Per Day ]");
                SearchCriteria.NumberOfRequestPerKeyword = 0;
                SearchCriteria.NumberOfrequestPerDay = true;
                if (!string.IsNullOrEmpty(txt_campMaximumNoRetweet.Text) && NumberHelper.ValidateNumber(txt_campMaximumNoRetweet.Text))
                {
                    SearchCriteria.NumberOfRequestPerKeyword = Convert.ToInt32(txt_campMaximumNoRetweet.Text);
                    AddLoggerManageConnection("[ " + DateTime.Now + " ] => [ " + SearchCriteria.NumberOfRequestPerKeyword + " Maximum No Of Invites Per Day ]");
                }
                else
                {
                    SearchCriteria.NumberOfRequestPerKeyword = 10;
                    AddLoggerManageConnection("[ " + DateTime.Now + " ] => [ Setting Maximum No Of Invites Per Day as 10 ]");
                }

                clsDBQueryManager DbQueryManager = new clsDBQueryManager();
                DataSet Ds = DbQueryManager.SearchWithInvite(Account, "SearchWithInvite");

                int TodayTweet = Ds.Tables["tb_SearchWithInvite"].Rows.Count;
                SearchCriteria.AlreadyRequestedUser = TodayTweet;
                AddLoggerManageConnection("[ " + DateTime.Now + " ] => [ " + TodayTweet + " Already Invites today ]");

                if (TodayTweet >= SearchCriteria.NumberOfRequestPerKeyword)
                {
                    AddLoggerManageConnection("[ " + DateTime.Now + " ] => [ Already Invited : " + SearchCriteria.NumberOfRequestPerKeyword + " ]");
                    AddLoggerManageConnection("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                    return;
                }
            }

            
            try
            {
                {
                    Queue<string> que_SearchKeywords = new Queue<string>();

                    foreach (string itemKeyword in _lstConnectionSearchKeyword)
                    {
                        que_SearchKeywords.Enqueue(itemKeyword);
                    }

                    ManageConnections.ConnectUsingSearchKeywod ConnectUsing_Search = new ConnectUsingSearchKeywod(item.Value._Username, item.Value._Password, item.Value._ProxyAddress, item.Value._ProxyPort, item.Value._ProxyUsername, item.Value._ProxyPassword, que_SearchKeywords);
                    ManageConnections.ConnectUsing_Search.SearchUsingkeywordForInvite(ref ConnectUsing_Search, SearchMinDelay, SearchMaxDelay);
                }

            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Connection --> SendInviteUsingKeyWords() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Add Connection --> SendInviteUsingKeyWords() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAddConnectionErrorLogs);
            }
            finally
            {
                counter_Search_connection--;
                if (counter_Search_connection == 0)
                {
                    if (btnSearchKeyword.InvokeRequired)
                    {
                        btnSearchKeyword.Invoke(new MethodInvoker(delegate
                        {
                            AddLoggerManageConnection("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                            AddLoggerManageConnection("---------------------------------------------------------------------------------------------------------------------------");
                            btnSearchKeyword.Cursor = Cursors.Default;

                        }));
                    }
                }
            }
        }

        #endregion

        #region frmSearchWithInvites_Paint
        private void frmSearchWithInvites_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                Graphics g;
                g = e.Graphics;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.DrawImage(image, 0, 0, this.Width, this.Height);
            }
            catch
            {
            }
        } 
        #endregion

        frmSheduler scheduler = new frmSheduler();
        private void btnScheduleForLater_SearchWithInvite_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime dt_scheduledTime = DateTime.Parse(dateTimePicker_Start_SearchWithInvite.Value.ToString());

                if (DateTime.Now < dt_scheduledTime)
                {
                   
                     string IsScheduledDaily = "0";
                    if (chkboxSearchWithInviteScheduledDaily.Checked)
                    {
                        IsScheduledDaily = "1";
                    }

                    clsDBQueryManager queryManager = new clsDBQueryManager();
                    queryManager.InsertUpdateTBScheduler("SearchkeywordInvites_", dateTimePicker_Start_SearchWithInvite.Value.ToString(), IsScheduledDaily);

                    if (scheduler != null && scheduler.IsDisposed == false)
                    {
                        scheduler.LoadDataGrid();
                    }
                }
                else
                {
                    MessageBox.Show("Scheduled Tasks Can Only Be Saved in the Future Timing");
                    AddLoggerManageConnection("[ " + DateTime.Now + " ] => [ Scheduled Tasks Can Only Be Saved in the Future Timing]");
                }

            }
            catch { }
        }

        private void btnShedulerStart_Click(object sender, EventArgs e)
        {
            frmSheduler obj_frmSheduler = new frmSheduler();
            obj_frmSheduler.Show();
        }


        private void btnEmailInvite_Click(object sender, EventArgs e)
        {
            try
            {
                IsStop = true;
                List<Thread> lstTemp = lstSearchconnectionThread.Distinct().ToList();
                foreach (Thread item in lstTemp)
                {
                    try
                    {
                        item.Abort();
                    }
                    catch
                    {
                    }

                }
                AddLoggerManageConnection("-------------------------------------------------------------------------------------------------------------------------------");
                AddLoggerManageConnection("[ " + DateTime.Now + " ] => [ PROCESS STOPPED ]");
                AddLoggerManageConnection("-------------------------------------------------------------------------------------------------------------------------------");
                btnSearchKeyword.Cursor = Cursors.Default;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error >>> " + ex.StackTrace);
            }

        }

       

    }
}
