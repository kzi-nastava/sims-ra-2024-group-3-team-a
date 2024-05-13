using BookingApp.Model;
using BookingApp.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Service
{
    public class SuperGuestService
    {
        private ISuperGuestRepository _superGuestRepository;

        public SuperGuestService(ISuperGuestRepository superGuestRepository)
        {
            _superGuestRepository = superGuestRepository;
        }

        public SuperGuest GetById(int id)
        {
            return _superGuestRepository.GetById(id);
        }
        public SuperGuest GetByUserId(int id)
        {
            return _superGuestRepository.GetByUserId(id);
        }

        public List<SuperGuest> GetAll()
        {
            return _superGuestRepository.GetAll();
        }

        public SuperGuest Save(SuperGuest user)
        {
            return _superGuestRepository.Save(user);
        }

        public void Delete(SuperGuest user)
        {
            _superGuestRepository.Delete(user);
        }

        public SuperGuest Update(SuperGuest user)
        {
            return _superGuestRepository.Update(user);
        }
    }
}
