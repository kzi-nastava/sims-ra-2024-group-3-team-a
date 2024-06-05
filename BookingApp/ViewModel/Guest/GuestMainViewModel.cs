using BookingApp.DTO;
using BookingApp.Service;
using BookingApp.View.Guest;
using BookingApp.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BookingApp.Repository;
using BookingApp.Commands;
using BookingApp.Repository.Interfaces;
using BookingApp.InjectorNameSpace;
using BookingApp.View.Owner;
using BookingApp.Model;

namespace BookingApp.ViewModel.Guest
{
    public class GuestMainViewModel: ViewModel
    {
        private ObservableCollection<AccommodationReservationDTO> _accommodationReservationsDTO;
        public static ObservableCollection<AccommodationDTO> _accommodationsDTO;

        private AccommodationReservationService _accommodationReservationService;
        private AccommodationService _accommodationService;
        private AccommodationReservationChangeRequestService _accommodationReservationChangeRequestService;
        private MessageService _messageService;

        private UserService _userService;
        private UserDTO _loggedInGuest;
        private SuperGuestService _superGuestService;

        private ObservableCollection<AccommodationReservationChangeRequestDTO> _myChangeRequestsDTO;
        private ObservableCollection<MessageDTO> _myMessagesDTO;
        private AccommodationDTO _selectedAccommodationDTO;
        private bool _isFrameMyRequestsVisible;
        private bool _isFrameMyInboxVisible;

        private RelayCommand _logOutCommand;
        private RelayCommand _showMyRequestsCommand;
        private RelayCommand _showGuestReservationsCommand;
        private RelayCommand _increaseButtonCapacityCommand;
        private RelayCommand _decreaseButtonCapacityCommand;
        private RelayCommand _increaseButtonMinDaysCommand;
        private RelayCommand _decreaseButtonMinDaysCommand;
        private RelayCommand _showAccommodationReservationsPageCommand;
        private RelayCommand _searchAccommodationsCommand;
        private RelayCommand _showInboxCommand;
        private RelayCommand _showRatingsFromOwnersCommand;
        private RelayCommand _showMyProfileCommand;
        private RelayCommand _showSideMenuCommand;
        

        public GuestMainViewModel(UserDTO loggedInGuest)
        {
            _loggedInGuest = loggedInGuest;
            
            IAccommodationReservationRepository accommodationReservationRepository = Injector.CreateInstance<IAccommodationReservationRepository>();
            IAccommodationReservationChangeRequestRepository accommodationReservationChangeRequestRepository = Injector.CreateInstance<IAccommodationReservationChangeRequestRepository>();
            IMessageRepository messageRepository = Injector.CreateInstance<IMessageRepository>();
            IAccommodationRepository accommodationRepository = Injector.CreateInstance<IAccommodationRepository>();
            IUserRepository userRepository = Injector.CreateInstance<IUserRepository>();
            ITourRepository tourRepository = Injector.CreateInstance<ITourRepository>();
            ITouristRepository touristRepository = Injector.CreateInstance<ITouristRepository>();
            ITourReservationRepository tourReservationRepository = Injector.CreateInstance<ITourReservationRepository>();
            ITourReviewRepository tourReviewRepository = Injector.CreateInstance<ITourReviewRepository>();
            IVoucherRepository voucherRepository = Injector.CreateInstance<IVoucherRepository>();
            ISuperGuestRepository superGuestRepository = Injector.CreateInstance<ISuperGuestRepository>();
            _userService = new UserService(userRepository);
            _accommodationReservationService = new AccommodationReservationService(accommodationReservationRepository, accommodationRepository, userRepository);
            _accommodationService = new AccommodationService(accommodationRepository);
            _accommodationReservationChangeRequestService = new AccommodationReservationChangeRequestService(accommodationReservationChangeRequestRepository, accommodationReservationRepository, accommodationRepository, userRepository);
            _messageService = new MessageService(messageRepository, accommodationReservationChangeRequestRepository, accommodationReservationRepository, accommodationRepository, userRepository, tourRepository, tourReservationRepository, touristRepository, tourReviewRepository, voucherRepository);
            _superGuestService = new SuperGuestService(superGuestRepository);

            _logOutCommand = new RelayCommand(LogOut);
            _showMyRequestsCommand = new RelayCommand(ShowMyRequests);
            _showGuestReservationsCommand = new RelayCommand(ShowGuestReservationsPage);
            _increaseButtonCapacityCommand = new RelayCommand(IncreaseButtonCapacity);
            _decreaseButtonCapacityCommand = new RelayCommand(DecreaseButtonCapacity);
            _increaseButtonMinDaysCommand= new RelayCommand(IncreaseButtonMinDays);
            _decreaseButtonMinDaysCommand = new RelayCommand(DecreaseButtonMinDays);
            _showAccommodationReservationsPageCommand = new RelayCommand(ShowAccommodationReservationPage);
            _searchAccommodationsCommand = new RelayCommand(SearchAccommodations);
            _showInboxCommand = new RelayCommand(MyInbox);
            _showRatingsFromOwnersCommand = new RelayCommand(ShowRatingsFromOwnersPage);
            _showMyProfileCommand = new RelayCommand(ShowMyProfilePage);
            _showSideMenuCommand = new RelayCommand(ShowSideMenu);

            GuestMainWindow._userDTO = new UserDTO(_accommodationReservationService.SetSuperGuest(_loggedInGuest.ToUser()));
            IsSuperGuest();

            UpdateReservations();

        }

