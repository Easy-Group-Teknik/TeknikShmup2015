﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using Microsoft.Xna.Framework;

namespace kontroll
{
    class Globals
    {
        private static Random r;
        public static Random Randomizer { get { if (r == null) r = new Random(); return r; } }

        public static bool gameOver;

        public static int Highscore
        {
            get
            {
                int highscore;

                StreamReader sr = new StreamReader("highscore.hi");
                highscore = int.Parse(sr.ReadLine());
                sr.Dispose();

                foreach (Player p in GameObjectManager.gameObjects.Where(item => item is Player))
                {
                    if (p.Score > highscore)
                    {
                        highscore = p.Score;

                        StreamWriter sw = new StreamWriter("highscore.hi");
                        sw.WriteLine(p.Score);
                        sw.Dispose();
                    }
                }
                return highscore;
            }
        }

        public static void Beep(int frequency, int duration)
        {
            Thread thread = new Thread(new ThreadStart(() => Console.Beep(frequency, duration)));
            thread.Start();
            thread.Abort();
        }

        public static void Beep()
        {
            Thread thread = new Thread(new ThreadStart(Console.Beep));
            thread.Start();
            thread.Abort();
        }

        public static float DistanceTo(Vector2 target, Vector2 target2)
        {
            return (float)Math.Sqrt((target.X - target2.X) * (target.X - target2.X) + (target.Y - target2.Y) * (target.Y - target2.Y));
        }

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
