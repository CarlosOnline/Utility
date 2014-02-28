using System;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace Utility
{
    public static class Extensions
    {
        /// <summary>
        /// String builder method for appending line with formatting.
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public static void AppendLine(this StringBuilder sb, string message, params object[] args)
        {
            sb.AppendLine(string.Format(message, args));
        }

        public static T ReadValue<T>(this SqlDataReader reader, int index)
        {
            var value = reader.GetValue(index);
            if (value != null && value != DBNull.Value)
            {
                return (T)value;
            }
            return default(T);
        }

        /// <summary>
        /// Get Value as type from DBRecord
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="record"></param>
        /// <param name="idx"></param>
        /// <returns></returns>
        public static T Value<T>(this IDataRecord record, int idx)
        {
            var value = record[idx];
            if (value != null && value != DBNull.Value)
            {
                return (T)value;
            }
            return default(T);
        }

        /// <summary>
        /// Extension: Get key value as type from NameValueCollection
        ///     ConfigurationManager.AppSettings.Value("refreshInterval", refreshInterval);
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T Value<T>(this NameValueCollection collection, string key, T defaultValue)
        {
            try
            {
                var value = collection[key];
                if (!String.IsNullOrEmpty(value))
                    return (T)Convert.ChangeType(value, typeof(T));
            }
            catch
            {
            }
            return defaultValue;
        }

    }

    public static class FileEx
    {
        public static byte[] ReadAllBytes(string filePath)
        {
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                var buffer = new byte[fs.Length];
                var read = fs.Read(buffer, 0, (int)fs.Length);
                if (read != (int)fs.Length)
                    throw new Exception(string.Format("Read bytes error. Expected: {0} Actual: {1} from {2}", read, fs.Length, filePath));
                return buffer;
            }
        }

        public static string ReadAllText(string filePath)
        {
            var buffer = ReadAllBytes(filePath);
            return System.Text.Encoding.ASCII.GetString(buffer);
        }

        public static string ReadAllText(string filePath, Encoding encoding)
        {
            var buffer = ReadAllBytes(filePath);
            return encoding.GetString(buffer);
        }
    }
}
