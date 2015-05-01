using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InBoardPro;
using System.Data;
using BaseLib;

namespace InBoardPro
{
    class LinkedinScrappDbManager
    {

        #region InsertScarppRecordData
        public void InsertScarppRecordData(string PostalCode, string Distance, string Industry, string LastName, int Result)
        {
            try
            {
                string strQuery = "INSERT INTO tb_LinkedinScrapResult (PostalCode,Distance,Industry,LastName,NumberOfResult) VALUES('" + PostalCode + "','" + Distance + "','" + Industry + "','" + LastName + "'," + Result + ") ";
                DataBaseHandler.InsertQuery(strQuery, "tb_LinkedinScrapResult");
            }
            catch (Exception)
            {
                //UpdateSettingData(Upmodule, Upfiletype, Upfilepath);
            }

        } 
        #endregion

        #region InsertIntoLinkedInSearchUrlResult
        public void InsertIntoLinkedInSearchUrlResult(string PostalCode, string Distance, string Industry, string LastName, string UserName, string Password, string Proxy, string ProxyPwd, int Result, string LdUrl)
        {
            try
            {
                string strQuery = "INSERT INTO tb_LinkedinSearchUrlResult(PostalCode,Distance,Industry,LastName,UserName,Password,proxyToUse,ProxyPwd,ResultNumber,LinkedInUrl,Status) VALUES('" + PostalCode + "','" + Distance + "','" + Industry + "','" + LastName + "','" + UserName + "','" + Password + "','" + Proxy + "','" + ProxyPwd + "'," + Result + ",'" + LdUrl + "','0') ";
                DataBaseHandler.InsertQuery(strQuery, "tb_LinkedinSearchUrlResult");
            }
            catch (Exception)
            {
                //UpdateSettingData(Upmodule, Upfiletype, Upfilepath);
            }

        } 
        #endregion

        #region Deletetb_GroupUrlforSpecificUser
        public void Deletetb_GroupUrlforSpecificUser(string user)
        {
            try
            {
                string strTable = "tb_GroupUrl";
                string strQuery = "DELETE FROM tb_GroupUrl where UserName='" + user + "'";
                DataBaseHandler.DeleteQuery(strQuery, strTable);
            }
            catch (Exception)
            {
            }
        } 
        #endregion

        #region Selecttb_GroupUrlDistinctUser
        public DataTable Selecttb_GroupUrlDistinctUser()
        {
            try
            {
                string strQuery = "SELECT Distinct UserName FROM tb_GroupUrl ";
                DataSet ds = DataBaseHandler.SelectQuery(strQuery, "tb_GroupUrl");
                DataTable dt = ds.Tables["tb_GroupUrl"];
                return dt;
            }
            catch (Exception)
            {
                return new DataTable();
            }
        } 
        #endregion

        #region Updatetb_LinkedinSearchUrlResult
        public void Updatetb_LinkedinSearchUrlResult(string PostalCode, string Distance, string Industry, string LastName, string UserName, string Password, string Proxy, string ProxyPwd, int Result, string LdUrl)
        {
            try
            {
                string strTable = "tb_LinkedinSearchUrlResult";
                string strQuery = "UPDATE tb_LinkedinSearchUrlResult SET Status='1' WHERE PostalCode='" + PostalCode + "' and Distance='" + Distance + "' and Industry='" + Industry + "' and LastName='" + LastName + "' and UserName='" + UserName + "' and Password='" + Password + "' and ProxyToUse='" + Proxy + "' and ProxyPwd='" + ProxyPwd + "' and LinkedInUrl='" + LdUrl + "'";
                DataBaseHandler.UpdateQuery(strQuery, strTable);
            }
            catch (Exception)
            {

            }
        } 
        #endregion
    }
}
