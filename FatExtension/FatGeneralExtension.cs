using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.Cryptography;
using System.Text;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace FatExtension
{
    public static class FatGeneralExtension
    {
        class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding { get { return Encoding.UTF8; } }
        }
        internal static JavaScriptSerializer m_JsonSerializer = new JavaScriptSerializer();
        public static byte[] ToPlainOldData<T>(this T pin) 
            where T : struct
        {
            pin.CheckNotNull();

            var len = Marshal.SizeOf(pin);

            var ptr = Marshal.AllocHGlobal(len);
            var pod = new byte[len];

            Marshal.StructureToPtr(pin, ptr, true); // copy pin into ptr
            Marshal.Copy(ptr, pod, 0, len);
            Marshal.FreeHGlobal(ptr);

            return pod;
        }
        public static T Construct<T>(this object src, params object[] along)
        {
            return (T)Activator.CreateInstance(typeof(T), new List<object>().AddRange(src).With(_ => along.Each(_.Add)).ToArray());
        }
        public static bool IsNull(this object src)
        {
            return src == null;
        }
        public static bool IsNotNull(this object src)
        {
            return !src.IsNull();
        }
        public static T CheckNotNull<T>(this T source, Action backupPlan = null)
        {
            var predict = typeof(T) == typeof(string) ? string.IsNullOrEmpty(source as string) : source.IsNull();

            if (!predict) return source;

            backupPlan.IsNotNull().True(() => backupPlan()).False(() => { throw new NullReferenceException(); });
            return source;
        }
        public static T CheckArgument<T>(this T src, Func<T, bool> fn, Action<T> onUnexpected = null)
        {
            fn.CheckNotNull();
            src.CheckNotNull();
            if (!fn(src)) 
                onUnexpected.IsNotNull().True(() => onUnexpected(src)).False(() => { throw new ArgumentException(); });
            return src;
        }
        public static bool IsInSet<T>(this T src, params T[] list)
        {
            src.CheckNotNull();
            list.CheckNotNull();
            return list.Has(src);
        }
        public static string ToJsonString<T>(this T src)
        {
            return m_JsonSerializer.Serialize(src);
        }

        internal static readonly XmlWriterSettings m_GlobalXmlWriter = new XmlWriterSettings
        {
            CheckCharacters = true,
            NewLineChars = Environment.NewLine,
            Encoding = Encoding.UTF8,
            OmitXmlDeclaration = false
        };

        public static string ToXmlString<T>(this T src) 
        {
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            var ser = new XmlSerializer(typeof(T));

            using (var writer = new Utf8StringWriter())
            using (var xmlWriter = XmlWriter.Create(writer, m_GlobalXmlWriter))
            {
                ser.Serialize(xmlWriter, src, ns);
                return writer.ToString();
            }
        }
        public static T With<T>(this T src, Action<T> _)
        {
            _(src);
            return src;
        }

        public static T WithLock<T>(this T src, Action<T> _) 
            where T : class
        {
            lock (src) { return src.With(_); }
        }
    }
}
