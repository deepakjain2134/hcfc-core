using HCF.BDO.Enums;
using HCF.PDI;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;


namespace HCF.Utility
{
   

    public class ScheduleDateTime
    {
        static readonly System.Globalization.DateTimeFormatInfo mfi = new System.Globalization.DateTimeFormatInfo();

        //public static string Convertruletext(int frequencyId, DateTime startDate, int? yearNo, int? monthNo, int? weekNo = 0, int? weekDays = 0)
        //{
        //    var weekNotext = $"{weekNo}{Common.GetDaySuffix(Convert.ToInt32(weekNo))}";
        //    var monthtext = $"{monthNo}{Common.GetDaySuffix(Convert.ToInt32(monthNo))}";
        //    var ruletext = string.Empty;
        //    switch (frequencyId)
        //    {
        //        case (int)Frequency.Daily:
        //            return "Daily at 12:00AM";
        //        case (int)Frequency.Weekly:
        //            return $"Every {(DayOfWeek) weekDays - 1}";
        //        case (int)Frequency.Monthly:
        //            return $"Every {weekNotext} {(DayOfWeek) weekDays - 1} of month";
        //        case (int)Frequency.SemiAnnually:
        //            return $"Every {weekNotext} {(DayOfWeek) weekDays - 1} of {monthtext} month";
        //        case (int)Frequency.Annually:
        //            return
        //                $"Every {weekNotext} {(DayOfWeek) weekDays - 1} of {mfi.GetMonthName(monthNo.HasValue && monthNo.Value > 0 ? monthNo.Value : 1).ToString()}";
        //        case (int)Frequency.Quarterly:
        //            return $"Every {weekNotext} {(DayOfWeek) weekDays - 1} of {monthtext} month";
        //        case (int)Frequency.Sixyears:
        //            return string.Format("Every {0} {1} of {2}", weekNotext, (DayOfWeek)weekDays - 1, mfi.GetMonthName(monthNo.HasValue && monthNo.Value > 0 ? monthNo.Value : 1).ToString());
        //        case (int)Frequency.Fiveyears:
        //            return
        //                $"Every {weekNotext} {(DayOfWeek) weekDays - 1} of {mfi.GetMonthName(monthNo.HasValue && monthNo.Value > 0 ? monthNo.Value : 1).ToString()}";
        //        case (int)Frequency.TwoMonths:
        //            return $"Every {weekNotext} {(DayOfWeek) weekDays - 1} of month";
        //        case (int)Frequency.Threeyears:
        //            return
        //                $"Every {weekNotext} {(DayOfWeek) weekDays - 1} of {mfi.GetMonthName(monthNo.HasValue && monthNo.Value > 0 ? monthNo.Value : 1).ToString()}";
        //    }
        //    return ruletext;
        //}


        #region Next Date

        private static DaysOfWeek GetDayOfWeek(int day)
        {
            var ob = new DaysOfWeek();
            switch (day)
            {
                case 1:
                    ob = DaysOfWeek.Sunday;
                    break;
                case 2:
                    ob = DaysOfWeek.Monday;
                    break;
                case 3:
                    ob = DaysOfWeek.Tuesday;
                    break;
                case 4:
                    ob = DaysOfWeek.Wednesday;
                    break;
                case 5:
                    ob = DaysOfWeek.Thursday;
                    break;
                case 6:
                    ob = DaysOfWeek.Friday;
                    break;
                case 7:
                    ob = DaysOfWeek.Saturday;
                    break;
            }
            return ob;
        }

        private static DayOccurrence GetDayOccurrence(int weekNumber)
        {
            DayOccurrence ob = new DayOccurrence();
            switch (weekNumber)
            {
                case 1:
                    ob = DayOccurrence.First;
                    break;
                case 2:
                    ob = DayOccurrence.Second;
                    break;
                case 3:
                    ob = DayOccurrence.Third;
                    break;
                case 4:
                    ob = DayOccurrence.Fourth;
                    break;
                case 5:
                    ob = DayOccurrence.Last;
                    break;
            }
            return ob;
        }

