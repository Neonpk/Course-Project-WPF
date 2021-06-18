using CourseProjectWPF.Model;
using CourseProjectWPF.ViewModel;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for TeacherTimetableDataPage.xaml
    /// </summary>
    public partial class TeacherTimetableDataPage : Page
    {
        public TeacherTimetableDataPage(TeacherTimetableViewModel teacherTimetableViewModel)
        {
            InitializeComponent();
            DataContext = teacherTimetableViewModel;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(dataGridTimetable.ItemsSource).Refresh();
        }

        private void dataGridTimetable_Loaded(object sender, RoutedEventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(dataGridTimetable.ItemsSource);
            view.Filter = UserFilter;
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

        private bool UserFilter(object item)
        {

            if (String.IsNullOrEmpty(textBoxFilter.Text))
                return true;

            switch (comboboxFilterType.SelectedIndex)
            {
                case 0:
                    return ((item as CourseProjectWPF.ViewModel.TimetableModel).Timetable.group_id.ToString().IndexOf(textBoxFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);

                case 1:
                    return System.Text.RegularExpressions.Regex.IsMatch(textBoxFilter.Text.ToLower(),
                        ((item as CourseProjectWPF.ViewModel.TimetableModel).Timetable.evenweek ? "^Четная$" : "^Нечетная$").ToLower());

                case 2:
                    return GetStringFromDate((item as CourseProjectWPF.ViewModel.TimetableModel).Timetable.date).IndexOf(textBoxFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0;

                case 3:
                    return ((item as CourseProjectWPF.ViewModel.TimetableModel).Timetable.room.ToString().IndexOf(textBoxFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);

                case 4:
                    return ((item as CourseProjectWPF.ViewModel.TimetableModel).Timetable.disciplines.discipline_name.IndexOf(textBoxFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);

                case 5:
                    return ((item as CourseProjectWPF.ViewModel.TimetableModel).Timetable.lesson_number.ToString().IndexOf(textBoxFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);


                default:
                    return true;
            }
        }

    }
}
