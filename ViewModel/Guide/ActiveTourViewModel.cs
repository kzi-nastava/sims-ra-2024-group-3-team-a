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
        //private List<string> _keyPointsString;
        private RelayCommand _markPointCommand;
        private RelayCommand _showTourReviewsWindowCommand;
        private RelayCommand _touristJoiningPointCommand;
        private RelayCommand _cancelTourCommand;
        private TourDTO _tourDTO;
        private int _counter = 0;
        private ObservableCollection<TouristDTO> _touristsDTO { get; set; }
        private ObservableCollection<KeyPointDTO> _keyPoints { get; set; }
        public ActiveTourViewModel(TourDTO tour, Boolean activeTourExists)
        {
            _tourDTO = tour;

            IUserRepository userRepository = Injector.CreateInstance<IUserRepository>();
            ITourRepository tourRepository = Injector.CreateInstance<ITourRepository>();
            IKeyPointRepository keyPointsRepository = Injector.CreateInstance<IKeyPointRepository>();
            ITourReservationRepository tourReservationRepository = Injector.CreateInstance<ITourReservationRepository>();
            ITouristRepository touristRepository = Injector.CreateInstance<ITouristRepository>();
            ITourReviewRepository tourReviewRepository = Injector.CreateInstance<ITourReviewRepository>();
            IVoucherRepository voucherRepository = Injector.CreateInstance<IVoucherRepository>();
            _tourReservationService = new TourReservationService(tourReservationRepository, userRepository, touristRepository, tourReviewRepository, voucherRepository);
            _tourService = new TourService(tourRepository, userRepository, touristRepository, tourReservationRepository, tourReviewRepository, voucherRepository);
            List<TouristDTO> touristsDTO = _tourReservationService.GetReservationTourists(tour.ToTourAllParam()).Select(t => new TouristDTO(t)).ToList();
            _touristsDTO = new ObservableCollection<TouristDTO>(touristsDTO);
            //_keyPointsString = new List<string>();
            _markPointCommand = new RelayCommand(MarkPoint);
            _showTourReviewsWindowCommand = new RelayCommand(ShowTourReviewsWindow);
            _cancelTourCommand = new RelayCommand(CancelTour);
            _touristJoiningPointCommand = new RelayCommand(TouristJoiningPoint);
            _keyPointsService = new KeyPointService(keyPointsRepository);
            List<KeyPointDTO> keypointsDTO = _keyPointsService.GetKeyPointsForTour(tour.ToTourAllParam()).Select(k=> new KeyPointDTO(k)).ToList();
            _keyPoints = new ObservableCollection<KeyPointDTO>(keypointsDTO);
          /*  _keyPointsString.Add(_keyPoints.Begining);
            foreach(string str in _keyPoints.Middle)
            {
                _keyPointsString.Add(str);
            }
            _keyPointsString.Add(_keyPoints.Ending);*/
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
            if (point.Type == Model.Enums.KeyPointsType.Ending)
            {
                _tourDTO.CurrentKeyPoint = "finished";
                _tourDTO.IsActive = false;
                _tourService.Update(_tourDTO.ToTourAllParam());
                ActiveTourWindow.GetInstance().Close();
                if (_tourDTO.TouristsPresent != 0)
                {
                    ShowTourReviewsWindow();
                }
            }
            else
            {
                _tourDTO.CurrentKeyPoint = point.Name;
                _tourDTO.IsActive = true;
                _tourService.Update(_tourDTO.ToTourAllParam());
            }
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
                MessageBox.Show("This tourist has already joined the tour", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
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
            ActiveTourWindow.GetInstance().Close();
            if (_tourDTO.TouristsPresent != 0)
            {
                ShowTourReviewsWindow();
            }
        }
    }
}
