using BookingApp.DTO;
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
    public class TourReviewService
    {
        private ITourReviewRepository _tourReviewRepository;

        public TourReviewService()
        {
            _tourReviewRepository = Injector.CreateInstance<ITourReviewRepository>();
        }

        public List<TourReview> GetAll()
        {
            return _tourReviewRepository.GetAll();
        }

        public TourReview Save(TourReview tourReview)
        {
            return _tourReviewRepository.Save(tourReview);
        }

        public TourReview Update(TourReview tourReview)
        {
            return _tourReviewRepository.Update(tourReview);
        }
        public TourReview GetById(int id)
        {
            return _tourReviewRepository.GetById(id);
        }
        public bool IsTourRated(Tour tour, User user)
        {
            foreach (TourReview tourReview in GetAll())
            {
                if (tour.Id == tourReview.TourId)
                {
                    if (user.Id == tourReview.TouristId)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
