using BookingApp.DTO;
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

        private readonly Serializer<AccommodationReservation> _serializer;

        private List<AccommodationReservation> _accomodationReservations;

        public AccommodationReservationRepository()
        {
            _serializer = new Serializer<AccommodationReservation>();
            _accomodationReservations = _serializer.FromCSV(FilePath);
        }

        public List<AccommodationReservation> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public AccommodationReservation Save(AccommodationReservation reservation)
        {
            reservation.Id = NextId();
            _accomodationReservations = _serializer.FromCSV(FilePath);
            _accomodationReservations.Add(reservation);
            _serializer.ToCSV(FilePath, _accomodationReservations);
            return reservation;
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

        public void Delete(AccommodationReservation reservation)
        {
            _accomodationReservations = _serializer.FromCSV(FilePath);
            AccommodationReservation founded = _accomodationReservations.Find(c => c.Id == reservation.Id);
            _accomodationReservations.Remove(founded);
            _serializer.ToCSV(FilePath, _accomodationReservations);
        }

        public AccommodationReservation Update(AccommodationReservation reservation)
        {
            _accomodationReservations = _serializer.FromCSV(FilePath);
            AccommodationReservation current = _accomodationReservations.Find(c => c.Id == reservation.Id);
            int index = _accomodationReservations.IndexOf(current);
            _accomodationReservations.Remove(current);
            _accomodationReservations.Insert(index, reservation);
            _serializer.ToCSV(FilePath, _accomodationReservations);
            return reservation;
        }

        public AccommodationReservation GetById(int id)
        {
            _accomodationReservations = _serializer.FromCSV(FilePath);
            return _accomodationReservations.FirstOrDefault(c => c.Id == id);
        }

        public List<AccommodationReservation> GetAllByAccommodationId(int id)
        {
            return _accomodationReservations.FindAll(c => c.AccommodationId == id);
        }

        public double GetAverageRating(List<AccommodationReservationDTO> finishedReservations)
        {
            int ratingSum = 0;
            int ratingCount = 0;
            foreach(var reservation in finishedReservations)
            {
                if(reservation.RatingDTO.GuestCleanlinessRating != 0)
                {
                    ratingCount++;
                    int rating = (reservation.RatingDTO.GuestCleanlinessRating + reservation.RatingDTO.GuestHospitalityRating) / 2;
                    ratingSum += rating;
                }
            }
            return (double)ratingSum / (double)ratingCount;
        }
    }
}
