using BookingApp.Model;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository
{
    public class TourReviewRepository
    {
        private const string FilePath = "../../../Resources/Data/tourReviews.csv";

        private readonly Serializer<TourReview> _serializer;

        private List<TourReview> _tourReview;

        public TourReviewRepository()
        {
            _serializer = new Serializer<TourReview>();
            _tourReview = _serializer.FromCSV(FilePath);
        }
        public List<TourReview> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public TourReview GetById(int id)
        {
            _tourReview = _serializer.FromCSV(FilePath);
            return _tourReview.FirstOrDefault(u => u.Id == id);
        }

        public TourReview Save(TourReview tourReview)
        {
            tourReview.Id = NextId();
            _tourReview = _serializer.FromCSV(FilePath);
            _tourReview.Add(tourReview);
            _serializer.ToCSV(FilePath, _tourReview);
            return tourReview;
        }

        public int NextId()
        {
            _tourReview = _serializer.FromCSV(FilePath);
            if (_tourReview.Count < 1)
            {
                return 1;
            }
            return _tourReview.Max(c => c.Id) + 1;
        }

        public TourReview Update(TourReview tourReview)
        {
            _tourReview = _serializer.FromCSV(FilePath);
            TourReview current = _tourReview.Find(c => c.Id == tourReview.Id);
            int index = _tourReview.IndexOf(current);
            _tourReview.Remove(current);
            _tourReview.Insert(index, tourReview);       // keep ascending order of ids in file 
            _serializer.ToCSV(FilePath, _tourReview);
            return tourReview;
        }
    }
}

