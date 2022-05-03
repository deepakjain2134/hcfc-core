using System;

namespace HCF.PDI
{
    /// <summary>
    /// This enumerated type defines occurrences for days of the week within a month
    /// </summary>
    [Serializable]
    public enum DayOccurrence
    {
        /// <summary>No day occurrence set</summary>
        [LRDescription("DONone")]
        None,
        /// <summary>The first occurrence in the month</summary>
        [LRDescription("DOFirst")]
        First,
        /// <summary>The second occurrence in the month</summary>
        [LRDescription("DOSecond")]
        Second,
        /// <summary>The third occurrence in the month</summary>
        [LRDescription("DOThird")]
        Third,
        /// <summary>The fourth occurrence in the month</summary>
        [LRDescription("DOFourth")]
        Fourth,
        /// <summary>The last occurrence in the month</summary>
        [LRDescription("DOLast")]
        Last
    }
}
