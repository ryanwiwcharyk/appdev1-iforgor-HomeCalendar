using System;
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
    using HomeCalendarWPF.interfaces;
    using static Calendar.Category;
    using System.Security.RightsManagement;

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
            public bool calledAddCategoriesToDropdown = false;
            public bool calledShowUpcomingEventsByCategory = false;
            public bool calledShowUpcomingEventsByMonth = false;
            public bool calledShowUpcomingEventsByMonthAndCategory = false;
            



            public void AddCategoriesToDropdown(List<Category> categories)
            {
                calledAddCategoriesToDropdown = true;
            }

            public void ShowNoUpcomingEvents(string message)
            {
                calledShowNoUpcomingEvents = true;
            }

            public void ShowNoUpcomingEvents()
            {
                calledShowNoUpcomingEvents = true;
            }

            public void ShowRecentFiles()
            {
                //not implemented yet (method in window)
                calledShowRecentFiles = true;
            }

            public void ShowUpcomingEvents(List<CalendarItem> upcomingEvents)
            {
                calledShowUpcomingEvents = true;
                upcomingEventsCount = upcomingEvents.Count();
            }


            public void ShowUpcomingEventsByCategory(List<CalendarItemsByCategory> upcomingEventsByCategory)
            {
                calledShowUpcomingEventsByCategory = true;
            }

            public void ShowUpcomingEventsByMonth(List<CalendarItemsByMonth> upcomingEventsByMonth)
            {
                calledShowUpcomingEventsByMonth = true;
            }

            public void ShowUpcomingEventsByMonthAndCategory(List<Dictionary<string, object>> items, List<Category> categories)
            {
                calledShowUpcomingEventsByMonthAndCategory = true;
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

        public class TestAddEventView : UpdateEventViewInterface
        {
            public bool calledAddCategoriesToDropdown = false;
            public bool calledShowErrorPopup = false;
            public bool calledShowSuccessPopup = false;
            public bool calledShowPopulatedFields = false;
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

            public void ShowPopulatedFields(string details, double duration, DateTime startDate, int hours, int minutes, string categoryDescription)
            {
                calledShowPopulatedFields = true;
            }

            public void ShowSuccessPopup(string message)
            {
                calledShowSuccessPopup = true;
            }
        }

        public class TestUpdateView : UpdateEventViewInterface
        {
            public bool calledAddCategoriesToDropdown = false;
            public bool calledShowErrorPopup = false;
            public bool calledShowPopulatedFields = false;
            public bool calledShowSuccessPopup = false;
            public int categoryDropdownCount = 0;

            public void AddCategoriesToDropdown(List<Category> cats)
            {
                calledAddCategoriesToDropdown = true;
                categoryDropdownCount = cats.Count();
            }

            public void ShowErrorPopup(string message)
            {
                calledShowErrorPopup = true;
            }

            public void ShowPopulatedFields(string details, double duration, DateTime startDate, int hours, int minutes, string categoryDescription)
            {
                calledShowPopulatedFields = true;
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

/*            [StaFact]
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

            }*/
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

                int id = 3;
                string description = "Work";
                CategoryType type = CategoryType.Event;
                Category selectedCat = new Category(id, description, type);

                p.NewCalendar(path, name);
                p.RegisterWindow(testAddEventView);
                p.RegisterWindow(testHomeView);

                testHomeView.upcomingEventsCount = 0;
                testHomeView.calledShowUpcomingEvents = false;

                p.ValidateEventFormInputAndCreate(details, duration, startTime, selectedCat);

                Assert.Equal(1, testHomeView.upcomingEventsCount);
                Assert.True(testHomeView.calledShowUpcomingEvents);

            }



            // Testing Event window


/*            [StaFact]
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

            }*/


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

                int id = 2;
                string description = "Work";
                CategoryType type = CategoryType.Event;
                Category selectedCat = new Category(id, description, type);

                testAddEventView.calledShowErrorPopup = false;

                p.ValidateEventFormInputAndCreate(details,duration, startTime, selectedCat);

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
                testAddEventView.calledShowErrorPopup = false;

                int id = 1;
                string description = "Sleep";
                CategoryType type = CategoryType.Event;
                Category selectedCat = new Category(id, description, type);

                p.ValidateEventFormInputAndCreate(details, duration, startTime, selectedCat);

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
                testAddEventView.calledShowErrorPopup = false;

                int id = 4;
                string description = "Sleep";
                CategoryType type = CategoryType.Event;
                Category selectedCat = new Category(id, description, type);

                p.ValidateEventFormInputAndCreate(details, duration, startTime, selectedCat);

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
                testAddEventView.calledShowErrorPopup = false;

                int id = 3;
                string description = "Sleep";
                CategoryType type = CategoryType.Event;
                Category selectedCat = new Category(id, description, type);

                p.ValidateEventFormInputAndCreate(details, duration, startTime, selectedCat);

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

                int id = 2;
                string description = "Work";
                CategoryType type = CategoryType.Event;
                Category selectedCat = new Category(id, description, type);

                p.ValidateEventFormInputAndCreate(details, duration, startTime, selectedCat);

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
                testAddEventView.calledShowSuccessPopup = false;

                int id = 3;
                string description = "Sleep";
                CategoryType type = CategoryType.Event;
                Category selectedCat = new Category(id, description, type);

                p.ValidateEventFormInputAndCreate(details, duration, startTime, selectedCat);

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

                int id = 3;
                string description = "Sleep";
                CategoryType catType = CategoryType.Event;
                Category selectedCat = new Category(id, description, catType);

                p.RegisterWindow(testAddCategoryView);
                p.RegisterWindow(testAddEventView);
                p.NewCalendar(path, name);
                p.ValidateDetailsFormInputAndCreateCategory(details, type);
                testAddEventView.calledShowSuccessPopup = false;
                p.ValidateEventFormInputAndCreate(eventDetails, duration, startTime, selectedCat);

                Assert.True(testAddEventView.calledShowSuccessPopup);
            }

            //Testing Update window

            [StaFact]
            public void TestPopulateCategoryDropdownForUpdate()
            {
                TestMainView view = new TestMainView();
                Presenter p = new Presenter(view);
                TestUpdateView testUpdateView = new TestUpdateView();
                string path = Directory.GetCurrentDirectory();
                string name = "testNewCalendar";
                p.NewCalendar(path, name);
                p.RegisterWindow(testUpdateView);

                p.PopulateCategoryDropdownForUpdate();

                Assert.True(testUpdateView.calledAddCategoriesToDropdown);
            }

            [StaFact]
            public void TestValidateEventFormInputAndUpdate()
            {
                TestMainView view = new TestMainView();
                Presenter p = new Presenter(view);
                TestUpdateView testUpdateView = new TestUpdateView();
                string path = Directory.GetCurrentDirectory();
                string name = "testNewCalendar";
                int eventId = 2;
                string details = "Walk";
                string duration = "30";
                DateTime? startTime = DateTime.Now;

                int id = 3;
                string description = "Work";
                CategoryType type = CategoryType.Event;
                Category selectedCat = new Category(id, description, type);

                p.NewCalendar(path, name);
                p.RegisterWindow(testUpdateView);

                p.ValidateEventFormInputAndUpdate(eventId, details, duration, startTime, selectedCat);

                Assert.True(testUpdateView.calledShowSuccessPopup);
            }

            [StaFact]
            public void TestPopulateCategoryDropdownInHomePage()
            {
                TestHomeView homeView = new TestHomeView();
                TestMainView mainView = new TestMainView();
                Presenter p = new Presenter(mainView);
                p.RegisterWindow(homeView);

                p.PopulateCategoryDropdownInHomePage();

                Assert.True(homeView.calledAddCategoriesToDropdown);
            }

            [StaFact]
            public void TestGetEventsFilteredByDate()
            {
                TestHomeView homeView = new TestHomeView();
                TestMainView mainView = new TestMainView();
                Presenter p = new Presenter(mainView);
                p.RegisterWindow(homeView);

                DateTime now = DateTime.Now;
                DateTime later = DateTime.Now.AddMinutes(1); //assuming theres no events within this period

                p.GetEventsFilteredByDate(now, later);

                Assert.True(homeView.calledShowNoUpcomingEvents);
            }

            [StaFact]
            public void TestViewSelectorSummaryByMonthFilterByCategory()
            {
                TestHomeView homeView = new TestHomeView();
                TestMainView mainView = new TestMainView();
                Presenter p = new Presenter(mainView);
                p.RegisterWindow(homeView);

                int id = 1;
                string description = "Sleep";
                CategoryType type = CategoryType.Event;
                Category selectedCat = new Category(id, description, type);

                DateTime now = DateTime.Now;
                DateTime later = DateTime.Now.AddDays(20);

                p.ViewSelector(summaryByMonthChecked: true, summaryByCategoryChecked: false, filterByCategoryChecked: true, selectedCat, now, later);

                Assert.True(homeView.calledShowUpcomingEventsByMonth);
            }

            [StaFact]
            public void TestDeleteEventContextMenu()
            {
                TestHomeView homeView = new TestHomeView();
                TestMainView mainView = new TestMainView();
                Presenter p = new Presenter(mainView);
                p.RegisterWindow(homeView);

                CalendarItem newItem = new CalendarItem { EventID = 4, StartDateTime = DateTime.Now, CategoryID = 3 };

                p.DeleteEvent(newItem);

                Assert.True(homeView.calledShowNoUpcomingEvents);
            }

            [StaFact]
            public void TestPopulateUpdateEventFields()
            {
                TestUpdateView updateView = new TestUpdateView();
                TestMainView mainView = new TestMainView();
                Presenter p = new Presenter(mainView);
                p.RegisterWindow(updateView);

                CalendarItem newItem = new CalendarItem { EventID = 1, StartDateTime = DateTime.Now, CategoryID = 2 };

                p.PopulateUpdateEventFields(newItem);

                Assert.True(updateView.calledShowPopulatedFields);
            }






        }
    }
}
