using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;

namespace BaseLib
{
    /// <summary>
    /// This Class has methods to spin texts. Can be used to spin both large texts and small texts.
    /// </summary>
    public class GlobusSpinHelper
    {
        public static List<string> GetSpinnedList(List<string> inputList, char separator)
        {
            List<string> tempList = new List<string>();
            foreach (string item in inputList)
            {
                tempList.Add(item);
            }
            inputList.Clear();
            foreach (string item in tempList)
            {
                List<string> tempSpunList = spinSmallText(item, separator);
                inputList.AddRange(tempSpunList);
            }
            return inputList;
        }

        
        /// <summary>
        /// Get Spinned Sentences
        /// </summary>
        /// <param name="RawText">Format: (I/we/us) (am/are) planning to go (market/outing)...</param>
        /// <param name="RawText">Format: "|" or "/"...</param>
        /// <returns></returns>
        public static List<string> spinSmallText(string RawText, char separator)
        {
            #region Using Dictionary
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
            foreach (Match Data in regex.Matches(RawText))
            {
                string data = Data.Value.Replace("(", "").Replace(")", "");
                string[] DataInsideBracesArray = data.Split(separator);//data.Split('/');
                commentsHashTable.Add(Data, DataInsideBracesArray);
                listDataInsideBracesArray.Add(DataInsideBracesArray);
            }

            string ModifiedComment = RawText;

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

            List<string> listRequiredComments = listModifiedComment.FindAll(s => !s.Contains("("));

            //listComments.AddRange(listRequiredComments);
            return listRequiredComments;
            #endregion
        }

        public static string spinLargeText(Random rnd, string str)
        {
            // Loop over string until all patterns exhausted.
            //string pattern = "{[^{}]*}";
            string pattern = @"\(([^)]*)\)";
            Match m = Regex.Match(str, pattern);
            while (m.Success)
            {
                // Get random choice and replace pattern match.
                string seg = str.Substring(m.Index + 1, m.Length - 2);
                string[] choices = seg.Split('|');
                str = str.Substring(0, m.Index) + choices[rnd.Next(choices.Length)] + str.Substring(m.Index + m.Length);
                m = Regex.Match(str, pattern);
            }

            // Return the modified string.
            return str;
        }
    }
}
