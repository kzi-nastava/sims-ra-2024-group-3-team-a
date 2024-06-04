using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.InjectorNameSpace;
using BookingApp.Repository;
using BookingApp.Repository.Interfaces;
using BookingApp.Service;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace BookingApp.ViewModel.Guide
{
    public class ReviewDetailsViewModel : ViewModel
    {
        private TouristDTO _touristDTO;
        private TourDTO _tourDTO;
        private readonly TouristService _touristService;
        private readonly TourService _tourService;
        private TourReviewDTO _review;
        private ScrollViewer _scrollViewer;
        public RelayCommand ScrollLeftCommand { get; }
        public RelayCommand ScrollRightCommand { get; }

        public ReviewDetailsViewModel(TouristDTO touristDTO)
        {
            _touristDTO = touristDTO;
            
            ITouristRepository touristRepository = Injector.CreateInstance<ITouristRepository>();
            IUserRepository userRepository = Injector.CreateInstance<IUserRepository>();
            ITourRepository tourRepository = Injector.CreateInstance<ITourRepository>();
            ITourReviewRepository tourReviewRepository = Injector.CreateInstance<ITourReviewRepository>();
            IKeyPointRepository keyPointsRepository = Injector.CreateInstance<IKeyPointRepository>();
            ITourReservationRepository tourReservationRepository = Injector.CreateInstance<ITourReservationRepository>();
            IVoucherRepository voucherRepository = Injector.CreateInstance<IVoucherRepository>();
            _touristService = new TouristService(touristRepository);
            _tourService = new TourService(tourRepository, userRepository,touristRepository,tourReservationRepository,tourReviewRepository,voucherRepository);
            _tourDTO = new TourDTO (_tourService.GetById(_touristDTO.Review.TourId));
            _review = _touristDTO.Review;
            ScrollLeftCommand = new RelayCommand(ScrollLeft);
            ScrollRightCommand = new RelayCommand(ScrollRight);
        }

        public TourDTO TourDTO
        {
            get { return _tourDTO; }
            set
            {
                _tourDTO = value;
                OnPropertyChanged();
            }
        }
        public TouristDTO TouristDTO
        {
            get { return _touristDTO; }
            set
            {
                _touristDTO = value;
                OnPropertyChanged();
            }
        }
        private void ScrollLeft(object parameter)
        {
            if (parameter is ScrollViewer scrollViewer)
            {
                scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset - 150);
            }
        }

        private void ScrollRight(object parameter)
        {
            if (parameter is ScrollViewer scrollViewer)
            {
                scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset + 150);
            }
        }

        public TourReviewDTO Review
        {
            get { return _review; }
            set
            {
                _review = value;
                OnPropertyChanged();
            }
        }

    }
}
