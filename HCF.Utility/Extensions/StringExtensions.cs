using System.Globalization;
using System.Text.RegularExpressions;

namespace HCF.Utility
{
    public static partial class StringExtensions
    {
        public static string CastToNA(this string text, string value = "NA")
        {
            if (!string.IsNullOrEmpty(text))
                return text;
            else
                return value;
        }

        public static string ToTitleCase(this string text)
        {
            if (!string.IsNullOrEmpty(text))
                return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text.ToLower());
            else
                return text;
        }

        public static bool IsValidEmailAddress(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return false;
            Regex regex = new(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            bool Isvalid = regex.IsMatch(text.Trim());
            return Isvalid;
        }

    }
}
