using BookingApp.DTO;
using BookingApp.InjectorNameSpace;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookingApp.Service
{
    public class TourService
    {
        private ITourRepository _tourRepository;
        private TourReservationService _tourReservationService;

        public TourService(ITourRepository tourRepository, IUserRepository userRepository, ITouristRepository touristRepository, ITourReservationRepository tourReservationRepository, ITourReviewRepository tourReviewRepository, IVoucherRepository voucherRepository) 
        {
            _tourRepository = tourRepository;
            _tourReservationService = new TourReservationService(tourReservationRepository, userRepository, touristRepository, tourReviewRepository, voucherRepository);
        }

        public List<Tour> GetAll() 
        {
            return _tourRepository.GetAll();
        }

        public Tour GetById(int id)
        {
            return _tourRepository.GetById(id);
        }
        public Tour Save(Tour tour) 
        {
            return _tourRepository.Save(tour);
        }
        public void Delete(Tour tour)
        {
             _tourRepository.Delete(tour);
        }
        public Tour Update(Tour tour)
        {
            return _tourRepository.Update(tour);
        }
        public List<Tour> GetActiveTours()
        {
            List<Tour> activeTours = new List<Tour>();
                foreach (Tour tour in GetAll())
                {
                    foreach (TourReservation tourReservation in _tourReservationService.GetAll())
                    {
                        if (tour.Id == tourReservation.TourId && tour.IsActive)
                        {
                            activeTours.Add(tour);
                        }
                    }
                }
           return activeTours.Distinct().ToList();
        }
        public List<Tour> GetAllActiveTours()
        {
            List<Tour> activeTours = new List<Tour>();
            foreach (Tour tour in GetAll())
            {
                if (tour.IsActive)
                {
                    activeTours.Add(tour);
                }
            }
            return activeTours;
        }
        public List<Tour> GetUnactiveTours()
        {
            List<Tour> unactiveTours = new List<Tour>();
            foreach (Tour tour in GetAll())
            {
                foreach (TourReservation tourReservation in _tourReservationService.GetAll())
                {
                    if (tour.Id == tourReservation.TourId && !tour.IsActive && !tour.CurrentKeyPoint.Equals("finished"))
                    {
                        unactiveTours.Add(tour);
                    }
                }
            }
            return unactiveTours.Distinct().ToList();
        }
        public List<Tour> GetFinishedTours()
        {
            List<Tour> finishedTours = new List<Tour>();
            foreach (Tour tour in GetAll())
            {
                foreach (TourReservation tourReservation in _tourReservationService.GetAll())
                {
                    if (tour.Id == tourReservation.TourId && tour.CurrentKeyPoint.Equals("finished"))
                    {
                        finishedTours.Add(tour);
                    }
                }
            }
            return finishedTours.Distinct().ToList();
        }
        public List<Tour> GetAllFinishedTours()
        {
            List<Tour> finishedTours = new List<Tour>();
            foreach (Tour tour in GetAll())
            {
                    if (tour.CurrentKeyPoint.Equals("finished"))
                    {
                        finishedTours.Add(tour);
                    }

            }
            return finishedTours;
        }
        public Tour GetMostVisitedTour()
        {
            return _tourRepository.GetMostVisitedTour();
        }
        public Tour GetMostVisitedByYear(int year)
        {
            return _tourRepository.GetMostVisitedByYear(year);
        }
        public List<Tour> GetNotCancelled()
        {
            return _tourRepository.GetNotCancelled();
        }
        public List<Tour> GetUpcoming()
        {
            return _tourRepository.GetUpcoming();
        }
        public List<Tour> GetTodayTours(User user)
        {
            List<Tour> toursToday = new List<Tour>();
            foreach (Tour tour in GetAll())
            {
                if (tour.BeginingTime.Date == DateTime.Today && tour.GuideId == user.Id)
                {
                    toursToday.Add(tour);
                }
            }
            return toursToday;
        }
        public List<Tour> GetToursWithSameLocation(Tour tour)
        {
            List<Tour> tours = new List<Tour>();
            foreach (Tour t in GetAll())
            {
                if (t.Place.Country == tour.Place.Country && t.Place.City == tour.Place.City && t.CurrentCapacity != 0)
                {
                    tours.Add(t);
                }
            }
            return tours;
        }

        public List<Tourist> GetTourists(Tour tour)
        {
            List<Tourist> tourists = new List<Tourist>();

            foreach (TourReservation reservation in _tourReservationService.GetAll())
            {
                if (reservation.TourId == tour.Id)
                {
                    foreach (Tourist tourist in reservation.Tourists)
                    {
                        
                        tourists.Add(tourist);
                    }
                }
            }
            return tourists;
        }

        
    }
}
