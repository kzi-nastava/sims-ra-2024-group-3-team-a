using BookingApp.Model;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BookingApp.Repository
{
    public class AccommodationRepository
    {
        private const string FilePath = "../../../Resources/Data/accommodations.csv";

        private readonly Serializer<Accommodation> _serializer;

        private List<Accommodation> _accomodations;

        public AccommodationRepository()
        {
            _serializer = new Serializer<Accommodation>();
            _accomodations = _serializer.FromCSV(FilePath);
        }

        public List<Accommodation> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public Accommodation Save(Accommodation accomodation)
        {
            accomodation.Id = NextId();
            _accomodations = _serializer.FromCSV(FilePath);
            _accomodations.Add(accomodation);
            _serializer.ToCSV(FilePath, _accomodations);
            return accomodation;
        }

        private int NextId()
        {
            _accomodations = _serializer.FromCSV(FilePath);
            if (_accomodations.Count < 1)
            {
                return 1;
            }
            return _accomodations.Max(c => c.Id) + 1;
        }

        public void Delete(Accommodation accomodation)
        {
            _accomodations = _serializer.FromCSV(FilePath);
            Accommodation founded = _accomodations.Find(c => c.Id == accomodation.Id);
            _accomodations.Remove(founded);
            _serializer.ToCSV(FilePath, _accomodations);
        }

        public Accommodation Update(Accommodation accomodation)
        {
            _accomodations = _serializer.FromCSV(FilePath);
            Accommodation current = _accomodations.Find(c => c.Id == accomodation.Id);
            int index = _accomodations.IndexOf(current);
            _accomodations.Remove(current);
            _accomodations.Insert(index, accomodation);
            _serializer.ToCSV(FilePath, _accomodations);
            return accomodation;
        }

        public Accommodation GetById(int id)
        {
            _accomodations = _serializer.FromCSV(FilePath);
            return _accomodations.FirstOrDefault(c => c.Id == id);
        }

        public Accommodation GetByName(string name)
        {
            _accomodations = _serializer.FromCSV(FilePath);
            return _accomodations.FirstOrDefault(c => c.Name == name);
        }
        public List<Accommodation> GetByOwner(int ownerId)
        {
            _accomodations = _serializer.FromCSV(FilePath);
            return _accomodations.FindAll(c => c.OwnerId == ownerId);
        }
    }
}
