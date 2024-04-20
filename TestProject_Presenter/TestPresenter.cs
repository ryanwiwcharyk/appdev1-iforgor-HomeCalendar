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
            public bool calledExistingDatabase = false;
            public bool calledNewDatabase = false;
            public void ExistingDatabase(string location)
            {
                calledExistingDatabase = true;
            }

            public void NewDatabase(string location, string name)
            {
                calledNewDatabase = true;
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
            #region HomePage Tests
            [Fact]
            public void TestRegisterHomeView()
            {
                //Im not sure how to setup tests from db input
                TestDatabaseView databaseView = new TestDatabaseView();
                //var presenter = new Presenter(databaseView);
            }

            [Fact]
            public void TestRegisterCreateCategoryView()
            {

            }

            [Fact]
            public void TestRegisterCreateEventView()
            {

            }

            #endregion

            #region Create Events Tests

            #endregion

            #region Create Category Tests

            #endregion
        }
    }
}
