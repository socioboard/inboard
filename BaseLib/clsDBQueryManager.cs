using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BaseLib
{
    public class clsDBQueryManager
    {
        #region global declaration
        string Upmodule = string.Empty;
        string Upfiletype = string.Empty;
        string Upfilepath = string.Empty;
        string DeathByCaptcha = string.Empty; 
        #endregion

        /// <summary>
        /// Inserts Settings in DataBase
        /// Updates if Settings already present
        /// </summary>
        /// <param name="module"></param>
        /// <param name="filetype"></param>
        /// <param name="filepath"></param>
        #region InsertOrUpdateSetting
        public void InsertOrUpdateSetting(string module, string filetype, string filepath)
        {
            try
            {
                this.Upmodule = module;
                this.Upfiletype = filetype;
                this.Upfilepath = filepath;

                string Upmodule = module;
                string UPfiletype = filetype;
                string strQuery = "INSERT INTO tb_Setting VALUES ('" + module + "','" + filetype + "','" + filepath + "') ";

                DataBaseHandler.InsertQuery(strQuery, "tb_Setting");
            }
            catch (Exception)
            {
                UpdateSettingData(Upmodule, Upfiletype, Upfilepath);
            }

        } 
        #endregion

        #region InsertOrUpdateScrapeSetting
        public void InsertOrUpdateScrapeSetting(string userId, string username, string TweetId)
        {
            try
            {
                string strQuery = "INSERT INTO tb_ScrapeData (Userid , Username , TweetId) VALUES ('" + userId + "' , '" + username + "' , '" + TweetId + "')";
                DataBaseHandler.InsertQuery(strQuery, "tb_ScrapeData");
            }
            catch (Exception ex)
            {
                //BaseLib.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> InsertUpdateSetting --> ScrapeData --> clsDBQueryManager --> " + ex.Message + "StackTrace --> >>>" + ex.StackTrace, Globals.Path_TwtErrorLogs);
            }

        } 
        #endregion


        //public void InsertQueryForEndorsement(string insertQuery)
        //{
        //    try
        //    {
        //        DataBaseHandler.InsertQuery(insertQuery, "tb_endorsement");
        //    }
        //    catch { }
        //}


        #region DeleteScrapeSettings
        public void DeleteScrapeSettings()
        {
            try
            {
                string strQuery = "DELETE From tb_ScrapeData";
                DataBaseHandler.InsertQuery(strQuery, "tb_ScrapeData");
            }
            catch (Exception ex)
            {

            }
        } 
        #endregion

        #region SelectSettingData
        public DataTable SelectSettingData()
        {
            try
            {

                string strQuery = "SELECT * FROM tb_setting";

                DataSet ds = DataBaseHandler.SelectQuery(strQuery, "tb_setting");

                DataTable dt = ds.Tables["tb_setting"];
                return dt;
            }
            catch (Exception)
            {

                return new DataTable();
            }
        } 
        #endregion

        #region SelectPrivateProxyData
        public DataSet SelectPrivateProxyData()
        {
            try
            {
                string strQuery = "SELECT * FROM tb_Proxies WHERE IsPublic = 1";

                DataSet ds = DataBaseHandler.SelectQuery(strQuery, "tb_Proxies");

                return ds;
            }
            catch (Exception)
            {
                return new DataSet();
            }
        } 
        #endregion

        #region SelectPublicProxyData
        public DataSet SelectPublicProxyData()
        {
            try
            {
                string strQuery = "SELECT * FROM tb_Proxies WHERE IsPublic = 0";

                DataSet ds = DataBaseHandler.SelectQuery(strQuery, "tb_Proxies");

                return ds;
            }
            catch (Exception)
            {
                return new DataSet();
            }
        } 
        #endregion

        #region DeletePublicProxyData
        public void DeletePublicProxyData()
        {
            try
            {
                string strQuery = "DELETE From tb_Proxies WHERE IsPublic = 0";
                DataBaseHandler.DeleteQuery(strQuery, "tb_Proxies");
            }
            catch (Exception ex)
            {

            }
        } 
        #endregion

        #region DeletePrivateProxyData
        public void DeletePrivateProxyData()
        {
            try
            {
                string strQuery = "DELETE From tb_Proxies WHERE IsPublic = 1";
                DataBaseHandler.DeleteQuery(strQuery, "tb_Proxies");
                
            }
            catch (Exception ex)
            {

            }
        } 
        #endregion

        #region UpdateSettingData
        public void UpdateSettingData(string module, string filetype, string filepath)
        {
            try
            {
                string strTable = "tb_Setting";
                string strQuery = "UPDATE tb_Setting SET Module='" + module + "', FilePath='" + filepath + "' WHERE FileType='" + filetype + "'";

                DataBaseHandler.UpdateQuery(strQuery, strTable);
            }
            catch (Exception)
            {

            }
        } 
        #endregion

        #region InsertDBCData
        public void InsertDBCData(string username, string DeathByCaptcha, string password)
        {
            try
            {

                string strQuery = "INSERT INTO tb_Setting VALUES ('" + username + "','" + DeathByCaptcha + "','" + password + "') ";

                DataBaseHandler.InsertQuery(strQuery, "tb_Setting");
            }
            catch (Exception)
            {
                UpdateSettingData(Upmodule, Upfiletype, Upfilepath);
            }
        } 
        #endregion

        #region DeleteDBCDecaptcherData
        public void DeleteDBCDecaptcherData(string strDeathByCaptcha)
        {
            try
            {

                string strTable = "tb_Setting";
                string strQuery = "DELETE FROM tb_Setting WHERE FileType='" + strDeathByCaptcha + "'";

                DataBaseHandler.DeleteQuery(strQuery, strTable);
            }
            catch (Exception)
            {
            }
        } 
        #endregion

        #region InsertDecaptcherData
        public void InsertDecaptcherData(string server, string port, string username, string password, string Decaptcher)
        {
            try
            {

                string strQuery = "INSERT INTO tb_Setting VALUES ('" + server + "<:>" + port + "','" + Decaptcher + "','" + username + "<:>" + password + "') ";

                DataBaseHandler.InsertQuery(strQuery, "tb_Setting");
            }
            catch (Exception)
            {
                UpdateSettingData(Upmodule, Upfiletype, Upfilepath);
            }
        } 
        #endregion

        #region SelectFollowData
        public DataTable SelectFollowData(string useremail)
        {
            try
            {

                string strQuery = "SELECT * FROM tb_Follow where username='" + useremail + "'";

                DataSet ds = DataBaseHandler.SelectQuery(strQuery, "tb_Setting");

                DataTable dt = ds.Tables["tb_Setting"];
                return dt;
            }
            catch (Exception)
            {

                return new DataTable();
            }
        } 
        #endregion

        #region InsertUpdateFollowTable
        public void InsertUpdateFollowTable(string useremail, string following_id, string following_username)
        {
            try
            {
                string strDateTime = DateTime.Now.ToString();
                string strQuery = "INSERT INTO tb_Follow (username, following_id, following_username, DateFollowed) VALUES ('" + useremail + "' , '" + following_id + "' , '" + following_username + "' , '" + strDateTime + "')";
                DataBaseHandler.InsertQuery(strQuery, "tb_Follow");
            }
            catch (Exception)
            {
                string strQuery = "UPDATE tb_Follow SET username='" + useremail + "', following_id='" + following_id + "', following_username='" + following_username + "' WHERE username='" + useremail + "'";
                DataBaseHandler.UpdateQuery(strQuery, "tb_Follow");
            }
        } 
        #endregion

        #region SelectFollowData_List
        public List<string> SelectFollowData_List(string useremail)
        {
            List<string> lst_Data = new List<string>();
            try
            {

                string strQuery = "SELECT * FROM tb_Follow where username='" + useremail + "'";

                DataSet ds = DataBaseHandler.SelectQuery(strQuery, "tb_Setting");

                DataTable dt = ds.Tables["tb_Setting"];

                foreach (DataRow dRow in dt.Rows)
                {
                    string following_id = dRow["following_id"].ToString() + "";
                    string following_username = dRow["following_id"].ToString() + "";
                    lst_Data.Add(following_id);
                    lst_Data.Add(following_username);
                }
                lst_Data = lst_Data.Distinct().ToList();
            }
            catch (Exception)
            {

            }
            return lst_Data;
        } 
        #endregion

        #region InsertUpdateTBScheduler
        public void InsertUpdateTBScheduler(string module, string strDateTime, string IsScheduledDaily)
        {
            try
            {
                string InsertQuery = "Insert into tb_Scheduler_Module (Module, ScheduledDateTime, IsScheduledDaily) VALUES ('" + module + "','" + strDateTime + "','" + IsScheduledDaily + "')";
                DataBaseHandler.InsertQuery(InsertQuery, "tb_Scheduler_Module");
            }
            catch (Exception)
            {
                string UpdateQuery = "UPDATE tb_Scheduler_Module SET ScheduledDateTime='" + strDateTime + "', IsScheduledDaily='" + IsScheduledDaily + "', IsAccomplished='" + "0" + "' WHERE Module='" + module + "'";
                DataBaseHandler.UpdateQuery(UpdateQuery, "tb_Scheduler_Module");
            }

        } 
        #endregion

        #region UpdateTBScheduler
        public void UpdateTBScheduler(string module)
        {
            try
            {
                string strTable = "tb_Setting";
                string strQuery = "UPDATE tb_Scheduler_Module SET IsAccomplished='" + "1" + "' WHERE Module='" + module + "'";

                DataBaseHandler.UpdateQuery(strQuery, strTable);

                //Increase 1 day if IsScheduledDaily
                {
                    string selectQuery = "SELECT * FROM tb_Scheduler_Module where Module='" + module + "' and IsAccomplished='1'";
                    DataSet ds = DataBaseHandler.SelectQuery(selectQuery, strTable);

                    DataTable dt = ds.Tables["tb_Setting"];

                    foreach (DataRow dRow in dt.Rows)
                    {
                        string strIsScheduledDaily = dRow["IsScheduledDaily"].ToString();
                        if (strIsScheduledDaily == "1")
                        {
                            string scheduledTime = dRow["ScheduledDateTime"].ToString();

                            DateTime dt_scheduledTime = DateTime.Parse(scheduledTime);

                            DateTime dt_nextscheduledTime = dt_scheduledTime.AddDays(1);

                            string nextscheduledTime = dt_nextscheduledTime.ToString();

                            string nextUpdateQuery = "UPDATE tb_Scheduler_Module SET ScheduledDateTime='" + nextscheduledTime + "', IsAccomplished='0" + "' WHERE Module='" + module + "'";

                            DataBaseHandler.UpdateQuery(nextUpdateQuery, "tb_Setting");
                        }

                    }
                }

                //Reschedule
                //{
                //    string selectQuery = "SELECT * FROM tb_Scheduler_Module where IsAccomplished='" + "1" + "'";
                //    DataSet ds = DataBaseHandler.SelectQuery(selectQuery, strTable);

                //    DataTable dt = ds.Tables["tb_Setting"];

                //    foreach (DataRow dRow in dt.Rows)
                //    {
                //        string strIsScheduledDaily = dRow["IsScheduledDaily"].ToString();
                //        if (strIsScheduledDaily == "1")
                //        {
                //            string scheduledTime = dRow["ScheduledDateTime"].ToString();

                //            DateTime dt_scheduledTime = DateTime.Parse(scheduledTime);

                //            DateTime dt_Now = DateTime.Now;

                //            //if (dt_Now.Day - dt_scheduledTime.Day)
                //            //{

                //            //}

                //            DateTime dt_nextscheduledTime = dt_scheduledTime.AddDays(1);

                //            string nextscheduledTime = dt_nextscheduledTime.ToString();

                //            string nextUpdateQuery = "UPDATE tb_Scheduler_Module SET ScheduledDateTime='" + nextscheduledTime + "' WHERE Module='" + module + "'";

                //            DataBaseHandler.UpdateQuery(nextUpdateQuery, "tb_Setting");
                //        }

                //    }
                //}
            }
            catch (Exception)
            {

            }
        } 
        #endregion

        #region SelectAllFromTBScheduler
        public DataTable SelectAllFromTBScheduler()
        {
            try
            {
                string strQuery = "SELECT * FROM tb_Scheduler_Module";

                DataSet ds = DataBaseHandler.SelectQuery(strQuery, "tb_Setting");

                DataTable dt = ds.Tables["tb_Setting"];
                return dt;
            }
            catch { return new DataTable(); }
        } 
        #endregion

        #region SelectUnaccomplishedFromTBScheduler
        public DataTable SelectUnaccomplishedFromTBScheduler()
        {
            try
            {
                string strQuery = "SELECT * FROM tb_Scheduler_Module where IsAccomplished='" + "0" + "'";

                DataSet ds = DataBaseHandler.SelectQuery(strQuery, "tb_Setting");

                DataTable dt = ds.Tables["tb_Setting"];
                return dt;
            }
            catch { return new DataTable(); }
        } 
        #endregion

        #region SelectUnaccomplishedPastScheduledTimeFromTBScheduler
        public List<string> SelectUnaccomplishedPastScheduledTimeFromTBScheduler()
        {
           List<string> listModules = new List<string>();

            try
            {
                string strQuery = "SELECT * FROM tb_Scheduler_Module where IsAccomplished='" + "0" + "'";

                DataSet ds = DataBaseHandler.SelectQuery(strQuery, "tb_Setting");

                DataTable dt = ds.Tables["tb_Setting"];

                foreach (DataRow dRow in dt.Rows)
                {
                    string scheduledTime = dRow["ScheduledDateTime"].ToString();

                    DateTime dt_scheduledTime = DateTime.Parse(scheduledTime);

                    if (dt_scheduledTime.Day == DateTime.Now.Day)
                    {
                        if (DateTime.Now >= dt_scheduledTime)
                        {
                            listModules.Add(dRow["Module"].ToString());
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
            return listModules;
        } 
        #endregion

        #region DeleteAccomplishedFromTBScheduler
        public void DeleteAccomplishedFromTBScheduler()
        {
            try
            {
                string strQuery = "Delete FROM tb_Scheduler_Module where IsAccomplished='" + "1" + "'";
                DataBaseHandler.DeleteQuery(strQuery, "tb_Scheduler_Module");
            }
            catch (Exception)
            {

            }

        } 
        #endregion

        #region InsertUserNameId
        public void InsertUserNameId(string Username, string UserId)
        {
            try
            {
                string strQuery = "INSERT INTO tb_UsernameDetails VALUES ('" + Username + "','" + UserId + "') ";
                DataBaseHandler.InsertQuery(strQuery, "tb_UsernameDetails");
            }
            catch (Exception)
            {

            }
        } 
        #endregion

        #region GetUserId
        public DataSet GetUserId(string username)
        {
            DataSet ds = new DataSet();

            try
            {
                string strQuery = "SELECT Userid FROM tb_UsernameDetails WHERE Username = '" + username + "' ";

                ds = DataBaseHandler.SelectQuery(strQuery, "tb_UsernameDetails");

                return ds;
            }
            catch (Exception ex)
            {
                return new DataSet();
            }
        } 
        #endregion

        #region InsertDataRetweet
        public void InsertDataRetweet(string Username, string RetweetUsername, string Tweet)
        {
            try
            {
                Tweet = StringEncoderDecoder.Encode(Tweet);
                string strQuery = "INSERT INTO tb_RetweetData (Username , RetweetUsername , Tweet , DateTime) VALUES ('" + Username + "' , '" + RetweetUsername + "' , '" + Tweet + "' ,'" + DateTime.Now.ToString() + "')";
                DataBaseHandler.InsertQuery(strQuery, "tb_RetweetData");
            }
            catch (Exception ex)
            {

            }
        } 
        #endregion

        #region InsertMessageData
        public void InsertMessageData(string Username, string Type, string OtherUsername, string Message)
        {
            try
            {
                string strQuery = "INSERT INTO tb_MessageRecord (Username , Type , OtherUsername , Message , DateTime ) VALUES ('" + Username + "' , '" + Type + "' , '" + OtherUsername + "' , '" + Message + "' , '" + DateTime.Today.ToString() + "') ";

                DataBaseHandler.InsertQuery(strQuery, "tb_MessageRecord");
            }
            catch (Exception)
            {

            }
        } 
        #endregion

        #region SelectMessageData
        public DataSet SelectMessageData(string Username, string Type)
        {
            DataSet ds = new DataSet();
            try
            {
                string strQuery = "SELECT * FROM tb_MessageRecord WHERE Type = '" + Type + "' and Username = '" + Username + "' and DateTime = '" + DateTime.Today.ToString() + "'";

                ds = DataBaseHandler.SelectQuery(strQuery, "tb_MessageRecord");
            }
            catch (Exception ex)
            {

            }
            return ds;
        } 
        #endregion

        #region SelectFollowMessageData
        public DataSet SelectFollowMessageData(string Username, string DateTime)
        {
            DataSet ds = new DataSet();
            try
            {
                //SELECT        id, username, following_id, following_username, DateFollowed FROM  tb_Follow WHERE (DateFollowed LIKE '2012-07-30 %') AND (username = 'marcos-paulo007@hotmail.com')
                string strQuery = "SELECT * FROM tb_Follow WHERE Username = '" + Username + "' and DateFollowed like '" + DateTime + "%'";

                ds = DataBaseHandler.SelectQuery(strQuery, "tb_Follow");
            }
            catch (Exception ex)
            {

            }
            return ds;
        } 
        #endregion


        #region SearchWithInvite
        public DataSet SearchWithInvite(string Username, string Type)
        {
            DataSet ds = new DataSet();
            try
            {
                string strQuery = "SELECT * FROM tb_SearchWithInvite WHERE  Username = '" + Username + "' and InviteDate = '" + DateTime.Today.ToString() + "'";

                ds = DataBaseHandler.SelectQuery(strQuery, "tb_SearchWithInvite");
            }
            catch (Exception ex)
            {

            }
            return ds;
        }
        #endregion

        #region CamapaignData

        public void InsertCamapaignData(string query, string table)
        {
            try
            {
                //string strQuery = "INSERT INTO tb_MessageRecord (Username , Type , OtherUsername , Message , DateTime ) VALUES ('" + Username + "' , '" + Type + "' , '" + OtherUsername + "' , '" + Message + "' , '" + DateTime.Today.ToString() + "') ";

                DataBaseHandler.InsertQuery(query, table);
            }
            catch (Exception)
            {

            }
        }

        #region SelectMessageData
        public DataSet SelectCamapaignData()
        {
            DataSet ds = new DataSet();
            try
            {
                string strQuery = "SELECT * FROM tb_CamapignSearcWithInvite";

                ds = DataBaseHandler.SelectQuery(strQuery, "tb_CamapignSearcWithInvite");
            }
            catch (Exception ex)
            {

            }
            return ds;
        } 
        #endregion

        #endregion

        #region DeleteCampaignRow
        public void DeleteDataLogin(string id)
        {

            string DeleteQuery = "delete from tb_CamapignSearcWithInvite where CamPaignName='" + id + "'";
            DataBaseHandler.DeleteQuery(DeleteQuery, "tb_CamapignSearcWithInvite");
        }

        #endregion

        #region CampaignJoinGroup

#region Selectdata
        public DataSet SelectCampaignJoinGroupData()
        {
            DataSet ds = new DataSet();
            try
            {
                string strQuery = "SELECT * FROM tb_CampaignJoinGroup";

                ds = DataBaseHandler.SelectQuery(strQuery, "tb_CampaignJoinGroup");
            }
            catch (Exception ex)
            {

            }
            return ds;
        }
#endregion
        #endregion 

        #region CampaignScraper
        public void InsertCampaignScraperData(string query, string table)
        {
            try
            {
                DataBaseHandler.InsertQuery(query, table);
            }
            catch
            { }
        }

        public DataSet getAllCampaignScraperData()
        {
            DataSet DS = new DataSet();
            try
            {
                string query = "Select CampaignName, Account, FirstName, LastName, Location, Country, AreaWiseLocation, PostalCode, Company, Keyword, Title, Industry, Relationship, Language, Groups, ExportedFileName, TitleValue, CompanyValue, within, YearsOfExperience, Function, SeniorLevel, IntrestedIn, CompanySize, Fortune1000, RecentlyJoined from tb_CampaignScraper";
                DS = DataBaseHandler.SelectQuery(query, "tb_CampaignScraper");
            }
            catch
            { }
            return DS;
        }

        public void InsertUrl(string query)
        {
            try
            {
                DataBaseHandler.InsertQuery(query, "tb_CampaignScraperURL");
            }
            catch
            { }
        }

        public void UpdateCampaignScraperUrl(string Url)
        {
            try
            {
                string UpdateQuery = "UPDATE tb_CampaignScraperURL SET Status = 'Scraped'" + " WHERE Url='" + Url + "'";
                DataBaseHandler.UpdateQuery(UpdateQuery, "tb_CampaignScraperURL");
            }
            catch
            { }
        }

        public DataSet SelectUrl(string Account)
        {
            DataSet DS = new DataSet();
            try
            {

                string SelectQuery = "SELECT Url FROM tb_CampaignScraperURL WHERE Account='" + Account + "' And Status='Not Scraped';";
                DS = DataBaseHandler.SelectQuery(SelectQuery, "tb_CampaignScraperURL");

            }
            catch
            { }
            return DS;
        }


        public void DeleteCampaignScraperData(string query)
        {
            try
            {
                DataBaseHandler.DeleteQuery(query, "tb_CampaignScraper");
            }
            catch
            { }
        }

        public DataSet getDataFromAccountsTable(string Account)
        {
            DataSet DS = new DataSet();
            try
            {
                string query = "Select * from tb_LinkedInAccount Where UserName=" + "'" + Account + "';";
                DS = DataBaseHandler.SelectQuery(query, "tb_LinkedInAccount");
            }
            catch
            { }
            return DS;
        }

        #endregion

        //public void DeleteDecaptcherData(string strDecaptcher)
        //{
        //    try
        //    {

        //        string strTable = "tb_Setting";
        //        string strQuery = "DELETE FROM tb_Setting WHERE FileType=" + strDecaptcher;

        //        DataBaseHandler.DeleteQuery(strQuery, strTable);
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}

        public static void InsertQueryForEndorsement(string query)
        {
            try
            {
                DataBaseHandler.InsertQuery(query, "tb_endorsement");
            }
            catch { }
        }

      }
}
