using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseProjectWPF.ViewModel
{

    public class WeeklyModel : BaseViewModel
    {
        private string _monday;
        public string Monday { get => _monday; set { _monday = value; OnPropertyChanged(nameof(Monday)); } }

        private string _tuesday;
        public string Tuesday { get => _tuesday; set { _tuesday = value; OnPropertyChanged(nameof(Tuesday)); } }

        private string _wednesday;
        public string Wednesday { get => _wednesday; set { _wednesday = value; OnPropertyChanged(nameof(Wednesday)); } }

        private string _thursday;
        public string Thursday { get => _thursday; set { _thursday = value; OnPropertyChanged(nameof(Thursday)); } }

        private string _friday;
        public string Friday { get => _friday; set { _friday = value; OnPropertyChanged(nameof(Friday)); } }

        private string _saturday;
        public string Saturday { get => _saturday; set { _saturday = value; OnPropertyChanged(nameof(Saturday)); } }

        private string _sunday;
        public string Sunday { get => _sunday; set { _sunday = value; OnPropertyChanged(nameof(Sunday)); } }

        public void ClearFields()
        {
            this.Monday = "";
            this.Tuesday = "";
            this.Wednesday = "";
            this.Thursday = "";
            this.Friday = "";
            this.Saturday = "";
            this.Sunday = "";
        }

    }
}
