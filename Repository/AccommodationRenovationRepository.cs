using BookingApp.Model;
using BookingApp.Repository.Interfaces;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository
{
    public class AccommodationRenovationRepository : IAccommodationRenovationRepository
    {
        private const string FilePath = "../../../Resources/Data/acommodationRenovations.csv";

        private readonly Serializer<AccommodationRenovation> _serializer;

        private List<AccommodationRenovation> _accommodationRenovations;

        public AccommodationRenovationRepository()
        {
            _serializer = new Serializer<AccommodationRenovation>();
            _accommodationRenovations = _serializer.FromCSV(FilePath);
        }

        public List<AccommodationRenovation> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public AccommodationRenovation Save(AccommodationRenovation renovation)
        {
            renovation.Id = NextId();
            _accommodationRenovations = _serializer.FromCSV(FilePath);
            _accommodationRenovations.Add(renovation);
            _serializer.ToCSV(FilePath, _accommodationRenovations);
            return renovation;
        }

        private int NextId()
        {
            _accommodationRenovations = _serializer.FromCSV(FilePath);
            if (_accommodationRenovations.Count < 1)
            {
                return 1;
            }
            return _accommodationRenovations.Max(c => c.Id) + 1;
        }

        public AccommodationRenovation Update(AccommodationRenovation renovation)
        {
            _accommodationRenovations = _serializer.FromCSV(FilePath);
            AccommodationRenovation current = _accommodationRenovations.Find(c => c.Id == renovation.Id);
            int index = _accommodationRenovations.IndexOf(current);
            _accommodationRenovations.Remove(current);
            _accommodationRenovations.Insert(index, renovation);
            _serializer.ToCSV(FilePath, _accommodationRenovations);
            return renovation;
        }

        public void Delete(AccommodationRenovation renovation)
        {
            _accommodationRenovations = _serializer.FromCSV(FilePath);
            AccommodationRenovation founded = _accommodationRenovations.Find(c => c.Id == renovation.Id);
            _accommodationRenovations.Remove(founded);
            _serializer.ToCSV(FilePath, _accommodationRenovations);
        }

        public AccommodationRenovation GetById(int id)
        {
            _accommodationRenovations = _serializer.FromCSV(FilePath);
            return _accommodationRenovations.Find(c => c.Id == id);
        }
    }
}
