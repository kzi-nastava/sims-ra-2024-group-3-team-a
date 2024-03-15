using BookingApp.Model.Enums;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.Model 
{
    public class Accommodation : ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Location Place {  get; set; }
        public AccomodationType Type { get; set; }
        public int Capacity { get; set; }
        public int MinDaysReservation { get; set; }
        public int CancellationPeriod { get; set; }
        public List<string> Images { get; set; }

        public Accommodation()
        { 
            Place = new Location();
            Images = new List<string>();
        }

        public Accommodation(string name, Location place, AccomodationType type, int capacity, int minDaysReservation, int cancellationPeriod, List<string> images)
        {
            Name = name;
            Place = place;
            Type = type;
            Capacity = capacity;
            MinDaysReservation = minDaysReservation;
            CancellationPeriod = cancellationPeriod;
            Images = images;
        }

        public string[] ToCSV()
        {
            if (Images != null)
            {
                string images = string.Join("|", Images);
                string[] csvValues = { Id.ToString(), Name, Place.City, Place.Country, Type.ToString(), Capacity.ToString(), MinDaysReservation.ToString(), CancellationPeriod.ToString(), images };
                return csvValues;
            }
            else
            {
                string[] csvValues = { Id.ToString(), Name, Place.City, Place.Country, Type.ToString(), Capacity.ToString(), MinDaysReservation.ToString(), CancellationPeriod.ToString() };
                return csvValues;
            }
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = values[1];
            Place.City = values[2];
            Place.Country = values[3];
            Type = (AccomodationType)Enum.Parse(typeof(AccomodationType), values[4]);
            Capacity = Convert.ToInt32(values[5]);
            MinDaysReservation = Convert.ToInt32(values[6]);
            CancellationPeriod = Convert.ToInt32(values[7]);
            for(int i = 8; i < values.Length; i++)
            {
                Images.Add(values[i]);
            }
        }   
    }
}
