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
using CourseProjectWPF.Model;
using CourseProjectWPF.Windows;

namespace CourseProjectWPF.Pages
{
    /// <summary>
    /// Логика взаимодействия для EditFacultyDataPage.xaml
    /// </summary>
    public partial class EditFacultyDataPage : Page
    {

        private AdminWindow adminWindow = null;

        public faculties SelectedFaculty { get; set; }
        private faculties EditFacultyReference { get; set; }

        private bool IsErrorField = false;


        public EditFacultyDataPage(AdminWindow adminWindow)
        {
            InitializeComponent();

            SelectedFaculty = new faculties
            {
                faculty_name = "Какой-то факультет"
            };

            this.adminWindow = adminWindow;

            DataContext = this;

        }

        public EditFacultyDataPage(AdminWindow adminWindow, faculties SelectedFaculty)
        {
            InitializeComponent();

            this.SelectedFaculty = (faculties)SelectedFaculty.Clone();
            this.EditFacultyReference = SelectedFaculty;

            this.adminWindow = adminWindow;

            DataContext = this;

        }


        private void TextBoxFields_Error(object sender, ValidationErrorEventArgs e)
        {

            if (e.Action == ValidationErrorEventAction.Added)
            {
                var errorToolTip = new ToolTip();
                errorToolTip.Content = e.Error.ErrorContent;

                (sender as TextBox).ToolTip = errorToolTip;

                IsErrorField = true;

            }

            if (e.Action == ValidationErrorEventAction.Removed)
            {
                IsErrorField = false;
            }

            buttonSaveIntoDB.IsEnabled = !IsErrorField;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.adminWindow.mainFrame.Navigate(new FacultiesDataPage(this.adminWindow));
        }

        private void SaveIntoDataBase(object sender, RoutedEventArgs e)
        {
            try
            {

                if (EditFacultyReference != null)
                {
                    EditFacultyReference.faculty_name = SelectedFaculty.faculty_name;
                }
                else
                {
                    Connection.Database.faculties.Add(SelectedFaculty);
                }

                Connection.Database.SaveChanges();

                adminWindow.mainFrame.Navigate(new FacultiesDataPage(adminWindow));


            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Ошибка при сохранении", MessageBoxButton.OK, MessageBoxImage.Error);
                Connection.Database.faculties.Remove(SelectedFaculty);
            }
        }

    }
}