        public static DateTime GetScheduleCustomDate(int frequencyId, DateTime startDate, int month, int week, int day, int sequence)
        {
            var recurDates = new DateTimeCollection();
            var rRecur = new Recurrence { StartDateTime = startDate, RecurUntil = RecurUntil(frequencyId, startDate) };

            switch (frequencyId)
            {
                case (int)Frequency.Daily:
                    rRecur.RecurDaily(1);
                    recurDates = rRecur.InstancesBetween(startDate, DateTime.Today.AddDays(5));
                    break;
                case (int)Frequency.ThreeDays:
                    rRecur.RecurDaily(3);
                    recurDates = rRecur.InstancesBetween(startDate, DateTime.Today.AddDays(15));
                    break;
                case (int)Frequency.Weekly:
                    rRecur.RecurWeekly(1, GetDayOfWeek(day));
                    recurDates = rRecur.InstancesBetween(startDate, DateTime.Today.AddMonths(1));
                    break;
                case (int)Frequency.Monthly:
                    rRecur.RecurMonthly(GetDayOccurrence(week), GetDayOfWeek(day), 1);
                    recurDates = rRecur.InstancesBetween(startDate, DateTime.Today.AddMonths(3));
                    break;
                case (int)Frequency.TwoMonths:
                    rRecur.RecurMonthly(GetDayOccurrence(week), GetDayOfWeek(day), 2);
                    recurDates = rRecur.InstancesBetween(startDate, DateTime.Today.AddMonths(6));
                    break;
                case (int)Frequency.Quarterly:
                    rRecur.RecurMonthly(GetDayOccurrence(week), GetDayOfWeek(day), 3);
                    recurDates = rRecur.InstancesBetween(startDate, DateTime.Today.AddMonths(6));
                    break;
                case (int)Frequency.SemiAnnually:
                    rRecur.RecurMonthly(GetDayOccurrence(week), GetDayOfWeek(day), 6);
                    recurDates = rRecur.InstancesBetween(startDate, DateTime.Today.AddMonths(12));
                    break;
                case (int)Frequency.Annually:
                    rRecur.RecurYearly(GetDayOccurrence(week), GetDayOfWeek(day), month, 1);
                    recurDates = rRecur.InstancesBetween(startDate, DateTime.Today.AddYears(3));
                    break;
                case (int)Frequency.Threeyears:
                    rRecur.RecurYearly(GetDayOccurrence(week), GetDayOfWeek(day), month, 3);
                    recurDates = rRecur.InstancesBetween(startDate, DateTime.Today.AddYears(5));
                    break;
                case (int)Frequency.Fiveyears:
                    rRecur.RecurYearly(GetDayOccurrence(week), GetDayOfWeek(day), month, 5);
                    recurDates = rRecur.InstancesBetween(startDate, DateTime.Today.AddYears(7));
                    break;
                case (int)Frequency.Sixyears:
                    rRecur.RecurYearly(GetDayOccurrence(week), GetDayOfWeek(day), month, 6);
                    recurDates = rRecur.InstancesBetween(startDate, DateTime.Today.AddYears(8));
                    break;
            }
            var nearestDate = recurDates.Where(x => x >= DateTime.Today).ToList()[sequence];
            return nearestDate;
        }

