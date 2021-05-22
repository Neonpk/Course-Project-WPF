using CourseProjectWPF.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CourseProjectWPF.Pages;

namespace CourseProjectWPF.ViewModel
{

    public class TimetableModel
    {
        public timetable Timetable { get; set; }
        public bool IsChanged { get; set; }
    }

    public class TeacherTimetableViewModel
    {

        public ObservableCollection<TimetableModel> Timetable { get; set; }
        public TimetableModel CurrentRow { get; set; }
        public teachers Teacher { get; set; }

        private Frame _frame;


        public TeacherTimetableViewModel(teachers teacher, Frame frame)
        {
            _frame = frame;

            this.Teacher = teacher;

            var timetable_changes = Connection.Database.timetable_changes.Select(x => x.timetable_id);

            this.Timetable = new ObservableCollection<TimetableModel>(
                Connection.Database.timetable.Where(x => x.teacher_id == teacher.teacher_id).Select(item => new TimetableModel
                {
                    Timetable = item,
                    IsChanged = timetable_changes.Contains(item.timetable_id)
                }));

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

        private ICommand _reloadDataCommand;
        public ICommand ReloadDataCommand
        {
            get
            {
                return _reloadDataCommand ?? new RelayCommand(obj =>
                {

                    foreach (var item in Connection.Database.timetable)
                    {
                        Connection.Database.Entry<timetable>(item).Reload();
                    }

                    foreach (var item in Connection.Database.timetable_changes)
                    {
                        Connection.Database.Entry<timetable_changes>(item).Reload();
                    }

                    var timetable_changes = Connection.Database.timetable_changes.Select(x => x.timetable_id);

                    Timetable.Clear();

                    foreach (var item in Connection.Database.timetable)
                    {
                        if (item.teacher_id != Teacher.teacher_id) continue;

                        this.Timetable.Add(new TimetableModel
                        {
                            Timetable = item,
                            IsChanged = timetable_changes.Contains(item.timetable_id)
                        });
                    }


                }, x => true);
            }
        }

        private ICommand _completeLessonCommand;
        public ICommand CompleteLessonCommand
        {
            get
            {
                return _completeLessonCommand ?? new RelayCommand(obj =>
                {

                    if (CurrentRow.Timetable.date > DateTime.Now)
                    {
                        MessageBox.Show("Вы не можете отметить занятие раньше времени.", "Предупреждение",
                            MessageBoxButton.OK, MessageBoxImage.Warning);

                        return;
                    }

                    CurrentRow.Timetable.completed = true;
                    Connection.Database.SaveChanges();

                    ReloadDataCommand.Execute(null);


                }, x => true);
            }
        }

        private ICommand _navigateToChangesCommand;
        public ICommand NavigateToChangesCommand
        {
            get
            {
                return _navigateToChangesCommand ?? new RelayCommand(obj =>
                {
                    _frame.Navigate(new TeacherTimetableChangesDataPage(new TeacherTimetableChangesViewModel(Teacher, CurrentRow.Timetable.timetable_id)));
                }, x => true);
            }
        }
    }
}
