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
    public class AccommodationReservationChangeRequestService
    {
        private IAccommodationReservationChangeRequestRepository _accommodationReservationChangeRequestRepository;

        private AccommodationReservationService _accommodationReservationService;

        public AccommodationReservationChangeRequestService(IAccommodationReservationChangeRequestRepository accommodationReservationChangeRequestRepository, IAccommodationReservationRepository accommodationReservationRepository, IAccommodationRepository accommodationRepository, IUserRepository userRepository)
        {
            _accommodationReservationChangeRequestRepository = accommodationReservationChangeRequestRepository;
            _accommodationReservationService = new AccommodationReservationService(accommodationReservationRepository, accommodationRepository, userRepository);
        }

        public List<AccommodationReservationChangeRequest> GetAll()
        {
            return _accommodationReservationChangeRequestRepository.GetAll();
        }

        public AccommodationReservationChangeRequest Save(AccommodationReservationChangeRequest accommodationReservationChangeRequest)
        {
            return _accommodationReservationChangeRequestRepository.Save(accommodationReservationChangeRequest);
        }

        public void Delete(AccommodationReservationChangeRequest accommodationReservationChangeRequest)
        {
            _accommodationReservationChangeRequestRepository.Delete(accommodationReservationChangeRequest);
        }

        public AccommodationReservationChangeRequest Update(AccommodationReservationChangeRequest accommodationReservationChangeRequest)
        {
            return _accommodationReservationChangeRequestRepository.Update(accommodationReservationChangeRequest);
        }

        public AccommodationReservationChangeRequest GetById(int id)
        {
            return _accommodationReservationChangeRequestRepository.GetById(id);
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
