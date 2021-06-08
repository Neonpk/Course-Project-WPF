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

    public class TeacherWeeklyModel : BaseViewModel
    {

        private bool _cboxChecked;
        public bool CboxChecked { get => _cboxChecked; set { _cboxChecked = value; OnPropertyChanged(nameof(CboxChecked)); } }

        private DateTime _dateFrom;
        public DateTime DateFrom { get => _dateFrom; set { _dateFrom = value; OnPropertyChanged(nameof(DateFrom)); } }

        private DateTime _dateTo;
        public DateTime DateTo { get => _dateTo; set { _dateTo = value; OnPropertyChanged(nameof(DateTo)); } }

        private int _selectedTeacherid;
        public int SelectedTeacherId { get => _selectedTeacherid; set { _selectedTeacherid = value; OnPropertyChanged(nameof(SelectedTeacherId)); } }

        public List<teachers> Teachers { get; set; }

        public ObservableCollection<WeeklyModel> DataCollection { get; set; }

        private ICommand _buttonResetFiltersCommand;
        public ICommand ButtonResetFiltersCommand
        {
            get
            {
                return (_buttonResetFiltersCommand ?? new RelayCommand(obj =>
                {

                    CboxChecked = false;
                    DateFrom = DateTime.MinValue;
                    DateTo = DateTime.MinValue;

                    SelectedTeacherId = -1;

                    foreach (var item in DataCollection)
                        item.ClearFields();

                }, x => true));
            }
        }

        private ICommand _buttonSearchCommand;
        public ICommand ButtonSearchCommand
        {
            get
            {
                return (_buttonSearchCommand ?? new RelayCommand(obj =>
                {
                    // Clear fields

                    foreach (var item in DataCollection)
                        item.ClearFields();

                    // Inserting

                    var timetableChanges = Connection.Database.timetable_changes.Select(x => x.timetable_id);

                    foreach (timetable item in Connection.Database.timetable)
                    {
                        if (timetableChanges.Contains(item.timetable_id))
                        {
                            var newItem = Connection.Database.timetable_changes.FirstOrDefault(
                                x => x.timetable_id == item.timetable_id && x.teacher_id == SelectedTeacherId && x.date >= DateFrom && item.date <= DateTo);

                            FillInforation(newItem);
                        }
                        else
                        {
                            if (item.teacher_id == SelectedTeacherId && item.date >= DateFrom && item.date <= DateTo)
                                FillInforation(item);
                        }
                    }


                }, x => true));
            }
        }

        public void FillInforation(timetable item)
        {
            if (item is null)
                return;

            DataCollection[item.lesson_number - 1].Monday += ((int)item.date.DayOfWeek) == 1 ?
                String.Format("--\n Группа: {0} \n Аудитория: {1} ({2}) \n Дисциплина: {3}\n --", item.group_id, item.room, item.audiences.type, item.disciplines.discipline_name) : "";

            DataCollection[item.lesson_number - 1].Tuesday += ((int)item.date.DayOfWeek) == 2 ?
                String.Format("--\n Группа: {0} \n Аудитория: {1} ({2}) \n Дисциплина: {3}\n --", item.group_id, item.room, item.audiences.type, item.disciplines.discipline_name) : "";

            DataCollection[item.lesson_number - 1].Wednesday += ((int)item.date.DayOfWeek) == 3 ?
                String.Format("--\n Группа: {0} \n Аудитория: {1} ({2}) \n Дисциплина: {3}\n --", item.group_id, item.room, item.audiences.type, item.disciplines.discipline_name) : "";

            DataCollection[item.lesson_number - 1].Thursday += ((int)item.date.DayOfWeek) == 4 ?
                String.Format("--\n Группа: {0} \n Аудитория: {1} ({2}) \n Дисциплина: {3}\n --", item.group_id, item.room, item.audiences.type, item.disciplines.discipline_name) : "";

            DataCollection[item.lesson_number - 1].Friday += ((int)item.date.DayOfWeek) == 5 ?
                String.Format("--\n Группа: {0} \n Аудитория: {1} ({2}) \n Дисциплина: {3}\n --", item.group_id, item.room, item.audiences.type, item.disciplines.discipline_name) : "";

            DataCollection[item.lesson_number - 1].Saturday += ((int)item.date.DayOfWeek) == 6 ?
                String.Format("--\n Группа: {0} \n Аудитория: {1} ({2}) \n Дисциплина: {3}\n --", item.group_id, item.room, item.audiences.type, item.disciplines.discipline_name) : "";

            DataCollection[item.lesson_number - 1].Sunday += ((int)item.date.DayOfWeek) == 7 ?
                String.Format("--\n Группа: {0} \n Аудитория: {1} ({2}) \n Дисциплина: {3}\n --", item.group_id, item.room, item.audiences.type, item.disciplines.discipline_name) : "";
        }

        public void FillInforation(timetable_changes item)
        {

            if (item is null)
                return;

            DataCollection[item.lesson_number - 1].Monday += ((int)item.date.DayOfWeek) == 1 ?
                String.Format("--\n Группа: {0} \n Аудитория: {1} ({2}) \n Дисциплина: {3}\n --", item.group_id, item.room, item.audiences.type, item.disciplines.discipline_name) : "";

            DataCollection[item.lesson_number - 1].Tuesday += ((int)item.date.DayOfWeek) == 2 ?
                String.Format("--\n Группа: {0} \n Аудитория: {1} ({2}) \n Дисциплина: {3}\n --", item.group_id, item.room, item.audiences.type, item.disciplines.discipline_name) : "";

            DataCollection[item.lesson_number - 1].Wednesday += ((int)item.date.DayOfWeek) == 3 ?
                String.Format("--\n Группа: {0} \n Аудитория: {1} ({2}) \n Дисциплина: {3}\n --", item.group_id, item.room, item.audiences.type, item.disciplines.discipline_name) : "";

            DataCollection[item.lesson_number - 1].Thursday += ((int)item.date.DayOfWeek) == 4 ?
                String.Format("--\n Группа: {0} \n Аудитория: {1} ({2}) \n Дисциплина: {3}\n --", item.group_id, item.room, item.audiences.type, item.disciplines.discipline_name) : "";

            DataCollection[item.lesson_number - 1].Friday += ((int)item.date.DayOfWeek) == 5 ?
                String.Format("--\n Группа: {0} \n Аудитория: {1} ({2}) \n Дисциплина: {3}\n --", item.group_id, item.room, item.audiences.type, item.disciplines.discipline_name) : "";

            DataCollection[item.lesson_number - 1].Saturday += ((int)item.date.DayOfWeek) == 6 ?
                String.Format("--\n Группа: {0} \n Аудитория: {1} ({2}) \n Дисциплина: {3}\n --", item.group_id, item.room, item.audiences.type, item.disciplines.discipline_name) : "";

            DataCollection[item.lesson_number - 1].Sunday += ((int)item.date.DayOfWeek) == 7 ?
                String.Format("--\n Группа: {0} \n Аудитория: {1} ({2}) \n Дисциплина: {3}\n --", item.group_id, item.room, item.audiences.type, item.disciplines.discipline_name) : "";

        }

        public TeacherWeeklyModel()
        {
            Teachers = Connection.Database.teachers.ToList();

            DataCollection = new ObservableCollection<WeeklyModel>(new WeeklyModel[7].Select(x => new WeeklyModel { }));
        }
    }

}
