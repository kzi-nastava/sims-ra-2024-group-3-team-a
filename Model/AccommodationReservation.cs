using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace BookingApp.Model
{
    public class AccommodationReservation: ISerializable
    {
        public int Id { get; set; }
        public int GuestId { get; set; }
        public int AccommodationId { get; set; }
        public DateOnly BeginDate { get; set; }
        public DateOnly EndDate { get; set;}
        //public List<AnonymousGuest> AnonymousGuests { get; set; }
        public GuestRating Rating { get; set; }

        public AccommodationReservation()
        { 
            Rating = new GuestRating();
            //AnonymousGuests = new List<AnonymousGuest>();
        }

        public AccommodationReservation(int id, int guestId, int accommodationId, DateOnly beginDate, DateOnly endDate, GuestRating rating)
        {
            Id = id;
            GuestId = guestId;
            AccommodationId = accommodationId;
            BeginDate = beginDate;
            EndDate = endDate;
            Rating = rating;
            //AnonymousGuests = anonymousGuests;
        }

        public string[] ToCSV()
        {
            //if (AnonymousGuests == null)
            //{
                string[] csvValues = { Id.ToString(), GuestId.ToString(), AccommodationId.ToString(), BeginDate.ToString(), EndDate.ToString(), Rating.CleannessRating.ToString(), Rating.RulesRespectRating.ToString(), Rating.Comment };
                return csvValues;
            //}
            //else
            /*
            {
                string guests = string.Empty;
                foreach (var guest in AnonymousGuests)
                {
                    guests += guest.Name + '|' + guest.Surname + '|' + guest.Age.ToString() + '|';
                }
                guests = guests.Substring(0, guests.Length - 1);
                string[] csvValues = { Id.ToString(), GuestId.ToString(), AccommodationId.ToString(), BeginDate.ToString(), EndDate.ToString(), Rating.CleannessRating.ToString(), Rating.RulesRespectRating.ToString(), Rating.Comment, guests };
                return csvValues;
            }
            */
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            GuestId = Convert.ToInt32(values[1]);
            AccommodationId = Convert.ToInt32(values[2]);
            BeginDate = DateOnly.Parse(values[3]);
            EndDate = DateOnly.Parse(values[4]);
            Rating.CleannessRating = Convert.ToInt32(values[5]);
            Rating.RulesRespectRating = Convert.ToInt32(values[6]);
            Rating.Comment = values[7];
            /*
            for (int i = 8; i < values.Length; i = i + 3)
            {
                if (i + 2 < values.Length)
                {
                    AnonymousGuests.Add(new AnonymousGuest(values[i], values[i + 1], Convert.ToInt32(values[i + 2])));
                }
            }
            */
        }
    }
}
