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
        public int GuideId { get; set; }
        public string Name { get; set; }
        public Location Place { get; set; }
        public string Description { get; set; }
        public Languages Language { get; set; }
        public int MaxTouristNumber { get; set; }
        public KeyPoints KeyPoints { get; set; }
        public DateTime BeginingTime { get; set; }
        public double Duration { get; set; }  
        public List<string> Images { get; set; }
        public string CurrentKeyPoint { get; set; }
        public int CurrentCapacity { get; set; }
        public bool IsActive  { get; set; }
        public int TouristsPresent { get; set; }

        public Tour() 
        {
            Place = new Location();
            KeyPoints = new KeyPoints();
            Images = new List<string>();
        }

        public Tour(int id,int guideId, string name, Location place, string description, Languages language, int maxTouristNumber, KeyPoints keyPoints,DateTime beginingTime ,double duration, List<string> images, string currentKeyPoint, bool isActive, int presentTourists)
        {
            Id = id;
            GuideId = guideId;
            Name = name;
            Place = place;
            Description = description;
            Language = language;
            MaxTouristNumber = maxTouristNumber;
            KeyPoints = keyPoints;
            BeginingTime = beginingTime;
            Duration = duration; 
            Images = images;
            CurrentCapacity = maxTouristNumber;
            CurrentKeyPoint = currentKeyPoint;
            IsActive = isActive;
            TouristsPresent = presentTourists;
        }

        public Tour(int id,int guideId, string name, Location place, string description, Languages language, int maxTouristNumber, KeyPoints keyPoints, DateTime beginingTime, double duration, List<string> images, string currentKeyPoint, bool isActive,  int currentCapacity, int presentTourists)
        {
            Id = id;
            GuideId = guideId;
            Name = name;
            Place = place;
            Description = description;
            Language = language;
            MaxTouristNumber = maxTouristNumber;
            KeyPoints = keyPoints;
            BeginingTime = beginingTime;
            Duration = duration;
            Images = images;
            CurrentKeyPoint = currentKeyPoint;
            IsActive = isActive;
            CurrentCapacity = currentCapacity;
            TouristsPresent = presentTourists;
        }

        public Tour( string name, Location place, Languages language, int maxTouristNumber, DateTime beginingTime)
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
                string[] csvValues = { Id.ToString(),GuideId.ToString(), Name, Place.City, Place.Country, Description, Language.ToString(), MaxTouristNumber.ToString(), BeginingTime.ToString(), Duration.ToString(), CurrentCapacity.ToString(), KeyPoints.Id.ToString(),CurrentKeyPoint,IsActive.ToString(),TouristsPresent.ToString(), images };
                return csvValues;
            }
            else
            {
                string[] csvValues = { Id.ToString(), GuideId.ToString(), Name, Place.City, Place.Country, Description, Language.ToString(), MaxTouristNumber.ToString(), BeginingTime.ToString(), Duration.ToString(), CurrentCapacity.ToString(), KeyPoints.Id.ToString(), CurrentKeyPoint,IsActive.ToString(), TouristsPresent.ToString() };
                return csvValues;
            }
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            GuideId = Convert.ToInt32(values[1]);
            Name = values[2]; 
            Place.City = values[3];
            Place.Country = values[4];
            Description = values[5];
            Language = (Languages)Enum.Parse(typeof(Languages), values[6]);
            MaxTouristNumber = Convert.ToInt32(values[7]);
            BeginingTime = DateTime.Parse(values[8]);
            Duration = double.Parse(values[9]);
            CurrentCapacity = Convert.ToInt32(values[10]);
            KeyPoints.Id = Convert.ToInt32(values[11]);
            CurrentKeyPoint = values[12];
            IsActive =Convert.ToBoolean( values[13]);
            TouristsPresent = Convert.ToInt32(values[14]);
            for (int i = 15; i < values.Length; i++)
            {
               Images.Add(values[i]);
            }
        }
    }
}
