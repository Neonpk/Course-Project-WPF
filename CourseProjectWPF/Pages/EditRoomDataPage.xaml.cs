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
    /// Логика взаимодействия для EditRoomDataPage.xaml
    /// </summary>
    public partial class EditRoomDataPage : Page
    {
        private AdminWindow adminWindow = null;
        public bool IsNotChangeMode { get; set; } = true;

        private int errorFieldCounter = 0;

        public audiences SelectedRoom { get; set; }
        private audiences EditRoomReference { get; set; }

        public EditRoomDataPage(AdminWindow adminWindow)
        {
            InitializeComponent();

            this.adminWindow = adminWindow;

            SelectedRoom = new audiences
            {
                room = 0,
                capacity = 0,
                type = "Какой-то тип"
            };

            DataContext = this;

        }

        public EditRoomDataPage(AdminWindow adminWindow, audiences SelectedRoom)
        {
            InitializeComponent();

            this.adminWindow = adminWindow;

            this.SelectedRoom = (audiences)SelectedRoom.Clone();
            this.EditRoomReference = SelectedRoom;

            IsNotChangeMode = false;

            DataContext = this;

        }


        private void TextBoxFields_Error(object sender, ValidationErrorEventArgs e)
        {

            if (e.Action == ValidationErrorEventAction.Added)
            {
                var errorToolTip = new ToolTip();
                errorToolTip.Content = e.Error.ErrorContent;

                (sender as TextBox).ToolTip = errorToolTip;

                errorFieldCounter++;

            }

            if (e.Action == ValidationErrorEventAction.Removed)
            {
                errorFieldCounter--;
            }

            buttonSaveIntoDB.IsEnabled = errorFieldCounter == 0;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.adminWindow.mainFrame.Navigate(new RoomsDataPage(this.adminWindow));
        }

        private void SaveIntoDataBase(object sender, RoutedEventArgs e)
        {
            try
            {

                if (EditRoomReference != null)
                {
                    EditRoomReference.room = SelectedRoom.room;
                    EditRoomReference.capacity = SelectedRoom.capacity;
                    EditRoomReference.type = SelectedRoom.type;
                }
                else
                {
                    Connection.Database.audiences.Add(SelectedRoom);
                }

                Connection.Database.SaveChanges();

                adminWindow.mainFrame.Navigate(new RoomsDataPage(adminWindow));


            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Ошибка при сохранении", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
