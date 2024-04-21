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
using Microsoft.Win32;
using System.IO;
using WinForms = System.Windows.Forms;

namespace HomeCalendarWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, MainViewInterface
    {
        private readonly Presenter presenter;

        public MainWindow()
        {
            InitializeComponent();
            presenter = new Presenter(this);
        }

        private void BtnClick_OpenFileExplorer(object sender, RoutedEventArgs e)
        {
            ExistingCalendar();
        }

        private void BtnClick_OpenFolderPicker(object sender, RoutedEventArgs e)
        {
            NewCalendar();

        }
        public void NewCalendar()
        {
            WinForms.FolderBrowserDialog folderBrowserDialog = new WinForms.FolderBrowserDialog();
            string name = newName.Text;
            folderBrowserDialog.InitialDirectory = $"{System.Environment.SpecialFolder.MyDocuments.ToString()}";
            WinForms.DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == WinForms.DialogResult.OK)
            {
                if (string.IsNullOrEmpty(name))
                {
                    name = "newCalendar";
                }
                string folder = folderBrowserDialog.SelectedPath;
                presenter.NewCalendar(folder, name);
                this.Close();
            }
        }
        public void ExistingCalendar()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            bool? success = openFileDialog.ShowDialog();

            if (success == true)
            {
                string? filePath = System.IO.Path.GetDirectoryName(openFileDialog.FileName);
                string? fileName = System.IO.Path.GetFileName(openFileDialog.FileName);
                presenter.ExistingCalendar(openFileDialog.FileName);
                this.Close();
            }
        }

    }
}
