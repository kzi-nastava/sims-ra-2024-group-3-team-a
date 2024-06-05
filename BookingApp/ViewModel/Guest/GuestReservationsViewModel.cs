using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.InjectorNameSpace;
using BookingApp.Model;
using BookingApp.Reports.Guest;
using BookingApp.Reports.Owner;
using BookingApp.Repository;
using BookingApp.Repository.Interfaces;
using BookingApp.Service;
using BookingApp.View.Guest;
using LiveCharts;
using LiveCharts.Wpf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BookingApp.ViewModel.Guest
{
    public class GuestReservationsViewModel: ViewModel
    {
        public static ObservableCollection<AccommodationReservationDTO> _myReservations { get; set; }
        public static ObservableCollection<AccommodationReservationDTO> _myRatedReservations { get; set; }
        public static ObservableCollection<AccommodationReservationChangeRequestDTO> _myChangeRequests { get; set; }

        private AccommodationReservationService _accommodationReservationService;
        private AccommodationService _accommodationService;
        private AccommodationReservationChangeRequestService _accommodationReservationChangeRequestService;
        private UserDTO _loggedInGuest;

        private DateTime _selectedNewBeginDate;
        private DateTime _selectedNewEndDate;
        private AccommodationReservationDTO _selectedReservation;
        private AccommodationReservationChangeRequestDTO _selectedChangeRequest;

        private List<string> _images;

        private bool _isFrameRateVisible;
        private bool _isFrameMyRequestsVisible;

        private RelayCommand _addImagesCommand;
        private RelayCommand _submitRateOwnerCommand;
        private RelayCommand _showSideMenuCommand;
        private RelayCommand _rateOwnerCommand;
        private RelayCommand _requestChangeCommand;
        private RelayCommand _submitRequestCommand;
        private RelayCommand _cancelReservationCommand;
        private RelayCommand _renovationRatingCommand;
        private RelayCommand _generateReportCommand;
        private RelayCommand _showChartPageCommand;
        
        private int _selectedRating;

        public SeriesCollection StatusSeriesCollection { get; set; }
        public SeriesCollection AccommodationSeriesCollection { get; set; }
        private Func<double, string> _yAxisLabelFormatter;

        public GuestReservationsViewModel(UserDTO loggedInGuest) 
        {
            _loggedInGuest = loggedInGuest;

            IAccommodationReservationRepository accommodationReservationRepository = Injector.CreateInstance<IAccommodationReservationRepository>();
            IAccommodationReservationChangeRequestRepository accommodationReservationChangeRequestRepository = Injector.CreateInstance<IAccommodationReservationChangeRequestRepository>();
            IAccommodationRepository accommodationRepository = Injector.CreateInstance<IAccommodationRepository>();
            IUserRepository userRepository = Injector.CreateInstance<IUserRepository>();
            _accommodationReservationService = new AccommodationReservationService(accommodationReservationRepository, accommodationRepository, userRepository);
            _accommodationReservationChangeRequestService = new AccommodationReservationChangeRequestService(accommodationReservationChangeRequestRepository, accommodationReservationRepository, accommodationRepository, userRepository);
            _accommodationService = new AccommodationService(accommodationRepository);

            _images = new List<string>();

            _myChangeRequests = new ObservableCollection<AccommodationReservationChangeRequestDTO>();

            _yAxisLabelFormatter = value => value.ToString("N0");

            _addImagesCommand = new RelayCommand(AddImages);
            _rateOwnerCommand = new RelayCommand(RateOwner);
            _submitRateOwnerCommand = new RelayCommand(SubmitRateOwner);
            _requestChangeCommand = new RelayCommand(RequestChange);
            _submitRequestCommand = new RelayCommand(SubmitRequest);
            _cancelReservationCommand = new RelayCommand(CancelReservation);
            _renovationRatingCommand = new RelayCommand(ChangeRating);
            _showSideMenuCommand = new RelayCommand(ShowSideMenu);
            _generateReportCommand = new RelayCommand(GenerateReport);
            _showChartPageCommand = new RelayCommand(ShowChartPage);
            InitializeCharts();
            UpdateMyReservations();
            
        }

        public void UpdateMyReservations()
        {
            List<AccommodationReservationDTO> AccommodationReservationsList = _accommodationReservationService.GetAllByGuestId(_loggedInGuest.Id).Select(accommodationReservation => new AccommodationReservationDTO(accommodationReservation)).ToList();
            List<AccommodationReservationDTO> RatedAccommodationReservationsList = _accommodationReservationService.GetAllRatedByGuestId(_loggedInGuest.Id).Select(accommodationReservation => new AccommodationReservationDTO(accommodationReservation)).ToList();
            _myReservations = new ObservableCollection<AccommodationReservationDTO>(AccommodationReservationsList);
            _myRatedReservations = new ObservableCollection<AccommodationReservationDTO>(RatedAccommodationReservationsList);
            List<AccommodationReservationChangeRequestDTO> MyRequestsList = _accommodationReservationChangeRequestService.GetAllByGuestId(_loggedInGuest.Id).Select(request => new AccommodationReservationChangeRequestDTO(request)).ToList();
            _myChangeRequests = new ObservableCollection<AccommodationReservationChangeRequestDTO>(MyRequestsList);
            MyChangeRequestsDTO = _myChangeRequests;
            UpdateCharts();
        }
        private void InitializeCharts()
        {
            StatusSeriesCollection = new SeriesCollection();
            AccommodationSeriesCollection = new SeriesCollection();
        }

        private void UpdateCharts()
        {
            UpdateStatusChart();
            UpdateAccommodationChart();
        }

        private void UpdateStatusChart()
        {
            var statusGroups = _myReservations.GroupBy(r => r.Canceled)
                                              .Select(g => new { Canceled = g.Key, Count = g.Count() })
                                              .ToList();

            StatusSeriesCollection.Clear();
            foreach (var statusGroup in statusGroups)
            {
                StatusSeriesCollection.Add(new PieSeries
                {
                    Title = statusGroup.Canceled.ToString(),
                    Values = new ChartValues<int> { statusGroup.Count }
                });
            }
        }

        private void UpdateAccommodationChart()
        {
            var accommodationGroups = _myReservations.GroupBy(r => r.AccommodationId)
                                                     .Select(g => new { AccommodationId = g.Key, Count = g.Count() })
                                                     .ToList();

            AccommodationSeriesCollection.Clear();
            foreach (var accommodationGroup in accommodationGroups)
            {
                AccommodationSeriesCollection.Add(new ColumnSeries
                {
                    Title = accommodationGroup.AccommodationId.ToString(),
                    Values = new ChartValues<int> { accommodationGroup.Count }
                });
            }
        }
        private void CancelReservation()
        {
            if (_selectedReservation == null)
            {
                MessageBox.Show("Please select reservation to cancel!");
            }
            else
            {
                Accommodation selectedAccommodation = _accommodationService.GetById(_selectedReservation.AccommodationId);
                if (_selectedReservation.Canceled == true)
                {
                    MessageBox.Show("Reservation already canceled!");
                }
                else if (_selectedReservation.BeginDate >= DateOnly.FromDateTime(DateTime.Today).AddDays(selectedAccommodation.CancellationPeriod))
                { 
                    _selectedReservation.Canceled = true;
                    _accommodationReservationService.Update(_selectedReservation.ToAccommodationReservation());
                    MessageBox.Show("Cancellation successful!");
                }
                else
                {
                    MessageBox.Show($"Too late to cancel. Cancelation period is {selectedAccommodation.CancellationPeriod} days!");
                }
            }
        }

        private void RequestChange()
        {
            if (_selectedReservation == null)
            {
                MessageBox.Show("Please select reservation to request date change!");
            }
            else
            {
                if (_selectedReservation.BeginDate <= DateOnly.FromDateTime(DateTime.Now))
                {
                    MessageBox.Show("This reservation is already over!");
                    return;
                }
                else
                {
                    IsFrameMyRequestsVisible = true;
                    IsFrameRateVisible = false;
                }
            }
        }

        private void RateOwner()
        {
            if (_selectedReservation == null)
            {
                MessageBox.Show("Please select reservation to rate!");
            }
            else
            {
                if (_selectedReservation.EndDate.AddDays(5) < DateOnly.FromDateTime(DateTime.Now))
                {
                    MessageBox.Show("You can't rate this - it was over more than 5 days ago!");
                    return;
                }
                else if (_selectedReservation.EndDate > DateOnly.FromDateTime(DateTime.Now))
                {
                    MessageBox.Show("You can't rate this - reservation not over yet!");
                    return;
                }
                else
                {
                    IsFrameRateVisible = true;
                    IsFrameMyRequestsVisible = false;
                }

            }
        }

        private void SubmitRequest()
        {
            DateOnly selectedBeginDate = DateOnly.FromDateTime(_selectedNewBeginDate);
            DateOnly selectedEndDate = DateOnly.FromDateTime(_selectedNewEndDate);

            AccommodationReservationChangeRequest changeRequest = new AccommodationReservationChangeRequest(0, _selectedReservation.Id, selectedBeginDate, selectedEndDate, Model.Enums.AccommodationChangeRequestStatus.WaitingForApproval, "");
            _accommodationReservationChangeRequestService.Save(changeRequest);
            _myChangeRequests.Add(new AccommodationReservationChangeRequestDTO(changeRequest));
            MessageBox.Show("Submitted request");
        }

        private void SubmitRateOwner()
        {
            _selectedReservation.RatingDTO.GuestImages = _images;
            _selectedReservation.RatingDTO.GuestRenovationRating = SelectedRating;
            _accommodationReservationService.Update(_selectedReservation.ToAccommodationReservation());
            MessageBox.Show("Rated Owner successfully!");

        }
        
        private void AddImages()
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;

            bool? response = openFileDialog.ShowDialog();

            if (response == true)
            {
                _images = openFileDialog.FileNames.ToList();

                for (int i = 0; i < _images.Count; i++)
                {
                    _images[i] = System.IO.Path.GetRelativePath(AppDomain.CurrentDomain.BaseDirectory, _images[i]).ToString();
                }
            }
        }
        private void ShowChartPage()
        {
            GuestMainViewWindow.MainFrame.Content = new ChartPage();
        }
        private void GenerateReport()
        {
            //AccommodationStatisticsReport accommodationStatisticsReport = new AccommodationStatisticsReport();
            //accommodationStatisticsReport.GenerateReport(_accommodationStatisticsDTO, _accommodationDTO);
            MyReservationsReport myReservationsReport = new MyReservationsReport();
            myReservationsReport.GenerateReport(_loggedInGuest, _myReservations);
        }

        private void ShowReservationDetails() 
        {
            if (_selectedReservation.BeginDate >= DateOnly.FromDateTime(DateTime.Now))
            {
                GuestMainViewWindow.MainFrame.Content = new UpcomingReservationPage(_selectedReservation);
            }
            else
            {
                GuestMainViewWindow.MainFrame.Content = new PastReservationPage(_selectedReservation);
            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb == null) return;

            switch (rb.Content.ToString())
            {
                case "1":
                    GuestReservationsPage.Instance.lblRecommendation.Content = "All is ok, some minor improvements needed only.";
                    break;
                case "2":
                    GuestReservationsPage.Instance.lblRecommendation.Content = "Minor wear and tear observed.";
                    break;
                case "3":
                    GuestReservationsPage.Instance.lblRecommendation.Content = "Moderate renovation may be required.";
                    break;
                case "4":
                    GuestReservationsPage.Instance.lblRecommendation.Content = "Major improvements needed soon.";
                    break;
                case "5":
                    GuestReservationsPage.Instance.lblRecommendation.Content = "Accommodation needs to be renovated ASAP.";
                    break;
                default:
                    GuestReservationsPage.Instance.lblRecommendation.Content = "Unknown rating.";
                    break;
            }
        }
        public Func<double, string> YAxisLabelFormatter
        {
            get => _yAxisLabelFormatter;
            set
            {
                _yAxisLabelFormatter = value;
                OnPropertyChanged(nameof(YAxisLabelFormatter));
            }
        }
        public ObservableCollection<AccommodationReservationDTO> MyReservationsDTO
        {
            get
            {
                return _myReservations;
            }
            set
            {
                _myReservations = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<AccommodationReservationChangeRequestDTO> MyChangeRequestsDTO
        {
            get
            {
                return _myChangeRequests;
            }
            set
            {
                _myChangeRequests = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<AccommodationReservationDTO> MyRatedReservationsDTO
        {
            get
            {
                return _myRatedReservations;
            }
            set
            {
                _myRatedReservations = value;
                OnPropertyChanged();
            }
        }
        public UserDTO LoggedInGuest
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
        public bool IsFrameRateVisible
        {
            get
            {
                return _isFrameRateVisible;
            }
            set
            {
                _isFrameRateVisible = value;
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

        public AccommodationReservationDTO SelectedReservation
        {
            get
            {
                return _selectedReservation;
            }
            set
            {
                _selectedReservation = value;
                OnPropertyChanged();
                ShowReservationDetails();
            }

        }
        public AccommodationReservationChangeRequestDTO SelectedChangeRequest
        {
            get
            {
                return _selectedChangeRequest;
            }
            set
            {
                _selectedChangeRequest = value;
                OnPropertyChanged();
            }

        }

        public DateTime SelectedNewBeginDate
        {
            get
            {
                return _selectedNewBeginDate;
            }
            set
            {
                _selectedNewBeginDate = value;
                OnPropertyChanged();
            }
        }

        public DateTime SelectedNewEndDate
        {
            get
            {
                return _selectedNewEndDate;
            }
            set
            {
                _selectedNewEndDate = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand ShowChartPageCommand
        {
            get
            {
                return _showChartPageCommand;
            }
            set
            {
                _showChartPageCommand = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand CancelReservationCommand
        {
            get
            {
                return _cancelReservationCommand;
            }
            set
            {
                _cancelReservationCommand = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand RateOwnerCommand
        {
            get
            {
                return _rateOwnerCommand;
            }
            set
            {
                _rateOwnerCommand = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand SubmitRateOwnerCommand
        {
            get
            {
                return _submitRateOwnerCommand;
            }
            set
            {
                _submitRateOwnerCommand = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand SubmitRequestCommand
        {
            get
            {
                return _submitRequestCommand;
            }
            set
            {
                _submitRequestCommand = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand AddImagesCommand
        {
            get
            {
                return _addImagesCommand;
            }
            set
            {
                _addImagesCommand = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand RequestChangeCommand
        {
            get
            {
                return _requestChangeCommand;
            }
            set
            {
                _requestChangeCommand = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand RenovationRatingCommand
        {
            get
            {
                return _renovationRatingCommand;
            }
            set
            {
                _renovationRatingCommand = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand GenerateReportCommand
        {
            get
            {
                return _generateReportCommand;
            }
            set
            {
                _generateReportCommand = value;
                OnPropertyChanged();
            }
        }
        public int SelectedRating
        {
            get => _selectedRating;
            set
            {
                if (_selectedRating != value)
                {
                    _selectedRating = value;
                    OnPropertyChanged(nameof(SelectedRating));
                    UpdateRecommendation();
                }
            }
        }

        private string _recommendationText;
        public string RecommendationText
        {
            get => _recommendationText;
            set
            {
                if (_recommendationText != value)
                {
                    _recommendationText = value;
                    OnPropertyChanged(nameof(RecommendationText));
                }
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
        public void ShowSideMenu()
        {
            GuestMainViewWindow.SideMenuFrame.Content = new GuestSideMenuPage();
        }
        public void CloseSideMenu()
        {
            GuestMainViewWindow.SideMenuFrame.Content = null;
        }

        private void ChangeRating(object parameter)
        {
            if (parameter is string ratingString && int.TryParse(ratingString, out int rating))
            {
                SelectedRating = rating;
            }
        }

        private void UpdateRecommendation()
        {
            // Update your recommendation logic here based on `SelectedRating`
            switch (SelectedRating)
            {
                case 1:
                    RecommendationText = "All is ok, some minor improvements needed only.";
                    GuestReservationsPage.Instance.lblRecommendation.GetBindingExpression(Label.ContentProperty).UpdateTarget();
                    //GuestReservationsPage.Instance.lblRecommendation.Content = "All is ok, some minor improvements needed only.";
                    break;
                case 2:
                    RecommendationText = "Minor wear and tear observed.";
                    GuestReservationsPage.Instance.lblRecommendation.GetBindingExpression(Label.ContentProperty).UpdateTarget();
                    //GuestReservationsPage.Instance.lblRecommendation.Content = "All is ok, some minor improvements needed only.";
                    break;
                case 3:
                    RecommendationText = "Moderate renovation may be required.";
                    GuestReservationsPage.Instance.lblRecommendation.GetBindingExpression(Label.ContentProperty).UpdateTarget();
                    break;
                case 4:
                    RecommendationText = "Major improvements needed soon.";
                    GuestReservationsPage.Instance.lblRecommendation.GetBindingExpression(Label.ContentProperty).UpdateTarget();
                    break;
                case 5:
                    RecommendationText = "Accommodation needs to be renovated ASAP.";
                    GuestReservationsPage.Instance.lblRecommendation.GetBindingExpression(Label.ContentProperty).UpdateTarget();
                    break;
                default:
                    RecommendationText = "Unknown rating.";
                    GuestReservationsPage.Instance.lblRecommendation.GetBindingExpression(Label.ContentProperty).UpdateTarget();
                    break;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

