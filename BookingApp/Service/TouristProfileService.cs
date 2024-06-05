using BookingApp.Model;
using BookingApp.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Service
{
    public class TouristProfileService
    {

        private ITouristProfileRepository _touristProfileRepository;

        public TouristProfileService(ITouristProfileRepository touristProfileRepository)
        {
            _touristProfileRepository = touristProfileRepository;
        }

        public List<TouristProfile> GetAll()
        {
            return _touristProfileRepository.GetAll();
        }

        public TouristProfile Save(TouristProfile tourist)
        {
            return _touristProfileRepository.Save(tourist);
        }

        public void Delete(TouristProfile tourist)
        {
            _touristProfileRepository.Delete(tourist);
        }

        public TouristProfile Update(TouristProfile tourist)
        {
            return _touristProfileRepository.Update(tourist);
        }
        public TouristProfile GetById(int id)
        {
            return _touristProfileRepository.GetById(id);
        }



    }
}
