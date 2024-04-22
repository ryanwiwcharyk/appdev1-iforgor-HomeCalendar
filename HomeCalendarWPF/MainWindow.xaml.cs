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
    public partial class MainWindow : Window, ViewInterface
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
            this.Close();
            presenter.NewCalendar(location,name);

        }
        private void Btn_Click_ExistingCalendar(object sender, RoutedEventArgs e)
        {
            string location = newLocation.Text;
            this.Close();
            presenter.ExistingCalendar(location);
        }
    }
}
