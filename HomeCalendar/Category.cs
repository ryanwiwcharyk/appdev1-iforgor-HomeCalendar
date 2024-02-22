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
    public class Category
    {
        // ====================================================================
        // Properties
        // ====================================================================
        public int Id { get; set; }
        public String Description { get; set; }
        public CategoryType Type { get; set; }
        public enum CategoryType
        {
            Event,
            AllDayEvent,
            Holiday,
            Availability
        };

        // ====================================================================
        // Constructor
        // ====================================================================
        public Category(int id, String description, CategoryType type = CategoryType.Event)
        {
            this.Id = id;
            this.Description = description;
            this.Type = type;
        }

        // ====================================================================
        // Copy Constructor
        // ====================================================================
        public Category(Category category)
        {
            this.Id = category.Id;;
            this.Description = category.Description;
            this.Type = category.Type;
        }
        // ====================================================================
        // String version of object
        // ====================================================================
        public override string ToString()
        {
            return Description;
        }

    }
}

