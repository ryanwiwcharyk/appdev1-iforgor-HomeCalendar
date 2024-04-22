﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject_Presenter
{
    using HomeCalendarWPF;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using Calendar;

    namespace TestProject_Presenter
    {

        public class TestMainView : MainViewInterface
        {
            public bool calledNewCalendar = false;
            public bool calledExistingCalendar = false;
            public void NewCalendar()
            {
                calledNewCalendar = true;
            }
            public void ExistingCalendar()
            {
                calledExistingCalendar = true;
            }

        }

        public class TestHomeView : HomeInterface
        {
            public bool calledShowNoUpcomingEvents = false;
            public bool calledShowRecentFiles = false;
            public bool calledShowUpcomingEvents = false;
            public int upcomingEventsCount = 0;

            public void ShowNoUpcomingEvents(string message)
            {
                calledShowNoUpcomingEvents = true;
            }

            public void ShowRecentFiles()
            {
                //not implemented yet (method in window)
                calledShowRecentFiles = true;
            }

            public void ShowUpcomingEvents(List<string> upcomingEvents)
            {
                calledShowUpcomingEvents = true;
                upcomingEventsCount = upcomingEvents.Count();
            }
        }

        public class TestAddCategoryView : CategoryView
        {
            public bool calledFillDropDown = false;
            public bool calledShowSuccessPopup = false;
            public bool calledShowWarningPopup = false;
            public int dropDownListCount = 0;
            public void FillDropDown(List<Category.CategoryType> types)
            {
                calledFillDropDown = true;
                dropDownListCount = types.Count();
            }

            public void ShowSuccessPopup(string message)
            {
                calledShowSuccessPopup = true;
            }

            public void ShowWarning(string warning)
            {
                calledShowWarningPopup = true;
            }
        }

        public class TestAddEventView : ICreateEventViewInterface
        {
            public bool calledAddCategoriesToDropdown = false;
            public bool calledShowErrorPopup = false;
            public bool calledShowSuccessPopup = false;
            public int categoryDropdownCount = 0;

            public void AddCategoriesToDropdown(List<Category> categories)
            {
                calledAddCategoriesToDropdown = true;
                categoryDropdownCount = categories.Count();
            }

            public void ShowErrorPopup(string message)
            {
                calledShowErrorPopup = true;
            }

            public void ShowSuccessPopup(string message)
            {
                calledShowSuccessPopup = true;
            }
        }

        public class UnitTests
        {

            // Testing constructor
            [StaFact]
            public void TestConstructor()
            {
                TestMainView view = new TestMainView();

                Presenter p = new Presenter(view);

                Assert.IsType<Presenter>(p);
            }

            // Testing Window registration

            [StaFact]
            public void ConstructorRegistersWelcomeWindow()
            {
                TestMainView view = new TestMainView();

                Presenter p = new Presenter(view);

                Assert.NotNull(p.MainViewInterface);
            }

            [StaFact]
            public void RegisterEventWindow()
            {
                TestAddEventView testAddEventView = new TestAddEventView();
                TestMainView view = new TestMainView();


                Presenter p = new Presenter(view);
                p.RegisterWindow( testAddEventView );

                Assert.NotNull(p.createEventViewInterface);

            }

            [StaFact]
            public void RegisterHomeWindow()
            {
                TestMainView view = new TestMainView();
                TestHomeView homeView = new TestHomeView();

                Presenter p = new Presenter(view);
                p.RegisterWindow(homeView);

                Assert.NotNull(p.HomeInterface);
            }

            [StaFact]
            public void RegisterCategoryWindow()
            {
                TestMainView view = new TestMainView();
                TestAddCategoryView addCategoryView = new TestAddCategoryView();

                Presenter p = new Presenter(view);
                p.RegisterWindow(addCategoryView);

                Assert.NotNull(p.CategoryView);
            }

            // Testing Home window

            [StaFact]
            public void NewCalendarCreatesEmptyCalendar()
            {
                TestMainView view = new TestMainView();
                Presenter p = new Presenter(view);
                string path = Directory.GetCurrentDirectory();
                string name = "testNewCalendar";
                p.NewCalendar(path, name);
                TestHomeView testHomeView = new TestHomeView();
                p.RegisterWindow(testHomeView);

                testHomeView.calledShowNoUpcomingEvents = false;
                p.GetUpcomingEvents();

                Assert.True(testHomeView.upcomingEventsCount == 0);
                Assert.True(testHomeView.calledShowNoUpcomingEvents);

            }
            [StaFact]
            public void NewCalendarNewEventUpcomingEventsUpdates()
            {
                TestMainView view = new TestMainView();
                Presenter p = new Presenter(view);
                TestHomeView testHomeView = new TestHomeView();
                TestAddEventView testAddEventView = new TestAddEventView();

                string path = Directory.GetCurrentDirectory();
                string name = "testNewCalendar";
                string details = "wowie";
                string duration = "30";
                DateTime startTime = DateTime.Now;
                string cat = "Sleep";

                p.NewCalendar(path, name);
                p.RegisterWindow(testAddEventView);
                p.RegisterWindow(testHomeView);

                testHomeView.upcomingEventsCount = 0;
                testHomeView.calledShowUpcomingEvents = false;

                p.ValidateEventFormInputAndCreate(details, duration, startTime, cat);

                Assert.Equal(1, testHomeView.upcomingEventsCount);
                Assert.True(testHomeView.calledShowUpcomingEvents);

            }

            // Testing Event window
            [StaFact]
            public void ExistingCalendarIsOpenedAndShowsUpcomingEvents()
            {
                TestMainView view = new TestMainView();
                Presenter p = new Presenter(view);
                TestAddEventView addEventView = new TestAddEventView();
                p.RegisterWindow(addEventView);
                string path = Directory.GetCurrentDirectory();
                string name = "testNewCalendar";
                string details = "new event";
                string duration = "30";
                DateTime startTime = DateTime.Now;
                string cat = "School";
                p.NewCalendar(path, name);
                p.ValidateEventFormInputAndCreate(details, duration, startTime, cat);

                p.ExistingCalendar($"{path}\\{name}");
                TestHomeView testHomeView = new TestHomeView();
                p.RegisterWindow(testHomeView);
                testHomeView.calledShowUpcomingEvents = false;

                p.GetUpcomingEvents();

                Assert.True(testHomeView.upcomingEventsCount == 1);
                Assert.True(testHomeView.calledShowUpcomingEvents);

            }
            [StaFact]
            public void TestPopulateCategoryDropdown()
            {
                TestMainView view = new TestMainView();
                Presenter p = new Presenter(view);
                TestAddEventView testAddEventView = new TestAddEventView();
                p.RegisterWindow( testAddEventView);
                string path = Directory.GetCurrentDirectory();
                string name = "testNewCalendar";
                p.NewCalendar(path, name);

                testAddEventView.calledAddCategoriesToDropdown = false;
                p.PopulateCategoryDropdown();

                Assert.True(testAddEventView.calledAddCategoriesToDropdown);
                Assert.True(testAddEventView.categoryDropdownCount != 0);

            }

            [StaFact]
            public void TestCreateInputNoDetailsProvided()
            {
                TestMainView view = new TestMainView();
                Presenter p = new Presenter(view);
                TestAddEventView testAddEventView = new TestAddEventView();
                p.RegisterWindow(testAddEventView);
                string path = Directory.GetCurrentDirectory();
                string name = "testNewCalendar";
                p.NewCalendar(path, name);;
                string details = "";
                string duration = "30";
                DateTime startTime = DateTime.Now;
                string cat = "Sleep";
                testAddEventView.calledShowErrorPopup = false;

                p.ValidateEventFormInputAndCreate(details,duration, startTime, cat);

                Assert.True(testAddEventView.calledShowErrorPopup);
            }

            [StaFact]
            public void TestCreateInputInvalidDurationProvided()
            {
                TestMainView view = new TestMainView();
                Presenter p = new Presenter(view);
                TestAddEventView testAddEventView = new TestAddEventView();
                p.RegisterWindow(testAddEventView);
                string path = Directory.GetCurrentDirectory();
                string name = "testNewCalendar";
                p.NewCalendar(path, name); ;
                string details = "wowie";
                string duration = "duration";
                DateTime startTime = DateTime.Now;
                string cat = "Sleep";
                testAddEventView.calledShowErrorPopup = false;

                p.ValidateEventFormInputAndCreate(details, duration, startTime, cat);

                Assert.True(testAddEventView.calledShowErrorPopup);
            }

            [StaFact]
            public void TestCreateInputNegativeDurationProvided() 
            {
                TestMainView view = new TestMainView();
                Presenter p = new Presenter(view);
                TestAddEventView testAddEventView = new TestAddEventView();
                p.RegisterWindow(testAddEventView);
                string path = Directory.GetCurrentDirectory();
                string name = "testNewCalendar";
                p.NewCalendar(path, name); ;
                string details = "wowie";
                string duration = "-30";
                DateTime startTime = DateTime.Now;
                string cat = "Sleep";
                testAddEventView.calledShowErrorPopup = false;

                p.ValidateEventFormInputAndCreate(details, duration, startTime, cat);

                Assert.True(testAddEventView.calledShowErrorPopup);
            }

            [StaFact]
            public void TestCreateInputNoStartTimeProvided()
            {
                TestMainView view = new TestMainView();
                Presenter p = new Presenter(view);
                TestAddEventView testAddEventView = new TestAddEventView();
                p.RegisterWindow(testAddEventView);
                string path = Directory.GetCurrentDirectory();
                string name = "testNewCalendar";
                p.NewCalendar(path, name); ;
                string details = "wowie";
                string duration = "30";
                DateTime? startTime = null;
                string cat = "Sleep";
                testAddEventView.calledShowErrorPopup = false;

                p.ValidateEventFormInputAndCreate(details, duration, startTime, cat);

                Assert.True(testAddEventView.calledShowErrorPopup);
            }

            [StaFact]
            public void TestCreateInputNoCategorySelected()
            {
                TestMainView view = new TestMainView();
                Presenter p = new Presenter(view);
                TestAddEventView testAddEventView = new TestAddEventView();
                p.RegisterWindow(testAddEventView);
                string path = Directory.GetCurrentDirectory();
                string name = "testNewCalendar";
                p.NewCalendar(path, name); ;
                string details = "wowie";
                string duration = "30";
                DateTime? startTime = DateTime.Now;
                string? cat = null;
                testAddEventView.calledShowErrorPopup = false;

                p.ValidateEventFormInputAndCreate(details, duration, startTime, cat);

                Assert.True(testAddEventView.calledShowErrorPopup);
            }

            [StaFact]
            public void TestCreateInputAllDataValid()
            {
                TestMainView view = new TestMainView();
                Presenter p = new Presenter(view);
                TestAddEventView testAddEventView = new TestAddEventView();
                p.RegisterWindow(testAddEventView);
                string path = Directory.GetCurrentDirectory();
                string name = "testNewCalendar";
                p.NewCalendar(path, name);
                string details = "wowie";
                string duration = "30";
                DateTime? startTime = DateTime.Now;
                string? cat = "Sleep";
                testAddEventView.calledShowSuccessPopup = false;

                p.ValidateEventFormInputAndCreate(details, duration, startTime, cat);

                Assert.True(testAddEventView.calledShowSuccessPopup);
            }

            // Testing Category window

            [StaFact]
            public void CategoryTypeDropDown_GetsFilled()
            {
                const int CATEGORY_TYPES_NB = 4;
                TestMainView view = new TestMainView();
                Presenter p = new Presenter(view);
                TestAddCategoryView addCategoryView = new TestAddCategoryView();
                p.RegisterWindow(addCategoryView);
                p.PopulateCategoryTypesDropdown();

                Assert.True(addCategoryView.calledFillDropDown);
                Assert.Equal(CATEGORY_TYPES_NB, addCategoryView.dropDownListCount);


            }
            [StaFact]
            public void TestCreateCategoryNoDetailsProvided()
            {
                TestMainView view = new TestMainView();
                Presenter p = new Presenter(view);
                TestAddCategoryView testAddCategoryView = new TestAddCategoryView();
                p.RegisterWindow(testAddCategoryView);
                string path = Directory.GetCurrentDirectory();
                string name = "testNewCalendar";
                p.NewCalendar(path, name); 
                string details = "";
                string type = "AllDayEvent";

                testAddCategoryView.calledShowWarningPopup = false;

                p.ValidateDetailsFormInputAndCreateCategory(details, type);

                Assert.True(testAddCategoryView.calledShowWarningPopup);
            }
            [StaFact]
            public void TestCreateCategoryUnvalidCategoryTypeProvided()
            {
                TestMainView view = new TestMainView();
                Presenter p = new Presenter(view);
                TestAddCategoryView testAddCategoryView = new TestAddCategoryView();
                p.RegisterWindow(testAddCategoryView);
                string path = Directory.GetCurrentDirectory();
                string name = "testNewCalendar";
                p.NewCalendar(path, name); ;
                string details = "details";
                string type = "UnvalidType";

                testAddCategoryView.calledShowWarningPopup = false;

                p.ValidateDetailsFormInputAndCreateCategory(details, type);

                Assert.True(testAddCategoryView.calledShowWarningPopup);
            }
            [StaFact]
            public void TestCreateCategoryAllValidProvided()
            {
                TestMainView view = new TestMainView();
                Presenter p = new Presenter(view);
                TestAddCategoryView testAddCategoryView = new TestAddCategoryView();
                p.RegisterWindow(testAddCategoryView);
                string path = Directory.GetCurrentDirectory();
                string name = "testNewCalendar";
                p.NewCalendar(path, name); ;
                string details = "details";
                string type = "AllDayEvent";

                testAddCategoryView.calledShowSuccessPopup = false;

                p.ValidateDetailsFormInputAndCreateCategory(details, type);

                Assert.True(testAddCategoryView.calledShowSuccessPopup);
            }
            [StaFact]
            public void TestCreateCategoryAndCreateEventWithCategory()
            {
                TestMainView view = new TestMainView();
                Presenter p = new Presenter(view);
                TestAddCategoryView testAddCategoryView = new TestAddCategoryView();
                TestAddEventView testAddEventView = new TestAddEventView();
                string path = Directory.GetCurrentDirectory();
                string name = "testNewCalendar";
                string details = "Tutoring";
                string type = "AllDayEvent";
                string eventDetails = "Tutoring Prog";
                string duration = "30";
                DateTime startTime = DateTime.Now;
                string cat = "Tutoring";

                p.RegisterWindow(testAddCategoryView);
                p.RegisterWindow(testAddEventView);
                p.NewCalendar(path, name);
                p.ValidateDetailsFormInputAndCreateCategory(details, type);
                testAddEventView.calledShowSuccessPopup = false;
                p.ValidateEventFormInputAndCreate(eventDetails, duration, startTime, cat);

                Assert.True(testAddEventView.calledShowSuccessPopup);
            }





        }
    }
}
