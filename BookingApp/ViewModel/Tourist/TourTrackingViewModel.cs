using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.InjectorNameSpace;
using BookingApp.Model;
using BookingApp.Repository.Interfaces;
using BookingApp.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BookingApp.ViewModel.Tourist
{
    public class TourTrackingViewModel : ViewModel
    {

        private KeyPointService _keyPointsService;
        private TourDTO _tourDTO;
        private TourReservationService _tourReservationService;
        private TourService _tourService;
        private ObservableCollection<TouristDTO> _touristsDTO;
        private ScrollViewer _scrollViewer;
        private ObservableCollection<KeyPointDTO> _keyPoints { get; set; }
        public RelayCommand ScrollLeftCommand { get; }
        public RelayCommand ScrollRightCommand { get; }
        public Action CloseAction { get; set; }
        private RelayCommand _closeWindowCommand;
        public TourTrackingViewModel(TourDTO tourDTO)
        { 
            _tourDTO = tourDTO;
            IKeyPointRepository keyPointsRepository = Injector.CreateInstance<IKeyPointRepository>();
            IUserRepository userRepository = Injector.CreateInstance<IUserRepository>();
            ITourRepository tourRepository = Injector.CreateInstance<ITourRepository>();
            ITouristRepository touristRepository = Injector.CreateInstance<ITouristRepository>();
            ITourReservationRepository tourReservationRepository = Injector.CreateInstance<ITourReservationRepository>();
            ITourReviewRepository tourReviewRepository = Injector.CreateInstance<ITourReviewRepository>();
            IVoucherRepository voucherRepository = Injector.CreateInstance<IVoucherRepository>();
            _tourReservationService = new TourReservationService(tourReservationRepository, userRepository, touristRepository, tourReviewRepository, voucherRepository);
            _tourService = new TourService(tourRepository, userRepository, touristRepository, tourReservationRepository, tourReviewRepository, voucherRepository);
            _keyPointsService = new KeyPointService(keyPointsRepository);
            List<TouristDTO> tourists = _tourService.GetTourists(tourDTO.ToTourAllParam()).Select(tourists => new TouristDTO(tourists)).ToList();
            _touristsDTO = new ObservableCollection<TouristDTO>(tourists);
            List<KeyPointDTO> keypointsDTO = _keyPointsService.GetKeyPointsForTour(_tourDTO.ToTourAllParam()).Select(k => new KeyPointDTO(k)).ToList();
            _keyPoints = new ObservableCollection<KeyPointDTO>(keypointsDTO);
            ScrollLeftCommand = new RelayCommand(ScrollLeft);
            ScrollRightCommand = new RelayCommand(ScrollRight);
            _closeWindowCommand = new RelayCommand(CloseWindow);
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

        public ObservableCollection<TouristDTO> TouristsDTO
        {
            get
            {
                return _touristsDTO;
            }
            set
            {
                _touristsDTO = value;
                OnPropertyChanged();
            }
        }
        public void SetScrollViewer(ScrollViewer scrollViewer)
        {
            _scrollViewer = scrollViewer;
        }

        private void ScrollRight(object obj)
        {

            _scrollViewer?.ScrollToHorizontalOffset(_scrollViewer.HorizontalOffset + 140);
        }
        private void ScrollLeft(object obj)
        {
            _scrollViewer?.ScrollToHorizontalOffset(_scrollViewer.HorizontalOffset - 140);
        }
        public RelayCommand CloseWindowCommand
        {
            get
            {
                return _closeWindowCommand;
            }
            set
            {
                _closeWindowCommand = value;
                OnPropertyChanged();
            }
        }
        public void CloseWindow()
        {


            CloseAction();
        }
    }
}
