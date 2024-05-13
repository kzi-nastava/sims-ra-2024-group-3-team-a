using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.InjectorNameSpace;
using BookingApp.Model;
using BookingApp.Model.Enums;
using BookingApp.Repository;
using BookingApp.Repository.Interfaces;
using BookingApp.Service;
using BookingApp.View;
using BookingApp.View.Owner;
using BookingApp.View.Tourist;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Xceed.Wpf.AvalonDock.Layout;

namespace BookingApp.ViewModel.Tourist
{
    public class TouristMainViewModel:ViewModel
    {
        private TourService _tourService {  get; set; }
        private TourReservationService _tourReservationService { get; set; }
        private MessageService _messageService { get; set; }
        private OrdinaryTourRequestService _ordinaryTourRequestService { get; set; }
        private ObservableCollection<TourDTO> _toursDTO;
        private ObservableCollection<TourDTO> _filteredToursDTO;
        private Message _message { get; set; }
        private UserDTO _userDTO { get; set; }
        private TourDTO _tourDTO { get; set; }
        private TourDTO _selectedTourDTO { get; set; }
        private RelayCommand _searchCommand;
        private RelayCommand _showMyToursWindowCommand;
        private RelayCommand _showInboxWindowCommand;
        private RelayCommand _showFinishedToursWindowCommand;
        private RelayCommand _showVoucherWindowCommand;
        private RelayCommand _showAppropriateWindowCommand;
        private RelayCommand _popUpCommand;
        private RelayCommand _closePopUpCommand;
        private RelayCommand _logOutCommand;
        private RelayCommand _showCommand;
        private RelayCommand _showOrindaryTourRequestWindow;
        private RelayCommand _showTourRequestsCommand;
        private RelayCommand _resetSearchParametersCommand;

        public TouristMainViewModel(UserDTO loggedInUser)
        {
            _message = new Message();
            _tourDTO = new TourDTO();
            _userDTO = loggedInUser;
            
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
            IOrdinaryTourRequestRepository ordinaryTourRequestRepository = Injector.CreateInstance<IOrdinaryTourRequestRepository>();
            _messageService = new MessageService(messageRepository, accommodationReservationChangeRequestRepository, accommodationReservationRepository, accommodationRepository, userRepository, tourRepository, tourReservationRepository, touristRepository, tourReviewRepository, voucherRepository);
            _tourReservationService = new TourReservationService(tourReservationRepository, userRepository, touristRepository, tourReviewRepository, voucherRepository);
            _tourService = new TourService(tourRepository, userRepository, touristRepository, tourReservationRepository, tourReviewRepository, voucherRepository);
            _ordinaryTourRequestService = new OrdinaryTourRequestService(accommodationReservationChangeRequestRepository, accommodationReservationRepository, accommodationRepository, ordinaryTourRequestRepository, tourRepository, messageRepository, touristRepository, userRepository, tourReservationRepository, tourReviewRepository, voucherRepository);
            

            List<TourDTO> tours = _tourService.GetAll().Select(tours => new TourDTO(tours)).ToList();
       
            _toursDTO = new ObservableCollection<TourDTO>(tours);
            _filteredToursDTO = _toursDTO;
            _searchCommand = new RelayCommand(Search);
            _showAppropriateWindowCommand = new RelayCommand(ShowAppropriateWindow);
            _showFinishedToursWindowCommand = new RelayCommand(ShowFinishedToursWindow);
            _showMyToursWindowCommand = new RelayCommand(ShowMyToursWindow);
            _showVoucherWindowCommand = new RelayCommand(ShowVoucherWindow);
            _showInboxWindowCommand = new RelayCommand(ShowinboxWindow);
            _showOrindaryTourRequestWindow = new RelayCommand(ShowOrindaryTourRequestWindow);
            _messageService.CreateMessage(_message, _userDTO.ToUser());
            _ordinaryTourRequestService.GetCandidatesForMessages(_userDTO);
            _ordinaryTourRequestService.FindInvalidOrdinaryTourRequests(_userDTO);
             SendNotification();
            _popUpCommand = new RelayCommand(OpenPopUp);
            _closePopUpCommand = new RelayCommand(ClosePopUp);
            _logOutCommand = new RelayCommand(LogOut);
            _showCommand = new RelayCommand(ShowWindow);
            _showTourRequestsCommand = new RelayCommand(ShowTourRequestsWindow);
            _resetSearchParametersCommand = new RelayCommand(ResetSearchParameters);

        }

        public TourDTO TourDTO
        {
            get
            {
                return _tourDTO;
            }
            set
            {
                _tourDTO = value;
                OnPropertyChanged();
            }
        }

