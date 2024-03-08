using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Model
{
    public class GuestRating
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

        public string[] toSV()
        {
            string[] csvValues = { Id.ToString(), AccommodationReservationId.ToString(), CleannessRating.ToString(), RulesRespectRating.ToString(), Comment };
            return csvValues;
        }
    }
}
