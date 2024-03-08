using BookingApp.Model.Enums;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BookingApp.Model
{
    public class GuestRating : ISerializable
    {
        public int Id { get; set; }
        public int AccommodationReservationId { get; set; }
        public int CleannessRating { get; set; }
        public int RulesRespectRating { get; set; }
        public string Comment { get; set; }

        public GuestRating() { }

        public GuestRating(int accommodationReservationId, int cleannessRating, int rulesRespectRating, string comment)
        {
            AccommodationReservationId = accommodationReservationId;
            CleannessRating = cleannessRating;
            RulesRespectRating = rulesRespectRating;
            Comment = comment;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), AccommodationReservationId.ToString(), CleannessRating.ToString(), RulesRespectRating.ToString(), Comment };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            AccommodationReservationId = Convert.ToInt32(values[1]);
            CleannessRating = Convert.ToInt32(values[2]);
            RulesRespectRating = Convert.ToInt32(values[3]);
            Comment = values[4];
        }
    }
}
