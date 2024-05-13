using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository.Interfaces
{
    public interface ISuperGuestRepository
    {
        public List<SuperGuest> GetAll();

        public SuperGuest Save(SuperGuest user);

        public void Delete(SuperGuest user);

        public SuperGuest Update(SuperGuest user);

        public SuperGuest GetById(int id);
        public SuperGuest GetByUserId(int id);
    }
}
