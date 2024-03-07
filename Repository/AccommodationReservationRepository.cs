using BookingApp.Model;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository
{
    public class AccommodationReservationRepository
    {
        private const string FilePath = "../../../Resources/Data/accommodationReservations.csv";

        private readonly Serializer<Accommodation> _serializer;

        private List<Accommodation> _accomodationReservations;

        public AccommodationReservationRepository()
        {
            _serializer = new Serializer<Accommodation>();
            _accomodationReservations = _serializer.FromCSV(FilePath);
        }

        public List<Accommodation> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public Accommodation Save(Accommodation accomodation)
        {
            accomodation.Id = NextId();
            _accomodationReservations = _serializer.FromCSV(FilePath);
            _accomodationReservations.Add(accomodation);
            _serializer.ToCSV(FilePath, _accomodationReservations);
            return accomodation;
        }

        private int NextId()
        {
            _accomodationReservations = _serializer.FromCSV(FilePath);
            if (_accomodationReservations.Count < 1)
            {
                return 1;
            }
            return _accomodationReservations.Max(c => c.Id) + 1;
        }

        public void Delete(Accommodation accomodation)
        {
            _accomodationReservations = _serializer.FromCSV(FilePath);
            Accommodation founded = _accomodationReservations.Find(c => c.Id == accomodation.Id);
            _accomodationReservations.Remove(founded);
            _serializer.ToCSV(FilePath, _accomodationReservations);
        }

        public Accommodation Update(Accommodation accomodation)
        {
            _accomodationReservations = _serializer.FromCSV(FilePath);
            Accommodation current = _accomodationReservations.Find(c => c.Id == accomodation.Id);
            int index = _accomodationReservations.IndexOf(current);
            _accomodationReservations.Remove(current);
            _accomodationReservations.Insert(index, accomodation);
            _serializer.ToCSV(FilePath, _accomodationReservations);
            return accomodation;
        }
    }
}
