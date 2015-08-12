using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using InBoardPro;
using BaseLib.DB_Repository;
using System.Threading;
using System.Data.SQLite;
using System.IO;
using System.Drawing.Drawing2D;
using BaseLib;


namespace InBoardPro
{
    public partial class frmAccounts : Form
    {

        #region Variable Declaration
        clsLinkedINAccount objclsLinkedINAccount = new clsLinkedINAccount();
        public static Events AccountsLogEvents = new Events();
        clsLDAccount objclsLDAccount = new clsLDAccount();
        List<string> ValidProxies = new List<string>();
        List<string> ValidPrivateProxies = new List<string>();
        List<Thread> lst_loadAccountsThread = new List<Thread>();
        CheckAccount Check_Account = new CheckAccount();
        int _accountcounter = 0;
        public System.Drawing.Image image;

        bool CheckNetConn = false;
        public int Ld_counter_AccountChecker = 0;
        List<string> Ld_lstAccountCheckerEmail = new List<string>();

        #endregion

        //public static BaseLib.Events AccountsLogEvents = new BaseLib.Events();

        #region AddToListBox
        private void AddToListBox(string log)
        {
            if (lstLogger.InvokeRequired)
            {
                lstLogger.Invoke(new MethodInvoker(delegate
                {
                    lstLogger.Items.Add(log);
                    lstLogger.SelectedIndex = lstLogger.Items.Count - 1;
                }));
            }
            else
            {
                lstLogger.Items.Add(log);
                lstLogger.SelectedIndex = lstLogger.Items.Count - 1;
            }
        }
        #endregion

        #region frmAccounts()
        public frmAccounts()
        {
            InitializeComponent();

            DataBaseHandler.CONstr = "Data Source=" + Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\InBoardPro\\DB_InBoardPro.db" + ";Version=3;";
        }
        #endregion

        #region frmAccounts_Load
        private void frmAccounts_Load(object sender, EventArgs e)
        {
            try
            {
                //LoadGridView();
                CreateFile();
                LoadDataGrid();
                image = Properties.Resources.background;
                LoadFileData();
                checkFreeTrialAccount();
                //RaiseLoadAccountsEvent();
                Check_Account.loggerEvent_AccountChecker.addToLogger += new EventHandler(logger_addToLogger);
            }
            catch
            {
            }
        }

        public void checkFreeTrialAccount()
        {
            if (Globals.IsFreeVersion)
            {

                try
                {
                    DataTable dt1 = objclsLinkedINAccount.SelectAccoutsForGridView();
                    btnClearAccounts.Enabled = false;
                    if (dt1.Rows.Count == 5)
                    {
                        btnLoadAccounts.Enabled = false;
                        btnClearAccounts.Enabled = false;
                    }

                }
                catch (Exception)
                {

                }

            }    
        }

        public void LoadFileData()
        {
            string accountData = string.Empty;
            string laoddata = string.Empty;

            clsDBQueryManager DataBase = new clsDBQueryManager();
            DataTable dt = DataBase.SelectSettingData();
            foreach (DataRow row in dt.Rows)
            {
                if ("LoadAccounts" == row[1].ToString())
                {
                    accountData = StringEncoderDecoder.Decode(row[2].ToString());
                    
                }
            }

            if (File.Exists(accountData))
            {
                txtAccountFile.Text = accountData;
                AddToListBox("[ " + DateTime.Now + " ] => [ Last Accounts Loaded From : " + accountData + " ]");
            }
            else
            {
                AddToListBox("[ " + DateTime.Now + " ] => [ File : " + accountData + " : Does not exsist ]");
            }
        }
        #endregion

        #region CreateFile

