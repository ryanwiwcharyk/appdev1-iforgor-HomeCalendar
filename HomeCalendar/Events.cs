using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Data.SQLite;
using System.Data.Common;

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
    /// Manages multiple events within a calender item, with methods allowing to read and write data, save, add and deleting these events.
    /// </summary>
    public class Events
    {
        private static String DefaultFileName = "calendar.txt";
        private List<Event> _Events = new List<Event>();
        private string _FileName;
        private string _DirName;
        private static SQLiteConnection _connection;

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

        // ====================================================================
        // populate categories from a file
        // if filepath is not specified, read/save in AppData file
        // Throws System.IO.FileNotFoundException if file does not exist
        // Throws System.Exception if cannot read the file correctly (parsing XML)
        // ====================================================================
        /// <summary>
        /// Reads the events data from a given filepath, allowing us to access th events within, and validates the filepath name exists and is valid.
        /// </summary>
        /// <param name="filepath">Represents the filepath of the file being opened for reading.</param>



        // ====================================================================
        // Add Event
        // ====================================================================
        /// <summary>
        /// Adds a specific event to the list of current events.
        /// </summary>
        /// <param name="exp">Represents the event object being added to the list.</param>
        private void Add(Event exp)
        {
            _Events.Add(exp);
        }

        /// <summary>
        /// Adds a event to the list of current events containing all details of the new event.
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
        /// Deletes an event based on its Id from the list of current events.
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
        /// Returns a list of all the events added.
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

        public Event GetEventFromId(int id)
        {
            try
            {
                var cmd = new SQLiteCommand(_connection);
                cmd.CommandText = $"SELECT * FROM events WHERE Id = '{id}'";
                var dataReader = cmd.ExecuteReader();
                if (dataReader.Read())
                {
                    int eventId = Convert.ToInt32(dataReader["Id"]);
                    double eventDuration = Convert.ToDouble(dataReader["DurationInMinutes"]);
                    DateTime startDateTime = Convert.ToDateTime(dataReader["StartDateTime"]);
                    string eventDetails = (string)dataReader["Details"];
                    int categoryId = Convert.ToInt32(dataReader["CategoryId"]);

                    if (eventId != 0) // Convert to int returns 0 if the given id read from db is null
                    {
                        Event newEvent = new Event(eventId, startDateTime, categoryId,eventDuration,eventDetails);
                        return newEvent;
                    }
                    else
                    {
                        throw new ArgumentNullException($"{id}", "The given id was not found in the database.");
                    }

                }
                else { throw new Exception(); }

            }
            catch (ArgumentNullException e)
            {

                Console.WriteLine(e.Message);
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }

        }
        // ====================================================================
        // read from an XML file and add categories to our categories list
        // ====================================================================
        private void _ReadXMLFile(String filepath)
        {


            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(filepath);

                // Loop over each Event
                foreach (XmlNode Event in doc.DocumentElement.ChildNodes)
                {
                    // set default Event parameters
                    int id = int.Parse((((XmlElement)Event).GetAttributeNode("ID")).InnerText);
                    String description = "";
                    DateTime date = DateTime.Parse("2000-01-01");
                    int category = 0;
                    Double DurationInMinutes = 0.0;

                    // get Event parameters
                    foreach (XmlNode info in Event.ChildNodes)
                    {
                        switch (info.Name)
                        {
                            case "StartDateTime":
                                date = DateTime.Parse(info.InnerText);
                                break;
                            case "DurationInMinutes":
                                DurationInMinutes = Double.Parse(info.InnerText);
                                break;
                            case "Details":
                                description = info.InnerText;
                                break;
                            case "Category":
                                category = int.Parse(info.InnerText);
                                break;
                        }
                    }

                    // have all info for Event, so create new one
                    this.Add(new Event(id, date, category, DurationInMinutes, description));

                }

            }
            catch (Exception e)
            {
                throw new Exception("ReadFromFileException: Reading XML " + e.Message);
            }
        }


        // ====================================================================
        // write to an XML file
        // if filepath is not specified, read/save in AppData file
        // ====================================================================
        private void _WriteXMLFile(String filepath)
        {
            // ---------------------------------------------------------------
            // loop over all categories and write them out as XML
            // ---------------------------------------------------------------
            try
            {
                // create top level element of Events
                XmlDocument doc = new XmlDocument();
                doc.LoadXml("<Events></Events>");

                // foreach Category, create an new xml element
                foreach (Event exp in _Events)
                {
                    // main element 'Event' with attribute ID
                    XmlElement ele = doc.CreateElement("Event");
                    XmlAttribute attr = doc.CreateAttribute("ID");
                    attr.Value = exp.Id.ToString();
                    ele.SetAttributeNode(attr);
                    doc.DocumentElement.AppendChild(ele);

                    // child attributes (date, description, DurationInMinutes, category)
                    XmlElement d = doc.CreateElement("StartDateTime");
                    XmlText dText = doc.CreateTextNode(exp.StartDateTime.ToString("M\\/d\\/yyyy h:mm:ss tt"));
                    ele.AppendChild(d);
                    d.AppendChild(dText);

                    XmlElement de = doc.CreateElement("Details");
                    XmlText deText = doc.CreateTextNode(exp.Details);
                    ele.AppendChild(de);
                    de.AppendChild(deText);

                    XmlElement a = doc.CreateElement("DurationInMinutes");
                    XmlText aText = doc.CreateTextNode(exp.DurationInMinutes.ToString());
                    ele.AppendChild(a);
                    a.AppendChild(aText);

                    XmlElement c = doc.CreateElement("Category");
                    XmlText cText = doc.CreateTextNode(exp.Category.ToString());
                    ele.AppendChild(c);
                    c.AppendChild(cText);

                }

                // write the xml to FilePath
                doc.Save(filepath);

            }
            catch (Exception e)
            {
                throw new Exception("SaveToFileException: Reading XML " + e.Message);
            }
        }

    }
}

