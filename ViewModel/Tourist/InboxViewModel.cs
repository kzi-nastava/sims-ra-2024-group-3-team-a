using BookingApp.DTO;
using BookingApp.InjectorNameSpace;
using BookingApp.Model;
using BookingApp.Repository.Interfaces;
using BookingApp.Service;
using BookingApp.View.Tourist;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.ViewModel.Tourist
{
    public class InboxViewModel: ViewModel
    {
        private MessageService _messageService;
        private TourService _tourService;
        private OrdinaryTourRequestService _ordinaryTourRequestService;
        private ObservableCollection<MessageDTO> _messageDTO;
        private MessageDTO _selectedMessageDTO = null;
        private UserDTO _userDTO;
        public InboxViewModel(UserDTO loggedInUser)
        {

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
            IOrdinaryTourRequestRepository ordinaryTourRequestRepository = Injector.CreateInstance<IOrdinaryTourRequestRepository>();
            _messageService = new MessageService(messageRepository, accommodationReservationChangeRequestRepository, accommodationReservationRepository, accommodationRepository, userRepository, tourRepository, tourReservationRepository, touristRepository, tourReviewRepository, voucherRepository);
            _ordinaryTourRequestService = new OrdinaryTourRequestService(accommodationReservationChangeRequestRepository, accommodationReservationRepository, accommodationRepository, ordinaryTourRequestRepository, tourRepository, messageRepository, touristRepository, userRepository, tourReservationRepository, tourReviewRepository, voucherRepository);
            _tourService = new TourService(tourRepository, userRepository, touristRepository, tourReservationRepository, tourReviewRepository, voucherRepository);
            _userDTO = loggedInUser;
            List<MessageDTO> messages = _messageService.GetByOwner(loggedInUser.Id).Select(message => new MessageDTO(message)).ToList();
            _messageDTO = new ObservableCollection<MessageDTO>(messages);

        }

        public ObservableCollection<MessageDTO> MessagesDTO 
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

        public MessageDTO SelectedMessageDTO
        {
            get
            {
                return _selectedMessageDTO;
            }
            set
            {
                _selectedMessageDTO= value; 
                OnPropertyChanged();
                ShowMessageDetailsWindow();
            }
        }
        private void ShowMessageDetailsWindow()
        {
            if (_selectedMessageDTO == null)
            {
                return;
            }

            var selectedItem = _selectedMessageDTO as MessageDTO;
            selectedItem.IsRead = true;
            _messageService.Update(selectedItem.ToMessage());

           
          
           
            
          

            if (selectedItem.Type.Equals(Model.Enums.MessageType.TourAttendance)) 
            {
                Tour tour = _tourService.GetById(selectedItem.RequestId);
                TrackTourWindow trackTourWindow = new TrackTourWindow(new TourDTO(tour));
                trackTourWindow.ShowDialog();   
            }
            else if(selectedItem.Type.Equals(Model.Enums.MessageType.NewCreatedTour))
            {
                Tour tour = _tourService.GetById(selectedItem.RequestId);
                TourInformationWindow tourInformationWindow = new TourInformationWindow(new TourDTO(tour), _userDTO);
                tourInformationWindow.ShowDialog();
            }
            else if(selectedItem.Type.Equals(Model.Enums.MessageType.AcceptedTourRequest))
            {
                OrdinaryTourRequest ordinaryTourRequest = _ordinaryTourRequestService.GetById(selectedItem.RequestId);
                OrdinaryTourRequestInfoWindow ordinaryTourRequestInfoWindow = new OrdinaryTourRequestInfoWindow(new OrdinaryTourRequestDTO(ordinaryTourRequest));
                ordinaryTourRequestInfoWindow.ShowDialog();
            }
        }
    }
}
