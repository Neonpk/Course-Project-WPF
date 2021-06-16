using CourseProjectWPF.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
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

        private teachers _selectedTeacher;
        public teachers SelectedTeacher { get => _selectedTeacher; set { _selectedTeacher = value; OnPropertyChanged(nameof(SelectedTeacher)); } }

        private double _sumHours;
        public double SumHours { get => _sumHours; set { _sumHours = value; OnPropertyChanged(nameof(SumHours)); } }

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
                    SumHours = 0;

                    SelectedTeacher = null;

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

                    SumHours = 0;

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
                                x => x.timetable_id == item.timetable_id && x.teacher_id == SelectedTeacher.teacher_id && x.date >= DateFrom && item.date <= DateTo);

                            FillInforation(newItem);

                            if (newItem != null)
                                SumHours += 1.5;

                        }
                        else
                        {
                            if (item.teacher_id == SelectedTeacher.teacher_id && item.date >= DateFrom && item.date <= DateTo)
                            {
                                SumHours += 1.5;
                                FillInforation(item);
                            }

                        }
                    }


                }, x => true));
            }
        }

        private ICommand _buttonPrintCommand;
        public ICommand ButtonPrintCommand
        {
            get
            {
                return (_buttonPrintCommand ?? new RelayCommand(x =>
                {

                    // TODO: test

                    PrintDialog printDialog = new PrintDialog();

                    if (printDialog.ShowDialog() == true)
                    {
                        printDialog.PrintVisual((x as DataGrid), "test");
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
