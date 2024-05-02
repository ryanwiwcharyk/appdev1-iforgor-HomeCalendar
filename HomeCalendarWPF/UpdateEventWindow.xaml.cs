using Calendar;
using HomeCalendarWPF.interfaces;
using System;
using System.Collections.Generic;
using System.Windows;

namespace HomeCalendarWPF
{
    /// <summary>
    /// Interaction logic for UpdateEventWindow.xaml
    /// </summary>
    public partial class UpdateEventWindow : Window, UpdateEventViewInterface
    {
        readonly Presenter _presenter;
        private CalendarItem _calendarItemToUpdate;

        #region Constructor
        public UpdateEventWindow(Presenter presenter, CalendarItem selectedCalendarItem)
        {
            InitializeComponent();
            _calendarItemToUpdate = selectedCalendarItem;
            _presenter = presenter;
            InitializeWindow();
        }
        #endregion

        #region Properties
        public int EventId { get { return _calendarItemToUpdate.EventID; } }
        #endregion

        #region Interface Implementation

        public void ShowPopulatedFields(string details, double duration, DateTime startDate, int hours, int minutes, Category category)
        {
            eventDetails.Text = details;
            eventDuration.Text = duration.ToString("F2");
            datePicker.Text = startDate.ToString("yyyy/MM/dd");
            hourSelector.Text = hours.ToString();
            minuteSelector.Text = minutes.ToString();
            categoryComboBox.Text = category.Description;
        }

        public void AddCategoriesToDropdown(List<Category> categories)
        {
            categoryComboBox.ItemsSource = categories;
        }

        public void ShowErrorPopup(string message)
        {
            MessageBox.Show(message, "Home Calendar", MessageBoxButton.OK, MessageBoxImage.Error);

        }
        public void ShowSuccessPopup(string message)
        {
            MessageBox.Show(message, "Home Calendar", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        #endregion

        #region Private Methods
        private void InitializeWindow()
        {
            _presenter.RegisterWindow(this);
            _presenter.PopulateUpdateEventFields(_calendarItemToUpdate);
            _presenter.PopulateCategoryDropdownForUpdate();
            PopulateHourDropdown();
            PopulateMinutesDropdown();
        }
        private void PopulateHourDropdown()
        {
            const int HOURS_IN_DAY = 24;
            List<int> hours = new List<int>();
            for (int i = 1; i <= HOURS_IN_DAY; i++)
            {
                hours.Add(i);
            }
            hourSelector.ItemsSource = hours;
        }

        private void PopulateMinutesDropdown()
        {
            List<int> minutesByFifteen = new List<int>() { 0, 15, 30, 45 };
            minuteSelector.ItemsSource = minutesByFifteen;
        }

        private void BtnClick_CancelEvent(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("You are about to cancel the update of this event", "Home Calendar", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }

        private void BtnClick_UpdateEvent(object sender, RoutedEventArgs e)
        {
            int hour = 0;
            int minutes = 0;
            string details = eventDetails.Text;
            string duration = eventDuration.Text;

            if (hourSelector.SelectedItem is not null)
                hour = (int)hourSelector.SelectedItem;
            if (minuteSelector.SelectedItem is not null)
                minutes = (int)minuteSelector.SelectedItem;

            DateTime? selectedDate = new DateTime(DateTime.Now.Year, datePicker.SelectedDate.Value.Month, datePicker.SelectedDate.Value.Day, hour, minutes, 0);
            Category cat = categoryComboBox.SelectedItem as Category;
            _presenter.ValidateEventFormInputAndUpdate(EventId, details, duration, selectedDate, cat);
        }

        private void BtnClick_AddCategory(object sender, RoutedEventArgs e)
        {
            AddCategory category = new AddCategory(_presenter);
            category.ShowDialog();

        }

        #endregion

    }
}
