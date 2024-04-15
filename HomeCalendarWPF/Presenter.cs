using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Calendar;


namespace HomeCalendarWPF
{
    public class Presenter
    {
        //private readonly ViewInterface view;

        private readonly IWelcomeViewInterface welcomeView;
        private readonly ICreateEventViewInterface createEventView;
        private readonly CategoryView categoryView;
        private readonly MainViewInterface mainView;
        private HomeCalendar model;
        public Presenter(IWelcomeViewInterface wv, CategoryView cv, ICreateEventViewInterface cev, MainViewInterface mv)
        {
            welcomeView = wv;
            createEventView = cev;
            categoryView = cv;
            mainView = mv;
        }

        #region Welcome Page

        public void NewCalendar(string location, string name)
        {
            model = new HomeCalendar($"{location}\\{name}", true);
            Home home = new Home(this);
        }

        public void ExistingCalendar(string location)
        {
            model = new HomeCalendar($"{location}");
            Home home = new Home(this); 
        }

        #endregion

        #region Home Page

        public void PopulateEventsList()
        {   
            List<Events> events = new List<Events>();

            mainView.ShowUpcomingEvents();
        }

        #endregion

        #region Create Events
        public void PopulateCategoriesMenu()
        {
            Categories categories = model.categories;
            List<Category> categoryList = categories.List();
            foreach (Category category in categoryList)
            {
                createEventView.AddCategoryToMenu(category.Description);
            }
        }

        #endregion

        #region Create Category

        #endregion
    }
}
