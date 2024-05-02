using Calendar;
using HomeCalendarWPF.interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("TestProject_Presenter")]


namespace HomeCalendarWPF
{
    public class Presenter
    {
        private UpdateEventViewInterface createEventView;
        private CategoryView createCategoryView;
        private HomeInterface homeView;
        private MainViewInterface mainView;
        private UpdateEventViewInterface updateView;
        private HomeCalendar model;
        public Presenter(MainViewInterface window)
        {
            mainView = window;
        }

        private List<Category> GetCategoryList()
        {
            Categories categories = model.categories;
            return categories.List();
        }

        #region Properties

        internal MainViewInterface MainViewInterface { get { return mainView; } }

        internal HomeInterface HomeInterface { get { return homeView; } }

        internal UpdateEventViewInterface createEventViewInterface { get { return createEventView; } }

        internal CategoryView CategoryView { get { return createCategoryView; } }


        #endregion

        #region Window Registration

        public void RegisterWindow(CategoryView view)
        {
            createCategoryView = view;
        }

        public void RegisterWindow(HomeInterface view)
        {
            homeView = view;
        }

        public void RegisterWindow(UpdateEventViewInterface view)
        {
            updateView = view;
            createEventView = view;
        }


        #endregion

        #region Welcome Page

        public void NewCalendar(string location, string name)
        {
            model = new HomeCalendar($"{location}\\{name}", true);
            Home home = new Home(this);
            home.Show();
        }

        public void ExistingCalendar(string location)
        {
            model = new HomeCalendar($"{location}");
            Home home = new Home(this);
            home.Show();
        }


        #endregion

        #region Home Page

        public void GetUpcomingEvents()
        {

            List<CalendarItem> events = model.GetCalendarItems(null, null, false, 0);
            List<string> names = new List<string>();
            foreach (CalendarItem item in events)
            {
                names.Add($"{item.ShortDescription} - {item.StartDateTime}");
            }
            if (events.Count == 0)
                homeView.ShowNoUpcomingEvents();
            else
                homeView.ShowUpcomingEvents(events);
        }

        public void PopulateCategoryDropdownInHomePage() //combo box category drop down
        {
            List<Category> categoryList = GetCategoryList();
            homeView.AddCategoriesToDropdown(categoryList);
        }

        #region Filtering and Summaries
        public void ValidateFilterToggleByCategory(Category selectedCategory, DateTime? start, DateTime? end)
        {
            if (selectedCategory != null)
            {
                List<CalendarItem> updatedList = model.GetCalendarItems(start, end, true, selectedCategory.Id);
                homeView.ShowUpcomingEvents(updatedList);
            }

        }

        public void GetEventsFilteredByDate(DateTime? startDate, DateTime? endDate, bool filter = false, int catId = 0)
        {
            List<CalendarItem> events = model.GetCalendarItems(startDate, endDate, filter, catId);
            if (events.Count == 0)
                homeView.ShowNoUpcomingEvents();
            else
                homeView.ShowUpcomingEvents(events);
        }

        public void GetEventsSortedByCategory(DateTime? start, DateTime? end, bool filter = false, int catId = 0)
        {
            if (start is null)
                start = DateTime.MinValue;
            if (end is null)
                end = DateTime.MaxValue;
            //Change to accept a filter flag if it is selected
            List<CalendarItemsByCategory> calendarItems = model.GetCalendarItemsByCategory(start, end, filter, catId);
            homeView.ShowUpcomingEventsByCategory(calendarItems);

        }

        public void GetEventsSortedByMonth(DateTime? start, DateTime? end, bool filter = false, int catId = 0)
        {
            if (start is null)
                start = DateTime.MinValue;
            if (end is null)
                end = DateTime.MaxValue;
            //Change to accept a filter flag if it is selected
            List<CalendarItemsByMonth> calendarItems = model.GetCalendarItemsByMonth(start, end, filter, catId);
            homeView.ShowUpcomingEventsByMonth(calendarItems);

        }

