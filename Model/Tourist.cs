using BookingApp.Model.Enums;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Model
{
    public class Tourist
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public bool Flag { get; set; }
        public string JoiningKeyPoint { get; set; }

        public TourReview Review { get; set; }
        public Tourist() { }

        public Tourist(string name, string surname, int age) 
        {
            Name = name;
            Surname = surname;
            Age = age;
            JoiningKeyPoint = "";
        }

        public Tourist(string name, string surname, int age, string joiningKeyPoint)
        {
            Name = name;
            Surname = surname;
            Age = age;
            JoiningKeyPoint = joiningKeyPoint;
        }
    }
}
