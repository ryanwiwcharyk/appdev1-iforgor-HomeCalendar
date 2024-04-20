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
    public partial class MainWindow : Window, DatabaseViewInterface
    {
        private readonly Presenter presenter;

        public MainWindow()
        {
            InitializeComponent();
            presenter = new Presenter(this);
        }
        private void Btn_Click_NewCalendar(object sender, RoutedEventArgs e)
        {
            string location = newLocation.Text ;
            string name = newName.Text ;
            this.Hide();
            ConnectToDb(location, name);


        }
        private void Btn_Click_ExistingCalendar(object sender, RoutedEventArgs e)
        {
            string location = existingLocation.Text;
            string name = "";
            this.Hide();
            ConnectToDb(location, name);
        }

        public void ConnectToDb(string location, string name)
        {
            presenter.ConnectingToExistingOrNewDatabase(location, name); //new method I created, may need extra error handling
        }

    }
}
