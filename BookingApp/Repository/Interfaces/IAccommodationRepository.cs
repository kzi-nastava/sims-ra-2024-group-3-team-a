using BookingApp.Model;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository.Interfaces
{
    public interface IAccommodationRepository
    {
        public List<Accommodation> GetAll();

        public Accommodation Save(Accommodation accomodation);

        public void Delete(Accommodation accomodation);

        public Accommodation Update(Accommodation accomodation);

        public Accommodation GetById(int id);

        public Accommodation GetByName(string name);

        public List<Accommodation> GetByOwner(int ownerId);
      
    }
}
