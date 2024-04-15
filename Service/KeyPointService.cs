using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Service
{
    public class KeyPointService
    {

        private KeyPointsRepository _keypointsRepository = new KeyPointsRepository();

        public List<KeyPoints> GetAll()
        {
            return _keypointsRepository.GetAll();
        }

        public KeyPoints Save(KeyPoints keyPoints)
        {
            return _keypointsRepository.Save(keyPoints);
        }

        public void Delete(KeyPoints keyPoints)
        {
            _keypointsRepository.Delete(keyPoints);
        }

        public KeyPoints Update(KeyPoints keyPoints)
        {
            return _keypointsRepository.Update(keyPoints);
        }
        public KeyPoints GetKeyPointsForTour(Tour tour)
        {
            return _keypointsRepository.GetAll().FirstOrDefault(k => k.Id == tour.KeyPoint.Id);
        }
    }
}
