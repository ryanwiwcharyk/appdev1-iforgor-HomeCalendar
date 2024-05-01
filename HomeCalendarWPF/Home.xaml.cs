using Calendar;
using Microsoft.VisualBasic.ApplicationServices;
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
using System.Windows.Forms;
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
            System.Windows.Application.Current.Shutdown();
        }

        public void AddCategoriesToDropdown(List<Category> categories)
        {
            categoryComboBox.ItemsSource = categories;
        }

        public void FilterCategory_Checked(object sender, RoutedEventArgs e)
        {
            _presenter.ValidateFilterToggleByCategory((Category)categoryComboBox.SelectedItem);

        }
        public void FilterCategory_Unchecked(object sender, RoutedEventArgs e)
        {
            _presenter.GetUpcomingEvents();

        }
        private void categoryComboBox_SelectionChange(object sender, RoutedEventArgs e)
        {
            if ((bool)FilterCategory.IsChecked)
            {
                _presenter.ValidateFilterToggleByCategory((Category)categoryComboBox.SelectedItem);
            }

        }


        // Interface implementation

        public void ShowUpcomingEvents(List<CalendarItem> upcomingEvents)
        {
            UpcomingEvents.ItemsSource = upcomingEvents;
            UpcomingEvents.Columns.Clear();
            var column = new DataGridTextColumn();
            column.Header = "Start Date";
            column.Binding = new System.Windows.Data.Binding("StartDateTime");
            var column2 = new DataGridTextColumn();
            column2.Header = "Total Busy Time";
            column2.Binding = new System.Windows.Data.Binding("TotalBusyTime"); // need to format 
            UpcomingEvents.Columns.Add(column);
            UpcomingEvents.Columns.Add(column2);
        }

        public void ShowNoUpcomingEvents(string message)
        {

        }

        public void ShowUpcomingEventsByCategory(List<CalendarItemsByCategory> items)
        {
            UpcomingEvents.ItemsSource = items;
            UpcomingEvents.Columns.Clear();
            var column = new DataGridTextColumn();
            column.Header = "Category";
            column.Binding = new System.Windows.Data.Binding("Category");
            var column2 = new DataGridTextColumn();
            column2.Header = "Total Busy Time";
            column2.Binding = new System.Windows.Data.Binding("TotalBusyTime"); // need to format 
            UpcomingEvents.Columns.Add(column);
            UpcomingEvents.Columns.Add(column2);

        }

        public void ShowUpcomingEventsByMonth(List<CalendarItemsByMonth> items)
        {

            UpcomingEvents.ItemsSource = items;
            UpcomingEvents.Columns.Clear();                      
            var column = new DataGridTextColumn();    
            column.Header = "Month";
            column.Binding = new System.Windows.Data.Binding("Month");         
            var column2 = new DataGridTextColumn(); 
            column2.Header = "Total Busy Time";
            column2.Binding = new System.Windows.Data.Binding("TotalBusyTime");          
            UpcomingEvents.Columns.Add(column);
            UpcomingEvents.Columns.Add(column2);

        }
        public void ShowUpcomingEventsByMonthAndCategory(List<Dictionary<string, object>> items, List<Category> categories)
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

            List<string> addedKeys = new List<string>();

            // get list of column name from first dictionary in the list
            // and create column and bind to dictionary element
            foreach (Dictionary<string, object> item in items)
            {
                foreach (string key in item.Keys)
                {

                    if (key.Contains("items"))
                    {

                    }
                    else if (addedKeys.Contains(key))
                    {

                    }
                    else
                    {

                        
                        var column = new DataGridTextColumn();
                        column.Header = key;
                        column.Binding = new System.Windows.Data.Binding($"[{key}]"); // Notice the square brackets!
                        UpcomingEvents.Columns.Add(column);
                        addedKeys.Add(key);
                    }

                }
            }

        }


        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        { 
           _presenter.GetEventsFilteredByDate(startDatePicker.SelectedDate, endDatePicker.SelectedDate);
        }


        private void summaryByCategory_Checked(object sender, RoutedEventArgs e)
        {
            DateTime? startDate = startDatePicker.SelectedDate;
            DateTime? endDate = endDatePicker.SelectedDate;
            //add varaible to get filter by category once implemented
            
            if ((bool)summaryByMonth.IsChecked)
                _presenter.GetEventsByMonthAndCategory(startDate, endDate);
            else
                _presenter.GetEventsSortedByCategory(startDate, endDate);
        }

        private void summaryByCategory_Unchecked(object sender, RoutedEventArgs e)
        {
            if ((bool)summaryByMonth.IsChecked)
                _presenter.GetEventsSortedByMonth(startDatePicker.SelectedDate, endDatePicker.SelectedDate);
            else
                _presenter.GetEventsFilteredByDate(startDatePicker.SelectedDate, endDatePicker.SelectedDate);
        }

        private void summaryByMonth_Checked(object sender, RoutedEventArgs e)
        {
            DateTime? startDate = startDatePicker.SelectedDate;
            DateTime? endDate = endDatePicker.SelectedDate;

            if ((bool)summaryByCategory.IsChecked)
                _presenter.GetEventsByMonthAndCategory(startDate, endDate);
            else
                _presenter.GetEventsSortedByMonth(startDate, endDate);
        }

        private void summaryByMonth_Unchecked(object sender, RoutedEventArgs e)
        {
            if ((bool)summaryByCategory.IsChecked)
                _presenter.GetEventsSortedByCategory(startDatePicker.SelectedDate, endDatePicker.SelectedDate);
            else
                _presenter.GetEventsFilteredByDate(startDatePicker.SelectedDate, endDatePicker.SelectedDate);
        }

        private void categoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
