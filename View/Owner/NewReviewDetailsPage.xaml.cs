using BookingApp.DTO;
using BookingApp.Repository;
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

namespace BookingApp.View.Owner
{
    /// <summary>
    /// Interaction logic for NewReviewDetailsPage.xaml
    /// </summary>
    public partial class NewReviewDetailsPage : Page
    {
        private OwnerMainWindow _ownerMainWindow;
        private MessageDTO _messageDTO;

        private MessageRepository _messageRepository;

        public NewReviewDetailsPage(OwnerMainWindow ownerMainWindow, MessageDTO messageDTO)
        {
            InitializeComponent();

            _ownerMainWindow = ownerMainWindow;
            _messageDTO = messageDTO;
            _messageDTO.IsRead = true;

            _messageRepository = new MessageRepository();

            _messageRepository.Update(_messageDTO.ToMessage());

            DataContext = _messageDTO;
        }

        private void ShowSideMenu(object sender, RoutedEventArgs e)
        {
            _ownerMainWindow.ShowSideMenu(sender, e);
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            _ownerMainWindow.frameMain.Content = new InboxPage(_ownerMainWindow);
        }
    }
}
