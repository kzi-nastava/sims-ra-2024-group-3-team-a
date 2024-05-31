using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BookingApp.Service
{
    public class ComplexTourRequestService
    {
        private IComplexTourRequestRepository _complexTourRequestRepository;
        private IOrdinaryTourRequestRepository _ordinaryTourRequestRepository;
        private ITourRepository _tourRepository;
        private IMessageRepository _messageRepository;
        private ITouristRepository _touristRepository;
        private IUserRepository _userRepository;
        private ITourReservationRepository _tourReservationRepository;
        private ITourReviewRepository _tourReviewRepository;
        private IVoucherRepository _voucherRepository;
        private IAccommodationReservationChangeRequestRepository _accommodationReservationChangeRequestRepository;
        private IAccommodationReservationRepository _accommodationReservationRepository;
        private IAccommodationRepository _accommodationRepository;
        private OrdinaryTourRequestService _ordinaryTourRequestService;
        private MessageService _messageService;

        public ComplexTourRequestService(IAccommodationReservationChangeRequestRepository accommodationReservationChangeRequestRepository, IAccommodationReservationRepository accommodationReservationRepository, IAccommodationRepository accommodationRepository, IOrdinaryTourRequestRepository ordinaryTourRequestRepository, ITourRepository tourRepository, IMessageRepository messageRepository, ITouristRepository touristRepository, IUserRepository userRepository, ITourReservationRepository tourReservationRepository, ITourReviewRepository tourReviewRepository, IVoucherRepository voucherRepository, IComplexTourRequestRepository complexTourRequestRepository)
        {
            _complexTourRequestRepository = complexTourRequestRepository;
            _ordinaryTourRequestService = new OrdinaryTourRequestService(accommodationReservationChangeRequestRepository, accommodationReservationRepository, accommodationRepository, ordinaryTourRequestRepository, tourRepository, messageRepository, touristRepository, userRepository, tourReservationRepository, tourReviewRepository, voucherRepository);
            _tourRepository = tourRepository;
            _touristRepository = touristRepository;
            _userRepository = userRepository;
            _tourReservationRepository = tourReservationRepository;
            _tourReviewRepository = tourReviewRepository;
            _voucherRepository = voucherRepository;
            _accommodationReservationChangeRequestRepository = accommodationReservationChangeRequestRepository;
            _accommodationRepository = accommodationRepository;
            _accommodationReservationRepository = accommodationReservationRepository;
            _messageService = new MessageService(messageRepository, accommodationReservationChangeRequestRepository, accommodationReservationRepository, accommodationRepository, userRepository, tourRepository, tourReservationRepository, touristRepository, tourReviewRepository, voucherRepository);


        }
        public List<ComplexTourRequest> GetAll()
        {
            return _complexTourRequestRepository.GetAll();
        }
        public ComplexTourRequest Save(ComplexTourRequest complexTourRequestRepository)
        {
            return _complexTourRequestRepository.Save(complexTourRequestRepository);
        }
        public void Delete(ComplexTourRequest complexTourRequestRepository)
        {
            _complexTourRequestRepository.Delete(complexTourRequestRepository);
        }

        public ComplexTourRequest Update(ComplexTourRequest complexTourRequestRepository)
        {
            return _complexTourRequestRepository.Update(complexTourRequestRepository);
        }
        public ComplexTourRequest GetById(int id)
        {
            return _complexTourRequestRepository.GetById(id);
        }
        public List<ComplexTourRequest> GetAllForUser(int userId) 
        {
            List<ComplexTourRequest> complexTourRequests = new List<ComplexTourRequest>();
            foreach (ComplexTourRequest complexTourRequest in GetAll())
            {
               
                if (getOrdinaryTourRequestsForUser(complexTourRequest.Id, userId).Count!=0)
                {
                    complexTourRequests.Add(complexTourRequest);
                } 
            }
            return complexTourRequests;
        }
        public List<OrdinaryTourRequest> getOrdinaryTourRequestsForUser(int complexTourRequestId, int userId)
        {
            List<OrdinaryTourRequest> ordinaryTourRequests = new List<OrdinaryTourRequest> ();
          

            foreach (OrdinaryTourRequest ordinaryTourRequest in _ordinaryTourRequestService.GetAllForUser(userId))
            {
                if(ordinaryTourRequest.ComplexTourRequestId == complexTourRequestId)
                    ordinaryTourRequests.Add(ordinaryTourRequest);
            }
            return ordinaryTourRequests;
        }
        public List<OrdinaryTourRequest> getOrdinaryTourRequests(int complexTourRequestId)
        {
            List<OrdinaryTourRequest> ordinaryTourRequests = new List<OrdinaryTourRequest>();


            foreach (OrdinaryTourRequest ordinaryTourRequest in _ordinaryTourRequestService.GetAll())
            {
                if (ordinaryTourRequest.ComplexTourRequestId == complexTourRequestId)
                    ordinaryTourRequests.Add(ordinaryTourRequest);
            }
            return ordinaryTourRequests;
        }

        public void CheckForInvalidComplexTourRequests(int userId)
        {
            foreach(ComplexTourRequest complexTourRequest in GetAll())
            {
                List<OrdinaryTourRequest> ordinaryTourRequests = new List<OrdinaryTourRequest>();
                foreach(OrdinaryTourRequest ordinaryTourRequest in getOrdinaryTourRequestsForUser(complexTourRequest.Id, userId))
                {
                    ordinaryTourRequests = getOrdinaryTourRequestsForUser(complexTourRequest.Id, userId);
                    if(Is48hoursbeforeFirstOrdinaryTour(ordinaryTourRequests))
                    {
                        complexTourRequest.Status = Model.Enums.TourRequestStatus.Invalid;
                        Update(complexTourRequest);
                    }
                }
            }
        }
        public bool Is48hoursbeforeFirstOrdinaryTour(List<OrdinaryTourRequest> ordinaryTourRequests)
        {
            if (ordinaryTourRequests == null || ordinaryTourRequests.Count == 0)
                return false;

            DateTime minBeginDate = ordinaryTourRequests.Min(request => request.BeginDate);
            DateTime now = DateTime.Now;
            TimeSpan difference = minBeginDate - now;
            bool is48HoursBefore = difference.TotalHours <= 48;

            return is48HoursBefore;
        }
    }
}
