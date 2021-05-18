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
    /// Interaction logic for ChangeNotesDataPage.xaml
    /// </summary>
    public partial class ChangeNotesDataPage : Page
    {

        private AdminWindow _adminWindow = null;

        public ObservableCollection<changedescription> ChangeNotes { get; set; }
        public changedescription CurrentRow { get; set; }

        public ChangeNotesDataPage(AdminWindow adminWindow)
        {
            try
            {

                InitializeComponent();

                ChangeNotes = new ObservableCollection<changedescription>(Connection.Database.changedescription);

                this._adminWindow = adminWindow;

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

        private void dataGridChangeNotes_Loaded(object sender, RoutedEventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(dataGridChangeNotes.ItemsSource);
            view.Filter = UserFilter;
        }

        private bool UserFilter(object item)
        {
            if (String.IsNullOrEmpty(textBoxFilter.Text))
                return true;

            return ((item as changedescription).description.IndexOf(textBoxFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private void textBoxFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(dataGridChangeNotes.ItemsSource).Refresh();
        }

        private void AddNewRow_ButtonClick(object sender, RoutedEventArgs e)
        {
            _adminWindow.mainFrame.Navigate(new EditChangeNotesDataPage(_adminWindow));
        }

        private void ChangeData_ButtonClick(object sender, RoutedEventArgs e)
        {
            if (CurrentRow != null)
            {
                _adminWindow.mainFrame.Navigate(new EditChangeNotesDataPage(_adminWindow, CurrentRow));
            }
            else
            {
                MessageBox.Show("Примечание не выбрано.", "Выберите примечание.", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void RemoveRow_ButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {

                if (CurrentRow != null)
                {

                    var messageBoxResult = MessageBox.Show("Вы действительно удалить данное примечание?", "Подтверждение действия",
                        MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (messageBoxResult == MessageBoxResult.Yes)
                    {

                        Connection.Database.changedescription.Remove(CurrentRow);
                        ChangeNotes.Remove(CurrentRow);

                        Connection.Database.SaveChanges();
                    }
                }
                else
                {
                    MessageBox.Show("Примечание не выбрано.", "Выберите примечание.", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Ошибка сохранения", MessageBoxButton.OK, MessageBoxImage.Warning);
                Connection.Database.changedescription.Remove(CurrentRow);
            }
        }

    }
}
