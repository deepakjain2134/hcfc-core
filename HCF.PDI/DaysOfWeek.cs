using System;

namespace HCF.PDI
{
    /// <summary>
    /// This enumerated type defines the days of the week on which a date instance can occur.  Unlike
    /// <see cref="System.DayOfWeek" />, these values are bit flags so that combinations of days can be
    /// specified.
    /// </summary>
    [Flags, Serializable]
    public enum DaysOfWeek
    {
        /// <summary>No days</summary>
        [LRDescription("DWNone")]
        None =      0x00000000,
        /// <summary>Occurs on Sunday</summary>
        Sunday =    0x00000001,
        /// <summary>Occurs on Monday</summary>
        Monday =    0x00000002,
        /// <summary>Occurs on Tuesday</summary>
        Tuesday =   0x00000004,
        /// <summary>Occurs on Wednesday</summary>
        Wednesday = 0x00000008,
        /// <summary>Occurs on Thursday</summary>
        Thursday=   0x00000010,
        /// <summary>Occurs on Friday</summary>
        Friday =    0x00000020,
        /// <summary>Occurs on Saturday</summary>
        Saturday =  0x00000040,
        /// <summary>Occurs on weekdays only (Monday through Friday)</summary>
        [LRDescription("DWWeekdays")]
        Weekdays =  0x0000003E,
        /// <summary>Occurs on weekends only (Saturday and Sunday)</summary>
        [LRDescription("DWWeekends")]
        Weekends =  0x00000041,
        /// <summary>Occurs on every day of the week</summary>
        [LRDescription("DWEveryDay")]
        EveryDay =  0x0000007F
    }
}
