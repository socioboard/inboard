using System;
using System.Data;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.IO;
using System.Web;
//using System.Web.Security;
//using System.Web.UI;
//using System.Web.UI.HtmlControls;
//using System.Web.UI.WebControls;
//using System.Web.UI.WebControls.WebParts;

using System.Xml.Linq;
using DataStreams.Xlsx;
using Excel = Microsoft.Office.Interop.Excel;
//using Microsoft.Office.Interop.Excel.Application;

/// <summary>
/// Summary description for clsEmailParser
/// </summary>
/// 
namespace BaseLib//SiteInfoScrapper
//namespace InBoardProGetData
{
    public class clsEmailParser
    {
        //DataClassesDataContext db_context = new DataClassesDataContext();
        public clsEmailParser()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public List<string[]> parseCSV(string path)
        {
            List<string[]> parsedData = new List<string[]>();

            try
            {
                using (StreamReader readFile = new StreamReader(path))
                {
                    string line;
                    string[] row;

                    while ((line = readFile.ReadLine()) != null)
                    {
                        row = line.Split(',');
                        parsedData.Add(row);
                    }
                }
                return parsedData;
            }
            catch (Exception e)
            {
                return parsedData;
            }
        }

        public List<string[]> parseExcel(string path)
        {
            List<string[]> parsedData = new List<string[]>();
            try
            {
                // string[] row =new string[5];
                string singlerow = string.Empty;
                //Excel.FillFormat xlfile = new Excel.FillFormat();
                Excel.Application xlApp;
                Excel.Workbook xlWorkBook;
                Excel.Worksheet xlWorkSheet;
                Excel.Range range;

                string str;
                int rCnt = 0;
                int cCnt = 0;

                xlApp = new Excel.ApplicationClass();
               // xlApp = new Excel.Application();
                xlWorkBook = xlApp.Workbooks.Open(path, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                //xlWorkBook = xlApp.Workbooks.Open(path, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel, "\t", false, false, 0, true, 1, 0);
                xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

                range = xlWorkSheet.UsedRange;

                for (rCnt = 1; rCnt <= range.Rows.Count; rCnt++)
                {
                    //string[] row = new string[9];
                    string[] row = new string[range.Columns.Count];
                    for (cCnt = 1; cCnt <= range.Columns.Count; cCnt++)
                    {
                        try
                        {
                            str = (string)(range.Cells[rCnt, cCnt] as Excel.Range).Value2.ToString();
                            row[cCnt - 1] = str;
                        }
                        catch { }
                    }
                    parsedData.Add(row);
                }

                xlWorkBook.Close(true, null, null);
                xlApp.Quit();

                releaseObject(xlWorkSheet);
                releaseObject(xlWorkBook);
                releaseObject(xlApp);
                return parsedData;
            }
            catch
            {
                return parsedData;
            }

        }

        public List<string> parseExcelforcountry(string path)
        {
            List<string> parsedData = new List<string>();
            try
            {
                // string[] row =new string[5];
                string singlerow = string.Empty;
                //Excel.FillFormat xlfile = new Excel.FillFormat();
                Excel.Application xlApp;
                Excel.Workbook xlWorkBook;
                Excel.Worksheet xlWorkSheet;
                Excel.Range range;

                string str;
                int rCnt = 0;
                int cCnt = 0;

                // xlApp = new Excel.ApplicationClass();
                xlApp = new Excel.Application();
                xlWorkBook = xlApp.Workbooks.Open(path, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                //xlWorkBook = xlApp.Workbooks.Open(path, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel, "\t", false, false, 0, true, 1, 0);
                xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

                range = xlWorkSheet.UsedRange;

                for (rCnt = 1; rCnt <= range.Rows.Count; rCnt++)
                {
                    //string[] row = new string[9];
                    string[] row = new string[range.Columns.Count];
                    for (cCnt = 1; cCnt <= range.Columns.Count; cCnt++)
                    {
                        try
                        {
                            str = (string)(range.Cells[rCnt, cCnt] as Excel.Range).Value2.ToString();
                            row[cCnt - 1] = str;
                            string items = str;
                            parsedData.Add(items);
                        }
                        catch { }
                    }
                   
                }

                xlWorkBook.Close(true, null, null);
                xlApp.Quit();

                releaseObject(xlWorkSheet);
                releaseObject(xlWorkBook);
                releaseObject(xlApp);
                return parsedData;
            }
            catch
            {
                return parsedData;
            }

        }
        /// <summary>
        /// Reading XLSX File
        /// </summary>
        /// <param name="path">FilePath</param>
        /// <returns>List<String[]></returns>
        public List<string[]> ParseXLSXFile(string path)
        {
            List<string[]> parsedData = new List<string[]>();
            try
            {

                XlsxReader reader;
                reader = new XlsxReader(path);


                foreach (string[] arr in reader)
                {
                    parsedData.Add(arr);
                }

                return parsedData;
            }
            catch
            {
                return parsedData;
            }

        }

        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                //MessageBox.Show("Unable to release the Object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }



    }

}