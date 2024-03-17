using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace BookingApp.Model
{
    public class TourReservation : ISerializable
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int TourId { get; set; }
        public List<Tourist> Tourists  { get; set; }
        public int NumberOfTourists { get; set; }

        public TourReservation() 
        {
            Tourists = new List<Tourist>();
        }

        public TourReservation(int id, int userId, int tourId, string userName, List<Tourist> tourists, int numberOfTourists)
        {
            Id = id;
            UserId = userId;
            TourId = tourId;
            UserName = userName;
            Tourists = tourists;
            NumberOfTourists = numberOfTourists; 
        }

        public string[] ToCSV()
        {
            if (Tourists == null)
            {
                string[] csvValues = { Id.ToString(), UserId.ToString(), TourId.ToString() };
                return csvValues;
            }
            else
            {
                string tourists = CreateTouristsString();
                string[] csvValues = { Id.ToString(), UserId.ToString(), TourId.ToString(), tourists };
                return csvValues;
            }
        }
        private string CreateTouristsString()
        {
            string tourists = string.Empty;
            foreach (var tourist in Tourists)
            {
                tourists += tourist.Name + '|' + tourist.Surname + '|' + tourist.Age.ToString() + '|';
            }

            tourists = tourists.Substring(0, tourists.Length - 1);
            return tourists;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            UserId = Convert.ToInt32(values[1]);
            TourId = Convert.ToInt32(values[2]);
            for (int i = 3;i<values.Length;i=i+3) 
            {
                if(i+2<values.Length)
                {
                    Tourists.Add(new Tourist(values[i], values[i + 1], Convert.ToInt32(values[i + 2])));
                }
            }
        }
    }
}
