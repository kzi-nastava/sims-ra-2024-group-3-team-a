using BookingApp.Model.Enums;
using BookingApp.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Navigation;

namespace BookingApp.Service
{
    public class AccommodationStatisticsService
    {
        private IAccommodationReservationRepository _accommodationReservationRepository;
        private IAccommodationReservationChangeRequestRepository _accommodationReservationChangeRequestRepository;

        public AccommodationStatisticsService(IAccommodationReservationRepository accommodationReservationRepository, IAccommodationReservationChangeRequestRepository accommodationReservationChangeRequestRepository)
        {
            _accommodationReservationRepository = accommodationReservationRepository;
            _accommodationReservationChangeRequestRepository = accommodationReservationChangeRequestRepository;
        }

        public int GetReservationsNumber(int accommodationId, int year, int month = 0)
        {
            int reservations = 0;
            foreach(var reservation in _accommodationReservationRepository.GetAll())
            {
                if (reservation.BeginDate.Year == year && accommodationId == reservation.AccommodationId && (reservation.BeginDate.Month == month || month == 0))
                {
                    reservations++;
                }
            }
            return reservations;
        }  
        
        public int GetCancellationsNumber(int accommodationId, int year, int month = 0)
        {
            int cancellations = 0;
            foreach (var reservation in _accommodationReservationRepository.GetAll())
            {
                if (reservation.BeginDate.Year == year && accommodationId == reservation.AccommodationId && reservation.Canceled && reservation.Canceled == true && (reservation.BeginDate.Month == month || month == 0))
                {
                    cancellations++;
                }
            }
            return cancellations;
        }

        public int GetRenovationRecommendationsNumber(int accommodationId, int year, int month = 0)
        {
            int recommendations = 0;
            foreach (var reservation in _accommodationReservationRepository.GetAll())
            {
                if (reservation.BeginDate.Year == year && accommodationId == reservation.AccommodationId && reservation.Rating.GuestRenovationRating != 0 && (reservation.BeginDate.Month == month || month == 0))
                {
                    recommendations++;
                }
            }
            return recommendations;
        }

        public int GetAccommodationReservationChangeRequestsNumber(int accommodationId, int year, int month = 0)
        {
            int changeRequests = 0;
            foreach (var changeRequest in _accommodationReservationChangeRequestRepository.GetAll())
            {
                if(_accommodationReservationRepository.GetById(changeRequest.AccommodationReservationId).AccommodationId == accommodationId && changeRequest.Status == AccommodationChangeRequestStatus.Accepted && changeRequest.BeginDateNew.Year == year && (changeRequest.BeginDateNew.Month == month || month == 0))
                {
                    changeRequests++;
                }
            }
            return changeRequests;
        }
        
        public int GetMostOccupiedYear(int accommodationId, int[] years)
        {
            int mostOccupiedYear = years[0];
            foreach(int year in years)
            {
                if(GetReservationsPrecentageByYear(accommodationId, year) > GetReservationsPrecentageByYear(accommodationId, mostOccupiedYear))
                {
                    mostOccupiedYear = year;
                }
            }
            return mostOccupiedYear;
        }
        public int GetMostOccupiedMonth(int accommodationId, int year, int[] months)
        {
            int mostOccupiedMonth = months[0];
            foreach (int month in months)
            {
                if (GetReservationsPrecentageByMonth(accommodationId, year, month) > GetReservationsPrecentageByMonth(accommodationId, year, mostOccupiedMonth))
                {
                    mostOccupiedMonth = month;
                }
            }
            return mostOccupiedMonth;
        }
        private double GetReservationsPrecentageByYear(int accommodationId, int year)
        {
            int reservationsDays = 0;
            foreach (var reservation in _accommodationReservationRepository.GetAll())
            {
                if (reservation.BeginDate.Year == year && accommodationId == reservation.AccommodationId)
                { 
                    DateOnly i = reservation.BeginDate;
                    for (; i <= reservation.EndDate; i = i.AddDays(1))
                    {
                        reservationsDays++;
                    }
                }
            }
            double reservationPrecentage = (double)reservationsDays / (DateTime.IsLeapYear(year) ? 366 : 365);
            return reservationPrecentage;
        }
        private double GetReservationsPrecentageByMonth(int accommodationId, int year, int month)
        {
            int reservationsDays = 0;
            foreach (var reservation in _accommodationReservationRepository.GetAll())
            {
                if (reservation.BeginDate.Year == year && accommodationId == reservation.AccommodationId && reservation.BeginDate.Month == month)
                {
                    DateOnly i = reservation.BeginDate;
                    for (; i <= reservation.EndDate; i = i.AddDays(1))
                    {
                        reservationsDays++;
                    }
                }
            }
            double reservationPrecentage = (double)reservationsDays / DateTime.DaysInMonth(year, month); 
            return reservationPrecentage;
        }
    }
}
