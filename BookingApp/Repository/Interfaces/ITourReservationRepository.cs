using BookingApp.Model;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository.Interfaces
{
    public interface ITourReservationRepository
    {
        public List<TourReservation> GetAll();

        public TourReservation GetById(int id);

        public TourReservation Save(TourReservation tourReservation);

        public int NextId();

        public void Delete(TourReservation tourReservation);

        public TourReservation Update(TourReservation tourReservation);
    }
}
