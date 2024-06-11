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
        public int GuestNumber { get; set; }
        public DateOnly BeginDate { get; set; }
        public DateOnly EndDate { get; set;}
        public bool Canceled { get; set; }
        public bool UsedPoint {  get; set; }
        public Review Rating { get; set; }

        public AccommodationReservation()
        { 
            Rating = new Review();
        }
        public AccommodationReservation(int id, int guestId, int accommodationId, int guestNumber, DateOnly beginDate, DateOnly endDate, bool canceled, bool usedPoint, Review rating)
        {
            Id = id;
            GuestId = guestId;
            AccommodationId = accommodationId;
            GuestNumber = guestNumber;
            BeginDate = beginDate;
            EndDate = endDate;
            Rating = rating;
            UsedPoint = usedPoint;
            Canceled = canceled;
        }
        public AccommodationReservation(int id, int guestId, int accommodationId, DateOnly beginDate, DateOnly endDate, bool canceled,bool usedPoint, Review rating)
        {
            Id = id;
            GuestId = guestId;
            AccommodationId = accommodationId;
            BeginDate = beginDate;
            EndDate = endDate;
            Rating = rating;
            UsedPoint = usedPoint;
            Canceled = canceled;
        }

        public string[] ToCSV()
        {

            if (Rating.GuestImages != null)
            {
                string images = string.Join("|", Rating.GuestImages);
                string[] csvValues = { Id.ToString(), GuestId.ToString(), AccommodationId.ToString(), GuestNumber.ToString() ,BeginDate.ToString(), EndDate.ToString(), Canceled.ToString(), Rating.OwnerCleannessRating.ToString(), Rating.OwnerRulesRespectRating.ToString(), Rating.OwnerComment, Rating.GuestCleannessRating.ToString(), Rating.GuestHospitalityRating.ToString(), Rating.GuestComment, Rating.GuestRenovationRating.ToString(), Rating.GuestRenovationComment, UsedPoint.ToString(),images };
                return csvValues;
            }
            else
            {
                string[] csvValues = { Id.ToString(), GuestId.ToString(), AccommodationId.ToString(), GuestNumber.ToString(), BeginDate.ToString(), EndDate.ToString(), Canceled.ToString(), Rating.OwnerCleannessRating.ToString(), Rating.OwnerRulesRespectRating.ToString(), Rating.OwnerComment, Rating.GuestCleannessRating.ToString(), Rating.GuestHospitalityRating.ToString(), Rating.GuestComment, Rating.GuestRenovationRating.ToString(), Rating.GuestRenovationComment, UsedPoint.ToString() };
                return csvValues;
            }
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            GuestId = Convert.ToInt32(values[1]);
            AccommodationId = Convert.ToInt32(values[2]);
            GuestNumber = Convert.ToInt32(values[3]);
            BeginDate = DateOnly.Parse(values[4]);
            EndDate = DateOnly.Parse(values[5]);
            Canceled = bool.Parse(values[6]);
            Rating.OwnerCleannessRating = Convert.ToInt32(values[7]);
            Rating.OwnerRulesRespectRating = Convert.ToInt32(values[8]);
            Rating.OwnerComment = values[9];
            Rating.GuestCleannessRating = Convert.ToInt32(values[10]);
            Rating.GuestHospitalityRating = Convert.ToInt32(values[11]);
            Rating.GuestComment = values[12];
            Rating.GuestRenovationRating = Convert.ToInt32(values[13]);
            Rating.GuestRenovationComment = values[14];
            UsedPoint = bool.Parse(values[15]);
            for (int i = 16; i < values.Length; i++)
            {
                Rating.GuestImages.Add(values[i]);
            }
        }
    }
}
