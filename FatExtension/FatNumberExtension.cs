using System;

namespace FatExtension
{
    public static class FatNumberExtension
    {
        public static T Square<T>(this T x)
            where T : struct, IConvertible, IComparable<T>, IEquatable<T>
        {
            var self = x.Cast<decimal>();
            return (self * self).Cast<T>();
        }
        public static T Cube<T>(this T x)
            where T : struct, IConvertible, IComparable<T>, IEquatable<T>
        {
            var self = x.Cast<decimal>();
            return (self * self * self).Cast<T>();
        }
        public static T Power<T>(this T x, double y)
            where T : struct, IConvertible, IComparable<T>, IEquatable<T>
        {
            return Math.Pow(x.Cast<double>(), y).Cast<T>();
        }
        public static T Sqrt<T>(this T x)
            where T : struct, IConvertible, IComparable<T>, IEquatable<T>
        {
            return Math.Sqrt(x.Cast<double>()).Cast<T>();
        }
        public static T Root<T>(this T x, double root)
             where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            return x.Power(1 / root).Cast<T>();
        }
        public static T Log<T>(this T x, double b)
            where T : struct, IComparable, IConvertible, IComparable<T>, IEquatable<T>
        {
            return Math.Log(x.Cast<double>(), b).Cast<T>();
        }

        public static bool DivisibleBy<T>(this T x, T y)
            where T : struct, IComparable, IConvertible, IComparable<T>, IEquatable<T>
        {
            return x.Cast<decimal>() % y.Cast<decimal>() == 0;
        }
    }
}
