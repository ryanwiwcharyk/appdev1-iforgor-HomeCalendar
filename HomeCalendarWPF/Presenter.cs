﻿using System;
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

        private readonly ICreateEventViewInterface createEventView;
        private readonly CategoryView categoryView;
        public MainViewInterface mainView;
        private HomeCalendar model;
        public Presenter(CategoryView cv, ICreateEventViewInterface cev, MainViewInterface mv)
        {
            createEventView = cev;
            categoryView = cv;
            mainView = mv;
        }

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
