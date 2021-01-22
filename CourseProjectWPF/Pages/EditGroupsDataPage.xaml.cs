using CourseProjectWPF.Model;
using CourseProjectWPF.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Threading;

namespace CourseProjectWPF.Pages
{
    /// <summary>
    /// Логика взаимодействия для EditGroupsDataPage.xaml
    /// </summary>
    public partial class EditGroupsDataPage : Page
    {

        private DispatcherWindow dispatcherWindow = null;

        public groups CurrentGroup { get; set; }

        private groups EditGroupReference { get; set; }

        public List<faculties> Faculties { get; set; }
        public List<specialties> Specialties { get; set; }
        public List<train_types> Train_types { get; set; }

        public bool IsNotChangeMode { get; set; } = true;


        private int errorFieldCounter = 0;

        public EditGroupsDataPage(DispatcherWindow window, groups CurrentGroup)
        {

            InitializeComponent();

            this.dispatcherWindow = window;

            this.CurrentGroup = (groups)CurrentGroup.Clone();
            this.EditGroupReference = CurrentGroup;

            Faculties = Connection.Database.faculties.ToList();
            Specialties = Connection.Database.specialties.ToList();
            Train_types = Connection.Database.train_types.ToList();

            IsNotChangeMode = false;

            DataContext = this;


        }

        public EditGroupsDataPage(DispatcherWindow window)
        {

            InitializeComponent();

            this.dispatcherWindow = window;


            this.CurrentGroup = new groups
            {
                group_id = 0,
                count_people = 0,
            };

            Faculties   = Connection.Database.faculties.ToList();
            Specialties = Connection.Database.specialties.ToList();
            Train_types = Connection.Database.train_types.ToList();

            DataContext = this;

        }

        private void SaveIntoDataBase_ButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {

                if (EditGroupReference != null)
                {
                    EditGroupReference.count_people = CurrentGroup.count_people;
                    EditGroupReference.group_id = CurrentGroup.group_id;
                    EditGroupReference.id_faculty = CurrentGroup.id_faculty;
                    EditGroupReference.id_specialty = CurrentGroup.id_specialty;
                    EditGroupReference.id_trainform = CurrentGroup.id_trainform;
                    EditGroupReference.faculties = CurrentGroup.faculties;
                    EditGroupReference.specialties = CurrentGroup.specialties;
                    EditGroupReference.train_types = CurrentGroup.train_types;

                }
                else
                {
                    Connection.Database.groups.Add(CurrentGroup);
                }

                Connection.Database.SaveChanges();

                dispatcherWindow.mainFrame.Navigate(new GroupsDataPage(dispatcherWindow));


            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Ошибка при сохранении", MessageBoxButton.OK, MessageBoxImage.Error);
                Connection.Database.groups.Remove(CurrentGroup);

            }
        }

        private void Cancel_ButtonClick(object sender, RoutedEventArgs e)
        {
            this.dispatcherWindow.mainFrame.Navigate(new GroupsDataPage(this.dispatcherWindow));
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
    }
}
