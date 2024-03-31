using BookingApp.DTO;
using BookingApp.Model.Enums;
using BookingApp.Repository;
using BookingApp.View.Owner.AnswerRequestPages;
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

namespace BookingApp.View.Owner
{
    /// <summary>
    /// Interaction logic for InboxPage.xaml
    /// </summary>
    public partial class InboxPage : Page
    {
        public static ObservableCollection<MessageDTO> MessagesDTO { get; set; }

        private readonly MessageRepository _messageRepository;

        private OwnerMainWindow _ownerMainWindow;

        public InboxPage(OwnerMainWindow ownerMainWindow)
        {
            InitializeComponent();
            DataContext = this;
            _ownerMainWindow = ownerMainWindow;

            _messageRepository = new MessageRepository();

            _ownerMainWindow.Update();
            MessagesDTO = OwnerMainWindow.MessagesDTO;
        }

        private void ShowMessageDetailsPage(object sender, SelectionChangedEventArgs e)
        {
            if (listViewInbox.SelectedItem == null)
            {
                return;
            }

            var selectedItem = listViewInbox.SelectedItem as MessageDTO;

            if(selectedItem.Type == MessageType.AccommodationChangeRequest)
            {
                _ownerMainWindow.frameMain.Content = new RequestDetailsPage(_ownerMainWindow, selectedItem);
                listViewInbox.SelectedItem = null;
            }
            else if(selectedItem.Type == MessageType.NewReviewNotification)
            {
                _ownerMainWindow.frameMain.Content = new NewReviewDetailsPage(_ownerMainWindow, selectedItem);
                listViewInbox.SelectedItem = null;
            }
        }

        private void ShowSideMenu(object sender, RoutedEventArgs e)
        {
            _ownerMainWindow.ShowSideMenu(sender, e);
        }
    }
}
