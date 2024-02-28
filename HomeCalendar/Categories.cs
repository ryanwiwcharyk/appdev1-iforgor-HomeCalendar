﻿using System;
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
        public Categories(SQLiteConnection dbConnection, bool newDB)
        {
            _connection = dbConnection; 
            if (newDB)
            {
                SetCategoriesToDefaults();
            }

        
        }
        public Categories()
        {

        }
     

        // ====================================================================
        // get a specific category from the list where the id is the one specified
        // ====================================================================
        public Category GetCategoryFromId(int i)
        {
            try
            {
                var cmd = new SQLiteCommand(_connection);
                cmd.CommandText = $"SELECT * FROM categories WHERE Id = '{i}'";
                var dataReader = cmd.ExecuteReader();
                if (dataReader.Read())
                {
                    int categoryID = Convert.ToInt32(dataReader["Id"]);
                    string categoryDescription = (string)dataReader["Description"];
                    int typeId = Convert.ToInt32(dataReader["TypeId"]);
                    
                    if (categoryID != 0)//Convert to int returns 0 if the given id read from db is null
                    {
                        Category newCategory = new Category(categoryID, categoryDescription, (Category.CategoryType)typeId);
                        return newCategory;
                    }
                    else
                    {
                        throw new ArgumentNullException($"{i}", "The given id was not found in the database.");
                    }

                }
                
            }
            catch (Exception)
            {

                throw;
            }
            Category? c = _Categories.Find(x => x.Id == i);
            if (c == null)
            {
                throw new Exception("Cannot find category with id " + i.ToString());
            }
            return c;
        }

        // ====================================================================
        // populate categories from a file
        // if filepath is not specified, read/save in AppData file
        // Throws System.IO.FileNotFoundException if file does not exist
        // Throws System.Exception if cannot read the file correctly (parsing XML)
        // ====================================================================
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


            cmd.CommandText = @$"INSERT INTO categories (Description, TypeID) VALUES ('{desc}', {(int)type})";
            cmd.ExecuteNonQuery();

            cmd.Dispose();
        }

        // ====================================================================
        // Delete category
        // ====================================================================
        public void Delete(int Id)
        {
            //int i = _Categories.FindIndex(x => x.Id == Id);
            //if (i == -1)
            //    return;
            //_Categories.RemoveAt(i);

            //Connect to the database
            Database.CloseDatabaseAndReleaseFile();//close the database if already open
            Database.dbConnection.Open(); //opening database

            var cmd = new SQLiteCommand(Database.dbConnection);

            //cmd.CommandText = $@"DELETE FROM events WHERE (Id = {Id})";
            //cmd.ExecuteNonQuery();

            cmd.CommandText = $@"DELETE FROM categories WHERE (Id = {Id})";
            cmd.ExecuteNonQuery();
            cmd.Dispose();


        }

        // ====================================================================
        // Return list of categories
        // Note:  make new copy of list, so user cannot modify what is part of
        //        this instance
        // ====================================================================
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