        public static DateTime GetScheduleFixedDate(int frequencyId, DateTime startDate, int sequence)
        {
            var recurDates = new DateTimeCollection();
            var rRecur = new Recurrence
            {
                StartDateTime = startDate,
                RecurUntil = RecurUntil(frequencyId, startDate)
            };
            switch (frequencyId)
            {
                case (int)Frequency.Daily:
                    rRecur.RecurDaily(1);
                    recurDates = rRecur.InstancesBetween(startDate, DateTime.Today.AddDays(5));
                    break;
                case (int)Frequency.ThreeDays:
                    rRecur.RecurDaily(3);
                    rRecur.RecurUntil = DateTime.Today.AddDays(15);
                    recurDates = rRecur.InstancesBetween(startDate, DateTime.Today.AddDays(15));
                    break;
                case (int)Frequency.Weekly:
                    rRecur.RecurWeekly(1, DaysOfWeek.Monday);
                    recurDates = rRecur.InstancesBetween(startDate, DateTime.Today.AddMonths(1));
                    break;
                case (int)Frequency.Monthly:
                    rRecur.RecurMonthly(startDate.Day, 1);
                    recurDates = rRecur.InstancesBetween(startDate, DateTime.Today.AddMonths(3));
                    break;
                case (int)Frequency.TwoMonths:
                    rRecur.RecurMonthly(startDate.Day, 2);
                    recurDates = rRecur.InstancesBetween(startDate, DateTime.Today.AddMonths(6));
                    break;
                case (int)Frequency.Quarterly:
                    rRecur.RecurMonthly(startDate.Day, 3);
                    recurDates = rRecur.InstancesBetween(startDate, DateTime.Today.AddMonths(6));
                    break;
                case (int)Frequency.ThreeXQuarterly:
                    rRecur.RecurMonthly(startDate.Day, 9);
                    recurDates = rRecur.InstancesBetween(startDate, DateTime.Today.AddMonths(20));
                    break;
                case (int)Frequency.SemiAnnually:
                    rRecur.RecurMonthly(startDate.Day, 6);
                    recurDates = rRecur.InstancesBetween(startDate, DateTime.Today.AddMonths(12));
                    break;
                case (int)Frequency.Annually:
                    rRecur.RecurYearly(startDate.Month, startDate.Day, 1);
                    recurDates = rRecur.InstancesBetween(startDate, DateTime.Today.AddYears(3));
                    break;
                case (int)Frequency.Threeyears:
                    rRecur.RecurYearly(startDate.Month, startDate.Day, 3);
                    recurDates = rRecur.InstancesBetween(startDate, DateTime.Today.AddYears(5));
                    break;
                case (int)Frequency.Fiveyears:
                    rRecur.RecurYearly(startDate.Month, startDate.Day, 5);
                    recurDates = rRecur.InstancesBetween(startDate, DateTime.Today.AddYears(7));
                    break;
                case (int)Frequency.Sixyears:
                    rRecur.RecurYearly(startDate.Month, startDate.Day, 6);
                    recurDates = rRecur.InstancesBetween(startDate, DateTime.Today.AddYears(8));
                    break;
            }
            if (recurDates.Count(x => x >= DateTime.Today) > 0)
            {
                var nearestDate = recurDates.Where(x => x >= DateTime.Today).ToList()[sequence];
                if (nearestDate != null)
                    return nearestDate;
                else
                    return DateTime.Today;
            }
            else
                return DateTime.Today;
        }

        private static DateTime RecurUntil(int frequencyId, DateTime startDate)
        {
            var recurUntil = DateTime.Today;
            switch (frequencyId)
            {
                case (int)Frequency.Daily:
                    recurUntil = (startDate > DateTime.Today) ? startDate.AddDays(5) : DateTime.Today.AddDays(5);
                    break;
                case (int)Frequency.ThreeDays:
                    recurUntil = (startDate > DateTime.Today) ? startDate.AddDays(15) : DateTime.Today.AddDays(15);
                    break;
                case (int)Frequency.Weekly:
                    recurUntil = (startDate > DateTime.Today) ? startDate.AddMonths(1) : DateTime.Today.AddMonths(1);
                    break;
                case (int)Frequency.Monthly:
                    recurUntil = (startDate > DateTime.Today) ? startDate.AddMonths(3) : DateTime.Today.AddMonths(3);
                    break;
                case (int)Frequency.TwoMonths:
                    recurUntil = (startDate > DateTime.Today) ? startDate.AddMonths(6) : DateTime.Today.AddMonths(6);
                    break;
                case (int)Frequency.Quarterly:
                    recurUntil = (startDate > DateTime.Today) ? startDate.AddMonths(9) : DateTime.Today.AddMonths(9);
                    break;
                case (int)Frequency.ThreeXQuarterly:
                    recurUntil = (startDate > DateTime.Today) ? startDate.AddMonths(9) : DateTime.Today.AddMonths(9);
                    break;
                case (int)Frequency.SemiAnnually:
                    recurUntil = (startDate > DateTime.Today) ? startDate.AddMonths(12) : DateTime.Today.AddMonths(12);
                    break;
                case (int)Frequency.Annually:
                    recurUntil = (startDate > DateTime.Today) ? startDate.AddYears(3) : DateTime.Today.AddYears(3);
                    break;
                case (int)Frequency.Threeyears:
                    recurUntil = (startDate > DateTime.Today) ? startDate.AddYears(5) : DateTime.Today.AddYears(5);
                    break;
                case (int)Frequency.Fiveyears:
                    recurUntil = (startDate > DateTime.Today) ? startDate.AddYears(7) : DateTime.Today.AddYears(7);
                    break;
                case (int)Frequency.Sixyears:
                    recurUntil = (startDate > DateTime.Today) ? startDate.AddYears(8) : DateTime.Today.AddYears(8);
                    break;
            }
            return recurUntil;
        }

