using BookingApp.Model.Enums;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Linq;

namespace BookingApp.Model
{
    public class AccommodationReservationChangeRequest : ISerializable
    {
        public int Id { get; set; }
        public int AccommodationReservationId { get; set; }
        public DateOnly BeginDateNew { get; set; }
        public DateOnly EndDateNew { get; set; }
        public AccommodationChangeRequestStatus Status { get; set; }
        public string RejectedMessage { get; set; }

        public AccommodationReservationChangeRequest() { }

        public AccommodationReservationChangeRequest(int id, int accommodationReservationId, DateOnly beginDateNew,  DateOnly endDateNew, AccommodationChangeRequestStatus status, string rejectedMessage)
        {
            Id = id;
            AccommodationReservationId = accommodationReservationId;
            BeginDateNew = beginDateNew;
            EndDateNew = endDateNew;
            Status = status;
            RejectedMessage = rejectedMessage;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), AccommodationReservationId.ToString(), BeginDateNew.ToString(), EndDateNew.ToString(), Status.ToString(), RejectedMessage };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            AccommodationReservationId = Convert.ToInt32(values[1]);
            BeginDateNew = DateOnly.Parse(values[2]);
            EndDateNew = DateOnly.Parse(values[3]);
            Status = (AccommodationChangeRequestStatus)Enum.Parse(typeof(AccommodationChangeRequestStatus), values[4]);
            RejectedMessage = values[5];
        }
    }
}
