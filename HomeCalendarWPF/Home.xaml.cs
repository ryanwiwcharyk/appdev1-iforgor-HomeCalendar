using Calendar;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HomeCalendarWPF
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Window, HomeInterface
    {
        readonly Presenter _presenter;
        public Home(Presenter presenter)
        {
            InitializeComponent();
            _presenter = presenter;
            _presenter.RegisterWindow(this);
            _presenter.GetUpcomingEvents();
            _presenter.PopulateCategoryDropdownInHomePage();
        }

        //private methods for button clicks
        private void AddEventBtn_Click(object sender, RoutedEventArgs e)
        {
            AddEvent newAddEvent = new AddEvent(_presenter);
            newAddEvent.ShowDialog();
        }

        private void AddCategoryBtn_Click(object sender, RoutedEventArgs e)
        {
            AddCategory addNewCategory = new AddCategory(_presenter);
            addNewCategory.ShowDialog();
        }

        private void BtnClick_CloseApplication(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        public void AddCategoriesToDropdown(List<Category> categories)
        {
            categoryComboBox.ItemsSource = categories;
        }

        public void FilterCateory_Btn(object sender, RoutedEventArgs e)
        {
            _presenter.ValidateFilterToggleByCategory((bool)FilterCategory.IsChecked, categoryComboBox.Text);

        }

        // Interface implementation

        public void ShowUpcomingEvents(List<CalendarItem> upcomingEvents)
        {
            UpcomingEvents.ItemsSource = upcomingEvents;
            ObservableCollection<DataGridColumn> columns = UpcomingEvents.Columns;
            foreach (DataGridColumn column in columns)
            {
                if ((string)column.Header == "Month" || (string)column.Header == "Total Busy Time")
                {
                    column.Visibility = Visibility.Hidden;
                }
                else
                {
                    column.Visibility = Visibility.Visible;
                }
            }
        }

        public void ShowNoUpcomingEvents(string message)
        {

        }

        public void ShowUpcomingEventsByCategory(List<CalendarItemsByCategory> items)
        {
            UpcomingEvents.ItemsSource = items;
            ObservableCollection<DataGridColumn> columns = UpcomingEvents.Columns;
            foreach (DataGridColumn column in columns)
            {
                if ((string)column.Header != "Category" && (string)column.Header != "Total Busy Time")
                {
                    column.Visibility = Visibility.Hidden;
                }
                else
                {
                    column.Visibility = Visibility.Visible;
                }
            }
        }

        public void ShowUpcomingEventsByMonth(List<CalendarItemsByMonth> items)
        {
            UpcomingEvents.ItemsSource = items;
            ObservableCollection<DataGridColumn> columns = UpcomingEvents.Columns;
            foreach (DataGridColumn column in columns)
            {
                if ((string)column.Header != "Month" && (string)column.Header != "Total Busy Time")
                {
                    column.Visibility = Visibility.Hidden;
                }
                else
                {
                    column.Visibility = Visibility.Visible;
                }
            }
        }
        public void ShowUpcomingEventsByMonthAndCategory()
        {

        }


        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        { 
           _presenter.GetEventsFilteredByDate(startDatePicker.SelectedDate, endDatePicker.SelectedDate);
            
        }


        private void filterByCategory_Checked(object sender, RoutedEventArgs e)
        {
            
            DateTime? startDate = startDatePicker.SelectedDate;
            DateTime? endDate = endDatePicker.SelectedDate;
            //add varaible to get filter by category once implemented
            _presenter.GetEventsSortedByCategory(startDate, endDate);
        }

        private void filterByCategory_Unchecked(object sender, RoutedEventArgs e)
        {
            _presenter.GetEventsFilteredByDate(startDatePicker.SelectedDate, endDatePicker.SelectedDate);
        }

        private void filterByMonth_Checked(object sender, RoutedEventArgs e)
        {

            DateTime? startDate = startDatePicker.SelectedDate;
            DateTime? endDate = endDatePicker.SelectedDate;

            _presenter.GetEventsSortedByMonth(startDate, endDate);
        }

        private void filterByMonth_Unchecked(object sender, RoutedEventArgs e)
        {
            _presenter.GetEventsFilteredByDate(startDatePicker.SelectedDate, endDatePicker.SelectedDate);
        }
    }
}
