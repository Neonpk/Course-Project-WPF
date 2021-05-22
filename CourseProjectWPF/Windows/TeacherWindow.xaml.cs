using CourseProjectWPF.Pages;
using CourseProjectWPF.ViewModel;
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
    /// <summary>
    /// Interaction logic for TeacherWindow.xaml
    /// </summary>
    public partial class TeacherWindow : Window
    {
        public TeacherWindow(TeacherWindowViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void treeViewItemTimetable_Selected(object sender, RoutedEventArgs e)
        {
            Page page = null;

            switch ((sender as TreeViewItem).Name)
            {
                case "treeViewItemTimetable":

                    page = new TeacherTimetableDataPage(new TeacherTimetableViewModel((this.DataContext as TeacherWindowViewModel).Teacher, mainFrame));

                    break;

                case "treeViewItemTimetableChanges":

                    page = new TeacherTimetableChangesDataPage(new TeacherTimetableChangesViewModel((this.DataContext as TeacherWindowViewModel).Teacher));

                    break;
            }


            mainFrame.Navigate(page);
        }
    }
}
