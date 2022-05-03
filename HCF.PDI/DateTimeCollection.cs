using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace HCF.PDI
{
    /// <summary>
	/// A type-safe collection of <see cref="DateTime"/> objects.  The other classes in this namespace use this
    /// collection when returning a list of dates.  The collection can be used as a data source for data binding.
	/// </summary>
    [Serializable]
	public class DateTimeCollection : Collection<DateTime>
	{
        #region Constructor
        //=====================================================================

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <overloads>There are two overloads for the constructor.</overloads>
        public DateTimeCollection()
        {
        }

        /// <summary>
        /// Construct the collection from an enumerable list of <see cref="DateTime"/> objects
        /// </summary>
        /// <param name="dates">The enumerable list of dates to add</param>
        public DateTimeCollection(IEnumerable<DateTime> dates)
        {
            if(dates != null)
                this.AddRange(dates);
        }
        #endregion

        #region Helper methods
        //=====================================================================

        /// <summary>
        /// Add a range of <see cref="DateTime"/> objects from an enumerable list
        /// </summary>
        /// <param name="dates">The enumerable list of dates to add</param>
        public void AddRange(IEnumerable<DateTime> dates)
        {
            if(dates != null)
                foreach(DateTime d in dates)
                    base.Add(d);
        }

        /// <summary>
        /// Remove a range of items from the collection
        /// </summary>
        /// <param name="index">The zero-based index at which to start removing items</param>
        /// <param name="count">The number of items to remove</param>
        public void RemoveRange(int index, int count)
        {
            ((List<DateTime>)base.Items).RemoveRange(index, count);
        }

        /// <summary>
        /// This is used to sort the collection in ascending or descending order
        /// </summary>
        /// <param name="ascending">Pass true for ascending order, false for descending order</param>
        public void Sort(bool ascending)
        {
            ((List<DateTime>)base.Items).Sort((x, y) =>
            {
                if(ascending)
                    return x.CompareTo(y);

                return y.CompareTo(x);
            });
        }
        #endregion
    }
}
