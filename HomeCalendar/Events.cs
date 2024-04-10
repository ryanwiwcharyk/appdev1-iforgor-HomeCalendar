using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Data.SQLite;
using System.Data.Common;
using System.Collections.Specialized;

// ============================================================================
// (c) Sandy Bultena 2018
// * Released under the GNU General Public License
// ============================================================================

namespace Calendar
{
    // ====================================================================
    // CLASS: Events
    //        - A collection of Event items,
    //        - Read / write to file
    //        - etc
    // ====================================================================
    /// <summary>
    /// Manages multiple events within a calender item, with methods allowing to read and write data from a database, save, add and deleting these events.
    /// </summary>
    public class Events
    {
        private static String DefaultFileName = "calendar.txt";
        private List<Event> _Events = new List<Event>();
        private string _FileName;
        private string _DirName;
        private static SQLiteConnection _connection;

        /// <summary>
        /// A parameterized constructor, allowing the database connection to establish itself with the necessary db params.
        /// </summary>
        /// <param name="dbConnection"></param>
        /// <param name="newDB"></param>
        public Events(SQLiteConnection dbConnection, bool newDB)
        {
            // Assigning the connection to the categories' connection field.
            _connection = dbConnection;
        }
        public Events()
        {

        }
        // ====================================================================
        // Properties
        // ====================================================================
        /// <summary>
        /// Gets the fileName of a file you'd like to access.
        /// </summary>
        /// <value>
        /// The name of the filename. This is a string value.
        /// </value>
        public String FileName { get { return _FileName; } }
        /// <summary>
        /// Gets the Directory name containing the file you want to access.
        /// </summary>
        /// <value>
        /// The directory name. This is a string value.
        /// </value>
        public String DirName { get { return _DirName; } }

        /// <summary>
        /// Adds an event with the specified properties to the database..
        /// </summary>
        /// <param name="date">Represents the date time of the event.</param>
        /// <param name="category">Represents the category of the event.</param>
        /// <param name="duration">Represents the duration in minutes of the event.</param>
        /// <param name="details">Represents the details of the event.</param>
        public void Add(DateTime date, int category, Double duration, String details)
        {

            var cmd = new SQLiteCommand(_connection);

            cmd.CommandText = $@"INSERT INTO events (DurationInMinutes, StartDateTime, Details, CategoryId)
                                 VALUES( @duration, @date, @details , @category)";
            cmd.Parameters.AddWithValue("@duration", duration);
            cmd.Parameters.AddWithValue("@date", date);
            cmd.Parameters.AddWithValue("@details", details);
            cmd.Parameters.AddWithValue("@category", category);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }

        // ====================================================================
        // Delete Event
        // ====================================================================
        /// <summary>
        /// Deletes an event based on its Id.
        /// </summary>
        /// <param name="Id">Represents the Id of the specific event.</param>
        public void Delete(int Id)
        {

            var cmd = new SQLiteCommand(_connection);


            cmd.CommandText = $@"DELETE FROM events WHERE Id = @id";
            cmd.Parameters.AddWithValue("@id", Id);
            cmd.ExecuteNonQuery();

            cmd.Dispose();
        }

        // ====================================================================
        // Return list of Events
        // Note:  make new copy of list, so user cannot modify what is part of
        //        this instance
        // ====================================================================
        /// <summary>
        /// Returns a list of all the events in the database.
        /// </summary>
        /// <returns>A list containing all events.</returns>
        public List<Event> List()
        {
            List<Event> newList = new List<Event>();
            var cmd = new SQLiteCommand(Database.dbConnection);
            cmd.CommandText = "SELECT * FROM events";
            var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                int eventId = Convert.ToInt32(dr["Id"]);
                int eventCategory = Convert.ToInt32(dr["CategoryId"]);    
                string date = (string)dr["StartDateTime"];
                DateTime datetime = Convert.ToDateTime(date);
                double duration = (double)dr["DurationInMinutes"];
                string details = (string)dr["Details"];
                
                Event newEvent = new Event(eventId, datetime, eventCategory, duration, details);
                newList.Add(newEvent);

            }
            return newList;
        }

        /// <summary>
        /// Updates an event in the database with the given arguments.
        /// </summary>
        /// <param name="id">The id of the event to update.</param>
        /// <param name="startDateTime">The start date to update the event with.</param>
        /// <param name="durationInMinutes">The duration to update the event with.</param>
        /// <param name="details">The details to update the event with.</param>
        /// <param name="catId">The category id of the event.</param>
        public void UpdateProperties(int id, DateTime startDateTime, double durationInMinutes, string details, int catId)
        {
            var cmd = new SQLiteCommand(Database.dbConnection);
            cmd.CommandText = $@"UPDATE events SET StartDateTime = @startTime, DurationInMinutes = @duration, Details = @details, CategoryId = @catId WHERE Id = @id";
            cmd.Parameters.AddWithValue("@startTime", startDateTime.ToString());
            cmd.Parameters.AddWithValue("@duration", durationInMinutes);
            cmd.Parameters.AddWithValue("@details", details);
            cmd.Parameters.AddWithValue("catId", catId);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }

    }
}

