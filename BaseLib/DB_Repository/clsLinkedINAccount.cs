using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BaseLib;

namespace BaseLib.DB_Repository
{
    public class clsLinkedINAccount
    {
        string strUsernaem=string.Empty;
        string strPassword=string.Empty;
        string strProxiaddress =string.Empty;
        string strProxyport=string.Empty;
        string strProxyName=string.Empty;
        string strProxypassword=string.Empty;
        string strProfileStatus = string.Empty;
        
        public List<string> SelectAccouts()
        {
            try
            {
                List<string> lstAccount = new List<string>();
                string strQuery = "SELECT * FROM tb_FBAccount";

                DataSet ds = DataBaseHandler.SelectQuery(strQuery, "tb_FBAccount");

                DataTable dt = ds.Tables["tb_FBAccount"];

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

        public DataTable SelectAccoutsForGridView()
        {
            try
            {
                List<string> lstAccount = new List<string>();
                string strQuery = "SELECT * FROM tb_LinkedInAccount";

                DataSet ds = DataBaseHandler.SelectQuery(strQuery, "tb_LinkedInAccount");

                DataTable dt = ds.Tables["tb_LinkedInAccount"];

                return dt;
            }
            catch (Exception)
            {
                return new DataTable();
            }
        }

        public void InsertFBAccount(string Usernaem,string password,string proxiaddress ,string proxyport,string proxyName,string proxypassword,string friendcount,string profilename)
        {
            try
            {
                this.strUsernaem=Usernaem;
                this.strPassword=password;
                this.strProxiaddress=proxiaddress;
                this.strProxyport=proxyport;
                this.strProxyName=proxyName;
                this.strProxypassword=proxypassword;
                this.strProfileStatus = "";

                string strQuery = "INSERT INTO tb_FBAccount VALUES ('" + Usernaem + "','" + password + "','" + proxiaddress + "','" + proxyport + "','" + proxyName + "','"+proxypassword+ "','"+friendcount+ "','"+profilename+"','"+strProfileStatus+"') ";

                DataBaseHandler.InsertQuery(strQuery, "tb_FBAccount");
            }
            catch (Exception)
            {
                try
                {
                    UpdateFBAccount(strUsernaem, strPassword, strProxiaddress, strProxyport, strProxyName, strProxypassword);
                }
                catch (Exception)
                {
                    
                    throw;
                } 
                 
            }
        }
        public void UpdateFBAccount(string Usernaem, string password, string proxiaddress, string proxyport, string proxyName, string proxypassword)
        {
            try
            {
                string strTable = "tb_FBAccount";
                string strQuery = "UPDATE tb_FBAccount SET Password='" + password + "', ProxyAddress='" + proxiaddress + "', ProxyPort='" + proxyport + "', ProxyUserName='" + proxyName + "', ProxyPassword='" + proxypassword + "' WHERE UserName='" + Usernaem+"'";

                DataBaseHandler.UpdateQuery(strQuery, strTable);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void UpdatePasswordForAccount(string userName,  string newPassword)
        {
            try
            {
                string UpdateQuery = "update tb_LinkedInAccount set  Password='" + newPassword + "' WHERE UserName='" + userName + "';";
                DataBaseHandler.UpdateQuery(UpdateQuery, "tb_LinkedInAccount");
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                //AlibabaScaper.GlobusFileHelper.AppendStringToTextfileNewLine("InsertQuery error" +ex+ ex.StackTrace, MainFrm.Error);
            }

        }
    }
}
