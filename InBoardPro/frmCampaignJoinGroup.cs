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
using System.Drawing.Drawing2D;
using System.IO;
using Campaign;
using ManageConnections;
using BaseLib;

namespace InBoardPro
{
    public partial class frmCampaignJoinGroup : Form
    {
        public System.Drawing.Image image;

        clsDBQueryManager queryManager = new clsDBQueryManager();
        DataSet CompaignsDataSet = new DataSet();

        String _CmpName = String.Empty;
        String _AccountFilePath = String.Empty;
        String _KeywordFilePath = String.Empty;
        string _SearchType = string.Empty;
        string _IsScheduledDaily = string.Empty;
        string _DelayFrom = string.Empty;
        string _DelayTo = string.Empty;
        public bool _IsFollowProcessStart = true;
        public int counterThreadsCampaignFollow = 0;

        public frmCampaignJoinGroup()
        {
            InitializeComponent();
        }

        private void frmCampaignJoinGroup_Load(object sender, EventArgs e)
        {
            image = Properties.Resources.background;
            ConnectUsingSearchKeywod.ConnectSearchLogEvents.addToLogger += new EventHandler(loggerAddConnection_addToLogger);

            //ConnectUsingSearchKeywod.CampaignSearchLogevents.addToLogger += new EventHandler(CampaignnameLog);

            //ConnectUsingSearchKeywod.CampaignStartLogevents.addToLogger += new EventHandler(FeatureNameLogLog);

            try
            {
                LoadCampaign();
            }
            catch { }

            if (CompaignsDataSet.Tables.Count != 0 && CompaignsDataSet.Tables[0].Rows.Count != 0)
            {
                new Thread(() =>
                {
                    ScheduledTasks();
                }).Start();
            }
        }

        public void LoadCampaign()
        {
            dgv_campaign.Invoke(new MethodInvoker(delegate
            {
                dgv_campaign.Rows.Clear();
                dgv_campaign.Refresh();
            }));

            this.dgv_campaign.AllowUserToAddRows = true;
            CompaignsDataSet.Clear();

            CompaignsDataSet = queryManager.SelectCamapaignData();
            DataView dataView = new DataView();
            dataView.Table = CompaignsDataSet.Tables[0];

            DataTable newTable = dataView.Table.DefaultView.ToTable(true, "CampaignName", "KeywordFilePath");

            for (int i = 0; i < dataView.Table.Rows.Count; i++)
            {
                DataGridViewRow row = (DataGridViewRow)dgv_campaign.Rows[0].Clone();
                row.Cells[0].Value = dataView.Table.Rows[i]["CampaignName"].ToString();
                row.Cells[1].Value = dataView.Table.Rows[i]["SearchType"].ToString();
                //row.Cells[3].Value = (System.Drawing.Image)Properties.Resources.on;

                var result = cls_variables.Lst_WokingThreads.ContainsKey(newTable.Rows[i]["CampaignName"].ToString());
                if (result)
                {
                    row.Cells[3].Value = (System.Drawing.Image)Properties.Resources.off;
                }
                else
                {
                    row.Cells[3].Value = (System.Drawing.Image)Properties.Resources.on;
                }

                dgv_campaign.Invoke(new MethodInvoker(delegate
                {
                    try
                    {
                        dgv_campaign.Rows.Add(row);
                    }
                    catch (Exception)
                    {
                    }
                }));
            }
            this.dgv_campaign.AllowUserToAddRows = false;
        }

