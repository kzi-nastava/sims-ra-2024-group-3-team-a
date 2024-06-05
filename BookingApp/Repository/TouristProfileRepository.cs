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
    public class TouristProfileRepository: ITouristProfileRepository
    {
        private const string FilePath = "../../../Resources/Data/touristProfiles.csv";

        private readonly Serializer<TouristProfile> _serializer;

        private List<TouristProfile> _tourists;

        public TouristProfileRepository()
        {
            _serializer = new Serializer<TouristProfile>();
            _tourists = _serializer.FromCSV(FilePath);
        }

        public List<TouristProfile> GetAll()
        {
            _tourists = _serializer.FromCSV(FilePath);
            return _tourists;
        }

        public TouristProfile Save(TouristProfile tourist)
        {
            tourist.Id = NextId();
            _tourists = _serializer.FromCSV(FilePath);
            _tourists.Add(tourist);
            _serializer.ToCSV(FilePath, _tourists);
            return tourist;

        }
        public int NextId()
        {
            _tourists = _serializer.FromCSV(FilePath);
            if (_tourists.Count < 1)
            {
                return 1;
            }
            return _tourists.Max(c => c.Id) + 1;
        }

        public void Delete(TouristProfile tourist)
        {
            _tourists = _serializer.FromCSV(FilePath);
            _tourists.Remove(tourist);
            _serializer.ToCSV(FilePath, _tourists);
        }
        public TouristProfile Update(TouristProfile tour)
        {
            _tourists = _serializer.FromCSV(FilePath);
            TouristProfile current = _tourists.Find(c => c.Id == tour.Id);
            int index = _tourists.IndexOf(current);
            _tourists.Remove(current);
            _tourists.Insert(index, tour);
            _serializer.ToCSV(FilePath, _tourists);
            return tour;
        }

        public TouristProfile GetById(int id)
        {
            _tourists = _serializer.FromCSV(FilePath);
            return _tourists.FirstOrDefault(u => u.Id == id);
        }
    }
}
