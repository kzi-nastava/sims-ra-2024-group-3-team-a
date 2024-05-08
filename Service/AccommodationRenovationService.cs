using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Service
{
    public class AccommodationRenovationService
    {
        private IAccommodationRenovationRepository _accommodationRenovationRepository;
        private IAccommodationReservationRepository _accommodationReservationRepository;
        private AccommodationService _accommodationService; 

        public AccommodationRenovationService(IAccommodationRenovationRepository accommodationRenovationRepository, IAccommodationReservationRepository accommodationReservationRepository, IAccommodationRepository accommodationRepository)
        {
            _accommodationRenovationRepository = accommodationRenovationRepository;
            _accommodationReservationRepository = accommodationReservationRepository;
            _accommodationService = new AccommodationService(accommodationRepository);
        }

        public List<AccommodationRenovation> GetAll()
        {
            return _accommodationRenovationRepository.GetAll();
        }

        public AccommodationRenovation Save(AccommodationRenovation renovation)
        {
            return _accommodationRenovationRepository.Save(renovation);
        }

        public void Delete(AccommodationRenovation renovation)
        {
            _accommodationRenovationRepository.Delete(renovation);
        }

        public AccommodationRenovation Update(AccommodationRenovation renovation)
        {
            return _accommodationRenovationRepository.Update(renovation);
        }

        public AccommodationRenovation GetById(int id)
        {
            return _accommodationRenovationRepository.GetById(id);
        }
        public List<DateTime> FindAvailableDates(int accommodationId, DateTime fromDate, DateTime toDate, int length)
        {
            List<AccommodationReservation> reservations = _accommodationReservationRepository.GetAll().Where(r => r.AccommodationId == accommodationId).ToList();
            List<DateTime> notAvailableDates = new List<DateTime>();
            foreach(AccommodationReservation reservation in reservations)
            {
                for(DateTime date = reservation.BeginDate.ToDateTime(new TimeOnly(0,0,0)); date <= reservation.EndDate.ToDateTime(new TimeOnly(0, 0, 0)); date = date.AddDays(1))
                {
                    notAvailableDates.Add(date);
                }
            }

            List<DateTime> availableDates = new List<DateTime>();
            int len = 0;
            for(DateTime i = fromDate; i <= toDate.AddDays(-length); i = i.AddDays(1))
            { 
                for(DateTime j = i; j <= i.AddDays(5); j = j.AddDays(1))
                {
                    if(!notAvailableDates.Contains(j))
                    {
                        len++;
                        if(len == length)
                        {
                            availableDates.Add(i);
                            len = 0;
                        }
                    }
                    else
                    {
                        len = 0;
                        continue;       
                    }
                }
            }

            return availableDates;
        }
        public List<AccommodationRenovation> GetRenovationsForOwner(User loggedInUser)
        {
            List<Accommodation> accommodations = _accommodationService.GetAccommodationsForOwner(loggedInUser);

            List<AccommodationRenovation> renovations = new List<AccommodationRenovation>();
            foreach(AccommodationRenovation renovation in GetAll())
            {
                foreach(Accommodation accommodation in accommodations)
                {
                    if(renovation.AccommodationId == accommodation.Id)
                    {
                        renovations.Add(renovation);
                    }
                }
            }

            return renovations;
        }
    }
}
