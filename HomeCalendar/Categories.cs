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
        /// </value>
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
            Category.CategoryType[] catTypes = (Category.CategoryType[])Enum.GetValues(typeof(Category.CategoryType));
            foreach (Category.CategoryType catType in catTypes)
            {
                cmd.CommandText = @$"INSERT INTO categoryTypes (Description) VALUES ('{catType}')";
                cmd.ExecuteNonQuery();
            }
            cmd.Dispose();

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

        /// <summary>
        /// Deletes all existing entries in the categories table, then re-populates the table with various categories.
        /// </summary>
        public void ResetCategories()
        {
            var cmd = new SQLiteCommand(_connection);

            cmd.CommandText = "DELETE FROM categories";
            cmd.ExecuteNonQuery();
            cmd.Dispose();

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

        // ====================================================================
        // Add category to database table
        // ====================================================================
        /// <summary>
        /// Inserts a new category into the database given a category description and type.
        /// </summary>
        /// <param name="desc"> The description of the category as a string. </param>
        /// <param name="type"> The type of the category as a Category.CategoryType </param>

        public void Add(String desc, Category.CategoryType type)
        {

            var cmd = new SQLiteCommand(Database.dbConnection);

            cmd.CommandText = @$"INSERT INTO categories (Description, TypeID) VALUES (@desc, @catType)";
            cmd.Parameters.AddWithValue("@desc", desc);
            cmd.Parameters.AddWithValue("@catType", type);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }


        /// <summary>
        /// Updates a category's properties given the category's id, a new description and a new category type.
        /// </summary>
        /// <param name="id"> The id of the category to update as an integer. </param>
        /// <param name="newDesc"> The new description of the category as a string. </param>
        /// <param name="categoryType"> The new type of the category as a Category.CategoryType </param>
        public void UpdateProperties(int id, string newDesc, Category.CategoryType categoryType)
        {
            var cmd = new SQLiteCommand(Database.dbConnection);
            cmd.CommandText = $@"UPDATE categories SET Description = @newDesc, TypeID = @catType WHERE Id = @id";
            cmd.Parameters.AddWithValue("@newDesc", newDesc);
            cmd.Parameters.AddWithValue("@catType", (int)categoryType);
            cmd.Parameters.AddWithValue("@id", id);
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

            cmd.CommandText = $@"DELETE FROM events WHERE CategoryId = @id";
            cmd.Parameters.AddWithValue("@id",Id);
            cmd.ExecuteNonQuery();

            cmd.CommandText = $@"DELETE FROM categories WHERE Id = @id";
            cmd.Parameters.AddWithValue("@id",Id);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }

        // ====================================================================
        // Return list of categories
        // Note:  make new copy of list, so user cannot modify what is part of
        //        this instance
        // ====================================================================
        /// <summary>
        /// Creates a list of category objects that contains the categories in the categories table from the database.
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

    }
}

