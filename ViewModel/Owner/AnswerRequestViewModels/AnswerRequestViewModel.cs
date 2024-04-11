using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.Model.Enums;
using BookingApp.Repository;
using BookingApp.Service;
using BookingApp.View.Owner;
using BookingApp.View.Owner.AnswerRequestPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.ViewModel.Owner.AnswerRequestViewModels
{
    public class AnswerRequestViewModel : ViewModel
    {
        private AccommodationReservationService _accommodationReservationService;
        private AccommodationReservationChangeRequestService _accommodationReservationChangeRequestService;
        private MessageService _messageService;

        private AccommodationReservationDTO _accommodationReservationDTO;
        private AccommodationReservationChangeRequestDTO _accommodationReservationChangeRequestDTO;
        private MessageDTO _messageDTO;

        private DateTime _calendarBeginDate;
        private DateTime _calendarEndDate;
        private List<DateTime> _occupiedDates;

        private RelayCommand _goBackCommand;
        private RelayCommand _showSideMenuCommand;
        private RelayCommand _acceptRequest;
        private RelayCommand _rejectRequest;

        public AnswerRequestViewModel(MessageDTO messageDTO)
        {
            _messageDTO = messageDTO;
            _accommodationReservationService = new AccommodationReservationService();
            _accommodationReservationChangeRequestService = new AccommodationReservationChangeRequestService();
            _messageService = new MessageService();

            _accommodationReservationChangeRequestDTO = new AccommodationReservationChangeRequestDTO(_accommodationReservationChangeRequestService.GetById(messageDTO.RequestId));
            _accommodationReservationDTO = new AccommodationReservationDTO(_accommodationReservationService.GetById(_accommodationReservationChangeRequestDTO.AccommodationReservationId));

            _calendarBeginDate = new DateTime(_accommodationReservationChangeRequestDTO.BeginDateNew.Year, _accommodationReservationChangeRequestDTO.BeginDateNew.Month, _accommodationReservationChangeRequestDTO.BeginDateNew.Day);
            _calendarEndDate = new DateTime(_accommodationReservationChangeRequestDTO.EndDateNew.Year, _accommodationReservationChangeRequestDTO.EndDateNew.Month, _accommodationReservationChangeRequestDTO.EndDateNew.Day);

            _occupiedDates = _accommodationReservationService.GetOccupiedDates(_accommodationReservationDTO.ToAccommodationReservation());

            _goBackCommand = new RelayCommand(GoBack);
            _showSideMenuCommand = new RelayCommand(ShowSideMenu);
            _acceptRequest = new RelayCommand(AcceptRequest);
            _rejectRequest = new RelayCommand(RejectRequest);
        }

        public DateTime CalendarBeginDate
        {
            get
            {
                return _calendarBeginDate;
            }
            set
            {
                _calendarBeginDate = value;
                OnPropertyChanged();
            }
        }

        public DateTime CalendarEndDate
        {
            get
            {
                return _calendarEndDate;
            }
            set
            {
                _calendarEndDate = value;
                OnPropertyChanged();
            }
        }

        public List<DateTime> OccupiedDates
        {
            get
            {
                return _occupiedDates;
            }
            set
            {
                _occupiedDates = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand GoBackCommand
        {
            get
            {
                return _goBackCommand;
            }
            set
            {
                _goBackCommand = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand ShowSideMenuCommand
        {
            get
            {
                return _showSideMenuCommand;
            }
            set
            {
                _showSideMenuCommand = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand AcceptRequestCommand
        {
            get
            {
                return _acceptRequest;
            }
            set
            {
                _acceptRequest = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand RejectRequestCommand
        {
            get
            {
                return _rejectRequest;
            }
            set
            {
                _rejectRequest = value;
                OnPropertyChanged();
            }
        }

        public void ShowSideMenu()
        {
            OwnerMainWindow.SideMenuFrame.Content = new SideMenuPage();
        }

        private void GoBack()
        {
            OwnerMainWindow.MainFrame.GoBack();
        }

        private void AcceptRequest()
        {
            _accommodationReservationChangeRequestDTO.Status = AccommodationChangeRequestStatus.Accepted;
            _accommodationReservationChangeRequestService.Update(_accommodationReservationChangeRequestDTO.ToAccommodationReservationChangeRequest());
            _messageService.Delete(_messageDTO.ToMessage());
            _accommodationReservationDTO.BeginDate = _accommodationReservationChangeRequestDTO.BeginDateNew;
            _accommodationReservationDTO.EndDate = _accommodationReservationChangeRequestDTO.EndDateNew;
            _accommodationReservationService.Update(_accommodationReservationDTO.ToAccommodationReservation());
            OwnerMainWindow.MainFrame.Content = new InboxPage(OwnerMainWindow.LoggedInOwner);
        }

        private void RejectRequest()
        {
            OwnerMainWindow.MainFrame.Content = new RejectedMessagePage(_accommodationReservationChangeRequestDTO, _messageDTO);
        }
    }
}
