using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// ============================================================================
// (c) Sandy Bultena 2018
// * Released under the GNU General Public License
// ============================================================================

namespace Calendar
{
    // ====================================================================
    // CLASS: CalendarItem
    //        A single calendar item, includes a Category and an Event
    // ====================================================================

    /// <summary>
    /// Represents a singular event with a category, start time and duration.
    /// </summary>
    public class CalendarItem
    {
        /// <summary>
        /// Gets and sets the category id.
        /// </summary>
        /// <value>
        /// Represents a category's id.
        /// </value>
        public int CategoryID { get; set; }
        /// <summary>
        /// Gets and sets the event id.
        /// </summary>
        /// <value>
        /// Represents an events id.
        /// </value>
        public int EventID { get; set; }
        /// <summary>
        ///Gets and sets the start time and date.
        /// </summary>
        /// <value>
        /// Represents the start time and date of a calendar item.
        /// </value>
        public DateTime StartDateTime { get; set; }
        /// <summary>
        /// Gets and sets the category name.
        /// </summary>
        /// <value>
        /// Represents the name of the calendar item category.
        /// </value>
        public String? Category { get; set; }
        /// <summary>
        /// Gets and sets the description.
        /// </summary>
        /// <value>
        /// Represents the description of a calendar item.
        /// </value>
        public String? ShortDescription { get; set; }
        /// <summary>
        ///Gets and sets the duration of a calendar item.
        /// </summary>
        /// <value>
        /// Represents the duration of a calendar item in minutes.
        /// </value>
        public Double DurationInMinutes { get; set; }
        /// <summary>
        /// Gets and sets the busy time.
        /// </summary>
        /// <value>Represents the busy time of a calendar item.</value>
        public Double BusyTime { get; set; }

    }

    /// <summary>
    /// Represents a collection of calendar items by month including total busy time.
    /// </summary>
    public class CalendarItemsByMonth
    {
        /// <summary>
        /// Gets or sets the month.
        /// </summary>
        /// <value>
        /// A string representing the month to group calendar items by.
        /// </value>
        public String? Month { get; set; }
        /// <summary>
        /// Gets or sets a collection of calendar items.
        /// </summary>
        /// <value>
        /// A collection of calendar items grouped by month.
        /// </value>
        public List<CalendarItem>? Items { get; set; }
        /// <summary>
        /// Gets or sets the total busy time.
        /// </summary>
        /// <value>
        /// Represents the total busy time of all calendar items grouped by month.
        /// </value>
        public Double TotalBusyTime { get; set; }
    }

    /// <summary>
    /// Represents a collection of calendar items including total busy time.
    /// </summary>
    public class CalendarItemsByCategory
    {
        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>
        /// Represents the category to filter calendar items by.
        /// </value>
        public String? Category { get; set; }
        /// <summary>
        /// Gets or sets a collection of calendar items.
        /// </summary>
        /// <value>
        /// Represents the collection of calendar items grouped by category.
        /// </value>
        public List<CalendarItem>? Items { get; set; }
        /// <summary>
        /// Gets or sets the total busy time..
        /// </summary>
        /// <value>
        /// Represesnts the total busy time of all calendar items grouped by category.
        /// </value>
        public Double TotalBusyTime { get; set; }

    }


}
