using BookingApp.Model;
using BookingApp.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Service
{
    public class OrdinaryTourRequestService
    {

        private IOrdinaryTourRequestRepository _ordinaryTourRequestRepository;

        public OrdinaryTourRequestService(IOrdinaryTourRequestRepository ordinaryTourRequestRepository)
        {
            _ordinaryTourRequestRepository = ordinaryTourRequestRepository;
        }

        public List<OrdinaryTourRequest> GetAll()
        {
            return _ordinaryTourRequestRepository.GetAll();
        }

        public OrdinaryTourRequest Save(OrdinaryTourRequest ordinaryTourRequestRepository)
        {
            return _ordinaryTourRequestRepository.Save(ordinaryTourRequestRepository);
        }

        public void Delete(OrdinaryTourRequest ordinaryTourRequestRepository)
        {
            _ordinaryTourRequestRepository.Delete(ordinaryTourRequestRepository);
        }

        public OrdinaryTourRequest Update(OrdinaryTourRequest ordinaryTourRequestRepository)
        {
            return _ordinaryTourRequestRepository.Update(ordinaryTourRequestRepository);
        }
        public OrdinaryTourRequest GetById(int id)
        {
            return _ordinaryTourRequestRepository.GetById(id);
        }
        public List<OrdinaryTourRequest> GetAllForUser(int userId) 
        {
            return _ordinaryTourRequestRepository.GetAll().Where(ordinaryTourRequest => ordinaryTourRequest.UserId == userId).ToList();
        }
    }
}
