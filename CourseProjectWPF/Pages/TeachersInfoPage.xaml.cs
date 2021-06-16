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
using CourseProjectWPF.Model;
using System.ComponentModel;

namespace CourseProjectWPF.Pages
{

    public class TeacherModel : CourseProjectWPF.ViewModel.BaseViewModel
    {
        public teachers Teacher { get; set; }

        public double _hours;
        public double Hours { get => _hours; set { _hours = value; OnPropertyChanged(nameof(Hours)); } }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    /// <summary>
    /// Логика взаимодействия для TeachersInfoPage.xaml
    /// </summary>
    public partial class TeachersInfoPage : Page
    {

        public ObservableCollection<TeacherModel> Teachers { get; set; }

        public double GetHours(int teacherId)
        {
            //shitcode

            var list = new List<int>();

            var ch = Connection.Database.timetable_changes.Select(x=>x.timetable_id);

            foreach(var t in Connection.Database.timetable)
            {
                if (ch.Contains(t.timetable_id) && Connection.Database.timetable_changes.FirstOrDefault(x => x.teacher_id == teacherId 
                    && x.timetable_id == t.timetable_id) != null)

                    list.Add(t.timetable_id);
                else if (t.teacher_id == teacherId && !ch.Contains(t.timetable_id))
                    list.Add(t.timetable_id);
            }

            return list.Count * 1.5;

        }

        public double GetHours(int teacherId, DateTime start, DateTime finish)
        {

            var list = new List<int>();

            var ch = Connection.Database.timetable_changes.Select(x => x.timetable_id);

            foreach (var t in Connection.Database.timetable)
            {
                if (ch.Contains(t.timetable_id) && Connection.Database.timetable_changes.FirstOrDefault(x => x.teacher_id == teacherId
                    && x.timetable_id == t.timetable_id) is var sb && sb != null && sb.date >= start && sb.date <= finish )
                    list.Add(t.timetable_id);
                else if (t.teacher_id == teacherId && t.date >= start && t.date <= finish && !ch.Contains(t.timetable_id))
                    list.Add(t.timetable_id);
            }

            return list.Count * 1.5;

        }
        
        public TeachersInfoPage(Window owner)
        {

            try
            {
                InitializeComponent();

                Teachers = new ObservableCollection<TeacherModel>(Connection.Database.teachers.ToList().Select(x => new TeacherModel
                {
                    Teacher = x,
                    Hours = GetHours(x.teacher_id)
                }));

                DataContext = this;

            }
            catch(EntityException err)
            {
                MessageBox.Show(err.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                MainWindow mainWindow = new MainWindow();
                mainWindow.IsConnected = false;
                mainWindow.Show();

                owner.Close();

            }catch(Exception err)
            {
                MessageBox.Show(err.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private bool UserFilter(object item)
        {
            if (String.IsNullOrEmpty(textBoxFilter.Text))
                return true;

            switch (comboboxFilterType.SelectedIndex)
            {
                case 0:
                    return ((item as teachers_hours).teacherName.IndexOf(textBoxFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);

                case 1:
                    return ((item as teachers_hours).teacher_rankName.IndexOf(textBoxFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);

                case 2:
                    return ((item as teachers_hours).teacher_departmentName.IndexOf(textBoxFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);

                case 3:
                    return ((item as teachers_hours).teacher_positionName.IndexOf(textBoxFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);

                case 4:
                    return ((item as teachers_hours).teacher_workhours.ToString().IndexOf(textBoxFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);

                default:
                    return true;
            }
        }

        private void textBoxFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView( (listBoxTeachersInfo.ItemsSource as ObservableCollection<teachers>) ).Refresh();
        }

        private void RefreshTeacherInfo_ButtonClick(object sender, RoutedEventArgs e)
        {
            foreach (var item in Connection.Database.teachers_hours)
            {
                Connection.Database.Entry<teachers_hours>(item).Reload();
            }

            DataContext = Connection.Database.teachers_hours.ToList();
        }

        private void CollectionViewSource_Filter(object sender, FilterEventArgs e)
        {
            e.Accepted = UserFilter(e.Item);
        }

        private void comboboxFilterSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboboxFilterSortFieldType == null) return;

            CollectionViewSource cvs = FindResource("collectionViewSource") as CollectionViewSource;

            cvs.SortDescriptions.Clear();

            cvs.SortDescriptions.Add(new SortDescription(
                (comboboxFilterSortFieldType.SelectedItem as ComboBoxItem).Tag as string,
                (ListSortDirection)comboboxFilterSortModeType.SelectedIndex
            ));
            
        }

        private void CalculateTeacherHours_Command(object sender, RoutedEventArgs e)
        {
            var selectedItem = ((listBoxTeachersInfo.ItemsSource as ICollectionView).CurrentItem) as TeacherModel;

            selectedItem.Hours = GetHours(selectedItem.Teacher.teacher_id, selectedItem.StartDate, selectedItem.EndDate);

        }


    }
}
