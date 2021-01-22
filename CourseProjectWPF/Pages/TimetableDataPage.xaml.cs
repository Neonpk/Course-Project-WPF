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
    /// Логика взаимодействия для TimetableDataPage.xaml
    /// </summary>
    public partial class TimetableDataPage : Page
    {
        public ObservableCollection<timetable> Timetable { get; set; }
        public timetable CurrentRow { get; set; }

        private DispatcherWindow dispatcherWindow = null;

        public TimetableDataPage(DispatcherWindow dispatcherWindow)
        {

            try
            {

                InitializeComponent();

                this.Timetable = new ObservableCollection<timetable>(Connection.Database.timetable.ToList());

                this.dispatcherWindow = dispatcherWindow;

                DataContext = this;

            }catch(EntityException err)
            {
                MessageBox.Show(err.Message, "Произошла ошибка.",
                    MessageBoxButton.OK, MessageBoxImage.Error);

                MainWindow mainWindow = new MainWindow();
                mainWindow.IsConnected = false;
                mainWindow.Show();

                dispatcherWindow.Close();
            }
            catch(Exception err)
            {
                MessageBox.Show(err.Message, "Произошла ошибка.",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void AddNewRow_ButtonClick(object sender, RoutedEventArgs e)
        {
            this.dispatcherWindow.mainFrame.Navigate(new EditTimetableDataPage(this.dispatcherWindow));
        }

        private void RemoveRow_ButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {

                if (CurrentRow != null)
                {

                    var messageBoxResult = MessageBox.Show("Вы действительно удалить данную запись?", "Подтверждение действия",
                        MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (messageBoxResult == MessageBoxResult.Yes)
                    {

                        Connection.Database.timetable.Remove(CurrentRow);
                        Timetable.Remove(CurrentRow);

                        Connection.Database.SaveChanges();

                    }

                }
                else
                {
                    MessageBox.Show("Запись не выбрана.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Ошибка сохранения", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ChangeData_ButtonClick(object sender, RoutedEventArgs e)
        {
            dispatcherWindow.mainFrame.Navigate(new EditTimetableDataPage(this.dispatcherWindow, CurrentRow));
        }

        private void textBoxFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(dataGridTimetable.ItemsSource).Refresh();
        }

        private void dataGridTimetable_Loaded(object sender, RoutedEventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(dataGridTimetable.ItemsSource);
            view.Filter = TimetableRowsFilter;
        }

        private bool TimetableRowsFilter(object item)
        {
            if (String.IsNullOrEmpty(textBoxFilter.Text))
                return true;

            switch (comboboxFilterType.SelectedIndex)
            {
                case 0:
                    return ((item as timetable).group_id.ToString().IndexOf(textBoxFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);

                case 1:
                    return ((item as timetable).room.ToString().IndexOf(textBoxFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);

                case 2:
                    return ((item as timetable).teachers.name.IndexOf(textBoxFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);

                case 3:
                    return ((item as timetable).disciplines.discipline_name.IndexOf(textBoxFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);

                case 4:
                    return ((item as timetable).lesson_number.ToString().IndexOf(textBoxFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);

                case 5:
                    return ((item as timetable).days_week.week_name.IndexOf(textBoxFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);

                case 6:
                    return ((item as timetable).week_account.ToString().IndexOf(textBoxFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);


                default:
                    return true;
            }
        }
    }
}
