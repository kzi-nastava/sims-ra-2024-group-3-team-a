using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository.Interfaces
{
    public interface ITouristProfileRepository
    {
        public List<TouristProfile> GetAll();

        public TouristProfile Save(TouristProfile tourist);

        public int NextId();

        public void Delete(TouristProfile tourist);

        public TouristProfile Update(TouristProfile tourist);

        public TouristProfile GetById(int id);
    }
}
