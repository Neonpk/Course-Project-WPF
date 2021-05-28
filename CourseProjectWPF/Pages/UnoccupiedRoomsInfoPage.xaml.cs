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
using CourseProjectWPF.ViewModel;

namespace CourseProjectWPF.Pages
{
    /// <summary>
    /// Interaction logic for UnoccupiedRoomsInfoPage.xaml
    /// </summary>
    public partial class UnoccupiedRoomsInfoPage : Page
    {
        public UnoccupiedRoomsInfoPage()
        {
            InitializeComponent();
            DataContext = new UnoccupiedRoomsViewModel();
        }
    }
}
