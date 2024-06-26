﻿using System;
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
        public DateTime BeginingTime { get; set; }
        public double Duration { get; set; }  
        public List<string> Images { get; set; }
        public string CurrentKeyPoint { get; set; }
        public int CurrentCapacity { get; set; }
        public bool IsActive  { get; set; }
        public bool MadeFromStatistics { get; set; }
        public int TouristsPresent { get; set; }

        public Tour() 
        {
            Place = new Location();
            Images = new List<string>();
        }

        public Tour(int id,int guideId, string name, Location place, string description, Languages language, int maxTouristNumber,DateTime beginingTime ,double duration, List<string> images, string currentKeyPoint, bool isActive, int presentTourists)
        {
            Id = id;
            GuideId = guideId;
            Name = name;
            Place = place;
            Description = description;
            Language = language;
            MaxTouristNumber = maxTouristNumber;
            BeginingTime = beginingTime;
            Duration = duration; 
            Images = images;
            CurrentCapacity = maxTouristNumber;
            CurrentKeyPoint = currentKeyPoint;
            IsActive = isActive;
            TouristsPresent = presentTourists;
            MadeFromStatistics = false;
        }

        public Tour(int id,int guideId, string name, Location place, string description, Languages language, int maxTouristNumber, DateTime beginingTime, double duration, List<string> images, string currentKeyPoint, bool isActive,  int currentCapacity, int presentTourists)
        {
            Id = id;
            GuideId = guideId;
            Name = name;
            Place = place;
            Description = description;
            Language = language;
            MaxTouristNumber = maxTouristNumber;
            BeginingTime = beginingTime;
            Duration = duration;
            Images = images;
            CurrentKeyPoint = currentKeyPoint;
            IsActive = isActive;
            CurrentCapacity = currentCapacity;
            TouristsPresent = presentTourists;
            MadeFromStatistics = false;
        }
        public Tour(int id, int guideId, string name, Location place, string description, Languages language, int maxTouristNumber, DateTime beginingTime, double duration, List<string> images, string currentKeyPoint, bool isActive,  int presentTourists,bool madeFromStatistics)
        {
            Id = id;
            GuideId = guideId;
            Name = name;
            Place = place;
            Description = description;
            Language = language;
            MaxTouristNumber = maxTouristNumber;
            BeginingTime = beginingTime;
            Duration = duration;
            Images = images;
            CurrentKeyPoint = currentKeyPoint;
            IsActive = isActive;
            CurrentCapacity = maxTouristNumber;
            TouristsPresent = presentTourists;
            MadeFromStatistics = madeFromStatistics;
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
                string[] csvValues = { Id.ToString(),GuideId.ToString(), Name, Place.City, Place.Country, Description, Language.ToString(), MaxTouristNumber.ToString(), BeginingTime.ToString(), Duration.ToString(), CurrentCapacity.ToString(), /*KeyPoints.Id.ToString()*/CurrentKeyPoint,IsActive.ToString(),TouristsPresent.ToString(),MadeFromStatistics.ToString(), images };
                return csvValues;
            }
            else
            {
                string[] csvValues = { Id.ToString(), GuideId.ToString(), Name, Place.City, Place.Country, Description, Language.ToString(), MaxTouristNumber.ToString(), BeginingTime.ToString(), Duration.ToString(), CurrentCapacity.ToString(), /*KeyPoints.Id.ToString()*/ CurrentKeyPoint,IsActive.ToString(), TouristsPresent.ToString(), MadeFromStatistics.ToString() };
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
            CurrentKeyPoint = values[11];
            IsActive =Convert.ToBoolean( values[12]);
            TouristsPresent = Convert.ToInt32(values[13]);
            MadeFromStatistics = Convert.ToBoolean(values[14]);
            for (int i = 15; i < values.Length; i++)
            {
               Images.Add(values[i]);
            }
        }
    }
}
