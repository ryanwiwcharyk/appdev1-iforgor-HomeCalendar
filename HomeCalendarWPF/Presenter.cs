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
        private MainViewInterface mainView;
        private readonly MainWindow welcomeWindow;
        private HomeCalendar model;
        public Presenter(MainWindow window)
        {
            welcomeWindow = window;
        }

        #region Window Registration
        public void RegisterWindow(Window window)
        {
            if (window is MainViewInterface)
                mainView = window as MainViewInterface;
            else if (window is CategoryView)
            {

                createCategoryView = window as CategoryView;
                List<Category.CategoryType> allCategoryTypes = PopulateCategoryTypesDropdown();
                createCategoryView.FillDropDown(allCategoryTypes);
            }
            else if (window is ICreateEventViewInterface)
                createEventView = window as ICreateEventViewInterface;
            else
                throw new Exception($"{window} was not able to be cast as a valid window type.");
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
            List<CalendarItem> events = model.GetCalendarItems(null, DateTime.Now.AddDays(7), false, 0);
            List<string> names = new List<string>();
            foreach (CalendarItem item in events)
            {
                names.Add($"{item.ShortDescription} - {item.StartDateTime}");
            }
            if (events.Count == 0)
                mainView.ShowNoUpcomingEvents("There are no upcoming events");
            else
                mainView.ShowUpcomingEvents(names);
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
                createEventView.ShowErrorPopup("No details");
            }
            else if (!double.TryParse(duration, out double validDurationAsDouble))
            {
                createEventView.ShowErrorPopup("No duration");
            }
            else if (!startTime.HasValue) //from https://stackoverflow.com/questions/41447490/how-do-i-get-value-from-datepickerwpf-in-c
            {
                createEventView.ShowErrorPopup("No start time");
            }
            else if (string.IsNullOrEmpty(selectedCategory))
            {
                createEventView.ShowErrorPopup("No category");
            }
            else
            {
                List<Category> categories = model.categories.List();
                Category category = categories.Find(x => x.Description == selectedCategory);
                
                if (category != null && startTime != null && validDurationAsDouble>0)
                {
                    model.events.Add((DateTime)startTime, category.Id, validDurationAsDouble, details);
                    createEventView.ShowSuccessPopup("yipee");
                    GetUpcomingEvents();
                    
                }
                else
                {
                    createEventView.ShowErrorPopup("bad");
                }

            }
        }

        #endregion

        #region Create Category

        //populate the category Types combo box

        //add category based on details + category type with validation

        public List<Category.CategoryType> PopulateCategoryTypesDropdown()
        {
            List<Category.CategoryType> categoryTypes = new List<Category.CategoryType>();

            Category.CategoryType[] catTypes = (Category.CategoryType[])Enum.GetValues(typeof(Category.CategoryType));
            foreach (Category.CategoryType catType in catTypes)
            {
                categoryTypes.Add(catType);
            }

            return categoryTypes;
        }

        public void ValidateDetailsFormInputAndCreateCategory(string details, int type) //called on the add button event handler.
        {
            const int MAX = 3;
            const int MIN = 1;

            if (string.IsNullOrEmpty(details))
            {
                createCategoryView.ShowWarning("Error, details cannot be empty. Please enter the name of the details of the category");
            }
            else if (type > MAX || type < MIN)
            {
                createCategoryView.ShowWarning("Error, Please select the category type for your new category");
            }
            else
            {

                model.categories.Add(details, (Category.CategoryType)type);

            }

            
        }

       

        #endregion
    }
}
