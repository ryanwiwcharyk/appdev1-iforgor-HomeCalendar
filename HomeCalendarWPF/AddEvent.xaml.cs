using Calendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
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
    /// Interaction logic for AddEvent.xaml
    /// </summary>
    public partial class AddEvent : Window, ICreateEventViewInterface
    {
        readonly Presenter _presenter;
        public AddEvent(Presenter presenter)
        {
            InitializeComponent();
            _presenter = presenter;
            _presenter.RegisterWindow(this);
            _presenter.PopulateCategoryDropdown();

        }
        private void BtnClick_CreateEvent(object sender, RoutedEventArgs e)
        {
            string details = eventDetails.Text;
            string duration = eventDuration.Text;
            DateTime? selectedDate = datePicker.SelectedDate;
            string comboBoxSelectedCategory = categoryComboBox.Text;
            _presenter.ValidateEventFormInputAndCreate(details, duration, selectedDate, comboBoxSelectedCategory);
            

        }
        private void BtnClick_CancelEvent(object sender, RoutedEventArgs e)
        {

        }
        public void ShowErrorPopup(string message)
        {
            MessageBox.Show( message, "Home Calendar");

        }
        public void ShowSuccessPopup(string message)
        {
            MessageBox.Show(message, "Home Calendar");


        }


        //Interface Implementation
        public string GetEventDetails()
        {
            throw new NotImplementedException();
        }

        public DateTime GetEventStartTime()
        {
            throw new NotImplementedException();
        }

        public double GetEventDurationInMinutes()
        {
            throw new NotImplementedException();
        }

        public int GetEventCategory()
        {
            throw new NotImplementedException();
        }

        public void AddCategoriesToDropdown(string categoryName)
        {
            throw new NotImplementedException();
        }

        public void AddCategoriesToDropdown(List<Category> categories)
        {
            categoryComboBox.ItemsSource = categories;
        }
    }
}
