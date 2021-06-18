using CourseProjectWPF.Model;
using CourseProjectWPF.Windows;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for MakeChangesTimetableDataPage.xaml
    /// </summary>
    public partial class MakeChangesTimetableDataPage : Page
    {
        private DispatcherWindow _dispatcherWindow = null;

        public timetable_changes CurrentRow { get; set; }

        public List<groups> Groups { get; set; }
        public List<audiences> Audiences { get; set; }
        public List<teachers> Teachers { get; set; }
        public List<dir_lessons> Dir_Lessons { get; set; }
        public List<changedescription> Changedescriptions { get; set; }

        public List<disciplines> Disciplines { get; set; }

        public Dictionary<string, bool> Evenweeks { get; set; } = new Dictionary<string, bool>
        {
            { "Четная", true },
            { "Нечетная", false }
        };

        private int errorFieldCounter = 0;

        public MakeChangesTimetableDataPage(DispatcherWindow dispatcherWindow, timetable SelectedRow)
        {


            InitializeComponent();

            Groups = Connection.Database.groups.ToList();
            Audiences = Connection.Database.audiences.ToList();
            Teachers = Connection.Database.teachers.ToList();
            Disciplines = Connection.Database.disciplines.ToList();
            Dir_Lessons = Connection.Database.dir_lessons.ToList();
            Changedescriptions = Connection.Database.changedescription.ToList();

            CurrentRow = new timetable_changes
            {
                timetable_id = SelectedRow.timetable_id,
                room = SelectedRow.room,
                teacher_id = SelectedRow.teacher_id,
                discipline_id = SelectedRow.teacher_id,
                lesson_number = SelectedRow.lesson_number,
                date = SelectedRow.date,
                group_id = SelectedRow.group_id,

                audiences = Audiences.FirstOrDefault(x => x.room == SelectedRow.room),
                teachers = Teachers.FirstOrDefault(x => x.teacher_id == SelectedRow.teacher_id),
                disciplines = Disciplines.FirstOrDefault(x => x.discipline_id == SelectedRow.discipline_id),
                dir_lessons = Dir_Lessons.FirstOrDefault(x=>x.lesson_number == SelectedRow.lesson_number),
                groups = Groups.FirstOrDefault(x=>x.group_id==SelectedRow.group_id)

            };

            this._dispatcherWindow = dispatcherWindow;

            DataContext = this;

        }


        private void CancelEdit_ButtonClick(object sender, RoutedEventArgs e)
        {
            this._dispatcherWindow.mainFrame.Navigate(new TimetableDataPage(this._dispatcherWindow));
        }

        private void textBoxWeek_account_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
            {
                var errorToolTip = new ToolTip();
                errorToolTip.Content = e.Error.ErrorContent;

                if (sender is TextBox)
                    (sender as TextBox).ToolTip = errorToolTip;
                else
                    (sender as ComboBox).ToolTip = errorToolTip;

                errorFieldCounter++;

            }

            if (e.Action == ValidationErrorEventAction.Removed)
            {
                errorFieldCounter--;
            }

            buttonSaveIntoDB.IsEnabled = errorFieldCounter == 0;
        }

        private void SaveIntoDatabase_ButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var cal = new GregorianCalendar();
                var weekNumber = cal.GetWeekOfYear(CurrentRow.date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

                CurrentRow.evenweek = weekNumber % 2 == 0;

                Connection.Database.timetable_changes.Add(CurrentRow);

                Connection.Database.SaveChanges();

                _dispatcherWindow.mainFrame.Navigate(new TimetableDataPage(_dispatcherWindow));

            }
            catch (Exception err)
            {
                Connection.Database.timetable_changes.Remove(CurrentRow);

                MessageBox.Show(err.InnerException.InnerException.Message, "Ошибка при сохранении", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
