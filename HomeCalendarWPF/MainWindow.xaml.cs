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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HomeCalendarWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Presenter presenter;
        private readonly MainViewInterface mainView;
        private readonly CategoryView categoryView;
        private readonly ICreateEventViewInterface createEventView;
        public MainWindow()
        {
            InitializeComponent();
            presenter = new Presenter(categoryView, createEventView, mainView);
        }
        private void Btn_Click_NewCalendar(object sender, RoutedEventArgs e)
        {
            string location = newLocation.Text ;
            string name = newName.Text ;
            this.Hide();
            presenter.NewCalendar(location,name);

        }
        private void Btn_Click_ExistingCalendar(object sender, RoutedEventArgs e)
        {
            string location = existingLocation.Text;
            this.Hide();
            presenter.ExistingCalendar(location);
        }
    }
}
