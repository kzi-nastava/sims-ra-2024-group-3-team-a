using BookingApp.Model;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository
{
    public class TouristRepository
    {
        private const string FilePath = "../../../Resources/Data/tourists.csv";

        private readonly Serializer<Tourist> _serializer;

        private List<Tourist> _tourists;

        public TouristRepository()
        {
            _serializer = new Serializer<Tourist>();
            _tourists = _serializer.FromCSV(FilePath);
        }

        public List<Tourist> GetAll()
        {
            _tourists = _serializer.FromCSV(FilePath);
            return _tourists;
        }

        public Tourist Save(Tourist tourist)
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

        public void Delete(Tourist tourist)
        {
            _tourists = _serializer.FromCSV(FilePath);
            _tourists.Remove(tourist);
            _serializer.ToCSV(FilePath, _tourists);
        }

        public Tourist Update(Tourist tourist)
        {
            _tourists = _serializer.FromCSV(FilePath);
            Tourist current = _tourists.Find(c => c.Name == tourist.Name && c.JoiningKeyPoint==tourist.JoiningKeyPoint);
            int index = _tourists.IndexOf(current);
            _tourists.Remove(current);
            _tourists.Insert(index, tourist);
            _serializer.ToCSV(FilePath, _tourists);
            return tourist;
        }
    }
}
