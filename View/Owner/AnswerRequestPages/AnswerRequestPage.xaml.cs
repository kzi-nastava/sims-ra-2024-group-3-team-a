using BookingApp.DTO;
using BookingApp.Model.Enums;
using BookingApp.Repository;
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

namespace BookingApp.View.Owner.AnswerRequestPages
{
    /// <summary>
    /// Interaction logic for AnswerRequestPage.xaml
    /// </summary>
    public partial class AnswerRequestPage : Page
    {
        private OwnerMainWindow _ownerMainWindow;
        public MessageDTO _messageDTO { get; set; }

        public static ObservableCollection<AccommodationReservationDTO> AccommodationReservationsDTO { get; set; }

        private AccommodationReservationChangeRequestDTO _accommodationReservationChangeRequestDTO;
        private AccommodationReservationDTO _accommodationReservationDTO;

        private AccommodationReservationChangeRequestRepository _accommodationReservationChangeRequestRepository;
        private AccommodationReservationRepository _accommodationReservationRepository;
        private MessageRepository _messageRepository;

        public AnswerRequestPage(OwnerMainWindow ownerMainWindow, MessageDTO messageDTO)
        {
            InitializeComponent();
            DataContext = this;

            AccommodationReservationsDTO = new ObservableCollection<AccommodationReservationDTO>();

            _accommodationReservationChangeRequestRepository = new AccommodationReservationChangeRequestRepository();
            _accommodationReservationRepository = new AccommodationReservationRepository();
            _messageRepository = new MessageRepository();

            _ownerMainWindow = ownerMainWindow;
            _messageDTO = messageDTO;
            _accommodationReservationChangeRequestDTO = new AccommodationReservationChangeRequestDTO(_accommodationReservationChangeRequestRepository.GetById(_messageDTO.RequestId));
            _accommodationReservationDTO = new AccommodationReservationDTO(_accommodationReservationRepository.GetById(_accommodationReservationChangeRequestDTO.AccommodationReservationId));

            SetAllReservations();
            calendarOccupiedDays.DisplayDateStart = new DateTime(_accommodationReservationChangeRequestDTO.BeginDateNew.Year, _accommodationReservationChangeRequestDTO.BeginDateNew.Month, _accommodationReservationChangeRequestDTO.BeginDateNew.Day);
            calendarOccupiedDays.DisplayDateEnd = new DateTime(_accommodationReservationChangeRequestDTO.EndDateNew.Year, _accommodationReservationChangeRequestDTO.EndDateNew.Month, _accommodationReservationChangeRequestDTO.EndDateNew.Day);
            BlackoutOccupiedDays();
        }

        private void SetAllReservations()
        {
            foreach(var reservation in _accommodationReservationRepository.GetAll())
            {
                AccommodationReservationsDTO.Add(new AccommodationReservationDTO(reservation));
            }
        }

        private void BlackoutOccupiedDays()
        {
            foreach(var reservation in AccommodationReservationsDTO)
            {
                if (reservation.AccommodationId == _accommodationReservationDTO.AccommodationId && reservation.GuestId != _accommodationReservationDTO.GuestId)
                {
                    DateTime beginDate = new DateTime(reservation.BeginDate.Year, reservation.BeginDate.Month, reservation.BeginDate.Day);
                    DateTime endDate = new DateTime(reservation.EndDate.Year, reservation.EndDate.Month, reservation.EndDate.Day);
                    calendarOccupiedDays.BlackoutDates.Add(new CalendarDateRange(beginDate, endDate));   
                }
            }
        }

        private void ShowSideMenu(object sender, RoutedEventArgs e)
        {
            _ownerMainWindow.ShowSideMenu(sender, e);
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            _ownerMainWindow.frameMain.GoBack();
        }

        private void AcceptRequest(object sender, RoutedEventArgs e)
        {
            _accommodationReservationChangeRequestDTO.Status = AccommodationChangeRequestStatus.Accepted;
            _accommodationReservationChangeRequestRepository.Update(_accommodationReservationChangeRequestDTO.ToAccommodationReservationChangeRequest());
            _messageRepository.Delete(_messageDTO.ToMessage());
            _accommodationReservationDTO.BeginDate = _accommodationReservationChangeRequestDTO.BeginDateNew;
            _accommodationReservationDTO.EndDate = _accommodationReservationChangeRequestDTO.EndDateNew;
            _accommodationReservationRepository.Update(_accommodationReservationDTO.ToAccommodationReservation());
            _ownerMainWindow.frameMain.Content = new InboxPage(_ownerMainWindow);
        }

        private void RejectRequest(object sender, RoutedEventArgs e)
        {
            _ownerMainWindow.frameMain.Content = new RejectedMessagePage(_ownerMainWindow, _accommodationReservationChangeRequestDTO, _messageDTO);
        }
    }
}
