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
    /// Логика взаимодействия для EditDepartmentDataPage.xaml
    /// </summary>
    public partial class EditDepartmentDataPage : Page
    {

        private AdminWindow adminWindow = null;

        public departments SelectedDepartment {get; set;}
        private departments EditDepartmentReference { get; set; }

        private bool IsErrorField = false;

        public EditDepartmentDataPage(AdminWindow adminWindow)
        {
            InitializeComponent();

            SelectedDepartment = new departments
            {
                department_name = "Какое название кафедры..."
            };

            this.adminWindow = adminWindow;

            DataContext = this;
        }

        public EditDepartmentDataPage(AdminWindow adminWindow, departments SelectedDepartment)
        {
            InitializeComponent();

            this.SelectedDepartment = (departments)SelectedDepartment.Clone();
            this.EditDepartmentReference = SelectedDepartment;

            this.adminWindow = adminWindow;

            DataContext = this;
        }

        private void buttonSaveIntoDB_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (EditDepartmentReference != null)
                {
                    EditDepartmentReference.department_name = SelectedDepartment.department_name;
                }
                else
                {
                    Connection.Database.departments.Add(SelectedDepartment);
                }

                Connection.Database.SaveChanges();

                adminWindow.mainFrame.Navigate(new DepartmentsDataPage(adminWindow));


            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Ошибка при сохранении", MessageBoxButton.OK, MessageBoxImage.Error);
                Connection.Database.departments.Remove(SelectedDepartment);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.adminWindow.mainFrame.Navigate(new DepartmentsDataPage(this.adminWindow));
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
    }
}
