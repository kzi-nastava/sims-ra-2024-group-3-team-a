using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository.Interfaces
{
    public interface IAccommodationRenovationRepository
    {
        public List<AccommodationRenovation> GetAll();

        public AccommodationRenovation Save(AccommodationRenovation renovation);

        public void Delete(AccommodationRenovation renovation);

        public AccommodationRenovation Update(AccommodationRenovation renovation);

        public AccommodationRenovation GetById(int id);
    }
}
