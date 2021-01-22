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

namespace CourseProjectWPF.Windows
{

    using Pages;

    /// <summary>
    /// Логика взаимодействия для DispatcherWindow.xaml
    /// </summary>
    public partial class DispatcherWindow : Window
    {
        public DispatcherWindow()
        {
            InitializeComponent();
        }

        private void ExitToMainWindow_ButtonClick(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();

            this.Close();
        }

        private void NavigateFrame_ButtonClick(object sender, RoutedEventArgs e)
        {
            Page page = null;

            switch( (sender as Button).Name )
            {
                case "buttonTeachersInfo":

                    page = new TeachersInfoPage(this);

                    break;

                case "buttonAudincesInfo":

                    page = new AudiencesInfoPage(this);

                    break;

            }
            mainFrame.Navigate(page);
        }

        private void TreeViewItem_Selected(object sender, RoutedEventArgs e)
        {
            Page page = null;

            switch ((sender as TreeViewItem).Name)
            {
                case "treeViewItemTeachers":

                    page = new TeachersDataPage(this);

                    break;

                case "treeViewItemGroups":

                    page = new GroupsDataPage(this);

                    break;

                case "treeViewItemTimetable":

                    page = new TimetableDataPage(this);

                    break;

            }
            mainFrame.Navigate(page);
        }
    }
}
