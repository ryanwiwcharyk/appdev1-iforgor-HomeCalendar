using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HomeCalendarWPF.interfaces
{
    /// <summary>
    /// Interaction logic for UpdateEvent.xaml
    /// </summary>
    public partial class UpdateEvent : Window, UpdateEventViewInterface
    {
        readonly Presenter _presenter;
        CalendarItem _calendarItemToUpdate;
        public UpdateEvent(Presenter presenter, CalendarItem selectedCalendarItem)
        {
            InitializeComponent();
            _calendarItemToUpdate = selectedCalendarItem;
            _presenter = presenter;
            _presenter.RegisterWindow(this);
            _presenter.PopulateCategoryUpdateFields();
        }

        public void PopulateFields()
        {

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
            _presenter.ValidateEventFormInputAndCreate(details, duration, selectedDate, comboBoxSelectedCategory);
        }
    }
}
