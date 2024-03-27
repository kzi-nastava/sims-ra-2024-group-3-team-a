using BookingApp.DTO;
using BookingApp.Model.Enums;
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

namespace BookingApp.View.Owner.AnswerRequestPages
{
    /// <summary>
    /// Interaction logic for RejectedMessagePage.xaml
    /// </summary>
    public partial class RejectedMessagePage : Page
    {
        private AccommodationReservationChangeRequestDTO _accommodationReservationChangeRequestDTO;
        private MessageDTO _messageDTO;

        private AccommodationReservationChangeRequestRepository _accommodationReservationChangeRequestRepository;
        private MessageRepository _messageRepository;

        private OwnerMainWindow _ownerMainWindow;

        public string RejectedMessage { get; set; }

        public RejectedMessagePage(OwnerMainWindow ownerMainWindow, AccommodationReservationChangeRequestDTO accommodationReservationChangeRequestDTO, MessageDTO messageDTO)
        {
            InitializeComponent();
            DataContext = this;

            _ownerMainWindow = ownerMainWindow;
            _accommodationReservationChangeRequestDTO = accommodationReservationChangeRequestDTO;

            _accommodationReservationChangeRequestRepository = new AccommodationReservationChangeRequestRepository();
            _messageRepository = new MessageRepository();
            _messageDTO = messageDTO;
        }

        private void ShowSideMenu(object sender, RoutedEventArgs e)
        {
            _ownerMainWindow.ShowSideMenu(sender, e);
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            _ownerMainWindow.frameMain.GoBack();
        }

        private void ConfirmRejectRequest(object sender, RoutedEventArgs e)
        {
            _accommodationReservationChangeRequestDTO.RejectedMessage = RejectedMessage;
            _accommodationReservationChangeRequestDTO.Status = AccommodationChangeRequestStatus.Rejected;
            _accommodationReservationChangeRequestRepository.Update(_accommodationReservationChangeRequestDTO.ToAccommodationReservationChangeRequest());
            _messageRepository.Delete(_messageDTO.ToMessage());
            _ownerMainWindow.frameMain.Content = new InboxPage(_ownerMainWindow);
        }
    }
}
