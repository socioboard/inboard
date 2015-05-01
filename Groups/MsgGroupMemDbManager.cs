using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseLib;

namespace Groups
{

    class MsgGroupMemDbManager
    {
        #region InsertScarppRecordData
        public void InsertMsgGroupMemData(string msgFrom, int msgToID, string msgTo, int msgGroupID, string msgGroupName, string msgSub, string msgBody)
        {
            msgFrom = msgFrom.Replace("'", "''");
            msgTo = msgTo.Replace("'", "''");
            msgGroupName = msgGroupName.Replace("'", "''");
            msgSub = msgSub.Replace("'", "''");
            msgBody = msgBody.Replace("'", "''");

            try
            {
                string strQuery = "INSERT INTO tb_ManageMsgGroupMem (MsgFrom,MsgToId,MsgTo,MsgGroupId,MsgGroupName,MsgSubject,MsgBody,DateTime) VALUES('" + msgFrom + "'," + msgToID + ",'" + msgTo + "'," + msgGroupID + ",'" + msgGroupName + "','" + msgSub + "','" + msgBody + "','" + DateTime.Now + "')";
                DataBaseHandler.InsertQuery(strQuery, "tb_ManageMsgGroupMem");
            }
            catch (Exception)
            { }
        }
        #endregion

    }
}