        public static DateTime RecurUntilEpNotification(int frequencyId, DateTime startDate, int sequence)
        {
            var recurDates = new DateTimeCollection();
            var rRecur = new Recurrence
            {
                StartDateTime = startDate,
                RecurUntil = RecurUntil(frequencyId, startDate)
            };
            switch (frequencyId)
            {
                case (int)Frequency.Daily:
                    rRecur.RecurDaily(1);
                    recurDates = rRecur.InstancesBetween(startDate, startDate.AddDays(1));
                    break;
                case (int)Frequency.ThreeDays:
                    rRecur.RecurDaily(3);
                    //rRecur.RecurUntil = DateTime.Today.AddDays(3);
                    recurDates = rRecur.InstancesBetween(startDate, startDate.AddDays(3));
                    break;
                case (int)Frequency.Weekly:
                    rRecur.RecurWeekly(7, DaysOfWeek.Monday);
                    recurDates = rRecur.InstancesBetween(startDate, startDate.AddDays(7));
                    break;
                case (int)Frequency.Monthly:
                    rRecur.RecurMonthly(startDate.Day, 1);
                    recurDates = rRecur.InstancesBetween(startDate, startDate.AddMonths(1));
                    break;
                case (int)Frequency.TwoMonths:
                    rRecur.RecurMonthly(startDate.Day, 2);
                    recurDates = rRecur.InstancesBetween(startDate, startDate.AddMonths(2));
                    break;
                case (int)Frequency.Quarterly:
                    rRecur.RecurMonthly(startDate.Day, 3);
                    recurDates = rRecur.InstancesBetween(startDate, startDate.AddMonths(3));
                    break;
                case (int)Frequency.ThreeXQuarterly:
                    rRecur.RecurMonthly(startDate.Day, 9);
                    recurDates = rRecur.InstancesBetween(startDate, DateTime.Today.AddMonths(20));
                    break;
                case (int)Frequency.SemiAnnually:
                    rRecur.RecurMonthly(startDate.Day, 6);
                    recurDates = rRecur.InstancesBetween(startDate, startDate.AddMonths(6));
                    break;
                case (int)Frequency.Annually:
                    rRecur.RecurYearly(startDate.Month, startDate.Day, 1);
                    recurDates = rRecur.InstancesBetween(startDate, startDate.AddYears(1));
                    break;
                case (int)Frequency.Threeyears:
                    rRecur.RecurYearly(startDate.Month, startDate.Day, 3);
                    recurDates = rRecur.InstancesBetween(startDate, startDate.AddYears(3));
                    break;
                case (int)Frequency.Fiveyears:
                    rRecur.RecurYearly(startDate.Month, startDate.Day, 5);
                    recurDates = rRecur.InstancesBetween(startDate, startDate.AddYears(5));
                    break;
                case (int)Frequency.Sixyears:
                    rRecur.RecurYearly(startDate.Month, startDate.Day, 6);
                    recurDates = rRecur.InstancesBetween(startDate, startDate.AddYears(6));
                    break;
            }
            if (recurDates.Count > 0)
            {
                var nearestDate = DateTime.Today;

                if (recurDates.Count == 1)
                {
                    nearestDate = recurDates.ToList()[0];
                }
                else
                {
                    nearestDate = recurDates.ToList()[sequence];
                }

                if (nearestDate != null)
                    return nearestDate;
                else
                    return DateTime.Today;
            }
            else
                return DateTime.Today;
        }


        #endregion


        #region Round Schedule Dates

        //public static IEnumerable<DateTime> GetDates(int frequencyId, DateTime startDate, int month, int week, int day, string Occurence, DateTime? enddate = null)
        //{
        //    var recurDates = new DateTimeCollection();
        //    var rRecur = new Recurrence
        //    {
        //        StartDateTime = startDate,
        //        //RecurUntil = startDate.A,
        //        CanOccurOnHoliday = false
        //    };

