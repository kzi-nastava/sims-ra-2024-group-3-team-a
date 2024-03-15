using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BookingApp.Serializer;

namespace BookingApp.Model
{
    public class Tour : ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Location Place { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public int MaxTouristNumber { get; set; }
        public KeyPoints KeyPoint { get; set; }
        public DateTime BeginingTime { get; set; }
        public TimeSpan Duration { get; set; }  
        public List<string> Images { get; set; }
        public int CurrentCapacity { get; set; }

        public Tour() 
        {
            Place = new Location();
            KeyPoint = new KeyPoints();
        }
        public Tour(string name, Location place, string description, string language, int maxTouristNumber, KeyPoints keyPoints,DateTime beginingTime ,TimeSpan duration, List<string> images)
        {
            Name = name;
            Place = place;
            Description = description;
            Language = language;
            MaxTouristNumber = maxTouristNumber;
            KeyPoint = keyPoints;
            BeginingTime = beginingTime;
            Duration = duration; 
            Images = images;
            CurrentCapacity = maxTouristNumber;
        }
        public Tour(int id,string name, Location place, string description, string language, int maxTouristNumber, KeyPoints keyPoints, DateTime beginingTime, TimeSpan duration, List<string> images, int currentCapacity)
        {
            Id = id;
            Name = name;
            Place = place;
            Description = description;
            Language = language;
            MaxTouristNumber = maxTouristNumber;
            KeyPoint = keyPoints;
            BeginingTime = beginingTime;
            Duration = duration;
            Images = images;
            CurrentCapacity = currentCapacity;
        }

        public Tour( string name, Location place, string language, int maxTouristNumber, DateTime beginingTime)
        {
            Name = name;
            Place = place;
            Language = language;
            MaxTouristNumber = maxTouristNumber;
            CurrentCapacity = maxTouristNumber;
        }
     
        public string[] ToCSV()
        {
            if (Images != null)
            {
                string images = string.Join("|", Images);
                string[] csvValues = { Id.ToString(), Name, Place.City, Place.Country, Description, Language, MaxTouristNumber.ToString(), BeginingTime.ToString(), Duration.ToString(), CurrentCapacity.ToString(), KeyPoint.Id.ToString(),images};
                return csvValues;
            }
            else
            {
                string[] csvValues = { Id.ToString(), Name, Place.City, Place.Country, Description, Language, MaxTouristNumber.ToString(), BeginingTime.ToString(), Duration.ToString(), CurrentCapacity.ToString(), KeyPoint.Id.ToString() };
                return csvValues;
            }
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = values[1]; 
            Place.City = values[2];
            Place.Country = values[3];
            Description = values[4];
            Language = values[5];
            MaxTouristNumber = Convert.ToInt32(values[6]);
            BeginingTime = DateTime.Parse(values[7]);
            Duration = TimeSpan.Parse(values[8]);
            CurrentCapacity = Convert.ToInt32(values[9]);
            KeyPoint.Id = Convert.ToInt32(values[10]);
            if (Images != null)
            {
                for (int i = 1; i < values.Length; i++)
                {
                     Images.Add(values[i]);
                }
            }
        }

        /*public Tour(string name,Location location , string description, string language, int maxTouristNumber,KeyPoints keypoint, DateTime beginingTime, TimeSpan duration, List<string> images)
        {
            Name = name;
            Place = location;
            Description = description;
            Language = language;
            MaxTouristNumber = maxTouristNumber;
            KeyPoint= keypoint;
            BeginingTime = beginingTime;
            Duration = duration;
            Images = images;
        }*/
    }

}
