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
    public class TourReservation: ISerializable
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; }
        public int TourId { get; set; }
        public List<AnonymousTourist> AnonymousTourists  { get; set; }

        public int NumberOfTourists { get; set; }

        public TourReservation() 
        {
            AnonymousTourists= new List<AnonymousTourist>();
        }
        public TourReservation(int id, int userId, int tourId, string userName ,List<AnonymousTourist> anonymousTourists, int numberOfTourists)
        {
            Id = id;
            UserId = userId;
            TourId = tourId;
            UserName = userName;
            NumberOfTourists = numberOfTourists;
            AnonymousTourists = anonymousTourists;

        }

        public string[] ToCSV()
        {
            if (AnonymousTourists == null)
            {
                string[] csvValues = { Id.ToString(), UserId.ToString(), TourId.ToString(), UserName };
                return csvValues;
            }
            else
            {
                string tourists = string.Empty;
                foreach (var tourist in AnonymousTourists)
                {
                    tourists = tourist.Name + '|' + tourist.Surname + '|' + tourist.Age.ToString();
                }
                string[] csvValues = { Id.ToString(), UserId.ToString(), TourId.ToString(), UserName, tourists };
                return csvValues;

            }

        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            UserId = Convert.ToInt32(values[1]);
            TourId = Convert.ToInt32(values[2]);
            UserName = Convert.ToString(values[3]);
            for (int i = 4;i<values.Length;i=i+3) 
            {
                AnonymousTourists.Add(new AnonymousTourist(values[i], values[i+1], Convert.ToInt32(values[i+2])));
            }

        }
    }
}