        //    switch (frequencyId)
        //    {
        //        case (int)Frequency.Daily:
        //            rRecur.RecurDaily(1);
        //            recurDates = rRecur.InstancesBetween(startDate, DateTime.Now.AddDays(50));
        //            break;
        //        case (int)Frequency.ThreeDays:
        //            rRecur.RecurDaily(3);
        //            recurDates = rRecur.InstancesBetween(startDate, DateTime.Now.AddDays(15));
        //            break;
        //        case (int)Frequency.Weekly:
        //            rRecur.RecurWeekly(50, GetDayOfWeek(startDate.DayOfYear));
        //            recurDates = rRecur.InstancesBetween(startDate, DateTime.Now.AddYears(1));
        //            break;
        //        case (int)Frequency.Monthly:
        //            rRecur.RecurMonthly(startDate.Day, 1);
        //            recurDates = rRecur.InstancesBetween(startDate, DateTime.Now.AddMonths(3));
        //            break;
        //        case (int)Frequency.TwoMonths:
        //            rRecur.RecurMonthly(startDate.Day, 2);
        //            recurDates = rRecur.InstancesBetween(startDate, DateTime.Now.AddMonths(6));
        //            break;
        //        case (int)Frequency.Quarterly:
        //            rRecur.RecurMonthly(startDate.Day, 3);
        //            recurDates = rRecur.InstancesBetween(startDate, DateTime.Now.AddMonths(6));
        //            break;
        //        case (int)Frequency.SemiAnnually:
        //            rRecur.RecurMonthly(startDate.Day, 6);
        //            recurDates = rRecur.InstancesBetween(startDate, DateTime.Now.AddMonths(12));
        //            break;
        //        case (int)Frequency.Annually:
        //            rRecur.RecurYearly(startDate.Month, DateTime.Now.Day, 1);
        //            recurDates = rRecur.InstancesBetween(startDate, startDate.AddYears(3));
        //            break;
        //    }



