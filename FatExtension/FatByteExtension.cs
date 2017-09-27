using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace FatExtension
{
    public static class FatByteExtension
    {
        public static string ToBase64String(this byte[] buffer)
        {
            return Convert.ToBase64String(buffer.CheckNotNull());
        }
        public static string ToRawString(this byte[] buffer)
        {
            return ToRawString(buffer, Encoding.UTF8);
        }
        public static string ToRawString(this byte[] buffer, Encoding encoder)
        {
            return encoder.CheckNotNull(() => encoder = Encoding.Default).GetString(buffer.CheckNotNull());
        }
        public static string ToHexString(this byte[] buffer, bool lowerCase = true)
        {
            var hex = BitConverter.ToString(buffer.CheckNotNull()).Replace("-", string.Empty);
            return lowerCase ? hex.ToLower() : hex;
        }
        public static MemoryStream ToMemoryStream(this byte[] buffer)
        {
            return new MemoryStream(buffer.CheckNotNull());
        }
        public static byte[] Fill(this byte[] buffer, byte val)
        {
            buffer.CheckNotNull();
            0.To(buffer.Length, i => buffer[i] = val);
            return buffer;
        }
        public static byte[] HashBy<T>(this byte[] buffer)
            where T : HashAlgorithm
        {
            buffer.CheckNotNull();

            var cls = typeof(T).GetMethod("Create", Type.EmptyTypes, null);
            var cryptoService = cls.IsNotNull() ? cls.Invoke(null, null) as T : Activator.CreateInstance<T>();

            return cryptoService.CheckNotNull(() => { throw new CryptographicException(); }).ComputeHash(buffer);
        }
        public static byte[] EncryptBy<T>(this byte[] buffer, int blockSize, CipherMode mode, PaddingMode pad, 
            byte[] key, byte[] iv = null) 
            where T : SymmetricAlgorithm, new()
        {
            buffer.CheckNotNull();
            key.CheckNotNull();

            var inst = new T
            {
                BlockSize = blockSize,
                Mode = mode,
                Padding = pad,
                Key = key,
            };

            inst.IV = iv.CheckNotNull(() => iv = new byte[inst.LegalKeySizes[0].MinSize / 8]);

            using (var encrypt = inst.CreateEncryptor())
                return encrypt.TransformFinalBlock(buffer, 0, buffer.Length);
        }
        public static byte[] DecryptBy<T>(this byte[] buffer, int blockSize, CipherMode mode, PaddingMode pad, 
            byte[] key, byte[] iv = null) 
            where T : SymmetricAlgorithm, new()
        {
            buffer.CheckNotNull();
            key.CheckNotNull();

            var inst = new T
            {
                BlockSize = blockSize,
                Mode = mode,
                Padding = pad,
                Key = key,
            };

            inst.IV = iv.CheckNotNull(() => iv = new byte[inst.LegalKeySizes[0].MinSize / 8]);

            using (var decrypt = inst.CreateDecryptor())
                return decrypt.TransformFinalBlock(buffer, 0, buffer.Length);
        }
        public static T FromPlainOldData<T>(this byte[] pod) 
            where T : struct
        {
            pod.CheckNotNull();

            var len = Marshal.SizeOf(typeof(T));
            var ptr = Marshal.AllocHGlobal(len);

            Marshal.Copy(pod, 0, ptr, len); // copy pod into ptr
            var managedObject = (T)Marshal.PtrToStructure(ptr, typeof(T)).CheckNotNull();
            Marshal.FreeHGlobal(ptr);

            return managedObject;
        }
    }
}
