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
    /// Логика взаимодействия для SpecialtiesDataPage.xaml
    /// </summary>
    public partial class SpecialtiesDataPage : Page
    {
        private AdminWindow adminWindow = null;

        public ObservableCollection<specialties> Specialties { get;set; }
        public specialties SelectedSpecialty { get; set; }

        public SpecialtiesDataPage(AdminWindow adminWindow)
        {

            try
            {

                InitializeComponent();


                Specialties = new ObservableCollection<specialties>(Connection.Database.specialties);

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

        private bool UserFilter(object item)
        {
            if (String.IsNullOrEmpty(textBoxFilter.Text))
                return true;

            switch (comboboxFilterType.SelectedIndex)
            {
                case 0:
                    return ((item as specialties).specialty_name.IndexOf(textBoxFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);

                case 1:
                    return ((item as specialties).faculties.faculty_name.IndexOf(textBoxFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);

                default:
                    return true;
            }
        }

        private void textBoxFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(listBoxSpecialties.ItemsSource).Refresh();
        }

        private void RemoveSpecialtyData_ButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {

                if (SelectedSpecialty != null)
                {

                    var messageBoxResult = MessageBox.Show("Вы действительно удалить данную специальность?", "Подтверждение действия",
                        MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (messageBoxResult == MessageBoxResult.Yes)
                    {

                        Connection.Database.specialties.Remove(SelectedSpecialty);
                        Specialties.Remove(SelectedSpecialty);

                        Connection.Database.SaveChanges();

                    }

                }
                else
                {
                    MessageBox.Show("Специальность не выбрана.", "Выберите специальность.", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Ошибка сохранения", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ChangeSpecialtyData_ButtonClick(object sender, RoutedEventArgs e)
        {
            if ( SelectedSpecialty != null )
            {
                adminWindow.mainFrame.Navigate(new EditSpecialtyDataPage(adminWindow, SelectedSpecialty));
            }
            else
            {
                MessageBox.Show("Вы не выбрали специальность.", "Выберите специальность", MessageBoxButton.OK,  MessageBoxImage.Warning);
            }
        }

        private void AddNewSpecialty_ButtonClick(object sender, RoutedEventArgs e)
        {
            adminWindow.mainFrame.Navigate(new EditSpecialtyDataPage(adminWindow));
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
