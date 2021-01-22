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
using CourseProjectWPF.Model;
using CourseProjectWPF.Windows;

namespace CourseProjectWPF.Pages
{
    /// <summary>
    /// Логика взаимодействия для EditDisciplineDataPage.xaml
    /// </summary>
    public partial class EditDisciplineDataPage : Page
    {
        private AdminWindow adminWindow = null;

        public disciplines CurrentDiscipline { get; set; }
        private disciplines EditDisciplineReference { get; set; }

        public List<departments> Departments { get; set; }

        private int errorCounter = 0;


        public EditDisciplineDataPage(AdminWindow adminWindow)
        {
            InitializeComponent();

            Departments = Connection.Database.departments.ToList();

            CurrentDiscipline = new disciplines
            {
                discipline_name = "Какая-то дисциплина..."
            };

            this.adminWindow = adminWindow;

            DataContext = this;
        }

        public EditDisciplineDataPage(AdminWindow adminWindow, disciplines CurrentDiscipline)
        {
            InitializeComponent();


            this.CurrentDiscipline = (disciplines)CurrentDiscipline.Clone();
            EditDisciplineReference = CurrentDiscipline;

            Departments = Connection.Database.departments.ToList();

            this.adminWindow = adminWindow;

            DataContext = this;
        }


        private void TextBoxFields_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
            {
                var errorToolTip = new ToolTip();
                errorToolTip.Content = e.Error.ErrorContent;

                if (sender is TextBox)
                    (sender as TextBox).ToolTip = errorToolTip;
                else
                    (sender as ComboBox).ToolTip = errorToolTip;

                errorCounter++;

            }

            if (e.Action == ValidationErrorEventAction.Removed)
            {
                errorCounter--;
            }

            buttonSaveIntoDB.IsEnabled = errorCounter == 0;
        }

        private void Cancel_ButtonClick(object sender, RoutedEventArgs e)
        {
            adminWindow.mainFrame.Navigate(new DisciplinesDataPage(adminWindow));
        }

        private void buttonSaveIntoDB_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (EditDisciplineReference != null)
                {
                    EditDisciplineReference.discipline_name = CurrentDiscipline.discipline_name;
                    EditDisciplineReference.departments = CurrentDiscipline.departments;
                }
                else
                {
                    Connection.Database.disciplines.Add(CurrentDiscipline);
                }

                Connection.Database.SaveChanges();

                adminWindow.mainFrame.Navigate(new DisciplinesDataPage(adminWindow));


            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Ошибка при сохранении", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
