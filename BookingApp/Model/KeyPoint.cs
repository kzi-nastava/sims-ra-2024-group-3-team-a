using BookingApp.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Serializer;
using System.Windows;
using System.Xml.Linq;
using System.Windows.Data;
using BookingApp.Model;

namespace BookingApp.Model
{
    public class KeyPoint : ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TourId { get; set; }
        public KeyPointsType Type { get; set; }
        public Boolean IsCurrent { get; set; }
        public Boolean HasPassed { get; set; }  
        public KeyPoint() { }

        public KeyPoint(int id, string name, int tourId, KeyPointsType type, Boolean isCurrent, Boolean hasPassed)
        {
            Id = id;
            Name = name;
            TourId = tourId;
            Type = type;
            IsCurrent = isCurrent;
            HasPassed = hasPassed;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Name, TourId.ToString(), Type.ToString(), IsCurrent.ToString(), HasPassed.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = values[1];
            TourId = Convert.ToInt32(values[2]);
            Type = (KeyPointsType)Enum.Parse(typeof(KeyPointsType),values[3]);
            IsCurrent = Convert.ToBoolean(values[4]);
            HasPassed = Convert.ToBoolean(values[5]);
        }
    }
}
