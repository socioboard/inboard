#region namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
#endregion

namespace BaseLib
{
    public class clsLDAccount
    {
        #region global declarations
        string strUsernaem=string.Empty;
        string strPassword=string.Empty;
        string strProxiaddress =string.Empty;
        string strProxyport=string.Empty;
        string strProxyName=string.Empty;
        string strProxypassword=string.Empty;
        string strProfileStatus = string.Empty;
        #endregion

        #region List<string> SelectAccouts()
        public List<string> SelectAccouts()
        {
            try
            {
                List<string> lstAccount = new List<string>();
                string strQuery = "SELECT * FROM tb_LinkedInAccount";

                DataSet ds = DataBaseHandler.SelectQuery(strQuery, "tb_LinkedInAccount");

                DataTable dt = ds.Tables["tb_LinkedInAccount"];

                foreach (DataRow row in dt.Rows)
                {

                    string str = null;
                    foreach (var item in row.ItemArray)
                    {
                        str = str + item.ToString() + ":";
                    }
                    lstAccount.Add(str);
                }

                return lstAccount;
            }
            catch (Exception)
            {

                return new List<string>();
            }
        }
        #endregion

        #region DataTable SelectAccoutsForGridView()
        public DataTable SelectAccoutsForGridView()
        {
            try
            {
                List<string> lstAccount = new List<string>();
                string strQuery = string.Empty;
                strQuery = "SELECT * FROM tb_LinkedInAccount";

                //if (Globals.IsFreeVersion)
                //{
                //    strQuery = "SELECT * FROM tb_LinkedInAccount LIMIT 5";
                //}
                //else
                //{
                //    strQuery = "SELECT * FROM tb_LinkedInAccount";
                //}

                DataSet ds = DataBaseHandler.SelectQuery(strQuery, "tb_LinkedInAccount");

                DataTable dt = ds.Tables["tb_LinkedInAccount"];

                return dt;
            }
            catch (Exception)
            {
                return new DataTable();
            }
        }
        #endregion

        #region DataTable SelectAccoutsForGridView(string table)
        public DataTable SelectAccoutsForGridView(string table)
        {
            try
            {
                List<string> lstAccount = new List<string>();
                string strQuery = "SELECT * FROM "+table+"";

                DataSet ds = DataBaseHandler.SelectQuery(strQuery, table);

                DataTable dt = ds.Tables[table];

                return dt;
            }
            catch (Exception)
            {
                return new DataTable();
            }
        }
        #endregion

        #region InsertUpdateFBAccount
        public void InsertUpdateFBAccount(string Username,string password,string proxiaddress ,string proxyport,string proxyName,string proxypassword,string friendcount,string profilename)
        {
            try
            {
                this.strUsernaem=Username;
                this.strPassword=password;
                this.strProxiaddress=proxiaddress;
                this.strProxyport=proxyport;
                this.strProxyName=proxyName;
                this.strProxypassword=proxypassword;
                this.strProfileStatus = "";

                string strQuery = "INSERT INTO tb_LinkedInAccount VALUES ('" + Username + "','" + password + "','" + proxiaddress + "','" + proxyport + "','" + proxyName + "','"+proxypassword+ "','"+friendcount+ "','"+profilename+"','"+strProfileStatus+"') ";

                DataBaseHandler.InsertQuery(strQuery, "tb_LinkedInAccount");
            }
            catch (Exception)
            {
                try
                {
                    UpdateFBAccount(strUsernaem, strPassword, strProxiaddress, strProxyport, strProxyName, strProxypassword);
                }
                catch { }
                 
            }
        }
        #endregion

        #region UpdateFBAccount
        public void UpdateFBAccount(string Usernaem, string password, string proxiaddress, string proxyport, string proxyName, string proxypassword)
        {
            try
            {
                string strTable = "tb_LinkedInAccount";
                string strQuery = "UPDATE tb_LinkedInAccount SET Password='" + password + "', ProxyAddress='" + proxiaddress + "', ProxyPort='" + proxyport + "', ProxyUserName='" + proxyName + "', ProxyPassword='" + proxypassword + "' WHERE UserName='" + Usernaem + "'";

                DataBaseHandler.UpdateQuery(strQuery, strTable);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion


        #region InsertSerachWithInviteRecord
        public void InsertSerachWithInviteRecord(string Username, string KeyWord, string InviteSentTo, string InvitationUrl , string InvitationUserId)
        {
            try
            {
                String strQuery = "INSERT INTO  tb_SearchWithInvite(UserName,SearchKeyword, InvitationSendTo, InvitationUrl,InvitationId,InviteDate)"
           + " VALUES ('" + Username + "','" + KeyWord + "','" + InviteSentTo + "','" + InvitationUrl + "','" + InvitationUserId + "','"+DateTime.Today.ToString()+"')";
                DataBaseHandler.InsertQuery(strQuery, "tb_SearchWithInvite");
            }
            catch (Exception)
            {
               
            }
        }
        #endregion
    }
}
