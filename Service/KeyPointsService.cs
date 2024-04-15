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
    public class KeyPointsService
    {
        private IKeyPointsRepository _keypointsRepository;

        public KeyPointsService() 
        {
            _keypointsRepository = Injector.CreateInstance<IKeyPointsRepository>();
        }

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
            return _keypointsRepository.GetAll().FirstOrDefault(k => k.Id == tour.KeyPoints.Id);
        }
    }
}
