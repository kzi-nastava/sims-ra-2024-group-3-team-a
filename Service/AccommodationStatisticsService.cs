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

        public int GetReservationsNumberByYear(int accommodationId, int year, int month = 0)
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
        
        public int GetCancellationsNumberByYear(int accommodationId, int year, int month = 0)
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

        public int GetAccommodationReservationChangeRequestsByYear(int accommodationId, int year, int month = 0)
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
                if(GetReservationsNumberByYear(accommodationId, year) > GetReservationsNumberByYear(accommodationId, mostOccupiedYear))
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
                if (GetReservationsNumberByYear(accommodationId, year, month) > GetReservationsNumberByYear(accommodationId, year, mostOccupiedMonth))
                {
                    mostOccupiedMonth = month;
                }
            }
            return mostOccupiedMonth;
        }
    }
}
