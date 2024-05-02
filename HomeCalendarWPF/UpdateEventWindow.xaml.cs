using Calendar;
using HomeCalendarWPF.interfaces;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for UpdateEventWindow.xaml
    /// </summary>
    public partial class UpdateEventWindow : Window, UpdateEventViewInterface
    {
        readonly Presenter _presenter;
        private CalendarItem _calendarItemToUpdate;

        public UpdateEventWindow(Presenter presenter, CalendarItem selectedCalendarItem)
        {
            InitializeComponent();
            _calendarItemToUpdate = selectedCalendarItem;
            _presenter = presenter;
            InitializeWindow();
            
        }

        public void ShowPopulatedFields(string details, double duration, DateTime startDate, int hours, int minutes, Category category)
        {
            eventDetails.Text = details;
            eventDuration.Text = duration.ToString("F2");
            datePicker.Text = startDate.ToString("yyyy/MM/dd");
            hourSelector.Text = hours.ToString();
            minuteSelector.Text = minutes.ToString();
            categoryComboBox.Text = category.Description;
        }

        public void ShowErrorPopup(string message)
        {
            MessageBox.Show(message, "Home Calendar");

        }
        public void ShowSuccessPopup(string message)
        {
            MessageBox.Show(message, "Home Calendar");
            this.Close();

        }

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
            MessageBoxResult messageBoxResult = MessageBox.Show("You are about to cancel the update of this event", "Home Calendar", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }

        private void BtnClick_UpdateEvent(object sender, RoutedEventArgs e)
        {
            int hour;
            int minutes;
            string details = eventDetails.Text;
            string duration = eventDuration.Text;

            if (hourSelector.SelectedItem is null)
                hour = 0;
            else
                hour = (int)hourSelector.SelectedItem;
            if (minuteSelector.SelectedItem is null)
                minutes = 0;
            else
                minutes = (int)minuteSelector.SelectedItem;

            DateTime? selectedDate = new DateTime(DateTime.Now.Year, datePicker.SelectedDate.Value.Month, datePicker.SelectedDate.Value.Day, hour, minutes, 0);
            string comboBoxSelectedCategory = categoryComboBox.Text;
            _presenter.ValidateEventFormInputAndUpdate(_calendarItemToUpdate.EventID, details, duration, selectedDate, comboBoxSelectedCategory);
        }

        private void BtnClick_AddCategory(object sender, RoutedEventArgs e)
        {
            AddCategory category = new AddCategory(_presenter);
            category.Show();

        }

        public void AddCategoriesToDropdown(List<Category> categories)
        {
            categoryComboBox.ItemsSource = categories;
        }

    }
}
