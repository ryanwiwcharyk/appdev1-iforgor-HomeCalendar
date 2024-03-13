namespace Calendar
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //use path for test files located at the exec scope
            // load the file, hardcode the getcalendar parameters and the user will have a chose of which function they wish to run
            // then print the console app just like how she displays it


            //Loading test file from exec level

        }

        private static void PrintAndReadMenu(HomeCalendar calendar)
        {
            Console.WriteLine($@"1.DisplayCalendarItems,
2.DisplayCalendarItemsByMonth,
3.DisplayCalendarItemsByCategory,
4.DisplayCalendarDictionaryByCategoryAndMonth");

            int userInput;
            bool validInput;

            validInput = int.TryParse(Console.ReadLine(), out userInput);

            while(!validInput || userInput > 4 || userInput < 1)
            {
                Console.WriteLine("ERROR: Please enter an integer between 1-4");
            }

            switch(userInput)
            {
                case 1:
                    DisplayCalendarItems(calendar);
                    break;
                    
                case 2:
                    DisplayCalendarItemsByMonth(calendar); 
                    break;
                    
                case 3:
                    DisplayCalendarItemsByCategory(calendar);
                    break;

                case 4: DisplayCalendarDictionaryByCategoryAndMonth(calendar);
                    break;
            }
        }

        #region DisplayCalendarItems
        public static void DisplayCalendarItems(HomeCalendar calendar)
        {
            //each of these methods will contain 3 use cases 
            //first use case will be default meaning all null false and 0 
            //second use case will be a start and end date but false and 0
            //and finally the third use case will be no dates but it will group by a category so true and a category id

            Console.WriteLine($@"1.Use Case 1,
2.Use Case 2,
3.Use Case 3");

            int userInput;
            bool validInput;

            validInput = int.TryParse(Console.ReadLine(), out userInput);

            while (!validInput || userInput > 3 || userInput < 1)
            {
                Console.WriteLine("ERROR: Please enter an integer between 1-3");
            }

            switch (userInput)
            {
                case 1:
                    DisplayDefaultItemsUseCaseOne(calendar);
                    break;

                case 2:
                    DisplayDefaultItemsUseCaseTwo(calendar);
                    break;

                case 3:
                    DisplayDefaultItemsUseCaseThree(calendar);
                    break;
            }
        }

        public static void DisplayDefaultItemsUseCaseOne(HomeCalendar calendar)
        {
            //USE CASE 1
            List<CalendarItem> defaultItems = calendar.GetCalendarItems(null, null, false, 0); //one

            for (int i = 0; i < defaultItems.Count; i++)
            {
                Console.WriteLine("Cat_ID,      Event_ID,       StartDateTime,      Details,        DurationInMinutes");
                Console.WriteLine($"{defaultItems[i].CategoryID}, {defaultItems[i].EventID}, {defaultItems[i].StartDateTime}, {defaultItems[i].ShortDescription}, {defaultItems[i].DurationInMinutes}"); //loop over it
            }
            Console.WriteLine("");
        }

        public static void DisplayDefaultItemsUseCaseTwo(HomeCalendar calendar)
        {
            //USE CASE 2
            List<CalendarItem> calendarItemsByMonth = calendar.GetCalendarItems(DateTime.Now.AddYears(-6), DateTime.Now, false, 0); //two

            for (int i = 0; i < calendarItemsByMonth.Count; i++)
            {
                Console.WriteLine($"{calendarItemsByMonth[i].CategoryID}, {calendarItemsByMonth[i].EventID}, {calendarItemsByMonth[i].StartDateTime}, {calendarItemsByMonth[i].ShortDescription}, {calendarItemsByMonth[i].DurationInMinutes}"); //loop over it
            }
            Console.WriteLine("");
        }

        public static void DisplayDefaultItemsUseCaseThree(HomeCalendar calendar)
        {
            //USE CASE 3
            List<CalendarItem> calendarItemsByCategory = calendar.GetCalendarItems(null, null, true, 8); //three

            for (int i = 0; i < calendarItemsByCategory.Count; i++)
            {
                Console.WriteLine($"{calendarItemsByCategory[i].CategoryID}, {calendarItemsByCategory[i].EventID}, {calendarItemsByCategory[i].StartDateTime}, {calendarItemsByCategory[i].ShortDescription}, {calendarItemsByCategory[i].DurationInMinutes}"); //loop over it
            }
        }

        #endregion

        #region DisplayCalendarItemsByMonth
        public static void DisplayCalendarItemsByMonth(HomeCalendar calendar)
        {
            Console.WriteLine($@"1.Use Case 1,
2.Use Case 2,
3.Use Case 3");

            int userInput;
            bool validInput;

            validInput = int.TryParse(Console.ReadLine(), out userInput);

            while (!validInput || userInput > 3 || userInput < 1)
            {
                Console.WriteLine("ERROR: Please enter an integer between 1-3");
            }

            switch (userInput)
            {
                case 1:
                    DisplayMonthlyItemsUseCaseOne(calendar);
                    break;

                case 2:
                    DisplayMonthlyItemsUseCaseTwo(calendar);
                    break;

                case 3:
                    DisplayMonthlyItemsUseCaseThree(calendar);
                    break;
            }
        }

        public static void DisplayMonthlyItemsUseCaseOne(HomeCalendar calendar)
        {
            //USE CASE 1
            List<CalendarItemsByMonth> defaultItems = calendar.GetCalendarItemsByMonth(null, null, false, 0); //one

            for (int i = 0; i < defaultItems.Count; i++)
            {
                Console.WriteLine("Month,       Item,       TotalBusyTime");
                Console.WriteLine($"{defaultItems[i].Month}, {defaultItems[i].Items}, {defaultItems[i].TotalBusyTime}"); //loop over it
            }
            Console.WriteLine("");
        }

        public static void DisplayMonthlyItemsUseCaseTwo(HomeCalendar calendar)
        {
            //USE CASE 2
            List<CalendarItemsByMonth> calendarItemsByMonth = calendar.GetCalendarItemsByMonth(DateTime.Now.AddYears(-6), DateTime.Now, false, 0); //two

            for (int i = 0; i < calendarItemsByMonth.Count; i++)
            {
                Console.WriteLine("Month,       Item,       TotalBusyTime");
                Console.WriteLine($"{calendarItemsByMonth[i].Month}, {calendarItemsByMonth[i].Items}, {calendarItemsByMonth[i].TotalBusyTime}"); //loop over it
            }
            Console.WriteLine("");
        }

        public static void DisplayMonthlyItemsUseCaseThree(HomeCalendar calendar)
        {
            //USE CASE 3
            List<CalendarItemsByMonth> calendarItemsByCategory = calendar.GetCalendarItemsByMonth(null, null, true, 8); //three

            for (int i = 0; i < calendarItemsByCategory.Count; i++)
            {
                Console.WriteLine("Month,       Item,       TotalBusyTime");
                Console.WriteLine($"{calendarItemsByCategory[i].Month}, {calendarItemsByCategory[i].Items}, {calendarItemsByCategory[i].TotalBusyTime}"); //loop over it
            }
        }


        #endregion

        #region DisplayCalendarItemsByCategory
        public static void DisplayCalendarItemsByCategory(HomeCalendar calendar)
        {
            Console.WriteLine($@"1.Use Case 1,
2.Use Case 2,
3.Use Case 3");

            int userInput;
            bool validInput;

            validInput = int.TryParse(Console.ReadLine(), out userInput);

            while (!validInput || userInput > 3 || userInput < 1)
            {
                Console.WriteLine("ERROR: Please enter an integer between 1-3");
            }

            switch (userInput)
            {
                case 1:
                    DisplayCategoryItemsUseCaseOne(calendar);
                    break;

                case 2:
                    DisplayCategoryItemsUseCaseTwo(calendar);
                    break;

                case 3:
                    DisplayCategoryItemsUseCaseThree(calendar);
                    break;
            }
        }

        public static void DisplayCategoryItemsUseCaseOne(HomeCalendar calendar)
        {
            //USE CASE 1
            List<CalendarItemsByCategory> defaultItems = calendar.GetCalendarItemsByCategory(null, null, false, 0); //one

            for (int i = 0; i < defaultItems.Count; i++)
            {
                Console.WriteLine("Category,        Items,      TotalBusyTime");
                Console.WriteLine($"{defaultItems[i].Category}, {defaultItems[i].Items}, {defaultItems[i].TotalBusyTime}"); //loop over it
            }
            Console.WriteLine("");
        }

        public static void DisplayCategoryItemsUseCaseTwo(HomeCalendar calendar)
        {
            //USE CASE 2
            List<CalendarItemsByCategory> calendarItemsByMonth = calendar.GetCalendarItemsByCategory(DateTime.Now.AddYears(-6), DateTime.Now, false, 0); //two

            for (int i = 0; i < calendarItemsByMonth.Count; i++)
            {
                Console.WriteLine("Category,        Items,      TotalBusyTime");
                Console.WriteLine($"{calendarItemsByMonth[i].Category}, {calendarItemsByMonth[i].Items}, {calendarItemsByMonth[i].TotalBusyTime}"); //loop over it
            }
            Console.WriteLine("");
        }

        public static void DisplayCategoryItemsUseCaseThree(HomeCalendar calendar)
        {
            //USE CASE 3
            List<CalendarItemsByCategory> calendarItemsByCategory = calendar.GetCalendarItemsByCategory(null, null, true, 8); //three

            for (int i = 0; i < calendarItemsByCategory.Count; i++)
            {
                Console.WriteLine("Category,        Items,      TotalBusyTime");
                Console.WriteLine($"{calendarItemsByCategory[i].Category}, {calendarItemsByCategory[i].Items}, {calendarItemsByCategory[i].TotalBusyTime}"); //loop over it
            }
        }

        #endregion

        #region DisplayCalendarDictionaryByCategoryAndMonth
        public static void DisplayCalendarDictionaryByCategoryAndMonth(HomeCalendar calendar)
        {
            Console.WriteLine($@"1.Use Case 1,
2.Use Case 2,
3.Use Case 3");

            int userInput;
            bool validInput;

            validInput = int.TryParse(Console.ReadLine(), out userInput);

            while (!validInput || userInput > 3 || userInput < 1)
            {
                Console.WriteLine("ERROR: Please enter an integer between 1-3");
            }

            switch (userInput)
            {
                case 1:
                    DisplayDictionaryItemsUseCaseOne(calendar);
                    break;

                case 2:
                    DisplayDictionaryItemsUseCaseTwo(calendar);
                    break;

                case 3:
                    DisplayDictionaryItemsUseCaseThree(calendar);
                    break;
            }
        }

        public static void DisplayDictionaryItemsUseCaseOne(HomeCalendar calendar)
        {
            //USE CASE 1
            //List<Dictionary<string, object>> GetCalendarDictionaryByCategoryAndMonth(null, null, false, 0); //one

            //for (int i = 0; i < defaultItems.Count; i++)
            //{
            //    Console.WriteLine("Category,        Items,      TotalBusyTime");
            //    Console.WriteLine($"{defaultItems[i].Category}, {defaultItems[i].Items}, {defaultItems[i].TotalBusyTime}"); //loop over it
            //}
            //Console.WriteLine("");
        }

        public static void DisplayDictionaryItemsUseCaseTwo(HomeCalendar calendar)
        {
            //USE CASE 2
            List<CalendarItemsByCategory> calendarItemsByMonth = calendar.GetCalendarItemsByCategory(DateTime.Now.AddYears(-6), DateTime.Now, false, 0); //two

            for (int i = 0; i < calendarItemsByMonth.Count; i++)
            {
                Console.WriteLine("Category,        Items,      TotalBusyTime");
                Console.WriteLine($"{calendarItemsByMonth[i].Category}, {calendarItemsByMonth[i].Items}, {calendarItemsByMonth[i].TotalBusyTime}"); //loop over it
            }
            Console.WriteLine("");
        }

        public static void DisplayDictionaryItemsUseCaseThree(HomeCalendar calendar)
        {
            //USE CASE 3
            List<CalendarItemsByCategory> calendarItemsByCategory = calendar.GetCalendarItemsByCategory(null, null, true, 8); //three

            for (int i = 0; i < calendarItemsByCategory.Count; i++)
            {
                Console.WriteLine("Category,        Items,      TotalBusyTime");
                Console.WriteLine($"{calendarItemsByCategory[i].Category}, {calendarItemsByCategory[i].Items}, {calendarItemsByCategory[i].TotalBusyTime}"); //loop over it
            }
        }

        #endregion
    }
}

//i want the user to select 1 - 4 and display the test calendar. 
