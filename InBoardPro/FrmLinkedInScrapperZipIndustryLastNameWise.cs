using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using InBoardPro;
using System.Threading;
using System.Text.RegularExpressions;
using BaseLib;
namespace InBoardPro
{
    public partial class FrmInBoardProGetDataZipIndustryLastNameWise : Form
    {
        public FrmInBoardProGetDataZipIndustryLastNameWise()
        {
            InitializeComponent();
        }

        #region global declaration
        List<string[]> excelData = new List<string[]>();
        public System.Drawing.Image image;
        List<string[]> listArrSftwareinputData = new List<string[]>();
        List<string> listSftwareinputData = new List<string>();
        List<Thread> lstScrapperStopThread = new List<Thread>();
        bool IsStopScrapper = false;
        bool CheckNetConn = false;
        InBoardPro.StartLinkedinScrapping objstartscrapp = new InBoardPro.StartLinkedinScrapping(); 
        #endregion

        #region FrmInBoardProGetDataUsingUrl_Paint
        private void FrmInBoardProGetDataUsingUrl_Paint(object sender, PaintEventArgs e)
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

        #region FrmInBoardProGetDataUsingUrl_Load
        private void FrmInBoardProGetDataUsingUrl_Load(object sender, EventArgs e)
        {
            image = Properties.Resources.background;
            rdbautopauseandresume.Checked = true;
        } 
        #endregion

        #region ScrapperLogEvents_addToLogger
        void ScrapperLogEvents_addToLogger(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eventArgs = e as EventsArgs;
                AddLoggerScrapData(eventArgs.log);

            }
        } 
        #endregion

