using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using static HCF.BDO.AGH.RequestConditions;

namespace HCF.Utility
{
    public static class Conversion
    {
        public static string GetSuffix(string day)
        {
            string suffix = "th";

            if (int.Parse(day) < 11 || int.Parse(day) > 20)
            {
                day = day.ToCharArray()[day.ToCharArray().Length - 1].ToString();
                switch (day)
                {
                    case "1":
                        suffix = "st";
                        break;
                    case "2":
                        suffix = "nd";
                        break;
                    case "3":
                        suffix = "rd";
                        break;
                }
            }
            return suffix;
        }

        public static DateTime ConvertToDateTime(double timestamp)
        {
            DateTime dateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
            dateTime = dateTime.AddSeconds(timestamp);
            return dateTime;
        }

        public static long ConvertToTimeSpan(DateTime? date)
        {
            long unixTimestamp = 0;
            if (date.HasValue)
            {
                unixTimestamp = (long)(date.Value.Subtract(new DateTime(1970, 1, 1, 0, 0, 0))).TotalSeconds;
                return unixTimestamp;
            }
            return unixTimestamp;
        }

        public static string Encrypt(string password, string EncryptionKey)
        {
            EncryptionKey = EncryptionKey.ToLower();
            var clearBytes = Encoding.Unicode.GetBytes(password);
            using (Aes encryptor = Aes.Create())
            {
                var pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    password = Convert.ToBase64String(ms.ToArray());
                }
            }
            return password;
        }

        public static string Decrypt(string cipherText, string encryptionKey)
        {
            encryptionKey = encryptionKey.ToLower();
            byte[] cipherBytes = Convert.FromBase64String(cipherText.Replace(" ", "+"));
            using (var encryptor = Aes.Create())
            {
                var pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        public static bool IsValidEmail(string emailString) => Regex.IsMatch(emailString, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);

        public static bool TryCastBoolean(object value)
        {
            if (value != null)
            {

                if (value is string)
                {
                    return (value as string).ToLower(CultureInfo.InvariantCulture) == "1";
                }

                if (bool.TryParse(value.ToString(), out bool retVal))
                {
                    return retVal;
                }
            }
            return false;
        }

        public static string TryCastString(object value)
        {
            if (value != null)
            {
                if (value is bool)
                {
                    return Convert.ToBoolean(value, CultureInfo.InvariantCulture) ? "1" : "0";
                }
                else
                {
                    if (value == DBNull.Value)
                    {
                        return string.Empty;
                    }
                    else
                    {
                        var retVal = value.ToString();
                        return retVal;
                    }
                }
            }
            else
            {
                return string.Empty;
            }
        }

        public static int TryCastInt32(object value)
        {
            if (value != null)
            {

                if (!string.IsNullOrEmpty(value.ToString()))
                {
                    return Convert.ToInt32(value);
                }
                else
                    return 0;
            }
            return 0;
        }

        public static int[] TryCastIntArray(object value)
        {
            int[] array = new int[0];
            if (value != null && !string.IsNullOrEmpty(value.ToString()))
            {
                array = value.ToString().Split(',').Select(Int32.Parse).ToArray();
            }
            return array;
        }

        public static Guid? ConvertToGuidOrNull(string value)
        {
            return (!string.IsNullOrEmpty(value)) ? Guid.Parse(value) : null;
        }

        public static Guid ConvertToGuid(string value)
        {
            return (!string.IsNullOrEmpty(value)) ? Guid.Parse(value) : Guid.Empty;
        }

        public static bool ConvertToBoolean(string value)
        {
            return (!string.IsNullOrEmpty(value)) ? Convert.ToBoolean(value) : false;
        }

        public static Guid TryCastGuid(object value)
        {
            if (value != null)
            {
                if (value is Guid)
                {
                    return Guid.Parse(value.ToString());
                }
                else
                    return Guid.Empty;
            }
            return Guid.Empty;
        }

        public static int? ToInt32(string v)
        {
            throw new NotImplementedException();
        }

        public static string Pluralize(int count)
        {
            return count > 1 ? "Plural" : "singular";
        }

        public static DateTime GetUtcToLocal(DateTime utc, string timeZone)
        {
            var temp = new DateTime(utc.Ticks, DateTimeKind.Utc);
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
            var ist = TimeZoneInfo.ConvertTimeFromUtc(temp, timeZoneInfo);
            return ist;

        }

        public static string ConvertToAmPm(TimeSpan timeSpan)
        {
            return timeSpan != null ? DateTime.Today.Add(timeSpan).ToString("hh:mm tt") : "";
        }

    }
}