        //    return recurDates.ToList();
        //}
        public static IEnumerable<DateTime> GetDates(int frequencyId, DateTime startDate, int month, int week, int day, string Occurence, DateTime? enddate = null,int? recurevery=1,int? The=1,int? recurfor=1)
        {
            var recurDates = new DateTimeCollection();
            var rRecur = new Recurrence
            {
                StartDateTime = startDate,
                //RecurUntil = startDate.A,
                CanOccurOnHoliday = false,
               
            };
            int startday = 0;
            int startmonth = 0;
            DayOccurrence DayOccurrence = (DayOccurrence)The;
             double weekday = Math.Pow(2, week);
            DaysOfWeek DaysOfWeek = (DaysOfWeek)weekday;
            switch (frequencyId)
            {
                case (int)Frequency.Daily:
                    rRecur.RecurDaily(recurevery.Value);
                    recurDates = rRecur.InstancesBetween(startDate, !enddate.HasValue ? DateTime.Now.AddYears(1) : enddate.Value);
                   // recurDates = rRecur.InstancesBetween(startDate, !enddate.HasValue ? DateTime.Now.AddDays(50) : enddate.Value);
                    break;
                case (int)Frequency.ThreeDays:
                    rRecur.RecurDaily(3);
                    recurDates = rRecur.InstancesBetween(startDate, !enddate.HasValue ? DateTime.Now.AddDays(15) : enddate.Value);
                    break;
                case (int)Frequency.Weekly:
                    //rRecur.RecurWeekly(50, GetDayOfWeek(startDate.DayOfYear));
                    //recurDates = rRecur.InstancesBetween(startDate, DateTime.Now.AddYears(1));
                    //break;
                    if (!string.IsNullOrEmpty(Occurence))
                    {
                        int reocuurenceday = !enddate.HasValue ? 50 : Convert.ToInt32((enddate.Value - startDate).TotalDays);
                        string[] Occurencearray = Occurence.Trim(',').Split(',');
                        Array.Sort(Occurencearray);
                        DateTime nextdate = startDate;
                        rRecur.RecurWeekly(recurevery.Value, GetDayOfWeek(startDate.DayOfYear));
                       // recurDates = rRecur.InstancesBetween(startDate, enddate.HasValue?enddate.Value: DateTime.Now.AddYears(1));
                            int initialdaystart = (int)startDate.DayOfWeek;
                        if (reocuurenceday > 0)
                        {
                            var reoccordt = new DateTimeCollection();
                            foreach (string dayval in Occurencearray)
                            {
                                //if (initialdaystart != Convert.ToInt32(dayval))
                                //{


                                    DayOfWeek dateon = (DayOfWeek)nextdate.DayOfWeek;
                                    DayOfWeek dayselection = (DayOfWeek)Convert.ToInt32(dayval);
                                    int start = (int)startDate.DayOfWeek;
                                    int target = (int)dayselection;
                                    if (target < start)
                                        target += 7;
                                    nextdate = startDate.AddDays(target - start);



                                    if (nextdate >= startDate)
                                    {
                                        DateTimeCollection recurDates1 = new DateTimeCollection();
                                        var rRecur1 = new Recurrence
                                        {
                                            StartDateTime = nextdate,
                                            //RecurUntil = startDate.A,
                                            CanOccurOnHoliday = false
                                        };
                            
                                        rRecur1.RecurWeekly(recurevery.Value, GetDayOfWeek((int)dayselection+1));
                                        recurDates1 = rRecur1.InstancesBetween(nextdate, enddate.HasValue ? enddate.Value : DateTime.Now.AddYears(1));
                                        foreach (DateTime date in recurDates1)
                                        {
                                            if (date <= (enddate.HasValue ? enddate.Value : DateTime.Now.AddYears(1)))
                                                recurDates.Add(date);
                                        }
                                    }

                                //}

                            }
                        }
                       
                       
                    }
                    else
                    {
                        rRecur.RecurWeekly(1, GetDayOfWeek(startDate.DayOfYear));
                        recurDates = rRecur.InstancesBetween(startDate, enddate.HasValue ? enddate.Value : DateTime.Now.AddYears(1));
                    }
                    break;
                case (int)Frequency.Monthly:
                    startday = (int)startDate.Day;
                    if (recurfor==3)
                    {
                        startday = day;
                    }
                    if (recurfor == 4)
                    {
                        startmonth = month;
                        startday = day;

                        rRecur.RecurMonthly(DayOccurrence, DaysOfWeek, recurevery.Value);
                    }
                    if (recurfor != 4)
                    {
                        rRecur.RecurMonthly(startday, recurevery.Value);
                    }
                    //recurDates = rRecur.InstancesBetween(startDate, DateTime.Now.AddMonths(3));
                    recurDates = rRecur.InstancesBetween(startDate, !enddate.HasValue ? DateTime.Now.AddYears(1) : enddate.Value);
                    break;
                case (int)Frequency.TwoMonths:
                    rRecur.RecurMonthly(startDate.Day, 2);
                    //recurDates = rRecur.InstancesBetween(startDate, DateTime.Now.AddMonths(6));
                    recurDates = rRecur.InstancesBetween(startDate, !enddate.HasValue ? DateTime.Now.AddMonths(6) : enddate.Value);
                    break;
                case (int)Frequency.Quarterly:
                    rRecur.RecurMonthly(startDate.Day, 3);
                    //recurDates = rRecur.InstancesBetween(startDate, DateTime.Now.AddMonths(6));
                    recurDates = rRecur.InstancesBetween(startDate, !enddate.HasValue ? DateTime.Now.AddMonths(6) : enddate.Value);
                    break;
                case (int)Frequency.SemiAnnually:
                    rRecur.RecurMonthly(startDate.Day, 6);
                    //recurDates = rRecur.InstancesBetween(startDate, DateTime.Now.AddMonths(12));
                    recurDates = rRecur.InstancesBetween(startDate, !enddate.HasValue ? DateTime.Now.AddMonths(12) : enddate.Value);
                    break;
                case (int)Frequency.Annually:

                    startmonth = (int)startDate.Month;
                    startday = (int)startDate.Day;
                    if (recurfor == 4)
                    {
                        startmonth = month;
                        startday = day;
                       
                        rRecur.RecurYearly(DayOccurrence, DaysOfWeek, month, recurevery.Value);
                    }
                    else if (recurfor == 5)
                    {
                        startmonth = month;
                        startday = day;

                        //startDate = new DateTime(DateTime.Now.Year, month, day);
                        //if(startDate<=DateTime.Now)
                        //{
                        //    startDate = new DateTime(DateTime.Now.Year+1, month, day);
                        //}
                    }
                    if (recurfor!=4)
                    {
                        //rRecur.CanOccurOnHoliday = true;
                        rRecur.RecurYearly(startmonth, startday, recurevery.Value);
                    }
                    // recurDates = rRecur.InstancesBetween(startDate, startDate.AddYears(3));
                    recurDates = rRecur.InstancesBetween(startDate, !enddate.HasValue ? startDate.AddYears(4) : enddate.Value);
                    break;
            }



            return recurDates.ToList();
        }
        #endregion
    }

}
