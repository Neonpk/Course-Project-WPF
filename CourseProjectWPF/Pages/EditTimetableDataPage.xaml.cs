using CourseProjectWPF.Model;
using CourseProjectWPF.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Логика взаимодействия для EditTimetableDataPage.xaml
    /// </summary>
    public partial class EditTimetableDataPage : Page
    {

        private DispatcherWindow dispatcherWindow = null;

        public timetable CurrentRow { get; set; }
        public timetable EditReferenceRow { get; set; }

        public List<groups> Groups { get; set; }
        public List<audiences> Audiences { get; set; }
        public List<teachers> Teachers { get; set; }
        public List<dir_lessons> Dir_Lessons { get; set; }
        public List<days_week> Days_Weeks { get; set; }

        public List<disciplines> Disciplines { get; set; }

        private int errorFieldCounter = 0;

        public EditTimetableDataPage(DispatcherWindow dispatcherWindow)
        {
            InitializeComponent();

            Groups = Connection.Database.groups.ToList();
            Audiences = Connection.Database.audiences.ToList();
            Teachers = Connection.Database.teachers.ToList();
            Disciplines = Connection.Database.disciplines.ToList();
            Dir_Lessons = Connection.Database.dir_lessons.ToList();
            Days_Weeks = Connection.Database.days_week.ToList();

            CurrentRow = new timetable();

            this.dispatcherWindow = dispatcherWindow;

            DataContext = this;

        }

        public EditTimetableDataPage(DispatcherWindow dispatcherWindow, timetable SelectedRow)
        {
            InitializeComponent();

            CurrentRow = (timetable)SelectedRow.Clone();
            EditReferenceRow = SelectedRow;

            Groups = Connection.Database.groups.ToList();
            Audiences = Connection.Database.audiences.ToList();
            Teachers = Connection.Database.teachers.ToList();
            Disciplines = Connection.Database.disciplines.ToList();
            Dir_Lessons = Connection.Database.dir_lessons.ToList();
            Days_Weeks = Connection.Database.days_week.ToList();

            this.dispatcherWindow = dispatcherWindow;

            DataContext = this;

        }


        private void CancelEdit_ButtonClick(object sender, RoutedEventArgs e)
        {
            this.dispatcherWindow.mainFrame.Navigate(new TimetableDataPage(this.dispatcherWindow));
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

                if (EditReferenceRow != null)
                {
                    EditReferenceRow.group_id = CurrentRow.group_id;
                    EditReferenceRow.groups = CurrentRow.groups;

                    EditReferenceRow.room = CurrentRow.room;
                    EditReferenceRow.audiences = CurrentRow.audiences;

                    EditReferenceRow.teacher_id = CurrentRow.teacher_id;
                    EditReferenceRow.teachers = CurrentRow.teachers;

                    EditReferenceRow.discipline_id = CurrentRow.discipline_id;
                    EditReferenceRow.disciplines = CurrentRow.disciplines;

                    EditReferenceRow.lesson_number = CurrentRow.lesson_number;
                    EditReferenceRow.dir_lessons = CurrentRow.dir_lessons;

                    EditReferenceRow.weekday_id = CurrentRow.weekday_id;
                    EditReferenceRow.days_week = CurrentRow.days_week;

                    EditReferenceRow.week_account = CurrentRow.week_account;

                }
                else
                {
                    Connection.Database.timetable.Add(CurrentRow);
                }

                Connection.Database.SaveChanges();

                dispatcherWindow.mainFrame.Navigate(new TimetableDataPage(dispatcherWindow));


            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Ошибка при сохранении", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }
    }
}
