using NHS111.Web.Presentation.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace NHS111.Web.Helpers
{
    public class QueryStringEncryptor : Dictionary<string, string>
    {
        private readonly IConfiguration _configuration;

        // Change the following keys to ensure uniqueness
        // Must be 8 bytes
        protected byte[] _keyBytes = { 0x0f, 0xfc, 0x71, 0x38, 0x24, 0x09, 0x62, 0xb1 };

        // Must be at least 8 characters
        protected string _keyString;

        // Name for checksum value (unlikely to be used as arguments by user)
        protected string _checksumKey = "__$$";

        /// <summary>
        /// Creates an empty dictionary
        /// </summary>
        public QueryStringEncryptor() : this(new Configuration())
        {
        }

        private QueryStringEncryptor(IConfiguration configuration)
        {
            _configuration = configuration;
            _keyString = _configuration.QueryStringEncryptionKey;
            _keyBytes = Convert.FromBase64String(_configuration.QueryStringEncryptionBytes);
        }

        /// <summary>
        /// Creates a dictionary from the given, encrypted string
        /// </summary>
        /// <param name="encryptedData"></param>
        public QueryStringEncryptor(string encryptedData) : this(new Configuration())
        {
            if (string.IsNullOrEmpty(encryptedData))
                throw new ArgumentException("Can't decrypt a null or empty string.");

            // Descrypt string
            var data = Decrypt(encryptedData);

            // Parse out key/value pairs and add to dictionary
            string checksum = null;
            var args = data.Split('&');

            foreach (var arg in args)
            {
                var i = arg.IndexOf('=');
                if (i == -1) continue;

                var key = arg.Substring(0, i);
                var value = arg.Substring(i + 1);
                if (key == _checksumKey)
                    checksum = value;
                else
                    base.Add(HttpUtility.UrlDecode(key), HttpUtility.UrlDecode(value));
            }

            // Clear contents if valid checksum not found
            if (checksum == null || checksum != ComputeChecksum())
                base.Clear();
        }

        /// <summary>
        /// Returns an encrypted string that contains the current dictionary
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            // Build query string from current contents
            var content = new StringBuilder();

            foreach (var key in base.Keys)
            {
                if (content.Length > 0)
                    content.Append('&');
                content.AppendFormat("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(base[key]));
            }

            // Add checksum
            if (content.Length > 0)
                content.Append('&');
            content.AppendFormat("{0}={1}", _checksumKey, ComputeChecksum());

            return Encrypt(content.ToString());
        }

        /// <summary>
        /// Returns a simple checksum for all keys and values in the collection
        /// </summary>
        /// <returns></returns>
        protected string ComputeChecksum()
        {
            var checksum = 0;

            foreach (var pair in this)
            {
                checksum += pair.Key.Sum(c => c - '0');
                checksum += pair.Value.Sum(c => c - '0');
            }

            return checksum.ToString("X");
        }

        /// <summary>
        /// Encrypts the given text
        /// </summary>
        /// <param name="text">Text to be encrypted</param>
        /// <returns></returns>
        protected string Encrypt(string text)
        {
            try
            {
                var keyData = Encoding.UTF8.GetBytes(_keyString.Substring(0, 8));
                var des = new DESCryptoServiceProvider();
                var textData = Encoding.UTF8.GetBytes(text);
                var ms = new MemoryStream();
                var cs = new CryptoStream(ms, des.CreateEncryptor(keyData, _keyBytes), CryptoStreamMode.Write);
                cs.Write(textData, 0, textData.Length);
                cs.FlushFinalBlock();
                return GetString(ms.ToArray());
            }
            catch (Exception)
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// Decrypts the given encrypted text
        /// </summary>
        /// <param name="text">Text to be decrypted</param>
        /// <returns></returns>
        protected string Decrypt(string text)
        {
            try
            {
                var keyData = Encoding.UTF8.GetBytes(_keyString.Substring(0, 8));
                var des = new DESCryptoServiceProvider();
                var textData = GetBytes(text);
                var ms = new MemoryStream();
                var cs = new CryptoStream(ms, des.CreateDecryptor(keyData, _keyBytes), CryptoStreamMode.Write);
                cs.Write(textData, 0, textData.Length);
                cs.FlushFinalBlock();
                return Encoding.UTF8.GetString(ms.ToArray());
            }
            catch (Exception)
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// Converts a byte array to a string of hex characters
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected string GetString(byte[] data)
        {
            var results = new StringBuilder();

            foreach (byte b in data)
                results.Append(b.ToString("X2"));

            return results.ToString();
        }

        /// <summary>
        /// Converts a string of hex characters to a byte array
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected byte[] GetBytes(string data)
        {
            // GetString() encodes the hex-numbers with two digits
            var results = new byte[data.Length / 2];

            for (var i = 0; i < data.Length; i += 2)
                results[i / 2] = Convert.ToByte(data.Substring(i, 2), 16);

            return results;
        }
    }
}