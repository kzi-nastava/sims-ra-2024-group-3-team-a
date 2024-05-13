using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Model
{
    public class AccommodationRenovation : ISerializable
    {
        public int Id { get; set; }
        public int AccommodationId { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }

        public AccommodationRenovation() { }

        public AccommodationRenovation(int id, int accommodationId, DateTime beginDate, DateTime endDate, string description)
        {
            Id = id;
            AccommodationId = accommodationId;
            BeginDate = beginDate;
            EndDate = endDate;
            Description = description;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), AccommodationId.ToString(), BeginDate.ToString(), EndDate.ToString(), Description };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            AccommodationId = Convert.ToInt32(values[1]);
            BeginDate = DateTime.Parse(values[2]);
            EndDate = DateTime.Parse(values[3]);
            Description = values[4];
        }
    }
}
