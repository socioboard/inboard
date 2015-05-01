using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaseLib
{
    public class RandomNumberGenerator
    {
        #region GenerateRandom
        public static int GenerateRandom(int minValue, int maxValue)
        {
            if (minValue <= maxValue)
            {
                Random random = new Random();
                return random.Next(minValue, maxValue);
            }
            else
            {
                return 0;
            }
        } 
        #endregion
    }
}
