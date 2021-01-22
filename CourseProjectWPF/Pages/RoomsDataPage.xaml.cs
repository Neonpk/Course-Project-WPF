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
    /// Логика взаимодействия для RoomsDataPage.xaml
    /// </summary>
    public partial class RoomsDataPage : Page
    {

        public ObservableCollection<audiences> Audiences { get; set; }
        public audiences SelectedRoom { get; set; }

        private AdminWindow adminWindow = null;
        public RoomsDataPage(AdminWindow adminWindow)
        {

            try
            {

                InitializeComponent();

                this.adminWindow = adminWindow;

                Audiences = new ObservableCollection<audiences>(Connection.Database.audiences);

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

        private void ChangeRoomData_ButtonClick(object sender, RoutedEventArgs e)
        {
            if (SelectedRoom != null)
            {
                adminWindow.mainFrame.Navigate(new EditRoomDataPage(adminWindow, SelectedRoom));
            }else
            {
                MessageBox.Show("Аудитория не выбрана", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void RemoveRoomData_ButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {

                if (SelectedRoom != null)
                {

                    var messageBoxResult = MessageBox.Show("Вы действительно удалить данную аудиторию?", "Подтверждение действия",
                        MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (messageBoxResult == MessageBoxResult.Yes)
                    {

                        Connection.Database.audiences.Remove(SelectedRoom);
                        Audiences.Remove(SelectedRoom);

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
                Connection.Database.audiences.Remove(SelectedRoom);
            }
        }

        private void AddNewRoom_ButtonClick(object sender, RoutedEventArgs e)
        {
            adminWindow.mainFrame.Navigate(new EditRoomDataPage(adminWindow));
        }

        private bool UserFilter(object item)
        {
            if (String.IsNullOrEmpty(textBoxFilter.Text))
                return true;

            switch (comboboxFilterType.SelectedIndex)
            {
                case 0:
                    return ((item as audiences).room.ToString().IndexOf(textBoxFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);

                case 1:
                    return ((item as audiences).capacity.ToString().IndexOf(textBoxFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);

                case 2:
                    return ((item as audiences).type.IndexOf(textBoxFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);

                default:
                    return true;
            }
        }

        private void textBoxFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(listBoxAudiences.ItemsSource).Refresh();
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

        private void CollectionViewSource_Filter(object sender, FilterEventArgs e)
        {
            e.Accepted = UserFilter(e.Item);
        }
    }
}
