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
    /// Interaction logic for AddEvent.xaml
    /// </summary>
    public partial class AddEvent : Window
    {
        readonly Presenter _presenter;
        public AddEvent(Presenter presenter)
        {
            InitializeComponent();
            _presenter = presenter;
            InitializeFields();

        }
        private void InitializeFields()
        {
            _presenter.PopulateCategoriesMenu();
        }
        private void AddCategoryToMenu(string categoryName)
        {
            
            CategoryMenu.Items.Add(categoryName);
        }
        private void Btn_Click_Create_Event(object sender, RoutedEventArgs e)
        {
            

        }
    }
}
