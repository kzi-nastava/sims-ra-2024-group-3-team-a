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

        public int TouristId { get; set; }
        public int TourId { get; set; }
        public List<AnonymousTourist> AnonymousTourists  { get; set; }

        public TourReservation() { }
        public TourReservation(int id, int touristId, int tourId)
        {
            Id = id;
            TourId = touristId;
            TourId = tourId;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), TouristId.ToString(), TourId.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            TouristId = Convert.ToInt32(values[1]);
            TourId = Convert.ToInt32(values[2]);


        }
    }
}
