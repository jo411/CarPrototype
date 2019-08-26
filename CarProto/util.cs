using System;

namespace CarProto
{
    static class Util
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
            if (randomSingleton == null)
            {
                randomSingleton = new Random(System.DateTime.Now.Millisecond);
            }

            return randomSingleton.Next(min, max);
        }
    }
}
