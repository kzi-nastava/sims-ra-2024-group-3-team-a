using BookingApp.Model.Enums;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace BookingApp.Model
{
    public class TourReview : ISerializable
    {
        public int Id { get; set; }

        public int TouristId { get; set; }

        public int TourId { get; set; }

        public int GuideKnowledgeRating { get; set; }

        public int GuideLanguageRating { get; set; }

        public int TourEntertainmentRating { get; set; }

        public string Comment { get; set; }

        public  List<string> Images { get; set; }

        public Boolean IsValid { get; set; }


        public TourReview()
        {
            Images = new List<string>();
        }

        public TourReview(int id, int touristId, int tourId, int guideKnowledgeRating, int guideLanguageRating, int tourEntertainmentRating, string comment,bool isValid, List<string> images)
        {
            Id = id;
            TouristId = touristId;
            TourId = tourId;
            GuideKnowledgeRating = guideKnowledgeRating;
            GuideLanguageRating = guideLanguageRating;
            TourEntertainmentRating = tourEntertainmentRating;
            Comment = comment;
            Images = images;
            IsValid = isValid;
        }

        public string[] ToCSV()
        {
            if (Images != null)
            {
                string images = string.Join("|", Images);
                string[] csvValues = { Id.ToString(), TourId.ToString(), TouristId.ToString(), GuideKnowledgeRating.ToString(), GuideLanguageRating.ToString(), TourEntertainmentRating.ToString(), Comment, IsValid.ToString(),  images };
                return csvValues;
            }
            else
            {
                string[] csvValues = { Id.ToString(), TourId.ToString(), TouristId.ToString(), GuideKnowledgeRating.ToString(), GuideLanguageRating.ToString(),TourEntertainmentRating.ToString(), Comment, IsValid.ToString() };
                return csvValues;
            }
        }
        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            TouristId = Convert.ToInt32(values[1]);
            TourId = Convert.ToInt32(values[2]);
            GuideKnowledgeRating = Convert.ToInt32(values[3]);
            GuideLanguageRating = Convert.ToInt32(values[4]);
            TourEntertainmentRating = Convert.ToInt32(values[5]);
            Comment = values[6];
            IsValid = Convert.ToBoolean(values[7]);

            Images = new List<string>();
            for (int i = 8; i < values.Length; i++)
            {
                Images.Add(values[i]);
            }
        }
    }
}