        #region ScrapperRecordsLogEvents_addToLogger
        void ScrapperRecordsLogEvents_addToLogger(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eventArgs = e as EventsArgs;
                AddLoggerScrapData(eventArgs.log);

            }
        } 
        #endregion

        #region AddLoggerScrapData
        private void AddLoggerScrapData(string log)
        {
            try
            {
                if (lstScarpperLogger.InvokeRequired)
                {
                    lstScarpperLogger.Invoke(new MethodInvoker(delegate
                       {
                           lstScarpperLogger.Items.Add(log);

                           lstScarpperLogger.SelectedIndex = lstScarpperLogger.Items.Count - 1;
                       }));
                }
                else
                {
                    lstScarpperLogger.Items.Add(log);

                    lstScarpperLogger.SelectedIndex = lstScarpperLogger.Items.Count - 1;
                }
            }
            catch { }

        } 
        #endregion

        #region btnscrapperwithinput_Click
        private void btnscrapperwithinput_Click(object sender, EventArgs e)
        {
            try
            {
                LinkedInManager.linkedInDictionaryExcelInput.Clear();
                clsEmailParser objparser = new clsEmailParser();
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    excelData.Clear();
                    ofd.InitialDirectory = System.Windows.Forms.Application.StartupPath;
                    ofd.Filter = "Text Files (*.xlsx)|*.xlsx";
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtscrapperwithinput.Text = ofd.FileName;
                        excelData = objparser.parseExcel(txtscrapperwithinput.Text);//.ParseXLSXFile(textBox1.Text);
                        //  ParserClasses.Lstcorporatelink = excelData;
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        } 
        #endregion

        #region btnLinkedinSearch_Click
        private void btnLinkedinSearch_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {
                try
                {
                    Thread thread_LinkedinSearch = new Thread(InBoardProGetDataThread);
                    thread_LinkedinSearch.Start();
                }
                catch { }
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddLoggerScrapData("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        } 
        #endregion

        #region InBoardProGetDataThread
        private void InBoardProGetDataThread()
        {

            foreach (string[] itemArr in excelData)
            {
                try
                {
                    LinkedInMaster objLinkedInMaster = new LinkedInMaster();
                    try
                    {
                        if (!string.IsNullOrEmpty(itemArr[0]))
                        {
                            objLinkedInMaster._Postalcode = itemArr[0];
                        }
                    }
                    catch { }
                    try
                    {
                        if (!string.IsNullOrEmpty(itemArr[1]))
                        {
                            objLinkedInMaster._Distance = itemArr[1];
                        }
                    }
                    catch { }
                    try
                    {
                        if (!string.IsNullOrEmpty(itemArr[2]))
                        {
                            objLinkedInMaster._IndustryType = itemArr[2];
                        }

                    }
                    catch { }
                    try
                    {
                        if (!string.IsNullOrEmpty(itemArr[3]))
                        {
                            objLinkedInMaster._LastName = itemArr[3];
                        }
                    }
                    catch { }
                    try
                    {
                        if (!string.IsNullOrEmpty(itemArr[4]))
                        {
                            objLinkedInMaster._Username = itemArr[4];
                        }
                    }
                    catch { }
                    try
                    {
                        if (!string.IsNullOrEmpty(itemArr[5]))
                        {
                            objLinkedInMaster._Password = itemArr[5];
                        }
                    }
                    catch { }
                    try
                    {
                        objLinkedInMaster._ProxyAddress = itemArr[6];
                        if (!string.IsNullOrEmpty(itemArr[6]))
                        {
                            string[] words = itemArr[6].Split(new char[] { char.Parse(":") });
                            if (words.Count() > 1)
                            {
                                try
                                {
                                    string Portno = words[1];
                                    objLinkedInMaster._ProxyPort = Portno;
                                    objLinkedInMaster._ProxyAddress = words[0];// +":" + words[1] + ":" + words[2] + ":" + words[3];
                                }
                                catch { }
                            }
                            else
                            {
                                try
                                {
                                    objLinkedInMaster._ProxyAddress = words[0];//+ ":" + words[1] + ":" + words[2] + ":" + words[3];
                                }
                                catch { }

                            }
                        }
                        // string[] itemss= 
                    }
                    catch { }
                    try
                    {
                        if (!string.IsNullOrEmpty(itemArr[7]))
                        {
                            objLinkedInMaster._ProxyPassword = itemArr[7];
                        }
                    }
                    catch { }
                    try
                    {
                        if (!string.IsNullOrEmpty(itemArr[0]) && !string.IsNullOrEmpty(itemArr[1]))
                        {
                            LinkedInManager.linkedInDictionaryExcelInput.Add(objLinkedInMaster._Username + ":" + objLinkedInMaster._LastName+":"+objLinkedInMaster._Postalcode, objLinkedInMaster);
                        }
                        else
                        {
                            break;
                        }
                    }
                    catch { }
                }
                catch { }
            }
            Thread objloginthread = new Thread(StartLoginUsingThreadPool);
            objloginthread.Start();
        } 
        #endregion

        #region StartLoginUsingThreadPool
        private void StartLoginUsingThreadPool()
        {
            foreach (KeyValuePair<string, LinkedInMaster> item in LinkedInManager.linkedInDictionaryExcelInput)
            {
                try
                {
                    try
                    {
                        if (NumberHelper.ValidateNumber(txtThreadFirstModescrapper.Text))
                        {
                            int numberofThreds = 20;
                            ThreadPool.SetMaxThreads(numberofThreds, 5);
                            ThreadPool.QueueUserWorkItem(new WaitCallback(LoginUsingInputData), new object[] { item });
                            Thread.Sleep(1000);
                        }
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
        #endregion

        #region LoginUsingInputData
        private void LoginUsingInputData(object Parameter)
        {
            try
            {

                if (!IsStopScrapper)
                {
                    lstScrapperStopThread.Add(Thread.CurrentThread);
                    lstScrapperStopThread.Distinct();
                    Thread.CurrentThread.IsBackground = true;
                }

                string account = string.Empty;
                string post = string.Empty;
                Array paramsArray = new object[1];

                paramsArray = (Array)Parameter;

                KeyValuePair<string, LinkedInMaster> item = (KeyValuePair<string, LinkedInMaster>)paramsArray.GetValue(0);
                account = item.Key;
                try
                {
                    account = account.Split(':')[0];
                }
                catch { }

                GlobusHttpHelper HttpHelper = new GlobusHttpHelper();
                LinkedinLogin Login = new LinkedinLogin();

                Login.accountUser = account;
                Login.accountPass = item.Value._Password;
                Login.proxyAddress = item.Value._ProxyAddress;
                Login.proxyPort = item.Value._ProxyPort;
                Login.proxyUserName = item.Value._ProxyUsername;
                Login.proxyPassword = item.Value._ProxyPassword;
                Login.Postalcode = item.Value._Postalcode;
                Login.Distance = item.Value._Distance;
                Login.IndustryType = item.Value._IndustryType;
                Login.LastName = item.Value._LastName;
                // StatusUpdate.StatusUpdate obj_StatusUpdate = new StatusUpdate.StatusUpdate(Login.accountUser, Login.accountPass, Login.proxyAddress, Login.proxyPort, Login.proxyUserName, Login.proxyPassword);
                InBoardPro.LinkinScrappRecord obj_Scrapper = new InBoardPro.LinkinScrappRecord(Login.accountUser, Login.accountPass, Login.proxyAddress, Login.proxyPort, Login.proxyUserName, Login.proxyPassword, Login.Postalcode, Login.Distance, Login.IndustryType, Login.LastName);
                //Login.logger.addToLogger += new EventHandler(logger_StatusUpdateaddToLogger);
                //obj_StatusUpdate.logger.addToLogger += new EventHandler(logger_StatusUpdateaddToLogger);
                obj_Scrapper.InBoardProGetDataResultLogEvents.addToLogger += ScrapperRecordsLogEvents_addToLogger;
                Login.logger.addToLogger += new EventHandler(ScrapperRecordsLogEvents_addToLogger);

                if (!Login.IsLoggedIn)
                {
                    // AddLoggerScrapData("Logging With : " + Login.accountUser);
                    Login.LoginHttpHelper(ref HttpHelper);
                }
                try
                {
                    if (Login.IsLoggedIn)
                    {
                        // AddLoggerScrapData("LoggedIn With : " + Login.accountUser);
                        obj_Scrapper.lastName = Login.LastName;
                        obj_Scrapper.industryType = Login.IndustryType;
                        obj_Scrapper.postalCode = Login.Postalcode;
                        obj_Scrapper.GetRecords1(ref HttpHelper);
                    }
                }

                catch { }

                obj_Scrapper.InBoardProGetDataResultLogEvents.addToLogger -= ScrapperRecordsLogEvents_addToLogger;
                Login.logger.addToLogger -= new EventHandler(ScrapperRecordsLogEvents_addToLogger);
            }
            catch { }
            finally
            {
                AddLoggerScrapData("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                AddLoggerScrapData("----------------------------------------------------------------------------------------------------------------------------");
            }
        } 
        #endregion

        #region btnBrowseCsv_Click
        private void btnBrowseCsv_Click(object sender, EventArgs e)
        {
            LinkedInManager.linkedInDictionaryExcelInput.Clear();

            try
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.InitialDirectory = Application.StartupPath;
                    ofd.Filter = "CSV Files (*.csv)|*.csv";
                    listSftwareinputData.Clear();
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtsoftwaregeneratedinputfile.Text = ofd.FileName;
                        listArrSftwareinputData = CSVUtilities.parseCSV(ofd.FileName);
                    }
                }
            }
            catch { }

        } 
        #endregion

        #region LinkedInStartScrapperThread
        private void LinkedInStartScrapperThread()
        {

            foreach (string[] itemArr in listArrSftwareinputData)
            {
                try
                {

                    LinkedInMaster objLinkedInMaster = new LinkedInMaster();
                    if (!itemArr[0].Contains("PostalCode"))
                    {
                        try
                        {
                            if (!string.IsNullOrEmpty(itemArr[0]))
                            {
                                objLinkedInMaster._Postalcode = itemArr[0];
                            }

                        }
                        catch { }
                        try
                        {
                            if (!string.IsNullOrEmpty(itemArr[1]))
                            {
                                objLinkedInMaster._Distance = itemArr[1];
                            }
                        }
                        catch { }
                        try
                        {
                            if (!string.IsNullOrEmpty(itemArr[2]))
                            {
                                objLinkedInMaster._IndustryType = itemArr[2];
                            }
                        }
                        catch { }
                        try
                        {
                            if (!string.IsNullOrEmpty(itemArr[3]))
                            {
                                objLinkedInMaster._LastName = itemArr[3];
                            }
                        }
                        catch { }
                        try
                        {
                            if (!string.IsNullOrEmpty(itemArr[4]))
                            {
                                objLinkedInMaster._Username = itemArr[4];
                            }
                        }
                        catch { }
                        try
                        {
                            if (!string.IsNullOrEmpty(itemArr[5]))
                            {
                                objLinkedInMaster._Password = itemArr[5];
                            }
                        }
                        catch { }
                        try
                        {
                            // objLinkedInMaster._ProxyAddress = itemArr[6];
                            if (!string.IsNullOrEmpty(itemArr[6]))
                            {
                                string[] words = itemArr[6].Split(new char[] { char.Parse(":") });
                                if (words.Count() > 1)
                                {
                                    try
                                    {
                                        string Portno = words[1];
                                        objLinkedInMaster._ProxyPort = Portno;
                                        objLinkedInMaster._ProxyAddress = words[0]; //+ ":" + words[1] + ":" + words[2] + ":" + words[3];
                                    }
                                    catch { }
                                }
                                else
                                {
                                    try
                                    {
                                        objLinkedInMaster._ProxyAddress = words[0];// +":" + words[1] + ":" + words[2] + ":" + words[3];
                                    }
                                    catch { }

                                }
                            }
                            //string[] Arrport=Regex.Split(itemArr[6], ":");
                        }
                        catch { }
                        try
                        {
                            if (!string.IsNullOrEmpty(itemArr[7]))
                            {
                                objLinkedInMaster._ProxyPassword = itemArr[7];
                            }
                        }
                        catch { }
                        try
                        {
                            LinkedInManager.linkedInDictionaryExcelInput.Add(objLinkedInMaster._Username + ":" + objLinkedInMaster._LastName + ":" + objLinkedInMaster._Postalcode, objLinkedInMaster);
                        }
                        catch { }
                    }
                }
                catch { }

            }
            Thread objloginthread = new Thread(StartScarpperLoginUsingThreadPool);
            objloginthread.Start();
        } 
        #endregion

        #region StartScarpperLoginUsingThreadPool
        private void StartScarpperLoginUsingThreadPool()
        {
            foreach (KeyValuePair<string, LinkedInMaster> item in LinkedInManager.linkedInDictionaryExcelInput)
            {
                try
                {
                    try
                    {
                        if (NumberHelper.ValidateNumber(txtThreadSecondModescrapper.Text.Trim()))
                        {
                            int numberofThreds = int.Parse(txtThreadSecondModescrapper.Text.Trim());

                            ThreadPool.SetMaxThreads(numberofThreds, 5);
                            ThreadPool.QueueUserWorkItem(new WaitCallback(StartScarppLoginUsingInputData), new object[] { item });
                            Thread.Sleep(1000);
                            // StartScarppLoginUsingInputData(new object[] { item });
                        }

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
        #endregion

        #region StartScarppLoginUsingInputData
        private void StartScarppLoginUsingInputData(object Parameter)
        {
            try
            {
                if (!IsStopScrapper)
                {
                    lstScrapperStopThread.Add(Thread.CurrentThread);
                    lstScrapperStopThread.Distinct();
                    Thread.CurrentThread.IsBackground = true;
                }
                try
                {
                    InBoardPro.StartLinkedinScrapping.lstNumberOfThreads = lstScrapperStopThread;
                }
                catch { }

                string account = string.Empty;
                string post = string.Empty;
                Array paramsArray = new object[1];

                paramsArray = (Array)Parameter;

                KeyValuePair<string, LinkedInMaster> item = (KeyValuePair<string, LinkedInMaster>)paramsArray.GetValue(0);
                account = item.Key;

                try
                {
                    account = account.Split(':')[0];
                }
                catch { }

                GlobusHttpHelper HttpHelper = new GlobusHttpHelper();
                LinkedinLogin Login = new LinkedinLogin();


                Login.accountUser = account;
                Login.accountPass = item.Value._Password;
                Login.proxyAddress = item.Value._ProxyAddress;
                Login.proxyPort = item.Value._ProxyPort;
                Login.proxyUserName = item.Value._ProxyUsername;
                Login.proxyPassword = item.Value._ProxyPassword;
                Login.Postalcode = item.Value._Postalcode;
                Login.Distance = item.Value._Distance;
                Login.IndustryType = item.Value._IndustryType;
                Login.LastName = item.Value._LastName;
             
                InBoardPro.StartLinkedinScrapping objstartscrapp = new InBoardPro.StartLinkedinScrapping();
                objstartscrapp.InBoardProGetDataLogEvents.addToLogger += ScrapperLogEvents_addToLogger;
                Login.logger.addToLogger += new EventHandler(ScrapperLogEvents_addToLogger);

                if (!Login.IsLoggedIn)
                {
                    try
                    {
                        Login.LoginHttpHelper(ref HttpHelper);
                    }
                    catch { }
                }
                try
                {
                    if (Login.IsLoggedIn)
                    {
                        objstartscrapp.ParsingOfInBoardProGetData(ref HttpHelper, Login.accountUser, Login.accountPass, Login.proxyAddress, Login.proxyPort, Login.proxyUserName, Login.proxyPassword, Login.Postalcode, Login.Distance, Login.IndustryType, Login.LastName);
                    }
                }
                catch { }
                objstartscrapp.InBoardProGetDataLogEvents.addToLogger -= ScrapperLogEvents_addToLogger;
                Login.logger.addToLogger -= new EventHandler(ScrapperLogEvents_addToLogger);
            }
            catch { }
        } 
        #endregion

        #region btnstartscrapper_Click
        private void btnstartscrapper_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {

                InBoardPro.StartLinkedinScrapping.counterforpause = 1;
                if (NumberHelper.ValidateNumber(txtminvalue.Text) && NumberHelper.ValidateNumber(txtmaxvalue.Text))
                {
                    try
                    {
                        int minDelay = int.Parse(txtminvalue.Text);
                        int maxDelay = int.Parse(txtmaxvalue.Text);
                        InBoardPro.StartLinkedinScrapping.Mindelay = minDelay;
                        InBoardPro.StartLinkedinScrapping.Maxdelay = maxDelay;
                        //  WallPoster.WallPoster.UseDelay = true;

                        AddLoggerScrapData("[ " + DateTime.Now + " ] => [ Delay Feature Set Between : " + minDelay + "-" + maxDelay + " second ]");
                    }
                    catch { }
                    //delayInSeconds = RandomNumberGenerator.GenerateRandom(minDelay, maxDelay) * 1000;
                }

                if (NumberHelper.ValidateNumber(txtnumberofline.Text.Trim()))
                {
                    try
                    {
                        int Numberoflinne = int.Parse(txtnumberofline.Text);
                        InBoardPro.StartLinkedinScrapping.processNumber = Numberoflinne;
                    }
                    catch { }
                }
                if (rdbautopause.Checked)
                {
                    InBoardPro.StartLinkedinScrapping.autopause = true;
                }
                if (rdbautopauseandresume.Checked)
                {
                    InBoardPro.StartLinkedinScrapping.autopauseandresume = true;
                }

                try
                {
                    Thread thread_LinkedinSearch = new Thread(LinkedInStartScrapperThread);
                    thread_LinkedinSearch.Start();
                }
                catch { }
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddLoggerScrapData("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        } 
        #endregion

        #region btnStopScrapperThread_Click
        private void btnStopScrapperThread_Click(object sender, EventArgs e)
        {
            IsStopScrapper = true;
            foreach (Thread item in lstScrapperStopThread)
            {
                try
                {
                    item.Abort();
                }
                catch { }
            }
            AddLoggerScrapData("-------------------------------------------------------------------------------------------------------------------------------");
            AddLoggerScrapData("[ " + DateTime.Now + " ] => [ PROCESS STOPPED ]");
            AddLoggerScrapData("-------------------------------------------------------------------------------------------------------------------------------");
        } 
        #endregion

        #region MyRegion
        private void btnPasuse_Click(object sender, EventArgs e)
        {
            foreach (Thread itempause in lstScrapperStopThread)
            {
                try
                {
                    itempause.Suspend();
                }
                catch { }
            }
            AddLoggerScrapData("-------------------------------------------------------------------------------------------------------------------------------");
            AddLoggerScrapData("[ " + DateTime.Now + " ] => [PROCESS PAUSE ]");
            AddLoggerScrapData("-------------------------------------------------------------------------------------------------------------------------------");
        } 
        #endregion

        private void btnResume_Click(object sender, EventArgs e)
        {

            DialogResult dlgResult = MessageBox.Show("Do you want to continue?", "Continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlgResult == DialogResult.Yes)
            {
                foreach (Thread itempause in lstScrapperStopThread)
                {
                    try
                    {
                        itempause.Resume();
                    }
                    catch { }
                }
                AddLoggerScrapData("------------------------------------------------------------------------------------------------------------------------------------");
                AddLoggerScrapData("[ " + DateTime.Now + " ] => [ PROCESS START ]");
                AddLoggerScrapData("------------------------------------------------------------------------------------------------------------------------------------");
            }
            else if (dlgResult == DialogResult.No)
            {
                IsStopScrapper = true;
                foreach (Thread item in lstScrapperStopThread)
                {
                    try
                    {
                        item.Abort();
                    }
                    catch { }
                }
                AddLoggerScrapData("-------------------------------------------------------------------------------------------------------------------------------");
                AddLoggerScrapData("[ " + DateTime.Now + " ] => [ PROCESS STOPPED ]");
                AddLoggerScrapData("-------------------------------------------------------------------------------------------------------------------------------");
            }
          
        }

       
      
    }
}
