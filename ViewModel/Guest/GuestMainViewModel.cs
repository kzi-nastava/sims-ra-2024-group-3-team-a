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

namespace BookingApp.ViewModel.Guest
{
    public class GuestMainViewModel: ViewModel
    {
        private ObservableCollection<AccommodationReservationDTO> _accommodationReservationsDTO;
        private ObservableCollection<AccommodationDTO> _accommodationsDTO;

        private AccommodationReservationService _accommodationReservationService;
        private AccommodationService _accommodationService;
        private AccommodationReservationChangeRequestService _accommodationReservationChangeRequestService;
        private MessageService _messageService;

        private UserService _userService;
        private UserDTO _loggedInGuest;

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
            _userService = new UserService(userRepository);
            _accommodationReservationService = new AccommodationReservationService(accommodationReservationRepository, accommodationRepository, userRepository);
            _accommodationService = new AccommodationService(accommodationRepository);
            _accommodationReservationChangeRequestService = new AccommodationReservationChangeRequestService(accommodationReservationChangeRequestRepository, accommodationReservationRepository, accommodationRepository, userRepository);
            _messageService = new MessageService(messageRepository, accommodationReservationChangeRequestRepository, accommodationReservationRepository, accommodationRepository, userRepository, tourRepository, tourReservationRepository, touristRepository, tourReviewRepository, voucherRepository);

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
            UpdateReservations();

        }

        public void UpdateReservations()
        {
            List<AccommodationReservationDTO> AccommodationReservationsList = _accommodationReservationService.GetAll().Select(accommodationReservation => new AccommodationReservationDTO(accommodationReservation)).ToList();
            _accommodationReservationsDTO = new ObservableCollection<AccommodationReservationDTO>(AccommodationReservationsList);
            List<AccommodationDTO> AcomodationsList = _accommodationService.GetAll().Select(accommodation => new AccommodationDTO(accommodation)).ToList(); 
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
            string searchNameInput = GuestMainWindow.Instance.searchNameTextBox.Text.ToLower();
            string searchCountryInput = GuestMainWindow.Instance.searchCountryTextBox.Text.ToLower();
            string searchCityInput = GuestMainWindow.Instance.searchCityTextBox.Text.ToLower();
            string searchTypeInput = GuestMainWindow.Instance.searchTypeTextBox.Text.ToLower();

            string searchCapacityInput = GuestMainWindow.Instance.searchCapacityTextBox.Text.ToLower();
            string searchMinDaysInput = GuestMainWindow.Instance.searchMinDaysTextBox.Text.ToLower();

            string allParams = searchCityInput + searchCountryInput + searchNameInput + searchTypeInput + searchCapacityInput + searchMinDaysInput;

            var filtered = AccommodationsDTO;

            if (allParams.Length == 0 || string.IsNullOrWhiteSpace(allParams))
            {
                GuestMainWindow.Instance.dataGridAccommodation.ItemsSource = AccommodationsDTO;
            }

            filtered = FilterAccommodations(filtered, searchCountryInput, searchCityInput, searchNameInput, searchTypeInput, searchCapacityInput, searchMinDaysInput);

            GuestMainWindow.Instance.dataGridAccommodation.ItemsSource = filtered;
        }

        private ObservableCollection<AccommodationDTO> FilterAccommodations(ObservableCollection<AccommodationDTO> accommodations, string searchCountryInput, string searchCityInput, string searchNameInput, string searchTypeInput, string searchCapacityInput, string searchMinDaysInput)
        {
            var filtered = new ObservableCollection<AccommodationDTO>();

            int? CapacityConvert = string.IsNullOrEmpty(searchCapacityInput) ? (int?)null : int.Parse(searchCapacityInput);
            int? MinDaysConvert = string.IsNullOrEmpty(searchMinDaysInput) ? (int?)null : int.Parse(searchMinDaysInput);

            foreach (var accommodation in accommodations)
            {
                if (accommodation.Name.ToLower().Contains(searchNameInput)
                    && accommodation.Type.ToString().ToLower().Contains(searchTypeInput)
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
