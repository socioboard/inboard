using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Data;
using BaseLib;


namespace ComposeMessage
{
    public class ComposeMessage
    {
        #region Global declaration
        public string accountUser = string.Empty;
        public string accountPass = string.Empty;
        public string proxyAddress = string.Empty;
        public string proxyPort = string.Empty;
        public string proxyUserName = string.Empty;
        public string proxyPassword = string.Empty;
        public static Dictionary<string, string> SlectedContacts1 = new Dictionary<string, string>();
        public static Dictionary<string, string> SlectedContacts2 = new Dictionary<string, string>(); 
        #endregion

        #region IsAllAccounts
        public static bool IsAllAccounts
        {
            get;
            set;
        } 
        #endregion

        #region NoOfFriends
        public static int NoOfFriends
        {
            get;
            set;
        } 
        #endregion

        #region ComposeMessage
        public ComposeMessage()
        {
        } 
        #endregion

        #region ComposeMessage
        public ComposeMessage(string UserName, string Password, string ProxyAddress, string ProxyPort, string ProxyUserName, string ProxyPassword)
        {
            this.accountUser = UserName;
            this.accountPass = Password;
            this.proxyAddress = ProxyAddress;
            this.proxyPort = ProxyPort;
            this.proxyUserName = ProxyUserName;
            this.proxyPassword = ProxyPassword;

        } 
        #endregion

        #region Events logger
         public Events logger = new Events();

         private void Log(string log)
         {
             try
             {
                 EventsArgs loggerEventsArgs = new EventsArgs(log);
                 logger.LogText(loggerEventsArgs);
             }
             catch (Exception ex)
             {

             }
         }
         #endregion

