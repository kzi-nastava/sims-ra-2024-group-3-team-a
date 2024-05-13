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
    public class KeyPointService
    {
        private IKeyPointRepository _keypointsRepository;

        public KeyPointService(IKeyPointRepository keyPointRepository)
        {
            _keypointsRepository = keyPointRepository;
        }
        public List<KeyPoint> GetAll()
        {
            return _keypointsRepository.GetAll();
        }

        public KeyPoint Save(KeyPoint keyPoints)
        {
            return _keypointsRepository.Save(keyPoints);
        }
        public KeyPoint GetByName(string name)
        {
            return _keypointsRepository.GetByName(name);
        }

        public void Delete(KeyPoint keyPoints)
        {
            _keypointsRepository.Delete(keyPoints);
        }

        public KeyPoint Update(KeyPoint keyPoints)
        {
            return _keypointsRepository.Update(keyPoints);
        }
        public List<KeyPoint> GetKeyPointsForTour(Tour tour)
        {
            return _keypointsRepository.GetAll().Where(k => k.TourId == tour.Id).ToList();
        }
    }
}
