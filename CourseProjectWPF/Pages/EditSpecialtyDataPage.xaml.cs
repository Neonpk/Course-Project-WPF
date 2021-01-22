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
    /// Логика взаимодействия для EditSpecialtyDataPage.xaml
    /// </summary>
    public partial class EditSpecialtyDataPage : Page
    {
        private AdminWindow adminWindow = null;
        public specialties CurrentSpecialty { get; set; }
        public specialties EditSpecialtyReference { get; set; }

        public List<faculties> Faculties { get; set; }

        private int errorFieldCounter = 0;

        public EditSpecialtyDataPage(AdminWindow adminWindow)
        {
            InitializeComponent();

            CurrentSpecialty = new specialties
            {
                specialty_name = "Какая-то специальность..."
            };

            Faculties = Connection.Database.faculties.ToList();

            this.adminWindow = adminWindow;

            DataContext = this;
        }

        public EditSpecialtyDataPage(AdminWindow adminWindow, specialties CurrentSpecialty)
        {
            InitializeComponent();

            this.CurrentSpecialty = (specialties)CurrentSpecialty.Clone();
            EditSpecialtyReference = CurrentSpecialty;

            Faculties = Connection.Database.faculties.ToList();

            this.adminWindow = adminWindow;

            DataContext = this;
        }


        private void Cancel_ButtonClick(object sender, RoutedEventArgs e)
        {
            adminWindow.mainFrame.Navigate(new SpecialtiesDataPage(adminWindow));
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

                errorFieldCounter++;

            }

            if (e.Action == ValidationErrorEventAction.Removed)
            {
                errorFieldCounter--; 
            }

            buttonSaveIntoDB.IsEnabled = errorFieldCounter == 0;
        }

        private void buttonSaveIntoDB_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (EditSpecialtyReference != null)
                {
                    EditSpecialtyReference.specialty_name = CurrentSpecialty.specialty_name;
                    EditSpecialtyReference.faculties = CurrentSpecialty.faculties;
                }
                else
                {
                    Connection.Database.specialties.Add(CurrentSpecialty);
                }

                Connection.Database.SaveChanges();

                adminWindow.mainFrame.Navigate(new SpecialtiesDataPage(adminWindow));


            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Ошибка при сохранении", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
