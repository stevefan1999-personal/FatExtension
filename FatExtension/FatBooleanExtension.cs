using System;

namespace FatExtension
{
    public static class FatBooleanExtension
    {
        public static bool True(this bool x, Action fn)
        {
            fn.CheckNotNull(() => fn = () => { });
            if (x) fn(); return x;
        }
        public static bool False(this bool x, Action fn)
        {
            fn.CheckNotNull(() => fn = () => { });
            if (!x) fn(); return x;
        }
    }
}
