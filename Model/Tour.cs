using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BookingApp.Model.Enums;
using BookingApp.Serializer;

namespace BookingApp.Model
{
    public class Tour : ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Location Place { get; set; }
        public string Description { get; set; }
        public Languages Language { get; set; }
        public int MaxTouristNumber { get; set; }
        public KeyPoints KeyPoint { get; set; }
        public DateTime BeginingTime { get; set; }
        public TimeSpan Duration { get; set; }  
        public List<string> Images { get; set; }
        public string CurrentKeyPoint { get; set; }

        public bool IsActive { get; set; }
        public Tour() 
        {
            Place = new Location();
            KeyPoint = new KeyPoints();
        }
        public Tour(string name, Location place, string description, Languages language, int maxTouristNumber, KeyPoints keyPoints,DateTime beginingTime ,TimeSpan duration, List<string> images, string currentKeyPoint, bool isActive)
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
            CurrentKeyPoint = currentKeyPoint;
            IsActive = isActive;
        }

        public Tour( string name, Location place, Languages language, int maxTouristNumber, DateTime beginingTime)
        {
            Name = name;
            Place = place;
            Language = language;
            MaxTouristNumber = maxTouristNumber;
            BeginingTime = beginingTime;
        }
        public string[] ToCSV()
        {
            if (Images != null)
            {
                string images = string.Join("|", Images);
                string[] csvValues = { Id.ToString(), Name, Place.City, Place.Country, Description, Language.ToString(), MaxTouristNumber.ToString(), BeginingTime.ToString(), Duration.ToString(), KeyPoint.Id.ToString(),CurrentKeyPoint, images };
                return csvValues;
            }
            else
            {
                string[] csvValues = { Id.ToString(), Name, Place.City, Place.Country, Description, Language.ToString(), MaxTouristNumber.ToString(), BeginingTime.ToString(), Duration.ToString(), KeyPoint.Id.ToString(), CurrentKeyPoint };
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
            Language = (Languages)Enum.Parse(typeof(Languages), values[5]);
            MaxTouristNumber = Convert.ToInt32(values[6]);
            BeginingTime = DateTime.Parse(values[7]);
            Duration = TimeSpan.Parse(values[8]);
            KeyPoint.Id = Convert.ToInt32(values[9]);
            CurrentKeyPoint = values[10];
            if (Images != null)
            {
                for (int i = 11; i < values.Length; i++)
                {
                     Images.Add(values[i]);
                }
            }
        }

    
    }

}
