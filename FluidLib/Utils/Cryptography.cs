using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FluidLib.Utils
{
    public class Cryptography
    {
        public struct CryptoResult_Text
        {
            public string Text;
            public bool Done;
        }
        public struct CryptoResult_Bytes
        {
            public byte[] Bytes;
            public bool Done;
        }

        public static CryptoResult_Text Encrypt(string text, string password, byte[] saltBytes)
        {
            CryptoResult_Text result = new CryptoResult_Text();
            result.Text = text;
            result.Done = false;

            if (string.IsNullOrEmpty(text))
                return result;

            if (string.IsNullOrEmpty(password))
                return result;

            byte[] data = Encoding.UTF8.GetBytes(text);
            byte[] _password = Encoding.UTF8.GetBytes(password);

            _password = SHA256.Create().ComputeHash(_password);

            CryptoResult_Bytes _result = Encrypt(data, _password, new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 });
            if (!_result.Done)
                return result;

            result.Done = true;
            result.Text = Encoding.UTF8.GetString(_result.Bytes);

            return result;
        }
        public static CryptoResult_Bytes Encrypt(byte[] data, string password, byte[] saltBytes)
        {
            byte[] _password = Encoding.UTF8.GetBytes(password);
            return Encrypt(data, _password, saltBytes);
        }
        //saltBytes should be atleast 8 (example: { 1, 2, 3, 4, 5, 6, 7, 8 });
        public static CryptoResult_Bytes Encrypt(byte[] data, byte[] password, byte[] saltBytes)
        {
            CryptoResult_Bytes result = new CryptoResult_Bytes();
            result.Bytes = data;
            result.Done = false;
            
            using (MemoryStream ms = new MemoryStream())
            using (RijndaelManaged AES = new RijndaelManaged())
            {
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(password, saltBytes, 1000);

                AES.KeySize = 256;
                AES.BlockSize = 128;
                AES.Key = key.GetBytes(AES.KeySize / 8);
                AES.IV = key.GetBytes(AES.BlockSize / 8);

                AES.Mode = CipherMode.CBC;

                using (CryptoStream cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(data, 0, data.Length);
                    cs.Close();
                }

                data = ms.ToArray();

                result.Bytes = data;
                result.Done = true;
            }

            return result;
        }

        public static CryptoResult_Text Decrypt(string text, string password, byte[] saltBytes)
        {
            CryptoResult_Text result = new CryptoResult_Text();
            result.Text = text;
            result.Done = false;

            if (string.IsNullOrEmpty(text))
                return result;

            if (string.IsNullOrEmpty(password))
                return result;

            byte[] data = Convert.FromBase64String(text);
            byte[] _password = Encoding.UTF8.GetBytes(password);

            _password = SHA256.Create().ComputeHash(_password);

            CryptoResult_Bytes _result = Decrypt(data, _password, saltBytes);
            if (!_result.Done)
                return result;

            result.Done = _result.Done;
            result.Text = Encoding.UTF8.GetString(_result.Bytes);

            return result;
        }
        public static CryptoResult_Bytes Decrypt(byte[] data, string password, byte[] saltBytes)
        {
            byte[] _password = Encoding.UTF8.GetBytes(password);
            return Decrypt(data, _password, saltBytes);
        }
        public static CryptoResult_Bytes Decrypt(byte[] data, byte[] password, byte[] saltBytes)
        {
            CryptoResult_Bytes result = new CryptoResult_Bytes();
            result.Bytes = data;
            result.Done = false;

            using (MemoryStream ms = new MemoryStream())
            using (RijndaelManaged AES = new RijndaelManaged())
            {
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(password, saltBytes, 1000);

                AES.KeySize = 256;
                AES.BlockSize = 128;
                AES.Key = key.GetBytes(AES.KeySize / 8);
                AES.IV = key.GetBytes(AES.BlockSize / 8);

                AES.Mode = CipherMode.CBC;

                using (CryptoStream cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(data, 0, data.Length);
                    cs.Close();
                }

                result.Done = true;
                result.Bytes = ms.ToArray();
            }

            return result;
        }
    }
}
