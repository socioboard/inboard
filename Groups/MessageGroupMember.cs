using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Data;
using BaseLib;

namespace Groups
{
    public class MessageGroupMember
    {
        #region Global declaration
        public string accountUser = string.Empty;
        public string accountPass = string.Empty;
        public string proxyAddress = string.Empty;
        public string proxyPort = string.Empty;
        public string proxyUserName = string.Empty;
        public string proxyPassword = string.Empty;

        public List<string> GrpMemSubjectlist = new List<string>();
        public List<string> GrpMemMessagelist = new List<string>();

        public static string TemporarySubject = string.Empty;
        public static List<string> lstSubjectReuse = new List<string>();

        #endregion

        #region MessageGroupMember
        public MessageGroupMember()
        {

        }
        #endregion

        #region MessageGroupMember
        public MessageGroupMember(string UserName, string Password, string ProxyAddress, string ProxyPort, string ProxyUserName, string ProxyPassword)
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

        #region PostFinalMsgGroupMember
        public void PostFinalMsgGroupMember(ref GlobusHttpHelper HttpHelper, Dictionary<string, string> SlectedContacts, string msg, string body, string UserName, string FromemailId, string FromEmailNam, int mindelay, int maxdelay)
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

                try
                {
                    string MessageText = string.Empty;
                    string PostedMessage = string.Empty;

                    int totalSelectedItems = SlectedContacts.Count;
                    Log("[ " + DateTime.Now + " ] => [ Total Selected Members : " + totalSelectedItems + " ]");

                    IEnumerable<IEnumerable<KeyValuePair<string, string>>> partitioned = SlectedContacts.Partition(50);


                    foreach (var item in partitioned)
                    {
                        string FString = string.Empty;
                        string Nstring = string.Empty;
                        string connId = string.Empty;
                        string FullName = string.Empty;
                        string ContactName = string.Empty;

                        string pageSource = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/home?trk=hb_tab_home_top"));
                        if (pageSource.Contains("csrfToken"))
                        {
                            csrfToken = pageSource.Substring(pageSource.IndexOf("csrfToken"), 50);
                            string[] Arr = csrfToken.Split('&');
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


                        try
                        {
                            foreach (KeyValuePair<string, string> itemChecked in item)
                            {
                                try
                                {


                                    string FName = string.Empty;
                                    string Lname = string.Empty;
                                    FullName = string.Empty;

                                    FName = itemChecked.Value.Split(' ')[0];
                                    Lname = itemChecked.Value.Split(' ')[1];

                                    FullName = FName + " " + Lname;
                                    try
                                    {
                                        ContactName = ContactName + "  :  " + FullName;

                                        if (ContactName.Contains("class="))
                                        {
                                            try
                                            {
                                                ContactName = ContactName.Substring(0, ContactName.IndexOf("class=")).Trim();
                                            }
                                            catch (Exception ex)
                                            {
                                            }
                                        }
                                    }
                                    catch { }
                                    Log("[ " + DateTime.Now + " ] => [ Adding Contact : " + FullName + " ]");

                                    string ToCd = itemChecked.Key;
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
                            }
                            Nstring += "}";

                            try
                            {

                                string PostMessage;
                                string ResponseStatusMsg;
                                Log("[ " + DateTime.Now + " ] => [ Message Sending Process Running.. ]");
                                PostMessage = "csrfToken=" + csrfToken + "&subject=" + Uri.EscapeDataString(msg.ToString()) + "&body=" + Uri.EscapeDataString(body.ToString()) + "&submit=Send+Message&showRecipeints=showRecipeints&fromName=" + FromEmailNam + "&fromEmail=" + FromemailId + "&connectionIds=" + connId + "&connectionNames=" + Uri.EscapeUriString(Nstring) + "&allowEditRcpts=true&addMoreRcpts=false&openSocialAppBodySuffix=&st=&viewerDestinationUrl=&goback=.smg_*1_*1_*1_*1_*1_*1_*1_*1_*1";
                                ResponseStatusMsg = HttpHelper.postFormData(new Uri("http://www.linkedin.com/msgToConns"), PostMessage);


                                if (ResponseStatusMsg.Contains("Your message was successfully sent."))
                                {
                                    Log("[ " + DateTime.Now + " ] => [ Subject Posted : " + msg + " ]");
                                    Log("[ " + DateTime.Now + " ] => [ Body Text Posted : " + body.ToString() + " ]");
                                    Log("[ " + DateTime.Now + " ] => [ Message Posted To  Accounts : " + item.Count() + " ] ");
                                    ReturnString = "Your message was successfully.";

                                    #region CSV
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
                                    string CSVHeader = "UserName" + "," + "Subject" + "," + "Body Text" + "," + "ContactName";
                                    string CSV_Content = UserName + "," + msg + "," + bdy.ToString() + "," + ContactName;
                                    CSVUtilities.ExportDataCSVFile(CSVHeader, CSV_Content, Globals.path_MessageSentGroupMember);

                                    #endregion

                                    //GlobusFileHelper.AppendStringToTextfileNewLine("Subject Posted : " + textMsg.Text.ToString(), Globals.path_MessageGroupMember);
                                    //GlobusFileHelper.AppendStringToTextfileNewLine("Body Text Posted : " + txtBody.Text.ToString(), Globals.path_MessageGroupMember);
                                    //GlobusFileHelper.AppendStringToTextfileNewLine("Message Posted To All Selected Accounts", Globals.path_MessageGroupMember);


                                }
                                else if (ResponseStatusMsg.Contains("There was an unexpected problem that prevented us from completing your request.") || ResponseStatusMsg.Contains("You are no longer authorized to message this"))
                                {
                                    //Log("Error In Message Posting");
                                    Log("[ " + DateTime.Now + " ] => [ There was an unexpected problem that prevented us from completing your request ]");
                                    //string CSVHeader = "UserName" + "," + "Subject" + "," + "Body Text" + "," + "ContactName";
                                    //string CSV_Content = UserName + "," + msg + "," + body.ToString() + "," + ContactName;
                                    //CSVUtilities.ExportDataCSVFile(CSVHeader, CSV_Content, Globals.path_MessageSentGroupMember);

                                    GlobusFileHelper.AppendStringToTextfileNewLine("Error In Message Posting", Globals.path_MessageGroupMember);

                                }

                            }
                            catch (Exception ex)
                            {
                                //Log("[ " + DateTime.Now + " ] => [ Error:" + ex.Message + "StackTrace --> >>>" + ex.StackTrace + " ]");

                                GlobusFileHelper.AppendStringToTextfileNewLine(" Error:" + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.path_MessageGroupMember);
                            }

                            int delay = RandomNumberGenerator.GenerateRandom(mindelay, maxdelay);
                            Log("[ " + DateTime.Now + " ] => [ Delay for : " + delay + " Seconds ]");
                            Thread.Sleep(delay * 1000);

                        }
                        catch (Exception ex)
                        {
                            GlobusFileHelper.AppendStringToTextfileNewLine(" Error:" + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.path_MessageGroupMember);
                        }
                    }

                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error -->  PostFinalMsgGroupMember() --> 1 --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> PostFinalMsgGroupMember() --> 1 --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.path_MessageGroupMember);
                }

            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error -->  PostFinalMsgGroupMember() --> 2 --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> PostFinalMsgGroupMember() --> 2 --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.path_MessageGroupMember);
            }

        }
        #endregion

        #region PostFinalMsgGroupMember_1By1
        public void PostFinalMsgGroupMember_1By1(ref GlobusHttpHelper HttpHelper, Dictionary<string, string> SlectedContacts, List<string> GrpMemSubjectlist, List<string> GrpMemMessagelist, string msg, string body, string UserName, string FromemailId, string FromEmailNam, string SelectedGrpName, string grpId, bool mesg_with_tag, bool msg_spintaxt, int mindelay, int maxdelay, bool preventMsgSameGroup, bool preventMsgWithoutGroup, bool preventMsgGlobal)
        {
            try
            {
                MsgGroupMemDbManager objMsgGroupMemDbMgr = new MsgGroupMemDbManager();

                string postdata = string.Empty;
                string postUrl = string.Empty;
                string ResLogin = string.Empty;
                string csrfToken = string.Empty;
                string sourceAlias = string.Empty;
                string ReturnString = string.Empty;
                string PostMsgSubject = string.Empty;
                string PostMsgBody = string.Empty;
                string FString = string.Empty;

                try
                {
                    string MessageText = string.Empty;
                    string PostedMessage = string.Empty;
                    string senderEmail = string.Empty;
                    string getComposeData = HttpHelper.getHtmlfromUrl(new Uri("https://www.linkedin.com/inbox/compose"));
                    try
                    {
                        int startindex = getComposeData.IndexOf("\"senderEmail\" value=\"");
                        if (startindex < 0)
                        {
                            startindex = getComposeData.IndexOf("\"senderEmail\",\"value\":\"");
                        }
                        string start = getComposeData.Substring(startindex).Replace("\"senderEmail\" value=\"", string.Empty).Replace("\"senderEmail\",\"value\":\"", string.Empty);
                        int endindex = start.IndexOf("\"/>");
                        if (endindex < 0)
                        {
                            endindex = start.IndexOf("\",\"");
                        }
                        string end = start.Substring(0, endindex).Replace("\"/>", string.Empty).Replace("\",\"", string.Empty);
                        senderEmail = end.Trim();
                    }
                    catch
                    { }
                    string pageSource = HttpHelper.getHtmlfromUrl1(new Uri("http://www.linkedin.com/home?trk=hb_tab_home_top"));
                    if (pageSource.Contains("csrfToken"))
                    {
                        try
                        {
                            csrfToken = pageSource.Substring(pageSource.IndexOf("csrfToken"), 50);
                            string[] Arr = csrfToken.Split('<');
                            csrfToken = Arr[0];
                            csrfToken = csrfToken.Replace("csrfToken", "").Replace("\"", string.Empty).Replace("value", string.Empty).Replace("cs", string.Empty).Replace("id", string.Empty).Replace("=", string.Empty).Replace("\n", string.Empty).Replace(">", string.Empty).Replace("<script typ", string.Empty);
                            csrfToken = csrfToken.Trim();
                        }
                        catch (Exception ex)
                        {

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

                        }
                    }

                    if (pageSource.Contains("goback="))
                    {
                        try
                        {
                        }
                        catch (Exception ex)
                        {

                        }
                    }

                    foreach (KeyValuePair<string, string> itemChecked in SlectedContacts)
                    {
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
                            string n_ame1 = string.Empty;

                            //grpId = itemChecked.Key.ToString();

                            try
                            {
                                // FName = itemChecked.Value.Split(' ')[0];
                                // Lname = itemChecked.Value.Split(' ')[1];
                                try
                                {
                                    n_ame1 = itemChecked.Value.Split(']')[1].Trim(); ;
                                }
                                catch
                                { }
                                if (string.IsNullOrEmpty(n_ame1))
                                {
                                    try
                                    {
                                        n_ame1 = itemChecked.Value;
                                    }
                                    catch
                                    { }
                                }
                                string[] n_ame = Regex.Split(n_ame1, " ");
                                FName = " " + n_ame[0];
                                Lname = n_ame[1];

                                if (!string.IsNullOrEmpty(n_ame[2]))
                                {
                                    Lname = Lname + n_ame[2];
                                }
                                if (!string.IsNullOrEmpty(n_ame[3]))
                                {
                                    Lname = Lname + n_ame[3];
                                }
                                if (!string.IsNullOrEmpty(n_ame[4]))
                                {
                                    Lname = Lname + n_ame[4];
                                }
                                if (!string.IsNullOrEmpty(n_ame[5]))
                                {
                                    Lname = Lname + n_ame[5];
                                }
                            }
                            catch (Exception ex)
                            {
                            }

                            try
                            {
                                ContactName = FName + " " + Lname;
                                ContactName = ContactName.Replace("%20", " ");
                            }
                            catch { }

                            Log("[ " + DateTime.Now + " ] => [ Adding Contact : " + ContactName + " ]");

                            string ToCd = itemChecked.Key;
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

                                Log("[ " + DateTime.Now + " ] => [ Message Sending Process Running.. ]");


                                if (msg_spintaxt == true)
                                {
                                    try
                                    {
                                        msg = GrpMemSubjectlist[RandomNumberGenerator.GenerateRandom(0, GrpMemSubjectlist.Count - 1)];
                                        body = GrpMemMessagelist[RandomNumberGenerator.GenerateRandom(0, GrpMemMessagelist.Count - 1)];
                                    }
                                    catch
                                    {
                                    }
                                }
                                try
                                {
                                    tempsubject = msg;
                                    tempBody = body;
                                }
                                catch
                                { }
                                if (mesg_with_tag == true)
                                {
                                    if (string.IsNullOrEmpty(FName))
                                    {
                                        tempBody = body.Replace("<Insert Name>", ContactName);
                                    }
                                    else
                                    {
                                        tempBody = GlobusSpinHelper.spinLargeText(new Random(), body);

                                        if (lstSubjectReuse.Count == GrpMemSubjectlist.Count)
                                        {
                                            lstSubjectReuse.Clear();
                                        }
                                        foreach (var itemSubject in GrpMemSubjectlist)
                                        {
                                            if (string.IsNullOrEmpty(TemporarySubject))
                                            {
                                                TemporarySubject = itemSubject;
                                                tempsubject = itemSubject;
                                                lstSubjectReuse.Add(itemSubject);
                                                break;
                                            }
                                            else if (!lstSubjectReuse.Contains(itemSubject))
                                            {
                                                TemporarySubject = itemSubject;
                                                tempsubject = itemSubject;
                                                lstSubjectReuse.Add(itemSubject);
                                                break;
                                            }
                                            else
                                            {
                                                continue;
                                            }
                                        }

                                        tempBody = tempBody.Replace("<Insert Name>", FName);
                                        tempsubject = tempsubject.Replace("<Insert Name>", FName);
                                    }
                                }
                                else
                                {
                                    if (string.IsNullOrEmpty(FName))
                                    {
                                        tempBody = body.Replace("<Insert Name>", ContactName);
                                    }
                                    else
                                    {
                                        tempBody = body.Replace("<Insert Name>", FName);
                                    }
                                }

                                if (SelectedGrpName.Contains(":"))
                                {
                                    try
                                    {
                                        string[] arrSelectedGrpName = Regex.Split(SelectedGrpName, ":");
                                        if (arrSelectedGrpName.Length > 1)
                                        {
                                            SelectedGrpName = arrSelectedGrpName[1];
                                        }
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                }




                                if (mesg_with_tag == true)
                                {
                                    tempBody = tempBody.Replace("<Insert Group>", SelectedGrpName);
                                    tempBody = tempBody.Replace("<Insert From Email>", FromEmailNam);
                                    tempBody = tempBody.Replace("<Insert Name>", string.Empty).Replace("<Insert Group>", string.Empty).Replace("<Insert From Email>", string.Empty);
                                }
                                else
                                {
                                    tempBody = tempBody.Replace("<Insert Group>", SelectedGrpName);
                                    tempBody = tempBody.Replace("<Insert From Email>", FromEmailNam);
                                    tempBody = tempBody.Replace("<Insert Name>", string.Empty).Replace("<Insert Group>", string.Empty).Replace("<Insert From Email>", string.Empty);
                                }

                                //Check BlackListed Accounts
                                try
                                {
                                    string Querystring = "Select ProfileID From tb_BlackListAccount Where ProfileID ='" + itemChecked.Key + "'";
                                    ds_bList = DataBaseHandler.SelectQuery(Querystring, "tb_BlackListAccount");
                                }
                                catch { }


                                if (preventMsgSameGroup)
                                {
                                    try
                                    {
                                        string Querystring = "Select MsgFrom,MsgToId,MsgTo,MsgGroupId,MsgGroupName,MsgSubject,MsgBody From tb_ManageMsgGroupMem Where MsgFrom ='" + UserName + "' and MsgGroupId = " + grpId + " and MsgToId = " + connId + "";
                                        ds = DataBaseHandler.SelectQuery(Querystring, "tb_ManageMsgGroupMem");
                                    }
                                    catch { }
                                }

                                if (preventMsgWithoutGroup)
                                {
                                    try
                                    {
                                        string Querystring = "Select MsgFrom,MsgToId,MsgTo,MsgGroupId,MsgGroupName,MsgSubject,MsgBody From tb_ManageMsgGroupMem Where MsgFrom ='" + UserName + "' and MsgToId = " + connId + "";
                                        ds = DataBaseHandler.SelectQuery(Querystring, "tb_ManageMsgGroupMem");
                                    }
                                    catch { }
                                }
                                if (preventMsgGlobal)
                                {
                                    try
                                    {
                                        string Querystring = "Select MsgFrom,MsgToId,MsgTo,MsgGroupId,MsgGroupName,MsgSubject,MsgBody From tb_ManageMsgGroupMem Where MsgToId = " + connId + "";
                                        ds = DataBaseHandler.SelectQuery(Querystring, "tb_ManageMsgGroupMem");
                                    }
                                    catch { }
                                }

                                try
                                {

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
                                                Log("[ " + DateTime.Now + " ] => [ User: " + ContactName + " is Added BlackListed List For Send Messages Pls Check ]");
                                                ResponseStatusMsg = "BlackListed";
                                            }
                                            else
                                            {
                                                PostMessage = "csrfToken=" + csrfToken + "&subject=" + Uri.EscapeDataString(msg.ToString()) + "&body=" + Uri.EscapeDataString(tempBody.ToString()) + "&submit=Send+Message&fromName=" + Uri.EscapeDataString(FromEmailNam) + "&showRecipeints=showRecipeints&fromEmail=" + FromemailId + "&connectionIds=" + connId + "&connectionNames=&allowEditRcpts=true&addMoreRcpts=false&openSocialAppBodySuffix=&st=&viewerDestinationUrl=&contentType=MEBC&groupID=" + grpId + "";
                                                ResponseStatusMsg = HttpHelper.postFormData(new Uri("http://www.linkedin.com/groupMsg"), PostMessage);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (ds_bList.Tables.Count > 0)
                                        {
                                            if (ds_bList.Tables[0].Rows.Count > 0)
                                            {

                                                Log("[ " + DateTime.Now + " ] => [ User: " + ContactName + " is Added BlackListed List For Send Messages Pls Check ]");
                                                ResponseStatusMsg = "BlackListed";
                                            }
                                            else
                                            {
                                                PostMessage = "csrfToken=" + csrfToken + "&subject=" + Uri.EscapeDataString(tempsubject.ToString()) + "&body=" + Uri.EscapeDataString(tempBody.ToString()) + "&submit=Send+Message&fromName=" + Uri.EscapeDataString(FromEmailNam) + "&showRecipeints=showRecipeints&fromEmail=" + FromemailId + "&connectionIds=" + connId + "&connectionNames=&allowEditRcpts=true&addMoreRcpts=false&openSocialAppBodySuffix=&st=&viewerDestinationUrl=&contentType=MEBC&groupID=" + grpId + "";
                                                ResponseStatusMsg = HttpHelper.postFormData(new Uri("http://www.linkedin.com/groupMsg"), PostMessage);
                                            }
                                        }
                                    }
                                }
                                catch (Exception)
                                {
                                    //PostMessage = "csrfToken=" + csrfToken + "&subject=" + Uri.EscapeDataString(msg.ToString()) + "&body=" + Uri.EscapeDataString(tempBody.ToString()) + "&submit=Send+Message&showRecipeints=showRecipeintsfromName=" + Uri.EscapeDataString(FromEmailNam) + "&fromEmail=" + FromemailId + "&connectionIds=" + connId + "&connectionNames=&allowEditRcpts=true&addMoreRcpts=false&openSocialAppBodySuffix=&st=&viewerDestinationUrl=&contentType=MEBC&groupID=" + grpId + "";
                                    //ResponseStatusMsg = HttpHelper.postFormData(new Uri("http://www.linkedin.com/groupMsg"), PostMessage);
                                }

                                if ((!ResponseStatusMsg.Contains("Your message was successfully sent.") && !ResponseStatusMsg.Contains("Already Sent")) && (!ResponseStatusMsg.Contains("Se ha enviado tu mensaje satisfactoriamente") && !ResponseStatusMsg.Contains("Ya ha sido enviada") && !ResponseStatusMsg.Contains("Uw bericht is verzonden")))
                                {

                                    if (ResponseStatusMsg.Contains("Already Sent") || (ResponseStatusMsg.Contains("Ya ha sido enviada") || (ResponseStatusMsg.Contains("BlackListed"))))
                                    {
                                        continue;
                                    }

                                    try
                                    {
                                        pageSource = HttpHelper.getHtmlfromUrl1(new Uri("https://www.linkedin.com/groups?viewMembers=&gid=" + grpId));

                                        if (pageSource.Contains("contentType="))
                                        {
                                            try
                                            {
                                                string contentType = pageSource.Substring(pageSource.IndexOf("contentType="), pageSource.IndexOf("&", pageSource.IndexOf("contentType=")) - pageSource.IndexOf("contentType=")).Replace("contentType=", string.Empty).Replace("contentType=", string.Empty).Trim();

                                                string pageSource2 = HttpHelper.getHtmlfromUrl1(new Uri("https://www.linkedin.com/groupMsg?displayCreate=&contentType=" + contentType + "&connId=" + connId + "&groupID=" + grpId + ""));

                                                PostMessage = "csrfToken=" + csrfToken + "&subject=" + Uri.EscapeDataString(msg.ToString()) + "&body=" + Uri.EscapeDataString(tempBody.ToString()) + "&submit=Send+Message&showRecipeints=showRecipeints&fromName=" + FromEmailNam + "&fromEmail=" + FromemailId + "&connectionIds=" + connId + "&connectionNames=" + Uri.EscapeUriString(Nstring) + "&allowEditRcpts=true&addMoreRcpts=false&openSocialAppBodySuffix=&st=&viewerDestinationUrl=&contentType=" + contentType + "&groupID=" + grpId + "";

                                            }
                                            catch (Exception ex)
                                            {
                                            }
                                        }


                                        ResponseStatusMsg = HttpHelper.postFormData(new Uri("https://www.linkedin.com/groupMsg"), PostMessage);
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                }

                                if ((ResponseStatusMsg.Contains("Your message was successfully sent.")) || (ResponseStatusMsg.Contains("Se ha enviado tu mensaje satisfactoriamente") || (ResponseStatusMsg.Contains("Uw bericht is verzonden"))))
                                {
                                    Log("[ " + DateTime.Now + " ] => [ Subject Posted : " + tempsubject + " ]");
                                    Log("[ " + DateTime.Now + " ] => [ Body Text Posted : " + tempBody.ToString() + " ]");
                                    Log("[ " + DateTime.Now + " ] => [ Message Posted To Account: " + ContactName + " ]");
                                    ReturnString = "Your message was successfully sent.";

                                    #region CSV
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
                                    string CSV_Content = UserName + "," + tempsubject + "," + bdy.ToString() + "," + ContactName;
                                    CSVUtilities.ExportDataCSVFile(CSVHeader, CSV_Content, Globals.path_MessageSentGroupMember);

                                    try
                                    {
                                        objMsgGroupMemDbMgr.InsertMsgGroupMemData(UserName, Convert.ToInt32(connId), ContactName, Convert.ToInt32(grpId), SelectedGrpName, msg, tempBody);
                                    }
                                    catch { }

                                    #endregion

                                }
                                else if (ResponseStatusMsg.Contains("There was an unexpected problem that prevented us from completing your request"))
                                {
                                    Log("[ " + DateTime.Now + " ] => [ There was an unexpected problem that prevented us from completing your request ! ]");
                                    GlobusFileHelper.AppendStringToTextfileNewLine("Error In Message Posting", Globals.path_MessageGroupMember);
                                }
                                else if (ResponseStatusMsg.Contains("You are no longer authorized to message this"))
                                {
                                    Log("[ " + DateTime.Now + " ] => [ You are no longer authorized to message this ! ]");
                                    GlobusFileHelper.AppendStringToTextfileNewLine("Error In Message Posting", Globals.path_MessageGroupMember);
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
                                    string CSV_Content = UserName + "," + msg + "," + bdy.ToString() + "," + ContactName;
                                    CSVUtilities.ExportDataCSVFile(CSVHeader, CSV_Content, Globals.path_MessageAlreadySentGroupMember);

                                    Log("[ " + DateTime.Now + " ] => [ Message Not Posted To Account: " + ContactName + " because it has sent the same message]");
                                }
                                else
                                {
                                    Log("[ " + DateTime.Now + " ] => [ Error In Message Posting ]");
                                    GlobusFileHelper.AppendStringToTextfileNewLine("Error In Message Posting", Globals.path_MessageGroupMember);
                                }

                                int delay = RandomNumberGenerator.GenerateRandom(mindelay, maxdelay);
                                Log("[ " + DateTime.Now + " ] => [ Delay for : " + delay + " Seconds ]");
                                Thread.Sleep(delay * 1000);

                            }
                            catch (Exception ex)
                            {
                                //Log("[ " + DateTime.Now + " ] => [ Error:" + ex.Message + "StackTrace --> >>>" + ex.StackTrace + " ]");
                                GlobusFileHelper.AppendStringToTextfileNewLine(" Error:" + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.path_MessageGroupMember);
                            }
                        }
                        catch (Exception ex)
                        {
                            //Log("[ " + DateTime.Now + " ] => [ Error:" + ex.Message + "StackTrace --> >>>" + ex.StackTrace);
                            GlobusFileHelper.AppendStringToTextfileNewLine(" Error:" + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.path_MessageGroupMember);
                        }
                    }

                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error -->  PostFinalMsgGroupMember() --> 1 --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                    GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> PostFinalMsgGroupMember() --> 1 --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.path_MessageGroupMember);
                }


            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error -->  PostFinalMsgGroupMember() --> 2 --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine("DateTime :- " + DateTime.Now + " :: Error --> PostFinalMsgGroupMember() --> 2 --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_LinkedinComposeMessageErrorLogs);
            }

        }
        #endregion
    }
}
