using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Model;

namespace BookingApp.DTO
{
    public class AccommodationReservationDTO : INotifyPropertyChanged
    {
        public AccommodationReservationDTO()
        {
            ratingDTO = new GuestRatingDTO();
        }

        public AccommodationReservationDTO(AccommodationReservation reservation) 
        {
            id = reservation.Id;
            accommodationId = reservation.AccommodationId;
            guestId = reservation.GuestId;
            beginDate = reservation.BeginDate;
            endDate = reservation.EndDate;
            anonymousGuests = reservation.AnonymousGuests;
            ratingDTO = new GuestRatingDTO(reservation.Rating);
        }

        public AccommodationReservationDTO(AccommodationReservationDTO reservation)
        {
            id = reservation.Id;
            accommodationId = reservation.AccommodationId;
            guestId = reservation.GuestId;
            beginDate = reservation.BeginDate;
            endDate = reservation.EndDate;
            anonymousGuests = reservation.AnonymousGuests;
            ratingDTO = new GuestRatingDTO(reservation.RatingDTO);
        }

        private int id;
        public int Id
        {
            get { return id; }
            set
            {
                if (value != id)
                {
                    id = value;
                    OnPropertyChanged();
                }
            }
        }

        private int guestId;
        public int GuestId
        {
            get { return guestId; }
            set
            {
                if (value != guestId)
                {
                    guestId = value;
                    OnPropertyChanged();
                }
            }
        }

        private int accommodationId;
        public int AccommodationId
        {
            get { return accommodationId; }
            set
            {
                if (value != accommodationId)
                {
                    accommodationId = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateOnly beginDate;
        public DateOnly BeginDate
        {
            get { return beginDate; }
            set
            {
                if (value != beginDate)
                {
                    beginDate = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateOnly endDate;
        public DateOnly EndDate
        {
            get { return endDate; }
            set
            {
                if (value != endDate)
                {
                    endDate = value;
                    OnPropertyChanged();
                }
            }
        }

        private GuestRatingDTO ratingDTO;
        public GuestRatingDTO RatingDTO
        {
            get { return ratingDTO; }
            set
            {
                if (value != ratingDTO)
                {
                    ratingDTO = value;
                    OnPropertyChanged();
                }
            }
        }

        private List<AnonymousGuest> anonymousGuests;
        public List<AnonymousGuest> AnonymousGuests
        {
            get { return anonymousGuests; }
            set
            {
                if (value != anonymousGuests)
                {
                    anonymousGuests = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public AccommodationReservation ToAccommodationReservation() 
        {
            return new AccommodationReservation(id, guestId, accommodationId, beginDate, endDate, ratingDTO.ToGuestRating());
        }
    }
}
