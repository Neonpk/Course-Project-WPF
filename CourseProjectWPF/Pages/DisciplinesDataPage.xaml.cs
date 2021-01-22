using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using CourseProjectWPF.Model;
using CourseProjectWPF.Windows;

namespace CourseProjectWPF.Pages
{
    /// <summary>
    /// Логика взаимодействия для DisciplinesDataPage.xaml
    /// </summary>
    public partial class DisciplinesDataPage : Page
    {
        private AdminWindow adminWindow = null;

        public ObservableCollection<disciplines> Disciplines { get; set; }
        public disciplines SelectedDiscipline { get; set; }

        public DisciplinesDataPage(AdminWindow adminWindow)
        {

            try
            {

                InitializeComponent();

                Disciplines = new ObservableCollection<disciplines>(Connection.Database.disciplines);

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

        private void AddNewRoom_ButtonClick(object sender, RoutedEventArgs e)
        {
            adminWindow.mainFrame.Navigate(new EditDisciplineDataPage(adminWindow));
        }

        private void textBoxFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(listboxDisciplines.ItemsSource).Refresh();
        }

        private bool UserFilter(object item)
        {
            if (String.IsNullOrEmpty(textBoxFilter.Text))
                return true;

            switch (comboboxFilterType.SelectedIndex)
            {
                case 0:
                    return ((item as disciplines).discipline_name.IndexOf(textBoxFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);

                case 1:
                    return ((item as disciplines).departments.department_name.IndexOf(textBoxFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);

                default:
                    return true;
            }
        }

        private void ChangeDisciplineData_ButtonClick(object sender, RoutedEventArgs e)
        {
            if ( SelectedDiscipline != null )
            {
                adminWindow.mainFrame.Navigate(new EditDisciplineDataPage(adminWindow, SelectedDiscipline));
            }
            else
            {
                MessageBox.Show("Дисциплина не выбрана.", "Выберите дисциплину.", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void RemoveDisciplineData_ButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {

                if (SelectedDiscipline != null)
                {

                    var messageBoxResult = MessageBox.Show("Вы действительно удалить данную дисциплину?", "Подтверждение действия",
                        MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (messageBoxResult == MessageBoxResult.Yes)
                    {

                        Connection.Database.disciplines.Remove(SelectedDiscipline);
                        Disciplines.Remove(SelectedDiscipline);

                        Connection.Database.SaveChanges();

                    }
                }
                else
                {
                    MessageBox.Show("Дисциплина не выбрана.", "Выберите дисциплину.", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Ошибка сохранения", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void CollectionViewSource_Filter(object sender, FilterEventArgs e)
        {
            e.Accepted = UserFilter(e.Item);
        }


        private void comboboxFilterSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboboxFilterSortFieldType == null) return;

            CollectionViewSource cvs = FindResource("collectionViewSource") as CollectionViewSource;

            cvs.SortDescriptions.Clear();

            cvs.SortDescriptions.Add(new SortDescription(
                (comboboxFilterSortFieldType.SelectedItem as ComboBoxItem).Tag as string,
                (ListSortDirection)comboboxFilterSortModeType.SelectedIndex
            ));

        }

    }
}
