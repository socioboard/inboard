#region namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
//using Finisar.SQLite;
using System.Data.SQLite;
using System.Windows.Forms;
#endregion

namespace BaseLib
{
    public class DataBaseHandler
    {
       
        public static string CONstr = "Data Source=" + Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\InBoardPro\\DB_InBoardPro.db" + ";Version=3;";

        #region public static DataSet SelectQuery(string query, string tablename)
        public static DataSet SelectQuery(string query, string tablename)
        {
            //try
            //{

                DataSet DS = new DataSet();
                using (SQLiteConnection CON = new SQLiteConnection(CONstr))
                {
                    SQLiteCommand CMD = new SQLiteCommand(query, CON);
                    SQLiteDataAdapter AD = new SQLiteDataAdapter(CMD);
                    AD.Fill(DS, tablename);

                }
                return DS;
            //}
            //catch
            //{
            //    return new DataSet();
            //}
        }
        #endregion

        #region public static void InsertQuery(string query, string tablename)
        public static void InsertQuery(string query, string tablename)
        {
            //try
            //{
            using (SQLiteConnection CON = new SQLiteConnection(CONstr))
            {
                SQLiteCommand CMD = new SQLiteCommand(query, CON);
                SQLiteDataAdapter AD = new SQLiteDataAdapter(CMD);
                DataSet DS = new DataSet();
                AD.Fill(DS, tablename);
            }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.StackTrace);
            //}
        }
        #endregion

        #region public static void DeleteQuery(string query, string tablename)
        public static void DeleteQuery(string query, string tablename)
        {
            //try
            //{
                using (SQLiteConnection CON = new SQLiteConnection(CONstr))
                {
                    SQLiteCommand CMD = new SQLiteCommand(query, CON);
                    SQLiteDataAdapter AD = new SQLiteDataAdapter(CMD);
                    DataSet DS = new DataSet();
                    AD.Fill(DS, tablename);
                }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.StackTrace);
            //}
        }
        #endregion

        #region public static void UpdateQuery(string query, string tablename)
        public static void UpdateQuery(string query, string tablename)
        {
            //try
            //{
                using (SQLiteConnection CON = new SQLiteConnection(CONstr))
                {
                    SQLiteCommand CMD = new SQLiteCommand(query, CON);
                    SQLiteDataAdapter AD = new SQLiteDataAdapter(CMD);
                    DataSet DS = new DataSet();
                    AD.Fill(DS, tablename);
                }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.StackTrace);
            //}
        }
        #endregion

    }
}
