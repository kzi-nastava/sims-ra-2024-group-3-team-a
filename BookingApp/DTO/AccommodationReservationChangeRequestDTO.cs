using BookingApp.Model;
using BookingApp.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.DTO
{
    public class AccommodationReservationChangeRequestDTO : INotifyPropertyChanged
    {
        public AccommodationReservationChangeRequestDTO() { }

        public AccommodationReservationChangeRequestDTO(AccommodationReservationChangeRequestDTO accommodationReservationChangeRequestDTO)
        {
            id = accommodationReservationChangeRequestDTO.Id;
            accommodationReservationId = accommodationReservationChangeRequestDTO.AccommodationReservationId;
            beginDateNew = accommodationReservationChangeRequestDTO.BeginDateNew;
            endDateNew = accommodationReservationChangeRequestDTO.EndDateNew;
            status = accommodationReservationChangeRequestDTO.Status;
            rejectedMessage = accommodationReservationChangeRequestDTO.RejectedMessage;
        }

        public AccommodationReservationChangeRequestDTO(AccommodationReservationChangeRequest accommodationReservationChangeRequest)
        {
            id = accommodationReservationChangeRequest.Id;
            accommodationReservationId = accommodationReservationChangeRequest.AccommodationReservationId;
            beginDateNew = accommodationReservationChangeRequest.BeginDateNew;
            endDateNew = accommodationReservationChangeRequest.EndDateNew;
            status = accommodationReservationChangeRequest.Status;
            rejectedMessage = accommodationReservationChangeRequest.RejectedMessage;
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

        private int accommodationReservationId;
        public int AccommodationReservationId
        {
            get { return accommodationReservationId; }
            set
            {
                if (value != accommodationReservationId)
                {
                    accommodationReservationId = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateOnly beginDateNew;
        public DateOnly BeginDateNew
        {
            get { return beginDateNew; }
            set
            {
                if (value != beginDateNew)
                {
                    beginDateNew = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateOnly endDateNew;
        public DateOnly EndDateNew
        {
            get { return endDateNew; }
            set
            {
                if (value != endDateNew)
                {
                    endDateNew = value;
                    OnPropertyChanged();
                }
            }
        }

        private AccommodationChangeRequestStatus status;
        public AccommodationChangeRequestStatus Status
        {
            get { return status; }
            set
            {
                if (value != status)
                {
                    status = value;
                    OnPropertyChanged();
                }
            }
        }

        private string rejectedMessage;
        public string RejectedMessage
        {
            get { return rejectedMessage; }
            set
            {
                if (value != rejectedMessage)
                {
                    rejectedMessage = value;
                    OnPropertyChanged();
                }
            }
        }

        public AccommodationReservationChangeRequest ToAccommodationReservationChangeRequest()
        {
            return new AccommodationReservationChangeRequest(id, accommodationReservationId, beginDateNew, endDateNew, status, rejectedMessage);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