        public UserDTO UserDTO
        {
            get
            {
                return _userDTO;
            }
            set
            {
                _userDTO = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<TourDTO> ToursDTO
        {
            get
            {
                return _toursDTO;
            }

            set
            {
                _toursDTO = value;
                OnPropertyChanged();

            }
        }

        public ObservableCollection<TourDTO> FilteredToursDTO
        {
            get
            {
                return _filteredToursDTO;
            }

            set
            {
                _filteredToursDTO = value;
                OnPropertyChanged();

            }
        }
        public TourDTO SelectedTourDTO
        {
            get { return _selectedTourDTO; }
            set
            {
                _selectedTourDTO = value;
                OnPropertyChanged();
            }
        }
        public IEnumerable<Languages> Languages
        {
            get
            {
                return Enum.GetValues(typeof(Languages)).Cast<Languages>();
            }
            set { }
        }

        

        private bool _isOpen;
        public bool IsOpen
        {
            get
            {
                return _isOpen;
            }

            set
            {
                _isOpen = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand PopUpCommand
        {
            get
            {
                return _popUpCommand;
            }
            set
            {
                _popUpCommand = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand SearchCommand
        {
            get
            {
                return _searchCommand;
            }
            set
            {
                _searchCommand = value;
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

        public RelayCommand ResetSearchParametersCommand
        {
            get
            {
                return _resetSearchParametersCommand;
            }
            set
            {
                _resetSearchParametersCommand = value;
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


        public RelayCommand ClosePopUpCommand
        {
            get
            {
                return _closePopUpCommand;
            }
            set
            {
                _closePopUpCommand = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand LogOutCommand
        {
            get
            {
                return _logOutCommand;
            }
            set
            {
                _logOutCommand = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand ShowCommand
        {
            get
            {
                return _showCommand;
            }
            set
            {
                _showCommand = value;
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

        private string _searchCountryInput = String.Empty;
        public string SearchCountryInput
        {
            get
            {
               return _searchCountryInput;
            }
            set
            {
                _searchCountryInput = value;
                OnPropertyChanged();
            }
        }

        private string _searchCityInput = String.Empty;
        public string SearchCityInput
        {
            get
            {
                return _searchCityInput;
            }
            set
            {
                _searchCityInput = value;
                OnPropertyChanged();
            }
        }

        private string _searchLanguageInput =  String.Empty;
        public string SearchLanguageInput
        {
            get
            {
                return _searchLanguageInput;
            }
            set
            {
                _searchLanguageInput = value;
                OnPropertyChanged();
            }
        }

        private string _searchMaxTouristNumberInput = String.Empty;
        public string SearchMaxTouristNumberInput
        {
            get
            {
                return _searchMaxTouristNumberInput;
            }
            set
            {
                _searchMaxTouristNumberInput = value;
                OnPropertyChanged();
            }
        }

        private string _searchDurationInput = String.Empty;
        public string SearchDurationInput
        {
            get
            {
                return _searchDurationInput;
            }
            set
            {
                _searchDurationInput = value;
                OnPropertyChanged();
            }
        }

      

        private void Search()
        {
            

             var filtered = _toursDTO.Where(t =>
                 (t.Duration.ToString().Equals(SearchDurationInput) || SearchDurationInput.Equals(string.Empty))
                 && t.Language.ToString().ToLower().Contains(SearchLanguageInput.ToString().ToLower())
                 && t.LocationDTO.Country.ToLower().Contains(SearchCountryInput.ToLower())
                 && t.LocationDTO.City.ToLower().Contains(SearchCityInput.ToLower())
                 && t.MaxTouristNumber.ToString().Contains(SearchMaxTouristNumberInput.ToLower())
             );

            FilteredToursDTO = new ObservableCollection<TourDTO>(filtered);
        }

        private void ResetSearchParameters()
        {
            SearchCountryInput = string.Empty;
            SearchCityInput = string.Empty;
            TouristMainWindow.GetInstance().languageComboBox.SelectedIndex = -1;
            SearchLanguageInput = string.Empty;
            SearchMaxTouristNumberInput = string.Empty;
            SearchDurationInput = string.Empty;
            Search();

        }

        public void ShowAppropriateWindow()
        {

            if (_selectedTourDTO == null)
            {
                return;
            }

            
            var selectedItem = _selectedTourDTO as TourDTO;
     
           if(selectedItem.CurrentCapacity >0)
            {
                TourInformationWindow tourInformationWindow = new TourInformationWindow(new TourDTO(selectedItem), _userDTO);
        
                tourInformationWindow.ShowDialog();
           }
            else
            {
                AlternativeToursWindow alternativeToursWindow = new AlternativeToursWindow(new TourDTO(selectedItem), _userDTO);
   
                alternativeToursWindow.ShowDialog();
            }
           
        }

        public void ShowFinishedToursWindow()
        {
            FinishedToursWindow finishedToursWindow = new FinishedToursWindow(_userDTO);
            finishedToursWindow.ShowDialog();
        }

        public void ShowMyToursWindow()
        {
            MyToursWindow myToursWindow = new MyToursWindow();
            myToursWindow.ShowDialog();
        }

        public void ShowVoucherWindow()
        {
            VoucherWindow voucherWindow = new VoucherWindow();
            voucherWindow.ShowDialog();
        }

        public void ShowinboxWindow()
        {
            InboxWindow inboxWindow = new InboxWindow(_userDTO);
            inboxWindow.ShowDialog();
        }
        public void ShowWindow()
        {
          TourInformationWindow tourInformationWindow = new TourInformationWindow(_tourDTO, _userDTO);
            tourInformationWindow.ShowDialog();
        }
        public void ShowOrindaryTourRequestWindow()
        {
           OrdinaryTourRequestWindow ordinaryTourRequestWindow = new OrdinaryTourRequestWindow( _userDTO);
           ordinaryTourRequestWindow.ShowDialog();
        }
        public void ShowTourRequestsWindow()
        {
            TourRequestsWindow ordinaryTourRequestWindow = new TourRequestsWindow(_userDTO);
            ordinaryTourRequestWindow.ShowDialog();
        }
        public void SendNotification()
        {

            foreach (Message message in _messageService.GetAll())
            {
                if ((message.Type.Equals(MessageType.TourAttendance) || message.Type.Equals(MessageType.NewCreatedTour) || message.Type.Equals(MessageType.AcceptedTourRequest)) && !message.IsRead)
                {
                    IsOpen = true; //SetSendNotification
                    return;
                }
            }

            IsOpen = false;
        }
        public void OpenPopUp()
        {
            IsOpen = true;
        }

        public void ClosePopUp()
        {
            IsOpen = false;
        }

        public void LogOut()
        {
            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            TouristMainWindow.GetInstance().Close();
        }
    }
}
