﻿using Calendar;
using HomeCalendarWPF.interfaces;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace HomeCalendarWPF
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : System.Windows.Window, HomeInterface
    {
        readonly Presenter _presenter;
        public Home(Presenter presenter)
        {
            InitializeComponent();
            _presenter = presenter;
            InitializeWindow();
        }

        private void InitializeWindow()
        {
            _presenter.RegisterWindow(this);
            _presenter.GetEventsFilteredByDate(startDatePicker.SelectedDate, endDatePicker.SelectedDate);
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


        private void BtnClick_Search(object sender, RoutedEventArgs e)
        {
            string searchedString = SearchBar.Text;
            var items = UpcomingEvents.ItemsSource as List<CalendarItem>;
            int selectedIndex = UpcomingEvents.SelectedIndex;
            _presenter.SearchFilteredEvents(searchedString, items, selectedIndex);
        }


        private void MenuItemEdit_Click(object sender, RoutedEventArgs e)
        {
            CalendarItem selected = UpcomingEvents.SelectedItem as CalendarItem;
            UpdateEventWindow update = new UpdateEventWindow(_presenter, selected);
            update.ShowDialog();
        }

        private void MenuItemDelete_Click(object sender, RoutedEventArgs e)
        {
            CalendarItem selected = UpcomingEvents.SelectedItem as CalendarItem;
            _presenter.DeleteEvent(selected);
        }

        private void DoubleClick_Edit(object sender, RoutedEventArgs e)
        {
            CalendarItem selected = UpcomingEvents.SelectedItem as CalendarItem;
            UpdateEventWindow window = new UpdateEventWindow(_presenter, selected);
            _presenter.PopulateUpdateEventFields(selected);
            window.Show();
        }

        // Interface implementation

        #region Interface Implementation
        public void AddCategoriesToDropdown(List<Category> categories)
        {
            categoryComboBox.ItemsSource = categories;
        }
        public void ShowUpcomingEvents(List<CalendarItem> upcomingEvents)
        {

            var column = new DataGridTextColumn();
            column.Header = "Start Date";
            column.Binding = new System.Windows.Data.Binding("StartDateTime");
            column.Binding.StringFormat = "{0:yyyy/MM/dd}";
            var column2 = new DataGridTextColumn();
            column2.Header = "Start Time";
            column2.Binding = new System.Windows.Data.Binding("StartDateTime");
            column2.Binding.StringFormat = "{0:HH:mm:ss}";
            var column3 = new DataGridTextColumn();
            column3.Header = "Category";
            column3.Binding = new System.Windows.Data.Binding("Category");
            var column4 = new DataGridTextColumn();
            column4.Header = "Description";
            column4.Binding = new System.Windows.Data.Binding("ShortDescription");
            var column5 = new DataGridTextColumn();
            column5.Header = "Duration";
            column5.Binding = new System.Windows.Data.Binding("DurationInMinutes");
            var column6 = new DataGridTextColumn();
            column6.Header = "Busy Time";
            column6.Binding = new System.Windows.Data.Binding("BusyTime");
            column6.Binding.StringFormat = "{0:F2}";

            UpcomingEvents.Columns.Clear();

            UpcomingEvents.Columns.Add(column);
            UpcomingEvents.Columns.Add(column2);
            UpcomingEvents.Columns.Add(column3);
            UpcomingEvents.Columns.Add(column4);
            UpcomingEvents.Columns.Add(column5);
            UpcomingEvents.Columns.Add(column6);

            UpcomingEvents.ItemsSource = upcomingEvents;
        }

        public void ShowNoUpcomingEvents()
        {
            UpcomingEvents.Columns.Clear();
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
            column2.Binding = new System.Windows.Data.Binding("TotalBusyTime");
            column2.Binding.StringFormat = "{0:F2}";
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
            column2.Binding.StringFormat = "{0:F2}";
            UpcomingEvents.Columns.Add(column);
            UpcomingEvents.Columns.Add(column2);

        }
        public void ShowUpcomingEventsByMonthAndCategory(List<Dictionary<string, object>> items, List<Category> categories)
        {
            UpcomingEvents.Columns.Clear();
            List<string> addedKeys = new List<string>();


            var column2 = new DataGridTextColumn();
            column2.Header = "Month";
            column2.Binding = new System.Windows.Data.Binding($"[Month]");
            UpcomingEvents.Columns.Add(column2);
            addedKeys.Add("Month");


            var column1 = new DataGridTextColumn();
            column1.Header = "Total Busy Time";
            column1.Binding = new System.Windows.Data.Binding($"[TotalBusyTime]");
            column1.Binding.StringFormat = "{0:F2}";
            UpcomingEvents.Columns.Add(column1);
            addedKeys.Add("TotalBusyTime");


            foreach (Category category in categories)
            {
                var column = new DataGridTextColumn();
                column.Header = category.Description;
                column.Binding = new System.Windows.Data.Binding($"[{category.Description}]");
                UpcomingEvents.Columns.Add(column);
                addedKeys.Add(category.Description);
            }

            UpcomingEvents.ItemsSource = items;
        }


        private void Change_InFilters(object sender, RoutedEventArgs e)
        {
            DateTime? startDate = startDatePicker.SelectedDate;
            DateTime? endDate = endDatePicker.SelectedDate;
            _presenter.ViewSelector((bool)summaryByMonth.IsChecked, (bool)summaryByCategory.IsChecked, (bool)FilterCategory.IsChecked, (Category)categoryComboBox.SelectedItem, startDate, endDate);

        }
         public void HighlightRow(int index, CalendarItem item)
        {
            UpcomingEvents.SelectedIndex = index;
            UpcomingEvents.ScrollIntoView(item);
        }
        #endregion
    }
}
