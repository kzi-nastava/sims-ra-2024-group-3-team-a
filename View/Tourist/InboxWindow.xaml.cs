using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Model.Enums;
using BookingApp.Repository;
using BookingApp.ViewModel.Tourist;
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
using System.Windows.Shapes;

namespace BookingApp.View.Tourist
{
    /// <summary>
    /// Interaction logic for InboxWindow.xaml
    /// </summary>
    public partial class InboxWindow : Window
    {
        private MessageDTO _messageDTO;

        private ObservableCollection<MessageDTO> Messages { get; set; }

        private MessageRepository _messageRepository;

        private InboxViewModel _inboxViewModel;
        public InboxWindow(UserDTO loggedInUser)
        {
            InitializeComponent();

            _inboxViewModel = new InboxViewModel(loggedInUser);

            DataContext = _inboxViewModel;
            
        }

        /* private void Update()
         {
             Messages.Clear();
             foreach (Message message in _messageRepository.GetAll())
             {
                 if (message.Type.Equals(MessageType.TourAttendance))
                     Messages.Add(new MessageDTO(message));
             }

         }public partial class InboxPage : Page
     {
         private InboxViewModel _inboxViewModel;

         public InboxPage(UserDTO loggedInUser)
         {
             InitializeComponent();

             _inboxViewModel = new InboxViewModel(loggedInUser);
             DataContext = _inboxViewModel;
         }
     }


         private void ShowTrackTourWindow(object sender, KeyEventArgs e)
         {
             if (e.Key == Key.Enter)
             {

                 var selectedItem = (sender as ListView).SelectedItem;


                 if (selectedItem != null)
                 {

                     MessageBox.Show("in progress");


                 }
             }
         }*/
    }
}
