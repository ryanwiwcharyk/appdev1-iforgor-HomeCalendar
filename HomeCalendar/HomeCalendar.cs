﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using static Calendar.Category;

// ============================================================================
// (c) Sandy Bultena 2018
// * Released under the GNU General Public License
// ============================================================================


namespace Calendar
{
    // ====================================================================
    // CLASS: HomeCalendar
    //        - Combines a Categories Class and an Events Class
    //        - One File defines Category and Events File
    //        - etc
    // ====================================================================
    /// <summary>
    /// Represents the fully built calender containing events, and categories with their given descriptions. 
    /// Contains methods to read and write the data to file. It also sorts and filters items based on categories as well as other data.
    /// </summary>
    public class HomeCalendar
    {
        private Categories _categories;
        private Events _events;
        private static SQLiteConnection _connection;

        // Properties (categories and events object)
        /// <summary>
        /// Gets the categories object containing the list of categories within the calendar item.
        /// </summary>
        /// <value>
        /// The list of catagories. This is a Categories value.
        /// </value>
        public Categories categories { get { return _categories; } }
        /// <summary>
        /// Gets the events object containing the list of events within the calendar item.
        /// </summary>
        /// <value>
        /// The list of events. This is a Events value.
        /// </value>
        public Events events { get { return _events; } }

        // -------------------------------------------------------------------
        // Constructor (new... default categories, no events)
        // -------------------------------------------------------------------
        /// <summary>
        /// Initializes an instance of a HomeCalendar with the given database file. Creates a new file if <paramref name="newDB"/> is true.
        /// </summary>
        /// <param name="databaseFile">The database file to initialize a home calendar with.</param>
        /// <param name="newDB">Creates a new database if true.</param>
        public HomeCalendar(String databaseFile, bool newDB = false)
        {
            // if database exists, and user doesn't want a new database, open existing DB
            if (!newDB && File.Exists(databaseFile))
            {
                Database.existingDatabase(databaseFile);
                _connection = Database.dbConnection;
                  
            }

            // file did not exist, or user wants a new database, so open NEW DB
            else
            {
                Database.newDatabase(databaseFile);
                newDB = true;
                _connection = Database.dbConnection;
            }

            // create the categories and events objects
            _categories = new Categories(_connection, newDB);
            _events = new Events(_connection, newDB);

        }
      

