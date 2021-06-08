using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CourseProjectWPF.ViewModel
{

    public class WeeklyloadingViewModel
    {
        public MainViewModel ViewModel { get; set; }

        public WeeklyloadingViewModel()
        {
            ViewModel = new MainViewModel();
        }

        private ICommand _comboboxSelectedItemCommand;
        public ICommand ComboboxSelectedItemCommand
        {
            get
            {
                return (_comboboxSelectedItemCommand ?? new RelayCommand(param =>
                {
                    switch ((string)param)
                    {
                        case "onGroup":
                            ViewModel.SelectedViewModel = new GroupWeeklyViewModel();
                            break;

                        case "onTeacher":
                            ViewModel.SelectedViewModel = new TeacherWeeklyModel();
                            break;

                    }

                }, x => true));
            } 
        }

        private ICommand _buttonRefreshDataCommand;
        public ICommand ButtonRefreshDataCommand
        {
            get
            {
                return (_buttonRefreshDataCommand ?? new RelayCommand(param =>
                {

                    ViewModel.SelectedViewModel = null;

                }, x => true));
            }
        }

    }
}
