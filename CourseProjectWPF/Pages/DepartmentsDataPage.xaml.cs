using CourseProjectWPF.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

using CourseProjectWPF.Windows;
using System.Data.Entity.Core;

namespace CourseProjectWPF.Pages
{
    /// <summary>
    /// Логика взаимодействия для DepartmentsDataPage.xaml
    /// </summary>
    public partial class DepartmentsDataPage : Page
    {
        private AdminWindow adminWindow = null;

        public ObservableCollection<departments> Departments { get; set; }
        public departments CurrentRow { get; set; }

        public DepartmentsDataPage(AdminWindow adminWindow)
        {
            try
            {

                InitializeComponent();

                Departments = new ObservableCollection<departments>(Connection.Database.departments);

                this.adminWindow = adminWindow;

                DataContext = this;

            }
            catch (EntityException err)
            {
                MessageBox.Show(err.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                MainWindow mainWindow = new MainWindow();
                mainWindow.IsConnected = false;
                mainWindow.Show();

                adminWindow.Close();

            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void dataGridDepartments_Loaded(object sender, RoutedEventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(dataGridDepartments.ItemsSource);
            view.Filter = UserFilter;
        }

        private bool UserFilter(object item)
        {
            if (String.IsNullOrEmpty(textBoxFilter.Text))
                return true;

            return ((item as departments).department_name.IndexOf(textBoxFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private void textBoxFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(dataGridDepartments.ItemsSource).Refresh();
        }

        private void AddNewRow_ButtonClick(object sender, RoutedEventArgs e)
        {
            adminWindow.mainFrame.Navigate(new EditDepartmentDataPage(adminWindow));
        }

        private void ChangeData_ButtonClick(object sender, RoutedEventArgs e)
        {
            if ( CurrentRow != null )
            {
                adminWindow.mainFrame.Navigate(new EditDepartmentDataPage(adminWindow, CurrentRow));
            }
            else
            {
                MessageBox.Show("Кафедра не выбрана.", "Выберите кафедру.", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void RemoveRow_ButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {

                if (CurrentRow != null)
                {

                    var messageBoxResult = MessageBox.Show("Вы действительно удалить данную кафедру?", "Подтверждение действия",
                        MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (messageBoxResult == MessageBoxResult.Yes)
                    {

                        Connection.Database.departments.Remove(CurrentRow);
                        Departments.Remove(CurrentRow);

                        Connection.Database.SaveChanges();
                    }
                }
                else
                {
                    MessageBox.Show("Кафедра не выбрана.", "Выберите кафедру.", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Ошибка сохранения", MessageBoxButton.OK, MessageBoxImage.Warning);
                Connection.Database.departments.Remove(CurrentRow);
            }
        }
    }
}
