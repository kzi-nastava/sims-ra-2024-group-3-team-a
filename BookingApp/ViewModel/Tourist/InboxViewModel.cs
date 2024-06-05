using BookingApp.Commands;
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
        private RelayCommand _showMessageCommand;
   
        private RelayCommand _showMyToursWindowCommand;
        private RelayCommand _showInboxWindowCommand;
        private RelayCommand _showFinishedToursWindowCommand;
        private RelayCommand _showVoucherWindowCommand;
        private RelayCommand _showAppropriateWindowCommand;
     
        private RelayCommand _showOrindaryTourRequestWindow;
      
        private RelayCommand _showTourRequestsCommand;
        public Action CloseAction { get; set; }
        private RelayCommand _closeWindowCommand;
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
            _showMessageCommand = new RelayCommand(ShowMessageDetailsWindow);
            _closeWindowCommand = new RelayCommand(CloseWindow);
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
                
            }
        }
        public RelayCommand ShowMessageCommand
        {
            get
            {
                return _showMessageCommand;
            }
            set
            {
                _showMessageCommand = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand CloseWindowCommand
        {
            get
            {
                return _closeWindowCommand;
            }
            set
            {
                _closeWindowCommand = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand ShowMyToursWindowCommand
        {
            get
            {
                return _showMyToursWindowCommand;
            }
            set
            {
                _showMyToursWindowCommand = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand ShowFinishedToursWindowCommand
        {
            get
            {
                return _showFinishedToursWindowCommand;
            }
            set
            {
                _showFinishedToursWindowCommand = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand ShowTourRequestsCommand
        {
            get
            {
                return _showTourRequestsCommand;
            }
            set
            {
                _showTourRequestsCommand = value;
                OnPropertyChanged();
            }
        }


        public RelayCommand ShowVoucherWindowCommand
        {
            get
            {
                return _showVoucherWindowCommand;
            }
            set
            {
                _showVoucherWindowCommand = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand ShowAppropriateWindowCommand
        {
            get
            {
                return _showAppropriateWindowCommand;
            }
            set
            {
                _showAppropriateWindowCommand = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand ShowOrindaryTourRequestWindowCommand
        {
            get
            {
                return _showOrindaryTourRequestWindow;
            }
            set
            {
                _showOrindaryTourRequestWindow = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand ShowInboxWindowCommand
        {
            get
            {
                return _showInboxWindowCommand;
            }
            set
            {
                _showInboxWindowCommand = value;
                OnPropertyChanged();
            }
        }
        private void ShowMessageDetailsWindow(object parameter)
        {
            var selectedItem = parameter as MessageDTO;
            if (selectedItem == null)
            {
                return;
            }


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
        public void CloseWindow()
        {


            CloseAction();
        }


    }
}
