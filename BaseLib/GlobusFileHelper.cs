using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Collections;

namespace BaseLib
{
    public static class GlobusFileHelper
    {

        private static readonly object obj_AppendStringToTextfileNewLine = new object();

        public static String ReadStringFromTextfile(string filepath)
        {
            string fileText = string.Empty;
            
            try
            {
                StreamReader reader = new StreamReader(filepath);
                fileText = reader.ReadToEnd();
                reader.Close();
            }
            catch
            {
            }
            return fileText;
        }

        private static int _bufferSize = 16384;

        public static List<string> ReadFile(string filename)
        {
            List<string> listFileContent = new List<string>();

            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                using (FileStream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read))
                {
                    using (StreamReader streamReader = new StreamReader(fileStream))
                    {
                        char[] fileContents = new char[_bufferSize];
                        int charsRead = streamReader.Read(fileContents, 0, _bufferSize);

                        // Can't do much with 0 bytes
                        if (charsRead == 0)
                            throw new Exception("File is 0 bytes");

                        while (charsRead > 0)
                        {
                            stringBuilder.Append(fileContents);
                            charsRead = streamReader.Read(fileContents, 0, _bufferSize);
                        }

                        string[] contentArray = stringBuilder.ToString().Split(new char[] { '\r', '\n' });
                        foreach (string line in contentArray)
                        {
                            listFileContent.Add(line.Replace("#", "").Replace("\0",string.Empty).Trim());
                        }
                        
                        listFileContent.RemoveAll(s => string.IsNullOrEmpty(s));
                    }
                }
            }
            catch
            {
            }
            return listFileContent;
        }

        public static List<string> ReadFiletoStringList(string filepath)
        {
            List<string> list = new List<string>();

            try
            {
                StreamReader reader = new StreamReader(filepath);
                string text = "";
                while ((text = reader.ReadLine()) != null)
                {
                    if (!string.IsNullOrEmpty(text.Replace(" ", "").Replace("\t", "")))
                    {
                        list.Add(text);
                    }
                }
                reader.Close();
            }
            catch
            {
            }
            return list;
        }

        public static void WriteStringToTextfile(string content,string filepath)
        {
            try
            {
                StreamWriter writer = new StreamWriter(filepath);
                writer.Write(content);
                writer.Close();
            }
            catch
            {
            }
           
        }

        public static void WriteStringToTextfileNewLine(String content, string filepath)
        {
            try
            {
                StreamWriter writer = new StreamWriter(filepath);

                StringReader reader = new StringReader(content);

                while (reader.ReadLine() != null)
                {
                    writer.WriteLine(content);
                }

                writer.Close();
            }
            catch
            {
            }
        }
        public static void AppendStringToTextfileNewLine(String content, string filepath)
        {
            
            try
            {
                lock (obj_AppendStringToTextfileNewLine)
                {
                    StreamWriter writer = new StreamWriter(filepath, true);

                    StringReader reader = new StringReader(content);

                    string temptext = "";

                    while ((temptext = reader.ReadLine()) != null)
                    {
                        writer.WriteLine(temptext);
                    }

                    writer.Close();
                }
            }
            catch
            {
            }
        }

        public static void AppendStringToTextfileNewLineWithCarat(String content, string filepath)
        {

            try
            {
                StreamWriter writer = new StreamWriter(filepath, true);

                StringReader reader = new StringReader(content);

                string temptext = "";

                while ((temptext = reader.ReadLine()) != null)
                {
                    writer.WriteLine(temptext);
                }
                for (int i = 0; i < 80; i++)
                {
                    writer.Write("-");
                }
                writer.WriteLine();

                writer.Close();
            }
            catch (Exception ex)
            { 
            
            }
        }

        public static List<string> ReadFiletoStringLst(string filepath)
        {
            List<string> list = new List<string>();

            try
            {
                StreamReader reader = new StreamReader(filepath);
                string text = "";
                while ((text = reader.ReadLine()) != null)
                {
                    if (!string.IsNullOrEmpty(text.Replace(" ", "").Replace("\t", "")))
                    {
                        list.Add(text.Replace("�", " "));
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
            }
            return list;

        }


        public static void WriteListtoTextfile(List<string> list,string filepath)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filepath))
                {
                    foreach (string listitem in list)
                    {
                        writer.WriteLine(listitem);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }



        public static List<string> readcsvfile(string filpath)
        {
            List<string> tempdata = new List<string>();

            try
            {
                StreamReader sr = new StreamReader(filpath, Encoding.GetEncoding(1250));
                string strline = "";
                int x = 0;
                while (!sr.EndOfStream)
                {
                    x++;
                    strline = sr.ReadLine();
                    tempdata.Add(strline);
                }
                sr.Close();
            }
            catch
            {
            }
            return tempdata;
        }

        public static void ReplaceStringFromCsv(string filePath, string searchText, string replaceText)
        {
            try
            {
                StreamReader reader = new StreamReader(filePath);
                string content = reader.ReadToEnd();
                reader.Close();

                content = Regex.Replace(content, searchText + "\r\n", replaceText);

                StreamWriter writer = new StreamWriter(filePath);
                writer.Write(content);
                writer.Close();
            }
            catch
            {
            }
        }

        /// <summary>
        /// Get Spinned Sentences
        /// </summary>
        /// <param name="RawComment">Format: (I/we/us) (am/are) planning to go (market/outing)...</param>
        /// <returns></returns>
        public static List<string> GetSpinnedComments(string RawComment)
        {
            #region Using Hashtable
            ///// <summary>
            ///// Hashtable that stores (DataInsideBraces) as Key and DataInsideBracesArray as Value
            ///// </summary>
            //Hashtable commentsHashTable = new Hashtable();

            /////This is final possible cominations of comments
            //List<string> listModComments = new List<string>();

            /////Put braces data in list of string array
            //List<string[]> listDataInsideBracesArray = new List<string[]>();

            /////This Regex will fetch data within braces and put it in list of string array
            //var regex = new Regex(@"\(([^)]*)\)", RegexOptions.Compiled);
            //foreach (Match Data in regex.Matches(RawComment))
            //{
            //    string data = Data.Value.Replace("(", "").Replace(")", "");
            //    string[] DataInsideBracesArray = data.Split(separator);//data.Split('/');
            //    commentsHashTable.Add(Data, DataInsideBracesArray);
            //    listDataInsideBracesArray.Add(DataInsideBracesArray);
            //}

            //string ModifiedComment = RawComment;

            //IDictionaryEnumerator en = commentsHashTable.GetEnumerator();

            //List<string> listModifiedComment = new List<string>();

            //listModifiedComment.Add(ModifiedComment);

            ////en.Reset();

            //string ModifiedComment1 = ModifiedComment;

            //#region Assigning Values and adding in List
            //foreach (string[] item in listDataInsideBracesArray)
            //{
            //    en.MoveNext();
            //    foreach (string modItem in listModifiedComment)
            //    {
            //        foreach (string innerItem in item)
            //        {
            //            string ModComment = modItem.Replace(en.Key.ToString(), innerItem);
            //            listModComments.Add(ModComment);
            //        }
            //    }
            //    listModifiedComment.AddRange(listModComments);
            //    //string ModComment = ModifiedComment1.Replace(en.Key, item
            //}
            //#endregion

            //List<string> listRequiredComments = listModifiedComment.FindAll(s => !s.Contains("("));

            ////listComments.AddRange(listRequiredComments);
            //return listRequiredComments;
            #endregion

            #region Using Dictionary
            /// <summary>
            /// Hashtable that stores (DataInsideBraces) as Key and DataInsideBracesArray as Value
            /// </summary>
            //Hashtable commentsHashTable = new Hashtable();
            List<string> listRequiredComments = new List<string>();

            try
            {
                Dictionary<Match, string[]> commentsHashTable = new Dictionary<Match, string[]>();

                ///This is final possible cominations of comments
                List<string> listModComments = new List<string>();

                ///Put braces data in list of string array
                List<string[]> listDataInsideBracesArray = new List<string[]>();

                ///This Regex will fetch data within braces and put it in list of string array
                var regex = new Regex(@"\(([^)]*)\)", RegexOptions.Compiled);
                foreach (Match Data in regex.Matches(RawComment))
                {
                    string data = Data.Value.Replace("(", "").Replace(")", "");
                    string[] DataInsideBracesArray = data.Split('|');
                    commentsHashTable.Add(Data, DataInsideBracesArray);
                    listDataInsideBracesArray.Add(DataInsideBracesArray);

                    if (listDataInsideBracesArray.Count >= 5)
                    {
                        break;
                    }
                }

                string ModifiedComment = RawComment;

                IDictionaryEnumerator en = commentsHashTable.GetEnumerator();

                List<string> listModifiedComment = new List<string>();

                listModifiedComment.Add(ModifiedComment);

                //en.Reset();

                string ModifiedComment1 = ModifiedComment;

                #region Assigning Values and adding in List
                
                    foreach (string[] item in listDataInsideBracesArray)
                    {
                        en.MoveNext();


                        foreach (string modItem in listModifiedComment)
                        {
                            foreach (string innerItem in item)
                            {
                                string ModComment = modItem.Replace(en.Key.ToString(), innerItem);
                                listModComments.Add(ModComment);

                            }
                        }

                        try
                        {
                            listModifiedComment.AddRange(listModComments);
                            //if (listModifiedComment.Count > 5000)
                            //{
                            //    listRequiredComments = listModifiedComment.FindAll(s => !s.Contains("|"));
                            //    listRequiredComments = listModifiedComment.FindAll(s => !s.Contains("("));
                            //    return listModifiedComment;
                            //}
                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                        //string ModComment = ModifiedComment1.Replace(en.Key, item

                    }
                #endregion

                    listRequiredComments = listModifiedComment.FindAll(s => !s.Contains("("));
                    //listComments.AddRange(listRequiredComments);
            }
            catch
            {
            }
            return listRequiredComments;
            #endregion
        }

        /// <summary>
        /// Get Spinned Sentences
        /// </summary>
        /// <param name="RawComment">Format: (I/we/us) (am/are) planning to go (market/outing)...</param>
        /// <param name="RawComment">Format: "|" or "/"...</param>
        /// <returns></returns>
        public static List<string> GetSpinnedComments(string RawComment, char separator)
        {
            #region Using Hashtable
            ///// <summary>
            ///// Hashtable that stores (DataInsideBraces) as Key and DataInsideBracesArray as Value
            ///// </summary>
            //Hashtable commentsHashTable = new Hashtable();

            /////This is final possible cominations of comments
            //List<string> listModComments = new List<string>();

            /////Put braces data in list of string array
            //List<string[]> listDataInsideBracesArray = new List<string[]>();

            /////This Regex will fetch data within braces and put it in list of string array
            //var regex = new Regex(@"\(([^)]*)\)", RegexOptions.Compiled);
            //foreach (Match Data in regex.Matches(RawComment))
            //{
            //    string data = Data.Value.Replace("(", "").Replace(")", "");
            //    string[] DataInsideBracesArray = data.Split(separator);//data.Split('/');
            //    commentsHashTable.Add(Data, DataInsideBracesArray);
            //    listDataInsideBracesArray.Add(DataInsideBracesArray);
            //}

            //string ModifiedComment = RawComment;

            //IDictionaryEnumerator en = commentsHashTable.GetEnumerator();

            //List<string> listModifiedComment = new List<string>();

            //listModifiedComment.Add(ModifiedComment);

            ////en.Reset();

            //string ModifiedComment1 = ModifiedComment;

            //#region Assigning Values and adding in List
            //foreach (string[] item in listDataInsideBracesArray)
            //{
            //    en.MoveNext();
            //    foreach (string modItem in listModifiedComment)
            //    {
            //        foreach (string innerItem in item)
            //        {
            //            string ModComment = modItem.Replace(en.Key.ToString(), innerItem);
            //            listModComments.Add(ModComment);
            //        }
            //    }
            //    listModifiedComment.AddRange(listModComments);
            //    //string ModComment = ModifiedComment1.Replace(en.Key, item
            //}
            //#endregion

            //List<string> listRequiredComments = listModifiedComment.FindAll(s => !s.Contains("("));

            ////listComments.AddRange(listRequiredComments);
            //return listRequiredComments;
            #endregion

            #region Using Dictionary

            List<string> listRequiredComments = new List<string>();

            try
            {
                /// <summary>
                /// Hashtable that stores (DataInsideBraces) as Key and DataInsideBracesArray as Value
                /// </summary>
                //Hashtable commentsHashTable = new Hashtable();
                Dictionary<Match, string[]> commentsHashTable = new Dictionary<Match, string[]>();

                ///This is final possible cominations of comments
                List<string> listModComments = new List<string>();

                ///Put braces data in list of string array
                List<string[]> listDataInsideBracesArray = new List<string[]>();

                ///This Regex will fetch data within braces and put it in list of string array
                var regex = new Regex(@"\(([^)]*)\)", RegexOptions.Compiled);
                foreach (Match Data in regex.Matches(RawComment))
                {
                    string data = Data.Value.Replace("(", "").Replace(")", "");
                    string[] DataInsideBracesArray = data.Split(separator);//data.Split('/');
                    commentsHashTable.Add(Data, DataInsideBracesArray);
                    listDataInsideBracesArray.Add(DataInsideBracesArray);
                }

                string ModifiedComment = RawComment;

                IDictionaryEnumerator en = commentsHashTable.GetEnumerator();

                List<string> listModifiedComment = new List<string>();

                listModifiedComment.Add(ModifiedComment);

                //en.Reset();

                string ModifiedComment1 = ModifiedComment;

                #region Assigning Values and adding in List
                foreach (string[] item in listDataInsideBracesArray)
                {
                    en.MoveNext();
                    foreach (string modItem in listModifiedComment)
                    {
                        foreach (string innerItem in item)
                        {
                            string ModComment = modItem.Replace(en.Key.ToString(), innerItem);
                            listModComments.Add(ModComment);
                        }
                    }
                    listModifiedComment.AddRange(listModComments);
                    //string ModComment = ModifiedComment1.Replace(en.Key, item
                }
                #endregion

                listRequiredComments = listModifiedComment.FindAll(s => !s.Contains("("));

                //listComments.AddRange(listRequiredComments);
            }
            catch
            {
            }
            return listRequiredComments;
            #endregion
        }
    }
}
