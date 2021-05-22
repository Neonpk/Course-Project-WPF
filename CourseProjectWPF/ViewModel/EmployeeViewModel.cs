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
    public class EmployeeViewModel : BaseViewModel
    {
        public bool IsConnected { get; set; }

        private string _person;
        private Window _window;

        private ICommand _buttonAuthEmployeeCommand;
        public ICommand ButtonAuthEmployeeCommand
        {
            get
            {
                return _buttonAuthEmployeeCommand ??
                    (_buttonAuthEmployeeCommand = new RelayCommand(obj =>
                    {

                        var passwd = ((PasswordBox)obj);

                        if(_person == "dispatcher" && passwd.Password == "dispatcher")
                        {
                            new DispatcherWindow().Show();
                        }
                        else if (_person == "admin" && passwd.Password == "admin")
                        {
                            new AdminWindow().Show();
                        }
                        else
                        {

                            MessageBox.Show("Неверный пароль", "Неудачный вход", MessageBoxButton.OK, MessageBoxImage.Stop);

                            return;
                        }

                        _window.Close();

                    }, x => true));
            }
        }

        public EmployeeViewModel(bool isConnected, string person, MainWindow window)
        {
            IsConnected = isConnected;
            _person = person;
            _window = window;
        }

    }
}
