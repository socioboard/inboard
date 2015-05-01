using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseLib;

namespace ComposeMessage
{
    class ComposeMsgDbManager
    {

        #region InsertComposeMsgData
        public void InsertComposeMsgData(string msgFrom, int msgToID, string msgTo, string msgSub, string msgBody)
        {
            msgFrom = msgFrom.Replace("'", "''");
            msgTo = msgTo.Replace("'", "''").Replace(":",string.Empty);
            msgSub = msgSub.Replace("'", "''");
            msgBody = msgBody.Replace("'", "''");

            try
            {
                string strQuery = "INSERT INTO tb_ManageComposeMsg (MsgFrom,MsgToId,MsgTo,MsgSubject,MsgBody,DateTime) VALUES('" + msgFrom + "'," + msgToID + ",'" + msgTo + "','" + msgSub + "','" + msgBody + "','" + DateTime.Now + "')";
                DataBaseHandler.InsertQuery(strQuery, "tb_ManageComposeMsg");
            }
            catch (Exception)
            { }
        }
        #endregion
    }
}
