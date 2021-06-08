using CourseProjectWPF.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CourseProjectWPF.ViewModel
{

    public class GroupWeeklyViewModel : BaseViewModel
    {

        private bool _cboxChecked;
        public bool CboxChecked { get => _cboxChecked; set { _cboxChecked = value; OnPropertyChanged(nameof(CboxChecked)); } }

        private DateTime _dateFrom;
        public DateTime DateFrom { get => _dateFrom; set { _dateFrom = value; OnPropertyChanged(nameof(DateFrom)); } }

        private DateTime _dateTo;
        public DateTime DateTo { get => _dateTo; set { _dateTo = value; OnPropertyChanged(nameof(DateTo)); } }

        private int _selectedGroupId;
        public int SelectedGroupId { get => _selectedGroupId; set { _selectedGroupId = value; OnPropertyChanged(nameof(SelectedGroupId)); } }

        public List<groups> Groups { get; set; }

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
                    
                    SelectedGroupId = -1;

                    foreach (var item in DataCollection)
                        item.ClearFields();

                }, x => true));
            }
        }

        private ICommand _buttonRoomSearchCommand;
        public ICommand ButtonRoomSearchCommand
        {
            get
            {
                return (_buttonRoomSearchCommand ?? new RelayCommand(obj =>
                {
                    // Clear fields

                    foreach(var item in DataCollection)
                        item.ClearFields();

                    // Inserting

                    var timetableChanges = Connection.Database.timetable_changes.Select(x => x.timetable_id);

                    foreach (timetable item in Connection.Database.timetable)
                    {
                        if (timetableChanges.Contains(item.timetable_id))
                        {
                            var newItem = Connection.Database.timetable_changes.FirstOrDefault(
                                x => x.timetable_id == item.timetable_id && x.group_id == SelectedGroupId && x.date >= DateFrom && item.date <= DateTo);

                            FillInforation(newItem);
                        }
                        else
                        {
                            if(item.group_id == SelectedGroupId && item.date >= DateFrom && item.date <= DateTo)
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
                String.Format("--\n Преподаватель: {0} \n Кафедра: {1} \n Аудитория: {2} ({3}) \n Дисциплина: {4}\n --", 
                    item.teachers.name, item.teachers.departments.department_name, item.room, item.audiences.type, item.disciplines.discipline_name) : "";

            DataCollection[item.lesson_number - 1].Tuesday      += ((int)item.date.DayOfWeek) == 2 ?
                String.Format("--\n Преподаватель: {0} \n Кафедра: {1} \n Аудитория: {2} ({3}) \n Дисциплина: {4}\n --",
                    item.teachers.name, item.teachers.departments.department_name, item.room, item.audiences.type, item.disciplines.discipline_name) : "";

            DataCollection[item.lesson_number - 1].Wednesday    += ((int)item.date.DayOfWeek) == 3 ?
                String.Format("--\n Преподаватель: {0} \n Кафедра: {1} \n Аудитория: {2} ({3}) \n Дисциплина: {4}\n --",
                    item.teachers.name, item.teachers.departments.department_name, item.room, item.audiences.type, item.disciplines.discipline_name) : "";

            DataCollection[item.lesson_number - 1].Thursday     += ((int)item.date.DayOfWeek) == 4 ?
                String.Format("--\n Преподаватель: {0} \n Кафедра: {1} \n Аудитория: {2} ({3}) \n Дисциплина: {4}\n --",
                    item.teachers.name, item.teachers.departments.department_name, item.room, item.audiences.type, item.disciplines.discipline_name) : "";

            DataCollection[item.lesson_number - 1].Friday       += ((int)item.date.DayOfWeek) == 5 ?
                String.Format("--\n Преподаватель: {0} \n Кафедра: {1} \n Аудитория: {2} ({3}) \n Дисциплина: {4}\n --",
                    item.teachers.name, item.teachers.departments.department_name, item.room, item.audiences.type, item.disciplines.discipline_name) : "";


            DataCollection[item.lesson_number - 1].Saturday     += ((int)item.date.DayOfWeek) == 6 ?
                String.Format("--\n Преподаватель: {0} \n Кафедра: {1} \n Аудитория: {2} ({3}) \n Дисциплина: {4}\n --",
                    item.teachers.name, item.teachers.departments.department_name, item.room, item.audiences.type, item.disciplines.discipline_name) : "";

            DataCollection[item.lesson_number - 1].Sunday       += ((int)item.date.DayOfWeek) == 7 ?
                String.Format("--\n Преподаватель: {0} \n Кафедра: {1} \n Аудитория: {2} ({3}) \n Дисциплина: {4}\n --",
                    item.teachers.name, item.teachers.departments.department_name, item.room, item.audiences.type, item.disciplines.discipline_name) : "";
        }

        public void FillInforation(timetable_changes item)
        {

            if (item is null)
                return;

            DataCollection[item.lesson_number - 1].Monday += ((int)item.date.DayOfWeek) == 1 ?
                String.Format("--\n Преподаватель: {0} \n Кафедра: {1} \n Аудитория: {2} ({3}) \n Дисциплина: {4}\n --",
                    item.teachers.name, item.teachers.departments.department_name, item.room, item.audiences.type, item.disciplines.discipline_name) : "";

            DataCollection[item.lesson_number - 1].Tuesday += ((int)item.date.DayOfWeek) == 2 ?
                String.Format("--\n Преподаватель: {0} \n Кафедра: {1} \n Аудитория: {2} ({3}) \n Дисциплина: {4}\n --",
                    item.teachers.name, item.teachers.departments.department_name, item.room, item.audiences.type, item.disciplines.discipline_name) : "";

            DataCollection[item.lesson_number - 1].Wednesday += ((int)item.date.DayOfWeek) == 3 ?
                String.Format("--\n Преподаватель: {0} \n Кафедра: {1} \n Аудитория: {2} ({3}) \n Дисциплина: {4}\n --",
                    item.teachers.name, item.teachers.departments.department_name, item.room, item.audiences.type, item.disciplines.discipline_name) : "";

            DataCollection[item.lesson_number - 1].Thursday += ((int)item.date.DayOfWeek) == 4 ?
                String.Format("--\n Преподаватель: {0} \n Кафедра: {1} \n Аудитория: {2} ({3}) \n Дисциплина: {4}\n --",
                    item.teachers.name, item.teachers.departments.department_name, item.room, item.audiences.type, item.disciplines.discipline_name) : "";

            DataCollection[item.lesson_number - 1].Friday += ((int)item.date.DayOfWeek) == 5 ?
                String.Format("--\n Преподаватель: {0} \n Кафедра: {1} \n Аудитория: {2} ({3}) \n Дисциплина: {4}\n --",
                    item.teachers.name, item.teachers.departments.department_name, item.room, item.audiences.type, item.disciplines.discipline_name) : "";


            DataCollection[item.lesson_number - 1].Saturday += ((int)item.date.DayOfWeek) == 6 ?
                String.Format("--\n Преподаватель: {0} \n Кафедра: {1} \n Аудитория: {2} ({3}) \n Дисциплина: {4}\n --",
                    item.teachers.name, item.teachers.departments.department_name, item.room, item.audiences.type, item.disciplines.discipline_name) : "";

            DataCollection[item.lesson_number - 1].Sunday += ((int)item.date.DayOfWeek) == 7 ?
                String.Format("--\n Преподаватель: {0} \n Кафедра: {1} \n Аудитория: {2} ({3}) \n Дисциплина: {4}\n --",
                    item.teachers.name, item.teachers.departments.department_name, item.room, item.audiences.type, item.disciplines.discipline_name) : "";
        }


        public GroupWeeklyViewModel()
        {
            Groups = Connection.Database.groups.ToList();
            DataCollection = new ObservableCollection<WeeklyModel>(new WeeklyModel[7].Select(x => new WeeklyModel { }));
        }

    }
}
