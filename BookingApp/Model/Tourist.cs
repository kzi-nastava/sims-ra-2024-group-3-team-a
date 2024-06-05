using BookingApp.Model.Enums;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.Model
{
    public class Tourist : ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public string JoiningKeyPoint { get; set; }

        public TourReview Review { get; set; }
        public Tourist() 
        {
            Review = new TourReview();
        }

        public Tourist(string name, string surname, int age) 
        {
            Name = name;
            Surname = surname;
            Age = age;
            JoiningKeyPoint = "";
        }

        public Tourist(int id, string name, string surname, int age, string joiningKeyPoint)
        {
            Id = Id;
            Name = name;
            Surname = surname;
            Age = age;
            JoiningKeyPoint = joiningKeyPoint;
        }
        public Tourist( string name, string surname, int age, string joiningKeyPoint)
        {
            Name = name;
            Surname = surname;
            Age = age;
            JoiningKeyPoint = joiningKeyPoint;
        }

        public Tourist(int id, string name, string surname, int age, string joiningKeyPoint, TourReview review)
        {
            Id = id;
            Name = name;
            Surname = surname;
            Age = age;
            JoiningKeyPoint = joiningKeyPoint;
            if (review != null)
            {
                Review = review;
            }
            else
            {
                Review = new TourReview();
            }
        }

        public string[] ToCSV()
        {
            if (Review != null)
            {
                return ToCSVWithReviews();
            } else
            {
                string[] csvValues = { Id.ToString(), Name, Surname, Age.ToString(), JoiningKeyPoint};
                return csvValues;
            }
            
        }

        private string[] ToCSVWithReviews()
        {
            if (Review.Images != null)
            {
                string images = string.Join("|", Review.Images);
                string[] csvValues = { Id.ToString(), Name, Surname, Age.ToString(), JoiningKeyPoint, Review.Id.ToString(), Review.TourId.ToString(),Review.TouristId.ToString(), Review.GuideLanguageRating.ToString(), Review.GuideKnowledgeRating.ToString(),
            Review.TourEntertainmentRating.ToString(), Review.Comment, Review.IsNotValid.ToString(), images};
                return csvValues;
            }
            else
            {
                string[] csvValues = { Id.ToString(), Name, Surname, Age.ToString(), JoiningKeyPoint, Review.Id.ToString(), Review.TourId.ToString(),Review.TouristId.ToString(), Review.GuideLanguageRating.ToString(), Review.GuideKnowledgeRating.ToString(),
            Review.TourEntertainmentRating.ToString(), Review.Comment, Review.IsNotValid.ToString()};
                return csvValues;
            }
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = values[1];
            Surname = values[2];
            Age = Convert.ToInt32(values[3]);
            JoiningKeyPoint = values[4];
            if (values.Length > 5)
            {
                Review.Id = Convert.ToInt32(values[5]);
                Review.TourId = Convert.ToInt32(values[6]);
                Review.TouristId = Convert.ToInt32(values[7]);
                Review.GuideLanguageRating = Convert.ToInt32(values[8]);
                Review.GuideKnowledgeRating = Convert.ToInt32(values[9]);
                Review.TourEntertainmentRating = Convert.ToInt32(values[10]);
                Review.Comment = values[11];
                Review.IsNotValid = Convert.ToBoolean(values[12]);
                for (int i = 13; i < values.Length; i++)
                {
                    Review.Images.Add(values[i]);
                }
            }
        }

    }
}
