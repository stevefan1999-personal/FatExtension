using System;
using System.Linq;

namespace FatExtension
{
    public static class FatArrayExtension
    {
        public static T[] Reverse<T>(this T[] input)
        {
            var list = input.CheckNotNull();
            Array.Reverse(list);
            return list.ToArray();
        }
    }
}
