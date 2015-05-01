using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
//using Finisar.SQLite;
using System.Data.SQLite;

namespace BaseLib.DB_Repository
{
    public class DataBaseHandler
    {
        //public static string CONstr = "Data Source=" + System.Windows.Forms.Application.StartupPath + "\\DB_FBDominator" + ";Version=3;"; 
        public static string CONstr = "Data Source=" + Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\LinkedInDominator\\DB_LinkedInDominator.db" + ";Version=3;";
        //data source="D:\LinkedShark - New\LinkedinDominator\LinkedinDominator\LinkedinDominator\bin\Debug\DB_LinkedInDominator.db"
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

    }
}
