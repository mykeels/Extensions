using System;
using System.Security.Cryptography;
using System.Configuration;

namespace Extensions
{
    public class Encryption
    {

        public static string pass = "password";
        public static byte[] EncryptBytes(byte[] b, string password = null)
        {
            if (password == null) password = pass;
            System.Security.Cryptography.RijndaelManaged AES = new System.Security.Cryptography.RijndaelManaged();
            System.Security.Cryptography.MD5CryptoServiceProvider Hash_AES = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] encrypted = null;
            try
            {
                byte[] hash = new byte[32];
                byte[] temp = Hash_AES.ComputeHash(System.Text.ASCIIEncoding.ASCII.GetBytes(password));
                Array.Copy(temp, 0, hash, 0, 16);
                Array.Copy(temp, 0, hash, 15, 16);
                AES.Key = hash;
                AES.Mode = CipherMode.ECB;
                System.Security.Cryptography.ICryptoTransform DESEncrypter = AES.CreateEncryptor();
                byte[] Buffer = b;
                encrypted = DESEncrypter.TransformFinalBlock(Buffer, 0, Buffer.Length);
                return encrypted;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static byte[] DecryptBytes(byte[] b, string password = null)
        {
            if (password == null) password = pass;
            System.Security.Cryptography.RijndaelManaged AES = new System.Security.Cryptography.RijndaelManaged();
            System.Security.Cryptography.MD5CryptoServiceProvider Hash_AES = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] decrypted = null;
            try
            {
                byte[] hash = new byte[32];
                byte[] temp = Hash_AES.ComputeHash(System.Text.ASCIIEncoding.ASCII.GetBytes(password));
                Array.Copy(temp, 0, hash, 0, 16);
                Array.Copy(temp, 0, hash, 15, 16);
                AES.Key = hash;
                AES.Mode = CipherMode.ECB;
                System.Security.Cryptography.ICryptoTransform DESDecrypter = AES.CreateDecryptor();
                byte[] Buffer = b;
                decrypted = DESDecrypter.TransformFinalBlock(Buffer, 0, Buffer.Length);
                return decrypted;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static string EncryptString(string input, string password = null)
        {
            return Convert.ToBase64String(EncryptBytes(System.Text.ASCIIEncoding.ASCII.GetBytes(input), password));
        }

        public static string DecryptString(string input, string password = null)
        {
            return System.Text.Encoding.UTF8.GetString(DecryptBytes(Convert.FromBase64String(input), password));
        }

        public static byte[] Compress(byte[] raw)
        {
            // Clean up memory with Using-statements.
            using (System.IO.MemoryStream memory = new System.IO.MemoryStream())
            {
                // Create compression stream.
                using (System.IO.Compression.GZipStream gzip = new System.IO.Compression.GZipStream(memory, System.IO.Compression.CompressionMode.Compress, true))
                {
                    // Write.
                    gzip.Write(raw, 0, raw.Length);
                }
                // Return array.
                return memory.ToArray();
            }
        }

        public static byte[] DeCompress(byte[] raw)
        {
            // Clean up memory with Using-statements.
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream(raw))
            {
                using (System.IO.Compression.GZipStream gzs = new System.IO.Compression.GZipStream(ms, System.IO.Compression.CompressionMode.Decompress))
                {
                    using (System.IO.StreamReader rdr = new System.IO.StreamReader(gzs))
                    {
                        string s = rdr.ReadToEnd();
                        return System.Text.Encoding.UTF8.GetBytes(s);
                    }
                    //rdr
                }
                //gzs
            }
            //ms
            return null;
        }
    }
}