        private void IsSuperGuest()
        {
            if (GuestMainWindow._userDTO.IsSuper)
            {
                SuperGuest superGuest = _superGuestService.GetByUserId(GuestMainWindow._userDTO.Id);
                if ( superGuest != null)
                {
                    if(superGuest.EndingDate < DateOnly.FromDateTime(DateTime.Now))
                    {
                        superGuest.EndingDate = DateOnly.FromDateTime(DateTime.Now).AddDays(365);
                        superGuest.Points = 5;
                        _superGuestService.Update(superGuest);
                    }
                }
                else
                {
                    superGuest = new SuperGuest(0, GuestMainWindow._userDTO.Id, 5, DateOnly.FromDateTime(DateTime.Now).AddDays(365));
                    _superGuestService.Save(superGuest);
                }
            }
        }

        public void UpdateReservations()
        {
            List<AccommodationReservationDTO> AccommodationReservationsList = _accommodationReservationService.GetAll().Select(accommodationReservation => new AccommodationReservationDTO(accommodationReservation)).ToList();
            _accommodationReservationsDTO = new ObservableCollection<AccommodationReservationDTO>(AccommodationReservationsList);
            List<AccommodationDTO> AcomodationsList = _accommodationService.GetAll().Select(accommodation => new AccommodationDTO(accommodation)).OrderByDescending(a => _userService.GetById(a.OwnerId).IsSuper).ToList(); 
            _accommodationsDTO = new ObservableCollection<AccommodationDTO>(AcomodationsList);
            List<AccommodationReservationChangeRequestDTO> MyRequestsList = _accommodationReservationChangeRequestService.GetAllByGuestId(_loggedInGuest.Id).Select(request => new AccommodationReservationChangeRequestDTO(request)).ToList();
            _myChangeRequestsDTO = new ObservableCollection<AccommodationReservationChangeRequestDTO>(MyRequestsList);
            MyChangeRequestsDTO = _myChangeRequestsDTO;
            List<MessageDTO> MyMessagesList = _messageService.GetByOwner(_loggedInGuest.Id).Select(message => new MessageDTO(message)).ToList();
            _myMessagesDTO = new ObservableCollection<MessageDTO>(MyMessagesList);
        }
        public UserDTO GetUserDTOById(int id)
        {
            return new UserDTO(_userService.GetById(id));
        }

        private void ShowGuestReservationsPage()
        {

            GuestMainWindow.MainFrame.Content = new GuestReservationsPage(_loggedInGuest);
            IsFrameMyRequestsVisible = false;
            IsFrameMyInboxVisible = false;
            GuestMainWindow.MainFrame.Visibility = Visibility.Visible;

        }
        private void ShowRatingsFromOwnersPage()
        {
            GuestMainWindow.MainFrame.Content = new GuestReviewsFromOwners(_loggedInGuest);
            IsFrameMyRequestsVisible = false;
            IsFrameMyInboxVisible = false;
            GuestMainWindow.MainFrame.Visibility = Visibility.Visible;
        }
        private void ShowSideMenu()
        {
            GuestMainViewWindow.SideMenuFrame.Content = new GuestSideMenuPage();
        }
        private void ShowMyProfilePage()
        {
            GuestMainWindow.MainFrame.Content = new GuestProfilePage(_loggedInGuest);
            IsFrameMyRequestsVisible = false;
            IsFrameMyInboxVisible = false;
            GuestMainWindow.MainFrame.Visibility = Visibility.Visible;
        }
        private void MyInbox()
        {
            UpdateReservations();
            IsFrameMyInboxVisible = true;
            IsFrameMyRequestsVisible = false;
        }

