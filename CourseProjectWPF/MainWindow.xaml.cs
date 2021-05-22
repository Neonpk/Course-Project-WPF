using CourseProjectWPF.Model;
using CourseProjectWPF.ViewModel;
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

        public MainViewModel ViewModel { get; set; }

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

        private ICommand _radioButtonSelectCommand;
        public ICommand RadioButtonSelectCommand
        {
            get
            {
                return _radioButtonSelectCommand ??
                  (_radioButtonSelectCommand = new RelayCommand(obj =>
                  {
                      //if (Watermark == null) return;

                      switch (obj)
                      {
                          case "radioButtonDispatcher":

                              //Watermark.Text = "Ввод пароля диспетчера.";

                              ViewModel.SelectedViewModel = new EmployeeViewModel(IsConnected, "dispatcher", this);

                              break;

                          case "radioButtonAdmin":

                              //Watermark.Text = "Ввод пароля администратора";

                              ViewModel.SelectedViewModel = new EmployeeViewModel(IsConnected, "admin", this);

                              break;

                          case "radioButtonTeacher":

                              ViewModel.SelectedViewModel = new TeacherAuthViewModel(IsConnected, this);

                              break;
                      }

                  }, x => true));
            }
        }



        public MainWindow()
        {
            InitializeComponent();

            Connection.Database = new Model.timetable_dbEntities();
            IsConnected = Connection.Database.Database.Exists();

            IsAboutShowing = false;

            ViewModel = new MainViewModel();

            DataContext = this;


        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            IsAboutShowing = !IsAboutShowing;
        }

    }
}
