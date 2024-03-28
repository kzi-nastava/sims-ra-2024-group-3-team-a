using BookingApp.Model;
using BookingApp.Model.Enums;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml;

namespace BookingApp.Repository
{
    public class MessageRepository
    {
        private const string FilePath = "../../../Resources/Data/messages.csv";

        private readonly Serializer<Message> _serializer;

        private List<Message> _messages;

        private AccommodationReservationRepository _accommodationReservationRepository;
        private UserRepository _userRepository;
        private AccommodationReservationChangeRequestRepository _accommodationReservationChangeRequestRepository;
        private AccommodationRepository _accommodationRepository;   

        public MessageRepository()
        {
            _serializer = new Serializer<Message>();
            _messages = _serializer.FromCSV(FilePath);

            _accommodationReservationRepository = new AccommodationReservationRepository();
            _userRepository = new UserRepository();
            _accommodationReservationChangeRequestRepository = new AccommodationReservationChangeRequestRepository();
            _accommodationRepository = new AccommodationRepository();
        }

        public List<Message> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public Message Save(Message message)
        {
            message.Id = NextId();
            _messages = _serializer.FromCSV(FilePath);
            _messages.Add(message);
            _serializer.ToCSV(FilePath, _messages);
            return message;
        }

        public int NextId()
        {
            _messages = _serializer.FromCSV(FilePath);
            if (_messages.Count < 1)
            {
                return 1;
            }
            return _messages.Max(c => c.Id) + 1;
        }

        public void Delete(Message message)
        {
            _messages = _serializer.FromCSV(FilePath);
            Message founded = _messages.Find(c => c.Id == message.Id);
            _messages.Remove(founded);
            _serializer.ToCSV(FilePath, _messages);
        }

        public Message Update(Message message)
        {
            _messages = _serializer.FromCSV(FilePath);
            Message current = _messages.Find(c => c.Id == message.Id);
            current.RequestId = message.RequestId;
            current.Sender = message.Sender;
            current.Header = message.Header;
            current.Content = message.Content;
            current.Type = message.Type;
            current.IsRead = message.IsRead;
            _serializer.ToCSV(FilePath, _messages);
            return current;
        }

        public void SetMessages()
        {
            _messages = _serializer.FromCSV(FilePath);
            SetAllUserReviewsMessages();
            SetAllAccommodationChangeRequests();
        }

        private void SetAllUserReviewsMessages()
        {
            foreach (var reservation in _accommodationReservationRepository.GetAll())
            {
                if (!_messages.Any(message => message.RequestId == reservation.Id) && reservation.Rating.GuestCleannessRating != 0 && reservation.Rating.OwnerCleannessRating != 0)
                {
                    string content = "You have a new review from " + _userRepository.GetById(reservation.GuestId).Username + "." +
                                     " He rated your cleanliness with: " + reservation.Rating.GuestCleannessRating + "." +
                                     " He rated your hospitality with: " + reservation.Rating.GuestHospitalityRating + "." +
                                     " And has additional comment: " + reservation.Rating.GuestComment;

                    Message message = new Message(0, reservation.Id, "System", "You have a new review", content, MessageType.NewReviewNotification, false);
                    Save(message);
                }
            }
        }

        private void SetAllAccommodationChangeRequests()
        {
            foreach (var request in _accommodationReservationChangeRequestRepository.GetAll())
            {
                if (!_messages.Any(message => message.RequestId == request.Id) && request.Status == AccommodationChangeRequestStatus.WaitingForApproval)
                {
                    AccommodationReservation accommodationReservation = _accommodationReservationRepository.GetById(request.AccommodationReservationId);
                    String content = "You have a request to change date for accommodation " + _accommodationRepository.GetById(accommodationReservation.AccommodationId).Name + " from " + accommodationReservation.BeginDate + " - " + accommodationReservation.EndDate + " to " +
                                     request.BeginDateNew + " - " + request.EndDateNew;

                    Message message = new Message(0, request.Id, _userRepository.GetById(accommodationReservation.GuestId).Username, "Reservaton date change request", content, MessageType.AccommodationChangeRequest, false);
                    Save(message);
                }
            }
        }
    }
}
