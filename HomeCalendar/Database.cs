using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Threading;
using System.Globalization;

// ===================================================================
// Very important notes:
// ... To keep everything working smoothly, you should always
//     dispose of EVERY SQLiteCommand even if you recycle a 
//     SQLiteCommand variable later on.
//     EXAMPLE:
//            Database.newDatabase(GetSolutionDir() + "\\" + filename);
//            var cmd = new SQLiteCommand(Database.dbConnection);
//            cmd.CommandText = "INSERT INTO categoryTypes(Description) VALUES('Whatever')";
//            cmd.ExecuteNonQuery();
//            cmd.Dispose();
//
// ... also dispose of reader objects
//
// ... by default, SQLite does not impose Foreign Key Restraints
//     so to add these constraints, connect to SQLite something like this:
//            string cs = $"Data Source=abc.sqlite; Foreign Keys=1";
//            var con = new SQLiteConnection(cs);
//
// ===================================================================


namespace Calendar
{
    public class Database
    {

        public static SQLiteConnection dbConnection { get { return _connection; } }
        private static SQLiteConnection _connection;

        // ===================================================================
        // create and open a new database
        // ===================================================================
        /// <summary>
        /// Creates a new database at the given <paramref name="filename"/>. Overwrites it if the file already exisits.
        /// </summary>
        /// <param name="filename">The filename to create the new database file.</param>
        public static void newDatabase(string filename)
        {

            CloseDatabaseAndReleaseFile();
            //attempt to connect to the db
            string connectionString = $@"Data Source={filename}; Foreign Keys=1";
            _connection = new SQLiteConnection(connectionString);
            dbConnection.Open();

            var cmd = new SQLiteCommand(dbConnection);

            cmd.CommandText = "DROP TABLE IF EXISTS events";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "DROP TABLE IF EXISTS categories";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "DROP TABLE IF EXISTS categoryTypes";
            cmd.ExecuteNonQuery();

            cmd.CommandText = @"CREATE TABLE categoryTypes(Id INTEGER PRIMARY KEY, Description TEXT)";
            cmd.ExecuteNonQuery();

            cmd.CommandText = @"CREATE TABLE categories(Id INTEGER PRIMARY KEY AUTOINCREMENT, Description TEXT, TypeId INTEGER NOT NULL,
            FOREIGN KEY (TypeId) REFERENCES categoryTypes(Id))";
            cmd.ExecuteNonQuery();
           
            cmd.CommandText = @"CREATE TABLE events(Id INTEGER PRIMARY KEY AUTOINCREMENT, DurationInMinutes DOUBLE, StartDateTime TEXT, Details TEXT, CategoryId INTEGER NOT NULL,
            FOREIGN KEY (CategoryId) REFERENCES categories(Id))";
            cmd.ExecuteNonQuery();
            
            cmd.Dispose();

        }

       // ===================================================================
       // open an existing database
       // ===================================================================
       /// <summary>
       /// Opens an existing database from the given <paramref name="filename"/>. 
       /// </summary>
       /// <param name="filename">The filename specifying the existing database file.</param>
       /// <exception cref="FileNotFoundException">Thrown when the given <paramref name="filename"/> cannot be found or does not exist.</exception>
       public static void existingDatabase(string filename)
        {
            CloseDatabaseAndReleaseFile();
            try
            {
                if (File.Exists(filename))
                {
                    string connectionString = $@"Data Source={filename}; Foreign Keys=1";
                    _connection = new SQLiteConnection(connectionString);
                    dbConnection.Open();
                }
                else
                {
                    throw new FileNotFoundException($"The specified file {filename} does not exist");
                }
            }
            catch (FileNotFoundException e )
            {
                Console.WriteLine(e.Message);
            }
       }

       // ===================================================================
       // close existing database, wait for garbage collector to
       // release the lock before continuing
       // ===================================================================
       /// <summary>
       /// Closes the connection to the current database.
       /// </summary>
        static public void CloseDatabaseAndReleaseFile()
        {
            if (Database.dbConnection != null)
            {
                // close the database connection
                Database.dbConnection.Close();
                

                // wait for the garbage collector to remove the
                // lock from the database file
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }
    }

}
