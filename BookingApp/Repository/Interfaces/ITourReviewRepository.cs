using BookingApp.Model;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository.Interfaces
{
    public interface ITourReviewRepository
    {
        public List<TourReview> GetAll();

        public TourReview GetById(int id);

        public TourReview Save(TourReview tourReview);

        public int NextId();

        public TourReview Update(TourReview tourReview);
    }
}
