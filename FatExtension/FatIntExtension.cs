using System;
namespace FatExtension
{
    public static class FatIntExtension
    {
        internal static readonly Random Rnd = new Random();
        internal static readonly object Lck = new object();
        public static void To(this int i, int to, Action<int> fn, int step = 1)
        {
            step.CheckArgument(_ => _ > 0);
            fn.CheckNotNull(() => fn = _ => { });
            if (i < to)
            {
                for (; i < to; i += step) fn(i);
            }
            else if (i > to)
            {
                for (; i > to; i -= step) fn(i);
            }
        }
        public static int Random(this int i, int min, int max)
        {
            lock (Lck) return Rnd.Next(min, max);
        }
        public static int Random(this int i)
        {
            return Random(i, int.MinValue, int.MaxValue);
        }
        public static void RandomSet(this int i, int count, Action<int> fn)
        {
            0.To(count, _ => fn(0.Random()));
        }
        public static int Years(this int year)
        {
            return (year * 12).Months();
        }
        public static int Months(this int month)
        {
            return (month * 30).Days();
        }
        public static int Days(this int day)
        {
            return (day * 24).Hours();
        }
        public static int Hours(this int hour)
        {
            return (hour * 60).Minutes();
        }
        public static int Minutes(this int minute)
        {
            return minute * 60;
        }
        public static int Tb(this int x)
        {
            return (x * 1024).Mb();
        }
        public static int Mb(this int x)
        {
            return (x * 1024).Kb();
        }
        public static int Kb(this int x)
        {
            return (x * 1024).B();
        }
        public static int B(this int x)
        {
            return x;
        }
        public static int Delta(this int x, int y, bool scalar = false)
        {
            return scalar || x > y ? x - y : y - x;
        }

        public static bool Bit(this int x, int bit)
        {
            return ((x >> bit) & 1) == 1;
        }
        public static int BitOn(this int x, int bit)
        {
            return x | (1 << bit);
        }
        public static int BitOff(this int x, int bit)
        {
            return x & ~(1 << bit);
        }
        public static int BitToggle(this int x, int bit)
        {
            return x ^ (1 << bit);
        }
    }
}
