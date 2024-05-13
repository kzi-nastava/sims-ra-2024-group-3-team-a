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
using System.Windows.Controls;

namespace BookingApp.Service
{
    public class AccommodationReservationService
    {
        private IAccommodationReservationRepository _accommodationReservationRepository;

        private AccommodationService _accommodationService;
        private UserService _userService;

        public AccommodationReservationService(IAccommodationReservationRepository accommodationReservationRepository, IAccommodationRepository accommodationRepository, IUserRepository userRepository)
        {
            _accommodationReservationRepository = accommodationReservationRepository;
            _accommodationService = new AccommodationService(accommodationRepository);
            _userService = new UserService(userRepository);
        }

        public List<AccommodationReservation> GetAll()
        {
            return _accommodationReservationRepository.GetAll();
        }

        public AccommodationReservation Save(AccommodationReservation accommodationReservation)
        {
            return _accommodationReservationRepository.Save(accommodationReservation);
        }

        public void Delete(AccommodationReservation accommodationReservation)
        {
            _accommodationReservationRepository.Delete(accommodationReservation);
        }

        public AccommodationReservation Update(AccommodationReservation accommodationReservation)
        {
            return _accommodationReservationRepository.Update(accommodationReservation);
        }

        public AccommodationReservation GetById(int id)
        {
            return _accommodationReservationRepository.GetById(id);
        }
        public List<AccommodationReservation> GetAllByAccommodationId(int id)
        {
            return _accommodationReservationRepository.GetAllByAccommodationId(id);
        }
        public List<AccommodationReservation> GetAllByGuestId(int id)
        {
            return _accommodationReservationRepository.GetAllByGuestId(id);
        }

        public List<AccommodationReservation> GetAllRatedByGuestId(int id)
        {
            return _accommodationReservationRepository.GetAllRatedByGuestId(id);
        }

        public List<AccommodationReservation> GetFinishedAccommodationReservations(User loggedInOwner)
        {
            List<AccommodationReservation>  finishedAccommodationReservations = new List<AccommodationReservation>();
            foreach (var reservation in _accommodationReservationRepository.GetAll())
            {
                Accommodation accommodation = _accommodationService.GetById(reservation.AccommodationId);
                    
                if (IsLoggedOwner(accommodation, loggedInOwner) && (IsNotExpiredOrCanceled(reservation) || IsOwnerReviewed(reservation)))
                    finishedAccommodationReservations.Add(reservation);
            }

            return finishedAccommodationReservations;
        }
        public List<AccommodationReservation> GetUserAndOwnerReviewedAccommodationReservations(User loggedInOwner)
        {
            List<AccommodationReservation> userAndOwnerReviewedAccommodationReservations = new List<AccommodationReservation>();
            foreach (var reservation in _accommodationReservationRepository.GetAll())
            {
                Accommodation accommodation = _accommodationService.GetById(reservation.AccommodationId);

                if (IsLoggedOwner(accommodation, loggedInOwner) && IsUserReviewed(reservation) && IsOwnerReviewed(reservation))
                    userAndOwnerReviewedAccommodationReservations.Add(reservation);
            }

            return userAndOwnerReviewedAccommodationReservations;
        }
        public List<AccommodationReservation> GetUserReviewedAccommodationReservations(User loggedInOwner)
        {
            List<AccommodationReservation> userAndOwnerReviewedAccommodationReservations = new List<AccommodationReservation>();
            foreach (var reservation in _accommodationReservationRepository.GetAll())
            {
                Accommodation accommodation = _accommodationService.GetById(reservation.AccommodationId);

                if (IsLoggedOwner(accommodation, loggedInOwner) && IsUserReviewed(reservation))
                    userAndOwnerReviewedAccommodationReservations.Add(reservation);
            }

            return userAndOwnerReviewedAccommodationReservations;
        }
        public List<AccommodationReservation> GetGuestLastYearAccommodationReservations(User loggedInGuest)
        {
            List<AccommodationReservation> guestLastYearAccommodationReservations = new List<AccommodationReservation>();
            foreach (var reservation in _accommodationReservationRepository.GetAllByGuestId(loggedInGuest.Id))
            {
                if(reservation.BeginDate.AddDays(365) > DateOnly.FromDateTime(DateTime.Now))
                {
                    guestLastYearAccommodationReservations.Add(reservation);
                }    
            }
            return guestLastYearAccommodationReservations;
        }
        public double GetAverageRating(User loggedInOwner)
        {
            List<AccommodationReservation> userReviewedAccommodationReservations = GetUserReviewedAccommodationReservations(loggedInOwner);
            double averageRating = _accommodationReservationRepository.GetAverageRating(userReviewedAccommodationReservations);

            return averageRating;
        }
        public User SetSuperOwner(User loggedInOwner)
        {
            int reviewNumber = GetUserAndOwnerReviewedAccommodationReservations(loggedInOwner).Count();
            double averageRating = GetAverageRating(loggedInOwner);
            if (averageRating >= 2.5 && reviewNumber > 1)
            {
                loggedInOwner.IsSuper = true;
                _userService.Update(loggedInOwner);
            }
            else
            {
                loggedInOwner.IsSuper = false;
                _userService.Update(loggedInOwner);
            }

            return loggedInOwner;
        }

        public User SetSuperGuest(User loggedInGuest)
        {
            int numberOfReservations = GetGuestLastYearAccommodationReservations(loggedInGuest).Count();
            if (numberOfReservations >= 10)
            {
                loggedInGuest.IsSuper = true;
                _userService.Update(loggedInGuest);
            }
            else
            {
                loggedInGuest.IsSuper = false;
                _userService.Update(loggedInGuest);
            }

            return loggedInGuest;
        }
        private bool IsLoggedOwner(Accommodation accommodation, User loggedInOwner)
        {
            if (accommodation.OwnerId == loggedInOwner.Id)
            {
                return true;
            }
            return false;
        }
        private bool IsNotExpiredOrCanceled(AccommodationReservation reservation)
        {
            if ((reservation.EndDate <= DateOnly.FromDateTime(DateTime.Now) && DateOnly.FromDateTime(DateTime.Now) <= reservation.EndDate.AddDays(5)) && !reservation.Canceled)
            {
                return true;
            }
            return false;
        }
        private bool IsOwnerReviewed(AccommodationReservation reservation)
        {
            if (reservation.Rating.OwnerCleannessRating != 0)
            {
                return true;
            }
            return false;
        }
        private bool IsUserReviewed(AccommodationReservation reservation)
        {
            if (reservation.Rating.GuestCleannessRating != 0)
            {
                return true;
            }
            return false;
        }

        public List<DateTime> GetOccupiedDates(AccommodationReservation accommodationReservation)
        {
            List<DateTime> occupiedDates = new List<DateTime>();

            foreach (var reservation in GetAll())
            {
                if (accommodationReservation.AccommodationId == reservation.AccommodationId && reservation.GuestId != accommodationReservation.GuestId)
                { 
                    DateTime beginDate = new DateTime(reservation.BeginDate.Year, reservation.BeginDate.Month, reservation.BeginDate.Day);
                    DateTime endDate = new DateTime(reservation.EndDate.Year, reservation.EndDate.Month, reservation.EndDate.Day);

                    for (DateTime date = beginDate; date <= endDate; date = date.AddDays(1))
                    {
                        occupiedDates.Add(date);
                    }
                }
            }

            return occupiedDates;
        }
    }
}
