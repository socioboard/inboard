using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Globussoft;

namespace BaseLib
{
    public class SpinnedListGenerator
    {
        #region GetSpinnedList
        public static List<string> GetSpinnedList(List<string> inputList)
        {
            List<string> tempList = new List<string>();
            foreach (string item in inputList)
            {
                tempList.Add(item);
            }
            inputList.Clear();
            foreach (string item in tempList)
            {
                List<string> tempSpunList = GlobusFileHelper.GetSpinnedComments(item);
                inputList.AddRange(tempSpunList);
            }
            return inputList;
        } 
        #endregion

        #region GetSpinnedList
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
                List<string> tempSpunList = GlobusFileHelper.GetSpinnedComments(item, separator);
                inputList.AddRange(tempSpunList);
            }
            return inputList;
        } 
        #endregion
    }
}
