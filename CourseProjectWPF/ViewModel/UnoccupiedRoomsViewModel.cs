using CourseProjectWPF.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace CourseProjectWPF.ViewModel
{

    public class RoomRow
    {
        public int LessonNumber { get; set; }
        public int Room { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

    }


    public class UnoccupiedRoomsViewModel
    {
        public ICollectionView UnoccupiedRoomsSource { get; set; }


        public ObservableCollection<RoomRow> UnoccupiedRooms { get; set; }
        public List<audiences> Audiences { get; set; }

        public DateTime SelectedDate { get; set; }
        public audiences SelectedRoom { get; set; }

        public UnoccupiedRoomsViewModel()
        {
            UnoccupiedRooms = new ObservableCollection<RoomRow>();

            GetUnoccupiedRooms();

            Audiences = Connection.Database.audiences.ToList();

            UnoccupiedRoomsSource = CollectionViewSource.GetDefaultView(UnoccupiedRooms);
        }

        private ICommand _buttonRoomSearchCommand;
        public ICommand ButtonRoomSearchCommand
        {
            get
            {
                return _buttonRoomSearchCommand ??
                    (_buttonRoomSearchCommand = new RelayCommand(obj =>
                    {

                        UnoccupiedRoomsSource.Filter =
                            w => (w as RoomRow).Date == SelectedDate && (w as RoomRow).Room == SelectedRoom.room;

                    }, x => true));
            }
        }

        private ICommand _buttonResetFiltersCommand;
        public ICommand ButtonResetFiltersCommand
        {
            get
            {
                return _buttonResetFiltersCommand ??
                    (_buttonResetFiltersCommand = new RelayCommand(obj =>
                    {

                        UnoccupiedRoomsSource.Filter = null;
                        UnoccupiedRoomsSource.Refresh();

                    }, x => true));
            }
        }

        private ICommand _buttonRefreshDataCommand;
        public ICommand ButtonRefreshDataCommand
        {
            get
            {
                return _buttonRefreshDataCommand ??
                    (_buttonRefreshDataCommand = new RelayCommand(obj =>
                    {
                        GetUnoccupiedRooms();
                    }, x => true));
            }
        }

        public void GetUnoccupiedRooms()
        {

            if (UnoccupiedRooms == null) return;

            UnoccupiedRooms.Clear();

            var timetableChanges = Connection.Database.timetable_changes.Select(x => x.timetable_id);

            foreach (var row in Connection.Database.timetable)
            {

                if (timetableChanges.Contains(row.timetable_id))
                {

                    var changedRow = Connection.Database.timetable_changes.FirstOrDefault(x => x.timetable_id == row.timetable_id);


                    if (UnoccupiedRooms.Any(x => x.Date == changedRow.date && x.Room == changedRow.room))
                        continue;

                    var result = Connection.Database.Database.SqlQuery<RoomRow>(String.Format("SELECT * FROM dbo.GetChangesUnoccupiedRooms('{0}', {1})", changedRow.date, changedRow.room));

                    if (result != null)
                    {
                        foreach (var resultRow in result)
                        {
                            UnoccupiedRooms.Add(resultRow);
                        }
                    }

                }
                else
                {

                    if (UnoccupiedRooms.Any(x => x.Date == row.date && x.Room == row.room))
                        continue;

                    var result = Connection.Database.Database.SqlQuery<RoomRow>(String.Format("SELECT * FROM dbo.GetUnoccupiedRooms('{0}', {1})", row.date, row.room));

                    if (result != null)
                    {
                        foreach (var resultRow in result)
                        {
                            UnoccupiedRooms.Add(resultRow);
                        }
                    }


                }

            }

        }


    }
}
