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

    namespace TestProject_Presenter
    {

        public class TestDatabaseView : DatabaseViewInterface
        {
            public bool calledConnectToDb = false;

            public void ConnectToDb(string location, string name)
            {
                calledConnectToDb = true;
            }

        }

        public class TestHomeView : MainViewInterface
        {
            public bool calledCloseApplication = false;
            public bool calledShowNoUpcomingEvents = false;
            public bool calledShowRecentFiles = false;
            public bool calledShowUpcomingEvents = false;
            public int upcomingEventsCount = 0;

            public void CloseApplication()
            {
                calledCloseApplication = true;
            }

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
            public bool calledGetEventCategory = false;
            public bool calledGetEventDetails = false;
            public bool calledGetEventDurationInMinutes = false;
            public bool calledGetEventStartTime = false;
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
                throw new NotImplementedException();
            }

            public void ShowSuccessPopup(string message)
            {
                throw new NotImplementedException();
            }
        }

        public class UnitTests
        {
            #region Database Selection Tests
            [Fact]
            public void TestConnectingToExistingOrNewDatabase()
            {
                TestDatabaseView databaseView = new TestDatabaseView();
                Presenter presenter = new Presenter(databaseView);
                string fileName = "testDBInput.db";
                string filePath = Directory.GetCurrentDirectory();


                presenter.ConnectingToExistingOrNewDatabase(filePath, fileName); //wplease let me know about the implementation.


                Assert.True(databaseView.calledConnectToDb);
            }

            #endregion

            #region HomePage Tests
            [Fact]
            public void TestRegisterHomeView()
            {
                TestDatabaseView databaseView = new TestDatabaseView();
                Presenter presenter = new Presenter(databaseView);
                TestHomeView homeView = new TestHomeView();
                string filePath = Directory.GetCurrentDirectory();
                string fileName = "testDBInput.db";
                presenter.ConnectingToExistingOrNewDatabase(filePath, fileName); //wplease let me know about the implementation.
                homeView.calledShowUpcomingEvents = false;


                presenter.RegisterWindow(homeView);

                Assert.True(homeView.calledShowUpcomingEvents);
            }

            [Fact]
            public void TestRegisterCreateCategoryView()
            {
                TestDatabaseView databaseView = new TestDatabaseView();
                Presenter presenter = new Presenter(databaseView);
                TestAddCategoryView categoryView = new TestAddCategoryView();
                string filePath = Directory.GetCurrentDirectory();
                string fileName = "testDBInput.db";
                presenter.ConnectingToExistingOrNewDatabase(filePath, fileName); //wplease let me know about the implementation.
                categoryView.calledFillDropDown = false;


                presenter.RegisterWindow(categoryView);

                Assert.True(categoryView.calledFillDropDown);
            }

            [Fact]
            public void TestRegisterCreateEventView()
            {
                TestDatabaseView databaseView = new TestDatabaseView();
                Presenter presenter = new Presenter(databaseView);
                TestAddCategoryView eventView = new TestAddCategoryView();
                string filePath = Directory.GetCurrentDirectory();
                string fileName = "testDBInput.db";
                presenter.ConnectingToExistingOrNewDatabase(filePath, fileName); //wplease let me know about the implementation.
                eventView.calledFillDropDown = false;


                presenter.RegisterWindow(eventView);

                Assert.True(eventView.calledFillDropDown);
            }

            #endregion

            #region Create Events Tests

            #endregion

            #region Create Category Tests

            #endregion
        }
    }
}
