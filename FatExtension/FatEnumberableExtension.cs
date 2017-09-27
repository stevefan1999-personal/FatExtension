using System;
using System.Collections.Generic;
using System.Linq;

namespace FatExtension
{
    public static class FatEnumberableExtension
    {
        public static ICollection<T> Each<T>(this ICollection<T> src, Action<T> fn)
        {
            src.CheckNotNull();
            fn.CheckNotNull(() => fn = _ => { });
            foreach (var item in src) fn(item);
            return src;
        }
        public static IEnumerable<T> Each<T>(this IEnumerable<T> src, Action<T> fn)
        {
            var e = src as T[] ?? src.ToArray();
            fn.CheckNotNull(() => fn = _ => { });
            foreach (var item in e) fn(item);
            return e;
        }
        public static IEnumerable<T> Filter<T>(this ICollection<T> src, Func<T, bool> fn)
        {
            return src.Where(fn);
        }
        public static bool Has<T>(this IEnumerable<T> src, T obj)
        {
            var e = src as T[] ?? src.ToArray();
            obj.CheckNotNull();
            return e.Contains(obj);
        }
        public static ICollection<T> AddRange<T>(this ICollection<T> src, params T[] values)
        {
            values.Each(src.Add);
            return src;
        }
    }

}
