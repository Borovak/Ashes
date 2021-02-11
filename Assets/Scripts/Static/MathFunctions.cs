using System;

namespace Static
{
    public static class MathFunctions
    {
        public static int DivFloorInt(int a, int b)
        {
            return Convert.ToInt32(Math.Floor(Convert.ToSingle(a) / Convert.ToSingle(b)));
        }
    }
}