        #region ScheduledTask
        public void ScheduledTasks()
        {
            DataRow[] CampDataRows = CompaignsDataSet.Tables[0].Select("IsSheduleDaily = '1'");

            if (CampDataRows.Count() == 0)
            {
                AddLoggerManageConnection("[ " + DateTime.Now + " ] => [ Not scheduled any Task. ]");
                return;
            }

            MessageBox.Show("Some task is scheduled. Please do not close your Campaign manager window.");
            string Module = string.Empty;
            foreach (DataRow CampRow in CampDataRows)
            {
                string CampaignName = CampRow["CampaignName"].ToString();
                if (rdbCampaignJoinGroupKeyword.Checked)
                {
                    Module = CampRow["KeywordFilePath"].ToString();
                }

                if (rdbCampaignJoinGroupURL.Checked)
                {
                    Module = CampRow["UrlFilePath"].ToString();
                }
                foreach (DataGridViewRow DgvRow in dgv_campaign.Rows)
                {
                    var GridRowCampName = DgvRow.Cells[0].Value;
                    int rowindex = DgvRow.Index;
                    if (CampaignName == GridRowCampName)
                    {
                        dgv_campaign.Invoke(new MethodInvoker(delegate
                        {
                            dgv_campaign.Rows[rowindex].Cells[3].Value = Properties.Resources.off;
                        }));
                    }
                }

                //Start Camapign 

                StartCampaign(CampaignName,  Module);
            }
        }

        #endregion

        public void StartCampaign(string CampaignName, string Module)
        {
            try
            {
                cls_variables.Lst_WokingThreads.Add(CampaignName, Thread.CurrentThread);
            }
            catch (Exception)
            {
            }

            DataRow[] drModelDetails = CompaignsDataSet.Tables[0].Select("CampaignName = '" + CampaignName + "'");

            if (drModelDetails.Count() == 0)
            {
                return;
            }

            //Get 1st row from arrey 
            DataRow DrCampaignDetails = drModelDetails[0];
            try
            {
                bool IsSchedulDaily = (Convert.ToInt32(DrCampaignDetails.ItemArray[9]) == 1) ? true : false;
                DateTime SchedulerStartTime = Convert.ToDateTime(DrCampaignDetails.ItemArray[11].ToString());
                DateTime SchedulerEndTime = Convert.ToDateTime(DrCampaignDetails.ItemArray[12].ToString());
            }
            catch
            { }

            StartProcess(CompaignsDataSet, CampaignName);

        }


