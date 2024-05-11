using BookingApp.InjectorNameSpace;
using BookingApp.Model;
using BookingApp.Model.Enums;
using BookingApp.Repository;
using BookingApp.Repository.Interfaces;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Service
{
    public class MessageService
    {
        private IMessageRepository _messageRepository;

        private AccommodationReservationService _accommodationReservationService;
        private UserService _userService;
        private AccommodationReservationChangeRequestService _accommodationReservationChangeRequestService;
        private AccommodationService _accommodationService;
        private TourService _tourService;
       // private OrdinaryTourRequestService _ordinaryTourRequestRepository;

        public MessageService(IMessageRepository messageRepository, IAccommodationReservationChangeRequestRepository accommodationReservationChangeRequestRepository, IAccommodationReservationRepository accommodationReservationRepository, IAccommodationRepository accommodationRepository, IUserRepository userRepository, ITourRepository tourRepository, ITourReservationRepository tourReservationRepository, ITouristRepository touristRepository, ITourReviewRepository tourReviewRepository, IVoucherRepository voucherRepository)
        {
            _messageRepository = messageRepository;
            _accommodationReservationChangeRequestService = new AccommodationReservationChangeRequestService(accommodationReservationChangeRequestRepository, accommodationReservationRepository, accommodationRepository, userRepository);
            _accommodationReservationService = new AccommodationReservationService(accommodationReservationRepository, accommodationRepository, userRepository);
            _accommodationService = new AccommodationService(accommodationRepository);
            _userService = new UserService(userRepository);
            _tourService = new TourService(tourRepository, userRepository, touristRepository, tourReservationRepository, tourReviewRepository, voucherRepository);
            //_ordinaryTourRequestRepository = new OrdinaryTourRequestService(ordinaryTourRequestRepository);
        }

        public List<Message> GetAll()
        {
            return _messageRepository.GetAll();
        }

        public Message Save(Message message)
        {
            return _messageRepository.Save(message);
        }

        public void Delete(Message message)
        {
            _messageRepository.Delete(message);
        }

        public Message Update(Message message)
        {
            return _messageRepository.Update(message);
        }

        public void UpdateAndCreateMessages()
        {
            List<Message> messages = GetAll();
            SetAllUserReviewsMessages(messages);
            SetAllAccommodationChangeRequests(messages);
            SetAllAccommodationReservationCanceledMessages(messages);
        }

        private void SetAllUserReviewsMessages(List<Message> messages)
        {
            foreach (var reservation in _accommodationReservationService.GetAll())
            {
                if (!(messages.Any(message => message.RequestId == reservation.Id && message.Type == MessageType.NewReviewNotification)) && reservation.Rating.GuestCleannessRating != 0 && reservation.Rating.OwnerCleannessRating != 0)
                {
                    string content = "You have a new review from " + _userService.GetById(reservation.GuestId).Username + "." +
                                     " He rated your cleanliness with: " + reservation.Rating.GuestCleannessRating + "." +
                                     " He rated your hospitality with: " + reservation.Rating.GuestHospitalityRating + "." +
                                     " And has additional comment: " + reservation.Rating.GuestComment;

                    int recieverId = _accommodationService.GetById(reservation.AccommodationId).OwnerId;

                    Message message = new Message(0, reservation.Id, "System", recieverId, "You have a new review", content, MessageType.NewReviewNotification, false);
                    Save(message);
                }
            }
        }

        private void SetAllAccommodationChangeRequests(List<Message> messages)
        {
            foreach (var request in _accommodationReservationChangeRequestService.GetAll())
            {
                if (!(messages.Any(message => message.RequestId == request.Id && message.Type == MessageType.AccommodationChangeRequest)) && request.Status == AccommodationChangeRequestStatus.WaitingForApproval)
                {
                    AccommodationReservation accommodationReservation = _accommodationReservationService.GetById(request.AccommodationReservationId);
                    String content = "You have a request to change date for accommodation " + _accommodationService.GetById(accommodationReservation.AccommodationId).Name + " from " + accommodationReservation.BeginDate + " - " + accommodationReservation.EndDate + " to " +
                                     request.BeginDateNew + " - " + request.EndDateNew;

                    int recieverId = _accommodationService.GetById(accommodationReservation.AccommodationId).OwnerId;

                    Message message = new Message(0, request.Id, _userService.GetById(accommodationReservation.GuestId).Username, recieverId, "Reservaton date change request", content, MessageType.AccommodationChangeRequest, false);
                    Save(message);
                }
            }
        }

        private void SetAllAccommodationReservationCanceledMessages(List<Message> messages)
        {
            foreach (var reservation in _accommodationReservationService.GetAll())
            {
                if (!(messages.Any(message => message.RequestId == reservation.Id && message.Type == MessageType.AccommodationReservationCanceled)) && reservation.Canceled == true)
                {
                    string content = "Your reservation for accommodation " + _accommodationService.GetById(reservation.AccommodationId).Name
                                   + " from " + reservation.BeginDate 
                                   + " - " + reservation.EndDate + " has been canceled.";

                    Message message = new Message(0, reservation.Id, _userService.GetById(reservation.GuestId).Username, _accommodationService.GetById(reservation.AccommodationId).OwnerId, "Reservation has been canceled", content, MessageType.AccommodationReservationCanceled, false);
                    Save(message);
                }
            }
        }

        public List<Message> GetByOwner(int ownerId) 
        {
            List<Message> messages = new List<Message>();

            foreach(Message message in GetAll())
            {
                if(message.RecieverId == ownerId)
                {
                    messages.Add(message);
                }
            }

            return messages;
        }
        public void CreateMessage(Message message, User user)
        {
            foreach (Tour tour in _tourService.GetActiveTours())
            {
                 
                    if(!DoesMessageExists(tour,user))
                    {
                        message.RequestId = tour.Id;
                        message.Sender = "Mile";
                        message.RecieverId = user.Id;
                        message.Header = "Active Tour Info";
                        message.Content = "Your tour is now active. Click for more info.";
                        message.Type = MessageType.TourAttendance;
                        message.IsRead = false;
                        Save(message);
                    }
            }
        }
        public bool DoesMessageExists(Tour tour, User user)
        {
            return GetByOwner(user.Id).Any(oldMessage => oldMessage.RequestId == tour.Id);
        }
       /* public void FindRejectedTourWithSameParameters(Tour tour)
        {
            foreach (OrdinaryTourRequest ordinaryTourRequest in _ordinaryTourRequestRepository.GetAll())
            {
                if (ordinaryTourRequest.Language.Equals(tour.Language) || (ordinaryTourRequest.Place.City == tour.Place.City && ordinaryTourRequest.Place.Country== tour.Place.City))
                {
                    bool sameLanguage = ordinaryTourRequest.Language.Equals(tour.Language);
                    bool sameLocation = ordinaryTourRequest.Place.City == tour.Place.City && ordinaryTourRequest.Place.Country == tour.Place.City;
                    CreateSystemMessage(ordinaryTourRequest.UserId, tour, sameLanguage, sameLocation);
                }
            }
        }
        public void CreateSystemMessage(int userId, Tour tour, bool sameLanguage, bool sameLocation)
        {
            Message message = new Message();
            message.RequestId = tour.Id;
            message.Sender = "System";
            message.RecieverId = userId;
            message.Header = "New tour created";
            message.Type = MessageType.NewCreatedTour;
            message.IsRead = false;
           
            if(sameLanguage && sameLocation)
            {
                message.Content = $"Guide has recently created a new tour with language {tour.Language} and location {tour.Place}. Click for more info.";
            }
            else if (sameLanguage)
            {
                message.Content = $"Guide has recently created a new tour with language {tour.Language}. Click for more info.";
            }
            else
            {
                message.Content = $"Guide has recently created a new tour with  location {tour.Place}. Click for more info.";
            }
            Save(message);
        }*/
    }
}