        public void CreateFile()
        {
            try
            {
                if (!File.Exists(Environment.GetFolderPath
                (Environment.SpecialFolder.LocalApplicationData) + "\\InBoardPro\\Logger.txt"))
                {
                    Directory.CreateDirectory(Environment.GetFolderPath
                    (Environment.SpecialFolder.LocalApplicationData) + "\\InBoardPro");
                    // File.Create(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "InBoardPro\\ListOfEmails.txt"));
                }
                if (!File.Exists(Environment.GetFolderPath
               (Environment.SpecialFolder.Desktop) + "\\InBoardPro\\Logger.txt"))
                {
                    Directory.CreateDirectory(Environment.GetFolderPath
                    (Environment.SpecialFolder.Desktop) + "\\InBoardPro");

                }

                if (!File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "InBoardPro\\ListOfEmails.txt")))
                {
                    File.Create(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "InBoardPro\\ListOfEmails.txt"));
                }
            }
            catch
            {
            }
        }
        #endregion

        #region btnLoadAccounts_Click
        private void btnLoadAccounts_Click(object sender, EventArgs e)
        {
            try
            {
                    Ld_lstAccountCheckerEmail.Clear();
                    lstLogger.Items.Clear();
                    System.Threading.Thread loadAccountsThread = new System.Threading.Thread(LoadAccounts);
                    loadAccountsThread.SetApartmentState(System.Threading.ApartmentState.STA);

                    loadAccountsThread.IsBackground = true;
                    lst_loadAccountsThread.Add(loadAccountsThread);
                    lst_loadAccountsThread = lst_loadAccountsThread.Distinct().ToList();

                    loadAccountsThread.Start();
                }
            
            catch
            {
            }
        }
        #endregion

        #region LoadAccounts()
        public void  LoadAccounts()
        {
            try
            {
                int countAccount = 0;
                if (Globals.IsFreeVersion)
                {
                    
                        //try
                        //{
                        //    string DeleteQuery = "Delete from tb_LinkedInAccount";
                        //    DataBaseHandler.DeleteQuery(DeleteQuery, "tb_LinkedInAccount");

                        //    LoadDataGrid();
                        //}
                        //catch (Exception)
                        //{

                        //}
                    try
                    {
                        btnClearAccounts.Enabled = false;
                        DataTable dt1 = objclsLinkedINAccount.SelectAccoutsForGridView();
                        countAccount = dt1.Rows.Count;
                        if (countAccount >= 5)
                        {
                            return;
                        }
                        else 
                        {
                            countAccount = 5 - countAccount;
                        }
                        

                    }
                    catch (Exception)
                    {

                    }
                   
                }


                DataTable dt = new DataTable();

               

                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "Text Files (*.txt)|*.txt";
                    ofd.InitialDirectory = Application.StartupPath;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        this.Invoke(new MethodInvoker(delegate
                        {
                            txtAccountFile.Text = ofd.FileName;
                            clsSettingDB ObjclsSettingDB = new clsSettingDB();
                            

                            ObjclsSettingDB.InsertOrUpdateSetting("LoadAccounts", "LoadAccounts", StringEncoderDecoder.Encode(txtAccountFile.Text));
                        }));

                        List<string> templist = GlobusFileHelper.ReadFiletoStringList(ofd.FileName);
                        Ld_lstAccountCheckerEmail.AddRange(templist);                       

                        int countlist = templist.Count();

                        if (Globals.IsFreeVersion)
                        {

                            try
                            {
                                //templist.RemoveRange(5, templist.Count - 5);
                                templist.RemoveRange(countAccount, templist.Count - countAccount);
                            }
                            catch { }

                             
                                 foreach (string item in templist)
                                 {
                                     LoadDBAccount(item);
                                 }

                                 LoadGridView();
                            

                            if (countlist > 5)
                            {

                                try
                                {
                                    this.Invoke(new MethodInvoker(delegate
                                                            {

                                                                FrmFreeTrial frmFreeTrial = new FrmFreeTrial();

                                                                frmFreeTrial.TopMost = true;
                                                                frmFreeTrial.BringToFront();
                                                                frmFreeTrial.ShowDialog();
                                                            }));
                                }
                                catch (Exception)
                                { }
                            }
                            this.Invoke(new MethodInvoker(delegate
                                                           {
                                                               this.Close();
                                                           }));

                        }

                        else
                        {
                            foreach (string item in templist)
                            {
                                LoadDBAccount(item);
                            }

                            LoadGridView();
                        }

                    }


                    AddLoggerAccountLoad("[ " + DateTime.Now + " ] => [ " + LinkedInManager.linkedInDictionary.Count + " Accounts loaded ]");
                    //AddLoggerAccounts(LinkedInManager.linkedInDictionary.Count + " Accounts loaded");                                                                           
                    Log("[ " + DateTime.Now + " ] => [ " + LinkedInManager.linkedInDictionary.Count + " Accounts loaded ]");
                }
            }
            catch
            {
            }


        }
        #endregion

        #region LoadDBAccount
        private void LoadDBAccount(string account)
        {
            try
            {
                try
                {
                    this.Invoke(new MethodInvoker(delegate
                    {
                        lblAccountStatus.Text = "Process Running !";
                    }));
                }
                catch
                {
                }

                string Username = string.Empty;
                string Password = string.Empty;
                string ProxyAddress = string.Empty;
                string ProxyPort = string.Empty;
                string ProxyUserName = string.Empty;
                string proxyPassword = string.Empty;

               // if (!string.IsNullOrEmpty(account) &&(string.IsNullOrEmpty(":")))
                
                      if(account.Contains(":"))
                    {

                    int DataCount = account.Split(':').Length;
                    if (DataCount > 1)
                    {
                        Username = account.Split(':')[0];
                        Password = account.Split(':')[1];
                    }
                    if (DataCount == 4)
                    {
                        ProxyAddress = account.Split(':')[2];
                        ProxyPort = account.Split(':')[3];
                    }
                    else if (DataCount == 6)
                    {
                        ProxyAddress = account.Split(':')[2];
                        ProxyPort = account.Split(':')[3];
                        ProxyUserName = account.Split(':')[4];
                        proxyPassword = account.Split(':')[5];
                    }


                    try
                    {

                        //*** Add to Database ***************************
                        string Query = "Insert into tb_LinkedInAccount values('" + Username + "','" + Password + "','" + ProxyAddress + "','" + ProxyPort + "','" + ProxyUserName + "','" + proxyPassword + "')";
                        DataBaseHandler.InsertQuery(Query, "tb_LinkedInAccount");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.StackTrace);
                        AddLoggerAccounts("[ " + DateTime.Now + " ] => [ Account Loading has some problem in DataBase: ]");
                    }

                }
            }
            catch
            {
            }
        }
        #endregion
     
        #region LoadGridView()
        public void LoadGridView()
        {
            try
            {
                DataTable dt = objclsLinkedINAccount.SelectAccoutsForGridView();
                try
                {
                    dgvAccount.Invoke(new MethodInvoker(delegate
                    {
                        dgvAccount.DataSource = dt;

                    }));
                }
                catch { }

                //*** Add to Dictonary *************************** 

                LinkedInManager.linkedInDictionary.Clear();


                try
                {
                    foreach (DataRow DTRow in dt.Rows)
                    {
                        try
                        {
                            LinkedInMaster objLinkedInMaster = new LinkedInMaster();
                            objLinkedInMaster._Username = DTRow[0].ToString();
                            objLinkedInMaster._Password = DTRow[1].ToString();
                            objLinkedInMaster._ProxyAddress = DTRow[2].ToString();
                            objLinkedInMaster._ProxyPort = DTRow[3].ToString();
                            objLinkedInMaster._ProxyUsername = DTRow[4].ToString();
                            objLinkedInMaster._ProxyPassword = DTRow[5].ToString();

                            LinkedInManager.linkedInDictionary.Add(objLinkedInMaster._Username, objLinkedInMaster);
                            //Ld_lstAccountCheckerEmail.Add(objLinkedInMaster._Username, objLinkedInMaster);
                            if (Globals.IsFreeVersion)
                            {
                                if (dt.Rows.Count >= 5)
                                {
                                    if (_accountcounter > 4)
                                    {
                                        FrmFreeTrial frmFreeTrial = new FrmFreeTrial();
                                        frmFreeTrial.TopMost = true;
                                        frmFreeTrial.ShowDialog();
                                        break;
                                    }
                                    else
                                    {
                                        _accountcounter++;
                                    }
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    //AddLoggerAccountLoad(LinkedInManager.linkedInDictionary.Count + " Accounts loaded");
                    ////AddLoggerAccounts(LinkedInManager.linkedInDictionary.Count + " Accounts loaded");                                                                           
                    //Log(LinkedInManager.linkedInDictionary.Count + " Accounts loaded");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    AddLoggerAccountLoad("[ " + DateTime.Now + " ] => [ Account Loading has some problem in GridView ]");
                }

                try
                {
                    this.Invoke(new MethodInvoker(delegate
                    {
                        lblAccountStatus.Text = "PROCESS COMPLETED";
                    }));
                }
                catch
                {
                }

                try
                {
                    if (dt.Rows.Count > 0)
                    {
                        label6.Invoke(new MethodInvoker(delegate
                        {
                            label6.Text = dt.Rows.Count.ToString();
                        }));

                    }
                    else
                    {
                        label6.Invoke(new MethodInvoker(delegate
                        {
                            label6.Text = (0).ToString();
                        }));
                    }
                }
                catch
                {
                }
            }
            catch
            {
            }
        }
        #endregion

        #region AddLoggerAccountLoad
        private void AddLoggerAccountLoad(string log)
        {
            this.Invoke(new MethodInvoker(delegate
            {
                lstLogger.Items.Add(log);
                lstLogger.SelectedIndex = lstLogger.Items.Count - 1;////Change lbGeneralLogs with  lstLogger 
            }));
        }
        #endregion

        #region AddLoggerAccounts
        private void AddLoggerAccounts(string message)
        {
            EventsArgs eventArgs = new EventsArgs(message);
            AccountsLogEvents.LogText(eventArgs);
        }
        #endregion

        #region logger_addToLogger

        void logger_addToLogger(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eventArgs = e as EventsArgs;
                AddLoggerAccountLoad(eventArgs.log);
            }
        }

        #endregion

        #region btnClearAccounts_Click
        private void btnClearAccounts_Click(object sender, EventArgs e)
        {
            try
            {

                if (dgvAccount.Rows.Count <= 1)
                {
                    MessageBox.Show("There are no Accounts in DataBase");
                    return;
                }


                else if (MessageBox.Show("Do you really want to delete all the accounts from Database", "Confirm delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    string DeleteQuery = "Delete from tb_LinkedInAccount";
                    DataBaseHandler.DeleteQuery(DeleteQuery, "tb_LinkedInAccount");
                    Ld_lstAccountCheckerEmail.Clear();

                    lstLogger.Items.Clear();
                    AddLoggerAccountLoad("[ " + DateTime.Now + " ] => [ All Accounts Are Clear From The Record ! ]");
                    LoadGridView();

                }
            }
            catch { }
        }
        
        #endregion

        #region ButonClearProxies_Click
        private void ButonClearProxies_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvAccount.Rows.Count <= 1)
                {
                    MessageBox.Show("There are no Accounts in DataBase");
                    return;
                }

                bool accountExist = false;
                foreach (DataGridViewRow item in dgvAccount.Rows)
                {
                    try
                    {
                        string proxy = item.Cells["ProxyAddress"].Value.ToString();

                        string Port = item.Cells["ProxyPort"].Value.ToString();

                        if (!string.IsNullOrEmpty(proxy) || Port!="0")
                        {
                            accountExist = true;
                            break;
                        }
                    }
                    catch { };

                }

                if (!accountExist)
                {
                    MessageBox.Show("There are no Proxy in DataBase");
                    return;
                }
               

              else if (MessageBox.Show("Do you really want to delete all the  Proxies from Database???", "Proxy", MessageBoxButtons.YesNo) == DialogResult.Yes)
             {
                System.Threading.Thread ClearProxiesThread = new System.Threading.Thread(ClearProxy);
                ClearProxiesThread.SetApartmentState(System.Threading.ApartmentState.STA);

                ClearProxiesThread.IsBackground = true;
                lst_loadAccountsThread.Add(ClearProxiesThread);
                lst_loadAccountsThread = lst_loadAccountsThread.Distinct().ToList();

                ClearProxiesThread.Start();
            
        
                }
            }
            catch { }
          }   

           
        

        private void ClearProxy()
        {
            try
            {
                if (dgvAccount.Rows.Count<= 1)
                {
                    MessageBox.Show("There are no Accounts in DataBase");
                    return;
                }
                else
                {
                    try
                    {
                        DataSet ds = new DataSet();

                        using (SQLiteConnection con = new SQLiteConnection(DataBaseHandler.CONstr))
                        {

                            //using (SQLiteDataAdapter ad = new SQLiteDataAdapter("SELECT * FROM tb_FBAccount WHERE ProxyAddress = '" + proxyAddress + "'", con))
                            using (SQLiteDataAdapter ad = new SQLiteDataAdapter("SELECT * FROM tb_LinkedInAccount", con))
                            {
                                ad.Fill(ds);

                                if (ds.Tables[0].Rows.Count >= 1)
                                {
                                    AddLoggerAccountLoad("[ " + DateTime.Now + " ] => [ Please Wait Process Is Running ...! ]");

                                    if (MessageBox.Show("Do you really want to clear all proxies from Accounts", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                    {
                                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                        {
                                            string UpdateQuery = "Update tb_LinkedInAccount Set IPAddress='" + "" + "', IPPort='" + "" + "', IPUserName='" + "" + "', IPPassword='" + "" + "' WHERE UserName='" + ds.Tables[0].Rows[i]["UserName"].ToString() + "'";
                                            DataBaseHandler.UpdateQuery(UpdateQuery, "tb_LinkedInAccount");
                                        }

                                        AddLoggerAccountLoad("[ " + DateTime.Now + " ] => [ All Proxy details has been cleared. ]");
                                        LoadDataGrid();//Refresh Datagrid
                                    }

                                }
                                else
                                {
                                    MessageBox.Show("No Proxies To Clear");
                                }

                            }



                        }
                    }

                    catch (Exception ex)
                    {
                        //GlobusFileHelper.AppendStringToTextfileNewLine("Error in Clearing Proxies :- " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_TwtErrorLogs);
                    }
                }
            }
            catch
            {
            }
        }

        #endregion

        #region ExportAccountsToFile
        private void ExportAccountsToFile(string filePath)
        {
            DataTable dt = objclsLDAccount.SelectAccoutsForGridView();

            foreach (DataRow dRow in dt.Rows)
            {
                try
                {
                    string user = dRow[0].ToString();
                    string pass = dRow[1].ToString();
                    string proxyAdd = dRow[2].ToString();
                    string proxyPort = dRow[3].ToString();
                    string proxyUser = dRow[4].ToString();
                    string proxyPass = dRow[5].ToString();

                    string data = user + ":" + pass + ":" + proxyAdd + ":" + proxyPort + ":" + proxyUser + ":" + proxyPass;
                    GlobusFileHelper.AppendStringToTextfileNewLine(data, filePath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }

            }

        }
        #endregion

        #region btnExportAccounts_Click
        private void btnExportAccounts_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvAccount.Rows.Count <= 1)
                {
                    MessageBox.Show("There are no Accounts in DataBase");
                    return;
                }

                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "Text Files (*.txt)|*.txt";
                    ofd.InitialDirectory = Application.StartupPath;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        ExportAccountsToFile(ofd.FileName);
                        AddToListBox("[ " + DateTime.Now + " ] => [ Accounts Exported to ---" + ofd.FileName + " ]");
                    }
                }
            }
            catch
            {
            }
        }
        #endregion

        #region LoadDataGrid()
        private void LoadDataGrid()
        {
            Ld_lstAccountCheckerEmail.Clear();
            try
            {
                DataTable dt = objclsLDAccount.SelectAccoutsForGridView();

                //ds = new DataSet();
                //ds.Tables.Add(dt);

                try
                {
                    this.Invoke(new MethodInvoker(delegate
                    {
                        lblAccountStatus.Text = "Process Running !";
                    }));
                }
                catch
                {
                }



                dgvAccount.Invoke(new MethodInvoker(delegate
                {
                    dgvAccount.DataSource = dt;
                }));

                Globals.listAccounts.Clear();
                LinkedInManager.linkedInDictionary.Clear();


                ///Add Twitter instances to TweetAccountContainer.dictionary_TweetAccount
                foreach (DataRow dRow in dt.Rows)
                {
                    try
                    {
                        LinkedInMaster LinkedIn = new LinkedInMaster();
                        LinkedIn._Username = dRow[0].ToString();
                        LinkedIn._Password = dRow[1].ToString();
                        LinkedIn._ProxyAddress = dRow[2].ToString();
                        LinkedIn._ProxyPort = dRow[3].ToString();
                        LinkedIn._ProxyUsername = dRow[4].ToString();
                        LinkedIn._ProxyPassword = dRow[5].ToString();

                        //if (!string.IsNullOrEmpty(dRow[7].ToString()))
                        //{
                        //    LinkedIn.profileStatus = int.Parse(dRow[7].ToString());
                        //}
                        //if (Globals.IsFreeVersion)
                        //{
                        //    if (dt.Rows.Count >= 5)
                        //    {
                        //        InBoardPro.FrmFreeTrial frmFreeTrial = new InBoardPro.FrmFreeTrial();
                        //        frmFreeTrial.TopMost = true;
                        //        frmFreeTrial.BringToFront();
                        //        frmFreeTrial.ShowDialog();
                        //        break;
                        //    }
                        //}


                        Globals.listAccounts.Add(LinkedIn._Username + ":" + LinkedIn._Password + ":" + LinkedIn._ProxyAddress + ":" + LinkedIn._ProxyPort + ":" + LinkedIn._ProxyUsername + ":" + LinkedIn._ProxyPassword);
                        LinkedInManager.linkedInDictionary.Add(LinkedIn._Username, LinkedIn);
                        
                        

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.StackTrace);
                    }

                }

                Ld_lstAccountCheckerEmail.AddRange(Globals.listAccounts);        

                try
                {
                    if (dt.Rows.Count > 0)
                    {
                        label6.Invoke(new MethodInvoker(delegate
                        {
                            label6.Text = dt.Rows.Count.ToString();
                        }));

                    }
                    else
                    {
                        label6.Invoke(new MethodInvoker(delegate
                        {
                            label6.Text = (0).ToString();
                        }));
                    }
                }
                catch
                {
                }

                Console.WriteLine(Globals.listAccounts.Count + " Accounts loaded");
                AddToListBox("[ " + DateTime.Now + " ] => [ " + Globals.listAccounts.Count + " Accounts loaded ]");
                Log("[ " + DateTime.Now + " ] => [ " + Globals.listAccounts.Count + " Accounts loaded ]");

                try
                {
                    this.Invoke(new MethodInvoker(delegate
                    {
                        lblAccountStatus.Text = "PROCESS COMPLETED";
                    }));
                }
                catch
                {
                }
            }
            catch { }

            //RaiseLoadAccountsEvent();
        }
        #endregion

        #region Log
        private void Log(string message)
        {
            EventsArgs eventArgs = new EventsArgs(message);
            AccountsLogEvents.LogText(eventArgs);
        }
        #endregion

        #region btnAssignProxy_Click
        int accountsPerProxy = 10;  //Change this to change Number of Accounts to be set per proxy
        static int i = 0;
        List<string> lstProxies = new List<string>();
        ProxyUtilitiesFromDataBase proxyFetcher = new ProxyUtilitiesFromDataBase();

        private void btnAssignProxy_Click(object sender, EventArgs e)
        {
            try
            {
                System.Threading.Thread AssignProxyThread = new System.Threading.Thread(AssignProxy);
                AssignProxyThread.SetApartmentState(System.Threading.ApartmentState.STA);

                AssignProxyThread.IsBackground = true;
                lst_loadAccountsThread.Add(AssignProxyThread);
                lst_loadAccountsThread = lst_loadAccountsThread.Distinct().ToList();

                AssignProxyThread.Start();
            }
            catch
            {
            }


        }

        private void AssignProxy()
        {
            try
            {


                if (MessageBox.Show("Assign Private/Public proxies from Database???", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    try
                    {


                        List<string> lstProxies = proxyFetcher.GetPublicProxies();
                        if (lstProxies.Count > 0)
                        {
                            if (!string.IsNullOrEmpty(txtAccountsPerProxy.Text) && GlobusRegex.ValidateNumber(txtAccountsPerProxy.Text))
                            {
                                try
                                {
                                    accountsPerProxy = int.Parse(txtAccountsPerProxy.Text);
                                }
                                catch
                                {
                                    accountsPerProxy = 10;
                                }
                            }

                            AddLoggerAccountLoad("[ " + DateTime.Now + " ] => [ Please Wait Process Running ....! ]");

                            proxyFetcher.AssignProxiesToAccounts(lstProxies, accountsPerProxy);//AssignProxiesToAccounts(lstProxies);
                            LoadDataGrid();   //Refresh Datagrid

                            if (dgvAccount.RowCount > 1)
                            {
                                AddLoggerAccountLoad("[ " + DateTime.Now + " ] => [ All Proxy has been Alloted. ]");

                            }
                            else
                            {
                                AddLoggerAccountLoad("[ " + DateTime.Now + " ] => [ Please Load Some Account ]");
                            }

                        }
                        else
                        {
                            MessageBox.Show("Please assign Private/Public proxies from Proxies Tab in Main Page OR Upload a proxies Text File");
                        }
                    }
                    catch { }
                }
                else
                {
                    try
                    {
                        using (OpenFileDialog ofd = new OpenFileDialog())
                        {
                            ofd.Filter = "Text Files (*.txt)|*.txt";
                            ofd.InitialDirectory = Application.StartupPath;
                            if (ofd.ShowDialog() == DialogResult.OK)
                            {
                                lstProxies = new List<string>();

                                lstProxies = GlobusFileHelper.ReadFiletoStringList(ofd.FileName);

                                if (!string.IsNullOrEmpty(txtAccountsPerProxy.Text) && GlobusRegex.ValidateNumber(txtAccountsPerProxy.Text))
                                {
                                    try
                                    {
                                        accountsPerProxy = int.Parse(txtAccountsPerProxy.Text);
                                    }
                                    catch
                                    {
                                        accountsPerProxy = 10;
                                    }
                                }
                                AddLoggerAccountLoad("[ " + DateTime.Now + " ] => [ Please Wait Process Running ....! ]");

                                proxyFetcher.AssignProxiesToAccounts(lstProxies, accountsPerProxy);//AssignProxiesToAccounts(lstProxies);
                                LoadDataGrid();   //Refresh Datagrid
                                //AssignProxiesToAccountsModified();
                            }
                        }
                    }
                    catch
                    {
                    }
                }
                AddLoggerAccountLoad("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                AddLoggerAccountLoad("----------------------------------------------------------------------------------------");
            }
            catch
            {
            }
        }
        #endregion

        string userNameForPasswordUpdate = string.Empty;
        string passwordForPasswordUpdate = string.Empty;

        #region dgvAccount_CellContentClick
        private void dgvAccount_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (Globals.IsFreeVersion)
            {
                if (dgvAccount.Rows.Count < 1)
                {
                    MessageBox.Show("There are no Accounts in DataBase");
                    return;
                }
                else
                {
                    try
                    {
                        frmAccountPasswordUpdate objfrmAccountPasswordUpdate = new frmAccountPasswordUpdate();
                        CheckAccount objCheckAccount = new CheckAccount();

                        userNameForPasswordUpdate = dgvAccount.Rows[e.RowIndex].Cells[0].FormattedValue.ToString();
                        passwordForPasswordUpdate = dgvAccount.Rows[e.RowIndex].Cells[1].FormattedValue.ToString();
                        objCheckAccount.UpdatePassword(userNameForPasswordUpdate, passwordForPasswordUpdate);
                        objfrmAccountPasswordUpdate.Show();

                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                } 
            }

            //using (OpenFileDialog ofd = new OpenFileDialog())
            //{
            //    ofd.Filter = "Text Files (*.txt)|*.txt";
            //    ofd.InitialDirectory = Application.StartupPath;
            //    if (ofd.ShowDialog() == DialogResult.OK)
            //    {
            //        ExportAccountsToFile(ofd.FileName);
            //        AddToListBox("Accounts Exported to ---" + ofd.FileName);
            //    }
            //}

        }
        #endregion

        #region frmAccounts_Paint
        private void frmAccounts_Paint(object sender, PaintEventArgs e)
        {

            Graphics g;

            g = e.Graphics;

            g.SmoothingMode = SmoothingMode.HighQuality;

            // Draw the background.
            //g.FillRectangle(Brushes.Yellow, new Rectangle(new Point(0, 0), this.ClientSize));

            //// Draw the image.
            g.DrawImage(image, 0, 0, this.Width, this.Height);

        }
        #endregion

        #region splitContainer1_Panel1_Paint
        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

            Graphics g;

            g = e.Graphics;

            g.SmoothingMode = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            // Draw the background.
            //g.FillRectangle(Brushes.Yellow, new Rectangle(new Point(0, 0), this.ClientSize));

            //// Draw the image.
            g.DrawImage(image, 0, 0, splitContainer1.Width, splitContainer1.Height);
        }
        #endregion

        #region splitContainer1_Panel2_Paint
        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

            Graphics g;

            g = e.Graphics;

            g.SmoothingMode = SmoothingMode.HighQuality;

            // Draw the background.
            //g.FillRectangle(Brushes.Yellow, new Rectangle(new Point(0, 0), this.ClientSize));

            //// Draw the image.
            g.DrawImage(image, 0, 0, splitContainer1.Width, splitContainer1.Height);
        }
        #endregion

        #region gbManageAccount_Paint
        private void gbManageAccount_Paint(object sender, PaintEventArgs e)
        {
            Graphics g;

            g = e.Graphics;

            g.SmoothingMode = SmoothingMode.HighQuality;

            //Draw the background.
            //g.FillRectangle(Brushes.Yellow, new Rectangle(new Point(0, 0), this.ClientSize));

            // Draw the image.
            g.DrawImage(image, 0, 0, gbManageAccount.Width, gbManageAccount.Height);
        }
        #endregion


        public static Events event_LoadAccounts = new Events();

        public void RaiseLoadAccountsEvent()
        {
            EventsArgs eArgs = new EventsArgs("");
            event_LoadAccounts.LogText(eArgs);
        }

        private void frmAccounts_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                // MessageBox.Show("Please Wait Proccess Running !");
                frmMain.IsAccountopen = true;
                if (lblAccountStatus.Text.Contains("Process Running !"))
                {
                    if (MessageBox.Show("Accounts are still being loaded into DataBase, do you still want to close this form? ", "Confirm close", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        foreach (Thread item in lst_loadAccountsThread)
                        {
                            try
                            {
                                item.Abort();
                            }
                            catch (Exception)
                            {

                            }
                        }
                    }
                }
                else
                {
                    foreach (Thread item in lst_loadAccountsThread)
                    {
                        try
                        {
                            item.Abort();

                        }
                        catch (Exception)
                        {

                        }
                    }
                }

            }
            catch
            {
            }

        }

        
        private void btnCheckAccount_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {

                try
                {
                    int NoofThread = 3;
                    if (NumberHelper.ValidateNumber(txtNoOf_Thread.Text.Trim()))
                    {
                        NoofThread = int.Parse(txtNoOf_Thread.Text.Trim());
                    }
                    if (NoofThread <= 3)
                    {
                        NoofThread = 4;
                    }
                    ThreadPool.SetMaxThreads(NoofThread, 3);

                    CheckAccount.TotalEmails = Ld_lstAccountCheckerEmail.Count;
                    CheckAccount.Counter = 0;

                    if (Ld_lstAccountCheckerEmail.Count > 0)
                    {
                        foreach (string account in Ld_lstAccountCheckerEmail)
                        {
                            ThreadPool.QueueUserWorkItem(new WaitCallback(LD_CheckAccountMultiThread), new object[] { account });
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please Upload The File For Check Account ! ");
                        return;
                    }
                }
                catch
                {
                }

                finally
                {
                    if (btnCheckAccount.InvokeRequired)
                        btnCheckAccount.Invoke(new MethodInvoker(delegate
                        {
                            AddToListBox("[ " + DateTime.Now + " ] => [ Account Checker => PROCESS COMPLETED ]");
                        }));
                }
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddToListBox("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        }


        #region LD_CheckAccountMultiThread
        private void LD_CheckAccountMultiThread(object obj_chk)
        {
            try
            {
                string Account = string.Empty;
                Array ArrayChkAcc = new object[3];
                ArrayChkAcc = (Array)obj_chk;
                Account = (string)ArrayChkAcc.GetValue(0);
                      
                CheckLDAccount(Account);

            }
            catch
            {
            }
        }

        #endregion

        string _Email = string.Empty;
        public static int Counter = 0;
        #region CheckLDAccount
        public void CheckLDAccount(string item)
        {
            GlobusHttpHelper objGlobusHttpHelper = new GlobusHttpHelper();
           

            try
            {
                LinkedinLogin Login = new LinkedinLogin();

                _Email = item;

           
                string account = item;
                string[] AccArr = account.Split(':');
                if (AccArr.Count() > 1)
                {
                    Login.accountUser = account.Split(':')[0];
                    Login.accountPass = account.Split(':')[1];
                    //Username = account.Split(':')[0];
                    //Password = account.Split(':')[1];
                    int DataCount = account.Split(':').Length;

                    if (DataCount == 4)
                    {
                        Login.proxyAddress = account.Split(':')[2];
                        Login.proxyPort = account.Split(':')[3];
                        //proxyAddress = account.Split(':')[2];
                        //proxyPort = account.Split(':')[3];
                    }
                    else if (DataCount == 6)
                    {
                        Login.proxyAddress = account.Split(':')[2];
                        Login.proxyPort = account.Split(':')[3];
                        Login.proxyUserName = account.Split(':')[4];
                        Login.proxyPassword = account.Split(':')[5];

                        //proxyAddress = account.Split(':')[2];
                        //proxyPort = account.Split(':')[3];
                        //proxyUserName = account.Split(':')[4];
                        //proxyPassword = account.Split(':')[5];
                    }
                }
                Login.logger.addToLogger += new EventHandler(logger_addToLogger);
                Login.LoginHttpHelper_Checker(ref objGlobusHttpHelper);
                                              
            }
            catch
            {

            }
            finally
            {
                Counter++;
                if (CheckAccount.TotalEmails <= Counter)
                {
                    AddToListBox("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED For Account Checking ]");
                    AddToListBox("------------------------------------------------------------------------------------------------------------------------------------");
                }
            }
        }
        #endregion

    }
}
