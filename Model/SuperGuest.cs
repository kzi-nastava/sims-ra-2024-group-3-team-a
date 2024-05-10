using BookingApp.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Serializer;

namespace BookingApp.Model
{
    public class SuperGuest: ISerializable
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int Points { get; set; }
        public DateOnly EndingDate {  get; set; }
        public SuperGuest() { }
        public SuperGuest(int id, int userId, int points, DateOnly endingDate)
        {
            Id = id;
            UserId = userId;
            Points = points;
            EndingDate = endingDate;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), UserId.ToString(), Points.ToString(), EndingDate.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            UserId = Convert.ToInt32(values[1]);
            Points = Convert.ToInt32(values[2]);
            EndingDate = DateOnly.Parse(values[3]);
        }
    }
}
