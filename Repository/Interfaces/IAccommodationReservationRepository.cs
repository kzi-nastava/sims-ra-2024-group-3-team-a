using BookingApp.Model;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository.Interfaces
{
    public interface IAccommodationReservationRepository
    {
        public List<AccommodationReservation> GetAll();

        public AccommodationReservation Save(AccommodationReservation reservation);

        public void Delete(AccommodationReservation reservation);

        public AccommodationReservation Update(AccommodationReservation reservation);

        public AccommodationReservation GetById(int id);

        public List<AccommodationReservation> GetAllByAccommodationId(int id);

        public List<AccommodationReservation> GetAllByGuestId(int id);
        public List<AccommodationReservation> GetAllRatedByGuestId(int id);

        public double GetAverageRating(List<AccommodationReservation> finishedReservations);
    }
}
