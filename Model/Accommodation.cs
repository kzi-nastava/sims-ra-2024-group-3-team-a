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
        public int OwnerId { get; set; }
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

        public Accommodation(int id, int ownerId, string name, Location place, AccomodationType type, int capacity, int minDaysReservation, int cancellationPeriod, List<string> images)
        {
            Id = id;
            OwnerId = ownerId;
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
                string[] csvValues = { Id.ToString(), OwnerId.ToString(), Name, Place.City, Place.Country, Type.ToString(), Capacity.ToString(), MinDaysReservation.ToString(), CancellationPeriod.ToString(), images };
                return csvValues;
            }
            else
            {
                string[] csvValues = { Id.ToString(), OwnerId.ToString(), Name, Place.City, Place.Country, Type.ToString(), Capacity.ToString(), MinDaysReservation.ToString(), CancellationPeriod.ToString() };
                return csvValues;
            }
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            OwnerId = Convert.ToInt32(values[1]);
            Name = values[2];
            Place.City = values[3];
            Place.Country = values[4];
            Type = (AccomodationType)Enum.Parse(typeof(AccomodationType), values[5]);
            Capacity = Convert.ToInt32(values[6]);
            MinDaysReservation = Convert.ToInt32(values[7]);
            CancellationPeriod = Convert.ToInt32(values[8]);
            for(int i = 9; i < values.Length; i++)
            {
                Images.Add(values[i]);
            }
        }   
    }
}
