
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class MyUtils
    {
        public static float ToRadians(float degrees)
        {
            return (float)(Math.PI * degrees / 180.0);
        }

        public static float ToDegrees(float radians)
        {
            return (float)(180.0 * radians / Math.PI);
        }

        public static Vector2 DegreesToVector(float radians)
        {
            return new Vector2((float)Math.Cos(radians), (float)Math.Sin(radians));
        }

        public static Vector3 DegreesToVectorXZ(float radians)
        {
            return new Vector3((float)Math.Cos(radians), 0.0f, (float)Math.Sin(radians));
        }

        public static float ClampRadians(float radians)
        {
            return radians % (float)(Math.PI * 2.0);
        }
    }
}
