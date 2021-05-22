using CourseProjectWPF.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CourseProjectWPF.ViewModel
{
    public class TeacherTimetableChangesViewModel
    {

        public ObservableCollection<timetable_changesview> TimetableChangesView { get; set; }

        public teachers Teacher { get; set; }
        
        public int TimetableId { get; set; }

        public TeacherTimetableChangesViewModel(teachers teacher)
        {
            this.Teacher = teacher;

            this.TimetableChangesView = new ObservableCollection<timetable_changesview>(
                Connection.Database.timetable_changesview.Where(x => x.teacher_id == teacher.teacher_id));
        }

        public TeacherTimetableChangesViewModel(teachers teacher, int timetableId)
        {
            this.Teacher = teacher;

            this.TimetableId = timetableId;

            this.TimetableChangesView = new ObservableCollection<timetable_changesview>(
                Connection.Database.timetable_changesview.Where(x => x.teacher_id == teacher.teacher_id));
        }

        private ICommand _reloadDataCommand;
        public ICommand ReloadDataCommand
        {
            get
            {
                return _reloadDataCommand ?? new RelayCommand(obj =>
                {

                    foreach (var item in Connection.Database.timetable_changesview)
                    {
                        Connection.Database.Entry<timetable_changesview>(item).Reload();
                    }

                    TimetableChangesView.Clear();

                    foreach (var item in Connection.Database.timetable_changesview)
                    {
                        if (item.teacher_id != Teacher.teacher_id) continue;

                        this.TimetableChangesView.Add(item);
                    }


                }, x => true);
            }
        }



    }
}
