using BookingApp.Model.Enums;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BookingApp.Model
{
    public class Forum : ISerializable
    {
        public int Id { get; set; }
        public Location Location { get; set; }
        public Boolean SeenByOwner { get; set; }
        public Boolean IsVeryImportant { get; set; }
        public Boolean IsClosed { get; set; }

        public Forum()
        {
            Location = new Location();
        }

        public Forum(int id, Location location, Boolean seenByOwner, Boolean isVeryImportant, Boolean isClosed)
        {
            Id = id;
            Location = location;
            SeenByOwner = seenByOwner;
            IsVeryImportant = isVeryImportant;
            IsClosed = isClosed;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Location.City, Location.Country, SeenByOwner.ToString(), IsVeryImportant.ToString(), IsClosed.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Location.City = values[1];
            Location.Country = values[2];
            SeenByOwner = Convert.ToBoolean(values[3]);
            IsVeryImportant = Convert.ToBoolean(values[4]);
        }
    }
}
