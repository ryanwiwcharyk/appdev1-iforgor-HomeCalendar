using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml;
using System.Data.SQLite;
using Microsoft.SqlServer.Server;
using System.Data.SqlClient;
using System.Xml.Linq;
using System.Data.Common;
using System.Data;
using System.Net.Http.Headers;

// ============================================================================
// (c) Sandy Bultena 2018
// * Released under the GNU General Public License
// ============================================================================

namespace Calendar
{
    // ====================================================================
    // CLASS: categories
    //        - A collection of category items,
    //        - Read / write to file
    //        - etc
    // ====================================================================
    /// <summary>
    /// The Categories class manages and gives access to the available categories in the database.
    /// </summary>
    public class Categories
    {
        private static String DefaultFileName = "calendarCategories.txt";
        private List<Category> _Categories = new List<Category>();
        private string? _FileName;
        private string? _DirName;
        private static SQLiteConnection _connection;

        // ====================================================================
        // Properties
        // ====================================================================
        public String? FileName { get { return _FileName; } }
        public String? DirName { get { return _DirName; } }

        // ====================================================================
        // Constructor
        // ====================================================================
        /// <summary>
        /// Creates a Categories object given a SQL connection and a bool representing wether or not we're creating a new database.  
        /// </summary>
        /// <param name="dbConnection"> The SQL connection to use to access the database. </param>
        /// <param name="newDB"> Bool representing wether or not we are creating a brand new database. </param>
        public Categories(SQLiteConnection dbConnection, bool newDB)
        {
            // Assigning the connection to the categories' connection field.
            _connection = dbConnection; 
            // If this is a new database, we fill the categories table with default categories.
            if (newDB)
            {
                SetCategoriesToDefaults();
            }

        
        }
        /// <summary>
        /// Default constructor. Will be removed in the future since it's not necessary when using a database.
        /// </summary>
        public Categories()
        {

        }


        // ====================================================================
        // get a specific category from the list where the id is the one specified
        // ====================================================================
        /// <summary>
        /// Gets a category from the database given its id.
        /// </summary>
        /// <param name="id"> The category id to look for in the database. </param>
        /// <returns> The category object who's id matches the provided id. </returns>
        /// <exception cref="Exception"> Thrown if there was an error reading the database. </exception>
        ///  <exception cref="ArgumentNullException"> Thrown if there was no category with the provided id. </exception>

