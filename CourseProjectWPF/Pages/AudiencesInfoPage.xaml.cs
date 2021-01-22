using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Security.Policy;
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

namespace CourseProjectWPF.Pages
{
    /// <summary>
    /// Логика взаимодействия для AudiencesInfoPage.xaml
    /// </summary>
    public partial class AudiencesInfoPage : Page
    {
        public AudiencesInfoPage(Window owner)
        {

            try
            {
                InitializeComponent();
                DataContext = Connection.Database.audience_loading.ToList();

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

        private void RefreshTeacherInfo_ButtonClick(object sender, RoutedEventArgs e)
        {
            foreach (var item in Connection.Database.audience_loading)
            {
                Connection.Database.Entry<audience_loading>(item).Reload();
            }

            DataContext = Connection.Database.audience_loading.ToList();
        }

        private bool UserFilter(object item)
        {
            if (String.IsNullOrEmpty(textBoxFilter.Text))
                return true;

            switch (comboboxFilterType.SelectedIndex)
            {
                case 0:
                    return ((item as audience_loading).discipline_name.IndexOf(textBoxFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);

                case 1:
                    return ((item as audience_loading).room.ToString().IndexOf(textBoxFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);

                case 2:
                    return ((item as audience_loading).lesson_number.ToString().IndexOf(textBoxFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);

                case 3:
                    return ((item as audience_loading).week_name.IndexOf(textBoxFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);

                case 4:
                    return ((item as audience_loading).week_account.ToString().IndexOf(textBoxFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);

                case 5:
                    return ((item as audience_loading).teacher_name.IndexOf(textBoxFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);

                case 6:
                    return ((item as audience_loading).department_name.IndexOf(textBoxFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);

                case 7:
                    return ((item as audience_loading).position_name.IndexOf(textBoxFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);

                case 8:
                    return ((item as audience_loading).rank_name.IndexOf(textBoxFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);

                case 9:
                    return ((item as audience_loading).group_id.ToString().IndexOf(textBoxFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);

                case 10:
                    return ((item as audience_loading).group_count_people.ToString().IndexOf(textBoxFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);
                
                case 11:
                    return ((item as audience_loading).specialty_name.ToString().IndexOf(textBoxFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);

                case 12:
                    return ((item as audience_loading).faculty_name.ToString().IndexOf(textBoxFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);

                case 13:
                    return ((item as audience_loading).train_type_name.ToString().IndexOf(textBoxFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);



                default:
                    return true;
            }
        }

        private void textBoxFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(dataGridTimetable.ItemsSource).Refresh();
        }

        private void dataGridTimetable_Loaded(object sender, RoutedEventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(dataGridTimetable.ItemsSource);
            view.Filter = UserFilter;
        }
    }
}
