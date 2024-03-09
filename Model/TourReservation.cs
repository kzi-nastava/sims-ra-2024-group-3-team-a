using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace BookingApp.Model
{
    public class TourReservation
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public int TourId { get; set; }
        public List<AnonymousTourist> AnonymousTourists  { get; set; }

        public TourReservation() { }
        public TourReservation(int id, int userId, int tourId)
        {
            Id = id;
            UserId = userId;
            TourId = tourId;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), UserId.ToString(), TourId.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            UserId = Convert.ToInt32(values[1]);
            TourId = Convert.ToInt32(values[2]);


        }
    }
}
