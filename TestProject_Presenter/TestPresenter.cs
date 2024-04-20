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

        public int GetEventCategory()
        {
            calledGetEventCategory = true;
        }

        public string GetEventDetails()
        {
            throw new NotImplementedException();
        }

        public double GetEventDurationInMinutes()
        {
            throw new NotImplementedException();
        }

        public DateTime GetEventStartTime()
        {
            throw new NotImplementedException();
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

        //RegisterWindow 
        //Test MainViewInterface window
        //Test CategoryView window
        //Test ICreateEventViewInterface

        [Fact]
        public void RegisterMainViewWindow()
        {
            MainWindow window = new MainWindow();
            Presenter _presenter = new Presenter(window);


            _presenter.RegisterWindow(window);

            Assert.True(window is MainViewInterface);
        }

        [Fact]
        public void RegisterAddCategoryWindow()
        {
            //ARRANGE
            MainWindow window = new MainWindow();
            Presenter _presenter = new Presenter(window);

            AddCategory categoryWindow = new AddCategory(_presenter);

            //ASSERT
            _presenter.RegisterWindow(categoryWindow);

            //ACT
            Assert.True(categoryWindow is CategoryView);
        }

        [Fact]
        public void RegisterAddEventWindow()
        {
            //ARRANGE
            MainWindow window = new MainWindow();
            Presenter _presenter = new Presenter(window);

            AddEvent eventWindow = new AddEvent(_presenter);

            //ASSERT
            _presenter.RegisterWindow(eventWindow);

            //ACT
            Assert.True(eventWindow is ICreateEventViewInterface);
        }


        //NewCalendar
        //Test creation with proper params
        //Test to verify Home window is being displayed


        //ExistingCalendar
        //opening of the calendar
        //Verify that Home window is being displayed


        //GetUpcomingEvents
        //no upcoming events
        //with some upcoming events
        //correct passing of events to mainview


        //PopulateCategoryDropdown
        //ensure valid population of dropdown with categories


        //ValidateEventFormAndCreate
        //Test each params for valid creation
        //No details 
        //Invalid duration
        //No start time 
        //No category
        //ensure a succesful event creation
        //ensure failed creation due to invalid params^


        //PopulateCategoryTypesDropdown
        //Ensure proper categroy types in dropdown.


        //ValidateDetailsFormInputAndCreateCategory
        //No details
        //invalid type
        //all valid types (4 of them iykyk)
        //creation of category is successful
        //ensuring the call to PopulateCategoryDropdown once created
    }
}
