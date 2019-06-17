using Pandora.Interactions.UI.Drawing;
using System;
using System.Runtime.InteropServices;

namespace Pandora.SFML.Graphics
{
    internal static class FloatUtil
    {
        [StructLayout(LayoutKind.Explicit)]
        private struct NanUnion
        {
            [FieldOffset(0)]
            internal float DoubleValue;

            [FieldOffset(0)]
            internal ulong UintValue;
        }

        //internal const double DBL_EPSILON = 2.2204460492503131E-16;

        //internal const float FLT_MIN = 1.17549435E-38f;

        public static bool AreClose(float value1, float value2)
        {
            if (value1 == value2)
            {
                return true;
            }
            var num = (Math.Abs(value1) + Math.Abs(value2) + 10.0) * 2.2204460492503131E-16;
            var num2 = value1 - value2;
            return -num < num2 && num > num2;
        }

        public static bool LessThan(float value1, float value2)
        {
            return value1 < value2 && !FloatUtil.AreClose(value1, value2);
        }

        public static bool GreaterThan(float value1, float value2)
        {
            return value1 > value2 && !FloatUtil.AreClose(value1, value2);
        }

        public static bool LessThanOrClose(float value1, float value2)
        {
            return value1 < value2 || FloatUtil.AreClose(value1, value2);
        }

        public static bool GreaterThanOrClose(float value1, float value2)
        {
            return value1 > value2 || FloatUtil.AreClose(value1, value2);
        }

        public static bool IsOne(float value)
        {
            return Math.Abs(value - 1.0) < 2.2204460492503131E-15;
        }

        public static bool IsZero(float value)
        {
            return Math.Abs(value) < 2.2204460492503131E-15;
        }

        public static bool AreClose(Vector2F point1, Vector2F point2)
        {
            return FloatUtil.AreClose(point1.X, point2.X) && FloatUtil.AreClose(point1.Y, point2.Y);
        }


        public static bool IsBetweenZeroAndOne(float val)
        {
            return FloatUtil.GreaterThanOrClose(val, 0.0F) && FloatUtil.LessThanOrClose(val, 1.0F);
        }

        public static int DoubleToInt(double val)
        {
            if (0.0 >= val)
            {
                return (int)(val - 0.5);
            }
            return (int)(val + 0.5);
        }

        public static bool IsNaN(float value)
        {
            NanUnion nanUnion = default(NanUnion);
            nanUnion.DoubleValue = value;
            ulong num = nanUnion.UintValue & 18442240474082181120uL;
            ulong num2 = nanUnion.UintValue & 4503599627370495uL;
            return (num == 9218868437227405312uL || num == 18442240474082181120uL) && num2 > 0uL;
        }
    }
}