        // ============================================================================
        // Get all events list
        // ============================================================================
        /// <summary>
        /// Searches for a calendar list in respect to given parameters such as start time, end time, category filters and specific category Ids.
        /// </summary>
        /// <param name="Start">The start time for all calendar items.</param>
        /// <param name="End">The end time for all calendar items.</param>
        /// <param name="FilterFlag">A boolean value representing if you'd like to sort by category ids.</param>
        /// <param name="CategoryID">A category Id allowing for sorting specific calendar instances.</param>
        /// <returns>A list of calendar items that follow the parameter guidelines.</returns>
        /// <example>
        /// Assume all the below examples have this input
        /// <code>
        /// Cat_ID  Event_ID  StartDateTime           Details                 DurationInMinutes
        ///    3       1      1/10/2018 10:00:00 AM   App Dev Homework             40
        ///    9       2      1/9/2020 12:00:00 AM    Honolulu		        1440
        ///    9       3      1/10/2020 12:00:00 AM   Honolulu                   1440
        ///    7       4      1/20/2020 11:00:00 AM   On call security            180
        ///    2       5      1/11/2018 7:30:00 PM    staff meeting                15
        ///    8       6      1/1/2020 12:00:00 AM    New Year's                 1440
        ///   11       7      1/12/2020 12:00:00 AM   Wendy's birthday           1440
        ///    2       8      1/11/2018 10:15:00 AM   Sprint retrospective         60
        /// </code>
        /// <code>
        /// <![CDATA[
        ///   List<CalendarItem> defaultItems = calendar.GetCalendarItems(null, null, false, 0); //one
        ///   double totalBusyTime = 0;
        ///
        ///    Console.WriteLine($"{"Cat_ID",-8} | {"Event_ID",-8} | {"StartDateTime",-25} | {"Details",-20} | {"DurationInMinutes",-8}");
        ///    for (int i = 0; i<defaultItems.Count; i++)
        ///    {
        ///        Console.WriteLine($"{defaultItems[i].CategoryID,-11} {defaultItems[i].EventID,-9} {defaultItems[i].StartDateTime,-26} {defaultItems[i].ShortDescription,-29} {defaultItems[i].DurationInMinutes,-8}"); //loop over it
        ///        totalBusyTime += defaultItems[i].BusyTime;
        ///    }
        ///    
        ///    Console.WriteLine("");
        ///    Console.WriteLine($"Total busy time: {totalBusyTime}");
        ///    ]]>
        ///    <b>Sample Output</b>
        ///    
        ///       Cat_ID   | Event_ID | StartDateTime             | Details              | DurationInMinutes
        ///         3           1         2018-01-10 10:00:00 AM     App Dev Homework              40
        ///         2           8         2018-01-11 10:15:00 AM     Sprint retrospective          60
        ///         2           5         2018-01-11 7:30:00 PM      staff meeting                 15
        ///         8           6         2020-01-01 12:00:00 AM     New Year's                    1440
        ///         9           2         2020-01-09 12:00:00 AM     Honolulu                      1440
        ///         9           2         2020-01-09 12:00:00 AM     Honolulu                      1440
        ///         9           2         2020-01-09 12:00:00 AM     Honolulu                      1440
        ///         9           3         2020-01-10 12:00:00 AM     Honolulu                      1440
        ///         11          7         2020-01-12 12:00:00 AM     Wendy's birthday              1440
        ///         7           4         2020-01-20 11:00:00 AM     On call security              180
        /// 
        /// Total busy time: 21170
        ///
        /// </code>
        /// </example>
        public List<CalendarItem> GetCalendarItems(DateTime? Start, DateTime? End, bool FilterFlag, int CategoryID)
        {
         
            Start = Start ?? new DateTime(1900, 1, 1);
            End = End ?? new DateTime(2500, 1, 1);

            // Initializing an empty list of calendar items
            List<CalendarItem> items = new List<CalendarItem>();
            // Initializing a busy time of 0
            Double totalBusyTime = 0;

            var cmd = new SQLiteCommand(_connection);

            // If the filter flag is true, we only select events within the time frame and with the specified category id
            if (FilterFlag)
            {
                cmd.CommandText = $"SELECT e.Id, e.DurationInMinutes, e.StartDateTime, e.Details, e.CategoryId, c.Description, c.TypeId FROM events e " +
                    $"INNER JOIN categories c ON e.CategoryId = c.Id WHERE e.StartDateTime >= @start AND e.StartDateTime <= @end AND e.CategoryId = @catId ORDER BY StartDateTime";
                cmd.Parameters.AddWithValue("@start", Start);
                cmd.Parameters.AddWithValue("@end", End);
                cmd.Parameters.AddWithValue("@catId", CategoryID);
            }
            // If the filter flag is false, we only select events within the time frame and with any category
            else
            {
                cmd.CommandText = $"SELECT e.Id, e.DurationInMinutes, e.StartDateTime, e.Details, e.CategoryId, c.Description, c.TypeId FROM events e " +
                    $"INNER JOIN categories c ON e.CategoryId = c.Id WHERE e.StartDateTime >= @start AND e.StartDateTime <= @end ORDER BY StartDateTime";
                cmd.Parameters.AddWithValue("@start", Start);
                cmd.Parameters.AddWithValue("@end", End);
            }
            // Reading the result(s) of the query
            var dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                // Retrieving all the fields needed to make the calendar item 
                int eventId = Convert.ToInt32(dataReader["Id"]);
                double durationInMinutes = Convert.ToDouble(dataReader["DurationInMinutes"]);
                string startDateTime = (string)dataReader["StartDateTime"];
                string details = (string)dataReader["Details"];
                int categoryId = Convert.ToInt32(dataReader["CategoryId"]);
                string categoryDescription = (string)dataReader["Description"];
                int categoryTypeId = Convert.ToInt32(dataReader["TypeId"]);

                // If the event's category is not of type availability, we add its duration to the total busy time
                if (categoryTypeId != (int)Category.CategoryType.Availability)
                {
                    totalBusyTime += durationInMinutes;
                }
                // Creating the calendar item and adding it to the list
                items.Add(new CalendarItem
                {
                    CategoryID = categoryId,
                    EventID = eventId,
                    ShortDescription = details,
                    StartDateTime = Convert.ToDateTime(startDateTime),
                    DurationInMinutes = durationInMinutes,
                    Category = categoryDescription,
                    BusyTime = totalBusyTime
                });
            }
            // Returning the list of calendar items
            return items;
        }