        #region PostFinalMsg
         public void PostFinalMsg(ref GlobusHttpHelper HttpHelper, Dictionary<string, string> SlectedContacts,string userText, List<string> GrpMemSubjectlist, string msg, string body, string UserEmail, string FromemailId, string FromEmailNam, bool msg_spintaxt, int mindelay, int maxdelay, bool preventMsgSameUser, bool preventMsgGlobalUser)
          {
              ComposeMsgDbManager objComposeMsgDbMgr = new ComposeMsgDbManager();

             try
             {
                 string postdata = string.Empty;
                 string postUrl = string.Empty;

                 string ResLogin = string.Empty;
                 string csrfToken = string.Empty;
                 string sourceAlias = string.Empty;

                 string ReturnString = string.Empty;
                 string PostMsgSubject = string.Empty;
                 string PostMsgBody = string.Empty;
                 string FString = string.Empty;
                 string Nstring = string.Empty;
                 string connId = string.Empty;
                 string FullName = string.Empty;
                 string ToMsg = string.Empty;
                 string ToCd = string.Empty;

                 try
                 {
                     DataSet ds_bList = new DataSet();
                     DataSet ds = new DataSet();
                     string MessageText = string.Empty;
                     string PostedMessage = string.Empty;
                    
                     string pageSource = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/home?trk=hb_tab_home_top"));
                     if (pageSource.Contains("csrfToken"))
                     {
                         csrfToken = pageSource.Substring(pageSource.IndexOf("csrfToken"), 50);
                         string[] Arr = csrfToken.Split('<');
                         csrfToken = Arr[0];
                         csrfToken = csrfToken.Replace("csrfToken", "").Replace("\"", string.Empty).Replace("value", string.Empty).Replace("cs", string.Empty).Replace("id", string.Empty).Replace("=", string.Empty).Replace("\n", string.Empty).Replace(">", string.Empty).Replace("<script typ", string.Empty);
                         csrfToken = csrfToken.Trim();
                     }

                     if (pageSource.Contains("sourceAlias"))
                     {
                         sourceAlias = pageSource.Substring(pageSource.IndexOf("sourceAlias"), 100);
                         string[] Arr = sourceAlias.Split('"');
                         sourceAlias = Arr[2];
                     }

                     if (IsAllAccounts)
                     {
                         try
                         {
                             ClsLinkedinMain obj_ClsLinkedinMain = new ClsLinkedinMain();

                             Dictionary<string, string> dTotalFriends = obj_ClsLinkedinMain.PostAddMembers(ref HttpHelper, UserEmail);

                             Log("[ " + DateTime.Now + " ] => [ No. Of Friends = " + dTotalFriends.Count + " With Username >>> " + UserEmail + " ]");

                             if (dTotalFriends.Count > 0)
                             {


                                 PostMessageToAllAccounts(ref HttpHelper,SlectedContacts, dTotalFriends, msg, body, UserEmail, FromemailId, FromEmailNam, mindelay, mindelay);

                                 int count = SlectedContacts2.Count();

                                 if (count > 0)
                                 {
                                     do
                                     {

                                         PostMessageToAllAccounts(ref HttpHelper, SlectedContacts2, dTotalFriends, msg, body, UserEmail, FromemailId, FromEmailNam, mindelay, mindelay);
                                         count = SlectedContacts2.Count();

                                     } while (count > 0);
                                 }
                             }
                             Log("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED With Username >>> " + UserEmail + " ]");
                             Log("-----------------------------------------------------------------------------------------------------------------------------------");

                             return;
                         }
                         catch (Exception ex)
                         {
                         }
                     }

                  
                             string ContactName = string.Empty;
                             int counter = 1;
                             Dictionary<string, string> SlectedSentContacts = new Dictionary<string, string>();
                             Nstring = string.Empty;
                             ContactName = string.Empty;
                             string ProfileUrl = string.Empty;
                             string ProfileID = string.Empty;
                             foreach (KeyValuePair<string, string> itemChecked in SlectedContacts)
                             {
                                 if (Globals.groupStatusString == "API")
                                 {
                                     ProfileUrl = "https://www.linkedin.com/contacts/view?id=" + itemChecked.Key + "";
                                     string profilePageSource = HttpHelper.getHtmlfromUrl1(new Uri(ProfileUrl));
                                     ProfileID = Utils.getBetween(profilePageSource, "id=", ",").Replace("\"", "");
                                 }
                                 else
                                 {
                                     ProfileUrl = "https://www.linkedin.com/profile/view?id=" + itemChecked.Key + "";

                                     ProfileID = itemChecked.Key;
                                 }

                                 if (counter < 50)
                                 {
                                     SlectedSentContacts.Add(itemChecked.Key, itemChecked.Value);

                                     try
                                     {
                                         string Querystring = "Select ProfileID From tb_BlackListAccount Where ProfileID ='" + ProfileID+ "'";
                                         //string Querystring = "Select ProfileID From tb_BlackListAccount Where ProfileID ='" + itemChecked.Key +"'";
                                         ds_bList = DataBaseHandler.SelectQuery(Querystring, "tb_BlackListAccount");
                                     }
                                     catch { }

                                     if (ds_bList.Tables.Count > 0 && ds_bList.Tables[0].Rows.Count > 0)
                                     {
                                         Log("[ " + DateTime.Now + " ] => [ User: " + itemChecked.Value.Replace(":", string.Empty).Trim() + " is Added BlackListed List For Send Messages Pls Check ]");

                                     }
                                     else
                                     {

                                         try
                                         {

                                             string FName = string.Empty;
                                             string Lname = string.Empty;

                                             try
                                             {
                                                 FName = itemChecked.Value.Split(' ')[0];
                                                 Lname = itemChecked.Value.Split(' ')[1];
                                             }
                                             catch { }

                                             FullName = FName + " " + Lname;
                                             try
                                             {
                                                 //ContactName = ContactName + "  :  " + FullName;
                                                 ContactName = "  :  " + FullName;
                                             }
                                             catch { }

                                             if (ToMsg == string.Empty)
                                             {
                                                 //ToMsg += FullName;
                                                 ToMsg = FullName;
                                             }
                                             else
                                             {
                                                 //ToMsg += ";" + FullName;
                                                 ToMsg = ";" + FullName;
                                             }

                                             Log("[ " + DateTime.Now + " ] => [ Adding Contact " + FullName + " ]");

                                             //ToCd = itemChecked.Key;  //for client sudi
                                             ToCd = ProfileID;
                                             List<string> AddAllString = new List<string>();

                                             if (FString == string.Empty)
                                             {
                                                 string CompString = "{" + "\"" + "_" + ToCd.Trim() + "\"" + ":" + "{" + "\"" + "memberId" + "\"" + ":" + "\"" + ToCd.Trim() + "\"" + "," + "\"" + "firstName" + "\"" + ":" + "\"" + FName + "\"" + "," + "\"" + "lastName" + "\"" + ":" + "\"" + Lname + "\"" + "}";
                                                 FString = CompString;
                                             }
                                             else
                                             {
                                                 string CompString = "\"" + "_" + ToCd.Trim() + "\"" + ":" + "{" + "\"" + "memberId" + "\"" + ":" + "\"" + ToCd.Trim() + "\"" + "," + "\"" + "firstName" + "\"" + ":" + "\"" + FName + "\"" + "," + "\"" + "lastName" + "\"" + ":" + "\"" + Lname + "\"" + "}";
                                                 FString = CompString;
                                             }

                                             if (Nstring == string.Empty)
                                             {
                                                 Nstring = FString;
                                                 connId = ToCd;
                                             }
                                             else
                                             {
                                                 //Nstring += "," + FString;
                                                 Nstring = FString;
                                                 connId = ToCd;
                                                 //connId += " " + ToCd;
                                             }


                                         }
                                         catch { }
                                         //Nstring += "}";
                                     }

                                 }
                                 else
                                 {
                                     try
                                     {
                                         SlectedContacts1.Add(itemChecked.Key, itemChecked.Value);
                                     }
                                     catch
                                     {

                                     }
                                 }
                                 counter++;


                                 //}

                                 if (SlectedContacts1.Count != 0)
                                 {
                                     foreach (KeyValuePair<string, string> itemremove in SlectedSentContacts)
                                     {
                                         SlectedContacts1.Remove(itemremove.Key);
                                     }
                                 }

                                 if (msg_spintaxt == true)
                                 {
                                     try
                                     {
                                         body = GlobusSpinHelper.spinLargeText(new Random(), userText);
                                         msg = GrpMemSubjectlist[RandomNumberGenerator.GenerateRandom(0, GrpMemSubjectlist.Count - 1)];
                                     }
                                     catch
                                     {
                                     }

                                     body = body.Replace("<Insert Name>", FullName);
                                     body = body.Replace("<Insert From Email>", FromEmailNam);

                                 }



                                 if (preventMsgSameUser)
                                 {
                                     try
                                     {
                                         string Querystring = "Select MsgFrom,MsgToId,MsgTo,MsgSubject,MsgBody From tb_ManageComposeMsg Where MsgFrom ='" + UserEmail + "' and MsgBody = '" + body + "' and MsgToId = " + connId + "";
                                         ds = DataBaseHandler.SelectQuery(Querystring, "tb_ManageComposeMsg");
                                     }
                                     catch { }
                                 }


                                 if (preventMsgGlobalUser)
                                 {
                                     try
                                     {
                                         string Querystring = "Select MsgFrom,MsgToId,MsgTo,MsgSubject,MsgBody From tb_ManageComposeMsg Where MsgToId = " + connId + "";
                                         ds = DataBaseHandler.SelectQuery(Querystring, "tb_ManageComposeMsg");
                                     }
                                     catch { }
                                 }

                                 try
                                 {
                                     string PostMessage = string.Empty;
                                     string ResponseStatusMsg = string.Empty;

                                     Log("[ " + DateTime.Now + " ] => [ Message Sending Process Running.. ]");


                                     if (ds.Tables.Count > 0)
                                     {
                                         if (ds.Tables[0].Rows.Count > 0)
                                         {

                                             PostMessage = "";
                                             ResponseStatusMsg = "Already Sent";

                                         }
                                         else
                                         {

                                             PostMessage = "senderEmail=" + FromemailId.Trim() + "&ccInput=&subject=" + Uri.EscapeDataString(msg.ToString()) + "&body=" + Uri.EscapeDataString(body.ToString()) + "&isReply=&isForward=&itemId=&recipients=" + Uri.EscapeUriString(connId) + "&recipientNames=" + Uri.EscapeUriString(Nstring) + "&groupId=&csrfToken=" + csrfToken + "&sourceAlias=" + sourceAlias + "&submit=Send+Message";
                                             //ResponseStatusMsg = HttpHelper.postFormData(new Uri("http://www.linkedin.com/inbox/mailbox/message/send"), PostMessage);

                                             ResponseStatusMsg = HttpHelper.postFormDataRef(new Uri("http://www.linkedin.com/inbox/mailbox/message/send"), PostMessage, "https://www.linkedin.com/inbox/", "", "", "XMLHttpRequest", "https://www.linkedin.com","1");   //ahmed sudi client changes
                                         }
                                     }
                                     else
                                     {

                                         PostMessage = "senderEmail=" + FromemailId.Trim() + "&ccInput=&subject=" + Uri.EscapeDataString(msg.ToString()) + "&body=" + Uri.EscapeDataString(body.ToString()) + "&isReply=&isForward=&itemId=&recipients=" + Uri.EscapeUriString(connId) + "&recipientNames=" + Uri.EscapeUriString(Nstring) + "&groupId=&csrfToken=" + csrfToken + "&sourceAlias=" + sourceAlias + "&submit=Send+Message";
                                         //ResponseStatusMsg = HttpHelper.postFormData(new Uri("http://www.linkedin.com/inbox/mailbox/message/send"), PostMessage);
                                         ResponseStatusMsg = HttpHelper.postFormDataRef(new Uri("http://www.linkedin.com/inbox/mailbox/message/send"), PostMessage, "https://www.linkedin.com/inbox/", "", "", "XMLHttpRequest", "https://www.linkedin.com", "1");   //ahmed sudi client changes
                                     }

                                     if (ResponseStatusMsg.Contains("Your message was successfully sent.") || ResponseStatusMsg.Contains("Tu mensaje ha sido enviado con éxito."))
                                     {
                                         foreach (var item in SlectedSentContacts)
                                         {
                                             try
                                             {
                                                 string Querystring = "Select ProfileID From tb_BlackListAccount Where ProfileID ='" + item.Key + "'";
                                                 ds_bList = DataBaseHandler.SelectQuery(Querystring, "tb_BlackListAccount");
                                             }
                                             catch { }

                                             if (ds_bList.Tables.Count > 0 && ds_bList.Tables[0].Rows.Count > 0)
                                             {

                                                 Log("[ " + DateTime.Now + " ] => [ User: " + item.Value.Replace(":", string.Empty).Trim() + " is Added BlackListed List For Send Messages Pls Check ]");
                                             }
                                             else
                                             {
                                                 Log("[ " + DateTime.Now + " ] => [ Subject Posted : " + msg + " ]");
                                                 Log("[ " + DateTime.Now + " ] => [ Body Text Posted : " + body.ToString() + " ]");
                                                 Log("[ " + DateTime.Now + " ] => [ Message Posted To Account: " + item.Value + " With Username >>> " + UserEmail + " ]");

                                                 ReturnString = "Your message was successfully sent.";
                                                 string bdy = string.Empty;

                                                 try
                                                 {
                                                     bdy = body.ToString().Replace("\r", string.Empty).Replace("\n", " ").Replace(",", " ");
                                                 }
                                                 catch { }
                                                 if (string.IsNullOrEmpty(bdy))
                                                 {
                                                     bdy = body.ToString().Replace(",", ":");
                                                 }


                                                 string CSVHeader = "UserName" + "," + "Subject" + "," + "Body Text" + "," + "ContactName" + "," + "ProfileUrl";
                                                 string CSV_Content = UserEmail + "," + msg + "," + bdy + "," + ContactName.Replace(":", string.Empty).Trim() + "," + ProfileUrl;
                                                 CSVUtilities.ExportDataCSVFile(CSVHeader, CSV_Content, Globals.path_ComposeMessageSent);

                                                 try
                                                 {
                                                     objComposeMsgDbMgr.InsertComposeMsgData(UserEmail, Convert.ToInt32(connId), ContactName, msg, bdy);
                                                 }
                                                 catch { }
                                             }
                                         }

                                     }

                                     else if (ResponseStatusMsg.Contains("There was an unexpected problem that prevented us from completing your request."))
                                     {
                                         Log("[ " + DateTime.Now + " ] => [ Error In Message Posting ]");
                                         GlobusFileHelper.AppendStringToTextfileNewLine("Error In Message Posting", Globals.path_ComposeMessage);
                                     }
                                     else if ((ResponseStatusMsg.Contains("Already Sent")) || (ResponseStatusMsg.Contains("Ya ha sido enviada")))
                                     {
                                         string bdy = string.Empty;
                                         try
                                         {
                                             bdy = body.ToString().Replace("\r", string.Empty).Replace("\n", " ").Replace(",", " ");
                                         }
                                         catch { }

                                         if (string.IsNullOrEmpty(bdy))
                                         {
                                             bdy = bdy.ToString().Replace(",", ":");
                                         }
                                         string CSVHeader = "UserName" + "," + "Subject" + "," + "Body Text" + "," + "ContactName";
                                         string CSV_Content = UserEmail + "," + msg + "," + bdy.ToString() + "," + ContactName;
                                         CSVUtilities.ExportDataCSVFile(CSVHeader, CSV_Content, Globals.path_MessageAlreadySentComposeMgs);

                                         Log("[ " + DateTime.Now + " ] => [ Message Not Posted To Account: " + ContactName.Replace(":", string.Empty) + " because it has sent the same message already]");
                                     }
                                     else
                                     {
                                         Log("[ " + DateTime.Now + " ] => [ Failed In Message Posting ]");
                                         GlobusFileHelper.AppendStringToTextfileNewLine("Failed In Message Posting", Globals.path_ComposeMessage);
                                     }

                                     //if (SlectedContacts1.Count != 0) //client ahmed sudi
                                     {
                                         int delay = RandomNumberGenerator.GenerateRandom(mindelay, maxdelay);
                                         Log("[ " + DateTime.Now + " ] => [ " + "Delay for : " + delay + " Seconds ]");
                                         Thread.Sleep(delay * 1000);
                                     }

                                 }
                                 catch (Exception ex)
                                 {
                                     GlobusFileHelper.AppendStringToTextfileNewLine(" Error:" + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.path_ComposeMessage);
                                 }
                             }
                             
                 }
                 catch (Exception ex)
                 {
                     GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error -->  PostFinalMsg() --> 1 --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                     GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> PostFinalMsg() --> 1 --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinComposeMessageErrorLogs);
                 }

             }
             catch (Exception ex)
             {
                 GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error -->  PostFinalMsg() --> 2 --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                 GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> PostFinalMsg() --> 2 --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinComposeMessageErrorLogs);
             }
             
         }
         #endregion

        #region PostFinalMsg_1By1
         //public void PostFinalMsg_1By1(ref GlobusHttpHelper HttpHelper, Dictionary<string, string> SlectedContacts, string msg, string body, string UserEmail, string FromemailId, string FromEmailNam,int mindelay,int maxdelay)
         public void PostFinalMsg_1By1(ref GlobusHttpHelper HttpHelper, Dictionary<string, string> SlectedContacts, List<string> messagesubject, string userText, string msg, string body, string UserEmail, string FromemailId, string FromEmailNam, bool msg_spintaxt, int mindelay, int maxdelay, bool preventMsgSameUser, bool preventMsgGlobalUser)
         {
             try
             {
                 string postdata = string.Empty;
                 string postUrl = string.Empty;

                 string ResLogin = string.Empty;
                 string csrfToken = string.Empty;
                 string sourceAlias = string.Empty;

                 string ReturnString = string.Empty;
                 string PostMsgSubject = string.Empty;
                 string PostMsgBody = string.Empty;
                 string FString = string.Empty;
                 //string Nstring = string.Empty;
                 //string connId = string.Empty;
                 string FullName = string.Empty;
                 string ToMsg = string.Empty;

                 try
                 {
                     string MessageText = string.Empty;
                     string PostedMessage = string.Empty;

                     string pageSource = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/home?trk=hb_tab_home_top"));
                     if (pageSource.Contains("csrfToken"))
                     {
                         csrfToken = pageSource.Substring(pageSource.IndexOf("csrfToken"), 50);
                         string[] Arr = csrfToken.Split('>');
                         csrfToken = Arr[0];
                         csrfToken = csrfToken.Replace("csrfToken", "").Replace("\"", string.Empty).Replace("value", string.Empty).Replace("cs", string.Empty).Replace("id", string.Empty).Replace("=", string.Empty);
                         csrfToken = csrfToken.Trim();
                     }

                     if (pageSource.Contains("sourceAlias"))
                     {
                         sourceAlias = pageSource.Substring(pageSource.IndexOf("sourceAlias"), 100);
                         string[] Arr = sourceAlias.Split('"');
                         sourceAlias = Arr[2];
                     }

                     if (IsAllAccounts)
                     {
                         try
                         {
                             ClsLinkedinMain obj_ClsLinkedinMain = new ClsLinkedinMain();

                             Dictionary<string, string> dTotalFriends = obj_ClsLinkedinMain.PostAddMembers(ref HttpHelper, UserEmail);

                             Log("[ " + DateTime.Now + " ] => [ No. Of Friends = " + dTotalFriends.Count + " With Username >>> " + UserEmail + " ]");

                             if (dTotalFriends.Count > 0)
                             {
                                 // PostMessageToAllAccounts_1By1(ref HttpHelper, dTotalFriends, msg, body, UserEmail, FromemailId, FromEmailNam, mindelay, maxdelay);
                                 PostMessageToAllAccounts_1By1(ref HttpHelper, dTotalFriends, messagesubject, userText, UserEmail, FromemailId, FromEmailNam, mindelay, maxdelay);
                             }
                             Log("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED With Username >>> " + UserEmail + " ]");
                             Log("-----------------------------------------------------------------------------------------------------------------------------------");

                             return;
                         }
                         catch (Exception ex)
                         {
                             GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error -->  PostFinalMsg_1By1() --> 1 --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                             GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> PostFinalMsg_1By1() --> 1 --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinComposeMessageErrorLogs);
                         }
                     }
                     
                     string ProfileUrl = string.Empty;
                     foreach (KeyValuePair<string, string> itemChecked in SlectedContacts)
                     {
                         ComposeMsgDbManager objComposeMsgDbMgr = new ComposeMsgDbManager();

                         //ProfileUrl = "https://www.linkedin.com/profile/view?id=" + itemChecked.Key + ""; for client ahmad

                         //ProfileUrl = "https://www.linkedin.com/contacts/view?id=" + itemChecked.Key + "";
                         //string profilePageSource = HttpHelper.getHtmlfromUrl1(new Uri(ProfileUrl));
                        //string  ProfileID = Utils.getBetween(profilePageSource, "id=", ",").Replace("\"", "");

                         string ProfileID = string.Empty;
                         if (Globals.groupStatusString == "API")
                         {
                             ProfileUrl = "https://www.linkedin.com/contacts/view?id=" + itemChecked.Key + "";
                             string profilePageSource = HttpHelper.getHtmlfromUrl1(new Uri(ProfileUrl));
                             ProfileID = Utils.getBetween(profilePageSource, "id=", ",").Replace("\"", "");
                         }
                         else
                         {
                             ProfileUrl = "https://www.linkedin.com/profile/view?id=" + itemChecked.Key + "";

                             ProfileID = itemChecked.Key;
                         }
                         try
                         {
                             DataSet ds = new DataSet();
                             DataSet ds_bList = new DataSet();
                             string ContactName = string.Empty;
                             string Nstring = string.Empty;
                             string connId = string.Empty;
                             string FName = string.Empty;
                             string Lname = string.Empty;
                             string tempBody = string.Empty;
                             string tempsubject = string.Empty;

                             try
                             {
                                 FName = itemChecked.Value.Split(' ')[0];
                                 Lname = itemChecked.Value.Split(' ')[1];
                             }
                             catch
                             {
                             }

                             FullName = FName + " " + Lname;
                             try
                             {
                                 ContactName = ContactName + "  :  " + FullName;
                             }
                             catch { }
                             if (ToMsg == string.Empty)
                             {
                                 ToMsg += FullName;
                             }
                             else
                             {
                                 ToMsg += ";" + FullName;
                             }

                             Log("[ " + DateTime.Now + " ] => [ Adding Contact " + FullName + " ]");
                             string ToCd = ProfileID;//itemChecked.Key; for client ahmed sudi
                             List<string> AddAllString = new List<string>();

                             if (FString == string.Empty)
                             {
                                 string CompString = "{" + "\"" + "_" + ToCd.Trim() + "\"" + ":" + "{" + "\"" + "memberId" + "\"" + ":" + "\"" + ToCd.Trim() + "\"" + "," + "\"" + "firstName" + "\"" + ":" + "\"" + FName + "\"" + "," + "\"" + "lastName" + "\"" + ":" + "\"" + Lname + "\"" + "}";
                                 FString = CompString;
                             }
                             else
                             {
                                 string CompString = "\"" + "_" + ToCd.Trim() + "\"" + ":" + "{" + "\"" + "memberId" + "\"" + ":" + "\"" + ToCd.Trim() + "\"" + "," + "\"" + "firstName" + "\"" + ":" + "\"" + FName + "\"" + "," + "\"" + "lastName" + "\"" + ":" + "\"" + Lname + "\"" + "}";
                                 FString = CompString;
                             }

                             if (Nstring == string.Empty)
                             {
                                 Nstring = FString;
                                 connId = ToCd;
                             }
                             else
                             {
                                 Nstring += "," + FString;
                                 connId += " " + ToCd;
                             }

                             Nstring += "}";

                             try
                             {
                                 string PostMessage = string.Empty;
                                 string ResponseStatusMsg = string.Empty;

                                 if (msg_spintaxt)
                                 {
                                     tempBody = GlobusSpinHelper.spinLargeText(new Random(), userText);
                                     // tempBody = messagebody[RandomNumberGenerator.GenerateRandom(0, messagebody.Count - 1)];
                                     tempsubject = messagesubject[RandomNumberGenerator.GenerateRandom(0, messagesubject.Count - 1)];
                                 }
                                 else
                                 {
                                     tempBody = body;
                                     tempsubject = msg;
                                 }


                                 Log("[ " + DateTime.Now + " ] => [ Sending Message ]");
                                 if (!FName.Contains("."))
                                 {
                                     //tempBody = tempBody.Replace("<Insert Name>", FullName);
                                     tempBody = tempBody.Replace("<Insert Name>", FName);
                                     tempBody = tempBody.Replace("<Insert From Email>", FromEmailNam);
                                 }
                                 else
                                 {
                                     tempBody = tempBody.Replace("<Insert Name>", FullName);
                                     tempBody = tempBody.Replace("<Insert From Email>", FromEmailNam);
                                 }

                                 //Check BlackListed Accounts
                                 try
                                 {
                                    // string Querystring = "Select ProfileID From tb_BlackListAccount Where ProfileID ='" + itemChecked.Key + "'";
                                     string Querystring = "Select ProfileID From tb_BlackListAccount Where ProfileID ='" + ProfileID + "'";
                                     ds_bList = DataBaseHandler.SelectQuery(Querystring, "tb_BlackListAccount");
                                 }
                                 catch { }

                                 if (preventMsgSameUser)
                                 {
                                     try
                                     {
                                         string Querystring = "Select MsgFrom,MsgToId,MsgTo,MsgSubject,MsgBody From tb_ManageComposeMsg Where MsgFrom ='" + UserEmail + "' and MsgBody = '" + tempBody + "' and MsgToId = " + connId + "";
                                         ds = DataBaseHandler.SelectQuery(Querystring, "tb_ManageComposeMsg");
                                     }
                                     catch { }
                                 }

                                 
                                 if (preventMsgGlobalUser)
                                 {
                                     try
                                     {
                                         string Querystring = "Select MsgFrom,MsgToId,MsgTo,MsgSubject,MsgBody From tb_ManageComposeMsg Where MsgToId = " + connId + "";
                                         ds = DataBaseHandler.SelectQuery(Querystring, "tb_ManageComposeMsg");
                                     }
                                     catch { }
                                 }

                                 if (ds.Tables.Count > 0)
                                 {
                                     if (ds.Tables[0].Rows.Count > 0)
                                     {

                                         PostMessage = "";
                                         ResponseStatusMsg = "Already Sent";

                                     }
                                     else
                                     {
                                         if (ds_bList.Tables.Count > 0 && ds_bList.Tables[0].Rows.Count > 0)
                                         {

                                             Log("[ " + DateTime.Now + " ] => [ User: " + ContactName.Replace(":", string.Empty).Trim() + " is Added BlackListed List For Send Messages Pls Check ]");
                                             ResponseStatusMsg = "BlackListed";
                                         }
                                         else
                                         {
                                             tempBody = tempBody.Replace("<Insert Name>", string.Empty).Replace("<Insert Group>", string.Empty).Replace("<Insert From Email>", string.Empty);

                                             PostMessage = "senderEmail=" + FromemailId.Trim() + "&ccInput=&subject=" + Uri.EscapeDataString(msg.ToString()) + "&body=" + Uri.EscapeDataString(tempBody.ToString()) + "&isReply=&isForward=&itemId=&recipients=" + Uri.EscapeUriString(connId) + "&recipientNames=" + Uri.EscapeUriString(Nstring) + "&groupId=&csrfToken=" + csrfToken + "&sourceAlias=" + sourceAlias + "&submit=Send+Message";
                                             ResponseStatusMsg = HttpHelper.postFormDataRef(new Uri("http://www.linkedin.com/inbox/mailbox/message/send"), PostMessage, "https://www.linkedin.com/inbox/", "", "", "XMLHttpRequest", "https://www.linkedin.com", "1");   //ahmed sudi client changes
                                             //PostMessage = "csrfToken=" + csrfToken + "&subject=" + Uri.EscapeDataString(tempsubject.ToString()) + "&body=" + Uri.EscapeDataString(tempBody.ToString()) + "&submit=Send+Message&fromName=" + FromEmailNam + "&fromEmail=" + FromemailId + "&connectionIds=" + connId + "&connectionNames=" + Uri.EscapeUriString(Nstring) + "&allowEditRcpts=true&addMoreRcpts=false&openSocialAppBodySuffix=&st=&viewerDestinationUrl=&goback=.smg_*1_*1_*1_*1_*1_*1_*1_*1_*1";   //commented for client ahmad sudi
                                             //ResponseStatusMsg = HttpHelper.postFormData(new Uri("http://www.linkedin.com/msgToConns"), PostMessage);
                                         }
                                     }
                                 }
                                 else
                                 {
                                     if (ds_bList.Tables.Count > 0 && ds_bList.Tables[0].Rows.Count > 0)
                                     {

                                         Log("[ " + DateTime.Now + " ] => [ User: " + ContactName.Replace(":", string.Empty).Trim() + " is Added BlackListed List For Send Messages Pls Check ]");
                                         ResponseStatusMsg = "BlackListed";
                                     }
                                     else
                                     {
                                         tempBody = tempBody.Replace("<Insert Name>", string.Empty).Replace("<Insert Group>", string.Empty).Replace("<Insert From Email>", string.Empty);
                                         PostMessage = "senderEmail=" + FromemailId.Trim() + "&ccInput=&subject=" + Uri.EscapeDataString(msg.ToString()) + "&body=" + Uri.EscapeDataString(tempBody.ToString()) + "&isReply=&isForward=&itemId=&recipients=" + Uri.EscapeUriString(connId) + "&recipientNames=" + Uri.EscapeUriString(Nstring) + "&groupId=&csrfToken=" + csrfToken + "&sourceAlias=" + sourceAlias + "&submit=Send+Message";
                                         ResponseStatusMsg = HttpHelper.postFormDataRef(new Uri("http://www.linkedin.com/inbox/mailbox/message/send"), PostMessage, "https://www.linkedin.com/inbox/", "", "", "XMLHttpRequest", "https://www.linkedin.com", "1");   //ahmed sudi client changes
                                             
                                         
                                         //PostMessage = "csrfToken=" + csrfToken + "&subject=" + Uri.EscapeDataString(tempsubject.ToString()) + "&body=" + Uri.EscapeDataString(tempBody.ToString()) + "&submit=Send+Message&fromName=" + FromEmailNam + "&fromEmail=" + FromemailId + "&connectionIds=" + connId + "&connectionNames=" + Uri.EscapeUriString(Nstring) + "&allowEditRcpts=true&addMoreRcpts=false&openSocialAppBodySuffix=&st=&viewerDestinationUrl=&goback=.smg_*1_*1_*1_*1_*1_*1_*1_*1_*1";
                                         //ResponseStatusMsg = HttpHelper.postFormData(new Uri("http://www.linkedin.com/msgToConns"), PostMessage);
                                     }
                                 }

                                 if ((ResponseStatusMsg.Contains("Your message was successfully sent.")) || (ResponseStatusMsg.Contains("Se ha enviado tu mensaje satisfactoriamente")) || (ResponseStatusMsg.Contains("Sua mensagem foi enviada")))
                                 {
                                     Log("[ " + DateTime.Now + " ] => [ Subject Posted : " + tempsubject + " ]");
                                     Log("[ " + DateTime.Now + " ] => [ Body Text Posted : " + tempBody.ToString() + " ]");
                                     Log("[ " + DateTime.Now + " ] => [ Message posted to " + FullName + " ]");

                                     ReturnString = "Your message was successfully sent.";
                                     string bdy = string.Empty;
                                     try
                                     {
                                         bdy = tempBody.ToString().Replace("\r", string.Empty).Replace("\n", " ").Replace(",", " ");
                                     }
                                     catch { }
                                     if (string.IsNullOrEmpty(bdy))
                                     {
                                         bdy = tempBody.ToString().Replace(",", ":");
                                     }

                                     string CSVHeader = "UserName" + "," + "Subject" + "," + "Body Text" + "," + "ContactName" + "," + "ProfileUrl";
                                     string CSV_Content = UserEmail + "," + msg + "," + bdy + "," + ContactName.Replace(":", string.Empty).Trim() + "," + ProfileUrl;
                                     CSVUtilities.ExportDataCSVFile(CSVHeader, CSV_Content, Globals.path_ComposeMessageSent);

                                     try
                                     {
                                         objComposeMsgDbMgr.InsertComposeMsgData(UserEmail, Convert.ToInt32(connId), ContactName, msg, tempBody);
                                     }
                                     catch { }

                                 }

                                 else if (ResponseStatusMsg.Contains("There was an unexpected problem that prevented us from completing your request."))
                                 {
                                     Log("[ " + DateTime.Now + " ] => [ Error In Message Posting ]");
                                     GlobusFileHelper.AppendStringToTextfileNewLine("Error In Message Posting", Globals.path_ComposeMessage);
                                 }
                                 else if ((ResponseStatusMsg.Contains("Already Sent")) || (ResponseStatusMsg.Contains("Ya ha sido enviada")))
                                 {
                                     string bdy = string.Empty;
                                     try
                                     {
                                         bdy = body.ToString().Replace("\r", string.Empty).Replace("\n", " ").Replace(",", " ");
                                     }
                                     catch { }

                                     if (string.IsNullOrEmpty(bdy))
                                     {
                                         bdy = tempBody.ToString().Replace(",", ":");
                                     }
                                     string CSVHeader = "UserName" + "," + "Subject" + "," + "Body Text" + "," + "ContactName";
                                     string CSV_Content = UserEmail + "," + msg + "," + bdy.ToString() + "," + ContactName.Replace(":",string.Empty);
                                     CSVUtilities.ExportDataCSVFile(CSVHeader, CSV_Content, Globals.path_MessageAlreadySentComposeMgs);

                                     Log("[ " + DateTime.Now + " ] => [ Message Not Posted To Account: " + ContactName.Replace(":", string.Empty) + " because it has sent the same message already]");
                                 }
                                 else if (ResponseStatusMsg.Contains("BlackListed"))
                                 { 
                                 
                                 }
                                 else
                                 {
                                     Log("[ " + DateTime.Now + " ] => [ Failed In Message Posting ]");
                                     GlobusFileHelper.AppendStringToTextfileNewLine("Failed In Message Posting", Globals.path_ComposeMessage);
                                 }
                             }
                             catch (Exception ex)
                             {
                                 //Log(DateTime.Now + " [ Error:" + ex.Message + "StackTrace --> >>>" + ex.StackTrace + " ]");
                                 GlobusFileHelper.AppendStringToTextfileNewLine(" Error:" + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.path_ComposeMessage);
                             }

                             int delay = RandomNumberGenerator.GenerateRandom(mindelay, maxdelay);
                             Log("[ " + DateTime.Now + " ] => [ Delay for : " + delay + " Seconds ]");
                             Thread.Sleep(delay * 1000);
                         }
                         catch (Exception ex)
                         {
                             //Log(DateTime.Now + " [ Error:" + ex.Message + "StackTrace --> >>>" + ex.StackTrace + " ]");
                             GlobusFileHelper.AppendStringToTextfileNewLine(" Error:" + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.path_ComposeMessage);
                         }
                     }
                 }
                 catch (Exception ex)
                 {
                     GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error -->  PostFinalMsg_1By1() --> 2 --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                     GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> PostFinalMsg_1By1() --> 2 --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinComposeMessageErrorLogs);
                 }

             }
             catch (Exception ex)
             {
                 GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error -->  PostFinalMsg_1By1() --> 3 --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                 GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> PostFinalMsg_1By1() --> 3 --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinComposeMessageErrorLogs);
             }

         }
         #endregion

        #region PostMessageToAllAccounts
         private void PostMessageToAllAccounts(ref GlobusHttpHelper HttpHelper,Dictionary<string, string> SlectedContacts, Dictionary<string, string> dTotalFriends, string msg, string body, string UserEmail, string FromemailId, string FromEmailNam,int mindelay, int maxdelay)
         {
             
                 string postdata = string.Empty;
                 string postUrl = string.Empty;
                 string ResLogin = string.Empty;
                 string csrfToken = string.Empty;
                 string sourceAlias = string.Empty;
                 string ReturnString = string.Empty;
                 string PostMsgSubject = string.Empty;
                 string PostMsgBody = string.Empty;
                 string FString = string.Empty;
                 string Nstring = string.Empty;
                 string connId = string.Empty;
                 string FullName = string.Empty;
                 string ToMsg = string.Empty;

                 try
                 {
                     string MessageText = string.Empty;
                     string PostedMessage = string.Empty;
                     string pageSource = string.Empty;
                     string ToCd = string.Empty;

                     try
                     {
                         pageSource = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/home?trk=hb_tab_home_top"));
                     }
                     catch (Exception ex)
                     {
                         Console.WriteLine("ex.Message >>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + "  ex.Stack Trace >>> " + ex.StackTrace);
                     }

                     if (string.IsNullOrEmpty(pageSource))
                     {
                         try
                         {
                             pageSource = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/home?trk=hb_tab_home_top"));

                             if (string.IsNullOrEmpty(pageSource))
                             {
                                 Log("[ " + DateTime.Now + " ] => [ Page Source Is Null With Username >>> " + UserEmail + " ]");
                                 return;
                             }
                         }
                         catch (Exception ex)
                         {
                             Console.WriteLine("ex.Message >>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + "  ex.Stack Trace >>> " + ex.StackTrace);

                         }
                     }

                     if (pageSource.Contains("csrfToken"))
                     {
                         try
                         {
                             csrfToken = pageSource.Substring(pageSource.IndexOf("csrfToken"), 50);
                             string[] Arr = csrfToken.Split('&');
                             csrfToken = Arr[0];
                             csrfToken = csrfToken.Replace("csrfToken", "").Replace("\"", string.Empty).Replace("value", string.Empty).Replace("cs", string.Empty).Replace("id", string.Empty).Replace("=", string.Empty).Replace(">\n<script src",string.Empty);
                             csrfToken = csrfToken.Trim();
                         }
                         catch (Exception ex)
                         {
                             Console.WriteLine("ex.Message >>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + "  ex.Stack Trace >>> " + ex.StackTrace);
                         }
                     }

                     if (pageSource.Contains("sourceAlias"))
                     {
                         try
                         {
                             sourceAlias = pageSource.Substring(pageSource.IndexOf("sourceAlias"), 100);
                             string[] Arr = sourceAlias.Split('"');
                             sourceAlias = Arr[2];
                         }
                         catch (Exception ex)
                         {
                             Console.WriteLine("ex.Message >>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + "  ex.Stack Trace >>> " + ex.StackTrace);
                         }
                     }

                     string ContactName = string.Empty;
                     Dictionary<string, string> SlectedSentContacts = new Dictionary<string, string>();
                     int counter = 1;

                    // foreach (KeyValuePair<string, string> itemChecked in dTotalFriends)
                         foreach (KeyValuePair<string, string> itemChecked in SlectedContacts ) 
                     {                        
                             if (IsAllAccounts)
                             {
                                   if (counter > NoOfFriends)
                                    {
                                        break;
                                    }                             
                             }
                      
                             if (counter < 50)
                                 {
                                 
                                     SlectedSentContacts.Add(itemChecked.Key, itemChecked.Value);

                                     try
                                     {
                                         string FName = string.Empty;
                                         string Lname = string.Empty;

                                         try
                                         {
                                             FName = itemChecked.Value.Split(' ')[0];
                                             Lname = itemChecked.Value.Split(' ')[1];
                                         }
                                         catch
                                         {
                                         }

                                         FullName = FName + " " + Lname;
                                         try
                                         {
                                             ContactName = ContactName + "  :  " + FullName;
                                         }
                                         catch { }

                                         if (ToMsg == string.Empty)
                                         {
                                             ToMsg += FullName;
                                         }
                                         else
                                         {
                                             ToMsg += ";" + FullName;
                                         }

                                         Log("[ " + DateTime.Now + " ] => [ Adding Contact " + FullName + " ]");

                                         try
                                         {
                                             //ToCd = itemChecked.Key.Split(':')[1];
                                             ToCd = itemChecked.Key.ToString();
                                         }
                                         catch { }

                                         List<string> AddAllString = new List<string>();

                                         if (FString == string.Empty)
                                         {
                                             string CompString = "{" + "\"" + "_" + ToCd.Trim() + "\"" + ":" + "{" + "\"" + "memberId" + "\"" + ":" + "\"" + ToCd.Trim() + "\"" + "," + "\"" + "firstName" + "\"" + ":" + "\"" + FName + "\"" + "," + "\"" + "lastName" + "\"" + ":" + "\"" + Lname + "\"" + "}";
                                             FString = CompString;
                                         }
                                         else
                                         {
                                             string CompString = "\"" + "_" + ToCd.Trim() + "\"" + ":" + "{" + "\"" + "memberId" + "\"" + ":" + "\"" + ToCd.Trim() + "\"" + "," + "\"" + "firstName" + "\"" + ":" + "\"" + FName + "\"" + "," + "\"" + "lastName" + "\"" + ":" + "\"" + Lname + "\"" + "}";
                                             FString = CompString;
                                         }

                                         if (Nstring == string.Empty)
                                         {
                                             Nstring = FString;
                                             connId = ToCd;
                                         }
                                         else
                                         {
                                             Nstring += "," + FString;
                                             connId += " " + ToCd;
                                         }
                                     }
                                     catch { }

                                     Nstring += "}";
                                     
                                 }
                                 else
                                 {
                                     try
                                     {
                                         SlectedContacts2.Add(itemChecked.Key, itemChecked.Value);
                                     }
                                     catch
                                     {
                                         
                                     }
                                    
                                 }

                                 counter++;
                               }


                             if (SlectedContacts2.Count != 0)
                             {
                                 foreach (KeyValuePair<string, string> itemremove in SlectedSentContacts)
                                 {
                                     SlectedContacts2.Remove(itemremove.Key);
                                 }
                             }
  
                     try
                     {
                         string PostMessage;
                         string ResponseStatusMsg;
                         Log("[ " + DateTime.Now + " ] => [ Sending Message ]");
                         
                         //PostMessage = "csrfToken=" + csrfToken + "&subject=" + txtMsgSubject.Text.ToString() + "&body=" + txtMsgBody.Text.ToString() + "&submit=Send Message&showRecipeints=showRecipeints&ccSender=ccSender&fromName=" + FromEmailNam + "&fromEmail=" + FromemailId + "&connectionIds=" + connId + "&connectionNames=" + Nstring + "&allowEditRcpts=true&addMoreRcpts=false&openSocialAppBodySuffix=&st=&viewerDestinationUrl=&goback=.rmg_*1_*1_*1_*1_*1_*1_*1_*1_*1";
                        
                         PostMessage = "csrfToken=" + csrfToken + "&subject=" + Uri.EscapeDataString(msg.ToString()) + "&body=" + Uri.EscapeDataString(body.ToString()) + "&submit=Send+Message&showRecipeints=showRecipeints&fromName=" + FromEmailNam + "&fromEmail=" + FromemailId + "&connectionIds=" + connId + "&connectionNames=" + Uri.EscapeUriString(Nstring) + "&allowEditRcpts=true&addMoreRcpts=false&openSocialAppBodySuffix=&st=&viewerDestinationUrl=&goback=.smg_*1_*1_*1_*1_*1_*1_*1_*1_*1";
                         //string PostMessage1 = "senderEmail=" + FromemailId + "&subject=" + Uri.EscapeDataString(msg.ToString()) + "&body=" + Uri.EscapeDataString(body.ToString()) + "&showRecipients=showRecipients&isReply=&isForward=&itemId=&recipients=" + connId + "&recipientNames=" + Uri.EscapeUriString(Nstring) + "&groupId=&csrfToken=" + csrfToken + "&sourceAlias=" + sourceAlias + "&submit=Send+Message";
                         //IncodePost = Uri.EscapeUriString(PostMessage).Replace(":", "%3A").Replace("%20", "+").Replace("++", "+");
                         ResponseStatusMsg = HttpHelper.postFormData(new Uri("http://www.linkedin.com/msgToConns"), PostMessage);
                        // string ResponseStatusMsg1 = HttpHelper.postFormData(new Uri("http://www.linkedin.com/msgToConns"), PostMessage);
                         //int totPost = counter--;

                         if (ResponseStatusMsg.Contains("Your message was successfully sent."))
                         {
                             Log("[ " + DateTime.Now + " ] => [ Subject Posted : " + msg + " ]");
                             Log("[ " + DateTime.Now + " ] => [ Body Text Posted : " + body.ToString() + " ]");
                             Log("[ " + DateTime.Now + " ] => [ Message Posted To Accounts With Username >>> " + UserEmail + " ]");
                             ReturnString = "Your message was successfully sent.";
                             string bdy = string.Empty;
                             try
                             {
                                 bdy = body.ToString().Replace("\r", string.Empty).Replace("\n", " ").Replace(",", " ");
                             }
                             catch (Exception ex)
                             {
                                 Console.WriteLine("ex.Message >>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + "  ex.Stack Trace >>> " + ex.StackTrace);
                             }
                             if (string.IsNullOrEmpty(bdy))
                             {
                                 bdy = body.ToString().Replace(",", ":");
                             }
                             string CSVHeader = "UserName" + "," + "Subject" + "," + "Body Text" + "," + "ContactName";
                             string CSV_Content = UserEmail + "," + msg + "," + bdy + "," + ContactName;
                             CSVUtilities.ExportDataCSVFile(CSVHeader, CSV_Content, Globals.path_ComposeMessageSent);

                             //GlobusFileHelper.AppendStringToTextfileNewLine("Subject Posted : " + txtMsgSubject.Text.ToString() + " To: " + ToMsg + " From: " + FromSendMsg.ToString(), Globals.path_ComposeMessage);
                             //GlobusFileHelper.AppendStringToTextfileNewLine("Body Text Posted : " + body.ToString() + " To: " + ToMsg + " From: " + FromSendMsg.ToString(), Globals.path_ComposeMessage);
                             //GlobusFileHelper.AppendStringToTextfileNewLine("-------------------------------------------------------------------------------------------------------", Globals.path_ComposeMessage);
                         }
                         //else if(ResponseStatusMsg.Contains("You have reached the daily login limit for your email provider. Please try again tomorrow."))
                         //{
                         //    Log("You have reached the daily login limit for your email provider. Please try again tomorrow.");
                         //    GlobusFileHelper.AppendStringToTextfileNewLine("Error In Message Posting", Globals.path_ComposeMessage);
                         //}
                         else if (ResponseStatusMsg.Contains("There was an unexpected problem that prevented us from completing your request."))
                         {
                             Log("[ " + DateTime.Now + " ] => [ Error In Message Posting ]");
                             GlobusFileHelper.AppendStringToTextfileNewLine("Error In Message Posting", Globals.path_ComposeMessage);
                         }
                         else
                         {
                             Log("[ " + DateTime.Now + " ] => [ Failed In Message Posting ]");
                             GlobusFileHelper.AppendStringToTextfileNewLine("Failed In Message Posting", Globals.path_ComposeMessage);
                         }

                         int delay = RandomNumberGenerator.GenerateRandom(mindelay, maxdelay);
                         Log("[ " + DateTime.Now + " ] => [ Delay for : " + delay + " Seconds ]");
                         Thread.Sleep(delay * 1000);
                      }
                     catch (Exception ex)
                     {
                         Log("[ " + DateTime.Now + " ] => [ Error:" + ex.StackTrace + " ]");
                         GlobusFileHelper.AppendStringToTextfileNewLine(" Error:" + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.path_ComposeMessage);
                     }
                 }
                 catch (Exception ex)
                 {
                     Console.WriteLine("ex.Message >>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + "  ex.Stack Trace >>> " + ex.StackTrace);
                 }
             
         } 
         #endregion

        #region PostMessageToAllAccounts_1By1
         //private void PostMessageToAllAccounts_1By1(ref GlobusHttpHelper HttpHelper, Dictionary<string, string> dTotalFriends, string msg, string body, string UserEmail, string FromemailId, string FromEmailNam, int mindelay, int maxdelay)
         private void PostMessageToAllAccounts_1By1(ref GlobusHttpHelper HttpHelper, Dictionary<string, string> dTotalFriends, List<string> msgsubject, string userText, string UserEmail, string FromemailId, string FromEmailNam, int mindelay, int maxdelay)
         {
             try
             {
                 string postdata = string.Empty;
                 string postUrl = string.Empty;

                 string ResLogin = string.Empty;
                 string csrfToken = string.Empty;
                 string sourceAlias = string.Empty;

                 string ReturnString = string.Empty;
                 string PostMsgSubject = string.Empty;
                 string PostMsgBody = string.Empty;
                 string FString = string.Empty;
                 //string Nstring = string.Empty;
                 //string connId = string.Empty;
                 string FullName = string.Empty;
                 string ToMsg = string.Empty;

                 try
                 {
                     string MessageText = string.Empty;
                     string PostedMessage = string.Empty;
                     string pageSource = string.Empty;

                     try
                     {
                         pageSource = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/home?trk=hb_tab_home_top"));
                     }
                     catch (Exception ex)
                     {
                         Console.WriteLine("ex.Message >>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + "  ex.Stack Trace >>> " + ex.StackTrace);
                     }

                     if (string.IsNullOrEmpty(pageSource))
                     {
                         try
                         {
                             pageSource = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/home?trk=hb_tab_home_top"));

                             if (string.IsNullOrEmpty(pageSource))
                             {
                                 Log("[ " + DateTime.Now + " ] => [ Page Source Is Null With Username >>> " + UserEmail + " ]");
                                 return;
                             }
                         }
                         catch (Exception ex)
                         {
                             Console.WriteLine("ex.Message >>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + "  ex.Stack Trace >>> " + ex.StackTrace);

                         }

                     }

                     if (pageSource.Contains("csrfToken"))
                     {
                         try
                         {
                             csrfToken = pageSource.Substring(pageSource.IndexOf("csrfToken"), 50);
                             string[] Arr = csrfToken.Split('&');
                             csrfToken = Arr[0];
                             csrfToken = csrfToken.Replace("csrfToken", "").Replace("\"", string.Empty).Replace("value", string.Empty).Replace("cs", string.Empty).Replace("id", string.Empty).Replace("=", string.Empty);
                             csrfToken = csrfToken.Trim();
                         }
                         catch (Exception ex)
                         {
                             Console.WriteLine("ex.Message >>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + "  ex.Stack Trace >>> " + ex.StackTrace);
                         }
                     }

                     if (pageSource.Contains("sourceAlias"))
                     {
                         try
                         {
                             sourceAlias = pageSource.Substring(pageSource.IndexOf("sourceAlias"), 100);
                             string[] Arr = sourceAlias.Split('"');
                             sourceAlias = Arr[2];
                         }
                         catch (Exception ex)
                         {
                             Console.WriteLine("ex.Message >>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + "  ex.Stack Trace >>> " + ex.StackTrace);
                         }
                     }

                     //string ContactName = string.Empty;

                     int counter = 1;

                     foreach (KeyValuePair<string, string> itemChecked in dTotalFriends)
                     {
                         try
                         {

                             if (!IsAllAccounts)
                             {
                                 if (counter > NoOfFriends)
                                 {
                                     break;
                                 }
                             }

                             counter++;

                             string ContactName = string.Empty;
                             string Nstring = string.Empty;
                             string connId = string.Empty;
                             string FName = string.Empty;
                             string Lname = string.Empty;
                             string tempBody = string.Empty;
                             string tempsubject = string.Empty;

                             try
                             {
                                 FName = itemChecked.Value.Split(' ')[0];
                                 Lname = itemChecked.Value.Split(' ')[1];
                             }
                             catch
                             {
                             }

                             FullName = FName + " " + Lname;
                             try
                             {
                                 ContactName = ContactName + "  :  " + FullName;
                             }
                             catch { }
                             if (ToMsg == string.Empty)
                             {
                                 ToMsg += FullName;
                             }
                             else
                             {
                                 ToMsg += ";" + FullName;
                             }

                             Log("[ " + DateTime.Now + " ] => [ Adding Contact " + FullName + " ]");


                             string ToCd = itemChecked.Key;

                             if (ToCd.Contains(":"))
                             {
                                 try
                                 {
                                     ToCd = ToCd.Substring(ToCd.IndexOf(":"), ToCd.Length - ToCd.IndexOf(":")).Replace(":", string.Empty).Trim();
                                 }
                                 catch (Exception ex)
                                 {
                                     Console.WriteLine("Error >>> " + ex.StackTrace);
                                 }

                             }

                             List<string> AddAllString = new List<string>();

                             if (FString == string.Empty)
                             {
                                 string CompString = "{" + "\"" + "_" + ToCd.Trim() + "\"" + ":" + "{" + "\"" + "memberId" + "\"" + ":" + "\"" + ToCd.Trim() + "\"" + "," + "\"" + "firstName" + "\"" + ":" + "\"" + FName + "\"" + "," + "\"" + "lastName" + "\"" + ":" + "\"" + Lname + "\"" + "}";
                                 FString = CompString;
                             }
                             else
                             {
                                 string CompString = "\"" + "_" + ToCd.Trim() + "\"" + ":" + "{" + "\"" + "memberId" + "\"" + ":" + "\"" + ToCd.Trim() + "\"" + "," + "\"" + "firstName" + "\"" + ":" + "\"" + FName + "\"" + "," + "\"" + "lastName" + "\"" + ":" + "\"" + Lname + "\"" + "}";
                                 FString = CompString;
                             }

                             if (Nstring == string.Empty)
                             {
                                 Nstring = FString;
                                 connId = ToCd;
                             }
                             else
                             {
                                 Nstring += "," + FString;
                                 connId += " " + ToCd;
                             }

                             Nstring += "}";

                             try
                             {
                                 string PostMessage;
                                 string ResponseStatusMsg;
                                 Log("[ " + DateTime.Now + " ] => [ Sending Message ]");
                                 tempBody = GlobusSpinHelper.spinLargeText(new Random(), userText);
                                 //tempBody = msgbody[RandomNumberGenerator.GenerateRandom(0, msgbody.Count - 1)];
                                 tempsubject = msgsubject[RandomNumberGenerator.GenerateRandom(0, msgsubject.Count - 1)];

                                 //if (body.Contains("<Insert Name>"))
                                 {
                                     tempBody = tempBody.Replace("<Insert Name>", FName);
                                 }

                                 //if (body.Contains("<Insert From Email>"))
                                 {
                                     tempBody = tempBody.Replace("<Insert From Email>", FromEmailNam);
                                 }

                                 tempBody = tempBody.Replace("<Insert Name>", string.Empty).Replace("<Insert Group>", string.Empty).Replace("<Insert From Email>", string.Empty);


                                 //PostMessage = "csrfToken=" + csrfToken + "&subject=" + txtMsgSubject.Text.ToString() + "&body=" + txtMsgBody.Text.ToString() + "&submit=Send Message&showRecipeints=showRecipeints&ccSender=ccSender&fromName=" + FromEmailNam + "&fromEmail=" + FromemailId + "&connectionIds=" + connId + "&connectionNames=" + Nstring + "&allowEditRcpts=true&addMoreRcpts=false&openSocialAppBodySuffix=&st=&viewerDestinationUrl=&goback=.rmg_*1_*1_*1_*1_*1_*1_*1_*1_*1";
                                 //PostMessage = "csrfToken=" + csrfToken + "&subject=" + Uri.EscapeDataString(msg.ToString()) + "&body=" + Uri.EscapeDataString(tempBody.ToString()) + "&submit=Send+Message&showRecipeints=showRecipeints&fromName=" + FromEmailNam + "&fromEmail=" + FromemailId + "&connectionIds=" + connId + "&connectionNames=" + Uri.EscapeUriString(Nstring) + "&allowEditRcpts=true&addMoreRcpts=false&openSocialAppBodySuffix=&st=&viewerDestinationUrl=&goback=.smg_*1_*1_*1_*1_*1_*1_*1_*1_*1";
                                 PostMessage = "csrfToken=" + csrfToken + "&subject=" + Uri.EscapeDataString(tempsubject.ToString()) + "&body=" + Uri.EscapeDataString(tempBody.ToString()) + "&submit=Send+Message&showRecipeints=showRecipeints&fromName=" + FromEmailNam + "&fromEmail=" + FromemailId + "&connectionIds=" + connId + "&connectionNames=" + Uri.EscapeUriString(Nstring) + "&allowEditRcpts=true&addMoreRcpts=false&openSocialAppBodySuffix=&st=&viewerDestinationUrl=&goback=.smg_*1_*1_*1_*1_*1_*1_*1_*1_*1";
                                 //IncodePost = Uri.EscapeUriString(PostMessage).Replace(":", "%3A").Replace("%20", "+").Replace("++", "+");
                                 ResponseStatusMsg = HttpHelper.postFormData(new Uri("http://www.linkedin.com/msgToConns"), PostMessage);

                                 if (ResponseStatusMsg.Contains("Your message was successfully sent."))
                                 {
                                     Log("[ " + DateTime.Now + " ] => [ Subject Posted : " + tempsubject + " ]");
                                     Log("[ " + DateTime.Now + " ] => [ Body Text Posted : " + tempBody.ToString() + " ]");
                                     Log("[ " + DateTime.Now + " ] => [ Message Posted To All Selected Accounts With Username >>> " + UserEmail + " ]");
                                     ReturnString = "Your message was successfully sent.";
                                     string bdy = string.Empty;
                                     try
                                     {
                                         bdy = tempBody.ToString().Replace("\r", string.Empty).Replace("\n", " ").Replace(",", " ");
                                     }
                                     catch (Exception ex)
                                     {
                                         Console.WriteLine("ex.Message >>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + "  ex.Stack Trace >>> " + ex.StackTrace);
                                     }
                                     if (string.IsNullOrEmpty(bdy))
                                     {
                                         bdy = tempBody.ToString().Replace(",", ":");
                                     }
                                     string CSVHeader = "UserName" + "," + "Subject" + "," + "Body Text" + "," + "ContactName";
                                     string CSV_Content = UserEmail + "," + tempsubject + "," + bdy + "," + ContactName;
                                     CSVUtilities.ExportDataCSVFile(CSVHeader, CSV_Content, Globals.path_ComposeMessageSent);

                                     //GlobusFileHelper.AppendStringToTextfileNewLine("Subject Posted : " + txtMsgSubject.Text.ToString() + " To: " + ToMsg + " From: " + FromSendMsg.ToString(), Globals.path_ComposeMessage);
                                     //GlobusFileHelper.AppendStringToTextfileNewLine("Body Text Posted : " + body.ToString() + " To: " + ToMsg + " From: " + FromSendMsg.ToString(), Globals.path_ComposeMessage);
                                     //GlobusFileHelper.AppendStringToTextfileNewLine("-------------------------------------------------------------------------------------------------------", Globals.path_ComposeMessage);
                                 }
                                 else if (ResponseStatusMsg.Contains("There was an unexpected problem that prevented us from completing your request."))
                                 {
                                     Log("[ " + DateTime.Now + " ] => [ Error In Message Posting ]");
                                     GlobusFileHelper.AppendStringToTextfileNewLine("Error In Message Posting", Globals.path_ComposeMessage);
                                 }
                                 else
                                 {
                                     Log("[ " + DateTime.Now + " ] => [ Failed In Message Posting ]");
                                     GlobusFileHelper.AppendStringToTextfileNewLine("Failed In Message Posting", Globals.path_ComposeMessage);
                                 }

                                 int delay = RandomNumberGenerator.GenerateRandom(mindelay, maxdelay);
                                 Log("[ " + DateTime.Now + " ] => [ Delay for : " + delay + " Seconds ]");
                                 Thread.Sleep(delay * 1000);
                              }
                             catch (Exception ex)
                             {
                                 Log("[ " + DateTime.Now + " ] => [ Error:" + ex.StackTrace + " ]");
                                 GlobusFileHelper.AppendStringToTextfileNewLine(" Error:" + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.path_ComposeMessage);
                             }
                         }
                         catch (Exception ex)
                         {
                             Console.WriteLine("ex.Message >>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + "  ex.Stack Trace >>> " + ex.StackTrace);
                         }
                     }



                 }
                 catch (Exception ex)
                 {
                     Console.WriteLine("ex.Message >>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + "  ex.Stack Trace >>> " + ex.StackTrace);
                 }
             }
             catch (Exception ex)
             {
                 Console.WriteLine("ex.Message >>> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace + "  ex.Stack Trace >>> " + ex.StackTrace);
             }
         } 
         #endregion

        #region Split
         public static IEnumerable<List<Dictionary<string, string>>> Split(List<Dictionary<string, string>> source, int splitNumber)
         {
             if (splitNumber <= 0)
             {
                 splitNumber = 1;
             }

             return source
             .Select((x, i) => new { Index = i, Value = x })
             .GroupBy(x => x.Index / splitNumber)
             .Select(x => x.Select(v => v.Value).ToList());
         } 
         #endregion
        
    }
}
