using BookingApp.DTO;
using BookingApp.InjectorNameSpace;
using BookingApp.Repository.Interfaces;
using BookingApp.Repository;
using BookingApp.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Commands;
using Xceed.Wpf.Toolkit.PropertyGrid.Converters;
using BookingApp.View.Guest;
using BookingApp.Model.Enums;
using BookingApp.View;
using BookingApp.View.Guide;

namespace BookingApp.ViewModel.Guide
{
    class TourRequestViewModel : ViewModel
    {
        private ObservableCollection<OrdinaryTourRequestDTO> _requestsDTO { get; set; }
        private ObservableCollection<OrdinaryTourRequestDTO> _filteredRequestsDTO { get; set; }
        private OrdinaryTourRequestService _tourRequestService;
        private MessageService _messageService;
        private TourDTO _selectedRequestDTO = null;
        private UserDTO _userDTO;
        private RelayCommand _searchCommand;
        private RelayCommand _resetSearchParametersCommand;
        private RelayCommand _cancelTourRequestCommand;
        private RelayCommand _acceptTourRequestCommand;
        private RelayCommand _showTourRequestStatisticsCommand;
        public TourRequestViewModel(UserDTO user)
        {
            _userDTO = user;
            IOrdinaryTourRequestRepository requestRepository = Injector.CreateInstance<IOrdinaryTourRequestRepository>();
          
            _tourRequestService = new OrdinaryTourRequestService(requestRepository);
            List<OrdinaryTourRequestDTO> requestsDTO = _tourRequestService.GetOnWait().Select(request => new OrdinaryTourRequestDTO(request)).ToList();
            _requestsDTO = new ObservableCollection<OrdinaryTourRequestDTO>(requestsDTO);
            _filteredRequestsDTO = _requestsDTO;
            _searchCommand = new RelayCommand(Search);
            _resetSearchParametersCommand = new RelayCommand(ResetSearchParameters);
            _cancelTourRequestCommand = new RelayCommand(CancelTourRequest);
            _acceptTourRequestCommand = new RelayCommand(AcceptTourRequest);
            _showTourRequestStatisticsCommand = new RelayCommand(ShowTourRequestStatistic);
        }
        public ObservableCollection<OrdinaryTourRequestDTO> RequestsDTO
        {
            get { return _requestsDTO; }
            set
            {
                _requestsDTO = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<OrdinaryTourRequestDTO> FilteredRequestsDTO
        {
            get { return _filteredRequestsDTO; }
            set
            {
                _filteredRequestsDTO = value;
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
        public RelayCommand CancelTourRequestCommand
        {
            get
            {
                return _cancelTourRequestCommand;
            }
            set
            {
                _cancelTourRequestCommand = value;
                OnPropertyChanged();
            }
        }
        private void  CancelTourRequest(object parameter)
        {
            OrdinaryTourRequestDTO requestDTO = parameter as OrdinaryTourRequestDTO;
            requestDTO.Status = TourRequestStatus.Rejected;
            _tourRequestService.Update(requestDTO.ToOrdinaryTourRequest());
            FilteredRequestsDTO=new ObservableCollection<OrdinaryTourRequestDTO>(_tourRequestService.GetOnWait().Select(request => new OrdinaryTourRequestDTO(request)).ToList());
            RequestsDTO=new ObservableCollection<OrdinaryTourRequestDTO>(_tourRequestService.GetOnWait().Select(request => new OrdinaryTourRequestDTO(request)).ToList());
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
        
            public RelayCommand ShowTourRequestStatistics
        {
            get { return _showTourRequestStatisticsCommand; }
            set
            {
                _showTourRequestStatisticsCommand = value;
                OnPropertyChanged();
            }
        }
        private void ShowTourRequestStatistic()
        {
            TourRequestStatisticsWindow statistics = new TourRequestStatisticsWindow();
            statistics.Show();
        }
        private void AcceptTourRequest(object parameter)
        {
            OrdinaryTourRequestDTO requestDTO = parameter as OrdinaryTourRequestDTO;
            AcceptTourWindow acceptWindow = new AcceptTourWindow(requestDTO,_userDTO);
            acceptWindow.Show();
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

        private string _searchLanguageInput = String.Empty;
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

        private string _searchTouristNumberInput = String.Empty;
        public string SearchTouristNumberInput
        {
            get
            {
                return _searchTouristNumberInput;
            }
            set
            {
                _searchTouristNumberInput = value;
                OnPropertyChanged();
            }
        }

        private DateTime _searchBeginDateInput ;
        public DateTime SearchBeginDateInput
        {
            get
            {
                return _searchBeginDateInput;
            }
            set
            {
                _searchBeginDateInput = value;
                OnPropertyChanged();
            }
        }
        private DateTime _searchEndDateInput = default ;
        public DateTime SearchEndDateInput
        {
            get
            {
                return _searchEndDateInput;
            }
            set
            {
                if (value == null)
                {
                    _searchEndDateInput = default;
                }
                else
                {
                    _searchEndDateInput = value;
                }
                
                OnPropertyChanged();
            }
        }

        private void Search()
        {
            var filteredRequests = RequestsDTO.Where(request =>
                    (string.IsNullOrEmpty(SearchCityInput) || request.LocationDTO.City.Contains(SearchCityInput)) &&
                    (string.IsNullOrEmpty(SearchCountryInput) || request.LocationDTO.Country.Contains(SearchCountryInput)) &&
                    (string.IsNullOrEmpty(SearchLanguageInput) || request.Language.ToString().ToLower().Contains(SearchLanguageInput.ToString().ToLower()) ) &&
                    ((SearchBeginDateInput == default || SearchEndDateInput == default) || (request.BeginDate >= SearchBeginDateInput && request.BeginDate < SearchEndDateInput && request.EndDate <= SearchEndDateInput && request.EndDate > SearchBeginDateInput)) &&
                    (string.IsNullOrEmpty(SearchTouristNumberInput) || request.NumberOfTourists.ToString().Contains(SearchTouristNumberInput))
                ).ToList();

            FilteredRequestsDTO = new ObservableCollection<OrdinaryTourRequestDTO>(filteredRequests);
        }

        public TourDTO SelectedRequestDTO
        {
            get { return _selectedRequestDTO; }
            set
            {
                _selectedRequestDTO = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand ResetSearchParametersCommand
        {

            get { return _resetSearchParametersCommand; }
            set
            {
                _resetSearchParametersCommand = value;
                OnPropertyChanged();
            }
        }
        private void ResetSearchParameters()
        {
            SearchCountryInput = string.Empty;
            SearchCityInput = string.Empty;
            TourRequestWindow.GetInstance().languageComboBox.SelectedIndex = -1;
            SearchLanguageInput = string.Empty;
            SearchTouristNumberInput = string.Empty;
            TourRequestWindow.GetInstance().datePicker.SelectedDate = null;
            TourRequestWindow.GetInstance().datePicker2.SelectedDate = null;
            SearchEndDateInput = default;
            SearchBeginDateInput = default;
            Search();

        }

    }
}
    