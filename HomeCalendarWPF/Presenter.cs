using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
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
        private CategoryView categoryView;
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
                categoryView = window as CategoryView;
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

        public List<CalendarItem> GetUpcomingEvents()
        {
            List<CalendarItem> events = model.GetCalendarItems(null, DateTime.Now.AddDays(7), false, 0);
            if (events.Count == 0)
                mainView.ShowNoUpcomingEvents("There are no upcoming events");
            else
                mainView.ShowNoUpcomingEvents("");
            return events;  
        }

        #endregion

        #region Create Events
        public List<Category> PopulateCategoriesMenu()
        {
            Categories categories = model.categories;
            List<Category> categoryList = categories.List();
            return categoryList;
        }

        #endregion

        #region Create Category

        #endregion
    }
}