        private void IncreaseButtonCapacity()
        {
            if (int.TryParse(GuestMainWindow.Instance.searchCapacityTextBox.Text, out int value))
            {
                GuestMainWindow.Instance.searchCapacityTextBox.Text = (value + 1).ToString();
            }
        }
        private void DecreaseButtonCapacity()
        {
            if (int.TryParse(GuestMainWindow.Instance.searchCapacityTextBox.Text, out int value))
            {
                GuestMainWindow.Instance.searchCapacityTextBox.Text = (value - 1).ToString();
            }
        }

        private void IncreaseButtonMinDays()
        {
            if (int.TryParse(GuestMainWindow.Instance.searchMinDaysTextBox.Text, out int value))
            {
                GuestMainWindow.Instance.searchMinDaysTextBox.Text = (value + 1).ToString();
            }
        }
        private void DecreaseButtonMinDays()
        {
            if (int.TryParse(GuestMainWindow.Instance.searchMinDaysTextBox.Text, out int value))
            {
                GuestMainWindow.Instance.searchMinDaysTextBox.Text = (value - 1).ToString();
            }
        }

        private void ShowMyRequests()
        {
            UpdateReservations();
            GuestMainWindow.MainFrame.Visibility = Visibility.Collapsed;
            IsFrameMyRequestsVisible = true;
            IsFrameMyInboxVisible = false;

        }
        private void ShowAccommodationReservationPage()
        {
            if (_selectedAccommodationDTO != null && GuestMainWindow.MainFrame.Content == null)
            {
                GuestMainWindow.MainFrame.Content = new MakeAccommodationReservationPage(_selectedAccommodationDTO, _loggedInGuest);

            }
            else if (GuestMainWindow.MainFrame.Content == null)
            {
                MessageBox.Show("Accommodation not selected");
            }
            else
            {
                GuestMainWindow.MainFrame.Content = null;
            }
        }

        private void SearchAccommodations()
        {
            string searchNameInput = GuestHomePage.Instance.searchNameTextBox.Text.ToLower();
            string searchCountryInput = GuestHomePage.Instance.searchCountryTextBox.Text.ToLower();
            string searchCityInput = GuestHomePage.Instance.searchCityTextBox.Text.ToLower();
            //string searchTypeInput = GuestHomePage.Instance.searchTypeTextBox.Text.ToLower();

            var selectedTypes = new List<string>();
            if (GuestHomePage.Instance.checkBoxApartment.IsChecked == true)
                selectedTypes.Add("apartment");
            if (GuestHomePage.Instance.checkBoxCottage.IsChecked == true)
                selectedTypes.Add("cottage");
            if (GuestHomePage.Instance.checkBoxHouse.IsChecked == true)
                selectedTypes.Add("house");

            string searchCapacityInput = GuestHomePage.Instance.searchCapacityTextBox.Text.ToLower();
            string searchMinDaysInput = GuestHomePage.Instance.searchMinDaysTextBox.Text.ToLower();

            string allParams = searchCityInput + searchCountryInput + searchNameInput + string.Join("", selectedTypes) + searchCapacityInput + searchMinDaysInput;

            var filtered = AccommodationsDTO;

            if (allParams.Length == 0 || string.IsNullOrWhiteSpace(allParams))
            {
                GuestHomePage.Instance.dataGridAccommodation.ItemsSource = AccommodationsDTO;
            }

            filtered = FilterAccommodations(filtered, searchCountryInput, searchCityInput, searchNameInput, selectedTypes, searchCapacityInput, searchMinDaysInput);
            GuestHomePage.Instance.dataGridAccommodation.ItemsSource = filtered;
        }

        private ObservableCollection<AccommodationDTO> FilterAccommodations(ObservableCollection<AccommodationDTO> accommodations, string searchCountryInput, string searchCityInput, string searchNameInput, List<string> selectedTypes, string searchCapacityInput, string searchMinDaysInput)
        {
            var filtered = new ObservableCollection<AccommodationDTO>();

            int? CapacityConvert = string.IsNullOrEmpty(searchCapacityInput) ? (int?)null : int.Parse(searchCapacityInput);
            int? MinDaysConvert = string.IsNullOrEmpty(searchMinDaysInput) ? (int?)null : int.Parse(searchMinDaysInput);

            foreach (var accommodation in accommodations)
            {
                bool typeMatches = selectedTypes.Count == 0 || selectedTypes.Contains(accommodation.Type.ToString().ToLower());

                if (accommodation.Name.ToLower().Contains(searchNameInput)
                    && typeMatches
                    && accommodation.PlaceDTO.Country.ToLower().Contains(searchCountryInput)
                    && accommodation.PlaceDTO.City.ToLower().Contains(searchCityInput)
                    && (accommodation.Capacity >= CapacityConvert || CapacityConvert == null)
                    && (accommodation.MinDaysReservation <= MinDaysConvert || MinDaysConvert == null))
                {
                    filtered.Add(accommodation);
                }
            }
            return filtered;
        }

