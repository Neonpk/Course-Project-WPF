using CourseProjectWPF.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CourseProjectWPF.ViewModel
{ 

    public class TeacherWindowViewModel
    {

        public teachers Teacher { get; set; }

        private ICommand _exitCommand;
        public ICommand ExitCommand
        {
            get
            {
                return _exitCommand ?? new RelayCommand(obj =>
                {

                    new MainWindow().Show();

                    (obj as Window).Close();

                }, x => true);
            }
        }

        public TeacherWindowViewModel(teachers teacher)
        {
            Teacher = teacher;
        }

    }
}
