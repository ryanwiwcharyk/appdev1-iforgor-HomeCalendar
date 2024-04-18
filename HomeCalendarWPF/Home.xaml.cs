using Calendar;
using System;
using System.Collections.Generic;
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
    public partial class Home : Window, MainViewInterface
    {
        readonly Presenter _presenter;
        private readonly HomeCalendar _model;
        private readonly CategoryView categoryView;
        private readonly ICreateEventViewInterface createEventView;
        public Home(Presenter presenter)
        {
            InitializeComponent();
            _presenter = presenter;
            _presenter.RegisterWindow(this);
            _presenter.GetUpcomingEvents();
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
            CloseApplication();
        }

        // Interface implementation
        public void CloseApplication()
        {
            Application.Current.Shutdown();
        }

        void MainViewInterface.ShowRecentFiles()
        {
            throw new NotImplementedException();
        }
        
        public void ShowUpcomingEvents(List<string> upcomingEvents)
        {
            UpcomingEvents.ItemsSource = upcomingEvents;
        }

        void MainViewInterface.ShowNoUpcomingEvents(string message)
        {
            UpcomingEventsStatus.Text = message;
        }

        
    }
}
