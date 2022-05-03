using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCF.BDO.Extensions
{
    public static class Conversion
    {
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
    }
}