        public void StartProcess(DataSet CampaignDataSet, string CampaignName)
        {
            try
            {
                DataRow[] drModelDetails = CompaignsDataSet.Tables[0].Select("CampaignName = '" + CampaignName + "'");

                DataRow DrCampaignDetails = drModelDetails[0];
                _CmpName = string.Empty;
                _CmpName = DrCampaignDetails.ItemArray[1].ToString();
                string AcFilePath = DrCampaignDetails.ItemArray[2].ToString();
                string SearchType = DrCampaignDetails.ItemArray[3].ToString();
                string KeywordFilePath = DrCampaignDetails.ItemArray[4].ToString();
                string UrlFilePath = DrCampaignDetails.ItemArray[5].ToString();
                int NoOfInviteUser = Convert.ToInt32(DrCampaignDetails.ItemArray[5]);

                bool IsSchedulDaily = (Convert.ToInt32(DrCampaignDetails.ItemArray[6]) == 1) ? true : false;
                //int Threads = Convert.ToInt32(DrCampaignDetails.ItemArray[10]);
                DateTime SchedulerStartTime = Convert.ToDateTime(DrCampaignDetails.ItemArray[7].ToString());
                DateTime SchedulerEndTime = Convert.ToDateTime(DrCampaignDetails.ItemArray[8].ToString());
                int DelayStart = Convert.ToInt32(DrCampaignDetails.ItemArray[9]);
                int DelayEnd = Convert.ToInt32(DrCampaignDetails.ItemArray[10]);

                List<string> _lstUserAccounts = new List<string>();

                if (!File.Exists(AcFilePath))
                {
                    AddLoggerManageConnection("[ " + DateTime.Now + " ] => [ Account File Doesn't Exist. Please change account File. ]");
                    return;
                }

                _lstUserAccounts = GlobusFileHelper.ReadFile(AcFilePath);

                AddLoggerManageConnection("[ " + DateTime.Now + " ] => [ " + _lstUserAccounts.Count + " Accounts is uploaded. ]");

                clsAccountManager objclsAccountManager = new clsAccountManager();
                                
                CampaignAccountContainer objCampaignFollowAccountContainer = objclsAccountManager.AccountManager(_lstUserAccounts);

                foreach (var item in objCampaignFollowAccountContainer.dictionary_CampaignAccounts)
                {
                    try
                    {

                        //Manage no of threads
                        //if (counterThreadsCampaignFollow >= Threads)
                        //{
                        //    lock (lockerThreadsCampaignFollow)
                        //    {
                        //        Monitor.Wait(lockerThreadsCampaignFollow);
                        //    }
                        //}

                        Thread threadGetStartProcessForFollow = new Thread(GetStartProcessForFollow);
                        threadGetStartProcessForFollow.Name = CampaignName + "_" + item.Key;
                        threadGetStartProcessForFollow.IsBackground = true;
                        threadGetStartProcessForFollow.Start(new object[] { item, SearchType, KeywordFilePath, UrlFilePath, DelayStart, DelayEnd, CampaignName, IsSchedulDaily, SchedulerStartTime, SchedulerEndTime});

                        Thread.Sleep(1000);

                        //LstCounter++;
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            catch
            { }
        }

        public void GetStartProcessForFollow(object paramarray)
        {
            string CampaignName = string.Empty;
            try
            {
                Interlocked.Increment(ref counterThreadsCampaignFollow);

                string Account = string.Empty;
                Array paramsArray = new object[5];
                paramsArray = (Array)paramarray;

                KeyValuePair<string, CampaignAccountManager> keyValuePair = (KeyValuePair<string, CampaignAccountManager>)paramsArray.GetValue(0);
                LinkedinLogin Login = new LinkedinLogin();
                //KeyValuePair<string, LinkedInMaster> item = (KeyValuePair<string, LinkedInMaster>)paramsArray.GetValue(0);
                GlobusHttpHelper HttpHelper = new GlobusHttpHelper();
                LinkedInMaster LinkedIn_Master1 = new LinkedInMaster();
                CampaignAccountManager LinkedIn_Master = keyValuePair.Value;
                string linkedInKey = keyValuePair.Key;
                Account = keyValuePair.Key;

                string SearchType = (string)paramsArray.GetValue(1);

                int KeywordFilePath = (int)paramsArray.GetValue(2);

                int UrlFilePath = (int)paramsArray.GetValue(3);

                int DelayStart = (int)paramsArray.GetValue(4);

                int DelayEnd = (int)paramsArray.GetValue(5);

                CampaignName = (string)paramsArray.GetValue(6);

                bool IsSchedulDaily = (bool)paramsArray.GetValue(7);

                DateTime SchedulerStartTime = (DateTime)paramsArray.GetValue(7);

                DateTime SchedulerEndTime = (DateTime)paramsArray.GetValue(8);

                //bool IsInviteperDay = (bool)paramsArray.GetValue(8);

                //int NumberOfInivitePerDay = (int)paramsArray.GetValue(9);

                //string Message = (string)paramsArray.GetValue(10);

                //bool Isspintax = (bool)paramsArray.GetValue(11);
                //Initialize Values in Local Variable 

                string accKey = (string)keyValuePair.Key;
                CampaignAccountManager campACCManager = (CampaignAccountManager)keyValuePair.Value;



                //Add List Of Working thread
                //we are using this list when we stop/abort running camp processes..
                try
                {
                    cls_variables.Lst_WokingThreads.Add(CampaignName + "_" + campACCManager.Username, Thread.CurrentThread);
                }
                catch (Exception)
                {
                }

                
                // 
                ////////
                // JOIN GROUP METHOD!!
                ///////
                //
            }
            catch (Exception)
            {
            }
            finally
            {
                ////count_AccountforCampaignFollower--;
                //Interlocked.Decrement(ref counterThreadsCampaignFollow);
                //lock (lockerThreadsCampaignFollow)
                //{
                //    Monitor.Pulse(lockerThreadsCampaignFollow);
                //}
                if (counterThreadsCampaignFollow == 0)
                {
                    AddLoggerManageConnection("[ " + DateTime.Now + " ] => [ Process is completed for Campaign " + CampaignName + " ]");
                    //_IsFollowProcessStart = true;
                }
            }
        }

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

        private void btn_UpdateCampaignJoinGroup_Click(object sender, EventArgs e)
        {
            string Result = Validations("Update");
            //if (string.IsNullOrEmpty(_InviteMsg))
            //{
            //    _InviteMsg = txtInviteMsg.Text.Trim();
            //}

            if (!string.IsNullOrEmpty(Result))
            {
                MessageBox.Show(Result);
            }
            else
            {
                btn_UpdateCampaign();
            }
        }

        public void btn_UpdateCampaign()
        {
            try
            {
                string query = "UPDATE tb_CamapignJoinGroup SET AccountFilePath ='" + _AccountFilePath + "',SearchType='" + _SearchType + "',KeywordFilePath='" + _KeywordFilePath + "'"
                        + " , UrlFilePath= '" + _KeywordFilePath + "' , IsSheduleDaily='" + _IsScheduledDaily + "' ,DelayFrom='" + _DelayFrom + "',DelayTo='" + _DelayTo + "' WHERE CampaignName='" + _CmpName + "';";



                clsDBQueryManager queryManager = new clsDBQueryManager();

                queryManager.InsertCamapaignData(query, "tb_CamapignJoinGroup");

                AddLoggerManageConnection("[ " + DateTime.Now + " ] => [  " + _CmpName + " is Saved. ]");

                LoadCampaign();
                ///Clear campaign 
                ClearCamapigns();
            }
            catch (Exception ex)
            {
            }
        }

        #region ClearCampaignData
        public void ClearCamapigns()
        {
            ///Clear Text box of campaign Name
            txt_CampaignName.Invoke(new MethodInvoker(delegate
            {
                txt_CampaignName.Text = "";
                txt_CampaignName.ReadOnly = false;
                _CmpName = string.Empty;
            }));


            // clear account file Text box 
            txt_accountfilepath.Invoke(new MethodInvoker(delegate
            {
                txt_accountfilepath.Text = "";
            }));

            //txtUploadKeyword.Invoke(new MethodInvoker(delegate
            //{
            //    txtUploadKeyword.Text = "";
            //}));

            //Clear selected feature ...
            #region commented

            //txt_campNoOfInvitesParAc.Invoke(new MethodInvoker(delegate
            //{
            //    txt_campNoOfInvitesParAc.Text = "";
            //}));

            //txt_campMaximumNoRetweet.Invoke(new MethodInvoker(delegate
            //{
            //    txt_campMaximumNoRetweet.Text = "";
            //}));

            #endregion

            chk_InvitePerDay.Invoke(new MethodInvoker(delegate
            {
                chk_InvitePerDay.Checked = false;
            }));    
                           
                              
           

            //txtSearchMindelay.Invoke(new MethodInvoker(delegate { txtSearchMindelay.Text = "20"; }));

            //txtSearchMaxDelay.Invoke(new MethodInvoker(delegate { txtSearchMaxDelay.Text = "25"; }));

            
        }
        #endregion

        #region validateOnSaveCampaign
        public string Validations(string Function)
        {
            string Result = string.Empty;
            try
            {
                if (Function == "Save")
                {
                    if (String.IsNullOrEmpty(txt_CampaignName.Text))
                    {
                        MessageBox.Show("Please Enter Campaign name.");
                        return Result = "Please Enter Campaign name.";
                    }
                    else
                    {
                        try
                        {
                            DataRow[] Drow = CompaignsDataSet.Tables[0].Select("CampaignName = '" + txt_CampaignName.Text + "'");
                            if (Drow.Count() != 0)
                            {
                                MessageBox.Show("Please enter different name of campaign.");
                                return Result = "Please enter different name of campaign.";
                            }
                        }
                        catch (Exception)
                        {
                        }

                        _CmpName = (txt_CampaignName.Text);
                    }
                }
                else
                {
                    _CmpName = (txt_CampaignName.Text);
                }


                if (String.IsNullOrEmpty(txt_accountfilepath.Text))
                {
                    MessageBox.Show("Please Select Account File.");
                    return Result = "Please Enter Account File.";
                }
                else
                    _AccountFilePath = (txt_accountfilepath.Text);

                if (!(rdbCampaignJoinGroupKeyword.Checked) || !(rdbCampaignJoinGroupURL.Checked))
                {
                    MessageBox.Show("Please Select Search Type.");
                    return Result = "Please Select Search Type.";
                }

                if (rdbCampaignJoinGroupKeyword.Checked)
                {
                    if (String.IsNullOrEmpty(TxtGroupURL.Text))
                    {
                        MessageBox.Show("Please Select Keyword File.");
                        return Result = "Please Enter Keyword File.";
                    }
                    else
                        _AccountFilePath = (txt_accountfilepath.Text);
                }

                if (rdbCampaignJoinGroupURL.Checked)
                {
                    if (String.IsNullOrEmpty(TxtGroupURL.Text))
                    {
                        MessageBox.Show("Please Select URL File.");
                        return Result = "Please Enter URL File.";
                    }
                    else
                        _AccountFilePath = (txt_accountfilepath.Text);
                }

                //if (String.IsNullOrEmpty(dateTimePicker_Start_SearchWithInvite.Text))
                //{
                //    MessageBox.Show("Delay start from process.");
                //    return Result = "Delay start from process.";
                //}
                //else
                //    _StartFrom = dateTimePicker_Start_SearchWithInvite.Text;

                //if (String.IsNullOrEmpty(dateTimePicker_End.Text))
                //{
                //    MessageBox.Show("Delay End Time (In Second)");
                //    return Result = "Delay End Time (In Second)";
                //}
                //else
                //    _EndTo = dateTimePicker_End.Text;

                //if (String.IsNullOrEmpty(txtSearchMindelay.Text))
                //{
                //    MessageBox.Show("Please Enter Delay Start Time");
                //    return Result = "Please Enter Delay Start Time";
                //}
                //else
                //    _DelayFrom = int.Parse(txtSearchMindelay.Text);

                //if (String.IsNullOrEmpty(txtSearchMaxDelay.Text))
                //{
                //    MessageBox.Show("Please Enter Delay End Time");
                //    return Result = "Please Enter Delay End Time";
                //}
                //else
                //    _DelayTo = int.Parse(txtSearchMaxDelay.Text);

                //if (cls_variables._DelayFrom > cls_variables._DelayTo)
                //{
                //    MessageBox.Show("Please Enter Currect value in delay.");
                //    return Result = "Please Enter Currect value in delay.";
                //}

                //if (chkboxSearchWithInviteScheduledDaily.Checked == true)
                //{
                //    var startTime = dateTimePicker_Start_SearchWithInvite.Value;
                //    var endtime = dateTimePicker_End.Value;
                //    if (endtime > startTime)
                //    {
                //        _StartFrom = startTime.ToString();
                //        _EndTo = endtime.ToString();
                //    }
                //    else
                //    {
                //        MessageBox.Show("Please Enter Currect time/ End Time is grater than Start Time", "Warning");
                //        return Result = "Please Enter Currect time/ End Time is grater than Start Time.";
                //    }
                //}



            }
            catch { };

            return Result;
        }
        #endregion
    }
}
