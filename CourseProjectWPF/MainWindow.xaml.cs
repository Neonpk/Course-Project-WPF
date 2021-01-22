using CourseProjectWPF.Model;
using CourseProjectWPF.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace CourseProjectWPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private bool _isaboutshowing = false;

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool IsConnected { get; set; } = false;

        public bool IsAboutShowing
        {
            get
            {
                return _isaboutshowing;
            }

            set
            {
                if (_isaboutshowing != value)
                {
                    _isaboutshowing = value;
                    OnPropertyChanged();
                }

            }
        }

        public MainWindow()
        {
            InitializeComponent();

            Connection.Database = new Model.timetable_dbEntities2();
            IsConnected = Connection.Database.Database.Exists();

            IsAboutShowing = false;

            DataContext = this;

        }

        private void Button_Auth(object sender, RoutedEventArgs e)
        {

            Window window = null;

            if(radioButtonDispatcher.IsChecked.Value && passwordBox.Password == "dispatcher")
            {
                window = new DispatcherWindow();
                window.Show();

            }
            else if(radioButtonAdmin.IsChecked.Value && passwordBox.Password == "admin")
            {

                window = new AdminWindow();
                window.Show();
            }
            else
            {


                Watermark.Text = passwordBox.Password.Length > 0 
                    ? "Неверный пароль." : "Пароль не был введен.";

                passwordBox.Password = String.Empty;
                return;
            }

            this.Close();

        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            IsAboutShowing = !IsAboutShowing;
        }

        private void passwordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            Watermark.Visibility = passwordBox.Password.Length > 0 ? Visibility.Collapsed : Visibility.Visible;
        }

        private void radioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (Watermark == null) return;

            switch( (sender as RadioButton).Name )
            {
                case "radioButtonDispatcher":

                    Watermark.Text = "Ввод пароля диспетчера.";

                    break;

                case "radioButtonAdmin":

                    Watermark.Text = "Ввод пароля администратора";

                    break;
            }
        }

    }
}
