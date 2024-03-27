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
    public class Review
    {
        public int OwnerCleannessRating { get; set; }
        public int OwnerRulesRespectRating { get; set; }
        public string OwnerComment { get; set; }
        public int GuestCleannessRating { get; set; }
        public int GuestHospitalityRating { get; set; }
        public string GuestComment { get; set; }
        public List<string> GuestImages { get; set; }

        public Review() { }

        public Review(int cleannessRating, int rulesRespectRating, string comment, int guestCleannessRating, int guestHospitalityRating, string guestComment, List<string> guestImages)
        {
            OwnerCleannessRating = cleannessRating;
            OwnerRulesRespectRating = rulesRespectRating;
            OwnerComment = comment;
            GuestCleannessRating = guestCleannessRating;
            GuestHospitalityRating = guestHospitalityRating;
            GuestComment = guestComment;
            GuestImages = guestImages;
        }
    }
}
