using CourseProjectWPF.Model;
using CourseProjectWPF.Windows;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CourseProjectWPF.Pages
{
    /// <summary>
    /// Interaction logic for TeacherChangePasswordPage.xaml
    /// </summary>
    public partial class TeacherChangePasswordPage : Page
    {

        public string Login { get; set; }
        public string Passwd { get; set; }

        private TeacherWindow _teacherWindow;
        private teachers _teacher;

        public TeacherChangePasswordPage(teachers Teacher, TeacherWindow teacherWindow)
        {
            InitializeComponent();

            Login = Teacher.login;
            Passwd = Teacher.passwd;

            passwordBox.Password = Passwd;

            _teacherWindow = teacherWindow;
            _teacher = Teacher;

            DataContext = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this._teacherWindow.mainFrame.Navigate(new TeacherTimetableDataPage(new ViewModel.TeacherTimetableViewModel(_teacher, _teacherWindow.mainFrame)));
        }

        private void buttonSaveIntoDB_Click(object sender, RoutedEventArgs e)
        {

            if(passwordBox.Password.Length < 3)
            {
                MessageBox.Show("Пароль должен быть от 3 символов и выше.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _teacher.passwd = passwordBox.Password;

            Connection.Database.SaveChanges();

            MessageBox.Show("Пароль успешно изменен", "Успешно", 
                MessageBoxButton.OK, MessageBoxImage.Information);

            this._teacherWindow.mainFrame.Navigate(new TeacherTimetableDataPage(new ViewModel.TeacherTimetableViewModel(_teacher, _teacherWindow.mainFrame)));

        }
    }
}
