﻿using System;
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
    public partial class AddEvent : Window, ICreateEventViewInterface
    {
        readonly Presenter _presenter;
        public AddEvent(Presenter presenter)
        {
            InitializeComponent();
            _presenter = presenter;
            _presenter.RegisterWindow(this);
            InitializeFields();

        }
        private void InitializeFields()
        {
            categoryComboBox.ItemsSource = _presenter.PopulateCategoriesMenu();
        }
        private void Btn_Click_Create_Event(object sender, RoutedEventArgs e)
        {
            

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

        public void AddCategoryToMenu(string categoryName)
        {
            throw new NotImplementedException();
        }
    }
}
