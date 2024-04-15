using BookingApp.Model;
using BookingApp.Serializer;
using BookingApp.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository.Interfaces
{
    public interface IAccommodationReservationChangeRequestRepository
    {
        public List<AccommodationReservationChangeRequest> GetAll();

        public AccommodationReservationChangeRequest Save(AccommodationReservationChangeRequest changeRequest);

        public void Delete(AccommodationReservationChangeRequest changeRequest);

        public AccommodationReservationChangeRequest Delete(int id);

        public AccommodationReservationChangeRequest Update(AccommodationReservationChangeRequest changeRequest);

        public AccommodationReservationChangeRequest GetById(int id);
    }
}

