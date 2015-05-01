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
using BaseLib;

namespace InBoardPro
{
    public partial class FrmLdScraperMultipleExcelInput : Form
    {

        #region FrmLdScraperMultipleExcelInput
        public FrmLdScraperMultipleExcelInput()
        {
            InitializeComponent();
        } 
        #endregion

        #region variable declaration
        public System.Drawing.Image image;
        List<string[]> excelDataMode1 = new List<string[]>();
        List<string[]> excelDataMode2 = new List<string[]>();
        List<string[]> excelDataMode3 = new List<string[]>();
        List<string[]> lstFinalInputData = new List<string[]>();
        List<Thread> lstScrapperStopThread = new List<Thread>();
        bool IsStopScrapper = false;
        bool CheckNetConn = false;
        #endregion

        #region ScrapperLogEvents_addToLogger
        void ScrapperLogEvents_addToLogger(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eventArgs = e as EventsArgs;
                AddLoggerScrapDataMultiExcelInput(eventArgs.log);

            }
        } 
        #endregion

        #region ScrapperRecordsLogEvents_addToLogger
        void ScrapperRecordsLogEvents_addToLogger(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eventArgs = e as EventsArgs;
                AddLoggerScrapDataMultiExcelInput(eventArgs.log);

            }
        } 
        #endregion

        #region AddLoggerScrapDataMultiExcelInput
        private void AddLoggerScrapDataMultiExcelInput(string log)
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

        #region FrmLdScraperMultipleInput_Load
        private void FrmLdScraperMultipleInput_Load(object sender, EventArgs e)
        {
            image = Properties.Resources.background;
            rdbautopauseandresume.Checked = true;
        }
        #endregion

        #region FrmLdScraperMultipleInput_Paint
        private void FrmLdScraperMultipleInput_Paint(object sender, PaintEventArgs e)
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

        #region btnBrowseSirNameWise_Click
        private void btnBrowseSirNameWise_Click(object sender, EventArgs e)
        {
            try
            {
                LinkedInManager.linkedInDictionaryExcelInput.Clear();
                clsEmailParser objparser = new clsEmailParser();
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    excelDataMode1.Clear();
                    ofd.InitialDirectory = System.Windows.Forms.Application.StartupPath;
                    ofd.Filter = "Text Files (*.xlsx)|*.xlsx";
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtInputSirNameWise.Text = ofd.FileName;
                        excelDataMode1 = objparser.parseExcel(txtInputSirNameWise.Text);
                    }

                }

                foreach (string[] item in excelDataMode1)
                {
                    AddLoggerScrapDataMultiExcelInput("[ " + DateTime.Now + " ] => [ Added: LastName:" + item[2] + " CentralZipCode:" + item[0] + " Distance from ZipCode:" + item[1] + " UserID:" + item[3] + " ]");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        #endregion

        #region btnBrowseIndustryZoneWise_Click
        private void btnBrowseIndustryZoneWise_Click(object sender, EventArgs e)
        {
            try
            {
                LinkedInManager.linkedInDictionaryExcelInput.Clear();
                clsEmailParser objparser = new clsEmailParser();
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    excelDataMode2.Clear();
                    ofd.InitialDirectory = System.Windows.Forms.Application.StartupPath;
                    ofd.Filter = "Text Files (*.xlsx)|*.xlsx";
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtInputIndustryZoneWise.Text = ofd.FileName;
                        excelDataMode2 = objparser.parseExcel(txtInputIndustryZoneWise.Text);
                    }
                }

                foreach (string[] item in excelDataMode2)
                {
                    AddLoggerScrapDataMultiExcelInput("[ " + DateTime.Now + " ] => [ Added: IndustryZone:" + item[2] + " CentralZipCode:" + item[0] + " Distance from ZipCode:" + item[1] + " UserID:" + item[3] + " ]");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        #endregion

        #region btnBrowseCentralZipWise_Click
        private void btnBrowseCentralZipWise_Click(object sender, EventArgs e)
        {
            try
            {
                LinkedInManager.linkedInDictionaryExcelInput.Clear();
                clsEmailParser objparser = new clsEmailParser();
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    excelDataMode3.Clear();
                    ofd.InitialDirectory = System.Windows.Forms.Application.StartupPath;
                    ofd.Filter = "Text Files (*.xlsx)|*.xlsx";
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtInputCentralZipWise.Text = ofd.FileName;
                        excelDataMode3 = objparser.parseExcel(txtInputCentralZipWise.Text);
                    }
                }

                foreach (string[] item in excelDataMode3)
                {
                    AddLoggerScrapDataMultiExcelInput("[ " + DateTime.Now + " ] => [ Added: CentralZipCode:" + item[0] + " Distance from ZipCode:" + item[1] + " UserID:" + item[2] + " ]");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        #endregion

        #region btnLinkedinSearchSirNameWise_Click
        private void btnLinkedinSearchSirNameWise_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {
                try
                {
                    Thread thread_LinkedinSearchMode1 = new Thread(InBoardProGetDataThreadMode1);
                    thread_LinkedinSearchMode1.Start();
                }
                catch { }
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddLoggerScrapDataMultiExcelInput("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        }
        #endregion

        #region btnLinkedinSearchIndustryZoneWise_Click
        private void btnLinkedinSearchIndustryZoneWise_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {

                try
                {
                    Thread thread_LinkedinSearchMode2 = new Thread(InBoardProGetDataThreadMode2);
                    thread_LinkedinSearchMode2.Start();
                }
                catch { }

            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddLoggerScrapDataMultiExcelInput("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        }
        #endregion

        #region btnLinkedinSearchCentralZipWise_Click
        private void btnLinkedinSearchCentralZipWise_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {

                try
                {
                    Thread thread_linkedinSearchMode3 = new Thread(InBoardProGetDataThreadMode3);
                    thread_linkedinSearchMode3.Start();
                }
                catch { }
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddLoggerScrapDataMultiExcelInput("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        }
        #endregion

        #region InBoardProGetDataThreadMode1
        private void InBoardProGetDataThreadMode1()
        {

            foreach (string[] itemArr in excelDataMode1)
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
                            objLinkedInMaster._LastName = itemArr[2];
                        }
                    }
                    catch { }
                    try
                    {
                        if (!string.IsNullOrEmpty(itemArr[3]))
                        {
                            objLinkedInMaster._Username = itemArr[3];
                        }
                    }
                    catch { }
                    try
                    {
                        if (!string.IsNullOrEmpty(itemArr[4]))
                        {
                            objLinkedInMaster._Password = itemArr[4];
                        }
                    }
                    catch { }
                    try
                    {
                        objLinkedInMaster._ProxyAddress = itemArr[5];
                        if (!string.IsNullOrEmpty(itemArr[5]))
                        {
                            string[] words = itemArr[5].Split(new char[] { char.Parse(":") });
                            if (words.Count() > 1)
                            {
                                try
                                {
                                    string Portno = words[1];
                                    objLinkedInMaster._ProxyPort = Portno;
                                    objLinkedInMaster._ProxyAddress = words[0];
                                }
                                catch { }
                            }
                            else
                            {
                                try
                                {
                                    objLinkedInMaster._ProxyAddress = words[0];
                                }
                                catch { }

                            }
                        }

                    }
                    catch { }
                    try
                    {
                        if (!string.IsNullOrEmpty(itemArr[6]))
                        {
                            objLinkedInMaster._ProxyPassword = itemArr[6];
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
            Thread objloginthread1 = new Thread(StartLoginUsingThreadPoolMode1);
            objloginthread1.Start();
        }
        #endregion

        #region InBoardProGetDataThreadMode2
        private void InBoardProGetDataThreadMode2()
        {

            foreach (string[] itemArr in excelDataMode2)
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
                            objLinkedInMaster._Username = itemArr[3];
                        }
                    }
                    catch { }
                    try
                    {
                        if (!string.IsNullOrEmpty(itemArr[4]))
                        {
                            objLinkedInMaster._Password = itemArr[4];
                        }
                    }
                    catch { }
                    try
                    {
                        objLinkedInMaster._ProxyAddress = itemArr[5];
                        if (!string.IsNullOrEmpty(itemArr[5]))
                        {
                            string[] words = itemArr[5].Split(new char[] { char.Parse(":") });
                            if (words.Count() > 1)
                            {
                                try
                                {
                                    string Portno = words[1];
                                    objLinkedInMaster._ProxyPort = Portno;
                                    objLinkedInMaster._ProxyAddress = words[0];
                                }
                                catch { }
                            }
                            else
                            {
                                try
                                {
                                    objLinkedInMaster._ProxyAddress = words[0];
                                }
                                catch { }

                            }
                        }

                    }
                    catch { }
                    try
                    {
                        if (!string.IsNullOrEmpty(itemArr[6]))
                        {
                            objLinkedInMaster._ProxyPassword = itemArr[6];
                        }
                    }
                    catch { }
                    try
                    {
                        if (!string.IsNullOrEmpty(itemArr[0]) && !string.IsNullOrEmpty(itemArr[1]))
                        {
                            LinkedInManager.linkedInDictionaryExcelInput.Add(objLinkedInMaster._Username+ ":" +objLinkedInMaster._IndustryType + ":" +objLinkedInMaster._Postalcode, objLinkedInMaster);
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
            Thread objloginthread2 = new Thread(StartLoginUsingThreadPoolMode2);
            objloginthread2.Start();
        }
        #endregion

        #region InBoardProGetDataThreadMode3
        private void InBoardProGetDataThreadMode3()
        {

            foreach (string[] itemArr in excelDataMode3)
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
                            objLinkedInMaster._Username = itemArr[2];
                        }
                    }
                    catch { }
                    try
                    {
                        if (!string.IsNullOrEmpty(itemArr[3]))
                        {
                            objLinkedInMaster._Password = itemArr[3];
                        }
                    }
                    catch { }
                    try
                    {
                        objLinkedInMaster._ProxyAddress = itemArr[4];
                        if (!string.IsNullOrEmpty(itemArr[4]))
                        {
                            string[] words = itemArr[4].Split(new char[] { char.Parse(":") });
                            if (words.Count() > 1)
                            {
                                try
                                {
                                    string Portno = words[1];
                                    objLinkedInMaster._ProxyPort = Portno;
                                    objLinkedInMaster._ProxyAddress = words[0];
                                }
                                catch { }
                            }
                            else
                            {
                                try
                                {
                                    objLinkedInMaster._ProxyAddress = words[0];
                                }
                                catch { }

                            }
                        }

                    }
                    catch { }
                    try
                    {
                        if (!string.IsNullOrEmpty(itemArr[5]))
                        {
                            objLinkedInMaster._ProxyPassword = itemArr[5];
                        }
                    }
                    catch { }
                    try
                    {
                        if (!string.IsNullOrEmpty(itemArr[0]) && !string.IsNullOrEmpty(itemArr[1]))
                        {
                            LinkedInManager.linkedInDictionaryExcelInput.Add(objLinkedInMaster._Username+ ":" +objLinkedInMaster._Postalcode, objLinkedInMaster);
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
            Thread objloginthread3 = new Thread(StartLoginUsingThreadPoolMode3);
            objloginthread3.Start();
        }
        #endregion

        int Counter_Thread = 0;
        #region StartLoginUsingThreadPoolMode1
        private void StartLoginUsingThreadPoolMode1()
        {
            Counter_Thread = LinkedInManager.linkedInDictionaryExcelInput.Count();
            foreach (KeyValuePair<string, LinkedInMaster> item in LinkedInManager.linkedInDictionaryExcelInput)
            {
                try
                {
                    try
                    {
                        if (NumberHelper.ValidateNumber(txtThreadFirstMode.Text))
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

        #region StartLoginUsingThreadPoolMode2
        private void StartLoginUsingThreadPoolMode2()
        {
            Counter_Thread = LinkedInManager.linkedInDictionaryExcelInput.Count();
            foreach (KeyValuePair<string, LinkedInMaster> item in LinkedInManager.linkedInDictionaryExcelInput)
            {
                try
                {
                    try
                    {
                        if (NumberHelper.ValidateNumber(txtThreadFirstMode.Text))
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

        #region StartLoginUsingThreadPoolMode3
        private void StartLoginUsingThreadPoolMode3()
        {
            Counter_Thread = LinkedInManager.linkedInDictionaryExcelInput.Count();
            foreach (KeyValuePair<string, LinkedInMaster> item in LinkedInManager.linkedInDictionaryExcelInput)
            {
                try
                {
                    try
                    {
                        if (NumberHelper.ValidateNumber(txtThreadFirstMode.Text))
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

                Login.accountUser = item.Value._Username; ;
                Login.accountPass = item.Value._Password;
                Login.proxyAddress = item.Value._ProxyAddress;
                Login.proxyPort = item.Value._ProxyPort;
                Login.proxyUserName = item.Value._ProxyUsername;
                Login.proxyPassword = item.Value._ProxyPassword;
                Login.Postalcode = item.Value._Postalcode;
                Login.Distance = item.Value._Distance;
                Login.LastName = item.Value._LastName;
                Login.IndustryType = item.Value._IndustryType;
                InBoardPro.LinkedinMultipleScrapRecord obj_Scrapper = new InBoardPro.LinkedinMultipleScrapRecord(Login.accountUser, Login.accountPass, Login.proxyAddress, Login.proxyPort, Login.proxyUserName, Login.proxyPassword, Login.Postalcode, Login.Distance, Login.IndustryType, Login.LastName);
                obj_Scrapper.InBoardProGetDataResultLogEvents.addToLogger += ScrapperRecordsLogEvents_addToLogger;
                Login.logger.addToLogger += new EventHandler(ScrapperRecordsLogEvents_addToLogger);

                if (!Login.IsLoggedIn)
                {
                    Login.LoginHttpHelper(ref HttpHelper);
                }
                try
                {
                    if (Login.IsLoggedIn)
                    {
                        obj_Scrapper.lastName = Login.LastName;
                        obj_Scrapper.postalCode = Login.Postalcode;
                        obj_Scrapper.industryType = Login.IndustryType;
                        obj_Scrapper.GetMultipleRecords(ref HttpHelper);
                    }
                }

                catch { }

                obj_Scrapper.InBoardProGetDataResultLogEvents.addToLogger -= ScrapperRecordsLogEvents_addToLogger;
                Login.logger.addToLogger -= new EventHandler(ScrapperRecordsLogEvents_addToLogger);
            }
            catch { }


            finally
            {
                Counter_Thread--;
                if (Counter_Thread == 0)
                {
                    if (btnLinkedinSearchSirNameWise.InvokeRequired)
                    {
                        btnLinkedinSearchSirNameWise.Invoke(new MethodInvoker(delegate
                        {
                            AddLoggerScrapDataMultiExcelInput("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                            AddLoggerScrapDataMultiExcelInput("---------------------------------------------------------------------------------------------------------------------------");
                        }));

                    }
                }
            }
        }
        #endregion

        #region btnBrowseFinalParsing_Click
        private void btnBrowseFinalParsing_Click(object sender, EventArgs e)
        {
            LinkedInManager.linkedInDictionaryExcelInput.Clear();

            try
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.InitialDirectory = Application.StartupPath;
                    ofd.Filter = "CSV Files (*.csv)|*.csv";
                    lstFinalInputData.Clear();
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtsoftwaregeneratedinputfile.Text = ofd.FileName;
                        lstFinalInputData = CSVUtilities.parseCSV(ofd.FileName);
                    }
                }
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

                lstScrapperStopThread.Clear();
                if (IsStopScrapper)
                {
                    IsStopScrapper = false;
                }
                if (rbtnMode1.Checked == false && rbtnMode2.Checked == false && rbtnMode3.Checked == false)
                {
                    AddLoggerScrapDataMultiExcelInput("[ " + DateTime.Now + " ] => [ Please Select which Mode you have to upload ]");
                    MessageBox.Show("Please Select which Mode you have to upload");
                    return;
                }

                InBoardPro.InBoardProGetDataMultipleExcelInput.counterforpause = 1;
                if (NumberHelper.ValidateNumber(txtminvalue.Text) && NumberHelper.ValidateNumber(txtmaxvalue.Text))
                {
                    try
                    {
                        int minDelay = int.Parse(txtminvalue.Text);
                        int maxDelay = int.Parse(txtmaxvalue.Text);
                        InBoardPro.InBoardProGetDataMultipleExcelInput.Mindelay = minDelay;
                        InBoardPro.InBoardProGetDataMultipleExcelInput.Maxdelay = maxDelay;

                        AddLoggerScrapDataMultiExcelInput("[ " + DateTime.Now + " ] => [ Delay Feature Set Between : " + minDelay + "-" + maxDelay + " second ]");
                    }
                    catch { }

                }

                if (NumberHelper.ValidateNumber(txtnumberofline.Text.Trim()))
                {
                    try
                    {
                        int Numberoflinne = int.Parse(txtnumberofline.Text);
                        InBoardPro.InBoardProGetDataMultipleExcelInput.processNumber = Numberoflinne;
                    }
                    catch { }
                }
                if (rdbautopause.Checked)
                {
                    InBoardPro.InBoardProGetDataMultipleExcelInput.autopause = true;
                }
                if (rdbautopauseandresume.Checked)
                {
                    InBoardPro.InBoardProGetDataMultipleExcelInput.autopauseandresume = true;
                }
                btnstartscrapper.Cursor = Cursors.AppStarting;
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
                AddLoggerScrapDataMultiExcelInput("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        }
        #endregion

        #region LinkedInStartScrapperThread
        private void LinkedInStartScrapperThread()
        {


            foreach (string[] itemArr in lstFinalInputData)
            {
                try
                {

                    LinkedInMaster objLinkedInMaster = new LinkedInMaster();

                    // Mode1
                    if (rbtnMode1.Checked == true)
                    {
                        InBoardPro.InBoardProGetDataMultipleExcelInput.SelectedMode = "Mode1";
                        if (!itemArr[0].Contains("LastName"))
                        {
                            try
                            {
                                if (!string.IsNullOrEmpty(itemArr[1]))
                                {
                                    objLinkedInMaster._Postalcode = itemArr[1];
                                }

                            }
                            catch { }
                            try
                            {
                                if (!string.IsNullOrEmpty(itemArr[2]))
                                {
                                    objLinkedInMaster._Distance = itemArr[2];
                                }
                            }
                            catch { }
                            try
                            {
                                //if (!string.IsNullOrEmpty(itemArr[2]))
                                //{
                                //    objLinkedInMaster._IndustryType = itemArr[2];
                                //}
                            }
                            catch { }
                            try
                            {
                                if (!string.IsNullOrEmpty(itemArr[0]))
                                {
                                    objLinkedInMaster._LastName = itemArr[0];
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
                                            objLinkedInMaster._ProxyAddress = words[0];
                                        }
                                        catch { }
                                    }
                                    else
                                    {
                                        try
                                        {
                                            objLinkedInMaster._ProxyAddress = words[0];
                                        }
                                        catch { }

                                    }
                                }

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
                                LinkedInManager.linkedInDictionaryExcelInput.Add(objLinkedInMaster._Username + ":" +objLinkedInMaster._LastName + ":" +objLinkedInMaster._Postalcode, objLinkedInMaster);
                            }
                            catch { }
                        }
                        //else
                        //{
                        //    AddLoggerScrapData("[ " + DateTime.Now + " ] => [ Wrong Mode Uploaded ! You Have selected Mode 1 (Surname Wise) ]");
                        //}
                    }



                    // Mode2
                    if (rbtnMode2.Checked == true)
                    {
                        InBoardPro.InBoardProGetDataMultipleExcelInput.SelectedMode = "Mode2";
                        if (!itemArr[0].Contains("IndustryZone"))
                        {
                            try
                            {
                                if (!string.IsNullOrEmpty(itemArr[1]))
                                {
                                    objLinkedInMaster._Postalcode = itemArr[1];
                                }

                            }
                            catch { }
                            try
                            {
                                if (!string.IsNullOrEmpty(itemArr[2]))
                                {
                                    objLinkedInMaster._Distance = itemArr[2];
                                }
                            }
                            catch { }
                            try
                            {
                                if (!string.IsNullOrEmpty(itemArr[0]))
                                {
                                    objLinkedInMaster._IndustryType = itemArr[0];
                                }
                            }
                            catch { }
                            try
                            {
                                //if (!string.IsNullOrEmpty(itemArr[0]))
                                //{
                                //    objLinkedInMaster._LastName = itemArr[0];
                                //}
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
                                if (!string.IsNullOrEmpty(itemArr[6]))
                                {
                                    string[] words = itemArr[6].Split(new char[] { char.Parse(":") });
                                    if (words.Count() > 1)
                                    {
                                        try
                                        {
                                            string Portno = words[1];
                                            objLinkedInMaster._ProxyPort = Portno;
                                            objLinkedInMaster._ProxyAddress = words[0];
                                        }
                                        catch { }
                                    }
                                    else
                                    {
                                        try
                                        {
                                            objLinkedInMaster._ProxyAddress = words[0];
                                        }
                                        catch { }

                                    }
                                }

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
                                LinkedInManager.linkedInDictionaryExcelInput.Add(objLinkedInMaster._Username + ":" + objLinkedInMaster._IndustryType + ":" + objLinkedInMaster._Postalcode, objLinkedInMaster);
                            }
                            catch { }
                        }
                        //else
                        //{
                        //    AddLoggerScrapData("[ " + DateTime.Now + " ] => [ Wrong Mode Uploaded ! You Have selected Mode 2 (Industry Zone Wise) ]");
                        //}
                    }


                    // Mode3
                    if (rbtnMode3.Checked == true)
                    {
                        InBoardPro.InBoardProGetDataMultipleExcelInput.SelectedMode = "Mode3";
                        if (!itemArr[0].Contains("CentralZipCode"))
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
                                //if (!string.IsNullOrEmpty(itemArr[0]))
                                //{
                                //    objLinkedInMaster._IndustryType = itemArr[0];
                                //}
                            }
                            catch { }
                            try
                            {
                                //if (!string.IsNullOrEmpty(itemArr[0]))
                                //{
                                //    objLinkedInMaster._LastName = itemArr[0];
                                //}
                            }
                            catch { }
                            try
                            {
                                if (!string.IsNullOrEmpty(itemArr[3]))
                                {
                                    objLinkedInMaster._Username = itemArr[3];
                                }
                            }
                            catch { }
                            try
                            {
                                if (!string.IsNullOrEmpty(itemArr[4]))
                                {
                                    objLinkedInMaster._Password = itemArr[4];
                                }
                            }
                            catch { }
                            try
                            {
                                // objLinkedInMaster._ProxyAddress = itemArr[6];
                                if (!string.IsNullOrEmpty(itemArr[5]))
                                {
                                    string[] words = itemArr[5].Split(new char[] { char.Parse(":") });
                                    if (words.Count() > 1)
                                    {
                                        try
                                        {
                                            string Portno = words[1];
                                            objLinkedInMaster._ProxyPort = Portno;
                                            objLinkedInMaster._ProxyAddress = words[0];
                                        }
                                        catch { }
                                    }
                                    else
                                    {
                                        try
                                        {
                                            objLinkedInMaster._ProxyAddress = words[0];
                                        }
                                        catch { }

                                    }
                                }

                            }
                            catch { }
                            try
                            {
                                if (!string.IsNullOrEmpty(itemArr[6]))
                                {
                                    objLinkedInMaster._ProxyPassword = itemArr[6];
                                }
                            }
                            catch { }
                            try
                            {
                                LinkedInManager.linkedInDictionaryExcelInput.Add(objLinkedInMaster._Username + ":"  + objLinkedInMaster._Postalcode, objLinkedInMaster);
                            }
                            catch { }
                        }
                        //else
                        //{
                        //    AddLoggerScrapData("[ " + DateTime.Now + " ] => [ Wrong Mode Uploaded ! You Have selected Mode 3 (Central Zip Wise) ]");
                        //}
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
                        if (NumberHelper.ValidateNumber(txtThreadFourthModescrapper.Text.Trim()))
                        {
                            int numberofThreds = int.Parse(txtThreadFourthModescrapper.Text.Trim());

                            ThreadPool.SetMaxThreads(numberofThreds, 5);
                            ThreadPool.QueueUserWorkItem(new WaitCallback(StartScarppLoginUsingInputData), new object[] { item });
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
                GlobusHttpHelper HttpHelper = new GlobusHttpHelper();
                LinkedinLogin Login = new LinkedinLogin();

                //Login.accountUser = item.Key;
                Login.accountUser = item.Value._Username;
                Login.accountPass = item.Value._Password;
                Login.proxyAddress = item.Value._ProxyAddress;
                Login.proxyPort = item.Value._ProxyPort;
                Login.proxyUserName = item.Value._ProxyUsername;
                Login.proxyPassword = item.Value._ProxyPassword;
                Login.Postalcode = item.Value._Postalcode;
                Login.Distance = item.Value._Distance;
                Login.IndustryType = item.Value._IndustryType;
                Login.LastName = item.Value._LastName;
                InBoardPro.InBoardProGetDataMultipleExcelInput objstartscrapp = new InBoardPro.InBoardProGetDataMultipleExcelInput();
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
                        objstartscrapp.ParsingOfInBoardProGetDataMultipleInput(ref HttpHelper, Login.accountUser, Login.accountPass, Login.proxyAddress, Login.proxyPort, Login.proxyUserName, Login.proxyPassword, Login.Postalcode, Login.Distance, Login.IndustryType, Login.LastName);
                    }
                }
                catch { }
                objstartscrapp.InBoardProGetDataLogEvents.addToLogger -= ScrapperLogEvents_addToLogger;
                Login.logger.addToLogger -= new EventHandler(ScrapperLogEvents_addToLogger);
            }
            catch { }
        }
        #endregion

        #region btnStopScrapperThread_Click
        private void btnStopScrapperThread_Click(object sender, EventArgs e)
        {
            IsStopScrapper = true;
            List<Thread> lstTemp = lstScrapperStopThread.Distinct().ToList();
            foreach (Thread item in lstTemp)
            {
                try
                {
                    item.Abort();
                }
                catch { }
            }
            AddLoggerScrapDataMultiExcelInput("------------------------------------------------------------------------------------------------------------------------------------");
            AddLoggerScrapDataMultiExcelInput("[ " + DateTime.Now + " ] => [ PROCESS STOPPED ]");
            AddLoggerScrapDataMultiExcelInput("------------------------------------------------------------------------------------------------------------------------------------");
            btnstartscrapper.Cursor = Cursors.Default;
            Counter_Thread = 0;
        } 
        #endregion

        #region btnPause_Click
        private void btnPause_Click(object sender, EventArgs e)
        {
            foreach (Thread itempause in lstScrapperStopThread)
            {
                try
                {
                    itempause.Suspend();
                }
                catch { }
            }
            AddLoggerScrapDataMultiExcelInput("------------------------------------------------------------------------------------------------------------------------------------");
            AddLoggerScrapDataMultiExcelInput("[ " + DateTime.Now + " ] => [ PROCESS PAUSE ]");
            AddLoggerScrapDataMultiExcelInput("------------------------------------------------------------------------------------------------------------------------------------");
        } 
        #endregion

        #region btnResume_Click
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
                AddLoggerScrapDataMultiExcelInput("------------------------------------------------------------------------------------------------------------------------------------");
                AddLoggerScrapDataMultiExcelInput("[ " + DateTime.Now + " ] => [ PROCESS START ]");
                AddLoggerScrapDataMultiExcelInput("------------------------------------------------------------------------------------------------------------------------------------");
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
                AddLoggerScrapDataMultiExcelInput("------------------------------------------------------------------------------------------------------------------------------------");
                AddLoggerScrapDataMultiExcelInput("[ " + DateTime.Now + " ] => [ PROCESS STOPPED ]");
                AddLoggerScrapDataMultiExcelInput("------------------------------------------------------------------------------------------------------------------------------------");

            }
        } 
        #endregion
    }
}
