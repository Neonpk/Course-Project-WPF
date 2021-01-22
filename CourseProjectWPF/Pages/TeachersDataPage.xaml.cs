using CourseProjectWPF.Model;
using CourseProjectWPF.Windows;
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

namespace CourseProjectWPF.Pages
{
    /// <summary>
    /// Логика взаимодействия для TeachersDataPage.xaml
    /// </summary>
    public partial class TeachersDataPage : Page
    {

        DispatcherWindow windowOwner = null;

        public string[] FiltersName { get; } = new string[] { "По имени", "По званию", "По кафедре", "По должности" };

        public teachers SelectedTeacher { get; set; }
        public ObservableCollection<teachers> Teachers { get; set; }

        public TeachersDataPage(DispatcherWindow owner)
        {

            try
            {

                InitializeComponent();

                this.windowOwner = owner;

                Teachers = new ObservableCollection<teachers>(Connection.Database.teachers);

                DataContext = this;

            }
            catch(EntityException err)
            {
                MessageBox.Show(err.Message, "Произошла ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                MainWindow mainWindow = new MainWindow();
                mainWindow.IsConnected = false;
                mainWindow.Show();

                owner.Close();

            }catch(Exception err)
            {
                MessageBox.Show(err.Message, "Произошла ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private bool UserFilter(object item)
        {
            if (String.IsNullOrEmpty(textBoxFilter.Text))
                return true;
            
            switch(comboboxFilterType.SelectedIndex)
            {
                case 0:
                    return ((item as teachers).name.IndexOf(textBoxFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);

                case 1:
                    return ((item as teachers).ranks.rank_name.IndexOf(textBoxFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);

                case 2:
                    return ((item as teachers).departments.department_name.IndexOf(textBoxFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);

                case 3:
                    return ((item as teachers).positions.position_name.IndexOf(textBoxFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);

                default:
                    return true;
            }              
        }

        private void TeacherChangeData_ButtonClick(object sender, RoutedEventArgs e)
        {

            if (SelectedTeacher != null)
            {
                windowOwner.mainFrame.Navigate(new EditTeacherDataPage(this.windowOwner, SelectedTeacher));
            }
            else
            {
                MessageBox.Show("Преподаватель не выбран", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }


        }

        private void TeacherRemoveData_ButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {

                if (SelectedTeacher != null)
                {

                    var messageBoxResult = MessageBox.Show("Вы действительно удалить данного преподавателя?", "Подтверждение действия", 
                        MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (messageBoxResult == MessageBoxResult.Yes)
                    {

                        Connection.Database.teachers.Remove(SelectedTeacher);
                        Teachers.Remove(SelectedTeacher);

                        Connection.Database.SaveChanges();

                    }

                }
                else
                {
                    MessageBox.Show("Преподаватель не выбран", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Ошибка сохранения", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }

        private void AddNewTeacher_ButtonClick(object sender, RoutedEventArgs e)
        {
            windowOwner.mainFrame.Navigate(new EditTeacherDataPage(this.windowOwner));
        }

        private void textBoxFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(listBoxTeachers.ItemsSource).Refresh();
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