        public void GetEventsByMonthAndCategory(DateTime? start, DateTime? end, bool filter = false, int catId = 0)
        {
            List<Dictionary<string, object>> calendarItems = model.GetCalendarDictionaryByCategoryAndMonth(start, end, filter, catId);
            List<Category> categories = GetCategoryList();
            homeView.ShowUpcomingEventsByMonthAndCategory(calendarItems, categories);

        }
        #endregion

        #endregion

        #region Create Events
        public void PopulateCategoryDropdown()
        {
            List<Category> categoryList = GetCategoryList();
            createEventView.AddCategoriesToDropdown(categoryList);
        }

        public void ValidateEventFormInputAndCreate(string details, string duration, DateTime? startTime, Category selectedCategory)
        {
            if (string.IsNullOrEmpty(details))
            {
                createEventView.ShowErrorPopup("Please provide event details.");
            }
            else if (!double.TryParse(duration, out double validDurationAsDouble))
            {
                createEventView.ShowErrorPopup("Please provide a valid duration.");
            }
            else if (validDurationAsDouble <= 0)
            {
                createEventView.ShowErrorPopup("Please provide a positive duration.");
            }
            else if (!startTime.HasValue) //from https://stackoverflow.com/questions/41447490/how-do-i-get-value-from-datepickerwpf-in-c
            {
                createEventView.ShowErrorPopup("Please provide a start time.");
            }
            else if (string.IsNullOrEmpty(selectedCategory.Description))
            {
                createEventView.ShowErrorPopup("Please pick a category.");
            }
            else
            {
                if (selectedCategory != null && startTime != null)
                {
                    model.events.Add((DateTime)startTime, selectedCategory.Id, validDurationAsDouble, details);
                    createEventView.ShowSuccessPopup("Event was successfully created.");
                    GetUpcomingEvents();
                }
                else
                {
                    createEventView.ShowErrorPopup("The selected category could not be found.");
                }

            }
        }

        public void PopulateCreateEventFields()
        {
            string detailsPlaceholder = "Enter event details here...";
            DateTime startTime = DateTime.Now;
            double duration = 0;
            int hour = 0;
            int minute = 0;
            string categoryPlaceholder = "Select a category";
            createEventView.ShowPopulatedFields(detailsPlaceholder,duration,startTime,hour,minute,categoryPlaceholder);
        }
        public void DeleteEvent(CalendarItem item)
        {
            model.events.Delete(item.EventID);
            GetUpcomingEvents();

        }

        #endregion

        #region Update Events
        public void PopulateCategoryDropdownForUpdate()
        {
            List<Category> categoryList = GetCategoryList();
            updateView.AddCategoriesToDropdown(categoryList);
        }

        public void ValidateEventFormInputAndUpdate(int eventId, string details, string duration, DateTime? startTime, Category selectedCategory)
        {
            if (string.IsNullOrEmpty(details))
            {
                updateView.ShowErrorPopup("Please provide event details.");
            }
            else if (!double.TryParse(duration, out double validDurationAsDouble))
            {
                updateView.ShowErrorPopup("Please provide a valid duration.");
            }
            else if (validDurationAsDouble <= 0)
            {
                updateView.ShowErrorPopup("Please provide a positive duration.");
            }
            else if (!startTime.HasValue) //from https://stackoverflow.com/questions/41447490/how-do-i-get-value-from-datepickerwpf-in-c
            {
                updateView.ShowErrorPopup("Please provide a start time.");
            }
            else if (selectedCategory is null)
            {
                updateView.ShowErrorPopup("Please pick a category.");
            }
            else
            {

                if (selectedCategory != null && startTime != null)
                {
                    model.events.UpdateProperties(eventId, (DateTime)startTime, selectedCategory.Id, validDurationAsDouble, details);
                    updateView.ShowSuccessPopup("Event was successfully updated.");
                    GetUpcomingEvents();

                }
                else
                {
                    updateView.ShowErrorPopup("The selected category could not be found.");
                }

            }
        }

