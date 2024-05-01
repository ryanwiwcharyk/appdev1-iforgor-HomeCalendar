using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Core.Mapping;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Calendar;
[assembly: InternalsVisibleTo("TestProject_Presenter")]


namespace HomeCalendarWPF
{
    public class Presenter
    {
        //private readonly ViewInterface view;

        private ICreateEventViewInterface createEventView;
        private CategoryView createCategoryView;
        private HomeInterface homeView;
        private MainViewInterface mainView;
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

        private string FindCategoryNameById(int id)
        {
            List<Category> cats = GetCategoryList();
            Category cat = cats.Find(x => x.Id == id);
            return cat.Description;
        }

        #region Properties

        internal MainViewInterface MainViewInterface { get { return mainView; } }

        internal HomeInterface HomeInterface { get { return homeView; } }

        internal ICreateEventViewInterface createEventViewInterface { get { return createEventView; } }

        internal CategoryView CategoryView { get {  return createCategoryView; } }    


        #endregion

        #region Window Registration

        public void RegisterWindow(ICreateEventViewInterface view)
        {
            createEventView = view;
        }

        public void RegisterWindow(CategoryView view)
        {
            createCategoryView = view;
        }

        public void RegisterWindow(HomeInterface view)
        {
            homeView = view;
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
            //ObservableCollection<CalendarItem> events2 = new ObservableCollection<CalendarItem>(events);
            List<string> names = new List<string>();
            foreach (CalendarItem item in events)
            {
                names.Add($"{item.ShortDescription} - {item.StartDateTime}");
            }
            if (events.Count == 0)
                homeView.ShowNoUpcomingEvents("There are no upcoming events");
            else
                homeView.ShowUpcomingEvents(events);
        }

        public void PopulateCategoryDropdownInHomePage() //combo box category drop down
        {
            Categories categories = model.categories;
            List<Category> categoryList = categories.List();
            homeView.AddCategoriesToDropdown(categoryList);
        }

        public void ValidateFilterToggleByCategory(Category selectedCategory, DateTime? start, DateTime? end)
        {
            if (selectedCategory!=null)
            {
                List<CalendarItem> updatedList = model.GetCalendarItems(start, end, true, selectedCategory.Id);
                homeView.ShowUpcomingEvents(updatedList);
            }
                
        }

        public void GetEventsFilteredByDate(DateTime? startDate, DateTime? endDate, bool filter = false, int catId = 0)
        {
            List<CalendarItem> ci = model.GetCalendarItems(startDate, endDate, filter, catId);
            homeView.ShowUpcomingEvents(ci);
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

        public void GetEventsSortedByMonth(DateTime? start, DateTime? end)
        {
            if (start is null)
                start = DateTime.MinValue;
            if (end is null)
                end = DateTime.MaxValue;
            //Change to accept a filter flag if it is selected
            List<CalendarItemsByMonth> calendarItems = model.GetCalendarItemsByMonth(start, end, false, 0);
            homeView.ShowUpcomingEventsByMonth(calendarItems);

        }

        public void GetEventsByMonthAndCategory(DateTime? start, DateTime? end)
        {
            List<Dictionary<string, object>> calendarItems = model.GetCalendarDictionaryByCategoryAndMonth(start, end, false, 0);
            List<Category> categories = GetCategoryList();
            homeView.ShowUpcomingEventsByMonthAndCategory(calendarItems, categories);

        }

        #endregion

        #region Create Events
        public void PopulateCategoryDropdown()
        {
            List<Category> categoryList = GetCategoryList();
            createEventView.AddCategoriesToDropdown(categoryList);
        }

        public void ValidateEventFormInputAndCreate(string details, string duration, DateTime? startTime, string selectedCategory)
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
            else if (string.IsNullOrEmpty(selectedCategory))
            {
                createEventView.ShowErrorPopup("Please pick a category.");
            }
            else
            {
                List<Category> categories = model.categories.List();
                Category category = categories.Find(x => x.Description == selectedCategory);
                
                if (category != null && startTime != null)
                {
                    model.events.Add((DateTime)startTime, category.Id, validDurationAsDouble, details);
                    createEventView.ShowSuccessPopup("Event was successfully created.");
                    GetUpcomingEvents();
                    
                }
                else
                {
                    createEventView.ShowErrorPopup("The selected category could not be found.");
                }

            }
        }

        #endregion

        #region Create Category

        //populate the category Types combo box

        //add category based on details + category type with validation

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
                }
            }

            
        }

      
        #endregion
    }
}
