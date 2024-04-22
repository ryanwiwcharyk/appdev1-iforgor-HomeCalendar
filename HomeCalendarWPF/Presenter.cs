using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Mapping;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Calendar;


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

        #region Properties

        public MainViewInterface MainViewInterface { get { return mainView; } }

        public HomeInterface HomeInterface { get { return homeView; } }

        public ICreateEventViewInterface createEventViewInterface { get { return createEventView; } }

        public CategoryView CategoryView { get {  return createCategoryView; } }    


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
            const int weekInAdvance = 7;
            List<CalendarItem> events = model.GetCalendarItems(null, DateTime.Now.AddDays(weekInAdvance), false, 0);
            List<string> names = new List<string>();
            foreach (CalendarItem item in events)
            {
                names.Add($"{item.ShortDescription} - {item.StartDateTime}");
            }
            if (events.Count == 0)
                homeView.ShowNoUpcomingEvents("There are no upcoming events");
            else
                homeView.ShowUpcomingEvents(names);
        }

        #endregion

        #region Create Events
        public void PopulateCategoryDropdown()
        {
            Categories categories = model.categories;
            List<Category> categoryList = categories.List();
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
                if (createCategoryView != null)
                {
                    PopulateCategoryDropdown();
                }
            }

            
        }

       

        #endregion
    }
}
