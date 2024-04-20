using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Model.Enums;
using BookingApp.Repository;
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
using System.Windows.Input;
using Xceed.Wpf.AvalonDock.Layout;

namespace BookingApp.ViewModel.Tourist
{
    public class TouristMainViewModel:ViewModel
    {
        private TourService _tourService {  get; set; }
        private TourReservationService _tourReservationService { get; set; }
        private MessageService _messageService { get; set; }
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

        public TouristMainViewModel(UserDTO loggedInUser)
        {
            _message = new Message();
            _tourService = new TourService();
            _tourDTO = new TourDTO();
            _userDTO = loggedInUser;
            _tourReservationService = new TourReservationService();
            List<TourDTO> tours = _tourService.GetAll().Select(tours => new TourDTO(tours)).ToList();
            _toursDTO = new ObservableCollection<TourDTO>(tours);
            _filteredToursDTO = _toursDTO;
            _messageService = new MessageService();
            _searchCommand = new RelayCommand(Search);
            _showAppropriateWindowCommand = new RelayCommand(ShowAppropriateWindow);
            _showFinishedToursWindowCommand = new RelayCommand(ShowFinishedToursWindow);
            _showMyToursWindowCommand = new RelayCommand(ShowMyToursWindow);
            _showVoucherWindowCommand = new RelayCommand(ShowVoucherWindow);
            _showInboxWindowCommand = new RelayCommand(ShowinboxWindow);
            _messageService.CreateMessage(_message, loggedInUser.ToUser());
            CheckOpenPopUp();
            _popUpCommand = new RelayCommand(OpenPopUp);
            _closePopUpCommand = new RelayCommand(ClosePopUp);
            _logOutCommand = new RelayCommand(LogOut);

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
                ShowAppropriateWindow();
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

        public void ShowAppropriateWindow()
        {

            if (_selectedTourDTO == null)
            {
                return;
            }

            
            var selectedItem = _selectedTourDTO as TourDTO;
            if(selectedItem.CurrentCapacity > 0)
            {
                TourReservationWindow tourReservationWindow = new TourReservationWindow(_tourReservationService, new TourDTO(selectedItem), _userDTO);
                tourReservationWindow.ShowDialog();
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

        public void CheckOpenPopUp()
        {
            
            foreach (Message message in _messageService.GetAll())
            {
                if (message.Type.Equals(MessageType.TourAttendance) && !message.IsRead)
                {
                    IsOpen = true;
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
