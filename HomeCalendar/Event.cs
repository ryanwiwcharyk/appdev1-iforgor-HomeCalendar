using System;
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
    // CLASS: Event
    //        - An individual event for calendar program
    // ====================================================================
    /// <summary>
    /// Represents the event instance, allowing the calendar items to have a event type. The class contains properties that identify the start time, duration, details as well as the category corresponding.
    /// </summary>
    public class Event
    {
        // ====================================================================
        // Properties
        // ====================================================================
        /// <summary>
        /// Gets the Id of a given event.
        /// </summary>
        /// <value>
        /// The number representing the Id. This is a int value.
        /// </value>
        public int Id { get; }
        /// <summary>
        /// Gets the start time of a given event.
        /// </summary>
        /// <value>
        /// The start time. This is a DateTime value.
        /// </value>
        public DateTime StartDateTime { get; }
        /// <summary>
        /// Gets the duration of the specific event.
        /// </summary>
        /// <value>
        /// The duration in minutes. This is a Double value.
        /// </value>
        public Double DurationInMinutes { get; }
        /// <summary>
        /// Gets the details of an event.
        /// </summary>
        /// <value>
        /// The event details. This is a string value.
        /// </value>
        public String Details { get; }
        /// <summary>
        /// Gets the category of a specific event.
        /// </summary>
        /// <value>
        /// The name of the category id. This is a int value.
        /// </value>
        public int Category { get; }

        // ====================================================================
        // Constructor
        //    NB: there is no verification the event category exists in the
        //        categories object
        // ====================================================================
        /// <summary>
        /// Initializes an event instance with a given Id, Date, category, duration and event details.
        /// </summary>
        /// <param name="id">Represents the Id of an event as an int.</param>
        /// <param name="date">Represents the date of an event as a DateTime.</param>
        /// <param name="category">Represents the category of an event as an int.</param>
        /// <param name="duration">Represents the duration of an event as a double.</param>
        /// <param name="details">Represents the details of an event as a string.</param>
        public Event(int id, DateTime date, int category, Double duration, String details)
        {
            this.Id = id;
            this.StartDateTime = date;
            this.Category = category;
            this.DurationInMinutes = duration;
            this.Details = details;
        }

        // ====================================================================
        // Copy constructor - does a deep copy
        // ====================================================================
        /// <summary>
        /// Initializes the event instance with an event object, copying all of its data to the new instance of event.
        /// </summary>
        /// <param name="obj">Represents the event obejct which is being copied.</param>
        public Event (Event obj)
        {
            this.Id = obj.Id;
            this.StartDateTime = obj.StartDateTime;
            this.Category = obj.Category;
            this.DurationInMinutes = obj.DurationInMinutes;
            this.Details = obj.Details;
           
        }
    }
}
