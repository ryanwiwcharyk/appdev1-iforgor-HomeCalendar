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
        private readonly IWelcomeViewInterface welcomeView;
        public Home(Presenter presenter)
        {
            InitializeComponent();
            _presenter = presenter;


        }

        void MainViewInterface.CloseApplication()
        {
            throw new NotImplementedException();
        }

        void MainViewInterface.ShowAddCategoryTab()
        {
            throw new NotImplementedException();
        }

        void MainViewInterface.ShowAddEventTab()
        {
            throw new NotImplementedException();
        }

        void MainViewInterface.ShowCalendarFileNameLocationForm()
        {
            throw new NotImplementedException();
        }

        void MainViewInterface.ShowRecentFiles()
        {
            throw new NotImplementedException();
        }

        void MainViewInterface.ShowUpcomingEvents()
        {
            throw new NotImplementedException();
        }
    }
}