        public Category GetCategoryFromId(int id)
        {
            try
            {
                var cmd = new SQLiteCommand(_connection);
                cmd.CommandText = $"SELECT * FROM categories WHERE Id = '{id}'";
                var dataReader = cmd.ExecuteReader();
                if (dataReader.Read())
                {
                    int categoryID = Convert.ToInt32(dataReader["Id"]);
                    string categoryDescription = (string)dataReader["Description"];
                    int typeId = Convert.ToInt32(dataReader["TypeId"]);
                    
                    if (categoryID != 0) // Convert to int returns 0 if the given id read from db is null
                    {
                        Category newCategory = new Category(categoryID, categoryDescription, (Category.CategoryType)typeId);
                        return newCategory;
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

        /// <summary>
        /// Populates the Categories list from a provided XML file. Will be removed in the future since it's not necessary when using a database.
        /// </summary>
        /// <param name="filepath">
        /// The string that represents the file path to read the info from. If no path provided, defaults to AppData. 
        /// </param>
        /// <exception cref="FileNotFoundException">
        /// Thrown if the file doesn't exist.
        /// </exception>
        /// <exception cref="Exception">
        /// Thrown if the file cannot be parsed into XML.
        /// </exception>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// Categories myCategories = new Categories() 
        /// myCategories.List() // Returns a list containing the default categories.
        /// myCategories.ReadFromFile("path") // Replace "path" with your file path to read the categories from.
        /// myCategories.List() // Returns a list containing the categories from the file - default ones got cleared.
        /// ]]>
        /// </code>
        /// </example>
        public void ReadFromFile(String? filepath = null)
        {

            // ---------------------------------------------------------------
            // reading from file resets all the current categories,
            // ---------------------------------------------------------------
            _Categories.Clear();

            // ---------------------------------------------------------------
            // reset default dir/filename to null 
            // ... filepath may not be valid, 
            // ---------------------------------------------------------------
            _DirName = null;
            _FileName = null;

            // ---------------------------------------------------------------
            // get filepath name (throws exception if it doesn't exist)
            // ---------------------------------------------------------------
            filepath = CalendarFiles.VerifyReadFromFileName(filepath, DefaultFileName);

            // ---------------------------------------------------------------
            // If file exists, read it
            // ---------------------------------------------------------------
            _ReadXMLFile(filepath);
            _DirName = Path.GetDirectoryName(filepath);
            _FileName = Path.GetFileName(filepath);
        }

        // ====================================================================
        // save to a file
        // if filepath is not specified, read/save in AppData file
        // ====================================================================
        /// <summary>
        /// Saves the categories and their information to a provided file path. If no path provided, defaults to AppData.  Will be removed in the future since it's not necessary when using a database.
        /// </summary>
        /// <param name="filepath">
        /// Th file path to write the info to as a string. Defaults to AppData if none provided.
        /// </param>
        /// <exception cref="Exception">
        /// Thrown if file doesn't exist.
        /// </exception>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// Categories myCategories = new Categories() 
        /// myCategories.SaveToFile("path") // Replace "path" with the file path to write the categories information to. 
        /// // The file at "path" will now contain information about myCategories. 
        /// ]]>
        /// </code>
        /// </example>
        public void SaveToFile(String? filepath = null)
        {
            // ---------------------------------------------------------------
            // if file path not specified, set to last read file
            // ---------------------------------------------------------------
            if (filepath == null && DirName != null && FileName != null)
            {
                filepath = DirName + "\\" + FileName;
            }

            // ---------------------------------------------------------------
            // just in case filepath doesn't exist, reset path info
            // ---------------------------------------------------------------
            _DirName = null;
            _FileName = null;

            // ---------------------------------------------------------------
            // get filepath name (throws exception if it doesn't exist)
            // ---------------------------------------------------------------
            filepath = CalendarFiles.VerifyWriteToFileName(filepath, DefaultFileName);

            // ---------------------------------------------------------------
            // save as XML
            // ---------------------------------------------------------------
            _WriteXMLFile(filepath);

            // ----------------------------------------------------------------
            // save filename info for later use
            // ----------------------------------------------------------------
            _DirName = Path.GetDirectoryName(filepath);
            _FileName = Path.GetFileName(filepath);
        }

        // ====================================================================
        // set categories to default
        // ====================================================================
        /// <summary>
        /// Clears the Categories list and populates it with default values.
        /// </summary>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// Categories myCategories = new Categories(connection,true); // Creating a categories object from an existing database 
        /// myCategories.List(); // Returns a list containing the categories from the database
        /// myCategories.SetCategoriesToDefaults(); // Resetting the database categories to default
        /// myCategories.List() // Returns a list containing just the default categories
        /// ]]>
        /// </code>
        /// </example>
        public void SetCategoriesToDefaults()
        {
            // ---------------------------------------------------------------
            // reset any current categorie

            // ---------------------------------------------------------------
            // Add Defaults
            // ---------------------------------------------------------------
            var cmd = new SQLiteCommand(_connection);

            cmd.CommandText = "DELETE FROM categories";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "DELETE FROM categoryTypes";
            cmd.ExecuteNonQuery();
            cmd.CommandText = @$"INSERT INTO categoryTypes (Description) VALUES ('Event')";
            cmd.ExecuteNonQuery();
            cmd.CommandText = @$"INSERT INTO categoryTypes(Description) VALUES ('AllDayEvent')";
            cmd.ExecuteNonQuery();
            cmd.CommandText = @$"INSERT INTO categoryTypes (Description) VALUES ('Holiday')";
            cmd.ExecuteNonQuery();
            cmd.CommandText = @$"INSERT INTO categoryTypes (Description) VALUES ('Availability')";
            cmd.ExecuteNonQuery();

            Add("School", Category.CategoryType.Event);
            Add("Personal", Category.CategoryType.Event);
            Add("VideoGames", Category.CategoryType.Event);
            Add("Medical", Category.CategoryType.Event);
            Add("Sleep", Category.CategoryType.Event);
            Add("Vacation", Category.CategoryType.AllDayEvent);
            Add("Travel days", Category.CategoryType.AllDayEvent);
            Add("Canadian Holidays", Category.CategoryType.Holiday);
            Add("US Holidays", Category.CategoryType.Holiday);
        }

        // ====================================================================
        // Add category to database table
        // ====================================================================
        /// <summary>
        /// Inserts a new category into the database given a category object.
        /// </summary>
        /// <param name="category"> The category to add to the database. </param>
        private void Add(Category category)
        {
            //Connect to the database
            Database.CloseDatabaseAndReleaseFile();//close the database if already open
            Database.dbConnection.Open(); //opening database

            //Insert category instance into the categories table
            var cmd = new SQLiteCommand(Database.dbConnection);

            cmd.CommandText = $@"INSERT INTO categories(Description, TypeId)
                                 VALUES(@name, @type)";
            cmd.Parameters.AddWithValue("@desc", category.Description.ToString());
            cmd.Parameters.AddWithValue("@type", (int)category.Type);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }
        /// <summary>
        /// Inserts a new category into the database given a category description and type.
        /// </summary>
        /// <param name="desc"> The description of the category as a string. </param>
        /// <param name="type"> The type of the category as a Category.CategoryType </param>

        public void Add(String desc, Category.CategoryType type)
        {
            //int new_num = 1;
            //if (_Categories.Count > 0)
            //{
            //    new_num = (from c in _Categories select c.Id).Max();
            //    new_num++;
            //}
            //_Categories.Add(new Category(new_num, desc, type));

            //^^Old Logic for adding to a list (IN MEMORY)

            var cmd = new SQLiteCommand(Database.dbConnection);

            //cmd.CommandText = $"SELECT COUNT(*) FROM categoryTypes WHERE Id = '{(int)type}'";
            //int count = Convert.ToInt32(cmd.ExecuteScalar());
            //if (count == 0)
            //{
            //    cmd.CommandText = @$"INSERT INTO categoryTypes (Id, Description) VALUES ({(int)type},'{type}')";
            //    cmd.ExecuteNonQuery();
            //}

            cmd.CommandText = @$"INSERT INTO categories (Description, TypeID) VALUES ('{desc}', '{(int)type}')";
            cmd.ExecuteNonQuery();

            cmd.Dispose();
        }

        // ====================================================================
        // Add category to database table
        // ====================================================================
        
        /// <summary>
        /// Updates a category's properties given the category's id, a new description and a new category type.
        /// </summary>
        /// <param name="id"> The id of the category to update as an integer. </param>
        /// <param name="newDesc"> The new description of the category as a string. </param>
        /// <param name="categoryType"> The new type of the category as a Category.CategoryType </param>

        public void UpdateProperties(int id, string newDesc, Category.CategoryType categoryType)
        {
            var cmd = new SQLiteCommand(Database.dbConnection);

            cmd.CommandText = $@"UPDATE categories SET Description = '{newDesc}', TypeID = '{(int)categoryType}' WHERE Id = '{id}'";
            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }

        // ====================================================================
        // Delete category
        // ====================================================================
        /// <summary>
        /// Deletes a category from the database given a category's id.
        /// </summary>
        /// <param name="Id"> The ID of the category to delete. </param>
        public void Delete(int Id)
        {


            var cmd = new SQLiteCommand(Database.dbConnection);

            cmd.CommandText = $@"DELETE FROM events WHERE CategoryId = '{Id}'";
            cmd.ExecuteNonQuery();

            cmd.CommandText = $@"DELETE FROM categories WHERE Id = '{Id}'";
            cmd.ExecuteNonQuery();

            cmd.Dispose();


        }

        // ====================================================================
        // Return list of categories
        // Note:  make new copy of list, so user cannot modify what is part of
        //        this instance
        // ====================================================================
        /// <summary>
        /// Creates a list of category objects that contains the categories in the categories table.
        /// </summary>
        /// <returns> The list of categories as a List<Category> </Category></returns>
        public List<Category> List()
        {
            List<Category> newList = new List<Category>();
            var cmd = new SQLiteCommand(_connection);
            cmd.CommandText = "SELECT * FROM categories";
            var dr = cmd.ExecuteReader();
            while (dr.Read()) 
            {
                int categoryID = Convert.ToInt32(dr["Id"]);
                string categoryDescription = (string)dr["Description"];
                int typeId  = Convert.ToInt32(dr["TypeId"]);
                Category newCategory = new Category(categoryID, categoryDescription, (Category.CategoryType)typeId);
                newList.Add(newCategory);

            }
            return newList;
        }
      
        // ====================================================================
        // read from an XML file and add categories to our categories list
        // ====================================================================
        private void _ReadXMLFile(String filepath)
        {

            // ---------------------------------------------------------------
            // read the categories from the xml file, and add to this instance
            // ---------------------------------------------------------------
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(filepath);

                foreach (XmlNode category in doc.DocumentElement.ChildNodes)
                {
                    String id = (((XmlElement)category).GetAttributeNode("ID")).InnerText;
                    String typestring = (((XmlElement)category).GetAttributeNode("type")).InnerText;
                    String desc = ((XmlElement)category).InnerText;

                    Category.CategoryType type;
                    switch (typestring.ToLower())
                    {
                        case "event":
                            type = Category.CategoryType.Event;
                            break;
                        case "alldayevent":
                            type = Category.CategoryType.AllDayEvent;
                            break;
                        case "availability":
                            type = Category.CategoryType.Availability;
                            break;
                        case "holiday":
                            type = Category.CategoryType.Holiday;
                            break;
                        default:
                            type = Category.CategoryType.Event;
                            break;
                    }
                    this.Add(new Category(int.Parse(id), desc, type));
                }

            }
            catch (Exception e)
            {
                throw new Exception("ReadXMLFile: Reading XML " + e.Message);
            }

        }


        // ====================================================================
        // write all categories in our list to XML file
        // ====================================================================
        private void _WriteXMLFile(String filepath)
        {
            try
            {
                // create top level element of categories
                XmlDocument doc = new XmlDocument();
                doc.LoadXml("<Categories></Categories>");

                // foreach Category, create an new xml element
                foreach (Category cat in _Categories)
                {
                    XmlElement ele = doc.CreateElement("Category");
                    XmlAttribute attr = doc.CreateAttribute("ID");
                    attr.Value = cat.Id.ToString();
                    ele.SetAttributeNode(attr);
                    XmlAttribute type = doc.CreateAttribute("type");
                    type.Value = cat.Type.ToString();
                    ele.SetAttributeNode(type);

                    XmlText text = doc.CreateTextNode(cat.Description);
                    doc.DocumentElement.AppendChild(ele);
                    doc.DocumentElement.LastChild.AppendChild(text);

                }

                // write the xml to FilePath
                doc.Save(filepath);

            }
            catch (Exception e)
            {
                throw new Exception("_WriteXMLFile: Reading XML " + e.Message);
            }

        }

    }
}

