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
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Xceed.Wpf.AvalonDock.Layout;

namespace BookingApp.ViewModel.Tourist
{
    public class TouristMainViewModel:Validation.ValidationBase
    {
        private TourService _tourService {  get; set; }
        private TourReservationService _tourReservationService { get; set; }
        private MessageService _messageService { get; set; }
        private UserService _userService { get; set; }
        private ComplexTourRequestService _complexTourRequestService { get; set; }
        private OrdinaryTourRequestService _ordinaryTourRequestService { get; set; }
        private VoucherService _voucherService { get; set; }
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
        private RelayCommand _showSettingsWindowCommand;
        private RelayCommand _closeWindowCommand;
        private App app;
        private string _currentLanguage;
        public Action CloseAction { get; set; }
        public RelayCommand OpenDropDownComboboxCommand { get; private set; }
        public RelayCommand OpenSugmenuCommand { get; private set; }
        public TouristMainViewModel(UserDTO loggedInUser)
        {
            _message = new Message();
            _tourDTO = new TourDTO();
            _userDTO = loggedInUser;
           
            IComplexTourRequestRepository complexTourRequestRepository = Injector.CreateInstance<IComplexTourRequestRepository>();
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
            _complexTourRequestService = new ComplexTourRequestService(accommodationReservationChangeRequestRepository, accommodationReservationRepository, accommodationRepository, ordinaryTourRequestRepository, tourRepository, messageRepository, touristRepository, userRepository, tourReservationRepository, tourReviewRepository, voucherRepository, complexTourRequestRepository);
            _voucherService = new VoucherService(voucherRepository);
            List<TourDTO> tours = _tourService.GetAllForTourist().Select(tours => new TourDTO(tours)).ToList();
       
            _toursDTO = new ObservableCollection<TourDTO>(tours);
            _filteredToursDTO = _toursDTO;
            _searchCommand = new RelayCommand(Search);
            _showAppropriateWindowCommand = new RelayCommand(ShowAppropriateWindow);
            _showFinishedToursWindowCommand = new RelayCommand(ShowFinishedToursWindow);
            _showMyToursWindowCommand = new RelayCommand(ShowMyToursWindow);
            _showVoucherWindowCommand = new RelayCommand(ShowVoucherWindow);
            _showInboxWindowCommand = new RelayCommand(ShowinboxWindow);
            _showOrindaryTourRequestWindow = new RelayCommand(ShowOrindaryTourRequestWindow);
            _showSettingsWindowCommand =new RelayCommand(ShowSettingsWindow);
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
            _complexTourRequestService.CheckForInvalidComplexTourRequests(_userDTO.Id);
            _tourService.FindCandidatesForVoucher(_userDTO.Id);
            _voucherService.DeleteExpiredVouchers(_userDTO.Id);
            _userService = new UserService(userRepository);
            OpenDropDownComboboxCommand = new RelayCommand(OpenCombobox);
            OpenSugmenuCommand = new RelayCommand(OpenSubmenu);
            _closeWindowCommand = new RelayCommand(CloseWindow);
            app = (App)System.Windows.Application.Current;
            app.ChangeLanguage("en-US");
            var currentLanguage = App.Instance.CurrentLanguage.Name;
            _currentLanguage = currentLanguage;
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
        public RelayCommand ShowSettingsWindowCommand
        {
            get
            {
                return _showSettingsWindowCommand;
            }
            set
            {
                _showSettingsWindowCommand = value;
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

        private bool _isDropDownComboboxOpenCommand;
        public bool IsDropDownComboboxOpenCommand
        {
            get
            {
                return _isDropDownComboboxOpenCommand;
            }
            set
            {
                _isDropDownComboboxOpenCommand = value;
                OnPropertyChanged();
            }
        }

        private void Search()
        {
            Validate1();
            if(IsValid)
            {
                if(Regex.IsMatch(SearchDurationInput, @"^\d+\.0$"))
                {
                    if (double.TryParse(SearchDurationInput, out double durationDouble))
                    {
                       
                        int durationInt = (int)durationDouble;
                        SearchDurationInput = durationInt.ToString();
                    }
                    
                }
                var filtered = _toursDTO.Where(t =>
                (t.Duration.ToString().Equals(SearchDurationInput) || SearchDurationInput.Equals(string.Empty))
                && t.Language.ToString().ToLower().Contains(SearchLanguageInput.ToString().ToLower())
                && t.LocationDTO.Country.ToLower().Contains(SearchCountryInput.ToLower())
                && t.LocationDTO.City.ToLower().Contains(SearchCityInput.ToLower())
                && t.MaxTouristNumber.ToString().Contains(SearchMaxTouristNumberInput.ToLower())
            );

                FilteredToursDTO = new ObservableCollection<TourDTO>(filtered);
            }

            
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
            MyToursWindow myToursWindow = new MyToursWindow(_userDTO);
            myToursWindow.ShowDialog();
        }

        public void ShowVoucherWindow()
        {
            VoucherWindow voucherWindow = new VoucherWindow(_userDTO);
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
          
          CreateTourRequestWindow createTourRequestWindow = new CreateTourRequestWindow(_userDTO);
           createTourRequestWindow.ShowDialog();
        }
        public void ShowTourRequestsWindow()
        {
            TourRequestsWindow ordinaryTourRequestWindow = new TourRequestsWindow(_userDTO);
            ordinaryTourRequestWindow.ShowDialog();
        }
        public void ShowSettingsWindow()
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.ShowDialog();
        }

        public void SendNotification()
        {

            foreach (Message message in _messageService.GetAll())
            {
                if ((message.Type.Equals(MessageType.TourAttendance) || message.Type.Equals(MessageType.NewCreatedTour) || message.Type.Equals(MessageType.AcceptedTourRequest)) && !message.IsRead)
                {
                    IsOpen = true; 
                    return;
                }
            }

            IsOpen = false;
        }

        protected override void ValidateSelf1()
        {
            
            if (!Regex.IsMatch(_searchCountryInput, @"^[a-zA-Z]+$") && !string.IsNullOrEmpty(_searchCountryInput))
            {
                if (_currentLanguage.Equals("en-US"))
                {
                    ValidationErrors["Country"] = "Contry can contains only latters.";
                }
                else
                {
                    ValidationErrors["Country"] = "Drzava moze sadrzati samo slova.";
                }
                    
            }
            if (!Regex.IsMatch(_searchCityInput, @"^[a-zA-Z]+$")&& !string.IsNullOrEmpty(_searchCityInput))
            {
                if (_currentLanguage.Equals("en-US"))
                {
                    ValidationErrors["City"] = "City can contains only latters.";
                }
                else
                {
                    ValidationErrors["City"] = "Grad moze sadrzati samo slova";
                }
              
            }
            if(!Regex.IsMatch(_searchMaxTouristNumberInput, @"^\d*$") && !string.IsNullOrEmpty(_searchMaxTouristNumberInput))
            {
                if (_currentLanguage.Equals("en-US"))
                {
                    ValidationErrors["MaxTourists"] = "Number of tourists contains only integer numbers.";
                }
                else
                {
                    ValidationErrors["MaxTourists"] = "Broj turista moze biti samo cio broj.";
                }
                   
            }
            if (!Regex.IsMatch(_searchDurationInput, @"^\d+(\.\d+)?$") && !string.IsNullOrEmpty(_searchDurationInput))
            {
                if (_currentLanguage.Equals("en-US"))
                {
                    ValidationErrors["Duration"] = "Wrong format of number.";
                }
                else
                {
                    ValidationErrors["Duration"] = "Pogresan format broja.";
                }
            }

        }
        protected override void ValidateSelf2()
        {
            throw new NotImplementedException();

        }


        public void OpenPopUp()
        {
            IsOpen = true;
        }
        private void OpenCombobox()
        {
            IsDropDownComboboxOpenCommand = true;
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
        public bool isSuperGuide(int id)
        {
            User guide = new User();
            guide = _userService.GetById(id);

            return guide.IsSuper;
            
        }
        private bool _isSubmenuOpenCommand;
        public bool IsSubmenuOpenCommand
        {
            get
            {
                return _isSubmenuOpenCommand;
            }
            set
            {
                _isSubmenuOpenCommand = value;
                OnPropertyChanged();
            }
        }
        private void OpenSubmenu()
        {
            IsSubmenuOpenCommand = true;
        }
        public void CloseWindow()
        {
            CloseAction();
        }
    }
}
