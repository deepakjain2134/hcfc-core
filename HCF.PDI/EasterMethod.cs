using System;

namespace HCF.PDI
{
    /// <summary>
    /// This enumerated type defines the various ways to calculate Easter
    /// </summary>
    [Serializable]
    public enum EasterMethod
    {
        /// <summary>Calculate Easter as the Sunday following the Paschal Full Moon (PFM) date for the year based
        /// on the Julian Calendar.  This method is valid for all years from 326 onward.</summary>
        Julian,
        /// <summary>This method is the same as the Julian method but converts the Julian calendar date to the
        /// equivalent Gregorian calendar date. This method is valid for all years from 1583 to 4099.</summary>
        Orthodox,
        /// <summary>Calculate Easter as the Sunday following the Paschal Full Moon (PFM) date for the year based
        /// on the Gregorian Calendar.  This method is valid for all years from 1583 to 4099.</summary>
        Gregorian
    }
}
