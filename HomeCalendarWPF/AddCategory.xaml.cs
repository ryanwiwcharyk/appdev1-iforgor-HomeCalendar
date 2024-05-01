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
            _presenter.RegisterWindow(this);
            _presenter.PopulateCategoryTypesDropdown();
        }

        private void Btn_Add(object sender, RoutedEventArgs e)
        {
           _presenter.ValidateDetailsFormInputAndCreateCategory(Details.Text, categoryTypeComboBox.Text);
        }

        private void Btn_Cancel(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("You are about to cancel the addition of this category", "Home Calendar", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }

        public void FillDropDown(List<Category.CategoryType> types)
        {

            categoryTypeComboBox.ItemsSource = types;
        }

        public void ShowSuccessPopup(string message)
        {
            MessageBox.Show(message, "Home Calendar");
            this.Close();
        }

        public void ShowWarning(string warning)
        {
            MessageBox.Show(warning);
        }
    }
}
