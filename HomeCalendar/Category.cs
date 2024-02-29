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
    // CLASS: Category
    //        - An individual category for Calendar program
    //        - Valid category types: Event,AllDayEvent,Holiday
    // ====================================================================
    /// <summary>
    /// Represents the category item, aswell as its identifiers, and constructors to initialize the item.
    /// </summary>
    public class Category
    {
        // ====================================================================
        // Properties
        // ====================================================================
        /// <summary>
        /// Gets and sets the Id of a specific category item.
        /// </summary>
        /// <value>
        /// The name of the Id. This is a int value.
        /// </value>
        public int Id { get; }
        /// <summary>
        /// Gets and sets the description of a specific category item.
        /// </summary>
        /// <value>
        /// The name of the description. This is a string value.
        /// </value>
        public String Description { get; }
        /// <summary>
        /// Gets and sets the type of a specific category item.
        /// </summary>
        /// <value>
        /// The name of the categoryType. This is a CategoryType value.
        /// </value>
        public CategoryType Type { get; }
        /// <summary>
        /// Represents the type state that the category is currently in.
        /// </summary>
        /// <value>
        /// The names of the categoryType. This is a enum value.
        /// </value>
        public enum CategoryType
        {
            /// <summary>
            /// Event is a catagoryType representing an ongoing activity.
            /// </summary>
            Event = 1,
            /// <summary>
            /// AllDayEvent is a catagoryType representing an event that lasts all day.
            /// </summary>
            AllDayEvent,
            /// <summary>
            /// Holiday is a catagoryType representing holidays.
            /// </summary>
            Holiday,
            /// <summary>
            /// Availability is a catagoryType representing the users free/available time
            /// </summary>
            Availability
        };

        // ====================================================================
        // Constructor
        // ====================================================================
        /// <summary>
        /// Sets the values of a category item based on a given description, categoryType and Id.
        /// </summary>
        /// <param name="id">Represents the Id of a category.</param>
        /// <param name="description">Represents the description of a category.</param>
        /// <param name="type">Represents the type of a category.</param>
        public Category(int id, String description, CategoryType type = CategoryType.Event)
        {
            this.Id = id;
            this.Description = description;
            this.Type = type;
        }

        // ====================================================================
        // Copy Constructor
        // ====================================================================
        /// <summary>
        /// Initializes another category with the same values as the passed category instance.
        /// </summary>
        /// <param name="category">Represents another category item.</param>
        public Category(Category category)
        {
            this.Id = category.Id;;
            this.Description = category.Description;
            this.Type = category.Type;
        }
        // ====================================================================
        // String version of object
        // ====================================================================
        /// <summary>
        /// Used to convert a category item to a string, for easier readability.
        /// </summary>
        /// <returns>The description of a given category.</returns>
        public override string ToString()
        {
            return Description;
        }

    }
}

