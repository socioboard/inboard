using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace BaseLib
{
   public class CheckAccount
    {
        #region Variable declarations
        GlobusHttpHelper HttpHelper = new GlobusHttpHelper();
        public Events loggerEvent_AccountChecker = new Events();
        string _Email = string.Empty;
        public static int Counter = 0;
        public static int TotalEmails = 0;
        #endregion

        #region CheckAccounts
        public CheckAccount()
        {
        } 
        #endregion

        #region CheckLDAccount
        public void CheckLDAccount(string item)
        {
            GlobusHttpHelper objGlobusHttpHelper = new GlobusHttpHelper();

            try
            {
                _Email = item;

                string Username = string.Empty;
                string Password = string.Empty;
                string proxyAddress = string.Empty;
                string proxyPort = string.Empty;
                string proxyUserName = string.Empty;
                string proxyPassword = string.Empty;

                string account = item;
                string[] AccArr = account.Split(':');
                if (AccArr.Count() > 1)
                {
                    Username = account.Split(':')[0];
                    Password = account.Split(':')[1];
                    int DataCount = account.Split(':').Length;

                    if (DataCount == 4)
                    {
                        proxyAddress = account.Split(':')[2];
                        proxyPort = account.Split(':')[3];
                    }
                    else if (DataCount == 6)
                    {
                        proxyAddress = account.Split(':')[2];
                        proxyPort = account.Split(':')[3];
                        proxyUserName = account.Split(':')[4];
                        proxyPassword = account.Split(':')[5];
                    }
                }

                LinkedinLogin Login = new LinkedinLogin();
                //For Sign Out +9+
                Login.LoginHttpHelper(ref objGlobusHttpHelper);

                //bool isLogin = LoginAccount(Username, Password, proxyAddress, proxyPort, proxyUserName, proxyPassword);
            }
            catch
            {

            }
            finally
            {
                Counter++;
                if (TotalEmails <= Counter)
                {
                    Log("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED For Account Checking ]");
                    Log("------------------------------------------------------------------------------------------------------------------------------------");
                }
            }
        } 
        #endregion

       
        #region LoginAccount
        private bool LoginAccount(string Username, string Password, string proxyAddress, string proxyPort, string proxyUserName, string proxyPassword)
        {
            bool IsLoggedIn = false;
            string _Username = string.Empty;
            string _Password = string.Empty;
            string _ProxyAddress = string.Empty;
            string _ProxyPort = string.Empty;
            string _ProxyUsername = string.Empty;
            string _ProxyPassword = string.Empty;

            try
            {
                _Username = Username;
                _Password = Password;
                _ProxyAddress = proxyAddress;
                _ProxyPort = proxyPort;
                _ProxyUsername = proxyUserName;
                _ProxyPassword = proxyPassword;

                if (string.IsNullOrEmpty(proxyPort) && !NumberHelper.ValidateNumber(proxyPort))
                {
                    _ProxyPort = "80";
                }

                Log("[ " + DateTime.Now + " ] => [ Logging in with : " + _Username + " ]");


                string Url = "https://www.linkedin.com/";
               
                string pageSrcLogin = HttpHelper.getHtmlfromUrlProxy(new Uri(Url), _ProxyAddress, Convert.ToInt32(_ProxyPort), _ProxyUsername, _ProxyPassword);


                string postdata = string.Empty;
                string postUrl = string.Empty;
                string ResLogin = string.Empty;
                string csrfToken = string.Empty;
                string sourceAlias = string.Empty;
                string regCsrfParam = string.Empty;

                if (pageSrcLogin.Contains("csrfToken"))
                {
                    try
                    {
                        int startIndex = pageSrcLogin.IndexOf("name=\"csrfToken\"");
                        string start = pageSrcLogin.Substring(startIndex).Replace("name=\"csrfToken\"", "");
                        int endIndex = start.IndexOf("\" ");
                        string end = start.Substring(0, endIndex).Replace("value=\"", "").Trim();
                        csrfToken = end;
                        csrfToken = csrfToken.Replace("ajax:", "");
                    }
                    catch (Exception ex)
                    {

                    }

                }

                try
                {
                    if (csrfToken.Contains("&"))
                    {
                        string[] Arr = csrfToken.Split('&');
                        csrfToken = Arr[0].Replace("\"", string.Empty);

                    }

                }
                catch { }

                SearchCriteria.CsrToken = csrfToken;
                if (pageSrcLogin.Contains("sourceAlias"))
                {
                    try
                    {
                        sourceAlias = pageSrcLogin.Substring(pageSrcLogin.IndexOf("sourceAlias"), 100);
                        string[] Arr = sourceAlias.Split('"');
                        sourceAlias = Arr[2].Replace("\\", string.Empty);
                    }
                    catch { }
                }

                try
                {
                    int SourceAliasStart = pageSrcLogin.IndexOf("regCsrfParam");
                    if (SourceAliasStart > 0)
                    {
                        try
                        {

                            regCsrfParam = pageSrcLogin.Substring(pageSrcLogin.IndexOf("regCsrfParam"), 100);
                            string[] Arr = regCsrfParam.Split('"');
                            regCsrfParam = Arr[2].Replace(@"\", string.Empty).Replace("//", string.Empty);
                        }
                        catch
                        {
                        }
                    }

                }
                catch { }


                //postUrl = "https://www.linkedin.com/uas/login-submit";
                //postdata = "session_key=" + Uri.EscapeDataString(_Username) + "&session_password=" + Uri.EscapeDataString(_Password) + "&source_app=&trk=guest_home_login&session_redirect=&csrfToken=" + csrfToken + "&sourceAlias=" + sourceAlias;
                //ResLogin = HttpHelper.postFormData(new Uri(postUrl), postdata);

                postUrl = "https://www.linkedin.com/uas/login-submit";
                postdata = "isJsEnabled=true&source_app=&tryCount=&session_key=" + Uri.EscapeDataString(_Username) + "&session_password=" + Uri.EscapeDataString(_Password) + "&signin=Sign%20In&session_redirect=&loginCsrfParam=" + regCsrfParam + "&csrfToken=" + csrfToken + "&sourceAlias=" + sourceAlias;

                try
                {
                    ResLogin = HttpHelper.postFormDataProxy(new Uri(postUrl), postdata, _ProxyPassword, Convert.ToInt32(_ProxyPort), _ProxyUsername, _ProxyPassword);//HttpHelper.postFormDataRef(new Uri(postUrl), postdata, "http://www.linkedin.com/uas/login?goback=&trk=hb_signin", "", "");
                }
                catch { }

                                
                if (ResLogin.Contains("Your LinkedIn account has been temporarily restricted") || ResLogin.Contains("Change your password") || ResLogin.Contains("Please confirm your email address"))
                {
                    if (ResLogin.Contains("Your LinkedIn account has been temporarily restricted"))
                    {
                        try
                        {
                            Log("[ " + DateTime.Now + " ] => [ Your LinkedIn account has been temporarily restricted : " + _Username + " ]");

                            GlobusFileHelper.AppendStringToTextfileNewLine(_Username, Globals.Path_TemporarilyRestrictedAccount_AccountChecker);
                        }
                        catch
                        {
                        }
                    }
                    if (ResLogin.Contains("Change your password"))
                    {
                        try
                        {
                            Log("[ " + DateTime.Now + " ] => [ Change your password : " + _Email + " ]");

                            GlobusFileHelper.AppendStringToTextfileNewLine(_Username, Globals.Path_ChangeYourPassword_AccountChecker);

                        }
                        catch
                        {
                        }
                    }
                    if (ResLogin.Contains("Please confirm your email address"))
                    {
                        try
                        {
                            Log("[ " + DateTime.Now + " ] => [ Please confirm your email address : " + _Username + " ]");

                            GlobusFileHelper.AppendStringToTextfileNewLine(_Username, Globals.Path_ConfirmYourEmailAddress_AccountChecker);

                        }
                        catch
                        {
                        }
                    }
                }

                if (ResLogin.Contains("The email address or password you provided does not match our records"))
                {
                    try
                    {
                        Log("[ " + DateTime.Now + " ] => [ The email address or password you provided does not match our records : " + _Username + " ]");

                        GlobusFileHelper.AppendStringToTextfileNewLine(_Username, Globals.Path_EmailAddressOrPasswordDoesNotMatch_AccountChecker);

                    }
                    catch
                    {
                    }

                }

                if (ResLogin.Contains("Security Verification"))
                {
                    try
                    {
                        Log("[ " + DateTime.Now + " ] => [ Security Verification : " + _Username + " ]");

                        GlobusFileHelper.AppendStringToTextfileNewLine(_Username, Globals.Path_SecurityVerification_AccountChecker);

                    }
                    catch
                    {
                    }

                }

                if (ResLogin.Contains("One or more of your email addresses needs confirmation"))
                {
                    try
                    {
                        Log("[ " + DateTime.Now + " ] => [ One or more of your email addresses needs confirmation : " + _Username + " ]");

                        GlobusFileHelper.AppendStringToTextfileNewLine(_Username, Globals.Path_ConfirmYourEmailAddress_AccountChecker);

                    }
                    catch
                    {
                    }

                }

                if (ResLogin.Contains("Sign Out") && !ResLogin.Contains("Your LinkedIn account has been temporarily restricted") && !ResLogin.Contains("Please confirm your email address"))
                {
                   IsLoggedIn = true;
                }

                string CheckedAccount = _Username + ":" +  _Password;

                if (IsLoggedIn)
                {
                    try
                    {
                        
                        Log("[ " + DateTime.Now + " ] => [ Successfully Logged In with : " + _Username + " ]");
                        GlobusFileHelper.AppendStringToTextfileNewLine(CheckedAccount, Globals.Path_WorkingAccount_AccountChecker);
                    }
                    catch
                    {
                    }
                }
                else
                {
                    try
                    {
                        Log("[ " + DateTime.Now + " ] => [ Failed to Login with : " + _Username + " ]");
                        GlobusFileHelper.AppendStringToTextfileNewLine(CheckedAccount, Globals.Path_NonWorkingAccount_AccountChecker);
                    }
                    catch
                    {
                    }
                }

                
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("Module Name >>> AccountChecker, UserName >>> " + _Email + " , ex.Message >>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + " , ex.StackTrace >>> " + ex.StackTrace + "  , Date Time >>> " + DateTime.Now, Globals.Path_ErrorFile_AccountChecker);
            }           
            
            return IsLoggedIn;          
        } 
        #endregion

        #region CheckLDEmail
        public void CheckLDEmail(string item)
        {
            try
            {
                _Email = item;

                string Username = string.Empty;
                string Password = string.Empty;
                string proxyAddress = string.Empty;
                string proxyPort = string.Empty;
                string proxyUserName = string.Empty;
                string proxyPassword = string.Empty;

                string account = item;
                string[] AccArr = account.Split(':');
                if (AccArr.Count() > 1)
                {
                    Username = account.Split(':')[0];
                    Password = account.Split(':')[1];
                    int DataCount = account.Split(':').Length;

                    if (DataCount == 4)
                    {
                        proxyAddress = account.Split(':')[2];
                        proxyPort = account.Split(':')[3];
                    }
                    else if (DataCount == 6)
                    {
                        proxyAddress = account.Split(':')[2];
                        proxyPort = account.Split(':')[3];
                        proxyUserName = account.Split(':')[4];
                        proxyPassword = account.Split(':')[5];
                    }
                }

                CheckEmail(Username, Password, proxyAddress, proxyPort, proxyUserName, proxyPassword);
            }
            catch
            {
            }
        } 
        #endregion

        #region CheckEmail
        private void CheckEmail(string Username, string Password, string proxyAddress, string proxyPort, string proxyUserName, string proxyPassword)
        {
            string _Username = string.Empty;
            try
            {
                string _Password = string.Empty;
                string _ProxyAddress = string.Empty;
                string _ProxyPort = string.Empty;
                string _ProxyUsername = string.Empty;
                string _ProxyPassword = string.Empty;

                _Username = Username;
                _Password = Password;
                _ProxyAddress = proxyAddress;
                _ProxyPort = proxyPort;
                _ProxyUsername = proxyUserName;
                _ProxyPassword = proxyPassword;

                if (_Username.Length < 1)
                {
                    return;
                }

                Log("[ " + DateTime.Now + " ] => [ Starting To Check Email With UserName : " + _Username + " ]");

                if (string.IsNullOrEmpty(proxyPort) && !NumberHelper.ValidateNumber(proxyPort))
                {
                    _ProxyPort = "80";
                }

                string Url = "https://www.linkedin.com/";
                string pageSrcLogin = HttpHelper.getHtmlfromUrlProxy(new Uri(Url), _ProxyAddress, Convert.ToInt32(_ProxyPort), _ProxyUsername, _ProxyPassword);

                string postdata = string.Empty;
                string postUrl = string.Empty;
                string ResLogin = string.Empty;
                string csrfToken = string.Empty;
                string sourceAlias = string.Empty;

                if (pageSrcLogin.Contains("csrfToken"))
                {
                    try
                    {
                        csrfToken = pageSrcLogin.Substring(pageSrcLogin.IndexOf("csrfToken"), 100);
                        string[] Arr = csrfToken.Split('"');
                        csrfToken = Arr[2].Replace("\\", string.Empty);
                    }
                    catch
                    {
                        try
                        {
                            csrfToken = pageSrcLogin.Substring(pageSrcLogin.IndexOf("csrfToken"), 100);
                            if (csrfToken.Contains("&"))
                            {
                                string[] Arr = csrfToken.Split('&');
                                csrfToken = Arr[0].Replace("\\", string.Empty).Replace("csrfToken=",string.Empty);
                            }
                            else if (csrfToken.Contains(","))
                            {
                                string[] Arr = csrfToken.Split(',');
                                csrfToken = Arr[0].Replace("\\", string.Empty);
                            }

                        }
                        catch { }

                    }

                }
                SearchCriteria.CsrToken = csrfToken;
                if (pageSrcLogin.Contains("sourceAlias"))
                {
                    try
                    {
                        sourceAlias = pageSrcLogin.Substring(pageSrcLogin.IndexOf("sourceAlias"), 100);
                        string[] Arr = sourceAlias.Split('"');
                        sourceAlias = Arr[2].Replace("\\", string.Empty);
                    }
                    catch { }
                }

                string getReqForCheckEmail = HttpHelper.getHtmlfromUrl1(new Uri("https://www.linkedin.com/uas/request-password-reset?session_redirect="));
                postUrl = "https://www.linkedin.com/uas/request-password-reset-submit";

                postdata = "email=" + Uri.EscapeDataString(_Username) + "&request=Submit+Address&sessionRedirect=&csrfToken=" + csrfToken + "&sourceAlias=" + sourceAlias + "";
                ResLogin = HttpHelper.postFormData(new Uri(postUrl), postdata);

                if (ResLogin.Contains("We couldn't find a LinkedIn account associated with " + _Username))
                {
                    try
                    {
                        Log("[ " + DateTime.Now + " ] => [ We couldn't find a LinkedIn account associated with " + _Username + " ]");
                        GlobusFileHelper.AppendStringToTextfileNewLine(_Username, Globals.Path_NonExistingEmail_EmailChecker);
                    }
                    catch
                    {
                    }
                }

                else if (ResLogin.Contains("Please check your email"))
                {
                    try
                    {
                        Log("[ " + DateTime.Now + " ] => [ Please check your email with : " + _Username + " ]");
                        GlobusFileHelper.AppendStringToTextfileNewLine(_Username, Globals.Path_ExistingEmail_EmailChecker);
                    }
                    catch
                    {
                    }
                }
                else if (ResLogin.Contains("Security Verification"))
                {
                    try
                    {
                        Log("[ " + DateTime.Now + " ] => [ Security Verification : " + _Username + " ]");

                        GlobusFileHelper.AppendStringToTextfileNewLine(_Username, Globals.Path_SecurityVerification_EmailChecker);

                    }
                    catch
                    {
                    }

                }
                else
                {
                    Log("[ " + DateTime.Now + " ] => [ There is some problem with : " + _Username + " ]");
                    GlobusFileHelper.AppendStringToTextfileNewLine(_Username, Globals.Path_OtherProblem_EmailChecker);
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("Module Name >>> EmailChecker, UserName >>> " + _Username + " , ex.Message >>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + " , ex.StackTrace >>> " + ex.StackTrace + "  , Date Time >>> " + DateTime.Now, Globals.Path_ErrorFile_EmailChecker);

            }

            Log("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED With UserName : " + _Username + " ]");
        } 
        #endregion

        #region Log
        private void Log(string log)
        {
            try
            {
                EventsArgs loggerEventsArgs = new EventsArgs(log);
                loggerEvent_AccountChecker.LogText(loggerEventsArgs);
            }
            catch (Exception ex)
            {

            }

        } 
        #endregion

        public static string userIdForUpdate = string.Empty;
        public static string passwordForUpdate = string.Empty;
        public void UpdatePassword(string userid, string pass)
        {
            userIdForUpdate = userid.ToString();
            passwordForUpdate = pass.ToString();
        }
    }
}
