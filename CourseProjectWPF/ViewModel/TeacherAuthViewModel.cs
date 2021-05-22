using CourseProjectWPF.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CourseProjectWPF.ViewModel
{
    public class TeacherAuthViewModel : BaseViewModel
    {

        private Window _window;

        private ICommand _buttonAuthTeacherCommand;
        public ICommand ButtonAuthTeacherCommand
        {
            get
            {
                return _buttonAuthTeacherCommand ??
                    (_buttonAuthTeacherCommand = new RelayCommand(obj =>
                    {

                        var data = (object[])obj;

                        string login = (string)data[0];
                        string password = (data[1] as PasswordBox).Password;

                        var row = Connection.Database.teachers.FirstOrDefault(x => x.login == login && x.passwd == password);

                        if(row == null)
                        {
                            MessageBox.Show("Вы неверно ввели логин или пароль.", "Неудачный вход", MessageBoxButton.OK, MessageBoxImage.Stop);
                            return;
                        }

                        new TeacherWindow(new TeacherWindowViewModel(row)).Show();

                        _window.Close();

                    }, x => true));
            }
        }

        public bool IsConnected { get; set; }

        public TeacherAuthViewModel(bool isConnected, MainWindow window)
        {
            IsConnected = isConnected;
            _window = window;
        }

        private string _login = "";
        public string Login 
        { 
            get 
            {
                return _login; 
            }
          
            set {
                _login = value;
                OnPropertyChanged(nameof(Login));
            }
        }
    }
}
