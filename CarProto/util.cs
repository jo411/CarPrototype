using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarProto
{
    static class util
    {
        static Random randomSingleton;
        /// <summary>
        /// Returns the value of "degrees" in radians
        /// </summary>
        /// <param name="degrees"></param>
        /// <returns></returns>
          public static float degToRad(float degrees)
        {
            return degrees * ((float)Math.PI / 180);
        }

        public static int randomBetween(int min, int max)
        {
            if(randomSingleton==null)
            {
                randomSingleton = new Random(System.DateTime.Now.Millisecond);
            }

            return randomSingleton.Next(min, max);
        }
    }
}
