using Calendar;
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
    /// Interaction logic for AddCategory.xaml
    /// </summary>


    public partial class AddCategory : Window, CategoryView
    {
        private readonly Presenter _presenter;
        private readonly HomeCalendar _model;

        public AddCategory(Presenter presenter)
        {
            InitializeComponent();
            _presenter = presenter;

        }

        private void Btn_Add(object sender, RoutedEventArgs e)
        {
            //the creation of the category is done here
        }

        private void Btn_Cancel(object sender, RoutedEventArgs e)
        {
            //close page
        }

        public void FillDropDown(List<Category.CategoryType> types)
        {
            // change this to presenter
            CategoryType.Items.Add(types);
        }

        public void RefreshPage()
        {
            throw new NotImplementedException();
        }

        public void ShowSuccess(string success)
        {
            throw new NotImplementedException();
        }

        public void ShowWarning(string warning)
        {
            throw new NotImplementedException();
        }
    }
}
