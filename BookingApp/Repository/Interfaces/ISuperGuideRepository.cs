using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository.Interfaces
{
    interface ISuperGuideRepository
    {
        public List<SuperGuide> GetAll();
        public int NextId();
        public SuperGuide Save(SuperGuide user);

        public void Delete(SuperGuide user);

        public SuperGuide Update(SuperGuide user);
    }
}
