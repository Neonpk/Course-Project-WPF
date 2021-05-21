using CourseProjectWPF.Model;
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
    /// Interaction logic for TimetableChangesDataPage.xaml
    /// </summary>
    public partial class TimetableChangesDataPage : Page
    {

        public int TimetableId { get; set; }
        public List<timetable_changesview> TimetableChangesView { get; set; }


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

        public TimetableChangesDataPage(Window owner)
        {
            try
            {
                InitializeComponent();


                TimetableChangesView = Connection.Database.timetable_changesview.ToList();

                DataContext = this;

            }
            catch (EntityException err)
            {
                MessageBox.Show(err.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                MainWindow mainWindow = new MainWindow();
                mainWindow.IsConnected = false;
                mainWindow.Show();

                owner.Close();

            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public TimetableChangesDataPage(Window owner, int timetableId)
        {
            try
            {
                InitializeComponent();


                TimetableChangesView = Connection.Database.timetable_changesview.ToList();


                this.TimetableId = timetableId;

                DataContext = this;


            }
            catch (EntityException err)
            {
                MessageBox.Show(err.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                MainWindow mainWindow = new MainWindow();
                mainWindow.IsConnected = false;
                mainWindow.Show();

                owner.Close();

            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void RefreshTeacherInfo_ButtonClick(object sender, RoutedEventArgs e)
        {
            foreach (var item in Connection.Database.timetable_changesview)
            {
                Connection.Database.Entry<timetable_changesview>(item).Reload();
            }

            DataContext = null;
            DataContext = this;
        }

        private bool UserFilter(object item)
        {
            if (String.IsNullOrEmpty(textBoxFilter.Text))
                return true;

            switch (comboboxFilterType.SelectedIndex)
            {
                case 0:
                    return ((item as timetable_changesview).group_id.ToString().IndexOf(textBoxFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);

                case 1:
                    return ((item as timetable_changesview).room.ToString().IndexOf(textBoxFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);

                case 2:
                    return ((item as timetable_changesview).teacher_name.IndexOf(textBoxFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);

                case 3:
                    return ((item as timetable_changesview).discipline_name.IndexOf(textBoxFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);

                case 4:
                    return ((item as timetable_changesview).lesson_number.IndexOf(textBoxFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);

                case 5:
                    return GetStringFromDate((item as timetable_changesview).date).IndexOf(textBoxFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0;

                case 6:
                    return System.Text.RegularExpressions.Regex.IsMatch(textBoxFilter.Text.ToLower(),
                        String.Format("^{0}$", (item as timetable_changesview).evenweek.ToLower()));

                case 7:
                    return ((item as timetable_changesview).description.IndexOf(textBoxFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);


                default:
                    return true;
            }
        }

        private void textBoxFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(dataGridTimetable.ItemsSource).Refresh();
        }

        private void dataGridTimetable_Loaded(object sender, RoutedEventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(dataGridTimetable.ItemsSource);
            view.Filter = UserFilter;

            dataGridTimetable.SelectedValue = this.TimetableId;
        }

        private void RemoveRow_ButtonClick(object sender, RoutedEventArgs e)
        {

            try
            {

                if (Connection.Database.timetable_changes.Any(x=>x.timetable_id == TimetableId))
                {

                    var messageBoxResult = MessageBox.Show("Вы действительно удалить данную запись?", "Подтверждение действия",
                        MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (messageBoxResult == MessageBoxResult.Yes)
                    {

                        Connection.Database.timetable_changes.Remove(Connection.Database.timetable_changes.FirstOrDefault(x => x.timetable_id == TimetableId));
                        Connection.Database.SaveChanges();

                        TimetableChangesView.Clear();
                        TimetableChangesView = Connection.Database.timetable_changesview.ToList();
                        RefreshTeacherInfo_ButtonClick(null, null);


                    }

                }
                else
                {
                    MessageBox.Show("Запись не выбрана.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Ошибка обновления", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

    }
}
