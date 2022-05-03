using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCF.BDO.Enums
{
    public enum Frequency
    {
        [Description("Daily")]
        Daily = 2,
        [Description("Weekly")]
        Weekly = 8,
        [Description("Monthly")]
        Monthly = 7,
        [Description("Ongoing")]
        Ongoing = 27,
        [Description("3 Days")]
        ThreeDays = 28,
        [Description("2 Months")]
        TwoMonths = 26,
        [Description("Quarterly")]
        Quarterly = 25,
        [Description("Semi Annually")]
        SemiAnnually = 6,
        [Description("9 Months")]
        ThreeXQuarterly = 21,
        [Description("Yearly")]
        Annually = 1,
        [Description("3 years")]
        Threeyears = 17,
        [Description("5 years")]
        Fiveyears = 9,
        [Description("6 years")]
        Sixyears = 12,
        [Description("3 Months")]
        ThreeMonths = 29

    }

    public enum RecurrencePattern
    {
        [Description("day(s)")]
        Daily = 2,
        [Description("week(s)")]
        Weekly = 8,
        [Description("month(s)")]
        Monthly = 7,
        [Description("year(s)")]
        Annually = 1,
    }

    public enum OtherRecurrencePattern
    {
        [Description("Recur Every")]
        Recur = 1,
        [Description("Every weekday")]
        Weekday = 2,
        [Description("Day")]
        day =3,
        [Description("On The")]
        onthe = 4,

        [Description("On")]
        on = 5,
    }

    public enum SequenceOcuurence
    {
       
        [Description("first")]
        first = 1,
        [Description("second")]
        second = 2,
        [Description("third")]
        third = 3,
        [Description("fourth")]
        fourth = 4,

        [Description("last")]
        last = 5,
    }

    public enum WeekDayOccurence
    {

        [Description("Sunday")]
        Sunday = 1,
        [Description("Monday")]
        Monday = 2,
        [Description("Tuesday")]
        Tuesday = 4,
        [Description("Wednesday")]
        Wednesday = 8,

        [Description("Thursday")]
        Thursday = 10,
        [Description("Friday")]
        Friday = 20,
        [Description("Saturday")]
        Saturday = 40,

    }

   
}
