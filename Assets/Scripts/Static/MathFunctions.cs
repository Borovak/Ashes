using System;
using UnityEngine;

namespace Static
{
    public static class MathFunctions
    {
        public static int DivFloorInt(int a, int b)
        {
            return Convert.ToInt32(Math.Floor(Convert.ToSingle(a) / Convert.ToSingle(b)));
        }
        
        public static Vector2 RadianToVector2(float radian)
        {
            return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
        }
        
        public static Vector2 DegreeToVector2(float degree)
        {
            return RadianToVector2(degree * Mathf.Deg2Rad);
        }
    }
}