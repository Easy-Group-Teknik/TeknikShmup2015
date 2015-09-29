using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace kontroll
{
    class Globals
    {
        private static Random r;
        public static Random Randomizer { get { if (r == null) r = new Random(); return r; } }


        public static float DegreesToRadian(float degree)
        {
            return degree * (float)Math.PI / 180;
        }

        public static float RadianToDegree(float radian)
        {
            return radian * 180 / (float)Math.PI;
        }

        public static float Lerp(float s, float e, float t)
        {
            return s + t * (e - s);
        }
    }
}
