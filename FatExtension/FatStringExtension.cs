using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml.Serialization;

namespace FatExtension
{
    public static class FatStringExtension
    {
        public static string FormatWith(this string text, params object[] args)
        {
            return string.Format(text, args);
        }
        public static string FormatNamed(this string formatString, object parameters)
        {
            return parameters.CheckNotNull().GetType().GetProperties()
                    .Aggregate(formatString, (current, p) => current.Replace("{" + p.Name + "}", p.GetValue(parameters, null) as string));
        }       
        public static string ToBase64String(this string text)
        {
            return text.ToByteArray().ToBase64String();
        }
        public static string FromBase64ToString(this string text)
        {
            return Convert.FromBase64String(text).ToRawString();
        }
        public static string FixSlashes(this string text)
        {
            return Regex.Replace(text, @"([^:][/|\\]\s*[/|\\]+|\\)", "/").Trim('/', '\\', ' ', '\n');
        }
        public static string Reverse(this string text)
        {
            text.CheckNotNull();
            return text.ToByteArray().Reverse().ToRawString();
        }
        public static string ToTitleCase(this string text)
        {
            text.CheckNotNull();
            return Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(text.ToLower());
        }
        public static bool IsJsonString(this string ptr)
        {
            try
            {
                ptr.FromJsonString<object>();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool IsXmlString(this string ptr)
        {
            try
            {
                ptr.FromXmlString<object>();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool IsNullOrEmpty(this string ptr)
        {
            return string.IsNullOrEmpty(ptr);
        }
        public static bool IsValidEmailAddress(this string toCheck)
        {
            return toCheck.Matches(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,6}$");
        }
        public static bool Matches(this string value, string pattern)
        {
            return new Regex(pattern).IsMatch(value);
        }
        public static bool ContainsUnicode(this string text)
        {
            return text.Any(c => c > 255);
        }
        public static byte[] ToByteArray(this string text)
        {
            return ToByteArray(text, Encoding.UTF8);
        }
        public static byte[] ToByteArray(this string text, Encoding encoder)
        {
            return encoder.GetBytes(text);
        }        
        public static int CountUppers(this string lookupString)
        {
            return lookupString.Count(c => c >= 'A' && c <= 'Z');
        }
        public static int CountLowers(this string lookupString)
        {
            return lookupString.Count(c => c >= 'a' && c <= 'z');
        }
        public static int CountNumbers(this string lookupString)
        {
            return lookupString.Count(c => c >= '0' && c <= '9');
        }
        public static int CountUnicodes(this string lookupString)
        {
            return lookupString.Count(c => c > 255);
        }
        public static T FromJsonString<T>(this string source)
        {
            return FatGeneralExtension.m_JsonSerializer.Deserialize<T>(source);
        }
        public static T FromXmlString<T>(this string source)
        {
            var ser = new XmlSerializer(typeof(T));

            using (var ms = source.ToStream())
                return (T) ser.Deserialize(ms);
        }
        public static Stream ToStream(this string source)
        {
            return source.ToByteArray().ToMemoryStream();
        }
        public static StringBuilder ToBuilder(this string source)
        {
            return new StringBuilder(source);
        }
        public static void Ajax(this string url, string method, string contentType, string data, 
            Action<WebResponse, string> onSuccess, Dictionary<string, string> headers = null )
        {
            method.CheckNotNull();
            data.CheckNotNull(() => data = "");
            method = method.ToUpper();

            (WebRequest.Create(url) as HttpWebRequest).CheckNotNull().With(_ =>
            {
                headers.IsNotNull().True(() => headers.Each(kv => _.Headers.Add(kv.Key, kv.Value)));

                _.Method = method;
                _.ContentLength = data.Length;
                _.ContentType = contentType.IsNullOrEmpty() ? "application/octet-stream" : contentType;
                _.UserAgent = "Mozilla/4.0";
                
                var block = data.ToByteArray();
                (method != "GET").True(() => _.GetRequestStream().Write(block, 0, block.Length) );
                _.BeginGetResponse(ar =>
                {
                    var response = _.EndGetResponse(ar).CheckNotNull();
                    using (var reader = new StreamReader(response.GetResponseStream().CheckNotNull()))
                        onSuccess(response, reader.ReadToEnd());
                }, null);
            });
        }
    }
}
