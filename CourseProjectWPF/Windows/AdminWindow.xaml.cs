using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using CourseProjectWPF.Pages;

namespace CourseProjectWPF.Windows
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
        }

        private void ExitToMainWindow_ButtonClick(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();

            this.Close();
        }

        private void TreeViewItem_Selected(object sender, RoutedEventArgs e)
        {
            Page page = null;

            switch ((sender as TreeViewItem).Name)
            {
                case "treeViewItemRooms":

                    page = new RoomsDataPage(this);

                    break;

                case "treeViewItemFaculties":

                    page = new FacultiesDataPage(this);

                    break;

                case "treeViewItemSpecialties":

                    page = new SpecialtiesDataPage(this);

                    break;


                case "treeViewItemDepartments":

                    page = new DepartmentsDataPage(this);

                    break;

                case "treeViewItemDisciplines":

                    page = new DisciplinesDataPage(this);

                    break;

                case "treeViewItemChangesTimetableDesc":

                    page = new ChangeNotesDataPage(this);

                    break;

            }

            mainFrame.Navigate(page);
        }

        private void PageLoad_ButtonClick(object sender, RoutedEventArgs e)
        {

            Page page = null;

            switch((sender as Button).Name)
            {
                case "weeklyLoadButton":
                    page = new WeeklyloadingInfoPage();
                    break;
            }

            mainFrame.Navigate(page);

        }
    }

}
