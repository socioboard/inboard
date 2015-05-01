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
using System.Drawing.Drawing2D;
using BaseLib;

namespace InBoardPro
{
    public partial class FrmAddExperience : Form
    {
        #region global declaration
        bool CheckNetConn = false;
        List<string> lstcmpname = new List<string>();
        List<string> lsttitlename = new List<string>();
        List<string> lstlocationname = new List<string>();
        List<string> lstdescriptionname = new List<string>();
        bool IsStopAddExperience = false;
        public System.Drawing.Image image;
        List<Thread> lstThreadforAddExperience = new List<Thread>();
        string StartMonth = string.Empty;
        string EndMonth = string.Empty; 
        #endregion

        #region FrmAddExperience
        public FrmAddExperience()
        {
            InitializeComponent();
        } 
        #endregion
      
        #region btncmpname_Click
        private void btncmpname_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "Text Files (*.txt)|*.txt";
                    ofd.InitialDirectory = Application.StartupPath;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtCmpName.Text = ofd.FileName;
                        lstcmpname.Clear();
                        List<string> templist = GlobusFileHelper.ReadFile(ofd.FileName);
                        foreach (string item in templist)
                        {
                            string newItem = item.Replace(" ", "").Replace("\t", "");
                            if (!lstcmpname.Contains(item) && !string.IsNullOrEmpty(newItem))
                            {
                                lstcmpname.Add(item);
                            }
                        }
                        AddExperienceLogger("[ " + DateTime.Now + " ] => [ " + lstcmpname.Count + "  Company Name Loaded ]");
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Account Creator --> btnfname_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Account Creator --> btnfname_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAccountCraetionErrorLogs);
            }
        } 
        #endregion

        #region btntitle_Click
        private void btntitle_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "Text Files (*.txt)|*.txt";
                    ofd.InitialDirectory = Application.StartupPath;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtTitleName.Text = ofd.FileName;
                        lsttitlename.Clear();
                        List<string> templist = GlobusFileHelper.ReadFile(ofd.FileName);
                        foreach (string item in templist)
                        {
                            string newItem = item.Replace("\t", "");
                            if (!lsttitlename.Contains(item) && !string.IsNullOrEmpty(newItem))
                            {
                                lsttitlename.Add(item);
                            }
                        }
                        AddExperienceLogger("[ " + DateTime.Now + " ] => [ " + lsttitlename.Count + "  Title Loaded ]");
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Account Creator --> btnfname_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Account Creator --> btnfname_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAccountCraetionErrorLogs);
            }

        } 
        #endregion

        #region btnlocation_Click
        private void btnlocation_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "Text Files (*.txt)|*.txt";
                    ofd.InitialDirectory = Application.StartupPath;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtLocation.Text = ofd.FileName;
                        lstlocationname.Clear();
                        List<string> templist = GlobusFileHelper.ReadFile(ofd.FileName);
                        foreach (string item in templist)
                        {
                            string newItem = item.Replace("\t", "");
                            if (!lstlocationname.Contains(item) && !string.IsNullOrEmpty(newItem))
                            {
                                lstlocationname.Add(item);
                            }
                        }
                        AddExperienceLogger("[ " + DateTime.Now + " ] => [ " + lstlocationname.Count + "  Location Loaded ]");
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Account Creator --> btnfname_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Account Creator --> btnfname_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAccountCraetionErrorLogs);
            }

        } 
        #endregion

        #region btndiscription_Click
        private void btndiscription_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "Text Files (*.txt)|*.txt";
                    ofd.InitialDirectory = Application.StartupPath;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtDiscription.Text = ofd.FileName;
                        lstdescriptionname.Clear();
                        List<string> templist = GlobusFileHelper.ReadFile(ofd.FileName);
                        foreach (string item in templist)
                        {
                            string newItem = item.Replace("\t", "");
                            if (!lstdescriptionname.Contains(item) && !string.IsNullOrEmpty(newItem))
                            {
                                lstdescriptionname.Add(item);
                            }
                        }
                        AddExperienceLogger("[ " + DateTime.Now + " ] => [ " + lstdescriptionname.Count + " Description Loaded ]");
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Account Creator --> btnfname_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Account Creator --> btnfname_Click() >>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinAccountCraetionErrorLogs);
            }

        } 
        #endregion

        #region btnStartAddExpedience_Click
        private void btnStartAddExperience_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {

                AddExperienceLogger("[ " + DateTime.Now + " ] => [ Starting Add Experience ]");
                bool Valid = false;
                try
                {
                    Valid = CheckValidation1();
                }
                catch { }

                if (!Valid)
                {
                    IsStopAddExperience = false;

                    if (lstcmpname.Count > 0 && lsttitlename.Count > 0)
                    {
                        try
                        {
                            btnStartAddExperience.Cursor = Cursors.AppStarting;

                            Thread thread_LinkedinSearch = new Thread(LinkedInAddExperience);
                            if (!IsStopAddExperience)
                            {
                                lstThreadforAddExperience.Add(thread_LinkedinSearch);
                                thread_LinkedinSearch.IsBackground = true;
                                lstThreadforAddExperience.Distinct();
                                thread_LinkedinSearch.Start();
                            }

                        }
                        catch { }
                    }
                    else
                    {
                        MessageBox.Show("Please Upload Mandatory(*) Data!");
                        return;
                    }
                }
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddExperienceLogger("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
           
        } 
        #endregion

        #region btnStop_Click
        private void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                IsStopAddExperience = true;

                for (int i = 0; i < 2; i++)
                {
                    foreach (Thread item in lstThreadforAddExperience)
                    {
                        try
                        {
                            item.Abort();
                        }
                        catch
                        {
                        }
                    }
                    Thread.Sleep(1000);
                }
                AddExperienceLogger("------------------------------------------------------------------------------------------------------------------------------------");
                AddExperienceLogger("[ " + DateTime.Now + " ] => [ PROCESS STOPPED ]");
                AddExperienceLogger("------------------------------------------------------------------------------------------------------------------------------------");
                btnStartAddExperience.Cursor = Cursors.Default;
                MessageBox.Show("Process Has Been Stopped !");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error >>> " + ex.StackTrace);
            }
        }
        #endregion

        #region CheckValidation()
        /*
        private bool CheckValidation()
        {
           
            try
            {
                if (chkTimePeriod.Checked)
                {
                    ProfileManager.ProfileManager.SameExperienceTime = true;
                    if (!string.IsNullOrEmpty(StartMonth) && !string.IsNullOrEmpty(txtstartyear.Text.ToString().Trim()) && NumberHelper.ValidateNumber(txtstartyear.Text.ToString().Trim()))
                    {
                        if (!string.IsNullOrEmpty(EndMonth) && !string.IsNullOrEmpty(txtendyear.Text.ToString().Trim()) && NumberHelper.ValidateNumber(txtendyear.Text.ToString().Trim()))
                        {
                            if (NumberHelper.ValidateNumber(txtstartyear.Text.ToString().Trim()) && NumberHelper.ValidateNumber(txtendyear.Text.ToString().Trim()))
                            {
                                try
                                {

                                    string startyeardata = StartMonth + txtstartyear.Text.ToString();
                                    string endyeardata = EndMonth + txtendyear.Text.ToString();
                                    if (Convert.ToInt32(startyeardata) != Convert.ToInt32(endyeardata))
                                    {
                                        try
                                        {
                                            //ProfileManager.ProfileManager.StartMonthFromcmb = StartMonth;
                                            //ProfileManager.ProfileManager.EndMonthFromcmb = EndMonth;
                                            //ProfileManager.ProfileManager.StartYearFromcmb = startyeardata;
                                            //ProfileManager.ProfileManager.EndYearFromcmb = endyeardata;

                                            ProfileManager.ProfileManager.StartYearFromcmb = string.Empty;
                                            ProfileManager.ProfileManager.EndYearFromcmb = string.Empty;
                                             startyeardata = txtstartyear.Text.ToString();
                                             endyeardata = txtendyear.Text.ToString();
                                            ProfileManager.ProfileManager.StartMonthFromcmb = StartMonth;
                                            ProfileManager.ProfileManager.StartYearFromcmb = startyeardata;

                                            ProfileManager.ProfileManager.EndMonthFromcmb = EndMonth;
                                            ProfileManager.ProfileManager.EndYearFromcmb = endyeardata;
                                        }
                                        catch { }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Your Start Year Value is Greater Than End Year. So Please Upload Correct Time Period ! ");
                                        return true;
                                    }
                                }
                                catch { }
                            }
                            else
                            {
                                MessageBox.Show("Please Enter Year in Numeric Form");
                                return true; 
                            }
                        }
                        else
                        {
                            if (chkcurrentlywork.Checked)
                            {
                                try
                                {
                                    //if (!string.IsNullOrEmpty(StartMonth) && !string.IsNullOrEmpty(txtstartyear.Text.ToString().Trim()))
                                    //{
                                        ProfileManager.ProfileManager.StartYearFromcmb = string.Empty;
                                        ProfileManager.ProfileManager.EndYearFromcmb = string.Empty;
                                        string startyeardata = txtstartyear.Text.ToString();
                                        string endyeardata = txtendyear.Text.ToString();
                                        ProfileManager.ProfileManager.StartMonthFromcmb = StartMonth;
                                        ProfileManager.ProfileManager.StartYearFromcmb = startyeardata;
                                    //}
                                    //else
                                    //{
                                    //    MessageBox.Show("Please Upload  Time Period ! ");
                                    //    return true;
                                    //}
                                }
                                catch { }
                               

                            }
                            else
                            {
                                if (chkcurrentlywork.Checked == false)
                                {
                                    try
                                    {
                                        MessageBox.Show("Please Checked 'I currently Working Checkbox' or Enter All Time Period Value !");
                                        return true;
                                    }
                                    catch { }
                                }
                                else
                                {
                                    try
                                    {
                                        MessageBox.Show("Please UnChecked 'I currently Working Checkbox' or Enter All Time Period Value !");
                                        return true;
                                    }
                                    catch { }

                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please Upload Time Period Data Or Uncheck 'Same Time Period For All Experience Checkbox ' !");
                        return true;
                    }
                }
                else
                {
                    if (chkcurrentlywork.Checked)
                    {
                        try
                        {
                            if (!string.IsNullOrEmpty(StartMonth) && !string.IsNullOrEmpty(txtstartyear.Text.ToString().Trim()) && NumberHelper.ValidateNumber(txtstartyear.Text.ToString().Trim()))
                            {
                                string startyeardata = txtstartyear.Text.ToString();
                                string endyeardata = txtendyear.Text.ToString();

                                ProfileManager.ProfileManager.StartYearFromcmb = string.Empty;
                                ProfileManager.ProfileManager.EndYearFromcmb = string.Empty;
                                ProfileManager.ProfileManager.StartMonthFromcmb = StartMonth;
                                ProfileManager.ProfileManager.StartYearFromcmb = startyeardata;
                            }
                            else
                            {
                                MessageBox.Show("Please Upload  Correct Time Period ! ");
                                return true;
                            }
                        }
                        catch { }

                    }
                   
                }
            }
            catch { }
            return false;
        } */
        #endregion

        #region CheckValidation1
        private bool CheckValidation1()
        {
            try
            {
                if (rdoTimePeriod.Checked)
                {
                    ProfileManager.ProfileManager.SameExperienceTime = true;
                    if (!string.IsNullOrEmpty(StartMonth) && !string.IsNullOrEmpty(txtstartyear.Text.ToString().Trim()) && NumberHelper.ValidateNumber(txtstartyear.Text.ToString().Trim()))
                    {
                        if (!string.IsNullOrEmpty(EndMonth) && !string.IsNullOrEmpty(txtendyear.Text.ToString().Trim()) && NumberHelper.ValidateNumber(txtendyear.Text.ToString().Trim()))
                        {
                            if (NumberHelper.ValidateNumber(txtstartyear.Text.Trim()) && NumberHelper.ValidateNumber(txtendyear.Text.Trim()))
                            {
                                if (Convert.ToInt16(txtstartyear.Text.Trim()) <= Convert.ToInt16(txtendyear.Text.Trim()))
                                {
                                    try
                                    {

                                        string startyeardata = StartMonth + txtstartyear.Text.ToString();
                                        string endyeardata = EndMonth + txtendyear.Text.ToString();
                                        if (Convert.ToInt32(startyeardata) != Convert.ToInt32(endyeardata))
                                        {
                                            try
                                            {
                                                //ProfileManager.ProfileManager.StartMonthFromcmb = StartMonth;
                                                //ProfileManager.ProfileManager.EndMonthFromcmb = EndMonth;
                                                //ProfileManager.ProfileManager.StartYearFromcmb = startyeardata;
                                                //ProfileManager.ProfileManager.EndYearFromcmb = endyeardata;

                                                ProfileManager.ProfileManager.StartYearFromcmb = string.Empty;
                                                ProfileManager.ProfileManager.EndYearFromcmb = string.Empty;
                                                startyeardata = txtstartyear.Text.ToString();
                                                endyeardata = txtendyear.Text.ToString();
                                                ProfileManager.ProfileManager.StartMonthFromcmb = StartMonth;
                                                ProfileManager.ProfileManager.StartYearFromcmb = startyeardata;

                                                ProfileManager.ProfileManager.EndMonthFromcmb = EndMonth;
                                                ProfileManager.ProfileManager.EndYearFromcmb = endyeardata;
                                            }
                                            catch { }
                                        }
                                        else
                                        {
                                            MessageBox.Show("Your Start Year Value is Greater Than End Year. So Please Upload Correct Time Period ! ");
                                            return true;
                                        }
                                    }

                                    catch { }
                                }
                                else
                                {
                                    MessageBox.Show("Your Start Year Value is Greater Than End Year. So Please Upload Correct Time Period ! ");
                                    return true;
                                }
                            }
                            else
                            {
                                MessageBox.Show("Please Enter Year in Numeric Form");
                                return true;
                            }
                        }
                        else
                        {
                            if (rdocurrentlywork.Checked)
                            {
                                try
                                {
                                    //if (!string.IsNullOrEmpty(StartMonth) && !string.IsNullOrEmpty(txtstartyear.Text.ToString().Trim()))
                                    //{
                                    ProfileManager.ProfileManager.StartYearFromcmb = string.Empty;
                                    ProfileManager.ProfileManager.EndYearFromcmb = string.Empty;
                                    string startyeardata = txtstartyear.Text.ToString();
                                    string endyeardata = txtendyear.Text.ToString();
                                    ProfileManager.ProfileManager.StartMonthFromcmb = StartMonth;
                                    ProfileManager.ProfileManager.StartYearFromcmb = startyeardata;
                                    //}
                                    //else
                                    //{
                                    //    MessageBox.Show("Please Upload  Time Period ! ");
                                    //    return true;
                                    //}
                                }
                                catch { }


                            }
                            else
                            {
                                if (rdocurrentlywork.Checked == false)
                                {
                                    try
                                    {
                                        MessageBox.Show("Please Checked 'I currently Working Checkbox' or Enter All Time Period Value !");
                                        return true;
                                    }
                                    catch { }
                                }
                                else
                                {
                                    try
                                    {
                                        MessageBox.Show("Please UnChecked 'I currently Working Checkbox' or Enter All Time Period Value !");
                                        return true;
                                    }
                                    catch { }

                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please Upload Time Period Data Or Uncheck 'Same Time Period For All Experience radiobutton ' !");
                        return true;
                    }
                }
                else
                {
                    if (rdocurrentlywork.Checked)
                    {
                        try
                        {
                            if (!string.IsNullOrEmpty(StartMonth) && !string.IsNullOrEmpty(txtstartyear.Text.ToString().Trim()) && NumberHelper.ValidateNumber(txtstartyear.Text.ToString().Trim()))
                            {
                                string startyeardata = txtstartyear.Text.ToString();
                                string endyeardata = txtendyear.Text.ToString();

                                ProfileManager.ProfileManager.StartYearFromcmb = string.Empty;
                                //ProfileManager.ProfileManager.EndYearFromcmb = string.Empty;
                                ProfileManager.ProfileManager.StartMonthFromcmb = StartMonth;
                                //ProfileManager.ProfileManager.StartYearFromcmb = startyeardata;
                            }
                            else
                            {
                                MessageBox.Show("Please Upload  Correct Time Period ! ");
                                return true;
                            }
                        }
                        catch { }

                    }

                }
            }
            catch { }
            return false;
        }  
        #endregion
   
        #region LinkedInAddExpperience
        static int counter_ProcessComplete = 0;
        private void LinkedInAddExperience()
        {
            ProfileManager.ProfileManager.lstDescriptionNames = lstdescriptionname;
            ProfileManager.ProfileManager.lstLocationNames = lstlocationname;
            ProfileManager.ProfileManager.lstCmpNames = lstcmpname;
            ProfileManager.ProfileManager.lstTitelNames = lsttitlename;

            counter_ProcessComplete = LinkedInManager.linkedInDictionary.Count();

            if (LinkedInManager.linkedInDictionary.Count > 0)
            {
                foreach (KeyValuePair<string, LinkedInMaster> item in LinkedInManager.linkedInDictionary)
                {
                    try
                    {
                        try
                        {
                            AddExperienceLogger("[ " + DateTime.Now + " ] => [ Adding Experience From : " + item.Key + " ]");
                            int numberofThreds = 20;
                            ThreadPool.SetMaxThreads(numberofThreds, 5);
                            ThreadPool.QueueUserWorkItem(new WaitCallback(LoginForAddExperience), new object[] { item });
                            Thread.Sleep(1000);

                        }
                        catch (Exception ex)
                        {
                            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Status Update --> LinkdinStatusUpdate() ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                            GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> Status Update --> LinkdinStatusUpdate() ---1--->>>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinStatusUpdateErrorLogs);
                        }
                    }
                    catch { }
                }
            }
            else
            {
                MessageBox.Show("Please Upload Account !");
                return;
            }

        } 
        #endregion

        #region LoginForAddExperience
        private void LoginForAddExperience(object Parameter)
        {
            try
            {
                if (IsStopAddExperience)
                {
                    return;
                }

                if (!IsStopAddExperience)
                {
                    lstThreadforAddExperience.Add(Thread.CurrentThread);
                    lstThreadforAddExperience.Distinct();
                    Thread.CurrentThread.IsBackground = true;
                }

                string account = string.Empty;
                string post = string.Empty;
                Array paramsArray = new object[1];

                paramsArray = (Array)Parameter;

                KeyValuePair<string, LinkedInMaster> item = (KeyValuePair<string, LinkedInMaster>)paramsArray.GetValue(0);
                account = item.Key;
                GlobusHttpHelper HttpHelper = new GlobusHttpHelper();
                LinkedinLogin Login = new LinkedinLogin();

                Login.accountUser = item.Key;
                Login.accountPass = item.Value._Password;
                Login.proxyAddress = item.Value._ProxyAddress;
                Login.proxyPort = item.Value._ProxyPort;
                Login.proxyUserName = item.Value._ProxyUsername;
                Login.proxyPassword = item.Value._ProxyPassword;

                ProfileManager.ProfileManager objAddExperience = new ProfileManager.ProfileManager(Login.accountUser, Login.accountPass, Login.proxyAddress, Login.proxyPort, Login.proxyUserName, Login.proxyPassword);

                objAddExperience.LinkedInAddExperienceLogEvents.addToLogger += ScrapperRecordsLogEvents_addToLogger;
                Login.logger.addToLogger += new EventHandler(ScrapperRecordsLogEvents_addToLogger);
                
                if (!Login.IsLoggedIn)
                {
                    Login.LoginHttpHelper(ref HttpHelper);
                }
                
                try
                {
                    if (Login.IsLoggedIn)
                    {
                        AddExperienceLogger("[ " + DateTime.Now + " ] => [ Logged In With " + item.Key + " , Adding Experience ]");
                        objAddExperience.AddExperience(ref HttpHelper);
                    }
                }

                catch { }
                
                objAddExperience.LinkedInAddExperienceLogEvents.addToLogger -= ScrapperRecordsLogEvents_addToLogger;
                Login.logger.addToLogger -= new EventHandler(ScrapperRecordsLogEvents_addToLogger);
            }
            catch { }
            finally
            {
                 counter_ProcessComplete--;

                if (counter_ProcessComplete == 0)
                {
                    if (btnStartAddExperience.InvokeRequired)
                    {
                        btnStartAddExperience.Invoke(new MethodInvoker(delegate
                        {
                            AddExperienceLogger("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                            AddExperienceLogger("----------------------------------------------------------------------------------------------------------------------------");
                            btnStartAddExperience.Cursor = Cursors.Default;
                        }));
                    }
                }
            }
        } 
        #endregion

        #region ScrapperRecordsLogEvents_addToLogger
        void ScrapperRecordsLogEvents_addToLogger(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eventArgs = e as EventsArgs;
                AddExperienceLogger(eventArgs.log);

            }
        } 
        #endregion

        #region AddExperienceLogger
        private void AddExperienceLogger(string log)
        {
            try
            {
                if (lstAddExperience.InvokeRequired)
                {
                    lstAddExperience.Invoke(new MethodInvoker(delegate
                    {
                        lstAddExperience.Items.Add(log);

                        lstAddExperience.SelectedIndex = lstAddExperience.Items.Count - 1;
                    }));
                }
                else
                {
                    lstAddExperience.Items.Add(log);

                    lstAddExperience.SelectedIndex = lstAddExperience.Items.Count - 1;
                }
            }
            catch { }

        } 
        #endregion

        #region FrmAddExperience_Load
        private void FrmAddExperience_Load(object sender, EventArgs e)
        {
            image = Properties.Resources.background;
        } 
        #endregion

        #region cmbStartMonth_SelectedIndexChanged
        private void cmbStartMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string item = cmbStartMonth.SelectedItem.ToString();
                StartMonth = ValueofMonth(item);
            }
            catch { }
        } 
        #endregion

        #region ValueofMonth
        private string ValueofMonth(string Month)
        {
            string val = Month;
            switch (val)
            {
                case "January":

                    return "1";
                case "February":

                    return "2";
                case "March":

                    return "3";
                case "April":

                    return "4";
                case "May":

                    return "5";
                case "June":

                    return "6";
                case "July":

                    return "7";
                case "August":

                    return "8";
                case "September":

                    return "9";
                case "October":

                    return "10";
                case "November":

                    return "11";
                case "December":

                    return "12";
                default:

                    return "1";

            }

        } 
        #endregion

        #region cmbEndMonth_SelectedIndexChanged
        private void cmbEndMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string item = cmbEndMonth.SelectedItem.ToString();
                EndMonth = ValueofMonth(item);
            }
            catch { }
        } 
        #endregion


        #region txtstartyear_MouseClick
        private void txtstartyear_MouseClick(object sender, MouseEventArgs e)
        {
            txtstartyear.Clear();
        } 
        #endregion

        #region txtendyear_MouseClick
        private void txtendyear_MouseClick(object sender, MouseEventArgs e)
        {
            txtendyear.Clear();
        } 
        #endregion

        #region FrmAddExperience_Paint
        private void FrmAddExperience_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                Graphics g;

                g = e.Graphics;

                g.SmoothingMode = SmoothingMode.HighQuality;

                g.DrawImage(image, 0, 0, this.Width, this.Height);
            }
            catch (Exception ex)
            {

            }
        } 
        #endregion

        private void txtstartyear_MouseMove(object sender, MouseEventArgs e)
        {
            if (txtstartyear.Text == "Year")
            {
                txtstartyear.Text = string.Empty;
                txtstartyear.Focus();
            }
            else 
            {
                txtstartyear.Focus();
            }
        }

        private void gbAddExpTimePeriodSetting_MouseHover(object sender, EventArgs e)
        {
            if (txtstartyear.Text == string.Empty)
            {
                txtstartyear.Text = "Year";
                gbAddExpTimePeriodSetting.Focus();
            }
            else
            {
                gbAddExpTimePeriodSetting.Focus();
            }

            if (txtendyear.Text == string.Empty)
            {
                txtendyear.Text = "Year";
                gbAddExpTimePeriodSetting.Focus();
            }
            else
            {
                gbAddExpTimePeriodSetting.Focus();
            }
        }

        private void txtendyear_MouseMove(object sender, MouseEventArgs e)
        {
            if (txtendyear.Text == "Year")
            {
                txtendyear.Text = string.Empty;
                txtendyear.Focus();
            }
            else
            {
                txtendyear.Focus();
            }
        }

        private void rdoTimePeriod_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoTimePeriod.Checked)
            {
                cmbStartMonth.Visible = true;
                cmbEndMonth.Visible = true;
                txtstartyear.Visible = true;
                txtendyear.Visible = true;
                label8.Visible = true;
            }

        }

        private void rdocurrentlywork_CheckedChanged(object sender, EventArgs e)
        {
            if (rdocurrentlywork.Checked)
            {
                try
                {
                    cmbEndMonth.Visible = false;
                    txtendyear.Visible = false;
                    EndMonth = string.Empty;
                    txtendyear.Text = string.Empty;
                    label8.Visible = false;

                }
                catch { }
            }

        }

      }
}
