using System;

namespace FatExtension
{
    public static class FatConvertibleExtension
    {
        public static T Cast<T>(this IConvertible value)
        {
            try
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch
            {
                return default(T);
            }
        }
    }
}
