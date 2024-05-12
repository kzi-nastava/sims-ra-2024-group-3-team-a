using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.InjectorNameSpace;
using BookingApp.Model.Enums;
using BookingApp.Repository.Interfaces;
using BookingApp.Service;
using BookingApp.View.Tourist;
using LiveCharts;
using LiveCharts.Wpf;
using System;
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

        private List<OrdinaryTourRequestDTO> _ordinaryTourRequestsDTO { get; set; }

        private List<SolidColorBrush> SolidColorBrushes { get; set; }

        private OrdinaryTourRequestDTO _selectedTourDTO = null;

        private RelayCommand _showOrdinaryTourRequestInfoWindowCommand;
        private RelayCommand _updateCommand;
        private RelayCommand _averageTouristNumberCommand;
        public SeriesCollection SeriesCollection { get; set; }
        public SeriesCollection HistogramData { get; set; }
        public SeriesCollection HistogramDataForLocation { get; set; }
        public string[] Labels { get; set; }
        public List<SolidColorBrush> brushes { get; set; }




        public TourRequestsViewModel(UserDTO loggedInUser)
        {
            _userDTO = loggedInUser;
            _ordinaryTourRequestDTO = new OrdinaryTourRequestDTO();
            IOrdinaryTourRequestRepository ordinaryTourRequestRepository = Injector.CreateInstance<IOrdinaryTourRequestRepository>();
            _ordinaryTourRequestService = new OrdinaryTourRequestService(ordinaryTourRequestRepository);
            List<OrdinaryTourRequestDTO> ordinaryTourRequests = _ordinaryTourRequestService.GetAllForUser(_userDTO.Id).Select(ordinaryTourRequests => new OrdinaryTourRequestDTO(ordinaryTourRequests)).ToList();
            _ordinaryTourRequestsDTO = new List<OrdinaryTourRequestDTO>(ordinaryTourRequests);
            _showOrdinaryTourRequestInfoWindowCommand = new RelayCommand(ShowOrdinaryTourRequestInfoWindow);
            _updateCommand = new RelayCommand(Update);
            _averageTouristNumberCommand = new RelayCommand(GetAverageTouristNumber);
            LoadDataForPieChart();

            // Convert hexadecimal color codes to Color objects
            Color color1 = (Color)ColorConverter.ConvertFromString("#ffe2f1"); // LightYellow
            Color color2 = (Color)ColorConverter.ConvertFromString("#ffd3ea"); // LightSteelBlue
            Color color3 = (Color)ColorConverter.ConvertFromString("#ffb9de"); // LightCoral
            Color color4 = (Color)ColorConverter.ConvertFromString("#ffaad7"); // LightCoral
            Color color5 = (Color)ColorConverter.ConvertFromString("#ffffd8"); // LightCoral

            // Create SolidColorBrush instances with the colors
           // SolidColorBrush brush1 = new SolidColorBrush(color1);
           // SolidColorBrush brush2 = new SolidColorBrush(color2);
            SolidColorBrush brush3 = new SolidColorBrush(color1);
            SolidColorBrush brush4 = new SolidColorBrush(color4);
            SolidColorBrush brush5 = new SolidColorBrush(color5);

            // Add SolidColorBrush instances to the list
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


        }



        public List<OrdinaryTourRequestDTO> OrdinaryTourRequestsDTO
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
                    DataLabels = true
                    //Fill = brushes[4]
                },
                new PieSeries
                {
                    Title = "Rejected",
                    Values = new ChartValues<double> { declinedPercentage },
                    DataLabels = true
                    //Fill = brushes[3]
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

    }
}
