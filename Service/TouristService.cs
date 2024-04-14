using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Service
{
    public class TouristService
    {
        private TouristRepository _touristRepository = new TouristRepository();

        //private TourReservationService _tourReservationService = new TourReservationService();

        public List<Tourist> GetAll()
        {
            return _touristRepository.GetAll();
        }

        public Tourist Save(Tourist tourist)
        {
            return _touristRepository.Save(tourist);
        }

        public void Delete(Tourist tourist)
        {
            _touristRepository.Delete(tourist);
        }

        public Tourist Update(Tourist tourist)
        {
            return _touristRepository.Update(tourist);
        }
        public Tourist GetById(int id)
        {
            return _touristRepository.GetById(id);
        }
    }
}