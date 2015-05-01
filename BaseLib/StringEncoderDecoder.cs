using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaseLib
{
    public class StringEncoderDecoder
    {
        #region Encode
        public static string Encode(string data)
        {
            string encoded = data.Replace("'", "^");
            return encoded;
        } 
        #endregion

        #region Decode
        public static string Decode(string data)
        {
            string decoded = data.Replace("^", "'");
            return decoded;
        } 
        #endregion
    }
}
