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
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.ViewModel.Guide
{
    class TourDetailsViewModel : ViewModel
    {
        private TourDTO _tourDTO;
        private TourService _tourService;
        private KeyPointService _keyPointService;
        private string _points;

        public TourDetailsViewModel(TourDTO tourDTO)
        {
            _tourDTO = tourDTO;
            IUserRepository userRepository = Injector.CreateInstance<IUserRepository>();
            ITourRepository tourRepository = Injector.CreateInstance<ITourRepository>();
            IKeyPointRepository keyPointsRepository = Injector.CreateInstance<IKeyPointRepository>();
            ITourReservationRepository tourReservationRepository = Injector.CreateInstance<ITourReservationRepository>();
            ITouristRepository touristRepository = Injector.CreateInstance<ITouristRepository>();
            ITourReviewRepository tourReviewRepository = Injector.CreateInstance<ITourReviewRepository>();
            IVoucherRepository voucherRepository = Injector.CreateInstance<IVoucherRepository>();
            _tourService = new TourService(tourRepository, userRepository, touristRepository, tourReservationRepository, tourReviewRepository, voucherRepository);
            _keyPointService = new KeyPointService(keyPointsRepository);
            List<KeyPointDTO> keypointsDTO = _keyPointService.GetKeyPointsForTour(tourDTO.ToTourAllParam()).Select(k => new KeyPointDTO(k)).ToList();
            _points = "";
            foreach (var keypoint in keypointsDTO)
            {
                _points += keypoint.Name + ", ";
            }
            _points = _points.Remove(_points.Length - 2);
        }
        public string Points
        {
            get { return _points; }
            set
            {
                _points = value;
                OnPropertyChanged();
            }
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
    }
}
