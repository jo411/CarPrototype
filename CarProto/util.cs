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
        /// <summary>
        /// Returns the value of "degrees" in radians
        /// </summary>
        /// <param name="degrees"></param>
        /// <returns></returns>
          public static float degToRad(float degrees)
        {
            return degrees * ((float)Math.PI / 180);
        }
    }
}
