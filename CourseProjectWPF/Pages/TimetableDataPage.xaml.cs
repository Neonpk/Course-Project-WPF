using CourseProjectWPF.Model;
using CourseProjectWPF.ViewModel;
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

    public class TimetableModel
    {
        public timetable Timetable { get; set; }
        public bool IsChanged { get; set; }
    }

    /// <summary>
    /// Логика взаимодействия для TimetableDataPage.xaml
    /// </summary>
    /// 
    public partial class TimetableDataPage : Page
    {

        public ObservableCollection<TimetableModel> Timetable { get; set; }
        public TimetableModel CurrentRow { get; set; }

        private DispatcherWindow _dispatcherWindow = null;

        private ICommand _navigateToChangesCommand;
        public ICommand NavigateToChangesCommand
        {
            get
            {
                return _navigateToChangesCommand ??
                  (_navigateToChangesCommand = new RelayCommand(obj =>
                  {
                      _dispatcherWindow.mainFrame.Navigate(new TimetableChangesDataPage(this._dispatcherWindow, CurrentRow.Timetable.timetable_id));

                  }, x => true));
            }
        }


        private Dictionary<string, string> _weeks = new Dictionary<string, string>
        {
            { "monday", "Понедельник" },
            { "tuesday", "Вторник" },
            { "wednesday", "Среда" },
            { "thursday", "Четверг" },
            { "friday", "Пятница" },
            { "saturday", "Суббота" },
            { "sunday", "Воскресенье" }
        };

        private string GetStringFromDate(DateTime date)
        {
            var week = "";

            bool condition = _weeks.TryGetValue(date.DayOfWeek.ToString().ToLower(), out week);

            return condition ? String.Format("{0} ({1})", date.ToString("dd.MM.yy"), week) : date.ToString("dd.MM.yy");
        }

        public TimetableDataPage(DispatcherWindow dispatcherWindow)
        {

            try
            {

                InitializeComponent();

                //this.Timetable = new ObservableCollection<TimetableModel>(Connection.Database.timetable.ToList());

                //GetTimeTable

                var timetable_changes = Connection.Database.timetable_changes.Select(x => x.timetable_id);

                this.Timetable = new ObservableCollection<TimetableModel>(Connection.Database.timetable.Select(item => new TimetableModel
                {
                    Timetable = item,
                    IsChanged = timetable_changes.Contains(item.timetable_id)
                }));

                // 

                this._dispatcherWindow = dispatcherWindow;

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
            this._dispatcherWindow.mainFrame.Navigate(new EditTimetableDataPage(this._dispatcherWindow));
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

                        Connection.Database.timetable.Remove(CurrentRow.Timetable);
                        Timetable.Remove(this.Timetable.FirstOrDefault(x=>x.Timetable.timetable_id == CurrentRow.Timetable.timetable_id));

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
            _dispatcherWindow.mainFrame.Navigate(new EditTimetableDataPage(this._dispatcherWindow, CurrentRow.Timetable));
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
                    return GetStringFromDate( (item as timetable).date ).IndexOf(textBoxFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0;

                case 6:
                    return System.Text.RegularExpressions.Regex.IsMatch(textBoxFilter.Text.ToLower(), 
                        ((item as timetable).evenweek ? "^Четная$" : "^Нечетная$").ToLower() );

                default:
                    return true;
            }
        }

        private void MakeChanges_ButtonClick(object sender, RoutedEventArgs e)
        {
            _dispatcherWindow.mainFrame.Navigate(new MakeChangesTimetableDataPage(this._dispatcherWindow, CurrentRow.Timetable));
        }
    }
}
