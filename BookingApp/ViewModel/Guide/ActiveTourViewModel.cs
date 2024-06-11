using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.InjectorNameSpace;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Repository.Interfaces;
using BookingApp.Service;
using BookingApp.View;
using BookingApp.View.Guide;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BookingApp.ViewModel.Guide
{
    public class ActiveTourViewModel : ViewModel
    {
        
        
        private KeyPointService _keyPointsService;
        private TourService _tourService;
        private TourReservationService _tourReservationService;
        private UserDTO _userDTO;
        private RelayCommand _markPointCommand;
        private RelayCommand _showTourReviewsWindowCommand;
        private RelayCommand _touristJoiningPointCommand;
        private RelayCommand _cancelTourCommand;
        private RelayCommand _showMainWindowCommand;
        private RelayCommand _showTourStatisticsCommand;
        private RelayCommand _logoutCommand;
        private RelayCommand _showTourDetailsCommand;
        private RelayCommand _showAllToursCommand;
        private RelayCommand _showTourRequestCommand;
        private RelayCommand _addNewTourCommand;
        private TourDTO _tourDTO;
        private UserDTO _loggedGuide;
        private int _counter = 0;
        private ObservableCollection<TouristDTO> _touristsDTO { get; set; }
        private ObservableCollection<KeyPointDTO> _keyPoints { get; set; }
        public ActiveTourViewModel(TourDTO tour, Boolean activeTourExists, UserDTO guide)
        {
            _tourDTO = tour;
            _loggedGuide= guide;
            
            IUserRepository userRepository = Injector.CreateInstance<IUserRepository>();
            ITourRepository tourRepository = Injector.CreateInstance<ITourRepository>();
            IKeyPointRepository keyPointsRepository = Injector.CreateInstance<IKeyPointRepository>();
            ITourReservationRepository tourReservationRepository = Injector.CreateInstance<ITourReservationRepository>();
            ITouristRepository touristRepository = Injector.CreateInstance<ITouristRepository>();
            ITourReviewRepository tourReviewRepository = Injector.CreateInstance<ITourReviewRepository>();
            IVoucherRepository voucherRepository = Injector.CreateInstance<IVoucherRepository>();
            _tourReservationService = new TourReservationService(tourReservationRepository, userRepository, touristRepository, tourReviewRepository, voucherRepository);
            _tourService = new TourService(tourRepository, userRepository, touristRepository, tourReservationRepository, tourReviewRepository, voucherRepository);
            _tourDTO.IsActive = true;
            _tourService.Update(_tourDTO.ToTourAllParam());
            UserService userService = new UserService(userRepository);
            _userDTO = new UserDTO(userService.GetById(_tourDTO.GuideId));
            List<TouristDTO> touristsDTO = _tourReservationService.GetReservationTourists(tour.ToTourAllParam()).Select(t => new TouristDTO(t)).ToList();
            _touristsDTO = new ObservableCollection<TouristDTO>(touristsDTO);
            _markPointCommand = new RelayCommand(MarkPoint);
            _showTourReviewsWindowCommand = new RelayCommand(ShowTourReviewsWindow);
            _cancelTourCommand = new RelayCommand(CancelTour);
            _addNewTourCommand = new RelayCommand(AddNewTour);
            _showTourRequestCommand = new RelayCommand(ShowTourRequest);
            _showTourDetailsCommand = new RelayCommand(ShowTourDetails);
            _touristJoiningPointCommand = new RelayCommand(TouristJoiningPoint);
            _showAllToursCommand = new RelayCommand(ShowAllTours);
            _logoutCommand = new RelayCommand(Logout);
            _showTourStatisticsCommand = new RelayCommand(ShowTourStatistics);
            _showMainWindowCommand = new RelayCommand(ShowMainWindow);
            _keyPointsService = new KeyPointService(keyPointsRepository);
            List<KeyPointDTO> keypointsDTO = _keyPointsService.GetKeyPointsForTour(tour.ToTourAllParam()).Select(k=> new KeyPointDTO(k)).ToList();
            _keyPoints = new ObservableCollection<KeyPointDTO>(keypointsDTO);
        }
        public TourDTO Tour
        {
            get { return _tourDTO; }
            set
            {
                _tourDTO = value;
                OnPropertyChanged();
            }
        }
        public UserDTO User
        {
            get { return _userDTO; }
            set
            {
                _userDTO = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand MarkPointCommand
        {
            get { return _markPointCommand; }
            set
            {
                _markPointCommand = value;
                OnPropertyChanged();
            }
        }
        private void MarkPoint(object parameter)
        {
            KeyPointDTO point=parameter as KeyPointDTO;
            _tourDTO.CurrentKeyPoint = point.Name;
            _tourService.Update(_tourDTO.ToTourAllParam());
            foreach(var keypoint in _keyPoints)
            {
                keypoint.IsCurrent = false;
                _keyPointsService.Update(keypoint.ToKeyPoint());
            }
            point.IsCurrent = true;
            point.HasPassed = true;
            _keyPointsService.Update(point.ToKeyPoint());
           /* if (point.Type == Model.Enums.KeyPointsType.Ending)
            {
                _tourDTO.CurrentKeyPoint = "finished";
                _tourDTO.IsActive = false;
                _tourService.Update(_tourDTO.ToTourAllParam());
                ActiveTourWindow.GetInstance().Close();
                if (_tourDTO.TouristsPresent != 0)
                {
                    ShowTourReviewsWindow();
                }
            }*/

                _tourDTO.CurrentKeyPoint = point.Name;
                _tourDTO.IsActive = true;
                _tourService.Update(_tourDTO.ToTourAllParam());

        }
        public RelayCommand ShowTourDetailsCommand
        {
            get { return _showTourDetailsCommand; }
            set
            {
                _showTourDetailsCommand = value;
                OnPropertyChanged();
            }
        }
        private void ShowTourDetails()
        {
            TourDetailsWindow tourDetails = new TourDetailsWindow(_tourDTO, _loggedGuide);
            tourDetails.Show();
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
            TourStatisticsWindow tourStatistics = new TourStatisticsWindow(_loggedGuide);
            tourStatistics.Show();
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
            GuideMainWindow mainWindow = new GuideMainWindow(_loggedGuide.ToUser());
            mainWindow.Show();
            ActiveTourWindow.GetInstance().Close();
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
            AllToursWindow allToursView = new AllToursWindow(_loggedGuide);

            allToursView.Show();
            ActiveTourWindow.GetInstance().Close();
        }
        public RelayCommand ShowTourReviewsWindowCommand
        {
            get { return _showTourReviewsWindowCommand; }
            set
            {
                _showTourReviewsWindowCommand = value;
                OnPropertyChanged();
            }
        }
        private void ShowTourReviewsWindow()
        {
            TourReviewsWindow tourReviews = new TourReviewsWindow(_tourDTO);
            tourReviews.Show();
        }
        public RelayCommand TouristJoiningPointCommand
        {
            get { return _touristJoiningPointCommand; }
            set
            {
                _touristJoiningPointCommand = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<TouristDTO> Tourists
        {
            get { return _touristsDTO; }
            set
            {
                _touristsDTO = value;
                OnPropertyChanged();
            }
        }
        private void TouristJoiningPoint(object parameter)
        {
            TouristDTO selectedTouristDTO = parameter as TouristDTO;
            if (selectedTouristDTO == null)
            {
                return;
            }
            if (selectedTouristDTO.JoiningKeyPoint != string.Empty)
            {
                MessageBox.Show("Ovaj turista se vec pridruzio turi", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            
            selectedTouristDTO.JoiningKeyPoint = _tourDTO.CurrentKeyPoint;
            if (_counter != 0)
            {
                _counter++;
                _tourDTO.TouristsPresent = _counter;
            }
            else
            {
                _counter = _tourDTO.TouristsPresent;
                _counter++;
                _tourDTO.TouristsPresent = _counter;
            }
            AddJoiningPoint(selectedTouristDTO);
            _tourService.Update(_tourDTO.ToTourAllParam());
        }
        private void AddJoiningPoint(TouristDTO touristDTO)
        {
            foreach (TourReservation reservation in _tourReservationService.GetAll())
            {
                if (reservation.TourId == _tourDTO.Id)
                {
                    foreach (Model.Tourist tourist in reservation.Tourists)
                    {
                        if (touristDTO.Name == tourist.Name && touristDTO.Surname == tourist.Surname)
                        {
                            tourist.JoiningKeyPoint = _tourDTO.CurrentKeyPoint;
                            _tourReservationService.Update(reservation);
                        }
                    }
                }
            }
        }
        public ObservableCollection<KeyPointDTO> KeyPoints
        {
            get { return _keyPoints; }
            set
            {
                _keyPoints = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand CancelTourCommand
        {
            get { return _cancelTourCommand; }
            set
            {
                _cancelTourCommand = value;
                OnPropertyChanged();
            }
        }
        private void CancelTour()
        {
            _tourDTO.CurrentKeyPoint = "finished";
            _tourDTO.IsActive = false;
            foreach (var keypoint in _keyPoints)
            {
                keypoint.IsCurrent = false;
                _keyPointsService.Update(keypoint.ToKeyPoint());
            }
            _tourService.Update(_tourDTO.ToTourAllParam());
           
            if (_tourDTO.TouristsPresent != 0)
            {
                
                ShowMainWindow();
                ShowTourReviewsWindow();
                ActiveTourWindow.GetInstance().Close();
            }
            else
            {
                ShowMainWindow();
                ActiveTourWindow.GetInstance().Close();
            }
        }
        public RelayCommand ShowTourRequestCommand
        {
            get { return _showTourRequestCommand; }
            set
            {
                _showTourRequestCommand = value;
                OnPropertyChanged();
            }
        }
        private void ShowTourRequest()
        {
            TourRequestWindow requests = new TourRequestWindow(_userDTO);
            requests.Show();

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
            ActiveTourWindow.GetInstance().Close();
        }

    }
}
