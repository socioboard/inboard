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
    public partial class frmCampaignSearchWithInvite : Form
    {

        #region
        public System.Drawing.Image image;
             
        clsDBQueryManager queryManager = new clsDBQueryManager();
        String _CmpName = String.Empty;
        String _AccountFilePath = String.Empty;
        String _KeywordFilePath = String.Empty;
        public bool _IsFollowProcessStart = true;
        public int counterThreadsCampaignFollow = 0;
        
        public readonly object lockerThreadsCampaignFollow = new object();

        public  string _StartFrom { get; set; }

        public  string _EndTo { get; set; }

        public  string _InviteMsg { get; set; }

        public  int _DelayFrom = 20;

        public  int _DelayTo = 25;

        public int _NoofInvitesParAC = 10;

        public int _IsInviteParDay = 0;

        public int _IsSpintaxChecked = 0;

        public  int _MaxNoOfInvitesParDay = 10;

        public  int _IsScheduledDaily = 0;
        DataSet CompaignsDataSet = new DataSet();
        int Threads = 7;
        public static int difference = 0;
        #endregion

        public frmCampaignSearchWithInvite()
        {
            InitializeComponent();
        }

        #region FormLoad
        private void frmCampaignSearchWithInvite_Load(object sender, EventArgs e)
        {
            image = Properties.Resources.background;
            ConnectUsingSearchKeywod.ConnectSearchLogEvents.addToLogger += new EventHandler(loggerAddConnection_addToLogger);

            ConnectUsingSearchKeywod.CampaignSearchLogevents.addToLogger += new EventHandler(CampaignnameLog);

            ConnectUsingSearchKeywod.CampaignStartLogevents.addToLogger += new EventHandler(FeatureNameLogLog);
          
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

        void CampaignnameLog(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eArgs = e as EventsArgs;
                StoprunningCampaign(eArgs.log);
            }
        }

        void FeatureNameLogLog(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eArgs = e as EventsArgs;
                StartCampaign(eArgs.log,eArgs.log);
            }
        }
        #endregion

        #region SaveCampaignOnbuttonClick
        private void btn_savecampaign_Click(object sender, EventArgs e)
        {
            Globals.btnSaveClickedCampaignSearchWithInvite = true;
            string Result = Validations("Save");
            if (string.IsNullOrEmpty(_InviteMsg))
            {
                _InviteMsg = txtInviteMsg.Text.Trim();
            }
            if (!string.IsNullOrEmpty(Result))
            {
                AddLoggerManageConnection("[ " + DateTime.Now + " ] => [ " + Result + " ]");
            }
            else
            {
                SaveFollowSettings();

            }
        } 
        #endregion


        #region Save Data of Campaign Save Method ()
        public void SaveFollowSettings()
        {
            try
            {
                string query = "Insert into 'tb_CamapignSearcWithInvite'(CampaignName, AcFilePath, KeywordFilePath, Message,ISUseSpinTax, IsInviteParDay, NumberOfInviteParDay,NoofInvite,  IsSheduleDaily, StartTime, EndTime, DelayFrom, DelayTo, NoOfThread)"
                    + " values ('" + _CmpName + "','" + _AccountFilePath + "','" + _KeywordFilePath + "','" + _InviteMsg + "','" + _IsSpintaxChecked + "','" + _IsInviteParDay + "'" 
                    +",'" + _MaxNoOfInvitesParDay + "','" + _NoofInvitesParAC + "','" + _IsScheduledDaily + "','" + _StartFrom + "','" + _EndTo + "'"
                    +",'" + _DelayFrom + "', '" + _DelayTo + "', '" + Threads + "')";


                    clsDBQueryManager queryManager = new clsDBQueryManager();

                    queryManager.InsertCamapaignData(query, "tb_CamapignSearcWithInvite");

                //AddToCampaignLoggerListBox("[ " + DateTime.Now + " ] => [  " + _CmpName + " is Saved. ]");

                //Reload all Saved campaign....
                LoadCampaign();

                //Re- start scheduled campaigns ... 
                ScheduledTasks();

                ///Clear campaign 
                ClearCamapigns();
            }
            catch (Exception)
            {
            }
        } 
        #endregion


        #region StopRunningCampaign
        public void StoprunningCampaign(String CampignName)
        {
            try
            {
                Dictionary<string, Thread> temp_WorkingThreads = new Dictionary<string, Thread>();

                Dictionary<string, Thread> selectedValues = cls_variables.Lst_WokingThreads
                    .Where(x => (x.Key.Contains(CampignName)))
                    .ToDictionary(x => x.Key, x => x.Value);


                if (selectedValues.Count != 0)
                {
                    foreach (KeyValuePair<string, Thread> selectedValues_item in selectedValues)
                    {
                        String _ThreadKey = selectedValues_item.Key;
                        Thread _ThreadValue = selectedValues_item.Value;

                        //if (Lst_WokingThreads.ContainsKey(_ThreadKey))
                        //{
                        //    Lst_WokingThreads.Remove(_ThreadKey);
                        //}

                        if (cls_variables.Lst_WokingThreads.ContainsKey(_ThreadKey))
                        {
                            cls_variables.Lst_WokingThreads.Remove(_ThreadKey);
                        }

                        if (LinkedInManager.linkedInDictionary.ContainsKey(_ThreadKey))
                        {
                            LinkedInManager.linkedInDictionary.Remove(_ThreadKey);

                        }

                        _ThreadValue.Abort();
                        LinkedInManager.linkedInDictionary.Clear();
                    }

                    AddLoggerManageConnection("[ " + DateTime.Now + " ] => [ Process is aborted for campaign :- " + CampignName + " ]");
                }



            }
            catch (Exception)
            {
            }
        } 
        #endregion

        #region sheduledTask
        public void ScheduledTasks()
        {
            
            DataRow[] CampDataRows = CompaignsDataSet.Tables[0].Select("IsSheduleDaily = '1'");

            if (CampDataRows.Count() == 0)
            {
                AddLoggerManageConnection("[ " + DateTime.Now + " ] => [ Not scheduled any Task. ]");
                return;
            }


            //MessageBox.Show("Some task is scheduled. Please do not close your Campaign manager window.");
            foreach (DataRow CampRow in CampDataRows)
            {
                string CampaignName = string.Empty;
                string Module = string.Empty;
                if (Globals.btnSaveClickedCampaignSearchWithInvite)
                {
                    CampaignName = _CmpName;
                    Module = _KeywordFilePath;
                }
                else
                {
                    CampaignName = CampRow["CampaignName"].ToString();
                    Module = CampRow["KeywordFilePath"].ToString();
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
                new Thread(() =>
                  {
                        StartCampaign(CampaignName, Module);
                }).Start();
                break;
            }

        } 
        #endregion

        #region LoadCampaignData
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
                row.Cells[1].Value = dataView.Table.Rows[i]["KeywordFilePath"].ToString();
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
        #endregion

        #region GetImageOff and On value on Campaign
        public string GetImageValue(Bitmap bitmap)
        {
            string Img = string.Empty;
            try
            {
                Bitmap bmp = new Bitmap(1, 1);
                using (Graphics g = Graphics.FromImage(bmp))
                {

                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.DrawImage(bitmap, new Rectangle(0, 0, 1, 1));
                }
                Color pixel = bmp.GetPixel(0, 0);
                // pixel will contain average values for entire orig Bitmap
                byte avgR = pixel.R;

                if (avgR.Equals(75))
                {
                    Img = "ON";
                }
                else if (avgR == 212)
                {
                    Img = "OFF";
                }
            }
            catch { };

            return Img;
        } 
        #endregion

        #region getBrowseData on Buttonupload click 
        private void btn_uploadaccounts_Click(object sender, EventArgs e)
        {
            using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
            {
                ofd.Filter = "Text Files (*.txt)|*.txt";
                ofd.InitialDirectory = Application.StartupPath;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txt_accountfilepath.Invoke(new MethodInvoker(delegate
                    {
                        txt_accountfilepath.Text = ofd.FileName;
                        _AccountFilePath = ofd.FileName;
                    }));
                }
            }
        }

        private void btnUploadKeyword_Click(object sender, EventArgs e)
        {
            //using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
            //{
            //    ofd.Filter = "Text Files (*.txt)|*.txt";
            //    ofd.InitialDirectory = Application.StartupPath;
            //    if (ofd.ShowDialog() == DialogResult.OK)
            //    {
            //        txt_accountfilepath.Invoke(new MethodInvoker(delegate
            //        {
            //            txt_accountfilepath.Text = ofd.FileName;
            //            _KeywordFilePath = ofd.FileName;
            //        }));
            //    }
            //}
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

                if (String.IsNullOrEmpty(txtUploadKeyword.Text))
                {
                    MessageBox.Show("Please Select Keyword File.");
                    return Result = "Please Enter Keyword File.";
                }
                else
                    _AccountFilePath = (txt_accountfilepath.Text);


                if (String.IsNullOrEmpty(dateTimePicker_Start_SearchWithInvite.Text))
                {
                    MessageBox.Show("Delay start from process.");
                    return Result = "Delay start from process.";
                }
                else
                    _StartFrom = dateTimePicker_Start_SearchWithInvite.Text;

                if (String.IsNullOrEmpty(dateTimePicker_End.Text))
                {
                    MessageBox.Show("Delay End Time (In Second)");
                    return Result = "Delay End Time (In Second)";
                }
                else
                    _EndTo = dateTimePicker_End.Text;

                if (String.IsNullOrEmpty(txtSearchMindelay.Text))
                {
                    MessageBox.Show("Please Enter Delay Start Time");
                    return Result = "Please Enter Delay Start Time";
                }
                else
                    _DelayFrom = int.Parse(txtSearchMindelay.Text);

                if (String.IsNullOrEmpty(txtSearchMaxDelay.Text))
                {
                    MessageBox.Show("Please Enter Delay End Time");
                    return Result = "Please Enter Delay End Time";
                }
                else
                    _DelayTo = int.Parse(txtSearchMaxDelay.Text);

                if (cls_variables._DelayFrom > cls_variables._DelayTo)
                {
                    MessageBox.Show("Please Enter Currect value in delay.");
                    return Result = "Please Enter Currect value in delay.";
                }

                if (chkboxSearchWithInviteScheduledDaily.Checked == true)
                {
                    var startTime = dateTimePicker_Start_SearchWithInvite.Value;
                    var endtime = dateTimePicker_End.Value;
                    if (endtime > startTime)
                    {
                        _StartFrom = startTime.ToString();
                        _EndTo = endtime.ToString();
                    }
                    else
                    {
                        MessageBox.Show("Please Enter Currect time/ End Time is grater than Start Time", "Warning");
                        return Result = "Please Enter Currect time/ End Time is grater than Start Time.";
                    }
                }



            }
            catch { };

            return Result;
        } 
        #endregion

        #region AddLoggerManageConnection

        private void AddLoggerManageConnection(string log)
        {
            //try
            //{
            //    if (this.InvokeRequired)
            //    {
            //        this.Invoke(new MethodInvoker(delegate
            //        {
            //            lbManageConnection.Items.Add(log);
            //            lbManageConnection.SelectedIndex = lbManageConnection.Items.Count - 1;
            //        }));
            //    }
            //    else
            //    {
            //        lbManageConnection.Items.Add(log);
            //        lbManageConnection.SelectedIndex = lbManageConnection.Items.Count - 1;
            //    }
            //}
            //catch
            //{ }

            try
            {
                if (lbManageConnection.InvokeRequired)
                {
                    lbManageConnection.Invoke(new MethodInvoker(delegate
                    {
                        lbManageConnection.Items.Add(log);
                        lbManageConnection.SelectedIndex = lbManageConnection.Items.Count - 1;
                    }
                    ));
                }
                else
                {
                    lbManageConnection.Items.Add(log);
                    lbManageConnection.SelectedIndex = lbManageConnection.Items.Count - 1;
                }
            }
            catch { }
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

        #region EnterControlData and Store into Variable
        private void chk_InvitePerDay_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked == true)
            {
                _IsInviteParDay = 1;
                //Grp_TweetParDay.Invoke(new MethodInvoker(delegate
                //{
                //    label85.Visible = true;
                //    txtMaximumTweet.Visible = true;
                //}));
            }
            else
            {
                _IsInviteParDay = 0;
                //Grp_TweetParDay.Invoke(new MethodInvoker(delegate
                //{
                //    label85.Visible = false;
                //    txtMaximumTweet.Visible = false;
                //}));
            }
        }

        private void txt_campMaximumNoRetweet_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int res;
                var IsNumber = int.TryParse(((TextBox)sender).Text, out res);
                if (IsNumber)
                {
                    if (!String.IsNullOrEmpty(((TextBox)sender).Text) && int.Parse(((TextBox)sender).Text) >= 10)
                        _MaxNoOfInvitesParDay = int.Parse((((TextBox)sender).Text));
                    else if (!String.IsNullOrEmpty(((TextBox)sender).Text) && int.Parse(((TextBox)sender).Text) < 10)
                    {
                        _MaxNoOfInvitesParDay = int.Parse((((TextBox)sender).Text));
                    }
                    else
                    {
                        MessageBox.Show("Please Enter Currect value in Text box.");
                    }
                }
                else
                {
                    MessageBox.Show("Please enter numeric value in Text box.");
                    txt_campMaximumNoRetweet.Text = "10";
                }
            }
            catch { };
        }

        private void txtThreadManageConnection_Validating(object sender, CancelEventArgs e)
        {
            String NoOfthread = ((TextBox)sender).Text;
            if (!String.IsNullOrEmpty(NoOfthread))
            {
                //if ((7 <= int.Parse(NoOfthread)) && int.Parse(NoOfthread) != 0)
                if (int.Parse(NoOfthread) != 0)
                    Threads = int.Parse(NoOfthread);
                else
                {
                    MessageBox.Show("Please Enter number of threads.");
                    Threads = 7;
                    txtThreadManageConnection.Text = "7";
                }
            }
            else
            {
                MessageBox.Show("Please Enter number of threads.");
            }
        }

        private void chkUseSpintax_InviteMessage_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked == true)
            {
                _IsSpintaxChecked = 1;

            }
            else
            {
                _IsSpintaxChecked = 0;

            }
        }

        private void txt_campNoOfInvitesParAc_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int res;
                var IsNumber = int.TryParse(((TextBox)sender).Text, out res);
                if (IsNumber)
                {
                    if (!String.IsNullOrEmpty(((TextBox)sender).Text) && int.Parse(((TextBox)sender).Text) >= 10)
                        _NoofInvitesParAC = int.Parse((((TextBox)sender).Text));
                    else if (!String.IsNullOrEmpty(((TextBox)sender).Text) && int.Parse(((TextBox)sender).Text) < 10)
                    {
                        _NoofInvitesParAC = int.Parse((((TextBox)sender).Text));
                    }
                    else
                    {
                        MessageBox.Show("Please Enter Currect value in Text box.");
                    }
                }
                else
                {
                    MessageBox.Show("Please enter numeric value in Text box.");
                    txt_campNoOfInvitesParAc.Text = "10";
                }
            }
            catch { };
        }

        private void txtInviteMsg_TextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(((TextBox)sender).Text))
             _InviteMsg = (((TextBox)sender).Text);
         
        
        }
              
        private void chkboxSearchWithInviteScheduledDaily_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked == true)
            {
                _IsScheduledDaily = 1;
                // lb_ScheduleTime.Enabled = true;
                //lb_To.Enabled = true;
                dateTimePicker_Start_SearchWithInvite.Enabled = true;
                dateTimePicker_End.Enabled = true;
            }
            else
            {
                _IsScheduledDaily = 0;
                _StartFrom = "";
                _EndTo = "";
                //lb_ScheduleTime.Enabled = false;
                //lb_To.Enabled = false;
                dateTimePicker_Start_SearchWithInvite.Enabled = false;
                dateTimePicker_End.Enabled = false;
            }
        }

        private void txtUploadKeyword_TextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(((TextBox)sender).Text))
                _KeywordFilePath = (((TextBox)sender).Text);
        } 
        #endregion

        #region CamapaignData save and Edit
        private void dgv_campaign_CellClick(object sender, DataGridViewCellEventArgs e)
        {
             if (e.ColumnIndex == 2)
            {
                #region ---- Edit Campaign's ----


                btnUpdate.Enabled = true;

                string CampaignName = dgv_campaign.Rows[e.RowIndex].Cells[0].FormattedValue.ToString();
                string FeaturName = dgv_campaign.Rows[e.RowIndex].Cells[1].FormattedValue.ToString();

                // Your code would go here below is just the code I used to test 
                Bitmap ImgBitmap = (Bitmap)(dgv_campaign.Rows[e.RowIndex].Cells[3].FormattedValue);
                string Img = GetImageValue(ImgBitmap);

                if (Img == "OFF")
                {
                    AddLoggerManageConnection("[ " + DateTime.Now + " ] => [  " + CampaignName + " is running. Please stop process before Editing. ]");
                    MessageBox.Show(CampaignName + " is running. Please stop process before Editing.", "Warning");
                    return;
                }

                new Thread(() =>
                {
                    editCampaign(CampaignName, FeaturName);
                }).Start();

                #endregion
            }
            else if (e.ColumnIndex == 3)
            {

                if (dgv_campaign.Columns[e.ColumnIndex].Name == "BtnOnOff")
                {
                    // Your code would go here below is just the code I used to test 
                    Bitmap ImgBitmap = (Bitmap)(dgv_campaign.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                    string Img = GetImageValue(ImgBitmap);

                    string CampaignName = dgv_campaign.Rows[e.RowIndex].Cells[0].FormattedValue.ToString();
                    string FeaturName = dgv_campaign.Rows[e.RowIndex].Cells[1].FormattedValue.ToString();

                    ConnectUsingSearchKeywod.FeatureName = FeaturName;
                    if (Img == "ON")
                    {
                        #region ---- Start Camapign's ----

                        dgv_campaign.Invoke(new MethodInvoker(delegate
                        {
                            dgv_campaign.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = (System.Drawing.Image)Properties.Resources.off;
                        }));

                        //Start Processes according to User Selection 
                        AddLoggerManageConnection("[ " + DateTime.Now + " ] => [ Start Campaign :- " + CampaignName + " ]");


                        new Thread(() =>
                        {
                            StartCampaign(CampaignName, FeaturName);
                        }).Start();

                        #endregion
                    }
                    else if (Img == "OFF")
                    {
                        #region ---- Stop campaign's ----
                        //If process is already running 
                        // and user wants to stop all process 
                        // then click off option and aborting all running processes 
                        dgv_campaign.Invoke(new MethodInvoker(delegate
                        {
                            dgv_campaign.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = (System.Drawing.Image)Properties.Resources.on;
                        }));

                        new Thread(() =>
                        {
                            StoprunningCampaign(CampaignName);
                        }).Start();

                        #endregion
                    }
                    else
                    {
                        return;
                    }
                }
            }//End if Colum Index ==3
        } 
        #endregion

        #region StartCampaignData
        public void StartCampaign(String CampaignName, String featurName)
        {
            //Add List Of Working thread
            //we are using this list when we stop/abort running camp processes..
            try
            {
                if (!cls_variables.Lst_WokingThreads.ContainsKey(CampaignName))
                {
                    cls_variables.Lst_WokingThreads.Add(CampaignName, Thread.CurrentThread);
                }
                else
                {
                    cls_variables.Lst_WokingThreads[CampaignName] = Thread.CurrentThread;
                    
                }
            }
            catch (Exception)
            {
            }

            //Get Detals from Data Set table by Campaign Name
            DataRow[] drModelDetails = CompaignsDataSet.Tables[0].Select("CampaignName = '" + CampaignName + "'");

            if (drModelDetails.Count() == 0)
            {
                return;
            }

            //Get 1st row from arrey 
            DataRow DrCampaignDetails = drModelDetails[0];
            bool IsSchedulDaily = (Convert.ToInt32(DrCampaignDetails.ItemArray[9]) == 1) ? true : false;
            DateTime SchedulerStartTime = Convert.ToDateTime(DrCampaignDetails.ItemArray[11].ToString());
            DateTime SchedulerEndTime = Convert.ToDateTime(DrCampaignDetails.ItemArray[12].ToString());

         
            if (IsSchedulDaily)
            {
                bool Flag = true;
                if(Flag == true)
                {
                    try
                    {
                        Flag = false;
                        
                        //MessageBox.Show(CampaignName + " task is scheduled. Task start timming  :- " + SchedulerStartTime);

                        AddLoggerManageConnection("[ " + DateTime.Now + " ] => [  " + CampaignName + " task is scheduled. Task start timming :- " + SchedulerStartTime + " ]");
                    }
                    catch
                    { }
                }

                //AddLoggerManageConnection("[ " + DateTime.Now + " ] => [  " + CampaignName + " task is scheduled. Task start timming :- " + SchedulerStartTime + " ]");
                while (true)
                {
                    if (SchedulerStartTime.Hour == (DateTime.Now.Hour) && SchedulerStartTime.Minute == (DateTime.Now.Minute) && (ConnectUsingSearchKeywod._IsFollowProcessStart == true))
                    {
                        //Flage = true;
                        ConnectUsingSearchKeywod._IsFollowProcessStart = false;
                      //new Thread(() =>
                      //{
                          StartProcess(CompaignsDataSet, CampaignName);

                      //}).Start();
                      //break;
                        //if (Flage)
                        //{
                        //    Flage = false;
                        //    difference = Convert.ToInt32(SchedulerEndTime.Second) - (SchedulerStartTime.Second);
                        //    difference = difference + 60 * 1000;
                        //    Thread.Sleep(difference*60*1000);  
                        //}

                    }

                }
            }
            else
            {
                StartProcess(CompaignsDataSet, CampaignName);
            }


        } 
        #endregion

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
              //  difference = Convert.ToInt32(SchedulerEndTime) - Convert.ToInt32(SchedulerStartTime);

                 if (!File.Exists(AcFilePath))
                {
                    AddLoggerManageConnection("[ " + DateTime.Now + " ] => [ Account File Doesn't Exist. Please change account File. ]");
                    return;
                }


                // Get User ID and pass from File ...

                _lstUserAccounts = GlobusFileHelper.ReadFile(AcFilePath);

                AddLoggerManageConnection("[ " + DateTime.Now + " ] => [ " + _lstUserAccounts.Count + " Accounts is uploaded. ]");

                //Get Followers Name 
                clsAccountManager objclsAccountManager = new clsAccountManager();

                //cls_variables.linkedInDictionary = objclsAccountManager.AccountManager(_lstUserAccounts);
                CampaignAccountContainer objCampaignFollowAccountContainer = objclsAccountManager.AccountManager(_lstUserAccounts);
                
                //set Max thread 
                ThreadPool.SetMaxThreads(Threads, Threads);

                //Lst_WokingThreads.Add(CampaignName, Thread.CurrentThread);

                int LstCounter = 0;
                foreach (var item in objCampaignFollowAccountContainer.dictionary_CampaignAccounts)
                {
                    try
                    {

                        //Manage no of threads
                        if (counterThreadsCampaignFollow >= Threads)
                        {
                            lock (lockerThreadsCampaignFollow)
                            {
                                Monitor.Wait(lockerThreadsCampaignFollow);
                            }
                        }

                        Thread threadGetStartProcessForFollow = new Thread(GetStartProcessForFollow);
                        threadGetStartProcessForFollow.Name = CampaignName + "_" + item.Key;
                        threadGetStartProcessForFollow.IsBackground = true;
                        threadGetStartProcessForFollow.Start(new object[] { item, Keyword, NoOfInviteUser, DelayStar, DelayEnd, CampaignName, IsSchedulDaily, SchedulerEndTime, IsInviteParDay, NoofInviteParDay, Message, IsSpinTax });

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


        public void GetStartProcessForFollow(object param)
        {
            string CampaignName = string.Empty;
            try
            {
                Interlocked.Increment(ref counterThreadsCampaignFollow);

                string Account = string.Empty;
                Array paramsArray = new object[5];
                paramsArray = (Array)param;

                KeyValuePair<string, CampaignAccountManager> keyValuePair = (KeyValuePair<string, CampaignAccountManager>)paramsArray.GetValue(0);
                LinkedinLogin Login = new LinkedinLogin();
                //KeyValuePair<string, LinkedInMaster> item = (KeyValuePair<string, LinkedInMaster>)paramsArray.GetValue(0);
                GlobusHttpHelper HttpHelper = new GlobusHttpHelper();
                LinkedInMaster LinkedIn_Master1 = new LinkedInMaster();
                CampaignAccountManager LinkedIn_Master = keyValuePair.Value;
                string linkedInKey = keyValuePair.Key;
                Account = keyValuePair.Key;

                string  keyword = (string)paramsArray.GetValue(1);

                int NoOfFollowPerAc = (int)paramsArray.GetValue(2);

                int DelayStart = (int)paramsArray.GetValue(3);

                int DelayEnd = (int)paramsArray.GetValue(4);

                CampaignName = (string)paramsArray.GetValue(5);

                bool IsSchedulDaily = (bool)paramsArray.GetValue(6);

                DateTime SchedulerEndTime = (DateTime)paramsArray.GetValue(7);

                bool IsInviteperDay = (bool)paramsArray.GetValue(8);

                int NumberOfInivitePerDay = (int)paramsArray.GetValue(9);

                string Message = (string)paramsArray.GetValue(10);

                bool Isspintax = (bool)paramsArray.GetValue(11);
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

                if (Isspintax)
                {
                    ConnectUsingSearchKeywod.isSpinTaxSearchWithInvite = true;
                    ConnectUsingSearchKeywod.UpdatelistGreetMessatge = SpinnedListGenerator.GetSpinnedList(new List<string> { Message });
                }
                else 
                {
                    ConnectUsingSearchKeywod.MessageWithoutspintax = Message;
                }
                

                if (IsInviteperDay)
                {
                   

                    try
                    {
                        AddLoggerManageConnection("[ " + DateTime.Now + " ] => [ Checking Invites Per Day ]");
                    }
                    catch
                    { }
                    SearchCriteria.NumberOfRequestPerKeyword = 0;
                    SearchCriteria.NumberOfrequestPerDay = true;
                    if (NumberOfInivitePerDay > 0)
                    {
                        SearchCriteria.NumberOfRequestPerKeyword = Convert.ToInt32(NumberOfInivitePerDay);
                        SearchCriteria.RequestLimit = SearchCriteria.NumberOfRequestPerKeyword;
                        AddLoggerManageConnection("[ " + DateTime.Now + " ] => [ " + SearchCriteria.NumberOfRequestPerKeyword + " Maximum No Of Invites Per Day ]");
                    }
                    else
                    {
                        SearchCriteria.NumberOfRequestPerKeyword = 10;
                        SearchCriteria.RequestLimit = SearchCriteria.NumberOfRequestPerKeyword;
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
                    int Totalcount = SearchCriteria.NumberOfRequestPerKeyword - TodayTweet;
                    SearchCriteria.NumberOfRequestPerKeyword = Totalcount;

                }
                if ((IsSchedulDaily) && (!(IsInviteperDay)))
                {
                    SearchCriteria.NumberOfRequestPerKeyword = NumberOfInivitePerDay;
                }
                Login.logger.addToLogger += new EventHandler(loggerAddConnection_addToLogger);
                ConnectUsingSearchKeywod.SchedulerEndTime = SchedulerEndTime;
                ConnectUsingSearchKeywod.campaignName = CampaignName;
                
                //count_AccountforCampaignFollower = CampaignAccountsList.dictionary_CampaignAccounts.Count();
                //get account logging 
                Queue<string> que_SearchKeywords = new Queue<string>();

                que_SearchKeywords.Enqueue(keyword);
                
                ConnectUsingSearchKeywod ConnectUsing_Search = new ConnectUsingSearchKeywod(keyValuePair.Value.Username, keyValuePair.Value.Password, keyValuePair.Value.proxyAddress, keyValuePair.Value.proxyPort, keyValuePair.Value.proxyUsername, keyValuePair.Value.proxyPassword, que_SearchKeywords);
                ManageConnections.ConnectUsing_Search.SearchUsingkeywordForInvite(ref ConnectUsing_Search, DelayStart, DelayEnd);
                
            }
            catch (Exception)
            {
            }
            finally
            {
                //count_AccountforCampaignFollower--;
                Interlocked.Decrement(ref counterThreadsCampaignFollow);
                lock (lockerThreadsCampaignFollow)
                {
                    Monitor.Pulse(lockerThreadsCampaignFollow);
                }
                if (counterThreadsCampaignFollow == 0)
                {
                    AddLoggerManageConnection("[ " + DateTime.Now + " ] => [ Process is completed for Campaign " + CampaignName + " ]");
                    //_IsFollowProcessStart = true;
                }
            }
        }

        #region EditCampaignCode
        public void editCampaign(String CampaignName, String featurName)
        {
            try
            {
                DataRow[] drModelDetails = CompaignsDataSet.Tables[0].Select("CampaignName = '" + CampaignName + "'");

                if (drModelDetails.Count() == 0)
                {

                }

                //Get 1st row from arrey 
                DataRow DrCampaignDetails = drModelDetails[0];

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


                try
                {

                    txt_CampaignName.Invoke(new MethodInvoker(delegate
                    {
                        txt_CampaignName.ReadOnly = true;
                        txt_CampaignName.Text = CampaignName;
                    }));
                }
                catch { }
                try
                {

                    txt_accountfilepath.Invoke(new MethodInvoker(delegate
                    {
                        txt_accountfilepath.Text = AcFilePath;
                    }));
                }
                catch { }


                if (!string.IsNullOrEmpty(Keyword))
                {

                    txtUploadKeyword.Invoke(new MethodInvoker(delegate
                    {
                        txtUploadKeyword.Text = Keyword;
                    }));


                }

                if (!string.IsNullOrEmpty(Message))
                {

                    txtInviteMsg.Invoke(new MethodInvoker(delegate
                    {
                        txtInviteMsg.Text = Message;
                    }));

                }

                if (NoofInviteParDay != 0)
                {
                    txt_campMaximumNoRetweet.Invoke(new MethodInvoker(delegate { txt_campMaximumNoRetweet.Text = NoofInviteParDay.ToString(); }));
                }

                if (NoOfInviteUser != 0)
                {
                    txt_campNoOfInvitesParAc.Invoke(new MethodInvoker(delegate { txt_campNoOfInvitesParAc.Text = NoOfInviteUser.ToString(); }));
                }

                if (IsInviteParDay == true)
                {
                    //#region
                    chk_InvitePerDay.Invoke(new MethodInvoker(delegate
                    {
                        chk_InvitePerDay.Checked = true;
                    }));
                }

                if (IsSpinTax == true)
                {
                    //#region
                    chkUseSpintax_InviteMessage.Invoke(new MethodInvoker(delegate
                    {
                        chkUseSpintax_InviteMessage.Checked = true;
                    }));
                }

                if (IsSchedulDaily == true)
                {
                    #region
                    chkboxSearchWithInviteScheduledDaily.Invoke(new MethodInvoker(delegate
                    {
                        chkboxSearchWithInviteScheduledDaily.Checked = true;
                    }));

                    dateTimePicker_Start_SearchWithInvite.Invoke(new MethodInvoker(delegate
                    {
                        if (SchedulerStartTime < DateTime.Now)
                            dateTimePicker_Start_SearchWithInvite.MinDate = SchedulerStartTime;
                        else
                            dateTimePicker_Start_SearchWithInvite.MinDate = DateTime.Now;

                        dateTimePicker_Start_SearchWithInvite.Value = SchedulerStartTime;
                    }));

                    dateTimePicker_End.Invoke(new MethodInvoker(delegate
                    {
                        if (SchedulerEndTime < DateTime.Now)
                            dateTimePicker_End.MinDate = SchedulerEndTime;
                        else
                            dateTimePicker_End.MinDate = DateTime.Now;

                        dateTimePicker_End.Value = (SchedulerEndTime);
                    }));

                    #endregion
                }

                if (DelayStar != 0)
                {
                    txtSearchMindelay.Invoke(new MethodInvoker(delegate { txtSearchMindelay.Text = DelayStar.ToString(); }));
                }

                if (DelayEnd != 0)
                {
                    txtSearchMaxDelay.Invoke(new MethodInvoker(delegate { txtSearchMaxDelay.Text = DelayEnd.ToString(); }));
                }
                if (Threads != 0)
                {
                    txtThreadManageConnection.Invoke(new MethodInvoker(delegate { txtThreadManageConnection.Text = Threads.ToString(); }));
                }
            }
            catch { }
        }
        #endregion

        #region UpdateQueryOnButtonUpdateClick
        public void btn_UpdateCampaign()
        {
            try
            {
                string query = "UPDATE tb_CamapignSearcWithInvite SET AcFilePath ='" + _AccountFilePath + "',KeywordFilePath='" + _KeywordFilePath + "',Message='" + _InviteMsg + "'"
                        + " , ISUseSpinTax= '" + _IsSpintaxChecked + "' , IsInviteParDay='" + _IsInviteParDay + "',NumberOfInviteParDay='" + _MaxNoOfInvitesParDay + "', NoofInvite='" + _NoofInvitesParAC + "'"
                        + ", IsSheduleDaily='" + _IsScheduledDaily + "',"
                        + " StartTime='" + _StartFrom + "', EndTime='" + _EndTo + "',DelayFrom='" + _DelayFrom + "',DelayTo='" + _DelayTo + "', NoOfThread='" + Threads + "' WHERE CampaignName='" + _CmpName + "';";



                clsDBQueryManager queryManager = new clsDBQueryManager();

                queryManager.InsertCamapaignData(query, "tb_CamapignSearcWithInvite");

                AddLoggerManageConnection("[ " + DateTime.Now + " ] => [  " + _CmpName + " is Saved. ]");

                LoadCampaign();
                ///Clear campaign 
                ClearCamapigns();
            }
            catch (Exception ex)
            {
            }
        } 
        #endregion

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

            txtUploadKeyword.Invoke(new MethodInvoker(delegate
            {
                txtUploadKeyword.Text = "";
            }));
            
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

            chkUseSpintax_InviteMessage.Invoke(new MethodInvoker(delegate
            {
                chkUseSpintax_InviteMessage.Checked = false;
            }));
            #region

            chkboxSearchWithInviteScheduledDaily.Invoke(new MethodInvoker(delegate
            {
                chkboxSearchWithInviteScheduledDaily.Checked = false;
            }));

            dateTimePicker_Start_SearchWithInvite.Invoke(new MethodInvoker(delegate
            {
                dateTimePicker_Start_SearchWithInvite.MinDate = DateTime.Now.Date;

                dateTimePicker_Start_SearchWithInvite.Value = DateTime.Now;
            }));

            dateTimePicker_End.Invoke(new MethodInvoker(delegate
            {
                dateTimePicker_End.MinDate = DateTime.Now.Date;

                dateTimePicker_End.Value = DateTime.Now;
            }));
            #endregion

            txtSearchMindelay.Invoke(new MethodInvoker(delegate { txtSearchMindelay.Text = "20"; }));

            txtSearchMaxDelay.Invoke(new MethodInvoker(delegate { txtSearchMaxDelay.Text = "25"; }));

            txtThreadManageConnection.Invoke(new MethodInvoker(delegate { txtThreadManageConnection.Text = "7"; }));
        } 
        #endregion

        #region BtnUpdateClick
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string Result = Validations("Update");
            if (string.IsNullOrEmpty(_InviteMsg))
            {
                _InviteMsg = txtInviteMsg.Text.Trim();
            }

            if (!string.IsNullOrEmpty(Result))
            {
                MessageBox.Show(Result);
            }
            else
            {
                btn_UpdateCampaign();
            }
        } 
        #endregion

        #region DeleteSelectedRow and Paint MethodofForm
        private void dgv_campaign_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            try
            {
                DataSet dsDataTable = new DataSet();
                dsDataTable = queryManager.SelectCamapaignData();
                string userId = dgv_campaign.SelectedRows[0].Cells[0].Value.ToString();
                DialogResult dialogResult = MessageBox.Show("Do you want to delete row  from Data Table and Grid.", "", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    //delete row and data from Table and gridview row ..
                    queryManager.DeleteDataLogin(userId);

                    try
                    {
                        DataRow[] drModelDetails = dsDataTable.Tables[0].Select("Id = '" + userId + "'");

                        if (drModelDetails.Count() != 0)
                            dsDataTable.Tables[0].Rows.Remove(drModelDetails[0]);

                        AddLoggerManageConnection("[ " + DateTime.Now + " ] => [ " + userId + " is Deleted. ]");

                    }
                    catch (Exception)
                    {
                    }

                }
                else
                {

                    e.Cancel = true;
                }
                //LoadCampaign();

            }
            catch { }

        }

        private void frmCampaignSearchWithInvite_Paint(object sender, PaintEventArgs e)
        {

            Graphics g;
            g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.DrawImage(image, 0, 0, this.Width, this.Height);
        }

       
        #endregion

        private void frmCampaignSearchWithInvite_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (cls_variables.Lst_WokingThreads.Count == 0)
            {
                ((Form)sender).Dispose();
                GC.Collect();
                return;
            }
            if ((MessageBox.Show("Do you want to close window. When you closed campaign window than all process aborted.!", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes))
            {
                foreach (DataGridViewRow Rowitem in dgv_campaign.Rows)
                {
                    string CampaignName = Rowitem.Cells[0].FormattedValue.ToString();
                    string FeaturName = Rowitem.Cells[1].FormattedValue.ToString();

                    // Your code would go here below is just the code I used to test 
                    Bitmap ImgBitmap = (Bitmap)(Rowitem.Cells[3].FormattedValue);
                    string Img = GetImageValue(ImgBitmap);

                    if (Img == "OFF")
                    {
                        new Thread(() =>
                        {
                            StoprunningCampaign(CampaignName);
                        }).Start();
                    }
                }
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void dgv_campaign_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

       
        
       
        

    }
}
                   