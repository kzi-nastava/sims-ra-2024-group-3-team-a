using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Service
{
    public class AccommodationReservationChangeRequestService
    {
        private AccommodationReservationChangeRequestRepository _accommodationReservationChangeRequestRepository = new AccommodationReservationChangeRequestRepository();

        public List<AccommodationReservationChangeRequest> GetAll()
        {
            return _accommodationReservationChangeRequestRepository.GetAll();
        }

        public AccommodationReservationChangeRequest Save(AccommodationReservationChangeRequest accommodationReservationChangeRequest)
        {
            return _accommodationReservationChangeRequestRepository.Save(accommodationReservationChangeRequest);
        }

        public void Delete(AccommodationReservationChangeRequest accommodationReservationChangeRequest)
        {
            _accommodationReservationChangeRequestRepository.Delete(accommodationReservationChangeRequest);
        }

        public AccommodationReservationChangeRequest Update(AccommodationReservationChangeRequest accommodationReservationChangeRequest)
        {
            return _accommodationReservationChangeRequestRepository.Update(accommodationReservationChangeRequest);
        }

        public AccommodationReservationChangeRequest GetById(int id)
        {
            return _accommodationReservationChangeRequestRepository.GetById(id);
        }

        public List<AccommodationReservationChangeRequest> GetAllByGuestId(int guestId)
        {
            return _accommodationReservationChangeRequestRepository.GetAllByGuestId(guestId);
        }
    }
}
