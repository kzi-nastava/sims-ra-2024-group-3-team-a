using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Service
{
    public class AccommodationService
    {
        private AccommodationRepository _accommodationRepository = new AccommodationRepository();

        public List<Accommodation> GetAll()
        {
            return _accommodationRepository.GetAll();
        }

        public Accommodation Save(Accommodation accommodation)
        {
            return _accommodationRepository.Save(accommodation);
        }

        public void Delete(Accommodation accommodation)
        {
            _accommodationRepository.Delete(accommodation);
        }

        public Accommodation Update(Accommodation accommodation)
        {
            return _accommodationRepository.Update(accommodation);
        }

        public Accommodation GetById(int id)
        {
            return _accommodationRepository.GetById(id);
        }

        public Accommodation GetByName(string name)
        {
            return _accommodationRepository.GetByName(name);
        }

        public List<Accommodation> GetAccommodationsForOwner(User owner)
        {
            return _accommodationRepository.GetByOwner(owner.Id);
        }
    }
}
