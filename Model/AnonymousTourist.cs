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
    public class AnonymousTourist:ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public bool Flag { get; set; }

        public AnonymousTourist() { }
        public AnonymousTourist( string name, string surname, int age) 
        {
            Name = name;
            Surname = surname;
            Age = age;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Name, Surname, Age.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        { 
            Name = values[0];
            Surname = values[1];
            Age = Convert.ToInt32(values[2]);
    
        }
    }
}
