using HCF.Utility.Configuration;
using System;


namespace HCF.Utility
{
    public static class DateTimeExtensions
    {
        public static DateTime ToClientTime(this DateTime dt)
        {
            var timeOffSet = ServiceLocator.ServiceProvider.GetService<IHCFSession>().Get<string>(SessionKey.TimeZoneOffSet);

            if (timeOffSet != null && dt.Year > 1)
            {
                var offset = int.Parse(timeOffSet.ToString());
                dt = dt.AddMinutes(-1 * offset);

                return dt;
            }


            // if there is no offset in session return the datetime in server timezone
            return dt.ToLocalTime();
        }

        public static string CastLocaleDate(this DateTime? dateTime)
        {
            if (dateTime.HasValue)
                return dateTime.Value.ToString("MMM d, yyyy");
            else
                return string.Empty;
        }
        
        public static string CastDate(this DateTime? dateTime)
        {
            if (dateTime.HasValue)
                return dateTime.Value.ToClientTime().ToFormatDate();
            else
                return string.Empty;
        }

        public static string DateSort(this DateTime? dateTime)
        {
            if (dateTime.HasValue)
                return Convert.ToString(dateTime.Value.Ticks);
            else
                return string.Empty;
        }

        public static string CastDateTime(this DateTime? dateTime)
        {
            if (dateTime.HasValue)
                return dateTime.Value.ToClientTime().ToFormatDateTime();
            else
                return string.Empty;
        }

        public static string CastLocaleDateTime(this DateTime? dateTime)
        {
            if (dateTime.HasValue)
                return dateTime.Value.ToFormatDateTime();
            else
                return string.Empty;
        }

        public static string ToFormatDateTime(this DateTime dt)
        {
            return dt.ToString("MMM d, yyyy hh:mm tt");
        }

        public static string ToFormatDate(this DateTime dt)
        {
            return dt.ToString("MMM d, yyyy");
        }        

        public static string ToFormatDate(this DateTime? dt)
        {
            if (dt.HasValue)
                return dt.Value.ToString("MMM d, yyyy");
            return "";
        }


        public static DateTime ToUTCTime(this DateTime dt)
        {
            var sessionService = ServiceLocator.ServiceProvider.GetService<IHCFSession>();
            var timeOffSet = sessionService.Get<string>(SessionKey.TimeZoneOffSet); //HttpContext.Current.Session[Utility.SessionKey.TimeZoneOffSet];  // read the value from session

            if (timeOffSet != null)
            {
                var offset = int.Parse(timeOffSet.ToString());
                dt = dt.AddMinutes(1 * offset);
                return dt;
            }
            // if there is no offset in session return the datetime in server timezone
            return dt.ToLocalTime();
        }

        public static string ConvertToString(this TimeSpan? time)
        {
            if (time.HasValue)
            {
                var datetime = new DateTime(time.Value.Ticks).ToString("hh:mm\\ tt");
                return datetime;
            }
            else
                return string.Empty;
        }
    }
}
