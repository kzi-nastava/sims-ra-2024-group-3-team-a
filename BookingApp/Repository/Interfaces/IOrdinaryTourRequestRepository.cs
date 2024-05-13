using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository.Interfaces
{
    public interface IOrdinaryTourRequestRepository
    {
        public List<OrdinaryTourRequest> GetAll();

        public OrdinaryTourRequest GetById(int id);

        public OrdinaryTourRequest Save(OrdinaryTourRequest ordinaryTourRequest);

        public void Delete(OrdinaryTourRequest ordinaryTourRequest);

        public OrdinaryTourRequest Update(OrdinaryTourRequest ordinaryTourRequest);
    }
}
