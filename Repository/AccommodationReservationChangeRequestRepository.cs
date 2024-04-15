using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Service;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository
{
    public class AccommodationReservationChangeRequestRepository
    {
        private const string FilePath = "../../../Resources/Data/accommodationReservationChangeRequests.csv";

        private readonly Serializer<AccommodationReservationChangeRequest> _serializer;

        private List<AccommodationReservationChangeRequest> _accomodationReservationChangeRequests;

        private AccommodationReservationService _accommodationReservationService;

        public AccommodationReservationChangeRequestRepository()
        {
            _serializer = new Serializer<AccommodationReservationChangeRequest>();
            _accomodationReservationChangeRequests = _serializer.FromCSV(FilePath);
            _accommodationReservationService = new AccommodationReservationService();  
        }

        public List<AccommodationReservationChangeRequest> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public AccommodationReservationChangeRequest Save(AccommodationReservationChangeRequest changeRequest)
        {
            changeRequest.Id = NextId();
            _accomodationReservationChangeRequests = _serializer.FromCSV(FilePath);
            _accomodationReservationChangeRequests.Add(changeRequest);
            _serializer.ToCSV(FilePath, _accomodationReservationChangeRequests);
            return changeRequest;
        }

        private int NextId()
        {
            _accomodationReservationChangeRequests = _serializer.FromCSV(FilePath);
            if (_accomodationReservationChangeRequests.Count < 1)
            {
                return 1;
            }
            return _accomodationReservationChangeRequests.Max(c => c.Id) + 1;
        }

        public void Delete(AccommodationReservationChangeRequest changeRequest)
        {
            _accomodationReservationChangeRequests = _serializer.FromCSV(FilePath);
            AccommodationReservationChangeRequest founded = _accomodationReservationChangeRequests.Find(c => c.Id == changeRequest.Id);
            _accomodationReservationChangeRequests.Remove(founded);
            _serializer.ToCSV(FilePath, _accomodationReservationChangeRequests);
        }

        public AccommodationReservationChangeRequest Delete(int id)
        {
            _accomodationReservationChangeRequests = _serializer.FromCSV(FilePath);
            AccommodationReservationChangeRequest founded = _accomodationReservationChangeRequests.Find(c => c.Id == id);
            _accomodationReservationChangeRequests.Remove(founded);
            _serializer.ToCSV(FilePath, _accomodationReservationChangeRequests);
            return founded;
        }

        public AccommodationReservationChangeRequest Update(AccommodationReservationChangeRequest changeRequest)
        {
            _accomodationReservationChangeRequests = _serializer.FromCSV(FilePath);
            AccommodationReservationChangeRequest current = _accomodationReservationChangeRequests.Find(c => c.Id == changeRequest.Id);
            int index = _accomodationReservationChangeRequests.IndexOf(current);
            _accomodationReservationChangeRequests.Remove(current);
            _accomodationReservationChangeRequests.Insert(index, changeRequest);
            _serializer.ToCSV(FilePath, _accomodationReservationChangeRequests);
            return changeRequest;
        }

        public AccommodationReservationChangeRequest GetById(int id)
        {
            _accomodationReservationChangeRequests = _serializer.FromCSV(FilePath);
            return _accomodationReservationChangeRequests.FirstOrDefault(c => c.Id == id);
        }

        public List<AccommodationReservationChangeRequest> GetAllByGuestId(int guestId)
        {
            List<AccommodationReservationChangeRequest> _myRequests = new List<AccommodationReservationChangeRequest>();
            foreach (var request in GetAll())
            {
                foreach (var reservation in _accommodationReservationService.GetAllByGuestId(guestId))
                {
                    if (request.AccommodationReservationId == reservation.Id)
                    {
                        _myRequests.Add(request);
                    }
                }
            }
            return _myRequests;
        }
    }
}
