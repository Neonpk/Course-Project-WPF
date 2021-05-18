using CourseProjectWPF.Model;
using CourseProjectWPF.Windows;
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

namespace CourseProjectWPF.Pages
{
    /// <summary>
    /// Interaction logic for EditChangeNotesDataPage.xaml
    /// </summary>
    public partial class EditChangeNotesDataPage : Page
    {

        private AdminWindow adminWindow = null;

        public changedescription SelectedChangeNote { get; set; }
        private changedescription EditChangeNoteReference { get; set; }

        private bool IsErrorField = false;

        public EditChangeNotesDataPage(AdminWindow adminWindow)
        {
            InitializeComponent();

            SelectedChangeNote = new changedescription
            {
                description = "Какое-то примечание..."
            };

            this.adminWindow = adminWindow;

            DataContext = this;
        }

        public EditChangeNotesDataPage(AdminWindow adminWindow, changedescription SelectedChangeNote)
        {
            InitializeComponent();

            this.SelectedChangeNote = (changedescription)SelectedChangeNote.Clone();
            this.EditChangeNoteReference = SelectedChangeNote;

            this.adminWindow = adminWindow;

            DataContext = this;
        }

        private void buttonSaveIntoDB_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (EditChangeNoteReference != null)
                {
                    EditChangeNoteReference.description = SelectedChangeNote.description;
                }
                else
                {
                    Connection.Database.changedescription.Add(SelectedChangeNote);
                }

                Connection.Database.SaveChanges();

                adminWindow.mainFrame.Navigate(new ChangeNotesDataPage(adminWindow));


            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Ошибка при сохранении", MessageBoxButton.OK, MessageBoxImage.Error);
                Connection.Database.changedescription.Remove(SelectedChangeNote);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.adminWindow.mainFrame.Navigate(new ChangeNotesDataPage(this.adminWindow));
        }

        private void TextBoxFields_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
            {
                var errorToolTip = new ToolTip();
                errorToolTip.Content = e.Error.ErrorContent;

                (sender as TextBox).ToolTip = errorToolTip;

                IsErrorField = true;

            }

            if (e.Action == ValidationErrorEventAction.Removed)
            {
                IsErrorField = false;
            }

            buttonSaveIntoDB.IsEnabled = !IsErrorField;
        }

    }
}
