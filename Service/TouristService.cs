using BookingApp.InjectorNameSpace;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Service
{
    public class TouristService
    {
        private ITouristRepository _touristRepository;

        public TouristService()
        {
            _touristRepository = Injector.CreateInstance<ITouristRepository>();
        }

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