        private void LogOut()
        {
            SignInForm signInForm = new SignInForm();
            GuestMainWindow.GetInstance().Close();
            signInForm.Show();
        }
        
        public ObservableCollection<MessageDTO> MyMessagesDTO
        {
            get
            {
                return _myMessagesDTO;
            }
            set
            {
                _myMessagesDTO = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<AccommodationReservationDTO> AccommodationReservationsDTO
        {
            get
            {
                return _accommodationReservationsDTO;
            }
            set
            {
                _accommodationReservationsDTO = value;
                OnPropertyChanged();
            }
        }
        public bool IsFrameMyRequestsVisible
        {
            get
            {
                return _isFrameMyRequestsVisible;
            }
            set
            {
                _isFrameMyRequestsVisible = value;
                OnPropertyChanged();
            }
        }
        public bool IsFrameMyInboxVisible
        {
            get
            {
                return _isFrameMyInboxVisible;
            }
            set
            {
                _isFrameMyInboxVisible = value;
                OnPropertyChanged();
            }
        }
        public UserDTO GuestDTO
        {
            get
            {
                return _loggedInGuest;
            }
            set
            {
                _loggedInGuest = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<AccommodationDTO> AccommodationsDTO
        {
            get
            {
                return _accommodationsDTO;
            }
            set
            {
                _accommodationsDTO = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<AccommodationReservationChangeRequestDTO> MyChangeRequestsDTO
        {
            get
            {
                return _myChangeRequestsDTO;
            }
            set
            {
                _myChangeRequestsDTO = value;
                OnPropertyChanged();
            }
        }

        public AccommodationDTO SelectedAccommodationDTO
        {
            get
            {
                return _selectedAccommodationDTO;
            }
            set
            {
                _selectedAccommodationDTO = value;
                OnPropertyChanged();
                ShowFindAvailableDatesPage();
                _selectedAccommodationDTO = null;
            }
        }

        private void ShowFindAvailableDatesPage()
        {
            GuestMainViewWindow.MainFrame.Content = new FindAvailableDatesPage(SelectedAccommodationDTO, _loggedInGuest);
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
        public RelayCommand ShowMyRequestsCommand
        {
            get
            {
                return _showMyRequestsCommand;
            }
            set
            {
                _showMyRequestsCommand = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand ShowRatingsFromOwnersCommand
        {
            get
            {
                return _showRatingsFromOwnersCommand;
            }
            set
            {
                _showRatingsFromOwnersCommand = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand ShowMyProfileCommand
        {
            get
            {
                return _showMyProfileCommand;
            }
            set
            {
                _showMyProfileCommand = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand ShowGuestReservationsCommand
        {
            get
            {
                return _showGuestReservationsCommand;
            }
            set
            {
                _showGuestReservationsCommand = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand SearchAccommodationsCommand
        {
            get
            {
                return _searchAccommodationsCommand;
            }
            set
            {
                _searchAccommodationsCommand = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand IncreaseButtonCapacityCommand
        {
            get
            {
                return _increaseButtonCapacityCommand;
            }
            set
            {
                _increaseButtonCapacityCommand = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand DecreaseButtonCapacityCommand
        {
            get
            {
                return _decreaseButtonCapacityCommand;
            }
            set
            {
                _decreaseButtonCapacityCommand = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand IncreaseButtonMinDaysCommand
        {
            get
            {
                return _increaseButtonMinDaysCommand;
            }
            set
            {
                _increaseButtonMinDaysCommand = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand DecreaseButtonMinDaysCommand
        {
            get
            {
                return _decreaseButtonMinDaysCommand;
            }
            set
            {
                _decreaseButtonMinDaysCommand = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand ShowAccommodationReservationsPageCommand
        {
            get
            {
                return _showAccommodationReservationsPageCommand;
            }
            set
            {
                _showAccommodationReservationsPageCommand = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand ShowInboxCommand
        {
            get
            {
                return _showInboxCommand;
            }
            set
            {
                _showInboxCommand = value;
                OnPropertyChanged();
            }
        }
    }
}