        // ============================================================================
        // Group all events month by month (sorted by year/month)
        // returns a list of CalendarItemsByMonth which is 
        // "year/month", list of calendar items, and totalBusyTime for that month
        // ============================================================================
        /// <summary>
        /// Searches for calander items by month, given specific parameters to follow such as a start/end time, a filter flag if choosing to sort by category Ids, aswell as the category Ids you'd like to sort by.
        /// </summary>
        /// <param name="Start">The start time of calendar items.</param>
        /// <param name="End">The end time of calendar items.</param>
        /// <param name="FilterFlag">A boolean value representing if you'd like to sort by category ids.</param>
        /// <param name="CategoryID">A category Id allowing for sorting specific calendar instances.</param>
        /// <returns>A list of calendar items by month that follow the parameter guidelines.</returns>
        /// Basic use cases for getting calendar items
        /// <example>
        /// Assume all the below examples have this input
        /// <code>
        /// Cat_ID  Event_ID  StartDateTime           Details                 DurationInMinutes
        ///    3       1      1/10/2018 10:00:00 AM   App Dev Homework             40
        ///    9       2      1/9/2020 12:00:00 AM    Honolulu		        1440
        ///    9       3      1/10/2020 12:00:00 AM   Honolulu                   1440
        ///    7       4      1/20/2020 11:00:00 AM   On call security            180
        ///    2       5      1/11/2018 7:30:00 PM    staff meeting                15
        ///    8       6      1/1/2020 12:00:00 AM    New Year's                 1440
        ///   11       7      1/12/2020 12:00:00 AM   Wendy's birthday           1440
        ///    2       8      1/11/2018 10:15:00 AM   Sprint retrospective         60
        /// </code>
        /// <code>
        /// <![CDATA[
        ///    List<CalendarItemsByMonth> defaultItems = calendar.GetCalendarItemsByMonth(null, null, false, 0); //one
        ///  
        ///    double totalBusyTime = 0;
        ///    Console.WriteLine($"{"Month",-10} | {"Details",-20} | {"Duration",-10}");
        ///
        ///    foreach (CalendarItemsByMonth listItem in defaultItems)
        ///    {
        ///       foreach (CalendarItem item in listItem.Items)
        ///       {
        ///            Console.WriteLine($"{listItem.Month,-11} {item.ShortDescription,-25} {item.DurationInMinutes}"); //loop over it
        ///            totalBusyTime += item.BusyTime;
        ///        }
        ///   }
        ///    Console.WriteLine("");
        ///    Console.WriteLine($"Total busy time: {totalBusyTime}");
        ///    ]]>
        ///    <b>Sample Output</b>
        /// Month      | Details              | Duration
        /// 2018/01     App Dev Homework          40
        /// 2018/01     Sprint retrospective      60
        /// 2018/01     staff meeting             15
        /// 2020/01     New Year's                1440
        /// 2020/01     Honolulu                  1440
        /// 2020/01     Honolulu                  1440
        /// 2020/01     Wendy's birthday          1440
        /// 2020/01     On call security          180
        /// 
        /// Total busy time: 21170
        ///
        ///
        /// </code>
        /// </example>
        public List<CalendarItemsByMonth> GetCalendarItemsByMonth(DateTime? Start, DateTime? End, bool FilterFlag, int CategoryID)
        {

            Start = Start ?? new DateTime(1900, 1, 1);
            End = End ?? new DateTime(2500, 1, 1);

            // Initializing an empty list of calendar items by month
            List<CalendarItemsByMonth> itemsByMonth = new List<CalendarItemsByMonth>();
            // Initializing a busy time of 0

            Double totalBusyTime = 0;

            var cmd = new SQLiteCommand(_connection);

            // If the filter flag is true, we only select events within the time frame and with the specified category id
            if (FilterFlag)
            {
                cmd.CommandText = $"SELECT STRFTIME('%Y-%m', e.StartDateTime) AS eventMonth FROM events e " +
                    $"INNER JOIN categories c ON e.CategoryId = c.Id WHERE e.StartDateTime >= @start AND e.StartDateTime <= @end AND e.CategoryId = @catId GROUP BY eventMonth";
                cmd.Parameters.AddWithValue("@start", Start);
                cmd.Parameters.AddWithValue("@end", End);
                cmd.Parameters.AddWithValue("@catId", CategoryID);
            }
            // If the filter flag is false, we only select events within the time frame and with any category
            else
            {
                cmd.CommandText = $"SELECT STRFTIME('%Y-%m', e.StartDateTime) AS eventMonth FROM events e " +
                    $"INNER JOIN categories c ON e.CategoryId = c.Id WHERE e.StartDateTime >= @start AND e.StartDateTime <= @end GROUP BY eventMonth";
                cmd.Parameters.AddWithValue("@start", Start);
                cmd.Parameters.AddWithValue("@end", End);
            }

            // NOTE: By using SELECT STRFTIME('%Y-%m', e.StartDateTime), I get the month and year of the grouped events in a YYYY-MM format.

            // Reading the result(s) of the query
            SQLiteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                // Retrieving the common month and year of the events
                string monthAndYear = (string)(reader["eventMonth"]);

                DateTime newStartTime = Convert.ToDateTime(monthAndYear + "-01"); // First day of the month
                DateTime newEndTime = Convert.ToDateTime(monthAndYear + "-31"); // Last day of the month

                // Retrieving all events that are within this month and year using our new start and end times
                List<CalendarItem> items = GetCalendarItems(newStartTime, newEndTime, FilterFlag, CategoryID);

                // For the purpose of the CalendarItemByMonth tests, the - needs to be replaced by a /
                string formatted = monthAndYear.Replace('-', '/');
                // Adding a the CalendarItemsByMonth to our list
                itemsByMonth.Add(new CalendarItemsByMonth{ Items = items, Month = formatted, TotalBusyTime = items[items.Count -1].BusyTime });
            }
            // Returning the list of CalendarItemsByMonth once we have looped trough all the months returned by the query
            return itemsByMonth;

        }

        // ============================================================================
        // Group all events by category (ordered by category name)
        // ============================================================================
        /// <summary>
        /// Searches for calendar items grouping them by category type, and sorts based on start/end time, a filte flag if choosing to sort by category Id, aswell as the category Id you'd like to sort by.
        /// </summary>
        /// <param name="Start">The start time of calendar items.</param>
        /// <param name="End">The end time of calendar items.</param>
        /// <param name="FilterFlag">A boolean value representing if you'd like to sort by category ids.</param>
        /// <param name="CategoryID">A category Id allowing for sorting specific calendar instances.</param>
        /// <returns>A list of calendar items by category that follow the parameter guidelines.</returns>
        /// Basic use cases for getting calendar items
        /// <example>
        /// Assume all the below examples have this input
        /// <code>
        /// Cat_ID  Event_ID  StartDateTime           Details                 DurationInMinutes
        ///    3       1      1/10/2018 10:00:00 AM   App Dev Homework             40
        ///    9       2      1/9/2020 12:00:00 AM    Honolulu		        1440
        ///    9       3      1/10/2020 12:00:00 AM   Honolulu                   1440
        ///    7       4      1/20/2020 11:00:00 AM   On call security            180
        ///    2       5      1/11/2018 7:30:00 PM    staff meeting                15
        ///    8       6      1/1/2020 12:00:00 AM    New Year's                 1440
        ///   11       7      1/12/2020 12:00:00 AM   Wendy's birthday           1440
        ///    2       8      1/11/2018 10:15:00 AM   Sprint retrospective         60
        /// </code>
        /// <code>
        /// <![CDATA[
        ///    List<CalendarItemsByCategory> defaultItems = calendar.GetCalendarItemsByCategory(null, null, false, 0); //one
        /// 
        ///    double totalBusyTime = 0;
        ///    Console.WriteLine($"{"Categories",-30} | {"Details",-20} | {"Duration"}");
        ///
        ///    foreach (CalendarItemsByCategory listItems in defaultItems)
        ///    {
        ///        foreach (CalendarItem item in listItems.Items)
        ///        {
        /// 
        ///            Console.WriteLine($"{listItems.Category,-31} {item.ShortDescription,-23} {item.DurationInMinutes}"); //loop over it
        ///            totalBusyTime += item.BusyTime;
        ///        }
        ///    }
        ///    Console.WriteLine("");
        ///    Console.WriteLine($"Total busy time: {totalBusyTime}");
        ///    ]]>
        ///    <b>Sample Output</b>
        /// Categories                     | Details              | Duration
        /// Birthdays                       Wendy's birthday        1440
        /// Canadian Holidays               New Year's              1440
        /// Fun                             App Dev Homework        40
        /// On call                         On call security        180
        /// Vacation                        Honolulu                1440
        /// Vacation                        Honolulu                1440
        /// Work                            Sprint retrospective    60
        /// Work                            staff meeting           15
        ///
        /// Total busy time: 21170
        ///
        ///
        ///
        /// </code>
        /// </example>
        public List<CalendarItemsByCategory> GetCalendarItemsByCategory(DateTime? Start, DateTime? End, bool FilterFlag, int CategoryID)
        {
            // -----------------------------------------------------------------------
            // get all items first
            // -----------------------------------------------------------------------
            Start = Start ?? new DateTime(1900, 1, 1);
            End = End ?? new DateTime(2500, 1, 1);

            // Initializng an empty list of calendar items by category
            List<CalendarItemsByCategory> itemsByCategory = new List<CalendarItemsByCategory>();

            var cmd = new SQLiteCommand(_connection);
            // If the filter flag is true, we only select events within the time frame and with the specified category id. Grouping by category.
            if (FilterFlag)
            {
                cmd.CommandText = $"SELECT e.CategoryId, c.Description FROM events e " +
                $"INNER JOIN categories c ON e.CategoryId = c.Id WHERE e.StartDateTime >= @start AND e.StartDateTime <= @end AND e.CategoryId = @catId GROUP BY c.Id ORDER BY c.Description";
                cmd.Parameters.AddWithValue("@start", Start);
                cmd.Parameters.AddWithValue("@end", End);
                cmd.Parameters.AddWithValue("@catId", CategoryID);
            }
            // If the filter flag is true, we only select events within the time frame. Grouping by category.
            else
            {
                cmd.CommandText = $"SELECT e.CategoryId, c.Description FROM events e " +
               $"INNER JOIN categories c ON e.CategoryId = c.Id WHERE e.StartDateTime >= @start AND e.StartDateTime <= @end GROUP BY c.Id ORDER BY c.Description";
                cmd.Parameters.AddWithValue("@start", Start);
                cmd.Parameters.AddWithValue("@end", End);
            }
            // Reading the query results
            SQLiteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                // Retrieving all the fields needed to make the calendar items by category
                int categoryId = Convert.ToInt32(reader["CategoryId"]);
                string desc = (string)reader["Description"];
                // Retrieving the calendar items with this specific category
                List<CalendarItem> items = GetCalendarItems(Start, End, true, categoryId);
                // Creating the calendar item by category and adding it to the list
                itemsByCategory.Add(new CalendarItemsByCategory { Category = desc, Items = items, TotalBusyTime = items[items.Count-1].BusyTime });
            }
            return itemsByCategory;
        }



        // ============================================================================
        // Group all events by category and Month
        // creates a list of Dictionary objects with:
        //          one dictionary object per month,
        //          and one dictionary object for the category total busy times
        // 
        // Each per month dictionary object has the following key value pairs:
        //           "Month", <name of month>
        //           "TotalBusyTime", <the total durations for the month>
        //             for each category for which there is an event in the month:
        //             "items:category", a List<CalendarItem>
        //             "category", the total busy time for that category for this month
        // The one dictionary for the category total busy times has the following key value pairs:
        //             for each category for which there is an event in ANY month:
        //             "category", the total busy time for that category for all the months
        // ============================================================================
        /// <summary>
        /// Creates a summary of calendar items sorted by a start/end time, a filter flag determining if you'd like to sort by a category id, aswell as the category you can optionally sort by. The summary includes the total busy time for easier readability.
        /// </summary>
        /// <param name="Start">The start time of calendar items.</param>
        /// <param name="End">The start time of calendar items.</param>
        /// <param name="FilterFlag">A boolean value representing if you'd like to sort by category ids.</param>
        /// <param name="CategoryID">A category Id allowing for sorting specific calendar instances.</param>
        /// <returns>A list of dictionaries representing a list of calendar months with all the data, whilst following the parameter guidelines.</returns>
        /// Basic use cases for getting calendar items
        /// <example>
        /// Assume all the below examples have this input
        /// <code>
        /// Cat_ID  Event_ID  StartDateTime           Details                 DurationInMinutes
        ///    3       1      1/10/2018 10:00:00 AM   App Dev Homework             40
        ///    9       2      1/9/2020 12:00:00 AM    Honolulu		        1440
        ///    9       3      1/10/2020 12:00:00 AM   Honolulu                   1440
        ///    7       4      1/20/2020 11:00:00 AM   On call security            180
        ///    2       5      1/11/2018 7:30:00 PM    staff meeting                15
        ///    8       6      1/1/2020 12:00:00 AM    New Year's                 1440
        ///   11       7      1/12/2020 12:00:00 AM   Wendy's birthday           1440
        ///    2       8      1/11/2018 10:15:00 AM   Sprint retrospective         60
        /// </code>
        /// <code>
        /// <![CDATA[
        ///    List<CalendarItemsByCategory> defaultItems = calendar.GetCalendarItemsByCategory(null, null, false, 0); //one
        /// 
        ///    double totalBusyTime = 0;
        ///    Console.WriteLine($"{"Categories",-30} | {"Details",-20} | {"Duration"}");
        ///
        ///    foreach (CalendarItemsByCategory listItems in defaultItems)
        ///    {
        ///        foreach (CalendarItem item in listItems.Items)
        ///        {
        /// 
        ///            Console.WriteLine($"{listItems.Category,-31} {item.ShortDescription,-23} {item.DurationInMinutes}"); //loop over it
        ///            totalBusyTime += item.BusyTime;
        ///        }
        ///    }
        ///    Console.WriteLine("");
        ///    Console.WriteLine($"Total busy time: {totalBusyTime}");
        ///    ]]>
        ///    <b>Sample Output</b>
        /// Month: 2018/01
        /// Category: Fun
        /// Item            | Description               | Duration
        /// 1                 App Dev Homework            40
        /// Category: Work
        /// Item            | Description               | Duration
        /// 8                 Sprint retrospective        60
        /// 5                 staff meeting               15
        ///
        /// Total busy time: 115
        ///
        /// Month: 2020/01
        /// Category: Birthdays
        /// Item            | Description               | Duration
        /// 7                 Wendy's birthday            1440
        /// Category: Canadian Holidays
        /// Item            | Description               | Duration
        /// 6                 New Year's                  1440
        /// Category: On call
        /// Item            | Description               | Duration
        /// 4                 On call security            180
        /// Category: Vacation
        /// Item            | Description               | Duration
        /// 2                 Honolulu                    1440
        /// 3                 Honolulu                    1440
        ///
        /// Total busy time: 5940
        ///
        /// Month: TOTALS
        /// Category: Month Total time spent: TOTALS
        /// Category: Work Total time spent: 75
        /// Category: Fun Total time spent: 40
        /// Category: On call Total time spent: 180
        /// Category: Canadian Holidays Total time spent: 1440
        /// Category: Vacation Total time spent: 2880
        /// Category: Birthdays Total time spent: 1440
        ///
        /// Total busy time: 21170
        ///
        ///
        ///
        ///
        /// </code>
        /// </example>
        public List<Dictionary<string,object>> GetCalendarDictionaryByCategoryAndMonth(DateTime? Start, DateTime? End, bool FilterFlag, int CategoryID)
        {
            // -----------------------------------------------------------------------
            // get all items by month 
            // -----------------------------------------------------------------------
            List<CalendarItemsByMonth> GroupedByMonth = GetCalendarItemsByMonth(Start, End, FilterFlag, CategoryID);

            // -----------------------------------------------------------------------
            // loop over each month
            // -----------------------------------------------------------------------
            var summary = new List<Dictionary<string, object>>();
            var totalBusyTimePerCategory = new Dictionary<String, Double>();

            foreach (var MonthGroup in GroupedByMonth)
            {
                // create record object for this month
                Dictionary<string, object> record = new Dictionary<string, object>();
                record["Month"] = MonthGroup.Month;
                record["TotalBusyTime"] = MonthGroup.TotalBusyTime;

                // break up the month items into categories
                var GroupedByCategory = MonthGroup.Items.GroupBy(c => c.Category);

                // -----------------------------------------------------------------------
                // loop over each category
                // -----------------------------------------------------------------------
                foreach (var CategoryGroup in GroupedByCategory.OrderBy(g => g.Key))
                {

                    // calculate totals for the cat/month, and create list of items
                    double totalCategoryBusyTimeForThisMonth = 0;
                    var details = new List<CalendarItem>();

                    foreach (var item in CategoryGroup)
                    {
                        totalCategoryBusyTimeForThisMonth = totalCategoryBusyTimeForThisMonth + item.DurationInMinutes;
                        details.Add(item);
                    }

                    // add new properties and values to our record object
                    record["items:" + CategoryGroup.Key] = details;
                    record[CategoryGroup.Key] = totalCategoryBusyTimeForThisMonth;

                    // keep track of totals for each category
                    if (totalBusyTimePerCategory.TryGetValue(CategoryGroup.Key, out Double currentTotalBusyTimeForCategory))
                    {
                        totalBusyTimePerCategory[CategoryGroup.Key] = currentTotalBusyTimeForCategory + totalCategoryBusyTimeForThisMonth;
                    }
                    else
                    {
                        totalBusyTimePerCategory[CategoryGroup.Key] = totalCategoryBusyTimeForThisMonth;
                    }
                }

                // add record to collection
                summary.Add(record);
            }
            // ---------------------------------------------------------------------------
            // add final record which is the totals for each category
            // ---------------------------------------------------------------------------
            Dictionary<string, object> totalsRecord = new Dictionary<string, object>();
            totalsRecord["Month"] = "TOTALS";

            foreach (var cat in categories.List())
            {
                try
                {
                    totalsRecord.Add(cat.Description, totalBusyTimePerCategory[cat.Description]);
                }
                catch { }
            }
            summary.Add(totalsRecord);


            return summary;
        }




    }
}
