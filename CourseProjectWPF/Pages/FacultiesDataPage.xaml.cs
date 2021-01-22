using CourseProjectWPF.Model;
using CourseProjectWPF.Windows;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Core;
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

namespace CourseProjectWPF.Pages
{
    /// <summary>
    /// Логика взаимодействия для FacultiesDataPage.xaml
    /// </summary>
    public partial class FacultiesDataPage : Page
    {

        private AdminWindow adminWindow = null;

        public ObservableCollection<faculties> Faculties { get; set; }
        public faculties CurrentRow { get; set; }

        public FacultiesDataPage(AdminWindow adminWindow)
        {

            try
            {

                InitializeComponent();

                Faculties = new ObservableCollection<faculties>(Connection.Database.faculties);

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

        private void dataGridFaculties_Loaded(object sender, RoutedEventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(dataGridFaculties.ItemsSource);
            view.Filter = UserFilter;
        }

        private bool UserFilter(object item)
        {
            if (String.IsNullOrEmpty(textBoxFilter.Text))
                return true;

            return ((item as faculties).faculty_name.IndexOf(textBoxFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);

        }

        private void ChangeData_ButtonClick(object sender, RoutedEventArgs e)
        {
            if (CurrentRow != null)
            {
                adminWindow.mainFrame.Navigate(new EditFacultyDataPage(adminWindow, CurrentRow));
            }
            else
            {
                MessageBox.Show("Факультет не выбран", "Выберите факультет", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void RemoveRow_ButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {

                if (CurrentRow != null)
                {

                    var messageBoxResult = MessageBox.Show("Вы действительно удалить данный факультет?", "Подтверждение действия",
                        MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (messageBoxResult == MessageBoxResult.Yes)
                    {

                        Connection.Database.faculties.Remove(CurrentRow);
                        Faculties.Remove(CurrentRow);

                        Connection.Database.SaveChanges();
                    }
                }
                else
                {
                    MessageBox.Show("Аудитория не выбрана.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Ошибка сохранения", MessageBoxButton.OK, MessageBoxImage.Warning);
                Connection.Database.faculties.Remove(CurrentRow);
            }
        }

        private void AddNewRow_ButtonClick(object sender, RoutedEventArgs e)
        {
            adminWindow.mainFrame.Navigate(new EditFacultyDataPage(adminWindow));
        }

        private void textBoxFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(dataGridFaculties.ItemsSource).Refresh();
        }
    }
}
