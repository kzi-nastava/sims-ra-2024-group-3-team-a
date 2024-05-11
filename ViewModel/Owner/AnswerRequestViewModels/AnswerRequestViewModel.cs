using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.InjectorNameSpace;
using BookingApp.Model;
using BookingApp.Model.Enums;
using BookingApp.Repository;
using BookingApp.Repository.Interfaces;
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
        private UserService _userService;

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
            IMessageRepository messageRepository = Injector.CreateInstance<IMessageRepository>();
            IAccommodationReservationChangeRequestRepository accommodationReservationChangeRequestRepository = Injector.CreateInstance<IAccommodationReservationChangeRequestRepository>();
            IAccommodationReservationRepository accommodationReservationRepository = Injector.CreateInstance<IAccommodationReservationRepository>();
            IAccommodationRepository accommodationRepository = Injector.CreateInstance<IAccommodationRepository>();
            IUserRepository userRepository = Injector.CreateInstance<IUserRepository>();
            ITourRepository tourRepository = Injector.CreateInstance<ITourRepository>();
            ITouristRepository touristRepository = Injector.CreateInstance<ITouristRepository>();
            ITourReservationRepository tourReservationRepository = Injector.CreateInstance<ITourReservationRepository>();
            ITourReviewRepository tourReviewRepository = Injector.CreateInstance<ITourReviewRepository>();
            IVoucherRepository voucherRepository = Injector.CreateInstance<IVoucherRepository>();
         //   _messageService = new MessageService(messageRepository, accommodationReservationChangeRequestRepository, accommodationReservationRepository, accommodationRepository, userRepository, tourRepository, tourReservationRepository, touristRepository, tourReviewRepository, voucherRepository );
            _accommodationReservationService = new AccommodationReservationService(accommodationReservationRepository, accommodationRepository, userRepository);
            _accommodationReservationChangeRequestService = new AccommodationReservationChangeRequestService(accommodationReservationChangeRequestRepository, accommodationReservationRepository, accommodationRepository, userRepository);
            _userService = new UserService(userRepository);

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

        public MessageDTO MessageDTO
        {
            get
            {
                return _messageDTO;
            }
            set
            {
                _messageDTO = value;
                OnPropertyChanged();
            }
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

            string sender = _userService.GetById(_messageDTO.RecieverId).Username;
            int receiverId = _userService.GetByUsername(_messageDTO.Sender).Id;
            Message _newRejectedMessage = new Message(0, _accommodationReservationChangeRequestDTO.Id, sender, receiverId, "Accepted Date Change Request", "accepted", MessageType.RejectedChangeRequest, false);
            _messageService.Save(_newRejectedMessage);
            OwnerMainWindow.MainFrame.Content = new InboxPage(OwnerMainWindow.LoggedInOwner);
        }

        private void RejectRequest()
        {
            OwnerMainWindow.MainFrame.Content = new RejectedMessagePage(_accommodationReservationChangeRequestDTO, _messageDTO);
        }
    }
}
