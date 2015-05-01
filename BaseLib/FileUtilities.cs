using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Data;
using System.Text.RegularExpressions;

namespace BaseLib
{
    public class FileUtilities
    {
        #region ReadFileToListString
        public static List<string> ReadFileToListString(string FilePath)
        {
            using (StreamReader reader = new StreamReader(FilePath))
            {
                List<string> list = new List<string>();
                string text = "";
                while ((text = reader.ReadLine()) != null)
                {
                    list.Add(text);
                }
                return list;
            }
        } 
        #endregion

        #region ReadTextFileFromstar
        public static List<string> ReadTextFileFromstar(string FilePath)
        {
            List<string> list = null;
            try
            {
                using (StreamReader reader = new StreamReader(FilePath))
                {
                    list = new List<string>();
                    string text = "";
                    text = reader.ReadToEnd();
                    // while ((text = reader.ReadLine()) != null)
                    {
                        if (text.Contains("*****"))
                        {
                            string[] arrItem = Regex.Split(text, "*****");
                            foreach (string item in arrItem)
                            {
                                list.Add(item);
                            }

                        }
                    }

                }
            }
            catch
            {
            }
            return list;
        } 
        #endregion

        #region ReadFileToString
        public static string ReadFileToString(string FilePath)
        {
            using (StreamReader reader = new StreamReader(FilePath))
            {
                string text = reader.ReadToEnd();
                return text;
            }
        } 
        #endregion

        #region AppendStringToTextFile
        public static void AppendStringToTextFile(string FilePath, string content)
        {
            using (StreamWriter sw = new StreamWriter(FilePath, true))
            {
                sw.WriteLine(content);
            }
        } 
        #endregion

        #region AppendStringToCSVFile
        public static void AppendStringToCSVFile(string FilePath, string content)
        {
            using (StreamWriter sw = new StreamWriter(FilePath, true))
            {
                string[] contentArray = content.Split(',');
                foreach (string item in contentArray)
                {
                    sw.Write(content);
                    sw.Write(",");
                }
                sw.WriteLine();
                //sw.WriteLine(content);
            }
        } 
        #endregion

        #region WriteListToTextFile
        public static void WriteListToTextFile(string FilePath, List<string> list)
        {
            using (StreamWriter sw = new StreamWriter(FilePath, true))
            {
                foreach (string line in list)
                {
                    sw.WriteLine(line);
                }
            }
        } 
        #endregion

        #region CreateRegistrationFile
        public static void CreateRegistrationFile(string path)
        {
            if (!File.Exists(path))
            {
                File.Create(path);
            }
        } 
        #endregion

  
    }
}
