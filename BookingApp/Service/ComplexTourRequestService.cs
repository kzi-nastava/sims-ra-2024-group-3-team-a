﻿using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
       /* public List<ComplexTourRequest> GetAllForUser(int userId) 
        {
            List<ComplexTourRequest> complexTourRequests = new List<ComplexTourRequest>();
            for (ComplexTourRequest  c in GetAll())
            {
                if()
            }

        }*/
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
    }
}
