using CourseProjectWPF.Model;
using CourseProjectWPF.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity.Core;
using System.Data.SqlClient;
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
    /// Логика взаимодействия для GroupsDataPage.xaml
    /// </summary>
    public partial class GroupsDataPage : Page
    {

        public groups SelectedGroup { get; set; }
        public ObservableCollection<groups> Groups { get; set; }

        public DispatcherWindow dispatcherWindow = null;

        public GroupsDataPage(DispatcherWindow window)
        {

            try
            {

                InitializeComponent();

                Groups = new ObservableCollection<groups>(Connection.Database.groups);

                DataContext = this;

                this.dispatcherWindow = window;

            }catch(EntityException err)
            {

                MessageBox.Show(err.Message, "Произошла ошибка.", 
                    MessageBoxButton.OK, MessageBoxImage.Error);

                MainWindow mainWindow = new MainWindow();
                mainWindow.IsConnected = false;
                mainWindow.Show();

                window.Close();
            }

            catch(Exception err)
            {
                MessageBox.Show(err.Message, "Произошла ошибка.",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }

        private void AddNewGroup_ButtonClick(object sender, RoutedEventArgs e)
        {
            this.dispatcherWindow.mainFrame.Navigate(new EditGroupsDataPage(this.dispatcherWindow));   
        }

        private void RemoveGroup_ButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {

                if (SelectedGroup != null)
                {

                    var messageBoxResult = MessageBox.Show("Вы действительно удалить данную группу?", "Подтверждение действия",
                        MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (messageBoxResult == MessageBoxResult.Yes)
                    {

                        Connection.Database.groups.Remove(SelectedGroup);
                        Groups.Remove(SelectedGroup);

                        Connection.Database.SaveChanges();

                    }

                }
                else
                {
                    MessageBox.Show("Группа не выбрана.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Ошибка сохранения", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ChangeGroupData_ButtonClick(object sender, RoutedEventArgs e)
        {
            if (SelectedGroup != null)
            {
                dispatcherWindow.mainFrame.Navigate(new EditGroupsDataPage(this.dispatcherWindow, SelectedGroup));
            }
            else
            {
                MessageBox.Show("Группа не выбрана.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private bool UserFilter(object item)
        {
            if (String.IsNullOrEmpty(textBoxFilter.Text))
                return true;

            switch (comboboxFilterType.SelectedIndex)
            {
                case 0:
                    return ((item as groups).group_id.ToString().IndexOf(textBoxFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);

                case 1:
                    return ((item as groups).count_people.ToString().IndexOf(textBoxFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);

                case 2:
                    return ((item as groups).specialties.specialty_name.IndexOf(textBoxFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);

                case 3:
                    return ((item as groups).faculties.faculty_name.IndexOf(textBoxFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);

                case 4:
                    return ((item as groups).train_types.name.IndexOf(textBoxFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);

                default:
                    return true;
            }
        }


        private void ListBoxGroups_Loaded(object sender, RoutedEventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(listBoxGroups.ItemsSource);
            view.Filter = UserFilter;
        }

        private void textBoxFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(listBoxGroups.ItemsSource).Refresh();
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
