using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.InjectorNameSpace;
using BookingApp.Model;
using BookingApp.Model.Enums;
using BookingApp.Repository;
using BookingApp.Repository.Interfaces;
using BookingApp.Service;
using BookingApp.View.Tourist;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BookingApp.ViewModel.Tourist
{
    public class TourRequestsViewModel : ViewModel
    {
        private OrdinaryTourRequestService _ordinaryTourRequestService { get; set; }
        private UserDTO _userDTO { get; set; }
        private OrdinaryTourRequestDTO _ordinaryTourRequestDTO { get; set; }

        private List<OrdinaryTourRequestDTO> _complexTourPartsDTO { get; set; }

        private ObservableCollection<OrdinaryTourRequestDTO> _ordinaryTourRequestsDTO { get; set; }

        private ObservableCollection<ComplexTourRequestDTO> _complexTourRequestsDTO { get; set; }

        private List<SolidColorBrush> SolidColorBrushes { get; set; }

        private OrdinaryTourRequestDTO _selectedTourDTO = null;
        private ComplexTourRequestService _complexTourRequestService { get; set; }

        private RelayCommand _showOrdinaryTourRequestInfoWindowCommand;
        private RelayCommand _updateCommand;
        private RelayCommand _averageTouristNumberCommand;
        private RelayCommand _showForAllYearsCommand;
        public SeriesCollection SeriesCollection { get; set; }
        public SeriesCollection HistogramData { get; set; }
        public SeriesCollection HistogramDataForLocation { get; set; }
        public string[] Labels { get; set; }
        public List<SolidColorBrush> brushes { get; set; }

        public Dictionary<ComplexTourRequestDTO, ObservableCollection<OrdinaryTourRequestDTO>> keyValuePairs { get; set; }
        private Dictionary<ComplexTourRequestDTO, ObservableCollection<OrdinaryTourRequestDTO>> _dictionary;

        public TourRequestsViewModel(UserDTO loggedInUser)
        {
            _userDTO = loggedInUser;
            _ordinaryTourRequestDTO = new OrdinaryTourRequestDTO();
            IComplexTourRequestRepository complexTourRequestRepository = Injector.CreateInstance<IComplexTourRequestRepository>();
            IOrdinaryTourRequestRepository ordinaryTourRequestRepository = Injector.CreateInstance<IOrdinaryTourRequestRepository>();
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
            _ordinaryTourRequestService = new OrdinaryTourRequestService(accommodationReservationChangeRequestRepository, accommodationReservationRepository, accommodationRepository, ordinaryTourRequestRepository, tourRepository, messageRepository, touristRepository, userRepository, tourReservationRepository, tourReviewRepository, voucherRepository);
            _complexTourRequestService = new ComplexTourRequestService(accommodationReservationChangeRequestRepository, accommodationReservationRepository, accommodationRepository, ordinaryTourRequestRepository, tourRepository, messageRepository, touristRepository, userRepository, tourReservationRepository, tourReviewRepository, voucherRepository, complexTourRequestRepository);
            List<OrdinaryTourRequestDTO> ordinaryTourRequests = _ordinaryTourRequestService.GetAllForUser(_userDTO.Id).Select(ordinaryTourRequests => new OrdinaryTourRequestDTO(ordinaryTourRequests)).ToList();
            List<ComplexTourRequestDTO> complexTourRequests = _complexTourRequestService.GetAll().Select(complexTourRequests => new ComplexTourRequestDTO(complexTourRequests)).ToList();
            _ordinaryTourRequestsDTO = new ObservableCollection<OrdinaryTourRequestDTO>(ordinaryTourRequests);
            _complexTourRequestsDTO= new ObservableCollection<ComplexTourRequestDTO>(complexTourRequests);
            _complexTourPartsDTO = new List<OrdinaryTourRequestDTO>();
            _showOrdinaryTourRequestInfoWindowCommand = new RelayCommand(ShowOrdinaryTourRequestInfoWindow);
            _updateCommand = new RelayCommand(Update);
            _averageTouristNumberCommand = new RelayCommand(GetAverageTouristNumber);
            _showForAllYearsCommand = new RelayCommand(LoadDataForPieChart);
            LoadDataForPieChart();
           
            _isLocationGridVisible = 3;
            keyValuePairs = new Dictionary<ComplexTourRequestDTO, ObservableCollection<OrdinaryTourRequestDTO>>();
            Color color1 = (Color)ColorConverter.ConvertFromString("#ffe2f1"); 
            Color color2 = (Color)ColorConverter.ConvertFromString("#ffd3ea"); 
            Color color3 = (Color)ColorConverter.ConvertFromString("#ffb9de"); 
            Color color4 = (Color)ColorConverter.ConvertFromString("#ffaad7"); 
            Color color5 = (Color)ColorConverter.ConvertFromString("#ffffd8"); 

           
            SolidColorBrush brush3 = new SolidColorBrush(color1);
            SolidColorBrush brush4 = new SolidColorBrush(color4);
            SolidColorBrush brush5 = new SolidColorBrush(color5);

            brushes = new List<SolidColorBrush>();
            brushes.Add(brush3);
            brushes.Add(brush4);
            brushes.Add(brush5);
            HistogramData = new SeriesCollection();
            int brushIndex = 0;
            foreach (var language in _ordinaryTourRequestService.GetLanguages(_userDTO.Id))
            {
                ColumnSeries columnSeries = new ColumnSeries
                {
                    Title = $"Number of {language}",
                    Values = new ChartValues<int> { _ordinaryTourRequestService.CountOrdinaryTourRequestsbyLanguage(_userDTO.Id, language.ToString()) },
                    Fill = brushes[brushIndex % brushes.Count],
                };
                HistogramData.Add(columnSeries);
                brushIndex++;
            }
            Labels = new[] { "Languages" };
            HistogramDataForLocation = new SeriesCollection();
            foreach (var location in _ordinaryTourRequestService.GetLocations(_userDTO.Id))
            {
                ColumnSeries columnSeries = new ColumnSeries
                {
                    Title = $"Number of {location.City} {location.Country}",
                    Values = new ChartValues<int> { _ordinaryTourRequestService.CountOrdinaryTourRequestsByLocation(_userDTO.Id, location) },
                    Fill = brushes[brushIndex % brushes.Count],
                };
                HistogramDataForLocation.Add(columnSeries);
                brushIndex++;
            }
            _dictionary = MakeDictionary();
            ComplexTourRequestStatus();
        }


        public ObservableCollection<OrdinaryTourRequestDTO> OrdinaryTourRequestsDTO
        {
            get
            {
                return _ordinaryTourRequestsDTO;
            }
            set
            {
                _ordinaryTourRequestsDTO = value;
                OnPropertyChanged();
            }

        }
      
        public ObservableCollection<ComplexTourRequestDTO> ComplexTourRequestsDTO
        {
            get
            {
                return _complexTourRequestsDTO;
            }
            set
            {
                _complexTourRequestsDTO = value;
              //  _complexTourPartsDTO = _complexTourRequestService.getOrdinaryTourRequestsForUser()
                OnPropertyChanged();
            }

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
        /*public ObservableCollection<OrdinaryTourRequestDTO> ComplexTourPartsDTO
        {
            get
            {
                
            }
            set
            {
                _ordinaryTourRequestsDTO = value;
                OnPropertyChanged();
            }

        }*/


        public OrdinaryTourRequestDTO SelectedTourDTO
        {
            get
            {
                return _selectedTourDTO;
            }
            set
            {
                _selectedTourDTO = value;
                OnPropertyChanged();

            }
        }

        public RelayCommand ShowOrdinaryTourRequestInfoWindowCommand
        {
            get
            {
                return _showOrdinaryTourRequestInfoWindowCommand;
            }
            set
            {
                _showOrdinaryTourRequestInfoWindowCommand = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand UpdateCommand
        {
            get
            {
                return _updateCommand;
            }
            set
            {
                _updateCommand = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand AverageTourisNumberCommand
        {
            get
            {
                return _averageTouristNumberCommand;
            }
            set
            {
                _averageTouristNumberCommand = value;
                OnPropertyChanged();
            }
        }

        private SeriesCollection _chartValues;
        public SeriesCollection ChartValues
        {
            get { return _chartValues; }
            set
            {
                _chartValues = value;
                OnPropertyChanged();
            }
        }
      
        public RelayCommand ShowForAllYearsCommand
        {
            get { return _showForAllYearsCommand; }
            set
            {
                _showForAllYearsCommand = value;
                OnPropertyChanged();
            }
        }

        private int _selectedYear;
        public int SelectedYear
        {
            get
            {
                return _selectedYear;
            }
            set
            {
                _selectedYear = value;
                Update();
                OnPropertyChanged();
            }
        }
        private int _selectedYearForAverageNumber;
        public int SelectedYearForAverageNumber
        {
            get
            {
                return _selectedYearForAverageNumber;
            }
            set
            {
                _selectedYearForAverageNumber = value;
                GetAverageTouristNumber();
                OnPropertyChanged();
            }
        }

        private int _selectedParameter;
        public int SelectedParameter
        {
            get
            {
                return _selectedParameter;
            }
            set
            {
                _selectedParameter = value;
                OnPropertyChanged();
            }
        }
        private int _selectedParameterForChar;
        public int SelectedParameterForChar
        {
            get
            {
                return _selectedParameterForChar;
            }
            set
            {
                _selectedParameterForChar = value;
                OnPropertyChanged();
            }
        }
        private int _isLanguageGridVisible;
        public int IsLanguageGridVisible
        {
            get { return _isLanguageGridVisible; }
            set
            {
                _isLanguageGridVisible = value;
                OnPropertyChanged();
            }
        }
        private int _isLocationGridVisible;
        public int IsLocationGridVisible
        {
            get { return _isLocationGridVisible; }
            set
            {
                _isLocationGridVisible = value;
                OnPropertyChanged();
            }
        }
        private double _averageTouristNumber;
        public double AverageTouristNumber
        {
            get
            {
                return _averageTouristNumber;
            }
            set
            {
                _averageTouristNumber = value;
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

        public IEnumerable<int> Years
        {
            get
            {
               return Enumerable.Range(1950, 2024).ToList();
            }
            set { }
        }
        public void ShowOrdinaryTourRequestInfoWindow()
        {

            if (_selectedTourDTO == null)
            {
                return;
            }

            var selectedItem = _selectedTourDTO as OrdinaryTourRequestDTO;

            
                OrdinaryTourRequestInfoWindow ordinaryTourRequestInfoWindow = new OrdinaryTourRequestInfoWindow(new OrdinaryTourRequestDTO(selectedItem));
                ordinaryTourRequestInfoWindow.ShowDialog();
            

        }

        public void LoadDataForPieChart()
        {
            Color color1 = (Color)ColorConverter.ConvertFromString("#ffe2f1");
            Color color2 = (Color)ColorConverter.ConvertFromString("#ffd3ea");
            Color color3 = (Color)ColorConverter.ConvertFromString("#ffb9de");
            Color color4 = (Color)ColorConverter.ConvertFromString("#ffaad7");
            Color color5 = (Color)ColorConverter.ConvertFromString("#ffffd8");


            SolidColorBrush brush3 = new SolidColorBrush(color1);
            SolidColorBrush brush4 = new SolidColorBrush(color4);
            SolidColorBrush brush5 = new SolidColorBrush(color5);

            brushes = new List<SolidColorBrush>();
            brushes.Add(brush3);
            brushes.Add(brush4);
            brushes.Add(brush5);

            int acceptedCount = _ordinaryTourRequestService.CountAcceptedOrdinaryTourRequests(_userDTO.Id);
            int rejectedCount = _ordinaryTourRequestService.CountRejectedOrdinaryTourRequests(_userDTO.Id);

            double totalRequests = acceptedCount + rejectedCount;

            
            double acceptedPercentage = (acceptedCount / totalRequests) * 100;
            double declinedPercentage = (rejectedCount / totalRequests) * 100;



            ChartValues = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "Accepted",
                    Values = new ChartValues<double> { acceptedPercentage },
                    DataLabels = true,
                    Fill = brushes[1]
                },
                new PieSeries
                {
                    Title = "Rejected",
                    Values = new ChartValues<double> { declinedPercentage },
                    DataLabels = true,
                    Fill = brushes[2]
        }
            };
        }

        public void Update()
        {
            var selectedItem = _selectedYear;
            int acceptedCount = _ordinaryTourRequestService.CountAcceptedOrdinaryTourRequestsForSpecificYear(_userDTO.Id, selectedItem);
            int rejectedCount = _ordinaryTourRequestService.CountRejectedOrdinaryTourRequestsForSpecificYear(_userDTO.Id, selectedItem);

            double totalRequests = acceptedCount + rejectedCount;


            double acceptedPercentage = (acceptedCount / totalRequests) * 100;
            double declinedPercentage = (rejectedCount / totalRequests) * 100;



            ChartValues = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "Accepted",
                    Values = new ChartValues<double> { acceptedPercentage },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "Rejected",
                    Values = new ChartValues<double> { declinedPercentage },
                    DataLabels = true
                }
            };
        }
        

        public void GetAverageTouristNumber()
        {
            var selectedItem = _selectedYearForAverageNumber;
            if(SelectedParameter==1)
            {
                AverageTouristNumber= _ordinaryTourRequestService.CalculateAverageTouristNumber(_userDTO.Id, 0);
            }
            else 
            {

                AverageTouristNumber= _ordinaryTourRequestService.CalculateAverageTouristNumber(_userDTO.Id, selectedItem);
            }
        }

        public Dictionary<ComplexTourRequestDTO, ObservableCollection<OrdinaryTourRequestDTO>> MakeDictionary()
        {


            List<OrdinaryTourRequestDTO> complexTourParts = new List<OrdinaryTourRequestDTO>();
            ObservableCollection<OrdinaryTourRequestDTO> complexPartsDTO = new ObservableCollection<OrdinaryTourRequestDTO>();

            foreach (ComplexTourRequest c in _complexTourRequestService.GetAllForUser(_userDTO.Id))
            {
                 complexTourParts = _complexTourRequestService.getOrdinaryTourRequestsForUser(c.Id, _userDTO.Id).Select(ordinaryTourRequests => new OrdinaryTourRequestDTO(ordinaryTourRequests)).ToList();
                complexPartsDTO = new ObservableCollection<OrdinaryTourRequestDTO>(complexTourParts);
                keyValuePairs.Add( new ComplexTourRequestDTO(c), complexPartsDTO);      
            
            
            }

            return keyValuePairs;

           
        }

        public void Test()
        {
            ObservableCollection<OrdinaryTourRequestDTO> complexTourParts = new ObservableCollection<OrdinaryTourRequestDTO>();
            ObservableCollection<ComplexTourRequestDTO> complex = new ObservableCollection<ComplexTourRequestDTO>();
            // complexTourParts = _dictionary.ContainsKey()
         
        }
        public void ComplexTourRequestStatus()
        {
            foreach (var pair in Dictionary)
            {
                var key = pair.Key;
                var values = pair.Value;

               
                bool allAccepted = values.All(v => v.Status == TourRequestStatus.Accepted);

                if (allAccepted)
                {
                    key.Status = TourRequestStatus.Accepted;
                    _complexTourRequestService.Update(key.ToComplexTourRequest());
                }
            }
        }
    }
}