        public void PopulateUpdateEventFields(CalendarItem item)
        {
            List<Event> events = model.events.List();
            List<Category> cats = model.categories.List();
            Event eventToUpdate = events.Find(x => x.Id == item.EventID);
            Category cat = cats.Find(x => x.Id == item.CategoryID);

            string details = eventToUpdate.Details;
            double duration = eventToUpdate.DurationInMinutes;
            DateTime start = eventToUpdate.StartDateTime;
            int hour = eventToUpdate.StartDateTime.Hour;
            int minute = eventToUpdate.StartDateTime.Minute;

            updateView.ShowPopulatedFields(details,duration,start,hour,minute,cat);
        }
        public void DeleteEvent(CalendarItem item)
        {
            model.events.Delete(item.EventID);
            GetUpcomingEvents();
            

        }
        public void ViewSelector(bool summaryByMonthChecked, bool summaryByCategoryChecked, bool filterByCategoryChecked, Category? selectedCategory, DateTime? startDate, DateTime? endDate)
        {
            if (filterByCategoryChecked)
            {
                if  (selectedCategory != null)
                {
                    if (summaryByCategoryChecked && summaryByMonthChecked)
                    {
                        GetEventsByMonthAndCategory(startDate, endDate,true,selectedCategory.Id);
                    }
                    else if (summaryByMonthChecked)
                    {
                        GetEventsSortedByMonth(startDate, endDate, true, selectedCategory.Id);
                    }
                    else if (summaryByCategoryChecked)
                    {
                        GetEventsSortedByCategory(startDate, endDate, true, selectedCategory.Id);
                    }
                    else
                    {
                        GetEventsFilteredByDate(startDate, endDate, true, selectedCategory.Id);
                    }
                }
                else
                {
                    homeView.ShowNoUpcomingEvents();
                }

            }
            else
            {
                if (summaryByCategoryChecked && summaryByMonthChecked)
                {
                    GetEventsByMonthAndCategory(startDate, endDate);
                }
                else if (summaryByMonthChecked)
                {
                    GetEventsSortedByMonth(startDate, endDate);
                }
                else if (summaryByCategoryChecked)
                {
                    GetEventsSortedByCategory(startDate, endDate);
                }
                else
                {
                    GetEventsFilteredByDate(startDate, endDate);
                }
            }

        }

        #endregion

        #region Create Category

        public void PopulateCategoryTypesDropdown()
        {
            List<Category.CategoryType> categoryTypes = new List<Category.CategoryType>();

            Category.CategoryType[] catTypes = (Category.CategoryType[])Enum.GetValues(typeof(Category.CategoryType));
            foreach (Category.CategoryType catType in catTypes)
            {
                categoryTypes.Add(catType);
            }

            createCategoryView.FillDropDown(categoryTypes);
        }

        public void ValidateDetailsFormInputAndCreateCategory(string details, string type) //called on the add button event handler.
        {

            int typeAsNumber = 0;
            switch (type)
            {
                case "Event":
                    typeAsNumber = 1;
                    break;

                case "Availability":
                    typeAsNumber = 2;
                    break;
                case "AllDayEvent":
                    typeAsNumber = 3;
                    break;

                case "Holiday":
                    typeAsNumber = 4;
                    break;
                default:
                    typeAsNumber = 0;
                    break;

            }

            if (string.IsNullOrEmpty(details))
            {
                createCategoryView.ShowWarning("Error, details cannot be empty. Please enter the name of the details of the category");
            }
            else if (typeAsNumber == 0)
            {
                createCategoryView.ShowWarning("Error, Please select the category type for your new category");
            }
            else
            {
                createCategoryView.ShowSuccessPopup("New category was successfully created.");
                model.categories.Add(details, (Category.CategoryType)typeAsNumber);
                if (createEventView != null)
                {
                    PopulateCategoryDropdown();
                    PopulateCategoryDropdownForUpdate();
                }
            }


        }


        #endregion
    }
}
