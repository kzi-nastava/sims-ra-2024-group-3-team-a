using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
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
        private AccommodationReservationRepository _accommodationReservationRepository = new AccommodationReservationRepository();

        private AccommodationService _accommodationService = new AccommodationService();
        private UserService _userService = new UserService();

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
        public List<AccommodationReservation> GetUserReviewedAccommodationReservations(User loggedInOwner)
        {
            List<AccommodationReservation> userReviewedAccommodationReservations = new List<AccommodationReservation>();
            foreach (var reservation in _accommodationReservationRepository.GetAll())
            {
                Accommodation accommodation = _accommodationService.GetById(reservation.AccommodationId);

                if (IsLoggedOwner(accommodation, loggedInOwner) && IsUserReviewed(reservation))
                    userReviewedAccommodationReservations.Add(reservation);
            }

            return userReviewedAccommodationReservations;
        }
        public double GetAverageRating(User loggedInOwner)
        {
            List<AccommodationReservation> userReviewedAccommodationReservations = GetUserReviewedAccommodationReservations(loggedInOwner);
            double averageRating = _accommodationReservationRepository.GetAverageRating(userReviewedAccommodationReservations);

            return averageRating;
        }
        public User SetSuperOwner(User loggedInOwner)
        {
            int reviewNumber = GetUserReviewedAccommodationReservations(loggedInOwner).Count();
            double averageRating = GetAverageRating(loggedInOwner);
            if (averageRating >= 4.5 && reviewNumber > 50)
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
            if (reservation.Rating.GuestCleannessRating != 0 && reservation.Rating.OwnerCleannessRating != 0)
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
