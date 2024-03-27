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
        public Review Rating { get; set; }

        public AccommodationReservation()
        { 
            Rating = new Review();
        }

        public AccommodationReservation(int id, int guestId, int accommodationId, DateOnly beginDate, DateOnly endDate, Review rating)
        {
            Id = id;
            GuestId = guestId;
            AccommodationId = accommodationId;
            BeginDate = beginDate;
            EndDate = endDate;
            Rating = rating;
        }

        public string[] ToCSV()
        {

            if (Rating.GuestImages != null)
            {
                string images = string.Join("|", Rating.GuestImages);
                string[] csvValues = { Id.ToString(), GuestId.ToString(), AccommodationId.ToString(), BeginDate.ToString(), EndDate.ToString(), Rating.OwnerCleannessRating.ToString(), Rating.OwnerRulesRespectRating.ToString(), Rating.OwnerComment, Rating.GuestCleannessRating.ToString(), Rating.GuestHospitalityRating.ToString(), Rating.GuestComment, images };
                return csvValues;
            }
            else
            {
                string[] csvValues = { Id.ToString(), GuestId.ToString(), AccommodationId.ToString(), BeginDate.ToString(), EndDate.ToString(), Rating.OwnerCleannessRating.ToString(), Rating.OwnerRulesRespectRating.ToString(), Rating.OwnerComment, Rating.GuestCleannessRating.ToString(), Rating.GuestHospitalityRating.ToString(), Rating.GuestComment };
                return csvValues;
            }
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            GuestId = Convert.ToInt32(values[1]);
            AccommodationId = Convert.ToInt32(values[2]);
            BeginDate = DateOnly.Parse(values[3]);
            EndDate = DateOnly.Parse(values[4]);
            Rating.OwnerCleannessRating = Convert.ToInt32(values[5]);
            Rating.OwnerRulesRespectRating = Convert.ToInt32(values[6]);
            Rating.OwnerComment = values[7];
            Rating.GuestCleannessRating = Convert.ToInt32(values[8]);
            Rating.GuestHospitalityRating = Convert.ToInt32(values[9]);
            Rating.GuestComment = values[10];
            for (int i = 11; i < values.Length; i++)
            {
                Rating.GuestImages.Add(values[i]);
            }
        }
    }
}
