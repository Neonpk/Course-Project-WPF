using CourseProjectWPF.Model;
using CourseProjectWPF.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.Xml;
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

namespace CourseProjectWPF.Pages
{
    /// <summary>
    /// Логика взаимодействия для EditTeacherDataPage.xaml
    /// </summary>
    public partial class EditTeacherDataPage : Page
    {

        private int errorFieldCounter = 0;

        public DispatcherWindow dispatcherWindow = null;

        public teachers CurrentTeacher { get; set; }
        public List<positions> Positions { get; set; }
        public List<ranks> Ranks { get; set; }
        public List<departments> Departments { get; set; }

        private teachers EditTeacherReference { get; set; }

        public EditTeacherDataPage(DispatcherWindow ownerWindow, teachers teacher)
        {
            InitializeComponent();

            this.dispatcherWindow = ownerWindow;

            CurrentTeacher = (teachers)teacher.Clone();
            EditTeacherReference = teacher;

            Positions   = Connection.Database.positions.ToList();
            Ranks       = Connection.Database.ranks.ToList();
            Departments = Connection.Database.departments.ToList();

            DataContext = this;

        }

        public EditTeacherDataPage(DispatcherWindow ownerWindow)
        {
            InitializeComponent();

            this.dispatcherWindow = ownerWindow;

            CurrentTeacher = new teachers { 
                name = "Иванов Иван Иванович" 
            };

            Positions = Connection.Database.positions.ToList();
            Ranks = Connection.Database.ranks.ToList();
            Departments = Connection.Database.departments.ToList();

            DataContext = this;

        }


        private void SaveIntoDataBase_ButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {

                if (EditTeacherReference != null)
                {
                    EditTeacherReference.name = CurrentTeacher.name;
                    EditTeacherReference.department_id = CurrentTeacher.department_id;
                    EditTeacherReference.rank_id = CurrentTeacher.rank_id;
                    EditTeacherReference.position_id = CurrentTeacher.position_id;
                    EditTeacherReference.positions = CurrentTeacher.positions;
                    EditTeacherReference.ranks = CurrentTeacher.ranks;
                    EditTeacherReference.departments = CurrentTeacher.departments;
                }
                else
                {
                    Connection.Database.teachers.Add(CurrentTeacher);
                }

                Connection.Database.SaveChanges();

                dispatcherWindow.mainFrame.Navigate(new TeachersDataPage(dispatcherWindow));


            }catch(Exception err)
            {
                MessageBox.Show(err.Message, "Ошибка при сохранении", MessageBoxButton.OK, MessageBoxImage.Error);
                Connection.Database.teachers.Remove(CurrentTeacher);

            }
        }

        private void textBoxFullname_Error(object sender, ValidationErrorEventArgs e)
        {

            if( e.Action == ValidationErrorEventAction.Added )
            {
                var errorToolTip = new ToolTip();
                errorToolTip.Content = e.Error.ErrorContent;

                if (sender is TextBox)
                    (sender as TextBox).ToolTip = errorToolTip;
                else
                    (sender as ComboBox).ToolTip = errorToolTip;

                errorFieldCounter++;

            }

            if( e.Action == ValidationErrorEventAction.Removed )
            {
                errorFieldCounter--;
            }


            buttonSaveIntoDB.IsEnabled  = errorFieldCounter == 0;

        }

        private void Cancel_ButtonClick(object sender, RoutedEventArgs e)
        {
            this.dispatcherWindow.mainFrame.Navigate(new TeachersDataPage(this.dispatcherWindow));
        }
    }
}
