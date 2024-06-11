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
using System.Collections;
using BookingApp.Model;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Input;

namespace BookingApp.ViewModel.Guide
{
    class TourRequestViewModel : ViewModel
    {
        private ObservableCollection<OrdinaryTourRequestDTO> _requestsDTO { get; set; }
        private ObservableCollection<ComplexTourRequestDTO> _complexRequestsDTO { get; set; }
        private ObservableCollection<OrdinaryTourRequestDTO> _filteredRequestsDTO { get; set; }
        private OrdinaryTourRequestService _tourRequestService;
        private ComplexTourRequestService _complexRequestService;
        private TourDTO _selectedRequestDTO = null;
        private UserDTO _userDTO;
        private RelayCommand _searchCommand;
        private RelayCommand _resetSearchParametersCommand;
        private RelayCommand _cancelTourRequestCommand;
        private RelayCommand _acceptTourRequestCommand;
        private RelayCommand _logoutCommand;
        private RelayCommand _showMainWindowCommand;
        private RelayCommand _showAllToursCommand;
        private RelayCommand _requestDetailsCommand;
        private RelayCommand _acceptComplexTourRequestCommand;
        private Dictionary<ComplexTourRequestDTO, ObservableCollection<OrdinaryTourRequestDTO>> _dictionary;
        private Dictionary<ComplexTourRequestDTO, ObservableCollection<OrdinaryTourRequestDTO>> _filteredDictionary;
        private RelayCommand _showTourRequestStatisticsCommand;
        private RelayCommand _showTourStatisticsCommand;
        private RelayCommand _addNewTourCommand;
        private readonly RequestReportService _reportService;
        public RelayCommand GenerateReportCommand { get; }
        public Dictionary<ComplexTourRequestDTO, ObservableCollection<OrdinaryTourRequestDTO>> keyValuePairs { get; set; }
        public TourRequestViewModel(UserDTO user)
        {
            _userDTO = user;
            IOrdinaryTourRequestRepository requestRepository = Injector.CreateInstance<IOrdinaryTourRequestRepository>();
            IMessageRepository messageRepository = Injector.CreateInstance<IMessageRepository>();
            ITourRepository tourRepository = Injector.CreateInstance<ITourRepository>();
            IUserRepository userRepository = Injector.CreateInstance<IUserRepository>();
            IKeyPointRepository keyPointsRepository = Injector.CreateInstance<IKeyPointRepository>();
            ITouristRepository touristRepository = Injector.CreateInstance<ITouristRepository>();
            ITourReservationRepository tourReservationRepository = Injector.CreateInstance<ITourReservationRepository>();
            ITourReviewRepository tourReviewRepository = Injector.CreateInstance<ITourReviewRepository>();
            IVoucherRepository voucherRepository = Injector.CreateInstance<IVoucherRepository>();
            IAccommodationReservationChangeRequestRepository accommodationReservationChangeRequestRepository = Injector.CreateInstance<IAccommodationReservationChangeRequestRepository>();
            IAccommodationReservationRepository accommodationReservationRepository = Injector.CreateInstance<IAccommodationReservationRepository>();
            IAccommodationRepository accommodationRepository = Injector.CreateInstance<IAccommodationRepository>();
            IComplexTourRequestRepository complexTourRequestRepository = Injector.CreateInstance<IComplexTourRequestRepository>();
            _tourRequestService = new OrdinaryTourRequestService(accommodationReservationChangeRequestRepository, accommodationReservationRepository, accommodationRepository, requestRepository, tourRepository, messageRepository, touristRepository, userRepository, tourReservationRepository, tourReviewRepository, voucherRepository);
            _complexRequestService = new ComplexTourRequestService(accommodationReservationChangeRequestRepository, accommodationReservationRepository, accommodationRepository, requestRepository, tourRepository, messageRepository, touristRepository, userRepository, tourReservationRepository, tourReviewRepository, voucherRepository, complexTourRequestRepository);
            List<OrdinaryTourRequestDTO> requestsDTO = _tourRequestService.GetOnWait().Select(request => new OrdinaryTourRequestDTO(request)).ToList();
            List<OrdinaryTourRequestDTO> requests = new List<OrdinaryTourRequestDTO>();
            foreach (OrdinaryTourRequestDTO r in requestsDTO)
            {
                if(r.ComplexTourRequestId == 0 || r.ComplexTourRequestId == -1 || r.ComplexTourRequestId==null)
                {
                    requests.Add(r);
                }
            }
            _requestsDTO = new ObservableCollection<OrdinaryTourRequestDTO>(requests);
            List<ComplexTourRequestDTO> complexDTO = _complexRequestService.GetAll().Select(request => new ComplexTourRequestDTO(request)).ToList();
            _complexRequestsDTO = new ObservableCollection<ComplexTourRequestDTO>(complexDTO);
            _filteredRequestsDTO = _requestsDTO;

            _searchCommand = new RelayCommand(Search);
            _resetSearchParametersCommand = new RelayCommand(ResetSearchParameters);
            _cancelTourRequestCommand = new RelayCommand(CancelTourRequest);
            _acceptTourRequestCommand = new RelayCommand(AcceptTourRequest);
            _requestDetailsCommand = new RelayCommand(RequestDetails);
            _acceptComplexTourRequestCommand = new RelayCommand(AcceptComplexTourRequest);
            _showTourRequestStatisticsCommand = new RelayCommand(ShowTourRequestStatistic);
            _showTourStatisticsCommand = new RelayCommand(ShowTourStatistics);
            _showAllToursCommand = new RelayCommand(ShowAllTours);
            _logoutCommand = new RelayCommand(Logout);
           _addNewTourCommand = new RelayCommand(AddNewTour);
            _showMainWindowCommand = new RelayCommand(ShowMainWindow);
            keyValuePairs = new Dictionary<ComplexTourRequestDTO, ObservableCollection<OrdinaryTourRequestDTO>>();
            foreach (OrdinaryTourRequest requestOrdinary in _tourRequestService.GetAll())
            {
               
                    requestOrdinary.CanBeAccepted = true;
                    _tourRequestService.Update(requestOrdinary);
                
            }
            _dictionary = MakeDictionary();
            _filteredDictionary = _dictionary;
            _reportService = new RequestReportService();
            GenerateReportCommand = new RelayCommand(GenerateReport);



        }
        private void GenerateReport()
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "PDF Files|*.pdf",
                Title = "Save Tour Requests Report"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                _reportService.GenerateTourRequestReport(_tourRequestService.GetAll().Select(request => new OrdinaryTourRequestDTO(request)).ToList(), saveFileDialog.FileName);
                MessageBox.Show("Uspjesno generisan izvestaj!", "", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        public RelayCommand AddNewTourCommand
        {
            get { return _addNewTourCommand; }
            set
            {
                _addNewTourCommand = value;
                OnPropertyChanged();
            }
        }
        private void AddNewTour()
        {
            AddTourWindow addTour = new AddTourWindow(_userDTO);
            addTour.Show();
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
        public RelayCommand ShowMainWindowCommand
        {
            get { return _showMainWindowCommand; }
            set
            {
                _showMainWindowCommand = value;
                OnPropertyChanged();
            }
        }
        private void ShowMainWindow()
        {
            GuideMainWindow mainWindow = new GuideMainWindow(_userDTO.ToUser());
            mainWindow.Show();
            TourRequestWindow.GetInstance().Close();
        }
        public RelayCommand ShowAllToursCommand
        {
            get { return _showAllToursCommand; }
            set
            {
                _showAllToursCommand = value;
                OnPropertyChanged();
            }
        }
        private void ShowAllTours()
        {
            AllToursWindow allToursView = new AllToursWindow(_userDTO);

            allToursView.Show();
            TourRequestWindow.GetInstance().Close();
        }
        public Dictionary<ComplexTourRequestDTO, ObservableCollection<OrdinaryTourRequestDTO>> MakeDictionary()
        {

            List<OrdinaryTourRequestDTO> complexTourParts = new List<OrdinaryTourRequestDTO>();
            ObservableCollection<OrdinaryTourRequestDTO> complexPartsDTO = new ObservableCollection<OrdinaryTourRequestDTO>();

            foreach (ComplexTourRequest c in _complexRequestService.GetAll())
            {
                complexTourParts = _complexRequestService.getOrdinaryTourRequests(c.Id).Select(ordinaryTourRequests => new OrdinaryTourRequestDTO(ordinaryTourRequests)).ToList();
                foreach (OrdinaryTourRequestDTO request in complexTourParts)
                {
                    if (request.Status == TourRequestStatus.Accepted && request.GuideId== _userDTO.Id)
                    {
                        foreach (OrdinaryTourRequestDTO requestOrdinary in complexTourParts)
                        {
                            if (request.ComplexTourRequestId == requestOrdinary.ComplexTourRequestId )
                            {
                                requestOrdinary.CanBeAccepted = false;
                                _tourRequestService.Update(requestOrdinary.ToOrdinaryTourRequest());
                            }
                        }
                    }
                }
                complexPartsDTO = new ObservableCollection<OrdinaryTourRequestDTO>(complexTourParts);
                keyValuePairs.Add(new ComplexTourRequestDTO(c), complexPartsDTO);
            }
            return keyValuePairs;
        }
        public ObservableCollection<ComplexTourRequestDTO> ComplexRequestsDTO
        {
            get { return _complexRequestsDTO; }
            set
            {
                _complexRequestsDTO = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand ShowTourStatisticsCommand
        {
            get { return _showTourStatisticsCommand; }
            set
            {
                _showTourStatisticsCommand = value;
                OnPropertyChanged();
            }
        }
        private void ShowTourStatistics()
        {
            TourStatisticsWindow tourStatistics = new TourStatisticsWindow(_userDTO);
            tourStatistics.Show();
        }
        public Dictionary<ComplexTourRequestDTO, ObservableCollection<OrdinaryTourRequestDTO>> Dictionary
        {
            get
            {
                return _dictionary;
            }
            set
            {
                _dictionary = value;
                OnPropertyChanged();
            }
        }
        public Dictionary<ComplexTourRequestDTO, ObservableCollection<OrdinaryTourRequestDTO>> FilteredDictionary
        {
            get
            {
                return _filteredDictionary;
            }
            set
            {
                _filteredDictionary = value;
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
        public RelayCommand RequestDetailsCommand
        {
            get
            {
                return _requestDetailsCommand;
            }
            set
            {
                _requestDetailsCommand = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand AcceptComplexTourRequestCommand
        {
            get
            {
                return _acceptComplexTourRequestCommand;
            }
            set
            {
                _acceptComplexTourRequestCommand = value;
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
            TourRequestStatisticsWindow statistics = new TourRequestStatisticsWindow(_userDTO);
            statistics.Show();
        }
        private void AcceptComplexTourRequest(object parameter)
        {
            OrdinaryTourRequestDTO requestDTO = parameter as OrdinaryTourRequestDTO;
            ComplexTourRequestDTO _complexRequestDTO = new ComplexTourRequestDTO(_complexRequestService.GetById(requestDTO.ComplexTourRequestId));
            AcceptComplexTourWindow acceptWindow = new AcceptComplexTourWindow(requestDTO,_complexRequestDTO,_userDTO);
            acceptWindow.Show();
        }
        private void AcceptTourRequest(object parameter)
        {
            OrdinaryTourRequestDTO requestDTO = parameter as OrdinaryTourRequestDTO;
            AcceptTourWindow acceptWindow = new AcceptTourWindow(requestDTO, _userDTO);
            acceptWindow.Show();
        }
        private void RequestDetails(object parameter)
        {
            OrdinaryTourRequestDTO requestDTO = parameter as OrdinaryTourRequestDTO;
            RequestDetailsWindow detailsWindow = new RequestDetailsWindow(requestDTO, _userDTO);
            detailsWindow.Show();
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
        private DateTime _searchEndDateInput = DateTime.Now ;
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
            var filteredDictionary = _dictionary
            .Where(kv =>
                kv.Value.Any(request =>
                    (string.IsNullOrEmpty(SearchCityInput) || request.LocationDTO.City.Contains(SearchCityInput)) &&
                    (string.IsNullOrEmpty(SearchCountryInput) || request.LocationDTO.Country.Contains(SearchCountryInput)) &&
                    (string.IsNullOrEmpty(SearchLanguageInput) || request.Language.ToString().ToLower().Contains(SearchLanguageInput.ToLower())) &&
                    ((SearchBeginDateInput == default || SearchEndDateInput == default) || (request.BeginDate >= SearchBeginDateInput && request.BeginDate < SearchEndDateInput && request.EndDate <= SearchEndDateInput && request.EndDate > SearchBeginDateInput)) &&
                    (string.IsNullOrEmpty(SearchTouristNumberInput) || request.NumberOfTourists.ToString().Contains(SearchTouristNumberInput))
                )
            )
            .ToDictionary(kv => kv.Key, kv => kv.Value);

            FilteredDictionary = filteredDictionary;
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
        public RelayCommand LogoutCommand
        {
            get { return _logoutCommand; }
            set
            {
                _logoutCommand = value;
                OnPropertyChanged();
            }
        }
        private void Logout()
        {
            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            TourRequestWindow.GetInstance().Close();
        }


    }
}
    