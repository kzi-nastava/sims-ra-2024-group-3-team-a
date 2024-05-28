using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.InjectorNameSpace;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Repository.Interfaces;
using BookingApp.Service;
using BookingApp.View.Guide;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace BookingApp.ViewModel.Guide
{
    public class AcceptTourViewModel : ViewModel
    {
        private OrdinaryTourRequestDTO _requestDTO;
        private UserDTO _userDTO;
        private RelayCommand _acceptTourRequestCommand;
        private OrdinaryTourRequestService _tourRequestService;
        private TourService _tourService;
        private MessageService _messageService;
        private DateTime _begin =default;
        private List<DateTime> _beginDates;
        public AcceptTourViewModel(OrdinaryTourRequestDTO  requestDTO, UserDTO user)
        {
            IMessageRepository messageRepository = Injector.CreateInstance<IMessageRepository>();
            IAccommodationReservationChangeRequestRepository accommodationReservationChangeRequestRepository = Injector.CreateInstance<IAccommodationReservationChangeRequestRepository>();
            IAccommodationReservationRepository accommodationReservationRepository = Injector.CreateInstance<IAccommodationReservationRepository>();
            IAccommodationRepository accommodationRepository = Injector.CreateInstance<IAccommodationRepository>();
            IUserRepository userRepository = Injector.CreateInstance<IUserRepository>();
            ITourRepository tourRepository = Injector.CreateInstance<ITourRepository>();
            ITourReservationRepository tourReservationRepository = Injector.CreateInstance<ITourReservationRepository>();
            ITouristRepository touristRepository = Injector.CreateInstance<ITouristRepository>();
            ITourReviewRepository tourReviewRepository = Injector.CreateInstance<ITourReviewRepository>();
            IVoucherRepository voucherRepository = Injector.CreateInstance<IVoucherRepository>();
            _messageService = new MessageService(messageRepository, accommodationReservationChangeRequestRepository, accommodationReservationRepository, accommodationRepository, userRepository, tourRepository, tourReservationRepository, touristRepository, tourReviewRepository, voucherRepository);
            _tourService = new TourService(tourRepository, userRepository, touristRepository, tourReservationRepository, tourReviewRepository, voucherRepository);
            _requestDTO = requestDTO;
            _userDTO = user;
            _acceptTourRequestCommand = new RelayCommand(AcceptTourRequest);
            IOrdinaryTourRequestRepository requestRepository = Injector.CreateInstance<IOrdinaryTourRequestRepository>();
            _tourRequestService = new OrdinaryTourRequestService(accommodationReservationChangeRequestRepository, accommodationReservationRepository, accommodationRepository, requestRepository, tourRepository, messageRepository, touristRepository, userRepository, tourReservationRepository, tourReviewRepository, voucherRepository);
            AddDates();
        }
        private void AddDates()
        {
            _beginDates = new List<DateTime>();

            for (DateTime date = _requestDTO.BeginDate; date <= _requestDTO.EndDate; date = date.AddDays(1))
            {
                if (!NotAvailableDates().Any(d => d.Date == date.Date))
                {
                    _beginDates.Add(date);
                }
                

            }
        }
        private List<DateTime> NotAvailableDates()
        {
            List<DateTime>  dates = new List<DateTime>();
            foreach (Tour tour in _tourService.GetUpcoming())
            {
                dates.Add(tour.BeginingTime);
            }
                return dates;
        }
        public List<DateTime> BeginDates
        {
            get
            {
                return _beginDates;
            }
        }
        public DateTime Begin
        {
            get
            {
                return _begin;
            }
            set
            {
                _begin = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand AcceptTourRequestCommand
        {
            get
            {
                return _acceptTourRequestCommand;
            }
            set
            {
                _acceptTourRequestCommand = value;
                OnPropertyChanged();
            }
        }
        private void AcceptTourRequest()
        {
            if(Begin!=default)
            {
            _requestDTO.BeginDate = _begin;
            _requestDTO.EndDate = _begin;
            _requestDTO.Status=Model.Enums.TourRequestStatus.Accepted;
             _requestDTO.RequestAcceptedDate = DateTime.Now;
            _tourRequestService.Update(_requestDTO.ToOrdinaryTourRequest());
                MessageDTO message = new MessageDTO();
                message.RequestId = _requestDTO.Id;
                message.Sender = _userDTO.Username;
                message.RecieverId = _requestDTO.UserId;
                message.Header = "Tour request accepted";
                message.Content = "Tour request accepted" ;
                message.Type = Model.Enums.MessageType.AcceptedTourRequest;
                message.IsRead = false;
                _messageService.Save(message.ToMessage());
             TourRequestWindow.GetInstance().Close();
             TourRequestWindow window = new TourRequestWindow(_userDTO);
             window.Show();
            AcceptTourWindow.GetInstance().Close();
                
            }
        }
    }
}
