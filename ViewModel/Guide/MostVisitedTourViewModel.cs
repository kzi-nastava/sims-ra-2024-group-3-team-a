using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.InjectorNameSpace;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Repository.Interfaces;
using BookingApp.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.ViewModel.Guide
{
    class MostVisitedTourViewModel : ViewModel
    {
        private TourDTO _mostVisitedTourDTO;
        private TourService _tourService;
        public MostVisitedTourViewModel(int year)
        {
            ITourRepository tourRepository = Injector.CreateInstance<ITourRepository>();
            IUserRepository userRepository = Injector.CreateInstance<IUserRepository>();
            ITouristRepository touristRepository = Injector.CreateInstance<ITouristRepository>();
            ITourReservationRepository tourReservationRepository = Injector.CreateInstance<ITourReservationRepository>();
            ITourReviewRepository tourReviewRepository = Injector.CreateInstance<ITourReviewRepository>();
            IVoucherRepository voucherRepository = Injector.CreateInstance<IVoucherRepository>();
            _tourService = new TourService(tourRepository, userRepository, touristRepository, tourReservationRepository, tourReviewRepository, voucherRepository);
            Tour tour = _tourService.GetMostVisitedByYear(year);
            if (tour != null) 
            { 
            _mostVisitedTourDTO = new TourDTO(tour);
            }else
            {
                _mostVisitedTourDTO = null;
            }
        }
        public TourDTO MostVisitedTourDTO
        {
            get { return _mostVisitedTourDTO; }
            set
            {
                _mostVisitedTourDTO = value;
                OnPropertyChanged();
            }
        }
    }

}
